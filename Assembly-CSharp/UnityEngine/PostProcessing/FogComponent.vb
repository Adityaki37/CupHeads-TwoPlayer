Imports System
Imports UnityEngine.Rendering

Namespace UnityEngine.PostProcessing
	' Token: 0x02000BA7 RID: 2983
	Public NotInheritable Class FogComponent
		Inherits PostProcessingComponentCommandBuffer(Of FogModel)

		' Token: 0x17000650 RID: 1616
		' (get) Token: 0x06004873 RID: 18547 RVA: 0x00260C7B File Offset: 0x0025F07B
		Public Overrides ReadOnly Property active As Boolean
			Get
				Return MyBase.model.enabled AndAlso Me.context.isGBufferAvailable AndAlso RenderSettings.fog AndAlso Not Me.context.interrupted
			End Get
		End Property

		' Token: 0x06004874 RID: 18548 RVA: 0x00260CB8 File Offset: 0x0025F0B8
		Public Overrides Function GetName() As String
			Return "Fog"
		End Function

		' Token: 0x06004875 RID: 18549 RVA: 0x00260CBF File Offset: 0x0025F0BF
		Public Overrides Function GetCameraFlags() As DepthTextureMode
			Return DepthTextureMode.Depth
		End Function

		' Token: 0x06004876 RID: 18550 RVA: 0x00260CC2 File Offset: 0x0025F0C2
		Public Overrides Function GetCameraEvent() As CameraEvent
			Return CameraEvent.AfterImageEffectsOpaque
		End Function

		' Token: 0x06004877 RID: 18551 RVA: 0x00260CC8 File Offset: 0x0025F0C8
		Public Overrides Sub PopulateCommandBuffer(cb As CommandBuffer)
			Dim settings As FogModel.Settings = MyBase.model.settings
			Dim material As Material = Me.context.materialFactory.[Get]("Hidden/Post FX/Fog")
			material.shaderKeywords = Nothing
			Dim color As Color = If((Not GraphicsUtils.isLinearColorSpace), RenderSettings.fogColor, RenderSettings.fogColor.linear)
			material.SetColor(FogComponent.Uniforms._FogColor, color)
			material.SetFloat(FogComponent.Uniforms._Density, RenderSettings.fogDensity)
			material.SetFloat(FogComponent.Uniforms._Start, RenderSettings.fogStartDistance)
			material.SetFloat(FogComponent.Uniforms._End, RenderSettings.fogEndDistance)
			Dim fogMode As FogMode = RenderSettings.fogMode
			If fogMode <> FogMode.Linear Then
				If fogMode <> FogMode.Exponential Then
					If fogMode = FogMode.ExponentialSquared Then
						material.EnableKeyword("FOG_EXP2")
					End If
				Else
					material.EnableKeyword("FOG_EXP")
				End If
			Else
				material.EnableKeyword("FOG_LINEAR")
			End If
			Dim renderTextureFormat As RenderTextureFormat = If((Not Me.context.isHdr), RenderTextureFormat.[Default], RenderTextureFormat.DefaultHDR)
			cb.GetTemporaryRT(FogComponent.Uniforms._TempRT, Me.context.width, Me.context.height, 24, FilterMode.Bilinear, renderTextureFormat)
			cb.Blit(BuiltinRenderTextureType.CameraTarget, FogComponent.Uniforms._TempRT)
			cb.Blit(FogComponent.Uniforms._TempRT, BuiltinRenderTextureType.CameraTarget, material, If((Not settings.excludeSkybox), 0, 1))
			cb.ReleaseTemporaryRT(FogComponent.Uniforms._TempRT)
		End Sub

		' Token: 0x04004DEF RID: 19951
		Private Const k_ShaderString As String = "Hidden/Post FX/Fog"

		' Token: 0x02000BA8 RID: 2984
		Private NotInheritable Class Uniforms
			' Token: 0x04004DF0 RID: 19952
			Friend Shared _FogColor As Integer = Shader.PropertyToID("_FogColor")

			' Token: 0x04004DF1 RID: 19953
			Friend Shared _Density As Integer = Shader.PropertyToID("_Density")

			' Token: 0x04004DF2 RID: 19954
			Friend Shared _Start As Integer = Shader.PropertyToID("_Start")

			' Token: 0x04004DF3 RID: 19955
			Friend Shared _End As Integer = Shader.PropertyToID("_End")

			' Token: 0x04004DF4 RID: 19956
			Friend Shared _TempRT As Integer = Shader.PropertyToID("_TempRT")
		End Class
	End Class
End Namespace
