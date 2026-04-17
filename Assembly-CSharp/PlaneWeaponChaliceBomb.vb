Imports System
Imports UnityEngine

' Token: 0x02000AB8 RID: 2744
Public Class PlaneWeaponChaliceBomb
	Inherits AbstractPlaneWeapon

	' Token: 0x170005C4 RID: 1476
	' (get) Token: 0x060041EF RID: 16879 RVA: 0x0023A19B File Offset: 0x0023859B
	Protected Overrides ReadOnly Property rapidFire As Boolean
		Get
			Return WeaponProperties.PlaneWeaponChaliceBomb.Basic.rapidFire
		End Get
	End Property

	' Token: 0x170005C5 RID: 1477
	' (get) Token: 0x060041F0 RID: 16880 RVA: 0x0023A1A2 File Offset: 0x002385A2
	Protected Overrides ReadOnly Property rapidFireRate As Single
		Get
			Return WeaponProperties.PlaneWeaponChaliceBomb.Basic.rapidFireRate
		End Get
	End Property

	' Token: 0x060041F1 RID: 16881 RVA: 0x0023A1AC File Offset: 0x002385AC
	Protected Overrides Function fireBasic() As AbstractProjectile
		Dim planeWeaponChaliceBombProjectile As PlaneWeaponChaliceBombProjectile = TryCast(MyBase.fireBasic(), PlaneWeaponChaliceBombProjectile)
		planeWeaponChaliceBombProjectile.transform.Rotate(New Vector3(0F, 0F, Global.UnityEngine.Random.Range(-WeaponProperties.PlaneWeaponChaliceBomb.Basic.angleRange, WeaponProperties.PlaneWeaponChaliceBomb.Basic.angleRange)))
		planeWeaponChaliceBombProjectile.velocity = WeaponProperties.PlaneWeaponChaliceBomb.Basic.speed * MathUtils.AngleToDirection(planeWeaponChaliceBombProjectile.transform.rotation.eulerAngles.z)
		planeWeaponChaliceBombProjectile.gravity = WeaponProperties.PlaneWeaponChaliceBomb.Basic.gravity
		planeWeaponChaliceBombProjectile.Damage = WeaponProperties.PlaneWeaponChaliceBomb.Basic.damage
		planeWeaponChaliceBombProjectile.size = WeaponProperties.PlaneWeaponChaliceBomb.Basic.size
		planeWeaponChaliceBombProjectile.damageExplosion = WeaponProperties.PlaneWeaponChaliceBomb.Basic.damageExplosion
		planeWeaponChaliceBombProjectile.explosionSize = WeaponProperties.PlaneWeaponChaliceBomb.Basic.sizeExplosion
		planeWeaponChaliceBombProjectile.PlayerId = Me.player.id
		planeWeaponChaliceBombProjectile.SetAnimation(Me.isA)
		Me.isA = Not Me.isA
		Return planeWeaponChaliceBombProjectile
	End Function

	' Token: 0x060041F2 RID: 16882 RVA: 0x0023A284 File Offset: 0x00238684
	Protected Overrides Function fireEx() As AbstractProjectile
		Dim planeWeaponChaliceBombExProjectile As PlaneWeaponChaliceBombExProjectile = TryCast(MyBase.fireEx(), PlaneWeaponChaliceBombExProjectile)
		planeWeaponChaliceBombExProjectile.FreezeTime = WeaponProperties.PlaneWeaponChaliceBomb.Ex.freezeTime
		planeWeaponChaliceBombExProjectile.Damage = WeaponProperties.PlaneWeaponChaliceBomb.Ex.damage
		planeWeaponChaliceBombExProjectile.DamageRate = WeaponProperties.PlaneWeaponChaliceBomb.Ex.damageRate
		planeWeaponChaliceBombExProjectile.DamageRateIncrease = WeaponProperties.PlaneWeaponChaliceBomb.Ex.damageRateIncrease
		planeWeaponChaliceBombExProjectile.Gravity = WeaponProperties.PlaneWeaponChaliceBomb.Ex.gravity
		planeWeaponChaliceBombExProjectile.Velocity = WeaponProperties.PlaneWeaponChaliceBomb.Ex.startSpeed * Vector3.right
		planeWeaponChaliceBombExProjectile.PlayerId = Me.player.id
		Dim meterScoreTracker As MeterScoreTracker = New MeterScoreTracker(MeterScoreTracker.Type.Ex)
		meterScoreTracker.Add(planeWeaponChaliceBombExProjectile)
		Return planeWeaponChaliceBombExProjectile
	End Function

	' Token: 0x04004854 RID: 18516
	Private isA As Boolean
End Class
