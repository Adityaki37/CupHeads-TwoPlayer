Imports System
Imports UnityEngine

Namespace UnityStandardAssets.ImageEffects
	' Token: 0x02000CB4 RID: 3252
	<ExecuteInEditMode()>
	<RequireComponent(GetType(Camera))>
	<AddComponentMenu("Image Effects/Other/Antialiasing")>
	Public Class Antialiasing
		Inherits PostEffectsBase

		' Token: 0x060051A9 RID: 20905 RVA: 0x0029B5B0 File Offset: 0x002999B0
		Public Function CurrentAAMaterial() As Material
			Dim material As Material
			Select Case Me.mode
				Case AAMode.FXAA2
					material = Me.materialFXAAII
				Case AAMode.FXAA3Console
					material = Me.materialFXAAIII
				Case AAMode.FXAA1PresetA
					material = Me.materialFXAAPreset2
				Case AAMode.FXAA1PresetB
					material = Me.materialFXAAPreset3
				Case AAMode.NFAA
					material = Me.nfaa
				Case AAMode.SSAA
					material = Me.ssaa
				Case AAMode.DLAA
					material = Me.dlaa
				Case Else
					material = Nothing
			End Select
			Return material
		End Function

		' Token: 0x060051AA RID: 20906 RVA: 0x0029B64C File Offset: 0x00299A4C
		Public Overrides Function CheckResources() As Boolean
			MyBase.CheckSupport(False)
			Me.materialFXAAPreset2 = MyBase.CreateMaterial(Me.shaderFXAAPreset2, Me.materialFXAAPreset2)
			Me.materialFXAAPreset3 = MyBase.CreateMaterial(Me.shaderFXAAPreset3, Me.materialFXAAPreset3)
			Me.materialFXAAII = MyBase.CreateMaterial(Me.shaderFXAAII, Me.materialFXAAII)
			Me.materialFXAAIII = MyBase.CreateMaterial(Me.shaderFXAAIII, Me.materialFXAAIII)
			Me.nfaa = MyBase.CreateMaterial(Me.nfaaShader, Me.nfaa)
			Me.ssaa = MyBase.CreateMaterial(Me.ssaaShader, Me.ssaa)
			Me.dlaa = MyBase.CreateMaterial(Me.dlaaShader, Me.dlaa)
			If Not Me.ssaaShader.isSupported Then
				MyBase.NotSupported()
				MyBase.ReportAutoDisable()
			End If
			Return Me.isSupported
		End Function

		' Token: 0x060051AB RID: 20907 RVA: 0x0029B72C File Offset: 0x00299B2C
		Public Sub OnRenderImage(source As RenderTexture, destination As RenderTexture)
			If Not Me.CheckResources() Then
				Graphics.Blit(source, destination)
				Return
			End If
			If Me.mode = AAMode.FXAA3Console AndAlso Me.materialFXAAIII IsNot Nothing Then
				Me.materialFXAAIII.SetFloat("_EdgeThresholdMin", Me.edgeThresholdMin)
				Me.materialFXAAIII.SetFloat("_EdgeThreshold", Me.edgeThreshold)
				Me.materialFXAAIII.SetFloat("_EdgeSharpness", Me.edgeSharpness)
				Graphics.Blit(source, destination, Me.materialFXAAIII)
			ElseIf Me.mode = AAMode.FXAA1PresetB AndAlso Me.materialFXAAPreset3 IsNot Nothing Then
				Graphics.Blit(source, destination, Me.materialFXAAPreset3)
			ElseIf Me.mode = AAMode.FXAA1PresetA AndAlso Me.materialFXAAPreset2 IsNot Nothing Then
				source.anisoLevel = 4
				Graphics.Blit(source, destination, Me.materialFXAAPreset2)
				source.anisoLevel = 0
			ElseIf Me.mode = AAMode.FXAA2 AndAlso Me.materialFXAAII IsNot Nothing Then
				Graphics.Blit(source, destination, Me.materialFXAAII)
			ElseIf Me.mode = AAMode.SSAA AndAlso Me.ssaa IsNot Nothing Then
				Graphics.Blit(source, destination, Me.ssaa)
			ElseIf Me.mode = AAMode.DLAA AndAlso Me.dlaa IsNot Nothing Then
				source.anisoLevel = 0
				Dim temporary As RenderTexture = RenderTexture.GetTemporary(source.width, source.height)
				Graphics.Blit(source, temporary, Me.dlaa, 0)
				Graphics.Blit(temporary, destination, Me.dlaa, If((Not Me.dlaaSharp), 1, 2))
				RenderTexture.ReleaseTemporary(temporary)
			ElseIf Me.mode = AAMode.NFAA AndAlso Me.nfaa IsNot Nothing Then
				source.anisoLevel = 0
				Me.nfaa.SetFloat("_OffsetScale", Me.offsetScale)
				Me.nfaa.SetFloat("_BlurRadius", Me.blurRadius)
				Graphics.Blit(source, destination, Me.nfaa, If((Not Me.showGeneratedNormals), 0, 1))
			Else
				Graphics.Blit(source, destination)
			End If
		End Sub

		' Token: 0x0400551B RID: 21787
		Public mode As AAMode = AAMode.FXAA3Console

		' Token: 0x0400551C RID: 21788
		Public showGeneratedNormals As Boolean

		' Token: 0x0400551D RID: 21789
		Public offsetScale As Single = 0.2F

		' Token: 0x0400551E RID: 21790
		Public blurRadius As Single = 18F

		' Token: 0x0400551F RID: 21791
		Public edgeThresholdMin As Single = 0.05F

		' Token: 0x04005520 RID: 21792
		Public edgeThreshold As Single = 0.2F

		' Token: 0x04005521 RID: 21793
		Public edgeSharpness As Single = 4F

		' Token: 0x04005522 RID: 21794
		Public dlaaSharp As Boolean

		' Token: 0x04005523 RID: 21795
		Public ssaaShader As Shader

		' Token: 0x04005524 RID: 21796
		Private ssaa As Material

		' Token: 0x04005525 RID: 21797
		Public dlaaShader As Shader

		' Token: 0x04005526 RID: 21798
		Private dlaa As Material

		' Token: 0x04005527 RID: 21799
		Public nfaaShader As Shader

		' Token: 0x04005528 RID: 21800
		Private nfaa As Material

		' Token: 0x04005529 RID: 21801
		Public shaderFXAAPreset2 As Shader

		' Token: 0x0400552A RID: 21802
		Private materialFXAAPreset2 As Material

		' Token: 0x0400552B RID: 21803
		Public shaderFXAAPreset3 As Shader

		' Token: 0x0400552C RID: 21804
		Private materialFXAAPreset3 As Material

		' Token: 0x0400552D RID: 21805
		Public shaderFXAAII As Shader

		' Token: 0x0400552E RID: 21806
		Private materialFXAAII As Material

		' Token: 0x0400552F RID: 21807
		Public shaderFXAAIII As Shader

		' Token: 0x04005530 RID: 21808
		Private materialFXAAIII As Material
	End Class
End Namespace
