Imports System
Imports UnityEngine

Namespace UnityStandardAssets.ImageEffects
	' Token: 0x02000CF0 RID: 3312
	<ExecuteInEditMode()>
	<RequireComponent(GetType(Camera))>
	<AddComponentMenu("Image Effects/Color Adjustments/Tonemapping")>
	Public Class Tonemapping
		Inherits PostEffectsBase

		' Token: 0x06005268 RID: 21096 RVA: 0x002A436C File Offset: 0x002A276C
		Public Overrides Function CheckResources() As Boolean
			MyBase.CheckSupport(False, True)
			Me.tonemapMaterial = MyBase.CheckShaderAndCreateMaterial(Me.tonemapper, Me.tonemapMaterial)
			If Not Me.curveTex AndAlso Me.type = Tonemapping.TonemapperType.UserCurve Then
				Me.curveTex = New Texture2D(256, 1, TextureFormat.ARGB32, False, True)
				Me.curveTex.filterMode = FilterMode.Bilinear
				Me.curveTex.wrapMode = TextureWrapMode.Clamp
				Me.curveTex.hideFlags = HideFlags.DontSave
			End If
			If Not Me.isSupported Then
				MyBase.ReportAutoDisable()
			End If
			Return Me.isSupported
		End Function

		' Token: 0x06005269 RID: 21097 RVA: 0x002A4408 File Offset: 0x002A2808
		Public Function UpdateCurve() As Single
			Dim num As Single = 1F
			If Me.remapCurve.keys.Length < 1 Then
				Me.remapCurve = New AnimationCurve(New Keyframe() { New Keyframe(0F, 0F), New Keyframe(2F, 1F) })
			End If
			If Me.remapCurve IsNot Nothing Then
				If Me.remapCurve.length > 0 Then
					num = Me.remapCurve(Me.remapCurve.length - 1).time
				End If
				Dim num2 As Single = 0F
				While num2 <= 1F
					Dim num3 As Single = Me.remapCurve.Evaluate(num2 * 1F * num)
					Me.curveTex.SetPixel(CInt(Mathf.Floor(num2 * 255F)), 0, New Color(num3, num3, num3))
					num2 += 0.003921569F
				End While
				Me.curveTex.Apply()
			End If
			Return 1F / num
		End Function

		' Token: 0x0600526A RID: 21098 RVA: 0x002A4518 File Offset: 0x002A2918
		Private Sub OnDisable()
			If Me.rt Then
				Global.UnityEngine.[Object].DestroyImmediate(Me.rt)
				Me.rt = Nothing
			End If
			If Me.tonemapMaterial Then
				Global.UnityEngine.[Object].DestroyImmediate(Me.tonemapMaterial)
				Me.tonemapMaterial = Nothing
			End If
			If Me.curveTex Then
				Global.UnityEngine.[Object].DestroyImmediate(Me.curveTex)
				Me.curveTex = Nothing
			End If
		End Sub

		' Token: 0x0600526B RID: 21099 RVA: 0x002A458C File Offset: 0x002A298C
		Private Function CreateInternalRenderTexture() As Boolean
			If Me.rt Then
				Return False
			End If
			Me.rtFormat = If((Not SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RGHalf)), RenderTextureFormat.ARGBHalf, RenderTextureFormat.RGHalf)
			Me.rt = New RenderTexture(1, 1, 0, Me.rtFormat)
			Me.rt.hideFlags = HideFlags.DontSave
			Return True
		End Function

		' Token: 0x0600526C RID: 21100 RVA: 0x002A45E8 File Offset: 0x002A29E8
		<ImageEffectTransformsToLDR()>
		Private Sub OnRenderImage(source As RenderTexture, destination As RenderTexture)
			If Not Me.CheckResources() Then
				Graphics.Blit(source, destination)
				Return
			End If
			Me.exposureAdjustment = If((Me.exposureAdjustment >= 0.001F), Me.exposureAdjustment, 0.001F)
			If Me.type = Tonemapping.TonemapperType.UserCurve Then
				Dim num As Single = Me.UpdateCurve()
				Me.tonemapMaterial.SetFloat("_RangeScale", num)
				Me.tonemapMaterial.SetTexture("_Curve", Me.curveTex)
				Graphics.Blit(source, destination, Me.tonemapMaterial, 4)
				Return
			End If
			If Me.type = Tonemapping.TonemapperType.SimpleReinhard Then
				Me.tonemapMaterial.SetFloat("_ExposureAdjustment", Me.exposureAdjustment)
				Graphics.Blit(source, destination, Me.tonemapMaterial, 6)
				Return
			End If
			If Me.type = Tonemapping.TonemapperType.Hable Then
				Me.tonemapMaterial.SetFloat("_ExposureAdjustment", Me.exposureAdjustment)
				Graphics.Blit(source, destination, Me.tonemapMaterial, 5)
				Return
			End If
			If Me.type = Tonemapping.TonemapperType.Photographic Then
				Me.tonemapMaterial.SetFloat("_ExposureAdjustment", Me.exposureAdjustment)
				Graphics.Blit(source, destination, Me.tonemapMaterial, 8)
				Return
			End If
			If Me.type = Tonemapping.TonemapperType.OptimizedHejiDawson Then
				Me.tonemapMaterial.SetFloat("_ExposureAdjustment", 0.5F * Me.exposureAdjustment)
				Graphics.Blit(source, destination, Me.tonemapMaterial, 7)
				Return
			End If
			Dim flag As Boolean = Me.CreateInternalRenderTexture()
			Dim temporary As RenderTexture = RenderTexture.GetTemporary(CInt(Me.adaptiveTextureSize), CInt(Me.adaptiveTextureSize), 0, Me.rtFormat)
			Graphics.Blit(source, temporary)
			Dim num2 As Integer = CInt(Mathf.Log(CSng(temporary.width) * 1F, 2F))
			Dim num3 As Integer = 2
			Dim array As RenderTexture() = New RenderTexture(num2 - 1) {}
			For i As Integer = 0 To num2 - 1
				array(i) = RenderTexture.GetTemporary(temporary.width / num3, temporary.width / num3, 0, Me.rtFormat)
				num3 *= 2
			Next
			Dim renderTexture As RenderTexture = array(num2 - 1)
			Graphics.Blit(temporary, array(0), Me.tonemapMaterial, 1)
			If Me.type = Tonemapping.TonemapperType.AdaptiveReinhardAutoWhite Then
				For j As Integer = 0 To num2 - 1 - 1
					Graphics.Blit(array(j), array(j + 1), Me.tonemapMaterial, 9)
					renderTexture = array(j + 1)
				Next
			ElseIf Me.type = Tonemapping.TonemapperType.AdaptiveReinhard Then
				For k As Integer = 0 To num2 - 1 - 1
					Graphics.Blit(array(k), array(k + 1))
					renderTexture = array(k + 1)
				Next
			End If
			Me.adaptionSpeed = If((Me.adaptionSpeed >= 0.001F), Me.adaptionSpeed, 0.001F)
			Me.tonemapMaterial.SetFloat("_AdaptionSpeed", Me.adaptionSpeed)
			Me.rt.MarkRestoreExpected()
			Graphics.Blit(renderTexture, Me.rt, Me.tonemapMaterial, If((Not flag), 2, 3))
			Me.middleGrey = If((Me.middleGrey >= 0.001F), Me.middleGrey, 0.001F)
			Me.tonemapMaterial.SetVector("_HdrParams", New Vector4(Me.middleGrey, Me.middleGrey, Me.middleGrey, Me.white * Me.white))
			Me.tonemapMaterial.SetTexture("_SmallTex", Me.rt)
			If Me.type = Tonemapping.TonemapperType.AdaptiveReinhard Then
				Graphics.Blit(source, destination, Me.tonemapMaterial, 0)
			ElseIf Me.type = Tonemapping.TonemapperType.AdaptiveReinhardAutoWhite Then
				Graphics.Blit(source, destination, Me.tonemapMaterial, 10)
			Else
				Global.Debug.LogError("No valid adaptive tonemapper type found!", Nothing)
				Graphics.Blit(source, destination)
			End If
			For l As Integer = 0 To num2 - 1
				RenderTexture.ReleaseTemporary(array(l))
			Next
			RenderTexture.ReleaseTemporary(temporary)
		End Sub

		' Token: 0x040056F7 RID: 22263
		Public type As Tonemapping.TonemapperType = Tonemapping.TonemapperType.Photographic

		' Token: 0x040056F8 RID: 22264
		Public adaptiveTextureSize As Tonemapping.AdaptiveTexSize = Tonemapping.AdaptiveTexSize.Square256

		' Token: 0x040056F9 RID: 22265
		Public remapCurve As AnimationCurve

		' Token: 0x040056FA RID: 22266
		Private curveTex As Texture2D

		' Token: 0x040056FB RID: 22267
		Public exposureAdjustment As Single = 1.5F

		' Token: 0x040056FC RID: 22268
		Public middleGrey As Single = 0.4F

		' Token: 0x040056FD RID: 22269
		Public white As Single = 2F

		' Token: 0x040056FE RID: 22270
		Public adaptionSpeed As Single = 1.5F

		' Token: 0x040056FF RID: 22271
		Public tonemapper As Shader

		' Token: 0x04005700 RID: 22272
		Public validRenderTextureFormat As Boolean = True

		' Token: 0x04005701 RID: 22273
		Private tonemapMaterial As Material

		' Token: 0x04005702 RID: 22274
		Private rt As RenderTexture

		' Token: 0x04005703 RID: 22275
		Private rtFormat As RenderTextureFormat = RenderTextureFormat.ARGBHalf

		' Token: 0x02000CF1 RID: 3313
		Public Enum TonemapperType
			' Token: 0x04005705 RID: 22277
			SimpleReinhard
			' Token: 0x04005706 RID: 22278
			UserCurve
			' Token: 0x04005707 RID: 22279
			Hable
			' Token: 0x04005708 RID: 22280
			Photographic
			' Token: 0x04005709 RID: 22281
			OptimizedHejiDawson
			' Token: 0x0400570A RID: 22282
			AdaptiveReinhard
			' Token: 0x0400570B RID: 22283
			AdaptiveReinhardAutoWhite
		End Enum

		' Token: 0x02000CF2 RID: 3314
		Public Enum AdaptiveTexSize
			' Token: 0x0400570D RID: 22285
			Square16 = 16
			' Token: 0x0400570E RID: 22286
			Square32 = 32
			' Token: 0x0400570F RID: 22287
			Square64 = 64
			' Token: 0x04005710 RID: 22288
			Square128 = 128
			' Token: 0x04005711 RID: 22289
			Square256 = 256
			' Token: 0x04005712 RID: 22290
			Square512 = 512
			' Token: 0x04005713 RID: 22291
			Square1024 = 1024
		End Enum
	End Class
End Namespace
