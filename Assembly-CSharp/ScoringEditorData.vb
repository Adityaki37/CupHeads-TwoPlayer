Imports System

' Token: 0x02000B2F RID: 2863
Public Class ScoringEditorData
	Inherits AbstractMonoBehaviour

	' Token: 0x04004B12 RID: 19218
	Public bestTimeMultiplierForNoScore As Single

	' Token: 0x04004B13 RID: 19219
	Public hitsForNoScore As Single

	' Token: 0x04004B14 RID: 19220
	Public parriesForHighestGrade As Single

	' Token: 0x04004B15 RID: 19221
	Public bonusParries As Single

	' Token: 0x04004B16 RID: 19222
	Public superMeterUsageForHighestGrade As Single

	' Token: 0x04004B17 RID: 19223
	Public bonusSuperMeterUsage As Single

	' Token: 0x04004B18 RID: 19224
	Public easyGradingCurve As ScoringEditorData.GradingCurveEntry()

	' Token: 0x04004B19 RID: 19225
	Public mediumGradingCurve As ScoringEditorData.GradingCurveEntry()

	' Token: 0x04004B1A RID: 19226
	Public hardGradingCurve As ScoringEditorData.GradingCurveEntry()

	' Token: 0x04004B1B RID: 19227
	Public timeWeight As Single

	' Token: 0x04004B1C RID: 19228
	Public hitsWeight As Single

	' Token: 0x04004B1D RID: 19229
	Public parriesWeight As Single

	' Token: 0x04004B1E RID: 19230
	Public superMeterUsageWeight As Single

	' Token: 0x02000B30 RID: 2864
	<Serializable()>
	Public Class GradingCurveEntry
		' Token: 0x04004B1F RID: 19231
		Public grade As LevelScoringData.Grade

		' Token: 0x04004B20 RID: 19232
		Public upperLimit As Single
	End Class
End Class
