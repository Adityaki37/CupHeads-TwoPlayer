Imports System

Namespace TMPro
	' Token: 0x02000C99 RID: 3225
	Public Module TMP_Math
		' Token: 0x06005179 RID: 20857 RVA: 0x002995AF File Offset: 0x002979AF
		Public Function Approximately(a As Single, b As Single) As Boolean
			Return b - 0.0001F < a AndAlso a < b + 0.0001F
		End Function
	End Module
End Namespace
