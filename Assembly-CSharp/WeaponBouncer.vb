Imports System
Imports UnityEngine

' Token: 0x02000A6D RID: 2669
Public Class WeaponBouncer
	Inherits AbstractLevelWeapon

	' Token: 0x1700057E RID: 1406
	' (get) Token: 0x06003FB5 RID: 16309 RVA: 0x0022C78D File Offset: 0x0022AB8D
	Protected Overrides ReadOnly Property rapidFire As Boolean
		Get
			Return True
		End Get
	End Property

	' Token: 0x1700057F RID: 1407
	' (get) Token: 0x06003FB6 RID: 16310 RVA: 0x0022C790 File Offset: 0x0022AB90
	Protected Overrides ReadOnly Property rapidFireRate As Single
		Get
			Return WeaponProperties.LevelWeaponBouncer.Basic.fireRate
		End Get
	End Property

	' Token: 0x06003FB7 RID: 16311 RVA: 0x0022C798 File Offset: 0x0022AB98
	Protected Overrides Function fireBasic() As AbstractProjectile
		Me.BasicSoundOneShot("player_weapon_bouncer", "player_weapon_bouncer_p2")
		Dim weaponBouncerProjectile As WeaponBouncerProjectile = TryCast(MyBase.fireBasic(), WeaponBouncerProjectile)
		Dim adjustedAngle As Single = Me.getAdjustedAngle(weaponBouncerProjectile.transform.rotation.eulerAngles.z)
		weaponBouncerProjectile.transform.SetEulerAngles(Nothing, Nothing, New Single?(0F))
		weaponBouncerProjectile.transform.SetScale(New Single?(1F), New Single?(1F), New Single?(1F))
		weaponBouncerProjectile.velocity = WeaponProperties.LevelWeaponBouncer.Basic.launchSpeed * MathUtils.AngleToDirection(adjustedAngle)
		weaponBouncerProjectile.gravity = WeaponProperties.LevelWeaponBouncer.Basic.gravity
		weaponBouncerProjectile.bounceRatio = WeaponProperties.LevelWeaponBouncer.Basic.bounceRatio
		weaponBouncerProjectile.bounceSpeedDampening = WeaponProperties.LevelWeaponBouncer.Basic.bounceSpeedDampening
		weaponBouncerProjectile.Damage = WeaponProperties.LevelWeaponBouncer.Basic.damage
		weaponBouncerProjectile.PlayerId = Me.player.id
		weaponBouncerProjectile.weapon = Me
		Return weaponBouncerProjectile
	End Function

	' Token: 0x06003FB8 RID: 16312 RVA: 0x0022C890 File Offset: 0x0022AC90
	Private Function getAdjustedAngle(angle As Single) As Single
		Dim num As Integer = Mathf.RoundToInt(angle)
		If num = 0 Then
			angle += WeaponProperties.LevelWeaponBouncer.Basic.straightExtraAngle
		ElseIf num = 45 Then
			angle += WeaponProperties.LevelWeaponBouncer.Basic.diagonalUpExtraAngle
		ElseIf num = 135 Then
			angle -= WeaponProperties.LevelWeaponBouncer.Basic.diagonalUpExtraAngle
		ElseIf num = 180 Then
			angle -= WeaponProperties.LevelWeaponBouncer.Basic.straightExtraAngle
		ElseIf num = 225 Then
			angle -= WeaponProperties.LevelWeaponBouncer.Basic.diagonalDownExtraAngle
		ElseIf num = 315 Then
			angle += WeaponProperties.LevelWeaponBouncer.Basic.diagonalDownExtraAngle
		End If
		Return angle
	End Function

	' Token: 0x06003FB9 RID: 16313 RVA: 0x0022C930 File Offset: 0x0022AD30
	Protected Overrides Function fireEx() As AbstractProjectile
		Dim weaponBouncerProjectile As WeaponBouncerProjectile = TryCast(MyBase.fireEx(), WeaponBouncerProjectile)
		Dim adjustedAngle As Single = Me.getAdjustedAngle(weaponBouncerProjectile.transform.rotation.eulerAngles.z)
		weaponBouncerProjectile.transform.SetEulerAngles(Nothing, Nothing, New Single?(0F))
		weaponBouncerProjectile.transform.SetScale(New Single?(1F), New Single?(1F), New Single?(1F))
		weaponBouncerProjectile.velocity = WeaponProperties.LevelWeaponBouncer.Basic.launchSpeed * MathUtils.AngleToDirection(adjustedAngle)
		weaponBouncerProjectile.gravity = WeaponProperties.LevelWeaponBouncer.Basic.gravity
		weaponBouncerProjectile.weapon = Me
		weaponBouncerProjectile.Damage = WeaponProperties.LevelWeaponBouncer.Ex.damage
		weaponBouncerProjectile.PlayerId = Me.player.id
		Return weaponBouncerProjectile
	End Function

	' Token: 0x06003FBA RID: 16314 RVA: 0x0022CA02 File Offset: 0x0022AE02
	Public Overrides Sub BeginBasic()
		Me.BeginBasicCheckAttenuation("player_weapon_bouncer", "player_weapon_bouncer_p2")
		MyBase.BeginBasic()
	End Sub

	' Token: 0x06003FBB RID: 16315 RVA: 0x0022CA1A File Offset: 0x0022AE1A
	Public Overrides Sub EndBasic()
		Me.EndBasicCheckAttenuation("player_weapon_bouncer", "player_weapon_bouncer_p2")
		MyBase.EndBasic()
	End Sub
End Class
