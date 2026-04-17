Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020002C9 RID: 713
Public Class TestLevel
	Inherits Level

	' Token: 0x060007E1 RID: 2017 RVA: 0x000779C0 File Offset: 0x00075DC0
	Protected Overrides Sub PartialInit()
		Me.properties = LevelProperties.Test.GetMode(MyBase.mode)
		AddHandler Me.properties.OnStateChange, AddressOf MyBase.zHack_OnStateChanged
		AddHandler Me.properties.OnBossDeath, AddressOf MyBase.zHack_OnWin
		MyBase.timeline = Me.properties.CreateTimeline(MyBase.mode)
		Me.goalTimes = Me.properties.goalTimes
		AddHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		MyBase.PartialInit()
	End Sub

	' Token: 0x17000145 RID: 325
	' (get) Token: 0x060007E2 RID: 2018 RVA: 0x00077A56 File Offset: 0x00075E56
	Public Overrides ReadOnly Property CurrentLevel As Levels
		Get
			Return Levels.Test
		End Get
	End Property

	' Token: 0x17000146 RID: 326
	' (get) Token: 0x060007E3 RID: 2019 RVA: 0x00077A59 File Offset: 0x00075E59
	Public Overrides ReadOnly Property CurrentScene As Scenes
		Get
			Return Scenes.scene_level_test
		End Get
	End Property

	' Token: 0x17000147 RID: 327
	' (get) Token: 0x060007E4 RID: 2020 RVA: 0x00077A5D File Offset: 0x00075E5D
	Public Overrides ReadOnly Property BossPortrait As Sprite
		Get
			Return Me._bossPortrait
		End Get
	End Property

	' Token: 0x17000148 RID: 328
	' (get) Token: 0x060007E5 RID: 2021 RVA: 0x00077A65 File Offset: 0x00075E65
	Public Overrides ReadOnly Property BossQuote As String
		Get
			Return Me._bossQuote
		End Get
	End Property

	' Token: 0x060007E6 RID: 2022 RVA: 0x00077A6D File Offset: 0x00075E6D
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.jared.LevelInit(Me.properties)
	End Sub

	' Token: 0x060007E7 RID: 2023 RVA: 0x00077A88 File Offset: 0x00075E88
	Protected Overrides Sub Update()
		MyBase.Update()
		If Input.GetKeyDown(KeyCode.Space) Then
			Dim player As LevelPlayerController = PlayerManager.GetPlayer(Of LevelPlayerController)(PlayerId.PlayerOne)
			player.animationController.SetColorOverTime(Color.blue, 1F)
		End If
	End Sub

	' Token: 0x060007E8 RID: 2024 RVA: 0x00077AC3 File Offset: 0x00075EC3
	Protected Overrides Sub OnLevelStart()
		MyBase.StartCoroutine(Me.testPattern_cr())
	End Sub

	' Token: 0x060007E9 RID: 2025 RVA: 0x00077AD2 File Offset: 0x00075ED2
	Protected Overrides Sub OnStateChanged()
		MyBase.OnStateChanged()
	End Sub

	' Token: 0x060007EA RID: 2026 RVA: 0x00077ADC File Offset: 0x00075EDC
	Private Iterator Function testPattern_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		While True
			Yield MyBase.StartCoroutine(Me.nextPattern_cr())
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060007EB RID: 2027 RVA: 0x00077AF8 File Offset: 0x00075EF8
	Private Iterator Function nextPattern_cr() As IEnumerator
		Dim p As LevelProperties.Test.Pattern = Me.properties.CurrentState.NextPattern
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		Return
	End Function

	' Token: 0x04001028 RID: 4136
	Private properties As LevelProperties.Test

	' Token: 0x04001029 RID: 4137
	<SerializeField()>
	Private jared As TestLevelFlyingJared

	' Token: 0x0400102A RID: 4138
	<Header("Boss Info")>
	<SerializeField()>
	Private _bossPortrait As Sprite

	' Token: 0x0400102B RID: 4139
	<SerializeField()>
	<Multiline()>
	Private _bossQuote As String

	' Token: 0x020004AB RID: 1195
	<Serializable()>
	Public Class Prefabs
	End Class
End Class
