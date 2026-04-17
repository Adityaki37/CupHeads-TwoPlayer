Imports System
Imports UnityEngine

Namespace UnityStandardAssets.ImageEffects
	' Token: 0x02000CB5 RID: 3253
	<ExecuteInEditMode()>
	<RequireComponent(GetType(Camera))>
	<AddComponentMenu("Image Effects/Bloom and Glow/Bloom")>
	Public Class Bloom
		Inherits PostEffectsBase

		' Token: 0x060051AD RID: 20909 RVA: 0x0029BA70 File Offset: 0x00299E70
		Public Overrides Function CheckResources() As Boolean
			MyBase.CheckSupport(False)
			Me.screenBlend = MyBase.CheckShaderAndCreateMaterial(Me.screenBlendShader, Me.screenBlend)
			Me.lensFlareMaterial = MyBase.CheckShaderAndCreateMaterial(Me.lensFlareShader, Me.lensFlareMaterial)
			Me.blurAndFlaresMaterial = MyBase.CheckShaderAndCreateMaterial(Me.blurAndFlaresShader, Me.blurAndFlaresMaterial)
			Me.brightPassFilterMaterial = MyBase.CheckShaderAndCreateMaterial(Me.brightPassFilterShader, Me.brightPassFilterMaterial)
			If Not Me.isSupported Then
				MyBase.ReportAutoDisable()
			End If
			Return Me.isSupported
		End Function

		' Token: 0x060051AE RID: 20910 RVA: 0x0029BAFC File Offset: 0x00299EFC
		Public Sub OnRenderImage(source As RenderTexture, destination As RenderTexture)
			If Not Me.CheckResources() Then
				Graphics.Blit(source, destination)
				Return
			End If
			Me.doHdr = False
			If Me.hdr = Bloom.HDRBloomMode.Auto Then
				Me.doHdr = source.format = RenderTextureFormat.ARGBHalf AndAlso MyBase.GetComponent(Of Camera)().allowHDR
			Else
				Me.doHdr = Me.hdr = Bloom.HDRBloomMode.[On]
			End If
			Me.doHdr = Me.doHdr AndAlso Me.supportHDRTextures
			Dim bloomScreenBlendMode As Bloom.BloomScreenBlendMode = Me.screenBlendMode
			If Me.doHdr Then
				bloomScreenBlendMode = Bloom.BloomScreenBlendMode.Add
			End If
			Dim renderTextureFormat As RenderTextureFormat = If((Not Me.doHdr), RenderTextureFormat.[Default], RenderTextureFormat.ARGBHalf)
			Dim num As Integer = source.width / 2
			Dim num2 As Integer = source.height / 2
			Dim num3 As Integer = source.width / 4
			Dim num4 As Integer = source.height / 4
			Dim num5 As Single = 1F * CSng(source.width) / (1F * CSng(source.height))
			Dim num6 As Single = 0.001953125F
			Dim temporary As RenderTexture = RenderTexture.GetTemporary(num3, num4, 0, renderTextureFormat)
			Dim temporary2 As RenderTexture = RenderTexture.GetTemporary(num, num2, 0, renderTextureFormat)
			If Me.quality > Bloom.BloomQuality.Cheap Then
				Graphics.Blit(source, temporary2, Me.screenBlend, 2)
				Dim temporary3 As RenderTexture = RenderTexture.GetTemporary(num3, num4, 0, renderTextureFormat)
				Graphics.Blit(temporary2, temporary3, Me.screenBlend, 2)
				Graphics.Blit(temporary3, temporary, Me.screenBlend, 6)
				RenderTexture.ReleaseTemporary(temporary3)
			Else
				Graphics.Blit(source, temporary2)
				Graphics.Blit(temporary2, temporary, Me.screenBlend, 6)
			End If
			RenderTexture.ReleaseTemporary(temporary2)
			Dim renderTexture As RenderTexture = RenderTexture.GetTemporary(num3, num4, 0, renderTextureFormat)
			Me.BrightFilter(Me.bloomThreshold * Me.bloomThresholdColor, temporary, renderTexture)
			If Me.bloomBlurIterations < 1 Then
				Me.bloomBlurIterations = 1
			ElseIf Me.bloomBlurIterations > 10 Then
				Me.bloomBlurIterations = 10
			End If
			For i As Integer = 0 To Me.bloomBlurIterations - 1
				Dim num7 As Single = (1F + CSng(i) * 0.25F) * Me.sepBlurSpread
				Dim renderTexture2 As RenderTexture = RenderTexture.GetTemporary(num3, num4, 0, renderTextureFormat)
				Me.blurAndFlaresMaterial.SetVector("_Offsets", New Vector4(0F, num7 * num6, 0F, 0F))
				Graphics.Blit(renderTexture, renderTexture2, Me.blurAndFlaresMaterial, 4)
				RenderTexture.ReleaseTemporary(renderTexture)
				renderTexture = renderTexture2
				renderTexture2 = RenderTexture.GetTemporary(num3, num4, 0, renderTextureFormat)
				Me.blurAndFlaresMaterial.SetVector("_Offsets", New Vector4(num7 / num5 * num6, 0F, 0F, 0F))
				Graphics.Blit(renderTexture, renderTexture2, Me.blurAndFlaresMaterial, 4)
				RenderTexture.ReleaseTemporary(renderTexture)
				renderTexture = renderTexture2
				If Me.quality > Bloom.BloomQuality.Cheap Then
					If i = 0 Then
						Graphics.SetRenderTarget(temporary)
						GL.Clear(False, True, Color.black)
						Graphics.Blit(renderTexture, temporary)
					Else
						temporary.MarkRestoreExpected()
						Graphics.Blit(renderTexture, temporary, Me.screenBlend, 10)
					End If
				End If
			Next
			If Me.quality > Bloom.BloomQuality.Cheap Then
				Graphics.SetRenderTarget(renderTexture)
				GL.Clear(False, True, Color.black)
				Graphics.Blit(temporary, renderTexture, Me.screenBlend, 6)
			End If
			If Me.lensflareIntensity > Mathf.Epsilon Then
				Dim temporary4 As RenderTexture = RenderTexture.GetTemporary(num3, num4, 0, renderTextureFormat)
				If Me.lensflareMode = Bloom.LensFlareStyle.Ghosting Then
					Me.BrightFilter(Me.lensflareThreshold, renderTexture, temporary4)
					If Me.quality > Bloom.BloomQuality.Cheap Then
						Me.blurAndFlaresMaterial.SetVector("_Offsets", New Vector4(0F, 1.5F / (1F * CSng(temporary.height)), 0F, 0F))
						Graphics.SetRenderTarget(temporary)
						GL.Clear(False, True, Color.black)
						Graphics.Blit(temporary4, temporary, Me.blurAndFlaresMaterial, 4)
						Me.blurAndFlaresMaterial.SetVector("_Offsets", New Vector4(1.5F / (1F * CSng(temporary.width)), 0F, 0F, 0F))
						Graphics.SetRenderTarget(temporary4)
						GL.Clear(False, True, Color.black)
						Graphics.Blit(temporary, temporary4, Me.blurAndFlaresMaterial, 4)
					End If
					Me.Vignette(0.975F, temporary4, temporary4)
					Me.BlendFlares(temporary4, renderTexture)
				Else
					Dim num8 As Single = 1F * Mathf.Cos(Me.flareRotation)
					Dim num9 As Single = 1F * Mathf.Sin(Me.flareRotation)
					Dim num10 As Single = Me.hollyStretchWidth * 1F / num5 * num6
					Me.blurAndFlaresMaterial.SetVector("_Offsets", New Vector4(num8, num9, 0F, 0F))
					Me.blurAndFlaresMaterial.SetVector("_Threshhold", New Vector4(Me.lensflareThreshold, 1F, 0F, 0F))
					Me.blurAndFlaresMaterial.SetVector("_TintColor", New Vector4(Me.flareColorA.r, Me.flareColorA.g, Me.flareColorA.b, Me.flareColorA.a) * Me.flareColorA.a * Me.lensflareIntensity)
					Me.blurAndFlaresMaterial.SetFloat("_Saturation", Me.lensFlareSaturation)
					temporary.DiscardContents()
					Graphics.Blit(temporary4, temporary, Me.blurAndFlaresMaterial, 2)
					temporary4.DiscardContents()
					Graphics.Blit(temporary, temporary4, Me.blurAndFlaresMaterial, 3)
					Me.blurAndFlaresMaterial.SetVector("_Offsets", New Vector4(num8 * num10, num9 * num10, 0F, 0F))
					Me.blurAndFlaresMaterial.SetFloat("_StretchWidth", Me.hollyStretchWidth)
					temporary.DiscardContents()
					Graphics.Blit(temporary4, temporary, Me.blurAndFlaresMaterial, 1)
					Me.blurAndFlaresMaterial.SetFloat("_StretchWidth", Me.hollyStretchWidth * 2F)
					temporary4.DiscardContents()
					Graphics.Blit(temporary, temporary4, Me.blurAndFlaresMaterial, 1)
					Me.blurAndFlaresMaterial.SetFloat("_StretchWidth", Me.hollyStretchWidth * 4F)
					temporary.DiscardContents()
					Graphics.Blit(temporary4, temporary, Me.blurAndFlaresMaterial, 1)
					For j As Integer = 0 To Me.hollywoodFlareBlurIterations - 1
						num10 = Me.hollyStretchWidth * 2F / num5 * num6
						Me.blurAndFlaresMaterial.SetVector("_Offsets", New Vector4(num10 * num8, num10 * num9, 0F, 0F))
						temporary4.DiscardContents()
						Graphics.Blit(temporary, temporary4, Me.blurAndFlaresMaterial, 4)
						Me.blurAndFlaresMaterial.SetVector("_Offsets", New Vector4(num10 * num8, num10 * num9, 0F, 0F))
						temporary.DiscardContents()
						Graphics.Blit(temporary4, temporary, Me.blurAndFlaresMaterial, 4)
					Next
					If Me.lensflareMode = Bloom.LensFlareStyle.Anamorphic Then
						Me.AddTo(1F, temporary, renderTexture)
					Else
						Me.Vignette(1F, temporary, temporary4)
						Me.BlendFlares(temporary4, temporary)
						Me.AddTo(1F, temporary, renderTexture)
					End If
				End If
				RenderTexture.ReleaseTemporary(temporary4)
			End If
			Dim num11 As Integer = CInt(bloomScreenBlendMode)
			Me.screenBlend.SetFloat("_Intensity", Me.bloomIntensity)
			Me.screenBlend.SetTexture("_ColorBuffer", source)
			If Me.quality > Bloom.BloomQuality.Cheap Then
				Dim temporary5 As RenderTexture = RenderTexture.GetTemporary(num, num2, 0, renderTextureFormat)
				Graphics.Blit(renderTexture, temporary5)
				Graphics.Blit(temporary5, destination, Me.screenBlend, num11)
				RenderTexture.ReleaseTemporary(temporary5)
			Else
				Graphics.Blit(renderTexture, destination, Me.screenBlend, num11)
			End If
			RenderTexture.ReleaseTemporary(temporary)
			RenderTexture.ReleaseTemporary(renderTexture)
		End Sub

		' Token: 0x060051AF RID: 20911 RVA: 0x0029C29D File Offset: 0x0029A69D
		Private Sub AddTo(intensity_ As Single, from As RenderTexture, [to] As RenderTexture)
			Me.screenBlend.SetFloat("_Intensity", intensity_)
			[to].MarkRestoreExpected()
			Graphics.Blit(from, [to], Me.screenBlend, 9)
		End Sub

		' Token: 0x060051B0 RID: 20912 RVA: 0x0029C2C8 File Offset: 0x0029A6C8
		Private Sub BlendFlares(from As RenderTexture, [to] As RenderTexture)
			Me.lensFlareMaterial.SetVector("colorA", New Vector4(Me.flareColorA.r, Me.flareColorA.g, Me.flareColorA.b, Me.flareColorA.a) * Me.lensflareIntensity)
			Me.lensFlareMaterial.SetVector("colorB", New Vector4(Me.flareColorB.r, Me.flareColorB.g, Me.flareColorB.b, Me.flareColorB.a) * Me.lensflareIntensity)
			Me.lensFlareMaterial.SetVector("colorC", New Vector4(Me.flareColorC.r, Me.flareColorC.g, Me.flareColorC.b, Me.flareColorC.a) * Me.lensflareIntensity)
			Me.lensFlareMaterial.SetVector("colorD", New Vector4(Me.flareColorD.r, Me.flareColorD.g, Me.flareColorD.b, Me.flareColorD.a) * Me.lensflareIntensity)
			[to].MarkRestoreExpected()
			Graphics.Blit(from, [to], Me.lensFlareMaterial)
		End Sub

		' Token: 0x060051B1 RID: 20913 RVA: 0x0029C418 File Offset: 0x0029A818
		Private Sub BrightFilter(thresh As Single, from As RenderTexture, [to] As RenderTexture)
			Me.brightPassFilterMaterial.SetVector("_Threshhold", New Vector4(thresh, thresh, thresh, thresh))
			Graphics.Blit(from, [to], Me.brightPassFilterMaterial, 0)
		End Sub

		' Token: 0x060051B2 RID: 20914 RVA: 0x0029C441 File Offset: 0x0029A841
		Private Sub BrightFilter(threshColor As Color, from As RenderTexture, [to] As RenderTexture)
			Me.brightPassFilterMaterial.SetVector("_Threshhold", threshColor)
			Graphics.Blit(from, [to], Me.brightPassFilterMaterial, 1)
		End Sub

		' Token: 0x060051B3 RID: 20915 RVA: 0x0029C468 File Offset: 0x0029A868
		Private Sub Vignette(amount As Single, from As RenderTexture, [to] As RenderTexture)
			If Me.lensFlareVignetteMask Then
				Me.screenBlend.SetTexture("_ColorBuffer", Me.lensFlareVignetteMask)
				[to].MarkRestoreExpected()
				Graphics.Blit(If((Not(from Is [to])), from, Nothing), [to], Me.screenBlend, If((Not(from Is [to])), 3, 7))
			ElseIf from IsNot [to] Then
				Graphics.SetRenderTarget([to])
				GL.Clear(False, True, Color.black)
				Graphics.Blit(from, [to])
			End If
		End Sub

		' Token: 0x04005531 RID: 21809
		Public tweakMode As Bloom.TweakMode

		' Token: 0x04005532 RID: 21810
		Public screenBlendMode As Bloom.BloomScreenBlendMode = Bloom.BloomScreenBlendMode.Add

		' Token: 0x04005533 RID: 21811
		Public hdr As Bloom.HDRBloomMode

		' Token: 0x04005534 RID: 21812
		Private doHdr As Boolean

		' Token: 0x04005535 RID: 21813
		Public sepBlurSpread As Single = 2.5F

		' Token: 0x04005536 RID: 21814
		Public quality As Bloom.BloomQuality = Bloom.BloomQuality.High

		' Token: 0x04005537 RID: 21815
		Public bloomIntensity As Single = 0.5F

		' Token: 0x04005538 RID: 21816
		Public bloomThreshold As Single = 0.5F

		' Token: 0x04005539 RID: 21817
		Public bloomThresholdColor As Color = Color.white

		' Token: 0x0400553A RID: 21818
		Public bloomBlurIterations As Integer = 2

		' Token: 0x0400553B RID: 21819
		Public hollywoodFlareBlurIterations As Integer = 2

		' Token: 0x0400553C RID: 21820
		Public flareRotation As Single

		' Token: 0x0400553D RID: 21821
		Public lensflareMode As Bloom.LensFlareStyle = Bloom.LensFlareStyle.Anamorphic

		' Token: 0x0400553E RID: 21822
		Public hollyStretchWidth As Single = 2.5F

		' Token: 0x0400553F RID: 21823
		Public lensflareIntensity As Single

		' Token: 0x04005540 RID: 21824
		Public lensflareThreshold As Single = 0.3F

		' Token: 0x04005541 RID: 21825
		Public lensFlareSaturation As Single = 0.75F

		' Token: 0x04005542 RID: 21826
		Public flareColorA As Color = New Color(0.4F, 0.4F, 0.8F, 0.75F)

		' Token: 0x04005543 RID: 21827
		Public flareColorB As Color = New Color(0.4F, 0.8F, 0.8F, 0.75F)

		' Token: 0x04005544 RID: 21828
		Public flareColorC As Color = New Color(0.8F, 0.4F, 0.8F, 0.75F)

		' Token: 0x04005545 RID: 21829
		Public flareColorD As Color = New Color(0.8F, 0.4F, 0F, 0.75F)

		' Token: 0x04005546 RID: 21830
		Public lensFlareVignetteMask As Texture2D

		' Token: 0x04005547 RID: 21831
		Public lensFlareShader As Shader

		' Token: 0x04005548 RID: 21832
		Private lensFlareMaterial As Material

		' Token: 0x04005549 RID: 21833
		Public screenBlendShader As Shader

		' Token: 0x0400554A RID: 21834
		Private screenBlend As Material

		' Token: 0x0400554B RID: 21835
		Public blurAndFlaresShader As Shader

		' Token: 0x0400554C RID: 21836
		Private blurAndFlaresMaterial As Material

		' Token: 0x0400554D RID: 21837
		Public brightPassFilterShader As Shader

		' Token: 0x0400554E RID: 21838
		Private brightPassFilterMaterial As Material

		' Token: 0x02000CB6 RID: 3254
		Public Enum LensFlareStyle
			' Token: 0x04005550 RID: 21840
			Ghosting
			' Token: 0x04005551 RID: 21841
			Anamorphic
			' Token: 0x04005552 RID: 21842
			Combined
		End Enum

		' Token: 0x02000CB7 RID: 3255
		Public Enum TweakMode
			' Token: 0x04005554 RID: 21844
			Basic
			' Token: 0x04005555 RID: 21845
			Complex
		End Enum

		' Token: 0x02000CB8 RID: 3256
		Public Enum HDRBloomMode
			' Token: 0x04005557 RID: 21847
			Auto
			' Token: 0x04005558 RID: 21848
			[On]
			' Token: 0x04005559 RID: 21849
			Off
		End Enum

		' Token: 0x02000CB9 RID: 3257
		Public Enum BloomScreenBlendMode
			' Token: 0x0400555B RID: 21851
			Screen
			' Token: 0x0400555C RID: 21852
			Add
		End Enum

		' Token: 0x02000CBA RID: 3258
		Public Enum BloomQuality
			' Token: 0x0400555E RID: 21854
			Cheap
			' Token: 0x0400555F RID: 21855
			High
		End Enum
	End Class
End Namespace
