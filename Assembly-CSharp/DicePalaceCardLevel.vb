Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000100 RID: 256
Public Class DicePalaceCardLevel
	Inherits AbstractDicePalaceLevel

	' Token: 0x060002E5 RID: 741 RVA: 0x0005EBC4 File Offset: 0x0005CFC4
	Protected Overrides Sub PartialInit()
		Me.properties = LevelProperties.DicePalaceCard.GetMode(MyBase.mode)
		AddHandler Me.properties.OnStateChange, AddressOf MyBase.zHack_OnStateChanged
		AddHandler Me.properties.OnBossDeath, AddressOf MyBase.zHack_OnWin
		MyBase.timeline = Me.properties.CreateTimeline(MyBase.mode)
		Me.goalTimes = Me.properties.goalTimes
		AddHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		MyBase.PartialInit()
	End Sub

	' Token: 0x17000078 RID: 120
	' (get) Token: 0x060002E6 RID: 742 RVA: 0x0005EC5A File Offset: 0x0005D05A
	Public Overrides ReadOnly Property CurrentDicePalaceLevel As DicePalaceLevels
		Get
			Return DicePalaceLevels.DicePalaceCard
		End Get
	End Property

	' Token: 0x17000079 RID: 121
	' (get) Token: 0x060002E7 RID: 743 RVA: 0x0005EC61 File Offset: 0x0005D061
	Public Overrides ReadOnly Property CurrentLevel As Levels
		Get
			Return Levels.DicePalaceCard
		End Get
	End Property

	' Token: 0x1700007A RID: 122
	' (get) Token: 0x060002E8 RID: 744 RVA: 0x0005EC68 File Offset: 0x0005D068
	Public Overrides ReadOnly Property CurrentScene As Scenes
		Get
			Return Scenes.scene_level_dice_palace_card
		End Get
	End Property

	' Token: 0x1700007B RID: 123
	' (get) Token: 0x060002E9 RID: 745 RVA: 0x0005EC6C File Offset: 0x0005D06C
	Public Overrides ReadOnly Property BossPortrait As Sprite
		Get
			Return Me._bossPortrait
		End Get
	End Property

	' Token: 0x1700007C RID: 124
	' (get) Token: 0x060002EA RID: 746 RVA: 0x0005EC74 File Offset: 0x0005D074
	Public Overrides ReadOnly Property BossQuote As String
		Get
			Return Me._bossQuote
		End Get
	End Property

	' Token: 0x060002EB RID: 747 RVA: 0x0005EC7C File Offset: 0x0005D07C
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.card.LevelInit(Me.properties)
		Me.gameManager.GameSetup(Me.properties)
	End Sub

	' Token: 0x060002EC RID: 748 RVA: 0x0005ECA6 File Offset: 0x0005D0A6
	Protected Overrides Sub OnLevelStart()
		MyBase.StartCoroutine(Me.dicepalacecardPattern_cr())
		MyBase.StartCoroutine(Me.gameManager.start_game_cr())
	End Sub

	' Token: 0x060002ED RID: 749 RVA: 0x0005ECC8 File Offset: 0x0005D0C8
	Private Iterator Function dicepalacecardPattern_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		While True
			Yield MyBase.StartCoroutine(Me.nextPattern_cr())
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060002EE RID: 750 RVA: 0x0005ECE4 File Offset: 0x0005D0E4
	Private Iterator Function nextPattern_cr() As IEnumerator
		Dim p As LevelProperties.DicePalaceCard.Pattern = Me.properties.CurrentState.NextPattern
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		Return
	End Function

	' Token: 0x0400054E RID: 1358
	Private properties As LevelProperties.DicePalaceCard

	' Token: 0x0400054F RID: 1359
	<SerializeField()>
	Private gameManager As DicePalaceCardGameManager

	' Token: 0x04000550 RID: 1360
	<SerializeField()>
	Private card As DicePalaceCardLevelCard

	' Token: 0x04000551 RID: 1361
	<Header("Boss Info")>
	<SerializeField()>
	Private _bossPortrait As Sprite

	' Token: 0x04000552 RID: 1362
	<SerializeField()>
	Private _bossQuote As String
End Class
