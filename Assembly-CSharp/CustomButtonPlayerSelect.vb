Imports System
Imports System.Collections
Imports Rewired.UI.ControlMapper
Imports UnityEngine
Imports UnityEngine.EventSystems
Imports UnityEngine.UI

' Token: 0x02000C2A RID: 3114
Public Class CustomButtonPlayerSelect
	Inherits CustomButton

	' Token: 0x06004C13 RID: 19475 RVA: 0x00272250 File Offset: 0x00270650
	Protected Overrides Sub Start()
		MyBase.Start()
		If Not PlayerManager.Multiplayer Then
			MyBase.interactable = False
		End If
		MyBase.StartCoroutine(Me.update_cr())
	End Sub

	' Token: 0x06004C14 RID: 19476 RVA: 0x00272278 File Offset: 0x00270678
	Private Iterator Function update_cr() As IEnumerator
		While Not Me.mapper
			Yield Nothing
		End While
		For i As Integer = 0 To Me.selectionTabs.Length - 1
			Me.selectionTabs(i).rectTransform.anchoredPosition = New Vector3((Me.associatedText.preferredWidth / 2F + 15F) * CSng((i * 2 - 1)), Me.selectionTabs(i).rectTransform.anchoredPosition.y, 0F)
		Next
		While True
			If Me.myInfo.intData = Me.mapper.currentPlayerId Then
				Me.associatedText.color = MyBase.colors.highlightedColor
			Else
				Me.associatedText.color = If((MyBase.currentSelectionState <> Selectable.SelectionState.Highlighted), MyBase.colors.normalColor, MyBase.colors.highlightedColor)
			End If
			For j As Integer = 0 To Me.selectionTabs.Length - 1
				Me.selectionTabs(j).enabled = Me.myInfo.intData = Me.mapper.currentPlayerId
			Next
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06004C15 RID: 19477 RVA: 0x00272293 File Offset: 0x00270693
	Public Overrides Sub OnSelect(eventData As BaseEventData)
		If Me.IsInteractable() Then
			MyBase.OnSelect(eventData)
		Else
			MyBase.StartCoroutine(Me.move_selection_cr())
		End If
	End Sub

	' Token: 0x06004C16 RID: 19478 RVA: 0x002722BC File Offset: 0x002706BC
	Private Iterator Function move_selection_cr() As IEnumerator
		Yield New WaitForEndOfFrame()
		EventSystem.current.SetSelectedGameObject(Me.FindSelectableOnUp().gameObject)
		Return
	End Function

	' Token: 0x06004C17 RID: 19479 RVA: 0x002722D8 File Offset: 0x002706D8
	Protected Overrides Sub DoStateTransition(state As Selectable.SelectionState, instant As Boolean)
		If Me.mapper AndAlso Me.myInfo.intData = Me.mapper.currentPlayerId Then
			Return
		End If
		Select Case state
			Case Selectable.SelectionState.Normal
				Me.associatedText.color = MyBase.colors.normalColor
			Case Selectable.SelectionState.Highlighted
				Me.associatedText.color = MyBase.colors.highlightedColor
			Case Selectable.SelectionState.Pressed
				Me.associatedText.color = MyBase.colors.pressedColor
			Case Selectable.SelectionState.Disabled
				Me.associatedText.color = MyBase.colors.disabledColor
		End Select
	End Sub

	' Token: 0x040050BA RID: 20666
	Public mapper As ControlMapper

	' Token: 0x040050BB RID: 20667
	<SerializeField()>
	Private myInfo As ButtonInfo

	' Token: 0x040050BC RID: 20668
	<SerializeField()>
	Private selectionTabs As Image()
End Class
