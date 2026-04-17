Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports TMPro
Imports UnityEngine
Imports UnityEngine.UI

' Token: 0x020009AA RID: 2474
Public Class SlotSelectScreen
	Inherits AbstractMonoBehaviour

	' Token: 0x170004B5 RID: 1205
	' (get) Token: 0x06003A01 RID: 14849 RVA: 0x0020F881 File Offset: 0x0020DC81
	Private ReadOnly Property RespondToDeadPlayer As Boolean
		Get
			Return True
		End Get
	End Property

	' Token: 0x06003A02 RID: 14850 RVA: 0x0020F884 File Offset: 0x0020DC84
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Cuphead.Init(False)
		Me.input = New CupheadInput.AnyPlayerInput(False)
		Me.isConsole = PlatformHelper.IsConsole
		PlayerData.inGame = False
		Dim list As List(Of Text) = New List(Of Text)(Me.mainMenuItems)
		Dim list2 As List(Of SlotSelectScreen.MainMenuItem) = New List(Of SlotSelectScreen.MainMenuItem)(CType([Enum].GetValues(GetType(SlotSelectScreen.MainMenuItem)), SlotSelectScreen.MainMenuItem()))
		If Me.isConsole Then
			Me.mainMenuItems(4).gameObject.SetActive(False)
			list.RemoveAt(4)
			list2.RemoveAt(4)
		End If
		If Not PlatformHelper.ShowDLCMenuItem Then
			Me.mainMenuItems(3).gameObject.SetActive(False)
			list.RemoveAt(3)
			list2.RemoveAt(3)
		End If
		If Not PlatformHelper.ShowAchievements Then
			Me.mainMenuItems(1).gameObject.SetActive(False)
			list.RemoveAt(1)
			list2.RemoveAt(1)
		End If
		Me.mainMenuItems = list.ToArray()
		Me._availableMainMenuItems = list2.ToArray()
	End Sub

	' Token: 0x06003A03 RID: 14851 RVA: 0x0020F97C File Offset: 0x0020DD7C
	Private Sub Update()
		If Me.dataStatus = SlotSelectScreen.SaveDataStatus.Received Then
			Me.dataStatus = SlotSelectScreen.SaveDataStatus.Initialized
			MyBase.StartCoroutine(Me.allDataLoaded_cr())
		End If
		Me.timeSinceStart += Time.deltaTime
		Select Case Me.state
			Case SlotSelectScreen.State.MainMenu
				Me.UpdateMainMenu()
			Case SlotSelectScreen.State.AchievementsMenu
				Me.UpdateAchievementsMenu()
			Case SlotSelectScreen.State.OptionsMenu
				Me.UpdateOptionsMenu()
			Case SlotSelectScreen.State.DLC
				Me.UpdateDLCMenu()
			Case SlotSelectScreen.State.SlotSelect
				Me.UpdateSlotSelect()
			Case SlotSelectScreen.State.ConfirmDelete
				Me.UpdateConfirmDelete()
			Case SlotSelectScreen.State.PlayerSelect
				Me.UpdatePlayerSelect()
		End Select
	End Sub

	' Token: 0x06003A04 RID: 14852 RVA: 0x0020FA38 File Offset: 0x0020DE38
	Private Sub Start()
		If StartScreenAudio.Instance Is Nothing Then
			Global.UnityEngine.[Object].Instantiate(Resources.Load("Audio/TitleScreenAudio"))
			AddHandler SceneLoader.OnLoaderCompleteEvent, AddressOf Me.PlayMusic
		End If
		CupheadLevelCamera.Current.StartSmoothShake(8F, 3F, 2)
		Me.SetState(SlotSelectScreen.State.InitializeStorage)
		PlayerData.Init(AddressOf Me.OnPlayerDataInitialized)
	End Sub

	' Token: 0x06003A05 RID: 14853 RVA: 0x0020FAA3 File Offset: 0x0020DEA3
	Private Sub PlayMusic()
		AudioManager.PlayBGMPlaylistManually(True)
	End Sub

	' Token: 0x06003A06 RID: 14854 RVA: 0x0020FAAB File Offset: 0x0020DEAB
	Private Sub OnDestroy()
		RemoveHandler SceneLoader.OnLoaderCompleteEvent, AddressOf Me.PlayMusic
	End Sub

	' Token: 0x06003A07 RID: 14855 RVA: 0x0020FAC0 File Offset: 0x0020DEC0
	Private Sub SetState(state As SlotSelectScreen.State)
		Me.state = state
		Me.mainMenuChild.gameObject.SetActive(state = SlotSelectScreen.State.MainMenu)
		Me.LoadingChild.gameObject.SetActive(state = SlotSelectScreen.State.InitializeStorage)
		Me.slotSelectChild.gameObject.SetActive(state = SlotSelectScreen.State.SlotSelect OrElse state = SlotSelectScreen.State.ConfirmDelete OrElse state = SlotSelectScreen.State.PlayerSelect)
		Me.confirmDeleteChild.gameObject.SetActive(state = SlotSelectScreen.State.ConfirmDelete)
		Me.confirmPrompt.gameObject.SetActive(state = SlotSelectScreen.State.MainMenu OrElse state = SlotSelectScreen.State.OptionsMenu OrElse state = SlotSelectScreen.State.SlotSelect OrElse state = SlotSelectScreen.State.ConfirmDelete OrElse state = SlotSelectScreen.State.PlayerSelect)
		Me.confirmGlyph.gameObject.SetActive(Me.confirmPrompt.gameObject.activeSelf)
		Me.confirmSpacer.gameObject.SetActive(Me.confirmPrompt.gameObject.activeSelf)
		Me.backPrompt.gameObject.SetActive(state = SlotSelectScreen.State.OptionsMenu OrElse state = SlotSelectScreen.State.SlotSelect OrElse state = SlotSelectScreen.State.ConfirmDelete OrElse state = SlotSelectScreen.State.PlayerSelect OrElse state = SlotSelectScreen.State.AchievementsMenu OrElse state = SlotSelectScreen.State.DLC)
		Me.backGlyph.gameObject.SetActive(Me.backPrompt.gameObject.activeSelf)
		Me.backSpacer.gameObject.SetActive(Me.backPrompt.gameObject.activeSelf)
		Me.deletePrompt.gameObject.SetActive(state = SlotSelectScreen.State.SlotSelect)
		Me.deleteGlyph.gameObject.SetActive(Me.deletePrompt.gameObject.activeSelf)
		Me.deleteSpacer.gameObject.SetActive(Me.deletePrompt.gameObject.activeSelf)
		Me.storePrompt.gameObject.SetActive(state = SlotSelectScreen.State.DLC AndAlso DLCManager.CanRedirectToStore() AndAlso Not DLCManager.DLCEnabled())
		Me.storeGlyph.gameObject.SetActive(Me.storePrompt.gameObject.activeSelf)
		Me.storeSpacer.gameObject.SetActive(Me.storePrompt.gameObject.activeSelf)
		Me.playerProfiles.gameObject.SetActive(state = SlotSelectScreen.State.SlotSelect OrElse state = SlotSelectScreen.State.MainMenu)
		PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerOne, state = SlotSelectScreen.State.SlotSelect OrElse state = SlotSelectScreen.State.MainMenu)
	End Sub

	' Token: 0x06003A08 RID: 14856 RVA: 0x0020FD14 File Offset: 0x0020E114
	Private Sub UpdateMainMenu()
		If Me.timeSinceStart < 0.75F Then
			Return
		End If
		If Me.GetButtonDown(CupheadButton.MenuDown) Then
			AudioManager.Play("level_menu_move")
			Me._mainMenuSelection = (Me._mainMenuSelection + 1) Mod Me.mainMenuItems.Length
		End If
		If Me.GetButtonDown(CupheadButton.MenuUp) Then
			AudioManager.Play("level_menu_move")
			Me._mainMenuSelection -= 1
			If Me._mainMenuSelection < 0 Then
				Me._mainMenuSelection = Me.mainMenuItems.Length - 1
			End If
		End If
		For i As Integer = 0 To Me.mainMenuItems.Length - 1
			Me.mainMenuItems(i).color = If((Me._mainMenuSelection <> i), Me.mainMenuUnselectedColor, Me.mainMenuSelectedColor)
		Next
		If Me.GetButtonDown(CupheadButton.Accept) Then
			AudioManager.Play("level_menu_select")
			Select Case Me._availableMainMenuItems(Me._mainMenuSelection)
				Case SlotSelectScreen.MainMenuItem.Start
					Me.SetState(SlotSelectScreen.State.SlotSelect)
					For j As Integer = 0 To 3 - 1
						Me.slots(j).Init(j)
					Next
				Case SlotSelectScreen.MainMenuItem.Achievements
					Me.SetState(SlotSelectScreen.State.AchievementsMenu)
					Me.achievements.ShowAchievements()
				Case SlotSelectScreen.MainMenuItem.Options
					Me.SetState(SlotSelectScreen.State.OptionsMenu)
					Me.options.ShowMainOptionMenu()
				Case SlotSelectScreen.MainMenuItem.DLC
					Me.SetState(SlotSelectScreen.State.DLC)
					Me.dlcMenu.ShowDLCMenu()
				Case SlotSelectScreen.MainMenuItem.[Exit]
					Application.Quit()
			End Select
		End If
	End Sub

	' Token: 0x06003A09 RID: 14857 RVA: 0x0020FEA4 File Offset: 0x0020E2A4
	Private Sub UpdateOptionsMenu()
		Me.prompts.gameObject.SetActive(Not Cuphead.Current.controlMapper.isOpen)
		If Not Me.options.optionMenuOpen AndAlso Not Me.options.justClosed Then
			Me.SetState(SlotSelectScreen.State.MainMenu)
		End If
	End Sub

	' Token: 0x06003A0A RID: 14858 RVA: 0x0020FEFA File Offset: 0x0020E2FA
	Private Sub UpdateAchievementsMenu()
		If Not Me.achievements.achievementsMenuOpen AndAlso Not Me.achievements.justClosed Then
			Me.SetState(SlotSelectScreen.State.MainMenu)
		End If
	End Sub

	' Token: 0x06003A0B RID: 14859 RVA: 0x0020FF23 File Offset: 0x0020E323
	Private Sub UpdateDLCMenu()
		If Not Me.dlcMenu.dlcMenuOpen AndAlso Not Me.dlcMenu.justClosed Then
			Me.SetState(SlotSelectScreen.State.MainMenu)
		End If
	End Sub

	' Token: 0x06003A0C RID: 14860 RVA: 0x0020FF4C File Offset: 0x0020E34C
	Private Sub UpdatePlayerSelect()
		If PlayerData.inGame Then
			Return
		End If
		If Me.GetButtonDown(CupheadButton.MenuLeft) OrElse Me.GetButtonDown(CupheadButton.MenuRight) Then
			AudioManager.Play("level_menu_move")
			Me.slots(Me._slotSelection).SwapSprite()
		ElseIf Me.GetButtonDown(CupheadButton.Cancel) Then
			AudioManager.Play("level_menu_select")
			For i As Integer = 0 To Me.slots.Length - 1
				If i <> Me._slotSelection Then
					MyBase.StartCoroutine(Me.activate_noise_cr(i))
				End If
			Next
			Me.slots(Me._slotSelection).StopSelectingPlayer()
			Me.SetState(SlotSelectScreen.State.SlotSelect)
		ElseIf Me.GetButtonDown(CupheadButton.Accept) Then
			AudioManager.Play("ui_menu_confirm")
			Me.slots(Me._slotSelection).PlayAnimation(Me._slotSelection)
			MyBase.StartCoroutine(Me.game_start_cr())
		End If
	End Sub

	' Token: 0x06003A0D RID: 14861 RVA: 0x00210044 File Offset: 0x0020E444
	Private Sub UpdateSlotSelect()
		If PlayerData.inGame Then
			Return
		End If
		If Me.GetButtonDown(CupheadButton.MenuDown) Then
			AudioManager.Play("ui_saveslot_move")
			Me._slotSelection = (Me._slotSelection + 1) Mod 3
		End If
		If Me.GetButtonDown(CupheadButton.MenuUp) Then
			AudioManager.Play("ui_saveslot_move")
			Me._slotSelection -= 1
			If Me._slotSelection < 0 Then
				Me._slotSelection = 2
			End If
		End If
		For i As Integer = 0 To 3 - 1
			Me.slots(i).SetSelected(Me._slotSelection = i)
		Next
		If Me.GetButtonDown(CupheadButton.Accept) Then
			AudioManager.Play("level_select")
			For j As Integer = 0 To Me.slots.Length - 1
				If j <> Me._slotSelection Then
					Me.slots(j).noise.gameObject.SetActive(False)
				End If
			Next
			Me.slots(Me._slotSelection).EnterSelectMenu()
			Me.SetState(SlotSelectScreen.State.PlayerSelect)
		ElseIf Me.GetButtonDown(CupheadButton.Cancel) Then
			AudioManager.Play("level_menu_select")
			Me.SetState(SlotSelectScreen.State.MainMenu)
		ElseIf Not Me.slots(Me._slotSelection).IsEmpty AndAlso Me.GetButtonDown(CupheadButton.EquipMenu) Then
			AudioManager.Play("level_menu_select")
			Me.SetState(SlotSelectScreen.State.ConfirmDelete)
			Me._confirmDeleteSelection = 1
			Me.confirmDeleteSlotTitle.text = Me.slots(Me._slotSelection).GetSlotTitle()
			Me.confirmDeleteSlotTitle.font = Me.slots(Me._slotSelection).GetSlotTitleFont()
			Me.confirmDeleteSlotSeparator.text = Me.slots(Me._slotSelection).GetSlotSeparator()
			Me.confirmDeleteSlotSeparator.font = Me.slots(Me._slotSelection).GetSlotSeparatorFont()
			Me.confirmDeleteSlotPercentage.text = Me.slots(Me._slotSelection).GetSlotPercentage() + "?"
			Me.confirmDeleteSlotPercentage.font = Me.slots(Me._slotSelection).GetSlotPercentageFont()
		End If
	End Sub

	' Token: 0x06003A0E RID: 14862 RVA: 0x0021026C File Offset: 0x0020E66C
	Private Iterator Function game_start_cr() As IEnumerator
		PlayerData.inGame = True
		For i As Integer = 0 To 45 - 1
			Yield Nothing
		Next
		Me.EnterGame()
		Return
	End Function

	' Token: 0x06003A0F RID: 14863 RVA: 0x00210288 File Offset: 0x0020E688
	Private Iterator Function activate_noise_cr(index As Integer) As IEnumerator
		For i As Integer = 0 To 10 - 1
			Yield Nothing
		Next
		Me.slots(index).noise.gameObject.SetActive(True)
		Yield Nothing
		Return
	End Function

	' Token: 0x06003A10 RID: 14864 RVA: 0x002102AC File Offset: 0x0020E6AC
	Private Sub EnterGame()
		DLCManager.RefreshDLC()
		PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerOne, False)
		PlayerData.CurrentSaveFileIndex = Me._slotSelection
		PlayerManager.player1IsMugman = Me.slots(Me._slotSelection).isPlayer1Mugman
		PlayerData.GetDataForSlot(Me._slotSelection).isPlayer1Mugman = PlayerManager.player1IsMugman
		If Not DLCManager.DLCEnabled() Then
			Dim data As PlayerData = PlayerData.Data
			For i As Integer = 0 To 2 - 1
				Dim playerId As PlayerId = CType(i, PlayerId)
				Dim playerLoadout As PlayerData.PlayerLoadouts.PlayerLoadout = data.Loadouts.GetPlayerLoadout(playerId)
				If Array.IndexOf(Of Weapon)(PlayerData.WeaponsDLC, playerLoadout.secondaryWeapon) >= 0 Then
					playerLoadout.secondaryWeapon = Weapon.None
				End If
				If Array.IndexOf(Of Weapon)(PlayerData.WeaponsDLC, playerLoadout.primaryWeapon) >= 0 Then
					playerLoadout.primaryWeapon = Weapon.level_weapon_peashot
					If playerLoadout.secondaryWeapon = Weapon.level_weapon_peashot Then
						playerLoadout.secondaryWeapon = Weapon.None
					End If
				End If
				If Array.IndexOf(Of Charm)(PlayerData.CharmsDLC, playerLoadout.charm) >= 0 Then
					playerLoadout.charm = Charm.None
				End If
			Next
		End If
		Level.ResetPreviousLevelInfo()
		If Not Me.slots(Me._slotSelection).IsEmpty Then
			If Not DLCManager.DLCEnabled() AndAlso PlayerData.Data.CurrentMap = Scenes.scene_map_world_DLC Then
				PlayerData.Data.CurrentMap = Scenes.scene_map_world_1
				PlayerData.Data.GetMapData(Scenes.scene_map_world_1).sessionStarted = False
			End If
			SceneLoader.LoadScene(PlayerData.Data.CurrentMap, SceneLoader.Transition.Fade, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, Nothing)
		Else
			PlayerData.Data.CurrentMap = Scenes.scene_map_world_1
			PlayerData.Data.GetMapData(Scenes.scene_map_world_1).sessionStarted = False
			Cutscene.Load(Scenes.scene_level_house_elder_kettle, Scenes.scene_cutscene_intro, SceneLoader.Transition.Fade, SceneLoader.Transition.Fade, SceneLoader.Icon.Hourglass)
		End If
		PlayerData.inGame = True
		If StartScreenAudio.Instance IsNot Nothing Then
			Global.UnityEngine.[Object].Destroy(StartScreenAudio.Instance.gameObject)
		End If
	End Sub

	' Token: 0x06003A11 RID: 14865 RVA: 0x00210464 File Offset: 0x0020E864
	Private Sub UpdateConfirmDelete()
		If Me.GetButtonDown(CupheadButton.MenuDown) Then
			AudioManager.Play("level_menu_move")
			Me._confirmDeleteSelection = (Me._confirmDeleteSelection + 1) Mod 2
		End If
		If Me.GetButtonDown(CupheadButton.MenuUp) Then
			AudioManager.Play("level_menu_move")
			Me._confirmDeleteSelection -= 1
			If Me._confirmDeleteSelection < 0 Then
				Me._confirmDeleteSelection = 1
			End If
		End If
		For i As Integer = 0 To 2 - 1
			Me.confirmDeleteItems(i).color = If((Me._confirmDeleteSelection <> i), Me.confirmDeleteUnselectedColor, Me.confirmDeleteSelectedColor)
		Next
		If Me.GetButtonDown(CupheadButton.Accept) Then
			Dim confirmDeleteSelection As SlotSelectScreen.ConfirmDeleteItem = CType(Me._confirmDeleteSelection, SlotSelectScreen.ConfirmDeleteItem)
			If confirmDeleteSelection <> SlotSelectScreen.ConfirmDeleteItem.Yes Then
				If confirmDeleteSelection = SlotSelectScreen.ConfirmDeleteItem.No Then
					AudioManager.Play("level_menu_select")
					Me.SetState(SlotSelectScreen.State.SlotSelect)
				End If
			Else
				AudioManager.Play("level_menu_select")
				PlayerData.ClearSlot(Me._slotSelection)
				Me.slots(Me._slotSelection).Init(Me._slotSelection)
				Me.SetState(SlotSelectScreen.State.SlotSelect)
			End If
		End If
		If Me.GetButtonDown(CupheadButton.Cancel) Then
			AudioManager.Play("level_menu_select")
			Me.SetState(SlotSelectScreen.State.SlotSelect)
		End If
	End Sub

	' Token: 0x06003A12 RID: 14866 RVA: 0x002105A0 File Offset: 0x0020E9A0
	Private Sub OnPlayerDataInitialized(success As Boolean)
		If Not success Then
			PlayerData.Init(AddressOf Me.OnPlayerDataInitialized)
			Return
		End If
		If PlatformHelper.IsConsole AndAlso Not PlatformHelper.PreloadSettingsData Then
			SettingsData.LoadFromCloud(AddressOf Me.OnSettingsDataLoaded)
		Else
			Me.dataStatus = SlotSelectScreen.SaveDataStatus.Received
		End If
	End Sub

	' Token: 0x06003A13 RID: 14867 RVA: 0x002105F6 File Offset: 0x0020E9F6
	Private Sub OnSettingsDataLoaded(success As Boolean)
		If Not success Then
			SettingsData.LoadFromCloud(AddressOf Me.OnSettingsDataLoaded)
			Return
		End If
		SettingsData.ApplySettingsOnStartup()
		MyBase.StartCoroutine(Me.allDataLoaded_cr())
	End Sub

	' Token: 0x06003A14 RID: 14868 RVA: 0x00210624 File Offset: 0x0020EA24
	Private Iterator Function allDataLoaded_cr() As IEnumerator
		Yield Nothing
		Me.SetState(SlotSelectScreen.State.MainMenu)
		For i As Integer = 0 To 3 - 1
			Me.slots(i).Init(i)
		Next
		ControllerDisconnectedPrompt.Instance.allowedToShow = True
		Me.options = Me.optionsPrefab.InstantiatePrefab(Of OptionsGUI)()
		Me.options.rectTransform.SetParent(Me.optionsRoot, False)
		Me.options.Init(False)
		If PlatformHelper.ShowAchievements Then
			Me.achievements = Me.achievementsPrefab.InstantiatePrefab(Of AchievementsGUI)()
			Me.achievements.rectTransform.SetParent(Me.achievementsRoot, False)
			Me.achievements.Init(False)
		End If
		If PlatformHelper.ShowDLCMenuItem Then
			Me.dlcMenu = Me.dlcMenuPrefab.InstantiatePrefab(Of DLCGUI)()
			Me.dlcMenu.rectTransform.SetParent(Me.dlcMenuRoot, False)
			Me.dlcMenu.Init(False)
		End If
		If PlatformHelper.IsConsole Then
			PlayerManager.LoadControllerMappings(PlayerId.PlayerOne)
		End If
		Me.SetRichPresence()
		Return
	End Function

	' Token: 0x06003A15 RID: 14869 RVA: 0x0021063F File Offset: 0x0020EA3F
	Protected Function GetButtonDown(button As CupheadButton) As Boolean
		Return Me.input.GetButtonDown(button)
	End Function

	' Token: 0x06003A16 RID: 14870 RVA: 0x00210655 File Offset: 0x0020EA55
	Private Sub SetRichPresence()
		OnlineManager.Instance.[Interface].SetRichPresence(PlayerId.Any, "SlotSelect", True)
	End Sub

	' Token: 0x040041EB RID: 16875
	Private state As SlotSelectScreen.State

	' Token: 0x040041EC RID: 16876
	<SerializeField()>
	Private LoadingChild As RectTransform

	' Token: 0x040041ED RID: 16877
	<SerializeField()>
	Private mainMenuChild As RectTransform

	' Token: 0x040041EE RID: 16878
	<SerializeField()>
	Private slotSelectChild As RectTransform

	' Token: 0x040041EF RID: 16879
	<SerializeField()>
	Private confirmDeleteChild As RectTransform

	' Token: 0x040041F0 RID: 16880
	<SerializeField()>
	Private mainMenuItems As Text()

	' Token: 0x040041F1 RID: 16881
	<SerializeField()>
	Private slots As SlotSelectScreenSlot()

	' Token: 0x040041F2 RID: 16882
	<SerializeField()>
	Private confirmDeleteItems As Text()

	' Token: 0x040041F3 RID: 16883
	<SerializeField()>
	Private playerProfiles As RectTransform

	' Token: 0x040041F4 RID: 16884
	<SerializeField()>
	Private confirmPrompt As RectTransform

	' Token: 0x040041F5 RID: 16885
	<SerializeField()>
	Private confirmGlyph As RectTransform

	' Token: 0x040041F6 RID: 16886
	<SerializeField()>
	Private confirmSpacer As RectTransform

	' Token: 0x040041F7 RID: 16887
	<SerializeField()>
	Private backPrompt As RectTransform

	' Token: 0x040041F8 RID: 16888
	<SerializeField()>
	Private backGlyph As RectTransform

	' Token: 0x040041F9 RID: 16889
	<SerializeField()>
	Private backSpacer As RectTransform

	' Token: 0x040041FA RID: 16890
	<SerializeField()>
	Private storePrompt As RectTransform

	' Token: 0x040041FB RID: 16891
	<SerializeField()>
	Private storeGlyph As RectTransform

	' Token: 0x040041FC RID: 16892
	<SerializeField()>
	Private storeSpacer As RectTransform

	' Token: 0x040041FD RID: 16893
	<SerializeField()>
	Private deletePrompt As RectTransform

	' Token: 0x040041FE RID: 16894
	<SerializeField()>
	Private deleteGlyph As RectTransform

	' Token: 0x040041FF RID: 16895
	<SerializeField()>
	Private deleteSpacer As RectTransform

	' Token: 0x04004200 RID: 16896
	<SerializeField()>
	Private prompts As RectTransform

	' Token: 0x04004201 RID: 16897
	<SerializeField()>
	Private mainMenuSelectedColor As Color

	' Token: 0x04004202 RID: 16898
	<SerializeField()>
	Private mainMenuUnselectedColor As Color

	' Token: 0x04004203 RID: 16899
	<SerializeField()>
	Private confirmDeleteSelectedColor As Color

	' Token: 0x04004204 RID: 16900
	<SerializeField()>
	Private confirmDeleteUnselectedColor As Color

	' Token: 0x04004205 RID: 16901
	<SerializeField()>
	Private optionsPrefab As OptionsGUI

	' Token: 0x04004206 RID: 16902
	<SerializeField()>
	Private optionsRoot As RectTransform

	' Token: 0x04004207 RID: 16903
	<SerializeField()>
	Private achievementsPrefab As AchievementsGUI

	' Token: 0x04004208 RID: 16904
	<SerializeField()>
	Private achievementsRoot As RectTransform

	' Token: 0x04004209 RID: 16905
	<SerializeField()>
	Private dlcMenuPrefab As DLCGUI

	' Token: 0x0400420A RID: 16906
	<SerializeField()>
	Private dlcMenuRoot As RectTransform

	' Token: 0x0400420B RID: 16907
	<SerializeField()>
	Private confirmDeleteSlotTitle As TMP_Text

	' Token: 0x0400420C RID: 16908
	<SerializeField()>
	Private confirmDeleteSlotSeparator As TMP_Text

	' Token: 0x0400420D RID: 16909
	<SerializeField()>
	Private confirmDeleteSlotPercentage As TMP_Text

	' Token: 0x0400420E RID: 16910
	Private options As OptionsGUI

	' Token: 0x0400420F RID: 16911
	Private achievements As AchievementsGUI

	' Token: 0x04004210 RID: 16912
	Private dlcMenu As DLCGUI

	' Token: 0x04004211 RID: 16913
	Private _slotSelection As Integer

	' Token: 0x04004212 RID: 16914
	Private _mainMenuSelection As Integer

	' Token: 0x04004213 RID: 16915
	Private _availableMainMenuItems As SlotSelectScreen.MainMenuItem()

	' Token: 0x04004214 RID: 16916
	Private _confirmDeleteSelection As Integer

	' Token: 0x04004215 RID: 16917
	Private input As CupheadInput.AnyPlayerInput

	' Token: 0x04004216 RID: 16918
	Private isConsole As Boolean

	' Token: 0x04004217 RID: 16919
	Private Const PATH As String = "Audio/TitleScreenAudio"

	' Token: 0x04004218 RID: 16920
	Private timeSinceStart As Single

	' Token: 0x04004219 RID: 16921
	Private dataStatus As SlotSelectScreen.SaveDataStatus

	' Token: 0x020009AB RID: 2475
	Public Enum State
		' Token: 0x0400421B RID: 16923
		InitializeStorage
		' Token: 0x0400421C RID: 16924
		MainMenu
		' Token: 0x0400421D RID: 16925
		AchievementsMenu
		' Token: 0x0400421E RID: 16926
		OptionsMenu
		' Token: 0x0400421F RID: 16927
		DLC
		' Token: 0x04004220 RID: 16928
		SlotSelect
		' Token: 0x04004221 RID: 16929
		ConfirmDelete
		' Token: 0x04004222 RID: 16930
		PlayerSelect
	End Enum

	' Token: 0x020009AC RID: 2476
	Public Enum MainMenuItem
		' Token: 0x04004224 RID: 16932
		Start
		' Token: 0x04004225 RID: 16933
		Achievements
		' Token: 0x04004226 RID: 16934
		Options
		' Token: 0x04004227 RID: 16935
		DLC
		' Token: 0x04004228 RID: 16936
		[Exit]
	End Enum

	' Token: 0x020009AD RID: 2477
	Public Enum ConfirmDeleteItem
		' Token: 0x0400422A RID: 16938
		Yes
		' Token: 0x0400422B RID: 16939
		No
	End Enum

	' Token: 0x020009AE RID: 2478
	Private Enum SaveDataStatus
		' Token: 0x0400422D RID: 16941
		Uninitialized
		' Token: 0x0400422E RID: 16942
		Received
		' Token: 0x0400422F RID: 16943
		Initialized
	End Enum
End Class
