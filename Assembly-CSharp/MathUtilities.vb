Imports System
Imports UnityEngine

' Token: 0x02000395 RID: 917
Public Module MathUtilities
	' Token: 0x06000B10 RID: 2832 RVA: 0x00081D5C File Offset: 0x0008015C
	Public Function SameSign(a As Single, b As Single) As Boolean
		Return(Mathf.Approximately(a, 0F) AndAlso Mathf.Approximately(b, 0F)) OrElse (a > 0F AndAlso b > 0F) OrElse (a < 0F AndAlso b < 0F)
	End Function

	' Token: 0x06000B11 RID: 2833 RVA: 0x00081DBC File Offset: 0x000801BC
	Public Function LerpMapping(value As Single, fromStart As Single, fromEnd As Single, toStart As Single, toEnd As Single, Optional clamp As Boolean = False) As Single
		Dim num As Single = (value - fromStart) / (fromEnd - fromStart)
		If clamp Then
			num = Mathf.Max(0F, Mathf.Min(1F, num))
		End If
		Return toStart + (toEnd - toStart) * num
	End Function

	' Token: 0x06000B12 RID: 2834 RVA: 0x00081DF8 File Offset: 0x000801F8
	Public Function SqrDistanceToLine(ray As Ray, point As Vector3) As Single
		Return Vector3.Cross(ray.direction, point - ray.origin).sqrMagnitude
	End Function

	' Token: 0x06000B13 RID: 2835 RVA: 0x00081E28 File Offset: 0x00080228
	Public Function DistanceToLine(ray As Ray, point As Vector3) As Single
		Return Vector3.Cross(ray.direction, point - ray.origin).magnitude
	End Function

	' Token: 0x06000B14 RID: 2836 RVA: 0x00081E56 File Offset: 0x00080256
	Public Function DecimalPart(value As Single) As Single
		If value < 0F Then
			Return value - Mathf.Ceil(value)
		End If
		Return value - Mathf.Floor(value)
	End Function

	' Token: 0x06000B15 RID: 2837 RVA: 0x00081E74 File Offset: 0x00080274
	Public Function NextIndex(currentIndex As Integer, indexLength As Integer) As Integer
		currentIndex += 1
		If currentIndex >= indexLength Then
			currentIndex = 0
		End If
		Return currentIndex
	End Function

	' Token: 0x06000B16 RID: 2838 RVA: 0x00081E86 File Offset: 0x00080286
	Public Function PreviousIndex(currentIndex As Integer, indexLength As Integer) As Integer
		currentIndex -= 1
		If currentIndex < 0 Then
			currentIndex = indexLength - 1
		End If
		Return currentIndex
	End Function

	' Token: 0x06000B17 RID: 2839 RVA: 0x00081E9C File Offset: 0x0008029C
	Public Function LinesIntersect(s1 As Vector2, e1 As Vector2, s2 As Vector2, e2 As Vector2, <System.Runtime.InteropServices.OutAttribute()> ByRef intersectionPoint As Vector2) As Boolean
		Dim num As Single = e1.y - s1.y
		Dim num2 As Single = s1.x - e1.x
		Dim num3 As Single = num * s1.x + num2 * s1.y
		Dim num4 As Single = e2.y - s2.y
		Dim num5 As Single = s2.x - e2.x
		Dim num6 As Single = num4 * s2.x + num5 * s2.y
		Dim num7 As Single = num * num5 - num4 * num2
		If Mathf.Approximately(num7, 0F) Then
			intersectionPoint = Vector2.zero
			Return False
		End If
		Dim num8 As Single = 1F / num7
		intersectionPoint = New Vector2((num5 * num3 - num2 * num6) * num8, (num * num6 - num4 * num3) * num8)
		Return True
	End Function

	' Token: 0x06000B18 RID: 2840 RVA: 0x00081F66 File Offset: 0x00080366
	Public Function HadamardProduct(v1 As Vector2, v2 As Vector2) As Vector2
		Return New Vector2(v1.x * v2.x, v1.y * v2.y)
	End Function

	' Token: 0x06000B19 RID: 2841 RVA: 0x00081F8B File Offset: 0x0008038B
	Public Function HadamardProduct(v1 As Vector3, v2 As Vector3) As Vector3
		Return New Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z)
	End Function

	' Token: 0x06000B1A RID: 2842 RVA: 0x00081FBF File Offset: 0x000803BF
	Public Function BetweenInclusive(value As Integer, min As Integer, max As Integer) As Boolean
		Return value >= min AndAlso value <= max
	End Function

	' Token: 0x06000B1B RID: 2843 RVA: 0x00081FD2 File Offset: 0x000803D2
	Public Function BetweenInclusive(value As Single, min As Single, max As Single) As Boolean
		Return value >= min AndAlso value <= max
	End Function

	' Token: 0x06000B1C RID: 2844 RVA: 0x00081FE5 File Offset: 0x000803E5
	Public Function BetweenExclusive(value As Single, min As Single, max As Single) As Boolean
		Return value > min AndAlso value < max
	End Function

	' Token: 0x06000B1D RID: 2845 RVA: 0x00081FF5 File Offset: 0x000803F5
	Public Function BetweenInclusiveExclusive(value As Single, min As Single, max As Single) As Boolean
		Return value >= min AndAlso value < max
	End Function

	' Token: 0x06000B1E RID: 2846 RVA: 0x00082005 File Offset: 0x00080405
	Public Function BetweenExclusiveInclusive(value As Single, min As Single, max As Single) As Boolean
		Return value > min AndAlso value <= max
	End Function

	' Token: 0x06000B1F RID: 2847 RVA: 0x00082018 File Offset: 0x00080418
	Public Function ClampAngleSoft(angle As Single) As Single
		If angle >= 6.2831855F Then
			angle -= 6.2831855F
		ElseIf angle < 0F Then
			angle += 6.2831855F
		End If
		Return angle
	End Function

	' Token: 0x06000B20 RID: 2848 RVA: 0x00082048 File Offset: 0x00080448
	Public Function DirectionToAngle(direction As Vector2) As Single
		Return Mathf.Atan2(direction.y, direction.x) * 57.29578F
	End Function

	' Token: 0x06000B21 RID: 2849 RVA: 0x00082064 File Offset: 0x00080464
	Public Function AngleToDirection(angle As Single) As Vector2
		Dim num As Single = angle * 0.017453292F
		Return New Vector2(Mathf.Cos(num), Mathf.Sin(num))
	End Function

	' Token: 0x06000B22 RID: 2850 RVA: 0x0008208C File Offset: 0x0008048C
	Public Function TrigonmetricVector(t As Single, amplitude As Single, frequency As Single, Optional phaseShift As Single = 0F, Optional globalPhaseShift As Single = 0F) As Vector2
		Dim vector As Vector2
		vector.x = amplitude * Mathf.Cos(frequency * (t + phaseShift) + globalPhaseShift)
		vector.y = amplitude * Mathf.Sin(frequency * (t + phaseShift) + globalPhaseShift)
		Return vector
	End Function

	' Token: 0x040014BB RID: 5307
	Public Const Sqrt2 As Single = 1.4142135F

	' Token: 0x040014BC RID: 5308
	Public Const InverseSqrt2 As Single = 0.70710677F

	' Token: 0x040014BD RID: 5309
	Public Const TwoPi As Single = 6.2831855F

	' Token: 0x040014BE RID: 5310
	Public Const HalfPi As Single = 1.5707964F
End Module
