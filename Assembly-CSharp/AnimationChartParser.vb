Imports System
Imports System.Collections.Generic

' Token: 0x0200039D RID: 925
Public Module AnimationChartParser
	' Token: 0x06000B47 RID: 2887 RVA: 0x00082918 File Offset: 0x00080D18
	Public Function CreateTextSegment(segment As String) As TextSegment
		Dim textSegment As TextSegment = New TextSegment()
		Dim text As String = segment
		Dim array As String() = segment.Split(New Char() { "x"c })
		If array.Length <= 2 Then
			If array.Length = 2 Then
				Parser.IntTryParse(array(array.Length - 1), textSegment.multiplier)
				text = array(0).Substring(1, array(0).Length - 2)
			ElseIf text.Substring(0, 1) = "(" Then
				text = array(0).Substring(1, array(0).Length - 2)
			End If
			Dim array2 As String() = text.Split(New Char() { "-"c })
			If array2.Length <= 2 Then
				For i As Integer = 0 To array2.Length - 1
					Dim array3 As Char() = array2(i).ToCharArray()
					Dim num As Integer = 0
					For j As Integer = 0 To array3.Length - 1
						If array3(j) <> vbNullChar Then
							num = j
							Exit For
						End If
					Next
					If i = 0 Then
						Parser.IntTryParse(array2(i).Substring(num), textSegment.frameStart)
					Else
						Parser.IntTryParse(array2(i).Substring(num), textSegment.frameEnd)
					End If
				Next
			Else
				Debug.LogError("Syntax Error: There can't be more than one hyphen in a segment", Nothing)
			End If
			If text = "#" Then
				textSegment.isBlank = True
			End If
			Return textSegment
		End If
		Debug.LogError("There can't be more than one x in a segment", Nothing)
		Return Nothing
	End Function

	' Token: 0x06000B48 RID: 2888 RVA: 0x00082A8C File Offset: 0x00080E8C
	Public Function GetFrames(spritesChosen As List(Of ImageData), textSegments As TextSegment()) As List(Of Integer)
		Dim flag As Boolean = False
		Dim list As List(Of Integer) = New List(Of Integer)()
		For i As Integer = 0 To textSegments.Length - 1
			If textSegments(i).frameStart > textSegments(i).frameEnd Then
				flag = True
			End If
			Dim num As Integer
			If textSegments(i).frameEnd = 0 AndAlso Not textSegments(i).isBlank Then
				num = 1
			Else
				num = If((Not flag), (textSegments(i).frameEnd - textSegments(i).frameStart), (textSegments(i).frameStart - textSegments(i).frameEnd)) + 1
			End If
			Dim num2 As Integer = If((textSegments(i).multiplier <> 0), textSegments(i).multiplier, 1)
			Dim num3 As Integer = 0
			For j As Integer = 0 To num2 - 1
				For k As Integer = 0 To num - 1
					For l As Integer = 0 To spritesChosen.Count - 1
						If textSegments(i).isBlank Then
							num3 = -1
						End If
						If spritesChosen(l).frameNum = textSegments(i).frameStart Then
							If k = 0 Then
								num3 = spritesChosen(l).frameNum - 1
							ElseIf num3 < textSegments(i).frameEnd - 1 Then
								num3 += 1
							ElseIf num3 > textSegments(i).frameEnd - 1 Then
								num3 -= 1
							End If
						End If
					Next
					list.Add(num3)
				Next
			Next
		Next
		Return list
	End Function

	' Token: 0x06000B49 RID: 2889 RVA: 0x00082C0C File Offset: 0x0008100C
	Public Function GetFrameNumber(path As String) As Integer
		Dim num As Integer = 0
		Dim array As Char() = path.ToCharArray()
		Dim num2 As Integer = 0
		Dim num3 As Integer = 0
		For i As Integer = array.Length - 1 To 0 + 1 Step -1
			If array(i) = "."c Then
				num3 = array.Length - i
			End If
			If array(i) = "0"c AndAlso array(i - 1) = "_"c Then
				num2 = i
				Exit For
			End If
		Next
		Dim text As String = path.Substring(num2, num3)
		array = text.ToCharArray()
		For j As Integer = 0 To array.Length - 1
			If array(j) <> "0"c Then
				num2 = j
				Exit For
			End If
		Next
		Parser.IntTryParse(text.Substring(num2), num)
		Return num
	End Function
End Module
