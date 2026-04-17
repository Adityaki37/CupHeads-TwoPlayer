Imports System

Namespace UnityEngine.PostProcessing
	' Token: 0x02000BD2 RID: 3026
	<Serializable()>
	Public Class ColorGradingModel
		Inherits PostProcessingModel

		' Token: 0x1700066D RID: 1645
		' (get) Token: 0x060048E2 RID: 18658 RVA: 0x00263AC9 File Offset: 0x00261EC9
		' (set) Token: 0x060048E3 RID: 18659 RVA: 0x00263AD1 File Offset: 0x00261ED1
		Public Property settings As ColorGradingModel.Settings
			Get
				Return Me.m_Settings
			End Get
			Set(value As ColorGradingModel.Settings)
				Me.m_Settings = value
				Me.OnValidate()
			End Set
		End Property

		' Token: 0x1700066E RID: 1646
		' (get) Token: 0x060048E4 RID: 18660 RVA: 0x00263AE0 File Offset: 0x00261EE0
		' (set) Token: 0x060048E5 RID: 18661 RVA: 0x00263AE8 File Offset: 0x00261EE8
		Public Property isDirty As Boolean

		' Token: 0x1700066F RID: 1647
		' (get) Token: 0x060048E6 RID: 18662 RVA: 0x00263AF1 File Offset: 0x00261EF1
		' (set) Token: 0x060048E7 RID: 18663 RVA: 0x00263AF9 File Offset: 0x00261EF9
		Public Property bakedLut As RenderTexture

		' Token: 0x060048E8 RID: 18664 RVA: 0x00263B02 File Offset: 0x00261F02
		Public Overrides Sub Reset()
			Me.m_Settings = ColorGradingModel.Settings.defaultSettings
			Me.OnValidate()
		End Sub

		' Token: 0x060048E9 RID: 18665 RVA: 0x00263B15 File Offset: 0x00261F15
		Public Overrides Sub OnValidate()
			Me.isDirty = True
		End Sub

		' Token: 0x04004EBD RID: 20157
		<SerializeField()>
		Private m_Settings As ColorGradingModel.Settings = ColorGradingModel.Settings.defaultSettings

		' Token: 0x02000BD3 RID: 3027
		Public Enum Tonemapper
			' Token: 0x04004EC1 RID: 20161
			None
			' Token: 0x04004EC2 RID: 20162
			ACES
			' Token: 0x04004EC3 RID: 20163
			Neutral
		End Enum

		' Token: 0x02000BD4 RID: 3028
		<Serializable()>
		Public Structure TonemappingSettings
			' Token: 0x17000670 RID: 1648
			' (get) Token: 0x060048EA RID: 18666 RVA: 0x00263B20 File Offset: 0x00261F20
			Public Shared ReadOnly Property defaultSettings As ColorGradingModel.TonemappingSettings
				Get
					Return New ColorGradingModel.TonemappingSettings() With { .tonemapper = ColorGradingModel.Tonemapper.Neutral, .neutralBlackIn = 0.02F, .neutralWhiteIn = 10F, .neutralBlackOut = 0F, .neutralWhiteOut = 10F, .neutralWhiteLevel = 5.3F, .neutralWhiteClip = 10F }
				End Get
			End Property

			' Token: 0x04004EC4 RID: 20164
			<Tooltip("Tonemapping algorithm to use at the end of the color grading process. Use ""Neutral"" if you need a customizable tonemapper or ""Filmic"" to give a standard filmic look to your scenes.")>
			Public tonemapper As ColorGradingModel.Tonemapper

			' Token: 0x04004EC5 RID: 20165
			<Range(-0.1F, 0.1F)>
			Public neutralBlackIn As Single

			' Token: 0x04004EC6 RID: 20166
			<Range(1F, 20F)>
			Public neutralWhiteIn As Single

			' Token: 0x04004EC7 RID: 20167
			<Range(-0.09F, 0.1F)>
			Public neutralBlackOut As Single

			' Token: 0x04004EC8 RID: 20168
			<Range(1F, 19F)>
			Public neutralWhiteOut As Single

			' Token: 0x04004EC9 RID: 20169
			<Range(0.1F, 20F)>
			Public neutralWhiteLevel As Single

			' Token: 0x04004ECA RID: 20170
			<Range(1F, 10F)>
			Public neutralWhiteClip As Single
		End Structure

		' Token: 0x02000BD5 RID: 3029
		<Serializable()>
		Public Structure BasicSettings
			' Token: 0x17000671 RID: 1649
			' (get) Token: 0x060048EB RID: 18667 RVA: 0x00263B88 File Offset: 0x00261F88
			Public Shared ReadOnly Property defaultSettings As ColorGradingModel.BasicSettings
				Get
					Return New ColorGradingModel.BasicSettings() With { .postExposure = 0F, .temperature = 0F, .tint = 0F, .hueShift = 0F, .saturation = 1F, .contrast = 1F }
				End Get
			End Property

			' Token: 0x04004ECB RID: 20171
			<Tooltip("Adjusts the overall exposure of the scene in EV units. This is applied after HDR effect and right before tonemapping so it won't affect previous effects in the chain.")>
			Public postExposure As Single

			' Token: 0x04004ECC RID: 20172
			<Range(-100F, 100F)>
			<Tooltip("Sets the white balance to a custom color temperature.")>
			Public temperature As Single

			' Token: 0x04004ECD RID: 20173
			<Range(-100F, 100F)>
			<Tooltip("Sets the white balance to compensate for a green or magenta tint.")>
			Public tint As Single

			' Token: 0x04004ECE RID: 20174
			<Range(-180F, 180F)>
			<Tooltip("Shift the hue of all colors.")>
			Public hueShift As Single

			' Token: 0x04004ECF RID: 20175
			<Range(0F, 2F)>
			<Tooltip("Pushes the intensity of all colors.")>
			Public saturation As Single

			' Token: 0x04004ED0 RID: 20176
			<Range(0F, 2F)>
			<Tooltip("Expands or shrinks the overall range of tonal values.")>
			Public contrast As Single
		End Structure

		' Token: 0x02000BD6 RID: 3030
		<Serializable()>
		Public Structure ChannelMixerSettings
			' Token: 0x17000672 RID: 1650
			' (get) Token: 0x060048EC RID: 18668 RVA: 0x00263BE8 File Offset: 0x00261FE8
			Public Shared ReadOnly Property defaultSettings As ColorGradingModel.ChannelMixerSettings
				Get
					Return New ColorGradingModel.ChannelMixerSettings() With { .red = New Vector3(1F, 0F, 0F), .green = New Vector3(0F, 1F, 0F), .blue = New Vector3(0F, 0F, 1F), .currentEditingChannel = 0 }
				End Get
			End Property

			' Token: 0x04004ED1 RID: 20177
			Public red As Vector3

			' Token: 0x04004ED2 RID: 20178
			Public green As Vector3

			' Token: 0x04004ED3 RID: 20179
			Public blue As Vector3

			' Token: 0x04004ED4 RID: 20180
			<HideInInspector()>
			Public currentEditingChannel As Integer
		End Structure

		' Token: 0x02000BD7 RID: 3031
		<Serializable()>
		Public Structure LogWheelsSettings
			' Token: 0x17000673 RID: 1651
			' (get) Token: 0x060048ED RID: 18669 RVA: 0x00263C58 File Offset: 0x00262058
			Public Shared ReadOnly Property defaultSettings As ColorGradingModel.LogWheelsSettings
				Get
					Return New ColorGradingModel.LogWheelsSettings() With { .slope = Color.clear, .power = Color.clear, .offset = Color.clear }
				End Get
			End Property

			' Token: 0x04004ED5 RID: 20181
			<Trackball("GetSlopeValue")>
			Public slope As Color

			' Token: 0x04004ED6 RID: 20182
			<Trackball("GetPowerValue")>
			Public power As Color

			' Token: 0x04004ED7 RID: 20183
			<Trackball("GetOffsetValue")>
			Public offset As Color
		End Structure

		' Token: 0x02000BD8 RID: 3032
		<Serializable()>
		Public Structure LinearWheelsSettings
			' Token: 0x17000674 RID: 1652
			' (get) Token: 0x060048EE RID: 18670 RVA: 0x00263C94 File Offset: 0x00262094
			Public Shared ReadOnly Property defaultSettings As ColorGradingModel.LinearWheelsSettings
				Get
					Return New ColorGradingModel.LinearWheelsSettings() With { .lift = Color.clear, .gamma = Color.clear, .gain = Color.clear }
				End Get
			End Property

			' Token: 0x04004ED8 RID: 20184
			<Trackball("GetLiftValue")>
			Public lift As Color

			' Token: 0x04004ED9 RID: 20185
			<Trackball("GetGammaValue")>
			Public gamma As Color

			' Token: 0x04004EDA RID: 20186
			<Trackball("GetGainValue")>
			Public gain As Color
		End Structure

		' Token: 0x02000BD9 RID: 3033
		Public Enum ColorWheelMode
			' Token: 0x04004EDC RID: 20188
			Linear
			' Token: 0x04004EDD RID: 20189
			Log
		End Enum

		' Token: 0x02000BDA RID: 3034
		<Serializable()>
		Public Structure ColorWheelsSettings
			' Token: 0x17000675 RID: 1653
			' (get) Token: 0x060048EF RID: 18671 RVA: 0x00263CD0 File Offset: 0x002620D0
			Public Shared ReadOnly Property defaultSettings As ColorGradingModel.ColorWheelsSettings
				Get
					Return New ColorGradingModel.ColorWheelsSettings() With { .mode = ColorGradingModel.ColorWheelMode.Log, .log = ColorGradingModel.LogWheelsSettings.defaultSettings, .linear = ColorGradingModel.LinearWheelsSettings.defaultSettings }
				End Get
			End Property

			' Token: 0x04004EDE RID: 20190
			Public mode As ColorGradingModel.ColorWheelMode

			' Token: 0x04004EDF RID: 20191
			<TrackballGroup()>
			Public log As ColorGradingModel.LogWheelsSettings

			' Token: 0x04004EE0 RID: 20192
			<TrackballGroup()>
			Public linear As ColorGradingModel.LinearWheelsSettings
		End Structure

		' Token: 0x02000BDB RID: 3035
		<Serializable()>
		Public Structure CurvesSettings
			' Token: 0x17000676 RID: 1654
			' (get) Token: 0x060048F0 RID: 18672 RVA: 0x00263D08 File Offset: 0x00262108
			Public Shared ReadOnly Property defaultSettings As ColorGradingModel.CurvesSettings
				Get
					Return New ColorGradingModel.CurvesSettings() With { .master = New ColorGradingCurve(New AnimationCurve(New Keyframe() { New Keyframe(0F, 0F, 1F, 1F), New Keyframe(1F, 1F, 1F, 1F) }), 0F, False, New Vector2(0F, 1F)), .red = New ColorGradingCurve(New AnimationCurve(New Keyframe() { New Keyframe(0F, 0F, 1F, 1F), New Keyframe(1F, 1F, 1F, 1F) }), 0F, False, New Vector2(0F, 1F)), .green = New ColorGradingCurve(New AnimationCurve(New Keyframe() { New Keyframe(0F, 0F, 1F, 1F), New Keyframe(1F, 1F, 1F, 1F) }), 0F, False, New Vector2(0F, 1F)), .blue = New ColorGradingCurve(New AnimationCurve(New Keyframe() { New Keyframe(0F, 0F, 1F, 1F), New Keyframe(1F, 1F, 1F, 1F) }), 0F, False, New Vector2(0F, 1F)), .hueVShue = New ColorGradingCurve(New AnimationCurve(), 0.5F, True, New Vector2(0F, 1F)), .hueVSsat = New ColorGradingCurve(New AnimationCurve(), 0.5F, True, New Vector2(0F, 1F)), .satVSsat = New ColorGradingCurve(New AnimationCurve(), 0.5F, False, New Vector2(0F, 1F)), .lumVSsat = New ColorGradingCurve(New AnimationCurve(), 0.5F, False, New Vector2(0F, 1F)), .e_CurrentEditingCurve = 0, .e_CurveY = True, .e_CurveR = False, .e_CurveG = False, .e_CurveB = False }
				End Get
			End Property

			' Token: 0x04004EE1 RID: 20193
			Public master As ColorGradingCurve

			' Token: 0x04004EE2 RID: 20194
			Public red As ColorGradingCurve

			' Token: 0x04004EE3 RID: 20195
			Public green As ColorGradingCurve

			' Token: 0x04004EE4 RID: 20196
			Public blue As ColorGradingCurve

			' Token: 0x04004EE5 RID: 20197
			Public hueVShue As ColorGradingCurve

			' Token: 0x04004EE6 RID: 20198
			Public hueVSsat As ColorGradingCurve

			' Token: 0x04004EE7 RID: 20199
			Public satVSsat As ColorGradingCurve

			' Token: 0x04004EE8 RID: 20200
			Public lumVSsat As ColorGradingCurve

			' Token: 0x04004EE9 RID: 20201
			<HideInInspector()>
			Public e_CurrentEditingCurve As Integer

			' Token: 0x04004EEA RID: 20202
			<HideInInspector()>
			Public e_CurveY As Boolean

			' Token: 0x04004EEB RID: 20203
			<HideInInspector()>
			Public e_CurveR As Boolean

			' Token: 0x04004EEC RID: 20204
			<HideInInspector()>
			Public e_CurveG As Boolean

			' Token: 0x04004EED RID: 20205
			<HideInInspector()>
			Public e_CurveB As Boolean
		End Structure

		' Token: 0x02000BDC RID: 3036
		<Serializable()>
		Public Structure Settings
			' Token: 0x17000677 RID: 1655
			' (get) Token: 0x060048F1 RID: 18673 RVA: 0x00263FB8 File Offset: 0x002623B8
			Public Shared ReadOnly Property defaultSettings As ColorGradingModel.Settings
				Get
					Return New ColorGradingModel.Settings() With { .tonemapping = ColorGradingModel.TonemappingSettings.defaultSettings, .basic = ColorGradingModel.BasicSettings.defaultSettings, .channelMixer = ColorGradingModel.ChannelMixerSettings.defaultSettings, .colorWheels = ColorGradingModel.ColorWheelsSettings.defaultSettings, .curves = ColorGradingModel.CurvesSettings.defaultSettings }
				End Get
			End Property

			' Token: 0x04004EEE RID: 20206
			Public tonemapping As ColorGradingModel.TonemappingSettings

			' Token: 0x04004EEF RID: 20207
			Public basic As ColorGradingModel.BasicSettings

			' Token: 0x04004EF0 RID: 20208
			Public channelMixer As ColorGradingModel.ChannelMixerSettings

			' Token: 0x04004EF1 RID: 20209
			Public colorWheels As ColorGradingModel.ColorWheelsSettings

			' Token: 0x04004EF2 RID: 20210
			Public curves As ColorGradingModel.CurvesSettings
		End Structure
	End Class
End Namespace
