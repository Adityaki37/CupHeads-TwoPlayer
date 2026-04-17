Imports System
Imports System.Collections.Generic
Imports TMPro
Imports UnityEngine

' Token: 0x02000912 RID: 2322
Public Class CustomLanguageLayout
	Inherits MonoBehaviour

	' Token: 0x06003665 RID: 13925 RVA: 0x001F817C File Offset: 0x001F657C
	Private Sub Awake()
		Me.rectTransform = MyBase.GetComponent(Of RectTransform)()
		Me.textContainer = MyBase.GetComponent(Of TextContainer)()
		Me.englishBasicLayout = Nothing
		Me.englishBasicLayout.positionOffset = Me.rectTransform.localPosition
		Me.englishBasicLayout.customWidth = Me.rectTransform.sizeDelta.x
		Me.englishBasicLayout.customHeight = Me.rectTransform.sizeDelta.y
	End Sub

	' Token: 0x06003666 RID: 13926 RVA: 0x001F8202 File Offset: 0x001F6602
	Private Sub OnDestroy()
		RemoveHandler Localization.OnLanguageChangedEvent, AddressOf Me.ReviewLayout
	End Sub

	' Token: 0x06003667 RID: 13927 RVA: 0x001F8215 File Offset: 0x001F6615
	Private Sub OnEnable()
		AddHandler Localization.OnLanguageChangedEvent, AddressOf Me.ReviewLayout
		Me.ReviewLayout()
	End Sub

	' Token: 0x06003668 RID: 13928 RVA: 0x001F822E File Offset: 0x001F662E
	Private Sub OnDisable()
		Me.ResetToEnglish()
		RemoveHandler Localization.OnLanguageChangedEvent, AddressOf Me.ReviewLayout
	End Sub

	' Token: 0x06003669 RID: 13929 RVA: 0x001F8248 File Offset: 0x001F6648
	Private Sub ReviewLayout()
		If Me.rectTransform Is Nothing Then
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
			Dim languageLayout As CustomLanguageLayout.LanguageLayout = Me.customLayouts(num)
			Me.ApplylayoutChanges(languageLayout)
		Else
			Me.ResetToEnglish()
		End If
	End Sub

	' Token: 0x0600366A RID: 13930 RVA: 0x001F82D0 File Offset: 0x001F66D0
	Private Sub ResetToEnglish()
		Me.rectTransform.localPosition = Me.englishBasicLayout.positionOffset
		If Me.textContainer IsNot Nothing Then
			Me.textContainer.height = Me.englishBasicLayout.customHeight
			Me.textContainer.width = Me.englishBasicLayout.customWidth
		Else
			Me.rectTransform.sizeDelta = New Vector2(Me.englishBasicLayout.customWidth, Me.englishBasicLayout.customHeight)
		End If
	End Sub

	' Token: 0x0600366B RID: 13931 RVA: 0x001F835C File Offset: 0x001F675C
	Private Sub ApplylayoutChanges(languageLayout As CustomLanguageLayout.LanguageLayout)
		If languageLayout.needCustomOffset Then
			Me.rectTransform.localPosition = New Vector3(Me.englishBasicLayout.positionOffset.x + languageLayout.positionOffset.x, Me.englishBasicLayout.positionOffset.y + languageLayout.positionOffset.y, Me.englishBasicLayout.positionOffset.z + languageLayout.positionOffset.z)
		Else
			Me.rectTransform.localPosition = New Vector3(Me.englishBasicLayout.positionOffset.x, Me.englishBasicLayout.positionOffset.y, Me.englishBasicLayout.positionOffset.z)
		End If
		If Me.textContainer IsNot Nothing Then
			Me.textContainer.width = If((Not languageLayout.needCustomWidth), Me.englishBasicLayout.customWidth, languageLayout.customWidth)
			Me.textContainer.height = If((Not languageLayout.needCustomHeight), Me.englishBasicLayout.customHeight, languageLayout.customHeight)
		Else
			Dim num As Single = If((Not languageLayout.needCustomWidth), Me.englishBasicLayout.customWidth, languageLayout.customWidth)
			Dim num2 As Single = If((Not languageLayout.needCustomHeight), Me.englishBasicLayout.customHeight, languageLayout.customHeight)
			Me.rectTransform.sizeDelta = New Vector2(num, num2)
		End If
	End Sub

	' Token: 0x04003E5D RID: 15965
	<SerializeField()>
	Public customLayouts As List(Of CustomLanguageLayout.LanguageLayout)

	' Token: 0x04003E5E RID: 15966
	Private rectTransform As RectTransform

	' Token: 0x04003E5F RID: 15967
	Private englishBasicLayout As CustomLanguageLayout.LanguageLayout

	' Token: 0x04003E60 RID: 15968
	Private textContainer As TextContainer

	' Token: 0x02000913 RID: 2323
	<Serializable()>
	Public Structure LanguageLayout
		' Token: 0x04003E61 RID: 15969
		Public languageApplied As Localization.Languages

		' Token: 0x04003E62 RID: 15970
		Public needCustomOffset As Boolean

		' Token: 0x04003E63 RID: 15971
		Public positionOffset As Vector3

		' Token: 0x04003E64 RID: 15972
		Public needCustomWidth As Boolean

		' Token: 0x04003E65 RID: 15973
		Public customWidth As Single

		' Token: 0x04003E66 RID: 15974
		Public needCustomHeight As Boolean

		' Token: 0x04003E67 RID: 15975
		Public customHeight As Single
	End Structure
End Class
