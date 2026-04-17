Imports System
Imports UnityEngine

' Token: 0x02000A78 RID: 2680
Public Class WeaponExploderProjectileExplosion
	Inherits Effect

	' Token: 0x0600400B RID: 16395 RVA: 0x0022EF9C File Offset: 0x0022D39C
	Public Sub Create(position As Vector2, radius As Single, damage As Single, damageMultiplier As Single, weapon As WeaponExploder, tracker As MeterScoreTracker)
		Dim num As Single = radius / 15F
		Dim weaponExploderProjectileExplosion As WeaponExploderProjectileExplosion = TryCast(MyBase.Create(position, New Vector3(num, num, 1F)), WeaponExploderProjectileExplosion)
		weaponExploderProjectileExplosion.damageDealer.SetDamage(damage)
		weaponExploderProjectileExplosion.damageDealer.DamageMultiplier *= damageMultiplier
		weaponExploderProjectileExplosion.damageDealer.SetDamageFlags(False, True, False)
		weaponExploderProjectileExplosion.weapon = weapon
		AddHandler weaponExploderProjectileExplosion.damageDealer.OnDealDamage, AddressOf weaponExploderProjectileExplosion.OnDealDamage
		If tracker IsNot Nothing Then
			tracker.Add(weaponExploderProjectileExplosion.damageDealer)
		End If
	End Sub

	' Token: 0x0600400C RID: 16396 RVA: 0x0022F030 File Offset: 0x0022D430
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageDealer = New DamageDealer(1F, 0F)
	End Sub

	' Token: 0x0600400D RID: 16397 RVA: 0x0022F04D File Offset: 0x0022D44D
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x0600400E RID: 16398 RVA: 0x0022F065 File Offset: 0x0022D465
	Protected Overrides Sub OnCollisionEnemy(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionEnemy(hit, phase)
		If phase = CollisionPhase.Enter AndAlso Me.damageDealer IsNot Nothing Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x0600400F RID: 16399 RVA: 0x0022F08D File Offset: 0x0022D48D
	Private Sub OnDealDamage(damage As Single, damageReceiver As DamageReceiver, damageDealer As DamageDealer)
		If Me.weapon IsNot Nothing Then
			Me.weapon.OnDealDamage(damage, damageReceiver, damageDealer)
		End If
	End Sub

	' Token: 0x040046D6 RID: 18134
	Private damageDealer As DamageDealer

	' Token: 0x040046D7 RID: 18135
	Private Const BASE_RADIUS As Single = 15F

	' Token: 0x040046D8 RID: 18136
	Private weapon As WeaponExploder
End Class
