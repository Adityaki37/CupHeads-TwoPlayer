Imports System
Imports UnityEngine

' Token: 0x02000B8C RID: 2956
Public Class LegacyNextGenRpgGui
	Inherits MonoBehaviour

	' Token: 0x060047FB RID: 18427 RVA: 0x0025CDF6 File Offset: 0x0025B1F6
	Private Sub Awake()
		Dialoguer.Initialize()
	End Sub

	' Token: 0x060047FC RID: 18428 RVA: 0x0025CDFD File Offset: 0x0025B1FD
	Private Sub Start()
		Me.addDialoguerEvents()
		Me._dialogue = False
	End Sub

	' Token: 0x060047FD RID: 18429 RVA: 0x0025CE0C File Offset: 0x0025B20C
	Private Sub Update()
		If Me._showWindow AndAlso Input.GetMouseButtonDown(0) Then
			If Me._choices IsNot Nothing Then
				Me.audioSelect.Play()
			End If
			Dialoguer.ContinueDialogue(Me._currentChoice)
		End If
	End Sub

	' Token: 0x060047FE RID: 18430 RVA: 0x0025CE48 File Offset: 0x0025B248
	Public Sub addDialoguerEvents()
		AddHandler Dialoguer.events.onStarted, AddressOf Me.onDialogueStartedHandler
		AddHandler Dialoguer.events.onEnded, AddressOf Me.onDialogueEndedHandler
		AddHandler Dialoguer.events.onInstantlyEnded, AddressOf Me.onDialogueInstantlyEndedHandler
		AddHandler Dialoguer.events.onTextPhase, AddressOf Me.onDialogueTextPhaseHandler
		AddHandler Dialoguer.events.onWindowClose, AddressOf Me.onDialogueWindowCloseHandler
		AddHandler Dialoguer.events.onMessageEvent, AddressOf Me.onDialoguerMessageEvent
	End Sub

	' Token: 0x060047FF RID: 18431 RVA: 0x0025CED9 File Offset: 0x0025B2D9
	Private Sub onDialogueStartedHandler()
		Me._dialogue = True
	End Sub

	' Token: 0x06004800 RID: 18432 RVA: 0x0025CEE2 File Offset: 0x0025B2E2
	Private Sub onDialogueEndedHandler()
		Me._dialogue = False
		Me._showWindow = False
	End Sub

	' Token: 0x06004801 RID: 18433 RVA: 0x0025CEF2 File Offset: 0x0025B2F2
	Private Sub onDialogueInstantlyEndedHandler()
		Me._dialogue = False
		Me._showWindow = False
	End Sub

	' Token: 0x06004802 RID: 18434 RVA: 0x0025CF04 File Offset: 0x0025B304
	Private Sub onDialogueTextPhaseHandler(data As DialoguerTextData)
		Me._currentChoice = 0
		If data.choices IsNot Nothing Then
			Me._choices = New String(5) {}
			For i As Integer = 0 To 6 - 1
				If data.choices.Length > i AndAlso data.choices(i) IsNot Nothing Then
					Me._choices(i) = data.choices(i)
					Me._currentChoice = i
				End If
			Next
		Else
			Me._choices = Nothing
		End If
		Me._text = data.text
		If data.name IsNot Nothing AndAlso data.name <> String.Empty Then
			Me._text = data.name + ": " + Me._text
		End If
		Me._showWindow = True
	End Sub

	' Token: 0x06004803 RID: 18435 RVA: 0x0025CFD6 File Offset: 0x0025B3D6
	Private Sub onDialogueWindowCloseHandler()
		Me._showWindow = False
	End Sub

	' Token: 0x06004804 RID: 18436 RVA: 0x0025CFDF File Offset: 0x0025B3DF
	Private Sub onDialoguerMessageEvent(message As String, metadata As String)
	End Sub

	' Token: 0x06004805 RID: 18437 RVA: 0x0025CFE4 File Offset: 0x0025B3E4
	Private Sub OnGUI()
		If Not Me._dialogue Then
			Return
		End If
		If Not Me._showWindow Then
			Return
		End If
		GUI.skin = Me.guiSkin
		Dim num As Integer = 260
		Dim rect As Rect = New Rect(CSng(Screen.width) * 0.5F - 300F, CSng((Screen.height - num)), 600F, 80F)
		Dim guistyle As GUIStyle = New GUIStyle("label")
		guistyle.alignment = TextAnchor.MiddleCenter
		Me.drawText(Me._text, rect, guistyle)
		If Me._choices IsNot Nothing Then
			Me.drawChoiceRing()
		End If
	End Sub

	' Token: 0x06004806 RID: 18438 RVA: 0x0025D07C File Offset: 0x0025B47C
	Private Sub drawText(text As String, rect As Rect)
		Dim guistyle As GUIStyle = New GUIStyle("label")
		Me.drawText(text, rect, guistyle)
	End Sub

	' Token: 0x06004807 RID: 18439 RVA: 0x0025D0A4 File Offset: 0x0025B4A4
	Private Sub drawText(text As String, rect As Rect, style As GUIStyle)
		GUI.color = Color.black
		For i As Integer = 0 To LegacyNextGenRpgGui.TEXT_OUTLINE_WIDTH - 1
			For j As Integer = 0 To LegacyNextGenRpgGui.TEXT_OUTLINE_WIDTH - 1
				GUI.Label(New Rect(rect.x + CSng((i + 1)), rect.y + CSng((j + 1)), rect.width, rect.height), text, style)
				GUI.Label(New Rect(rect.x - CSng((i + 1)), rect.y - CSng((j + 1)), rect.width, rect.height), text, style)
				GUI.Label(New Rect(rect.x + CSng((i + 1)), rect.y - CSng((j + 1)), rect.width, rect.height), text, style)
				GUI.Label(New Rect(rect.x - CSng((i + 1)), rect.y + CSng((j + 1)), rect.width, rect.height), text, style)
			Next
		Next
		GUI.color = GUI.contentColor
		GUI.Label(rect, text, style)
	End Sub

	' Token: 0x06004808 RID: 18440 RVA: 0x0025D1C4 File Offset: 0x0025B5C4
	Private Sub drawChoiceRing()
		Dim rect As Rect = New Rect(CSng(Screen.width) * 0.5F - 128F, CSng((Screen.height - 128 - 50)), 256F, 128F)
		If Me._ringeRects Is Nothing Then
			Me._ringeRects = New Rect(5) {}
			Me._ringeRects(0) = New Rect(rect.center.x, rect.y - 40F, CSng(Screen.width) * 0.5F, rect.height * 0.3333F + 40F)
			Me._ringeRects(1) = New Rect(rect.center.x, rect.y + rect.height * 0.3333F, CSng(Screen.width) * 0.5F, rect.height * 0.3333F)
			Me._ringeRects(2) = New Rect(rect.center.x, rect.y + rect.height * 0.3333F * 2F, CSng(Screen.width) * 0.5F, rect.height * 0.3333F + 40F)
			Me._ringeRects(3) = New Rect(0F, rect.y - 40F, CSng(Screen.width) * 0.5F, rect.height * 0.3333F + 40F)
			Me._ringeRects(4) = New Rect(0F, rect.y + rect.height * 0.3333F, CSng(Screen.width) * 0.5F, rect.height * 0.3333F)
			Me._ringeRects(5) = New Rect(0F, rect.y + rect.height * 0.3333F * 2F, CSng(Screen.width) * 0.5F, rect.height * 0.3333F + 40F)
		End If
		If Me._choicesTextRects Is Nothing Then
			Me._choicesTextRects = New Rect(5) {}
			Me._choicesTextRects(0) = New Rect(rect.center.x + rect.width * 0.5F - 10F, rect.y, CSng(Screen.width) * 0.5F - rect.width * 0.5F + 10F, rect.height * 0.3333F)
			Me._choicesTextRects(1) = New Rect(rect.center.x + rect.width * 0.5F + 10F, rect.y + rect.height * 0.3333F - 5F, CSng(Screen.width) * 0.5F - rect.width * 0.5F - 10F, rect.height * 0.3333F)
			Me._choicesTextRects(2) = New Rect(rect.center.x + rect.width * 0.5F, rect.y + rect.height * 0.3333F * 2F, CSng(Screen.width) * 0.5F - rect.width * 0.5F, rect.height * 0.3333F)
			Me._choicesTextRects(3) = New Rect(0F, rect.y, CSng(Screen.width) * 0.5F - rect.width * 0.5F + 10F, rect.height * 0.3333F)
			Me._choicesTextRects(4) = New Rect(0F, rect.y + rect.height * 0.3333F - 5F, CSng(Screen.width) * 0.5F - rect.width * 0.5F - 10F, rect.height * 0.3333F)
			Me._choicesTextRects(5) = New Rect(0F, rect.y + rect.height * 0.3333F * 2F, CSng(Screen.width) * 0.5F - rect.width * 0.5F, rect.height * 0.3333F)
		End If
		GUI.DrawTexture(rect, Me.ringBase)
		For i As Integer = 0 To 6 - 1
			If Me._choices(i) IsNot Nothing AndAlso Me._choices(i) <> String.Empty Then
				If Me._currentChoice <> i AndAlso Me._ringeRects(i).Contains(New Vector2(Input.mousePosition.x, CSng(Screen.height) - Input.mousePosition.y)) Then
					Me._currentChoice = i
					Me.audioChoice.PlayOneShot(Me.audioChoice.clip)
				End If
				If Me._currentChoice = i Then
					GUI.DrawTexture(rect, Me.ringHover.getPieces()(i))
				Else
					GUI.DrawTexture(rect, Me.ringNormal.getPieces()(i))
				End If
				Dim guistyle As GUIStyle = New GUIStyle("label")
				If i > 2 Then
					guistyle.alignment = TextAnchor.MiddleRight
				Else
					guistyle.alignment = TextAnchor.MiddleLeft
				End If
				Me.drawText(Me._choices(i), Me._choicesTextRects(i), guistyle)
			End If
		Next
	End Sub

	' Token: 0x04004D32 RID: 19762
	Private Shared TEXT_OUTLINE_WIDTH As Integer = 1

	' Token: 0x04004D33 RID: 19763
	Public guiSkin As GUISkin

	' Token: 0x04004D34 RID: 19764
	Public audioChoice As AudioSource

	' Token: 0x04004D35 RID: 19765
	Public audioSelect As AudioSource

	' Token: 0x04004D36 RID: 19766
	Public ringBase As Texture

	' Token: 0x04004D37 RID: 19767
	Public ringTop As Texture

	' Token: 0x04004D38 RID: 19768
	Public ringBottom As Texture

	' Token: 0x04004D39 RID: 19769
	Public ringNormal As NextGenRingPieces

	' Token: 0x04004D3A RID: 19770
	Public ringHover As NextGenRingPieces

	' Token: 0x04004D3B RID: 19771
	Private _currentChoice As Integer

	' Token: 0x04004D3C RID: 19772
	Private _ringeRects As Rect()

	' Token: 0x04004D3D RID: 19773
	Private _choicesTextRects As Rect()

	' Token: 0x04004D3E RID: 19774
	Private _dialogue As Boolean

	' Token: 0x04004D3F RID: 19775
	Private _showWindow As Boolean

	' Token: 0x04004D40 RID: 19776
	Private _text As String

	' Token: 0x04004D41 RID: 19777
	Private _choices As String()
End Class
