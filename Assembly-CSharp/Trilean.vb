Imports System
Imports UnityEngine

' Token: 0x0200037F RID: 895
Public Structure Trilean
	' Token: 0x06000A78 RID: 2680 RVA: 0x0007EDB1 File Offset: 0x0007D1B1
	Public Sub New(b As Boolean)
		If b Then
			Me.value = 1
		Else
			Me.value = -1
		End If
	End Sub

	' Token: 0x06000A79 RID: 2681 RVA: 0x0007EDCC File Offset: 0x0007D1CC
	Public Sub New(i As Integer)
		Me.value = i
	End Sub

	' Token: 0x06000A7A RID: 2682 RVA: 0x0007EDD5 File Offset: 0x0007D1D5
	Public Sub New(f As Single)
		If f = 0F Then
			Me.value = 0
		Else
			Me.value = CInt(Mathf.Sign(f))
		End If
	End Sub

	' Token: 0x170001FC RID: 508
	' (get) Token: 0x06000A7B RID: 2683 RVA: 0x0007EDFB File Offset: 0x0007D1FB
	' (set) Token: 0x06000A7C RID: 2684 RVA: 0x0007EE03 File Offset: 0x0007D203
	Public Property Value As Integer
		Get
			Return Me.value
		End Get
		Set(value As Integer)
			If value > 0 Then
				Me.value = 1
			ElseIf value < 0 Then
				Me.value = -1
			Else
				Me.value = 0
			End If
		End Set
	End Property

	' Token: 0x06000A7D RID: 2685 RVA: 0x0007EE32 File Offset: 0x0007D232
	Public Shared Widening Operator CType(b As Boolean) As Trilean
		Return New Trilean(b)
	End Operator

	' Token: 0x06000A7E RID: 2686 RVA: 0x0007EE3A File Offset: 0x0007D23A
	Public Shared Widening Operator CType(t As Trilean) As Boolean
		Return t.Value >= 0
	End Operator

	' Token: 0x06000A7F RID: 2687 RVA: 0x0007EE50 File Offset: 0x0007D250
	Public Shared Widening Operator CType(i As Integer) As Trilean
		Return New Trilean(i)
	End Operator

	' Token: 0x06000A80 RID: 2688 RVA: 0x0007EE58 File Offset: 0x0007D258
	Public Shared Widening Operator CType(t As Trilean) As Integer
		Return t.Value
	End Operator

	' Token: 0x06000A81 RID: 2689 RVA: 0x0007EE61 File Offset: 0x0007D261
	Public Shared Widening Operator CType(f As Single) As Trilean
		Return New Trilean(f)
	End Operator

	' Token: 0x06000A82 RID: 2690 RVA: 0x0007EE69 File Offset: 0x0007D269
	Public Shared Widening Operator CType(t As Trilean) As Single
		Return CSng(t.Value)
	End Operator

	' Token: 0x06000A83 RID: 2691 RVA: 0x0007EE73 File Offset: 0x0007D273
	Public Overrides Function ToString() As String
		Return Me.Value.ToStringInvariant()
	End Function

	' Token: 0x04001473 RID: 5235
	Private value As Integer
End Structure
