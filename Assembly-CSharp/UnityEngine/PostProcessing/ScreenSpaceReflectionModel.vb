Imports System

Namespace UnityEngine.PostProcessing
	' Token: 0x02000BEB RID: 3051
	<Serializable()>
	Public Class ScreenSpaceReflectionModel
		Inherits PostProcessingModel

		' Token: 0x17000684 RID: 1668
		' (get) Token: 0x06004911 RID: 18705 RVA: 0x002642CA File Offset: 0x002626CA
		' (set) Token: 0x06004912 RID: 18706 RVA: 0x002642D2 File Offset: 0x002626D2
		Public Property settings As ScreenSpaceReflectionModel.Settings
			Get
				Return Me.m_Settings
			End Get
			Set(value As ScreenSpaceReflectionModel.Settings)
				Me.m_Settings = value
			End Set
		End Property

		' Token: 0x06004913 RID: 18707 RVA: 0x002642DB File Offset: 0x002626DB
		Public Overrides Sub Reset()
			Me.m_Settings = ScreenSpaceReflectionModel.Settings.defaultSettings
		End Sub

		' Token: 0x04004F19 RID: 20249
		<SerializeField()>
		Private m_Settings As ScreenSpaceReflectionModel.Settings = ScreenSpaceReflectionModel.Settings.defaultSettings

		' Token: 0x02000BEC RID: 3052
		Public Enum SSRResolution
			' Token: 0x04004F1B RID: 20251
			High
			' Token: 0x04004F1C RID: 20252
			Low = 2
		End Enum

		' Token: 0x02000BED RID: 3053
		Public Enum SSRReflectionBlendType
			' Token: 0x04004F1E RID: 20254
			PhysicallyBased
			' Token: 0x04004F1F RID: 20255
			Additive
		End Enum

		' Token: 0x02000BEE RID: 3054
		<Serializable()>
		Public Structure IntensitySettings
			' Token: 0x04004F20 RID: 20256
			<Tooltip("Nonphysical multiplier for the SSR reflections. 1.0 is physically based.")>
			<Range(0F, 2F)>
			Public reflectionMultiplier As Single

			' Token: 0x04004F21 RID: 20257
			<Tooltip("How far away from the maxDistance to begin fading SSR.")>
			<Range(0F, 1000F)>
			Public fadeDistance As Single

			' Token: 0x04004F22 RID: 20258
			<Tooltip("Amplify Fresnel fade out. Increase if floor reflections look good close to the surface and bad farther 'under' the floor.")>
			<Range(0F, 1F)>
			Public fresnelFade As Single

			' Token: 0x04004F23 RID: 20259
			<Tooltip("Higher values correspond to a faster Fresnel fade as the reflection changes from the grazing angle.")>
			<Range(0.1F, 10F)>
			Public fresnelFadePower As Single
		End Structure

		' Token: 0x02000BEF RID: 3055
		<Serializable()>
		Public Structure ReflectionSettings
			' Token: 0x04004F24 RID: 20260
			<Tooltip("How the reflections are blended into the render.")>
			Public blendType As ScreenSpaceReflectionModel.SSRReflectionBlendType

			' Token: 0x04004F25 RID: 20261
			<Tooltip("Half resolution SSRR is much faster, but less accurate.")>
			Public reflectionQuality As ScreenSpaceReflectionModel.SSRResolution

			' Token: 0x04004F26 RID: 20262
			<Tooltip("Maximum reflection distance in world units.")>
			<Range(0.1F, 300F)>
			Public maxDistance As Single

			' Token: 0x04004F27 RID: 20263
			<Tooltip("Max raytracing length.")>
			<Range(16F, 1024F)>
			Public iterationCount As Integer

			' Token: 0x04004F28 RID: 20264
			<Tooltip("Log base 2 of ray tracing coarse step size. Higher traces farther, lower gives better quality silhouettes.")>
			<Range(1F, 16F)>
			Public stepSize As Integer

			' Token: 0x04004F29 RID: 20265
			<Tooltip("Typical thickness of columns, walls, furniture, and other objects that reflection rays might pass behind.")>
			<Range(0.01F, 10F)>
			Public widthModifier As Single

			' Token: 0x04004F2A RID: 20266
			<Tooltip("Blurriness of reflections.")>
			<Range(0.1F, 8F)>
			Public reflectionBlur As Single

			' Token: 0x04004F2B RID: 20267
			<Tooltip("Disable for a performance gain in scenes where most glossy objects are horizontal, like floors, water, and tables. Leave on for scenes with glossy vertical objects.")>
			Public reflectBackfaces As Boolean
		End Structure

		' Token: 0x02000BF0 RID: 3056
		<Serializable()>
		Public Structure ScreenEdgeMask
			' Token: 0x04004F2C RID: 20268
			<Tooltip("Higher = fade out SSRR near the edge of the screen so that reflections don't pop under camera motion.")>
			<Range(0F, 1F)>
			Public intensity As Single
		End Structure

		' Token: 0x02000BF1 RID: 3057
		<Serializable()>
		Public Structure Settings
			' Token: 0x17000685 RID: 1669
			' (get) Token: 0x06004914 RID: 18708 RVA: 0x002642E8 File Offset: 0x002626E8
			Public Shared ReadOnly Property defaultSettings As ScreenSpaceReflectionModel.Settings
				Get
					Return New ScreenSpaceReflectionModel.Settings() With { .reflection = New ScreenSpaceReflectionModel.ReflectionSettings() With { .blendType = ScreenSpaceReflectionModel.SSRReflectionBlendType.PhysicallyBased, .reflectionQuality = ScreenSpaceReflectionModel.SSRResolution.Low, .maxDistance = 100F, .iterationCount = 256, .stepSize = 3, .widthModifier = 0.5F, .reflectionBlur = 1F, .reflectBackfaces = False }, .intensity = New ScreenSpaceReflectionModel.IntensitySettings() With { .reflectionMultiplier = 1F, .fadeDistance = 100F, .fresnelFade = 1F, .fresnelFadePower = 1F }, .screenEdgeMask = New ScreenSpaceReflectionModel.ScreenEdgeMask() With { .intensity = 0.03F } }
				End Get
			End Property

			' Token: 0x04004F2D RID: 20269
			Public reflection As ScreenSpaceReflectionModel.ReflectionSettings

			' Token: 0x04004F2E RID: 20270
			Public intensity As ScreenSpaceReflectionModel.IntensitySettings

			' Token: 0x04004F2F RID: 20271
			Public screenEdgeMask As ScreenSpaceReflectionModel.ScreenEdgeMask
		End Structure
	End Class
End Namespace
