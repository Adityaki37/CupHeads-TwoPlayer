Imports System

' Token: 0x02000A8F RID: 2703
Public Class ParryAttackSpark
	Inherits Effect

	' Token: 0x1700059D RID: 1437
	' (set) Token: 0x060040A2 RID: 16546 RVA: 0x00232B71 File Offset: 0x00230F71
	Public WriteOnly Property IsCuphead As Boolean
		Set(value As Boolean)
			MyBase.animator.SetBool("IsCuphead", value)
		End Set
	End Property
End Class
