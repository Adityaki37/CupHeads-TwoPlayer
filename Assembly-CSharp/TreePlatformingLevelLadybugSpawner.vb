Imports System
Imports UnityEngine

' Token: 0x02000890 RID: 2192
Public Class TreePlatformingLevelLadybugSpawner
	Inherits PlatformingLevelEnemySpawner

	' Token: 0x060032F6 RID: 13046 RVA: 0x001D9D2C File Offset: 0x001D812C
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.typeIndex = Global.UnityEngine.Random.Range(0, Me.typeString.Split(New Char() { ","c }).Length)
		Me.sideIndex = Global.UnityEngine.Random.Range(0, Me.sideString.Split(New Char() { ","c }).Length)
	End Sub

	' Token: 0x060032F7 RID: 13047 RVA: 0x001D9D88 File Offset: 0x001D8188
	Protected Overrides Sub Spawn()
		Dim direction As PlatformingLevelGroundMovementEnemy.Direction = PlatformingLevelGroundMovementEnemy.Direction.Right
		Dim type As TreePlatformingLevelLadyBug.Type = TreePlatformingLevelLadyBug.Type.BounceFast
		If Me.sideString.Split(New Char() { ","c })(Me.sideIndex)(0) = "R"c Then
			Me.spawnPosition.x = CupheadLevelCamera.Current.Bounds.xMin - 50F
			direction = PlatformingLevelGroundMovementEnemy.Direction.Right
		ElseIf Me.sideString.Split(New Char() { ","c })(Me.sideIndex)(0) = "L"c Then
			Me.spawnPosition.x = CupheadLevelCamera.Current.Bounds.xMax + 50F
			direction = PlatformingLevelGroundMovementEnemy.Direction.Left
		End If
		Dim text As String = Me.typeString.Split(New Char() { ","c })(Me.typeIndex)
		If text IsNot Nothing Then
			If Not(text = "BS") Then
				If Not(text = "BF") Then
					If Not(text = "GS") Then
						If Not(text = "GF") Then
							If text = "P" Then
								type = TreePlatformingLevelLadyBug.Type.BouncePink
								Me.spawnPosition.x = CupheadLevelCamera.Current.Bounds.xMax + 50F
								direction = PlatformingLevelGroundMovementEnemy.Direction.Left
							End If
						Else
							type = TreePlatformingLevelLadyBug.Type.GroundFast
						End If
					Else
						type = TreePlatformingLevelLadyBug.Type.GroundSlow
					End If
				Else
					type = TreePlatformingLevelLadyBug.Type.BounceFast
					Me.spawnPosition.x = CupheadLevelCamera.Current.Bounds.xMax + 50F
					direction = PlatformingLevelGroundMovementEnemy.Direction.Left
				End If
			Else
				type = TreePlatformingLevelLadyBug.Type.BounceSlow
				Me.spawnPosition.x = CupheadLevelCamera.Current.Bounds.xMax + 50F
				direction = PlatformingLevelGroundMovementEnemy.Direction.Left
			End If
		End If
		Me.spawnPosition.y = CupheadLevelCamera.Current.transform.position.y
		Me.ladybugPrefab.Spawn(Me.spawnPosition, direction, True, type)
		Me.typeIndex = (Me.typeIndex + 1) Mod Me.typeString.Split(New Char() { ","c }).Length
		Me.sideIndex = (Me.sideIndex + 1) Mod Me.sideString.Split(New Char() { ","c }).Length
		MyBase.Spawn()
	End Sub

	' Token: 0x04003B1A RID: 15130
	<SerializeField()>
	Private ladybugPrefab As TreePlatformingLevelLadyBug

	' Token: 0x04003B1B RID: 15131
	<SerializeField()>
	Private typeString As String

	' Token: 0x04003B1C RID: 15132
	<SerializeField()>
	Private sideString As String

	' Token: 0x04003B1D RID: 15133
	Private typeIndex As Integer

	' Token: 0x04003B1E RID: 15134
	Private sideIndex As Integer

	' Token: 0x04003B1F RID: 15135
	Private spawnPosition As Vector3
End Class
