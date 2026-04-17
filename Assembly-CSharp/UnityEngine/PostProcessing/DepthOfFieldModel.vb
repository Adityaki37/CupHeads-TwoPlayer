Imports System

Namespace UnityEngine.PostProcessing
	' Token: 0x02000BDD RID: 3037
	<Serializable()>
	Public Class DepthOfFieldModel
		Inherits PostProcessingModel

		' Token: 0x17000678 RID: 1656
		' (get) Token: 0x060048F3 RID: 18675 RVA: 0x0026401D File Offset: 0x0026241D
		' (set) Token: 0x060048F4 RID: 18676 RVA: 0x00264025 File Offset: 0x00262425
		Public Property settings As DepthOfFieldModel.Settings
			Get
				Return Me.m_Settings
			End Get
			Set(value As DepthOfFieldModel.Settings)
				Me.m_Settings = value
			End Set
		End Property

		' Token: 0x060048F5 RID: 18677 RVA: 0x0026402E File Offset: 0x0026242E
		Public Overrides Sub Reset()
			Me.m_Settings = DepthOfFieldModel.Settings.defaultSettings
		End Sub

		' Token: 0x04004EF3 RID: 20211
		<SerializeField()>
		Private m_Settings As DepthOfFieldModel.Settings = DepthOfFieldModel.Settings.defaultSettings

		' Token: 0x02000BDE RID: 3038
		Public Enum KernelSize
			' Token: 0x04004EF5 RID: 20213
			Small
			' Token: 0x04004EF6 RID: 20214
			Medium
			' Token: 0x04004EF7 RID: 20215
			Large
			' Token: 0x04004EF8 RID: 20216
			VeryLarge
		End Enum

		' Token: 0x02000BDF RID: 3039
		<Serializable()>
		Public Structure Settings
			' Token: 0x17000679 RID: 1657
			' (get) Token: 0x060048F6 RID: 18678 RVA: 0x0026403C File Offset: 0x0026243C
			Public Shared ReadOnly Property defaultSettings As DepthOfFieldModel.Settings
				Get
					Return New DepthOfFieldModel.Settings() With { .focusDistance = 10F, .aperture = 5.6F, .focalLength = 50F, .useCameraFov = False, .kernelSize = DepthOfFieldModel.KernelSize.Medium }
				End Get
			End Property

			' Token: 0x04004EF9 RID: 20217
			<Min(0.1F)>
			<Tooltip("Distance to the point of focus.")>
			Public focusDistance As Single

			' Token: 0x04004EFA RID: 20218
			<Range(0.05F, 32F)>
			<Tooltip("Ratio of aperture (known as f-stop or f-number). The smaller the value is, the shallower the depth of field is.")>
			Public aperture As Single

			' Token: 0x04004EFB RID: 20219
			<Range(1F, 300F)>
			<Tooltip("Distance between the lens and the film. The larger the value is, the shallower the depth of field is.")>
			Public focalLength As Single

			' Token: 0x04004EFC RID: 20220
			<Tooltip("Calculate the focal length automatically from the field-of-view value set on the camera. Using this setting isn't recommended.")>
			Public useCameraFov As Boolean

			' Token: 0x04004EFD RID: 20221
			<Tooltip("Convolution kernel size of the bokeh filter, which determines the maximum radius of bokeh. It also affects the performance (the larger the kernel is, the longer the GPU time is required).")>
			Public kernelSize As DepthOfFieldModel.KernelSize
		End Structure
	End Class
End Namespace
