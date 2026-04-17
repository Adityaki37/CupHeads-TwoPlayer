Imports System

Namespace DialoguerCore
	' Token: 0x02000B61 RID: 2913
	Public Class DialoguerDialogueManager
		' Token: 0x06004644 RID: 17988 RVA: 0x0024DF28 File Offset: 0x0024C328
		Public Shared Sub startDialogueWithCallback(dialogueId As Integer, callback As DialoguerCallback)
			DialoguerDialogueManager.onEndCallback = callback
			DialoguerDialogueManager.startDialogue(dialogueId)
		End Sub

		' Token: 0x06004645 RID: 17989 RVA: 0x0024DF36 File Offset: 0x0024C336
		Public Shared Sub startDialogue(dialogueId As Integer)
			If DialoguerDialogueManager.dialogue IsNot Nothing Then
				DialoguerEventManager.dispatchOnSuddenlyEnded()
			End If
			DialoguerEventManager.dispatchOnStarted()
			DialoguerDialogueManager.dialogue = DialoguerDataManager.GetDialogueById(dialogueId)
			DialoguerDialogueManager.dialogue.Reset()
			DialoguerDialogueManager.setupPhase(DialoguerDialogueManager.dialogue.startPhaseId)
		End Sub

		' Token: 0x06004646 RID: 17990 RVA: 0x0024DF70 File Offset: 0x0024C370
		Public Shared Sub continueDialogue(outId As Integer)
			If DialoguerDialogueManager.currentPhase IsNot Nothing Then
				DialoguerDialogueManager.currentPhase.[Continue](outId)
			End If
		End Sub

		' Token: 0x06004647 RID: 17991 RVA: 0x0024DF87 File Offset: 0x0024C387
		Public Shared Sub endDialogue()
			If DialoguerDialogueManager.dialogue Is Nothing Then
				Return
			End If
			If DialoguerDialogueManager.onEndCallback IsNot Nothing Then
				DialoguerDialogueManager.onEndCallback()
			End If
			DialoguerEventManager.dispatchOnWindowClose()
			DialoguerEventManager.dispatchOnEnded()
			DialoguerDialogueManager.dialogue.Reset()
			DialoguerDialogueManager.reset()
		End Sub

		' Token: 0x06004648 RID: 17992 RVA: 0x0024DFC4 File Offset: 0x0024C3C4
		Private Shared Sub setupPhase(nextPhaseId As Integer)
			If DialoguerDialogueManager.dialogue Is Nothing Then
				Return
			End If
			Dim abstractDialoguePhase As AbstractDialoguePhase = DialoguerDialogueManager.dialogue.phases(nextPhaseId)
			If TypeOf abstractDialoguePhase Is EndPhase Then
				DialoguerDialogueManager.endDialogue()
				Return
			End If
			If DialoguerDialogueManager.currentPhase IsNot Nothing Then
				DialoguerDialogueManager.currentPhase.resetEvents()
			End If
			AddHandler abstractDialoguePhase.onPhaseComplete, AddressOf DialoguerDialogueManager.phaseComplete
			If TypeOf abstractDialoguePhase Is TextPhase OrElse TypeOf abstractDialoguePhase Is BranchedTextPhase Then
				DialoguerEventManager.dispatchOnTextPhase(TryCast(abstractDialoguePhase, TextPhase).data)
			End If
			DialoguerDialogueManager.currentPhase = abstractDialoguePhase
			abstractDialoguePhase.Start(DialoguerDialogueManager.dialogue.localVariables)
		End Sub

		' Token: 0x06004649 RID: 17993 RVA: 0x0024E071 File Offset: 0x0024C471
		Private Shared Sub phaseComplete(nextPhaseId As Integer)
			DialoguerDialogueManager.setupPhase(nextPhaseId)
		End Sub

		' Token: 0x0600464A RID: 17994 RVA: 0x0024E079 File Offset: 0x0024C479
		Private Shared Function isWindowed(phase As AbstractDialoguePhase) As Boolean
			Return TypeOf phase Is TextPhase OrElse TypeOf phase Is BranchedTextPhase
		End Function

		' Token: 0x0600464B RID: 17995 RVA: 0x0024E094 File Offset: 0x0024C494
		Private Shared Sub reset()
			DialoguerDialogueManager.currentPhase = Nothing
			DialoguerDialogueManager.dialogue = Nothing
			DialoguerDialogueManager.onEndCallback = Nothing
		End Sub

		' Token: 0x04004C5E RID: 19550
		Private Shared currentPhase As AbstractDialoguePhase

		' Token: 0x04004C5F RID: 19551
		Private Shared dialogue As DialoguerDialogue

		' Token: 0x04004C60 RID: 19552
		Private Shared onEndCallback As DialoguerCallback
	End Class
End Namespace
