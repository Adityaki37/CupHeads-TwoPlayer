Imports System
Imports System.Collections.Generic

Namespace DialoguerCore
	' Token: 0x02000B72 RID: 2930
	<Serializable()>
	Public Class DialoguerVariables
		' Token: 0x060046A1 RID: 18081 RVA: 0x0024EC1F File Offset: 0x0024D01F
		Public Sub New(booleans As List(Of Boolean), floats As List(Of Single), strings As List(Of String))
			Me.booleans = booleans
			Me.floats = floats
			Me.strings = strings
		End Sub

		' Token: 0x060046A2 RID: 18082 RVA: 0x0024EC3C File Offset: 0x0024D03C
		Public Function Clone() As DialoguerVariables
			Dim list As List(Of Boolean) = New List(Of Boolean)()
			For i As Integer = 0 To Me.booleans.Count - 1
				list.Add(Me.booleans(i))
			Next
			Dim list2 As List(Of Single) = New List(Of Single)()
			For j As Integer = 0 To Me.floats.Count - 1
				list2.Add(Me.floats(j))
			Next
			Dim list3 As List(Of String) = New List(Of String)()
			For k As Integer = 0 To Me.strings.Count - 1
				list3.Add(Me.strings(k))
			Next
			Return New DialoguerVariables(list, list2, list3)
		End Function

		' Token: 0x04004C8A RID: 19594
		Public booleans As List(Of Boolean)

		' Token: 0x04004C8B RID: 19595
		Public floats As List(Of Single)

		' Token: 0x04004C8C RID: 19596
		Public strings As List(Of String)
	End Class
End Namespace
