Imports System
Imports System.Collections.Generic
Imports DialoguerCore
Imports UnityEngine

' Token: 0x02000B70 RID: 2928
Public Structure DialoguerTextData
	' Token: 0x0600469B RID: 18075 RVA: 0x0024E998 File Offset: 0x0024CD98
	Public Sub New(text As String, themeName As String, newWindow As Boolean, name As String, portrait As String, metadata As String, audio As String, audioDelay As Single, rect As Rect, choices As List(Of String), dialogueID As Integer, nodeID As Integer)
		Me.dialogueID = dialogueID
		Me.nodeID = nodeID
		Me.rawText = text
		Me.theme = themeName
		Me.newWindow = newWindow
		Me.name = name
		Me.portrait = portrait
		Me.metadata = metadata
		Me.audio = audio
		Me.audioDelay = audioDelay
		Me.rect = New Rect(rect.x, rect.y, rect.width, rect.height)
		If choices IsNot Nothing Then
			Dim array As String() = choices.ToArray()
			Me.choices = TryCast(array.Clone(), String())
		Else
			Me.choices = Nothing
		End If
		Me._cachedText = Nothing
	End Sub

	' Token: 0x1700063F RID: 1599
	' (get) Token: 0x0600469C RID: 18076 RVA: 0x0024EA4C File Offset: 0x0024CE4C
	Public ReadOnly Property text As String
		Get
			If Me._cachedText Is Nothing Then
				Me._cachedText = DialoguerUtils.insertTextPhaseStringVariables(Me.rawText)
			End If
			Return Me._cachedText
		End Get
	End Property

	' Token: 0x17000640 RID: 1600
	' (get) Token: 0x0600469D RID: 18077 RVA: 0x0024EA70 File Offset: 0x0024CE70
	Public ReadOnly Property usingPositionRect As Boolean
		Get
			Return Me.rect.x <> 0F OrElse Me.rect.y <> 0F OrElse Me.rect.width <> 0F OrElse Me.rect.height <> 0F
		End Get
	End Property

	' Token: 0x17000641 RID: 1601
	' (get) Token: 0x0600469E RID: 18078 RVA: 0x0024EAE0 File Offset: 0x0024CEE0
	Public ReadOnly Property windowType As DialoguerTextPhaseType
		Get
			Return If((Me.choices IsNot Nothing), DialoguerTextPhaseType.BranchedText, DialoguerTextPhaseType.Text)
		End Get
	End Property

	' Token: 0x0600469F RID: 18079 RVA: 0x0024EAF4 File Offset: 0x0024CEF4
	Public Overrides Function ToString() As String
		Return String.Concat(New Object() { vbLf & "Theme ID: ", Me.theme, vbLf & "New Window: ", Me.newWindow.ToString(), vbLf & "Name: ", Me.name, vbLf & "Portrait: ", Me.portrait, vbLf & "Metadata: ", Me.metadata, vbLf & "Audio Clip: ", Me.audio, vbLf & "Audio Delay: ", Me.audioDelay.ToString(), vbLf & "Rect: ", Me.rect.ToString(), vbLf & "Raw Text: ", Me.rawText, vbLf & "Dialogue ID:", Me.dialogueID, vbLf & "Node ID:", Me.nodeID })
	End Function

	' Token: 0x04004C7B RID: 19579
	Public dialogueID As Integer

	' Token: 0x04004C7C RID: 19580
	Public nodeID As Integer

	' Token: 0x04004C7D RID: 19581
	Public rawText As String

	' Token: 0x04004C7E RID: 19582
	Public theme As String

	' Token: 0x04004C7F RID: 19583
	Public newWindow As Boolean

	' Token: 0x04004C80 RID: 19584
	Public name As String

	' Token: 0x04004C81 RID: 19585
	Public portrait As String

	' Token: 0x04004C82 RID: 19586
	Public metadata As String

	' Token: 0x04004C83 RID: 19587
	Public audio As String

	' Token: 0x04004C84 RID: 19588
	Public audioDelay As Single

	' Token: 0x04004C85 RID: 19589
	Public rect As Rect

	' Token: 0x04004C86 RID: 19590
	Public choices As String()

	' Token: 0x04004C87 RID: 19591
	Private _cachedText As String
End Structure
