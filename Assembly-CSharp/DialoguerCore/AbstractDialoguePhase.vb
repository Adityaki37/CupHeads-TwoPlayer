Imports System
Imports System.Collections.Generic
Imports System.Diagnostics

Namespace DialoguerCore
	' Token: 0x02000B73 RID: 2931
	Public MustInherit Class AbstractDialoguePhase
		' Token: 0x060046A3 RID: 18083 RVA: 0x0024ECF8 File Offset: 0x0024D0F8
		Public Sub New(outs As List(Of Integer))
			If outs IsNot Nothing Then
				Dim array As Integer() = outs.ToArray()
				Me.outs = TryCast(array.Clone(), Integer())
			End If
		End Sub

		' Token: 0x17000642 RID: 1602
		' (get) Token: 0x060046A4 RID: 18084 RVA: 0x0024ED29 File Offset: 0x0024D129
		' (set) Token: 0x060046A5 RID: 18085 RVA: 0x0024ED34 File Offset: 0x0024D134
		Public Property state As PhaseState
			Get
				Return Me._state
			End Get
			Protected Set(value As PhaseState)
				Me._state = value
				Select Case Me._state
					Case PhaseState.Start
						Me.onStart()
					Case PhaseState.Action
						Me.onAction()
					Case PhaseState.Complete
						Me.onComplete()
				End Select
			End Set
		End Property

		' Token: 0x060046A6 RID: 18086 RVA: 0x0024ED90 File Offset: 0x0024D190
		Public Sub Start(localVars As DialoguerVariables)
			Me.Reset()
			Me._localVariables = localVars
			Me.state = PhaseState.Start
		End Sub

		' Token: 0x060046A7 RID: 18087 RVA: 0x0024EDA8 File Offset: 0x0024D1A8
		Public Overridable Sub [Continue](outId As Integer)
			Dim num As Integer = 0
			If Me.outs IsNot Nothing AndAlso Me.outs(outId) >= 0 Then
				num = Me.outs(outId)
			End If
			Me.nextPhaseId = num
		End Sub

		' Token: 0x060046A8 RID: 18088 RVA: 0x0024EDE5 File Offset: 0x0024D1E5
		Protected Overridable Sub onStart()
			Me.state = PhaseState.Action
		End Sub

		' Token: 0x060046A9 RID: 18089 RVA: 0x0024EDEE File Offset: 0x0024D1EE
		Protected Overridable Sub onAction()
			Me.state = PhaseState.Complete
		End Sub

		' Token: 0x060046AA RID: 18090 RVA: 0x0024EDF7 File Offset: 0x0024D1F7
		Protected Overridable Sub onComplete()
			Me.dispatchPhaseComplete(Me.nextPhaseId)
			Me.state = PhaseState.Inactive
			Me.Reset()
		End Sub

		' Token: 0x060046AB RID: 18091 RVA: 0x0024EE12 File Offset: 0x0024D212
		Protected Overridable Sub Reset()
			Me.nextPhaseId = If((Me.outs Is Nothing OrElse Me.outs(0) < 0), 0, Me.outs(0))
			Me._localVariables = Nothing
		End Sub

		' Token: 0x140000E3 RID: 227
		' (add) Token: 0x060046AC RID: 18092 RVA: 0x0024EE48 File Offset: 0x0024D248
		' (remove) Token: 0x060046AD RID: 18093 RVA: 0x0024EE80 File Offset: 0x0024D280
		<DebuggerBrowsable(DebuggerBrowsableState.Never)>
		Public Event onPhaseComplete As AbstractDialoguePhase.PhaseCompleteHandler

		' Token: 0x060046AE RID: 18094 RVA: 0x0024EEB6 File Offset: 0x0024D2B6
		Private Sub dispatchPhaseComplete(nextPhaseId As Integer)
			If Me.onPhaseComplete IsNot Nothing Then
				Me.onPhaseComplete(nextPhaseId)
			End If
		End Sub

		' Token: 0x060046AF RID: 18095 RVA: 0x0024EECF File Offset: 0x0024D2CF
		Public Sub resetEvents()
			Me.onPhaseComplete = Nothing
		End Sub

		' Token: 0x060046B0 RID: 18096 RVA: 0x0024EED8 File Offset: 0x0024D2D8
		Public Overrides Function ToString() As String
			Return "AbstractDialoguePhase"
		End Function

		' Token: 0x04004C8D RID: 19597
		Public outs As Integer()

		' Token: 0x04004C8E RID: 19598
		Protected nextPhaseId As Integer

		' Token: 0x04004C8F RID: 19599
		Protected _localVariables As DialoguerVariables

		' Token: 0x04004C90 RID: 19600
		Private _state As PhaseState

		' Token: 0x02000B74 RID: 2932
		' (Invoke) Token: 0x060046B2 RID: 18098
		Public Delegate Sub PhaseCompleteHandler(nextPhaseId As Integer)
	End Class
End Namespace
