Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000793 RID: 1939
Public Class RumRunnersLevelMobBoss
	Inherits AbstractCollidableObject

	' Token: 0x06002B03 RID: 11011 RVA: 0x0019150A File Offset: 0x0018F90A
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.circleCollider = MyBase.GetComponent(Of CircleCollider2D)()
	End Sub

	' Token: 0x06002B04 RID: 11012 RVA: 0x00191520 File Offset: 0x0018F920
	Public Sub Setup(properties As LevelProperties.RumRunners, anteater As RumRunnersLevelAnteater, positioner As Transform)
		Me.properties = properties
		Me.anteater = anteater
		Me.positioner = positioner
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		Me.parryString = New PatternString(properties.CurrentState.boss.bossProjectileParryString, True)
	End Sub

	' Token: 0x06002B05 RID: 11013 RVA: 0x00191584 File Offset: 0x0018F984
	Public Sub Begin()
		Me.begun = True
		MyBase.gameObject.SetActive(True)
		Me.setActiveDirection(RumRunnersLevelMobBoss.Direction.Attack0)
		MyBase.animator.Update(0F)
		MyBase.StartCoroutine(Me.timer_cr())
		MyBase.StartCoroutine(Me.shoot_cr())
	End Sub

	' Token: 0x06002B06 RID: 11014 RVA: 0x001915D5 File Offset: 0x0018F9D5
	Private Sub LateUpdate()
		If Not Me.begun Then
			Return
		End If
		Me.updatePosition()
	End Sub

	' Token: 0x06002B07 RID: 11015 RVA: 0x001915EC File Offset: 0x0018F9EC
	Private Iterator Function shoot_cr() As IEnumerator
		Dim p As LevelProperties.RumRunners.Boss = Me.properties.CurrentState.boss
		Yield CupheadTime.WaitForSeconds(Me, p.initialDelay)
		While True
			Dim player As AbstractPlayerController = PlayerManager.GetNext()
			Me.targetedPlayer = player.id
			Me.targetedPosition = player.center
			Dim bossCenter As Vector3 = Me.circleCollider.bounds.center
			Dim angle As Single = MathUtils.DirectionToAngle(Me.targetedPosition - bossCenter)
			If(Me.shootingRight AndAlso Me.targetedPosition.x < bossCenter.x) OrElse (Not Me.shootingRight AndAlso Me.targetedPosition.x > bossCenter.x) Then
				MyBase.animator.SetTrigger("Turn")
			End If
			Me.targetedDirection = Me.chooseDirection(angle, True)
			Me.setActiveDirection(Me.targetedDirection)
			MyBase.animator.SetTrigger("Attack")
			Dim animatorHash As Integer = Animator.StringToHash("AttackMiddle")
			While Me.getAnimatorCurrentStateInfo().shortNameHash <> animatorHash
				Yield Nothing
			End While
			While Me.getAnimatorCurrentStateInfo().normalizedTime < 1F
				Yield Nothing
			End While
			Me.shoot()
			MyBase.animator.SetTrigger("Continue")
			animatorHash = Animator.StringToHash("AttackEnd")
			While Me.getAnimatorCurrentStateInfo().shortNameHash <> animatorHash
				Yield Nothing
			End While
			While Me.getAnimatorCurrentStateInfo().shortNameHash = animatorHash
				Yield Nothing
			End While
			Dim totalAttackDelay As Single = p.coinDelay.GetFloatAt(Me.minMaxParameter)
			If totalAttackDelay > 0.6666667F Then
				If totalAttackDelay <= 0.7083333F Then
					MyBase.animator.SetFloat(RumRunnersLevelMobBoss.StartSpeedParameter, 1.2F)
				End If
				Dim waitTime As Single = totalAttackDelay - 0.6666667F
				If waitTime > 0F Then
					Yield CupheadTime.WaitForSeconds(Me, waitTime)
				End If
			Else
				Dim num As Single
				Dim num2 As Single
				If totalAttackDelay > 0.5833333F Then
					num = 1.2F
					num2 = 1F
				ElseIf totalAttackDelay > 0.5416667F Then
					num = 1.2F
					num2 = 1.3333334F
				ElseIf totalAttackDelay > 0.5F Then
					num = 1.5F
					num2 = 1.3333334F
				ElseIf totalAttackDelay > 0.45833334F Then
					num = 2F
					num2 = 1.3333334F
				Else
					num = 2F
					num2 = 2F
				End If
				MyBase.animator.SetFloat(RumRunnersLevelMobBoss.StartSpeedParameter, num)
				MyBase.animator.SetFloat(RumRunnersLevelMobBoss.EndSpeedParameter, num2)
			End If
		End While
		Return
	End Function

	' Token: 0x06002B08 RID: 11016 RVA: 0x00191608 File Offset: 0x0018FA08
	Private Iterator Function timer_cr() As IEnumerator
		Dim p As LevelProperties.RumRunners.Boss = Me.properties.CurrentState.boss
		Dim totalTime As Single = p.coinMinMaxTime
		Dim elapsedTime As Single = 0F
		While elapsedTime < totalTime
			elapsedTime += CupheadTime.Delta
			Me.minMaxParameter = Mathf.Clamp01(elapsedTime / totalTime)
			Yield Nothing
		End While
		Me.minMaxParameter = 1F
		Return
	End Function

	' Token: 0x06002B09 RID: 11017 RVA: 0x00191623 File Offset: 0x0018FA23
	Private Sub die()
		If Me.dead Then
			Return
		End If
		Me.dead = True
		Me.StopAllCoroutines()
		MyBase.gameObject.SetActive(False)
		Me.anteater.RealDeath()
	End Sub

	' Token: 0x06002B0A RID: 11018 RVA: 0x00191655 File Offset: 0x0018FA55
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.properties.DealDamage(info.damage)
		If Me.properties.CurrentHealth <= 0F AndAlso Not Me.dead Then
			Me.die()
		End If
	End Sub

	' Token: 0x06002B0B RID: 11019 RVA: 0x00191690 File Offset: 0x0018FA90
	Private Sub animationEvent_Flip()
		Me.shootingRight = Not Me.shootingRight
		Dim localScale As Vector3 = MyBase.transform.localScale
		localScale.x *= -1F
		MyBase.transform.localScale = localScale
		If PlayerManager.DoesPlayerExist(Me.targetedPlayer) Then
			Me.targetedPosition = PlayerManager.GetPlayer(Me.targetedPlayer).center
		End If
		Dim center As Vector3 = Me.circleCollider.bounds.center
		Dim num As Single = MathUtils.DirectionToAngle(Me.targetedPosition - center)
		Me.targetedDirection = Me.chooseDirection(num, True)
		Me.setActiveDirection(Me.targetedDirection)
		Me.updatePosition()
	End Sub

	' Token: 0x06002B0C RID: 11020 RVA: 0x00191748 File Offset: 0x0018FB48
	Private Sub shoot()
		Dim center As Vector3
		If PlayerManager.DoesPlayerExist(Me.targetedPlayer) Then
			center = PlayerManager.GetPlayer(Me.targetedPlayer).center
		Else
			center = Me.targetedPosition
		End If
		Dim vector As Vector3 = MyBase.transform.TransformPoint(Me.projectileRoots(CInt(Me.targetedDirection)))
		Dim num As Single = MathUtils.DirectionToAngle(center - vector)
		Dim num2 As Single = If((Not Me.shootingRight), RumRunnersLevelMobBoss.ReferenceAnglesLeft(CInt(Me.targetedDirection)), RumRunnersLevelMobBoss.ReferenceAnglesRight(CInt(Me.targetedDirection)))
		Dim num3 As Single = If((Not Me.shootingRight), If((num <= 0F), (-180F - num), (180F - num)), num)
		Dim num4 As Single = If((Not Me.shootingRight), If((num2 <= 0F), (-180F - num2), (180F - num2)), num2)
		If Mathf.Abs(num3 - num4) > RumRunnersLevelMobBoss.AcceptableAngleVariance Then
			Dim direction As RumRunnersLevelMobBoss.Direction = Me.chooseDirection(num, True)
			Dim num5 As Integer = direction - Me.targetedDirection
			If Mathf.Abs(num5) > 1 Then
				num5 = CInt(Mathf.Sign(CSng(num5)))
				Me.targetedDirection = CType(Mathf.Clamp(CInt((Me.targetedDirection + num5)), 0, 8), RumRunnersLevelMobBoss.Direction)
			Else
				Me.targetedDirection = direction
			End If
			Me.setActiveDirection(Me.targetedDirection)
			vector = MyBase.transform.TransformPoint(Me.projectileRoots(CInt(Me.targetedDirection)))
		End If
		If Me.shootingRight Then
			num2 = RumRunnersLevelMobBoss.ReferenceAnglesRight(CInt(Me.targetedDirection))
			num = Mathf.Clamp(num, num2 - RumRunnersLevelMobBoss.AcceptableAngleVariance, num2 + RumRunnersLevelMobBoss.AcceptableAngleVariance)
		ElseIf Me.targetedDirection = RumRunnersLevelMobBoss.Direction.Attack0 Then
			If num < 0F Then
				num = Mathf.Clamp(num, -180F - RumRunnersLevelMobBoss.AcceptableAngleVariance, -180F + RumRunnersLevelMobBoss.AcceptableAngleVariance)
			Else
				num = Mathf.Clamp(num, 180F - RumRunnersLevelMobBoss.AcceptableAngleVariance, 180F + RumRunnersLevelMobBoss.AcceptableAngleVariance)
			End If
		Else
			num2 = RumRunnersLevelMobBoss.ReferenceAnglesLeft(CInt(Me.targetedDirection))
			num = Mathf.Clamp(num, num2 - RumRunnersLevelMobBoss.AcceptableAngleVariance, num2 + RumRunnersLevelMobBoss.AcceptableAngleVariance)
		End If
		Dim floatAt As Single = Me.properties.CurrentState.boss.coinSpeed.GetFloatAt(Me.minMaxParameter)
		Dim basicProjectile As BasicProjectile = Me.projectile.Create(vector, num, floatAt)
		Me.projectileMuzzleFX.Create(vector).transform.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(num))
		basicProjectile.SetParryable(Me.parryString.PopLetter() = "P"c)
		Me.SFX_RUMRUN_P4_Snail_ProjectileShoot()
	End Sub

	' Token: 0x06002B0D RID: 11021 RVA: 0x00191A10 File Offset: 0x0018FE10
	Private Sub updatePosition()
		Dim vector As Vector3 = Me.positioner.position
		If Not Me.shootingRight Then
			vector += Me.flippedOffset
		End If
		MyBase.transform.position = vector + Me.positionOffset
	End Sub

	' Token: 0x06002B0E RID: 11022 RVA: 0x00191A64 File Offset: 0x0018FE64
	Private Function chooseDirection(angle As Single, canOvershoot As Boolean) As RumRunnersLevelMobBoss.Direction
		Dim direction As RumRunnersLevelMobBoss.Direction
		If Me.shootingRight Then
			If angle > 33.75F Then
				direction = RumRunnersLevelMobBoss.Direction.Attack45
			ElseIf angle > 11.25F Then
				direction = RumRunnersLevelMobBoss.Direction.Attack22
			ElseIf angle > -11.25F Then
				direction = RumRunnersLevelMobBoss.Direction.Attack0
			ElseIf angle > -33.75F Then
				direction = RumRunnersLevelMobBoss.Direction.Attack337
			ElseIf angle > -56.25F Then
				direction = RumRunnersLevelMobBoss.Direction.Attack315
			ElseIf angle > -78.75F Then
				direction = RumRunnersLevelMobBoss.Direction.Attack292
			ElseIf Not canOvershoot Then
				direction = RumRunnersLevelMobBoss.Direction.Attack270
			ElseIf angle > -101.25F Then
				direction = RumRunnersLevelMobBoss.Direction.Attack270
			Else
				direction = RumRunnersLevelMobBoss.Direction.Attack247
			End If
		ElseIf angle >= 168.75F OrElse angle <= -168.75F Then
			direction = RumRunnersLevelMobBoss.Direction.Attack0
		ElseIf angle < -146.25F Then
			direction = RumRunnersLevelMobBoss.Direction.Attack337
		ElseIf angle < -123.75F Then
			direction = RumRunnersLevelMobBoss.Direction.Attack315
		ElseIf angle < -101.25F Then
			direction = RumRunnersLevelMobBoss.Direction.Attack292
		ElseIf(Not canOvershoot AndAlso angle < 0F) OrElse (canOvershoot AndAlso angle < -78.75F) Then
			direction = RumRunnersLevelMobBoss.Direction.Attack270
		ElseIf canOvershoot AndAlso angle < 0F Then
			direction = RumRunnersLevelMobBoss.Direction.Attack247
		ElseIf angle < 168.75F Then
			direction = RumRunnersLevelMobBoss.Direction.Attack22
		Else
			direction = RumRunnersLevelMobBoss.Direction.Attack45
		End If
		Return direction
	End Function

	' Token: 0x06002B0F RID: 11023 RVA: 0x00191BB7 File Offset: 0x0018FFB7
	Private Function getAnimatorCurrentStateInfo() As AnimatorStateInfo
		Return MyBase.animator.GetCurrentAnimatorStateInfo(CInt((Me.targetedDirection + 1)))
	End Function

	' Token: 0x06002B10 RID: 11024 RVA: 0x00191BCC File Offset: 0x0018FFCC
	Private Sub setActiveDirection(direction As RumRunnersLevelMobBoss.Direction)
		For i As Integer = 1 To 8
			Dim num As Single = If((direction <> CType((i - 1), RumRunnersLevelMobBoss.Direction)), 0F, 1F)
			MyBase.animator.SetLayerWeight(i, num)
		Next
	End Sub

	' Token: 0x06002B11 RID: 11025 RVA: 0x00191C11 File Offset: 0x00190011
	Private Sub SFX_RUMRUN_P4_Snail_ProjectileShoot()
		AudioManager.Play("sfx_dlc_rumrun_p4_snail_projectile_shoot")
	End Sub

	' Token: 0x040033B3 RID: 13235
	Private Shared AcceptableAngleVariance As Single = 15F

	' Token: 0x040033B4 RID: 13236
	Private Shared ReferenceAnglesRight As Single() = New Single() { 45F, 22.5F, 0F, -22.5F, -45F, -67.5F, -90F, -112.5F }

	' Token: 0x040033B5 RID: 13237
	Private Shared ReferenceAnglesLeft As Single() = New Single() { 135F, 157.5F, 180F, -157.5F, -135F, -112.5F, -90F, -67.5F }

	' Token: 0x040033B6 RID: 13238
	Private Shared StartSpeedParameter As Integer = Animator.StringToHash("StartSpeed")

	' Token: 0x040033B7 RID: 13239
	Private Shared EndSpeedParameter As Integer = Animator.StringToHash("EndSpeed")

	' Token: 0x040033B8 RID: 13240
	<SerializeField()>
	Private projectile As BasicProjectile

	' Token: 0x040033B9 RID: 13241
	<SerializeField()>
	Private projectileMuzzleFX As Effect

	' Token: 0x040033BA RID: 13242
	<SerializeField()>
	Private projectileRoots As Vector2()

	' Token: 0x040033BB RID: 13243
	<SerializeField()>
	Private positionOffset As Vector2

	' Token: 0x040033BC RID: 13244
	<SerializeField()>
	Private flippedOffset As Vector2

	' Token: 0x040033BD RID: 13245
	Private begun As Boolean

	' Token: 0x040033BE RID: 13246
	Private dead As Boolean

	' Token: 0x040033BF RID: 13247
	Private minMaxParameter As Single

	' Token: 0x040033C0 RID: 13248
	Private shootingRight As Boolean = True

	' Token: 0x040033C1 RID: 13249
	Private parryString As PatternString

	' Token: 0x040033C2 RID: 13250
	Private targetedPlayer As PlayerId

	' Token: 0x040033C3 RID: 13251
	Private targetedPosition As Vector3

	' Token: 0x040033C4 RID: 13252
	Private targetedDirection As RumRunnersLevelMobBoss.Direction

	' Token: 0x040033C5 RID: 13253
	Private properties As LevelProperties.RumRunners

	' Token: 0x040033C6 RID: 13254
	Private anteater As RumRunnersLevelAnteater

	' Token: 0x040033C7 RID: 13255
	Private positioner As Transform

	' Token: 0x040033C8 RID: 13256
	Private damageReceiver As DamageReceiver

	' Token: 0x040033C9 RID: 13257
	Private circleCollider As CircleCollider2D

	' Token: 0x02000794 RID: 1940
	Private Enum Direction
		' Token: 0x040033CB RID: 13259
		Attack45
		' Token: 0x040033CC RID: 13260
		Attack22
		' Token: 0x040033CD RID: 13261
		Attack0
		' Token: 0x040033CE RID: 13262
		Attack337
		' Token: 0x040033CF RID: 13263
		Attack315
		' Token: 0x040033D0 RID: 13264
		Attack292
		' Token: 0x040033D1 RID: 13265
		Attack270
		' Token: 0x040033D2 RID: 13266
		Attack247
		' Token: 0x040033D3 RID: 13267
		AttackCount
	End Enum
End Class
