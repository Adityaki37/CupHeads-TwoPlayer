Imports System

Namespace UnityEngine.PostProcessing
	' Token: 0x02000BF4 RID: 3060
	<Serializable()>
	Public Class VignetteModel
		Inherits PostProcessingModel

		' Token: 0x17000688 RID: 1672
		' (get) Token: 0x0600491B RID: 18715 RVA: 0x00264429 File Offset: 0x00262829
		' (set) Token: 0x0600491C RID: 18716 RVA: 0x00264431 File Offset: 0x00262831
		Public Property settings As VignetteModel.Settings
			Get
				Return Me.m_Settings
			End Get
			Set(value As VignetteModel.Settings)
				Me.m_Settings = value
			End Set
		End Property

		' Token: 0x0600491D RID: 18717 RVA: 0x0026443A File Offset: 0x0026283A
		Public Overrides Sub Reset()
			Me.m_Settings = VignetteModel.Settings.defaultSettings
		End Sub

		' Token: 0x04004F33 RID: 20275
		<SerializeField()>
		Private m_Settings As VignetteModel.Settings = VignetteModel.Settings.defaultSettings

		' Token: 0x02000BF5 RID: 3061
		Public Enum Mode
			' Token: 0x04004F35 RID: 20277
			Classic
			' Token: 0x04004F36 RID: 20278
			Masked
		End Enum

		' Token: 0x02000BF6 RID: 3062
		<Serializable()>
		Public Structure Settings
			' Token: 0x17000689 RID: 1673
			' (get) Token: 0x0600491E RID: 18718 RVA: 0x00264448 File Offset: 0x00262848
			Public Shared ReadOnly Property defaultSettings As VignetteModel.Settings
				Get
					Return New VignetteModel.Settings() With { .mode = VignetteModel.Mode.Classic, .color = New Color(0F, 0F, 0F, 1F), .center = New Vector2(0.5F, 0.5F), .intensity = 0.45F, .smoothness = 0.2F, .roundness = 1F, .mask = Nothing, .opacity = 1F, .rounded = False }
				End Get
			End Property

			' Token: 0x04004F37 RID: 20279
			<Tooltip("Use the ""Classic"" mode for parametric controls. Use the ""Masked"" mode to use your own texture mask.")>
			Public mode As VignetteModel.Mode

			' Token: 0x04004F38 RID: 20280
			<ColorUsage(False)>
			<Tooltip("Vignette color. Use the alpha channel for transparency.")>
			Public color As Color

			' Token: 0x04004F39 RID: 20281
			<Tooltip("Sets the vignette center point (screen center is [0.5,0.5]).")>
			Public center As Vector2

			' Token: 0x04004F3A RID: 20282
			<Range(0F, 1F)>
			<Tooltip("Amount of vignetting on screen.")>
			Public intensity As Single

			' Token: 0x04004F3B RID: 20283
			<Range(0.01F, 1F)>
			<Tooltip("Smoothness of the vignette borders.")>
			Public smoothness As Single

			' Token: 0x04004F3C RID: 20284
			<Range(0F, 1F)>
			<Tooltip("Lower values will make a square-ish vignette.")>
			Public roundness As Single

			' Token: 0x04004F3D RID: 20285
			<Tooltip("A black and white mask to use as a vignette.")>
			Public mask As Texture

			' Token: 0x04004F3E RID: 20286
			<Range(0F, 1F)>
			<Tooltip("Mask opacity.")>
			Public opacity As Single

			' Token: 0x04004F3F RID: 20287
			<Tooltip("Should the vignette be perfectly round or be dependent on the current aspect ratio?")>
			Public rounded As Boolean
		End Structure
	End Class
End Namespace
