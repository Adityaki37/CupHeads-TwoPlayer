Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000758 RID: 1880
Public Class RetroArcadeSnakeBodyPart
	Inherits AbstractCollidableObject

	' Token: 0x170003E2 RID: 994
	' (get) Token: 0x060028F6 RID: 10486 RVA: 0x0017DE73 File Offset: 0x0017C273
	' (set) Token: 0x060028F7 RID: 10487 RVA: 0x0017DE7B File Offset: 0x0017C27B
	Public Property currentDirection As RetroArcadeSnakeBodyPart.Direction

	' Token: 0x060028F8 RID: 10488 RVA: 0x0017DE84 File Offset: 0x0017C284
	Public Function Create(pos As Vector2, isHead As Boolean, direction As RetroArcadeSnakeBodyPart.Direction, manager As RetroArcadeSnakeManager, previousPart As RetroArcadeSnakeBodyPart, speed As Single) As RetroArcadeSnakeBodyPart
		Dim retroArcadeSnakeBodyPart As RetroArcadeSnakeBodyPart = Me.InstantiatePrefab(Of RetroArcadeSnakeBodyPart)()
		retroArcadeSnakeBodyPart.transform.position = pos
		retroArcadeSnakeBodyPart.currentDirection = direction
		retroArcadeSnakeBodyPart.partInFront = previousPart
		retroArcadeSnakeBodyPart.speed = speed
		retroArcadeSnakeBodyPart.isHead = isHead
		retroArcadeSnakeBodyPart.manager = manager
		Return retroArcadeSnakeBodyPart
	End Function

	' Token: 0x060028F9 RID: 10489 RVA: 0x0017DED0 File Offset: 0x0017C2D0
	Private Sub Start()
		Me.canTurn = True
		Me.ChangeDirection(Me.currentDirection, False)
		If Me.isHead Then
			MyBase.StartCoroutine(Me.head_check_cr())
		Else
			MyBase.StartCoroutine(Me.body_check_cr())
		End If
	End Sub

	' Token: 0x060028FA RID: 10490 RVA: 0x0017DF10 File Offset: 0x0017C310
	Private Sub FixedUpdate()
		If Not Me.isDead Then
			MyBase.transform.position += Me.dir * Me.speed * CupheadTime.FixedDelta
		End If
	End Sub

	' Token: 0x060028FB RID: 10491 RVA: 0x0017DF50 File Offset: 0x0017C350
	Private Iterator Function body_check_cr() As IEnumerator
		While True
			If Me.currentDirection <> Me.partInFront.currentDirection Then
				If Me.currentDirection = RetroArcadeSnakeBodyPart.Direction.Right Then
					If MyBase.transform.position.x >= Me.partInFront.turnPos.x Then
						Me.ClampDirectionChange()
					End If
				ElseIf Me.currentDirection = RetroArcadeSnakeBodyPart.Direction.Left Then
					If MyBase.transform.position.x <= Me.partInFront.turnPos.x Then
						Me.ClampDirectionChange()
					End If
				ElseIf Me.currentDirection = RetroArcadeSnakeBodyPart.Direction.Up Then
					If MyBase.transform.position.y >= Me.partInFront.turnPos.y Then
						Me.ClampDirectionChange()
					End If
				ElseIf Me.currentDirection = RetroArcadeSnakeBodyPart.Direction.Down AndAlso MyBase.transform.position.y <= Me.partInFront.turnPos.y Then
					Me.ClampDirectionChange()
				End If
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060028FC RID: 10492 RVA: 0x0017DF6C File Offset: 0x0017C36C
	Private Sub ClampDirectionChange()
		MyBase.transform.position = Me.partInFront.transform.position + -Me.partInFront.dir * 60F
		Me.ChangeDirection(Me.partInFront.currentDirection, False)
	End Sub

	' Token: 0x060028FD RID: 10493 RVA: 0x0017DFC8 File Offset: 0x0017C3C8
	Private Iterator Function head_check_cr() As IEnumerator
		Dim player As AbstractPlayerController = PlayerManager.GetPlayer(PlayerId.PlayerOne)
		Dim player2 As AbstractPlayerController = PlayerManager.GetPlayer(PlayerId.PlayerTwo)
		While True
			If Me.currentDirection = RetroArcadeSnakeBodyPart.Direction.Up OrElse Me.currentDirection = RetroArcadeSnakeBodyPart.Direction.Down Then
				If MyBase.transform.position.y >= 230F OrElse MyBase.transform.position.y <= -120F Then
					Me.SwitchToHorizontal()
				Else
					If player IsNot Nothing AndAlso Not player.IsDead Then
						Me.CheckPlayerY(player)
					End If
					If player2 IsNot Nothing AndAlso Not player2.IsDead Then
						Me.CheckPlayerY(player2)
					End If
				End If
			ElseIf MyBase.transform.position.x >= 330F OrElse MyBase.transform.position.x <= -330F Then
				Me.SwitchToVertical()
			Else
				If player IsNot Nothing AndAlso Not player.IsDead Then
					Me.CheckPlayerX(player)
				End If
				If player2 IsNot Nothing AndAlso Not player2.IsDead Then
					Me.CheckPlayerX(player2)
				End If
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060028FE RID: 10494 RVA: 0x0017DFE4 File Offset: 0x0017C3E4
	Private Sub CheckPlayerX(player As AbstractPlayerController)
		Dim num As Single = player.transform.position.x - MyBase.transform.position.x
		If Mathf.Abs(num) < 20F Then
			If player.transform.position.y < MyBase.transform.position.y Then
				If MyBase.transform.position.y <= -100F Then
					Return
				End If
				Me.ChangeDirection(RetroArcadeSnakeBodyPart.Direction.Down, True)
			Else
				If MyBase.transform.position.y >= 210F Then
					Return
				End If
				Me.ChangeDirection(RetroArcadeSnakeBodyPart.Direction.Up, True)
			End If
		End If
	End Sub

	' Token: 0x060028FF RID: 10495 RVA: 0x0017E0A8 File Offset: 0x0017C4A8
	Private Sub CheckPlayerY(player As AbstractPlayerController)
		Dim num As Single = player.transform.position.y - MyBase.transform.position.y
		If Mathf.Abs(num) < 20F Then
			If player.transform.position.x < MyBase.transform.position.x Then
				If player.transform.position.x <= -310F Then
					Return
				End If
				Me.ChangeDirection(RetroArcadeSnakeBodyPart.Direction.Left, True)
			Else
				If player.transform.position.x >= 310F Then
					Return
				End If
				Me.ChangeDirection(RetroArcadeSnakeBodyPart.Direction.Right, True)
			End If
		End If
	End Sub

	' Token: 0x06002900 RID: 10496 RVA: 0x0017E16C File Offset: 0x0017C56C
	Private Sub SwitchToHorizontal()
		If MyBase.transform.position.x < 0F Then
			Me.ChangeDirection(RetroArcadeSnakeBodyPart.Direction.Right, True)
		Else
			Me.ChangeDirection(RetroArcadeSnakeBodyPart.Direction.Left, True)
		End If
	End Sub

	' Token: 0x06002901 RID: 10497 RVA: 0x0017E1AC File Offset: 0x0017C5AC
	Private Sub SwitchToVertical()
		If MyBase.transform.position.y < 0F Then
			Me.ChangeDirection(RetroArcadeSnakeBodyPart.Direction.Up, True)
		Else
			Me.ChangeDirection(RetroArcadeSnakeBodyPart.Direction.Down, True)
		End If
	End Sub

	' Token: 0x06002902 RID: 10498 RVA: 0x0017E1EC File Offset: 0x0017C5EC
	Private Sub ChangeDirection(direction As RetroArcadeSnakeBodyPart.Direction, checkTurn As Boolean)
		If checkTurn AndAlso Not Me.canTurn Then
			Return
		End If
		Me.currentDirection = direction
		Me.turnPos = MyBase.transform.position
		Select Case Me.currentDirection
			Case RetroArcadeSnakeBodyPart.Direction.Left
				Me.dir = Vector3.left
			Case RetroArcadeSnakeBodyPart.Direction.Right
				Me.dir = Vector3.right
			Case RetroArcadeSnakeBodyPart.Direction.Up
				Me.dir = Vector3.up
			Case RetroArcadeSnakeBodyPart.Direction.Down
				Me.dir = Vector3.down
		End Select
		MyBase.StartCoroutine(Me.turn_timer_cr())
	End Sub

	' Token: 0x06002903 RID: 10499 RVA: 0x0017E294 File Offset: 0x0017C694
	Private Iterator Function turn_timer_cr() As IEnumerator
		Dim t As Single = 0F
		Dim time As Single = 0.5F
		Me.canTurn = False
		While t < time
			t += CupheadTime.Delta
			Yield Nothing
		End While
		Me.canTurn = True
		Yield Nothing
		Return
	End Function

	' Token: 0x06002904 RID: 10500 RVA: 0x0017E2B0 File Offset: 0x0017C6B0
	Protected Overrides Sub OnCollisionEnemy(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionEnemy(hit, phase)
		If Me.isHead AndAlso Not Me.isDead AndAlso hit.GetComponent(Of RetroArcadeSnakeBodyPart)() IsNot Me.partBehind Then
			Me.manager.EndPhase()
			Me.isDead = True
		End If
	End Sub

	' Token: 0x06002905 RID: 10501 RVA: 0x0017E303 File Offset: 0x0017C703
	Public Sub GetPartBehind(behind As RetroArcadeSnakeBodyPart)
		Me.partBehind = behind
	End Sub

	' Token: 0x06002906 RID: 10502 RVA: 0x0017E30C File Offset: 0x0017C70C
	Public Sub Die()
		Me.StopAllCoroutines()
		Me.isDead = True
	End Sub

	' Token: 0x040031DD RID: 12765
	Public turnPos As Vector3

	' Token: 0x040031DE RID: 12766
	Public dir As Vector3

	' Token: 0x040031DF RID: 12767
	Private Const TOP_Y As Single = 230F

	' Token: 0x040031E0 RID: 12768
	Private Const BOTTOM_Y As Single = -120F

	' Token: 0x040031E1 RID: 12769
	Private Const OFFSCREEN_Y As Single = 300F

	' Token: 0x040031E2 RID: 12770
	Private Const SIDE_X As Single = 330F

	' Token: 0x040031E3 RID: 12771
	Private Const MIN_DISTANCE As Single = 20F

	' Token: 0x040031E4 RID: 12772
	Private Const SPACING As Single = 60F

	' Token: 0x040031E6 RID: 12774
	Private partInFront As RetroArcadeSnakeBodyPart

	' Token: 0x040031E7 RID: 12775
	Private partBehind As RetroArcadeSnakeBodyPart

	' Token: 0x040031E8 RID: 12776
	Private manager As RetroArcadeSnakeManager

	' Token: 0x040031E9 RID: 12777
	Private speed As Single

	' Token: 0x040031EA RID: 12778
	Private canTurn As Boolean

	' Token: 0x040031EB RID: 12779
	Private isHead As Boolean

	' Token: 0x040031EC RID: 12780
	Private isDead As Boolean

	' Token: 0x02000759 RID: 1881
	Public Enum Direction
		' Token: 0x040031EE RID: 12782
		Left
		' Token: 0x040031EF RID: 12783
		Right
		' Token: 0x040031F0 RID: 12784
		Up
		' Token: 0x040031F1 RID: 12785
		Down
	End Enum
End Class
