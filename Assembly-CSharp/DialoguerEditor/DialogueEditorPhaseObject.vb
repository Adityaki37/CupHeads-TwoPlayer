Imports System
Imports System.Collections.Generic
Imports UnityEngine

Namespace DialoguerEditor
	' Token: 0x02000B46 RID: 2886
	<Serializable()>
	Public Class DialogueEditorPhaseObject
		' Token: 0x060045CC RID: 17868 RVA: 0x0024D084 File Offset: 0x0024B484
		Public Sub New()
			Me.type = DialogueEditorPhaseTypes.EmptyPhase
			Me.position = Vector2.zero
			Me.text = String.Empty
			Me.outs = New List(Of Integer)()
			Me.choices = New List(Of String)()
			Me.waitType = DialogueEditorWaitTypes.Seconds
		End Sub

		' Token: 0x060045CD RID: 17869 RVA: 0x0024D0D1 File Offset: 0x0024B4D1
		Public Sub addNewOut()
			Me.outs.Add(-1)
		End Sub

		' Token: 0x060045CE RID: 17870 RVA: 0x0024D0DF File Offset: 0x0024B4DF
		Public Sub removeOut()
			Me.outs.RemoveAt(Me.outs.Count - 1)
		End Sub

		' Token: 0x060045CF RID: 17871 RVA: 0x0024D0F9 File Offset: 0x0024B4F9
		Public Sub addNewChoice()
			Me.addNewOut()
			Me.choices.Add(String.Empty)
		End Sub

		' Token: 0x060045D0 RID: 17872 RVA: 0x0024D111 File Offset: 0x0024B511
		Public Sub removeChoice()
			Me.removeOut()
			Me.choices.RemoveAt(Me.choices.Count - 1)
		End Sub

		' Token: 0x04004BF8 RID: 19448
		Public id As Integer

		' Token: 0x04004BF9 RID: 19449
		Public type As DialogueEditorPhaseTypes

		' Token: 0x04004BFA RID: 19450
		Public paramaters As Object

		' Token: 0x04004BFB RID: 19451
		Public theme As String

		' Token: 0x04004BFC RID: 19452
		Public position As Vector2

		' Token: 0x04004BFD RID: 19453
		Public outs As List(Of Integer)

		' Token: 0x04004BFE RID: 19454
		Public advanced As Boolean

		' Token: 0x04004BFF RID: 19455
		Public metadata As String

		' Token: 0x04004C00 RID: 19456
		Public text As String

		' Token: 0x04004C01 RID: 19457
		Public name As String

		' Token: 0x04004C02 RID: 19458
		Public portrait As String

		' Token: 0x04004C03 RID: 19459
		Public audio As String

		' Token: 0x04004C04 RID: 19460
		Public audioDelay As Single

		' Token: 0x04004C05 RID: 19461
		Public rect As Rect

		' Token: 0x04004C06 RID: 19462
		Public newWindow As Boolean

		' Token: 0x04004C07 RID: 19463
		Public choices As List(Of String)

		' Token: 0x04004C08 RID: 19464
		Public waitType As DialogueEditorWaitTypes

		' Token: 0x04004C09 RID: 19465
		Public waitDuration As Single

		' Token: 0x04004C0A RID: 19466
		Public variableScope As VariableEditorScopes

		' Token: 0x04004C0B RID: 19467
		Public variableType As VariableEditorTypes

		' Token: 0x04004C0C RID: 19468
		Public variableId As Integer

		' Token: 0x04004C0D RID: 19469
		Public variableScrollPosition As Vector2

		' Token: 0x04004C0E RID: 19470
		Public variableSetEquation As VariableEditorSetEquation

		' Token: 0x04004C0F RID: 19471
		Public variableSetValue As String

		' Token: 0x04004C10 RID: 19472
		Public variableGetEquation As VariableEditorGetEquation

		' Token: 0x04004C11 RID: 19473
		Public variableGetValue As String

		' Token: 0x04004C12 RID: 19474
		Public messageName As String
	End Class
End Namespace
