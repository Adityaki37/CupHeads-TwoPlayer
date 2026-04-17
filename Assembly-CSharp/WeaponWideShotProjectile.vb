Imports System
Imports UnityEngine

' Token: 0x02000A8E RID: 2702
Public Class WeaponWideShotProjectile
	Inherits BasicProjectile

	' Token: 0x0600409E RID: 16542 RVA: 0x00232AB6 File Offset: 0x00230EB6
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.damageDealer.isDLCWeapon = True
		MyBase.GetComponent(Of SpriteRenderer)().flipY = Rand.Bool()
	End Sub

	' Token: 0x0600409F RID: 16543 RVA: 0x00232ADA File Offset: 0x00230EDA
	Protected Overrides Sub OnDealDamage(damage As Single, receiver As DamageReceiver, damageDealer As DamageDealer)
		MyBase.OnDealDamage(damage, receiver, damageDealer)
		Me.hitSpark.Create(MyBase.transform.position + MyBase.transform.right * 100F)
	End Sub

	' Token: 0x060040A0 RID: 16544 RVA: 0x00232B18 File Offset: 0x00230F18
	Protected Overrides Sub OnCollisionDie(hit As GameObject, phase As CollisionPhase)
		Me.hitSpark.Create(MyBase.transform.position + MyBase.transform.right * 100F)
		MyBase.OnCollisionDie(hit, phase)
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x04004758 RID: 18264
	Private Const HITSPARK_OFFSET As Single = 100F

	' Token: 0x04004759 RID: 18265
	<SerializeField()>
	Private hitSpark As Effect
End Class
