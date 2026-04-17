Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x020005B9 RID: 1465
Public Class DicePalaceDominoLevelFloor
	Inherits AbstractCollidableObject

	' Token: 0x06001C73 RID: 7283 RVA: 0x001046FD File Offset: 0x00102AFD
	Public Sub InitFloor(properties As LevelProperties.DicePalaceDomino)
		Me.properties = properties
		Me.tiles = New List(Of DicePalaceDominoLevelFloorTile)()
		Me.preTiles = New List(Of DicePalaceDominoLevelFloorTile)()
	End Sub

	' Token: 0x06001C74 RID: 7284 RVA: 0x0010471C File Offset: 0x00102B1C
	Public Sub StartSpawningTiles()
		MyBase.StartCoroutine(Me.tileSpawn_cr())
	End Sub

	' Token: 0x06001C75 RID: 7285 RVA: 0x0010472C File Offset: 0x00102B2C
	Private Iterator Function tileSpawn_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 3F)
		For i As Integer = 0 To Me._floors.Length - 1
			Me._floors(i).speed = Me.properties.CurrentState.domino.floorSpeed
		Next
		Me._teethSprite.speed = Me.properties.CurrentState.domino.floorSpeed
		Me.AddForces()
		For j As Integer = 0 To Me.preTiles.Count - 1
			If Me.preTiles(j).currentColourIndex = CInt(Me.spikesColour) Then
				Me.preTiles(j).TriggerSpikes(True)
			Else
				Me.preTiles(j).TriggerSpikes(False)
			End If
			Me.preTiles(j).InitTile()
		Next
		Return
	End Function

	' Token: 0x06001C76 RID: 7286 RVA: 0x00104748 File Offset: 0x00102B48
	Private Sub AddForces()
		For Each abstractPlayerController As AbstractPlayerController In PlayerManager.GetAllPlayers()
			Dim levelPlayerController As LevelPlayerController = CType(abstractPlayerController, LevelPlayerController)
			If Not(levelPlayerController Is Nothing) Then
				Me.levelForce = New LevelPlayerMotor.VelocityManager.Force(LevelPlayerMotor.VelocityManager.Force.Type.Ground, -Me.properties.CurrentState.domino.floorSpeed)
				levelPlayerController.motor.AddForce(Me.levelForce)
			End If
		Next
	End Sub

	' Token: 0x06001C77 RID: 7287 RVA: 0x001047E8 File Offset: 0x00102BE8
	Private Function ParseColour(c As Char) As Integer
		If c = "B"c Then
			Return 0
		End If
		If c = "G"c Then
			Return 1
		End If
		If c = "R"c Then
			Return 2
		End If
		If c <> "Y"c Then
			Return 0
		End If
		Return 3
	End Function

	' Token: 0x06001C78 RID: 7288 RVA: 0x00104818 File Offset: 0x00102C18
	Public Sub CheckTiles(color As DicePalaceDominoLevelBouncyBall.Colour)
		Me.spikesColour = color
		MyBase.StartCoroutine(Me.check_tiles_cr(Me.spikesColour))
	End Sub

	' Token: 0x06001C79 RID: 7289 RVA: 0x00104834 File Offset: 0x00102C34
	Private Iterator Function check_tiles_cr(color As DicePalaceDominoLevelBouncyBall.Colour) As IEnumerator
		For Each dicePalaceDominoLevelFloorTile As DicePalaceDominoLevelFloorTile In Me.tiles
			If dicePalaceDominoLevelFloorTile.isActivated Then
				If dicePalaceDominoLevelFloorTile.currentColourIndex = CInt(color) Then
					dicePalaceDominoLevelFloorTile.TriggerSpikes(True)
				Else
					dicePalaceDominoLevelFloorTile.TriggerSpikes(False)
				End If
			End If
		Next
		Yield Nothing
		Return
	End Function

	' Token: 0x0400256C RID: 9580
	<Header("Floor")>
	<SerializeField()>
	Private _floors As DicePalaceDominoLevelScrollingFloor()

	' Token: 0x0400256D RID: 9581
	<SerializeField()>
	Private _teethSprite As ScrollingSprite

	' Token: 0x0400256E RID: 9582
	Private spikesColour As DicePalaceDominoLevelBouncyBall.Colour = DicePalaceDominoLevelBouncyBall.Colour.none

	' Token: 0x0400256F RID: 9583
	Public OnToggleFlashEvent As Action

	' Token: 0x04002570 RID: 9584
	Public OnColourChangeEvent As Action

	' Token: 0x04002571 RID: 9585
	Private properties As LevelProperties.DicePalaceDomino

	' Token: 0x04002572 RID: 9586
	Private tiles As List(Of DicePalaceDominoLevelFloorTile)

	' Token: 0x04002573 RID: 9587
	Private preTiles As List(Of DicePalaceDominoLevelFloorTile)

	' Token: 0x04002574 RID: 9588
	Private levelForce As LevelPlayerMotor.VelocityManager.Force
End Class
