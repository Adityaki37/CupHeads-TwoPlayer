Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports UnityEngine
Imports UnityEngine.UI

Namespace TMPro
	' Token: 0x02000C73 RID: 3187
	Public Module TMP_MaterialManager
		' Token: 0x06004FDC RID: 20444 RVA: 0x00294B08 File Offset: 0x00292F08
		Public Function GetStencilMaterial(baseMaterial As Material, stencilID As Integer) As Material
			If Not baseMaterial.HasProperty(ShaderUtilities.ID_StencilID) Then
				Return baseMaterial
			End If
			Dim instanceID As Integer = baseMaterial.GetInstanceID()
			For i As Integer = 0 To TMP_MaterialManager.m_materialList.Count - 1
				If TMP_MaterialManager.m_materialList(i).baseMaterial.GetInstanceID() = instanceID AndAlso TMP_MaterialManager.m_materialList(i).stencilID = stencilID Then
					TMP_MaterialManager.m_materialList(i).count += 1
					Return TMP_MaterialManager.m_materialList(i).stencilMaterial
				End If
			Next
			Dim material As Material = New Material(baseMaterial)
			material.hideFlags = HideFlags.HideAndDontSave
			Dim material2 As Material = material
			material2.name = material2.name + " Masking ID:" + stencilID
			material.shaderKeywords = baseMaterial.shaderKeywords
			ShaderUtilities.GetShaderPropertyIDs()
			material.SetFloat(ShaderUtilities.ID_StencilID, CSng(stencilID))
			material.SetFloat(ShaderUtilities.ID_StencilComp, 4F)
			Dim maskingMaterial As TMP_MaterialManager.MaskingMaterial = New TMP_MaterialManager.MaskingMaterial()
			maskingMaterial.baseMaterial = baseMaterial
			maskingMaterial.stencilMaterial = material
			maskingMaterial.stencilID = stencilID
			maskingMaterial.count = 1
			TMP_MaterialManager.m_materialList.Add(maskingMaterial)
			Return material
		End Function

		' Token: 0x06004FDD RID: 20445 RVA: 0x00294C2C File Offset: 0x0029302C
		Public Sub ReleaseStencilMaterial(stencilMaterial As Material)
			Dim instanceID As Integer = stencilMaterial.GetInstanceID()
			For i As Integer = 0 To TMP_MaterialManager.m_materialList.Count - 1
				If TMP_MaterialManager.m_materialList(i).stencilMaterial.GetInstanceID() = instanceID Then
					If TMP_MaterialManager.m_materialList(i).count > 1 Then
						TMP_MaterialManager.m_materialList(i).count -= 1
					Else
						Global.UnityEngine.[Object].DestroyImmediate(TMP_MaterialManager.m_materialList(i).stencilMaterial)
						TMP_MaterialManager.m_materialList.RemoveAt(i)
						stencilMaterial = Nothing
					End If
					Exit For
				End If
			Next
		End Sub

		' Token: 0x06004FDE RID: 20446 RVA: 0x00294CD4 File Offset: 0x002930D4
		Public Function GetBaseMaterial(stencilMaterial As Material) As Material
			Dim num As Integer = TMP_MaterialManager.m_materialList.FindIndex(Function(item As TMP_MaterialManager.MaskingMaterial) item.stencilMaterial Is stencilMaterial)
			If num = -1 Then
				Return Nothing
			End If
			Return TMP_MaterialManager.m_materialList(num).baseMaterial
		End Function

		' Token: 0x06004FDF RID: 20447 RVA: 0x00294D1E File Offset: 0x0029311E
		Public Function SetStencil(material As Material, stencilID As Integer) As Material
			material.SetFloat(ShaderUtilities.ID_StencilID, CSng(stencilID))
			If stencilID = 0 Then
				material.SetFloat(ShaderUtilities.ID_StencilComp, 8F)
			Else
				material.SetFloat(ShaderUtilities.ID_StencilComp, 4F)
			End If
			Return material
		End Function

		' Token: 0x06004FE0 RID: 20448 RVA: 0x00294D5C File Offset: 0x0029315C
		Public Sub AddMaskingMaterial(baseMaterial As Material, stencilMaterial As Material, stencilID As Integer)
			Dim num As Integer = TMP_MaterialManager.m_materialList.FindIndex(Function(item As TMP_MaterialManager.MaskingMaterial) item.stencilMaterial Is stencilMaterial)
			If num = -1 Then
				Dim maskingMaterial As TMP_MaterialManager.MaskingMaterial = New TMP_MaterialManager.MaskingMaterial()
				maskingMaterial.baseMaterial = baseMaterial
				maskingMaterial.stencilMaterial = stencilMaterial
				maskingMaterial.stencilID = stencilID
				maskingMaterial.count = 1
				TMP_MaterialManager.m_materialList.Add(maskingMaterial)
			Else
				stencilMaterial = TMP_MaterialManager.m_materialList(num).stencilMaterial
				TMP_MaterialManager.m_materialList(num).count += 1
			End If
		End Sub

		' Token: 0x06004FE1 RID: 20449 RVA: 0x00294DFC File Offset: 0x002931FC
		Public Sub RemoveStencilMaterial(stencilMaterial As Material)
			Dim num As Integer = TMP_MaterialManager.m_materialList.FindIndex(Function(item As TMP_MaterialManager.MaskingMaterial) item.stencilMaterial Is stencilMaterial)
			If num <> -1 Then
				TMP_MaterialManager.m_materialList.RemoveAt(num)
			End If
		End Sub

		' Token: 0x06004FE2 RID: 20450 RVA: 0x00294E40 File Offset: 0x00293240
		Public Sub ReleaseBaseMaterial(baseMaterial As Material)
			Dim num As Integer = TMP_MaterialManager.m_materialList.FindIndex(Function(item As TMP_MaterialManager.MaskingMaterial) item.baseMaterial Is baseMaterial)
			If num <> -1 Then
				If TMP_MaterialManager.m_materialList(num).count > 1 Then
					TMP_MaterialManager.m_materialList(num).count -= 1
				Else
					Global.UnityEngine.[Object].DestroyImmediate(TMP_MaterialManager.m_materialList(num).stencilMaterial)
					TMP_MaterialManager.m_materialList.RemoveAt(num)
				End If
			End If
		End Sub

		' Token: 0x06004FE3 RID: 20451 RVA: 0x00294ED0 File Offset: 0x002932D0
		Public Sub ClearMaterials()
			If TMP_MaterialManager.m_materialList.Count() = 0 Then
				Return
			End If
			For i As Integer = 0 To TMP_MaterialManager.m_materialList.Count() - 1
				Dim stencilMaterial As Material = TMP_MaterialManager.m_materialList(i).stencilMaterial
				Global.UnityEngine.[Object].DestroyImmediate(stencilMaterial)
				TMP_MaterialManager.m_materialList.RemoveAt(i)
			Next
		End Sub

		' Token: 0x06004FE4 RID: 20452 RVA: 0x00294F2C File Offset: 0x0029332C
		Public Function GetStencilID(obj As GameObject) As Integer
			Dim num As Integer = 0
			Dim list As List(Of Mask) = TMP_ListPool(Of Mask).[Get]()
			obj.GetComponentsInParent(Of Mask)(False, list)
			For i As Integer = 0 To list.Count - 1
				If list(i).MaskEnabled() Then
					num += 1
				End If
			Next
			TMP_ListPool(Of Mask).Release(list)
			Return Mathf.Min((1 << num) - 1, 255)
		End Function

		' Token: 0x06004FE5 RID: 20453 RVA: 0x00294F90 File Offset: 0x00293390
		Public Function GetFallbackMaterial(source As Material, mainTex As Texture) As Material
			Dim instanceID As Integer = source.GetInstanceID()
			Dim instanceID2 As Integer = mainTex.GetInstanceID()
			For i As Integer = 0 To TMP_MaterialManager.m_fallbackMaterialList.Count - 1
				If TMP_MaterialManager.m_fallbackMaterialList(i).fallbackMaterial Is Nothing Then
					TMP_MaterialManager.m_fallbackMaterialList.RemoveAt(i)
				ElseIf TMP_MaterialManager.m_fallbackMaterialList(i).baseMaterial.GetInstanceID() = instanceID AndAlso TMP_MaterialManager.m_fallbackMaterialList(i).fallbackMaterial.mainTexture.GetInstanceID() = instanceID2 Then
					TMP_MaterialManager.m_fallbackMaterialList(i).count += 1
					Return TMP_MaterialManager.m_fallbackMaterialList(i).fallbackMaterial
				End If
			Next
			Dim material As Material = New Material(source)
			Dim material2 As Material = material
			material2.name += " (Fallback Instance)"
			material.mainTexture = mainTex
			Dim fallbackMaterial As TMP_MaterialManager.FallbackMaterial = New TMP_MaterialManager.FallbackMaterial()
			fallbackMaterial.baseID = instanceID
			fallbackMaterial.baseMaterial = source
			fallbackMaterial.fallbackMaterial = material
			fallbackMaterial.count = 1
			TMP_MaterialManager.m_fallbackMaterialList.Add(fallbackMaterial)
			Return material
		End Function

		' Token: 0x04005293 RID: 21139
		Private m_materialList As List(Of TMP_MaterialManager.MaskingMaterial) = New List(Of TMP_MaterialManager.MaskingMaterial)()

		' Token: 0x04005294 RID: 21140
		Private m_fallbackMaterialList As List(Of TMP_MaterialManager.FallbackMaterial) = New List(Of TMP_MaterialManager.FallbackMaterial)()

		' Token: 0x02000C74 RID: 3188
		Private Class FallbackMaterial
			' Token: 0x04005295 RID: 21141
			Public baseID As Integer

			' Token: 0x04005296 RID: 21142
			Public baseMaterial As Material

			' Token: 0x04005297 RID: 21143
			Public fallbackMaterial As Material

			' Token: 0x04005298 RID: 21144
			Public count As Integer
		End Class

		' Token: 0x02000C75 RID: 3189
		Private Class MaskingMaterial
			' Token: 0x04005299 RID: 21145
			Public baseMaterial As Material

			' Token: 0x0400529A RID: 21146
			Public stencilMaterial As Material

			' Token: 0x0400529B RID: 21147
			Public count As Integer

			' Token: 0x0400529C RID: 21148
			Public stencilID As Integer
		End Class
	End Module
End Namespace
