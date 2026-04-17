Imports System
Imports UnityEngine

' Token: 0x020003DD RID: 989
Public Class DLCCutsceneParallaxLayer
	Inherits AbstractPausableComponent

	' Token: 0x1700023E RID: 574
	' (get) Token: 0x06000D45 RID: 3397 RVA: 0x0008C85D File Offset: 0x0008AC5D
	Protected ReadOnly Property _offset As Vector2
		Get
			Return Me._startPosition - Me._cameraStartPosition
		End Get
	End Property

	' Token: 0x06000D46 RID: 3398 RVA: 0x0008C875 File Offset: 0x0008AC75
	Protected Overridable Sub Start()
		Me._camera = CupheadCutsceneCamera.Current
		Me._startPosition = MyBase.transform.position
		Me._cameraStartPosition = Me._camera.transform.position
	End Sub

	' Token: 0x06000D47 RID: 3399 RVA: 0x0008C8AC File Offset: 0x0008ACAC
	Private Sub LateUpdate()
		Dim type As DLCCutsceneParallaxLayer.Type = Me.type
		If type = DLCCutsceneParallaxLayer.Type.Comparative OrElse type <> DLCCutsceneParallaxLayer.Type.Centered Then
			Me.UpdateComparative()
		Else
			Me.UpdateCentered()
		End If
	End Sub

	' Token: 0x06000D48 RID: 3400 RVA: 0x0008C8EC File Offset: 0x0008ACEC
	Protected Overridable Sub UpdateComparative()
		Dim position As Vector3 = MyBase.transform.position
		position.x = Me._offset.x + Me._camera.transform.position.x * Me.percentage
		position.y = Me._offset.y + Me._camera.transform.position.y * Me.percentage
		MyBase.transform.position = position
	End Sub

	' Token: 0x06000D49 RID: 3401 RVA: 0x0008C97C File Offset: 0x0008AD7C
	Private Sub UpdateCentered()
		Dim position As Vector3 = MyBase.transform.position
		position.x = Me._startPosition.x + (Me._camera.transform.position.x - Me._startPosition.x) * Me.percentage
		position.y = Me._startPosition.y + (Me._camera.transform.position.y - Me._startPosition.y) * Me.percentage
		MyBase.transform.position = position
	End Sub

	' Token: 0x040016AF RID: 5807
	Public type As DLCCutsceneParallaxLayer.Type

	' Token: 0x040016B0 RID: 5808
	<Range(-3F, 3F)>
	Public percentage As Single

	' Token: 0x040016B1 RID: 5809
	Public bottomLeft As Vector2

	' Token: 0x040016B2 RID: 5810
	Public topRight As Vector2

	' Token: 0x040016B3 RID: 5811
	Protected _camera As AbstractCupheadCamera

	' Token: 0x040016B4 RID: 5812
	Private _initialized As Boolean

	' Token: 0x040016B5 RID: 5813
	Private _startPosition As Vector3

	' Token: 0x040016B6 RID: 5814
	Private _cameraStartPosition As Vector3

	' Token: 0x040016B7 RID: 5815
	<SerializeField()>
	Private overrideCameraRange As Boolean

	' Token: 0x040016B8 RID: 5816
	<SerializeField()>
	Private overrideCameraX As MinMax

	' Token: 0x040016B9 RID: 5817
	<SerializeField()>
	Private overrideCameraY As MinMax

	' Token: 0x020003DE RID: 990
	Public Enum Type
		' Token: 0x040016BB RID: 5819
		MinMax
		' Token: 0x040016BC RID: 5820
		Comparative
		' Token: 0x040016BD RID: 5821
		Centered
	End Enum
End Class
