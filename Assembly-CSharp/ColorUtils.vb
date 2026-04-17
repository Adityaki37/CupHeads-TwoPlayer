Imports System
Imports System.Globalization
Imports UnityEngine

' Token: 0x0200038C RID: 908
Public Class ColorUtils
	' Token: 0x06000AC4 RID: 2756 RVA: 0x0008093D File Offset: 0x0007ED3D
	Public Shared Function GetAverageColor(colors As Color()) As Color
		Return ColorUtils.GetAverageColor(colors, 1)
	End Function

	' Token: 0x06000AC5 RID: 2757 RVA: 0x00080948 File Offset: 0x0007ED48
	Public Shared Function GetAverageColor(colors As Color(), quality As Integer) As Color
		Dim num As Integer = 0
		Dim num2 As Single = 0F
		Dim num3 As Single = 0F
		Dim num4 As Single = 0F
		For i As Integer = 0 To colors.Length - 1
			If i >= colors.Length Then
				Exit For
			End If
			num2 += colors(i).r
			num3 += colors(i).g
			num4 += colors(i).b
			num += 1
		Next
		num2 /= CSng(num)
		num3 /= CSng(num)
		num4 /= CSng(num)
		Return New Color(num2, num3, num4)
	End Function

	' Token: 0x06000AC6 RID: 2758 RVA: 0x000809DC File Offset: 0x0007EDDC
	Public Shared Function ColorToHex(color As Color32, Optional alpha As Boolean = False) As String
		Dim text As String = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2")
		If alpha Then
			text += color.a.ToString("X2")
		End If
		Return text
	End Function

	' Token: 0x06000AC7 RID: 2759 RVA: 0x00080A44 File Offset: 0x0007EE44
	Public Shared Function HexToColor(hex As String) As Color
		Dim b As Byte = Parser.ByteParse(hex.Substring(0, 2), NumberStyles.HexNumber)
		Dim b2 As Byte = Parser.ByteParse(hex.Substring(2, 2), NumberStyles.HexNumber)
		Dim b3 As Byte = Parser.ByteParse(hex.Substring(4, 2), NumberStyles.HexNumber)
		Return New Color32(b, b2, b3, Byte.MaxValue)
	End Function
End Class
