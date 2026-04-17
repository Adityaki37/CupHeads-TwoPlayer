Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200070F RID: 1807
Public Class OldManLevelSockPuppetHandler
	Inherits LevelProperties.OldMan.Entity

	' Token: 0x06002726 RID: 10022 RVA: 0x0016E7FD File Offset: 0x0016CBFD
	Private Sub Start()
		Me.transState = OldManLevelSockPuppetHandler.TransitionState.None
		Me.dwarvesObject.gameObject.SetActive(False)
	End Sub

	' Token: 0x06002727 RID: 10023 RVA: 0x0016E817 File Offset: 0x0016CC17
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.WORKAROUND_NullifyFields()
	End Sub

	' Token: 0x06002728 RID: 10024 RVA: 0x0016E828 File Offset: 0x0016CC28
	Public Sub StartPhase2()
		Me.dwarvesObject.gameObject.SetActive(True)
		Me.sockPuppetLeft.gameObject.SetActive(True)
		Me.sockPuppetRight.gameObject.SetActive(True)
		Me.damageDealer = DamageDealer.NewEnemy()
		AddHandler Me.sockPuppetRight.GetComponent(Of CollisionChild)().OnPlayerCollision, AddressOf Me.OnCollisionPlayer
		AddHandler Me.sockPuppetLeft.GetComponent(Of CollisionChild)().OnPlayerCollision, AddressOf Me.OnCollisionPlayer
		AddHandler Me.sockPuppetRight.GetComponent(Of DamageReceiver)().OnDamageTaken, AddressOf Me.OnDamageTaken
		AddHandler Me.sockPuppetLeft.GetComponent(Of DamageReceiver)().OnDamageTaken, AddressOf Me.OnDamageTaken
		MyBase.StartCoroutine(Me.bounce_ball_cr())
		MyBase.StartCoroutine(Me.dwarves_arc_cr())
		AddHandler CType(PlayerManager.GetPlayer(PlayerId.PlayerOne), LevelPlayerController).motor.OnHitEvent, AddressOf Me.TriggerLaugh
		If PlayerManager.Multiplayer Then
			AddHandler CType(PlayerManager.GetPlayer(PlayerId.PlayerTwo), LevelPlayerController).motor.OnHitEvent, AddressOf Me.TriggerLaugh
		End If
	End Sub

	' Token: 0x06002729 RID: 10025 RVA: 0x0016E94B File Offset: 0x0016CD4B
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x0600272A RID: 10026 RVA: 0x0016E963 File Offset: 0x0016CD63
	Public Overrides Sub LevelInit(properties As LevelProperties.OldMan)
		MyBase.LevelInit(properties)
	End Sub

	' Token: 0x0600272B RID: 10027 RVA: 0x0016E96C File Offset: 0x0016CD6C
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		MyBase.properties.DealDamage(info.damage)
	End Sub

	' Token: 0x0600272C RID: 10028 RVA: 0x0016E97F File Offset: 0x0016CD7F
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x0600272D RID: 10029 RVA: 0x0016E9A0 File Offset: 0x0016CDA0
	Private Iterator Function dwarves_arc_cr() As IEnumerator
		Dim p As LevelProperties.OldMan.Dwarf = MyBase.properties.CurrentState.dwarf
		Dim arcAttackDelay As PatternString = New PatternString(p.arcAttackDelayString, True, True)
		Dim arcAttackPos As PatternString = New PatternString(p.arcAttackPosString, True, True)
		Dim arcShootHeight As PatternString = New PatternString(p.arcShootHeightString, True, True)
		Dim parryableString As PatternString = New PatternString(p.parryString, True)
		Dim posIndex As Integer = 0
		Dim typeA As Boolean = Rand.Bool()
		Yield CupheadTime.WaitForSeconds(Me, 2F)
		While True
			posIndex = arcAttackPos.PopInt()
			If Me.dwarves(posIndex).inPlace Then
				Me.dwarves(posIndex).ShootInArc(arcShootHeight.PopFloat(), p.arcApex, p.arcHealth, typeA, parryableString.PopLetter() = "P"c, p.arcAttackWarningTime)
				typeA = Not typeA
				Yield CupheadTime.WaitForSeconds(Me, arcAttackDelay.PopFloat())
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600272E RID: 10030 RVA: 0x0016E9BC File Offset: 0x0016CDBC
	Private Iterator Function bounce_ball_cr() As IEnumerator
		Dim p As LevelProperties.OldMan.Hands = MyBase.properties.CurrentState.hands
		Dim leftHandPosString As PatternString = New PatternString(p.leftHandPosString, True, True)
		Dim rightHandPosString As PatternString = New PatternString(p.rightHandPosString, True, True)
		Yield CupheadTime.WaitForSeconds(Me, 0.3F)
		Me.fromLeft = Rand.Bool()
		If Me.fromLeft Then
			Me.sockPuppetLeft.AnIEvent_HoldingBall()
		Else
			Me.sockPuppetRight.AnIEvent_HoldingBall()
		End If
		Me.sockPuppetLeft.MoveToPos(Me.KDpuppetYPositions(Me.leftHandPos).position.y, 1F)
		MyBase.StartCoroutine(Me.move_level_borders_time_cr(1060, Level.Current.Right, 0.5F))
		Yield CupheadTime.WaitForSeconds(Me, 0.45F)
		MyBase.animator.Play("Ph2_Enter")
		Yield MyBase.animator.WaitForAnimationToStart(Me, "LookUpLeftAndBack", False)
		Yield CupheadTime.WaitForSeconds(Me, 0.3F)
		Me.sockPuppetRight.MoveToPos(Me.DpuppetYPositions(Me.rightHandPos).position.y, 1F)
		MyBase.StartCoroutine(Me.move_level_borders_time_cr(-Level.Current.Left, 152, 0.5F))
		Yield CupheadTime.WaitForSeconds(Me, 0.7F)
		MyBase.animator.Play("LookUpRightAndBack")
		Yield Nothing
		MyBase.animator.SetTrigger("EndIntroLook")
		Dim first As Boolean = True
		MyBase.StartCoroutine(Me.animate_face_cr())
		While True
			If Not first Then
				Me.rightHandPosOld = Me.rightHandPos
				Me.leftHandPosOld = Me.leftHandPos
				Me.rightHandPos = rightHandPosString.PopInt()
				Me.leftHandPos = leftHandPosString.PopInt()
				Me.sockPuppetLeft.MoveToPos(Me.KDpuppetYPositions(Me.leftHandPos).position.y, CSng(Mathf.Abs(Me.leftHandPosOld - Me.leftHandPos)))
				Me.sockPuppetRight.MoveToPos(Me.DpuppetYPositions(Me.rightHandPos).position.y, CSng(Mathf.Abs(Me.rightHandPosOld - Me.rightHandPos)))
			End If
			first = False
			Me.sockPuppetRight.animator.SetBool("CanTaunt", Me.fromLeft)
			Me.sockPuppetLeft.animator.SetBool("CanTaunt", Not Me.fromLeft)
			While Not Me.sockPuppetLeft.ready OrElse Not Me.sockPuppetRight.ready
				Yield Nothing
			End While
			Me.sockPuppetRight.animator.SetBool("IsCatching", Me.fromLeft)
			Me.sockPuppetLeft.animator.SetBool("IsCatching", Not Me.fromLeft)
			Yield CupheadTime.WaitForSeconds(Me, p.throwDelay)
			Dim throwingPuppet As OldManLevelSockPuppet = If((Not Me.fromLeft), Me.sockPuppetRight, Me.sockPuppetLeft)
			throwingPuppet.animator.SetTrigger("IsThrowing")
			Me.sockPuppetRight.animator.SetBool("CanTaunt", False)
			Me.sockPuppetLeft.animator.SetBool("CanTaunt", False)
			Me.sockPuppetRight.StopTaunt()
			Me.sockPuppetLeft.StopTaunt()
			Yield throwingPuppet.animator.WaitForAnimationToEnd(Me, "Throw_Start", False, True)
			Yield CupheadTime.WaitForSeconds(Me, p.throwWarningTime)
			throwingPuppet.animator.Play("Throw")
			Yield Nothing
			While throwingPuppet.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.9F
				Yield Nothing
			End While
			Dim startPos As Vector3 = If((Not Me.fromLeft), Me.sockPuppetRight.throwPosition(), Me.sockPuppetLeft.throwPosition())
			Dim endPos As Vector3 = If((Not Me.fromLeft), Me.sockPuppetLeft.catchPosition(), Me.sockPuppetRight.catchPosition())
			Dim pos As Vector3 = New Vector3(Me.mainPlatformCollider.transform.position.x + CSng((Me.leftHandPos - Me.rightHandPos)) * p.bouncePositionSpacing, Me.mainPlatformCollider.bounds.max.y)
			Me.puppetBall = Me.puppetBallPrefab.Spawn()
			Me.puppetBall.Init(startPos, pos, endPos, p)
			throwingPuppet.animator.Play("Throw_End")
			throwingPuppet.animator.Update(0F)
			throwingPuppet.AnIEvent_NotHoldingBall()
			While Not Me.puppetBall.readyToCatch
				Yield Nothing
			End While
			If Me.fromLeft Then
				Me.sockPuppetRight.animator.Play("Catch")
				Me.sockPuppetRight.animator.Update(0F)
				Me.sockPuppetRight.animator.SetBool("IsCatching", False)
			Else
				Me.sockPuppetLeft.animator.Play("Catch")
				Me.sockPuppetLeft.animator.Update(0F)
				Me.sockPuppetLeft.animator.SetBool("IsCatching", False)
			End If
			Me.fromLeft = Not Me.fromLeft
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600272F RID: 10031 RVA: 0x0016E9D8 File Offset: 0x0016CDD8
	Private Iterator Function animate_face_cr() As IEnumerator
		Dim t As Single = 0F
		Dim waitTime As Single = 0F
		Dim lookLeft As Boolean = Rand.Bool()
		Dim laughString As PatternString = New PatternString("L,N,N,N,N,N,N,N,L,N,N,N,N,L,N,N,N,N,N,N", True)
		While True
			Yield MyBase.animator.WaitForAnimationToStart(Me, "Idle", False)
			If Me.triggerLaugh OrElse laughString.PopLetter() = "L"c Then
				MyBase.animator.Play("Laugh")
				Me.triggerLaugh = False
				Yield Nothing
			Else
				waitTime = Me.idleHoldRange.RandomFloat()
				t = 0F
				While t < waitTime AndAlso Not Me.triggerLaugh
					t += CupheadTime.Delta
					Yield Nothing
				End While
				Dim curLook As Integer = If((Not lookLeft), Me.rightHandPos, Me.leftHandPos)
				If Not lookLeft AndAlso Not Me.sockPuppetRight.ready AndAlso Me.rightHandPos = 2 Then
					curLook = 1
				End If
				MyBase.animator.Play(If((Not lookLeft), If((curLook <= 0), "LookRight", If((curLook <= 1), "LookMidRight", "LookUpRight")), If((curLook <= 0), "LookLeft", "LookUpLeft")))
				Yield MyBase.animator.WaitForAnimationToStart(Me, If((Not lookLeft), If((curLook <= 0), "LookRightHold", If((curLook <= 1), "LookMidRightHold", "LookUpRightHold")), If((curLook <= 0), "LookLeftHold", "LookUpLeftHold")), False)
				waitTime = Me.lookHoldRange.RandomFloat()
				t = 0F
				While t < waitTime AndAlso (t < Me.lookHoldRange.min OrElse If((Not lookLeft), Me.sockPuppetRight.ready, Me.sockPuppetLeft.ready)) AndAlso Not Me.triggerLaugh
					t += CupheadTime.Delta
					Yield Nothing
				End While
				MyBase.animator.SetTrigger("Continue")
				If Global.UnityEngine.Random.Range(0F, 1F) < Me.chanceToSwitchLookSides Then
					lookLeft = Not lookLeft
				End If
			End If
		End While
		Return
	End Function

	' Token: 0x06002730 RID: 10032 RVA: 0x0016E9F4 File Offset: 0x0016CDF4
	Private Sub TriggerLaugh()
		If Not MyBase.animator.GetCurrentAnimatorStateInfo(0).IsName("Laugh") Then
			Me.triggerLaugh = True
		End If
	End Sub

	' Token: 0x06002731 RID: 10033 RVA: 0x0016EA26 File Offset: 0x0016CE26
	Public Sub CatchBall()
		Me.puppetBall.GetCaught()
	End Sub

	' Token: 0x06002732 RID: 10034 RVA: 0x0016EA33 File Offset: 0x0016CE33
	Public Sub OnPhase3()
		If Me.OnDeathEvent IsNot Nothing Then
			Me.OnDeathEvent()
		End If
		Me.StopAllCoroutines()
		MyBase.StartCoroutine(Me.deathAnimation_cr())
	End Sub

	' Token: 0x06002733 RID: 10035 RVA: 0x0016EA60 File Offset: 0x0016CE60
	Private Iterator Function deathAnimation_cr() As IEnumerator
		RemoveHandler CType(PlayerManager.GetPlayer(PlayerId.PlayerOne), LevelPlayerController).motor.OnHitEvent, AddressOf Me.TriggerLaugh
		If PlayerManager.Multiplayer Then
			RemoveHandler CType(PlayerManager.GetPlayer(PlayerId.PlayerTwo), LevelPlayerController).motor.OnHitEvent, AddressOf Me.TriggerLaugh
		End If
		Me.sockPuppetLeft.Die()
		Me.sockPuppetRight.Die()
		If Me.puppetBall Is Nothing Then
			Me.puppetBall = Me.puppetBallPrefab.Spawn()
			Me.puppetBall.transform.position = If((Not Me.fromLeft), Me.sockPuppetRight.throwPosition(), Me.sockPuppetLeft.throwPosition())
		End If
		Me.puppetBall.Explode()
		For Each oldManLevelDwarf As OldManLevelDwarf In Me.dwarves
			oldManLevelDwarf.Death(True)
		Next
		MyBase.animator.SetTrigger("Continue")
		Yield MyBase.animator.WaitForAnimationToStart(Me, "Idle", False)
		MyBase.animator.Play("Angry")
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		Dim startPos As Vector3 = Me.oldManAngry.localPosition
		Dim endPos As Vector3 = New Vector3(Me.oldManAngry.localPosition.x, 200F)
		Dim sockPuppetLeftStart As Vector3 = Me.sockPuppetLeft.rootPosition
		Dim sockPuppetRightStart As Vector3 = Me.sockPuppetRight.rootPosition
		Dim sockPuppetLeftEnd As Vector3 = If((Level.Current.mode <> Level.Mode.Easy), New Vector3(Me.sockPuppetLeft.rootPosition.x - 300F, -1100F), New Vector3(Me.sockPuppetLeft.rootPosition.x, Me.KDpuppetYPositions(1).position.y))
		Dim sockPuppetRightEnd As Vector3 = If((Level.Current.mode <> Level.Mode.Easy), New Vector3(Me.sockPuppetRight.rootPosition.x + 300F, -1100F), New Vector3(Me.sockPuppetRight.rootPosition.x, Me.DpuppetYPositions(1).position.y))
		Yield CupheadTime.WaitForSeconds(Me, If((Level.Current.mode <> Level.Mode.Easy), 2F, 0.1F))
		Dim t As Single = 0F
		Dim time As Single = If((Level.Current.mode <> Level.Mode.Easy), MyBase.properties.CurrentState.hands.endSlideUpTime, 2F)
		While t < time
			t += CupheadTime.FixedDelta
			If Level.Current.mode <> Level.Mode.Easy Then
				Me.oldManAngry.SetPosition(Nothing, New Single?(Mathf.Lerp(startPos.y, endPos.y, t / time)), Nothing)
				Me.oldManAngryNoseShadow.localPosition = New Vector3(Me.oldManAngryNoseShadow.localPosition.x, Mathf.Lerp(0F, 10F, t / time))
			End If
			Me.sockPuppetLeft.rootPosition = New Vector3(Mathf.Lerp(sockPuppetLeftStart.x, sockPuppetLeftEnd.x, EaseUtils.EaseInSine(0F, 1F, t / time)), Mathf.Lerp(sockPuppetLeftStart.y, sockPuppetLeftEnd.y, EaseUtils.EaseInSine(0F, 1F, t / time)))
			Me.sockPuppetRight.rootPosition = New Vector3(Mathf.Lerp(sockPuppetRightStart.x, sockPuppetRightEnd.x, EaseUtils.EaseInSine(0F, 1F, t / time)), Mathf.Lerp(sockPuppetRightStart.y, sockPuppetRightEnd.y, EaseUtils.EaseInSine(0F, 1F, t / time)))
			If t <= 0.5F AndAlso t + CupheadTime.FixedDelta > 0.5F Then
				Me.sockPuppetLeft.GetComponent(Of LevelBossDeathExploder)().StopExplosions()
				Me.sockPuppetRight.GetComponent(Of LevelBossDeathExploder)().StopExplosions()
			End If
			Yield wait
		End While
		Global.UnityEngine.[Object].Destroy(Me.dwarvesObject.gameObject)
		If Level.Current.mode <> Level.Mode.Easy Then
			MyBase.animator.SetTrigger("ContinueDeath")
			While Me.transState <> OldManLevelSockPuppetHandler.TransitionState.PlatformDestroyed
				Yield Nothing
			End While
			Yield MyBase.StartCoroutine(Me.move_level_borders_anim_sync_cr(925, 93, 56F))
		End If
		Return
	End Function

	' Token: 0x06002734 RID: 10036 RVA: 0x0016EA7C File Offset: 0x0016CE7C
	Private Iterator Function move_level_borders_time_cr(left As Integer, right As Integer, time As Single) As IEnumerator
		Dim t As Single = 0F
		Dim startLeft As Single = CSng((-CSng(Level.Current.Left)))
		Dim startRight As Single = CSng(Level.Current.Right)
		While t < time
			t += CupheadTime.Delta
			Dim tm As Single = Mathf.InverseLerp(0F, time, t)
			Level.Current.SetBounds(New Integer?(CInt(Mathf.Lerp(startLeft, CSng(left), tm))), New Integer?(CInt(Mathf.Lerp(startRight, CSng(right), tm))), Nothing, Nothing)
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06002735 RID: 10037 RVA: 0x0016EAA8 File Offset: 0x0016CEA8
	Private Iterator Function move_level_borders_anim_sync_cr(left As Integer, right As Integer, endFrame As Single) As IEnumerator
		Dim startTime As Single = MyBase.animator.GetCurrentAnimatorStateInfo(0).normalizedTime
		Dim startLeft As Single = CSng((-CSng(Level.Current.Left)))
		Dim startRight As Single = CSng(Level.Current.Right)
		While MyBase.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < (endFrame + 1F) / 79F
			Dim tm As Single = Mathf.InverseLerp(startTime, endFrame / 79F, MyBase.animator.GetCurrentAnimatorStateInfo(0).normalizedTime)
			Level.Current.SetBounds(New Integer?(CInt(Mathf.Lerp(startLeft, CSng(left), tm))), New Integer?(CInt(Mathf.Lerp(startRight, CSng(right), tm))), Nothing, Nothing)
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06002736 RID: 10038 RVA: 0x0016EAD8 File Offset: 0x0016CED8
	Private Iterator Function shake_platform_cr() As IEnumerator
		Dim amount As Single = 1.5F
		While Me.transState <> OldManLevelSockPuppetHandler.TransitionState.PlatformDestroyed
			Me.handsParent.transform.localPosition = New Vector3(Global.UnityEngine.Random.Range(-amount, amount), Global.UnityEngine.Random.Range(-amount, amount))
			amount += 0.25F
			Yield CupheadTime.WaitForSeconds(Me, 0.016666668F)
		End While
		Return
	End Function

	' Token: 0x06002737 RID: 10039 RVA: 0x0016EAF4 File Offset: 0x0016CEF4
	Private Sub AniEvent_HandsGrip()
		CupheadLevelCamera.Current.Shake(5F, 0.2F, False)
		Me.beardObject.transform.parent = Me.handsParent.transform
		Me.rocksUnderBeardObject.transform.parent = Me.handsParent.transform
		MyBase.StartCoroutine(Me.shake_platform_cr())
	End Sub

	' Token: 0x06002738 RID: 10040 RVA: 0x0016EB5C File Offset: 0x0016CF5C
	Private Sub AniEvent_PlatformDestroyed()
		Me.beardObject.transform.parent = Me.BGParent.transform
		Me.rocksUnderBeardObject.transform.parent = Me.BGParent.transform
		Me.transState = OldManLevelSockPuppetHandler.TransitionState.PlatformDestroyed
		CupheadLevelCamera.Current.Shake(30F, 0.7F, False)
	End Sub

	' Token: 0x06002739 RID: 10041 RVA: 0x0016EBBB File Offset: 0x0016CFBB
	Private Sub AniEvent_FinishedSwallow()
		Me.transState = OldManLevelSockPuppetHandler.TransitionState.InStomach
	End Sub

	' Token: 0x0600273A RID: 10042 RVA: 0x0016EBC4 File Offset: 0x0016CFC4
	Public Sub SwallowedPlayers()
		MyBase.animator.SetTrigger("SwallowedPlayers")
	End Sub

	' Token: 0x0600273B RID: 10043 RVA: 0x0016EBD6 File Offset: 0x0016CFD6
	Public Sub FinishPuppet()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x0600273C RID: 10044 RVA: 0x0016EBE3 File Offset: 0x0016CFE3
	Private Sub AnimationEvent_SFX_OMM_P2_EndBreakPlatformEat()
		AudioManager.Play("sfx_dlc_omm_p2_end_breakplatformeat")
		Me.emitAudioFromObject.Add("sfx_dlc_omm_p2_end_breakplatformeat")
	End Sub

	' Token: 0x0600273D RID: 10045 RVA: 0x0016EBFF File Offset: 0x0016CFFF
	Private Sub AnimationEvent_SFX_OMM_P2_EndBurp()
		AudioManager.Play("sfx_dlc_omm_p2_end_burp")
		Me.emitAudioFromObject.Add("sfx_dlc_omm_p2_end_burp")
	End Sub

	' Token: 0x0600273E RID: 10046 RVA: 0x0016EC1C File Offset: 0x0016D01C
	Private Sub WORKAROUND_NullifyFields()
		Me.OnDeathEvent = Nothing
		Me.idleHoldRange = Nothing
		Me.lookHoldRange = Nothing
		Me.oldManAngry = Nothing
		Me.oldManAngryNoseShadow = Nothing
		Me.mainPlatformCollider = Nothing
		Me.puppetBallPrefab = Nothing
		Me.sockPuppetLeft = Nothing
		Me.sockPuppetRight = Nothing
		Me.platformManager = Nothing
		Me.KDpuppetYPositions = Nothing
		Me.DpuppetYPositions = Nothing
		Me.dwarves = Nothing
		Me.dwarvesObject = Nothing
		Me.handsParent = Nothing
		Me.BGParent = Nothing
		Me.beardObject = Nothing
		Me.rocksUnderBeardObject = Nothing
		Me.damageDealer = Nothing
		Me.puppetBall = Nothing
	End Sub

	' Token: 0x04002FDA RID: 12250
	Private Const POST_DEATH_PRE_MOVE_TIME As Single = 2F

	' Token: 0x04002FDB RID: 12251
	<SerializeField()>
	Private idleHoldRange As MinMax = New MinMax(0.01F, 0.1F)

	' Token: 0x04002FDC RID: 12252
	<SerializeField()>
	Private lookHoldRange As MinMax = New MinMax(0.75F, 1.5F)

	' Token: 0x04002FDD RID: 12253
	<SerializeField()>
	Private chanceToSwitchLookSides As Single = 0.75F

	' Token: 0x04002FDE RID: 12254
	<SerializeField()>
	Private chanceToLaugh As Single = 0.25F

	' Token: 0x04002FDF RID: 12255
	Public transState As OldManLevelSockPuppetHandler.TransitionState

	' Token: 0x04002FE0 RID: 12256
	<SerializeField()>
	Private oldManAngry As Transform

	' Token: 0x04002FE1 RID: 12257
	<SerializeField()>
	Private oldManAngryNoseShadow As Transform

	' Token: 0x04002FE2 RID: 12258
	<SerializeField()>
	Private mainPlatformCollider As Collider2D

	' Token: 0x04002FE3 RID: 12259
	<SerializeField()>
	Private puppetBallPrefab As OldManLevelPuppetBall

	' Token: 0x04002FE4 RID: 12260
	<SerializeField()>
	Private sockPuppetLeft As OldManLevelSockPuppet

	' Token: 0x04002FE5 RID: 12261
	<SerializeField()>
	Private sockPuppetRight As OldManLevelSockPuppet

	' Token: 0x04002FE6 RID: 12262
	<SerializeField()>
	Private platformManager As OldManLevelPlatformManager

	' Token: 0x04002FE7 RID: 12263
	<SerializeField()>
	Private KDpuppetYPositions As Transform()

	' Token: 0x04002FE8 RID: 12264
	<SerializeField()>
	Private DpuppetYPositions As Transform()

	' Token: 0x04002FE9 RID: 12265
	<SerializeField()>
	Private dwarves As OldManLevelDwarf()

	' Token: 0x04002FEA RID: 12266
	<SerializeField()>
	Private dwarvesObject As GameObject

	' Token: 0x04002FEB RID: 12267
	<SerializeField()>
	Private handsParent As GameObject

	' Token: 0x04002FEC RID: 12268
	<SerializeField()>
	Private BGParent As GameObject

	' Token: 0x04002FED RID: 12269
	<SerializeField()>
	Private beardObject As GameObject

	' Token: 0x04002FEE RID: 12270
	<SerializeField()>
	Private rocksUnderBeardObject As GameObject

	' Token: 0x04002FEF RID: 12271
	Private damageDealer As DamageDealer

	' Token: 0x04002FF0 RID: 12272
	Public OnDeathEvent As Action

	' Token: 0x04002FF1 RID: 12273
	Private puppetBall As OldManLevelPuppetBall

	' Token: 0x04002FF2 RID: 12274
	Private leftHandPos As Integer = 1

	' Token: 0x04002FF3 RID: 12275
	Private rightHandPos As Integer = 1

	' Token: 0x04002FF4 RID: 12276
	Private rightHandPosOld As Integer

	' Token: 0x04002FF5 RID: 12277
	Private leftHandPosOld As Integer

	' Token: 0x04002FF6 RID: 12278
	Private triggerLaugh As Boolean

	' Token: 0x04002FF7 RID: 12279
	Private fromLeft As Boolean

	' Token: 0x02000710 RID: 1808
	Public Enum TransitionState
		' Token: 0x04002FF9 RID: 12281
		None
		' Token: 0x04002FFA RID: 12282
		PlatformDestroyed
		' Token: 0x04002FFB RID: 12283
		InStomach
	End Enum
End Class
