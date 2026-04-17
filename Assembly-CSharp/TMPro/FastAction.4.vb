Imports System
Imports System.Collections.Generic

Namespace TMPro
	' Token: 0x02000C63 RID: 3171
	Public Class FastAction(Of A, B, C)
		' Token: 0x06004EE0 RID: 20192 RVA: 0x0027AE0A File Offset: 0x0027920A
		Public Sub Add(rhs As Action(Of A, B, C))
			If Me.lookup.ContainsKey(rhs) Then
				Return
			End If
			Me.lookup(rhs) = Me.delegates.AddLast(rhs)
		End Sub

		' Token: 0x06004EE1 RID: 20193 RVA: 0x0027AE38 File Offset: 0x00279238
		Public Sub Remove(rhs As Action(Of A, B, C))
			Dim linkedListNode As LinkedListNode(Of Action(Of A, B, C))
			If Me.lookup.TryGetValue(rhs, linkedListNode) Then
				Me.lookup.Remove(rhs)
				Me.delegates.Remove(linkedListNode)
			End If
		End Sub

		' Token: 0x06004EE2 RID: 20194 RVA: 0x0027AE74 File Offset: 0x00279274
		Public Sub [Call](a As A, b As B, c As C)
			Dim linkedListNode As LinkedListNode(Of Action(Of A, B, C)) = Me.delegates.First
			While linkedListNode IsNot Nothing
				linkedListNode.Value(a, b, c)
				linkedListNode = linkedListNode.[Next]
			End While
		End Sub

		' Token: 0x04005204 RID: 20996
		Private delegates As LinkedList(Of Action(Of A, B, C)) = New LinkedList(Of Action(Of A, B, C))()

		' Token: 0x04005205 RID: 20997
		Private lookup As Dictionary(Of Action(Of A, B, C), LinkedListNode(Of Action(Of A, B, C))) = New Dictionary(Of Action(Of A, B, C), LinkedListNode(Of Action(Of A, B, C)))()
	End Class
End Namespace
