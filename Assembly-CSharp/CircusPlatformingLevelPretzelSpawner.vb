Imports System
Imports UnityEngine

' Token: 0x020008AC RID: 2220
Public Class CircusPlatformingLevelPretzelSpawner
	Inherits PlatformingLevelEnemySpawner

	' Token: 0x060033BD RID: 13245 RVA: 0x001E0920 File Offset: 0x001DED20
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.sideIndex = Global.UnityEngine.Random.Range(0, Me.sideString.Split(New Char() { ","c }).Length)
		If Me.path Is Nothing OrElse Me.path.Length = 0 Then
			Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		End If
	End Sub

	' Token: 0x060033BE RID: 13246 RVA: 0x001E097C File Offset: 0x001DED7C
	Protected Overrides Sub Spawn()
		MyBase.Spawn()
		Dim flag As Boolean = False
		Dim num As Integer = -1
		If Me.sideString.Split(New Char() { ","c })(Me.sideIndex)(0) = "L"c Then
			flag = True
			num = Me.path.Length - 1
			For i As Integer = 0 To Me.path.Length - 1
				If Me.path(i).position.x > CupheadLevelCamera.Current.Bounds.xMax + 100F Then
					num = i
					Exit For
				End If
			Next
		ElseIf Me.sideString.Split(New Char() { ","c })(Me.sideIndex)(0) = "R"c Then
			num = 0
			For j As Integer = Me.path.Length - 1 To 0 Step -1
				If Me.path(j).position.x < CupheadLevelCamera.Current.Bounds.xMin - 100F Then
					num = j
					Exit For
				End If
			Next
			flag = False
		End If
		Me.spawnPosition.y = CupheadLevelCamera.Current.transform.position.y
		Dim circusPlatformingLevelPretzel As CircusPlatformingLevelPretzel = Me.pretzelPrefab.Spawn()
		circusPlatformingLevelPretzel.SetPath(Me.path)
		circusPlatformingLevelPretzel.goingLeft = flag
		circusPlatformingLevelPretzel.SetStartPosition(num)
		Me.sideIndex = (Me.sideIndex + 1) Mod Me.sideString.Split(New Char() { ","c }).Length
	End Sub

	' Token: 0x04003C0A RID: 15370
	<SerializeField()>
	Private pretzelPrefab As CircusPlatformingLevelPretzel

	' Token: 0x04003C0B RID: 15371
	<SerializeField()>
	Private sideString As String

	' Token: 0x04003C0C RID: 15372
	<SerializeField()>
	Private path As Transform()

	' Token: 0x04003C0D RID: 15373
	Private sideIndex As Integer

	' Token: 0x04003C0E RID: 15374
	Private spawnPosition As Vector3
End Class
