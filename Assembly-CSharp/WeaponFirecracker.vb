Imports System
Imports UnityEngine

' Token: 0x02000A79 RID: 2681
Public Class WeaponFirecracker
	Inherits AbstractLevelWeapon

	' Token: 0x17000589 RID: 1417
	' (get) Token: 0x06004011 RID: 16401 RVA: 0x0022F0B6 File Offset: 0x0022D4B6
	Protected Overrides ReadOnly Property rapidFire As Boolean
		Get
			Return True
		End Get
	End Property

	' Token: 0x1700058A RID: 1418
	' (get) Token: 0x06004012 RID: 16402 RVA: 0x0022F0B9 File Offset: 0x0022D4B9
	Protected Overrides ReadOnly Property rapidFireRate As Single
		Get
			Return WeaponProperties.LevelWeaponFirecracker.Basic.fireRate
		End Get
	End Property

	' Token: 0x06004013 RID: 16403 RVA: 0x0022F0C0 File Offset: 0x0022D4C0
	Private Sub Start()
		Me.CreateDummyObject()
		Me.explosionAngles = WeaponProperties.LevelWeaponFirecrackerB.Basic.explosionAngleString.Split(New Char() { ","c })
	End Sub

	' Token: 0x06004014 RID: 16404 RVA: 0x0022F0E4 File Offset: 0x0022D4E4
	Protected Overrides Function fireBasic() As AbstractProjectile
		Dim weaponFirecrackerProjectile As WeaponFirecrackerProjectile = TryCast(MyBase.fireBasic(), WeaponFirecrackerProjectile)
		If Me.isTypeB Then
			weaponFirecrackerProjectile.explosionRadiusSize = WeaponProperties.LevelWeaponFirecrackerB.Basic.explosionsRadiusSize
			Dim num As Single = 0F
			Parser.FloatTryParse(Me.explosionAngles(Me.explosionAngleIndex), num)
			weaponFirecrackerProjectile.explosionAngle = num
			Me.explosionAngleIndex = (Me.explosionAngleIndex + 1) Mod Me.explosionAngles.Length
			weaponFirecrackerProjectile.Speed = WeaponProperties.LevelWeaponFirecrackerB.Basic.bulletSpeed
			weaponFirecrackerProjectile.Damage = WeaponProperties.LevelWeaponFirecrackerB.Basic.explosionDamage
			weaponFirecrackerProjectile.bulletLife = WeaponProperties.LevelWeaponFirecrackerB.Basic.bulletLife
			weaponFirecrackerProjectile.explosionSize = WeaponProperties.LevelWeaponFirecrackerB.Basic.explosionSize
			weaponFirecrackerProjectile.explosionDuration = WeaponProperties.LevelWeaponFirecrackerB.Basic.explosionDuration
		Else
			weaponFirecrackerProjectile.Speed = WeaponProperties.LevelWeaponFirecracker.Basic.bulletSpeed
			weaponFirecrackerProjectile.Damage = WeaponProperties.LevelWeaponFirecracker.Basic.explosionDamage
			weaponFirecrackerProjectile.bulletLife = WeaponProperties.LevelWeaponFirecracker.Basic.bulletLife
			weaponFirecrackerProjectile.explosionSize = WeaponProperties.LevelWeaponFirecracker.Basic.explosionSize
			weaponFirecrackerProjectile.explosionDuration = WeaponProperties.LevelWeaponFirecracker.Basic.explosionDuration
		End If
		weaponFirecrackerProjectile.collider.enabled = False
		weaponFirecrackerProjectile.PlayerId = Me.player.id
		weaponFirecrackerProjectile.DamagesType.PlayerProjectileDefault()
		weaponFirecrackerProjectile.CollisionDeath.PlayerProjectileDefault()
		Me.dummyObject.transform.eulerAngles = Me.player.transform.eulerAngles
		Me.dummyObject.transform.localScale = Me.player.transform.localScale
		weaponFirecrackerProjectile.transform.parent = Me.dummyObject.transform
		weaponFirecrackerProjectile.SetupFirecracker(Me.dummyObject.transform, Me.player, Me.isTypeB)
		Return weaponFirecrackerProjectile
	End Function

	' Token: 0x06004015 RID: 16405 RVA: 0x0022F268 File Offset: 0x0022D668
	Protected Overrides Function fireEx() As AbstractProjectile
		Dim weaponFirecrackerEXProjectile As WeaponFirecrackerEXProjectile = TryCast(MyBase.fireEx(), WeaponFirecrackerEXProjectile)
		If Me.isTypeB Then
			weaponFirecrackerEXProjectile.Speed = WeaponProperties.LevelWeaponFirecrackerB.Ex.exSpeed
			weaponFirecrackerEXProjectile.bulletLife = WeaponProperties.LevelWeaponFirecrackerB.Ex.exLife
			weaponFirecrackerEXProjectile.explosionSize = WeaponProperties.LevelWeaponFirecrackerB.Ex.explosionRadius
			weaponFirecrackerEXProjectile.DamageRate = WeaponProperties.LevelWeaponFirecrackerB.Ex.damageRate
			weaponFirecrackerEXProjectile.Damage = WeaponProperties.LevelWeaponFirecrackerB.Ex.explosionDamage
			weaponFirecrackerEXProjectile.explosionDuration = WeaponProperties.LevelWeaponFirecrackerB.Ex.explosionTime
		Else
			weaponFirecrackerEXProjectile.Speed = WeaponProperties.LevelWeaponFirecracker.Ex.exSpeed
			weaponFirecrackerEXProjectile.bulletLife = WeaponProperties.LevelWeaponFirecracker.Ex.exLife
			weaponFirecrackerEXProjectile.explosionSize = WeaponProperties.LevelWeaponFirecracker.Ex.explosionRadius
			weaponFirecrackerEXProjectile.DamageRate = WeaponProperties.LevelWeaponFirecracker.Ex.damageRate
			weaponFirecrackerEXProjectile.Damage = WeaponProperties.LevelWeaponFirecracker.Ex.explosionDamage
			weaponFirecrackerEXProjectile.explosionDuration = WeaponProperties.LevelWeaponFirecracker.Ex.explosionTime
		End If
		Dim meterScoreTracker As MeterScoreTracker = New MeterScoreTracker(MeterScoreTracker.Type.Ex)
		meterScoreTracker.Add(weaponFirecrackerEXProjectile)
		Return weaponFirecrackerEXProjectile
	End Function

	' Token: 0x06004016 RID: 16406 RVA: 0x0022F324 File Offset: 0x0022D724
	Private Sub Update()
		Me.dummyObject.transform.position = Me.player.transform.position
	End Sub

	' Token: 0x06004017 RID: 16407 RVA: 0x0022F346 File Offset: 0x0022D746
	Private Sub CreateDummyObject()
		Me.dummyObject = New GameObject()
		Me.dummyObject.name = "FirecrackerDummyObj"
	End Sub

	' Token: 0x040046D9 RID: 18137
	Public isTypeB As Boolean

	' Token: 0x040046DA RID: 18138
	Private dummyObject As GameObject

	' Token: 0x040046DB RID: 18139
	Private explosionAngles As String()

	' Token: 0x040046DC RID: 18140
	Private explosionAngleIndex As Integer
End Class
