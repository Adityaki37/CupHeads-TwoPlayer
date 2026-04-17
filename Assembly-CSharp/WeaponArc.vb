Imports System
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x02000A67 RID: 2663
Public Class WeaponArc
	Inherits AbstractLevelWeapon

	' Token: 0x17000577 RID: 1399
	' (get) Token: 0x06003F88 RID: 16264 RVA: 0x0022B204 File Offset: 0x00229604
	Protected Overrides ReadOnly Property rapidFire As Boolean
		Get
			Return WeaponProperties.LevelWeaponArc.Basic.rapidFire
		End Get
	End Property

	' Token: 0x17000578 RID: 1400
	' (get) Token: 0x06003F89 RID: 16265 RVA: 0x0022B20B File Offset: 0x0022960B
	Protected Overrides ReadOnly Property rapidFireRate As Single
		Get
			Return WeaponProperties.LevelWeaponArc.Basic.fireRate
		End Get
	End Property

	' Token: 0x06003F8A RID: 16266 RVA: 0x0022B214 File Offset: 0x00229614
	Protected Overrides Function fireBasic() As AbstractProjectile
		AudioManager.Play("player_weapon_arc")
		Me.emitAudioFromObject.Add("player_weapon_arc")
		Dim weaponArcProjectile As WeaponArcProjectile = TryCast(MyBase.fireBasic(), WeaponArcProjectile)
		weaponArcProjectile.PlayerId = Me.player.id
		Dim num As Single = weaponArcProjectile.transform.rotation.eulerAngles.z
		If num = 0F Then
			num += WeaponProperties.LevelWeaponArc.Basic.straightShotAngle
			Me.isDiagonal = False
		ElseIf num = 180F Then
			num -= WeaponProperties.LevelWeaponArc.Basic.straightShotAngle
			Me.isDiagonal = False
		ElseIf Mathf.Approximately(num, 45F) OrElse Mathf.Approximately(num, 135F) Then
			num += WeaponProperties.LevelWeaponArc.Basic.diagShotAngle
			Me.isDiagonal = True
		Else
			Me.isDiagonal = False
		End If
		weaponArcProjectile.transform.SetEulerAngles(Nothing, Nothing, New Single?(num))
		If Me.isDiagonal Then
			weaponArcProjectile.velocity = WeaponProperties.LevelWeaponArc.Basic.diagLaunchSpeed * MathUtils.AngleToDirection(weaponArcProjectile.transform.rotation.eulerAngles.z)
			weaponArcProjectile.gravity = WeaponProperties.LevelWeaponArc.Basic.diagGravity
		Else
			weaponArcProjectile.velocity = WeaponProperties.LevelWeaponArc.Basic.launchSpeed * MathUtils.AngleToDirection(weaponArcProjectile.transform.rotation.eulerAngles.z)
			weaponArcProjectile.gravity = WeaponProperties.LevelWeaponArc.Basic.gravity
		End If
		weaponArcProjectile.weapon = Me
		Return weaponArcProjectile
	End Function

	' Token: 0x06003F8B RID: 16267 RVA: 0x0022B3A4 File Offset: 0x002297A4
	Protected Overrides Function fireEx() As AbstractProjectile
		AudioManager.Play("player_weapon_peashot_ex")
		Dim weaponArcProjectile As WeaponArcProjectile = TryCast(MyBase.fireEx(), WeaponArcProjectile)
		weaponArcProjectile.velocity = WeaponProperties.LevelWeaponArc.Basic.launchSpeed * MathUtils.AngleToDirection(weaponArcProjectile.transform.rotation.eulerAngles.z)
		weaponArcProjectile.gravity = WeaponProperties.LevelWeaponArc.Basic.gravity
		weaponArcProjectile.weapon = Me
		weaponArcProjectile.Damage = WeaponProperties.LevelWeaponArc.Ex.damage
		weaponArcProjectile.PlayerId = Me.player.id
		Dim meterScoreTracker As MeterScoreTracker = New MeterScoreTracker(MeterScoreTracker.Type.Ex)
		meterScoreTracker.Add(weaponArcProjectile)
		Return weaponArcProjectile
	End Function

	' Token: 0x04004676 RID: 18038
	Public projectilesOnGround As List(Of WeaponArcProjectile) = New List(Of WeaponArcProjectile)()

	' Token: 0x04004677 RID: 18039
	Private isDiagonal As Boolean
End Class
