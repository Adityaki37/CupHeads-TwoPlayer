Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000029 RID: 41
Public Class AirshipCrabLevel
	Inherits Level

	' Token: 0x06000076 RID: 118 RVA: 0x00054504 File Offset: 0x00052904
	Protected Overrides Sub PartialInit()
		Me.properties = LevelProperties.AirshipCrab.GetMode(MyBase.mode)
		AddHandler Me.properties.OnStateChange, AddressOf MyBase.zHack_OnStateChanged
		AddHandler Me.properties.OnBossDeath, AddressOf MyBase.zHack_OnWin
		MyBase.timeline = Me.properties.CreateTimeline(MyBase.mode)
		Me.goalTimes = Me.properties.goalTimes
		AddHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		MyBase.PartialInit()
	End Sub

	' Token: 0x1700000E RID: 14
	' (get) Token: 0x06000077 RID: 119 RVA: 0x0005459A File Offset: 0x0005299A
	Public Overrides ReadOnly Property CurrentLevel As Levels
		Get
			Return Levels.AirshipCrab
		End Get
	End Property

	' Token: 0x1700000F RID: 15
	' (get) Token: 0x06000078 RID: 120 RVA: 0x000545A1 File Offset: 0x000529A1
	Public Overrides ReadOnly Property CurrentScene As Scenes
		Get
			Return Scenes.scene_level_airship_crab
		End Get
	End Property

	' Token: 0x17000010 RID: 16
	' (get) Token: 0x06000079 RID: 121 RVA: 0x000545A5 File Offset: 0x000529A5
	Public Overrides ReadOnly Property BossPortrait As Sprite
		Get
			Return Me._bossPortrait
		End Get
	End Property

	' Token: 0x17000011 RID: 17
	' (get) Token: 0x0600007A RID: 122 RVA: 0x000545AD File Offset: 0x000529AD
	Public Overrides ReadOnly Property BossQuote As String
		Get
			Return Me._bossQuote
		End Get
	End Property

	' Token: 0x0600007B RID: 123 RVA: 0x000545B5 File Offset: 0x000529B5
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.crab.LevelInit(Me.properties)
	End Sub

	' Token: 0x0600007C RID: 124 RVA: 0x000545CE File Offset: 0x000529CE
	Protected Overrides Sub OnLevelStart()
		MyBase.StartCoroutine(Me.airshipcrabPattern_cr())
	End Sub

	' Token: 0x0600007D RID: 125 RVA: 0x000545E0 File Offset: 0x000529E0
	Private Iterator Function airshipcrabPattern_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		While True
			Yield MyBase.StartCoroutine(Me.nextPattern_cr())
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600007E RID: 126 RVA: 0x000545FC File Offset: 0x000529FC
	Private Iterator Function nextPattern_cr() As IEnumerator
		Dim p As LevelProperties.AirshipCrab.Pattern = Me.properties.CurrentState.NextPattern
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		Return
	End Function

	' Token: 0x04000125 RID: 293
	Private properties As LevelProperties.AirshipCrab

	' Token: 0x04000126 RID: 294
	<SerializeField()>
	Private crab As AirshipCrabLevelCrab

	' Token: 0x04000127 RID: 295
	<Header("Boss Info")>
	<SerializeField()>
	Private _bossPortrait As Sprite

	' Token: 0x04000128 RID: 296
	<SerializeField()>
	<Multiline()>
	Private _bossQuote As String
End Class
