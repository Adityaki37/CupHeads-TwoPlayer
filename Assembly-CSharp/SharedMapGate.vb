Imports System

' Token: 0x02000968 RID: 2408
Public Class SharedMapGate
	Inherits MapLevelDependentObstacle

	' Token: 0x0600381C RID: 14364 RVA: 0x00201A34 File Offset: 0x001FFE34
	Protected Overrides Function ValidateSucess() As Boolean
		Dim flag As Boolean = False
		For Each levels2 As Levels In Me._levels
			Dim levelData As PlayerData.PlayerLevelDataObject = PlayerData.Data.GetLevelData(levels2)
			If levelData.completed Then
				flag = True
				Me.difficulty = levelData.difficultyBeaten
				Me.grade = levelData.grade
				Exit For
			End If
		Next
		Return flag
	End Function

	' Token: 0x0600381D RID: 14365 RVA: 0x00201AA0 File Offset: 0x001FFEA0
	Protected Overrides Function ValidateCondition(level As Levels) As Boolean
		Dim flag As Boolean = False
		If Level.PreviousLevel <> level AndAlso PlayerData.Data.GetLevelData(level).completed Then
			Me.previouslyWon = True
		End If
		If Me.previouslyWon Then
			Return False
		End If
		If Not Level.PreviouslyWon AndAlso Level.Won Then
			flag = True
		End If
		If Me.ReactToGradeChange AndAlso Level.Grade > Level.PreviousGrade Then
			Me.gradeChanged = True
			flag = True
		End If
		If Me.ReactToDifficultyChange AndAlso Level.Difficulty > Level.PreviousDifficulty Then
			Me.difficultyChanged = True
			flag = True
		End If
		Return flag
	End Function

	' Token: 0x04003FFD RID: 16381
	Private previouslyWon As Boolean
End Class
