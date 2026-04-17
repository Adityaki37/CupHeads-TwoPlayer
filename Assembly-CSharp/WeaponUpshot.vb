Imports System
Imports UnityEngine

' Token: 0x02000A89 RID: 2697
Public Class WeaponUpshot
	Inherits AbstractLevelWeapon

	' Token: 0x17000596 RID: 1430
	' (get) Token: 0x06004076 RID: 16502 RVA: 0x00231991 File Offset: 0x0022FD91
	Protected Overrides ReadOnly Property rapidFire As Boolean
		Get
			Return True
		End Get
	End Property

	' Token: 0x17000597 RID: 1431
	' (get) Token: 0x06004077 RID: 16503 RVA: 0x00231994 File Offset: 0x0022FD94
	Protected Overrides ReadOnly Property rapidFireRate As Single
		Get
			Return WeaponProperties.LevelWeaponUpshot.Basic.fireRate
		End Get
	End Property

	' Token: 0x06004078 RID: 16504 RVA: 0x0023199C File Offset: 0x0022FD9C
	Protected Overrides Function fireBasic() As AbstractProjectile
		Me.animationCycleCount += 1
		For i As Integer = 0 To 3 - 1
			Dim weaponUpshotProjectile As WeaponUpshotProjectile = If((i <> 0), TryCast(MyBase.fireBasicNoEffect(), WeaponUpshotProjectile), TryCast(MyBase.fireBasic(), WeaponUpshotProjectile))
			If i = 1 Then
				weaponUpshotProjectile.GetComponent(Of SpriteRenderer)().sortingOrder = 1
			End If
			weaponUpshotProjectile.Damage = WeaponProperties.LevelWeaponUpshot.Basic.damage
			weaponUpshotProjectile.PlayerId = Me.player.id
			weaponUpshotProjectile.DamagesType.PlayerProjectileDefault()
			weaponUpshotProjectile.CollisionDeath.PlayerProjectileDefault()
			weaponUpshotProjectile.CollisionDeath.Other = False
			weaponUpshotProjectile.xSpeed = WeaponProperties.LevelWeaponUpshot.Basic.xSpeed(i)
			weaponUpshotProjectile.ySpeedMinMax = WeaponProperties.LevelWeaponUpshot.Basic.ySpeed(i)
			weaponUpshotProjectile.timeToArc = WeaponProperties.LevelWeaponUpshot.Basic.timeToMaxSpeed(i)
			weaponUpshotProjectile.animator.Play(((Me.animationCycleCount + i) Mod 3).ToString(), 0, Global.UnityEngine.Random.Range(0F, 1F))
		Next
		Return Nothing
	End Function

	' Token: 0x06004079 RID: 16505 RVA: 0x00231A98 File Offset: 0x0022FE98
	Protected Overrides Function fireEx() As AbstractProjectile
		Dim weaponUpshotExProjectile As WeaponUpshotExProjectile = TryCast(MyBase.fireEx(), WeaponUpshotExProjectile)
		weaponUpshotExProjectile.Damage = WeaponProperties.LevelWeaponUpshot.Ex.damage
		weaponUpshotExProjectile.DamageRate = WeaponProperties.LevelWeaponUpshot.Ex.damageRate
		weaponUpshotExProjectile.PlayerId = Me.player.id
		weaponUpshotExProjectile.rotateDir = Mathf.Sign(Me.player.gameObject.transform.localScale.x)
		Dim meterScoreTracker As MeterScoreTracker = New MeterScoreTracker(MeterScoreTracker.Type.Ex)
		meterScoreTracker.Add(weaponUpshotExProjectile)
		Return weaponUpshotExProjectile
	End Function

	' Token: 0x0600407A RID: 16506 RVA: 0x00231B0F File Offset: 0x0022FF0F
	Public Overrides Sub BeginBasic()
		MyBase.BeginBasic()
		AudioManager.Play("player_weapon_upshot_start")
		Me.emitAudioFromObject.Add("player_weapon_upshot_start")
		Me.BasicSoundLoop("player_weapon_upshot_loop_p1", "player_weapon_upshot_loop_p2")
	End Sub

	' Token: 0x0600407B RID: 16507 RVA: 0x00231B41 File Offset: 0x0022FF41
	Public Overrides Sub EndBasic()
		MyBase.EndBasic()
		Me.StopLoopSound("player_weapon_upshot_loop_p1", "player_weapon_upshot_loop_p2")
	End Sub

	' Token: 0x04004737 RID: 18231
	Private Const NUM_OF_BULLETS As Integer = 3

	' Token: 0x04004738 RID: 18232
	Private xOffset As Integer() = New Integer() { -1, 1, 0, 1, -1, 0 }

	' Token: 0x04004739 RID: 18233
	Private xIndex As Integer

	' Token: 0x0400473A RID: 18234
	Private animationCycleCount As Integer
End Class
