Imports System
Imports System.Collections.Generic
Imports UnityEngine

Namespace DialoguerCore
	' Token: 0x02000B7B RID: 2939
	Public Class TextPhase
		Inherits AbstractDialoguePhase

		' Token: 0x060046C4 RID: 18116 RVA: 0x0024EEE0 File Offset: 0x0024D2E0
		Public Sub New(text As String, themeName As String, newWindow As Boolean, name As String, portrait As String, metadata As String, audio As String, audioDelay As Single, rect As Rect, outs As List(Of Integer), choices As List(Of String), dialogueID As Integer, nodeID As Integer)
			MyBase.New(outs)
			Me.data = New DialoguerTextData(text, themeName, newWindow, name, portrait, metadata, audio, audioDelay, rect, choices, dialogueID, nodeID)
		End Sub

		' Token: 0x060046C5 RID: 18117 RVA: 0x0024EF15 File Offset: 0x0024D315
		Protected Overrides Sub onStart()
		End Sub

		' Token: 0x060046C6 RID: 18118 RVA: 0x0024EF18 File Offset: 0x0024D318
		Public Overrides Sub [Continue](nextPhaseId As Integer)
			If Me.data.newWindow Then
				DialoguerEventManager.dispatchOnWindowClose()
			End If
			MyBase.[Continue](nextPhaseId)
			MyBase.state = PhaseState.Complete
		End Sub

		' Token: 0x060046C7 RID: 18119 RVA: 0x0024EF4C File Offset: 0x0024D34C
		Public Overrides Function ToString() As String
			Return String.Concat(New Object() { "Text Phase", Me.data.ToString(), vbLf & "Out: ", Me.outs(0), vbLf })
		End Function

		' Token: 0x04004CA8 RID: 19624
		Public data As DialoguerTextData
	End Class
End Namespace
