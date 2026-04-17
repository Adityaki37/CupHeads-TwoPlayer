Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x02000744 RID: 1860
Public Class RetroArcadeChaserManager
	Inherits LevelProperties.RetroArcade.Entity

	' Token: 0x06002885 RID: 10373 RVA: 0x00179B71 File Offset: 0x00177F71
	Public Sub StartChasers()
		Me.SetupSpawnPoints()
		MyBase.StartCoroutine(Me.chasers_cr())
	End Sub

	' Token: 0x06002886 RID: 10374 RVA: 0x00179B88 File Offset: 0x00177F88
	Private Sub SetupSpawnPoints()
		Dim num As Integer = 8
		Dim num2 As Single = CSng((Level.Current.Right / (num - 1) * 2))
		Dim num3 As Single = 5F
		Dim num4 As Integer = 0
		Me.spawnPositions = New Vector3(num * 2 - 1) {}
		For i As Integer = 0 To num * 2 - 1
			Dim num5 As Single = CSng(Level.Current.Left) + num2 * CSng(num4)
			Dim num6 As Single = If((i >= num), (CSng(Level.Current.Ground) + num3), (CSng(Level.Current.Ceiling) - num3))
			Me.spawnPositions(i) = New Vector3(num5, num6)
			num4 = If((i <> num - 1), (num4 + 1), 0)
		Next
	End Sub

	' Token: 0x06002887 RID: 10375 RVA: 0x00179C44 File Offset: 0x00178044
	Private Iterator Function chasers_cr() As IEnumerator
		Dim p As LevelProperties.RetroArcade.Chasers = MyBase.properties.CurrentState.chasers
		Dim mainColorIndex As Integer = Global.UnityEngine.Random.Range(0, p.colorString.Length)
		Dim colorString As String() = p.colorString(mainColorIndex).Split(New Char() { ","c })
		Dim mainDelayIndex As Integer = Global.UnityEngine.Random.Range(0, p.delayString.Length)
		Dim delayString As String() = p.delayString(mainDelayIndex).Split(New Char() { ","c })
		Dim delayIndex As Integer = Global.UnityEngine.Random.Range(0, delayString.Length)
		Dim mainOrderIndex As Integer = Global.UnityEngine.Random.Range(0, p.orderString.Length)
		Dim orderString As String() = p.orderString(mainOrderIndex).Split(New Char() { ","c })
		Dim orderIndex As Integer = Global.UnityEngine.Random.Range(0, orderString.Length)
		Dim chaserSelected As RetroArcadeChaser = Nothing
		Dim spawnIndex As Integer = 0
		Dim delay As Single = 0F
		Dim chaserSpeed As Single = 0F
		Dim chaserrotation As Single = 0F
		Dim chaserHp As Single = 0F
		Dim chaserDuration As Single = 0F
		Me.chasers = New List(Of RetroArcadeChaser)()
		For i As Integer = 0 To colorString.Length - 1
			Dim player As AbstractPlayerController = PlayerManager.GetNext()
			orderString = p.orderString(mainOrderIndex).Split(New Char() { ","c })
			delayString = p.delayString(mainDelayIndex).Split(New Char() { ","c })
			If colorString(i)(0) = "G"c Then
				chaserSelected = Me.chaserGreenPrefab
				chaserSpeed = p.greenSpeed
				chaserrotation = p.greenRotation
				chaserHp = p.greenHP
				chaserDuration = p.greenDuration
			ElseIf colorString(i)(0) = "Y"c Then
				chaserSelected = Me.chaserYellowPrefab
				chaserSpeed = p.yellowSpeed
				chaserrotation = p.yellowRotation
				chaserHp = p.yellowHP
				chaserDuration = p.yellowDuration
			ElseIf colorString(i)(0) = "O"c Then
				chaserSelected = Me.chaserOrangePrefab
				chaserSpeed = p.orangeSpeed
				chaserrotation = p.orangeRotation
				chaserHp = p.orangeHP
				chaserDuration = p.orangeDuration
			End If
			Parser.IntTryParse(orderString(orderIndex), spawnIndex)
			Parser.FloatTryParse(delayString(delayIndex), delay)
			Dim chaser As RetroArcadeChaser = chaserSelected.Spawn()
			chaser.Init(Me.spawnPositions(spawnIndex), 0F, chaserSpeed, chaserSpeed, chaserrotation, chaserDuration, chaserHp, player, p)
			Me.chasers.Add(chaser)
			If orderIndex < p.orderString.Length - 1 Then
				orderIndex += 1
			Else
				mainOrderIndex = (mainOrderIndex + 1) Mod p.orderString.Length
				orderIndex = 0
			End If
			If delayIndex < p.delayString.Length - 1 Then
				delayIndex += 1
			Else
				mainDelayIndex = (mainDelayIndex + 1) Mod p.delayString.Length
				delayIndex = 0
			End If
			Yield CupheadTime.WaitForSeconds(Me, delay)
		Next
		Dim isDone As Boolean = False
		While Not isDone
			isDone = True
			For Each retroArcadeChaser As RetroArcadeChaser In Me.chasers
				isDone = retroArcadeChaser.IsDone
			Next
			Yield Nothing
		End While
		For Each retroArcadeChaser2 As RetroArcadeChaser In Me.chasers
			Global.UnityEngine.[Object].Destroy(retroArcadeChaser2)
		Next
		MyBase.properties.DealDamageToNextNamedState()
		Yield Nothing
		Return
	End Function

	' Token: 0x0400315A RID: 12634
	<SerializeField()>
	Private chaserGreenPrefab As RetroArcadeChaser

	' Token: 0x0400315B RID: 12635
	<SerializeField()>
	Private chaserYellowPrefab As RetroArcadeChaser

	' Token: 0x0400315C RID: 12636
	<SerializeField()>
	Private chaserOrangePrefab As RetroArcadeChaser

	' Token: 0x0400315D RID: 12637
	Private spawnPositions As Vector3()

	' Token: 0x0400315E RID: 12638
	Private chasers As List(Of RetroArcadeChaser)
End Class
