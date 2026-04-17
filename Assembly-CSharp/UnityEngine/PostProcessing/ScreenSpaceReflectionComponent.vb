Imports System
Imports UnityEngine.Rendering

Namespace UnityEngine.PostProcessing
	' Token: 0x02000BB3 RID: 2995
	Public NotInheritable Class ScreenSpaceReflectionComponent
		Inherits PostProcessingComponentCommandBuffer(Of ScreenSpaceReflectionModel)

		' Token: 0x0600489E RID: 18590 RVA: 0x00261EBB File Offset: 0x002602BB
		Public Overrides Function GetCameraFlags() As DepthTextureMode
			Return DepthTextureMode.Depth
		End Function

		' Token: 0x17000656 RID: 1622
		' (get) Token: 0x0600489F RID: 18591 RVA: 0x00261EBE File Offset: 0x002602BE
		Public Overrides ReadOnly Property active As Boolean
			Get
				Return MyBase.model.enabled AndAlso Me.context.isGBufferAvailable AndAlso Not Me.context.interrupted
			End Get
		End Property

		' Token: 0x060048A0 RID: 18592 RVA: 0x00261EF4 File Offset: 0x002602F4
		Public Overrides Sub OnEnable()
			Me.m_ReflectionTextures(0) = Shader.PropertyToID("_ReflectionTexture0")
			Me.m_ReflectionTextures(1) = Shader.PropertyToID("_ReflectionTexture1")
			Me.m_ReflectionTextures(2) = Shader.PropertyToID("_ReflectionTexture2")
			Me.m_ReflectionTextures(3) = Shader.PropertyToID("_ReflectionTexture3")
			Me.m_ReflectionTextures(4) = Shader.PropertyToID("_ReflectionTexture4")
		End Sub

		' Token: 0x060048A1 RID: 18593 RVA: 0x00261F5B File Offset: 0x0026035B
		Public Overrides Function GetName() As String
			Return "Screen Space Reflection"
		End Function

		' Token: 0x060048A2 RID: 18594 RVA: 0x00261F62 File Offset: 0x00260362
		Public Overrides Function GetCameraEvent() As CameraEvent
			Return CameraEvent.AfterFinalPass
		End Function

		' Token: 0x060048A3 RID: 18595 RVA: 0x00261F68 File Offset: 0x00260368
		Public Overrides Sub PopulateCommandBuffer(cb As CommandBuffer)
			Dim settings As ScreenSpaceReflectionModel.Settings = MyBase.model.settings
			Dim camera As Camera = Me.context.camera
			Dim num As Integer = If((settings.reflection.reflectionQuality <> ScreenSpaceReflectionModel.SSRResolution.High), 2, 1)
			Dim num2 As Integer = Me.context.width / num
			Dim num3 As Integer = Me.context.height / num
			Dim num4 As Single = CSng(Me.context.width)
			Dim num5 As Single = CSng(Me.context.height)
			Dim num6 As Single = num4 / 2F
			Dim num7 As Single = num5 / 2F
			Dim material As Material = Me.context.materialFactory.[Get]("Hidden/Post FX/Screen Space Reflection")
			material.SetInt(ScreenSpaceReflectionComponent.Uniforms._RayStepSize, settings.reflection.stepSize)
			material.SetInt(ScreenSpaceReflectionComponent.Uniforms._AdditiveReflection, If((settings.reflection.blendType <> ScreenSpaceReflectionModel.SSRReflectionBlendType.Additive), 0, 1))
			material.SetInt(ScreenSpaceReflectionComponent.Uniforms._BilateralUpsampling, If((Not Me.k_BilateralUpsample), 0, 1))
			material.SetInt(ScreenSpaceReflectionComponent.Uniforms._TreatBackfaceHitAsMiss, If((Not Me.k_TreatBackfaceHitAsMiss), 0, 1))
			material.SetInt(ScreenSpaceReflectionComponent.Uniforms._AllowBackwardsRays, If((Not settings.reflection.reflectBackfaces), 0, 1))
			material.SetInt(ScreenSpaceReflectionComponent.Uniforms._TraceBehindObjects, If((Not Me.k_TraceBehindObjects), 0, 1))
			material.SetInt(ScreenSpaceReflectionComponent.Uniforms._MaxSteps, settings.reflection.iterationCount)
			material.SetInt(ScreenSpaceReflectionComponent.Uniforms._FullResolutionFiltering, 0)
			material.SetInt(ScreenSpaceReflectionComponent.Uniforms._HalfResolution, If((settings.reflection.reflectionQuality = ScreenSpaceReflectionModel.SSRResolution.High), 0, 1))
			material.SetInt(ScreenSpaceReflectionComponent.Uniforms._HighlightSuppression, If((Not Me.k_HighlightSuppression), 0, 1))
			Dim num8 As Single = num4 / (-2F * Mathf.Tan(camera.fieldOfView / 180F * 3.1415927F * 0.5F))
			material.SetFloat(ScreenSpaceReflectionComponent.Uniforms._PixelsPerMeterAtOneMeter, num8)
			material.SetFloat(ScreenSpaceReflectionComponent.Uniforms._ScreenEdgeFading, settings.screenEdgeMask.intensity)
			material.SetFloat(ScreenSpaceReflectionComponent.Uniforms._ReflectionBlur, settings.reflection.reflectionBlur)
			material.SetFloat(ScreenSpaceReflectionComponent.Uniforms._MaxRayTraceDistance, settings.reflection.maxDistance)
			material.SetFloat(ScreenSpaceReflectionComponent.Uniforms._FadeDistance, settings.intensity.fadeDistance)
			material.SetFloat(ScreenSpaceReflectionComponent.Uniforms._LayerThickness, settings.reflection.widthModifier)
			material.SetFloat(ScreenSpaceReflectionComponent.Uniforms._SSRMultiplier, settings.intensity.reflectionMultiplier)
			material.SetFloat(ScreenSpaceReflectionComponent.Uniforms._FresnelFade, settings.intensity.fresnelFade)
			material.SetFloat(ScreenSpaceReflectionComponent.Uniforms._FresnelFadePower, settings.intensity.fresnelFadePower)
			Dim projectionMatrix As Matrix4x4 = camera.projectionMatrix
			Dim vector As Vector4 = New Vector4(-2F / (num4 * projectionMatrix(0)), -2F / (num5 * projectionMatrix(5)), (1F - projectionMatrix(2)) / projectionMatrix(0), (1F + projectionMatrix(6)) / projectionMatrix(5))
			Dim vector2 As Vector3 = If((Not Single.IsPositiveInfinity(camera.farClipPlane)), New Vector3(camera.nearClipPlane * camera.farClipPlane, camera.nearClipPlane - camera.farClipPlane, camera.farClipPlane), New Vector3(camera.nearClipPlane, -1F, 1F))
			material.SetVector(ScreenSpaceReflectionComponent.Uniforms._ReflectionBufferSize, New Vector2(CSng(num2), CSng(num3)))
			material.SetVector(ScreenSpaceReflectionComponent.Uniforms._ScreenSize, New Vector2(num4, num5))
			material.SetVector(ScreenSpaceReflectionComponent.Uniforms._InvScreenSize, New Vector2(1F / num4, 1F / num5))
			material.SetVector(ScreenSpaceReflectionComponent.Uniforms._ProjInfo, vector)
			material.SetVector(ScreenSpaceReflectionComponent.Uniforms._CameraClipInfo, vector2)
			Dim matrix4x As Matrix4x4 = Nothing
			matrix4x.SetRow(0, New Vector4(num6, 0F, 0F, num6))
			matrix4x.SetRow(1, New Vector4(0F, num7, 0F, num7))
			matrix4x.SetRow(2, New Vector4(0F, 0F, 1F, 0F))
			matrix4x.SetRow(3, New Vector4(0F, 0F, 0F, 1F))
			Dim matrix4x2 As Matrix4x4 = matrix4x * projectionMatrix
			material.SetMatrix(ScreenSpaceReflectionComponent.Uniforms._ProjectToPixelMatrix, matrix4x2)
			material.SetMatrix(ScreenSpaceReflectionComponent.Uniforms._WorldToCameraMatrix, camera.worldToCameraMatrix)
			material.SetMatrix(ScreenSpaceReflectionComponent.Uniforms._CameraToWorldMatrix, camera.worldToCameraMatrix.inverse)
			Dim renderTextureFormat As RenderTextureFormat = If((Not Me.context.isHdr), RenderTextureFormat.ARGB32, RenderTextureFormat.ARGBHalf)
			Dim normalAndRoughnessTexture As Integer = ScreenSpaceReflectionComponent.Uniforms._NormalAndRoughnessTexture
			Dim hitPointTexture As Integer = ScreenSpaceReflectionComponent.Uniforms._HitPointTexture
			Dim blurTexture As Integer = ScreenSpaceReflectionComponent.Uniforms._BlurTexture
			Dim filteredReflections As Integer = ScreenSpaceReflectionComponent.Uniforms._FilteredReflections
			Dim finalReflectionTexture As Integer = ScreenSpaceReflectionComponent.Uniforms._FinalReflectionTexture
			Dim tempTexture As Integer = ScreenSpaceReflectionComponent.Uniforms._TempTexture
			cb.GetTemporaryRT(normalAndRoughnessTexture, -1, -1, 0, FilterMode.Point, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear)
			cb.GetTemporaryRT(hitPointTexture, num2, num3, 0, FilterMode.Bilinear, RenderTextureFormat.ARGBHalf, RenderTextureReadWrite.Linear)
			For i As Integer = 0 To 5 - 1
				cb.GetTemporaryRT(Me.m_ReflectionTextures(i), num2 >> i, num3 >> i, 0, FilterMode.Bilinear, renderTextureFormat)
			Next
			cb.GetTemporaryRT(filteredReflections, num2, num3, 0, If((Not Me.k_BilateralUpsample), FilterMode.Bilinear, FilterMode.Point), renderTextureFormat)
			cb.GetTemporaryRT(finalReflectionTexture, num2, num3, 0, FilterMode.Point, renderTextureFormat)
			cb.Blit(BuiltinRenderTextureType.CameraTarget, normalAndRoughnessTexture, material, 6)
			cb.Blit(BuiltinRenderTextureType.CameraTarget, hitPointTexture, material, 0)
			cb.Blit(BuiltinRenderTextureType.CameraTarget, filteredReflections, material, 5)
			cb.Blit(filteredReflections, Me.m_ReflectionTextures(0), material, 8)
			For j As Integer = 1 To 5 - 1
				Dim num9 As Integer = Me.m_ReflectionTextures(j - 1)
				Dim num10 As Integer = j
				cb.GetTemporaryRT(blurTexture, num2 >> num10, num3 >> num10, 0, FilterMode.Bilinear, renderTextureFormat)
				cb.SetGlobalVector(ScreenSpaceReflectionComponent.Uniforms._Axis, New Vector4(1F, 0F, 0F, 0F))
				cb.SetGlobalFloat(ScreenSpaceReflectionComponent.Uniforms._CurrentMipLevel, CSng(j) - 1F)
				cb.Blit(num9, blurTexture, material, 2)
				cb.SetGlobalVector(ScreenSpaceReflectionComponent.Uniforms._Axis, New Vector4(0F, 1F, 0F, 0F))
				num9 = Me.m_ReflectionTextures(j)
				cb.Blit(blurTexture, num9, material, 2)
				cb.ReleaseTemporaryRT(blurTexture)
			Next
			cb.Blit(Me.m_ReflectionTextures(0), finalReflectionTexture, material, 3)
			cb.GetTemporaryRT(tempTexture, camera.pixelWidth, camera.pixelHeight, 0, FilterMode.Bilinear, renderTextureFormat)
			cb.Blit(BuiltinRenderTextureType.CameraTarget, tempTexture, material, 1)
			cb.Blit(tempTexture, BuiltinRenderTextureType.CameraTarget)
			cb.ReleaseTemporaryRT(tempTexture)
		End Sub

		' Token: 0x04004E2D RID: 20013
		Private k_HighlightSuppression As Boolean

		' Token: 0x04004E2E RID: 20014
		Private k_TraceBehindObjects As Boolean = True

		' Token: 0x04004E2F RID: 20015
		Private k_TreatBackfaceHitAsMiss As Boolean

		' Token: 0x04004E30 RID: 20016
		Private k_BilateralUpsample As Boolean = True

		' Token: 0x04004E31 RID: 20017
		Private m_ReflectionTextures As Integer() = New Integer(4) {}

		' Token: 0x02000BB4 RID: 2996
		Private NotInheritable Class Uniforms
			' Token: 0x04004E32 RID: 20018
			Friend Shared _RayStepSize As Integer = Shader.PropertyToID("_RayStepSize")

			' Token: 0x04004E33 RID: 20019
			Friend Shared _AdditiveReflection As Integer = Shader.PropertyToID("_AdditiveReflection")

			' Token: 0x04004E34 RID: 20020
			Friend Shared _BilateralUpsampling As Integer = Shader.PropertyToID("_BilateralUpsampling")

			' Token: 0x04004E35 RID: 20021
			Friend Shared _TreatBackfaceHitAsMiss As Integer = Shader.PropertyToID("_TreatBackfaceHitAsMiss")

			' Token: 0x04004E36 RID: 20022
			Friend Shared _AllowBackwardsRays As Integer = Shader.PropertyToID("_AllowBackwardsRays")

			' Token: 0x04004E37 RID: 20023
			Friend Shared _TraceBehindObjects As Integer = Shader.PropertyToID("_TraceBehindObjects")

			' Token: 0x04004E38 RID: 20024
			Friend Shared _MaxSteps As Integer = Shader.PropertyToID("_MaxSteps")

			' Token: 0x04004E39 RID: 20025
			Friend Shared _FullResolutionFiltering As Integer = Shader.PropertyToID("_FullResolutionFiltering")

			' Token: 0x04004E3A RID: 20026
			Friend Shared _HalfResolution As Integer = Shader.PropertyToID("_HalfResolution")

			' Token: 0x04004E3B RID: 20027
			Friend Shared _HighlightSuppression As Integer = Shader.PropertyToID("_HighlightSuppression")

			' Token: 0x04004E3C RID: 20028
			Friend Shared _PixelsPerMeterAtOneMeter As Integer = Shader.PropertyToID("_PixelsPerMeterAtOneMeter")

			' Token: 0x04004E3D RID: 20029
			Friend Shared _ScreenEdgeFading As Integer = Shader.PropertyToID("_ScreenEdgeFading")

			' Token: 0x04004E3E RID: 20030
			Friend Shared _ReflectionBlur As Integer = Shader.PropertyToID("_ReflectionBlur")

			' Token: 0x04004E3F RID: 20031
			Friend Shared _MaxRayTraceDistance As Integer = Shader.PropertyToID("_MaxRayTraceDistance")

			' Token: 0x04004E40 RID: 20032
			Friend Shared _FadeDistance As Integer = Shader.PropertyToID("_FadeDistance")

			' Token: 0x04004E41 RID: 20033
			Friend Shared _LayerThickness As Integer = Shader.PropertyToID("_LayerThickness")

			' Token: 0x04004E42 RID: 20034
			Friend Shared _SSRMultiplier As Integer = Shader.PropertyToID("_SSRMultiplier")

			' Token: 0x04004E43 RID: 20035
			Friend Shared _FresnelFade As Integer = Shader.PropertyToID("_FresnelFade")

			' Token: 0x04004E44 RID: 20036
			Friend Shared _FresnelFadePower As Integer = Shader.PropertyToID("_FresnelFadePower")

			' Token: 0x04004E45 RID: 20037
			Friend Shared _ReflectionBufferSize As Integer = Shader.PropertyToID("_ReflectionBufferSize")

			' Token: 0x04004E46 RID: 20038
			Friend Shared _ScreenSize As Integer = Shader.PropertyToID("_ScreenSize")

			' Token: 0x04004E47 RID: 20039
			Friend Shared _InvScreenSize As Integer = Shader.PropertyToID("_InvScreenSize")

			' Token: 0x04004E48 RID: 20040
			Friend Shared _ProjInfo As Integer = Shader.PropertyToID("_ProjInfo")

			' Token: 0x04004E49 RID: 20041
			Friend Shared _CameraClipInfo As Integer = Shader.PropertyToID("_CameraClipInfo")

			' Token: 0x04004E4A RID: 20042
			Friend Shared _ProjectToPixelMatrix As Integer = Shader.PropertyToID("_ProjectToPixelMatrix")

			' Token: 0x04004E4B RID: 20043
			Friend Shared _WorldToCameraMatrix As Integer = Shader.PropertyToID("_WorldToCameraMatrix")

			' Token: 0x04004E4C RID: 20044
			Friend Shared _CameraToWorldMatrix As Integer = Shader.PropertyToID("_CameraToWorldMatrix")

			' Token: 0x04004E4D RID: 20045
			Friend Shared _Axis As Integer = Shader.PropertyToID("_Axis")

			' Token: 0x04004E4E RID: 20046
			Friend Shared _CurrentMipLevel As Integer = Shader.PropertyToID("_CurrentMipLevel")

			' Token: 0x04004E4F RID: 20047
			Friend Shared _NormalAndRoughnessTexture As Integer = Shader.PropertyToID("_NormalAndRoughnessTexture")

			' Token: 0x04004E50 RID: 20048
			Friend Shared _HitPointTexture As Integer = Shader.PropertyToID("_HitPointTexture")

			' Token: 0x04004E51 RID: 20049
			Friend Shared _BlurTexture As Integer = Shader.PropertyToID("_BlurTexture")

			' Token: 0x04004E52 RID: 20050
			Friend Shared _FilteredReflections As Integer = Shader.PropertyToID("_FilteredReflections")

			' Token: 0x04004E53 RID: 20051
			Friend Shared _FinalReflectionTexture As Integer = Shader.PropertyToID("_FinalReflectionTexture")

			' Token: 0x04004E54 RID: 20052
			Friend Shared _TempTexture As Integer = Shader.PropertyToID("_TempTexture")
		End Class

		' Token: 0x02000BB5 RID: 2997
		Private Enum PassIndex
			' Token: 0x04004E56 RID: 20054
			RayTraceStep
			' Token: 0x04004E57 RID: 20055
			CompositeFinal
			' Token: 0x04004E58 RID: 20056
			Blur
			' Token: 0x04004E59 RID: 20057
			CompositeSSR
			' Token: 0x04004E5A RID: 20058
			MinMipGeneration
			' Token: 0x04004E5B RID: 20059
			HitPointToReflections
			' Token: 0x04004E5C RID: 20060
			BilateralKeyPack
			' Token: 0x04004E5D RID: 20061
			BlitDepthAsCSZ
			' Token: 0x04004E5E RID: 20062
			PoissonBlur
		End Enum
	End Class
End Namespace
