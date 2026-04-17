Imports System
Imports UnityEngine

Namespace UnityStandardAssets.ImageEffects
	' Token: 0x02000CE4 RID: 3300
	<ExecuteInEditMode()>
	<RequireComponent(GetType(Camera))>
	<AddComponentMenu("Image Effects/Other/Screen Overlay")>
	Public Class ScreenOverlay
		Inherits PostEffectsBase

		' Token: 0x06005251 RID: 21073 RVA: 0x002A3460 File Offset: 0x002A1860
		Public Overrides Function CheckResources() As Boolean
			MyBase.CheckSupport(False)
			Me.overlayMaterial = MyBase.CheckShaderAndCreateMaterial(Me.overlayShader, Me.overlayMaterial)
			If Not Me.isSupported Then
				MyBase.ReportAutoDisable()
			End If
			Return Me.isSupported
		End Function

		' Token: 0x06005252 RID: 21074 RVA: 0x002A349C File Offset: 0x002A189C
		Private Sub OnRenderImage(source As RenderTexture, destination As RenderTexture)
			If Not Me.CheckResources() Then
				Graphics.Blit(source, destination)
				Return
			End If
			Dim vector As Vector4 = New Vector4(1F, 0F, 0F, 1F)
			Me.overlayMaterial.SetVector("_UV_Transform", vector)
			Me.overlayMaterial.SetFloat("_Intensity", Me.intensity)
			Me.overlayMaterial.SetTexture("_Overlay", Me.texture)
			Graphics.Blit(source, destination, Me.overlayMaterial, CInt(Me.blendMode))
		End Sub

		' Token: 0x040056B2 RID: 22194
		Public blendMode As ScreenOverlay.OverlayBlendMode = ScreenOverlay.OverlayBlendMode.Overlay

		' Token: 0x040056B3 RID: 22195
		Public intensity As Single = 1F

		' Token: 0x040056B4 RID: 22196
		Public texture As Texture2D

		' Token: 0x040056B5 RID: 22197
		Public overlayShader As Shader

		' Token: 0x040056B6 RID: 22198
		Private overlayMaterial As Material

		' Token: 0x02000CE5 RID: 3301
		Public Enum OverlayBlendMode
			' Token: 0x040056B8 RID: 22200
			Additive
			' Token: 0x040056B9 RID: 22201
			ScreenBlend
			' Token: 0x040056BA RID: 22202
			Multiply
			' Token: 0x040056BB RID: 22203
			Overlay
			' Token: 0x040056BC RID: 22204
			AlphaBlend
		End Enum
	End Class
End Namespace
