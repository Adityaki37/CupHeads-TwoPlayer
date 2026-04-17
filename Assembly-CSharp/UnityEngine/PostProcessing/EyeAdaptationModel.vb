Imports System

Namespace UnityEngine.PostProcessing
	' Token: 0x02000BE2 RID: 3042
	<Serializable()>
	Public Class EyeAdaptationModel
		Inherits PostProcessingModel

		' Token: 0x1700067C RID: 1660
		' (get) Token: 0x060048FD RID: 18685 RVA: 0x002640E1 File Offset: 0x002624E1
		' (set) Token: 0x060048FE RID: 18686 RVA: 0x002640E9 File Offset: 0x002624E9
		Public Property settings As EyeAdaptationModel.Settings
			Get
				Return Me.m_Settings
			End Get
			Set(value As EyeAdaptationModel.Settings)
				Me.m_Settings = value
			End Set
		End Property

		' Token: 0x060048FF RID: 18687 RVA: 0x002640F2 File Offset: 0x002624F2
		Public Overrides Sub Reset()
			Me.m_Settings = EyeAdaptationModel.Settings.defaultSettings
		End Sub

		' Token: 0x04004EFF RID: 20223
		<SerializeField()>
		Private m_Settings As EyeAdaptationModel.Settings = EyeAdaptationModel.Settings.defaultSettings

		' Token: 0x02000BE3 RID: 3043
		Public Enum EyeAdaptationType
			' Token: 0x04004F01 RID: 20225
			Progressive
			' Token: 0x04004F02 RID: 20226
			Fixed
		End Enum

		' Token: 0x02000BE4 RID: 3044
		<Serializable()>
		Public Structure Settings
			' Token: 0x1700067D RID: 1661
			' (get) Token: 0x06004900 RID: 18688 RVA: 0x00264100 File Offset: 0x00262500
			Public Shared ReadOnly Property defaultSettings As EyeAdaptationModel.Settings
				Get
					Return New EyeAdaptationModel.Settings() With { .lowPercent = 45F, .highPercent = 95F, .minLuminance = -5F, .maxLuminance = 1F, .keyValue = 0.25F, .dynamicKeyValue = True, .adaptationType = EyeAdaptationModel.EyeAdaptationType.Progressive, .speedUp = 2F, .speedDown = 1F, .logMin = -8, .logMax = 4 }
				End Get
			End Property

			' Token: 0x04004F03 RID: 20227
			<Range(1F, 99F)>
			<Tooltip("Filters the dark part of the histogram when computing the average luminance to avoid very dark pixels from contributing to the auto exposure. Unit is in percent.")>
			Public lowPercent As Single

			' Token: 0x04004F04 RID: 20228
			<Range(1F, 99F)>
			<Tooltip("Filters the bright part of the histogram when computing the average luminance to avoid very dark pixels from contributing to the auto exposure. Unit is in percent.")>
			Public highPercent As Single

			' Token: 0x04004F05 RID: 20229
			<Tooltip("Minimum average luminance to consider for auto exposure (in EV).")>
			Public minLuminance As Single

			' Token: 0x04004F06 RID: 20230
			<Tooltip("Maximum average luminance to consider for auto exposure (in EV).")>
			Public maxLuminance As Single

			' Token: 0x04004F07 RID: 20231
			<Min(0F)>
			<Tooltip("Exposure bias. Use this to offset the global exposure of the scene.")>
			Public keyValue As Single

			' Token: 0x04004F08 RID: 20232
			<Tooltip("Set this to true to let Unity handle the key value automatically based on average luminance.")>
			Public dynamicKeyValue As Boolean

			' Token: 0x04004F09 RID: 20233
			<Tooltip("Use ""Progressive"" if you want the auto exposure to be animated. Use ""Fixed"" otherwise.")>
			Public adaptationType As EyeAdaptationModel.EyeAdaptationType

			' Token: 0x04004F0A RID: 20234
			<Min(0F)>
			<Tooltip("Adaptation speed from a dark to a light environment.")>
			Public speedUp As Single

			' Token: 0x04004F0B RID: 20235
			<Min(0F)>
			<Tooltip("Adaptation speed from a light to a dark environment.")>
			Public speedDown As Single

			' Token: 0x04004F0C RID: 20236
			<Range(-16F, -1F)>
			<Tooltip("Lower bound for the brightness range of the generated histogram (in EV). The bigger the spread between min & max, the lower the precision will be.")>
			Public logMin As Integer

			' Token: 0x04004F0D RID: 20237
			<Range(1F, 16F)>
			<Tooltip("Upper bound for the brightness range of the generated histogram (in EV). The bigger the spread between min & max, the lower the precision will be.")>
			Public logMax As Integer
		End Structure
	End Class
End Namespace
