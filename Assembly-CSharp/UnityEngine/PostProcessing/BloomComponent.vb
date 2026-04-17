Imports System

Namespace UnityEngine.PostProcessing
	' Token: 0x02000B97 RID: 2967
	Public NotInheritable Class BloomComponent
		Inherits PostProcessingComponentRenderTexture(Of BloomModel)

		' Token: 0x17000646 RID: 1606
		' (get) Token: 0x06004824 RID: 18468 RVA: 0x0025E1F0 File Offset: 0x0025C5F0
		Public Overrides ReadOnly Property active As Boolean
			Get
				Return MyBase.model.enabled AndAlso MyBase.model.settings.bloom.intensity > 0F AndAlso Not Me.context.interrupted
			End Get
		End Property

		' Token: 0x06004825 RID: 18469 RVA: 0x0025E240 File Offset: 0x0025C640
		Public Sub Prepare(source As RenderTexture, uberMaterial As Material, autoExposure As Texture)
			Dim bloom As BloomModel.BloomSettings = MyBase.model.settings.bloom
			Dim lensDirt As BloomModel.LensDirtSettings = MyBase.model.settings.lensDirt
			Dim material As Material = Me.context.materialFactory.[Get]("Hidden/Post FX/Bloom")
			material.shaderKeywords = Nothing
			material.SetTexture(BloomComponent.Uniforms._AutoExposure, autoExposure)
			Dim num As Integer = Me.context.width / 2
			Dim num2 As Integer = Me.context.height / 2
			Dim isMobilePlatform As Boolean = Application.isMobilePlatform
			Dim renderTextureFormat As RenderTextureFormat = If((Not isMobilePlatform), RenderTextureFormat.DefaultHDR, RenderTextureFormat.[Default])
			Dim num3 As Single = Mathf.Log(CSng(num2), 2F) + bloom.radius - 8F
			Dim num4 As Integer = CInt(num3)
			Dim num5 As Integer = Mathf.Clamp(num4, 1, 16)
			Dim thresholdLinear As Single = bloom.thresholdLinear
			material.SetFloat(BloomComponent.Uniforms._Threshold, thresholdLinear)
			Dim num6 As Single = thresholdLinear * bloom.softKnee + 1E-05F
			Dim vector As Vector3 = New Vector3(thresholdLinear - num6, num6 * 2F, 0.25F / num6)
			material.SetVector(BloomComponent.Uniforms._Curve, vector)
			material.SetFloat(BloomComponent.Uniforms._PrefilterOffs, If((Not bloom.antiFlicker), 0F, (-0.5F)))
			Dim num7 As Single = 0.5F + num3 - CSng(num4)
			material.SetFloat(BloomComponent.Uniforms._SampleScale, num7)
			If bloom.antiFlicker Then
				material.EnableKeyword("ANTI_FLICKER")
			End If
			Dim renderTexture As RenderTexture = Me.context.renderTextureFactory.[Get](num, num2, 0, renderTextureFormat, RenderTextureReadWrite.[Default], FilterMode.Bilinear, TextureWrapMode.Clamp, "FactoryTempTexture")
			Graphics.Blit(source, renderTexture, material, 0)
			Dim renderTexture2 As RenderTexture = renderTexture
			For i As Integer = 0 To num5 - 1
				Me.m_BlurBuffer1(i) = Me.context.renderTextureFactory.[Get](renderTexture2.width / 2, renderTexture2.height / 2, 0, renderTextureFormat, RenderTextureReadWrite.[Default], FilterMode.Bilinear, TextureWrapMode.Clamp, "FactoryTempTexture")
				Dim num8 As Integer = If((i <> 0), 2, 1)
				Graphics.Blit(renderTexture2, Me.m_BlurBuffer1(i), material, num8)
				renderTexture2 = Me.m_BlurBuffer1(i)
			Next
			For j As Integer = num5 - 2 To 0 Step -1
				Dim renderTexture3 As RenderTexture = Me.m_BlurBuffer1(j)
				material.SetTexture(BloomComponent.Uniforms._BaseTex, renderTexture3)
				Me.m_BlurBuffer2(j) = Me.context.renderTextureFactory.[Get](renderTexture3.width, renderTexture3.height, 0, renderTextureFormat, RenderTextureReadWrite.[Default], FilterMode.Bilinear, TextureWrapMode.Clamp, "FactoryTempTexture")
				Graphics.Blit(renderTexture2, Me.m_BlurBuffer2(j), material, 3)
				renderTexture2 = Me.m_BlurBuffer2(j)
			Next
			Dim renderTexture4 As RenderTexture = renderTexture2
			For k As Integer = 0 To 16 - 1
				If Me.m_BlurBuffer1(k) IsNot Nothing Then
					Me.context.renderTextureFactory.Release(Me.m_BlurBuffer1(k))
				End If
				If Me.m_BlurBuffer2(k) IsNot Nothing AndAlso Me.m_BlurBuffer2(k) IsNot renderTexture4 Then
					Me.context.renderTextureFactory.Release(Me.m_BlurBuffer2(k))
				End If
				Me.m_BlurBuffer1(k) = Nothing
				Me.m_BlurBuffer2(k) = Nothing
			Next
			Me.context.renderTextureFactory.Release(renderTexture)
			uberMaterial.SetTexture(BloomComponent.Uniforms._BloomTex, renderTexture4)
			uberMaterial.SetVector(BloomComponent.Uniforms._Bloom_Settings, New Vector2(num7, bloom.intensity))
			If lensDirt.intensity > 0F AndAlso lensDirt.texture IsNot Nothing Then
				uberMaterial.SetTexture(BloomComponent.Uniforms._Bloom_DirtTex, lensDirt.texture)
				uberMaterial.SetFloat(BloomComponent.Uniforms._Bloom_DirtIntensity, lensDirt.intensity)
				uberMaterial.EnableKeyword("BLOOM_LENS_DIRT")
			Else
				uberMaterial.EnableKeyword("BLOOM")
			End If
		End Sub

		' Token: 0x04004D90 RID: 19856
		Private Const k_MaxPyramidBlurLevel As Integer = 16

		' Token: 0x04004D91 RID: 19857
		Private m_BlurBuffer1 As RenderTexture() = New RenderTexture(15) {}

		' Token: 0x04004D92 RID: 19858
		Private m_BlurBuffer2 As RenderTexture() = New RenderTexture(15) {}

		' Token: 0x02000B98 RID: 2968
		Private NotInheritable Class Uniforms
			' Token: 0x04004D93 RID: 19859
			Friend Shared _AutoExposure As Integer = Shader.PropertyToID("_AutoExposure")

			' Token: 0x04004D94 RID: 19860
			Friend Shared _Threshold As Integer = Shader.PropertyToID("_Threshold")

			' Token: 0x04004D95 RID: 19861
			Friend Shared _Curve As Integer = Shader.PropertyToID("_Curve")

			' Token: 0x04004D96 RID: 19862
			Friend Shared _PrefilterOffs As Integer = Shader.PropertyToID("_PrefilterOffs")

			' Token: 0x04004D97 RID: 19863
			Friend Shared _SampleScale As Integer = Shader.PropertyToID("_SampleScale")

			' Token: 0x04004D98 RID: 19864
			Friend Shared _BaseTex As Integer = Shader.PropertyToID("_BaseTex")

			' Token: 0x04004D99 RID: 19865
			Friend Shared _BloomTex As Integer = Shader.PropertyToID("_BloomTex")

			' Token: 0x04004D9A RID: 19866
			Friend Shared _Bloom_Settings As Integer = Shader.PropertyToID("_Bloom_Settings")

			' Token: 0x04004D9B RID: 19867
			Friend Shared _Bloom_DirtTex As Integer = Shader.PropertyToID("_Bloom_DirtTex")

			' Token: 0x04004D9C RID: 19868
			Friend Shared _Bloom_DirtIntensity As Integer = Shader.PropertyToID("_Bloom_DirtIntensity")
		End Class
	End Class
End Namespace
