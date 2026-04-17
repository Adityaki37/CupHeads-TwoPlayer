Imports System
Imports UnityEngine

Namespace UnityStandardAssets.ImageEffects
	' Token: 0x02000CCC RID: 3276
	<ExecuteInEditMode()>
	<RequireComponent(GetType(Camera))>
	<AddComponentMenu("Image Effects/Color Adjustments/Contrast Enhance (Unsharp Mask)")>
	Friend Class ContrastEnhance
		Inherits PostEffectsBase

		' Token: 0x060051E9 RID: 20969 RVA: 0x0029EEE0 File Offset: 0x0029D2E0
		Public Overrides Function CheckResources() As Boolean
			MyBase.CheckSupport(False)
			Me.contrastCompositeMaterial = MyBase.CheckShaderAndCreateMaterial(Me.contrastCompositeShader, Me.contrastCompositeMaterial)
			Me.separableBlurMaterial = MyBase.CheckShaderAndCreateMaterial(Me.separableBlurShader, Me.separableBlurMaterial)
			If Not Me.isSupported Then
				MyBase.ReportAutoDisable()
			End If
			Return Me.isSupported
		End Function

		' Token: 0x060051EA RID: 20970 RVA: 0x0029EF3C File Offset: 0x0029D33C
		Private Sub OnRenderImage(source As RenderTexture, destination As RenderTexture)
			If Not Me.CheckResources() Then
				Graphics.Blit(source, destination)
				Return
			End If
			Dim width As Integer = source.width
			Dim height As Integer = source.height
			Dim temporary As RenderTexture = RenderTexture.GetTemporary(width / 2, height / 2, 0)
			Graphics.Blit(source, temporary)
			Dim renderTexture As RenderTexture = RenderTexture.GetTemporary(width / 4, height / 4, 0)
			Graphics.Blit(temporary, renderTexture)
			RenderTexture.ReleaseTemporary(temporary)
			Me.separableBlurMaterial.SetVector("offsets", New Vector4(0F, Me.blurSpread * 1F / CSng(renderTexture.height), 0F, 0F))
			Dim temporary2 As RenderTexture = RenderTexture.GetTemporary(width / 4, height / 4, 0)
			Graphics.Blit(renderTexture, temporary2, Me.separableBlurMaterial)
			RenderTexture.ReleaseTemporary(renderTexture)
			Me.separableBlurMaterial.SetVector("offsets", New Vector4(Me.blurSpread * 1F / CSng(renderTexture.width), 0F, 0F, 0F))
			renderTexture = RenderTexture.GetTemporary(width / 4, height / 4, 0)
			Graphics.Blit(temporary2, renderTexture, Me.separableBlurMaterial)
			RenderTexture.ReleaseTemporary(temporary2)
			Me.contrastCompositeMaterial.SetTexture("_MainTexBlurred", renderTexture)
			Me.contrastCompositeMaterial.SetFloat("intensity", Me.intensity)
			Me.contrastCompositeMaterial.SetFloat("threshhold", Me.threshold)
			Graphics.Blit(source, destination, Me.contrastCompositeMaterial)
			RenderTexture.ReleaseTemporary(renderTexture)
		End Sub

		' Token: 0x040055EF RID: 21999
		Public intensity As Single = 0.5F

		' Token: 0x040055F0 RID: 22000
		Public threshold As Single

		' Token: 0x040055F1 RID: 22001
		Private separableBlurMaterial As Material

		' Token: 0x040055F2 RID: 22002
		Private contrastCompositeMaterial As Material

		' Token: 0x040055F3 RID: 22003
		Public blurSpread As Single = 1F

		' Token: 0x040055F4 RID: 22004
		Public separableBlurShader As Shader

		' Token: 0x040055F5 RID: 22005
		Public contrastCompositeShader As Shader
	End Class
End Namespace
