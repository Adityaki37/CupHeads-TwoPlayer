Imports System

Namespace UnityEngine.PostProcessing
	' Token: 0x02000B9D RID: 2973
	Public NotInheritable Class ChromaticAberrationComponent
		Inherits PostProcessingComponentRenderTexture(Of ChromaticAberrationModel)

		' Token: 0x1700064B RID: 1611
		' (get) Token: 0x0600483D RID: 18493 RVA: 0x0025EE30 File Offset: 0x0025D230
		Public Overrides ReadOnly Property active As Boolean
			Get
				Return MyBase.model.enabled AndAlso MyBase.model.settings.intensity > 0F AndAlso Not Me.context.interrupted
			End Get
		End Property

		' Token: 0x0600483E RID: 18494 RVA: 0x0025EE7B File Offset: 0x0025D27B
		Public Overrides Sub OnDisable()
			GraphicsUtils.Destroy(Me.m_SpectrumLut)
			Me.m_SpectrumLut = Nothing
		End Sub

		' Token: 0x0600483F RID: 18495 RVA: 0x0025EE90 File Offset: 0x0025D290
		Public Overrides Sub Prepare(uberMaterial As Material)
			Dim settings As ChromaticAberrationModel.Settings = MyBase.model.settings
			Dim texture2D As Texture2D = settings.spectralTexture
			If texture2D Is Nothing Then
				If Me.m_SpectrumLut Is Nothing Then
					Me.m_SpectrumLut = New Texture2D(3, 1, TextureFormat.RGB24, False) With { .name = "Chromatic Aberration Spectrum Lookup", .filterMode = FilterMode.Bilinear, .wrapMode = TextureWrapMode.Clamp, .anisoLevel = 0, .hideFlags = HideFlags.DontSave }
					Dim array As Color() = New Color() { New Color(1F, 0F, 0F), New Color(0F, 1F, 0F), New Color(0F, 0F, 1F) }
					Me.m_SpectrumLut.SetPixels(array)
					Me.m_SpectrumLut.Apply()
				End If
				texture2D = Me.m_SpectrumLut
			End If
			uberMaterial.EnableKeyword("CHROMATIC_ABERRATION")
			uberMaterial.SetFloat(ChromaticAberrationComponent.Uniforms._ChromaticAberration_Amount, settings.intensity * 0.03F)
			uberMaterial.SetTexture(ChromaticAberrationComponent.Uniforms._ChromaticAberration_Spectrum, texture2D)
		End Sub

		' Token: 0x04004DAF RID: 19887
		Private m_SpectrumLut As Texture2D

		' Token: 0x02000B9E RID: 2974
		Private NotInheritable Class Uniforms
			' Token: 0x04004DB0 RID: 19888
			Friend Shared _ChromaticAberration_Amount As Integer = Shader.PropertyToID("_ChromaticAberration_Amount")

			' Token: 0x04004DB1 RID: 19889
			Friend Shared _ChromaticAberration_Spectrum As Integer = Shader.PropertyToID("_ChromaticAberration_Spectrum")
		End Class
	End Class
End Namespace
