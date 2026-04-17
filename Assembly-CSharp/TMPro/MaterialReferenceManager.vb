Imports System
Imports System.Collections.Generic
Imports UnityEngine

Namespace TMPro
	' Token: 0x02000C66 RID: 3174
	Public Class MaterialReferenceManager
		' Token: 0x1700080F RID: 2063
		' (get) Token: 0x06004F02 RID: 20226 RVA: 0x0027B429 File Offset: 0x00279829
		Public Shared ReadOnly Property instance As MaterialReferenceManager
			Get
				If MaterialReferenceManager.s_Instance Is Nothing Then
					MaterialReferenceManager.s_Instance = New MaterialReferenceManager()
				End If
				Return MaterialReferenceManager.s_Instance
			End Get
		End Property

		' Token: 0x06004F03 RID: 20227 RVA: 0x0027B444 File Offset: 0x00279844
		Public Shared Sub AddFontAsset(fontAsset As TMP_FontAsset)
			MaterialReferenceManager.instance.AddFontAssetInternal(fontAsset)
		End Sub

		' Token: 0x06004F04 RID: 20228 RVA: 0x0027B454 File Offset: 0x00279854
		Private Sub AddFontAssetInternal(fontAsset As TMP_FontAsset)
			If Me.m_FontAssetReferenceLookup.ContainsKey(fontAsset.hashCode) Then
				Return
			End If
			Me.m_FontAssetReferenceLookup.Add(fontAsset.hashCode, fontAsset)
			Me.m_FontMaterialReferenceLookup.Add(fontAsset.materialHashCode, fontAsset.material)
		End Sub

		' Token: 0x06004F05 RID: 20229 RVA: 0x0027B4A1 File Offset: 0x002798A1
		Public Shared Sub AddSpriteAsset(spriteAsset As TMP_SpriteAsset)
			MaterialReferenceManager.instance.AddSpriteAssetInternal(spriteAsset)
		End Sub

		' Token: 0x06004F06 RID: 20230 RVA: 0x0027B4B0 File Offset: 0x002798B0
		Private Sub AddSpriteAssetInternal(spriteAsset As TMP_SpriteAsset)
			If Me.m_SpriteAssetReferenceLookup.ContainsKey(spriteAsset.hashCode) Then
				Return
			End If
			Me.m_SpriteAssetReferenceLookup.Add(spriteAsset.hashCode, spriteAsset)
			Me.m_FontMaterialReferenceLookup.Add(spriteAsset.hashCode, spriteAsset.material)
		End Sub

		' Token: 0x06004F07 RID: 20231 RVA: 0x0027B4FD File Offset: 0x002798FD
		Public Shared Sub AddSpriteAsset(hashCode As Integer, spriteAsset As TMP_SpriteAsset)
			MaterialReferenceManager.instance.AddSpriteAssetInternal(hashCode, spriteAsset)
		End Sub

		' Token: 0x06004F08 RID: 20232 RVA: 0x0027B50C File Offset: 0x0027990C
		Private Sub AddSpriteAssetInternal(hashCode As Integer, spriteAsset As TMP_SpriteAsset)
			If Me.m_SpriteAssetReferenceLookup.ContainsKey(hashCode) Then
				Return
			End If
			Me.m_SpriteAssetReferenceLookup.Add(hashCode, spriteAsset)
			Me.m_FontMaterialReferenceLookup.Add(hashCode, spriteAsset.material)
			If spriteAsset.hashCode = 0 Then
				spriteAsset.hashCode = hashCode
			End If
		End Sub

		' Token: 0x06004F09 RID: 20233 RVA: 0x0027B55C File Offset: 0x0027995C
		Public Shared Sub AddFontMaterial(hashCode As Integer, material As Material)
			MaterialReferenceManager.instance.AddFontMaterialInternal(hashCode, material)
		End Sub

		' Token: 0x06004F0A RID: 20234 RVA: 0x0027B56A File Offset: 0x0027996A
		Private Sub AddFontMaterialInternal(hashCode As Integer, material As Material)
			Me.m_FontMaterialReferenceLookup.Add(hashCode, material)
		End Sub

		' Token: 0x06004F0B RID: 20235 RVA: 0x0027B579 File Offset: 0x00279979
		Public Function Contains(font As TMP_FontAsset) As Boolean
			Return Me.m_FontAssetReferenceLookup.ContainsKey(font.hashCode)
		End Function

		' Token: 0x06004F0C RID: 20236 RVA: 0x0027B594 File Offset: 0x00279994
		Public Function Contains(sprite As TMP_SpriteAsset) As Boolean
			Return Me.m_FontAssetReferenceLookup.ContainsKey(sprite.hashCode)
		End Function

		' Token: 0x06004F0D RID: 20237 RVA: 0x0027B5AF File Offset: 0x002799AF
		Public Shared Function TryGetFontAsset(hashCode As Integer, <System.Runtime.InteropServices.OutAttribute()> ByRef fontAsset As TMP_FontAsset) As Boolean
			Return MaterialReferenceManager.instance.TryGetFontAssetInternal(hashCode, fontAsset)
		End Function

		' Token: 0x06004F0E RID: 20238 RVA: 0x0027B5BD File Offset: 0x002799BD
		Private Function TryGetFontAssetInternal(hashCode As Integer, <System.Runtime.InteropServices.OutAttribute()> ByRef fontAsset As TMP_FontAsset) As Boolean
			fontAsset = Nothing
			Return Me.m_FontAssetReferenceLookup.TryGetValue(hashCode, fontAsset)
		End Function

		' Token: 0x06004F0F RID: 20239 RVA: 0x0027B5D7 File Offset: 0x002799D7
		Public Shared Function TryGetSpriteAsset(hashCode As Integer, <System.Runtime.InteropServices.OutAttribute()> ByRef spriteAsset As TMP_SpriteAsset) As Boolean
			Return MaterialReferenceManager.instance.TryGetSpriteAssetInternal(hashCode, spriteAsset)
		End Function

		' Token: 0x06004F10 RID: 20240 RVA: 0x0027B5E5 File Offset: 0x002799E5
		Private Function TryGetSpriteAssetInternal(hashCode As Integer, <System.Runtime.InteropServices.OutAttribute()> ByRef spriteAsset As TMP_SpriteAsset) As Boolean
			spriteAsset = Nothing
			Return Me.m_SpriteAssetReferenceLookup.TryGetValue(hashCode, spriteAsset)
		End Function

		' Token: 0x06004F11 RID: 20241 RVA: 0x0027B5FF File Offset: 0x002799FF
		Public Shared Function TryGetMaterial(hashCode As Integer, <System.Runtime.InteropServices.OutAttribute()> ByRef material As Material) As Boolean
			Return MaterialReferenceManager.instance.TryGetMaterialInternal(hashCode, material)
		End Function

		' Token: 0x06004F12 RID: 20242 RVA: 0x0027B60D File Offset: 0x00279A0D
		Private Function TryGetMaterialInternal(hashCode As Integer, <System.Runtime.InteropServices.OutAttribute()> ByRef material As Material) As Boolean
			material = Nothing
			Return Me.m_FontMaterialReferenceLookup.TryGetValue(hashCode, material)
		End Function

		' Token: 0x04005211 RID: 21009
		Private Shared s_Instance As MaterialReferenceManager

		' Token: 0x04005212 RID: 21010
		Private m_FontMaterialReferenceLookup As Dictionary(Of Integer, Material) = New Dictionary(Of Integer, Material)()

		' Token: 0x04005213 RID: 21011
		Private m_FontAssetReferenceLookup As Dictionary(Of Integer, TMP_FontAsset) = New Dictionary(Of Integer, TMP_FontAsset)()

		' Token: 0x04005214 RID: 21012
		Private m_SpriteAssetReferenceLookup As Dictionary(Of Integer, TMP_SpriteAsset) = New Dictionary(Of Integer, TMP_SpriteAsset)()
	End Class
End Namespace
