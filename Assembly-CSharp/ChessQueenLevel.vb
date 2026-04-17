Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020000BA RID: 186
Public Class ChessQueenLevel
	Inherits ChessLevel

	' Token: 0x06000228 RID: 552 RVA: 0x0005B49C File Offset: 0x0005989C
	Protected Overrides Sub PartialInit()
		Me.properties = LevelProperties.ChessQueen.GetMode(MyBase.mode)
		AddHandler Me.properties.OnStateChange, AddressOf MyBase.zHack_OnStateChanged
		AddHandler Me.properties.OnBossDeath, AddressOf MyBase.zHack_OnWin
		MyBase.timeline = Me.properties.CreateTimeline(MyBase.mode)
		Me.goalTimes = Me.properties.goalTimes
		AddHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		MyBase.PartialInit()
	End Sub

	' Token: 0x17000059 RID: 89
	' (get) Token: 0x06000229 RID: 553 RVA: 0x0005B532 File Offset: 0x00059932
	Public Overrides ReadOnly Property CurrentLevel As Levels
		Get
			Return Levels.ChessQueen
		End Get
	End Property

	' Token: 0x1700005A RID: 90
	' (get) Token: 0x0600022A RID: 554 RVA: 0x0005B539 File Offset: 0x00059939
	Public Overrides ReadOnly Property CurrentScene As Scenes
		Get
			Return Scenes.scene_level_chess_queen
		End Get
	End Property

	' Token: 0x1700005B RID: 91
	' (get) Token: 0x0600022B RID: 555 RVA: 0x0005B53D File Offset: 0x0005993D
	Public Overrides ReadOnly Property BossPortrait As Sprite
		Get
			Return Me._bossPortraitMain
		End Get
	End Property

	' Token: 0x1700005C RID: 92
	' (get) Token: 0x0600022C RID: 556 RVA: 0x0005B545 File Offset: 0x00059945
	Public Overrides ReadOnly Property BossQuote As String
		Get
			Return Me._bossQuoteMain
		End Get
	End Property

	' Token: 0x0600022D RID: 557 RVA: 0x0005B54D File Offset: 0x0005994D
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me._bossPortraitMain = Nothing
		Me.queen = Nothing
		Me.mouseAnimator = Nothing
	End Sub

	' Token: 0x0600022E RID: 558 RVA: 0x0005B56A File Offset: 0x0005996A
	Protected Overrides Sub Start()
		Level.IsChessBoss = True
		MyBase.Start()
		Me.queen.LevelInit(Me.properties)
	End Sub

	' Token: 0x0600022F RID: 559 RVA: 0x0005B58C File Offset: 0x0005998C
	Public Overrides Sub OnLevelEnd()
		MyBase.OnLevelEnd()
		Dim num As Single = Global.UnityEngine.Random.Range(0F, 1F)
		Me.mouseAnimator(0).Play("Win", 0, num)
		Me.mouseAnimator(1).Play("Win", 0, num + 0.33F)
		Me.mouseAnimator(2).Play("Win", 0, num + 0.66F)
		Me.mouseAnimator(0).Play("Idle", 1, 0F)
		Me.mouseAnimator(1).Play("Idle", 1, 0.33F)
		Me.mouseAnimator(2).Play("Idle", 1, 0.66F)
	End Sub

	' Token: 0x06000230 RID: 560 RVA: 0x0005B63F File Offset: 0x00059A3F
	Protected Overrides Sub OnStateChanged()
		MyBase.OnStateChanged()
		Me.queen.StateChanged()
	End Sub

	' Token: 0x06000231 RID: 561 RVA: 0x0005B652 File Offset: 0x00059A52
	Protected Overrides Sub OnLevelStart()
		MyBase.StartCoroutine(Me.chessQueenPattern_cr())
	End Sub

	' Token: 0x06000232 RID: 562 RVA: 0x0005B664 File Offset: 0x00059A64
	Private Iterator Function chessQueenPattern_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		While True
			Yield MyBase.StartCoroutine(Me.nextPattern_cr())
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000233 RID: 563 RVA: 0x0005B680 File Offset: 0x00059A80
	Private Iterator Function nextPattern_cr() As IEnumerator
		Dim p As LevelProperties.ChessQueen.Pattern = Me.properties.CurrentState.NextPattern
		If p <> LevelProperties.ChessQueen.Pattern.Lightning Then
			If p <> LevelProperties.ChessQueen.Pattern.Egg Then
				Yield CupheadTime.WaitForSeconds(Me, 1F)
			Else
				Yield MyBase.StartCoroutine(Me.egg_cr())
			End If
		Else
			Yield MyBase.StartCoroutine(Me.lightning_cr())
		End If
		Return
	End Function

	' Token: 0x06000234 RID: 564 RVA: 0x0005B69C File Offset: 0x00059A9C
	Public Function NextPatternIsEgg() As Boolean
		If Me.properties.CurrentState.PeekNextPattern = LevelProperties.ChessQueen.Pattern.Egg Then
			Dim nextPattern As LevelProperties.ChessQueen.Pattern = Me.properties.CurrentState.NextPattern
			Return True
		End If
		Return False
	End Function

	' Token: 0x06000235 RID: 565 RVA: 0x0005B6D4 File Offset: 0x00059AD4
	Private Iterator Function lightning_cr() As IEnumerator
		While Me.queen.state <> ChessQueenLevelQueen.States.Idle
			Yield Nothing
		End While
		Me.queen.StartLightning()
		While Me.queen.state <> ChessQueenLevelQueen.States.Idle
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000236 RID: 566 RVA: 0x0005B6F0 File Offset: 0x00059AF0
	Private Iterator Function egg_cr() As IEnumerator
		While Me.queen.state <> ChessQueenLevelQueen.States.Idle
			Yield Nothing
		End While
		Me.queen.StartEgg()
		While Me.queen.state <> ChessQueenLevelQueen.States.Idle
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x040003C5 RID: 965
	Private properties As LevelProperties.ChessQueen

	' Token: 0x040003C6 RID: 966
	<SerializeField()>
	Private queen As ChessQueenLevelQueen

	' Token: 0x040003C7 RID: 967
	<SerializeField()>
	Private mouseAnimator As Animator()

	' Token: 0x040003C8 RID: 968
	<Header("Boss Info")>
	<SerializeField()>
	Private _bossPortraitMain As Sprite

	' Token: 0x040003C9 RID: 969
	<SerializeField()>
	Private _bossQuoteMain As String

	' Token: 0x040003CA RID: 970
	Public cannonBlastFXVariant As Boolean
End Class
