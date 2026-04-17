Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020007BA RID: 1978
Public Class SallyStagePlayLevelUmbrellaProjectile
	Inherits AbstractProjectile

	' Token: 0x06002CB2 RID: 11442 RVA: 0x001A5BCF File Offset: 0x001A3FCF
	Protected Overrides Sub Update()
		Me.damageDealer.Update()
		MyBase.Update()
	End Sub

	' Token: 0x06002CB3 RID: 11443 RVA: 0x001A5BE4 File Offset: 0x001A3FE4
	Public Sub InitProjectile(properties As LevelProperties.SallyStagePlay, direction As Integer)
		Me.properties = properties
		Me.active = False
		Me.direction = direction
		MyBase.transform.SetScale(New Single?(CSng((-CSng(direction)))), Nothing, Nothing)
		Me.currentVelocity = Vector3.down * properties.CurrentState.umbrella.objectSpeed
		MyBase.StartCoroutine(Me.move_cr())
		MyBase.StartCoroutine(Me.check_bounds_cr())
	End Sub

	' Token: 0x06002CB4 RID: 11444 RVA: 0x001A5C68 File Offset: 0x001A4068
	Private Iterator Function move_cr() As IEnumerator
		Dim isFalling As Boolean = False
		While True
			If Me.active Then
				For i As Integer = 0 To 2 - 1
					Dim [next] As AbstractPlayerController = PlayerManager.GetNext()
					If MyBase.transform.position.x >= [next].center.x - 10F AndAlso MyBase.transform.position.x <= [next].center.x + 10F Then
						If Not isFalling Then
							MyBase.animator.SetTrigger("OnFall")
							isFalling = True
						End If
						Me.currentVelocity = Vector3.down * Me.properties.CurrentState.umbrella.objectDropSpeed
					End If
				Next
			End If
			MyBase.transform.position += Me.currentVelocity * CupheadTime.Delta
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06002CB5 RID: 11445 RVA: 0x001A5C84 File Offset: 0x001A4084
	Private Iterator Function check_bounds_cr() As IEnumerator
		Dim offset As Single = 50F
		Dim goingVertically As Boolean = False
		Dim goingUp As Boolean = False
		While True
			If MyBase.transform.position.y >= 360F - offset AndAlso goingVertically Then
				MyBase.transform.position = New Vector3(MyBase.transform.position.x, 360F - offset, 0F)
				Me.currentVelocity = Vector3.left * Me.properties.CurrentState.umbrella.objectSpeed * CSng(Me.direction)
				Me.active = True
				goingVertically = False
			ElseIf MyBase.transform.position.y <= -360F + offset AndAlso goingVertically Then
				Me.currentVelocity = Vector3.right * Me.properties.CurrentState.umbrella.objectSpeed * CSng(Me.direction)
				goingVertically = False
			ElseIf(MyBase.transform.position.x <= -630F AndAlso Not goingVertically) OrElse (MyBase.transform.position.x >= 630F AndAlso Not goingVertically) Then
				If Not goingUp Then
					MyBase.animator.SetTrigger("OnClimb")
					goingUp = True
				End If
				If Not Me.dropped Then
					MyBase.GetComponent(Of SpriteRenderer)().material = Me.change
					Me.dropped = True
				End If
				Me.currentVelocity = Vector3.up * Me.properties.CurrentState.umbrella.objectSpeed
				goingVertically = True
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06002CB6 RID: 11446 RVA: 0x001A5C9F File Offset: 0x001A409F
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
			Me.Die()
		End If
	End Sub

	' Token: 0x06002CB7 RID: 11447 RVA: 0x001A5CC4 File Offset: 0x001A40C4
	Protected Overrides Sub OnCollisionGround(hit As GameObject, phase As CollisionPhase)
		If Me.active Then
			Me.Die()
		ElseIf Not Me.dropped Then
			MyBase.GetComponent(Of SpriteRenderer)().material = Me.change
			MyBase.animator.SetTrigger("OnDrop")
			Me.dropped = True
		End If
		If phase = CollisionPhase.Enter Then
			Me.currentVelocity = Vector3.right * Me.properties.CurrentState.umbrella.objectSpeed * CSng(Me.direction)
		End If
		MyBase.OnCollisionGround(hit, phase)
	End Sub

	' Token: 0x06002CB8 RID: 11448 RVA: 0x001A5D5C File Offset: 0x001A415C
	Protected Overrides Sub Die()
		Me.StopAllCoroutines()
		MyBase.animator.SetTrigger("OnDeath")
		For Each spriteDeathParts As SpriteDeathParts In Me.sprites
			spriteDeathParts.CreatePart(MyBase.transform.position)
		Next
		MyBase.Die()
	End Sub

	' Token: 0x06002CB9 RID: 11449 RVA: 0x001A5DB6 File Offset: 0x001A41B6
	Private Sub Kill()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06002CBA RID: 11450 RVA: 0x001A5DC3 File Offset: 0x001A41C3
	Protected Overrides Sub OnDestroy()
		Me.StopAllCoroutines()
		MyBase.OnDestroy()
		Me.sprites = Nothing
	End Sub

	' Token: 0x04003535 RID: 13621
	Private active As Boolean

	' Token: 0x04003536 RID: 13622
	Private dropped As Boolean

	' Token: 0x04003537 RID: 13623
	Private direction As Integer

	' Token: 0x04003538 RID: 13624
	Private currentVelocity As Vector3

	' Token: 0x04003539 RID: 13625
	Private properties As LevelProperties.SallyStagePlay

	' Token: 0x0400353A RID: 13626
	<SerializeField()>
	Private change As Material

	' Token: 0x0400353B RID: 13627
	<SerializeField()>
	Private sprites As SpriteDeathParts()
End Class
