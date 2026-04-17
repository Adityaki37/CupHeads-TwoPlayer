Imports System
Imports System.Collections.Generic
Imports UnityEngine

Namespace TMPro
	' Token: 0x02000C67 RID: 3175
	Public Structure MaterialReference
		' Token: 0x06004F13 RID: 20243 RVA: 0x0027B628 File Offset: 0x00279A28
		Public Sub New(index As Integer, fontAsset As TMP_FontAsset, spriteAsset As TMP_SpriteAsset, material As Material, padding As Single)
			Me.index = index
			Me.fontAsset = fontAsset
			Me.spriteAsset = spriteAsset
			Me.material = material
			Me.isDefaultMaterial = material.GetInstanceID() = fontAsset.material.GetInstanceID()
			Me.isFallbackFont = False
			Me.padding = padding
			Me.referenceCount = 0
		End Sub

		' Token: 0x06004F14 RID: 20244 RVA: 0x0027B68C File Offset: 0x00279A8C
		Public Shared Function Contains(materialReferences As MaterialReference(), fontAsset As TMP_FontAsset) As Boolean
			Dim instanceID As Integer = fontAsset.GetInstanceID()
			Dim num As Integer = 0
			While num < materialReferences.Length AndAlso materialReferences(num).fontAsset IsNot Nothing
				If materialReferences(num).fontAsset.GetInstanceID() = instanceID Then
					Return True
				End If
				num += 1
			End While
			Return False
		End Function

		' Token: 0x06004F15 RID: 20245 RVA: 0x0027B6E8 File Offset: 0x00279AE8
		Public Shared Function AddMaterialReference(material As Material, fontAsset As TMP_FontAsset, materialReferences As MaterialReference(), materialReferenceIndexLookup As Dictionary(Of Integer, Integer)) As Integer
			Dim instanceID As Integer = material.GetInstanceID()
			Dim num As Integer = 0
			If materialReferenceIndexLookup.TryGetValue(instanceID, num) Then
				Return num
			End If
			num = materialReferenceIndexLookup.Count
			materialReferenceIndexLookup(instanceID) = num
			materialReferences(num).index = num
			materialReferences(num).fontAsset = fontAsset
			materialReferences(num).spriteAsset = Nothing
			materialReferences(num).material = material
			materialReferences(num).isDefaultMaterial = instanceID = fontAsset.material.GetInstanceID()
			materialReferences(num).referenceCount = 0
			Return num
		End Function

		' Token: 0x06004F16 RID: 20246 RVA: 0x0027B784 File Offset: 0x00279B84
		Public Shared Function AddMaterialReference(material As Material, spriteAsset As TMP_SpriteAsset, materialReferences As MaterialReference(), materialReferenceIndexLookup As Dictionary(Of Integer, Integer)) As Integer
			Dim instanceID As Integer = material.GetInstanceID()
			Dim num As Integer = 0
			If materialReferenceIndexLookup.TryGetValue(instanceID, num) Then
				Return num
			End If
			num = materialReferenceIndexLookup.Count
			materialReferenceIndexLookup(instanceID) = num
			materialReferences(num).index = num
			materialReferences(num).fontAsset = materialReferences(0).fontAsset
			materialReferences(num).spriteAsset = spriteAsset
			materialReferences(num).material = material
			materialReferences(num).isDefaultMaterial = True
			materialReferences(num).referenceCount = 0
			Return num
		End Function

		' Token: 0x04005215 RID: 21013
		Public index As Integer

		' Token: 0x04005216 RID: 21014
		Public fontAsset As TMP_FontAsset

		' Token: 0x04005217 RID: 21015
		Public spriteAsset As TMP_SpriteAsset

		' Token: 0x04005218 RID: 21016
		Public material As Material

		' Token: 0x04005219 RID: 21017
		Public isDefaultMaterial As Boolean

		' Token: 0x0400521A RID: 21018
		Public isFallbackFont As Boolean

		' Token: 0x0400521B RID: 21019
		Public padding As Single

		' Token: 0x0400521C RID: 21020
		Public referenceCount As Integer
	End Structure
End Namespace
