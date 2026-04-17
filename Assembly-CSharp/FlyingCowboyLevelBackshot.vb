Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000649 RID: 1609
Public Class FlyingCowboyLevelBackshot
	Inherits BasicUprightProjectile

	' Token: 0x06002107 RID: 8455 RVA: 0x00131524 File Offset: 0x0012F924
	Public Overridable Function Create(position As Vector3, rotation As Single, speed As Single, bulletSpeed As Single, health As Single, anticipationStartDistance As Single, childParryable As Boolean) As BasicProjectile
		Dim flyingCowboyLevelBackshot As FlyingCowboyLevelBackshot = TryCast(Me.Create(position, rotation, speed), FlyingCowboyLevelBackshot)
		flyingCowboyLevelBackshot.bulletSpeed = bulletSpeed
		flyingCowboyLevelBackshot.StartCoroutine(flyingCowboyLevelBackshot.waitToShoot_cr(speed, anticipationStartDistance))
		flyingCowboyLevelBackshot.health = health
		flyingCowboyLevelBackshot.childParryable = childParryable
		Return flyingCowboyLevelBackshot
	End Function

	' Token: 0x06002108 RID: 8456 RVA: 0x0013156E File Offset: 0x0012F96E
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
	End Sub

	' Token: 0x06002109 RID: 8457 RVA: 0x0013159C File Offset: 0x0012F99C
	Protected Overrides Sub FixedUpdate()
		MyBase.FixedUpdate()
		If MyBase.transform.position.x < FlyingCowboyLevelBackshot.AttackPosition Then
			MyBase.transform.SetPosition(New Single?(FlyingCowboyLevelBackshot.AttackPosition), Nothing, Nothing)
		End If
	End Sub

	' Token: 0x0600210A RID: 8458 RVA: 0x001315F3 File Offset: 0x0012F9F3
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.health -= info.damage
		If Me.health < 0F AndAlso Not MyBase.dead Then
			Level.Current.RegisterMinionKilled()
			Me.Die()
		End If
	End Sub

	' Token: 0x0600210B RID: 8459 RVA: 0x00131634 File Offset: 0x0012FA34
	Protected Overrides Sub Die()
		Dim speed As Single = Me.Speed
		MyBase.Die()
		Me.StopAllCoroutines()
		MyBase.StartCoroutine(Me.death_cr(speed))
	End Sub

	' Token: 0x0600210C RID: 8460 RVA: 0x00131664 File Offset: 0x0012FA64
	Private Iterator Function death_cr(speed As Single) As IEnumerator
		Dim leftWing As Transform = Me.leftWings.GetRandom()
		leftWing.GetComponent(Of SpriteRenderer)().enabled = True
		Dim rightWing As Transform = Me.rightWings.GetRandom()
		rightWing.GetComponent(Of SpriteRenderer)().enabled = True
		MyBase.animator.Play("Death")
		MyBase.StartCoroutine(Me.moveWings_cr(speed, leftWing, rightWing))
		Me.SFX_COWGIRL_P1_HorseflySpit()
		Yield MyBase.animator.WaitForNormalizedTime(Me, 1F, "Death", 0, True, False, True)
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Return
	End Function

	' Token: 0x0600210D RID: 8461 RVA: 0x00131688 File Offset: 0x0012FA88
	Private Iterator Function moveWings_cr(speed As Single, leftWing As Transform, rightWing As Transform) As IEnumerator
		Dim wingSpeedLeft As Vector3 = New Vector2(-speed * Global.UnityEngine.Random.Range(0.25F, 0.5F), -Global.UnityEngine.Random.Range(75F, 125F))
		Dim windSpeedRight As Vector3 = New Vector2(-speed * Global.UnityEngine.Random.Range(0.25F, 0.5F), -Global.UnityEngine.Random.Range(75F, 125F))
		While True
			Yield Nothing
			Dim position As Vector3 = leftWing.position
			position += wingSpeedLeft * CupheadTime.Delta
			leftWing.position = position
			position = rightWing.position
			position += windSpeedRight * CupheadTime.Delta
			rightWing.position = position
		End While
		Return
	End Function

	' Token: 0x0600210E RID: 8462 RVA: 0x001316B4 File Offset: 0x0012FAB4
	Private Iterator Function waitToShoot_cr(speed As Single, anticipationStartDistance As Single) As IEnumerator
		Dim timeToAnticipation As Single = anticipationStartDistance / speed
		Dim remainder As Single = MathUtilities.DecimalPart(timeToAnticipation / 1F)
		Dim offset As Single = 1F - remainder
		Dim totalNormalizedTime As Single = timeToAnticipation / 1F + offset + 0.625F
		MyBase.animator.Update(0F)
		MyBase.animator.Play(0, 0, 0.625F + offset)
		Yield MyBase.animator.WaitForNormalizedTime(Me, totalNormalizedTime, "Idle", 0, False, False, True)
		MyBase.animator.Play("AnticipationStart")
		While MyBase.transform.position.x > -550F
			Yield Nothing
		End While
		MyBase.animator.SetTrigger("Attack")
		Dim initialSpeed As Single = Me.Speed
		Dim decelerationTime As Single = KinematicUtilities.CalculateTimeToChangeVelocity(initialSpeed, 0F, -550F - FlyingCowboyLevelBackshot.AttackPosition)
		Dim elapsedTime As Single = 0F
		While elapsedTime < decelerationTime
			Yield Nothing
			elapsedTime += CupheadTime.Delta
			Me.Speed = Mathf.Lerp(initialSpeed, 0F, elapsedTime / decelerationTime)
		End While
		Me.move = False
		Yield MyBase.animator.WaitForNormalizedTime(Me, 1F, "Attack", 0, True, False, True)
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Return
	End Function

	' Token: 0x0600210F RID: 8463 RVA: 0x001316E0 File Offset: 0x0012FAE0
	Private Sub animationEvent_ShootBullet()
		Dim [next] As AbstractPlayerController = PlayerManager.GetNext()
		Dim num As Single = MathUtils.DirectionToAngle([next].center - Me.projectileSpawnPosition.position)
		Dim basicProjectile As BasicProjectile = Me.projectile.Create(Me.projectileSpawnPosition.position, num, Me.bulletSpeed)
		basicProjectile.SetParryable(Me.childParryable)
		basicProjectile.StartCoroutine(Me.growBullet(basicProjectile.transform))
	End Sub

	' Token: 0x06002110 RID: 8464 RVA: 0x00131758 File Offset: 0x0012FB58
	Private Iterator Function growBullet(transform As Transform) As IEnumerator
		transform.SetScale(New Single?(0.6F), New Single?(0.6F), Nothing)
		Dim wait As WaitForFrameTimePersistent = New WaitForFrameTimePersistent(0.041666668F, False)
		Dim elapsedTime As Single = 0F
		While elapsedTime < 0.3F
			Yield wait
			elapsedTime += wait.totalDelta
			Dim scale As Single = Mathf.Lerp(0.6F, 1F, elapsedTime / 0.3F)
			transform.SetScale(New Single?(scale), New Single?(scale), Nothing)
		End While
		Return
	End Function

	' Token: 0x06002111 RID: 8465 RVA: 0x00131773 File Offset: 0x0012FB73
	Private Sub AnimationEvent_SFX_COWGIRL_P1_HorseflySpit()
		AudioManager.Play("sfx_DLC_Cowgirl_P1_Horsefly_Spit")
		Me.emitAudioFromObject.Add("sfx_DLC_Cowgirl_P1_Horsefly_Spit")
	End Sub

	' Token: 0x06002112 RID: 8466 RVA: 0x0013178F File Offset: 0x0012FB8F
	Private Sub SFX_COWGIRL_P1_HorseflySpit()
		AudioManager.Play("sfx_DLC_Cowgirl_P1_Horsefly_Death")
		Me.emitAudioFromObject.Add("sfx_DLC_Cowgirl_P1_Horsefly_Death")
	End Sub

	' Token: 0x040029AA RID: 10666
	Private Shared AttackPosition As Single = -600F

	' Token: 0x040029AB RID: 10667
	<SerializeField()>
	Private projectile As BasicProjectile

	' Token: 0x040029AC RID: 10668
	<SerializeField()>
	Private projectileSpawnPosition As Transform

	' Token: 0x040029AD RID: 10669
	<SerializeField()>
	Private leftWings As Transform()

	' Token: 0x040029AE RID: 10670
	<SerializeField()>
	Private rightWings As Transform()

	' Token: 0x040029AF RID: 10671
	Private damageReceiver As DamageReceiver

	' Token: 0x040029B0 RID: 10672
	Private bulletSpeed As Single

	' Token: 0x040029B1 RID: 10673
	Private health As Single

	' Token: 0x040029B2 RID: 10674
	Private childParryable As Boolean
End Class
