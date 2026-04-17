Imports System
Imports UnityEngine

' Token: 0x020003E6 RID: 998
Public Class MapParallaxLayer
	Inherits AbstractPausableComponent

	' Token: 0x17000240 RID: 576
	' (get) Token: 0x06000D64 RID: 3428 RVA: 0x0008E045 File Offset: 0x0008C445
	Private ReadOnly Property _offset As Vector2
		Get
			Return Me._startPosition - Me._cameraStartPosition
		End Get
	End Property

	' Token: 0x06000D65 RID: 3429 RVA: 0x0008E05D File Offset: 0x0008C45D
	Private Sub Start()
		Me._camera = CupheadMapCamera.Current
		Me._startPosition = MyBase.transform.position
	End Sub

	' Token: 0x06000D66 RID: 3430 RVA: 0x0008E07B File Offset: 0x0008C47B
	Private Sub LateUpdate()
		Me.UpdateComparative()
	End Sub

	' Token: 0x06000D67 RID: 3431 RVA: 0x0008E084 File Offset: 0x0008C484
	Private Sub UpdateComparative()
		Dim position As Vector3 = MyBase.transform.position
		position.x = Me._offset.x + Me._camera.transform.position.x * Me.percentage
		position.y = Me._offset.y + Me._camera.transform.position.y * Me.percentage
		MyBase.transform.position = position
	End Sub

	' Token: 0x040016EE RID: 5870
	<Range(-3F, 3F)>
	Public percentage As Single

	' Token: 0x040016EF RID: 5871
	<SerializeField()>
	Private _cameraStartPosition As Vector3

	' Token: 0x040016F0 RID: 5872
	Private _camera As CupheadMapCamera

	' Token: 0x040016F1 RID: 5873
	Private _startPosition As Vector3
End Class
