Imports System
Imports Rewired.Utils
Imports UnityEngine
Imports UnityEngine.EventSystems
Imports UnityEngine.UI

Namespace Rewired.UI.ControlMapper
	' Token: 0x02000C37 RID: 3127
	<AddComponentMenu("")>
	<RequireComponent(GetType(Selectable))>
	Public Class ScrollRectSelectableChild
		Inherits MonoBehaviour
		Implements ISelectHandler, IEventSystemHandler

		' Token: 0x17000792 RID: 1938
		' (get) Token: 0x06004CE1 RID: 19681 RVA: 0x0027499B File Offset: 0x00272D9B
		Private ReadOnly Property parentScrollRectContentTransform As RectTransform
			Get
				Return Me.parentScrollRect.content
			End Get
		End Property

		' Token: 0x17000793 RID: 1939
		' (get) Token: 0x06004CE2 RID: 19682 RVA: 0x002749A8 File Offset: 0x00272DA8
		Private ReadOnly Property selectable As Selectable
			Get
				Dim selectable As Selectable = Me._selectable
				Dim selectable2 As Selectable = selectable
				If selectable Is Nothing Then
					Dim component As Selectable = MyBase.GetComponent(Of Selectable)()
					Dim selectable3 As Selectable = component
					Me._selectable = component
					selectable2 = selectable3
				End If
				Return selectable2
			End Get
		End Property

		' Token: 0x17000794 RID: 1940
		' (get) Token: 0x06004CE3 RID: 19683 RVA: 0x002749D1 File Offset: 0x00272DD1
		Private ReadOnly Property rectTransform As RectTransform
			Get
				Return TryCast(MyBase.transform, RectTransform)
			End Get
		End Property

		' Token: 0x06004CE4 RID: 19684 RVA: 0x002749DE File Offset: 0x00272DDE
		Private Sub Start()
			Me.parentScrollRect = MyBase.transform.GetComponentInParent(Of ScrollRect)()
			If Me.parentScrollRect Is Nothing Then
				Global.UnityEngine.Debug.LogError("Rewired Control Mapper: No ScrollRect found! This component must be a child of a ScrollRect!")
				Return
			End If
		End Sub

		' Token: 0x06004CE5 RID: 19685 RVA: 0x00274A10 File Offset: 0x00272E10
		Public Sub OnSelect(eventData As BaseEventData) Implements UnityEngine.EventSystems.ISelectHandler.OnSelect
			If Me.parentScrollRect Is Nothing Then
				Return
			End If
			If Not(TypeOf eventData Is AxisEventData) Then
				Return
			End If
			Dim rectTransform As RectTransform = TryCast(Me.parentScrollRect.transform, RectTransform)
			Dim rect As Rect = MathTools.TransformRect(Me.rectTransform.rect, Me.rectTransform, rectTransform)
			Dim rect2 As Rect = rectTransform.rect
			Dim rect3 As Rect = rectTransform.rect
			Dim height As Single
			If Me.useCustomEdgePadding Then
				height = Me.customEdgePadding
			Else
				height = rect.height
			End If
			rect3.yMax -= height
			rect3.yMin += height
			If MathTools.RectContains(rect3, rect) Then
				Return
			End If
			Dim vector As Vector2
			If Not MathTools.GetOffsetToContainRect(rect3, rect, vector) Then
				Return
			End If
			Dim anchoredPosition As Vector2 = Me.parentScrollRectContentTransform.anchoredPosition
			anchoredPosition.x = Mathf.Clamp(anchoredPosition.x + vector.x, 0F, Mathf.Abs(rect2.width - Me.parentScrollRectContentTransform.sizeDelta.x))
			anchoredPosition.y = Mathf.Clamp(anchoredPosition.y + vector.y, 0F, Mathf.Abs(rect2.height - Me.parentScrollRectContentTransform.sizeDelta.y))
			Me.parentScrollRectContentTransform.anchoredPosition = anchoredPosition
		End Sub

		' Token: 0x04005133 RID: 20787
		Public useCustomEdgePadding As Boolean

		' Token: 0x04005134 RID: 20788
		Public customEdgePadding As Single = 50F

		' Token: 0x04005135 RID: 20789
		Private parentScrollRect As ScrollRect

		' Token: 0x04005136 RID: 20790
		Private _selectable As Selectable
	End Class
End Namespace
