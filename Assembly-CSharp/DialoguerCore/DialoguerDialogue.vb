Imports System
Imports System.Collections.Generic

Namespace DialoguerCore
	' Token: 0x02000B6E RID: 2926
	Public Class DialoguerDialogue
		' Token: 0x06004696 RID: 18070 RVA: 0x0024E82C File Offset: 0x0024CC2C
		Public Sub New(name As String, startPhaseId As Integer, localVariables As DialoguerVariables, phases As List(Of AbstractDialoguePhase))
			Me.name = name
			Me.startPhaseId = startPhaseId
			Me.phases = phases
			Me._originalLocalVariables = localVariables
		End Sub

		' Token: 0x06004697 RID: 18071 RVA: 0x0024E851 File Offset: 0x0024CC51
		Public Sub Reset()
			Me.localVariables = Me._originalLocalVariables.Clone()
		End Sub

		' Token: 0x06004698 RID: 18072 RVA: 0x0024E864 File Offset: 0x0024CC64
		Public Overrides Function ToString() As String
			Dim text As String = "Dialogue: " + Me.name + vbLf & "-"
			text = text + vbLf & "Local Booleans: " + Me._originalLocalVariables.booleans.Count
			text = text + vbLf & "Local Floats: " + Me._originalLocalVariables.floats.Count
			text = text + vbLf & "Local Strings: " + Me._originalLocalVariables.strings.Count
			text += vbLf
			For i As Integer = 0 To Me.phases.Count - 1
				Dim text2 As String = text
				text = String.Concat(New Object() { text2, vbLf & "Phase ", i, ": ", Me.phases(i).ToString() })
			Next
			Return text
		End Function

		' Token: 0x04004C73 RID: 19571
		Public name As String

		' Token: 0x04004C74 RID: 19572
		Public startPhaseId As Integer

		' Token: 0x04004C75 RID: 19573
		Public phases As List(Of AbstractDialoguePhase)

		' Token: 0x04004C76 RID: 19574
		Private _originalLocalVariables As DialoguerVariables

		' Token: 0x04004C77 RID: 19575
		Public localVariables As DialoguerVariables
	End Class
End Namespace
