Imports System
Imports TMPro
Imports UnityEngine

' Token: 0x0200090E RID: 2318
Public Class AdjustTMPMaterial
	Inherits MonoBehaviour

	' Token: 0x0600365A RID: 13914 RVA: 0x001F7E04 File Offset: 0x001F6204
	Private Sub Update()
		If Not Me.initialSetupComplete OrElse Localization.language <> Me.previousLanguage Then
			Me.initialSetupComplete = True
			Me.previousLanguage = Localization.language
			Dim language As Localization.Languages = Localization.language
			Dim material As Material = Me.getMaterial(language)
			If material IsNot Nothing Then
				Me.text.fontMaterial = material
			End If
		End If
	End Sub

	' Token: 0x0600365B RID: 13915 RVA: 0x001F7E64 File Offset: 0x001F6264
	Private Function getMaterial(language As Localization.Languages) As Material
		For Each materialData As AdjustTMPMaterial.MaterialData In Me.materials
			If materialData.language = language Then
				Return FontLoader.GetTMPMaterial(materialData.materialName)
			End If
		Next
		Return Me.defaultMaterial
	End Function

	' Token: 0x04003E4A RID: 15946
	<SerializeField()>
	Private text As TextMeshProUGUI

	' Token: 0x04003E4B RID: 15947
	<SerializeField()>
	Private defaultMaterial As Material

	' Token: 0x04003E4C RID: 15948
	<SerializeField()>
	Private materials As AdjustTMPMaterial.MaterialData()

	' Token: 0x04003E4D RID: 15949
	Private previousLanguage As Localization.Languages

	' Token: 0x04003E4E RID: 15950
	Private initialSetupComplete As Boolean

	' Token: 0x0200090F RID: 2319
	<Serializable()>
	Public Structure MaterialData
		' Token: 0x04003E4F RID: 15951
		Public language As Localization.Languages

		' Token: 0x04003E50 RID: 15952
		Public materialName As String
	End Structure
End Class
