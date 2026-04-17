Imports System
Imports UnityEngine

Namespace UnityStandardAssets.ImageEffects
	' Token: 0x02000CF5 RID: 3317
	<ExecuteInEditMode()>
	<RequireComponent(GetType(Camera))>
	<AddComponentMenu("Image Effects/Camera/Vignette and Chromatic Aberration")>
	Public Class VignetteAndChromaticAberration
		Inherits PostEffectsBase

		' Token: 0x06005276 RID: 21110 RVA: 0x002A4D78 File Offset: 0x002A3178
		Public Overrides Function CheckResources() As Boolean
			MyBase.CheckSupport(False)
			Me.m_VignetteMaterial = MyBase.CheckShaderAndCreateMaterial(Me.vignetteShader, Me.m_VignetteMaterial)
			Me.m_SeparableBlurMaterial = MyBase.CheckShaderAndCreateMaterial(Me.separableBlurShader, Me.m_SeparableBlurMaterial)
			Me.m_ChromAberrationMaterial = MyBase.CheckShaderAndCreateMaterial(Me.chromAberrationShader, Me.m_ChromAberrationMaterial)
			If Not Me.isSupported Then
				MyBase.ReportAutoDisable()
			End If
			Return Me.isSupported
		End Function

		' Token: 0x06005277 RID: 21111 RVA: 0x002A4DEC File Offset: 0x002A31EC
		Private Sub OnRenderImage(source As RenderTexture, destination As RenderTexture)
			If Not Me.CheckResources() Then
				Graphics.Blit(source, destination)
				Return
			End If
			Dim width As Integer = source.width
			Dim height As Integer = source.height
			Dim flag As Boolean = Mathf.Abs(Me.blur) > 0F OrElse Mathf.Abs(Me.intensity) > 0F
			Dim num As Single = 1F * CSng(width) / (1F * CSng(height))
			Dim renderTexture As RenderTexture = Nothing
			Dim renderTexture2 As RenderTexture = Nothing
			If flag Then
				renderTexture = RenderTexture.GetTemporary(width, height, 0, source.format)
				If Mathf.Abs(Me.blur) > 0F Then
					renderTexture2 = RenderTexture.GetTemporary(width / 2, height / 2, 0, source.format)
					Graphics.Blit(source, renderTexture2, Me.m_ChromAberrationMaterial, 0)
					For i As Integer = 0 To 2 - 1
						Me.m_SeparableBlurMaterial.SetVector("offsets", New Vector4(0F, Me.blurSpread * 0.001953125F, 0F, 0F))
						Dim temporary As RenderTexture = RenderTexture.GetTemporary(width / 2, height / 2, 0, source.format)
						Graphics.Blit(renderTexture2, temporary, Me.m_SeparableBlurMaterial)
						RenderTexture.ReleaseTemporary(renderTexture2)
						Me.m_SeparableBlurMaterial.SetVector("offsets", New Vector4(Me.blurSpread * 0.001953125F / num, 0F, 0F, 0F))
						renderTexture2 = RenderTexture.GetTemporary(width / 2, height / 2, 0, source.format)
						Graphics.Blit(temporary, renderTexture2, Me.m_SeparableBlurMaterial)
						RenderTexture.ReleaseTemporary(temporary)
					Next
				End If
				Me.m_VignetteMaterial.SetFloat("_Intensity", Me.intensity)
				Me.m_VignetteMaterial.SetFloat("_Blur", Me.blur)
				Me.m_VignetteMaterial.SetTexture("_VignetteTex", renderTexture2)
				Graphics.Blit(source, renderTexture, Me.m_VignetteMaterial, 0)
			End If
			Me.m_ChromAberrationMaterial.SetFloat("_ChromaticAberration", Me.chromaticAberration)
			Me.m_ChromAberrationMaterial.SetFloat("_AxialAberration", Me.axialAberration)
			Me.m_ChromAberrationMaterial.SetVector("_BlurDistance", New Vector2(-Me.blurDistance, Me.blurDistance))
			Me.m_ChromAberrationMaterial.SetFloat("_Luminance", 1F / Mathf.Max(Mathf.Epsilon, Me.luminanceDependency))
			If flag Then
				renderTexture.wrapMode = TextureWrapMode.Clamp
			Else
				source.wrapMode = TextureWrapMode.Clamp
			End If
			Graphics.Blit(If((Not flag), source, renderTexture), destination, Me.m_ChromAberrationMaterial, If((Me.mode <> VignetteAndChromaticAberration.AberrationMode.Advanced), 1, 2))
			RenderTexture.ReleaseTemporary(renderTexture)
			RenderTexture.ReleaseTemporary(renderTexture2)
		End Sub

		' Token: 0x04005719 RID: 22297
		Public mode As VignetteAndChromaticAberration.AberrationMode

		' Token: 0x0400571A RID: 22298
		Public intensity As Single = 0.375F

		' Token: 0x0400571B RID: 22299
		Public chromaticAberration As Single = 0.2F

		' Token: 0x0400571C RID: 22300
		Public axialAberration As Single = 0.5F

		' Token: 0x0400571D RID: 22301
		Public blur As Single

		' Token: 0x0400571E RID: 22302
		Public blurSpread As Single = 0.75F

		' Token: 0x0400571F RID: 22303
		Public luminanceDependency As Single = 0.25F

		' Token: 0x04005720 RID: 22304
		Public blurDistance As Single = 2.5F

		' Token: 0x04005721 RID: 22305
		Public vignetteShader As Shader

		' Token: 0x04005722 RID: 22306
		Public separableBlurShader As Shader

		' Token: 0x04005723 RID: 22307
		Public chromAberrationShader As Shader

		' Token: 0x04005724 RID: 22308
		Private m_VignetteMaterial As Material

		' Token: 0x04005725 RID: 22309
		Private m_SeparableBlurMaterial As Material

		' Token: 0x04005726 RID: 22310
		Private m_ChromAberrationMaterial As Material

		' Token: 0x02000CF6 RID: 3318
		Public Enum AberrationMode
			' Token: 0x04005728 RID: 22312
			Simple
			' Token: 0x04005729 RID: 22313
			Advanced
		End Enum
	End Class
End Namespace
