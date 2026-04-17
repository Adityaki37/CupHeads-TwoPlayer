Imports System
Imports UnityEngine

Namespace UnityStandardAssets.ImageEffects
	' Token: 0x02000CF3 RID: 3315
	Friend Class Triangles
		' Token: 0x0600526E RID: 21102 RVA: 0x002A49C4 File Offset: 0x002A2DC4
		Private Shared Function HasMeshes() As Boolean
			If Triangles.meshes Is Nothing Then
				Return False
			End If
			For i As Integer = 0 To Triangles.meshes.Length - 1
				If Nothing Is Triangles.meshes(i) Then
					Return False
				End If
			Next
			Return True
		End Function

		' Token: 0x0600526F RID: 21103 RVA: 0x002A4A0C File Offset: 0x002A2E0C
		Private Shared Sub Cleanup()
			If Triangles.meshes Is Nothing Then
				Return
			End If
			For i As Integer = 0 To Triangles.meshes.Length - 1
				If Nothing IsNot Triangles.meshes(i) Then
					Global.UnityEngine.[Object].DestroyImmediate(Triangles.meshes(i))
					Triangles.meshes(i) = Nothing
				End If
			Next
			Triangles.meshes = Nothing
		End Sub

		' Token: 0x06005270 RID: 21104 RVA: 0x002A4A68 File Offset: 0x002A2E68
		Private Shared Function GetMeshes(totalWidth As Integer, totalHeight As Integer) As Mesh()
			If Triangles.HasMeshes() AndAlso Triangles.currentTris = totalWidth * totalHeight Then
				Return Triangles.meshes
			End If
			Dim num As Integer = 21666
			Dim num2 As Integer = totalWidth * totalHeight
			Triangles.currentTris = num2
			Dim num3 As Integer = Mathf.CeilToInt(1F * CSng(num2) / (1F * CSng(num)))
			Triangles.meshes = New Mesh(num3 - 1) {}
			Dim num4 As Integer = 0
			For i As Integer = 0 To num2 - 1
				Dim num5 As Integer = Mathf.FloorToInt(CSng(Mathf.Clamp(num2 - i, 0, num)))
				Triangles.meshes(num4) = Triangles.GetMesh(num5, i, totalWidth, totalHeight)
				num4 += 1
			Next
			Return Triangles.meshes
		End Function

		' Token: 0x06005271 RID: 21105 RVA: 0x002A4B0C File Offset: 0x002A2F0C
		Private Shared Function GetMesh(triCount As Integer, triOffset As Integer, totalWidth As Integer, totalHeight As Integer) As Mesh
			Dim mesh As Mesh = New Mesh()
			mesh.hideFlags = HideFlags.DontSave
			Dim array As Vector3() = New Vector3(triCount * 3 - 1) {}
			Dim array2 As Vector2() = New Vector2(triCount * 3 - 1) {}
			Dim array3 As Vector2() = New Vector2(triCount * 3 - 1) {}
			Dim array4 As Integer() = New Integer(triCount * 3 - 1) {}
			For i As Integer = 0 To triCount - 1
				Dim num As Integer = i * 3
				Dim num2 As Integer = triOffset + i
				Dim num3 As Single = Mathf.Floor(CSng((num2 Mod totalWidth))) / CSng(totalWidth)
				Dim num4 As Single = Mathf.Floor(CSng((num2 / totalWidth))) / CSng(totalHeight)
				Dim vector As Vector3 = New Vector3(num3 * 2F - 1F, num4 * 2F - 1F, 1F)
				array(num) = vector
				array(num + 1) = vector
				array(num + 2) = vector
				array2(num) = New Vector2(0F, 0F)
				array2(num + 1) = New Vector2(1F, 0F)
				array2(num + 2) = New Vector2(0F, 1F)
				array3(num) = New Vector2(num3, num4)
				array3(num + 1) = New Vector2(num3, num4)
				array3(num + 2) = New Vector2(num3, num4)
				array4(num) = num
				array4(num + 1) = num + 1
				array4(num + 2) = num + 2
			Next
			mesh.vertices = array
			mesh.triangles = array4
			mesh.uv = array2
			mesh.uv2 = array3
			Return mesh
		End Function

		' Token: 0x04005714 RID: 22292
		Private Shared meshes As Mesh()

		' Token: 0x04005715 RID: 22293
		Private Shared currentTris As Integer
	End Class
End Namespace
