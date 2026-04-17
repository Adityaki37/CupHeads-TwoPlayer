Imports System
Imports UnityEngine

' Token: 0x0200093B RID: 2363
Public Class MapLayerChanger
	Inherits AbstractMonoBehaviour

	' Token: 0x0600374E RID: 14158 RVA: 0x001FD450 File Offset: 0x001FB850
	Private Sub OnTriggerEnter2D(collider As Collider2D)
		Dim componentsInChildren As SpriteRenderer() = collider.GetComponentsInChildren(Of SpriteRenderer)()
		For Each spriteRenderer As SpriteRenderer In componentsInChildren
			spriteRenderer.sortingOrder = Me.sortingOrder
		Next
	End Sub

	' Token: 0x0600374F RID: 14159 RVA: 0x001FD48C File Offset: 0x001FB88C
	Private Sub OnTriggerStay2D(collider As Collider2D)
		Dim componentsInChildren As SpriteRenderer() = collider.GetComponentsInChildren(Of SpriteRenderer)()
		For Each spriteRenderer As SpriteRenderer In componentsInChildren
			spriteRenderer.sortingOrder = Me.sortingOrder
		Next
	End Sub

	' Token: 0x04003F6A RID: 16234
	<SerializeField()>
	Private sortingOrder As Integer
End Class
