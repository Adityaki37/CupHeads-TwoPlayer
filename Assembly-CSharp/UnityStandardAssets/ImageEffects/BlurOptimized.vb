Imports System
Imports UnityEngine

Namespace UnityStandardAssets.ImageEffects
	' Token: 0x02000CC4 RID: 3268
	<ExecuteInEditMode()>
	<RequireComponent(GetType(Camera))>
	<AddComponentMenu("Image Effects/Blur/Blur (Optimized)")>
	Public Class BlurOptimized
		Inherits PostEffectsBase

		' Token: 0x060051C8 RID: 20936 RVA: 0x0029D3E4 File Offset: 0x0029B7E4
		Public Overrides Function CheckResources() As Boolean
			MyBase.CheckSupport(False)
			Me.blurMaterial = MyBase.CheckShaderAndCreateMaterial(Me.blurShader, Me.blurMaterial)
			If Not Me.isSupported Then
				MyBase.ReportAutoDisable()
			End If
			Return Me.isSupported
		End Function

		' Token: 0x060051C9 RID: 20937 RVA: 0x0029D41D File Offset: 0x0029B81D
		Public Sub OnDisable()
			If Me.blurMaterial Then
				Global.UnityEngine.[Object].DestroyImmediate(Me.blurMaterial)
			End If
		End Sub

		' Token: 0x060051CA RID: 20938 RVA: 0x0029D43C File Offset: 0x0029B83C
		Public Sub OnRenderImage(source As RenderTexture, destination As RenderTexture)
			If Not Me.CheckResources() Then
				Graphics.Blit(source, destination)
				Return
			End If
			Dim num As Single = CSng(destination.width) / CSng(destination.height)
			Dim num2 As Single = If((num >= 1.7777778F), 1F, (num / 1.7777778F))
			num2 *= 1F - 0.1F * SettingsData.Data.overscan
			Dim num3 As Single = CSng(destination.height) / 1080F * 1F / (1F * CSng((1 << Me.downsample)))
			num3 *= num2
			Me.blurMaterial.SetVector("_Parameter", New Vector4(Me.blurSize * num3, -Me.blurSize * num3, 0F, 0F))
			source.filterMode = FilterMode.Bilinear
			Dim num4 As Integer = source.width >> Me.downsample
			Dim num5 As Integer = source.height >> Me.downsample
			Dim renderTexture As RenderTexture = RenderTexture.GetTemporary(num4, num5, 0, source.format)
			renderTexture.filterMode = FilterMode.Bilinear
			Graphics.Blit(source, renderTexture, Me.blurMaterial, 0)
			Dim num6 As Integer = If((Me.blurType <> BlurOptimized.BlurType.StandardGauss), 2, 0)
			For i As Integer = 0 To Me.blurIterations - 1
				Dim num7 As Single = CSng(i) * 1F
				Me.blurMaterial.SetVector("_Parameter", New Vector4(Me.blurSize * num3 + num7, -Me.blurSize * num3 - num7, 0F, 0F))
				Dim renderTexture2 As RenderTexture = RenderTexture.GetTemporary(num4, num5, 0, source.format)
				renderTexture2.filterMode = FilterMode.Bilinear
				Graphics.Blit(renderTexture, renderTexture2, Me.blurMaterial, 1 + num6)
				RenderTexture.ReleaseTemporary(renderTexture)
				renderTexture = renderTexture2
				renderTexture2 = RenderTexture.GetTemporary(num4, num5, 0, source.format)
				renderTexture2.filterMode = FilterMode.Bilinear
				Graphics.Blit(renderTexture, renderTexture2, Me.blurMaterial, 2 + num6)
				RenderTexture.ReleaseTemporary(renderTexture)
				renderTexture = renderTexture2
			Next
			Graphics.Blit(renderTexture, destination)
			RenderTexture.ReleaseTemporary(renderTexture)
		End Sub

		' Token: 0x040055A2 RID: 21922
		<Range(0F, 2F)>
		Public downsample As Integer = 1

		' Token: 0x040055A3 RID: 21923
		<Range(0F, 10F)>
		Public blurSize As Single = 3F

		' Token: 0x040055A4 RID: 21924
		<Range(1F, 4F)>
		Public blurIterations As Integer = 2

		' Token: 0x040055A5 RID: 21925
		Public blurType As BlurOptimized.BlurType

		' Token: 0x040055A6 RID: 21926
		Public blurShader As Shader

		' Token: 0x040055A7 RID: 21927
		Private blurMaterial As Material

		' Token: 0x02000CC5 RID: 3269
		Public Enum BlurType
			' Token: 0x040055A9 RID: 21929
			StandardGauss
			' Token: 0x040055AA RID: 21930
			SgxGauss
		End Enum
	End Class
End Namespace
