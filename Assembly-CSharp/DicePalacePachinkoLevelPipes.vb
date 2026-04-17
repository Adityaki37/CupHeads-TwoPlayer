Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020005DB RID: 1499
Public Class DicePalacePachinkoLevelPipes
	Inherits LevelProperties.DicePalacePachinko.Entity

	' Token: 0x06001DA0 RID: 7584 RVA: 0x0011040C File Offset: 0x0010E80C
	Public Overrides Sub LevelInit(properties As LevelProperties.DicePalacePachinko)
		MyBase.LevelInit(properties)
		Me.spawnPointIndex = Global.UnityEngine.Random.Range(0, properties.CurrentState.balls.spawnOrderString.Split(New Char() { ","c }).Length)
		Me.spawnDelayIndex = Global.UnityEngine.Random.Range(0, properties.CurrentState.balls.ballDelayString.Split(New Char() { ","c }).Length)
		Me.pinkBallSpawnIndex = Global.UnityEngine.Random.Range(0, properties.CurrentState.balls.pinkString.Split(New Char() { ","c }).Length)
		Me.currentBallCount = 0
		AddHandler Level.Current.OnIntroEvent, AddressOf Me.StartAttack
	End Sub

	' Token: 0x06001DA1 RID: 7585 RVA: 0x001104C7 File Offset: 0x0010E8C7
	Private Sub StartAttack()
		MyBase.StartCoroutine(Me.attack_cr())
	End Sub

	' Token: 0x06001DA2 RID: 7586 RVA: 0x001104D8 File Offset: 0x0010E8D8
	Private Iterator Function attack_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.balls.initialAttackDelay)
		While True
			Yield CupheadTime.WaitForSeconds(Me, Parser.FloatParse(MyBase.properties.CurrentState.balls.ballDelayString.Split(New Char() { ","c })(Me.spawnDelayIndex)))
			Dim ball As AbstractProjectile = Me.ballPrefab.Create(Me.spawnPoints(Parser.IntParse(MyBase.properties.CurrentState.balls.spawnOrderString.Split(New Char() { ","c })(Me.spawnPointIndex)) - 1).position)
			ball.GetComponent(Of DicePalacePachinkoLevelPipeBall)().InitBall(MyBase.properties)
			If Me.currentBallCount < Parser.IntParse(MyBase.properties.CurrentState.balls.pinkString.Split(New Char() { ","c })(Me.pinkBallSpawnIndex)) Then
				Me.currentBallCount += 1
			Else
				ball.SetParryable(True)
				ball.GetComponentInChildren(Of SpriteRenderer)().color = Color.red
				Me.pinkBallSpawnIndex = Global.UnityEngine.Random.Range(0, MyBase.properties.CurrentState.balls.pinkString.Split(New Char() { ","c }).Length)
				Me.currentBallCount = 0
			End If
			Me.spawnPointIndex += 1
			If Me.spawnPointIndex >= MyBase.properties.CurrentState.balls.spawnOrderString.Split(New Char() { ","c }).Length Then
				Me.spawnPointIndex = 0
			End If
			Me.spawnDelayIndex += 1
			If Me.spawnDelayIndex >= MyBase.properties.CurrentState.balls.ballDelayString.Split(New Char() { ","c }).Length Then
				Me.spawnDelayIndex = 0
			End If
		End While
		Return
	End Function

	' Token: 0x06001DA3 RID: 7587 RVA: 0x001104F3 File Offset: 0x0010E8F3
	Protected Overrides Sub OnDestroy()
		Me.StopAllCoroutines()
		MyBase.OnDestroy()
	End Sub

	' Token: 0x0400267C RID: 9852
	<SerializeField()>
	Private spawnPoints As Transform()

	' Token: 0x0400267D RID: 9853
	<SerializeField()>
	Private ballPrefab As DicePalacePachinkoLevelPipeBall

	' Token: 0x0400267E RID: 9854
	Private spawnDelayIndex As Integer

	' Token: 0x0400267F RID: 9855
	Private spawnPointIndex As Integer

	' Token: 0x04002680 RID: 9856
	Private pinkBallSpawnIndex As Integer

	' Token: 0x04002681 RID: 9857
	Private currentBallCount As Integer
End Class
