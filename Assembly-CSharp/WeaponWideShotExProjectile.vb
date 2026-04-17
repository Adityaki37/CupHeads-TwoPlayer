Imports System
Imports UnityEngine

' Token: 0x02000A8D RID: 2701
Public Class WeaponWideShotExProjectile
	Inherits AbstractProjectile

	' Token: 0x06004099 RID: 16537 RVA: 0x002328FC File Offset: 0x00230CFC
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.transform.position += MyBase.transform.right * 100F
		Me.damageDealer.isDLCWeapon = True
	End Sub

	' Token: 0x0600409A RID: 16538 RVA: 0x0023293B File Offset: 0x00230D3B
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.mainTimer < Me.mainDuration Then
			Me.mainTimer += CupheadTime.Delta
		Else
			Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		End If
	End Sub

	' Token: 0x0600409B RID: 16539 RVA: 0x0023297C File Offset: 0x00230D7C
	Protected Overrides Sub OnDealDamage(damage As Single, receiver As DamageReceiver, damageDealer As DamageDealer)
		MyBase.OnDealDamage(damage, receiver, damageDealer)
		Dim componentInChildren As Collider2D = receiver.GetComponentInChildren(Of Collider2D)()
		Dim vector As Vector3 = receiver.transform.position
		If componentInChildren IsNot Nothing Then
			vector = componentInChildren.transform.position + New Vector3(componentInChildren.offset.x * receiver.transform.lossyScale.x, componentInChildren.offset.y * receiver.transform.lossyScale.y)
		End If
		Dim vector2 As Vector3 = MathUtils.AngleToDirection(MyBase.transform.eulerAngles.z)
		Dim vector3 As Vector3 = Me.origin + vector2 * Vector3.Distance(Me.origin, vector)
		Me.hitsparkPrefab.Create(Vector3.Lerp(vector3, vector, 0.5F) + MathUtils.RandomPointInUnitCircle() * 30F)
	End Sub

	' Token: 0x0600409C RID: 16540 RVA: 0x00232A7F File Offset: 0x00230E7F
	Protected Overrides Sub OnCollisionEnemy(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionEnemy(hit, phase)
		Me.damageDealer.DealDamage(hit)
		AddHandler Me.damageDealer.OnDealDamage, AddressOf Me.OnDealDamage
	End Sub

	' Token: 0x04004754 RID: 18260
	Public mainDuration As Single

	' Token: 0x04004755 RID: 18261
	Private mainTimer As Single

	' Token: 0x04004756 RID: 18262
	Public origin As Vector3

	' Token: 0x04004757 RID: 18263
	<SerializeField()>
	Private hitsparkPrefab As Effect
End Class
