Imports System
Imports UnityEngine

' Token: 0x020003E9 RID: 1001
Public Class RookParallaxLayer
	Inherits ParallaxLayer

	' Token: 0x06000D70 RID: 3440 RVA: 0x0008E128 File Offset: 0x0008C528
	Protected Overrides Sub UpdateComparative()
		Dim position As Vector3 = MyBase.transform.position
		position.x = MyBase._offset.x + Me._camera.transform.position.x * Me.percentage
		position.y = MyBase._offset.y + Me._camera.transform.position.y * Me.percentage * Me.yModifier
		MyBase.transform.position = position
	End Sub

	' Token: 0x04001701 RID: 5889
	<SerializeField()>
	Private yModifier As Single = 0.5F
End Class
