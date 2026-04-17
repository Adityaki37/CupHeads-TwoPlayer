Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000AB4 RID: 2740
Public Class PlaneWeaponBomb
	Inherits AbstractPlaneWeapon

	' Token: 0x170005C1 RID: 1473
	' (get) Token: 0x060041D0 RID: 16848 RVA: 0x002394CA File Offset: 0x002378CA
	Protected Overrides ReadOnly Property rapidFire As Boolean
		Get
			Return WeaponProperties.PlaneWeaponBomb.Basic.rapidFire
		End Get
	End Property

	' Token: 0x170005C2 RID: 1474
	' (get) Token: 0x060041D1 RID: 16849 RVA: 0x002394D1 File Offset: 0x002378D1
	Protected Overrides ReadOnly Property rapidFireRate As Single
		Get
			Return WeaponProperties.PlaneWeaponBomb.Basic.rapidFireRate
		End Get
	End Property

	' Token: 0x060041D2 RID: 16850 RVA: 0x002394D8 File Offset: 0x002378D8
	Protected Overrides Function fireBasic() As AbstractProjectile
		Dim planeWeaponBombProjectile As PlaneWeaponBombProjectile = TryCast(MyBase.fireBasic(), PlaneWeaponBombProjectile)
		planeWeaponBombProjectile.shootsUp = False
		planeWeaponBombProjectile.velocity = WeaponProperties.PlaneWeaponBomb.Basic.speed * MathUtils.AngleToDirection(planeWeaponBombProjectile.transform.rotation.eulerAngles.z)
		planeWeaponBombProjectile.gravity = WeaponProperties.PlaneWeaponBomb.Basic.gravity
		planeWeaponBombProjectile.Damage = WeaponProperties.PlaneWeaponBomb.Basic.damage
		planeWeaponBombProjectile.PlayerId = Me.player.id
		planeWeaponBombProjectile.bulletSize = WeaponProperties.PlaneWeaponBomb.Basic.size
		planeWeaponBombProjectile.explosionSize = WeaponProperties.PlaneWeaponBomb.Basic.sizeExplosion
		planeWeaponBombProjectile.SetAnimation(Me.player.id)
		If WeaponProperties.PlaneWeaponBomb.Basic.Up Then
			Dim planeWeaponBombProjectile2 As PlaneWeaponBombProjectile = TryCast(MyBase.fireBasic(), PlaneWeaponBombProjectile)
			planeWeaponBombProjectile2.shootsUp = True
			planeWeaponBombProjectile2.velocity = WeaponProperties.PlaneWeaponBomb.Basic.speed * MathUtils.AngleToDirection(planeWeaponBombProjectile.transform.rotation.eulerAngles.z)
			planeWeaponBombProjectile2.gravity = WeaponProperties.PlaneWeaponBomb.Basic.gravity
			planeWeaponBombProjectile2.Damage = WeaponProperties.PlaneWeaponBomb.Basic.damage
			planeWeaponBombProjectile2.PlayerId = Me.player.id
			planeWeaponBombProjectile2.bulletSize = WeaponProperties.PlaneWeaponBomb.Basic.size
			planeWeaponBombProjectile2.explosionSize = WeaponProperties.PlaneWeaponBomb.Basic.sizeExplosion
			planeWeaponBombProjectile2.SetAnimation(Me.player.id)
		End If
		Return planeWeaponBombProjectile
	End Function

	' Token: 0x060041D3 RID: 16851 RVA: 0x00239614 File Offset: 0x00237A14
	Protected Overrides Function fireEx() As AbstractProjectile
		MyBase.StartCoroutine(Me.ex_cr())
		Return Nothing
	End Function

	' Token: 0x060041D4 RID: 16852 RVA: 0x00239624 File Offset: 0x00237A24
	Private Iterator Function ex_cr() As IEnumerator
		For wave As Integer = 0 To WeaponProperties.PlaneWeaponBomb.Ex.counts.Length - 1
			Dim count As Integer = WeaponProperties.PlaneWeaponBomb.Ex.counts(wave)
			Dim angle As Single = WeaponProperties.PlaneWeaponBomb.Ex.angles(wave)
			Dim tracker As MeterScoreTracker = New MeterScoreTracker(MeterScoreTracker.Type.Ex)
			For i As Integer = 0 To count - 1
				Dim num As Single = Mathf.Lerp(0F, angle, CSng(i) / CSng(count)) - 90F
				Dim planeWeaponBombExProjectile As PlaneWeaponBombExProjectile = TryCast(MyBase.fireEx(), PlaneWeaponBombExProjectile)
				planeWeaponBombExProjectile.transform.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(num))
				planeWeaponBombExProjectile.rotation = num
				planeWeaponBombExProjectile.speed = WeaponProperties.PlaneWeaponBomb.Ex.speed
				planeWeaponBombExProjectile.Damage = WeaponProperties.PlaneWeaponBomb.Ex.damage
				planeWeaponBombExProjectile.PlayerId = Me.player.id
				planeWeaponBombExProjectile.rotationSpeed = WeaponProperties.PlaneWeaponBomb.Ex.rotationSpeed
				planeWeaponBombExProjectile.rotationSpeedEaseTime = WeaponProperties.PlaneWeaponBomb.Ex.rotationSpeedEaseTime
				planeWeaponBombExProjectile.timeBeforeEaseRotationSpeed = WeaponProperties.PlaneWeaponBomb.Ex.timeBeforeEaseRotationSpeed
				tracker.Add(planeWeaponBombExProjectile)
				planeWeaponBombExProjectile.Init()
				planeWeaponBombExProjectile.FindTarget()
			Next
			Yield CupheadTime.WaitForSeconds(Me, 0.1F)
		Next
		Return
	End Function
End Class
