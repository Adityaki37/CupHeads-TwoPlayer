Imports System
Imports System.Collections
Imports TMPro
Imports UnityEngine
Imports UnityEngine.PostProcessing
Imports UnityEngine.UI

' Token: 0x02000B32 RID: 2866
Public Class WinScreen
	Inherits AbstractMonoBehaviour

	' Token: 0x06004570 RID: 17776 RVA: 0x00248C78 File Offset: 0x00247078
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.OnePlayerCuphead.SetActive(False)
		Me.TwoPlayerCupheadMugman.SetActive(False)
		Cuphead.Init(False)
		Dim scoringData As LevelScoringData = Level.ScoringData
		If scoringData IsNot Nothing Then
			Me.player1IsChalice = scoringData.player1IsChalice
			Me.player2IsChalice = scoringData.player2IsChalice
		End If
		If Not PlayerManager.Multiplayer Then
			Me.player2IsChalice = False
		End If
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
		If PlayerManager.Multiplayer Then
			Dim animator As Animator
			If PlayerManager.player1IsMugman Then
				If Me.player1IsChalice Then
					animator = Me.TwoPlayerTitleChaliceCuphead
					Me.TwoPlayerChaliceCuphead.SetActive(True)
					Me.results.transform.position = Me.TwoPlayerChaliceCupheadUIRoot.transform.position
				ElseIf Me.player2IsChalice Then
					animator = Me.TwoPlayerTitleMugmanChalice
					Me.TwoPlayerMugmanChalice.SetActive(True)
					Me.results.transform.position = Me.TwoPlayerMugmanChaliceUIRoot.transform.position
				Else
					animator = Me.TwoPlayerTitleMugman
					Me.TwoPlayerMugmanCuphead.SetActive(True)
					Me.results.transform.position = Me.TwoPlayerMugmanCupheadUIRoot.transform.position
				End If
			ElseIf Me.player1IsChalice Then
				animator = Me.TwoPlayerTitleChaliceMugman
				Me.TwoPlayerChaliceMugman.SetActive(True)
				Me.results.transform.position = Me.TwoPlayerChaliceMugmanUIRoot.transform.position
			ElseIf Me.player2IsChalice Then
				animator = Me.TwoPlayerTitleCupheadChalice
				Me.TwoPlayerCupheadChalice.SetActive(True)
				Me.results.transform.position = Me.TwoPlayerCupheadChaliceUIRoot.transform.position
			Else
				animator = Me.TwoPlayerTitleCuphead
				Me.TwoPlayerCupheadMugman.SetActive(True)
				Me.results.transform.position = Me.TwoPlayerCupheadMugmanUIRoot.transform.position
			End If
			If Localization.language = Localization.Languages.Polish OrElse Localization.language = Localization.Languages.Italian OrElse Localization.language = Localization.Languages.French Then
				Me.CenterResultTitles(Me.japaneseTitleRoot)
			End If
			If Localization.language = Localization.Languages.English Then
				animator.SetBool("pickedA", Rand.Bool())
			End If
			animator.SetTrigger(Me.GetTriggerName(Localization.language))
		Else
			Dim animator As Animator
			If Me.player1IsChalice Then
				animator = Me.OnePlayerTitleChalice
				Me.OnePlayerChalice.SetActive(True)
			ElseIf PlayerManager.player1IsMugman Then
				animator = Me.OnePlayerTitleMugman
				Me.OnePlayerMugman.SetActive(True)
			Else
				animator = Me.OnePlayerTitleCuphead
				Me.OnePlayerCuphead.SetActive(True)
			End If
			Me.results.transform.position = Me.OnePlayerUIRoot.transform.position
			If Localization.language = Localization.Languages.Polish OrElse Localization.language = Localization.Languages.Italian OrElse Localization.language = Localization.Languages.French OrElse Localization.language = Localization.Languages.SimplifiedChinese OrElse Localization.language = Localization.Languages.Japanese Then
				Me.CenterResultTitles(Me.playerOneOffCenterTitleRoot)
			End If
			If Localization.language = Localization.Languages.English Then
				animator.SetBool("pickedA", Rand.Bool())
			End If
			animator.SetTrigger(Me.GetTriggerName(Localization.language))
		End If
		MyBase.StartCoroutine(Me.main_cr())
		Me.continuePrompt.SetActive(False)
		Me.input = New CupheadInput.AnyPlayerInput(False)
		MyBase.StartCoroutine(Me.rotate_bg_cr())
	End Sub

	' Token: 0x06004571 RID: 17777 RVA: 0x0024907C File Offset: 0x0024747C
	Private Sub DisableEnglishMDHRSubtitles()
		For Each spriteRenderer As SpriteRenderer In Me.studioMHDRSubtitles
			spriteRenderer.enabled = False
		Next
	End Sub

	' Token: 0x06004572 RID: 17778 RVA: 0x002490B0 File Offset: 0x002474B0
	Private Sub CenterResultTitles(rootPosition As Vector3)
		If Me.player1IsChalice Then
			rootPosition += If((Not PlayerManager.Multiplayer), Me.chaliceTitleOffset1P, Me.chaliceTitleOffset2P)
		End If
		For Each transform As Transform In Me.resultsTitles
			transform.localPosition = rootPosition
		Next
	End Sub

	' Token: 0x06004573 RID: 17779 RVA: 0x00249114 File Offset: 0x00247514
	Private Iterator Function main_cr() As IEnumerator
		Dim data As LevelScoringData = Level.ScoringData
		If Localization.language = Localization.Languages.Korean Then
			For Each textMeshProUGUI As TextMeshProUGUI In Me.scoring.GetComponentsInChildren(Of TextMeshProUGUI)()
				textMeshProUGUI.fontStyle = FontStyles.Bold
			Next
			Me.gradeLabel.fontStyle = FontStyles.Bold
		End If
		If data.difficulty = Level.Mode.Easy AndAlso Level.PreviousDifficulty = Level.Mode.Easy AndAlso Level.PreviousLevelType = Level.Type.Battle AndAlso Not Level.IsDicePalace AndAlso Not Level.IsDicePalaceMain AndAlso Level.PreviousLevel <> Levels.Devil AndAlso Level.PreviousLevel <> Levels.Saltbaker Then
			If Array.IndexOf(Of Levels)(Level.worldDLCBossLevels, Level.PreviousLevel) >= 0 Then
				Me.isDLCLevel = True
			Else
				Me.isDLCLevel = False
			End If
			Dim translation As Localization.Translation = If((Not Me.isDLCLevel), Localization.Translate("ResultsTryRegular"), Localization.Translate("WinScreen_Tooltip_SimpleIngredient"))
			Me.tryRegular.SetActive(True)
			If(translation.image Is Nothing AndAlso Not translation.hasSpriteAtlasImage) OrElse Me.isDLCLevel Then
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
				If Localization.language <> Localization.Languages.English OrElse Me.isDLCLevel Then
					Me.glowScript.BeginGlow()
				End If
			Else
				Me.tryRegularText.enabled = False
			End If
		End If
		If data Is Nothing Then
			Return
		End If
		Me.timeTicker.TargetValue = CInt(data.time)
		Me.timeTicker.MaxValue = CInt(data.goalTime)
		Me.hitsTicker.TargetValue = Mathf.Clamp(data.finalHP, 0, 3)
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
		If Level.PreviousLevel = Levels.Devil Then
			Cutscene.Load(Scenes.scene_title, Scenes.scene_cutscene_outro, SceneLoader.Transition.Iris, SceneLoader.Transition.Fade, SceneLoader.Icon.Hourglass)
		ElseIf Level.PreviousLevel = Levels.Saltbaker Then
			Cutscene.Load(Scenes.scene_map_world_DLC, Scenes.scene_cutscene_dlc_ending, SceneLoader.Transition.Iris, SceneLoader.Transition.Fade, SceneLoader.Icon.Hourglass)
		Else
			SceneLoader.LoadLastMap()
		End If
		Return
	End Function

	' Token: 0x06004574 RID: 17780 RVA: 0x00249130 File Offset: 0x00247530
	Private Sub AlignBannerText(bannerText As GameObject)
		bannerText.GetComponent(Of TextMeshCurveAndJitter)().CurveScale = CSng(If((Not Me.isDLCLevel), WinScreen.TryRegularCurveValues(CInt(Localization.language)), WinScreen.TryRegularCurveValuesDLC(CInt(Localization.language))))
		Dim localPosition As Vector3 = bannerText.transform.localPosition
		localPosition.y = CSng(If((Not Me.isDLCLevel), WinScreen.TryRegularCurveOffsets(CInt(Localization.language)), WinScreen.TryRegularCurveOffsetsDLC(CInt(Localization.language))))
		bannerText.transform.localPosition = localPosition
	End Sub

	' Token: 0x06004575 RID: 17781 RVA: 0x002491B8 File Offset: 0x002475B8
	Private Iterator Function rotate_bg_cr() As IEnumerator
		Dim frameTime As Single = 0F
		Dim normalTime As Single = 0F
		Dim speed As Single = 50F
		While True
			frameTime += CupheadTime.Delta
			While frameTime > 0.041666668F
				frameTime -= 0.041666668F
				Me.Background.Rotate(0F, 0F, speed * CupheadTime.Delta)
				Yield Nothing
			End While
			If Me.gradeDisplay.Celebration AndAlso speed < 150F Then
				normalTime += CupheadTime.Delta
				speed = Mathf.Lerp(50F, 150F, normalTime / 0.5F)
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06004576 RID: 17782 RVA: 0x002491D4 File Offset: 0x002475D4
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

	' Token: 0x04004B49 RID: 19273
	Private Shared TryRegularCurveValues As Integer() = New Integer() { 0, 62, 48, 57, 65, 74, 58, 36, 54, 72, 83, 27 }

	' Token: 0x04004B4A RID: 19274
	Private Shared TryRegularCurveOffsets As Integer() = New Integer() { 0, 32, 18, 27, 35, 45, 28, 7, 24, 42, 50, -3 }

	' Token: 0x04004B4B RID: 19275
	Private Shared TryRegularCurveValuesDLC As Integer() = New Integer() { 62, 62, 48, 57, 65, 74, 58, 36, 54, 72, 83, 27 }

	' Token: 0x04004B4C RID: 19276
	Private Shared TryRegularCurveOffsetsDLC As Integer() = New Integer() { 32, 32, 18, 27, 35, 45, 28, 7, 24, 42, 50, -3 }

	' Token: 0x04004B4D RID: 19277
	Private Const BOB_FRAME_TIME As Single = 0.041666668F

	' Token: 0x04004B4E RID: 19278
	<Header("Delays")>
	<SerializeField()>
	Private introDelay As Single = 10F

	' Token: 0x04004B4F RID: 19279
	<SerializeField()>
	Private talliesDelay As Single = 0.5F

	' Token: 0x04004B50 RID: 19280
	<SerializeField()>
	Private gradeDelay As Single = 0.7F

	' Token: 0x04004B51 RID: 19281
	<SerializeField()>
	Private advanceDelay As Single = 10F

	' Token: 0x04004B52 RID: 19282
	<SerializeField()>
	Private timeTicker As WinScreenTicker

	' Token: 0x04004B53 RID: 19283
	<SerializeField()>
	Private hitsTicker As WinScreenTicker

	' Token: 0x04004B54 RID: 19284
	<SerializeField()>
	Private parriesTicker As WinScreenTicker

	' Token: 0x04004B55 RID: 19285
	<SerializeField()>
	Private superMeterTicker As WinScreenTicker

	' Token: 0x04004B56 RID: 19286
	<SerializeField()>
	Private difficultyTicker As WinScreenTicker

	' Token: 0x04004B57 RID: 19287
	<SerializeField()>
	Private spiritStockLabelLocalizationHelper As LocalizationHelper

	' Token: 0x04004B58 RID: 19288
	<SerializeField()>
	Private gradeDisplay As WinScreenGradeDisplay

	' Token: 0x04004B59 RID: 19289
	<SerializeField()>
	Private continuePrompt As GameObject

	' Token: 0x04004B5A RID: 19290
	Private player1IsChalice As Boolean

	' Token: 0x04004B5B RID: 19291
	Private player2IsChalice As Boolean

	' Token: 0x04004B5C RID: 19292
	<Header("UI Scoring")>
	<SerializeField()>
	Private scoring As GameObject

	' Token: 0x04004B5D RID: 19293
	<SerializeField()>
	Private gradeLabel As TextMeshProUGUI

	' Token: 0x04004B5E RID: 19294
	<Header("Try Text")>
	<SerializeField()>
	Private tryRegular As GameObject

	' Token: 0x04004B5F RID: 19295
	<SerializeField()>
	Private tryRegularText As TMP_Text

	' Token: 0x04004B60 RID: 19296
	<SerializeField()>
	Private tryRegularEnglishBackground As SpriteRenderer

	' Token: 0x04004B61 RID: 19297
	<Header("Glow effect")>
	<SerializeField()>
	Private glowingText As GameObject

	' Token: 0x04004B62 RID: 19298
	<SerializeField()>
	Private glowScript As GlowText

	' Token: 0x04004B63 RID: 19299
	<SerializeField()>
	Private postProcessingScript As PostProcessingBehaviour

	' Token: 0x04004B64 RID: 19300
	<SerializeField()>
	Public asianProfile As PostProcessingProfile

	' Token: 0x04004B65 RID: 19301
	<SerializeField()>
	Private tryExpert As GameObject

	' Token: 0x04004B66 RID: 19302
	<SerializeField()>
	Private tryExpertText As TMP_Text

	' Token: 0x04004B67 RID: 19303
	<Header("Background")>
	<SerializeField()>
	Private Background As Transform

	' Token: 0x04004B68 RID: 19304
	<Header("DifferentLayouts")>
	<SerializeField()>
	Private OnePlayerCuphead As GameObject

	' Token: 0x04004B69 RID: 19305
	<SerializeField()>
	Private OnePlayerMugman As GameObject

	' Token: 0x04004B6A RID: 19306
	<SerializeField()>
	Private OnePlayerUIRoot As Transform

	' Token: 0x04004B6B RID: 19307
	<SerializeField()>
	Private OnePlayerTitleCuphead As Animator

	' Token: 0x04004B6C RID: 19308
	<SerializeField()>
	Private OnePlayerTitleMugman As Animator

	' Token: 0x04004B6D RID: 19309
	<Space(10F)>
	<SerializeField()>
	Private TwoPlayerCupheadMugman As GameObject

	' Token: 0x04004B6E RID: 19310
	<SerializeField()>
	Private TwoPlayerMugmanCuphead As GameObject

	' Token: 0x04004B6F RID: 19311
	<SerializeField()>
	Private TwoPlayerCupheadMugmanUIRoot As Transform

	' Token: 0x04004B70 RID: 19312
	<SerializeField()>
	Private TwoPlayerMugmanCupheadUIRoot As Transform

	' Token: 0x04004B71 RID: 19313
	<SerializeField()>
	Private TwoPlayerTitleCuphead As Animator

	' Token: 0x04004B72 RID: 19314
	<SerializeField()>
	Private TwoPlayerTitleMugman As Animator

	' Token: 0x04004B73 RID: 19315
	<Space(10F)>
	<SerializeField()>
	Private OnePlayerChalice As GameObject

	' Token: 0x04004B74 RID: 19316
	<SerializeField()>
	Private TwoPlayerChaliceCuphead As GameObject

	' Token: 0x04004B75 RID: 19317
	<SerializeField()>
	Private TwoPlayerCupheadChalice As GameObject

	' Token: 0x04004B76 RID: 19318
	<SerializeField()>
	Private TwoPlayerMugmanChalice As GameObject

	' Token: 0x04004B77 RID: 19319
	<SerializeField()>
	Private TwoPlayerChaliceMugman As GameObject

	' Token: 0x04004B78 RID: 19320
	<SerializeField()>
	Private OnePlayerChaliceUIRoot As Transform

	' Token: 0x04004B79 RID: 19321
	<SerializeField()>
	Private TwoPlayerChaliceCupheadUIRoot As Transform

	' Token: 0x04004B7A RID: 19322
	<SerializeField()>
	Private TwoPlayerCupheadChaliceUIRoot As Transform

	' Token: 0x04004B7B RID: 19323
	<SerializeField()>
	Private TwoPlayerMugmanChaliceUIRoot As Transform

	' Token: 0x04004B7C RID: 19324
	<SerializeField()>
	Private TwoPlayerChaliceMugmanUIRoot As Transform

	' Token: 0x04004B7D RID: 19325
	<SerializeField()>
	Private OnePlayerTitleChalice As Animator

	' Token: 0x04004B7E RID: 19326
	<SerializeField()>
	Private TwoPlayerTitleChaliceCuphead As Animator

	' Token: 0x04004B7F RID: 19327
	<SerializeField()>
	Private TwoPlayerTitleCupheadChalice As Animator

	' Token: 0x04004B80 RID: 19328
	<SerializeField()>
	Private TwoPlayerTitleMugmanChalice As Animator

	' Token: 0x04004B81 RID: 19329
	<SerializeField()>
	Private TwoPlayerTitleChaliceMugman As Animator

	' Token: 0x04004B82 RID: 19330
	<Space(10F)>
	<SerializeField()>
	Private studioMHDRSubtitles As SpriteRenderer()

	' Token: 0x04004B83 RID: 19331
	<SerializeField()>
	Private resultsTitles As Transform()

	' Token: 0x04004B84 RID: 19332
	<SerializeField()>
	Private playerOneOffCenterTitleRoot As Vector3

	' Token: 0x04004B85 RID: 19333
	<SerializeField()>
	Private japaneseTitleRoot As Vector3

	' Token: 0x04004B86 RID: 19334
	<SerializeField()>
	Private koreanTitleRoot As Vector3

	' Token: 0x04004B87 RID: 19335
	<SerializeField()>
	Private chineseTitleRoot As Vector3

	' Token: 0x04004B88 RID: 19336
	<Space(10F)>
	<SerializeField()>
	Private chaliceTitleOffset1P As Vector3

	' Token: 0x04004B89 RID: 19337
	<SerializeField()>
	Private chaliceTitleOffset2P As Vector3

	' Token: 0x04004B8A RID: 19338
	<Space(10F)>
	<SerializeField()>
	Private results As Canvas

	' Token: 0x04004B8B RID: 19339
	<Header("BannerCurve")>
	<SerializeField()>
	Private textWidthRange As MinMax

	' Token: 0x04004B8C RID: 19340
	<SerializeField()>
	Private curveScaleRange As MinMax

	' Token: 0x04004B8D RID: 19341
	<SerializeField()>
	Private yOffsetDelta As Single

	' Token: 0x04004B8E RID: 19342
	Private input As CupheadInput.AnyPlayerInput

	' Token: 0x04004B8F RID: 19343
	Private Const BG_NORMAL_SPEED As Single = 50F

	' Token: 0x04004B90 RID: 19344
	Private Const BG_FAST_SPEED As Single = 150F

	' Token: 0x04004B91 RID: 19345
	Private isDLCLevel As Boolean
End Class
