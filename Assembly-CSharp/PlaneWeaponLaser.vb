Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000ABF RID: 2751
Public Class PlaneWeaponLaser
	Inherits AbstractPlaneWeapon

	' Token: 0x170005C8 RID: 1480
	' (get) Token: 0x06004214 RID: 16916 RVA: 0x0023B346 File Offset: 0x00239746
	Protected Overrides ReadOnly Property rapidFire As Boolean
		Get
			Return WeaponProperties.PlaneWeaponLaser.Basic.rapidFire
		End Get
	End Property

	' Token: 0x170005C9 RID: 1481
	' (get) Token: 0x06004215 RID: 16917 RVA: 0x0023B34D File Offset: 0x0023974D
	Protected Overrides ReadOnly Property rapidFireRate As Single
		Get
			Return WeaponProperties.PlaneWeaponLaser.Basic.rapidFireRate
		End Get
	End Property

	' Token: 0x06004216 RID: 16918 RVA: 0x0023B354 File Offset: 0x00239754
	Protected Overrides Function fireBasic() As AbstractProjectile
		Dim basicProjectile As BasicProjectile = TryCast(MyBase.fireBasic(), BasicProjectile)
		basicProjectile.Speed = WeaponProperties.PlaneWeaponLaser.Basic.speed
		basicProjectile.Damage = WeaponProperties.PlaneWeaponLaser.Basic.damage
		basicProjectile.PlayerId = Me.player.id
		Dim num As Single = Me.yPositions(Me.currentY)
		Me.currentY += 1
		If Me.currentY >= Me.yPositions.Length Then
			Me.currentY = 0
		End If
		basicProjectile.transform.AddPosition(0F, num, 0F)
		If Me.player.Shrunk Then
			basicProjectile.Damage *= Me.shrunkDamageMultiplier
			basicProjectile.transform.AddPosition(0F, num * -0.5F, 0F)
			basicProjectile.DestroyDistance = CSng(Global.UnityEngine.Random.Range(200, 350))
			basicProjectile.DestroyDistanceAnimated = True
		End If
		Return basicProjectile
	End Function

	' Token: 0x06004217 RID: 16919 RVA: 0x0023B43D File Offset: 0x0023983D
	Protected Overrides Function fireEx() As AbstractProjectile
		MyBase.StartCoroutine(Me.ex_cr())
		Return Nothing
	End Function

	' Token: 0x06004218 RID: 16920 RVA: 0x0023B450 File Offset: 0x00239850
	Private Iterator Function ex_cr() As IEnumerator
		For wave As Integer = 0 To WeaponProperties.PlaneWeaponLaser.Ex.counts.Length - 1
			Dim count As Integer = WeaponProperties.PlaneWeaponLaser.Ex.counts(wave)
			Dim angle As Single = WeaponProperties.PlaneWeaponLaser.Ex.angles(wave)
			For i As Integer = 0 To count - 1
				Dim num As Single = Mathf.Lerp(0F, angle, CSng(i) / CSng(count)) - 90F
				Dim basicProjectile As BasicProjectile = TryCast(MyBase.fireEx(), BasicProjectile)
				basicProjectile.transform.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(num))
				basicProjectile.Speed = WeaponProperties.PlaneWeaponLaser.Ex.speed
				basicProjectile.Damage = WeaponProperties.PlaneWeaponLaser.Ex.damage
				basicProjectile.PlayerId = Me.player.id
			Next
			Yield CupheadTime.WaitForSeconds(Me, 0.1F)
		Next
		Return
	End Function

	' Token: 0x04004890 RID: 18576
	Private Const Y_POS As Single = 20F

	' Token: 0x04004891 RID: 18577
	Private yPositions As Single() = New Single() { 20F, -20F }

	' Token: 0x04004892 RID: 18578
	Private currentY As Integer
End Class
