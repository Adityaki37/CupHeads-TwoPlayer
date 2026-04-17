Imports System

' Token: 0x0200037C RID: 892
Public Class ClassStringAssembler
	' Token: 0x06000A65 RID: 2661 RVA: 0x0007EA00 File Offset: 0x0007CE00
	Public Sub New(Optional indent As Integer = 0)
		Me.indents = indent
	End Sub

	' Token: 0x06000A66 RID: 2662 RVA: 0x0007EA1A File Offset: 0x0007CE1A
	Public Sub Add(s As String)
		Me.value += s
	End Sub

	' Token: 0x06000A67 RID: 2663 RVA: 0x0007EA30 File Offset: 0x0007CE30
	Public Sub AddLine(s As String)
		Dim num As Integer = 0
		If s.Length > 0 Then
			num = s.Length - 1
		End If
		If s.Length > 0 AndAlso (s(0) = "}"c OrElse s(0) = ")"c OrElse s(0) = "]"c) Then
			Me.indents -= 1
		End If
		Me.Add(vbLf + Me.PreIndent() + s)
		If s.Length > 0 AndAlso (s(num) = "{"c OrElse s(num) = "("c OrElse s(num) = "["c) Then
			Me.indents += 1
		End If
	End Sub

	' Token: 0x06000A68 RID: 2664 RVA: 0x0007EAF3 File Offset: 0x0007CEF3
	Public Sub Break()
		Me.value += vbLf
	End Sub

	' Token: 0x06000A69 RID: 2665 RVA: 0x0007EB0B File Offset: 0x0007CF0B
	Public Sub Indent()
		Me.indents += 1
	End Sub

	' Token: 0x06000A6A RID: 2666 RVA: 0x0007EB1B File Offset: 0x0007CF1B
	Public Sub Undent()
		Me.indents -= 1
	End Sub

	' Token: 0x06000A6B RID: 2667 RVA: 0x0007EB2C File Offset: 0x0007CF2C
	Private Function PreIndent() As String
		Dim text As String = String.Empty
		For i As Integer = 0 To Me.indents - 1
			text += vbTab
		Next
		Return text
	End Function

	' Token: 0x0400146B RID: 5227
	Private indents As Integer

	' Token: 0x0400146C RID: 5228
	Public value As String = String.Empty
End Class
