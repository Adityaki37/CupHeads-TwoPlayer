Imports System
Imports System.Globalization

' Token: 0x02000B29 RID: 2857
Public Module Parser
	' Token: 0x06004538 RID: 17720 RVA: 0x00247A6A File Offset: 0x00245E6A
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Function ToStringInvariant(value As Integer) As String
		Return value.ToString(Parser.InvariantInfo)
	End Function

	' Token: 0x06004539 RID: 17721 RVA: 0x00247A78 File Offset: 0x00245E78
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Function ToStringInvariant(value As Single) As String
		Return value.ToString(Parser.InvariantInfo)
	End Function

	' Token: 0x0600453A RID: 17722 RVA: 0x00247A86 File Offset: 0x00245E86
	Public Function IntParse(s As String) As Integer
		Return Integer.Parse(s, Parser.InvariantInfo)
	End Function

	' Token: 0x0600453B RID: 17723 RVA: 0x00247A93 File Offset: 0x00245E93
	Public Function IntTryParse(s As String, <System.Runtime.InteropServices.OutAttribute()> ByRef result As Integer) As Boolean
		Return Integer.TryParse(s, NumberStyles.[Integer], Parser.InvariantInfo, result)
	End Function

	' Token: 0x0600453C RID: 17724 RVA: 0x00247AA2 File Offset: 0x00245EA2
	Public Function FloatParse(s As String) As Single
		Return Single.Parse(s, Parser.InvariantInfo)
	End Function

	' Token: 0x0600453D RID: 17725 RVA: 0x00247AAF File Offset: 0x00245EAF
	Public Function FloatTryParse(s As String, <System.Runtime.InteropServices.OutAttribute()> ByRef result As Single) As Boolean
		Return Single.TryParse(s, NumberStyles.AllowLeadingWhite Or NumberStyles.AllowTrailingWhite Or NumberStyles.AllowLeadingSign Or NumberStyles.AllowDecimalPoint Or NumberStyles.AllowThousands Or NumberStyles.AllowExponent, Parser.InvariantInfo, result)
	End Function

	' Token: 0x0600453E RID: 17726 RVA: 0x00247AC2 File Offset: 0x00245EC2
	Public Function ByteParse(s As String) As Byte
		Return Byte.Parse(s, Parser.InvariantInfo)
	End Function

	' Token: 0x0600453F RID: 17727 RVA: 0x00247ACF File Offset: 0x00245ECF
	Public Function ByteParse(s As String, style As NumberStyles) As Byte
		Return Byte.Parse(s, style, Parser.InvariantInfo)
	End Function

	' Token: 0x06004540 RID: 17728 RVA: 0x00247ADD File Offset: 0x00245EDD
	Public Function ByteTryParse(s As String, <System.Runtime.InteropServices.OutAttribute()> ByRef result As Byte) As Boolean
		Return Byte.TryParse(s, NumberStyles.[Integer], Parser.InvariantInfo, result)
	End Function

	' Token: 0x04004AEA RID: 19178
	Private InvariantInfo As NumberFormatInfo = CultureInfo.InvariantCulture.NumberFormat
End Module
