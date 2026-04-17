Imports System
Imports System.Collections.Generic
Imports UnityEngine
Imports UnityEngine.UI

Namespace TMPro
	' Token: 0x02000C6B RID: 3179
	<ExecuteInEditMode()>
	<DisallowMultipleComponent()>
	<RequireComponent(GetType(RectTransform))>
	<RequireComponent(GetType(CanvasRenderer))>
	<AddComponentMenu("UI/TextMeshPro - Text", 11)>
	<SelectionBase()>
	Public Class TextMeshProUGUI
		Inherits TMP_Text
		Implements ILayoutElement

		' Token: 0x1700082B RID: 2091
		' (get) Token: 0x06004F86 RID: 20358 RVA: 0x0028C352 File Offset: 0x0028A752
		Public Overrides ReadOnly Property materialForRendering As Material
			Get
				If Me.m_sharedMaterial Is Nothing Then
					Return Nothing
				End If
				Return Me.GetModifiedMaterial(Me.m_sharedMaterial)
			End Get
		End Property

		' Token: 0x1700082C RID: 2092
		' (get) Token: 0x06004F87 RID: 20359 RVA: 0x0028C373 File Offset: 0x0028A773
		Public Overrides ReadOnly Property mesh As Mesh
			Get
				Return Me.m_mesh
			End Get
		End Property

		' Token: 0x1700082D RID: 2093
		' (get) Token: 0x06004F88 RID: 20360 RVA: 0x0028C37B File Offset: 0x0028A77B
		Public ReadOnly Property canvasRenderer As CanvasRenderer
			Get
				If Me.m_canvasRenderer Is Nothing Then
					Me.m_canvasRenderer = MyBase.GetComponent(Of CanvasRenderer)()
				End If
				Return Me.m_canvasRenderer
			End Get
		End Property

		' Token: 0x1700082E RID: 2094
		' (get) Token: 0x06004F89 RID: 20361 RVA: 0x0028C3A0 File Offset: 0x0028A7A0
		Public ReadOnly Property inlineGraphicManager As InlineGraphicManager
			Get
				Return Me.m_inlineGraphics
			End Get
		End Property

		' Token: 0x1700082F RID: 2095
		' (get) Token: 0x06004F8A RID: 20362 RVA: 0x0028C3A8 File Offset: 0x0028A7A8
		Public Overrides ReadOnly Property bounds As Bounds
			Get
				If Me.m_mesh IsNot Nothing Then
					Return Me.m_mesh.bounds
				End If
				Return Nothing
			End Get
		End Property

		' Token: 0x06004F8B RID: 20363 RVA: 0x0028C3DC File Offset: 0x0028A7DC
		Public Sub CalculateLayoutInputHorizontal() Implements UnityEngine.UI.ILayoutElement.CalculateLayoutInputHorizontal
			If Not MyBase.gameObject.activeInHierarchy Then
				Return
			End If
			If Me.m_isCalculateSizeRequired OrElse Me.m_rectTransform.hasChanged Then
				Me.m_preferredWidth = MyBase.GetPreferredWidth()
				Me.ComputeMarginSize()
				Me.m_isLayoutDirty = True
			End If
		End Sub

		' Token: 0x06004F8C RID: 20364 RVA: 0x0028C430 File Offset: 0x0028A830
		Public Sub CalculateLayoutInputVertical() Implements UnityEngine.UI.ILayoutElement.CalculateLayoutInputVertical
			If Not MyBase.gameObject.activeInHierarchy Then
				Return
			End If
			If Me.m_isCalculateSizeRequired OrElse Me.m_rectTransform.hasChanged Then
				Me.m_preferredHeight = MyBase.GetPreferredHeight()
				Me.ComputeMarginSize()
				Me.m_isLayoutDirty = True
			End If
			Me.m_isCalculateSizeRequired = False
		End Sub

		' Token: 0x06004F8D RID: 20365 RVA: 0x0028C489 File Offset: 0x0028A889
		Public Overrides Sub SetVerticesDirty()
			If Me.m_verticesAlreadyDirty OrElse Not Me.IsActive() Then
				Return
			End If
			Me.m_verticesAlreadyDirty = True
			CanvasUpdateRegistry.RegisterCanvasElementForGraphicRebuild(Me)
		End Sub

		' Token: 0x06004F8E RID: 20366 RVA: 0x0028C4AF File Offset: 0x0028A8AF
		Public Overrides Sub SetLayoutDirty()
			If Me.m_layoutAlreadyDirty OrElse Not Me.IsActive() Then
				Return
			End If
			Me.m_layoutAlreadyDirty = True
			LayoutRebuilder.MarkLayoutForRebuild(MyBase.rectTransform)
			Me.m_isLayoutDirty = True
		End Sub

		' Token: 0x06004F8F RID: 20367 RVA: 0x0028C4E1 File Offset: 0x0028A8E1
		Public Overrides Sub SetMaterialDirty()
			If Not Me.IsActive() Then
				Return
			End If
			Me.m_isMaterialDirty = True
			CanvasUpdateRegistry.RegisterCanvasElementForGraphicRebuild(Me)
		End Sub

		' Token: 0x06004F90 RID: 20368 RVA: 0x0028C4FC File Offset: 0x0028A8FC
		Public Overrides Sub SetAllDirty()
			Me.SetLayoutDirty()
			Me.SetVerticesDirty()
			Me.SetMaterialDirty()
		End Sub

		' Token: 0x06004F91 RID: 20369 RVA: 0x0028C510 File Offset: 0x0028A910
		Public Overrides Sub Rebuild(update As CanvasUpdate)
			If update = CanvasUpdate.PreRender Then
				Me.OnPreRenderCanvas()
				Me.m_verticesAlreadyDirty = False
				Me.m_layoutAlreadyDirty = False
				If Not Me.m_isMaterialDirty Then
					Return
				End If
				Me.UpdateMaterial()
				Me.m_isMaterialDirty = False
			End If
		End Sub

		' Token: 0x06004F92 RID: 20370 RVA: 0x0028C548 File Offset: 0x0028A948
		Private Sub UpdateSubObjectPivot()
			If Me.m_textInfo Is Nothing Then
				Return
			End If
			For i As Integer = 1 To Me.m_textInfo.materialCount - 1
				Me.m_subTextObjects(i).SetPivotDirty()
			Next
		End Sub

		' Token: 0x06004F93 RID: 20371 RVA: 0x0028C58C File Offset: 0x0028A98C
		Public Overrides Function GetModifiedMaterial(baseMaterial As Material) As Material
			Dim material As Material = baseMaterial
			If Me.m_ShouldRecalculateStencil Then
				Me.m_stencilID = TMP_MaterialManager.GetStencilID(MyBase.gameObject)
				Me.m_ShouldRecalculateStencil = False
			End If
			If Me.m_stencilID > 0 Then
				material = TMP_MaterialManager.GetStencilMaterial(baseMaterial, Me.m_stencilID)
				If Me.m_MaskMaterial IsNot Nothing Then
					TMP_MaterialManager.ReleaseStencilMaterial(Me.m_MaskMaterial)
				End If
				Me.m_MaskMaterial = material
			End If
			Return material
		End Function

		' Token: 0x06004F94 RID: 20372 RVA: 0x0028C5FC File Offset: 0x0028A9FC
		Protected Overrides Sub UpdateMaterial()
			If Me.m_canvasRenderer Is Nothing Then
				Me.m_canvasRenderer = Me.canvasRenderer
			End If
			Me.m_canvasRenderer.materialCount = 1
			Me.m_canvasRenderer.SetMaterial(Me.materialForRendering, Me.m_sharedMaterial.mainTexture)
		End Sub

		' Token: 0x17000830 RID: 2096
		' (get) Token: 0x06004F95 RID: 20373 RVA: 0x0028C64E File Offset: 0x0028AA4E
		' (set) Token: 0x06004F96 RID: 20374 RVA: 0x0028C656 File Offset: 0x0028AA56
		Public Property maskOffset As Vector4
			Get
				Return Me.m_maskOffset
			End Get
			Set(value As Vector4)
				Me.m_maskOffset = value
				Me.UpdateMask()
				Me.m_havePropertiesChanged = True
			End Set
		End Property

		' Token: 0x06004F97 RID: 20375 RVA: 0x0028C66C File Offset: 0x0028AA6C
		Public Overrides Sub RecalculateClipping()
			MyBase.RecalculateClipping()
		End Sub

		' Token: 0x06004F98 RID: 20376 RVA: 0x0028C674 File Offset: 0x0028AA74
		Public Overrides Sub RecalculateMasking()
			Me.m_ShouldRecalculateStencil = True
			Me.SetMaterialDirty()
		End Sub

		' Token: 0x06004F99 RID: 20377 RVA: 0x0028C684 File Offset: 0x0028AA84
		Public Overrides Sub UpdateMeshPadding()
			Me.m_padding = ShaderUtilities.GetPadding(Me.m_sharedMaterial, Me.m_enableExtraPadding, Me.m_isUsingBold)
			Me.m_isMaskingEnabled = ShaderUtilities.IsMaskingEnabled(Me.m_sharedMaterial)
			Me.m_havePropertiesChanged = True
			Me.checkPaddingRequired = False
			For i As Integer = 1 To Me.m_textInfo.materialCount - 1
				Me.m_subTextObjects(i).UpdateMeshPadding(Me.m_enableExtraPadding, Me.m_isUsingBold)
			Next
		End Sub

		' Token: 0x06004F9A RID: 20378 RVA: 0x0028C702 File Offset: 0x0028AB02
		Public Overrides Sub ForceMeshUpdate()
			Me.m_havePropertiesChanged = True
			Me.OnPreRenderCanvas()
		End Sub

		' Token: 0x06004F9B RID: 20379 RVA: 0x0028C714 File Offset: 0x0028AB14
		Public Overrides Function GetTextInfo(text As String) As TMP_TextInfo
			MyBase.StringToCharArray(text, Me.m_char_buffer)
			Me.SetArraySizes(Me.m_char_buffer)
			Me.m_renderMode = TextRenderFlags.DontRender
			Me.ComputeMarginSize()
			If Me.m_canvas Is Nothing Then
				Me.m_canvas = MyBase.canvas
			End If
			Me.GenerateTextMesh()
			Me.m_renderMode = TextRenderFlags.Render
			Return MyBase.textInfo
		End Function

		' Token: 0x06004F9C RID: 20380 RVA: 0x0028C77C File Offset: 0x0028AB7C
		Public Overrides Sub UpdateGeometry(mesh As Mesh, index As Integer)
			mesh.RecalculateBounds()
			If index = 0 Then
				Me.m_canvasRenderer.SetMesh(mesh)
			Else
				Me.m_subTextObjects(index).canvasRenderer.SetMesh(mesh)
			End If
		End Sub

		' Token: 0x06004F9D RID: 20381 RVA: 0x0028C7B0 File Offset: 0x0028ABB0
		Public Overrides Overloads Sub UpdateVertexData(flags As TMP_VertexDataUpdateFlags)
			Dim materialCount As Integer = Me.m_textInfo.materialCount
			For i As Integer = 0 To materialCount - 1
				Dim mesh As Mesh
				If i = 0 Then
					mesh = Me.m_mesh
				Else
					mesh = Me.m_subTextObjects(i).mesh
				End If
				If(flags And TMP_VertexDataUpdateFlags.Vertices) = TMP_VertexDataUpdateFlags.Vertices Then
					mesh.vertices = Me.m_textInfo.meshInfo(i).vertices
				End If
				If(flags And TMP_VertexDataUpdateFlags.Uv0) = TMP_VertexDataUpdateFlags.Uv0 Then
					mesh.uv = Me.m_textInfo.meshInfo(i).uvs0
				End If
				If(flags And TMP_VertexDataUpdateFlags.Uv2) = TMP_VertexDataUpdateFlags.Uv2 Then
					mesh.uv2 = Me.m_textInfo.meshInfo(i).uvs2
				End If
				If(flags And TMP_VertexDataUpdateFlags.Colors32) = TMP_VertexDataUpdateFlags.Colors32 Then
					mesh.colors32 = Me.m_textInfo.meshInfo(i).colors32
				End If
				mesh.RecalculateBounds()
				If i = 0 Then
					Me.m_canvasRenderer.SetMesh(mesh)
				Else
					Me.m_subTextObjects(i).canvasRenderer.SetMesh(mesh)
				End If
			Next
		End Sub

		' Token: 0x06004F9E RID: 20382 RVA: 0x0028C8C4 File Offset: 0x0028ACC4
		Public Overrides Overloads Sub UpdateVertexData()
			Dim materialCount As Integer = Me.m_textInfo.materialCount
			For i As Integer = 0 To materialCount - 1
				Dim mesh As Mesh
				If i = 0 Then
					mesh = Me.m_mesh
				Else
					mesh = Me.m_subTextObjects(i).mesh
				End If
				mesh.vertices = Me.m_textInfo.meshInfo(i).vertices
				mesh.uv = Me.m_textInfo.meshInfo(i).uvs0
				mesh.uv2 = Me.m_textInfo.meshInfo(i).uvs2
				mesh.colors32 = Me.m_textInfo.meshInfo(i).colors32
				mesh.RecalculateBounds()
				If i = 0 Then
					Me.m_canvasRenderer.SetMesh(mesh)
				Else
					Me.m_subTextObjects(i).canvasRenderer.SetMesh(mesh)
				End If
			Next
		End Sub

		' Token: 0x06004F9F RID: 20383 RVA: 0x0028C9AF File Offset: 0x0028ADAF
		Public Sub UpdateFontAsset()
			Me.LoadFontAsset()
		End Sub

		' Token: 0x06004FA0 RID: 20384 RVA: 0x0028C9B8 File Offset: 0x0028ADB8
		Protected Overrides Sub Awake()
			Me.m_isAwake = True
			Me.m_canvas = MyBase.canvas
			Me.m_isOrthographic = True
			Me.m_rectTransform = MyBase.gameObject.GetComponent(Of RectTransform)()
			If Me.m_rectTransform Is Nothing Then
				Me.m_rectTransform = MyBase.gameObject.AddComponent(Of RectTransform)()
			End If
			Me.m_canvasRenderer = MyBase.GetComponent(Of CanvasRenderer)()
			If Me.m_canvasRenderer Is Nothing Then
				Me.m_canvasRenderer = MyBase.gameObject.AddComponent(Of CanvasRenderer)()
			End If
			If Me.m_mesh Is Nothing Then
				Me.m_mesh = New Mesh()
				Me.m_mesh.hideFlags = HideFlags.HideAndDontSave
			End If
			If Me.m_text Is Nothing Then
				Me.m_enableWordWrapping = TMP_Settings.enableWordWrapping
				Me.m_enableKerning = TMP_Settings.enableKerning
				Me.m_enableExtraPadding = TMP_Settings.enableExtraPadding
				Me.m_tintAllSprites = TMP_Settings.enableTintAllSprites
			End If
			Me.LoadFontAsset()
			TMP_StyleSheet.LoadDefaultStyleSheet()
			Me.m_char_buffer = New Integer(Me.m_max_characters - 1) {}
			Me.m_cached_TextElement = New TMP_Glyph()
			Me.m_isFirstAllocation = True
			Me.m_textInfo = New TMP_TextInfo(Me)
			If Me.m_fontAsset Is Nothing Then
				Return
			End If
			If Me.m_fontSizeMin = 0F Then
				Me.m_fontSizeMin = Me.m_fontSize / 2F
			End If
			If Me.m_fontSizeMax = 0F Then
				Me.m_fontSizeMax = Me.m_fontSize * 2F
			End If
			Me.m_isInputParsingRequired = True
			Me.m_havePropertiesChanged = True
			Me.m_isCalculateSizeRequired = True
		End Sub

		' Token: 0x06004FA1 RID: 20385 RVA: 0x0028CB48 File Offset: 0x0028AF48
		Protected Overrides Sub OnEnable()
			If Not Me.m_isRegisteredForEvents Then
				Me.m_isRegisteredForEvents = True
			End If
			Me.m_canvas = Me.GetCanvas()
			GraphicRegistry.RegisterGraphicForCanvas(Me.m_canvas, Me)
			Me.ComputeMarginSize()
			Me.m_verticesAlreadyDirty = False
			Me.m_layoutAlreadyDirty = False
			Me.m_ShouldRecalculateStencil = True
			Me.SetAllDirty()
			Me.RecalculateClipping()
		End Sub

		' Token: 0x06004FA2 RID: 20386 RVA: 0x0028CBA8 File Offset: 0x0028AFA8
		Protected Overrides Sub OnDisable()
			If Me.m_MaskMaterial IsNot Nothing Then
				TMP_MaterialManager.ReleaseStencilMaterial(Me.m_MaskMaterial)
				Me.m_MaskMaterial = Nothing
			End If
			GraphicRegistry.UnregisterGraphicForCanvas(Me.m_canvas, Me)
			CanvasUpdateRegistry.UnRegisterCanvasElementForRebuild(Me)
			If Me.m_canvasRenderer IsNot Nothing Then
				Me.m_canvasRenderer.Clear()
			End If
			LayoutRebuilder.MarkLayoutForRebuild(Me.m_rectTransform)
			Me.RecalculateClipping()
		End Sub

		' Token: 0x06004FA3 RID: 20387 RVA: 0x0028CC18 File Offset: 0x0028B018
		Protected Overrides Sub OnDestroy()
			GraphicRegistry.UnregisterGraphicForCanvas(Me.m_canvas, Me)
			If Me.m_mesh IsNot Nothing Then
				Global.UnityEngine.[Object].DestroyImmediate(Me.m_mesh)
			End If
			If Me.m_MaskMaterial IsNot Nothing Then
				TMP_MaterialManager.ReleaseStencilMaterial(Me.m_MaskMaterial)
			End If
			Me.m_isRegisteredForEvents = False
		End Sub

		' Token: 0x06004FA4 RID: 20388 RVA: 0x0028CC70 File Offset: 0x0028B070
		Protected Overrides Sub LoadFontAsset()
			ShaderUtilities.GetShaderPropertyIDs()
			If Me.m_fontAsset Is Nothing Then
				If TMP_Settings.defaultFontAsset IsNot Nothing Then
					Me.m_fontAsset = TMP_Settings.defaultFontAsset
				Else
					Me.m_fontAsset = TryCast(Resources.Load("Fonts & Materials/ARIAL SDF", GetType(TMP_FontAsset)), TMP_FontAsset)
				End If
				If Me.m_fontAsset Is Nothing Then
					Return
				End If
				If Me.m_fontAsset.characterDictionary Is Nothing Then
				End If
				Me.m_sharedMaterial = Me.m_fontAsset.material
			Else
				If Me.m_fontAsset.characterDictionary Is Nothing Then
					Me.m_fontAsset.ReadFontDefinition()
				End If
				If Me.m_sharedMaterial Is Nothing AndAlso Me.m_baseMaterial IsNot Nothing Then
					Me.m_sharedMaterial = Me.m_baseMaterial
					Me.m_baseMaterial = Nothing
				End If
				If Me.m_sharedMaterial Is Nothing OrElse Me.m_sharedMaterial.mainTexture Is Nothing OrElse Me.m_fontAsset.atlas.GetInstanceID() <> Me.m_sharedMaterial.mainTexture.GetInstanceID() Then
					If Not(Me.m_fontAsset.material Is Nothing) Then
						Me.m_sharedMaterial = Me.m_fontAsset.material
					End If
				End If
			End If
			MyBase.GetSpecialCharacters(Me.m_fontAsset)
			Me.m_padding = Me.GetPaddingForMaterial()
			Me.SetMaterialDirty()
		End Sub

		' Token: 0x06004FA5 RID: 20389 RVA: 0x0028CDF0 File Offset: 0x0028B1F0
		Private Function GetCanvas() As Canvas
			Dim canvas As Canvas = Nothing
			Dim list As List(Of Canvas) = TMP_ListPool(Of Canvas).[Get]()
			MyBase.gameObject.GetComponentsInParent(Of Canvas)(False, list)
			If list.Count > 0 Then
				canvas = list(0)
			End If
			TMP_ListPool(Of Canvas).Release(list)
			Return canvas
		End Function

		' Token: 0x06004FA6 RID: 20390 RVA: 0x0028CE30 File Offset: 0x0028B230
		Private Sub UpdateEnvMapMatrix()
			If Not Me.m_sharedMaterial.HasProperty(ShaderUtilities.ID_EnvMap) OrElse Me.m_sharedMaterial.GetTexture(ShaderUtilities.ID_EnvMap) Is Nothing Then
				Return
			End If
			Dim vector As Vector3 = Me.m_sharedMaterial.GetVector(ShaderUtilities.ID_EnvMatrixRotation)
			Me.m_EnvMapMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(vector), Vector3.one)
			Me.m_sharedMaterial.SetMatrix(ShaderUtilities.ID_EnvMatrix, Me.m_EnvMapMatrix)
		End Sub

		' Token: 0x06004FA7 RID: 20391 RVA: 0x0028CEB8 File Offset: 0x0028B2B8
		Private Sub EnableMasking()
			If Me.m_fontMaterial Is Nothing Then
				Me.m_fontMaterial = Me.CreateMaterialInstance(Me.m_sharedMaterial)
				Me.m_canvasRenderer.SetMaterial(Me.m_fontMaterial, Me.m_sharedMaterial.mainTexture)
			End If
			Me.m_sharedMaterial = Me.m_fontMaterial
			If Me.m_sharedMaterial.HasProperty(ShaderUtilities.ID_ClipRect) Then
				Me.m_sharedMaterial.EnableKeyword(ShaderUtilities.Keyword_MASK_SOFT)
				Me.m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_HARD)
				Me.m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_TEX)
				Me.UpdateMask()
			End If
			Me.m_isMaskingEnabled = True
		End Sub

		' Token: 0x06004FA8 RID: 20392 RVA: 0x0028CF64 File Offset: 0x0028B364
		Private Sub DisableMasking()
			If Me.m_fontMaterial IsNot Nothing Then
				If Me.m_stencilID > 0 Then
					Me.m_sharedMaterial = Me.m_MaskMaterial
				End If
				Me.m_canvasRenderer.SetMaterial(Me.m_sharedMaterial, Me.m_sharedMaterial.mainTexture)
				Global.UnityEngine.[Object].DestroyImmediate(Me.m_fontMaterial)
			End If
			Me.m_isMaskingEnabled = False
		End Sub

		' Token: 0x06004FA9 RID: 20393 RVA: 0x0028CFC8 File Offset: 0x0028B3C8
		Private Sub UpdateMask()
			If Me.m_rectTransform IsNot Nothing Then
				If Not ShaderUtilities.isInitialized Then
					ShaderUtilities.GetShaderPropertyIDs()
				End If
				Me.m_isScrollRegionSet = True
				Dim num As Single = Mathf.Min(Mathf.Min(Me.m_margin.x, Me.m_margin.z), Me.m_sharedMaterial.GetFloat(ShaderUtilities.ID_MaskSoftnessX))
				Dim num2 As Single = Mathf.Min(Mathf.Min(Me.m_margin.y, Me.m_margin.w), Me.m_sharedMaterial.GetFloat(ShaderUtilities.ID_MaskSoftnessY))
				num = If((num <= 0F), 0F, num)
				num2 = If((num2 <= 0F), 0F, num2)
				Dim num3 As Single = (Me.m_rectTransform.rect.width - Mathf.Max(Me.m_margin.x, 0F) - Mathf.Max(Me.m_margin.z, 0F)) / 2F + num
				Dim num4 As Single = (Me.m_rectTransform.rect.height - Mathf.Max(Me.m_margin.y, 0F) - Mathf.Max(Me.m_margin.w, 0F)) / 2F + num2
				Dim vector As Vector2 = Me.m_rectTransform.localPosition + New Vector3((0.5F - Me.m_rectTransform.pivot.x) * Me.m_rectTransform.rect.width + (Mathf.Max(Me.m_margin.x, 0F) - Mathf.Max(Me.m_margin.z, 0F)) / 2F, (0.5F - Me.m_rectTransform.pivot.y) * Me.m_rectTransform.rect.height + (-Mathf.Max(Me.m_margin.y, 0F) + Mathf.Max(Me.m_margin.w, 0F)) / 2F)
				Dim vector2 As Vector4 = New Vector4(vector.x, vector.y, num3, num4)
				Me.m_sharedMaterial.SetVector(ShaderUtilities.ID_ClipRect, vector2)
			End If
		End Sub

		' Token: 0x06004FAA RID: 20394 RVA: 0x0028D228 File Offset: 0x0028B628
		Protected Overrides Function GetMaterial(mat As Material) As Material
			ShaderUtilities.GetShaderPropertyIDs()
			If Me.m_fontMaterial Is Nothing OrElse Me.m_fontMaterial.GetInstanceID() <> mat.GetInstanceID() Then
				Me.m_fontMaterial = Me.CreateMaterialInstance(mat)
			End If
			Me.m_sharedMaterial = Me.m_fontMaterial
			Me.m_padding = Me.GetPaddingForMaterial()
			Me.m_ShouldRecalculateStencil = True
			Me.SetVerticesDirty()
			Me.SetMaterialDirty()
			Return Me.m_sharedMaterial
		End Function

		' Token: 0x06004FAB RID: 20395 RVA: 0x0028D2A0 File Offset: 0x0028B6A0
		Protected Overrides Function GetMaterials(mats As Material()) As Material()
			Dim materialCount As Integer = Me.m_textInfo.materialCount
			If Me.m_fontMaterials Is Nothing Then
				Me.m_fontMaterials = New Material(materialCount - 1) {}
			ElseIf Me.m_fontMaterials.Length <> materialCount Then
				TMP_TextInfo.Resize(Of Material)(Me.m_fontMaterials, materialCount, False)
			End If
			For i As Integer = 0 To materialCount - 1
				If i = 0 Then
					Me.m_fontMaterials(i) = MyBase.fontMaterial
				Else
					Me.m_fontMaterials(i) = Me.m_subTextObjects(i).material
				End If
			Next
			Me.m_fontSharedMaterials = Me.m_fontMaterials
			Return Me.m_fontMaterials
		End Function

		' Token: 0x06004FAC RID: 20396 RVA: 0x0028D342 File Offset: 0x0028B742
		Protected Overrides Sub SetSharedMaterial(mat As Material)
			Me.m_sharedMaterial = mat
			Me.m_padding = Me.GetPaddingForMaterial()
			Me.SetMaterialDirty()
		End Sub

		' Token: 0x06004FAD RID: 20397 RVA: 0x0028D360 File Offset: 0x0028B760
		Protected Overrides Function GetSharedMaterials() As Material()
			Dim materialCount As Integer = Me.m_textInfo.materialCount
			If Me.m_fontSharedMaterials Is Nothing Then
				Me.m_fontSharedMaterials = New Material(materialCount - 1) {}
			ElseIf Me.m_fontSharedMaterials.Length <> materialCount Then
				TMP_TextInfo.Resize(Of Material)(Me.m_fontSharedMaterials, materialCount, False)
			End If
			For i As Integer = 0 To materialCount - 1
				If i = 0 Then
					Me.m_fontSharedMaterials(i) = Me.m_sharedMaterial
				Else
					Me.m_fontSharedMaterials(i) = Me.m_subTextObjects(i).sharedMaterial
				End If
			Next
			Return Me.m_fontSharedMaterials
		End Function

		' Token: 0x06004FAE RID: 20398 RVA: 0x0028D3F8 File Offset: 0x0028B7F8
		Protected Overrides Sub SetSharedMaterials(materials As Material())
			Dim materialCount As Integer = Me.m_textInfo.materialCount
			If Me.m_fontSharedMaterials Is Nothing Then
				Me.m_fontSharedMaterials = New Material(materialCount - 1) {}
			ElseIf Me.m_fontSharedMaterials.Length <> materialCount Then
				TMP_TextInfo.Resize(Of Material)(Me.m_fontSharedMaterials, materialCount, False)
			End If
			For i As Integer = 0 To materialCount - 1
				If i = 0 Then
					If Not(materials(i).mainTexture Is Nothing) AndAlso materials(i).mainTexture.GetInstanceID() = Me.m_sharedMaterial.mainTexture.GetInstanceID() Then
						Dim fontSharedMaterials As Material() = Me.m_fontSharedMaterials
						Dim num As Integer = i
						Dim material As Material = materials(i)
						Dim material2 As Material = material
						fontSharedMaterials(num) = material
						Me.m_sharedMaterial = material2
						Me.m_padding = Me.GetPaddingForMaterial(Me.m_sharedMaterial)
					End If
				ElseIf Not(materials(i).mainTexture Is Nothing) AndAlso materials(i).mainTexture.GetInstanceID() = Me.m_subTextObjects(i).sharedMaterial.mainTexture.GetInstanceID() Then
					If Me.m_subTextObjects(i).isDefaultMaterial Then
						Dim tmp_SubMeshUI As TMP_SubMeshUI = Me.m_subTextObjects(i)
						Dim fontSharedMaterials2 As Material() = Me.m_fontSharedMaterials
						Dim num2 As Integer = i
						Dim material3 As Material = materials(i)
						Dim material2 As Material = material3
						fontSharedMaterials2(num2) = material3
						tmp_SubMeshUI.sharedMaterial = material2
					End If
				End If
			Next
		End Sub

		' Token: 0x06004FAF RID: 20399 RVA: 0x0028D534 File Offset: 0x0028B934
		Protected Overrides Sub SetOutlineThickness(thickness As Single)
			If Me.m_fontMaterial IsNot Nothing AndAlso Me.m_sharedMaterial.GetInstanceID() <> Me.m_fontMaterial.GetInstanceID() Then
				Me.m_sharedMaterial = Me.m_fontMaterial
				Me.m_canvasRenderer.SetMaterial(Me.m_sharedMaterial, Me.m_sharedMaterial.mainTexture)
			ElseIf Me.m_fontMaterial Is Nothing Then
				Me.m_fontMaterial = Me.CreateMaterialInstance(Me.m_sharedMaterial)
				Me.m_sharedMaterial = Me.m_fontMaterial
				Me.m_canvasRenderer.SetMaterial(Me.m_sharedMaterial, Me.m_sharedMaterial.mainTexture)
			End If
			thickness = Mathf.Clamp01(thickness)
			Me.m_sharedMaterial.SetFloat(ShaderUtilities.ID_OutlineWidth, thickness)
			Me.m_padding = Me.GetPaddingForMaterial()
		End Sub

		' Token: 0x06004FB0 RID: 20400 RVA: 0x0028D60C File Offset: 0x0028BA0C
		Protected Overrides Sub SetFaceColor(color As Color32)
			If Me.m_fontMaterial Is Nothing Then
				Me.m_fontMaterial = Me.CreateMaterialInstance(Me.m_sharedMaterial)
			End If
			Me.m_sharedMaterial = Me.m_fontMaterial
			Me.m_padding = Me.GetPaddingForMaterial()
			Me.m_sharedMaterial.SetColor(ShaderUtilities.ID_FaceColor, color)
		End Sub

		' Token: 0x06004FB1 RID: 20401 RVA: 0x0028D66C File Offset: 0x0028BA6C
		Protected Overrides Sub SetOutlineColor(color As Color32)
			If Me.m_fontMaterial Is Nothing Then
				Me.m_fontMaterial = Me.CreateMaterialInstance(Me.m_sharedMaterial)
			End If
			Me.m_sharedMaterial = Me.m_fontMaterial
			Me.m_padding = Me.GetPaddingForMaterial()
			Me.m_sharedMaterial.SetColor(ShaderUtilities.ID_OutlineColor, color)
		End Sub

		' Token: 0x06004FB2 RID: 20402 RVA: 0x0028D6CC File Offset: 0x0028BACC
		Protected Overrides Sub SetShaderDepth()
			If Me.m_canvas Is Nothing OrElse Me.m_sharedMaterial Is Nothing Then
				Return
			End If
			If Me.m_canvas.renderMode = RenderMode.ScreenSpaceOverlay OrElse Me.m_isOverlay Then
				Me.m_sharedMaterial.SetFloat(ShaderUtilities.ShaderTag_ZTestMode, 0F)
			Else
				Me.m_sharedMaterial.SetFloat(ShaderUtilities.ShaderTag_ZTestMode, 4F)
			End If
		End Sub

		' Token: 0x06004FB3 RID: 20403 RVA: 0x0028D748 File Offset: 0x0028BB48
		Protected Overrides Sub SetCulling()
			If Me.m_isCullingEnabled Then
				Me.m_canvasRenderer.GetMaterial().SetFloat("_CullMode", 2F)
			Else
				Me.m_canvasRenderer.GetMaterial().SetFloat("_CullMode", 0F)
			End If
		End Sub

		' Token: 0x06004FB4 RID: 20404 RVA: 0x0028D799 File Offset: 0x0028BB99
		Private Sub SetPerspectiveCorrection()
			If Me.m_isOrthographic Then
				Me.m_sharedMaterial.SetFloat(ShaderUtilities.ID_PerspectiveFilter, 0F)
			Else
				Me.m_sharedMaterial.SetFloat(ShaderUtilities.ID_PerspectiveFilter, 0.875F)
			End If
		End Sub

		' Token: 0x06004FB5 RID: 20405 RVA: 0x0028D7D8 File Offset: 0x0028BBD8
		Protected Overrides Overloads Function GetPaddingForMaterial(mat As Material) As Single
			Me.m_padding = ShaderUtilities.GetPadding(mat, Me.m_enableExtraPadding, Me.m_isUsingBold)
			Me.m_isMaskingEnabled = ShaderUtilities.IsMaskingEnabled(Me.m_sharedMaterial)
			Me.m_isSDFShader = mat.HasProperty(ShaderUtilities.ID_WeightNormal)
			Return Me.m_padding
		End Function

		' Token: 0x06004FB6 RID: 20406 RVA: 0x0028D828 File Offset: 0x0028BC28
		Protected Overrides Overloads Function GetPaddingForMaterial() As Single
			ShaderUtilities.GetShaderPropertyIDs()
			Me.m_padding = ShaderUtilities.GetPadding(Me.m_sharedMaterial, Me.m_enableExtraPadding, Me.m_isUsingBold)
			Me.m_isMaskingEnabled = ShaderUtilities.IsMaskingEnabled(Me.m_sharedMaterial)
			Me.m_isSDFShader = Me.m_sharedMaterial.HasProperty(ShaderUtilities.ID_WeightNormal)
			Return Me.m_padding
		End Function

		' Token: 0x06004FB7 RID: 20407 RVA: 0x0028D884 File Offset: 0x0028BC84
		Private Sub SetMeshArrays(size As Integer)
			Me.m_textInfo.meshInfo(0).ResizeMeshInfo(size)
			Me.m_canvasRenderer.SetMesh(Me.m_textInfo.meshInfo(0).mesh)
		End Sub

		' Token: 0x06004FB8 RID: 20408 RVA: 0x0028D8C0 File Offset: 0x0028BCC0
		Protected Overrides Function SetArraySizes(chars As Integer()) As Integer
			Dim num As Integer = 0
			Dim num2 As Integer = 0
			Me.m_totalCharacterCount = 0
			Me.m_isUsingBold = False
			Me.m_isParsingText = False
			Me.tag_NoParsing = False
			Me.m_style = Me.m_fontStyle
			Me.m_currentFontAsset = Me.m_fontAsset
			Me.m_currentMaterial = Me.m_sharedMaterial
			Me.m_currentMaterialIndex = 0
			Me.m_materialReferenceStack.SetDefault(New MaterialReference(0, Me.m_currentFontAsset, Nothing, Me.m_currentMaterial, Me.m_padding))
			Me.m_materialReferenceIndexLookup.Clear()
			MaterialReference.AddMaterialReference(Me.m_currentMaterial, Me.m_currentFontAsset, Me.m_materialReferences, Me.m_materialReferenceIndexLookup)
			If Me.m_textInfo Is Nothing Then
				Me.m_textInfo = New TMP_TextInfo()
			End If
			Me.m_textElementType = TMP_TextElementType.Character
			Dim num3 As Integer = 0
			Dim num6 As Integer
			While chars(num3) <> 0
				If Me.m_textInfo.characterInfo Is Nothing OrElse Me.m_totalCharacterCount >= Me.m_textInfo.characterInfo.Length Then
					TMP_TextInfo.Resize(Of TMP_CharacterInfo)(Me.m_textInfo.characterInfo, Me.m_totalCharacterCount + 1, True)
				End If
				Dim num4 As Integer = chars(num3)
				If Not Me.m_isRichText OrElse num4 <> 60 Then
					GoTo IL_01FE
				End If
				Dim currentMaterialIndex As Integer = Me.m_currentMaterialIndex
				If Not MyBase.ValidateHtmlTag(chars, num3 + 1, num) Then
					GoTo IL_01FE
				End If
				num3 = num
				If(Me.m_style And FontStyles.Bold) = FontStyles.Bold Then
					Me.m_isUsingBold = True
				End If
				If Me.m_textElementType = TMP_TextElementType.Sprite Then
					Dim materialReferences As MaterialReference() = Me.m_materialReferences
					Dim currentMaterialIndex2 As Integer = Me.m_currentMaterialIndex
					materialReferences(currentMaterialIndex2).referenceCount = materialReferences(currentMaterialIndex2).referenceCount + 1
					Me.m_textInfo.characterInfo(Me.m_totalCharacterCount).character = CChar((57344 + Me.m_spriteIndex))
					Me.m_textInfo.characterInfo(Me.m_totalCharacterCount).fontAsset = Me.m_currentFontAsset
					Me.m_textInfo.characterInfo(Me.m_totalCharacterCount).materialReferenceIndex = Me.m_currentMaterialIndex
					Me.m_textElementType = TMP_TextElementType.Character
					Me.m_currentMaterialIndex = currentMaterialIndex
					num2 += 1
					Me.m_totalCharacterCount += 1
				End If
				IL_07BE:
				num3 += 1
				Continue While
				IL_01FE:
				Dim flag As Boolean = False
				Dim flag2 As Boolean = False
				Dim currentFontAsset As TMP_FontAsset = Me.m_currentFontAsset
				Dim currentMaterial As Material = Me.m_currentMaterial
				Dim currentMaterialIndex3 As Integer = Me.m_currentMaterialIndex
				If Me.m_textElementType = TMP_TextElementType.Character Then
					If(Me.m_style And FontStyles.UpperCase) = FontStyles.UpperCase Then
						If Char.IsLower(CChar(num4)) Then
							num4 = CInt(Char.ToUpper(CChar(num4)))
						End If
					ElseIf(Me.m_style And FontStyles.LowerCase) = FontStyles.LowerCase Then
						If Char.IsUpper(CChar(num4)) Then
							num4 = CInt(Char.ToLower(CChar(num4)))
						End If
					ElseIf((Me.m_fontStyle And FontStyles.SmallCaps) = FontStyles.SmallCaps OrElse (Me.m_style And FontStyles.SmallCaps) = FontStyles.SmallCaps) AndAlso Char.IsLower(CChar(num4)) Then
						num4 = CInt(Char.ToUpper(CChar(num4)))
					End If
				End If
				Dim tmp_Glyph As TMP_Glyph
				If Not Me.m_currentFontAsset.characterDictionary.TryGetValue(num4, tmp_Glyph) Then
					If Me.m_currentFontAsset.fallbackFontAssets IsNot Nothing AndAlso Me.m_currentFontAsset.fallbackFontAssets.Count > 0 Then
						For i As Integer = 0 To Me.m_currentFontAsset.fallbackFontAssets.Count - 1
							Dim tmp_FontAsset As TMP_FontAsset = Me.m_currentFontAsset.fallbackFontAssets(i)
							If Not(tmp_FontAsset Is Nothing) Then
								If tmp_FontAsset.characterDictionary.TryGetValue(num4, tmp_Glyph) Then
									flag = True
									Me.m_currentFontAsset = tmp_FontAsset
									Me.m_currentMaterial = tmp_FontAsset.material
									Me.m_currentMaterialIndex = MaterialReference.AddMaterialReference(Me.m_currentMaterial, tmp_FontAsset, Me.m_materialReferences, Me.m_materialReferenceIndexLookup)
									Exit For
								End If
							End If
						Next
					End If
					If tmp_Glyph Is Nothing AndAlso TMP_Settings.fallbackFontAssets IsNot Nothing AndAlso TMP_Settings.fallbackFontAssets.Count > 0 Then
						For j As Integer = 0 To TMP_Settings.fallbackFontAssets.Count - 1
							Dim tmp_FontAsset As TMP_FontAsset = TMP_Settings.fallbackFontAssets(j)
							If Not(tmp_FontAsset Is Nothing) Then
								If tmp_FontAsset.characterDictionary.TryGetValue(num4, tmp_Glyph) Then
									flag = True
									Me.m_currentFontAsset = tmp_FontAsset
									Me.m_currentMaterial = tmp_FontAsset.material
									Me.m_currentMaterialIndex = MaterialReference.AddMaterialReference(Me.m_currentMaterial, tmp_FontAsset, Me.m_materialReferences, Me.m_materialReferenceIndexLookup)
									Exit For
								End If
							End If
						Next
					End If
					If tmp_Glyph Is Nothing Then
						If Char.IsLower(CChar(num4)) Then
							If Me.m_currentFontAsset.characterDictionary.TryGetValue(CInt(Char.ToUpper(CChar(num4))), tmp_Glyph) Then
								Dim num5 As Integer = num3
								Dim c As Char = Char.ToUpper(CChar(num4))
								num6 = CInt(c)
								chars(num5) = CInt(c)
								num4 = num6
							End If
						ElseIf Char.IsUpper(CChar(num4)) AndAlso Me.m_currentFontAsset.characterDictionary.TryGetValue(CInt(Char.ToLower(CChar(num4))), tmp_Glyph) Then
							Dim num7 As Integer = num3
							Dim c2 As Char = Char.ToLower(CChar(num4))
							num6 = CInt(c2)
							chars(num7) = CInt(c2)
							num4 = num6
						End If
					End If
					If tmp_Glyph Is Nothing Then
						If Me.m_currentFontAsset.characterDictionary.TryGetValue(9633, tmp_Glyph) Then
							If Not TMP_Settings.warningsDisabled Then
							End If
							Dim num8 As Integer = num3
							Dim num9 As Integer = 9633
							num6 = num9
							chars(num8) = num9
							num4 = num6
						Else
							If TMP_Settings.fallbackFontAssets IsNot Nothing AndAlso TMP_Settings.fallbackFontAssets.Count > 0 Then
								For k As Integer = 0 To TMP_Settings.fallbackFontAssets.Count - 1
									Dim tmp_FontAsset As TMP_FontAsset = TMP_Settings.fallbackFontAssets(k)
									If Not(tmp_FontAsset Is Nothing) Then
										If tmp_FontAsset.characterDictionary.TryGetValue(9633, tmp_Glyph) Then
											If Not TMP_Settings.warningsDisabled Then
											End If
											Dim num10 As Integer = num3
											Dim num11 As Integer = 9633
											num6 = num11
											chars(num10) = num11
											num4 = num6
											flag = True
											Me.m_currentFontAsset = tmp_FontAsset
											Me.m_currentMaterial = tmp_FontAsset.material
											Me.m_currentMaterialIndex = MaterialReference.AddMaterialReference(Me.m_currentMaterial, tmp_FontAsset, Me.m_materialReferences, Me.m_materialReferenceIndexLookup)
											Exit For
										End If
									End If
								Next
							End If
							If tmp_Glyph Is Nothing Then
								Dim tmp_FontAsset As TMP_FontAsset = TMP_Settings.GetFontAsset()
								If tmp_FontAsset IsNot Nothing AndAlso tmp_FontAsset.characterDictionary.TryGetValue(9633, tmp_Glyph) Then
									If Not TMP_Settings.warningsDisabled Then
									End If
									Dim num12 As Integer = num3
									Dim num13 As Integer = 9633
									num6 = num13
									chars(num12) = num13
									num4 = num6
									flag = True
									Me.m_currentFontAsset = tmp_FontAsset
									Me.m_currentMaterial = tmp_FontAsset.material
									Me.m_currentMaterialIndex = MaterialReference.AddMaterialReference(Me.m_currentMaterial, tmp_FontAsset, Me.m_materialReferences, Me.m_materialReferenceIndexLookup)
								Else
									tmp_FontAsset = TMP_FontAsset.defaultFontAsset
									If tmp_FontAsset IsNot Nothing AndAlso tmp_FontAsset.characterDictionary.TryGetValue(9633, tmp_Glyph) Then
										If Not TMP_Settings.warningsDisabled Then
										End If
										Dim num14 As Integer = num3
										Dim num15 As Integer = 9633
										num6 = num15
										chars(num14) = num15
										num4 = num6
										flag = True
										Me.m_currentFontAsset = tmp_FontAsset
										Me.m_currentMaterial = tmp_FontAsset.material
										Me.m_currentMaterialIndex = MaterialReference.AddMaterialReference(Me.m_currentMaterial, tmp_FontAsset, Me.m_materialReferences, Me.m_materialReferenceIndexLookup)
									End If
								End If
							End If
						End If
					End If
				End If
				Me.m_textInfo.characterInfo(Me.m_totalCharacterCount).textElement = tmp_Glyph
				Me.m_textInfo.characterInfo(Me.m_totalCharacterCount).character = CChar(num4)
				Me.m_textInfo.characterInfo(Me.m_totalCharacterCount).fontAsset = Me.m_currentFontAsset
				Me.m_textInfo.characterInfo(Me.m_totalCharacterCount).material = Me.m_currentMaterial
				Me.m_textInfo.characterInfo(Me.m_totalCharacterCount).materialReferenceIndex = Me.m_currentMaterialIndex
				If Not Char.IsWhiteSpace(CChar(num4)) Then
					Dim materialReferences2 As MaterialReference() = Me.m_materialReferences
					Dim currentMaterialIndex4 As Integer = Me.m_currentMaterialIndex
					materialReferences2(currentMaterialIndex4).referenceCount = materialReferences2(currentMaterialIndex4).referenceCount + 1
					If flag2 Then
						Me.m_currentMaterialIndex = currentMaterialIndex3
					End If
					If flag Then
						Me.m_currentFontAsset = currentFontAsset
						Me.m_currentMaterial = currentMaterial
						Me.m_currentMaterialIndex = currentMaterialIndex3
					End If
				End If
				Me.m_totalCharacterCount += 1
				GoTo IL_07BE
			End While
			Me.m_textInfo.spriteCount = num2
			Dim textInfo As TMP_TextInfo = Me.m_textInfo
			Dim count As Integer = Me.m_materialReferenceIndexLookup.Count
			num6 = count
			textInfo.materialCount = count
			Dim num16 As Integer = num6
			If num16 > Me.m_textInfo.meshInfo.Length Then
				TMP_TextInfo.Resize(Of TMP_MeshInfo)(Me.m_textInfo.meshInfo, num16, False)
			End If
			For l As Integer = 0 To num16 - 1
				If l > 0 Then
					If Me.m_subTextObjects(l) Is Nothing Then
						Me.m_subTextObjects(l) = TMP_SubMeshUI.AddSubTextObject(Me, Me.m_materialReferences(l))
						Me.m_textInfo.meshInfo(l).vertices = Nothing
					End If
					If Me.m_rectTransform.pivot <> Me.m_subTextObjects(l).rectTransform.pivot Then
						Me.m_subTextObjects(l).rectTransform.pivot = Me.m_rectTransform.pivot
					End If
					If Me.m_subTextObjects(l).sharedMaterial Is Nothing OrElse Me.m_subTextObjects(l).sharedMaterial.GetInstanceID() <> Me.m_materialReferences(l).material.GetInstanceID() Then
						Dim isDefaultMaterial As Boolean = Me.m_materialReferences(l).isDefaultMaterial
						Me.m_subTextObjects(l).isDefaultMaterial = isDefaultMaterial
						If Not isDefaultMaterial OrElse Me.m_subTextObjects(l).sharedMaterial Is Nothing OrElse Me.m_subTextObjects(l).sharedMaterial.mainTexture.GetInstanceID() <> Me.m_materialReferences(l).material.mainTexture.GetInstanceID() Then
							Me.m_subTextObjects(l).sharedMaterial = Me.m_materialReferences(l).material
							Me.m_subTextObjects(l).fontAsset = Me.m_materialReferences(l).fontAsset
							Me.m_subTextObjects(l).spriteAsset = Me.m_materialReferences(l).spriteAsset
						End If
					End If
				End If
				Dim referenceCount As Integer = Me.m_materialReferences(l).referenceCount
				If Me.m_textInfo.meshInfo(l).vertices Is Nothing OrElse Me.m_textInfo.meshInfo(l).vertices.Length < referenceCount * 4 Then
					If Me.m_textInfo.meshInfo(l).vertices Is Nothing Then
						If l = 0 Then
							Me.m_textInfo.meshInfo(l) = New TMP_MeshInfo(Me.m_mesh, referenceCount + 1)
						Else
							Me.m_textInfo.meshInfo(l) = New TMP_MeshInfo(Me.m_subTextObjects(l).mesh, referenceCount + 1)
						End If
					Else
						Me.m_textInfo.meshInfo(l).ResizeMeshInfo(If((referenceCount <= 1024), Mathf.NextPowerOfTwo(referenceCount), (referenceCount + 256)))
					End If
				End If
			Next
			Dim num17 As Integer = num16
			While num17 < Me.m_subTextObjects.Length + 1 AndAlso Me.m_subTextObjects(num17) IsNot Nothing
				If num17 < Me.m_textInfo.meshInfo.Length Then
					Me.m_subTextObjects(num17).canvasRenderer.SetMesh(Nothing)
				End If
				num17 += 1
			End While
			Return Me.m_totalCharacterCount
		End Function

		' Token: 0x06004FB9 RID: 20409 RVA: 0x0028E42C File Offset: 0x0028C82C
		Protected Overrides Sub ComputeMarginSize()
			If MyBase.rectTransform IsNot Nothing Then
				Me.m_marginWidth = Me.m_rectTransform.rect.width - Me.m_margin.x - Me.m_margin.z
				Me.m_marginHeight = Me.m_rectTransform.rect.height - Me.m_margin.y - Me.m_margin.w
				Me.m_RectTransformCorners = Me.GetTextContainerLocalCorners()
			End If
		End Sub

		' Token: 0x06004FBA RID: 20410 RVA: 0x0028E4B8 File Offset: 0x0028C8B8
		Protected Overrides Sub OnDidApplyAnimationProperties()
			Me.m_havePropertiesChanged = True
			Me.SetVerticesDirty()
			Me.SetLayoutDirty()
		End Sub

		' Token: 0x06004FBB RID: 20411 RVA: 0x0028E4CD File Offset: 0x0028C8CD
		Protected Overrides Sub OnTransformParentChanged()
			MyBase.OnTransformParentChanged()
			Me.m_canvas = MyBase.canvas
			Me.ComputeMarginSize()
			Me.m_havePropertiesChanged = True
		End Sub

		' Token: 0x06004FBC RID: 20412 RVA: 0x0028E4EE File Offset: 0x0028C8EE
		Protected Overrides Sub OnRectTransformDimensionsChange()
			If Not MyBase.gameObject.activeInHierarchy Then
				Return
			End If
			Me.ComputeMarginSize()
			Me.UpdateSubObjectPivot()
			Me.SetVerticesDirty()
			Me.SetLayoutDirty()
		End Sub

		' Token: 0x06004FBD RID: 20413 RVA: 0x0028E51C File Offset: 0x0028C91C
		Private Sub LateUpdate()
			If Me.m_rectTransform.hasChanged Then
				Dim lossyScale As Vector3 = Me.m_rectTransform.lossyScale
				If Not Me.m_havePropertiesChanged AndAlso lossyScale.y <> Me.m_previousLossyScale.y AndAlso Me.m_text <> String.Empty AndAlso Me.m_text IsNot Nothing Then
					Me.UpdateSDFScale(lossyScale.y)
					Me.m_previousLossyScale = lossyScale
				End If
				Me.m_rectTransform.hasChanged = False
			End If
		End Sub

		' Token: 0x06004FBE RID: 20414 RVA: 0x0028E5A8 File Offset: 0x0028C9A8
		Private Sub OnPreRenderCanvas()
			If Not Me.IsActive() Then
				Return
			End If
			If Me.m_canvas Is Nothing Then
				Me.m_canvas = MyBase.canvas
				If Me.m_canvas Is Nothing Then
					Return
				End If
			End If
			Me.loopCountA = 0
			If Me.m_havePropertiesChanged OrElse Me.m_isLayoutDirty Then
				If Me.checkPaddingRequired Then
					Me.UpdateMeshPadding()
				End If
				If Me.m_isInputParsingRequired OrElse Me.m_isTextTruncated Then
					MyBase.ParseInputText()
				End If
				If Me.m_enableAutoSizing Then
					Me.m_fontSize = Mathf.Clamp(Me.m_fontSize, Me.m_fontSizeMin, Me.m_fontSizeMax)
				End If
				Me.m_maxFontSize = Me.m_fontSizeMax
				Me.m_minFontSize = Me.m_fontSizeMin
				Me.m_lineSpacingDelta = 0F
				Me.m_charWidthAdjDelta = 0F
				Me.m_recursiveCount = 0
				Me.m_isCharacterWrappingEnabled = False
				Me.m_isTextTruncated = False
				Me.m_isLayoutDirty = False
				Me.GenerateTextMesh()
				Me.m_havePropertiesChanged = False
			End If
		End Sub

		' Token: 0x06004FBF RID: 20415 RVA: 0x0028E6BC File Offset: 0x0028CABC
		Protected Overrides Sub GenerateTextMesh()
			If Me.m_fontAsset Is Nothing OrElse Me.m_fontAsset.characterDictionary Is Nothing Then
				Return
			End If
			If Me.m_textInfo IsNot Nothing Then
				Me.m_textInfo.Clear()
			End If
			If Me.m_char_buffer Is Nothing OrElse Me.m_char_buffer.Length = 0 OrElse Me.m_char_buffer(0) = 0 Then
				Me.ClearMesh()
				Me.m_preferredWidth = 0F
				Me.m_preferredHeight = 0F
				Return
			End If
			Me.m_currentFontAsset = Me.m_fontAsset
			Me.m_currentMaterial = Me.m_sharedMaterial
			Me.m_currentMaterialIndex = 0
			Me.m_materialReferenceStack.SetDefault(New MaterialReference(0, Me.m_currentFontAsset, Nothing, Me.m_currentMaterial, Me.m_padding))
			Me.m_currentSpriteAsset = Me.m_spriteAsset
			Dim totalCharacterCount As Integer = Me.m_totalCharacterCount
			Me.m_fontScale = Me.m_fontSize / Me.m_currentFontAsset.fontInfo.PointSize
			Dim num As Single = Me.m_fontSize / Me.m_fontAsset.fontInfo.PointSize * Me.m_fontAsset.fontInfo.Scale
			Dim num2 As Single = Me.m_fontScale
			Me.m_fontScaleMultiplier = 1F
			Me.m_currentFontSize = Me.m_fontSize
			Me.m_sizeStack.SetDefault(Me.m_currentFontSize)
			Me.m_style = Me.m_fontStyle
			Me.m_lineJustification = Me.m_textAlignment
			Dim num3 As Single = 0F
			Dim num4 As Single = 1F
			Me.m_baselineOffset = 0F
			Dim flag As Boolean = False
			Dim zero As Vector3 = Vector3.zero
			Dim zero2 As Vector3 = Vector3.zero
			Dim flag2 As Boolean = False
			Dim zero3 As Vector3 = Vector3.zero
			Dim zero4 As Vector3 = Vector3.zero
			Me.m_fontColor32 = Me.m_fontColor
			Me.m_htmlColor = Me.m_fontColor32
			Me.m_colorStack.SetDefault(Me.m_htmlColor)
			Me.m_styleStack.Clear()
			Me.m_actionStack.Clear()
			Me.m_lineOffset = 0F
			Me.m_lineHeight = 0F
			Dim num5 As Single = Me.m_currentFontAsset.fontInfo.LineHeight - (Me.m_currentFontAsset.fontInfo.Ascender - Me.m_currentFontAsset.fontInfo.Descender)
			Me.m_cSpacing = 0F
			Me.m_monoSpacing = 0F
			Me.m_xAdvance = 0F
			Me.tag_LineIndent = 0F
			Me.tag_Indent = 0F
			Me.m_indentStack.SetDefault(0F)
			Me.tag_NoParsing = False
			Me.m_characterCount = 0
			Me.m_visibleCharacterCount = 0
			Me.m_firstCharacterOfLine = 0
			Me.m_lastCharacterOfLine = 0
			Me.m_firstVisibleCharacterOfLine = 0
			Me.m_lastVisibleCharacterOfLine = 0
			Me.m_maxLineAscender = Single.NegativeInfinity
			Me.m_maxLineDescender = Single.PositiveInfinity
			Me.m_lineNumber = 0
			Dim flag3 As Boolean = True
			Me.m_pageNumber = 0
			Dim num6 As Integer = Mathf.Clamp(Me.m_pageToDisplay - 1, 0, Me.m_textInfo.pageInfo.Length - 1)
			Dim num7 As Integer = 0
			Dim margin As Vector4 = Me.m_margin
			Dim marginWidth As Single = Me.m_marginWidth
			Dim marginHeight As Single = Me.m_marginHeight
			Me.m_marginLeft = 0F
			Me.m_marginRight = 0F
			Me.m_width = -1F
			Me.m_meshExtents.min = TMP_Text.k_InfinityVectorPositive
			Me.m_meshExtents.max = TMP_Text.k_InfinityVectorNegative
			Me.m_textInfo.ClearLineInfo()
			Me.m_maxAscender = 0F
			Me.m_maxDescender = 0F
			Dim num8 As Single = 0F
			Dim num9 As Single = 0F
			Dim flag4 As Boolean = False
			Me.m_isNewPage = False
			Dim flag5 As Boolean = True
			Dim flag6 As Boolean = False
			Dim num10 As Integer = 0
			Me.loopCountA += 1
			Dim num11 As Integer = 0
			Dim num12 As Integer = 0
			While Me.m_char_buffer(num12) <> 0
				Dim num13 As Integer = Me.m_char_buffer(num12)
				Me.m_textElementType = TMP_TextElementType.Character
				Me.m_currentMaterialIndex = Me.m_textInfo.characterInfo(Me.m_characterCount).materialReferenceIndex
				Me.m_currentFontAsset = Me.m_materialReferences(Me.m_currentMaterialIndex).fontAsset
				Dim currentMaterialIndex As Integer = Me.m_currentMaterialIndex
				If Not Me.m_isRichText OrElse num13 <> 60 Then
					GoTo IL_044F
				End If
				Me.m_isParsingText = True
				If Not MyBase.ValidateHtmlTag(Me.m_char_buffer, num12 + 1, num11) Then
					GoTo IL_044F
				End If
				num12 = num11
				If Me.m_textElementType <> TMP_TextElementType.Character Then
					GoTo IL_044F
				End If
				IL_285B:
				num12 += 1
				Continue While
				IL_044F:
				Me.m_isParsingText = False
				Dim flag7 As Boolean = False
				Dim flag8 As Boolean = False
				Dim num14 As Single = 1F
				If Me.m_textElementType = TMP_TextElementType.Character Then
					If(Me.m_style And FontStyles.UpperCase) = FontStyles.UpperCase Then
						If Char.IsLower(CChar(num13)) Then
							num13 = CInt(Char.ToUpper(CChar(num13)))
						End If
					ElseIf(Me.m_style And FontStyles.LowerCase) = FontStyles.LowerCase Then
						If Char.IsUpper(CChar(num13)) Then
							num13 = CInt(Char.ToLower(CChar(num13)))
						End If
					ElseIf((Me.m_fontStyle And FontStyles.SmallCaps) = FontStyles.SmallCaps OrElse (Me.m_style And FontStyles.SmallCaps) = FontStyles.SmallCaps) AndAlso Char.IsLower(CChar(num13)) Then
						num14 = 0.8F
						num13 = CInt(Char.ToUpper(CChar(num13)))
					End If
				End If
				If Me.m_textElementType = TMP_TextElementType.Sprite Then
					Dim tmp_Sprite As TMP_Sprite = Me.m_currentSpriteAsset.spriteInfoList(Me.m_spriteIndex)
					num13 = 57344 + Me.m_spriteIndex
					Me.m_currentFontAsset = Me.m_fontAsset
					Dim num15 As Single = Me.m_currentFontSize / Me.m_fontAsset.fontInfo.PointSize * Me.m_fontAsset.fontInfo.Scale
					num2 = Me.m_fontAsset.fontInfo.Ascender / tmp_Sprite.height * tmp_Sprite.scale * num15
					Me.m_cached_TextElement = tmp_Sprite
					Me.m_textInfo.characterInfo(Me.m_characterCount).elementType = TMP_TextElementType.Sprite
					Me.m_textInfo.characterInfo(Me.m_characterCount).scale = num15
					Me.m_textInfo.characterInfo(Me.m_characterCount).spriteAsset = Me.m_currentSpriteAsset
					Me.m_textInfo.characterInfo(Me.m_characterCount).fontAsset = Me.m_currentFontAsset
					Me.m_textInfo.characterInfo(Me.m_characterCount).materialReferenceIndex = Me.m_currentMaterialIndex
					Me.m_currentMaterialIndex = currentMaterialIndex
					num3 = 0F
				ElseIf Me.m_textElementType = TMP_TextElementType.Character Then
					Me.m_cached_TextElement = Me.m_textInfo.characterInfo(Me.m_characterCount).textElement
					If Me.m_cached_TextElement Is Nothing Then
						GoTo IL_285B
					End If
					Me.m_currentFontAsset = Me.m_textInfo.characterInfo(Me.m_characterCount).fontAsset
					Me.m_currentMaterial = Me.m_textInfo.characterInfo(Me.m_characterCount).material
					Me.m_currentMaterialIndex = Me.m_textInfo.characterInfo(Me.m_characterCount).materialReferenceIndex
					Me.m_fontScale = Me.m_currentFontSize * num14 / Me.m_currentFontAsset.fontInfo.PointSize * Me.m_currentFontAsset.fontInfo.Scale
					num2 = Me.m_fontScale * Me.m_fontScaleMultiplier
					Me.m_textInfo.characterInfo(Me.m_characterCount).elementType = TMP_TextElementType.Character
					Me.m_textInfo.characterInfo(Me.m_characterCount).scale = num2
					num3 = If((Me.m_currentMaterialIndex <> 0), Me.m_subTextObjects(Me.m_currentMaterialIndex).padding, Me.m_padding)
				End If
				If Me.m_isRightToLeft Then
					Me.m_xAdvance -= ((Me.m_cached_TextElement.xAdvance * num4 + Me.m_characterSpacing + Me.m_currentFontAsset.normalSpacingOffset) * num2 + Me.m_cSpacing) * (1F - Me.m_charWidthAdjDelta)
				End If
				Me.m_textInfo.characterInfo(Me.m_characterCount).character = CChar(num13)
				Me.m_textInfo.characterInfo(Me.m_characterCount).pointSize = Me.m_currentFontSize
				Me.m_textInfo.characterInfo(Me.m_characterCount).color = Me.m_htmlColor
				Me.m_textInfo.characterInfo(Me.m_characterCount).style = Me.m_style
				Me.m_textInfo.characterInfo(Me.m_characterCount).index = CShort(num12)
				If Me.m_enableKerning AndAlso Me.m_characterCount >= 1 Then
					Dim character As Integer = CInt(Me.m_textInfo.characterInfo(Me.m_characterCount - 1).character)
					Dim kerningPairKey As KerningPairKey = New KerningPairKey(character, num13)
					Dim kerningPair As KerningPair
					Me.m_currentFontAsset.kerningDictionary.TryGetValue(kerningPairKey.key, kerningPair)
					If kerningPair IsNot Nothing Then
						Me.m_xAdvance += kerningPair.XadvanceOffset * num2
					End If
				End If
				Dim num16 As Single = 0F
				If Me.m_monoSpacing <> 0F Then
					num16 = (Me.m_monoSpacing / 2F - (Me.m_cached_TextElement.width / 2F + Me.m_cached_TextElement.xOffset) * num2) * (1F - Me.m_charWidthAdjDelta)
					Me.m_xAdvance += num16
				End If
				Dim num17 As Single
				If Me.m_textElementType = TMP_TextElementType.Character AndAlso Not flag8 AndAlso ((Me.m_style And FontStyles.Bold) = FontStyles.Bold OrElse (Me.m_fontStyle And FontStyles.Bold) = FontStyles.Bold) Then
					num17 = Me.m_currentFontAsset.boldStyle * 2F
					num4 = 1F + Me.m_currentFontAsset.boldSpacing * 0.01F
				Else
					num17 = Me.m_currentFontAsset.normalStyle * 2F
					num4 = 1F
				End If
				Dim baseline As Single = Me.m_currentFontAsset.fontInfo.Baseline
				Dim vector As Vector3
				vector.x = Me.m_xAdvance + (Me.m_cached_TextElement.xOffset - num3 - num17) * num2 * (1F - Me.m_charWidthAdjDelta)
				vector.y = (baseline + Me.m_cached_TextElement.yOffset + num3) * num2 - Me.m_lineOffset + Me.m_baselineOffset
				vector.z = 0F
				Dim vector2 As Vector3
				vector2.x = vector.x
				vector2.y = vector.y - (Me.m_cached_TextElement.height + num3 * 2F) * num2
				vector2.z = 0F
				Dim vector3 As Vector3
				vector3.x = vector2.x + (Me.m_cached_TextElement.width + num3 * 2F + num17 * 2F) * num2 * (1F - Me.m_charWidthAdjDelta)
				vector3.y = vector.y
				vector3.z = 0F
				Dim vector4 As Vector3
				vector4.x = vector3.x
				vector4.y = vector2.y
				vector4.z = 0F
				If Me.m_textElementType = TMP_TextElementType.Character AndAlso Not flag8 AndAlso ((Me.m_style And FontStyles.Italic) = FontStyles.Italic OrElse (Me.m_fontStyle And FontStyles.Italic) = FontStyles.Italic) Then
					Dim num18 As Single = CSng(Me.m_currentFontAsset.italicStyle) * 0.01F
					Dim vector5 As Vector3 = New Vector3(num18 * ((Me.m_cached_TextElement.yOffset + num3 + num17) * num2), 0F, 0F)
					Dim vector6 As Vector3 = New Vector3(num18 * ((Me.m_cached_TextElement.yOffset - Me.m_cached_TextElement.height - num3 - num17) * num2), 0F, 0F)
					vector += vector5
					vector2 += vector6
					vector3 += vector5
					vector4 += vector6
				End If
				Me.m_textInfo.characterInfo(Me.m_characterCount).bottomLeft = vector2
				Me.m_textInfo.characterInfo(Me.m_characterCount).topLeft = vector
				Me.m_textInfo.characterInfo(Me.m_characterCount).topRight = vector3
				Me.m_textInfo.characterInfo(Me.m_characterCount).bottomRight = vector4
				Me.m_textInfo.characterInfo(Me.m_characterCount).origin = Me.m_xAdvance
				Me.m_textInfo.characterInfo(Me.m_characterCount).baseLine = 0F - Me.m_lineOffset + Me.m_baselineOffset
				Me.m_textInfo.characterInfo(Me.m_characterCount).aspectRatio = (vector3.x - vector2.x) / (vector.y - vector2.y)
				Dim num19 As Single = Me.m_currentFontAsset.fontInfo.Ascender * If((Me.m_textElementType <> TMP_TextElementType.Character), Me.m_textInfo.characterInfo(Me.m_characterCount).scale, num2) + Me.m_baselineOffset
				Me.m_textInfo.characterInfo(Me.m_characterCount).ascender = num19 - Me.m_lineOffset
				Me.m_maxLineAscender = If((num19 <= Me.m_maxLineAscender), Me.m_maxLineAscender, num19)
				Dim num20 As Single = Me.m_currentFontAsset.fontInfo.Descender * If((Me.m_textElementType <> TMP_TextElementType.Character), Me.m_textInfo.characterInfo(Me.m_characterCount).scale, num2) + Me.m_baselineOffset
				Dim characterInfo As TMP_CharacterInfo() = Me.m_textInfo.characterInfo
				Dim characterCount As Integer = Me.m_characterCount
				Dim num21 As Single = num20 - Me.m_lineOffset
				Dim num22 As Single = num21
				characterInfo(characterCount).descender = num21
				Dim num23 As Single = num22
				Me.m_maxLineDescender = If((num20 >= Me.m_maxLineDescender), Me.m_maxLineDescender, num20)
				If(Me.m_style And FontStyles.Subscript) = FontStyles.Subscript OrElse (Me.m_style And FontStyles.Superscript) = FontStyles.Superscript Then
					Dim num24 As Single = (num19 - Me.m_baselineOffset) / Me.m_currentFontAsset.fontInfo.SubSize
					num19 = Me.m_maxLineAscender
					Me.m_maxLineAscender = If((num24 <= Me.m_maxLineAscender), Me.m_maxLineAscender, num24)
					Dim num25 As Single = (num20 - Me.m_baselineOffset) / Me.m_currentFontAsset.fontInfo.SubSize
					num20 = Me.m_maxLineDescender
					Me.m_maxLineDescender = If((num25 >= Me.m_maxLineDescender), Me.m_maxLineDescender, num25)
				End If
				If Me.m_lineNumber = 0 Then
					Me.m_maxAscender = If((Me.m_maxAscender <= num19), num19, Me.m_maxAscender)
				End If
				If Me.m_lineOffset = 0F Then
					num8 = If((num8 <= num19), num19, num8)
				End If
				Me.m_textInfo.characterInfo(Me.m_characterCount).isVisible = False
				If num13 = 9 OrElse Not Char.IsWhiteSpace(CChar(num13)) OrElse Me.m_textElementType = TMP_TextElementType.Sprite Then
					Me.m_textInfo.characterInfo(Me.m_characterCount).isVisible = True
					Dim num26 As Single = If((Me.m_width = -1F), (marginWidth + 0.0001F - Me.m_marginLeft - Me.m_marginRight), Mathf.Min(marginWidth + 0.0001F - Me.m_marginLeft - Me.m_marginRight, Me.m_width))
					Me.m_textInfo.lineInfo(Me.m_lineNumber).width = num26
					Me.m_textInfo.lineInfo(Me.m_lineNumber).marginLeft = Me.m_marginLeft
					If Mathf.Abs(Me.m_xAdvance) + If(Me.m_isRightToLeft, 0F, Me.m_cached_TextElement.xAdvance) * (1F - Me.m_charWidthAdjDelta) * num2 > num26 Then
						num7 = Me.m_characterCount - 1
						If MyBase.enableWordWrapping AndAlso Me.m_characterCount <> Me.m_firstCharacterOfLine Then
							If num10 = Me.m_SavedWordWrapState.previous_WordBreak OrElse flag5 Then
								If Me.m_enableAutoSizing AndAlso Me.m_fontSize > Me.m_fontSizeMin Then
									If Me.m_charWidthAdjDelta < Me.m_charWidthMaxAdj / 100F Then
										Me.loopCountA = 0
										Me.m_charWidthAdjDelta += 0.01F
										Me.GenerateTextMesh()
										Return
									End If
									Me.m_maxFontSize = Me.m_fontSize
									Me.m_fontSize -= Mathf.Max((Me.m_fontSize - Me.m_minFontSize) / 2F, 0.05F)
									Me.m_fontSize = CSng(CInt((Mathf.Max(Me.m_fontSize, Me.m_fontSizeMin) * 20F + 0.5F))) / 20F
									If Me.loopCountA > 20 Then
										Return
									End If
									Me.GenerateTextMesh()
									Return
								Else
									If Not Me.m_isCharacterWrappingEnabled Then
										Me.m_isCharacterWrappingEnabled = True
									Else
										flag6 = True
									End If
									Me.m_recursiveCount += 1
									If Me.m_recursiveCount > 20 Then
										GoTo IL_285B
									End If
								End If
							End If
							num12 = MyBase.RestoreWordWrappingState(Me.m_SavedWordWrapState)
							num10 = num12
							If Me.m_lineNumber > 0 AndAlso Not TMP_Math.Approximately(Me.m_maxLineAscender, Me.m_startOfLineAscender) AndAlso Me.m_lineHeight = 0F AndAlso Not Me.m_isNewPage Then
								Dim num27 As Single = Me.m_maxLineAscender - Me.m_startOfLineAscender
								Me.AdjustLineOffset(Me.m_firstCharacterOfLine, Me.m_characterCount, num27)
								Me.m_lineOffset += num27
								Me.m_SavedWordWrapState.lineOffset = Me.m_lineOffset
								Me.m_SavedWordWrapState.previousLineAscender = Me.m_maxLineAscender
							End If
							Me.m_isNewPage = False
							Dim num28 As Single = Me.m_maxLineAscender - Me.m_lineOffset
							Dim num29 As Single = Me.m_maxLineDescender - Me.m_lineOffset
							Me.m_maxDescender = If((Me.m_maxDescender >= num29), num29, Me.m_maxDescender)
							If Not flag4 Then
								num9 = Me.m_maxDescender
							End If
							If Me.m_characterCount >= Me.m_maxVisibleCharacters OrElse Me.m_lineNumber >= Me.m_maxVisibleLines Then
								flag4 = True
							End If
							Me.m_textInfo.lineInfo(Me.m_lineNumber).firstCharacterIndex = Me.m_firstCharacterOfLine
							Me.m_textInfo.lineInfo(Me.m_lineNumber).firstVisibleCharacterIndex = Me.m_firstVisibleCharacterOfLine
							Me.m_textInfo.lineInfo(Me.m_lineNumber).lastCharacterIndex = If((Me.m_characterCount - 1 <= 0), 0, (Me.m_characterCount - 1))
							Me.m_textInfo.lineInfo(Me.m_lineNumber).lastVisibleCharacterIndex = Me.m_lastVisibleCharacterOfLine
							Me.m_textInfo.lineInfo(Me.m_lineNumber).characterCount = Me.m_textInfo.lineInfo(Me.m_lineNumber).lastCharacterIndex - Me.m_textInfo.lineInfo(Me.m_lineNumber).firstCharacterIndex + 1
							Me.m_textInfo.lineInfo(Me.m_lineNumber).lineExtents.min = New Vector2(Me.m_textInfo.characterInfo(Me.m_firstVisibleCharacterOfLine).bottomLeft.x, num29)
							Me.m_textInfo.lineInfo(Me.m_lineNumber).lineExtents.max = New Vector2(Me.m_textInfo.characterInfo(Me.m_lastVisibleCharacterOfLine).topRight.x, num28)
							Me.m_textInfo.lineInfo(Me.m_lineNumber).length = Me.m_textInfo.lineInfo(Me.m_lineNumber).lineExtents.max.x
							Me.m_textInfo.lineInfo(Me.m_lineNumber).maxAdvance = Me.m_textInfo.characterInfo(Me.m_lastVisibleCharacterOfLine).xAdvance - (Me.m_characterSpacing + Me.m_currentFontAsset.normalSpacingOffset) * num2
							Me.m_textInfo.lineInfo(Me.m_lineNumber).baseline = 0F - Me.m_lineOffset
							Me.m_textInfo.lineInfo(Me.m_lineNumber).ascender = num28
							Me.m_textInfo.lineInfo(Me.m_lineNumber).descender = num29
							Me.m_firstCharacterOfLine = Me.m_characterCount
							MyBase.SaveWordWrappingState(Me.m_SavedLineState, num12, Me.m_characterCount - 1)
							Me.m_lineNumber += 1
							flag3 = True
							If Me.m_lineNumber >= Me.m_textInfo.lineInfo.Length Then
								MyBase.ResizeLineExtents(Me.m_lineNumber)
							End If
							If Me.m_lineHeight = 0F Then
								Dim num30 As Single = Me.m_textInfo.characterInfo(Me.m_characterCount).ascender - Me.m_textInfo.characterInfo(Me.m_characterCount).baseLine
								Dim num31 As Single = 0F - Me.m_maxLineDescender + num30 + (num5 + Me.m_lineSpacing + Me.m_lineSpacingDelta) * num
								Me.m_lineOffset += num31
								Me.m_startOfLineAscender = num30
							Else
								Me.m_lineOffset += Me.m_lineHeight + Me.m_lineSpacing * num
							End If
							Me.m_maxLineAscender = Single.NegativeInfinity
							Me.m_maxLineDescender = Single.PositiveInfinity
							Me.m_xAdvance = Me.tag_Indent
							GoTo IL_285B
						End If
						If Me.m_enableAutoSizing AndAlso Me.m_fontSize > Me.m_fontSizeMin Then
							If Me.m_charWidthAdjDelta < Me.m_charWidthMaxAdj / 100F Then
								Me.loopCountA = 0
								Me.m_charWidthAdjDelta += 0.01F
								Me.GenerateTextMesh()
								Return
							End If
							Me.m_maxFontSize = Me.m_fontSize
							Me.m_fontSize -= Mathf.Max((Me.m_fontSize - Me.m_minFontSize) / 2F, 0.05F)
							Me.m_fontSize = CSng(CInt((Mathf.Max(Me.m_fontSize, Me.m_fontSizeMin) * 20F + 0.5F))) / 20F
							Me.m_recursiveCount = 0
							If Me.loopCountA > 20 Then
								Return
							End If
							Me.GenerateTextMesh()
							Return
						Else
							Select Case Me.m_overflowMode
								Case TextOverflowModes.Overflow
									If Me.m_isMaskingEnabled Then
										Me.DisableMasking()
									End If
								Case TextOverflowModes.Ellipsis
									If Me.m_isMaskingEnabled Then
										Me.DisableMasking()
									End If
									Me.m_isTextTruncated = True
									If Me.m_characterCount >= 1 Then
										Me.m_char_buffer(num12 - 1) = 8230
										Me.m_char_buffer(num12) = 0
										If Me.m_cached_Ellipsis_GlyphInfo IsNot Nothing Then
											Me.m_textInfo.characterInfo(num7).character = "…"c
											Me.m_textInfo.characterInfo(num7).textElement = Me.m_cached_Ellipsis_GlyphInfo
											Me.m_textInfo.characterInfo(num7).fontAsset = Me.m_materialReferences(0).fontAsset
											Me.m_textInfo.characterInfo(num7).material = Me.m_materialReferences(0).material
											Me.m_textInfo.characterInfo(num7).materialReferenceIndex = 0
										End If
										Me.m_totalCharacterCount = num7 + 1
										Me.GenerateTextMesh()
										Return
									End If
									Me.m_textInfo.characterInfo(Me.m_characterCount).isVisible = False
									Me.m_visibleCharacterCount = 0
								Case TextOverflowModes.Masking
									If Not Me.m_isMaskingEnabled Then
										Me.EnableMasking()
									End If
								Case TextOverflowModes.Truncate
									If Me.m_isMaskingEnabled Then
										Me.DisableMasking()
									End If
									Me.m_textInfo.characterInfo(Me.m_characterCount).isVisible = False
								Case TextOverflowModes.ScrollRect
									If Not Me.m_isMaskingEnabled Then
										Me.EnableMasking()
									End If
							End Select
						End If
					End If
					If num13 <> 9 Then
						Dim color As Color32
						If flag7 Then
							color = Color.red
						ElseIf Me.m_overrideHtmlColors Then
							color = Me.m_fontColor32
						Else
							color = Me.m_htmlColor
						End If
						If Me.m_textElementType = TMP_TextElementType.Character Then
							Me.SaveGlyphVertexInfo(num3, num17, color)
						ElseIf Me.m_textElementType = TMP_TextElementType.Sprite Then
							Me.SaveSpriteVertexInfo(color)
						End If
					Else
						Me.m_textInfo.characterInfo(Me.m_characterCount).isVisible = False
						Me.m_lastVisibleCharacterOfLine = Me.m_characterCount
						Dim lineInfo As TMP_LineInfo() = Me.m_textInfo.lineInfo
						Dim lineNumber As Integer = Me.m_lineNumber
						lineInfo(lineNumber).spaceCount = lineInfo(lineNumber).spaceCount + 1
						Me.m_textInfo.spaceCount += 1
					End If
					If Me.m_textInfo.characterInfo(Me.m_characterCount).isVisible Then
						If flag3 Then
							flag3 = False
							Me.m_firstVisibleCharacterOfLine = Me.m_characterCount
						End If
						Me.m_visibleCharacterCount += 1
						Me.m_lastVisibleCharacterOfLine = Me.m_characterCount
					End If
				ElseIf num13 = 10 OrElse Char.IsSeparator(CChar(num13)) Then
					Dim lineInfo2 As TMP_LineInfo() = Me.m_textInfo.lineInfo
					Dim lineNumber2 As Integer = Me.m_lineNumber
					lineInfo2(lineNumber2).spaceCount = lineInfo2(lineNumber2).spaceCount + 1
					Me.m_textInfo.spaceCount += 1
				End If
				If Me.m_lineNumber > 0 AndAlso Not TMP_Math.Approximately(Me.m_maxLineAscender, Me.m_startOfLineAscender) AndAlso Me.m_lineHeight = 0F AndAlso Not Me.m_isNewPage Then
					Dim num32 As Single = Me.m_maxLineAscender - Me.m_startOfLineAscender
					Me.AdjustLineOffset(Me.m_firstCharacterOfLine, Me.m_characterCount, num32)
					num23 -= num32
					Me.m_lineOffset += num32
					Me.m_startOfLineAscender += num32
					Me.m_SavedWordWrapState.lineOffset = Me.m_lineOffset
					Me.m_SavedWordWrapState.previousLineAscender = Me.m_startOfLineAscender
				End If
				Me.m_textInfo.characterInfo(Me.m_characterCount).lineNumber = CShort(Me.m_lineNumber)
				Me.m_textInfo.characterInfo(Me.m_characterCount).pageNumber = CShort(Me.m_pageNumber)
				If(num13 <> 10 AndAlso num13 <> 13 AndAlso num13 <> 8230) OrElse Me.m_textInfo.lineInfo(Me.m_lineNumber).characterCount = 1 Then
					Me.m_textInfo.lineInfo(Me.m_lineNumber).alignment = Me.m_lineJustification
				End If
				If Me.m_maxAscender - num23 > marginHeight + 0.0001F Then
					If Me.m_enableAutoSizing AndAlso Me.m_lineSpacingDelta > Me.m_lineSpacingMax AndAlso Me.m_lineNumber > 0 Then
						Me.m_lineSpacingDelta -= 1F
						Me.GenerateTextMesh()
						Return
					End If
					If Me.m_enableAutoSizing AndAlso Me.m_fontSize > Me.m_fontSizeMin Then
						Me.m_maxFontSize = Me.m_fontSize
						Me.m_fontSize -= Mathf.Max((Me.m_fontSize - Me.m_minFontSize) / 2F, 0.05F)
						Me.m_fontSize = CSng(CInt((Mathf.Max(Me.m_fontSize, Me.m_fontSizeMin) * 20F + 0.5F))) / 20F
						Me.m_recursiveCount = 0
						If Me.loopCountA > 20 Then
							Return
						End If
						Me.GenerateTextMesh()
						Return
					Else
						Select Case Me.m_overflowMode
							Case TextOverflowModes.Overflow
								If Me.m_isMaskingEnabled Then
									Me.DisableMasking()
								End If
							Case TextOverflowModes.Ellipsis
								If Me.m_isMaskingEnabled Then
									Me.DisableMasking()
								End If
								If Me.m_lineNumber > 0 Then
									Me.m_char_buffer(CInt(Me.m_textInfo.characterInfo(num7).index)) = 8230
									Me.m_char_buffer(CInt((Me.m_textInfo.characterInfo(num7).index + 1S))) = 0
									If Me.m_cached_Ellipsis_GlyphInfo IsNot Nothing Then
										Me.m_textInfo.characterInfo(num7).character = "…"c
										Me.m_textInfo.characterInfo(num7).textElement = Me.m_cached_Ellipsis_GlyphInfo
										Me.m_textInfo.characterInfo(num7).fontAsset = Me.m_materialReferences(0).fontAsset
										Me.m_textInfo.characterInfo(num7).material = Me.m_materialReferences(0).material
										Me.m_textInfo.characterInfo(num7).materialReferenceIndex = 0
									End If
									Me.m_totalCharacterCount = num7 + 1
									Me.GenerateTextMesh()
									Me.m_isTextTruncated = True
									Return
								End If
								Me.ClearMesh()
								Return
							Case TextOverflowModes.Masking
								If Not Me.m_isMaskingEnabled Then
									Me.EnableMasking()
								End If
							Case TextOverflowModes.Truncate
								If Me.m_isMaskingEnabled Then
									Me.DisableMasking()
								End If
								If Me.m_lineNumber > 0 Then
									Me.m_char_buffer(CInt((Me.m_textInfo.characterInfo(num7).index + 1S))) = 0
									Me.m_totalCharacterCount = num7 + 1
									Me.GenerateTextMesh()
									Me.m_isTextTruncated = True
									Return
								End If
								Me.ClearMesh()
								Return
							Case TextOverflowModes.ScrollRect
								If Not Me.m_isMaskingEnabled Then
									Me.EnableMasking()
								End If
							Case TextOverflowModes.Page
								If Me.m_isMaskingEnabled Then
									Me.DisableMasking()
								End If
								If num13 <> 13 AndAlso num13 <> 10 Then
									num12 = MyBase.RestoreWordWrappingState(Me.m_SavedLineState)
									If num12 = 0 Then
										Me.ClearMesh()
										Return
									End If
									Me.m_isNewPage = True
									Me.m_xAdvance = Me.tag_Indent
									Me.m_lineOffset = 0F
									Me.m_lineNumber += 1
									Me.m_pageNumber += 1
									GoTo IL_285B
								End If
						End Select
					End If
				End If
				If num13 = 9 Then
					Me.m_xAdvance += Me.m_currentFontAsset.fontInfo.TabWidth * num2
				ElseIf Me.m_monoSpacing <> 0F Then
					Me.m_xAdvance += (Me.m_monoSpacing - num16 + (Me.m_characterSpacing + Me.m_currentFontAsset.normalSpacingOffset) * num2 + Me.m_cSpacing) * (1F - Me.m_charWidthAdjDelta)
				ElseIf Not Me.m_isRightToLeft Then
					Me.m_xAdvance += ((Me.m_cached_TextElement.xAdvance * num4 + Me.m_characterSpacing + Me.m_currentFontAsset.normalSpacingOffset) * num2 + Me.m_cSpacing) * (1F - Me.m_charWidthAdjDelta)
				End If
				Me.m_textInfo.characterInfo(Me.m_characterCount).xAdvance = Me.m_xAdvance
				If num13 = 13 Then
					Me.m_xAdvance = Me.tag_Indent
				End If
				If num13 = 10 OrElse Me.m_characterCount = totalCharacterCount - 1 Then
					If Me.m_lineNumber > 0 AndAlso Not TMP_Math.Approximately(Me.m_maxLineAscender, Me.m_startOfLineAscender) AndAlso Me.m_lineHeight = 0F AndAlso Not Me.m_isNewPage Then
						Dim num33 As Single = Me.m_maxLineAscender - Me.m_startOfLineAscender
						Me.AdjustLineOffset(Me.m_firstCharacterOfLine, Me.m_characterCount, num33)
						num23 -= num33
						Me.m_lineOffset += num33
					End If
					Me.m_isNewPage = False
					Dim num34 As Single = Me.m_maxLineAscender - Me.m_lineOffset
					Dim num35 As Single = Me.m_maxLineDescender - Me.m_lineOffset
					Me.m_maxDescender = If((Me.m_maxDescender >= num35), num35, Me.m_maxDescender)
					If Not flag4 Then
						num9 = Me.m_maxDescender
					End If
					If Me.m_characterCount >= Me.m_maxVisibleCharacters OrElse Me.m_lineNumber >= Me.m_maxVisibleLines Then
						flag4 = True
					End If
					Me.m_textInfo.lineInfo(Me.m_lineNumber).firstCharacterIndex = Me.m_firstCharacterOfLine
					Me.m_textInfo.lineInfo(Me.m_lineNumber).firstVisibleCharacterIndex = Me.m_firstVisibleCharacterOfLine
					Me.m_textInfo.lineInfo(Me.m_lineNumber).lastCharacterIndex = Me.m_characterCount
					Me.m_textInfo.lineInfo(Me.m_lineNumber).lastVisibleCharacterIndex = If((Me.m_lastVisibleCharacterOfLine < Me.m_firstVisibleCharacterOfLine), Me.m_firstVisibleCharacterOfLine, Me.m_lastVisibleCharacterOfLine)
					Me.m_textInfo.lineInfo(Me.m_lineNumber).characterCount = Me.m_textInfo.lineInfo(Me.m_lineNumber).lastCharacterIndex - Me.m_textInfo.lineInfo(Me.m_lineNumber).firstCharacterIndex + 1
					Me.m_textInfo.lineInfo(Me.m_lineNumber).lineExtents.min = New Vector2(Me.m_textInfo.characterInfo(Me.m_firstVisibleCharacterOfLine).bottomLeft.x, num35)
					Me.m_textInfo.lineInfo(Me.m_lineNumber).lineExtents.max = New Vector2(Me.m_textInfo.characterInfo(Me.m_lastVisibleCharacterOfLine).topRight.x, num34)
					Me.m_textInfo.lineInfo(Me.m_lineNumber).length = Me.m_textInfo.lineInfo(Me.m_lineNumber).lineExtents.max.x - num3 * num2
					Me.m_textInfo.lineInfo(Me.m_lineNumber).maxAdvance = Me.m_textInfo.characterInfo(Me.m_lastVisibleCharacterOfLine).xAdvance - (Me.m_characterSpacing + Me.m_currentFontAsset.normalSpacingOffset) * num2
					Me.m_textInfo.lineInfo(Me.m_lineNumber).baseline = 0F - Me.m_lineOffset
					Me.m_textInfo.lineInfo(Me.m_lineNumber).ascender = num34
					Me.m_textInfo.lineInfo(Me.m_lineNumber).descender = num35
					Me.m_firstCharacterOfLine = Me.m_characterCount + 1
					If num13 = 10 Then
						MyBase.SaveWordWrappingState(Me.m_SavedLineState, num12, Me.m_characterCount)
						MyBase.SaveWordWrappingState(Me.m_SavedWordWrapState, num12, Me.m_characterCount)
						Me.m_lineNumber += 1
						flag3 = True
						If Me.m_lineNumber >= Me.m_textInfo.lineInfo.Length Then
							MyBase.ResizeLineExtents(Me.m_lineNumber)
						End If
						If Me.m_lineHeight = 0F Then
							Dim num31 As Single = 0F - Me.m_maxLineDescender + num19 + (num5 + Me.m_lineSpacing + Me.m_paragraphSpacing + Me.m_lineSpacingDelta) * num
							Me.m_lineOffset += num31
						Else
							Me.m_lineOffset += Me.m_lineHeight + (Me.m_lineSpacing + Me.m_paragraphSpacing) * num
						End If
						Me.m_maxLineAscender = Single.NegativeInfinity
						Me.m_maxLineDescender = Single.PositiveInfinity
						Me.m_startOfLineAscender = num19
						Me.m_xAdvance = Me.tag_LineIndent + Me.tag_Indent
						num7 = Me.m_characterCount - 1
						Me.m_characterCount += 1
						GoTo IL_285B
					End If
				End If
				If Me.m_textInfo.characterInfo(Me.m_characterCount).isVisible Then
					Me.m_meshExtents.min.x = Mathf.Min(Me.m_meshExtents.min.x, Me.m_textInfo.characterInfo(Me.m_characterCount).bottomLeft.x)
					Me.m_meshExtents.min.y = Mathf.Min(Me.m_meshExtents.min.y, Me.m_textInfo.characterInfo(Me.m_characterCount).bottomLeft.y)
					Me.m_meshExtents.max.x = Mathf.Max(Me.m_meshExtents.max.x, Me.m_textInfo.characterInfo(Me.m_characterCount).topRight.x)
					Me.m_meshExtents.max.y = Mathf.Max(Me.m_meshExtents.max.y, Me.m_textInfo.characterInfo(Me.m_characterCount).topRight.y)
				End If
				If Me.m_overflowMode = TextOverflowModes.Page AndAlso num13 <> 13 AndAlso num13 <> 10 AndAlso Me.m_pageNumber < 16 Then
					Me.m_textInfo.pageInfo(Me.m_pageNumber).ascender = num8
					Me.m_textInfo.pageInfo(Me.m_pageNumber).descender = If((num20 >= Me.m_textInfo.pageInfo(Me.m_pageNumber).descender), Me.m_textInfo.pageInfo(Me.m_pageNumber).descender, num20)
					If Me.m_pageNumber = 0 AndAlso Me.m_characterCount = 0 Then
						Me.m_textInfo.pageInfo(Me.m_pageNumber).firstCharacterIndex = Me.m_characterCount
					ElseIf Me.m_characterCount > 0 AndAlso Me.m_pageNumber <> CInt(Me.m_textInfo.characterInfo(Me.m_characterCount - 1).pageNumber) Then
						Me.m_textInfo.pageInfo(Me.m_pageNumber - 1).lastCharacterIndex = Me.m_characterCount - 1
						Me.m_textInfo.pageInfo(Me.m_pageNumber).firstCharacterIndex = Me.m_characterCount
					ElseIf Me.m_characterCount = totalCharacterCount - 1 Then
						Me.m_textInfo.pageInfo(Me.m_pageNumber).lastCharacterIndex = Me.m_characterCount
					End If
				End If
				If Me.m_enableWordWrapping OrElse Me.m_overflowMode = TextOverflowModes.Truncate OrElse Me.m_overflowMode = TextOverflowModes.Ellipsis Then
					If Char.IsWhiteSpace(CChar(num13)) AndAlso Not Me.m_isNonBreakingSpace Then
						MyBase.SaveWordWrappingState(Me.m_SavedWordWrapState, num12, Me.m_characterCount)
						Me.m_isCharacterWrappingEnabled = False
						flag5 = False
					ElseIf num13 > 11904 AndAlso num13 < 40959 AndAlso Not Me.m_isNonBreakingSpace Then
						If Not Me.m_currentFontAsset.lineBreakingInfo.leadingCharacters.ContainsKey(num13) AndAlso Me.m_characterCount < totalCharacterCount - 1 AndAlso Not Me.m_currentFontAsset.lineBreakingInfo.followingCharacters.ContainsKey(CInt(Me.m_textInfo.characterInfo(Me.m_characterCount + 1).character)) Then
							MyBase.SaveWordWrappingState(Me.m_SavedWordWrapState, num12, Me.m_characterCount)
							Me.m_isCharacterWrappingEnabled = False
							flag5 = False
						End If
					ElseIf flag5 OrElse Me.m_isCharacterWrappingEnabled OrElse flag6 Then
						MyBase.SaveWordWrappingState(Me.m_SavedWordWrapState, num12, Me.m_characterCount)
					End If
				End If
				Me.m_characterCount += 1
				GoTo IL_285B
			End While
			Dim num36 As Single = Me.m_maxFontSize - Me.m_minFontSize
			If Not Me.m_isCharacterWrappingEnabled AndAlso Me.m_enableAutoSizing AndAlso num36 > 0.051F AndAlso Me.m_fontSize < Me.m_fontSizeMax Then
				Me.m_minFontSize = Me.m_fontSize
				Me.m_fontSize += Mathf.Max((Me.m_maxFontSize - Me.m_fontSize) / 2F, 0.05F)
				Me.m_fontSize = CSng(CInt((Mathf.Min(Me.m_fontSize, Me.m_fontSizeMax) * 20F + 0.5F))) / 20F
				If Me.loopCountA > 20 Then
					Return
				End If
				Me.GenerateTextMesh()
				Return
			Else
				Me.m_isCharacterWrappingEnabled = False
				If Me.m_visibleCharacterCount = 0 AndAlso Me.m_visibleSpriteCount = 0 Then
					Me.ClearMesh()
					Return
				End If
				Dim num37 As Integer = Me.m_materialReferences(0).referenceCount * 4
				Me.m_textInfo.meshInfo(0).Clear(False)
				Dim vector7 As Vector3 = Vector3.zero
				Dim rectTransformCorners As Vector3() = Me.m_RectTransformCorners
				Select Case Me.m_textAlignment
					Case TextAlignmentOptions.TopLeft, TextAlignmentOptions.Top, TextAlignmentOptions.TopRight, TextAlignmentOptions.TopJustified
						If Me.m_overflowMode <> TextOverflowModes.Page Then
							vector7 = rectTransformCorners(1) + New Vector3(margin.x, 0F - Me.m_maxAscender - margin.y, 0F)
						Else
							vector7 = rectTransformCorners(1) + New Vector3(margin.x, 0F - Me.m_textInfo.pageInfo(num6).ascender - margin.y, 0F)
						End If
					Case TextAlignmentOptions.Left, TextAlignmentOptions.Center, TextAlignmentOptions.Right, TextAlignmentOptions.Justified
						If Me.m_overflowMode <> TextOverflowModes.Page Then
							vector7 = (rectTransformCorners(0) + rectTransformCorners(1)) / 2F + New Vector3(margin.x, 0F - (Me.m_maxAscender + margin.y + num9 - margin.w) / 2F, 0F)
						Else
							vector7 = (rectTransformCorners(0) + rectTransformCorners(1)) / 2F + New Vector3(margin.x, 0F - (Me.m_textInfo.pageInfo(num6).ascender + margin.y + Me.m_textInfo.pageInfo(num6).descender - margin.w) / 2F, 0F)
						End If
					Case TextAlignmentOptions.BottomLeft, TextAlignmentOptions.Bottom, TextAlignmentOptions.BottomRight, TextAlignmentOptions.BottomJustified
						If Me.m_overflowMode <> TextOverflowModes.Page Then
							vector7 = rectTransformCorners(0) + New Vector3(margin.x, 0F - num9 + margin.w, 0F)
						Else
							vector7 = rectTransformCorners(0) + New Vector3(margin.x, 0F - Me.m_textInfo.pageInfo(num6).descender + margin.w, 0F)
						End If
					Case TextAlignmentOptions.BaselineLeft, TextAlignmentOptions.Baseline, TextAlignmentOptions.BaselineRight, TextAlignmentOptions.BaselineJustified
						vector7 = (rectTransformCorners(0) + rectTransformCorners(1)) / 2F + New Vector3(margin.x, 0F, 0F)
					Case TextAlignmentOptions.MidlineLeft, TextAlignmentOptions.Midline, TextAlignmentOptions.MidlineRight, TextAlignmentOptions.MidlineJustified
						vector7 = (rectTransformCorners(0) + rectTransformCorners(1)) / 2F + New Vector3(margin.x, 0F - (Me.m_meshExtents.max.y + margin.y + Me.m_meshExtents.min.y - margin.w) / 2F, 0F)
				End Select
				Dim vector8 As Vector3 = Vector3.zero
				Dim vector9 As Vector3 = Vector3.zero
				Dim num38 As Integer = 0
				Dim num39 As Integer = 0
				Dim num40 As Integer = 0
				Dim num41 As Integer = 0
				Dim num42 As Integer = 0
				Dim flag9 As Boolean = False
				Dim num43 As Integer = 0
				Dim flag10 As Boolean = Not(Me.m_canvas.worldCamera Is Nothing)
				Dim num44 As Single = If((Me.m_rectTransform.lossyScale.y = 0F), 1F, Me.m_rectTransform.lossyScale.y)
				Dim renderMode As RenderMode = Me.m_canvas.renderMode
				Dim scaleFactor As Single = Me.m_canvas.scaleFactor
				Dim color2 As Color32 = Color.white
				Dim color3 As Color32 = Color.white
				Dim num45 As Single = 0F
				Dim num46 As Single = 0F
				Dim num47 As Single = Single.PositiveInfinity
				Dim num48 As Integer = 0
				Dim num49 As Single = 0F
				Dim num50 As Single = 0F
				Dim num51 As Single = 0F
				Dim characterInfo2 As TMP_CharacterInfo() = Me.m_textInfo.characterInfo
				For i As Integer = 0 To Me.m_characterCount - 1
					Dim character2 As Char = characterInfo2(i).character
					Dim lineNumber3 As Integer = CInt(characterInfo2(i).lineNumber)
					Dim tmp_LineInfo As TMP_LineInfo = Me.m_textInfo.lineInfo(lineNumber3)
					num41 = lineNumber3 + 1
					Select Case tmp_LineInfo.alignment
						Case TextAlignmentOptions.TopLeft, TextAlignmentOptions.Left, TextAlignmentOptions.BottomLeft, TextAlignmentOptions.BaselineLeft, TextAlignmentOptions.MidlineLeft
							If Not Me.m_isRightToLeft Then
								vector8 = New Vector3(tmp_LineInfo.marginLeft, 0F, 0F)
							Else
								vector8 = New Vector3(0F - tmp_LineInfo.maxAdvance, 0F, 0F)
							End If
						Case TextAlignmentOptions.Top, TextAlignmentOptions.Center, TextAlignmentOptions.Bottom, TextAlignmentOptions.Baseline, TextAlignmentOptions.Midline
							vector8 = New Vector3(tmp_LineInfo.marginLeft + tmp_LineInfo.width / 2F - tmp_LineInfo.maxAdvance / 2F, 0F, 0F)
						Case TextAlignmentOptions.TopRight, TextAlignmentOptions.Right, TextAlignmentOptions.BottomRight, TextAlignmentOptions.BaselineRight, TextAlignmentOptions.MidlineRight
							If Not Me.m_isRightToLeft Then
								vector8 = New Vector3(tmp_LineInfo.marginLeft + tmp_LineInfo.width - tmp_LineInfo.maxAdvance, 0F, 0F)
							Else
								vector8 = New Vector3(tmp_LineInfo.marginLeft + tmp_LineInfo.width, 0F, 0F)
							End If
						Case TextAlignmentOptions.TopJustified, TextAlignmentOptions.Justified, TextAlignmentOptions.BottomJustified, TextAlignmentOptions.BaselineJustified, TextAlignmentOptions.MidlineJustified
							Dim character3 As Char = characterInfo2(tmp_LineInfo.lastCharacterIndex).character
							If Not Char.IsControl(character3) AndAlso lineNumber3 < Me.m_lineNumber Then
								Dim num52 As Single = tmp_LineInfo.width - tmp_LineInfo.maxAdvance
								Dim num53 As Single = If((tmp_LineInfo.spaceCount <= 2), 1F, Me.m_wordWrappingRatios)
								If lineNumber3 <> num42 OrElse i = 0 Then
									vector8 = New Vector3(tmp_LineInfo.marginLeft, 0F, 0F)
								ElseIf character2 = vbTab OrElse Char.IsSeparator(character2) Then
									Dim num54 As Integer = If((tmp_LineInfo.spaceCount - 1 <= 0), 1, (tmp_LineInfo.spaceCount - 1))
									vector8 += New Vector3(num52 * (1F - num53) / CSng(num54), 0F, 0F)
								Else
									vector8 += New Vector3(num52 * num53 / CSng((tmp_LineInfo.characterCount - tmp_LineInfo.spaceCount - 1)), 0F, 0F)
								End If
							Else
								vector8 = New Vector3(tmp_LineInfo.marginLeft, 0F, 0F)
							End If
					End Select
					vector9 = vector7 + vector8
					Dim isVisible As Boolean = characterInfo2(i).isVisible
					If isVisible Then
						Dim elementType As TMP_TextElementType = characterInfo2(i).elementType
						If elementType <> TMP_TextElementType.Character Then
							If elementType <> TMP_TextElementType.Sprite Then
							End If
						Else
							Dim lineExtents As Extents = tmp_LineInfo.lineExtents
							Dim num55 As Single = Me.m_uvLineOffset * CSng(lineNumber3) Mod 1F + Me.m_uvOffset.x
							Select Case Me.m_horizontalMapping
								Case TextureMappingOptions.Character
									characterInfo2(i).vertex_BL.uv2.x = Me.m_uvOffset.x
									characterInfo2(i).vertex_TL.uv2.x = Me.m_uvOffset.x
									characterInfo2(i).vertex_TR.uv2.x = 1F + Me.m_uvOffset.x
									characterInfo2(i).vertex_BR.uv2.x = 1F + Me.m_uvOffset.x
								Case TextureMappingOptions.Line
									If Me.m_textAlignment <> TextAlignmentOptions.Justified Then
										characterInfo2(i).vertex_BL.uv2.x = (characterInfo2(i).vertex_BL.position.x - lineExtents.min.x) / (lineExtents.max.x - lineExtents.min.x) + num55
										characterInfo2(i).vertex_TL.uv2.x = (characterInfo2(i).vertex_TL.position.x - lineExtents.min.x) / (lineExtents.max.x - lineExtents.min.x) + num55
										characterInfo2(i).vertex_TR.uv2.x = (characterInfo2(i).vertex_TR.position.x - lineExtents.min.x) / (lineExtents.max.x - lineExtents.min.x) + num55
										characterInfo2(i).vertex_BR.uv2.x = (characterInfo2(i).vertex_BR.position.x - lineExtents.min.x) / (lineExtents.max.x - lineExtents.min.x) + num55
									Else
										characterInfo2(i).vertex_BL.uv2.x = (characterInfo2(i).vertex_BL.position.x + vector8.x - Me.m_meshExtents.min.x) / (Me.m_meshExtents.max.x - Me.m_meshExtents.min.x) + num55
										characterInfo2(i).vertex_TL.uv2.x = (characterInfo2(i).vertex_TL.position.x + vector8.x - Me.m_meshExtents.min.x) / (Me.m_meshExtents.max.x - Me.m_meshExtents.min.x) + num55
										characterInfo2(i).vertex_TR.uv2.x = (characterInfo2(i).vertex_TR.position.x + vector8.x - Me.m_meshExtents.min.x) / (Me.m_meshExtents.max.x - Me.m_meshExtents.min.x) + num55
										characterInfo2(i).vertex_BR.uv2.x = (characterInfo2(i).vertex_BR.position.x + vector8.x - Me.m_meshExtents.min.x) / (Me.m_meshExtents.max.x - Me.m_meshExtents.min.x) + num55
									End If
								Case TextureMappingOptions.Paragraph
									characterInfo2(i).vertex_BL.uv2.x = (characterInfo2(i).vertex_BL.position.x + vector8.x - Me.m_meshExtents.min.x) / (Me.m_meshExtents.max.x - Me.m_meshExtents.min.x) + num55
									characterInfo2(i).vertex_TL.uv2.x = (characterInfo2(i).vertex_TL.position.x + vector8.x - Me.m_meshExtents.min.x) / (Me.m_meshExtents.max.x - Me.m_meshExtents.min.x) + num55
									characterInfo2(i).vertex_TR.uv2.x = (characterInfo2(i).vertex_TR.position.x + vector8.x - Me.m_meshExtents.min.x) / (Me.m_meshExtents.max.x - Me.m_meshExtents.min.x) + num55
									characterInfo2(i).vertex_BR.uv2.x = (characterInfo2(i).vertex_BR.position.x + vector8.x - Me.m_meshExtents.min.x) / (Me.m_meshExtents.max.x - Me.m_meshExtents.min.x) + num55
								Case TextureMappingOptions.MatchAspect
									Select Case Me.m_verticalMapping
										Case TextureMappingOptions.Character
											characterInfo2(i).vertex_BL.uv2.y = Me.m_uvOffset.y
											characterInfo2(i).vertex_TL.uv2.y = 1F + Me.m_uvOffset.y
											characterInfo2(i).vertex_TR.uv2.y = Me.m_uvOffset.y
											characterInfo2(i).vertex_BR.uv2.y = 1F + Me.m_uvOffset.y
										Case TextureMappingOptions.Line
											characterInfo2(i).vertex_BL.uv2.y = (characterInfo2(i).vertex_BL.position.y - lineExtents.min.y) / (lineExtents.max.y - lineExtents.min.y) + num55
											characterInfo2(i).vertex_TL.uv2.y = (characterInfo2(i).vertex_TL.position.y - lineExtents.min.y) / (lineExtents.max.y - lineExtents.min.y) + num55
											characterInfo2(i).vertex_TR.uv2.y = characterInfo2(i).vertex_BL.uv2.y
											characterInfo2(i).vertex_BR.uv2.y = characterInfo2(i).vertex_TL.uv2.y
										Case TextureMappingOptions.Paragraph
											characterInfo2(i).vertex_BL.uv2.y = (characterInfo2(i).vertex_BL.position.y - Me.m_meshExtents.min.y) / (Me.m_meshExtents.max.y - Me.m_meshExtents.min.y) + num55
											characterInfo2(i).vertex_TL.uv2.y = (characterInfo2(i).vertex_TL.position.y - Me.m_meshExtents.min.y) / (Me.m_meshExtents.max.y - Me.m_meshExtents.min.y) + num55
											characterInfo2(i).vertex_TR.uv2.y = characterInfo2(i).vertex_BL.uv2.y
											characterInfo2(i).vertex_BR.uv2.y = characterInfo2(i).vertex_TL.uv2.y
									End Select
									Dim num56 As Single = (1F - (characterInfo2(i).vertex_BL.uv2.y + characterInfo2(i).vertex_TL.uv2.y) * characterInfo2(i).aspectRatio) / 2F
									characterInfo2(i).vertex_BL.uv2.x = characterInfo2(i).vertex_BL.uv2.y * characterInfo2(i).aspectRatio + num56 + num55
									characterInfo2(i).vertex_TL.uv2.x = characterInfo2(i).vertex_BL.uv2.x
									characterInfo2(i).vertex_TR.uv2.x = characterInfo2(i).vertex_TL.uv2.y * characterInfo2(i).aspectRatio + num56 + num55
									characterInfo2(i).vertex_BR.uv2.x = characterInfo2(i).vertex_TR.uv2.x
							End Select
							Select Case Me.m_verticalMapping
								Case TextureMappingOptions.Character
									characterInfo2(i).vertex_BL.uv2.y = Me.m_uvOffset.y
									characterInfo2(i).vertex_TL.uv2.y = 1F + Me.m_uvOffset.y
									characterInfo2(i).vertex_TR.uv2.y = 1F + Me.m_uvOffset.y
									characterInfo2(i).vertex_BR.uv2.y = Me.m_uvOffset.y
								Case TextureMappingOptions.Line
									characterInfo2(i).vertex_BL.uv2.y = (characterInfo2(i).vertex_BL.position.y - tmp_LineInfo.descender) / (tmp_LineInfo.ascender - tmp_LineInfo.descender) + Me.m_uvOffset.y
									characterInfo2(i).vertex_TL.uv2.y = (characterInfo2(i).vertex_TL.position.y - tmp_LineInfo.descender) / (tmp_LineInfo.ascender - tmp_LineInfo.descender) + Me.m_uvOffset.y
									characterInfo2(i).vertex_BR.uv2.y = characterInfo2(i).vertex_BL.uv2.y
									characterInfo2(i).vertex_TR.uv2.y = characterInfo2(i).vertex_TL.uv2.y
								Case TextureMappingOptions.Paragraph
									characterInfo2(i).vertex_BL.uv2.y = (characterInfo2(i).vertex_BL.position.y - Me.m_meshExtents.min.y) / (Me.m_meshExtents.max.y - Me.m_meshExtents.min.y) + Me.m_uvOffset.y
									characterInfo2(i).vertex_TL.uv2.y = (characterInfo2(i).vertex_TL.position.y - Me.m_meshExtents.min.y) / (Me.m_meshExtents.max.y - Me.m_meshExtents.min.y) + Me.m_uvOffset.y
									characterInfo2(i).vertex_TR.uv2.y = characterInfo2(i).vertex_TL.uv2.y
									characterInfo2(i).vertex_BR.uv2.y = characterInfo2(i).vertex_BL.uv2.y
								Case TextureMappingOptions.MatchAspect
									Dim num57 As Single = (1F - (characterInfo2(i).vertex_BL.uv2.x + characterInfo2(i).vertex_TR.uv2.x) / characterInfo2(i).aspectRatio) / 2F
									characterInfo2(i).vertex_BL.uv2.y = num57 + characterInfo2(i).vertex_BL.uv2.x / characterInfo2(i).aspectRatio + Me.m_uvOffset.y
									characterInfo2(i).vertex_TL.uv2.y = num57 + characterInfo2(i).vertex_TR.uv2.x / characterInfo2(i).aspectRatio + Me.m_uvOffset.y
									characterInfo2(i).vertex_BR.uv2.y = characterInfo2(i).vertex_BL.uv2.y
									characterInfo2(i).vertex_TR.uv2.y = characterInfo2(i).vertex_TL.uv2.y
							End Select
							Dim num58 As Single = characterInfo2(i).scale * (1F - Me.m_charWidthAdjDelta)
							If(characterInfo2(i).style And FontStyles.Bold) = FontStyles.Bold Then
								num58 *= -1F
							End If
							If renderMode <> RenderMode.ScreenSpaceOverlay Then
								If renderMode <> RenderMode.ScreenSpaceCamera Then
									If renderMode = RenderMode.WorldSpace Then
										num58 *= num44
									End If
								Else
									num58 *= If((Not flag10), 1F, num44)
								End If
							Else
								num58 *= num44 / scaleFactor
							End If
							Dim num59 As Single = characterInfo2(i).vertex_BL.uv2.x
							Dim num60 As Single = characterInfo2(i).vertex_BL.uv2.y
							Dim num61 As Single = characterInfo2(i).vertex_TR.uv2.x
							Dim num62 As Single = characterInfo2(i).vertex_TR.uv2.y
							Dim num63 As Single = Mathf.Floor(num59)
							Dim num64 As Single = Mathf.Floor(num60)
							num59 -= num63
							num61 -= num63
							num60 -= num64
							num62 -= num64
							characterInfo2(i).vertex_BL.uv2.x = MyBase.PackUV(num59, num60)
							characterInfo2(i).vertex_BL.uv2.y = num58
							characterInfo2(i).vertex_TL.uv2.x = MyBase.PackUV(num59, num62)
							characterInfo2(i).vertex_TL.uv2.y = num58
							characterInfo2(i).vertex_TR.uv2.x = MyBase.PackUV(num61, num62)
							characterInfo2(i).vertex_TR.uv2.y = num58
							characterInfo2(i).vertex_BR.uv2.x = MyBase.PackUV(num61, num60)
							characterInfo2(i).vertex_BR.uv2.y = num58
						End If
						If i < Me.m_maxVisibleCharacters AndAlso lineNumber3 < Me.m_maxVisibleLines AndAlso Me.m_overflowMode <> TextOverflowModes.Page Then
							Dim array As TMP_CharacterInfo() = characterInfo2
							Dim num65 As Integer = i
							array(num65).vertex_BL.position = array(num65).vertex_BL.position + vector9
							Dim array2 As TMP_CharacterInfo() = characterInfo2
							Dim num66 As Integer = i
							array2(num66).vertex_TL.position = array2(num66).vertex_TL.position + vector9
							Dim array3 As TMP_CharacterInfo() = characterInfo2
							Dim num67 As Integer = i
							array3(num67).vertex_TR.position = array3(num67).vertex_TR.position + vector9
							Dim array4 As TMP_CharacterInfo() = characterInfo2
							Dim num68 As Integer = i
							array4(num68).vertex_BR.position = array4(num68).vertex_BR.position + vector9
						ElseIf i < Me.m_maxVisibleCharacters AndAlso lineNumber3 < Me.m_maxVisibleLines AndAlso Me.m_overflowMode = TextOverflowModes.Page AndAlso CInt(characterInfo2(i).pageNumber) = num6 Then
							Dim array5 As TMP_CharacterInfo() = characterInfo2
							Dim num69 As Integer = i
							array5(num69).vertex_BL.position = array5(num69).vertex_BL.position + vector9
							Dim array6 As TMP_CharacterInfo() = characterInfo2
							Dim num70 As Integer = i
							array6(num70).vertex_TL.position = array6(num70).vertex_TL.position + vector9
							Dim array7 As TMP_CharacterInfo() = characterInfo2
							Dim num71 As Integer = i
							array7(num71).vertex_TR.position = array7(num71).vertex_TR.position + vector9
							Dim array8 As TMP_CharacterInfo() = characterInfo2
							Dim num72 As Integer = i
							array8(num72).vertex_BR.position = array8(num72).vertex_BR.position + vector9
						Else
							characterInfo2(i).vertex_BL.position = Vector3.zero
							characterInfo2(i).vertex_TL.position = Vector3.zero
							characterInfo2(i).vertex_TR.position = Vector3.zero
							characterInfo2(i).vertex_BR.position = Vector3.zero
						End If
						If elementType = TMP_TextElementType.Character Then
							Me.FillCharacterVertexBuffers(i, num38)
						ElseIf elementType = TMP_TextElementType.Sprite Then
							Me.FillSpriteVertexBuffers(i, num39)
						End If
					End If
					Dim characterInfo3 As TMP_CharacterInfo() = Me.m_textInfo.characterInfo
					Dim num73 As Integer = i
					characterInfo3(num73).bottomLeft = characterInfo3(num73).bottomLeft + vector9
					Dim characterInfo4 As TMP_CharacterInfo() = Me.m_textInfo.characterInfo
					Dim num74 As Integer = i
					characterInfo4(num74).topLeft = characterInfo4(num74).topLeft + vector9
					Dim characterInfo5 As TMP_CharacterInfo() = Me.m_textInfo.characterInfo
					Dim num75 As Integer = i
					characterInfo5(num75).topRight = characterInfo5(num75).topRight + vector9
					Dim characterInfo6 As TMP_CharacterInfo() = Me.m_textInfo.characterInfo
					Dim num76 As Integer = i
					characterInfo6(num76).bottomRight = characterInfo6(num76).bottomRight + vector9
					Dim characterInfo7 As TMP_CharacterInfo() = Me.m_textInfo.characterInfo
					Dim num77 As Integer = i
					characterInfo7(num77).origin = characterInfo7(num77).origin + vector9.x
					Dim characterInfo8 As TMP_CharacterInfo() = Me.m_textInfo.characterInfo
					Dim num78 As Integer = i
					characterInfo8(num78).xAdvance = characterInfo8(num78).xAdvance + vector9.x
					Dim characterInfo9 As TMP_CharacterInfo() = Me.m_textInfo.characterInfo
					Dim num79 As Integer = i
					characterInfo9(num79).ascender = characterInfo9(num79).ascender + vector9.y
					Dim characterInfo10 As TMP_CharacterInfo() = Me.m_textInfo.characterInfo
					Dim num80 As Integer = i
					characterInfo10(num80).descender = characterInfo10(num80).descender + vector9.y
					Dim characterInfo11 As TMP_CharacterInfo() = Me.m_textInfo.characterInfo
					Dim num81 As Integer = i
					characterInfo11(num81).baseLine = characterInfo11(num81).baseLine + vector9.y
					If isVisible Then
					End If
					If lineNumber3 <> num42 OrElse i = Me.m_characterCount - 1 Then
						If lineNumber3 <> num42 Then
							Me.m_textInfo.lineInfo(num42).lineExtents.min = New Vector2(Me.m_textInfo.characterInfo(Me.m_textInfo.lineInfo(num42).firstCharacterIndex).bottomLeft.x, Me.m_textInfo.lineInfo(num42).descender)
							Me.m_textInfo.lineInfo(num42).lineExtents.max = New Vector2(Me.m_textInfo.characterInfo(Me.m_textInfo.lineInfo(num42).lastVisibleCharacterIndex).topRight.x, Me.m_textInfo.lineInfo(num42).ascender)
							Dim lineInfo3 As TMP_LineInfo() = Me.m_textInfo.lineInfo
							Dim num82 As Integer = num42
							lineInfo3(num82).baseline = lineInfo3(num82).baseline + vector9.y
							Dim lineInfo4 As TMP_LineInfo() = Me.m_textInfo.lineInfo
							Dim num83 As Integer = num42
							lineInfo4(num83).ascender = lineInfo4(num83).ascender + vector9.y
							Dim lineInfo5 As TMP_LineInfo() = Me.m_textInfo.lineInfo
							Dim num84 As Integer = num42
							lineInfo5(num84).descender = lineInfo5(num84).descender + vector9.y
						End If
						If i = Me.m_characterCount - 1 Then
							Me.m_textInfo.lineInfo(lineNumber3).lineExtents.min = New Vector2(Me.m_textInfo.characterInfo(Me.m_textInfo.lineInfo(lineNumber3).firstCharacterIndex).bottomLeft.x, Me.m_textInfo.lineInfo(lineNumber3).descender)
							Me.m_textInfo.lineInfo(lineNumber3).lineExtents.max = New Vector2(Me.m_textInfo.characterInfo(Me.m_textInfo.lineInfo(lineNumber3).lastVisibleCharacterIndex).topRight.x, Me.m_textInfo.lineInfo(lineNumber3).ascender)
							Dim lineInfo6 As TMP_LineInfo() = Me.m_textInfo.lineInfo
							Dim num85 As Integer = lineNumber3
							lineInfo6(num85).baseline = lineInfo6(num85).baseline + vector9.y
							Dim lineInfo7 As TMP_LineInfo() = Me.m_textInfo.lineInfo
							Dim num86 As Integer = lineNumber3
							lineInfo7(num86).ascender = lineInfo7(num86).ascender + vector9.y
							Dim lineInfo8 As TMP_LineInfo() = Me.m_textInfo.lineInfo
							Dim num87 As Integer = lineNumber3
							lineInfo8(num87).descender = lineInfo8(num87).descender + vector9.y
						End If
					End If
					If Char.IsLetterOrDigit(character2) OrElse character2 = "-"c OrElse character2 = "’"c Then
						If Not flag9 Then
							flag9 = True
							num43 = i
						End If
						If flag9 AndAlso i = Me.m_characterCount - 1 Then
							Dim num88 As Integer = Me.m_textInfo.wordInfo.Length
							Dim wordCount As Integer = Me.m_textInfo.wordCount
							If Me.m_textInfo.wordCount + 1 > num88 Then
								TMP_TextInfo.Resize(Of TMP_WordInfo)(Me.m_textInfo.wordInfo, num88 + 1)
							End If
							Dim num89 As Integer = i
							Me.m_textInfo.wordInfo(wordCount).firstCharacterIndex = num43
							Me.m_textInfo.wordInfo(wordCount).lastCharacterIndex = num89
							Me.m_textInfo.wordInfo(wordCount).characterCount = num89 - num43 + 1
							Me.m_textInfo.wordInfo(wordCount).textComponent = Me
							num40 += 1
							Me.m_textInfo.wordCount += 1
							Dim lineInfo9 As TMP_LineInfo() = Me.m_textInfo.lineInfo
							Dim num90 As Integer = lineNumber3
							lineInfo9(num90).wordCount = lineInfo9(num90).wordCount + 1
						End If
					ElseIf flag9 OrElse (i = 0 AndAlso (Not Char.IsPunctuation(character2) OrElse Char.IsWhiteSpace(character2) OrElse i = Me.m_characterCount - 1)) Then
						If i <= 0 OrElse i >= Me.m_characterCount OrElse character2 <> "'"c OrElse Not Char.IsLetterOrDigit(characterInfo2(i - 1).character) OrElse Not Char.IsLetterOrDigit(characterInfo2(i + 1).character) Then
							Dim num89 As Integer = If((i <> Me.m_characterCount - 1 OrElse Not Char.IsLetterOrDigit(character2)), (i - 1), i)
							flag9 = False
							Dim num91 As Integer = Me.m_textInfo.wordInfo.Length
							Dim wordCount2 As Integer = Me.m_textInfo.wordCount
							If Me.m_textInfo.wordCount + 1 > num91 Then
								TMP_TextInfo.Resize(Of TMP_WordInfo)(Me.m_textInfo.wordInfo, num91 + 1)
							End If
							Me.m_textInfo.wordInfo(wordCount2).firstCharacterIndex = num43
							Me.m_textInfo.wordInfo(wordCount2).lastCharacterIndex = num89
							Me.m_textInfo.wordInfo(wordCount2).characterCount = num89 - num43 + 1
							Me.m_textInfo.wordInfo(wordCount2).textComponent = Me
							num40 += 1
							Me.m_textInfo.wordCount += 1
							Dim lineInfo10 As TMP_LineInfo() = Me.m_textInfo.lineInfo
							Dim num92 As Integer = lineNumber3
							lineInfo10(num92).wordCount = lineInfo10(num92).wordCount + 1
						End If
					End If
					Dim flag11 As Boolean = (Me.m_textInfo.characterInfo(i).style And FontStyles.Underline) = FontStyles.Underline
					If flag11 Then
						Dim flag12 As Boolean = True
						Dim pageNumber As Integer = CInt(Me.m_textInfo.characterInfo(i).pageNumber)
						If i > Me.m_maxVisibleCharacters OrElse lineNumber3 > Me.m_maxVisibleLines OrElse (Me.m_overflowMode = TextOverflowModes.Page AndAlso pageNumber + 1 <> Me.m_pageToDisplay) Then
							flag12 = False
						End If
						If Not Char.IsWhiteSpace(character2) Then
							num46 = Mathf.Max(num46, Me.m_textInfo.characterInfo(i).scale)
							num47 = Mathf.Min(If((pageNumber <> num48), Single.PositiveInfinity, num47), Me.m_textInfo.characterInfo(i).baseLine + MyBase.font.fontInfo.Underline * num46)
							num48 = pageNumber
						End If
						If Not flag AndAlso flag12 AndAlso i <= tmp_LineInfo.lastVisibleCharacterIndex AndAlso character2 <> vbLf AndAlso character2 <> vbCr Then
							If i <> tmp_LineInfo.lastVisibleCharacterIndex OrElse Not Char.IsSeparator(character2) Then
								flag = True
								num45 = Me.m_textInfo.characterInfo(i).scale
								If num46 = 0F Then
									num46 = num45
								End If
								zero = New Vector3(Me.m_textInfo.characterInfo(i).bottomLeft.x, num47, 0F)
								color2 = Me.m_textInfo.characterInfo(i).color
							End If
						End If
						If flag AndAlso Me.m_characterCount = 1 Then
							flag = False
							zero2 = New Vector3(Me.m_textInfo.characterInfo(i).topRight.x, num47, 0F)
							Dim num93 As Single = Me.m_textInfo.characterInfo(i).scale
							Me.DrawUnderlineMesh(zero, zero2, num37, num45, num93, num46, color2)
							num46 = 0F
							num47 = Single.PositiveInfinity
						ElseIf flag AndAlso (i = tmp_LineInfo.lastCharacterIndex OrElse i >= tmp_LineInfo.lastVisibleCharacterIndex) Then
							Dim num93 As Single
							If Char.IsWhiteSpace(character2) Then
								Dim lastVisibleCharacterIndex As Integer = tmp_LineInfo.lastVisibleCharacterIndex
								zero2 = New Vector3(Me.m_textInfo.characterInfo(lastVisibleCharacterIndex).topRight.x, num47, 0F)
								num93 = Me.m_textInfo.characterInfo(lastVisibleCharacterIndex).scale
							Else
								zero2 = New Vector3(Me.m_textInfo.characterInfo(i).topRight.x, num47, 0F)
								num93 = Me.m_textInfo.characterInfo(i).scale
							End If
							flag = False
							Me.DrawUnderlineMesh(zero, zero2, num37, num45, num93, num46, color2)
							num46 = 0F
							num47 = Single.PositiveInfinity
						ElseIf flag AndAlso Not flag12 Then
							flag = False
							zero2 = New Vector3(Me.m_textInfo.characterInfo(i - 1).topRight.x, num47, 0F)
							Dim num93 As Single = Me.m_textInfo.characterInfo(i - 1).scale
							Me.DrawUnderlineMesh(zero, zero2, num37, num45, num93, num46, color2)
							num46 = 0F
							num47 = Single.PositiveInfinity
						End If
					ElseIf flag Then
						flag = False
						zero2 = New Vector3(Me.m_textInfo.characterInfo(i - 1).topRight.x, num47, 0F)
						Dim num93 As Single = Me.m_textInfo.characterInfo(i - 1).scale
						Me.DrawUnderlineMesh(zero, zero2, num37, num45, num93, num46, color2)
						num46 = 0F
						num47 = Single.PositiveInfinity
					End If
					Dim flag13 As Boolean = (Me.m_textInfo.characterInfo(i).style And FontStyles.Strikethrough) = FontStyles.Strikethrough
					If flag13 Then
						Dim flag14 As Boolean = True
						If i > Me.m_maxVisibleCharacters OrElse lineNumber3 > Me.m_maxVisibleLines OrElse (Me.m_overflowMode = TextOverflowModes.Page AndAlso CInt((Me.m_textInfo.characterInfo(i).pageNumber + 1S)) <> Me.m_pageToDisplay) Then
							flag14 = False
						End If
						If Not flag2 AndAlso flag14 AndAlso i <= tmp_LineInfo.lastVisibleCharacterIndex AndAlso character2 <> vbLf AndAlso character2 <> vbCr Then
							If i <> tmp_LineInfo.lastVisibleCharacterIndex OrElse Not Char.IsSeparator(character2) Then
								flag2 = True
								num49 = Me.m_textInfo.characterInfo(i).pointSize
								num50 = Me.m_textInfo.characterInfo(i).scale
								zero3 = New Vector3(Me.m_textInfo.characterInfo(i).bottomLeft.x, Me.m_textInfo.characterInfo(i).baseLine + (MyBase.font.fontInfo.Ascender + MyBase.font.fontInfo.Descender) / 2.75F * num50, 0F)
								color3 = Me.m_textInfo.characterInfo(i).color
								num51 = Me.m_textInfo.characterInfo(i).baseLine
							End If
						End If
						If flag2 AndAlso Me.m_characterCount = 1 Then
							flag2 = False
							zero4 = New Vector3(Me.m_textInfo.characterInfo(i).topRight.x, Me.m_textInfo.characterInfo(i).baseLine + (MyBase.font.fontInfo.Ascender + MyBase.font.fontInfo.Descender) / 2F * num50, 0F)
							Me.DrawUnderlineMesh(zero3, zero4, num37, num50, num50, num50, color3)
						ElseIf flag2 AndAlso i = tmp_LineInfo.lastCharacterIndex Then
							If Char.IsWhiteSpace(character2) Then
								Dim lastVisibleCharacterIndex2 As Integer = tmp_LineInfo.lastVisibleCharacterIndex
								zero4 = New Vector3(Me.m_textInfo.characterInfo(lastVisibleCharacterIndex2).topRight.x, Me.m_textInfo.characterInfo(lastVisibleCharacterIndex2).baseLine + (MyBase.font.fontInfo.Ascender + MyBase.font.fontInfo.Descender) / 2F * num50, 0F)
							Else
								zero4 = New Vector3(Me.m_textInfo.characterInfo(i).topRight.x, Me.m_textInfo.characterInfo(i).baseLine + (MyBase.font.fontInfo.Ascender + MyBase.font.fontInfo.Descender) / 2F * num50, 0F)
							End If
							flag2 = False
							Me.DrawUnderlineMesh(zero3, zero4, num37, num50, num50, num50, color3)
						ElseIf flag2 AndAlso i < Me.m_characterCount AndAlso (Me.m_textInfo.characterInfo(i + 1).pointSize <> num49 OrElse Not TMP_Math.Approximately(Me.m_textInfo.characterInfo(i + 1).baseLine + vector9.y, num51)) Then
							flag2 = False
							Dim lastVisibleCharacterIndex3 As Integer = tmp_LineInfo.lastVisibleCharacterIndex
							If i > lastVisibleCharacterIndex3 Then
								zero4 = New Vector3(Me.m_textInfo.characterInfo(lastVisibleCharacterIndex3).topRight.x, Me.m_textInfo.characterInfo(lastVisibleCharacterIndex3).baseLine + (MyBase.font.fontInfo.Ascender + MyBase.font.fontInfo.Descender) / 2F * num50, 0F)
							Else
								zero4 = New Vector3(Me.m_textInfo.characterInfo(i).topRight.x, Me.m_textInfo.characterInfo(i).baseLine + (MyBase.font.fontInfo.Ascender + MyBase.font.fontInfo.Descender) / 2F * num50, 0F)
							End If
							Me.DrawUnderlineMesh(zero3, zero4, num37, num50, num50, num50, color3)
						ElseIf flag2 AndAlso Not flag14 Then
							flag2 = False
							zero4 = New Vector3(Me.m_textInfo.characterInfo(i - 1).topRight.x, Me.m_textInfo.characterInfo(i - 1).baseLine + (MyBase.font.fontInfo.Ascender + MyBase.font.fontInfo.Descender) / 2F * num50, 0F)
							Me.DrawUnderlineMesh(zero3, zero4, num37, num50, num50, num50, color3)
						End If
					ElseIf flag2 Then
						flag2 = False
						zero4 = New Vector3(Me.m_textInfo.characterInfo(i - 1).topRight.x, Me.m_textInfo.characterInfo(i - 1).baseLine + (MyBase.font.fontInfo.Ascender + MyBase.font.fontInfo.Descender) / 2F * Me.m_fontScale, 0F)
						Me.DrawUnderlineMesh(zero3, zero4, num37, num50, num50, num50, color3)
					End If
					num42 = lineNumber3
				Next
				Me.m_textInfo.characterCount = CInt(CShort(Me.m_characterCount))
				Me.m_textInfo.spriteCount = Me.m_spriteCount
				Me.m_textInfo.lineCount = CInt(CShort(num41))
				Me.m_textInfo.wordCount = CInt(If((num40 = 0 OrElse Me.m_characterCount <= 0), 1S, CShort(num40)))
				Me.m_textInfo.pageCount = Me.m_pageNumber + 1
				If Me.m_renderMode = TextRenderFlags.Render Then
					Me.m_mesh.MarkDynamic()
					Me.m_mesh.vertices = Me.m_textInfo.meshInfo(0).vertices
					Me.m_mesh.uv = Me.m_textInfo.meshInfo(0).uvs0
					Me.m_mesh.uv2 = Me.m_textInfo.meshInfo(0).uvs2
					Me.m_mesh.colors32 = Me.m_textInfo.meshInfo(0).colors32
					Me.m_mesh.RecalculateBounds()
					Me.m_canvasRenderer.SetMesh(Me.m_mesh)
					For j As Integer = 1 To Me.m_textInfo.materialCount - 1
						Me.m_textInfo.meshInfo(j).ClearUnusedVertices()
						Me.m_subTextObjects(j).mesh.MarkDynamic()
						Me.m_subTextObjects(j).mesh.vertices = Me.m_textInfo.meshInfo(j).vertices
						Me.m_subTextObjects(j).mesh.uv = Me.m_textInfo.meshInfo(j).uvs0
						Me.m_subTextObjects(j).mesh.uv2 = Me.m_textInfo.meshInfo(j).uvs2
						Me.m_subTextObjects(j).mesh.colors32 = Me.m_textInfo.meshInfo(j).colors32
						Me.m_subTextObjects(j).mesh.RecalculateBounds()
						Me.m_subTextObjects(j).canvasRenderer.SetMesh(Me.m_subTextObjects(j).mesh)
					Next
				End If
				Return
			End If
		End Sub

		' Token: 0x06004FC0 RID: 20416 RVA: 0x00293D72 File Offset: 0x00292172
		Protected Overrides Function GetTextContainerLocalCorners() As Vector3()
			If Me.m_rectTransform Is Nothing Then
				Me.m_rectTransform = MyBase.rectTransform
			End If
			Me.m_rectTransform.GetLocalCorners(Me.m_RectTransformCorners)
			Return Me.m_RectTransformCorners
		End Function

		' Token: 0x06004FC1 RID: 20417 RVA: 0x00293DA8 File Offset: 0x002921A8
		Private Sub ClearMesh()
			Me.m_canvasRenderer.SetMesh(Nothing)
			Dim count As Integer = Me.m_materialReferenceIndexLookup.Count
			For i As Integer = 1 To count - 1
				Me.m_subTextObjects(i).canvasRenderer.SetMesh(Nothing)
			Next
		End Sub

		' Token: 0x06004FC2 RID: 20418 RVA: 0x00293DF4 File Offset: 0x002921F4
		Private Sub UpdateSDFScale(lossyScale As Single)
			lossyScale = If((lossyScale <> 0F), lossyScale, 1F)
			Dim scaleFactor As Single = Me.m_canvas.scaleFactor
			Dim num As Single
			If Me.m_canvas.renderMode = RenderMode.ScreenSpaceOverlay Then
				num = lossyScale / scaleFactor
			ElseIf Me.m_canvas.renderMode = RenderMode.ScreenSpaceCamera Then
				num = If((Not(Me.m_canvas.worldCamera IsNot Nothing)), 1F, lossyScale)
			Else
				num = lossyScale
			End If
			For i As Integer = 0 To Me.m_textInfo.characterCount - 1
				If Me.m_textInfo.characterInfo(i).isVisible AndAlso Me.m_textInfo.characterInfo(i).elementType = TMP_TextElementType.Character Then
					Dim num2 As Single = num * Me.m_textInfo.characterInfo(i).scale * (1F - Me.m_charWidthAdjDelta)
					If(Me.m_textInfo.characterInfo(i).style And FontStyles.Bold) = FontStyles.Bold Then
						num2 *= -1F
					End If
					Dim materialReferenceIndex As Integer = Me.m_textInfo.characterInfo(i).materialReferenceIndex
					Dim vertexIndex As Integer = CInt(Me.m_textInfo.characterInfo(i).vertexIndex)
					Me.m_textInfo.meshInfo(materialReferenceIndex).uvs2(vertexIndex).y = num2
					Me.m_textInfo.meshInfo(materialReferenceIndex).uvs2(vertexIndex + 1).y = num2
					Me.m_textInfo.meshInfo(materialReferenceIndex).uvs2(vertexIndex + 2).y = num2
					Me.m_textInfo.meshInfo(materialReferenceIndex).uvs2(vertexIndex + 3).y = num2
				End If
			Next
			For j As Integer = 0 To Me.m_textInfo.materialCount - 1
				If j = 0 Then
					Me.m_mesh.uv2 = Me.m_textInfo.meshInfo(0).uvs2
					Me.m_canvasRenderer.SetMesh(Me.m_mesh)
				Else
					Me.m_subTextObjects(j).mesh.uv2 = Me.m_textInfo.meshInfo(j).uvs2
					Me.m_subTextObjects(j).canvasRenderer.SetMesh(Me.m_subTextObjects(j).mesh)
				End If
			Next
		End Sub

		' Token: 0x06004FC3 RID: 20419 RVA: 0x0029408C File Offset: 0x0029248C
		Protected Overrides Sub AdjustLineOffset(startIndex As Integer, endIndex As Integer, offset As Single)
			Dim vector As Vector3 = New Vector3(0F, offset, 0F)
			For i As Integer = startIndex To endIndex
				Dim characterInfo As TMP_CharacterInfo() = Me.m_textInfo.characterInfo
				Dim num As Integer = i
				characterInfo(num).bottomLeft = characterInfo(num).bottomLeft - vector
				Dim characterInfo2 As TMP_CharacterInfo() = Me.m_textInfo.characterInfo
				Dim num2 As Integer = i
				characterInfo2(num2).topLeft = characterInfo2(num2).topLeft - vector
				Dim characterInfo3 As TMP_CharacterInfo() = Me.m_textInfo.characterInfo
				Dim num3 As Integer = i
				characterInfo3(num3).topRight = characterInfo3(num3).topRight - vector
				Dim characterInfo4 As TMP_CharacterInfo() = Me.m_textInfo.characterInfo
				Dim num4 As Integer = i
				characterInfo4(num4).bottomRight = characterInfo4(num4).bottomRight - vector
				Dim characterInfo5 As TMP_CharacterInfo() = Me.m_textInfo.characterInfo
				Dim num5 As Integer = i
				characterInfo5(num5).ascender = characterInfo5(num5).ascender - vector.y
				Dim characterInfo6 As TMP_CharacterInfo() = Me.m_textInfo.characterInfo
				Dim num6 As Integer = i
				characterInfo6(num6).baseLine = characterInfo6(num6).baseLine - vector.y
				Dim characterInfo7 As TMP_CharacterInfo() = Me.m_textInfo.characterInfo
				Dim num7 As Integer = i
				characterInfo7(num7).descender = characterInfo7(num7).descender - vector.y
				If Me.m_textInfo.characterInfo(i).isVisible Then
					Dim characterInfo8 As TMP_CharacterInfo() = Me.m_textInfo.characterInfo
					Dim num8 As Integer = i
					characterInfo8(num8).vertex_BL.position = characterInfo8(num8).vertex_BL.position - vector
					Dim characterInfo9 As TMP_CharacterInfo() = Me.m_textInfo.characterInfo
					Dim num9 As Integer = i
					characterInfo9(num9).vertex_TL.position = characterInfo9(num9).vertex_TL.position - vector
					Dim characterInfo10 As TMP_CharacterInfo() = Me.m_textInfo.characterInfo
					Dim num10 As Integer = i
					characterInfo10(num10).vertex_TR.position = characterInfo10(num10).vertex_TR.position - vector
					Dim characterInfo11 As TMP_CharacterInfo() = Me.m_textInfo.characterInfo
					Dim num11 As Integer = i
					characterInfo11(num11).vertex_BR.position = characterInfo11(num11).vertex_BR.position - vector
				End If
			Next
		End Sub

		' Token: 0x0400524F RID: 21071
		Private m_isRebuildingLayout As Boolean

		' Token: 0x04005250 RID: 21072
		<SerializeField()>
		Private m_uvOffset As Vector2 = Vector2.zero

		' Token: 0x04005251 RID: 21073
		<SerializeField()>
		Private m_uvLineOffset As Single

		' Token: 0x04005252 RID: 21074
		<SerializeField()>
		Private m_hasFontAssetChanged As Boolean

		' Token: 0x04005253 RID: 21075
		<SerializeField()>
		Protected m_subTextObjects As TMP_SubMeshUI() = New TMP_SubMeshUI(7) {}

		' Token: 0x04005254 RID: 21076
		Private m_previousLossyScale As Vector3

		' Token: 0x04005255 RID: 21077
		Private m_RectTransformCorners As Vector3() = New Vector3(3) {}

		' Token: 0x04005256 RID: 21078
		Private m_canvasRenderer As CanvasRenderer

		' Token: 0x04005257 RID: 21079
		Private m_canvas As Canvas

		' Token: 0x04005258 RID: 21080
		Private m_isFirstAllocation As Boolean

		' Token: 0x04005259 RID: 21081
		Private m_max_characters As Integer = 8

		' Token: 0x0400525A RID: 21082
		Private m_SavedWordWrapState As WordWrapState = Nothing

		' Token: 0x0400525B RID: 21083
		Private m_SavedLineState As WordWrapState = Nothing

		' Token: 0x0400525C RID: 21084
		Private m_isMaskingEnabled As Boolean

		' Token: 0x0400525D RID: 21085
		<SerializeField()>
		Private m_baseMaterial As Material

		' Token: 0x0400525E RID: 21086
		Private m_isScrollRegionSet As Boolean

		' Token: 0x0400525F RID: 21087
		Private m_stencilID As Integer

		' Token: 0x04005260 RID: 21088
		<SerializeField()>
		Private m_maskOffset As Vector4

		' Token: 0x04005261 RID: 21089
		Private m_EnvMapMatrix As Matrix4x4 = Nothing

		' Token: 0x04005262 RID: 21090
		Private m_isAwake As Boolean

		' Token: 0x04005263 RID: 21091
		<NonSerialized()>
		Private m_isRegisteredForEvents As Boolean

		' Token: 0x04005264 RID: 21092
		Private m_recursiveCount As Integer

		' Token: 0x04005265 RID: 21093
		Private m_recursiveCountA As Integer

		' Token: 0x04005266 RID: 21094
		Private loopCountA As Integer
	End Class
End Namespace
