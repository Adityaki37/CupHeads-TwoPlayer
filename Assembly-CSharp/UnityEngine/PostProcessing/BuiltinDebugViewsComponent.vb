Imports System
Imports System.Collections.Generic
Imports UnityEngine.Rendering

Namespace UnityEngine.PostProcessing
	' Token: 0x02000B99 RID: 2969
	Public NotInheritable Class BuiltinDebugViewsComponent
		Inherits PostProcessingComponentCommandBuffer(Of BuiltinDebugViewsModel)

		' Token: 0x17000647 RID: 1607
		' (get) Token: 0x06004828 RID: 18472 RVA: 0x0025E6CF File Offset: 0x0025CACF
		Public Overrides ReadOnly Property active As Boolean
			Get
				Return MyBase.model.IsModeActive(BuiltinDebugViewsModel.Mode.Depth) OrElse MyBase.model.IsModeActive(BuiltinDebugViewsModel.Mode.Normals) OrElse MyBase.model.IsModeActive(BuiltinDebugViewsModel.Mode.MotionVectors)
			End Get
		End Property

		' Token: 0x06004829 RID: 18473 RVA: 0x0025E704 File Offset: 0x0025CB04
		Public Overrides Function GetCameraFlags() As DepthTextureMode
			Dim mode As BuiltinDebugViewsModel.Mode = MyBase.model.settings.mode
			Dim depthTextureMode As DepthTextureMode = DepthTextureMode.None
			If mode <> BuiltinDebugViewsModel.Mode.Normals Then
				If mode <> BuiltinDebugViewsModel.Mode.MotionVectors Then
					If mode = BuiltinDebugViewsModel.Mode.Depth Then
						depthTextureMode = depthTextureMode Or DepthTextureMode.Depth
					End If
				Else
					depthTextureMode = depthTextureMode Or DepthTextureMode.Depth Or DepthTextureMode.MotionVectors
				End If
			Else
				depthTextureMode = depthTextureMode Or DepthTextureMode.DepthNormals
			End If
			Return depthTextureMode
		End Function

		' Token: 0x0600482A RID: 18474 RVA: 0x0025E760 File Offset: 0x0025CB60
		Public Overrides Function GetCameraEvent() As CameraEvent
			Return If((MyBase.model.settings.mode <> BuiltinDebugViewsModel.Mode.MotionVectors), CameraEvent.BeforeImageEffectsOpaque, CameraEvent.BeforeImageEffects)
		End Function

		' Token: 0x0600482B RID: 18475 RVA: 0x0025E78F File Offset: 0x0025CB8F
		Public Overrides Function GetName() As String
			Return "Builtin Debug Views"
		End Function

		' Token: 0x0600482C RID: 18476 RVA: 0x0025E798 File Offset: 0x0025CB98
		Public Overrides Sub PopulateCommandBuffer(cb As CommandBuffer)
			Dim settings As BuiltinDebugViewsModel.Settings = MyBase.model.settings
			Dim material As Material = Me.context.materialFactory.[Get]("Hidden/Post FX/Builtin Debug Views")
			material.shaderKeywords = Nothing
			If Me.context.isGBufferAvailable Then
				material.EnableKeyword("SOURCE_GBUFFER")
			End If
			Dim mode As BuiltinDebugViewsModel.Mode = settings.mode
			If mode <> BuiltinDebugViewsModel.Mode.Depth Then
				If mode <> BuiltinDebugViewsModel.Mode.Normals Then
					If mode = BuiltinDebugViewsModel.Mode.MotionVectors Then
						Me.MotionVectorsPass(cb)
					End If
				Else
					Me.DepthNormalsPass(cb)
				End If
			Else
				Me.DepthPass(cb)
			End If
			Me.context.Interrupt()
		End Sub

		' Token: 0x0600482D RID: 18477 RVA: 0x0025E83C File Offset: 0x0025CC3C
		Private Sub DepthPass(cb As CommandBuffer)
			Dim material As Material = Me.context.materialFactory.[Get]("Hidden/Post FX/Builtin Debug Views")
			Dim depth As BuiltinDebugViewsModel.DepthSettings = MyBase.model.settings.depth
			cb.SetGlobalFloat(BuiltinDebugViewsComponent.Uniforms._DepthScale, 1F / depth.scale)
			cb.Blit(Nothing, BuiltinRenderTextureType.CameraTarget, material, 0)
		End Sub

		' Token: 0x0600482E RID: 18478 RVA: 0x0025E89C File Offset: 0x0025CC9C
		Private Sub DepthNormalsPass(cb As CommandBuffer)
			Dim material As Material = Me.context.materialFactory.[Get]("Hidden/Post FX/Builtin Debug Views")
			cb.Blit(Nothing, BuiltinRenderTextureType.CameraTarget, material, 1)
		End Sub

		' Token: 0x0600482F RID: 18479 RVA: 0x0025E8D0 File Offset: 0x0025CCD0
		Private Sub MotionVectorsPass(cb As CommandBuffer)
			Dim material As Material = Me.context.materialFactory.[Get]("Hidden/Post FX/Builtin Debug Views")
			Dim motionVectors As BuiltinDebugViewsModel.MotionVectorsSettings = MyBase.model.settings.motionVectors
			Dim num As Integer = BuiltinDebugViewsComponent.Uniforms._TempRT
			cb.GetTemporaryRT(num, Me.context.width, Me.context.height, 0, FilterMode.Bilinear)
			cb.SetGlobalFloat(BuiltinDebugViewsComponent.Uniforms._Opacity, motionVectors.sourceOpacity)
			cb.SetGlobalTexture(BuiltinDebugViewsComponent.Uniforms._MainTex, BuiltinRenderTextureType.CameraTarget)
			cb.Blit(BuiltinRenderTextureType.CameraTarget, num, material, 2)
			If motionVectors.motionImageOpacity > 0F AndAlso motionVectors.motionImageAmplitude > 0F Then
				Dim tempRT As Integer = BuiltinDebugViewsComponent.Uniforms._TempRT2
				cb.GetTemporaryRT(tempRT, Me.context.width, Me.context.height, 0, FilterMode.Bilinear)
				cb.SetGlobalFloat(BuiltinDebugViewsComponent.Uniforms._Opacity, motionVectors.motionImageOpacity)
				cb.SetGlobalFloat(BuiltinDebugViewsComponent.Uniforms._Amplitude, motionVectors.motionImageAmplitude)
				cb.SetGlobalTexture(BuiltinDebugViewsComponent.Uniforms._MainTex, num)
				cb.Blit(num, tempRT, material, 3)
				cb.ReleaseTemporaryRT(num)
				num = tempRT
			End If
			If motionVectors.motionVectorsOpacity > 0F AndAlso motionVectors.motionVectorsAmplitude > 0F Then
				Me.PrepareArrows()
				Dim num2 As Single = 1F / CSng(motionVectors.motionVectorsResolution)
				Dim num3 As Single = num2 * CSng(Me.context.height) / CSng(Me.context.width)
				cb.SetGlobalVector(BuiltinDebugViewsComponent.Uniforms._Scale, New Vector2(num3, num2))
				cb.SetGlobalFloat(BuiltinDebugViewsComponent.Uniforms._Opacity, motionVectors.motionVectorsOpacity)
				cb.SetGlobalFloat(BuiltinDebugViewsComponent.Uniforms._Amplitude, motionVectors.motionVectorsAmplitude)
				cb.DrawMesh(Me.m_Arrows.mesh, Matrix4x4.identity, material, 0, 4)
			End If
			cb.SetGlobalTexture(BuiltinDebugViewsComponent.Uniforms._MainTex, num)
			cb.Blit(num, BuiltinRenderTextureType.CameraTarget)
			cb.ReleaseTemporaryRT(num)
		End Sub

		' Token: 0x06004830 RID: 18480 RVA: 0x0025EAD8 File Offset: 0x0025CED8
		Private Sub PrepareArrows()
			Dim motionVectorsResolution As Integer = MyBase.model.settings.motionVectors.motionVectorsResolution
			Dim num As Integer = motionVectorsResolution * Screen.width / Screen.height
			If Me.m_Arrows Is Nothing Then
				Me.m_Arrows = New BuiltinDebugViewsComponent.ArrowArray()
			End If
			If Me.m_Arrows.columnCount <> num OrElse Me.m_Arrows.rowCount <> motionVectorsResolution Then
				Me.m_Arrows.Release()
				Me.m_Arrows.BuildMesh(num, motionVectorsResolution)
			End If
		End Sub

		' Token: 0x06004831 RID: 18481 RVA: 0x0025EB5C File Offset: 0x0025CF5C
		Public Overrides Sub OnDisable()
			If Me.m_Arrows IsNot Nothing Then
				Me.m_Arrows.Release()
			End If
			Me.m_Arrows = Nothing
		End Sub

		' Token: 0x04004D9D RID: 19869
		Private Const k_ShaderString As String = "Hidden/Post FX/Builtin Debug Views"

		' Token: 0x04004D9E RID: 19870
		Private m_Arrows As BuiltinDebugViewsComponent.ArrowArray

		' Token: 0x02000B9A RID: 2970
		Private NotInheritable Class Uniforms
			' Token: 0x04004D9F RID: 19871
			Friend Shared _DepthScale As Integer = Shader.PropertyToID("_DepthScale")

			' Token: 0x04004DA0 RID: 19872
			Friend Shared _TempRT As Integer = Shader.PropertyToID("_TempRT")

			' Token: 0x04004DA1 RID: 19873
			Friend Shared _Opacity As Integer = Shader.PropertyToID("_Opacity")

			' Token: 0x04004DA2 RID: 19874
			Friend Shared _MainTex As Integer = Shader.PropertyToID("_MainTex")

			' Token: 0x04004DA3 RID: 19875
			Friend Shared _TempRT2 As Integer = Shader.PropertyToID("_TempRT2")

			' Token: 0x04004DA4 RID: 19876
			Friend Shared _Amplitude As Integer = Shader.PropertyToID("_Amplitude")

			' Token: 0x04004DA5 RID: 19877
			Friend Shared _Scale As Integer = Shader.PropertyToID("_Scale")
		End Class

		' Token: 0x02000B9B RID: 2971
		Private Enum Pass
			' Token: 0x04004DA7 RID: 19879
			Depth
			' Token: 0x04004DA8 RID: 19880
			Normals
			' Token: 0x04004DA9 RID: 19881
			MovecOpacity
			' Token: 0x04004DAA RID: 19882
			MovecImaging
			' Token: 0x04004DAB RID: 19883
			MovecArrows
		End Enum

		' Token: 0x02000B9C RID: 2972
		Private Class ArrowArray
			' Token: 0x17000648 RID: 1608
			' (get) Token: 0x06004834 RID: 18484 RVA: 0x0025EBFA File Offset: 0x0025CFFA
			' (set) Token: 0x06004835 RID: 18485 RVA: 0x0025EC02 File Offset: 0x0025D002
			Public Property mesh As Mesh

			' Token: 0x17000649 RID: 1609
			' (get) Token: 0x06004836 RID: 18486 RVA: 0x0025EC0B File Offset: 0x0025D00B
			' (set) Token: 0x06004837 RID: 18487 RVA: 0x0025EC13 File Offset: 0x0025D013
			Public Property columnCount As Integer

			' Token: 0x1700064A RID: 1610
			' (get) Token: 0x06004838 RID: 18488 RVA: 0x0025EC1C File Offset: 0x0025D01C
			' (set) Token: 0x06004839 RID: 18489 RVA: 0x0025EC24 File Offset: 0x0025D024
			Public Property rowCount As Integer

			' Token: 0x0600483A RID: 18490 RVA: 0x0025EC30 File Offset: 0x0025D030
			Public Sub BuildMesh(columns As Integer, rows As Integer)
				Dim array As Vector3() = New Vector3() { New Vector3(0F, 0F, 0F), New Vector3(0F, 1F, 0F), New Vector3(0F, 1F, 0F), New Vector3(-1F, 1F, 0F), New Vector3(0F, 1F, 0F), New Vector3(1F, 1F, 0F) }
				Dim num As Integer = 6 * columns * rows
				Dim list As List(Of Vector3) = New List(Of Vector3)(num)
				Dim list2 As List(Of Vector2) = New List(Of Vector2)(num)
				For i As Integer = 0 To rows - 1
					For j As Integer = 0 To columns - 1
						Dim vector As Vector2 = New Vector2((0.5F + CSng(j)) / CSng(columns), (0.5F + CSng(i)) / CSng(rows))
						For k As Integer = 0 To 6 - 1
							list.Add(array(k))
							list2.Add(vector)
						Next
					Next
				Next
				Dim array2 As Integer() = New Integer(num - 1) {}
				For l As Integer = 0 To num - 1
					array2(l) = l
				Next
				Me.mesh = New Mesh() With { .hideFlags = HideFlags.DontSave }
				Me.mesh.SetVertices(list)
				Me.mesh.SetUVs(0, list2)
				Me.mesh.SetIndices(array2, MeshTopology.Lines, 0)
				Me.mesh.UploadMeshData(True)
				Me.columnCount = columns
				Me.rowCount = rows
			End Sub

			' Token: 0x0600483B RID: 18491 RVA: 0x0025EE13 File Offset: 0x0025D213
			Public Sub Release()
				GraphicsUtils.Destroy(Me.mesh)
				Me.mesh = Nothing
			End Sub
		End Class
	End Class
End Namespace
