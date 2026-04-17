Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000783 RID: 1923
Public Class RumRunnersLevelAnteater
	Inherits LevelProperties.RumRunners.Entity

	' Token: 0x06002A42 RID: 10818 RVA: 0x0018B6AE File Offset: 0x00189AAE
	Private Sub OnEnable()
		AddHandler MyBase.GetComponent(Of DamageReceiver)().OnDamageTaken, AddressOf Me.onDamageTakenEventHandler
	End Sub

	' Token: 0x06002A43 RID: 10819 RVA: 0x0018B6C7 File Offset: 0x00189AC7
	Private Sub OnDisable()
		RemoveHandler MyBase.GetComponent(Of DamageReceiver)().OnDamageTaken, AddressOf Me.onDamageTakenEventHandler
	End Sub

	' Token: 0x06002A44 RID: 10820 RVA: 0x0018B6E0 File Offset: 0x00189AE0
	Private Sub Start()
		Me.damageDealer = DamageDealer.NewEnemy()
		For Each collisionChild As CollisionChild In Me.collChildren
			AddHandler collisionChild.OnPlayerCollision, AddressOf Me.OnCollisionPlayer
		Next
		AddHandler Me.tongue.OnPlayerCollision, AddressOf Me.OnCollisionPlayer
	End Sub

	' Token: 0x06002A45 RID: 10821 RVA: 0x0018B742 File Offset: 0x00189B42
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06002A46 RID: 10822 RVA: 0x0018B75A File Offset: 0x00189B5A
	Public Sub DoDamage(damage As Single)
		MyBase.properties.DealDamage(damage)
	End Sub

	' Token: 0x06002A47 RID: 10823 RVA: 0x0018B768 File Offset: 0x00189B68
	Private Sub onDamageTakenEventHandler(info As DamageDealer.DamageInfo)
		MyBase.properties.DealDamage(info.damage)
	End Sub

	' Token: 0x06002A48 RID: 10824 RVA: 0x0018B77B File Offset: 0x00189B7B
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If Me.damageDealer IsNot Nothing AndAlso phase = CollisionPhase.Enter Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06002A49 RID: 10825 RVA: 0x0018B7A4 File Offset: 0x00189BA4
	Public Overrides Sub LevelInit(properties As LevelProperties.RumRunners)
		MyBase.LevelInit(properties)
		Me.snoutAttackPattern = New PatternString(properties.CurrentState.anteaterSnout.snoutActionArray, True, False)
		Me.snoutPositionPattern = New PatternString(properties.CurrentState.anteaterSnout.snoutPosString, True, True)
		Me.snout.Setup(properties)
	End Sub

	' Token: 0x06002A4A RID: 10826 RVA: 0x0018B7FE File Offset: 0x00189BFE
	Public Sub StartAnteater()
		MyBase.StartCoroutine(Me.snout_cr())
	End Sub

	' Token: 0x06002A4B RID: 10827 RVA: 0x0018B810 File Offset: 0x00189C10
	Private Iterator Function snout_cr() As IEnumerator
		Dim p As LevelProperties.RumRunners.AnteaterSnout = MyBase.properties.CurrentState.anteaterSnout
		While True
			Dim attackIntro As RumRunnersLevelAnteater.AttackIntro = If((Not Rand.Bool()), RumRunnersLevelAnteater.AttackIntro.B, RumRunnersLevelAnteater.AttackIntro.A)
			MyBase.animator.SetInteger("AttackIntro", CInt(attackIntro))
			Dim count As Integer = Me.snoutAttackPattern.SubStringLength()
			For i As Integer = 0 To count - 1
				Dim attackType As RumRunnersLevelSnout.AttackType
				Dim introLength As RumRunnersLevelAnteater.AttackIntroLength
				Me.parseAttackString(Me.snoutAttackPattern.GetString(), attackType, introLength)
				Me.snoutAttackPattern.IncrementString()
				Dim nextAttackType As RumRunnersLevelSnout.AttackType
				Dim nextIntroLength As RumRunnersLevelAnteater.AttackIntroLength
				Me.parseAttackString(Me.snoutAttackPattern.GetString(), nextAttackType, nextIntroLength)
				Me.queuedAttack = attackType
				If p.anticipationBoilDelay > 0F Then
					Yield CupheadTime.WaitForSeconds(Me, p.anticipationBoilDelay)
				End If
				MyBase.animator.SetTrigger("AnticipationComplete")
				While Not Me.snout.isAttacking
					Yield Nothing
				End While
				Dim endAnimationName As String
				If i = 0 Then
					endAnimationName = "AttackInitialEnd"
				Else
					Dim text As String = If((attackIntro <> RumRunnersLevelAnteater.AttackIntro.A), "AttackB.", "AttackA.")
					endAnimationName = text + "AttackEnd"
				End If
				Dim nextAttackIsFinal As Boolean = nextAttackType = RumRunnersLevelSnout.AttackType.Tongue OrElse i = count - 1
				If Not nextAttackIsFinal Then
					attackIntro = If((attackIntro <> RumRunnersLevelAnteater.AttackIntro.A), RumRunnersLevelAnteater.AttackIntro.A, RumRunnersLevelAnteater.AttackIntro.B)
				Else
					If i < count - 1 AndAlso nextAttackType = RumRunnersLevelSnout.AttackType.Tongue Then
						Me.snoutAttackPattern.IncrementString()
					End If
					attackIntro = RumRunnersLevelAnteater.AttackIntro.Final
				End If
				MyBase.animator.SetInteger("AttackIntro", CInt(attackIntro))
				MyBase.animator.SetBool("LongIntro", nextIntroLength = RumRunnersLevelAnteater.AttackIntroLength.[Long])
				While Me.snout.isAttacking
					Yield Nothing
				End While
				MyBase.animator.SetTrigger("EndAttack")
				Yield MyBase.animator.WaitForAnimationToEnd(Me, endAnimationName, False, True)
				If nextAttackIsFinal Then
					Exit For
				End If
			Next
			Me.queuedAttack = RumRunnersLevelSnout.AttackType.Tongue
			If p.anticipationBoilDelay > 0F Then
				Yield CupheadTime.WaitForSeconds(Me, p.anticipationBoilDelay)
			End If
			MyBase.animator.SetTrigger("AnticipationComplete")
			MyBase.StartCoroutine(Me.finalAttackEyes_cr())
			While Not Me.snout.isAttacking
				Yield Nothing
			End While
			While Me.snout.isAttacking
				Yield Nothing
			End While
			MyBase.animator.SetTrigger("EndAttack")
			MyBase.animator.Play("Off", RumRunnersLevelAnteater.EyesAnimationLayer, 0F)
			MyBase.animator.Update(0F)
			Yield MyBase.animator.WaitForAnimationToStart(Me, "AttackFinal.AttackEndHold", False)
			Yield CupheadTime.WaitForSeconds(Me, p.finalAttackTauntDuration)
			MyBase.animator.SetTrigger("EndTaunt")
			Yield MyBase.animator.WaitForAnimationToEnd(Me, "AttackFinal.AttackEnd", False, True)
		End While
		Return
	End Function

	' Token: 0x06002A4C RID: 10828 RVA: 0x0018B82C File Offset: 0x00189C2C
	Private Iterator Function finalAttackEyes_cr() As IEnumerator
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "AttackFinal.AttackStart", False, True)
		MyBase.animator.Play("Hold", RumRunnersLevelAnteater.EyesAnimationLayer, 0F)
		MyBase.animator.Update(0F)
		Return
	End Function

	' Token: 0x06002A4D RID: 10829 RVA: 0x0018B848 File Offset: 0x00189C48
	Public Sub FakeDeathStart()
		Me.StopAllCoroutines()
		Dim boss As LevelProperties.RumRunners.Boss = MyBase.properties.CurrentState.boss
		Me.mobBoss.Setup(MyBase.properties, Me, Me.mobBossHelperTransform)
		For Each spriteRenderer As SpriteRenderer In Me.flipRenderers
			spriteRenderer.flipX = False
		Next
		Me.snout.Death()
		MyBase.animator.Play("Off", RumRunnersLevelAnteater.EyesAnimationLayer)
		MyBase.animator.Play("Off", RumRunnersLevelAnteater.HandsAnimatorLayer)
		MyBase.animator.Play("Death")
		MyBase.animator.Update(0F)
		MyBase.GetComponent(Of AnimationHelper)().Speed = 0F
		Me.eyes.transform.localPosition = New Vector3(2F, -62F)
		MyBase.GetComponent(Of LevelBossDeathExploder)().StartExplosion(True)
	End Sub

	' Token: 0x06002A4E RID: 10830 RVA: 0x0018B93A File Offset: 0x00189D3A
	Public Sub FakeDeathContinue()
		MyBase.GetComponent(Of AnimationHelper)().Speed = 1F
	End Sub

	' Token: 0x06002A4F RID: 10831 RVA: 0x0018B94C File Offset: 0x00189D4C
	Private Iterator Function fakeDeathEyes_cr() As IEnumerator
		Dim eyesPattern As PatternString = New PatternString(MyBase.properties.CurrentState.boss.anteaterEyeClosedOpenString, True)
		While True
			Dim currentPattern As String() = eyesPattern.PopString().Split(New Char() { ":"c })
			If currentPattern.Length <> 2 Then
				Exit For
			End If
			Dim closedCount As Integer
			Parser.IntTryParse(currentPattern(0), closedCount)
			Dim openCount As Integer
			Parser.IntTryParse(currentPattern(1), openCount)
			Yield MyBase.StartCoroutine(Me.eyeHandler_cr(closedCount, "DeathLoop"))
			Yield MyBase.animator.WaitForAnimationToStart(Me, "DeathLoopEyeOpen", False)
			Yield MyBase.StartCoroutine(Me.eyeHandler_cr(openCount, "DeathLoopOpen"))
			Yield MyBase.animator.WaitForAnimationToStart(Me, "DeathLoopEyeClose", False)
		End While
		Throw New Exception("Invalid anteater eye pattern")
		Return
	End Function

	' Token: 0x06002A50 RID: 10832 RVA: 0x0018B968 File Offset: 0x00189D68
	Private Iterator Function eyeHandler_cr(count As Integer, loopAnimationName As String) As IEnumerator
		If count = 0 Then
			MyBase.animator.SetTrigger("DeathLoopEyeChange")
		Else
			Yield MyBase.animator.WaitForAnimationToStart(Me, loopAnimationName, False)
			While MyBase.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < CSng(count) - 0.5F
				Yield Nothing
			End While
			MyBase.animator.SetTrigger("DeathLoopEyeChange")
		End If
		Return
	End Function

	' Token: 0x06002A51 RID: 10833 RVA: 0x0018B991 File Offset: 0x00189D91
	Public Sub RealDeath()
		Me.StopAllCoroutines()
		MyBase.animator.Play("ActualDeath", 0, 0.2631579F)
	End Sub

	' Token: 0x06002A52 RID: 10834 RVA: 0x0018B9B0 File Offset: 0x00189DB0
	Private Sub animationEvent_LeftDirt()
		CupheadLevelCamera.Current.Shake(20F, 0.3F, True)
		CType(Level.Current, RumRunnersLevel).FullscreenDirt(1, New Single?(-640F + Global.UnityEngine.Random.Range(100F, 200F)), -1F, -1F)
	End Sub

	' Token: 0x06002A53 RID: 10835 RVA: 0x0018BA08 File Offset: 0x00189E08
	Private Sub animationEvent_RightDirt()
		CupheadLevelCamera.Current.Shake(20F, 0.3F, True)
		CType(Level.Current, RumRunnersLevel).FullscreenDirt(1, New Single?(640F - Global.UnityEngine.Random.Range(100F, 200F)), -1F, -1F)
	End Sub

	' Token: 0x06002A54 RID: 10836 RVA: 0x0018BA5E File Offset: 0x00189E5E
	Private Sub animationEvent_MiddleBridgeDestroy()
		CType(Level.Current, RumRunnersLevel).DestroyMiddleBridge()
	End Sub

	' Token: 0x06002A55 RID: 10837 RVA: 0x0018BA6F File Offset: 0x00189E6F
	Private Sub animationEvent_UpperBridgeDestroy()
		CType(Level.Current, RumRunnersLevel).DestroyUpperBridge()
	End Sub

	' Token: 0x06002A56 RID: 10838 RVA: 0x0018BA80 File Offset: 0x00189E80
	Private Sub animationEvent_BridgeShatter()
		CType(Level.Current, RumRunnersLevel).ShatterBridges()
	End Sub

	' Token: 0x06002A57 RID: 10839 RVA: 0x0018BA94 File Offset: 0x00189E94
	Private Sub animationEvent_InitialAttackStarted()
		If Me.firstAttack Then
			Me.firstAttack = False
			Return
		End If
		Me.onLeft = Not Me.onLeft
		For Each spriteRenderer As SpriteRenderer In Me.flipRenderers
			spriteRenderer.flipX = Not Me.onLeft
		Next
	End Sub

	' Token: 0x06002A58 RID: 10840 RVA: 0x0018BAF4 File Offset: 0x00189EF4
	Private Sub animationEvent_SnoutAttack()
		Dim num As Integer = Me.snoutPositionPattern.PopInt()
		Dim vector As Vector2
		vector.x = If((Not Me.onLeft), RumRunnersLevelAnteater.SNOUT_SPAWN_X, (-RumRunnersLevelAnteater.SNOUT_SPAWN_X))
		vector.y = Me.spawnPoints(num).position.y
		Me.snout.Attack(vector, Me.snoutShadowPositions(num), Me.onLeft, Me.queuedAttack)
	End Sub

	' Token: 0x06002A59 RID: 10841 RVA: 0x0018BB7C File Offset: 0x00189F7C
	Private Sub animationEvent_HandsUp()
		Dim num As Integer
		If Me.onLeft Then
			num = RumRunnersLevelAnteater.HandsUpAnimatorHash
		Else
			num = RumRunnersLevelAnteater.HandsDownAnimatorHash
		End If
		Dim currentAnimatorStateInfo As AnimatorStateInfo = MyBase.animator.GetCurrentAnimatorStateInfo(RumRunnersLevelAnteater.HandsAnimatorLayer)
		If currentAnimatorStateInfo.shortNameHash <> num OrElse currentAnimatorStateInfo.normalizedTime >= 1F Then
			MyBase.animator.Play(num, RumRunnersLevelAnteater.HandsAnimatorLayer, 0F)
		End If
		MyBase.animator.Update(0F)
	End Sub

	' Token: 0x06002A5A RID: 10842 RVA: 0x0018BBFC File Offset: 0x00189FFC
	Private Sub animationEvent_HandsDown()
		Dim num As Integer
		If Me.onLeft Then
			num = RumRunnersLevelAnteater.HandsDownAnimatorHash
		Else
			num = RumRunnersLevelAnteater.HandsUpAnimatorHash
		End If
		Dim currentAnimatorStateInfo As AnimatorStateInfo = MyBase.animator.GetCurrentAnimatorStateInfo(RumRunnersLevelAnteater.HandsAnimatorLayer)
		If currentAnimatorStateInfo.shortNameHash <> num OrElse currentAnimatorStateInfo.normalizedTime >= 1F Then
			MyBase.animator.Play(num, RumRunnersLevelAnteater.HandsAnimatorLayer, 0F)
		End If
		MyBase.animator.Update(0F)
	End Sub

	' Token: 0x06002A5B RID: 10843 RVA: 0x0018BC7C File Offset: 0x0018A07C
	Private Sub animationEvent_HandsUpHalfway()
		Dim num As Integer
		If Me.onLeft Then
			num = RumRunnersLevelAnteater.HandsUpAnimatorHash
		Else
			num = RumRunnersLevelAnteater.HandsDownAnimatorHash
		End If
		Dim currentAnimatorStateInfo As AnimatorStateInfo = MyBase.animator.GetCurrentAnimatorStateInfo(RumRunnersLevelAnteater.HandsAnimatorLayer)
		MyBase.animator.Play(num, RumRunnersLevelAnteater.HandsAnimatorLayer, 0.5F)
		MyBase.animator.Update(0F)
	End Sub

	' Token: 0x06002A5C RID: 10844 RVA: 0x0018BCDC File Offset: 0x0018A0DC
	Private Sub animationEvent_HandsStartTaunt()
		If Me.onLeft Then
			MyBase.animator.Play("HandsTaunt", 2, 0F)
		Else
			MyBase.animator.Play("HandsTauntRight", 2, 0F)
		End If
		MyBase.animator.Update(0F)
	End Sub

	' Token: 0x06002A5D RID: 10845 RVA: 0x0018BD35 File Offset: 0x0018A135
	Private Sub animationEvent_HandsEndTaunt()
		MyBase.animator.SetTrigger("HandsEndTaunt")
	End Sub

	' Token: 0x06002A5E RID: 10846 RVA: 0x0018BD47 File Offset: 0x0018A147
	Private Sub animationEvent_FalseDeathDust()
		MyBase.animator.Play("DeathDust", RumRunnersLevelAnteater.DeathDustAnimatorLayer)
		CupheadLevelCamera.Current.Shake(35F, 0.5F, False)
	End Sub

	' Token: 0x06002A5F RID: 10847 RVA: 0x0018BD73 File Offset: 0x0018A173
	Private Sub animationEvent_FalseDeathEnded()
		Me.mobBoss.Begin()
		MyBase.GetComponent(Of LevelBossDeathExploder)().StopExplosions()
		MyBase.StartCoroutine(Me.fakeDeathEyes_cr())
	End Sub

	' Token: 0x06002A60 RID: 10848 RVA: 0x0018BD98 File Offset: 0x0018A198
	Private Sub parseAttackString(attackString As String, <System.Runtime.InteropServices.OutAttribute()> ByRef attackType As RumRunnersLevelSnout.AttackType, <System.Runtime.InteropServices.OutAttribute()> ByRef introLength As RumRunnersLevelAnteater.AttackIntroLength)
		Dim c As Char
		Dim c2 As Char
		If attackString.Length = 2 Then
			c = attackString(1)
			c2 = attackString(0)
		Else
			c = attackString(0)
			c2 = "0"c
		End If
		If c = "Q"c Then
			attackType = RumRunnersLevelSnout.AttackType.Quick
		ElseIf c = "F"c Then
			attackType = RumRunnersLevelSnout.AttackType.Fake
		Else
			If c <> "T"c Then
				Throw New Exception("Invalid attack string: " + attackString)
			End If
			attackType = RumRunnersLevelSnout.AttackType.Tongue
		End If
		If c2 = "L"c Then
			introLength = RumRunnersLevelAnteater.AttackIntroLength.[Long]
		Else
			introLength = RumRunnersLevelAnteater.AttackIntroLength.Standard
		End If
	End Sub

	' Token: 0x06002A61 RID: 10849 RVA: 0x0018BE28 File Offset: 0x0018A228
	Public Sub TriggerEyesTurnaround()
		MyBase.animator.SetTrigger(If((Me.snout.transform.position.y >= 150F), "EyesLookUp", "EyesLookDown"))
	End Sub

	' Token: 0x06002A62 RID: 10850 RVA: 0x0018BE74 File Offset: 0x0018A274
	Public Sub SetEyeSide(onLeft As Boolean)
		Me.eyes.transform.localPosition = New Vector3(If((Not onLeft), Me.eyePositionAttack.x, (-Me.eyePositionAttack.x)), Me.eyePositionAttack.y)
	End Sub

	' Token: 0x06002A63 RID: 10851 RVA: 0x0018BEC3 File Offset: 0x0018A2C3
	Private Sub AnimationEvent_SFX_RUMRUN_P3_Anteater_Intro()
		AudioManager.Play("sfx_dlc_rumrun_p3_anteater_intro")
		Me.emitAudioFromObject.Add("sfx_dlc_rumrun_p3_anteater_intro")
	End Sub

	' Token: 0x06002A64 RID: 10852 RVA: 0x0018BEDF File Offset: 0x0018A2DF
	Private Sub AnimationEvent_SFX_RUMRUN_P3_Anteater_HandSlamFirst()
		AudioManager.Play("sfx_dlc_rumrun_p3_anteater_handslamfirst")
		Me.emitAudioFromObject.Add("sfx_dlc_rumrun_p3_anteater_handslamfirst")
	End Sub

	' Token: 0x06002A65 RID: 10853 RVA: 0x0018BEFB File Offset: 0x0018A2FB
	Private Sub AnimationEvent_SFX_RUMRUN_P3_Anteater_HandSlamSecond()
		AudioManager.Play("sfx_dlc_rumrun_p3_anteater_handslamsecond")
		Me.emitAudioFromObject.Add("sfx_dlc_rumrun_p3_anteater_handslamsecond")
	End Sub

	' Token: 0x06002A66 RID: 10854 RVA: 0x0018BF17 File Offset: 0x0018A317
	Private Sub AnimationEvent_SFX_RUMRUN_P3_Anteater_Intro_Hat_Off()
		AudioManager.Play("sfx_dlc_rumrun_p3_anteater_intro_hat_off")
		Me.emitAudioFromObject.Add("sfx_dlc_rumrun_p3_anteater_intro_hat_off")
	End Sub

	' Token: 0x06002A67 RID: 10855 RVA: 0x0018BF33 File Offset: 0x0018A333
	Private Sub AnimationEvent_SFX_RUMRUN_P3_Anteater_Intro_Hatton()
		AudioManager.Play("sfx_dlc_rumrun_p3_anteater_intro_hatton")
		Me.emitAudioFromObject.Add("sfx_dlc_rumrun_p3_anteater_intro_hatton")
	End Sub

	' Token: 0x06002A68 RID: 10856 RVA: 0x0018BF4F File Offset: 0x0018A34F
	Private Sub AnimationEvent_SFX_RUMRUN_P3_AntEater_Attack_Start()
		AudioManager.Play("sfx_dlc_rumrun_p3_anteater_attack_initial_start")
		Me.emitAudioFromObject.Add("sfx_dlc_rumrun_p3_anteater_attack_initial_start")
	End Sub

	' Token: 0x06002A69 RID: 10857 RVA: 0x0018BF6B File Offset: 0x0018A36B
	Private Sub AnimationEvent_SFX_RUMRUN_P4_IntroSnailLaugh()
		AudioManager.Play("sfx_dlc_rumrun_vx_fakeannouncer_laughing")
		Me.emitAudioFromObject.Add("sfx_dlc_rumrun_vx_fakeannouncer_laughing")
	End Sub

	' Token: 0x04003319 RID: 13081
	Private Shared SNOUT_SPAWN_X As Single = 0F

	' Token: 0x0400331A RID: 13082
	Private Shared EyesAnimationLayer As Integer = 1

	' Token: 0x0400331B RID: 13083
	Private Shared HandsAnimatorLayer As Integer = 2

	' Token: 0x0400331C RID: 13084
	Private Shared DeathDustAnimatorLayer As Integer = 3

	' Token: 0x0400331D RID: 13085
	Private Shared HandsUpAnimatorHash As Integer = Animator.StringToHash("HandsUp")

	' Token: 0x0400331E RID: 13086
	Private Shared HandsDownAnimatorHash As Integer = Animator.StringToHash("HandsDown")

	' Token: 0x0400331F RID: 13087
	Private eyePositionAttack As Vector2 = New Vector2(348F, 145F)

	' Token: 0x04003320 RID: 13088
	<SerializeField()>
	Private spawnPoints As Transform()

	' Token: 0x04003321 RID: 13089
	<SerializeField()>
	Private snoutShadowPositions As Vector2()

	' Token: 0x04003322 RID: 13090
	<SerializeField()>
	Private snout As RumRunnersLevelSnout

	' Token: 0x04003323 RID: 13091
	<SerializeField()>
	Private mobBoss As RumRunnersLevelMobBoss

	' Token: 0x04003324 RID: 13092
	<SerializeField()>
	Private mobBossHelperTransform As Transform

	' Token: 0x04003325 RID: 13093
	<SerializeField()>
	Private collChildren As CollisionChild()

	' Token: 0x04003326 RID: 13094
	<SerializeField()>
	Private tongue As RumRunnersLevelSnoutTongue

	' Token: 0x04003327 RID: 13095
	<SerializeField()>
	Private flipRenderers As SpriteRenderer()

	' Token: 0x04003328 RID: 13096
	<SerializeField()>
	Private blinkProbability As Single

	' Token: 0x04003329 RID: 13097
	<SerializeField()>
	Private eyes As GameObject

	' Token: 0x0400332A RID: 13098
	Private damageDealer As DamageDealer

	' Token: 0x0400332B RID: 13099
	Private snoutAttackPattern As PatternString

	' Token: 0x0400332C RID: 13100
	Private snoutPositionPattern As PatternString

	' Token: 0x0400332D RID: 13101
	Private onLeft As Boolean = True

	' Token: 0x0400332E RID: 13102
	Private firstAttack As Boolean = True

	' Token: 0x0400332F RID: 13103
	Private queuedAttack As RumRunnersLevelSnout.AttackType

	' Token: 0x02000784 RID: 1924
	Private Enum AttackIntro
		' Token: 0x04003331 RID: 13105
		Initial = -1
		' Token: 0x04003332 RID: 13106
		A
		' Token: 0x04003333 RID: 13107
		B
		' Token: 0x04003334 RID: 13108
		Final
	End Enum

	' Token: 0x02000785 RID: 1925
	Private Enum AttackIntroLength
		' Token: 0x04003336 RID: 13110
		Standard
		' Token: 0x04003337 RID: 13111
		[Long]
	End Enum
End Class
