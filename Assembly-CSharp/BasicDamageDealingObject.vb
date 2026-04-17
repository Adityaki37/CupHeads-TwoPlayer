Imports System
Imports UnityEngine

' Token: 0x02000430 RID: 1072
<RequireComponent(GetType(Collider2D))>
Public Class BasicDamageDealingObject
	Inherits AbstractCollidableObject

	' Token: 0x06000FA5 RID: 4005 RVA: 0x0009C651 File Offset: 0x0009AA51
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageDealer.SetRate(Me.damageRate)
	End Sub

	' Token: 0x06000FA6 RID: 4006 RVA: 0x0009C675 File Offset: 0x0009AA75
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06000FA7 RID: 4007 RVA: 0x0009C693 File Offset: 0x0009AA93
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x040018D8 RID: 6360
	<SerializeField()>
	Private damageRate As Single = 0.2F

	' Token: 0x040018D9 RID: 6361
	Private damageDealer As DamageDealer
End Class
