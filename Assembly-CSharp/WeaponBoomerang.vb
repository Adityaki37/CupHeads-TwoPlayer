Imports System
Imports UnityEngine

' Token: 0x02000A6B RID: 2667
Public Class WeaponBoomerang
	Inherits AbstractLevelWeapon

	' Token: 0x1700057B RID: 1403
	' (get) Token: 0x06003FA1 RID: 16289 RVA: 0x0022B9BB File Offset: 0x00229DBB
	Protected Overrides ReadOnly Property rapidFire As Boolean
		Get
			Return True
		End Get
	End Property

	' Token: 0x1700057C RID: 1404
	' (get) Token: 0x06003FA2 RID: 16290 RVA: 0x0022B9BE File Offset: 0x00229DBE
	Protected Overrides ReadOnly Property rapidFireRate As Single
		Get
			Return WeaponProperties.LevelWeaponBoomerang.Basic.fireRate
		End Get
	End Property

	' Token: 0x06003FA3 RID: 16291 RVA: 0x0022B9C8 File Offset: 0x00229DC8
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Dim array As String() = WeaponProperties.LevelWeaponBoomerang.Basic.xDistanceString.Split(New Char() { ","c })
		Dim array2 As String() = WeaponProperties.LevelWeaponBoomerang.Basic.yDistanceString.Split(New Char() { ","c })
		Me.distances = New Vector2(Mathf.Min(array.Length, array2.Length) - 1) {}
		For i As Integer = 0 To Me.distances.Length - 1
			Parser.FloatTryParse(array(i), Me.distances(i).x)
			Parser.FloatTryParse(array2(i), Me.distances(i).y)
		Next
		Me.distanceIndex = Global.UnityEngine.Random.Range(0, Me.distances.Length)
	End Sub

	' Token: 0x06003FA4 RID: 16292 RVA: 0x0022BA7E File Offset: 0x00229E7E
	Public Overrides Sub BeginBasic()
		Me.BeginBasicCheckAttenuation("player_weapon_boomerang", "player_weapon_boomerang_p2")
		MyBase.BeginBasic()
	End Sub

	' Token: 0x06003FA5 RID: 16293 RVA: 0x0022BA98 File Offset: 0x00229E98
	Protected Overrides Function fireBasic() As AbstractProjectile
		Me.BasicSoundOneShot("player_weapon_boomerang", "player_weapon_boomerang_p2")
		Dim weaponBoomerangProjectile As WeaponBoomerangProjectile = TryCast(MyBase.fireBasic(), WeaponBoomerangProjectile)
		weaponBoomerangProjectile.Speed = WeaponProperties.LevelWeaponBoomerang.Basic.speed
		weaponBoomerangProjectile.Damage = WeaponProperties.LevelWeaponBoomerang.Basic.damage
		weaponBoomerangProjectile.PlayerId = Me.player.id
		weaponBoomerangProjectile.DamagesType.PlayerProjectileDefault()
		weaponBoomerangProjectile.CollisionDeath.PlayerProjectileDefault()
		weaponBoomerangProjectile.CollisionDeath.Other = False
		weaponBoomerangProjectile.player = Me.player
		Me.distanceIndex = (Me.distanceIndex + 1) Mod Me.distances.Length
		weaponBoomerangProjectile.forwardDistance = Me.distances(Me.distanceIndex).x
		weaponBoomerangProjectile.lateralDistance = Me.distances(Me.distanceIndex).y
		Return weaponBoomerangProjectile
	End Function

	' Token: 0x06003FA6 RID: 16294 RVA: 0x0022BB67 File Offset: 0x00229F67
	Public Overrides Sub EndBasic()
		MyBase.EndBasic()
		Me.EndBasicCheckAttenuation("player_weapon_boomerang", "player_weapon_boomerang_p2")
	End Sub

	' Token: 0x06003FA7 RID: 16295 RVA: 0x0022BB80 File Offset: 0x00229F80
	Protected Overrides Function fireEx() As AbstractProjectile
		Dim weaponBoomerangProjectile As WeaponBoomerangProjectile = TryCast(MyBase.fireEx(), WeaponBoomerangProjectile)
		weaponBoomerangProjectile.Speed = WeaponProperties.LevelWeaponBoomerang.Ex.speed
		weaponBoomerangProjectile.Damage = WeaponProperties.LevelWeaponBoomerang.Ex.damage
		weaponBoomerangProjectile.maxDamage = WeaponProperties.LevelWeaponBoomerang.Ex.maxDamage * PlayerManager.DamageMultiplier
		weaponBoomerangProjectile.PlayerId = Me.player.id
		weaponBoomerangProjectile.hitFreezeTime = WeaponProperties.LevelWeaponBoomerang.Ex.hitFreezeTime
		weaponBoomerangProjectile.DamageRate = WeaponProperties.LevelWeaponBoomerang.Ex.damageRate + weaponBoomerangProjectile.hitFreezeTime
		weaponBoomerangProjectile.DamagesType.PlayerProjectileDefault()
		weaponBoomerangProjectile.forwardDistance = WeaponProperties.LevelWeaponBoomerang.Ex.xDistance
		weaponBoomerangProjectile.lateralDistance = WeaponProperties.LevelWeaponBoomerang.Ex.yDistance
		weaponBoomerangProjectile.player = Me.player
		weaponBoomerangProjectile.CollisionDeath.Other = False
		Dim meterScoreTracker As MeterScoreTracker = New MeterScoreTracker(MeterScoreTracker.Type.Ex)
		meterScoreTracker.Add(weaponBoomerangProjectile)
		Return weaponBoomerangProjectile
	End Function

	' Token: 0x04004683 RID: 18051
	Private distanceIndex As Integer

	' Token: 0x04004684 RID: 18052
	Private distances As Vector2()
End Class
