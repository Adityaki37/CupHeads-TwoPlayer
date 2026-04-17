Imports System
Imports UnityEngine
Imports UnityEngine.UI

Namespace Rewired.UI.ControlMapper
	' Token: 0x02000C3A RID: 3130
	<Serializable()>
	Public Class ThemeSettings
		Inherits ScriptableObject

		' Token: 0x06004CEF RID: 19695 RVA: 0x00274BE0 File Offset: 0x00272FE0
		Public Sub Apply(elementInfo As ThemedElement.ElementInfo())
			If elementInfo Is Nothing Then
				Return
			End If
			For i As Integer = 0 To elementInfo.Length - 1
				If elementInfo(i) IsNot Nothing Then
					Me.Apply(elementInfo(i).themeClass, elementInfo(i).component)
				End If
			Next
		End Sub

		' Token: 0x06004CF0 RID: 19696 RVA: 0x00274C2C File Offset: 0x0027302C
		Private Sub Apply(themeClass As String, component As Component)
			If TryCast(component, Selectable) IsNot Nothing Then
				Me.Apply(themeClass, CType(component, Selectable))
				Return
			End If
			If TryCast(component, Image) IsNot Nothing Then
				Me.Apply(themeClass, CType(component, Image))
				Return
			End If
			If TryCast(component, Text) IsNot Nothing Then
				Me.Apply(themeClass, CType(component, Text))
				Return
			End If
			If TryCast(component, UIImageHelper) IsNot Nothing Then
				Me.Apply(themeClass, CType(component, UIImageHelper))
				Return
			End If
		End Sub

		' Token: 0x06004CF1 RID: 19697 RVA: 0x00274CB8 File Offset: 0x002730B8
		Private Sub Apply(themeClass As String, item As Selectable)
			If item Is Nothing Then
				Return
			End If
			Dim selectableSettings_Base As ThemeSettings.SelectableSettings_Base
			If TryCast(item, Button) IsNot Nothing Then
				If themeClass IsNot Nothing Then
					If themeClass = "inputGridField" Then
						selectableSettings_Base = Me._inputGridFieldSettings
						GoTo IL_00A5
					End If
					If themeClass = "windowButton" Then
						selectableSettings_Base = Me._windowButtonSettings
						GoTo IL_00A5
					End If
					If themeClass = "playerButton" Then
						selectableSettings_Base = Me._playerButtonSettings
						GoTo IL_00A5
					End If
					If themeClass = "playerDropdownButton" Then
						selectableSettings_Base = Me._inputGridFieldSettings
						GoTo IL_00A5
					End If
				End If
				selectableSettings_Base = Me._buttonSettings
				IL_00A5:
			ElseIf TryCast(item, Scrollbar) IsNot Nothing Then
				selectableSettings_Base = Me._scrollbarSettings
			ElseIf TryCast(item, Slider) IsNot Nothing Then
				selectableSettings_Base = Me._sliderSettings
			ElseIf TryCast(item, Toggle) IsNot Nothing Then
				If themeClass IsNot Nothing Then
					If themeClass = "inputGridField" Then
						selectableSettings_Base = Me._inputGridFieldSettings
						GoTo IL_0144
					End If
					If themeClass = "button" Then
						selectableSettings_Base = Me._buttonSettings
						GoTo IL_0144
					End If
				End If
				selectableSettings_Base = Me._selectableSettings
				IL_0144:
			Else
				selectableSettings_Base = Me._selectableSettings
			End If
			selectableSettings_Base.Apply(item)
		End Sub

		' Token: 0x06004CF2 RID: 19698 RVA: 0x00274E1C File Offset: 0x0027321C
		Private Sub Apply(themeClass As String, item As Image)
			If item Is Nothing Then
				Return
			End If
			Select Case themeClass
				Case "area"
					Me._areaBackground.CopyTo(item)
				Case "popupWindow"
					Me._popupWindowBackground.CopyTo(item)
				Case "mainWindow"
					Me._mainWindowBackground.CopyTo(item)
				Case "calibrationValueMarker"
					Me._calibrationValueMarker.CopyTo(item)
				Case "calibrationRawValueMarker"
					Me._calibrationRawValueMarker.CopyTo(item)
				Case "invertToggle"
					Me._invertToggle.CopyTo(item)
				Case "invertToggleBackground"
					Me._inputGridFieldSettings.imageSettings.CopyTo(item)
					item.sprite = Me._inputGridFieldSettings.imageSettings.sprite
				Case "invertToggleButtonBackground"
					Me._buttonSettings.imageSettings.CopyTo(item)
			End Select
		End Sub

		' Token: 0x06004CF3 RID: 19699 RVA: 0x00274F98 File Offset: 0x00273398
		Private Sub Apply(themeClass As String, item As Text)
			If item Is Nothing Then
				Return
			End If
			Dim textSettings As ThemeSettings.TextSettings
			Select Case themeClass
				Case "button"
					textSettings = Me._buttonTextSettings
					GoTo IL_0171
				Case "windowButton"
					textSettings = Me._windowButtonTextSettings
					GoTo IL_0171
				Case "playerButton"
					textSettings = Me._playerButtonTextSettings
					GoTo IL_0171
				Case "playerDropdownButton"
					textSettings = Me._playerDropdownButtonTextSettings
					GoTo IL_0171
				Case "restoreDefaultButton"
					textSettings = Me._restoreDefaultButtonTextSettings
					GoTo IL_0171
				Case "inputGridField"
					textSettings = Me._inputGridFieldTextSettings
					GoTo IL_0171
				Case "actionsColumn"
					textSettings = Me._actionColumnTextSettings
					GoTo IL_0171
				Case "actionsColumnDeactivated"
					textSettings = Me._actionColumnDeactivatedTextSettings
					GoTo IL_0171
				Case "actionsColumnHeader"
					textSettings = Me._actionColumnHeaderTextSettings
					GoTo IL_0171
				Case "inputColumnHeader"
					textSettings = Me._inputColumnHeaderTextSettings
					GoTo IL_0171
			End Select
			textSettings = Me._textSettings
			IL_0171:
			If textSettings.fontTypes IsNot Nothing AndAlso CType(textSettings.fontTypes.Length, Localization.Languages) > Localization.language Then
				item.font = FontLoader.GetFont(textSettings.fontTypes(CInt(Localization.language)))
			End If
			item.color = textSettings.color
			item.lineSpacing = textSettings.lineSpacing
			If textSettings.sizeMultiplier <> 1F Then
				item.fontSize = CInt((CSng(item.fontSize) * textSettings.sizeMultiplier))
				item.resizeTextMaxSize = CInt((CSng(item.resizeTextMaxSize) * textSettings.sizeMultiplier))
				item.resizeTextMinSize = CInt((CSng(item.resizeTextMinSize) * textSettings.sizeMultiplier))
			End If
			If textSettings.overrideSize <> 0 Then
				item.fontSize = textSettings.overrideSize
				item.resizeTextMaxSize = textSettings.overrideSize
				item.resizeTextMinSize = textSettings.overrideSize
			End If
			If CType(textSettings.style.Length, Localization.Languages) > Localization.language AndAlso textSettings.style(CInt(Localization.language)) <> ThemeSettings.FontStyleOverride.[Default] Then
				item.fontStyle = CType((textSettings.style(CInt(Localization.language)) - 1), FontStyle)
			End If
		End Sub

		' Token: 0x06004CF4 RID: 19700 RVA: 0x00275217 File Offset: 0x00273617
		Private Sub Apply(themeClass As String, item As UIImageHelper)
			If item Is Nothing Then
				Return
			End If
			item.SetEnabledStateColor(Me._invertToggle.color)
			item.SetDisabledStateColor(Me._invertToggleDisabledColor)
			item.Refresh()
		End Sub

		' Token: 0x0400513A RID: 20794
		<SerializeField()>
		Private _mainWindowBackground As ThemeSettings.ImageSettings

		' Token: 0x0400513B RID: 20795
		<SerializeField()>
		Private _popupWindowBackground As ThemeSettings.ImageSettings

		' Token: 0x0400513C RID: 20796
		<SerializeField()>
		Private _areaBackground As ThemeSettings.ImageSettings

		' Token: 0x0400513D RID: 20797
		<SerializeField()>
		Private _selectableSettings As ThemeSettings.SelectableSettings

		' Token: 0x0400513E RID: 20798
		<SerializeField()>
		Private _buttonSettings As ThemeSettings.SelectableSettings

		' Token: 0x0400513F RID: 20799
		<SerializeField()>
		Private _windowButtonSettings As ThemeSettings.SelectableSettings

		' Token: 0x04005140 RID: 20800
		<SerializeField()>
		Private _playerButtonSettings As ThemeSettings.SelectableSettings

		' Token: 0x04005141 RID: 20801
		<SerializeField()>
		Private _inputGridFieldSettings As ThemeSettings.SelectableSettings

		' Token: 0x04005142 RID: 20802
		<SerializeField()>
		Private _scrollbarSettings As ThemeSettings.ScrollbarSettings

		' Token: 0x04005143 RID: 20803
		<SerializeField()>
		Private _sliderSettings As ThemeSettings.SliderSettings

		' Token: 0x04005144 RID: 20804
		<SerializeField()>
		Private _invertToggle As ThemeSettings.ImageSettings

		' Token: 0x04005145 RID: 20805
		<SerializeField()>
		Private _invertToggleDisabledColor As Color

		' Token: 0x04005146 RID: 20806
		<SerializeField()>
		Private _calibrationValueMarker As ThemeSettings.ImageSettings

		' Token: 0x04005147 RID: 20807
		<SerializeField()>
		Private _calibrationRawValueMarker As ThemeSettings.ImageSettings

		' Token: 0x04005148 RID: 20808
		<SerializeField()>
		Private _textSettings As ThemeSettings.TextSettings

		' Token: 0x04005149 RID: 20809
		<SerializeField()>
		Private _buttonTextSettings As ThemeSettings.TextSettings

		' Token: 0x0400514A RID: 20810
		<SerializeField()>
		Private _windowButtonTextSettings As ThemeSettings.TextSettings

		' Token: 0x0400514B RID: 20811
		<SerializeField()>
		Private _playerButtonTextSettings As ThemeSettings.TextSettings

		' Token: 0x0400514C RID: 20812
		<SerializeField()>
		Private _playerDropdownButtonTextSettings As ThemeSettings.TextSettings

		' Token: 0x0400514D RID: 20813
		<SerializeField()>
		Private _restoreDefaultButtonTextSettings As ThemeSettings.TextSettings

		' Token: 0x0400514E RID: 20814
		<SerializeField()>
		Private _actionColumnTextSettings As ThemeSettings.TextSettings

		' Token: 0x0400514F RID: 20815
		<SerializeField()>
		Private _actionColumnDeactivatedTextSettings As ThemeSettings.TextSettings

		' Token: 0x04005150 RID: 20816
		<SerializeField()>
		Private _actionColumnHeaderTextSettings As ThemeSettings.TextSettings

		' Token: 0x04005151 RID: 20817
		<SerializeField()>
		Private _inputColumnHeaderTextSettings As ThemeSettings.TextSettings

		' Token: 0x04005152 RID: 20818
		<SerializeField()>
		Private _inputGridFieldTextSettings As ThemeSettings.TextSettings

		' Token: 0x02000C3B RID: 3131
		<Serializable()>
		Private MustInherit Class SelectableSettings_Base
			' Token: 0x17000797 RID: 1943
			' (get) Token: 0x06004CF6 RID: 19702 RVA: 0x00275251 File Offset: 0x00273651
			Public ReadOnly Property transition As Selectable.Transition
				Get
					Return Me._transition
				End Get
			End Property

			' Token: 0x17000798 RID: 1944
			' (get) Token: 0x06004CF7 RID: 19703 RVA: 0x00275259 File Offset: 0x00273659
			Public ReadOnly Property selectableColors As ThemeSettings.CustomColorBlock
				Get
					Return Me._colors
				End Get
			End Property

			' Token: 0x17000799 RID: 1945
			' (get) Token: 0x06004CF8 RID: 19704 RVA: 0x00275261 File Offset: 0x00273661
			Public ReadOnly Property spriteState As ThemeSettings.CustomSpriteState
				Get
					Return Me._spriteState
				End Get
			End Property

			' Token: 0x1700079A RID: 1946
			' (get) Token: 0x06004CF9 RID: 19705 RVA: 0x00275269 File Offset: 0x00273669
			Public ReadOnly Property animationTriggers As ThemeSettings.CustomAnimationTriggers
				Get
					Return Me._animationTriggers
				End Get
			End Property

			' Token: 0x06004CFA RID: 19706 RVA: 0x00275274 File Offset: 0x00273674
			Public Overridable Sub Apply(item As Selectable)
				Dim transition As Selectable.Transition = Me._transition
				Dim flag As Boolean = item.transition <> transition
				item.transition = transition
				Dim customSelectable As ICustomSelectable = TryCast(item, ICustomSelectable)
				Dim colors As ThemeSettings.CustomColorBlock = Me._colors
				colors.fadeDuration = 0F
				item.colors = colors
				colors.fadeDuration = Me._colors.fadeDuration
				item.colors = colors
				If customSelectable IsNot Nothing Then
					customSelectable.disabledHighlightedColor = colors.disabledHighlightedColor
				End If
				If transition = Selectable.Transition.SpriteSwap Then
					item.spriteState = Me._spriteState
					If customSelectable IsNot Nothing Then
						customSelectable.disabledHighlightedSprite = Me._spriteState.disabledHighlightedSprite
					End If
				ElseIf transition = Selectable.Transition.Animation Then
					item.animationTriggers.disabledTrigger = Me._animationTriggers.disabledTrigger
					item.animationTriggers.highlightedTrigger = Me._animationTriggers.highlightedTrigger
					item.animationTriggers.normalTrigger = Me._animationTriggers.normalTrigger
					item.animationTriggers.pressedTrigger = Me._animationTriggers.pressedTrigger
					If customSelectable IsNot Nothing Then
						customSelectable.disabledHighlightedTrigger = Me._animationTriggers.disabledHighlightedTrigger
					End If
				End If
				If flag Then
					item.targetGraphic.CrossFadeColor(item.targetGraphic.color, 0F, True, True)
				End If
			End Sub

			' Token: 0x04005155 RID: 20821
			<SerializeField()>
			Protected _transition As Selectable.Transition

			' Token: 0x04005156 RID: 20822
			<SerializeField()>
			Protected _colors As ThemeSettings.CustomColorBlock

			' Token: 0x04005157 RID: 20823
			<SerializeField()>
			Protected _spriteState As ThemeSettings.CustomSpriteState

			' Token: 0x04005158 RID: 20824
			<SerializeField()>
			Protected _animationTriggers As ThemeSettings.CustomAnimationTriggers
		End Class

		' Token: 0x02000C3C RID: 3132
		<Serializable()>
		Private Class SelectableSettings
			Inherits ThemeSettings.SelectableSettings_Base

			' Token: 0x1700079B RID: 1947
			' (get) Token: 0x06004CFC RID: 19708 RVA: 0x002753C8 File Offset: 0x002737C8
			Public ReadOnly Property imageSettings As ThemeSettings.ImageSettings
				Get
					Return Me._imageSettings
				End Get
			End Property

			' Token: 0x06004CFD RID: 19709 RVA: 0x002753D0 File Offset: 0x002737D0
			Public Overrides Sub Apply(item As Selectable)
				If item Is Nothing Then
					Return
				End If
				MyBase.Apply(item)
				If Me._imageSettings IsNot Nothing Then
					Me._imageSettings.CopyTo(TryCast(item.targetGraphic, Image))
				End If
			End Sub

			' Token: 0x04005159 RID: 20825
			<SerializeField()>
			Private _imageSettings As ThemeSettings.ImageSettings
		End Class

		' Token: 0x02000C3D RID: 3133
		<Serializable()>
		Private Class SliderSettings
			Inherits ThemeSettings.SelectableSettings_Base

			' Token: 0x1700079C RID: 1948
			' (get) Token: 0x06004CFF RID: 19711 RVA: 0x0027540F File Offset: 0x0027380F
			Public ReadOnly Property handleImageSettings As ThemeSettings.ImageSettings
				Get
					Return Me._handleImageSettings
				End Get
			End Property

			' Token: 0x1700079D RID: 1949
			' (get) Token: 0x06004D00 RID: 19712 RVA: 0x00275417 File Offset: 0x00273817
			Public ReadOnly Property fillImageSettings As ThemeSettings.ImageSettings
				Get
					Return Me._fillImageSettings
				End Get
			End Property

			' Token: 0x1700079E RID: 1950
			' (get) Token: 0x06004D01 RID: 19713 RVA: 0x0027541F File Offset: 0x0027381F
			Public ReadOnly Property backgroundImageSettings As ThemeSettings.ImageSettings
				Get
					Return Me._backgroundImageSettings
				End Get
			End Property

			' Token: 0x06004D02 RID: 19714 RVA: 0x00275428 File Offset: 0x00273828
			Private Overloads Sub Apply(item As Slider)
				If item Is Nothing Then
					Return
				End If
				If Me._handleImageSettings IsNot Nothing Then
					Me._handleImageSettings.CopyTo(TryCast(item.targetGraphic, Image))
				End If
				If Me._fillImageSettings IsNot Nothing Then
					Dim fillRect As RectTransform = item.fillRect
					If fillRect IsNot Nothing Then
						Me._fillImageSettings.CopyTo(fillRect.GetComponent(Of Image)())
					End If
				End If
				If Me._backgroundImageSettings IsNot Nothing Then
					Dim transform As Transform = item.transform.Find("Background")
					If transform IsNot Nothing Then
						Me._backgroundImageSettings.CopyTo(transform.GetComponent(Of Image)())
					End If
				End If
			End Sub

			' Token: 0x06004D03 RID: 19715 RVA: 0x002754CB File Offset: 0x002738CB
			Public Overrides Overloads Sub Apply(item As Selectable)
				MyBase.Apply(item)
				Me.Apply(TryCast(item, Slider))
			End Sub

			' Token: 0x0400515A RID: 20826
			<SerializeField()>
			Private _handleImageSettings As ThemeSettings.ImageSettings

			' Token: 0x0400515B RID: 20827
			<SerializeField()>
			Private _fillImageSettings As ThemeSettings.ImageSettings

			' Token: 0x0400515C RID: 20828
			<SerializeField()>
			Private _backgroundImageSettings As ThemeSettings.ImageSettings
		End Class

		' Token: 0x02000C3E RID: 3134
		<Serializable()>
		Private Class ScrollbarSettings
			Inherits ThemeSettings.SelectableSettings_Base

			' Token: 0x1700079F RID: 1951
			' (get) Token: 0x06004D05 RID: 19717 RVA: 0x002754E8 File Offset: 0x002738E8
			Public ReadOnly Property handle As ThemeSettings.ImageSettings
				Get
					Return Me._handleImageSettings
				End Get
			End Property

			' Token: 0x170007A0 RID: 1952
			' (get) Token: 0x06004D06 RID: 19718 RVA: 0x002754F0 File Offset: 0x002738F0
			Public ReadOnly Property background As ThemeSettings.ImageSettings
				Get
					Return Me._backgroundImageSettings
				End Get
			End Property

			' Token: 0x06004D07 RID: 19719 RVA: 0x002754F8 File Offset: 0x002738F8
			Private Overloads Sub Apply(item As Scrollbar)
				If item Is Nothing Then
					Return
				End If
				If Me._handleImageSettings IsNot Nothing Then
					Me._handleImageSettings.CopyTo(TryCast(item.targetGraphic, Image))
				End If
				If Me._backgroundImageSettings IsNot Nothing Then
					Me._backgroundImageSettings.CopyTo(item.GetComponent(Of Image)())
				End If
			End Sub

			' Token: 0x06004D08 RID: 19720 RVA: 0x0027554F File Offset: 0x0027394F
			Public Overrides Overloads Sub Apply(item As Selectable)
				MyBase.Apply(item)
				Me.Apply(TryCast(item, Scrollbar))
			End Sub

			' Token: 0x0400515D RID: 20829
			<SerializeField()>
			Private _handleImageSettings As ThemeSettings.ImageSettings

			' Token: 0x0400515E RID: 20830
			<SerializeField()>
			Private _backgroundImageSettings As ThemeSettings.ImageSettings
		End Class

		' Token: 0x02000C3F RID: 3135
		<Serializable()>
		Private Class ImageSettings
			' Token: 0x170007A1 RID: 1953
			' (get) Token: 0x06004D0A RID: 19722 RVA: 0x00275577 File Offset: 0x00273977
			Public ReadOnly Property color As Color
				Get
					Return Me._color
				End Get
			End Property

			' Token: 0x170007A2 RID: 1954
			' (get) Token: 0x06004D0B RID: 19723 RVA: 0x0027557F File Offset: 0x0027397F
			Public ReadOnly Property sprite As Sprite
				Get
					Return Me._sprite
				End Get
			End Property

			' Token: 0x170007A3 RID: 1955
			' (get) Token: 0x06004D0C RID: 19724 RVA: 0x00275587 File Offset: 0x00273987
			Public ReadOnly Property materal As Material
				Get
					Return Me._materal
				End Get
			End Property

			' Token: 0x170007A4 RID: 1956
			' (get) Token: 0x06004D0D RID: 19725 RVA: 0x0027558F File Offset: 0x0027398F
			Public ReadOnly Property type As Image.Type
				Get
					Return Me._type
				End Get
			End Property

			' Token: 0x170007A5 RID: 1957
			' (get) Token: 0x06004D0E RID: 19726 RVA: 0x00275597 File Offset: 0x00273997
			Public ReadOnly Property preserveAspect As Boolean
				Get
					Return Me._preserveAspect
				End Get
			End Property

			' Token: 0x170007A6 RID: 1958
			' (get) Token: 0x06004D0F RID: 19727 RVA: 0x0027559F File Offset: 0x0027399F
			Public ReadOnly Property fillCenter As Boolean
				Get
					Return Me._fillCenter
				End Get
			End Property

			' Token: 0x170007A7 RID: 1959
			' (get) Token: 0x06004D10 RID: 19728 RVA: 0x002755A7 File Offset: 0x002739A7
			Public ReadOnly Property fillMethod As Image.FillMethod
				Get
					Return Me._fillMethod
				End Get
			End Property

			' Token: 0x170007A8 RID: 1960
			' (get) Token: 0x06004D11 RID: 19729 RVA: 0x002755AF File Offset: 0x002739AF
			Public ReadOnly Property fillAmout As Single
				Get
					Return Me._fillAmout
				End Get
			End Property

			' Token: 0x170007A9 RID: 1961
			' (get) Token: 0x06004D12 RID: 19730 RVA: 0x002755B7 File Offset: 0x002739B7
			Public ReadOnly Property fillClockwise As Boolean
				Get
					Return Me._fillClockwise
				End Get
			End Property

			' Token: 0x170007AA RID: 1962
			' (get) Token: 0x06004D13 RID: 19731 RVA: 0x002755BF File Offset: 0x002739BF
			Public ReadOnly Property fillOrigin As Integer
				Get
					Return Me._fillOrigin
				End Get
			End Property

			' Token: 0x06004D14 RID: 19732 RVA: 0x002755C8 File Offset: 0x002739C8
			Public Overridable Sub CopyTo(image As Image)
				If image Is Nothing Then
					Return
				End If
				image.color = Me._color
				image.sprite = Me._sprite
				image.material = Me._materal
				image.type = Me._type
				image.preserveAspect = Me._preserveAspect
				image.fillCenter = Me._fillCenter
				image.fillMethod = Me._fillMethod
				image.fillAmount = Me._fillAmout
				image.fillClockwise = Me._fillClockwise
				image.fillOrigin = Me._fillOrigin
			End Sub

			' Token: 0x0400515F RID: 20831
			<SerializeField()>
			Private _color As Color = Color.white

			' Token: 0x04005160 RID: 20832
			<SerializeField()>
			Private _sprite As Sprite

			' Token: 0x04005161 RID: 20833
			<SerializeField()>
			Private _materal As Material

			' Token: 0x04005162 RID: 20834
			<SerializeField()>
			Private _type As Image.Type

			' Token: 0x04005163 RID: 20835
			<SerializeField()>
			Private _preserveAspect As Boolean

			' Token: 0x04005164 RID: 20836
			<SerializeField()>
			Private _fillCenter As Boolean

			' Token: 0x04005165 RID: 20837
			<SerializeField()>
			Private _fillMethod As Image.FillMethod

			' Token: 0x04005166 RID: 20838
			<SerializeField()>
			Private _fillAmout As Single

			' Token: 0x04005167 RID: 20839
			<SerializeField()>
			Private _fillClockwise As Boolean

			' Token: 0x04005168 RID: 20840
			<SerializeField()>
			Private _fillOrigin As Integer
		End Class

		' Token: 0x02000C40 RID: 3136
		<Serializable()>
		Private Structure CustomColorBlock
			' Token: 0x170007AB RID: 1963
			' (get) Token: 0x06004D15 RID: 19733 RVA: 0x0027565A File Offset: 0x00273A5A
			' (set) Token: 0x06004D16 RID: 19734 RVA: 0x00275662 File Offset: 0x00273A62
			Public Property colorMultiplier As Single
				Get
					Return Me.m_ColorMultiplier
				End Get
				Set(value As Single)
					Me.m_ColorMultiplier = value
				End Set
			End Property

			' Token: 0x170007AC RID: 1964
			' (get) Token: 0x06004D17 RID: 19735 RVA: 0x0027566B File Offset: 0x00273A6B
			' (set) Token: 0x06004D18 RID: 19736 RVA: 0x00275673 File Offset: 0x00273A73
			Public Property disabledColor As Color
				Get
					Return Me.m_DisabledColor
				End Get
				Set(value As Color)
					Me.m_DisabledColor = value
				End Set
			End Property

			' Token: 0x170007AD RID: 1965
			' (get) Token: 0x06004D19 RID: 19737 RVA: 0x0027567C File Offset: 0x00273A7C
			' (set) Token: 0x06004D1A RID: 19738 RVA: 0x00275684 File Offset: 0x00273A84
			Public Property fadeDuration As Single
				Get
					Return Me.m_FadeDuration
				End Get
				Set(value As Single)
					Me.m_FadeDuration = value
				End Set
			End Property

			' Token: 0x170007AE RID: 1966
			' (get) Token: 0x06004D1B RID: 19739 RVA: 0x0027568D File Offset: 0x00273A8D
			' (set) Token: 0x06004D1C RID: 19740 RVA: 0x00275695 File Offset: 0x00273A95
			Public Property highlightedColor As Color
				Get
					Return Me.m_HighlightedColor
				End Get
				Set(value As Color)
					Me.m_HighlightedColor = value
				End Set
			End Property

			' Token: 0x170007AF RID: 1967
			' (get) Token: 0x06004D1D RID: 19741 RVA: 0x0027569E File Offset: 0x00273A9E
			' (set) Token: 0x06004D1E RID: 19742 RVA: 0x002756A6 File Offset: 0x00273AA6
			Public Property normalColor As Color
				Get
					Return Me.m_NormalColor
				End Get
				Set(value As Color)
					Me.m_NormalColor = value
				End Set
			End Property

			' Token: 0x170007B0 RID: 1968
			' (get) Token: 0x06004D1F RID: 19743 RVA: 0x002756AF File Offset: 0x00273AAF
			' (set) Token: 0x06004D20 RID: 19744 RVA: 0x002756B7 File Offset: 0x00273AB7
			Public Property pressedColor As Color
				Get
					Return Me.m_PressedColor
				End Get
				Set(value As Color)
					Me.m_PressedColor = value
				End Set
			End Property

			' Token: 0x170007B1 RID: 1969
			' (get) Token: 0x06004D21 RID: 19745 RVA: 0x002756C0 File Offset: 0x00273AC0
			' (set) Token: 0x06004D22 RID: 19746 RVA: 0x002756C8 File Offset: 0x00273AC8
			Public Property disabledHighlightedColor As Color
				Get
					Return Me.m_DisabledHighlightedColor
				End Get
				Set(value As Color)
					Me.m_DisabledHighlightedColor = value
				End Set
			End Property

			' Token: 0x06004D23 RID: 19747 RVA: 0x002756D4 File Offset: 0x00273AD4
			Public Shared Widening Operator CType(item As ThemeSettings.CustomColorBlock) As ColorBlock
				Return New ColorBlock() With { .colorMultiplier = item.m_ColorMultiplier, .disabledColor = item.m_DisabledColor, .fadeDuration = item.m_FadeDuration, .highlightedColor = item.m_HighlightedColor, .normalColor = item.m_NormalColor, .pressedColor = item.m_PressedColor }
			End Operator

			' Token: 0x04005169 RID: 20841
			<SerializeField()>
			Private m_ColorMultiplier As Single

			' Token: 0x0400516A RID: 20842
			<SerializeField()>
			Private m_DisabledColor As Color

			' Token: 0x0400516B RID: 20843
			<SerializeField()>
			Private m_FadeDuration As Single

			' Token: 0x0400516C RID: 20844
			<SerializeField()>
			Private m_HighlightedColor As Color

			' Token: 0x0400516D RID: 20845
			<SerializeField()>
			Private m_NormalColor As Color

			' Token: 0x0400516E RID: 20846
			<SerializeField()>
			Private m_PressedColor As Color

			' Token: 0x0400516F RID: 20847
			<SerializeField()>
			Private m_DisabledHighlightedColor As Color
		End Structure

		' Token: 0x02000C41 RID: 3137
		<Serializable()>
		Private Structure CustomSpriteState
			' Token: 0x170007B2 RID: 1970
			' (get) Token: 0x06004D24 RID: 19748 RVA: 0x0027573E File Offset: 0x00273B3E
			' (set) Token: 0x06004D25 RID: 19749 RVA: 0x00275746 File Offset: 0x00273B46
			Public Property disabledSprite As Sprite
				Get
					Return Me.m_DisabledSprite
				End Get
				Set(value As Sprite)
					Me.m_DisabledSprite = value
				End Set
			End Property

			' Token: 0x170007B3 RID: 1971
			' (get) Token: 0x06004D26 RID: 19750 RVA: 0x0027574F File Offset: 0x00273B4F
			' (set) Token: 0x06004D27 RID: 19751 RVA: 0x00275757 File Offset: 0x00273B57
			Public Property highlightedSprite As Sprite
				Get
					Return Me.m_HighlightedSprite
				End Get
				Set(value As Sprite)
					Me.m_HighlightedSprite = value
				End Set
			End Property

			' Token: 0x170007B4 RID: 1972
			' (get) Token: 0x06004D28 RID: 19752 RVA: 0x00275760 File Offset: 0x00273B60
			' (set) Token: 0x06004D29 RID: 19753 RVA: 0x00275768 File Offset: 0x00273B68
			Public Property pressedSprite As Sprite
				Get
					Return Me.m_PressedSprite
				End Get
				Set(value As Sprite)
					Me.m_PressedSprite = value
				End Set
			End Property

			' Token: 0x170007B5 RID: 1973
			' (get) Token: 0x06004D2A RID: 19754 RVA: 0x00275771 File Offset: 0x00273B71
			' (set) Token: 0x06004D2B RID: 19755 RVA: 0x00275779 File Offset: 0x00273B79
			Public Property disabledHighlightedSprite As Sprite
				Get
					Return Me.m_DisabledHighlightedSprite
				End Get
				Set(value As Sprite)
					Me.m_DisabledHighlightedSprite = value
				End Set
			End Property

			' Token: 0x06004D2C RID: 19756 RVA: 0x00275784 File Offset: 0x00273B84
			Public Shared Widening Operator CType(item As ThemeSettings.CustomSpriteState) As SpriteState
				Return New SpriteState() With { .disabledSprite = item.m_DisabledSprite, .highlightedSprite = item.m_HighlightedSprite, .pressedSprite = item.m_PressedSprite }
			End Operator

			' Token: 0x04005170 RID: 20848
			<SerializeField()>
			Private m_DisabledSprite As Sprite

			' Token: 0x04005171 RID: 20849
			<SerializeField()>
			Private m_HighlightedSprite As Sprite

			' Token: 0x04005172 RID: 20850
			<SerializeField()>
			Private m_PressedSprite As Sprite

			' Token: 0x04005173 RID: 20851
			<SerializeField()>
			Private m_DisabledHighlightedSprite As Sprite
		End Structure

		' Token: 0x02000C42 RID: 3138
		<Serializable()>
		Private Class CustomAnimationTriggers
			' Token: 0x06004D2D RID: 19757 RVA: 0x002757C4 File Offset: 0x00273BC4
			Public Sub New()
				Me.m_DisabledTrigger = String.Empty
				Me.m_HighlightedTrigger = String.Empty
				Me.m_NormalTrigger = String.Empty
				Me.m_PressedTrigger = String.Empty
				Me.m_DisabledHighlightedTrigger = String.Empty
			End Sub

			' Token: 0x170007B6 RID: 1974
			' (get) Token: 0x06004D2E RID: 19758 RVA: 0x00275803 File Offset: 0x00273C03
			' (set) Token: 0x06004D2F RID: 19759 RVA: 0x0027580B File Offset: 0x00273C0B
			Public Property disabledTrigger As String
				Get
					Return Me.m_DisabledTrigger
				End Get
				Set(value As String)
					Me.m_DisabledTrigger = value
				End Set
			End Property

			' Token: 0x170007B7 RID: 1975
			' (get) Token: 0x06004D30 RID: 19760 RVA: 0x00275814 File Offset: 0x00273C14
			' (set) Token: 0x06004D31 RID: 19761 RVA: 0x0027581C File Offset: 0x00273C1C
			Public Property highlightedTrigger As String
				Get
					Return Me.m_HighlightedTrigger
				End Get
				Set(value As String)
					Me.m_HighlightedTrigger = value
				End Set
			End Property

			' Token: 0x170007B8 RID: 1976
			' (get) Token: 0x06004D32 RID: 19762 RVA: 0x00275825 File Offset: 0x00273C25
			' (set) Token: 0x06004D33 RID: 19763 RVA: 0x0027582D File Offset: 0x00273C2D
			Public Property normalTrigger As String
				Get
					Return Me.m_NormalTrigger
				End Get
				Set(value As String)
					Me.m_NormalTrigger = value
				End Set
			End Property

			' Token: 0x170007B9 RID: 1977
			' (get) Token: 0x06004D34 RID: 19764 RVA: 0x00275836 File Offset: 0x00273C36
			' (set) Token: 0x06004D35 RID: 19765 RVA: 0x0027583E File Offset: 0x00273C3E
			Public Property pressedTrigger As String
				Get
					Return Me.m_PressedTrigger
				End Get
				Set(value As String)
					Me.m_PressedTrigger = value
				End Set
			End Property

			' Token: 0x170007BA RID: 1978
			' (get) Token: 0x06004D36 RID: 19766 RVA: 0x00275847 File Offset: 0x00273C47
			' (set) Token: 0x06004D37 RID: 19767 RVA: 0x0027584F File Offset: 0x00273C4F
			Public Property disabledHighlightedTrigger As String
				Get
					Return Me.m_DisabledHighlightedTrigger
				End Get
				Set(value As String)
					Me.m_DisabledHighlightedTrigger = value
				End Set
			End Property

			' Token: 0x06004D38 RID: 19768 RVA: 0x00275858 File Offset: 0x00273C58
			Public Shared Widening Operator CType(item As ThemeSettings.CustomAnimationTriggers) As AnimationTriggers
				Return New AnimationTriggers() With { .disabledTrigger = item.m_DisabledTrigger, .highlightedTrigger = item.m_HighlightedTrigger, .normalTrigger = item.m_NormalTrigger, .pressedTrigger = item.m_PressedTrigger }
			End Operator

			' Token: 0x04005174 RID: 20852
			<SerializeField()>
			Private m_DisabledTrigger As String

			' Token: 0x04005175 RID: 20853
			<SerializeField()>
			Private m_HighlightedTrigger As String

			' Token: 0x04005176 RID: 20854
			<SerializeField()>
			Private m_NormalTrigger As String

			' Token: 0x04005177 RID: 20855
			<SerializeField()>
			Private m_PressedTrigger As String

			' Token: 0x04005178 RID: 20856
			<SerializeField()>
			Private m_DisabledHighlightedTrigger As String
		End Class

		' Token: 0x02000C43 RID: 3139
		<Serializable()>
		Private Class TextSettings
			' Token: 0x170007BB RID: 1979
			' (get) Token: 0x06004D3A RID: 19770 RVA: 0x002758C5 File Offset: 0x00273CC5
			Public ReadOnly Property color As Color
				Get
					Return Me._color
				End Get
			End Property

			' Token: 0x170007BC RID: 1980
			' (get) Token: 0x06004D3B RID: 19771 RVA: 0x002758CD File Offset: 0x00273CCD
			Public ReadOnly Property fontTypes As FontLoader.FontType()
				Get
					Return Me._fontTypes
				End Get
			End Property

			' Token: 0x170007BD RID: 1981
			' (get) Token: 0x06004D3C RID: 19772 RVA: 0x002758D5 File Offset: 0x00273CD5
			Public ReadOnly Property style As ThemeSettings.FontStyleOverride()
				Get
					Return Me._style
				End Get
			End Property

			' Token: 0x170007BE RID: 1982
			' (get) Token: 0x06004D3D RID: 19773 RVA: 0x002758DD File Offset: 0x00273CDD
			Public ReadOnly Property lineSpacing As Single
				Get
					Return Me._lineSpacing
				End Get
			End Property

			' Token: 0x170007BF RID: 1983
			' (get) Token: 0x06004D3E RID: 19774 RVA: 0x002758E5 File Offset: 0x00273CE5
			Public ReadOnly Property sizeMultiplier As Single
				Get
					Return Me._sizeMultiplier
				End Get
			End Property

			' Token: 0x170007C0 RID: 1984
			' (get) Token: 0x06004D3F RID: 19775 RVA: 0x002758ED File Offset: 0x00273CED
			Public ReadOnly Property overrideSize As Integer
				Get
					Return Me._overrideSize
				End Get
			End Property

			' Token: 0x04005179 RID: 20857
			<SerializeField()>
			Private _color As Color = Color.white

			' Token: 0x0400517A RID: 20858
			<SerializeField()>
			Private _fontTypes As FontLoader.FontType()

			' Token: 0x0400517B RID: 20859
			<SerializeField()>
			Private _style As ThemeSettings.FontStyleOverride()

			' Token: 0x0400517C RID: 20860
			<SerializeField()>
			Private _lineSpacing As Single = 1F

			' Token: 0x0400517D RID: 20861
			<SerializeField()>
			Private _sizeMultiplier As Single = 1F

			' Token: 0x0400517E RID: 20862
			<SerializeField()>
			Private _overrideSize As Integer
		End Class

		' Token: 0x02000C44 RID: 3140
		Private Enum FontStyleOverride
			' Token: 0x04005180 RID: 20864
			[Default]
			' Token: 0x04005181 RID: 20865
			Normal
			' Token: 0x04005182 RID: 20866
			Bold
			' Token: 0x04005183 RID: 20867
			Italic
			' Token: 0x04005184 RID: 20868
			BoldAndItalic
		End Enum
	End Class
End Namespace
