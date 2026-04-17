Imports System
Imports UnityEngine
Imports UnityEngine.UI

Namespace Rewired.UI.ControlMapper
	' Token: 0x02000C4E RID: 3150
	Public Module UITools
		' Token: 0x06004D64 RID: 19812 RVA: 0x00276094 File Offset: 0x00274494
		Public Function InstantiateGUIObject(Of T As Component)(prefab As GameObject, parent As Transform, name As String) As GameObject
			Dim gameObject As GameObject = UITools.InstantiateGUIObject_Pre(Of T)(prefab, parent, name)
			If gameObject Is Nothing Then
				Return Nothing
			End If
			Dim component As RectTransform = gameObject.GetComponent(Of RectTransform)()
			If component Is Nothing Then
				Global.UnityEngine.Debug.LogError(name + " prefab is missing RectTransform component!")
			Else
				component.localScale = Vector3.one
			End If
			Return gameObject
		End Function

		' Token: 0x06004D65 RID: 19813 RVA: 0x002760EC File Offset: 0x002744EC
		Public Function InstantiateGUIObject(Of T As Component)(prefab As GameObject, parent As Transform, name As String, pivot As Vector2, anchorMin As Vector2, anchorMax As Vector2, anchoredPosition As Vector2) As GameObject
			Dim gameObject As GameObject = UITools.InstantiateGUIObject_Pre(Of T)(prefab, parent, name)
			If gameObject Is Nothing Then
				Return Nothing
			End If
			Dim component As RectTransform = gameObject.GetComponent(Of RectTransform)()
			If component Is Nothing Then
				Global.UnityEngine.Debug.LogError(name + " prefab is missing RectTransform component!")
			Else
				component.localScale = Vector3.one
				component.pivot = pivot
				component.anchorMin = anchorMin
				component.anchorMax = anchorMax
				component.anchoredPosition = anchoredPosition
			End If
			Return gameObject
		End Function

		' Token: 0x06004D66 RID: 19814 RVA: 0x00276164 File Offset: 0x00274564
		Private Function InstantiateGUIObject_Pre(Of T As Component)(prefab As GameObject, parent As Transform, name As String) As GameObject
			If prefab Is Nothing Then
				Global.UnityEngine.Debug.LogError(name + " prefab is null!")
				Return Nothing
			End If
			Dim gameObject As GameObject = Global.UnityEngine.[Object].Instantiate(Of GameObject)(prefab)
			If Not String.IsNullOrEmpty(name) Then
				gameObject.name = name
			End If
			Dim component As T = gameObject.GetComponent(Of T)()
			If component Is Nothing Then
				Global.UnityEngine.Debug.LogError(name + " prefab is missing the " + component.[GetType]().ToString() + " component!")
				Return Nothing
			End If
			If parent IsNot Nothing Then
				gameObject.transform.SetParent(parent, False)
			End If
			Return gameObject
		End Function

		' Token: 0x06004D67 RID: 19815 RVA: 0x00276204 File Offset: 0x00274604
		Public Function GetPointOnRectEdge(rectTransform As RectTransform, dir As Vector2) As Vector3
			If rectTransform Is Nothing Then
				Return Vector3.zero
			End If
			If dir <> Vector2.zero Then
				dir /= Mathf.Max(Mathf.Abs(dir.x), Mathf.Abs(dir.y))
			End If
			Dim rect As Rect = rectTransform.rect
			dir = rect.center + Vector2.Scale(rect.size, dir * 0.5F)
			Return dir
		End Function

		' Token: 0x06004D68 RID: 19816 RVA: 0x0027628C File Offset: 0x0027468C
		Public Function GetWorldSpaceRect(rt As RectTransform) As Rect
			If rt Is Nothing Then
				Return Nothing
			End If
			Dim rect As Rect = rt.rect
			Dim vector As Vector3 = rt.TransformPoint(New Vector2(rect.xMin, rect.yMin))
			Dim vector2 As Vector3 = rt.TransformPoint(New Vector2(rect.xMin, rect.yMax))
			Dim vector3 As Vector3 = rt.TransformPoint(New Vector2(rect.xMax, rect.yMin))
			Return New Rect(vector.x, vector.y, vector3.x - vector.x, vector2.y - vector.y)
		End Function

		' Token: 0x06004D69 RID: 19817 RVA: 0x00276348 File Offset: 0x00274748
		Public Sub SetInteractable(selectable As Selectable, state As Boolean, playTransition As Boolean)
			If selectable Is Nothing Then
				Return
			End If
			If Not playTransition Then
				If selectable.transition = Selectable.Transition.ColorTint Then
					Dim colors As ColorBlock = selectable.colors
					Dim fadeDuration As Single = colors.fadeDuration
					colors.fadeDuration = 0F
					selectable.colors = colors
					selectable.interactable = state
					colors.fadeDuration = fadeDuration
					selectable.colors = colors
				End If
			Else
				selectable.interactable = state
			End If
		End Sub
	End Module
End Namespace
