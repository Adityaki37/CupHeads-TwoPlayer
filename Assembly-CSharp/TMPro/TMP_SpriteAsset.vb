Imports System
Imports System.Collections.Generic
Imports UnityEngine

Namespace TMPro
	' Token: 0x02000C79 RID: 3193
	Public Class TMP_SpriteAsset
		Inherits TMP_Asset

		' Token: 0x17000844 RID: 2116
		' (get) Token: 0x06005002 RID: 20482 RVA: 0x002953C7 File Offset: 0x002937C7
		Public Shared ReadOnly Property defaultSpriteAsset As TMP_SpriteAsset
			Get
				If TMP_SpriteAsset.m_defaultSpriteAsset Is Nothing Then
					TMP_SpriteAsset.m_defaultSpriteAsset = Resources.Load(Of TMP_SpriteAsset)("Sprite Assets/Default Sprite Asset")
				End If
				Return TMP_SpriteAsset.m_defaultSpriteAsset
			End Get
		End Property

		' Token: 0x06005003 RID: 20483 RVA: 0x002953ED File Offset: 0x002937ED
		Private Sub OnEnable()
		End Sub

		' Token: 0x06005004 RID: 20484 RVA: 0x002953F0 File Offset: 0x002937F0
		Private Function GetDefaultSpriteMaterial() As Material
			ShaderUtilities.GetShaderPropertyIDs()
			Dim shader As Shader = Shader.Find("TMPro/Sprite")
			Dim material As Material = New Material(shader)
			material.SetTexture(ShaderUtilities.ID_MainTex, Me.spriteSheet)
			material.hideFlags = HideFlags.HideInHierarchy
			Return material
		End Function

		' Token: 0x06005005 RID: 20485 RVA: 0x00295430 File Offset: 0x00293830
		Public Function GetSpriteIndex(hashCode As Integer) As Integer
			For i As Integer = 0 To Me.spriteInfoList.Count - 1
				If Me.spriteInfoList(i).hashCode = hashCode Then
					Return i
				End If
			Next
			Return -1
		End Function

		' Token: 0x040052B0 RID: 21168
		Public Shared m_defaultSpriteAsset As TMP_SpriteAsset

		' Token: 0x040052B1 RID: 21169
		Public spriteSheet As Texture

		' Token: 0x040052B2 RID: 21170
		Public spriteInfoList As List(Of TMP_Sprite)

		' Token: 0x040052B3 RID: 21171
		Private m_sprites As List(Of Sprite)
	End Class
End Namespace
