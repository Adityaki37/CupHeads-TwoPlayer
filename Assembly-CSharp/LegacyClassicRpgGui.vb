Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000B8B RID: 2955
Public Class LegacyClassicRpgGui
	Inherits MonoBehaviour

	' Token: 0x060047E3 RID: 18403 RVA: 0x0025C18E File Offset: 0x0025A58E
	Private Sub Awake()
		Dialoguer.Initialize()
	End Sub

	' Token: 0x060047E4 RID: 18404 RVA: 0x0025C195 File Offset: 0x0025A595
	Private Sub Start()
		Me.addDialoguerEvents()
		Me._showDialogueBox = False
	End Sub

	' Token: 0x060047E5 RID: 18405 RVA: 0x0025C1A4 File Offset: 0x0025A5A4
	Private Sub Update()
		If Not Me._dialogue Then
			Return
		End If
		If Me._windowReady Then
			Me.calculateText()
		End If
		If Not Me._dialogue OrElse Me._ending Then
			Return
		End If
		If Not Me._isBranchedText Then
			If Input.GetMouseButtonDown(0) OrElse Input.GetKeyDown(KeyCode.X) OrElse Input.GetKeyDown(KeyCode.Space) OrElse Input.GetKeyDown(KeyCode.[Return]) Then
				If Me._windowCurrentText = Me._windowTargetText Then
					Dialoguer.ContinueDialogue(0)
				Else
					Me._windowCurrentText = Me._windowTargetText
					Me.audioTextEnd.Play()
				End If
			End If
		Else
			If Input.GetKeyDown(KeyCode.DownArrow) Then
				Me._currentChoice = CInt(Mathf.Repeat(CSng((Me._currentChoice + 1)), CSng(Me._branchedTextChoices.Length)))
				Me.audioText.Play()
			End If
			If Input.GetKeyDown(KeyCode.UpArrow) Then
				Me._currentChoice = CInt(Mathf.Repeat(CSng((Me._currentChoice - 1)), CSng(Me._branchedTextChoices.Length)))
				Me.audioText.Play()
			End If
			If Input.GetMouseButtonDown(0) AndAlso Me._windowCurrentText <> Me._windowTargetText Then
				Me._windowCurrentText = Me._windowTargetText
			End If
			If Input.GetKeyDown(KeyCode.X) OrElse Input.GetKeyDown(KeyCode.Space) OrElse Input.GetKeyDown(KeyCode.[Return]) Then
				If Me._windowCurrentText = Me._windowTargetText Then
					Dialoguer.ContinueDialogue(Me._currentChoice)
				Else
					Me._windowCurrentText = Me._windowTargetText
					Me.audioTextEnd.Play()
				End If
			End If
		End If
	End Sub

	' Token: 0x060047E6 RID: 18406 RVA: 0x0025C35C File Offset: 0x0025A75C
	Public Sub addDialoguerEvents()
		AddHandler Dialoguer.events.onStarted, AddressOf Me.onDialogueStartedHandler
		AddHandler Dialoguer.events.onEnded, AddressOf Me.onDialogueEndedHandler
		AddHandler Dialoguer.events.onInstantlyEnded, AddressOf Me.onDialogueInstantlyEndedHandler
		AddHandler Dialoguer.events.onTextPhase, AddressOf Me.onDialogueTextPhaseHandler
		AddHandler Dialoguer.events.onWindowClose, AddressOf Me.onDialogueWindowCloseHandler
		AddHandler Dialoguer.events.onMessageEvent, AddressOf Me.onDialoguerMessageEvent
	End Sub

	' Token: 0x060047E7 RID: 18407 RVA: 0x0025C3ED File Offset: 0x0025A7ED
	Private Sub onDialogueStartedHandler()
		Me._dialogue = True
	End Sub

	' Token: 0x060047E8 RID: 18408 RVA: 0x0025C3F6 File Offset: 0x0025A7F6
	Private Sub onDialogueEndedHandler()
		Me._ending = True
		Me.audioTextEnd.Play()
	End Sub

	' Token: 0x060047E9 RID: 18409 RVA: 0x0025C40A File Offset: 0x0025A80A
	Private Sub onDialogueInstantlyEndedHandler()
		Me._dialogue = False
		Me._showDialogueBox = False
		Me.resetWindowSize()
	End Sub

	' Token: 0x060047EA RID: 18410 RVA: 0x0025C420 File Offset: 0x0025A820
	Private Sub onDialogueTextPhaseHandler(data As DialoguerTextData)
		Me._usingPositionRect = data.usingPositionRect
		Me._positionRect = data.rect
		Me._windowCurrentText = String.Empty
		Me._windowTargetText = data.text
		Me._nameText = data.name
		Me._showDialogueBox = True
		Me._isBranchedText = data.windowType = DialoguerTextPhaseType.BranchedText
		Me._branchedTextChoices = data.choices
		Me._currentChoice = 0
		If data.theme <> Me._theme Then
			Me.resetWindowSize()
		End If
		Me._theme = data.theme
		Me.startWindowTweenIn()
	End Sub

	' Token: 0x060047EB RID: 18411 RVA: 0x0025C4C7 File Offset: 0x0025A8C7
	Private Sub onDialogueWindowCloseHandler()
		Me.startWindowTweenOut()
	End Sub

	' Token: 0x060047EC RID: 18412 RVA: 0x0025C4CF File Offset: 0x0025A8CF
	Private Sub onDialoguerMessageEvent(message As String, metadata As String)
		If message = "playOldRpgSound" Then
			Me.playOldRpgSound(metadata)
		End If
	End Sub

	' Token: 0x060047ED RID: 18413 RVA: 0x0025C4E8 File Offset: 0x0025A8E8
	Private Sub OnGUI()
		If Not Me._showDialogueBox Then
			Return
		End If
		GUI.skin = Me.skin
		GUI.depth = 10
		Dim num As Single = If(Me._usingPositionRect, Me._positionRect.x, (CSng(Screen.width) * 0.5F))
		Dim num2 As Single = If(Me._usingPositionRect, Me._positionRect.y, CSng((Screen.height - 100)))
		Dim num3 As Single = If(Me._usingPositionRect, Me._positionRect.width, 512F)
		Dim num4 As Single = If(Me._usingPositionRect, Me._positionRect.height, 190F)
		Dim rect As Rect = Me.centerRect(New Rect(num, num2, num3 * Me._windowTweenValue, num4 * Me._windowTweenValue))
		rect.width = Mathf.Clamp(rect.width, 32F, 2000F)
		rect.height = Mathf.Clamp(rect.height, 32F, 2000F)
		If Me._theme = "good" Then
			Me.drawDialogueBox(rect, New Color(0.2F, 0.8F, 0.4F))
		ElseIf Me._theme = "bad" Then
			Me.drawDialogueBox(rect, New Color(0.8F, 0.2F, 0.2F))
		Else
			Me.drawDialogueBox(rect)
		End If
		If Me._nameText <> String.Empty Then
			Dim rect2 As Rect = New Rect(rect.x, rect.y - 60F, 150F * Me._windowTweenValue, 50F * Me._windowTweenValue)
			rect2.width = Mathf.Clamp(rect2.width, 32F, 2000F)
			rect2.height = Mathf.Clamp(rect2.height, 32F, 2000F)
			Me.drawDialogueBox(rect2)
			Me.drawShadowedText(New Rect(rect2.x + 15F * Me._windowTweenValue - 5F * (1F - Me._windowTweenValue), rect2.y + 5F * Me._windowTweenValue - 10F * (1F - Me._windowTweenValue), rect2.width - 30F * Me._windowTweenValue, rect2.height - 5F * Me._windowTweenValue), Me._nameText)
		End If
		Dim rect3 As Rect = New Rect(rect.x + 20F * Me._windowTweenValue, rect.y + 10F * Me._windowTweenValue, rect.width - 40F * Me._windowTweenValue, rect.height - 20F * Me._windowTweenValue)
		Me.drawShadowedText(rect3, Me._windowCurrentText)
		If Me._isBranchedText AndAlso Me._windowCurrentText = Me._windowTargetText AndAlso Me._branchedTextChoices IsNot Nothing Then
			For i As Integer = 0 To Me._branchedTextChoices.Length - 1
				Dim num5 As Single = rect.yMax - CSng((38 * Me._branchedTextChoices.Length - 38 * i)) - 20F
				Dim rect4 As Rect = New Rect(rect.x + 60F, num5, rect.width - 80F, 38F)
				Me.drawShadowedText(rect4, Me._branchedTextChoices(i))
				If rect4.Contains(New Vector2(Input.mousePosition.x, CSng(Screen.height) - Input.mousePosition.y)) Then
					If Me._currentChoice <> i Then
						Me.audioText.Play()
						Me._currentChoice = i
					End If
					If Input.GetMouseButtonDown(0) Then
						Dialoguer.ContinueDialogue(Me._currentChoice)
						Exit For
					End If
				End If
				If Me._currentChoice = i Then
					GUI.Box(New Rect(rect4.x - 64F, rect4.y, 64F, 64F), String.Empty, GUI.skin.GetStyle("box_cursor"))
				End If
			Next
		End If
	End Sub

	' Token: 0x060047EE RID: 18414 RVA: 0x0025C941 File Offset: 0x0025AD41
	Private Sub drawDialogueBox(rect As Rect)
		Me.drawDialogueBox(rect, New Color(0.1764706F, 0.43529412F, 1F))
	End Sub

	' Token: 0x060047EF RID: 18415 RVA: 0x0025C960 File Offset: 0x0025AD60
	Private Sub drawDialogueBox(rect As Rect, color As Color)
		GUI.color = color
		GUI.Box(rect, String.Empty, GUI.skin.GetStyle("box_background"))
		GUI.color = GUI.contentColor
		GUI.color = New Color(0F, 0F, 0F, 0.25F)
		Dim rect2 As Rect = New Rect(rect.x + 7F, rect.y + 7F, rect.width - 14F, rect.height - 14F)
		GUI.DrawTextureWithTexCoords(rect2, Me.diagonalLines, New Rect(0F, 0F, rect2.width / CSng(Me.diagonalLines.width), rect2.height / CSng(Me.diagonalLines.height)))
		GUI.color = GUI.contentColor
		GUI.depth = 20
		GUI.Box(rect, String.Empty, GUI.skin.GetStyle("box_border"))
		GUI.depth = 10
	End Sub

	' Token: 0x060047F0 RID: 18416 RVA: 0x0025CA68 File Offset: 0x0025AE68
	Private Sub drawShadowedText(rect As Rect, text As String)
		GUI.color = New Color(0F, 0F, 0F, 0.5F)
		GUI.Label(New Rect(rect.x + 1F, rect.y + 2F, rect.width, rect.height), text)
		GUI.color = GUI.contentColor
		GUI.Label(rect, text)
	End Sub

	' Token: 0x060047F1 RID: 18417 RVA: 0x0025CAD7 File Offset: 0x0025AED7
	Private Sub playOldRpgSound(metadata As String)
		If metadata = "good" Then
			Me.audioGood.Play()
		ElseIf metadata = "bad" Then
			Me.audioBad.Play()
		End If
	End Sub

	' Token: 0x060047F2 RID: 18418 RVA: 0x0025CB14 File Offset: 0x0025AF14
	Private Sub resetWindowSize()
		Me._windowTweenValue = 0F
		Me._windowReady = False
	End Sub

	' Token: 0x060047F3 RID: 18419 RVA: 0x0025CB28 File Offset: 0x0025AF28
	Private Sub startWindowTweenIn()
		Me._showDialogueBox = True
		DialogueriTween.ValueTo(MyBase.gameObject, New Hashtable() From { { "from", Me._windowTweenValue }, { "to", 1 }, { "onupdatetarget", MyBase.gameObject }, { "onupdate", "updateWindowTweenValue" }, { "oncompletetarget", MyBase.gameObject }, { "oncomplete", "windowInComplete" }, { "time", 0.5F }, { "easetype", DialogueriTween.EaseType.easeOutBack } })
	End Sub

	' Token: 0x060047F4 RID: 18420 RVA: 0x0025CBE0 File Offset: 0x0025AFE0
	Private Sub startWindowTweenOut()
		Me._windowReady = False
		DialogueriTween.ValueTo(MyBase.gameObject, New Hashtable() From { { "from", Me._windowTweenValue }, { "to", 0 }, { "onupdatetarget", MyBase.gameObject }, { "onupdate", "updateWindowTweenValue" }, { "oncompletetarget", MyBase.gameObject }, { "oncomplete", "windowOutComplete" }, { "time", 0.5F }, { "easetype", DialogueriTween.EaseType.easeInBack } })
	End Sub

	' Token: 0x060047F5 RID: 18421 RVA: 0x0025CC96 File Offset: 0x0025B096
	Private Sub updateWindowTweenValue(newValue As Single)
		Me._windowTweenValue = newValue
	End Sub

	' Token: 0x060047F6 RID: 18422 RVA: 0x0025CC9F File Offset: 0x0025B09F
	Private Sub windowInComplete()
		Me._windowReady = True
	End Sub

	' Token: 0x060047F7 RID: 18423 RVA: 0x0025CCA8 File Offset: 0x0025B0A8
	Private Sub windowOutComplete()
		Me._showDialogueBox = False
		Me.resetWindowSize()
		If Me._ending Then
			Me._dialogue = False
			Me._ending = False
		End If
	End Sub

	' Token: 0x060047F8 RID: 18424 RVA: 0x0025CCD0 File Offset: 0x0025B0D0
	Private Function centerRect(rect As Rect) As Rect
		Return New Rect(rect.x - rect.width * 0.5F, rect.y - rect.height * 0.5F, rect.width, rect.height)
	End Function

	' Token: 0x060047F9 RID: 18425 RVA: 0x0025CD10 File Offset: 0x0025B110
	Private Sub calculateText()
		If Me._windowTargetText = String.Empty OrElse Me._windowCurrentText = Me._windowTargetText Then
			Return
		End If
		Dim num As Integer = 2
		If Me._textFrames < num Then
			Me._textFrames += 1
			Return
		End If
		Me._textFrames = 0
		Dim num2 As Integer = 1
		If Me._windowCurrentText <> Me._windowTargetText Then
			For i As Integer = 0 To num2 - 1
				If Me._windowTargetText.Length <= Me._windowCurrentText.Length Then
					Exit For
				End If
				Me._windowCurrentText += Me._windowTargetText(Me._windowCurrentText.Length)
			Next
		End If
		Me.audioText.Play()
	End Sub

	' Token: 0x04004D1C RID: 19740
	Public skin As GUISkin

	' Token: 0x04004D1D RID: 19741
	Public diagonalLines As Texture2D

	' Token: 0x04004D1E RID: 19742
	Public audioText As AudioSource

	' Token: 0x04004D1F RID: 19743
	Public audioTextEnd As AudioSource

	' Token: 0x04004D20 RID: 19744
	Public audioGood As AudioSource

	' Token: 0x04004D21 RID: 19745
	Public audioBad As AudioSource

	' Token: 0x04004D22 RID: 19746
	Private _dialogue As Boolean

	' Token: 0x04004D23 RID: 19747
	Private _ending As Boolean

	' Token: 0x04004D24 RID: 19748
	Private _showDialogueBox As Boolean

	' Token: 0x04004D25 RID: 19749
	Private _usingPositionRect As Boolean

	' Token: 0x04004D26 RID: 19750
	Private _positionRect As Rect = New Rect(0F, 0F, 0F, 0F)

	' Token: 0x04004D27 RID: 19751
	Private _windowTargetText As String = String.Empty

	' Token: 0x04004D28 RID: 19752
	Private _windowCurrentText As String = String.Empty

	' Token: 0x04004D29 RID: 19753
	Private _nameText As String = String.Empty

	' Token: 0x04004D2A RID: 19754
	Private _isBranchedText As Boolean

	' Token: 0x04004D2B RID: 19755
	Private _branchedTextChoices As String()

	' Token: 0x04004D2C RID: 19756
	Private _currentChoice As Integer

	' Token: 0x04004D2D RID: 19757
	Private _theme As String

	' Token: 0x04004D2E RID: 19758
	Private _windowTweenValue As Single

	' Token: 0x04004D2F RID: 19759
	Private _windowReady As Boolean

	' Token: 0x04004D30 RID: 19760
	Private _nameTweenValue As Single

	' Token: 0x04004D31 RID: 19761
	Private _textFrames As Integer = Integer.MaxValue
End Class
