Imports System
Imports UnityEngine

' Token: 0x02000557 RID: 1367
Public Class ClownLevelBomb
	Inherits AbstractCollidableObject

	' Token: 0x06001983 RID: 6531 RVA: 0x000E7BAC File Offset: 0x000E5FAC
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
	End Sub

	' Token: 0x06001984 RID: 6532 RVA: 0x000E7BBF File Offset: 0x000E5FBF
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06001985 RID: 6533 RVA: 0x000E7BD7 File Offset: 0x000E5FD7
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x040022A5 RID: 8869
	Private damageDealer As DamageDealer
End Class
