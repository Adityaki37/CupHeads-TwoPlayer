Imports System
Imports UnityEngine

' Token: 0x020005CB RID: 1483
Public Class DicePalaceFlyingMemoryLevelSpiralProjectile
	Inherits BasicProjectile

	' Token: 0x06001D08 RID: 7432 RVA: 0x0010A8A0 File Offset: 0x00108CA0
	Public Overridable Function Create(position As Vector2, rotation As Single, speed As Single, rotationSpeed As Single, direction As Integer) As BasicProjectile
		Dim dicePalaceFlyingMemoryLevelSpiralProjectile As DicePalaceFlyingMemoryLevelSpiralProjectile = TryCast(MyBase.Create(position, rotation, speed), DicePalaceFlyingMemoryLevelSpiralProjectile)
		dicePalaceFlyingMemoryLevelSpiralProjectile.rotationSpeed = rotationSpeed
		dicePalaceFlyingMemoryLevelSpiralProjectile.rotationBase = New GameObject("SpiralProjectileBase").transform
		dicePalaceFlyingMemoryLevelSpiralProjectile.rotationBase.position = position
		dicePalaceFlyingMemoryLevelSpiralProjectile.transform.parent = dicePalaceFlyingMemoryLevelSpiralProjectile.rotationBase
		dicePalaceFlyingMemoryLevelSpiralProjectile.direction = direction
		Return dicePalaceFlyingMemoryLevelSpiralProjectile
	End Function

	' Token: 0x06001D09 RID: 7433 RVA: 0x0010A904 File Offset: 0x00108D04
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
	End Sub

	' Token: 0x06001D0A RID: 7434 RVA: 0x0010A9B4 File Offset: 0x00108DB4
	Protected Overrides Sub Die()
		MyBase.GetComponent(Of SpriteRenderer)().enabled = False
		MyBase.Die()
	End Sub

	' Token: 0x04002600 RID: 9728
	Private direction As Integer

	' Token: 0x04002601 RID: 9729
	Private rotationSpeed As Single

	' Token: 0x04002602 RID: 9730
	Private rotationBase As Transform
End Class
