Imports System
Imports System.Collections.Generic

Namespace DialoguerEditor
	' Token: 0x02000B4E RID: 2894
	<Serializable()>
	Public Class DialogueEditorVariablesContainer
		' Token: 0x060045EA RID: 17898 RVA: 0x0024D7E4 File Offset: 0x0024BBE4
		Public Sub New()
			Me.selection = 0
			Me.variables = New List(Of DialogueEditorVariableObject)()
		End Sub

		' Token: 0x060045EB RID: 17899 RVA: 0x0024D800 File Offset: 0x0024BC00
		Public Sub addVariable()
			Dim count As Integer = Me.variables.Count
			Me.variables.Add(New DialogueEditorVariableObject())
			Me.variables(count).id = count
			Me.selection = Me.variables.Count - 1
		End Sub

		' Token: 0x060045EC RID: 17900 RVA: 0x0024D850 File Offset: 0x0024BC50
		Public Sub removeVariable()
			If Me.variables.Count < 1 Then
				Return
			End If
			Me.variables.RemoveAt(Me.variables.Count - 1)
			If Me.selection > Me.variables.Count - 1 Then
				Me.selection = Me.variables.Count - 1
			End If
		End Sub

		' Token: 0x04004C2C RID: 19500
		Public variables As List(Of DialogueEditorVariableObject)

		' Token: 0x04004C2D RID: 19501
		Public selection As Integer
	End Class
End Namespace
