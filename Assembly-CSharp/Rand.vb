Imports System
Imports UnityEngine

' Token: 0x02000383 RID: 899
Public Module Rand
	' Token: 0x06000AA3 RID: 2723 RVA: 0x0007F971 File Offset: 0x0007DD71
	Public Function Bool() As Boolean
		Return Global.UnityEngine.Random.Range(0, 2) = 1
	End Function

	' Token: 0x06000AA4 RID: 2724 RVA: 0x0007F987 File Offset: 0x0007DD87
	Public Function PosOrNeg() As Integer
		Return If((Not Rand.Bool()), (-1), 1)
	End Function
End Module
