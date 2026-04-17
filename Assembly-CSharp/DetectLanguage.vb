Imports System
Imports System.Globalization

' Token: 0x02000916 RID: 2326
Public Module DetectLanguage
	' Token: 0x06003674 RID: 13940 RVA: 0x001F8690 File Offset: 0x001F6A90
	Public Function GetDefaultLanguage() As Localization.Languages
		Dim languages As Localization.Languages = Localization.Languages.English
		DetectLanguage.getDefaultLanguage(languages)
		Return languages
	End Function

	' Token: 0x06003675 RID: 13941 RVA: 0x001F86A8 File Offset: 0x001F6AA8
	Private Sub getDefaultLanguage(ByRef defaultLanguage As Localization.Languages)
		Dim currentUICulture As CultureInfo = CultureInfo.CurrentUICulture
		Dim twoLetterISOLanguageName As String = currentUICulture.TwoLetterISOLanguageName
		If twoLetterISOLanguageName = "fr" Then
			defaultLanguage = Localization.Languages.French
		ElseIf twoLetterISOLanguageName = "de" Then
			defaultLanguage = Localization.Languages.German
		ElseIf twoLetterISOLanguageName = "it" Then
			defaultLanguage = Localization.Languages.Italian
		ElseIf twoLetterISOLanguageName = "ja" Then
			defaultLanguage = Localization.Languages.Japanese
		ElseIf twoLetterISOLanguageName = "zh" Then
			defaultLanguage = Localization.Languages.SimplifiedChinese
		ElseIf twoLetterISOLanguageName = "ru" Then
			defaultLanguage = Localization.Languages.Russian
		ElseIf twoLetterISOLanguageName = "es" Then
			If currentUICulture.Name = "es-ES" OrElse currentUICulture.Name = "es" Then
				defaultLanguage = Localization.Languages.SpanishSpain
			Else
				defaultLanguage = Localization.Languages.SpanishAmerica
			End If
		ElseIf twoLetterISOLanguageName = "ko" Then
			defaultLanguage = Localization.Languages.Korean
		ElseIf twoLetterISOLanguageName = "po" Then
			defaultLanguage = Localization.Languages.Polish
		ElseIf currentUICulture.Name = "pt-BR" Then
			defaultLanguage = Localization.Languages.PortugueseBrazil
		Else
			defaultLanguage = Localization.Languages.English
		End If
	End Sub
End Module
