Imports System
Imports UnityEngine
Imports UnityEngine.UI

Namespace TMPro
	' Token: 0x02000C7D RID: 3197
	<ExecuteInEditMode()>
	Public Class TMP_SubMeshUI
		Inherits MaskableGraphic
		Implements ITextElement, IClippable, IMaskable, IMaterialModifier

		' Token: 0x17000855 RID: 2133
		' (get) Token: 0x06005037 RID: 20535 RVA: 0x00295BC7 File Offset: 0x00293FC7
		' (set) Token: 0x06005038 RID: 20536 RVA: 0x00295BCF File Offset: 0x00293FCF
		Public Property fontAsset As TMP_FontAsset
			Get
				Return Me.m_fontAsset
			End Get
			Set(value As TMP_FontAsset)
				Me.m_fontAsset = value
			End Set
		End Property

		' Token: 0x17000856 RID: 2134
		' (get) Token: 0x06005039 RID: 20537 RVA: 0x00295BD8 File Offset: 0x00293FD8
		' (set) Token: 0x0600503A RID: 20538 RVA: 0x00295BE0 File Offset: 0x00293FE0
		Public Property spriteAsset As TMP_SpriteAsset
			Get
				Return Me.m_spriteAsset
			End Get
			Set(value As TMP_SpriteAsset)
				Me.m_spriteAsset = value
			End Set
		End Property

		' Token: 0x17000857 RID: 2135
		' (get) Token: 0x0600503B RID: 20539 RVA: 0x00295BE9 File Offset: 0x00293FE9
		Public Overrides ReadOnly Property mainTexture As Texture
			Get
				If Me.sharedMaterial IsNot Nothing Then
					Return Me.sharedMaterial.mainTexture
				End If
				Return Nothing
			End Get
		End Property

		' Token: 0x17000858 RID: 2136
		' (get) Token: 0x0600503C RID: 20540 RVA: 0x00295C09 File Offset: 0x00294009
		' (set) Token: 0x0600503D RID: 20541 RVA: 0x00295C17 File Offset: 0x00294017
		Public Overrides Property material As Material
			Get
				Return Me.GetMaterial(Me.m_sharedMaterial)
			End Get
			Set(value As Material)
				If Me.m_sharedMaterial.GetInstanceID() = value.GetInstanceID() Then
					Return
				End If
				Me.m_sharedMaterial = value
				Me.m_padding = Me.GetPaddingForMaterial()
				Me.SetVerticesDirty()
				Me.SetMaterialDirty()
			End Set
		End Property

		' Token: 0x17000859 RID: 2137
		' (get) Token: 0x0600503E RID: 20542 RVA: 0x00295C4F File Offset: 0x0029404F
		' (set) Token: 0x0600503F RID: 20543 RVA: 0x00295C57 File Offset: 0x00294057
		Public Property sharedMaterial As Material Implements TMPro.ITextElement.sharedMaterial
			Get
				Return Me.m_sharedMaterial
			End Get
			Set(value As Material)
				Me.SetSharedMaterial(value)
			End Set
		End Property

		' Token: 0x1700085A RID: 2138
		' (get) Token: 0x06005040 RID: 20544 RVA: 0x00295C60 File Offset: 0x00294060
		Public Overrides ReadOnly Property materialForRendering As Material
			Get
				If Me.m_sharedMaterial Is Nothing Then
					Return Nothing
				End If
				Return Me.GetModifiedMaterial(Me.m_sharedMaterial)
			End Get
		End Property

		' Token: 0x1700085B RID: 2139
		' (get) Token: 0x06005041 RID: 20545 RVA: 0x00295C81 File Offset: 0x00294081
		' (set) Token: 0x06005042 RID: 20546 RVA: 0x00295C89 File Offset: 0x00294089
		Public Property isDefaultMaterial As Boolean
			Get
				Return Me.m_isDefaultMaterial
			End Get
			Set(value As Boolean)
				Me.m_isDefaultMaterial = value
			End Set
		End Property

		' Token: 0x1700085C RID: 2140
		' (get) Token: 0x06005043 RID: 20547 RVA: 0x00295C92 File Offset: 0x00294092
		' (set) Token: 0x06005044 RID: 20548 RVA: 0x00295C9A File Offset: 0x0029409A
		Public Property padding As Single
			Get
				Return Me.m_padding
			End Get
			Set(value As Single)
				Me.m_padding = value
			End Set
		End Property

		' Token: 0x1700085D RID: 2141
		' (get) Token: 0x06005045 RID: 20549 RVA: 0x00295CA3 File Offset: 0x002940A3
		Public ReadOnly Property canvasRenderer As CanvasRenderer
			Get
				If Me.m_canvasRenderer Is Nothing Then
					Me.m_canvasRenderer = MyBase.GetComponent(Of CanvasRenderer)()
				End If
				Return Me.m_canvasRenderer
			End Get
		End Property

		' Token: 0x1700085E RID: 2142
		' (get) Token: 0x06005046 RID: 20550 RVA: 0x00295CC8 File Offset: 0x002940C8
		' (set) Token: 0x06005047 RID: 20551 RVA: 0x00295CF9 File Offset: 0x002940F9
		Public Property mesh As Mesh
			Get
				If Me.m_mesh Is Nothing Then
					Me.m_mesh = New Mesh()
					Me.m_mesh.hideFlags = HideFlags.HideAndDontSave
				End If
				Return Me.m_mesh
			End Get
			Set(value As Mesh)
				Me.m_mesh = value
			End Set
		End Property

		' Token: 0x06005048 RID: 20552 RVA: 0x00295D04 File Offset: 0x00294104
		Public Shared Function AddSubTextObject(textComponent As TextMeshProUGUI, materialReference As MaterialReference) As TMP_SubMeshUI
			Dim gameObject As GameObject = New GameObject("TMP UI SubObject [" + materialReference.material.name + "]")
			gameObject.layer = textComponent.gameObject.layer
			Dim rectTransform As RectTransform = gameObject.AddComponent(Of RectTransform)()
			rectTransform.anchorMin = Vector2.zero
			rectTransform.anchorMax = Vector2.one
			rectTransform.sizeDelta = Vector2.zero
			rectTransform.pivot = textComponent.rectTransform.pivot
			Dim tmp_SubMeshUI As TMP_SubMeshUI = gameObject.AddComponent(Of TMP_SubMeshUI)()
			tmp_SubMeshUI.m_canvasRenderer = tmp_SubMeshUI.canvasRenderer
			tmp_SubMeshUI.m_TextComponent = textComponent
			tmp_SubMeshUI.m_materialReferenceIndex = materialReference.index
			tmp_SubMeshUI.m_fontAsset = materialReference.fontAsset
			tmp_SubMeshUI.m_spriteAsset = materialReference.spriteAsset
			tmp_SubMeshUI.m_isDefaultMaterial = materialReference.isDefaultMaterial
			tmp_SubMeshUI.SetSharedMaterial(materialReference.material)
			gameObject.transform.SetParent(textComponent.transform, False)
			Return tmp_SubMeshUI
		End Function

		' Token: 0x06005049 RID: 20553 RVA: 0x00295DEA File Offset: 0x002941EA
		Protected Overrides Sub OnEnable()
			If Not Me.m_isRegisteredForEvents Then
				Me.m_isRegisteredForEvents = True
			End If
			Me.m_ShouldRecalculateStencil = True
			Me.RecalculateClipping()
			Me.RecalculateMasking()
		End Sub

		' Token: 0x0600504A RID: 20554 RVA: 0x00295E11 File Offset: 0x00294211
		Protected Overrides Sub OnDisable()
			TMP_UpdateRegistry.UnRegisterCanvasElementForRebuild(Me)
			If Me.m_MaskMaterial IsNot Nothing Then
				TMP_MaterialManager.ReleaseStencilMaterial(Me.m_MaskMaterial)
				Me.m_MaskMaterial = Nothing
			End If
			MyBase.OnDisable()
		End Sub

		' Token: 0x0600504B RID: 20555 RVA: 0x00295E44 File Offset: 0x00294244
		Protected Overrides Sub OnDestroy()
			If Me.m_mesh IsNot Nothing Then
				Global.UnityEngine.[Object].DestroyImmediate(Me.m_mesh)
			End If
			If Me.m_MaskMaterial IsNot Nothing Then
				TMP_MaterialManager.ReleaseStencilMaterial(Me.m_MaskMaterial)
			End If
			Me.m_isRegisteredForEvents = False
			Me.RecalculateClipping()
		End Sub

		' Token: 0x0600504C RID: 20556 RVA: 0x00295E96 File Offset: 0x00294296
		Protected Overrides Sub OnTransformParentChanged()
			If Not Me.IsActive() Then
				Return
			End If
			Me.m_ShouldRecalculateStencil = True
			Me.RecalculateClipping()
			Me.RecalculateMasking()
		End Sub

		' Token: 0x0600504D RID: 20557 RVA: 0x00295EB8 File Offset: 0x002942B8
		Public Overrides Function GetModifiedMaterial(baseMaterial As Material) As Material Implements UnityEngine.UI.IMaterialModifier.GetModifiedMaterial
			Dim material As Material = baseMaterial
			If Me.m_ShouldRecalculateStencil Then
				Me.m_StencilValue = TMP_MaterialManager.GetStencilID(MyBase.gameObject)
				Me.m_ShouldRecalculateStencil = False
			End If
			If Me.m_StencilValue > 0 Then
				material = TMP_MaterialManager.GetStencilMaterial(baseMaterial, Me.m_StencilValue)
				If Me.m_MaskMaterial IsNot Nothing Then
					TMP_MaterialManager.ReleaseStencilMaterial(Me.m_MaskMaterial)
				End If
				Me.m_MaskMaterial = material
			End If
			Return material
		End Function

		' Token: 0x0600504E RID: 20558 RVA: 0x00295F28 File Offset: 0x00294328
		Public Function GetPaddingForMaterial() As Single
			Return ShaderUtilities.GetPadding(Me.m_sharedMaterial, Me.m_TextComponent.extraPadding, Me.m_TextComponent.isUsingBold)
		End Function

		' Token: 0x0600504F RID: 20559 RVA: 0x00295F58 File Offset: 0x00294358
		Public Function GetPaddingForMaterial(mat As Material) As Single
			Return ShaderUtilities.GetPadding(mat, Me.m_TextComponent.extraPadding, Me.m_TextComponent.isUsingBold)
		End Function

		' Token: 0x06005050 RID: 20560 RVA: 0x00295F83 File Offset: 0x00294383
		Public Sub UpdateMeshPadding(isExtraPadding As Boolean, isUsingBold As Boolean)
			Me.m_padding = ShaderUtilities.GetPadding(Me.m_sharedMaterial, isExtraPadding, isUsingBold)
		End Sub

		' Token: 0x06005051 RID: 20561 RVA: 0x00295F98 File Offset: 0x00294398
		Public Overrides Sub SetAllDirty()
		End Sub

		' Token: 0x06005052 RID: 20562 RVA: 0x00295F9A File Offset: 0x0029439A
		Public Overrides Sub SetVerticesDirty()
			If Not Me.IsActive() Then
				Return
			End If
			If Me.m_TextComponent IsNot Nothing Then
				Me.m_TextComponent.havePropertiesChanged = True
				Me.m_TextComponent.SetVerticesDirty()
			End If
		End Sub

		' Token: 0x06005053 RID: 20563 RVA: 0x00295FD0 File Offset: 0x002943D0
		Public Overrides Sub SetLayoutDirty()
		End Sub

		' Token: 0x06005054 RID: 20564 RVA: 0x00295FD2 File Offset: 0x002943D2
		Public Overrides Sub SetMaterialDirty()
			Me.m_materialDirty = True
			Me.UpdateMaterial()
		End Sub

		' Token: 0x06005055 RID: 20565 RVA: 0x00295FE1 File Offset: 0x002943E1
		Public Sub SetPivotDirty()
			If Not Me.IsActive() Then
				Return
			End If
			MyBase.rectTransform.pivot = Me.m_TextComponent.rectTransform.pivot
		End Sub

		' Token: 0x06005056 RID: 20566 RVA: 0x0029600A File Offset: 0x0029440A
		Protected Overrides Sub UpdateGeometry()
		End Sub

		' Token: 0x06005057 RID: 20567 RVA: 0x0029600C File Offset: 0x0029440C
		Public Overrides Sub Rebuild(update As CanvasUpdate) Implements TMPro.ITextElement.Rebuild
			If update = CanvasUpdate.PreRender Then
				If Not Me.m_materialDirty Then
					Return
				End If
				Me.UpdateMaterial()
				Me.m_materialDirty = False
			End If
		End Sub

		' Token: 0x06005058 RID: 20568 RVA: 0x0029602E File Offset: 0x0029442E
		Public Sub RefreshMaterial()
			Me.UpdateMaterial()
		End Sub

		' Token: 0x06005059 RID: 20569 RVA: 0x00296038 File Offset: 0x00294438
		Protected Overrides Sub UpdateMaterial()
			If Me.m_canvasRenderer Is Nothing Then
				Me.m_canvasRenderer = Me.canvasRenderer
			End If
			Me.m_canvasRenderer.materialCount = 1
			Me.m_canvasRenderer.SetMaterial(Me.materialForRendering, 0)
			Me.m_canvasRenderer.SetTexture(Me.mainTexture)
		End Sub

		' Token: 0x0600505A RID: 20570 RVA: 0x00296091 File Offset: 0x00294491
		Public Overrides Sub RecalculateClipping() Implements UnityEngine.UI.IClippable.RecalculateClipping
			MyBase.RecalculateClipping()
		End Sub

		' Token: 0x0600505B RID: 20571 RVA: 0x00296099 File Offset: 0x00294499
		Public Overrides Sub RecalculateMasking() Implements UnityEngine.UI.IMaskable.RecalculateMasking
			Me.m_ShouldRecalculateStencil = True
			Me.SetMaterialDirty()
		End Sub

		' Token: 0x0600505C RID: 20572 RVA: 0x002960A8 File Offset: 0x002944A8
		Private Function GetMaterial() As Material
			Return Me.m_sharedMaterial
		End Function

		' Token: 0x0600505D RID: 20573 RVA: 0x002960B0 File Offset: 0x002944B0
		Private Function GetMaterial(mat As Material) As Material
			If Me.m_material Is Nothing OrElse Me.m_material.GetInstanceID() <> mat.GetInstanceID() Then
				Me.m_material = Me.CreateMaterialInstance(mat)
			End If
			Me.m_sharedMaterial = Me.m_material
			Me.m_padding = Me.GetPaddingForMaterial()
			Me.SetVerticesDirty()
			Me.SetMaterialDirty()
			Return Me.m_sharedMaterial
		End Function

		' Token: 0x0600505E RID: 20574 RVA: 0x0029611C File Offset: 0x0029451C
		Private Function CreateMaterialInstance(source As Material) As Material
			Dim material As Material = New Material(source)
			material.shaderKeywords = source.shaderKeywords
			Dim material2 As Material = material
			material2.name += " (Instance)"
			Return material
		End Function

		' Token: 0x0600505F RID: 20575 RVA: 0x00296153 File Offset: 0x00294553
		Private Function GetSharedMaterial() As Material
			If Me.m_canvasRenderer Is Nothing Then
				Me.m_canvasRenderer = MyBase.GetComponent(Of CanvasRenderer)()
			End If
			Return Me.m_canvasRenderer.GetMaterial()
		End Function

		' Token: 0x06005060 RID: 20576 RVA: 0x0029617D File Offset: 0x0029457D
		Private Sub SetSharedMaterial(mat As Material)
			Me.m_sharedMaterial = mat
			Me.m_Material = Me.m_sharedMaterial
			Me.m_padding = Me.GetPaddingForMaterial()
			Me.SetMaterialDirty()
		End Sub

		' Token: 0x06005061 RID: 20577 RVA: 0x002961A4 File Offset: 0x002945A4
		Function GetInstanceID() As Integer Implements TMPro.ITextElement.GetInstanceID
			Return MyBase.GetInstanceID()
		End Function

		' Token: 0x040052C8 RID: 21192
		<SerializeField()>
		Private m_fontAsset As TMP_FontAsset

		' Token: 0x040052C9 RID: 21193
		<SerializeField()>
		Private m_spriteAsset As TMP_SpriteAsset

		' Token: 0x040052CA RID: 21194
		<SerializeField()>
		Private m_material As Material

		' Token: 0x040052CB RID: 21195
		<SerializeField()>
		Private m_sharedMaterial As Material

		' Token: 0x040052CC RID: 21196
		<SerializeField()>
		Private m_isDefaultMaterial As Boolean

		' Token: 0x040052CD RID: 21197
		<SerializeField()>
		Private m_padding As Single

		' Token: 0x040052CE RID: 21198
		<SerializeField()>
		Private m_canvasRenderer As CanvasRenderer

		' Token: 0x040052CF RID: 21199
		Private m_mesh As Mesh

		' Token: 0x040052D0 RID: 21200
		<SerializeField()>
		Private m_TextComponent As TextMeshProUGUI

		' Token: 0x040052D1 RID: 21201
		<NonSerialized()>
		Private m_isRegisteredForEvents As Boolean

		' Token: 0x040052D2 RID: 21202
		Private m_materialDirty As Boolean

		' Token: 0x040052D3 RID: 21203
		<SerializeField()>
		Private m_materialReferenceIndex As Integer
	End Class
End Namespace
