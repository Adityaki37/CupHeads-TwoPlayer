Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000A8C RID: 2700
Public Class WeaponWideShot
	Inherits AbstractLevelWeapon

	' Token: 0x1700059B RID: 1435
	' (get) Token: 0x06004090 RID: 16528 RVA: 0x00232503 File Offset: 0x00230903
	Protected Overrides ReadOnly Property rapidFire As Boolean
		Get
			Return True
		End Get
	End Property

	' Token: 0x1700059C RID: 1436
	' (get) Token: 0x06004091 RID: 16529 RVA: 0x00232506 File Offset: 0x00230906
	Protected Overrides ReadOnly Property rapidFireRate As Single
		Get
			Return WeaponProperties.LevelWeaponWideShot.Basic.rapidFireRate
		End Get
	End Property

	' Token: 0x06004092 RID: 16530 RVA: 0x0023250D File Offset: 0x0023090D
	Private Sub Start()
		Me.maxAngle = WeaponProperties.LevelWeaponWideShot.Basic.angleRange.max
		MyBase.StartCoroutine(Me.angle_cr())
		Me.isInitialized = True
	End Sub

	' Token: 0x06004093 RID: 16531 RVA: 0x00232533 File Offset: 0x00230933
	Protected Overrides Sub OnEnable()
		MyBase.OnEnable()
		If Me.isInitialized Then
			MyBase.StartCoroutine(Me.angle_cr())
		End If
	End Sub

	' Token: 0x06004094 RID: 16532 RVA: 0x00232553 File Offset: 0x00230953
	Public Overrides Sub BeginBasic()
		MyBase.BeginBasic()
		Me.BasicSoundOneShot("player_wide_shot_start", "player_wide_shot_start_p2")
	End Sub

	' Token: 0x06004095 RID: 16533 RVA: 0x0023256C File Offset: 0x0023096C
	Protected Overrides Function fireBasic() As AbstractProjectile
		Me.BasicSoundOneShot("player_wide_shot_shoot", "player_wide_shot_shoot_p2")
		Dim damage As Single = WeaponProperties.LevelWeaponWideShot.Basic.damage
		Dim basicProjectile As BasicProjectile = Nothing
		Dim minMax As MinMax = New MinMax(0F, Me.maxAngle)
		Me.animationCycleCount += 1
		Dim num As Integer = 0
		While CSng(num) < 3F
			Dim floatAt As Single = minMax.GetFloatAt(CSng(num) / 2F)
			Dim num2 As Single = minMax.max / 2F
			basicProjectile = If((num <> 0), TryCast(MyBase.fireBasicNoEffect(), BasicProjectile), TryCast(MyBase.fireBasic(), BasicProjectile))
			basicProjectile.Speed = WeaponProperties.LevelWeaponWideShot.Basic.speed
			basicProjectile.DestroyDistance = WeaponProperties.LevelWeaponWideShot.Basic.distance - 20F * CSng((num + 1))
			basicProjectile.Damage = damage
			basicProjectile.PlayerId = Me.player.id
			basicProjectile.transform.AddEulerAngles(0F, 0F, floatAt - num2)
			basicProjectile.transform.position += basicProjectile.transform.right * 50F
			basicProjectile.animator.SetInteger("Variant", (Me.animationCycleCount + num) Mod 3)
			num += 1
		End While
		Return basicProjectile
	End Function

	' Token: 0x06004096 RID: 16534 RVA: 0x002326A0 File Offset: 0x00230AA0
	Protected Overrides Function fireEx() As AbstractProjectile
		Dim weaponWideShotExProjectile As WeaponWideShotExProjectile = TryCast(MyBase.fireEx(), WeaponWideShotExProjectile)
		weaponWideShotExProjectile.Damage = WeaponProperties.LevelWeaponWideShot.Ex.exDamage
		weaponWideShotExProjectile.DamageRate = 0F
		weaponWideShotExProjectile.origin = weaponWideShotExProjectile.transform.position
		weaponWideShotExProjectile.mainDuration = WeaponProperties.LevelWeaponWideShot.Ex.exDuration
		weaponWideShotExProjectile.GetComponent(Of BoxCollider2D)().size = New Vector2(2000F, WeaponProperties.LevelWeaponWideShot.Ex.exHeight)
		weaponWideShotExProjectile.PlayerId = Me.player.id
		Dim meterScoreTracker As MeterScoreTracker = New MeterScoreTracker(MeterScoreTracker.Type.Ex)
		meterScoreTracker.Add(weaponWideShotExProjectile)
		Return weaponWideShotExProjectile
	End Function

	' Token: 0x06004097 RID: 16535 RVA: 0x00232728 File Offset: 0x00230B28
	Private Iterator Function angle_cr() As IEnumerator
		Dim openTimeMax As Single = WeaponProperties.LevelWeaponWideShot.Basic.openingAngleSpeed
		Dim closeTimeMax As Single = WeaponProperties.LevelWeaponWideShot.Basic.closingAngleSpeed
		Dim t As Single = 0F
		Dim val As Single = 0F
		Dim playerLocked As Boolean = False
		While True
			If playerLocked Then
				If val < 1F Then
					val = t / closeTimeMax
					t += CupheadTime.Delta
				Else
					val = 1F
					t = 1F
				End If
			ElseIf val > 0F Then
				val = t / openTimeMax
				t -= CupheadTime.Delta
			Else
				val = 0F
				t = 0F
			End If
			playerLocked = Me.player.input.actions.GetButton(6)
			Me.maxAngle = WeaponProperties.LevelWeaponWideShot.Basic.angleRange.GetFloatAt(val)
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x04004751 RID: 18257
	Private maxAngle As Single

	' Token: 0x04004752 RID: 18258
	Private isInitialized As Boolean

	' Token: 0x04004753 RID: 18259
	Private animationCycleCount As Integer
End Class
