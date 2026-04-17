Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x020007A6 RID: 1958
Public Class SallyStagePlayLevelBackgroundHandler
	Inherits AbstractPausableComponent

	' Token: 0x17000404 RID: 1028
	' (get) Token: 0x06002BED RID: 11245 RVA: 0x0019BAC9 File Offset: 0x00199EC9
	' (set) Token: 0x06002BEE RID: 11246 RVA: 0x0019BAD0 File Offset: 0x00199ED0
	Public Shared Property HUSBAND_GONE As Boolean

	' Token: 0x17000405 RID: 1029
	' (get) Token: 0x06002BEF RID: 11247 RVA: 0x0019BAD8 File Offset: 0x00199ED8
	' (set) Token: 0x06002BF0 RID: 11248 RVA: 0x0019BADF File Offset: 0x00199EDF
	Public Shared Property CURTAIN_OPEN As Boolean

	' Token: 0x06002BF1 RID: 11249 RVA: 0x0019BAE7 File Offset: 0x00199EE7
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.curtain.gameObject.SetActive(True)
	End Sub

	' Token: 0x06002BF2 RID: 11250 RVA: 0x0019BB00 File Offset: 0x00199F00
	Private Sub Start()
		AddHandler Level.Current.OnLevelStartEvent, AddressOf Me.StartPriestLoop
		AddHandler Level.Current.OnLevelStartEvent, AddressOf Me.StartHusbandLoop
		Me.curtainStartPos = Me.curtainSprite.position
		Me.curtainShadowStartPos = Me.curtainShadow.position
		Me.chandelierStartPosX = Me.chandelier.transform.position.x
		SallyStagePlayLevelBackgroundHandler.HUSBAND_GONE = False
		For Each cupid As SallyStagePlayLevelBackgroundHandler.Cupid In Me.cupids
			cupid.startPosition = cupid.cupidTransform.position
		Next
		Me.applauseHandler.SlideApplause(True)
	End Sub

	' Token: 0x06002BF3 RID: 11251 RVA: 0x0019BBBC File Offset: 0x00199FBC
	Public Sub GetProperties(properties As LevelProperties.SallyStagePlay, parent As SallyStagePlayLevel)
		Me.properties = properties
		Me.parent = parent
		For Each spriteRenderer As SpriteRenderer In Me.flickeringLights
			MyBase.StartCoroutine(Me.flicker_cr(spriteRenderer))
		Next
		Me.phaseDependentCoroutines = New List(Of Coroutine)()
		Me.phaseDependentCoroutines.Add(MyBase.StartCoroutine(Me.check_bools_cr()))
		For j As Integer = 0 To Me.cupids.Length - 1
			Me.phaseDependentCoroutines.Add(MyBase.StartCoroutine(Me.cupid_check_falling(Me.cupids(j))))
		Next
		For Each transform As Transform In Me.churchSwingies
			Me.phaseDependentCoroutines.Add(MyBase.StartCoroutine(Me.swing_cr(transform)))
		Next
		Dim [next] As AbstractPlayerController = PlayerManager.GetNext()
		Dim levelPlayerController As LevelPlayerController = CType([next], LevelPlayerController)
		AddHandler levelPlayerController.motor.OnHitEvent, AddressOf Me.PlayYay
		AddHandler parent.OnPhase2, AddressOf Me.OnPhase2
		AddHandler parent.OnPhase3, AddressOf Me.OnPhase3
		AddHandler parent.OnPhase4, AddressOf Me.OnPhase4
	End Sub

	' Token: 0x06002BF4 RID: 11252 RVA: 0x0019BCFD File Offset: 0x0019A0FD
	Public Sub OpenCurtain(backgroundSelected As SallyStagePlayLevelBackgroundHandler.Backgrounds)
		If backgroundSelected <> SallyStagePlayLevelBackgroundHandler.Backgrounds.Finale Then
			Me.applauseHandler.SlideApplause(False)
		End If
		MyBase.StartCoroutine(Me.open_curtain_cr(backgroundSelected))
	End Sub

	' Token: 0x06002BF5 RID: 11253 RVA: 0x0019BD20 File Offset: 0x0019A120
	Public Sub CloseCurtain(backgroundSelected As SallyStagePlayLevelBackgroundHandler.Backgrounds)
		Me.applauseHandler.SlideApplause(True)
		MyBase.StartCoroutine(Me.close_curtain_cr(backgroundSelected))
	End Sub

	' Token: 0x06002BF6 RID: 11254 RVA: 0x0019BD3C File Offset: 0x0019A13C
	Private Iterator Function open_curtain_cr(backgroundsSelected As SallyStagePlayLevelBackgroundHandler.Backgrounds) As IEnumerator
		Me.SelectBackground(backgroundsSelected)
		Dim t As Single = 0F
		Dim frameTime As Single = 0F
		Dim openTime As Single = 1.58F
		Dim shadowRoot As Vector3 = New Vector3(Me.curtainUpRoot.position.x, Me.curtainUpRoot.position.y - 100F)
		AudioManager.Play("sally_bg_stage_curtain_raise")
		Me.emitAudioFromObject.Add("sally_bg_stage_curtain_raise")
		While t < openTime
			frameTime += CupheadTime.Delta
			t += CupheadTime.Delta
			If frameTime > 0.041666668F Then
				Dim num As Single = EaseUtils.Ease(EaseUtils.EaseType.linear, 0F, 1F, t / openTime)
				Me.curtainSprite.transform.position = Vector2.Lerp(Me.curtainSprite.transform.position, Me.curtainUpRoot.position, num)
				Me.curtainShadow.transform.position = Vector2.Lerp(Me.curtainShadow.transform.position, shadowRoot, num)
				frameTime -= 0.041666668F
			End If
			Yield Nothing
		End While
		SallyStagePlayLevelBackgroundHandler.CURTAIN_OPEN = True
		Yield Nothing
		Return
	End Function

	' Token: 0x06002BF7 RID: 11255 RVA: 0x0019BD60 File Offset: 0x0019A160
	Private Iterator Function close_curtain_cr(backgroundSelected As SallyStagePlayLevelBackgroundHandler.Backgrounds) As IEnumerator
		Dim t As Single = 0F
		Dim frameTime As Single = 0F
		Dim closeTime As Single = 1.58F
		AudioManager.Play("sally_bg_stage_curtain_lower")
		Me.emitAudioFromObject.Add("sally_bg_stage_curtain_lower")
		Select Case backgroundSelected
			Case SallyStagePlayLevelBackgroundHandler.Backgrounds.House
				AudioManager.Play("sally_bg_stage_reset_phase1")
				Me.emitAudioFromObject.Add("sally_bg_stage_reset_phase1")
			Case SallyStagePlayLevelBackgroundHandler.Backgrounds.Nunnery
				AudioManager.Play("sally_bg_stage_reset_phase1")
				Me.emitAudioFromObject.Add("sally_bg_stage_reset_phase1")
			Case SallyStagePlayLevelBackgroundHandler.Backgrounds.Purgatory
				AudioManager.Play("sally_bg_stage_reset_phase2")
				Me.emitAudioFromObject.Add("sally_bg_stage_reset_phase2")
			Case SallyStagePlayLevelBackgroundHandler.Backgrounds.Finale
				AudioManager.Play("sally_bg_stage_reset_phase3")
				Me.emitAudioFromObject.Add("sally_bg_stage_reset_phase3")
		End Select
		While t < closeTime
			frameTime += CupheadTime.Delta
			t += CupheadTime.Delta
			If frameTime > 0.041666668F Then
				Dim num As Single = EaseUtils.Ease(EaseUtils.EaseType.linear, 0F, 1F, t / closeTime)
				Me.curtainSprite.transform.position = Vector2.Lerp(Me.curtainSprite.transform.position, Me.curtainStartPos, num)
				Me.curtainShadow.transform.position = Vector2.Lerp(Me.curtainShadow.transform.position, Me.curtainShadowStartPos, num)
				frameTime -= 0.041666668F
			End If
			Yield Nothing
		End While
		SallyStagePlayLevelBackgroundHandler.CURTAIN_OPEN = False
		Yield Nothing
		Return
	End Function

	' Token: 0x06002BF8 RID: 11256 RVA: 0x0019BD84 File Offset: 0x0019A184
	Private Sub SelectBackground(backgroundSelected As SallyStagePlayLevelBackgroundHandler.Backgrounds)
		For i As Integer = 0 To Me.backgrounds.Length - 1
			If i = CInt(backgroundSelected) Then
				Me.backgrounds(i).SetActive(True)
			Else
				Me.backgrounds(i).SetActive(False)
			End If
		Next
	End Sub

	' Token: 0x06002BF9 RID: 11257 RVA: 0x0019BDD4 File Offset: 0x0019A1D4
	Private Iterator Function flicker_cr(flicker As SpriteRenderer) As IEnumerator
		Dim flickerTime As Single = 0.3F
		While True
			Dim counter As Integer = 0
			Dim waitTime As Single = Global.UnityEngine.Random.Range(Me.fadeWaitMinSecond, Me.fadeWaitMaxSecond)
			Dim t As Single = 0F
			Yield CupheadTime.WaitForSeconds(Me, waitTime)
			While counter < 2
				While t < flickerTime
					flicker.color = New Color(1F, 1F, 1F, 1F - t / flickerTime)
					t += CupheadTime.Delta
					Yield Nothing
				End While
				t = 0F
				flicker.color = New Color(1F, 1F, 1F, 0F)
				While t < flickerTime
					flicker.color = New Color(1F, 1F, 1F, t / flickerTime)
					t += CupheadTime.Delta
					Yield Nothing
				End While
				flicker.color = New Color(1F, 1F, 1F, 1F)
				counter += 1
				Yield Nothing
			End While
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06002BFA RID: 11258 RVA: 0x0019BDF8 File Offset: 0x0019A1F8
	Private Iterator Function swing_cr(swing As Transform) As IEnumerator
		Dim t As Single = 0F
		Dim speed As Single = 0F
		Dim maxSpeed As Single = 8F
		Dim minSpeed As Single = 3F
		Dim movingRight As Boolean = Rand.Bool()
		speed = minSpeed
		While True
			t = If((Not movingRight), (t - CupheadTime.Delta), (t + CupheadTime.Delta))
			Dim phase As Single = Mathf.Sin(t)
			swing.localRotation = Quaternion.Euler(New Vector3(0F, 0F, phase * speed))
			If CupheadLevelCamera.Current.isShaking Then
				If speed < maxSpeed Then
					speed += 0.15F
				End If
			ElseIf speed > minSpeed Then
				speed -= 0.05F
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06002BFB RID: 11259 RVA: 0x0019BE13 File Offset: 0x0019A213
	Private Sub StartPriestLoop()
		Me.phaseDependentCoroutines.Add(MyBase.StartCoroutine(Me.priest_animations_cr()))
	End Sub

	' Token: 0x06002BFC RID: 11260 RVA: 0x0019BE2C File Offset: 0x0019A22C
	Private Iterator Function priest_animations_cr() As IEnumerator
		While Me.properties.CurrentState.stateName = LevelProperties.SallyStagePlay.States.Generic
			Dim tuckDown As Boolean = False
			Dim counter As Integer = 0
			Dim maxCounter As Integer = Global.UnityEngine.Random.Range(2, 6)
			While Not tuckDown
				Yield CupheadTime.WaitForSeconds(Me, Global.UnityEngine.Random.Range(2F, 5F))
				Me.priest.SetTrigger("Continue")
				If counter < maxCounter Then
					counter += 1
				Else
					tuckDown = True
				End If
				Yield Nothing
			End While
			Dim isLookingRight As Boolean = True
			maxCounter = Global.UnityEngine.Random.Range(4, 8)
			counter = 0
			Me.priest.Play("Tuck_Down")
			While tuckDown
				Yield CupheadTime.WaitForSeconds(Me, Global.UnityEngine.Random.Range(2F, 5F))
				Me.priest.SetBool("isLookingRight", isLookingRight)
				If counter < maxCounter Then
					counter += 1
				Else
					tuckDown = False
				End If
				isLookingRight = Not isLookingRight
				Yield Nothing
			End While
			Me.priest.Play("Stand_Up")
			Yield Nothing
		End While
		Me.priest.Play("Look_Around")
		Yield Nothing
		Return
	End Function

	' Token: 0x06002BFD RID: 11261 RVA: 0x0019BE47 File Offset: 0x0019A247
	Private Sub StartHusbandLoop()
		Me.husband.SetTrigger("Continue")
		Me.phaseDependentCoroutines.Add(MyBase.StartCoroutine(Me.husband_move_cr()))
	End Sub

	' Token: 0x06002BFE RID: 11262 RVA: 0x0019BE70 File Offset: 0x0019A270
	Private Iterator Function husband_move_cr() As IEnumerator
		Dim movingRight As Boolean = True
		Dim start As Single = 0F
		Dim t As Single = 0F
		Dim time As Single = 2F
		Dim [end] As Single = 0F
		Dim moveOffset As Single = 400F
		Yield Me.husband.WaitForAnimationToStart(Me, "Move", False)
		While Me.husbandMoving
			Yield Nothing
			t = 0F
			start = Me.husband.transform.position.x
			If movingRight Then
				[end] = 640F - moveOffset
			Else
				[end] = -640F + moveOffset
			End If
			While t < time AndAlso Me.husbandMoving
				While Me.husband.GetCurrentAnimatorStateInfo(0).IsName("OhNo") OrElse Me.husband.GetCurrentAnimatorStateInfo(0).IsName("Yay")
					Yield Nothing
				End While
				Dim val As Single = t / time
				Me.husband.transform.SetPosition(New Single?(EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, start, [end], val)), Nothing, Nothing)
				t += CupheadTime.Delta
				Yield Nothing
			End While
			If Me.husbandMoving Then
				Me.husband.transform.SetPosition(New Single?([end]), Nothing, Nothing)
			End If
			movingRight = Not movingRight
			Yield Nothing
		End While
		If SallyStagePlayLevelBackgroundHandler.HUSBAND_GONE Then
			t = 0F
			time = 0.3F
			start = Me.husband.transform.position.x
			While t < time
				t += CupheadTime.Delta
				Me.husband.transform.SetPosition(New Single?(Mathf.Lerp(start, 0F, t / time)), Nothing, Nothing)
				Yield Nothing
			End While
		End If
		Yield Nothing
		Return
	End Function

	' Token: 0x06002BFF RID: 11263 RVA: 0x0019BE8C File Offset: 0x0019A28C
	Private Sub PlayYay()
		If Me.husbandMoving AndAlso Not AudioManager.CheckIfPlaying("sally_bg_church_fiance_yay") Then
			AudioManager.Play("sally_bg_church_fiance_yay")
			Me.emitAudioFromObject.Add("sally_bg_church_fiance_yay")
			Me.husband.Play("Yay")
		End If
	End Sub

	' Token: 0x06002C00 RID: 11264 RVA: 0x0019BEE0 File Offset: 0x0019A2E0
	Private Iterator Function cupid_check_falling(cupid As SallyStagePlayLevelBackgroundHandler.Cupid) As IEnumerator
		Dim p As LevelProperties.SallyStagePlay.General = Me.properties.CurrentState.general
		While True
			Dim player As AbstractPlayerController = PlayerManager.GetPlayer(PlayerId.PlayerOne)
			Dim player2 As AbstractPlayerController = PlayerManager.GetPlayer(PlayerId.PlayerTwo)
			If player2 IsNot Nothing AndAlso Not player2.IsDead Then
				If player.IsDead Then
					If player2.transform.parent IsNot cupid.cupidTransform AndAlso cupid.cupidTransform.position.y >= p.cupidDropMaxY Then
						If cupid.cupidTransform.position.y < cupid.startPosition.y Then
							cupid.cupidTransform.position += Vector3.up * p.cupidMoveSpeed * CupheadTime.Delta
						End If
						Yield Nothing
					ElseIf cupid.cupidTransform.position.y > p.cupidDropMaxY Then
						cupid.cupidTransform.position += Vector3.down * p.cupidMoveSpeed * CupheadTime.Delta
					End If
				ElseIf player.transform.parent IsNot cupid.cupidTransform AndAlso player2.transform.parent IsNot cupid.cupidTransform AndAlso cupid.cupidTransform.position.y >= p.cupidDropMaxY Then
					If cupid.cupidTransform.position.y < cupid.startPosition.y Then
						cupid.cupidTransform.position += Vector3.up * p.cupidMoveSpeed * CupheadTime.Delta
					End If
					Yield Nothing
				ElseIf cupid.cupidTransform.position.y > p.cupidDropMaxY Then
					cupid.cupidTransform.position += Vector3.down * p.cupidMoveSpeed * CupheadTime.Delta
				End If
			ElseIf player.transform.parent IsNot cupid.cupidTransform AndAlso cupid.cupidTransform.position.y >= p.cupidDropMaxY Then
				If cupid.cupidTransform.position.y < cupid.startPosition.y Then
					cupid.cupidTransform.position += Vector3.up * p.cupidMoveSpeed * CupheadTime.Delta
				End If
				Yield Nothing
			ElseIf cupid.cupidTransform.position.y > p.cupidDropMaxY Then
				cupid.cupidTransform.position += Vector3.down * p.cupidMoveSpeed * CupheadTime.Delta
			End If
			If cupid.cupidTransform.position.y <= p.cupidDropMaxY Then
				If Not cupid.playSound Then
					AudioManager.Play("sally_platform_cherub_full_travel")
					Me.emitAudioFromObject.Add("sally_platform_cherub_full_travel")
					cupid.playSound = True
				End If
				cupid.acceptableLevel = True
			Else
				cupid.acceptableLevel = False
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06002C01 RID: 11265 RVA: 0x0019BF04 File Offset: 0x0019A304
	Private Iterator Function check_bools_cr() As IEnumerator
		Dim chandelierWarning As Boolean = False
		While Not Me.cupids(0).acceptableLevel OrElse Not Me.cupids(1).acceptableLevel
			If(Me.cupids(0).acceptableLevel OrElse Me.cupids(1).acceptableLevel) AndAlso Not chandelierWarning Then
				chandelierWarning = True
				MyBase.StartCoroutine(Me.chandelier_cr(True))
			End If
			Yield Nothing
		End While
		MyBase.StartCoroutine(Me.chandelier_cr(False))
		SallyStagePlayLevelBackgroundHandler.HUSBAND_GONE = True
		Dim clamp As Single = 20F
		While Me.husband.transform.position.x > clamp OrElse Me.husband.transform.position.x < -clamp
			Yield Nothing
		End While
		Me.husbandMoving = False
		Me.husband.SetTrigger("Dead")
		Me.properties.DealDamageToNextNamedState()
		Yield CupheadTime.WaitForSeconds(Me, 0.75F)
		Me.dropChandelier = True
		Yield Nothing
		Dim t As Single = 0F
		Dim frameTime As Single = 0F
		Dim moveTime As Single = 0.3F
		Dim start As Vector3 = New Vector3(Me.chandelierStartPosX, Me.chandelier.transform.position.y)
		Dim [end] As Vector3 = New Vector3(Me.chandelierStartPosX, Me.sallyBackground.transform.position.y - 70F)
		While t < moveTime
			frameTime += CupheadTime.Delta
			t += CupheadTime.Delta
			If frameTime > 0.041666668F Then
				Dim num As Single = EaseUtils.Ease(EaseUtils.EaseType.linear, 0F, 1F, t / moveTime)
				Me.chandelier.transform.position = Vector2.Lerp(start, [end], num)
				frameTime -= 0.041666668F
			End If
			Yield Nothing
		End While
		Yield Nothing
		CupheadLevelCamera.Current.Shake(10F, 0.4F, False)
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		Return
	End Function

	' Token: 0x06002C02 RID: 11266 RVA: 0x0019BF20 File Offset: 0x0019A320
	Private Iterator Function chandelier_cr(isWarning As Boolean) As IEnumerator
		Dim t As Single = 0F
		Me.chandelier.GetComponent(Of Animator)().Play("Shake")
		While Not Me.dropChandelier
			t += CupheadTime.Delta
			If t > 0.8F AndAlso isWarning Then
				Exit While
			End If
			Yield Nothing
		End While
		If isWarning Then
			Me.chandelier.GetComponent(Of Animator)().SetTrigger("OnSlump")
			AudioManager.Play("sally_chandelier_warning")
		Else
			Me.chandelier.GetComponent(Of Animator)().Play("Off")
			AudioManager.Play("sally_chandelier_impact")
		End If
		Yield Nothing
		Return
	End Function

	' Token: 0x06002C03 RID: 11267 RVA: 0x0019BF42 File Offset: 0x0019A342
	Private Sub OnPhase2()
		If Not SallyStagePlayLevelBackgroundHandler.HUSBAND_GONE Then
			MyBase.StartCoroutine(Me.just_married_cr())
		Else
			MyBase.StartCoroutine(Me.crying_cr())
		End If
	End Sub

	' Token: 0x06002C04 RID: 11268 RVA: 0x0019BF70 File Offset: 0x0019A370
	Public Sub RollUpCupids()
		For Each cupid As SallyStagePlayLevelBackgroundHandler.Cupid In Me.cupids
			MyBase.StartCoroutine(Me.roll_up_cupids_cr(cupid))
		Next
	End Sub

	' Token: 0x06002C05 RID: 11269 RVA: 0x0019BFAC File Offset: 0x0019A3AC
	Private Iterator Function roll_up_cupids_cr(cupid As SallyStagePlayLevelBackgroundHandler.Cupid) As IEnumerator
		Dim t As Single = 0F
		Dim frameTime As Single = 0F
		Dim moveTime As Single = 3.5F
		Dim [end] As Vector3 = New Vector3(cupid.cupidTransform.position.x, 800F)
		cupid.cupidTransform.GetComponent(Of Collider2D)().enabled = False
		While t < moveTime
			frameTime += CupheadTime.Delta
			t += CupheadTime.Delta
			If frameTime > 0.041666668F Then
				Dim num As Single = EaseUtils.Ease(EaseUtils.EaseType.linear, 0F, 1F, t / moveTime)
				cupid.cupidTransform.position = Vector2.Lerp(cupid.cupidTransform.position, [end], num)
				frameTime -= 0.041666668F
			End If
			Yield Nothing
		End While
		Yield Nothing
		Return
	End Function

	' Token: 0x06002C06 RID: 11270 RVA: 0x0019BFC8 File Offset: 0x0019A3C8
	Private Iterator Function just_married_cr() As IEnumerator
		Dim t As Single = 0F
		Dim frameTime As Single = 0F
		Dim moveTime As Single = 1.5F
		Dim [end] As Vector3 = New Vector3(Me.husband.transform.position.x, Me.car.position.y)
		While Me.sally.state <> SallyStagePlayLevelSally.State.Transition
			Yield Nothing
		End While
		Me.sally.animator.SetTrigger("OnPhase2")
		Me.priest.SetTrigger("CarAppeared")
		Me.husbandMoving = False
		Me.husband.SetTrigger("Married")
		Yield Me.husband.WaitForAnimationToEnd(Me, "Tada_Start", False, True)
		Me.sallyBackground.Play("Wave")
		AudioManager.Play("sally_ph1_bg_car_enter")
		Me.sallyBackground.transform.parent = Me.car.transform
		Me.sallyBackground.transform.position = Me.carRoot.transform.position
		Me.sallyBackground.transform.position = Me.carRoot.transform.position
		While t < moveTime
			frameTime += CupheadTime.Delta
			t += CupheadTime.Delta
			If frameTime > 0.041666668F Then
				Dim num As Single = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0F, 1F, t / moveTime)
				Me.car.transform.position = Vector2.Lerp(Me.car.transform.position, [end], num)
				frameTime -= 0.041666668F
			End If
			Yield Nothing
		End While
		AudioManager.PlayLoop("sally_ph1_bg_car_loop")
		Me.husband.SetTrigger("Drive")
		Me.husband.transform.parent = Me.car.transform
		AudioManager.Play("sally_ph1_bg_car_exit")
		AudioManager.[Stop]("sally_ph1_bg_car_loop")
		t = 0F
		frameTime = 0F
		moveTime = 2F
		[end].x = 1140F
		Yield CupheadTime.WaitForSeconds(Me, 0.5F)
		While t < moveTime
			frameTime += CupheadTime.Delta
			t += CupheadTime.Delta
			If frameTime > 0.041666668F Then
				Dim num2 As Single = EaseUtils.Ease(EaseUtils.EaseType.easeInSine, 0F, 1F, t / moveTime)
				Me.car.transform.position = Vector2.Lerp(Me.car.transform.position, [end], num2)
				frameTime -= 0.041666668F
			End If
			Yield Nothing
		End While
		Me.CloseCurtain(SallyStagePlayLevelBackgroundHandler.Backgrounds.House)
		Yield CupheadTime.WaitForSeconds(Me, Me.curtainWaitTime)
		Me.HaltCoroutines()
		Me.OpenCurtain(SallyStagePlayLevelBackgroundHandler.Backgrounds.House)
		Me.residence.StartPhase2(Me.parent, Me.properties)
		Me.sally.StartPhase2()
		Me.StartPeeking()
		Yield Nothing
		Return
	End Function

	' Token: 0x06002C07 RID: 11271 RVA: 0x0019BFE4 File Offset: 0x0019A3E4
	Private Iterator Function cry_sound_cr() As IEnumerator
		Yield Me.sallyBackground.WaitForAnimationToEnd(Me, "Run", False, True)
		AudioManager.Play("sally_cry")
		Me.emitAudioFromObject.Add("sally_cry")
		Yield Nothing
		Return
	End Function

	' Token: 0x06002C08 RID: 11272 RVA: 0x0019C000 File Offset: 0x0019A400
	Private Iterator Function crying_cr() As IEnumerator
		While Me.sally.state <> SallyStagePlayLevelSally.State.Transition
			Yield Nothing
		End While
		Me.sally.animator.SetTrigger("OnPhase2")
		Yield CupheadTime.WaitForSeconds(Me, 1.5F)
		Me.priest.Play("Tuck_Down_Disappear")
		MyBase.StartCoroutine(Me.cry_sound_cr())
		Me.sallyBackground.Play("Run")
		Yield Me.sallyBackground.WaitForAnimationToEnd(Me, "Run_End", False, True)
		Me.CloseCurtain(SallyStagePlayLevelBackgroundHandler.Backgrounds.Nunnery)
		Dim ex As SallyStagePlayHusbandExplosion = Me.husband.GetComponent(Of SallyStagePlayHusbandExplosion)()
		If ex IsNot Nothing Then
			ex.StopExplosions()
		End If
		Yield CupheadTime.WaitForSeconds(Me, Me.curtainWaitTime)
		Me.HaltCoroutines()
		Me.OpenCurtain(SallyStagePlayLevelBackgroundHandler.Backgrounds.Nunnery)
		Me.residence.StartPhase2(Me.parent, Me.properties)
		Me.sally.StartPhase2()
		Me.StartPeeking()
		Yield Nothing
		Return
	End Function

	' Token: 0x06002C09 RID: 11273 RVA: 0x0019C01C File Offset: 0x0019A41C
	Private Sub HaltCoroutines()
		For Each coroutine As Coroutine In Me.phaseDependentCoroutines
			MyBase.StopCoroutine(coroutine)
		Next
		Me.phaseDependentCoroutines.Clear()
	End Sub

	' Token: 0x06002C0A RID: 11274 RVA: 0x0019C084 File Offset: 0x0019A484
	Private Sub StartPeeking()
		If SallyStagePlayLevelBackgroundHandler.HUSBAND_GONE Then
			Me.phaseDependentCoroutines.Add(MyBase.StartCoroutine(Me.peek_cr(Me.priestPhase2, Me.priestRoots)))
		Else
			Me.phaseDependentCoroutines.Add(MyBase.StartCoroutine(Me.peek_cr(Me.husbandPhase2, Me.husbandRoots)))
		End If
	End Sub

	' Token: 0x06002C0B RID: 11275 RVA: 0x0019C0E8 File Offset: 0x0019A4E8
	Private Iterator Function peek_cr(animator As Animator, roots As Transform()) As IEnumerator
		Dim waitTime As Single = Global.UnityEngine.Random.Range(8F, 20F)
		While True
			Yield CupheadTime.WaitForSeconds(Me, waitTime)
			Yield Nothing
			Dim rootChosen As Integer = Global.UnityEngine.Random.Range(0, roots.Length)
			animator.GetComponent(Of Transform)().position = roots(rootChosen).position
			animator.GetComponent(Of Transform)().SetScale(New Single?(roots(rootChosen).localScale.x), Nothing, Nothing)
			If SallyStagePlayLevelBackgroundHandler.HUSBAND_GONE AndAlso rootChosen = 1 Then
				animator.SetBool("isDiag", True)
			Else
				animator.SetBool("isDiag", Rand.Bool())
			End If
			animator.SetTrigger("Peek")
			waitTime = Global.UnityEngine.Random.Range(8F, 20F)
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06002C0C RID: 11276 RVA: 0x0019C114 File Offset: 0x0019A514
	Private Sub OnPhase3()
		Me.HaltCoroutines()
		For Each transform As Transform In Me.purgSwingies
			Me.phaseDependentCoroutines.Add(MyBase.StartCoroutine(Me.swing_cr(transform)))
		Next
		If SallyStagePlayLevelBackgroundHandler.HUSBAND_GONE Then
			Me.priestPhase2.Play("Shake")
		Else
			Me.husbandPhase2.Play("Cry")
		End If
		MyBase.StartCoroutine(Me.phase3_background_cr())
	End Sub

	' Token: 0x06002C0D RID: 11277 RVA: 0x0019C19C File Offset: 0x0019A59C
	Private Iterator Function phase3_background_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 2F)
		Me.CloseCurtain(SallyStagePlayLevelBackgroundHandler.Backgrounds.Purgatory)
		Yield CupheadTime.WaitForSeconds(Me, Me.curtainWaitTime)
		Me.OpenCurtain(SallyStagePlayLevelBackgroundHandler.Backgrounds.Purgatory)
		Yield Nothing
		Return
	End Function

	' Token: 0x06002C0E RID: 11278 RVA: 0x0019C1B8 File Offset: 0x0019A5B8
	Private Sub OnPhase4()
		Me.HaltCoroutines()
		For Each transform As Transform In Me.finaleSwingies
			Me.phaseDependentCoroutines.Add(MyBase.StartCoroutine(Me.swing_cr(transform)))
		Next
		MyBase.StartCoroutine(Me.phase4_background_cr())
		If SallyStagePlayLevelBackgroundHandler.HUSBAND_GONE Then
			Me.husbandDeadObject.SetActive(True)
		Else
			Me.husbandAliveObject.SetActive(True)
		End If
	End Sub

	' Token: 0x06002C0F RID: 11279 RVA: 0x0019C238 File Offset: 0x0019A638
	Private Iterator Function phase4_background_cr() As IEnumerator
		Me.CloseCurtain(SallyStagePlayLevelBackgroundHandler.Backgrounds.Finale)
		Yield CupheadTime.WaitForSeconds(Me, Me.curtainWaitTime)
		Me.OpenCurtain(SallyStagePlayLevelBackgroundHandler.Backgrounds.Finale)
		Yield Nothing
		Return
	End Function

	' Token: 0x0400349A RID: 13466
	Private Const FRAME_TIME As Single = 0.041666668F

	' Token: 0x0400349B RID: 13467
	<Header("Main")>
	<SerializeField()>
	Private sally As SallyStagePlayLevelSally

	' Token: 0x0400349C RID: 13468
	<SerializeField()>
	Private curtain As Transform

	' Token: 0x0400349D RID: 13469
	<SerializeField()>
	Private curtainSprite As Transform

	' Token: 0x0400349E RID: 13470
	<SerializeField()>
	Private curtainShadow As Transform

	' Token: 0x0400349F RID: 13471
	<SerializeField()>
	Private curtainUpRoot As Transform

	' Token: 0x040034A0 RID: 13472
	<SerializeField()>
	Private flickeringLights As SpriteRenderer()

	' Token: 0x040034A1 RID: 13473
	<SerializeField()>
	Private applauseHandler As SallyStagePlayApplauseHandler

	' Token: 0x040034A2 RID: 13474
	<Header("Church")>
	<SerializeField()>
	Private churchSwingies As Transform()

	' Token: 0x040034A3 RID: 13475
	<SerializeField()>
	Private cupids As SallyStagePlayLevelBackgroundHandler.Cupid()

	' Token: 0x040034A4 RID: 13476
	<SerializeField()>
	Private priest As Animator

	' Token: 0x040034A5 RID: 13477
	<SerializeField()>
	Private husband As Animator

	' Token: 0x040034A6 RID: 13478
	<SerializeField()>
	Private sallyBackground As Animator

	' Token: 0x040034A7 RID: 13479
	<SerializeField()>
	Private car As Transform

	' Token: 0x040034A8 RID: 13480
	<SerializeField()>
	Private carRoot As Transform

	' Token: 0x040034A9 RID: 13481
	<SerializeField()>
	Private chandelier As Transform

	' Token: 0x040034AA RID: 13482
	<SerializeField()>
	Private sallyRoot As Transform

	' Token: 0x040034AB RID: 13483
	<Header("Residence")>
	<SerializeField()>
	Private residence As SallyStagePlayLevelHouse

	' Token: 0x040034AC RID: 13484
	<SerializeField()>
	Private husbandPhase2 As Animator

	' Token: 0x040034AD RID: 13485
	<SerializeField()>
	Private husbandRoots As Transform()

	' Token: 0x040034AE RID: 13486
	<SerializeField()>
	Private priestPhase2 As Animator

	' Token: 0x040034AF RID: 13487
	<SerializeField()>
	Private priestRoots As Transform()

	' Token: 0x040034B0 RID: 13488
	<Header("Purgatory")>
	<SerializeField()>
	Private purgSwingies As Transform()

	' Token: 0x040034B1 RID: 13489
	<Header("Finale")>
	<SerializeField()>
	Private finaleSwingies As Transform()

	' Token: 0x040034B2 RID: 13490
	<SerializeField()>
	Private husbandAliveObject As GameObject

	' Token: 0x040034B3 RID: 13491
	<SerializeField()>
	Private husbandDeadObject As GameObject

	' Token: 0x040034B4 RID: 13492
	<Header("Backgrounds")>
	<SerializeField()>
	Private backgrounds As GameObject()

	' Token: 0x040034B5 RID: 13493
	Private fadeWaitMinSecond As Single = 8F

	' Token: 0x040034B6 RID: 13494
	Private fadeWaitMaxSecond As Single = 25F

	' Token: 0x040034B7 RID: 13495
	Private curtainWaitTime As Single = 2.5F

	' Token: 0x040034B8 RID: 13496
	Private chandelierStartPosX As Single

	' Token: 0x040034B9 RID: 13497
	Private husbandMoving As Boolean = True

	' Token: 0x040034BA RID: 13498
	Private dropChandelier As Boolean

	' Token: 0x040034BB RID: 13499
	Private curtainStartPos As Vector3

	' Token: 0x040034BC RID: 13500
	Private curtainShadowStartPos As Vector3

	' Token: 0x040034BD RID: 13501
	Private properties As LevelProperties.SallyStagePlay

	' Token: 0x040034BE RID: 13502
	Private parent As SallyStagePlayLevel

	' Token: 0x040034BF RID: 13503
	Private phaseDependentCoroutines As List(Of Coroutine)

	' Token: 0x020007A7 RID: 1959
	Public Enum Backgrounds
		' Token: 0x040034C1 RID: 13505
		Church
		' Token: 0x040034C2 RID: 13506
		House
		' Token: 0x040034C3 RID: 13507
		Nunnery
		' Token: 0x040034C4 RID: 13508
		Purgatory
		' Token: 0x040034C5 RID: 13509
		Finale
	End Enum

	' Token: 0x020007A8 RID: 1960
	<Serializable()>
	Public Class Cupid
		' Token: 0x040034C6 RID: 13510
		Public cupidTransform As Transform

		' Token: 0x040034C7 RID: 13511
		Public startPosition As Vector3

		' Token: 0x040034C8 RID: 13512
		Public acceptableLevel As Boolean

		' Token: 0x040034C9 RID: 13513
		Public playSound As Boolean
	End Class
End Class
