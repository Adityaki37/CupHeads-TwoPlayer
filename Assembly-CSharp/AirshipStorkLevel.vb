Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000039 RID: 57
Public Class AirshipStorkLevel
	Inherits Level

	' Token: 0x060000A2 RID: 162 RVA: 0x00054A78 File Offset: 0x00052E78
	Protected Overrides Sub PartialInit()
		Me.properties = LevelProperties.AirshipStork.GetMode(MyBase.mode)
		AddHandler Me.properties.OnStateChange, AddressOf MyBase.zHack_OnStateChanged
		AddHandler Me.properties.OnBossDeath, AddressOf MyBase.zHack_OnWin
		MyBase.timeline = Me.properties.CreateTimeline(MyBase.mode)
		Me.goalTimes = Me.properties.goalTimes
		AddHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		MyBase.PartialInit()
	End Sub

	' Token: 0x17000018 RID: 24
	' (get) Token: 0x060000A3 RID: 163 RVA: 0x00054B0E File Offset: 0x00052F0E
	Public Overrides ReadOnly Property CurrentLevel As Levels
		Get
			Return Levels.AirshipStork
		End Get
	End Property

	' Token: 0x17000019 RID: 25
	' (get) Token: 0x060000A4 RID: 164 RVA: 0x00054B15 File Offset: 0x00052F15
	Public Overrides ReadOnly Property CurrentScene As Scenes
		Get
			Return Scenes.scene_level_airship_stork
		End Get
	End Property

	' Token: 0x1700001A RID: 26
	' (get) Token: 0x060000A5 RID: 165 RVA: 0x00054B19 File Offset: 0x00052F19
	Public Overrides ReadOnly Property BossPortrait As Sprite
		Get
			Return Me._bossPortrait
		End Get
	End Property

	' Token: 0x1700001B RID: 27
	' (get) Token: 0x060000A6 RID: 166 RVA: 0x00054B21 File Offset: 0x00052F21
	Public Overrides ReadOnly Property BossQuote As String
		Get
			Return Me._bossQuote
		End Get
	End Property

	' Token: 0x060000A7 RID: 167 RVA: 0x00054B29 File Offset: 0x00052F29
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.stork.LevelInit(Me.properties)
	End Sub

	' Token: 0x060000A8 RID: 168 RVA: 0x00054B42 File Offset: 0x00052F42
	Protected Overrides Sub OnLevelStart()
		MyBase.StartCoroutine(Me.airshipstorkPattern_cr())
	End Sub

	' Token: 0x060000A9 RID: 169 RVA: 0x00054B54 File Offset: 0x00052F54
	Private Iterator Function airshipstorkPattern_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		While True
			Yield MyBase.StartCoroutine(Me.nextPattern_cr())
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060000AA RID: 170 RVA: 0x00054B70 File Offset: 0x00052F70
	Private Iterator Function nextPattern_cr() As IEnumerator
		Dim p As LevelProperties.AirshipStork.Pattern = Me.properties.CurrentState.NextPattern
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		Return
	End Function

	' Token: 0x04000158 RID: 344
	Private properties As LevelProperties.AirshipStork

	' Token: 0x04000159 RID: 345
	<SerializeField()>
	Private stork As AirshipStorkLevelStork

	' Token: 0x0400015A RID: 346
	<Header("Boss Info")>
	<SerializeField()>
	Private _bossPortrait As Sprite

	' Token: 0x0400015B RID: 347
	<SerializeField()>
	<Multiline()>
	Private _bossQuote As String
End Class
