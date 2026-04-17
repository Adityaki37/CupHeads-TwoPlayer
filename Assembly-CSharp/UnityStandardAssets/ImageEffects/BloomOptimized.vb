Imports System
Imports UnityEngine

Namespace UnityStandardAssets.ImageEffects
	' Token: 0x02000CC0 RID: 3264
	<ExecuteInEditMode()>
	<RequireComponent(GetType(Camera))>
	<AddComponentMenu("Image Effects/Bloom and Glow/Bloom (Optimized)")>
	Public Class BloomOptimized
		Inherits PostEffectsBase

		' Token: 0x060051BC RID: 20924 RVA: 0x0029CF94 File Offset: 0x0029B394
		Public Overrides Function CheckResources() As Boolean
			MyBase.CheckSupport(False)
			Me.fastBloomMaterial = MyBase.CheckShaderAndCreateMaterial(Me.fastBloomShader, Me.fastBloomMaterial)
			If Not Me.isSupported Then
				MyBase.ReportAutoDisable()
			End If
			Return Me.isSupported
		End Function

		' Token: 0x060051BD RID: 20925 RVA: 0x0029CFCD File Offset: 0x0029B3CD
		Private Sub OnDisable()
			If Me.fastBloomMaterial Then
				Global.UnityEngine.[Object].DestroyImmediate(Me.fastBloomMaterial)
			End If
		End Sub

		' Token: 0x060051BE RID: 20926 RVA: 0x0029CFEC File Offset: 0x0029B3EC
		Private Sub OnRenderImage(source As RenderTexture, destination As RenderTexture)
			If Not Me.CheckResources() Then
				Graphics.Blit(source, destination)
				Return
			End If
			Dim num As Integer = If((Me.resolution <> BloomOptimized.Resolution.Low), 2, 4)
			Dim num2 As Single = If((Me.resolution <> BloomOptimized.Resolution.Low), 1F, 0.5F)
			Me.fastBloomMaterial.SetVector("_Parameter", New Vector4(Me.blurSize * num2, 0F, Me.threshold, Me.intensity))
			source.filterMode = FilterMode.Bilinear
			Dim num3 As Integer = source.width / num
			Dim num4 As Integer = source.height / num
			Dim renderTexture As RenderTexture = RenderTexture.GetTemporary(num3, num4, 0, source.format)
			renderTexture.filterMode = FilterMode.Bilinear
			Graphics.Blit(source, renderTexture, Me.fastBloomMaterial, 1)
			Dim num5 As Integer = If((Me.blurType <> BloomOptimized.BlurType.Standard), 2, 0)
			For i As Integer = 0 To Me.blurIterations - 1
				Me.fastBloomMaterial.SetVector("_Parameter", New Vector4(Me.blurSize * num2 + CSng(i) * 1F, 0F, Me.threshold, Me.intensity))
				Dim renderTexture2 As RenderTexture = RenderTexture.GetTemporary(num3, num4, 0, source.format)
				renderTexture2.filterMode = FilterMode.Bilinear
				Graphics.Blit(renderTexture, renderTexture2, Me.fastBloomMaterial, 2 + num5)
				RenderTexture.ReleaseTemporary(renderTexture)
				renderTexture = renderTexture2
				renderTexture2 = RenderTexture.GetTemporary(num3, num4, 0, source.format)
				renderTexture2.filterMode = FilterMode.Bilinear
				Graphics.Blit(renderTexture, renderTexture2, Me.fastBloomMaterial, 3 + num5)
				RenderTexture.ReleaseTemporary(renderTexture)
				renderTexture = renderTexture2
			Next
			Me.fastBloomMaterial.SetTexture("_Bloom", renderTexture)
			Graphics.Blit(source, destination, Me.fastBloomMaterial, 0)
			RenderTexture.ReleaseTemporary(renderTexture)
		End Sub

		' Token: 0x04005590 RID: 21904
		<Range(0F, 1.5F)>
		Public threshold As Single = 0.25F

		' Token: 0x04005591 RID: 21905
		<Range(0F, 2.5F)>
		Public intensity As Single = 0.75F

		' Token: 0x04005592 RID: 21906
		<Range(0.25F, 5.5F)>
		Public blurSize As Single = 1F

		' Token: 0x04005593 RID: 21907
		Private resolution As BloomOptimized.Resolution

		' Token: 0x04005594 RID: 21908
		<Range(1F, 4F)>
		Public blurIterations As Integer = 1

		' Token: 0x04005595 RID: 21909
		Public blurType As BloomOptimized.BlurType

		' Token: 0x04005596 RID: 21910
		Public fastBloomShader As Shader

		' Token: 0x04005597 RID: 21911
		Private fastBloomMaterial As Material

		' Token: 0x02000CC1 RID: 3265
		Public Enum Resolution
			' Token: 0x04005599 RID: 21913
			Low
			' Token: 0x0400559A RID: 21914
			High
		End Enum

		' Token: 0x02000CC2 RID: 3266
		Public Enum BlurType
			' Token: 0x0400559C RID: 21916
			Standard
			' Token: 0x0400559D RID: 21917
			Sgx
		End Enum
	End Class
End Namespace
