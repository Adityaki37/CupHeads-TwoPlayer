Imports System
Imports UnityEngine

Namespace UnityStandardAssets.ImageEffects
	' Token: 0x02000CDB RID: 3291
	<ExecuteInEditMode()>
	<AddComponentMenu("Image Effects/Color Adjustments/Grayscale")>
	Public Class Grayscale
		Inherits ImageEffectBase

		' Token: 0x06005220 RID: 21024 RVA: 0x002A1EB9 File Offset: 0x002A02B9
		Private Sub OnRenderImage(source As RenderTexture, destination As RenderTexture)
			MyBase.material.SetTexture("_RampTex", Me.textureRamp)
			MyBase.material.SetFloat("_RampOffset", Me.rampOffset)
			Graphics.Blit(source, destination, MyBase.material)
		End Sub

		' Token: 0x04005682 RID: 22146
		Public textureRamp As Texture

		' Token: 0x04005683 RID: 22147
		Public rampOffset As Single
	End Class
End Namespace
