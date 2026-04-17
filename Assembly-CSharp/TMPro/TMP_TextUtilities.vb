Imports System
Imports UnityEngine

Namespace TMPro
	' Token: 0x02000C90 RID: 3216
	Public Module TMP_TextUtilities
		' Token: 0x0600512A RID: 20778 RVA: 0x0029654C File Offset: 0x0029494C
		Public Function GetCursorInsertionIndex(textComponent As TMP_Text, position As Vector3, camera As Camera) As CaretInfo
			Dim num As Integer = TMP_TextUtilities.FindNearestCharacter(textComponent, position, camera, False)
			Dim rectTransform As RectTransform = textComponent.rectTransform
			TMP_TextUtilities.ScreenPointToWorldPointInRectangle(rectTransform, position, camera, position)
			Dim tmp_CharacterInfo As TMP_CharacterInfo = textComponent.textInfo.characterInfo(num)
			Dim vector As Vector3 = rectTransform.TransformPoint(tmp_CharacterInfo.bottomLeft)
			Dim vector2 As Vector3 = rectTransform.TransformPoint(tmp_CharacterInfo.topRight)
			Dim num2 As Single = (position.x - vector.x) / (vector2.x - vector.x)
			If num2 < 0.5F Then
				Return New CaretInfo(num, CaretPosition.Left)
			End If
			Return New CaretInfo(num, CaretPosition.Right)
		End Function

		' Token: 0x0600512B RID: 20779 RVA: 0x002965EC File Offset: 0x002949EC
		Public Function GetCursorIndexFromPosition(textComponent As TMP_Text, position As Vector3, camera As Camera) As Integer
			Dim num As Integer = TMP_TextUtilities.FindNearestCharacter(textComponent, position, camera, False)
			Dim rectTransform As RectTransform = textComponent.rectTransform
			TMP_TextUtilities.ScreenPointToWorldPointInRectangle(rectTransform, position, camera, position)
			Dim tmp_CharacterInfo As TMP_CharacterInfo = textComponent.textInfo.characterInfo(num)
			Dim vector As Vector3 = rectTransform.TransformPoint(tmp_CharacterInfo.bottomLeft)
			Dim vector2 As Vector3 = rectTransform.TransformPoint(tmp_CharacterInfo.topRight)
			Dim num2 As Single = (position.x - vector.x) / (vector2.x - vector.x)
			If num2 < 0.5F Then
				Return num
			End If
			Return num + 1
		End Function

		' Token: 0x0600512C RID: 20780 RVA: 0x00296680 File Offset: 0x00294A80
		Public Function IsIntersectingRectTransform(rectTransform As RectTransform, position As Vector3, camera As Camera) As Boolean
			TMP_TextUtilities.ScreenPointToWorldPointInRectangle(rectTransform, position, camera, position)
			rectTransform.GetWorldCorners(TMP_TextUtilities.m_rectWorldCorners)
			Return TMP_TextUtilities.PointIntersectRectangle(position, TMP_TextUtilities.m_rectWorldCorners(0), TMP_TextUtilities.m_rectWorldCorners(1), TMP_TextUtilities.m_rectWorldCorners(2), TMP_TextUtilities.m_rectWorldCorners(3))
		End Function

		' Token: 0x0600512D RID: 20781 RVA: 0x002966F8 File Offset: 0x00294AF8
		Public Function FindIntersectingCharacter(text As TextMeshProUGUI, position As Vector3, camera As Camera, visibleOnly As Boolean) As Integer
			Dim rectTransform As RectTransform = text.rectTransform
			TMP_TextUtilities.ScreenPointToWorldPointInRectangle(rectTransform, position, camera, position)
			For i As Integer = 0 To text.textInfo.characterCount - 1
				Dim tmp_CharacterInfo As TMP_CharacterInfo = text.textInfo.characterInfo(i)
				If Not visibleOnly OrElse tmp_CharacterInfo.isVisible Then
					Dim vector As Vector3 = rectTransform.TransformPoint(tmp_CharacterInfo.bottomLeft)
					Dim vector2 As Vector3 = rectTransform.TransformPoint(New Vector3(tmp_CharacterInfo.bottomLeft.x, tmp_CharacterInfo.topRight.y, 0F))
					Dim vector3 As Vector3 = rectTransform.TransformPoint(tmp_CharacterInfo.topRight)
					Dim vector4 As Vector3 = rectTransform.TransformPoint(New Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.bottomLeft.y, 0F))
					If TMP_TextUtilities.PointIntersectRectangle(position, vector, vector2, vector3, vector4) Then
						Return i
					End If
				End If
			Next
			Return -1
		End Function

		' Token: 0x0600512E RID: 20782 RVA: 0x002967EC File Offset: 0x00294BEC
		Public Function FindIntersectingCharacter(text As TextMeshPro, position As Vector3, camera As Camera, visibleOnly As Boolean) As Integer
			Dim transform As Transform = text.transform
			TMP_TextUtilities.ScreenPointToWorldPointInRectangle(transform, position, camera, position)
			For i As Integer = 0 To text.textInfo.characterCount - 1
				Dim tmp_CharacterInfo As TMP_CharacterInfo = text.textInfo.characterInfo(i)
				If Not visibleOnly OrElse tmp_CharacterInfo.isVisible Then
					Dim vector As Vector3 = transform.TransformPoint(tmp_CharacterInfo.bottomLeft)
					Dim vector2 As Vector3 = transform.TransformPoint(New Vector3(tmp_CharacterInfo.bottomLeft.x, tmp_CharacterInfo.topRight.y, 0F))
					Dim vector3 As Vector3 = transform.TransformPoint(tmp_CharacterInfo.topRight)
					Dim vector4 As Vector3 = transform.TransformPoint(New Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.bottomLeft.y, 0F))
					If TMP_TextUtilities.PointIntersectRectangle(position, vector, vector2, vector3, vector4) Then
						Return i
					End If
				End If
			Next
			Return -1
		End Function

		' Token: 0x0600512F RID: 20783 RVA: 0x002968E0 File Offset: 0x00294CE0
		Public Function FindNearestCharacter(text As TMP_Text, position As Vector3, camera As Camera, visibleOnly As Boolean) As Integer
			Dim rectTransform As RectTransform = text.rectTransform
			Dim num As Single = Single.PositiveInfinity
			Dim num2 As Integer = 0
			TMP_TextUtilities.ScreenPointToWorldPointInRectangle(rectTransform, position, camera, position)
			For i As Integer = 0 To text.textInfo.characterCount - 1
				Dim tmp_CharacterInfo As TMP_CharacterInfo = text.textInfo.characterInfo(i)
				If Not visibleOnly OrElse tmp_CharacterInfo.isVisible Then
					Dim vector As Vector3 = rectTransform.TransformPoint(tmp_CharacterInfo.bottomLeft)
					Dim vector2 As Vector3 = rectTransform.TransformPoint(New Vector3(tmp_CharacterInfo.bottomLeft.x, tmp_CharacterInfo.topRight.y, 0F))
					Dim vector3 As Vector3 = rectTransform.TransformPoint(tmp_CharacterInfo.topRight)
					Dim vector4 As Vector3 = rectTransform.TransformPoint(New Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.bottomLeft.y, 0F))
					If TMP_TextUtilities.PointIntersectRectangle(position, vector, vector2, vector3, vector4) Then
						Return i
					End If
					Dim num3 As Single = TMP_TextUtilities.DistanceToLine(vector, vector2, position)
					Dim num4 As Single = TMP_TextUtilities.DistanceToLine(vector2, vector3, position)
					Dim num5 As Single = TMP_TextUtilities.DistanceToLine(vector3, vector4, position)
					Dim num6 As Single = TMP_TextUtilities.DistanceToLine(vector4, vector, position)
					Dim num7 As Single = If((num3 >= num4), num4, num3)
					num7 = If((num7 >= num5), num5, num7)
					num7 = If((num7 >= num6), num6, num7)
					If num > num7 Then
						num = num7
						num2 = i
					End If
				End If
			Next
			Return num2
		End Function

		' Token: 0x06005130 RID: 20784 RVA: 0x00296A58 File Offset: 0x00294E58
		Public Function FindNearestCharacter(text As TextMeshProUGUI, position As Vector3, camera As Camera, visibleOnly As Boolean) As Integer
			Dim rectTransform As RectTransform = text.rectTransform
			Dim num As Single = Single.PositiveInfinity
			Dim num2 As Integer = 0
			TMP_TextUtilities.ScreenPointToWorldPointInRectangle(rectTransform, position, camera, position)
			For i As Integer = 0 To text.textInfo.characterCount - 1
				Dim tmp_CharacterInfo As TMP_CharacterInfo = text.textInfo.characterInfo(i)
				If Not visibleOnly OrElse tmp_CharacterInfo.isVisible Then
					Dim vector As Vector3 = rectTransform.TransformPoint(tmp_CharacterInfo.bottomLeft)
					Dim vector2 As Vector3 = rectTransform.TransformPoint(New Vector3(tmp_CharacterInfo.bottomLeft.x, tmp_CharacterInfo.topRight.y, 0F))
					Dim vector3 As Vector3 = rectTransform.TransformPoint(tmp_CharacterInfo.topRight)
					Dim vector4 As Vector3 = rectTransform.TransformPoint(New Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.bottomLeft.y, 0F))
					If TMP_TextUtilities.PointIntersectRectangle(position, vector, vector2, vector3, vector4) Then
						Return i
					End If
					Dim num3 As Single = TMP_TextUtilities.DistanceToLine(vector, vector2, position)
					Dim num4 As Single = TMP_TextUtilities.DistanceToLine(vector2, vector3, position)
					Dim num5 As Single = TMP_TextUtilities.DistanceToLine(vector3, vector4, position)
					Dim num6 As Single = TMP_TextUtilities.DistanceToLine(vector4, vector, position)
					Dim num7 As Single = If((num3 >= num4), num4, num3)
					num7 = If((num7 >= num5), num5, num7)
					num7 = If((num7 >= num6), num6, num7)
					If num > num7 Then
						num = num7
						num2 = i
					End If
				End If
			Next
			Return num2
		End Function

		' Token: 0x06005131 RID: 20785 RVA: 0x00296BD0 File Offset: 0x00294FD0
		Public Function FindNearestCharacter(text As TextMeshPro, position As Vector3, camera As Camera, visibleOnly As Boolean) As Integer
			Dim transform As Transform = text.transform
			Dim num As Single = Single.PositiveInfinity
			Dim num2 As Integer = 0
			TMP_TextUtilities.ScreenPointToWorldPointInRectangle(transform, position, camera, position)
			For i As Integer = 0 To text.textInfo.characterCount - 1
				Dim tmp_CharacterInfo As TMP_CharacterInfo = text.textInfo.characterInfo(i)
				If Not visibleOnly OrElse tmp_CharacterInfo.isVisible Then
					Dim vector As Vector3 = transform.TransformPoint(tmp_CharacterInfo.bottomLeft)
					Dim vector2 As Vector3 = transform.TransformPoint(New Vector3(tmp_CharacterInfo.bottomLeft.x, tmp_CharacterInfo.topRight.y, 0F))
					Dim vector3 As Vector3 = transform.TransformPoint(tmp_CharacterInfo.topRight)
					Dim vector4 As Vector3 = transform.TransformPoint(New Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.bottomLeft.y, 0F))
					If TMP_TextUtilities.PointIntersectRectangle(position, vector, vector2, vector3, vector4) Then
						Return i
					End If
					Dim num3 As Single = TMP_TextUtilities.DistanceToLine(vector, vector2, position)
					Dim num4 As Single = TMP_TextUtilities.DistanceToLine(vector2, vector3, position)
					Dim num5 As Single = TMP_TextUtilities.DistanceToLine(vector3, vector4, position)
					Dim num6 As Single = TMP_TextUtilities.DistanceToLine(vector4, vector, position)
					Dim num7 As Single = If((num3 >= num4), num4, num3)
					num7 = If((num7 >= num5), num5, num7)
					num7 = If((num7 >= num6), num6, num7)
					If num > num7 Then
						num = num7
						num2 = i
					End If
				End If
			Next
			Return num2
		End Function

		' Token: 0x06005132 RID: 20786 RVA: 0x00296D48 File Offset: 0x00295148
		Public Function FindIntersectingWord(text As TextMeshProUGUI, position As Vector3, camera As Camera) As Integer
			Dim rectTransform As RectTransform = text.rectTransform
			TMP_TextUtilities.ScreenPointToWorldPointInRectangle(rectTransform, position, camera, position)
			For i As Integer = 0 To text.textInfo.wordCount - 1
				Dim tmp_WordInfo As TMP_WordInfo = text.textInfo.wordInfo(i)
				Dim flag As Boolean = False
				Dim vector As Vector3 = Vector3.zero
				Dim vector2 As Vector3 = Vector3.zero
				Dim vector3 As Vector3 = Vector3.zero
				Dim vector4 As Vector3 = Vector3.zero
				Dim num As Single = Single.NegativeInfinity
				Dim num2 As Single = Single.PositiveInfinity
				For j As Integer = 0 To tmp_WordInfo.characterCount - 1
					Dim num3 As Integer = tmp_WordInfo.firstCharacterIndex + j
					Dim tmp_CharacterInfo As TMP_CharacterInfo = text.textInfo.characterInfo(num3)
					Dim lineNumber As Integer = CInt(tmp_CharacterInfo.lineNumber)
					Dim flag2 As Boolean = num3 <= text.maxVisibleCharacters AndAlso CInt(tmp_CharacterInfo.lineNumber) <= text.maxVisibleLines AndAlso (text.OverflowMode <> TextOverflowModes.Page OrElse CInt((tmp_CharacterInfo.pageNumber + 1S)) = text.pageToDisplay)
					num = Mathf.Max(num, tmp_CharacterInfo.ascender)
					num2 = Mathf.Min(num2, tmp_CharacterInfo.descender)
					If Not flag AndAlso flag2 Then
						flag = True
						vector = New Vector3(tmp_CharacterInfo.bottomLeft.x, tmp_CharacterInfo.descender, 0F)
						vector2 = New Vector3(tmp_CharacterInfo.bottomLeft.x, tmp_CharacterInfo.ascender, 0F)
						If tmp_WordInfo.characterCount = 1 Then
							flag = False
							vector3 = New Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0F)
							vector4 = New Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0F)
							vector = rectTransform.TransformPoint(New Vector3(vector.x, num2, 0F))
							vector2 = rectTransform.TransformPoint(New Vector3(vector2.x, num, 0F))
							vector4 = rectTransform.TransformPoint(New Vector3(vector4.x, num, 0F))
							vector3 = rectTransform.TransformPoint(New Vector3(vector3.x, num2, 0F))
							If TMP_TextUtilities.PointIntersectRectangle(position, vector, vector2, vector4, vector3) Then
								Return i
							End If
						End If
					End If
					If flag AndAlso j = tmp_WordInfo.characterCount - 1 Then
						flag = False
						vector3 = New Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0F)
						vector4 = New Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0F)
						vector = rectTransform.TransformPoint(New Vector3(vector.x, num2, 0F))
						vector2 = rectTransform.TransformPoint(New Vector3(vector2.x, num, 0F))
						vector4 = rectTransform.TransformPoint(New Vector3(vector4.x, num, 0F))
						vector3 = rectTransform.TransformPoint(New Vector3(vector3.x, num2, 0F))
						If TMP_TextUtilities.PointIntersectRectangle(position, vector, vector2, vector4, vector3) Then
							Return i
						End If
					ElseIf flag AndAlso lineNumber <> CInt(text.textInfo.characterInfo(num3 + 1).lineNumber) Then
						flag = False
						vector3 = New Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0F)
						vector4 = New Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0F)
						vector = rectTransform.TransformPoint(New Vector3(vector.x, num2, 0F))
						vector2 = rectTransform.TransformPoint(New Vector3(vector2.x, num, 0F))
						vector4 = rectTransform.TransformPoint(New Vector3(vector4.x, num, 0F))
						vector3 = rectTransform.TransformPoint(New Vector3(vector3.x, num2, 0F))
						num = Single.NegativeInfinity
						num2 = Single.PositiveInfinity
						If TMP_TextUtilities.PointIntersectRectangle(position, vector, vector2, vector4, vector3) Then
							Return i
						End If
					End If
				Next
			Next
			Return -1
		End Function

		' Token: 0x06005133 RID: 20787 RVA: 0x0029716C File Offset: 0x0029556C
		Public Function FindIntersectingWord(text As TextMeshPro, position As Vector3, camera As Camera) As Integer
			Dim transform As Transform = text.transform
			TMP_TextUtilities.ScreenPointToWorldPointInRectangle(transform, position, camera, position)
			For i As Integer = 0 To text.textInfo.wordCount - 1
				Dim tmp_WordInfo As TMP_WordInfo = text.textInfo.wordInfo(i)
				Dim flag As Boolean = False
				Dim vector As Vector3 = Vector3.zero
				Dim vector2 As Vector3 = Vector3.zero
				Dim vector3 As Vector3 = Vector3.zero
				Dim vector4 As Vector3 = Vector3.zero
				Dim num As Single = Single.NegativeInfinity
				Dim num2 As Single = Single.PositiveInfinity
				For j As Integer = 0 To tmp_WordInfo.characterCount - 1
					Dim num3 As Integer = tmp_WordInfo.firstCharacterIndex + j
					Dim tmp_CharacterInfo As TMP_CharacterInfo = text.textInfo.characterInfo(num3)
					Dim lineNumber As Integer = CInt(tmp_CharacterInfo.lineNumber)
					Dim flag2 As Boolean = num3 <= text.maxVisibleCharacters AndAlso CInt(tmp_CharacterInfo.lineNumber) <= text.maxVisibleLines AndAlso (text.OverflowMode <> TextOverflowModes.Page OrElse CInt((tmp_CharacterInfo.pageNumber + 1S)) = text.pageToDisplay)
					num = Mathf.Max(num, tmp_CharacterInfo.ascender)
					num2 = Mathf.Min(num2, tmp_CharacterInfo.descender)
					If Not flag AndAlso flag2 Then
						flag = True
						vector = New Vector3(tmp_CharacterInfo.bottomLeft.x, tmp_CharacterInfo.descender, 0F)
						vector2 = New Vector3(tmp_CharacterInfo.bottomLeft.x, tmp_CharacterInfo.ascender, 0F)
						If tmp_WordInfo.characterCount = 1 Then
							flag = False
							vector3 = New Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0F)
							vector4 = New Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0F)
							vector = transform.TransformPoint(New Vector3(vector.x, num2, 0F))
							vector2 = transform.TransformPoint(New Vector3(vector2.x, num, 0F))
							vector4 = transform.TransformPoint(New Vector3(vector4.x, num, 0F))
							vector3 = transform.TransformPoint(New Vector3(vector3.x, num2, 0F))
							If TMP_TextUtilities.PointIntersectRectangle(position, vector, vector2, vector4, vector3) Then
								Return i
							End If
						End If
					End If
					If flag AndAlso j = tmp_WordInfo.characterCount - 1 Then
						flag = False
						vector3 = New Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0F)
						vector4 = New Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0F)
						vector = transform.TransformPoint(New Vector3(vector.x, num2, 0F))
						vector2 = transform.TransformPoint(New Vector3(vector2.x, num, 0F))
						vector4 = transform.TransformPoint(New Vector3(vector4.x, num, 0F))
						vector3 = transform.TransformPoint(New Vector3(vector3.x, num2, 0F))
						If TMP_TextUtilities.PointIntersectRectangle(position, vector, vector2, vector4, vector3) Then
							Return i
						End If
					ElseIf flag AndAlso lineNumber <> CInt(text.textInfo.characterInfo(num3 + 1).lineNumber) Then
						flag = False
						vector3 = New Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0F)
						vector4 = New Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0F)
						vector = transform.TransformPoint(New Vector3(vector.x, num2, 0F))
						vector2 = transform.TransformPoint(New Vector3(vector2.x, num, 0F))
						vector4 = transform.TransformPoint(New Vector3(vector4.x, num, 0F))
						vector3 = transform.TransformPoint(New Vector3(vector3.x, num2, 0F))
						num = Single.NegativeInfinity
						num2 = Single.PositiveInfinity
						If TMP_TextUtilities.PointIntersectRectangle(position, vector, vector2, vector4, vector3) Then
							Return i
						End If
					End If
				Next
			Next
			Return -1
		End Function

		' Token: 0x06005134 RID: 20788 RVA: 0x00297590 File Offset: 0x00295990
		Public Function FindNearestWord(text As TextMeshProUGUI, position As Vector3, camera As Camera) As Integer
			Dim rectTransform As RectTransform = text.rectTransform
			Dim num As Single = Single.PositiveInfinity
			Dim num2 As Integer = 0
			TMP_TextUtilities.ScreenPointToWorldPointInRectangle(rectTransform, position, camera, position)
			For i As Integer = 0 To text.textInfo.wordCount - 1
				Dim tmp_WordInfo As TMP_WordInfo = text.textInfo.wordInfo(i)
				Dim flag As Boolean = False
				Dim vector As Vector3 = Vector3.zero
				Dim vector2 As Vector3 = Vector3.zero
				Dim vector3 As Vector3 = Vector3.zero
				Dim vector4 As Vector3 = Vector3.zero
				For j As Integer = 0 To tmp_WordInfo.characterCount - 1
					Dim num3 As Integer = tmp_WordInfo.firstCharacterIndex + j
					Dim tmp_CharacterInfo As TMP_CharacterInfo = text.textInfo.characterInfo(num3)
					Dim lineNumber As Integer = CInt(tmp_CharacterInfo.lineNumber)
					Dim flag2 As Boolean = num3 <= text.maxVisibleCharacters AndAlso CInt(tmp_CharacterInfo.lineNumber) <= text.maxVisibleLines AndAlso (text.OverflowMode <> TextOverflowModes.Page OrElse CInt((tmp_CharacterInfo.pageNumber + 1S)) = text.pageToDisplay)
					If Not flag AndAlso flag2 Then
						flag = True
						vector = rectTransform.TransformPoint(New Vector3(tmp_CharacterInfo.bottomLeft.x, tmp_CharacterInfo.descender, 0F))
						vector2 = rectTransform.TransformPoint(New Vector3(tmp_CharacterInfo.bottomLeft.x, tmp_CharacterInfo.ascender, 0F))
						If tmp_WordInfo.characterCount = 1 Then
							flag = False
							vector3 = rectTransform.TransformPoint(New Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0F))
							vector4 = rectTransform.TransformPoint(New Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0F))
							If TMP_TextUtilities.PointIntersectRectangle(position, vector, vector2, vector4, vector3) Then
								Return i
							End If
						End If
					End If
					If flag AndAlso j = tmp_WordInfo.characterCount - 1 Then
						flag = False
						vector3 = rectTransform.TransformPoint(New Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0F))
						vector4 = rectTransform.TransformPoint(New Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0F))
						If TMP_TextUtilities.PointIntersectRectangle(position, vector, vector2, vector4, vector3) Then
							Return i
						End If
					ElseIf flag AndAlso lineNumber <> CInt(text.textInfo.characterInfo(num3 + 1).lineNumber) Then
						flag = False
						vector3 = rectTransform.TransformPoint(New Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0F))
						vector4 = rectTransform.TransformPoint(New Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0F))
						If TMP_TextUtilities.PointIntersectRectangle(position, vector, vector2, vector4, vector3) Then
							Return i
						End If
					End If
				Next
				Dim num4 As Single = TMP_TextUtilities.DistanceToLine(vector, vector2, position)
				Dim num5 As Single = TMP_TextUtilities.DistanceToLine(vector2, vector4, position)
				Dim num6 As Single = TMP_TextUtilities.DistanceToLine(vector4, vector3, position)
				Dim num7 As Single = TMP_TextUtilities.DistanceToLine(vector3, vector, position)
				Dim num8 As Single = If((num4 >= num5), num5, num4)
				num8 = If((num8 >= num6), num6, num8)
				num8 = If((num8 >= num7), num7, num8)
				If num > num8 Then
					num = num8
					num2 = i
				End If
			Next
			Return num2
		End Function

		' Token: 0x06005135 RID: 20789 RVA: 0x002978EC File Offset: 0x00295CEC
		Public Function FindNearestWord(text As TextMeshPro, position As Vector3, camera As Camera) As Integer
			Dim transform As Transform = text.transform
			Dim num As Single = Single.PositiveInfinity
			Dim num2 As Integer = 0
			TMP_TextUtilities.ScreenPointToWorldPointInRectangle(transform, position, camera, position)
			For i As Integer = 0 To text.textInfo.wordCount - 1
				Dim tmp_WordInfo As TMP_WordInfo = text.textInfo.wordInfo(i)
				Dim flag As Boolean = False
				Dim vector As Vector3 = Vector3.zero
				Dim vector2 As Vector3 = Vector3.zero
				Dim vector3 As Vector3 = Vector3.zero
				Dim vector4 As Vector3 = Vector3.zero
				For j As Integer = 0 To tmp_WordInfo.characterCount - 1
					Dim num3 As Integer = tmp_WordInfo.firstCharacterIndex + j
					Dim tmp_CharacterInfo As TMP_CharacterInfo = text.textInfo.characterInfo(num3)
					Dim lineNumber As Integer = CInt(tmp_CharacterInfo.lineNumber)
					Dim flag2 As Boolean = num3 <= text.maxVisibleCharacters AndAlso CInt(tmp_CharacterInfo.lineNumber) <= text.maxVisibleLines AndAlso (text.OverflowMode <> TextOverflowModes.Page OrElse CInt((tmp_CharacterInfo.pageNumber + 1S)) = text.pageToDisplay)
					If Not flag AndAlso flag2 Then
						flag = True
						vector = transform.TransformPoint(New Vector3(tmp_CharacterInfo.bottomLeft.x, tmp_CharacterInfo.descender, 0F))
						vector2 = transform.TransformPoint(New Vector3(tmp_CharacterInfo.bottomLeft.x, tmp_CharacterInfo.ascender, 0F))
						If tmp_WordInfo.characterCount = 1 Then
							flag = False
							vector3 = transform.TransformPoint(New Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0F))
							vector4 = transform.TransformPoint(New Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0F))
							If TMP_TextUtilities.PointIntersectRectangle(position, vector, vector2, vector4, vector3) Then
								Return i
							End If
						End If
					End If
					If flag AndAlso j = tmp_WordInfo.characterCount - 1 Then
						flag = False
						vector3 = transform.TransformPoint(New Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0F))
						vector4 = transform.TransformPoint(New Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0F))
						If TMP_TextUtilities.PointIntersectRectangle(position, vector, vector2, vector4, vector3) Then
							Return i
						End If
					ElseIf flag AndAlso lineNumber <> CInt(text.textInfo.characterInfo(num3 + 1).lineNumber) Then
						flag = False
						vector3 = transform.TransformPoint(New Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0F))
						vector4 = transform.TransformPoint(New Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0F))
						If TMP_TextUtilities.PointIntersectRectangle(position, vector, vector2, vector4, vector3) Then
							Return i
						End If
					End If
				Next
				Dim num4 As Single = TMP_TextUtilities.DistanceToLine(vector, vector2, position)
				Dim num5 As Single = TMP_TextUtilities.DistanceToLine(vector2, vector4, position)
				Dim num6 As Single = TMP_TextUtilities.DistanceToLine(vector4, vector3, position)
				Dim num7 As Single = TMP_TextUtilities.DistanceToLine(vector3, vector, position)
				Dim num8 As Single = If((num4 >= num5), num5, num4)
				num8 = If((num8 >= num6), num6, num8)
				num8 = If((num8 >= num7), num7, num8)
				If num > num8 Then
					num = num8
					num2 = i
				End If
			Next
			Return num2
		End Function

		' Token: 0x06005136 RID: 20790 RVA: 0x00297C48 File Offset: 0x00296048
		Public Function FindIntersectingLink(text As TextMeshProUGUI, position As Vector3, camera As Camera) As Integer
			Dim transform As Transform = text.transform
			TMP_TextUtilities.ScreenPointToWorldPointInRectangle(transform, position, camera, position)
			For i As Integer = 0 To text.textInfo.linkCount - 1
				Dim tmp_LinkInfo As TMP_LinkInfo = text.textInfo.linkInfo(i)
				Dim flag As Boolean = False
				Dim vector As Vector3 = Vector3.zero
				Dim vector2 As Vector3 = Vector3.zero
				Dim vector3 As Vector3 = Vector3.zero
				Dim vector4 As Vector3 = Vector3.zero
				For j As Integer = 0 To tmp_LinkInfo.linkTextLength - 1
					Dim num As Integer = tmp_LinkInfo.linkTextfirstCharacterIndex + j
					Dim tmp_CharacterInfo As TMP_CharacterInfo = text.textInfo.characterInfo(num)
					Dim lineNumber As Integer = CInt(tmp_CharacterInfo.lineNumber)
					If Not flag Then
						flag = True
						vector = transform.TransformPoint(New Vector3(tmp_CharacterInfo.bottomLeft.x, tmp_CharacterInfo.descender, 0F))
						vector2 = transform.TransformPoint(New Vector3(tmp_CharacterInfo.bottomLeft.x, tmp_CharacterInfo.ascender, 0F))
						If tmp_LinkInfo.linkTextLength = 1 Then
							flag = False
							vector3 = transform.TransformPoint(New Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0F))
							vector4 = transform.TransformPoint(New Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0F))
							If TMP_TextUtilities.PointIntersectRectangle(position, vector, vector2, vector4, vector3) Then
								Return i
							End If
						End If
					End If
					If flag AndAlso j = tmp_LinkInfo.linkTextLength - 1 Then
						flag = False
						vector3 = transform.TransformPoint(New Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0F))
						vector4 = transform.TransformPoint(New Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0F))
						If TMP_TextUtilities.PointIntersectRectangle(position, vector, vector2, vector4, vector3) Then
							Return i
						End If
					ElseIf flag AndAlso lineNumber <> CInt(text.textInfo.characterInfo(num + 1).lineNumber) Then
						flag = False
						vector3 = transform.TransformPoint(New Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0F))
						vector4 = transform.TransformPoint(New Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0F))
						If TMP_TextUtilities.PointIntersectRectangle(position, vector, vector2, vector4, vector3) Then
							Return i
						End If
					End If
				Next
			Next
			Return -1
		End Function

		' Token: 0x06005137 RID: 20791 RVA: 0x00297ECC File Offset: 0x002962CC
		Public Function FindIntersectingLink(text As TextMeshPro, position As Vector3, camera As Camera) As Integer
			Dim transform As Transform = text.transform
			TMP_TextUtilities.ScreenPointToWorldPointInRectangle(transform, position, camera, position)
			For i As Integer = 0 To text.textInfo.linkCount - 1
				Dim tmp_LinkInfo As TMP_LinkInfo = text.textInfo.linkInfo(i)
				Dim flag As Boolean = False
				Dim vector As Vector3 = Vector3.zero
				Dim vector2 As Vector3 = Vector3.zero
				Dim vector3 As Vector3 = Vector3.zero
				Dim vector4 As Vector3 = Vector3.zero
				For j As Integer = 0 To tmp_LinkInfo.linkTextLength - 1
					Dim num As Integer = tmp_LinkInfo.linkTextfirstCharacterIndex + j
					Dim tmp_CharacterInfo As TMP_CharacterInfo = text.textInfo.characterInfo(num)
					Dim lineNumber As Integer = CInt(tmp_CharacterInfo.lineNumber)
					If Not flag Then
						flag = True
						vector = transform.TransformPoint(New Vector3(tmp_CharacterInfo.bottomLeft.x, tmp_CharacterInfo.descender, 0F))
						vector2 = transform.TransformPoint(New Vector3(tmp_CharacterInfo.bottomLeft.x, tmp_CharacterInfo.ascender, 0F))
						If tmp_LinkInfo.linkTextLength = 1 Then
							flag = False
							vector3 = transform.TransformPoint(New Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0F))
							vector4 = transform.TransformPoint(New Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0F))
							If TMP_TextUtilities.PointIntersectRectangle(position, vector, vector2, vector4, vector3) Then
								Return i
							End If
						End If
					End If
					If flag AndAlso j = tmp_LinkInfo.linkTextLength - 1 Then
						flag = False
						vector3 = transform.TransformPoint(New Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0F))
						vector4 = transform.TransformPoint(New Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0F))
						If TMP_TextUtilities.PointIntersectRectangle(position, vector, vector2, vector4, vector3) Then
							Return i
						End If
					ElseIf flag AndAlso lineNumber <> CInt(text.textInfo.characterInfo(num + 1).lineNumber) Then
						flag = False
						vector3 = transform.TransformPoint(New Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0F))
						vector4 = transform.TransformPoint(New Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0F))
						If TMP_TextUtilities.PointIntersectRectangle(position, vector, vector2, vector4, vector3) Then
							Return i
						End If
					End If
				Next
			Next
			Return -1
		End Function

		' Token: 0x06005138 RID: 20792 RVA: 0x00298150 File Offset: 0x00296550
		Public Function FindNearestLink(text As TextMeshProUGUI, position As Vector3, camera As Camera) As Integer
			Dim rectTransform As RectTransform = text.rectTransform
			TMP_TextUtilities.ScreenPointToWorldPointInRectangle(rectTransform, position, camera, position)
			Dim num As Single = Single.PositiveInfinity
			Dim num2 As Integer = 0
			For i As Integer = 0 To text.textInfo.linkCount - 1
				Dim tmp_LinkInfo As TMP_LinkInfo = text.textInfo.linkInfo(i)
				Dim flag As Boolean = False
				Dim vector As Vector3 = Vector3.zero
				Dim vector2 As Vector3 = Vector3.zero
				Dim vector3 As Vector3 = Vector3.zero
				Dim vector4 As Vector3 = Vector3.zero
				For j As Integer = 0 To tmp_LinkInfo.linkTextLength - 1
					Dim num3 As Integer = tmp_LinkInfo.linkTextfirstCharacterIndex + j
					Dim tmp_CharacterInfo As TMP_CharacterInfo = text.textInfo.characterInfo(num3)
					Dim lineNumber As Integer = CInt(tmp_CharacterInfo.lineNumber)
					If Not flag Then
						flag = True
						vector = rectTransform.TransformPoint(New Vector3(tmp_CharacterInfo.bottomLeft.x, tmp_CharacterInfo.descender, 0F))
						vector2 = rectTransform.TransformPoint(New Vector3(tmp_CharacterInfo.bottomLeft.x, tmp_CharacterInfo.ascender, 0F))
						If tmp_LinkInfo.linkTextLength = 1 Then
							flag = False
							vector3 = rectTransform.TransformPoint(New Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0F))
							vector4 = rectTransform.TransformPoint(New Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0F))
							If TMP_TextUtilities.PointIntersectRectangle(position, vector, vector2, vector4, vector3) Then
								Return i
							End If
						End If
					End If
					If flag AndAlso j = tmp_LinkInfo.linkTextLength - 1 Then
						flag = False
						vector3 = rectTransform.TransformPoint(New Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0F))
						vector4 = rectTransform.TransformPoint(New Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0F))
						If TMP_TextUtilities.PointIntersectRectangle(position, vector, vector2, vector4, vector3) Then
							Return i
						End If
					ElseIf flag AndAlso lineNumber <> CInt(text.textInfo.characterInfo(num3 + 1).lineNumber) Then
						flag = False
						vector3 = rectTransform.TransformPoint(New Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0F))
						vector4 = rectTransform.TransformPoint(New Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0F))
						If TMP_TextUtilities.PointIntersectRectangle(position, vector, vector2, vector4, vector3) Then
							Return i
						End If
					End If
				Next
				Dim num4 As Single = TMP_TextUtilities.DistanceToLine(vector, vector2, position)
				Dim num5 As Single = TMP_TextUtilities.DistanceToLine(vector2, vector4, position)
				Dim num6 As Single = TMP_TextUtilities.DistanceToLine(vector4, vector3, position)
				Dim num7 As Single = TMP_TextUtilities.DistanceToLine(vector3, vector, position)
				Dim num8 As Single = If((num4 >= num5), num5, num4)
				num8 = If((num8 >= num6), num6, num8)
				num8 = If((num8 >= num7), num7, num8)
				If num > num8 Then
					num = num8
					num2 = i
				End If
			Next
			Return num2
		End Function

		' Token: 0x06005139 RID: 20793 RVA: 0x00298460 File Offset: 0x00296860
		Public Function FindNearestLink(text As TextMeshPro, position As Vector3, camera As Camera) As Integer
			Dim transform As Transform = text.transform
			TMP_TextUtilities.ScreenPointToWorldPointInRectangle(transform, position, camera, position)
			Dim num As Single = Single.PositiveInfinity
			Dim num2 As Integer = 0
			For i As Integer = 0 To text.textInfo.linkCount - 1
				Dim tmp_LinkInfo As TMP_LinkInfo = text.textInfo.linkInfo(i)
				Dim flag As Boolean = False
				Dim vector As Vector3 = Vector3.zero
				Dim vector2 As Vector3 = Vector3.zero
				Dim vector3 As Vector3 = Vector3.zero
				Dim vector4 As Vector3 = Vector3.zero
				For j As Integer = 0 To tmp_LinkInfo.linkTextLength - 1
					Dim num3 As Integer = tmp_LinkInfo.linkTextfirstCharacterIndex + j
					Dim tmp_CharacterInfo As TMP_CharacterInfo = text.textInfo.characterInfo(num3)
					Dim lineNumber As Integer = CInt(tmp_CharacterInfo.lineNumber)
					If Not flag Then
						flag = True
						vector = transform.TransformPoint(New Vector3(tmp_CharacterInfo.bottomLeft.x, tmp_CharacterInfo.descender, 0F))
						vector2 = transform.TransformPoint(New Vector3(tmp_CharacterInfo.bottomLeft.x, tmp_CharacterInfo.ascender, 0F))
						If tmp_LinkInfo.linkTextLength = 1 Then
							flag = False
							vector3 = transform.TransformPoint(New Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0F))
							vector4 = transform.TransformPoint(New Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0F))
							If TMP_TextUtilities.PointIntersectRectangle(position, vector, vector2, vector4, vector3) Then
								Return i
							End If
						End If
					End If
					If flag AndAlso j = tmp_LinkInfo.linkTextLength - 1 Then
						flag = False
						vector3 = transform.TransformPoint(New Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0F))
						vector4 = transform.TransformPoint(New Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0F))
						If TMP_TextUtilities.PointIntersectRectangle(position, vector, vector2, vector4, vector3) Then
							Return i
						End If
					ElseIf flag AndAlso lineNumber <> CInt(text.textInfo.characterInfo(num3 + 1).lineNumber) Then
						flag = False
						vector3 = transform.TransformPoint(New Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0F))
						vector4 = transform.TransformPoint(New Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0F))
						If TMP_TextUtilities.PointIntersectRectangle(position, vector, vector2, vector4, vector3) Then
							Return i
						End If
					End If
				Next
				Dim num4 As Single = TMP_TextUtilities.DistanceToLine(vector, vector2, position)
				Dim num5 As Single = TMP_TextUtilities.DistanceToLine(vector2, vector4, position)
				Dim num6 As Single = TMP_TextUtilities.DistanceToLine(vector4, vector3, position)
				Dim num7 As Single = TMP_TextUtilities.DistanceToLine(vector3, vector, position)
				Dim num8 As Single = If((num4 >= num5), num5, num4)
				num8 = If((num8 >= num6), num6, num8)
				num8 = If((num8 >= num7), num7, num8)
				If num > num8 Then
					num = num8
					num2 = i
				End If
			Next
			Return num2
		End Function

		' Token: 0x0600513A RID: 20794 RVA: 0x00298770 File Offset: 0x00296B70
		Private Function PointIntersectRectangle(m As Vector3, a As Vector3, b As Vector3, c As Vector3, d As Vector3) As Boolean
			Dim vector As Vector3 = b - a
			Dim vector2 As Vector3 = m - a
			Dim vector3 As Vector3 = c - b
			Dim vector4 As Vector3 = m - b
			Dim num As Single = Vector3.Dot(vector, vector2)
			Dim num2 As Single = Vector3.Dot(vector3, vector4)
			Return 0F <= num AndAlso num <= Vector3.Dot(vector, vector) AndAlso 0F <= num2 AndAlso num2 <= Vector3.Dot(vector3, vector3)
		End Function

		' Token: 0x0600513B RID: 20795 RVA: 0x002987E8 File Offset: 0x00296BE8
		Public Function ScreenPointToWorldPointInRectangle(transform As Transform, screenPoint As Vector2, cam As Camera, <System.Runtime.InteropServices.OutAttribute()> ByRef worldPoint As Vector3) As Boolean
			worldPoint = Vector2.zero
			Dim ray As Ray = RectTransformUtility.ScreenPointToRay(cam, screenPoint)
			Dim plane As Plane = New Plane(transform.rotation * Vector3.back, transform.position)
			Dim num As Single
			If Not plane.Raycast(ray, num) Then
				Return False
			End If
			worldPoint = ray.GetPoint(num)
			Return True
		End Function

		' Token: 0x0600513C RID: 20796 RVA: 0x0029884C File Offset: 0x00296C4C
		Private Function IntersectLinePlane(line As TMP_TextUtilities.LineSegment, point As Vector3, normal As Vector3, <System.Runtime.InteropServices.OutAttribute()> ByRef intersectingPoint As Vector3) As Boolean
			intersectingPoint = Vector3.zero
			Dim vector As Vector3 = line.Point2 - line.Point1
			Dim vector2 As Vector3 = line.Point1 - point
			Dim num As Single = Vector3.Dot(normal, vector)
			Dim num2 As Single = -Vector3.Dot(normal, vector2)
			If Mathf.Abs(num) < Mathf.Epsilon Then
				Return num2 = 0F
			End If
			Dim num3 As Single = num2 / num
			If num3 < 0F OrElse num3 > 1F Then
				Return False
			End If
			intersectingPoint = line.Point1 + num3 * vector
			Return True
		End Function

		' Token: 0x0600513D RID: 20797 RVA: 0x002988F0 File Offset: 0x00296CF0
		Public Function DistanceToLine(a As Vector3, b As Vector3, point As Vector3) As Single
			Dim vector As Vector3 = b - a
			Dim vector2 As Vector3 = a - point
			Dim num As Single = Vector3.Dot(vector, vector2)
			If num > 0F Then
				Return Vector3.Dot(vector2, vector2)
			End If
			Dim vector3 As Vector3 = point - b
			If Vector3.Dot(vector, vector3) > 0F Then
				Return Vector3.Dot(vector3, vector3)
			End If
			Dim vector4 As Vector3 = vector2 - vector * (num / Vector3.Dot(vector, vector))
			Return Vector3.Dot(vector4, vector4)
		End Function

		' Token: 0x0600513E RID: 20798 RVA: 0x00298969 File Offset: 0x00296D69
		Public Function ToLowerFast(c As Char) As Char
			Return "-------------------------------- !-#$%&-()*+,-./0123456789:;<=>?@abcdefghijklmnopqrstuvwxyz[-]^_`abcdefghijklmnopqrstuvwxyz{|}~-"(CInt(c))
		End Function

		' Token: 0x0600513F RID: 20799 RVA: 0x00298976 File Offset: 0x00296D76
		Public Function ToUpperFast(c As Char) As Char
			Return "-------------------------------- !-#$%&-()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[-]^_`ABCDEFGHIJKLMNOPQRSTUVWXYZ{|}~-"(CInt(c))
		End Function

		' Token: 0x06005140 RID: 20800 RVA: 0x00298984 File Offset: 0x00296D84
		Public Function GetSimpleHashCode(s As String) As Integer
			Dim num As Integer = 0
			For i As Integer = 0 To s.Length - 1
				num = ((num << 5) + num) Xor CInt(s(i))
			Next
			Return num
		End Function

		' Token: 0x06005141 RID: 20801 RVA: 0x002989BC File Offset: 0x00296DBC
		Public Function GetSimpleHashCodeLowercase(s As String) As UInteger
			Dim num As UInteger = 5381UI
			For i As Integer = 0 To s.Length - 1
				num = ((num << 5) + num) Xor CUInt(TMP_TextUtilities.ToLowerFast(s(i)))
			Next
			Return num
		End Function

		' Token: 0x040053E9 RID: 21481
		Private m_rectWorldCorners As Vector3() = New Vector3(3) {}

		' Token: 0x040053EA RID: 21482
		Private Const k_lookupStringL As String = "-------------------------------- !-#$%&-()*+,-./0123456789:;<=>?@abcdefghijklmnopqrstuvwxyz[-]^_`abcdefghijklmnopqrstuvwxyz{|}~-"

		' Token: 0x040053EB RID: 21483
		Private Const k_lookupStringU As String = "-------------------------------- !-#$%&-()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[-]^_`ABCDEFGHIJKLMNOPQRSTUVWXYZ{|}~-"

		' Token: 0x02000C91 RID: 3217
		Private Structure LineSegment
			' Token: 0x06005143 RID: 20803 RVA: 0x00298A07 File Offset: 0x00296E07
			Public Sub New(p1 As Vector3, p2 As Vector3)
				Me.Point1 = p1
				Me.Point2 = p2
			End Sub

			' Token: 0x040053EC RID: 21484
			Public Point1 As Vector3

			' Token: 0x040053ED RID: 21485
			Public Point2 As Vector3
		End Structure
	End Module
End Namespace
