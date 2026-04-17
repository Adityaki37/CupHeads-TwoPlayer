Imports System

Namespace UnityEngine.PostProcessing
	' Token: 0x02000BC7 RID: 3015
	<Serializable()>
	Public Class BloomModel
		Inherits PostProcessingModel

		' Token: 0x17000661 RID: 1633
		' (get) Token: 0x060048CB RID: 18635 RVA: 0x0026383D File Offset: 0x00261C3D
		' (set) Token: 0x060048CC RID: 18636 RVA: 0x00263845 File Offset: 0x00261C45
		Public Property settings As BloomModel.Settings
			Get
				Return Me.m_Settings
			End Get
			Set(value As BloomModel.Settings)
				Me.m_Settings = value
			End Set
		End Property

		' Token: 0x060048CD RID: 18637 RVA: 0x0026384E File Offset: 0x00261C4E
		Public Overrides Sub Reset()
			Me.m_Settings = BloomModel.Settings.defaultSettings
		End Sub

		' Token: 0x04004E9A RID: 20122
		<SerializeField()>
		Private m_Settings As BloomModel.Settings = BloomModel.Settings.defaultSettings

		' Token: 0x02000BC8 RID: 3016
		<Serializable()>
		Public Structure BloomSettings
			' Token: 0x17000662 RID: 1634
			' (get) Token: 0x060048CF RID: 18639 RVA: 0x00263869 File Offset: 0x00261C69
			' (set) Token: 0x060048CE RID: 18638 RVA: 0x0026385B File Offset: 0x00261C5B
			Public Property thresholdLinear As Single
				Get
					Return Mathf.GammaToLinearSpace(Me.threshold)
				End Get
				Set(value As Single)
					Me.threshold = Mathf.LinearToGammaSpace(value)
				End Set
			End Property

			' Token: 0x17000663 RID: 1635
			' (get) Token: 0x060048D0 RID: 18640 RVA: 0x00263878 File Offset: 0x00261C78
			Public Shared ReadOnly Property defaultSettings As BloomModel.BloomSettings
				Get
					Return New BloomModel.BloomSettings() With { .intensity = 0.5F, .threshold = 1.1F, .softKnee = 0.5F, .radius = 4F, .antiFlicker = False }
				End Get
			End Property

			' Token: 0x04004E9B RID: 20123
			<Min(0F)>
			<Tooltip("Strength of the bloom filter.")>
			Public intensity As Single

			' Token: 0x04004E9C RID: 20124
			<Min(0F)>
			<Tooltip("Filters out pixels under this level of brightness.")>
			Public threshold As Single

			' Token: 0x04004E9D RID: 20125
			<Range(0F, 1F)>
			<Tooltip("Makes transition between under/over-threshold gradual (0 = hard threshold, 1 = soft threshold).")>
			Public softKnee As Single

			' Token: 0x04004E9E RID: 20126
			<Range(1F, 7F)>
			<Tooltip("Changes extent of veiling effects in a screen resolution-independent fashion.")>
			Public radius As Single

			' Token: 0x04004E9F RID: 20127
			<Tooltip("Reduces flashing noise with an additional filter.")>
			Public antiFlicker As Boolean
		End Structure

		' Token: 0x02000BC9 RID: 3017
		<Serializable()>
		Public Structure LensDirtSettings
			' Token: 0x17000664 RID: 1636
			' (get) Token: 0x060048D1 RID: 18641 RVA: 0x002638C8 File Offset: 0x00261CC8
			Public Shared ReadOnly Property defaultSettings As BloomModel.LensDirtSettings
				Get
					Return New BloomModel.LensDirtSettings() With { .texture = Nothing, .intensity = 3F }
				End Get
			End Property

			' Token: 0x04004EA0 RID: 20128
			<Tooltip("Dirtiness texture to add smudges or dust to the lens.")>
			Public texture As Texture

			' Token: 0x04004EA1 RID: 20129
			<Min(0F)>
			<Tooltip("Amount of lens dirtiness.")>
			Public intensity As Single
		End Structure

		' Token: 0x02000BCA RID: 3018
		<Serializable()>
		Public Structure Settings
			' Token: 0x17000665 RID: 1637
			' (get) Token: 0x060048D2 RID: 18642 RVA: 0x002638F4 File Offset: 0x00261CF4
			Public Shared ReadOnly Property defaultSettings As BloomModel.Settings
				Get
					Return New BloomModel.Settings() With { .bloom = BloomModel.BloomSettings.defaultSettings, .lensDirt = BloomModel.LensDirtSettings.defaultSettings }
				End Get
			End Property

			' Token: 0x04004EA2 RID: 20130
			Public bloom As BloomModel.BloomSettings

			' Token: 0x04004EA3 RID: 20131
			Public lensDirt As BloomModel.LensDirtSettings
		End Structure
	End Class
End Namespace
