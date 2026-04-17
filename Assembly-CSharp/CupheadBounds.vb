Imports System
Imports UnityEngine

' Token: 0x0200037B RID: 891
<Serializable()>
Public Class CupheadBounds
	' Token: 0x06000A60 RID: 2656 RVA: 0x0007E908 File Offset: 0x0007CD08
	Public Sub New()
		Me.left = 0F
		Me.right = 0F
		Me.top = 0F
		Me.bottom = 0F
	End Sub

	' Token: 0x06000A61 RID: 2657 RVA: 0x0007E93C File Offset: 0x0007CD3C
	Public Sub New(left As Single, right As Single, top As Single, bottom As Single)
		Me.left = left
		Me.right = right
		Me.top = top
		Me.bottom = bottom
	End Sub

	' Token: 0x06000A62 RID: 2658 RVA: 0x0007E964 File Offset: 0x0007CD64
	Public Sub New(r As Rect)
		Me.left = r.center.x - r.x
		Me.top = r.center.y - r.y
		Me.right = r.xMax - r.center.x
		Me.bottom = r.yMax - r.center.y
	End Sub

	' Token: 0x06000A63 RID: 2659 RVA: 0x0007E9EB File Offset: 0x0007CDEB
	Public Shared Widening Operator CType(r As Rect) As CupheadBounds
		Return New CupheadBounds(r)
	End Operator

	' Token: 0x06000A64 RID: 2660 RVA: 0x0007E9F3 File Offset: 0x0007CDF3
	Public Function Copy() As CupheadBounds
		Return TryCast(MyBase.MemberwiseClone(), CupheadBounds)
	End Function

	' Token: 0x04001467 RID: 5223
	Public left As Single

	' Token: 0x04001468 RID: 5224
	Public right As Single

	' Token: 0x04001469 RID: 5225
	Public top As Single

	' Token: 0x0400146A RID: 5226
	Public bottom As Single
End Class
