Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020000C3 RID: 195
Public Class ChessRookLevel
	Inherits ChessLevel

	' Token: 0x06000245 RID: 581 RVA: 0x0005BB10 File Offset: 0x00059F10
	Protected Overrides Sub PartialInit()
		Me.properties = LevelProperties.ChessRook.GetMode(MyBase.mode)
		AddHandler Me.properties.OnStateChange, AddressOf MyBase.zHack_OnStateChanged
		AddHandler Me.properties.OnBossDeath, AddressOf MyBase.zHack_OnWin
		MyBase.timeline = Me.properties.CreateTimeline(MyBase.mode)
		Me.goalTimes = Me.properties.goalTimes
		AddHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		MyBase.PartialInit()
	End Sub

	' Token: 0x1700005E RID: 94
	' (get) Token: 0x06000246 RID: 582 RVA: 0x0005BBA6 File Offset: 0x00059FA6
	Public Overrides ReadOnly Property CurrentLevel As Levels
		Get
			Return Levels.ChessRook
		End Get
	End Property

	' Token: 0x1700005F RID: 95
	' (get) Token: 0x06000247 RID: 583 RVA: 0x0005BBAD File Offset: 0x00059FAD
	Public Overrides ReadOnly Property CurrentScene As Scenes
		Get
			Return Scenes.scene_level_chess_rook
		End Get
	End Property

	' Token: 0x17000060 RID: 96
	' (get) Token: 0x06000248 RID: 584 RVA: 0x0005BBB1 File Offset: 0x00059FB1
	Public Overrides ReadOnly Property BossPortrait As Sprite
		Get
			Return Me._bossPortraitMain
		End Get
	End Property

	' Token: 0x17000061 RID: 97
	' (get) Token: 0x06000249 RID: 585 RVA: 0x0005BBB9 File Offset: 0x00059FB9
	Public Overrides ReadOnly Property BossQuote As String
		Get
			Return Me._bossQuoteMain
		End Get
	End Property

	' Token: 0x0600024A RID: 586 RVA: 0x0005BBC1 File Offset: 0x00059FC1
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me._bossPortraitMain = Nothing
		Me.rook = Nothing
	End Sub

	' Token: 0x0600024B RID: 587 RVA: 0x0005BBD7 File Offset: 0x00059FD7
	Protected Overrides Sub Start()
		Level.IsChessBoss = True
		MyBase.Start()
		Me.rook.LevelInit(Me.properties)
	End Sub

	' Token: 0x0600024C RID: 588 RVA: 0x0005BBF6 File Offset: 0x00059FF6
	Protected Overrides Sub OnLevelStart()
	End Sub

	' Token: 0x0600024D RID: 589 RVA: 0x0005BBF8 File Offset: 0x00059FF8
	Protected Overrides Sub OnStateChanged()
		MyBase.OnStateChanged()
		Me.rook.OnPhaseChange()
	End Sub

	' Token: 0x0600024E RID: 590 RVA: 0x0005BC0C File Offset: 0x0005A00C
	Private Iterator Function chessrookPattern_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		While True
			Yield MyBase.StartCoroutine(Me.nextPattern_cr())
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600024F RID: 591 RVA: 0x0005BC28 File Offset: 0x0005A028
	Private Iterator Function nextPattern_cr() As IEnumerator
		Dim p As LevelProperties.ChessRook.Pattern = Me.properties.CurrentState.NextPattern
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		Return
	End Function

	' Token: 0x040003F4 RID: 1012
	Private properties As LevelProperties.ChessRook

	' Token: 0x040003F5 RID: 1013
	<SerializeField()>
	Private rook As ChessRookLevelRook

	' Token: 0x040003F6 RID: 1014
	<Header("Boss Info")>
	<SerializeField()>
	Private _bossPortraitMain As Sprite

	' Token: 0x040003F7 RID: 1015
	<SerializeField()>
	Private _bossQuoteMain As String
End Class
