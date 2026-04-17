Imports System
Imports UnityEngine

' Token: 0x02000A88 RID: 2696
Public Class WeaponSpread
	Inherits AbstractLevelWeapon

	' Token: 0x17000594 RID: 1428
	' (get) Token: 0x0600406F RID: 16495 RVA: 0x00231776 File Offset: 0x0022FB76
	Protected Overrides ReadOnly Property rapidFire As Boolean
		Get
			Return True
		End Get
	End Property

	' Token: 0x17000595 RID: 1429
	' (get) Token: 0x06004070 RID: 16496 RVA: 0x00231779 File Offset: 0x0022FB79
	Protected Overrides ReadOnly Property rapidFireRate As Single
		Get
			Return WeaponProperties.LevelWeaponSpreadshot.Basic.rapidFireRate
		End Get
	End Property

	' Token: 0x06004071 RID: 16497 RVA: 0x00231780 File Offset: 0x0022FB80
	Protected Overrides Function fireBasic() As AbstractProjectile
		Dim array As Single() = New Single() { 0.5F, 0.75F }
		Dim damage As Single = WeaponProperties.LevelWeaponSpreadshot.Basic.damage
		For i As Integer = 0 To 2 - 1
			Dim basicProjectile As BasicProjectile = TryCast(MyBase.fireBasicNoEffect(), BasicProjectile)
			basicProjectile.Speed = WeaponProperties.LevelWeaponSpreadshot.Basic.speed * array(i)
			basicProjectile.DestroyDistance = WeaponProperties.LevelWeaponSpreadshot.Basic.distance - 20F * CSng((i + 1))
			basicProjectile.Damage = damage
			basicProjectile.PlayerId = Me.player.id
			basicProjectile.transform.AddEulerAngles(0F, 0F, 15F * CSng((i + 1)))
			Dim component As Animator = basicProjectile.GetComponent(Of Animator)()
			component.SetBool("Large", i = 1)
			Dim basicProjectile2 As BasicProjectile = TryCast(MyBase.fireBasicNoEffect(), BasicProjectile)
			basicProjectile2.Speed = WeaponProperties.LevelWeaponSpreadshot.Basic.speed * array(i)
			basicProjectile2.DestroyDistance = WeaponProperties.LevelWeaponSpreadshot.Basic.distance - 20F * CSng((i + 1))
			basicProjectile2.Damage = damage
			basicProjectile2.PlayerId = Me.player.id
			basicProjectile2.transform.AddEulerAngles(0F, 0F, -15F * CSng((i + 1)))
			Dim component2 As Animator = basicProjectile2.GetComponent(Of Animator)()
			component2.SetBool("Large", i = 1)
		Next
		Dim basicProjectile3 As BasicProjectile = TryCast(MyBase.fireBasic(), BasicProjectile)
		basicProjectile3.Speed = WeaponProperties.LevelWeaponSpreadshot.Basic.speed
		basicProjectile3.Damage = damage
		basicProjectile3.PlayerId = Me.player.id
		basicProjectile3.DestroyDistance = WeaponProperties.LevelWeaponSpreadshot.Basic.distance
		Return basicProjectile3
	End Function

	' Token: 0x06004072 RID: 16498 RVA: 0x00231904 File Offset: 0x0022FD04
	Protected Overrides Function fireEx() As AbstractProjectile
		AudioManager.Play("player_weapon_exploder_fire")
		Dim playerLevelSpreadEx As PlayerLevelSpreadEx = TryCast(MyBase.fireEx(), PlayerLevelSpreadEx)
		playerLevelSpreadEx.Init(WeaponProperties.LevelWeaponSpreadshot.Ex.speed, WeaponProperties.LevelWeaponSpreadshot.Ex.damage, WeaponProperties.LevelWeaponSpreadshot.Ex.childCount, WeaponProperties.LevelWeaponSpreadshot.Ex.radius)
		Return playerLevelSpreadEx
	End Function

	' Token: 0x06004073 RID: 16499 RVA: 0x00231942 File Offset: 0x0022FD42
	Public Overrides Sub BeginBasic()
		MyBase.BeginBasic()
		Me.BasicSoundLoop("player_weapon_spread_loop", "player_weapon_spread_loop_p2")
	End Sub

	' Token: 0x06004074 RID: 16500 RVA: 0x0023195A File Offset: 0x0022FD5A
	Public Overrides Sub EndBasic()
		MyBase.EndBasic()
		Me.StopLoopSound("player_weapon_spread_loop", "player_weapon_spread_loop_p2")
	End Sub
End Class
