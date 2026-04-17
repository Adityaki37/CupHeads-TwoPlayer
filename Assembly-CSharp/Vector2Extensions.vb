Imports System
Imports UnityEngine

' Token: 0x02000371 RID: 881
Public Module Vector2Extensions
	' Token: 0x06000A28 RID: 2600 RVA: 0x0007E390 File Offset: 0x0007C790
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Function [Set](v As Vector2, Optional x As Single? = Nothing, Optional y As Single? = Nothing) As Vector2
		Dim vector As Vector2 = v
		If x IsNot Nothing Then
			vector.x = x.Value
		End If
		If y IsNot Nothing Then
			vector.y = y.Value
		End If
		Return vector
	End Function
End Module
