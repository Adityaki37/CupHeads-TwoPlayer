Imports System
Imports UnityEngine

Namespace UnityStandardAssets.ImageEffects
	' Token: 0x02000CC8 RID: 3272
	<ExecuteInEditMode()>
	<AddComponentMenu("Image Effects/Color Adjustments/Color Correction (Curves, Saturation)")>
	Public Class ColorCorrectionCurves
		Inherits PostEffectsBase

		' Token: 0x060051D8 RID: 20952 RVA: 0x0029E54B File Offset: 0x0029C94B
		Private Sub Start()
			MyBase.Start()
			Me.updateTexturesOnStartup = True
		End Sub

		' Token: 0x060051D9 RID: 20953 RVA: 0x0029E55A File Offset: 0x0029C95A
		Private Sub Awake()
		End Sub

		' Token: 0x060051DA RID: 20954 RVA: 0x0029E55C File Offset: 0x0029C95C
		Public Overrides Function CheckResources() As Boolean
			MyBase.CheckSupport(Me.mode = ColorCorrectionCurves.ColorCorrectionMode.Advanced)
			Me.ccMaterial = MyBase.CheckShaderAndCreateMaterial(Me.simpleColorCorrectionCurvesShader, Me.ccMaterial)
			Me.ccDepthMaterial = MyBase.CheckShaderAndCreateMaterial(Me.colorCorrectionCurvesShader, Me.ccDepthMaterial)
			Me.selectiveCcMaterial = MyBase.CheckShaderAndCreateMaterial(Me.colorCorrectionSelectiveShader, Me.selectiveCcMaterial)
			If Not Me.rgbChannelTex Then
				Me.rgbChannelTex = New Texture2D(256, 4, TextureFormat.ARGB32, False, True)
			End If
			If Not Me.rgbDepthChannelTex Then
				Me.rgbDepthChannelTex = New Texture2D(256, 4, TextureFormat.ARGB32, False, True)
			End If
			If Not Me.zCurveTex Then
				Me.zCurveTex = New Texture2D(256, 1, TextureFormat.ARGB32, False, True)
			End If
			Me.rgbChannelTex.hideFlags = HideFlags.DontSave
			Me.rgbDepthChannelTex.hideFlags = HideFlags.DontSave
			Me.zCurveTex.hideFlags = HideFlags.DontSave
			Me.rgbChannelTex.wrapMode = TextureWrapMode.Clamp
			Me.rgbDepthChannelTex.wrapMode = TextureWrapMode.Clamp
			Me.zCurveTex.wrapMode = TextureWrapMode.Clamp
			If Not Me.isSupported Then
				MyBase.ReportAutoDisable()
			End If
			Return Me.isSupported
		End Function

		' Token: 0x060051DB RID: 20955 RVA: 0x0029E690 File Offset: 0x0029CA90
		Public Sub UpdateParameters()
			Me.CheckResources()
			If Me.redChannel IsNot Nothing AndAlso Me.greenChannel IsNot Nothing AndAlso Me.blueChannel IsNot Nothing Then
				Dim num As Single = 0F
				While num <= 1F
					Dim num2 As Single = Mathf.Clamp(Me.redChannel.Evaluate(num), 0F, 1F)
					Dim num3 As Single = Mathf.Clamp(Me.greenChannel.Evaluate(num), 0F, 1F)
					Dim num4 As Single = Mathf.Clamp(Me.blueChannel.Evaluate(num), 0F, 1F)
					Me.rgbChannelTex.SetPixel(CInt(Mathf.Floor(num * 255F)), 0, New Color(num2, num2, num2))
					Me.rgbChannelTex.SetPixel(CInt(Mathf.Floor(num * 255F)), 1, New Color(num3, num3, num3))
					Me.rgbChannelTex.SetPixel(CInt(Mathf.Floor(num * 255F)), 2, New Color(num4, num4, num4))
					Dim num5 As Single = Mathf.Clamp(Me.zCurve.Evaluate(num), 0F, 1F)
					Me.zCurveTex.SetPixel(CInt(Mathf.Floor(num * 255F)), 0, New Color(num5, num5, num5))
					num2 = Mathf.Clamp(Me.depthRedChannel.Evaluate(num), 0F, 1F)
					num3 = Mathf.Clamp(Me.depthGreenChannel.Evaluate(num), 0F, 1F)
					num4 = Mathf.Clamp(Me.depthBlueChannel.Evaluate(num), 0F, 1F)
					Me.rgbDepthChannelTex.SetPixel(CInt(Mathf.Floor(num * 255F)), 0, New Color(num2, num2, num2))
					Me.rgbDepthChannelTex.SetPixel(CInt(Mathf.Floor(num * 255F)), 1, New Color(num3, num3, num3))
					Me.rgbDepthChannelTex.SetPixel(CInt(Mathf.Floor(num * 255F)), 2, New Color(num4, num4, num4))
					num += 0.003921569F
				End While
				Me.rgbChannelTex.Apply()
				Me.rgbDepthChannelTex.Apply()
				Me.zCurveTex.Apply()
			End If
		End Sub

		' Token: 0x060051DC RID: 20956 RVA: 0x0029E8B3 File Offset: 0x0029CCB3
		Private Sub UpdateTextures()
			Me.UpdateParameters()
		End Sub

		' Token: 0x060051DD RID: 20957 RVA: 0x0029E8BC File Offset: 0x0029CCBC
		Private Sub OnRenderImage(source As RenderTexture, destination As RenderTexture)
			If Not Me.CheckResources() Then
				Graphics.Blit(source, destination)
				Return
			End If
			If Me.updateTexturesOnStartup Then
				Me.UpdateParameters()
				Me.updateTexturesOnStartup = False
			End If
			If Me.useDepthCorrection Then
				MyBase.GetComponent(Of Camera)().depthTextureMode = MyBase.GetComponent(Of Camera)().depthTextureMode Or DepthTextureMode.Depth
			End If
			Dim renderTexture As RenderTexture = destination
			If Me.selectiveCc Then
				renderTexture = RenderTexture.GetTemporary(source.width, source.height)
			End If
			If Me.useDepthCorrection Then
				Me.ccDepthMaterial.SetTexture("_RgbTex", Me.rgbChannelTex)
				Me.ccDepthMaterial.SetTexture("_ZCurve", Me.zCurveTex)
				Me.ccDepthMaterial.SetTexture("_RgbDepthTex", Me.rgbDepthChannelTex)
				Me.ccDepthMaterial.SetFloat("_Saturation", Me.saturation)
				Graphics.Blit(source, renderTexture, Me.ccDepthMaterial)
			Else
				Me.ccMaterial.SetTexture("_RgbTex", Me.rgbChannelTex)
				Me.ccMaterial.SetFloat("_Saturation", Me.saturation)
				Graphics.Blit(source, renderTexture, Me.ccMaterial)
			End If
			If Me.selectiveCc Then
				Me.selectiveCcMaterial.SetColor("selColor", Me.selectiveFromColor)
				Me.selectiveCcMaterial.SetColor("targetColor", Me.selectiveToColor)
				Graphics.Blit(renderTexture, destination, Me.selectiveCcMaterial)
				RenderTexture.ReleaseTemporary(renderTexture)
			End If
		End Sub

		' Token: 0x040055CF RID: 21967
		Public redChannel As AnimationCurve = New AnimationCurve(New Keyframe() { New Keyframe(0F, 0F), New Keyframe(1F, 1F) })

		' Token: 0x040055D0 RID: 21968
		Public greenChannel As AnimationCurve = New AnimationCurve(New Keyframe() { New Keyframe(0F, 0F), New Keyframe(1F, 1F) })

		' Token: 0x040055D1 RID: 21969
		Public blueChannel As AnimationCurve = New AnimationCurve(New Keyframe() { New Keyframe(0F, 0F), New Keyframe(1F, 1F) })

		' Token: 0x040055D2 RID: 21970
		Public useDepthCorrection As Boolean

		' Token: 0x040055D3 RID: 21971
		Public zCurve As AnimationCurve = New AnimationCurve(New Keyframe() { New Keyframe(0F, 0F), New Keyframe(1F, 1F) })

		' Token: 0x040055D4 RID: 21972
		Public depthRedChannel As AnimationCurve = New AnimationCurve(New Keyframe() { New Keyframe(0F, 0F), New Keyframe(1F, 1F) })

		' Token: 0x040055D5 RID: 21973
		Public depthGreenChannel As AnimationCurve = New AnimationCurve(New Keyframe() { New Keyframe(0F, 0F), New Keyframe(1F, 1F) })

		' Token: 0x040055D6 RID: 21974
		Public depthBlueChannel As AnimationCurve = New AnimationCurve(New Keyframe() { New Keyframe(0F, 0F), New Keyframe(1F, 1F) })

		' Token: 0x040055D7 RID: 21975
		Private ccMaterial As Material

		' Token: 0x040055D8 RID: 21976
		Private ccDepthMaterial As Material

		' Token: 0x040055D9 RID: 21977
		Private selectiveCcMaterial As Material

		' Token: 0x040055DA RID: 21978
		Private rgbChannelTex As Texture2D

		' Token: 0x040055DB RID: 21979
		Private rgbDepthChannelTex As Texture2D

		' Token: 0x040055DC RID: 21980
		Private zCurveTex As Texture2D

		' Token: 0x040055DD RID: 21981
		Public saturation As Single = 1F

		' Token: 0x040055DE RID: 21982
		Public selectiveCc As Boolean

		' Token: 0x040055DF RID: 21983
		Public selectiveFromColor As Color = Color.white

		' Token: 0x040055E0 RID: 21984
		Public selectiveToColor As Color = Color.white

		' Token: 0x040055E1 RID: 21985
		Public mode As ColorCorrectionCurves.ColorCorrectionMode

		' Token: 0x040055E2 RID: 21986
		Public updateTextures As Boolean = True

		' Token: 0x040055E3 RID: 21987
		Public colorCorrectionCurvesShader As Shader

		' Token: 0x040055E4 RID: 21988
		Public simpleColorCorrectionCurvesShader As Shader

		' Token: 0x040055E5 RID: 21989
		Public colorCorrectionSelectiveShader As Shader

		' Token: 0x040055E6 RID: 21990
		Private updateTexturesOnStartup As Boolean = True

		' Token: 0x02000CC9 RID: 3273
		Public Enum ColorCorrectionMode
			' Token: 0x040055E8 RID: 21992
			Simple
			' Token: 0x040055E9 RID: 21993
			Advanced
		End Enum
	End Class
End Namespace
