Imports System
Imports UnityEngine
Imports UnityEngine.UI

' Token: 0x02000997 RID: 2455
Public Class MapEquipUICursor
	Inherits AbstractMonoBehaviour

	' Token: 0x0600396B RID: 14699 RVA: 0x002085B8 File Offset: 0x002069B8
	Public Overridable Sub SetPosition(position As Vector3)
		MyBase.transform.position = position
	End Sub

	' Token: 0x0600396C RID: 14700 RVA: 0x002085C6 File Offset: 0x002069C6
	Public Overridable Sub SelectIcon(onSame As Boolean)
		If onSame Then
			MyBase.animator.Play("Select_V2", 1)
		Else
			MyBase.animator.Play("Select", 1)
		End If
	End Sub

	' Token: 0x0600396D RID: 14701 RVA: 0x002085F5 File Offset: 0x002069F5
	Public Overridable Sub OnLocked()
		MyBase.animator.Play("Locked", 1)
	End Sub

	' Token: 0x0600396E RID: 14702 RVA: 0x00208608 File Offset: 0x00206A08
	Public Overridable Sub Hide()
		Me.image.enabled = False
	End Sub

	' Token: 0x0600396F RID: 14703 RVA: 0x00208616 File Offset: 0x00206A16
	Public Overridable Sub Show()
		Me.image.enabled = True
	End Sub

	' Token: 0x06003970 RID: 14704 RVA: 0x00208624 File Offset: 0x00206A24
	Private Sub HideSelectionCursor()
		Me.selectionCursor.enabled = False
	End Sub

	' Token: 0x06003971 RID: 14705 RVA: 0x00208632 File Offset: 0x00206A32
	Private Sub ShowSelectionCursor()
		Me.selectionCursor.enabled = True
	End Sub

	' Token: 0x04004116 RID: 16662
	<SerializeField()>
	Private selectionCursor As Image

	' Token: 0x04004117 RID: 16663
	<SerializeField()>
	Protected image As Image

	' Token: 0x04004118 RID: 16664
	Public index As Integer
End Class
