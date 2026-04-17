Imports System
Imports UnityEngine

' Token: 0x02000A6F RID: 2671
Public Class WeaponCharge
	Inherits AbstractLevelWeapon

	' Token: 0x17000581 RID: 1409
	' (get) Token: 0x06003FC8 RID: 16328 RVA: 0x0022D059 File Offset: 0x0022B459
	Protected Overrides ReadOnly Property rapidFire As Boolean
		Get
			Return False
		End Get
	End Property

	' Token: 0x17000582 RID: 1410
	' (get) Token: 0x06003FC9 RID: 16329 RVA: 0x0022D05C File Offset: 0x0022B45C
	Protected Overrides ReadOnly Property rapidFireRate As Single
		Get
			Return WeaponProperties.LevelWeaponCharge.Basic.fireRate
		End Get
	End Property

	' Token: 0x17000583 RID: 1411
	' (get) Token: 0x06003FCA RID: 16330 RVA: 0x0022D063 File Offset: 0x0022B463
	Protected Overrides ReadOnly Property isChargeWeapon As Boolean
		Get
			Return True
		End Get
	End Property

	' Token: 0x06003FCB RID: 16331 RVA: 0x0022D068 File Offset: 0x0022B468
	Protected Overrides Sub StartCharging()
		MyBase.StartCharging()
		Me.BasicSoundOneShot("player_weapon_charge_start", "player_weapon_charge_start_p2")
		If Me.chargeEffect IsNot Nothing Then
			Global.UnityEngine.[Object].Destroy(Me.chargeEffect.gameObject)
			Me.chargeEffect = Nothing
		End If
		Me.chargeEffect = Me.chargeEffectPrefab.Create(MyBase.transform.position)
		Me.chargeEffect.transform.parent = Me.player.transform
		Me.timeCharged = 0F
	End Sub

	' Token: 0x06003FCC RID: 16332 RVA: 0x0022D0FA File Offset: 0x0022B4FA
	Protected Overrides Sub StopCharging()
		MyBase.StopCharging()
		If Me.chargeEffect IsNot Nothing Then
			Global.UnityEngine.[Object].Destroy(Me.chargeEffect.gameObject)
			Me.chargeEffect = Nothing
			Me.timeCharged = 0F
		End If
	End Sub

	' Token: 0x06003FCD RID: 16333 RVA: 0x0022D138 File Offset: 0x0022B538
	Private Sub FixedUpdate()
		If Me.chargeEffect Is Nothing Then
			Me.fullyCharged = False
			Me.damage = WeaponProperties.LevelWeaponCharge.Basic.baseDamage
		Else
			Me.chargeEffect.transform.position = Me.player.weaponManager.GetBulletPosition()
			Me.timeCharged += CupheadTime.FixedDelta
			If Me.timeCharged > WeaponProperties.LevelWeaponCharge.Basic.timeStateThree Then
				Me.fullyCharged = True
				If Me.AllowChargeSound Then
					AudioManager.Play("player_weapon_charge_ready")
					Me.AllowChargeSound = False
				End If
				Me.chargeEffect.animator.SetTrigger("IsFull")
				Me.damage = WeaponProperties.LevelWeaponCharge.Basic.damageStateThree
			Else
				Me.fullyCharged = False
				Me.damage = WeaponProperties.LevelWeaponCharge.Basic.baseDamage
			End If
		End If
	End Sub

	' Token: 0x06003FCE RID: 16334 RVA: 0x0022D210 File Offset: 0x0022B610
	Protected Overrides Function fireBasic() As AbstractProjectile
		Dim weaponChargeProjectile As WeaponChargeProjectile
		If Me.fullyCharged Then
			Dim basicEffectPrefab As Effect = Me.basicEffectPrefab
			Me.basicEffectPrefab = Nothing
			weaponChargeProjectile = TryCast(MyBase.fireBasic(), WeaponChargeProjectile)
			Me.basicEffectPrefab = basicEffectPrefab
		Else
			weaponChargeProjectile = TryCast(MyBase.fireBasic(), WeaponChargeProjectile)
		End If
		weaponChargeProjectile.Speed = If((Not Me.fullyCharged), WeaponProperties.LevelWeaponCharge.Basic.speed, WeaponProperties.LevelWeaponCharge.Basic.speedStateTwo)
		weaponChargeProjectile.Damage = Me.damage
		weaponChargeProjectile.PlayerId = Me.player.id
		weaponChargeProjectile.DamagesType.PlayerProjectileDefault()
		weaponChargeProjectile.CollisionDeath.PlayerProjectileDefault()
		If Me.fullyCharged AndAlso Me.player.motor.Ducking Then
			weaponChargeProjectile.CollisionDeath.Ground = False
			weaponChargeProjectile.CollisionDeath.Walls = False
			weaponChargeProjectile.CollisionDeath.Other = False
		End If
		weaponChargeProjectile.fullyCharged = Me.fullyCharged
		weaponChargeProjectile.animator.SetBool("FullCharge", Me.fullyCharged)
		If Me.chargeEffect IsNot Nothing Then
			Global.UnityEngine.[Object].Destroy(Me.chargeEffect.gameObject)
			Me.chargeEffect = Nothing
			Me.timeCharged = 0F
		End If
		If Me.fullyCharged Then
			Dim effect As Effect = Me.fullChargeFx.Create(weaponChargeProjectile.transform.position)
			effect.transform.eulerAngles = New Vector3(0F, 0F, Me.weaponManager.GetBulletRotation())
			Me.BasicSoundOneShot("player_weapon_charge_full_fireball", "player_weapon_charge_full_fireball_p2")
			Me.AllowChargeSound = True
		Else
			Me.BasicSoundOneShot("player_weapon_charge_fire_small", "player_weapon_charge_fire_small_p2")
		End If
		Return weaponChargeProjectile
	End Function

	' Token: 0x06003FCF RID: 16335 RVA: 0x0022D3BC File Offset: 0x0022B7BC
	Protected Overrides Function fireEx() As AbstractProjectile
		Dim weaponChargeExBurst As WeaponChargeExBurst = TryCast(MyBase.fireEx(), WeaponChargeExBurst)
		Dim vector As Vector2 = 125F * MathUtils.AngleToDirection(weaponChargeExBurst.transform.eulerAngles.z)
		weaponChargeExBurst.transform.AddPosition(vector.x, vector.y, 0F)
		weaponChargeExBurst.transform.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(0F))
		weaponChargeExBurst.transform.SetScale(New Single?(CSng(If((Not Rand.Bool()), 1, (-1)))), Nothing, Nothing)
		weaponChargeExBurst.PlayerId = Me.player.id
		weaponChargeExBurst.Damage = WeaponProperties.LevelWeaponCharge.Ex.damage
		Dim meterScoreTracker As MeterScoreTracker = New MeterScoreTracker(MeterScoreTracker.Type.Ex)
		meterScoreTracker.Add(weaponChargeExBurst)
		Return weaponChargeExBurst
	End Function

	' Token: 0x06003FD0 RID: 16336 RVA: 0x0022D4A0 File Offset: 0x0022B8A0
	Public Overrides Sub BeginBasic()
		If Me.fullyCharged Then
			Me.BeginBasicCheckAttenuation("player_weapon_charge_full_fireball", "player_weapon_charge_full_fireball_p2")
		Else
			Me.BeginBasicCheckAttenuation("player_weapon_charge_fire_small", "player_weapon_charge_fire_small_p2")
		End If
		MyBase.BeginBasic()
	End Sub

	' Token: 0x06003FD1 RID: 16337 RVA: 0x0022D4D8 File Offset: 0x0022B8D8
	Public Overrides Sub EndBasic()
		If Me.fullyCharged Then
			Me.EndBasicCheckAttenuation("player_weapon_charge_full_fireball", "player_weapon_charge_full_fireball_p2")
		Else
			Me.EndBasicCheckAttenuation("player_weapon_charge_fire_small", "player_weapon_charge_fire_small_p2")
		End If
		MyBase.EndBasic()
	End Sub

	' Token: 0x06003FD2 RID: 16338 RVA: 0x0022D510 File Offset: 0x0022B910
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.chargeEffectPrefab = Nothing
		Me.fullChargeFx = Nothing
	End Sub

	' Token: 0x040046A9 RID: 18089
	<SerializeField()>
	Private chargeEffectPrefab As WeaponChargeChargingEffect

	' Token: 0x040046AA RID: 18090
	<SerializeField()>
	Private fullChargeFx As Effect

	' Token: 0x040046AB RID: 18091
	Private chargeEffect As WeaponChargeChargingEffect

	' Token: 0x040046AC RID: 18092
	Private fullyCharged As Boolean

	' Token: 0x040046AD RID: 18093
	Private timeCharged As Single

	' Token: 0x040046AE RID: 18094
	Private damageState As Integer

	' Token: 0x040046AF RID: 18095
	Private damage As Single

	' Token: 0x040046B0 RID: 18096
	Private AllowChargeSound As Boolean = True
End Class
