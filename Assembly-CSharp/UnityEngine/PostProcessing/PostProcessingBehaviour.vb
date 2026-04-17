Imports System
Imports System.Collections.Generic
Imports UnityEngine.Rendering

Namespace UnityEngine.PostProcessing
	' Token: 0x02000BF7 RID: 3063
	<ImageEffectAllowedInSceneView()>
	<RequireComponent(GetType(Camera))>
	<DisallowMultipleComponent()>
	<ExecuteInEditMode()>
	<AddComponentMenu("Effects/Post-Processing Behaviour", -1)>
	Public Class PostProcessingBehaviour
		Inherits MonoBehaviour

		' Token: 0x06004920 RID: 18720 RVA: 0x002644FC File Offset: 0x002628FC
		Private Sub OnEnable()
			Me.m_CommandBuffers = New Dictionary(Of Type, KeyValuePair(Of CameraEvent, CommandBuffer))()
			Me.m_MaterialFactory = New MaterialFactory()
			Me.m_RenderTextureFactory = New RenderTextureFactory()
			Me.m_Context = New PostProcessingContext()
			Me.m_Components = New List(Of PostProcessingComponentBase)()
			Me.m_DebugViews = Me.AddComponent(Of BuiltinDebugViewsComponent)(New BuiltinDebugViewsComponent())
			Me.m_AmbientOcclusion = Me.AddComponent(Of AmbientOcclusionComponent)(New AmbientOcclusionComponent())
			Me.m_ScreenSpaceReflection = Me.AddComponent(Of ScreenSpaceReflectionComponent)(New ScreenSpaceReflectionComponent())
			Me.m_FogComponent = Me.AddComponent(Of FogComponent)(New FogComponent())
			Me.m_MotionBlur = Me.AddComponent(Of MotionBlurComponent)(New MotionBlurComponent())
			Me.m_Taa = Me.AddComponent(Of TaaComponent)(New TaaComponent())
			Me.m_EyeAdaptation = Me.AddComponent(Of EyeAdaptationComponent)(New EyeAdaptationComponent())
			Me.m_DepthOfField = Me.AddComponent(Of DepthOfFieldComponent)(New DepthOfFieldComponent())
			Me.m_Bloom = Me.AddComponent(Of BloomComponent)(New BloomComponent())
			Me.m_ChromaticAberration = Me.AddComponent(Of ChromaticAberrationComponent)(New ChromaticAberrationComponent())
			Me.m_ColorGrading = Me.AddComponent(Of ColorGradingComponent)(New ColorGradingComponent())
			Me.m_UserLut = Me.AddComponent(Of UserLutComponent)(New UserLutComponent())
			Me.m_Grain = Me.AddComponent(Of GrainComponent)(New GrainComponent())
			Me.m_Vignette = Me.AddComponent(Of VignetteComponent)(New VignetteComponent())
			Me.m_Dithering = Me.AddComponent(Of DitheringComponent)(New DitheringComponent())
			Me.m_Fxaa = Me.AddComponent(Of FxaaComponent)(New FxaaComponent())
			Me.m_ComponentStates = New Dictionary(Of PostProcessingComponentBase, Boolean)()
			For Each postProcessingComponentBase As PostProcessingComponentBase In Me.m_Components
				Me.m_ComponentStates.Add(postProcessingComponentBase, False)
			Next
			MyBase.useGUILayout = False
		End Sub

		' Token: 0x06004921 RID: 18721 RVA: 0x002646B8 File Offset: 0x00262AB8
		Private Sub OnPreCull()
			Me.m_Camera = MyBase.GetComponent(Of Camera)()
			If Me.profile Is Nothing OrElse Me.m_Camera Is Nothing Then
				Return
			End If
			Dim postProcessingContext As PostProcessingContext = Me.m_Context.Reset()
			postProcessingContext.profile = Me.profile
			postProcessingContext.renderTextureFactory = Me.m_RenderTextureFactory
			postProcessingContext.materialFactory = Me.m_MaterialFactory
			postProcessingContext.camera = Me.m_Camera
			Me.m_DebugViews.Init(postProcessingContext, Me.profile.debugViews)
			Me.m_AmbientOcclusion.Init(postProcessingContext, Me.profile.ambientOcclusion)
			Me.m_ScreenSpaceReflection.Init(postProcessingContext, Me.profile.screenSpaceReflection)
			Me.m_FogComponent.Init(postProcessingContext, Me.profile.fog)
			Me.m_MotionBlur.Init(postProcessingContext, Me.profile.motionBlur)
			Me.m_Taa.Init(postProcessingContext, Me.profile.antialiasing)
			Me.m_EyeAdaptation.Init(postProcessingContext, Me.profile.eyeAdaptation)
			Me.m_DepthOfField.Init(postProcessingContext, Me.profile.depthOfField)
			Me.m_Bloom.Init(postProcessingContext, Me.profile.bloom)
			Me.m_ChromaticAberration.Init(postProcessingContext, Me.profile.chromaticAberration)
			Me.m_ColorGrading.Init(postProcessingContext, Me.profile.colorGrading)
			Me.m_UserLut.Init(postProcessingContext, Me.profile.userLut)
			Me.m_Grain.Init(postProcessingContext, Me.profile.grain)
			Me.m_Vignette.Init(postProcessingContext, Me.profile.vignette)
			Me.m_Dithering.Init(postProcessingContext, Me.profile.dithering)
			Me.m_Fxaa.Init(postProcessingContext, Me.profile.antialiasing)
			If Me.m_PreviousProfile IsNot Me.profile Then
				Me.DisableComponents()
				Me.m_PreviousProfile = Me.profile
			End If
			Me.CheckObservers()
			Dim depthTextureMode As DepthTextureMode = postProcessingContext.camera.depthTextureMode
			For Each postProcessingComponentBase As PostProcessingComponentBase In Me.m_Components
				If postProcessingComponentBase.active Then
					depthTextureMode = depthTextureMode Or postProcessingComponentBase.GetCameraFlags()
				End If
			Next
			postProcessingContext.camera.depthTextureMode = depthTextureMode
			If Not Me.m_RenderingInSceneView AndAlso Me.m_Taa.active AndAlso Not Me.profile.debugViews.willInterrupt Then
				Me.m_Taa.SetProjectionMatrix(Me.jitteredMatrixFunc)
			End If
		End Sub

		' Token: 0x06004922 RID: 18722 RVA: 0x00264984 File Offset: 0x00262D84
		Private Sub OnPreRender()
			If Me.profile Is Nothing Then
				Return
			End If
			Me.TryExecuteCommandBuffer(Of BuiltinDebugViewsModel)(Me.m_DebugViews)
			Me.TryExecuteCommandBuffer(Of AmbientOcclusionModel)(Me.m_AmbientOcclusion)
			Me.TryExecuteCommandBuffer(Of ScreenSpaceReflectionModel)(Me.m_ScreenSpaceReflection)
			Me.TryExecuteCommandBuffer(Of FogModel)(Me.m_FogComponent)
			If Not Me.m_RenderingInSceneView Then
				Me.TryExecuteCommandBuffer(Of MotionBlurModel)(Me.m_MotionBlur)
			End If
		End Sub

		' Token: 0x06004923 RID: 18723 RVA: 0x002649EC File Offset: 0x00262DEC
		Private Sub OnPostRender()
			If Me.profile Is Nothing OrElse Me.m_Camera Is Nothing Then
				Return
			End If
			If Not Me.m_RenderingInSceneView AndAlso Me.m_Taa.active AndAlso Not Me.profile.debugViews.willInterrupt Then
				Me.m_Context.camera.ResetProjectionMatrix()
			End If
		End Sub

		' Token: 0x06004924 RID: 18724 RVA: 0x00264A5C File Offset: 0x00262E5C
		Private Sub OnRenderImage(source As RenderTexture, destination As RenderTexture)
			If Me.profile Is Nothing OrElse Me.m_Camera Is Nothing Then
				Graphics.Blit(source, destination)
				Return
			End If
			Dim flag As Boolean = False
			Dim active As Boolean = Me.m_Fxaa.active
			Dim flag2 As Boolean = Me.m_Taa.active AndAlso Not Me.m_RenderingInSceneView
			Dim flag3 As Boolean = Me.m_DepthOfField.active AndAlso Not Me.m_RenderingInSceneView
			Dim material As Material = Me.m_MaterialFactory.[Get]("Hidden/Post FX/Uber Shader")
			material.shaderKeywords = Nothing
			Dim renderTexture As RenderTexture = source
			If flag2 Then
				Dim renderTexture2 As RenderTexture = Me.m_RenderTextureFactory.[Get](renderTexture)
				Me.m_Taa.Render(renderTexture, renderTexture2)
				renderTexture = renderTexture2
			End If
			Dim texture As Texture = GraphicsUtils.whiteTexture
			If Me.m_EyeAdaptation.active Then
				flag = True
				texture = Me.m_EyeAdaptation.Prepare(renderTexture, material)
			End If
			material.SetTexture("_AutoExposure", texture)
			If flag3 Then
				flag = True
				Me.m_DepthOfField.Prepare(renderTexture, material, flag2, Me.m_Taa.jitterVector, Me.m_Taa.model.settings.taaSettings.motionBlending)
			End If
			If Me.m_Bloom.active Then
				flag = True
				Me.m_Bloom.Prepare(renderTexture, material, texture)
			End If
			flag = flag Or Me.TryPrepareUberImageEffect(Of ChromaticAberrationModel)(Me.m_ChromaticAberration, material)
			flag = flag Or Me.TryPrepareUberImageEffect(Of ColorGradingModel)(Me.m_ColorGrading, material)
			flag = flag Or Me.TryPrepareUberImageEffect(Of VignetteModel)(Me.m_Vignette, material)
			flag = flag Or Me.TryPrepareUberImageEffect(Of UserLutModel)(Me.m_UserLut, material)
			Dim material2 As Material = If((Not active), Nothing, Me.m_MaterialFactory.[Get]("Hidden/Post FX/FXAA"))
			If active Then
				material2.shaderKeywords = Nothing
				Me.TryPrepareUberImageEffect(Of GrainModel)(Me.m_Grain, material2)
				Me.TryPrepareUberImageEffect(Of DitheringModel)(Me.m_Dithering, material2)
				If flag Then
					Dim renderTexture3 As RenderTexture = Me.m_RenderTextureFactory.[Get](renderTexture)
					Graphics.Blit(renderTexture, renderTexture3, material, 0)
					renderTexture = renderTexture3
				End If
				Me.m_Fxaa.Render(renderTexture, destination)
			Else
				flag = flag Or Me.TryPrepareUberImageEffect(Of GrainModel)(Me.m_Grain, material)
				flag = flag Or Me.TryPrepareUberImageEffect(Of DitheringModel)(Me.m_Dithering, material)
				If flag Then
					If Not GraphicsUtils.isLinearColorSpace Then
						material.EnableKeyword("UNITY_COLORSPACE_GAMMA")
					End If
					Graphics.Blit(renderTexture, destination, material, 0)
				End If
			End If
			If Not flag AndAlso Not active Then
				Graphics.Blit(renderTexture, destination)
			End If
			Me.m_RenderTextureFactory.ReleaseAll()
		End Sub

		' Token: 0x06004925 RID: 18725 RVA: 0x00264CF0 File Offset: 0x002630F0
		Private Sub OnGUI()
			If [Event].current.type <> EventType.Repaint Then
				Return
			End If
			If Me.profile Is Nothing OrElse Me.m_Camera Is Nothing Then
				Return
			End If
			If Me.m_EyeAdaptation.active AndAlso Me.profile.debugViews.IsModeActive(BuiltinDebugViewsModel.Mode.EyeAdaptation) Then
				Me.m_EyeAdaptation.OnGUI()
			ElseIf Me.m_ColorGrading.active AndAlso Me.profile.debugViews.IsModeActive(BuiltinDebugViewsModel.Mode.LogLut) Then
				Me.m_ColorGrading.OnGUI()
			ElseIf Me.m_UserLut.active AndAlso Me.profile.debugViews.IsModeActive(BuiltinDebugViewsModel.Mode.UserLut) Then
				Me.m_UserLut.OnGUI()
			End If
		End Sub

		' Token: 0x06004926 RID: 18726 RVA: 0x00264DD0 File Offset: 0x002631D0
		Private Sub OnDisable()
			For Each keyValuePair As KeyValuePair(Of CameraEvent, CommandBuffer) In Me.m_CommandBuffers.Values
				Me.m_Camera.RemoveCommandBuffer(keyValuePair.Key, keyValuePair.Value)
				keyValuePair.Value.Dispose()
			Next
			Me.m_CommandBuffers.Clear()
			If Me.profile IsNot Nothing Then
				Me.DisableComponents()
			End If
			Me.m_Components.Clear()
			Me.m_MaterialFactory.Dispose()
			Me.m_RenderTextureFactory.Dispose()
			GraphicsUtils.Dispose()
		End Sub

		' Token: 0x06004927 RID: 18727 RVA: 0x00264E98 File Offset: 0x00263298
		Public Sub ResetTemporalEffects()
			Me.m_Taa.ResetHistory()
			Me.m_MotionBlur.ResetHistory()
			Me.m_EyeAdaptation.ResetHistory()
		End Sub

		' Token: 0x06004928 RID: 18728 RVA: 0x00264EBC File Offset: 0x002632BC
		Private Sub CheckObservers()
			For Each keyValuePair As KeyValuePair(Of PostProcessingComponentBase, Boolean) In Me.m_ComponentStates
				Dim key As PostProcessingComponentBase = keyValuePair.Key
				Dim enabled As Boolean = key.GetModel().enabled
				If enabled <> keyValuePair.Value Then
					If enabled Then
						Me.m_ComponentsToEnable.Add(key)
					Else
						Me.m_ComponentsToDisable.Add(key)
					End If
				End If
			Next
			For i As Integer = 0 To Me.m_ComponentsToDisable.Count - 1
				Dim postProcessingComponentBase As PostProcessingComponentBase = Me.m_ComponentsToDisable(i)
				Me.m_ComponentStates(postProcessingComponentBase) = False
				postProcessingComponentBase.OnDisable()
			Next
			For j As Integer = 0 To Me.m_ComponentsToEnable.Count - 1
				Dim postProcessingComponentBase2 As PostProcessingComponentBase = Me.m_ComponentsToEnable(j)
				Me.m_ComponentStates(postProcessingComponentBase2) = True
				postProcessingComponentBase2.OnEnable()
			Next
			Me.m_ComponentsToDisable.Clear()
			Me.m_ComponentsToEnable.Clear()
		End Sub

		' Token: 0x06004929 RID: 18729 RVA: 0x00264FF4 File Offset: 0x002633F4
		Private Sub DisableComponents()
			For Each postProcessingComponentBase As PostProcessingComponentBase In Me.m_Components
				Dim model As PostProcessingModel = postProcessingComponentBase.GetModel()
				If model IsNot Nothing AndAlso model.enabled Then
					postProcessingComponentBase.OnDisable()
				End If
			Next
		End Sub

		' Token: 0x0600492A RID: 18730 RVA: 0x00265068 File Offset: 0x00263468
		Private Function AddCommandBuffer(Of T As PostProcessingModel)(evt As CameraEvent, name As String) As CommandBuffer
			Dim commandBuffer As CommandBuffer = New CommandBuffer() With { .name = name }
			Dim keyValuePair As KeyValuePair(Of CameraEvent, CommandBuffer) = New KeyValuePair(Of CameraEvent, CommandBuffer)(evt, commandBuffer)
			Me.m_CommandBuffers.Add(GetType(T), keyValuePair)
			Me.m_Camera.AddCommandBuffer(evt, keyValuePair.Value)
			Return keyValuePair.Value
		End Function

		' Token: 0x0600492B RID: 18731 RVA: 0x002650C0 File Offset: 0x002634C0
		Private Sub RemoveCommandBuffer(Of T As PostProcessingModel)()
			Dim typeFromHandle As Type = GetType(T)
			Dim keyValuePair As KeyValuePair(Of CameraEvent, CommandBuffer)
			If Not Me.m_CommandBuffers.TryGetValue(typeFromHandle, keyValuePair) Then
				Return
			End If
			Me.m_Camera.RemoveCommandBuffer(keyValuePair.Key, keyValuePair.Value)
			Me.m_CommandBuffers.Remove(typeFromHandle)
			keyValuePair.Value.Dispose()
		End Sub

		' Token: 0x0600492C RID: 18732 RVA: 0x00265120 File Offset: 0x00263520
		Private Function GetCommandBuffer(Of T As PostProcessingModel)(evt As CameraEvent, name As String) As CommandBuffer
			Dim keyValuePair As KeyValuePair(Of CameraEvent, CommandBuffer)
			Dim commandBuffer As CommandBuffer
			If Not Me.m_CommandBuffers.TryGetValue(GetType(T), keyValuePair) Then
				commandBuffer = Me.AddCommandBuffer(Of T)(evt, name)
			ElseIf keyValuePair.Key <> evt Then
				Me.RemoveCommandBuffer(Of T)()
				commandBuffer = Me.AddCommandBuffer(Of T)(evt, name)
			Else
				commandBuffer = keyValuePair.Value
			End If
			Return commandBuffer
		End Function

		' Token: 0x0600492D RID: 18733 RVA: 0x00265184 File Offset: 0x00263584
		Private Sub TryExecuteCommandBuffer(Of T As PostProcessingModel)(component As PostProcessingComponentCommandBuffer(Of T))
			If component.active Then
				Dim commandBuffer As CommandBuffer = Me.GetCommandBuffer(Of T)(component.GetCameraEvent(), component.GetName())
				commandBuffer.Clear()
				component.PopulateCommandBuffer(commandBuffer)
			Else
				Me.RemoveCommandBuffer(Of T)()
			End If
		End Sub

		' Token: 0x0600492E RID: 18734 RVA: 0x002651C7 File Offset: 0x002635C7
		Private Function TryPrepareUberImageEffect(Of T As PostProcessingModel)(component As PostProcessingComponentRenderTexture(Of T), material As Material) As Boolean
			If Not component.active Then
				Return False
			End If
			component.Prepare(material)
			Return True
		End Function

		' Token: 0x0600492F RID: 18735 RVA: 0x002651DE File Offset: 0x002635DE
		Private Function AddComponent(Of T As PostProcessingComponentBase)(component As T) As T
			Me.m_Components.Add(component)
			Return component
		End Function

		' Token: 0x04004F40 RID: 20288
		Public profile As PostProcessingProfile

		' Token: 0x04004F41 RID: 20289
		Public jitteredMatrixFunc As Func(Of Vector2, Matrix4x4)

		' Token: 0x04004F42 RID: 20290
		Private m_CommandBuffers As Dictionary(Of Type, KeyValuePair(Of CameraEvent, CommandBuffer))

		' Token: 0x04004F43 RID: 20291
		Private m_Components As List(Of PostProcessingComponentBase)

		' Token: 0x04004F44 RID: 20292
		Private m_ComponentStates As Dictionary(Of PostProcessingComponentBase, Boolean)

		' Token: 0x04004F45 RID: 20293
		Private m_MaterialFactory As MaterialFactory

		' Token: 0x04004F46 RID: 20294
		Private m_RenderTextureFactory As RenderTextureFactory

		' Token: 0x04004F47 RID: 20295
		Private m_Context As PostProcessingContext

		' Token: 0x04004F48 RID: 20296
		Private m_Camera As Camera

		' Token: 0x04004F49 RID: 20297
		Private m_PreviousProfile As PostProcessingProfile

		' Token: 0x04004F4A RID: 20298
		Private m_RenderingInSceneView As Boolean

		' Token: 0x04004F4B RID: 20299
		Private m_DebugViews As BuiltinDebugViewsComponent

		' Token: 0x04004F4C RID: 20300
		Private m_AmbientOcclusion As AmbientOcclusionComponent

		' Token: 0x04004F4D RID: 20301
		Private m_ScreenSpaceReflection As ScreenSpaceReflectionComponent

		' Token: 0x04004F4E RID: 20302
		Private m_FogComponent As FogComponent

		' Token: 0x04004F4F RID: 20303
		Private m_MotionBlur As MotionBlurComponent

		' Token: 0x04004F50 RID: 20304
		Private m_Taa As TaaComponent

		' Token: 0x04004F51 RID: 20305
		Private m_EyeAdaptation As EyeAdaptationComponent

		' Token: 0x04004F52 RID: 20306
		Private m_DepthOfField As DepthOfFieldComponent

		' Token: 0x04004F53 RID: 20307
		Private m_Bloom As BloomComponent

		' Token: 0x04004F54 RID: 20308
		Private m_ChromaticAberration As ChromaticAberrationComponent

		' Token: 0x04004F55 RID: 20309
		Private m_ColorGrading As ColorGradingComponent

		' Token: 0x04004F56 RID: 20310
		Private m_UserLut As UserLutComponent

		' Token: 0x04004F57 RID: 20311
		Private m_Grain As GrainComponent

		' Token: 0x04004F58 RID: 20312
		Private m_Vignette As VignetteComponent

		' Token: 0x04004F59 RID: 20313
		Private m_Dithering As DitheringComponent

		' Token: 0x04004F5A RID: 20314
		Private m_Fxaa As FxaaComponent

		' Token: 0x04004F5B RID: 20315
		Private m_ComponentsToEnable As List(Of PostProcessingComponentBase) = New List(Of PostProcessingComponentBase)()

		' Token: 0x04004F5C RID: 20316
		Private m_ComponentsToDisable As List(Of PostProcessingComponentBase) = New List(Of PostProcessingComponentBase)()
	End Class
End Namespace
