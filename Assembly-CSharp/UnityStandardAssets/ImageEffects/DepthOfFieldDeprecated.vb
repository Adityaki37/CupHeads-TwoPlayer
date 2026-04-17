Imports System
Imports UnityEngine

Namespace UnityStandardAssets.ImageEffects
	' Token: 0x02000CD2 RID: 3282
	<ExecuteInEditMode()>
	<RequireComponent(GetType(Camera))>
	<AddComponentMenu("Image Effects/Camera/Depth of Field (deprecated)")>
	Public Class DepthOfFieldDeprecated
		Inherits PostEffectsBase

		' Token: 0x06005202 RID: 20994 RVA: 0x002A0684 File Offset: 0x0029EA84
		Private Sub CreateMaterials()
			Me.dofBlurMaterial = MyBase.CheckShaderAndCreateMaterial(Me.dofBlurShader, Me.dofBlurMaterial)
			Me.dofMaterial = MyBase.CheckShaderAndCreateMaterial(Me.dofShader, Me.dofMaterial)
			Me.bokehSupport = Me.bokehShader.isSupported
			If Me.bokeh AndAlso Me.bokehSupport AndAlso Me.bokehShader Then
				Me.bokehMaterial = MyBase.CheckShaderAndCreateMaterial(Me.bokehShader, Me.bokehMaterial)
			End If
		End Sub

		' Token: 0x06005203 RID: 20995 RVA: 0x002A0710 File Offset: 0x0029EB10
		Public Overrides Function CheckResources() As Boolean
			MyBase.CheckSupport(True)
			Me.dofBlurMaterial = MyBase.CheckShaderAndCreateMaterial(Me.dofBlurShader, Me.dofBlurMaterial)
			Me.dofMaterial = MyBase.CheckShaderAndCreateMaterial(Me.dofShader, Me.dofMaterial)
			Me.bokehSupport = Me.bokehShader.isSupported
			If Me.bokeh AndAlso Me.bokehSupport AndAlso Me.bokehShader Then
				Me.bokehMaterial = MyBase.CheckShaderAndCreateMaterial(Me.bokehShader, Me.bokehMaterial)
			End If
			If Not Me.isSupported Then
				MyBase.ReportAutoDisable()
			End If
			Return Me.isSupported
		End Function

		' Token: 0x06005204 RID: 20996 RVA: 0x002A07BB File Offset: 0x0029EBBB
		Private Sub OnDisable()
			Quads.Cleanup()
		End Sub

		' Token: 0x06005205 RID: 20997 RVA: 0x002A07C2 File Offset: 0x0029EBC2
		Private Sub OnEnable()
			Me._camera = MyBase.GetComponent(Of Camera)()
			Me._camera.depthTextureMode = Me._camera.depthTextureMode Or DepthTextureMode.Depth
		End Sub

		' Token: 0x06005206 RID: 20998 RVA: 0x002A07E4 File Offset: 0x0029EBE4
		Private Function FocalDistance01(worldDist As Single) As Single
			Return Me._camera.WorldToViewportPoint((worldDist - Me._camera.nearClipPlane) * Me._camera.transform.forward + Me._camera.transform.position).z / (Me._camera.farClipPlane - Me._camera.nearClipPlane)
		End Function

		' Token: 0x06005207 RID: 20999 RVA: 0x002A0854 File Offset: 0x0029EC54
		Private Function GetDividerBasedOnQuality() As Integer
			Dim num As Integer = 1
			If Me.resolution = DepthOfFieldDeprecated.DofResolution.Medium Then
				num = 2
			ElseIf Me.resolution = DepthOfFieldDeprecated.DofResolution.Low Then
				num = 2
			End If
			Return num
		End Function

		' Token: 0x06005208 RID: 21000 RVA: 0x002A0888 File Offset: 0x0029EC88
		Private Function GetLowResolutionDividerBasedOnQuality(baseDivider As Integer) As Integer
			Dim num As Integer = baseDivider
			If Me.resolution = DepthOfFieldDeprecated.DofResolution.High Then
				num *= 2
			End If
			If Me.resolution = DepthOfFieldDeprecated.DofResolution.Low Then
				num *= 2
			End If
			Return num
		End Function

		' Token: 0x06005209 RID: 21001 RVA: 0x002A08B8 File Offset: 0x0029ECB8
		Private Sub OnRenderImage(source As RenderTexture, destination As RenderTexture)
			If Not Me.CheckResources() Then
				Graphics.Blit(source, destination)
				Return
			End If
			If Me.smoothness < 0.1F Then
				Me.smoothness = 0.1F
			End If
			Me.bokeh = Me.bokeh AndAlso Me.bokehSupport
			Dim num As Single = If((Not Me.bokeh), 1F, DepthOfFieldDeprecated.BOKEH_EXTRA_BLUR)
			Dim flag As Boolean = Me.quality > DepthOfFieldDeprecated.Dof34QualitySetting.OnlyBackground
			Dim num2 As Single = Me.focalSize / (Me._camera.farClipPlane - Me._camera.nearClipPlane)
			If Me.simpleTweakMode Then
				Me.focalDistance01 = If((Not Me.objectFocus), Me.FocalDistance01(Me.focalPoint), (Me._camera.WorldToViewportPoint(Me.objectFocus.position).z / Me._camera.farClipPlane))
				Me.focalStartCurve = Me.focalDistance01 * Me.smoothness
				Me.focalEndCurve = Me.focalStartCurve
				flag = flag AndAlso Me.focalPoint > Me._camera.nearClipPlane + Mathf.Epsilon
			Else
				If Me.objectFocus Then
					Dim vector As Vector3 = Me._camera.WorldToViewportPoint(Me.objectFocus.position)
					vector.z /= Me._camera.farClipPlane
					Me.focalDistance01 = vector.z
				Else
					Me.focalDistance01 = Me.FocalDistance01(Me.focalZDistance)
				End If
				Me.focalStartCurve = Me.focalZStartCurve
				Me.focalEndCurve = Me.focalZEndCurve
				flag = flag AndAlso Me.focalPoint > Me._camera.nearClipPlane + Mathf.Epsilon
			End If
			Me.widthOverHeight = 1F * CSng(source.width) / (1F * CSng(source.height))
			Me.oneOverBaseSize = 0.001953125F
			Me.dofMaterial.SetFloat("_ForegroundBlurExtrude", Me.foregroundBlurExtrude)
			Me.dofMaterial.SetVector("_CurveParams", New Vector4(If((Not Me.simpleTweakMode), Me.focalStartCurve, (1F / Me.focalStartCurve)), If((Not Me.simpleTweakMode), Me.focalEndCurve, (1F / Me.focalEndCurve)), num2 * 0.5F, Me.focalDistance01))
			Me.dofMaterial.SetVector("_InvRenderTargetSize", New Vector4(1F / (1F * CSng(source.width)), 1F / (1F * CSng(source.height)), 0F, 0F))
			Dim dividerBasedOnQuality As Integer = Me.GetDividerBasedOnQuality()
			Dim lowResolutionDividerBasedOnQuality As Integer = Me.GetLowResolutionDividerBasedOnQuality(dividerBasedOnQuality)
			Me.AllocateTextures(flag, source, dividerBasedOnQuality, lowResolutionDividerBasedOnQuality)
			Graphics.Blit(source, source, Me.dofMaterial, 3)
			Me.Downsample(source, Me.mediumRezWorkTexture)
			Me.Blur(Me.mediumRezWorkTexture, Me.mediumRezWorkTexture, DepthOfFieldDeprecated.DofBlurriness.Low, 4, Me.maxBlurSpread)
			If Me.bokeh AndAlso (DepthOfFieldDeprecated.BokehDestination.Foreground And Me.bokehDestination) <> CType(0, DepthOfFieldDeprecated.BokehDestination) Then
				Me.dofMaterial.SetVector("_Threshhold", New Vector4(Me.bokehThresholdContrast, Me.bokehThresholdLuminance, 0.95F, 0F))
				Graphics.Blit(Me.mediumRezWorkTexture, Me.bokehSource2, Me.dofMaterial, 11)
				Graphics.Blit(Me.mediumRezWorkTexture, Me.lowRezWorkTexture)
				Me.Blur(Me.lowRezWorkTexture, Me.lowRezWorkTexture, Me.bluriness, 0, Me.maxBlurSpread * num)
			Else
				Me.Downsample(Me.mediumRezWorkTexture, Me.lowRezWorkTexture)
				Me.Blur(Me.lowRezWorkTexture, Me.lowRezWorkTexture, Me.bluriness, 0, Me.maxBlurSpread)
			End If
			Me.dofBlurMaterial.SetTexture("_TapLow", Me.lowRezWorkTexture)
			Me.dofBlurMaterial.SetTexture("_TapMedium", Me.mediumRezWorkTexture)
			Graphics.Blit(Nothing, Me.finalDefocus, Me.dofBlurMaterial, 3)
			If Me.bokeh AndAlso (DepthOfFieldDeprecated.BokehDestination.Foreground And Me.bokehDestination) <> CType(0, DepthOfFieldDeprecated.BokehDestination) Then
				Me.AddBokeh(Me.bokehSource2, Me.bokehSource, Me.finalDefocus)
			End If
			Me.dofMaterial.SetTexture("_TapLowBackground", Me.finalDefocus)
			Me.dofMaterial.SetTexture("_TapMedium", Me.mediumRezWorkTexture)
			Graphics.Blit(source, If((Not flag), destination, Me.foregroundTexture), Me.dofMaterial, If((Not Me.visualize), 0, 2))
			If flag Then
				Graphics.Blit(Me.foregroundTexture, source, Me.dofMaterial, 5)
				Me.Downsample(source, Me.mediumRezWorkTexture)
				Me.BlurFg(Me.mediumRezWorkTexture, Me.mediumRezWorkTexture, DepthOfFieldDeprecated.DofBlurriness.Low, 2, Me.maxBlurSpread)
				If Me.bokeh AndAlso (DepthOfFieldDeprecated.BokehDestination.Foreground And Me.bokehDestination) <> CType(0, DepthOfFieldDeprecated.BokehDestination) Then
					Me.dofMaterial.SetVector("_Threshhold", New Vector4(Me.bokehThresholdContrast * 0.5F, Me.bokehThresholdLuminance, 0F, 0F))
					Graphics.Blit(Me.mediumRezWorkTexture, Me.bokehSource2, Me.dofMaterial, 11)
					Graphics.Blit(Me.mediumRezWorkTexture, Me.lowRezWorkTexture)
					Me.BlurFg(Me.lowRezWorkTexture, Me.lowRezWorkTexture, Me.bluriness, 1, Me.maxBlurSpread * num)
				Else
					Me.BlurFg(Me.mediumRezWorkTexture, Me.lowRezWorkTexture, Me.bluriness, 1, Me.maxBlurSpread)
				End If
				Graphics.Blit(Me.lowRezWorkTexture, Me.finalDefocus)
				Me.dofMaterial.SetTexture("_TapLowForeground", Me.finalDefocus)
				Graphics.Blit(source, destination, Me.dofMaterial, If((Not Me.visualize), 4, 1))
				If Me.bokeh AndAlso (DepthOfFieldDeprecated.BokehDestination.Foreground And Me.bokehDestination) <> CType(0, DepthOfFieldDeprecated.BokehDestination) Then
					Me.AddBokeh(Me.bokehSource2, Me.bokehSource, destination)
				End If
			End If
			Me.ReleaseTextures()
		End Sub

		' Token: 0x0600520A RID: 21002 RVA: 0x002A0ED4 File Offset: 0x0029F2D4
		Private Sub Blur(from As RenderTexture, [to] As RenderTexture, iterations As DepthOfFieldDeprecated.DofBlurriness, blurPass As Integer, spread As Single)
			Dim temporary As RenderTexture = RenderTexture.GetTemporary([to].width, [to].height)
			If iterations > DepthOfFieldDeprecated.DofBlurriness.Low Then
				Me.BlurHex(from, [to], blurPass, spread, temporary)
				If iterations > DepthOfFieldDeprecated.DofBlurriness.High Then
					Me.dofBlurMaterial.SetVector("offsets", New Vector4(0F, spread * Me.oneOverBaseSize, 0F, 0F))
					Graphics.Blit([to], temporary, Me.dofBlurMaterial, blurPass)
					Me.dofBlurMaterial.SetVector("offsets", New Vector4(spread / Me.widthOverHeight * Me.oneOverBaseSize, 0F, 0F, 0F))
					Graphics.Blit(temporary, [to], Me.dofBlurMaterial, blurPass)
				End If
			Else
				Me.dofBlurMaterial.SetVector("offsets", New Vector4(0F, spread * Me.oneOverBaseSize, 0F, 0F))
				Graphics.Blit(from, temporary, Me.dofBlurMaterial, blurPass)
				Me.dofBlurMaterial.SetVector("offsets", New Vector4(spread / Me.widthOverHeight * Me.oneOverBaseSize, 0F, 0F, 0F))
				Graphics.Blit(temporary, [to], Me.dofBlurMaterial, blurPass)
			End If
			RenderTexture.ReleaseTemporary(temporary)
		End Sub

		' Token: 0x0600520B RID: 21003 RVA: 0x002A1018 File Offset: 0x0029F418
		Private Sub BlurFg(from As RenderTexture, [to] As RenderTexture, iterations As DepthOfFieldDeprecated.DofBlurriness, blurPass As Integer, spread As Single)
			Me.dofBlurMaterial.SetTexture("_TapHigh", from)
			Dim temporary As RenderTexture = RenderTexture.GetTemporary([to].width, [to].height)
			If iterations > DepthOfFieldDeprecated.DofBlurriness.Low Then
				Me.BlurHex(from, [to], blurPass, spread, temporary)
				If iterations > DepthOfFieldDeprecated.DofBlurriness.High Then
					Me.dofBlurMaterial.SetVector("offsets", New Vector4(0F, spread * Me.oneOverBaseSize, 0F, 0F))
					Graphics.Blit([to], temporary, Me.dofBlurMaterial, blurPass)
					Me.dofBlurMaterial.SetVector("offsets", New Vector4(spread / Me.widthOverHeight * Me.oneOverBaseSize, 0F, 0F, 0F))
					Graphics.Blit(temporary, [to], Me.dofBlurMaterial, blurPass)
				End If
			Else
				Me.dofBlurMaterial.SetVector("offsets", New Vector4(0F, spread * Me.oneOverBaseSize, 0F, 0F))
				Graphics.Blit(from, temporary, Me.dofBlurMaterial, blurPass)
				Me.dofBlurMaterial.SetVector("offsets", New Vector4(spread / Me.widthOverHeight * Me.oneOverBaseSize, 0F, 0F, 0F))
				Graphics.Blit(temporary, [to], Me.dofBlurMaterial, blurPass)
			End If
			RenderTexture.ReleaseTemporary(temporary)
		End Sub

		' Token: 0x0600520C RID: 21004 RVA: 0x002A116C File Offset: 0x0029F56C
		Private Sub BlurHex(from As RenderTexture, [to] As RenderTexture, blurPass As Integer, spread As Single, tmp As RenderTexture)
			Me.dofBlurMaterial.SetVector("offsets", New Vector4(0F, spread * Me.oneOverBaseSize, 0F, 0F))
			Graphics.Blit(from, tmp, Me.dofBlurMaterial, blurPass)
			Me.dofBlurMaterial.SetVector("offsets", New Vector4(spread / Me.widthOverHeight * Me.oneOverBaseSize, 0F, 0F, 0F))
			Graphics.Blit(tmp, [to], Me.dofBlurMaterial, blurPass)
			Me.dofBlurMaterial.SetVector("offsets", New Vector4(spread / Me.widthOverHeight * Me.oneOverBaseSize, spread * Me.oneOverBaseSize, 0F, 0F))
			Graphics.Blit([to], tmp, Me.dofBlurMaterial, blurPass)
			Me.dofBlurMaterial.SetVector("offsets", New Vector4(spread / Me.widthOverHeight * Me.oneOverBaseSize, -spread * Me.oneOverBaseSize, 0F, 0F))
			Graphics.Blit(tmp, [to], Me.dofBlurMaterial, blurPass)
		End Sub

		' Token: 0x0600520D RID: 21005 RVA: 0x002A1288 File Offset: 0x0029F688
		Private Sub Downsample(from As RenderTexture, [to] As RenderTexture)
			Me.dofMaterial.SetVector("_InvRenderTargetSize", New Vector4(1F / (1F * CSng([to].width)), 1F / (1F * CSng([to].height)), 0F, 0F))
			Graphics.Blit(from, [to], Me.dofMaterial, DepthOfFieldDeprecated.SMOOTH_DOWNSAMPLE_PASS)
		End Sub

		' Token: 0x0600520E RID: 21006 RVA: 0x002A12EC File Offset: 0x0029F6EC
		Private Sub AddBokeh(bokehInfo As RenderTexture, tempTex As RenderTexture, finalTarget As RenderTexture)
			If Me.bokehMaterial Then
				Dim meshes As Mesh() = Quads.GetMeshes(tempTex.width, tempTex.height)
				RenderTexture.active = tempTex
				GL.Clear(False, True, New Color(0F, 0F, 0F, 0F))
				GL.PushMatrix()
				GL.LoadIdentity()
				bokehInfo.filterMode = FilterMode.Point
				Dim num As Single = CSng(bokehInfo.width) * 1F / (CSng(bokehInfo.height) * 1F)
				Dim num2 As Single = 2F / (1F * CSng(bokehInfo.width))
				num2 += Me.bokehScale * Me.maxBlurSpread * DepthOfFieldDeprecated.BOKEH_EXTRA_BLUR * Me.oneOverBaseSize
				Me.bokehMaterial.SetTexture("_Source", bokehInfo)
				Me.bokehMaterial.SetTexture("_MainTex", Me.bokehTexture)
				Me.bokehMaterial.SetVector("_ArScale", New Vector4(num2, num2 * num, 0.5F, 0.5F * num))
				Me.bokehMaterial.SetFloat("_Intensity", Me.bokehIntensity)
				Me.bokehMaterial.SetPass(0)
				For Each mesh As Mesh In meshes
					If mesh Then
						Graphics.DrawMeshNow(mesh, Matrix4x4.identity)
					End If
				Next
				GL.PopMatrix()
				Graphics.Blit(tempTex, finalTarget, Me.dofMaterial, 8)
				bokehInfo.filterMode = FilterMode.Bilinear
			End If
		End Sub

		' Token: 0x0600520F RID: 21007 RVA: 0x002A1460 File Offset: 0x0029F860
		Private Sub ReleaseTextures()
			If Me.foregroundTexture Then
				RenderTexture.ReleaseTemporary(Me.foregroundTexture)
			End If
			If Me.finalDefocus Then
				RenderTexture.ReleaseTemporary(Me.finalDefocus)
			End If
			If Me.mediumRezWorkTexture Then
				RenderTexture.ReleaseTemporary(Me.mediumRezWorkTexture)
			End If
			If Me.lowRezWorkTexture Then
				RenderTexture.ReleaseTemporary(Me.lowRezWorkTexture)
			End If
			If Me.bokehSource Then
				RenderTexture.ReleaseTemporary(Me.bokehSource)
			End If
			If Me.bokehSource2 Then
				RenderTexture.ReleaseTemporary(Me.bokehSource2)
			End If
		End Sub

		' Token: 0x06005210 RID: 21008 RVA: 0x002A1510 File Offset: 0x0029F910
		Private Sub AllocateTextures(blurForeground As Boolean, source As RenderTexture, divider As Integer, lowTexDivider As Integer)
			Me.foregroundTexture = Nothing
			If blurForeground Then
				Me.foregroundTexture = RenderTexture.GetTemporary(source.width, source.height, 0)
			End If
			Me.mediumRezWorkTexture = RenderTexture.GetTemporary(source.width / divider, source.height / divider, 0)
			Me.finalDefocus = RenderTexture.GetTemporary(source.width / divider, source.height / divider, 0)
			Me.lowRezWorkTexture = RenderTexture.GetTemporary(source.width / lowTexDivider, source.height / lowTexDivider, 0)
			Me.bokehSource = Nothing
			Me.bokehSource2 = Nothing
			If Me.bokeh Then
				Me.bokehSource = RenderTexture.GetTemporary(source.width / (lowTexDivider * Me.bokehDownsample), source.height / (lowTexDivider * Me.bokehDownsample), 0, RenderTextureFormat.ARGBHalf)
				Me.bokehSource2 = RenderTexture.GetTemporary(source.width / (lowTexDivider * Me.bokehDownsample), source.height / (lowTexDivider * Me.bokehDownsample), 0, RenderTextureFormat.ARGBHalf)
				Me.bokehSource.filterMode = FilterMode.Bilinear
				Me.bokehSource2.filterMode = FilterMode.Bilinear
				RenderTexture.active = Me.bokehSource2
				GL.Clear(False, True, New Color(0F, 0F, 0F, 0F))
			End If
			source.filterMode = FilterMode.Bilinear
			Me.finalDefocus.filterMode = FilterMode.Bilinear
			Me.mediumRezWorkTexture.filterMode = FilterMode.Bilinear
			Me.lowRezWorkTexture.filterMode = FilterMode.Bilinear
			If Me.foregroundTexture Then
				Me.foregroundTexture.filterMode = FilterMode.Bilinear
			End If
		End Sub

		' Token: 0x0400562B RID: 22059
		Private Shared SMOOTH_DOWNSAMPLE_PASS As Integer = 6

		' Token: 0x0400562C RID: 22060
		Private Shared BOKEH_EXTRA_BLUR As Single = 2F

		' Token: 0x0400562D RID: 22061
		Public quality As DepthOfFieldDeprecated.Dof34QualitySetting = DepthOfFieldDeprecated.Dof34QualitySetting.OnlyBackground

		' Token: 0x0400562E RID: 22062
		Public resolution As DepthOfFieldDeprecated.DofResolution = DepthOfFieldDeprecated.DofResolution.Low

		' Token: 0x0400562F RID: 22063
		Public simpleTweakMode As Boolean = True

		' Token: 0x04005630 RID: 22064
		Public focalPoint As Single = 1F

		' Token: 0x04005631 RID: 22065
		Public smoothness As Single = 0.5F

		' Token: 0x04005632 RID: 22066
		Public focalZDistance As Single

		' Token: 0x04005633 RID: 22067
		Public focalZStartCurve As Single = 1F

		' Token: 0x04005634 RID: 22068
		Public focalZEndCurve As Single = 1F

		' Token: 0x04005635 RID: 22069
		Private focalStartCurve As Single = 2F

		' Token: 0x04005636 RID: 22070
		Private focalEndCurve As Single = 2F

		' Token: 0x04005637 RID: 22071
		Private focalDistance01 As Single = 0.1F

		' Token: 0x04005638 RID: 22072
		Public objectFocus As Transform

		' Token: 0x04005639 RID: 22073
		Public focalSize As Single

		' Token: 0x0400563A RID: 22074
		Public bluriness As DepthOfFieldDeprecated.DofBlurriness = DepthOfFieldDeprecated.DofBlurriness.High

		' Token: 0x0400563B RID: 22075
		Public maxBlurSpread As Single = 1.75F

		' Token: 0x0400563C RID: 22076
		Public foregroundBlurExtrude As Single = 1.15F

		' Token: 0x0400563D RID: 22077
		Public dofBlurShader As Shader

		' Token: 0x0400563E RID: 22078
		Private dofBlurMaterial As Material

		' Token: 0x0400563F RID: 22079
		Public dofShader As Shader

		' Token: 0x04005640 RID: 22080
		Private dofMaterial As Material

		' Token: 0x04005641 RID: 22081
		Public visualize As Boolean

		' Token: 0x04005642 RID: 22082
		Public bokehDestination As DepthOfFieldDeprecated.BokehDestination = DepthOfFieldDeprecated.BokehDestination.Background

		' Token: 0x04005643 RID: 22083
		Private widthOverHeight As Single = 1.25F

		' Token: 0x04005644 RID: 22084
		Private oneOverBaseSize As Single = 0.001953125F

		' Token: 0x04005645 RID: 22085
		Public bokeh As Boolean

		' Token: 0x04005646 RID: 22086
		Public bokehSupport As Boolean = True

		' Token: 0x04005647 RID: 22087
		Public bokehShader As Shader

		' Token: 0x04005648 RID: 22088
		Public bokehTexture As Texture2D

		' Token: 0x04005649 RID: 22089
		Public bokehScale As Single = 2.4F

		' Token: 0x0400564A RID: 22090
		Public bokehIntensity As Single = 0.15F

		' Token: 0x0400564B RID: 22091
		Public bokehThresholdContrast As Single = 0.1F

		' Token: 0x0400564C RID: 22092
		Public bokehThresholdLuminance As Single = 0.55F

		' Token: 0x0400564D RID: 22093
		Public bokehDownsample As Integer = 1

		' Token: 0x0400564E RID: 22094
		Private bokehMaterial As Material

		' Token: 0x0400564F RID: 22095
		Private _camera As Camera

		' Token: 0x04005650 RID: 22096
		Private foregroundTexture As RenderTexture

		' Token: 0x04005651 RID: 22097
		Private mediumRezWorkTexture As RenderTexture

		' Token: 0x04005652 RID: 22098
		Private finalDefocus As RenderTexture

		' Token: 0x04005653 RID: 22099
		Private lowRezWorkTexture As RenderTexture

		' Token: 0x04005654 RID: 22100
		Private bokehSource As RenderTexture

		' Token: 0x04005655 RID: 22101
		Private bokehSource2 As RenderTexture

		' Token: 0x02000CD3 RID: 3283
		Public Enum Dof34QualitySetting
			' Token: 0x04005657 RID: 22103
			OnlyBackground = 1
			' Token: 0x04005658 RID: 22104
			BackgroundAndForeground
		End Enum

		' Token: 0x02000CD4 RID: 3284
		Public Enum DofResolution
			' Token: 0x0400565A RID: 22106
			High = 2
			' Token: 0x0400565B RID: 22107
			Medium
			' Token: 0x0400565C RID: 22108
			Low
		End Enum

		' Token: 0x02000CD5 RID: 3285
		Public Enum DofBlurriness
			' Token: 0x0400565E RID: 22110
			Low = 1
			' Token: 0x0400565F RID: 22111
			High
			' Token: 0x04005660 RID: 22112
			VeryHigh = 4
		End Enum

		' Token: 0x02000CD6 RID: 3286
		Public Enum BokehDestination
			' Token: 0x04005662 RID: 22114
			Background = 1
			' Token: 0x04005663 RID: 22115
			Foreground
			' Token: 0x04005664 RID: 22116
			BackgroundAndForeground
		End Enum
	End Class
End Namespace
