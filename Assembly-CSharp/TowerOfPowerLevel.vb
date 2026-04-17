Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020002D1 RID: 721
Public Class TowerOfPowerLevel
	Inherits Level

	' Token: 0x060007F9 RID: 2041 RVA: 0x00077CC8 File Offset: 0x000760C8
	Protected Overrides Sub PartialInit()
		Me.properties = LevelProperties.TowerOfPower.GetMode(MyBase.mode)
		AddHandler Me.properties.OnStateChange, AddressOf MyBase.zHack_OnStateChanged
		AddHandler Me.properties.OnBossDeath, AddressOf MyBase.zHack_OnWin
		MyBase.timeline = Me.properties.CreateTimeline(MyBase.mode)
		Me.goalTimes = Me.properties.goalTimes
		AddHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		MyBase.PartialInit()
	End Sub

	' Token: 0x1700014A RID: 330
	' (get) Token: 0x060007FA RID: 2042 RVA: 0x00077D5E File Offset: 0x0007615E
	Public Overrides ReadOnly Property CurrentLevel As Levels
		Get
			Return Levels.TowerOfPower
		End Get
	End Property

	' Token: 0x1700014B RID: 331
	' (get) Token: 0x060007FB RID: 2043 RVA: 0x00077D65 File Offset: 0x00076165
	Public Overrides ReadOnly Property CurrentScene As Scenes
		Get
			Return Scenes.scene_level_tower_of_power
		End Get
	End Property

	' Token: 0x1700014C RID: 332
	' (get) Token: 0x060007FC RID: 2044 RVA: 0x00077D69 File Offset: 0x00076169
	Public Overrides ReadOnly Property BossPortrait As Sprite
		Get
			Return Me._bossPortrait
		End Get
	End Property

	' Token: 0x1700014D RID: 333
	' (get) Token: 0x060007FD RID: 2045 RVA: 0x00077D71 File Offset: 0x00076171
	Public Overrides ReadOnly Property BossQuote As String
		Get
			Return Me._bossQuote
		End Get
	End Property

	' Token: 0x1700014E RID: 334
	' (get) Token: 0x060007FE RID: 2046 RVA: 0x00077D79 File Offset: 0x00076179
	Protected Overrides ReadOnly Property LevelIntroTime As Single
		Get
			Return 0F
		End Get
	End Property

	' Token: 0x1700014F RID: 335
	' (get) Token: 0x060007FF RID: 2047 RVA: 0x00077D80 File Offset: 0x00076180
	Public ReadOnly Property GameManager As TowerOfPowerLevelGameManager
		Get
			Return Me.gameManager
		End Get
	End Property

	' Token: 0x06000800 RID: 2048 RVA: 0x00077D88 File Offset: 0x00076188
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.gameManager.LevelInit(Me.properties)
		For Each abstractPlayerController As AbstractPlayerController In Me.players
			If abstractPlayerController IsNot Nothing Then
				abstractPlayerController.gameObject.SetActive(False)
			End If
		Next
	End Sub

	' Token: 0x06000801 RID: 2049 RVA: 0x00077DE4 File Offset: 0x000761E4
	Protected Overrides Sub Awake()
		MyBase.Awake()
		If TowerOfPowerLevelGameInfo.GameInfo IsNot Nothing Then
			AddHandler Level.Current.OnLoseEvent, AddressOf TowerOfPowerLevelGameInfo.GameInfo.CleanUp
		End If
		AddHandler MyBase.OnLoseEvent, AddressOf Me.ResetScore
	End Sub

	' Token: 0x06000802 RID: 2050 RVA: 0x00077E33 File Offset: 0x00076233
	Protected Overrides Sub OnPlayerJoined(playerId As PlayerId)
		TowerOfPowerLevelGameInfo.InitAddedPlayer(playerId, Me.properties.CurrentState.slotMachine.DefaultStartingToken)
		MyBase.OnPlayerJoined(playerId)
		TowerOfPowerLevelGameInfo.InitEquipment(playerId)
	End Sub

	' Token: 0x06000803 RID: 2051 RVA: 0x00077E5D File Offset: 0x0007625D
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Level.IsTowerOfPowerMain = False
		RemoveHandler MyBase.OnLoseEvent, AddressOf Me.ResetScore
	End Sub

	' Token: 0x06000804 RID: 2052 RVA: 0x00077E7D File Offset: 0x0007627D
	Private Sub ResetScore()
		RemoveHandler MyBase.OnLoseEvent, AddressOf Me.ResetScore
		MyBase.CleanUpScore()
	End Sub

	' Token: 0x06000805 RID: 2053 RVA: 0x00077E97 File Offset: 0x00076297
	Protected Overrides Sub CheckIfInABossesHub()
		MyBase.CheckIfInABossesHub()
		Level.IsTowerOfPower = True
		Level.IsTowerOfPowerMain = True
	End Sub

	' Token: 0x06000806 RID: 2054 RVA: 0x00077EAB File Offset: 0x000762AB
	Protected Overrides Sub OnLevelStart()
		MyBase.StartCoroutine(Me.towerofpowerPattern_cr())
	End Sub

	' Token: 0x06000807 RID: 2055 RVA: 0x00077EBC File Offset: 0x000762BC
	Private Iterator Function towerofpowerPattern_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		While True
			Yield MyBase.StartCoroutine(Me.nextPattern_cr())
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000808 RID: 2056 RVA: 0x00077ED8 File Offset: 0x000762D8
	Private Iterator Function nextPattern_cr() As IEnumerator
		Dim p As LevelProperties.TowerOfPower.Pattern = Me.properties.CurrentState.NextPattern
		Yield Nothing
		Return
	End Function

	' Token: 0x06000809 RID: 2057 RVA: 0x00077EF3 File Offset: 0x000762F3
	Private Sub OnGUI()
	End Sub

	' Token: 0x0400104C RID: 4172
	Private properties As LevelProperties.TowerOfPower

	' Token: 0x0400104D RID: 4173
	<SerializeField()>
	Private gameManager As TowerOfPowerLevelGameManager

	' Token: 0x0400104E RID: 4174
	<Header("Boss Info")>
	<SerializeField()>
	Private _bossPortrait As Sprite

	' Token: 0x0400104F RID: 4175
	<SerializeField()>
	<Multiline()>
	Private _bossQuote As String
End Class
