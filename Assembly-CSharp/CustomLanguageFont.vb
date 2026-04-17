Imports System
Imports System.Collections.Generic
Imports TMPro
Imports UnityEngine
Imports UnityEngine.UI

' Token: 0x02000910 RID: 2320
Public Class CustomLanguageFont
	Inherits MonoBehaviour

	' Token: 0x0600365D RID: 13917 RVA: 0x001F7EC4 File Offset: 0x001F62C4
	Private Sub Awake()
		Me.textMeshProComponent = MyBase.GetComponent(Of TMP_Text)()
		Me.textComponent = MyBase.GetComponent(Of Text)()
		Me.englishBasicFont = Nothing
		Me.englishBasicFont.characterSpacing = Me.textMeshProComponent.characterSpacing
		Me.englishBasicFont.lineSpacing = Me.textMeshProComponent.lineSpacing
		Me.englishBasicFont.paragraphSpacing = Me.textMeshProComponent.paragraphSpacing
		Me.englishBasicFont.needFontSize = True
		Me.englishBasicFont.customFontSize = Me.textMeshProComponent.fontSize
		Me.englishBasicFont.needKerning = Me.textMeshProComponent.enableKerning
	End Sub

	' Token: 0x0600365E RID: 13918 RVA: 0x001F7F72 File Offset: 0x001F6372
	Private Sub Start()
		AddHandler Localization.OnLanguageChangedEvent, AddressOf Me.ReviewFont
		Me.ReviewFont()
	End Sub

	' Token: 0x0600365F RID: 13919 RVA: 0x001F7F8B File Offset: 0x001F638B
	Private Sub OnDestroy()
		RemoveHandler Localization.OnLanguageChangedEvent, AddressOf Me.ReviewFont
	End Sub

	' Token: 0x06003660 RID: 13920 RVA: 0x001F7F9E File Offset: 0x001F639E
	Private Sub OnEnable()
		Me.ReviewFont()
	End Sub

	' Token: 0x06003661 RID: 13921 RVA: 0x001F7FA8 File Offset: 0x001F63A8
	Private Sub ReviewFont()
		If Me.textMeshProComponent Is Nothing AndAlso Me.textComponent Is Nothing Then
			Return
		End If
		Dim num As Integer = 0
		Dim flag As Boolean = False
		While Not flag AndAlso num < Me.customLayouts.Count
			flag = Me.customLayouts(num).languageApplied = Localization.language
			num += 1
		End While
		num -= 1
		If flag Then
			If Me.customLayouts(num).needSpacing Then
				Me.ApplySpacingChanges(Me.customLayouts(num))
			End If
			If Me.customLayouts(num).needFontSize Then
				Me.ApplyFontSizeChanges(Me.customLayouts(num))
			End If
			Me.textMeshProComponent.enableKerning = Me.customLayouts(num).needKerning
		Else
			Me.ApplySpacingChanges(Me.englishBasicFont)
			Me.ApplyFontSizeChanges(Me.englishBasicFont)
			Me.textMeshProComponent.enableKerning = Me.englishBasicFont.needKerning
		End If
	End Sub

	' Token: 0x06003662 RID: 13922 RVA: 0x001F80CC File Offset: 0x001F64CC
	Private Sub ApplySpacingChanges(languageLayout As CustomLanguageFont.LanguageFont)
		If Me.textMeshProComponent IsNot Nothing Then
			Me.textMeshProComponent.characterSpacing = languageLayout.characterSpacing
			Me.textMeshProComponent.lineSpacing = languageLayout.lineSpacing
			Me.textMeshProComponent.paragraphSpacing = languageLayout.paragraphSpacing
		Else
			Me.textComponent.lineSpacing = languageLayout.lineSpacing
		End If
	End Sub

	' Token: 0x06003663 RID: 13923 RVA: 0x001F8137 File Offset: 0x001F6537
	Private Sub ApplyFontSizeChanges(languageLayout As CustomLanguageFont.LanguageFont)
		If Me.textMeshProComponent IsNot Nothing Then
			Me.textMeshProComponent.fontSize = languageLayout.customFontSize
		Else
			Me.textComponent.fontSize = CInt(languageLayout.customFontSize)
		End If
	End Sub

	' Token: 0x04003E51 RID: 15953
	<SerializeField()>
	Public customLayouts As List(Of CustomLanguageFont.LanguageFont)

	' Token: 0x04003E52 RID: 15954
	Private englishBasicFont As CustomLanguageFont.LanguageFont

	' Token: 0x04003E53 RID: 15955
	Private textMeshProComponent As TMP_Text

	' Token: 0x04003E54 RID: 15956
	Private textComponent As Text

	' Token: 0x02000911 RID: 2321
	<Serializable()>
	Public Structure LanguageFont
		' Token: 0x04003E55 RID: 15957
		Public languageApplied As Localization.Languages

		' Token: 0x04003E56 RID: 15958
		Public needSpacing As Boolean

		' Token: 0x04003E57 RID: 15959
		Public characterSpacing As Single

		' Token: 0x04003E58 RID: 15960
		Public lineSpacing As Single

		' Token: 0x04003E59 RID: 15961
		Public paragraphSpacing As Single

		' Token: 0x04003E5A RID: 15962
		Public needFontSize As Boolean

		' Token: 0x04003E5B RID: 15963
		Public customFontSize As Single

		' Token: 0x04003E5C RID: 15964
		Public needKerning As Boolean
	End Structure
End Class
