Imports System
Imports UnityEngine

Namespace UnityStandardAssets.ImageEffects
	' Token: 0x02000CE3 RID: 3299
	Friend Class Quads
		' Token: 0x0600524B RID: 21067 RVA: 0x002A30DC File Offset: 0x002A14DC
		Private Shared Function HasMeshes() As Boolean
			If Quads.meshes Is Nothing Then
				Return False
			End If
			For Each mesh As Mesh In Quads.meshes
				If Nothing Is mesh Then
					Return False
				End If
			Next
			Return True
		End Function

		' Token: 0x0600524C RID: 21068 RVA: 0x002A3124 File Offset: 0x002A1524
		Public Shared Sub Cleanup()
			If Quads.meshes Is Nothing Then
				Return
			End If
			For i As Integer = 0 To Quads.meshes.Length - 1
				If Nothing IsNot Quads.meshes(i) Then
					Global.UnityEngine.[Object].DestroyImmediate(Quads.meshes(i))
					Quads.meshes(i) = Nothing
				End If
			Next
			Quads.meshes = Nothing
		End Sub

		' Token: 0x0600524D RID: 21069 RVA: 0x002A3180 File Offset: 0x002A1580
		Public Shared Function GetMeshes(totalWidth As Integer, totalHeight As Integer) As Mesh()
			If Quads.HasMeshes() AndAlso Quads.currentQuads = totalWidth * totalHeight Then
				Return Quads.meshes
			End If
			Dim num As Integer = 10833
			Dim num2 As Integer = totalWidth * totalHeight
			Quads.currentQuads = num2
			Dim num3 As Integer = Mathf.CeilToInt(1F * CSng(num2) / (1F * CSng(num)))
			Quads.meshes = New Mesh(num3 - 1) {}
			Dim num4 As Integer = 0
			For i As Integer = 0 To num2 - 1
				Dim num5 As Integer = Mathf.FloorToInt(CSng(Mathf.Clamp(num2 - i, 0, num)))
				Quads.meshes(num4) = Quads.GetMesh(num5, i, totalWidth, totalHeight)
				num4 += 1
			Next
			Return Quads.meshes
		End Function

		' Token: 0x0600524E RID: 21070 RVA: 0x002A3224 File Offset: 0x002A1624
		Private Shared Function GetMesh(triCount As Integer, triOffset As Integer, totalWidth As Integer, totalHeight As Integer) As Mesh
			Dim mesh As Mesh = New Mesh()
			mesh.hideFlags = HideFlags.DontSave
			Dim array As Vector3() = New Vector3(triCount * 4 - 1) {}
			Dim array2 As Vector2() = New Vector2(triCount * 4 - 1) {}
			Dim array3 As Vector2() = New Vector2(triCount * 4 - 1) {}
			Dim array4 As Integer() = New Integer(triCount * 6 - 1) {}
			For i As Integer = 0 To triCount - 1
				Dim num As Integer = i * 4
				Dim num2 As Integer = i * 6
				Dim num3 As Integer = triOffset + i
				Dim num4 As Single = Mathf.Floor(CSng((num3 Mod totalWidth))) / CSng(totalWidth)
				Dim num5 As Single = Mathf.Floor(CSng((num3 / totalWidth))) / CSng(totalHeight)
				Dim vector As Vector3 = New Vector3(num4 * 2F - 1F, num5 * 2F - 1F, 1F)
				array(num) = vector
				array(num + 1) = vector
				array(num + 2) = vector
				array(num + 3) = vector
				array2(num) = New Vector2(0F, 0F)
				array2(num + 1) = New Vector2(1F, 0F)
				array2(num + 2) = New Vector2(0F, 1F)
				array2(num + 3) = New Vector2(1F, 1F)
				array3(num) = New Vector2(num4, num5)
				array3(num + 1) = New Vector2(num4, num5)
				array3(num + 2) = New Vector2(num4, num5)
				array3(num + 3) = New Vector2(num4, num5)
				array4(num2) = num
				array4(num2 + 1) = num + 1
				array4(num2 + 2) = num + 2
				array4(num2 + 3) = num + 1
				array4(num2 + 4) = num + 2
				array4(num2 + 5) = num + 3
			Next
			mesh.vertices = array
			mesh.triangles = array4
			mesh.uv = array2
			mesh.uv2 = array3
			Return mesh
		End Function

		' Token: 0x040056B0 RID: 22192
		Private Shared meshes As Mesh()

		' Token: 0x040056B1 RID: 22193
		Private Shared currentQuads As Integer
	End Class
End Namespace
