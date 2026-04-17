Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200052E RID: 1326
Public Class ChessBOldBLevelGameManager
	Inherits AbstractPausableComponent

	' Token: 0x17000330 RID: 816
	' (get) Token: 0x060017FE RID: 6142 RVA: 0x000D8C87 File Offset: 0x000D7087
	' (set) Token: 0x060017FF RID: 6143 RVA: 0x000D8C8F File Offset: 0x000D708F
	Public Property WaitingForParry As Boolean

	' Token: 0x06001800 RID: 6144 RVA: 0x000D8C98 File Offset: 0x000D7098
	Public Sub SetupGameManager(properties As LevelProperties.ChessBOldB)
		Me.properties = properties
		Me.goingClockwise = Rand.Bool()
		Me.InitBalls()
	End Sub

	' Token: 0x06001801 RID: 6145 RVA: 0x000D8CB4 File Offset: 0x000D70B4
	Private Sub InitBalls()
		Me.birdies = New ChessBOldBReduxLevelBirdie(5) {}
		Dim vector As Vector3 = New Vector3(0F, 1000F)
		For i As Integer = 0 To 6 - 1
			Me.birdies(i) = Global.UnityEngine.[Object].Instantiate(Of ChessBOldBReduxLevelBirdie)(Me.birdiePrefab)
			Me.birdies(i).transform.position = vector
			Me.birdies(i).ParryBirdie = AddressOf Me.ParriedBall
		Next
		MyBase.StartCoroutine(Me.game_cr())
	End Sub

	' Token: 0x06001802 RID: 6146 RVA: 0x000D8D3C File Offset: 0x000D713C
	Private Iterator Function game_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 4F)
		Dim p As LevelProperties.ChessBOldB.Birdie = Me.properties.CurrentState.birdie
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		Me.OnStateChanged()
		Dim t As Single = 0F
		Dim spinTime As Single = 0F
		Dim speedTime As Single = 0F
		Dim timesToSwitchDir As Integer = 0
		While True
			t = 0F
			Yield MyBase.StartCoroutine(Me.flash_balls_cr())
			Me.WaitingForParry = True
			p = Me.properties.CurrentState.birdie
			Me.spinSpeedString = p.spinSpeedString(Me.spinSpeedStringMainIndex).Split(New Char() { ","c })
			Me.spinTimeString = p.spinTimeString(Me.spinTimeStringMainIndex).Split(New Char() { ","c })
			Me.changeDirString = p.changeDirectionString(Me.changeDirStringMainIndex).Split(New Char() { ","c })
			Me.initialDirString = p.initialDirectionString(Me.initialDirStringMainIndex).Split(New Char() { ","c })
			Parser.FloatTryParse(Me.spinTimeString(Me.spinTimeStringIndex), spinTime)
			Parser.FloatTryParse(Me.spinSpeedString(Me.spinSpeedStringIndex), speedTime)
			Parser.IntTryParse(Me.changeDirString(Me.changeDirStringIndex), timesToSwitchDir)
			Me.goingClockwise = Me.initialDirString(Me.initialDirStringIndex)(0) = "R"c
			For i As Integer = 0 To Me.birdies.Length - 1
				Me.birdies(i).HandleMovement(speedTime, Me.goingClockwise)
			Next
			Dim timeUntilSwitch As Single = spinTime / CSng(timesToSwitchDir)
			Dim dirCounter As Integer = 0
			Dim turnedPink As Boolean = False
			While t < spinTime
				If Not Me.WaitingForParry Then
					Exit While
				End If
				t += CupheadTime.FixedDelta
				If Not turnedPink AndAlso t >= p.prePinkTime Then
					For j As Integer = 0 To Me.birdies.Length - 1
						Me.birdies(j).TurnPink()
					Next
					turnedPink = True
				End If
				If dirCounter < timesToSwitchDir AndAlso t > timeUntilSwitch * CSng(dirCounter) Then
					dirCounter += 1
					Me.goingClockwise = Not Me.goingClockwise
					For k As Integer = 0 To Me.birdies.Length - 1
						Me.birdies(k).HandleMovement(speedTime, Me.goingClockwise)
					Next
				End If
				Yield wait
			End While
			For l As Integer = 0 To Me.birdies.Length - 1
				Me.birdies(l).StopMoving()
			Next
			While Me.WaitingForParry
				Yield Nothing
			End While
			If Me.spinSpeedStringIndex < Me.spinSpeedString.Length - 1 Then
				Me.spinSpeedStringIndex += 1
			Else
				Me.spinSpeedStringMainIndex = (Me.spinSpeedStringMainIndex + 1) Mod p.spinSpeedString.Length
				Me.spinSpeedStringIndex = 0
			End If
			If Me.spinTimeStringIndex < Me.spinTimeString.Length - 1 Then
				Me.spinTimeStringIndex += 1
			Else
				Me.spinTimeStringMainIndex = (Me.spinTimeStringMainIndex + 1) Mod p.spinTimeString.Length
				Me.spinTimeStringIndex = 0
			End If
			If Me.changeDirStringIndex < Me.changeDirString.Length - 1 Then
				Me.changeDirStringIndex += 1
			Else
				Me.changeDirStringMainIndex = (Me.changeDirStringMainIndex + 1) Mod p.changeDirectionString.Length
				Me.changeDirStringIndex = 0
			End If
			If Me.initialDirStringIndex < Me.initialDirString.Length - 1 Then
				Me.initialDirStringIndex += 1
			Else
				Me.initialDirStringMainIndex = (Me.initialDirStringMainIndex + 1) Mod p.initialDirectionString.Length
				Me.initialDirStringIndex = 0
			End If
			Yield wait
		End While
		Return
	End Function

	' Token: 0x06001803 RID: 6147 RVA: 0x000D8D57 File Offset: 0x000D7157
	Private Sub ParriedBall(chosenBall As Boolean)
		If chosenBall Then
			Me.WaitingForParry = False
			Me.properties.DealDamage(1F)
			Me.boss.HandleHurt(True)
		End If
	End Sub

	' Token: 0x06001804 RID: 6148 RVA: 0x000D8D84 File Offset: 0x000D7184
	Public Sub OnStateChanged()
		Dim birdie As LevelProperties.ChessBOldB.Birdie = Me.properties.CurrentState.birdie
		Me.spinSpeedStringMainIndex = Global.UnityEngine.Random.Range(0, birdie.spinSpeedString.Length)
		Me.spinSpeedString = birdie.spinSpeedString(Me.spinSpeedStringMainIndex).Split(New Char() { ","c })
		Me.spinSpeedStringIndex = Global.UnityEngine.Random.Range(0, Me.spinSpeedString.Length)
		Me.spinTimeStringMainIndex = Global.UnityEngine.Random.Range(0, birdie.spinTimeString.Length)
		Me.spinTimeString = birdie.spinTimeString(Me.spinTimeStringMainIndex).Split(New Char() { ","c })
		Me.spinTimeStringIndex = Global.UnityEngine.Random.Range(0, Me.spinTimeString.Length)
		Me.changeDirStringMainIndex = Global.UnityEngine.Random.Range(0, birdie.changeDirectionString.Length)
		Me.changeDirString = birdie.changeDirectionString(Me.changeDirStringMainIndex).Split(New Char() { ","c })
		Me.changeDirStringIndex = Global.UnityEngine.Random.Range(0, Me.changeDirString.Length)
		Me.initialDirStringMainIndex = Global.UnityEngine.Random.Range(0, birdie.initialDirectionString.Length)
		Me.initialDirString = birdie.initialDirectionString(Me.initialDirStringMainIndex).Split(New Char() { ","c })
		Me.initialDirStringIndex = Global.UnityEngine.Random.Range(0, Me.initialDirString.Length)
		Me.chosenStringMainIndex = Global.UnityEngine.Random.Range(0, birdie.chosenString.Length)
		Me.chosenString = birdie.chosenString(Me.chosenStringMainIndex).Split(New Char() { ","c })
		Me.chosenStringIndex = Global.UnityEngine.Random.Range(0, Me.chosenString.Length)
	End Sub

	' Token: 0x06001805 RID: 6149 RVA: 0x000D8F1C File Offset: 0x000D731C
	Private Iterator Function flash_balls_cr() As IEnumerator
		Dim p As LevelProperties.ChessBOldB.Birdie = Me.properties.CurrentState.birdie
		For i As Integer = 0 To Me.birdies.Length - 1
			Me.birdies(i).transform.position = New Vector3(0F, 1000F)
		Next
		Yield CupheadTime.WaitForSeconds(Me, p.fadeInTime)
		Me.boss.HandleHurt(False)
		Dim angleOffset As Single = 60F
		Dim chosenIndex As Integer = 0
		Me.chosenString = p.chosenString(Me.chosenStringMainIndex).Split(New Char() { ","c })
		Parser.IntTryParse(Me.chosenString(Me.chosenStringIndex), chosenIndex)
		For j As Integer = 0 To Me.birdies.Length - 1
			Dim flag As Boolean = j = chosenIndex
			Me.birdies(j).Setup(Me.pivotPoint, angleOffset * CSng(j), Me.properties.CurrentState.birdie, 370F, flag)
		Next
		Dim color As Color = Me.birdies(chosenIndex).GetComponent(Of SpriteRenderer)().color
		Me.birdies(chosenIndex).GetComponent(Of SpriteRenderer)().color = Me.redFlash
		Yield CupheadTime.WaitForSeconds(Me, p.flashTime)
		Me.birdies(chosenIndex).GetComponent(Of SpriteRenderer)().color = color
		If Me.chosenStringIndex < Me.chosenString.Length - 1 Then
			Me.chosenStringIndex += 1
		Else
			Me.chosenStringMainIndex = (Me.chosenStringMainIndex + 1) Mod p.chosenString.Length
			Me.chosenStringIndex = 0
		End If
		Yield Nothing
		Return
	End Function

	' Token: 0x04002126 RID: 8486
	Private Const NUM_OF_BALLS As Integer = 6

	' Token: 0x04002127 RID: 8487
	Private Const LOOP_SIZE As Single = 370F

	' Token: 0x04002128 RID: 8488
	<SerializeField()>
	Private redFlash As Color

	' Token: 0x04002129 RID: 8489
	<SerializeField()>
	Private pivotPoint As Transform

	' Token: 0x0400212A RID: 8490
	<SerializeField()>
	Private boss As ChessBOldBLevelBoss

	' Token: 0x0400212B RID: 8491
	<SerializeField()>
	Private birdiePrefab As ChessBOldBReduxLevelBirdie

	' Token: 0x0400212C RID: 8492
	Private birdies As ChessBOldBReduxLevelBirdie()

	' Token: 0x0400212D RID: 8493
	Private properties As LevelProperties.ChessBOldB

	' Token: 0x0400212F RID: 8495
	Private goingClockwise As Boolean

	' Token: 0x04002130 RID: 8496
	Private spinSpeedStringMainIndex As Integer

	' Token: 0x04002131 RID: 8497
	Private spinSpeedString As String()

	' Token: 0x04002132 RID: 8498
	Private spinSpeedStringIndex As Integer

	' Token: 0x04002133 RID: 8499
	Private spinTimeStringMainIndex As Integer

	' Token: 0x04002134 RID: 8500
	Private spinTimeString As String()

	' Token: 0x04002135 RID: 8501
	Private spinTimeStringIndex As Integer

	' Token: 0x04002136 RID: 8502
	Private changeDirStringMainIndex As Integer

	' Token: 0x04002137 RID: 8503
	Private changeDirString As String()

	' Token: 0x04002138 RID: 8504
	Private changeDirStringIndex As Integer

	' Token: 0x04002139 RID: 8505
	Private initialDirStringMainIndex As Integer

	' Token: 0x0400213A RID: 8506
	Private initialDirString As String()

	' Token: 0x0400213B RID: 8507
	Private initialDirStringIndex As Integer

	' Token: 0x0400213C RID: 8508
	Private chosenStringMainIndex As Integer

	' Token: 0x0400213D RID: 8509
	Private chosenString As String()

	' Token: 0x0400213E RID: 8510
	Private chosenStringIndex As Integer
End Class
