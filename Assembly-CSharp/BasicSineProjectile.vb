Imports System
Imports UnityEngine

' Token: 0x02000AE9 RID: 2793
Public Class BasicSineProjectile
	Inherits BasicProjectile

	' Token: 0x060043AC RID: 17324 RVA: 0x000CD83C File Offset: 0x000CBC3C
	Public Function Create(pos As Vector2, rotation As Single, velocity As Single, sinVelocity As Single, sinSize As Single) As BasicSineProjectile
		Dim basicSineProjectile As BasicSineProjectile = TryCast(Me.Create(pos), BasicSineProjectile)
		basicSineProjectile.velocity = velocity
		basicSineProjectile.rotation = rotation
		basicSineProjectile.sinSize = sinSize
		basicSineProjectile.sinVelocity = sinVelocity
		Return basicSineProjectile
	End Function

	' Token: 0x060043AD RID: 17325 RVA: 0x000CD875 File Offset: 0x000CBC75
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.CalculateSin()
	End Sub

	' Token: 0x060043AE RID: 17326 RVA: 0x000CD884 File Offset: 0x000CBC84
	Private Sub CalculateSin()
		Dim zero As Vector2 = Vector2.zero
		zero.x = (Me.direction.x + MyBase.transform.position.x) / 2F
		zero.y = (Me.direction.y + MyBase.transform.position.y) / 2F
		Dim num As Single = -((Me.direction.x - MyBase.transform.position.x) / (Me.direction.y - MyBase.transform.position.y))
		Dim num2 As Single = zero.y - num * zero.x
		Dim zero2 As Vector2 = Vector2.zero
		zero2.x = zero.x + 1F
		zero2.y = num * zero2.x + num2
		Me.normalized = Vector3.zero
		Me.normalized = zero2 - zero
		Me.normalized.Normalize()
	End Sub

	' Token: 0x060043AF RID: 17327 RVA: 0x000CD99C File Offset: 0x000CBD9C
	Protected Overrides Sub Move()
		Me.direction = MathUtils.AngleToDirection(Me.rotation)
		Dim vector As Vector2 = MyBase.transform.position
		Me.angle += Me.sinVelocity * CupheadTime.Delta
		vector += Me.normalized * Mathf.Sin(Me.angle) * Me.sinSize
		vector += Me.direction * Me.velocity * CupheadTime.Delta
		MyBase.transform.position = vector
	End Sub

	' Token: 0x04004960 RID: 18784
	Protected direction As Vector2

	' Token: 0x04004961 RID: 18785
	Protected normalized As Vector2

	' Token: 0x04004962 RID: 18786
	Public velocity As Single

	' Token: 0x04004963 RID: 18787
	Public sinVelocity As Single

	' Token: 0x04004964 RID: 18788
	Protected angle As Single

	' Token: 0x04004965 RID: 18789
	Public rotation As Single

	' Token: 0x04004966 RID: 18790
	Public sinSize As Single
End Class
