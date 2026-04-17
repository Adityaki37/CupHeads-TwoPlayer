Imports System

Namespace DialoguerEditor
	' Token: 0x02000B43 RID: 2883
	<Serializable()>
	Public Class DialogueEditorGlobalVariablesContainer
		' Token: 0x060045C1 RID: 17857 RVA: 0x0024C939 File Offset: 0x0024AD39
		Public Sub New()
			Me.booleans = New DialogueEditorVariablesContainer()
			Me.floats = New DialogueEditorVariablesContainer()
			Me.strings = New DialogueEditorVariablesContainer()
		End Sub

		' Token: 0x04004BEE RID: 19438
		Public booleans As DialogueEditorVariablesContainer

		' Token: 0x04004BEF RID: 19439
		Public floats As DialogueEditorVariablesContainer

		' Token: 0x04004BF0 RID: 19440
		Public strings As DialogueEditorVariablesContainer
	End Class
End Namespace
