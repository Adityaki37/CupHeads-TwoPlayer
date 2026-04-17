Imports System
Imports UnityEngine

' Token: 0x02000967 RID: 2407
Public Class MapZoomOut
	Inherits AbstractCollidableObject

	' Token: 0x06003818 RID: 14360 RVA: 0x00201814 File Offset: 0x001FFC14
	Private Sub Start()
		Me._filter = Nothing.NoFilter()
		Me._startSize = Me._camera.camera.orthographicSize
		Me._collider = MyBase.GetComponent(Of BoxCollider2D)()
		Me._maxDistance = Me._collider.bounds.extents.y
		Me._zoomDistance = Me._maxZoomOut - Me._startSize
	End Sub

	' Token: 0x06003819 RID: 14361 RVA: 0x0020188C File Offset: 0x001FFC8C
	Private Sub Update()
		Dim num As Integer = Me._collider.OverlapCollider(Me._filter, Me.buffer)
		Dim num2 As Single = 0F
		Dim num3 As Single = 0F
		For i As Integer = 0 To num - 1
			Dim component As MapPlayerController = Me.buffer(i).GetComponent(Of MapPlayerController)()
			If Not(component Is Nothing) Then
				num3 += 1F
				Dim magnitude As Single = (component.transform.position - MyBase.transform.position).magnitude
				num2 = If((num2 < magnitude), magnitude, num2)
			End If
		Next
		If(PlayerManager.Multiplayer AndAlso num3 = 2F) OrElse (Not PlayerManager.Multiplayer AndAlso num3 = 1F) Then
			Me._currentZoomRatio = 1F - Mathf.Clamp(num2 / Me._maxDistance, 0F, 1F)
		Else
			Me._currentZoomRatio = 0F
		End If
		Me._camera.camera.orthographicSize = Mathf.Lerp(Me._camera.camera.orthographicSize, Me.EaseInOutQuad(Me._startSize, Me._zoomDistance, Me._currentZoomRatio), Time.deltaTime * Me.ZoomSharpness)
	End Sub

	' Token: 0x0600381A RID: 14362 RVA: 0x002019D8 File Offset: 0x001FFDD8
	Private Function EaseInOutQuad(startValue As Single, endValue As Single, time As Single) As Single
		time *= 2F
		If time < 1F Then
			Return endValue / 2F * time * time + startValue
		End If
		time -= 1F
		Return-endValue / 2F * (time * (time - 2F) - 1F) + startValue
	End Function

	' Token: 0x04003FF3 RID: 16371
	<SerializeField()>
	Private _camera As CupheadMapCamera

	' Token: 0x04003FF4 RID: 16372
	<SerializeField()>
	Private _maxZoomOut As Single

	' Token: 0x04003FF5 RID: 16373
	<SerializeField()>
	Private ZoomSharpness As Single = 1F

	' Token: 0x04003FF6 RID: 16374
	Private _startSize As Single

	' Token: 0x04003FF7 RID: 16375
	Private _maxDistance As Single

	' Token: 0x04003FF8 RID: 16376
	Private _zoomDistance As Single

	' Token: 0x04003FF9 RID: 16377
	Private _currentZoomRatio As Single

	' Token: 0x04003FFA RID: 16378
	Private _collider As BoxCollider2D

	' Token: 0x04003FFB RID: 16379
	Private _filter As ContactFilter2D

	' Token: 0x04003FFC RID: 16380
	Private buffer As Collider2D() = New Collider2D(9) {}
End Class
