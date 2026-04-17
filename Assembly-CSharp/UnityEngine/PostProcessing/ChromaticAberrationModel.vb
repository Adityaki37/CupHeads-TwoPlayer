Imports System

Namespace UnityEngine.PostProcessing
	' Token: 0x02000BD0 RID: 3024
	<Serializable()>
	Public Class ChromaticAberrationModel
		Inherits PostProcessingModel

		' Token: 0x1700066B RID: 1643
		' (get) Token: 0x060048DD RID: 18653 RVA: 0x00263A6D File Offset: 0x00261E6D
		' (set) Token: 0x060048DE RID: 18654 RVA: 0x00263A75 File Offset: 0x00261E75
		Public Property settings As ChromaticAberrationModel.Settings
			Get
				Return Me.m_Settings
			End Get
			Set(value As ChromaticAberrationModel.Settings)
				Me.m_Settings = value
			End Set
		End Property

		' Token: 0x060048DF RID: 18655 RVA: 0x00263A7E File Offset: 0x00261E7E
		Public Overrides Sub Reset()
			Me.m_Settings = ChromaticAberrationModel.Settings.defaultSettings
		End Sub

		' Token: 0x04004EBA RID: 20154
		<SerializeField()>
		Private m_Settings As ChromaticAberrationModel.Settings = ChromaticAberrationModel.Settings.defaultSettings

		' Token: 0x02000BD1 RID: 3025
		<Serializable()>
		Public Structure Settings
			' Token: 0x1700066C RID: 1644
			' (get) Token: 0x060048E0 RID: 18656 RVA: 0x00263A8C File Offset: 0x00261E8C
			Public Shared ReadOnly Property defaultSettings As ChromaticAberrationModel.Settings
				Get
					Return New ChromaticAberrationModel.Settings() With { .spectralTexture = Nothing, .intensity = 0.1F }
				End Get
			End Property

			' Token: 0x04004EBB RID: 20155
			<Tooltip("Shift the hue of chromatic aberrations.")>
			Public spectralTexture As Texture2D

			' Token: 0x04004EBC RID: 20156
			<Range(0F, 1F)>
			<Tooltip("Amount of tangential distortion.")>
			Public intensity As Single
		End Structure
	End Class
End Namespace
