Imports System
Imports UnityEngine

' Token: 0x02000ABE RID: 2750
Public Class WeaponChalice3Way
	Inherits AbstractPlaneWeapon

	' Token: 0x170005C6 RID: 1478
	' (get) Token: 0x0600420E RID: 16910 RVA: 0x0023B01F File Offset: 0x0023941F
	Protected Overrides ReadOnly Property rapidFire As Boolean
		Get
			Return True
		End Get
	End Property

	' Token: 0x170005C7 RID: 1479
	' (get) Token: 0x0600420F RID: 16911 RVA: 0x0023B022 File Offset: 0x00239422
	Protected Overrides ReadOnly Property rapidFireRate As Single
		Get
			Return WeaponProperties.PlaneWeaponChaliceWay.Basic.rapidFireRate
		End Get
	End Property

	' Token: 0x06004210 RID: 16912 RVA: 0x0023B029 File Offset: 0x00239429
	Protected Overrides Function GetEffect(mode As AbstractPlaneWeapon.Mode) As Effect
		If mode = AbstractPlaneWeapon.Mode.Basic OrElse mode <> AbstractPlaneWeapon.Mode.Ex Then
			Return If((Me.bulletNumber <> 0), Nothing, Me.basicEffectPrefab)
		End If
		Return Me.exEffectPrefab
	End Function

	' Token: 0x06004211 RID: 16913 RVA: 0x0023B05C File Offset: 0x0023945C
	Protected Overrides Function fireBasic() As AbstractProjectile
		Dim damage As Single = WeaponProperties.PlaneWeaponChaliceWay.Basic.damage
		Dim basicProjectile As BasicProjectile = Nothing
		Dim num As Single = 0F
		Dim angle As Single = WeaponProperties.PlaneWeaponChaliceWay.Basic.angle
		Me.bulletNumber = 0
		While CSng(Me.bulletNumber) < 3F
			If Me.bulletNumber > 0 Then
				num = If((CSng(Me.bulletNumber) < 2F), (-angle), angle)
			End If
			basicProjectile = TryCast(MyBase.fireBasic(), BasicProjectile)
			basicProjectile.Speed = WeaponProperties.PlaneWeaponChaliceWay.Basic.speed
			basicProjectile.DestroyDistance = WeaponProperties.PlaneWeaponChaliceWay.Basic.distance - 20F * CSng((Me.bulletNumber + 1))
			basicProjectile.Damage = damage
			basicProjectile.PlayerId = Me.player.id
			basicProjectile.transform.AddEulerAngles(0F, 0F, num)
			basicProjectile.transform.position += Vector3.right * If((Me.bulletNumber <> 0), (-40F), 40F)
			Dim component As Animator = basicProjectile.GetComponent(Of Animator)()
			component.Play(((Me.bulletNumber + Me.animatorOffset) Mod 3).ToString(), 0, Global.UnityEngine.Random.Range(0F, 1F))
			AudioManager.Play("player_plane_weapon_chalice")
			Me.emitAudioFromObject.Add("player_plane_weapon_chalice")
			Me.bulletNumber += 1
		End While
		Me.animatorOffset += 1
		Return basicProjectile
	End Function

	' Token: 0x06004212 RID: 16914 RVA: 0x0023B1D0 File Offset: 0x002395D0
	Protected Overrides Function fireEx() As AbstractProjectile
		Dim array As PlaneWeaponChalice3WayExProjectile() = New PlaneWeaponChalice3WayExProjectile(1) {}
		For i As Integer = 0 To 2 - 1
			array(i) = TryCast(MyBase.fireEx(), PlaneWeaponChalice3WayExProjectile)
			array(i).Damage = WeaponProperties.PlaneWeaponChaliceWay.Ex.damageBeforeLaunch
			array(i).PlayerId = Me.player.id
			array(i).arcSpeed = WeaponProperties.PlaneWeaponChaliceWay.Ex.arcSpeed
			array(i).arcX = WeaponProperties.PlaneWeaponChaliceWay.Ex.arcX
			array(i).arcY = WeaponProperties.PlaneWeaponChaliceWay.Ex.arcY
			array(i).damageAfterLaunch = WeaponProperties.PlaneWeaponChaliceWay.Ex.damageAfterLaunch
			array(i).pauseTime = WeaponProperties.PlaneWeaponChaliceWay.Ex.pauseTime
			array(i).FreezeTime = WeaponProperties.PlaneWeaponChaliceWay.Ex.freezeTime
			array(i).speedAfterLaunch = WeaponProperties.PlaneWeaponChaliceWay.Ex.speedAfterLaunch
			array(i).accelAfterLaunch = WeaponProperties.PlaneWeaponChaliceWay.Ex.accelAfterLaunch
			array(i).minXDistance = WeaponProperties.PlaneWeaponChaliceWay.Ex.minXDistance
			array(i).xDistanceNoTarget = CSng(WeaponProperties.PlaneWeaponChaliceWay.Ex.xDistanceNoTarget)
			array(i).transform.parent = MyBase.transform
			array(i).SetArcPosition()
			array(i).vDirection = CSng(If((i <> 0), 1, (-1)))
			array(i).DamageRate = WeaponProperties.PlaneWeaponChaliceWay.Ex.damageRateBeforeLaunch
			array(i).CollisionDeath.OnlyBounds()
			array(i).ID = i
			Dim meterScoreTracker As MeterScoreTracker = New MeterScoreTracker(MeterScoreTracker.Type.Ex)
			meterScoreTracker.Add(array(i))
		Next
		array(0).partner = array(1)
		array(1).partner = array(0)
		Return Nothing
	End Function

	' Token: 0x0400488E RID: 18574
	Private animatorOffset As Integer

	' Token: 0x0400488F RID: 18575
	Private bulletNumber As Integer
End Class
