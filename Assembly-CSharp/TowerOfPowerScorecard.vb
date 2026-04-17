Imports System
Imports System.Collections
Imports TMPro
Imports UnityEngine
Imports UnityEngine.PostProcessing
Imports UnityEngine.UI

' Token: 0x02000B31 RID: 2865
Public Class TowerOfPowerScorecard
	Inherits AbstractMonoBehaviour

	' Token: 0x06004568 RID: 17768 RVA: 0x002481E8 File Offset: 0x002465E8
	Protected Overrides Sub Awake()
		MyBase.Awake()
		If Localization.language <> Localization.Languages.English Then
			Me.DisableEnglishMDHRSubtitles()
		End If
		If Localization.language = Localization.Languages.Japanese Then
			Me.CenterResultTitles(Me.japaneseTitleRoot)
		ElseIf Localization.language = Localization.Languages.Korean Then
			Me.CenterResultTitles(Me.koreanTitleRoot)
		ElseIf Localization.language = Localization.Languages.SimplifiedChinese OrElse Localization.language = Localization.Languages.German OrElse Localization.language = Localization.Languages.SpanishSpain OrElse Localization.language = Localization.Languages.SpanishAmerica OrElse Localization.language = Localization.Languages.Russian OrElse Localization.language = Localization.Languages.PortugueseBrazil Then
			Me.CenterResultTitles(Me.chineseTitleRoot)
		End If
		Cuphead.Init(False)
		If PlayerManager.Multiplayer Then
			If Localization.language = Localization.Languages.Polish OrElse Localization.language = Localization.Languages.Italian OrElse Localization.language = Localization.Languages.French Then
				Me.CenterResultTitles(Me.japaneseTitleRoot)
			End If
		ElseIf Localization.language = Localization.Languages.Polish OrElse Localization.language = Localization.Languages.Italian OrElse Localization.language = Localization.Languages.French OrElse Localization.language = Localization.Languages.SimplifiedChinese OrElse Localization.language = Localization.Languages.Japanese Then
			Me.CenterResultTitles(Me.playerOneOffCenterTitleRoot)
		End If
		MyBase.StartCoroutine(Me.main_cr())
		Me.continuePrompt.SetActive(False)
		Me.input = New CupheadInput.AnyPlayerInput(False)
	End Sub

	' Token: 0x06004569 RID: 17769 RVA: 0x00248340 File Offset: 0x00246740
	Private Sub DisableEnglishMDHRSubtitles()
		For Each spriteRenderer As SpriteRenderer In Me.studioMHDRSubtitles
			spriteRenderer.enabled = False
		Next
	End Sub

	' Token: 0x0600456A RID: 17770 RVA: 0x00248374 File Offset: 0x00246774
	Private Sub CenterResultTitles(rootPosition As Vector3)
		For Each transform As Transform In Me.resultsTitles
			transform.localPosition = rootPosition
		Next
	End Sub

	' Token: 0x0600456B RID: 17771 RVA: 0x002483A8 File Offset: 0x002467A8
	Private Iterator Function main_cr() As IEnumerator
		Dim data As LevelScoringData = Level.ScoringData
		Me.done = False
		If Localization.language = Localization.Languages.Korean Then
			For Each textMeshProUGUI As TextMeshProUGUI In Me.scoring.GetComponentsInChildren(Of TextMeshProUGUI)()
				textMeshProUGUI.fontStyle = FontStyles.Bold
			Next
			Me.gradeLabel.fontStyle = FontStyles.Bold
		End If
		If Not Level.IsTowerOfPowerMain AndAlso data.difficulty = Level.Mode.Easy AndAlso Level.PreviousDifficulty = Level.Mode.Easy AndAlso Level.PreviousLevelType = Level.Type.Battle AndAlso Not Level.IsDicePalace AndAlso Not Level.IsDicePalaceMain AndAlso Level.PreviousLevel <> Levels.Devil Then
			Dim translation As Localization.Translation = Localization.Translate("ResultsTryRegular")
			Me.tryRegular.SetActive(True)
			If translation.image Is Nothing AndAlso Not translation.hasSpriteAtlasImage Then
				Me.tryRegular.GetComponent(Of SpriteRenderer)().enabled = False
				Me.tryRegularEnglishBackground.enabled = False
				Me.tryRegularText.text = translation.text
				Me.tryRegularText.font = translation.fonts.fontAsset
				Me.tryRegularText.fontSize = If((translation.fonts.fontAssetSize <> 0F), translation.fonts.fontAssetSize, Me.tryRegularText.fontSize)
				Me.tryRegularText.outlineWidth = If((Localization.language <> Localization.Languages.Korean), Me.tryRegularText.outlineWidth, 0.07F)
				Me.AlignBannerText(Me.tryRegularText.gameObject)
				If Localization.language = Localization.Languages.Korean OrElse Localization.language = Localization.Languages.Japanese Then
					Me.postProcessingScript.profile = Me.asianProfile
				End If
				Me.AlignBannerText(Me.glowingText)
				Me.glowScript.InitTMPText(New MaskableGraphic() { Me.tryRegularText })
				If Localization.language <> Localization.Languages.English Then
					Me.glowScript.BeginGlow()
				End If
			Else
				Me.tryRegularText.enabled = False
			End If
		End If
		Me.timeTicker.TargetValue = CInt(data.time)
		Me.timeTicker.MaxValue = CInt(data.goalTime)
		Me.hitsTicker.TargetValue = If((data.numTimesHit >= 3), 0, (3 - data.numTimesHit))
		Me.hitsTicker.MaxValue = 3
		Me.parriesTicker.TargetValue = Mathf.Min(data.numParries, CInt(Cuphead.Current.ScoringProperties.parriesForHighestGrade))
		Me.parriesTicker.MaxValue = CInt(Cuphead.Current.ScoringProperties.parriesForHighestGrade)
		Me.superMeterTicker.TargetValue = Mathf.Min(data.superMeterUsed, CInt(Cuphead.Current.ScoringProperties.superMeterUsageForHighestGrade))
		Me.superMeterTicker.MaxValue = CInt(Cuphead.Current.ScoringProperties.superMeterUsageForHighestGrade)
		If data.useCoinsInsteadOfSuperMeter Then
			Me.superMeterTicker.TargetValue = data.coinsCollected
			Me.superMeterTicker.MaxValue = 5
			Me.spiritStockLabelLocalizationHelper.currentID = Localization.Find("ResultsMenuCoins").id
		End If
		Me.difficultyTicker.TargetValue = If((data.difficulty <> Level.Mode.Easy), If((data.difficulty <> Level.Mode.Normal), 2, 1), 0)
		Me.gradeDisplay.Grade = Level.Grade
		Me.gradeDisplay.Difficulty = data.difficulty
		Yield New WaitForSeconds(Me.introDelay)
		Dim tickers As WinScreenTicker() = New WinScreenTicker() { Me.timeTicker, Me.hitsTicker, Me.parriesTicker, Me.superMeterTicker, Me.difficultyTicker }
		For Each ticker As WinScreenTicker In tickers
			ticker.StartCounting()
			While Not ticker.FinishedCounting
				Yield Nothing
			End While
			If ticker.TargetValue <> 0 Then
				Yield New WaitForSeconds(Me.talliesDelay)
			End If
		Next
		InterruptingPrompt.SetCanInterrupt(True)
		Dim timer As Single = 0F
		While timer < Me.gradeDelay
			If Me.input.GetAnyButtonDown() Then
				Exit While
			End If
			If Not InterruptingPrompt.IsInterrupting() Then
				timer += Time.deltaTime
			End If
			Yield Nothing
		End While
		Me.gradeDisplay.Show()
		While Not Me.gradeDisplay.FinishedGrading
			Yield Nothing
		End While
		timer = 0F
		Me.continuePrompt.SetActive(True)
		While timer < Me.advanceDelay
			If Me.input.GetActionButtonDown() Then
				Exit While
			End If
			If Not InterruptingPrompt.IsInterrupting() Then
				timer += Time.deltaTime
			End If
			Yield Nothing
		End While
		Me.done = True
		Return
	End Function

	' Token: 0x0600456C RID: 17772 RVA: 0x002483C4 File Offset: 0x002467C4
	Private Sub AlignBannerText(bannerText As GameObject)
		bannerText.GetComponent(Of TextMeshCurveAndJitter)().CurveScale = CSng(TowerOfPowerScorecard.TryRegularCurveValues(CInt(Localization.language)))
		Dim localPosition As Vector3 = bannerText.transform.localPosition
		localPosition.y = CSng(TowerOfPowerScorecard.TryRegularCurveOffsets(CInt(Localization.language)))
		bannerText.transform.localPosition = localPosition
	End Sub

	' Token: 0x0600456D RID: 17773 RVA: 0x00248414 File Offset: 0x00246814
	Private Function GetTriggerName(language As Localization.Languages) As String
		Select Case language
			Case Localization.Languages.French
				Return "useFrench"
			Case Localization.Languages.Italian
				Return "useItalian"
			Case Localization.Languages.German
				Return "useGerman"
			Case Localization.Languages.SpanishSpain
				Return "useSpanishSpain"
			Case Localization.Languages.SpanishAmerica
				Return "useSpanishAmerica"
			Case Localization.Languages.Korean
				Return "useKorean"
			Case Localization.Languages.Russian
				Return "useRussian"
			Case Localization.Languages.Polish
				Return "usePolish"
			Case Localization.Languages.PortugueseBrazil
				Return "usePortuguese"
			Case Localization.Languages.Japanese
				Return "useJapanese"
			Case Localization.Languages.SimplifiedChinese
				Return "useChinese"
			Case Else
				Return "useEnglish"
		End Select
	End Function

	' Token: 0x04004B21 RID: 19233
	Private Shared TryRegularCurveValues As Integer() = New Integer() { 0, 62, 48, 57, 65, 74, 58, 36, 54, 72, 83, 27 }

	' Token: 0x04004B22 RID: 19234
	Private Shared TryRegularCurveOffsets As Integer() = New Integer() { 0, 32, 18, 27, 35, 45, 28, 7, 24, 42, 50, -3 }

	' Token: 0x04004B23 RID: 19235
	Private Const BOB_FRAME_TIME As Single = 0.041666668F

	' Token: 0x04004B24 RID: 19236
	<Header("Delays")>
	<SerializeField()>
	Private introDelay As Single = 10F

	' Token: 0x04004B25 RID: 19237
	<SerializeField()>
	Private talliesDelay As Single = 0.5F

	' Token: 0x04004B26 RID: 19238
	<SerializeField()>
	Private gradeDelay As Single = 0.7F

	' Token: 0x04004B27 RID: 19239
	<SerializeField()>
	Private advanceDelay As Single = 10F

	' Token: 0x04004B28 RID: 19240
	<SerializeField()>
	Private timeTicker As WinScreenTicker

	' Token: 0x04004B29 RID: 19241
	<SerializeField()>
	Private hitsTicker As WinScreenTicker

	' Token: 0x04004B2A RID: 19242
	<SerializeField()>
	Private parriesTicker As WinScreenTicker

	' Token: 0x04004B2B RID: 19243
	<SerializeField()>
	Private superMeterTicker As WinScreenTicker

	' Token: 0x04004B2C RID: 19244
	<SerializeField()>
	Private difficultyTicker As WinScreenTicker

	' Token: 0x04004B2D RID: 19245
	<SerializeField()>
	Private spiritStockLabelLocalizationHelper As LocalizationHelper

	' Token: 0x04004B2E RID: 19246
	<SerializeField()>
	Private gradeDisplay As WinScreenGradeDisplay

	' Token: 0x04004B2F RID: 19247
	<SerializeField()>
	Private continuePrompt As GameObject

	' Token: 0x04004B30 RID: 19248
	<Header("UI Scoring")>
	<SerializeField()>
	Private scoring As GameObject

	' Token: 0x04004B31 RID: 19249
	<SerializeField()>
	Private gradeLabel As TextMeshProUGUI

	' Token: 0x04004B32 RID: 19250
	<Header("Try Text")>
	<SerializeField()>
	Private tryRegular As GameObject

	' Token: 0x04004B33 RID: 19251
	<SerializeField()>
	Private tryRegularText As TMP_Text

	' Token: 0x04004B34 RID: 19252
	<SerializeField()>
	Private tryRegularEnglishBackground As SpriteRenderer

	' Token: 0x04004B35 RID: 19253
	<Header("Glow effect")>
	<SerializeField()>
	Private glowingText As GameObject

	' Token: 0x04004B36 RID: 19254
	<SerializeField()>
	Private glowScript As GlowText

	' Token: 0x04004B37 RID: 19255
	<SerializeField()>
	Private postProcessingScript As PostProcessingBehaviour

	' Token: 0x04004B38 RID: 19256
	<SerializeField()>
	Public asianProfile As PostProcessingProfile

	' Token: 0x04004B39 RID: 19257
	<SerializeField()>
	Private tryExpert As GameObject

	' Token: 0x04004B3A RID: 19258
	<SerializeField()>
	Private tryExpertText As TMP_Text

	' Token: 0x04004B3B RID: 19259
	<Space(10F)>
	<SerializeField()>
	Private studioMHDRSubtitles As SpriteRenderer()

	' Token: 0x04004B3C RID: 19260
	<SerializeField()>
	Private resultsTitles As Transform()

	' Token: 0x04004B3D RID: 19261
	<SerializeField()>
	Private playerOneOffCenterTitleRoot As Vector3

	' Token: 0x04004B3E RID: 19262
	<SerializeField()>
	Private japaneseTitleRoot As Vector3

	' Token: 0x04004B3F RID: 19263
	<SerializeField()>
	Private koreanTitleRoot As Vector3

	' Token: 0x04004B40 RID: 19264
	<SerializeField()>
	Private chineseTitleRoot As Vector3

	' Token: 0x04004B41 RID: 19265
	<Space(10F)>
	<SerializeField()>
	Private results As Canvas

	' Token: 0x04004B42 RID: 19266
	<Header("BannerCurve")>
	<SerializeField()>
	Private textWidthRange As MinMax

	' Token: 0x04004B43 RID: 19267
	<SerializeField()>
	Private curveScaleRange As MinMax

	' Token: 0x04004B44 RID: 19268
	<SerializeField()>
	Private yOffsetDelta As Single

	' Token: 0x04004B45 RID: 19269
	Private input As CupheadInput.AnyPlayerInput

	' Token: 0x04004B46 RID: 19270
	Private Const BG_NORMAL_SPEED As Single = 50F

	' Token: 0x04004B47 RID: 19271
	Private Const BG_FAST_SPEED As Single = 150F

	' Token: 0x04004B48 RID: 19272
	Public done As Boolean
End Class
