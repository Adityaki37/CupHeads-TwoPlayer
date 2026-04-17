Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020008EE RID: 2286
Public Class MountainPlatformingLevelSatyrSpawner
	Inherits AbstractPausableComponent

	' Token: 0x060035A0 RID: 13728 RVA: 0x001F3BF4 File Offset: 0x001F1FF4
	Private Sub Start()
		MyBase.StartCoroutine(Me.spawn_cr())
		Me.directionIndex = Global.UnityEngine.Random.Range(0, Me.directionString.Split(New Char() { ","c }).Length)
		Me.spawnIndex = Global.UnityEngine.Random.Range(0, Me.spawnString.Split(New Char() { ","c }).Length)
	End Sub

	' Token: 0x060035A1 RID: 13729 RVA: 0x001F3C58 File Offset: 0x001F2058
	Private Iterator Function spawn_cr() As IEnumerator
		Dim direction As PlatformingLevelGroundMovementEnemy.Direction = PlatformingLevelGroundMovementEnemy.Direction.Right
		Dim isForeground As Boolean = False
		While True
			Yield CupheadTime.WaitForSeconds(Me, Me.spawnDelayRange.RandomFloat())
			Dim spawnPos As Vector2 = MyBase.transform.position
			spawnPos.y = MyBase.transform.position.y
			spawnPos.x += Global.UnityEngine.Random.Range(-Me.xRange, Me.xRange)
			Dim player As AbstractPlayerController = PlayerManager.GetNext()
			If CupheadLevelCamera.Current.ContainsPoint(spawnPos, New Vector2(0F, 1000F)) Then
				If Me.spawnString.Split(New Char() { ","c })(Me.spawnIndex)(0) = "F"c Then
					isForeground = True
				ElseIf Me.spawnString.Split(New Char() { ","c })(Me.spawnIndex)(0) = "B"c Then
					isForeground = False
				End If
				If Me.directionString.Split(New Char() { ","c })(Me.directionIndex)(0) = "L"c Then
					direction = PlatformingLevelGroundMovementEnemy.Direction.Left
				ElseIf Me.directionString.Split(New Char() { ","c })(Me.directionIndex)(0) = "R"c Then
					direction = PlatformingLevelGroundMovementEnemy.Direction.Right
				ElseIf Me.directionString.Split(New Char() { ","c })(Me.directionIndex)(0) = "P"c Then
					If player.transform.position.x < spawnPos.x Then
						direction = PlatformingLevelGroundMovementEnemy.Direction.Left
					Else
						direction = PlatformingLevelGroundMovementEnemy.Direction.Right
					End If
				End If
				Dim mountainPlatformingLevelSatyr As MountainPlatformingLevelSatyr = TryCast(Me.satyrPrefab.Spawn(spawnPos, direction, True), MountainPlatformingLevelSatyr)
				mountainPlatformingLevelSatyr.Init(direction, isForeground)
				Me.directionIndex = (Me.directionIndex + 1) Mod Me.directionString.Split(New Char() { ","c }).Length
				Me.spawnIndex = (Me.spawnIndex + 1) Mod Me.spawnString.Split(New Char() { ","c }).Length
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060035A2 RID: 13730 RVA: 0x001F3C74 File Offset: 0x001F2074
	Protected Overrides Sub OnDrawGizmos()
		Gizmos.color = New Color(1F, 1F, 0F, 1F)
		Gizmos.DrawLine(MyBase.baseTransform.position - New Vector3(Me.xRange, 0F, 0F), MyBase.baseTransform.position + New Vector3(Me.xRange, 0F, 0F))
	End Sub

	' Token: 0x04003DB1 RID: 15793
	<SerializeField()>
	Private directionString As String

	' Token: 0x04003DB2 RID: 15794
	<SerializeField()>
	Private spawnString As String

	' Token: 0x04003DB3 RID: 15795
	<SerializeField()>
	Private xRange As Single

	' Token: 0x04003DB4 RID: 15796
	<SerializeField()>
	Private satyrPrefab As MountainPlatformingLevelSatyr

	' Token: 0x04003DB5 RID: 15797
	<SerializeField()>
	Private spawnDelayRange As MinMax

	' Token: 0x04003DB6 RID: 15798
	Private directionIndex As Integer

	' Token: 0x04003DB7 RID: 15799
	Private spawnIndex As Integer
End Class
