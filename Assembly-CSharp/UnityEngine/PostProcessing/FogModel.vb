Imports System

Namespace UnityEngine.PostProcessing
	' Token: 0x02000BE5 RID: 3045
	<Serializable()>
	Public Class FogModel
		Inherits PostProcessingModel

		' Token: 0x1700067E RID: 1662
		' (get) Token: 0x06004902 RID: 18690 RVA: 0x0026419E File Offset: 0x0026259E
		' (set) Token: 0x06004903 RID: 18691 RVA: 0x002641A6 File Offset: 0x002625A6
		Public Property settings As FogModel.Settings
			Get
				Return Me.m_Settings
			End Get
			Set(value As FogModel.Settings)
				Me.m_Settings = value
			End Set
		End Property

		' Token: 0x06004904 RID: 18692 RVA: 0x002641AF File Offset: 0x002625AF
		Public Overrides Sub Reset()
			Me.m_Settings = FogModel.Settings.defaultSettings
		End Sub

		' Token: 0x04004F0E RID: 20238
		<SerializeField()>
		Private m_Settings As FogModel.Settings = FogModel.Settings.defaultSettings

		' Token: 0x02000BE6 RID: 3046
		<Serializable()>
		Public Structure Settings
			' Token: 0x1700067F RID: 1663
			' (get) Token: 0x06004905 RID: 18693 RVA: 0x002641BC File Offset: 0x002625BC
			Public Shared ReadOnly Property defaultSettings As FogModel.Settings
				Get
					Return New FogModel.Settings() With { .excludeSkybox = True }
				End Get
			End Property

			' Token: 0x04004F0F RID: 20239
			<Tooltip("Should the fog affect the skybox?")>
			Public excludeSkybox As Boolean
		End Structure
	End Class
End Namespace
