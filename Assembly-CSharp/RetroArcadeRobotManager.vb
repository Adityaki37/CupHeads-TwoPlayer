Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000754 RID: 1876
Public Class RetroArcadeRobotManager
	Inherits LevelProperties.RetroArcade.Entity

	' Token: 0x060028E3 RID: 10467 RVA: 0x0017CB18 File Offset: 0x0017AF18
	Public Sub StartRobots()
		Me.p = MyBase.properties.CurrentState.robots
		Me.phase = 0
		MyBase.StartCoroutine(Me.bonus_cr())
		Me.StartNewPhase()
	End Sub

	' Token: 0x060028E4 RID: 10468 RVA: 0x0017CB4C File Offset: 0x0017AF4C
	Public Sub StartNewPhase()
		Me.numDied = 0
		Dim array As String() = Me.p.robotWaves(Me.phase).Split(New Char() { ","c })
		Me.numRobotsToKill = array.Length
		For i As Integer = 0 To Me.numRobotsToKill - 1
			Dim num As Integer
			Parser.IntTryParse(array(i), num)
			If num > 0 AndAlso num <= Me.p.robotsXPositions.Length Then
				Dim array2 As String() = Me.p.robotColorPattern(Me.phase).Split(New Char() { ","c })
				Dim array3 As String() = array2(i).Split(New Char() { "-"c })
				Dim num2 As Single = Me.p.robotsXPositions(num - 1)
				Me.bigRobotPrefab.Create(num2, Me.p, CSng(i) / 3F, Me, array3)
			End If
		Next
	End Sub

	' Token: 0x060028E5 RID: 10469 RVA: 0x0017CC30 File Offset: 0x0017B030
	Private Iterator Function bonus_cr() As IEnumerator
		For i As Integer = 0 To Me.p.bonusCount - 1
			Yield CupheadTime.WaitForSeconds(Me, Me.p.bonusDelay.RandomFloat())
			Me.bonusRobotPrefab.Create(If((Not Rand.Bool()), RetroArcadeBonusRobot.Direction.Right, RetroArcadeBonusRobot.Direction.Left), Me.p)
		Next
		Return
	End Function

	' Token: 0x060028E6 RID: 10470 RVA: 0x0017CC4C File Offset: 0x0017B04C
	Public Sub OnRobotGroupDie()
		Me.numDied += 1
		If Me.numDied >= Me.numRobotsToKill Then
			If Me.phase >= Me.p.robotWaves.Length - 1 Then
				MyBase.properties.DealDamageToNextNamedState()
				Me.StopAllCoroutines()
			Else
				Me.phase += 1
				Me.StartNewPhase()
			End If
		End If
	End Sub

	' Token: 0x040031BE RID: 12734
	Private Const BIG_ROBOT_SPACING As Single = 160F

	' Token: 0x040031BF RID: 12735
	<SerializeField()>
	Private bigRobotPrefab As RetroArcadeBigRobot

	' Token: 0x040031C0 RID: 12736
	<SerializeField()>
	Private bonusRobotPrefab As RetroArcadeBonusRobot

	' Token: 0x040031C1 RID: 12737
	Private p As LevelProperties.RetroArcade.Robots

	' Token: 0x040031C2 RID: 12738
	Private numDied As Integer

	' Token: 0x040031C3 RID: 12739
	Private phase As Integer

	' Token: 0x040031C4 RID: 12740
	Private numRobotsToKill As Integer
End Class
