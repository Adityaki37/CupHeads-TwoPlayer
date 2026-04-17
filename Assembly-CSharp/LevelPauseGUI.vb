Imports System
Imports System.Collections
Imports System.Diagnostics
Imports UnityEngine
Imports UnityEngine.UI

' Token: 0x02000483 RID: 1155
Public Class LevelPauseGUI
	Inherits AbstractPauseGUI

	' Token: 0x170002C9 RID: 713
	' (get) Token: 0x060011E9 RID: 4585 RVA: 0x000A788A File Offset: 0x000A5C8A
	' (set) Token: 0x060011EA RID: 4586 RVA: 0x000A7891 File Offset: 0x000A5C91
	Public Shared Property COLOR_SELECTED As Color

	' Token: 0x170002CA RID: 714
	' (get) Token: 0x060011EB RID: 4587 RVA: 0x000A7899 File Offset: 0x000A5C99
	' (set) Token: 0x060011EC RID: 4588 RVA: 0x000A78A0 File Offset: 0x000A5CA0
	Public Shared Property COLOR_INACTIVE As Color

	' Token: 0x1400002E RID: 46
	' (add) Token: 0x060011ED RID: 4589 RVA: 0x000A78A8 File Offset: 0x000A5CA8
	' (remove) Token: 0x060011EE RID: 4590 RVA: 0x000A78DC File Offset: 0x000A5CDC
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Shared Event OnPauseEvent As Action

	' Token: 0x1400002F RID: 47
	' (add) Token: 0x060011EF RID: 4591 RVA: 0x000A7910 File Offset: 0x000A5D10
	' (remove) Token: 0x060011F0 RID: 4592 RVA: 0x000A7944 File Offset: 0x000A5D44
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Shared Event OnUnpauseEvent As Action

	' Token: 0x170002CB RID: 715
	' (get) Token: 0x060011F1 RID: 4593 RVA: 0x000A7978 File Offset: 0x000A5D78
	' (set) Token: 0x060011F2 RID: 4594 RVA: 0x000A7980 File Offset: 0x000A5D80
	Private Property selection As Integer
		Get
			Return Me._selection
		End Get
		Set(value As Integer)
			Dim flag As Boolean = value > Me._selection
			Dim num As Integer = CInt(Mathf.Repeat(CSng(value), CSng(Me.menuItems.Length)))
			While Not Me.menuItems(num).gameObject.activeSelf
				num = If((Not flag), (num - 1), (num + 1))
				num = CInt(Mathf.Repeat(CSng(num), CSng(Me.menuItems.Length)))
			End While
			Me._selection = num
			Me.UpdateSelection()
		End Set
	End Property

	' Token: 0x170002CC RID: 716
	' (get) Token: 0x060011F3 RID: 4595 RVA: 0x000A79F8 File Offset: 0x000A5DF8
	Protected Overrides ReadOnly Property CanPause As Boolean
		Get
			Return Level.Current.Started AndAlso Not Level.Current.Ending AndAlso PauseManager.state <> PauseManager.State.Paused AndAlso Not SceneLoader.CurrentlyLoading AndAlso Not Me.forceDisablePause
		End Get
	End Property

	' Token: 0x060011F4 RID: 4596 RVA: 0x000A7A44 File Offset: 0x000A5E44
	Private Sub OnEnable()
		AddHandler Localization.OnLanguageChangedEvent, AddressOf Me.onLanguageChangedEventHandler
	End Sub

	' Token: 0x060011F5 RID: 4597 RVA: 0x000A7A57 File Offset: 0x000A5E57
	Private Sub OnDisable()
		RemoveHandler Localization.OnLanguageChangedEvent, AddressOf Me.onLanguageChangedEventHandler
	End Sub

	' Token: 0x060011F6 RID: 4598 RVA: 0x000A7A6A File Offset: 0x000A5E6A
	Protected Overrides Sub Awake()
		MyBase.Awake()
		LevelPauseGUI.COLOR_SELECTED = Me.menuItems(0).color
		LevelPauseGUI.COLOR_INACTIVE = Me.menuItems(Me.menuItems.Length - 1).color
	End Sub

	' Token: 0x060011F7 RID: 4599 RVA: 0x000A7A9F File Offset: 0x000A5E9F
	Public Overrides Overloads Sub Init(checkIfDead As Boolean, options As OptionsGUI, achievements As AchievementsGUI)
		Me.Init(checkIfDead, options, achievements, Nothing)
	End Sub

	' Token: 0x060011F8 RID: 4600 RVA: 0x000A7AAC File Offset: 0x000A5EAC
	Public Overrides Overloads Sub Init(checkIfDead As Boolean, options As OptionsGUI, achievements As AchievementsGUI, restartTowerConfirm As RestartTowerConfirmGUI)
		MyBase.Init(checkIfDead, options, achievements)
		Me.options = options
		Me.achievements = achievements
		Me.restartTowerConfirm = restartTowerConfirm
		If PlatformHelper.IsConsole AndAlso Me.menuItems.Length > 7 Then
			Me.menuItems(7).gameObject.SetActive(False)
		End If
		If Level.Current IsNot Nothing AndAlso Level.Current.CurrentLevel = Levels.Airplane Then
			Me.menuItems(2).gameObject.SetActive(True)
			Me.updateRotateControlsToggleVisualValue()
		ElseIf Not PlatformHelper.ShowAchievements AndAlso Me.menuItems.Length > 2 Then
			Me.menuItems(2).gameObject.SetActive(False)
		End If
		If Level.IsTowerOfPower Then
			Me.ReplaceRestartWRestartTowerOfPower()
		End If
		options.Init(checkIfDead)
		If achievements IsNot Nothing Then
			achievements.Init(checkIfDead)
		End If
		If restartTowerConfirm IsNot Nothing Then
			restartTowerConfirm.Init(checkIfDead)
		End If
	End Sub

	' Token: 0x060011F9 RID: 4601 RVA: 0x000A7BAF File Offset: 0x000A5FAF
	Public Sub ForceDisablePause(value As Boolean)
		Me.forceDisablePause = value
	End Sub

	' Token: 0x060011FA RID: 4602 RVA: 0x000A7BB8 File Offset: 0x000A5FB8
	Protected Overrides Sub OnPause()
		MyBase.OnPause()
		If CupheadLevelCamera.Current IsNot Nothing Then
			CupheadLevelCamera.Current.StartBlur()
		Else
			CupheadMapCamera.Current.StartBlur()
		End If
		PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerOne, PlatformHelper.CanSwitchUserFromPause)
		PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerTwo, PlatformHelper.CanSwitchUserFromPause)
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, False, False)
		Me.menuItems(4).gameObject.SetActive(PlayerManager.Multiplayer)
		If LevelPauseGUI.OnPauseEvent IsNot Nothing Then
			LevelPauseGUI.OnPauseEvent()
		End If
		Me.selection = 0
	End Sub

	' Token: 0x060011FB RID: 4603 RVA: 0x000A7C44 File Offset: 0x000A6044
	Protected Overrides Sub OnUnpause()
		MyBase.OnUnpause()
		If CupheadLevelCamera.Current IsNot Nothing Then
			CupheadLevelCamera.Current.EndBlur()
		Else
			CupheadMapCamera.Current.EndBlur()
		End If
		If Level.Current IsNot Nothing AndAlso Level.Current.CurrentLevel = Levels.Airplane Then
			SettingsData.Save()
			If PlatformHelper.IsConsole Then
				SettingsData.SaveToCloud()
			End If
		End If
		PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerOne, False)
		PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerTwo, False)
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, True, True)
		If LevelPauseGUI.OnUnpauseEvent IsNot Nothing Then
			LevelPauseGUI.OnUnpauseEvent()
		End If
	End Sub

	' Token: 0x060011FC RID: 4604 RVA: 0x000A7CE2 File Offset: 0x000A60E2
	Private Sub OnDestroy()
		PauseManager.Unpause()
	End Sub

	' Token: 0x060011FD RID: 4605 RVA: 0x000A7CEC File Offset: 0x000A60EC
	Protected Overrides Sub Update()
		MyBase.Update()
		If MyBase.state <> AbstractPauseGUI.State.Paused OrElse Me.options.optionMenuOpen OrElse Me.options.justClosed OrElse (Me.achievements IsNot Nothing AndAlso (Me.achievements.achievementsMenuOpen OrElse Me.achievements.justClosed)) OrElse (Me.restartTowerConfirm IsNot Nothing AndAlso (Me.restartTowerConfirm.restartTowerConfirmMenuOpen OrElse Me.restartTowerConfirm.justClosed)) Then
			Return
		End If
		If MyBase.GetButtonDown(CupheadButton.Pause) OrElse MyBase.GetButtonDown(CupheadButton.Cancel) Then
			Me.Unpause()
			Return
		End If
		If Level.Current IsNot Nothing AndAlso Level.Current.CurrentLevel = Levels.Airplane AndAlso Me.selection = 2 AndAlso (MyBase.GetButtonDown(CupheadButton.Accept) OrElse MyBase.GetButtonDown(CupheadButton.MenuLeft) OrElse MyBase.GetButtonDown(CupheadButton.MenuRight)) Then
			MyBase.MenuSelectSound()
			Me.ToggleRotateControls()
			Return
		End If
		If MyBase.GetButtonDown(CupheadButton.Accept) Then
			MyBase.MenuSelectSound()
			Me.[Select]()
			Return
		End If
		If Me._selectionTimer >= 0.15F Then
			If MyBase.GetButton(CupheadButton.MenuUp) Then
				MyBase.MenuMoveSound()
				Me.selection -= 1
			End If
			If MyBase.GetButton(CupheadButton.MenuDown) Then
				MyBase.MenuMoveSound()
				Me.selection += 1
			End If
		Else
			Me._selectionTimer += Time.deltaTime
		End If
	End Sub

	' Token: 0x060011FE RID: 4606 RVA: 0x000A7E98 File Offset: 0x000A6298
	Private Sub [Select]()
		Select Case Me.selection
			Case 0
				Me.Unpause()
			Case 1
				Me.Restart()
			Case 2
				Me.Achievements()
			Case 3
				Me.Options()
			Case 4
				Me.Player2Leave()
			Case 5
				Me.[Exit]()
			Case 6
				Me.ExitToTitle()
			Case 7
				Me.ExitToDesktop()
		End Select
	End Sub

	' Token: 0x060011FF RID: 4607 RVA: 0x000A7F2F File Offset: 0x000A632F
	Protected Overrides Sub OnUnpauseSound()
		MyBase.OnUnpauseSound()
	End Sub

	' Token: 0x06001200 RID: 4608 RVA: 0x000A7F38 File Offset: 0x000A6338
	Private Sub UpdateSelection()
		Me._selectionTimer = 0F
		For i As Integer = 0 To Me.menuItems.Length - 1
			Dim text As Text = Me.menuItems(i)
			If i = Me.selection Then
				text.color = LevelPauseGUI.COLOR_SELECTED
			Else
				text.color = LevelPauseGUI.COLOR_INACTIVE
			End If
		Next
	End Sub

	' Token: 0x06001201 RID: 4609 RVA: 0x000A7F9C File Offset: 0x000A639C
	Private Sub Restart()
		If Level.IsTowerOfPower Then
			Me.RestartTowerConfirm()
		Else
			Me.OnUnpauseSound()
			MyBase.state = AbstractPauseGUI.State.Animating
			PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerOne, False)
			PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerTwo, False)
			SceneLoader.ReloadLevel()
			Dialoguer.EndDialogue()
			If Level.IsDicePalaceMain OrElse Level.IsDicePalace Then
				DicePalaceMainLevelGameInfo.CleanUpRetry()
			End If
		End If
	End Sub

	' Token: 0x06001202 RID: 4610 RVA: 0x000A7FFC File Offset: 0x000A63FC
	Private Sub ReplaceRestartWRestartTowerOfPower()
		Me.retryLocHelper.currentID = Localization.Find("OptionMenuRestartTower").id
		Me.retryLocHelper.ApplyTranslation()
	End Sub

	' Token: 0x06001203 RID: 4611 RVA: 0x000A8023 File Offset: 0x000A6423
	Private Sub [Exit]()
		MyBase.state = AbstractPauseGUI.State.Animating
		PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerOne, False)
		PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerTwo, False)
		Dialoguer.EndDialogue()
		If Level.IsDicePalaceMain OrElse Level.IsDicePalace Then
			DicePalaceMainLevelGameInfo.CleanUpRetry()
		End If
		SceneLoader.LoadLastMap()
	End Sub

	' Token: 0x06001204 RID: 4612 RVA: 0x000A805D File Offset: 0x000A645D
	Private Sub Player2Leave()
		PlayerManager.PlayerLeave(PlayerId.PlayerTwo)
		Me.Unpause()
	End Sub

	' Token: 0x06001205 RID: 4613 RVA: 0x000A806B File Offset: 0x000A646B
	Private Sub ExitToTitle()
		MyBase.state = AbstractPauseGUI.State.Animating
		PlayerManager.ResetPlayers()
		Dialoguer.EndDialogue()
		SceneLoader.LoadScene(Scenes.scene_title, SceneLoader.Transition.Fade, SceneLoader.Transition.Fade, SceneLoader.Icon.Hourglass, Nothing)
	End Sub

	' Token: 0x06001206 RID: 4614 RVA: 0x000A8088 File Offset: 0x000A6488
	Private Sub ExitToDesktop()
		Dialoguer.EndDialogue()
		Application.Quit()
	End Sub

	' Token: 0x06001207 RID: 4615 RVA: 0x000A8094 File Offset: 0x000A6494
	Private Sub Options()
		MyBase.StartCoroutine(Me.in_options_cr())
	End Sub

	' Token: 0x06001208 RID: 4616 RVA: 0x000A80A4 File Offset: 0x000A64A4
	Private Iterator Function in_options_cr() As IEnumerator
		Me.HideImmediate()
		Me.options.ShowMainOptionMenu()
		PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerOne, False)
		PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerTwo, False)
		While Me.options.optionMenuOpen
			Yield Nothing
		End While
		PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerOne, PlatformHelper.CanSwitchUserFromPause)
		PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerTwo, PlatformHelper.CanSwitchUserFromPause)
		Me.selection = 0
		Me.ShowImmediate()
		Yield Nothing
		Return
	End Function

	' Token: 0x06001209 RID: 4617 RVA: 0x000A80BF File Offset: 0x000A64BF
	Private Sub RestartTowerConfirm()
		MyBase.StartCoroutine(Me.in_restarttowerconfirm_cr())
	End Sub

	' Token: 0x0600120A RID: 4618 RVA: 0x000A80D0 File Offset: 0x000A64D0
	Private Iterator Function in_restarttowerconfirm_cr() As IEnumerator
		Me.HideImmediate()
		Me.restartTowerConfirm.ShowMenu()
		PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerOne, False)
		PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerTwo, False)
		While Me.restartTowerConfirm.restartTowerConfirmMenuOpen
			Yield Nothing
		End While
		PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerOne, PlatformHelper.CanSwitchUserFromPause)
		PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerTwo, PlatformHelper.CanSwitchUserFromPause)
		Me.selection = 0
		Me.ShowImmediate()
		Yield Nothing
		Return
	End Function

	' Token: 0x0600120B RID: 4619 RVA: 0x000A80EB File Offset: 0x000A64EB
	Private Sub Achievements()
		MyBase.StartCoroutine(Me.in_achievements_cr())
	End Sub

	' Token: 0x0600120C RID: 4620 RVA: 0x000A80FC File Offset: 0x000A64FC
	Private Iterator Function in_achievements_cr() As IEnumerator
		Me.HideImmediate()
		Me.achievements.ShowAchievements()
		PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerOne, False)
		PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerTwo, False)
		While Me.achievements.achievementsMenuOpen
			Yield Nothing
		End While
		PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerOne, PlatformHelper.CanSwitchUserFromPause)
		PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerTwo, PlatformHelper.CanSwitchUserFromPause)
		Me.selection = 0
		Me.ShowImmediate()
		Yield Nothing
		Return
	End Function

	' Token: 0x0600120D RID: 4621 RVA: 0x000A8117 File Offset: 0x000A6517
	Private Sub ToggleRotateControls()
		SettingsData.Data.rotateControlsWithCamera = Not SettingsData.Data.rotateControlsWithCamera
		Me.updateRotateControlsToggleVisualValue()
	End Sub

	' Token: 0x0600120E RID: 4622 RVA: 0x000A8138 File Offset: 0x000A6538
	Private Sub updateRotateControlsToggleVisualValue()
		Dim text As Text = Me.menuItems(2)
		text.GetComponent(Of LocalizationHelper)().ApplyTranslation(Localization.Find("CameraRotationControl"), Nothing)
		text.text = String.Format(text.text, If((Not SettingsData.Data.rotateControlsWithCamera), "A", "B"))
	End Sub

	' Token: 0x0600120F RID: 4623 RVA: 0x000A8193 File Offset: 0x000A6593
	Private Sub onLanguageChangedEventHandler()
		If Level.Current IsNot Nothing AndAlso Level.Current.CurrentLevel = Levels.Airplane Then
			MyBase.StartCoroutine(Me.changeRotationToggleLanguage_cr())
		End If
	End Sub

	' Token: 0x06001210 RID: 4624 RVA: 0x000A81C8 File Offset: 0x000A65C8
	Private Iterator Function changeRotationToggleLanguage_cr() As IEnumerator
		Yield Nothing
		Yield Nothing
		Yield Nothing
		Me.updateRotateControlsToggleVisualValue()
		Return
	End Function

	' Token: 0x06001211 RID: 4625 RVA: 0x000A81E3 File Offset: 0x000A65E3
	Protected Overrides Sub InAnimation(i As Single)
	End Sub

	' Token: 0x06001212 RID: 4626 RVA: 0x000A81E5 File Offset: 0x000A65E5
	Protected Overrides Sub OutAnimation(i As Single)
	End Sub

	' Token: 0x04001B73 RID: 7027
	<SerializeField()>
	Private menuItems As Text()

	' Token: 0x04001B74 RID: 7028
	Private options As OptionsGUI

	' Token: 0x04001B75 RID: 7029
	Private achievements As AchievementsGUI

	' Token: 0x04001B76 RID: 7030
	Private restartTowerConfirm As RestartTowerConfirmGUI

	' Token: 0x04001B77 RID: 7031
	Private _selectionTimer As Single

	' Token: 0x04001B78 RID: 7032
	Private Const _SELECTION_TIME As Single = 0.15F

	' Token: 0x04001B79 RID: 7033
	<SerializeField()>
	Private retryLocHelper As LocalizationHelper

	' Token: 0x04001B7A RID: 7034
	Private _selection As Integer

	' Token: 0x04001B7B RID: 7035
	Private forceDisablePause As Boolean

	' Token: 0x02000484 RID: 1156
	Private Enum MenuItems
		' Token: 0x04001B7D RID: 7037
		Unpause
		' Token: 0x04001B7E RID: 7038
		Restart
		' Token: 0x04001B7F RID: 7039
		Achievements
		' Token: 0x04001B80 RID: 7040
		Options
		' Token: 0x04001B81 RID: 7041
		Player2Leave
		' Token: 0x04001B82 RID: 7042
		ExitToMap
		' Token: 0x04001B83 RID: 7043
		ExitToTitle
		' Token: 0x04001B84 RID: 7044
		ExitToDesktop
	End Enum
End Class
