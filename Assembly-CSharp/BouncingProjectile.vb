Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000AEB RID: 2795
Public Class BouncingProjectile
	Inherits AbstractProjectile

	' Token: 0x060043B4 RID: 17332 RVA: 0x0023FFF2 File Offset: 0x0023E3F2
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x060043B5 RID: 17333 RVA: 0x00240008 File Offset: 0x0023E408
	Private Iterator Function move_cr() As IEnumerator
		While True
			If Me.isMoving Then
				MyBase.transform.position += Me.velocity * Me.speed * CupheadTime.FixedDelta
			End If
			Yield New WaitForFixedUpdate()
		End While
		Return
	End Function

	' Token: 0x060043B6 RID: 17334 RVA: 0x00240024 File Offset: 0x0023E424
	Protected Overrides Sub OnCollisionCeiling(hit As GameObject, phase As CollisionPhase)
		Dim vector As Vector3 = Me.velocity
		vector.y = Mathf.Min(vector.y, -vector.y)
		Me.ChangeDir(vector)
	End Sub

	' Token: 0x060043B7 RID: 17335 RVA: 0x0024005C File Offset: 0x0023E45C
	Protected Overrides Sub OnCollisionGround(hit As GameObject, phase As CollisionPhase)
		Dim vector As Vector3 = Me.velocity
		vector.y = Mathf.Max(vector.y, -vector.y)
		Me.ChangeDir(vector)
	End Sub

	' Token: 0x060043B8 RID: 17336 RVA: 0x00240094 File Offset: 0x0023E494
	Protected Overrides Sub OnCollisionWalls(hit As GameObject, phase As CollisionPhase)
		Dim vector As Vector3 = Me.velocity
		If MyBase.transform.position.x > 0F Then
			vector.x = Mathf.Min(vector.x, -vector.x)
			Me.ChangeDir(vector)
		Else
			vector.x = Mathf.Max(vector.x, -vector.x)
			Me.ChangeDir(vector)
		End If
	End Sub

	' Token: 0x060043B9 RID: 17337 RVA: 0x00240110 File Offset: 0x0023E510
	Protected Overridable Sub ChangeDir(newVelocity As Vector3)
		Me.velocity = newVelocity
		Me.currentAngle = Mathf.Atan2(Me.velocity.y, Me.velocity.x) * 57.29578F
		MyBase.transform.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(Me.currentAngle))
	End Sub

	' Token: 0x04004968 RID: 18792
	Public isMoving As Boolean

	' Token: 0x04004969 RID: 18793
	Protected speed As Single

	' Token: 0x0400496A RID: 18794
	Protected currentAngle As Single

	' Token: 0x0400496B RID: 18795
	Protected velocity As Vector3
End Class
