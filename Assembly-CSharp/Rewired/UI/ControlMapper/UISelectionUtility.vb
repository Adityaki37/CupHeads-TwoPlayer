Imports System
Imports System.Collections.Generic
Imports Rewired.Utils
Imports UnityEngine
Imports UnityEngine.UI

Namespace Rewired.UI.ControlMapper
	' Token: 0x02000C4C RID: 3148
	Public Module UISelectionUtility
		' Token: 0x06004D5D RID: 19805 RVA: 0x00275CE8 File Offset: 0x002740E8
		Public Function FindNextSelectable(selectable As Selectable, transform As Transform, allSelectables As List(Of Selectable), direction As Vector3) As Selectable
			Dim rectTransform As RectTransform = TryCast(transform, RectTransform)
			If rectTransform Is Nothing Then
				Return Nothing
			End If
			direction = direction.normalized
			Dim vector As Vector2 = Quaternion.Inverse(transform.rotation) * direction
			Dim vector2 As Vector2 = transform.TransformPoint(UITools.GetPointOnRectEdge(rectTransform, vector))
			Dim flag As Boolean = direction = Vector3.left OrElse direction = Vector3.right
			Dim num As Single = Single.PositiveInfinity
			Dim num2 As Single = Single.PositiveInfinity
			Dim selectable2 As Selectable = Nothing
			Dim selectable3 As Selectable = Nothing
			Dim vector3 As Vector2 = vector2 + vector * 999999F
			For i As Integer = 0 To allSelectables.Count - 1
				Dim selectable4 As Selectable = allSelectables(i)
				If Not(selectable4 Is selectable) AndAlso Not(selectable4 Is Nothing) Then
					If selectable4.navigation.mode <> Navigation.Mode.None Then
						If selectable4.IsInteractable() OrElse ReflectionTools.GetPrivateField(Of Selectable, Boolean)(selectable4, "m_GroupsAllowInteraction") Then
							Dim rectTransform2 As RectTransform = TryCast(selectable4.transform, RectTransform)
							If Not(rectTransform2 Is Nothing) Then
								Dim worldSpaceRect As Rect = UITools.GetWorldSpaceRect(rectTransform2)
								Dim num3 As Single
								If MathTools.LineIntersectsRect(vector2, vector3, worldSpaceRect, num3) Then
									If flag Then
										num3 *= 0.25F
									End If
									If num3 < num2 Then
										num2 = num3
										selectable3 = selectable4
									End If
								End If
								Dim vector4 As Vector2 = rectTransform2.rect.center
								Dim vector5 As Vector2 = selectable4.transform.TransformPoint(vector4) - vector2
								Dim num4 As Single = Mathf.Abs(Vector2.Angle(vector, vector5))
								If num4 <= 75F Then
									Dim sqrMagnitude As Single = vector5.sqrMagnitude
									If sqrMagnitude < num Then
										num = sqrMagnitude
										selectable2 = selectable4
									End If
								End If
							End If
						End If
					End If
				End If
			Next
			If Not(selectable3 IsNot Nothing) OrElse Not(selectable2 IsNot Nothing) Then
				Return If(selectable3, selectable2)
			End If
			If num2 > num Then
				Return selectable2
			End If
			Return selectable3
		End Function
	End Module
End Namespace
