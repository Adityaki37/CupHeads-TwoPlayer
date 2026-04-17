Imports System
Imports UnityEngine

' Token: 0x02000B0C RID: 2828
Public Class AnimatableSortingOrder
	Inherits MonoBehaviour

	' Token: 0x060044A4 RID: 17572 RVA: 0x00246125 File Offset: 0x00244525
	Private Sub Start()
		Me.sr = MyBase.GetComponent(Of SpriteRenderer)()
	End Sub

	' Token: 0x060044A5 RID: 17573 RVA: 0x00246134 File Offset: 0x00244534
	Private Sub LateUpdate()
		Dim num As Integer = CInt(Me.sortingLayer)
		Me.sr.sortingOrder = num
	End Sub

	' Token: 0x04004A5E RID: 19038
	Private sr As SpriteRenderer

	' Token: 0x04004A5F RID: 19039
	Public sortingLayer As Single
End Class
