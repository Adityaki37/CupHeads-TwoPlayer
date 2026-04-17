Imports System

' Token: 0x02000A63 RID: 2659
Public Class WeaponAccuracy
	Inherits AbstractLevelWeapon

	' Token: 0x17000575 RID: 1397
	' (get) Token: 0x06003F72 RID: 16242 RVA: 0x0022AE89 File Offset: 0x00229289
	Protected Overrides ReadOnly Property rapidFire As Boolean
		Get
			Return True
		End Get
	End Property

	' Token: 0x17000576 RID: 1398
	' (get) Token: 0x06003F73 RID: 16243 RVA: 0x0022AE8C File Offset: 0x0022928C
	Protected Overrides ReadOnly Property rapidFireRate As Single
		Get
			Return Me.fireRate
		End Get
	End Property

	' Token: 0x06003F74 RID: 16244 RVA: 0x0022AE94 File Offset: 0x00229294
	Private Sub Start()
		Me.level = WeaponAccuracy.Levels.One
		Me.speed = WeaponProperties.LevelWeaponAccuracy.Basic.LvlOneSpeed
		Me.fireRate = WeaponProperties.LevelWeaponAccuracy.Basic.LvlOneFireRate
		Me.size = WeaponProperties.LevelWeaponAccuracy.Basic.LvlOneSize
		Me.damage = WeaponProperties.LevelWeaponAccuracy.Basic.LvlOneDamage
	End Sub

	' Token: 0x06003F75 RID: 16245 RVA: 0x0022AECC File Offset: 0x002292CC
	Protected Overrides Function fireBasic() As AbstractProjectile
		Dim weaponAccuracyProjectile As WeaponAccuracyProjectile = TryCast(MyBase.fireBasic(), WeaponAccuracyProjectile)
		weaponAccuracyProjectile.Speed = Me.speed
		weaponAccuracyProjectile.PlayerId = Me.player.id
		weaponAccuracyProjectile.CollisionDeath.PlayerProjectileDefault()
		weaponAccuracyProjectile.EnemyDeath = AddressOf Me.EnemyHit
		weaponAccuracyProjectile.Damage = Me.damage
		weaponAccuracyProjectile.SetSize(Me.size)
		Return weaponAccuracyProjectile
	End Function

	' Token: 0x06003F76 RID: 16246 RVA: 0x0022AF38 File Offset: 0x00229338
	Protected Overrides Function fireEx() As AbstractProjectile
		Dim weaponAccuracyProjectile As WeaponAccuracyProjectile = TryCast(MyBase.fireEx(), WeaponAccuracyProjectile)
		weaponAccuracyProjectile.Speed = WeaponProperties.LevelWeaponAccuracy.Ex.exSpeed
		weaponAccuracyProjectile.Damage = WeaponProperties.LevelWeaponAccuracy.Ex.exDamage
		weaponAccuracyProjectile.SetSize(WeaponProperties.LevelWeaponAccuracy.Ex.exShotSize)
		weaponAccuracyProjectile.CollisionDeath.PlayerProjectileDefault()
		weaponAccuracyProjectile.PlayerId = Me.player.id
		weaponAccuracyProjectile.EnemyDeath = AddressOf Me.EXEnemyHit
		Dim meterScoreTracker As MeterScoreTracker = New MeterScoreTracker(MeterScoreTracker.Type.Ex)
		meterScoreTracker.Add(weaponAccuracyProjectile)
		Return weaponAccuracyProjectile
	End Function

	' Token: 0x06003F77 RID: 16247 RVA: 0x0022AFAF File Offset: 0x002293AF
	Private Sub EXEnemyHit(exEnemyHit As Boolean)
		If exEnemyHit Then
			Me.shotCounter += WeaponProperties.LevelWeaponAccuracy.Ex.exShotEquivalent
			Me.CheckLevels()
		Else
			Me.shotCounter = 0
			Me.LevelOne()
		End If
	End Sub

	' Token: 0x06003F78 RID: 16248 RVA: 0x0022AFE1 File Offset: 0x002293E1
	Private Sub EnemyHit(enemyHit As Boolean)
		If enemyHit Then
			Me.shotCounter += 1
			Me.CheckLevels()
		Else
			Me.shotCounter = 0
			Me.LevelOne()
		End If
	End Sub

	' Token: 0x06003F79 RID: 16249 RVA: 0x0022B010 File Offset: 0x00229410
	Private Sub CheckLevels()
		Select Case Me.level
			Case WeaponAccuracy.Levels.One
				If Me.shotCounter >= WeaponProperties.LevelWeaponAccuracy.Basic.LvlTwoCounter Then
					Me.LevelTwo()
				End If
			Case WeaponAccuracy.Levels.Two
				If Me.shotCounter >= WeaponProperties.LevelWeaponAccuracy.Basic.LvlThreeCounter Then
					Me.LevelThree()
				End If
			Case WeaponAccuracy.Levels.Three
				If Me.shotCounter >= WeaponProperties.LevelWeaponAccuracy.Basic.LvlFourCounter Then
					Me.LevelFour()
				End If
			Case WeaponAccuracy.Levels.Four
			Case Else
				Me.LevelOne()
		End Select
	End Sub

	' Token: 0x06003F7A RID: 16250 RVA: 0x0022B0A0 File Offset: 0x002294A0
	Private Sub LevelOne()
		Me.level = WeaponAccuracy.Levels.One
		Me.speed = WeaponProperties.LevelWeaponAccuracy.Basic.LvlOneSpeed
		Me.fireRate = WeaponProperties.LevelWeaponAccuracy.Basic.LvlOneFireRate
		Me.size = WeaponProperties.LevelWeaponAccuracy.Basic.LvlOneSize
		Me.damage = WeaponProperties.LevelWeaponAccuracy.Basic.LvlOneDamage
	End Sub

	' Token: 0x06003F7B RID: 16251 RVA: 0x0022B0D5 File Offset: 0x002294D5
	Private Sub LevelTwo()
		Me.level = WeaponAccuracy.Levels.Two
		Me.speed = WeaponProperties.LevelWeaponAccuracy.Basic.LvlTwoSpeed
		Me.fireRate = WeaponProperties.LevelWeaponAccuracy.Basic.LvlTwoFireRate
		Me.size = WeaponProperties.LevelWeaponAccuracy.Basic.LvlTwoSize
		Me.damage = WeaponProperties.LevelWeaponAccuracy.Basic.LvlTwoDamage
	End Sub

	' Token: 0x06003F7C RID: 16252 RVA: 0x0022B10A File Offset: 0x0022950A
	Private Sub LevelThree()
		Me.level = WeaponAccuracy.Levels.Three
		Me.speed = WeaponProperties.LevelWeaponAccuracy.Basic.LvlThreeSpeed
		Me.fireRate = WeaponProperties.LevelWeaponAccuracy.Basic.LvlThreeFireRate
		Me.size = WeaponProperties.LevelWeaponAccuracy.Basic.LvlThreeSize
		Me.damage = WeaponProperties.LevelWeaponAccuracy.Basic.LvlThreeDamage
	End Sub

	' Token: 0x06003F7D RID: 16253 RVA: 0x0022B13F File Offset: 0x0022953F
	Private Sub LevelFour()
		Me.level = WeaponAccuracy.Levels.Four
		Me.speed = WeaponProperties.LevelWeaponAccuracy.Basic.LvlFourSpeed
		Me.fireRate = WeaponProperties.LevelWeaponAccuracy.Basic.LvlFourFireRate
		Me.size = WeaponProperties.LevelWeaponAccuracy.Basic.LvlFourSize
		Me.damage = WeaponProperties.LevelWeaponAccuracy.Basic.LvlFourDamage
	End Sub

	' Token: 0x04004669 RID: 18025
	Private shotCounter As Integer

	' Token: 0x0400466A RID: 18026
	Private level As WeaponAccuracy.Levels

	' Token: 0x0400466B RID: 18027
	Private speed As Single

	' Token: 0x0400466C RID: 18028
	Private fireRate As Single

	' Token: 0x0400466D RID: 18029
	Private size As Single

	' Token: 0x0400466E RID: 18030
	Private damage As Single

	' Token: 0x02000A64 RID: 2660
	Private Enum Levels
		' Token: 0x04004670 RID: 18032
		One
		' Token: 0x04004671 RID: 18033
		Two
		' Token: 0x04004672 RID: 18034
		Three
		' Token: 0x04004673 RID: 18035
		Four
	End Enum
End Class
