Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Linq
Imports UnityEngine

' Token: 0x02000803 RID: 2051
Public Class TowerOfPowerLevelGameManager
	Inherits LevelProperties.TowerOfPower.Entity

	' Token: 0x06002F70 RID: 12144 RVA: 0x001C1BE4 File Offset: 0x001BFFE4
	Public Overrides Sub LevelInit(properties As LevelProperties.TowerOfPower)
		MyBase.LevelInit(properties)
		Me.anyInput = New CupheadInput.AnyPlayerInput(False)
		TowerOfPowerLevelGameInfo.CURRENT_TURN = TowerOfPowerLevelGameInfo.TURN_COUNTER
		If TowerOfPowerLevelGameInfo.CURRENT_TURN = 0 Then
			TowerOfPowerLevelGameInfo.baseDifficulty = Level.Current.mode
			TowerOfPowerLevelGameInfo.SetDefaultToken(properties.CurrentState.slotMachine.DefaultStartingToken)
			TowerOfPowerLevelGameInfo.MIN_RANK_NEED_TO_GET_TOKEN = properties.CurrentState.slotMachine.MinRankToGainToken
			Me.InitDifficultyBossByIndex()
			Me.InitPools()
			Me.SetTowerBosses()
			Me.InitSlotMachine()
			TowerOfPowerLevelGameInfo.InitEquipment(PlayerId.PlayerOne)
			If PlayerManager.Multiplayer Then
				TowerOfPowerLevelGameInfo.InitEquipment(PlayerId.PlayerTwo)
			End If
			If Me.debugSkipToLastFight Then
				TowerOfPowerLevelGameInfo.TURN_COUNTER = TowerOfPowerLevelGameInfo.allStageSpaces.Count - 1
				TowerOfPowerLevelGameInfo.CURRENT_TURN = TowerOfPowerLevelGameInfo.TURN_COUNTER
			End If
		End If
		MyBase.StartCoroutine(Me.main_cr())
	End Sub

	' Token: 0x06002F71 RID: 12145 RVA: 0x001C1CB8 File Offset: 0x001C00B8
	Public Sub ChangePlayersWeapon(playerId As PlayerId)
		If playerId = PlayerId.PlayerTwo AndAlso Not PlayerManager.Multiplayer Then
			Return
		End If
		Dim flag As Boolean = TowerOfPowerLevelGameInfo.PLAYER_STATS(CInt(playerId)).BaseCharm = Charm.charm_chalice
		Me.bonusHP(CInt(playerId)) = 0
		Me.bonusToken(CInt(playerId)) = 0
		Me.SlotMachineWeapon2Attempt(CInt(playerId)) = 0
		Dim weaponSlotEnumByName As TowerOfPowerLevelGameManager.Weapon_Slot = TowerOfPowerLevelGameManager.GetWeaponSlotEnumByName(TowerOfPowerLevelGameInfo.SlotOne.RandomChoice())
		Dim count As Integer = TowerOfPowerLevelGameInfo.SlotTwo.Count
		Dim num As Integer = Global.UnityEngine.Random.Range(0, count)
		Dim weapon_Slot As TowerOfPowerLevelGameManager.Weapon_Slot = TowerOfPowerLevelGameManager.GetWeaponSlotEnumByName(TowerOfPowerLevelGameInfo.SlotTwo(num))
		While weapon_Slot = weaponSlotEnumByName
			num += 1
			Me.SlotMachineWeapon2Attempt(CInt(playerId)) += 1
			If num >= count Then
				num = 0
			End If
			weapon_Slot = TowerOfPowerLevelGameManager.GetWeaponSlotEnumByName(TowerOfPowerLevelGameInfo.SlotTwo(num))
			If Me.SlotMachineWeapon2Attempt(CInt(playerId)) = count Then
				Global.Debug.LogError("The slotTwo list needs at least two kinds of weapon. Modify the Tower of Power in the Level Editor--Slot Two weapon in the SlotMachine section.", Nothing)
				Exit While
			End If
		End While
		Dim charm_Slot As TowerOfPowerLevelGameManager.Charm_Slot
		Dim super_Slot As TowerOfPowerLevelGameManager.Super_Slot
		If flag Then
			charm_Slot = TowerOfPowerLevelGameManager.GetCharmSlotEnumByName(TowerOfPowerLevelGameInfo.SlotThreeChalice.RandomChoice())
			super_Slot = TowerOfPowerLevelGameManager.GetSuperSlotEnumByName(TowerOfPowerLevelGameInfo.SlotFourChalice.RandomChoice())
		Else
			charm_Slot = TowerOfPowerLevelGameManager.GetCharmSlotEnumByName(TowerOfPowerLevelGameInfo.SlotThree.RandomChoice())
			super_Slot = TowerOfPowerLevelGameManager.GetSuperSlotEnumByName(TowerOfPowerLevelGameInfo.SlotFour.RandomChoice())
		End If
		If charm_Slot = TowerOfPowerLevelGameManager.Charm_Slot.charm_extra_token Then
			Me.bonusToken(CInt(playerId)) = 1
		End If
		If charm_Slot = TowerOfPowerLevelGameManager.Charm_Slot.charm_health_up_1 Then
			Me.bonusHP(CInt(playerId)) = 1
		ElseIf charm_Slot = TowerOfPowerLevelGameManager.Charm_Slot.charm_health_up_2 Then
			Me.bonusHP(CInt(playerId)) = 2
		End If
		Dim playerLoadout As PlayerData.PlayerLoadouts.PlayerLoadout = PlayerData.Data.Loadouts.GetPlayerLoadout(playerId)
		playerLoadout.primaryWeapon = TowerOfPowerLevelGameManager.GetWeaponEnumByName(weaponSlotEnumByName.ToString())
		playerLoadout.secondaryWeapon = TowerOfPowerLevelGameManager.GetWeaponEnumByName(weapon_Slot.ToString())
		playerLoadout.charm = TowerOfPowerLevelGameManager.GetCharmEnumByName(charm_Slot.ToString())
		playerLoadout.super = TowerOfPowerLevelGameManager.GetSuperEnumByName(super_Slot.ToString())
	End Sub

	' Token: 0x06002F72 RID: 12146 RVA: 0x001C1E9C File Offset: 0x001C029C
	Private Iterator Function startMiniBoss_cr(level As Levels) As IEnumerator
		TowerOfPowerLevelGameInfo.SetPlayersStats(PlayerId.PlayerOne)
		If PlayerManager.Multiplayer Then
			TowerOfPowerLevelGameInfo.SetPlayersStats(PlayerId.PlayerTwo)
		End If
		Level.ScoringData.time += Level.Current.LevelTime
		SceneLoader.LoadLevel(level, SceneLoader.Transition.Fade, SceneLoader.Icon.Hourglass, Nothing)
		Yield Nothing
		Return
	End Function

	' Token: 0x06002F73 RID: 12147 RVA: 0x001C1EB8 File Offset: 0x001C02B8
	Public Shared Function GetWeaponSlotEnumByName(Name As String) As TowerOfPowerLevelGameManager.Weapon_Slot
		Return CType([Enum].Parse(GetType(TowerOfPowerLevelGameManager.Weapon_Slot), Name), TowerOfPowerLevelGameManager.Weapon_Slot)
	End Function

	' Token: 0x06002F74 RID: 12148 RVA: 0x001C1EDC File Offset: 0x001C02DC
	Public Shared Function GetCharmSlotEnumByName(Name As String) As TowerOfPowerLevelGameManager.Charm_Slot
		Return CType([Enum].Parse(GetType(TowerOfPowerLevelGameManager.Charm_Slot), Name), TowerOfPowerLevelGameManager.Charm_Slot)
	End Function

	' Token: 0x06002F75 RID: 12149 RVA: 0x001C1F00 File Offset: 0x001C0300
	Public Shared Function GetSuperSlotEnumByName(Name As String) As TowerOfPowerLevelGameManager.Super_Slot
		Return CType([Enum].Parse(GetType(TowerOfPowerLevelGameManager.Super_Slot), Name), TowerOfPowerLevelGameManager.Super_Slot)
	End Function

	' Token: 0x06002F76 RID: 12150 RVA: 0x001C1F24 File Offset: 0x001C0324
	Public Shared Function GetWeaponEnumByName(Name As String) As Weapon
		Dim weapon As Weapon = Weapon.None
		If [Enum].IsDefined(GetType(Weapon), Name) Then
			weapon = CType([Enum].Parse(GetType(Weapon), Name), Weapon)
		End If
		Return weapon
	End Function

	' Token: 0x06002F77 RID: 12151 RVA: 0x001C1F64 File Offset: 0x001C0364
	Public Shared Function GetCharmEnumByName(Name As String) As Charm
		Dim charm As Charm = Charm.None
		If [Enum].IsDefined(GetType(Charm), Name) Then
			charm = CType([Enum].Parse(GetType(Charm), Name), Charm)
		End If
		Return charm
	End Function

	' Token: 0x06002F78 RID: 12152 RVA: 0x001C1FA4 File Offset: 0x001C03A4
	Public Shared Function GetSuperEnumByName(Name As String) As Super
		Dim super As Super = Super.None
		If [Enum].IsDefined(GetType(Super), Name) Then
			super = CType([Enum].Parse(GetType(Super), Name), Super)
		End If
		Return super
	End Function

	' Token: 0x06002F79 RID: 12153 RVA: 0x001C1FE3 File Offset: 0x001C03E3
	Private Sub InitPools()
		Me.InitBossPools()
		Me.InitShmupPools()
		Me.InitKingDicePools()
	End Sub

	' Token: 0x06002F7A RID: 12154 RVA: 0x001C1FF8 File Offset: 0x001C03F8
	Private Sub InitBossPools()
		Dim array As String() = MyBase.properties.CurrentState.bossesPropertises.PoolOneString.Split(New Char() { ","c })
		Dim array2 As String() = MyBase.properties.CurrentState.bossesPropertises.PoolTwoString.Split(New Char() { ","c })
		Dim array3 As String() = MyBase.properties.CurrentState.bossesPropertises.PoolThreeString.Split(New Char() { ","c })
		Me.BossPools = New List(Of Levels)(2) {}
		Me.BossPools(0) = New List(Of Levels)()
		For i As Integer = 0 To array.Length - 1
			Me.BossPools(0).Add(Level.GetEnumByName(array(i)))
		Next
		Me.BossPools(1) = New List(Of Levels)()
		For j As Integer = 0 To array2.Length - 1
			Me.BossPools(1).Add(Level.GetEnumByName(array2(j)))
		Next
		Me.BossPools(2) = New List(Of Levels)()
		For k As Integer = 0 To array3.Length - 1
			Me.BossPools(2).Add(Level.GetEnumByName(array3(k)))
		Next
	End Sub

	' Token: 0x06002F7B RID: 12155 RVA: 0x001C2130 File Offset: 0x001C0530
	Private Sub InitShmupPools()
		Me.ShmupPlacement.Clear()
		Dim array As String() = MyBase.properties.CurrentState.bossesPropertises.ShmupPoolOneString.Split(New Char() { ","c })
		Dim array2 As String() = MyBase.properties.CurrentState.bossesPropertises.ShmupPoolTwoString.Split(New Char() { ","c })
		Dim array3 As String() = MyBase.properties.CurrentState.bossesPropertises.ShmupPoolThreeString.Split(New Char() { ","c })
		Dim list As List(Of String) = MyBase.properties.CurrentState.bossesPropertises.ShmupPlacementString.Split(New Char() { ","c }).ToList()
		Dim shmupCountString As String = MyBase.properties.CurrentState.bossesPropertises.ShmupCountString
		Dim array4 As String() = shmupCountString.Split(New Char() { ","c })
		Dim num As Integer = Parser.IntParse(array4(Global.UnityEngine.Random.Range(0, array4.Length)))
		If num > 0 Then
			Do
				Dim placement As Integer = Parser.IntParse(list(Global.UnityEngine.Random.Range(0, list.Count)))
				Me.ShmupPlacement.Add(placement)
				list.RemoveAll(Function(x As String) x = placement.ToString())
			Loop While Me.ShmupPlacement.Count < num AndAlso list.Count <> 0
		End If
		Me.ShmupPools = New List(Of Levels)(2) {}
		Me.ShmupPools(0) = New List(Of Levels)()
		For i As Integer = 0 To array.Length - 1
			Me.ShmupPools(0).Add(Level.GetEnumByName(array(i)))
		Next
		Me.ShmupPools(1) = New List(Of Levels)()
		For j As Integer = 0 To array2.Length - 1
			Me.ShmupPools(1).Add(Level.GetEnumByName(array2(j)))
		Next
		Me.ShmupPools(2) = New List(Of Levels)()
		For k As Integer = 0 To array3.Length - 1
			Me.ShmupPools(2).Add(Level.GetEnumByName(array3(k)))
		Next
	End Sub

	' Token: 0x06002F7C RID: 12156 RVA: 0x001C2354 File Offset: 0x001C0754
	Private Sub InitKingDicePools()
		Dim array As String() = MyBase.properties.CurrentState.bossesPropertises.KingDicePoolOneString.Split(New Char() { ","c })
		Dim array2 As String() = MyBase.properties.CurrentState.bossesPropertises.KingDicePoolTwoString.Split(New Char() { ","c })
		Dim array3 As String() = MyBase.properties.CurrentState.bossesPropertises.KingDicePoolThreeString.Split(New Char() { ","c })
		Dim array4 As String() = MyBase.properties.CurrentState.bossesPropertises.KingDicePoolFourString.Split(New Char() { ","c })
		Dim kingDiceMiniBossCount As Integer = MyBase.properties.CurrentState.bossesPropertises.KingDiceMiniBossCount
		Me.KingDicePools = New List(Of Levels)(3) {}
		Me.KingDicePools(0) = New List(Of Levels)()
		For i As Integer = 0 To array.Length - 1
			Me.KingDicePools(0).Add(Level.GetEnumByName(array(i)))
		Next
		Me.KingDicePools(1) = New List(Of Levels)()
		For j As Integer = 0 To array2.Length - 1
			Me.KingDicePools(1).Add(Level.GetEnumByName(array2(j)))
		Next
		Me.KingDicePools(2) = New List(Of Levels)()
		For k As Integer = 0 To array3.Length - 1
			Me.KingDicePools(2).Add(Level.GetEnumByName(array3(k)))
		Next
		Me.KingDicePools(3) = New List(Of Levels)()
		For l As Integer = 0 To array4.Length - 1
			Me.KingDicePools(3).Add(Level.GetEnumByName(array4(l)))
		Next
	End Sub

	' Token: 0x06002F7D RID: 12157 RVA: 0x001C2508 File Offset: 0x001C0908
	Private Sub SetTowerBosses()
		TowerOfPowerLevelGameInfo.allStageSpaces.Clear()
		For i As Integer = 0 To 3 - 1
			For j As Integer = 0 To 3 - 1
				If i = 2 AndAlso j = 2 Then
					Dim kingDiceMiniBossCount As Integer = MyBase.properties.CurrentState.bossesPropertises.KingDiceMiniBossCount
					For k As Integer = 0 To kingDiceMiniBossCount - 1
						Me.SetKingDiceBosses(k)
					Next
					TowerOfPowerLevelGameInfo.allStageSpaces.Add(Levels.DicePalaceMain)
					If MyBase.properties.CurrentState.bossesPropertises.DevilFinalBoss Then
						TowerOfPowerLevelGameInfo.allStageSpaces.Add(Levels.Devil)
					End If
				Else
					Dim count As Integer = TowerOfPowerLevelGameInfo.allStageSpaces.Count
					If Me.ShmupPlacement.Contains(count + 1) Then
						Me.AddShmupInTower(Me.ShmupPlacement.IndexOf(count + 1))
					Else
						Me.AddBossInTower(i)
					End If
				End If
			Next
		Next
	End Sub

	' Token: 0x06002F7E RID: 12158 RVA: 0x001C2600 File Offset: 0x001C0A00
	Private Sub AddBossInTower(tier As Integer)
		Me.BossPools(tier).RemoveAll(Function(x As Levels) TowerOfPowerLevelGameInfo.allStageSpaces.Contains(x))
		If Me.BossPools(tier).Count = 0 Then
			Global.Debug.LogError("Number of Boss in the pool " + tier + " is empty.", Nothing)
			Return
		End If
		Dim randLv As Levels = Me.BossPools(tier).RandomChoice()
		If TowerOfPowerLevelGameInfo.allStageSpaces.Contains(randLv) Then
			Global.Debug.LogError("RemoveAll(x => allStageSpaces.Contains(x) don't work like experted", Nothing)
		Else
			TowerOfPowerLevelGameInfo.allStageSpaces.Add(randLv)
			Me.BossPools(tier).RemoveAll(Function(x As Levels) x = randLv)
		End If
	End Sub

	' Token: 0x06002F7F RID: 12159 RVA: 0x001C26D0 File Offset: 0x001C0AD0
	Private Sub AddShmupInTower(tier As Integer)
		Me.ShmupPools(tier).RemoveAll(Function(x As Levels) TowerOfPowerLevelGameInfo.allStageSpaces.Contains(x))
		If Me.ShmupPools(tier).Count = 0 Then
			Global.Debug.LogError("Number of Boss in the pool " + tier + " is empty.", Nothing)
			Return
		End If
		Dim randLv As Levels = Me.ShmupPools(tier).RandomChoice()
		If TowerOfPowerLevelGameInfo.allStageSpaces.Contains(randLv) Then
			Global.Debug.LogError("RemoveAll(x => allStageSpaces.Contains(x) don't work like experted", Nothing)
		Else
			TowerOfPowerLevelGameInfo.allStageSpaces.Add(randLv)
			Me.ShmupPools(tier).RemoveAll(Function(x As Levels) x = randLv)
		End If
	End Sub

	' Token: 0x06002F80 RID: 12160 RVA: 0x001C27A0 File Offset: 0x001C0BA0
	Private Sub SetKingDiceBosses(tier As Integer)
		Me.KingDicePools(tier).RemoveAll(Function(x As Levels) TowerOfPowerLevelGameInfo.allStageSpaces.Contains(x))
		If Me.KingDicePools(tier).Count = 0 Then
			Global.Debug.LogError("Number of Boss in the pool " + tier + " is empty.", Nothing)
			Return
		End If
		Dim randLv As Levels = Me.KingDicePools(tier).RandomChoice()
		If TowerOfPowerLevelGameInfo.allStageSpaces.Contains(randLv) Then
			Global.Debug.LogError("RemoveAll(x => allStageSpaces.Contains(x) don't work like expected", Nothing)
		Else
			TowerOfPowerLevelGameInfo.allStageSpaces.Add(randLv)
			Me.KingDicePools(tier).RemoveAll(Function(x As Levels) x = randLv)
		End If
	End Sub

	' Token: 0x06002F81 RID: 12161 RVA: 0x001C2870 File Offset: 0x001C0C70
	Private Sub InitDifficultyBossByIndex()
		Dim array As String() = MyBase.properties.CurrentState.bossesPropertises.MiniBossDifficultyByIndex.Split(New Char() { ","c })
		TowerOfPowerLevelGameInfo.difficultyByBossIndex = New Level.Mode(array.Length - 1) {}
		For i As Integer = 0 To array.Length - 1
			Dim num As Integer = 0
			Parser.IntTryParse(array(i), num)
			TowerOfPowerLevelGameInfo.difficultyByBossIndex(i) = CType(num, Level.Mode)
		Next
	End Sub

	' Token: 0x06002F82 RID: 12162 RVA: 0x001C28DC File Offset: 0x001C0CDC
	Private Sub InitSlotMachine()
		Dim array As String() = MyBase.properties.CurrentState.slotMachine.SlotOneWeapon.Split(New Char() { ","c })
		Dim array2 As String() = MyBase.properties.CurrentState.slotMachine.SlotTwoWeapon.Split(New Char() { ","c })
		Dim array3 As String() = MyBase.properties.CurrentState.slotMachine.SlotThreeCharm.Split(New Char() { ","c })
		Dim array4 As String() = MyBase.properties.CurrentState.slotMachine.SlotThreeChalice.Split(New Char() { ","c })
		Dim array5 As String() = MyBase.properties.CurrentState.slotMachine.SlotFourSuper.Split(New Char() { ","c })
		Dim array6 As String() = MyBase.properties.CurrentState.slotMachine.SlotFourChalice.Split(New Char() { ","c })
		For i As Integer = 0 To array.Length - 1
			Dim text As String = If((Not(array(i) <> "None")), array(i), ("level_weapon_" + array(i)))
			TowerOfPowerLevelGameInfo.SlotOne.Add(text)
		Next
		For j As Integer = 0 To array2.Length - 1
			Dim text As String = If((Not(array2(j) <> "None")), array2(j), ("level_weapon_" + array2(j)))
			TowerOfPowerLevelGameInfo.SlotTwo.Add(text)
		Next
		For k As Integer = 0 To array3.Length - 1
			Dim text As String = If((Not(array3(k) <> "None")), array3(k), ("charm_" + array3(k)))
			TowerOfPowerLevelGameInfo.SlotThree.Add(text)
		Next
		For l As Integer = 0 To array4.Length - 1
			Dim text As String = If((Not(array4(l) <> "None")), array4(l), ("charm_" + array4(l)))
			TowerOfPowerLevelGameInfo.SlotThreeChalice.Add(text)
		Next
		For m As Integer = 0 To array5.Length - 1
			Dim text As String = If((Not(array5(m) <> "None")), array5(m), ("level_super_" + array5(m)))
			TowerOfPowerLevelGameInfo.SlotFour.Add(text)
		Next
		For n As Integer = 0 To array6.Length - 1
			Dim text As String = If((Not(array6(n) <> "None")), array6(n), ("level_super_chalice_" + array6(n)))
			TowerOfPowerLevelGameInfo.SlotFourChalice.Add(text)
		Next
	End Sub

	' Token: 0x06002F83 RID: 12163 RVA: 0x001C2BB8 File Offset: 0x001C0FB8
	Private Sub SetDifficulty()
		Dim num As Integer = TowerOfPowerLevelGameInfo.CURRENT_TURN
		If num >= TowerOfPowerLevelGameInfo.difficultyByBossIndex.Length Then
			num = TowerOfPowerLevelGameInfo.difficultyByBossIndex.Length - 1
		End If
		Level.SetCurrentMode(TowerOfPowerLevelGameInfo.difficultyByBossIndex(num))
	End Sub

	' Token: 0x06002F84 RID: 12164 RVA: 0x001C2BF0 File Offset: 0x001C0FF0
	Private Sub RevivePlayer(playerId As PlayerId)
		Dim stats As PlayerStatsManager = PlayerManager.GetPlayer(playerId).stats
		TowerOfPowerLevelGameInfo.PLAYER_STATS(CInt(playerId)).HP = 3
		TowerOfPowerLevelGameInfo.PLAYER_STATS(CInt(playerId)).BonusHP = 3
		stats.SetHealth(3)
	End Sub

	' Token: 0x06002F85 RID: 12165 RVA: 0x001C2C2C File Offset: 0x001C102C
	Private Iterator Function main_cr() As IEnumerator
		Dim turn As Integer = TowerOfPowerLevelGameInfo.CURRENT_TURN
		Dim allStage As List(Of Levels) = TowerOfPowerLevelGameInfo.allStageSpaces
		If turn > 0 Then
			Me.showingScorecard = True
			While SceneLoader.CurrentlyLoading
				Yield Nothing
			End While
			Me.scorecard.gameObject.SetActive(True)
			While Not Me.scorecard.done
				Yield Nothing
			End While
			Me.showingScorecard = False
			Me.scorecard.gameObject.SetActive(False)
			If PlayerManager.GetPlayer(PlayerId.PlayerOne).IsDead AndAlso TowerOfPowerLevelGameInfo.IsTokenLeft(0) Then
				TowerOfPowerLevelGameInfo.ReduceToken(0)
				Me.RevivePlayer(PlayerId.PlayerOne)
			End If
			If PlayerManager.Multiplayer AndAlso PlayerManager.GetPlayer(PlayerId.PlayerTwo).IsDead AndAlso TowerOfPowerLevelGameInfo.IsTokenLeft(1) Then
				TowerOfPowerLevelGameInfo.ReduceToken(1)
				Me.RevivePlayer(PlayerId.PlayerTwo)
			End If
		End If
		If(turn <> 0 AndAlso turn Mod 3 = 0 AndAlso turn < 8) OrElse allStage(turn) = Levels.Devil OrElse Me.debugForceSlotMachineEveryTurn OrElse (turn = 1 AndAlso Me.debugForceSlotMachineAfterOneFight) Then
			If Not PlayerManager.GetPlayer(PlayerId.PlayerOne).IsDead Then
				Me.ChangePlayersWeapon(PlayerId.PlayerOne)
				Me.slotsDone(0) = False
				MyBase.StartCoroutine(Me.play_slot_machine_cr(PlayerId.PlayerOne))
			Else
				Me.slotsDone(0) = True
			End If
			If PlayerManager.Multiplayer AndAlso Not PlayerManager.GetPlayer(PlayerId.PlayerTwo).IsDead Then
				Me.slotsDone(1) = False
				Me.ChangePlayersWeapon(PlayerId.PlayerTwo)
				MyBase.StartCoroutine(Me.play_slot_machine_cr(PlayerId.PlayerTwo))
			Else
				Me.slotsDone(1) = True
			End If
		Else
			MyBase.StartCoroutine(Me.go_to_next_level_cr())
		End If
		Yield Nothing
		Return
	End Function

	' Token: 0x06002F86 RID: 12166 RVA: 0x001C2C48 File Offset: 0x001C1048
	Private Iterator Function spin_slot_machine_cr(playerId As PlayerId) As IEnumerator
		Yield Nothing
		Return
	End Function

	' Token: 0x06002F87 RID: 12167 RVA: 0x001C2C5C File Offset: 0x001C105C
	Private Iterator Function stop_slot_machine_cr(playerId As PlayerId) As IEnumerator
		While True
			If PauseManager.state = PauseManager.State.Paused Then
				Me.waitForButtonRelease = 3
				Yield Nothing
			End If
			If PlayerManager.GetPlayer(playerId).input.actions.GetButtonUp(13) Then
				Me.waitForButtonRelease = 0
			End If
			If Me.waitForButtonRelease = 0 AndAlso PlayerManager.GetPlayer(playerId).input.actions.GetButtonDown(13) Then
				Exit For
			End If
			If Me.waitForButtonRelease > 0 Then
				Me.waitForButtonRelease -= 1
			End If
			Yield Me.slowdown_slots_cr()
		End While
		Yield Nothing
		Return
	End Function

	' Token: 0x06002F88 RID: 12168 RVA: 0x001C2C80 File Offset: 0x001C1080
	Private Iterator Function slowdown_slots_cr() As IEnumerator
		Yield Nothing
		Return
	End Function

	' Token: 0x06002F89 RID: 12169 RVA: 0x001C2C94 File Offset: 0x001C1094
	Private Iterator Function play_slot_machine_cr(playerId As PlayerId) As IEnumerator
		Yield Me.spin_slot_machine_cr(playerId)
		Me.slotsConfirm(CInt(playerId)) = False
		Me.slotsCanSpinAgain(CInt(playerId)) = False
		Me.slotsAreSpinning(CInt(playerId)) = True
		Yield Me.stop_slot_machine_cr(playerId)
		Me.slotsAreSpinning(CInt(playerId)) = False
		Me.slotsConfirm(CInt(playerId)) = True
		Dim playAgain As Boolean = False
		While Not PlayerManager.GetPlayerInput(playerId).GetButtonDown(13)
			If TowerOfPowerLevelGameInfo.PLAYER_STATS(CInt(playerId)).tokenCount > 0 Then
				Me.slotsCanSpinAgain(CInt(playerId)) = True
				If PlayerManager.GetPlayer(playerId).input.actions.GetButtonDown(7) Then
					playAgain = True
					TowerOfPowerLevelGameInfo.PLAYER_STATS(CInt(playerId)).tokenCount -= 1
					Me.ChangePlayersWeapon(playerId)
					IL_01B9:
					Me.slotsConfirm(CInt(playerId)) = False
					Me.slotsCanSpinAgain(CInt(playerId)) = False
					Yield Nothing
					If playAgain Then
						Yield Me.play_slot_machine_cr(playerId)
					Else
						Me.slotsDone(CInt(playerId)) = True
						Dim chaliceCharmEquipped As Boolean = TowerOfPowerLevelGameInfo.PLAYER_STATS(CInt(playerId)).BaseCharm = Charm.charm_chalice
						Dim P1loadout As PlayerData.PlayerLoadouts.PlayerLoadout = PlayerData.Data.Loadouts.GetPlayerLoadout(playerId)
						If Me.bonusHP(CInt(playerId)) > 0 Then
							P1loadout.charm = If((Not chaliceCharmEquipped), Charm.None, Charm.charm_chalice)
						End If
						If Me.bonusToken(CInt(playerId)) > 0 Then
							P1loadout.charm = If((Not chaliceCharmEquipped), Charm.None, Charm.charm_chalice)
						End If
						Dim playerStats As PlayerStatsManager = PlayerManager.GetPlayer(playerId).stats
						Dim hp As Integer = playerStats.Health + Me.bonusHP(CInt(playerId))
						If hp > 8 Then
							hp = 8
						End If
						TowerOfPowerLevelGameInfo.PLAYER_STATS(CInt(playerId)).HP = hp
						TowerOfPowerLevelGameInfo.PLAYER_STATS(CInt(playerId)).BonusHP = hp - 3
						playerStats.SetHealth(hp)
						Me.bonusHP(CInt(playerId)) = 0
						TowerOfPowerLevelGameInfo.PLAYER_STATS(CInt(playerId)).tokenCount += Me.bonusToken(CInt(playerId))
						Me.bonusToken(CInt(playerId)) = 0
						If Me.slotsDone(0) AndAlso Me.slotsDone(1) Then
							Yield Me.go_to_next_level_cr()
						End If
					End If
					Return
				End If
			End If
			Yield Nothing
		End While
		GoTo IL_01B9
	End Function

	' Token: 0x06002F8A RID: 12170 RVA: 0x001C2CB6 File Offset: 0x001C10B6
	Public Function SlotsDone() As Boolean
		If PlayerManager.Multiplayer Then
			Return Me.slotsDone(0) AndAlso Me.slotsDone(1)
		End If
		Return Me.slotsDone(0)
	End Function

	' Token: 0x06002F8B RID: 12171 RVA: 0x001C2CE4 File Offset: 0x001C10E4
	Private Iterator Function go_to_next_level_cr() As IEnumerator
		While SceneLoader.CurrentlyLoading
			Yield Nothing
		End While
		While True
			If PauseManager.state = PauseManager.State.Paused Then
				Me.waitForButtonRelease = 3
				Yield Nothing
			End If
			If Me.anyInput.GetButtonUp(CupheadButton.Accept) Then
				Me.waitForButtonRelease = 0
			End If
			If Me.waitForButtonRelease = 0 AndAlso Me.anyInput.GetButtonDown(CupheadButton.Accept) Then
				Exit For
			End If
			If Me.waitForButtonRelease > 0 Then
				Me.waitForButtonRelease -= 1
			End If
			Yield Nothing
		End While
		Dim currentLevel As Integer = TowerOfPowerLevelGameInfo.CURRENT_TURN
		If TowerOfPowerLevelGameInfo.CURRENT_TURN < TowerOfPowerLevelGameInfo.allStageSpaces.Count Then
			If TowerOfPowerLevelGameInfo.PLAYER_STATS(0) IsNot Nothing Then
				Me.SetDifficulty()
				Yield MyBase.StartCoroutine(Me.startMiniBoss_cr(TowerOfPowerLevelGameInfo.allStageSpaces(currentLevel)))
			End If
		Else
			SceneLoader.LoadLastMap()
		End If
		Return
	End Function

	' Token: 0x0400383C RID: 14396
	Private Const PREWEAPON_NAME As String = "level_weapon_"

	' Token: 0x0400383D RID: 14397
	Private Const PRESUPER_NAME As String = "level_super_"

	' Token: 0x0400383E RID: 14398
	Private Const PRESUPER_CHALICE_NAME As String = "level_super_chalice_"

	' Token: 0x0400383F RID: 14399
	Private Const PRECHARM_NAME As String = "charm_"

	' Token: 0x04003840 RID: 14400
	Private Const PRECHARM_CHALICE_NAME As String = "charm_chalice_"

	' Token: 0x04003841 RID: 14401
	<SerializeField()>
	Private advanceDelay As Single = 10F

	' Token: 0x04003842 RID: 14402
	Private anyInput As CupheadInput.AnyPlayerInput

	' Token: 0x04003843 RID: 14403
	Private bonusHP As Integer() = New Integer(1) {}

	' Token: 0x04003844 RID: 14404
	Private bonusToken As Integer() = New Integer(1) {}

	' Token: 0x04003845 RID: 14405
	Private BossPools As List(Of Levels)()

	' Token: 0x04003846 RID: 14406
	Private ShmupPools As List(Of Levels)()

	' Token: 0x04003847 RID: 14407
	Private KingDicePools As List(Of Levels)()

	' Token: 0x04003848 RID: 14408
	Private ShmupPlacement As List(Of Integer) = New List(Of Integer)()

	' Token: 0x04003849 RID: 14409
	Private SlotMachineWeapon2Attempt As Integer() = New Integer(1) {}

	' Token: 0x0400384A RID: 14410
	Public slotsAreSpinning As Boolean() = New Boolean(1) {}

	' Token: 0x0400384B RID: 14411
	Public slotsCanSpinAgain As Boolean() = New Boolean(1) {}

	' Token: 0x0400384C RID: 14412
	Public slotsConfirm As Boolean() = New Boolean(1) {}

	' Token: 0x0400384D RID: 14413
	Private slotsDone As Boolean() = New Boolean() { True, True }

	' Token: 0x0400384E RID: 14414
	Private waitForButtonRelease As Integer

	' Token: 0x0400384F RID: 14415
	<SerializeField()>
	Private scorecard As TowerOfPowerScorecard

	' Token: 0x04003850 RID: 14416
	Public showingScorecard As Boolean

	' Token: 0x04003851 RID: 14417
	<SerializeField()>
	Private debugForceSlotMachineEveryTurn As Boolean

	' Token: 0x04003852 RID: 14418
	<SerializeField()>
	Private debugForceSlotMachineAfterOneFight As Boolean

	' Token: 0x04003853 RID: 14419
	<SerializeField()>
	Private debugSkipToLastFight As Boolean

	' Token: 0x02000804 RID: 2052
	Public Enum Charm_Slot
		' Token: 0x04003858 RID: 14424
		charm_health_up_1
		' Token: 0x04003859 RID: 14425
		charm_health_up_2
		' Token: 0x0400385A RID: 14426
		charm_super_builder
		' Token: 0x0400385B RID: 14427
		charm_smoke_dash
		' Token: 0x0400385C RID: 14428
		charm_parry_plus
		' Token: 0x0400385D RID: 14429
		charm_pit_saver
		' Token: 0x0400385E RID: 14430
		charm_parry_attack
		' Token: 0x0400385F RID: 14431
		charm_chalice
		' Token: 0x04003860 RID: 14432
		charm_directional_dash
		' Token: 0x04003861 RID: 14433
		None
		' Token: 0x04003862 RID: 14434
		charm_extra_token
	End Enum

	' Token: 0x02000805 RID: 2053
	Public Enum Weapon_Slot
		' Token: 0x04003864 RID: 14436
		level_weapon_peashot
		' Token: 0x04003865 RID: 14437
		level_weapon_spreadshot
		' Token: 0x04003866 RID: 14438
		level_weapon_arc
		' Token: 0x04003867 RID: 14439
		level_weapon_homing
		' Token: 0x04003868 RID: 14440
		level_weapon_exploder
		' Token: 0x04003869 RID: 14441
		level_weapon_boomerang
		' Token: 0x0400386A RID: 14442
		level_weapon_charge
		' Token: 0x0400386B RID: 14443
		level_weapon_bouncer
		' Token: 0x0400386C RID: 14444
		level_weapon_wide_shot
		' Token: 0x0400386D RID: 14445
		plane_weapon_peashot
		' Token: 0x0400386E RID: 14446
		plane_weapon_laser
		' Token: 0x0400386F RID: 14447
		plane_weapon_bomb
		' Token: 0x04003870 RID: 14448
		plane_chalice_weapon_3way
		' Token: 0x04003871 RID: 14449
		arcade_weapon_peashot
		' Token: 0x04003872 RID: 14450
		arcade_weapon_rocket_peashot
		' Token: 0x04003873 RID: 14451
		None
	End Enum

	' Token: 0x02000806 RID: 2054
	Public Enum Super_Slot
		' Token: 0x04003875 RID: 14453
		level_super_beam
		' Token: 0x04003876 RID: 14454
		level_super_ghost
		' Token: 0x04003877 RID: 14455
		level_super_invincible
		' Token: 0x04003878 RID: 14456
		level_super_chalice_shmup
		' Token: 0x04003879 RID: 14457
		level_super_chalice_vert_beam
		' Token: 0x0400387A RID: 14458
		level_super_chalice_shield
		' Token: 0x0400387B RID: 14459
		plane_super_bomb
		' Token: 0x0400387C RID: 14460
		plane_super_chalice_stream
		' Token: 0x0400387D RID: 14461
		None
	End Enum
End Class
