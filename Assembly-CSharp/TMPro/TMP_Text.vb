Imports System
Imports System.Collections.Generic
Imports System.Text
Imports UnityEngine
Imports UnityEngine.Serialization
Imports UnityEngine.UI

Namespace TMPro
	' Token: 0x02000C8A RID: 3210
	Public Class TMP_Text
		Inherits MaskableGraphic

		' Token: 0x17000860 RID: 2144
		' (get) Token: 0x06005066 RID: 20582 RVA: 0x0027C3C7 File Offset: 0x0027A7C7
		' (set) Token: 0x06005067 RID: 20583 RVA: 0x0027C3D0 File Offset: 0x0027A7D0
		Public Property text As String
			Get
				Return Me.m_text
			End Get
			Set(value As String)
				If Me.m_text = value Then
					Return
				End If
				Me.m_text = value
				Me.m_inputSource = TMP_Text.TextInputSources.Text
				Me.m_havePropertiesChanged = True
				Me.m_isCalculateSizeRequired = True
				Me.m_isInputParsingRequired = True
				Me.SetVerticesDirty()
				Me.SetLayoutDirty()
			End Set
		End Property

		' Token: 0x17000861 RID: 2145
		' (get) Token: 0x06005068 RID: 20584 RVA: 0x0027C41E File Offset: 0x0027A81E
		' (set) Token: 0x06005069 RID: 20585 RVA: 0x0027C428 File Offset: 0x0027A828
		Public Property font As TMP_FontAsset
			Get
				Return Me.m_fontAsset
			End Get
			Set(value As TMP_FontAsset)
				If Me.m_fontAsset Is value Then
					Return
				End If
				Me.m_fontAsset = value
				Me.LoadFontAsset()
				Me.m_havePropertiesChanged = True
				Me.m_isCalculateSizeRequired = True
				Me.m_isInputParsingRequired = True
				Me.SetVerticesDirty()
				Me.SetLayoutDirty()
			End Set
		End Property

		' Token: 0x17000862 RID: 2146
		' (get) Token: 0x0600506A RID: 20586 RVA: 0x0027C475 File Offset: 0x0027A875
		' (set) Token: 0x0600506B RID: 20587 RVA: 0x0027C47D File Offset: 0x0027A87D
		Public Overridable Property fontSharedMaterial As Material
			Get
				Return Me.m_sharedMaterial
			End Get
			Set(value As Material)
				If Me.m_sharedMaterial Is value Then
					Return
				End If
				Me.SetSharedMaterial(value)
				Me.m_havePropertiesChanged = True
				Me.m_isInputParsingRequired = True
				Me.SetVerticesDirty()
				Me.SetMaterialDirty()
			End Set
		End Property

		' Token: 0x17000863 RID: 2147
		' (get) Token: 0x0600506C RID: 20588 RVA: 0x0027C4B2 File Offset: 0x0027A8B2
		' (set) Token: 0x0600506D RID: 20589 RVA: 0x0027C4BA File Offset: 0x0027A8BA
		Public Overridable Property fontSharedMaterials As Material()
			Get
				Return Me.GetSharedMaterials()
			End Get
			Set(value As Material())
				Me.SetSharedMaterials(value)
				Me.m_havePropertiesChanged = True
				Me.m_isInputParsingRequired = True
				Me.SetVerticesDirty()
				Me.SetMaterialDirty()
			End Set
		End Property

		' Token: 0x17000864 RID: 2148
		' (get) Token: 0x0600506E RID: 20590 RVA: 0x0027C4DD File Offset: 0x0027A8DD
		' (set) Token: 0x0600506F RID: 20591 RVA: 0x0027C4EC File Offset: 0x0027A8EC
		Public Property fontMaterial As Material
			Get
				Return Me.GetMaterial(Me.m_sharedMaterial)
			End Get
			Set(value As Material)
				If Me.m_sharedMaterial IsNot Nothing AndAlso Me.m_sharedMaterial.GetInstanceID() = value.GetInstanceID() Then
					Return
				End If
				Me.m_sharedMaterial = value
				Me.m_padding = Me.GetPaddingForMaterial()
				Me.m_havePropertiesChanged = True
				Me.m_isInputParsingRequired = True
				Me.SetVerticesDirty()
				Me.SetMaterialDirty()
			End Set
		End Property

		' Token: 0x17000865 RID: 2149
		' (get) Token: 0x06005070 RID: 20592 RVA: 0x0027C54E File Offset: 0x0027A94E
		' (set) Token: 0x06005071 RID: 20593 RVA: 0x0027C55C File Offset: 0x0027A95C
		Public Overridable Property fontMaterials As Material()
			Get
				Return Me.GetMaterials(Me.m_fontSharedMaterials)
			End Get
			Set(value As Material())
				Me.SetSharedMaterials(value)
				Me.m_havePropertiesChanged = True
				Me.m_isInputParsingRequired = True
				Me.SetVerticesDirty()
				Me.SetMaterialDirty()
			End Set
		End Property

		' Token: 0x17000866 RID: 2150
		' (get) Token: 0x06005072 RID: 20594 RVA: 0x0027C57F File Offset: 0x0027A97F
		' (set) Token: 0x06005073 RID: 20595 RVA: 0x0027C587 File Offset: 0x0027A987
		Public Property color As Color
			Get
				Return Me.m_fontColor
			End Get
			Set(value As Color)
				If Me.m_fontColor = value Then
					Return
				End If
				Me.m_havePropertiesChanged = True
				Me.m_fontColor = value
				Me.SetVerticesDirty()
			End Set
		End Property

		' Token: 0x17000867 RID: 2151
		' (get) Token: 0x06005074 RID: 20596 RVA: 0x0027C5AF File Offset: 0x0027A9AF
		' (set) Token: 0x06005075 RID: 20597 RVA: 0x0027C5BC File Offset: 0x0027A9BC
		Public Property alpha As Single
			Get
				Return Me.m_fontColor.a
			End Get
			Set(value As Single)
				If Me.m_fontColor.a = value Then
					Return
				End If
				Me.m_fontColor.a = value
				Me.m_havePropertiesChanged = True
				Me.SetVerticesDirty()
			End Set
		End Property

		' Token: 0x17000868 RID: 2152
		' (get) Token: 0x06005076 RID: 20598 RVA: 0x0027C5E9 File Offset: 0x0027A9E9
		' (set) Token: 0x06005077 RID: 20599 RVA: 0x0027C5F1 File Offset: 0x0027A9F1
		Public Property enableVertexGradient As Boolean
			Get
				Return Me.m_enableVertexGradient
			End Get
			Set(value As Boolean)
				If Me.m_enableVertexGradient = value Then
					Return
				End If
				Me.m_havePropertiesChanged = True
				Me.m_enableVertexGradient = value
				Me.SetVerticesDirty()
			End Set
		End Property

		' Token: 0x17000869 RID: 2153
		' (get) Token: 0x06005078 RID: 20600 RVA: 0x0027C614 File Offset: 0x0027AA14
		' (set) Token: 0x06005079 RID: 20601 RVA: 0x0027C61C File Offset: 0x0027AA1C
		Public Property colorGradient As VertexGradient
			Get
				Return Me.m_fontColorGradient
			End Get
			Set(value As VertexGradient)
				Me.m_havePropertiesChanged = True
				Me.m_fontColorGradient = value
				Me.SetVerticesDirty()
			End Set
		End Property

		' Token: 0x1700086A RID: 2154
		' (get) Token: 0x0600507A RID: 20602 RVA: 0x0027C632 File Offset: 0x0027AA32
		' (set) Token: 0x0600507B RID: 20603 RVA: 0x0027C63A File Offset: 0x0027AA3A
		Public Property spriteAsset As TMP_SpriteAsset
			Get
				Return Me.m_spriteAsset
			End Get
			Set(value As TMP_SpriteAsset)
				Me.m_spriteAsset = value
			End Set
		End Property

		' Token: 0x1700086B RID: 2155
		' (get) Token: 0x0600507C RID: 20604 RVA: 0x0027C643 File Offset: 0x0027AA43
		' (set) Token: 0x0600507D RID: 20605 RVA: 0x0027C64B File Offset: 0x0027AA4B
		Public Property tintAllSprites As Boolean
			Get
				Return Me.m_tintAllSprites
			End Get
			Set(value As Boolean)
				If Me.m_tintAllSprites = value Then
					Return
				End If
				Me.m_tintAllSprites = value
				Me.m_havePropertiesChanged = True
				Me.SetVerticesDirty()
			End Set
		End Property

		' Token: 0x1700086C RID: 2156
		' (get) Token: 0x0600507E RID: 20606 RVA: 0x0027C66E File Offset: 0x0027AA6E
		' (set) Token: 0x0600507F RID: 20607 RVA: 0x0027C676 File Offset: 0x0027AA76
		Public Property overrideColorTags As Boolean
			Get
				Return Me.m_overrideHtmlColors
			End Get
			Set(value As Boolean)
				If Me.m_overrideHtmlColors = value Then
					Return
				End If
				Me.m_havePropertiesChanged = True
				Me.m_overrideHtmlColors = value
				Me.SetVerticesDirty()
			End Set
		End Property

		' Token: 0x1700086D RID: 2157
		' (get) Token: 0x06005080 RID: 20608 RVA: 0x0027C699 File Offset: 0x0027AA99
		' (set) Token: 0x06005081 RID: 20609 RVA: 0x0027C6D4 File Offset: 0x0027AAD4
		Public Property faceColor As Color32
			Get
				If Me.m_sharedMaterial Is Nothing Then
					Return Me.m_faceColor
				End If
				Me.m_faceColor = Me.m_sharedMaterial.GetColor(ShaderUtilities.ID_FaceColor)
				Return Me.m_faceColor
			End Get
			Set(value As Color32)
				If Me.m_faceColor.Compare(value) Then
					Return
				End If
				Me.SetFaceColor(value)
				Me.m_havePropertiesChanged = True
				Me.m_faceColor = value
				Me.SetVerticesDirty()
				Me.SetMaterialDirty()
			End Set
		End Property

		' Token: 0x1700086E RID: 2158
		' (get) Token: 0x06005082 RID: 20610 RVA: 0x0027C709 File Offset: 0x0027AB09
		' (set) Token: 0x06005083 RID: 20611 RVA: 0x0027C744 File Offset: 0x0027AB44
		Public Property outlineColor As Color32
			Get
				If Me.m_sharedMaterial Is Nothing Then
					Return Me.m_outlineColor
				End If
				Me.m_outlineColor = Me.m_sharedMaterial.GetColor(ShaderUtilities.ID_OutlineColor)
				Return Me.m_outlineColor
			End Get
			Set(value As Color32)
				If Me.m_outlineColor.Compare(value) Then
					Return
				End If
				Me.SetOutlineColor(value)
				Me.m_havePropertiesChanged = True
				Me.m_outlineColor = value
				Me.SetVerticesDirty()
			End Set
		End Property

		' Token: 0x1700086F RID: 2159
		' (get) Token: 0x06005084 RID: 20612 RVA: 0x0027C773 File Offset: 0x0027AB73
		' (set) Token: 0x06005085 RID: 20613 RVA: 0x0027C7A9 File Offset: 0x0027ABA9
		Public Property outlineWidth As Single
			Get
				If Me.m_sharedMaterial Is Nothing Then
					Return Me.m_outlineWidth
				End If
				Me.m_outlineWidth = Me.m_sharedMaterial.GetFloat(ShaderUtilities.ID_OutlineWidth)
				Return Me.m_outlineWidth
			End Get
			Set(value As Single)
				If Me.m_outlineWidth = value Then
					Return
				End If
				Me.SetOutlineThickness(value)
				Me.m_havePropertiesChanged = True
				Me.m_outlineWidth = value
				Me.SetVerticesDirty()
			End Set
		End Property

		' Token: 0x17000870 RID: 2160
		' (get) Token: 0x06005086 RID: 20614 RVA: 0x0027C7D3 File Offset: 0x0027ABD3
		' (set) Token: 0x06005087 RID: 20615 RVA: 0x0027C7DC File Offset: 0x0027ABDC
		Public Property fontSize As Single
			Get
				Return Me.m_fontSize
			End Get
			Set(value As Single)
				If Me.m_fontSize = value Then
					Return
				End If
				Me.m_havePropertiesChanged = True
				Me.m_isCalculateSizeRequired = True
				Me.SetVerticesDirty()
				Me.SetLayoutDirty()
				Me.m_fontSize = value
				If Not Me.m_enableAutoSizing Then
					Me.m_fontSizeBase = Me.m_fontSize
				End If
			End Set
		End Property

		' Token: 0x17000871 RID: 2161
		' (get) Token: 0x06005088 RID: 20616 RVA: 0x0027C82E File Offset: 0x0027AC2E
		Public ReadOnly Property fontScale As Single
			Get
				Return Me.m_fontScale
			End Get
		End Property

		' Token: 0x17000872 RID: 2162
		' (get) Token: 0x06005089 RID: 20617 RVA: 0x0027C836 File Offset: 0x0027AC36
		' (set) Token: 0x0600508A RID: 20618 RVA: 0x0027C83E File Offset: 0x0027AC3E
		Public Property fontWeight As Integer
			Get
				Return Me.m_fontWeight
			End Get
			Set(value As Integer)
				If Me.m_fontWeight = value Then
					Return
				End If
				Me.m_fontWeight = value
				Me.m_isCalculateSizeRequired = True
				Me.SetVerticesDirty()
				Me.SetLayoutDirty()
			End Set
		End Property

		' Token: 0x17000873 RID: 2163
		' (get) Token: 0x0600508B RID: 20619 RVA: 0x0027C868 File Offset: 0x0027AC68
		Public ReadOnly Property pixelsPerUnit As Single
			Get
				Dim canvas As Canvas = MyBase.canvas
				If Not canvas Then
					Return 1F
				End If
				If Not Me.font Then
					Return canvas.scaleFactor
				End If
				If Me.m_currentFontAsset Is Nothing OrElse Me.m_currentFontAsset.fontInfo.PointSize <= 0F OrElse Me.m_fontSize <= 0F Then
					Return 1F
				End If
				Return Me.m_fontSize / Me.m_currentFontAsset.fontInfo.PointSize
			End Get
		End Property

		' Token: 0x17000874 RID: 2164
		' (get) Token: 0x0600508C RID: 20620 RVA: 0x0027C8FC File Offset: 0x0027ACFC
		' (set) Token: 0x0600508D RID: 20621 RVA: 0x0027C904 File Offset: 0x0027AD04
		Public Property enableAutoSizing As Boolean
			Get
				Return Me.m_enableAutoSizing
			End Get
			Set(value As Boolean)
				If Me.m_enableAutoSizing = value Then
					Return
				End If
				Me.m_enableAutoSizing = value
				Me.SetVerticesDirty()
				Me.SetLayoutDirty()
			End Set
		End Property

		' Token: 0x17000875 RID: 2165
		' (get) Token: 0x0600508E RID: 20622 RVA: 0x0027C926 File Offset: 0x0027AD26
		' (set) Token: 0x0600508F RID: 20623 RVA: 0x0027C92E File Offset: 0x0027AD2E
		Public Property fontSizeMin As Single
			Get
				Return Me.m_fontSizeMin
			End Get
			Set(value As Single)
				If Me.m_fontSizeMin = value Then
					Return
				End If
				Me.m_fontSizeMin = value
				Me.SetVerticesDirty()
				Me.SetLayoutDirty()
			End Set
		End Property

		' Token: 0x17000876 RID: 2166
		' (get) Token: 0x06005090 RID: 20624 RVA: 0x0027C950 File Offset: 0x0027AD50
		' (set) Token: 0x06005091 RID: 20625 RVA: 0x0027C958 File Offset: 0x0027AD58
		Public Property fontSizeMax As Single
			Get
				Return Me.m_fontSizeMax
			End Get
			Set(value As Single)
				If Me.m_fontSizeMax = value Then
					Return
				End If
				Me.m_fontSizeMax = value
				Me.SetVerticesDirty()
				Me.SetLayoutDirty()
			End Set
		End Property

		' Token: 0x17000877 RID: 2167
		' (get) Token: 0x06005092 RID: 20626 RVA: 0x0027C97A File Offset: 0x0027AD7A
		' (set) Token: 0x06005093 RID: 20627 RVA: 0x0027C982 File Offset: 0x0027AD82
		Public Property fontStyle As FontStyles
			Get
				Return Me.m_fontStyle
			End Get
			Set(value As FontStyles)
				If Me.m_fontStyle = value Then
					Return
				End If
				Me.m_fontStyle = value
				Me.m_havePropertiesChanged = True
				Me.checkPaddingRequired = True
				Me.SetVerticesDirty()
				Me.SetLayoutDirty()
			End Set
		End Property

		' Token: 0x17000878 RID: 2168
		' (get) Token: 0x06005094 RID: 20628 RVA: 0x0027C9B2 File Offset: 0x0027ADB2
		Public ReadOnly Property isUsingBold As Boolean
			Get
				Return Me.m_isUsingBold
			End Get
		End Property

		' Token: 0x17000879 RID: 2169
		' (get) Token: 0x06005095 RID: 20629 RVA: 0x0027C9BA File Offset: 0x0027ADBA
		' (set) Token: 0x06005096 RID: 20630 RVA: 0x0027C9C2 File Offset: 0x0027ADC2
		Public Property alignment As TextAlignmentOptions
			Get
				Return Me.m_textAlignment
			End Get
			Set(value As TextAlignmentOptions)
				If Me.m_textAlignment = value Then
					Return
				End If
				Me.m_havePropertiesChanged = True
				Me.m_textAlignment = value
				Me.SetVerticesDirty()
			End Set
		End Property

		' Token: 0x1700087A RID: 2170
		' (get) Token: 0x06005097 RID: 20631 RVA: 0x0027C9E5 File Offset: 0x0027ADE5
		' (set) Token: 0x06005098 RID: 20632 RVA: 0x0027C9ED File Offset: 0x0027ADED
		Public Property characterSpacing As Single
			Get
				Return Me.m_characterSpacing
			End Get
			Set(value As Single)
				If Me.m_characterSpacing = value Then
					Return
				End If
				Me.m_havePropertiesChanged = True
				Me.m_isCalculateSizeRequired = True
				Me.SetVerticesDirty()
				Me.SetLayoutDirty()
				Me.m_characterSpacing = value
			End Set
		End Property

		' Token: 0x1700087B RID: 2171
		' (get) Token: 0x06005099 RID: 20633 RVA: 0x0027CA1D File Offset: 0x0027AE1D
		' (set) Token: 0x0600509A RID: 20634 RVA: 0x0027CA25 File Offset: 0x0027AE25
		Public Property lineSpacing As Single
			Get
				Return Me.m_lineSpacing
			End Get
			Set(value As Single)
				If Me.m_lineSpacing = value Then
					Return
				End If
				Me.m_havePropertiesChanged = True
				Me.m_isCalculateSizeRequired = True
				Me.SetVerticesDirty()
				Me.SetLayoutDirty()
				Me.m_lineSpacing = value
			End Set
		End Property

		' Token: 0x1700087C RID: 2172
		' (get) Token: 0x0600509B RID: 20635 RVA: 0x0027CA55 File Offset: 0x0027AE55
		' (set) Token: 0x0600509C RID: 20636 RVA: 0x0027CA5D File Offset: 0x0027AE5D
		Public Property paragraphSpacing As Single
			Get
				Return Me.m_paragraphSpacing
			End Get
			Set(value As Single)
				If Me.m_paragraphSpacing = value Then
					Return
				End If
				Me.m_havePropertiesChanged = True
				Me.m_isCalculateSizeRequired = True
				Me.SetVerticesDirty()
				Me.SetLayoutDirty()
				Me.m_paragraphSpacing = value
			End Set
		End Property

		' Token: 0x1700087D RID: 2173
		' (get) Token: 0x0600509D RID: 20637 RVA: 0x0027CA8D File Offset: 0x0027AE8D
		' (set) Token: 0x0600509E RID: 20638 RVA: 0x0027CA95 File Offset: 0x0027AE95
		Public Property characterWidthAdjustment As Single
			Get
				Return Me.m_charWidthMaxAdj
			End Get
			Set(value As Single)
				If Me.m_charWidthMaxAdj = value Then
					Return
				End If
				Me.m_havePropertiesChanged = True
				Me.m_isCalculateSizeRequired = True
				Me.SetVerticesDirty()
				Me.SetLayoutDirty()
				Me.m_charWidthMaxAdj = value
			End Set
		End Property

		' Token: 0x1700087E RID: 2174
		' (get) Token: 0x0600509F RID: 20639 RVA: 0x0027CAC5 File Offset: 0x0027AEC5
		' (set) Token: 0x060050A0 RID: 20640 RVA: 0x0027CACD File Offset: 0x0027AECD
		Public Property enableWordWrapping As Boolean
			Get
				Return Me.m_enableWordWrapping
			End Get
			Set(value As Boolean)
				If Me.m_enableWordWrapping = value Then
					Return
				End If
				Me.m_havePropertiesChanged = True
				Me.m_isInputParsingRequired = True
				Me.m_isCalculateSizeRequired = True
				Me.m_enableWordWrapping = value
				Me.SetVerticesDirty()
				Me.SetLayoutDirty()
			End Set
		End Property

		' Token: 0x1700087F RID: 2175
		' (get) Token: 0x060050A1 RID: 20641 RVA: 0x0027CB04 File Offset: 0x0027AF04
		' (set) Token: 0x060050A2 RID: 20642 RVA: 0x0027CB0C File Offset: 0x0027AF0C
		Public Property wordWrappingRatios As Single
			Get
				Return Me.m_wordWrappingRatios
			End Get
			Set(value As Single)
				If Me.m_wordWrappingRatios = value Then
					Return
				End If
				Me.m_wordWrappingRatios = value
				Me.m_havePropertiesChanged = True
				Me.m_isCalculateSizeRequired = True
				Me.SetVerticesDirty()
				Me.SetLayoutDirty()
			End Set
		End Property

		' Token: 0x17000880 RID: 2176
		' (get) Token: 0x060050A3 RID: 20643 RVA: 0x0027CB3C File Offset: 0x0027AF3C
		' (set) Token: 0x060050A4 RID: 20644 RVA: 0x0027CB44 File Offset: 0x0027AF44
		Public Property OverflowMode As TextOverflowModes
			Get
				Return Me.m_overflowMode
			End Get
			Set(value As TextOverflowModes)
				If Me.m_overflowMode = value Then
					Return
				End If
				Me.m_overflowMode = value
				Me.m_havePropertiesChanged = True
				Me.m_isCalculateSizeRequired = True
				Me.SetVerticesDirty()
				Me.SetLayoutDirty()
			End Set
		End Property

		' Token: 0x17000881 RID: 2177
		' (get) Token: 0x060050A5 RID: 20645 RVA: 0x0027CB74 File Offset: 0x0027AF74
		' (set) Token: 0x060050A6 RID: 20646 RVA: 0x0027CB7C File Offset: 0x0027AF7C
		Public Property enableKerning As Boolean
			Get
				Return Me.m_enableKerning
			End Get
			Set(value As Boolean)
				If Me.m_enableKerning = value Then
					Return
				End If
				Me.m_havePropertiesChanged = True
				Me.m_isCalculateSizeRequired = True
				Me.SetVerticesDirty()
				Me.SetLayoutDirty()
				Me.m_enableKerning = value
			End Set
		End Property

		' Token: 0x17000882 RID: 2178
		' (get) Token: 0x060050A7 RID: 20647 RVA: 0x0027CBAC File Offset: 0x0027AFAC
		' (set) Token: 0x060050A8 RID: 20648 RVA: 0x0027CBB4 File Offset: 0x0027AFB4
		Public Property extraPadding As Boolean
			Get
				Return Me.m_enableExtraPadding
			End Get
			Set(value As Boolean)
				If Me.m_enableExtraPadding = value Then
					Return
				End If
				Me.m_havePropertiesChanged = True
				Me.m_enableExtraPadding = value
				Me.UpdateMeshPadding()
				Me.SetVerticesDirty()
			End Set
		End Property

		' Token: 0x17000883 RID: 2179
		' (get) Token: 0x060050A9 RID: 20649 RVA: 0x0027CBDD File Offset: 0x0027AFDD
		' (set) Token: 0x060050AA RID: 20650 RVA: 0x0027CBE5 File Offset: 0x0027AFE5
		Public Property richText As Boolean
			Get
				Return Me.m_isRichText
			End Get
			Set(value As Boolean)
				If Me.m_isRichText = value Then
					Return
				End If
				Me.m_isRichText = value
				Me.m_havePropertiesChanged = True
				Me.m_isCalculateSizeRequired = True
				Me.SetVerticesDirty()
				Me.SetLayoutDirty()
				Me.m_isInputParsingRequired = True
			End Set
		End Property

		' Token: 0x17000884 RID: 2180
		' (get) Token: 0x060050AB RID: 20651 RVA: 0x0027CC1C File Offset: 0x0027B01C
		' (set) Token: 0x060050AC RID: 20652 RVA: 0x0027CC24 File Offset: 0x0027B024
		Public Property parseCtrlCharacters As Boolean
			Get
				Return Me.m_parseCtrlCharacters
			End Get
			Set(value As Boolean)
				If Me.m_parseCtrlCharacters = value Then
					Return
				End If
				Me.m_parseCtrlCharacters = value
				Me.m_havePropertiesChanged = True
				Me.m_isCalculateSizeRequired = True
				Me.SetVerticesDirty()
				Me.SetLayoutDirty()
				Me.m_isInputParsingRequired = True
			End Set
		End Property

		' Token: 0x17000885 RID: 2181
		' (get) Token: 0x060050AD RID: 20653 RVA: 0x0027CC5B File Offset: 0x0027B05B
		' (set) Token: 0x060050AE RID: 20654 RVA: 0x0027CC63 File Offset: 0x0027B063
		Public Property isOverlay As Boolean
			Get
				Return Me.m_isOverlay
			End Get
			Set(value As Boolean)
				If Me.m_isOverlay = value Then
					Return
				End If
				Me.m_isOverlay = value
				Me.SetShaderDepth()
				Me.m_havePropertiesChanged = True
				Me.SetVerticesDirty()
			End Set
		End Property

		' Token: 0x17000886 RID: 2182
		' (get) Token: 0x060050AF RID: 20655 RVA: 0x0027CC8C File Offset: 0x0027B08C
		' (set) Token: 0x060050B0 RID: 20656 RVA: 0x0027CC94 File Offset: 0x0027B094
		Public Property isOrthographic As Boolean
			Get
				Return Me.m_isOrthographic
			End Get
			Set(value As Boolean)
				If Me.m_isOrthographic = value Then
					Return
				End If
				Me.m_havePropertiesChanged = True
				Me.m_isOrthographic = value
				Me.SetVerticesDirty()
			End Set
		End Property

		' Token: 0x17000887 RID: 2183
		' (get) Token: 0x060050B1 RID: 20657 RVA: 0x0027CCB7 File Offset: 0x0027B0B7
		' (set) Token: 0x060050B2 RID: 20658 RVA: 0x0027CCBF File Offset: 0x0027B0BF
		Public Property enableCulling As Boolean
			Get
				Return Me.m_isCullingEnabled
			End Get
			Set(value As Boolean)
				If Me.m_isCullingEnabled = value Then
					Return
				End If
				Me.m_isCullingEnabled = value
				Me.SetCulling()
				Me.m_havePropertiesChanged = True
			End Set
		End Property

		' Token: 0x17000888 RID: 2184
		' (get) Token: 0x060050B3 RID: 20659 RVA: 0x0027CCE2 File Offset: 0x0027B0E2
		' (set) Token: 0x060050B4 RID: 20660 RVA: 0x0027CCEA File Offset: 0x0027B0EA
		Public Property ignoreVisibility As Boolean
			Get
				Return Me.m_ignoreCulling
			End Get
			Set(value As Boolean)
				If Me.m_ignoreCulling = value Then
					Return
				End If
				Me.m_havePropertiesChanged = True
				Me.m_ignoreCulling = value
			End Set
		End Property

		' Token: 0x17000889 RID: 2185
		' (get) Token: 0x060050B5 RID: 20661 RVA: 0x0027CD07 File Offset: 0x0027B107
		' (set) Token: 0x060050B6 RID: 20662 RVA: 0x0027CD0F File Offset: 0x0027B10F
		Public Property horizontalMapping As TextureMappingOptions
			Get
				Return Me.m_horizontalMapping
			End Get
			Set(value As TextureMappingOptions)
				If Me.m_horizontalMapping = value Then
					Return
				End If
				Me.m_havePropertiesChanged = True
				Me.m_horizontalMapping = value
				Me.SetVerticesDirty()
			End Set
		End Property

		' Token: 0x1700088A RID: 2186
		' (get) Token: 0x060050B7 RID: 20663 RVA: 0x0027CD32 File Offset: 0x0027B132
		' (set) Token: 0x060050B8 RID: 20664 RVA: 0x0027CD3A File Offset: 0x0027B13A
		Public Property verticalMapping As TextureMappingOptions
			Get
				Return Me.m_verticalMapping
			End Get
			Set(value As TextureMappingOptions)
				If Me.m_verticalMapping = value Then
					Return
				End If
				Me.m_havePropertiesChanged = True
				Me.m_verticalMapping = value
				Me.SetVerticesDirty()
			End Set
		End Property

		' Token: 0x1700088B RID: 2187
		' (get) Token: 0x060050B9 RID: 20665 RVA: 0x0027CD5D File Offset: 0x0027B15D
		' (set) Token: 0x060050BA RID: 20666 RVA: 0x0027CD65 File Offset: 0x0027B165
		Public Property renderMode As TextRenderFlags
			Get
				Return Me.m_renderMode
			End Get
			Set(value As TextRenderFlags)
				If Me.m_renderMode = value Then
					Return
				End If
				Me.m_renderMode = value
				Me.m_havePropertiesChanged = True
			End Set
		End Property

		' Token: 0x1700088C RID: 2188
		' (get) Token: 0x060050BB RID: 20667 RVA: 0x0027CD82 File Offset: 0x0027B182
		' (set) Token: 0x060050BC RID: 20668 RVA: 0x0027CD8A File Offset: 0x0027B18A
		Public Property maxVisibleCharacters As Integer
			Get
				Return Me.m_maxVisibleCharacters
			End Get
			Set(value As Integer)
				If Me.m_maxVisibleCharacters = value Then
					Return
				End If
				Me.m_havePropertiesChanged = True
				Me.m_maxVisibleCharacters = value
				Me.SetVerticesDirty()
			End Set
		End Property

		' Token: 0x1700088D RID: 2189
		' (get) Token: 0x060050BD RID: 20669 RVA: 0x0027CDAD File Offset: 0x0027B1AD
		' (set) Token: 0x060050BE RID: 20670 RVA: 0x0027CDB5 File Offset: 0x0027B1B5
		Public Property maxVisibleWords As Integer
			Get
				Return Me.m_maxVisibleWords
			End Get
			Set(value As Integer)
				If Me.m_maxVisibleWords = value Then
					Return
				End If
				Me.m_havePropertiesChanged = True
				Me.m_maxVisibleWords = value
				Me.SetVerticesDirty()
			End Set
		End Property

		' Token: 0x1700088E RID: 2190
		' (get) Token: 0x060050BF RID: 20671 RVA: 0x0027CDD8 File Offset: 0x0027B1D8
		' (set) Token: 0x060050C0 RID: 20672 RVA: 0x0027CDE0 File Offset: 0x0027B1E0
		Public Property maxVisibleLines As Integer
			Get
				Return Me.m_maxVisibleLines
			End Get
			Set(value As Integer)
				If Me.m_maxVisibleLines = value Then
					Return
				End If
				Me.m_havePropertiesChanged = True
				Me.m_isInputParsingRequired = True
				Me.m_maxVisibleLines = value
				Me.SetVerticesDirty()
			End Set
		End Property

		' Token: 0x1700088F RID: 2191
		' (get) Token: 0x060050C1 RID: 20673 RVA: 0x0027CE0A File Offset: 0x0027B20A
		' (set) Token: 0x060050C2 RID: 20674 RVA: 0x0027CE12 File Offset: 0x0027B212
		Public Property pageToDisplay As Integer
			Get
				Return Me.m_pageToDisplay
			End Get
			Set(value As Integer)
				If Me.m_pageToDisplay = value Then
					Return
				End If
				Me.m_havePropertiesChanged = True
				Me.m_pageToDisplay = value
				Me.SetVerticesDirty()
			End Set
		End Property

		' Token: 0x17000890 RID: 2192
		' (get) Token: 0x060050C3 RID: 20675 RVA: 0x0027CE35 File Offset: 0x0027B235
		' (set) Token: 0x060050C4 RID: 20676 RVA: 0x0027CE3D File Offset: 0x0027B23D
		Public Overridable Property margin As Vector4
			Get
				Return Me.m_margin
			End Get
			Set(value As Vector4)
				If Me.m_margin = value Then
					Return
				End If
				Me.m_margin = value
				Me.ComputeMarginSize()
				Me.m_havePropertiesChanged = True
				Me.SetVerticesDirty()
			End Set
		End Property

		' Token: 0x17000891 RID: 2193
		' (get) Token: 0x060050C5 RID: 20677 RVA: 0x0027CE6B File Offset: 0x0027B26B
		Public ReadOnly Property textInfo As TMP_TextInfo
			Get
				Return Me.m_textInfo
			End Get
		End Property

		' Token: 0x17000892 RID: 2194
		' (get) Token: 0x060050C6 RID: 20678 RVA: 0x0027CE73 File Offset: 0x0027B273
		' (set) Token: 0x060050C7 RID: 20679 RVA: 0x0027CE7B File Offset: 0x0027B27B
		Public Property havePropertiesChanged As Boolean
			Get
				Return Me.m_havePropertiesChanged
			End Get
			Set(value As Boolean)
				If Me.m_havePropertiesChanged = value Then
					Return
				End If
				Me.m_havePropertiesChanged = value
				Me.SetVerticesDirty()
				Me.SetLayoutDirty()
			End Set
		End Property

		' Token: 0x17000893 RID: 2195
		' (get) Token: 0x060050C8 RID: 20680 RVA: 0x0027CE9D File Offset: 0x0027B29D
		' (set) Token: 0x060050C9 RID: 20681 RVA: 0x0027CEA5 File Offset: 0x0027B2A5
		Public Property isUsingLegacyAnimationComponent As Boolean
			Get
				Return Me.m_isUsingLegacyAnimationComponent
			End Get
			Set(value As Boolean)
				Me.m_isUsingLegacyAnimationComponent = value
			End Set
		End Property

		' Token: 0x17000894 RID: 2196
		' (get) Token: 0x060050CA RID: 20682 RVA: 0x0027CEAE File Offset: 0x0027B2AE
		Public ReadOnly Property transform As Transform
			Get
				If Me.m_transform Is Nothing Then
					Me.m_transform = MyBase.GetComponent(Of Transform)()
				End If
				Return Me.m_transform
			End Get
		End Property

		' Token: 0x17000895 RID: 2197
		' (get) Token: 0x060050CB RID: 20683 RVA: 0x0027CED3 File Offset: 0x0027B2D3
		Public ReadOnly Property rectTransform As RectTransform
			Get
				If Me.m_rectTransform Is Nothing Then
					Me.m_rectTransform = MyBase.GetComponent(Of RectTransform)()
				End If
				Return Me.m_rectTransform
			End Get
		End Property

		' Token: 0x17000896 RID: 2198
		' (get) Token: 0x060050CC RID: 20684 RVA: 0x0027CEF8 File Offset: 0x0027B2F8
		' (set) Token: 0x060050CD RID: 20685 RVA: 0x0027CF00 File Offset: 0x0027B300
		Public Overridable Property autoSizeTextContainer As Boolean

		' Token: 0x17000897 RID: 2199
		' (get) Token: 0x060050CE RID: 20686 RVA: 0x0027CF09 File Offset: 0x0027B309
		Public Overridable ReadOnly Property mesh As Mesh
			Get
				Return Me.m_mesh
			End Get
		End Property

		' Token: 0x17000898 RID: 2200
		' (get) Token: 0x060050CF RID: 20687 RVA: 0x0027CF11 File Offset: 0x0027B311
		' (set) Token: 0x060050D0 RID: 20688 RVA: 0x0027CF19 File Offset: 0x0027B319
		Public Overridable Property bounds As Bounds

		' Token: 0x17000899 RID: 2201
		' (get) Token: 0x060050D1 RID: 20689 RVA: 0x0027CF22 File Offset: 0x0027B322
		Public ReadOnly Property flexibleHeight As Single
			Get
				Return Me.m_flexibleHeight
			End Get
		End Property

		' Token: 0x1700089A RID: 2202
		' (get) Token: 0x060050D2 RID: 20690 RVA: 0x0027CF2A File Offset: 0x0027B32A
		Public ReadOnly Property flexibleWidth As Single
			Get
				Return Me.m_flexibleWidth
			End Get
		End Property

		' Token: 0x1700089B RID: 2203
		' (get) Token: 0x060050D3 RID: 20691 RVA: 0x0027CF32 File Offset: 0x0027B332
		Public ReadOnly Property minHeight As Single
			Get
				Return Me.m_minHeight
			End Get
		End Property

		' Token: 0x1700089C RID: 2204
		' (get) Token: 0x060050D4 RID: 20692 RVA: 0x0027CF3A File Offset: 0x0027B33A
		Public ReadOnly Property minWidth As Single
			Get
				Return Me.m_minWidth
			End Get
		End Property

		' Token: 0x1700089D RID: 2205
		' (get) Token: 0x060050D5 RID: 20693 RVA: 0x0027CF42 File Offset: 0x0027B342
		Public Overridable ReadOnly Property preferredWidth As Single
			Get
				Return If((Me.m_preferredWidth <> 9999F), Me.m_preferredWidth, Me.GetPreferredWidth())
			End Get
		End Property

		' Token: 0x1700089E RID: 2206
		' (get) Token: 0x060050D6 RID: 20694 RVA: 0x0027CF65 File Offset: 0x0027B365
		Public Overridable ReadOnly Property preferredHeight As Single
			Get
				Return If((Me.m_preferredHeight <> 9999F), Me.m_preferredHeight, Me.GetPreferredHeight())
			End Get
		End Property

		' Token: 0x1700089F RID: 2207
		' (get) Token: 0x060050D7 RID: 20695 RVA: 0x0027CF88 File Offset: 0x0027B388
		Public ReadOnly Property layoutPriority As Integer
			Get
				Return Me.m_layoutPriority
			End Get
		End Property

		' Token: 0x060050D8 RID: 20696 RVA: 0x0027CF90 File Offset: 0x0027B390
		Protected Overridable Sub LoadFontAsset()
		End Sub

		' Token: 0x060050D9 RID: 20697 RVA: 0x0027CF92 File Offset: 0x0027B392
		Protected Overridable Sub SetSharedMaterial(mat As Material)
		End Sub

		' Token: 0x060050DA RID: 20698 RVA: 0x0027CF94 File Offset: 0x0027B394
		Protected Overridable Function GetMaterial(mat As Material) As Material
			Return Nothing
		End Function

		' Token: 0x060050DB RID: 20699 RVA: 0x0027CF97 File Offset: 0x0027B397
		Protected Overridable Sub SetFontBaseMaterial(mat As Material)
		End Sub

		' Token: 0x060050DC RID: 20700 RVA: 0x0027CF99 File Offset: 0x0027B399
		Protected Overridable Function GetSharedMaterials() As Material()
			Return Nothing
		End Function

		' Token: 0x060050DD RID: 20701 RVA: 0x0027CF9C File Offset: 0x0027B39C
		Protected Overridable Sub SetSharedMaterials(materials As Material())
		End Sub

		' Token: 0x060050DE RID: 20702 RVA: 0x0027CF9E File Offset: 0x0027B39E
		Protected Overridable Function GetMaterials(mats As Material()) As Material()
			Return Nothing
		End Function

		' Token: 0x060050DF RID: 20703 RVA: 0x0027CFA4 File Offset: 0x0027B3A4
		Protected Overridable Function CreateMaterialInstance(source As Material) As Material
			Dim material As Material = New Material(source)
			material.shaderKeywords = source.shaderKeywords
			Dim material2 As Material = material
			material2.name += " (Instance)"
			Return material
		End Function

		' Token: 0x060050E0 RID: 20704 RVA: 0x0027CFDB File Offset: 0x0027B3DB
		Protected Overridable Sub SetFaceColor(color As Color32)
		End Sub

		' Token: 0x060050E1 RID: 20705 RVA: 0x0027CFDD File Offset: 0x0027B3DD
		Protected Overridable Sub SetOutlineColor(color As Color32)
		End Sub

		' Token: 0x060050E2 RID: 20706 RVA: 0x0027CFDF File Offset: 0x0027B3DF
		Protected Overridable Sub SetOutlineThickness(thickness As Single)
		End Sub

		' Token: 0x060050E3 RID: 20707 RVA: 0x0027CFE1 File Offset: 0x0027B3E1
		Protected Overridable Sub SetShaderDepth()
		End Sub

		' Token: 0x060050E4 RID: 20708 RVA: 0x0027CFE3 File Offset: 0x0027B3E3
		Protected Overridable Sub SetCulling()
		End Sub

		' Token: 0x060050E5 RID: 20709 RVA: 0x0027CFE5 File Offset: 0x0027B3E5
		Protected Overridable Function GetPaddingForMaterial() As Single
			Return 0F
		End Function

		' Token: 0x060050E6 RID: 20710 RVA: 0x0027CFEC File Offset: 0x0027B3EC
		Protected Overridable Function GetPaddingForMaterial(mat As Material) As Single
			Return 0F
		End Function

		' Token: 0x060050E7 RID: 20711 RVA: 0x0027CFF3 File Offset: 0x0027B3F3
		Protected Overridable Function GetTextContainerLocalCorners() As Vector3()
			Return Nothing
		End Function

		' Token: 0x060050E8 RID: 20712 RVA: 0x0027CFF6 File Offset: 0x0027B3F6
		Public Overridable Sub ForceMeshUpdate()
		End Sub

		' Token: 0x060050E9 RID: 20713 RVA: 0x0027CFF8 File Offset: 0x0027B3F8
		Public Overridable Sub UpdateGeometry(mesh As Mesh, index As Integer)
		End Sub

		' Token: 0x060050EA RID: 20714 RVA: 0x0027CFFA File Offset: 0x0027B3FA
		Public Overridable Sub UpdateVertexData(flags As TMP_VertexDataUpdateFlags)
		End Sub

		' Token: 0x060050EB RID: 20715 RVA: 0x0027CFFC File Offset: 0x0027B3FC
		Public Overridable Sub UpdateVertexData()
		End Sub

		' Token: 0x060050EC RID: 20716 RVA: 0x0027CFFE File Offset: 0x0027B3FE
		Public Overridable Sub SetVertices(vertices As Vector3())
		End Sub

		' Token: 0x060050ED RID: 20717 RVA: 0x0027D000 File Offset: 0x0027B400
		Public Overridable Sub UpdateMeshPadding()
		End Sub

		' Token: 0x060050EE RID: 20718 RVA: 0x0027D002 File Offset: 0x0027B402
		Public Sub SetText(text As String)
			Me.StringToCharArray(text, Me.m_char_buffer)
			Me.m_inputSource = TMP_Text.TextInputSources.SetCharArray
			Me.m_isInputParsingRequired = True
			Me.m_havePropertiesChanged = True
			Me.m_isCalculateSizeRequired = True
			Me.SetVerticesDirty()
			Me.SetLayoutDirty()
		End Sub

		' Token: 0x060050EF RID: 20719 RVA: 0x0027D039 File Offset: 0x0027B439
		Public Sub SetText(text As String, arg0 As Single)
			Me.SetText(text, arg0, 255F, 255F)
		End Sub

		' Token: 0x060050F0 RID: 20720 RVA: 0x0027D04D File Offset: 0x0027B44D
		Public Sub SetText(text As String, arg0 As Single, arg1 As Single)
			Me.SetText(text, arg0, arg1, 255F)
		End Sub

		' Token: 0x060050F1 RID: 20721 RVA: 0x0027D060 File Offset: 0x0027B460
		Public Sub SetText(text As String, arg0 As Single, arg1 As Single, arg2 As Single)
			If text = Me.old_text AndAlso arg0 = Me.old_arg0 AndAlso arg1 = Me.old_arg1 AndAlso arg2 = Me.old_arg2 Then
				Return
			End If
			Me.old_text = text
			Me.old_arg1 = 255F
			Me.old_arg2 = 255F
			Dim num As Integer = 0
			Dim num2 As Integer = 0
			For i As Integer = 0 To text.Length - 1
				Dim c As Char = text(i)
				If c = "{"c Then
					If text(i + 2) = ":"c Then
						num = CInt((text(i + 3) - "0"c))
					End If
					Dim num3 As Integer = CInt((text(i + 1) - "0"c))
					If num3 <> 0 Then
						If num3 <> 1 Then
							If num3 = 2 Then
								Me.old_arg2 = arg2
								Me.AddFloatToCharArray(arg2, num2, num)
							End If
						Else
							Me.old_arg1 = arg1
							Me.AddFloatToCharArray(arg1, num2, num)
						End If
					Else
						Me.old_arg0 = arg0
						Me.AddFloatToCharArray(arg0, num2, num)
					End If
					If text(i + 2) = ":"c Then
						i += 4
					Else
						i += 2
					End If
				Else
					Me.m_input_CharArray(num2) = c
					num2 += 1
				End If
			Next
			Me.m_input_CharArray(num2) = vbNullChar
			Me.m_charArray_Length = num2
			Me.m_inputSource = TMP_Text.TextInputSources.SetText
			Me.m_isInputParsingRequired = True
			Me.m_havePropertiesChanged = True
			Me.m_isCalculateSizeRequired = True
			Me.SetVerticesDirty()
			Me.SetLayoutDirty()
		End Sub

		' Token: 0x060050F2 RID: 20722 RVA: 0x0027D1DE File Offset: 0x0027B5DE
		Public Sub SetText(text As StringBuilder)
			Me.StringBuilderToIntArray(text, Me.m_char_buffer)
			Me.m_inputSource = TMP_Text.TextInputSources.SetCharArray
			Me.m_isInputParsingRequired = True
			Me.m_havePropertiesChanged = True
			Me.m_isCalculateSizeRequired = True
			Me.SetVerticesDirty()
			Me.SetLayoutDirty()
		End Sub

		' Token: 0x060050F3 RID: 20723 RVA: 0x0027D218 File Offset: 0x0027B618
		Public Sub SetCharArray(charArray As Char())
			If charArray Is Nothing OrElse charArray.Length = 0 Then
				Return
			End If
			If Me.m_char_buffer.Length <= charArray.Length Then
				Dim num As Integer = Mathf.NextPowerOfTwo(charArray.Length + 1)
				Me.m_char_buffer = New Integer(num - 1) {}
			End If
			Dim num2 As Integer = 0
			Dim i As Integer = 0
			While i < charArray.Length
				If charArray(i) <> "\"c OrElse i >= charArray.Length - 1 Then
					GoTo IL_00BC
				End If
				Dim num3 As Integer = CInt(charArray(i + 1))
				If num3 <> 110 Then
					If num3 <> 114 Then
						If num3 <> 116 Then
							GoTo IL_00BC
						End If
						Me.m_char_buffer(num2) = 9
						i += 1
						num2 += 1
					Else
						Me.m_char_buffer(num2) = 13
						i += 1
						num2 += 1
					End If
				Else
					Me.m_char_buffer(num2) = 10
					i += 1
					num2 += 1
				End If
				IL_00CB:
				i += 1
				Continue While
				IL_00BC:
				Me.m_char_buffer(num2) = CInt(charArray(i))
				num2 += 1
				GoTo IL_00CB
			End While
			Me.m_char_buffer(num2) = 0
			Me.m_inputSource = TMP_Text.TextInputSources.SetCharArray
			Me.m_havePropertiesChanged = True
			Me.m_isInputParsingRequired = True
		End Sub

		' Token: 0x060050F4 RID: 20724 RVA: 0x0027D31C File Offset: 0x0027B71C
		Protected Sub SetTextArrayToCharArray(charArray As Char(), ByRef charBuffer As Integer())
			If charArray Is Nothing OrElse Me.m_charArray_Length = 0 Then
				Return
			End If
			If charBuffer.Length <= Me.m_charArray_Length Then
				Dim num As Integer = If((Me.m_charArray_Length <= 1024), Mathf.NextPowerOfTwo(Me.m_charArray_Length + 1), (Me.m_charArray_Length + 256))
				charBuffer = New Integer(num - 1) {}
			End If
			Dim num2 As Integer = 0
			For i As Integer = 0 To Me.m_charArray_Length - 1
				If Char.IsHighSurrogate(charArray(i)) AndAlso Char.IsLowSurrogate(charArray(i + 1)) Then
					charBuffer(num2) = Char.ConvertToUtf32(charArray(i), charArray(i + 1))
					i += 1
					num2 += 1
				Else
					charBuffer(num2) = CInt(charArray(i))
					num2 += 1
				End If
			Next
			charBuffer(num2) = 0
		End Sub

		' Token: 0x060050F5 RID: 20725 RVA: 0x0027D3E4 File Offset: 0x0027B7E4
		Protected Sub StringToCharArray(text As String, ByRef chars As Integer())
			If text Is Nothing Then
				chars(0) = 0
				Return
			End If
			If chars Is Nothing OrElse chars.Length <= text.Length Then
				Dim num As Integer = If((text.Length <= 1024), Mathf.NextPowerOfTwo(text.Length + 1), (text.Length + 256))
				chars = New Integer(num - 1) {}
			End If
			Dim num2 As Integer = 0
			Dim i As Integer = 0
			While i < text.Length
				If Not Me.m_parseCtrlCharacters OrElse text(i) <> "\"c OrElse text.Length <= i + 1 Then
					GoTo IL_019B
				End If
				Dim num3 As Integer = CInt(text(i + 1))
				Select Case num3
					Case 114
						chars(num2) = 13
						i += 1
						num2 += 1
					Case Else
						If num3 <> 85 Then
							If num3 <> 92 Then
								If num3 <> 110 Then
									GoTo IL_019B
								End If
								chars(num2) = 10
								i += 1
								num2 += 1
							Else
								If text.Length <= i + 2 Then
									GoTo IL_019B
								End If
								chars(num2) = CInt(text(i + 1))
								chars(num2 + 1) = CInt(text(i + 2))
								i += 2
								num2 += 2
							End If
						Else
							If text.Length <= i + 9 Then
								GoTo IL_019B
							End If
							chars(num2) = Me.GetUTF32(i + 2)
							i += 9
							num2 += 1
						End If
					Case 116
						chars(num2) = 9
						i += 1
						num2 += 1
					Case 117
						If text.Length <= i + 5 Then
							GoTo IL_019B
						End If
						chars(num2) = CInt(CUShort(Me.GetUTF16(i + 2)))
						i += 5
						num2 += 1
				End Select
				IL_01F4:
				i += 1
				Continue While
				IL_019B:
				If Char.IsHighSurrogate(text(i)) AndAlso Char.IsLowSurrogate(text(i + 1)) Then
					chars(num2) = Char.ConvertToUtf32(text(i), text(i + 1))
					i += 1
					num2 += 1
					GoTo IL_01F4
				End If
				chars(num2) = CInt(text(i))
				num2 += 1
				GoTo IL_01F4
			End While
			chars(num2) = 0
		End Sub

		' Token: 0x060050F6 RID: 20726 RVA: 0x0027D5FC File Offset: 0x0027B9FC
		Protected Sub StringBuilderToIntArray(text As StringBuilder, ByRef chars As Integer())
			If text Is Nothing Then
				chars(0) = 0
				Return
			End If
			If chars Is Nothing OrElse chars.Length <= text.Length Then
				Dim num As Integer = If((text.Length <= 1024), Mathf.NextPowerOfTwo(text.Length + 1), (text.Length + 256))
				chars = New Integer(num - 1) {}
			End If
			Dim num2 As Integer = 0
			Dim i As Integer = 0
			While i < text.Length
				If Not Me.m_parseCtrlCharacters OrElse text(i) <> "\"c OrElse text.Length <= i + 1 Then
					GoTo IL_019B
				End If
				Dim num3 As Integer = CInt(text(i + 1))
				Select Case num3
					Case 114
						chars(num2) = 13
						i += 1
						num2 += 1
					Case Else
						If num3 <> 85 Then
							If num3 <> 92 Then
								If num3 <> 110 Then
									GoTo IL_019B
								End If
								chars(num2) = 10
								i += 1
								num2 += 1
							Else
								If text.Length <= i + 2 Then
									GoTo IL_019B
								End If
								chars(num2) = CInt(text(i + 1))
								chars(num2 + 1) = CInt(text(i + 2))
								i += 2
								num2 += 2
							End If
						Else
							If text.Length <= i + 9 Then
								GoTo IL_019B
							End If
							chars(num2) = Me.GetUTF32(i + 2)
							i += 9
							num2 += 1
						End If
					Case 116
						chars(num2) = 9
						i += 1
						num2 += 1
					Case 117
						If text.Length <= i + 5 Then
							GoTo IL_019B
						End If
						chars(num2) = CInt(CUShort(Me.GetUTF16(i + 2)))
						i += 5
						num2 += 1
				End Select
				IL_01F4:
				i += 1
				Continue While
				IL_019B:
				If Char.IsHighSurrogate(text(i)) AndAlso Char.IsLowSurrogate(text(i + 1)) Then
					chars(num2) = Char.ConvertToUtf32(text(i), text(i + 1))
					i += 1
					num2 += 1
					GoTo IL_01F4
				End If
				chars(num2) = CInt(text(i))
				num2 += 1
				GoTo IL_01F4
			End While
			chars(num2) = 0
		End Sub

		' Token: 0x060050F7 RID: 20727 RVA: 0x0027D814 File Offset: 0x0027BC14
		Protected Sub AddFloatToCharArray(number As Single, ByRef index As Integer, precision As Integer)
			If number < 0F Then
				Dim input_CharArray As Char() = Me.m_input_CharArray
				Dim num As Integer = index
				Dim num2 As Integer = num
				index = num + 1
				input_CharArray(num2) = 45
				number = -number
			End If
			number += Me.k_Power(Mathf.Min(9, precision))
			Dim num3 As Integer = CInt(number)
			Me.AddIntToCharArray(num3, index, precision)
			If precision > 0 Then
				Dim input_CharArray2 As Char() = Me.m_input_CharArray
				Dim num4 As Integer = index
				Dim num2 As Integer = num4
				index = num4 + 1
				input_CharArray2(num2) = 46
				number -= CSng(num3)
				For i As Integer = 0 To precision - 1
					number *= 10F
					Dim num5 As Integer = CInt(number)
					Dim input_CharArray3 As Char() = Me.m_input_CharArray
					Dim num6 As Integer = index
					num2 = num6
					index = num6 + 1
					input_CharArray3(num2) = CUShort((num5 + 48))
					number -= CSng(num5)
				Next
			End If
		End Sub

		' Token: 0x060050F8 RID: 20728 RVA: 0x0027D8BC File Offset: 0x0027BCBC
		Protected Sub AddIntToCharArray(number As Integer, ByRef index As Integer, precision As Integer)
			If number < 0 Then
				Dim input_CharArray As Char() = Me.m_input_CharArray
				Dim num As Integer = index
				Dim num2 As Integer = num
				index = num + 1
				input_CharArray(num2) = 45
				number = -number
			End If
			Dim num3 As Integer = index
			Do
				Dim input_CharArray2 As Char() = Me.m_input_CharArray
				Dim num4 As Integer = num3
				num3 = num4 + 1
				input_CharArray2(num4) = CUShort((number Mod 10 + 48))
				number /= 10
			Loop While number > 0
			Dim num5 As Integer = num3
			While index + 1 < num3
				num3 -= 1
				Dim c As Char = Me.m_input_CharArray(index)
				Me.m_input_CharArray(index) = Me.m_input_CharArray(num3)
				Me.m_input_CharArray(num3) = c
				index += 1
			End While
			index = num5
		End Sub

		' Token: 0x060050F9 RID: 20729 RVA: 0x0027D94C File Offset: 0x0027BD4C
		Protected Overridable Function SetArraySizes(chars As Integer()) As Integer
			Return 0
		End Function

		' Token: 0x060050FA RID: 20730 RVA: 0x0027D950 File Offset: 0x0027BD50
		Protected Sub ParseInputText()
			Me.m_isInputParsingRequired = False
			Dim inputSource As TMP_Text.TextInputSources = Me.m_inputSource
			If inputSource <> TMP_Text.TextInputSources.Text Then
				If inputSource <> TMP_Text.TextInputSources.SetText Then
					If inputSource <> TMP_Text.TextInputSources.SetCharArray Then
					End If
				Else
					Me.SetTextArrayToCharArray(Me.m_input_CharArray, Me.m_char_buffer)
				End If
			Else
				Me.StringToCharArray(Me.m_text, Me.m_char_buffer)
			End If
			Me.SetArraySizes(Me.m_char_buffer)
		End Sub

		' Token: 0x060050FB RID: 20731 RVA: 0x0027D9C4 File Offset: 0x0027BDC4
		Protected Overridable Sub GenerateTextMesh()
		End Sub

		' Token: 0x060050FC RID: 20732 RVA: 0x0027D9C8 File Offset: 0x0027BDC8
		Public Function GetPreferredValues() As Vector2
			If Me.m_isInputParsingRequired OrElse Me.m_isTextTruncated Then
				Me.ParseInputText()
			End If
			Dim preferredWidth As Single = Me.GetPreferredWidth()
			Dim preferredHeight As Single = Me.GetPreferredHeight()
			Return New Vector2(preferredWidth, preferredHeight)
		End Function

		' Token: 0x060050FD RID: 20733 RVA: 0x0027DA08 File Offset: 0x0027BE08
		Public Function GetPreferredValues(width As Single, height As Single) As Vector2
			If Me.m_isInputParsingRequired OrElse Me.m_isTextTruncated Then
				Me.ParseInputText()
			End If
			Dim vector As Vector2 = New Vector2(width, height)
			Dim preferredWidth As Single = Me.GetPreferredWidth(vector)
			Dim preferredHeight As Single = Me.GetPreferredHeight(vector)
			Return New Vector2(preferredWidth, preferredHeight)
		End Function

		' Token: 0x060050FE RID: 20734 RVA: 0x0027DA54 File Offset: 0x0027BE54
		Public Function GetPreferredValues(text As String) As Vector2
			Me.StringToCharArray(text, Me.m_char_buffer)
			Me.SetArraySizes(Me.m_char_buffer)
			Dim vector As Vector2 = New Vector2(Single.PositiveInfinity, Single.PositiveInfinity)
			Dim preferredWidth As Single = Me.GetPreferredWidth(vector)
			Dim preferredHeight As Single = Me.GetPreferredHeight(vector)
			Return New Vector2(preferredWidth, preferredHeight)
		End Function

		' Token: 0x060050FF RID: 20735 RVA: 0x0027DAA4 File Offset: 0x0027BEA4
		Public Function GetPreferredValues(text As String, width As Single, height As Single) As Vector2
			Me.StringToCharArray(text, Me.m_char_buffer)
			Me.SetArraySizes(Me.m_char_buffer)
			Dim vector As Vector2 = New Vector2(width, height)
			Dim preferredWidth As Single = Me.GetPreferredWidth(vector)
			Dim preferredHeight As Single = Me.GetPreferredHeight(vector)
			Return New Vector2(preferredWidth, preferredHeight)
		End Function

		' Token: 0x06005100 RID: 20736 RVA: 0x0027DAEC File Offset: 0x0027BEEC
		Protected Function GetPreferredWidth() As Single
			Dim num As Single = If((Not Me.m_enableAutoSizing), Me.m_fontSize, Me.m_fontSizeMax)
			Dim vector As Vector2 = New Vector2(Single.PositiveInfinity, Single.PositiveInfinity)
			If Me.m_isInputParsingRequired OrElse Me.m_isTextTruncated Then
				Me.ParseInputText()
			End If
			Return Me.CalculatePreferredValues(num, vector).x
		End Function

		' Token: 0x06005101 RID: 20737 RVA: 0x0027DB58 File Offset: 0x0027BF58
		Protected Function GetPreferredWidth(margin As Vector2) As Single
			Dim num As Single = If((Not Me.m_enableAutoSizing), Me.m_fontSize, Me.m_fontSizeMax)
			Return Me.CalculatePreferredValues(num, margin).x
		End Function

		' Token: 0x06005102 RID: 20738 RVA: 0x0027DB94 File Offset: 0x0027BF94
		Protected Function GetPreferredHeight() As Single
			Dim num As Single = If((Not Me.m_enableAutoSizing), Me.m_fontSize, Me.m_fontSizeMax)
			Dim vector As Vector2 = New Vector2(If((Me.m_marginWidth = 0F), Single.PositiveInfinity, Me.m_marginWidth), Single.PositiveInfinity)
			If Me.m_isInputParsingRequired OrElse Me.m_isTextTruncated Then
				Me.ParseInputText()
			End If
			Return Me.CalculatePreferredValues(num, vector).y
		End Function

		' Token: 0x06005103 RID: 20739 RVA: 0x0027DC18 File Offset: 0x0027C018
		Protected Function GetPreferredHeight(margin As Vector2) As Single
			Dim num As Single = If((Not Me.m_enableAutoSizing), Me.m_fontSize, Me.m_fontSizeMax)
			Return Me.CalculatePreferredValues(num, margin).y
		End Function

		' Token: 0x06005104 RID: 20740 RVA: 0x0027DC54 File Offset: 0x0027C054
		Protected Overridable Function CalculatePreferredValues(defaultFontSize As Single, marginSize As Vector2) As Vector2
			If Me.m_fontAsset Is Nothing OrElse Me.m_fontAsset.characterDictionary Is Nothing Then
				Return Vector2.zero
			End If
			If Me.m_char_buffer Is Nothing OrElse Me.m_char_buffer.Length = 0 OrElse Me.m_char_buffer(0) = 0 Then
				Return Vector2.zero
			End If
			Me.m_currentFontAsset = Me.m_fontAsset
			Me.m_currentMaterial = Me.m_sharedMaterial
			Me.m_currentMaterialIndex = 0
			Me.m_materialReferenceStack.SetDefault(New MaterialReference(0, Me.m_currentFontAsset, Nothing, Me.m_currentMaterial, Me.m_padding))
			Dim totalCharacterCount As Integer = Me.m_totalCharacterCount
			If Me.m_internalCharacterInfo Is Nothing OrElse totalCharacterCount > Me.m_internalCharacterInfo.Length Then
				Me.m_internalCharacterInfo = New TMP_CharacterInfo(If((totalCharacterCount <= 1024), Mathf.NextPowerOfTwo(totalCharacterCount), (totalCharacterCount + 256)) - 1) {}
			End If
			Me.m_fontScale = defaultFontSize / Me.m_currentFontAsset.fontInfo.PointSize * If((Not Me.m_isOrthographic), 0.1F, 1F)
			Me.m_fontScaleMultiplier = 1F
			Dim num As Single = defaultFontSize / Me.m_fontAsset.fontInfo.PointSize * Me.m_fontAsset.fontInfo.Scale * If((Not Me.m_isOrthographic), 0.1F, 1F)
			Dim num2 As Single = Me.m_fontScale
			Me.m_currentFontSize = defaultFontSize
			Me.m_sizeStack.SetDefault(Me.m_currentFontSize)
			Me.m_style = Me.m_fontStyle
			Me.m_baselineOffset = 0F
			Me.m_styleStack.Clear()
			Me.m_lineOffset = 0F
			Me.m_lineHeight = 0F
			Dim num3 As Single = Me.m_currentFontAsset.fontInfo.LineHeight - (Me.m_currentFontAsset.fontInfo.Ascender - Me.m_currentFontAsset.fontInfo.Descender)
			Me.m_cSpacing = 0F
			Me.m_monoSpacing = 0F
			Me.m_xAdvance = 0F
			Dim num4 As Single = 0F
			Me.tag_LineIndent = 0F
			Me.tag_Indent = 0F
			Me.m_indentStack.SetDefault(0F)
			Me.tag_NoParsing = False
			Me.m_characterCount = 0
			Me.m_firstCharacterOfLine = 0
			Me.m_maxLineAscender = Single.NegativeInfinity
			Me.m_maxLineDescender = Single.PositiveInfinity
			Me.m_lineNumber = 0
			Dim x As Single = marginSize.x
			Me.m_marginLeft = 0F
			Me.m_marginRight = 0F
			Me.m_width = -1F
			Dim num5 As Single = 0F
			Dim num6 As Single = 0F
			Me.m_maxAscender = 0F
			Me.m_maxDescender = 0F
			Dim flag As Boolean = True
			Dim flag2 As Boolean = False
			Dim wordWrapState As WordWrapState = Nothing
			Me.SaveWordWrappingState(wordWrapState, 0, 0)
			Dim wordWrapState2 As WordWrapState = Nothing
			Dim num7 As Integer = 0
			Dim num8 As Integer = 0
			Dim num9 As Integer = 0
			While Me.m_char_buffer(num9) <> 0
				Dim num10 As Integer = Me.m_char_buffer(num9)
				Me.m_textElementType = TMP_TextElementType.Character
				Me.m_currentMaterialIndex = Me.m_textInfo.characterInfo(Me.m_characterCount).materialReferenceIndex
				Me.m_currentFontAsset = Me.m_materialReferences(Me.m_currentMaterialIndex).fontAsset
				Dim currentMaterialIndex As Integer = Me.m_currentMaterialIndex
				If Not Me.m_isRichText OrElse num10 <> 60 Then
					GoTo IL_038C
				End If
				Me.m_isParsingText = True
				If Not Me.ValidateHtmlTag(Me.m_char_buffer, num9 + 1, num8) Then
					GoTo IL_038C
				End If
				num9 = num8
				If Me.m_textElementType <> TMP_TextElementType.Character Then
					GoTo IL_038C
				End If
				IL_0FE9:
				num9 += 1
				Continue While
				IL_038C:
				Me.m_isParsingText = False
				Dim num11 As Single = 1F
				If Me.m_textElementType = TMP_TextElementType.Character Then
					If(Me.m_style And FontStyles.UpperCase) = FontStyles.UpperCase Then
						If Char.IsLower(CChar(num10)) Then
							num10 = CInt(Char.ToUpper(CChar(num10)))
						End If
					ElseIf(Me.m_style And FontStyles.LowerCase) = FontStyles.LowerCase Then
						If Char.IsUpper(CChar(num10)) Then
							num10 = CInt(Char.ToLower(CChar(num10)))
						End If
					ElseIf((Me.m_fontStyle And FontStyles.SmallCaps) = FontStyles.SmallCaps OrElse (Me.m_style And FontStyles.SmallCaps) = FontStyles.SmallCaps) AndAlso Char.IsLower(CChar(num10)) Then
						num11 = 0.8F
						num10 = CInt(Char.ToUpper(CChar(num10)))
					End If
				End If
				If Me.m_textElementType = TMP_TextElementType.Sprite Then
					Dim tmp_Sprite As TMP_Sprite = Me.m_currentSpriteAsset.spriteInfoList(Me.m_spriteIndex)
					If tmp_Sprite Is Nothing Then
						GoTo IL_0FE9
					End If
					num10 = 57344 + Me.m_spriteIndex
					Me.m_cached_TextElement = tmp_Sprite
					num2 = Me.m_fontAsset.fontInfo.Ascender / tmp_Sprite.height * tmp_Sprite.scale * num
					Me.m_internalCharacterInfo(Me.m_characterCount).elementType = TMP_TextElementType.Sprite
					Me.m_currentMaterialIndex = currentMaterialIndex
				ElseIf Me.m_textElementType = TMP_TextElementType.Character Then
					Me.m_cached_TextElement = Me.m_textInfo.characterInfo(Me.m_characterCount).textElement
					Me.m_currentFontAsset = Me.m_textInfo.characterInfo(Me.m_characterCount).fontAsset
					Me.m_currentMaterialIndex = Me.m_textInfo.characterInfo(Me.m_characterCount).materialReferenceIndex
					Me.m_fontScale = Me.m_currentFontSize * num11 / Me.m_currentFontAsset.fontInfo.PointSize * Me.m_currentFontAsset.fontInfo.Scale * If((Not Me.m_isOrthographic), 0.1F, 1F)
					num2 = Me.m_fontScale * Me.m_fontScaleMultiplier
					Me.m_internalCharacterInfo(Me.m_characterCount).elementType = TMP_TextElementType.Character
				End If
				Me.m_internalCharacterInfo(Me.m_characterCount).character = CChar(num10)
				If Me.m_enableKerning AndAlso Me.m_characterCount >= 1 Then
					Dim character As Integer = CInt(Me.m_internalCharacterInfo(Me.m_characterCount - 1).character)
					Dim kerningPairKey As KerningPairKey = New KerningPairKey(character, num10)
					Dim kerningPair As KerningPair
					Me.m_currentFontAsset.kerningDictionary.TryGetValue(kerningPairKey.key, kerningPair)
					If kerningPair IsNot Nothing Then
						Me.m_xAdvance += kerningPair.XadvanceOffset * num2
					End If
				End If
				Dim num12 As Single = 0F
				If Me.m_monoSpacing <> 0F Then
					num12 = Me.m_monoSpacing / 2F - (Me.m_cached_TextElement.width / 2F + Me.m_cached_TextElement.xOffset) * num2
					Me.m_xAdvance += num12
				End If
				Dim num13 As Single
				If(Me.m_style And FontStyles.Bold) = FontStyles.Bold OrElse (Me.m_fontStyle And FontStyles.Bold) = FontStyles.Bold Then
					num13 = 1F + Me.m_currentFontAsset.boldSpacing * 0.01F
				Else
					num13 = 1F
				End If
				Me.m_internalCharacterInfo(Me.m_characterCount).baseLine = 0F - Me.m_lineOffset + Me.m_baselineOffset
				Dim num14 As Single = Me.m_currentFontAsset.fontInfo.Ascender * If((Me.m_textElementType <> TMP_TextElementType.Character), num, num2) + Me.m_baselineOffset
				Me.m_internalCharacterInfo(Me.m_characterCount).ascender = num14 - Me.m_lineOffset
				Me.m_maxLineAscender = If((num14 <= Me.m_maxLineAscender), Me.m_maxLineAscender, num14)
				Dim num15 As Single = Me.m_currentFontAsset.fontInfo.Descender * If((Me.m_textElementType <> TMP_TextElementType.Character), num, num2) + Me.m_baselineOffset
				Dim internalCharacterInfo As TMP_CharacterInfo() = Me.m_internalCharacterInfo
				Dim characterCount As Integer = Me.m_characterCount
				Dim num16 As Single = num15 - Me.m_lineOffset
				Dim num17 As Single = num16
				internalCharacterInfo(characterCount).descender = num16
				Dim num18 As Single = num17
				Me.m_maxLineDescender = If((num15 >= Me.m_maxLineDescender), Me.m_maxLineDescender, num15)
				If(Me.m_style And FontStyles.Subscript) = FontStyles.Subscript OrElse (Me.m_style And FontStyles.Superscript) = FontStyles.Superscript Then
					Dim num19 As Single = (num14 - Me.m_baselineOffset) / Me.m_currentFontAsset.fontInfo.SubSize
					num14 = Me.m_maxLineAscender
					Me.m_maxLineAscender = If((num19 <= Me.m_maxLineAscender), Me.m_maxLineAscender, num19)
					Dim num20 As Single = (num15 - Me.m_baselineOffset) / Me.m_currentFontAsset.fontInfo.SubSize
					num15 = Me.m_maxLineDescender
					Me.m_maxLineDescender = If((num20 >= Me.m_maxLineDescender), Me.m_maxLineDescender, num20)
				End If
				If Me.m_lineNumber = 0 Then
					Me.m_maxAscender = If((Me.m_maxAscender <= num14), num14, Me.m_maxAscender)
				End If
				If num10 = 9 OrElse Not Char.IsWhiteSpace(CChar(num10)) OrElse Me.m_textElementType = TMP_TextElementType.Sprite Then
					Dim num21 As Single = If((Me.m_width = -1F), (x + 0.0001F - Me.m_marginLeft - Me.m_marginRight), Mathf.Min(x + 0.0001F - Me.m_marginLeft - Me.m_marginRight, Me.m_width))
					If Me.m_xAdvance + Me.m_cached_TextElement.xAdvance * num2 > num21 AndAlso Me.enableWordWrapping AndAlso Me.m_characterCount <> Me.m_firstCharacterOfLine Then
						If num7 = wordWrapState2.previous_WordBreak OrElse flag Then
							If Not Me.m_isCharacterWrappingEnabled Then
								Me.m_isCharacterWrappingEnabled = True
							Else
								flag2 = True
							End If
						End If
						num9 = Me.RestoreWordWrappingState(wordWrapState2)
						num7 = num9
						If Me.m_lineNumber > 0 AndAlso Not TMP_Math.Approximately(Me.m_maxLineAscender, Me.m_startOfLineAscender) AndAlso Me.m_lineHeight = 0F Then
							Dim num22 As Single = Me.m_maxLineAscender - Me.m_startOfLineAscender
							Me.AdjustLineOffset(Me.m_firstCharacterOfLine, Me.m_characterCount, num22)
							Me.m_lineOffset += num22
							wordWrapState2.lineOffset = Me.m_lineOffset
							wordWrapState2.previousLineAscender = Me.m_maxLineAscender
						End If
						Dim num23 As Single = Me.m_maxLineAscender - Me.m_lineOffset
						Dim num24 As Single = Me.m_maxLineDescender - Me.m_lineOffset
						Me.m_maxDescender = If((Me.m_maxDescender >= num24), num24, Me.m_maxDescender)
						Me.m_firstCharacterOfLine = Me.m_characterCount
						num5 += Me.m_xAdvance
						If Me.m_enableWordWrapping Then
							num6 = Me.m_maxAscender - Me.m_maxDescender
						Else
							num6 = Mathf.Max(num6, num23 - num24)
						End If
						Me.SaveWordWrappingState(wordWrapState, num9, Me.m_characterCount - 1)
						Me.m_lineNumber += 1
						If Me.m_lineHeight = 0F Then
							Dim num25 As Single = Me.m_internalCharacterInfo(Me.m_characterCount).ascender - Me.m_internalCharacterInfo(Me.m_characterCount).baseLine
							Dim num26 As Single = 0F - Me.m_maxLineDescender + num25 + (num3 + Me.m_lineSpacing + Me.m_lineSpacingDelta) * num
							Me.m_lineOffset += num26
							Me.m_startOfLineAscender = num25
						Else
							Me.m_lineOffset += Me.m_lineHeight + Me.m_lineSpacing * num
						End If
						Me.m_maxLineAscender = Single.NegativeInfinity
						Me.m_maxLineDescender = Single.PositiveInfinity
						Me.m_xAdvance = Me.tag_Indent
						GoTo IL_0FE9
					End If
				End If
				If Me.m_lineNumber > 0 AndAlso Not TMP_Math.Approximately(Me.m_maxLineAscender, Me.m_startOfLineAscender) AndAlso Me.m_lineHeight = 0F AndAlso Not Me.m_isNewPage Then
					Dim num27 As Single = Me.m_maxLineAscender - Me.m_startOfLineAscender
					Me.AdjustLineOffset(Me.m_firstCharacterOfLine, Me.m_characterCount, num27)
					num18 -= num27
					Me.m_lineOffset += num27
					Me.m_startOfLineAscender += num27
					wordWrapState2.lineOffset = Me.m_lineOffset
					wordWrapState2.previousLineAscender = Me.m_startOfLineAscender
				End If
				If num10 = 9 Then
					Me.m_xAdvance += Me.m_currentFontAsset.fontInfo.TabWidth * num2
				ElseIf Me.m_monoSpacing <> 0F Then
					Me.m_xAdvance += Me.m_monoSpacing - num12 + (Me.m_characterSpacing + Me.m_currentFontAsset.normalSpacingOffset) * num2 + Me.m_cSpacing
				Else
					Me.m_xAdvance += (Me.m_cached_TextElement.xAdvance * num13 + Me.m_characterSpacing + Me.m_currentFontAsset.normalSpacingOffset) * num2 + Me.m_cSpacing
				End If
				If num10 = 13 Then
					num4 = Mathf.Max(num4, num5 + Me.m_xAdvance)
					num5 = 0F
					Me.m_xAdvance = Me.tag_Indent
				End If
				If num10 = 10 OrElse Me.m_characterCount = totalCharacterCount - 1 Then
					If Me.m_lineNumber > 0 AndAlso Not TMP_Math.Approximately(Me.m_maxLineAscender, Me.m_startOfLineAscender) AndAlso Me.m_lineHeight = 0F Then
						Dim num28 As Single = Me.m_maxLineAscender - Me.m_startOfLineAscender
						Me.AdjustLineOffset(Me.m_firstCharacterOfLine, Me.m_characterCount, num28)
						num18 -= num28
						Me.m_lineOffset += num28
					End If
					Dim num29 As Single = Me.m_maxLineDescender - Me.m_lineOffset
					Me.m_maxDescender = If((Me.m_maxDescender >= num29), num29, Me.m_maxDescender)
					Me.m_firstCharacterOfLine = Me.m_characterCount + 1
					If num10 = 10 AndAlso Me.m_characterCount <> totalCharacterCount - 1 Then
						num4 = Mathf.Max(num4, num5 + Me.m_xAdvance)
						num5 = 0F
					Else
						num5 = Mathf.Max(num4, num5 + Me.m_xAdvance)
					End If
					num6 = Me.m_maxAscender - Me.m_maxDescender
					If num10 = 10 Then
						Me.SaveWordWrappingState(wordWrapState, num9, Me.m_characterCount)
						Me.SaveWordWrappingState(wordWrapState2, num9, Me.m_characterCount)
						Me.m_lineNumber += 1
						If Me.m_lineHeight = 0F Then
							Dim num26 As Single = 0F - Me.m_maxLineDescender + num14 + (num3 + Me.m_lineSpacing + Me.m_paragraphSpacing + Me.m_lineSpacingDelta) * num
							Me.m_lineOffset += num26
						Else
							Me.m_lineOffset += Me.m_lineHeight + (Me.m_lineSpacing + Me.m_paragraphSpacing) * num
						End If
						Me.m_maxLineAscender = Single.NegativeInfinity
						Me.m_maxLineDescender = Single.PositiveInfinity
						Me.m_startOfLineAscender = num14
						Me.m_xAdvance = Me.tag_LineIndent + Me.tag_Indent
					End If
				End If
				If Me.m_enableWordWrapping OrElse Me.m_overflowMode = TextOverflowModes.Truncate OrElse Me.m_overflowMode = TextOverflowModes.Ellipsis Then
					If(num10 = 9 OrElse num10 = 32) AndAlso Not Me.m_isNonBreakingSpace Then
						Me.SaveWordWrappingState(wordWrapState2, num9, Me.m_characterCount)
						Me.m_isCharacterWrappingEnabled = False
						flag = False
					ElseIf num10 > 11904 AndAlso num10 < 40959 Then
						If Not Me.m_currentFontAsset.lineBreakingInfo.leadingCharacters.ContainsKey(num10) AndAlso Me.m_characterCount < totalCharacterCount - 1 AndAlso Not Me.m_currentFontAsset.lineBreakingInfo.followingCharacters.ContainsKey(CInt(Me.m_internalCharacterInfo(Me.m_characterCount + 1).character)) Then
							Me.SaveWordWrappingState(wordWrapState2, num9, Me.m_characterCount)
							Me.m_isCharacterWrappingEnabled = False
							flag = False
						End If
					ElseIf flag OrElse Me.m_isCharacterWrappingEnabled OrElse flag2 Then
						Me.SaveWordWrappingState(wordWrapState2, num9, Me.m_characterCount)
					End If
				End If
				Me.m_characterCount += 1
				GoTo IL_0FE9
			End While
			Me.m_isCharacterWrappingEnabled = False
			num5 += If((Me.m_margin.x <= 0F), 0F, Me.m_margin.x)
			num5 += If((Me.m_margin.z <= 0F), 0F, Me.m_margin.z)
			num6 += If((Me.m_margin.y <= 0F), 0F, Me.m_margin.y)
			num6 += If((Me.m_margin.w <= 0F), 0F, Me.m_margin.w)
			Return New Vector2(num5, num6)
		End Function

		' Token: 0x06005105 RID: 20741 RVA: 0x0027ED2A File Offset: 0x0027D12A
		Protected Overridable Sub AdjustLineOffset(startIndex As Integer, endIndex As Integer, offset As Single)
		End Sub

		' Token: 0x06005106 RID: 20742 RVA: 0x0027ED2C File Offset: 0x0027D12C
		Protected Sub ResizeLineExtents(size As Integer)
			size = If((size <= 1024), Mathf.NextPowerOfTwo(size + 1), (size + 256))
			Dim array As TMP_LineInfo() = New TMP_LineInfo(size - 1) {}
			For i As Integer = 0 To size - 1
				If i < Me.m_textInfo.lineInfo.Length Then
					array(i) = Me.m_textInfo.lineInfo(i)
				Else
					array(i).lineExtents.min = TMP_Text.k_InfinityVectorPositive
					array(i).lineExtents.max = TMP_Text.k_InfinityVectorNegative
					array(i).ascender = TMP_Text.k_InfinityVectorNegative.x
					array(i).descender = TMP_Text.k_InfinityVectorPositive.x
				End If
			Next
			Me.m_textInfo.lineInfo = array
		End Sub

		' Token: 0x06005107 RID: 20743 RVA: 0x0027EE11 File Offset: 0x0027D211
		Public Overridable Function GetTextInfo(text As String) As TMP_TextInfo
			Return Nothing
		End Function

		' Token: 0x06005108 RID: 20744 RVA: 0x0027EE14 File Offset: 0x0027D214
		Protected Overridable Sub ComputeMarginSize()
		End Sub

		' Token: 0x06005109 RID: 20745 RVA: 0x0027EE18 File Offset: 0x0027D218
		Protected Function GetArraySizes(chars As Integer()) As Integer
			Dim num As Integer = 0
			Me.m_totalCharacterCount = 0
			Me.m_isUsingBold = False
			Me.m_isParsingText = False
			Dim num2 As Integer = 0
			While chars(num2) <> 0
				Dim num3 As Integer = chars(num2)
				If Me.m_isRichText AndAlso num3 = 60 AndAlso Me.ValidateHtmlTag(chars, num2 + 1, num) Then
					num2 = num
					If(Me.m_style And FontStyles.Bold) = FontStyles.Bold Then
						Me.m_isUsingBold = True
					End If
				Else
					If Not Char.IsWhiteSpace(CChar(num3)) Then
					End If
					Me.m_totalCharacterCount += 1
				End If
				num2 += 1
			End While
			Return Me.m_totalCharacterCount
		End Function

		' Token: 0x0600510A RID: 20746 RVA: 0x0027EEB4 File Offset: 0x0027D2B4
		Protected Sub SaveWordWrappingState(ByRef state As WordWrapState, index As Integer, count As Integer)
			state.currentFontAsset = Me.m_currentFontAsset
			state.currentSpriteAsset = Me.m_currentSpriteAsset
			state.currentMaterial = Me.m_currentMaterial
			state.currentMaterialIndex = Me.m_currentMaterialIndex
			state.previous_WordBreak = index
			state.total_CharacterCount = count
			state.visible_CharacterCount = Me.m_visibleCharacterCount
			state.visible_SpriteCount = Me.m_visibleSpriteCount
			state.visible_LinkCount = Me.m_textInfo.linkCount
			state.firstCharacterIndex = Me.m_firstCharacterOfLine
			state.firstVisibleCharacterIndex = Me.m_firstVisibleCharacterOfLine
			state.lastVisibleCharIndex = Me.m_lastVisibleCharacterOfLine
			state.fontStyle = Me.m_style
			state.fontScale = Me.m_fontScale
			state.fontScaleMultiplier = Me.m_fontScaleMultiplier
			state.currentFontSize = Me.m_currentFontSize
			state.xAdvance = Me.m_xAdvance
			state.maxAscender = Me.m_maxAscender
			state.maxDescender = Me.m_maxDescender
			state.maxLineAscender = Me.m_maxLineAscender
			state.maxLineDescender = Me.m_maxLineDescender
			state.previousLineAscender = Me.m_startOfLineAscender
			state.preferredWidth = Me.m_preferredWidth
			state.preferredHeight = Me.m_preferredHeight
			state.meshExtents = Me.m_meshExtents
			state.lineNumber = Me.m_lineNumber
			state.lineOffset = Me.m_lineOffset
			state.baselineOffset = Me.m_baselineOffset
			state.vertexColor = Me.m_htmlColor
			state.tagNoParsing = Me.tag_NoParsing
			state.colorStack = Me.m_colorStack
			state.sizeStack = Me.m_sizeStack
			state.fontWeightStack = Me.m_fontWeightStack
			state.styleStack = Me.m_styleStack
			state.actionStack = Me.m_actionStack
			state.materialReferenceStack = Me.m_materialReferenceStack
			If Me.m_lineNumber < Me.m_textInfo.lineInfo.Length Then
				state.lineInfo = Me.m_textInfo.lineInfo(Me.m_lineNumber)
			End If
		End Sub

		' Token: 0x0600510B RID: 20747 RVA: 0x0027F0A8 File Offset: 0x0027D4A8
		Protected Function RestoreWordWrappingState(ByRef state As WordWrapState) As Integer
			Dim previous_WordBreak As Integer = state.previous_WordBreak
			Me.m_currentFontAsset = state.currentFontAsset
			Me.m_currentSpriteAsset = state.currentSpriteAsset
			Me.m_currentMaterial = state.currentMaterial
			Me.m_currentMaterialIndex = state.currentMaterialIndex
			Me.m_characterCount = state.total_CharacterCount + 1
			Me.m_visibleCharacterCount = state.visible_CharacterCount
			Me.m_visibleSpriteCount = state.visible_SpriteCount
			Me.m_textInfo.linkCount = state.visible_LinkCount
			Me.m_firstCharacterOfLine = state.firstCharacterIndex
			Me.m_firstVisibleCharacterOfLine = state.firstVisibleCharacterIndex
			Me.m_lastVisibleCharacterOfLine = state.lastVisibleCharIndex
			Me.m_style = state.fontStyle
			Me.m_fontScale = state.fontScale
			Me.m_fontScaleMultiplier = state.fontScaleMultiplier
			Me.m_currentFontSize = state.currentFontSize
			Me.m_xAdvance = state.xAdvance
			Me.m_maxAscender = state.maxAscender
			Me.m_maxDescender = state.maxDescender
			Me.m_maxLineAscender = state.maxLineAscender
			Me.m_maxLineDescender = state.maxLineDescender
			Me.m_startOfLineAscender = state.previousLineAscender
			Me.m_preferredWidth = state.preferredWidth
			Me.m_preferredHeight = state.preferredHeight
			Me.m_meshExtents = state.meshExtents
			Me.m_lineNumber = state.lineNumber
			Me.m_lineOffset = state.lineOffset
			Me.m_baselineOffset = state.baselineOffset
			Me.m_htmlColor = state.vertexColor
			Me.tag_NoParsing = state.tagNoParsing
			Me.m_colorStack = state.colorStack
			Me.m_sizeStack = state.sizeStack
			Me.m_fontWeightStack = state.fontWeightStack
			Me.m_styleStack = state.styleStack
			Me.m_actionStack = state.actionStack
			Me.m_materialReferenceStack = state.materialReferenceStack
			If Me.m_lineNumber < Me.m_textInfo.lineInfo.Length Then
				Me.m_textInfo.lineInfo(Me.m_lineNumber) = state.lineInfo
			End If
			Return previous_WordBreak
		End Function

		' Token: 0x0600510C RID: 20748 RVA: 0x0027F2A4 File Offset: 0x0027D6A4
		Protected Overridable Sub SaveGlyphVertexInfo(padding As Single, style_padding As Single, vertexColor As Color32)
			Me.m_textInfo.characterInfo(Me.m_characterCount).vertex_BL.position = Me.m_textInfo.characterInfo(Me.m_characterCount).bottomLeft
			Me.m_textInfo.characterInfo(Me.m_characterCount).vertex_TL.position = Me.m_textInfo.characterInfo(Me.m_characterCount).topLeft
			Me.m_textInfo.characterInfo(Me.m_characterCount).vertex_TR.position = Me.m_textInfo.characterInfo(Me.m_characterCount).topRight
			Me.m_textInfo.characterInfo(Me.m_characterCount).vertex_BR.position = Me.m_textInfo.characterInfo(Me.m_characterCount).bottomRight
			vertexColor.a = If((Me.m_fontColor32.a >= vertexColor.a), vertexColor.a, Me.m_fontColor32.a)
			If Not Me.m_enableVertexGradient Then
				Me.m_textInfo.characterInfo(Me.m_characterCount).vertex_BL.color = vertexColor
				Me.m_textInfo.characterInfo(Me.m_characterCount).vertex_TL.color = vertexColor
				Me.m_textInfo.characterInfo(Me.m_characterCount).vertex_TR.color = vertexColor
				Me.m_textInfo.characterInfo(Me.m_characterCount).vertex_BR.color = vertexColor
			ElseIf Not Me.m_overrideHtmlColors AndAlso Not Me.m_htmlColor.CompareRGB(Me.m_fontColor32) Then
				Me.m_textInfo.characterInfo(Me.m_characterCount).vertex_BL.color = vertexColor
				Me.m_textInfo.characterInfo(Me.m_characterCount).vertex_TL.color = vertexColor
				Me.m_textInfo.characterInfo(Me.m_characterCount).vertex_TR.color = vertexColor
				Me.m_textInfo.characterInfo(Me.m_characterCount).vertex_BR.color = vertexColor
			Else
				Me.m_textInfo.characterInfo(Me.m_characterCount).vertex_BL.color = Me.m_fontColorGradient.bottomLeft * vertexColor
				Me.m_textInfo.characterInfo(Me.m_characterCount).vertex_TL.color = Me.m_fontColorGradient.topLeft * vertexColor
				Me.m_textInfo.characterInfo(Me.m_characterCount).vertex_TR.color = Me.m_fontColorGradient.topRight * vertexColor
				Me.m_textInfo.characterInfo(Me.m_characterCount).vertex_BR.color = Me.m_fontColorGradient.bottomRight * vertexColor
			End If
			If Not Me.m_isSDFShader Then
				style_padding = 0F
			End If
			Dim fontInfo As FaceInfo = Me.m_currentFontAsset.fontInfo
			Dim vector As Vector2
			vector.x = (Me.m_cached_TextElement.x - padding - style_padding) / fontInfo.AtlasWidth
			vector.y = 1F - (Me.m_cached_TextElement.y + padding + style_padding + Me.m_cached_TextElement.height) / fontInfo.AtlasHeight
			Dim vector2 As Vector2
			vector2.x = vector.x
			vector2.y = 1F - (Me.m_cached_TextElement.y - padding - style_padding) / fontInfo.AtlasHeight
			Dim vector3 As Vector2
			vector3.x = (Me.m_cached_TextElement.x + padding + style_padding + Me.m_cached_TextElement.width) / fontInfo.AtlasWidth
			vector3.y = vector2.y
			Dim vector4 As Vector2
			vector4.x = vector3.x
			vector4.y = vector.y
			Me.m_textInfo.characterInfo(Me.m_characterCount).vertex_BL.uv = vector
			Me.m_textInfo.characterInfo(Me.m_characterCount).vertex_TL.uv = vector2
			Me.m_textInfo.characterInfo(Me.m_characterCount).vertex_TR.uv = vector3
			Me.m_textInfo.characterInfo(Me.m_characterCount).vertex_BR.uv = vector4
		End Sub

		' Token: 0x0600510D RID: 20749 RVA: 0x0027F770 File Offset: 0x0027DB70
		Protected Overridable Sub SaveSpriteVertexInfo(vertexColor As Color32)
			Me.m_textInfo.characterInfo(Me.m_characterCount).vertex_BL.position = Me.m_textInfo.characterInfo(Me.m_characterCount).bottomLeft
			Me.m_textInfo.characterInfo(Me.m_characterCount).vertex_TL.position = Me.m_textInfo.characterInfo(Me.m_characterCount).topLeft
			Me.m_textInfo.characterInfo(Me.m_characterCount).vertex_TR.position = Me.m_textInfo.characterInfo(Me.m_characterCount).topRight
			Me.m_textInfo.characterInfo(Me.m_characterCount).vertex_BR.position = Me.m_textInfo.characterInfo(Me.m_characterCount).bottomRight
			If Me.m_tintAllSprites Then
				Me.m_tintSprite = True
			End If
			Dim color As Color32 = If((Not Me.m_tintSprite), Me.m_spriteColor, Me.m_spriteColor.Multiply(vertexColor))
			Dim b3 As Byte
			If color.a < Me.m_fontColor32.a Then
				Dim b As Byte = If((color.a >= vertexColor.a), vertexColor.a, color.a)
				Dim b2 As Byte = b
				color.a = b
				b3 = b2
			Else
				b3 = Me.m_fontColor32.a
			End If
			color.a = b3
			If Not Me.m_enableVertexGradient Then
				Me.m_textInfo.characterInfo(Me.m_characterCount).vertex_BL.color = color
				Me.m_textInfo.characterInfo(Me.m_characterCount).vertex_TL.color = color
				Me.m_textInfo.characterInfo(Me.m_characterCount).vertex_TR.color = color
				Me.m_textInfo.characterInfo(Me.m_characterCount).vertex_BR.color = color
			ElseIf Not Me.m_overrideHtmlColors AndAlso Not Me.m_htmlColor.CompareRGB(Me.m_fontColor32) Then
				Me.m_textInfo.characterInfo(Me.m_characterCount).vertex_BL.color = color
				Me.m_textInfo.characterInfo(Me.m_characterCount).vertex_TL.color = color
				Me.m_textInfo.characterInfo(Me.m_characterCount).vertex_TR.color = color
				Me.m_textInfo.characterInfo(Me.m_characterCount).vertex_BR.color = color
			Else
				Me.m_textInfo.characterInfo(Me.m_characterCount).vertex_BL.color = If((Not Me.m_tintSprite), color, color.Multiply(Me.m_fontColorGradient.bottomLeft))
				Me.m_textInfo.characterInfo(Me.m_characterCount).vertex_TL.color = If((Not Me.m_tintSprite), color, color.Multiply(Me.m_fontColorGradient.topLeft))
				Me.m_textInfo.characterInfo(Me.m_characterCount).vertex_TR.color = If((Not Me.m_tintSprite), color, color.Multiply(Me.m_fontColorGradient.topRight))
				Me.m_textInfo.characterInfo(Me.m_characterCount).vertex_BR.color = If((Not Me.m_tintSprite), color, color.Multiply(Me.m_fontColorGradient.bottomRight))
			End If
			Dim vector As Vector2 = New Vector2(Me.m_cached_TextElement.x / CSng(Me.m_currentSpriteAsset.spriteSheet.width), Me.m_cached_TextElement.y / CSng(Me.m_currentSpriteAsset.spriteSheet.height))
			Dim vector2 As Vector2 = New Vector2(vector.x, (Me.m_cached_TextElement.y + Me.m_cached_TextElement.height) / CSng(Me.m_currentSpriteAsset.spriteSheet.height))
			Dim vector3 As Vector2 = New Vector2((Me.m_cached_TextElement.x + Me.m_cached_TextElement.width) / CSng(Me.m_currentSpriteAsset.spriteSheet.width), vector2.y)
			Dim vector4 As Vector2 = New Vector2(vector3.x, vector.y)
			Me.m_textInfo.characterInfo(Me.m_characterCount).vertex_BL.uv = vector
			Me.m_textInfo.characterInfo(Me.m_characterCount).vertex_TL.uv = vector2
			Me.m_textInfo.characterInfo(Me.m_characterCount).vertex_TR.uv = vector3
			Me.m_textInfo.characterInfo(Me.m_characterCount).vertex_BR.uv = vector4
		End Sub

		' Token: 0x0600510E RID: 20750 RVA: 0x0027FCA0 File Offset: 0x0027E0A0
		Protected Overridable Sub FillCharacterVertexBuffers(i As Integer, index_X4 As Integer)
			Dim materialReferenceIndex As Integer = Me.m_textInfo.characterInfo(i).materialReferenceIndex
			index_X4 = Me.m_textInfo.meshInfo(materialReferenceIndex).vertexCount
			Dim characterInfo As TMP_CharacterInfo() = Me.m_textInfo.characterInfo
			Me.m_textInfo.characterInfo(i).vertexIndex = CShort(index_X4)
			Me.m_textInfo.meshInfo(materialReferenceIndex).vertices(index_X4) = characterInfo(i).vertex_BL.position
			Me.m_textInfo.meshInfo(materialReferenceIndex).vertices(1 + index_X4) = characterInfo(i).vertex_TL.position
			Me.m_textInfo.meshInfo(materialReferenceIndex).vertices(2 + index_X4) = characterInfo(i).vertex_TR.position
			Me.m_textInfo.meshInfo(materialReferenceIndex).vertices(3 + index_X4) = characterInfo(i).vertex_BR.position
			Me.m_textInfo.meshInfo(materialReferenceIndex).uvs0(index_X4) = characterInfo(i).vertex_BL.uv
			Me.m_textInfo.meshInfo(materialReferenceIndex).uvs0(1 + index_X4) = characterInfo(i).vertex_TL.uv
			Me.m_textInfo.meshInfo(materialReferenceIndex).uvs0(2 + index_X4) = characterInfo(i).vertex_TR.uv
			Me.m_textInfo.meshInfo(materialReferenceIndex).uvs0(3 + index_X4) = characterInfo(i).vertex_BR.uv
			Me.m_textInfo.meshInfo(materialReferenceIndex).uvs2(index_X4) = characterInfo(i).vertex_BL.uv2
			Me.m_textInfo.meshInfo(materialReferenceIndex).uvs2(1 + index_X4) = characterInfo(i).vertex_TL.uv2
			Me.m_textInfo.meshInfo(materialReferenceIndex).uvs2(2 + index_X4) = characterInfo(i).vertex_TR.uv2
			Me.m_textInfo.meshInfo(materialReferenceIndex).uvs2(3 + index_X4) = characterInfo(i).vertex_BR.uv2
			Me.m_textInfo.meshInfo(materialReferenceIndex).colors32(index_X4) = characterInfo(i).vertex_BL.color
			Me.m_textInfo.meshInfo(materialReferenceIndex).colors32(1 + index_X4) = characterInfo(i).vertex_TL.color
			Me.m_textInfo.meshInfo(materialReferenceIndex).colors32(2 + index_X4) = characterInfo(i).vertex_TR.color
			Me.m_textInfo.meshInfo(materialReferenceIndex).colors32(3 + index_X4) = characterInfo(i).vertex_BR.color
			Me.m_textInfo.meshInfo(materialReferenceIndex).vertexCount = index_X4 + 4
		End Sub

		' Token: 0x0600510F RID: 20751 RVA: 0x00280054 File Offset: 0x0027E454
		Protected Overridable Sub FillSpriteVertexBuffers(i As Integer, index_X4 As Integer)
			Dim materialReferenceIndex As Integer = Me.m_textInfo.characterInfo(i).materialReferenceIndex
			index_X4 = Me.m_textInfo.meshInfo(materialReferenceIndex).vertexCount
			Dim characterInfo As TMP_CharacterInfo() = Me.m_textInfo.characterInfo
			Me.m_textInfo.characterInfo(i).vertexIndex = CShort(index_X4)
			Me.m_textInfo.meshInfo(materialReferenceIndex).vertices(index_X4) = characterInfo(i).vertex_BL.position
			Me.m_textInfo.meshInfo(materialReferenceIndex).vertices(1 + index_X4) = characterInfo(i).vertex_TL.position
			Me.m_textInfo.meshInfo(materialReferenceIndex).vertices(2 + index_X4) = characterInfo(i).vertex_TR.position
			Me.m_textInfo.meshInfo(materialReferenceIndex).vertices(3 + index_X4) = characterInfo(i).vertex_BR.position
			Me.m_textInfo.meshInfo(materialReferenceIndex).uvs0(index_X4) = characterInfo(i).vertex_BL.uv
			Me.m_textInfo.meshInfo(materialReferenceIndex).uvs0(1 + index_X4) = characterInfo(i).vertex_TL.uv
			Me.m_textInfo.meshInfo(materialReferenceIndex).uvs0(2 + index_X4) = characterInfo(i).vertex_TR.uv
			Me.m_textInfo.meshInfo(materialReferenceIndex).uvs0(3 + index_X4) = characterInfo(i).vertex_BR.uv
			Me.m_textInfo.meshInfo(materialReferenceIndex).uvs2(index_X4) = characterInfo(i).vertex_BL.uv2
			Me.m_textInfo.meshInfo(materialReferenceIndex).uvs2(1 + index_X4) = characterInfo(i).vertex_TL.uv2
			Me.m_textInfo.meshInfo(materialReferenceIndex).uvs2(2 + index_X4) = characterInfo(i).vertex_TR.uv2
			Me.m_textInfo.meshInfo(materialReferenceIndex).uvs2(3 + index_X4) = characterInfo(i).vertex_BR.uv2
			Me.m_textInfo.meshInfo(materialReferenceIndex).colors32(index_X4) = characterInfo(i).vertex_BL.color
			Me.m_textInfo.meshInfo(materialReferenceIndex).colors32(1 + index_X4) = characterInfo(i).vertex_TL.color
			Me.m_textInfo.meshInfo(materialReferenceIndex).colors32(2 + index_X4) = characterInfo(i).vertex_TR.color
			Me.m_textInfo.meshInfo(materialReferenceIndex).colors32(3 + index_X4) = characterInfo(i).vertex_BR.color
			Me.m_textInfo.meshInfo(materialReferenceIndex).vertexCount = index_X4 + 4
		End Sub

		' Token: 0x06005110 RID: 20752 RVA: 0x00280408 File Offset: 0x0027E808
		Protected Overridable Sub DrawUnderlineMesh(start As Vector3, [end] As Vector3, ByRef index As Integer, startScale As Single, endScale As Single, maxScale As Single, underlineColor As Color32)
			If Me.m_cached_Underline_GlyphInfo Is Nothing Then
				If Not TMP_Settings.warningsDisabled Then
				End If
				Return
			End If
			Dim num As Integer = index + 12
			If num > Me.m_textInfo.meshInfo(0).vertices.Length Then
				Me.m_textInfo.meshInfo(0).ResizeMeshInfo(num / 4)
			End If
			start.y = Mathf.Min(start.y, [end].y)
			[end].y = Mathf.Min(start.y, [end].y)
			Dim num2 As Single = Me.m_cached_Underline_GlyphInfo.width / 2F * maxScale
			If [end].x - start.x < Me.m_cached_Underline_GlyphInfo.width * maxScale Then
				num2 = ([end].x - start.x) / 2F
			End If
			Dim num3 As Single = Me.m_padding * startScale / maxScale
			Dim num4 As Single = Me.m_padding * endScale / maxScale
			Dim height As Single = Me.m_cached_Underline_GlyphInfo.height
			Dim vertices As Vector3() = Me.m_textInfo.meshInfo(0).vertices
			vertices(index) = start + New Vector3(0F, 0F - (height + Me.m_padding) * maxScale, 0F)
			vertices(index + 1) = start + New Vector3(0F, Me.m_padding * maxScale, 0F)
			vertices(index + 2) = vertices(index + 1) + New Vector3(num2, 0F, 0F)
			vertices(index + 3) = vertices(index) + New Vector3(num2, 0F, 0F)
			vertices(index + 4) = vertices(index + 3)
			vertices(index + 5) = vertices(index + 2)
			vertices(index + 6) = [end] + New Vector3(-num2, Me.m_padding * maxScale, 0F)
			vertices(index + 7) = [end] + New Vector3(-num2, -(height + Me.m_padding) * maxScale, 0F)
			vertices(index + 8) = vertices(index + 7)
			vertices(index + 9) = vertices(index + 6)
			vertices(index + 10) = [end] + New Vector3(0F, Me.m_padding * maxScale, 0F)
			vertices(index + 11) = [end] + New Vector3(0F, -(height + Me.m_padding) * maxScale, 0F)
			Dim uvs As Vector2() = Me.m_textInfo.meshInfo(0).uvs0
			Dim vector As Vector2 = New Vector2((Me.m_cached_Underline_GlyphInfo.x - num3) / Me.m_fontAsset.fontInfo.AtlasWidth, 1F - (Me.m_cached_Underline_GlyphInfo.y + Me.m_padding + Me.m_cached_Underline_GlyphInfo.height) / Me.m_fontAsset.fontInfo.AtlasHeight)
			Dim vector2 As Vector2 = New Vector2(vector.x, 1F - (Me.m_cached_Underline_GlyphInfo.y - Me.m_padding) / Me.m_fontAsset.fontInfo.AtlasHeight)
			Dim vector3 As Vector2 = New Vector2((Me.m_cached_Underline_GlyphInfo.x - num3 + Me.m_cached_Underline_GlyphInfo.width / 2F) / Me.m_fontAsset.fontInfo.AtlasWidth, vector2.y)
			Dim vector4 As Vector2 = New Vector2(vector3.x, vector.y)
			Dim vector5 As Vector2 = New Vector2((Me.m_cached_Underline_GlyphInfo.x + num4 + Me.m_cached_Underline_GlyphInfo.width / 2F) / Me.m_fontAsset.fontInfo.AtlasWidth, vector2.y)
			Dim vector6 As Vector2 = New Vector2(vector5.x, vector.y)
			Dim vector7 As Vector2 = New Vector2((Me.m_cached_Underline_GlyphInfo.x + num4 + Me.m_cached_Underline_GlyphInfo.width) / Me.m_fontAsset.fontInfo.AtlasWidth, vector2.y)
			Dim vector8 As Vector2 = New Vector2(vector7.x, vector.y)
			uvs(index) = vector
			uvs(1 + index) = vector2
			uvs(2 + index) = vector3
			uvs(3 + index) = vector4
			uvs(4 + index) = New Vector2(vector3.x - vector3.x * 0.001F, vector.y)
			uvs(5 + index) = New Vector2(vector3.x - vector3.x * 0.001F, vector2.y)
			uvs(6 + index) = New Vector2(vector3.x + vector3.x * 0.001F, vector2.y)
			uvs(7 + index) = New Vector2(vector3.x + vector3.x * 0.001F, vector.y)
			uvs(8 + index) = vector6
			uvs(9 + index) = vector5
			uvs(10 + index) = vector7
			uvs(11 + index) = vector8
			Dim num5 As Single = (vertices(index + 2).x - start.x) / ([end].x - start.x)
			Dim num6 As Single = If((maxScale * Me.m_rectTransform.lossyScale.y <> 0F), Me.m_rectTransform.lossyScale.y, 1F)
			Dim num7 As Single = num6
			Dim uvs2 As Vector2() = Me.m_textInfo.meshInfo(0).uvs2
			uvs2(index) = Me.PackUV(0F, 0F, num6)
			uvs2(1 + index) = Me.PackUV(0F, 1F, num6)
			uvs2(2 + index) = Me.PackUV(num5, 1F, num6)
			uvs2(3 + index) = Me.PackUV(num5, 0F, num6)
			Dim num8 As Single = (vertices(index + 4).x - start.x) / ([end].x - start.x)
			num5 = (vertices(index + 6).x - start.x) / ([end].x - start.x)
			uvs2(4 + index) = Me.PackUV(num8, 0F, num7)
			uvs2(5 + index) = Me.PackUV(num8, 1F, num7)
			uvs2(6 + index) = Me.PackUV(num5, 1F, num7)
			uvs2(7 + index) = Me.PackUV(num5, 0F, num7)
			num8 = (vertices(index + 8).x - start.x) / ([end].x - start.x)
			num5 = (vertices(index + 6).x - start.x) / ([end].x - start.x)
			uvs2(8 + index) = Me.PackUV(num8, 0F, num6)
			uvs2(9 + index) = Me.PackUV(num8, 1F, num6)
			uvs2(10 + index) = Me.PackUV(1F, 1F, num6)
			uvs2(11 + index) = Me.PackUV(1F, 0F, num6)
			Dim colors As Color32() = Me.m_textInfo.meshInfo(0).colors32
			colors(index) = underlineColor
			colors(1 + index) = underlineColor
			colors(2 + index) = underlineColor
			colors(3 + index) = underlineColor
			colors(4 + index) = underlineColor
			colors(5 + index) = underlineColor
			colors(6 + index) = underlineColor
			colors(7 + index) = underlineColor
			colors(8 + index) = underlineColor
			colors(9 + index) = underlineColor
			colors(10 + index) = underlineColor
			colors(11 + index) = underlineColor
			index += 12
		End Sub

		' Token: 0x06005111 RID: 20753 RVA: 0x00280DD0 File Offset: 0x0027F1D0
		Protected Sub GetSpecialCharacters(fontAsset As TMP_FontAsset)
			If fontAsset.characterDictionary.TryGetValue(95, Me.m_cached_Underline_GlyphInfo) OrElse Not TMP_Settings.warningsDisabled Then
			End If
			If fontAsset.characterDictionary.TryGetValue(8230, Me.m_cached_Ellipsis_GlyphInfo) OrElse Not TMP_Settings.warningsDisabled Then
			End If
		End Sub

		' Token: 0x06005112 RID: 20754 RVA: 0x00280E24 File Offset: 0x0027F224
		Protected Function GetMaterialReferenceForFontWeight() As Integer
			Me.m_currentMaterialIndex = MaterialReference.AddMaterialReference(Me.m_currentFontAsset.fontWeights(0).italicTypeface.material, Me.m_currentFontAsset.fontWeights(0).italicTypeface, Me.m_materialReferences, Me.m_materialReferenceIndexLookup)
			Return 0
		End Function

		' Token: 0x06005113 RID: 20755 RVA: 0x00280E74 File Offset: 0x0027F274
		Protected Function GetAlternativeFontAsset() As TMP_FontAsset
			Dim flag As Boolean = (Me.m_style And FontStyles.Italic) = FontStyles.Italic OrElse (Me.m_fontStyle And FontStyles.Italic) = FontStyles.Italic
			Dim num As Integer = Me.m_fontWeightInternal / 100
			Dim tmp_FontAsset As TMP_FontAsset
			If flag Then
				tmp_FontAsset = Me.m_currentFontAsset.fontWeights(num).italicTypeface
			Else
				tmp_FontAsset = Me.m_currentFontAsset.fontWeights(num).regularTypeface
			End If
			If tmp_FontAsset Is Nothing Then
				Return Me.m_currentFontAsset
			End If
			Me.m_currentFontAsset = tmp_FontAsset
			Return Me.m_currentFontAsset
		End Function

		' Token: 0x06005114 RID: 20756 RVA: 0x00280EFC File Offset: 0x0027F2FC
		Protected Function PackUV(x As Single, y As Single, scale As Single) As Vector2
			Dim vector As Vector2
			vector.x = Mathf.Floor(x * 511F)
			vector.y = Mathf.Floor(y * 511F)
			vector.x = vector.x * 4096F + vector.y
			vector.y = scale
			Return vector
		End Function

		' Token: 0x06005115 RID: 20757 RVA: 0x00280F54 File Offset: 0x0027F354
		Protected Function PackUV(x As Single, y As Single) As Single
			Dim num As Double = Math.Floor(CDbl((x * 511F)))
			Dim num2 As Double = Math.Floor(CDbl((y * 511F)))
			Return CSng((num * 4096.0 + num2))
		End Function

		' Token: 0x06005116 RID: 20758 RVA: 0x00280F8C File Offset: 0x0027F38C
		Protected Function HexToInt(hex As Char) As Integer
			Select Case hex
				Case "0"c
					Return 0
				Case "1"c
					Return 1
				Case "2"c
					Return 2
				Case "3"c
					Return 3
				Case "4"c
					Return 4
				Case "5"c
					Return 5
				Case "6"c
					Return 6
				Case "7"c
					Return 7
				Case "8"c
					Return 8
				Case "9"c
					Return 9
				Case Else
					Select Case hex
						Case "a"c
							Return 10
						Case "b"c
							Return 11
						Case "c"c
							Return 12
						Case "d"c
							Return 13
						Case "e"c
							Return 14
						Case "f"c
							Return 15
						Case Else
							Return 15
					End Select
				Case "A"c
					Return 10
				Case "B"c
					Return 11
				Case "C"c
					Return 12
				Case "D"c
					Return 13
				Case "E"c
					Return 14
				Case "F"c
					Return 15
			End Select
		End Function

		' Token: 0x06005117 RID: 20759 RVA: 0x00281060 File Offset: 0x0027F460
		Protected Function GetUTF16(i As Integer) As Integer
			Dim num As Integer = Me.HexToInt(Me.m_text(i)) * 4096
			num += Me.HexToInt(Me.m_text(i + 1)) * 256
			num += Me.HexToInt(Me.m_text(i + 2)) * 16
			Return num + Me.HexToInt(Me.m_text(i + 3))
		End Function

		' Token: 0x06005118 RID: 20760 RVA: 0x002810D8 File Offset: 0x0027F4D8
		Protected Function GetUTF32(i As Integer) As Integer
			Dim num As Integer = 0
			num += Me.HexToInt(Me.m_text(i)) * 268435456
			num += Me.HexToInt(Me.m_text(i + 1)) * 16777216
			num += Me.HexToInt(Me.m_text(i + 2)) * 1048576
			num += Me.HexToInt(Me.m_text(i + 3)) * 65536
			num += Me.HexToInt(Me.m_text(i + 4)) * 4096
			num += Me.HexToInt(Me.m_text(i + 5)) * 256
			num += Me.HexToInt(Me.m_text(i + 6)) * 16
			Return num + Me.HexToInt(Me.m_text(i + 7))
		End Function

		' Token: 0x06005119 RID: 20761 RVA: 0x002811C8 File Offset: 0x0027F5C8
		Protected Function HexCharsToColor(hexChars As Char(), tagCount As Integer) As Color32
			If tagCount = 7 Then
				Dim b As Byte = CByte((Me.HexToInt(hexChars(1)) * 16 + Me.HexToInt(hexChars(2))))
				Dim b2 As Byte = CByte((Me.HexToInt(hexChars(3)) * 16 + Me.HexToInt(hexChars(4))))
				Dim b3 As Byte = CByte((Me.HexToInt(hexChars(5)) * 16 + Me.HexToInt(hexChars(6))))
				Return New Color32(b, b2, b3, Byte.MaxValue)
			End If
			If tagCount = 9 Then
				Dim b4 As Byte = CByte((Me.HexToInt(hexChars(1)) * 16 + Me.HexToInt(hexChars(2))))
				Dim b5 As Byte = CByte((Me.HexToInt(hexChars(3)) * 16 + Me.HexToInt(hexChars(4))))
				Dim b6 As Byte = CByte((Me.HexToInt(hexChars(5)) * 16 + Me.HexToInt(hexChars(6))))
				Dim b7 As Byte = CByte((Me.HexToInt(hexChars(7)) * 16 + Me.HexToInt(hexChars(8))))
				Return New Color32(b4, b5, b6, b7)
			End If
			If tagCount = 13 Then
				Dim b8 As Byte = CByte((Me.HexToInt(hexChars(7)) * 16 + Me.HexToInt(hexChars(8))))
				Dim b9 As Byte = CByte((Me.HexToInt(hexChars(9)) * 16 + Me.HexToInt(hexChars(10))))
				Dim b10 As Byte = CByte((Me.HexToInt(hexChars(11)) * 16 + Me.HexToInt(hexChars(12))))
				Return New Color32(b8, b9, b10, Byte.MaxValue)
			End If
			If tagCount = 15 Then
				Dim b11 As Byte = CByte((Me.HexToInt(hexChars(7)) * 16 + Me.HexToInt(hexChars(8))))
				Dim b12 As Byte = CByte((Me.HexToInt(hexChars(9)) * 16 + Me.HexToInt(hexChars(10))))
				Dim b13 As Byte = CByte((Me.HexToInt(hexChars(11)) * 16 + Me.HexToInt(hexChars(12))))
				Dim b14 As Byte = CByte((Me.HexToInt(hexChars(13)) * 16 + Me.HexToInt(hexChars(14))))
				Return New Color32(b11, b12, b13, b14)
			End If
			Return New Color32(Byte.MaxValue, Byte.MaxValue, Byte.MaxValue, Byte.MaxValue)
		End Function

		' Token: 0x0600511A RID: 20762 RVA: 0x002813AC File Offset: 0x0027F7AC
		Protected Function HexCharsToColor(hexChars As Char(), startIndex As Integer, length As Integer) As Color32
			If length = 7 Then
				Dim b As Byte = CByte((Me.HexToInt(hexChars(startIndex + 1)) * 16 + Me.HexToInt(hexChars(startIndex + 2))))
				Dim b2 As Byte = CByte((Me.HexToInt(hexChars(startIndex + 3)) * 16 + Me.HexToInt(hexChars(startIndex + 4))))
				Dim b3 As Byte = CByte((Me.HexToInt(hexChars(startIndex + 5)) * 16 + Me.HexToInt(hexChars(startIndex + 6))))
				Return New Color32(b, b2, b3, Byte.MaxValue)
			End If
			If length = 9 Then
				Dim b4 As Byte = CByte((Me.HexToInt(hexChars(startIndex + 1)) * 16 + Me.HexToInt(hexChars(startIndex + 2))))
				Dim b5 As Byte = CByte((Me.HexToInt(hexChars(startIndex + 3)) * 16 + Me.HexToInt(hexChars(startIndex + 4))))
				Dim b6 As Byte = CByte((Me.HexToInt(hexChars(startIndex + 5)) * 16 + Me.HexToInt(hexChars(startIndex + 6))))
				Dim b7 As Byte = CByte((Me.HexToInt(hexChars(startIndex + 7)) * 16 + Me.HexToInt(hexChars(startIndex + 8))))
				Return New Color32(b4, b5, b6, b7)
			End If
			Return New Color32(Byte.MaxValue, Byte.MaxValue, Byte.MaxValue, Byte.MaxValue)
		End Function

		' Token: 0x0600511B RID: 20763 RVA: 0x002814C4 File Offset: 0x0027F8C4
		Protected Function ConvertToFloat(chars As Char(), startIndex As Integer, length As Integer, decimalPointIndex As Integer) As Single
			If startIndex = 0 Then
				Return -9999F
			End If
			Dim num As Integer = startIndex + length - 1
			Dim num2 As Single = 0F
			Dim num3 As Single = 1F
			decimalPointIndex = If((decimalPointIndex <= 0), (num + 1), decimalPointIndex)
			If chars(startIndex) = "-"c Then
				startIndex += 1
				num3 = -1F
			End If
			If chars(startIndex) = "+"c OrElse chars(startIndex) = "%"c Then
				startIndex += 1
			End If
			For i As Integer = startIndex To num + 1 - 1
				If Not Char.IsDigit(chars(i)) AndAlso chars(i) <> "."c Then
					Return -9999F
				End If
				Dim num4 As Integer = decimalPointIndex - i
				Select Case num4 + 3
					Case 0
						num2 += CSng((chars(i) - "0"c)) * 0.001F
					Case 1
						num2 += CSng((chars(i) - "0"c)) * 0.01F
					Case 2
						num2 += CSng((chars(i) - "0"c)) * 0.1F
					Case 4
						num2 += CSng((chars(i) - "0"c))
					Case 5
						num2 += CSng(((chars(i) - "0"c) * vbLf))
					Case 6
						num2 += CSng(((chars(i) - "0"c) * "d"c))
					Case 7
						num2 += CSng(((chars(i) - "0"c) * "Ϩ"c))
				End Select
			Next
			Return num2 * num3
		End Function

		' Token: 0x0600511C RID: 20764 RVA: 0x00281620 File Offset: 0x0027FA20
		Protected Function ValidateHtmlTag(chars As Integer(), startIndex As Integer, <System.Runtime.InteropServices.OutAttribute()> ByRef endIndex As Integer) As Boolean
			Dim num As Integer = 0
			Dim b As Byte = 0
			Dim tagUnits As TagUnits = TagUnits.Pixels
			Dim tagType As TagType = TagType.None
			Dim num2 As Integer = 0
			Me.m_xmlAttribute(num2).nameHashCode = 0
			Me.m_xmlAttribute(num2).valueType = TagType.None
			Me.m_xmlAttribute(num2).valueHashCode = 0
			Me.m_xmlAttribute(num2).valueStartIndex = 0
			Me.m_xmlAttribute(num2).valueLength = 0
			Me.m_xmlAttribute(num2).valueDecimalIndex = 0
			endIndex = startIndex
			Dim flag As Boolean = False
			Dim flag2 As Boolean = False
			Dim num3 As Integer = startIndex
			While num3 < chars.Length AndAlso chars(num3) <> 0 AndAlso num < Me.m_htmlTag.Length AndAlso chars(num3) <> 60
				If chars(num3) = 62 Then
					flag2 = True
					endIndex = num3
					Me.m_htmlTag(num) = vbNullChar
					Exit While
				End If
				Me.m_htmlTag(num) = CChar(chars(num3))
				num += 1
				If b = 1 Then
					If Me.m_xmlAttribute(num2).valueStartIndex = 0 Then
						If chars(num3) = 43 OrElse chars(num3) = 45 OrElse Char.IsDigit(CChar(chars(num3))) Then
							tagType = TagType.NumericalValue
							Me.m_xmlAttribute(num2).valueType = TagType.NumericalValue
							Me.m_xmlAttribute(num2).valueStartIndex = num - 1
							Dim xmlAttribute As XML_TagAttribute() = Me.m_xmlAttribute
							Dim num4 As Integer = num2
							xmlAttribute(num4).valueLength = xmlAttribute(num4).valueLength + 1
						ElseIf chars(num3) = 35 Then
							tagType = TagType.ColorValue
							Me.m_xmlAttribute(num2).valueType = TagType.ColorValue
							Me.m_xmlAttribute(num2).valueStartIndex = num - 1
							Dim xmlAttribute2 As XML_TagAttribute() = Me.m_xmlAttribute
							Dim num5 As Integer = num2
							xmlAttribute2(num5).valueLength = xmlAttribute2(num5).valueLength + 1
						ElseIf chars(num3) <> 34 Then
							tagType = TagType.StringValue
							Me.m_xmlAttribute(num2).valueType = TagType.StringValue
							Me.m_xmlAttribute(num2).valueStartIndex = num - 1
							Me.m_xmlAttribute(num2).valueHashCode = ((Me.m_xmlAttribute(num2).valueHashCode << 5) + Me.m_xmlAttribute(num2).valueHashCode) Xor chars(num3)
							Dim xmlAttribute3 As XML_TagAttribute() = Me.m_xmlAttribute
							Dim num6 As Integer = num2
							xmlAttribute3(num6).valueLength = xmlAttribute3(num6).valueLength + 1
						End If
					ElseIf tagType = TagType.NumericalValue Then
						If chars(num3) = 46 Then
							Me.m_xmlAttribute(num2).valueDecimalIndex = num - 1
						End If
						If chars(num3) = 112 OrElse chars(num3) = 101 OrElse chars(num3) = 37 OrElse chars(num3) = 32 Then
							b = 2
							tagType = TagType.None
							num2 += 1
							Me.m_xmlAttribute(num2).nameHashCode = 0
							Me.m_xmlAttribute(num2).valueType = TagType.None
							Me.m_xmlAttribute(num2).valueHashCode = 0
							Me.m_xmlAttribute(num2).valueStartIndex = 0
							Me.m_xmlAttribute(num2).valueLength = 0
							Me.m_xmlAttribute(num2).valueDecimalIndex = 0
							If chars(num3) = 101 Then
								tagUnits = TagUnits.FontUnits
							ElseIf chars(num3) = 37 Then
								tagUnits = TagUnits.Percentage
							End If
						ElseIf b <> 2 Then
							Dim xmlAttribute4 As XML_TagAttribute() = Me.m_xmlAttribute
							Dim num7 As Integer = num2
							xmlAttribute4(num7).valueLength = xmlAttribute4(num7).valueLength + 1
						End If
					ElseIf tagType = TagType.ColorValue Then
						If chars(num3) <> 32 Then
							Dim xmlAttribute5 As XML_TagAttribute() = Me.m_xmlAttribute
							Dim num8 As Integer = num2
							xmlAttribute5(num8).valueLength = xmlAttribute5(num8).valueLength + 1
						Else
							b = 2
							tagType = TagType.None
							num2 += 1
							Me.m_xmlAttribute(num2).nameHashCode = 0
							Me.m_xmlAttribute(num2).valueType = TagType.None
							Me.m_xmlAttribute(num2).valueHashCode = 0
							Me.m_xmlAttribute(num2).valueStartIndex = 0
							Me.m_xmlAttribute(num2).valueLength = 0
							Me.m_xmlAttribute(num2).valueDecimalIndex = 0
						End If
					ElseIf tagType = TagType.StringValue Then
						If chars(num3) <> 34 Then
							Me.m_xmlAttribute(num2).valueHashCode = ((Me.m_xmlAttribute(num2).valueHashCode << 5) + Me.m_xmlAttribute(num2).valueHashCode) Xor chars(num3)
							Dim xmlAttribute6 As XML_TagAttribute() = Me.m_xmlAttribute
							Dim num9 As Integer = num2
							xmlAttribute6(num9).valueLength = xmlAttribute6(num9).valueLength + 1
						Else
							b = 2
							tagType = TagType.None
							num2 += 1
							Me.m_xmlAttribute(num2).nameHashCode = 0
							Me.m_xmlAttribute(num2).valueType = TagType.None
							Me.m_xmlAttribute(num2).valueHashCode = 0
							Me.m_xmlAttribute(num2).valueStartIndex = 0
							Me.m_xmlAttribute(num2).valueLength = 0
							Me.m_xmlAttribute(num2).valueDecimalIndex = 0
						End If
					End If
				End If
				If chars(num3) = 61 Then
					b = 1
				End If
				If b = 0 AndAlso chars(num3) = 32 Then
					If flag Then
						Return False
					End If
					flag = True
					b = 2
					tagType = TagType.None
					num2 += 1
					Me.m_xmlAttribute(num2).nameHashCode = 0
					Me.m_xmlAttribute(num2).valueType = TagType.None
					Me.m_xmlAttribute(num2).valueHashCode = 0
					Me.m_xmlAttribute(num2).valueStartIndex = 0
					Me.m_xmlAttribute(num2).valueLength = 0
					Me.m_xmlAttribute(num2).valueDecimalIndex = 0
				End If
				If b = 0 Then
					Me.m_xmlAttribute(num2).nameHashCode = (Me.m_xmlAttribute(num2).nameHashCode << 3) - Me.m_xmlAttribute(num2).nameHashCode + chars(num3)
				End If
				If b = 2 AndAlso chars(num3) = 32 Then
					b = 0
				End If
				num3 += 1
			End While
			If Not flag2 Then
				Return False
			End If
			If Me.tag_NoParsing AndAlso Me.m_xmlAttribute(0).nameHashCode <> 53822163 Then
				Return False
			End If
			If Me.m_xmlAttribute(0).nameHashCode = 53822163 Then
				Me.tag_NoParsing = False
				Return True
			End If
			If Me.m_htmlTag(0) = "#"c AndAlso num = 7 Then
				Me.m_htmlColor = Me.HexCharsToColor(Me.m_htmlTag, num)
				Me.m_colorStack.Add(Me.m_htmlColor)
				Return True
			End If
			If Me.m_htmlTag(0) = "#"c AndAlso num = 9 Then
				Me.m_htmlColor = Me.HexCharsToColor(Me.m_htmlTag, num)
				Me.m_colorStack.Add(Me.m_htmlColor)
				Return True
			End If
			Dim nameHashCode As Integer = Me.m_xmlAttribute(0).nameHashCode
			Select Case nameHashCode
				Case 115
					Me.m_style = Me.m_style Or FontStyles.Strikethrough
					Return True
				Case Else
					If nameHashCode = 426 Then
						Return True
					End If
					If nameHashCode = 427 Then
						If(Me.m_fontStyle And FontStyles.Bold) <> FontStyles.Bold Then
							Me.m_style = Me.m_style And CType((-2), FontStyles)
							Me.m_fontWeightInternal = Me.m_fontWeightStack.Remove()
						End If
						Return True
					End If
					Select Case nameHashCode
						Case 444
							If(Me.m_fontStyle And FontStyles.Strikethrough) <> FontStyles.Strikethrough Then
								Me.m_style = Me.m_style And CType((-65), FontStyles)
							End If
							Return True
						Case Else
							If nameHashCode <> 13526026 Then
								If nameHashCode = 730022849 Then
									Me.m_style = Me.m_style Or FontStyles.LowerCase
									Return True
								End If
								If nameHashCode = 766244328 Then
									Me.m_style = Me.m_style Or FontStyles.SmallCaps
									Return True
								End If
								If nameHashCode <> 781906058 Then
									If nameHashCode <> 1100728678 Then
										If nameHashCode <> 1109349752 Then
											If nameHashCode <> 1109386397 Then
												If nameHashCode = -1885698441 Then
													Me.m_fontWeightInternal = Me.m_fontWeightStack.Remove()
													Return True
												End If
												If nameHashCode = -1668324918 Then
													Me.m_style = Me.m_style And CType((-9), FontStyles)
													Return True
												End If
												If nameHashCode <> -1632103439 Then
													If nameHashCode <> -1616441709 Then
														If nameHashCode <> -884817987 Then
															If nameHashCode = -445573839 Then
																Me.m_lineHeight = 0F
																Return True
															End If
															If nameHashCode = -445537194 Then
																Me.tag_LineIndent = 0F
																Return True
															End If
															If nameHashCode <> -330774850 Then
																If nameHashCode = 98 Then
																	Me.m_style = Me.m_style Or FontStyles.Bold
																	Me.m_fontWeightInternal = 700
																	Me.m_fontWeightStack.Add(700)
																	Return True
																End If
																If nameHashCode = 105 Then
																	Me.m_style = Me.m_style Or FontStyles.Italic
																	Return True
																End If
																If nameHashCode = 434 Then
																	Me.m_style = Me.m_style And CType((-3), FontStyles)
																	Return True
																End If
																If nameHashCode <> 6380 Then
																	If nameHashCode = 6552 Then
																		Me.m_fontScaleMultiplier = If((Me.m_currentFontAsset.fontInfo.SubSize <= 0F), 1F, Me.m_currentFontAsset.fontInfo.SubSize)
																		Me.m_baselineOffset = Me.m_currentFontAsset.fontInfo.SubscriptOffset * Me.m_fontScale * Me.m_fontScaleMultiplier
																		Me.m_style = Me.m_style Or FontStyles.Subscript
																		Return True
																	End If
																	If nameHashCode = 6566 Then
																		Me.m_fontScaleMultiplier = If((Me.m_currentFontAsset.fontInfo.SubSize <= 0F), 1F, Me.m_currentFontAsset.fontInfo.SubSize)
																		Me.m_baselineOffset = Me.m_currentFontAsset.fontInfo.SuperscriptOffset * Me.m_fontScale * Me.m_fontScaleMultiplier
																		Me.m_style = Me.m_style Or FontStyles.Superscript
																		Return True
																	End If
																	If nameHashCode = 22501 Then
																		Me.m_isIgnoringAlignment = False
																		Return True
																	End If
																	If nameHashCode = 22673 Then
																		If(Me.m_style And FontStyles.Subscript) = FontStyles.Subscript Then
																			If(Me.m_style And FontStyles.Superscript) = FontStyles.Superscript Then
																				Me.m_fontScaleMultiplier = If((Me.m_currentFontAsset.fontInfo.SubSize <= 0F), 1F, Me.m_currentFontAsset.fontInfo.SubSize)
																				Me.m_baselineOffset = Me.m_currentFontAsset.fontInfo.SuperscriptOffset * Me.m_fontScale * Me.m_fontScaleMultiplier
																			Else
																				Me.m_baselineOffset = 0F
																				Me.m_fontScaleMultiplier = 1F
																			End If
																			Me.m_style = Me.m_style And CType((-257), FontStyles)
																		End If
																		Return True
																	End If
																	If nameHashCode = 22687 Then
																		If(Me.m_style And FontStyles.Superscript) = FontStyles.Superscript Then
																			If(Me.m_style And FontStyles.Subscript) = FontStyles.Subscript Then
																				Me.m_fontScaleMultiplier = If((Me.m_currentFontAsset.fontInfo.SubSize <= 0F), 1F, Me.m_currentFontAsset.fontInfo.SubSize)
																				Me.m_baselineOffset = Me.m_currentFontAsset.fontInfo.SubscriptOffset * Me.m_fontScale * Me.m_fontScaleMultiplier
																			Else
																				Me.m_baselineOffset = 0F
																				Me.m_fontScaleMultiplier = 1F
																			End If
																			Me.m_style = Me.m_style And CType((-129), FontStyles)
																		End If
																		Return True
																	End If
																	If nameHashCode <> 41311 Then
																		If nameHashCode = 43066 Then
																			If Me.m_isParsingText Then
																				Dim num10 As Integer = Me.m_textInfo.linkInfo.Length
																				If Me.m_textInfo.linkCount + 1 > num10 Then
																					TMP_TextInfo.Resize(Of TMP_LinkInfo)(Me.m_textInfo.linkInfo, num10 + 1)
																				End If
																				Dim linkCount As Integer = Me.m_textInfo.linkCount
																				Me.m_textInfo.linkInfo(linkCount).textComponent = Me
																				Me.m_textInfo.linkInfo(linkCount).hashCode = Me.m_xmlAttribute(0).valueHashCode
																				Me.m_textInfo.linkInfo(linkCount).linkTextfirstCharacterIndex = Me.m_characterCount
																				Me.m_textInfo.linkInfo(linkCount).linkIdFirstCharacterIndex = startIndex + Me.m_xmlAttribute(0).valueStartIndex
																				Me.m_textInfo.linkInfo(linkCount).linkIdLength = Me.m_xmlAttribute(0).valueLength
																				Me.m_textInfo.linkInfo(linkCount).SetLinkID(Me.m_htmlTag, Me.m_xmlAttribute(0).valueStartIndex, Me.m_xmlAttribute(0).valueLength)
																			End If
																			Return True
																		End If
																		If nameHashCode = 43969 Then
																			Me.m_isNonBreakingSpace = True
																			Return True
																		End If
																		If nameHashCode = 43991 Then
																			If Me.m_overflowMode = TextOverflowModes.Page Then
																				Me.m_xAdvance = Me.tag_LineIndent + Me.tag_Indent
																				Me.m_lineOffset = 0F
																				Me.m_pageNumber += 1
																				Me.m_isNewPage = True
																			End If
																			Return True
																		End If
																		If nameHashCode <> 45545 Then
																			If nameHashCode = 154158 Then
																				Dim materialReference As MaterialReference = Me.m_materialReferenceStack.Remove()
																				Me.m_currentFontAsset = materialReference.fontAsset
																				Me.m_currentMaterial = materialReference.material
																				Me.m_currentMaterialIndex = materialReference.index
																				Me.m_fontScale = Me.m_currentFontSize / Me.m_currentFontAsset.fontInfo.PointSize * Me.m_currentFontAsset.fontInfo.Scale * If((Not Me.m_isOrthographic), 0.1F, 1F)
																				Return True
																			End If
																			If nameHashCode = 155913 Then
																				If Me.m_isParsingText Then
																					Me.m_textInfo.linkInfo(Me.m_textInfo.linkCount).linkTextLength = Me.m_characterCount - Me.m_textInfo.linkInfo(Me.m_textInfo.linkCount).linkTextfirstCharacterIndex
																					Me.m_textInfo.linkCount += 1
																				End If
																				Return True
																			End If
																			If nameHashCode = 156816 Then
																				Me.m_isNonBreakingSpace = False
																				Return True
																			End If
																			If nameHashCode = 158392 Then
																				Me.m_currentFontSize = Me.m_sizeStack.Remove()
																				Me.m_fontScale = Me.m_currentFontSize / Me.m_currentFontAsset.fontInfo.PointSize * Me.m_currentFontAsset.fontInfo.Scale * If((Not Me.m_isOrthographic), 0.1F, 1F)
																				Return True
																			End If
																			If nameHashCode <> 275917 Then
																				If nameHashCode <> 276254 Then
																					If nameHashCode = 280416 Then
																						Return False
																					End If
																					If nameHashCode <> 281955 Then
																						If nameHashCode <> 320078 Then
																							If nameHashCode <> 322689 Then
																								If nameHashCode <> 327550 Then
																									If nameHashCode = 1065846 Then
																										Me.m_lineJustification = Me.m_textAlignment
																										Return True
																									End If
																									If nameHashCode = 1071884 Then
																										Me.m_htmlColor = Me.m_colorStack.Remove()
																										Return True
																									End If
																									If nameHashCode <> 1112618 Then
																										If nameHashCode = 1117479 Then
																											Me.m_width = -1F
																											Return True
																										End If
																										If nameHashCode = 1750458 Then
																											Return False
																										End If
																										If nameHashCode = 1913798 Then
																											Dim valueHashCode As Integer = Me.m_xmlAttribute(0).valueHashCode
																											If Me.m_isParsingText Then
																												Me.m_actionStack.Add(valueHashCode)
																											End If
																											Return True
																										End If
																										If nameHashCode <> 1983971 Then
																											If nameHashCode <> 2068980 Then
																												If nameHashCode <> 2109854 Then
																													If nameHashCode <> 2152041 Then
																														If nameHashCode = 2246877 Then
																															Dim valueHashCode2 As Integer = Me.m_xmlAttribute(0).valueHashCode
																															Dim tmp_SpriteAsset As TMP_SpriteAsset
																															If Me.m_xmlAttribute(0).valueType = TagType.None OrElse Me.m_xmlAttribute(0).valueType = TagType.NumericalValue Then
																																If Me.m_defaultSpriteAsset Is Nothing Then
																																	If TMP_Settings.defaultSpriteAsset IsNot Nothing Then
																																		Me.m_defaultSpriteAsset = TMP_Settings.defaultSpriteAsset
																																	Else
																																		Me.m_defaultSpriteAsset = Resources.Load(Of TMP_SpriteAsset)("Sprite Assets/Default Sprite Asset")
																																	End If
																																End If
																																Me.m_currentSpriteAsset = Me.m_defaultSpriteAsset
																																If Me.m_currentSpriteAsset Is Nothing Then
																																	Return False
																																End If
																															ElseIf MaterialReferenceManager.TryGetSpriteAsset(valueHashCode2, tmp_SpriteAsset) Then
																																Me.m_currentSpriteAsset = tmp_SpriteAsset
																															Else
																																If tmp_SpriteAsset Is Nothing Then
																																	tmp_SpriteAsset = Resources.Load(Of TMP_SpriteAsset)("Sprites/" + New String(Me.m_htmlTag, Me.m_xmlAttribute(0).valueStartIndex, Me.m_xmlAttribute(0).valueLength))
																																End If
																																If tmp_SpriteAsset Is Nothing Then
																																	Return False
																																End If
																																MaterialReferenceManager.AddSpriteAsset(valueHashCode2, tmp_SpriteAsset)
																																Me.m_currentSpriteAsset = tmp_SpriteAsset
																															End If
																															If Me.m_xmlAttribute(0).valueType = TagType.NumericalValue Then
																																Dim num11 As Integer = CInt(Me.ConvertToFloat(Me.m_htmlTag, Me.m_xmlAttribute(0).valueStartIndex, Me.m_xmlAttribute(0).valueLength, Me.m_xmlAttribute(0).valueDecimalIndex))
																																If num11 = -9999 Then
																																	Return False
																																End If
																																If num11 > Me.m_currentSpriteAsset.spriteInfoList.Count - 1 Then
																																	Return False
																																End If
																																Me.m_spriteIndex = num11
																															ElseIf Me.m_xmlAttribute(1).nameHashCode = 43347 Then
																																Dim spriteIndex As Integer = Me.m_currentSpriteAsset.GetSpriteIndex(Me.m_xmlAttribute(1).valueHashCode)
																																If spriteIndex = -1 Then
																																	Return False
																																End If
																																Me.m_spriteIndex = spriteIndex
																															Else
																																If Me.m_xmlAttribute(1).nameHashCode <> 295562 Then
																																	Return False
																																End If
																																Dim num12 As Integer = CInt(Me.ConvertToFloat(Me.m_htmlTag, Me.m_xmlAttribute(1).valueStartIndex, Me.m_xmlAttribute(1).valueLength, Me.m_xmlAttribute(1).valueDecimalIndex))
																																If num12 = -9999 Then
																																	Return False
																																End If
																																If num12 > Me.m_currentSpriteAsset.spriteInfoList.Count - 1 Then
																																	Return False
																																End If
																																Me.m_spriteIndex = num12
																															End If
																															Me.m_currentMaterialIndex = MaterialReference.AddMaterialReference(Me.m_currentSpriteAsset.material, Me.m_currentSpriteAsset, Me.m_materialReferences, Me.m_materialReferenceIndexLookup)
																															Me.m_spriteColor = TMP_Text.s_colorWhite
																															Me.m_tintSprite = False
																															If Me.m_xmlAttribute(1).nameHashCode = 45819 Then
																																Me.m_tintSprite = Me.ConvertToFloat(Me.m_htmlTag, Me.m_xmlAttribute(1).valueStartIndex, Me.m_xmlAttribute(1).valueLength, Me.m_xmlAttribute(1).valueDecimalIndex) <> 0F
																															ElseIf Me.m_xmlAttribute(2).nameHashCode = 45819 Then
																																Me.m_tintSprite = Me.ConvertToFloat(Me.m_htmlTag, Me.m_xmlAttribute(2).valueStartIndex, Me.m_xmlAttribute(2).valueLength, Me.m_xmlAttribute(2).valueDecimalIndex) <> 0F
																															End If
																															If Me.m_xmlAttribute(1).nameHashCode = 281955 Then
																																Me.m_spriteColor = Me.HexCharsToColor(Me.m_htmlTag, Me.m_xmlAttribute(1).valueStartIndex, Me.m_xmlAttribute(1).valueLength)
																															ElseIf Me.m_xmlAttribute(2).nameHashCode = 281955 Then
																																Me.m_spriteColor = Me.HexCharsToColor(Me.m_htmlTag, Me.m_xmlAttribute(2).valueStartIndex, Me.m_xmlAttribute(2).valueLength)
																															End If
																															Me.m_xmlAttribute(1).nameHashCode = 0
																															Me.m_xmlAttribute(2).nameHashCode = 0
																															Me.m_textElementType = TMP_TextElementType.Sprite
																															Return True
																														End If
																														If nameHashCode = 7443301 Then
																															If Me.m_isParsingText Then
																															End If
																															Me.m_actionStack.Remove()
																															Return True
																														End If
																														If nameHashCode = 7513474 Then
																															Me.m_cSpacing = 0F
																															Return True
																														End If
																														If nameHashCode = 7598483 Then
																															Me.tag_Indent = Me.m_indentStack.Remove()
																															Return True
																														End If
																														If nameHashCode = 7639357 Then
																															Me.m_marginLeft = 0F
																															Me.m_marginRight = 0F
																															Return True
																														End If
																														If nameHashCode = 7681544 Then
																															Me.m_monoSpacing = 0F
																															Return True
																														End If
																														If nameHashCode = 15115642 Then
																															Me.tag_NoParsing = True
																															Return True
																														End If
																														If nameHashCode <> 16034505 Then
																															If nameHashCode <> 52232547 Then
																																If nameHashCode <> 54741026 Then
																																	Return False
																																End If
																																Me.m_baselineOffset = 0F
																																Return True
																															End If
																														Else
																															Dim num13 As Single = Me.ConvertToFloat(Me.m_htmlTag, Me.m_xmlAttribute(0).valueStartIndex, Me.m_xmlAttribute(0).valueLength, Me.m_xmlAttribute(0).valueDecimalIndex)
																															If num13 = -9999F OrElse num13 = 0F Then
																																Return False
																															End If
																															If tagUnits = TagUnits.Pixels Then
																																Me.m_baselineOffset = num13
																																Return True
																															End If
																															If tagUnits <> TagUnits.FontUnits Then
																																Return tagUnits <> TagUnits.Percentage AndAlso False
																															End If
																															Me.m_baselineOffset = num13 * Me.m_fontScale * Me.m_fontAsset.fontInfo.Ascender
																															Return True
																														End If
																													Else
																														Dim num13 As Single = Me.ConvertToFloat(Me.m_htmlTag, Me.m_xmlAttribute(0).valueStartIndex, Me.m_xmlAttribute(0).valueLength, Me.m_xmlAttribute(0).valueDecimalIndex)
																														If num13 = -9999F OrElse num13 = 0F Then
																															Return False
																														End If
																														If tagUnits <> TagUnits.Pixels Then
																															If tagUnits <> TagUnits.FontUnits Then
																																If tagUnits = TagUnits.Percentage Then
																																	Return False
																																End If
																															Else
																																Me.m_monoSpacing = num13
																																Me.m_monoSpacing *= Me.m_fontScale * Me.m_fontAsset.fontInfo.TabWidth / CSng(Me.m_fontAsset.tabSize)
																															End If
																														Else
																															Me.m_monoSpacing = num13
																														End If
																														Return True
																													End If
																												Else
																													Dim num13 As Single = Me.ConvertToFloat(Me.m_htmlTag, Me.m_xmlAttribute(0).valueStartIndex, Me.m_xmlAttribute(0).valueLength, Me.m_xmlAttribute(0).valueDecimalIndex)
																													If num13 = -9999F OrElse num13 = 0F Then
																														Return False
																													End If
																													Me.m_marginLeft = num13
																													If tagUnits <> TagUnits.Pixels Then
																														If tagUnits <> TagUnits.FontUnits Then
																															If tagUnits = TagUnits.Percentage Then
																																Me.m_marginLeft = (Me.m_marginWidth - If((Me.m_width = -1F), 0F, Me.m_width)) * Me.m_marginLeft / 100F
																															End If
																														Else
																															Me.m_marginLeft *= Me.m_fontScale * Me.m_fontAsset.fontInfo.TabWidth / CSng(Me.m_fontAsset.tabSize)
																														End If
																													End If
																													Me.m_marginLeft = If((Me.m_marginLeft < 0F), 0F, Me.m_marginLeft)
																													Me.m_marginRight = Me.m_marginLeft
																													Return True
																												End If
																											Else
																												Dim num13 As Single = Me.ConvertToFloat(Me.m_htmlTag, Me.m_xmlAttribute(0).valueStartIndex, Me.m_xmlAttribute(0).valueLength, Me.m_xmlAttribute(0).valueDecimalIndex)
																												If num13 = -9999F OrElse num13 = 0F Then
																													Return False
																												End If
																												If tagUnits <> TagUnits.Pixels Then
																													If tagUnits <> TagUnits.FontUnits Then
																														If tagUnits = TagUnits.Percentage Then
																															Me.tag_Indent = Me.m_marginWidth * num13 / 100F
																														End If
																													Else
																														Me.tag_Indent = num13
																														Me.tag_Indent *= Me.m_fontScale * Me.m_fontAsset.fontInfo.TabWidth / CSng(Me.m_fontAsset.tabSize)
																													End If
																												Else
																													Me.tag_Indent = num13
																												End If
																												Me.m_indentStack.Add(Me.tag_Indent)
																												Me.m_xAdvance = Me.tag_Indent
																												Return True
																											End If
																										Else
																											Dim num13 As Single = Me.ConvertToFloat(Me.m_htmlTag, Me.m_xmlAttribute(0).valueStartIndex, Me.m_xmlAttribute(0).valueLength, Me.m_xmlAttribute(0).valueDecimalIndex)
																											If num13 = -9999F OrElse num13 = 0F Then
																												Return False
																											End If
																											If tagUnits <> TagUnits.Pixels Then
																												If tagUnits <> TagUnits.FontUnits Then
																													If tagUnits = TagUnits.Percentage Then
																														Return False
																													End If
																												Else
																													Me.m_cSpacing = num13
																													Me.m_cSpacing *= Me.m_fontScale * Me.m_fontAsset.fontInfo.TabWidth / CSng(Me.m_fontAsset.tabSize)
																												End If
																											Else
																												Me.m_cSpacing = num13
																											End If
																											Return True
																										End If
																									Else
																										Dim tmp_Style As TMP_Style = TMP_StyleSheet.GetStyle(Me.m_xmlAttribute(0).valueHashCode)
																										If tmp_Style Is Nothing Then
																											Dim num14 As Integer = Me.m_styleStack.Remove()
																											tmp_Style = TMP_StyleSheet.GetStyle(num14)
																										End If
																										If tmp_Style Is Nothing Then
																											Return False
																										End If
																										For i As Integer = 0 To tmp_Style.styleClosingTagArray.Length - 1
																											If tmp_Style.styleClosingTagArray(i) = 60 Then
																												Me.ValidateHtmlTag(tmp_Style.styleClosingTagArray, i + 1, i)
																											End If
																										Next
																										Return True
																									End If
																								Else
																									Dim num13 As Single = Me.ConvertToFloat(Me.m_htmlTag, Me.m_xmlAttribute(0).valueStartIndex, Me.m_xmlAttribute(0).valueLength, Me.m_xmlAttribute(0).valueDecimalIndex)
																									If num13 = -9999F OrElse num13 = 0F Then
																										Return False
																									End If
																									If tagUnits <> TagUnits.Pixels Then
																										If tagUnits = TagUnits.FontUnits Then
																											Return False
																										End If
																										If tagUnits = TagUnits.Percentage Then
																											Me.m_width = Me.m_marginWidth * num13 / 100F
																										End If
																									Else
																										Me.m_width = num13
																									End If
																									Return True
																								End If
																							Else
																								Dim tmp_Style As TMP_Style = TMP_StyleSheet.GetStyle(Me.m_xmlAttribute(0).valueHashCode)
																								If tmp_Style Is Nothing Then
																									Return False
																								End If
																								Me.m_styleStack.Add(tmp_Style.hashCode)
																								For j As Integer = 0 To tmp_Style.styleOpeningTagArray.Length - 1
																									If tmp_Style.styleOpeningTagArray(j) = 60 Then
																										Me.ValidateHtmlTag(tmp_Style.styleOpeningTagArray, j + 1, j)
																									End If
																								Next
																								Return True
																							End If
																						Else
																							Dim num13 As Single = Me.ConvertToFloat(Me.m_htmlTag, Me.m_xmlAttribute(0).valueStartIndex, Me.m_xmlAttribute(0).valueLength, Me.m_xmlAttribute(0).valueDecimalIndex)
																							If num13 = -9999F OrElse num13 = 0F Then
																								Return False
																							End If
																							If tagUnits = TagUnits.Pixels Then
																								Me.m_xAdvance += num13
																								Return True
																							End If
																							If tagUnits <> TagUnits.FontUnits Then
																								Return tagUnits <> TagUnits.Percentage AndAlso False
																							End If
																							Me.m_xAdvance += num13 * Me.m_fontScale * Me.m_fontAsset.fontInfo.TabWidth / CSng(Me.m_fontAsset.tabSize)
																							Return True
																						End If
																					Else
																						If Me.m_htmlTag(6) = "#"c AndAlso num = 13 Then
																							Me.m_htmlColor = Me.HexCharsToColor(Me.m_htmlTag, num)
																							Me.m_colorStack.Add(Me.m_htmlColor)
																							Return True
																						End If
																						If Me.m_htmlTag(6) = "#"c AndAlso num = 15 Then
																							Me.m_htmlColor = Me.HexCharsToColor(Me.m_htmlTag, num)
																							Me.m_colorStack.Add(Me.m_htmlColor)
																							Return True
																						End If
																						Dim valueHashCode3 As Integer = Me.m_xmlAttribute(0).valueHashCode
																						If valueHashCode3 = -36881330 Then
																							Me.m_htmlColor = New Color32(160, 32, 240, Byte.MaxValue)
																							Me.m_colorStack.Add(Me.m_htmlColor)
																							Return True
																						End If
																						If valueHashCode3 = 125395 Then
																							Me.m_htmlColor = Color.red
																							Me.m_colorStack.Add(Me.m_htmlColor)
																							Return True
																						End If
																						If valueHashCode3 = 3573310 Then
																							Me.m_htmlColor = Color.blue
																							Me.m_colorStack.Add(Me.m_htmlColor)
																							Return True
																						End If
																						If valueHashCode3 = 26556144 Then
																							Me.m_htmlColor = New Color32(Byte.MaxValue, 128, 0, Byte.MaxValue)
																							Me.m_colorStack.Add(Me.m_htmlColor)
																							Return True
																						End If
																						If valueHashCode3 = 117905991 Then
																							Me.m_htmlColor = Color.black
																							Me.m_colorStack.Add(Me.m_htmlColor)
																							Return True
																						End If
																						If valueHashCode3 = 121463835 Then
																							Me.m_htmlColor = Color.green
																							Me.m_colorStack.Add(Me.m_htmlColor)
																							Return True
																						End If
																						If valueHashCode3 = 140357351 Then
																							Me.m_htmlColor = Color.white
																							Me.m_colorStack.Add(Me.m_htmlColor)
																							Return True
																						End If
																						If valueHashCode3 <> 554054276 Then
																							Return False
																						End If
																						Me.m_htmlColor = Color.yellow
																						Me.m_colorStack.Add(Me.m_htmlColor)
																						Return True
																					End If
																				Else
																					If Me.m_xmlAttribute(0).valueLength <> 3 Then
																						Return False
																					End If
																					Me.m_htmlColor.a = CByte((Me.HexToInt(Me.m_htmlTag(7)) * 16 + Me.HexToInt(Me.m_htmlTag(8))))
																					Return True
																				End If
																			Else
																				Dim valueHashCode4 As Integer = Me.m_xmlAttribute(0).valueHashCode
																				If valueHashCode4 = -523808257 Then
																					Me.m_lineJustification = TextAlignmentOptions.Justified
																					Return True
																				End If
																				If valueHashCode4 = -458210101 Then
																					Me.m_lineJustification = TextAlignmentOptions.Center
																					Return True
																				End If
																				If valueHashCode4 = 3774683 Then
																					Me.m_lineJustification = TextAlignmentOptions.Left
																					Return True
																				End If
																				If valueHashCode4 <> 136703040 Then
																					Return False
																				End If
																				Me.m_lineJustification = TextAlignmentOptions.Right
																				Return True
																			End If
																		Else
																			Dim num13 As Single = Me.ConvertToFloat(Me.m_htmlTag, Me.m_xmlAttribute(0).valueStartIndex, Me.m_xmlAttribute(0).valueLength, Me.m_xmlAttribute(0).valueDecimalIndex)
																			If num13 = -9999F OrElse num13 = 0F Then
																				Return False
																			End If
																			If tagUnits <> TagUnits.Pixels Then
																				If tagUnits = TagUnits.FontUnits Then
																					Me.m_currentFontSize = Me.m_fontSize * num13
																					Me.m_sizeStack.Add(Me.m_currentFontSize)
																					Me.m_fontScale = Me.m_currentFontSize / Me.m_currentFontAsset.fontInfo.PointSize * Me.m_currentFontAsset.fontInfo.Scale * If((Not Me.m_isOrthographic), 0.1F, 1F)
																					Return True
																				End If
																				If tagUnits <> TagUnits.Percentage Then
																					Return False
																				End If
																				Me.m_currentFontSize = Me.m_fontSize * num13 / 100F
																				Me.m_sizeStack.Add(Me.m_currentFontSize)
																				Me.m_fontScale = Me.m_currentFontSize / Me.m_currentFontAsset.fontInfo.PointSize * Me.m_currentFontAsset.fontInfo.Scale * If((Not Me.m_isOrthographic), 0.1F, 1F)
																				Return True
																			Else
																				If Me.m_htmlTag(5) = "+"c Then
																					Me.m_currentFontSize = Me.m_fontSize + num13
																					Me.m_sizeStack.Add(Me.m_currentFontSize)
																					Me.m_fontScale = Me.m_currentFontSize / Me.m_currentFontAsset.fontInfo.PointSize * Me.m_currentFontAsset.fontInfo.Scale * If((Not Me.m_isOrthographic), 0.1F, 1F)
																					Return True
																				End If
																				If Me.m_htmlTag(5) = "-"c Then
																					Me.m_currentFontSize = Me.m_fontSize + num13
																					Me.m_sizeStack.Add(Me.m_currentFontSize)
																					Me.m_fontScale = Me.m_currentFontSize / Me.m_currentFontAsset.fontInfo.PointSize * Me.m_currentFontAsset.fontInfo.Scale * If((Not Me.m_isOrthographic), 0.1F, 1F)
																					Return True
																				End If
																				Me.m_currentFontSize = num13
																				Me.m_sizeStack.Add(Me.m_currentFontSize)
																				Me.m_fontScale = Me.m_currentFontSize / Me.m_currentFontAsset.fontInfo.PointSize * Me.m_currentFontAsset.fontInfo.Scale * If((Not Me.m_isOrthographic), 0.1F, 1F)
																				Return True
																			End If
																		End If
																	Else
																		Dim valueHashCode5 As Integer = Me.m_xmlAttribute(0).valueHashCode
																		Dim nameHashCode2 As Integer = Me.m_xmlAttribute(1).nameHashCode
																		Dim valueHashCode6 As Integer = Me.m_xmlAttribute(1).valueHashCode
																		If valueHashCode5 = 764638571 OrElse valueHashCode5 = 523367755 Then
																			Me.m_currentFontAsset = Me.m_materialReferences(0).fontAsset
																			Me.m_currentMaterial = Me.m_materialReferences(0).material
																			Me.m_currentMaterialIndex = 0
																			Me.m_fontScale = Me.m_currentFontSize / Me.m_currentFontAsset.fontInfo.PointSize * Me.m_currentFontAsset.fontInfo.Scale * If((Not Me.m_isOrthographic), 0.1F, 1F)
																			Me.m_materialReferenceStack.Add(Me.m_materialReferences(0))
																			Return True
																		End If
																		Dim tmp_FontAsset As TMP_FontAsset
																		If Not MaterialReferenceManager.TryGetFontAsset(valueHashCode5, tmp_FontAsset) Then
																			tmp_FontAsset = Resources.Load(Of TMP_FontAsset)("Fonts & Materials/" + New String(Me.m_htmlTag, Me.m_xmlAttribute(0).valueStartIndex, Me.m_xmlAttribute(0).valueLength))
																			If tmp_FontAsset Is Nothing Then
																				Return False
																			End If
																			MaterialReferenceManager.AddFontAsset(tmp_FontAsset)
																		End If
																		If nameHashCode2 = 0 AndAlso valueHashCode6 = 0 Then
																			Me.m_currentMaterial = tmp_FontAsset.material
																			Me.m_currentMaterialIndex = MaterialReference.AddMaterialReference(Me.m_currentMaterial, tmp_FontAsset, Me.m_materialReferences, Me.m_materialReferenceIndexLookup)
																			Me.m_materialReferenceStack.Add(Me.m_materialReferences(Me.m_currentMaterialIndex))
																		Else
																			If nameHashCode2 <> 103415287 Then
																				Return False
																			End If
																			Dim material As Material
																			If MaterialReferenceManager.TryGetMaterial(valueHashCode6, material) Then
																				Me.m_currentMaterial = material
																				Me.m_currentMaterialIndex = MaterialReference.AddMaterialReference(Me.m_currentMaterial, tmp_FontAsset, Me.m_materialReferences, Me.m_materialReferenceIndexLookup)
																				Me.m_materialReferenceStack.Add(Me.m_materialReferences(Me.m_currentMaterialIndex))
																			Else
																				material = Resources.Load(Of Material)("Fonts & Materials/" + New String(Me.m_htmlTag, Me.m_xmlAttribute(1).valueStartIndex, Me.m_xmlAttribute(1).valueLength))
																				If material Is Nothing Then
																					Return False
																				End If
																				MaterialReferenceManager.AddFontMaterial(valueHashCode6, material)
																				Me.m_currentMaterial = material
																				Me.m_currentMaterialIndex = MaterialReference.AddMaterialReference(Me.m_currentMaterial, tmp_FontAsset, Me.m_materialReferences, Me.m_materialReferenceIndexLookup)
																				Me.m_materialReferenceStack.Add(Me.m_materialReferences(Me.m_currentMaterialIndex))
																			End If
																		End If
																		Me.m_currentFontAsset = tmp_FontAsset
																		Me.m_fontScale = Me.m_currentFontSize / Me.m_currentFontAsset.fontInfo.PointSize * Me.m_currentFontAsset.fontInfo.Scale * If((Not Me.m_isOrthographic), 0.1F, 1F)
																		Return True
																	End If
																Else
																	Dim num13 As Single = Me.ConvertToFloat(Me.m_htmlTag, Me.m_xmlAttribute(0).valueStartIndex, Me.m_xmlAttribute(0).valueLength, Me.m_xmlAttribute(0).valueDecimalIndex)
																	If num13 = -9999F Then
																		Return False
																	End If
																	If tagUnits = TagUnits.Pixels Then
																		Me.m_xAdvance = num13
																		Return True
																	End If
																	If tagUnits = TagUnits.FontUnits Then
																		Me.m_xAdvance = num13 * Me.m_fontScale * Me.m_fontAsset.fontInfo.TabWidth / CSng(Me.m_fontAsset.tabSize)
																		Return True
																	End If
																	If tagUnits <> TagUnits.Percentage Then
																		Return False
																	End If
																	Me.m_xAdvance = Me.m_marginWidth * num13 / 100F
																	Return True
																End If
															Else
																Dim num13 As Single = Me.ConvertToFloat(Me.m_htmlTag, Me.m_xmlAttribute(0).valueStartIndex, Me.m_xmlAttribute(0).valueLength, Me.m_xmlAttribute(0).valueDecimalIndex)
																If num13 = -9999F OrElse num13 = 0F Then
																	Return False
																End If
																If(Me.m_fontStyle And FontStyles.Bold) = FontStyles.Bold Then
																	Return True
																End If
																Me.m_style = Me.m_style And CType((-2), FontStyles)
																Dim num15 As Integer = CInt(num13)
																If num15 <> 100 Then
																	If num15 <> 200 Then
																		If num15 <> 300 Then
																			If num15 <> 400 Then
																				If num15 <> 500 Then
																					If num15 <> 600 Then
																						If num15 <> 700 Then
																							If num15 <> 800 Then
																								If num15 = 900 Then
																									Me.m_fontWeightInternal = 900
																								End If
																							Else
																								Me.m_fontWeightInternal = 800
																							End If
																						Else
																							Me.m_fontWeightInternal = 700
																							Me.m_style = Me.m_style Or FontStyles.Bold
																						End If
																					Else
																						Me.m_fontWeightInternal = 600
																					End If
																				Else
																					Me.m_fontWeightInternal = 500
																				End If
																			Else
																				Me.m_fontWeightInternal = 400
																			End If
																		Else
																			Me.m_fontWeightInternal = 300
																		End If
																	Else
																		Me.m_fontWeightInternal = 200
																	End If
																Else
																	Me.m_fontWeightInternal = 100
																End If
																Me.m_fontWeightStack.Add(Me.m_fontWeightInternal)
																Return True
															End If
														Else
															Dim num13 As Single = Me.ConvertToFloat(Me.m_htmlTag, Me.m_xmlAttribute(0).valueStartIndex, Me.m_xmlAttribute(0).valueLength, Me.m_xmlAttribute(0).valueDecimalIndex)
															If num13 = -9999F OrElse num13 = 0F Then
																Return False
															End If
															Me.m_marginRight = num13
															If tagUnits <> TagUnits.Pixels Then
																If tagUnits <> TagUnits.FontUnits Then
																	If tagUnits = TagUnits.Percentage Then
																		Me.m_marginRight = (Me.m_marginWidth - If((Me.m_width = -1F), 0F, Me.m_width)) * Me.m_marginRight / 100F
																	End If
																Else
																	Me.m_marginRight *= Me.m_fontScale * Me.m_fontAsset.fontInfo.TabWidth / CSng(Me.m_fontAsset.tabSize)
																End If
															End If
															Me.m_marginRight = If((Me.m_marginRight < 0F), 0F, Me.m_marginRight)
															Return True
														End If
													End If
													Me.m_style = Me.m_style And CType((-17), FontStyles)
													Return True
												End If
												Me.m_style = Me.m_style And CType((-33), FontStyles)
												Return True
											Else
												Dim num13 As Single = Me.ConvertToFloat(Me.m_htmlTag, Me.m_xmlAttribute(0).valueStartIndex, Me.m_xmlAttribute(0).valueLength, Me.m_xmlAttribute(0).valueDecimalIndex)
												If num13 = -9999F OrElse num13 = 0F Then
													Return False
												End If
												If tagUnits <> TagUnits.Pixels Then
													If tagUnits <> TagUnits.FontUnits Then
														If tagUnits = TagUnits.Percentage Then
															Me.tag_LineIndent = Me.m_marginWidth * num13 / 100F
														End If
													Else
														Me.tag_LineIndent = num13
														Me.tag_LineIndent *= Me.m_fontScale * Me.m_fontAsset.fontInfo.TabWidth / CSng(Me.m_fontAsset.tabSize)
													End If
												Else
													Me.tag_LineIndent = num13
												End If
												Me.m_xAdvance += Me.tag_LineIndent
												Return True
											End If
										Else
											Dim num13 As Single = Me.ConvertToFloat(Me.m_htmlTag, Me.m_xmlAttribute(0).valueStartIndex, Me.m_xmlAttribute(0).valueLength, Me.m_xmlAttribute(0).valueDecimalIndex)
											If num13 = -9999F OrElse num13 = 0F Then
												Return False
											End If
											Me.m_lineHeight = num13
											If tagUnits <> TagUnits.Pixels Then
												If tagUnits <> TagUnits.FontUnits Then
													If tagUnits = TagUnits.Percentage Then
														Me.m_lineHeight = Me.m_fontAsset.fontInfo.LineHeight * Me.m_lineHeight / 100F * Me.m_fontScale
													End If
												Else
													Me.m_lineHeight *= Me.m_fontAsset.fontInfo.LineHeight * Me.m_fontScale
												End If
											End If
											Return True
										End If
									Else
										Dim num13 As Single = Me.ConvertToFloat(Me.m_htmlTag, Me.m_xmlAttribute(0).valueStartIndex, Me.m_xmlAttribute(0).valueLength, Me.m_xmlAttribute(0).valueDecimalIndex)
										If num13 = -9999F OrElse num13 = 0F Then
											Return False
										End If
										Me.m_marginLeft = num13
										If tagUnits <> TagUnits.Pixels Then
											If tagUnits <> TagUnits.FontUnits Then
												If tagUnits = TagUnits.Percentage Then
													Me.m_marginLeft = (Me.m_marginWidth - If((Me.m_width = -1F), 0F, Me.m_width)) * Me.m_marginLeft / 100F
												End If
											Else
												Me.m_marginLeft *= Me.m_fontScale * Me.m_fontAsset.fontInfo.TabWidth / CSng(Me.m_fontAsset.tabSize)
											End If
										End If
										Me.m_marginLeft = If((Me.m_marginLeft < 0F), 0F, Me.m_marginLeft)
										Return True
									End If
								End If
							End If
							Me.m_style = Me.m_style Or FontStyles.UpperCase
							Return True
						Case 446
							If(Me.m_fontStyle And FontStyles.Underline) <> FontStyles.Underline Then
								Me.m_style = Me.m_style And CType((-5), FontStyles)
							End If
							Return True
					End Select
				Case 117
					Me.m_style = Me.m_style Or FontStyles.Underline
					Return True
			End Select
		End Function

		' Token: 0x04005320 RID: 21280
		<SerializeField()>
		Protected m_text As String

		' Token: 0x04005321 RID: 21281
		<SerializeField()>
		Protected m_fontAsset As TMP_FontAsset

		' Token: 0x04005322 RID: 21282
		Protected m_currentFontAsset As TMP_FontAsset

		' Token: 0x04005323 RID: 21283
		Protected m_isSDFShader As Boolean

		' Token: 0x04005324 RID: 21284
		<SerializeField()>
		Protected m_sharedMaterial As Material

		' Token: 0x04005325 RID: 21285
		Protected m_currentMaterial As Material

		' Token: 0x04005326 RID: 21286
		Protected m_materialReferences As MaterialReference() = New MaterialReference(31) {}

		' Token: 0x04005327 RID: 21287
		Protected m_materialReferenceIndexLookup As Dictionary(Of Integer, Integer) = New Dictionary(Of Integer, Integer)()

		' Token: 0x04005328 RID: 21288
		Protected m_materialReferenceStack As TMP_XmlTagStack(Of MaterialReference) = New TMP_XmlTagStack(Of MaterialReference)(New MaterialReference(15) {})

		' Token: 0x04005329 RID: 21289
		Protected m_currentMaterialIndex As Integer

		' Token: 0x0400532A RID: 21290
		Protected m_sharedMaterialHashCode As Integer

		' Token: 0x0400532B RID: 21291
		<SerializeField()>
		Protected m_fontSharedMaterials As Material()

		' Token: 0x0400532C RID: 21292
		<SerializeField()>
		Protected m_fontMaterial As Material

		' Token: 0x0400532D RID: 21293
		<SerializeField()>
		Protected m_fontMaterials As Material()

		' Token: 0x0400532E RID: 21294
		Protected m_isMaterialDirty As Boolean

		' Token: 0x0400532F RID: 21295
		<FormerlySerializedAs("m_fontColor")>
		<SerializeField()>
		Protected m_fontColor32 As Color32 = Color.white

		' Token: 0x04005330 RID: 21296
		<SerializeField()>
		Protected m_fontColor As Color = Color.white

		' Token: 0x04005331 RID: 21297
		Protected Shared s_colorWhite As Color32 = New Color32(Byte.MaxValue, Byte.MaxValue, Byte.MaxValue, Byte.MaxValue)

		' Token: 0x04005332 RID: 21298
		<SerializeField()>
		Protected m_enableVertexGradient As Boolean

		' Token: 0x04005333 RID: 21299
		<SerializeField()>
		Protected m_fontColorGradient As VertexGradient = New VertexGradient(Color.white)

		' Token: 0x04005334 RID: 21300
		Protected m_spriteAsset As TMP_SpriteAsset

		' Token: 0x04005335 RID: 21301
		<SerializeField()>
		Protected m_tintAllSprites As Boolean

		' Token: 0x04005336 RID: 21302
		Protected m_tintSprite As Boolean

		' Token: 0x04005337 RID: 21303
		Protected m_spriteColor As Color32

		' Token: 0x04005338 RID: 21304
		<SerializeField()>
		Protected m_overrideHtmlColors As Boolean

		' Token: 0x04005339 RID: 21305
		<SerializeField()>
		Protected m_faceColor As Color32 = Color.white

		' Token: 0x0400533A RID: 21306
		<SerializeField()>
		Protected m_outlineColor As Color32 = Color.black

		' Token: 0x0400533B RID: 21307
		Protected m_outlineWidth As Single

		' Token: 0x0400533C RID: 21308
		<SerializeField()>
		Protected m_fontSize As Single = 36F

		' Token: 0x0400533D RID: 21309
		Protected m_currentFontSize As Single

		' Token: 0x0400533E RID: 21310
		<SerializeField()>
		Protected m_fontSizeBase As Single = 36F

		' Token: 0x0400533F RID: 21311
		Protected m_sizeStack As TMP_XmlTagStack(Of Single) = New TMP_XmlTagStack(Of Single)(New Single(15) {})

		' Token: 0x04005340 RID: 21312
		<SerializeField()>
		Protected m_fontWeight As Integer = 400

		' Token: 0x04005341 RID: 21313
		Protected m_fontWeightInternal As Integer

		' Token: 0x04005342 RID: 21314
		Protected m_fontWeightStack As TMP_XmlTagStack(Of Integer) = New TMP_XmlTagStack(Of Integer)(New Integer(15) {})

		' Token: 0x04005343 RID: 21315
		<SerializeField()>
		Protected m_enableAutoSizing As Boolean

		' Token: 0x04005344 RID: 21316
		Protected m_maxFontSize As Single

		' Token: 0x04005345 RID: 21317
		Protected m_minFontSize As Single

		' Token: 0x04005346 RID: 21318
		<SerializeField()>
		Protected m_fontSizeMin As Single

		' Token: 0x04005347 RID: 21319
		<SerializeField()>
		Protected m_fontSizeMax As Single

		' Token: 0x04005348 RID: 21320
		<SerializeField()>
		Protected m_fontStyle As FontStyles

		' Token: 0x04005349 RID: 21321
		Protected m_style As FontStyles

		' Token: 0x0400534A RID: 21322
		Protected m_isUsingBold As Boolean

		' Token: 0x0400534B RID: 21323
		<SerializeField()>
		<FormerlySerializedAs("m_lineJustification")>
		Protected m_textAlignment As TextAlignmentOptions

		' Token: 0x0400534C RID: 21324
		Protected m_lineJustification As TextAlignmentOptions

		' Token: 0x0400534D RID: 21325
		Protected m_textContainerLocalCorners As Vector3() = New Vector3(3) {}

		' Token: 0x0400534E RID: 21326
		<SerializeField()>
		Protected m_characterSpacing As Single

		' Token: 0x0400534F RID: 21327
		Protected m_cSpacing As Single

		' Token: 0x04005350 RID: 21328
		Protected m_monoSpacing As Single

		' Token: 0x04005351 RID: 21329
		<SerializeField()>
		Protected m_lineSpacing As Single

		' Token: 0x04005352 RID: 21330
		Protected m_lineSpacingDelta As Single

		' Token: 0x04005353 RID: 21331
		Protected m_lineHeight As Single

		' Token: 0x04005354 RID: 21332
		<SerializeField()>
		Protected m_lineSpacingMax As Single

		' Token: 0x04005355 RID: 21333
		<SerializeField()>
		Protected m_paragraphSpacing As Single

		' Token: 0x04005356 RID: 21334
		<SerializeField()>
		Protected m_charWidthMaxAdj As Single

		' Token: 0x04005357 RID: 21335
		Protected m_charWidthAdjDelta As Single

		' Token: 0x04005358 RID: 21336
		<SerializeField()>
		Protected m_enableWordWrapping As Boolean

		' Token: 0x04005359 RID: 21337
		Protected m_isCharacterWrappingEnabled As Boolean

		' Token: 0x0400535A RID: 21338
		Protected m_isNonBreakingSpace As Boolean

		' Token: 0x0400535B RID: 21339
		Protected m_isIgnoringAlignment As Boolean

		' Token: 0x0400535C RID: 21340
		<SerializeField()>
		Protected m_wordWrappingRatios As Single = 0.4F

		' Token: 0x0400535D RID: 21341
		<SerializeField()>
		Protected m_overflowMode As TextOverflowModes

		' Token: 0x0400535E RID: 21342
		Protected m_isTextTruncated As Boolean

		' Token: 0x0400535F RID: 21343
		<SerializeField()>
		Protected m_enableKerning As Boolean

		' Token: 0x04005360 RID: 21344
		<SerializeField()>
		Protected m_enableExtraPadding As Boolean

		' Token: 0x04005361 RID: 21345
		<SerializeField()>
		Protected checkPaddingRequired As Boolean

		' Token: 0x04005362 RID: 21346
		<SerializeField()>
		Protected m_isRichText As Boolean = True

		' Token: 0x04005363 RID: 21347
		Protected m_parseCtrlCharacters As Boolean = True

		' Token: 0x04005364 RID: 21348
		Protected m_isOverlay As Boolean

		' Token: 0x04005365 RID: 21349
		<SerializeField()>
		Protected m_isOrthographic As Boolean

		' Token: 0x04005366 RID: 21350
		<SerializeField()>
		Protected m_isCullingEnabled As Boolean

		' Token: 0x04005367 RID: 21351
		<SerializeField()>
		Protected m_ignoreCulling As Boolean = True

		' Token: 0x04005368 RID: 21352
		<SerializeField()>
		Protected m_horizontalMapping As TextureMappingOptions

		' Token: 0x04005369 RID: 21353
		<SerializeField()>
		Protected m_verticalMapping As TextureMappingOptions

		' Token: 0x0400536A RID: 21354
		Protected m_renderMode As TextRenderFlags = TextRenderFlags.Render

		' Token: 0x0400536B RID: 21355
		Protected m_maxVisibleCharacters As Integer = 99999

		' Token: 0x0400536C RID: 21356
		Protected m_maxVisibleWords As Integer = 99999

		' Token: 0x0400536D RID: 21357
		Protected m_maxVisibleLines As Integer = 99999

		' Token: 0x0400536E RID: 21358
		<SerializeField()>
		Protected m_pageToDisplay As Integer = 1

		' Token: 0x0400536F RID: 21359
		Protected m_isNewPage As Boolean

		' Token: 0x04005370 RID: 21360
		<SerializeField()>
		Protected m_margin As Vector4 = New Vector4(0F, 0F, 0F, 0F)

		' Token: 0x04005371 RID: 21361
		Protected m_marginLeft As Single

		' Token: 0x04005372 RID: 21362
		Protected m_marginRight As Single

		' Token: 0x04005373 RID: 21363
		Protected m_marginWidth As Single

		' Token: 0x04005374 RID: 21364
		Protected m_marginHeight As Single

		' Token: 0x04005375 RID: 21365
		Protected m_width As Single = -1F

		' Token: 0x04005376 RID: 21366
		<SerializeField()>
		Protected m_textInfo As TMP_TextInfo

		' Token: 0x04005377 RID: 21367
		<SerializeField()>
		Protected m_havePropertiesChanged As Boolean

		' Token: 0x04005378 RID: 21368
		<SerializeField()>
		Protected m_isUsingLegacyAnimationComponent As Boolean

		' Token: 0x04005379 RID: 21369
		Protected m_transform As Transform

		' Token: 0x0400537A RID: 21370
		Protected m_rectTransform As RectTransform

		' Token: 0x0400537C RID: 21372
		Protected m_mesh As Mesh

		' Token: 0x0400537E RID: 21374
		Protected m_flexibleHeight As Single = -1F

		' Token: 0x0400537F RID: 21375
		Protected m_flexibleWidth As Single = -1F

		' Token: 0x04005380 RID: 21376
		Protected m_minHeight As Single

		' Token: 0x04005381 RID: 21377
		Protected m_minWidth As Single

		' Token: 0x04005382 RID: 21378
		Protected m_preferredWidth As Single = 9999F

		' Token: 0x04005383 RID: 21379
		Protected m_renderedWidth As Single

		' Token: 0x04005384 RID: 21380
		Protected m_preferredHeight As Single = 9999F

		' Token: 0x04005385 RID: 21381
		Protected m_renderedHeight As Single

		' Token: 0x04005386 RID: 21382
		Protected m_layoutPriority As Integer

		' Token: 0x04005387 RID: 21383
		Protected m_isCalculateSizeRequired As Boolean

		' Token: 0x04005388 RID: 21384
		Protected m_isLayoutDirty As Boolean

		' Token: 0x04005389 RID: 21385
		Protected m_verticesAlreadyDirty As Boolean

		' Token: 0x0400538A RID: 21386
		Protected m_layoutAlreadyDirty As Boolean

		' Token: 0x0400538B RID: 21387
		<SerializeField()>
		Protected m_isInputParsingRequired As Boolean

		' Token: 0x0400538C RID: 21388
		<SerializeField()>
		Protected m_isRightToLeft As Boolean

		' Token: 0x0400538D RID: 21389
		<SerializeField()>
		Protected m_inputSource As TMP_Text.TextInputSources

		' Token: 0x0400538E RID: 21390
		Protected old_text As String

		' Token: 0x0400538F RID: 21391
		Protected old_arg0 As Single

		' Token: 0x04005390 RID: 21392
		Protected old_arg1 As Single

		' Token: 0x04005391 RID: 21393
		Protected old_arg2 As Single

		' Token: 0x04005392 RID: 21394
		Protected m_fontScale As Single

		' Token: 0x04005393 RID: 21395
		Protected m_fontScaleMultiplier As Single

		' Token: 0x04005394 RID: 21396
		Protected m_htmlTag As Char() = New Char(63) {}

		' Token: 0x04005395 RID: 21397
		Protected m_xmlAttribute As XML_TagAttribute() = New XML_TagAttribute(7) {}

		' Token: 0x04005396 RID: 21398
		Protected tag_LineIndent As Single

		' Token: 0x04005397 RID: 21399
		Protected tag_Indent As Single

		' Token: 0x04005398 RID: 21400
		Protected m_indentStack As TMP_XmlTagStack(Of Single) = New TMP_XmlTagStack(Of Single)(New Single(15) {})

		' Token: 0x04005399 RID: 21401
		Protected tag_NoParsing As Boolean

		' Token: 0x0400539A RID: 21402
		Protected m_isParsingText As Boolean

		' Token: 0x0400539B RID: 21403
		Protected m_char_buffer As Integer()

		' Token: 0x0400539C RID: 21404
		Private m_internalCharacterInfo As TMP_CharacterInfo()

		' Token: 0x0400539D RID: 21405
		Protected m_input_CharArray As Char() = New Char(255) {}

		' Token: 0x0400539E RID: 21406
		Private m_charArray_Length As Integer

		' Token: 0x0400539F RID: 21407
		Protected m_totalCharacterCount As Integer

		' Token: 0x040053A0 RID: 21408
		Protected m_characterCount As Integer

		' Token: 0x040053A1 RID: 21409
		Protected m_visibleCharacterCount As Integer

		' Token: 0x040053A2 RID: 21410
		Protected m_visibleSpriteCount As Integer

		' Token: 0x040053A3 RID: 21411
		Protected m_firstCharacterOfLine As Integer

		' Token: 0x040053A4 RID: 21412
		Protected m_firstVisibleCharacterOfLine As Integer

		' Token: 0x040053A5 RID: 21413
		Protected m_lastCharacterOfLine As Integer

		' Token: 0x040053A6 RID: 21414
		Protected m_lastVisibleCharacterOfLine As Integer

		' Token: 0x040053A7 RID: 21415
		Protected m_lineNumber As Integer

		' Token: 0x040053A8 RID: 21416
		Protected m_pageNumber As Integer

		' Token: 0x040053A9 RID: 21417
		Protected m_maxAscender As Single

		' Token: 0x040053AA RID: 21418
		Protected m_maxDescender As Single

		' Token: 0x040053AB RID: 21419
		Protected m_maxLineAscender As Single

		' Token: 0x040053AC RID: 21420
		Protected m_maxLineDescender As Single

		' Token: 0x040053AD RID: 21421
		Protected m_startOfLineAscender As Single

		' Token: 0x040053AE RID: 21422
		Protected m_lineOffset As Single

		' Token: 0x040053AF RID: 21423
		Protected m_meshExtents As Extents

		' Token: 0x040053B0 RID: 21424
		Protected m_htmlColor As Color32 = New Color(255F, 255F, 255F, 128F)

		' Token: 0x040053B1 RID: 21425
		Protected m_colorStack As TMP_XmlTagStack(Of Color32) = New TMP_XmlTagStack(Of Color32)(New Color32(15) {})

		' Token: 0x040053B2 RID: 21426
		Protected m_tabSpacing As Single

		' Token: 0x040053B3 RID: 21427
		Protected m_spacing As Single

		' Token: 0x040053B4 RID: 21428
		Protected IsRectTransformDriven As Boolean

		' Token: 0x040053B5 RID: 21429
		Protected m_styleStack As TMP_XmlTagStack(Of Integer) = New TMP_XmlTagStack(Of Integer)(New Integer(15) {})

		' Token: 0x040053B6 RID: 21430
		Protected m_actionStack As TMP_XmlTagStack(Of Integer) = New TMP_XmlTagStack(Of Integer)(New Integer(15) {})

		' Token: 0x040053B7 RID: 21431
		Protected m_padding As Single

		' Token: 0x040053B8 RID: 21432
		Protected m_baselineOffset As Single

		' Token: 0x040053B9 RID: 21433
		Protected m_xAdvance As Single

		' Token: 0x040053BA RID: 21434
		Protected m_textElementType As TMP_TextElementType

		' Token: 0x040053BB RID: 21435
		Protected m_cached_TextElement As TMP_TextElement

		' Token: 0x040053BC RID: 21436
		Protected m_cached_Underline_GlyphInfo As TMP_Glyph

		' Token: 0x040053BD RID: 21437
		Protected m_cached_Ellipsis_GlyphInfo As TMP_Glyph

		' Token: 0x040053BE RID: 21438
		Protected m_defaultSpriteAsset As TMP_SpriteAsset

		' Token: 0x040053BF RID: 21439
		Protected m_currentSpriteAsset As TMP_SpriteAsset

		' Token: 0x040053C0 RID: 21440
		Protected m_spriteCount As Integer

		' Token: 0x040053C1 RID: 21441
		Protected m_spriteIndex As Integer

		' Token: 0x040053C2 RID: 21442
		Protected m_inlineGraphics As InlineGraphicManager

		' Token: 0x040053C3 RID: 21443
		Private k_Power As Single() = New Single() { 0.5F, 0.05F, 0.005F, 0.0005F, 5E-05F, 5E-06F, 5E-07F, 5E-08F, 5E-09F, 5E-10F }

		' Token: 0x040053C4 RID: 21444
		Protected Shared k_InfinityVectorPositive As Vector2 = New Vector2(1000000F, 1000000F)

		' Token: 0x040053C5 RID: 21445
		Protected Shared k_InfinityVectorNegative As Vector2 = New Vector2(-1000000F, -1000000F)

		' Token: 0x02000C8B RID: 3211
		Protected Enum TextInputSources
			' Token: 0x040053C7 RID: 21447
			Text
			' Token: 0x040053C8 RID: 21448
			SetText
			' Token: 0x040053C9 RID: 21449
			SetCharArray
		End Enum
	End Class
End Namespace
