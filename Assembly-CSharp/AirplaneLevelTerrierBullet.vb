Imports System
Imports UnityEngine

' Token: 0x02000AE5 RID: 2789
Public Class AirplaneLevelTerrierBullet
	Inherits BasicProjectile

	' Token: 0x0600438D RID: 17293 RVA: 0x0023FDA8 File Offset: 0x0023E1A8
	Public Function Create(position As Vector2, rotation As Single, speed As Single, acceleration As Single) As AirplaneLevelTerrierBullet
		Dim airplaneLevelTerrierBullet As AirplaneLevelTerrierBullet = TryCast(Me.Create(position, rotation), AirplaneLevelTerrierBullet)
		airplaneLevelTerrierBullet.endVelocity = MathUtils.AngleToDirection(rotation) * speed
		airplaneLevelTerrierBullet.startVelocity = airplaneLevelTerrierBullet.endVelocity.normalized * 0.1F
		airplaneLevelTerrierBullet.accelT = 0F
		airplaneLevelTerrierBullet.accel = acceleration
		airplaneLevelTerrierBullet.transform.rotation = Quaternion.identity
		Dim num As Single = Vector3.SignedAngle(airplaneLevelTerrierBullet.velocity, Vector3.up, Vector3.forward)
		While Mathf.Abs(num) > 45F
			num -= 90F * Mathf.Sign(num)
		End While
		airplaneLevelTerrierBullet.transform.Rotate(New Vector3(0F, 0F, -num))
		Return airplaneLevelTerrierBullet
	End Function

	' Token: 0x0600438E RID: 17294 RVA: 0x0023FE6F File Offset: 0x0023E26F
	Public Sub PlayWow()
		MyBase.animator.Play("WowIntro")
	End Sub

	' Token: 0x0600438F RID: 17295 RVA: 0x0023FE84 File Offset: 0x0023E284
	Protected Overrides Sub Move()
		Me.accelT += CupheadTime.FixedDelta * Me.accel * 1.6F
		Me.velocity = Vector3.Lerp(Me.startVelocity, Me.endVelocity, Me.accelT)
		MyBase.transform.position += Me.velocity * CupheadTime.FixedDelta
	End Sub

	' Token: 0x04004953 RID: 18771
	Private Const BASE_ACCELERATION As Single = 1.6F

	' Token: 0x04004954 RID: 18772
	Protected velocity As Vector3

	' Token: 0x04004955 RID: 18773
	Protected startVelocity As Vector3

	' Token: 0x04004956 RID: 18774
	Protected endVelocity As Vector3

	' Token: 0x04004957 RID: 18775
	Protected accelT As Single

	' Token: 0x04004958 RID: 18776
	Protected accel As Single
End Class
