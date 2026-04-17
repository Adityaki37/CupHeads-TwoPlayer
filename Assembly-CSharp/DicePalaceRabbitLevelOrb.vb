Imports System
Imports UnityEngine

' Token: 0x020005DF RID: 1503
Public Class DicePalaceRabbitLevelOrb
	Inherits AbstractProjectile

	' Token: 0x06001DB7 RID: 7607 RVA: 0x00111318 File Offset: 0x0010F718
	Protected Overrides Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
		MyBase.Update()
	End Sub

	' Token: 0x06001DB8 RID: 7608 RVA: 0x00111336 File Offset: 0x0010F736
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x06001DB9 RID: 7609 RVA: 0x00111354 File Offset: 0x0010F754
	Public Sub SetAsGold(isGold As Boolean)
		If isGold Then
			MyBase.animator.SetTrigger("Gold")
		Else
			MyBase.animator.SetTrigger("Blue")
		End If
	End Sub
End Class
