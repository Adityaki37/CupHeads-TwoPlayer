Imports System
Imports System.Collections.Generic
Imports UnityEngine
Imports UnityEngine.UI

' Token: 0x02000914 RID: 2324
Public Class CustomLanguageLayoutGroup
	Inherits MonoBehaviour

	' Token: 0x0600366D RID: 13933 RVA: 0x001F84F8 File Offset: 0x001F68F8
	Private Sub Awake()
		Me.englishBasicLayout = Nothing
		Me.englishBasicLayout.needPadding = True
		Me.englishBasicLayout.padding = Me.layoutComponent.padding
		Me.englishBasicLayout.needSpacing = True
		Me.englishBasicLayout.spacing = Me.layoutComponent.spacing
	End Sub

	' Token: 0x0600366E RID: 13934 RVA: 0x001F8558 File Offset: 0x001F6958
	Private Sub Start()
		AddHandler Localization.OnLanguageChangedEvent, AddressOf Me.ReviewLayout
	End Sub

	' Token: 0x0600366F RID: 13935 RVA: 0x001F856B File Offset: 0x001F696B
	Private Sub OnDestroy()
		RemoveHandler Localization.OnLanguageChangedEvent, AddressOf Me.ReviewLayout
	End Sub

	' Token: 0x06003670 RID: 13936 RVA: 0x001F857E File Offset: 0x001F697E
	Private Sub OnEnable()
		Me.ReviewLayout()
	End Sub

	' Token: 0x06003671 RID: 13937 RVA: 0x001F8588 File Offset: 0x001F6988
	Private Sub ReviewLayout()
		If Me.layoutComponent Is Nothing Then
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
			If Me.customLayouts(num).needPadding Then
				Me.ApplyPaddingChanges(Me.customLayouts(num))
			End If
		Else
			Me.ApplySpacingChanges(Me.englishBasicLayout)
			Me.ApplyPaddingChanges(Me.englishBasicLayout)
		End If
	End Sub

	' Token: 0x06003672 RID: 13938 RVA: 0x001F8665 File Offset: 0x001F6A65
	Private Sub ApplySpacingChanges(languageLayout As CustomLanguageLayoutGroup.LanguageLayoutGroup)
		Me.layoutComponent.spacing = languageLayout.spacing
	End Sub

	' Token: 0x06003673 RID: 13939 RVA: 0x001F8679 File Offset: 0x001F6A79
	Private Sub ApplyPaddingChanges(languageLayout As CustomLanguageLayoutGroup.LanguageLayoutGroup)
		Me.layoutComponent.padding = languageLayout.padding
	End Sub

	' Token: 0x04003E68 RID: 15976
	<SerializeField()>
	Private layoutComponent As HorizontalOrVerticalLayoutGroup

	' Token: 0x04003E69 RID: 15977
	<SerializeField()>
	Public customLayouts As List(Of CustomLanguageLayoutGroup.LanguageLayoutGroup)

	' Token: 0x04003E6A RID: 15978
	Private englishBasicLayout As CustomLanguageLayoutGroup.LanguageLayoutGroup

	' Token: 0x02000915 RID: 2325
	<Serializable()>
	Public Structure LanguageLayoutGroup
		' Token: 0x04003E6B RID: 15979
		Public languageApplied As Localization.Languages

		' Token: 0x04003E6C RID: 15980
		Public needPadding As Boolean

		' Token: 0x04003E6D RID: 15981
		Public padding As RectOffset

		' Token: 0x04003E6E RID: 15982
		Public needSpacing As Boolean

		' Token: 0x04003E6F RID: 15983
		Public spacing As Single
	End Structure
End Class
