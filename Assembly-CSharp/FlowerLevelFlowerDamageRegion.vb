Imports System
Imports UnityEngine

' Token: 0x0200060A RID: 1546
Public Class FlowerLevelFlowerDamageRegion
	Inherits CollisionChild

	' Token: 0x06001F14 RID: 7956 RVA: 0x0011DF3B File Offset: 0x0011C33B
	Protected Overrides Sub Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
		MyBase.Awake()
	End Sub

	' Token: 0x06001F15 RID: 7957 RVA: 0x0011DF4E File Offset: 0x0011C34E
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06001F16 RID: 7958 RVA: 0x0011DF66 File Offset: 0x0011C366
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x040027B5 RID: 10165
	Private damageDealer As DamageDealer
End Class
