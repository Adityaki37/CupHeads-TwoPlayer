Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000083 RID: 131
Public Class ChessBOldALevel
	Inherits Level

	' Token: 0x06000167 RID: 359 RVA: 0x00057EC0 File Offset: 0x000562C0
	Protected Overrides Sub PartialInit()
		Me.properties = LevelProperties.ChessBOldA.GetMode(MyBase.mode)
		AddHandler Me.properties.OnStateChange, AddressOf MyBase.zHack_OnStateChanged
		AddHandler Me.properties.OnBossDeath, AddressOf MyBase.zHack_OnWin
		MyBase.timeline = Me.properties.CreateTimeline(MyBase.mode)
		Me.goalTimes = Me.properties.goalTimes
		AddHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		MyBase.PartialInit()
	End Sub

	' Token: 0x17000039 RID: 57
	' (get) Token: 0x06000168 RID: 360 RVA: 0x00057F56 File Offset: 0x00056356
	Public Overrides ReadOnly Property CurrentLevel As Levels
		Get
			Return Levels.ChessBOldA
		End Get
	End Property

	' Token: 0x1700003A RID: 58
	' (get) Token: 0x06000169 RID: 361 RVA: 0x00057F5D File Offset: 0x0005635D
	Public Overrides ReadOnly Property CurrentScene As Scenes
		Get
			Return Scenes.scene_level_chess_bolda
		End Get
	End Property

	' Token: 0x1700003B RID: 59
	' (get) Token: 0x0600016A RID: 362 RVA: 0x00057F61 File Offset: 0x00056361
	Public Overrides ReadOnly Property BossPortrait As Sprite
		Get
			Return Me._bossPortraitMain
		End Get
	End Property

	' Token: 0x1700003C RID: 60
	' (get) Token: 0x0600016B RID: 363 RVA: 0x00057F69 File Offset: 0x00056369
	Public Overrides ReadOnly Property BossQuote As String
		Get
			Return Me._bossQuoteMain
		End Get
	End Property

	' Token: 0x0600016C RID: 364 RVA: 0x00057F71 File Offset: 0x00056371
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me._bossPortraitMain = Nothing
	End Sub

	' Token: 0x0600016D RID: 365 RVA: 0x00057F80 File Offset: 0x00056380
	Protected Overrides Sub Start()
		Level.IsChessBoss = True
		MyBase.Start()
		Me.bishop.LevelInit(Me.properties)
		For Each transform As Transform In Me.topPlatforms
			transform.transform.SetPosition(Nothing, New Single?(-360F + Me.properties.CurrentState.stage.platformHeight * 2F), Nothing)
		Next
		For Each transform2 As Transform In Me.bottomPlatforms
			transform2.transform.SetPosition(Nothing, New Single?(-360F + Me.properties.CurrentState.stage.platformHeight), Nothing)
		Next
	End Sub

	' Token: 0x0600016E RID: 366 RVA: 0x00058072 File Offset: 0x00056472
	Protected Overrides Sub OnLevelStart()
	End Sub

	' Token: 0x0600016F RID: 367 RVA: 0x00058074 File Offset: 0x00056474
	Private Iterator Function chessbishopPattern_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		While True
			Yield MyBase.StartCoroutine(Me.nextPattern_cr())
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000170 RID: 368 RVA: 0x00058090 File Offset: 0x00056490
	Private Iterator Function nextPattern_cr() As IEnumerator
		Dim p As LevelProperties.ChessBOldA.Pattern = Me.properties.CurrentState.NextPattern
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		Return
	End Function

	' Token: 0x040002DF RID: 735
	Private properties As LevelProperties.ChessBOldA

	' Token: 0x040002E0 RID: 736
	<SerializeField()>
	Private bishop As ChessBOldALevelBishop

	' Token: 0x040002E1 RID: 737
	<SerializeField()>
	Private topPlatforms As Transform()

	' Token: 0x040002E2 RID: 738
	<SerializeField()>
	Private bottomPlatforms As Transform()

	' Token: 0x040002E3 RID: 739
	<Header("Boss Info")>
	<SerializeField()>
	Private _bossPortraitMain As Sprite

	' Token: 0x040002E4 RID: 740
	<SerializeField()>
	Private _bossQuoteMain As String
End Class
