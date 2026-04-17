Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200013B RID: 315
Public Class DicePalaceLightLevel
	Inherits AbstractDicePalaceLevel

	' Token: 0x06000392 RID: 914 RVA: 0x00060474 File Offset: 0x0005E874
	Protected Overrides Sub PartialInit()
		Me.properties = LevelProperties.DicePalaceLight.GetMode(MyBase.mode)
		AddHandler Me.properties.OnStateChange, AddressOf MyBase.zHack_OnStateChanged
		AddHandler Me.properties.OnBossDeath, AddressOf MyBase.zHack_OnWin
		MyBase.timeline = Me.properties.CreateTimeline(MyBase.mode)
		Me.goalTimes = Me.properties.goalTimes
		AddHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		MyBase.PartialInit()
	End Sub

	' Token: 0x170000A2 RID: 162
	' (get) Token: 0x06000393 RID: 915 RVA: 0x0006050A File Offset: 0x0005E90A
	Public Overrides ReadOnly Property CurrentDicePalaceLevel As DicePalaceLevels
		Get
			Return DicePalaceLevels.DicePalaceLight
		End Get
	End Property

	' Token: 0x170000A3 RID: 163
	' (get) Token: 0x06000394 RID: 916 RVA: 0x00060511 File Offset: 0x0005E911
	Public Overrides ReadOnly Property CurrentLevel As Levels
		Get
			Return Levels.DicePalaceLight
		End Get
	End Property

	' Token: 0x170000A4 RID: 164
	' (get) Token: 0x06000395 RID: 917 RVA: 0x00060518 File Offset: 0x0005E918
	Public Overrides ReadOnly Property CurrentScene As Scenes
		Get
			Return Scenes.scene_level_dice_palace_light
		End Get
	End Property

	' Token: 0x170000A5 RID: 165
	' (get) Token: 0x06000396 RID: 918 RVA: 0x0006051C File Offset: 0x0005E91C
	Public Overrides ReadOnly Property BossPortrait As Sprite
		Get
			Return Me._bossPortrait
		End Get
	End Property

	' Token: 0x170000A6 RID: 166
	' (get) Token: 0x06000397 RID: 919 RVA: 0x00060524 File Offset: 0x0005E924
	Public Overrides ReadOnly Property BossQuote As String
		Get
			Return Me._bossQuote
		End Get
	End Property

	' Token: 0x06000398 RID: 920 RVA: 0x0006052C File Offset: 0x0005E92C
	Protected Overrides Sub Start()
		MyBase.Start()
	End Sub

	' Token: 0x06000399 RID: 921 RVA: 0x00060534 File Offset: 0x0005E934
	Protected Overrides Sub OnLevelStart()
		MyBase.StartCoroutine(Me.dicepalacelightPattern_cr())
	End Sub

	' Token: 0x0600039A RID: 922 RVA: 0x00060544 File Offset: 0x0005E944
	Private Iterator Function dicepalacelightPattern_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		While True
			Yield MyBase.StartCoroutine(Me.nextPattern_cr())
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600039B RID: 923 RVA: 0x00060560 File Offset: 0x0005E960
	Private Iterator Function nextPattern_cr() As IEnumerator
		Dim p As LevelProperties.DicePalaceLight.Pattern = Me.properties.CurrentState.NextPattern
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		Return
	End Function

	' Token: 0x04000630 RID: 1584
	Private properties As LevelProperties.DicePalaceLight

	' Token: 0x04000631 RID: 1585
	<SerializeField()>
	Private lightBoss As RumRunnersLevelWorm

	' Token: 0x04000632 RID: 1586
	<SerializeField()>
	Private _bossPortrait As Sprite

	' Token: 0x04000633 RID: 1587
	<SerializeField()>
	Private _bossQuote As String
End Class
