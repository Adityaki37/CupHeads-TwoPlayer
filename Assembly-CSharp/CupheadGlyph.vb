Imports System
Imports System.Collections.Generic
Imports System.Text
Imports UnityEngine
Imports UnityEngine.UI

' Token: 0x02000458 RID: 1112
Public Class CupheadGlyph
	Inherits MonoBehaviour

	' Token: 0x170002A5 RID: 677
	' (get) Token: 0x060010CF RID: 4303 RVA: 0x000A0E2C File Offset: 0x0009F22C
	Public ReadOnly Property preferredWidth As Single
		Get
			Return Mathf.Max(Me.glyphText.preferredWidth + Me.paddingText, Me.glyphChar.rectTransform.sizeDelta.y)
		End Get
	End Property

	' Token: 0x060010D0 RID: 4304 RVA: 0x000A0E68 File Offset: 0x0009F268
	Private Sub Awake()
		Me.initialFontSize = Me.glyphChar.fontSize
		Me.initialCharColor = Me.glyphChar.color
		Me.initialScale = MyBase.transform.localScale
		Me.initialCharWrapMode = Me.glyphChar.verticalOverflow
		If Me.platformGlyphType = CupheadGlyph.PlatformGlyphType.TutorialInstruction OrElse Me.platformGlyphType = CupheadGlyph.PlatformGlyphType.TutorialInstructionDescend OrElse Me.platformGlyphType = CupheadGlyph.PlatformGlyphType.Shop OrElse Me.platformGlyphType = CupheadGlyph.PlatformGlyphType.ShmupTutorial Then
			Me.initialCharMaterial = Me.glyphChar.material
		End If
	End Sub

	' Token: 0x060010D1 RID: 4305 RVA: 0x000A0EFA File Offset: 0x0009F2FA
	Private Sub Start()
		Me.Init()
		AddHandler PlayerManager.OnControlsChanged, AddressOf Me.OnControlsChanged
		AddHandler Localization.OnLanguageChangedEvent, AddressOf Me.OnLanguageChanged
	End Sub

	' Token: 0x060010D2 RID: 4306 RVA: 0x000A0F24 File Offset: 0x0009F324
	Private Sub OnLanguageChanged()
		Me.Init()
	End Sub

	' Token: 0x060010D3 RID: 4307 RVA: 0x000A0F2C File Offset: 0x0009F32C
	Private Sub OnControlsChanged()
		Me.Init()
	End Sub

	' Token: 0x060010D4 RID: 4308 RVA: 0x000A0F34 File Offset: 0x0009F334
	Public Sub Init()
		Dim translation As Localization.Translation = CupheadInput.InputDisplayForButton(Me.button, Me.rewiredPlayerId)
		Me.AlignDashInstructions(translation)
		Dim text As String = translation.text
		Dim flag As Boolean = text.Length > 1
		Me.glyphSymbolText.gameObject.SetActive(flag)
		Me.glyphText.gameObject.SetActive(flag)
		Me.glyphChar.gameObject.SetActive(Not flag)
		Me.glyphSymbolChar.gameObject.SetActive(Not flag)
		Me.glyphText.text = text
		Me.glyphChar.text = text
		Me.glyphText.font = If((translation.fonts Is Nothing), Localization.Instance.fonts(CInt(Localization.language))(29).font, translation.fonts.font)
		For i As Integer = 0 To Me.rectTransformTexts.Length - 1
			If flag Then
				Dim preferredWidth As Single = Me.preferredWidth
				If Me.maxSize > 0F AndAlso preferredWidth > Me.maxSize Then
					preferredWidth = Me.maxSize
				End If
				Me.rectTransformTexts(i).sizeDelta = New Vector2(preferredWidth, Me.rectTransformTexts(i).sizeDelta.y)
			Else
				Dim component As RectTransform = Me.glyphChar.GetComponent(Of RectTransform)()
				If component IsNot Nothing Then
					Dim bytes As Byte() = Encoding.ASCII.GetBytes(text)
					If bytes.Length > 0 Then
						Dim num As Integer = CInt((bytes(0) - 65))
						If Me.letterOffset = CupheadGlyph.LetterOffset.Normal Then
							If num >= 0 AndAlso num < CupheadGlyph.letterSpecificOffset.Length Then
								component.anchoredPosition = CupheadGlyph.letterSpecificOffset(num)
							Else
								num = Me.PS4CharToIndex(CChar(bytes(0)))
								If num >= 0 Then
									component.anchoredPosition = Me.ps4NormalOffset(num)
								End If
							End If
						ElseIf num >= 0 AndAlso num < CupheadGlyph.letterSpecificSmallOffset.Length Then
							component.anchoredPosition = CupheadGlyph.letterSpecificSmallOffset(num)
						Else
							num = Me.PS4CharToIndex(CChar(bytes(0)))
							If num >= 0 Then
								component.anchoredPosition = CupheadGlyph.ps4SmallOffset(num)
							End If
						End If
					End If
				End If
				Me.rectTransformTexts(i).sizeDelta = New Vector2(Mathf.Max(Me.preferredWidth, Me.rectTransformTexts(i).sizeDelta.y), Me.rectTransformTexts(i).sizeDelta.y)
			End If
		Next
		Dim component2 As LayoutElement = MyBase.GetComponent(Of LayoutElement)()
		If component2 IsNot Nothing Then
			component2.preferredWidth = If((Not flag), (Me.preferredWidth - Me.paddingText), Me.preferredWidth)
		End If
		If flag AndAlso Me.maxSize > 0F Then
			Me.glyphText.resizeTextMaxSize = Me.glyphText.fontSize * 4
			Me.glyphText.resizeTextForBestFit = True
			Dim component3 As RectTransform = Me.glyphText.GetComponent(Of RectTransform)()
			component3.sizeDelta *= 4F
			component3.localScale = Vector3.one * 0.25F
		End If
	End Sub

	' Token: 0x060010D5 RID: 4309 RVA: 0x000A1294 File Offset: 0x0009F694
	Private Sub OnDestroy()
		RemoveHandler PlayerManager.OnControlsChanged, AddressOf Me.OnControlsChanged
		RemoveHandler Localization.OnLanguageChangedEvent, AddressOf Me.OnLanguageChanged
	End Sub

	' Token: 0x060010D6 RID: 4310 RVA: 0x000A12B8 File Offset: 0x0009F6B8
	Private Function PS4CharToIndex(c As Char) As Integer
		Return -1
	End Function

	' Token: 0x060010D7 RID: 4311 RVA: 0x000A12BB File Offset: 0x0009F6BB
	Private Function SwitchCharToIndex(c As Char) As Integer
		If c = CupheadGlyph.NintendoSwitchUp Then
			Return 0
		End If
		If c = CupheadGlyph.NintendoSwitchDown Then
			Return 1
		End If
		If c = CupheadGlyph.NintendoSwitchLeft Then
			Return 2
		End If
		If c = CupheadGlyph.NintendoSwitchRight Then
			Return 3
		End If
		Return -1
	End Function

	' Token: 0x060010D8 RID: 4312 RVA: 0x000A12F4 File Offset: 0x0009F6F4
	Private Sub SetSwitchGlyph(isSwitchGlyph As Boolean, rectTransform As RectTransform)
		If isSwitchGlyph Then
			Me.glyphSymbolChar.gameObject.SetActive(False)
			Me.glyphChar.fontSize = CupheadGlyph.NintendoSwitchFontSize
			Me.glyphChar.color = CupheadGlyph.NintendoSwitchColor
			Me.glyphChar.verticalOverflow = VerticalWrapMode.Overflow
			If Me.platformGlyphType = CupheadGlyph.PlatformGlyphType.TutorialInstruction OrElse Me.platformGlyphType = CupheadGlyph.PlatformGlyphType.TutorialInstructionDescend Then
				Me.glyphChar.material = Nothing
				Me.glyphChar.color = CupheadGlyph.NintendoSwitchTutorialInstructionColor
			ElseIf Me.platformGlyphType = CupheadGlyph.PlatformGlyphType.Shop OrElse Me.platformGlyphType = CupheadGlyph.PlatformGlyphType.ShmupTutorial Then
				Me.glyphChar.material = Nothing
			End If
			If Me.platformGlyphType = CupheadGlyph.PlatformGlyphType.SwitchWeapon Then
				MyBase.transform.localScale = Vector3.one
			End If
			If Me.platformGlyphType = CupheadGlyph.PlatformGlyphType.Equip Then
				Me.glyphChar.GetComponent(Of Shadow)().enabled = True
				Me.glyphChar.GetComponent(Of Outline)().enabled = True
			End If
			Dim vector As Vector2 = CupheadGlyph.SwitchOffsetMapping(CInt(Me.platformGlyphType))
			rectTransform.anchoredPosition = vector
		Else
			Me.glyphSymbolChar.gameObject.SetActive(True)
			Me.glyphChar.fontSize = Me.initialFontSize
			Me.glyphChar.color = Me.initialCharColor
			Me.glyphChar.verticalOverflow = Me.initialCharWrapMode
			If Me.platformGlyphType = CupheadGlyph.PlatformGlyphType.TutorialInstruction OrElse Me.platformGlyphType = CupheadGlyph.PlatformGlyphType.TutorialInstructionDescend OrElse Me.platformGlyphType = CupheadGlyph.PlatformGlyphType.Shop OrElse Me.platformGlyphType = CupheadGlyph.PlatformGlyphType.ShmupTutorial Then
				Me.glyphChar.material = Me.initialCharMaterial
			End If
			If Me.platformGlyphType = CupheadGlyph.PlatformGlyphType.SwitchWeapon Then
				MyBase.transform.localScale = Me.initialScale
			End If
			If Me.platformGlyphType = CupheadGlyph.PlatformGlyphType.Equip Then
				Me.glyphChar.GetComponent(Of Shadow)().enabled = False
				Me.glyphChar.GetComponent(Of Outline)().enabled = False
			End If
		End If
	End Sub

	' Token: 0x060010D9 RID: 4313 RVA: 0x000A14D8 File Offset: 0x0009F8D8
	Public Sub AlignDashInstructions(translation As Localization.Translation)
		If Me.glyphLayouts IsNot Nothing Then
			Dim flag As Boolean = Not translation.text.Equals("Y")
			For i As Integer = 0 To Me.glyphLayouts.Length - 1
				Me.glyphLayouts(i).enabled = flag
			Next
		End If
	End Sub

	' Token: 0x04001A17 RID: 6679
	Public Shared NintendoSwitchUp As Char = "{"c

	' Token: 0x04001A18 RID: 6680
	Public Shared NintendoSwitchDown As Char = "}"c

	' Token: 0x04001A19 RID: 6681
	Public Shared NintendoSwitchLeft As Char = "<"c

	' Token: 0x04001A1A RID: 6682
	Public Shared NintendoSwitchRight As Char = ">"c

	' Token: 0x04001A1B RID: 6683
	Public Shared PlayStation4Cross As Char = "†"c

	' Token: 0x04001A1C RID: 6684
	Public Shared PlayStation4Circle As Char = "‡"c

	' Token: 0x04001A1D RID: 6685
	Public Shared PlayStation4Square As Char = "°"c

	' Token: 0x04001A1E RID: 6686
	Public Shared PlayStation4Triangle As Char = "~"c

	' Token: 0x04001A1F RID: 6687
	Private Shared NintendoSwitchFontSize As Integer = 24

	' Token: 0x04001A20 RID: 6688
	Private Shared NintendoSwitchColor As Color = Color.white

	' Token: 0x04001A21 RID: 6689
	Private Shared NintendoSwitchTutorialInstructionColor As Color = New Color(0.25490198F, 0.25490198F, 0.25490198F, 1F)

	' Token: 0x04001A22 RID: 6690
	Private Shared letterSpecificOffset As Vector2() = New Vector2() { New Vector2(1.6F, -0.4F), New Vector2(1.29F, -0.85F), New Vector2(1.3F, -1F), New Vector2(1.81F, -1.18F), New Vector2(0.9F, -1F), New Vector2(0.9F, -1F), New Vector2(1.2F, -1F), New Vector2(1F, -1F), New Vector2(0.8F, -1.2F), New Vector2(1.3F, -1.2F), New Vector2(0.5F, -1.2F), New Vector2(1.1F, -1F), New Vector2(0.8F, -1F), New Vector2(1.2F, -1.2F), New Vector2(1.1F, -1F), New Vector2(1.3F, -1.2F), New Vector2(1.1F, 0F), New Vector2(1.5F, -1.2F), New Vector2(1.5F, -1.2F), New Vector2(1.3F, -1.8F), New Vector2(0.9F, -1.4F), New Vector2(1.35F, -1.6F), New Vector2(0.6F, -2F), New Vector2(0.8F, -1.3F), New Vector2(0.95F, -1.8F), New Vector2(1.6F, -1F) }

	' Token: 0x04001A23 RID: 6691
	Private Shared letterSpecificSmallOffset As Vector2() = New Vector2() { New Vector2(1.2F, 0F), New Vector2(0.5F, -0.2F), New Vector2(0.9F, -0.6F), New Vector2(1.1F, -0.3F), New Vector2(0.32F, -0.27F), New Vector2(0.32F, -0.85F), New Vector2(0.93F, -0.64F), New Vector2(0.64F, -0.56F), New Vector2(0.69F, -0.56F), New Vector2(0.53F, -0.38F), New Vector2(1.01F, -0.38F), New Vector2(0.77F, -0.19F), New Vector2(0.93F, -0.49F), New Vector2(0.79F, -0.67F), New Vector2(0.92F, -0.47F), New Vector2(1.34F, -0.44F), New Vector2(0.97F, 0.63F), New Vector2(1.01F, -0.3F), New Vector2(0.81F, -0.8F), New Vector2(0.48F, -1.02F), New Vector2(0.23F, -0.81F), New Vector2(0.44F, -0.81F), New Vector2(0.94F, -1.36F), New Vector2(1.19F, -0.73F), New Vector2(1.19F, -0.62F), New Vector2(0.89F, -0.62F) }

	' Token: 0x04001A24 RID: 6692
	Private Shared ps4SmallOffset As Vector2() = New Vector2() { New Vector2(0.69F, -0.48F), New Vector2(0.97F, -0.38F), New Vector2(0.91F, -0.34F), New Vector2(1.11F, 0.41F) }

	' Token: 0x04001A25 RID: 6693
	Protected ps4NormalOffset As Vector2() = New Vector2() { New Vector2(1.74F, -1F), New Vector2(2.27F, -0.97F), New Vector2(2.78F, -1.13F), New Vector2(1.55F, 0.55F) }

	' Token: 0x04001A26 RID: 6694
	Private Shared SwitchOffsetMapping As Dictionary(Of Integer, Vector2) = New Dictionary(Of Integer, Vector2)() From { { 0, New Vector2(-0.97F, -1F) }, { 1, New Vector2(0F, 9.06F) }, { 2, New Vector2(3.24F, 9.06F) }, { 3, New Vector2(-8.3F, -0.8F) }, { 4, New Vector2(0F, 9.06F) }, { 5, New Vector2(-1.61F, -1F) }, { 6, New Vector2(-0.97F, 9.06F) }, { 7, New Vector2(0F, 0F) }, { 8, New Vector2(0F, 6F) } }

	' Token: 0x04001A27 RID: 6695
	Private Shared PlayStation4OffsetMapping As Dictionary(Of Integer, Vector2) = New Dictionary(Of Integer, Vector2)() From { { 0, New Vector2(1.1F, -1F) }, { 1, New Vector2(0.8F, -0.4F) }, { 2, New Vector2(0.8F, -0.4F) }, { 3, New Vector2(1F, -1F) }, { 4, New Vector2(0.1F, -0.1F) }, { 5, New Vector2(0.3F, -0.35F) }, { 6, New Vector2(0.5F, -0.4F) }, { 7, New Vector2(0.5F, 0F) }, { 8, New Vector2(1.2F, -1.09F) } }

	' Token: 0x04001A28 RID: 6696
	Protected Const PADDINGH As Single = 25F

	' Token: 0x04001A29 RID: 6697
	Public rewiredPlayerId As Integer

	' Token: 0x04001A2A RID: 6698
	Public button As CupheadButton

	' Token: 0x04001A2B RID: 6699
	<SerializeField()>
	Private glyphSymbolText As Image

	' Token: 0x04001A2C RID: 6700
	<SerializeField()>
	Private glyphText As Text

	' Token: 0x04001A2D RID: 6701
	<SerializeField()>
	Private glyphSymbolChar As Image

	' Token: 0x04001A2E RID: 6702
	<SerializeField()>
	Private glyphChar As Text

	' Token: 0x04001A2F RID: 6703
	<SerializeField()>
	Private rectTransformTexts As RectTransform()

	' Token: 0x04001A30 RID: 6704
	<SerializeField()>
	Protected startSize As Vector2 = New Vector2(37F, 37F)

	' Token: 0x04001A31 RID: 6705
	<SerializeField()>
	Protected paddingText As Single = 10.7F

	' Token: 0x04001A32 RID: 6706
	<SerializeField()>
	Private maxSize As Single

	' Token: 0x04001A33 RID: 6707
	<SerializeField()>
	Private letterOffset As CupheadGlyph.LetterOffset

	' Token: 0x04001A34 RID: 6708
	<SerializeField()>
	Private platformGlyphType As CupheadGlyph.PlatformGlyphType

	' Token: 0x04001A35 RID: 6709
	<SerializeField()>
	Private glyphLayouts As CustomLanguageLayout()

	' Token: 0x04001A36 RID: 6710
	Private initialFontSize As Integer

	' Token: 0x04001A37 RID: 6711
	Private initialCharColor As Color

	' Token: 0x04001A38 RID: 6712
	Private initialScale As Vector3

	' Token: 0x04001A39 RID: 6713
	Private initialCharWrapMode As VerticalWrapMode

	' Token: 0x04001A3A RID: 6714
	Private initialCharMaterial As Material

	' Token: 0x02000459 RID: 1113
	Public Enum LetterOffset
		' Token: 0x04001A3C RID: 6716
		Normal
		' Token: 0x04001A3D RID: 6717
		Small
	End Enum

	' Token: 0x0200045A RID: 1114
	Public Enum PlatformGlyphType
		' Token: 0x04001A3F RID: 6719
		Normal
		' Token: 0x04001A40 RID: 6720
		TutorialInstruction
		' Token: 0x04001A41 RID: 6721
		TutorialInstructionDescend
		' Token: 0x04001A42 RID: 6722
		LevelUIInteractionDialogue
		' Token: 0x04001A43 RID: 6723
		Shop
		' Token: 0x04001A44 RID: 6724
		SwitchWeapon
		' Token: 0x04001A45 RID: 6725
		ShmupTutorial
		' Token: 0x04001A46 RID: 6726
		Equip
		' Token: 0x04001A47 RID: 6727
		OffsetPrompt
	End Enum
End Class
