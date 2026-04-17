Imports System
Imports UnityEngine

' Token: 0x02000405 RID: 1029
Public Class GenericTextHandler
	Inherits AbstractPausableComponent

	' Token: 0x06000E4E RID: 3662 RVA: 0x00092950 File Offset: 0x00090D50
	Private Sub Start()
		For Each gameObject As GameObject In Me.otherText
			gameObject.SetActive(False)
		Next
	End Sub

	' Token: 0x06000E4F RID: 3663 RVA: 0x00092983 File Offset: 0x00090D83
	Private Sub ShowText()
		Me.textChosen.SetActive(True)
	End Sub

	' Token: 0x0400179A RID: 6042
	<SerializeField()>
	Private textChosen As GameObject

	' Token: 0x0400179B RID: 6043
	<SerializeField()>
	Private otherText As GameObject()
End Class
