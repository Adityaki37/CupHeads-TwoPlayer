Imports System
Imports UnityEngine

' Token: 0x02000A70 RID: 2672
Public Class WeaponChargeExBurst
	Inherits AbstractProjectile

	' Token: 0x06003FD4 RID: 16340 RVA: 0x0022D52E File Offset: 0x0022B92E
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.GetComponent(Of SpriteRenderer)().flipX = Rand.Bool()
	End Sub

	' Token: 0x06003FD5 RID: 16341 RVA: 0x0022D546 File Offset: 0x0022B946
	Protected Overrides Sub OnCollisionEnemy(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionEnemy(hit, phase)
		If phase = CollisionPhase.Enter AndAlso Me.damageDealer IsNot Nothing Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06003FD6 RID: 16342 RVA: 0x0022D56E File Offset: 0x0022B96E
	Private Sub OnEffectComplete()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x040046B1 RID: 18097
	Public Const Offset As Single = 125F
End Class
