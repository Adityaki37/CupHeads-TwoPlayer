Imports System
Imports UnityEngine

Namespace TMPro
	' Token: 0x02000C65 RID: 3173
	<ExecuteInEditMode()>
	Public Class InlineGraphicManager
		Inherits MonoBehaviour

		' Token: 0x1700080B RID: 2059
		' (get) Token: 0x06004EED RID: 20205 RVA: 0x0027AFFA File Offset: 0x002793FA
		' (set) Token: 0x06004EEE RID: 20206 RVA: 0x0027B002 File Offset: 0x00279402
		Public Property spriteAsset As TMP_SpriteAsset
			Get
				Return Me.m_spriteAsset
			End Get
			Set(value As TMP_SpriteAsset)
				Me.LoadSpriteAsset(value)
			End Set
		End Property

		' Token: 0x1700080C RID: 2060
		' (get) Token: 0x06004EEF RID: 20207 RVA: 0x0027B00B File Offset: 0x0027940B
		' (set) Token: 0x06004EF0 RID: 20208 RVA: 0x0027B013 File Offset: 0x00279413
		Public Property inlineGraphic As InlineGraphic
			Get
				Return Me.m_inlineGraphic
			End Get
			Set(value As InlineGraphic)
				If Me.m_inlineGraphic IsNot value Then
					Me.m_inlineGraphic = value
				End If
			End Set
		End Property

		' Token: 0x1700080D RID: 2061
		' (get) Token: 0x06004EF1 RID: 20209 RVA: 0x0027B02D File Offset: 0x0027942D
		Public ReadOnly Property canvasRenderer As CanvasRenderer
			Get
				Return Me.m_inlineGraphicCanvasRenderer
			End Get
		End Property

		' Token: 0x1700080E RID: 2062
		' (get) Token: 0x06004EF2 RID: 20210 RVA: 0x0027B035 File Offset: 0x00279435
		Public ReadOnly Property uiVertex As UIVertex()
			Get
				Return Me.m_uiVertex
			End Get
		End Property

		' Token: 0x06004EF3 RID: 20211 RVA: 0x0027B03D File Offset: 0x0027943D
		Private Sub Awake()
		End Sub

		' Token: 0x06004EF4 RID: 20212 RVA: 0x0027B03F File Offset: 0x0027943F
		Private Sub OnEnable()
			MyBase.enabled = False
		End Sub

		' Token: 0x06004EF5 RID: 20213 RVA: 0x0027B048 File Offset: 0x00279448
		Private Sub OnDisable()
		End Sub

		' Token: 0x06004EF6 RID: 20214 RVA: 0x0027B04A File Offset: 0x0027944A
		Private Sub OnDestroy()
		End Sub

		' Token: 0x06004EF7 RID: 20215 RVA: 0x0027B04C File Offset: 0x0027944C
		Private Sub LoadSpriteAsset(spriteAsset As TMP_SpriteAsset)
			If spriteAsset Is Nothing Then
				If TMP_Settings.defaultSpriteAsset IsNot Nothing Then
					spriteAsset = TMP_Settings.defaultSpriteAsset
				Else
					spriteAsset = TryCast(Resources.Load("Sprite Assets/Default Sprite Asset"), TMP_SpriteAsset)
				End If
			End If
			Me.m_spriteAsset = spriteAsset
			Me.m_inlineGraphic.texture = Me.m_spriteAsset.spriteSheet
			If Me.m_textComponent IsNot Nothing AndAlso Me.m_isInitialized Then
				Me.m_textComponent.havePropertiesChanged = True
				Me.m_textComponent.SetVerticesDirty()
			End If
		End Sub

		' Token: 0x06004EF8 RID: 20216 RVA: 0x0027B0E4 File Offset: 0x002794E4
		Public Sub AddInlineGraphicsChild()
			If Me.m_inlineGraphic IsNot Nothing Then
				Return
			End If
			Dim gameObject As GameObject = New GameObject("Inline Graphic")
			Me.m_inlineGraphic = gameObject.AddComponent(Of InlineGraphic)()
			Me.m_inlineGraphicRectTransform = gameObject.GetComponent(Of RectTransform)()
			Me.m_inlineGraphicCanvasRenderer = gameObject.GetComponent(Of CanvasRenderer)()
			Me.m_inlineGraphicRectTransform.SetParent(MyBase.transform, False)
			Me.m_inlineGraphicRectTransform.localPosition = Vector3.zero
			Me.m_inlineGraphicRectTransform.anchoredPosition3D = Vector3.zero
			Me.m_inlineGraphicRectTransform.sizeDelta = Vector2.zero
			Me.m_inlineGraphicRectTransform.anchorMin = Vector2.zero
			Me.m_inlineGraphicRectTransform.anchorMax = Vector2.one
			Me.m_textComponent = MyBase.GetComponent(Of TMP_Text)()
		End Sub

		' Token: 0x06004EF9 RID: 20217 RVA: 0x0027B1A0 File Offset: 0x002795A0
		Public Sub AllocatedVertexBuffers(size As Integer)
			If Me.m_inlineGraphic Is Nothing Then
				Me.AddInlineGraphicsChild()
				Me.LoadSpriteAsset(Me.m_spriteAsset)
			End If
			If Me.m_uiVertex Is Nothing Then
				Me.m_uiVertex = New UIVertex(3) {}
			End If
			Dim num As Integer = size * 4
			If num > Me.m_uiVertex.Length Then
				Me.m_uiVertex = New UIVertex(Mathf.NextPowerOfTwo(num) - 1) {}
			End If
		End Sub

		' Token: 0x06004EFA RID: 20218 RVA: 0x0027B20A File Offset: 0x0027960A
		Public Sub UpdatePivot(pivot As Vector2)
			If Me.m_inlineGraphicRectTransform Is Nothing Then
				Me.m_inlineGraphicRectTransform = Me.m_inlineGraphic.GetComponent(Of RectTransform)()
			End If
			Me.m_inlineGraphicRectTransform.pivot = pivot
		End Sub

		' Token: 0x06004EFB RID: 20219 RVA: 0x0027B23A File Offset: 0x0027963A
		Public Sub ClearUIVertex()
			If Me.uiVertex IsNot Nothing AndAlso Me.uiVertex.Length > 0 Then
				Array.Clear(Me.uiVertex, 0, Me.uiVertex.Length)
				Me.m_inlineGraphicCanvasRenderer.Clear()
			End If
		End Sub

		' Token: 0x06004EFC RID: 20220 RVA: 0x0027B274 File Offset: 0x00279674
		Public Sub DrawSprite(uiVertices As UIVertex(), spriteCount As Integer)
			If Me.m_inlineGraphicCanvasRenderer Is Nothing Then
				Me.m_inlineGraphicCanvasRenderer = Me.m_inlineGraphic.GetComponent(Of CanvasRenderer)()
			End If
			Me.m_inlineGraphicCanvasRenderer.SetVertices(uiVertices, spriteCount * 4)
			Me.m_inlineGraphic.UpdateMaterial()
		End Sub

		' Token: 0x06004EFD RID: 20221 RVA: 0x0027B2B4 File Offset: 0x002796B4
		Public Function GetSprite(index As Integer) As TMP_Sprite
			If Me.m_spriteAsset Is Nothing Then
				Return Nothing
			End If
			If Me.m_spriteAsset.spriteInfoList Is Nothing OrElse index > Me.m_spriteAsset.spriteInfoList.Count - 1 Then
				Return Nothing
			End If
			Return Me.m_spriteAsset.spriteInfoList(index)
		End Function

		' Token: 0x06004EFE RID: 20222 RVA: 0x0027B310 File Offset: 0x00279710
		Public Function GetSpriteIndexByHashCode(hashCode As Integer) As Integer
			If Me.m_spriteAsset Is Nothing OrElse Me.m_spriteAsset.spriteInfoList Is Nothing Then
				Return -1
			End If
			Return Me.m_spriteAsset.spriteInfoList.FindIndex(Function(item As TMP_Sprite) item.hashCode = hashCode)
		End Function

		' Token: 0x06004EFF RID: 20223 RVA: 0x0027B36C File Offset: 0x0027976C
		Public Function GetSpriteIndexByIndex(index As Integer) As Integer
			If Me.m_spriteAsset Is Nothing OrElse Me.m_spriteAsset.spriteInfoList Is Nothing Then
				Return -1
			End If
			Return Me.m_spriteAsset.spriteInfoList.FindIndex(Function(item As TMP_Sprite) item.id = index)
		End Function

		' Token: 0x06004F00 RID: 20224 RVA: 0x0027B3C7 File Offset: 0x002797C7
		Public Sub SetUIVertex(uiVertex As UIVertex())
			Me.m_uiVertex = uiVertex
		End Sub

		' Token: 0x0400520A RID: 21002
		<SerializeField()>
		Private m_spriteAsset As TMP_SpriteAsset

		' Token: 0x0400520B RID: 21003
		<SerializeField()>
		<HideInInspector()>
		Private m_inlineGraphic As InlineGraphic

		' Token: 0x0400520C RID: 21004
		<SerializeField()>
		<HideInInspector()>
		Private m_inlineGraphicCanvasRenderer As CanvasRenderer

		' Token: 0x0400520D RID: 21005
		Private m_uiVertex As UIVertex()

		' Token: 0x0400520E RID: 21006
		Private m_inlineGraphicRectTransform As RectTransform

		' Token: 0x0400520F RID: 21007
		Private m_textComponent As TMP_Text

		' Token: 0x04005210 RID: 21008
		Private m_isInitialized As Boolean
	End Class
End Namespace
