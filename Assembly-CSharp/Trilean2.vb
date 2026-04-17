Imports System
Imports UnityEngine

' Token: 0x02000380 RID: 896
Public Structure Trilean2
	' Token: 0x06000A84 RID: 2692 RVA: 0x0007EE80 File Offset: 0x0007D280
	Public Sub New(v As Vector2)
		Me.x = v.x
		Me.y = v.y
	End Sub

	' Token: 0x06000A85 RID: 2693 RVA: 0x0007EEA6 File Offset: 0x0007D2A6
	Public Sub New(x As Boolean, y As Boolean)
		Me.x = x
		Me.y = y
	End Sub

	' Token: 0x06000A86 RID: 2694 RVA: 0x0007EEC0 File Offset: 0x0007D2C0
	Public Sub New(x As Integer, y As Integer)
		Me.x = x
		Me.y = y
	End Sub

	' Token: 0x06000A87 RID: 2695 RVA: 0x0007EEDA File Offset: 0x0007D2DA
	Public Sub New(x As Single, y As Single)
		Me.x = x
		Me.y = y
	End Sub

	' Token: 0x06000A88 RID: 2696 RVA: 0x0007EEF4 File Offset: 0x0007D2F4
	Public Shared Widening Operator CType(v As Vector2) As Trilean2
		Return New Trilean2(v)
	End Operator

	' Token: 0x06000A89 RID: 2697 RVA: 0x0007EEFC File Offset: 0x0007D2FC
	Public Shared Widening Operator CType(t As Trilean2) As Vector2
		Return New Vector2(t.x, t.y)
	End Operator

	' Token: 0x06000A8A RID: 2698 RVA: 0x0007EF1B File Offset: 0x0007D31B
	Public Overrides Function Equals(obj As Object) As Boolean
		Return MyBase.Equals(obj)
	End Function

	' Token: 0x06000A8B RID: 2699 RVA: 0x0007EF2E File Offset: 0x0007D32E
	Public Overrides Function GetHashCode() As Integer
		Return MyBase.GetHashCode()
	End Function

	' Token: 0x06000A8C RID: 2700 RVA: 0x0007EF40 File Offset: 0x0007D340
	Public Overrides Function ToString() As String
		Return String.Concat(New Object() { "Trilean2(x:", Me.x.Value, ", y:", Me.y.Value, ")" })
	End Function

	' Token: 0x06000A8D RID: 2701 RVA: 0x0007EF96 File Offset: 0x0007D396
	Public Shared Operator =(a As Trilean2, b As Trilean2) As Boolean
		Return a.x.Value = b.x.Value AndAlso a.y.Value = b.y.Value
	End Operator

	' Token: 0x06000A8E RID: 2702 RVA: 0x0007EFD2 File Offset: 0x0007D3D2
	Public Shared Operator <>(a As Trilean2, b As Trilean2) As Boolean
		Return a.x.Value <> b.x.Value OrElse a.y.Value <> b.y.Value
	End Operator

	' Token: 0x04001474 RID: 5236
	Public x As Trilean

	' Token: 0x04001475 RID: 5237
	Public y As Trilean
End Structure
