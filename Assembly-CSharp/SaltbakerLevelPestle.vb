Imports System
Imports UnityEngine

' Token: 0x020007D0 RID: 2000
Public Class SaltbakerLevelPestle
	Inherits AbstractProjectile

	' Token: 0x06002D67 RID: 11623 RVA: 0x001AC9C6 File Offset: 0x001AADC6
	Public Function Init(spawnPos As Vector3, velocityX As Single, velocityY As Single, gravity As Single) As SaltbakerLevelPestle
		MyBase.ResetLifetime()
		MyBase.ResetDistance()
		MyBase.transform.position = spawnPos
		Me.speed = New Vector3(velocityX, velocityY)
		Me.gravity = gravity
		Return Me
	End Function

	' Token: 0x06002D68 RID: 11624 RVA: 0x001AC9F6 File Offset: 0x001AADF6
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x06002D69 RID: 11625 RVA: 0x001ACA14 File Offset: 0x001AAE14
	Protected Overrides Sub FixedUpdate()
		MyBase.FixedUpdate()
		Me.Move()
	End Sub

	' Token: 0x06002D6A RID: 11626 RVA: 0x001ACA24 File Offset: 0x001AAE24
	Private Sub Move()
		Me.speed += New Vector3(0F, Me.gravity * CupheadTime.FixedDelta)
		MyBase.transform.Translate(Me.speed * CupheadTime.FixedDelta)
	End Sub

	' Token: 0x040035ED RID: 13805
	Private speed As Vector3

	' Token: 0x040035EE RID: 13806
	Private gravity As Single
End Class
