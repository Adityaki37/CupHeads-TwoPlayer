Imports System
Imports System.Collections.Generic
Imports UnityEngine

Namespace Rewired.UI.ControlMapper
	' Token: 0x02000C34 RID: 3124
	Public Class LanguageData
		Inherits ScriptableObject

		' Token: 0x06004C9B RID: 19611 RVA: 0x00273E44 File Offset: 0x00272244
		Public Sub Initialize()
			If Me._initialized Then
				Return
			End If
			Me.customDict = LanguageData.CustomEntry.ToDictionary(Me._customEntries)
			Me._initialized = True
		End Sub

		' Token: 0x06004C9C RID: 19612 RVA: 0x00273E6C File Offset: 0x0027226C
		Public Function GetCustomEntry(key As String) As String
			If String.IsNullOrEmpty(key) Then
				Return String.Empty
			End If
			Dim text As String
			If Not Me.customDict.TryGetValue(key, text) Then
				Return String.Empty
			End If
			Return text
		End Function

		' Token: 0x06004C9D RID: 19613 RVA: 0x00273EA4 File Offset: 0x002722A4
		Public Function ContainsCustomEntryKey(key As String) As Boolean
			Return Not String.IsNullOrEmpty(key) AndAlso Me.customDict.ContainsKey(key)
		End Function

		' Token: 0x17000766 RID: 1894
		' (get) Token: 0x06004C9E RID: 19614 RVA: 0x00273EC0 File Offset: 0x002722C0
		Public ReadOnly Property yes As String
			Get
				Return Localization.Translate(Me._yes).text
			End Get
		End Property

		' Token: 0x17000767 RID: 1895
		' (get) Token: 0x06004C9F RID: 19615 RVA: 0x00273EE0 File Offset: 0x002722E0
		Public ReadOnly Property no As String
			Get
				Return Localization.Translate(Me._no).text
			End Get
		End Property

		' Token: 0x17000768 RID: 1896
		' (get) Token: 0x06004CA0 RID: 19616 RVA: 0x00273F00 File Offset: 0x00272300
		Public ReadOnly Property add As String
			Get
				Return Localization.Translate(Me._add).text
			End Get
		End Property

		' Token: 0x17000769 RID: 1897
		' (get) Token: 0x06004CA1 RID: 19617 RVA: 0x00273F20 File Offset: 0x00272320
		Public ReadOnly Property replace As String
			Get
				Return Localization.Translate(Me._replace).text
			End Get
		End Property

		' Token: 0x1700076A RID: 1898
		' (get) Token: 0x06004CA2 RID: 19618 RVA: 0x00273F40 File Offset: 0x00272340
		Public ReadOnly Property remove As String
			Get
				Return Localization.Translate(Me._remove).text
			End Get
		End Property

		' Token: 0x1700076B RID: 1899
		' (get) Token: 0x06004CA3 RID: 19619 RVA: 0x00273F60 File Offset: 0x00272360
		Public ReadOnly Property cancel As String
			Get
				Return Localization.Translate(Me._cancel).text
			End Get
		End Property

		' Token: 0x1700076C RID: 1900
		' (get) Token: 0x06004CA4 RID: 19620 RVA: 0x00273F80 File Offset: 0x00272380
		Public ReadOnly Property none As String
			Get
				Return Localization.Translate(Me._none).text
			End Get
		End Property

		' Token: 0x1700076D RID: 1901
		' (get) Token: 0x06004CA5 RID: 19621 RVA: 0x00273FA0 File Offset: 0x002723A0
		Public ReadOnly Property okay As String
			Get
				Return Localization.Translate(Me._okay).text
			End Get
		End Property

		' Token: 0x1700076E RID: 1902
		' (get) Token: 0x06004CA6 RID: 19622 RVA: 0x00273FC0 File Offset: 0x002723C0
		Public ReadOnly Property done As String
			Get
				Return Localization.Translate(Me._done).text
			End Get
		End Property

		' Token: 0x1700076F RID: 1903
		' (get) Token: 0x06004CA7 RID: 19623 RVA: 0x00273FE0 File Offset: 0x002723E0
		Public ReadOnly Property default_ As String
			Get
				Return Localization.Translate(Me._default).text
			End Get
		End Property

		' Token: 0x17000770 RID: 1904
		' (get) Token: 0x06004CA8 RID: 19624 RVA: 0x00274000 File Offset: 0x00272400
		Public ReadOnly Property assignControllerWindowTitle As String
			Get
				Return Localization.Translate(Me._assignControllerWindowTitle).text
			End Get
		End Property

		' Token: 0x17000771 RID: 1905
		' (get) Token: 0x06004CA9 RID: 19625 RVA: 0x00274020 File Offset: 0x00272420
		Public ReadOnly Property assignControllerWindowMessage As String
			Get
				Return Localization.Translate(Me._assignControllerWindowMessage).text
			End Get
		End Property

		' Token: 0x17000772 RID: 1906
		' (get) Token: 0x06004CAA RID: 19626 RVA: 0x00274040 File Offset: 0x00272440
		Public ReadOnly Property controllerAssignmentConflictWindowTitle As String
			Get
				Return Localization.Translate(Me._controllerAssignmentConflictWindowTitle).text
			End Get
		End Property

		' Token: 0x17000773 RID: 1907
		' (get) Token: 0x06004CAB RID: 19627 RVA: 0x00274060 File Offset: 0x00272460
		Public ReadOnly Property elementAssignmentPrePollingWindowMessage As String
			Get
				Return Localization.Translate(Me._elementAssignmentPrePollingWindowMessage).text
			End Get
		End Property

		' Token: 0x17000774 RID: 1908
		' (get) Token: 0x06004CAC RID: 19628 RVA: 0x00274080 File Offset: 0x00272480
		Public ReadOnly Property elementAssignmentConflictWindowMessage As String
			Get
				Return Localization.Translate(Me._elementAssignmentConflictWindowMessage).text
			End Get
		End Property

		' Token: 0x17000775 RID: 1909
		' (get) Token: 0x06004CAD RID: 19629 RVA: 0x002740A0 File Offset: 0x002724A0
		Public ReadOnly Property mouseAssignmentConflictWindowTitle As String
			Get
				Return Localization.Translate(Me._mouseAssignmentConflictWindowTitle).text
			End Get
		End Property

		' Token: 0x17000776 RID: 1910
		' (get) Token: 0x06004CAE RID: 19630 RVA: 0x002740C0 File Offset: 0x002724C0
		Public ReadOnly Property calibrateControllerWindowTitle As String
			Get
				Return Localization.Translate(Me._calibrateControllerWindowTitle).text
			End Get
		End Property

		' Token: 0x17000777 RID: 1911
		' (get) Token: 0x06004CAF RID: 19631 RVA: 0x002740E0 File Offset: 0x002724E0
		Public ReadOnly Property calibrateAxisStep1WindowTitle As String
			Get
				Return Localization.Translate(Me._calibrateAxisStep1WindowTitle).text
			End Get
		End Property

		' Token: 0x17000778 RID: 1912
		' (get) Token: 0x06004CB0 RID: 19632 RVA: 0x00274100 File Offset: 0x00272500
		Public ReadOnly Property calibrateAxisStep2WindowTitle As String
			Get
				Return Localization.Translate(Me._calibrateAxisStep2WindowTitle).text
			End Get
		End Property

		' Token: 0x17000779 RID: 1913
		' (get) Token: 0x06004CB1 RID: 19633 RVA: 0x00274120 File Offset: 0x00272520
		Public ReadOnly Property inputBehaviorSettingsWindowTitle As String
			Get
				Return Localization.Translate(Me._inputBehaviorSettingsWindowTitle).text
			End Get
		End Property

		' Token: 0x1700077A RID: 1914
		' (get) Token: 0x06004CB2 RID: 19634 RVA: 0x00274140 File Offset: 0x00272540
		Public ReadOnly Property restoreDefaultsWindowTitle As String
			Get
				Return Localization.Translate(Me._restoreDefaultsWindowTitle).text
			End Get
		End Property

		' Token: 0x1700077B RID: 1915
		' (get) Token: 0x06004CB3 RID: 19635 RVA: 0x00274160 File Offset: 0x00272560
		Public ReadOnly Property actionColumnLabel As String
			Get
				Return Localization.Translate(Me._actionColumnLabel).text
			End Get
		End Property

		' Token: 0x1700077C RID: 1916
		' (get) Token: 0x06004CB4 RID: 19636 RVA: 0x00274180 File Offset: 0x00272580
		Public ReadOnly Property keyboardColumnLabel As String
			Get
				Return Localization.Translate(Me._keyboardColumnLabel).text
			End Get
		End Property

		' Token: 0x1700077D RID: 1917
		' (get) Token: 0x06004CB5 RID: 19637 RVA: 0x002741A0 File Offset: 0x002725A0
		Public ReadOnly Property mouseColumnLabel As String
			Get
				Return Localization.Translate(Me._mouseColumnLabel).text
			End Get
		End Property

		' Token: 0x1700077E RID: 1918
		' (get) Token: 0x06004CB6 RID: 19638 RVA: 0x002741C0 File Offset: 0x002725C0
		Public ReadOnly Property controllerColumnLabel As String
			Get
				Return Localization.Translate(Me._controllerColumnLabel).text
			End Get
		End Property

		' Token: 0x1700077F RID: 1919
		' (get) Token: 0x06004CB7 RID: 19639 RVA: 0x002741E0 File Offset: 0x002725E0
		Public ReadOnly Property removeControllerButtonLabel As String
			Get
				Return Localization.Translate(Me._removeControllerButtonLabel).text
			End Get
		End Property

		' Token: 0x17000780 RID: 1920
		' (get) Token: 0x06004CB8 RID: 19640 RVA: 0x00274200 File Offset: 0x00272600
		Public ReadOnly Property calibrateControllerButtonLabel As String
			Get
				Return Localization.Translate(Me._calibrateControllerButtonLabel).text
			End Get
		End Property

		' Token: 0x17000781 RID: 1921
		' (get) Token: 0x06004CB9 RID: 19641 RVA: 0x00274220 File Offset: 0x00272620
		Public ReadOnly Property assignControllerButtonLabel As String
			Get
				Return Localization.Translate(Me._assignControllerButtonLabel).text
			End Get
		End Property

		' Token: 0x17000782 RID: 1922
		' (get) Token: 0x06004CBA RID: 19642 RVA: 0x00274240 File Offset: 0x00272640
		Public ReadOnly Property inputBehaviorSettingsButtonLabel As String
			Get
				Return Localization.Translate(Me._inputBehaviorSettingsButtonLabel).text
			End Get
		End Property

		' Token: 0x17000783 RID: 1923
		' (get) Token: 0x06004CBB RID: 19643 RVA: 0x00274260 File Offset: 0x00272660
		Public ReadOnly Property doneButtonLabel As String
			Get
				Return Localization.Translate(Me._doneButtonLabel).text
			End Get
		End Property

		' Token: 0x17000784 RID: 1924
		' (get) Token: 0x06004CBC RID: 19644 RVA: 0x00274280 File Offset: 0x00272680
		Public ReadOnly Property restoreDefaultsButtonLabel As String
			Get
				Return Localization.Translate(Me._restoreDefaultsButtonLabel).text
			End Get
		End Property

		' Token: 0x17000785 RID: 1925
		' (get) Token: 0x06004CBD RID: 19645 RVA: 0x002742A0 File Offset: 0x002726A0
		Public ReadOnly Property controllerSettingsGroupLabel As String
			Get
				Return Localization.Translate(Me._controllerSettingsGroupLabel).text
			End Get
		End Property

		' Token: 0x17000786 RID: 1926
		' (get) Token: 0x06004CBE RID: 19646 RVA: 0x002742C0 File Offset: 0x002726C0
		Public ReadOnly Property playersGroupLabel As String
			Get
				Return Localization.Translate(Me._playersGroupLabel).text
			End Get
		End Property

		' Token: 0x17000787 RID: 1927
		' (get) Token: 0x06004CBF RID: 19647 RVA: 0x002742E0 File Offset: 0x002726E0
		Public ReadOnly Property assignedControllersGroupLabel As String
			Get
				Return Localization.Translate(Me._assignedControllersGroupLabel).text
			End Get
		End Property

		' Token: 0x17000788 RID: 1928
		' (get) Token: 0x06004CC0 RID: 19648 RVA: 0x00274300 File Offset: 0x00272700
		Public ReadOnly Property settingsGroupLabel As String
			Get
				Return Localization.Translate(Me._settingsGroupLabel).text
			End Get
		End Property

		' Token: 0x17000789 RID: 1929
		' (get) Token: 0x06004CC1 RID: 19649 RVA: 0x00274320 File Offset: 0x00272720
		Public ReadOnly Property mapCategoriesGroupLabel As String
			Get
				Return Localization.Translate(Me._mapCategoriesGroupLabel).text
			End Get
		End Property

		' Token: 0x1700078A RID: 1930
		' (get) Token: 0x06004CC2 RID: 19650 RVA: 0x00274340 File Offset: 0x00272740
		Public ReadOnly Property restoreDefaultsWindowMessage As String
			Get
				If ReInput.players.playerCount > 1 Then
					Return Localization.Translate(Me._restoreDefaultsWindowMessage_multiPlayer).text
				End If
				Return Localization.Translate(Me._restoreDefaultsWindowMessage_onePlayer).text
			End Get
		End Property

		' Token: 0x1700078B RID: 1931
		' (get) Token: 0x06004CC3 RID: 19651 RVA: 0x00274384 File Offset: 0x00272784
		Public ReadOnly Property calibrateWindow_deadZoneSliderLabel As String
			Get
				Return Localization.Translate(Me._calibrateWindow_deadZoneSliderLabel).text
			End Get
		End Property

		' Token: 0x1700078C RID: 1932
		' (get) Token: 0x06004CC4 RID: 19652 RVA: 0x002743A4 File Offset: 0x002727A4
		Public ReadOnly Property calibrateWindow_zeroSliderLabel As String
			Get
				Return Localization.Translate(Me._calibrateWindow_zeroSliderLabel).text
			End Get
		End Property

		' Token: 0x1700078D RID: 1933
		' (get) Token: 0x06004CC5 RID: 19653 RVA: 0x002743C4 File Offset: 0x002727C4
		Public ReadOnly Property calibrateWindow_sensitivitySliderLabel As String
			Get
				Return Localization.Translate(Me._calibrateWindow_sensitivitySliderLabel).text
			End Get
		End Property

		' Token: 0x1700078E RID: 1934
		' (get) Token: 0x06004CC6 RID: 19654 RVA: 0x002743E4 File Offset: 0x002727E4
		Public ReadOnly Property calibrateWindow_invertToggleLabel As String
			Get
				Return Localization.Translate(Me._calibrateWindow_invertToggleLabel).text
			End Get
		End Property

		' Token: 0x1700078F RID: 1935
		' (get) Token: 0x06004CC7 RID: 19655 RVA: 0x00274404 File Offset: 0x00272804
		Public ReadOnly Property calibrateWindow_calibrateButtonLabel As String
			Get
				Return Localization.Translate(Me._calibrateWindow_calibrateButtonLabel).text
			End Get
		End Property

		' Token: 0x06004CC8 RID: 19656 RVA: 0x00274424 File Offset: 0x00272824
		Public Function GetControllerAssignmentConflictWindowMessage(joystickName As String, otherPlayerName As String, currentPlayerName As String) As String
			Return String.Format(Localization.Translate(Me._controllerAssignmentConflictWindowMessage).text, joystickName, otherPlayerName, currentPlayerName)
		End Function

		' Token: 0x06004CC9 RID: 19657 RVA: 0x0027444C File Offset: 0x0027284C
		Public Function GetJoystickElementAssignmentPollingWindowMessage(actionName As String) As String
			Return String.Format(Localization.Translate(Me._joystickElementAssignmentPollingWindowMessage).text, actionName)
		End Function

		' Token: 0x06004CCA RID: 19658 RVA: 0x00274472 File Offset: 0x00272872
		Public Function GetJoystickElementAssignmentPollingWindowMessage_FullAxisFieldOnly(actionName As String) As String
			Return String.Format(Me._joystickElementAssignmentPollingWindowMessage_fullAxisFieldOnly, actionName)
		End Function

		' Token: 0x06004CCB RID: 19659 RVA: 0x00274480 File Offset: 0x00272880
		Public Function GetKeyboardElementAssignmentPollingWindowMessage(actionName As String) As String
			Return String.Format(Localization.Translate(Me._keyboardElementAssignmentPollingWindowMessage).text, actionName)
		End Function

		' Token: 0x06004CCC RID: 19660 RVA: 0x002744A8 File Offset: 0x002728A8
		Public Function GetMouseElementAssignmentPollingWindowMessage(actionName As String) As String
			Return String.Format(Localization.Translate(Me._mouseElementAssignmentPollingWindowMessage).text, actionName)
		End Function

		' Token: 0x06004CCD RID: 19661 RVA: 0x002744CE File Offset: 0x002728CE
		Public Function GetMouseElementAssignmentPollingWindowMessage_FullAxisFieldOnly(actionName As String) As String
			Return String.Format(Me._mouseElementAssignmentPollingWindowMessage_fullAxisFieldOnly, actionName)
		End Function

		' Token: 0x06004CCE RID: 19662 RVA: 0x002744DC File Offset: 0x002728DC
		Public Function GetElementAlreadyInUseBlocked(elementName As String) As String
			Return String.Format(Localization.Translate(Me._elementAlreadyInUseBlocked).text, elementName)
		End Function

		' Token: 0x06004CCF RID: 19663 RVA: 0x00274504 File Offset: 0x00272904
		Public Function GetElementAlreadyInUseCanReplace(elementName As String, allowConflicts As Boolean) As String
			If Not allowConflicts Then
				Return String.Format(Localization.Translate(Me._elementAlreadyInUseCanReplace).text, elementName)
			End If
			Return String.Format(Localization.Translate(Me._elementAlreadyInUseCanReplace_conflictAllowed).text, elementName)
		End Function

		' Token: 0x06004CD0 RID: 19664 RVA: 0x0027454C File Offset: 0x0027294C
		Public Function GetElementAlreadyInUseCanReplaceFontSize(allowConflicts As Boolean) As Integer
			If Not allowConflicts Then
				Return Localization.Translate(Me._elementAlreadyInUseCanReplace).fonts.fontSize
			End If
			Return Localization.Translate(Me._elementAlreadyInUseCanReplace_conflictAllowed).fonts.fontSize
		End Function

		' Token: 0x06004CD1 RID: 19665 RVA: 0x00274590 File Offset: 0x00272990
		Public Function GetMouseAssignmentConflictWindowMessage(otherPlayerName As String, thisPlayerName As String) As String
			Return String.Format(Localization.Translate(Me._mouseAssignmentConflictWindowMessage).text, otherPlayerName, thisPlayerName)
		End Function

		' Token: 0x06004CD2 RID: 19666 RVA: 0x002745B8 File Offset: 0x002729B8
		Public Function GetCalibrateAxisStep1WindowMessage(axisName As String) As String
			Return String.Format(Localization.Translate(Me._calibrateAxisStep1WindowMessage).text, axisName)
		End Function

		' Token: 0x06004CD3 RID: 19667 RVA: 0x002745E0 File Offset: 0x002729E0
		Public Function GetCalibrateAxisStep2WindowMessage(axisName As String) As String
			Return String.Format(Localization.Translate(Me._calibrateAxisStep2WindowMessage).text, axisName)
		End Function

		' Token: 0x040050F4 RID: 20724
		<SerializeField()>
		Private _yes As String = "Yes"

		' Token: 0x040050F5 RID: 20725
		<SerializeField()>
		Private _no As String = "No"

		' Token: 0x040050F6 RID: 20726
		<SerializeField()>
		Private _add As String = "Add"

		' Token: 0x040050F7 RID: 20727
		<SerializeField()>
		Private _replace As String = "Replace"

		' Token: 0x040050F8 RID: 20728
		<SerializeField()>
		Private _remove As String = "Remove"

		' Token: 0x040050F9 RID: 20729
		<SerializeField()>
		Private _cancel As String = "Cancel"

		' Token: 0x040050FA RID: 20730
		<SerializeField()>
		Private _none As String = "None"

		' Token: 0x040050FB RID: 20731
		<SerializeField()>
		Private _okay As String = "Okay"

		' Token: 0x040050FC RID: 20732
		<SerializeField()>
		Private _done As String = "Done"

		' Token: 0x040050FD RID: 20733
		<SerializeField()>
		Private _default As String = "Default"

		' Token: 0x040050FE RID: 20734
		<SerializeField()>
		Private _assignControllerWindowTitle As String = "Choose Controller"

		' Token: 0x040050FF RID: 20735
		<SerializeField()>
		Private _assignControllerWindowMessage As String = "Press any button or move an axis on the controller you would like to use."

		' Token: 0x04005100 RID: 20736
		<SerializeField()>
		Private _controllerAssignmentConflictWindowTitle As String = "Controller Assignment"

		' Token: 0x04005101 RID: 20737
		<SerializeField()>
		<Tooltip("{0} = Joystick Name" & vbLf & "{1} = Other Player Name" & vbLf & "{2} = This Player Name")>
		Private _controllerAssignmentConflictWindowMessage As String = "{0} is already assigned to {1}. Do you want to assign this controller to {2} instead?"

		' Token: 0x04005102 RID: 20738
		<SerializeField()>
		Private _elementAssignmentPrePollingWindowMessage As String = "First center or zero all sticks and axes and press any button or wait for the timer to finish."

		' Token: 0x04005103 RID: 20739
		<SerializeField()>
		<Tooltip("{0} = Action Name")>
		Private _joystickElementAssignmentPollingWindowMessage As String = "Now press a button or move an axis to assign it to {0}."

		' Token: 0x04005104 RID: 20740
		<SerializeField()>
		<Tooltip("This text is only displayed when split-axis fields have been disabled and the user clicks on the full-axis field. Button/key/D-pad input cannot be assigned to a full-axis field." & vbLf & "{0} = Action Name")>
		Private _joystickElementAssignmentPollingWindowMessage_fullAxisFieldOnly As String = "Now move an axis to assign it to {0}."

		' Token: 0x04005105 RID: 20741
		<SerializeField()>
		<Tooltip("{0} = Action Name")>
		Private _keyboardElementAssignmentPollingWindowMessage As String = "Press a key to assign it to {0}. Modifier keys may also be used. To assign a modifier key alone, hold it down for 1 second."

		' Token: 0x04005106 RID: 20742
		<SerializeField()>
		<Tooltip("{0} = Action Name")>
		Private _mouseElementAssignmentPollingWindowMessage As String = "Press a mouse button or move an axis to assign it to {0}."

		' Token: 0x04005107 RID: 20743
		<SerializeField()>
		<Tooltip("This text is only displayed when split-axis fields have been disabled and the user clicks on the full-axis field. Button/key/D-pad input cannot be assigned to a full-axis field." & vbLf & "{0} = Action Name")>
		Private _mouseElementAssignmentPollingWindowMessage_fullAxisFieldOnly As String = "Move an axis to assign it to {0}."

		' Token: 0x04005108 RID: 20744
		<SerializeField()>
		Private _elementAssignmentConflictWindowMessage As String = "Assignment Conflict"

		' Token: 0x04005109 RID: 20745
		<SerializeField()>
		<Tooltip("{0} = Element Name")>
		Private _elementAlreadyInUseBlocked As String = "{0} is already in use cannot be replaced."

		' Token: 0x0400510A RID: 20746
		<SerializeField()>
		<Tooltip("{0} = Element Name")>
		Private _elementAlreadyInUseCanReplace As String = "{0} is already in use. Do you want to replace it?"

		' Token: 0x0400510B RID: 20747
		<SerializeField()>
		<Tooltip("{0} = Element Name")>
		Private _elementAlreadyInUseCanReplace_conflictAllowed As String = "{0} is already in use. Do you want to replace it? You may also choose to add the assignment anyway."

		' Token: 0x0400510C RID: 20748
		<SerializeField()>
		Private _mouseAssignmentConflictWindowTitle As String = "Mouse Assignment"

		' Token: 0x0400510D RID: 20749
		<SerializeField()>
		<Tooltip("{0} = Other Player Name" & vbLf & "{1} = This Player Name")>
		Private _mouseAssignmentConflictWindowMessage As String = "The mouse is already assigned to {0}. Do you want to assign the mouse to {1} instead?"

		' Token: 0x0400510E RID: 20750
		<SerializeField()>
		Private _calibrateControllerWindowTitle As String = "Calibrate Controller"

		' Token: 0x0400510F RID: 20751
		<SerializeField()>
		Private _calibrateAxisStep1WindowTitle As String = "Calibrate Zero"

		' Token: 0x04005110 RID: 20752
		<SerializeField()>
		<Tooltip("{0} = Axis Name")>
		Private _calibrateAxisStep1WindowMessage As String = "Center or zero {0} and press any button or wait for the timer to finish."

		' Token: 0x04005111 RID: 20753
		<SerializeField()>
		Private _calibrateAxisStep2WindowTitle As String = "Calibrate Range"

		' Token: 0x04005112 RID: 20754
		<SerializeField()>
		<Tooltip("{0} = Axis Name")>
		Private _calibrateAxisStep2WindowMessage As String = "Move {0} through its entire range then press any button or wait for the timer to finish."

		' Token: 0x04005113 RID: 20755
		<SerializeField()>
		Private _inputBehaviorSettingsWindowTitle As String = "Sensitivity Settings"

		' Token: 0x04005114 RID: 20756
		<SerializeField()>
		Private _restoreDefaultsWindowTitle As String = "Restore Defaults"

		' Token: 0x04005115 RID: 20757
		<SerializeField()>
		<Tooltip("Message for a single player game.")>
		Private _restoreDefaultsWindowMessage_onePlayer As String = "This will restore the default input configuration. Are you sure you want to do this?"

		' Token: 0x04005116 RID: 20758
		<SerializeField()>
		<Tooltip("Message for a multi-player game.")>
		Private _restoreDefaultsWindowMessage_multiPlayer As String = "This will restore the default input configuration for all players. Are you sure you want to do this?"

		' Token: 0x04005117 RID: 20759
		<SerializeField()>
		Private _actionColumnLabel As String = "Actions"

		' Token: 0x04005118 RID: 20760
		<SerializeField()>
		Private _keyboardColumnLabel As String = "Keyboard"

		' Token: 0x04005119 RID: 20761
		<SerializeField()>
		Private _mouseColumnLabel As String = "Mouse"

		' Token: 0x0400511A RID: 20762
		<SerializeField()>
		Private _controllerColumnLabel As String = "Controller"

		' Token: 0x0400511B RID: 20763
		<SerializeField()>
		Private _removeControllerButtonLabel As String = "Remove"

		' Token: 0x0400511C RID: 20764
		<SerializeField()>
		Private _calibrateControllerButtonLabel As String = "Calibrate"

		' Token: 0x0400511D RID: 20765
		<SerializeField()>
		Private _assignControllerButtonLabel As String = "Assign Controller"

		' Token: 0x0400511E RID: 20766
		<SerializeField()>
		Private _inputBehaviorSettingsButtonLabel As String = "Sensitivity"

		' Token: 0x0400511F RID: 20767
		<SerializeField()>
		Private _doneButtonLabel As String = "Done"

		' Token: 0x04005120 RID: 20768
		<SerializeField()>
		Private _restoreDefaultsButtonLabel As String = "Restore Defaults"

		' Token: 0x04005121 RID: 20769
		<SerializeField()>
		Private _playersGroupLabel As String = "Players:"

		' Token: 0x04005122 RID: 20770
		<SerializeField()>
		Private _controllerSettingsGroupLabel As String = "Controller:"

		' Token: 0x04005123 RID: 20771
		<SerializeField()>
		Private _assignedControllersGroupLabel As String = "Assigned Controllers:"

		' Token: 0x04005124 RID: 20772
		<SerializeField()>
		Private _settingsGroupLabel As String = "Settings:"

		' Token: 0x04005125 RID: 20773
		<SerializeField()>
		Private _mapCategoriesGroupLabel As String = "Categories:"

		' Token: 0x04005126 RID: 20774
		<SerializeField()>
		Private _calibrateWindow_deadZoneSliderLabel As String = "Dead Zone:"

		' Token: 0x04005127 RID: 20775
		<SerializeField()>
		Private _calibrateWindow_zeroSliderLabel As String = "Zero:"

		' Token: 0x04005128 RID: 20776
		<SerializeField()>
		Private _calibrateWindow_sensitivitySliderLabel As String = "Sensitivity:"

		' Token: 0x04005129 RID: 20777
		<SerializeField()>
		Private _calibrateWindow_invertToggleLabel As String = "Invert"

		' Token: 0x0400512A RID: 20778
		<SerializeField()>
		Private _calibrateWindow_calibrateButtonLabel As String = "Calibrate"

		' Token: 0x0400512B RID: 20779
		<SerializeField()>
		Private _customEntries As LanguageData.CustomEntry()

		' Token: 0x0400512C RID: 20780
		Private _initialized As Boolean

		' Token: 0x0400512D RID: 20781
		Private customDict As Dictionary(Of String, String)

		' Token: 0x02000C35 RID: 3125
		<Serializable()>
		Private Class CustomEntry
			' Token: 0x06004CD4 RID: 19668 RVA: 0x00274606 File Offset: 0x00272A06
			Public Sub New()
			End Sub

			' Token: 0x06004CD5 RID: 19669 RVA: 0x0027460E File Offset: 0x00272A0E
			Public Sub New(key As String, value As String)
				Me.key = key
				Me.value = value
			End Sub

			' Token: 0x06004CD6 RID: 19670 RVA: 0x00274624 File Offset: 0x00272A24
			Public Shared Function ToDictionary(array As LanguageData.CustomEntry()) As Dictionary(Of String, String)
				If array Is Nothing Then
					Return New Dictionary(Of String, String)()
				End If
				Dim dictionary As Dictionary(Of String, String) = New Dictionary(Of String, String)()
				For i As Integer = 0 To array.Length - 1
					If array(i) IsNot Nothing Then
						If Not String.IsNullOrEmpty(array(i).key) AndAlso Not String.IsNullOrEmpty(array(i).value) Then
							If dictionary.ContainsKey(array(i).key) Then
								Global.UnityEngine.Debug.LogError("Key """ + array(i).key + """ is already in dictionary!")
							Else
								dictionary.Add(array(i).key, array(i).value)
							End If
						End If
					End If
				Next
				Return dictionary
			End Function

			' Token: 0x0400512E RID: 20782
			Public key As String

			' Token: 0x0400512F RID: 20783
			Public value As String
		End Class
	End Class
End Namespace
