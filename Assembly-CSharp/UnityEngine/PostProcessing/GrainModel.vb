Imports System

Namespace UnityEngine.PostProcessing
	' Token: 0x02000BE7 RID: 3047
	<Serializable()>
	Public Class GrainModel
		Inherits PostProcessingModel

		' Token: 0x17000680 RID: 1664
		' (get) Token: 0x06004907 RID: 18695 RVA: 0x002641ED File Offset: 0x002625ED
		' (set) Token: 0x06004908 RID: 18696 RVA: 0x002641F5 File Offset: 0x002625F5
		Public Property settings As GrainModel.Settings
			Get
				Return Me.m_Settings
			End Get
			Set(value As GrainModel.Settings)
				Me.m_Settings = value
			End Set
		End Property

		' Token: 0x06004909 RID: 18697 RVA: 0x002641FE File Offset: 0x002625FE
		Public Overrides Sub Reset()
			Me.m_Settings = GrainModel.Settings.defaultSettings
		End Sub

		' Token: 0x04004F10 RID: 20240
		<SerializeField()>
		Private m_Settings As GrainModel.Settings = GrainModel.Settings.defaultSettings

		' Token: 0x02000BE8 RID: 3048
		<Serializable()>
		Public Structure Settings
			' Token: 0x17000681 RID: 1665
			' (get) Token: 0x0600490A RID: 18698 RVA: 0x0026420C File Offset: 0x0026260C
			Public Shared ReadOnly Property defaultSettings As GrainModel.Settings
				Get
					Return New GrainModel.Settings() With { .colored = True, .intensity = 0.5F, .size = 1F, .luminanceContribution = 0.8F }
				End Get
			End Property

			' Token: 0x04004F11 RID: 20241
			<Tooltip("Enable the use of colored grain.")>
			Public colored As Boolean

			' Token: 0x04004F12 RID: 20242
			<Range(0F, 1F)>
			<Tooltip("Grain strength. Higher means more visible grain.")>
			Public intensity As Single

			' Token: 0x04004F13 RID: 20243
			<Range(0.3F, 3F)>
			<Tooltip("Grain particle size.")>
			Public size As Single

			' Token: 0x04004F14 RID: 20244
			<Range(0F, 1F)>
			<Tooltip("Controls the noisiness response curve based on scene luminance. Lower values mean less noise in dark areas.")>
			Public luminanceContribution As Single
		End Structure
	End Class
End Namespace
