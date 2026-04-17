Imports System
Imports UnityEngine.Rendering

Namespace UnityEngine.PostProcessing
	' Token: 0x02000BAD RID: 2989
	Public NotInheritable Class MotionBlurComponent
		Inherits PostProcessingComponentCommandBuffer(Of MotionBlurModel)

		' Token: 0x17000653 RID: 1619
		' (get) Token: 0x06004883 RID: 18563 RVA: 0x00261213 File Offset: 0x0025F613
		Public ReadOnly Property reconstructionFilter As MotionBlurComponent.ReconstructionFilter
			Get
				If Me.m_ReconstructionFilter Is Nothing Then
					Me.m_ReconstructionFilter = New MotionBlurComponent.ReconstructionFilter()
				End If
				Return Me.m_ReconstructionFilter
			End Get
		End Property

		' Token: 0x17000654 RID: 1620
		' (get) Token: 0x06004884 RID: 18564 RVA: 0x00261231 File Offset: 0x0025F631
		Public ReadOnly Property frameBlendingFilter As MotionBlurComponent.FrameBlendingFilter
			Get
				If Me.m_FrameBlendingFilter Is Nothing Then
					Me.m_FrameBlendingFilter = New MotionBlurComponent.FrameBlendingFilter()
				End If
				Return Me.m_FrameBlendingFilter
			End Get
		End Property

		' Token: 0x17000655 RID: 1621
		' (get) Token: 0x06004885 RID: 18565 RVA: 0x00261250 File Offset: 0x0025F650
		Public Overrides ReadOnly Property active As Boolean
			Get
				Dim settings As MotionBlurModel.Settings = MyBase.model.settings
				Return MyBase.model.enabled AndAlso ((settings.shutterAngle > 0F AndAlso Me.reconstructionFilter.IsSupported()) OrElse settings.frameBlending > 0F) AndAlso SystemInfo.graphicsDeviceType <> GraphicsDeviceType.OpenGLES2 AndAlso Not Me.context.interrupted
			End Get
		End Property

		' Token: 0x06004886 RID: 18566 RVA: 0x002612C7 File Offset: 0x0025F6C7
		Public Overrides Function GetName() As String
			Return "Motion Blur"
		End Function

		' Token: 0x06004887 RID: 18567 RVA: 0x002612CE File Offset: 0x0025F6CE
		Public Sub ResetHistory()
			If Me.m_FrameBlendingFilter IsNot Nothing Then
				Me.m_FrameBlendingFilter.Dispose()
			End If
			Me.m_FrameBlendingFilter = Nothing
		End Sub

		' Token: 0x06004888 RID: 18568 RVA: 0x002612ED File Offset: 0x0025F6ED
		Public Overrides Function GetCameraFlags() As DepthTextureMode
			Return DepthTextureMode.Depth Or DepthTextureMode.MotionVectors
		End Function

		' Token: 0x06004889 RID: 18569 RVA: 0x002612F0 File Offset: 0x0025F6F0
		Public Overrides Function GetCameraEvent() As CameraEvent
			Return CameraEvent.BeforeImageEffects
		End Function

		' Token: 0x0600488A RID: 18570 RVA: 0x002612F4 File Offset: 0x0025F6F4
		Public Overrides Sub OnEnable()
			Me.m_FirstFrame = True
		End Sub

		' Token: 0x0600488B RID: 18571 RVA: 0x00261300 File Offset: 0x0025F700
		Public Overrides Sub PopulateCommandBuffer(cb As CommandBuffer)
			If Me.m_FirstFrame Then
				Me.m_FirstFrame = False
				Return
			End If
			Dim material As Material = Me.context.materialFactory.[Get]("Hidden/Post FX/Motion Blur")
			Dim material2 As Material = Me.context.materialFactory.[Get]("Hidden/Post FX/Blit")
			Dim settings As MotionBlurModel.Settings = MyBase.model.settings
			Dim renderTextureFormat As RenderTextureFormat = If((Not Me.context.isHdr), RenderTextureFormat.[Default], RenderTextureFormat.DefaultHDR)
			Dim tempRT As Integer = MotionBlurComponent.Uniforms._TempRT
			cb.GetTemporaryRT(tempRT, Me.context.width, Me.context.height, 0, FilterMode.Point, renderTextureFormat)
			If settings.shutterAngle > 0F AndAlso settings.frameBlending > 0F Then
				Me.reconstructionFilter.ProcessImage(Me.context, cb, settings, BuiltinRenderTextureType.CameraTarget, tempRT, material)
				Me.frameBlendingFilter.BlendFrames(cb, settings.frameBlending, tempRT, BuiltinRenderTextureType.CameraTarget, material)
				Me.frameBlendingFilter.PushFrame(cb, tempRT, Me.context.width, Me.context.height, material)
			ElseIf settings.shutterAngle > 0F Then
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, BuiltinRenderTextureType.CameraTarget)
				cb.Blit(BuiltinRenderTextureType.CameraTarget, tempRT, material2, 0)
				Me.reconstructionFilter.ProcessImage(Me.context, cb, settings, tempRT, BuiltinRenderTextureType.CameraTarget, material)
			ElseIf settings.frameBlending > 0F Then
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, BuiltinRenderTextureType.CameraTarget)
				cb.Blit(BuiltinRenderTextureType.CameraTarget, tempRT, material2, 0)
				Me.frameBlendingFilter.BlendFrames(cb, settings.frameBlending, tempRT, BuiltinRenderTextureType.CameraTarget, material)
				Me.frameBlendingFilter.PushFrame(cb, tempRT, Me.context.width, Me.context.height, material)
			End If
			cb.ReleaseTemporaryRT(tempRT)
		End Sub

		' Token: 0x0600488C RID: 18572 RVA: 0x00261515 File Offset: 0x0025F915
		Public Overrides Sub OnDisable()
			If Me.m_FrameBlendingFilter IsNot Nothing Then
				Me.m_FrameBlendingFilter.Dispose()
			End If
		End Sub

		' Token: 0x04004DFC RID: 19964
		Private m_ReconstructionFilter As MotionBlurComponent.ReconstructionFilter

		' Token: 0x04004DFD RID: 19965
		Private m_FrameBlendingFilter As MotionBlurComponent.FrameBlendingFilter

		' Token: 0x04004DFE RID: 19966
		Private m_FirstFrame As Boolean = True

		' Token: 0x02000BAE RID: 2990
		Private NotInheritable Class Uniforms
			' Token: 0x04004DFF RID: 19967
			Friend Shared _VelocityScale As Integer = Shader.PropertyToID("_VelocityScale")

			' Token: 0x04004E00 RID: 19968
			Friend Shared _MaxBlurRadius As Integer = Shader.PropertyToID("_MaxBlurRadius")

			' Token: 0x04004E01 RID: 19969
			Friend Shared _RcpMaxBlurRadius As Integer = Shader.PropertyToID("_RcpMaxBlurRadius")

			' Token: 0x04004E02 RID: 19970
			Friend Shared _VelocityTex As Integer = Shader.PropertyToID("_VelocityTex")

			' Token: 0x04004E03 RID: 19971
			Friend Shared _MainTex As Integer = Shader.PropertyToID("_MainTex")

			' Token: 0x04004E04 RID: 19972
			Friend Shared _Tile2RT As Integer = Shader.PropertyToID("_Tile2RT")

			' Token: 0x04004E05 RID: 19973
			Friend Shared _Tile4RT As Integer = Shader.PropertyToID("_Tile4RT")

			' Token: 0x04004E06 RID: 19974
			Friend Shared _Tile8RT As Integer = Shader.PropertyToID("_Tile8RT")

			' Token: 0x04004E07 RID: 19975
			Friend Shared _TileMaxOffs As Integer = Shader.PropertyToID("_TileMaxOffs")

			' Token: 0x04004E08 RID: 19976
			Friend Shared _TileMaxLoop As Integer = Shader.PropertyToID("_TileMaxLoop")

			' Token: 0x04004E09 RID: 19977
			Friend Shared _TileVRT As Integer = Shader.PropertyToID("_TileVRT")

			' Token: 0x04004E0A RID: 19978
			Friend Shared _NeighborMaxTex As Integer = Shader.PropertyToID("_NeighborMaxTex")

			' Token: 0x04004E0B RID: 19979
			Friend Shared _LoopCount As Integer = Shader.PropertyToID("_LoopCount")

			' Token: 0x04004E0C RID: 19980
			Friend Shared _TempRT As Integer = Shader.PropertyToID("_TempRT")

			' Token: 0x04004E0D RID: 19981
			Friend Shared _History1LumaTex As Integer = Shader.PropertyToID("_History1LumaTex")

			' Token: 0x04004E0E RID: 19982
			Friend Shared _History2LumaTex As Integer = Shader.PropertyToID("_History2LumaTex")

			' Token: 0x04004E0F RID: 19983
			Friend Shared _History3LumaTex As Integer = Shader.PropertyToID("_History3LumaTex")

			' Token: 0x04004E10 RID: 19984
			Friend Shared _History4LumaTex As Integer = Shader.PropertyToID("_History4LumaTex")

			' Token: 0x04004E11 RID: 19985
			Friend Shared _History1ChromaTex As Integer = Shader.PropertyToID("_History1ChromaTex")

			' Token: 0x04004E12 RID: 19986
			Friend Shared _History2ChromaTex As Integer = Shader.PropertyToID("_History2ChromaTex")

			' Token: 0x04004E13 RID: 19987
			Friend Shared _History3ChromaTex As Integer = Shader.PropertyToID("_History3ChromaTex")

			' Token: 0x04004E14 RID: 19988
			Friend Shared _History4ChromaTex As Integer = Shader.PropertyToID("_History4ChromaTex")

			' Token: 0x04004E15 RID: 19989
			Friend Shared _History1Weight As Integer = Shader.PropertyToID("_History1Weight")

			' Token: 0x04004E16 RID: 19990
			Friend Shared _History2Weight As Integer = Shader.PropertyToID("_History2Weight")

			' Token: 0x04004E17 RID: 19991
			Friend Shared _History3Weight As Integer = Shader.PropertyToID("_History3Weight")

			' Token: 0x04004E18 RID: 19992
			Friend Shared _History4Weight As Integer = Shader.PropertyToID("_History4Weight")
		End Class

		' Token: 0x02000BAF RID: 2991
		Private Enum Pass
			' Token: 0x04004E1A RID: 19994
			VelocitySetup
			' Token: 0x04004E1B RID: 19995
			TileMax1
			' Token: 0x04004E1C RID: 19996
			TileMax2
			' Token: 0x04004E1D RID: 19997
			TileMaxV
			' Token: 0x04004E1E RID: 19998
			NeighborMax
			' Token: 0x04004E1F RID: 19999
			Reconstruction
			' Token: 0x04004E20 RID: 20000
			FrameCompression
			' Token: 0x04004E21 RID: 20001
			FrameBlendingChroma
			' Token: 0x04004E22 RID: 20002
			FrameBlendingRaw
		End Enum

		' Token: 0x02000BB0 RID: 2992
		Public Class ReconstructionFilter
			' Token: 0x0600488E RID: 18574 RVA: 0x002616C3 File Offset: 0x0025FAC3
			Public Sub New()
				Me.CheckTextureFormatSupport()
			End Sub

			' Token: 0x0600488F RID: 18575 RVA: 0x002616E0 File Offset: 0x0025FAE0
			Private Sub CheckTextureFormatSupport()
				If Not SystemInfo.SupportsRenderTextureFormat(Me.m_PackedRTFormat) Then
					Me.m_PackedRTFormat = RenderTextureFormat.ARGB32
				End If
			End Sub

			' Token: 0x06004890 RID: 18576 RVA: 0x002616F9 File Offset: 0x0025FAF9
			Public Function IsSupported() As Boolean
				Return SystemInfo.supportsMotionVectors
			End Function

			' Token: 0x06004891 RID: 18577 RVA: 0x00261700 File Offset: 0x0025FB00
			Public Sub ProcessImage(context As PostProcessingContext, cb As CommandBuffer, ByRef settings As MotionBlurModel.Settings, source As RenderTargetIdentifier, destination As RenderTargetIdentifier, material As Material)
				Dim num As Integer = CInt((5F * CSng(context.height) / 100F))
				Dim num2 As Integer = ((num - 1) / 8 + 1) * 8
				Dim num3 As Single = settings.shutterAngle / 360F
				cb.SetGlobalFloat(MotionBlurComponent.Uniforms._VelocityScale, num3)
				cb.SetGlobalFloat(MotionBlurComponent.Uniforms._MaxBlurRadius, CSng(num))
				cb.SetGlobalFloat(MotionBlurComponent.Uniforms._RcpMaxBlurRadius, 1F / CSng(num))
				Dim velocityTex As Integer = MotionBlurComponent.Uniforms._VelocityTex
				cb.GetTemporaryRT(velocityTex, context.width, context.height, 0, FilterMode.Point, Me.m_PackedRTFormat, RenderTextureReadWrite.Linear)
				cb.Blit(Nothing, velocityTex, material, 0)
				Dim tile2RT As Integer = MotionBlurComponent.Uniforms._Tile2RT
				cb.GetTemporaryRT(tile2RT, context.width / 2, context.height / 2, 0, FilterMode.Point, Me.m_VectorRTFormat, RenderTextureReadWrite.Linear)
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, velocityTex)
				cb.Blit(velocityTex, tile2RT, material, 1)
				Dim tile4RT As Integer = MotionBlurComponent.Uniforms._Tile4RT
				cb.GetTemporaryRT(tile4RT, context.width / 4, context.height / 4, 0, FilterMode.Point, Me.m_VectorRTFormat, RenderTextureReadWrite.Linear)
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, tile2RT)
				cb.Blit(tile2RT, tile4RT, material, 2)
				cb.ReleaseTemporaryRT(tile2RT)
				Dim tile8RT As Integer = MotionBlurComponent.Uniforms._Tile8RT
				cb.GetTemporaryRT(tile8RT, context.width / 8, context.height / 8, 0, FilterMode.Point, Me.m_VectorRTFormat, RenderTextureReadWrite.Linear)
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, tile4RT)
				cb.Blit(tile4RT, tile8RT, material, 2)
				cb.ReleaseTemporaryRT(tile4RT)
				Dim vector As Vector2 = Vector2.one * (CSng(num2) / 8F - 1F) * -0.5F
				cb.SetGlobalVector(MotionBlurComponent.Uniforms._TileMaxOffs, vector)
				cb.SetGlobalFloat(MotionBlurComponent.Uniforms._TileMaxLoop, CSng(CInt((CSng(num2) / 8F))))
				Dim tileVRT As Integer = MotionBlurComponent.Uniforms._TileVRT
				cb.GetTemporaryRT(tileVRT, context.width / num2, context.height / num2, 0, FilterMode.Point, Me.m_VectorRTFormat, RenderTextureReadWrite.Linear)
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, tile8RT)
				cb.Blit(tile8RT, tileVRT, material, 3)
				cb.ReleaseTemporaryRT(tile8RT)
				Dim neighborMaxTex As Integer = MotionBlurComponent.Uniforms._NeighborMaxTex
				Dim num4 As Integer = context.width / num2
				Dim num5 As Integer = context.height / num2
				cb.GetTemporaryRT(neighborMaxTex, num4, num5, 0, FilterMode.Point, Me.m_VectorRTFormat, RenderTextureReadWrite.Linear)
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, tileVRT)
				cb.Blit(tileVRT, neighborMaxTex, material, 4)
				cb.ReleaseTemporaryRT(tileVRT)
				cb.SetGlobalFloat(MotionBlurComponent.Uniforms._LoopCount, CSng(Mathf.Clamp(settings.sampleCount / 2, 1, 64)))
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, source)
				cb.Blit(source, destination, material, 5)
				cb.ReleaseTemporaryRT(velocityTex)
				cb.ReleaseTemporaryRT(neighborMaxTex)
			End Sub

			' Token: 0x04004E23 RID: 20003
			Private m_VectorRTFormat As RenderTextureFormat = RenderTextureFormat.RGHalf

			' Token: 0x04004E24 RID: 20004
			Private m_PackedRTFormat As RenderTextureFormat = RenderTextureFormat.ARGB2101010
		End Class

		' Token: 0x02000BB1 RID: 2993
		Public Class FrameBlendingFilter
			' Token: 0x06004892 RID: 18578 RVA: 0x002619E2 File Offset: 0x0025FDE2
			Public Sub New()
				Me.m_UseCompression = MotionBlurComponent.FrameBlendingFilter.CheckSupportCompression()
				Me.m_RawTextureFormat = MotionBlurComponent.FrameBlendingFilter.GetPreferredRenderTextureFormat()
				Me.m_FrameList = New MotionBlurComponent.FrameBlendingFilter.Frame(3) {}
			End Sub

			' Token: 0x06004893 RID: 18579 RVA: 0x00261A0C File Offset: 0x0025FE0C
			Public Sub Dispose()
				For Each frame As MotionBlurComponent.FrameBlendingFilter.Frame In Me.m_FrameList
					frame.Release()
				Next
			End Sub

			' Token: 0x06004894 RID: 18580 RVA: 0x00261A48 File Offset: 0x0025FE48
			Public Sub PushFrame(cb As CommandBuffer, source As RenderTargetIdentifier, width As Integer, height As Integer, material As Material)
				Dim frameCount As Integer = Time.frameCount
				If frameCount = Me.m_LastFrameCount Then
					Return
				End If
				Dim num As Integer = frameCount Mod Me.m_FrameList.Length
				If Me.m_UseCompression Then
					Me.m_FrameList(num).MakeRecord(cb, source, width, height, material)
				Else
					Me.m_FrameList(num).MakeRecordRaw(cb, source, width, height, Me.m_RawTextureFormat)
				End If
				Me.m_LastFrameCount = frameCount
			End Sub

			' Token: 0x06004895 RID: 18581 RVA: 0x00261AC0 File Offset: 0x0025FEC0
			Public Sub BlendFrames(cb As CommandBuffer, strength As Single, source As RenderTargetIdentifier, destination As RenderTargetIdentifier, material As Material)
				Dim time As Single = Time.time
				Dim frameRelative As MotionBlurComponent.FrameBlendingFilter.Frame = Me.GetFrameRelative(-1)
				Dim frameRelative2 As MotionBlurComponent.FrameBlendingFilter.Frame = Me.GetFrameRelative(-2)
				Dim frameRelative3 As MotionBlurComponent.FrameBlendingFilter.Frame = Me.GetFrameRelative(-3)
				Dim frameRelative4 As MotionBlurComponent.FrameBlendingFilter.Frame = Me.GetFrameRelative(-4)
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._History1LumaTex, frameRelative.lumaTexture)
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._History2LumaTex, frameRelative2.lumaTexture)
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._History3LumaTex, frameRelative3.lumaTexture)
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._History4LumaTex, frameRelative4.lumaTexture)
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._History1ChromaTex, frameRelative.chromaTexture)
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._History2ChromaTex, frameRelative2.chromaTexture)
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._History3ChromaTex, frameRelative3.chromaTexture)
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._History4ChromaTex, frameRelative4.chromaTexture)
				cb.SetGlobalFloat(MotionBlurComponent.Uniforms._History1Weight, frameRelative.CalculateWeight(strength, time))
				cb.SetGlobalFloat(MotionBlurComponent.Uniforms._History2Weight, frameRelative2.CalculateWeight(strength, time))
				cb.SetGlobalFloat(MotionBlurComponent.Uniforms._History3Weight, frameRelative3.CalculateWeight(strength, time))
				cb.SetGlobalFloat(MotionBlurComponent.Uniforms._History4Weight, frameRelative4.CalculateWeight(strength, time))
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, source)
				cb.Blit(source, destination, material, If((Not Me.m_UseCompression), 8, 7))
			End Sub

			' Token: 0x06004896 RID: 18582 RVA: 0x00261C28 File Offset: 0x00260028
			Private Shared Function CheckSupportCompression() As Boolean
				Return SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.R8) AndAlso SystemInfo.supportedRenderTargetCount > 1
			End Function

			' Token: 0x06004897 RID: 18583 RVA: 0x00261C44 File Offset: 0x00260044
			Private Shared Function GetPreferredRenderTextureFormat() As RenderTextureFormat
				Dim array As RenderTextureFormat() = New RenderTextureFormat() { RenderTextureFormat.RGB565, RenderTextureFormat.ARGB1555, RenderTextureFormat.ARGB4444 }
				For Each renderTextureFormat As RenderTextureFormat In array
					If SystemInfo.SupportsRenderTextureFormat(renderTextureFormat) Then
						Return renderTextureFormat
					End If
				Next
				Return RenderTextureFormat.[Default]
			End Function

			' Token: 0x06004898 RID: 18584 RVA: 0x00261C8C File Offset: 0x0026008C
			Private Function GetFrameRelative(offset As Integer) As MotionBlurComponent.FrameBlendingFilter.Frame
				Dim num As Integer = (Time.frameCount + Me.m_FrameList.Length + offset) Mod Me.m_FrameList.Length
				Return Me.m_FrameList(num)
			End Function

			' Token: 0x04004E25 RID: 20005
			Private m_UseCompression As Boolean

			' Token: 0x04004E26 RID: 20006
			Private m_RawTextureFormat As RenderTextureFormat

			' Token: 0x04004E27 RID: 20007
			Private m_FrameList As MotionBlurComponent.FrameBlendingFilter.Frame()

			' Token: 0x04004E28 RID: 20008
			Private m_LastFrameCount As Integer

			' Token: 0x02000BB2 RID: 2994
			Private Structure Frame
				' Token: 0x06004899 RID: 18585 RVA: 0x00261CC4 File Offset: 0x002600C4
				Public Function CalculateWeight(strength As Single, currentTime As Single) As Single
					If Mathf.Approximately(Me.m_Time, 0F) Then
						Return 0F
					End If
					Dim num As Single = Mathf.Lerp(80F, 16F, strength)
					Return Mathf.Exp((Me.m_Time - currentTime) * num)
				End Function

				' Token: 0x0600489A RID: 18586 RVA: 0x00261D0C File Offset: 0x0026010C
				Public Sub Release()
					If Me.lumaTexture IsNot Nothing Then
						RenderTexture.ReleaseTemporary(Me.lumaTexture)
					End If
					If Me.chromaTexture IsNot Nothing Then
						RenderTexture.ReleaseTemporary(Me.chromaTexture)
					End If
					Me.lumaTexture = Nothing
					Me.chromaTexture = Nothing
				End Sub

				' Token: 0x0600489B RID: 18587 RVA: 0x00261D60 File Offset: 0x00260160
				Public Sub MakeRecord(cb As CommandBuffer, source As RenderTargetIdentifier, width As Integer, height As Integer, material As Material)
					Me.Release()
					Me.lumaTexture = RenderTexture.GetTemporary(width, height, 0, RenderTextureFormat.R8, RenderTextureReadWrite.Linear)
					Me.chromaTexture = RenderTexture.GetTemporary(width, height, 0, RenderTextureFormat.R8, RenderTextureReadWrite.Linear)
					Me.lumaTexture.filterMode = FilterMode.Point
					Me.chromaTexture.filterMode = FilterMode.Point
					If Me.m_MRT Is Nothing Then
						Me.m_MRT = New RenderTargetIdentifier(1) {}
					End If
					Me.m_MRT(0) = Me.lumaTexture
					Me.m_MRT(1) = Me.chromaTexture
					cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, source)
					cb.SetRenderTarget(Me.m_MRT, Me.lumaTexture)
					cb.DrawMesh(GraphicsUtils.quad, Matrix4x4.identity, material, 0, 6)
					Me.m_Time = Time.time
				End Sub

				' Token: 0x0600489C RID: 18588 RVA: 0x00261E40 File Offset: 0x00260240
				Public Sub MakeRecordRaw(cb As CommandBuffer, source As RenderTargetIdentifier, width As Integer, height As Integer, format As RenderTextureFormat)
					Me.Release()
					Me.lumaTexture = RenderTexture.GetTemporary(width, height, 0, format)
					Me.lumaTexture.filterMode = FilterMode.Point
					cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, source)
					cb.Blit(source, Me.lumaTexture)
					Me.m_Time = Time.time
				End Sub

				' Token: 0x04004E29 RID: 20009
				Public lumaTexture As RenderTexture

				' Token: 0x04004E2A RID: 20010
				Public chromaTexture As RenderTexture

				' Token: 0x04004E2B RID: 20011
				Private m_Time As Single

				' Token: 0x04004E2C RID: 20012
				Private m_MRT As RenderTargetIdentifier()
			End Structure
		End Class
	End Class
End Namespace
