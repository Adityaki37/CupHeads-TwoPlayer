Imports System
Imports UnityEngine

' Token: 0x02000394 RID: 916
Public Module KinematicUtilities
	' Token: 0x06000B07 RID: 2823 RVA: 0x00081CB0 File Offset: 0x000800B0
	Public Function CalculateAcceleration(distance As Single, finalSpeed As Single) As Single
		Return 0.5F * finalSpeed * finalSpeed / distance
	End Function

	' Token: 0x06000B08 RID: 2824 RVA: 0x00081CBD File Offset: 0x000800BD
	Public Function CalculateTimeToSpeed(distance As Single, finalSpeed As Single) As Single
		Return 2F * distance / finalSpeed
	End Function

	' Token: 0x06000B09 RID: 2825 RVA: 0x00081CC8 File Offset: 0x000800C8
	Public Function CalculateTimeToTravelDistance(distance As Single, speed As Single) As Single
		Return distance / speed
	End Function

	' Token: 0x06000B0A RID: 2826 RVA: 0x00081CCD File Offset: 0x000800CD
	Public Function CalculateVelocityFromZero(distance As Single, time As Single) As Single
		Return 2F * distance / time
	End Function

	' Token: 0x06000B0B RID: 2827 RVA: 0x00081CD8 File Offset: 0x000800D8
	Public Function CalculateAccelerationFromZero(distance As Single, time As Single) As Single
		Dim num As Single = time * time
		Return 2F * distance / num
	End Function

	' Token: 0x06000B0C RID: 2828 RVA: 0x00081CF2 File Offset: 0x000800F2
	Public Function CalculateTimeToChangeVelocity(v1 As Single, v2 As Single, distance As Single) As Single
		Return 2F * distance / (v1 + v2)
	End Function

	' Token: 0x06000B0D RID: 2829 RVA: 0x00081CFF File Offset: 0x000800FF
	Public Function CalculateInitialSpeedToReachApex(apexHeight As Single, gravity As Single) As Single
		Return Mathf.Sqrt(2F * gravity * apexHeight)
	End Function

	' Token: 0x06000B0E RID: 2830 RVA: 0x00081D0F File Offset: 0x0008010F
	Public Function CalculateDistanceTravelled(initialVelocity As Vector2, startingHeight As Single, gravity As Single) As Single
		Return initialVelocity.x / gravity * (initialVelocity.y + Mathf.Sqrt(initialVelocity.y * initialVelocity.y + 2F * gravity * startingHeight))
	End Function

	' Token: 0x06000B0F RID: 2831 RVA: 0x00081D41 File Offset: 0x00080141
	Public Function CalculateHorizontalSpeedToTravelDistance(distance As Single, velocityY As Single, startingHeight As Single, gravity As Single) As Single
		Return distance * gravity / (velocityY + Mathf.Sqrt(velocityY * velocityY + 2F * gravity * startingHeight))
	End Function
End Module
