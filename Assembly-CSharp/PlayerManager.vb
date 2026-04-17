Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports Rewired
Imports UnityEngine
Imports UnityEngine.SceneManagement

' Token: 0x02000ACB RID: 2763
Public Module PlayerManager
	' Token: 0x140000AA RID: 170
	' (add) Token: 0x0600424E RID: 16974 RVA: 0x0023C568 File Offset: 0x0023A968
	' (remove) Token: 0x0600424F RID: 16975 RVA: 0x0023C59C File Offset: 0x0023A99C
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnPlayerJoinedEvent As PlayerManager.PlayerChangedDelegate

	' Token: 0x140000AB RID: 171
	' (add) Token: 0x06004250 RID: 16976 RVA: 0x0023C5D0 File Offset: 0x0023A9D0
	' (remove) Token: 0x06004251 RID: 16977 RVA: 0x0023C604 File Offset: 0x0023AA04
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnPlayerLeaveEvent As PlayerManager.PlayerChangedDelegate

	' Token: 0x140000AC RID: 172
	' (add) Token: 0x06004252 RID: 16978 RVA: 0x0023C638 File Offset: 0x0023AA38
	' (remove) Token: 0x06004253 RID: 16979 RVA: 0x0023C66C File Offset: 0x0023AA6C
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnControlsChanged As Action

	' Token: 0x170005D2 RID: 1490
	' (get) Token: 0x06004254 RID: 16980 RVA: 0x0023C6A0 File Offset: 0x0023AAA0
	Public ReadOnly Property ShouldShowJoinPrompt As Boolean
		Get
			Return PlayerManager.playerSlots(1).joinState = PlayerManager.PlayerSlot.JoinState.JoinPromptDisplayed
		End Get
	End Property

	' Token: 0x06004255 RID: 16981 RVA: 0x0023C6B4 File Offset: 0x0023AAB4
	Public Sub Awake()
		PlayerManager.Multiplayer = False
		PlayerManager.players = New Dictionary(Of Integer, AbstractPlayerController)()
		PlayerManager.players.Add(0, Nothing)
		PlayerManager.players.Add(1, Nothing)
		PlayerManager.playerInputs = New Dictionary(Of Integer, Player)()
		PlayerManager.playerInputs.Add(0, ReInput.players.GetPlayer(0))
		PlayerManager.playerInputs.Add(1, ReInput.players.GetPlayer(1))
	End Sub

	' Token: 0x06004256 RID: 16982 RVA: 0x0023C720 File Offset: 0x0023AB20
	Public Sub Init()
		AddHandler OnlineManager.Instance.[Interface].OnUserSignedIn, AddressOf PlayerManager.OnUserSignedIn
		AddHandler OnlineManager.Instance.[Interface].OnUserSignedOut, AddressOf PlayerManager.OnUserSignedOut
		AddHandler ReInput.ControllerConnectedEvent, AddressOf PlayerManager.OnControllerConnected
		AddHandler ReInput.ControllerDisconnectedEvent, AddressOf PlayerManager.OnControllerDisconnected
		AddHandler PlmManager.Instance.[Interface].OnUnconstrained, AddressOf PlayerManager.OnUnconstrained
		AddHandler PlmManager.Instance.[Interface].OnResume, AddressOf PlayerManager.OnResume
		AddHandler PlmManager.Instance.[Interface].OnSuspend, AddressOf PlayerManager.OnSuspend
	End Sub

	' Token: 0x06004257 RID: 16983 RVA: 0x0023C850 File Offset: 0x0023AC50
	Public Sub SetPlayerCanJoin(player As PlayerId, canJoin As Boolean, promptBeforeJoin As Boolean)
		Dim playerSlot As PlayerManager.PlayerSlot = If((player <> PlayerId.PlayerOne), PlayerManager.playerSlots(1), PlayerManager.playerSlots(0))
		playerSlot.canJoin = canJoin
		playerSlot.promptBeforeJoin = promptBeforeJoin
		If Not canJoin AndAlso playerSlot.joinState = PlayerManager.PlayerSlot.JoinState.JoinPromptDisplayed Then
			playerSlot.joinState = PlayerManager.PlayerSlot.JoinState.NotJoining
		End If
	End Sub

	' Token: 0x06004258 RID: 16984 RVA: 0x0023C8A0 File Offset: 0x0023ACA0
	Public Sub ClearJoinPrompt()
		For i As Integer = 0 To 2 - 1
			If PlayerManager.playerSlots(i).joinState = PlayerManager.PlayerSlot.JoinState.JoinPromptDisplayed Then
				PlayerManager.playerSlots(i).joinState = PlayerManager.PlayerSlot.JoinState.NotJoining
			End If
		Next
	End Sub

	' Token: 0x06004259 RID: 16985 RVA: 0x0023C8E0 File Offset: 0x0023ACE0
	Public Sub SetPlayerCanSwitch(player As PlayerId, canSwitch As Boolean)
		Dim playerSlot As PlayerManager.PlayerSlot = If((player <> PlayerId.PlayerOne), PlayerManager.playerSlots(1), PlayerManager.playerSlots(0))
		playerSlot.canSwitch = canSwitch
		playerSlot.requestedSwitch = False
	End Sub

	' Token: 0x0600425A RID: 16986 RVA: 0x0023C918 File Offset: 0x0023AD18
	Public Sub PlayerLeave(player As PlayerId)
		Dim playerSlot As PlayerManager.PlayerSlot = If((player <> PlayerId.PlayerOne), PlayerManager.playerSlots(1), PlayerManager.playerSlots(0))
		playerSlot.joinState = PlayerManager.PlayerSlot.JoinState.Leaving
	End Sub

	' Token: 0x0600425B RID: 16987 RVA: 0x0023C946 File Offset: 0x0023AD46
	Public Sub OnChaliceCharmUnequipped(player As PlayerId)
		PlayerManager.playerWasChalice(CInt(player)) = False
	End Sub

	' Token: 0x0600425C RID: 16988 RVA: 0x0023C950 File Offset: 0x0023AD50
	Private Sub OnControllerConnected(args As ControllerStatusChangedEventArgs)
	End Sub

	' Token: 0x0600425D RID: 16989 RVA: 0x0023C954 File Offset: 0x0023AD54
	Public Sub Update()
		If InterruptingPrompt.IsInterrupting() Then
			For i As Integer = 0 To PlayerManager.playerSlots.Length - 1
				If PlayerManager.playerSlots(i).joinState = PlayerManager.PlayerSlot.JoinState.Joined AndAlso PlayerManager.playerSlots(i).controllerState = PlayerManager.PlayerSlot.ControllerState.ReconnectPromptDisplayed Then
					Dim playerId As PlayerId = If((i <> 0), PlayerId.PlayerTwo, PlayerId.PlayerOne)
					Dim joystick As Joystick = CupheadInput.CheckForUnconnectedControllerPress()
					Dim playerInput As Player = PlayerManager.GetPlayerInput(playerId)
					If joystick IsNot Nothing Then
						PlayerManager.playerSlots(i).controllerState = PlayerManager.PlayerSlot.ControllerState.UsingController
						PlayerManager.playerSlots(i).controllerId = joystick.id
						PlayerManager.playerSlots(i).controllerDisconnectFromPlm = False
						PlayerManager.playerSlots(i).lastController = ControllerType.Joystick
						playerInput.controllers.AddController(joystick, True)
						ReInput.userDataStore.LoadControllerData(PlayerManager.playerInputs(CInt(playerId)).id, ControllerType.Joystick, PlayerManager.playerSlots(i).controllerId)
						PlayerManager.ControlsChanged()
					End If
					If Not PlatformHelper.IsConsole AndAlso playerInput.GetAnyButtonDown() Then
						PlayerManager.playerSlots(i).controllerState = PlayerManager.PlayerSlot.ControllerState.NoController
						PlayerManager.playerSlots(i).controllerDisconnectFromPlm = False
						PlayerManager.ControlsChanged()
						PlayerManager.playerSlots(i).lastController = ControllerType.Keyboard
					End If
				End If
			Next
			Return
		End If
		For j As Integer = 0 To PlayerManager.playerSlots.Length - 1
			If PlayerManager.playerSlots(j).canJoin AndAlso PlayerManager.playerSlots(j).joinState <> PlayerManager.PlayerSlot.JoinState.Joined Then
				Dim playerId2 As PlayerId = If((j <> 0), PlayerId.PlayerTwo, PlayerId.PlayerOne)
				Dim flag As Boolean = False
				Dim joystick2 As Joystick = CupheadInput.CheckForUnconnectedControllerPress()
				Dim playerInput2 As Player = PlayerManager.GetPlayerInput(playerId2)
				If joystick2 IsNot Nothing Then
					flag = True
					PlayerManager.playerSlots(j).controllerState = PlayerManager.PlayerSlot.ControllerState.UsingController
					PlayerManager.playerSlots(j).controllerId = joystick2.id
				ElseIf Not PlatformHelper.IsConsole AndAlso If((Not(SceneManager.GetActiveScene().name = "scene_title")), playerInput2.GetAnyButtonDown(), (playerInput2.controllers.Keyboard.GetAnyButtonDown() AndAlso PlayerManager.playerSlots(j).joinState = PlayerManager.PlayerSlot.JoinState.NotJoining)) Then
					flag = True
					PlayerManager.playerSlots(j).controllerState = PlayerManager.PlayerSlot.ControllerState.NoController
				End If
				If flag Then
					If PlayerManager.playerSlots(j).joinState = PlayerManager.PlayerSlot.JoinState.NotJoining AndAlso PlayerManager.playerSlots(j).promptBeforeJoin Then
						PlayerManager.playerSlots(j).joinState = PlayerManager.PlayerSlot.JoinState.JoinPromptDisplayed
					Else
						Dim flag2 As Boolean = False
						PlayerManager.playerSlots(j).joinState = PlayerManager.PlayerSlot.JoinState.JoinRequested
						If OnlineManager.Instance.[Interface].SupportsMultipleUsers Then
							Dim value As ULong = CULng(joystick2.systemId.Value)
							Dim userForController As OnlineUser = OnlineManager.Instance.[Interface].GetUserForController(value)
							If userForController IsNot Nothing AndAlso ((j = 0 AndAlso Not userForController.Equals(OnlineManager.Instance.[Interface].SecondaryUser)) OrElse (j = 1 AndAlso Not userForController.Equals(OnlineManager.Instance.[Interface].MainUser))) Then
								OnlineManager.Instance.[Interface].SetUser(playerId2, userForController)
								flag2 = True
							Else
								OnlineManager.Instance.[Interface].SignInUser(False, playerId2, value)
							End If
						ElseIf OnlineManager.Instance.[Interface].SupportsUserSignIn AndAlso playerId2 = PlayerId.PlayerOne Then
							OnlineManager.Instance.[Interface].SignInUser(False, playerId2, 0UL)
						Else
							flag2 = True
						End If
						If flag2 Then
							If joystick2 IsNot Nothing Then
								playerInput2.controllers.AddController(joystick2, True)
								ReInput.userDataStore.LoadControllerData(PlayerManager.playerInputs(CInt(playerId2)).id, ControllerType.Joystick, PlayerManager.playerSlots(j).controllerId)
							End If
							PlayerManager.playerSlots(j).joinState = PlayerManager.PlayerSlot.JoinState.Joined
							If playerId2 = PlayerId.PlayerTwo Then
								PlayerManager.Multiplayer = True
							End If
							PlayerManager.OnPlayerJoinedEvent(playerId2)
							AudioManager.Play("player_spawn")
						End If
					End If
				End If
			End If
		Next
		For k As Integer = 0 To PlayerManager.playerSlots.Length - 1
			If OnlineManager.Instance.[Interface].SupportsUserSignIn AndAlso PlayerManager.playerSlots(k).canSwitch AndAlso PlayerManager.playerSlots(k).joinState = PlayerManager.PlayerSlot.JoinState.Joined Then
				If OnlineManager.Instance.[Interface].SupportsMultipleUsers OrElse k <> 1 Then
					Dim playerId3 As PlayerId = If((k <> 0), PlayerId.PlayerTwo, PlayerId.PlayerOne)
					Dim playerInput3 As Player = PlayerManager.GetPlayerInput(playerId3)
					If playerInput3.GetButtonDown(11) Then
						PlayerManager.playerSlots(k).requestedSwitch = True
						PlayerManager.playerSlots((k + 1) Mod 2).requestedSwitch = False
						Dim num As ULong = 0UL
						If playerInput3.controllers.joystickCount > 0 Then
							num = CULng(playerInput3.controllers.Joysticks(0).systemId.Value)
						End If
						OnlineManager.Instance.[Interface].SwitchUser(playerId3, num)
					End If
				End If
			End If
		Next
		For l As Integer = 0 To PlayerManager.playerSlots.Length - 1
			If SceneLoader.CurrentlyLoading Then
				Exit For
			End If
			If PlayerManager.playerSlots(l).joinState = PlayerManager.PlayerSlot.JoinState.Leaving Then
				Dim playerId4 As PlayerId = If((l <> 0), PlayerId.PlayerTwo, PlayerId.PlayerOne)
				Dim playerInput4 As Player = PlayerManager.GetPlayerInput(playerId4)
				playerInput4.controllers.ClearControllersOfType(Of Joystick)()
				PlayerManager.playerSlots(l).joinState = PlayerManager.PlayerSlot.JoinState.NotJoining
				If playerId4 = PlayerId.PlayerTwo Then
					PlayerManager.Multiplayer = False
				End If
				OnlineManager.Instance.[Interface].SetRichPresenceActive(playerId4, False)
				OnlineManager.Instance.[Interface].SetUser(playerId4, Nothing)
				If playerId4 = PlayerId.PlayerOne Then
					PlayerManager.shouldGoToStartScreen = True
				ElseIf PlayerManager.OnPlayerLeaveEvent IsNot Nothing Then
					PlayerManager.OnPlayerLeaveEvent(playerId4)
					AudioManager.Play("player_despawn")
				End If
			End If
		Next
		For m As Integer = 0 To PlayerManager.playerSlots.Length - 1
			If PlayerManager.playerSlots(m).shouldAssignController Then
				Dim playerId5 As PlayerId = If((m <> 0), PlayerId.PlayerTwo, PlayerId.PlayerOne)
				Dim playerInput5 As Player = PlayerManager.GetPlayerInput(playerId5)
				playerInput5.controllers.AddController(Of Joystick)(PlayerManager.playerSlots(m).controllerId, True)
				ReInput.userDataStore.LoadControllerData(PlayerManager.playerInputs(CInt(playerId5)).id, ControllerType.Joystick, PlayerManager.playerSlots(m).controllerId)
				PlayerManager.playerSlots(m).shouldAssignController = False
			End If
		Next
		If ControllerDisconnectedPrompt.Instance IsNot Nothing AndAlso Not ControllerDisconnectedPrompt.Instance.Visible AndAlso ControllerDisconnectedPrompt.Instance.allowedToShow Then
			For n As Integer = 0 To 2 - 1
				Dim playerId6 As PlayerId = If((n <> 0), PlayerId.PlayerTwo, PlayerId.PlayerOne)
				If PlayerManager.IsControllerDisconnected(playerId6, False) Then
					ControllerDisconnectedPrompt.Instance.Show(playerId6)
					Exit For
				End If
			Next
		End If
		If PlmManager.Instance.[Interface].IsConstrained() Then
			If InterruptingPrompt.CanInterrupt() AndAlso PauseManager.state <> PauseManager.State.Paused Then
				PauseManager.Pause()
				PlayerManager.pausedDueToPlm = True
			End If
		ElseIf PlayerManager.pausedDueToPlm Then
			PauseManager.Unpause()
			PlayerManager.pausedDueToPlm = False
		End If
		If PlayerManager.shouldGoToSlotSelect Then
			PlayerManager.goToSlotSelect()
			PlayerManager.shouldGoToSlotSelect = False
		End If
		If PlayerManager.shouldGoToStartScreen Then
			PlayerManager.goToStartScreen()
			PlayerManager.shouldGoToStartScreen = False
		End If
		For num2 As Integer = 0 To 2 - 1
			Dim playerId7 As PlayerId = If((num2 <> 0), PlayerId.PlayerTwo, PlayerId.PlayerOne)
			Dim lastActiveController As Controller = PlayerManager.GetPlayerInput(playerId7).controllers.GetLastActiveController()
			If lastActiveController IsNot Nothing AndAlso lastActiveController.type <> PlayerManager.playerSlots(num2).lastController Then
				PlayerManager.playerSlots(num2).lastController = lastActiveController.type
				PlayerManager.ControlsChanged()
			End If
		Next
	End Sub

	' Token: 0x0600425E RID: 16990 RVA: 0x0023D120 File Offset: 0x0023B520
	Public Sub ControllerRemapped(playerId As PlayerId, usingController As Boolean, controllerId As Integer)
		Dim num As Integer = If((playerId <> PlayerId.PlayerOne), 1, 0)
		PlayerManager.playerSlots(num).controllerState = If((Not usingController), PlayerManager.PlayerSlot.ControllerState.NoController, PlayerManager.PlayerSlot.ControllerState.UsingController)
		PlayerManager.playerSlots(num).controllerId = controllerId
	End Sub

	' Token: 0x0600425F RID: 16991 RVA: 0x0023D161 File Offset: 0x0023B561
	Public Sub ControlsChanged()
		If PlayerManager.OnControlsChanged IsNot Nothing Then
			PlayerManager.OnControlsChanged()
		End If
	End Sub

	' Token: 0x06004260 RID: 16992 RVA: 0x0023D178 File Offset: 0x0023B578
	Private Sub OnUserSignedIn(user As OnlineUser)
		For i As Integer = 0 To PlayerManager.playerSlots.Length - 1
			If PlayerManager.playerSlots(i).canJoin AndAlso PlayerManager.playerSlots(i).joinState = PlayerManager.PlayerSlot.JoinState.JoinRequested Then
				OnlineManager.Instance.[Interface].UpdateControllerMapping()
				If user Is Nothing OrElse (i = 0 AndAlso user.Equals(OnlineManager.Instance.[Interface].SecondaryUser)) OrElse (i = 1 AndAlso user.Equals(OnlineManager.Instance.[Interface].MainUser)) Then
					PlayerManager.playerSlots(i).joinState = PlayerManager.PlayerSlot.JoinState.NotJoining
				Else
					Dim playerId As PlayerId = If((i <> 0), PlayerId.PlayerTwo, PlayerId.PlayerOne)
					OnlineManager.Instance.[Interface].SetUser(playerId, user)
					If PlayerManager.playerSlots(i).controllerState = PlayerManager.PlayerSlot.ControllerState.UsingController Then
						PlayerManager.playerSlots(i).shouldAssignController = True
					End If
					PlayerManager.playerSlots(i).joinState = PlayerManager.PlayerSlot.JoinState.Joined
					If playerId = PlayerId.PlayerTwo Then
						PlayerManager.Multiplayer = True
					End If
					PlayerManager.OnPlayerJoinedEvent(playerId)
				End If
			End If
		Next
		For j As Integer = 0 To PlayerManager.playerSlots.Length - 1
			If PlayerManager.playerSlots(j).canSwitch AndAlso PlayerManager.playerSlots(j).requestedSwitch AndAlso PlayerManager.playerSlots(j).joinState = PlayerManager.PlayerSlot.JoinState.Joined Then
				OnlineManager.Instance.[Interface].UpdateControllerMapping()
				PlayerManager.playerSlots(j).requestedSwitch = False
				Dim playerId2 As PlayerId = If((j <> 0), PlayerId.PlayerTwo, PlayerId.PlayerOne)
				If user IsNot Nothing AndAlso Not user.Equals(OnlineManager.Instance.[Interface].MainUser) AndAlso Not user.Equals(OnlineManager.Instance.[Interface].SecondaryUser) Then
					OnlineManager.Instance.[Interface].SetUser(playerId2, user)
					If j = 0 Then
						PlayerManager.shouldGoToSlotSelect = True
					End If
				End If
			End If
		Next
	End Sub

	' Token: 0x06004261 RID: 16993 RVA: 0x0023D368 File Offset: 0x0023B768
	Private Sub OnUserSignedOut(player As PlayerId, name As String)
		If PlmManager.Instance.[Interface].IsConstrained() Then
			Return
		End If
		Dim playerSlot As PlayerManager.PlayerSlot = If((player <> PlayerId.PlayerOne), PlayerManager.playerSlots(1), PlayerManager.playerSlots(0))
		If playerSlot.requestedSwitch Then
			Return
		End If
		PlayerManager.PlayerLeave(player)
	End Sub

	' Token: 0x06004262 RID: 16994 RVA: 0x0023D3B8 File Offset: 0x0023B7B8
	Private Sub OnControllerDisconnected(args As ControllerStatusChangedEventArgs)
		If PlmManager.Instance.[Interface].IsConstrained() Then
			Return
		End If
		For i As Integer = 0 To PlayerManager.playerSlots.Length - 1
			Dim playerId As PlayerId = If((i <> 0), PlayerId.PlayerTwo, PlayerId.PlayerOne)
			If PlayerManager.playerSlots(i).controllerState = PlayerManager.PlayerSlot.ControllerState.UsingController AndAlso PlayerManager.playerSlots(i).controllerId = args.controllerId AndAlso PlayerManager.playerSlots(i).joinState = PlayerManager.PlayerSlot.JoinState.Joined Then
				PlayerManager.playerInputs(CInt(playerId)).controllers.RemoveController(Of Joystick)(args.controllerId)
				PlayerManager.playerSlots(i).controllerState = PlayerManager.PlayerSlot.ControllerState.Disconnected
				If playerId = PlayerId.PlayerOne Then
					PlayerManager.player1DisconnectedControllerId = args.controllerId
				End If
			End If
		Next
	End Sub

	' Token: 0x06004263 RID: 16995 RVA: 0x0023D479 File Offset: 0x0023B879
	Private Sub OnSuspend()
	End Sub

	' Token: 0x06004264 RID: 16996 RVA: 0x0023D47B File Offset: 0x0023B87B
	Private Sub OnResume()
	End Sub

	' Token: 0x06004265 RID: 16997 RVA: 0x0023D47D File Offset: 0x0023B87D
	Private Sub OnCloudStorageInitialized(success As Boolean)
		If Not success Then
			OnlineManager.Instance.[Interface].InitializeCloudStorage(PlayerId.PlayerOne, AddressOf PlayerManager.OnCloudStorageInitialized)
			Return
		End If
	End Sub

	' Token: 0x06004266 RID: 16998 RVA: 0x0023D4B3 File Offset: 0x0023B8B3
	Private Sub OnUnconstrained()
		PlayerManager.CheckForPairingsChanges()
	End Sub

	' Token: 0x06004267 RID: 16999 RVA: 0x0023D4BC File Offset: 0x0023B8BC
	Private Sub CheckForPairingsChanges()
		Dim flag As Boolean = OnlineManager.Instance.[Interface].ControllerMappingChanged()
		For i As Integer = 0 To PlayerManager.playerSlots.Length - 1
			Dim playerId As PlayerId = If((i <> 0), PlayerId.PlayerTwo, PlayerId.PlayerOne)
			If PlayerManager.playerSlots(i).joinState = PlayerManager.PlayerSlot.JoinState.Joined Then
				If Not OnlineManager.Instance.[Interface].IsUserSignedIn(playerId) Then
					PlayerManager.PlayerLeave(playerId)
					If playerId = PlayerId.PlayerOne Then
						PlayerManager.PlayerLeave(PlayerId.PlayerTwo)
					End If
				ElseIf Not flag Then
					If PlayerManager.playerSlots(i).controllerState = PlayerManager.PlayerSlot.ControllerState.UsingController AndAlso PlayerManager.playerInputs(CInt(playerId)).controllers.joystickCount = 0 Then
						PlayerManager.playerInputs(CInt(playerId)).controllers.AddController(Of Joystick)(PlayerManager.playerSlots(i).controllerId, True)
						ReInput.userDataStore.LoadControllerData(PlayerManager.playerInputs(CInt(playerId)).id, ControllerType.Joystick, PlayerManager.playerSlots(i).controllerId)
					End If
				Else
					Dim controllersForUser As List(Of ULong) = OnlineManager.Instance.[Interface].GetControllersForUser(playerId)
					If controllersForUser Is Nothing OrElse controllersForUser.Count <> 1 Then
						PlayerManager.playerInputs(CInt(playerId)).controllers.ClearControllersOfType(Of Joystick)()
						PlayerManager.playerSlots(i).controllerState = PlayerManager.PlayerSlot.ControllerState.Disconnected
						PlayerManager.playerSlots(i).controllerDisconnectFromPlm = True
					Else
						Dim num As ULong = controllersForUser(0)
						For Each joystick As Joystick In ReInput.controllers.Joysticks
							If joystick.systemId.Value = CLng(num) Then
								If PlayerManager.playerInputs(CInt(playerId)).controllers.joystickCount > 0 Then
								End If
								PlayerManager.playerInputs(CInt(playerId)).controllers.ClearControllersOfType(Of Joystick)()
								PlayerManager.playerInputs(CInt(playerId)).controllers.AddController(joystick, True)
								ReInput.userDataStore.LoadControllerData(PlayerManager.playerInputs(CInt(playerId)).id, ControllerType.Joystick, PlayerManager.playerSlots(i).controllerId)
								PlayerManager.playerSlots(i).controllerId = joystick.id
								Exit For
							End If
						Next
					End If
				End If
			End If
		Next
	End Sub

	' Token: 0x06004268 RID: 17000 RVA: 0x0023D710 File Offset: 0x0023BB10
	Public Sub LoadControllerMappings(player As PlayerId)
		Dim num As Integer = If((player <> PlayerId.PlayerOne), 1, 0)
		ReInput.userDataStore.LoadControllerData(PlayerManager.playerInputs(CInt(player)).id, ControllerType.Joystick, PlayerManager.playerSlots(num).controllerId)
	End Sub

	' Token: 0x06004269 RID: 17001 RVA: 0x0023D754 File Offset: 0x0023BB54
	Public Function IsControllerDisconnected(playerId As PlayerId, Optional countWaitingForReconnectAsDisconnected As Boolean = True) As Boolean
		Dim num As Integer = If((playerId <> PlayerId.PlayerOne), 1, 0)
		Return PlayerManager.playerSlots(num).joinState = PlayerManager.PlayerSlot.JoinState.Joined AndAlso (PlayerManager.playerSlots(num).controllerState = PlayerManager.PlayerSlot.ControllerState.Disconnected OrElse PlayerManager.playerSlots(num).controllerState = PlayerManager.PlayerSlot.ControllerState.ReconnectPromptDisplayed OrElse (countWaitingForReconnectAsDisconnected AndAlso PlayerManager.playerSlots(num).controllerState = PlayerManager.PlayerSlot.ControllerState.WaitingForReconnect))
	End Function

	' Token: 0x0600426A RID: 17002 RVA: 0x0023D7C4 File Offset: 0x0023BBC4
	Public Sub OnDisconnectPromptDisplayed(playerId As PlayerId)
		Dim num As Integer = If((playerId <> PlayerId.PlayerOne), 1, 0)
		PlayerManager.playerSlots(num).controllerState = PlayerManager.PlayerSlot.ControllerState.ReconnectPromptDisplayed
	End Sub

	' Token: 0x0600426B RID: 17003 RVA: 0x0023D7EC File Offset: 0x0023BBEC
	Private Sub goToSlotSelect()
		Cuphead.Current.controlMapper.Close(True)
		PlayerManager.playerSlots(0).canSwitch = False
		PlayerManager.playerSlots(0).requestedSwitch = False
		PlayerManager.playerSlots(0).canJoin = False
		PlayerManager.GetPlayerInput(PlayerId.PlayerTwo).controllers.ClearControllersOfType(Of Joystick)()
		PlayerManager.playerSlots(1) = New PlayerManager.PlayerSlot()
		PlayerManager.Multiplayer = False
		OnlineManager.Instance.[Interface].SetUser(PlayerId.PlayerTwo, Nothing)
		SceneLoader.LoadScene(Scenes.scene_slot_select, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, Nothing)
	End Sub

	' Token: 0x0600426C RID: 17004 RVA: 0x0023D86D File Offset: 0x0023BC6D
	Private Sub goToStartScreen()
		Cuphead.Current.controlMapper.Close(True)
		PlayerManager.ResetPlayers()
		If StartScreenAudio.Instance IsNot Nothing Then
			Global.UnityEngine.[Object].Destroy(StartScreenAudio.Instance.gameObject)
		End If
		SceneLoader.LoadScene(Scenes.scene_title, SceneLoader.Transition.Fade, SceneLoader.Transition.Fade, SceneLoader.Icon.Hourglass, Nothing)
	End Sub

	' Token: 0x0600426D RID: 17005 RVA: 0x0023D8B0 File Offset: 0x0023BCB0
	Public Sub ResetPlayers()
		PlayerManager.playerSlots(0) = New PlayerManager.PlayerSlot()
		PlayerManager.playerSlots(1) = New PlayerManager.PlayerSlot()
		PlayerManager.GetPlayerInput(PlayerId.PlayerOne).controllers.ClearControllersOfType(Of Joystick)()
		PlayerManager.GetPlayerInput(PlayerId.PlayerTwo).controllers.ClearControllersOfType(Of Joystick)()
		PlayerManager.Multiplayer = False
		If OnlineManager.Instance.[Interface].SupportsMultipleUsers Then
			OnlineManager.Instance.[Interface].SetUser(PlayerId.PlayerOne, Nothing)
			OnlineManager.Instance.[Interface].SetUser(PlayerId.PlayerTwo, Nothing)
		End If
	End Sub

	' Token: 0x0600426E RID: 17006 RVA: 0x0023D931 File Offset: 0x0023BD31
	Public Function GetPlayerInput(id As PlayerId) As Player
		Return PlayerManager.playerInputs(CInt(id))
	End Function

	' Token: 0x170005D3 RID: 1491
	' (get) Token: 0x0600426F RID: 17007 RVA: 0x0023D93E File Offset: 0x0023BD3E
	Public ReadOnly Property Current As AbstractPlayerController
		Get
			Return PlayerManager.GetPlayer(PlayerManager.currentId)
		End Get
	End Property

	' Token: 0x06004270 RID: 17008 RVA: 0x0023D94A File Offset: 0x0023BD4A
	Public Sub SetPlayer(id As PlayerId, player As AbstractPlayerController)
		PlayerManager.players(CInt(id)) = player
	End Sub

	' Token: 0x06004271 RID: 17009 RVA: 0x0023D958 File Offset: 0x0023BD58
	Public Sub ClearPlayer(id As PlayerId)
		PlayerManager.players(CInt(id)) = Nothing
	End Sub

	' Token: 0x06004272 RID: 17010 RVA: 0x0023D966 File Offset: 0x0023BD66
	Public Sub ClearPlayers()
		PlayerManager.currentId = PlayerId.PlayerOne
		PlayerManager.players(0) = Nothing
		PlayerManager.players(1) = Nothing
	End Sub

	' Token: 0x06004273 RID: 17011 RVA: 0x0023D986 File Offset: 0x0023BD86
	Public Function GetPlayer(id As PlayerId) As AbstractPlayerController
		Return PlayerManager.players(CInt(id))
	End Function

	' Token: 0x06004274 RID: 17012 RVA: 0x0023D993 File Offset: 0x0023BD93
	Public Function GetPlayer(Of T As AbstractPlayerController)(id As PlayerId) As T
		Return TryCast(PlayerManager.GetPlayer(id), T)
	End Function

	' Token: 0x06004275 RID: 17013 RVA: 0x0023D9A5 File Offset: 0x0023BDA5
	Public Function GetRandom() As AbstractPlayerController
		If Not PlayerManager.Multiplayer OrElse Not PlayerManager.DoesPlayerExist(PlayerId.PlayerTwo) Then
			Return PlayerManager.players(0)
		End If
		Return PlayerManager.GetPlayer(EnumUtils.Random(Of PlayerId)())
	End Function

	' Token: 0x06004276 RID: 17014 RVA: 0x0023D9D4 File Offset: 0x0023BDD4
	Public Function GetNext() As AbstractPlayerController
		If Not PlayerManager.Multiplayer OrElse Not PlayerManager.DoesPlayerExist(PlayerId.PlayerTwo) Then
			Return PlayerManager.players(0)
		End If
		If Not PlayerManager.DoesPlayerExist(PlayerId.PlayerOne) Then
			Return PlayerManager.players(1)
		End If
		Dim abstractPlayerController As AbstractPlayerController = PlayerManager.Current
		Dim playerId As PlayerId = PlayerManager.currentId
		If playerId = PlayerId.PlayerOne OrElse playerId <> PlayerId.PlayerTwo Then
			PlayerManager.currentId = PlayerId.PlayerTwo
		Else
			PlayerManager.currentId = PlayerId.PlayerOne
		End If
		Return abstractPlayerController
	End Function

	' Token: 0x06004277 RID: 17015 RVA: 0x0023DA4E File Offset: 0x0023BE4E
	Public Function DoesPlayerExist(player As PlayerId) As Boolean
		Return Not(PlayerManager.players(CInt(player)) Is Nothing) AndAlso Not PlayerManager.players(CInt(player)).IsDead
	End Function

	' Token: 0x06004278 RID: 17016 RVA: 0x0023DA80 File Offset: 0x0023BE80
	Public Function BothPlayersActive() As Boolean
		Return PlayerManager.DoesPlayerExist(PlayerId.PlayerOne) AndAlso PlayerManager.DoesPlayerExist(PlayerId.PlayerTwo)
	End Function

	' Token: 0x06004279 RID: 17017 RVA: 0x0023DA96 File Offset: 0x0023BE96
	Public Function GetFirst() As AbstractPlayerController
		If Not PlayerManager.DoesPlayerExist(PlayerId.PlayerOne) Then
			Return PlayerManager.players(1)
		End If
		Return PlayerManager.players(0)
	End Function

	' Token: 0x0600427A RID: 17018 RVA: 0x0023DABA File Offset: 0x0023BEBA
	Public Function GetAllPlayers() As Dictionary(Of Integer, AbstractPlayerController).ValueCollection
		Return PlayerManager.players.Values
	End Function

	' Token: 0x170005D4 RID: 1492
	' (get) Token: 0x0600427B RID: 17019 RVA: 0x0023DAC8 File Offset: 0x0023BEC8
	Public ReadOnly Property Count As Integer
		Get
			Dim num As Integer = 0
			For Each num2 As Integer In PlayerManager.players.Keys
				If PlayerManager.DoesPlayerExist(CType(num2, PlayerId)) AndAlso Not PlayerManager.GetPlayer(CType(num2, PlayerId)).IsDead Then
					num += 1
				End If
			Next
			Return num
		End Get
	End Property

	' Token: 0x170005D5 RID: 1493
	' (get) Token: 0x0600427C RID: 17020 RVA: 0x0023DB44 File Offset: 0x0023BF44
	Public ReadOnly Property Center As Vector2
		Get
			If Not PlayerManager.Multiplayer OrElse PlayerManager.Count < 2 Then
				Return PlayerManager.GetFirst().center
			End If
			Return(PlayerManager.players(0).center + PlayerManager.players(1).center) / 2F
		End Get
	End Property

	' Token: 0x170005D6 RID: 1494
	' (get) Token: 0x0600427D RID: 17021 RVA: 0x0023DBAC File Offset: 0x0023BFAC
	Public ReadOnly Property CameraCenter As Vector2
		Get
			If Not PlayerManager.Multiplayer OrElse PlayerManager.Count < 2 Then
				Return PlayerManager.GetFirst().CameraCenter
			End If
			Return(PlayerManager.players(0).center + PlayerManager.players(1).CameraCenter) / 2F
		End Get
	End Property

	' Token: 0x170005D7 RID: 1495
	' (get) Token: 0x0600427E RID: 17022 RVA: 0x0023DC14 File Offset: 0x0023C014
	Public ReadOnly Property TopPlayerPosition As Vector2
		Get
			If Not PlayerManager.Multiplayer OrElse PlayerManager.Count < 2 Then
				Return PlayerManager.GetFirst().transform.position
			End If
			Dim num As Single = Mathf.Max(PlayerManager.players(0).transform.position.y, PlayerManager.players(1).transform.position.y)
			Return New Vector2((PlayerManager.players(0).transform.position.x + PlayerManager.players(0).transform.position.x) / 2F, num)
		End Get
	End Property

	' Token: 0x170005D8 RID: 1496
	' (get) Token: 0x0600427F RID: 17023 RVA: 0x0023DCD3 File Offset: 0x0023C0D3
	Public ReadOnly Property DamageMultiplier As Single
		Get
			If PlayerManager.Count > 1 Then
				Return 0.5F
			End If
			Return 1F
		End Get
	End Property

	' Token: 0x040048BF RID: 18623
	Private Const SINGLE_PLAYER_DAMAGE_MULTIPLIER As Single = 1F

	' Token: 0x040048C0 RID: 18624
	Private Const MULTIPLAYER_DAMAGE_MULTIPLIER As Single = 0.5F

	' Token: 0x040048C1 RID: 18625
	Private playerSlots As PlayerManager.PlayerSlot() = New PlayerManager.PlayerSlot() { New PlayerManager.PlayerSlot(), New PlayerManager.PlayerSlot() }

	' Token: 0x040048C2 RID: 18626
	Public Multiplayer As Boolean

	' Token: 0x040048C3 RID: 18627
	Private shouldGoToSlotSelect As Boolean = False

	' Token: 0x040048C4 RID: 18628
	Private shouldGoToStartScreen As Boolean = False

	' Token: 0x040048C5 RID: 18629
	Private pausedDueToPlm As Boolean = False

	' Token: 0x040048C9 RID: 18633
	Public player1DisconnectedControllerId As Integer

	' Token: 0x040048CA RID: 18634
	Public player1IsMugman As Boolean

	' Token: 0x040048CB RID: 18635
	Public playerWasChalice As Boolean() = New Boolean(1) {}

	' Token: 0x040048CC RID: 18636
	Private playerInputs As Dictionary(Of Integer, Player)

	' Token: 0x040048CD RID: 18637
	Private players As Dictionary(Of Integer, AbstractPlayerController)

	' Token: 0x040048CE RID: 18638
	Private currentId As PlayerId

	' Token: 0x02000ACC RID: 2764
	Private Class PlayerSlot
		' Token: 0x040048D7 RID: 18647
		Public canJoin As Boolean

		' Token: 0x040048D8 RID: 18648
		Public joinState As PlayerManager.PlayerSlot.JoinState

		' Token: 0x040048D9 RID: 18649
		Public controllerState As PlayerManager.PlayerSlot.ControllerState

		' Token: 0x040048DA RID: 18650
		Public canSwitch As Boolean

		' Token: 0x040048DB RID: 18651
		Public requestedSwitch As Boolean

		' Token: 0x040048DC RID: 18652
		Public promptBeforeJoin As Boolean

		' Token: 0x040048DD RID: 18653
		Public controllerId As Integer

		' Token: 0x040048DE RID: 18654
		Public shouldAssignController As Boolean

		' Token: 0x040048DF RID: 18655
		Public controllerDisconnectFromPlm As Boolean

		' Token: 0x040048E0 RID: 18656
		Public lastController As ControllerType = ControllerType.Custom

		' Token: 0x02000ACD RID: 2765
		Public Enum JoinState
			' Token: 0x040048E2 RID: 18658
			NotJoining
			' Token: 0x040048E3 RID: 18659
			JoinPromptDisplayed
			' Token: 0x040048E4 RID: 18660
			JoinRequested
			' Token: 0x040048E5 RID: 18661
			Joined
			' Token: 0x040048E6 RID: 18662
			Leaving
		End Enum

		' Token: 0x02000ACE RID: 2766
		Public Enum ControllerState
			' Token: 0x040048E8 RID: 18664
			NoController
			' Token: 0x040048E9 RID: 18665
			UsingController
			' Token: 0x040048EA RID: 18666
			Disconnected
			' Token: 0x040048EB RID: 18667
			ReconnectPromptDisplayed
			' Token: 0x040048EC RID: 18668
			WaitingForReconnect
		End Enum
	End Class

	' Token: 0x02000ACF RID: 2767
	' (Invoke) Token: 0x06004283 RID: 17027
	Public Delegate Sub PlayerChangedDelegate(playerId As PlayerId)
End Module
