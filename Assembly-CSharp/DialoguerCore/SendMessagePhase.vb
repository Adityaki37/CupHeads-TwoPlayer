Imports System
Imports System.Collections.Generic

Namespace DialoguerCore
	' Token: 0x02000B79 RID: 2937
	Public Class SendMessagePhase
		Inherits AbstractDialoguePhase

		' Token: 0x060046BE RID: 18110 RVA: 0x0024F4B7 File Offset: 0x0024D8B7
		Public Sub New(message As String, metadata As String, outs As List(Of Integer))
			MyBase.New(outs)
			Me.message = message
			Me.metadata = metadata
		End Sub

		' Token: 0x060046BF RID: 18111 RVA: 0x0024F4CE File Offset: 0x0024D8CE
		Protected Overrides Sub onStart()
			DialoguerEventManager.dispatchOnMessageEvent(Me.message, Me.metadata)
			MyBase.state = PhaseState.Complete
		End Sub

		' Token: 0x060046C0 RID: 18112 RVA: 0x0024F4E8 File Offset: 0x0024D8E8
		Public Overrides Function ToString() As String
			Return String.Concat(New String() { "Send Message Phase" & vbLf & "Message: ", Me.message, vbLf & "Metadata: ", Me.metadata, vbLf })
		End Function

		' Token: 0x04004C9E RID: 19614
		Public message As String

		' Token: 0x04004C9F RID: 19615
		Public metadata As String
	End Class
End Namespace
