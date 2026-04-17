Imports System
Imports UnityEngine

Namespace UnityStandardAssets.ImageEffects
	' Token: 0x02000CBF RID: 3263
	<ExecuteInEditMode()>
	<RequireComponent(GetType(Camera))>
	<AddComponentMenu("Image Effects/Bloom and Glow/BloomAndFlares (3.5, Deprecated)")>
	Public Class BloomAndFlares
		Inherits PostEffectsBase

		' Token: 0x060051B5 RID: 20917 RVA: 0x0029C5F8 File Offset: 0x0029A9F8
		Public Overrides Function CheckResources() As Boolean
			MyBase.CheckSupport(False)
			Me.screenBlend = MyBase.CheckShaderAndCreateMaterial(Me.screenBlendShader, Me.screenBlend)
			Me.lensFlareMaterial = MyBase.CheckShaderAndCreateMaterial(Me.lensFlareShader, Me.lensFlareMaterial)
			Me.vignetteMaterial = MyBase.CheckShaderAndCreateMaterial(Me.vignetteShader, Me.vignetteMaterial)
			Me.separableBlurMaterial = MyBase.CheckShaderAndCreateMaterial(Me.separableBlurShader, Me.separableBlurMaterial)
			Me.addBrightStuffBlendOneOneMaterial = MyBase.CheckShaderAndCreateMaterial(Me.addBrightStuffOneOneShader, Me.addBrightStuffBlendOneOneMaterial)
			Me.hollywoodFlaresMaterial = MyBase.CheckShaderAndCreateMaterial(Me.hollywoodFlaresShader, Me.hollywoodFlaresMaterial)
			Me.brightPassFilterMaterial = MyBase.CheckShaderAndCreateMaterial(Me.brightPassFilterShader, Me.brightPassFilterMaterial)
			If Not Me.isSupported Then
				MyBase.ReportAutoDisable()
			End If
			Return Me.isSupported
		End Function

		' Token: 0x060051B6 RID: 20918 RVA: 0x0029C6CC File Offset: 0x0029AACC
		Private Sub OnRenderImage(source As RenderTexture, destination As RenderTexture)
			If Not Me.CheckResources() Then
				Graphics.Blit(source, destination)
				Return
			End If
			Me.doHdr = False
			If Me.hdr = HDRBloomMode.Auto Then
				Me.doHdr = source.format = RenderTextureFormat.ARGBHalf AndAlso MyBase.GetComponent(Of Camera)().allowHDR
			Else
				Me.doHdr = Me.hdr = HDRBloomMode.[On]
			End If
			Me.doHdr = Me.doHdr AndAlso Me.supportHDRTextures
			Dim bloomScreenBlendMode As BloomScreenBlendMode = Me.screenBlendMode
			If Me.doHdr Then
				bloomScreenBlendMode = BloomScreenBlendMode.Add
			End If
			Dim renderTextureFormat As RenderTextureFormat = If((Not Me.doHdr), RenderTextureFormat.[Default], RenderTextureFormat.ARGBHalf)
			Dim temporary As RenderTexture = RenderTexture.GetTemporary(source.width / 2, source.height / 2, 0, renderTextureFormat)
			Dim temporary2 As RenderTexture = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0, renderTextureFormat)
			Dim temporary3 As RenderTexture = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0, renderTextureFormat)
			Dim temporary4 As RenderTexture = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0, renderTextureFormat)
			Dim num As Single = 1F * CSng(source.width) / (1F * CSng(source.height))
			Dim num2 As Single = 0.001953125F
			Graphics.Blit(source, temporary, Me.screenBlend, 2)
			Graphics.Blit(temporary, temporary2, Me.screenBlend, 2)
			RenderTexture.ReleaseTemporary(temporary)
			Me.BrightFilter(Me.bloomThreshold, Me.useSrcAlphaAsMask, temporary2, temporary3)
			temporary2.DiscardContents()
			If Me.bloomBlurIterations < 1 Then
				Me.bloomBlurIterations = 1
			End If
			For i As Integer = 0 To Me.bloomBlurIterations - 1
				Dim num3 As Single = (1F + CSng(i) * 0.5F) * Me.sepBlurSpread
				Me.separableBlurMaterial.SetVector("offsets", New Vector4(0F, num3 * num2, 0F, 0F))
				Dim renderTexture As RenderTexture = If((i <> 0), temporary2, temporary3)
				Graphics.Blit(renderTexture, temporary4, Me.separableBlurMaterial)
				renderTexture.DiscardContents()
				Me.separableBlurMaterial.SetVector("offsets", New Vector4(num3 / num * num2, 0F, 0F, 0F))
				Graphics.Blit(temporary4, temporary2, Me.separableBlurMaterial)
				temporary4.DiscardContents()
			Next
			If Me.lensflares Then
				If Me.lensflareMode = LensflareStyle34.Ghosting Then
					Me.BrightFilter(Me.lensflareThreshold, 0F, temporary2, temporary4)
					temporary2.DiscardContents()
					Me.Vignette(0.975F, temporary4, temporary3)
					temporary4.DiscardContents()
					Me.BlendFlares(temporary3, temporary2)
					temporary3.DiscardContents()
				Else
					Me.hollywoodFlaresMaterial.SetVector("_threshold", New Vector4(Me.lensflareThreshold, 1F / (1F - Me.lensflareThreshold), 0F, 0F))
					Me.hollywoodFlaresMaterial.SetVector("tintColor", New Vector4(Me.flareColorA.r, Me.flareColorA.g, Me.flareColorA.b, Me.flareColorA.a) * Me.flareColorA.a * Me.lensflareIntensity)
					Graphics.Blit(temporary4, temporary3, Me.hollywoodFlaresMaterial, 2)
					temporary4.DiscardContents()
					Graphics.Blit(temporary3, temporary4, Me.hollywoodFlaresMaterial, 3)
					temporary3.DiscardContents()
					Me.hollywoodFlaresMaterial.SetVector("offsets", New Vector4(Me.sepBlurSpread * 1F / num * num2, 0F, 0F, 0F))
					Me.hollywoodFlaresMaterial.SetFloat("stretchWidth", Me.hollyStretchWidth)
					Graphics.Blit(temporary4, temporary3, Me.hollywoodFlaresMaterial, 1)
					temporary4.DiscardContents()
					Me.hollywoodFlaresMaterial.SetFloat("stretchWidth", Me.hollyStretchWidth * 2F)
					Graphics.Blit(temporary3, temporary4, Me.hollywoodFlaresMaterial, 1)
					temporary3.DiscardContents()
					Me.hollywoodFlaresMaterial.SetFloat("stretchWidth", Me.hollyStretchWidth * 4F)
					Graphics.Blit(temporary4, temporary3, Me.hollywoodFlaresMaterial, 1)
					temporary4.DiscardContents()
					If Me.lensflareMode = LensflareStyle34.Anamorphic Then
						For j As Integer = 0 To Me.hollywoodFlareBlurIterations - 1
							Me.separableBlurMaterial.SetVector("offsets", New Vector4(Me.hollyStretchWidth * 2F / num * num2, 0F, 0F, 0F))
							Graphics.Blit(temporary3, temporary4, Me.separableBlurMaterial)
							temporary3.DiscardContents()
							Me.separableBlurMaterial.SetVector("offsets", New Vector4(Me.hollyStretchWidth * 2F / num * num2, 0F, 0F, 0F))
							Graphics.Blit(temporary4, temporary3, Me.separableBlurMaterial)
							temporary4.DiscardContents()
						Next
						Me.AddTo(1F, temporary3, temporary2)
						temporary3.DiscardContents()
					Else
						For k As Integer = 0 To Me.hollywoodFlareBlurIterations - 1
							Me.separableBlurMaterial.SetVector("offsets", New Vector4(Me.hollyStretchWidth * 2F / num * num2, 0F, 0F, 0F))
							Graphics.Blit(temporary3, temporary4, Me.separableBlurMaterial)
							temporary3.DiscardContents()
							Me.separableBlurMaterial.SetVector("offsets", New Vector4(Me.hollyStretchWidth * 2F / num * num2, 0F, 0F, 0F))
							Graphics.Blit(temporary4, temporary3, Me.separableBlurMaterial)
							temporary4.DiscardContents()
						Next
						Me.Vignette(1F, temporary3, temporary4)
						temporary3.DiscardContents()
						Me.BlendFlares(temporary4, temporary3)
						temporary4.DiscardContents()
						Me.AddTo(1F, temporary3, temporary2)
						temporary3.DiscardContents()
					End If
				End If
			End If
			Me.screenBlend.SetFloat("_Intensity", Me.bloomIntensity)
			Me.screenBlend.SetTexture("_ColorBuffer", source)
			Graphics.Blit(temporary2, destination, Me.screenBlend, CInt(bloomScreenBlendMode))
			RenderTexture.ReleaseTemporary(temporary2)
			RenderTexture.ReleaseTemporary(temporary3)
			RenderTexture.ReleaseTemporary(temporary4)
		End Sub

		' Token: 0x060051B7 RID: 20919 RVA: 0x0029CD03 File Offset: 0x0029B103
		Private Sub AddTo(intensity_ As Single, from As RenderTexture, [to] As RenderTexture)
			Me.addBrightStuffBlendOneOneMaterial.SetFloat("_Intensity", intensity_)
			Graphics.Blit(from, [to], Me.addBrightStuffBlendOneOneMaterial)
		End Sub

		' Token: 0x060051B8 RID: 20920 RVA: 0x0029CD24 File Offset: 0x0029B124
		Private Sub BlendFlares(from As RenderTexture, [to] As RenderTexture)
			Me.lensFlareMaterial.SetVector("colorA", New Vector4(Me.flareColorA.r, Me.flareColorA.g, Me.flareColorA.b, Me.flareColorA.a) * Me.lensflareIntensity)
			Me.lensFlareMaterial.SetVector("colorB", New Vector4(Me.flareColorB.r, Me.flareColorB.g, Me.flareColorB.b, Me.flareColorB.a) * Me.lensflareIntensity)
			Me.lensFlareMaterial.SetVector("colorC", New Vector4(Me.flareColorC.r, Me.flareColorC.g, Me.flareColorC.b, Me.flareColorC.a) * Me.lensflareIntensity)
			Me.lensFlareMaterial.SetVector("colorD", New Vector4(Me.flareColorD.r, Me.flareColorD.g, Me.flareColorD.b, Me.flareColorD.a) * Me.lensflareIntensity)
			Graphics.Blit(from, [to], Me.lensFlareMaterial)
		End Sub

		' Token: 0x060051B9 RID: 20921 RVA: 0x0029CE70 File Offset: 0x0029B270
		Private Sub BrightFilter(thresh As Single, useAlphaAsMask As Single, from As RenderTexture, [to] As RenderTexture)
			If Me.doHdr Then
				Me.brightPassFilterMaterial.SetVector("threshold", New Vector4(thresh, 1F, 0F, 0F))
			Else
				Me.brightPassFilterMaterial.SetVector("threshold", New Vector4(thresh, 1F / (1F - thresh), 0F, 0F))
			End If
			Me.brightPassFilterMaterial.SetFloat("useSrcAlphaAsMask", useAlphaAsMask)
			Graphics.Blit(from, [to], Me.brightPassFilterMaterial)
		End Sub

		' Token: 0x060051BA RID: 20922 RVA: 0x0029CF00 File Offset: 0x0029B300
		Private Sub Vignette(amount As Single, from As RenderTexture, [to] As RenderTexture)
			If Me.lensFlareVignetteMask Then
				Me.screenBlend.SetTexture("_ColorBuffer", Me.lensFlareVignetteMask)
				Graphics.Blit(from, [to], Me.screenBlend, 3)
			Else
				Me.vignetteMaterial.SetFloat("vignetteIntensity", amount)
				Graphics.Blit(from, [to], Me.vignetteMaterial)
			End If
		End Sub

		' Token: 0x0400556E RID: 21870
		Public tweakMode As TweakMode34

		' Token: 0x0400556F RID: 21871
		Public screenBlendMode As BloomScreenBlendMode = BloomScreenBlendMode.Add

		' Token: 0x04005570 RID: 21872
		Public hdr As HDRBloomMode

		' Token: 0x04005571 RID: 21873
		Private doHdr As Boolean

		' Token: 0x04005572 RID: 21874
		Public sepBlurSpread As Single = 1.5F

		' Token: 0x04005573 RID: 21875
		Public useSrcAlphaAsMask As Single = 0.5F

		' Token: 0x04005574 RID: 21876
		Public bloomIntensity As Single = 1F

		' Token: 0x04005575 RID: 21877
		Public bloomThreshold As Single = 0.5F

		' Token: 0x04005576 RID: 21878
		Public bloomBlurIterations As Integer = 2

		' Token: 0x04005577 RID: 21879
		Public lensflares As Boolean

		' Token: 0x04005578 RID: 21880
		Public hollywoodFlareBlurIterations As Integer = 2

		' Token: 0x04005579 RID: 21881
		Public lensflareMode As LensflareStyle34 = LensflareStyle34.Anamorphic

		' Token: 0x0400557A RID: 21882
		Public hollyStretchWidth As Single = 3.5F

		' Token: 0x0400557B RID: 21883
		Public lensflareIntensity As Single = 1F

		' Token: 0x0400557C RID: 21884
		Public lensflareThreshold As Single = 0.3F

		' Token: 0x0400557D RID: 21885
		Public flareColorA As Color = New Color(0.4F, 0.4F, 0.8F, 0.75F)

		' Token: 0x0400557E RID: 21886
		Public flareColorB As Color = New Color(0.4F, 0.8F, 0.8F, 0.75F)

		' Token: 0x0400557F RID: 21887
		Public flareColorC As Color = New Color(0.8F, 0.4F, 0.8F, 0.75F)

		' Token: 0x04005580 RID: 21888
		Public flareColorD As Color = New Color(0.8F, 0.4F, 0F, 0.75F)

		' Token: 0x04005581 RID: 21889
		Public lensFlareVignetteMask As Texture2D

		' Token: 0x04005582 RID: 21890
		Public lensFlareShader As Shader

		' Token: 0x04005583 RID: 21891
		Private lensFlareMaterial As Material

		' Token: 0x04005584 RID: 21892
		Public vignetteShader As Shader

		' Token: 0x04005585 RID: 21893
		Private vignetteMaterial As Material

		' Token: 0x04005586 RID: 21894
		Public separableBlurShader As Shader

		' Token: 0x04005587 RID: 21895
		Private separableBlurMaterial As Material

		' Token: 0x04005588 RID: 21896
		Public addBrightStuffOneOneShader As Shader

		' Token: 0x04005589 RID: 21897
		Private addBrightStuffBlendOneOneMaterial As Material

		' Token: 0x0400558A RID: 21898
		Public screenBlendShader As Shader

		' Token: 0x0400558B RID: 21899
		Private screenBlend As Material

		' Token: 0x0400558C RID: 21900
		Public hollywoodFlaresShader As Shader

		' Token: 0x0400558D RID: 21901
		Private hollywoodFlaresMaterial As Material

		' Token: 0x0400558E RID: 21902
		Public brightPassFilterShader As Shader

		' Token: 0x0400558F RID: 21903
		Private brightPassFilterMaterial As Material
	End Class
End Namespace
