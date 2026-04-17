Imports System
Imports System.Collections.Generic
Imports DialoguerEditor

Namespace DialoguerCore
	' Token: 0x02000B76 RID: 2934
	Public Class ConditionalPhase
		Inherits AbstractDialoguePhase

		' Token: 0x060046B7 RID: 18103 RVA: 0x0024F07D File Offset: 0x0024D47D
		Public Sub New(scope As VariableEditorScopes, type As VariableEditorTypes, variableId As Integer, equation As VariableEditorGetEquation, getValue As String, outs As List(Of Integer))
			MyBase.New(outs)
			Me.scope = scope
			Me.type = type
			Me.variableId = variableId
			Me.equation = equation
			Me.getValue = getValue
		End Sub

		' Token: 0x060046B8 RID: 18104 RVA: 0x0024F0AC File Offset: 0x0024D4AC
		Protected Overrides Sub onStart()
			Dim variableEditorTypes As VariableEditorTypes = Me.type
			If variableEditorTypes <> VariableEditorTypes.[Boolean] Then
				If variableEditorTypes <> VariableEditorTypes.Float Then
					If variableEditorTypes = VariableEditorTypes.[String] Then
						Me._parsedString = Me.getValue
						If Me.scope = VariableEditorScopes.Local Then
							Me._checkString = Me._localVariables.strings(Me.variableId)
						Else
							Me._checkString = Dialoguer.GetGlobalString(Me.variableId)
						End If
					End If
				Else
					If Not Parser.FloatTryParse(Me.getValue, Me._parsedFloat) Then
						Debug.LogError("[ConditionalPhase] Could Not Parse Float: " + Me.getValue, Nothing)
					End If
					If Me.scope = VariableEditorScopes.Local Then
						Me._checkFloat = Me._localVariables.floats(Me.variableId)
					Else
						Me._checkFloat = Dialoguer.GetGlobalFloat(Me.variableId)
					End If
				End If
			Else
				If Not Boolean.TryParse(Me.getValue, Me._parsedBool) Then
					Debug.LogError("[ConditionalPhase] Could Not Parse Bool: " + Me.getValue, Nothing)
				End If
				If Me.scope = VariableEditorScopes.Local Then
					Me._checkBool = Me._localVariables.booleans(Me.variableId)
				Else
					Me._checkBool = Dialoguer.GetGlobalBoolean(Me.variableId)
				End If
			End If
			Dim flag As Boolean = False
			Dim variableEditorTypes2 As VariableEditorTypes = Me.type
			If variableEditorTypes2 <> VariableEditorTypes.[Boolean] Then
				If variableEditorTypes2 <> VariableEditorTypes.Float Then
					If variableEditorTypes2 = VariableEditorTypes.[String] Then
						Dim variableEditorGetEquation As VariableEditorGetEquation = Me.equation
						If variableEditorGetEquation <> VariableEditorGetEquation.Equals Then
							If variableEditorGetEquation = VariableEditorGetEquation.NotEquals Then
								If Me._parsedString <> Me._checkString Then
									flag = True
								End If
							End If
						ElseIf Me._parsedString = Me._checkString Then
							flag = True
						End If
					End If
				Else
					Select Case Me.equation
						Case VariableEditorGetEquation.Equals
							If Me._checkFloat = Me._parsedFloat Then
								flag = True
							End If
						Case VariableEditorGetEquation.NotEquals
							If Me._checkFloat <> Me._parsedFloat Then
								flag = True
							End If
						Case VariableEditorGetEquation.GreaterThan
							If Me._checkFloat > Me._parsedFloat Then
								flag = True
							End If
						Case VariableEditorGetEquation.LessThan
							If Me._checkFloat < Me._parsedFloat Then
								flag = True
							End If
						Case VariableEditorGetEquation.EqualOrGreaterThan
							If Me._checkFloat >= Me._parsedFloat Then
								flag = True
							End If
						Case VariableEditorGetEquation.EqualOrLessThan
							If Me._checkFloat <= Me._parsedFloat Then
								flag = True
							End If
					End Select
				End If
			Else
				Dim variableEditorGetEquation2 As VariableEditorGetEquation = Me.equation
				If variableEditorGetEquation2 <> VariableEditorGetEquation.Equals Then
					If variableEditorGetEquation2 = VariableEditorGetEquation.NotEquals Then
						If Me._parsedBool <> Me._checkBool Then
							flag = True
						End If
					End If
				ElseIf Me._parsedBool = Me._checkBool Then
					flag = True
				End If
			End If
			If flag Then
				Me.[Continue](0)
			Else
				Me.[Continue](1)
			End If
			MyBase.state = PhaseState.Complete
		End Sub

		' Token: 0x060046B9 RID: 18105 RVA: 0x0024F3BC File Offset: 0x0024D7BC
		Public Overrides Function ToString() As String
			Return String.Concat(New Object() { "Set Variable Phase" & vbLf & "Scope: ", Me.scope.ToString(), vbLf & "Type: ", Me.type.ToString(), vbLf & "Variable ID: ", Me.variableId, vbLf & "Equation: ", Me.equation.ToString(), vbLf & "Get Value: ", Me.getValue, vbLf & "True Out: ", Me.outs(0), vbLf & "False Out: ", Me.outs(1), vbLf })
		End Function

		' Token: 0x04004C93 RID: 19603
		Public scope As VariableEditorScopes

		' Token: 0x04004C94 RID: 19604
		Public type As VariableEditorTypes

		' Token: 0x04004C95 RID: 19605
		Public variableId As Integer

		' Token: 0x04004C96 RID: 19606
		Public equation As VariableEditorGetEquation

		' Token: 0x04004C97 RID: 19607
		Public getValue As String

		' Token: 0x04004C98 RID: 19608
		Private _parsedBool As Boolean

		' Token: 0x04004C99 RID: 19609
		Private _checkBool As Boolean

		' Token: 0x04004C9A RID: 19610
		Private _parsedFloat As Single

		' Token: 0x04004C9B RID: 19611
		Private _checkFloat As Single

		' Token: 0x04004C9C RID: 19612
		Private _parsedString As String

		' Token: 0x04004C9D RID: 19613
		Private _checkString As String
	End Class
End Namespace
