Imports System
Imports UnityEngine

' Token: 0x02000908 RID: 2312
Public Class PlatformingLevelParryObject
	Inherits ParrySwitch

	' Token: 0x06003642 RID: 13890 RVA: 0x001F7B23 File Offset: 0x001F5F23
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
	End Sub

	' Token: 0x06003643 RID: 13891 RVA: 0x001F7B36 File Offset: 0x001F5F36
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06003644 RID: 13892 RVA: 0x001F7B4E File Offset: 0x001F5F4E
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If Me.hurtsPlayer AndAlso Me.damageDealer IsNot Nothing AndAlso phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x04003E3A RID: 15930
	Public hurtsPlayer As Boolean

	' Token: 0x04003E3B RID: 15931
	Private damageDealer As DamageDealer
End Class
