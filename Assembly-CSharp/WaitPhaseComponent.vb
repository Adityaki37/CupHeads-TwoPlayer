Imports System
Imports DialoguerCore
Imports DialoguerEditor
Imports UnityEngine

' Token: 0x02000B6C RID: 2924
Public Class WaitPhaseComponent
	Inherits MonoBehaviour

	' Token: 0x06004691 RID: 18065 RVA: 0x0024E5ED File Offset: 0x0024C9ED
	Public Sub Init(phase As WaitPhase, type As DialogueEditorWaitTypes, duration As Single)
		Me.phase = phase
		Me.type = type
		Me.duration = duration
		Me.elapsed = 0F
		Me.go = True
	End Sub

	' Token: 0x06004692 RID: 18066 RVA: 0x0024E618 File Offset: 0x0024CA18
	Private Sub Update()
		If Not Me.go Then
			Return
		End If
		Dim deltaTime As Single = Time.deltaTime
		Dim dialogueEditorWaitTypes As DialogueEditorWaitTypes = Me.type
		If dialogueEditorWaitTypes <> DialogueEditorWaitTypes.Seconds Then
			If dialogueEditorWaitTypes = DialogueEditorWaitTypes.Frames Then
				Me.elapsed += 1F
				If Me.elapsed >= Me.duration Then
					Me.waitComplete()
				End If
			End If
		Else
			Me.elapsed += deltaTime
			If Me.elapsed >= Me.duration Then
				Me.waitComplete()
			End If
		End If
	End Sub

	' Token: 0x06004693 RID: 18067 RVA: 0x0024E6A8 File Offset: 0x0024CAA8
	Private Sub waitComplete()
		Me.go = False
		Me.phase.waitComplete()
		Me.phase = Nothing
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x04004C6B RID: 19563
	Public type As DialogueEditorWaitTypes

	' Token: 0x04004C6C RID: 19564
	Public phase As WaitPhase

	' Token: 0x04004C6D RID: 19565
	Public go As Boolean

	' Token: 0x04004C6E RID: 19566
	Public duration As Single

	' Token: 0x04004C6F RID: 19567
	Public elapsed As Single
End Class
