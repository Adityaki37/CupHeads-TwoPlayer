Imports System
Imports System.Collections.Generic
Imports Rewired.UI.ControlMapper
Imports UnityEngine
Imports UnityEngine.UI

' Token: 0x02000460 RID: 1120
Public Class OptionsGUI
	Inherits AbstractMonoBehaviour

	' Token: 0x170002AB RID: 683
	' (get) Token: 0x060010FE RID: 4350 RVA: 0x000A286E File Offset: 0x000A0C6E
	' (set) Token: 0x060010FF RID: 4351 RVA: 0x000A2876 File Offset: 0x000A0C76
	Public Property state As OptionsGUI.State

	' Token: 0x170002AC RID: 684
	' (get) Token: 0x06001100 RID: 4352 RVA: 0x000A287F File Offset: 0x000A0C7F
	' (set) Token: 0x06001101 RID: 4353 RVA: 0x000A2886 File Offset: 0x000A0C86
	Public Shared Property COLOR_SELECTED As Color

	' Token: 0x170002AD RID: 685
	' (get) Token: 0x06001102 RID: 4354 RVA: 0x000A288E File Offset: 0x000A0C8E
	' (set) Token: 0x06001103 RID: 4355 RVA: 0x000A2895 File Offset: 0x000A0C95
	Public Shared Property COLOR_INACTIVE As Color

	' Token: 0x170002AE RID: 686
	' (get) Token: 0x06001104 RID: 4356 RVA: 0x000A289D File Offset: 0x000A0C9D
	' (set) Token: 0x06001105 RID: 4357 RVA: 0x000A28A5 File Offset: 0x000A0CA5
	Public Property optionMenuOpen As Boolean

	' Token: 0x170002AF RID: 687
	' (get) Token: 0x06001106 RID: 4358 RVA: 0x000A28AE File Offset: 0x000A0CAE
	' (set) Token: 0x06001107 RID: 4359 RVA: 0x000A28B6 File Offset: 0x000A0CB6
	Public Property inputEnabled As Boolean

	' Token: 0x170002B0 RID: 688
	' (get) Token: 0x06001108 RID: 4360 RVA: 0x000A28BF File Offset: 0x000A0CBF
	' (set) Token: 0x06001109 RID: 4361 RVA: 0x000A28C8 File Offset: 0x000A0CC8
	Private Property verticalSelection As Integer
		Get
			Return Me._verticalSelection
		End Get
		Set(value As Integer)
			Dim flag As Boolean = value > Me._verticalSelection
			Dim num As Integer = CInt(Mathf.Repeat(CSng(value), CSng(Me.currentItems.Count)))
			While Not Me.currentItems(num).text.gameObject.activeSelf
				num = If((Not flag), (num - 1), (num + 1))
				num = CInt(Mathf.Repeat(CSng(num), CSng(Me.currentItems.Count)))
			End While
			Me._verticalSelection = num
			Me.UpdateVerticalSelection()
		End Set
	End Property

	' Token: 0x170002B1 RID: 689
	' (get) Token: 0x0600110A RID: 4362 RVA: 0x000A294D File Offset: 0x000A0D4D
	' (set) Token: 0x0600110B RID: 4363 RVA: 0x000A2955 File Offset: 0x000A0D55
	Public Property justClosed As Boolean

	' Token: 0x0600110C RID: 4364 RVA: 0x000A2960 File Offset: 0x000A0D60
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.isConsole = PlatformHelper.IsConsole
		Me.showAlignOption = True
		Me.showTitleScreenOption = DLCManager.DLCEnabled()
		Me.optionMenuOpen = False
		Me.canvasGroup = MyBase.GetComponent(Of CanvasGroup)()
		Me.canvasGroup.alpha = 0F
		Me.currentItems = New List(Of OptionsGUI.Button)(Me.mainObjectButtons)
		Me.resolutions = New List(Of Resolution)()
		For Each resolution As Resolution In Screen.resolutions
			Dim resolution2 As Resolution = Nothing
			resolution2.width = resolution.width
			resolution2.height = resolution.height
			resolution2.refreshRate = 60
			If Not Me.resolutions.Contains(resolution2) Then
				Me.resolutions.Add(resolution2)
			End If
		Next
		Me.SetupButtons()
		OptionsGUI.COLOR_SELECTED = Me.currentItems(0).text.color
		OptionsGUI.COLOR_INACTIVE = Me.currentItems(Me.currentItems.Count - 1).text.color
		Me.initialAudioCenter = Me.audioObject.transform.localPosition.x
		Me.initialVisualCenter = Me.visualObject.transform.localPosition.x
		Me.initialAudioBackCenter = Me.audioObjectButtons(4).text.transform.localPosition.x
		Me.initialVisualBackCenter = Me.visualObjectButtons(7).text.transform.localPosition.x
	End Sub

	' Token: 0x0600110D RID: 4365 RVA: 0x000A2B12 File Offset: 0x000A0F12
	Private Sub Start()
		AddHandler Localization.OnLanguageChangedEvent, AddressOf Me.UpdateLanguages
	End Sub

	' Token: 0x0600110E RID: 4366 RVA: 0x000A2B25 File Offset: 0x000A0F25
	Private Sub OnDestroy()
		RemoveHandler Localization.OnLanguageChangedEvent, AddressOf Me.UpdateLanguages
	End Sub

	' Token: 0x0600110F RID: 4367 RVA: 0x000A2B38 File Offset: 0x000A0F38
	Private Sub UpdateLanguages()
		For i As Integer = 0 To Me.audioObjectButtons.Length - 1
			If Me.audioObjectButtons(i).localizationHelper IsNot Nothing Then
				Me.audioObjectButtons(i).localizationHelper.ApplyTranslation()
			End If
		Next
		For j As Integer = 0 To Me.visualObjectButtons.Length - 1
			If Me.visualObjectButtons(j).localizationHelper IsNot Nothing Then
				Me.visualObjectButtons(j).localizationHelper.ApplyTranslation()
			End If
		Next
	End Sub

	' Token: 0x06001110 RID: 4368 RVA: 0x000A2BCC File Offset: 0x000A0FCC
	Public Sub SetupButtons()
		Dim array As String() = New String(Me.resolutions.Count - 1) {}
		Dim num As Integer = 0
		For i As Integer = 0 To Me.resolutions.Count - 1
			array(i) = Me.resolutions(i).width + "x" + Me.resolutions(i).height
			If Screen.width = Me.resolutions(i).width AndAlso Screen.height = Me.resolutions(i).height Then
				num = i
			End If
		Next
		If Me.isConsole Then
			For Each gameObject As GameObject In Me.PcOnlyObjects
				gameObject.SetActive(False)
			Next
		End If
		If Not DLCManager.DLCEnabled() Then
			For Each gameObject2 As GameObject In Me.dlcHideObjects
				gameObject2.SetActive(False)
			Next
		End If
		Dim flag As Boolean = PlayerData.inGame AndAlso (PlayerData.Data.unlockedBlackAndWhite OrElse PlayerData.Data.unlocked2Strip OrElse PlayerData.Data.unlockedChaliceRecolor)
		For Each gameObject3 As GameObject In Me.FilterUnlockedOnlyObjects
			gameObject3.SetActive(flag)
		Next
		If Not Me.isConsole Then
			Me.visualObjectButtons(0).options = array
			Me.visualObjectButtons(1).options = New String() { "OptionMenuDisplayWindowed", "OptionMenuDisplayFullscreen" }
			Me.visualObjectButtons(2).options = New String() { "OptionMenuOn", "OptionMenuOff" }
		End If
		If Me.showAlignOption Then
			Me.visualObjectButtons(3).options = Me.slider
			Me.visualObjectButtons(3).wrap = False
		End If
		Me.visualObjectButtons(4).options = Me.slider
		Me.visualObjectButtons(4).wrap = False
		Me.visualObjectButtons(5).options = Me.slider
		Me.visualObjectButtons(5).wrap = False
		If Me.showTitleScreenOption Then
			Me.visualObjectButtons(6).options = New String() { "TitleScreenOptionsMenuOriginal", "TitleScreenOptionsMenuDLC" }
			Me.visualObjectButtons(6).wrap = True
		End If
		Dim list As List(Of String) = New List(Of String)()
		Me.unlockedFilters = New List(Of BlurGamma.Filter)()
		Me.unlockedFilters.Add(BlurGamma.Filter.None)
		list.Add("OptionMenuFilterNone")
		If PlayerData.Data.unlocked2Strip Then
			list.Add("OptionMenuFilter2Strip")
			Me.unlockedFilters.Add(BlurGamma.Filter.TwoStrip)
		End If
		If PlayerData.Data.unlockedBlackAndWhite Then
			list.Add("OptionMenuFilterBlackWhite")
			Me.unlockedFilters.Add(BlurGamma.Filter.BW)
		End If
		If PlayerData.Data.unlockedChaliceRecolor Then
			list.Add("ChaliceCostumeOptionsMenu")
			Me.unlockedFilters.Add(BlurGamma.Filter.Chalice)
		End If
		Me.visualObjectButtons(7).options = list.ToArray()
		If Not Me.isConsole Then
			Me.visualObjectButtons(0).updateSelection(num)
			Me.visualObjectButtons(1).updateSelection(If((Not Screen.fullScreen), 0, 1))
			Me.visualObjectButtons(2).updateSelection(If((QualitySettings.vSyncCount <= 0), 1, 0))
		End If
		If Me.showAlignOption Then
			Me.visualObjectButtons(3).updateSelection(Me.floatToSliderIndex(SettingsData.Data.overscan, 0F, 1F))
		End If
		Me.visualObjectButtons(4).updateSelection(Me.floatToSliderIndex(SettingsData.Data.Brightness, -1F, 1F))
		Me.visualObjectButtons(5).updateSelection(Me.floatToSliderIndex(SettingsData.Data.chromaticAberration, 0.5F, 1.5F))
		If Me.showTitleScreenOption Then
			Me.visualObjectButtons(6).updateSelection(If((Not SettingsData.Data.forceOriginalTitleScreen), 1, 0))
		End If
		Me.visualObjectButtons(7).updateSelection(Mathf.Min(CInt(SettingsData.Data.filter), list.Count - 1))
		Me.audioObjectButtons(0).options = Me.slider
		Me.audioObjectButtons(0).wrap = False
		Me.audioObjectButtons(1).options = Me.slider
		Me.audioObjectButtons(1).wrap = False
		Me.audioObjectButtons(2).options = Me.slider
		Me.audioObjectButtons(2).wrap = False
		Me.audioObjectButtons(3).options = New String() { "OptionMenuOff", "OptionMenuOn" }
		Me.audioObjectButtons(0).updateSelection(Me.floatToSliderIndex(SettingsData.Data.masterVolume, -48F, 0F))
		Me.audioObjectButtons(1).updateSelection(Me.floatToSliderIndex(SettingsData.Data.sFXVolume, -48F, 0F))
		Me.audioObjectButtons(2).updateSelection(Me.floatToSliderIndex(SettingsData.Data.musicVolume, -48F, 0F))
		Me.audioObjectButtons(3).updateSelection(If((Not SettingsData.Data.vintageAudioEnabled), 0, 1))
		Dim num2 As Integer = 0
		For m As Integer = 0 To Me.languageTranslations.Length - 1
			If Me.languageTranslations(m).language = Localization.language Then
				num2 = m
				Exit For
			End If
		Next
		Dim array3 As String() = New String(Me.languageTranslations.Length - 1) {}
		For n As Integer = 0 To Me.languageTranslations.Length - 1
			array3(n) = "Language" + Me.languageTranslations(n).translation
		Next
		Me.languageObjectButtons(0).options = array3
		Me.languageObjectButtons(0).updateSelection(num2)
	End Sub

	' Token: 0x06001111 RID: 4369 RVA: 0x000A321C File Offset: 0x000A161C
	Public Sub ChangeStateCustomLayoutScripts()
		Dim text As String = Me.visualObjectButtons(1).text.text
		Dim text2 As String = Localization.Find(Me.visualObjectButtons(1).options(0)).translation.SanitizedText()
		Dim flag As Boolean = text.Equals(text2)
		For i As Integer = 0 To Me.customPositionning.Length - 1
			Me.customPositionning(i).enabled = flag
		Next
	End Sub

	' Token: 0x06001112 RID: 4370 RVA: 0x000A3292 File Offset: 0x000A1692
	Public Sub Init(checkIfDead As Boolean)
		Me.input = New CupheadInput.AnyPlayerInput(checkIfDead)
	End Sub

	' Token: 0x06001113 RID: 4371 RVA: 0x000A32A0 File Offset: 0x000A16A0
	Private Sub Update()
		Me.justClosed = False
		If Not Me.inputEnabled Then
			Return
		End If
		If Me.state = OptionsGUI.State.Controls Then
			If Cuphead.Current.controlMapper.isOpen Then
				Return
			End If
			Me.state = OptionsGUI.State.MainOptions
			Me.canvasGroup.alpha = 1F
			Me.ToggleSubMenu(OptionsGUI.State.MainOptions)
			PlayerManager.ControlsChanged()
			Return
		Else
			If Me.GetButtonDown(CupheadButton.Pause) OrElse Me.GetButtonDown(CupheadButton.Cancel) Then
				If Me.state = OptionsGUI.State.MainOptions Then
					Me.MenuSelectSound()
					Me.HideMainOptionMenu()
				Else
					Me.MenuSelectSound()
					Me.ToMainOptions()
				End If
				Return
			End If
			If Me.GetButtonDown(CupheadButton.Accept) Then
				Select Case Me.state
					Case OptionsGUI.State.MainOptions
						Me.OptionSelect()
					Case OptionsGUI.State.Visual
						Me.VisualSelect()
					Case OptionsGUI.State.Audio
						Me.AudioSelect()
					Case OptionsGUI.State.Language
						Me.LanguageSelect()
				End Select
				Return
			End If
			If Me._selectionTimer >= 0.15F Then
				If Me.GetButton(CupheadButton.MenuUp) Then
					Me.MenuMoveSound()
					Me.verticalSelection -= 1
				End If
				If Me.GetButton(CupheadButton.MenuDown) Then
					Me.MenuMoveSound()
					Me.verticalSelection += 1
				End If
				If Me.GetButton(CupheadButton.MenuRight) AndAlso Me.currentItems(Me.verticalSelection).options.Length > 0 Then
					Me.currentItems(Me.verticalSelection).incrementSelection()
					Me.UpdateHorizontalSelection()
				End If
				If Me.GetButton(CupheadButton.MenuLeft) AndAlso Me.currentItems(Me.verticalSelection).options.Length > 0 Then
					Me.currentItems(Me.verticalSelection).decrementSelection()
					Me.UpdateHorizontalSelection()
				End If
			Else
				Me._selectionTimer += Time.deltaTime
			End If
			Return
		End If
	End Sub

	' Token: 0x06001114 RID: 4372 RVA: 0x000A34A0 File Offset: 0x000A18A0
	Private Sub UpdateVerticalSelection()
		Me._selectionTimer = 0F
		If Me.state = OptionsGUI.State.Controls Then
			Return
		End If
		If Me.state = OptionsGUI.State.Visual AndAlso Me.isConsole AndAlso Me.showAlignOption AndAlso Me._verticalSelection < 3 Then
			Me._verticalSelection = 3
		End If
		If Me.state = OptionsGUI.State.Visual AndAlso Me.isConsole AndAlso Not Me.showAlignOption AndAlso Me._verticalSelection < 4 Then
			Me._verticalSelection = 4
		End If
		For i As Integer = 0 To Me.currentItems.Count - 1
			Dim button As OptionsGUI.Button = Me.currentItems(i)
			If i = Me.verticalSelection Then
				button.text.color = OptionsGUI.COLOR_SELECTED
			Else
				button.text.color = OptionsGUI.COLOR_INACTIVE
			End If
		Next
	End Sub

	' Token: 0x06001115 RID: 4373 RVA: 0x000A358C File Offset: 0x000A198C
	Private Sub UpdateHorizontalSelection()
		Me._selectionTimer = 0F
		For i As Integer = 0 To Me.currentItems.Count - 1
			Dim button As OptionsGUI.Button = Me.currentItems(i)
			If i = Me.verticalSelection AndAlso Me.currentItems(i).options.Length > 0 Then
				Dim state As OptionsGUI.State = Me.state
				If state <> OptionsGUI.State.Audio Then
					If state <> OptionsGUI.State.Visual Then
						If state = OptionsGUI.State.Language Then
							Me.LanguageHorizontalSelect(Me.currentItems(i))
						End If
					Else
						Me.VisualHorizontalSelect(Me.currentItems(i))
					End If
				Else
					Me.AudioHorizontalSelect(Me.currentItems(i))
				End If
			End If
		Next
	End Sub

	' Token: 0x06001116 RID: 4374 RVA: 0x000A3658 File Offset: 0x000A1A58
	Public Sub ShowMainOptionMenu()
		Me.state = OptionsGUI.State.MainOptions
		Me.ToggleSubMenu(Me.state)
		Me.optionMenuOpen = True
		Me.verticalSelection = 0
		Me.canvasGroup.alpha = 1F
		MyBase.FrameDelayedCallback(AddressOf Me.Interactable, 1)
		Me.UpdateVerticalSelection()
	End Sub

	' Token: 0x06001117 RID: 4375 RVA: 0x000A36B0 File Offset: 0x000A1AB0
	Public Sub HideMainOptionMenu()
		SettingsData.Save()
		If PlatformHelper.IsConsole Then
			SettingsData.SaveToCloud()
		End If
		If Me.savePlayerData Then
			PlayerData.SaveCurrentFile()
		End If
		Me.savePlayerData = False
		Me.verticalSelection = 0
		Me.canvasGroup.alpha = 0F
		Me.canvasGroup.interactable = False
		Me.canvasGroup.blocksRaycasts = False
		Me.inputEnabled = False
		Me.optionMenuOpen = False
		Me.justClosed = True
	End Sub

	' Token: 0x06001118 RID: 4376 RVA: 0x000A372C File Offset: 0x000A1B2C
	Private Sub Interactable()
		Me.verticalSelection = 0
		Me.canvasGroup.interactable = True
		Me.canvasGroup.blocksRaycasts = True
		Me.inputEnabled = True
	End Sub

	' Token: 0x06001119 RID: 4377 RVA: 0x000A3754 File Offset: 0x000A1B54
	Private Sub OptionSelect()
		Me.MenuSelectSound()
		Select Case Me.verticalSelection
			Case 0
				Me.ToAudio()
			Case 1
				Me.ToVisual()
			Case 2
				Me.ToControls()
			Case 3
				Me.ToLanguage()
			Case 4
				Me.ToPauseMenu()
		End Select
	End Sub

	' Token: 0x0600111A RID: 4378 RVA: 0x000A37C4 File Offset: 0x000A1BC4
	Protected Sub MenuSelectSound()
		AudioManager.Play("level_menu_select")
	End Sub

	' Token: 0x0600111B RID: 4379 RVA: 0x000A37D0 File Offset: 0x000A1BD0
	Protected Sub MenuMoveSound()
		AudioManager.Play("level_menu_move")
	End Sub

	' Token: 0x0600111C RID: 4380 RVA: 0x000A37DC File Offset: 0x000A1BDC
	Private Sub ToVisual()
		Me.state = OptionsGUI.State.Visual
		Me.CenterVisual()
		If Not Me.isConsole Then
			Me.ChangeStateCustomLayoutScripts()
		End If
		Me.ToggleSubMenu(Me.state)
	End Sub

	' Token: 0x0600111D RID: 4381 RVA: 0x000A3808 File Offset: 0x000A1C08
	Private Sub ToAudio()
		Me.state = OptionsGUI.State.Audio
		Me.CenterAudio()
		Me.ToggleSubMenu(Me.state)
	End Sub

	' Token: 0x0600111E RID: 4382 RVA: 0x000A3823 File Offset: 0x000A1C23
	Private Sub ToLanguage()
		Me.state = OptionsGUI.State.Language
		Me.ToggleSubMenu(Me.state)
	End Sub

	' Token: 0x0600111F RID: 4383 RVA: 0x000A3838 File Offset: 0x000A1C38
	Private Sub ToControls()
		Me.state = OptionsGUI.State.Controls
		Me.ToggleSubMenu(Me.state)
	End Sub

	' Token: 0x06001120 RID: 4384 RVA: 0x000A384D File Offset: 0x000A1C4D
	Private Sub ToPauseMenu()
		Me.optionMenuOpen = False
		Me.HideMainOptionMenu()
	End Sub

	' Token: 0x06001121 RID: 4385 RVA: 0x000A385C File Offset: 0x000A1C5C
	Private Sub ToggleSubMenu(state As OptionsGUI.State)
		Me.currentItems.Clear()
		Select Case state
			Case OptionsGUI.State.MainOptions
				Me.mainObject.SetActive(True)
				Me.visualObject.SetActive(False)
				Me.audioObject.SetActive(False)
				Me.languageObject.SetActive(False)
				Me.bigCard.SetActive(False)
				Me.bigNoise.SetActive(False)
				Me.currentItems.AddRange(Me.mainObjectButtons)
			Case OptionsGUI.State.Visual
				Me.mainObject.SetActive(False)
				Me.visualObject.SetActive(True)
				Me.audioObject.SetActive(False)
				Me.bigCard.SetActive(True)
				Me.bigNoise.SetActive(True)
				Me.currentItems.AddRange(Me.visualObjectButtons)
			Case OptionsGUI.State.Audio
				Me.mainObject.SetActive(False)
				Me.visualObject.SetActive(False)
				Me.audioObject.SetActive(True)
				Me.languageObject.SetActive(False)
				Me.bigCard.SetActive(True)
				Me.bigNoise.SetActive(True)
				Me.currentItems.AddRange(Me.audioObjectButtons)
			Case OptionsGUI.State.Controls
				Me.mainObject.SetActive(False)
				Me.visualObject.SetActive(False)
				Me.audioObject.SetActive(False)
				Me.languageObject.SetActive(False)
				Me.ShowControlMapper()
			Case OptionsGUI.State.Language
				Me.languageObjectButtons(0).updateSelection(CInt(Localization.language))
				Me.mainObject.SetActive(False)
				Me.audioObject.SetActive(False)
				Me.languageObject.SetActive(True)
				Me.bigCard.SetActive(False)
				Me.bigNoise.SetActive(False)
				Me.currentItems.AddRange(Me.languageObjectButtons)
		End Select
		If state <> OptionsGUI.State.Controls Then
			Me.verticalSelection = 0
			Me.UpdateVerticalSelection()
		End If
	End Sub

	' Token: 0x06001122 RID: 4386 RVA: 0x000A3A54 File Offset: 0x000A1E54
	Private Sub ShowControlMapper()
		Dim controlMapper As ControlMapper = Cuphead.Current.controlMapper
		Dim componentInChildren As Canvas = controlMapper.GetComponentInChildren(Of Canvas)(True)
		Dim cupheadUICamera As CupheadUICamera = Global.UnityEngine.[Object].FindObjectOfType(Of CupheadUICamera)()
		If cupheadUICamera IsNot Nothing AndAlso componentInChildren IsNot Nothing Then
			componentInChildren.worldCamera = cupheadUICamera.GetComponent(Of Camera)()
		End If
		controlMapper.showPlayers = True
		If PlatformHelper.IsConsole Then
			controlMapper.showKeyboard = False
			controlMapper.showControllerGroupButtons = False
		End If
		controlMapper.showControllerGroupButtons = Not PlatformHelper.IsConsole
		controlMapper.Reset()
		controlMapper.Open()
		Me.canvasGroup.alpha = 0F
		Me.canvasGroup.interactable = False
		Me.canvasGroup.blocksRaycasts = False
	End Sub

	' Token: 0x06001123 RID: 4387 RVA: 0x000A3AFF File Offset: 0x000A1EFF
	Private Sub ToMainOptions()
		Me.state = OptionsGUI.State.MainOptions
		Me.ToggleSubMenu(Me.state)
	End Sub

	' Token: 0x06001124 RID: 4388 RVA: 0x000A3B14 File Offset: 0x000A1F14
	Private Sub VisualHorizontalSelect(button As OptionsGUI.Button)
		Select Case Me.verticalSelection
			Case 0
				Me.MenuSelectSound()
				If button.selection < Me.resolutions.Count Then
					SettingsData.Data.screenWidth = Me.resolutions(button.selection).width
					SettingsData.Data.screenHeight = Me.resolutions(button.selection).height
					Screen.SetResolution(SettingsData.Data.screenWidth, SettingsData.Data.screenHeight, Screen.fullScreen, 60)
				End If
			Case 1
				Me.MenuSelectSound()
				SettingsData.Data.fullScreen = button.selection = 1
				If Not Me.isConsole Then
					Me.ChangeStateCustomLayoutScripts()
				End If
				Screen.fullScreen = SettingsData.Data.fullScreen
			Case 2
				Me.MenuSelectSound()
				SettingsData.Data.vSyncCount = If((button.selection <> 0), 0, 1)
				QualitySettings.vSyncCount = SettingsData.Data.vSyncCount
			Case 3
				SettingsData.Data.overscan = Me.sliderIndexToFloat(button.selection, 0F, 1F)
			Case 4
				SettingsData.Data.Brightness = Me.sliderIndexToFloat(button.selection, -1F, 1F)
			Case 5
				SettingsData.Data.chromaticAberration = Me.sliderIndexToFloat(button.selection, 0.5F, 1.5F)
			Case 6
				SettingsData.Data.forceOriginalTitleScreen = Not SettingsData.Data.forceOriginalTitleScreen
			Case 7
				Me.MenuSelectSound()
				PlayerData.Data.filter = Me.unlockedFilters(button.selection)
				EventManager.Instance.Raise(New ChaliceRecolorEvent(Me.unlockedFilters(button.selection) = BlurGamma.Filter.Chalice))
				Me.savePlayerData = True
		End Select
	End Sub

	' Token: 0x06001125 RID: 4389 RVA: 0x000A3D2C File Offset: 0x000A212C
	Private Sub VisualSelect()
		AudioManager.Play("level_menu_select")
		Dim verticalSelection As Integer = Me.verticalSelection
		If verticalSelection = 8 Then
			Me.ToMainOptions()
		End If
	End Sub

	' Token: 0x06001126 RID: 4390 RVA: 0x000A3D64 File Offset: 0x000A2164
	Private Sub LanguageSelect()
		AudioManager.Play("level_menu_select")
		Dim verticalSelection As Integer = Me.verticalSelection
		If verticalSelection = 1 Then
			Me.ToMainOptions()
		End If
	End Sub

	' Token: 0x06001127 RID: 4391 RVA: 0x000A3D9C File Offset: 0x000A219C
	Private Sub LanguageHorizontalSelect(button As OptionsGUI.Button)
		Dim verticalSelection As Integer = Me.verticalSelection
		If verticalSelection = 0 Then
			Localization.language = Me.languageTranslations(button.selection).language
			button.updateSelection(button.selection)
			For i As Integer = 0 To Me.elementsToTranslate.Length - 1
				Me.elementsToTranslate(i).ApplyTranslation()
			Next
		End If
	End Sub

	' Token: 0x06001128 RID: 4392 RVA: 0x000A3E10 File Offset: 0x000A2210
	Private Sub AudioHorizontalSelect(button As OptionsGUI.Button)
		Select Case Me.verticalSelection
			Case 0
				AudioManager.masterVolume = If((button.selection > 0), Me.sliderIndexToFloat(button.selection, -48F, 0F), (-80F))
				SettingsData.Data.masterVolume = AudioManager.masterVolume
			Case 1
				AudioManager.sfxOptionsVolume = If((button.selection > 0), Me.sliderIndexToFloat(button.selection, -48F, 0F), (-80F))
				SettingsData.Data.sFXVolume = AudioManager.sfxOptionsVolume
			Case 2
				AudioManager.bgmOptionsVolume = If((button.selection > 0), Me.sliderIndexToFloat(button.selection, -48F, 0F), (-80F))
				SettingsData.Data.musicVolume = AudioManager.bgmOptionsVolume
			Case 3
				Me.MenuSelectSound()
				If button.selection = 0 Then
					PlayerData.Data.vintageAudioEnabled = False
				ElseIf button.options(button.selection) = button.options(1) Then
					PlayerData.Data.vintageAudioEnabled = True
				End If
				Me.savePlayerData = True
		End Select
	End Sub

	' Token: 0x06001129 RID: 4393 RVA: 0x000A3F65 File Offset: 0x000A2365
	Private Sub MasterVolume([option] As String)
	End Sub

	' Token: 0x0600112A RID: 4394 RVA: 0x000A3F68 File Offset: 0x000A2368
	Private Sub AudioSelect()
		AudioManager.Play("level_menu_select")
		Dim verticalSelection As Integer = Me.verticalSelection
		If verticalSelection = 4 Then
			Me.ToMainOptions()
		End If
	End Sub

	' Token: 0x0600112B RID: 4395 RVA: 0x000A3F9D File Offset: 0x000A239D
	Protected Function GetButtonDown(button As CupheadButton) As Boolean
		Return Me.input.GetButtonDown(button)
	End Function

	' Token: 0x0600112C RID: 4396 RVA: 0x000A3FB3 File Offset: 0x000A23B3
	Protected Function GetButton(button As CupheadButton) As Boolean
		Return Me.input.GetButton(button)
	End Function

	' Token: 0x0600112D RID: 4397 RVA: 0x000A3FC9 File Offset: 0x000A23C9
	Private Function sliderIndexToFloat(index As Integer, min As Single, max As Single) As Single
		If index <> Me.lastIndex Then
			Me.MenuSelectSound()
		End If
		Me.lastIndex = index
		Return CSng(index) / CSng((Me.slider.Length - 1)) * (max - min) + min
	End Function

	' Token: 0x0600112E RID: 4398 RVA: 0x000A3FF8 File Offset: 0x000A23F8
	Private Function floatToSliderIndex(value As Single, min As Single, max As Single) As Integer
		Dim num As Integer = Mathf.RoundToInt((value - min) / (max - min) * CSng((Me.slider.Length - 1)))
		If num > Me.slider.Length - 1 Then
			num = Me.slider.Length - 1
		End If
		If num < 0 Then
			num = 0
		End If
		Return num
	End Function

	' Token: 0x0600112F RID: 4399 RVA: 0x000A4044 File Offset: 0x000A2444
	Private Sub CenterAudio()
		Dim num As Single = Me.audioCenterPositions(CInt(Localization.language))
		Me.audioObject.transform.SetLocalPosition(New Single?(Me.initialAudioCenter + num), Nothing, Nothing)
		Me.audioObjectButtons(4).text.transform.SetLocalPosition(New Single?(Me.initialAudioBackCenter - num), Nothing, Nothing)
	End Sub

	' Token: 0x06001130 RID: 4400 RVA: 0x000A40C4 File Offset: 0x000A24C4
	Private Sub CenterVisual()
		Dim num As Single = Me.visualCenterPositions(CInt(Localization.language))
		Me.visualObject.transform.SetLocalPosition(New Single?(Me.initialVisualCenter + num), Nothing, Nothing)
		Me.visualObjectButtons(7).text.transform.SetLocalPosition(New Single?(Me.initialVisualBackCenter - num), Nothing, Nothing)
	End Sub

	' Token: 0x04001A66 RID: 6758
	Private Const BRIGHTNESS_MAX As Single = 1F

	' Token: 0x04001A67 RID: 6759
	Private Const VOLUME_MIN As Single = -48F

	' Token: 0x04001A68 RID: 6760
	Private Const CHROMATIC_ABERRATION_MIN As Single = 0.5F

	' Token: 0x04001A69 RID: 6761
	Private Const CHROMATIC_ABERRATION_MAX As Single = 1.5F

	' Token: 0x04001A6A RID: 6762
	Private Const VOLUME_NONE As Single = -80F

	' Token: 0x04001A70 RID: 6768
	<SerializeField()>
	Private mainObject As GameObject

	' Token: 0x04001A71 RID: 6769
	<SerializeField()>
	Private visualObject As GameObject

	' Token: 0x04001A72 RID: 6770
	<SerializeField()>
	Private audioObject As GameObject

	' Token: 0x04001A73 RID: 6771
	<SerializeField()>
	Private languageObject As GameObject

	' Token: 0x04001A74 RID: 6772
	<SerializeField()>
	Private mainObjectButtons As OptionsGUI.Button()

	' Token: 0x04001A75 RID: 6773
	<SerializeField()>
	Private PcOnlyObjects As GameObject()

	' Token: 0x04001A76 RID: 6774
	<SerializeField()>
	Private playStation4HideObjects As GameObject()

	' Token: 0x04001A77 RID: 6775
	<SerializeField()>
	Private dlcHideObjects As GameObject()

	' Token: 0x04001A78 RID: 6776
	<SerializeField()>
	Private FilterUnlockedOnlyObjects As GameObject()

	' Token: 0x04001A79 RID: 6777
	<SerializeField()>
	Private bigCard As GameObject

	' Token: 0x04001A7A RID: 6778
	<SerializeField()>
	Private bigNoise As GameObject

	' Token: 0x04001A7B RID: 6779
	<SerializeField()>
	Private visualObjectButtons As OptionsGUI.Button()

	' Token: 0x04001A7C RID: 6780
	<SerializeField()>
	Private audioObjectButtons As OptionsGUI.Button()

	' Token: 0x04001A7D RID: 6781
	<SerializeField()>
	Private languageObjectButtons As OptionsGUI.Button()

	' Token: 0x04001A7E RID: 6782
	<SerializeField()>
	Private languageTranslations As OptionsGUI.LanguageTranslation()

	' Token: 0x04001A7F RID: 6783
	<SerializeField()>
	Private elementsToTranslate As LocalizationHelper()

	' Token: 0x04001A80 RID: 6784
	<SerializeField()>
	Private audioCenterPositions As Single()

	' Token: 0x04001A81 RID: 6785
	<SerializeField()>
	Private visualCenterPositions As Single()

	' Token: 0x04001A82 RID: 6786
	<SerializeField()>
	Private customPositionning As CustomLanguageLayout()

	' Token: 0x04001A83 RID: 6787
	Private currentItems As List(Of OptionsGUI.Button)

	' Token: 0x04001A84 RID: 6788
	Private unlockedFilters As List(Of BlurGamma.Filter)

	' Token: 0x04001A85 RID: 6789
	Private isConsole As Boolean

	' Token: 0x04001A86 RID: 6790
	Private showAlignOption As Boolean

	' Token: 0x04001A87 RID: 6791
	Private showTitleScreenOption As Boolean

	' Token: 0x04001A88 RID: 6792
	Private slider As String() = New String() { "|----------", "-|---------", "--|--------", "---|-------", "----|------", "-----|-----", "------|----", "-------|---", "--------|--", "---------|-", "----------|" }

	' Token: 0x04001A89 RID: 6793
	Private canvasGroup As CanvasGroup

	' Token: 0x04001A8A RID: 6794
	Private pauseMenu As AbstractPauseGUI

	' Token: 0x04001A8B RID: 6795
	Private _selectionTimer As Single

	' Token: 0x04001A8C RID: 6796
	Private Const _SELECTION_TIME As Single = 0.15F

	' Token: 0x04001A8D RID: 6797
	Private _verticalSelection As Integer

	' Token: 0x04001A8E RID: 6798
	Private input As CupheadInput.AnyPlayerInput

	' Token: 0x04001A8F RID: 6799
	Private lastIndex As Integer

	' Token: 0x04001A90 RID: 6800
	Private resolutions As List(Of Resolution)

	' Token: 0x04001A92 RID: 6802
	Private initialAudioCenter As Single

	' Token: 0x04001A93 RID: 6803
	Private initialVisualCenter As Single

	' Token: 0x04001A94 RID: 6804
	Private initialAudioBackCenter As Single

	' Token: 0x04001A95 RID: 6805
	Private initialVisualBackCenter As Single

	' Token: 0x04001A96 RID: 6806
	Private savePlayerData As Boolean

	' Token: 0x02000461 RID: 1121
	<Serializable()>
	Public Structure LanguageTranslation
		' Token: 0x04001A97 RID: 6807
		<SerializeField()>
		Public language As Localization.Languages

		' Token: 0x04001A98 RID: 6808
		<SerializeField()>
		Public translation As String
	End Structure

	' Token: 0x02000462 RID: 1122
	Public Enum State
		' Token: 0x04001A9A RID: 6810
		MainOptions
		' Token: 0x04001A9B RID: 6811
		Visual
		' Token: 0x04001A9C RID: 6812
		Audio
		' Token: 0x04001A9D RID: 6813
		Controls
		' Token: 0x04001A9E RID: 6814
		Language
	End Enum

	' Token: 0x02000463 RID: 1123
	Private Enum VisualOptions
		' Token: 0x04001AA0 RID: 6816
		Resolution
		' Token: 0x04001AA1 RID: 6817
		Display
		' Token: 0x04001AA2 RID: 6818
		VSync
		' Token: 0x04001AA3 RID: 6819
		Align
		' Token: 0x04001AA4 RID: 6820
		Brightness
		' Token: 0x04001AA5 RID: 6821
		ChromaticAberration
		' Token: 0x04001AA6 RID: 6822
		TitleScreen
		' Token: 0x04001AA7 RID: 6823
		Filter
	End Enum

	' Token: 0x02000464 RID: 1124
	Private Enum AudioOptions
		' Token: 0x04001AA9 RID: 6825
		MasterVol
		' Token: 0x04001AAA RID: 6826
		SFXVol
		' Token: 0x04001AAB RID: 6827
		MusicVol
		' Token: 0x04001AAC RID: 6828
		Vintage
	End Enum

	' Token: 0x02000465 RID: 1125
	Private Enum LanguageOptions
		' Token: 0x04001AAE RID: 6830
		Language
	End Enum

	' Token: 0x02000466 RID: 1126
	<Serializable()>
	Public Class Button
		' Token: 0x06001132 RID: 4402 RVA: 0x000A4154 File Offset: 0x000A2554
		Public Sub updateSelection(index As Integer)
			Me.selection = index
			If Me.localizationHelper Is Nothing Then
				Me.text.text = Me.options(index)
			Else
				Me.localizationHelper.ApplyTranslation(Localization.Find(Me.options(index)), Nothing)
			End If
		End Sub

		' Token: 0x06001133 RID: 4403 RVA: 0x000A41AA File Offset: 0x000A25AA
		Public Sub incrementSelection()
			If Me.wrap OrElse Me.selection < Me.options.Length - 1 Then
				Me.updateSelection((Me.selection + 1) Mod Me.options.Length)
			End If
		End Sub

		' Token: 0x06001134 RID: 4404 RVA: 0x000A41E4 File Offset: 0x000A25E4
		Public Sub decrementSelection()
			If Me.wrap OrElse Me.selection > 0 Then
				Me.updateSelection(If((Me.selection <> 0), (Me.selection - 1), (Me.options.Length - 1)))
			End If
		End Sub

		' Token: 0x04001AAF RID: 6831
		Public text As Text

		' Token: 0x04001AB0 RID: 6832
		Public localizationHelper As LocalizationHelper

		' Token: 0x04001AB1 RID: 6833
		Public options As String()

		' Token: 0x04001AB2 RID: 6834
		Public selection As Integer

		' Token: 0x04001AB3 RID: 6835
		Public wrap As Boolean = True
	End Class
End Class
