Imports System
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x02000802 RID: 2050
Public Class TowerOfPowerLevelGameInfo
	Inherits AbstractMonoBehaviour

	' Token: 0x17000418 RID: 1048
	' (get) Token: 0x06002F5F RID: 12127 RVA: 0x001C1620 File Offset: 0x001BFA20
	Public Shared ReadOnly Property GameInfo As TowerOfPowerLevelGameInfo
		Get
			If TowerOfPowerLevelGameInfo.gameInfo Is Nothing Then
				TowerOfPowerLevelGameInfo.gameInfo = New GameObject() With { .name = "GameInfo" }.AddComponent(Of TowerOfPowerLevelGameInfo)()
			End If
			Return TowerOfPowerLevelGameInfo.gameInfo
		End Get
	End Property

	' Token: 0x06002F60 RID: 12128 RVA: 0x001C165E File Offset: 0x001BFA5E
	Protected Overrides Sub Awake()
		MyBase.Awake()
		TowerOfPowerLevelGameInfo.gameInfo = Me
		TowerOfPowerLevelGameInfo.PLAYER_STATS(0) = Nothing
		TowerOfPowerLevelGameInfo.PLAYER_STATS(1) = Nothing
		Global.UnityEngine.[Object].DontDestroyOnLoad(Me)
	End Sub

	' Token: 0x06002F61 RID: 12129 RVA: 0x001C1682 File Offset: 0x001BFA82
	Public Sub CleanUp()
		TowerOfPowerLevelGameInfo.TURN_COUNTER = 0
		TowerOfPowerLevelGameInfo.ResetWeapons(PlayerId.PlayerOne)
		If PlayerManager.Multiplayer Then
			TowerOfPowerLevelGameInfo.ResetWeapons(PlayerId.PlayerTwo)
		End If
		TowerOfPowerLevelGameInfo.PLAYER_STATS(0) = Nothing
		TowerOfPowerLevelGameInfo.PLAYER_STATS(1) = Nothing
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06002F62 RID: 12130 RVA: 0x001C16BB File Offset: 0x001BFABB
	Public Shared Sub ResetTowerOfPower()
		TowerOfPowerLevelGameInfo.TURN_COUNTER = 0
		TowerOfPowerLevelGameInfo.CURRENT_TURN = 0
		TowerOfPowerLevelGameInfo.ResetWeapons(PlayerId.PlayerOne)
		If PlayerManager.Multiplayer Then
			TowerOfPowerLevelGameInfo.ResetWeapons(PlayerId.PlayerTwo)
		End If
		TowerOfPowerLevelGameInfo.PLAYER_STATS(0) = Nothing
		TowerOfPowerLevelGameInfo.PLAYER_STATS(1) = Nothing
	End Sub

	' Token: 0x06002F63 RID: 12131 RVA: 0x001C16F0 File Offset: 0x001BFAF0
	Public Shared Sub InitAddedPlayer(playerId As PlayerId, startingToken As Integer)
		TowerOfPowerLevelGameInfo.PLAYER_STATS(CInt(playerId)) = New PlayersStatsBossesHub()
		If TowerOfPowerLevelGameInfo.CURRENT_TURN = 0 Then
			TowerOfPowerLevelGameInfo.PLAYER_STATS(CInt(playerId)).HP = 3
			TowerOfPowerLevelGameInfo.PLAYER_STATS(CInt(playerId)).BonusHP = 3
			TowerOfPowerLevelGameInfo.PLAYER_STATS(CInt(playerId)).SuperCharge = 0F
			TowerOfPowerLevelGameInfo.PLAYER_STATS(CInt(playerId)).tokenCount = startingToken
		Else
			TowerOfPowerLevelGameInfo.PLAYER_STATS(CInt(playerId)).HP = 1
			TowerOfPowerLevelGameInfo.PLAYER_STATS(CInt(playerId)).BonusHP = 0
			TowerOfPowerLevelGameInfo.PLAYER_STATS(CInt(playerId)).SuperCharge = 0F
			TowerOfPowerLevelGameInfo.PLAYER_STATS(CInt(playerId)).tokenCount = 0
		End If
	End Sub

	' Token: 0x06002F64 RID: 12132 RVA: 0x001C1788 File Offset: 0x001BFB88
	Public Shared Sub ResetWeapons(playerId As PlayerId)
		If TowerOfPowerLevelGameInfo.PLAYER_STATS(CInt(playerId)) Is Nothing Then
			Return
		End If
		Dim playerLoadout As PlayerData.PlayerLoadouts.PlayerLoadout = PlayerData.Data.Loadouts.GetPlayerLoadout(playerId)
		playerLoadout.primaryWeapon = TowerOfPowerLevelGameInfo.PLAYER_STATS(CInt(playerId)).basePrimaryWeapon
		playerLoadout.secondaryWeapon = TowerOfPowerLevelGameInfo.PLAYER_STATS(CInt(playerId)).baseSecondaryWeapon
		playerLoadout.super = TowerOfPowerLevelGameInfo.PLAYER_STATS(CInt(playerId)).BaseSuper
		playerLoadout.charm = TowerOfPowerLevelGameInfo.PLAYER_STATS(CInt(playerId)).BaseCharm
		PlayerData.SaveCurrentFile()
	End Sub

	' Token: 0x06002F65 RID: 12133 RVA: 0x001C1804 File Offset: 0x001BFC04
	Public Shared Sub InitEquipment(playerId As PlayerId)
		If TowerOfPowerLevelGameInfo.PLAYER_STATS(CInt(playerId)) Is Nothing Then
			TowerOfPowerLevelGameInfo.PLAYER_STATS(CInt(playerId)) = New PlayersStatsBossesHub()
		End If
		Dim playerLoadout As PlayerData.PlayerLoadouts.PlayerLoadout = PlayerData.Data.Loadouts.GetPlayerLoadout(playerId)
		TowerOfPowerLevelGameInfo.PLAYER_STATS(CInt(playerId)).basePrimaryWeapon = playerLoadout.primaryWeapon
		TowerOfPowerLevelGameInfo.PLAYER_STATS(CInt(playerId)).baseSecondaryWeapon = playerLoadout.secondaryWeapon
		TowerOfPowerLevelGameInfo.PLAYER_STATS(CInt(playerId)).BaseSuper = playerLoadout.super
		TowerOfPowerLevelGameInfo.PLAYER_STATS(CInt(playerId)).BaseCharm = playerLoadout.charm
	End Sub

	' Token: 0x06002F66 RID: 12134 RVA: 0x001C1884 File Offset: 0x001BFC84
	Public Shared Sub SetPlayersStats(playerId As PlayerId)
		If TowerOfPowerLevelGameInfo.PLAYER_STATS(CInt(playerId)) Is Nothing Then
			TowerOfPowerLevelGameInfo.PLAYER_STATS(CInt(playerId)) = New PlayersStatsBossesHub()
		End If
		Dim stats As PlayerStatsManager = PlayerManager.GetPlayer(playerId).stats
		TowerOfPowerLevelGameInfo.PLAYER_STATS(CInt(playerId)).HP = stats.Health
		TowerOfPowerLevelGameInfo.PLAYER_STATS(CInt(playerId)).SuperCharge = stats.SuperMeter
	End Sub

	' Token: 0x06002F67 RID: 12135 RVA: 0x001C18DC File Offset: 0x001BFCDC
	Public Shared Sub AddToken()
		If TowerOfPowerLevelGameInfo.PLAYER_STATS(0) Is Nothing Then
			Return
		End If
		If TowerOfPowerLevelGameInfo.PLAYER_STATS(0).HP > 0 Then
			TowerOfPowerLevelGameInfo.PLAYER_STATS(0).tokenCount += 1
		End If
		If PlayerManager.Multiplayer AndAlso TowerOfPowerLevelGameInfo.PLAYER_STATS(1).HP > 0 Then
			TowerOfPowerLevelGameInfo.PLAYER_STATS(1).tokenCount += 1
		End If
	End Sub

	' Token: 0x06002F68 RID: 12136 RVA: 0x001C194C File Offset: 0x001BFD4C
	Public Shared Sub ReduceToken()
		TowerOfPowerLevelGameInfo.PLAYER_STATS(0).tokenCount -= 1
		TowerOfPowerLevelGameInfo.PLAYER_STATS(0).tokenCount = Mathf.Max(0, TowerOfPowerLevelGameInfo.PLAYER_STATS(0).tokenCount)
		If PlayerManager.Multiplayer Then
			TowerOfPowerLevelGameInfo.PLAYER_STATS(1).tokenCount -= 1
			TowerOfPowerLevelGameInfo.PLAYER_STATS(1).tokenCount = Mathf.Max(0, TowerOfPowerLevelGameInfo.PLAYER_STATS(1).tokenCount)
		End If
	End Sub

	' Token: 0x06002F69 RID: 12137 RVA: 0x001C19C7 File Offset: 0x001BFDC7
	Public Shared Sub ReduceToken(playerNum As Integer)
		If TowerOfPowerLevelGameInfo.PLAYER_STATS(playerNum) Is Nothing OrElse TowerOfPowerLevelGameInfo.PLAYER_STATS(playerNum).tokenCount = 0 Then
			Return
		End If
		TowerOfPowerLevelGameInfo.PLAYER_STATS(playerNum).tokenCount -= 1
	End Sub

	' Token: 0x06002F6A RID: 12138 RVA: 0x001C19FC File Offset: 0x001BFDFC
	Public Shared Sub SetDefaultToken(defaultTokenCount As Integer)
		If TowerOfPowerLevelGameInfo.PLAYER_STATS(0) Is Nothing Then
			TowerOfPowerLevelGameInfo.PLAYER_STATS(0) = New PlayersStatsBossesHub()
		End If
		TowerOfPowerLevelGameInfo.PLAYER_STATS(0).tokenCount = defaultTokenCount
		If PlayerManager.Multiplayer Then
			If TowerOfPowerLevelGameInfo.PLAYER_STATS(1) Is Nothing Then
				TowerOfPowerLevelGameInfo.PLAYER_STATS(1) = New PlayersStatsBossesHub()
			End If
			TowerOfPowerLevelGameInfo.PLAYER_STATS(1).tokenCount = defaultTokenCount
		End If
	End Sub

	' Token: 0x06002F6B RID: 12139 RVA: 0x001C1A60 File Offset: 0x001BFE60
	Public Shared Function IsTokenLeft() As Boolean
		If TowerOfPowerLevelGameInfo.PLAYER_STATS(0) Is Nothing AndAlso TowerOfPowerLevelGameInfo.PLAYER_STATS(1) Is Nothing Then
			Return False
		End If
		Dim tokenCount As Integer = TowerOfPowerLevelGameInfo.PLAYER_STATS(0).tokenCount
		Dim num As Integer = If((Not PlayerManager.Multiplayer), 0, TowerOfPowerLevelGameInfo.PLAYER_STATS(1).tokenCount)
		Return tokenCount > 0 OrElse num > 0
	End Function

	' Token: 0x06002F6C RID: 12140 RVA: 0x001C1ABF File Offset: 0x001BFEBF
	Public Shared Function IsTokenLeft(playerNum As Integer) As Boolean
		Return TowerOfPowerLevelGameInfo.PLAYER_STATS(playerNum) IsNot Nothing AndAlso TowerOfPowerLevelGameInfo.PLAYER_STATS(playerNum).tokenCount <> 0
	End Function

	' Token: 0x06002F6D RID: 12141 RVA: 0x001C1AE1 File Offset: 0x001BFEE1
	Private Sub OnDestroy()
		TowerOfPowerLevelGameInfo.ResetWeapons(PlayerId.PlayerOne)
		If PlayerManager.Multiplayer Then
			TowerOfPowerLevelGameInfo.ResetWeapons(PlayerId.PlayerTwo)
		End If
	End Sub

	' Token: 0x0400382E RID: 14382
	Private Shared gameInfo As TowerOfPowerLevelGameInfo

	' Token: 0x0400382F RID: 14383
	Public Shared allStageSpaces As List(Of Levels) = New List(Of Levels)()

	' Token: 0x04003830 RID: 14384
	Public Shared difficultyByBossIndex As Level.Mode()

	' Token: 0x04003831 RID: 14385
	Public Shared SlotOne As List(Of String) = New List(Of String)()

	' Token: 0x04003832 RID: 14386
	Public Shared SlotTwo As List(Of String) = New List(Of String)()

	' Token: 0x04003833 RID: 14387
	Public Shared SlotThree As List(Of String) = New List(Of String)()

	' Token: 0x04003834 RID: 14388
	Public Shared SlotThreeChalice As List(Of String) = New List(Of String)()

	' Token: 0x04003835 RID: 14389
	Public Shared SlotFour As List(Of String) = New List(Of String)()

	' Token: 0x04003836 RID: 14390
	Public Shared SlotFourChalice As List(Of String) = New List(Of String)()

	' Token: 0x04003837 RID: 14391
	Public Shared baseDifficulty As Level.Mode

	' Token: 0x04003838 RID: 14392
	Public Shared CURRENT_TURN As Integer

	' Token: 0x04003839 RID: 14393
	Public Shared TURN_COUNTER As Integer

	' Token: 0x0400383A RID: 14394
	Public Shared MIN_RANK_NEED_TO_GET_TOKEN As Integer

	' Token: 0x0400383B RID: 14395
	Public Shared PLAYER_STATS As PlayersStatsBossesHub() = New PlayersStatsBossesHub(1) {}
End Class
