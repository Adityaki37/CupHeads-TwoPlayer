Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020001F5 RID: 501
Public Class GraveyardLevel
	Inherits Level

	' Token: 0x06000587 RID: 1415 RVA: 0x00068FC4 File Offset: 0x000673C4
	Protected Overrides Sub PartialInit()
		Me.properties = LevelProperties.Graveyard.GetMode(MyBase.mode)
		AddHandler Me.properties.OnStateChange, AddressOf MyBase.zHack_OnStateChanged
		AddHandler Me.properties.OnBossDeath, AddressOf MyBase.zHack_OnWin
		MyBase.timeline = Me.properties.CreateTimeline(MyBase.mode)
		Me.goalTimes = Me.properties.goalTimes
		AddHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		MyBase.PartialInit()
	End Sub

	' Token: 0x170000F8 RID: 248
	' (get) Token: 0x06000588 RID: 1416 RVA: 0x0006905A File Offset: 0x0006745A
	Public Overrides ReadOnly Property CurrentLevel As Levels
		Get
			Return Levels.Graveyard
		End Get
	End Property

	' Token: 0x170000F9 RID: 249
	' (get) Token: 0x06000589 RID: 1417 RVA: 0x00069061 File Offset: 0x00067461
	Public Overrides ReadOnly Property CurrentScene As Scenes
		Get
			Return Scenes.scene_level_graveyard
		End Get
	End Property

	' Token: 0x170000FA RID: 250
	' (get) Token: 0x0600058A RID: 1418 RVA: 0x00069065 File Offset: 0x00067465
	Public Overrides ReadOnly Property BossPortrait As Sprite
		Get
			Return Me._bossPortraitMain
		End Get
	End Property

	' Token: 0x170000FB RID: 251
	' (get) Token: 0x0600058B RID: 1419 RVA: 0x0006906D File Offset: 0x0006746D
	Public Overrides ReadOnly Property BossQuote As String
		Get
			Return Me._bossQuoteMain
		End Get
	End Property

	' Token: 0x0600058C RID: 1420 RVA: 0x00069075 File Offset: 0x00067475
	Protected Overrides Sub Awake()
		Me.originalMode = Level.CurrentMode
		Level.SetCurrentMode(Level.Mode.Normal)
		MyBase.Awake()
		Level.IsGraveyard = True
	End Sub

	' Token: 0x0600058D RID: 1421 RVA: 0x00069094 File Offset: 0x00067494
	Protected Overrides Sub Start()
		MyBase.Start()
		For i As Integer = 0 To Me.splitDevil.Length - 1
			Me.splitDevil(i).LevelInit(Me.properties)
		Next
		Me.attackCounterString = New PatternString(Me.properties.CurrentState.splitDevilBeam.attacksBeforeBeamString, True)
		Me.attackCounter = Me.attackCounterString.PopInt()
		AudioManager.PlayLoop("sfx_dlc_graveyard_amb_loop")
	End Sub

	' Token: 0x0600058E RID: 1422 RVA: 0x0006910F File Offset: 0x0006750F
	Protected Overrides Sub PlayAnnouncerReady()
		AudioManager.Play("level_announcer_ready_ghostly")
	End Sub

	' Token: 0x0600058F RID: 1423 RVA: 0x0006911B File Offset: 0x0006751B
	Protected Overrides Sub PlayAnnouncerBegin()
		AudioManager.Play("level_announcer_begin_ghostly")
	End Sub

	' Token: 0x06000590 RID: 1424 RVA: 0x00069127 File Offset: 0x00067527
	Public Function CheckForBeamAttack() As Boolean
		Me.attackCounter -= 1
		If Me.attackCounter = -1 Then
			Me.attackCounter = Me.attackCounterString.PopInt()
			Return True
		End If
		Return False
	End Function

	' Token: 0x06000591 RID: 1425 RVA: 0x00069158 File Offset: 0x00067558
	Protected Overrides Sub OnLevelStart()
		For i As Integer = 0 To Me.splitDevil.Length - 1
			Me.splitDevil(i).NextPattern()
		Next
	End Sub

	' Token: 0x06000592 RID: 1426 RVA: 0x0006918C File Offset: 0x0006758C
	Protected Overrides Sub OnWin()
		MyBase.OnWin()
		For i As Integer = 0 To Me.splitDevil.Length - 1
			Me.splitDevil(i).Die()
		Next
	End Sub

	' Token: 0x06000593 RID: 1427 RVA: 0x000691C5 File Offset: 0x000675C5
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me._bossPortraitMain = Nothing
		Level.SetCurrentMode(Me.originalMode)
		Me.splitDevil = Nothing
	End Sub

	' Token: 0x06000594 RID: 1428 RVA: 0x000691E8 File Offset: 0x000675E8
	Private Iterator Function devilPattern_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		While True
			Yield MyBase.StartCoroutine(Me.nextPattern_cr())
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000595 RID: 1429 RVA: 0x00069204 File Offset: 0x00067604
	Private Iterator Function nextPattern_cr() As IEnumerator
		Dim p As LevelProperties.Graveyard.Pattern = Me.properties.CurrentState.NextPattern
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		Return
	End Function

	' Token: 0x04000A68 RID: 2664
	Private properties As LevelProperties.Graveyard

	' Token: 0x04000A69 RID: 2665
	<SerializeField()>
	Private splitDevil As GraveyardLevelSplitDevil()

	' Token: 0x04000A6A RID: 2666
	Private attackCounterString As PatternString

	' Token: 0x04000A6B RID: 2667
	Private attackCounter As Integer

	' Token: 0x04000A6C RID: 2668
	Private originalMode As Level.Mode

	' Token: 0x04000A6D RID: 2669
	<Header("Boss Info")>
	<SerializeField()>
	Private _bossPortraitMain As Sprite

	' Token: 0x04000A6E RID: 2670
	<SerializeField()>
	Private _bossQuoteMain As String
End Class
