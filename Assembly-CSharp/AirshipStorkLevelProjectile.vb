Imports System
Imports UnityEngine

' Token: 0x020004DA RID: 1242
Public Class AirshipStorkLevelProjectile
	Inherits BasicProjectile

	' Token: 0x06001539 RID: 5433 RVA: 0x000BE408 File Offset: 0x000BC808
	Public Overridable Function Create(position As Vector2, rotation As Single, speed As Single, rotationSpeed As Single, direction As Integer) As BasicProjectile
		Dim airshipStorkLevelProjectile As AirshipStorkLevelProjectile = TryCast(MyBase.Create(position, rotation, speed), AirshipStorkLevelProjectile)
		airshipStorkLevelProjectile.rotationSpeed = rotationSpeed
		airshipStorkLevelProjectile.rotationBase = New GameObject("SpiralProjectileBase").transform
		airshipStorkLevelProjectile.rotationBase.position = position
		airshipStorkLevelProjectile.transform.parent = airshipStorkLevelProjectile.rotationBase
		airshipStorkLevelProjectile.direction = direction
		Return airshipStorkLevelProjectile
	End Function

	' Token: 0x0600153A RID: 5434 RVA: 0x000BE46C File Offset: 0x000BC86C
	Protected Overrides Sub Move()
		Dim num As Single = 360F
		If Me.direction = 1 Then
			num = -360F
		ElseIf Me.direction = 2 Then
			num = 360F
		End If
		If Me.Speed = 0F Then
		End If
		MyBase.transform.localPosition += Me.rotationBase.InverseTransformDirection(MyBase.transform.right) * Me.Speed * CupheadTime.FixedDelta
		Me.rotationBase.AddEulerAngles(0F, 0F, Me.rotationSpeed * num * CupheadTime.FixedDelta)
		If MyBase.transform.position.y < -360F OrElse MyBase.transform.position.y > 360F Then
			Me.Die()
		End If
	End Sub

	' Token: 0x0600153B RID: 5435 RVA: 0x000BE55C File Offset: 0x000BC95C
	Protected Overrides Sub Die()
		MyBase.GetComponent(Of SpriteRenderer)().enabled = False
		MyBase.Die()
	End Sub

	' Token: 0x04001E97 RID: 7831
	Private direction As Integer

	' Token: 0x04001E98 RID: 7832
	Private rotationSpeed As Single

	' Token: 0x04001E99 RID: 7833
	Private rotationBase As Transform
End Class
