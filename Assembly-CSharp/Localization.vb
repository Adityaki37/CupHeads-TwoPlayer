Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.IO
Imports System.Text
Imports TMPro
Imports UnityEngine

' Token: 0x02000918 RID: 2328
<CreateAssetMenu(fileName := "LocalizationAsset", menuName := "Localization Asset", order := 1)>
Public Class Localization
	Inherits ScriptableObject
	Implements ISerializationCallbackReceiver

	' Token: 0x17000467 RID: 1127
	' (get) Token: 0x06003679 RID: 13945 RVA: 0x001F8817 File Offset: 0x001F6C17
	Public Shared ReadOnly Property Instance As Localization
		Get
			If Localization._instance Is Nothing Then
				Localization._instance = Resources.Load(Of Localization)("LocalizationAsset")
			End If
			Return Localization._instance
		End Get
	End Property

	' Token: 0x14000065 RID: 101
	' (add) Token: 0x0600367A RID: 13946 RVA: 0x001F8840 File Offset: 0x001F6C40
	' (remove) Token: 0x0600367B RID: 13947 RVA: 0x001F8874 File Offset: 0x001F6C74
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Shared Event OnLanguageChangedEvent As Localization.LanguageChanged

	' Token: 0x17000468 RID: 1128
	' (get) Token: 0x0600367C RID: 13948 RVA: 0x001F88A8 File Offset: 0x001F6CA8
	' (set) Token: 0x0600367D RID: 13949 RVA: 0x001F88D3 File Offset: 0x001F6CD3
	Public Shared Property language As Localization.Languages
		Get
			If SettingsData.Data.language = -1 Then
				SettingsData.Data.language = CInt(DetectLanguage.GetDefaultLanguage())
			End If
			Return CType(SettingsData.Data.language, Localization.Languages)
		End Get
		Set(value As Localization.Languages)
			SettingsData.Data.language = CInt(value)
			If Localization.OnLanguageChangedEvent IsNot Nothing Then
				Localization.OnLanguageChangedEvent()
			End If
		End Set
	End Property

	' Token: 0x0600367E RID: 13950 RVA: 0x001F88F4 File Offset: 0x001F6CF4
	Public Shared Function Translate(key As String) As Localization.Translation
		Dim num As Integer
		If Parser.IntTryParse(key, num) Then
			Return Localization.Translate(num)
		End If
		Dim translation As Localization.Translation = Nothing
		For i As Integer = 0 To Localization.Instance.m_TranslationElements.Count - 1
			If Localization._instance.m_TranslationElements(i).key = key Then
				Dim translationElement As TranslationElement = Localization._instance.m_TranslationElements(i)
				translation = translationElement.translation
			End If
		Next
		Return translation
	End Function

	' Token: 0x0600367F RID: 13951 RVA: 0x001F8978 File Offset: 0x001F6D78
	Public Shared Function Translate(id As Integer) As Localization.Translation
		Dim translation As Localization.Translation = Nothing
		For i As Integer = 0 To Localization.Instance.m_TranslationElements.Count - 1
			If Localization._instance.m_TranslationElements(i).id = id Then
				Dim translationElement As TranslationElement = Localization._instance.m_TranslationElements(i)
				translation = translationElement.translation
			End If
		Next
		Return translation
	End Function

	' Token: 0x06003680 RID: 13952 RVA: 0x001F89E4 File Offset: 0x001F6DE4
	Public Shared Function Find(key As String) As TranslationElement
		For i As Integer = 0 To Localization.Instance.m_TranslationElements.Count - 1
			If Localization._instance.m_TranslationElements(i).key = key Then
				Return Localization._instance.m_TranslationElements(i)
			End If
		Next
		Return Nothing
	End Function

	' Token: 0x06003681 RID: 13953 RVA: 0x001F8A44 File Offset: 0x001F6E44
	Public Shared Function Find(id As Integer) As TranslationElement
		For i As Integer = 0 To Localization.Instance.m_TranslationElements.Count - 1
			If Localization._instance.m_TranslationElements(i).id = id Then
				Return Localization._instance.m_TranslationElements(i)
			End If
		Next
		Return Nothing
	End Function

	' Token: 0x06003682 RID: 13954 RVA: 0x001F8AA0 File Offset: 0x001F6EA0
	Public Shared Sub ExportCsv(path As String)
		Dim text As String = "|lang|"
		Dim text2 As String = "|lang|_cuphead"
		Dim text3 As String = "|lang|_mugman"
		Dim c As Char = "@"c
		Dim text4 As String = vbCrLf
		Dim stringBuilder As StringBuilder = New StringBuilder()
		Dim num As Integer = [Enum].GetNames(GetType(Localization.Languages)).Length
		Dim num2 As Integer = [Enum].GetNames(GetType(Localization.Categories)).Length
		For i As Integer = 0 To Localization.csvKeys.Length - 1
			If Localization.csvKeys(i).Contains(text) Then
				Dim text5 As String = Localization.csvKeys(i).Replace(text, String.Empty)
				For j As Integer = 0 To num - 1
					If i > 0 Then
						stringBuilder.Append(c)
					End If
					Dim stringBuilder2 As StringBuilder = stringBuilder
					Dim languages As Localization.Languages = CType(j, Localization.Languages)
					stringBuilder2.Append(languages.ToString())
					stringBuilder.Append(text5)
				Next
			Else
				If i > 0 Then
					stringBuilder.Append(c)
				End If
				stringBuilder.Append(Localization.csvKeys(i))
			End If
		Next
		stringBuilder.Append(text4)
		Dim text6 As String = String.Empty
		For k As Integer = 0 To Localization.Instance.m_TranslationElements.Count - 1
			Dim translationElement As TranslationElement = Localization._instance.m_TranslationElements(k)
			If translationElement.depth <> -1 Then
				For l As Integer = 0 To Localization.csvKeys.Length - 1
					If Localization.csvKeys(l).Contains(text) Then
						For m As Integer = 0 To num - 1
							If l > 0 Then
								stringBuilder.Append(c)
							End If
							text6 = String.Empty
							Dim text7 As String
							Dim translation As Localization.Translation
							If Localization.csvKeys(l).Contains(text2) Then
								text7 = Localization.csvKeys(l).Replace(text2, String.Empty)
								If translationElement.translationsCuphead Is Nothing OrElse translationElement.translationsCuphead.Length = 0 Then
									translation = Nothing
								Else
									translation = translationElement.translationsCuphead(m)
								End If
							ElseIf Localization.csvKeys(l).Contains(text3) Then
								text7 = Localization.csvKeys(l).Replace(text3, String.Empty)
								If translationElement.translationsMugman Is Nothing OrElse translationElement.translationsMugman.Length = 0 Then
									translation = Nothing
								Else
									translation = translationElement.translationsMugman(m)
								End If
							Else
								text7 = Localization.csvKeys(l).Replace(text, String.Empty)
								translation = translationElement.translations(m)
							End If
							If text7 = "_text" Then
								text6 = translation.text
								If Not String.IsNullOrEmpty(text6) Then
									text6 = text6.Replace(vbLf.ToString(), "\"c + "n")
								End If
							ElseIf text7 = "_image" Then
								If translation.image IsNot Nothing Then
									text6 = translation.image.name
								End If
							ElseIf text7 = "_spriteAtlasName" Then
								text6 = translation.spriteAtlasName
							ElseIf text7 = "_spriteAtlasImageName" Then
								text6 = translation.spriteAtlasImageName
							ElseIf text7 = "_font" Then
								If translation.fonts.fontType <> FontLoader.FontType.None Then
									text6 = FontLoader.GetFilename(translation.fonts.fontType)
								End If
							ElseIf text7 = "_fontSize" Then
								If translation.fonts.fontSize > 0 Then
									text6 = translation.fonts.fontSize.ToString()
								Else
									text6 = String.Empty
								End If
							ElseIf text7 = "_fontAsset" Then
								If translation.fonts.tmpFontType <> FontLoader.TMPFontType.None Then
									text6 = FontLoader.GetFilename(translation.fonts.tmpFontType)
								End If
							ElseIf text7 = "_fontAssetSize" Then
								If translation.fonts.fontAssetSize > 0F Then
									text6 = translation.fonts.fontAssetSize.ToString()
								Else
									text6 = String.Empty
								End If
							End If
							If text6 IsNot Nothing Then
								stringBuilder.Append(text6)
							End If
						Next
					Else
						If l > 0 Then
							stringBuilder.Append(c)
						End If
						text6 = String.Empty
						Dim text7 As String = Localization.csvKeys(l)
						If text7 = "id" Then
							text6 = translationElement.id.ToString()
						ElseIf text7 = "key" Then
							text6 = translationElement.key
						ElseIf text7 = "category" Then
							text6 = translationElement.category.ToString()
						ElseIf text7 = "description" Then
							text6 = translationElement.description
						End If
						If text6 IsNot Nothing Then
							stringBuilder.Append(text6)
						End If
					End If
				Next
				stringBuilder.Append(text4)
			End If
		Next
		Dim encoding As Encoding = New UTF8Encoding(True)
		Dim bytes As Byte() = encoding.GetBytes(stringBuilder.ToString())
		Dim fileStream As FileStream = New FileStream(path, FileMode.Create)
		Dim preamble As Byte() = encoding.GetPreamble()
		fileStream.Write(preamble, 0, preamble.Length)
		fileStream.Write(bytes, 0, bytes.Length)
		fileStream.Dispose()
	End Sub

	' Token: 0x06003683 RID: 13955 RVA: 0x001F9068 File Offset: 0x001F7468
	Public Shared Sub ImportCsv(path As String)
		Dim c As Char = "@"c
		Dim text As String = vbCrLf
		Dim encoding As Encoding = New UTF8Encoding(True)
		Dim fileStream As FileStream = New FileStream(path, FileMode.Open)
		Dim preamble As Byte() = encoding.GetPreamble()
		Dim array As Byte() = New Byte(preamble.Length - 1) {}
		fileStream.Read(array, 0, preamble.Length)
		Dim flag As Boolean = True
		For i As Integer = 0 To preamble.Length - 1
			If preamble(i) <> array(i) Then
				flag = False
				Exit For
			End If
		Next
		If flag Then
			array = New Byte(fileStream.Length - CLng(preamble.Length) - 1) {}
			fileStream.Read(array, 0, array.Length)
		Else
			array = New Byte(fileStream.Length - 1) {}
			fileStream.Position = 0L
			fileStream.Read(array, 0, CInt(fileStream.Length))
		End If
		fileStream.Dispose()
		Dim [string] As String = encoding.GetString(array)
		Dim array2 As String() = [string].Split(New String() { text }, StringSplitOptions.RemoveEmptyEntries)
		Dim array3 As String() = array2(0).Split(New Char() { c })
		Localization.processImportedLines(array3, array2, c)
	End Sub

	' Token: 0x06003684 RID: 13956 RVA: 0x001F917C File Offset: 0x001F757C
	Private Shared Sub processImportedLines(headers As String(), lines As String(), separator As Char)
		Dim text As String = "_cuphead"
		Dim text2 As String = "_mugman"
		Dim names As String() = [Enum].GetNames(GetType(Localization.Languages))
		Dim names2 As String() = [Enum].GetNames(GetType(Localization.Categories))
		Dim dictionary As Dictionary(Of String, Font) = New Dictionary(Of String, Font)()
		Dim dictionary2 As Dictionary(Of String, TMP_FontAsset) = New Dictionary(Of String, TMP_FontAsset)()
		Localization.Instance.m_TranslationElements.Clear()
		Dim translationElement As TranslationElement = New TranslationElement("Root", -1, 0)
		Localization._instance.m_TranslationElements.Add(translationElement)
		For i As Integer = 1 To lines.Length - 1
			Dim array As String() = lines(i).Split(New Char() { separator })
			If array.Length <> headers.Length Then
				If lines(i) <> String.Empty Then
				End If
			Else
				translationElement = Localization.Instance.AddKey()
				For j As Integer = 0 To array.Length - 1
					If Not String.IsNullOrEmpty(array(j)) Then
						Dim text3 As String = headers(j)
						If text3 = "id" Then
							translationElement.id = Parser.IntParse(array(j))
						ElseIf text3 = "key" Then
							translationElement.key = array(j)
						ElseIf text3 = "category" Then
							Dim num As Integer = -1
							For k As Integer = 0 To names2.Length - 1
								If names2(k) = array(j) Then
									num = k
								End If
							Next
							translationElement.category = CType(num, Localization.Categories)
						ElseIf text3 = "description" Then
							translationElement.description = array(j)
						Else
							For l As Integer = 0 To names.Length - 1
								If text3.Contains(names(l)) Then
									text3 = text3.Replace(names(l), String.Empty)
									Dim flag As Boolean = False
									Dim flag2 As Boolean = False
									Dim translation As Localization.Translation
									If text3.Contains(text) Then
										flag = True
										text3 = text3.Replace(text, String.Empty)
										If translationElement.translationsCuphead Is Nothing OrElse translationElement.translationsCuphead.Length = 0 Then
											translationElement.translationsCuphead = New Localization.Translation(names.Length - 1) {}
											translationElement.translationsMugman = New Localization.Translation(names.Length - 1) {}
										End If
										translation = translationElement.translationsCuphead(l)
									ElseIf text3.Contains(text2) Then
										flag2 = True
										text3 = text3.Replace(text2, String.Empty)
										If translationElement.translationsCuphead Is Nothing OrElse translationElement.translationsCuphead.Length = 0 Then
											translationElement.translationsCuphead = New Localization.Translation(names.Length - 1) {}
											translationElement.translationsMugman = New Localization.Translation(names.Length - 1) {}
										End If
										translation = translationElement.translationsMugman(l)
									Else
										translation = translationElement.translations(l)
									End If
									If translation.fonts Is Nothing Then
										translation.fonts = New Localization.CategoryLanguageFont()
									End If
									If text3 = "_text" Then
										translation.text = array(j)
									ElseIf text3 = "_image" Then
										If String.IsNullOrEmpty(array(j)) Then
											Exit For
										End If
									ElseIf text3 = "_spriteAtlasName" Then
										translation.spriteAtlasName = array(j)
									ElseIf text3 = "_spriteAtlasImageName" Then
										translation.spriteAtlasImageName = array(j)
									ElseIf text3 = "_font" Then
										If String.IsNullOrEmpty(array(j)) Then
											Exit For
										End If
									ElseIf text3 = "_fontSize" Then
										If Not String.IsNullOrEmpty(array(j)) Then
											Dim num2 As Integer = Convert.ToInt32(array(j))
											If num2 = 0 Then
												Exit For
											End If
											translation.fonts.fontSize = num2
										End If
									ElseIf text3 = "_fontAsset" Then
										If String.IsNullOrEmpty(array(j)) Then
											Exit For
										End If
									ElseIf text3 = "_fontAssetSize" AndAlso Not String.IsNullOrEmpty(array(j)) Then
										Dim num3 As Single = Convert.ToSingle(array(j))
										If num3 = 0F Then
											Exit For
										End If
										translation.fonts.fontAssetSize = num3
									End If
									If flag Then
										translationElement.translationsCuphead(l) = translation
									ElseIf flag2 Then
										translationElement.translationsMugman(l) = translation
									Else
										translationElement.translations(l) = translation
									End If
									Exit For
								End If
							Next
						End If
					End If
				Next
			End If
		Next
		If Localization.OnLanguageChangedEvent IsNot Nothing Then
			Localization.OnLanguageChangedEvent()
		End If
	End Sub

	' Token: 0x17000469 RID: 1129
	' (get) Token: 0x06003685 RID: 13957 RVA: 0x001F9678 File Offset: 0x001F7A78
	' (set) Token: 0x06003686 RID: 13958 RVA: 0x001F96EF File Offset: 0x001F7AEF
	<SerializeField()>
	Public Property fonts As Localization.CategoryLanguageFonts()
		Get
			If Me.m_Fonts Is Nothing Then
				Dim num As Integer = [Enum].GetNames(GetType(Localization.Languages)).Length
				Dim num2 As Integer = [Enum].GetNames(GetType(Localization.Categories)).Length
				Me.m_Fonts = New Localization.CategoryLanguageFonts(num - 1) {}
				For i As Integer = 0 To num - 1
					Me.m_Fonts(i).fonts = New Localization.CategoryLanguageFont(num2 - 1) {}
				Next
			End If
			Return Me.m_Fonts
		End Get
		Set(value As Localization.CategoryLanguageFonts())
			Me.m_Fonts = value
		End Set
	End Property

	' Token: 0x1700046A RID: 1130
	' (get) Token: 0x06003687 RID: 13959 RVA: 0x001F96F8 File Offset: 0x001F7AF8
	' (set) Token: 0x06003688 RID: 13960 RVA: 0x001F9700 File Offset: 0x001F7B00
	<SerializeField()>
	Public Property translationElements As List(Of TranslationElement)
		Get
			Return Me.m_TranslationElements
		End Get
		Set(value As List(Of TranslationElement))
			Me.m_TranslationElements = value
		End Set
	End Property

	' Token: 0x06003689 RID: 13961 RVA: 0x001F970C File Offset: 0x001F7B0C
	Public Function AddKey() As TranslationElement
		Dim num As Integer = -1
		For i As Integer = 0 To Me.m_TranslationElements.Count - 1
			If Me.m_TranslationElements(i).id > num Then
				num = Me.m_TranslationElements(i).id
			End If
		Next
		num += 1
		Dim translationElement As TranslationElement = New TranslationElement("Key" + num, Localization.Categories.NoCategory, String.Empty, String.Empty, String.Empty, 0, num)
		Me.m_TranslationElements.Add(translationElement)
		Return translationElement
	End Function

	' Token: 0x0600368A RID: 13962 RVA: 0x001F979C File Offset: 0x001F7B9C
	Private Sub Awake()
		If Me.m_TranslationElements.Count = 0 Then
			Me.m_TranslationElements = New List(Of TranslationElement)(1)
			Dim translationElement As TranslationElement = New TranslationElement("Root", -1, 0)
			Me.m_TranslationElements.Add(translationElement)
		End If
	End Sub

	' Token: 0x0600368B RID: 13963 RVA: 0x001F97DE File Offset: 0x001F7BDE
	Public Sub OnBeforeSerialize() Implements UnityEngine.ISerializationCallbackReceiver.OnBeforeSerialize
	End Sub

	' Token: 0x0600368C RID: 13964 RVA: 0x001F97E0 File Offset: 0x001F7BE0
	Public Sub OnAfterDeserialize() Implements UnityEngine.ISerializationCallbackReceiver.OnAfterDeserialize
		Dim flag As Boolean = False
		Dim num As Integer = [Enum].GetNames(GetType(Localization.Languages)).Length
		If Me.fonts.Length < num Then
			flag = True
		End If
		Dim num2 As Integer = [Enum].GetNames(GetType(Localization.Categories)).Length
		If Me.fonts(0).fonts.Length < num2 Then
			flag = True
		End If
		If flag Then
			Me.fonts = Me.GrowFonts(Me.fonts, num, num2)
		End If
	End Sub

	' Token: 0x0600368D RID: 13965 RVA: 0x001F9858 File Offset: 0x001F7C58
	Private Function GrowFonts(oldFonts As Localization.CategoryLanguageFonts(), newLanguagesLength As Integer, newCategoriesLength As Integer) As Localization.CategoryLanguageFonts()
		Dim array As Localization.CategoryLanguageFonts() = New Localization.CategoryLanguageFonts(newLanguagesLength - 1) {}
		For i As Integer = 0 To newLanguagesLength - 1
			array(i).fonts = New Localization.CategoryLanguageFont(newCategoriesLength - 1) {}
		Next
		For j As Integer = 0 To oldFonts.Length - 1
			For k As Integer = 0 To oldFonts(j).fonts.Length - 1
				array(j)(k) = oldFonts(j)(k)
			Next
		Next
		Return array
	End Function

	' Token: 0x04003E77 RID: 15991
	Public Const LanguagesEnumSize As Integer = 12

	' Token: 0x04003E78 RID: 15992
	Public Const PATH As String = "LocalizationAsset"

	' Token: 0x04003E79 RID: 15993
	Private Shared csvKeys As String() = New String() { "id", "key", "category", "description", "|lang|_text", "|lang|_cuphead_text", "|lang|_mugman_text", "|lang|_image", "|lang|_spriteAtlasName", "|lang|_spriteAtlasImageName", "|lang|_cuphead_image", "|lang|_mugman_image", "|lang|_font", "|lang|_fontSize", "|lang|_fontAsset", "|lang|_fontAssetSize" }

	' Token: 0x04003E7A RID: 15994
	Private Shared _instance As Localization

	' Token: 0x04003E7C RID: 15996
	Public Shared language1 As Localization.Languages = Localization.Languages.English

	' Token: 0x04003E7D RID: 15997
	Public Shared language2 As Localization.Languages = Localization.Languages.French

	' Token: 0x04003E7E RID: 15998
	<SerializeField()>
	Private m_TranslationElements As List(Of TranslationElement) = New List(Of TranslationElement)()

	' Token: 0x04003E7F RID: 15999
	<SerializeField()>
	Public m_Fonts As Localization.CategoryLanguageFonts()

	' Token: 0x02000919 RID: 2329
	<SerializeField()>
	Public Enum Languages
		' Token: 0x04003E81 RID: 16001
		English
		' Token: 0x04003E82 RID: 16002
		French
		' Token: 0x04003E83 RID: 16003
		Italian
		' Token: 0x04003E84 RID: 16004
		German
		' Token: 0x04003E85 RID: 16005
		SpanishSpain
		' Token: 0x04003E86 RID: 16006
		SpanishAmerica
		' Token: 0x04003E87 RID: 16007
		Korean
		' Token: 0x04003E88 RID: 16008
		Russian
		' Token: 0x04003E89 RID: 16009
		Polish
		' Token: 0x04003E8A RID: 16010
		PortugueseBrazil
		' Token: 0x04003E8B RID: 16011
		Japanese
		' Token: 0x04003E8C RID: 16012
		SimplifiedChinese
	End Enum

	' Token: 0x0200091A RID: 2330
	<SerializeField()>
	Public Enum Categories
		' Token: 0x04003E8E RID: 16014
		NoCategory
		' Token: 0x04003E8F RID: 16015
		LevelSelectionName
		' Token: 0x04003E90 RID: 16016
		LevelSelectionIn
		' Token: 0x04003E91 RID: 16017
		LevelSelectionStage
		' Token: 0x04003E92 RID: 16018
		LevelSelectionDifficultyHeader
		' Token: 0x04003E93 RID: 16019
		LevelSelectionDifficultys
		' Token: 0x04003E94 RID: 16020
		EquipCategoryNames
		' Token: 0x04003E95 RID: 16021
		EquipWeaponNames
		' Token: 0x04003E96 RID: 16022
		EquipCategoryBackName
		' Token: 0x04003E97 RID: 16023
		EquipCategoryBackTitle
		' Token: 0x04003E98 RID: 16024
		EquipCategoryBackSubtitle
		' Token: 0x04003E99 RID: 16025
		EquipCategoryBackDescription
		' Token: 0x04003E9A RID: 16026
		ChecklistTitle
		' Token: 0x04003E9B RID: 16027
		ChecklistWorldNames
		' Token: 0x04003E9C RID: 16028
		ChecklistContractHeaders
		' Token: 0x04003E9D RID: 16029
		ChecklistContracts
		' Token: 0x04003E9E RID: 16030
		PauseMenuItems
		' Token: 0x04003E9F RID: 16031
		DeathMenuQuote
		' Token: 0x04003EA0 RID: 16032
		DeathMenuItems
		' Token: 0x04003EA1 RID: 16033
		ResultsMenuTitle
		' Token: 0x04003EA2 RID: 16034
		ResultsMenuCategories
		' Token: 0x04003EA3 RID: 16035
		ResultsMenuGrade
		' Token: 0x04003EA4 RID: 16036
		ResultsMenuNewRecord
		' Token: 0x04003EA5 RID: 16037
		ResultsMenuTryNormal
		' Token: 0x04003EA6 RID: 16038
		IntroEndingText
		' Token: 0x04003EA7 RID: 16039
		IntroEndingAction
		' Token: 0x04003EA8 RID: 16040
		CutScenesText
		' Token: 0x04003EA9 RID: 16041
		SpeechBalloons
		' Token: 0x04003EAA RID: 16042
		WorldMapTitles
		' Token: 0x04003EAB RID: 16043
		Glyphs
		' Token: 0x04003EAC RID: 16044
		TitleScreenSelection
		' Token: 0x04003EAD RID: 16045
		Notifications
		' Token: 0x04003EAE RID: 16046
		Tutorials
		' Token: 0x04003EAF RID: 16047
		OptionMenu
		' Token: 0x04003EB0 RID: 16048
		RemappingMenu
		' Token: 0x04003EB1 RID: 16049
		RemappingButton
		' Token: 0x04003EB2 RID: 16050
		XboxNotification
		' Token: 0x04003EB3 RID: 16051
		AttractScreen
		' Token: 0x04003EB4 RID: 16052
		JoinPrompt
		' Token: 0x04003EB5 RID: 16053
		ConfirmMenu
		' Token: 0x04003EB6 RID: 16054
		DifficultyMenu
		' Token: 0x04003EB7 RID: 16055
		ShopElement
		' Token: 0x04003EB8 RID: 16056
		StageTitles
		' Token: 0x04003EB9 RID: 16057
		NintendoSwitchNotification
		' Token: 0x04003EBA RID: 16058
		Achievements
	End Enum

	' Token: 0x0200091B RID: 2331
	<Serializable()>
	Public Structure Translation
		' Token: 0x1700046B RID: 1131
		' (get) Token: 0x0600368F RID: 13967 RVA: 0x001F998C File Offset: 0x001F7D8C
		Public ReadOnly Property hasSpriteAtlasImage As Boolean
			Get
				Return Me.spriteAtlasName IsNot Nothing AndAlso Me.spriteAtlasName.Length > 0 AndAlso Me.spriteAtlasImageName IsNot Nothing AndAlso Me.spriteAtlasImageName.Length > 0
			End Get
		End Property

		' Token: 0x1700046C RID: 1132
		' (get) Token: 0x06003690 RID: 13968 RVA: 0x001F99C6 File Offset: 0x001F7DC6
		Public ReadOnly Property hasCustomFont As Boolean
			Get
				Return Me.fonts.fontType <> FontLoader.FontType.None
			End Get
		End Property

		' Token: 0x1700046D RID: 1133
		' (get) Token: 0x06003691 RID: 13969 RVA: 0x001F99D9 File Offset: 0x001F7DD9
		Public ReadOnly Property hasCustomFontAsset As Boolean
			Get
				Return Me.fonts.tmpFontType <> FontLoader.TMPFontType.None
			End Get
		End Property

		' Token: 0x06003692 RID: 13970 RVA: 0x001F99EC File Offset: 0x001F7DEC
		Public Function SanitizedText() As String
			Return Me.text.Replace("\n", vbLf)
		End Function

		' Token: 0x04003EBB RID: 16059
		<SerializeField()>
		Public hasImage As Boolean

		' Token: 0x04003EBC RID: 16060
		<SerializeField()>
		Public text As String

		' Token: 0x04003EBD RID: 16061
		<SerializeField()>
		Public fonts As Localization.CategoryLanguageFont

		' Token: 0x04003EBE RID: 16062
		<SerializeField()>
		Public image As Sprite

		' Token: 0x04003EBF RID: 16063
		<SerializeField()>
		Public spriteAtlasName As String

		' Token: 0x04003EC0 RID: 16064
		<SerializeField()>
		Public spriteAtlasImageName As String
	End Structure

	' Token: 0x0200091C RID: 2332
	<Serializable()>
	Public Class CategoryLanguageFont
		' Token: 0x1700046E RID: 1134
		' (get) Token: 0x06003694 RID: 13972 RVA: 0x001F9A0B File Offset: 0x001F7E0B
		Public ReadOnly Property font As Font
			Get
				Return FontLoader.GetFont(Me.fontType)
			End Get
		End Property

		' Token: 0x1700046F RID: 1135
		' (get) Token: 0x06003695 RID: 13973 RVA: 0x001F9A18 File Offset: 0x001F7E18
		Public ReadOnly Property fontAsset As TMP_FontAsset
			Get
				Return FontLoader.GetTMPFont(Me.tmpFontType)
			End Get
		End Property

		' Token: 0x04003EC1 RID: 16065
		Public fontSize As Integer

		' Token: 0x04003EC2 RID: 16066
		Public fontType As FontLoader.FontType

		' Token: 0x04003EC3 RID: 16067
		Public fontAssetSize As Single

		' Token: 0x04003EC4 RID: 16068
		Public tmpFontType As FontLoader.TMPFontType

		' Token: 0x04003EC5 RID: 16069
		Public charSpacing As Single
	End Class

	' Token: 0x0200091D RID: 2333
	<Serializable()>
	Public Structure CategoryLanguageFonts
		' Token: 0x17000470 RID: 1136
		Public Default Property Item(index As Integer) As Localization.CategoryLanguageFont
			Get
				Return Me.fonts(index)
			End Get
			Set(value As Localization.CategoryLanguageFont)
				Me.fonts(index) = value
			End Set
		End Property

		' Token: 0x04003EC6 RID: 16070
		<SerializeField()>
		Public fonts As Localization.CategoryLanguageFont()
	End Structure

	' Token: 0x0200091E RID: 2334
	' (Invoke) Token: 0x06003699 RID: 13977
	Public Delegate Sub LanguageChanged()
End Class
