Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000143 RID: 323
Public Class DicePalaceMainLevel
	Inherits AbstractDicePalaceLevel

	' Token: 0x060003A9 RID: 937 RVA: 0x00060728 File Offset: 0x0005EB28
	Protected Overrides Sub PartialInit()
		Me.properties = LevelProperties.DicePalaceMain.GetMode(MyBase.mode)
		AddHandler Me.properties.OnStateChange, AddressOf MyBase.zHack_OnStateChanged
		AddHandler Me.properties.OnBossDeath, AddressOf MyBase.zHack_OnWin
		MyBase.timeline = Me.properties.CreateTimeline(MyBase.mode)
		Me.goalTimes = Me.properties.goalTimes
		AddHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		MyBase.PartialInit()
	End Sub

	' Token: 0x170000A8 RID: 168
	' (get) Token: 0x060003AA RID: 938 RVA: 0x000607BE File Offset: 0x0005EBBE
	Public Overrides ReadOnly Property CurrentDicePalaceLevel As DicePalaceLevels
		Get
			Return DicePalaceLevels.DicePalaceMain
		End Get
	End Property

	' Token: 0x170000A9 RID: 169
	' (get) Token: 0x060003AB RID: 939 RVA: 0x000607C5 File Offset: 0x0005EBC5
	Public Overrides ReadOnly Property CurrentLevel As Levels
		Get
			Return Levels.DicePalaceMain
		End Get
	End Property

	' Token: 0x170000AA RID: 170
	' (get) Token: 0x060003AC RID: 940 RVA: 0x000607CC File Offset: 0x0005EBCC
	Public Overrides ReadOnly Property CurrentScene As Scenes
		Get
			Return Scenes.scene_level_dice_palace_main
		End Get
	End Property

	' Token: 0x170000AB RID: 171
	' (get) Token: 0x060003AD RID: 941 RVA: 0x000607D0 File Offset: 0x0005EBD0
	Public Overrides ReadOnly Property BossPortrait As Sprite
		Get
			Return Me._bossPortrait
		End Get
	End Property

	' Token: 0x170000AC RID: 172
	' (get) Token: 0x060003AE RID: 942 RVA: 0x000607D8 File Offset: 0x0005EBD8
	Public Overrides ReadOnly Property BossQuote As String
		Get
			Return Me._bossQuote
		End Get
	End Property

	' Token: 0x170000AD RID: 173
	' (get) Token: 0x060003AF RID: 943 RVA: 0x000607E0 File Offset: 0x0005EBE0
	Public ReadOnly Property GameManager As DicePalaceMainLevelGameManager
		Get
			Return Me.gameManager
		End Get
	End Property

	' Token: 0x060003B0 RID: 944 RVA: 0x000607E8 File Offset: 0x0005EBE8
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.gameManager.LevelInit(Me.properties)
		Me.kingDice.LevelInit(Me.properties)
		If PlayerManager.GetPlayer(PlayerId.PlayerOne).stats.isChalice Then
			DicePalaceMainLevelGameInfo.CHALICE_PLAYER = 0
		ElseIf PlayerManager.GetPlayer(PlayerId.PlayerTwo) IsNot Nothing AndAlso PlayerManager.GetPlayer(PlayerId.PlayerTwo).stats.isChalice Then
			DicePalaceMainLevelGameInfo.CHALICE_PLAYER = 1
		End If
	End Sub

	' Token: 0x060003B1 RID: 945 RVA: 0x00060869 File Offset: 0x0005EC69
	Protected Overrides Sub OnLevelStart()
		MyBase.StartCoroutine(Me.dicepalacemainPattern_cr())
	End Sub

	' Token: 0x060003B2 RID: 946 RVA: 0x00060878 File Offset: 0x0005EC78
	Protected Overrides Sub CheckIfInABossesHub()
		MyBase.CheckIfInABossesHub()
		If Not Me.isTowerOfPower Then
			Level.IsDicePalaceMain = True
		End If
	End Sub

	' Token: 0x060003B3 RID: 947 RVA: 0x00060891 File Offset: 0x0005EC91
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me._bossPortrait = Nothing
	End Sub

	' Token: 0x060003B4 RID: 948 RVA: 0x000608A0 File Offset: 0x0005ECA0
	Private Iterator Function dicepalacemainPattern_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		While True
			Yield MyBase.StartCoroutine(Me.nextPattern_cr())
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060003B5 RID: 949 RVA: 0x000608BC File Offset: 0x0005ECBC
	Private Iterator Function nextPattern_cr() As IEnumerator
		Dim p As LevelProperties.DicePalaceMain.Pattern = Me.properties.CurrentState.NextPattern
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		Return
	End Function

	' Token: 0x0400064B RID: 1611
	Private properties As LevelProperties.DicePalaceMain

	' Token: 0x0400064C RID: 1612
	<SerializeField()>
	Private gameManager As DicePalaceMainLevelGameManager

	' Token: 0x0400064D RID: 1613
	<SerializeField()>
	Private kingDice As DicePalaceMainLevelKingDice

	' Token: 0x0400064E RID: 1614
	<Header("Boss Info")>
	<SerializeField()>
	Private _bossPortrait As Sprite

	' Token: 0x0400064F RID: 1615
	<SerializeField()>
	Private _bossQuote As String
End Class
