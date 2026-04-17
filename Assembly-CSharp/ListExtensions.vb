Imports System
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x02000369 RID: 873
Public Module ListExtensions
	' Token: 0x060009C1 RID: 2497 RVA: 0x0007CC80 File Offset: 0x0007B080
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Sub Move(Of T)(list As List(Of T), index As Integer, direction As Integer)
		If direction < 0 Then
			If index = 0 Then
				Return
			End If
			Dim t As T = list(index - 1)
			list(index - 1) = list(index)
			list(index) = t
		ElseIf direction > 0 Then
			If index >= list.Count - 1 Then
				Return
			End If
			Dim t2 As T = list(index + 1)
			list(index + 1) = list(index)
			list(index) = t2
		End If
	End Sub

	' Token: 0x060009C2 RID: 2498 RVA: 0x0007CCFC File Offset: 0x0007B0FC
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Sub Shuffle(Of T)(list As IList(Of T))
		For i As Integer = 0 To list.Count - 1
			Dim num As Integer = Global.UnityEngine.Random.Range(i, list.Count)
			Dim t As T = list(i)
			list(i) = list(num)
			list(num) = t
		Next
	End Sub

	' Token: 0x060009C3 RID: 2499 RVA: 0x0007CD4B File Offset: 0x0007B14B
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Function RandomChoice(Of T)(list As IList(Of T)) As T
		Return list(Global.UnityEngine.Random.Range(0, list.Count))
	End Function
End Module
