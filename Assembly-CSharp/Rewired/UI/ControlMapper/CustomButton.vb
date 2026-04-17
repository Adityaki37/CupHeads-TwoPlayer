Imports System
Imports System.Collections
Imports System.Diagnostics
Imports UnityEngine
Imports UnityEngine.Events
Imports UnityEngine.EventSystems
Imports UnityEngine.UI

Namespace Rewired.UI.ControlMapper
	' Token: 0x02000C28 RID: 3112
	<AddComponentMenu("")>
	Public Class CustomButton
		Inherits Button
		Implements ICustomSelectable, ICancelHandler, IEventSystemHandler

		' Token: 0x1700073F RID: 1855
		' (get) Token: 0x06004BE7 RID: 19431 RVA: 0x00271901 File Offset: 0x0026FD01
		' (set) Token: 0x06004BE8 RID: 19432 RVA: 0x00271909 File Offset: 0x0026FD09
		Public Property disabledHighlightedSprite As Sprite Implements Rewired.UI.ControlMapper.ICustomSelectable.disabledHighlightedSprite
			Get
				Return Me._disabledHighlightedSprite
			End Get
			Set(value As Sprite)
				Me._disabledHighlightedSprite = value
			End Set
		End Property

		' Token: 0x17000740 RID: 1856
		' (get) Token: 0x06004BE9 RID: 19433 RVA: 0x00271912 File Offset: 0x0026FD12
		' (set) Token: 0x06004BEA RID: 19434 RVA: 0x0027191A File Offset: 0x0026FD1A
		Public Property disabledHighlightedColor As Color Implements Rewired.UI.ControlMapper.ICustomSelectable.disabledHighlightedColor
			Get
				Return Me._disabledHighlightedColor
			End Get
			Set(value As Color)
				Me._disabledHighlightedColor = value
			End Set
		End Property

		' Token: 0x17000741 RID: 1857
		' (get) Token: 0x06004BEB RID: 19435 RVA: 0x00271923 File Offset: 0x0026FD23
		' (set) Token: 0x06004BEC RID: 19436 RVA: 0x0027192B File Offset: 0x0026FD2B
		Public Property disabledHighlightedTrigger As String Implements Rewired.UI.ControlMapper.ICustomSelectable.disabledHighlightedTrigger
			Get
				Return Me._disabledHighlightedTrigger
			End Get
			Set(value As String)
				Me._disabledHighlightedTrigger = value
			End Set
		End Property

		' Token: 0x17000742 RID: 1858
		' (get) Token: 0x06004BED RID: 19437 RVA: 0x00271934 File Offset: 0x0026FD34
		' (set) Token: 0x06004BEE RID: 19438 RVA: 0x0027193C File Offset: 0x0026FD3C
		Public Property autoNavUp As Boolean Implements Rewired.UI.ControlMapper.ICustomSelectable.autoNavUp
			Get
				Return Me._autoNavUp
			End Get
			Set(value As Boolean)
				Me._autoNavUp = value
			End Set
		End Property

		' Token: 0x17000743 RID: 1859
		' (get) Token: 0x06004BEF RID: 19439 RVA: 0x00271945 File Offset: 0x0026FD45
		' (set) Token: 0x06004BF0 RID: 19440 RVA: 0x0027194D File Offset: 0x0026FD4D
		Public Property autoNavDown As Boolean Implements Rewired.UI.ControlMapper.ICustomSelectable.autoNavDown
			Get
				Return Me._autoNavDown
			End Get
			Set(value As Boolean)
				Me._autoNavDown = value
			End Set
		End Property

		' Token: 0x17000744 RID: 1860
		' (get) Token: 0x06004BF1 RID: 19441 RVA: 0x00271956 File Offset: 0x0026FD56
		' (set) Token: 0x06004BF2 RID: 19442 RVA: 0x0027195E File Offset: 0x0026FD5E
		Public Property autoNavLeft As Boolean Implements Rewired.UI.ControlMapper.ICustomSelectable.autoNavLeft
			Get
				Return Me._autoNavLeft
			End Get
			Set(value As Boolean)
				Me._autoNavLeft = value
			End Set
		End Property

		' Token: 0x17000745 RID: 1861
		' (get) Token: 0x06004BF3 RID: 19443 RVA: 0x00271967 File Offset: 0x0026FD67
		' (set) Token: 0x06004BF4 RID: 19444 RVA: 0x0027196F File Offset: 0x0026FD6F
		Public Property autoNavRight As Boolean Implements Rewired.UI.ControlMapper.ICustomSelectable.autoNavRight
			Get
				Return Me._autoNavRight
			End Get
			Set(value As Boolean)
				Me._autoNavRight = value
			End Set
		End Property

		' Token: 0x17000746 RID: 1862
		' (get) Token: 0x06004BF5 RID: 19445 RVA: 0x00271978 File Offset: 0x0026FD78
		Private ReadOnly Property isDisabled As Boolean
			Get
				Return Not Me.IsInteractable()
			End Get
		End Property

		' Token: 0x140000F1 RID: 241
		' (add) Token: 0x06004BF6 RID: 19446 RVA: 0x00271984 File Offset: 0x0026FD84
		' (remove) Token: 0x06004BF7 RID: 19447 RVA: 0x002719BC File Offset: 0x0026FDBC
		<DebuggerBrowsable(DebuggerBrowsableState.Never)>
		Private Event _CancelEvent As UnityAction

		' Token: 0x140000F2 RID: 242
		' (add) Token: 0x06004BF8 RID: 19448 RVA: 0x002719F2 File Offset: 0x0026FDF2
		' (remove) Token: 0x06004BF9 RID: 19449 RVA: 0x002719FB File Offset: 0x0026FDFB
		Public Custom Event CancelEvent As UnityAction Implements Rewired.UI.ControlMapper.ICustomSelectable.CancelEvent
			AddHandler
				AddHandler Me._CancelEvent, value
			End AddHandler
			RemoveHandler
				RemoveHandler Me._CancelEvent, value
			End RemoveHandler
		End Event

		' Token: 0x06004BFA RID: 19450 RVA: 0x00271A04 File Offset: 0x0026FE04
		Public Overrides Function FindSelectableOnLeft() As Selectable
			If(MyBase.navigation.mode And Navigation.Mode.Horizontal) <> Navigation.Mode.None OrElse Me._autoNavLeft Then
				Return UISelectionUtility.FindNextSelectable(Me, MyBase.transform, Selectable.allSelectables, MyBase.transform.rotation * Vector3.left)
			End If
			Return MyBase.FindSelectableOnLeft()
		End Function

		' Token: 0x06004BFB RID: 19451 RVA: 0x00271A60 File Offset: 0x0026FE60
		Public Overrides Function FindSelectableOnRight() As Selectable
			If(MyBase.navigation.mode And Navigation.Mode.Horizontal) <> Navigation.Mode.None OrElse Me._autoNavRight Then
				Return UISelectionUtility.FindNextSelectable(Me, MyBase.transform, Selectable.allSelectables, MyBase.transform.rotation * Vector3.right)
			End If
			Return MyBase.FindSelectableOnRight()
		End Function

		' Token: 0x06004BFC RID: 19452 RVA: 0x00271ABC File Offset: 0x0026FEBC
		Public Overrides Function FindSelectableOnUp() As Selectable
			If(MyBase.navigation.mode And Navigation.Mode.Vertical) <> Navigation.Mode.None OrElse Me._autoNavUp Then
				Return UISelectionUtility.FindNextSelectable(Me, MyBase.transform, Selectable.allSelectables, MyBase.transform.rotation * Vector3.up)
			End If
			Return MyBase.FindSelectableOnUp()
		End Function

		' Token: 0x06004BFD RID: 19453 RVA: 0x00271B18 File Offset: 0x0026FF18
		Public Overrides Function FindSelectableOnDown() As Selectable
			If(MyBase.navigation.mode And Navigation.Mode.Vertical) <> Navigation.Mode.None OrElse Me._autoNavDown Then
				Return UISelectionUtility.FindNextSelectable(Me, MyBase.transform, Selectable.allSelectables, MyBase.transform.rotation * Vector3.down)
			End If
			Return MyBase.FindSelectableOnDown()
		End Function

		' Token: 0x06004BFE RID: 19454 RVA: 0x00271B72 File Offset: 0x0026FF72
		Protected Overrides Sub OnCanvasGroupChanged()
			MyBase.OnCanvasGroupChanged()
			If EventSystem.current Is Nothing Then
				Return
			End If
			Me.EvaluateHightlightDisabled(EventSystem.current.currentSelectedGameObject Is MyBase.gameObject)
		End Sub

		' Token: 0x06004BFF RID: 19455 RVA: 0x00271BA6 File Offset: 0x0026FFA6
		Protected Overrides Sub OnEnable()
			MyBase.OnEnable()
			MyBase.StartCoroutine(Me.update_text_color_on_enable_cr())
		End Sub

		' Token: 0x06004C00 RID: 19456 RVA: 0x00271BBC File Offset: 0x0026FFBC
		Private Iterator Function update_text_color_on_enable_cr() As IEnumerator
			Yield New WaitForEndOfFrame()
			Me.UpdateAssociatedTextColor(MyBase.currentSelectionState)
			Return
		End Function

		' Token: 0x06004C01 RID: 19457 RVA: 0x00271BD8 File Offset: 0x0026FFD8
		Protected Sub UpdateAssociatedTextColor(state As Selectable.SelectionState)
			If Me.associatedText Then
				If Me.textOverrideColor.Length = 4 Then
					Me.associatedText.color = Me.textOverrideColor(CInt(state))
				Else
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
				End If
			End If
		End Sub

		' Token: 0x06004C02 RID: 19458 RVA: 0x00271CBC File Offset: 0x002700BC
		Protected Overrides Sub DoStateTransition(state As Selectable.SelectionState, instant As Boolean)
			Me.UpdateAssociatedTextColor(state)
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
							Me.DoSpriteSwap(If((state <> Selectable.SelectionState.Highlighted), disabledHighlightedSprite, MyBase.spriteState.highlightedSprite))
						End If
					Else
						Me.StartColorTween(disabledHighlightedColor * MyBase.colors.colorMultiplier, instant)
					End If
				End If
			Else
				MyBase.DoStateTransition(state, instant)
			End If
		End Sub

		' Token: 0x06004C03 RID: 19459 RVA: 0x00271D84 File Offset: 0x00270184
		Private Sub StartColorTween(targetColor As Color, instant As Boolean)
			If MyBase.targetGraphic Is Nothing Then
				Return
			End If
			MyBase.targetGraphic.CrossFadeColor(targetColor, If((Not instant), MyBase.colors.fadeDuration, 0F), True, True)
		End Sub

		' Token: 0x06004C04 RID: 19460 RVA: 0x00271DCF File Offset: 0x002701CF
		Private Sub DoSpriteSwap(newSprite As Sprite)
			If MyBase.image Is Nothing Then
				Return
			End If
			MyBase.image.overrideSprite = newSprite
		End Sub

		' Token: 0x06004C05 RID: 19461 RVA: 0x00271DF0 File Offset: 0x002701F0
		Private Sub TriggerAnimation(triggername As String)
			If MyBase.animator Is Nothing OrElse Not MyBase.animator.enabled OrElse Not MyBase.animator.isActiveAndEnabled OrElse MyBase.animator.runtimeAnimatorController Is Nothing OrElse String.IsNullOrEmpty(triggername) Then
				Return
			End If
			MyBase.animator.ResetTrigger(Me._disabledHighlightedTrigger)
			MyBase.animator.SetTrigger(triggername)
		End Sub

		' Token: 0x06004C06 RID: 19462 RVA: 0x00271E6D File Offset: 0x0027026D
		Public Overrides Sub OnSelect(eventData As BaseEventData)
			MyBase.OnSelect(eventData)
			AudioManager.Play("level_menu_move")
			Me.EvaluateHightlightDisabled(True)
		End Sub

		' Token: 0x06004C07 RID: 19463 RVA: 0x00271E87 File Offset: 0x00270287
		Public Overrides Sub OnDeselect(eventData As BaseEventData)
			MyBase.OnDeselect(eventData)
			Me.EvaluateHightlightDisabled(False)
		End Sub

		' Token: 0x06004C08 RID: 19464 RVA: 0x00271E98 File Offset: 0x00270298
		Public Sub SetNavOnToggle(setting As Boolean)
			MyBase.navigation = New Navigation() With { .mode = If((Not setting), Navigation.Mode.None, Navigation.Mode.Automatic) }
		End Sub

		' Token: 0x06004C09 RID: 19465 RVA: 0x00271EC8 File Offset: 0x002702C8
		Public Sub Press()
			If Not Me.IsActive() OrElse Not Me.IsInteractable() Then
				Return
			End If
			MyBase.onClick.Invoke()
		End Sub

		' Token: 0x06004C0A RID: 19466 RVA: 0x00271EEC File Offset: 0x002702EC
		Public Overrides Sub OnPointerClick(eventData As PointerEventData)
			If Not Me.IsActive() OrElse Not Me.IsInteractable() Then
				Return
			End If
			If eventData.button <> PointerEventData.InputButton.Left Then
				Return
			End If
			Me.Press()
			If Not Me.IsActive() OrElse Not Me.IsInteractable() Then
				Me.isHighlightDisabled = True
				Me.DoStateTransition(Selectable.SelectionState.Disabled, False)
			End If
		End Sub

		' Token: 0x06004C0B RID: 19467 RVA: 0x00271F48 File Offset: 0x00270348
		Public Overrides Sub OnSubmit(eventData As BaseEventData)
			Me.Press()
			If Not Me.IsActive() OrElse Not Me.IsInteractable() Then
				Me.isHighlightDisabled = True
				Me.DoStateTransition(Selectable.SelectionState.Disabled, False)
				Return
			End If
			Me.DoStateTransition(Selectable.SelectionState.Pressed, False)
			MyBase.StartCoroutine(Me.OnFinishSubmit())
		End Sub

		' Token: 0x06004C0C RID: 19468 RVA: 0x00271F98 File Offset: 0x00270398
		Private Iterator Function OnFinishSubmit() As IEnumerator
			Dim fadeTime As Single = MyBase.colors.fadeDuration
			Dim elapsedTime As Single = 0F
			While elapsedTime < fadeTime
				elapsedTime += Time.unscaledDeltaTime
				Yield Nothing
			End While
			Me.DoStateTransition(MyBase.currentSelectionState, False)
			Return
		End Function

		' Token: 0x06004C0D RID: 19469 RVA: 0x00271FB4 File Offset: 0x002703B4
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

		' Token: 0x06004C0E RID: 19470 RVA: 0x00272019 File Offset: 0x00270419
		Public Sub OnCancel(eventData As BaseEventData) Implements UnityEngine.EventSystems.ICancelHandler.OnCancel
			If Me._CancelEvent IsNot Nothing Then
				Me._CancelEvent()
			End If
		End Sub

		' Token: 0x040050AA RID: 20650
		<SerializeField()>
		Protected associatedText As Text

		' Token: 0x040050AB RID: 20651
		<SerializeField()>
		Private textOverrideColor As Color()

		' Token: 0x040050AC RID: 20652
		<SerializeField()>
		Private _disabledHighlightedSprite As Sprite

		' Token: 0x040050AD RID: 20653
		<SerializeField()>
		Private _disabledHighlightedColor As Color

		' Token: 0x040050AE RID: 20654
		<SerializeField()>
		Private _disabledHighlightedTrigger As String

		' Token: 0x040050AF RID: 20655
		<SerializeField()>
		Private _autoNavUp As Boolean = True

		' Token: 0x040050B0 RID: 20656
		<SerializeField()>
		Private _autoNavDown As Boolean = True

		' Token: 0x040050B1 RID: 20657
		<SerializeField()>
		Private _autoNavLeft As Boolean = True

		' Token: 0x040050B2 RID: 20658
		<SerializeField()>
		Private _autoNavRight As Boolean = True

		' Token: 0x040050B3 RID: 20659
		Protected isHighlightDisabled As Boolean
	End Class
End Namespace
