Imports System

Namespace UnityEngine.PostProcessing
	' Token: 0x02000BBC RID: 3004
	<Serializable()>
	Public Class AmbientOcclusionModel
		Inherits PostProcessingModel

		' Token: 0x1700065B RID: 1627
		' (get) Token: 0x060048BD RID: 18621 RVA: 0x0026343A File Offset: 0x0026183A
		' (set) Token: 0x060048BE RID: 18622 RVA: 0x00263442 File Offset: 0x00261842
		Public Property settings As AmbientOcclusionModel.Settings
			Get
				Return Me.m_Settings
			End Get
			Set(value As AmbientOcclusionModel.Settings)
				Me.m_Settings = value
			End Set
		End Property

		' Token: 0x060048BF RID: 18623 RVA: 0x0026344B File Offset: 0x0026184B
		Public Overrides Sub Reset()
			Me.m_Settings = AmbientOcclusionModel.Settings.defaultSettings
		End Sub

		' Token: 0x04004E72 RID: 20082
		<SerializeField()>
		Private m_Settings As AmbientOcclusionModel.Settings = AmbientOcclusionModel.Settings.defaultSettings

		' Token: 0x02000BBD RID: 3005
		Public Enum SampleCount
			' Token: 0x04004E74 RID: 20084
			Lowest = 3
			' Token: 0x04004E75 RID: 20085
			Low = 6
			' Token: 0x04004E76 RID: 20086
			Medium = 10
			' Token: 0x04004E77 RID: 20087
			High = 16
		End Enum

		' Token: 0x02000BBE RID: 3006
		<Serializable()>
		Public Structure Settings
			' Token: 0x1700065C RID: 1628
			' (get) Token: 0x060048C0 RID: 18624 RVA: 0x00263458 File Offset: 0x00261858
			Public Shared ReadOnly Property defaultSettings As AmbientOcclusionModel.Settings
				Get
					Return New AmbientOcclusionModel.Settings() With { .intensity = 1F, .radius = 0.3F, .sampleCount = AmbientOcclusionModel.SampleCount.Medium, .downsampling = True, .forceForwardCompatibility = False, .ambientOnly = False, .highPrecision = False }
				End Get
			End Property

			' Token: 0x04004E78 RID: 20088
			<Range(0F, 4F)>
			<Tooltip("Degree of darkness produced by the effect.")>
			Public intensity As Single

			' Token: 0x04004E79 RID: 20089
			<Min(0.0001F)>
			<Tooltip("Radius of sample points, which affects extent of darkened areas.")>
			Public radius As Single

			' Token: 0x04004E7A RID: 20090
			<Tooltip("Number of sample points, which affects quality and performance.")>
			Public sampleCount As AmbientOcclusionModel.SampleCount

			' Token: 0x04004E7B RID: 20091
			<Tooltip("Halves the resolution of the effect to increase performance at the cost of visual quality.")>
			Public downsampling As Boolean

			' Token: 0x04004E7C RID: 20092
			<Tooltip("Forces compatibility with Forward rendered objects when working with the Deferred rendering path.")>
			Public forceForwardCompatibility As Boolean

			' Token: 0x04004E7D RID: 20093
			<Tooltip("Enables the ambient-only mode in that the effect only affects ambient lighting. This mode is only available with the Deferred rendering path and HDR rendering.")>
			Public ambientOnly As Boolean

			' Token: 0x04004E7E RID: 20094
			<Tooltip("Toggles the use of a higher precision depth texture with the forward rendering path (may impact performances). Has no effect with the deferred rendering path.")>
			Public highPrecision As Boolean
		End Structure
	End Class
End Namespace
