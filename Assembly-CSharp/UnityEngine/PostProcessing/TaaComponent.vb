Imports System

Namespace UnityEngine.PostProcessing
	' Token: 0x02000BB6 RID: 2998
	Public NotInheritable Class TaaComponent
		Inherits PostProcessingComponentRenderTexture(Of AntialiasingModel)

		' Token: 0x17000657 RID: 1623
		' (get) Token: 0x060048A6 RID: 18598 RVA: 0x002628B8 File Offset: 0x00260CB8
		Public Overrides ReadOnly Property active As Boolean
			Get
				Return MyBase.model.enabled AndAlso MyBase.model.settings.method = AntialiasingModel.Method.Taa AndAlso SystemInfo.supportsMotionVectors AndAlso SystemInfo.supportedRenderTargetCount >= 2 AndAlso Not Me.context.interrupted
			End Get
		End Property

		' Token: 0x060048A7 RID: 18599 RVA: 0x00262914 File Offset: 0x00260D14
		Public Overrides Function GetCameraFlags() As DepthTextureMode
			Return DepthTextureMode.Depth Or DepthTextureMode.MotionVectors
		End Function

		' Token: 0x17000658 RID: 1624
		' (get) Token: 0x060048A8 RID: 18600 RVA: 0x00262917 File Offset: 0x00260D17
		' (set) Token: 0x060048A9 RID: 18601 RVA: 0x0026291F File Offset: 0x00260D1F
		Public Property jitterVector As Vector2

		' Token: 0x060048AA RID: 18602 RVA: 0x00262928 File Offset: 0x00260D28
		Public Sub ResetHistory()
			Me.m_ResetHistory = True
		End Sub

		' Token: 0x060048AB RID: 18603 RVA: 0x00262934 File Offset: 0x00260D34
		Public Sub SetProjectionMatrix(jitteredFunc As Func(Of Vector2, Matrix4x4))
			Dim taaSettings As AntialiasingModel.TaaSettings = MyBase.model.settings.taaSettings
			Dim vector As Vector2 = Me.GenerateRandomOffset()
			vector *= taaSettings.jitterSpread
			Me.context.camera.nonJitteredProjectionMatrix = Me.context.camera.projectionMatrix
			If jitteredFunc IsNot Nothing Then
				Me.context.camera.projectionMatrix = jitteredFunc(vector)
			Else
				Me.context.camera.projectionMatrix = If((Not Me.context.camera.orthographic), Me.GetPerspectiveProjectionMatrix(vector), Me.GetOrthographicProjectionMatrix(vector))
			End If
			Me.context.camera.useJitteredProjectionMatrixForTransparentRendering = False
			vector.x /= CSng(Me.context.width)
			vector.y /= CSng(Me.context.height)
			Dim material As Material = Me.context.materialFactory.[Get]("Hidden/Post FX/Temporal Anti-aliasing")
			material.SetVector(TaaComponent.Uniforms._Jitter, vector)
			Me.jitterVector = vector
		End Sub

		' Token: 0x060048AC RID: 18604 RVA: 0x00262A58 File Offset: 0x00260E58
		Public Sub Render(source As RenderTexture, destination As RenderTexture)
			Dim material As Material = Me.context.materialFactory.[Get]("Hidden/Post FX/Temporal Anti-aliasing")
			material.shaderKeywords = Nothing
			Dim taaSettings As AntialiasingModel.TaaSettings = MyBase.model.settings.taaSettings
			If Me.m_ResetHistory OrElse Me.m_HistoryTexture Is Nothing OrElse Me.m_HistoryTexture.width <> source.width OrElse Me.m_HistoryTexture.height <> source.height Then
				If Me.m_HistoryTexture Then
					RenderTexture.ReleaseTemporary(Me.m_HistoryTexture)
				End If
				Me.m_HistoryTexture = RenderTexture.GetTemporary(source.width, source.height, 0, source.format)
				Me.m_HistoryTexture.name = "TAA History"
				Graphics.Blit(source, Me.m_HistoryTexture, material, 2)
			End If
			material.SetVector(TaaComponent.Uniforms._SharpenParameters, New Vector4(taaSettings.sharpen, 0F, 0F, 0F))
			material.SetVector(TaaComponent.Uniforms._FinalBlendParameters, New Vector4(taaSettings.stationaryBlending, taaSettings.motionBlending, 6000F, 0F))
			material.SetTexture(TaaComponent.Uniforms._MainTex, source)
			material.SetTexture(TaaComponent.Uniforms._HistoryTex, Me.m_HistoryTexture)
			Dim temporary As RenderTexture = RenderTexture.GetTemporary(source.width, source.height, 0, source.format)
			temporary.name = "TAA History"
			Me.m_MRT(0) = destination.colorBuffer
			Me.m_MRT(1) = temporary.colorBuffer
			Graphics.SetRenderTarget(Me.m_MRT, source.depthBuffer)
			GraphicsUtils.Blit(material, If((Not Me.context.camera.orthographic), 0, 1))
			RenderTexture.ReleaseTemporary(Me.m_HistoryTexture)
			Me.m_HistoryTexture = temporary
			Me.m_ResetHistory = False
		End Sub

		' Token: 0x060048AD RID: 18605 RVA: 0x00262C40 File Offset: 0x00261040
		Private Function GetHaltonValue(index As Integer, radix As Integer) As Single
			Dim num As Single = 0F
			Dim num2 As Single = 1F / CSng(radix)
			While index > 0
				num += CSng((index Mod radix)) * num2
				index /= radix
				num2 /= CSng(radix)
			End While
			Return num
		End Function

		' Token: 0x060048AE RID: 18606 RVA: 0x00262C7C File Offset: 0x0026107C
		Private Function GenerateRandomOffset() As Vector2
			Dim vector As Vector2 = New Vector2(Me.GetHaltonValue(Me.m_SampleIndex And 1023, 2), Me.GetHaltonValue(Me.m_SampleIndex And 1023, 3))
			Dim num As Integer = Me.m_SampleIndex + 1
			Dim num2 As Integer = num
			Me.m_SampleIndex = num
			If num2 >= 8 Then
				Me.m_SampleIndex = 0
			End If
			Return vector
		End Function

		' Token: 0x060048AF RID: 18607 RVA: 0x00262CD8 File Offset: 0x002610D8
		Private Function GetPerspectiveProjectionMatrix(offset As Vector2) As Matrix4x4
			Dim num As Single = Mathf.Tan(0.008726646F * Me.context.camera.fieldOfView)
			Dim num2 As Single = num * Me.context.camera.aspect
			offset.x *= num2 / (0.5F * CSng(Me.context.width))
			offset.y *= num / (0.5F * CSng(Me.context.height))
			Dim num3 As Single = (offset.x - num2) * Me.context.camera.nearClipPlane
			Dim num4 As Single = (offset.x + num2) * Me.context.camera.nearClipPlane
			Dim num5 As Single = (offset.y + num) * Me.context.camera.nearClipPlane
			Dim num6 As Single = (offset.y - num) * Me.context.camera.nearClipPlane
			Dim matrix4x As Matrix4x4 = Nothing
			matrix4x(0, 0) = 2F * Me.context.camera.nearClipPlane / (num4 - num3)
			matrix4x(0, 1) = 0F
			matrix4x(0, 2) = (num4 + num3) / (num4 - num3)
			matrix4x(0, 3) = 0F
			matrix4x(1, 0) = 0F
			matrix4x(1, 1) = 2F * Me.context.camera.nearClipPlane / (num5 - num6)
			matrix4x(1, 2) = (num5 + num6) / (num5 - num6)
			matrix4x(1, 3) = 0F
			matrix4x(2, 0) = 0F
			matrix4x(2, 1) = 0F
			matrix4x(2, 2) = -(Me.context.camera.farClipPlane + Me.context.camera.nearClipPlane) / (Me.context.camera.farClipPlane - Me.context.camera.nearClipPlane)
			matrix4x(2, 3) = -(2F * Me.context.camera.farClipPlane * Me.context.camera.nearClipPlane) / (Me.context.camera.farClipPlane - Me.context.camera.nearClipPlane)
			matrix4x(3, 0) = 0F
			matrix4x(3, 1) = 0F
			matrix4x(3, 2) = -1F
			matrix4x(3, 3) = 0F
			Return matrix4x
		End Function

		' Token: 0x060048B0 RID: 18608 RVA: 0x00262F68 File Offset: 0x00261368
		Private Function GetOrthographicProjectionMatrix(offset As Vector2) As Matrix4x4
			Dim orthographicSize As Single = Me.context.camera.orthographicSize
			Dim num As Single = orthographicSize * Me.context.camera.aspect
			offset.x *= num / (0.5F * CSng(Me.context.width))
			offset.y *= orthographicSize / (0.5F * CSng(Me.context.height))
			Dim num2 As Single = offset.x - num
			Dim num3 As Single = offset.x + num
			Dim num4 As Single = offset.y + orthographicSize
			Dim num5 As Single = offset.y - orthographicSize
			Return Matrix4x4.Ortho(num2, num3, num5, num4, Me.context.camera.nearClipPlane, Me.context.camera.farClipPlane)
		End Function

		' Token: 0x060048B1 RID: 18609 RVA: 0x00263032 File Offset: 0x00261432
		Public Overrides Sub OnDisable()
			If Me.m_HistoryTexture IsNot Nothing Then
				RenderTexture.ReleaseTemporary(Me.m_HistoryTexture)
			End If
			Me.m_HistoryTexture = Nothing
			Me.m_SampleIndex = 0
			Me.ResetHistory()
		End Sub

		' Token: 0x04004E5F RID: 20063
		Private Const k_ShaderString As String = "Hidden/Post FX/Temporal Anti-aliasing"

		' Token: 0x04004E60 RID: 20064
		Private Const k_SampleCount As Integer = 8

		' Token: 0x04004E61 RID: 20065
		Private m_MRT As RenderBuffer() = New RenderBuffer(1) {}

		' Token: 0x04004E62 RID: 20066
		Private m_SampleIndex As Integer

		' Token: 0x04004E63 RID: 20067
		Private m_ResetHistory As Boolean = True

		' Token: 0x04004E64 RID: 20068
		Private m_HistoryTexture As RenderTexture

		' Token: 0x02000BB7 RID: 2999
		Private NotInheritable Class Uniforms
			' Token: 0x04004E66 RID: 20070
			Friend Shared _Jitter As Integer = Shader.PropertyToID("_Jitter")

			' Token: 0x04004E67 RID: 20071
			Friend Shared _SharpenParameters As Integer = Shader.PropertyToID("_SharpenParameters")

			' Token: 0x04004E68 RID: 20072
			Friend Shared _FinalBlendParameters As Integer = Shader.PropertyToID("_FinalBlendParameters")

			' Token: 0x04004E69 RID: 20073
			Friend Shared _HistoryTex As Integer = Shader.PropertyToID("_HistoryTex")

			' Token: 0x04004E6A RID: 20074
			Friend Shared _MainTex As Integer = Shader.PropertyToID("_MainTex")
		End Class
	End Class
End Namespace
