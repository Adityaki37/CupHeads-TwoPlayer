Imports System
Imports UnityEngine

' Token: 0x02000368 RID: 872
Public Module ColorExtensions
	' Token: 0x060009BE RID: 2494 RVA: 0x0007CBF0 File Offset: 0x0007AFF0
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Function ToHex(color As Color) As String
		Return ColorUtils.ColorToHex(color, False)
	End Function

	' Token: 0x060009BF RID: 2495 RVA: 0x0007CBFE File Offset: 0x0007AFFE
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Function ToHex(color As Color, alpha As Boolean) As String
		Return ColorUtils.ColorToHex(color, alpha)
	End Function

	' Token: 0x060009C0 RID: 2496 RVA: 0x0007CC0C File Offset: 0x0007B00C
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Function ToNiceString(color As Color) As String
		Return String.Concat(New Object() { "R:", color.r, " G:", color.g, " B:", color.b, " A:", color.a })
	End Function
End Module
