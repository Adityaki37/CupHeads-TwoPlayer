Imports System
Imports UnityEngine

Namespace TMPro
	' Token: 0x02000C96 RID: 3222
	Public Module TMPro_EventManager
		' Token: 0x0600515F RID: 20831 RVA: 0x00298F9B File Offset: 0x0029739B
		Public Sub ON_PRE_RENDER_OBJECT_CHANGED()
			TMPro_EventManager.OnPreRenderObject_Event.[Call]()
		End Sub

		' Token: 0x06005160 RID: 20832 RVA: 0x00298FA7 File Offset: 0x002973A7
		Public Sub ON_MATERIAL_PROPERTY_CHANGED(isChanged As Boolean, mat As Material)
			TMPro_EventManager.MATERIAL_PROPERTY_EVENT.[Call](isChanged, mat)
		End Sub

		' Token: 0x06005161 RID: 20833 RVA: 0x00298FB5 File Offset: 0x002973B5
		Public Sub ON_FONT_PROPERTY_CHANGED(isChanged As Boolean, font As TMP_FontAsset)
			TMPro_EventManager.FONT_PROPERTY_EVENT.[Call](isChanged, font)
		End Sub

		' Token: 0x06005162 RID: 20834 RVA: 0x00298FC3 File Offset: 0x002973C3
		Public Sub ON_SPRITE_ASSET_PROPERTY_CHANGED(isChanged As Boolean, obj As Global.UnityEngine.[Object])
			TMPro_EventManager.SPRITE_ASSET_PROPERTY_EVENT.[Call](isChanged, obj)
		End Sub

		' Token: 0x06005163 RID: 20835 RVA: 0x00298FD1 File Offset: 0x002973D1
		Public Sub ON_TEXTMESHPRO_PROPERTY_CHANGED(isChanged As Boolean, obj As TextMeshPro)
			TMPro_EventManager.TEXTMESHPRO_PROPERTY_EVENT.[Call](isChanged, obj)
		End Sub

		' Token: 0x06005164 RID: 20836 RVA: 0x00298FDF File Offset: 0x002973DF
		Public Sub ON_DRAG_AND_DROP_MATERIAL_CHANGED(sender As GameObject, currentMaterial As Material, newMaterial As Material)
			TMPro_EventManager.DRAG_AND_DROP_MATERIAL_EVENT.[Call](sender, currentMaterial, newMaterial)
		End Sub

		' Token: 0x06005165 RID: 20837 RVA: 0x00298FEE File Offset: 0x002973EE
		Public Sub ON_TEXT_STYLE_PROPERTY_CHANGED(isChanged As Boolean)
			TMPro_EventManager.TEXT_STYLE_PROPERTY_EVENT.[Call](isChanged)
		End Sub

		' Token: 0x06005166 RID: 20838 RVA: 0x00298FFB File Offset: 0x002973FB
		Public Sub ON_TEXT_CHANGED(obj As Global.UnityEngine.[Object])
			TMPro_EventManager.TEXT_CHANGED_EVENT.[Call](obj)
		End Sub

		' Token: 0x06005167 RID: 20839 RVA: 0x00299008 File Offset: 0x00297408
		Public Sub ON_TMP_SETTINGS_CHANGED()
			TMPro_EventManager.TMP_SETTINGS_PROPERTY_EVENT.[Call]()
		End Sub

		' Token: 0x06005168 RID: 20840 RVA: 0x00299014 File Offset: 0x00297414
		Public Sub ON_TEXTMESHPRO_UGUI_PROPERTY_CHANGED(isChanged As Boolean, obj As TextMeshProUGUI)
			TMPro_EventManager.TEXTMESHPRO_UGUI_PROPERTY_EVENT.[Call](isChanged, obj)
		End Sub

		' Token: 0x06005169 RID: 20841 RVA: 0x00299022 File Offset: 0x00297422
		Public Sub ON_BASE_MATERIAL_CHANGED(mat As Material)
			TMPro_EventManager.BASE_MATERIAL_EVENT.[Call](mat)
		End Sub

		' Token: 0x0600516A RID: 20842 RVA: 0x0029902F File Offset: 0x0029742F
		Public Sub ON_COMPUTE_DT_EVENT(Sender As Object, e As Compute_DT_EventArgs)
			TMPro_EventManager.COMPUTE_DT_EVENT.[Call](Sender, e)
		End Sub

		' Token: 0x040053FD RID: 21501
		Public COMPUTE_DT_EVENT As FastAction(Of Object, Compute_DT_EventArgs) = New FastAction(Of Object, Compute_DT_EventArgs)()

		' Token: 0x040053FE RID: 21502
		Public MATERIAL_PROPERTY_EVENT As FastAction(Of Boolean, Material) = New FastAction(Of Boolean, Material)()

		' Token: 0x040053FF RID: 21503
		Public FONT_PROPERTY_EVENT As FastAction(Of Boolean, TMP_FontAsset) = New FastAction(Of Boolean, TMP_FontAsset)()

		' Token: 0x04005400 RID: 21504
		Public SPRITE_ASSET_PROPERTY_EVENT As FastAction(Of Boolean, Global.UnityEngine.[Object]) = New FastAction(Of Boolean, Global.UnityEngine.[Object])()

		' Token: 0x04005401 RID: 21505
		Public TEXTMESHPRO_PROPERTY_EVENT As FastAction(Of Boolean, TextMeshPro) = New FastAction(Of Boolean, TextMeshPro)()

		' Token: 0x04005402 RID: 21506
		Public DRAG_AND_DROP_MATERIAL_EVENT As FastAction(Of GameObject, Material, Material) = New FastAction(Of GameObject, Material, Material)()

		' Token: 0x04005403 RID: 21507
		Public TEXT_STYLE_PROPERTY_EVENT As FastAction(Of Boolean) = New FastAction(Of Boolean)()

		' Token: 0x04005404 RID: 21508
		Public TMP_SETTINGS_PROPERTY_EVENT As FastAction = New FastAction()

		' Token: 0x04005405 RID: 21509
		Public TEXTMESHPRO_UGUI_PROPERTY_EVENT As FastAction(Of Boolean, TextMeshProUGUI) = New FastAction(Of Boolean, TextMeshProUGUI)()

		' Token: 0x04005406 RID: 21510
		Public BASE_MATERIAL_EVENT As FastAction(Of Material) = New FastAction(Of Material)()

		' Token: 0x04005407 RID: 21511
		Public OnPreRenderObject_Event As FastAction = New FastAction()

		' Token: 0x04005408 RID: 21512
		Public TEXT_CHANGED_EVENT As FastAction(Of Global.UnityEngine.[Object]) = New FastAction(Of Global.UnityEngine.[Object])()

		' Token: 0x04005409 RID: 21513
		Public WILL_RENDER_CANVASES As FastAction = New FastAction()
	End Module
End Namespace
