Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000652 RID: 1618
Public Class FlyingCowboyLevelDebris
	Inherits BasicProjectile

	' Token: 0x17000390 RID: 912
	' (get) Token: 0x0600219A RID: 8602 RVA: 0x00137C79 File Offset: 0x00136079
	Private ReadOnly Property isCurved As Boolean
		Get
			Return Me.gravity <> 0F
		End Get
	End Property

	' Token: 0x0600219B RID: 8603 RVA: 0x00137C8C File Offset: 0x0013608C
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		Gizmos.DrawLine(New Vector3(FlyingCowboyLevelDebris.OffsetAimXCutoff, 1000F), New Vector3(FlyingCowboyLevelDebris.OffsetAimXCutoff, -1000F))
		Dim vector As Vector3 = MyBase.transform.position
		Dim vector2 As Vector3 = Me.curveSpeed
		For i As Integer = 0 To 50 - 1
			If Me.isCurved Then
				vector2 += New Vector3(Me.gravity * CupheadTime.FixedDelta, 0F)
				vector += vector2 * CupheadTime.FixedDelta
			Else
				vector += MyBase.transform.right * Me.Speed * CupheadTime.FixedDelta
			End If
			Gizmos.DrawWireSphere(vector, 10F)
		Next
	End Sub

	' Token: 0x0600219C RID: 8604 RVA: 0x00137D59 File Offset: 0x00136159
	Public Sub SetupLinearSpeed(speedRange As MinMax, speedUpDistance As Single, aimTransform As Transform)
		MyBase.StartCoroutine(Me.speedUp_cr(speedRange, speedUpDistance, aimTransform))
	End Sub

	' Token: 0x0600219D RID: 8605 RVA: 0x00137D6C File Offset: 0x0013616C
	Private Iterator Function speedUp_cr(speedRange As MinMax, distance As Single, aimTransform As Transform) As IEnumerator
		Dim wait As WaitForFixedUpdate = New WaitForFixedUpdate()
		Dim sqrDistance As Single = distance * distance
		While Vector3.SqrMagnitude(MyBase.transform.position - aimTransform.position) > sqrDistance
			Yield wait
		End While
		Dim duration As Single = KinematicUtilities.CalculateTimeToChangeVelocity(speedRange.min, speedRange.max, distance)
		Dim elapsedTime As Single = 0F
		While elapsedTime < duration
			Yield wait
			elapsedTime += CupheadTime.FixedDelta
			Me.Speed = Mathf.Lerp(speedRange.min, speedRange.max, elapsedTime / duration)
		End While
		Me.Speed = speedRange.max
		Return
	End Function

	' Token: 0x0600219E RID: 8606 RVA: 0x00137D9C File Offset: 0x0013619C
	Public Sub SetupVacuum(aimTransform As Transform, destroyTransform As Transform)
		MyBase.StartCoroutine(Me.vacuumAim_cr(aimTransform, destroyTransform))
	End Sub

	' Token: 0x0600219F RID: 8607 RVA: 0x00137DB0 File Offset: 0x001361B0
	Private Iterator Function vacuumAim_cr(aimTransform As Transform, destroyTransform As Transform) As IEnumerator
		If Me.isCurved Then
			While Me.curveSpeed.x < 0F
				Yield Nothing
			End While
		ElseIf MyBase.transform.position.x >= FlyingCowboyLevelDebris.OffsetAimXCutoff Then
			Dim distanceThreshold As Single = If((MyBase.transform.position.y <= 0F), 105F, 80F)
			MyBase.transform.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(MathUtils.DirectionToAngle(aimTransform.position + FlyingCowboyLevelDebris.OffsetAimAmount - MyBase.transform.position)))
			If MyBase.transform.position.x > aimTransform.position.x + FlyingCowboyLevelDebris.OffsetAimAmount.x Then
				While Mathf.Abs(MyBase.transform.position.y - aimTransform.position.y) > distanceThreshold
					Yield Nothing
				End While
			End If
			MyBase.transform.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(MathUtilities.DirectionToAngle(aimTransform.position - MyBase.transform.position)))
		End If
		While MyBase.transform.position.x < aimTransform.position.x
			Yield Nothing
		End While
		Me.StopAllCoroutines()
		MyBase.StartCoroutine(Me.vacuumSuckIn_cr(destroyTransform))
		Return
	End Function

	' Token: 0x060021A0 RID: 8608 RVA: 0x00137DDC File Offset: 0x001361DC
	Private Iterator Function vacuumSuckIn_cr(destroyTransform As Transform) As IEnumerator
		MyBase.transform.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(MathUtilities.DirectionToAngle(destroyTransform.position - MyBase.transform.position)))
		Me.move = True
		If Me.isCurved Then
			Me.Speed = Me.curveSpeed.magnitude
		End If
		Me.Speed *= 1.25F
		MyBase.StartCoroutine(Me.squash_cr())
		While MyBase.transform.position.x < destroyTransform.position.x
			Yield Nothing
		End While
		MyBase.GetComponent(Of SpriteRenderer)().enabled = False
		Me.Die()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Return
	End Function

	' Token: 0x060021A1 RID: 8609 RVA: 0x00137E00 File Offset: 0x00136200
	Private Iterator Function squash_cr() As IEnumerator
		Dim wait As WaitForFrameTimePersistent = New WaitForFrameTimePersistent(0.041666668F, False)
		Dim elapsedTime As Single = 0F
		While elapsedTime < FlyingCowboyLevelDebris.SquashDuration
			Yield wait
			elapsedTime += wait.frameTime + wait.accumulator
			Dim scale As Vector3 = Vector3.Lerp(Vector2.one, FlyingCowboyLevelDebris.SquashAmount, elapsedTime / FlyingCowboyLevelDebris.SquashDuration)
			MyBase.transform.localScale = scale
		End While
		Return
	End Function

	' Token: 0x060021A2 RID: 8610 RVA: 0x00137E1B File Offset: 0x0013621B
	Public Sub ToCurve(speed As Vector3, gravity As Single)
		Me.curveSpeed = speed
		Me.gravity = gravity
		MyBase.StartCoroutine(Me.gravity_cr())
	End Sub

	' Token: 0x060021A3 RID: 8611 RVA: 0x00137E38 File Offset: 0x00136238
	Private Iterator Function gravity_cr() As IEnumerator
		Me.move = False
		While True
			Me.curveSpeed += New Vector3(Me.gravity * CupheadTime.FixedDelta, 0F)
			MyBase.transform.Translate(Me.curveSpeed * CupheadTime.FixedDelta)
			Yield New WaitForFixedUpdate()
		End While
		Return
	End Function

	' Token: 0x04002A38 RID: 10808
	Private Shared OffsetAimXCutoff As Single = 0F

	' Token: 0x04002A39 RID: 10809
	Private Shared OffsetAimAmount As Vector3 = New Vector3(-50F, 0F)

	' Token: 0x04002A3A RID: 10810
	Private Shared SquashAmount As Vector3 = New Vector3(1.2F, 0.5F, 1F)

	' Token: 0x04002A3B RID: 10811
	Private Shared SquashDuration As Single = 0.4F

	' Token: 0x04002A3C RID: 10812
	Private curveSpeed As Vector3

	' Token: 0x04002A3D RID: 10813
	Private gravity As Single
End Class
