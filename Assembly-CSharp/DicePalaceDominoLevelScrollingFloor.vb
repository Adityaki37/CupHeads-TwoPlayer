Imports System
Imports UnityEngine

' Token: 0x020005BE RID: 1470
Public Class DicePalaceDominoLevelScrollingFloor
	Inherits MonoBehaviour

	' Token: 0x06001C92 RID: 7314 RVA: 0x001050F7 File Offset: 0x001034F7
	Private Sub Start()
		Me.RefreshTilesAndSpikes()
	End Sub

	' Token: 0x06001C93 RID: 7315 RVA: 0x00105100 File Offset: 0x00103500
	Private Sub Update()
		Dim position As Vector3 = MyBase.transform.position
		position.x -= Me.speed * CupheadTime.Delta
		MyBase.transform.position = position
		If MyBase.transform.position.x <= 0F Then
			position.x += Me.resetPositionX
			MyBase.transform.position = position
			Me.RefreshTilesAndSpikes()
		End If
	End Sub

	' Token: 0x06001C94 RID: 7316 RVA: 0x00105188 File Offset: 0x00103588
	Private Sub RefreshTilesAndSpikes()
		For i As Integer = 0 To Me.dominoLevelRandomTiles.Length - 1
			Me.dominoLevelRandomTiles(i).ChangeTile()
		Next
		For j As Integer = 0 To Me.dominoLevelRandomSpikes.Length - 1
			Me.dominoLevelRandomSpikes(j).ChangeSpikes()
		Next
	End Sub

	' Token: 0x06001C95 RID: 7317 RVA: 0x001051E1 File Offset: 0x001035E1
	Private Sub OnDestroy()
		Me.dominoLevelRandomTiles = Nothing
		Me.dominoLevelRandomSpikes = Nothing
	End Sub

	' Token: 0x0400257E RID: 9598
	Public speed As Single

	' Token: 0x0400257F RID: 9599
	Public resetPositionX As Single = 2808F

	' Token: 0x04002580 RID: 9600
	<SerializeField()>
	Private dominoLevelRandomTiles As DicePalaceDominoLevelRandomTile()

	' Token: 0x04002581 RID: 9601
	<SerializeField()>
	Private dominoLevelRandomSpikes As DicePalaceDominoLevelRandomSpike()
End Class
