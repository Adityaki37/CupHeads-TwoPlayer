Imports System
Imports UnityEngine

' Token: 0x02000B2D RID: 2861
Public Class LevelScoringData
	' Token: 0x06004564 RID: 17764 RVA: 0x00247FDC File Offset: 0x002463DC
	Public Function CalculateGrade() As LevelScoringData.Grade
		If Me.pacifistRun AndAlso Not Me.usedDjimmi Then
			Return LevelScoringData.Grade.P
		End If
		Dim scoringProperties As ScoringEditorData = Cuphead.Current.ScoringProperties
		Dim num As Single = Mathf.Clamp(100F - 100F * (Me.time - Me.goalTime) / (Me.goalTime * (scoringProperties.bestTimeMultiplierForNoScore - 1F)), 0F, 100F)
		Dim num2 As Single = Mathf.Clamp(100F - 100F * ((scoringProperties.hitsForNoScore - CSng(Me.finalHP)) / scoringProperties.hitsForNoScore), 0F, 100F)
		Dim num3 As Single = 100F * Mathf.Min(CSng(Me.numParries), scoringProperties.parriesForHighestGrade + scoringProperties.bonusParries) / scoringProperties.parriesForHighestGrade
		Dim num4 As Single = 100F * Mathf.Min(CSng(Me.superMeterUsed), scoringProperties.superMeterUsageForHighestGrade + scoringProperties.bonusSuperMeterUsage) / scoringProperties.superMeterUsageForHighestGrade
		If Me.useCoinsInsteadOfSuperMeter Then
			num4 = 100F * (CSng(Me.coinsCollected) / 5F)
		End If
		Dim num5 As Single = num * scoringProperties.timeWeight + num2 * scoringProperties.hitsWeight + num3 * scoringProperties.parriesWeight + num4 * scoringProperties.superMeterUsageWeight
		Dim array As ScoringEditorData.GradingCurveEntry() = If((Me.difficulty <> Level.Mode.Easy), If((Me.difficulty <> Level.Mode.Normal), scoringProperties.hardGradingCurve, scoringProperties.mediumGradingCurve), scoringProperties.easyGradingCurve)
		Dim grade As LevelScoringData.Grade = LevelScoringData.Grade.DMinus
		For Each gradingCurveEntry As ScoringEditorData.GradingCurveEntry In array
			grade = gradingCurveEntry.grade
			If num5 < gradingCurveEntry.upperLimit Then
				Exit For
			End If
		Next
		If Me.usedDjimmi AndAlso grade > LevelScoringData.Grade.BPlus Then
			grade = LevelScoringData.Grade.BPlus
		End If
		Return grade
	End Function

	' Token: 0x04004AF6 RID: 19190
	Public time As Single

	' Token: 0x04004AF7 RID: 19191
	Public goalTime As Single

	' Token: 0x04004AF8 RID: 19192
	Public finalHP As Integer

	' Token: 0x04004AF9 RID: 19193
	Public numTimesHit As Integer

	' Token: 0x04004AFA RID: 19194
	Public numParries As Integer

	' Token: 0x04004AFB RID: 19195
	Public superMeterUsed As Integer

	' Token: 0x04004AFC RID: 19196
	Public coinsCollected As Integer

	' Token: 0x04004AFD RID: 19197
	Public difficulty As Level.Mode

	' Token: 0x04004AFE RID: 19198
	Public pacifistRun As Boolean

	' Token: 0x04004AFF RID: 19199
	Public useCoinsInsteadOfSuperMeter As Boolean

	' Token: 0x04004B00 RID: 19200
	Public usedDjimmi As Boolean

	' Token: 0x04004B01 RID: 19201
	Public player1IsChalice As Boolean

	' Token: 0x04004B02 RID: 19202
	Public player2IsChalice As Boolean

	' Token: 0x02000B2E RID: 2862
	Public Enum Grade
		' Token: 0x04004B04 RID: 19204
		DMinus
		' Token: 0x04004B05 RID: 19205
		D
		' Token: 0x04004B06 RID: 19206
		DPlus
		' Token: 0x04004B07 RID: 19207
		CMinus
		' Token: 0x04004B08 RID: 19208
		C
		' Token: 0x04004B09 RID: 19209
		CPlus
		' Token: 0x04004B0A RID: 19210
		BMinus
		' Token: 0x04004B0B RID: 19211
		B
		' Token: 0x04004B0C RID: 19212
		BPlus
		' Token: 0x04004B0D RID: 19213
		AMinus
		' Token: 0x04004B0E RID: 19214
		A
		' Token: 0x04004B0F RID: 19215
		APlus
		' Token: 0x04004B10 RID: 19216
		S
		' Token: 0x04004B11 RID: 19217
		P
	End Enum
End Class
