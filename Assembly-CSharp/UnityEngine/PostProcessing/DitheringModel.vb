Imports System

Namespace UnityEngine.PostProcessing
	' Token: 0x02000BE0 RID: 3040
	<Serializable()>
	Public Class DitheringModel
		Inherits PostProcessingModel

		' Token: 0x1700067A RID: 1658
		' (get) Token: 0x060048F8 RID: 18680 RVA: 0x00264099 File Offset: 0x00262499
		' (set) Token: 0x060048F9 RID: 18681 RVA: 0x002640A1 File Offset: 0x002624A1
		Public Property settings As DitheringModel.Settings
			Get
				Return Me.m_Settings
			End Get
			Set(value As DitheringModel.Settings)
				Me.m_Settings = value
			End Set
		End Property

		' Token: 0x060048FA RID: 18682 RVA: 0x002640AA File Offset: 0x002624AA
		Public Overrides Sub Reset()
			Me.m_Settings = DitheringModel.Settings.defaultSettings
		End Sub

		' Token: 0x04004EFE RID: 20222
		<SerializeField()>
		Private m_Settings As DitheringModel.Settings = DitheringModel.Settings.defaultSettings

		' Token: 0x02000BE1 RID: 3041
		<Serializable()>
		Public Structure Settings
			' Token: 0x1700067B RID: 1659
			' (get) Token: 0x060048FB RID: 18683 RVA: 0x002640B8 File Offset: 0x002624B8
			Public Shared ReadOnly Property defaultSettings As DitheringModel.Settings
				Get
					Return Nothing
				End Get
			End Property
		End Structure
	End Class
End Namespace
