Imports System
Imports UnityEngine

' Token: 0x02000392 RID: 914
Public Class GeometryUtils
	' Token: 0x06000B06 RID: 2822 RVA: 0x00081BC4 File Offset: 0x0007FFC4
	Public Shared Function GetCircle(center As Vector3, radius As Single, Optional axis As GeometryUtils.Axis = GeometryUtils.Axis.Y, Optional resolution As Integer = 128) As Vector3()
		Dim array As Vector3() = New Vector3(resolution - 1) {}
		Dim num As Single = 6.2831855F / CSng(resolution)
		For i As Integer = 0 To resolution - 1
			Dim num2 As Single = num * CSng(i)
			Dim num3 As Single = radius * Mathf.Cos(num2)
			Dim num4 As Single = radius * Mathf.Sin(num2)
			array(i) = New Vector3(num3, num4, 0F)
		Next
		Dim quaternion As Quaternion
		If axis = GeometryUtils.Axis.X Then
			quaternion = Quaternion.AngleAxis(90F, Vector3.up)
		ElseIf axis = GeometryUtils.Axis.Y Then
			quaternion = Quaternion.AngleAxis(90F, Vector3.right)
		Else
			quaternion = Quaternion.AngleAxis(0F, Vector3.up)
		End If
		For j As Integer = 0 To array.Length - 1
			array(j) = quaternion * array(j) + center
		Next
		Return array
	End Function

	' Token: 0x02000393 RID: 915
	Public Enum Axis
		' Token: 0x040014B8 RID: 5304
		X
		' Token: 0x040014B9 RID: 5305
		Y
		' Token: 0x040014BA RID: 5306
		Z
	End Enum
End Class
