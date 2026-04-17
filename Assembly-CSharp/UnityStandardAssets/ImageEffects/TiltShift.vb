Imports System
Imports UnityEngine

Namespace UnityStandardAssets.ImageEffects
	' Token: 0x02000CED RID: 3309
	<RequireComponent(GetType(Camera))>
	<AddComponentMenu("Image Effects/Camera/Tilt Shift (Lens Blur)")>
	Friend Class TiltShift
		Inherits PostEffectsBase

		' Token: 0x06005265 RID: 21093 RVA: 0x002A41B1 File Offset: 0x002A25B1
		Public Overrides Function CheckResources() As Boolean
			MyBase.CheckSupport(True)
			Me.tiltShiftMaterial = MyBase.CheckShaderAndCreateMaterial(Me.tiltShiftShader, Me.tiltShiftMaterial)
			If Not Me.isSupported Then
				MyBase.ReportAutoDisable()
			End If
			Return Me.isSupported
		End Function

		' Token: 0x06005266 RID: 21094 RVA: 0x002A41EC File Offset: 0x002A25EC
		Private Sub OnRenderImage(source As RenderTexture, destination As RenderTexture)
			If Not Me.CheckResources() Then
				Graphics.Blit(source, destination)
				Return
			End If
			Me.tiltShiftMaterial.SetFloat("_BlurSize", If((Me.maxBlurSize >= 0F), Me.maxBlurSize, 0F))
			Me.tiltShiftMaterial.SetFloat("_BlurArea", Me.blurArea)
			source.filterMode = FilterMode.Bilinear
			Dim renderTexture As RenderTexture = destination
			If CSng(Me.downsample) > 0F Then
				renderTexture = RenderTexture.GetTemporary(source.width >> Me.downsample, source.height >> Me.downsample, 0, source.format)
				renderTexture.filterMode = FilterMode.Bilinear
			End If
			Dim num As Integer = CInt(Me.quality)
			num *= 2
			Graphics.Blit(source, renderTexture, Me.tiltShiftMaterial, If((Me.mode <> TiltShift.TiltShiftMode.TiltShiftMode), (num + 1), num))
			If Me.downsample > 0 Then
				Me.tiltShiftMaterial.SetTexture("_Blurred", renderTexture)
				Graphics.Blit(source, destination, Me.tiltShiftMaterial, 6)
			End If
			If renderTexture IsNot destination Then
				RenderTexture.ReleaseTemporary(renderTexture)
			End If
		End Sub

		' Token: 0x040056E9 RID: 22249
		Public mode As TiltShift.TiltShiftMode

		' Token: 0x040056EA RID: 22250
		Public quality As TiltShift.TiltShiftQuality = TiltShift.TiltShiftQuality.Normal

		' Token: 0x040056EB RID: 22251
		<Range(0F, 15F)>
		Public blurArea As Single = 1F

		' Token: 0x040056EC RID: 22252
		<Range(0F, 25F)>
		Public maxBlurSize As Single = 5F

		' Token: 0x040056ED RID: 22253
		<Range(0F, 1F)>
		Public downsample As Integer

		' Token: 0x040056EE RID: 22254
		Public tiltShiftShader As Shader

		' Token: 0x040056EF RID: 22255
		Private tiltShiftMaterial As Material

		' Token: 0x02000CEE RID: 3310
		Public Enum TiltShiftMode
			' Token: 0x040056F1 RID: 22257
			TiltShiftMode
			' Token: 0x040056F2 RID: 22258
			IrisMode
		End Enum

		' Token: 0x02000CEF RID: 3311
		Public Enum TiltShiftQuality
			' Token: 0x040056F4 RID: 22260
			Preview
			' Token: 0x040056F5 RID: 22261
			Normal
			' Token: 0x040056F6 RID: 22262
			High
		End Enum
	End Class
End Namespace
