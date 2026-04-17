Imports System
Imports UnityEngine

' Token: 0x0200090B RID: 2315
Public Class PlatformingLevelRotator
	Inherits AbstractCollidableObject

	' Token: 0x06003650 RID: 13904 RVA: 0x001F7D3A File Offset: 0x001F613A
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
	End Sub

	' Token: 0x06003651 RID: 13905 RVA: 0x001F7D4D File Offset: 0x001F614D
	Private Sub Update()
		MyBase.transform.AddEulerAngles(0F, 0F, -Me.speed * CupheadTime.Delta)
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06003652 RID: 13906 RVA: 0x001F7D8C File Offset: 0x001F618C
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x04003E42 RID: 15938
	Public speed As Single = 180F

	' Token: 0x04003E43 RID: 15939
	Private damageDealer As DamageDealer
End Class
