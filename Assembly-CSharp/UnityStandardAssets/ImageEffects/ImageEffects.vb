Imports System
Imports UnityEngine

Namespace UnityStandardAssets.ImageEffects
	' Token: 0x02000CDD RID: 3293
	<AddComponentMenu("")>
	Public Class ImageEffects
		' Token: 0x06005226 RID: 21030 RVA: 0x002A1EFC File Offset: 0x002A02FC
		Public Shared Sub RenderDistortion(material As Material, source As RenderTexture, destination As RenderTexture, angle As Single, center As Vector2, radius As Vector2)
			Dim flag As Boolean = source.texelSize.y < 0F
			If flag Then
				center.y = 1F - center.y
				angle = -angle
			End If
			Dim matrix4x As Matrix4x4 = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0F, 0F, angle), Vector3.one)
			material.SetMatrix("_RotationMatrix", matrix4x)
			material.SetVector("_CenterRadius", New Vector4(center.x, center.y, radius.x, radius.y))
			material.SetFloat("_Angle", angle * 0.017453292F)
			Graphics.Blit(source, destination, material)
		End Sub

		' Token: 0x06005227 RID: 21031 RVA: 0x002A1FAF File Offset: 0x002A03AF
		<Obsolete("Use Graphics.Blit(source,dest) instead")>
		Public Shared Sub Blit(source As RenderTexture, dest As RenderTexture)
			Graphics.Blit(source, dest)
		End Sub

		' Token: 0x06005228 RID: 21032 RVA: 0x002A1FB8 File Offset: 0x002A03B8
		<Obsolete("Use Graphics.Blit(source, destination, material) instead")>
		Public Shared Sub BlitWithMaterial(material As Material, source As RenderTexture, dest As RenderTexture)
			Graphics.Blit(source, dest, material)
		End Sub
	End Class
End Namespace
