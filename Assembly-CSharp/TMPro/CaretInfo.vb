Imports System

Namespace TMPro
	' Token: 0x02000C8F RID: 3215
	Public Structure CaretInfo
		' Token: 0x06005129 RID: 20777 RVA: 0x00296539 File Offset: 0x00294939
		Public Sub New(index As Integer, position As CaretPosition)
			Me.index = index
			Me.position = position
		End Sub

		' Token: 0x040053E7 RID: 21479
		Public index As Integer

		' Token: 0x040053E8 RID: 21480
		Public position As CaretPosition
	End Structure
End Namespace
