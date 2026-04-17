Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports TMPro
Imports UnityEngine
Imports UnityEngine.UI

' Token: 0x0200042A RID: 1066
Public Class SpeechBubble
	Inherits AbstractPausableComponent

	' Token: 0x17000269 RID: 617
	' (get) Token: 0x06000F77 RID: 3959 RVA: 0x0009A5F9 File Offset: 0x000989F9
	' (set) Token: 0x06000F78 RID: 3960 RVA: 0x0009A601 File Offset: 0x00098A01
	Public Property mode As SpeechBubble.Mode

	' Token: 0x1700026A RID: 618
	' (get) Token: 0x06000F79 RID: 3961 RVA: 0x0009A60A File Offset: 0x00098A0A
	' (set) Token: 0x06000F7A RID: 3962 RVA: 0x0009A612 File Offset: 0x00098A12
	Public Property displayState As SpeechBubble.DisplayState

	' Token: 0x06000F7B RID: 3963 RVA: 0x0009A61C File Offset: 0x00098A1C
	Protected Overrides Sub Awake()
		If SpeechBubble.Instance IsNot Nothing Then
			Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Else
			SpeechBubble.Instance = Me
		End If
		Me.arrowAnchoredPosition = Me.arrowBox.anchoredPosition
		Me.panPosition = MyBase.transform.position
		MyBase.Awake()
		Dialoguer.Initialize()
	End Sub

	' Token: 0x06000F7C RID: 3964 RVA: 0x0009A684 File Offset: 0x00098A84
	Private Sub Start()
		If Me.expandOnTheRight Then
			MyBase.rectTransform.anchorMin = Vector2.zero
			MyBase.rectTransform.anchorMax = Vector2.zero
			MyBase.rectTransform.pivot = Vector2.zero
		Else
			MyBase.rectTransform.anchorMin = New Vector2(1F, 0F)
			MyBase.rectTransform.anchorMax = New Vector2(1F, 0F)
			MyBase.rectTransform.pivot = Vector2.one
		End If
		Me.canvasGroup.alpha = 0F
		Me.basePosition = MyBase.rectTransform.position
		Me.input = New CupheadInput.AnyPlayerInput(False)
		Me.AddDialoguerEvents()
	End Sub

	' Token: 0x06000F7D RID: 3965 RVA: 0x0009A750 File Offset: 0x00098B50
	Private Function ProcessChoice(playerSelection As Integer) As Integer
		Dim num As Integer = 0
		Dim i As Integer = 0
		While i <= playerSelection
			If Not Me.OptionHidden(num) Then
				i += 1
			End If
			num += 1
		End While
		Return num - 1
	End Function

	' Token: 0x06000F7E RID: 3966 RVA: 0x0009A784 File Offset: 0x00098B84
	Private Sub Update()
		If MapEventNotification.Current Is Nothing OrElse Not MapEventNotification.Current.showing Then
			If Me.waiting Then
				Return
			End If
			If Me.waitForFade Then
				Return
			End If
			If Me.input.GetButtonUp(CupheadButton.Accept) Then
				Me.waitForRealease = False
			End If
			If Not Me.waitForRealease AndAlso Me.input.GetButtonDown(CupheadButton.Accept) Then
				If Me.currentChoiceIndex >= 0 Then
					If Me.displayState = SpeechBubble.DisplayState.WaitForSelection Then
						AudioManager.Play("level_menu_select")
					End If
					Dialoguer.ContinueDialogue(Me.ProcessChoice(Me.currentChoiceIndex))
				Else
					Dialoguer.ContinueDialogue()
				End If
			End If
			If Me.displayState = SpeechBubble.DisplayState.WaitForSelection AndAlso Me.input.GetButtonDown(CupheadButton.Cancel) Then
				Dialoguer.EndDialogue()
			End If
		End If
	End Sub

	' Token: 0x06000F7F RID: 3967 RVA: 0x0009A85E File Offset: 0x00098C5E
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.RemoveDialoguerEvents()
		SpeechBubble.Instance = Nothing
	End Sub

	' Token: 0x06000F80 RID: 3968 RVA: 0x0009A872 File Offset: 0x00098C72
	Private Sub OnLanguageChanged()
		Me.delayedShow = True
	End Sub

	' Token: 0x06000F81 RID: 3969 RVA: 0x0009A87B File Offset: 0x00098C7B
	Private Sub OnEnable()
		If Me.delayedShow Then
			Me.delayedShow = False
			Me.Show(Me.data.text)
		End If
	End Sub

	' Token: 0x06000F82 RID: 3970 RVA: 0x0009A8A0 File Offset: 0x00098CA0
	Private Function ProcessTextPreShow(text As String) As String
		Dim normalizedText As String = Me.GetNormalizedText(Localization.Translate(text).SanitizedText())
		Dim fontAsset As TMP_FontAsset = Localization.Instance.fonts(CInt(Localization.language))(27).fontAsset
		Dim fontAsset2 As TMP_FontAsset = Localization.Instance.fonts(0)(27).fontAsset
		Return If((Not(fontAsset Is fontAsset2)), normalizedText, Me.AdjustSpacingInFont(StringVariantGenerator.Instance.Generate(normalizedText)))
	End Function

	' Token: 0x06000F83 RID: 3971 RVA: 0x0009A928 File Offset: 0x00098D28
	Public Sub Show(text As String)
		If Me.showCoroutine IsNot Nothing Then
			MyBase.StopCoroutine(Me.showCoroutine)
		End If
		Dim text2 As String = Me.ProcessTextPreShow(text)
		Dim num As Integer = 8
		If Localization.language = Localization.Languages.Japanese OrElse Localization.language = Localization.Languages.SimplifiedChinese Then
			num = 5
		ElseIf Localization.language = Localization.Languages.Korean Then
			num = 6
		End If
		Me.showCoroutine = MyBase.StartCoroutine(Me.show_cr(SpeechBubble.Mode.Text, String.Concat(New Object() { text2, "<space=", num, "em> " }), Nothing))
	End Sub

	' Token: 0x06000F84 RID: 3972 RVA: 0x0009A9C0 File Offset: 0x00098DC0
	Public Sub Show(text As String, listItems As List(Of String))
		If Me.showCoroutine IsNot Nothing Then
			MyBase.StopCoroutine(Me.showCoroutine)
		End If
		Dim text2 As String = Me.ProcessTextPreShow(text)
		Me.showCoroutine = MyBase.StartCoroutine(Me.show_cr(SpeechBubble.Mode.ListChoice, text2, listItems))
	End Sub

	' Token: 0x06000F85 RID: 3973 RVA: 0x0009AA01 File Offset: 0x00098E01
	Public Sub Dismiss()
		MyBase.StartCoroutine(Me.dismiss_cr(Me.preventQuit))
	End Sub

	' Token: 0x06000F86 RID: 3974 RVA: 0x0009AA18 File Offset: 0x00098E18
	Protected Overridable Function GetNormalizedText(text As String) As String
		Dim text2 As String = Me.mainText.text
		Dim font As TMP_FontAsset = Me.mainText.font
		text = text.Replace("{DEATHS}", "<size=15> </size><font=""CupheadVogue-BoldSDF""><b><size=36>" + PlayerData.Data.DeathCount(PlayerId.Any).ToStringInvariant() + "</size></b></font><size=15> </size>")
		text = text.Replace("{BOSSREF}", Me.setBossRefText)
		Dim text3 As String = text
		If Localization.language <> Localization.Languages.Japanese Then
			text3 = String.Empty
			text = text.Replace(vbLf, " ")
			text = text.Replace(" ", "<space=11.19853> ")
			text = text.Replace("{BR}", vbLf)
			Me.mainText.text = text
			Me.mainText.font = Localization.Instance.fonts(CInt(Localization.language))(27).fontAsset
			Me.mainText.CalculateLayoutInputHorizontal()
			Dim text4 As String = String.Empty
			Dim num As Integer = 10000
			Dim num2 As Integer = 0
			While Me.mainText.text.Length > 0 AndAlso num > 0
				num -= 1
				While Me.mainText.text.Length > 0 AndAlso Me.mainText.preferredWidth > Me.maxWidth AndAlso num > 0
					num -= 1
					Dim text5 As String = Me.mainText.text.Substring(Me.mainText.text.Length - 1, 1)
					If text5.Equals(" ") Then
						Dim num3 As Integer = Me.mainText.text.LastIndexOf("<")
						text4 = Me.mainText.text.Substring(num3, Me.mainText.text.Length - num3) + text4
						Me.mainText.text = Me.mainText.text.Substring(0, num3)
					Else
						text4 = text5 + text4
						Me.mainText.text = Me.mainText.text.Substring(0, Me.mainText.text.Length - 1)
					End If
					Me.mainText.CalculateLayoutInputHorizontal()
				End While
				Dim num4 As Integer = Me.mainText.text.LastIndexOf(" ")
				If num4 = -1 OrElse String.IsNullOrEmpty(text4) Then
					If Not String.IsNullOrEmpty(text4) AndAlso text4.Substring(0, 1).Equals("<") Then
						text3 = text3 + Me.mainText.text + vbLf
					Else
						text3 += Me.mainText.text
					End If
				Else
					text4 = Me.mainText.text.Substring(num4 + 1) + text4
					text3 = text3 + Me.mainText.text.Substring(0, num4) + vbLf
				End If
				Me.mainText.text = text4
				Me.mainText.CalculateLayoutInputHorizontal()
				text4 = String.Empty
				num2 += 1
			End While
			If num = 0 Then
				Global.Debug.LogError("THE WHILES ARE DEAD, BAD CODE !!!", Nothing)
			End If
			If Me.maxLines <> -1 AndAlso num2 > Me.maxLines Then
				text3 = text3.Replace(vbLf, " ")
				Me.mainText.enableAutoSizing = True
				Me.textLayoutElement.enabled = True
				Me.layout.padding.left = 20
				Me.layout.padding.right = 20
				Me.layout.padding.bottom = 20
				Me.layout.padding.top = 20
			Else
				Me.mainText.enableAutoSizing = False
				Me.textLayoutElement.enabled = False
			End If
		Else
			Me.mainText.enableAutoSizing = False
			Me.textLayoutElement.enabled = False
		End If
		Me.mainText.text = text2
		Me.mainText.font = font
		Return text3
	End Function

	' Token: 0x06000F87 RID: 3975 RVA: 0x0009AE30 File Offset: 0x00099230
	Private Function AdjustSpacingInFont(text As String) As String
		Dim text2 As String = String.Empty
		text2 = text.Replace("<space=11.19853>" & vbLf, vbLf)
		text2 = text2.Replace("<space=11.19853>]", "<space=0.01244>]")
		Return text2.Replace("<space=11.19853>}", "<space=-0.00622>}")
	End Function

	' Token: 0x06000F88 RID: 3976 RVA: 0x0009AE78 File Offset: 0x00099278
	Private Iterator Function show_cr(mode As SpeechBubble.Mode, text As String, listItems As List(Of String)) As IEnumerator
		Me.waitForFade = True
		If Me.displayState <> SpeechBubble.DisplayState.Hidden Then
			Yield MyBase.StartCoroutine(Me.dismiss_cr(False))
		End If
		If Me.expandOnTheRight Then
			Me.box.GetComponent(Of RectTransform)().pivot = Vector2.zero
		Else
			Me.box.GetComponent(Of RectTransform)().pivot = New Vector2(1F, 0F)
		End If
		Me.layout.padding.left = 30
		Me.layout.padding.right = 30
		Me.layout.padding.bottom = 30
		Me.layout.padding.top = 30
		Me.layout.spacing = 0F
		Me.mainText.text = text
		Me.mainText.font = Localization.Instance.fonts(CInt(Localization.language))(27).fontAsset
		Me.choiceText.font = Me.mainText.font
		For Each rectTransform As RectTransform In Me.bullets
			rectTransform.gameObject.SetActive(False)
		Next
		Me.currentChoiceIndex = -1
		If mode = SpeechBubble.Mode.ListChoice Then
			Dim choiceColumn As String = String.Empty
			For i As Integer = 0 To listItems.Count - 1
				If i < listItems.Count - 1 Then
					choiceColumn = choiceColumn + Localization.Translate(listItems(i)).SanitizedText() + vbLf
				Else
					choiceColumn += Localization.Translate(listItems(i)).SanitizedText()
				End If
			Next
			If Localization.language <> Localization.Languages.Korean Then
				Me.choiceText.text = StringVariantGenerator.Instance.Generate(choiceColumn)
			Else
				Me.choiceText.text = choiceColumn
			End If
			Me.currentChoiceIndex = 0
			Me.layout.spacing = 30F
			Yield Nothing
		Else
			Me.choiceText.text = Nothing
		End If
		If Me.tailOnTheLeft Then
			Me.tail.rectTransform.anchorMin = Vector2.zero
			Me.tail.rectTransform.anchorMax = Vector2.zero
			Me.tail.rectTransform.anchoredPosition = New Vector2(73F, Me.tail.rectTransform.anchoredPosition.y)
		Else
			Me.tail.rectTransform.anchorMin = New Vector2(1F, 0F)
			Me.tail.rectTransform.anchorMax = New Vector2(1F, 0F)
			Me.tail.rectTransform.anchoredPosition = New Vector2(-73F, Me.tail.rectTransform.anchoredPosition.y)
		End If
		Me.arrow.color = New Color(1F, 1F, 1F, 0F)
		Dim maxOffset As Single = 0.05F
		If CupheadLevelCamera.Current IsNot Nothing Then
			maxOffset *= 100F
		End If
		MyBase.rectTransform.position = Me.basePosition + New Vector2(Global.UnityEngine.Random.Range(-maxOffset, maxOffset), Global.UnityEngine.Random.Range(-maxOffset, maxOffset)) * MyBase.rectTransform.localScale.x
		Me.tail.sprite = Me.tailVariants.RandomChoice()
		Me.tail.enabled = Not Me.hideTail
		Me.arrow.sprite = Me.arrowVariants.RandomChoice()
		MyBase.animator.Play("Idle", 0, Global.UnityEngine.Random.Range(0F, 1F))
		MyBase.animator.Play("Idle", 1, Global.UnityEngine.Random.Range(0F, 1F))
		Me.displayState = SpeechBubble.DisplayState.FadeIn
		Yield MyBase.StartCoroutine(Me.fade_cr(Me.canvasGroup.alpha, 1F))
		Yield CupheadTime.WaitForSeconds(Me, 0.125F)
		Me.displayState = SpeechBubble.DisplayState.Showing
		Me.showCoroutine = Nothing
		Dim colorHidden As Color = New Color(1F, 1F, 1F, 0F)
		Dim colorShown As Color = New Color(1F, 1F, 1F, 1F)
		If Me.expandOnTheRight Then
			Me.arrowBox.anchoredPosition = New Vector2(Me.arrowAnchoredPosition.x + Me.box.sizeDelta.x, Me.arrowBox.anchoredPosition.y)
		End If
		If mode = SpeechBubble.Mode.Text Then
			Me.arrow.color = If((Not Me.waiting), colorShown, colorHidden)
			Me.cursor.color = colorHidden
		Else
			Me.cursor.color = If((Not Me.waiting), colorShown, colorHidden)
			Me.displayState = SpeechBubble.DisplayState.WaitForSelection
			Me.waitForFade = False
			While Me.displayState = SpeechBubble.DisplayState.WaitForSelection
				If Me.waiting Then
					Yield Nothing
				Else
					If PauseManager.state <> PauseManager.State.Paused Then
						If Me.input.GetButtonDown(CupheadButton.MenuDown) AndAlso Me.currentChoiceIndex < listItems.Count - 1 Then
							Me.currentChoiceIndex += 1
							MyBase.animator.SetTrigger("MoveDown")
							AudioManager.Play("level_menu_move")
						End If
						If Me.input.GetButtonDown(CupheadButton.MenuUp) AndAlso Me.currentChoiceIndex > 0 Then
							Me.currentChoiceIndex -= 1
							MyBase.animator.SetTrigger("MoveUp")
							AudioManager.Play("level_menu_move")
						End If
					End If
					Me.cursorRoot.anchoredPosition = Me.getCursorPos(Me.currentChoiceIndex, listItems.Count)
					Me.cursor.color = colorShown
					Yield Nothing
				End If
			End While
		End If
		Me.waitForFade = False
		Me.cursor.color = colorHidden
		Return
	End Function

	' Token: 0x06000F89 RID: 3977 RVA: 0x0009AEA8 File Offset: 0x000992A8
	Private Iterator Function dismiss_cr(watchPreventQuit As Boolean) As IEnumerator
		If Me.displayState = SpeechBubble.DisplayState.Hidden Then
			Return
		End If
		While Me.displayState = SpeechBubble.DisplayState.FadeIn
			Yield Nothing
		End While
		If watchPreventQuit Then
			While Me.preventQuit
				Yield Nothing
			End While
		End If
		Me.displayState = SpeechBubble.DisplayState.FadeOut
		Yield MyBase.StartCoroutine(Me.fade_cr(Me.canvasGroup.alpha, 0F))
		Me.displayState = SpeechBubble.DisplayState.Hidden
		Return
	End Function

	' Token: 0x06000F8A RID: 3978 RVA: 0x0009AECC File Offset: 0x000992CC
	Private Iterator Function fade_cr(startOpacity As Single, endOpacity As Single) As IEnumerator
		If endOpacity = 0F Then
			Me.canvasGroup.alpha = endOpacity
			Return
		End If
		Yield Nothing
		Dim t As Single = 0F
		While t < 0.07F
			Yield Nothing
			t += CupheadTime.Delta
			Me.canvasGroup.alpha = Mathf.Lerp(startOpacity, endOpacity, t / 0.07F)
		End While
		Me.canvasGroup.alpha = endOpacity
		Return
	End Function

	' Token: 0x06000F8B RID: 3979 RVA: 0x0009AEF8 File Offset: 0x000992F8
	Private Function getCursorPos(choiceIndex As Integer, choiceCount As Integer) As Vector2
		Dim num As Single = Me.choiceText.bounds.extents.y / CSng(choiceCount) * 2F
		Return New Vector2(Me.choiceText.margin.x - 10F, 0F) + Vector2.up * ((CSng(choiceCount) - 1F) / 2F) * num + Vector2.down * (CSng(choiceIndex) * num)
	End Function

	' Token: 0x06000F8C RID: 3980 RVA: 0x0009AF83 File Offset: 0x00099383
	Private Sub setOpacity(opacity As Single)
	End Sub

	' Token: 0x06000F8D RID: 3981 RVA: 0x0009AF88 File Offset: 0x00099388
	Public Sub AddDialoguerEvents()
		AddHandler Dialoguer.events.onStarted, AddressOf Me.OnDialogueStartedHandler
		AddHandler Dialoguer.events.onEnded, AddressOf Me.OnDialogueEndedHandler
		AddHandler Dialoguer.events.onInstantlyEnded, AddressOf Me.OnDialogueInstantlyEndedHandler
		AddHandler Dialoguer.events.onTextPhase, AddressOf Me.OnDialogueTextPhaseHandler
		AddHandler Dialoguer.events.onWindowClose, AddressOf Me.OnDialogueWindowCloseHandler
		AddHandler Dialoguer.events.onMessageEvent, AddressOf Me.OnDialoguerMessageEvent
	End Sub

	' Token: 0x06000F8E RID: 3982 RVA: 0x0009B01C File Offset: 0x0009941C
	Public Sub RemoveDialoguerEvents()
		RemoveHandler Dialoguer.events.onStarted, AddressOf Me.OnDialogueStartedHandler
		RemoveHandler Dialoguer.events.onEnded, AddressOf Me.OnDialogueEndedHandler
		RemoveHandler Dialoguer.events.onInstantlyEnded, AddressOf Me.OnDialogueInstantlyEndedHandler
		RemoveHandler Dialoguer.events.onTextPhase, AddressOf Me.OnDialogueTextPhaseHandler
		RemoveHandler Dialoguer.events.onWindowClose, AddressOf Me.OnDialogueWindowCloseHandler
		RemoveHandler Dialoguer.events.onMessageEvent, AddressOf Me.OnDialoguerMessageEvent
	End Sub

	' Token: 0x06000F8F RID: 3983 RVA: 0x0009B0B0 File Offset: 0x000994B0
	Private Sub OnDialogueStartedHandler()
		AddHandler Localization.OnLanguageChangedEvent, AddressOf Me.OnLanguageChanged
		If Map.Current IsNot Nothing Then
			Map.Current.CurrentState = Map.State.[Event]
		End If
		If CupheadMapCamera.Current IsNot Nothing Then
			CupheadMapCamera.Current.MoveToPosition(Me.panPosition, 0.75F, 1F)
		End If
		If MapUIVignetteDialogue.Current IsNot Nothing Then
			MapUIVignetteDialogue.Current.FadeIn()
		End If
	End Sub

	' Token: 0x06000F90 RID: 3984 RVA: 0x0009B12E File Offset: 0x0009952E
	Private Sub OnDialogueEndedHandler()
		RemoveHandler Localization.OnLanguageChangedEvent, AddressOf Me.OnLanguageChanged
		Me.Dismiss()
	End Sub

	' Token: 0x06000F91 RID: 3985 RVA: 0x0009B147 File Offset: 0x00099547
	Private Sub OnDialogueInstantlyEndedHandler()
		RemoveHandler Localization.OnLanguageChangedEvent, AddressOf Me.OnLanguageChanged
		Me.Dismiss()
	End Sub

	' Token: 0x06000F92 RID: 3986 RVA: 0x0009B160 File Offset: 0x00099560
	Private Sub OnDialogueTextPhaseHandler(data As DialoguerTextData)
		Me.data = data
		If data.choices Is Nothing Then
			Me.Show(data.text)
		ElseIf data.choices.Length > 0 Then
			Dim list As List(Of String) = New List(Of String)()
			For i As Integer = 0 To data.choices.Length - 1
				If Not Me.OptionHidden(i) Then
					list.Add(data.choices(i))
				End If
			Next
			Me.Show(data.text, list)
		End If
	End Sub

	' Token: 0x06000F93 RID: 3987 RVA: 0x0009B1EA File Offset: 0x000995EA
	Public Sub ClearHideOptionBitmask()
		Me.hideOptionBitmask = 0
	End Sub

	' Token: 0x06000F94 RID: 3988 RVA: 0x0009B1F3 File Offset: 0x000995F3
	Public Sub HideOptionByIndex(i As Integer)
		Me.hideOptionBitmask = Me.hideOptionBitmask Or 1 << i
	End Sub

	' Token: 0x06000F95 RID: 3989 RVA: 0x0009B208 File Offset: 0x00099608
	Private Function OptionHidden(i As Integer) As Boolean
		Return(Me.hideOptionBitmask And (1 << i)) <> 0
	End Function

	' Token: 0x06000F96 RID: 3990 RVA: 0x0009B220 File Offset: 0x00099620
	Private Sub OnDialogueWindowCloseHandler()
		Me.Dismiss()
		Me.ClearHideOptionBitmask()
		If MapUIVignetteDialogue.Current IsNot Nothing Then
			MapUIVignetteDialogue.Current.FadeOut()
		End If
		If Map.Current IsNot Nothing AndAlso Map.Current.CurrentState <> Map.State.Graveyard Then
			Map.Current.CurrentState = Map.State.Ready
		End If
	End Sub

	' Token: 0x06000F97 RID: 3991 RVA: 0x0009B27E File Offset: 0x0009967E
	Private Sub OnDialoguerMessageEvent(message As String, metadata As String)
		If message = "Wait" Then
			MyBase.StartCoroutine(Me.wait_cr(Parser.FloatParse(metadata)))
		End If
	End Sub

	' Token: 0x06000F98 RID: 3992 RVA: 0x0009B2A4 File Offset: 0x000996A4
	Private Iterator Function wait_cr(waitDuration As Single) As IEnumerator
		Me.waiting = True
		Me.arrow.color = New Color(1F, 1F, 1F, 0F)
		While waitDuration > 0F
			Yield Nothing
			waitDuration -= CupheadTime.Delta
		End While
		Me.waiting = False
		Me.arrow.color = New Color(1F, 1F, 1F, 1F)
		Return
	End Function

	' Token: 0x04001892 RID: 6290
	Private Const REGULAR_ARROW_PADDING As Integer = 8

	' Token: 0x04001893 RID: 6291
	Private Const KOREAN_ARROW_PADDING As Integer = 6

	' Token: 0x04001894 RID: 6292
	Private Const JAP_CHI_ARROW_PADDING As Integer = 5

	' Token: 0x04001895 RID: 6293
	Private Const DEFAULT_TIME As Single = 2F

	' Token: 0x04001896 RID: 6294
	Private Const FADE_TIME As Single = 0.07F

	' Token: 0x04001897 RID: 6295
	Private Const END_TIME As Single = 0.25F

	' Token: 0x04001898 RID: 6296
	Private Const ARROW_WAIT_TIME As Single = 0.125F

	' Token: 0x04001899 RID: 6297
	Private Const MAX_RANDOM_OFFSET As Single = 0.05F

	' Token: 0x0400189A RID: 6298
	Private Const MAX_CHOICES_PER_COLUMN As Integer = 4

	' Token: 0x0400189B RID: 6299
	Private Const COLUMN_PADDING As Integer = 55

	' Token: 0x0400189C RID: 6300
	Private Const COLUMN_SPACING As Integer = 45

	' Token: 0x0400189D RID: 6301
	Private Const DEFAULT_PADDING As Integer = 30

	' Token: 0x0400189E RID: 6302
	Private Const SMALL_PADDING As Integer = 20

	' Token: 0x0400189F RID: 6303
	Private Const TAIL_POSITION_X As Single = -73F

	' Token: 0x040018A0 RID: 6304
	Private Const CURSOR_OFFSET_H As Single = 10F

	' Token: 0x040018A3 RID: 6307
	Public Shared Instance As SpeechBubble

	' Token: 0x040018A4 RID: 6308
	<SerializeField()>
	Private mainText As TextMeshProUGUI

	' Token: 0x040018A5 RID: 6309
	<SerializeField()>
	Private choiceText As TextMeshProUGUI

	' Token: 0x040018A6 RID: 6310
	<SerializeField()>
	Private layout As VerticalLayoutGroup

	' Token: 0x040018A7 RID: 6311
	<SerializeField()>
	Private tail As Image

	' Token: 0x040018A8 RID: 6312
	<SerializeField()>
	Private tailVariants As List(Of Sprite)

	' Token: 0x040018A9 RID: 6313
	<SerializeField()>
	Private arrowBox As RectTransform

	' Token: 0x040018AA RID: 6314
	<SerializeField()>
	Private arrow As Image

	' Token: 0x040018AB RID: 6315
	<SerializeField()>
	Private cursor As Image

	' Token: 0x040018AC RID: 6316
	<SerializeField()>
	Private cursorRoot As RectTransform

	' Token: 0x040018AD RID: 6317
	<SerializeField()>
	Private box As RectTransform

	' Token: 0x040018AE RID: 6318
	<SerializeField()>
	Private arrowVariants As List(Of Sprite)

	' Token: 0x040018AF RID: 6319
	<SerializeField()>
	Private canvasGroup As CanvasGroup

	' Token: 0x040018B0 RID: 6320
	<SerializeField()>
	Private bullets As List(Of RectTransform)

	' Token: 0x040018B1 RID: 6321
	Private maxWidth As Single = 558F

	' Token: 0x040018B2 RID: 6322
	Private arrowAnchoredPosition As Vector2

	' Token: 0x040018B3 RID: 6323
	Public basePosition As Vector2

	' Token: 0x040018B4 RID: 6324
	Public panPosition As Vector2

	' Token: 0x040018B5 RID: 6325
	Private currentChoiceIndex As Integer

	' Token: 0x040018B6 RID: 6326
	Private hideOptionBitmask As Integer

	' Token: 0x040018B7 RID: 6327
	Public setBossRefText As String = String.Empty

	' Token: 0x040018B8 RID: 6328
	Public maxLines As Integer = -1

	' Token: 0x040018B9 RID: 6329
	Public tailOnTheLeft As Boolean

	' Token: 0x040018BA RID: 6330
	Public expandOnTheRight As Boolean

	' Token: 0x040018BB RID: 6331
	Private input As CupheadInput.AnyPlayerInput

	' Token: 0x040018BC RID: 6332
	Public waitForRealease As Boolean

	' Token: 0x040018BD RID: 6333
	Public waitForFade As Boolean

	' Token: 0x040018BE RID: 6334
	Public hideTail As Boolean

	' Token: 0x040018BF RID: 6335
	Private showCoroutine As Coroutine

	' Token: 0x040018C0 RID: 6336
	<SerializeField()>
	Private textLayoutElement As LayoutElement

	' Token: 0x040018C1 RID: 6337
	Private waiting As Boolean

	' Token: 0x040018C2 RID: 6338
	Private delayedShow As Boolean

	' Token: 0x040018C3 RID: 6339
	Private data As DialoguerTextData

	' Token: 0x040018C4 RID: 6340
	<HideInInspector()>
	Public preventQuit As Boolean

	' Token: 0x0200042B RID: 1067
	Public Enum Mode
		' Token: 0x040018C6 RID: 6342
		Text
		' Token: 0x040018C7 RID: 6343
		ListChoice
	End Enum

	' Token: 0x0200042C RID: 1068
	Public Enum DisplayState
		' Token: 0x040018C9 RID: 6345
		Hidden
		' Token: 0x040018CA RID: 6346
		FadeIn
		' Token: 0x040018CB RID: 6347
		Showing
		' Token: 0x040018CC RID: 6348
		WaitForSelection
		' Token: 0x040018CD RID: 6349
		FadeOut
	End Enum
End Class
