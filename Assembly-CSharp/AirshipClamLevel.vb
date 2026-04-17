Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200001F RID: 31
Public Class AirshipClamLevel
	Inherits Level

	' Token: 0x0600005B RID: 91 RVA: 0x0005400C File Offset: 0x0005240C
	Protected Overrides Sub PartialInit()
		Me.properties = LevelProperties.AirshipClam.GetMode(MyBase.mode)
		AddHandler Me.properties.OnStateChange, AddressOf MyBase.zHack_OnStateChanged
		AddHandler Me.properties.OnBossDeath, AddressOf MyBase.zHack_OnWin
		MyBase.timeline = Me.properties.CreateTimeline(MyBase.mode)
		Me.goalTimes = Me.properties.goalTimes
		AddHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		MyBase.PartialInit()
	End Sub

	' Token: 0x17000009 RID: 9
	' (get) Token: 0x0600005C RID: 92 RVA: 0x000540A2 File Offset: 0x000524A2
	Public Overrides ReadOnly Property CurrentLevel As Levels
		Get
			Return Levels.AirshipClam
		End Get
	End Property

	' Token: 0x1700000A RID: 10
	' (get) Token: 0x0600005D RID: 93 RVA: 0x000540A9 File Offset: 0x000524A9
	Public Overrides ReadOnly Property CurrentScene As Scenes
		Get
			Return Scenes.scene_level_airship_clam
		End Get
	End Property

	' Token: 0x1700000B RID: 11
	' (get) Token: 0x0600005E RID: 94 RVA: 0x000540AD File Offset: 0x000524AD
	Public Overrides ReadOnly Property BossPortrait As Sprite
		Get
			Return Me._bossPortrait
		End Get
	End Property

	' Token: 0x1700000C RID: 12
	' (get) Token: 0x0600005F RID: 95 RVA: 0x000540B5 File Offset: 0x000524B5
	Public Overrides ReadOnly Property BossQuote As String
		Get
			Return Me._bossQuote
		End Get
	End Property

	' Token: 0x06000060 RID: 96 RVA: 0x000540BD File Offset: 0x000524BD
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.clam.LevelInit(Me.properties)
	End Sub

	' Token: 0x06000061 RID: 97 RVA: 0x000540D6 File Offset: 0x000524D6
	Protected Overrides Sub OnLevelStart()
		MyBase.StartCoroutine(Me.airshipclamPattern_cr())
	End Sub

	' Token: 0x06000062 RID: 98 RVA: 0x000540E8 File Offset: 0x000524E8
	Private Iterator Function airshipclamPattern_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		While True
			Yield MyBase.StartCoroutine(Me.nextPattern_cr())
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000063 RID: 99 RVA: 0x00054104 File Offset: 0x00052504
	Private Iterator Function nextPattern_cr() As IEnumerator
		Dim p As LevelProperties.AirshipClam.Pattern = Me.properties.CurrentState.NextPattern
		If p <> LevelProperties.AirshipClam.Pattern.Spit Then
			If p <> LevelProperties.AirshipClam.Pattern.Barnacles Then
				Yield CupheadTime.WaitForSeconds(Me, 1F)
			Else
				MyBase.StartCoroutine(Me.barnacles_cr())
			End If
		Else
			MyBase.StartCoroutine(Me.spit_cr())
		End If
		Return
	End Function

	' Token: 0x06000064 RID: 100 RVA: 0x00054120 File Offset: 0x00052520
	Private Iterator Function spit_cr() As IEnumerator
		If Not Me.attacking Then
			Me.clam.OnSpitStart(AddressOf Me.EndAttack)
			Me.attacking = True
		End If
		While Me.attacking
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000065 RID: 101 RVA: 0x0005413C File Offset: 0x0005253C
	Private Iterator Function barnacles_cr() As IEnumerator
		If Not Me.attacking Then
			Me.clam.OnBarnaclesStart(AddressOf Me.EndAttack)
			Me.attacking = True
		End If
		While Me.attacking
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000066 RID: 102 RVA: 0x00054157 File Offset: 0x00052557
	Private Sub EndAttack()
		Me.attacking = False
	End Sub

	' Token: 0x04000100 RID: 256
	Private properties As LevelProperties.AirshipClam

	' Token: 0x04000101 RID: 257
	<SerializeField()>
	Private clam As AirshipClamLevelClam

	' Token: 0x04000102 RID: 258
	Private attacking As Boolean

	' Token: 0x04000103 RID: 259
	<Header("Boss Info")>
	<SerializeField()>
	Private _bossPortrait As Sprite

	' Token: 0x04000104 RID: 260
	<SerializeField()>
	<Multiline()>
	Private _bossQuote As String
End Class
