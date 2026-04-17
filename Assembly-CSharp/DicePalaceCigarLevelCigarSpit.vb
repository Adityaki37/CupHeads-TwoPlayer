Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020005B0 RID: 1456
Public Class DicePalaceCigarLevelCigarSpit
	Inherits AbstractProjectile

	' Token: 0x06001C2F RID: 7215 RVA: 0x001029C4 File Offset: 0x00100DC4
	Public Sub InitProjectile(properties As LevelProperties.DicePalaceCigar, clockwise As Boolean, onRight As Boolean)
		Me.time = 0F
		Me.centerPoint = MyBase.transform.position
		Me.onRight = onRight
		If Not clockwise Then
			Me.circleSpeed = -properties.CurrentState.spiralSmoke.circleSpeed
		Else
			Me.circleSpeed = properties.CurrentState.spiralSmoke.circleSpeed
		End If
		Me.properties = properties
		MyBase.StartCoroutine(Me.move_cr())
		MyBase.StartCoroutine(Me.bullet_trail_cr())
	End Sub

	' Token: 0x06001C30 RID: 7216 RVA: 0x00102A50 File Offset: 0x00100E50
	Private Iterator Function move_cr() As IEnumerator
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		While True
			Me.centerPoint += -MyBase.transform.right * Me.properties.CurrentState.spiralSmoke.horizontalSpeed * CupheadTime.FixedDelta
			Dim newPos As Vector3 = Me.centerPoint
			newPos.y = Me.centerPoint.y + Mathf.Sin(Me.time * Me.circleSpeed) * Me.properties.CurrentState.spiralSmoke.spiralSmokeCircleSize
			If Me.onRight Then
				newPos.x = Me.centerPoint.x + Mathf.Cos(Me.time * Me.circleSpeed) * Me.properties.CurrentState.spiralSmoke.spiralSmokeCircleSize
			Else
				newPos.x = Me.centerPoint.x + -Mathf.Cos(Me.time * Me.circleSpeed) * Me.properties.CurrentState.spiralSmoke.spiralSmokeCircleSize
			End If
			MyBase.transform.position = newPos
			Me.time += CupheadTime.FixedDelta
			Yield wait
		End While
		Return
	End Function

	' Token: 0x06001C31 RID: 7217 RVA: 0x00102A6C File Offset: 0x00100E6C
	Private Iterator Function bullet_trail_cr() As IEnumerator
		While True
			Yield CupheadTime.WaitForSeconds(Me, Global.UnityEngine.Random.Range(0.16F, 0.2F))
			Me.bulletFX.Create(MyBase.transform.position)
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06001C32 RID: 7218 RVA: 0x00102A87 File Offset: 0x00100E87
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x0400253B RID: 9531
	<SerializeField()>
	Private bulletFX As Effect

	' Token: 0x0400253C RID: 9532
	Private onRight As Boolean

	' Token: 0x0400253D RID: 9533
	Private time As Single

	' Token: 0x0400253E RID: 9534
	Private circleSpeed As Single

	' Token: 0x0400253F RID: 9535
	Private centerPoint As Vector3

	' Token: 0x04002540 RID: 9536
	Private properties As LevelProperties.DicePalaceCigar
End Class
