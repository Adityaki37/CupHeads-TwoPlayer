Imports System
Imports System.Collections.Generic
Imports DialoguerEditor

Namespace DialoguerCore
	' Token: 0x02000B7A RID: 2938
	Public Class SetVariablePhase
		Inherits AbstractDialoguePhase

		' Token: 0x060046C1 RID: 18113 RVA: 0x0024F51F File Offset: 0x0024D91F
		Public Sub New(scope As VariableEditorScopes, type As VariableEditorTypes, variableId As Integer, equation As VariableEditorSetEquation, setValue As String, outs As List(Of Integer))
			MyBase.New(outs)
			Me.scope = scope
			Me.type = type
			Me.variableId = variableId
			Me.equation = equation
			Me.setValue = setValue
		End Sub

		' Token: 0x060046C2 RID: 18114 RVA: 0x0024F550 File Offset: 0x0024D950
		Protected Overrides Sub onStart()
			Dim flag As Boolean = False
			Dim variableEditorTypes As VariableEditorTypes = Me.type
			If variableEditorTypes <> VariableEditorTypes.[Boolean] Then
				If variableEditorTypes <> VariableEditorTypes.Float Then
					If variableEditorTypes = VariableEditorTypes.[String] Then
						flag = True
						Me._setString = Me.setValue
						Dim variableEditorSetEquation As VariableEditorSetEquation = Me.equation
						If variableEditorSetEquation <> VariableEditorSetEquation.Equals Then
							If variableEditorSetEquation = VariableEditorSetEquation.Add Then
								If Me.scope = VariableEditorScopes.Local Then
									Dim strings As List(Of String) = Me._localVariables.strings
									Dim list As List(Of String) = strings
									Dim num As Integer = Me.variableId
									Dim num2 As Integer = num
									strings(num) = list(num2) + Me._setString
								Else
									Dialoguer.SetGlobalString(Me.variableId, Dialoguer.GetGlobalString(Me.variableId) + Me._setString)
								End If
							End If
						ElseIf Me.scope = VariableEditorScopes.Local Then
							Me._localVariables.strings(Me.variableId) = Me._setString
						Else
							Dialoguer.SetGlobalString(Me.variableId, Me._setString)
						End If
					End If
				Else
					flag = Parser.FloatTryParse(Me.setValue, Me._setFloat)
					Select Case Me.equation
						Case VariableEditorSetEquation.Equals
							If Me.scope = VariableEditorScopes.Local Then
								Me._localVariables.floats(Me.variableId) = Me._setFloat
							Else
								Dialoguer.SetGlobalFloat(Me.variableId, Me._setFloat)
							End If
						Case VariableEditorSetEquation.Add
							If Me.scope = VariableEditorScopes.Local Then
								Dim floats As List(Of Single) = Me._localVariables.floats
								Dim list2 As List(Of Single) = floats
								Dim num3 As Integer = Me.variableId
								Dim num4 As Integer = num3
								floats(num3) = list2(num4) + Me._setFloat
							Else
								Dialoguer.SetGlobalFloat(Me.variableId, Dialoguer.GetGlobalFloat(Me.variableId) + Me._setFloat)
							End If
						Case VariableEditorSetEquation.Subtract
							If Me.scope = VariableEditorScopes.Local Then
								Dim floats2 As List(Of Single) = Me._localVariables.floats
								Dim list2 As List(Of Single) = floats2
								Dim num5 As Integer = Me.variableId
								Dim num6 As Integer = num5
								floats2(num5) = list2(num6) - Me._setFloat
							Else
								Dialoguer.SetGlobalFloat(Me.variableId, Dialoguer.GetGlobalFloat(Me.variableId) - Me._setFloat)
							End If
						Case VariableEditorSetEquation.Multiply
							If Me.scope = VariableEditorScopes.Local Then
								Dim floats3 As List(Of Single) = Me._localVariables.floats
								Dim list2 As List(Of Single) = floats3
								Dim num7 As Integer = Me.variableId
								Dim num8 As Integer = num7
								floats3(num7) = list2(num8) * Me._setFloat
							Else
								Dialoguer.SetGlobalFloat(Me.variableId, Dialoguer.GetGlobalFloat(Me.variableId) * Me._setFloat)
							End If
						Case VariableEditorSetEquation.Divide
							If Me.scope = VariableEditorScopes.Local Then
								Dim floats4 As List(Of Single) = Me._localVariables.floats
								Dim list2 As List(Of Single) = floats4
								Dim num9 As Integer = Me.variableId
								Dim num10 As Integer = num9
								floats4(num9) = list2(num10) / Me._setFloat
							Else
								Dialoguer.SetGlobalFloat(Me.variableId, Dialoguer.GetGlobalFloat(Me.variableId) / Me._setFloat)
							End If
					End Select
				End If
			Else
				flag = Boolean.TryParse(Me.setValue, Me._setBool)
				Dim variableEditorSetEquation2 As VariableEditorSetEquation = Me.equation
				If variableEditorSetEquation2 <> VariableEditorSetEquation.Equals Then
					If variableEditorSetEquation2 = VariableEditorSetEquation.Toggle Then
						If Me.scope = VariableEditorScopes.Local Then
							Me._localVariables.booleans(Me.variableId) = Not Me._localVariables.booleans(Me.variableId)
						Else
							Dialoguer.SetGlobalBoolean(Me.variableId, Not Dialoguer.GetGlobalBoolean(Me.variableId))
						End If
						flag = True
					End If
				ElseIf Me.scope = VariableEditorScopes.Local Then
					Me._localVariables.booleans(Me.variableId) = Me._setBool
				Else
					Dialoguer.SetGlobalBoolean(Me.variableId, Me._setBool)
				End If
			End If
			If Not flag Then
			End If
			Me.[Continue](0)
			MyBase.state = PhaseState.Complete
		End Sub

		' Token: 0x060046C3 RID: 18115 RVA: 0x0024F93C File Offset: 0x0024DD3C
		Public Overrides Function ToString() As String
			Return String.Concat(New Object() { "Set Variable Phase" & vbLf & "Scope: ", Me.scope.ToString(), vbLf & "Type: ", Me.type.ToString(), vbLf & "Variable ID: ", Me.variableId, vbLf & "Equation: ", Me.equation.ToString(), vbLf & "Set Value: ", Me.setValue, vbLf & "Out: ", Me.outs(0), vbLf })
		End Function

		' Token: 0x04004CA0 RID: 19616
		Public scope As VariableEditorScopes

		' Token: 0x04004CA1 RID: 19617
		Public type As VariableEditorTypes

		' Token: 0x04004CA2 RID: 19618
		Public variableId As Integer

		' Token: 0x04004CA3 RID: 19619
		Public equation As VariableEditorSetEquation

		' Token: 0x04004CA4 RID: 19620
		Public setValue As String

		' Token: 0x04004CA5 RID: 19621
		Private _setBool As Boolean

		' Token: 0x04004CA6 RID: 19622
		Private _setFloat As Single

		' Token: 0x04004CA7 RID: 19623
		Private _setString As String
	End Class
End Namespace
