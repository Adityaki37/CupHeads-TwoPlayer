Imports System
Imports UnityEngine
Imports UnityEngine.UI

Namespace TMPro
	' Token: 0x02000C64 RID: 3172
	Public Class InlineGraphic
		Inherits MaskableGraphic

		' Token: 0x1700080A RID: 2058
		' (get) Token: 0x06004EE4 RID: 20196 RVA: 0x0027AEB5 File Offset: 0x002792B5
		Public Overrides ReadOnly Property mainTexture As Texture
			Get
				If Me.texture Is Nothing Then
					Return Graphic.s_WhiteTexture
				End If
				Return Me.texture
			End Get
		End Property

		' Token: 0x06004EE5 RID: 20197 RVA: 0x0027AED4 File Offset: 0x002792D4
		Protected Overrides Sub Awake()
			Me.m_manager = MyBase.GetComponentInParent(Of InlineGraphicManager)()
		End Sub

		' Token: 0x06004EE6 RID: 20198 RVA: 0x0027AEE4 File Offset: 0x002792E4
		Protected Overrides Sub OnEnable()
			If Me.m_RectTransform Is Nothing Then
				Me.m_RectTransform = MyBase.gameObject.GetComponent(Of RectTransform)()
			End If
			If Me.m_manager IsNot Nothing AndAlso Me.m_manager.spriteAsset IsNot Nothing Then
				Me.texture = Me.m_manager.spriteAsset.spriteSheet
			End If
		End Sub

		' Token: 0x06004EE7 RID: 20199 RVA: 0x0027AF50 File Offset: 0x00279350
		Protected Overrides Sub OnDisable()
			MyBase.OnDisable()
		End Sub

		' Token: 0x06004EE8 RID: 20200 RVA: 0x0027AF58 File Offset: 0x00279358
		Protected Overrides Sub OnTransformParentChanged()
		End Sub

		' Token: 0x06004EE9 RID: 20201 RVA: 0x0027AF5C File Offset: 0x0027935C
		Protected Overrides Sub OnRectTransformDimensionsChange()
			If Me.m_RectTransform Is Nothing Then
				Me.m_RectTransform = MyBase.gameObject.GetComponent(Of RectTransform)()
			End If
			If Me.m_ParentRectTransform Is Nothing Then
				Me.m_ParentRectTransform = Me.m_RectTransform.parent.GetComponent(Of RectTransform)()
			End If
			If Me.m_RectTransform.pivot <> Me.m_ParentRectTransform.pivot Then
				Me.m_RectTransform.pivot = Me.m_ParentRectTransform.pivot
			End If
		End Sub

		' Token: 0x06004EEA RID: 20202 RVA: 0x0027AFE8 File Offset: 0x002793E8
		Public Sub UpdateMaterial()
			MyBase.UpdateMaterial()
		End Sub

		' Token: 0x06004EEB RID: 20203 RVA: 0x0027AFF0 File Offset: 0x002793F0
		Protected Overrides Sub UpdateGeometry()
		End Sub

		' Token: 0x04005206 RID: 20998
		Public texture As Texture

		' Token: 0x04005207 RID: 20999
		Private m_manager As InlineGraphicManager

		' Token: 0x04005208 RID: 21000
		Private m_RectTransform As RectTransform

		' Token: 0x04005209 RID: 21001
		Private m_ParentRectTransform As RectTransform
	End Class
End Namespace
