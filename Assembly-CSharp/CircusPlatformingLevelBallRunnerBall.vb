Imports System
Imports UnityEngine

' Token: 0x0200089D RID: 2205
Public Class CircusPlatformingLevelBallRunnerBall
	Inherits AbstractCollidableObject

	' Token: 0x0600334F RID: 13135 RVA: 0x001DDD3F File Offset: 0x001DC13F
	Private Sub Start()
		MyBase.animator.SetBool("isBlue", Rand.Bool())
		Me.damageDealer = DamageDealer.NewEnemy()
	End Sub

	' Token: 0x06003350 RID: 13136 RVA: 0x001DDD64 File Offset: 0x001DC164
	Protected Overrides Sub OnCollisionWalls(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionWalls(hit, phase)
		If hit.GetComponentInParent(Of PlatformingLevelEditorPlatform)() AndAlso phase = CollisionPhase.Enter AndAlso Me.isMoving Then
			Me.direction *= -1F
		End If
	End Sub

	' Token: 0x06003351 RID: 13137 RVA: 0x001DDDB0 File Offset: 0x001DC1B0
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06003352 RID: 13138 RVA: 0x001DDDC8 File Offset: 0x001DC1C8
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06003353 RID: 13139 RVA: 0x001DDDE6 File Offset: 0x001DC1E6
	Private Sub FixedUpdate()
		If Me.isMoving Then
			Me.Move()
		End If
	End Sub

	' Token: 0x06003354 RID: 13140 RVA: 0x001DDDF9 File Offset: 0x001DC1F9
	Protected Overridable Sub Move()
		MyBase.transform.position += Me.direction * Me.Speed * CupheadTime.FixedDelta
	End Sub

	' Token: 0x04003B9B RID: 15259
	Public isMoving As Boolean

	' Token: 0x04003B9C RID: 15260
	<SerializeField()>
	Private Speed As Single = 500F

	' Token: 0x04003B9D RID: 15261
	<SerializeField()>
	Private runner As CircusPlatformingLevelBallRunner

	' Token: 0x04003B9E RID: 15262
	Private damageDealer As DamageDealer

	' Token: 0x04003B9F RID: 15263
	Public direction As Vector3 = Vector3.right
End Class
