Imports System
Imports System.Collections.Generic

' Token: 0x0200037D RID: 893
<Serializable()>
Public Class KeyValue
	' Token: 0x06000A6C RID: 2668 RVA: 0x0007EB63 File Offset: 0x0007CF63
	Public Sub New()
	End Sub

	' Token: 0x06000A6D RID: 2669 RVA: 0x0007EB76 File Offset: 0x0007CF76
	Public Sub New(key As String, value As Single)
		Me.key = key
		Me.value = value
	End Sub

	' Token: 0x06000A6E RID: 2670 RVA: 0x0007EB98 File Offset: 0x0007CF98
	Public Shared Function ListFromString(keyValueString As String, allowedCharacters As Char()) As KeyValue()
		Dim list As List(Of KeyValue) = New List(Of KeyValue)()
		Dim list2 As List(Of Char) = New List(Of Char)(allowedCharacters)
		list2.Add(","c)
		list2.Add(":"c)
		keyValueString.Replace(" ", String.Empty)
		For i As Integer = 0 To keyValueString.Length - 1
			Dim flag As Boolean = True
			For Each c As Char In list2
				If keyValueString(i) = c Then
					flag = False
				End If
			Next
			If flag Then
				keyValueString.Remove(i, 1)
			End If
		Next
		Dim array As String() = keyValueString.Split(New Char() { ","c })
		For j As Integer = 0 To array.Length - 1
			Dim array2 As String() = array(j).Split(New Char() { ":"c })
			If array2.Length = 2 Then
				Dim text As String = array2(0).Replace(" ", String.Empty)
				Dim num As Single = 0F
				Dim flag2 As Boolean = Parser.FloatTryParse(array2(1), num)
				If flag2 AndAlso text IsNot Nothing AndAlso Not(text = String.Empty) Then
					list.Add(New KeyValue(text, num))
				End If
			End If
		Next
		Return list.ToArray()
	End Function

	' Token: 0x06000A6F RID: 2671 RVA: 0x0007ED04 File Offset: 0x0007D104
	Public Function Clone() As KeyValue
		Return New KeyValue(Me.key, Me.value)
	End Function

	' Token: 0x0400146D RID: 5229
	Public Const PAIR_SEPARATOR As Char = ","c

	' Token: 0x0400146E RID: 5230
	Public Const VALUE_SEPARATOR As Char = ":"c

	' Token: 0x0400146F RID: 5231
	Public key As String = String.Empty

	' Token: 0x04001470 RID: 5232
	Public value As Single
End Class
