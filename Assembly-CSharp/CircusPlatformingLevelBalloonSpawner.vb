Imports System
Imports UnityEngine

' Token: 0x0200089B RID: 2203
Public Class CircusPlatformingLevelBalloonSpawner
	Inherits PlatformingLevelEnemySpawner

	' Token: 0x06003349 RID: 13129 RVA: 0x001DD97C File Offset: 0x001DBD7C
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.delayIndex = Global.UnityEngine.Random.Range(0, Me.spawnDelayString.Split(New Char() { ","c }).Length)
		Me.posIndex = Global.UnityEngine.Random.Range(0, Me.spawnPositionString.Split(New Char() { ","c }).Length)
		Me.sideIndex = Global.UnityEngine.Random.Range(0, Me.spawnSideString.Split(New Char() { ","c }).Length)
		Me.pinkSplits = Me.pinkString.Split(New Char() { ","c })
		Me.pinkIndex = Global.UnityEngine.Random.Range(0, Me.pinkSplits.Length)
	End Sub

	' Token: 0x0600334A RID: 13130 RVA: 0x001DDA2C File Offset: 0x001DBE2C
	Protected Overrides Sub Spawn()
		MyBase.Spawn()
		Me.spawnDelay.min = Parser.FloatParse(Me.spawnDelayString.Split(New Char() { ","c })(Me.delayIndex))
		Me.spawnDelay.max = Parser.FloatParse(Me.spawnDelayString.Split(New Char() { ","c })(Me.delayIndex))
		If Me.spawnSideString.Split(New Char() { ","c })(Me.sideIndex)(0) = "L"c Then
			Me.spawnPosition.x = CupheadLevelCamera.Current.Bounds.xMin - 50F
			Me.rotation = 0F
		ElseIf Me.spawnSideString.Split(New Char() { ","c })(Me.sideIndex)(0) = "R"c Then
			Me.spawnPosition.x = CupheadLevelCamera.Current.Bounds.xMax + 50F
			Me.rotation = 180F
		End If
		Me.spawnPosition.y = CupheadLevelCamera.Current.transform.position.y + Parser.FloatParse(Me.spawnPositionString.Split(New Char() { ","c })(Me.posIndex))
		Dim circusPlatformingLevelBalloon As CircusPlatformingLevelBalloon = Global.UnityEngine.[Object].Instantiate(Of CircusPlatformingLevelBalloon)(Me.balloonPrefab)
		circusPlatformingLevelBalloon.Init(Me.spawnPosition, Me.rotation, Me.spreadCount, Me.pinkSplits(Me.pinkIndex))
		Me.sideIndex = (Me.sideIndex + 1) Mod Me.spawnSideString.Split(New Char() { ","c }).Length
		Me.posIndex = (Me.posIndex + 1) Mod Me.spawnPositionString.Split(New Char() { ","c }).Length
		Me.delayIndex = (Me.delayIndex + 1) Mod Me.spawnDelayString.Split(New Char() { ","c }).Length
		Me.pinkIndex = (Me.pinkIndex + 1) Mod Me.pinkSplits.Length
	End Sub

	' Token: 0x04003B8B RID: 15243
	<SerializeField()>
	Private balloonPrefab As CircusPlatformingLevelBalloon

	' Token: 0x04003B8C RID: 15244
	<SerializeField()>
	Private spawnDelayString As String

	' Token: 0x04003B8D RID: 15245
	<SerializeField()>
	Private spawnPositionString As String

	' Token: 0x04003B8E RID: 15246
	<SerializeField()>
	Private spawnSideString As String

	' Token: 0x04003B8F RID: 15247
	<SerializeField()>
	Private spreadCount As String

	' Token: 0x04003B90 RID: 15248
	<SerializeField()>
	Private pinkString As String

	' Token: 0x04003B91 RID: 15249
	Private rotation As Single

	' Token: 0x04003B92 RID: 15250
	Private delayIndex As Integer

	' Token: 0x04003B93 RID: 15251
	Private posIndex As Integer

	' Token: 0x04003B94 RID: 15252
	Private sideIndex As Integer

	' Token: 0x04003B95 RID: 15253
	Private spawnPosition As Vector3

	' Token: 0x04003B96 RID: 15254
	Private pinkSplits As String()

	' Token: 0x04003B97 RID: 15255
	Private pinkIndex As Integer
End Class
