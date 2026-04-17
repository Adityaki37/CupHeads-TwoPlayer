Imports System
Imports UnityEngine

' Token: 0x0200092D RID: 2349
Public Class ChangeLightingZone
	Inherits AbstractCollidableObject

	' Token: 0x060036FA RID: 14074 RVA: 0x001FB118 File Offset: 0x001F9518
	Private Sub Start()
		Me._filter = Nothing.NoFilter()
	End Sub

	' Token: 0x060036FB RID: 14075 RVA: 0x001FB13C File Offset: 0x001F953C
	Private Sub Update()
		Dim num As Integer = Me._collider.OverlapCollider(Me._filter, Me.buffer)
		For i As Integer = 0 To num - 1
			Dim component As MapPlayerAnimationController = Me.buffer(i).GetComponent(Of MapPlayerAnimationController)()
			If Not(component Is Nothing) Then
				Dim magnitude As Single = (component.transform.position - MyBase.transform.position).magnitude
				Dim num2 As Single = Mathf.Clamp(magnitude / Me._maxDistance, 0F, 1F)
				component.spriteRenderer.color = Color.Lerp(Me._minTint, Me._maxTint, num2)
			End If
		Next
	End Sub

	' Token: 0x04003F2A RID: 16170
	<SerializeField()>
	Private _minTint As Color

	' Token: 0x04003F2B RID: 16171
	<SerializeField()>
	Private _maxTint As Color

	' Token: 0x04003F2C RID: 16172
	<SerializeField()>
	Private _collider As BoxCollider2D

	' Token: 0x04003F2D RID: 16173
	<SerializeField()>
	Private _maxDistance As Single

	' Token: 0x04003F2E RID: 16174
	Private _filter As ContactFilter2D

	' Token: 0x04003F2F RID: 16175
	Private buffer As Collider2D() = New Collider2D(9) {}
End Class
