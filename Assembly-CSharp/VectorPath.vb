Imports System
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x02000381 RID: 897
<Serializable()>
Public Class VectorPath
	' Token: 0x06000A90 RID: 2704 RVA: 0x0007F074 File Offset: 0x0007D474
	Public Shared Function Lerp(path As VectorPath, t As Single) As Vector3
		If path Is Nothing OrElse path._points.Count < 1 Then
			Return Vector3.zero
		End If
		If path._points.Count = 1 Then
			Return path._points(0)
		End If
		If path._points.Count = 2 Then
			Return Vector3.Lerp(path._points(0), path._points(1), t)
		End If
		Dim vector As Vector3 = Nothing
		Dim num As Integer = 0
		If path.Distance < 0F Then
			path.Calculate()
		End If
		For i As Integer = 0 To path.infoNodes.Count - 1 - 1
			num = i
			If path.infoNodes(i + 1).distance > t Then
				Exit For
			End If
		Next
		Dim vector2 As Vector3 = path.infoNodes(num)
		Dim vector3 As Vector3 = path.infoNodes(num + 1)
		Dim distance As Single = path.infoNodes(num).distance
		Dim distance2 As Single = path.infoNodes(num + 1).distance
		Dim num2 As Single = (t - distance) / (distance2 - distance)
		Return Vector3.Lerp(path.infoNodes(num), path.infoNodes(num + 1), num2)
	End Function

	' Token: 0x170001FD RID: 509
	' (get) Token: 0x06000A91 RID: 2705 RVA: 0x0007F1DE File Offset: 0x0007D5DE
	Public ReadOnly Property Points As List(Of Vector3)
		Get
			Return Me._points
		End Get
	End Property

	' Token: 0x170001FE RID: 510
	' (get) Token: 0x06000A92 RID: 2706 RVA: 0x0007F1E6 File Offset: 0x0007D5E6
	' (set) Token: 0x06000A93 RID: 2707 RVA: 0x0007F1EE File Offset: 0x0007D5EE
	Public Property Closed As Boolean
		Get
			Return Me._closed
		End Get
		Set(value As Boolean)
			Me._closed = value
			Me.Calculate()
		End Set
	End Property

	' Token: 0x170001FF RID: 511
	' (get) Token: 0x06000A94 RID: 2708 RVA: 0x0007F1FD File Offset: 0x0007D5FD
	Public ReadOnly Property Distance As Single
		Get
			If Me._distance < 0F Then
				Me.Calculate()
			End If
			Return Me._distance
		End Get
	End Property

	' Token: 0x17000200 RID: 512
	' (get) Token: 0x06000A95 RID: 2709 RVA: 0x0007F21B File Offset: 0x0007D61B
	Public ReadOnly Property infoNodes As List(Of VectorPath.Node)
		Get
			If Me.__infoNodes Is Nothing Then
				Me.Calculate()
			End If
			Return Me.__infoNodes
		End Get
	End Property

	' Token: 0x06000A96 RID: 2710 RVA: 0x0007F234 File Offset: 0x0007D634
	Private Sub Calculate()
		Me.__infoNodes = VectorPath.Node.NewList(Me._points)
		If Me._closed Then
			Me.infoNodes.Add(New VectorPath.Node(Me._points(0)))
		End If
		Me._distance = 0F
		For i As Integer = 1 To Me.infoNodes.Count - 1
			Me._distance += Vector3.Distance(Me.infoNodes(i - 1), Me.infoNodes(i))
		Next
		Dim num As Single = 0F
		For j As Integer = 1 To Me.infoNodes.Count - 1
			num += Vector3.Distance(Me.infoNodes(j - 1), Me.infoNodes(j))
			Dim node As VectorPath.Node = Me.infoNodes(j)
			node.distance = num / Me._distance
			Me.infoNodes(j) = node
		Next
	End Sub

	' Token: 0x06000A97 RID: 2711 RVA: 0x0007F34D File Offset: 0x0007D74D
	Public Function Lerp(t As Single) As Vector3
		Return VectorPath.Lerp(Me, t)
	End Function

	' Token: 0x06000A98 RID: 2712 RVA: 0x0007F358 File Offset: 0x0007D758
	Public Function GetClosestPoint(originalPosition As Vector2, positionToCheck As Vector2, moveX As Boolean, moveY As Boolean) As Vector2
		Dim vector As Vector2 = originalPosition
		Dim num As Single = Single.MaxValue
		Dim vector2 As Vector2 = Vector2.zero
		Dim vector3 As Vector2 = Vector2.zero
		Dim vector4 As Vector2 = Vector2.zero
		Dim vector5 As Vector2 = Vector2.zero
		Dim vector6 As Vector2 = Vector2.zero
		For i As Integer = 0 To Me.Points.Count - 1 - 1
			vector3 = Me.Points(i)
			vector4 = Me.Points(i + 1)
			vector5 = positionToCheck - vector3
			vector6 = vector4 - vector3
			If moveX Then
				Dim num2 As Single = vector5.x / vector6.x
				If num2 < 0F Then
					vector2 = vector3
				ElseIf num2 > 1F Then
					vector2 = vector4
				Else
					vector2 = vector3 + vector6 * num2
				End If
				Dim num3 As Single = Vector2.Distance(positionToCheck, vector2)
				If num3 <= num Then
					num = num3
					vector = vector2
				End If
			End If
			If moveY Then
				Dim num2 As Single = vector5.y / vector6.y
				If num2 < 0F Then
					vector2 = vector3
				ElseIf num2 > 1F Then
					vector2 = vector4
				Else
					vector2 = vector3 + vector6 * num2
				End If
				Dim num3 As Single = Vector2.Distance(positionToCheck, vector2)
				If num3 <= num Then
					num = num3
					vector = vector2
				End If
			End If
		Next
		Return vector
	End Function

	' Token: 0x06000A99 RID: 2713 RVA: 0x0007F4C8 File Offset: 0x0007D8C8
	Public Function GetClosestNormalizedPoint(originalPosition As Vector2, positionToCheck As Vector2, moveX As Boolean, moveY As Boolean) As Single
		Dim vector As Vector2 = originalPosition
		Dim num As Single = Single.MaxValue
		Dim vector2 As Vector2 = Vector2.zero
		Dim node As VectorPath.Node = Vector2.zero
		Dim node2 As VectorPath.Node = Vector2.zero
		Dim vector3 As Vector2 = Vector2.zero
		Dim vector4 As Vector2 = Vector2.zero
		Dim node3 As VectorPath.Node = Vector2.zero
		Dim node4 As VectorPath.Node = Vector2.zero
		For i As Integer = 0 To Me.Points.Count - 1 - 1
			node = Me.infoNodes(i)
			node2 = Me.infoNodes(i + 1)
			vector3 = positionToCheck - node.position
			vector4 = node2.position - node.position
			If moveX Then
				Dim num2 As Single = vector3.x / vector4.x
				If num2 < 0F Then
					vector2 = node
				ElseIf num2 > 1F Then
					vector2 = node2
				Else
					vector2 = node + vector4 * num2
				End If
				Dim num3 As Single = Vector2.Distance(positionToCheck, vector2)
				If num3 <= num Then
					num = num3
					vector = vector2
					node3 = node
					node4 = node2
				End If
			End If
			If moveY Then
				Dim num2 As Single = vector3.y / vector4.y
				If num2 < 0F Then
					vector2 = node
				ElseIf num2 > 1F Then
					vector2 = node2
				Else
					vector2 = node + vector4 * num2
				End If
				Dim num3 As Single = Vector2.Distance(positionToCheck, vector2)
				If num3 <= num Then
					num = num3
					vector = vector2
					node3 = node
					node4 = node2
				End If
			End If
		Next
		Dim num4 As Single = Vector2.Distance(node3.position, node4.position)
		Dim num5 As Single = Vector2.Distance(node3.position, vector)
		Return Mathf.Lerp(node3.distance, node4.distance, num5 / num4)
	End Function

	' Token: 0x06000A9A RID: 2714 RVA: 0x0007F6E8 File Offset: 0x0007DAE8
	Public Sub DrawGizmos(offset As Vector3)
		Me.DrawGizmos(1F, offset)
	End Sub

	' Token: 0x06000A9B RID: 2715 RVA: 0x0007F6F8 File Offset: 0x0007DAF8
	Public Sub DrawGizmos(a As Single, offset As Vector3)
		For i As Integer = 0 To Me._points.Count - 1
			Gizmos.color = New Color(0F, 0F, 1F, a)
			Gizmos.DrawWireSphere(Me._points(i) + offset, 10F)
			If i < Me._points.Count - 1 Then
				Gizmos.color = New Color(0F, 0F, 1F, a)
				Dim vector As Vector3 = Me._points(i) + offset
				Dim vector2 As Vector3 = Me._points(i + 1) + offset
				Gizmos.DrawLine(vector, vector2)
				Dim vector3 As Vector3 = Vector3.Lerp(vector, vector2, 0.45F)
				Dim vector4 As Vector3 = Vector3.Lerp(vector, vector2, 0.55F)
				Dim vector5 As Vector3 = Quaternion.Euler(0F, 0F, 90F) * (vector2 - vector).normalized * 10F
				Gizmos.color = New Color(0F, 1F, 0F, a)
				Gizmos.DrawLine(vector3 + vector5, vector4)
				Gizmos.DrawLine(vector3 - vector5, vector4)
			End If
		Next
		If Me.Closed Then
			Gizmos.color = New Color(0F, 1F, 0F, a * 0.5F)
			Gizmos.DrawLine(Me._points(Me._points.Count - 1) + offset, Me._points(0) + offset)
		End If
	End Sub

	' Token: 0x04001476 RID: 5238
	<SerializeField()>
	Private _points As List(Of Vector3) = New List(Of Vector3)() From { New Vector2(-100F, 0F), New Vector2(100F, 0F) }

	' Token: 0x04001477 RID: 5239
	<SerializeField()>
	Private _closed As Boolean

	' Token: 0x04001478 RID: 5240
	Private _distance As Single = -1F

	' Token: 0x04001479 RID: 5241
	Private __infoNodes As List(Of VectorPath.Node)

	' Token: 0x02000382 RID: 898
	Public Structure Node
		' Token: 0x06000A9C RID: 2716 RVA: 0x0007F89B File Offset: 0x0007DC9B
		Public Sub New(v As Vector3)
			Me.x = v.x
			Me.y = v.y
			Me.z = v.z
			Me.distance = 0F
		End Sub

		' Token: 0x17000201 RID: 513
		' (get) Token: 0x06000A9D RID: 2717 RVA: 0x0007F8CF File Offset: 0x0007DCCF
		Public ReadOnly Property position As Vector3
			Get
				Return New Vector3(Me.x, Me.y, Me.z)
			End Get
		End Property

		' Token: 0x06000A9E RID: 2718 RVA: 0x0007F8E8 File Offset: 0x0007DCE8
		Public Shared Function NewList(oldList As List(Of Vector3)) As List(Of VectorPath.Node)
			Dim list As List(Of VectorPath.Node) = New List(Of VectorPath.Node)(oldList.Count)
			For i As Integer = 0 To oldList.Count - 1
				list.Add(New VectorPath.Node(oldList(i)))
			Next
			Return list
		End Function

		' Token: 0x06000A9F RID: 2719 RVA: 0x0007F92B File Offset: 0x0007DD2B
		Public Shared Widening Operator CType(v As Vector2) As VectorPath.Node
			Return New VectorPath.Node(v)
		End Operator

		' Token: 0x06000AA0 RID: 2720 RVA: 0x0007F938 File Offset: 0x0007DD38
		Public Shared Widening Operator CType(t As VectorPath.Node) As Vector2
			Return New Vector2(t.x, t.y)
		End Operator

		' Token: 0x06000AA1 RID: 2721 RVA: 0x0007F94D File Offset: 0x0007DD4D
		Public Shared Widening Operator CType(v As Vector3) As VectorPath.Node
			Return New VectorPath.Node(v)
		End Operator

		' Token: 0x06000AA2 RID: 2722 RVA: 0x0007F955 File Offset: 0x0007DD55
		Public Shared Widening Operator CType(t As VectorPath.Node) As Vector3
			Return New Vector3(t.x, t.y, t.z)
		End Operator

		' Token: 0x0400147A RID: 5242
		Public x As Single

		' Token: 0x0400147B RID: 5243
		Public y As Single

		' Token: 0x0400147C RID: 5244
		Public z As Single

		' Token: 0x0400147D RID: 5245
		Public distance As Single
	End Structure
End Class
