Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020004E3 RID: 1251
Public Class BaronessLevelFollowingProjectile
	Inherits AbstractProjectile

	' Token: 0x060015A0 RID: 5536 RVA: 0x000C1D77 File Offset: 0x000C0177
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.isActive = True
	End Sub

	' Token: 0x060015A1 RID: 5537 RVA: 0x000C1D88 File Offset: 0x000C0188
	Public Sub Init(pos As Vector2, target As Vector3, properties As LevelProperties.Baroness.BaronessVonBonbon, player As AbstractPlayerController, parent As BaronessLevelCastle)
		MyBase.transform.position = pos
		Me.properties = properties
		Me.target = target
		Me.player = player
		Me.parent = parent
		AddHandler Me.parent.OnDeathEvent, AddressOf Me.KillProjectile
	End Sub

	' Token: 0x060015A2 RID: 5538 RVA: 0x000C1DDB File Offset: 0x000C01DB
	Private Sub KillProjectile()
		Me.isActive = False
	End Sub

	' Token: 0x060015A3 RID: 5539 RVA: 0x000C1DE4 File Offset: 0x000C01E4
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x060015A4 RID: 5540 RVA: 0x000C1DF9 File Offset: 0x000C01F9
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
		If Not Me.isActive Then
			Me.Die()
		End If
	End Sub

	' Token: 0x060015A5 RID: 5541 RVA: 0x000C1E28 File Offset: 0x000C0228
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x060015A6 RID: 5542 RVA: 0x000C1E48 File Offset: 0x000C0248
	Private Iterator Function move_cr() As IEnumerator
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		Dim count As Single = 0F
		While True
			Dim start As Vector2 = MyBase.transform.position
			Me.target = Me.player.transform.position
			Dim followTime As Single = Me.properties.finalProjectileMoveDuration
			Dim t As Single = 0F
			While t < followTime
				MyBase.transform.position = Vector3.MoveTowards(MyBase.transform.position, Me.target, Me.properties.finalProjectileSpeed * CupheadTime.FixedDelta)
				t += CupheadTime.FixedDelta
				Yield wait
			End While
			Me.player = PlayerManager.GetNext()
			count += 1F
			If count > Me.properties.finalProjectileRedirectCount Then
				Exit For
			End If
			Yield CupheadTime.WaitForSeconds(Me, Me.properties.finalProjectileRedirectDelay)
		End While
		If Me.player Is Nothing OrElse Me.player.IsDead Then
			Me.player = PlayerManager.GetNext()
		End If
		Dim dir As Vector3 = Me.player.transform.position - MyBase.transform.position
		While True
			MyBase.transform.position += dir.normalized * Me.properties.finalProjectileSpeed * CupheadTime.FixedDelta
			Yield wait
		End While
		Return
	End Function

	' Token: 0x060015A7 RID: 5543 RVA: 0x000C1E63 File Offset: 0x000C0263
	Protected Overrides Sub Die()
		MyBase.GetComponent(Of SpriteRenderer)().enabled = False
		MyBase.GetComponent(Of Collider2D)().enabled = False
		MyBase.Die()
	End Sub

	' Token: 0x04001EFE RID: 7934
	Public properties As LevelProperties.Baroness.BaronessVonBonbon

	' Token: 0x04001EFF RID: 7935
	Private player As AbstractPlayerController

	' Token: 0x04001F00 RID: 7936
	Private target As Vector3

	' Token: 0x04001F01 RID: 7937
	Private parent As BaronessLevelCastle

	' Token: 0x04001F02 RID: 7938
	Private timesUp As Boolean

	' Token: 0x04001F03 RID: 7939
	Private isActive As Boolean
End Class
