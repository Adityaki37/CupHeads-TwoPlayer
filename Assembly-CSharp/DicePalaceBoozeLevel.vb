Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020000F9 RID: 249
Public Class DicePalaceBoozeLevel
	Inherits AbstractDicePalaceLevel

	' Token: 0x060002CD RID: 717 RVA: 0x0005E608 File Offset: 0x0005CA08
	Protected Overrides Sub PartialInit()
		Me.properties = LevelProperties.DicePalaceBooze.GetMode(MyBase.mode)
		AddHandler Me.properties.OnStateChange, AddressOf MyBase.zHack_OnStateChanged
		AddHandler Me.properties.OnBossDeath, AddressOf MyBase.zHack_OnWin
		MyBase.timeline = Me.properties.CreateTimeline(MyBase.mode)
		Me.goalTimes = Me.properties.goalTimes
		AddHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		MyBase.PartialInit()
	End Sub

	' Token: 0x17000072 RID: 114
	' (get) Token: 0x060002CE RID: 718 RVA: 0x0005E69E File Offset: 0x0005CA9E
	Public Overrides ReadOnly Property CurrentDicePalaceLevel As DicePalaceLevels
		Get
			Return DicePalaceLevels.DicePalaceBooze
		End Get
	End Property

	' Token: 0x17000073 RID: 115
	' (get) Token: 0x060002CF RID: 719 RVA: 0x0005E6A5 File Offset: 0x0005CAA5
	Public Overrides ReadOnly Property CurrentLevel As Levels
		Get
			Return Levels.DicePalaceBooze
		End Get
	End Property

	' Token: 0x17000074 RID: 116
	' (get) Token: 0x060002D0 RID: 720 RVA: 0x0005E6AC File Offset: 0x0005CAAC
	Public Overrides ReadOnly Property CurrentScene As Scenes
		Get
			Return Scenes.scene_level_dice_palace_booze
		End Get
	End Property

	' Token: 0x17000075 RID: 117
	' (get) Token: 0x060002D1 RID: 721 RVA: 0x0005E6B0 File Offset: 0x0005CAB0
	Public Overrides ReadOnly Property BossPortrait As Sprite
		Get
			Return Me._bossPortrait
		End Get
	End Property

	' Token: 0x17000076 RID: 118
	' (get) Token: 0x060002D2 RID: 722 RVA: 0x0005E6B8 File Offset: 0x0005CAB8
	Public Overrides ReadOnly Property BossQuote As String
		Get
			Return Me._bossQuote
		End Get
	End Property

	' Token: 0x060002D3 RID: 723 RVA: 0x0005E6C0 File Offset: 0x0005CAC0
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.decanter.LevelInit(Me.properties)
		Me.martini.LevelInit(Me.properties)
		Me.tumbler.LevelInit(Me.properties)
		RemoveHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		MyBase.timeline = New Level.Timeline()
		MyBase.timeline.health = Me.properties.CurrentState.decanter.decanterHP + Me.properties.CurrentState.martini.martiniHP + Me.properties.CurrentState.tumbler.tumblerHP
		For Each transform As Transform In Me.lamps
			MyBase.StartCoroutine(Me.lamps_cr(transform))
		Next
	End Sub

	' Token: 0x060002D4 RID: 724 RVA: 0x0005E7A6 File Offset: 0x0005CBA6
	Protected Overrides Sub OnLevelStart()
		MyBase.StartCoroutine(Me.dicepalaceboozePattern_cr())
	End Sub

	' Token: 0x060002D5 RID: 725 RVA: 0x0005E7B5 File Offset: 0x0005CBB5
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me._bossPortrait = Nothing
	End Sub

	' Token: 0x060002D6 RID: 726 RVA: 0x0005E7C4 File Offset: 0x0005CBC4
	Private Iterator Function dicepalaceboozePattern_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		While True
			Yield MyBase.StartCoroutine(Me.nextPattern_cr())
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060002D7 RID: 727 RVA: 0x0005E7E0 File Offset: 0x0005CBE0
	Private Iterator Function nextPattern_cr() As IEnumerator
		Dim p As LevelProperties.DicePalaceBooze.Pattern = Me.properties.CurrentState.NextPattern
		If p <> LevelProperties.DicePalaceBooze.Pattern.[Default] Then
			Yield CupheadTime.WaitForSeconds(Me, 1F)
		Else
			Yield Nothing
		End If
		Return
	End Function

	' Token: 0x060002D8 RID: 728 RVA: 0x0005E7FC File Offset: 0x0005CBFC
	Private Iterator Function lamps_cr(lamp As Transform) As IEnumerator
		Dim t As Single = 0F
		Dim maxSpeed As Single = 0F
		Dim speed As Single = maxSpeed
		While True
			t = 0F
			maxSpeed = Global.UnityEngine.Random.Range(5F, 15F)
			speed = maxSpeed
			While Not CupheadLevelCamera.Current.isShaking
				Yield Nothing
			End While
			Dim movingRight As Boolean = Rand.Bool()
			While speed > 0F
				t = If((Not movingRight), (t - CupheadTime.Delta), (t + CupheadTime.Delta))
				Dim phase As Single = Mathf.Sin(t)
				lamp.localRotation = Quaternion.Euler(New Vector3(0F, 0F, phase * speed))
				speed -= 0.05F
				Yield Nothing
			End While
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x04000537 RID: 1335
	Private properties As LevelProperties.DicePalaceBooze

	' Token: 0x04000538 RID: 1336
	<SerializeField()>
	Private lamps As Transform()

	' Token: 0x04000539 RID: 1337
	<SerializeField()>
	Private decanter As DicePalaceBoozeLevelDecanter

	' Token: 0x0400053A RID: 1338
	<SerializeField()>
	Private martini As DicePalaceBoozeLevelMartini

	' Token: 0x0400053B RID: 1339
	<SerializeField()>
	Private tumbler As DicePalaceBoozeLevelTumbler

	' Token: 0x0400053C RID: 1340
	<Header("Boss Info")>
	<SerializeField()>
	Private _bossPortrait As Sprite

	' Token: 0x0400053D RID: 1341
	<SerializeField()>
	Private _bossQuote As String
End Class
