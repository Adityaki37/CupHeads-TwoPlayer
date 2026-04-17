Imports System
Imports UnityEngine

' Token: 0x0200035B RID: 859
Public Class LineAttribute
	Inherits PropertyAttribute

	' Token: 0x06000953 RID: 2387 RVA: 0x0007BE7C File Offset: 0x0007A27C
	Public Sub New(height As Integer)
		Me.height = height
	End Sub

	' Token: 0x04001428 RID: 5160
	Public height As Integer
End Class
