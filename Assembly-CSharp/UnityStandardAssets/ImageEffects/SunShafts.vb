Imports System
Imports UnityEngine

Namespace UnityStandardAssets.ImageEffects
	' Token: 0x02000CEA RID: 3306
	<ExecuteInEditMode()>
	<RequireComponent(GetType(Camera))>
	<AddComponentMenu("Image Effects/Rendering/Sun Shafts")>
	Public Class SunShafts
		Inherits PostEffectsBase

		' Token: 0x06005262 RID: 21090 RVA: 0x002A3D34 File Offset: 0x002A2134
		Public Overrides Function CheckResources() As Boolean
			MyBase.CheckSupport(Me.useDepthTexture)
			Me.sunShaftsMaterial = MyBase.CheckShaderAndCreateMaterial(Me.sunShaftsShader, Me.sunShaftsMaterial)
			Me.simpleClearMaterial = MyBase.CheckShaderAndCreateMaterial(Me.simpleClearShader, Me.simpleClearMaterial)
			If Not Me.isSupported Then
				MyBase.ReportAutoDisable()
			End If
			Return Me.isSupported
		End Function

		' Token: 0x06005263 RID: 21091 RVA: 0x002A3D98 File Offset: 0x002A2198
		Private Sub OnRenderImage(source As RenderTexture, destination As RenderTexture)
			If Not Me.CheckResources() Then
				Graphics.Blit(source, destination)
				Return
			End If
			If Me.useDepthTexture Then
				MyBase.GetComponent(Of Camera)().depthTextureMode = MyBase.GetComponent(Of Camera)().depthTextureMode Or DepthTextureMode.Depth
			End If
			Dim num As Integer = 4
			If Me.resolution = SunShafts.SunShaftsResolution.Normal Then
				num = 2
			ElseIf Me.resolution = SunShafts.SunShaftsResolution.High Then
				num = 1
			End If
			Dim vector As Vector3 = Vector3.one * 0.5F
			If Me.sunTransform Then
				vector = MyBase.GetComponent(Of Camera)().WorldToViewportPoint(Me.sunTransform.position)
			Else
				vector = New Vector3(0.5F, 0.5F, 0F)
			End If
			Dim num2 As Integer = source.width / num
			Dim num3 As Integer = source.height / num
			Dim renderTexture As RenderTexture = RenderTexture.GetTemporary(num2, num3, 0)
			Me.sunShaftsMaterial.SetVector("_BlurRadius4", New Vector4(1F, 1F, 0F, 0F) * Me.sunShaftBlurRadius)
			Me.sunShaftsMaterial.SetVector("_SunPosition", New Vector4(vector.x, vector.y, vector.z, Me.maxRadius))
			Me.sunShaftsMaterial.SetVector("_SunThreshold", Me.sunThreshold)
			If Not Me.useDepthTexture Then
				Dim renderTextureFormat As RenderTextureFormat = If((Not MyBase.GetComponent(Of Camera)().allowHDR), RenderTextureFormat.[Default], RenderTextureFormat.DefaultHDR)
				Dim temporary As RenderTexture = RenderTexture.GetTemporary(source.width, source.height, 0, renderTextureFormat)
				RenderTexture.active = temporary
				GL.ClearWithSkybox(False, MyBase.GetComponent(Of Camera)())
				Me.sunShaftsMaterial.SetTexture("_Skybox", temporary)
				Graphics.Blit(source, renderTexture, Me.sunShaftsMaterial, 3)
				RenderTexture.ReleaseTemporary(temporary)
			Else
				Graphics.Blit(source, renderTexture, Me.sunShaftsMaterial, 2)
			End If
			MyBase.DrawBorder(renderTexture, Me.simpleClearMaterial)
			Me.radialBlurIterations = Mathf.Clamp(Me.radialBlurIterations, 1, 4)
			Dim num4 As Single = Me.sunShaftBlurRadius * 0.0013020834F
			Me.sunShaftsMaterial.SetVector("_BlurRadius4", New Vector4(num4, num4, 0F, 0F))
			Me.sunShaftsMaterial.SetVector("_SunPosition", New Vector4(vector.x, vector.y, vector.z, Me.maxRadius))
			For i As Integer = 0 To Me.radialBlurIterations - 1
				Dim temporary2 As RenderTexture = RenderTexture.GetTemporary(num2, num3, 0)
				Graphics.Blit(renderTexture, temporary2, Me.sunShaftsMaterial, 1)
				RenderTexture.ReleaseTemporary(renderTexture)
				num4 = Me.sunShaftBlurRadius * ((CSng(i) * 2F + 1F) * 6F) / 768F
				Me.sunShaftsMaterial.SetVector("_BlurRadius4", New Vector4(num4, num4, 0F, 0F))
				renderTexture = RenderTexture.GetTemporary(num2, num3, 0)
				Graphics.Blit(temporary2, renderTexture, Me.sunShaftsMaterial, 1)
				RenderTexture.ReleaseTemporary(temporary2)
				num4 = Me.sunShaftBlurRadius * ((CSng(i) * 2F + 2F) * 6F) / 768F
				Me.sunShaftsMaterial.SetVector("_BlurRadius4", New Vector4(num4, num4, 0F, 0F))
			Next
			If vector.z >= 0F Then
				Me.sunShaftsMaterial.SetVector("_SunColor", New Vector4(Me.sunColor.r, Me.sunColor.g, Me.sunColor.b, Me.sunColor.a) * Me.sunShaftIntensity)
			Else
				Me.sunShaftsMaterial.SetVector("_SunColor", Vector4.zero)
			End If
			Me.sunShaftsMaterial.SetTexture("_ColorBuffer", renderTexture)
			Graphics.Blit(source, destination, Me.sunShaftsMaterial, If((Me.screenBlendMode <> SunShafts.ShaftsScreenBlendMode.Screen), 4, 0))
			RenderTexture.ReleaseTemporary(renderTexture)
		End Sub

		' Token: 0x040056D4 RID: 22228
		Public resolution As SunShafts.SunShaftsResolution = SunShafts.SunShaftsResolution.Normal

		' Token: 0x040056D5 RID: 22229
		Public screenBlendMode As SunShafts.ShaftsScreenBlendMode

		' Token: 0x040056D6 RID: 22230
		Public sunTransform As Transform

		' Token: 0x040056D7 RID: 22231
		Public radialBlurIterations As Integer = 2

		' Token: 0x040056D8 RID: 22232
		Public sunColor As Color = Color.white

		' Token: 0x040056D9 RID: 22233
		Public sunThreshold As Color = New Color(0.87F, 0.74F, 0.65F)

		' Token: 0x040056DA RID: 22234
		Public sunShaftBlurRadius As Single = 2.5F

		' Token: 0x040056DB RID: 22235
		Public sunShaftIntensity As Single = 1.15F

		' Token: 0x040056DC RID: 22236
		Public maxRadius As Single = 0.75F

		' Token: 0x040056DD RID: 22237
		Public useDepthTexture As Boolean = True

		' Token: 0x040056DE RID: 22238
		Public sunShaftsShader As Shader

		' Token: 0x040056DF RID: 22239
		Private sunShaftsMaterial As Material

		' Token: 0x040056E0 RID: 22240
		Public simpleClearShader As Shader

		' Token: 0x040056E1 RID: 22241
		Private simpleClearMaterial As Material

		' Token: 0x02000CEB RID: 3307
		Public Enum SunShaftsResolution
			' Token: 0x040056E3 RID: 22243
			Low
			' Token: 0x040056E4 RID: 22244
			Normal
			' Token: 0x040056E5 RID: 22245
			High
		End Enum

		' Token: 0x02000CEC RID: 3308
		Public Enum ShaftsScreenBlendMode
			' Token: 0x040056E7 RID: 22247
			Screen
			' Token: 0x040056E8 RID: 22248
			Add
		End Enum
	End Class
End Namespace
