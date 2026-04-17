Imports System
Imports UnityEngine.UI

' Token: 0x02000923 RID: 2339
Public Class TextAutoLocalize
	Inherits Text

	' Token: 0x17000471 RID: 1137
	' (get) Token: 0x060036AA RID: 13994 RVA: 0x001FA0F3 File Offset: 0x001F84F3
	' (set) Token: 0x060036AB RID: 13995 RVA: 0x001FA0FC File Offset: 0x001F84FC
	Public Overrides Property text As String
		Get
			Return MyBase.text
		End Get
		Set(value As String)
			Dim translationElement As TranslationElement = Localization.Find(value)
			MyBase.text = If((translationElement Is Nothing), value, translationElement.translations(CInt(Localization.language)).text)
		End Set
	End Property
End Class
