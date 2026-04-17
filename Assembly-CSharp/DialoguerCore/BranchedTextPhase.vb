Imports System
Imports System.Collections.Generic
Imports UnityEngine

Namespace DialoguerCore
	' Token: 0x02000B75 RID: 2933
	Public Class BranchedTextPhase
		Inherits TextPhase

		' Token: 0x060046B5 RID: 18101 RVA: 0x0024EFA4 File Offset: 0x0024D3A4
		Public Sub New(text As String, choices As List(Of String), themeName As String, newWindow As Boolean, name As String, portrait As String, metadata As String, audio As String, audioDelay As Single, rect As Rect, outs As List(Of Integer), dialogueID As Integer, nodeID As Integer)
			MyBase.New(text, themeName, newWindow, name, portrait, metadata, audio, audioDelay, rect, outs, choices, dialogueID, nodeID)
			Me.choices = choices
		End Sub

		' Token: 0x060046B6 RID: 18102 RVA: 0x0024EFD8 File Offset: 0x0024D3D8
		Public Overrides Function ToString() As String
			Dim text As String = String.Empty
			For i As Integer = 0 To Me.choices.Count - 1
				Dim text2 As String = text
				text = String.Concat(New Object() { text2, i, ": ", Me.choices(i), " : Out ", Me.outs(i), vbLf })
			Next
			Return "Branched Text Phase" + Me.data.ToString() + vbLf + text
		End Function

		' Token: 0x04004C92 RID: 19602
		Public choices As List(Of String)
	End Class
End Namespace
