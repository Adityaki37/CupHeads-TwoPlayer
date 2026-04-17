Imports System

' Token: 0x02000A83 RID: 2691
Public Class WeaponSplitter
	Inherits AbstractLevelWeapon

	' Token: 0x17000592 RID: 1426
	' (get) Token: 0x06004053 RID: 16467 RVA: 0x002310C9 File Offset: 0x0022F4C9
	Protected Overrides ReadOnly Property rapidFire As Boolean
		Get
			Return True
		End Get
	End Property

	' Token: 0x17000593 RID: 1427
	' (get) Token: 0x06004054 RID: 16468 RVA: 0x002310CC File Offset: 0x0022F4CC
	Protected Overrides ReadOnly Property rapidFireRate As Single
		Get
			Return WeaponProperties.LevelWeaponSplitter.Basic.fireRate
		End Get
	End Property

	' Token: 0x06004055 RID: 16469 RVA: 0x002310D4 File Offset: 0x0022F4D4
	Protected Overrides Function fireBasic() As AbstractProjectile
		Dim weaponSplitterProjectile As WeaponSplitterProjectile = TryCast(MyBase.fireBasic(), WeaponSplitterProjectile)
		weaponSplitterProjectile.Speed = WeaponProperties.LevelWeaponSplitter.Basic.speed
		weaponSplitterProjectile.Damage = WeaponProperties.LevelWeaponSplitter.Basic.bulletDamage
		weaponSplitterProjectile.isMain = True
		weaponSplitterProjectile.nextDistance = WeaponProperties.LevelWeaponSplitter.Basic.splitDistanceA
		weaponSplitterProjectile.PlayerId = Me.player.id
		weaponSplitterProjectile.DamagesType.PlayerProjectileDefault()
		weaponSplitterProjectile.CollisionDeath.PlayerProjectileDefault()
		Return weaponSplitterProjectile
	End Function

	' Token: 0x06004056 RID: 16470 RVA: 0x00231140 File Offset: 0x0022F540
	Protected Overrides Function fireEx() As AbstractProjectile
		Dim weaponPeashotExProjectile As WeaponPeashotExProjectile = TryCast(MyBase.fireEx(), WeaponPeashotExProjectile)
		weaponPeashotExProjectile.moveSpeed = WeaponProperties.LevelWeaponPeashot.Ex.speed
		weaponPeashotExProjectile.Damage = WeaponProperties.LevelWeaponPeashot.Ex.damage
		weaponPeashotExProjectile.hitFreezeTime = WeaponProperties.LevelWeaponPeashot.Ex.freezeTime
		weaponPeashotExProjectile.DamageRate = weaponPeashotExProjectile.hitFreezeTime + WeaponProperties.LevelWeaponPeashot.Ex.damageDistance / weaponPeashotExProjectile.moveSpeed
		weaponPeashotExProjectile.maxDamage = WeaponProperties.LevelWeaponPeashot.Ex.maxDamage
		weaponPeashotExProjectile.PlayerId = Me.player.id
		Dim meterScoreTracker As MeterScoreTracker = New MeterScoreTracker(MeterScoreTracker.Type.Ex)
		meterScoreTracker.Add(weaponPeashotExProjectile)
		Return weaponPeashotExProjectile
	End Function

	' Token: 0x06004057 RID: 16471 RVA: 0x002311BE File Offset: 0x0022F5BE
	Public Overrides Sub BeginBasic()
		Me.OneShotCooldown("player_default_fire_start")
		Me.BasicSoundLoop("player_default_fire_loop", "player_default_fire_loop_p2")
		MyBase.BeginBasic()
	End Sub

	' Token: 0x06004058 RID: 16472 RVA: 0x002311E1 File Offset: 0x0022F5E1
	Public Overrides Sub EndBasic()
		Me.ActivateCooldown()
		MyBase.EndBasic()
		Me.StopLoopSound("player_default_fire_loop", "player_default_fire_loop_p2")
	End Sub

	' Token: 0x0400472C RID: 18220
	Private Const ROTATION_OFFSET As Single = 3F
End Class
