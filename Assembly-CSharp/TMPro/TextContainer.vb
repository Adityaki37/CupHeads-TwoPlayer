Imports System
Imports UnityEngine
Imports UnityEngine.EventSystems

Namespace TMPro
	' Token: 0x02000C69 RID: 3177
	<ExecuteInEditMode()>
	<RequireComponent(GetType(RectTransform))>
	<AddComponentMenu("Layout/Text Container")>
	Public Class TextContainer
		Inherits UIBehaviour

		' Token: 0x17000810 RID: 2064
		' (get) Token: 0x06004F18 RID: 20248 RVA: 0x0027B83A File Offset: 0x00279C3A
		' (set) Token: 0x06004F19 RID: 20249 RVA: 0x0027B842 File Offset: 0x00279C42
		Public Property hasChanged As Boolean
			Get
				Return Me.m_hasChanged
			End Get
			Set(value As Boolean)
				Me.m_hasChanged = value
			End Set
		End Property

		' Token: 0x17000811 RID: 2065
		' (get) Token: 0x06004F1A RID: 20250 RVA: 0x0027B84B File Offset: 0x00279C4B
		' (set) Token: 0x06004F1B RID: 20251 RVA: 0x0027B853 File Offset: 0x00279C53
		Public Property pivot As Vector2
			Get
				Return Me.m_pivot
			End Get
			Set(value As Vector2)
				If Me.m_pivot <> value Then
					Me.m_pivot = value
					Me.m_anchorPosition = Me.GetAnchorPosition(Me.m_pivot)
					Me.m_hasChanged = True
					Me.OnContainerChanged()
				End If
			End Set
		End Property

		' Token: 0x17000812 RID: 2066
		' (get) Token: 0x06004F1C RID: 20252 RVA: 0x0027B88C File Offset: 0x00279C8C
		' (set) Token: 0x06004F1D RID: 20253 RVA: 0x0027B894 File Offset: 0x00279C94
		Public Property anchorPosition As TextContainerAnchors
			Get
				Return Me.m_anchorPosition
			End Get
			Set(value As TextContainerAnchors)
				If Me.m_anchorPosition <> value Then
					Me.m_anchorPosition = value
					Me.m_pivot = Me.GetPivot(Me.m_anchorPosition)
					Me.m_hasChanged = True
					Me.OnContainerChanged()
				End If
			End Set
		End Property

		' Token: 0x17000813 RID: 2067
		' (get) Token: 0x06004F1E RID: 20254 RVA: 0x0027B8C8 File Offset: 0x00279CC8
		' (set) Token: 0x06004F1F RID: 20255 RVA: 0x0027B8D0 File Offset: 0x00279CD0
		Public Property rect As Rect
			Get
				Return Me.m_rect
			End Get
			Set(value As Rect)
				If Me.m_rect <> value Then
					Me.m_rect = value
					Me.m_hasChanged = True
					Me.OnContainerChanged()
				End If
			End Set
		End Property

		' Token: 0x17000814 RID: 2068
		' (get) Token: 0x06004F20 RID: 20256 RVA: 0x0027B8F7 File Offset: 0x00279CF7
		' (set) Token: 0x06004F21 RID: 20257 RVA: 0x0027B914 File Offset: 0x00279D14
		Public Property size As Vector2
			Get
				Return New Vector2(Me.m_rect.width, Me.m_rect.height)
			End Get
			Set(value As Vector2)
				If New Vector2(Me.m_rect.width, Me.m_rect.height) <> value Then
					Me.SetRect(value)
					Me.m_hasChanged = True
					Me.m_isDefaultWidth = False
					Me.m_isDefaultHeight = False
					Me.OnContainerChanged()
				End If
			End Set
		End Property

		' Token: 0x17000815 RID: 2069
		' (get) Token: 0x06004F22 RID: 20258 RVA: 0x0027B969 File Offset: 0x00279D69
		' (set) Token: 0x06004F23 RID: 20259 RVA: 0x0027B976 File Offset: 0x00279D76
		Public Property width As Single
			Get
				Return Me.m_rect.width
			End Get
			Set(value As Single)
				Me.SetRect(New Vector2(value, Me.m_rect.height))
				Me.m_hasChanged = True
				Me.m_isDefaultWidth = False
				Me.OnContainerChanged()
			End Set
		End Property

		' Token: 0x17000816 RID: 2070
		' (get) Token: 0x06004F24 RID: 20260 RVA: 0x0027B9A3 File Offset: 0x00279DA3
		' (set) Token: 0x06004F25 RID: 20261 RVA: 0x0027B9B0 File Offset: 0x00279DB0
		Public Property height As Single
			Get
				Return Me.m_rect.height
			End Get
			Set(value As Single)
				Me.SetRect(New Vector2(Me.m_rect.width, value))
				Me.m_hasChanged = True
				Me.m_isDefaultHeight = False
				Me.OnContainerChanged()
			End Set
		End Property

		' Token: 0x17000817 RID: 2071
		' (get) Token: 0x06004F26 RID: 20262 RVA: 0x0027B9DD File Offset: 0x00279DDD
		Public ReadOnly Property isDefaultWidth As Boolean
			Get
				Return Me.m_isDefaultWidth
			End Get
		End Property

		' Token: 0x17000818 RID: 2072
		' (get) Token: 0x06004F27 RID: 20263 RVA: 0x0027B9E5 File Offset: 0x00279DE5
		Public ReadOnly Property isDefaultHeight As Boolean
			Get
				Return Me.m_isDefaultHeight
			End Get
		End Property

		' Token: 0x17000819 RID: 2073
		' (get) Token: 0x06004F28 RID: 20264 RVA: 0x0027B9ED File Offset: 0x00279DED
		' (set) Token: 0x06004F29 RID: 20265 RVA: 0x0027B9F5 File Offset: 0x00279DF5
		Public Property isAutoFitting As Boolean
			Get
				Return Me.m_isAutoFitting
			End Get
			Set(value As Boolean)
				Me.m_isAutoFitting = value
			End Set
		End Property

		' Token: 0x1700081A RID: 2074
		' (get) Token: 0x06004F2A RID: 20266 RVA: 0x0027B9FE File Offset: 0x00279DFE
		Public ReadOnly Property corners As Vector3()
			Get
				Return Me.m_corners
			End Get
		End Property

		' Token: 0x1700081B RID: 2075
		' (get) Token: 0x06004F2B RID: 20267 RVA: 0x0027BA06 File Offset: 0x00279E06
		Public ReadOnly Property worldCorners As Vector3()
			Get
				Return Me.m_worldCorners
			End Get
		End Property

		' Token: 0x1700081C RID: 2076
		' (get) Token: 0x06004F2C RID: 20268 RVA: 0x0027BA0E File Offset: 0x00279E0E
		' (set) Token: 0x06004F2D RID: 20269 RVA: 0x0027BA16 File Offset: 0x00279E16
		Public Property margins As Vector4
			Get
				Return Me.m_margins
			End Get
			Set(value As Vector4)
				If Me.m_margins <> value Then
					Me.m_margins = value
					Me.m_hasChanged = True
					Me.OnContainerChanged()
				End If
			End Set
		End Property

		' Token: 0x1700081D RID: 2077
		' (get) Token: 0x06004F2E RID: 20270 RVA: 0x0027BA3D File Offset: 0x00279E3D
		Public ReadOnly Property rectTransform As RectTransform
			Get
				If Me.m_rectTransform Is Nothing Then
					Me.m_rectTransform = MyBase.GetComponent(Of RectTransform)()
				End If
				Return Me.m_rectTransform
			End Get
		End Property

		' Token: 0x1700081E RID: 2078
		' (get) Token: 0x06004F2F RID: 20271 RVA: 0x0027BA62 File Offset: 0x00279E62
		Public ReadOnly Property textMeshPro As TextMeshPro
			Get
				If Me.m_textMeshPro Is Nothing Then
					Me.m_textMeshPro = MyBase.GetComponent(Of TextMeshPro)()
				End If
				Return Me.m_textMeshPro
			End Get
		End Property

		' Token: 0x06004F30 RID: 20272 RVA: 0x0027BA88 File Offset: 0x00279E88
		Protected Overrides Sub Awake()
			Me.m_rectTransform = Me.rectTransform
			If Me.m_rectTransform Is Nothing Then
				Dim pivot As Vector2 = Me.m_pivot
				Me.m_rectTransform = MyBase.gameObject.AddComponent(Of RectTransform)()
				Me.m_pivot = pivot
			End If
			Me.m_textMeshPro = TryCast(MyBase.GetComponent(GetType(TextMeshPro)), TextMeshPro)
			If Me.m_rect.width = 0F OrElse Me.m_rect.height = 0F Then
				If Me.m_textMeshPro IsNot Nothing AndAlso Me.m_textMeshPro.anchor <> TMP_Compatibility.AnchorPositions.None Then
					Me.m_isDefaultHeight = True
					Dim num As Integer = CInt(Me.m_textMeshPro.anchor)
					Me.m_textMeshPro.anchor = TMP_Compatibility.AnchorPositions.None
					If num = 9 Then
						Select Case Me.m_textMeshPro.alignment
							Case TextAlignmentOptions.TopLeft
								Me.m_textMeshPro.alignment = TextAlignmentOptions.BaselineLeft
							Case TextAlignmentOptions.Top
								Me.m_textMeshPro.alignment = TextAlignmentOptions.Baseline
							Case TextAlignmentOptions.TopRight
								Me.m_textMeshPro.alignment = TextAlignmentOptions.BaselineRight
							Case TextAlignmentOptions.TopJustified
								Me.m_textMeshPro.alignment = TextAlignmentOptions.BaselineJustified
						End Select
						num = 3
					End If
					Me.m_anchorPosition = CType(num, TextContainerAnchors)
					Me.m_pivot = Me.GetPivot(Me.m_anchorPosition)
					If Me.m_textMeshPro.lineLength = 72F Then
						Me.m_rect.size = Me.m_textMeshPro.GetPreferredValues(Me.m_textMeshPro.text)
					Else
						Me.m_rect.width = Me.m_textMeshPro.lineLength
						Me.m_rect.height = Me.m_textMeshPro.GetPreferredValues(Me.m_rect.width, Single.PositiveInfinity).y
					End If
				Else
					Me.m_isDefaultWidth = True
					Me.m_isDefaultHeight = True
					Me.m_pivot = Me.GetPivot(Me.m_anchorPosition)
					Me.m_rect.width = 20F
					Me.m_rect.height = 5F
					Me.m_rectTransform.sizeDelta = Me.size
				End If
				Me.m_margins = New Vector4(0F, 0F, 0F, 0F)
				Me.UpdateCorners()
			End If
		End Sub

		' Token: 0x06004F31 RID: 20273 RVA: 0x0027BCE5 File Offset: 0x0027A0E5
		Protected Overrides Sub OnEnable()
			Me.OnContainerChanged()
		End Sub

		' Token: 0x06004F32 RID: 20274 RVA: 0x0027BCED File Offset: 0x0027A0ED
		Protected Overrides Sub OnDisable()
		End Sub

		' Token: 0x06004F33 RID: 20275 RVA: 0x0027BCF0 File Offset: 0x0027A0F0
		Private Sub OnContainerChanged()
			Me.UpdateCorners()
			If Me.m_rectTransform IsNot Nothing Then
				Me.m_rectTransform.sizeDelta = Me.size
				Me.m_rectTransform.hasChanged = True
			End If
			If Me.textMeshPro IsNot Nothing Then
				Me.m_textMeshPro.SetVerticesDirty()
				Me.m_textMeshPro.margin = Me.m_margins
			End If
		End Sub

		' Token: 0x06004F34 RID: 20276 RVA: 0x0027BD60 File Offset: 0x0027A160
		Protected Overrides Sub OnRectTransformDimensionsChange()
			If Me.rectTransform Is Nothing Then
				Me.m_rectTransform = MyBase.gameObject.AddComponent(Of RectTransform)()
			End If
			If Me.m_rectTransform.sizeDelta <> TextContainer.k_defaultSize Then
				Me.size = Me.m_rectTransform.sizeDelta
			End If
			Me.pivot = Me.m_rectTransform.pivot
			Me.m_hasChanged = True
			Me.OnContainerChanged()
		End Sub

		' Token: 0x06004F35 RID: 20277 RVA: 0x0027BDD8 File Offset: 0x0027A1D8
		Private Sub SetRect(size As Vector2)
			Me.m_rect = New Rect(Me.m_rect.x, Me.m_rect.y, size.x, size.y)
		End Sub

		' Token: 0x06004F36 RID: 20278 RVA: 0x0027BE0C File Offset: 0x0027A20C
		Private Sub UpdateCorners()
			Me.m_corners(0) = New Vector3(-Me.m_pivot.x * Me.m_rect.width, -Me.m_pivot.y * Me.m_rect.height)
			Me.m_corners(1) = New Vector3(-Me.m_pivot.x * Me.m_rect.width, (1F - Me.m_pivot.y) * Me.m_rect.height)
			Me.m_corners(2) = New Vector3((1F - Me.m_pivot.x) * Me.m_rect.width, (1F - Me.m_pivot.y) * Me.m_rect.height)
			Me.m_corners(3) = New Vector3((1F - Me.m_pivot.x) * Me.m_rect.width, -Me.m_pivot.y * Me.m_rect.height)
			If Me.m_rectTransform IsNot Nothing Then
				Me.m_rectTransform.pivot = Me.m_pivot
			End If
		End Sub

		' Token: 0x06004F37 RID: 20279 RVA: 0x0027BF68 File Offset: 0x0027A368
		Private Function GetPivot(anchor As TextContainerAnchors) As Vector2
			Dim zero As Vector2 = Vector2.zero
			Select Case anchor
				Case TextContainerAnchors.TopLeft
					zero = New Vector2(0F, 1F)
				Case TextContainerAnchors.Top
					zero = New Vector2(0.5F, 1F)
				Case TextContainerAnchors.TopRight
					zero = New Vector2(1F, 1F)
				Case TextContainerAnchors.Left
					zero = New Vector2(0F, 0.5F)
				Case TextContainerAnchors.Middle
					zero = New Vector2(0.5F, 0.5F)
				Case TextContainerAnchors.Right
					zero = New Vector2(1F, 0.5F)
				Case TextContainerAnchors.BottomLeft
					zero = New Vector2(0F, 0F)
				Case TextContainerAnchors.Bottom
					zero = New Vector2(0.5F, 0F)
				Case TextContainerAnchors.BottomRight
					zero = New Vector2(1F, 0F)
			End Select
			Return zero
		End Function

		' Token: 0x06004F38 RID: 20280 RVA: 0x0027C074 File Offset: 0x0027A474
		Private Function GetAnchorPosition(pivot As Vector2) As TextContainerAnchors
			If pivot = New Vector2(0F, 1F) Then
				Return TextContainerAnchors.TopLeft
			End If
			If pivot = New Vector2(0.5F, 1F) Then
				Return TextContainerAnchors.Top
			End If
			If pivot = New Vector2(1F, 1F) Then
				Return TextContainerAnchors.TopRight
			End If
			If pivot = New Vector2(0F, 0.5F) Then
				Return TextContainerAnchors.Left
			End If
			If pivot = New Vector2(0.5F, 0.5F) Then
				Return TextContainerAnchors.Middle
			End If
			If pivot = New Vector2(1F, 0.5F) Then
				Return TextContainerAnchors.Right
			End If
			If pivot = New Vector2(0F, 0F) Then
				Return TextContainerAnchors.BottomLeft
			End If
			If pivot = New Vector2(0.5F, 0F) Then
				Return TextContainerAnchors.Bottom
			End If
			If pivot = New Vector2(1F, 0F) Then
				Return TextContainerAnchors.BottomRight
			End If
			Return TextContainerAnchors.Custom
		End Function

		' Token: 0x04005228 RID: 21032
		Private m_hasChanged As Boolean

		' Token: 0x04005229 RID: 21033
		<SerializeField()>
		Private m_pivot As Vector2

		' Token: 0x0400522A RID: 21034
		<SerializeField()>
		Private m_anchorPosition As TextContainerAnchors = TextContainerAnchors.Middle

		' Token: 0x0400522B RID: 21035
		<SerializeField()>
		Private m_rect As Rect

		' Token: 0x0400522C RID: 21036
		Private m_isDefaultWidth As Boolean

		' Token: 0x0400522D RID: 21037
		Private m_isDefaultHeight As Boolean

		' Token: 0x0400522E RID: 21038
		Private m_isAutoFitting As Boolean

		' Token: 0x0400522F RID: 21039
		Private m_corners As Vector3() = New Vector3(3) {}

		' Token: 0x04005230 RID: 21040
		Private m_worldCorners As Vector3() = New Vector3(3) {}

		' Token: 0x04005231 RID: 21041
		<SerializeField()>
		Private m_margins As Vector4

		' Token: 0x04005232 RID: 21042
		Private m_rectTransform As RectTransform

		' Token: 0x04005233 RID: 21043
		Private Shared k_defaultSize As Vector2 = New Vector2(100F, 100F)

		' Token: 0x04005234 RID: 21044
		Private m_textMeshPro As TextMeshPro
	End Class
End Namespace
