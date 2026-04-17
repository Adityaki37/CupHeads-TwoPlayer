Imports System
Imports UnityEngine

' Token: 0x02000AC0 RID: 2752
Public Class PlaneWeaponPeashot
	Inherits AbstractPlaneWeapon

	' Token: 0x170005CA RID: 1482
	' (get) Token: 0x0600421B RID: 16923 RVA: 0x0023B613 File Offset: 0x00239A13
	Protected Overrides ReadOnly Property rapidFire As Boolean
		Get
			Return WeaponProperties.PlaneWeaponPeashot.Basic.rapidFire
		End Get
	End Property

	' Token: 0x170005CB RID: 1483
	' (get) Token: 0x0600421C RID: 16924 RVA: 0x0023B61A File Offset: 0x00239A1A
	Protected Overrides ReadOnly Property rapidFireRate As Single
		Get
			Return WeaponProperties.PlaneWeaponPeashot.Basic.rapidFireRate
		End Get
	End Property

	' Token: 0x0600421D RID: 16925 RVA: 0x0023B624 File Offset: 0x00239A24
	Protected Overrides Function fireBasic() As AbstractProjectile
		If(Me.player.id = PlayerId.PlayerOne AndAlso Not PlayerManager.player1IsMugman) OrElse (Me.player.id = PlayerId.PlayerTwo AndAlso PlayerManager.player1IsMugman) Then
			If Not AudioManager.CheckIfPlaying("player_plane_weapon_fire_loop_cuphead") Then
				AudioManager.PlayLoop("player_plane_weapon_fire_loop_cuphead")
			End If
		ElseIf Not AudioManager.CheckIfPlaying("player_plane_weapon_fire_loop_mugman") Then
			AudioManager.PlayLoop("player_plane_weapon_fire_loop_mugman")
		End If
		Me.emitAudioFromObject.Add("player_plane_weapon_fire_loop_cuphead")
		Me.emitAudioFromObject.Add("player_plane_weapon_fire_loop_mugman")
		Dim basicProjectile As BasicProjectile = TryCast(MyBase.fireBasic(), BasicProjectile)
		basicProjectile.Speed = WeaponProperties.PlaneWeaponPeashot.Basic.speed
		basicProjectile.Damage = WeaponProperties.PlaneWeaponPeashot.Basic.damage
		basicProjectile.PlayerId = Me.player.id
		Dim num As Single = Me.yPositions(Me.currentY)
		Me.currentY += 1
		If Me.currentY >= Me.yPositions.Length Then
			Me.currentY = 0
		End If
		basicProjectile.transform.AddPosition(0F, num, 0F)
		Dim component As Animator = basicProjectile.GetComponent(Of Animator)()
		component.SetInteger("Variant", Global.UnityEngine.Random.Range(0, component.GetInteger("MaxVariants")))
		component.SetBool("isCH", (basicProjectile.PlayerId = PlayerId.PlayerOne AndAlso Not PlayerManager.player1IsMugman) OrElse (basicProjectile.PlayerId = PlayerId.PlayerTwo AndAlso PlayerManager.player1IsMugman))
		If Me.player.Shrunk Then
			basicProjectile.Damage *= Me.shrunkDamageMultiplier
			basicProjectile.transform.AddPosition(0F, num * -0.5F, 0F)
			basicProjectile.DestroyDistance = CSng(Global.UnityEngine.Random.Range(200, 350))
			basicProjectile.DestroyDistanceAnimated = True
			basicProjectile.DamageSource = DamageDealer.DamageSource.SmallPlane
		End If
		Return basicProjectile
	End Function

	' Token: 0x0600421E RID: 16926 RVA: 0x0023B800 File Offset: 0x00239C00
	Protected Overrides Function fireEx() As AbstractProjectile
		Dim planeWeaponPeashotExProjectile As PlaneWeaponPeashotExProjectile = TryCast(MyBase.fireEx(), PlaneWeaponPeashotExProjectile)
		planeWeaponPeashotExProjectile.MaxSpeed = WeaponProperties.PlaneWeaponPeashot.Ex.maxSpeed
		planeWeaponPeashotExProjectile.Acceleration = WeaponProperties.PlaneWeaponPeashot.Ex.acceleration
		planeWeaponPeashotExProjectile.FreezeTime = WeaponProperties.PlaneWeaponPeashot.Ex.freezeTime
		planeWeaponPeashotExProjectile.Damage = WeaponProperties.PlaneWeaponPeashot.Ex.damage
		planeWeaponPeashotExProjectile.DamageRate = WeaponProperties.PlaneWeaponPeashot.Ex.freezeTime + WeaponProperties.PlaneWeaponPeashot.Ex.damageDistance / planeWeaponPeashotExProjectile.MaxSpeed
		planeWeaponPeashotExProjectile.PlayerId = Me.player.id
		planeWeaponPeashotExProjectile.speed = Mathf.Clamp(Me.player.motor.Velocity.x, 0F, planeWeaponPeashotExProjectile.MaxSpeed)
		Dim meterScoreTracker As MeterScoreTracker = New MeterScoreTracker(MeterScoreTracker.Type.Ex)
		meterScoreTracker.Add(planeWeaponPeashotExProjectile)
		planeWeaponPeashotExProjectile.Init()
		Return planeWeaponPeashotExProjectile
	End Function

	' Token: 0x04004893 RID: 18579
	Private Const Y_POS As Single = 20F

	' Token: 0x04004894 RID: 18580
	Private yPositions As Single() = New Single() { 20F, -20F }

	' Token: 0x04004895 RID: 18581
	Private currentY As Integer
End Class
