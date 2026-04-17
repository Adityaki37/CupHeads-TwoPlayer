Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020001C6 RID: 454
Public Class FlyingGenieLevel
	Inherits Level

	' Token: 0x060004F7 RID: 1271 RVA: 0x000662B0 File Offset: 0x000646B0
	Protected Overrides Sub PartialInit()
		Me.properties = LevelProperties.FlyingGenie.GetMode(MyBase.mode)
		AddHandler Me.properties.OnStateChange, AddressOf MyBase.zHack_OnStateChanged
		AddHandler Me.properties.OnBossDeath, AddressOf MyBase.zHack_OnWin
		MyBase.timeline = Me.properties.CreateTimeline(MyBase.mode)
		Me.goalTimes = Me.properties.goalTimes
		AddHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		MyBase.PartialInit()
	End Sub

	' Token: 0x170000E1 RID: 225
	' (get) Token: 0x060004F8 RID: 1272 RVA: 0x00066346 File Offset: 0x00064746
	Public Overrides ReadOnly Property CurrentLevel As Levels
		Get
			Return Levels.FlyingGenie
		End Get
	End Property

	' Token: 0x170000E2 RID: 226
	' (get) Token: 0x060004F9 RID: 1273 RVA: 0x0006634D File Offset: 0x0006474D
	Public Overrides ReadOnly Property CurrentScene As Scenes
		Get
			Return Scenes.scene_level_flying_genie
		End Get
	End Property

	' Token: 0x170000E3 RID: 227
	' (get) Token: 0x060004FA RID: 1274 RVA: 0x00066354 File Offset: 0x00064754
	Public Overrides ReadOnly Property BossPortrait As Sprite
		Get
			Select Case Me.properties.CurrentState.stateName
				Case LevelProperties.FlyingGenie.States.Main, LevelProperties.FlyingGenie.States.Generic
					Return Me._bossPortraitMain
				Case LevelProperties.FlyingGenie.States.Giant
					Return Me._bossPortraitGiant
				Case LevelProperties.FlyingGenie.States.Marionette
					Return Me._bossPortraitMarionette
				Case LevelProperties.FlyingGenie.States.Disappear
					Return If((Me.genie.state <> FlyingGenieLevelGenie.State.Disappear), Me._bossPortraitCoffin, Me._bossPortraitDisappear)
				Case Else
					Global.Debug.LogError("Couldn't find portrait for state " + Me.properties.CurrentState.stateName + ". Using Main.", Nothing)
					Return Me._bossPortraitMain
			End Select
		End Get
	End Property

	' Token: 0x170000E4 RID: 228
	' (get) Token: 0x060004FB RID: 1275 RVA: 0x000663FC File Offset: 0x000647FC
	Public Overrides ReadOnly Property BossQuote As String
		Get
			Select Case Me.properties.CurrentState.stateName
				Case LevelProperties.FlyingGenie.States.Main, LevelProperties.FlyingGenie.States.Generic
					Return Me._bossQuoteMain
				Case LevelProperties.FlyingGenie.States.Giant
					If PlayerData.Data.DjimmiActivatedBaseGame() Then
						Return Me._bossQuoteGameDjimmi
					End If
					Return Me._bossQuoteGiant
				Case LevelProperties.FlyingGenie.States.Marionette
					Return Me._bossQuoteMarionette
				Case LevelProperties.FlyingGenie.States.Disappear
					Return If((Me.genie.state <> FlyingGenieLevelGenie.State.Disappear), Me._bossQuoteCoffin, Me._bossQuoteDisappear)
				Case Else
					Global.Debug.LogError("Couldn't find quote for state " + Me.properties.CurrentState.stateName + ". Using Main.", Nothing)
					Return Me._bossQuoteMain
			End Select
		End Get
	End Property

	' Token: 0x060004FC RID: 1276 RVA: 0x000664B7 File Offset: 0x000648B7
	Protected Overrides Sub Awake()
		MyBase.Awake()
		MyBase.StartCoroutine(FlyingGenieLevel.timer_cr())
	End Sub

	' Token: 0x060004FD RID: 1277 RVA: 0x000664CB File Offset: 0x000648CB
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.genie.LevelInit(Me.properties)
		Me.genieTransformed.LevelInit(Me.properties)
		Me.goop.LevelInit(Me.properties)
	End Sub

	' Token: 0x060004FE RID: 1278 RVA: 0x00066506 File Offset: 0x00064906
	Protected Overrides Sub OnLevelStart()
	End Sub

	' Token: 0x060004FF RID: 1279 RVA: 0x00066508 File Offset: 0x00064908
	Protected Overrides Sub OnStateChanged()
		MyBase.OnStateChanged()
		If Me.properties.CurrentState.stateName = LevelProperties.FlyingGenie.States.Marionette Then
			MyBase.StartCoroutine(Me.phase2_cr())
		ElseIf Me.properties.CurrentState.stateName = LevelProperties.FlyingGenie.States.Giant Then
			MyBase.StartCoroutine(Me.phase3_cr())
		ElseIf Me.properties.CurrentState.stateName = LevelProperties.FlyingGenie.States.Disappear Then
			MyBase.StartCoroutine(Me.pillar_cr())
		ElseIf Me.properties.CurrentState.stateName = LevelProperties.FlyingGenie.States.Generic Then
			MyBase.StartCoroutine(Me.treasure_cr())
		End If
	End Sub

	' Token: 0x06000500 RID: 1280 RVA: 0x000665B6 File Offset: 0x000649B6
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me._bossPortraitCoffin = Nothing
		Me._bossPortraitDisappear = Nothing
		Me._bossPortraitGiant = Nothing
		Me._bossPortraitMain = Nothing
		Me._bossPortraitMarionette = Nothing
	End Sub

	' Token: 0x06000501 RID: 1281 RVA: 0x000665E4 File Offset: 0x000649E4
	Private Iterator Function flyinggeniePattern_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		While True
			Yield MyBase.StartCoroutine(Me.nextPattern_cr())
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000502 RID: 1282 RVA: 0x00066600 File Offset: 0x00064A00
	Private Iterator Function nextPattern_cr() As IEnumerator
		Dim p As LevelProperties.FlyingGenie.Pattern = Me.properties.CurrentState.NextPattern
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		Return
	End Function

	' Token: 0x06000503 RID: 1283 RVA: 0x0006661C File Offset: 0x00064A1C
	Private Iterator Function phase2_cr() As IEnumerator
		Me.genie.HitTrigger()
		While Me.genie.state <> FlyingGenieLevelGenie.State.Idle
			Yield Nothing
		End While
		Me.genie.animator.SetTrigger("ToPhase2")
		Return
	End Function

	' Token: 0x06000504 RID: 1284 RVA: 0x00066638 File Offset: 0x00064A38
	Private Iterator Function phase3_cr() As IEnumerator
		If Not Me.genieTransformed.skipMarionette Then
			Me.genieTransformed.EndMarionette()
		Else
			Me.secretTriggered = True
		End If
		Yield Nothing
		Return
	End Function

	' Token: 0x06000505 RID: 1285 RVA: 0x00066654 File Offset: 0x00064A54
	Private Iterator Function pillar_cr() As IEnumerator
		Me.genie.HitTrigger()
		While Me.genie.state <> FlyingGenieLevelGenie.State.Idle
			Yield Nothing
		End While
		Me.genie.StartObelisk()
		While Me.genie.state <> FlyingGenieLevelGenie.State.Idle
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000506 RID: 1286 RVA: 0x00066670 File Offset: 0x00064A70
	Private Iterator Function treasure_cr() As IEnumerator
		Me.genie.HitTrigger()
		While Me.genie.state <> FlyingGenieLevelGenie.State.Idle
			Yield Nothing
		End While
		Me.genie.StartTreasure()
		While Me.genie.state <> FlyingGenieLevelGenie.State.Idle
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000507 RID: 1287 RVA: 0x0006668C File Offset: 0x00064A8C
	Private Shared Iterator Function timer_cr() As IEnumerator
		FlyingGenieLevel.mainTimer = 9.25F
		While True
			FlyingGenieLevel.mainTimer += CupheadTime.Delta
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x04000966 RID: 2406
	Private properties As LevelProperties.FlyingGenie

	' Token: 0x04000967 RID: 2407
	Public Const SHADE_PERIOD As Single = 12F

	' Token: 0x04000968 RID: 2408
	Private Const SHADE_START_TIME As Single = 9.25F

	' Token: 0x04000969 RID: 2409
	Public Shared mainTimer As Single

	' Token: 0x0400096A RID: 2410
	<SerializeField()>
	Private goop As FlyingGenieLevelGoop

	' Token: 0x0400096B RID: 2411
	<SerializeField()>
	Private genie As FlyingGenieLevelGenie

	' Token: 0x0400096C RID: 2412
	<SerializeField()>
	Private genieTransformed As FlyingGenieLevelGenieTransform

	' Token: 0x0400096D RID: 2413
	<Header("Boss Info")>
	<SerializeField()>
	Private _bossPortraitMain As Sprite

	' Token: 0x0400096E RID: 2414
	<SerializeField()>
	Private _bossPortraitDisappear As Sprite

	' Token: 0x0400096F RID: 2415
	<SerializeField()>
	Private _bossPortraitCoffin As Sprite

	' Token: 0x04000970 RID: 2416
	<SerializeField()>
	Private _bossPortraitMarionette As Sprite

	' Token: 0x04000971 RID: 2417
	<SerializeField()>
	Private _bossPortraitGiant As Sprite

	' Token: 0x04000972 RID: 2418
	<SerializeField()>
	Private _bossQuoteMain As String

	' Token: 0x04000973 RID: 2419
	<SerializeField()>
	Private _bossQuoteDisappear As String

	' Token: 0x04000974 RID: 2420
	<SerializeField()>
	Private _bossQuoteCoffin As String

	' Token: 0x04000975 RID: 2421
	<SerializeField()>
	Private _bossQuoteMarionette As String

	' Token: 0x04000976 RID: 2422
	<SerializeField()>
	Private _bossQuoteGiant As String

	' Token: 0x04000977 RID: 2423
	<SerializeField()>
	Private _bossQuoteGameDjimmi As String
End Class
