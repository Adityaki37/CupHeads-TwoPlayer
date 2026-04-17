Imports System

' Token: 0x02000385 RID: 901
<Serializable()>
Public Structure Rangef
	' Token: 0x06000AA8 RID: 2728 RVA: 0x0007FA5C File Offset: 0x0007DE5C
	Public Sub New(minimum As Single, maximum As Single)
		Me.minimum = minimum
		Me.maximum = maximum
	End Sub

	' Token: 0x06000AA9 RID: 2729 RVA: 0x0007FA6C File Offset: 0x0007DE6C
	Public Function ContainsInclusive(checkValue As Single) As Boolean
		Return MathUtilities.BetweenInclusive(checkValue, Me.minimum, Me.maximum)
	End Function

	' Token: 0x06000AAA RID: 2730 RVA: 0x0007FA80 File Offset: 0x0007DE80
	Public Function ContainsExclusive(checkValue As Single) As Boolean
		Return MathUtilities.BetweenExclusive(checkValue, Me.minimum, Me.maximum)
	End Function

	' Token: 0x06000AAB RID: 2731 RVA: 0x0007FA94 File Offset: 0x0007DE94
	Public Function ContainsInclusiveExclusive(checkValue As Single) As Boolean
		Return MathUtilities.BetweenInclusiveExclusive(checkValue, Me.minimum, Me.maximum)
	End Function

	' Token: 0x06000AAC RID: 2732 RVA: 0x0007FAA8 File Offset: 0x0007DEA8
	Public Function ContainsExclusiveInclusive(checkValue As Single) As Boolean
		Return MathUtilities.BetweenExclusiveInclusive(checkValue, Me.minimum, Me.maximum)
	End Function

	' Token: 0x06000AAD RID: 2733 RVA: 0x0007FABC File Offset: 0x0007DEBC
	Public Overrides Function ToString() As String
		Return String.Format("({0}, {1})", Me.minimum, Me.maximum)
	End Function

	' Token: 0x0400147E RID: 5246
	Public minimum As Single

	' Token: 0x0400147F RID: 5247
	Public maximum As Single
End Structure
