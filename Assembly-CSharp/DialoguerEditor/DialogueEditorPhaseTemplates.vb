Imports System
Imports System.Collections.Generic
Imports UnityEngine

Namespace DialoguerEditor
	' Token: 0x02000B49 RID: 2889
	Public Class DialogueEditorPhaseTemplates
		' Token: 0x060045D6 RID: 17878 RVA: 0x0024D320 File Offset: 0x0024B720
		Public Shared Function newTextPhase(id As Integer) As DialogueEditorPhaseObject
			Return New DialogueEditorPhaseObject() With { .id = id, .type = DialogueEditorPhaseTypes.TextPhase, .position = Vector2.zero, .advanced = False, .metadata = String.Empty, .name = String.Empty, .portrait = String.Empty, .audio = String.Empty, .audioDelay = 0F, .rect = New Rect(0F, 0F, 0F, 0F), .newWindow = False, .outs = New List(Of Integer)(), .outs = { -1 } }
		End Function

		' Token: 0x060045D7 RID: 17879 RVA: 0x0024D3C8 File Offset: 0x0024B7C8
		Public Shared Function newBranchedTextPhase(id As Integer) As DialogueEditorPhaseObject
			Return New DialogueEditorPhaseObject() With { .id = id, .type = DialogueEditorPhaseTypes.BranchedTextPhase, .position = Vector2.zero, .advanced = False, .metadata = String.Empty, .name = String.Empty, .portrait = String.Empty, .audio = String.Empty, .audioDelay = 0F, .rect = New Rect(0F, 0F, 0F, 0F), .newWindow = False, .outs = New List(Of Integer)(), .outs = { -1, -1 }, .choices = New List(Of String)(), .choices = { String.Empty, String.Empty } }
		End Function

		' Token: 0x060045D8 RID: 17880 RVA: 0x0024D4A8 File Offset: 0x0024B8A8
		Public Shared Function newWaitPhase(id As Integer) As DialogueEditorPhaseObject
			Return New DialogueEditorPhaseObject() With { .id = id, .type = DialogueEditorPhaseTypes.WaitPhase, .position = Vector2.zero, .advanced = False, .metadata = String.Empty, .outs = New List(Of Integer)(), .outs = { -1 } }
		End Function

		' Token: 0x060045D9 RID: 17881 RVA: 0x0024D500 File Offset: 0x0024B900
		Public Shared Function newSetVariablePhase(id As Integer) As DialogueEditorPhaseObject
			Return New DialogueEditorPhaseObject() With { .id = id, .type = DialogueEditorPhaseTypes.SetVariablePhase, .position = Vector2.zero, .advanced = False, .metadata = String.Empty, .outs = New List(Of Integer)(), .outs = { -1 }, .variableScope = VariableEditorScopes.Local, .variableType = VariableEditorTypes.[Boolean], .variableSetEquation = VariableEditorSetEquation.Equals, .variableScrollPosition = Nothing, .variableId = 0, .variableSetValue = String.Empty }
		End Function

		' Token: 0x060045DA RID: 17882 RVA: 0x0024D58C File Offset: 0x0024B98C
		Public Shared Function newConditionalPhase(id As Integer) As DialogueEditorPhaseObject
			Return New DialogueEditorPhaseObject() With { .id = id, .type = DialogueEditorPhaseTypes.ConditionalPhase, .position = Vector2.zero, .advanced = False, .metadata = String.Empty, .outs = New List(Of Integer)(), .outs = { -1, -1 } }
		End Function

		' Token: 0x060045DB RID: 17883 RVA: 0x0024D5F0 File Offset: 0x0024B9F0
		Public Shared Function newSendMessagePhase(id As Integer) As DialogueEditorPhaseObject
			Return New DialogueEditorPhaseObject() With { .id = id, .type = DialogueEditorPhaseTypes.SendMessagePhase, .position = Vector2.zero, .advanced = False, .metadata = String.Empty, .outs = New List(Of Integer)(), .outs = { -1 }, .messageName = String.Empty }
		End Function

		' Token: 0x060045DC RID: 17884 RVA: 0x0024D654 File Offset: 0x0024BA54
		Public Shared Function newEndPhase(id As Integer) As DialogueEditorPhaseObject
			Return New DialogueEditorPhaseObject() With { .id = id, .type = DialogueEditorPhaseTypes.EndPhase, .position = Vector2.zero }
		End Function
	End Class
End Namespace
