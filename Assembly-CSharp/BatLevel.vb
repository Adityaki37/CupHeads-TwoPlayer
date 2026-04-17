Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000058 RID: 88
Public Class BatLevel
	Inherits Level

	' Token: 0x060000E6 RID: 230 RVA: 0x000558FC File Offset: 0x00053CFC
	Protected Overrides Sub PartialInit()
		Me.properties = LevelProperties.Bat.GetMode(MyBase.mode)
		AddHandler Me.properties.OnStateChange, AddressOf MyBase.zHack_OnStateChanged
		AddHandler Me.properties.OnBossDeath, AddressOf MyBase.zHack_OnWin
		MyBase.timeline = Me.properties.CreateTimeline(MyBase.mode)
		Me.goalTimes = Me.properties.goalTimes
		AddHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		MyBase.PartialInit()
	End Sub

	' Token: 0x17000023 RID: 35
	' (get) Token: 0x060000E7 RID: 231 RVA: 0x00055992 File Offset: 0x00053D92
	Public Overrides ReadOnly Property CurrentLevel As Levels
		Get
			Return Levels.Bat
		End Get
	End Property

	' Token: 0x17000024 RID: 36
	' (get) Token: 0x060000E8 RID: 232 RVA: 0x00055995 File Offset: 0x00053D95
	Public Overrides ReadOnly Property CurrentScene As Scenes
		Get
			Return Scenes.scene_level_bat
		End Get
	End Property

	' Token: 0x17000025 RID: 37
	' (get) Token: 0x060000E9 RID: 233 RVA: 0x00055999 File Offset: 0x00053D99
	Public Overrides ReadOnly Property BossPortrait As Sprite
		Get
			Return Me._bossPortrait
		End Get
	End Property

	' Token: 0x17000026 RID: 38
	' (get) Token: 0x060000EA RID: 234 RVA: 0x000559A1 File Offset: 0x00053DA1
	Public Overrides ReadOnly Property BossQuote As String
		Get
			Return Me._bossQuote
		End Get
	End Property

	' Token: 0x060000EB RID: 235 RVA: 0x000559A9 File Offset: 0x00053DA9
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.bat.LevelInit(Me.properties)
	End Sub

	' Token: 0x060000EC RID: 236 RVA: 0x000559C2 File Offset: 0x00053DC2
	Protected Overrides Sub OnLevelStart()
		MyBase.OnLevelStart()
		MyBase.StartCoroutine(Me.batPattern_cr())
		MyBase.StartCoroutine(Me.goblins_cr())
	End Sub

	' Token: 0x060000ED RID: 237 RVA: 0x000559E4 File Offset: 0x00053DE4
	Protected Overrides Sub OnStateChanged()
		MyBase.OnStateChanged()
		If Me.properties.CurrentState.stateName = LevelProperties.Bat.States.Coffin Then
			Me.StopAllCoroutines()
			MyBase.StartCoroutine(Me.phase_2_cr())
		ElseIf Me.properties.CurrentState.stateName = LevelProperties.Bat.States.Wolf Then
			Me.StopAllCoroutines()
			MyBase.StartCoroutine(Me.phase_3_cr())
		End If
	End Sub

	' Token: 0x060000EE RID: 238 RVA: 0x00055A50 File Offset: 0x00053E50
	Private Iterator Function batPattern_cr() As IEnumerator
		Yield New WaitForSeconds(1F)
		While True
			Yield MyBase.StartCoroutine(Me.nextPattern_cr())
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060000EF RID: 239 RVA: 0x00055A6C File Offset: 0x00053E6C
	Private Iterator Function nextPattern_cr() As IEnumerator
		Dim p As LevelProperties.Bat.Pattern = Me.properties.CurrentState.NextPattern
		If p <> LevelProperties.Bat.Pattern.Bouncer Then
			If p <> LevelProperties.Bat.Pattern.Lightning Then
				Yield New WaitForSeconds(1F)
			Else
				Yield MyBase.StartCoroutine(Me.lightning_cr())
			End If
		Else
			Yield MyBase.StartCoroutine(Me.bouncer_cr())
		End If
		Return
	End Function

	' Token: 0x060000F0 RID: 240 RVA: 0x00055A88 File Offset: 0x00053E88
	Private Iterator Function bouncer_cr() As IEnumerator
		While Me.bat.state <> BatLevelBat.State.Idle
			Yield Nothing
		End While
		Me.bat.StartBouncer()
		While Me.bat.state <> BatLevelBat.State.Idle
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060000F1 RID: 241 RVA: 0x00055AA4 File Offset: 0x00053EA4
	Private Iterator Function lightning_cr() As IEnumerator
		While Me.bat.state <> BatLevelBat.State.Idle
			Yield Nothing
		End While
		Me.bat.StartLightning()
		While Me.bat.state <> BatLevelBat.State.Idle
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060000F2 RID: 242 RVA: 0x00055AC0 File Offset: 0x00053EC0
	Private Iterator Function phase_2_cr() As IEnumerator
		Me.bat.StartPhase2()
		Yield Nothing
		Return
	End Function

	' Token: 0x060000F3 RID: 243 RVA: 0x00055ADC File Offset: 0x00053EDC
	Private Iterator Function phase_3_cr() As IEnumerator
		Me.bat.StartPhase3()
		Yield Nothing
		Return
	End Function

	' Token: 0x060000F4 RID: 244 RVA: 0x00055AF8 File Offset: 0x00053EF8
	Private Iterator Function goblins_cr() As IEnumerator
		If Not Me.properties.CurrentState.goblins.Enabled Then
			Yield Nothing
		Else
			Me.bat.StartGoblin()
		End If
		Yield Nothing
		Return
	End Function

	' Token: 0x04000210 RID: 528
	Private properties As LevelProperties.Bat

	' Token: 0x04000211 RID: 529
	<Space(10F)>
	<SerializeField()>
	Private bat As BatLevelBat

	' Token: 0x04000212 RID: 530
	<Header("Boss Info")>
	<SerializeField()>
	Private _bossPortrait As Sprite

	' Token: 0x04000213 RID: 531
	<SerializeField()>
	<Multiline()>
	Private _bossQuote As String

	' Token: 0x020004FF RID: 1279
	<Serializable()>
	Public Class Prefabs
	End Class
End Class
