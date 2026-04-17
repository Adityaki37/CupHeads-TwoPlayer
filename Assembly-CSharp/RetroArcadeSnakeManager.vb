Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200075A RID: 1882
Public Class RetroArcadeSnakeManager
	Inherits LevelProperties.RetroArcade.Entity

	' Token: 0x06002908 RID: 10504 RVA: 0x0017E848 File Offset: 0x0017CC48
	Public Sub StartSnake()
		MyBase.StartCoroutine(Me.spawn_snake_cr())
	End Sub

	' Token: 0x06002909 RID: 10505 RVA: 0x0017E858 File Offset: 0x0017CC58
	Private Iterator Function spawn_snake_cr() As IEnumerator
		Dim player As AbstractPlayerController = PlayerManager.GetNext()
		Me.snakeFull = New RetroArcadeSnakeBodyPart(7) {}
		Dim direction As RetroArcadeSnakeBodyPart.Direction = RetroArcadeSnakeBodyPart.Direction.Down
		Dim startPos As Vector3 = New Vector3(-player.transform.position.x, 200F)
		Me.snakeFull(0) = Me.bodyPrefab.Create(New Vector2(startPos.x, startPos.y), True, direction, Me, Me.snakeFull(0), MyBase.properties.CurrentState.snake.moveSpeed)
		For i As Integer = 1 To 8 - 1
			Me.snakeFull(i) = Me.bodyPrefab.Create(New Vector2(startPos.x, startPos.y + CSng(i) * 60F), i = 0, direction, Me, If((i <> 0), Me.snakeFull(i - 1), Me.snakeFull(i)), MyBase.properties.CurrentState.snake.moveSpeed)
		Next
		Me.snakeFull(0).GetPartBehind(Me.snakeFull(1))
		Yield Nothing
		Return
	End Function

	' Token: 0x0600290A RID: 10506 RVA: 0x0017E873 File Offset: 0x0017CC73
	Public Sub EndPhase()
		MyBase.StartCoroutine(Me.end_phase_cr())
	End Sub

	' Token: 0x0600290B RID: 10507 RVA: 0x0017E884 File Offset: 0x0017CC84
	Private Iterator Function end_phase_cr() As IEnumerator
		For j As Integer = 0 To Me.snakeFull.Length - 1
			Me.snakeFull(j).Die()
		Next
		Yield CupheadTime.WaitForSeconds(Me, 0.1F)
		For i As Integer = Me.snakeFull.Length - 1 To 0 Step -1
			Global.UnityEngine.[Object].Destroy(Me.snakeFull(i).gameObject)
			Yield CupheadTime.WaitForSeconds(Me, 0.1F)
		Next
		MyBase.properties.DealDamageToNextNamedState()
		Yield Nothing
		Return
	End Function

	' Token: 0x040031F2 RID: 12786
	<SerializeField()>
	Private bodyPrefab As RetroArcadeSnakeBodyPart

	' Token: 0x040031F3 RID: 12787
	Private snakeFull As RetroArcadeSnakeBodyPart()

	' Token: 0x040031F4 RID: 12788
	Private Const BODYPARTS As Integer = 8

	' Token: 0x040031F5 RID: 12789
	Private Const SPACING As Single = 60F

	' Token: 0x040031F6 RID: 12790
	Private Const OFFSCREEN_Y As Single = 300F
End Class
