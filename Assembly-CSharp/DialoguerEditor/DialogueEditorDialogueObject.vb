Imports System
Imports System.Collections.Generic
Imports UnityEngine

Namespace DialoguerEditor
	' Token: 0x02000B41 RID: 2881
	<Serializable()>
	Public Class DialogueEditorDialogueObject
		' Token: 0x060045BD RID: 17853 RVA: 0x0024C694 File Offset: 0x0024AA94
		Public Sub New()
			Me.name = String.Empty
			Me.phases = New List(Of DialogueEditorPhaseObject)()
			Me.floats = New DialogueEditorVariablesContainer()
			Me.strings = New DialogueEditorVariablesContainer()
			Me.booleans = New DialogueEditorVariablesContainer()
		End Sub

		' Token: 0x060045BE RID: 17854 RVA: 0x0024C6E8 File Offset: 0x0024AAE8
		Public Sub addPhase(phaseType As DialogueEditorPhaseTypes, newPhasePosition As Vector2)
			Select Case phaseType
				Case DialogueEditorPhaseTypes.TextPhase
					Me.phases.Add(DialogueEditorPhaseTemplates.newTextPhase(Me.phases.Count))
				Case DialogueEditorPhaseTypes.BranchedTextPhase
					Me.phases.Add(DialogueEditorPhaseTemplates.newBranchedTextPhase(Me.phases.Count))
				Case DialogueEditorPhaseTypes.WaitPhase
					Me.phases.Add(DialogueEditorPhaseTemplates.newWaitPhase(Me.phases.Count))
				Case DialogueEditorPhaseTypes.SetVariablePhase
					Me.phases.Add(DialogueEditorPhaseTemplates.newSetVariablePhase(Me.phases.Count))
				Case DialogueEditorPhaseTypes.ConditionalPhase
					Me.phases.Add(DialogueEditorPhaseTemplates.newConditionalPhase(Me.phases.Count))
				Case DialogueEditorPhaseTypes.SendMessagePhase
					Me.phases.Add(DialogueEditorPhaseTemplates.newSendMessagePhase(Me.phases.Count))
				Case DialogueEditorPhaseTypes.EndPhase
					Me.phases.Add(DialogueEditorPhaseTemplates.newEndPhase(Me.phases.Count))
			End Select
			Me.phases(Me.phases.Count - 1).position = newPhasePosition
		End Sub

		' Token: 0x060045BF RID: 17855 RVA: 0x0024C81C File Offset: 0x0024AC1C
		Public Sub removePhase(phaseId As Integer)
			For i As Integer = 0 To Me.phases.Count - 1
				Dim dialogueEditorPhaseObject As DialogueEditorPhaseObject = Me.phases(i)
				For j As Integer = 0 To dialogueEditorPhaseObject.outs.Count - 1
					If dialogueEditorPhaseObject.outs(j) >= 0 AndAlso dialogueEditorPhaseObject.outs(j) > phaseId Then
						Dim outs As List(Of Integer) = dialogueEditorPhaseObject.outs
						Dim list As List(Of Integer) = outs
						Dim num As Integer = j
						Dim num2 As Integer = num
						outs(num) = list(num2) - 1
					ElseIf dialogueEditorPhaseObject.outs(j) >= 0 AndAlso dialogueEditorPhaseObject.outs(j) = phaseId Then
						dialogueEditorPhaseObject.outs(j) = -1
					End If
				Next
				If Me.startPage >= 0 AndAlso Me.startPage = phaseId Then
					Me.startPage = -1
				End If
				If i > phaseId Then
					dialogueEditorPhaseObject.id -= 1
				End If
			Next
			Me.phases.RemoveAt(phaseId)
		End Sub

		' Token: 0x04004BE4 RID: 19428
		Public id As Integer

		' Token: 0x04004BE5 RID: 19429
		Public name As String

		' Token: 0x04004BE6 RID: 19430
		Public startPage As Integer = -1

		' Token: 0x04004BE7 RID: 19431
		Public scrollPosition As Vector2

		' Token: 0x04004BE8 RID: 19432
		Public phases As List(Of DialogueEditorPhaseObject)

		' Token: 0x04004BE9 RID: 19433
		Public floats As DialogueEditorVariablesContainer

		' Token: 0x04004BEA RID: 19434
		Public strings As DialogueEditorVariablesContainer

		' Token: 0x04004BEB RID: 19435
		Public booleans As DialogueEditorVariablesContainer
	End Class
End Namespace
