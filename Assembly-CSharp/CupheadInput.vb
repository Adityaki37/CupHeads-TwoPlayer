Imports System
Imports Rewired

' Token: 0x0200046F RID: 1135
Public Class CupheadInput
	' Token: 0x06001163 RID: 4451 RVA: 0x000A53CC File Offset: 0x000A37CC
	Public Shared Function InputDisplayForButton(button As CupheadButton, Optional rewiredPlayerId As Integer = 0) As Localization.Translation
		Dim controllers As Player.ControllerHelper = ReInput.players.GetPlayer(rewiredPlayerId).controllers
		Dim actionElementMap As ActionElementMap
		If controllers IsNot Nothing AndAlso controllers.joystickCount > 0 Then
			Dim controllerType As ControllerType = ControllerType.Joystick
			Dim lastActiveController As Controller = ReInput.players.GetPlayer(rewiredPlayerId).controllers.GetLastActiveController()
			If lastActiveController IsNot Nothing Then
				controllerType = lastActiveController.type
			End If
			actionElementMap = controllers.maps.GetFirstElementMapWithAction(controllerType, CInt(button), True)
		Else
			If PlatformHelper.IsConsole Then
				Return Nothing
			End If
			actionElementMap = ReInput.players.GetPlayer(rewiredPlayerId).controllers.maps.GetFirstElementMapWithAction(CInt(button), True)
		End If
		If actionElementMap Is Nothing Then
			Return New Localization.Translation() With { .text = String.Empty }
		End If
		Dim text As String = actionElementMap.elementIdentifierName
		If button = CupheadButton.EquipMenu AndAlso text.Contains("Shift") Then
			text = "Shift"
		End If
		Dim text2 As String = CupheadInput.handleCustomGlyphs(text, rewiredPlayerId)
		Dim translation As Localization.Translation = Localization.Translate(text)
		If text2 Is Nothing Then
			If Not String.IsNullOrEmpty(translation.text) Then
				text = translation.text
			End If
		Else
			text = text2
		End If
		text = text.ToUpper()
		text = text.Replace(" SHOULDER", "B")
		text = text.Replace(" BUMPER", "B")
		text = text.Replace(" TRIGGER", "T")
		text = text.Replace("LEFT", "L")
		text = text.Replace("RIGHT", "R")
		text = text.Replace("R SHIFT", "SHIFT")
		text = text.Replace("L SHIFT", "SHIFT")
		text = text.Replace(" +", String.Empty)
		text = text.Replace(" -", String.Empty)
		translation.text = text
		Return translation
	End Function

	' Token: 0x06001164 RID: 4452 RVA: 0x000A55AF File Offset: 0x000A39AF
	Private Shared Function handleCustomGlyphs(input As String, rewiredPlayerId As Integer) As String
		Return Nothing
	End Function

	' Token: 0x06001165 RID: 4453 RVA: 0x000A55B4 File Offset: 0x000A39B4
	Public Shared Function InputSymbolForButton(button As CupheadButton) As CupheadInput.InputSymbols
		Dim inputSymbols As CupheadInput.InputSymbols
		Select Case button
			Case CupheadButton.Jump
				inputSymbols = CupheadInput.InputSymbols.XBOX_A
			Case CupheadButton.Shoot
				inputSymbols = CupheadInput.InputSymbols.XBOX_X
			Case CupheadButton.Super
				inputSymbols = CupheadInput.InputSymbols.XBOX_B
			Case CupheadButton.SwitchWeapon
				inputSymbols = CupheadInput.InputSymbols.XBOX_LB
			Case CupheadButton.Lock
				inputSymbols = CupheadInput.InputSymbols.XBOX_RB
			Case CupheadButton.Dash
				inputSymbols = CupheadInput.InputSymbols.XBOX_Y
			Case Else
				If button <> CupheadButton.None Then
				End If
				inputSymbols = CupheadInput.InputSymbols.XBOX_NONE
			Case CupheadButton.Accept
				inputSymbols = CupheadInput.InputSymbols.XBOX_A
			Case CupheadButton.Cancel
				inputSymbols = CupheadInput.InputSymbols.XBOX_B
		End Select
		Return inputSymbols
	End Function

	' Token: 0x06001166 RID: 4454 RVA: 0x000A564F File Offset: 0x000A3A4F
	Public Shared Function DialogueStringFromButton(button As CupheadButton) As String
		Return " {" + button + "} "
	End Function

	' Token: 0x06001167 RID: 4455 RVA: 0x000A5668 File Offset: 0x000A3A68
	Public Shared Function CheckForUnconnectedControllerPress() As Joystick
		For Each joystick As Joystick In ReInput.controllers.Joysticks
			If Not ReInput.controllers.IsJoystickAssigned(joystick) Then
				If joystick.GetAnyButtonDown() Then
					Return joystick
				End If
			End If
		Next
		Return Nothing
	End Function

	' Token: 0x06001168 RID: 4456 RVA: 0x000A56E8 File Offset: 0x000A3AE8
	Public Shared Function CheckForControllerPress(systemID As Long) As Joystick
		For Each joystick As Joystick In ReInput.controllers.Joysticks
			If joystick.systemId = systemID AndAlso joystick.GetAnyButtonDown() Then
				Return joystick
			End If
		Next
		Return Nothing
	End Function

	' Token: 0x06001169 RID: 4457 RVA: 0x000A5774 File Offset: 0x000A3B74
	Public Shared Function AutoAssignController(rewiredPlayerId As Integer) As Boolean
		For Each joystick As Joystick In ReInput.controllers.Joysticks
			If Not ReInput.controllers.IsJoystickAssigned(joystick) Then
				Dim player As Player = ReInput.players.GetPlayer(rewiredPlayerId)
				If player IsNot Nothing Then
					If player.controllers.joystickCount <= 0 Then
						player.controllers.AddController(joystick, True)
						Return True
					End If
				End If
			End If
		Next
		Return False
	End Function

	' Token: 0x04001AF8 RID: 6904
	Public Shared pairs As CupheadInput.Pair() = New CupheadInput.Pair() { New CupheadInput.Pair(CupheadInput.InputSymbols.XBOX_A, "<sprite=0>", "<sprite=1>"), New CupheadInput.Pair(CupheadInput.InputSymbols.XBOX_B, "<sprite=2>", "<sprite=3>"), New CupheadInput.Pair(CupheadInput.InputSymbols.XBOX_X, "<sprite=4>", "<sprite=5>"), New CupheadInput.Pair(CupheadInput.InputSymbols.XBOX_Y, "<sprite=6>", "<sprite=7>") }

	' Token: 0x02000470 RID: 1136
	Public Enum InputDevice
		' Token: 0x04001AFA RID: 6906
		Keyboard
		' Token: 0x04001AFB RID: 6907
		Controller_1
		' Token: 0x04001AFC RID: 6908
		Controller_2
	End Enum

	' Token: 0x02000471 RID: 1137
	Public Enum InputSymbols
		' Token: 0x04001AFE RID: 6910
		XBOX_NONE
		' Token: 0x04001AFF RID: 6911
		XBOX_A
		' Token: 0x04001B00 RID: 6912
		XBOX_B
		' Token: 0x04001B01 RID: 6913
		XBOX_X
		' Token: 0x04001B02 RID: 6914
		XBOX_Y
		' Token: 0x04001B03 RID: 6915
		XBOX_RB
		' Token: 0x04001B04 RID: 6916
		XBOX_LB
	End Enum

	' Token: 0x02000472 RID: 1138
	Public Class AnyPlayerInput
		' Token: 0x0600116B RID: 4459 RVA: 0x000A5884 File Offset: 0x000A3C84
		Public Sub New(Optional checkIfDead As Boolean = False)
			Me.checkIfDead = checkIfDead
			Me.players = New Player() { PlayerManager.GetPlayerInput(PlayerId.PlayerOne), PlayerManager.GetPlayerInput(PlayerId.PlayerTwo) }
		End Sub

		' Token: 0x0600116C RID: 4460 RVA: 0x000A58B4 File Offset: 0x000A3CB4
		Public Function GetButton(button As CupheadButton) As Boolean
			For Each player As Player In Me.players
				If player.GetButton(CInt(button)) AndAlso (Not Me.checkIfDead OrElse Not Me.IsDead(player)) Then
					Return True
				End If
			Next
			Return False
		End Function

		' Token: 0x0600116D RID: 4461 RVA: 0x000A5908 File Offset: 0x000A3D08
		Public Function GetButtonDown(button As CupheadButton) As Boolean
			If InterruptingPrompt.IsInterrupting() Then
				Return False
			End If
			For Each player As Player In Me.players
				If player.GetButtonDown(CInt(button)) AndAlso (Not Me.checkIfDead OrElse Not Me.IsDead(player)) Then
					Return True
				End If
			Next
			Return False
		End Function

		' Token: 0x0600116E RID: 4462 RVA: 0x000A5968 File Offset: 0x000A3D68
		Public Function GetActionButtonDown() As Boolean
			If InterruptingPrompt.IsInterrupting() Then
				Return False
			End If
			For Each player As Player In Me.players
				If(player.GetButtonDown(13) OrElse player.GetButtonDown(14) OrElse player.GetButtonDown(7) OrElse player.GetButtonDown(15) OrElse player.GetButtonDown(2) OrElse player.GetButtonDown(6) OrElse player.GetButtonDown(8) OrElse player.GetButtonDown(3) OrElse player.GetButtonDown(4) OrElse player.GetButtonDown(5)) AndAlso (Not Me.checkIfDead OrElse Not Me.IsDead(player)) Then
					Return True
				End If
			Next
			Return False
		End Function

		' Token: 0x0600116F RID: 4463 RVA: 0x000A5A38 File Offset: 0x000A3E38
		Public Function GetAnyButtonDown() As Boolean
			If InterruptingPrompt.IsInterrupting() Then
				Return False
			End If
			For Each player As Player In Me.players
				For Each controller As Controller In player.controllers.Controllers
					If controller.GetAnyButtonDown() AndAlso (Not Me.checkIfDead OrElse Not Me.IsDead(player)) Then
						Return True
					End If
				Next
			Next
			Return False
		End Function

		' Token: 0x06001170 RID: 4464 RVA: 0x000A5AF0 File Offset: 0x000A3EF0
		Public Function GetAnyButtonHeld() As Boolean
			If InterruptingPrompt.IsInterrupting() Then
				Return False
			End If
			For Each player As Player In Me.players
				For Each controller As Controller In player.controllers.Controllers
					If controller.GetAnyButton() AndAlso (Not Me.checkIfDead OrElse Not Me.IsDead(player)) Then
						Return True
					End If
				Next
			Next
			Return False
		End Function

		' Token: 0x06001171 RID: 4465 RVA: 0x000A5BA8 File Offset: 0x000A3FA8
		Public Function GetButtonUp(button As CupheadButton) As Boolean
			For Each player As Player In Me.players
				If player.GetButtonUp(CInt(button)) AndAlso (Not Me.checkIfDead OrElse Not Me.IsDead(player)) Then
					Return True
				End If
			Next
			Return False
		End Function

		' Token: 0x06001172 RID: 4466 RVA: 0x000A5BFC File Offset: 0x000A3FFC
		Private Function IsDead(player As Player) As Boolean
			Dim playerId As PlayerId = If((player IsNot Me.players(0)), PlayerId.PlayerTwo, PlayerId.PlayerOne)
			Dim player2 As AbstractPlayerController = PlayerManager.GetPlayer(playerId)
			Return player2 Is Nothing OrElse player2.IsDead
		End Function

		' Token: 0x04001B05 RID: 6917
		Private players As Player()

		' Token: 0x04001B06 RID: 6918
		Public checkIfDead As Boolean
	End Class

	' Token: 0x02000473 RID: 1139
	Public Class Pair
		' Token: 0x06001173 RID: 4467 RVA: 0x000A5C3B File Offset: 0x000A403B
		Public Sub New(symbol As CupheadInput.InputSymbols, first As String, second As String)
			Me.symbol = symbol
			Me.first = first
			Me.second = second
		End Sub

		' Token: 0x04001B07 RID: 6919
		Public symbol As CupheadInput.InputSymbols

		' Token: 0x04001B08 RID: 6920
		Public first As String

		' Token: 0x04001B09 RID: 6921
		Public second As String
	End Class
End Class
