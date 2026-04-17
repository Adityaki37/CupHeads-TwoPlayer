Imports System

Namespace UnityEngine.PostProcessing
	' Token: 0x02000BE9 RID: 3049
	<Serializable()>
	Public Class MotionBlurModel
		Inherits PostProcessingModel

		' Token: 0x17000682 RID: 1666
		' (get) Token: 0x0600490C RID: 18700 RVA: 0x00264261 File Offset: 0x00262661
		' (set) Token: 0x0600490D RID: 18701 RVA: 0x00264269 File Offset: 0x00262669
		Public Property settings As MotionBlurModel.Settings
			Get
				Return Me.m_Settings
			End Get
			Set(value As MotionBlurModel.Settings)
				Me.m_Settings = value
			End Set
		End Property

		' Token: 0x0600490E RID: 18702 RVA: 0x00264272 File Offset: 0x00262672
		Public Overrides Sub Reset()
			Me.m_Settings = MotionBlurModel.Settings.defaultSettings
		End Sub

		' Token: 0x04004F15 RID: 20245
		<SerializeField()>
		Private m_Settings As MotionBlurModel.Settings = MotionBlurModel.Settings.defaultSettings

		' Token: 0x02000BEA RID: 3050
		<Serializable()>
		Public Structure Settings
			' Token: 0x17000683 RID: 1667
			' (get) Token: 0x0600490F RID: 18703 RVA: 0x00264280 File Offset: 0x00262680
			Public Shared ReadOnly Property defaultSettings As MotionBlurModel.Settings
				Get
					Return New MotionBlurModel.Settings() With { .shutterAngle = 270F, .sampleCount = 10, .frameBlending = 0F }
				End Get
			End Property

			' Token: 0x04004F16 RID: 20246
			<Range(0F, 360F)>
			<Tooltip("The angle of rotary shutter. Larger values give longer exposure.")>
			Public shutterAngle As Single

			' Token: 0x04004F17 RID: 20247
			<Range(4F, 32F)>
			<Tooltip("The amount of sample points, which affects quality and performances.")>
			Public sampleCount As Integer

			' Token: 0x04004F18 RID: 20248
			<Range(0F, 1F)>
			<Tooltip("The strength of multiple frame blending. The opacity of preceding frames are determined from this coefficient and time differences.")>
			Public frameBlending As Single
		End Structure
	End Class
End Namespace
