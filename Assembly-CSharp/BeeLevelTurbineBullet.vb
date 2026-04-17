Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000525 RID: 1317
Public Class BeeLevelTurbineBullet
	Inherits AbstractProjectile

	' Token: 0x060017B0 RID: 6064 RVA: 0x000D55BC File Offset: 0x000D39BC
	Public Function Create(pos As Vector2, rotation As Single, onRight As Boolean, properties As LevelProperties.Bee.TurbineBlasters) As BeeLevelTurbineBullet
		Dim beeLevelTurbineBullet As BeeLevelTurbineBullet = TryCast(MyBase.Create(), BeeLevelTurbineBullet)
		beeLevelTurbineBullet.properties = properties
		beeLevelTurbineBullet.transform.position = pos
		beeLevelTurbineBullet.onRight = onRight
		beeLevelTurbineBullet.direction = MathUtils.AngleToDirection(rotation)
		beeLevelTurbineBullet.velocity = properties.bulletSpeed
		beeLevelTurbineBullet.transform.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(rotation))
		beeLevelTurbineBullet.sprite.flipX = onRight
		Return beeLevelTurbineBullet
	End Function

	' Token: 0x060017B1 RID: 6065 RVA: 0x000D5645 File Offset: 0x000D3A45
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.StartCoroutine(Me.trail_cr())
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x060017B2 RID: 6066 RVA: 0x000D5667 File Offset: 0x000D3A67
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x060017B3 RID: 6067 RVA: 0x000D5685 File Offset: 0x000D3A85
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x060017B4 RID: 6068 RVA: 0x000D56A4 File Offset: 0x000D3AA4
	Private Iterator Function move_cr() As IEnumerator
		While MyBase.transform.position.y < 360F - Me.loopSizeY
			MyBase.transform.position += Me.direction * Me.velocity * CupheadTime.Delta
			Yield Nothing
		End While
		MyBase.StartCoroutine(Me.move_in_circle_cr())
		Yield Nothing
		Return
	End Function

	' Token: 0x060017B5 RID: 6069 RVA: 0x000D56C0 File Offset: 0x000D3AC0
	Private Iterator Function move_in_circle_cr() As IEnumerator
		Me.pivotPoint = MyBase.transform.position + Vector3.right * If((Not Me.onRight), Me.loopSizeX, (-Me.loopSizeX))
		Dim handleRotationX As Vector3 = Vector3.zero
		Dim offset As Single = 100F
		Me.circleAngle -= 1.5707964F
		Dim endPos As Single
		Dim endVelocity As Single
		Dim rotateInCir As Single
		If Me.onRight Then
			endPos = -640F - offset
			endVelocity = -Me.velocity
			rotateInCir = -90F
		Else
			endPos = 640F + offset
			endVelocity = Me.velocity
			rotateInCir = 90F
		End If
		While Me.circleAngle < 6.108652F
			Me.circleAngle += Me.properties.bulletCircleTime * CupheadTime.Delta
			If Me.onRight Then
				handleRotationX = New Vector3(-Mathf.Sin(Me.circleAngle) * Me.loopSizeX, 0F, 0F)
			Else
				handleRotationX = New Vector3(Mathf.Sin(Me.circleAngle) * Me.loopSizeX, 0F, 0F)
			End If
			Dim handleRotationY As Vector3 = New Vector3(0F, Mathf.Cos(Me.circleAngle) * Me.loopSizeY, 0F)
			MyBase.transform.position = Me.pivotPoint
			MyBase.transform.position += handleRotationX + handleRotationY
			Dim dir As Vector3 = Me.pivotPoint - MyBase.transform.position
			MyBase.transform.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(MathUtils.DirectionToAngle(dir) + rotateInCir))
			Yield Nothing
		End While
		While MyBase.transform.position.x <> endPos
			MyBase.transform.AddPosition(endVelocity * CupheadTime.Delta, 0F, 0F)
			Yield Nothing
		End While
		Me.Die()
		Yield Nothing
		Return
	End Function

	' Token: 0x060017B6 RID: 6070 RVA: 0x000D56DC File Offset: 0x000D3ADC
	Private Iterator Function trail_cr() As IEnumerator
		While True
			Me.trailPrefab.Create(MyBase.transform.position)
			Yield CupheadTime.WaitForSeconds(Me, 0.25F)
		End While
		Return
	End Function

	' Token: 0x060017B7 RID: 6071 RVA: 0x000D56F7 File Offset: 0x000D3AF7
	Protected Overrides Sub Die()
		MyBase.Die()
		MyBase.transform.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(0F))
	End Sub

	' Token: 0x040020DD RID: 8413
	<SerializeField()>
	Private trailPrefab As Effect

	' Token: 0x040020DE RID: 8414
	<SerializeField()>
	Private sprite As SpriteRenderer

	' Token: 0x040020DF RID: 8415
	Private properties As LevelProperties.Bee.TurbineBlasters

	' Token: 0x040020E0 RID: 8416
	Private velocity As Single

	' Token: 0x040020E1 RID: 8417
	Private circleAngle As Single

	' Token: 0x040020E2 RID: 8418
	Private loopSizeY As Single = 200F

	' Token: 0x040020E3 RID: 8419
	Private loopSizeX As Single = 500F

	' Token: 0x040020E4 RID: 8420
	Private onRight As Boolean

	' Token: 0x040020E5 RID: 8421
	Private direction As Vector3

	' Token: 0x040020E6 RID: 8422
	Private pivotPoint As Vector3
End Class
