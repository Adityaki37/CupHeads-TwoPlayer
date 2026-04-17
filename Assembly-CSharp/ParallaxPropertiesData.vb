Imports System
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x020008F4 RID: 2292
Public Class ParallaxPropertiesData
	Inherits ScriptableObject

	' Token: 0x17000451 RID: 1105
	' (get) Token: 0x060035C9 RID: 13769 RVA: 0x001F5DF2 File Offset: 0x001F41F2
	Public Shared ReadOnly Property Instance As ParallaxPropertiesData
		Get
			If ParallaxPropertiesData._instance Is Nothing Then
				ParallaxPropertiesData._instance = Resources.Load(Of ParallaxPropertiesData)("Parallax/data")
			End If
			Return ParallaxPropertiesData._instance
		End Get
	End Property

	' Token: 0x17000452 RID: 1106
	' (get) Token: 0x060035CA RID: 13770 RVA: 0x001F5E18 File Offset: 0x001F4218
	Public ReadOnly Property Properties As List(Of ParallaxPropertiesData.ThemeProperties)
		Get
			Return Me._properties
		End Get
	End Property

	' Token: 0x060035CB RID: 13771 RVA: 0x001F5E20 File Offset: 0x001F4220
	Public Function GetProperty(theme As PlatformingLevel.Theme, layer As Integer, side As PlatformingLevelParallax.Sides) As ParallaxPropertiesData.ThemeProperties.Layer
		If side = PlatformingLevelParallax.Sides.Background OrElse side <> PlatformingLevelParallax.Sides.Foreground Then
			Return Me.GetTheme(theme).background.GetLayer(layer)
		End If
		Return Me.GetTheme(theme).foreground.GetLayer(layer)
	End Function

	' Token: 0x060035CC RID: 13772 RVA: 0x001F5E5C File Offset: 0x001F425C
	Private Function GetTheme(theme As PlatformingLevel.Theme) As ParallaxPropertiesData.ThemeProperties
		For Each themeProperties As ParallaxPropertiesData.ThemeProperties In Me._properties
			If themeProperties.theme = theme Then
				Return themeProperties
			End If
		Next
		Return Nothing
	End Function

	' Token: 0x04003DDA RID: 15834
	Private Const PATH As String = "Parallax/data"

	' Token: 0x04003DDB RID: 15835
	Public Const LAYER_COUNT As Integer = 20

	' Token: 0x04003DDC RID: 15836
	Private Shared _instance As ParallaxPropertiesData

	' Token: 0x04003DDD RID: 15837
	<SerializeField()>
	Private _properties As List(Of ParallaxPropertiesData.ThemeProperties)

	' Token: 0x020008F5 RID: 2293
	<Serializable()>
	Public Class ThemeProperties
		' Token: 0x060035CD RID: 13773 RVA: 0x001F5EC8 File Offset: 0x001F42C8
		Public Sub New()
			Me.background = New ParallaxPropertiesData.ThemeProperties.LayerGroup()
			Me.background.InvertSortingOrder()
			Me.foreground = New ParallaxPropertiesData.ThemeProperties.LayerGroup()
			Me.foreground.InvertSpeed()
		End Sub

		' Token: 0x060035CE RID: 13774 RVA: 0x001F5EFC File Offset: 0x001F42FC
		Public Sub New(theme As PlatformingLevel.Theme)
			Me.theme = theme
			Me.background = New ParallaxPropertiesData.ThemeProperties.LayerGroup()
			Me.background.InvertSortingOrder()
			Me.foreground = New ParallaxPropertiesData.ThemeProperties.LayerGroup()
			Me.foreground.InvertSpeed()
		End Sub

		' Token: 0x04003DDE RID: 15838
		Public theme As PlatformingLevel.Theme

		' Token: 0x04003DDF RID: 15839
		Public background As ParallaxPropertiesData.ThemeProperties.LayerGroup

		' Token: 0x04003DE0 RID: 15840
		Public foreground As ParallaxPropertiesData.ThemeProperties.LayerGroup

		' Token: 0x04003DE1 RID: 15841
		<NonSerialized()>
		Public zEditor_expanded As Boolean

		' Token: 0x020008F6 RID: 2294
		<Serializable()>
		Public Class LayerGroup
			' Token: 0x060035CF RID: 13775 RVA: 0x001F5F38 File Offset: 0x001F4338
			Public Sub New()
				Me.layers = New ParallaxPropertiesData.ThemeProperties.Layer(19) {}
				For i As Integer = 0 To Me.layers.Length - 1
					Me.layers(i) = New ParallaxPropertiesData.ThemeProperties.Layer()
					Me.layers(i).speed = 0.05F * CSng((i + 1))
					Me.layers(i).sortingOrder = 100 * (i + 1)
				Next
			End Sub

			' Token: 0x060035D0 RID: 13776 RVA: 0x001F5FA8 File Offset: 0x001F43A8
			Public Sub InvertSpeed()
				For Each layer As ParallaxPropertiesData.ThemeProperties.Layer In Me.layers
					layer.speed *= -1F
				Next
			End Sub

			' Token: 0x060035D1 RID: 13777 RVA: 0x001F5FE8 File Offset: 0x001F43E8
			Public Sub InvertSortingOrder()
				For Each layer As ParallaxPropertiesData.ThemeProperties.Layer In Me.layers
					layer.sortingOrder *= -1
				Next
			End Sub

			' Token: 0x060035D2 RID: 13778 RVA: 0x001F6022 File Offset: 0x001F4422
			Public Function GetLayer(layer As Integer) As ParallaxPropertiesData.ThemeProperties.Layer
				Return Me.layers(layer)
			End Function

			' Token: 0x04003DE2 RID: 15842
			Public layers As ParallaxPropertiesData.ThemeProperties.Layer()

			' Token: 0x04003DE3 RID: 15843
			<NonSerialized()>
			Public zEditor_expanded As Boolean
		End Class

		' Token: 0x020008F7 RID: 2295
		<Serializable()>
		Public Class Layer
			' Token: 0x04003DE4 RID: 15844
			Public speed As Single

			' Token: 0x04003DE5 RID: 15845
			Public sortingOrder As Integer
		End Class
	End Class
End Class
