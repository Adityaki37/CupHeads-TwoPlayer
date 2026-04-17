Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000778 RID: 1912
Public Class RobotLevelRobotHatch
	Inherits RobotLevelRobotBodyPart

	' Token: 0x060029D1 RID: 10705 RVA: 0x00186E40 File Offset: 0x00185240
	Public Overrides Sub InitBodyPart(parent As RobotLevelRobot, properties As LevelProperties.Robot, Optional primaryHP As Integer = 0, Optional secondaryHP As Integer = 1, Optional attackDelayMinus As Single = 0F)
		Me.primaryAttackDelay = properties.CurrentState.shotBot.initialSpawnDelay.RandomFloat()
		Me.secondaryAttackDelay = properties.CurrentState.bombBot.bombDelay
		primaryHP = properties.CurrentState.shotBot.hatchGateHealth
		attackDelayMinus = properties.CurrentState.shotBot.shotbotSpawnDelayMinus
		Me.shotbotSpawnDelay = properties.CurrentState.shotBot.shotbotDelay
		MyBase.InitBodyPart(parent, properties, primaryHP, secondaryHP, attackDelayMinus)
		MyBase.animator.Play("Closed", 0, 0.75F)
		MyBase.animator.Play("Loop", 1, 0.75F)
		MyBase.animator.Play("Loop", 2, 0.75F)
		MyBase.animator.Play("Loop", 3, 0.75F)
		Me.StartPrimary()
		Me.damageEffectRenderer = Me.damageEffect.GetComponent(Of SpriteRenderer)()
	End Sub

	' Token: 0x060029D2 RID: 10706 RVA: 0x00186F34 File Offset: 0x00185334
	Protected Overrides Sub OnPrimaryAttack()
		If Me.current = RobotLevelRobotBodyPart.state.primary Then
			MyBase.StartCoroutine(Me.openHatch_cr())
			Me.primaryAttackDelay = Me.properties.CurrentState.shotBot.shotbotWaveDelay.RandomFloat()
			MyBase.OnPrimaryAttack()
		End If
	End Sub

	' Token: 0x060029D3 RID: 10707 RVA: 0x00186F74 File Offset: 0x00185374
	Private Iterator Function openHatch_cr() As IEnumerator
		Dim elapsedTime As Single = MyBase.animator.GetCurrentAnimatorStateInfo(2).length
		Dim normalizedTime As Single = Me.parent.animator.GetCurrentAnimatorStateInfo(7).normalizedTime
		normalizedTime = normalizedTime Mod 1F
		Dim delay As Single = normalizedTime * 24F
		Dim currentFrame As Integer = CInt((delay / 24F))
		If currentFrame < 2 Then
			delay = CSng(((2 - currentFrame) / 24)) * elapsedTime
			Me.nearestEventFrame = 2
		ElseIf currentFrame < 14 Then
			delay = CSng(((14 - currentFrame) / 24)) * elapsedTime
			Me.nearestEventFrame = 14
		Else
			delay = CSng((24 - currentFrame)) * elapsedTime
			delay += CSng((2 - currentFrame)) * elapsedTime
			Me.nearestEventFrame = 2
		End If
		Yield CupheadTime.WaitForSeconds(Me, delay)
		Yield Nothing
		normalizedTime = Me.parent.animator.GetCurrentAnimatorStateInfo(7).normalizedTime
		If Me.nearestEventFrame = 2 Then
			MyBase.animator.SetTrigger("IsOpenFrame2")
		ElseIf Me.nearestEventFrame = 14 Then
			MyBase.animator.SetTrigger("IsOpenFrame14")
		End If
		If Me.current = RobotLevelRobotBodyPart.state.primary Then
		End If
		Return
	End Function

	' Token: 0x060029D4 RID: 10708 RVA: 0x00186F90 File Offset: 0x00185390
	Private Sub Open()
		If Me.current <> RobotLevelRobotBodyPart.state.secondary Then
			MyBase.GetComponent(Of SpriteRenderer)().enabled = True
			For Each spriteRenderer As SpriteRenderer In MyBase.transform.GetComponentsInChildren(Of SpriteRenderer)()
				spriteRenderer.enabled = True
			Next
		End If
	End Sub

	' Token: 0x060029D5 RID: 10709 RVA: 0x00186FE0 File Offset: 0x001853E0
	Private Sub Close()
		If Me.current <> RobotLevelRobotBodyPart.state.secondary Then
			MyBase.GetComponent(Of SpriteRenderer)().enabled = False
			For Each spriteRenderer As SpriteRenderer In MyBase.transform.GetComponentsInChildren(Of SpriteRenderer)()
				spriteRenderer.enabled = False
			Next
		End If
	End Sub

	' Token: 0x060029D6 RID: 10710 RVA: 0x00187030 File Offset: 0x00185430
	Private Iterator Function closeHatch_cr() As IEnumerator
		MyBase.GetComponent(Of SpriteRenderer)().enabled = True
		MyBase.animator.SetTrigger("IsClosing")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, True)
		Yield Nothing
		If Me.current = RobotLevelRobotBodyPart.state.primary Then
			Me.isAttacking = False
		End If
		Yield Nothing
		Return
	End Function

	' Token: 0x060029D7 RID: 10711 RVA: 0x0018704B File Offset: 0x0018544B
	Private Sub SpawnShotbotWave()
		MyBase.StartCoroutine(Me.spawnShotbotWave_cr())
	End Sub

	' Token: 0x060029D8 RID: 10712 RVA: 0x0018705C File Offset: 0x0018545C
	Private Iterator Function spawnShotbotWave_cr() As IEnumerator
		For i As Integer = 0 To Me.properties.CurrentState.shotBot.shotbotCount - 1
			Dim shotbot As GameObject = Global.UnityEngine.[Object].Instantiate(Of GameObject)(Me.primary, MyBase.transform.position + Vector3.right * 80F + Vector3.down * 20F, Quaternion.identity)
			shotbot.GetComponent(Of RobotLevelHatchShotbot)().InitShotbot(Me.properties.CurrentState.shotBot.shotbotHealth, Me.properties.CurrentState.shotBot.bulletSpeed, Me.properties.CurrentState.shotBot.pinkBulletCount, Me.properties.CurrentState.shotBot.shotbotShootDelay, Me.properties.CurrentState.shotBot.shotbotFlightSpeed)
			Yield CupheadTime.WaitForSeconds(Me, Me.shotbotSpawnDelay)
			If Me.current <> RobotLevelRobotBodyPart.state.primary Then
				Exit For
			End If
		Next
		Yield CupheadTime.WaitForSeconds(Me, 0.4F)
		MyBase.StartCoroutine(Me.closeHatch_cr())
		Return
	End Function

	' Token: 0x060029D9 RID: 10713 RVA: 0x00187078 File Offset: 0x00185478
	Protected Overrides Sub OnSecondaryAttack()
		Dim homingProjectile As HomingProjectile = Me.secondary.GetComponent(Of RobotLevelHatchBombBot)().Create(MyBase.transform.position, 180F, CSng(Me.properties.CurrentState.bombBot.initialBombMovementSpeed), CSng(Me.properties.CurrentState.bombBot.bombHomingSpeed), Me.properties.CurrentState.bombBot.bombRotationSpeed, CSng(Me.properties.CurrentState.bombBot.bombLifeTime), Me.properties.CurrentState.bombBot.bombInitialMovementDuration.RandomFloat(), 4F, PlayerManager.GetNext())
		homingProjectile.GetComponent(Of RobotLevelHatchBombBot)().InitBombBot(Me.properties.CurrentState.bombBot)
		homingProjectile.transform.right = Vector3.down
		If Me.currentHealth(1) <= 0F Then
			MyBase.gameObject.SetActive(False)
			Me.StopAllCoroutines()
		End If
		MyBase.OnSecondaryAttack()
	End Sub

	' Token: 0x060029DA RID: 10714 RVA: 0x0018717C File Offset: 0x0018557C
	Protected Overrides Sub OnPrimaryDeath()
		If Me.current <> RobotLevelRobotBodyPart.state.secondary AndAlso Me.currentHealth(0) <= 0F Then
			AudioManager.Play("robot_lower_chest_port_destroyed")
			Me.emitAudioFromObject.Add("robot_lower_chest_port_destroyed")
			MyBase.animator.Play("Off")
			MyBase.GetComponent(Of BoxCollider2D)().enabled = False
			Me.StartSecondary()
			Me.DeathEffect()
			MyBase.StopCoroutine(Me.openHatch_cr())
			MyBase.StopCoroutine(Me.closeHatch_cr())
			MyBase.enabled = False
			For Each spriteRenderer As SpriteRenderer In MyBase.transform.GetComponentsInChildren(Of SpriteRenderer)()
				spriteRenderer.enabled = False
			Next
			For Each gameObject As GameObject In Me.damagedHatches
				gameObject.SetActive(True)
				gameObject.GetComponent(Of SpriteRenderer)().enabled = True
			Next
		End If
		MyBase.OnPrimaryDeath()
	End Sub

	' Token: 0x060029DB RID: 10715 RVA: 0x00187274 File Offset: 0x00185674
	Protected Overrides Sub ExitCurrentAttacks()
		If Me.current = RobotLevelRobotBodyPart.state.primary Then
			MyBase.StopCoroutine(Me.openHatch_cr())
			MyBase.StartCoroutine(Me.closeHatch_cr())
		End If
		If Me.current = RobotLevelRobotBodyPart.state.secondary Then
			MyBase.StopCoroutine(Me.secondaryAttack_cr())
		End If
		MyBase.ExitCurrentAttacks()
	End Sub

	' Token: 0x060029DC RID: 10716 RVA: 0x001872C3 File Offset: 0x001856C3
	Public Sub InitAnims()
		MyBase.animator.SetTrigger("OnRobotIntro")
	End Sub

	' Token: 0x060029DD RID: 10717 RVA: 0x001872D8 File Offset: 0x001856D8
	Protected Overrides Sub Die()
		If Me.damageEffectRoutine IsNot Nothing Then
			MyBase.StopCoroutine(Me.damageEffectRoutine)
		End If
		Me.damageEffect.SetActive(False)
		For Each spriteRenderer As SpriteRenderer In MyBase.transform.GetComponentsInChildren(Of SpriteRenderer)()
			spriteRenderer.enabled = False
		Next
		MyBase.Die()
	End Sub

	' Token: 0x060029DE RID: 10718 RVA: 0x00187339 File Offset: 0x00185739
	Protected Overrides Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		MyBase.OnDamageTaken(info)
		If Me.damageEffectRoutine IsNot Nothing Then
			MyBase.StopCoroutine(Me.damageEffectRoutine)
		End If
		Me.damageEffectRoutine = Me.damageEffect_cr()
		MyBase.StartCoroutine(Me.damageEffectRoutine)
	End Sub

	' Token: 0x060029DF RID: 10719 RVA: 0x00187374 File Offset: 0x00185774
	Private Iterator Function damageEffect_cr() As IEnumerator
		For i As Integer = 0 To 3 - 1
			Me.damageEffectRenderer.enabled = True
			Me.damageEffect.SetActive(True)
			Yield CupheadTime.WaitForSeconds(Me, 0.041666668F)
			Me.damageEffect.SetActive(False)
			Yield CupheadTime.WaitForSeconds(Me, 0.041666668F)
		Next
		Return
	End Function

	' Token: 0x040032C3 RID: 12995
	Private shotbotSpawnDelay As Single

	' Token: 0x040032C4 RID: 12996
	Private nearestEventFrame As Integer

	' Token: 0x040032C5 RID: 12997
	<SerializeField()>
	Private damagedHatches As GameObject()

	' Token: 0x040032C6 RID: 12998
	<SerializeField()>
	Private damageEffect As GameObject

	' Token: 0x040032C7 RID: 12999
	Private damageEffectRoutine As IEnumerator

	' Token: 0x040032C8 RID: 13000
	Private damageEffectRenderer As SpriteRenderer
End Class
