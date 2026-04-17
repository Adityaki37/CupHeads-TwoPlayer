Imports System
Imports UnityEngine

Namespace UnityStandardAssets.ImageEffects
	' Token: 0x02000CE6 RID: 3302
	<ExecuteInEditMode()>
	<RequireComponent(GetType(Camera))>
	<AddComponentMenu("Image Effects/Rendering/Screen Space Ambient Obscurance")>
	Friend Class ScreenSpaceAmbientObscurance
		Inherits PostEffectsBase

		' Token: 0x06005254 RID: 21076 RVA: 0x002A3557 File Offset: 0x002A1957
		Public Overrides Function CheckResources() As Boolean
			MyBase.CheckSupport(True)
			Me.aoMaterial = MyBase.CheckShaderAndCreateMaterial(Me.aoShader, Me.aoMaterial)
			If Not Me.isSupported Then
				MyBase.ReportAutoDisable()
			End If
			Return Me.isSupported
		End Function

		' Token: 0x06005255 RID: 21077 RVA: 0x002A3590 File Offset: 0x002A1990
		Private Sub OnDisable()
			If Me.aoMaterial Then
				Global.UnityEngine.[Object].DestroyImmediate(Me.aoMaterial)
			End If
			Me.aoMaterial = Nothing
		End Sub

		' Token: 0x06005256 RID: 21078 RVA: 0x002A35B4 File Offset: 0x002A19B4
		<ImageEffectOpaque()>
		Private Sub OnRenderImage(source As RenderTexture, destination As RenderTexture)
			If Not Me.CheckResources() Then
				Graphics.Blit(source, destination)
				Return
			End If
			Dim projectionMatrix As Matrix4x4 = MyBase.GetComponent(Of Camera)().projectionMatrix
			Dim inverse As Matrix4x4 = projectionMatrix.inverse
			Dim vector As Vector4 = New Vector4(-2F / (CSng(Screen.width) * projectionMatrix(0)), -2F / (CSng(Screen.height) * projectionMatrix(5)), (1F - projectionMatrix(2)) / projectionMatrix(0), (1F + projectionMatrix(6)) / projectionMatrix(5))
			Me.aoMaterial.SetVector("_ProjInfo", vector)
			Me.aoMaterial.SetMatrix("_ProjectionInv", inverse)
			Me.aoMaterial.SetTexture("_Rand", Me.rand)
			Me.aoMaterial.SetFloat("_Radius", Me.radius)
			Me.aoMaterial.SetFloat("_Radius2", Me.radius * Me.radius)
			Me.aoMaterial.SetFloat("_Intensity", Me.intensity)
			Me.aoMaterial.SetFloat("_BlurFilterDistance", Me.blurFilterDistance)
			Dim width As Integer = source.width
			Dim height As Integer = source.height
			Dim renderTexture As RenderTexture = RenderTexture.GetTemporary(width >> Me.downsample, height >> Me.downsample)
			Graphics.Blit(source, renderTexture, Me.aoMaterial, 0)
			If Me.downsample > 0 Then
				Dim renderTexture2 As RenderTexture = RenderTexture.GetTemporary(width, height)
				Graphics.Blit(renderTexture, renderTexture2, Me.aoMaterial, 4)
				RenderTexture.ReleaseTemporary(renderTexture)
				renderTexture = renderTexture2
			End If
			For i As Integer = 0 To Me.blurIterations - 1
				Me.aoMaterial.SetVector("_Axis", New Vector2(1F, 0F))
				Dim renderTexture2 As RenderTexture = RenderTexture.GetTemporary(width, height)
				Graphics.Blit(renderTexture, renderTexture2, Me.aoMaterial, 1)
				RenderTexture.ReleaseTemporary(renderTexture)
				Me.aoMaterial.SetVector("_Axis", New Vector2(0F, 1F))
				renderTexture = RenderTexture.GetTemporary(width, height)
				Graphics.Blit(renderTexture2, renderTexture, Me.aoMaterial, 1)
				RenderTexture.ReleaseTemporary(renderTexture2)
			Next
			Me.aoMaterial.SetTexture("_AOTex", renderTexture)
			Graphics.Blit(source, destination, Me.aoMaterial, 2)
			RenderTexture.ReleaseTemporary(renderTexture)
		End Sub

		' Token: 0x040056BD RID: 22205
		<Range(0F, 3F)>
		Public intensity As Single = 0.5F

		' Token: 0x040056BE RID: 22206
		<Range(0.1F, 3F)>
		Public radius As Single = 0.2F

		' Token: 0x040056BF RID: 22207
		<Range(0F, 3F)>
		Public blurIterations As Integer = 1

		' Token: 0x040056C0 RID: 22208
		<Range(0F, 5F)>
		Public blurFilterDistance As Single = 1.25F

		' Token: 0x040056C1 RID: 22209
		<Range(0F, 1F)>
		Public downsample As Integer

		' Token: 0x040056C2 RID: 22210
		Public rand As Texture2D

		' Token: 0x040056C3 RID: 22211
		Public aoShader As Shader

		' Token: 0x040056C4 RID: 22212
		Private aoMaterial As Material
	End Class
End Namespace
