Imports System
Imports System.Collections.Generic
Imports DialoguerEditor
Imports UnityEngine

Namespace DialoguerCore
	' Token: 0x02000B7C RID: 2940
	Public Class WaitPhase
		Inherits AbstractDialoguePhase

		' Token: 0x060046C8 RID: 18120 RVA: 0x0024F9FD File Offset: 0x0024DDFD
		Public Sub New(type As DialogueEditorWaitTypes, duration As Single, outs As List(Of Integer))
			MyBase.New(outs)
			Me.type = type
			Me.duration = duration
		End Sub

		' Token: 0x060046C9 RID: 18121 RVA: 0x0024FA14 File Offset: 0x0024DE14
		Protected Overrides Sub onStart()
			DialoguerEventManager.dispatchOnWaitStart()
			If Me.type = DialogueEditorWaitTypes.[Continue] Then
				Return
			End If
			Dim gameObject As GameObject = New GameObject("Dialoguer WaitPhaseTimer")
			Dim waitPhaseComponent As WaitPhaseComponent = gameObject.AddComponent(Of WaitPhaseComponent)()
			waitPhaseComponent.Init(Me, Me.type, Me.duration)
		End Sub

		' Token: 0x060046CA RID: 18122 RVA: 0x0024FA58 File Offset: 0x0024DE58
		Public Sub waitComplete()
			DialoguerEventManager.dispatchOnWaitComplete()
			MyBase.state = PhaseState.Complete
		End Sub

		' Token: 0x060046CB RID: 18123 RVA: 0x0024FA66 File Offset: 0x0024DE66
		Public Overrides Sub [Continue](outId As Integer)
			If Me.type <> DialogueEditorWaitTypes.[Continue] Then
				Return
			End If
			DialoguerEventManager.dispatchOnWaitComplete()
			MyBase.[Continue](outId)
		End Sub

		' Token: 0x060046CC RID: 18124 RVA: 0x0024FA84 File Offset: 0x0024DE84
		Public Overrides Function ToString() As String
			Return String.Concat(New Object() { "Wait Phase" & vbLf & "Type: ", Me.type.ToString(), vbLf & "Duration: ", Me.duration, vbLf })
		End Function

		' Token: 0x04004CA9 RID: 19625
		Public type As DialogueEditorWaitTypes

		' Token: 0x04004CAA RID: 19626
		Public duration As Single
	End Class
End Namespace
