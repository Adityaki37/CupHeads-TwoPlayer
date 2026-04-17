Imports System
Imports UnityEngine

' Token: 0x02000991 RID: 2449
Public Class MapEquipUICardBackSelectSelectionCursor
	Inherits MapEquipUICursor

	' Token: 0x0600394A RID: 14666 RVA: 0x0020864F File Offset: 0x00206A4F
	Public Overrides Sub SetPosition(position As Vector3)
		MyBase.SetPosition(position)
		Me.Show()
	End Sub

	' Token: 0x0600394B RID: 14667 RVA: 0x0020865E File Offset: 0x00206A5E
	Public Overrides Sub Show()
		MyBase.Show()
		MyBase.animator.Play("Idle")
	End Sub

	' Token: 0x0600394C RID: 14668 RVA: 0x00208676 File Offset: 0x00206A76
	Public Sub [Select]()
		MyBase.animator.Play("Select")
	End Sub

	' Token: 0x040040DC RID: 16604
	Public selectedIndex As Integer = -1
End Class
