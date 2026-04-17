Imports System
Imports UnityEngine

' Token: 0x0200046B RID: 1131
Public Class SymbolPicker
	Inherits MonoBehaviour

	' Token: 0x06001153 RID: 4435 RVA: 0x000A465C File Offset: 0x000A2A5C
	Private Sub OnEnable()
		Me.ApplySymbol()
	End Sub

	' Token: 0x06001154 RID: 4436 RVA: 0x000A4664 File Offset: 0x000A2A64
	Public Sub ApplySymbol()
		Dim translationElement As TranslationElement = Localization.Find(Me.button.ToString())
		Me.localizationHelper.ApplyTranslation(translationElement, Nothing)
	End Sub

	' Token: 0x04001AC6 RID: 6854
	<SerializeField()>
	Private localizationHelper As LocalizationHelper

	' Token: 0x04001AC7 RID: 6855
	Public button As CupheadButton
End Class
