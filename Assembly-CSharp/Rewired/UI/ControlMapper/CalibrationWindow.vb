Imports System
Imports System.Collections.Generic
Imports Rewired.Integration.UnityUI
Imports Rewired.Utils
Imports UnityEngine
Imports UnityEngine.UI

Namespace Rewired.UI.ControlMapper
	' Token: 0x02000C07 RID: 3079
	<AddComponentMenu("")>
	Public Class CalibrationWindow
		Inherits Window

		' Token: 0x17000697 RID: 1687
		' (get) Token: 0x06004972 RID: 18802 RVA: 0x0026667C File Offset: 0x00264A7C
		Private ReadOnly Property axisSelected As Boolean
			Get
				Return Me.joystick IsNot Nothing AndAlso Me.selectedAxis >= 0 AndAlso Me.selectedAxis < Me.joystick.calibrationMap.axisCount
			End Get
		End Property

		' Token: 0x17000698 RID: 1688
		' (get) Token: 0x06004973 RID: 18803 RVA: 0x002666B5 File Offset: 0x00264AB5
		Private ReadOnly Property axisCalibration As AxisCalibration
			Get
				If Not Me.axisSelected Then
					Return Nothing
				End If
				Return Me.joystick.calibrationMap.GetAxis(Me.selectedAxis)
			End Get
		End Property

		' Token: 0x06004974 RID: 18804 RVA: 0x002666DC File Offset: 0x00264ADC
		Public Overrides Sub Initialize(id As Integer, isFocusedCallback As Func(Of Integer, Boolean))
			If Me.rightContentContainer Is Nothing OrElse Me.valueDisplayGroup Is Nothing OrElse Me.calibratedValueMarker Is Nothing OrElse Me.rawValueMarker Is Nothing OrElse Me.calibratedZeroMarker Is Nothing OrElse Me.deadzoneArea Is Nothing OrElse Me.deadzoneSlider Is Nothing OrElse Me.sensitivitySlider Is Nothing OrElse Me.zeroSlider Is Nothing OrElse Me.invertToggle Is Nothing OrElse Me.axisScrollAreaContent Is Nothing OrElse Me.doneButton Is Nothing OrElse Me.calibrateButton Is Nothing OrElse Me.axisButtonPrefab Is Nothing OrElse Me.doneButtonLabel Is Nothing OrElse Me.cancelButtonLabel Is Nothing OrElse Me.defaultButtonLabel Is Nothing OrElse Me.deadzoneSliderLabel Is Nothing OrElse Me.zeroSliderLabel Is Nothing OrElse Me.sensitivitySliderLabel Is Nothing OrElse Me.invertToggleLabel Is Nothing OrElse Me.calibrateButtonLabel Is Nothing Then
				Global.UnityEngine.Debug.LogError("Rewired Control Mapper: All inspector values must be assigned!")
				Return
			End If
			Me.axisButtons = New List(Of Button)()
			Me.buttonCallbacks = New Dictionary(Of Integer, Action(Of Integer))()
			Me.doneButtonLabel.text = ControlMapper.GetLanguage().done
			Me.cancelButtonLabel.text = ControlMapper.GetLanguage().cancel
			Me.defaultButtonLabel.text = ControlMapper.GetLanguage().default_
			Me.deadzoneSliderLabel.text = ControlMapper.GetLanguage().calibrateWindow_deadZoneSliderLabel
			Me.zeroSliderLabel.text = ControlMapper.GetLanguage().calibrateWindow_zeroSliderLabel
			Me.sensitivitySliderLabel.text = ControlMapper.GetLanguage().calibrateWindow_sensitivitySliderLabel
			Me.invertToggleLabel.text = ControlMapper.GetLanguage().calibrateWindow_invertToggleLabel
			Me.calibrateButtonLabel.text = ControlMapper.GetLanguage().calibrateWindow_calibrateButtonLabel
			MyBase.Initialize(id, isFocusedCallback)
		End Sub

		' Token: 0x06004975 RID: 18805 RVA: 0x00266930 File Offset: 0x00264D30
		Public Sub SetJoystick(playerId As Integer, joystick As Joystick)
			If Not MyBase.initialized Then
				Return
			End If
			Me.playerId = playerId
			Me.joystick = joystick
			If joystick Is Nothing Then
				Global.UnityEngine.Debug.LogError("Rewired Control Mapper: Joystick cannot be null!")
				Return
			End If
			Dim num As Single = 0F
			For i As Integer = 0 To joystick.axisCount - 1
				Dim index As Integer = i
				Dim gameObject As GameObject = UITools.InstantiateGUIObject(Of Button)(Me.axisButtonPrefab, Me.axisScrollAreaContent, "Axis" + i)
				Dim button As Button = gameObject.GetComponent(Of Button)()
				button.onClick.AddListener(Sub()
					Me.OnAxisSelected(index, button)
				End Sub)
				Dim componentInSelfOrChildren As Text = UnityTools.GetComponentInSelfOrChildren(Of Text)(gameObject)
				If componentInSelfOrChildren IsNot Nothing Then
					componentInSelfOrChildren.text = joystick.AxisElementIdentifiers(i).name
				End If
				If num = 0F Then
					num = UnityTools.GetComponentInSelfOrChildren(Of LayoutElement)(gameObject).minHeight
				End If
				Me.axisButtons.Add(button)
			Next
			Dim spacing As Single = Me.axisScrollAreaContent.GetComponent(Of VerticalLayoutGroup)().spacing
			Me.axisScrollAreaContent.sizeDelta = New Vector2(Me.axisScrollAreaContent.sizeDelta.x, Mathf.Max(CSng(joystick.axisCount) * (num + spacing) - spacing, Me.axisScrollAreaContent.sizeDelta.y))
			Me.origCalibrationData = joystick.calibrationMap.ToXmlString()
			Me.displayAreaWidth = Me.rightContentContainer.sizeDelta.x
			Me.rewiredStandaloneInputModule = MyBase.gameObject.transform.root.GetComponentInChildren(Of RewiredStandaloneInputModule)()
			If Me.rewiredStandaloneInputModule IsNot Nothing Then
				Me.menuHorizActionId = ReInput.mapping.GetActionId(Me.rewiredStandaloneInputModule.horizontalAxis)
				Me.menuVertActionId = ReInput.mapping.GetActionId(Me.rewiredStandaloneInputModule.verticalAxis)
			End If
			If joystick.axisCount > 0 Then
				Me.SelectAxis(0)
			End If
			MyBase.defaultUIElement = Me.doneButton.gameObject
			Me.RefreshControls()
			Me.Redraw()
		End Sub

		' Token: 0x06004976 RID: 18806 RVA: 0x00266B54 File Offset: 0x00264F54
		Public Sub SetButtonCallback(buttonIdentifier As CalibrationWindow.ButtonIdentifier, callback As Action(Of Integer))
			If Not MyBase.initialized Then
				Return
			End If
			If callback Is Nothing Then
				Return
			End If
			If Me.buttonCallbacks.ContainsKey(CInt(buttonIdentifier)) Then
				Me.buttonCallbacks(CInt(buttonIdentifier)) = callback
			Else
				Me.buttonCallbacks.Add(CInt(buttonIdentifier), callback)
			End If
		End Sub

		' Token: 0x06004977 RID: 18807 RVA: 0x00266BA4 File Offset: 0x00264FA4
		Public Overrides Sub Cancel()
			If Not MyBase.initialized Then
				Return
			End If
			If Me.joystick IsNot Nothing Then
				Me.joystick.ImportCalibrationMapFromXmlString(Me.origCalibrationData)
			End If
			Dim action As Action(Of Integer)
			If Not Me.buttonCallbacks.TryGetValue(1, action) Then
				If Me.cancelCallback IsNot Nothing Then
					Me.cancelCallback()
				End If
				Return
			End If
			action(MyBase.id)
		End Sub

		' Token: 0x06004978 RID: 18808 RVA: 0x00266C10 File Offset: 0x00265010
		Protected Overrides Sub Update()
			If Not MyBase.initialized Then
				Return
			End If
			MyBase.Update()
			Me.UpdateDisplay()
		End Sub

		' Token: 0x06004979 RID: 18809 RVA: 0x00266C2C File Offset: 0x0026502C
		Public Sub OnDone()
			If Not MyBase.initialized Then
				Return
			End If
			Dim action As Action(Of Integer)
			If Not Me.buttonCallbacks.TryGetValue(0, action) Then
				Return
			End If
			action(MyBase.id)
		End Sub

		' Token: 0x0600497A RID: 18810 RVA: 0x00266C65 File Offset: 0x00265065
		Public Sub OnCancel()
			Me.Cancel()
		End Sub

		' Token: 0x0600497B RID: 18811 RVA: 0x00266C6D File Offset: 0x0026506D
		Public Sub OnRestoreDefault()
			If Not MyBase.initialized Then
				Return
			End If
			If Me.joystick Is Nothing Then
				Return
			End If
			Me.joystick.calibrationMap.Reset()
			Me.RefreshControls()
			Me.Redraw()
		End Sub

		' Token: 0x0600497C RID: 18812 RVA: 0x00266CA4 File Offset: 0x002650A4
		Public Sub OnCalibrate()
			If Not MyBase.initialized Then
				Return
			End If
			Dim action As Action(Of Integer)
			If Not Me.buttonCallbacks.TryGetValue(3, action) Then
				Return
			End If
			action(Me.selectedAxis)
		End Sub

		' Token: 0x0600497D RID: 18813 RVA: 0x00266CDD File Offset: 0x002650DD
		Public Sub OnInvert(state As Boolean)
			If Not MyBase.initialized Then
				Return
			End If
			If Not Me.axisSelected Then
				Return
			End If
			Me.axisCalibration.invert = state
		End Sub

		' Token: 0x0600497E RID: 18814 RVA: 0x00266D03 File Offset: 0x00265103
		Public Sub OnZeroValueChange(value As Single)
			If Not MyBase.initialized Then
				Return
			End If
			If Not Me.axisSelected Then
				Return
			End If
			Me.axisCalibration.calibratedZero = value
			Me.RedrawCalibratedZero()
		End Sub

		' Token: 0x0600497F RID: 18815 RVA: 0x00266D2F File Offset: 0x0026512F
		Public Sub OnZeroCancel()
			If Not MyBase.initialized Then
				Return
			End If
			If Not Me.axisSelected Then
				Return
			End If
			Me.axisCalibration.calibratedZero = Me.origSelectedAxisCalibrationData.zero
			Me.RedrawCalibratedZero()
			Me.RefreshControls()
		End Sub

		' Token: 0x06004980 RID: 18816 RVA: 0x00266D6C File Offset: 0x0026516C
		Public Sub OnDeadzoneValueChange(value As Single)
			If Not MyBase.initialized Then
				Return
			End If
			If Not Me.axisSelected Then
				Return
			End If
			Me.axisCalibration.deadZone = Mathf.Clamp(value, 0F, 0.8F)
			If value > 0.8F Then
				Me.deadzoneSlider.value = 0.8F
			End If
			Me.RedrawDeadzone()
		End Sub

		' Token: 0x06004981 RID: 18817 RVA: 0x00266DCD File Offset: 0x002651CD
		Public Sub OnDeadzoneCancel()
			If Not MyBase.initialized Then
				Return
			End If
			If Not Me.axisSelected Then
				Return
			End If
			Me.axisCalibration.deadZone = Me.origSelectedAxisCalibrationData.deadZone
			Me.RedrawDeadzone()
			Me.RefreshControls()
		End Sub

		' Token: 0x06004982 RID: 18818 RVA: 0x00266E0C File Offset: 0x0026520C
		Public Sub OnSensitivityValueChange(value As Single)
			If Not MyBase.initialized Then
				Return
			End If
			If Not Me.axisSelected Then
				Return
			End If
			Me.axisCalibration.sensitivity = Mathf.Clamp(value, Me.minSensitivity, Single.PositiveInfinity)
			If value < Me.minSensitivity Then
				Me.sensitivitySlider.value = Me.minSensitivity
			End If
		End Sub

		' Token: 0x06004983 RID: 18819 RVA: 0x00266E6A File Offset: 0x0026526A
		Public Sub OnSensitivityCancel(value As Single)
			If Not MyBase.initialized Then
				Return
			End If
			If Not Me.axisSelected Then
				Return
			End If
			Me.axisCalibration.sensitivity = Me.origSelectedAxisCalibrationData.sensitivity
			Me.RefreshControls()
		End Sub

		' Token: 0x06004984 RID: 18820 RVA: 0x00266EA0 File Offset: 0x002652A0
		Public Sub OnAxisScrollRectScroll(pos As Vector2)
			If Not MyBase.initialized Then
				Return
			End If
		End Sub

		' Token: 0x06004985 RID: 18821 RVA: 0x00266EAE File Offset: 0x002652AE
		Private Sub OnAxisSelected(axisIndex As Integer, button As Button)
			If Not MyBase.initialized Then
				Return
			End If
			If Me.joystick Is Nothing Then
				Return
			End If
			Me.SelectAxis(axisIndex)
			Me.RefreshControls()
			Me.Redraw()
		End Sub

		' Token: 0x06004986 RID: 18822 RVA: 0x00266EDB File Offset: 0x002652DB
		Private Sub UpdateDisplay()
			Me.RedrawValueMarkers()
		End Sub

		' Token: 0x06004987 RID: 18823 RVA: 0x00266EE3 File Offset: 0x002652E3
		Private Sub Redraw()
			Me.RedrawCalibratedZero()
			Me.RedrawValueMarkers()
		End Sub

		' Token: 0x06004988 RID: 18824 RVA: 0x00266EF4 File Offset: 0x002652F4
		Private Sub RefreshControls()
			If Not Me.axisSelected Then
				Me.deadzoneSlider.value = 0F
				Me.zeroSlider.value = 0F
				Me.sensitivitySlider.value = 0F
				Me.invertToggle.isOn = False
			Else
				Me.deadzoneSlider.value = Me.axisCalibration.deadZone
				Me.zeroSlider.value = Me.axisCalibration.calibratedZero
				Me.sensitivitySlider.value = Me.axisCalibration.sensitivity
				Me.invertToggle.isOn = Me.axisCalibration.invert
			End If
		End Sub

		' Token: 0x06004989 RID: 18825 RVA: 0x00266FA8 File Offset: 0x002653A8
		Private Sub RedrawDeadzone()
			If Not Me.axisSelected Then
				Return
			End If
			Dim num As Single = Me.displayAreaWidth * Me.axisCalibration.deadZone
			Me.deadzoneArea.sizeDelta = New Vector2(num, Me.deadzoneArea.sizeDelta.y)
			Me.deadzoneArea.anchoredPosition = New Vector2(Me.axisCalibration.calibratedZero * -Me.deadzoneArea.parent.localPosition.x, Me.deadzoneArea.anchoredPosition.y)
		End Sub

		' Token: 0x0600498A RID: 18826 RVA: 0x00267040 File Offset: 0x00265440
		Private Sub RedrawCalibratedZero()
			If Not Me.axisSelected Then
				Return
			End If
			Me.calibratedZeroMarker.anchoredPosition = New Vector2(Me.axisCalibration.calibratedZero * -Me.deadzoneArea.parent.localPosition.x, Me.calibratedZeroMarker.anchoredPosition.y)
			Me.RedrawDeadzone()
		End Sub

		' Token: 0x0600498B RID: 18827 RVA: 0x002670A8 File Offset: 0x002654A8
		Private Sub RedrawValueMarkers()
			If Not Me.axisSelected Then
				Me.calibratedValueMarker.anchoredPosition = New Vector2(0F, Me.calibratedValueMarker.anchoredPosition.y)
				Me.rawValueMarker.anchoredPosition = New Vector2(0F, Me.rawValueMarker.anchoredPosition.y)
				Return
			End If
			Dim axis As Single = Me.joystick.GetAxis(Me.selectedAxis)
			Dim num As Single = Mathf.Clamp(Me.joystick.GetAxisRaw(Me.selectedAxis), -1F, 1F)
			Me.calibratedValueMarker.anchoredPosition = New Vector2(Me.displayAreaWidth * 0.5F * axis, Me.calibratedValueMarker.anchoredPosition.y)
			Me.rawValueMarker.anchoredPosition = New Vector2(Me.displayAreaWidth * 0.5F * num, Me.rawValueMarker.anchoredPosition.y)
		End Sub

		' Token: 0x0600498C RID: 18828 RVA: 0x002671A8 File Offset: 0x002655A8
		Private Sub SelectAxis(index As Integer)
			If index < 0 OrElse index >= Me.axisButtons.Count Then
				Return
			End If
			If Me.axisButtons(index) Is Nothing Then
				Return
			End If
			Me.axisButtons(index).interactable = False
			Me.axisButtons(index).[Select]()
			For i As Integer = 0 To Me.axisButtons.Count - 1
				If i <> index Then
					Me.axisButtons(i).interactable = True
				End If
			Next
			Me.selectedAxis = index
			Me.origSelectedAxisCalibrationData = Me.axisCalibration.GetData()
			Me.SetMinSensitivity()
		End Sub

		' Token: 0x0600498D RID: 18829 RVA: 0x00267261 File Offset: 0x00265661
		Public Overrides Sub TakeInputFocus()
			MyBase.TakeInputFocus()
			If Me.selectedAxis >= 0 Then
				Me.SelectAxis(Me.selectedAxis)
			End If
			Me.RefreshControls()
			Me.Redraw()
		End Sub

		' Token: 0x0600498E RID: 18830 RVA: 0x00267290 File Offset: 0x00265690
		Private Sub SetMinSensitivity()
			If Not Me.axisSelected Then
				Return
			End If
			Me.minSensitivity = 0.1F
			If Me.rewiredStandaloneInputModule IsNot Nothing Then
				If Me.IsMenuAxis(Me.menuHorizActionId, Me.selectedAxis) Then
					Me.GetAxisButtonDeadZone(Me.playerId, Me.menuHorizActionId, Me.minSensitivity)
				ElseIf Me.IsMenuAxis(Me.menuVertActionId, Me.selectedAxis) Then
					Me.GetAxisButtonDeadZone(Me.playerId, Me.menuVertActionId, Me.minSensitivity)
				End If
			End If
		End Sub

		' Token: 0x0600498F RID: 18831 RVA: 0x00267328 File Offset: 0x00265728
		Private Function IsMenuAxis(actionId As Integer, axisIndex As Integer) As Boolean
			If Me.rewiredStandaloneInputModule Is Nothing Then
				Return False
			End If
			Dim allPlayers As IList(Of Player) = ReInput.players.AllPlayers
			Dim count As Integer = allPlayers.Count
			For i As Integer = 0 To count - 1
				Dim maps As IList(Of JoystickMap) = allPlayers(i).controllers.maps.GetMaps(Of JoystickMap)(Me.joystick.id)
				If maps IsNot Nothing Then
					Dim count2 As Integer = maps.Count
					For j As Integer = 0 To count2 - 1
						Dim axisMaps As IList(Of ActionElementMap) = maps(j).AxisMaps
						If axisMaps IsNot Nothing Then
							Dim count3 As Integer = axisMaps.Count
							For k As Integer = 0 To count3 - 1
								Dim actionElementMap As ActionElementMap = axisMaps(k)
								If actionElementMap.actionId = actionId AndAlso actionElementMap.elementIndex = axisIndex Then
									Return True
								End If
							Next
						End If
					Next
				End If
			Next
			Return False
		End Function

		' Token: 0x06004990 RID: 18832 RVA: 0x0026741C File Offset: 0x0026581C
		Private Sub GetAxisButtonDeadZone(playerId As Integer, actionId As Integer, ByRef value As Single)
			Dim action As InputAction = ReInput.mapping.GetAction(actionId)
			If action Is Nothing Then
				Return
			End If
			Dim behaviorId As Integer = action.behaviorId
			Dim inputBehavior As InputBehavior = ReInput.mapping.GetInputBehavior(playerId, behaviorId)
			If inputBehavior Is Nothing Then
				Return
			End If
			value = inputBehavior.buttonDeadZone + 0.1F
		End Sub

		' Token: 0x04004F83 RID: 20355
		Private Const minSensitivityOtherAxes As Single = 0.1F

		' Token: 0x04004F84 RID: 20356
		Private Const maxDeadzone As Single = 0.8F

		' Token: 0x04004F85 RID: 20357
		<SerializeField()>
		Private rightContentContainer As RectTransform

		' Token: 0x04004F86 RID: 20358
		<SerializeField()>
		Private valueDisplayGroup As RectTransform

		' Token: 0x04004F87 RID: 20359
		<SerializeField()>
		Private calibratedValueMarker As RectTransform

		' Token: 0x04004F88 RID: 20360
		<SerializeField()>
		Private rawValueMarker As RectTransform

		' Token: 0x04004F89 RID: 20361
		<SerializeField()>
		Private calibratedZeroMarker As RectTransform

		' Token: 0x04004F8A RID: 20362
		<SerializeField()>
		Private deadzoneArea As RectTransform

		' Token: 0x04004F8B RID: 20363
		<SerializeField()>
		Private deadzoneSlider As Slider

		' Token: 0x04004F8C RID: 20364
		<SerializeField()>
		Private zeroSlider As Slider

		' Token: 0x04004F8D RID: 20365
		<SerializeField()>
		Private sensitivitySlider As Slider

		' Token: 0x04004F8E RID: 20366
		<SerializeField()>
		Private invertToggle As Toggle

		' Token: 0x04004F8F RID: 20367
		<SerializeField()>
		Private axisScrollAreaContent As RectTransform

		' Token: 0x04004F90 RID: 20368
		<SerializeField()>
		Private doneButton As Button

		' Token: 0x04004F91 RID: 20369
		<SerializeField()>
		Private calibrateButton As Button

		' Token: 0x04004F92 RID: 20370
		<SerializeField()>
		Private doneButtonLabel As Text

		' Token: 0x04004F93 RID: 20371
		<SerializeField()>
		Private cancelButtonLabel As Text

		' Token: 0x04004F94 RID: 20372
		<SerializeField()>
		Private defaultButtonLabel As Text

		' Token: 0x04004F95 RID: 20373
		<SerializeField()>
		Private deadzoneSliderLabel As Text

		' Token: 0x04004F96 RID: 20374
		<SerializeField()>
		Private zeroSliderLabel As Text

		' Token: 0x04004F97 RID: 20375
		<SerializeField()>
		Private sensitivitySliderLabel As Text

		' Token: 0x04004F98 RID: 20376
		<SerializeField()>
		Private invertToggleLabel As Text

		' Token: 0x04004F99 RID: 20377
		<SerializeField()>
		Private calibrateButtonLabel As Text

		' Token: 0x04004F9A RID: 20378
		<SerializeField()>
		Private axisButtonPrefab As GameObject

		' Token: 0x04004F9B RID: 20379
		Private joystick As Joystick

		' Token: 0x04004F9C RID: 20380
		Private origCalibrationData As String

		' Token: 0x04004F9D RID: 20381
		Private selectedAxis As Integer = -1

		' Token: 0x04004F9E RID: 20382
		Private origSelectedAxisCalibrationData As AxisCalibrationData

		' Token: 0x04004F9F RID: 20383
		Private displayAreaWidth As Single

		' Token: 0x04004FA0 RID: 20384
		Private axisButtons As List(Of Button)

		' Token: 0x04004FA1 RID: 20385
		Private buttonCallbacks As Dictionary(Of Integer, Action(Of Integer))

		' Token: 0x04004FA2 RID: 20386
		Private playerId As Integer

		' Token: 0x04004FA3 RID: 20387
		Private rewiredStandaloneInputModule As RewiredStandaloneInputModule

		' Token: 0x04004FA4 RID: 20388
		Private menuHorizActionId As Integer = -1

		' Token: 0x04004FA5 RID: 20389
		Private menuVertActionId As Integer = -1

		' Token: 0x04004FA6 RID: 20390
		Private minSensitivity As Single

		' Token: 0x02000C08 RID: 3080
		Public Enum ButtonIdentifier
			' Token: 0x04004FA8 RID: 20392
			Done
			' Token: 0x04004FA9 RID: 20393
			Cancel
			' Token: 0x04004FAA RID: 20394
			[Default]
			' Token: 0x04004FAB RID: 20395
			Calibrate
		End Enum
	End Class
End Namespace
