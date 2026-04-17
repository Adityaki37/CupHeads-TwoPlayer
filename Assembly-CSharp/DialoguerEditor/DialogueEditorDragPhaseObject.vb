Imports System
Imports UnityEngine

Namespace DialoguerEditor
	' Token: 0x02000B42 RID: 2882
	Public Class DialogueEditorDragPhaseObject
		' Token: 0x060045C0 RID: 17856 RVA: 0x0024C923 File Offset: 0x0024AD23
		Public Sub New(phaseId As Integer, mouseOffset As Vector2)
			Me.phaseId = phaseId
			Me.mouseOffset = mouseOffset
		End Sub

		' Token: 0x04004BEC RID: 19436
		Public phaseId As Integer

		' Token: 0x04004BED RID: 19437
		Public mouseOffset As Vector2
	End Class
End Namespace
