Imports System
Imports UnityEngine

' Token: 0x0200037E RID: 894
<Serializable()>
Public Class MinMax
	' Token: 0x06000A70 RID: 2672 RVA: 0x0007ED17 File Offset: 0x0007D117
	Public Sub New(min As Single, max As Single)
		Me.min = min
		Me.max = max
	End Sub

	' Token: 0x06000A71 RID: 2673 RVA: 0x0007ED2D File Offset: 0x0007D12D
	Public Function RandomFloat() As Single
		Return Global.UnityEngine.Random.Range(Me.min, Me.max)
	End Function

	' Token: 0x06000A72 RID: 2674 RVA: 0x0007ED40 File Offset: 0x0007D140
	Public Function RandomInt() As Integer
		Dim num As Integer = CInt(Me.min)
		Dim num2 As Integer = CInt(Me.max)
		Return Global.UnityEngine.Random.Range(num, num2)
	End Function

	' Token: 0x06000A73 RID: 2675 RVA: 0x0007ED64 File Offset: 0x0007D164
	Public Function GetFloatAt(i As Single) As Single
		Return Mathf.Lerp(Me.min, Me.max, i)
	End Function

	' Token: 0x06000A74 RID: 2676 RVA: 0x0007ED78 File Offset: 0x0007D178
	Public Function GetIntAt(i As Single) As Single
		Return CSng(CInt(Mathf.Lerp(Me.min, Me.max, i)))
	End Function

	' Token: 0x06000A75 RID: 2677 RVA: 0x0007ED8E File Offset: 0x0007D18E
	Public Function Clone() As MinMax
		Return New MinMax(Me.min, Me.max)
	End Function

	' Token: 0x06000A76 RID: 2678 RVA: 0x0007EDA1 File Offset: 0x0007D1A1
	Public Shared Widening Operator CType(m As MinMax) As Single
		Return m.RandomFloat()
	End Operator

	' Token: 0x06000A77 RID: 2679 RVA: 0x0007EDA9 File Offset: 0x0007D1A9
	Public Shared Widening Operator CType(m As MinMax) As Integer
		Return m.RandomInt()
	End Operator

	' Token: 0x04001471 RID: 5233
	Public min As Single

	' Token: 0x04001472 RID: 5234
	Public max As Single
End Class
