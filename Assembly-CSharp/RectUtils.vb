Imports System
Imports UnityEngine

' Token: 0x02000398 RID: 920
Public Class RectUtils
	' Token: 0x06000B35 RID: 2869 RVA: 0x000824D9 File Offset: 0x000808D9
	Public Shared Function OffsetRect(rect As Rect, offset As Integer) As Rect
		Return New Rect(rect.x - CSng(offset), rect.y - CSng(offset), rect.width + CSng((offset * 2)), rect.height + CSng((offset * 2)))
	End Function

	' Token: 0x06000B36 RID: 2870 RVA: 0x0008250C File Offset: 0x0008090C
	Public Shared Function HorizontalDivide(rect As Rect, sections As Integer, space As Single) As Rect()
		Dim array As Rect() = New Rect(sections - 1) {}
		Dim num As Single = rect.width / CSng(sections) - space * CSng((sections - 1)) / CSng(sections)
		For i As Integer = 0 To sections - 1
			array(i) = New Rect(rect.x + num * CSng(i) + space * CSng(i), rect.y, num, rect.height)
		Next
		Return array
	End Function

	' Token: 0x06000B37 RID: 2871 RVA: 0x00082579 File Offset: 0x00080979
	Public Shared Function NewFromCenter(xCenter As Single, yCenter As Single, width As Single, height As Single) As Rect
		Return New Rect(xCenter - width * 0.5F, yCenter - height * 0.5F, width, height)
	End Function
End Class
