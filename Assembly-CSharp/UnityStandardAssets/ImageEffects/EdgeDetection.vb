Imports System
Imports UnityEngine

Namespace UnityStandardAssets.ImageEffects
	' Token: 0x02000CD7 RID: 3287
	<ExecuteInEditMode()>
	<RequireComponent(GetType(Camera))>
	<AddComponentMenu("Image Effects/Edge Detection/Edge Detection")>
	Public Class EdgeDetection
		Inherits PostEffectsBase

		' Token: 0x06005213 RID: 21011 RVA: 0x002A170C File Offset: 0x0029FB0C
		Public Overrides Function CheckResources() As Boolean
			MyBase.CheckSupport(True)
			Me.edgeDetectMaterial = MyBase.CheckShaderAndCreateMaterial(Me.edgeDetectShader, Me.edgeDetectMaterial)
			If Me.mode <> Me.oldMode Then
				Me.SetCameraFlag()
			End If
			Me.oldMode = Me.mode
			If Not Me.isSupported Then
				MyBase.ReportAutoDisable()
			End If
			Return Me.isSupported
		End Function

		' Token: 0x06005214 RID: 21012 RVA: 0x002A1773 File Offset: 0x0029FB73
		Private Sub Start()
			Me.oldMode = Me.mode
		End Sub

		' Token: 0x06005215 RID: 21013 RVA: 0x002A1784 File Offset: 0x0029FB84
		Private Sub SetCameraFlag()
			If Me.mode = EdgeDetection.EdgeDetectMode.SobelDepth OrElse Me.mode = EdgeDetection.EdgeDetectMode.SobelDepthThin Then
				MyBase.GetComponent(Of Camera)().depthTextureMode = MyBase.GetComponent(Of Camera)().depthTextureMode Or DepthTextureMode.Depth
			ElseIf Me.mode = EdgeDetection.EdgeDetectMode.TriangleDepthNormals OrElse Me.mode = EdgeDetection.EdgeDetectMode.RobertsCrossDepthNormals Then
				MyBase.GetComponent(Of Camera)().depthTextureMode = MyBase.GetComponent(Of Camera)().depthTextureMode Or DepthTextureMode.DepthNormals
			End If
		End Sub

		' Token: 0x06005216 RID: 21014 RVA: 0x002A17EB File Offset: 0x0029FBEB
		Private Sub OnEnable()
			Me.SetCameraFlag()
		End Sub

		' Token: 0x06005217 RID: 21015 RVA: 0x002A17F4 File Offset: 0x0029FBF4
		<ImageEffectOpaque()>
		Private Sub OnRenderImage(source As RenderTexture, destination As RenderTexture)
			If Not Me.CheckResources() Then
				Graphics.Blit(source, destination)
				Return
			End If
			Dim vector As Vector2 = New Vector2(Me.sensitivityDepth, Me.sensitivityNormals)
			Me.edgeDetectMaterial.SetVector("_Sensitivity", New Vector4(vector.x, vector.y, 1F, vector.y))
			Me.edgeDetectMaterial.SetFloat("_BgFade", Me.edgesOnly)
			Me.edgeDetectMaterial.SetFloat("_SampleDistance", Me.sampleDist)
			Me.edgeDetectMaterial.SetVector("_BgColor", Me.edgesOnlyBgColor)
			Me.edgeDetectMaterial.SetFloat("_Exponent", Me.edgeExp)
			Me.edgeDetectMaterial.SetFloat("_Threshold", Me.lumThreshold)
			Graphics.Blit(source, destination, Me.edgeDetectMaterial, CInt(Me.mode))
		End Sub

		' Token: 0x04005665 RID: 22117
		Public mode As EdgeDetection.EdgeDetectMode = EdgeDetection.EdgeDetectMode.SobelDepthThin

		' Token: 0x04005666 RID: 22118
		Public sensitivityDepth As Single = 1F

		' Token: 0x04005667 RID: 22119
		Public sensitivityNormals As Single = 1F

		' Token: 0x04005668 RID: 22120
		Public lumThreshold As Single = 0.2F

		' Token: 0x04005669 RID: 22121
		Public edgeExp As Single = 1F

		' Token: 0x0400566A RID: 22122
		Public sampleDist As Single = 1F

		' Token: 0x0400566B RID: 22123
		Public edgesOnly As Single

		' Token: 0x0400566C RID: 22124
		Public edgesOnlyBgColor As Color = Color.white

		' Token: 0x0400566D RID: 22125
		Public edgeDetectShader As Shader

		' Token: 0x0400566E RID: 22126
		Private edgeDetectMaterial As Material

		' Token: 0x0400566F RID: 22127
		Private oldMode As EdgeDetection.EdgeDetectMode = EdgeDetection.EdgeDetectMode.SobelDepthThin

		' Token: 0x02000CD8 RID: 3288
		Public Enum EdgeDetectMode
			' Token: 0x04005671 RID: 22129
			TriangleDepthNormals
			' Token: 0x04005672 RID: 22130
			RobertsCrossDepthNormals
			' Token: 0x04005673 RID: 22131
			SobelDepth
			' Token: 0x04005674 RID: 22132
			SobelDepthThin
			' Token: 0x04005675 RID: 22133
			TriangleLuminance
		End Enum
	End Class
End Namespace
