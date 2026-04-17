Imports System
Imports UnityEngine

' Token: 0x020006A3 RID: 1699
Public Class FlyingMermaidLevelTurtleSpiralProjectile
	Inherits BasicProjectile

	' Token: 0x06002406 RID: 9222 RVA: 0x00152640 File Offset: 0x00150A40
	Public Overridable Function Create(position As Vector2, rotation As Single, speed As Single, rotationSpeed As Single) As FlyingMermaidLevelTurtleSpiralProjectile
		Dim flyingMermaidLevelTurtleSpiralProjectile As FlyingMermaidLevelTurtleSpiralProjectile = TryCast(MyBase.Create(position, rotation, speed), FlyingMermaidLevelTurtleSpiralProjectile)
		flyingMermaidLevelTurtleSpiralProjectile.rotationSpeed = rotationSpeed
		flyingMermaidLevelTurtleSpiralProjectile.rotationBase = New GameObject("SpiralProjectileBase")
		flyingMermaidLevelTurtleSpiralProjectile.rotationBase.transform.position = position
		flyingMermaidLevelTurtleSpiralProjectile.transform.parent = flyingMermaidLevelTurtleSpiralProjectile.rotationBase.transform
		flyingMermaidLevelTurtleSpiralProjectile.animator.Play("A", 0, Global.UnityEngine.Random.Range(0F, 1F))
		flyingMermaidLevelTurtleSpiralProjectile.animator.Play("A", 1, Global.UnityEngine.Random.Range(0F, 1F))
		Return flyingMermaidLevelTurtleSpiralProjectile
	End Function

	' Token: 0x06002407 RID: 9223 RVA: 0x001526E4 File Offset: 0x00150AE4
	Protected Overrides Sub Move()
		If Me.Speed = 0F Then
		End If
		MyBase.transform.localPosition += Me.rotationBase.transform.InverseTransformDirection(MyBase.transform.right) * Me.Speed * CupheadTime.FixedDelta
		Me.rotationBase.transform.AddEulerAngles(0F, 0F, Me.rotationSpeed * 360F * CupheadTime.FixedDelta)
	End Sub

	' Token: 0x06002408 RID: 9224 RVA: 0x00152773 File Offset: 0x00150B73
	Protected Overrides Sub Die()
		MyBase.Die()
	End Sub

	' Token: 0x06002409 RID: 9225 RVA: 0x0015277B File Offset: 0x00150B7B
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Global.UnityEngine.[Object].Destroy(Me.rotationBase.gameObject)
	End Sub

	' Token: 0x04002CD4 RID: 11476
	Private rotationSpeed As Single

	' Token: 0x04002CD5 RID: 11477
	Private rotationBase As GameObject
End Class
