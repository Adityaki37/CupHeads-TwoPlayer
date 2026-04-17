Imports System

Namespace DialoguerEditor
	' Token: 0x02000B4A RID: 2890
	Public Class DialogueEditorSelectionObject
		' Token: 0x060045DD RID: 17885 RVA: 0x0024D681 File Offset: 0x0024BA81
		Public Sub New(phaseId As Integer, outputIndex As Integer)
			If phaseId < 0 Then
				phaseId = 0
			End If
			If outputIndex < 0 Then
				outputIndex = 0
			End If
			Me.phaseId = phaseId
			Me.outputIndex = outputIndex
			Me.isStart = False
		End Sub

		' Token: 0x060045DE RID: 17886 RVA: 0x0024D6B2 File Offset: 0x0024BAB2
		Public Sub New(isStart As Boolean)
			Me.isStart = True
			Me.phaseId = Integer.MinValue
			Me.outputIndex = Integer.MinValue
		End Sub

		' Token: 0x1700063A RID: 1594
		' (get) Token: 0x060045DF RID: 17887 RVA: 0x0024D6D7 File Offset: 0x0024BAD7
		' (set) Token: 0x060045E0 RID: 17888 RVA: 0x0024D6DF File Offset: 0x0024BADF
		Public Property phaseId As Integer

		' Token: 0x1700063B RID: 1595
		' (get) Token: 0x060045E1 RID: 17889 RVA: 0x0024D6E8 File Offset: 0x0024BAE8
		' (set) Token: 0x060045E2 RID: 17890 RVA: 0x0024D6F0 File Offset: 0x0024BAF0
		Public Property outputIndex As Integer

		' Token: 0x1700063C RID: 1596
		' (get) Token: 0x060045E3 RID: 17891 RVA: 0x0024D6F9 File Offset: 0x0024BAF9
		' (set) Token: 0x060045E4 RID: 17892 RVA: 0x0024D701 File Offset: 0x0024BB01
		Public Property isStart As Boolean
	End Class
End Namespace
