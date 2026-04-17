Imports System
Imports UnityEngine

' Token: 0x020003CF RID: 975
Public Class AudioWarble
	Inherits AbstractPausableComponent

	' Token: 0x06000CB1 RID: 3249 RVA: 0x0008911C File Offset: 0x0008751C
	Public Sub HandleWarble()
		Dim array As Single() = New Single(Me.warbles.Length - 1) {}
		Dim array2 As Single() = New Single(Me.warbles.Length - 1) {}
		Dim array3 As Single() = New Single(Me.warbles.Length - 1) {}
		Dim array4 As Single() = New Single(Me.warbles.Length - 1) {}
		For i As Integer = 0 To Me.warbles.Length - 1
			array(i) = Me.warbles(i).minVal
			array2(i) = Me.warbles(i).maxVal
			array3(i) = Me.warbles(i).warbleTime
			array4(i) = Me.warbles(i).playTime
		Next
		AudioManager.WarbleBGMPitch(Me.warbles.Length, array, array2, array3, array4)
	End Sub

	' Token: 0x0400163D RID: 5693
	<SerializeField()>
	Private warbles As AudioWarble.WarbleAttributes()

	' Token: 0x020003D0 RID: 976
	<Serializable()>
	Public Class WarbleAttributes
		' Token: 0x0400163E RID: 5694
		Public minVal As Single

		' Token: 0x0400163F RID: 5695
		Public maxVal As Single

		' Token: 0x04001640 RID: 5696
		Public warbleTime As Single

		' Token: 0x04001641 RID: 5697
		Public playTime As Single
	End Class
End Class
