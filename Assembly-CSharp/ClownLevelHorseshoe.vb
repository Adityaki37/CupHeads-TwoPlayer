Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000569 RID: 1385
Public Class ClownLevelHorseshoe
	Inherits AbstractProjectile

	' Token: 0x06001A27 RID: 6695 RVA: 0x000EEED0 File Offset: 0x000ED2D0
	Public Sub Init(pos As Vector2, velocityX As Single, velocityY As Single, onRight As Boolean, durationBeforeDrop As Single, properties As LevelProperties.Clown.Horse, horseType As ClownLevelClownHorse.HorseType)
		MyBase.transform.position = pos
		Me.velocityX = velocityX
		Me.velocityY = velocityY
		Me.properties = properties
		Me.onRight = onRight
		Me.durationBeforeDrop = durationBeforeDrop
		If horseType <> ClownLevelClownHorse.HorseType.Wave Then
			If horseType = ClownLevelClownHorse.HorseType.Drop Then
				MyBase.StartCoroutine(Me.move_to_drop_point_cr())
				Me.selectedSparkle = Me.yellowSparkle
				MyBase.animator.SetInteger("type", 0)
			End If
		Else
			MyBase.StartCoroutine(Me.wave_cr())
			If MyBase.CanParry Then
				MyBase.animator.SetInteger("type", 2)
				Me.selectedSparkle = Me.pinkSparkle
			Else
				MyBase.animator.SetInteger("type", 1)
				Me.selectedSparkle = Me.greenSparkle
			End If
		End If
		MyBase.StartCoroutine(Me.spawn_sparkle_cr())
	End Sub

	' Token: 0x06001A28 RID: 6696 RVA: 0x000EEFC5 File Offset: 0x000ED3C5
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06001A29 RID: 6697 RVA: 0x000EEFE3 File Offset: 0x000ED3E3
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06001A2A RID: 6698 RVA: 0x000EF004 File Offset: 0x000ED404
	Private Iterator Function wave_cr() As IEnumerator
		Dim angle As Single = 0F
		Dim speed As Single = 0F
		Dim loopSize As Single = 0F
		Dim moveX As Vector3 = MyBase.transform.position
		Dim edge As Single = If((Not Me.onRight), 690F, (-690F))
		speed = If((Not Me.onRight), Me.velocityX, (-Me.velocityX))
		While If((Not Me.onRight), (MyBase.transform.position.x < edge), (MyBase.transform.position.x > edge))
			If Me.velocityY < 0F Then
				loopSize = -Me.properties.WaveBulletAmount
			Else
				loopSize = Me.properties.WaveBulletAmount
			End If
			angle += Me.velocityY * CupheadTime.Delta
			Dim moveY As Vector3 = New Vector3(0F, Mathf.Sin(angle + Me.properties.WaveBulletAmount) * CupheadTime.Delta * 60F * loopSize / 2F)
			moveX = MyBase.transform.right * speed * CupheadTime.Delta
			MyBase.transform.position += moveX + moveY
			Yield Nothing
		End While
		Me.Die()
		Yield Nothing
		Return
	End Function

	' Token: 0x06001A2B RID: 6699 RVA: 0x000EF020 File Offset: 0x000ED420
	Private Iterator Function move_to_drop_point_cr() As IEnumerator
		Dim pos As Vector3 = MyBase.transform.position
		If Me.onRight Then
			Dim leavePos As Single = -740F
			While MyBase.transform.position.x > leavePos
				MyBase.transform.AddPosition(-Me.velocityX * CupheadTime.Delta, 0F, 0F)
				Yield Nothing
			End While
			pos.x = -740F
		Else
			Dim leavePos As Single = 740F
			While MyBase.transform.position.x < leavePos
				MyBase.transform.AddPosition(Me.velocityX * CupheadTime.Delta, 0F, 0F)
				Yield Nothing
			End While
			pos.x = 740F
		End If
		pos.y = 260F
		MyBase.transform.position = pos
		Dim dropPos As Single = If(Me.onRight, (640F - Me.velocityY), (-640F + Me.velocityY))
		MyBase.animator.SetTrigger("onTop")
		Yield CupheadTime.WaitForSeconds(Me, Me.properties.DropBulletDelay)
		While MyBase.transform.position.x <> dropPos
			pos.x = Mathf.MoveTowards(MyBase.transform.position.x, dropPos, Me.velocityX * CupheadTime.Delta)
			MyBase.transform.position = pos
			Yield Nothing
		End While
		Me.isSparkling = False
		Yield CupheadTime.WaitForSeconds(Me, Me.durationBeforeDrop)
		Me.isSparkling = True
		MyBase.animator.SetTrigger("down")
		AudioManager.Play("clown_horseshoe_drop")
		Me.emitAudioFromObject.Add("clown_horseshoe_drop")
		While MyBase.transform.position.y > CSng(Level.Current.Ground)
			pos.y -= Me.properties.DropBulletSpeedDown * CupheadTime.Delta
			MyBase.transform.position = pos
			Yield Nothing
		End While
		AudioManager.Play("clown_horseshoe_land")
		Me.emitAudioFromObject.Add("clown_horseshoe_land")
		MyBase.animator.SetTrigger("dead")
		Me.deathPoof.Create(MyBase.transform.position)
		Yield Nothing
		Return
	End Function

	' Token: 0x06001A2C RID: 6700 RVA: 0x000EF03C File Offset: 0x000ED43C
	Public Iterator Function drop_cr() As IEnumerator
		Dim pos As Vector3 = MyBase.transform.position
		Yield Nothing
		Return
	End Function

	' Token: 0x06001A2D RID: 6701 RVA: 0x000EF058 File Offset: 0x000ED458
	Private Iterator Function simple_cr() As IEnumerator
		Dim speed As Single = 0F
		Dim edge As Single = CSng(If((Not Me.onRight), 640, (-640)))
		speed = If((Not Me.onRight), Me.velocityX, (-Me.velocityX))
		While MyBase.transform.position.x <> edge
			MyBase.transform.AddPosition(speed * CupheadTime.Delta, 0F, 0F)
			Yield Nothing
		End While
		Yield Nothing
		Return
	End Function

	' Token: 0x06001A2E RID: 6702 RVA: 0x000EF074 File Offset: 0x000ED474
	Private Iterator Function spawn_sparkle_cr() As IEnumerator
		While True
			Yield CupheadTime.WaitForSeconds(Me, 0.1F)
			If Me.isSparkling Then
				Me.selectedSparkle.Create(MyBase.transform.position)
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06001A2F RID: 6703 RVA: 0x000EF08F File Offset: 0x000ED48F
	Protected Overrides Sub Die()
		Me.StopAllCoroutines()
		MyBase.transform.GetComponent(Of SpriteRenderer)().enabled = False
		MyBase.Die()
	End Sub

	' Token: 0x06001A30 RID: 6704 RVA: 0x000EF0AE File Offset: 0x000ED4AE
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.greenSparkle = Nothing
		Me.yellowSparkle = Nothing
		Me.pinkSparkle = Nothing
		Me.deathPoof = Nothing
	End Sub

	' Token: 0x04002345 RID: 9029
	<SerializeField()>
	Private greenSparkle As Effect

	' Token: 0x04002346 RID: 9030
	<SerializeField()>
	Private pinkSparkle As Effect

	' Token: 0x04002347 RID: 9031
	<SerializeField()>
	Private yellowSparkle As Effect

	' Token: 0x04002348 RID: 9032
	<SerializeField()>
	Private deathPoof As Effect

	' Token: 0x04002349 RID: 9033
	Private selectedSparkle As Effect

	' Token: 0x0400234A RID: 9034
	Private properties As LevelProperties.Clown.Horse

	' Token: 0x0400234B RID: 9035
	Private velocityX As Single

	' Token: 0x0400234C RID: 9036
	Private velocityY As Single

	' Token: 0x0400234D RID: 9037
	Private onRight As Boolean

	' Token: 0x0400234E RID: 9038
	Private durationBeforeDrop As Single

	' Token: 0x0400234F RID: 9039
	Private isSparkling As Boolean = True
End Class
