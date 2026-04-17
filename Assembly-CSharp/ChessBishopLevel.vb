Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000078 RID: 120
Public Class ChessBishopLevel
	Inherits ChessLevel

	' Token: 0x0600014D RID: 333 RVA: 0x00057BE0 File Offset: 0x00055FE0
	Protected Overrides Sub PartialInit()
		Me.properties = LevelProperties.ChessBishop.GetMode(MyBase.mode)
		AddHandler Me.properties.OnStateChange, AddressOf MyBase.zHack_OnStateChanged
		AddHandler Me.properties.OnBossDeath, AddressOf MyBase.zHack_OnWin
		MyBase.timeline = Me.properties.CreateTimeline(MyBase.mode)
		Me.goalTimes = Me.properties.goalTimes
		AddHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		MyBase.PartialInit()
	End Sub

	' Token: 0x17000034 RID: 52
	' (get) Token: 0x0600014E RID: 334 RVA: 0x00057C76 File Offset: 0x00056076
	Public Overrides ReadOnly Property CurrentLevel As Levels
		Get
			Return Levels.ChessBishop
		End Get
	End Property

	' Token: 0x17000035 RID: 53
	' (get) Token: 0x0600014F RID: 335 RVA: 0x00057C7D File Offset: 0x0005607D
	Public Overrides ReadOnly Property CurrentScene As Scenes
		Get
			Return Scenes.scene_level_chess_bishop
		End Get
	End Property

	' Token: 0x17000036 RID: 54
	' (get) Token: 0x06000150 RID: 336 RVA: 0x00057C81 File Offset: 0x00056081
	Public Overrides ReadOnly Property BossPortrait As Sprite
		Get
			Return Me._bossPortrait
		End Get
	End Property

	' Token: 0x17000037 RID: 55
	' (get) Token: 0x06000151 RID: 337 RVA: 0x00057C89 File Offset: 0x00056089
	Public Overrides ReadOnly Property BossQuote As String
		Get
			Return Me._bossQuote
		End Get
	End Property

	' Token: 0x06000152 RID: 338 RVA: 0x00057C91 File Offset: 0x00056091
	Protected Overrides Sub Start()
		Level.IsChessBoss = True
		MyBase.Start()
		Me.bishop.LevelInit(Me.properties)
	End Sub

	' Token: 0x06000153 RID: 339 RVA: 0x00057CB0 File Offset: 0x000560B0
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.bishop = Nothing
		Me._bossPortrait = Nothing
	End Sub

	' Token: 0x06000154 RID: 340 RVA: 0x00057CC6 File Offset: 0x000560C6
	Protected Overrides Sub OnStateChanged()
		MyBase.OnStateChanged()
		Me.bishop.StartNewPhase()
	End Sub

	' Token: 0x06000155 RID: 341 RVA: 0x00057CDC File Offset: 0x000560DC
	Private Iterator Function bishopPattern_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		While True
			Yield MyBase.StartCoroutine(Me.nextPattern_cr())
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000156 RID: 342 RVA: 0x00057CF8 File Offset: 0x000560F8
	Private Iterator Function nextPattern_cr() As IEnumerator
		Dim p As LevelProperties.ChessBishop.Pattern = Me.properties.CurrentState.NextPattern
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		Return
	End Function

	' Token: 0x040002B3 RID: 691
	Private properties As LevelProperties.ChessBishop

	' Token: 0x040002B4 RID: 692
	<SerializeField()>
	Private bishop As ChessBishopLevelBishop

	' Token: 0x040002B5 RID: 693
	<Header("Boss Info")>
	<SerializeField()>
	Private _bossPortrait As Sprite

	' Token: 0x040002B6 RID: 694
	<SerializeField()>
	<Multiline()>
	Private _bossQuote As String
End Class
