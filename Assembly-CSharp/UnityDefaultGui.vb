Imports System
Imports UnityEngine

' Token: 0x02000B8E RID: 2958
Public Class UnityDefaultGui
	Inherits MonoBehaviour

	' Token: 0x0600480D RID: 18445 RVA: 0x0025D86D File Offset: 0x0025BC6D
	Private Sub Awake()
		Dialoguer.Initialize()
	End Sub

	' Token: 0x0600480E RID: 18446 RVA: 0x0025D874 File Offset: 0x0025BC74
	Private Sub Start()
		Me.addDialoguerEvents()
	End Sub

	' Token: 0x0600480F RID: 18447 RVA: 0x0025D87C File Offset: 0x0025BC7C
	Private Sub OnGUI()
		If Not Me._showing Then
			Return
		End If
		If Not Me._windowShowing Then
			Return
		End If
		GUI.color = Me._guiColor
		GUI.depth = 10
		Dim rect As Rect = New Rect(CSng(Screen.width) * 0.5F - 250F, CSng(Screen.height) - 200F - 100F, 500F, 200F)
		Dim rect2 As Rect = New Rect(rect.x, rect.y, rect.width, rect.height - CSng((45 * Me._choices.Length)))
		GUI.Box(rect2, String.Empty)
		GUI.color = GUI.contentColor
		GUI.Label(New Rect(rect2.x + 10F, rect2.y + 10F, rect2.width - 20F, rect2.height - 20F), Me._windowText)
		If Me._selectionClicked Then
			Return
		End If
		For i As Integer = 0 To Me._choices.Length - 1
			Dim rect3 As Rect = New Rect(rect.x, rect.yMax - CSng((45 * (Me._choices.Length - i))) + 5F, rect.width, 40F)
			If GUI.Button(rect3, Me._choices(i)) Then
				Me._selectionClicked = True
				Dialoguer.ContinueDialogue(i)
			End If
		Next
		GUI.color = GUI.contentColor
	End Sub

	' Token: 0x06004810 RID: 18448 RVA: 0x0025D9F8 File Offset: 0x0025BDF8
	Public Sub addDialoguerEvents()
		AddHandler Dialoguer.events.onStarted, AddressOf Me.onStartedHandler
		AddHandler Dialoguer.events.onEnded, AddressOf Me.onEndedHandler
		AddHandler Dialoguer.events.onInstantlyEnded, AddressOf Me.onInstantlyEndedHandler
		AddHandler Dialoguer.events.onTextPhase, AddressOf Me.onTextPhaseHandler
		AddHandler Dialoguer.events.onWindowClose, AddressOf Me.onWindowCloseHandler
	End Sub

	' Token: 0x06004811 RID: 18449 RVA: 0x0025DA73 File Offset: 0x0025BE73
	Private Sub onStartedHandler()
		Me._showing = True
	End Sub

	' Token: 0x06004812 RID: 18450 RVA: 0x0025DA7C File Offset: 0x0025BE7C
	Private Sub onEndedHandler()
		Me._showing = False
	End Sub

	' Token: 0x06004813 RID: 18451 RVA: 0x0025DA85 File Offset: 0x0025BE85
	Private Sub onInstantlyEndedHandler()
		Me._showing = True
		Me._windowShowing = False
		Me._selectionClicked = False
	End Sub

	' Token: 0x06004814 RID: 18452 RVA: 0x0025DA9C File Offset: 0x0025BE9C
	Private Sub onTextPhaseHandler(data As DialoguerTextData)
		Me._guiColor = GUI.contentColor
		Me._windowText = data.text
		If data.windowType = DialoguerTextPhaseType.Text Then
			Me._choices = New String() { "Continue" }
		Else
			Me._choices = data.choices
		End If
		Dim theme As String = data.theme
		If theme IsNot Nothing Then
			If theme = "bad" Then
				Me._guiColor = Color.red
				GoTo IL_00AD
			End If
			If theme = "good" Then
				Me._guiColor = Color.green
				GoTo IL_00AD
			End If
		End If
		Me._guiColor = GUI.contentColor
		IL_00AD:
		Me._windowShowing = True
		Me._selectionClicked = False
	End Sub

	' Token: 0x06004815 RID: 18453 RVA: 0x0025DB64 File Offset: 0x0025BF64
	Private Sub onWindowCloseHandler()
		Me._windowShowing = False
		Me._selectionClicked = False
	End Sub

	' Token: 0x04004D49 RID: 19785
	Public Const HEIGHT As Single = 200F

	' Token: 0x04004D4A RID: 19786
	Public Const WIDTH As Single = 500F

	' Token: 0x04004D4B RID: 19787
	Private _showing As Boolean

	' Token: 0x04004D4C RID: 19788
	Private _windowShowing As Boolean

	' Token: 0x04004D4D RID: 19789
	Private _selectionClicked As Boolean

	' Token: 0x04004D4E RID: 19790
	Private _windowText As String = String.Empty

	' Token: 0x04004D4F RID: 19791
	Private _choices As String()

	' Token: 0x04004D50 RID: 19792
	Private _guiColor As Color
End Class
