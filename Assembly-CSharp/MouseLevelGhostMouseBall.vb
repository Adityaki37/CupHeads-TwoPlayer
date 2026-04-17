Imports System
Imports UnityEngine

' Token: 0x020006F1 RID: 1777
Public Class MouseLevelGhostMouseBall
	Inherits AbstractProjectile

	' Token: 0x06002618 RID: 9752 RVA: 0x0016435C File Offset: 0x0016275C
	Public Function Create(pos As Vector2, speed As Single, childSpeed As Single) As MouseLevelGhostMouseBall
		Dim mouseLevelGhostMouseBall As MouseLevelGhostMouseBall = Me.InstantiatePrefab(Of MouseLevelGhostMouseBall)()
		Dim vector As Vector2 = New Vector2(PlayerManager.GetNext().transform.position.x, CSng(Level.Current.Ground))
		Dim normalized As Vector2 = (vector - pos).normalized
		mouseLevelGhostMouseBall.transform.position = pos
		mouseLevelGhostMouseBall.velocity = speed * normalized
		mouseLevelGhostMouseBall.childSpeed = childSpeed
		mouseLevelGhostMouseBall.state = MouseLevelGhostMouseBall.State.Moving
		mouseLevelGhostMouseBall.transform.Rotate(0F, 0F, MathUtils.DirectionToAngle(normalized) - 90F)
		Return mouseLevelGhostMouseBall
	End Function

	' Token: 0x06002619 RID: 9753 RVA: 0x001643F8 File Offset: 0x001627F8
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.state = MouseLevelGhostMouseBall.State.Moving Then
			If MyBase.transform.position.y < CSng(Level.Current.Ground) Then
				Me.Explode()
				Return
			End If
			MyBase.transform.AddPosition(Me.velocity.x * CupheadTime.Delta, Me.velocity.y * CupheadTime.Delta, 0F)
		End If
	End Sub

	' Token: 0x0600261A RID: 9754 RVA: 0x0016447D File Offset: 0x0016287D
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If Me.damageDealer IsNot Nothing AndAlso phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x0600261B RID: 9755 RVA: 0x001644A8 File Offset: 0x001628A8
	Private Sub Explode()
		Me.state = MouseLevelGhostMouseBall.State.Dead
		Me.childProjectile.Create(MyBase.transform.position, 0F, Vector2.one, Me.childSpeed)
		Me.childProjectile.Create(MyBase.transform.position, 0F, New Vector2(1F, -1F), -Me.childSpeed)
		Me.Die()
	End Sub

	' Token: 0x0600261C RID: 9756 RVA: 0x00164525 File Offset: 0x00162925
	Protected Overrides Sub Die()
		MyBase.Die()
		MyBase.transform.SetLocalEulerAngles(New Single?(0F), New Single?(0F), New Single?(0F))
	End Sub

	' Token: 0x0600261D RID: 9757 RVA: 0x00164556 File Offset: 0x00162956
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.childProjectile = Nothing
	End Sub

	' Token: 0x04002E98 RID: 11928
	<SerializeField()>
	Private childProjectile As BasicProjectile

	' Token: 0x04002E99 RID: 11929
	Private state As MouseLevelGhostMouseBall.State

	' Token: 0x04002E9A RID: 11930
	Private velocity As Vector2

	' Token: 0x04002E9B RID: 11931
	Private childSpeed As Single

	' Token: 0x020006F2 RID: 1778
	Public Enum State
		' Token: 0x04002E9D RID: 11933
		Init
		' Token: 0x04002E9E RID: 11934
		Moving
		' Token: 0x04002E9F RID: 11935
		Dead
	End Enum
End Class
