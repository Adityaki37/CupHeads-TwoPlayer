Imports System
Imports System.Collections
Imports System.Diagnostics
Imports UnityEngine
Imports UnityEngine.Events
Imports UnityEngine.EventSystems
Imports UnityEngine.UI

Namespace Rewired.UI.ControlMapper
	' Token: 0x02000C2C RID: 3116
	<AddComponentMenu("")>
	Public Class CustomToggle
		Inherits Toggle
		Implements ICustomSelectable, ICancelHandler, IEventSystemHandler

		' Token: 0x1700074F RID: 1871
		' (get) Token: 0x06004C3A RID: 19514 RVA: 0x00272B9D File Offset: 0x00270F9D
		' (set) Token: 0x06004C3B RID: 19515 RVA: 0x00272BA5 File Offset: 0x00270FA5
		Public Property disabledHighlightedSprite As Sprite Implements Rewired.UI.ControlMapper.ICustomSelectable.disabledHighlightedSprite
			Get
				Return Me._disabledHighlightedSprite
			End Get
			Set(value As Sprite)
				Me._disabledHighlightedSprite = value
			End Set
		End Property

		' Token: 0x17000750 RID: 1872
		' (get) Token: 0x06004C3C RID: 19516 RVA: 0x00272BAE File Offset: 0x00270FAE
		' (set) Token: 0x06004C3D RID: 19517 RVA: 0x00272BB6 File Offset: 0x00270FB6
		Public Property disabledHighlightedColor As Color Implements Rewired.UI.ControlMapper.ICustomSelectable.disabledHighlightedColor
			Get
				Return Me._disabledHighlightedColor
			End Get
			Set(value As Color)
				Me._disabledHighlightedColor = value
			End Set
		End Property

		' Token: 0x17000751 RID: 1873
		' (get) Token: 0x06004C3E RID: 19518 RVA: 0x00272BBF File Offset: 0x00270FBF
		' (set) Token: 0x06004C3F RID: 19519 RVA: 0x00272BC7 File Offset: 0x00270FC7
		Public Property disabledHighlightedTrigger As String Implements Rewired.UI.ControlMapper.ICustomSelectable.disabledHighlightedTrigger
			Get
				Return Me._disabledHighlightedTrigger
			End Get
			Set(value As String)
				Me._disabledHighlightedTrigger = value
			End Set
		End Property

		' Token: 0x17000752 RID: 1874
		' (get) Token: 0x06004C40 RID: 19520 RVA: 0x00272BD0 File Offset: 0x00270FD0
		' (set) Token: 0x06004C41 RID: 19521 RVA: 0x00272BD8 File Offset: 0x00270FD8
		Public Property autoNavUp As Boolean Implements Rewired.UI.ControlMapper.ICustomSelectable.autoNavUp
			Get
				Return Me._autoNavUp
			End Get
			Set(value As Boolean)
				Me._autoNavUp = value
			End Set
		End Property

		' Token: 0x17000753 RID: 1875
		' (get) Token: 0x06004C42 RID: 19522 RVA: 0x00272BE1 File Offset: 0x00270FE1
		' (set) Token: 0x06004C43 RID: 19523 RVA: 0x00272BE9 File Offset: 0x00270FE9
		Public Property autoNavDown As Boolean Implements Rewired.UI.ControlMapper.ICustomSelectable.autoNavDown
			Get
				Return Me._autoNavDown
			End Get
			Set(value As Boolean)
				Me._autoNavDown = value
			End Set
		End Property

		' Token: 0x17000754 RID: 1876
		' (get) Token: 0x06004C44 RID: 19524 RVA: 0x00272BF2 File Offset: 0x00270FF2
		' (set) Token: 0x06004C45 RID: 19525 RVA: 0x00272BFA File Offset: 0x00270FFA
		Public Property autoNavLeft As Boolean Implements Rewired.UI.ControlMapper.ICustomSelectable.autoNavLeft
			Get
				Return Me._autoNavLeft
			End Get
			Set(value As Boolean)
				Me._autoNavLeft = value
			End Set
		End Property

		' Token: 0x17000755 RID: 1877
		' (get) Token: 0x06004C46 RID: 19526 RVA: 0x00272C03 File Offset: 0x00271003
		' (set) Token: 0x06004C47 RID: 19527 RVA: 0x00272C0B File Offset: 0x0027100B
		Public Property autoNavRight As Boolean Implements Rewired.UI.ControlMapper.ICustomSelectable.autoNavRight
			Get
				Return Me._autoNavRight
			End Get
			Set(value As Boolean)
				Me._autoNavRight = value
			End Set
		End Property

		' Token: 0x17000756 RID: 1878
		' (get) Token: 0x06004C48 RID: 19528 RVA: 0x00272C14 File Offset: 0x00271014
		Private ReadOnly Property isDisabled As Boolean
			Get
				Return Not Me.IsInteractable()
			End Get
		End Property

		' Token: 0x140000F5 RID: 245
		' (add) Token: 0x06004C49 RID: 19529 RVA: 0x00272C20 File Offset: 0x00271020
		' (remove) Token: 0x06004C4A RID: 19530 RVA: 0x00272C58 File Offset: 0x00271058
		<DebuggerBrowsable(DebuggerBrowsableState.Never)>
		Private Event _CancelEvent As UnityAction

		' Token: 0x140000F6 RID: 246
		' (add) Token: 0x06004C4B RID: 19531 RVA: 0x00272C8E File Offset: 0x0027108E
		' (remove) Token: 0x06004C4C RID: 19532 RVA: 0x00272C97 File Offset: 0x00271097
		Public Custom Event CancelEvent As UnityAction Implements Rewired.UI.ControlMapper.ICustomSelectable.CancelEvent
			AddHandler
				AddHandler Me._CancelEvent, value
			End AddHandler
			RemoveHandler
				RemoveHandler Me._CancelEvent, value
			End RemoveHandler
		End Event

		' Token: 0x06004C4D RID: 19533 RVA: 0x00272CA0 File Offset: 0x002710A0
		Public Overrides Function FindSelectableOnLeft() As Selectable
			If(MyBase.navigation.mode And Navigation.Mode.Horizontal) <> Navigation.Mode.None OrElse Me._autoNavLeft Then
				Return UISelectionUtility.FindNextSelectable(Me, MyBase.transform, Selectable.allSelectables, MyBase.transform.rotation * Vector3.left)
			End If
			Return MyBase.FindSelectableOnLeft()
		End Function

		' Token: 0x06004C4E RID: 19534 RVA: 0x00272CFC File Offset: 0x002710FC
		Public Overrides Function FindSelectableOnRight() As Selectable
			If(MyBase.navigation.mode And Navigation.Mode.Horizontal) <> Navigation.Mode.None OrElse Me._autoNavRight Then
				Return UISelectionUtility.FindNextSelectable(Me, MyBase.transform, Selectable.allSelectables, MyBase.transform.rotation * Vector3.right)
			End If
			Return MyBase.FindSelectableOnRight()
		End Function

		' Token: 0x06004C4F RID: 19535 RVA: 0x00272D58 File Offset: 0x00271158
		Public Overrides Function FindSelectableOnUp() As Selectable
			If(MyBase.navigation.mode And Navigation.Mode.Vertical) <> Navigation.Mode.None OrElse Me._autoNavUp Then
				Return UISelectionUtility.FindNextSelectable(Me, MyBase.transform, Selectable.allSelectables, MyBase.transform.rotation * Vector3.up)
			End If
			Return MyBase.FindSelectableOnUp()
		End Function

		' Token: 0x06004C50 RID: 19536 RVA: 0x00272DB4 File Offset: 0x002711B4
		Public Overrides Function FindSelectableOnDown() As Selectable
			If(MyBase.navigation.mode And Navigation.Mode.Vertical) <> Navigation.Mode.None OrElse Me._autoNavDown Then
				Return UISelectionUtility.FindNextSelectable(Me, MyBase.transform, Selectable.allSelectables, MyBase.transform.rotation * Vector3.down)
			End If
			Return MyBase.FindSelectableOnDown()
		End Function

		' Token: 0x06004C51 RID: 19537 RVA: 0x00272E0E File Offset: 0x0027120E
		Protected Overrides Sub OnCanvasGroupChanged()
			MyBase.OnCanvasGroupChanged()
			If EventSystem.current Is Nothing Then
				Return
			End If
			Me.EvaluateHightlightDisabled(EventSystem.current.currentSelectedGameObject Is MyBase.gameObject)
		End Sub

		' Token: 0x06004C52 RID: 19538 RVA: 0x00272E42 File Offset: 0x00271242
		Protected Overrides Sub Start()
			MyBase.Start()
			AddHandler ControlMapper.OnPlayerChange, AddressOf Me.UpdateColors
		End Sub

		' Token: 0x06004C53 RID: 19539 RVA: 0x00272E5B File Offset: 0x0027125B
		Protected Overrides Sub OnEnable()
			MyBase.OnEnable()
			MyBase.StartCoroutine(Me.update_text_color_on_enable_cr())
		End Sub

		' Token: 0x06004C54 RID: 19540 RVA: 0x00272E70 File Offset: 0x00271270
		Protected Overrides Sub OnDestroy()
			RemoveHandler ControlMapper.OnPlayerChange, AddressOf Me.UpdateColors
		End Sub

		' Token: 0x06004C55 RID: 19541 RVA: 0x00272E84 File Offset: 0x00271284
		Private Iterator Function update_text_color_on_enable_cr() As IEnumerator
			Yield New WaitForEndOfFrame()
			Me.UpdateAssociatedImageColors(MyBase.currentSelectionState)
			Return
		End Function

		' Token: 0x06004C56 RID: 19542 RVA: 0x00272E9F File Offset: 0x0027129F
		Private Sub UpdateColors()
			Me.UpdateAssociatedImageColors(MyBase.currentSelectionState)
		End Sub

		' Token: 0x06004C57 RID: 19543 RVA: 0x00272EAD File Offset: 0x002712AD
		Public Overrides Sub OnPointerClick(eventData As PointerEventData)
			MyBase.OnPointerClick(eventData)
			Me.UpdateColors()
		End Sub

		' Token: 0x06004C58 RID: 19544 RVA: 0x00272EBC File Offset: 0x002712BC
		Public Overrides Sub OnSubmit(eventData As BaseEventData)
			MyBase.OnSubmit(eventData)
			Me.UpdateColors()
		End Sub

		' Token: 0x06004C59 RID: 19545 RVA: 0x00272ECC File Offset: 0x002712CC
		Protected Sub UpdateAssociatedImageColors(state As Selectable.SelectionState)
			If state = Selectable.SelectionState.Highlighted Then
				Me.checkImage.color = Me.checkOverrideColor(If((Not MyBase.isOn), 3, 2))
			Else
				Me.checkImage.color = Me.checkOverrideColor(If((Not MyBase.isOn), 3, ControlMapper.CurrentPlayer()))
			End If
			Me.checkBoxImage.color = Me.checkBoxOverrideColor(CInt(state))
		End Sub

		' Token: 0x06004C5A RID: 19546 RVA: 0x00272F60 File Offset: 0x00271360
		Protected Overrides Sub DoStateTransition(state As Selectable.SelectionState, instant As Boolean)
			Me.UpdateAssociatedImageColors(state)
			Dim colors As ColorBlock = MyBase.colors
			colors.normalColor = New Color(MyBase.colors.normalColor.r, MyBase.colors.normalColor.g, MyBase.colors.normalColor.b, 0F)
			MyBase.colors = colors
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

		' Token: 0x06004C5B RID: 19547 RVA: 0x0027307C File Offset: 0x0027147C
		Private Sub StartColorTween(targetColor As Color, instant As Boolean)
			If MyBase.targetGraphic Is Nothing Then
				Return
			End If
			MyBase.targetGraphic.CrossFadeColor(targetColor, If((Not instant), MyBase.colors.fadeDuration, 0F), True, True)
		End Sub

		' Token: 0x06004C5C RID: 19548 RVA: 0x002730C7 File Offset: 0x002714C7
		Private Sub DoSpriteSwap(newSprite As Sprite)
			If MyBase.image Is Nothing Then
				Return
			End If
			MyBase.image.overrideSprite = newSprite
		End Sub

		' Token: 0x06004C5D RID: 19549 RVA: 0x002730E8 File Offset: 0x002714E8
		Private Sub TriggerAnimation(triggername As String)
			If MyBase.animator Is Nothing OrElse Not MyBase.animator.enabled OrElse Not MyBase.animator.isActiveAndEnabled OrElse MyBase.animator.runtimeAnimatorController Is Nothing OrElse String.IsNullOrEmpty(triggername) Then
				Return
			End If
			MyBase.animator.ResetTrigger(Me._disabledHighlightedTrigger)
			MyBase.animator.SetTrigger(triggername)
		End Sub

		' Token: 0x06004C5E RID: 19550 RVA: 0x00273165 File Offset: 0x00271565
		Public Overrides Sub OnSelect(eventData As BaseEventData)
			MyBase.OnSelect(eventData)
			Me.EvaluateHightlightDisabled(True)
		End Sub

		' Token: 0x06004C5F RID: 19551 RVA: 0x00273175 File Offset: 0x00271575
		Public Overrides Sub OnDeselect(eventData As BaseEventData)
			MyBase.OnDeselect(eventData)
			Me.EvaluateHightlightDisabled(False)
		End Sub

		' Token: 0x06004C60 RID: 19552 RVA: 0x00273188 File Offset: 0x00271588
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

		' Token: 0x06004C61 RID: 19553 RVA: 0x002731ED File Offset: 0x002715ED
		Public Sub OnCancel(eventData As BaseEventData) Implements UnityEngine.EventSystems.ICancelHandler.OnCancel
			If Me._CancelEvent IsNot Nothing Then
				Me._CancelEvent()
			End If
		End Sub

		' Token: 0x040050C6 RID: 20678
		<SerializeField()>
		Private checkImage As Image

		' Token: 0x040050C7 RID: 20679
		<SerializeField()>
		Private checkBoxImage As Image

		' Token: 0x040050C8 RID: 20680
		<SerializeField()>
		Private checkOverrideColor As Color()

		' Token: 0x040050C9 RID: 20681
		<SerializeField()>
		Private checkBoxOverrideColor As Color()

		' Token: 0x040050CA RID: 20682
		<SerializeField()>
		Private _disabledHighlightedSprite As Sprite

		' Token: 0x040050CB RID: 20683
		<SerializeField()>
		Private _disabledHighlightedColor As Color

		' Token: 0x040050CC RID: 20684
		<SerializeField()>
		Private _disabledHighlightedTrigger As String

		' Token: 0x040050CD RID: 20685
		<SerializeField()>
		Private _autoNavUp As Boolean = True

		' Token: 0x040050CE RID: 20686
		<SerializeField()>
		Private _autoNavDown As Boolean = True

		' Token: 0x040050CF RID: 20687
		<SerializeField()>
		Private _autoNavLeft As Boolean = True

		' Token: 0x040050D0 RID: 20688
		<SerializeField()>
		Private _autoNavRight As Boolean = True

		' Token: 0x040050D1 RID: 20689
		Private isHighlightDisabled As Boolean
	End Class
End Namespace
