Imports System

Namespace DialoguerCore
	' Token: 0x02000B78 RID: 2936
	Public Class EndPhase
		Inherits AbstractDialoguePhase

		' Token: 0x060046BC RID: 18108 RVA: 0x0024F4A7 File Offset: 0x0024D8A7
		Public Sub New()
			MyBase.New(Nothing)
		End Sub

		' Token: 0x060046BD RID: 18109 RVA: 0x0024F4B0 File Offset: 0x0024D8B0
		Public Overrides Function ToString() As String
			Return "End Phase" & vbLf
		End Function
	End Class
End Namespace
