Imports System

' Token: 0x02000399 RID: 921
Public Module TimeUtils
	' Token: 0x06000B38 RID: 2872 RVA: 0x00082594 File Offset: 0x00080994
	Public Function GetCurrentSecond() As Integer
		Dim dateTime As DateTime = New DateTime(1970, 1, 1, 8, 0, 0, DateTimeKind.Utc)
		Return CInt((DateTime.UtcNow - dateTime).TotalSeconds)
	End Function
End Module
