Imports System
Imports UnityEngine.Rendering

Namespace UnityEngine.PostProcessing
	' Token: 0x02000B94 RID: 2964
	Public NotInheritable Class AmbientOcclusionComponent
		Inherits PostProcessingComponentCommandBuffer(Of AmbientOcclusionModel)

		' Token: 0x17000643 RID: 1603
		' (get) Token: 0x0600481B RID: 18459 RVA: 0x0025DC30 File Offset: 0x0025C030
		Private ReadOnly Property occlusionSource As AmbientOcclusionComponent.OcclusionSource
			Get
				If Me.context.isGBufferAvailable AndAlso Not MyBase.model.settings.forceForwardCompatibility Then
					Return AmbientOcclusionComponent.OcclusionSource.GBuffer
				End If
				If MyBase.model.settings.highPrecision AndAlso (Not Me.context.isGBufferAvailable OrElse MyBase.model.settings.forceForwardCompatibility) Then
					Return AmbientOcclusionComponent.OcclusionSource.DepthTexture
				End If
				Return AmbientOcclusionComponent.OcclusionSource.DepthNormalsTexture
			End Get
		End Property

		' Token: 0x17000644 RID: 1604
		' (get) Token: 0x0600481C RID: 18460 RVA: 0x0025DCAC File Offset: 0x0025C0AC
		Private ReadOnly Property ambientOnlySupported As Boolean
			Get
				Return Me.context.isHdr AndAlso MyBase.model.settings.ambientOnly AndAlso Me.context.isGBufferAvailable AndAlso Not MyBase.model.settings.forceForwardCompatibility
			End Get
		End Property

		' Token: 0x17000645 RID: 1605
		' (get) Token: 0x0600481D RID: 18461 RVA: 0x0025DD0C File Offset: 0x0025C10C
		Public Overrides ReadOnly Property active As Boolean
			Get
				Return MyBase.model.enabled AndAlso MyBase.model.settings.intensity > 0F AndAlso Not Me.context.interrupted
			End Get
		End Property

		' Token: 0x0600481E RID: 18462 RVA: 0x0025DD58 File Offset: 0x0025C158
		Public Overrides Function GetCameraFlags() As DepthTextureMode
			Dim depthTextureMode As DepthTextureMode = DepthTextureMode.None
			If Me.occlusionSource = AmbientOcclusionComponent.OcclusionSource.DepthTexture Then
				depthTextureMode = depthTextureMode Or DepthTextureMode.Depth
			End If
			If Me.occlusionSource <> AmbientOcclusionComponent.OcclusionSource.GBuffer Then
				depthTextureMode = depthTextureMode Or DepthTextureMode.DepthNormals
			End If
			Return depthTextureMode
		End Function

		' Token: 0x0600481F RID: 18463 RVA: 0x0025DD87 File Offset: 0x0025C187
		Public Overrides Function GetName() As String
			Return "Ambient Occlusion"
		End Function

		' Token: 0x06004820 RID: 18464 RVA: 0x0025DD8E File Offset: 0x0025C18E
		Public Overrides Function GetCameraEvent() As CameraEvent
			Return If((Not Me.ambientOnlySupported OrElse Me.context.profile.debugViews.IsModeActive(BuiltinDebugViewsModel.Mode.AmbientOcclusion)), CameraEvent.BeforeImageEffectsOpaque, CameraEvent.BeforeReflections)
		End Function

		' Token: 0x06004821 RID: 18465 RVA: 0x0025DDC0 File Offset: 0x0025C1C0
		Public Overrides Sub PopulateCommandBuffer(cb As CommandBuffer)
			Dim settings As AmbientOcclusionModel.Settings = MyBase.model.settings
			Dim material As Material = Me.context.materialFactory.[Get]("Hidden/Post FX/Blit")
			Dim material2 As Material = Me.context.materialFactory.[Get]("Hidden/Post FX/Ambient Occlusion")
			material2.shaderKeywords = Nothing
			material2.SetFloat(AmbientOcclusionComponent.Uniforms._Intensity, settings.intensity)
			material2.SetFloat(AmbientOcclusionComponent.Uniforms._Radius, settings.radius)
			material2.SetFloat(AmbientOcclusionComponent.Uniforms._Downsample, If((Not settings.downsampling), 1F, 0.5F))
			material2.SetInt(AmbientOcclusionComponent.Uniforms._SampleCount, CInt(settings.sampleCount))
			If Not Me.context.isGBufferAvailable AndAlso RenderSettings.fog Then
				material2.SetVector(AmbientOcclusionComponent.Uniforms._FogParams, New Vector3(RenderSettings.fogDensity, RenderSettings.fogStartDistance, RenderSettings.fogEndDistance))
				Dim fogMode As FogMode = RenderSettings.fogMode
				If fogMode <> FogMode.Linear Then
					If fogMode <> FogMode.Exponential Then
						If fogMode = FogMode.ExponentialSquared Then
							material2.EnableKeyword("FOG_EXP2")
						End If
					Else
						material2.EnableKeyword("FOG_EXP")
					End If
				Else
					material2.EnableKeyword("FOG_LINEAR")
				End If
			Else
				material2.EnableKeyword("FOG_OFF")
			End If
			Dim width As Integer = Me.context.width
			Dim height As Integer = Me.context.height
			Dim num As Integer = If((Not settings.downsampling), 1, 2)
			Dim num2 As Integer = AmbientOcclusionComponent.Uniforms._OcclusionTexture1
			cb.GetTemporaryRT(num2, width / num, height / num, 0, FilterMode.Bilinear, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear)
			cb.Blit(Nothing, num2, material2, CInt(Me.occlusionSource))
			Dim occlusionTexture As Integer = AmbientOcclusionComponent.Uniforms._OcclusionTexture2
			cb.GetTemporaryRT(occlusionTexture, width, height, 0, FilterMode.Bilinear, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear)
			cb.SetGlobalTexture(AmbientOcclusionComponent.Uniforms._MainTex, num2)
			cb.Blit(num2, occlusionTexture, material2, If((Me.occlusionSource <> AmbientOcclusionComponent.OcclusionSource.GBuffer), 3, 4))
			cb.ReleaseTemporaryRT(num2)
			num2 = AmbientOcclusionComponent.Uniforms._OcclusionTexture
			cb.GetTemporaryRT(num2, width, height, 0, FilterMode.Bilinear, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear)
			cb.SetGlobalTexture(AmbientOcclusionComponent.Uniforms._MainTex, occlusionTexture)
			cb.Blit(occlusionTexture, num2, material2, 5)
			cb.ReleaseTemporaryRT(occlusionTexture)
			If Me.context.profile.debugViews.IsModeActive(BuiltinDebugViewsModel.Mode.AmbientOcclusion) Then
				cb.SetGlobalTexture(AmbientOcclusionComponent.Uniforms._MainTex, num2)
				cb.Blit(num2, BuiltinRenderTextureType.CameraTarget, material2, 8)
				Me.context.Interrupt()
			ElseIf Me.ambientOnlySupported Then
				cb.SetRenderTarget(Me.m_MRT, BuiltinRenderTextureType.CameraTarget)
				cb.DrawMesh(GraphicsUtils.quad, Matrix4x4.identity, material2, 0, 7)
			Else
				Dim renderTextureFormat As RenderTextureFormat = If((Not Me.context.isHdr), RenderTextureFormat.[Default], RenderTextureFormat.DefaultHDR)
				Dim tempRT As Integer = AmbientOcclusionComponent.Uniforms._TempRT
				cb.GetTemporaryRT(tempRT, Me.context.width, Me.context.height, 0, FilterMode.Bilinear, renderTextureFormat)
				cb.Blit(BuiltinRenderTextureType.CameraTarget, tempRT, material, 0)
				cb.SetGlobalTexture(AmbientOcclusionComponent.Uniforms._MainTex, tempRT)
				cb.Blit(tempRT, BuiltinRenderTextureType.CameraTarget, material2, 6)
				cb.ReleaseTemporaryRT(tempRT)
			End If
			cb.ReleaseTemporaryRT(num2)
		End Sub

		' Token: 0x04004D7F RID: 19839
		Private Const k_BlitShaderString As String = "Hidden/Post FX/Blit"

		' Token: 0x04004D80 RID: 19840
		Private Const k_ShaderString As String = "Hidden/Post FX/Ambient Occlusion"

		' Token: 0x04004D81 RID: 19841
		Private m_MRT As RenderTargetIdentifier() = New RenderTargetIdentifier() { BuiltinRenderTextureType.GBuffer0, BuiltinRenderTextureType.CameraTarget }

		' Token: 0x02000B95 RID: 2965
		Private NotInheritable Class Uniforms
			' Token: 0x04004D82 RID: 19842
			Friend Shared _Intensity As Integer = Shader.PropertyToID("_Intensity")

			' Token: 0x04004D83 RID: 19843
			Friend Shared _Radius As Integer = Shader.PropertyToID("_Radius")

			' Token: 0x04004D84 RID: 19844
			Friend Shared _FogParams As Integer = Shader.PropertyToID("_FogParams")

			' Token: 0x04004D85 RID: 19845
			Friend Shared _Downsample As Integer = Shader.PropertyToID("_Downsample")

			' Token: 0x04004D86 RID: 19846
			Friend Shared _SampleCount As Integer = Shader.PropertyToID("_SampleCount")

			' Token: 0x04004D87 RID: 19847
			Friend Shared _OcclusionTexture1 As Integer = Shader.PropertyToID("_OcclusionTexture1")

			' Token: 0x04004D88 RID: 19848
			Friend Shared _OcclusionTexture2 As Integer = Shader.PropertyToID("_OcclusionTexture2")

			' Token: 0x04004D89 RID: 19849
			Friend Shared _OcclusionTexture As Integer = Shader.PropertyToID("_OcclusionTexture")

			' Token: 0x04004D8A RID: 19850
			Friend Shared _MainTex As Integer = Shader.PropertyToID("_MainTex")

			' Token: 0x04004D8B RID: 19851
			Friend Shared _TempRT As Integer = Shader.PropertyToID("_TempRT")
		End Class

		' Token: 0x02000B96 RID: 2966
		Private Enum OcclusionSource
			' Token: 0x04004D8D RID: 19853
			DepthTexture
			' Token: 0x04004D8E RID: 19854
			DepthNormalsTexture
			' Token: 0x04004D8F RID: 19855
			GBuffer
		End Enum
	End Class
End Namespace
