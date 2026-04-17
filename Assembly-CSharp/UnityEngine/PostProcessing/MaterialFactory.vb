Imports System
Imports System.Collections.Generic

Namespace UnityEngine.PostProcessing
	' Token: 0x02000C01 RID: 3073
	Public NotInheritable Class MaterialFactory
		Implements IDisposable

		' Token: 0x0600495C RID: 18780 RVA: 0x0026582E File Offset: 0x00263C2E
		Public Sub New()
			Me.m_Materials = New Dictionary(Of String, Material)()
		End Sub

		' Token: 0x0600495D RID: 18781 RVA: 0x00265844 File Offset: 0x00263C44
		Public Function [Get](shaderName As String) As Material
			Dim material As Material
			If Not Me.m_Materials.TryGetValue(shaderName, material) Then
				Dim shader As Shader = Shader.Find(shaderName)
				If shader Is Nothing Then
					Throw New ArgumentException(String.Format("Shader not found ({0})", shaderName))
				End If
				material = New Material(shader) With { .name = String.Format("PostFX - {0}", shaderName.Substring(shaderName.LastIndexOf("/") + 1)), .hideFlags = HideFlags.DontSave }
				Me.m_Materials.Add(shaderName, material)
			End If
			Return material
		End Function

		' Token: 0x0600495E RID: 18782 RVA: 0x002658CC File Offset: 0x00263CCC
		Public Sub Dispose() Implements System.IDisposable.Dispose
			For Each keyValuePair As KeyValuePair(Of String, Material) In Me.m_Materials
				Dim value As Material = keyValuePair.Value
				GraphicsUtils.Destroy(value)
			Next
			Me.m_Materials.Clear()
		End Sub

		' Token: 0x04004F7B RID: 20347
		Private m_Materials As Dictionary(Of String, Material)
	End Class
End Namespace
