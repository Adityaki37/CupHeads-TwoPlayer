Imports System
Imports System.Collections
Imports UnityEngine
Imports UnityEngine.UI

Namespace Rewired.UI.ControlMapper
	' Token: 0x02000C36 RID: 3126
	<AddComponentMenu("")>
	Public Class ScrollbarVisibilityHelper
		Inherits MonoBehaviour

		' Token: 0x17000790 RID: 1936
		' (get) Token: 0x06004CD8 RID: 19672 RVA: 0x002746E0 File Offset: 0x00272AE0
		Private ReadOnly Property hScrollBar As Scrollbar
			Get
				Return If((Not(Me.scrollRect IsNot Nothing)), Nothing, Me.scrollRect.horizontalScrollbar)
			End Get
		End Property

		' Token: 0x17000791 RID: 1937
		' (get) Token: 0x06004CD9 RID: 19673 RVA: 0x00274704 File Offset: 0x00272B04
		Private ReadOnly Property vScrollBar As Scrollbar
			Get
				Return If((Not(Me.scrollRect IsNot Nothing)), Nothing, Me.scrollRect.verticalScrollbar)
			End Get
		End Property

		' Token: 0x06004CDA RID: 19674 RVA: 0x00274728 File Offset: 0x00272B28
		Private Sub Awake()
			If Me.scrollRect IsNot Nothing Then
				Me.target = Me.scrollRect.gameObject.AddComponent(Of ScrollbarVisibilityHelper)()
				Me.target.onlySendMessage = True
				Me.target.target = Me
			End If
		End Sub

		' Token: 0x06004CDB RID: 19675 RVA: 0x00274774 File Offset: 0x00272B74
		Private Sub OnRectTransformDimensionsChange()
			If Me.onlySendMessage Then
				If Me.target IsNot Nothing Then
					Me.target.ScrollRectTransformDimensionsChanged()
				End If
			Else
				Me.EvaluateScrollbar()
			End If
		End Sub

		' Token: 0x06004CDC RID: 19676 RVA: 0x002747A8 File Offset: 0x00272BA8
		Private Sub ScrollRectTransformDimensionsChanged()
			Me.OnRectTransformDimensionsChange()
		End Sub

		' Token: 0x06004CDD RID: 19677 RVA: 0x002747B0 File Offset: 0x00272BB0
		Private Sub EvaluateScrollbar()
			If Me.scrollRect Is Nothing Then
				Return
			End If
			If Me.vScrollBar Is Nothing AndAlso Me.hScrollBar Is Nothing Then
				Return
			End If
			If Not MyBase.gameObject.activeInHierarchy Then
				Return
			End If
			Dim rect As Rect = Me.scrollRect.content.rect
			Dim rect2 As Rect = TryCast(Me.scrollRect.transform, RectTransform).rect
			If Me.vScrollBar IsNot Nothing Then
				Dim flag As Boolean = rect.height > rect2.height
				Me.SetActiveDeferred(Me.vScrollBar.gameObject, flag)
			End If
			If Me.hScrollBar IsNot Nothing Then
				Dim flag2 As Boolean = rect.width > rect2.width
				Me.SetActiveDeferred(Me.hScrollBar.gameObject, flag2)
			End If
		End Sub

		' Token: 0x06004CDE RID: 19678 RVA: 0x002748A6 File Offset: 0x00272CA6
		Private Sub SetActiveDeferred(obj As GameObject, value As Boolean)
			MyBase.StopAllCoroutines()
			MyBase.StartCoroutine(Me.SetActiveCoroutine(obj, value))
		End Sub

		' Token: 0x06004CDF RID: 19679 RVA: 0x002748C0 File Offset: 0x00272CC0
		Private Iterator Function SetActiveCoroutine(obj As GameObject, value As Boolean) As IEnumerator
			Yield Nothing
			If obj IsNot Nothing Then
				obj.SetActive(value)
			End If
			Return
		End Function

		' Token: 0x04005130 RID: 20784
		Public scrollRect As ScrollRect

		' Token: 0x04005131 RID: 20785
		Private onlySendMessage As Boolean

		' Token: 0x04005132 RID: 20786
		Private target As ScrollbarVisibilityHelper
	End Class
End Namespace
