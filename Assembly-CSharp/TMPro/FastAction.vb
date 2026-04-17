Imports System
Imports System.Collections.Generic

Namespace TMPro
	' Token: 0x02000C60 RID: 3168
	Public Class FastAction
		' Token: 0x06004ED4 RID: 20180 RVA: 0x0027ABCE File Offset: 0x00278FCE
		Public Sub Add(rhs As Action)
			If Me.lookup.ContainsKey(rhs) Then
				Return
			End If
			Me.lookup(rhs) = Me.delegates.AddLast(rhs)
		End Sub

		' Token: 0x06004ED5 RID: 20181 RVA: 0x0027ABFC File Offset: 0x00278FFC
		Public Sub Remove(rhs As Action)
			Dim linkedListNode As LinkedListNode(Of Action)
			If Me.lookup.TryGetValue(rhs, linkedListNode) Then
				Me.lookup.Remove(rhs)
				Me.delegates.Remove(linkedListNode)
			End If
		End Sub

		' Token: 0x06004ED6 RID: 20182 RVA: 0x0027AC38 File Offset: 0x00279038
		Public Sub [Call]()
			Dim linkedListNode As LinkedListNode(Of Action) = Me.delegates.First
			While linkedListNode IsNot Nothing
				linkedListNode.Value()
				linkedListNode = linkedListNode.[Next]
			End While
		End Sub

		' Token: 0x040051FE RID: 20990
		Private delegates As LinkedList(Of Action) = New LinkedList(Of Action)()

		' Token: 0x040051FF RID: 20991
		Private lookup As Dictionary(Of Action, LinkedListNode(Of Action)) = New Dictionary(Of Action, LinkedListNode(Of Action))()
	End Class
End Namespace
