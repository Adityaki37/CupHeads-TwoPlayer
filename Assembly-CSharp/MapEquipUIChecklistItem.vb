Imports System
Imports TMPro
Imports UnityEngine
Imports UnityEngine.UI

' Token: 0x02000996 RID: 2454
Public Class MapEquipUIChecklistItem
	Inherits AbstractMonoBehaviour

	' Token: 0x170004A8 RID: 1192
	' (get) Token: 0x06003963 RID: 14691 RVA: 0x00209A18 File Offset: 0x00207E18
	Private ReadOnly Property lineWidth As Single
		Get
			Return Me.descriptionText.rectTransform.sizeDelta.x
		End Get
	End Property

	' Token: 0x06003964 RID: 14692 RVA: 0x00209A3D File Offset: 0x00207E3D
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.originalFontSize = Me.descriptionText.fontSize
	End Sub

	' Token: 0x06003965 RID: 14693 RVA: 0x00209A58 File Offset: 0x00207E58
	Public Function EnableCheckbox(enabled As Boolean) As Boolean
		If Me.checkBox IsNot Nothing Then
			Me.checkBox.enabled = enabled
			Return enabled
		End If
		Return False
	End Function

	' Token: 0x06003966 RID: 14694 RVA: 0x00209A88 File Offset: 0x00207E88
	Public Sub SetDescription(selectedLevel As Levels, levelName As String, isFinale As Boolean)
		Dim levelData As PlayerData.PlayerLevelDataObject = PlayerData.Data.GetLevelData(selectedLevel)
		Dim translation As Localization.Translation = Localization.Translate(selectedLevel.ToString())
		Dim text As String = If((Localization.language <> Localization.Languages.Japanese), " ", String.Empty)
		levelName = translation.text.Replace("\n", text)
		If levelData.played Then
			Me.descriptionText.text = levelName
			Me.descriptionText.font = Localization.Instance.fonts(CInt(Localization.language))(15).fontAsset
		Else
			Me.descriptionText.text = Me.unknown
			Me.descriptionText.font = Localization.Instance.fonts(0)(15).fontAsset
		End If
		If Me.isDicePalaceMiniBoss Then
			Me.descriptionText.fontSize = If((translation.fonts.fontSize <= 0), Me.originalFontSize, CSng(translation.fonts.fontSize))
		End If
		If Not Me.isDicePalaceMiniBoss Then
			Dim num As Single = Me.originalFontSize
			While Me.lineWidth - Me.descriptionText.preferredWidth < 0F AndAlso Me.originalFontSize > 0F
				num -= 1F
				Me.descriptionText.fontSize = num
			End While
			Me.SetLeaderDots(levelName, isFinale)
		End If
		If levelData.played Then
			If Not Me.isDicePalaceMiniBoss AndAlso levelData.completed Then
				Me.gradeText.text = Me.grades(CInt(levelData.grade))
				Me.timeText.text = Me.SecondsToMinutes(levelData.bestTime)
				If levelData.difficultyBeaten = Level.Mode.Normal AndAlso Me.checkBox IsNot Nothing AndAlso Me.checkBox.enabled Then
					Me.checkMark.enabled = True
					Me.checkMarkHard.enabled = False
				End If
				If levelData.difficultyBeaten = Level.Mode.Hard AndAlso Me.checkBox IsNot Nothing AndAlso Me.checkBox.enabled Then
					Me.checkMark.enabled = False
					Me.checkMarkHard.enabled = True
				End If
			End If
		Else
			Me.ClearDescription(isFinale)
		End If
	End Sub

	' Token: 0x06003967 RID: 14695 RVA: 0x00209CE8 File Offset: 0x002080E8
	Private Function SecondsToMinutes(seconds As Single) As String
		If seconds = 3.4028235E+38F Then
			Return "6:66"
		End If
		Dim num As Integer = CInt(seconds) / 60
		Dim num2 As Integer = CInt(seconds) Mod 60
		Return String.Format("{0}:{1:00}", num, num2)
	End Function

	' Token: 0x06003968 RID: 14696 RVA: 0x00209D2C File Offset: 0x0020812C
	Public Sub ClearDescription(isFinale As Boolean)
		If Not Me.isDicePalaceMiniBoss Then
			Me.gradeText.text = "?"
			Me.timeText.text = "?"
			Me.descriptionText.text = Me.unknown
			If isFinale Then
				Me.SetLeaderDots(Me.unknown, isFinale)
			Else
				Me.SetLeaderDots(Me.unknown, isFinale)
			End If
		Else
			Me.descriptionText.text = Me.unknown
		End If
		If Me.checkMark IsNot Nothing Then
			Me.checkMark.enabled = False
		End If
		If Me.checkMarkHard IsNot Nothing Then
			Me.checkMarkHard.enabled = False
		End If
	End Sub

	' Token: 0x06003969 RID: 14697 RVA: 0x00209DEC File Offset: 0x002081EC
	Private Sub SetLeaderDots(name As String, isFinale As Boolean)
		Me.leaderDotText.text = Me.dots
		Dim num As Single = Me.lineWidth - Me.descriptionText.preferredWidth - Me.dotsPadding
		If num < 0F Then
			Me.leaderDotText.text = String.Empty
			Return
		End If
		Dim num2 As Integer = 100000
		While Me.leaderDotText.text.Length > 2 AndAlso Me.leaderDotText.preferredWidth > num AndAlso num2 > 0
			num2 -= 1
			Me.leaderDotText.text = Me.leaderDotText.text.Substring(0, Me.leaderDotText.text.Length - 2)
		End While
	End Sub

	' Token: 0x04004109 RID: 16649
	<Header("Text")>
	Public descriptionText As TextMeshProUGUI

	' Token: 0x0400410A RID: 16650
	Public leaderDotText As Text

	' Token: 0x0400410B RID: 16651
	Public gradeText As Text

	' Token: 0x0400410C RID: 16652
	Public timeText As Text

	' Token: 0x0400410D RID: 16653
	<Header("Images")>
	Public checkBox As Image

	' Token: 0x0400410E RID: 16654
	Public checkMark As Image

	' Token: 0x0400410F RID: 16655
	Public checkMarkHard As Image

	' Token: 0x04004110 RID: 16656
	Private grades As String() = New String() { "D-", "D", "D+", "C-", "C", "C+", "B-", "B", "B+", "A-", "A", "A+", "S", "P" }

	' Token: 0x04004111 RID: 16657
	Private unknown As String = "?????"

	' Token: 0x04004112 RID: 16658
	Private dots As String = ". . . . . . . . . . . . . . . . . . . . . . ."

	' Token: 0x04004113 RID: 16659
	Public isDicePalaceMiniBoss As Boolean

	' Token: 0x04004114 RID: 16660
	Private dotsPadding As Single = 5F

	' Token: 0x04004115 RID: 16661
	Private originalFontSize As Single
End Class
