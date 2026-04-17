Imports System

Namespace TMPro
	' Token: 0x02000C9F RID: 3231
	<Serializable()>
	Public Class KerningPair
		' Token: 0x0600517F RID: 20863 RVA: 0x00299674 File Offset: 0x00297A74
		Public Sub New(left As Integer, right As Integer, offset As Single)
			Me.AscII_Left = left
			Me.AscII_Right = right
			Me.XadvanceOffset = offset
		End Sub

		' Token: 0x04005436 RID: 21558
		Public AscII_Left As Integer

		' Token: 0x04005437 RID: 21559
		Public AscII_Right As Integer

		' Token: 0x04005438 RID: 21560
		Public XadvanceOffset As Single
	End Class
End Namespace
