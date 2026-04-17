Imports System

Namespace UnityEngine.PostProcessing
	' Token: 0x02000BCB RID: 3019
	<Serializable()>
	Public Class BuiltinDebugViewsModel
		Inherits PostProcessingModel

		' Token: 0x17000666 RID: 1638
		' (get) Token: 0x060048D4 RID: 18644 RVA: 0x00263935 File Offset: 0x00261D35
		' (set) Token: 0x060048D5 RID: 18645 RVA: 0x0026393D File Offset: 0x00261D3D
		Public Property settings As BuiltinDebugViewsModel.Settings
			Get
				Return Me.m_Settings
			End Get
			Set(value As BuiltinDebugViewsModel.Settings)
				Me.m_Settings = value
			End Set
		End Property

		' Token: 0x17000667 RID: 1639
		' (get) Token: 0x060048D6 RID: 18646 RVA: 0x00263946 File Offset: 0x00261D46
		Public ReadOnly Property willInterrupt As Boolean
			Get
				Return Not Me.IsModeActive(BuiltinDebugViewsModel.Mode.None) AndAlso Not Me.IsModeActive(BuiltinDebugViewsModel.Mode.EyeAdaptation) AndAlso Not Me.IsModeActive(BuiltinDebugViewsModel.Mode.PreGradingLog) AndAlso Not Me.IsModeActive(BuiltinDebugViewsModel.Mode.LogLut) AndAlso Not Me.IsModeActive(BuiltinDebugViewsModel.Mode.UserLut)
			End Get
		End Property

		' Token: 0x060048D7 RID: 18647 RVA: 0x00263986 File Offset: 0x00261D86
		Public Overrides Sub Reset()
			Me.settings = BuiltinDebugViewsModel.Settings.defaultSettings
		End Sub

		' Token: 0x060048D8 RID: 18648 RVA: 0x00263993 File Offset: 0x00261D93
		Public Function IsModeActive(mode As BuiltinDebugViewsModel.Mode) As Boolean
			Return Me.m_Settings.mode = mode
		End Function

		' Token: 0x04004EA4 RID: 20132
		<SerializeField()>
		Private m_Settings As BuiltinDebugViewsModel.Settings = BuiltinDebugViewsModel.Settings.defaultSettings

		' Token: 0x02000BCC RID: 3020
		<Serializable()>
		Public Structure DepthSettings
			' Token: 0x17000668 RID: 1640
			' (get) Token: 0x060048D9 RID: 18649 RVA: 0x002639A4 File Offset: 0x00261DA4
			Public Shared ReadOnly Property defaultSettings As BuiltinDebugViewsModel.DepthSettings
				Get
					Return New BuiltinDebugViewsModel.DepthSettings() With { .scale = 1F }
				End Get
			End Property

			' Token: 0x04004EA5 RID: 20133
			<Range(0F, 1F)>
			<Tooltip("Scales the camera far plane before displaying the depth map.")>
			Public scale As Single
		End Structure

		' Token: 0x02000BCD RID: 3021
		<Serializable()>
		Public Structure MotionVectorsSettings
			' Token: 0x17000669 RID: 1641
			' (get) Token: 0x060048DA RID: 18650 RVA: 0x002639C8 File Offset: 0x00261DC8
			Public Shared ReadOnly Property defaultSettings As BuiltinDebugViewsModel.MotionVectorsSettings
				Get
					Return New BuiltinDebugViewsModel.MotionVectorsSettings() With { .sourceOpacity = 1F, .motionImageOpacity = 0F, .motionImageAmplitude = 16F, .motionVectorsOpacity = 1F, .motionVectorsResolution = 24, .motionVectorsAmplitude = 64F }
				End Get
			End Property

			' Token: 0x04004EA6 RID: 20134
			<Range(0F, 1F)>
			<Tooltip("Opacity of the source render.")>
			Public sourceOpacity As Single

			' Token: 0x04004EA7 RID: 20135
			<Range(0F, 1F)>
			<Tooltip("Opacity of the per-pixel motion vector colors.")>
			Public motionImageOpacity As Single

			' Token: 0x04004EA8 RID: 20136
			<Min(0F)>
			<Tooltip("Because motion vectors are mainly very small vectors, you can use this setting to make them more visible.")>
			Public motionImageAmplitude As Single

			' Token: 0x04004EA9 RID: 20137
			<Range(0F, 1F)>
			<Tooltip("Opacity for the motion vector arrows.")>
			Public motionVectorsOpacity As Single

			' Token: 0x04004EAA RID: 20138
			<Range(8F, 64F)>
			<Tooltip("The arrow density on screen.")>
			Public motionVectorsResolution As Integer

			' Token: 0x04004EAB RID: 20139
			<Min(0F)>
			<Tooltip("Tweaks the arrows length.")>
			Public motionVectorsAmplitude As Single
		End Structure

		' Token: 0x02000BCE RID: 3022
		Public Enum Mode
			' Token: 0x04004EAD RID: 20141
			None
			' Token: 0x04004EAE RID: 20142
			Depth
			' Token: 0x04004EAF RID: 20143
			Normals
			' Token: 0x04004EB0 RID: 20144
			MotionVectors
			' Token: 0x04004EB1 RID: 20145
			AmbientOcclusion
			' Token: 0x04004EB2 RID: 20146
			EyeAdaptation
			' Token: 0x04004EB3 RID: 20147
			FocusPlane
			' Token: 0x04004EB4 RID: 20148
			PreGradingLog
			' Token: 0x04004EB5 RID: 20149
			LogLut
			' Token: 0x04004EB6 RID: 20150
			UserLut
		End Enum

		' Token: 0x02000BCF RID: 3023
		<Serializable()>
		Public Structure Settings
			' Token: 0x1700066A RID: 1642
			' (get) Token: 0x060048DB RID: 18651 RVA: 0x00263A24 File Offset: 0x00261E24
			Public Shared ReadOnly Property defaultSettings As BuiltinDebugViewsModel.Settings
				Get
					Return New BuiltinDebugViewsModel.Settings() With { .mode = BuiltinDebugViewsModel.Mode.None, .depth = BuiltinDebugViewsModel.DepthSettings.defaultSettings, .motionVectors = BuiltinDebugViewsModel.MotionVectorsSettings.defaultSettings }
				End Get
			End Property

			' Token: 0x04004EB7 RID: 20151
			Public mode As BuiltinDebugViewsModel.Mode

			' Token: 0x04004EB8 RID: 20152
			Public depth As BuiltinDebugViewsModel.DepthSettings

			' Token: 0x04004EB9 RID: 20153
			Public motionVectors As BuiltinDebugViewsModel.MotionVectorsSettings
		End Structure
	End Class
End Namespace
