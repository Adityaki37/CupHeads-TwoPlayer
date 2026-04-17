Imports System
Imports UnityEngine

Namespace UnityStandardAssets.ImageEffects
	' Token: 0x02000CCE RID: 3278
	<ExecuteInEditMode()>
	<RequireComponent(GetType(Camera))>
	<AddComponentMenu("Image Effects/Edge Detection/Crease Shading")>
	Friend Class CreaseShading
		Inherits PostEffectsBase

		' Token: 0x060051F6 RID: 20982 RVA: 0x0029F4C4 File Offset: 0x0029D8C4
		Public Overrides Function CheckResources() As Boolean
			MyBase.CheckSupport(True)
			Me.blurMaterial = MyBase.CheckShaderAndCreateMaterial(Me.blurShader, Me.blurMaterial)
			Me.depthFetchMaterial = MyBase.CheckShaderAndCreateMaterial(Me.depthFetchShader, Me.depthFetchMaterial)
			Me.creaseApplyMaterial = MyBase.CheckShaderAndCreateMaterial(Me.creaseApplyShader, Me.creaseApplyMaterial)
			If Not Me.isSupported Then
				MyBase.ReportAutoDisable()
			End If
			Return Me.isSupported
		End Function

		' Token: 0x060051F7 RID: 20983 RVA: 0x0029F538 File Offset: 0x0029D938
		Private Sub OnRenderImage(source As RenderTexture, destination As RenderTexture)
			If Not Me.CheckResources() Then
				Graphics.Blit(source, destination)
				Return
			End If
			Dim width As Integer = source.width
			Dim height As Integer = source.height
			Dim num As Single = 1F * CSng(width) / (1F * CSng(height))
			Dim num2 As Single = 0.001953125F
			Dim temporary As RenderTexture = RenderTexture.GetTemporary(width, height, 0)
			Dim renderTexture As RenderTexture = RenderTexture.GetTemporary(width / 2, height / 2, 0)
			Graphics.Blit(source, temporary, Me.depthFetchMaterial)
			Graphics.Blit(temporary, renderTexture)
			For i As Integer = 0 To Me.softness - 1
				Dim renderTexture2 As RenderTexture = RenderTexture.GetTemporary(width / 2, height / 2, 0)
				Me.blurMaterial.SetVector("offsets", New Vector4(0F, Me.spread * num2, 0F, 0F))
				Graphics.Blit(renderTexture, renderTexture2, Me.blurMaterial)
				RenderTexture.ReleaseTemporary(renderTexture)
				renderTexture = renderTexture2
				renderTexture2 = RenderTexture.GetTemporary(width / 2, height / 2, 0)
				Me.blurMaterial.SetVector("offsets", New Vector4(Me.spread * num2 / num, 0F, 0F, 0F))
				Graphics.Blit(renderTexture, renderTexture2, Me.blurMaterial)
				RenderTexture.ReleaseTemporary(renderTexture)
				renderTexture = renderTexture2
			Next
			Me.creaseApplyMaterial.SetTexture("_HrDepthTex", temporary)
			Me.creaseApplyMaterial.SetTexture("_LrDepthTex", renderTexture)
			Me.creaseApplyMaterial.SetFloat("intensity", Me.intensity)
			Graphics.Blit(source, destination, Me.creaseApplyMaterial)
			RenderTexture.ReleaseTemporary(temporary)
			RenderTexture.ReleaseTemporary(renderTexture)
		End Sub

		' Token: 0x04005603 RID: 22019
		Public intensity As Single = 0.5F

		' Token: 0x04005604 RID: 22020
		Public softness As Integer = 1

		' Token: 0x04005605 RID: 22021
		Public spread As Single = 1F

		' Token: 0x04005606 RID: 22022
		Public blurShader As Shader

		' Token: 0x04005607 RID: 22023
		Private blurMaterial As Material

		' Token: 0x04005608 RID: 22024
		Public depthFetchShader As Shader

		' Token: 0x04005609 RID: 22025
		Private depthFetchMaterial As Material

		' Token: 0x0400560A RID: 22026
		Public creaseApplyShader As Shader

		' Token: 0x0400560B RID: 22027
		Private creaseApplyMaterial As Material
	End Class
End Namespace
