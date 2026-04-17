Imports System

Namespace TMPro
	' Token: 0x02000C9E RID: 3230
	Public Structure KerningPairKey
		' Token: 0x0600517E RID: 20862 RVA: 0x00299658 File Offset: 0x00297A58
		Public Sub New(ascii_left As Integer, ascii_right As Integer)
			Me.ascii_Left = ascii_left
			Me.ascii_Right = ascii_right
			Me.key = (ascii_right << 16) + ascii_left
		End Sub

		' Token: 0x04005433 RID: 21555
		Public ascii_Left As Integer

		' Token: 0x04005434 RID: 21556
		Public ascii_Right As Integer

		' Token: 0x04005435 RID: 21557
		Public key As Integer
	End Structure
End Namespace
