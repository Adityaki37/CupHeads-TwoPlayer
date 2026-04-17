Imports System
Imports UnityEngine

' Token: 0x02000396 RID: 918
Public Module MathUtils
	' Token: 0x06000B23 RID: 2851 RVA: 0x000820C6 File Offset: 0x000804C6
	Public Function GetPercentage(min As Single, max As Single, t As Single) As Single
		Return(t - min) / (max - min)
	End Function

	' Token: 0x06000B24 RID: 2852 RVA: 0x000820CF File Offset: 0x000804CF
	Public Function PlusOrMinus() As Integer
		Return If((Global.UnityEngine.Random.value <= 0.5F), (-1), 1)
	End Function

	' Token: 0x06000B25 RID: 2853 RVA: 0x000820E7 File Offset: 0x000804E7
	Public Function ExpRandom(mean As Single) As Single
		Return-Mathf.Log(Global.UnityEngine.Random.Range(0F, 1F)) * mean
	End Function

	' Token: 0x06000B26 RID: 2854 RVA: 0x00082100 File Offset: 0x00080500
	Public Function RandomBool() As Boolean
		Return Global.UnityEngine.Random.value > 0.5F
	End Function

	' Token: 0x06000B27 RID: 2855 RVA: 0x0008210E File Offset: 0x0008050E
	Public Function RandomPointInUnitCircle() As Vector2
		Return MathUtils.AngleToDirection(Global.UnityEngine.Random.Range(0F, 360F)) * Mathf.Sqrt(Global.UnityEngine.Random.Range(0F, 1F))
	End Function

	' Token: 0x06000B28 RID: 2856 RVA: 0x0008213D File Offset: 0x0008053D
	Public Function DirectionToAngle(direction As Vector2) As Single
		Return Mathf.Atan2(direction.y, direction.x) * 360F / 6.2831855F
	End Function

	' Token: 0x06000B29 RID: 2857 RVA: 0x00082160 File Offset: 0x00080560
	Public Function AngleToDirection(angle As Single) As Vector2
		Dim num As Single = angle * 3.1415927F * 2F / 360F
		Return New Vector2(Mathf.Cos(num), Mathf.Sin(num))
	End Function

	' Token: 0x06000B2A RID: 2858 RVA: 0x00082194 File Offset: 0x00080594
	Public Function CircleContains(center As Vector2, radius As Single, point As Vector2) As Boolean
		Return Mathf.Pow(point.x - center.x, 2F) + Mathf.Pow(point.y - center.y, 2F) < Mathf.Pow(radius, 2F)
	End Function
End Module
