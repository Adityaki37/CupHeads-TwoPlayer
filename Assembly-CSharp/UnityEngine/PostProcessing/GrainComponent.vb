Imports System

Namespace UnityEngine.PostProcessing
	' Token: 0x02000BAB RID: 2987
	Public NotInheritable Class GrainComponent
		Inherits PostProcessingComponentRenderTexture(Of GrainModel)

		' Token: 0x17000652 RID: 1618
		' (get) Token: 0x0600487E RID: 18558 RVA: 0x00260FD4 File Offset: 0x0025F3D4
		Public Overrides ReadOnly Property active As Boolean
			Get
				Return MyBase.model.enabled AndAlso MyBase.model.settings.intensity > 0F AndAlso SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf) AndAlso Not Me.context.interrupted
			End Get
		End Property

		' Token: 0x0600487F RID: 18559 RVA: 0x0026102A File Offset: 0x0025F42A
		Public Overrides Sub OnDisable()
			GraphicsUtils.Destroy(Me.m_GrainLookupRT)
			Me.m_GrainLookupRT = Nothing
		End Sub

		' Token: 0x06004880 RID: 18560 RVA: 0x00261040 File Offset: 0x0025F440
		Public Overrides Sub Prepare(uberMaterial As Material)
			Dim settings As GrainModel.Settings = MyBase.model.settings
			uberMaterial.EnableKeyword("GRAIN")
			Dim realtimeSinceStartup As Single = Time.realtimeSinceStartup
			Dim value As Single = Random.value
			Dim value2 As Single = Random.value
			If Me.m_GrainLookupRT Is Nothing OrElse Not Me.m_GrainLookupRT.IsCreated() Then
				GraphicsUtils.Destroy(Me.m_GrainLookupRT)
				Me.m_GrainLookupRT = New RenderTexture(192, 192, 0, RenderTextureFormat.ARGBHalf) With { .filterMode = FilterMode.Bilinear, .wrapMode = TextureWrapMode.Repeat, .anisoLevel = 0, .name = "Grain Lookup Texture" }
				Me.m_GrainLookupRT.Create()
			End If
			Dim material As Material = Me.context.materialFactory.[Get]("Hidden/Post FX/Grain Generator")
			material.SetFloat(GrainComponent.Uniforms._Phase, realtimeSinceStartup / 20F)
			Graphics.Blit(Nothing, Me.m_GrainLookupRT, material, If((Not settings.colored), 0, 1))
			uberMaterial.SetTexture(GrainComponent.Uniforms._GrainTex, Me.m_GrainLookupRT)
			uberMaterial.SetVector(GrainComponent.Uniforms._Grain_Params1, New Vector2(settings.luminanceContribution, settings.intensity * 20F))
			uberMaterial.SetVector(GrainComponent.Uniforms._Grain_Params2, New Vector4(CSng(Me.context.width) / CSng(Me.m_GrainLookupRT.width) / settings.size, CSng(Me.context.height) / CSng(Me.m_GrainLookupRT.height) / settings.size, value, value2))
		End Sub

		' Token: 0x04004DF7 RID: 19959
		Private m_GrainLookupRT As RenderTexture

		' Token: 0x02000BAC RID: 2988
		Private NotInheritable Class Uniforms
			' Token: 0x04004DF8 RID: 19960
			Friend Shared _Grain_Params1 As Integer = Shader.PropertyToID("_Grain_Params1")

			' Token: 0x04004DF9 RID: 19961
			Friend Shared _Grain_Params2 As Integer = Shader.PropertyToID("_Grain_Params2")

			' Token: 0x04004DFA RID: 19962
			Friend Shared _GrainTex As Integer = Shader.PropertyToID("_GrainTex")

			' Token: 0x04004DFB RID: 19963
			Friend Shared _Phase As Integer = Shader.PropertyToID("_Phase")
		End Class
	End Class
End Namespace
