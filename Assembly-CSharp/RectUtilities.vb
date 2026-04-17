Imports System
Imports UnityEngine

' Token: 0x02000397 RID: 919
Public Module RectUtilities
	' Token: 0x06000B2B RID: 2859 RVA: 0x000821E1 File Offset: 0x000805E1
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Function AdjustSize(rect As Rect, left As Single, right As Single, top As Single, bottom As Single) As Rect
		rect.xMin += left
		rect.xMax += right
		rect.yMin += top
		rect.yMax += bottom
		Return rect
	End Function

	' Token: 0x06000B2C RID: 2860 RVA: 0x00082224 File Offset: 0x00080624
	Public Function SliceLeft(ByRef rect As Rect, amount As Single) As Rect
		Dim rect2 As Rect = New Rect(rect)
		rect2.xMax = rect2.xMin + amount
		rect.xMin += amount
		Return rect2
	End Function

	' Token: 0x06000B2D RID: 2861 RVA: 0x00082260 File Offset: 0x00080660
	Public Function SliceRight(ByRef rect As Rect, amount As Single) As Rect
		Dim rect2 As Rect = New Rect(rect)
		rect2.xMin = rect2.xMax - amount
		rect.xMax -= amount
		Return rect2
	End Function

	' Token: 0x06000B2E RID: 2862 RVA: 0x0008229C File Offset: 0x0008069C
	Public Function SliceTop(ByRef rect As Rect, amount As Single) As Rect
		Dim rect2 As Rect = New Rect(rect)
		rect2.yMax = rect2.yMin + amount
		rect.yMin += amount
		Return rect2
	End Function

	' Token: 0x06000B2F RID: 2863 RVA: 0x000822D8 File Offset: 0x000806D8
	Public Function SliceBottom(ByRef rect As Rect, amount As Single) As Rect
		Dim rect2 As Rect = New Rect(rect)
		rect2.yMin = rect2.yMax - amount
		rect.yMax -= amount
		Return rect2
	End Function

	' Token: 0x06000B30 RID: 2864 RVA: 0x00082314 File Offset: 0x00080714
	Public Function SplitVertical(rect As Rect, numberOfGeneratedRects As Integer) As Rect()
		Dim array As Single() = New Single(numberOfGeneratedRects - 1) {}
		For i As Integer = 0 To array.Length - 1
			array(i) = 1F
		Next
		Return RectUtilities.SplitVertical(rect, array)
	End Function

	' Token: 0x06000B31 RID: 2865 RVA: 0x0008234C File Offset: 0x0008074C
	Public Function SplitVertical(rect As Rect, ParamArray weights As Single()) As Rect()
		Dim totalWeight As Single = 0F
		Array.ForEach(Of Single)(weights, Sub(weight As Single)
			totalWeight += weight
		End Sub)
		Dim array As Rect() = New Rect(weights.Length - 1) {}
		Dim height As Single = rect.height
		For i As Integer = 0 To weights.Length - 1 - 1
			array(i) = RectUtilities.SliceTop(rect, Mathf.Floor(height * weights(i) / totalWeight))
		Next
		array(array.Length - 1) = rect
		Return array
	End Function

	' Token: 0x06000B32 RID: 2866 RVA: 0x000823DC File Offset: 0x000807DC
	Public Function SplitHorizontal(rect As Rect, numberOfGeneratedRects As Integer) As Rect()
		Dim array As Single() = New Single(numberOfGeneratedRects - 1) {}
		For i As Integer = 0 To array.Length - 1
			array(i) = 1F
		Next
		Return RectUtilities.SplitHorizontal(rect, array)
	End Function

	' Token: 0x06000B33 RID: 2867 RVA: 0x00082414 File Offset: 0x00080814
	Public Function SplitHorizontal(rect As Rect, ParamArray weights As Single()) As Rect()
		Dim totalWeight As Single = 0F
		Array.ForEach(Of Single)(weights, Sub(weight As Single)
			totalWeight += weight
		End Sub)
		Dim array As Rect() = New Rect(weights.Length - 1) {}
		Dim width As Single = rect.width
		For i As Integer = 0 To weights.Length - 1 - 1
			array(i) = RectUtilities.SliceLeft(rect, Mathf.Floor(width * weights(i) / totalWeight))
		Next
		array(array.Length - 1) = rect
		Return array
	End Function
End Module
