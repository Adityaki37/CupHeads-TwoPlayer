Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000665 RID: 1637
Public Class FlyingGenieLevelBouncer
	Inherits AbstractProjectile

	' Token: 0x06002216 RID: 8726 RVA: 0x0013D7A0 File Offset: 0x0013BBA0
	Public Function Init(pos As Vector3, properties As LevelProperties.FlyingGenie.Obelisk, angle As Single) As FlyingGenieLevelBouncer
		MyBase.transform.position = pos
		Me.properties = properties
		MyBase.transform.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(angle))
		Me.sprite.transform.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(0F))
		Return Me
	End Function

	' Token: 0x06002217 RID: 8727 RVA: 0x0013D814 File Offset: 0x0013BC14
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x06002218 RID: 8728 RVA: 0x0013D829 File Offset: 0x0013BC29
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06002219 RID: 8729 RVA: 0x0013D847 File Offset: 0x0013BC47
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x0600221A RID: 8730 RVA: 0x0013D868 File Offset: 0x0013BC68
	Protected Overrides Sub OnCollisionWalls(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionWalls(hit, phase)
		If phase = CollisionPhase.Enter Then
			If Vector3.Dot(hit.transform.position, Vector3.right) > 0F Then
				Dim vector As Vector3 = New Vector3(CSng(Level.Current.Left), MyBase.transform.position.y, 0F)
				Dim position As Vector3 = MyBase.transform.position
				Me.collisionPoint = vector - position
				MyBase.StartCoroutine(Me.change_dir_cr(Me.collisionPoint))
			Else
				Dim position2 As Vector3 = MyBase.transform.position
				Dim vector2 As Vector3 = New Vector3(CSng(Level.Current.Right), MyBase.transform.position.y, 0F)
				Me.collisionPoint = vector2 - position2
				MyBase.StartCoroutine(Me.change_dir_cr(Me.collisionPoint))
			End If
		End If
	End Sub

	' Token: 0x0600221B RID: 8731 RVA: 0x0013D958 File Offset: 0x0013BD58
	Protected Overrides Sub OnCollisionCeiling(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionCeiling(hit, phase)
		If phase = CollisionPhase.Enter Then
			Dim position As Vector3 = MyBase.transform.position
			Dim vector As Vector3 = New Vector3(MyBase.transform.position.x, CSng(Level.Current.Ground), 0F)
			Me.collisionPoint = vector - position
			MyBase.StartCoroutine(Me.change_dir_cr(Me.collisionPoint))
		End If
	End Sub

	' Token: 0x0600221C RID: 8732 RVA: 0x0013D9CC File Offset: 0x0013BDCC
	Protected Overrides Sub OnCollisionGround(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionGround(hit, phase)
		If phase = CollisionPhase.Enter Then
			Dim position As Vector3 = MyBase.transform.position
			Dim vector As Vector3 = New Vector3(MyBase.transform.position.x, CSng(Level.Current.Ceiling), 0F)
			Me.collisionPoint = vector - position
			MyBase.StartCoroutine(Me.change_dir_cr(Me.collisionPoint))
		End If
	End Sub

	' Token: 0x0600221D RID: 8733 RVA: 0x0013DA40 File Offset: 0x0013BE40
	Private Iterator Function move_cr() As IEnumerator
		Me.velocity = MyBase.transform.up
		Me.speed = Me.properties.bouncerSpeed
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		While True
			MyBase.transform.position += Me.velocity * Me.speed * CupheadTime.Delta
			Yield wait
		End While
		Return
	End Function

	' Token: 0x0600221E RID: 8734 RVA: 0x0013DA5C File Offset: 0x0013BE5C
	Private Iterator Function change_dir_cr(collisionPoint As Vector3) As IEnumerator
		Me.velocity = 1F * (-2F * Vector3.Dot(Me.velocity, Vector3.Normalize(collisionPoint.normalized)) * Vector3.Normalize(collisionPoint.normalized) + Me.velocity)
		Yield Nothing
		Return
	End Function

	' Token: 0x0600221F RID: 8735 RVA: 0x0013DA80 File Offset: 0x0013BE80
	Private Sub ChangeSpeed()
		If Vector3.Dot(Me.velocity, Vector3.right) > 0F Then
			Dim num As Single = Me.properties.bouncerSpeed - Me.properties.obeliskMovementSpeed
			Me.speed -= num
		Else
			Me.speed = Me.properties.bouncerSpeed
		End If
	End Sub

	' Token: 0x06002220 RID: 8736 RVA: 0x0013DAE3 File Offset: 0x0013BEE3
	Protected Overrides Sub Die()
		MyBase.Die()
	End Sub

	' Token: 0x04002AC3 RID: 10947
	<SerializeField()>
	Private sprite As Transform

	' Token: 0x04002AC4 RID: 10948
	Private properties As LevelProperties.FlyingGenie.Obelisk

	' Token: 0x04002AC5 RID: 10949
	Private velocity As Vector3

	' Token: 0x04002AC6 RID: 10950
	Private average As Vector3

	' Token: 0x04002AC7 RID: 10951
	Private collisionPoint As Vector3

	' Token: 0x04002AC8 RID: 10952
	Private speed As Single
End Class
