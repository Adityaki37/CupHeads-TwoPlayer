Imports System
Imports System.Collections.Generic

Namespace TMPro
	' Token: 0x02000C62 RID: 3170
	Public Class FastAction(Of A, B)
		' Token: 0x06004EDC RID: 20188 RVA: 0x0027AD49 File Offset: 0x00279149
		Public Sub Add(rhs As Action(Of A, B))
			If Me.lookup.ContainsKey(rhs) Then
				Return
			End If
			Me.lookup(rhs) = Me.delegates.AddLast(rhs)
		End Sub

		' Token: 0x06004EDD RID: 20189 RVA: 0x0027AD78 File Offset: 0x00279178
		Public Sub Remove(rhs As Action(Of A, B))
			Dim linkedListNode As LinkedListNode(Of Action(Of A, B))
			If Me.lookup.TryGetValue(rhs, linkedListNode) Then
				Me.lookup.Remove(rhs)
				Me.delegates.Remove(linkedListNode)
			End If
		End Sub

		' Token: 0x06004EDE RID: 20190 RVA: 0x0027ADB4 File Offset: 0x002791B4
		Public Sub [Call](a As A, b As B)
			Dim linkedListNode As LinkedListNode(Of Action(Of A, B)) = Me.delegates.First
			While linkedListNode IsNot Nothing
				linkedListNode.Value(a, b)
				linkedListNode = linkedListNode.[Next]
			End While
		End Sub

		' Token: 0x04005202 RID: 20994
		Private delegates As LinkedList(Of Action(Of A, B)) = New LinkedList(Of Action(Of A, B))()

		' Token: 0x04005203 RID: 20995
		Private lookup As Dictionary(Of Action(Of A, B), LinkedListNode(Of Action(Of A, B))) = New Dictionary(Of Action(Of A, B), LinkedListNode(Of Action(Of A, B)))()
	End Class
End Namespace
