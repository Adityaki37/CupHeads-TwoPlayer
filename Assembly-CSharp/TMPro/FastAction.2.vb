Imports System
Imports System.Collections.Generic

Namespace TMPro
	' Token: 0x02000C61 RID: 3169
	Public Class FastAction(Of A)
		' Token: 0x06004ED8 RID: 20184 RVA: 0x0027AC8C File Offset: 0x0027908C
		Public Sub Add(rhs As Action(Of A))
			If Me.lookup.ContainsKey(rhs) Then
				Return
			End If
			Me.lookup(rhs) = Me.delegates.AddLast(rhs)
		End Sub

		' Token: 0x06004ED9 RID: 20185 RVA: 0x0027ACB8 File Offset: 0x002790B8
		Public Sub Remove(rhs As Action(Of A))
			Dim linkedListNode As LinkedListNode(Of Action(Of A))
			If Me.lookup.TryGetValue(rhs, linkedListNode) Then
				Me.lookup.Remove(rhs)
				Me.delegates.Remove(linkedListNode)
			End If
		End Sub

		' Token: 0x06004EDA RID: 20186 RVA: 0x0027ACF4 File Offset: 0x002790F4
		Public Sub [Call](a As A)
			Dim linkedListNode As LinkedListNode(Of Action(Of A)) = Me.delegates.First
			While linkedListNode IsNot Nothing
				linkedListNode.Value(a)
				linkedListNode = linkedListNode.[Next]
			End While
		End Sub

		' Token: 0x04005200 RID: 20992
		Private delegates As LinkedList(Of Action(Of A)) = New LinkedList(Of Action(Of A))()

		' Token: 0x04005201 RID: 20993
		Private lookup As Dictionary(Of Action(Of A), LinkedListNode(Of Action(Of A))) = New Dictionary(Of Action(Of A), LinkedListNode(Of Action(Of A)))()
	End Class
End Namespace
