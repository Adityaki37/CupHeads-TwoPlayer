Imports System
Imports UnityEngine

' Token: 0x020005AA RID: 1450
Public Class DicePalaceCardLevelSpikes
	Inherits AbstractCollidableObject

	' Token: 0x06001BED RID: 7149 RVA: 0x000FFE95 File Offset: 0x000FE295
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
	End Sub

	' Token: 0x06001BEE RID: 7150 RVA: 0x000FFEA8 File Offset: 0x000FE2A8
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x06001BEF RID: 7151 RVA: 0x000FFEC6 File Offset: 0x000FE2C6
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x04002504 RID: 9476
	Private damageDealer As DamageDealer
End Class
