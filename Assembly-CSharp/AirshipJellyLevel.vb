Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000030 RID: 48
Public Class AirshipJellyLevel
	Inherits Level

	' Token: 0x0600008B RID: 139 RVA: 0x000547C4 File Offset: 0x00052BC4
	Protected Overrides Sub PartialInit()
		Me.properties = LevelProperties.AirshipJelly.GetMode(MyBase.mode)
		AddHandler Me.properties.OnStateChange, AddressOf MyBase.zHack_OnStateChanged
		AddHandler Me.properties.OnBossDeath, AddressOf MyBase.zHack_OnWin
		MyBase.timeline = Me.properties.CreateTimeline(MyBase.mode)
		Me.goalTimes = Me.properties.goalTimes
		AddHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		MyBase.PartialInit()
	End Sub

	' Token: 0x17000013 RID: 19
	' (get) Token: 0x0600008C RID: 140 RVA: 0x0005485A File Offset: 0x00052C5A
	Public Overrides ReadOnly Property CurrentLevel As Levels
		Get
			Return Levels.AirshipJelly
		End Get
	End Property

	' Token: 0x17000014 RID: 20
	' (get) Token: 0x0600008D RID: 141 RVA: 0x0005485D File Offset: 0x00052C5D
	Public Overrides ReadOnly Property CurrentScene As Scenes
		Get
			Return Scenes.scene_level_airship_jelly
		End Get
	End Property

	' Token: 0x17000015 RID: 21
	' (get) Token: 0x0600008E RID: 142 RVA: 0x00054861 File Offset: 0x00052C61
	Public Overrides ReadOnly Property BossPortrait As Sprite
		Get
			Return Me._bossPortrait
		End Get
	End Property

	' Token: 0x17000016 RID: 22
	' (get) Token: 0x0600008F RID: 143 RVA: 0x00054869 File Offset: 0x00052C69
	Public Overrides ReadOnly Property BossQuote As String
		Get
			Return Me._bossQuote
		End Get
	End Property

	' Token: 0x06000090 RID: 144 RVA: 0x00054871 File Offset: 0x00052C71
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.jelly.LevelInit(Me.properties)
	End Sub

	' Token: 0x06000091 RID: 145 RVA: 0x0005488A File Offset: 0x00052C8A
	Protected Overrides Sub OnLevelStart()
	End Sub

	' Token: 0x06000092 RID: 146 RVA: 0x0005488C File Offset: 0x00052C8C
	Private Iterator Function airshipPattern_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		While True
			Yield MyBase.StartCoroutine(Me.nextPattern_cr())
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000093 RID: 147 RVA: 0x000548A8 File Offset: 0x00052CA8
	Private Iterator Function nextPattern_cr() As IEnumerator
		Dim p As LevelProperties.AirshipJelly.Pattern = Me.properties.CurrentState.NextPattern
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		Return
	End Function

	' Token: 0x04000136 RID: 310
	Private properties As LevelProperties.AirshipJelly

	' Token: 0x04000137 RID: 311
	<SerializeField()>
	Private jelly As AirshipJellyLevelJelly

	' Token: 0x04000138 RID: 312
	<Header("Boss Info")>
	<SerializeField()>
	Private _bossPortrait As Sprite

	' Token: 0x04000139 RID: 313
	<SerializeField()>
	<Multiline()>
	Private _bossQuote As String

	' Token: 0x020004D3 RID: 1235
	<Serializable()>
	Public Class Prefabs
	End Class
End Class
