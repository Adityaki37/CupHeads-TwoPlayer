Imports System
Imports UnityEngine
Imports UnityStandardAssets.ImageEffects

' Token: 0x020003DF RID: 991
<ExecuteInEditMode()>
<RequireComponent(GetType(Camera))>
Public Class BlurGamma
	Inherits PostEffectsBase

	' Token: 0x06000D4B RID: 3403 RVA: 0x0008CEDA File Offset: 0x0008B2DA
	Public Overrides Function CheckResources() As Boolean
		MyBase.CheckSupport(False)
		Me.blurMaterial = MyBase.CheckShaderAndCreateMaterial(Me.blurShader, Me.blurMaterial)
		If Not Me.isSupported Then
			MyBase.ReportAutoDisable()
		End If
		Return Me.isSupported
	End Function

	' Token: 0x06000D4C RID: 3404 RVA: 0x0008CF13 File Offset: 0x0008B313
	Public Sub OnDisable()
		If Me.blurMaterial Then
			Global.UnityEngine.[Object].DestroyImmediate(Me.blurMaterial)
		End If
	End Sub

	' Token: 0x06000D4D RID: 3405 RVA: 0x0008CF30 File Offset: 0x0008B330
	Public Sub OnRenderImage(source As RenderTexture, destination As RenderTexture)
		If Not Me.CheckResources() Then
			Graphics.Blit(source, destination)
			Return
		End If
		Dim num As Single = CSng(source.width) / CSng(source.height)
		Dim num2 As Single = If((num >= 1.7777778F), 1F, (num / 1.7777778F))
		num2 *= 1F - 0.1F * SettingsData.Data.overscan
		Dim num3 As Single = CSng(source.height) / 1080F
		num3 *= num2
		If SettingsData.Data.filter = BlurGamma.Filter.BW Then
			num3 *= 1.35F
		End If
		Me.blurMaterial.SetVector("_Parameter", New Vector4(Me.blurSize * num3, -Me.blurSize * num3, Mathf.Pow(1.4F, -SettingsData.Data.Brightness), 0F))
		source.filterMode = FilterMode.Bilinear
		Dim temporary As RenderTexture = RenderTexture.GetTemporary(source.width, source.height, 0, source.format)
		Graphics.Blit(source, temporary, Me.blurMaterial, 0)
		Dim num4 As Integer = 1
		Dim filter As BlurGamma.Filter = SettingsData.Data.filter
		If filter <> BlurGamma.Filter.TwoStrip Then
			If filter = BlurGamma.Filter.BW Then
				num4 += 2
			End If
		Else
			num4 += 1
		End If
		Graphics.Blit(temporary, destination, Me.blurMaterial, num4)
		RenderTexture.ReleaseTemporary(temporary)
	End Sub

	' Token: 0x040016BE RID: 5822
	<Range(0F, 10F)>
	Public blurSize As Single = 3F

	' Token: 0x040016BF RID: 5823
	<Range(1F, 4F)>
	Public blurIterations As Integer = 2

	' Token: 0x040016C0 RID: 5824
	Public blurShader As Shader

	' Token: 0x040016C1 RID: 5825
	Private blurMaterial As Material

	' Token: 0x020003E0 RID: 992
	Public Enum Filter
		' Token: 0x040016C3 RID: 5827
		None
		' Token: 0x040016C4 RID: 5828
		TwoStrip
		' Token: 0x040016C5 RID: 5829
		BW
		' Token: 0x040016C6 RID: 5830
		Chalice
	End Enum
End Class
