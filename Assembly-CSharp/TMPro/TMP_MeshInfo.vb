Imports System
Imports UnityEngine

Namespace TMPro
	' Token: 0x02000CA3 RID: 3235
	Public Structure TMP_MeshInfo
		' Token: 0x06005189 RID: 20873 RVA: 0x002998DC File Offset: 0x00297CDC
		Public Sub New(mesh As Mesh, size As Integer)
			If mesh Is Nothing Then
				mesh = New Mesh()
			Else
				mesh.Clear()
			End If
			Me.mesh = mesh
			Dim num As Integer = size * 4
			Dim num2 As Integer = size * 6
			Me.vertexCount = 0
			Me.vertices = New Vector3(num - 1) {}
			Me.uvs0 = New Vector2(num - 1) {}
			Me.uvs2 = New Vector2(num - 1) {}
			Me.colors32 = New Color32(num - 1) {}
			Me.normals = New Vector3(num - 1) {}
			Me.tangents = New Vector4(num - 1) {}
			Me.triangles = New Integer(num2 - 1) {}
			Dim num3 As Integer = 0
			Dim num4 As Integer = 0
			While num4 / 4 < size
				For i As Integer = 0 To 4 - 1
					Me.vertices(num4 + i) = Vector3.zero
					Me.uvs0(num4 + i) = Vector2.zero
					Me.uvs2(num4 + i) = Vector2.zero
					Me.colors32(num4 + i) = TMP_MeshInfo.s_DefaultColor
					Me.normals(num4 + i) = TMP_MeshInfo.s_DefaultNormal
					Me.tangents(num4 + i) = TMP_MeshInfo.s_DefaultTangent
				Next
				Me.triangles(num3) = num4
				Me.triangles(num3 + 1) = num4 + 1
				Me.triangles(num3 + 2) = num4 + 2
				Me.triangles(num3 + 3) = num4 + 2
				Me.triangles(num3 + 4) = num4 + 3
				Me.triangles(num3 + 5) = num4
				num4 += 4
				num3 += 6
			End While
			Me.mesh.vertices = Me.vertices
			Me.mesh.normals = Me.normals
			Me.mesh.tangents = Me.tangents
			Me.mesh.triangles = Me.triangles
			Me.mesh.bounds = New Bounds(Vector3.zero, New Vector3(3840F, 2160F, 0F))
		End Sub

		' Token: 0x0600518A RID: 20874 RVA: 0x00299AEC File Offset: 0x00297EEC
		Public Sub ResizeMeshInfo(size As Integer)
			Dim num As Integer = size * 4
			Dim num2 As Integer = size * 6
			Dim num3 As Integer = Me.vertices.Length / 4
			Array.Resize(Of Vector3)(Me.vertices, num)
			Array.Resize(Of Vector3)(Me.normals, num)
			Array.Resize(Of Vector4)(Me.tangents, num)
			Array.Resize(Of Vector2)(Me.uvs0, num)
			Array.Resize(Of Vector2)(Me.uvs2, num)
			Array.Resize(Of Color32)(Me.colors32, num)
			Array.Resize(Of Integer)(Me.triangles, num2)
			If size <= num3 Then
				Return
			End If
			For i As Integer = num3 To size - 1
				Dim num4 As Integer = i * 4
				Dim num5 As Integer = i * 6
				Me.normals(num4) = TMP_MeshInfo.s_DefaultNormal
				Me.normals(1 + num4) = TMP_MeshInfo.s_DefaultNormal
				Me.normals(2 + num4) = TMP_MeshInfo.s_DefaultNormal
				Me.normals(3 + num4) = TMP_MeshInfo.s_DefaultNormal
				Me.tangents(num4) = TMP_MeshInfo.s_DefaultTangent
				Me.tangents(1 + num4) = TMP_MeshInfo.s_DefaultTangent
				Me.tangents(2 + num4) = TMP_MeshInfo.s_DefaultTangent
				Me.tangents(3 + num4) = TMP_MeshInfo.s_DefaultTangent
				Me.triangles(num5) = num4
				Me.triangles(1 + num5) = 1 + num4
				Me.triangles(2 + num5) = 2 + num4
				Me.triangles(3 + num5) = 2 + num4
				Me.triangles(4 + num5) = 3 + num4
				Me.triangles(5 + num5) = num4
			Next
			Me.mesh.vertices = Me.vertices
			Me.mesh.normals = Me.normals
			Me.mesh.tangents = Me.tangents
			Me.mesh.triangles = Me.triangles
		End Sub

		' Token: 0x0600518B RID: 20875 RVA: 0x00299CE0 File Offset: 0x002980E0
		Public Sub Clear()
			If Me.vertices Is Nothing Then
				Return
			End If
			Array.Clear(Me.vertices, 0, Me.vertices.Length)
			Me.vertexCount = 0
			If Me.mesh IsNot Nothing Then
				Me.mesh.vertices = Me.vertices
			End If
		End Sub

		' Token: 0x0600518C RID: 20876 RVA: 0x00299D38 File Offset: 0x00298138
		Public Sub Clear(uploadChanges As Boolean)
			If Me.vertices Is Nothing Then
				Return
			End If
			Array.Clear(Me.vertices, 0, Me.vertices.Length)
			Me.vertexCount = 0
			If uploadChanges AndAlso Me.mesh IsNot Nothing Then
				Me.mesh.vertices = Me.vertices
			End If
		End Sub

		' Token: 0x0600518D RID: 20877 RVA: 0x00299D94 File Offset: 0x00298194
		Public Sub ClearUnusedVertices()
			Dim num As Integer = Me.vertices.Length - Me.vertexCount
			If num > 0 Then
				Array.Clear(Me.vertices, Me.vertexCount, num)
			End If
		End Sub

		' Token: 0x0600518E RID: 20878 RVA: 0x00299DCC File Offset: 0x002981CC
		Public Sub ClearUnusedVertices(startIndex As Integer)
			Dim num As Integer = Me.vertices.Length - startIndex
			If num > 0 Then
				Array.Clear(Me.vertices, startIndex, num)
			End If
		End Sub

		' Token: 0x0600518F RID: 20879 RVA: 0x00299DF8 File Offset: 0x002981F8
		Public Sub ClearUnusedVertices(startIndex As Integer, updateMesh As Boolean)
			Dim num As Integer = Me.vertices.Length - startIndex
			If num > 0 Then
				Array.Clear(Me.vertices, startIndex, num)
			End If
			If updateMesh AndAlso Me.mesh IsNot Nothing Then
				Me.mesh.vertices = Me.vertices
			End If
		End Sub

		' Token: 0x04005446 RID: 21574
		Private Shared s_DefaultColor As Color32 = New Color32(Byte.MaxValue, Byte.MaxValue, Byte.MaxValue, Byte.MaxValue)

		' Token: 0x04005447 RID: 21575
		Private Shared s_DefaultNormal As Vector3 = New Vector3(0F, 0F, -1F)

		' Token: 0x04005448 RID: 21576
		Private Shared s_DefaultTangent As Vector4 = New Vector4(-1F, 0F, 0F, 1F)

		' Token: 0x04005449 RID: 21577
		Public mesh As Mesh

		' Token: 0x0400544A RID: 21578
		Public vertexCount As Integer

		' Token: 0x0400544B RID: 21579
		Public vertices As Vector3()

		' Token: 0x0400544C RID: 21580
		Public normals As Vector3()

		' Token: 0x0400544D RID: 21581
		Public tangents As Vector4()

		' Token: 0x0400544E RID: 21582
		Public uvs0 As Vector2()

		' Token: 0x0400544F RID: 21583
		Public uvs2 As Vector2()

		' Token: 0x04005450 RID: 21584
		Public colors32 As Color32()

		' Token: 0x04005451 RID: 21585
		Public triangles As Integer()
	End Structure
End Namespace
