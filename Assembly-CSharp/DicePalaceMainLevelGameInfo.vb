Imports System
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x020005D4 RID: 1492
Public Class DicePalaceMainLevelGameInfo
	Inherits AbstractMonoBehaviour

	' Token: 0x1700036D RID: 877
	' (get) Token: 0x06001D59 RID: 7513 RVA: 0x0010D010 File Offset: 0x0010B410
	Public Shared ReadOnly Property GameInfo As DicePalaceMainLevelGameInfo
		Get
			If DicePalaceMainLevelGameInfo.gameInfo Is Nothing Then
				DicePalaceMainLevelGameInfo.gameInfo = New GameObject() With { .name = "GameInfo" }.AddComponent(Of DicePalaceMainLevelGameInfo)()
			End If
			Return DicePalaceMainLevelGameInfo.gameInfo
		End Get
	End Property

	' Token: 0x06001D5A RID: 7514 RVA: 0x0010D04E File Offset: 0x0010B44E
	Protected Overrides Sub Awake()
		MyBase.Awake()
		DicePalaceMainLevelGameInfo.gameInfo = Me
		DicePalaceMainLevelGameInfo.IS_FIRST_ENTRY = True
		DicePalaceMainLevelGameInfo.SAFE_INDEXES = New List(Of Integer)()
		DicePalaceMainLevelGameInfo.ChooseHearts()
		Global.UnityEngine.[Object].DontDestroyOnLoad(Me)
	End Sub

	' Token: 0x06001D5B RID: 7515 RVA: 0x0010D078 File Offset: 0x0010B478
	Public Sub CleanUp()
		DicePalaceMainLevelGameInfo.SAFE_INDEXES.Clear()
		DicePalaceMainLevelGameInfo.TURN_COUNTER = 0
		DicePalaceMainLevelGameInfo.PLAYER_SPACES_MOVED = 0
		DicePalaceMainLevelGameInfo.ChooseHearts()
		DicePalaceMainLevelGameInfo.PLAYER_ONE_STATS = Nothing
		DicePalaceMainLevelGameInfo.PLAYER_TWO_STATS = Nothing
		DicePalaceMainLevelGameInfo.PLAYED_INTRO_SFX = False
		DicePalaceMainLevelGameInfo.CHALICE_PLAYER = -1
		DicePalaceMainLevelGameInfo.IS_FIRST_ENTRY = True
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06001D5C RID: 7516 RVA: 0x0010D0C9 File Offset: 0x0010B4C9
	Public Shared Sub CleanUpRetry()
		DicePalaceMainLevelGameInfo.SAFE_INDEXES.Clear()
		DicePalaceMainLevelGameInfo.TURN_COUNTER = 0
		DicePalaceMainLevelGameInfo.PLAYER_SPACES_MOVED = 0
		DicePalaceMainLevelGameInfo.ChooseHearts()
		DicePalaceMainLevelGameInfo.PLAYER_ONE_STATS = Nothing
		DicePalaceMainLevelGameInfo.PLAYER_TWO_STATS = Nothing
		DicePalaceMainLevelGameInfo.PLAYED_INTRO_SFX = False
		DicePalaceMainLevelGameInfo.CHALICE_PLAYER = -1
		DicePalaceMainLevelGameInfo.IS_FIRST_ENTRY = True
	End Sub

	' Token: 0x06001D5D RID: 7517 RVA: 0x0010D104 File Offset: 0x0010B504
	Private Shared Sub ChooseHearts()
		DicePalaceMainLevelGameInfo.HEART_INDEXES(0) = Global.UnityEngine.Random.Range(0, 3)
		DicePalaceMainLevelGameInfo.HEART_INDEXES(1) = Global.UnityEngine.Random.Range(4, 7)
		DicePalaceMainLevelGameInfo.HEART_INDEXES(2) = Global.UnityEngine.Random.Range(8, 11)
	End Sub

	' Token: 0x06001D5E RID: 7518 RVA: 0x0010D134 File Offset: 0x0010B534
	Public Shared Sub SetPlayersStats()
		If DicePalaceMainLevelGameInfo.PLAYER_ONE_STATS Is Nothing Then
			DicePalaceMainLevelGameInfo.PLAYER_ONE_STATS = New PlayersStatsBossesHub()
		End If
		Dim stats As PlayerStatsManager = PlayerManager.GetPlayer(PlayerId.PlayerOne).stats
		DicePalaceMainLevelGameInfo.PLAYER_ONE_STATS.healerHP = stats.HealerHP
		DicePalaceMainLevelGameInfo.PLAYER_ONE_STATS.healerHPReceived = stats.HealerHPReceived
		DicePalaceMainLevelGameInfo.PLAYER_ONE_STATS.healerHPCounter = stats.HealerHPCounter
		DicePalaceMainLevelGameInfo.PLAYER_ONE_STATS.HP = stats.Health
		DicePalaceMainLevelGameInfo.PLAYER_ONE_STATS.SuperCharge = stats.SuperMeter
		If PlayerManager.Multiplayer Then
			If DicePalaceMainLevelGameInfo.PLAYER_TWO_STATS Is Nothing Then
				DicePalaceMainLevelGameInfo.PLAYER_TWO_STATS = New PlayersStatsBossesHub()
			End If
			Dim stats2 As PlayerStatsManager = PlayerManager.GetPlayer(PlayerId.PlayerTwo).stats
			DicePalaceMainLevelGameInfo.PLAYER_TWO_STATS.healerHP = stats2.HealerHP
			DicePalaceMainLevelGameInfo.PLAYER_TWO_STATS.healerHPReceived = stats2.HealerHPReceived
			DicePalaceMainLevelGameInfo.PLAYER_TWO_STATS.healerHPCounter = stats2.HealerHPCounter
			DicePalaceMainLevelGameInfo.PLAYER_TWO_STATS.HP = stats2.Health
			DicePalaceMainLevelGameInfo.PLAYER_TWO_STATS.SuperCharge = stats2.SuperMeter
		End If
	End Sub

	' Token: 0x0400263A RID: 9786
	Private Shared gameInfo As DicePalaceMainLevelGameInfo

	' Token: 0x0400263B RID: 9787
	Public Shared TURN_COUNTER As Integer

	' Token: 0x0400263C RID: 9788
	Public Shared PLAYER_SPACES_MOVED As Integer

	' Token: 0x0400263D RID: 9789
	Public Shared SAFE_INDEXES As List(Of Integer)

	' Token: 0x0400263E RID: 9790
	Public Shared HEART_INDEXES As Integer() = New Integer(2) {}

	' Token: 0x0400263F RID: 9791
	Public Shared PLAYER_ONE_STATS As PlayersStatsBossesHub

	' Token: 0x04002640 RID: 9792
	Public Shared PLAYER_TWO_STATS As PlayersStatsBossesHub

	' Token: 0x04002641 RID: 9793
	Public Shared PLAYED_INTRO_SFX As Boolean

	' Token: 0x04002642 RID: 9794
	Public Shared IS_FIRST_ENTRY As Boolean = True

	' Token: 0x04002643 RID: 9795
	Public Shared CHALICE_PLAYER As Integer = -1
End Class
