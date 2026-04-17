Imports System

Namespace DialoguerCore
	' Token: 0x02000B77 RID: 2935
	Public Class EmptyPhase
		Inherits AbstractDialoguePhase

		' Token: 0x060046BA RID: 18106 RVA: 0x0024F497 File Offset: 0x0024D897
		Public Sub New()
			MyBase.New(Nothing)
		End Sub

		' Token: 0x060046BB RID: 18107 RVA: 0x0024F4A0 File Offset: 0x0024D8A0
		Public Overrides Function ToString() As String
			Return "Empty Phase" & vbLf & "Empty Phases should not be generated, something went wrong." & vbLf
		End Function
	End Class
End Namespace
