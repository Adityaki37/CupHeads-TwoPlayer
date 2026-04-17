Imports System
Imports UnityEngine

Namespace UnityStandardAssets.ImageEffects
	' Token: 0x02000CCB RID: 3275
	<ExecuteInEditMode()>
	<AddComponentMenu("Image Effects/Color Adjustments/Color Correction (Ramp)")>
	Public Class ColorCorrectionRamp
		Inherits ImageEffectBase

		' Token: 0x060051E7 RID: 20967 RVA: 0x0029EE9C File Offset: 0x0029D29C
		Private Sub OnRenderImage(source As RenderTexture, destination As RenderTexture)
			MyBase.material.SetTexture("_RampTex", Me.textureRamp)
			Graphics.Blit(source, destination, MyBase.material)
		End Sub

		' Token: 0x040055EE RID: 21998
		Public textureRamp As Texture
	End Class
End Namespace
