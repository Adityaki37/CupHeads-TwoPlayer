Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200073A RID: 1850
Public Class RetroArcadeBouncyBall
	Inherits RetroArcadeEnemy

	' Token: 0x0600284B RID: 10315 RVA: 0x00177E94 File Offset: 0x00176294
	Public Function Create(pos As Vector3, manager As RetroArcadeBouncyManager, properties As LevelProperties.RetroArcade.Bouncy, startAngle As Single) As RetroArcadeBouncyBall
		Dim retroArcadeBouncyBall As RetroArcadeBouncyBall = Me.InstantiatePrefab(Of RetroArcadeBouncyBall)()
		retroArcadeBouncyBall.transform.position = pos
		retroArcadeBouncyBall.startAngle = startAngle
		retroArcadeBouncyBall.properties = properties
		retroArcadeBouncyBall.GetComponent(Of Collider2D)().enabled = False
		Return retroArcadeBouncyBall
	End Function

	' Token: 0x0600284C RID: 10316 RVA: 0x00177ED0 File Offset: 0x001762D0
	Public Sub StartMoving(middlePos As Vector3)
		Me.hp = 1F
		MyBase.transform.parent = Nothing
		MyBase.GetComponent(Of Collider2D)().enabled = True
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x0600284D RID: 10317 RVA: 0x00177F04 File Offset: 0x00176304
	Private Iterator Function move_cr() As IEnumerator
		Me.velocity = MathUtils.AngleToDirection(Me.startAngle)
		While True
			MyBase.transform.position += Me.velocity * Me.properties.groupMoveSpeed * CupheadTime.FixedDelta
			Yield New WaitForFixedUpdate()
		End While
		Return
	End Function

	' Token: 0x0600284E RID: 10318 RVA: 0x00177F20 File Offset: 0x00176320
	Protected Overrides Sub OnCollisionCeiling(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionCeiling(hit, phase)
		Dim vector As Vector3 = Me.velocity
		vector.y = Mathf.Min(vector.y, -vector.y)
		Me.ChangeDir(vector)
	End Sub

	' Token: 0x0600284F RID: 10319 RVA: 0x00177F60 File Offset: 0x00176360
	Protected Overrides Sub OnCollisionGround(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionGround(hit, phase)
		Dim vector As Vector3 = Me.velocity
		vector.y = Mathf.Max(vector.y, -vector.y)
		Me.ChangeDir(vector)
	End Sub

	' Token: 0x06002850 RID: 10320 RVA: 0x00177FA0 File Offset: 0x001763A0
	Protected Sub ChangeDir(newVelocity As Vector3)
		Me.velocity = newVelocity
		Me.currentAngle = Mathf.Atan2(Me.velocity.y, Me.velocity.x) * 57.29578F
		MyBase.transform.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(Me.currentAngle))
	End Sub

	' Token: 0x06002851 RID: 10321 RVA: 0x00178008 File Offset: 0x00176408
	Protected Overrides Sub OnCollisionWalls(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionWalls(hit, phase)
		Dim vector As Vector3 = Me.velocity
		If MyBase.transform.position.x > 0F Then
			vector.x = Mathf.Min(vector.x, -vector.x)
			Me.ChangeDir(vector)
		Else
			vector.x = Mathf.Max(vector.x, -vector.x)
			Me.ChangeDir(vector)
		End If
	End Sub

	' Token: 0x06002852 RID: 10322 RVA: 0x0017808A File Offset: 0x0017648A
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If Not MyBase.IsDead Then
			MyBase.OnCollisionPlayer(hit, phase)
		End If
	End Sub

	' Token: 0x06002853 RID: 10323 RVA: 0x0017809F File Offset: 0x0017649F
	Public Overrides Sub Dead()
		MyBase.Dead()
		MyBase.GetComponent(Of Collider2D)().enabled = True
		MyBase.GetComponent(Of DamageReceiver)().enabled = False
		Global.UnityEngine.[Object].Destroy(MyBase.GetComponent(Of Rigidbody2D)())
	End Sub

	' Token: 0x04003114 RID: 12564
	Private properties As LevelProperties.RetroArcade.Bouncy

	' Token: 0x04003115 RID: 12565
	Private manager As RetroArcadeBouncyManager

	' Token: 0x04003116 RID: 12566
	Private velocity As Vector3

	' Token: 0x04003117 RID: 12567
	Private currentAngle As Single

	' Token: 0x04003118 RID: 12568
	Private startAngle As Single
End Class
