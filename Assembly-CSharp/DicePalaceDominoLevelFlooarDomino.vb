Imports System
Imports UnityEngine

' Token: 0x020005BA RID: 1466
Public Class DicePalaceDominoLevelFlooarDomino
	Inherits MonoBehaviour

	' Token: 0x06001C7B RID: 7291 RVA: 0x00104B28 File Offset: 0x00102F28
	Private Sub Update()
		Dim position As Vector3 = MyBase.transform.position
		position.x -= Me.speed * CupheadTime.Delta
		MyBase.transform.position = position
		If MyBase.transform.position.x <= 0F Then
			position.x += Me.resetPositionX
			MyBase.transform.position = position
		End If
	End Sub

	' Token: 0x04002575 RID: 9589
	<SerializeField()>
	Public speed As Single = 300F

	' Token: 0x04002576 RID: 9590
	Public resetPositionX As Single = 2808F
End Class
