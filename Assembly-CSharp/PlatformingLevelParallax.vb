Imports System
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x020008F8 RID: 2296
<ExecuteInEditMode()>
Public Class PlatformingLevelParallax
	Inherits AbstractPausableComponent

	' Token: 0x17000453 RID: 1107
	' (get) Token: 0x060035D5 RID: 13781 RVA: 0x001F6047 File Offset: 0x001F4447
	Public ReadOnly Property Theme As PlatformingLevel.Theme
		Get
			Return Me._theme
		End Get
	End Property

	' Token: 0x17000454 RID: 1108
	' (get) Token: 0x060035D6 RID: 13782 RVA: 0x001F604F File Offset: 0x001F444F
	Public ReadOnly Property Color As Color
		Get
			Return Me._color
		End Get
	End Property

	' Token: 0x17000455 RID: 1109
	' (get) Token: 0x060035D7 RID: 13783 RVA: 0x001F6057 File Offset: 0x001F4457
	Public ReadOnly Property Side As PlatformingLevelParallax.Sides
		Get
			Return Me._side
		End Get
	End Property

	' Token: 0x17000456 RID: 1110
	' (get) Token: 0x060035D8 RID: 13784 RVA: 0x001F605F File Offset: 0x001F445F
	Public ReadOnly Property Layer As Integer
		Get
			Return Me._layer
		End Get
	End Property

	' Token: 0x17000457 RID: 1111
	' (get) Token: 0x060035D9 RID: 13785 RVA: 0x001F6067 File Offset: 0x001F4467
	Public ReadOnly Property SortingOrderOffset As Integer
		Get
			Return Me._sortingOrderOffset
		End Get
	End Property

	' Token: 0x17000458 RID: 1112
	' (get) Token: 0x060035DA RID: 13786 RVA: 0x001F6070 File Offset: 0x001F4470
	Private ReadOnly Property _spriteRenderers As SpriteRenderer()
		Get
			If Me._s Is Nothing Then
				Dim list As List(Of SpriteRenderer) = New List(Of SpriteRenderer)(MyBase.GetComponentsInChildren(Of SpriteRenderer)())
				Me._s = list.ToArray()
			End If
			Return Me._s
		End Get
	End Property

	' Token: 0x17000459 RID: 1113
	' (get) Token: 0x060035DB RID: 13787 RVA: 0x001F60A6 File Offset: 0x001F44A6
	Protected ReadOnly Property transform As Transform
		Get
			If Not Me.transformCached Then
				Me._cachedTransform = MyBase.transform
				Me.transformCached = True
			End If
			Return Me._cachedTransform
		End Get
	End Property

	' Token: 0x1700045A RID: 1114
	' (get) Token: 0x060035DC RID: 13788 RVA: 0x001F60CC File Offset: 0x001F44CC
	Private ReadOnly Property LayerProperties As ParallaxPropertiesData.ThemeProperties.Layer
		Get
			If Not Me.layerPropertiesCached Then
				Me._layerProperties = ParallaxPropertiesData.Instance.GetProperty(Me._theme, Me._layer, Me._side)
				Me.layerPropertiesCached = True
			End If
			Return Me._layerProperties
		End Get
	End Property

	' Token: 0x060035DD RID: 13789 RVA: 0x001F6108 File Offset: 0x001F4508
	Private Sub Start()
		MyBase.FrameDelayedCallback(AddressOf Me.DelayedStart, 1)
		Me.UpdatePosition()
	End Sub

	' Token: 0x060035DE RID: 13790 RVA: 0x001F6124 File Offset: 0x001F4524
	Private Sub DelayedStart()
		Me.SetSpriteProperties()
		Me.UpdatePosition()
	End Sub

	' Token: 0x060035DF RID: 13791 RVA: 0x001F6134 File Offset: 0x001F4534
	Public Sub UpdateBasePosition()
		If Me.levelCameraTransform Is Nothing Then
			Dim cupheadLevelCamera As CupheadLevelCamera = Global.UnityEngine.[Object].FindObjectOfType(Of CupheadLevelCamera)()
			If cupheadLevelCamera Is Nothing Then
				Return
			End If
			Me.levelCameraTransform = cupheadLevelCamera.transform
			If Me.levelCameraTransform Is Nothing Then
				Return
			End If
		End If
		If Me.overrideLayerYSpeed Then
			Me.basePos.x = Me.transform.position.x - Me.levelCameraTransform.position.x * Me.LayerProperties.speed
			Me.basePos.y = Me.transform.position.y - Me.levelCameraTransform.position.y * Me.overrideYSpeed
		Else
			Me.basePos = Me.transform.position - Me.levelCameraTransform.position * Me.LayerProperties.speed
		End If
	End Sub

	' Token: 0x060035E0 RID: 13792 RVA: 0x001F623C File Offset: 0x001F463C
	Public Sub SetSpriteProperties()
		For Each spriteRenderer As SpriteRenderer In Me._spriteRenderers
			spriteRenderer.sortingLayerName = If((Me._side <> PlatformingLevelParallax.Sides.Background), SpriteLayer.Foreground.ToString(), SpriteLayer.Background.ToString())
			spriteRenderer.sortingOrder = Me.LayerProperties.sortingOrder + Me._sortingOrderOffset
			Dim component As PlatformingLevelParallaxChild = spriteRenderer.gameObject.GetComponent(Of PlatformingLevelParallaxChild)()
			If component IsNot Nothing Then
				spriteRenderer.sortingOrder += component.SortingOrderOffset
			End If
			spriteRenderer.color = Me._color
		Next
	End Sub

	' Token: 0x060035E1 RID: 13793 RVA: 0x001F62EF File Offset: 0x001F46EF
	Private Sub LateUpdate()
		Me.UpdatePosition()
	End Sub

	' Token: 0x060035E2 RID: 13794 RVA: 0x001F62F8 File Offset: 0x001F46F8
	Private Sub UpdatePosition()
		If Me.levelCameraTransform Is Nothing Then
			Dim cupheadLevelCamera As CupheadLevelCamera = Global.UnityEngine.[Object].FindObjectOfType(Of CupheadLevelCamera)()
			If cupheadLevelCamera Is Nothing Then
				Return
			End If
			Me.levelCameraTransform = cupheadLevelCamera.transform
			If Me.levelCameraTransform Is Nothing Then
				Return
			End If
		End If
		If Me.overrideLayerYSpeed Then
			Me.transform.SetPosition(New Single?(Me.basePos.x + Me.levelCameraTransform.position.x * Me.LayerProperties.speed), New Single?(Me.basePos.y + Me.levelCameraTransform.position.y * Me.overrideYSpeed), Nothing)
		Else
			Me.transform.position = Me.basePos + Me.levelCameraTransform.position * Me.LayerProperties.speed
		End If
	End Sub

	' Token: 0x060035E3 RID: 13795 RVA: 0x001F63F8 File Offset: 0x001F47F8
	Private Sub OnValidate()
		For Each spriteRenderer As SpriteRenderer In Me._spriteRenderers
			If Not(spriteRenderer Is Nothing) Then
				spriteRenderer.hideFlags = HideFlags.None
			End If
		Next
	End Sub

	' Token: 0x04003DE6 RID: 15846
	<SerializeField()>
	Private _theme As PlatformingLevel.Theme

	' Token: 0x04003DE7 RID: 15847
	<SerializeField()>
	Private _color As Color = Color.white

	' Token: 0x04003DE8 RID: 15848
	<SerializeField()>
	Private _side As PlatformingLevelParallax.Sides

	' Token: 0x04003DE9 RID: 15849
	<SerializeField()>
	<Range(0F, 19F)>
	Private _layer As Integer

	' Token: 0x04003DEA RID: 15850
	<SerializeField()>
	<Range(-2000F, 2000F)>
	Private _sortingOrderOffset As Integer

	' Token: 0x04003DEB RID: 15851
	<HideInInspector()>
	Public basePos As Vector3

	' Token: 0x04003DEC RID: 15852
	<HideInInspector()>
	Public lastPos As Vector3

	' Token: 0x04003DED RID: 15853
	Public overrideLayerYSpeed As Boolean

	' Token: 0x04003DEE RID: 15854
	Public overrideYSpeed As Single

	' Token: 0x04003DEF RID: 15855
	Private levelCameraTransform As Transform

	' Token: 0x04003DF0 RID: 15856
	Private _parallaxLayer As ParallaxLayer

	' Token: 0x04003DF1 RID: 15857
	Private _s As SpriteRenderer()

	' Token: 0x04003DF2 RID: 15858
	Private transformCached As Boolean

	' Token: 0x04003DF3 RID: 15859
	Private _cachedTransform As Transform

	' Token: 0x04003DF4 RID: 15860
	Private layerPropertiesCached As Boolean

	' Token: 0x04003DF5 RID: 15861
	Private _layerProperties As ParallaxPropertiesData.ThemeProperties.Layer

	' Token: 0x020008F9 RID: 2297
	Public Enum Sides
		' Token: 0x04003DF7 RID: 15863
		Background
		' Token: 0x04003DF8 RID: 15864
		Foreground
	End Enum
End Class
