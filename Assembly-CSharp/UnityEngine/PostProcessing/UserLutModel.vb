Imports System

Namespace UnityEngine.PostProcessing
	' Token: 0x02000BF2 RID: 3058
	<Serializable()>
	Public Class UserLutModel
		Inherits PostProcessingModel

		' Token: 0x17000686 RID: 1670
		' (get) Token: 0x06004916 RID: 18710 RVA: 0x002643CD File Offset: 0x002627CD
		' (set) Token: 0x06004917 RID: 18711 RVA: 0x002643D5 File Offset: 0x002627D5
		Public Property settings As UserLutModel.Settings
			Get
				Return Me.m_Settings
			End Get
			Set(value As UserLutModel.Settings)
				Me.m_Settings = value
			End Set
		End Property

		' Token: 0x06004918 RID: 18712 RVA: 0x002643DE File Offset: 0x002627DE
		Public Overrides Sub Reset()
			Me.m_Settings = UserLutModel.Settings.defaultSettings
		End Sub

		' Token: 0x04004F30 RID: 20272
		<SerializeField()>
		Private m_Settings As UserLutModel.Settings = UserLutModel.Settings.defaultSettings

		' Token: 0x02000BF3 RID: 3059
		<Serializable()>
		Public Structure Settings
			' Token: 0x17000687 RID: 1671
			' (get) Token: 0x06004919 RID: 18713 RVA: 0x002643EC File Offset: 0x002627EC
			Public Shared ReadOnly Property defaultSettings As UserLutModel.Settings
				Get
					Return New UserLutModel.Settings() With { .lut = Nothing, .contribution = 1F }
				End Get
			End Property

			' Token: 0x04004F31 RID: 20273
			<Tooltip("Custom lookup texture (strip format, e.g. 256x16).")>
			Public lut As Texture2D

			' Token: 0x04004F32 RID: 20274
			<Range(0F, 1F)>
			<Tooltip("Blending factor.")>
			Public contribution As Single
		End Structure
	End Class
End Namespace
