Imports System
Imports System.Collections.Generic
Imports UnityEngine
Imports UnityEngine.UI

Namespace Rewired.UI.ControlMapper
	' Token: 0x02000C2E RID: 3118
	<AddComponentMenu("")>
	Public Class InputBehaviorWindow
		Inherits Window

		' Token: 0x06004C73 RID: 19571 RVA: 0x002732AC File Offset: 0x002716AC
		Public Overrides Sub Initialize(id As Integer, isFocusedCallback As Func(Of Integer, Boolean))
			If Me.spawnTransform Is Nothing OrElse Me.doneButton Is Nothing OrElse Me.cancelButton Is Nothing OrElse Me.defaultButton Is Nothing OrElse Me.uiControlSetPrefab Is Nothing OrElse Me.uiSliderControlPrefab Is Nothing OrElse Me.doneButtonLabel Is Nothing OrElse Me.cancelButtonLabel Is Nothing OrElse Me.defaultButtonLabel Is Nothing Then
				Global.UnityEngine.Debug.LogError("Rewired Control Mapper: All inspector values must be assigned!")
				Return
			End If
			Me.inputBehaviorInfo = New List(Of InputBehaviorWindow.InputBehaviorInfo)()
			Me.buttonCallbacks = New Dictionary(Of Integer, Action(Of Integer))()
			Me.doneButtonLabel.text = ControlMapper.GetLanguage().done
			Me.cancelButtonLabel.text = ControlMapper.GetLanguage().cancel
			Me.defaultButtonLabel.text = ControlMapper.GetLanguage().default_
			MyBase.Initialize(id, isFocusedCallback)
		End Sub

		' Token: 0x06004C74 RID: 19572 RVA: 0x002733BC File Offset: 0x002717BC
		Public Sub SetData(playerId As Integer, data As ControlMapper.InputBehaviorSettings())
			If Not MyBase.initialized Then
				Return
			End If
			Me.playerId = playerId
			For Each inputBehaviorSettings As ControlMapper.InputBehaviorSettings In data
				If inputBehaviorSettings IsNot Nothing AndAlso inputBehaviorSettings.isValid Then
					Dim inputBehavior As InputBehavior = Me.GetInputBehavior(inputBehaviorSettings.inputBehaviorId)
					If inputBehavior IsNot Nothing Then
						Dim uicontrolSet As UIControlSet = Me.CreateControlSet()
						Dim dictionary As Dictionary(Of Integer, InputBehaviorWindow.PropertyType) = New Dictionary(Of Integer, InputBehaviorWindow.PropertyType)()
						Dim customEntry As String = ControlMapper.GetLanguage().GetCustomEntry(inputBehaviorSettings.labelLanguageKey)
						If Not String.IsNullOrEmpty(customEntry) Then
							uicontrolSet.SetTitle(customEntry)
						Else
							uicontrolSet.SetTitle(inputBehavior.name)
						End If
						If inputBehaviorSettings.showJoystickAxisSensitivity Then
							Dim uisliderControl As UISliderControl = Me.CreateSlider(uicontrolSet, inputBehavior.id, Nothing, ControlMapper.GetLanguage().GetCustomEntry(inputBehaviorSettings.joystickAxisSensitivityLabelLanguageKey), inputBehaviorSettings.joystickAxisSensitivityIcon, inputBehaviorSettings.joystickAxisSensitivityMin, inputBehaviorSettings.joystickAxisSensitivityMax, AddressOf Me.JoystickAxisSensitivityValueChanged, AddressOf Me.JoystickAxisSensitivityCanceled)
							uisliderControl.slider.value = Mathf.Clamp(inputBehavior.joystickAxisSensitivity, inputBehaviorSettings.joystickAxisSensitivityMin, inputBehaviorSettings.joystickAxisSensitivityMax)
							dictionary.Add(uisliderControl.id, InputBehaviorWindow.PropertyType.JoystickAxisSensitivity)
						End If
						If inputBehaviorSettings.showMouseXYAxisSensitivity Then
							Dim uisliderControl2 As UISliderControl = Me.CreateSlider(uicontrolSet, inputBehavior.id, Nothing, ControlMapper.GetLanguage().GetCustomEntry(inputBehaviorSettings.mouseXYAxisSensitivityLabelLanguageKey), inputBehaviorSettings.mouseXYAxisSensitivityIcon, inputBehaviorSettings.mouseXYAxisSensitivityMin, inputBehaviorSettings.mouseXYAxisSensitivityMax, AddressOf Me.MouseXYAxisSensitivityValueChanged, AddressOf Me.MouseXYAxisSensitivityCanceled)
							uisliderControl2.slider.value = Mathf.Clamp(inputBehavior.mouseXYAxisSensitivity, inputBehaviorSettings.mouseXYAxisSensitivityMin, inputBehaviorSettings.mouseXYAxisSensitivityMax)
							dictionary.Add(uisliderControl2.id, InputBehaviorWindow.PropertyType.MouseXYAxisSensitivity)
						End If
						Me.inputBehaviorInfo.Add(New InputBehaviorWindow.InputBehaviorInfo(inputBehavior, uicontrolSet, dictionary))
					End If
				End If
			Next
			MyBase.defaultUIElement = Me.doneButton.gameObject
		End Sub

		' Token: 0x06004C75 RID: 19573 RVA: 0x0027359C File Offset: 0x0027199C
		Public Sub SetButtonCallback(buttonIdentifier As InputBehaviorWindow.ButtonIdentifier, callback As Action(Of Integer))
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

		' Token: 0x06004C76 RID: 19574 RVA: 0x002735EC File Offset: 0x002719EC
		Public Overrides Sub Cancel()
			If Not MyBase.initialized Then
				Return
			End If
			For Each inputBehaviorInfo As InputBehaviorWindow.InputBehaviorInfo In Me.inputBehaviorInfo
				inputBehaviorInfo.RestorePreviousData()
			Next
			Dim action As Action(Of Integer)
			If Not Me.buttonCallbacks.TryGetValue(1, action) Then
				If Me.cancelCallback IsNot Nothing Then
					Me.cancelCallback()
				End If
				Return
			End If
			action(MyBase.id)
		End Sub

		' Token: 0x06004C77 RID: 19575 RVA: 0x0027368C File Offset: 0x00271A8C
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

		' Token: 0x06004C78 RID: 19576 RVA: 0x002736C5 File Offset: 0x00271AC5
		Public Sub OnCancel()
			Me.Cancel()
		End Sub

		' Token: 0x06004C79 RID: 19577 RVA: 0x002736D0 File Offset: 0x00271AD0
		Public Sub OnRestoreDefault()
			If Not MyBase.initialized Then
				Return
			End If
			For Each inputBehaviorInfo As InputBehaviorWindow.InputBehaviorInfo In Me.inputBehaviorInfo
				inputBehaviorInfo.RestoreDefaultData()
			Next
		End Sub

		' Token: 0x06004C7A RID: 19578 RVA: 0x00273738 File Offset: 0x00271B38
		Private Sub JoystickAxisSensitivityValueChanged(inputBehaviorId As Integer, controlId As Integer, value As Single)
			Me.GetInputBehavior(inputBehaviorId).joystickAxisSensitivity = value
		End Sub

		' Token: 0x06004C7B RID: 19579 RVA: 0x00273747 File Offset: 0x00271B47
		Private Sub MouseXYAxisSensitivityValueChanged(inputBehaviorId As Integer, controlId As Integer, value As Single)
			Me.GetInputBehavior(inputBehaviorId).mouseXYAxisSensitivity = value
		End Sub

		' Token: 0x06004C7C RID: 19580 RVA: 0x00273758 File Offset: 0x00271B58
		Private Sub JoystickAxisSensitivityCanceled(inputBehaviorId As Integer, controlId As Integer)
			Dim inputBehaviorInfo As InputBehaviorWindow.InputBehaviorInfo = Me.GetInputBehaviorInfo(inputBehaviorId)
			If inputBehaviorInfo Is Nothing Then
				Return
			End If
			inputBehaviorInfo.RestoreData(InputBehaviorWindow.PropertyType.JoystickAxisSensitivity, controlId)
		End Sub

		' Token: 0x06004C7D RID: 19581 RVA: 0x0027377C File Offset: 0x00271B7C
		Private Sub MouseXYAxisSensitivityCanceled(inputBehaviorId As Integer, controlId As Integer)
			Dim inputBehaviorInfo As InputBehaviorWindow.InputBehaviorInfo = Me.GetInputBehaviorInfo(inputBehaviorId)
			If inputBehaviorInfo Is Nothing Then
				Return
			End If
			inputBehaviorInfo.RestoreData(InputBehaviorWindow.PropertyType.MouseXYAxisSensitivity, controlId)
		End Sub

		' Token: 0x06004C7E RID: 19582 RVA: 0x002737A0 File Offset: 0x00271BA0
		Public Overrides Sub TakeInputFocus()
			MyBase.TakeInputFocus()
		End Sub

		' Token: 0x06004C7F RID: 19583 RVA: 0x002737A8 File Offset: 0x00271BA8
		Private Function CreateControlSet() As UIControlSet
			Dim gameObject As GameObject = Global.UnityEngine.[Object].Instantiate(Of GameObject)(Me.uiControlSetPrefab)
			gameObject.transform.SetParent(Me.spawnTransform, False)
			Return gameObject.GetComponent(Of UIControlSet)()
		End Function

		' Token: 0x06004C80 RID: 19584 RVA: 0x002737DC File Offset: 0x00271BDC
		Private Function CreateSlider([set] As UIControlSet, inputBehaviorId As Integer, defaultTitle As String, overrideTitle As String, icon As Sprite, minValue As Single, maxValue As Single, valueChangedCallback As Action(Of Integer, Integer, Single), cancelCallback As Action(Of Integer, Integer)) As UISliderControl
			Dim uisliderControl As UISliderControl = [set].CreateSlider(Me.uiSliderControlPrefab, icon, minValue, maxValue, Sub(cId As Integer, value As Single)
				valueChangedCallback(inputBehaviorId, cId, value)
			End Sub, Sub(cId As Integer)
				cancelCallback(inputBehaviorId, cId)
			End Sub)
			Dim text As String = If((Not String.IsNullOrEmpty(overrideTitle)), overrideTitle, defaultTitle)
			If Not String.IsNullOrEmpty(text) Then
				uisliderControl.showTitle = True
				uisliderControl.title.text = text
			Else
				uisliderControl.showTitle = False
			End If
			uisliderControl.showIcon = icon IsNot Nothing
			Return uisliderControl
		End Function

		' Token: 0x06004C81 RID: 19585 RVA: 0x0027387F File Offset: 0x00271C7F
		Private Function GetInputBehavior(id As Integer) As InputBehavior
			Return ReInput.mapping.GetInputBehavior(Me.playerId, id)
		End Function

		' Token: 0x06004C82 RID: 19586 RVA: 0x00273894 File Offset: 0x00271C94
		Private Function GetInputBehaviorInfo(inputBehaviorId As Integer) As InputBehaviorWindow.InputBehaviorInfo
			Dim count As Integer = Me.inputBehaviorInfo.Count
			For i As Integer = 0 To count - 1
				If Me.inputBehaviorInfo(i).inputBehavior.id = inputBehaviorId Then
					Return Me.inputBehaviorInfo(i)
				End If
			Next
			Return Nothing
		End Function

		' Token: 0x040050D3 RID: 20691
		Private Const minSensitivity As Single = 0.1F

		' Token: 0x040050D4 RID: 20692
		<SerializeField()>
		Private spawnTransform As RectTransform

		' Token: 0x040050D5 RID: 20693
		<SerializeField()>
		Private doneButton As Button

		' Token: 0x040050D6 RID: 20694
		<SerializeField()>
		Private cancelButton As Button

		' Token: 0x040050D7 RID: 20695
		<SerializeField()>
		Private defaultButton As Button

		' Token: 0x040050D8 RID: 20696
		<SerializeField()>
		Private doneButtonLabel As Text

		' Token: 0x040050D9 RID: 20697
		<SerializeField()>
		Private cancelButtonLabel As Text

		' Token: 0x040050DA RID: 20698
		<SerializeField()>
		Private defaultButtonLabel As Text

		' Token: 0x040050DB RID: 20699
		<SerializeField()>
		Private uiControlSetPrefab As GameObject

		' Token: 0x040050DC RID: 20700
		<SerializeField()>
		Private uiSliderControlPrefab As GameObject

		' Token: 0x040050DD RID: 20701
		Private inputBehaviorInfo As List(Of InputBehaviorWindow.InputBehaviorInfo)

		' Token: 0x040050DE RID: 20702
		Private buttonCallbacks As Dictionary(Of Integer, Action(Of Integer))

		' Token: 0x040050DF RID: 20703
		Private playerId As Integer

		' Token: 0x02000C2F RID: 3119
		Private Class InputBehaviorInfo
			' Token: 0x06004C83 RID: 19587 RVA: 0x002738EE File Offset: 0x00271CEE
			Public Sub New(inputBehavior As InputBehavior, controlSet As UIControlSet, idToProperty As Dictionary(Of Integer, InputBehaviorWindow.PropertyType))
				Me._inputBehavior = inputBehavior
				Me._controlSet = controlSet
				Me.idToProperty = idToProperty
				Me.copyOfOriginal = New InputBehavior(inputBehavior)
			End Sub

			' Token: 0x1700075E RID: 1886
			' (get) Token: 0x06004C84 RID: 19588 RVA: 0x00273917 File Offset: 0x00271D17
			Public ReadOnly Property inputBehavior As InputBehavior
				Get
					Return Me._inputBehavior
				End Get
			End Property

			' Token: 0x1700075F RID: 1887
			' (get) Token: 0x06004C85 RID: 19589 RVA: 0x0027391F File Offset: 0x00271D1F
			Public ReadOnly Property controlSet As UIControlSet
				Get
					Return Me._controlSet
				End Get
			End Property

			' Token: 0x06004C86 RID: 19590 RVA: 0x00273927 File Offset: 0x00271D27
			Public Sub RestorePreviousData()
				Me._inputBehavior.ImportData(Me.copyOfOriginal)
			End Sub

			' Token: 0x06004C87 RID: 19591 RVA: 0x0027393B File Offset: 0x00271D3B
			Public Sub RestoreDefaultData()
				Me._inputBehavior.Reset()
				Me.RefreshControls()
			End Sub

			' Token: 0x06004C88 RID: 19592 RVA: 0x00273950 File Offset: 0x00271D50
			Public Sub RestoreData(propertyType As InputBehaviorWindow.PropertyType, controlId As Integer)
				If propertyType <> InputBehaviorWindow.PropertyType.JoystickAxisSensitivity Then
					If propertyType = InputBehaviorWindow.PropertyType.MouseXYAxisSensitivity Then
						Dim mouseXYAxisSensitivity As Single = Me.copyOfOriginal.mouseXYAxisSensitivity
						Me._inputBehavior.mouseXYAxisSensitivity = mouseXYAxisSensitivity
						Dim control As UISliderControl = Me._controlSet.GetControl(Of UISliderControl)(controlId)
						If control IsNot Nothing Then
							control.slider.value = mouseXYAxisSensitivity
						End If
					End If
				Else
					Dim joystickAxisSensitivity As Single = Me.copyOfOriginal.joystickAxisSensitivity
					Me._inputBehavior.joystickAxisSensitivity = joystickAxisSensitivity
					Dim control2 As UISliderControl = Me._controlSet.GetControl(Of UISliderControl)(controlId)
					If control2 IsNot Nothing Then
						control2.slider.value = joystickAxisSensitivity
					End If
				End If
			End Sub

			' Token: 0x06004C89 RID: 19593 RVA: 0x002739F4 File Offset: 0x00271DF4
			Public Sub RefreshControls()
				If Me._controlSet Is Nothing Then
					Return
				End If
				If Me.idToProperty Is Nothing Then
					Return
				End If
				For Each keyValuePair As KeyValuePair(Of Integer, InputBehaviorWindow.PropertyType) In Me.idToProperty
					Dim control As UISliderControl = Me._controlSet.GetControl(Of UISliderControl)(keyValuePair.Key)
					If Not(control Is Nothing) Then
						Dim value As InputBehaviorWindow.PropertyType = keyValuePair.Value
						If value <> InputBehaviorWindow.PropertyType.JoystickAxisSensitivity Then
							If value = InputBehaviorWindow.PropertyType.MouseXYAxisSensitivity Then
								control.slider.value = Me._inputBehavior.mouseXYAxisSensitivity
							End If
						Else
							control.slider.value = Me._inputBehavior.joystickAxisSensitivity
						End If
					End If
				Next
			End Sub

			' Token: 0x040050E0 RID: 20704
			Private _inputBehavior As InputBehavior

			' Token: 0x040050E1 RID: 20705
			Private _controlSet As UIControlSet

			' Token: 0x040050E2 RID: 20706
			Private idToProperty As Dictionary(Of Integer, InputBehaviorWindow.PropertyType)

			' Token: 0x040050E3 RID: 20707
			Private copyOfOriginal As InputBehavior
		End Class

		' Token: 0x02000C30 RID: 3120
		Public Enum ButtonIdentifier
			' Token: 0x040050E5 RID: 20709
			Done
			' Token: 0x040050E6 RID: 20710
			Cancel
			' Token: 0x040050E7 RID: 20711
			[Default]
		End Enum

		' Token: 0x02000C31 RID: 3121
		Private Enum PropertyType
			' Token: 0x040050E9 RID: 20713
			JoystickAxisSensitivity
			' Token: 0x040050EA RID: 20714
			MouseXYAxisSensitivity
		End Enum
	End Class
End Namespace
