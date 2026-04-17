Imports System
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x02000A7C RID: 2684
Public Class WeaponHoming
	Inherits AbstractLevelWeapon

	' Token: 0x1700058B RID: 1419
	' (get) Token: 0x06004026 RID: 16422 RVA: 0x0022FB41 File Offset: 0x0022DF41
	Protected Overrides ReadOnly Property rapidFire As Boolean
		Get
			Return True
		End Get
	End Property

	' Token: 0x1700058C RID: 1420
	' (get) Token: 0x06004027 RID: 16423 RVA: 0x0022FB44 File Offset: 0x0022DF44
	Protected Overrides ReadOnly Property rapidFireRate As Single
		Get
			Return Me.fireRate
		End Get
	End Property

	' Token: 0x06004028 RID: 16424 RVA: 0x0022FB4C File Offset: 0x0022DF4C
	Private Sub FixedUpdate()
		If Me.player.motor.Locked Then
			Me.fireRateLockRatio = Mathf.Clamp01(Me.fireRateLockRatio + CupheadTime.FixedDelta / WeaponProperties.LevelWeaponHoming.Basic.lockedShotAccelerationTime)
		Else
			Me.fireRateLockRatio = 0F
		End If
		Me.fireRate = WeaponProperties.LevelWeaponHoming.Basic.fireRate.GetFloatAt(1F - Me.fireRateLockRatio)
	End Sub

	' Token: 0x06004029 RID: 16425 RVA: 0x0022FBB8 File Offset: 0x0022DFB8
	Protected Overrides Function fireBasic() As AbstractProjectile
		Dim weaponHomingProjectile As WeaponHomingProjectile = TryCast(MyBase.fireBasic(), WeaponHomingProjectile)
		weaponHomingProjectile.rotation = weaponHomingProjectile.transform.rotation.eulerAngles.z + Global.UnityEngine.Random.Range(-WeaponProperties.LevelWeaponHoming.Basic.angleVariation, WeaponProperties.LevelWeaponHoming.Basic.angleVariation)
		weaponHomingProjectile.speed = WeaponProperties.LevelWeaponHoming.Basic.speed + Global.UnityEngine.Random.Range(-WeaponProperties.LevelWeaponHoming.Basic.speedVariation, WeaponProperties.LevelWeaponHoming.Basic.speedVariation)
		weaponHomingProjectile.rotationSpeed = WeaponProperties.LevelWeaponHoming.Basic.rotationSpeed
		weaponHomingProjectile.rotationSpeedEaseTime = WeaponProperties.LevelWeaponHoming.Basic.rotationSpeedEaseTime
		weaponHomingProjectile.timeBeforeEaseRotationSpeed = WeaponProperties.LevelWeaponHoming.Basic.timeBeforeEaseRotationSpeed
		weaponHomingProjectile.Damage = WeaponProperties.LevelWeaponHoming.Basic.damage
		weaponHomingProjectile.PlayerId = Me.player.id
		weaponHomingProjectile.DamagesType.PlayerProjectileDefault()
		weaponHomingProjectile.CollisionDeath.PlayerProjectileDefault()
		weaponHomingProjectile.trailFollowFrames = Mathf.Clamp(WeaponProperties.LevelWeaponHoming.Basic.trailFrameDelay, 1, 10)
		If Global.UnityEngine.Random.Range(0, 4) = 0 Then
			weaponHomingProjectile.transform.SetScale(New Single?(0.8F), New Single?(0.8F), Nothing)
		End If
		If MathUtils.RandomBool() Then
			weaponHomingProjectile.transform.SetScale(New Single?(-weaponHomingProjectile.transform.localScale.x), Nothing, Nothing)
		End If
		weaponHomingProjectile.FindTarget()
		Return weaponHomingProjectile
	End Function

	' Token: 0x0600402A RID: 16426 RVA: 0x0022FD04 File Offset: 0x0022E104
	Protected Overrides Function fireEx() As AbstractProjectile
		For Each weaponHomingProjectile As WeaponHomingProjectile In Me.swirlingProjectiles
			If weaponHomingProjectile IsNot Nothing Then
				weaponHomingProjectile.StopSwirling()
			End If
		Next
		Dim meterScoreTracker As MeterScoreTracker = New MeterScoreTracker(MeterScoreTracker.Type.Ex)
		Me.swirlingProjectiles.Clear()
		For i As Integer = 0 To WeaponProperties.LevelWeaponHoming.Ex.bulletCount - 1
			Dim weaponHomingProjectile2 As WeaponHomingProjectile = TryCast(MyBase.fireEx(), WeaponHomingProjectile)
			weaponHomingProjectile2.speed = WeaponProperties.LevelWeaponHoming.Ex.speed
			weaponHomingProjectile2.rotationSpeed = WeaponProperties.LevelWeaponHoming.Basic.rotationSpeed
			weaponHomingProjectile2.rotationSpeedEaseTime = WeaponProperties.LevelWeaponHoming.Basic.rotationSpeedEaseTime
			weaponHomingProjectile2.timeBeforeEaseRotationSpeed = WeaponProperties.LevelWeaponHoming.Basic.timeBeforeEaseRotationSpeed
			weaponHomingProjectile2.Damage = WeaponProperties.LevelWeaponHoming.Ex.damage
			weaponHomingProjectile2.PlayerId = Me.player.id
			weaponHomingProjectile2.DamagesType.PlayerProjectileDefault()
			weaponHomingProjectile2.CollisionDeath.PlayerProjectileDefault()
			weaponHomingProjectile2.CollisionDeath.SetBounds(False)
			weaponHomingProjectile2.swirlDistance = WeaponProperties.LevelWeaponHoming.Ex.swirlDistance
			weaponHomingProjectile2.swirlEaseTime = WeaponProperties.LevelWeaponHoming.Ex.swirlEaseTime
			weaponHomingProjectile2.trailFollowFrames = Mathf.Clamp(WeaponProperties.LevelWeaponHoming.Ex.trailFrameDelay, 1, 10)
			weaponHomingProjectile2.StartSwirling(i, WeaponProperties.LevelWeaponHoming.Ex.bulletCount, WeaponProperties.LevelWeaponHoming.Ex.spread, Me.player)
			weaponHomingProjectile2.isEx = True
			Me.swirlingProjectiles.Add(weaponHomingProjectile2)
			meterScoreTracker.Add(weaponHomingProjectile2)
		Next
		Return Me.swirlingProjectiles(0)
	End Function

	' Token: 0x0600402B RID: 16427 RVA: 0x0022FE84 File Offset: 0x0022E284
	Public Overrides Sub BeginBasic()
		MyBase.BeginBasic()
		AudioManager.Play("player_weapon_homing_fire_start")
		Me.emitAudioFromObject.Add("player_weapon_homing_fire_start")
		Me.BasicSoundLoop("player_weapon_homing_loop", "player_weapon_homing_loop_p2")
	End Sub

	' Token: 0x0600402C RID: 16428 RVA: 0x0022FEB6 File Offset: 0x0022E2B6
	Public Overrides Sub EndBasic()
		MyBase.EndBasic()
		Me.StopLoopSound("player_weapon_homing_loop", "player_weapon_homing_loop_p2")
	End Sub

	' Token: 0x040046EF RID: 18159
	Private fireRate As Single = 1F

	' Token: 0x040046F0 RID: 18160
	Private fireRateLockRatio As Single

	' Token: 0x040046F1 RID: 18161
	Public Shared target As Transform

	' Token: 0x040046F2 RID: 18162
	Private swirlingProjectiles As List(Of WeaponHomingProjectile) = New List(Of WeaponHomingProjectile)()
End Class
