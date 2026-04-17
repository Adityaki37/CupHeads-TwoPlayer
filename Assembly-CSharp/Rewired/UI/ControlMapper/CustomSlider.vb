Imports System
Imports System.Diagnostics
Imports UnityEngine
Imports UnityEngine.Events
Imports UnityEngine.EventSystems
Imports UnityEngine.UI

Namespace Rewired.UI.ControlMapper
	' Token: 0x02000C2B RID: 3115
	<AddComponentMenu("")>
	Public Class CustomSlider
		Inherits Slider
		Implements ICustomSelectable, ICancelHandler, IEventSystemHandler

		' Token: 0x17000747 RID: 1863
		' (get) Token: 0x06004C19 RID: 19481 RVA: 0x002726A4 File Offset: 0x00270AA4
		' (set) Token: 0x06004C1A RID: 19482 RVA: 0x002726AC File Offset: 0x00270AAC
		Public Property disabledHighlightedSprite As Sprite Implements Rewired.UI.ControlMapper.ICustomSelectable.disabledHighlightedSprite
			Get
				Return Me._disabledHighlightedSprite
			End Get
			Set(value As Sprite)
				Me._disabledHighlightedSprite = value
			End Set
		End Property

		' Token: 0x17000748 RID: 1864
		' (get) Token: 0x06004C1B RID: 19483 RVA: 0x002726B5 File Offset: 0x00270AB5
		' (set) Token: 0x06004C1C RID: 19484 RVA: 0x002726BD File Offset: 0x00270ABD
		Public Property disabledHighlightedColor As Color Implements Rewired.UI.ControlMapper.ICustomSelectable.disabledHighlightedColor
			Get
				Return Me._disabledHighlightedColor
			End Get
			Set(value As Color)
				Me._disabledHighlightedColor = value
			End Set
		End Property

		' Token: 0x17000749 RID: 1865
		' (get) Token: 0x06004C1D RID: 19485 RVA: 0x002726C6 File Offset: 0x00270AC6
		' (set) Token: 0x06004C1E RID: 19486 RVA: 0x002726CE File Offset: 0x00270ACE
		Public Property disabledHighlightedTrigger As String Implements Rewired.UI.ControlMapper.ICustomSelectable.disabledHighlightedTrigger
			Get
				Return Me._disabledHighlightedTrigger
			End Get
			Set(value As String)
				Me._disabledHighlightedTrigger = value
			End Set
		End Property

		' Token: 0x1700074A RID: 1866
		' (get) Token: 0x06004C1F RID: 19487 RVA: 0x002726D7 File Offset: 0x00270AD7
		' (set) Token: 0x06004C20 RID: 19488 RVA: 0x002726DF File Offset: 0x00270ADF
		Public Property autoNavUp As Boolean Implements Rewired.UI.ControlMapper.ICustomSelectable.autoNavUp
			Get
				Return Me._autoNavUp
			End Get
			Set(value As Boolean)
				Me._autoNavUp = value
			End Set
		End Property

		' Token: 0x1700074B RID: 1867
		' (get) Token: 0x06004C21 RID: 19489 RVA: 0x002726E8 File Offset: 0x00270AE8
		' (set) Token: 0x06004C22 RID: 19490 RVA: 0x002726F0 File Offset: 0x00270AF0
		Public Property autoNavDown As Boolean Implements Rewired.UI.ControlMapper.ICustomSelectable.autoNavDown
			Get
				Return Me._autoNavDown
			End Get
			Set(value As Boolean)
				Me._autoNavDown = value
			End Set
		End Property

		' Token: 0x1700074C RID: 1868
		' (get) Token: 0x06004C23 RID: 19491 RVA: 0x002726F9 File Offset: 0x00270AF9
		' (set) Token: 0x06004C24 RID: 19492 RVA: 0x00272701 File Offset: 0x00270B01
		Public Property autoNavLeft As Boolean Implements Rewired.UI.ControlMapper.ICustomSelectable.autoNavLeft
			Get
				Return Me._autoNavLeft
			End Get
			Set(value As Boolean)
				Me._autoNavLeft = value
			End Set
		End Property

		' Token: 0x1700074D RID: 1869
		' (get) Token: 0x06004C25 RID: 19493 RVA: 0x0027270A File Offset: 0x00270B0A
		' (set) Token: 0x06004C26 RID: 19494 RVA: 0x00272712 File Offset: 0x00270B12
		Public Property autoNavRight As Boolean Implements Rewired.UI.ControlMapper.ICustomSelectable.autoNavRight
			Get
				Return Me._autoNavRight
			End Get
			Set(value As Boolean)
				Me._autoNavRight = value
			End Set
		End Property

		' Token: 0x1700074E RID: 1870
		' (get) Token: 0x06004C27 RID: 19495 RVA: 0x0027271B File Offset: 0x00270B1B
		Private ReadOnly Property isDisabled As Boolean
			Get
				Return Not Me.IsInteractable()
			End Get
		End Property

		' Token: 0x140000F3 RID: 243
		' (add) Token: 0x06004C28 RID: 19496 RVA: 0x00272728 File Offset: 0x00270B28
		' (remove) Token: 0x06004C29 RID: 19497 RVA: 0x00272760 File Offset: 0x00270B60
		<DebuggerBrowsable(DebuggerBrowsableState.Never)>
		Private Event _CancelEvent As UnityAction

		' Token: 0x140000F4 RID: 244
		' (add) Token: 0x06004C2A RID: 19498 RVA: 0x00272796 File Offset: 0x00270B96
		' (remove) Token: 0x06004C2B RID: 19499 RVA: 0x0027279F File Offset: 0x00270B9F
		Public Custom Event CancelEvent As UnityAction Implements Rewired.UI.ControlMapper.ICustomSelectable.CancelEvent
			AddHandler
				AddHandler Me._CancelEvent, value
			End AddHandler
			RemoveHandler
				RemoveHandler Me._CancelEvent, value
			End RemoveHandler
		End Event

		' Token: 0x06004C2C RID: 19500 RVA: 0x002727A8 File Offset: 0x00270BA8
		Public Overrides Function FindSelectableOnLeft() As Selectable
			If(MyBase.navigation.mode And Navigation.Mode.Horizontal) <> Navigation.Mode.None OrElse Me._autoNavLeft Then
				Return UISelectionUtility.FindNextSelectable(Me, MyBase.transform, Selectable.allSelectables, MyBase.transform.rotation * Vector3.left)
			End If
			Return MyBase.FindSelectableOnLeft()
		End Function

		' Token: 0x06004C2D RID: 19501 RVA: 0x00272804 File Offset: 0x00270C04
		Public Overrides Function FindSelectableOnRight() As Selectable
			If(MyBase.navigation.mode And Navigation.Mode.Horizontal) <> Navigation.Mode.None OrElse Me._autoNavRight Then
				Return UISelectionUtility.FindNextSelectable(Me, MyBase.transform, Selectable.allSelectables, MyBase.transform.rotation * Vector3.right)
			End If
			Return MyBase.FindSelectableOnRight()
		End Function

		' Token: 0x06004C2E RID: 19502 RVA: 0x00272860 File Offset: 0x00270C60
		Public Overrides Function FindSelectableOnUp() As Selectable
			If(MyBase.navigation.mode And Navigation.Mode.Vertical) <> Navigation.Mode.None OrElse Me._autoNavUp Then
				Return UISelectionUtility.FindNextSelectable(Me, MyBase.transform, Selectable.allSelectables, MyBase.transform.rotation * Vector3.up)
			End If
			Return MyBase.FindSelectableOnUp()
		End Function

		' Token: 0x06004C2F RID: 19503 RVA: 0x002728BC File Offset: 0x00270CBC
		Public Overrides Function FindSelectableOnDown() As Selectable
			If(MyBase.navigation.mode And Navigation.Mode.Vertical) <> Navigation.Mode.None OrElse Me._autoNavDown Then
				Return UISelectionUtility.FindNextSelectable(Me, MyBase.transform, Selectable.allSelectables, MyBase.transform.rotation * Vector3.down)
			End If
			Return MyBase.FindSelectableOnDown()
		End Function

		' Token: 0x06004C30 RID: 19504 RVA: 0x00272916 File Offset: 0x00270D16
		Protected Overrides Sub OnCanvasGroupChanged()
			MyBase.OnCanvasGroupChanged()
			If EventSystem.current Is Nothing Then
				Return
			End If
			Me.EvaluateHightlightDisabled(EventSystem.current.currentSelectedGameObject Is MyBase.gameObject)
		End Sub

		' Token: 0x06004C31 RID: 19505 RVA: 0x0027294C File Offset: 0x00270D4C
		Protected Overrides Sub DoStateTransition(state As Selectable.SelectionState, instant As Boolean)
			If Me.isHighlightDisabled Then
				Dim disabledHighlightedColor As Color = Me._disabledHighlightedColor
				Dim disabledHighlightedSprite As Sprite = Me._disabledHighlightedSprite
				Dim disabledHighlightedTrigger As String = Me._disabledHighlightedTrigger
				If MyBase.gameObject.activeInHierarchy Then
					Dim transition As Selectable.Transition = MyBase.transition
					If transition <> Selectable.Transition.ColorTint Then
						If transition <> Selectable.Transition.SpriteSwap Then
							If transition = Selectable.Transition.Animation Then
								Me.TriggerAnimation(disabledHighlightedTrigger)
							End If
						Else
							Me.DoSpriteSwap(disabledHighlightedSprite)
						End If
					Else
						Me.StartColorTween(disabledHighlightedColor * MyBase.colors.colorMultiplier, instant)
					End If
				End If
			Else
				MyBase.DoStateTransition(state, instant)
			End If
		End Sub

		' Token: 0x06004C32 RID: 19506 RVA: 0x002729F0 File Offset: 0x00270DF0
		Private Sub StartColorTween(targetColor As Color, instant As Boolean)
			If MyBase.targetGraphic Is Nothing Then
				Return
			End If
			MyBase.targetGraphic.CrossFadeColor(targetColor, If((Not instant), MyBase.colors.fadeDuration, 0F), True, True)
		End Sub

		' Token: 0x06004C33 RID: 19507 RVA: 0x00272A3B File Offset: 0x00270E3B
		Private Sub DoSpriteSwap(newSprite As Sprite)
			If MyBase.image Is Nothing Then
				Return
			End If
			MyBase.image.overrideSprite = newSprite
		End Sub

		' Token: 0x06004C34 RID: 19508 RVA: 0x00272A5C File Offset: 0x00270E5C
		Private Sub TriggerAnimation(triggername As String)
			If MyBase.animator Is Nothing OrElse Not MyBase.animator.enabled OrElse Not MyBase.animator.isActiveAndEnabled OrElse MyBase.animator.runtimeAnimatorController Is Nothing OrElse String.IsNullOrEmpty(triggername) Then
				Return
			End If
			MyBase.animator.ResetTrigger(Me._disabledHighlightedTrigger)
			MyBase.animator.SetTrigger(triggername)
		End Sub

		' Token: 0x06004C35 RID: 19509 RVA: 0x00272AD9 File Offset: 0x00270ED9
		Public Overrides Sub OnSelect(eventData As BaseEventData)
			MyBase.OnSelect(eventData)
			Me.EvaluateHightlightDisabled(True)
		End Sub

		' Token: 0x06004C36 RID: 19510 RVA: 0x00272AE9 File Offset: 0x00270EE9
		Public Overrides Sub OnDeselect(eventData As BaseEventData)
			MyBase.OnDeselect(eventData)
			Me.EvaluateHightlightDisabled(False)
		End Sub

		' Token: 0x06004C37 RID: 19511 RVA: 0x00272AFC File Offset: 0x00270EFC
		Private Sub EvaluateHightlightDisabled(isSelected As Boolean)
			If Not isSelected Then
				If Me.isHighlightDisabled Then
					Me.isHighlightDisabled = False
					Dim selectionState As Selectable.SelectionState = If((Not Me.isDisabled), MyBase.currentSelectionState, Selectable.SelectionState.Disabled)
					Me.DoStateTransition(selectionState, False)
				End If
			Else
				If Not Me.isDisabled Then
					Return
				End If
				Me.isHighlightDisabled = True
				Me.DoStateTransition(Selectable.SelectionState.Disabled, False)
			End If
		End Sub

		' Token: 0x06004C38 RID: 19512 RVA: 0x00272B61 File Offset: 0x00270F61
		Public Sub OnCancel(eventData As BaseEventData) Implements UnityEngine.EventSystems.ICancelHandler.OnCancel
			If Me._CancelEvent IsNot Nothing Then
				Me._CancelEvent()
			End If
		End Sub

		' Token: 0x040050BD RID: 20669
		<SerializeField()>
		Private _disabledHighlightedSprite As Sprite

		' Token: 0x040050BE RID: 20670
		<SerializeField()>
		Private _disabledHighlightedColor As Color

		' Token: 0x040050BF RID: 20671
		<SerializeField()>
		Private _disabledHighlightedTrigger As String

		' Token: 0x040050C0 RID: 20672
		<SerializeField()>
		Private _autoNavUp As Boolean = True

		' Token: 0x040050C1 RID: 20673
		<SerializeField()>
		Private _autoNavDown As Boolean = True

		' Token: 0x040050C2 RID: 20674
		<SerializeField()>
		Private _autoNavLeft As Boolean = True

		' Token: 0x040050C3 RID: 20675
		<SerializeField()>
		Private _autoNavRight As Boolean = True

		' Token: 0x040050C4 RID: 20676
		Private isHighlightDisabled As Boolean
	End Class
End Namespace
