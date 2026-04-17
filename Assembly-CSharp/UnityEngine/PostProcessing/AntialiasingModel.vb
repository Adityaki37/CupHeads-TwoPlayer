Imports System

Namespace UnityEngine.PostProcessing
	' Token: 0x02000BBF RID: 3007
	<Serializable()>
	Public Class AntialiasingModel
		Inherits PostProcessingModel

		' Token: 0x1700065D RID: 1629
		' (get) Token: 0x060048C2 RID: 18626 RVA: 0x002634C2 File Offset: 0x002618C2
		' (set) Token: 0x060048C3 RID: 18627 RVA: 0x002634CA File Offset: 0x002618CA
		Public Property settings As AntialiasingModel.Settings
			Get
				Return Me.m_Settings
			End Get
			Set(value As AntialiasingModel.Settings)
				Me.m_Settings = value
			End Set
		End Property

		' Token: 0x060048C4 RID: 18628 RVA: 0x002634D3 File Offset: 0x002618D3
		Public Overrides Sub Reset()
			Me.m_Settings = AntialiasingModel.Settings.defaultSettings
		End Sub

		' Token: 0x04004E7F RID: 20095
		<SerializeField()>
		Private m_Settings As AntialiasingModel.Settings = AntialiasingModel.Settings.defaultSettings

		' Token: 0x02000BC0 RID: 3008
		Public Enum Method
			' Token: 0x04004E81 RID: 20097
			Fxaa
			' Token: 0x04004E82 RID: 20098
			Taa
		End Enum

		' Token: 0x02000BC1 RID: 3009
		Public Enum FxaaPreset
			' Token: 0x04004E84 RID: 20100
			ExtremePerformance
			' Token: 0x04004E85 RID: 20101
			Performance
			' Token: 0x04004E86 RID: 20102
			[Default]
			' Token: 0x04004E87 RID: 20103
			Quality
			' Token: 0x04004E88 RID: 20104
			ExtremeQuality
		End Enum

		' Token: 0x02000BC2 RID: 3010
		<Serializable()>
		Public Structure FxaaQualitySettings
			' Token: 0x04004E89 RID: 20105
			<Tooltip("The amount of desired sub-pixel aliasing removal. Effects the sharpeness of the output.")>
			<Range(0F, 1F)>
			Public subpixelAliasingRemovalAmount As Single

			' Token: 0x04004E8A RID: 20106
			<Tooltip("The minimum amount of local contrast required to qualify a region as containing an edge.")>
			<Range(0.063F, 0.333F)>
			Public edgeDetectionThreshold As Single

			' Token: 0x04004E8B RID: 20107
			<Tooltip("Local contrast adaptation value to disallow the algorithm from executing on the darker regions.")>
			<Range(0F, 0.0833F)>
			Public minimumRequiredLuminance As Single

			' Token: 0x04004E8C RID: 20108
			Public Shared presets As AntialiasingModel.FxaaQualitySettings() = New AntialiasingModel.FxaaQualitySettings() { New AntialiasingModel.FxaaQualitySettings() With { .subpixelAliasingRemovalAmount = 0F, .edgeDetectionThreshold = 0.333F, .minimumRequiredLuminance = 0.0833F }, New AntialiasingModel.FxaaQualitySettings() With { .subpixelAliasingRemovalAmount = 0.25F, .edgeDetectionThreshold = 0.25F, .minimumRequiredLuminance = 0.0833F }, New AntialiasingModel.FxaaQualitySettings() With { .subpixelAliasingRemovalAmount = 0.75F, .edgeDetectionThreshold = 0.166F, .minimumRequiredLuminance = 0.0833F }, New AntialiasingModel.FxaaQualitySettings() With { .subpixelAliasingRemovalAmount = 1F, .edgeDetectionThreshold = 0.125F, .minimumRequiredLuminance = 0.0625F }, New AntialiasingModel.FxaaQualitySettings() With { .subpixelAliasingRemovalAmount = 1F, .edgeDetectionThreshold = 0.063F, .minimumRequiredLuminance = 0.0312F } }
		End Structure

		' Token: 0x02000BC3 RID: 3011
		<Serializable()>
		Public Structure FxaaConsoleSettings
			' Token: 0x04004E8D RID: 20109
			<Tooltip("The amount of spread applied to the sampling coordinates while sampling for subpixel information.")>
			<Range(0.33F, 0.5F)>
			Public subpixelSpreadAmount As Single

			' Token: 0x04004E8E RID: 20110
			<Tooltip("This value dictates how sharp the edges in the image are kept; a higher value implies sharper edges.")>
			<Range(2F, 8F)>
			Public edgeSharpnessAmount As Single

			' Token: 0x04004E8F RID: 20111
			<Tooltip("The minimum amount of local contrast required to qualify a region as containing an edge.")>
			<Range(0.125F, 0.25F)>
			Public edgeDetectionThreshold As Single

			' Token: 0x04004E90 RID: 20112
			<Tooltip("Local contrast adaptation value to disallow the algorithm from executing on the darker regions.")>
			<Range(0.04F, 0.06F)>
			Public minimumRequiredLuminance As Single

			' Token: 0x04004E91 RID: 20113
			Public Shared presets As AntialiasingModel.FxaaConsoleSettings() = New AntialiasingModel.FxaaConsoleSettings() { New AntialiasingModel.FxaaConsoleSettings() With { .subpixelSpreadAmount = 0.33F, .edgeSharpnessAmount = 8F, .edgeDetectionThreshold = 0.25F, .minimumRequiredLuminance = 0.06F }, New AntialiasingModel.FxaaConsoleSettings() With { .subpixelSpreadAmount = 0.33F, .edgeSharpnessAmount = 8F, .edgeDetectionThreshold = 0.125F, .minimumRequiredLuminance = 0.06F }, New AntialiasingModel.FxaaConsoleSettings() With { .subpixelSpreadAmount = 0.5F, .edgeSharpnessAmount = 8F, .edgeDetectionThreshold = 0.125F, .minimumRequiredLuminance = 0.05F }, New AntialiasingModel.FxaaConsoleSettings() With { .subpixelSpreadAmount = 0.5F, .edgeSharpnessAmount = 4F, .edgeDetectionThreshold = 0.125F, .minimumRequiredLuminance = 0.04F }, New AntialiasingModel.FxaaConsoleSettings() With { .subpixelSpreadAmount = 0.5F, .edgeSharpnessAmount = 2F, .edgeDetectionThreshold = 0.125F, .minimumRequiredLuminance = 0.04F } }
		End Structure

		' Token: 0x02000BC4 RID: 3012
		<Serializable()>
		Public Structure FxaaSettings
			' Token: 0x1700065E RID: 1630
			' (get) Token: 0x060048C7 RID: 18631 RVA: 0x0026378C File Offset: 0x00261B8C
			Public Shared ReadOnly Property defaultSettings As AntialiasingModel.FxaaSettings
				Get
					Return New AntialiasingModel.FxaaSettings() With { .preset = AntialiasingModel.FxaaPreset.[Default] }
				End Get
			End Property

			' Token: 0x04004E92 RID: 20114
			Public preset As AntialiasingModel.FxaaPreset
		End Structure

		' Token: 0x02000BC5 RID: 3013
		<Serializable()>
		Public Structure TaaSettings
			' Token: 0x1700065F RID: 1631
			' (get) Token: 0x060048C8 RID: 18632 RVA: 0x002637AC File Offset: 0x00261BAC
			Public Shared ReadOnly Property defaultSettings As AntialiasingModel.TaaSettings
				Get
					Return New AntialiasingModel.TaaSettings() With { .jitterSpread = 0.75F, .sharpen = 0.3F, .stationaryBlending = 0.95F, .motionBlending = 0.85F }
				End Get
			End Property

			' Token: 0x04004E93 RID: 20115
			<Tooltip("The diameter (in texels) inside which jitter samples are spread. Smaller values result in crisper but more aliased output, while larger values result in more stable but blurrier output.")>
			<Range(0.1F, 1F)>
			Public jitterSpread As Single

			' Token: 0x04004E94 RID: 20116
			<Tooltip("Controls the amount of sharpening applied to the color buffer.")>
			<Range(0F, 3F)>
			Public sharpen As Single

			' Token: 0x04004E95 RID: 20117
			<Tooltip("The blend coefficient for a stationary fragment. Controls the percentage of history sample blended into the final color.")>
			<Range(0F, 0.99F)>
			Public stationaryBlending As Single

			' Token: 0x04004E96 RID: 20118
			<Tooltip("The blend coefficient for a fragment with significant motion. Controls the percentage of history sample blended into the final color.")>
			<Range(0F, 0.99F)>
			Public motionBlending As Single
		End Structure

		' Token: 0x02000BC6 RID: 3014
		<Serializable()>
		Public Structure Settings
			' Token: 0x17000660 RID: 1632
			' (get) Token: 0x060048C9 RID: 18633 RVA: 0x002637F4 File Offset: 0x00261BF4
			Public Shared ReadOnly Property defaultSettings As AntialiasingModel.Settings
				Get
					Return New AntialiasingModel.Settings() With { .method = AntialiasingModel.Method.Fxaa, .fxaaSettings = AntialiasingModel.FxaaSettings.defaultSettings, .taaSettings = AntialiasingModel.TaaSettings.defaultSettings }
				End Get
			End Property

			' Token: 0x04004E97 RID: 20119
			Public method As AntialiasingModel.Method

			' Token: 0x04004E98 RID: 20120
			Public fxaaSettings As AntialiasingModel.FxaaSettings

			' Token: 0x04004E99 RID: 20121
			Public taaSettings As AntialiasingModel.TaaSettings
		End Structure
	End Class
End Namespace
