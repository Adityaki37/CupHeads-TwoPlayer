Imports System
Imports UnityEngine

' Token: 0x0200095F RID: 2399
Public Class MapSecretPathTrigger
	Inherits AbstractMonoBehaviour

	' Token: 0x06003800 RID: 14336 RVA: 0x00200FBA File Offset: 0x001FF3BA
	Private Sub Start()
		Me.size = MyBase.GetComponent(Of BoxCollider2D)().size
	End Sub

	' Token: 0x06003801 RID: 14337 RVA: 0x00200FD0 File Offset: 0x001FF3D0
	Private Function PointInBounds(pos As Vector3) As Boolean
		Return pos.x > MyBase.transform.position.x - Me.size.x / 2F AndAlso pos.x < MyBase.transform.position.x + Me.size.x / 2F AndAlso pos.y > MyBase.transform.position.y - Me.size.y / 2F AndAlso pos.y < MyBase.transform.position.y + Me.size.y / 2F
	End Function

	' Token: 0x06003802 RID: 14338 RVA: 0x002010A4 File Offset: 0x001FF4A4
	Private Sub OnTriggerStay2D(collider As Collider2D)
		Dim component As MapPlayerController = collider.GetComponent(Of MapPlayerController)()
		If component AndAlso Me.PointInBounds(component.transform.position) Then
			component.SecretPathEnter(Me.enablePath)
		End If
	End Sub

	' Token: 0x04003FE7 RID: 16359
	<SerializeField()>
	Private enablePath As Boolean

	' Token: 0x04003FE8 RID: 16360
	Private size As Vector2
End Class
