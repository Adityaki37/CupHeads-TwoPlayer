Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x0200040B RID: 1035
<Serializable()>
Public Class PlayerData
	' Token: 0x06000E82 RID: 3714 RVA: 0x00094118 File Offset: 0x00092518
	Public Sub New()
		If String.IsNullOrEmpty(PlayerData.emptyDialoguerState) Then
			Dialoguer.Initialize()
			PlayerData.emptyDialoguerState = Dialoguer.GetGlobalVariablesState()
		End If
		Me.dialoguerState = PlayerData.emptyDialoguerState
	End Sub

	' Token: 0x17000251 RID: 593
	' (get) Token: 0x06000E83 RID: 3715 RVA: 0x000941B3 File Offset: 0x000925B3
	' (set) Token: 0x06000E84 RID: 3716 RVA: 0x000941C9 File Offset: 0x000925C9
	Public Shared Property CurrentSaveFileIndex As Integer
		Get
			Return Mathf.Clamp(PlayerData._CurrentSaveFileIndex, 0, PlayerData.SAVE_FILE_KEYS.Length - 1)
		End Get
		Set(value As Integer)
			PlayerData._CurrentSaveFileIndex = Mathf.Clamp(value, 0, PlayerData.SAVE_FILE_KEYS.Length - 1)
			PlayerData.Data.LoadDialogueVariables()
		End Set
	End Property

	' Token: 0x17000252 RID: 594
	' (get) Token: 0x06000E85 RID: 3717 RVA: 0x000941EA File Offset: 0x000925EA
	' (set) Token: 0x06000E86 RID: 3718 RVA: 0x000941F1 File Offset: 0x000925F1
	Public Shared Property Initialized As Boolean
		Get
			Return PlayerData._initialized
		End Get
		Private Set(value As Boolean)
			PlayerData._initialized = value
		End Set
	End Property

	' Token: 0x17000253 RID: 595
	' (get) Token: 0x06000E87 RID: 3719 RVA: 0x000941F9 File Offset: 0x000925F9
	Public Shared ReadOnly Property Data As PlayerData
		Get
			Return PlayerData.GetDataForSlot(PlayerData.CurrentSaveFileIndex)
		End Get
	End Property

	' Token: 0x06000E88 RID: 3720 RVA: 0x00094208 File Offset: 0x00092608
	Public Shared Function GetDataForSlot(slot As Integer) As PlayerData
		If PlayerData._saveFiles Is Nothing OrElse PlayerData._saveFiles.Length <> PlayerData.SAVE_FILE_KEYS.Length Then
			PlayerData._saveFiles = New PlayerData(PlayerData.SAVE_FILE_KEYS.Length - 1) {}
			For i As Integer = 0 To PlayerData.SAVE_FILE_KEYS.Length - 1
				PlayerData._saveFiles(i) = New PlayerData()
			Next
		End If
		If PlayerData._saveFiles(slot).curseCharmPuzzleOrder Is Nothing OrElse PlayerData._saveFiles(slot).curseCharmPuzzleOrder.Length = 0 Then
			PlayerData._saveFiles(slot).CreateCursePuzzleVariables()
		End If
		Return PlayerData._saveFiles(slot)
	End Function

	' Token: 0x06000E89 RID: 3721 RVA: 0x000942A0 File Offset: 0x000926A0
	Private Sub CreateCursePuzzleVariables()
		Dim list As List(Of Integer) = New List(Of Integer)() From { 0, 1, 2, 3, 4, 5, 6, 7 }
		Me.curseCharmPuzzleOrder = New Integer(2) {}
		For i As Integer = 0 To Me.curseCharmPuzzleOrder.Length - 1
			Dim num As Integer = Global.UnityEngine.Random.Range(0, list.Count)
			Me.curseCharmPuzzleOrder(i) = list(num)
			list.Remove(list(num))
		Next
	End Sub

	' Token: 0x06000E8A RID: 3722 RVA: 0x0009433E File Offset: 0x0009273E
	Public Shared Sub ClearSlot(slot As Integer)
		If PlayerData._saveFiles Is Nothing OrElse PlayerData._saveFiles.Length <> PlayerData.SAVE_FILE_KEYS.Length Then
			Return
		End If
		PlayerData.ResetDialoguer()
		PlayerData._saveFiles(slot) = New PlayerData()
		PlayerData.Save(slot)
	End Sub

	' Token: 0x06000E8B RID: 3723 RVA: 0x00094378 File Offset: 0x00092778
	Public Shared Sub Init(handler As PlayerData.PlayerDataInitHandler)
		PlayerData._saveFiles = New PlayerData(PlayerData.SAVE_FILE_KEYS.Length - 1) {}
		For i As Integer = 0 To PlayerData.SAVE_FILE_KEYS.Length - 1
			PlayerData._saveFiles(i) = New PlayerData()
		Next
		PlayerData._playerDatatInitHandler = handler
		OnlineManager.Instance.[Interface].InitializeCloudStorage(PlayerId.PlayerOne, AddressOf PlayerData.OnCloudStorageInitialized)
	End Sub

	' Token: 0x06000E8C RID: 3724 RVA: 0x000943ED File Offset: 0x000927ED
	Private Sub LoadDialogueVariables()
		Dialoguer.Initialize()
		Dialoguer.EndDialogue()
		Dialoguer.SetGlobalVariablesState(Me.dialoguerState)
	End Sub

	' Token: 0x06000E8D RID: 3725 RVA: 0x00094404 File Offset: 0x00092804
	Private Shared Sub OnCloudStorageInitialized(success As Boolean)
		If Not success Then
			PlayerData._playerDatatInitHandler(False)
			Return
		End If
		OnlineManager.Instance.[Interface].LoadCloudData(PlayerData.SAVE_FILE_KEYS, AddressOf PlayerData.OnLoaded)
	End Sub

	' Token: 0x06000E8E RID: 3726 RVA: 0x00094454 File Offset: 0x00092854
	Private Shared Sub OnLoaded(data As String(), result As CloudLoadResult)
		If result = CloudLoadResult.Failed Then
			Global.Debug.LogError("[PlayerData] LOAD FAILED", Nothing)
			OnlineManager.Instance.[Interface].LoadCloudData(PlayerData.SAVE_FILE_KEYS, AddressOf PlayerData.OnLoaded)
			Return
		End If
		If result = CloudLoadResult.NoData Then
			Global.Debug.LogError("[PlayerData] No data. Saving default data to cloud", Nothing)
			PlayerData.SaveAll()
			Return
		End If
		Dim flag As Boolean = False
		For i As Integer = 0 To PlayerData.SAVE_FILE_KEYS.Length - 1
			If data(i) IsNot Nothing Then
				Dim playerData As PlayerData = Nothing
				Try
					playerData = JsonUtility.FromJson(Of PlayerData)(data(i))
					If playerData IsNot Nothing AndAlso Not playerData.coinManager.hasMigratedCoins Then
						playerData = PlayerData.Migrate(playerData)
						flag = True
					End If
				Catch ex As ArgumentException
					Global.Debug.LogError("Unable to parse player data. " + ex.StackTrace, Nothing)
				End Try
				If playerData Is Nothing Then
					Global.Debug.LogError("[PlayerData] Data could not be unserialized for key: " + PlayerData.SAVE_FILE_KEYS(i), Nothing)
				Else
					PlayerData._saveFiles(i) = playerData
				End If
			End If
		Next
		PlayerData.Initialized = True
		If flag Then
			PlayerData.SaveAll()
		End If
		If PlayerData._playerDatatInitHandler IsNot Nothing Then
			PlayerData._playerDatatInitHandler(True)
			PlayerData._playerDatatInitHandler = Nothing
		End If
	End Sub

	' Token: 0x06000E8F RID: 3727 RVA: 0x00094590 File Offset: 0x00092990
	Public Shared Function Migrate(playerData As PlayerData) As PlayerData
		For i As Integer = 0 To playerData.coinManager.LevelsAndCoins.Count - 1
			Dim levelAndCoins As PlayerData.PlayerCoinManager.LevelAndCoins = New PlayerData.PlayerCoinManager.LevelAndCoins()
			levelAndCoins.level = playerData.coinManager.LevelsAndCoins(i).level
			playerData.coinManager.LevelsAndCoins(i) = levelAndCoins
		Next
		For j As Integer = 0 To playerData.coinManager.coins.Count - 1
			Dim coinID As String = playerData.coinManager.coins(j).coinID
			Dim flag As Boolean = False
			For k As Integer = 0 To PlayerData.platformingCoinIDs.Length - 1
				Dim levelsAndCoins As List(Of PlayerData.PlayerCoinManager.LevelAndCoins) = playerData.coinManager.LevelsAndCoins
				Dim num As Integer = -1
				For l As Integer = 0 To levelsAndCoins.Count - 1
					If levelsAndCoins(l).level = PlayerData.platformingCoinIDs(k).levelId Then
						num = l
					End If
				Next
				For m As Integer = 0 To PlayerData.platformingCoinIDs(k).coinIds.Length - 1
					Dim text As String = PlayerData.platformingCoinIDs(k).coinIds(m)(0)
					For n As Integer = 0 To PlayerData.platformingCoinIDs(k).coinIds(m).Length - 1
						If coinID = PlayerData.platformingCoinIDs(k).coinIds(m)(n) Then
							playerData.coinManager.coins(j).coinID = text
							flag = True
							Select Case m
								Case 0
									levelsAndCoins(num).Coin1Collected = True
								Case 1
									levelsAndCoins(num).Coin2Collected = True
								Case 2
									levelsAndCoins(num).Coin3Collected = True
								Case 3
									levelsAndCoins(num).Coin4Collected = True
								Case 4
									levelsAndCoins(num).Coin5Collected = True
							End Select
							Exit For
						End If
					Next
					If flag Then
						Exit For
					End If
				Next
				If flag Then
					Exit For
				End If
			Next
		Next
		playerData.coinManager.hasMigratedCoins = True
		Return playerData
	End Function

	' Token: 0x06000E90 RID: 3728 RVA: 0x000947F3 File Offset: 0x00092BF3
	Private Shared Function GetSaveFileKey(fileIndex As Integer) As String
		Return PlayerData.SAVE_FILE_KEYS(fileIndex)
	End Function

	' Token: 0x06000E91 RID: 3729 RVA: 0x000947FC File Offset: 0x00092BFC
	Private Shared Sub Save(fileIndex As Integer)
		PlayerData._saveFiles(fileIndex).dialoguerState = Dialoguer.GetGlobalVariablesState()
		Dim dictionary As Dictionary(Of String, String) = New Dictionary(Of String, String)()
		dictionary(PlayerData.SAVE_FILE_KEYS(fileIndex)) = JsonUtility.ToJson(PlayerData._saveFiles(fileIndex))
		OnlineManager.Instance.[Interface].SaveCloudData(dictionary, AddressOf PlayerData.OnSaved)
	End Sub

	' Token: 0x06000E92 RID: 3730 RVA: 0x00094868 File Offset: 0x00092C68
	Private Shared Sub SaveAll()
		Dim dictionary As Dictionary(Of String, String) = New Dictionary(Of String, String)()
		For i As Integer = 0 To PlayerData.SAVE_FILE_KEYS.Length - 1
			dictionary(PlayerData.SAVE_FILE_KEYS(i)) = JsonUtility.ToJson(PlayerData._saveFiles(i))
		Next
		OnlineManager.Instance.[Interface].SaveCloudData(dictionary, AddressOf PlayerData.OnSavedAll)
	End Sub

	' Token: 0x06000E93 RID: 3731 RVA: 0x000948D9 File Offset: 0x00092CD9
	Private Shared Sub OnSaved(success As Boolean)
		If Not success Then
			Global.Debug.LogError("[PlayerData] SAVE FAILED. Retrying...", Nothing)
			PlayerData.Save(PlayerData.CurrentSaveFileIndex)
		End If
	End Sub

	' Token: 0x06000E94 RID: 3732 RVA: 0x000948FB File Offset: 0x00092CFB
	Private Shared Sub OnSavedAll(success As Boolean)
		If success Then
			PlayerData.Initialized = True
			If PlayerData._playerDatatInitHandler IsNot Nothing Then
				PlayerData._playerDatatInitHandler(True)
				PlayerData._playerDatatInitHandler = Nothing
			End If
		Else
			Global.Debug.LogError("[PlayerData] SAVE FAILED. Retrying...", Nothing)
			PlayerData.SaveAll()
		End If
	End Sub

	' Token: 0x06000E95 RID: 3733 RVA: 0x00094939 File Offset: 0x00092D39
	Public Shared Sub SaveCurrentFile()
		PlayerData.Save(PlayerData.CurrentSaveFileIndex)
	End Sub

	' Token: 0x06000E96 RID: 3734 RVA: 0x00094945 File Offset: 0x00092D45
	Public Shared Sub ResetDialoguer()
		Dialoguer.SetGlobalVariablesState(PlayerData.emptyDialoguerState)
	End Sub

	' Token: 0x06000E97 RID: 3735 RVA: 0x00094954 File Offset: 0x00092D54
	Public Shared Sub ResetAll()
		For i As Integer = 0 To PlayerData.SAVE_FILE_KEYS.Length - 1
			PlayerData.ClearSlot(i)
		Next
	End Sub

	' Token: 0x06000E98 RID: 3736 RVA: 0x0009497F File Offset: 0x00092D7F
	Public Shared Sub Unload()
		PlayerData._saveFiles = Nothing
	End Sub

	' Token: 0x17000254 RID: 596
	' (get) Token: 0x06000E99 RID: 3737 RVA: 0x00094987 File Offset: 0x00092D87
	Public ReadOnly Property Loadouts As PlayerData.PlayerLoadouts
		Get
			Return Me.loadouts
		End Get
	End Property

	' Token: 0x17000255 RID: 597
	' (get) Token: 0x06000E9A RID: 3738 RVA: 0x0009498F File Offset: 0x00092D8F
	' (set) Token: 0x06000E9B RID: 3739 RVA: 0x00094997 File Offset: 0x00092D97
	Public Property IsHardModeAvailable As Boolean
		Get
			Return Me._isHardModeAvailable
		End Get
		Set(value As Boolean)
			Me._isHardModeAvailable = value
		End Set
	End Property

	' Token: 0x17000256 RID: 598
	' (get) Token: 0x06000E9C RID: 3740 RVA: 0x000949A0 File Offset: 0x00092DA0
	' (set) Token: 0x06000E9D RID: 3741 RVA: 0x000949A8 File Offset: 0x00092DA8
	Public Property IsHardModeAvailableDLC As Boolean
		Get
			Return Me._isHardModeAvailableDLC
		End Get
		Set(value As Boolean)
			Me._isHardModeAvailableDLC = value
		End Set
	End Property

	' Token: 0x17000257 RID: 599
	' (get) Token: 0x06000E9E RID: 3742 RVA: 0x000949B1 File Offset: 0x00092DB1
	' (set) Token: 0x06000E9F RID: 3743 RVA: 0x000949B9 File Offset: 0x00092DB9
	Public Property IsTutorialCompleted As Boolean
		Get
			Return Me._isTutorialCompleted
		End Get
		Set(value As Boolean)
			Me._isTutorialCompleted = value
		End Set
	End Property

	' Token: 0x17000258 RID: 600
	' (get) Token: 0x06000EA0 RID: 3744 RVA: 0x000949C2 File Offset: 0x00092DC2
	' (set) Token: 0x06000EA1 RID: 3745 RVA: 0x000949CA File Offset: 0x00092DCA
	Public Property IsFlyingTutorialCompleted As Boolean
		Get
			Return Me._isFlyingTutorialCompleted
		End Get
		Set(value As Boolean)
			Me._isFlyingTutorialCompleted = value
		End Set
	End Property

	' Token: 0x17000259 RID: 601
	' (get) Token: 0x06000EA2 RID: 3746 RVA: 0x000949D3 File Offset: 0x00092DD3
	' (set) Token: 0x06000EA3 RID: 3747 RVA: 0x000949DB File Offset: 0x00092DDB
	Public Property IsChaliceTutorialCompleted As Boolean
		Get
			Return Me._isChaliceTutorialCompleted
		End Get
		Set(value As Boolean)
			Me._isChaliceTutorialCompleted = value
		End Set
	End Property

	' Token: 0x06000EA4 RID: 3748 RVA: 0x000949E4 File Offset: 0x00092DE4
	Public Function IsUnlocked(player As PlayerId, value As Weapon) As Boolean
		If player = PlayerId.PlayerOne Then
			Return Me.inventories.GetPlayer(PlayerId.PlayerOne).IsUnlocked(value)
		End If
		If player = PlayerId.PlayerTwo Then
			Return Me.inventories.GetPlayer(PlayerId.PlayerTwo).IsUnlocked(value)
		End If
		If player <> PlayerId.Any Then
			If player <> PlayerId.None Then
			End If
			Return False
		End If
		Return Me.inventories.GetPlayer(PlayerId.PlayerOne).IsUnlocked(value) OrElse Me.inventories.GetPlayer(PlayerId.PlayerTwo).IsUnlocked(value)
	End Function

	' Token: 0x06000EA5 RID: 3749 RVA: 0x00094A70 File Offset: 0x00092E70
	Public Function IsUnlocked(player As PlayerId, value As Super) As Boolean
		If player = PlayerId.PlayerOne Then
			Return Me.inventories.GetPlayer(PlayerId.PlayerOne).IsUnlocked(value)
		End If
		If player = PlayerId.PlayerTwo Then
			Return Me.inventories.GetPlayer(PlayerId.PlayerTwo).IsUnlocked(value)
		End If
		If player <> PlayerId.Any Then
			If player <> PlayerId.None Then
			End If
			Return False
		End If
		Return Me.inventories.GetPlayer(PlayerId.PlayerOne).IsUnlocked(value) OrElse Me.inventories.GetPlayer(PlayerId.PlayerTwo).IsUnlocked(value)
	End Function

	' Token: 0x06000EA6 RID: 3750 RVA: 0x00094AFC File Offset: 0x00092EFC
	Public Function IsUnlocked(player As PlayerId, value As Charm) As Boolean
		If player = PlayerId.PlayerOne Then
			Return Me.inventories.GetPlayer(PlayerId.PlayerOne).IsUnlocked(value)
		End If
		If player = PlayerId.PlayerTwo Then
			Return Me.inventories.GetPlayer(PlayerId.PlayerTwo).IsUnlocked(value)
		End If
		If player <> PlayerId.Any Then
			If player <> PlayerId.None Then
			End If
			Return False
		End If
		Return Me.inventories.GetPlayer(PlayerId.PlayerOne).IsUnlocked(value) OrElse Me.inventories.GetPlayer(PlayerId.PlayerTwo).IsUnlocked(value)
	End Function

	' Token: 0x06000EA7 RID: 3751 RVA: 0x00094B88 File Offset: 0x00092F88
	Public Function HasNewPurchase(player As PlayerId) As Boolean
		If player = PlayerId.PlayerOne Then
			Return Me.inventories.GetPlayer(PlayerId.PlayerOne).newPurchase
		End If
		If player = PlayerId.PlayerTwo Then
			Return Me.inventories.GetPlayer(PlayerId.PlayerTwo).newPurchase
		End If
		If player <> PlayerId.Any Then
			If player <> PlayerId.None Then
			End If
			Return False
		End If
		Return Me.inventories.GetPlayer(PlayerId.PlayerOne).newPurchase OrElse Me.inventories.GetPlayer(PlayerId.PlayerTwo).newPurchase
	End Function

	' Token: 0x06000EA8 RID: 3752 RVA: 0x00094C10 File Offset: 0x00093010
	Public Sub ResetHasNewPurchase(player As PlayerId)
		If player = PlayerId.PlayerOne Then
			Me.inventories.GetPlayer(PlayerId.PlayerOne).newPurchase = False
			Return
		End If
		If player = PlayerId.PlayerTwo Then
			Me.inventories.GetPlayer(PlayerId.PlayerTwo).newPurchase = False
			Return
		End If
		If player <> PlayerId.Any Then
			If player <> PlayerId.None Then
			End If
			Return
		End If
		Me.inventories.GetPlayer(PlayerId.PlayerOne).newPurchase = False
		Me.inventories.GetPlayer(PlayerId.PlayerTwo).newPurchase = False
	End Sub

	' Token: 0x06000EA9 RID: 3753 RVA: 0x00094C90 File Offset: 0x00093090
	Public Function Buy(player As PlayerId, value As Weapon) As Boolean
		Return Me.inventories.GetPlayer(player).Buy(value)
	End Function

	' Token: 0x06000EAA RID: 3754 RVA: 0x00094CA4 File Offset: 0x000930A4
	Public Function Buy(player As PlayerId, value As Super) As Boolean
		Return Me.inventories.GetPlayer(player).Buy(value)
	End Function

	' Token: 0x06000EAB RID: 3755 RVA: 0x00094CB8 File Offset: 0x000930B8
	Public Function Buy(player As PlayerId, value As Charm) As Boolean
		Return Me.inventories.GetPlayer(player).Buy(value)
	End Function

	' Token: 0x06000EAC RID: 3756 RVA: 0x00094CCC File Offset: 0x000930CC
	Public Sub Gift(player As PlayerId, value As Weapon)
		Me.inventories.GetPlayer(player)._weapons.Add(value)
	End Sub

	' Token: 0x06000EAD RID: 3757 RVA: 0x00094CE5 File Offset: 0x000930E5
	Public Sub Gift(player As PlayerId, value As Super)
		Me.inventories.GetPlayer(player)._supers.Add(value)
	End Sub

	' Token: 0x06000EAE RID: 3758 RVA: 0x00094CFE File Offset: 0x000930FE
	Public Sub Gift(player As PlayerId, value As Charm)
		Me.inventories.GetPlayer(player)._charms.Add(value)
	End Sub

	' Token: 0x06000EAF RID: 3759 RVA: 0x00094D17 File Offset: 0x00093117
	Public Function NumWeapons(player As PlayerId) As Integer
		Return Me.inventories.GetPlayer(player)._weapons.Count
	End Function

	' Token: 0x06000EB0 RID: 3760 RVA: 0x00094D2F File Offset: 0x0009312F
	Public Function NumCharms(player As PlayerId) As Integer
		Return Me.inventories.GetPlayer(player)._charms.Count
	End Function

	' Token: 0x06000EB1 RID: 3761 RVA: 0x00094D47 File Offset: 0x00093147
	Public Function NumSupers(player As PlayerId) As Integer
		Return Me.inventories.GetPlayer(player)._supers.Count
	End Function

	' Token: 0x06000EB2 RID: 3762 RVA: 0x00094D5F File Offset: 0x0009315F
	Public Function GetCurrency(player As PlayerId) As Integer
		Return Me.inventories.GetPlayer(player).money
	End Function

	' Token: 0x06000EB3 RID: 3763 RVA: 0x00094D72 File Offset: 0x00093172
	Public Sub AddCurrency(player As PlayerId, value As Integer)
		Me.inventories.GetPlayer(player).money += value
	End Sub

	' Token: 0x06000EB4 RID: 3764 RVA: 0x00094D8D File Offset: 0x0009318D
	Public Sub ResetLevelCoinManager()
		Me.levelCoinManager = New PlayerData.PlayerCoinManager()
	End Sub

	' Token: 0x06000EB5 RID: 3765 RVA: 0x00094D9A File Offset: 0x0009319A
	Public Function GetCoinCollected(coin As LevelCoin) As Boolean
		Return Me.coinManager.GetCoinCollected(coin)
	End Function

	' Token: 0x06000EB6 RID: 3766 RVA: 0x00094DA8 File Offset: 0x000931A8
	Public Sub SetLevelCoinCollected(coin As LevelCoin, collected As Boolean, player As PlayerId)
		Me.levelCoinManager.SetCoinValue(coin, collected, player)
	End Sub

	' Token: 0x1700025A RID: 602
	' (get) Token: 0x06000EB7 RID: 3767 RVA: 0x00094DB8 File Offset: 0x000931B8
	Public ReadOnly Property NumCoinsCollected As Integer
		Get
			Return Me.coinManager.NumCoinsCollected()
		End Get
	End Property

	' Token: 0x1700025B RID: 603
	' (get) Token: 0x06000EB8 RID: 3768 RVA: 0x00094DC5 File Offset: 0x000931C5
	Public ReadOnly Property NumCoinsCollectedMainGame As Integer
		Get
			Return Me.coinManager.NumCoinsCollected(False)
		End Get
	End Property

	' Token: 0x06000EB9 RID: 3769 RVA: 0x00094DD4 File Offset: 0x000931D4
	Public Function GetNumCoinsCollectedInLevel(level As Levels) As Integer
		Dim levelsAndCoins As List(Of PlayerData.PlayerCoinManager.LevelAndCoins) = Me.coinManager.LevelsAndCoins
		For i As Integer = 0 To levelsAndCoins.Count - 1
			If levelsAndCoins(i).level = level Then
				Dim num As Integer = 0
				If levelsAndCoins(i).Coin1Collected Then
					num += 1
				End If
				If levelsAndCoins(i).Coin2Collected Then
					num += 1
				End If
				If levelsAndCoins(i).Coin3Collected Then
					num += 1
				End If
				If levelsAndCoins(i).Coin4Collected Then
					num += 1
				End If
				If levelsAndCoins(i).Coin5Collected Then
					num += 1
				End If
				Return num
			End If
		Next
		Return 0
	End Function

	' Token: 0x06000EBA RID: 3770 RVA: 0x00094E84 File Offset: 0x00093284
	Public Sub ApplyLevelCoins()
		For Each playerCoinProperties As PlayerData.PlayerCoinProperties In Me.levelCoinManager.coins
			Me.coinManager.SetCoinValue(playerCoinProperties.coinID, playerCoinProperties.collected, playerCoinProperties.player)
			If playerCoinProperties.collected Then
				PlayerData.Data.AddCurrency(PlayerId.PlayerOne, 1)
				PlayerData.Data.AddCurrency(PlayerId.PlayerTwo, 1)
			End If
		Next
		Me.levelCoinManager = New PlayerData.PlayerCoinManager()
	End Sub

	' Token: 0x1700025C RID: 604
	' (get) Token: 0x06000EBB RID: 3771 RVA: 0x00094F2C File Offset: 0x0009332C
	Public ReadOnly Property CurrentMapData As PlayerData.MapData
		Get
			Return Me.mapDataManager.GetCurrentMapData()
		End Get
	End Property

	' Token: 0x06000EBC RID: 3772 RVA: 0x00094F39 File Offset: 0x00093339
	Public Function GetMapData(map As Scenes) As PlayerData.MapData
		Return Me.mapDataManager.GetMapData(map)
	End Function

	' Token: 0x1700025D RID: 605
	' (get) Token: 0x06000EBD RID: 3773 RVA: 0x00094F47 File Offset: 0x00093347
	' (set) Token: 0x06000EBE RID: 3774 RVA: 0x00094F54 File Offset: 0x00093354
	Public Property CurrentMap As Scenes
		Get
			Return Me.mapDataManager.currentMap
		End Get
		Set(value As Scenes)
			Me.mapDataManager.currentMap = value
		End Set
	End Property

	' Token: 0x06000EBF RID: 3775 RVA: 0x00094F62 File Offset: 0x00093362
	Public Function GetLevelData(levelID As Levels) As PlayerData.PlayerLevelDataObject
		Return Me.levelDataManager.GetLevelData(levelID)
	End Function

	' Token: 0x06000EC0 RID: 3776 RVA: 0x00094F70 File Offset: 0x00093370
	Public Function CountLevelsCompleted(levels As Levels()) As Integer
		Dim num As Integer = 0
		For Each levels2 As Levels In levels
			Dim levelData As PlayerData.PlayerLevelDataObject = Me.GetLevelData(levels2)
			If levelData.completed Then
				num += 1
			End If
		Next
		Return num
	End Function

	' Token: 0x06000EC1 RID: 3777 RVA: 0x00094FB4 File Offset: 0x000933B4
	Public Function CheckLevelsCompleted(levels As Levels()) As Boolean
		For Each levels2 As Levels In levels
			Dim levelData As PlayerData.PlayerLevelDataObject = Me.GetLevelData(levels2)
			If Not levelData.completed Then
				Return False
			End If
		Next
		Return True
	End Function

	' Token: 0x06000EC2 RID: 3778 RVA: 0x00094FF4 File Offset: 0x000933F4
	Public Function CheckLevelCompleted(level As Levels) As Boolean
		Dim levelData As PlayerData.PlayerLevelDataObject = Me.GetLevelData(level)
		Return levelData.completed
	End Function

	' Token: 0x06000EC3 RID: 3779 RVA: 0x00095018 File Offset: 0x00093418
	Public Function CountLevelsHaveMinGrade(levels As Levels(), minGrade As LevelScoringData.Grade) As Integer
		Dim num As Integer = 0
		For Each levels2 As Levels In levels
			Dim levelData As PlayerData.PlayerLevelDataObject = Me.GetLevelData(levels2)
			If levelData.completed AndAlso levelData.grade >= minGrade Then
				num += 1
			End If
		Next
		Return num
	End Function

	' Token: 0x06000EC4 RID: 3780 RVA: 0x00095068 File Offset: 0x00093468
	Public Function CheckLevelsHaveMinGrade(levels As Levels(), minGrade As LevelScoringData.Grade) As Boolean
		For Each levels2 As Levels In levels
			Dim levelData As PlayerData.PlayerLevelDataObject = Me.GetLevelData(levels2)
			If Not levelData.completed OrElse levelData.grade < minGrade Then
				Return False
			End If
		Next
		Return True
	End Function

	' Token: 0x06000EC5 RID: 3781 RVA: 0x000950B4 File Offset: 0x000934B4
	Public Function CountLevelsHaveMinDifficulty(levels As Levels(), minDifficulty As Level.Mode) As Integer
		Dim num As Integer = 0
		For Each levels2 As Levels In levels
			Dim levelData As PlayerData.PlayerLevelDataObject = Me.GetLevelData(levels2)
			If levelData.completed AndAlso levelData.difficultyBeaten >= minDifficulty Then
				num += 1
			End If
		Next
		Return num
	End Function

	' Token: 0x06000EC6 RID: 3782 RVA: 0x00095104 File Offset: 0x00093504
	Public Function CheckLevelsHaveMinDifficulty(levels As Levels(), minDifficulty As Level.Mode) As Boolean
		For Each levels2 As Levels In levels
			Dim levelData As PlayerData.PlayerLevelDataObject = Me.GetLevelData(levels2)
			If Not levelData.completed OrElse levelData.difficultyBeaten < minDifficulty Then
				Return False
			End If
		Next
		Return True
	End Function

	' Token: 0x06000EC7 RID: 3783 RVA: 0x00095150 File Offset: 0x00093550
	Public Function CountLevelsChaliceCompleted(levels As Levels(), playerId As PlayerId) As Integer
		Dim num As Integer = 0
		For Each levels2 As Levels In levels
			If(playerId = PlayerId.PlayerOne AndAlso Me.GetLevelData(levels2).completedAsChaliceP1) OrElse (playerId = PlayerId.PlayerTwo AndAlso Me.GetLevelData(levels2).completedAsChaliceP2) Then
				num += 1
			End If
		Next
		Return num
	End Function

	' Token: 0x06000EC8 RID: 3784 RVA: 0x000951B0 File Offset: 0x000935B0
	Private Shared Function CurseCharmValue(level As Levels) As Single
		Dim list As List(Of Levels) = New List(Of Levels)(Level.world1BossLevels)
		If Array.IndexOf(Of Levels)(Level.world1BossLevels, level) >= 0 Then
			Return 2F
		End If
		If Array.IndexOf(Of Levels)(Level.world2BossLevels, level) >= 0 Then
			Return 2.5F
		End If
		If Array.IndexOf(Of Levels)(Level.world3BossLevels, level) >= 0 Then
			Return 3F
		End If
		If Array.IndexOf(Of Levels)(Level.world4MiniBossLevels, level) >= 0 Then
			Return 1F
		End If
		If level = Levels.DicePalaceMain Then
			Return 1F
		End If
		If level = Levels.Devil Then
			Return 4F
		End If
		If Array.IndexOf(Of Levels)(Level.worldDLCBossLevels, level) >= 0 Then
			Return 3F
		End If
		If level = Levels.Saltbaker Then
			Return 4F
		End If
		Return 0F
	End Function

	' Token: 0x06000EC9 RID: 3785 RVA: 0x00095274 File Offset: 0x00093674
	Private Function completionPercentageOnly_CalculateCurseCharmLevel(playerId As PlayerId) As Integer
		If Not Me.GetLevelData(Levels.Graveyard).completed Then
			Return -1
		End If
		Dim array As Levels() = New Levels() { Levels.Veggies, Levels.Slime, Levels.FlyingBlimp, Levels.Flower, Levels.Frogs, Levels.Baroness, Levels.Clown, Levels.FlyingGenie, Levels.Dragon, Levels.FlyingBird, Levels.Bee, Levels.Pirate, Levels.SallyStagePlay, Levels.Mouse, Levels.Robot, Levels.FlyingMermaid, Levels.Train, Levels.DicePalaceBooze, Levels.DicePalaceChips, Levels.DicePalaceCigar, Levels.DicePalaceDomino, Levels.DicePalaceEightBall, Levels.DicePalaceFlyingHorse, Levels.DicePalaceFlyingMemory, Levels.DicePalaceRabbit, Levels.DicePalaceRoulette, Levels.DicePalaceMain, Levels.Devil, Levels.Airplane, Levels.RumRunners, Levels.OldMan, Levels.SnowCult, Levels.FlyingCowboy, Levels.Saltbaker }
		Dim num As Integer = Me.CalculateCurseCharmAccumulatedValue(playerId, array)
		Dim levelThreshold As Integer() = WeaponProperties.CharmCurse.levelThreshold
		For i As Integer = 0 To levelThreshold.Length - 1
			If num < levelThreshold(i) Then
				Return i - 1
			End If
		Next
		Return levelThreshold.Length - 1
	End Function

	' Token: 0x06000ECA RID: 3786 RVA: 0x000952E0 File Offset: 0x000936E0
	Private Function completionPercentageOnly_CurseCharmIsMaxLevel(playerId As PlayerId) As Boolean
		Dim levelThreshold As Integer() = WeaponProperties.CharmCurse.levelThreshold
		Return Me.completionPercentageOnly_CalculateCurseCharmLevel(playerId) = levelThreshold.Length - 1
	End Function

	' Token: 0x06000ECB RID: 3787 RVA: 0x00095304 File Offset: 0x00093704
	Public Function CalculateCurseCharmAccumulatedValue(playerId As PlayerId, levels As Levels()) As Integer
		Dim num As Single = 0F
		For Each levels2 As Levels In levels
			Dim levelData As PlayerData.PlayerLevelDataObject = Me.GetLevelData(levels2)
			If playerId = PlayerId.PlayerOne AndAlso levelData.curseCharmP1 Then
				num += PlayerData.CurseCharmValue(levels2)
			ElseIf playerId = PlayerId.PlayerTwo AndAlso levelData.curseCharmP2 Then
				num += PlayerData.CurseCharmValue(levels2)
			End If
		Next
		Return CInt(num)
	End Function

	' Token: 0x06000ECC RID: 3788 RVA: 0x00095378 File Offset: 0x00093778
	Public Function GetCompletionPercentage() As Single
		Dim list As List(Of Levels) = New List(Of Levels)()
		list.AddRange(Level.world1BossLevels)
		list.AddRange(Level.world2BossLevels)
		list.AddRange(Level.world3BossLevels)
		Dim num As Integer = 0
		Dim num2 As Integer = 0
		Dim num3 As Integer = 0
		Dim num4 As Integer = 0
		Dim num5 As Integer = 0
		Dim num6 As Integer = 0
		Dim num7 As Integer = 0
		Dim num8 As Integer = 0
		For Each levels As Levels In list
			Dim levelData As PlayerData.PlayerLevelDataObject = Me.GetLevelData(levels)
			If levelData.completed Then
				num += 1
				Dim difficultyBeaten As Level.Mode = levelData.difficultyBeaten
				If difficultyBeaten <> Level.Mode.Normal Then
					If difficultyBeaten = Level.Mode.Hard Then
						num2 += 1
						num6 += 1
					End If
				Else
					num2 += 1
				End If
			End If
		Next
		For Each levels2 As Levels In Level.platformingLevels
			Dim levelData2 As PlayerData.PlayerLevelDataObject = Me.GetLevelData(levels2)
			If levelData2.completed Then
				num3 += 1
			End If
		Next
		Dim num9 As Integer = Me.coinManager.NumCoinsCollected(False)
		Dim num10 As Integer = Me.NumSupers(PlayerId.PlayerOne)
		Dim levelData3 As PlayerData.PlayerLevelDataObject = Me.GetLevelData(Levels.DicePalaceMain)
		If levelData3.completed Then
			num4 += 1
			If levelData3.difficultyBeaten = Level.Mode.Hard Then
				num7 += 1
			End If
		End If
		Dim levelData4 As PlayerData.PlayerLevelDataObject = Me.GetLevelData(Levels.Devil)
		If levelData4.completed Then
			num5 += 1
			If levelData4.difficultyBeaten = Level.Mode.Hard Then
				num8 += 1
			End If
		End If
		Return CSng(num) * 1.5F + CSng(num3) * 1.5F + CSng(num9) * 0.5F + CSng(num10) * 1.5F + CSng((num2 * 2)) + CSng((num4 * 3)) + CSng((num5 * 4)) + CSng((num6 * 5)) + CSng((num7 * 7)) + CSng((num8 * 8))
	End Function

	' Token: 0x06000ECD RID: 3789 RVA: 0x00095560 File Offset: 0x00093960
	Public Function GetCompletionPercentageDLC() As Single
		If Not DLCManager.DLCEnabled() Then
			Return 0F
		End If
		Dim list As List(Of Levels) = New List(Of Levels)()
		list.AddRange(Level.worldDLCBossLevels)
		Dim num As Integer = 0
		Dim num2 As Integer = 0
		Dim num3 As Integer = 0
		Dim num4 As Integer = 0
		Dim num5 As Integer = 0
		Dim num6 As Integer = 0
		Dim num7 As Integer = 0
		Dim num8 As Integer = 0
		For Each levels As Levels In list
			Dim levelData As PlayerData.PlayerLevelDataObject = Me.GetLevelData(levels)
			If levelData.completed Then
				num += 1
				Dim difficultyBeaten As Level.Mode = levelData.difficultyBeaten
				If difficultyBeaten <> Level.Mode.Normal Then
					If difficultyBeaten = Level.Mode.Hard Then
						num2 += 1
						num3 += 1
					End If
				Else
					num2 += 1
				End If
			End If
		Next
		Dim num9 As Integer = Me.coinManager.NumCoinsCollected(True)
		Dim levelData2 As PlayerData.PlayerLevelDataObject = Me.GetLevelData(Levels.Saltbaker)
		If levelData2.completed Then
			num4 += 1
			If levelData2.difficultyBeaten = Level.Mode.Hard Then
				num5 += 1
			End If
		End If
		If Me.curseCharmPuzzleComplete Then
			num6 += 1
		End If
		If Me.GetLevelData(Levels.Graveyard).completed Then
			num7 += 1
		End If
		If Me.completionPercentageOnly_CurseCharmIsMaxLevel(PlayerId.PlayerOne) OrElse Me.completionPercentageOnly_CurseCharmIsMaxLevel(PlayerId.PlayerTwo) Then
			num8 += 1
		End If
		Return CSng(num) * 3.5F + CSng(num2) * 5F + CSng(num3) * 4.5F + CSng(num4) * 6F + CSng(num5) * 6F + CSng(num9) * 1F + CSng(num6) * 1F + CSng(num7) * 3F + CSng(num8) * 3F
	End Function

	' Token: 0x06000ECE RID: 3790 RVA: 0x0009571C File Offset: 0x00093B1C
	Public Function DeathCount(player As PlayerId) As Integer
		If player = PlayerId.PlayerOne Then
			Return Me.statictics.GetPlayer(PlayerId.PlayerOne).DeathCount()
		End If
		If player = PlayerId.PlayerTwo Then
			Return Me.statictics.GetPlayer(PlayerId.PlayerTwo).DeathCount()
		End If
		If player <> PlayerId.Any Then
			If player <> PlayerId.None Then
			End If
			Return 0
		End If
		Return Me.statictics.GetPlayer(PlayerId.PlayerOne).DeathCount() + Me.statictics.GetPlayer(PlayerId.PlayerTwo).DeathCount()
	End Function

	' Token: 0x06000ECF RID: 3791 RVA: 0x0009579C File Offset: 0x00093B9C
	Public Sub Die(player As PlayerId)
		If player <> PlayerId.PlayerOne Then
			If player <> PlayerId.PlayerTwo Then
				If player <> PlayerId.Any AndAlso player <> PlayerId.None Then
				End If
			Else
				Me.statictics.GetPlayer(PlayerId.PlayerTwo).Die()
			End If
		Else
			Me.statictics.GetPlayer(PlayerId.PlayerOne).Die()
		End If
	End Sub

	' Token: 0x06000ED0 RID: 3792 RVA: 0x00095804 File Offset: 0x00093C04
	Public Function GetNumParriesInRow(player As PlayerId) As Integer
		If player = PlayerId.PlayerOne Then
			Return Me.statictics.GetPlayer(PlayerId.PlayerOne).numParriesInRow
		End If
		If player = PlayerId.PlayerTwo Then
			Return Me.statictics.GetPlayer(PlayerId.PlayerTwo).numParriesInRow
		End If
		If player <> PlayerId.Any Then
			If player <> PlayerId.None Then
			End If
			Return 0
		End If
		Return Mathf.Max(Me.statictics.GetPlayer(PlayerId.PlayerOne).numParriesInRow, Me.statictics.GetPlayer(PlayerId.PlayerTwo).numParriesInRow)
	End Function

	' Token: 0x06000ED1 RID: 3793 RVA: 0x00095888 File Offset: 0x00093C88
	Public Sub SetNumParriesInRow(player As PlayerId, numParriesInRow As Integer)
		If player <> PlayerId.PlayerOne Then
			If player <> PlayerId.PlayerTwo Then
				If player <> PlayerId.Any AndAlso player <> PlayerId.None Then
				End If
			Else
				Me.statictics.GetPlayer(PlayerId.PlayerTwo).numParriesInRow = numParriesInRow
			End If
		Else
			Me.statictics.GetPlayer(PlayerId.PlayerOne).numParriesInRow = numParriesInRow
		End If
	End Sub

	' Token: 0x06000ED2 RID: 3794 RVA: 0x000958F0 File Offset: 0x00093CF0
	Public Sub IncrementKingOfGamesCounter()
		If Me.CountLevelsCompleted(Level.kingOfGamesLevels) = Level.kingOfGamesLevels.Length Then
			Return
		End If
		Me.chessBossAttemptCounter += 1
	End Sub

	' Token: 0x06000ED3 RID: 3795 RVA: 0x00095918 File Offset: 0x00093D18
	Public Sub ResetKingOfGamesCounter()
		Me.chessBossAttemptCounter = 0
	End Sub

	' Token: 0x06000ED4 RID: 3796 RVA: 0x00095924 File Offset: 0x00093D24
	Public Function TryActivateDjimmi() As Boolean
		If Me.DjimmiFreedCurrentRegion() Then
			If Me.DjimmiActivatedCurrentRegion() Then
				If Me.CurrentMap = Scenes.scene_map_world_DLC Then
					Me.djimmiActivatedInfiniteWishDLC = False
				Else
					Me.djimmiActivatedInfiniteWishBaseGame = False
				End If
				AudioManager.Play("sfx_worldmap_djimmi_deactivate")
			Else
				If Me.CurrentMap = Scenes.scene_map_world_DLC Then
					Me.djimmiActivatedInfiniteWishDLC = True
				Else
					Me.djimmiActivatedInfiniteWishBaseGame = True
				End If
				MapEventNotification.Current.ShowEvent(MapEventNotification.Type.DjimmiFreed)
			End If
			PlayerData.SaveCurrentFile()
			Return True
		End If
		If Me.djimmiActivatedCountedWish Then
			Me.djimmiActivatedCountedWish = False
			Me.djimmiWishes += 1
			PlayerData.SaveCurrentFile()
			AudioManager.Play("sfx_worldmap_djimmi_deactivate")
			Return True
		End If
		If Not Me.djimmiActivatedCountedWish AndAlso Me.djimmiWishes > 0 Then
			Me.djimmiActivatedCountedWish = True
			Me.djimmiWishes -= 1
			PlayerData.SaveCurrentFile()
			MapEventNotification.Current.ShowEvent(MapEventNotification.Type.Djimmi)
			Return True
		End If
		Return False
	End Function

	' Token: 0x06000ED5 RID: 3797 RVA: 0x00095A1A File Offset: 0x00093E1A
	Public Function DjimmiActivatedCurrentRegion() As Boolean
		Return If((Me.CurrentMap <> Scenes.scene_map_world_DLC), Me.DjimmiActivatedBaseGame(), Me.DjimmiActivatedDLC())
	End Function

	' Token: 0x06000ED6 RID: 3798 RVA: 0x00095A3A File Offset: 0x00093E3A
	Public Function DjimmiActivatedBaseGame() As Boolean
		Return Me.djimmiActivatedCountedWish OrElse Me.djimmiActivatedInfiniteWishBaseGame
	End Function

	' Token: 0x06000ED7 RID: 3799 RVA: 0x00095A50 File Offset: 0x00093E50
	Public Function DjimmiActivatedDLC() As Boolean
		Return Me.djimmiActivatedCountedWish OrElse Me.djimmiActivatedInfiniteWishDLC
	End Function

	' Token: 0x06000ED8 RID: 3800 RVA: 0x00095A66 File Offset: 0x00093E66
	Public Function DjimmiFreedCurrentRegion() As Boolean
		Return If((Me.CurrentMap <> Scenes.scene_map_world_DLC), Me.DjimmiFreedBaseGame(), Me.DjimmiFreedDLC())
	End Function

	' Token: 0x06000ED9 RID: 3801 RVA: 0x00095A88 File Offset: 0x00093E88
	Public Function DjimmiFreedBaseGame() As Boolean
		Return Me.CheckLevelsCompleted(Level.world1BossLevels) AndAlso Me.CheckLevelsCompleted(Level.world2BossLevels) AndAlso Me.CheckLevelsCompleted(Level.world3BossLevels) AndAlso Me.CheckLevelsCompleted(Level.world4BossLevels) AndAlso Me.CheckLevelsCompleted(Level.platformingLevels)
	End Function

	' Token: 0x06000EDA RID: 3802 RVA: 0x00095AE3 File Offset: 0x00093EE3
	Public Function DjimmiFreedDLC() As Boolean
		Return Me.CheckLevelsCompleted(Level.worldDLCBossLevelsWithSaltbaker) AndAlso Me.CheckLevelsCompleted(Level.kingOfGamesLevels)
	End Function

	' Token: 0x06000EDB RID: 3803 RVA: 0x00095B03 File Offset: 0x00093F03
	Public Sub DeactivateDjimmi()
		If Me.DjimmiFreedCurrentRegion() Then
			If Me.CurrentMap = Scenes.scene_map_world_DLC Then
				Me.djimmiActivatedInfiniteWishDLC = False
			Else
				Me.djimmiActivatedInfiniteWishBaseGame = False
			End If
		Else
			Me.djimmiActivatedCountedWish = False
		End If
		PlayerData.SaveCurrentFile()
	End Sub

	' Token: 0x040017B5 RID: 6069
	Public Shared platformingCoinIDs As PlayerData.LevelCoinIds() = New PlayerData.LevelCoinIds() { New PlayerData.LevelCoinIds(Levels.Platforming_Level_1_1, New String()() { New String() { "scene_level_platforming_1_1F::Level_Coin :: 5fd52d1b-a7f2-43a6-80e2-cb170cbc7d4d" }, New String() { "scene_level_platforming_1_1F::Level_Coin :: 63c021bf-52f0-41de-bedf-c77117d244cc" }, New String() { "scene_level_platforming_1_1F::Level_Coin :: 245037a6-1fa2-4167-a631-0723abff8138" }, New String() { "scene_level_platforming_1_1F::Level_Coin :: eaefb009-c117-4b9a-96c1-7abc5558d213" }, New String() { "scene_level_platforming_1_1F::Level_Coin :: 5526f7bc-a902-4c13-9e7a-1632a5abe378" } }), New PlayerData.LevelCoinIds(Levels.Platforming_Level_1_2, New String()() { New String() { "scene_level_platforming_1_2F::Level_Coin :: 323989de-349e-4740-a764-dbc12217a27c" }, New String() { "scene_level_platforming_1_2F::Level_Coin :: 55a46261-b14c-4065-9ada-18524eaed9f3" }, New String() { "scene_level_platforming_1_2F::Level_Coin :: da0983f6-62d4-4ace-81f2-cad7181d5fe9" }, New String() { "scene_level_platforming_1_2F::Level_Coin :: 7088ec51-4792-49c0-ab2c-c45ec9deb9f0" }, New String() { "scene_level_platforming_1_2F::Level_Coin :: e02954c1-ff76-4ba4-849f-90aae53a7787" } }), New PlayerData.LevelCoinIds(Levels.Platforming_Level_2_1, New String()() { New String() { "scene_level_platforming_2_1F::Level_Coin :: 24ef654a-a65b-4a1c-b5e5-c3c64e250646" }, New String() { "scene_level_platforming_2_1F::Level_Coin :: b8d96f03-d264-4a61-9ab9-07de34f660aa" }, New String() { "scene_level_platforming_2_1F::Level_Coin :: 383d9b3b-c280-4825-a6b3-1a21fe42d0ac" }, New String() { "scene_level_platforming_2_1F::Level_Coin :: f1b99bcd-0fa8-4aac-9a54-f310e173ddf9" }, New String() { "scene_level_platforming_2_1F::Level_Coin :: c763ef21-2ee7-491c-a143-b906856fed6c" } }), New PlayerData.LevelCoinIds(Levels.Platforming_Level_2_2, New String()() { New String() { "scene_level_platforming_2_2F::Level_Coin :: 9025a0e9-fff1-4f14-93d1-1930eef27405", "scene_level_platforming_2_2F::Level_Coin :: abbfb110-69d1-4948-9c70-223c6425c6f5", "scene_level_platforming_2_2F::Level_Coin :: 159497a2-3ded-4c0e-8852-4f6c41046df7", "scene_level_platforming_2_2F::Level_Coin :: 22bd722b-bf79-438b-92b0-56c638ae7114", "scene_level_platforming_2_2F::Level_Coin :: 84c8547b-b9b8-4fe9-9b0f-75980a3f5454", "scene_level_platforming_2_2F::Level_Coin :: d8d1b996-c4ef-4586-9c69-a3f18ebaeece", "scene_level_platforming_2_2F::Level_Coin :: 79695e06-f5c3-4826-96e8-5318399cdaf0", "scene_level_platforming_2_2F::Level_Coin :: 3aa60c71-a8c9-4b44-b53e-f954c9c70b29" }, New String() { "scene_level_platforming_2_2F::Level_Coin :: 284ea6f9-5db4-4f80-b0e5-1d9513a8acb7" }, New String() { "scene_level_platforming_2_2F::Level_Coin :: 43a8fc82-b8b8-4a92-b56f-c3e718b46b2c" }, New String() { "scene_level_platforming_2_2F::Level_Coin :: bf86d025-4524-4ce8-ba07-540ef3f61ed8" }, New String() { "scene_level_platforming_2_2F::Level_Coin :: a7c0e2b9-9560-4ed7-a3a4-428365222cb9" } }), New PlayerData.LevelCoinIds(Levels.Platforming_Level_3_1, New String()() { New String() { "scene_level_platforming_3_1F::Level_Coin :: 26ba2e1d-4b0a-4964-ba4d-f58655ef47db", "scene_level_platforming_3_1F::Level_Coin :: 8d1cd543-fa2f-41d6-9e50-d8ea356c9d26", "scene_level_platforming_3_1F::Level_Coin :: 90912b91-c396-429a-b061-0af90b666a0f", "scene_level_platforming_3_1F::Level_Coin :: 7a4de11e-fed9-479a-8ace-57bb7a00baa7", "scene_level_platforming_3_1F::Level_Coin :: eabb3294-336c-4615-8975-210343a039b5", "scene_level_platforming_3_1F::Level_Coin :: 6c032ae2-7bb9-4236-abc4-c27177201615", "scene_level_platforming_3_1F::Level_Coin :: 6fcd5ca7-9953-4343-a7f4-55d3fbc7d287", "scene_level_platforming_3_1F::Level_Coin :: e280e5f3-9fa1-4587-9139-84c127413e7a" }, New String() { "scene_level_platforming_3_1F::Level_Coin :: 0f13fbe6-1041-445f-97ed-1bbe2cb0339e", "scene_level_platforming_3_1F::Level_Coin :: 9aa051bf-5ec9-47b2-93f5-09f1495e78f2", "scene_level_platforming_3_1F::Level_Coin :: 4f4c2a23-244a-484b-84c9-ca5c6fc4e6bb", "scene_level_platforming_3_1F::Level_Coin :: 5c1e1ce4-055a-4ed6-8f5a-c667dbcac5af", "scene_level_platforming_3_1F::Level_Coin :: c1b74075-ae62-45ab-8d60-08286a35936f", "scene_level_platforming_3_1F::Level_Coin :: 5e1c290f-e2a4-4410-a52c-ba41ce7e56c5", "scene_level_platforming_3_1F::Level_Coin :: b18f581d-67b3-4020-b031-3a5bb62a9fa1", "scene_level_platforming_3_1F::Level_Coin :: 7b9e2b26-9132-4558-922b-ea400d4fdb0f" }, New String() { "scene_level_platforming_3_1F::Level_Coin :: 0086a9b3-87b8-4406-b97b-b94a1fd60bb0", "scene_level_platforming_3_1F::Level_Coin :: 273f231f-11d1-42db-888d-7d78696b934b", "scene_level_platforming_3_1F::Level_Coin :: cab629f0-54fa-43d3-8573-5d82db28e5c9", "scene_level_platforming_3_1F::Level_Coin :: b9ffa14a-984d-426b-8a96-7e71c58d8542", "scene_level_platforming_3_1F::Level_Coin :: b0f7e7a4-16a8-4a58-9abc-51f2aac1aac3", "scene_level_platforming_3_1F::Level_Coin :: 2b7cac59-e975-47f2-bf74-e49cb612266a", "scene_level_platforming_3_1F::Level_Coin :: e74bfad7-8657-4d6b-b853-9fef027e8600", "scene_level_platforming_3_1F::Level_Coin :: 6153c3cb-493f-465e-b6b2-dcddd5c0c50e" }, New String() { "scene_level_platforming_3_1F::Level_Coin :: 0a6fbbe4-5c13-4b17-9b58-91e7bbdacde4", "scene_level_platforming_3_1F::Level_Coin :: f72bdba8-cc0b-4d17-a83f-9892c3507b1c", "scene_level_platforming_3_1F::Level_Coin :: 5eaabcfd-0101-4ff5-92f4-a65d885be960", "scene_level_platforming_3_1F::Level_Coin :: 036a2830-7c80-443b-b9f6-1576dbf5cb33", "scene_level_platforming_3_1F::Level_Coin :: 1c782442-7e15-4a48-a66b-19c5a862e61e", "scene_level_platforming_3_1F::Level_Coin :: 01b6dc66-dd9a-4a6f-ac4d-e93a173395ef", "scene_level_platforming_3_1F::Level_Coin :: 4ca8faee-fb21-4f5a-b521-4deb89d853c3", "scene_level_platforming_3_1F::Level_Coin :: 5bfd4fdf-546c-4751-b2a7-eb99c7cdd2f4" }, New String() { "scene_level_platforming_3_1F::Level_Coin :: beb664ad-5577-4055-9164-b1b2f77430f3", "scene_level_platforming_3_1F::Level_Coin :: 2ffc0eef-d922-4825-bfb4-7377c16e197d", "scene_level_platforming_3_1F::Level_Coin :: 0c636a66-f96c-4046-9ccd-12897ab77649", "scene_level_platforming_3_1F::Level_Coin :: 267b5e81-84e6-4297-848c-bea5549b1690", "scene_level_platforming_3_1F::Level_Coin :: 76e64c16-b4d3-472f-85ae-d1dbd5c055e3", "scene_level_platforming_3_1F::Level_Coin :: 05b62218-8f30-4d74-bab4-7d27f4e0ab90", "scene_level_platforming_3_1F::Level_Coin :: 6222ae58-b0c8-44e8-81f6-417f00cc1be1", "scene_level_platforming_3_1F::Level_Coin :: 54c21221-4a03-4437-a2bd-a5972c3e2bfc" } }), New PlayerData.LevelCoinIds(Levels.Platforming_Level_3_2, New String()() { New String() { "scene_level_platforming_3_2F::Level_Coin :: 5da68904-6505-4841-9684-71d2931c1bd6" }, New String() { "scene_level_platforming_3_2F::Level_Coin :: 999c9b0d-d554-471d-ad96-ee6d57ccfd19" }, New String() { "scene_level_platforming_3_2F::Level_Coin :: cf0a7cae-d8d9-4be0-9502-8b8544606e04" }, New String() { "scene_level_platforming_3_2F::Level_Coin :: e671db16-cf6e-421c-937c-2b6f5c7ad0e7" }, New String() { "scene_level_platforming_3_2F::Level_Coin :: 084a7b75-e752-452f-8710-687db1e165fe" } }) }

	' Token: 0x040017B6 RID: 6070
	Private Const KEY As String = "cuphead_player_data_v1_slot_"

	' Token: 0x040017B7 RID: 6071
	Private Shared SAVE_FILE_KEYS As String() = New String() { "cuphead_player_data_v1_slot_0", "cuphead_player_data_v1_slot_1", "cuphead_player_data_v1_slot_2" }

	' Token: 0x040017B8 RID: 6072
	Public Shared WeaponsDLC As Weapon() = New Weapon() { Weapon.level_weapon_wide_shot, Weapon.level_weapon_upshot, Weapon.level_weapon_crackshot }

	' Token: 0x040017B9 RID: 6073
	Public Shared CharmsDLC As Charm() = New Charm() { Charm.charm_chalice, Charm.charm_healer, Charm.charm_curse }

	' Token: 0x040017BA RID: 6074
	Private Shared emptyDialoguerState As String = String.Empty

	' Token: 0x040017BB RID: 6075
	Private Shared _CurrentSaveFileIndex As Integer = 0

	' Token: 0x040017BC RID: 6076
	Private Shared _initialized As Boolean = False

	' Token: 0x040017BD RID: 6077
	Public Shared inGame As Boolean = False

	' Token: 0x040017BE RID: 6078
	Private Shared _saveFiles As PlayerData()

	' Token: 0x040017BF RID: 6079
	Private Shared _playerDatatInitHandler As PlayerData.PlayerDataInitHandler

	' Token: 0x040017C0 RID: 6080
	Public isPlayer1Mugman As Boolean

	' Token: 0x040017C1 RID: 6081
	Public hasMadeFirstPurchase As Boolean

	' Token: 0x040017C2 RID: 6082
	Public hasBeatenAnyBossOnEasy As Boolean

	' Token: 0x040017C3 RID: 6083
	Public hasBeatenAnyDLCBossOnEasy As Boolean

	' Token: 0x040017C4 RID: 6084
	Public hasUnlockedFirstSuper As Boolean

	' Token: 0x040017C5 RID: 6085
	Public shouldShowShopkeepTooltip As Boolean

	' Token: 0x040017C6 RID: 6086
	Public shouldShowTurtleTooltip As Boolean

	' Token: 0x040017C7 RID: 6087
	Public shouldShowCanteenTooltip As Boolean

	' Token: 0x040017C8 RID: 6088
	Public shouldShowForkTooltip As Boolean

	' Token: 0x040017C9 RID: 6089
	Public shouldShowKineDiceTooltip As Boolean

	' Token: 0x040017CA RID: 6090
	Public shouldShowMausoleumTooltip As Boolean

	' Token: 0x040017CB RID: 6091
	Public hasUnlockedBoatman As Boolean

	' Token: 0x040017CC RID: 6092
	Public shouldShowBoatmanTooltip As Boolean

	' Token: 0x040017CD RID: 6093
	Public shouldShowChaliceTooltip As Boolean

	' Token: 0x040017CE RID: 6094
	Public hasTalkedToChaliceFan As Boolean

	' Token: 0x040017CF RID: 6095
	Public curseCharmPuzzleOrder As Integer()

	' Token: 0x040017D0 RID: 6096
	Public curseCharmPuzzleComplete As Boolean

	' Token: 0x040017D1 RID: 6097
	Public currentChessBossZone As MapCastleZones.Zone

	' Token: 0x040017D2 RID: 6098
	Public usedChessBossZones As List(Of MapCastleZones.Zone) = New List(Of MapCastleZones.Zone)()

	' Token: 0x040017D3 RID: 6099
	Public chessBossAttemptCounter As Integer

	' Token: 0x040017D4 RID: 6100
	Public djimmiActivatedCountedWish As Boolean

	' Token: 0x040017D5 RID: 6101
	Public djimmiActivatedInfiniteWishBaseGame As Boolean

	' Token: 0x040017D6 RID: 6102
	Public djimmiActivatedInfiniteWishDLC As Boolean

	' Token: 0x040017D7 RID: 6103
	Public djimmiWishes As Integer = 3

	' Token: 0x040017D8 RID: 6104
	Public djimmiFreed As Boolean

	' Token: 0x040017D9 RID: 6105
	Public djimmiFreedDLC As Boolean

	' Token: 0x040017DA RID: 6106
	Public dummy As Integer

	' Token: 0x040017DB RID: 6107
	<SerializeField()>
	Private loadouts As PlayerData.PlayerLoadouts = New PlayerData.PlayerLoadouts()

	' Token: 0x040017DC RID: 6108
	<SerializeField()>
	Private _isHardModeAvailable As Boolean

	' Token: 0x040017DD RID: 6109
	<SerializeField()>
	Private _isHardModeAvailableDLC As Boolean

	' Token: 0x040017DE RID: 6110
	<SerializeField()>
	Private _isTutorialCompleted As Boolean

	' Token: 0x040017DF RID: 6111
	<SerializeField()>
	Private _isFlyingTutorialCompleted As Boolean

	' Token: 0x040017E0 RID: 6112
	<SerializeField()>
	Private _isChaliceTutorialCompleted As Boolean

	' Token: 0x040017E1 RID: 6113
	<SerializeField()>
	Private inventories As PlayerData.PlayerInventories = New PlayerData.PlayerInventories()

	' Token: 0x040017E2 RID: 6114
	Public dialoguerState As String

	' Token: 0x040017E3 RID: 6115
	<SerializeField()>
	Public coinManager As PlayerData.PlayerCoinManager = New PlayerData.PlayerCoinManager()

	' Token: 0x040017E4 RID: 6116
	Private levelCoinManager As PlayerData.PlayerCoinManager = New PlayerData.PlayerCoinManager()

	' Token: 0x040017E5 RID: 6117
	Public unlockedBlackAndWhite As Boolean

	' Token: 0x040017E6 RID: 6118
	Public unlocked2Strip As Boolean

	' Token: 0x040017E7 RID: 6119
	Public unlockedChaliceRecolor As Boolean

	' Token: 0x040017E8 RID: 6120
	Public vintageAudioEnabled As Boolean

	' Token: 0x040017E9 RID: 6121
	Public pianoAudioEnabled As Boolean

	' Token: 0x040017EA RID: 6122
	Public filter As BlurGamma.Filter

	' Token: 0x040017EB RID: 6123
	<SerializeField()>
	Private mapDataManager As PlayerData.MapDataManager = New PlayerData.MapDataManager()

	' Token: 0x040017EC RID: 6124
	<SerializeField()>
	Private levelDataManager As PlayerData.PlayerLevelDataManager = New PlayerData.PlayerLevelDataManager()

	' Token: 0x040017ED RID: 6125
	<SerializeField()>
	Private statictics As PlayerData.PlayerStats = New PlayerData.PlayerStats()

	' Token: 0x0200040C RID: 1036
	Public Structure LevelCoinIds
		' Token: 0x06000EDD RID: 3805 RVA: 0x00095FBD File Offset: 0x000943BD
		Public Sub New(level As Levels, coins As String()())
			Me.levelId = level
			Me.coinIds = coins
		End Sub

		' Token: 0x040017F3 RID: 6131
		Public levelId As Levels

		' Token: 0x040017F4 RID: 6132
		Public coinIds As String()()
	End Structure

	' Token: 0x0200040D RID: 1037
	' (Invoke) Token: 0x06000EDF RID: 3807
	Public Delegate Sub PlayerDataInitHandler(success As Boolean)

	' Token: 0x0200040E RID: 1038
	<Serializable()>
	Public Class PlayerLoadouts
		' Token: 0x06000EE2 RID: 3810 RVA: 0x00095FCD File Offset: 0x000943CD
		Public Sub New()
			Me.playerOne = New PlayerData.PlayerLoadouts.PlayerLoadout()
			Me.playerTwo = New PlayerData.PlayerLoadouts.PlayerLoadout()
		End Sub

		' Token: 0x06000EE3 RID: 3811 RVA: 0x00095FEB File Offset: 0x000943EB
		Public Sub New(playerOne As PlayerData.PlayerLoadouts.PlayerLoadout, playerTwo As PlayerData.PlayerLoadouts.PlayerLoadout)
			Me.playerOne = playerOne
			Me.playerTwo = playerTwo
		End Sub

		' Token: 0x06000EE4 RID: 3812 RVA: 0x00096001 File Offset: 0x00094401
		Public Function GetPlayerLoadout(player As PlayerId) As PlayerData.PlayerLoadouts.PlayerLoadout
			If player = PlayerId.PlayerOne Then
				Return Me.playerOne
			End If
			If player <> PlayerId.PlayerTwo Then
				Return Nothing
			End If
			Return Me.playerTwo
		End Function

		' Token: 0x040017F5 RID: 6133
		Public playerOne As PlayerData.PlayerLoadouts.PlayerLoadout

		' Token: 0x040017F6 RID: 6134
		Public playerTwo As PlayerData.PlayerLoadouts.PlayerLoadout

		' Token: 0x0200040F RID: 1039
		<Serializable()>
		Public Class PlayerLoadout
			' Token: 0x06000EE5 RID: 3813 RVA: 0x00096024 File Offset: 0x00094424
			Public Sub New()
				Me.primaryWeapon = Weapon.level_weapon_peashot
				Me.secondaryWeapon = Weapon.None
				Me.super = Super.None
				Me.charm = Charm.None
			End Sub

			' Token: 0x1700025E RID: 606
			' (get) Token: 0x06000EE6 RID: 3814 RVA: 0x00096058 File Offset: 0x00094458
			' (set) Token: 0x06000EE7 RID: 3815 RVA: 0x00096060 File Offset: 0x00094460
			Public Property HasEquippedSecondaryRegularWeapon As Boolean

			' Token: 0x1700025F RID: 607
			' (get) Token: 0x06000EE8 RID: 3816 RVA: 0x00096069 File Offset: 0x00094469
			' (set) Token: 0x06000EE9 RID: 3817 RVA: 0x00096071 File Offset: 0x00094471
			Public Property HasEquippedSecondarySHMUPWeapon As Boolean

			' Token: 0x17000260 RID: 608
			' (get) Token: 0x06000EEA RID: 3818 RVA: 0x0009607A File Offset: 0x0009447A
			' (set) Token: 0x06000EEB RID: 3819 RVA: 0x00096082 File Offset: 0x00094482
			Public Property MustNotifySwitchRegularWeapon As Boolean

			' Token: 0x17000261 RID: 609
			' (get) Token: 0x06000EEC RID: 3820 RVA: 0x0009608B File Offset: 0x0009448B
			' (set) Token: 0x06000EED RID: 3821 RVA: 0x00096093 File Offset: 0x00094493
			Public Property MustNotifySwitchSHMUPWeapon As Boolean

			' Token: 0x040017F7 RID: 6135
			Public primaryWeapon As Weapon

			' Token: 0x040017F8 RID: 6136
			Public secondaryWeapon As Weapon

			' Token: 0x040017F9 RID: 6137
			Public super As Super

			' Token: 0x040017FA RID: 6138
			Public charm As Charm
		End Class
	End Class

	' Token: 0x02000410 RID: 1040
	<Serializable()>
	Public Class PlayerInventories
		' Token: 0x06000EEF RID: 3823 RVA: 0x000960BA File Offset: 0x000944BA
		Public Function GetPlayer(player As PlayerId) As PlayerData.PlayerInventory
			If player = PlayerId.PlayerOne Then
				Return Me.playerOne
			End If
			If player <> PlayerId.PlayerTwo Then
				Return Nothing
			End If
			Return Me.playerTwo
		End Function

		' Token: 0x040017FF RID: 6143
		Public dummy As Integer

		' Token: 0x04001800 RID: 6144
		Public playerOne As PlayerData.PlayerInventory = New PlayerData.PlayerInventory()

		' Token: 0x04001801 RID: 6145
		Public playerTwo As PlayerData.PlayerInventory = New PlayerData.PlayerInventory()
	End Class

	' Token: 0x02000411 RID: 1041
	<Serializable()>
	Public Class PlayerInventory
		' Token: 0x06000EF0 RID: 3824 RVA: 0x000960E0 File Offset: 0x000944E0
		Public Sub New()
			Me.money = 0
			Me._weapons = New List(Of Weapon)()
			Me._supers = New List(Of Super)()
			Me._charms = New List(Of Charm)()
			Me._weapons.Add(Weapon.level_weapon_peashot)
			Me._weapons.Add(Weapon.plane_weapon_peashot)
		End Sub

		' Token: 0x06000EF1 RID: 3825 RVA: 0x0009613B File Offset: 0x0009453B
		Public Function IsUnlocked(weapon As Weapon) As Boolean
			Return Me._weapons.Contains(weapon)
		End Function

		' Token: 0x06000EF2 RID: 3826 RVA: 0x00096149 File Offset: 0x00094549
		Public Function IsUnlocked(super As Super) As Boolean
			Return Me._supers.Contains(super)
		End Function

		' Token: 0x06000EF3 RID: 3827 RVA: 0x00096157 File Offset: 0x00094557
		Public Function IsUnlocked(charm As Charm) As Boolean
			Return Me._charms.Contains(charm)
		End Function

		' Token: 0x06000EF4 RID: 3828 RVA: 0x00096168 File Offset: 0x00094568
		Public Function Buy(value As Weapon) As Boolean
			If Me.IsUnlocked(value) Then
				Return False
			End If
			If Me.money < WeaponProperties.GetValue(value) Then
				Return False
			End If
			Me.money -= WeaponProperties.GetValue(value)
			Me._weapons.Add(value)
			Me.newPurchase = True
			Return True
		End Function

		' Token: 0x06000EF5 RID: 3829 RVA: 0x000961C0 File Offset: 0x000945C0
		Public Function Buy(value As Super) As Boolean
			If Me.IsUnlocked(value) Then
				Return False
			End If
			If Me.money < WeaponProperties.GetValue(value) Then
				Return False
			End If
			Me.money -= WeaponProperties.GetValue(value)
			Me._supers.Add(value)
			Me.newPurchase = True
			Return True
		End Function

		' Token: 0x06000EF6 RID: 3830 RVA: 0x00096218 File Offset: 0x00094618
		Public Function Buy(value As Charm) As Boolean
			If Me.IsUnlocked(value) Then
				Return False
			End If
			If Me.money < WeaponProperties.GetValue(value) Then
				Return False
			End If
			Me.money -= WeaponProperties.GetValue(value)
			Me._charms.Add(value)
			Me.newPurchase = True
			Return True
		End Function

		' Token: 0x04001802 RID: 6146
		Public Const STARTING_MONEY As Integer = 0

		' Token: 0x04001803 RID: 6147
		Public money As Integer

		' Token: 0x04001804 RID: 6148
		Public newPurchase As Boolean

		' Token: 0x04001805 RID: 6149
		Public _weapons As List(Of Weapon)

		' Token: 0x04001806 RID: 6150
		Public _supers As List(Of Super)

		' Token: 0x04001807 RID: 6151
		Public _charms As List(Of Charm)
	End Class

	' Token: 0x02000412 RID: 1042
	<Serializable()>
	Public Class PlayerCoinManager
		' Token: 0x06000EF7 RID: 3831 RVA: 0x00096270 File Offset: 0x00094670
		Public Sub New()
			Me.LevelsAndCoins = New List(Of PlayerData.PlayerCoinManager.LevelAndCoins)()
			Dim enumerator As IEnumerator = [Enum].GetValues(GetType(Levels)).GetEnumerator()
			Try
				While enumerator.MoveNext()
					Dim obj As Object = enumerator.Current
					Dim levels As Levels = CType(obj, Levels)
					Dim levelAndCoins As PlayerData.PlayerCoinManager.LevelAndCoins = New PlayerData.PlayerCoinManager.LevelAndCoins()
					levelAndCoins.level = levels
					Me.LevelsAndCoins.Add(levelAndCoins)
				End While
			Finally
				Dim disposable As IDisposable = TryCast(enumerator, IDisposable)
				Dim disposable2 As IDisposable = disposable
				If disposable IsNot Nothing Then
					disposable2.Dispose()
				End If
			End Try
		End Sub

		' Token: 0x06000EF8 RID: 3832 RVA: 0x00096318 File Offset: 0x00094718
		Public Function GetCoinCollected(coin As LevelCoin) As Boolean
			Return Me.GetCoinCollected(coin.GlobalID)
		End Function

		' Token: 0x06000EF9 RID: 3833 RVA: 0x00096326 File Offset: 0x00094726
		Public Function GetCoinCollected(coinID As String) As Boolean
			Return Me.ContainsCoin(coinID) AndAlso Me.GetCoin(coinID).collected
		End Function

		' Token: 0x06000EFA RID: 3834 RVA: 0x00096344 File Offset: 0x00094744
		Public Function NumCoinsCollected() As Integer
			Dim num As Integer = 0
			For Each playerCoinProperties As PlayerData.PlayerCoinProperties In Me.coins
				If playerCoinProperties.collected Then
					num += 1
				End If
			Next
			Return num
		End Function

		' Token: 0x06000EFB RID: 3835 RVA: 0x000963AC File Offset: 0x000947AC
		Public Function NumCoinsCollected(DLC As Boolean) As Integer
			Dim num As Integer = 0
			For Each playerCoinProperties As PlayerData.PlayerCoinProperties In Me.coins
				If playerCoinProperties.collected AndAlso Me.IsDLCCoin(playerCoinProperties.coinID) = DLC Then
					num += 1
				End If
			Next
			Return num
		End Function

		' Token: 0x06000EFC RID: 3836 RVA: 0x00096428 File Offset: 0x00094828
		Public Sub SetCoinValue(coin As LevelCoin, collected As Boolean, player As PlayerId)
			Me.SetCoinValue(coin.GlobalID, collected, player)
		End Sub

		' Token: 0x06000EFD RID: 3837 RVA: 0x00096438 File Offset: 0x00094838
		Public Sub SetCoinValue(coinID As String, collected As Boolean, player As PlayerId)
			If Me.ContainsCoin(coinID) Then
				Dim coin As PlayerData.PlayerCoinProperties = Me.GetCoin(coinID)
				coin.collected = collected
				coin.player = player
			Else
				Me.AddCoin(New PlayerData.PlayerCoinProperties(coinID) With { .collected = collected })
			End If
		End Sub

		' Token: 0x06000EFE RID: 3838 RVA: 0x00096481 File Offset: 0x00094881
		Private Function GetCoin(coin As LevelCoin) As PlayerData.PlayerCoinProperties
			Return Me.GetCoin(coin.GlobalID)
		End Function

		' Token: 0x06000EFF RID: 3839 RVA: 0x00096490 File Offset: 0x00094890
		Private Function GetCoin(coinID As String) As PlayerData.PlayerCoinProperties
			For i As Integer = 0 To Me.coins.Count - 1
				If Me.coins(i).coinID = coinID Then
					Return Me.coins(i)
				End If
			Next
			Return Nothing
		End Function

		' Token: 0x06000F00 RID: 3840 RVA: 0x000964E3 File Offset: 0x000948E3
		Private Sub AddCoin(coin As LevelCoin)
			Me.AddCoin(coin.GlobalID)
		End Sub

		' Token: 0x06000F01 RID: 3841 RVA: 0x000964F1 File Offset: 0x000948F1
		Private Sub AddCoin(coinID As String)
			If Not Me.ContainsCoin(coinID) Then
				Me.coins.Add(New PlayerData.PlayerCoinProperties(coinID))
			End If
			Me.RegisterCoin(coinID)
		End Sub

		' Token: 0x06000F02 RID: 3842 RVA: 0x00096517 File Offset: 0x00094917
		Private Sub AddCoin(coin As PlayerData.PlayerCoinProperties)
			If Not Me.ContainsCoin(coin.coinID) Then
				Me.coins.Add(coin)
			End If
			Me.RegisterCoin(coin.coinID)
		End Sub

		' Token: 0x06000F03 RID: 3843 RVA: 0x00096544 File Offset: 0x00094944
		Private Sub RegisterCoin(coinID As String)
			Dim platformingLevel As PlatformingLevel = TryCast(Level.Current, PlatformingLevel)
			If platformingLevel Then
				Dim levelsAndCoins As List(Of PlayerData.PlayerCoinManager.LevelAndCoins) = Me.LevelsAndCoins
				Dim num As Integer = -1
				For i As Integer = 0 To levelsAndCoins.Count - 1
					If levelsAndCoins(i).level = platformingLevel.CurrentLevel Then
						num = i
					End If
				Next
				If num >= 0 Then
					For j As Integer = 0 To platformingLevel.LevelCoinsIDs.Count - 1
						If platformingLevel.LevelCoinsIDs(j).CoinID = coinID Then
							Select Case j
								Case 0
									levelsAndCoins(num).Coin1Collected = True
								Case 1
									levelsAndCoins(num).Coin2Collected = True
								Case 2
									levelsAndCoins(num).Coin3Collected = True
								Case 3
									levelsAndCoins(num).Coin4Collected = True
								Case 4
									levelsAndCoins(num).Coin5Collected = True
							End Select
							Exit For
						End If
					Next
				End If
			ElseIf Map.Current IsNot Nothing Then
				Dim levelsAndCoins2 As List(Of PlayerData.PlayerCoinManager.LevelAndCoins) = PlayerData.Data.coinManager.LevelsAndCoins
				Dim num2 As Integer = -1
				For k As Integer = 0 To levelsAndCoins2.Count - 1
					If levelsAndCoins2(k).level = Map.Current.level Then
						num2 = k
					End If
				Next
				If num2 >= 0 Then
					For l As Integer = 0 To Map.Current.LevelCoinsIDs.Count - 1
						If Map.Current.LevelCoinsIDs(l).CoinID = coinID Then
							Select Case l
								Case 0
									levelsAndCoins2(num2).Coin1Collected = True
								Case 1
									levelsAndCoins2(num2).Coin2Collected = True
								Case 2
									levelsAndCoins2(num2).Coin3Collected = True
								Case 3
									levelsAndCoins2(num2).Coin4Collected = True
								Case 4
									levelsAndCoins2(num2).Coin5Collected = True
							End Select
							Exit For
						End If
					Next
				End If
			End If
			Dim flag As Boolean = True
			For Each levels As Levels In Level.platformingLevels
				If PlayerData.Data.GetNumCoinsCollectedInLevel(levels) < 5 Then
					flag = False
				End If
			Next
			If flag Then
				OnlineManager.Instance.[Interface].UnlockAchievement(PlayerId.Any, "FoundAllLevelMoney")
			End If
			If PlayerData.Data.NumCoinsCollectedMainGame >= 40 Then
				OnlineManager.Instance.[Interface].UnlockAchievement(PlayerId.Any, "FoundAllMoney")
			End If
		End Sub

		' Token: 0x06000F04 RID: 3844 RVA: 0x00096830 File Offset: 0x00094C30
		Private Function ContainsCoin(coin As LevelCoin) As Boolean
			Return Me.ContainsCoin(coin.GlobalID)
		End Function

		' Token: 0x06000F05 RID: 3845 RVA: 0x00096840 File Offset: 0x00094C40
		Private Function ContainsCoin(coinID As String) As Boolean
			For i As Integer = 0 To Me.coins.Count - 1
				If Me.coins(i).coinID = coinID Then
					Return True
				End If
			Next
			Return False
		End Function

		' Token: 0x06000F06 RID: 3846 RVA: 0x00096888 File Offset: 0x00094C88
		Private Function IsDLCCoin(coin As LevelCoin) As Boolean
			Return Me.IsDLCCoin(coin.GlobalID)
		End Function

		' Token: 0x06000F07 RID: 3847 RVA: 0x00096898 File Offset: 0x00094C98
		Private Function IsDLCCoin(coinID As String) As Boolean
			Return coinID = "619e92f1-e0fd-4f6e-9c2d-5ce5dbaf393f" OrElse coinID = "scene_level_chalice_tutorial::Level_Coin :: 578c0218-df9e-4cdd-932a-a1277b5b7129" OrElse coinID = "a37b3d37-a32e-4b88-a583-34489496494d" OrElse coinID = "25f15554-d229-4330-96cc-ac8a13c18ea0" OrElse coinID = "eacf4228-e200-4839-9d79-3439cfcc5824" OrElse coinID = "47f7edb1-b5c5-4afb-9acb-a46f5e6df557" OrElse coinID = "3826615a-498b-4158-af7b-0d01acbc18c8" OrElse coinID = "d52b1cc6-414c-4a7c-9f8a-250316566d58" OrElse coinID = "fc2c48cd-5dec-472a-ae18-dccfc94232c6" OrElse coinID = "16732bc8-7230-467a-a9ac-ff9c62ab7657" OrElse coinID = "e0c6e8bc-0c56-4e52-a9a1-c53887f5ca4c" OrElse coinID = "19090606-09e8-4e56-92ac-e08200926b94" OrElse coinID = "39bfe6d8-0dbc-4886-9998-52c67b57969e" OrElse coinID = "7f3422f5-6650-497f-9c35-9735b64100d6" OrElse coinID = "9970ad6a-560a-4ae3-9d15-a6b636b67024" OrElse coinID = "3367b9b0-da35-4c81-a895-2720862b5b1b"
		End Function

		' Token: 0x04001808 RID: 6152
		Public dummy As Integer

		' Token: 0x04001809 RID: 6153
		Public coins As List(Of PlayerData.PlayerCoinProperties) = New List(Of PlayerData.PlayerCoinProperties)()

		' Token: 0x0400180A RID: 6154
		Public hasMigratedCoins As Boolean

		' Token: 0x0400180B RID: 6155
		Public LevelsAndCoins As List(Of PlayerData.PlayerCoinManager.LevelAndCoins) = New List(Of PlayerData.PlayerCoinManager.LevelAndCoins)()

		' Token: 0x02000413 RID: 1043
		<Serializable()>
		Public Class LevelAndCoins
			' Token: 0x0400180C RID: 6156
			Public level As Levels

			' Token: 0x0400180D RID: 6157
			Public Coin1Collected As Boolean

			' Token: 0x0400180E RID: 6158
			Public Coin2Collected As Boolean

			' Token: 0x0400180F RID: 6159
			Public Coin3Collected As Boolean

			' Token: 0x04001810 RID: 6160
			Public Coin4Collected As Boolean

			' Token: 0x04001811 RID: 6161
			Public Coin5Collected As Boolean
		End Class
	End Class

	' Token: 0x02000414 RID: 1044
	<Serializable()>
	Public Class PlayerCoinProperties
		' Token: 0x06000F09 RID: 3849 RVA: 0x000969AB File Offset: 0x00094DAB
		Public Sub New()
		End Sub

		' Token: 0x06000F0A RID: 3850 RVA: 0x000969C9 File Offset: 0x00094DC9
		Public Sub New(coin As LevelCoin)
			Me.coinID = coin.GlobalID
		End Sub

		' Token: 0x06000F0B RID: 3851 RVA: 0x000969F3 File Offset: 0x00094DF3
		Public Sub New(coinID As String)
			Me.coinID = coinID
		End Sub

		' Token: 0x04001812 RID: 6162
		Public coinID As String = String.Empty

		' Token: 0x04001813 RID: 6163
		Public collected As Boolean

		' Token: 0x04001814 RID: 6164
		Public player As PlayerId = PlayerId.None
	End Class

	' Token: 0x02000415 RID: 1045
	<Serializable()>
	Public Class MapData
		' Token: 0x04001815 RID: 6165
		Public mapId As Scenes

		' Token: 0x04001816 RID: 6166
		Public sessionStarted As Boolean

		' Token: 0x04001817 RID: 6167
		Public hasVisitedDieHouse As Boolean

		' Token: 0x04001818 RID: 6168
		Public hasKingDiceDisappeared As Boolean

		' Token: 0x04001819 RID: 6169
		Public playerOnePosition As Vector3 = Vector3.zero

		' Token: 0x0400181A RID: 6170
		Public playerTwoPosition As Vector3 = Vector3.zero

		' Token: 0x0400181B RID: 6171
		<NonSerialized()>
		Public enteringFrom As PlayerData.MapData.EntryMethod

		' Token: 0x02000416 RID: 1046
		Public Enum EntryMethod
			' Token: 0x0400181D RID: 6173
			None
			' Token: 0x0400181E RID: 6174
			DiceHouseLeft
			' Token: 0x0400181F RID: 6175
			DiceHouseRight
			' Token: 0x04001820 RID: 6176
			Boatman
		End Enum
	End Class

	' Token: 0x02000417 RID: 1047
	<Serializable()>
	Public Class MapDataManager
		' Token: 0x06000F0D RID: 3853 RVA: 0x00096A36 File Offset: 0x00094E36
		Public Sub New()
			Me.mapData = New List(Of PlayerData.MapData)()
		End Sub

		' Token: 0x06000F0E RID: 3854 RVA: 0x00096A50 File Offset: 0x00094E50
		Public Function GetCurrentMapData() As PlayerData.MapData
			Return Me.GetMapData(Me.currentMap)
		End Function

		' Token: 0x06000F0F RID: 3855 RVA: 0x00096A60 File Offset: 0x00094E60
		Public Function GetMapData(map As Scenes) As PlayerData.MapData
			For i As Integer = 0 To Me.mapData.Count - 1
				If Me.mapData(i).mapId = map Then
					Return Me.mapData(i)
				End If
			Next
			Dim mapData As PlayerData.MapData = New PlayerData.MapData()
			mapData.mapId = map
			Me.mapData.Add(mapData)
			Return mapData
		End Function

		' Token: 0x04001821 RID: 6177
		Public currentMap As Scenes = Scenes.scene_map_world_1

		' Token: 0x04001822 RID: 6178
		Public mapData As List(Of PlayerData.MapData)
	End Class

	' Token: 0x02000418 RID: 1048
	<Serializable()>
	Public Class PlayerLevelDataManager
		' Token: 0x06000F10 RID: 3856 RVA: 0x00096AC8 File Offset: 0x00094EC8
		Public Sub New()
			Me.levelObjects = New List(Of PlayerData.PlayerLevelDataObject)()
			For Each levels As Levels In EnumUtils.GetValues(Of Levels)()
				Dim playerLevelDataObject As PlayerData.PlayerLevelDataObject = New PlayerData.PlayerLevelDataObject(levels)
				playerLevelDataObject.levelID = levels
				Me.levelObjects.Add(playerLevelDataObject)
			Next
		End Sub

		' Token: 0x06000F11 RID: 3857 RVA: 0x00096B20 File Offset: 0x00094F20
		Public Function GetLevelData(levelID As Levels) As PlayerData.PlayerLevelDataObject
			For i As Integer = 0 To Me.levelObjects.Count - 1
				If Me.levelObjects(i).levelID = levelID Then
					Return Me.levelObjects(i)
				End If
			Next
			Dim playerLevelDataObject As PlayerData.PlayerLevelDataObject = New PlayerData.PlayerLevelDataObject(levelID)
			Me.levelObjects.Add(playerLevelDataObject)
			Return playerLevelDataObject
		End Function

		' Token: 0x04001823 RID: 6179
		Public dummy As Integer

		' Token: 0x04001824 RID: 6180
		Public levelObjects As List(Of PlayerData.PlayerLevelDataObject)
	End Class

	' Token: 0x02000419 RID: 1049
	<Serializable()>
	Public Class PlayerLevelDataObject
		' Token: 0x06000F12 RID: 3858 RVA: 0x00096B81 File Offset: 0x00094F81
		Public Sub New(id As Levels)
			Me.levelID = id
		End Sub

		' Token: 0x04001825 RID: 6181
		Public levelID As Levels

		' Token: 0x04001826 RID: 6182
		Public completed As Boolean

		' Token: 0x04001827 RID: 6183
		Public completedAsChaliceP1 As Boolean

		' Token: 0x04001828 RID: 6184
		Public completedAsChaliceP2 As Boolean

		' Token: 0x04001829 RID: 6185
		Public played As Boolean

		' Token: 0x0400182A RID: 6186
		Public grade As LevelScoringData.Grade

		' Token: 0x0400182B RID: 6187
		Public difficultyBeaten As Level.Mode

		' Token: 0x0400182C RID: 6188
		Public bestTime As Single = Single.MaxValue

		' Token: 0x0400182D RID: 6189
		Public curseCharmP1 As Boolean

		' Token: 0x0400182E RID: 6190
		Public curseCharmP2 As Boolean

		' Token: 0x0400182F RID: 6191
		Public bgmPlayListCurrent As Integer
	End Class

	' Token: 0x0200041A RID: 1050
	<Serializable()>
	Public Class PlayerStats
		' Token: 0x06000F14 RID: 3860 RVA: 0x00096BB9 File Offset: 0x00094FB9
		Public Function GetPlayer(player As PlayerId) As PlayerData.PlayerStat
			If player = PlayerId.PlayerOne Then
				Return Me.playerOne
			End If
			If player <> PlayerId.PlayerTwo Then
				Return Nothing
			End If
			Return Me.playerTwo
		End Function

		' Token: 0x04001830 RID: 6192
		Public dummy As Integer

		' Token: 0x04001831 RID: 6193
		Public playerOne As PlayerData.PlayerStat = New PlayerData.PlayerStat()

		' Token: 0x04001832 RID: 6194
		Public playerTwo As PlayerData.PlayerStat = New PlayerData.PlayerStat()
	End Class

	' Token: 0x0200041B RID: 1051
	<Serializable()>
	Public Class PlayerStat
		' Token: 0x06000F15 RID: 3861 RVA: 0x00096BDC File Offset: 0x00094FDC
		Public Sub New()
			Me.numDeaths = 0
			Me.numParriesInRow = 0
		End Sub

		' Token: 0x06000F16 RID: 3862 RVA: 0x00096BF2 File Offset: 0x00094FF2
		Public Function DeathCount() As Integer
			Return Me.numDeaths
		End Function

		' Token: 0x06000F17 RID: 3863 RVA: 0x00096BFA File Offset: 0x00094FFA
		Public Sub Die()
			Me.numDeaths += 1
		End Sub

		' Token: 0x04001833 RID: 6195
		Public numDeaths As Integer

		' Token: 0x04001834 RID: 6196
		Public numParriesInRow As Integer
	End Class
End Class
