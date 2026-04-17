Imports System
Imports UnityEngine

' Token: 0x02000882 RID: 2178
Public Class ForestPlatformingLevelLobberProjectileExplosion
	Inherits Effect

	' Token: 0x06003292 RID: 12946 RVA: 0x001D685F File Offset: 0x001D4C5F
	Protected Overrides Sub Awake()
		MyBase.Awake()
		AudioManager.Play("level_lobber_projectile_explosion")
		Me.emitAudioFromObject.Add("level_lobber_projectile_explosion")
		Me.damageDealer = DamageDealer.NewEnemy()
	End Sub

	' Token: 0x06003293 RID: 12947 RVA: 0x001D688C File Offset: 0x001D4C8C
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06003294 RID: 12948 RVA: 0x001D68A4 File Offset: 0x001D4CA4
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If Me.damageDealer IsNot Nothing AndAlso phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x04003ADF RID: 15071
	Private damageDealer As DamageDealer
End Class
