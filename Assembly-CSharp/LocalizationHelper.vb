Imports System
Imports TMPro
Imports UnityEngine
Imports UnityEngine.U2D
Imports UnityEngine.UI

' Token: 0x0200091F RID: 2335
Public Class LocalizationHelper
	Inherits MonoBehaviour

	' Token: 0x0600369D RID: 13981 RVA: 0x001F9A50 File Offset: 0x001F7E50
	Private Sub Init()
		If Me.textComponent IsNot Nothing Then
			Me.initialFontSize = Me.textComponent.fontSize
		End If
		If Me.textMeshProComponent IsNot Nothing Then
			Me.initialFontAssetSize = Me.textMeshProComponent.fontSize
		End If
		Me.isInit = True
	End Sub

	' Token: 0x0600369E RID: 13982 RVA: 0x001F9AA8 File Offset: 0x001F7EA8
	Private Sub Awake()
		Me.platformOverride = MyBase.GetComponent(Of LocalizationHelperPlatformOverride)()
		Me.hasOverride = Me.platformOverride IsNot Nothing
	End Sub

	' Token: 0x0600369F RID: 13983 RVA: 0x001F9AC8 File Offset: 0x001F7EC8
	Private Sub Start()
		AddHandler Localization.OnLanguageChangedEvent, AddressOf Me.ApplyTranslation
	End Sub

	' Token: 0x060036A0 RID: 13984 RVA: 0x001F9ADB File Offset: 0x001F7EDB
	Private Sub OnDestroy()
		RemoveHandler Localization.OnLanguageChangedEvent, AddressOf Me.ApplyTranslation
	End Sub

	' Token: 0x060036A1 RID: 13985 RVA: 0x001F9AEE File Offset: 0x001F7EEE
	Private Sub OnEnable()
		Me.ApplyTranslation()
	End Sub

	' Token: 0x060036A2 RID: 13986 RVA: 0x001F9AF8 File Offset: 0x001F7EF8
	Public Sub ApplyTranslation()
		Dim num As Integer = Me.currentID
		Dim num2 As Integer
		If Me.hasOverride AndAlso Me.platformOverride.HasOverrideForCurrentPlatform(num2) Then
			num = num2
		End If
		Me.ApplyTranslation(Localization.Find(num))
	End Sub

	' Token: 0x060036A3 RID: 13987 RVA: 0x001F9B37 File Offset: 0x001F7F37
	Public Sub ApplyTranslation(translationElement As TranslationElement, Optional subTranslations As LocalizationHelper.LocalizationSubtext() = Nothing)
		Me.subTranslations = subTranslations
		Me.ApplyTranslation(translationElement)
	End Sub

	' Token: 0x060036A4 RID: 13988 RVA: 0x001F9B48 File Offset: 0x001F7F48
	Private Sub ApplyTranslation(translationElement As TranslationElement)
		If Not Me.isInit Then
			Me.Init()
		End If
		Me.currentLanguage = Localization.language
		If Me.currentLanguage = CType((-1), Localization.Languages) OrElse translationElement Is Nothing Then
			Return
		End If
		If String.IsNullOrEmpty(translationElement.key) Then
			Return
		End If
		Dim translation As Localization.Translation = translationElement.translation
		If String.IsNullOrEmpty(translation.text) Then
			translation = Localization.Translate(translationElement.key)
		End If
		Dim text As String = translation.text
		If text IsNot Nothing Then
			text = text.Replace("\n", vbLf)
		End If
		If text IsNot Nothing AndAlso text.Contains("{") AndAlso text.Contains("}") Then
			If Me.subTranslations IsNot Nothing Then
				Dim flag As Boolean = True
				While flag
					flag = False
					For i As Integer = 0 To Me.subTranslations.Length - 1
						If text.Contains("{" + Me.subTranslations(i).key + "}") Then
							flag = True
							If Me.subTranslations(i).dontTranslate Then
								text = text.Replace("{" + Me.subTranslations(i).key + "}", Me.subTranslations(i).value)
							Else
								Dim translation2 As Localization.Translation = Localization.Translate(Me.subTranslations(i).value)
								If String.IsNullOrEmpty(translation2.text) Then
									text = text.Replace("{" + Me.subTranslations(i).key + "}", Me.subTranslations(i).value)
								Else
									text = text.Replace("{" + Me.subTranslations(i).key + "}", translation2.text)
								End If
							End If
						End If
					Next
				End While
			End If
			Dim array As String() = text.Split(New Char() { "{"c })
			If array.Length > 1 Then
				Dim array2 As String() = array(1).Split(New Char() { "}"c })
				If array2.Length > 1 Then
					Dim text2 As String = array2(0)
					Dim translation3 As Localization.Translation = Localization.Translate(text2)
					If Not String.IsNullOrEmpty(translation3.text) Then
						text = text.Replace("{" + text2 + "}", translation3.text)
					End If
				End If
			End If
		End If
		If Me.textComponent IsNot Nothing Then
			Me.textComponent.text = text
			Me.textComponent.enabled = Not String.IsNullOrEmpty(text)
			If translation.hasCustomFont Then
				Me.textComponent.font = translation.fonts.font
			ElseIf Localization.Instance.fonts(CInt(Me.currentLanguage))(CInt(translationElement.category)).fontType <> FontLoader.FontType.None Then
				Me.textComponent.font = Localization.Instance.fonts(CInt(Me.currentLanguage))(CInt(translationElement.category)).font
			End If
			Me.textComponent.fontSize = If((translation.fonts.fontSize <= 0), Me.initialFontSize, translation.fonts.fontSize)
		End If
		If Me.textMeshProComponent IsNot Nothing Then
			Me.textMeshProComponent.text = text
			Me.textMeshProComponent.enabled = Not String.IsNullOrEmpty(text)
			Me.textMeshProComponent.characterSpacing = translation.fonts.charSpacing
			If translation.hasCustomFontAsset Then
				Me.textMeshProComponent.font = translation.fonts.fontAsset
			Else
				Me.textMeshProComponent.font = Localization.Instance.fonts(CInt(Me.currentLanguage))(CInt(translationElement.category)).fontAsset
			End If
			Me.textMeshProComponent.fontSize = If((translation.fonts.fontAssetSize <= 0F), Me.initialFontAssetSize, translation.fonts.fontAssetSize)
		End If
		If Me.spriteRendererComponent IsNot Nothing Then
			Dim sprite As Sprite
			If translation.hasSpriteAtlasImage Then
				Dim cachedAsset As SpriteAtlas = AssetLoader(Of SpriteAtlas).GetCachedAsset(translation.spriteAtlasName)
				sprite = cachedAsset.GetSprite(translation.spriteAtlasImageName)
			Else
				sprite = translation.image
			End If
			Me.spriteRendererComponent.sprite = sprite
			Me.spriteRendererComponent.enabled = False
			Me.spriteRendererComponent.enabled = sprite IsNot Nothing
		End If
		If Me.imageComponent IsNot Nothing Then
			Dim sprite2 As Sprite
			If translation.hasSpriteAtlasImage Then
				Dim cachedAsset2 As SpriteAtlas = AssetLoader(Of SpriteAtlas).GetCachedAsset(translation.spriteAtlasName)
				sprite2 = cachedAsset2.GetSprite(translation.spriteAtlasImageName)
			Else
				sprite2 = translation.image
			End If
			Me.imageComponent.sprite = sprite2
			Me.imageComponent.enabled = False
			Me.imageComponent.enabled = sprite2 IsNot Nothing
		End If
	End Sub

	' Token: 0x04003EC7 RID: 16071
	Public existingKey As Boolean

	' Token: 0x04003EC8 RID: 16072
	Public currentID As Integer = -1

	' Token: 0x04003EC9 RID: 16073
	Public currentLanguage As Localization.Languages = CType((-1), Localization.Languages)

	' Token: 0x04003ECA RID: 16074
	Public currentCategory As Localization.Categories

	' Token: 0x04003ECB RID: 16075
	Public currentCustomFont As Boolean

	' Token: 0x04003ECC RID: 16076
	Public textComponent As Text

	' Token: 0x04003ECD RID: 16077
	Public imageComponent As Image

	' Token: 0x04003ECE RID: 16078
	Public spriteRendererComponent As SpriteRenderer

	' Token: 0x04003ECF RID: 16079
	Public textMeshProComponent As TMP_Text

	' Token: 0x04003ED0 RID: 16080
	Private initialFontSize As Integer

	' Token: 0x04003ED1 RID: 16081
	Private initialFontAssetSize As Single

	' Token: 0x04003ED2 RID: 16082
	Private isInit As Boolean

	' Token: 0x04003ED3 RID: 16083
	Private subTranslations As LocalizationHelper.LocalizationSubtext()

	' Token: 0x04003ED4 RID: 16084
	Private hasOverride As Boolean

	' Token: 0x04003ED5 RID: 16085
	Private platformOverride As LocalizationHelperPlatformOverride

	' Token: 0x02000920 RID: 2336
	Public Structure LocalizationSubtext
		' Token: 0x060036A5 RID: 13989 RVA: 0x001FA075 File Offset: 0x001F8475
		Public Sub New(key As String, value As String, Optional dontTranslate As Boolean = False)
			Me.key = key
			Me.value = value
			Me.dontTranslate = dontTranslate
		End Sub

		' Token: 0x04003ED6 RID: 16086
		Public key As String

		' Token: 0x04003ED7 RID: 16087
		Public value As String

		' Token: 0x04003ED8 RID: 16088
		Public dontTranslate As Boolean
	End Structure
End Class
