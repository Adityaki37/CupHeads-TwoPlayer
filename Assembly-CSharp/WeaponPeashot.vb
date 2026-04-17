Imports System

' Token: 0x02000A7F RID: 2687
Public Class WeaponPeashot
	Inherits AbstractLevelWeapon

	' Token: 0x1700058E RID: 1422
	' (get) Token: 0x0600403F RID: 16447 RVA: 0x00230999 File Offset: 0x0022ED99
	Protected Overrides ReadOnly Property rapidFire As Boolean
		Get
			Return WeaponProperties.LevelWeaponPeashot.Basic.rapidFire
		End Get
	End Property

	' Token: 0x1700058F RID: 1423
	' (get) Token: 0x06004040 RID: 16448 RVA: 0x002309A0 File Offset: 0x0022EDA0
	Protected Overrides ReadOnly Property rapidFireRate As Single
		Get
			Return WeaponProperties.LevelWeaponPeashot.Basic.rapidFireRate
		End Get
	End Property

	' Token: 0x06004041 RID: 16449 RVA: 0x002309A8 File Offset: 0x0022EDA8
	Protected Overrides Function fireBasic() As AbstractProjectile
		Dim basicProjectile As BasicProjectile = TryCast(MyBase.fireBasic(), BasicProjectile)
		basicProjectile.Speed = WeaponProperties.LevelWeaponPeashot.Basic.speed
		basicProjectile.Damage = WeaponProperties.LevelWeaponPeashot.Basic.damage
		basicProjectile.PlayerId = Me.player.id
		basicProjectile.DamagesType.PlayerProjectileDefault()
		basicProjectile.CollisionDeath.PlayerProjectileDefault()
		Dim num As Single = Me.yPositions(Me.currentY)
		Me.currentY += 1
		If Me.currentY >= Me.yPositions.Length Then
			Me.currentY = 0
		End If
		basicProjectile.transform.AddPosition(0F, num, 0F)
		Return basicProjectile
	End Function

	' Token: 0x06004042 RID: 16450 RVA: 0x00230A4C File Offset: 0x0022EE4C
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

	' Token: 0x06004043 RID: 16451 RVA: 0x00230ACA File Offset: 0x0022EECA
	Public Overrides Sub BeginBasic()
		Me.OneShotCooldown("player_default_fire_start")
		Me.BasicSoundLoop("player_default_fire_loop", "player_default_fire_loop_p2")
		MyBase.BeginBasic()
	End Sub

	' Token: 0x06004044 RID: 16452 RVA: 0x00230AED File Offset: 0x0022EEED
	Public Overrides Sub EndBasic()
		Me.ActivateCooldown()
		MyBase.EndBasic()
		Me.StopLoopSound("player_default_fire_loop", "player_default_fire_loop_p2")
	End Sub

	' Token: 0x04004711 RID: 18193
	Private Const Y_POS As Single = 20F

	' Token: 0x04004712 RID: 18194
	Private Const ROTATION_OFFSET As Single = 3F

	' Token: 0x04004713 RID: 18195
	Private yPositions As Single() = New Single() { 0F, 20F, 40F, 20F }

	' Token: 0x04004714 RID: 18196
	Private currentY As Integer
End Class
