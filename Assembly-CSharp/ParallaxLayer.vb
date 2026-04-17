Imports System
Imports UnityEngine

' Token: 0x020003E7 RID: 999
Public Class ParallaxLayer
	Inherits AbstractPausableComponent

	' Token: 0x17000241 RID: 577
	' (get) Token: 0x06000D69 RID: 3433 RVA: 0x0008DB48 File Offset: 0x0008BF48
	Protected ReadOnly Property _offset As Vector2
		Get
			Return Me._startPosition - Me._cameraStartPosition
		End Get
	End Property

	' Token: 0x06000D6A RID: 3434 RVA: 0x0008DB60 File Offset: 0x0008BF60
	Protected Overridable Sub Start()
		Me._camera = CupheadLevelCamera.Current
		Me._startPosition = MyBase.transform.position
		Me._cameraStartPosition = Me._camera.transform.position
	End Sub

	' Token: 0x06000D6B RID: 3435 RVA: 0x0008DB94 File Offset: 0x0008BF94
	Private Sub LateUpdate()
		Select Case Me.type
			Case ParallaxLayer.Type.MinMax
				Me.UpdateMinMax()
			Case Else
				Me.UpdateComparative()
			Case ParallaxLayer.Type.Centered
				Me.UpdateCentered()
		End Select
	End Sub

	' Token: 0x06000D6C RID: 3436 RVA: 0x0008DBE0 File Offset: 0x0008BFE0
	Protected Overridable Sub UpdateComparative()
		Dim position As Vector3 = MyBase.transform.position
		position.x = Me._offset.x + Me._camera.transform.position.x * Me.percentage
		position.y = Me._offset.y + Me._camera.transform.position.y * Me.percentage
		MyBase.transform.position = position
	End Sub

	' Token: 0x06000D6D RID: 3437 RVA: 0x0008DC70 File Offset: 0x0008C070
	Protected Overridable Sub UpdateMinMax()
		Dim position As Vector3 = MyBase.transform.position
		Dim vector As Vector2 = Me._camera.transform.position
		Dim zero As Vector2 = Vector2.zero
		Dim num As Single = vector.x + Mathf.Abs(Me._camera.Left)
		Dim num2 As Single = Me._camera.Right + Mathf.Abs(Me._camera.Left)
		Dim num3 As Single = vector.y + Mathf.Abs(Me._camera.Bottom)
		Dim num4 As Single = Me._camera.Top + Mathf.Abs(Me._camera.Bottom)
		If Me.overrideCameraRange Then
			num = vector.x + Mathf.Abs(Me.overrideCameraX.min)
			num3 = vector.y + Mathf.Abs(Me.overrideCameraY.min)
			num2 = Me.overrideCameraX.max - Me.overrideCameraX.min
			num4 = Me.overrideCameraY.max - Me.overrideCameraY.min
		End If
		zero.x = num / num2
		zero.y = num3 / num4
		If Single.IsNaN(zero.x) Then
			zero.x = 0.5F
		End If
		If Single.IsNaN(zero.y) Then
			zero.y = 0.5F
		End If
		position.x = Mathf.Lerp(Me.bottomLeft.x, Me.topRight.x, zero.x) + Me._camera.transform.position.x
		position.y = Mathf.Lerp(Me.bottomLeft.y, Me.topRight.y, zero.y) + Me._camera.transform.position.y
		MyBase.transform.position = position
	End Sub

	' Token: 0x06000D6E RID: 3438 RVA: 0x0008DE68 File Offset: 0x0008C268
	Private Sub UpdateCentered()
		Dim position As Vector3 = MyBase.transform.position
		position.x = Me._startPosition.x + (Me._camera.transform.position.x - Me._startPosition.x) * Me.percentage
		position.y = Me._startPosition.y + (Me._camera.transform.position.y - Me._startPosition.y) * Me.percentage
		MyBase.transform.position = position
	End Sub

	' Token: 0x040016F2 RID: 5874
	Public type As ParallaxLayer.Type

	' Token: 0x040016F3 RID: 5875
	<Range(-3F, 3F)>
	Public percentage As Single

	' Token: 0x040016F4 RID: 5876
	Public bottomLeft As Vector2

	' Token: 0x040016F5 RID: 5877
	Public topRight As Vector2

	' Token: 0x040016F6 RID: 5878
	Protected _camera As CupheadLevelCamera

	' Token: 0x040016F7 RID: 5879
	Private _initialized As Boolean

	' Token: 0x040016F8 RID: 5880
	Private _startPosition As Vector3

	' Token: 0x040016F9 RID: 5881
	Private _cameraStartPosition As Vector3

	' Token: 0x040016FA RID: 5882
	<SerializeField()>
	Private overrideCameraRange As Boolean

	' Token: 0x040016FB RID: 5883
	<SerializeField()>
	Private overrideCameraX As MinMax

	' Token: 0x040016FC RID: 5884
	<SerializeField()>
	Private overrideCameraY As MinMax

	' Token: 0x020003E8 RID: 1000
	Public Enum Type
		' Token: 0x040016FE RID: 5886
		MinMax
		' Token: 0x040016FF RID: 5887
		Comparative
		' Token: 0x04001700 RID: 5888
		Centered
	End Enum
End Class
