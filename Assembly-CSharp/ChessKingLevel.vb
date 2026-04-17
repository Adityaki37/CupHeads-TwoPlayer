Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200009B RID: 155
Public Class ChessKingLevel
	Inherits Level

	' Token: 0x060001D0 RID: 464 RVA: 0x0005A35C File Offset: 0x0005875C
	Protected Overrides Sub PartialInit()
		Me.properties = LevelProperties.ChessKing.GetMode(MyBase.mode)
		AddHandler Me.properties.OnStateChange, AddressOf MyBase.zHack_OnStateChanged
		AddHandler Me.properties.OnBossDeath, AddressOf MyBase.zHack_OnWin
		MyBase.timeline = Me.properties.CreateTimeline(MyBase.mode)
		Me.goalTimes = Me.properties.goalTimes
		AddHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		MyBase.PartialInit()
	End Sub

	' Token: 0x1700004A RID: 74
	' (get) Token: 0x060001D1 RID: 465 RVA: 0x0005A3F2 File Offset: 0x000587F2
	Public Overrides ReadOnly Property CurrentLevel As Levels
		Get
			Return Levels.ChessKing
		End Get
	End Property

	' Token: 0x1700004B RID: 75
	' (get) Token: 0x060001D2 RID: 466 RVA: 0x0005A3F9 File Offset: 0x000587F9
	Public Overrides ReadOnly Property CurrentScene As Scenes
		Get
			Return Scenes.scene_level_chess_king
		End Get
	End Property

	' Token: 0x1700004C RID: 76
	' (get) Token: 0x060001D3 RID: 467 RVA: 0x0005A3FD File Offset: 0x000587FD
	Public Overrides ReadOnly Property BossPortrait As Sprite
		Get
			Return Me._bossPortrait
		End Get
	End Property

	' Token: 0x1700004D RID: 77
	' (get) Token: 0x060001D4 RID: 468 RVA: 0x0005A405 File Offset: 0x00058805
	Public Overrides ReadOnly Property BossQuote As String
		Get
			Return Me._bossQuote
		End Get
	End Property

	' Token: 0x060001D5 RID: 469 RVA: 0x0005A40D File Offset: 0x0005880D
	Protected Overrides Sub Start()
		Level.IsChessBoss = True
		MyBase.Start()
		Me.king.LevelInit(Me.properties)
	End Sub

	' Token: 0x060001D6 RID: 470 RVA: 0x0005A42C File Offset: 0x0005882C
	Protected Overrides Sub OnLevelStart()
		Me.king.StartGame()
	End Sub

	' Token: 0x060001D7 RID: 471 RVA: 0x0005A439 File Offset: 0x00058839
	Protected Overrides Sub OnStateChanged()
		MyBase.OnStateChanged()
		Me.king.StateChange()
	End Sub

	' Token: 0x060001D8 RID: 472 RVA: 0x0005A44C File Offset: 0x0005884C
	Private Iterator Function chesskingPattern_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		While True
			Yield MyBase.StartCoroutine(Me.nextPattern_cr())
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060001D9 RID: 473 RVA: 0x0005A468 File Offset: 0x00058868
	Private Iterator Function nextPattern_cr() As IEnumerator
		Dim p As LevelProperties.ChessKing.Pattern = Me.properties.CurrentState.NextPattern
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		Return
	End Function

	' Token: 0x04000352 RID: 850
	Private properties As LevelProperties.ChessKing

	' Token: 0x04000353 RID: 851
	<SerializeField()>
	Private king As ChessKingLevelKing

	' Token: 0x04000354 RID: 852
	<Header("Boss Info")>
	<SerializeField()>
	Private _bossPortrait As Sprite

	' Token: 0x04000355 RID: 853
	<SerializeField()>
	<Multiline()>
	Private _bossQuote As String
End Class
