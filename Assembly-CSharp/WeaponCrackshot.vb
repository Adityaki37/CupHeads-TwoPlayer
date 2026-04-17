Imports System
Imports UnityEngine

' Token: 0x02000A72 RID: 2674
Public Class WeaponCrackshot
	Inherits AbstractLevelWeapon

	' Token: 0x17000584 RID: 1412
	' (get) Token: 0x06003FDA RID: 16346 RVA: 0x0022D64D File Offset: 0x0022BA4D
	Protected Overrides ReadOnly Property rapidFire As Boolean
		Get
			Return True
		End Get
	End Property

	' Token: 0x17000585 RID: 1413
	' (get) Token: 0x06003FDB RID: 16347 RVA: 0x0022D650 File Offset: 0x0022BA50
	Protected Overrides ReadOnly Property rapidFireRate As Single
		Get
			Return WeaponProperties.LevelWeaponCrackshot.Basic.fireRate
		End Get
	End Property

	' Token: 0x06003FDC RID: 16348 RVA: 0x0022D658 File Offset: 0x0022BA58
	Protected Overrides Function fireBasic() As AbstractProjectile
		Dim weaponCrackshotProjectile As WeaponCrackshotProjectile = TryCast(MyBase.fireBasic(), WeaponCrackshotProjectile)
		weaponCrackshotProjectile.Speed = WeaponProperties.LevelWeaponCrackshot.Basic.initialSpeed
		weaponCrackshotProjectile.Damage = WeaponProperties.LevelWeaponCrackshot.Basic.initialDamage
		weaponCrackshotProjectile.PlayerId = Me.player.id
		weaponCrackshotProjectile.DamagesType.PlayerProjectileDefault()
		weaponCrackshotProjectile.CollisionDeath.PlayerProjectileDefault()
		weaponCrackshotProjectile.maxAngleRange = If((Not WeaponProperties.LevelWeaponCrackshot.Basic.enableMaxAngle), 180F, WeaponProperties.LevelWeaponCrackshot.Basic.maxAngle)
		weaponCrackshotProjectile.[variant] = Me.variantString.PopInt()
		weaponCrackshotProjectile.useBComet = Me.useBComet
		Me.useBComet = Not Me.useBComet
		Dim num As Single = Me.yPositions(Me.currentY)
		Me.currentY += 1
		If Me.currentY >= Me.yPositions.Length Then
			Me.currentY = 0
		End If
		weaponCrackshotProjectile.transform.AddPosition(0F, num, 0F)
		Return weaponCrackshotProjectile
	End Function

	' Token: 0x06003FDD RID: 16349 RVA: 0x0022D748 File Offset: 0x0022BB48
	Protected Overrides Function fireEx() As AbstractProjectile
		If Me.activeEX Then
			Me.activeEX.GetReplaced()
		End If
		Dim weaponCrackshotExProjectile As WeaponCrackshotExProjectile = TryCast(MyBase.fireEx(), WeaponCrackshotExProjectile)
		weaponCrackshotExProjectile.Damage = WeaponProperties.LevelWeaponCrackshot.Ex.collideDamage
		weaponCrackshotExProjectile.DamageRate = 0F
		weaponCrackshotExProjectile.PlayerId = Me.player.id
		Dim meterScoreTracker As MeterScoreTracker = New MeterScoreTracker(MeterScoreTracker.Type.Ex)
		meterScoreTracker.Add(weaponCrackshotExProjectile)
		Me.activeEX = weaponCrackshotExProjectile
		Return weaponCrackshotExProjectile
	End Function

	' Token: 0x06003FDE RID: 16350 RVA: 0x0022D7B9 File Offset: 0x0022BBB9
	Public Overrides Sub BeginBasic()
		AudioManager.Play("player_weapon_crackshot_shoot_start")
		Me.emitAudioFromObject.Add("player_weapon_crackshot_shoot_start")
		Me.variantString = New PatternString("0,1,0,2,1,2,0,1,2", True)
		MyBase.BeginBasic()
	End Sub

	' Token: 0x06003FDF RID: 16351 RVA: 0x0022D7EC File Offset: 0x0022BBEC
	Public Overrides Sub EndBasic()
		Me.ActivateCooldown()
		MyBase.EndBasic()
	End Sub

	' Token: 0x040046B3 RID: 18099
	Private Const Y_POS As Single = 20F

	' Token: 0x040046B4 RID: 18100
	Private Const ROTATION_OFFSET As Single = 3F

	' Token: 0x040046B5 RID: 18101
	Private yPositions As Single() = New Single() { 0F, 20F, 40F, 20F }

	' Token: 0x040046B6 RID: 18102
	Private currentY As Integer

	' Token: 0x040046B7 RID: 18103
	<SerializeField()>
	Private variantString As PatternString

	' Token: 0x040046B8 RID: 18104
	Private useBComet As Boolean

	' Token: 0x040046B9 RID: 18105
	Private activeEX As WeaponCrackshotExProjectile
End Class
