Imports System
Imports System.Collections.Generic
Imports DialoguerEditor

Namespace DialoguerCore
	' Token: 0x02000B81 RID: 2945
	Public Class DialoguerUtils
		' Token: 0x060046D3 RID: 18131 RVA: 0x0024FC78 File Offset: 0x0024E078
		Public Shared Function insertTextPhaseStringVariables(input As String) As String
			Dim num As Integer = 0
			Dim text As String = DialoguerUtils.substituteStringVariable(input, VariableEditorScopes.[Global], VariableEditorTypes.[Boolean], num)
			text = DialoguerUtils.substituteStringVariable(text, VariableEditorScopes.[Global], VariableEditorTypes.Float, num)
			Return DialoguerUtils.substituteStringVariable(text, VariableEditorScopes.[Global], VariableEditorTypes.[String], num)
		End Function

		' Token: 0x060046D4 RID: 18132 RVA: 0x0024FCA8 File Offset: 0x0024E0A8
		Private Shared Function substituteStringVariable(input As String, scope As VariableEditorScopes, type As VariableEditorTypes, dialogueId As Integer) As String
			If input Is Nothing Then
				Return input
			End If
			Dim text As String = String.Empty
			Dim array As String() = New String() { "<" + DialoguerUtils.scopeStrings(scope) + DialoguerUtils.typeStrings(type) + ">" }
			Dim array2 As String() = New String() { "</" + DialoguerUtils.scopeStrings(scope) + DialoguerUtils.typeStrings(type) + ">" }
			Dim array3 As String() = input.Split(array, StringSplitOptions.None)
			If array3.Length < 2 Then
				Return input
			End If
			For i As Integer = 0 To array3.Length - 1
				Dim array4 As String() = array3(i).Split(array2, StringSplitOptions.None)
				Dim num As Integer
				Dim flag As Boolean = Integer.TryParse(array4(0), num)
				If flag Then
					If scope <> VariableEditorScopes.[Global] Then
						If scope = VariableEditorScopes.Local Then
							If type <> VariableEditorTypes.[Boolean] Then
								If type <> VariableEditorTypes.Float Then
									If type <> VariableEditorTypes.[String] Then
									End If
								End If
							End If
						End If
					ElseIf type <> VariableEditorTypes.[Boolean] Then
						If type <> VariableEditorTypes.Float Then
							If type = VariableEditorTypes.[String] Then
								array4(0) = Dialoguer.GetGlobalString(num)
							End If
						Else
							array4(0) = Dialoguer.GetGlobalFloat(num).ToString()
						End If
					Else
						array4(0) = Dialoguer.GetGlobalBoolean(num).ToString()
					End If
				End If
				text += String.Join(String.Empty, array4)
			Next
			Return text
		End Function

		' Token: 0x04004CB8 RID: 19640
		Private Shared scopeStrings As Dictionary(Of VariableEditorScopes, String) = New Dictionary(Of VariableEditorScopes, String)() From { { VariableEditorScopes.[Global], "g" }, { VariableEditorScopes.Local, "l" } }

		' Token: 0x04004CB9 RID: 19641
		Private Shared typeStrings As Dictionary(Of VariableEditorTypes, String) = New Dictionary(Of VariableEditorTypes, String)() From { { VariableEditorTypes.[Boolean], "b" }, { VariableEditorTypes.Float, "f" }, { VariableEditorTypes.[String], "s" } }
	End Class
End Namespace
