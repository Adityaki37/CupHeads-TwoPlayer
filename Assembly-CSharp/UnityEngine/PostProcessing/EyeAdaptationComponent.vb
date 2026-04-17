Imports System

Namespace UnityEngine.PostProcessing
	' Token: 0x02000BA5 RID: 2981
	Public NotInheritable Class EyeAdaptationComponent
		Inherits PostProcessingComponentRenderTexture(Of EyeAdaptationModel)

		' Token: 0x1700064F RID: 1615
		' (get) Token: 0x0600486A RID: 18538 RVA: 0x0026065F File Offset: 0x0025EA5F
		Public Overrides ReadOnly Property active As Boolean
			Get
				Return MyBase.model.enabled AndAlso SystemInfo.supportsComputeShaders AndAlso Not Me.context.interrupted
			End Get
		End Property

		' Token: 0x0600486B RID: 18539 RVA: 0x0026068C File Offset: 0x0025EA8C
		Public Sub ResetHistory()
			Me.m_FirstFrame = True
		End Sub

		' Token: 0x0600486C RID: 18540 RVA: 0x00260695 File Offset: 0x0025EA95
		Public Overrides Sub OnEnable()
			Me.m_FirstFrame = True
		End Sub

		' Token: 0x0600486D RID: 18541 RVA: 0x002606A0 File Offset: 0x0025EAA0
		Public Overrides Sub OnDisable()
			For Each renderTexture As RenderTexture In Me.m_AutoExposurePool
				GraphicsUtils.Destroy(renderTexture)
			Next
			If Me.m_HistogramBuffer IsNot Nothing Then
				Me.m_HistogramBuffer.Release()
			End If
			Me.m_HistogramBuffer = Nothing
			If Me.m_DebugHistogram IsNot Nothing Then
				Me.m_DebugHistogram.Release()
			End If
			Me.m_DebugHistogram = Nothing
		End Sub

		' Token: 0x0600486E RID: 18542 RVA: 0x00260714 File Offset: 0x0025EB14
		Private Function GetHistogramScaleOffsetRes() As Vector4
			Dim settings As EyeAdaptationModel.Settings = MyBase.model.settings
			Dim num As Single = CSng((settings.logMax - settings.logMin))
			Dim num2 As Single = 1F / num
			Dim num3 As Single = CSng((-CSng(settings.logMin))) * num2
			Return New Vector4(num2, num3, Mathf.Floor(CSng(Me.context.width) / 2F), Mathf.Floor(CSng(Me.context.height) / 2F))
		End Function

		' Token: 0x0600486F RID: 18543 RVA: 0x00260788 File Offset: 0x0025EB88
		Public Function Prepare(source As RenderTexture, uberMaterial As Material) As Texture
			Dim settings As EyeAdaptationModel.Settings = MyBase.model.settings
			If Me.m_EyeCompute Is Nothing Then
				Me.m_EyeCompute = Resources.Load(Of ComputeShader)("Shaders/EyeHistogram")
			End If
			Dim material As Material = Me.context.materialFactory.[Get]("Hidden/Post FX/Eye Adaptation")
			material.shaderKeywords = Nothing
			If Me.m_HistogramBuffer Is Nothing Then
				Me.m_HistogramBuffer = New ComputeBuffer(64, 4)
			End If
			If EyeAdaptationComponent.s_EmptyHistogramBuffer Is Nothing Then
				EyeAdaptationComponent.s_EmptyHistogramBuffer = New UInteger(63) {}
			End If
			Dim histogramScaleOffsetRes As Vector4 = Me.GetHistogramScaleOffsetRes()
			Dim renderTexture As RenderTexture = Me.context.renderTextureFactory.[Get](CInt(histogramScaleOffsetRes.z), CInt(histogramScaleOffsetRes.w), 0, source.format, RenderTextureReadWrite.[Default], FilterMode.Bilinear, TextureWrapMode.Clamp, "FactoryTempTexture")
			Graphics.Blit(source, renderTexture)
			If Me.m_AutoExposurePool(0) Is Nothing OrElse Not Me.m_AutoExposurePool(0).IsCreated() Then
				Me.m_AutoExposurePool(0) = New RenderTexture(1, 1, 0, RenderTextureFormat.RFloat)
			End If
			If Me.m_AutoExposurePool(1) Is Nothing OrElse Not Me.m_AutoExposurePool(1).IsCreated() Then
				Me.m_AutoExposurePool(1) = New RenderTexture(1, 1, 0, RenderTextureFormat.RFloat)
			End If
			Me.m_HistogramBuffer.SetData(EyeAdaptationComponent.s_EmptyHistogramBuffer)
			Dim num As Integer = Me.m_EyeCompute.FindKernel("KEyeHistogram")
			Me.m_EyeCompute.SetBuffer(num, "_Histogram", Me.m_HistogramBuffer)
			Me.m_EyeCompute.SetTexture(num, "_Source", renderTexture)
			Me.m_EyeCompute.SetVector("_ScaleOffsetRes", histogramScaleOffsetRes)
			Me.m_EyeCompute.Dispatch(num, Mathf.CeilToInt(CSng(renderTexture.width) / 16F), Mathf.CeilToInt(CSng(renderTexture.height) / 16F), 1)
			Me.context.renderTextureFactory.Release(renderTexture)
			settings.highPercent = Mathf.Clamp(settings.highPercent, 1.01F, 99F)
			settings.lowPercent = Mathf.Clamp(settings.lowPercent, 1F, settings.highPercent - 0.01F)
			material.SetBuffer("_Histogram", Me.m_HistogramBuffer)
			material.SetVector(EyeAdaptationComponent.Uniforms._Params, New Vector4(settings.lowPercent * 0.01F, settings.highPercent * 0.01F, Mathf.Exp(settings.minLuminance * 0.6931472F), Mathf.Exp(settings.maxLuminance * 0.6931472F)))
			material.SetVector(EyeAdaptationComponent.Uniforms._Speed, New Vector2(settings.speedDown, settings.speedUp))
			material.SetVector(EyeAdaptationComponent.Uniforms._ScaleOffsetRes, histogramScaleOffsetRes)
			material.SetFloat(EyeAdaptationComponent.Uniforms._ExposureCompensation, settings.keyValue)
			If settings.dynamicKeyValue Then
				material.EnableKeyword("AUTO_KEY_VALUE")
			End If
			If Me.m_FirstFrame OrElse Not Application.isPlaying Then
				Me.m_CurrentAutoExposure = Me.m_AutoExposurePool(0)
				Graphics.Blit(Nothing, Me.m_CurrentAutoExposure, material, 1)
				Graphics.Blit(Me.m_AutoExposurePool(0), Me.m_AutoExposurePool(1))
			Else
				Dim num2 As Integer = Me.m_AutoExposurePingPing
				Dim autoExposurePool As RenderTexture() = Me.m_AutoExposurePool
				Dim num3 As Integer = num2 + 1
				num2 = num3
				Dim renderTexture2 As RenderTexture = autoExposurePool(num3 Mod 2)
				Dim autoExposurePool2 As RenderTexture() = Me.m_AutoExposurePool
				Dim num4 As Integer = num2 + 1
				num2 = num4
				Dim renderTexture3 As RenderTexture = autoExposurePool2(num4 Mod 2)
				Graphics.Blit(renderTexture2, renderTexture3, material, CInt(settings.adaptationType))
				Me.m_AutoExposurePingPing = (num2 + 1) Mod 2
				Me.m_CurrentAutoExposure = renderTexture3
			End If
			If Me.context.profile.debugViews.IsModeActive(BuiltinDebugViewsModel.Mode.EyeAdaptation) Then
				If Me.m_DebugHistogram Is Nothing OrElse Not Me.m_DebugHistogram.IsCreated() Then
					Me.m_DebugHistogram = New RenderTexture(256, 128, 0, RenderTextureFormat.ARGB32) With { .filterMode = FilterMode.Point, .wrapMode = TextureWrapMode.Clamp }
				End If
				material.SetFloat(EyeAdaptationComponent.Uniforms._DebugWidth, CSng(Me.m_DebugHistogram.width))
				Graphics.Blit(Nothing, Me.m_DebugHistogram, material, 2)
			End If
			Me.m_FirstFrame = False
			Return Me.m_CurrentAutoExposure
		End Function

		' Token: 0x06004870 RID: 18544 RVA: 0x00260B8C File Offset: 0x0025EF8C
		Public Sub OnGUI()
			If Me.m_DebugHistogram Is Nothing OrElse Not Me.m_DebugHistogram.IsCreated() Then
				Return
			End If
			Dim rect As Rect = New Rect(Me.context.viewport.x * CSng(Screen.width) + 8F, 8F, CSng(Me.m_DebugHistogram.width), CSng(Me.m_DebugHistogram.height))
			GUI.DrawTexture(rect, Me.m_DebugHistogram)
		End Sub

		' Token: 0x04004DDE RID: 19934
		Private m_EyeCompute As ComputeShader

		' Token: 0x04004DDF RID: 19935
		Private m_HistogramBuffer As ComputeBuffer

		' Token: 0x04004DE0 RID: 19936
		Private m_AutoExposurePool As RenderTexture() = New RenderTexture(1) {}

		' Token: 0x04004DE1 RID: 19937
		Private m_AutoExposurePingPing As Integer

		' Token: 0x04004DE2 RID: 19938
		Private m_CurrentAutoExposure As RenderTexture

		' Token: 0x04004DE3 RID: 19939
		Private m_DebugHistogram As RenderTexture

		' Token: 0x04004DE4 RID: 19940
		Private Shared s_EmptyHistogramBuffer As UInteger()

		' Token: 0x04004DE5 RID: 19941
		Private m_FirstFrame As Boolean = True

		' Token: 0x04004DE6 RID: 19942
		Private Const k_HistogramBins As Integer = 64

		' Token: 0x04004DE7 RID: 19943
		Private Const k_HistogramThreadX As Integer = 16

		' Token: 0x04004DE8 RID: 19944
		Private Const k_HistogramThreadY As Integer = 16

		' Token: 0x02000BA6 RID: 2982
		Private NotInheritable Class Uniforms
			' Token: 0x04004DE9 RID: 19945
			Friend Shared _Params As Integer = Shader.PropertyToID("_Params")

			' Token: 0x04004DEA RID: 19946
			Friend Shared _Speed As Integer = Shader.PropertyToID("_Speed")

			' Token: 0x04004DEB RID: 19947
			Friend Shared _ScaleOffsetRes As Integer = Shader.PropertyToID("_ScaleOffsetRes")

			' Token: 0x04004DEC RID: 19948
			Friend Shared _ExposureCompensation As Integer = Shader.PropertyToID("_ExposureCompensation")

			' Token: 0x04004DED RID: 19949
			Friend Shared _AutoExposure As Integer = Shader.PropertyToID("_AutoExposure")

			' Token: 0x04004DEE RID: 19950
			Friend Shared _DebugWidth As Integer = Shader.PropertyToID("_DebugWidth")
		End Class
	End Class
End Namespace
