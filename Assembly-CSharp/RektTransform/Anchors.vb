Imports System

Namespace RektTransform
	' Token: 0x0200036B RID: 875
	Public Module Anchors
		' Token: 0x04001449 RID: 5193
		Public TopLeft As MinMax = New MinMax(0F, 1F, 0F, 1F)

		' Token: 0x0400144A RID: 5194
		Public TopCenter As MinMax = New MinMax(0.5F, 1F, 0.5F, 1F)

		' Token: 0x0400144B RID: 5195
		Public TopRight As MinMax = New MinMax(1F, 1F, 1F, 1F)

		' Token: 0x0400144C RID: 5196
		Public TopStretch As MinMax = New MinMax(0F, 1F, 1F, 1F)

		' Token: 0x0400144D RID: 5197
		Public MiddleLeft As MinMax = New MinMax(0F, 0.5F, 0F, 0.5F)

		' Token: 0x0400144E RID: 5198
		Public TrueCenter As MinMax = New MinMax(0.5F, 0.5F, 0.5F, 0.5F)

		' Token: 0x0400144F RID: 5199
		Public MiddleCenter As MinMax = New MinMax(0.5F, 0.5F, 0.5F, 0.5F)

		' Token: 0x04001450 RID: 5200
		Public MiddleRight As MinMax = New MinMax(1F, 0.5F, 1F, 0.5F)

		' Token: 0x04001451 RID: 5201
		Public MiddleStretch As MinMax = New MinMax(0F, 0.5F, 1F, 0.5F)

		' Token: 0x04001452 RID: 5202
		Public BottomLeft As MinMax = New MinMax(0F, 0F, 0F, 0F)

		' Token: 0x04001453 RID: 5203
		Public BottomCenter As MinMax = New MinMax(0.5F, 0F, 0.5F, 0F)

		' Token: 0x04001454 RID: 5204
		Public BottomRight As MinMax = New MinMax(1F, 0F, 1F, 0F)

		' Token: 0x04001455 RID: 5205
		Public BottomStretch As MinMax = New MinMax(0F, 0F, 1F, 0F)

		' Token: 0x04001456 RID: 5206
		Public StretchLeft As MinMax = New MinMax(0F, 0F, 0F, 1F)

		' Token: 0x04001457 RID: 5207
		Public StretchCenter As MinMax = New MinMax(0.5F, 0F, 0.5F, 1F)

		' Token: 0x04001458 RID: 5208
		Public StretchRight As MinMax = New MinMax(1F, 0F, 1F, 1F)

		' Token: 0x04001459 RID: 5209
		Public TrueStretch As MinMax = New MinMax(0F, 0F, 1F, 1F)

		' Token: 0x0400145A RID: 5210
		Public StretchStretch As MinMax = New MinMax(0F, 0F, 1F, 1F)
	End Module
End Namespace
