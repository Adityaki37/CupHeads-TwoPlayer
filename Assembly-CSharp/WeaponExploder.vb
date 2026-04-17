Imports System
Imports System.Collections.Generic

' Token: 0x02000A76 RID: 2678
Public Class WeaponExploder
	Inherits AbstractLevelWeapon

	' Token: 0x17000587 RID: 1415
	' (get) Token: 0x06003FFE RID: 16382 RVA: 0x0022EB93 File Offset: 0x0022CF93
	Protected Overrides ReadOnly Property rapidFire As Boolean
		Get
			Return True
		End Get
	End Property

	' Token: 0x17000588 RID: 1416
	' (get) Token: 0x06003FFF RID: 16383 RVA: 0x0022EB96 File Offset: 0x0022CF96
	Protected Overrides ReadOnly Property rapidFireRate As Single
		Get
			Return WeaponProperties.LevelWeaponExploder.Basic.fireRate
		End Get
	End Property

	' Token: 0x06004000 RID: 16384 RVA: 0x0022EBA0 File Offset: 0x0022CFA0
	Protected Overrides Function fireBasic() As AbstractProjectile
		Dim weaponExploderProjectile As WeaponExploderProjectile = TryCast(MyBase.fireBasic(), WeaponExploderProjectile)
		weaponExploderProjectile.Speed = WeaponProperties.LevelWeaponExploder.Basic.speed
		weaponExploderProjectile.PlayerId = Me.player.id
		weaponExploderProjectile.DamagesType.SetAll(False)
		weaponExploderProjectile.CollisionDeath.PlayerProjectileDefault()
		weaponExploderProjectile.weapon = Me
		weaponExploderProjectile.minMaxSpeed = WeaponProperties.LevelWeaponExploder.Basic.easeSpeed
		weaponExploderProjectile.easeTime = WeaponProperties.LevelWeaponExploder.Basic.easeTime
		If WeaponProperties.LevelWeaponExploder.Basic.easing Then
			weaponExploderProjectile.EaseSpeed()
		End If
		Return weaponExploderProjectile
	End Function

	' Token: 0x06004001 RID: 16385 RVA: 0x0022EC1C File Offset: 0x0022D01C
	Protected Overrides Function fireEx() As AbstractProjectile
		Dim weaponExploderProjectile As WeaponExploderProjectile = TryCast(MyBase.fireEx(), WeaponExploderProjectile)
		weaponExploderProjectile.Speed = WeaponProperties.LevelWeaponExploder.Ex.speed
		weaponExploderProjectile.Damage = WeaponProperties.LevelWeaponExploder.Ex.damage
		weaponExploderProjectile.explodeRadius = WeaponProperties.LevelWeaponExploder.Ex.explodeRadius
		weaponExploderProjectile.PlayerId = Me.player.id
		weaponExploderProjectile.DamagesType.SetAll(False)
		weaponExploderProjectile.CollisionDeath.PlayerProjectileDefault()
		Dim meterScoreTracker As MeterScoreTracker = New MeterScoreTracker(MeterScoreTracker.Type.Ex)
		meterScoreTracker.Add(weaponExploderProjectile)
		Return weaponExploderProjectile
	End Function

	' Token: 0x040046CD RID: 18125
	Public projectilesOnGround As List(Of WeaponArcProjectile) = New List(Of WeaponArcProjectile)()
End Class
