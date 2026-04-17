Imports System
Imports UnityEngine

' Token: 0x020007C9 RID: 1993
Public Class SaltbakerLevelHand
	Inherits AbstractCollidableObject

	' Token: 0x06002D32 RID: 11570 RVA: 0x001A9E36 File Offset: 0x001A8236
	Private Sub Start()
		Me.damageDealer = DamageDealer.NewEnemy()
	End Sub

	' Token: 0x06002D33 RID: 11571 RVA: 0x001A9E43 File Offset: 0x001A8243
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06002D34 RID: 11572 RVA: 0x001A9E5B File Offset: 0x001A825B
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06002D35 RID: 11573 RVA: 0x001A9E7C File Offset: 0x001A827C
	Public Sub Shoot(speed As Single)
		Dim num As Single = If((Not Me.leftHand), 180F, 0F)
		Me.projectile.Create(Me.root.position, num, speed)
	End Sub

	' Token: 0x040035A9 RID: 13737
	<SerializeField()>
	Private projectile As BasicProjectile

	' Token: 0x040035AA RID: 13738
	<SerializeField()>
	Private root As Transform

	' Token: 0x040035AB RID: 13739
	<SerializeField()>
	Private leftHand As Boolean

	' Token: 0x040035AC RID: 13740
	Private damageDealer As DamageDealer
End Class
