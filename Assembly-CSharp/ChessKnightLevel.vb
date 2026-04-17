Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020000A8 RID: 168
Public Class ChessKnightLevel
	Inherits ChessLevel

	' Token: 0x060001EC RID: 492 RVA: 0x0005A630 File Offset: 0x00058A30
	Protected Overrides Sub PartialInit()
		Me.properties = LevelProperties.ChessKnight.GetMode(MyBase.mode)
		AddHandler Me.properties.OnStateChange, AddressOf MyBase.zHack_OnStateChanged
		AddHandler Me.properties.OnBossDeath, AddressOf MyBase.zHack_OnWin
		MyBase.timeline = Me.properties.CreateTimeline(MyBase.mode)
		Me.goalTimes = Me.properties.goalTimes
		AddHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		MyBase.PartialInit()
	End Sub

	' Token: 0x1700004F RID: 79
	' (get) Token: 0x060001ED RID: 493 RVA: 0x0005A6C6 File Offset: 0x00058AC6
	Public Overrides ReadOnly Property CurrentLevel As Levels
		Get
			Return Levels.ChessKnight
		End Get
	End Property

	' Token: 0x17000050 RID: 80
	' (get) Token: 0x060001EE RID: 494 RVA: 0x0005A6CD File Offset: 0x00058ACD
	Public Overrides ReadOnly Property CurrentScene As Scenes
		Get
			Return Scenes.scene_level_chess_knight
		End Get
	End Property

	' Token: 0x17000051 RID: 81
	' (get) Token: 0x060001EF RID: 495 RVA: 0x0005A6D1 File Offset: 0x00058AD1
	Public Overrides ReadOnly Property BossPortrait As Sprite
		Get
			Return Me._bossPortraitMain
		End Get
	End Property

	' Token: 0x17000052 RID: 82
	' (get) Token: 0x060001F0 RID: 496 RVA: 0x0005A6D9 File Offset: 0x00058AD9
	Public Overrides ReadOnly Property BossQuote As String
		Get
			Return Me._bossQuoteMain
		End Get
	End Property

	' Token: 0x060001F1 RID: 497 RVA: 0x0005A6E1 File Offset: 0x00058AE1
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me._bossPortraitMain = Nothing
		Me.knight = Nothing
	End Sub

	' Token: 0x060001F2 RID: 498 RVA: 0x0005A6F8 File Offset: 0x00058AF8
	Protected Overrides Sub Start()
		Level.IsChessBoss = True
		MyBase.Start()
		Me.knight.LevelInit(Me.properties)
		For Each abstractPlayerController As AbstractPlayerController In PlayerManager.GetAllPlayers()
			Dim levelPlayerController As LevelPlayerController = CType(abstractPlayerController, LevelPlayerController)
			If levelPlayerController IsNot Nothing Then
				levelPlayerController.gameObject.layer = 31
				For Each transform As Transform In levelPlayerController.gameObject.GetComponentsInChildren(Of Transform)(True)
					transform.gameObject.layer = 31
				Next
			End If
		Next
	End Sub

	' Token: 0x060001F3 RID: 499 RVA: 0x0005A7C0 File Offset: 0x00058BC0
	Protected Overrides Sub OnPlayerJoined(playerId As PlayerId)
		MyBase.OnPlayerJoined(playerId)
		Dim player As AbstractPlayerController = PlayerManager.GetPlayer(playerId)
		If player Then
			For Each spriteRenderer As SpriteRenderer In player.GetComponentsInChildren(Of SpriteRenderer)()
				spriteRenderer.gameObject.layer = 31
			Next
		End If
	End Sub

	' Token: 0x060001F4 RID: 500 RVA: 0x0005A812 File Offset: 0x00058C12
	Protected Overrides Sub OnLevelStart()
		MyBase.StartCoroutine(Me.chessknightPattern_cr())
	End Sub

	' Token: 0x060001F5 RID: 501 RVA: 0x0005A824 File Offset: 0x00058C24
	Private Iterator Function chessknightPattern_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		While True
			Yield MyBase.StartCoroutine(Me.nextPattern_cr())
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060001F6 RID: 502 RVA: 0x0005A840 File Offset: 0x00058C40
	Private Iterator Function nextPattern_cr() As IEnumerator
		Select Case Me.properties.CurrentState.NextPattern
			Case LevelProperties.ChessKnight.Pattern.[Long]
				Yield MyBase.StartCoroutine(Me.long_cr())
			Case LevelProperties.ChessKnight.Pattern.[Short]
				Yield MyBase.StartCoroutine(Me.short_cr())
			Case LevelProperties.ChessKnight.Pattern.Up
				Yield MyBase.StartCoroutine(Me.up_cr())
			Case Else
				Yield CupheadTime.WaitForSeconds(Me, 1F)
		End Select
		Return
	End Function

	' Token: 0x060001F7 RID: 503 RVA: 0x0005A85C File Offset: 0x00058C5C
	Private Iterator Function short_cr() As IEnumerator
		While Me.knight.state <> ChessKnightLevelKnight.State.Idle
			Yield Nothing
		End While
		Me.knight.[Short]()
		While Me.knight.state <> ChessKnightLevelKnight.State.Idle
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060001F8 RID: 504 RVA: 0x0005A878 File Offset: 0x00058C78
	Private Iterator Function long_cr() As IEnumerator
		While Me.knight.state <> ChessKnightLevelKnight.State.Idle
			Yield Nothing
		End While
		Me.knight.[Long]()
		While Me.knight.state <> ChessKnightLevelKnight.State.Idle
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060001F9 RID: 505 RVA: 0x0005A894 File Offset: 0x00058C94
	Private Iterator Function up_cr() As IEnumerator
		While Me.knight.state <> ChessKnightLevelKnight.State.Idle
			Yield Nothing
		End While
		Me.knight.Up()
		While Me.knight.state <> ChessKnightLevelKnight.State.Idle
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x04000380 RID: 896
	Private properties As LevelProperties.ChessKnight

	' Token: 0x04000381 RID: 897
	<SerializeField()>
	Private knight As ChessKnightLevelKnight

	' Token: 0x04000382 RID: 898
	<Header("Boss Info")>
	<SerializeField()>
	Private _bossPortraitMain As Sprite

	' Token: 0x04000383 RID: 899
	<SerializeField()>
	Private _bossQuoteMain As String
End Class
