Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020004B5 RID: 1205
Public Class AirplaneLevelBulldogPlane
	Inherits LevelProperties.Airplane.Entity

	' Token: 0x060013CA RID: 5066 RVA: 0x000AF164 File Offset: 0x000AD564
	Private Sub Start()
		Me.state = AirplaneLevelBulldogPlane.State.Intro
		Me.startPosY = 256F
		Me.baseX = MyBase.transform.position.x
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = Me.bullDogPlane.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		AddHandler Me.bulldogParachute.GetComponent(Of DamageReceiver)().OnDamageTaken, AddressOf Me.OnDamageTaken
		AddHandler Me.bulldogCatAttack.GetComponent(Of DamageReceiver)().OnDamageTaken, AddressOf Me.OnDamageTaken
		Me.bulldogParachute.gameObject.SetActive(False)
		Me.bulldogCatAttack.gameObject.SetActive(False)
		MyBase.StartCoroutine(Me.idle_timer_cr())
		MyBase.StartCoroutine(Me.rotate_cr())
	End Sub

	' Token: 0x060013CB RID: 5067 RVA: 0x000AF243 File Offset: 0x000AD643
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x060013CC RID: 5068 RVA: 0x000AF259 File Offset: 0x000AD659
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x060013CD RID: 5069 RVA: 0x000AF274 File Offset: 0x000AD674
	Private Sub FixedUpdate()
		If Me.state = AirplaneLevelBulldogPlane.State.Intro Then
			Return
		End If
		Me.moveTime += Time.fixedDeltaTime
		Dim vector As Vector2 = MyBase.transform.position
		vector.y = Mathf.Sin(Me.moveTime / 0.3F) * 4F
		If Me.bounceXTimer > 0F Then
			Me.bounceXTimer -= CupheadTime.FixedDelta * 3F * If((Me.bounceXTimer <= 0.5F), 0.25F, 1F)
			Dim num As Single
			If Me.bounceXTimer > 0.5F Then
				num = Mathf.Sin(Me.bounceXTimer * 3.1415927F) * 30F
			Else
				Dim num2 As Single = EaseUtils.EaseInOutSine(30F, 0F, 1F - Me.bounceXTimer * 2F)
				Dim num3 As Single = num2
				Me.bounceX = num2
				num = num3
			End If
			Me.bounceX = num
			Me.bounceX *= Me.bounceXDir
		Else
			Me.bounceXTimer = 0F
			Me.bounceX = 0F
		End If
		If Me.bounceYTimer > 0F Then
			Me.bounceYTimer -= CupheadTime.FixedDelta * If((Not Me.exitBounce), 1.7F, 1.6F)
			Dim num4 As Single
			If Me.bounceYTimer > 0.5F Then
				num4 = Mathf.Sin(Me.bounceYTimer * 3.1415927F) * If((Not Me.exitBounce), 60F, 40F)
			Else
				Dim num5 As Single = EaseUtils.EaseInOutSine(If((Not Me.exitBounce), 60F, 40F), 0F, 1F - Me.bounceYTimer * 2F)
				Dim num3 As Single = num5
				Me.bounceY = num5
				num4 = num3
			End If
			Me.bounceY = num4
		Else
			Me.bounceYTimer = 0F
			Me.bounceY = 0F
		End If
		MyBase.transform.SetPosition(New Single?(Me.baseX + Mathf.Sin(Me.wobbleTimer * 3F) * Me.wobbleX + Me.bounceX), New Single?(Me.startPosY + vector.y - Me.bounceY + Mathf.Sin(Me.wobbleTimer * 2F) * Me.wobbleY), Nothing)
		Me.wobbleTimer += CupheadTime.FixedDelta * Me.wobbleSpeed
		If Not Me.isDead Then
			Me.smokePuffLTimer -= CupheadTime.FixedDelta
			Me.smokePuffRTimer -= CupheadTime.FixedDelta
			If Me.smokePuffLTimer <= 0F Then
				Me.smokePuff(Me.smokePuffLCounter Mod 3).Play((Me.smokePuffLCounter Mod 4).ToString(), 0, 0F)
				Me.smokePuff(Me.smokePuffLCounter Mod 3).transform.localPosition = Vector3.left * 300F + Vector3.up * 50F
				Me.smokePuffLTimer += 0.25F
				Me.smokePuffLCounter += 1
			End If
			If Me.smokePuffRTimer <= 0F Then
				Me.smokePuff(Me.smokePuffRCounter Mod 3 + 3).Play((Me.smokePuffRCounter Mod 4).ToString(), 0, 0F)
				Me.smokePuff(Me.smokePuffRCounter Mod 3 + 3).transform.localPosition = Vector3.right * 300F + Vector3.up * 50F
				Me.smokePuffRTimer += 0.27F
				Me.smokePuffRCounter += 1
			End If
		End If
		For i As Integer = 0 To Me.smokePuff.Length - 1
			Me.smokePuff(i).transform.localPosition += New Vector3(CSng(If((i >= 3), (-3), 3)), 2F - 4F * Me.smokePuff(i).GetCurrentAnimatorStateInfo(0).normalizedTime) * CupheadTime.FixedDelta * 100F
		Next
	End Sub

	' Token: 0x060013CE RID: 5070 RVA: 0x000AF700 File Offset: 0x000ADB00
	Private Iterator Function rotate_cr() As IEnumerator
		Dim t As Single = 0F
		Dim time As Single = 4F
		Dim maxAngle As Single = 1F
		MyBase.transform.rotation = Quaternion.Euler(New Vector3(0F, 0F, -maxAngle))
		While True
			While t < time
				t += CupheadTime.Delta
				MyBase.transform.rotation = Quaternion.Euler(New Vector3(0F, 0F, Mathf.Lerp(-maxAngle, maxAngle, EaseUtils.EaseInOutSine(0F, 1F, t / time))))
				Yield Nothing
			End While
			t = 0F
			maxAngle = -maxAngle
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060013CF RID: 5071 RVA: 0x000AF71B File Offset: 0x000ADB1B
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		If Me.dontDamage Then
			Return
		End If
		MyBase.properties.DealDamage(info.damage)
	End Sub

	' Token: 0x060013D0 RID: 5072 RVA: 0x000AF73A File Offset: 0x000ADB3A
	Public Overrides Sub LevelInit(properties As LevelProperties.Airplane)
		MyBase.LevelInit(properties)
		Me.sideString = New PatternString(properties.CurrentState.parachute.sideString, True)
		MyBase.StartCoroutine(Me.intro_cr())
	End Sub

	' Token: 0x060013D1 RID: 5073 RVA: 0x000AF76C File Offset: 0x000ADB6C
	Private Iterator Function intro_cr() As IEnumerator
		Me.leaderIntroBG.SetTrigger("Continue")
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		Yield Me.bullDogPlane.WaitForAnimationToStart(Me, "Intro", False)
		Dim target As Integer = Animator.StringToHash(Me.bullDogPlane.GetLayerName(0) + ".Intro")
		While Me.bullDogPlane.GetCurrentAnimatorStateInfo(0).fullPathHash = target
			Dim s As Single = Me.bullDogPlane.GetCurrentAnimatorStateInfo(0).normalizedTime
			If s > 0.7F AndAlso s < 0.95F Then
				CType(Level.Current, AirplaneLevel).UpdateShadow(1F - Mathf.Sin(Mathf.InverseLerp(0.7F, 0.95F, s) * 3.1415927F) * 0.2F)
			Else
				CType(Level.Current, AirplaneLevel).UpdateShadow(1F)
			End If
			Yield wait
		End While
		CType(Level.Current, AirplaneLevel).UpdateShadow(1F)
		Yield CupheadTime.WaitForSeconds(Me, 0.35F)
		Me.SFX_DOGFIGHT_BulldogPlane_Loop()
		Me.SFX_DOGFIGHT_Intro_BulldogPlaneDecend()
		MyBase.StartCoroutine(Me.turret_cr())
		MyBase.StartCoroutine(Me.mainattack_cr())
		Dim t As Single = 0F
		Dim time As Single = 0.8F
		Dim endTime As Single = 0.4F
		Dim start As Single = MyBase.transform.position.y
		MyBase.StartCoroutine(Me.scale_in_cr())
		While t < time
			t += CupheadTime.FixedDelta
			Dim val As Single = EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, 0F, 1F, t / time)
			MyBase.transform.SetPosition(Nothing, New Single?(Mathf.Lerp(start, 156F, val)), Nothing)
			Yield wait
		End While
		t = 0F
		start = MyBase.transform.position.y
		While t < endTime
			t += CupheadTime.FixedDelta
			Dim val2 As Single = EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, 0F, 1F, t / endTime)
			MyBase.transform.SetPosition(Nothing, New Single?(Mathf.Lerp(start, 256F, val2)), Nothing)
			Yield wait
		End While
		MyBase.transform.SetPosition(Nothing, New Single?(256F), Nothing)
		If Me.state = AirplaneLevelBulldogPlane.State.Intro Then
			Me.state = AirplaneLevelBulldogPlane.State.Main
		End If
		MyBase.StartCoroutine(Me.move_cr())
		Yield Nothing
		Return
	End Function

	' Token: 0x060013D2 RID: 5074 RVA: 0x000AF788 File Offset: 0x000ADB88
	Private Iterator Function scale_in_cr() As IEnumerator
		MyBase.transform.localScale = New Vector3(0.8F, 0.8F)
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		Dim t As Single = 0F
		Dim frameTime As Single = 0F
		While t < 1.2F
			While frameTime < 0.041666668F
				frameTime += CupheadTime.FixedDelta
				Yield wait
			End While
			t += frameTime
			frameTime -= 0.041666668F
			MyBase.transform.localScale = Vector3.Lerp(New Vector3(0.8F, 0.8F), New Vector3(1F, 1F), EaseUtils.EaseOutSine(0F, 1F, Mathf.InverseLerp(0F, 1.2F, t)))
		End While
		MyBase.transform.localScale = New Vector3(1F, 1F)
		Return
	End Function

	' Token: 0x060013D3 RID: 5075 RVA: 0x000AF7A3 File Offset: 0x000ADBA3
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.WORKAROUND_NullifyFields()
	End Sub

	' Token: 0x060013D4 RID: 5076 RVA: 0x000AF7B4 File Offset: 0x000ADBB4
	Private Iterator Function mainattack_cr() As IEnumerator
		Dim p As LevelProperties.Airplane.Main = MyBase.properties.CurrentState.main
		Dim attackType As PatternString = New PatternString(p.attackType, True)
		While True
			If Me.firstAttack Then
				Yield CupheadTime.WaitForSeconds(Me, 0.6F)
				Yield CupheadTime.WaitForSeconds(Me, p.firstAttackDelay)
				Me.firstAttack = False
			Else
				Yield CupheadTime.WaitForSeconds(Me, p.attackDelayRange.RandomFloat())
			End If
			Dim c As Char = attackType.PopLetter()
			If c <> "P"c Then
				If c = "T"c Then
					Yield MyBase.StartCoroutine(Me.catattack_cr())
				End If
			Else
				Yield MyBase.StartCoroutine(Me.parachute_cr())
			End If
			Me.state = AirplaneLevelBulldogPlane.State.Main
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060013D5 RID: 5077 RVA: 0x000AF7D0 File Offset: 0x000ADBD0
	Private Iterator Function idle_timer_cr() As IEnumerator
		Dim pickSide As Boolean = Rand.Bool()
		While True
			Yield CupheadTime.WaitForSeconds(Me, Global.UnityEngine.Random.Range(2F, 3F))
			If Me.state = AirplaneLevelBulldogPlane.State.Main Then
				pickSide = If((Not Rand.Bool()), (MyBase.transform.position.x > Me.canteenPlane.transform.position.x), Rand.Bool())
				Dim side As String = If((Not pickSide), "Right", "Left")
				Me.bullDogPlane.SetTrigger("OnIdle" + side)
				Me.bullDogPlane.SetInteger("IdleLoopCount", If((pickSide <> MyBase.transform.position.x > Me.canteenPlane.transform.position.x), 2, 0))
				Yield Me.bullDogPlane.WaitForAnimationToStart(Me, "Idle", False)
			End If
			While Me.state <> AirplaneLevelBulldogPlane.State.Main
				Yield Nothing
			End While
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060013D6 RID: 5078 RVA: 0x000AF7EC File Offset: 0x000ADBEC
	Private Iterator Function move_cr() As IEnumerator
		Me.movingRight = Rand.Bool()
		Dim t As Single = 0F
		Dim time As Single = MyBase.properties.CurrentState.main.moveTime
		Dim start As Single = 0F
		Dim [end] As Single = 0F
		Dim speedModifier As Single = 1F
		While True
			t = 0F
			start = MyBase.transform.position.x
			[end] = If((Not Me.movingRight), (-245F), 245F)
			While t < time
				t += CupheadTime.FixedDelta * speedModifier
				speedModifier = Mathf.Clamp(speedModifier + If((Me.state <> AirplaneLevelBulldogPlane.State.Main), (-0.01F), 0.01F), 0F, 1F)
				Dim val As Single = t / time
				Me.baseX = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, start, [end], val)
				Yield New WaitForFixedUpdate()
			End While
			MyBase.transform.SetPosition(New Single?([end]), Nothing, Nothing)
			Me.movingRight = Not Me.movingRight
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060013D7 RID: 5079 RVA: 0x000AF808 File Offset: 0x000ADC08
	Private Iterator Function turret_cr() As IEnumerator
		Dim p As LevelProperties.Airplane.Turrets = MyBase.properties.CurrentState.turrets
		Dim positionString As PatternString = New PatternString(p.positionString, True, True)
		While True
			Yield CupheadTime.WaitForSeconds(Me, p.attackDelayRange.RandomFloat())
			Me.turretSpawnPoints(positionString.PopInt()).StartAttack(p.velocityX, p.velocityY, p.gravity)
		End While
		Return
	End Function

	' Token: 0x060013D8 RID: 5080 RVA: 0x000AF823 File Offset: 0x000ADC23
	Public Sub StartRocket()
		MyBase.StartCoroutine(Me.rocket_cr())
	End Sub

	' Token: 0x060013D9 RID: 5081 RVA: 0x000AF834 File Offset: 0x000ADC34
	Private Iterator Function rocket_cr() As IEnumerator
		Dim p As LevelProperties.Airplane.Rocket = MyBase.properties.CurrentState.rocket
		Dim delayString As PatternString = New PatternString(p.attackDelayString, True, True)
		Dim dirString As PatternString = New PatternString(p.attackOrderString, True, True)
		Me.hydrantAttackBG.Play("Fly")
		Yield Me.hydrantAttackBG.WaitForAnimationToEnd(Me, "Fly", False, True)
		While True
			Yield CupheadTime.WaitForSeconds(Me, CSng(delayString.PopInt()))
			Dim position As Vector3 = If((dirString.PopLetter() <> "R"c), Me.rocketSpawnLeft.position, Me.rocketSpawnRight.position)
			Me.rocketPrefab.Create(PlayerManager.GetNext(), position, p.homingSpeed, p.homingRotation, p.homingHP, p.homingTime)
		End While
		Return
	End Function

	' Token: 0x060013DA RID: 5082 RVA: 0x000AF850 File Offset: 0x000ADC50
	Private Iterator Function parachute_cr() As IEnumerator
		Dim onLeft As Boolean = Me.sideString.PopLetter() = "L"c
		Me.bullDogPlane.SetInteger("IdleLoopCount", 3)
		Me.bullDogPlane.SetBool("InParachuteATK", True)
		Yield Me.bullDogPlane.WaitForAnimationToStart(Me, "Parachute_Start", False)
		Me.state = AirplaneLevelBulldogPlane.State.Parachute
		Yield CupheadTime.WaitForSeconds(Me, 0.6666667F)
		Me.exitBounce = True
		Me.bounceYTimer = 1F
		Me.bullDogPlane.GetComponent(Of Collider2D)().enabled = False
		Yield Me.bullDogPlane.WaitForAnimationToEnd(Me, "Parachute_Start", False, False)
		Me.SFX_DOGFIGHT_Bulldog_ParachuteDown()
		Dim posX As Single = If((Not onLeft), 575F, (-575F))
		Dim pos As Vector3 = New Vector3(posX, 0F)
		Dim scale As Single = CSng(If((Not onLeft), (-1), 1))
		Yield CupheadTime.WaitForSeconds(Me, 0.35F)
		Me.bulldogParachute.gameObject.SetActive(True)
		Me.bulldogParachute.StartDescent(pos, scale)
		While Me.bulldogParachute.isMoving
			Yield Nothing
		End While
		Me.bulldogParachute.gameObject.SetActive(False)
		Me.bullDogPlane.SetBool("InParachuteATK", False)
		Yield CupheadTime.WaitForSeconds(Me, 0.125F)
		Me.exitBounce = False
		Me.bounceYTimer = 1F
		Me.SFX_DOGFIGHT_BulldogPlane_ParachuteDownStop()
		Yield Me.bullDogPlane.WaitForAnimationToEnd(Me, "Parachute_End", False, False)
		Me.bullDogPlane.GetComponent(Of Collider2D)().enabled = True
		Return
	End Function

	' Token: 0x060013DB RID: 5083 RVA: 0x000AF86C File Offset: 0x000ADC6C
	Private Iterator Function catattack_cr() As IEnumerator
		Dim p As LevelProperties.Airplane.Triple = MyBase.properties.CurrentState.triple
		Dim onLeft As Boolean = Me.sideString.PopLetter() = "L"c
		Me.bullDogPlane.SetInteger("IdleLoopCount", 3)
		Me.bullDogPlane.SetBool("InParachuteATK", True)
		Me.bullDogPlane.SetBool("OnLeft", onLeft)
		Yield Me.bullDogPlane.WaitForAnimationToStart(Me, "Parachute_Start", False)
		Me.state = AirplaneLevelBulldogPlane.State.CatAttack
		Yield CupheadTime.WaitForSeconds(Me, 0.6666667F)
		Me.exitBounce = True
		Me.bounceYTimer = 1F
		Me.bullDogPlane.GetComponent(Of Collider2D)().enabled = False
		Yield Me.bullDogPlane.WaitForAnimationToEnd(Me, "Parachute_Start", False, False)
		Yield CupheadTime.WaitForSeconds(Me, 0.35F)
		Dim posX As Single = If((Not onLeft), 600F, (-600F))
		Dim startPos As Vector3 = New Vector3(posX, p.yHeight)
		Me.bulldogCatAttack.gameObject.SetActive(True)
		Yield Nothing
		Me.bulldogCatAttack.StartCat(startPos)
		While Me.bulldogCatAttack.isAttacking
			Yield Nothing
		End While
		Me.bulldogCatAttack.gameObject.SetActive(False)
		Me.bullDogPlane.SetBool("InParachuteATK", False)
		Yield CupheadTime.WaitForSeconds(Me, 0.125F)
		Me.exitBounce = False
		Me.bounceYTimer = 1F
		Me.SFX_DOGFIGHT_BulldogPlane_ParachuteDownStop()
		Yield Me.bullDogPlane.WaitForAnimationToEnd(Me, "Parachute_End", False, False)
		Me.bullDogPlane.GetComponent(Of Collider2D)().enabled = True
		Return
	End Function

	' Token: 0x060013DC RID: 5084 RVA: 0x000AF888 File Offset: 0x000ADC88
	Public Sub OnStageChange()
		Me.endPhaseOne = True
		Me.dontDamage = True
		If Me.bulldogCatAttack.isAttacking Then
			Me.bulldogCatAttack.EarlyExit()
		End If
		If Me.bulldogParachute.isMoving Then
			Me.bulldogParachute.EarlyExit()
		End If
	End Sub

	' Token: 0x060013DD RID: 5085 RVA: 0x000AF8D9 File Offset: 0x000ADCD9
	Public Sub BulldogDeath()
		Me.isDead = True
		Me.StopAllCoroutines()
		MyBase.StartCoroutine(Me.death_cr())
	End Sub

	' Token: 0x060013DE RID: 5086 RVA: 0x000AF8F8 File Offset: 0x000ADCF8
	Private Iterator Function death_cr() As IEnumerator
		Me.bullDogPlane.SetBool("Dead", True)
		Me.bullDogPlane.SetBool("InTripleATK", False)
		If Not Me.bulldogParachute.gameObject.activeInHierarchy Then
			Me.bullDogPlane.SetBool("InParachuteATK", False)
		End If
		Yield Me.bullDogPlane.WaitForAnimationToStart(Me, "Death", False)
		Me.startPhaseTwo = True
		Me.SFX_DOGFIGHT_BulldogPlane_StopLoop()
		Me.bullDogPlane.GetComponent(Of Collider2D)().enabled = False
		Me.bullDogPlane.SetLayerWeight(1, 0F)
		Me.bullDogPlane.SetLayerWeight(2, 0F)
		Me.bullDogPlane.SetLayerWeight(3, 0F)
		Yield Me.bullDogPlane.WaitForAnimationToEnd(Me, "Death", False, True)
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Yield Nothing
		Return
	End Function

	' Token: 0x060013DF RID: 5087 RVA: 0x000AF913 File Offset: 0x000ADD13
	Private Sub SFX_DOGFIGHT_Intro_BulldogPlaneDecend()
		AudioManager.Play("sfx_dlc_dogfight_bulldogplane_introdecend")
	End Sub

	' Token: 0x060013E0 RID: 5088 RVA: 0x000AF91F File Offset: 0x000ADD1F
	Private Sub SFX_DOGFIGHT_BulldogPlane_Loop()
		AudioManager.PlayLoop("sfx_dlc_dogfight_bulldogplane_loop")
		AudioManager.FadeSFXVolumeLinear("sfx_dlc_dogfight_bulldogplane_loop", 0.25F, 3F)
	End Sub

	' Token: 0x060013E1 RID: 5089 RVA: 0x000AF93F File Offset: 0x000ADD3F
	Private Sub SFX_DOGFIGHT_BulldogPlane_StopLoop()
		AudioManager.[Stop]("sfx_dlc_dogfight_bulldogplane_loop")
	End Sub

	' Token: 0x060013E2 RID: 5090 RVA: 0x000AF94B File Offset: 0x000ADD4B
	Private Sub SFX_DOGFIGHT_Bulldog_ParachuteDown()
		AudioManager.Play("sfx_dlc_dogfight_p1_bulldog_parachutedown")
		Me.emitAudioFromObject.Add("sfx_dlc_dogfight_p1_bulldog_parachutedown")
		AudioManager.Play("sfx_DLC_Dogfight_P1_Bulldog_ParachuteFlump")
	End Sub

	' Token: 0x060013E3 RID: 5091 RVA: 0x000AF971 File Offset: 0x000ADD71
	Private Sub SFX_DOGFIGHT_BulldogPlane_ParachuteDownStop()
		AudioManager.[Stop]("sfx_dlc_dogfight_p1_bulldog_parachutedown")
	End Sub

	' Token: 0x060013E4 RID: 5092 RVA: 0x000AF980 File Offset: 0x000ADD80
	Private Sub WORKAROUND_NullifyFields()
		Me.leaderIntroBG = Nothing
		Me.hydrantAttackBG = Nothing
		Me.turretSpawnPoints = Nothing
		Me.rocketSpawnLeft = Nothing
		Me.rocketSpawnRight = Nothing
		Me.rocketPrefab = Nothing
		Me.bullDogPlane = Nothing
		Me.bulldogParachute = Nothing
		Me.bulldogCatAttack = Nothing
		Me.canteenPlane = Nothing
		Me.damageDealer = Nothing
		Me.sideString = Nothing
		Me.smokePuff = Nothing
	End Sub

	' Token: 0x04001CD9 RID: 7385
	Private Const MOVE_POS_X As Single = 245F

	' Token: 0x04001CDA RID: 7386
	Private Const PARACHUTE_POS_X As Single = 575F

	' Token: 0x04001CDB RID: 7387
	Private Const PARACHUTE_APPEAR_DELAY As Single = 0.35F

	' Token: 0x04001CDC RID: 7388
	Private Const PARACHUTE_EXIT_BOUNCE_HEIGHT As Single = 40F

	' Token: 0x04001CDD RID: 7389
	Private Const PARACHUTE_EXIT_BOUNCE_SPEED As Single = 1.6F

	' Token: 0x04001CDE RID: 7390
	Private Const PARACHUTE_EXIT_BOUNCE_FRAME_DELAY As Single = 16F

	' Token: 0x04001CDF RID: 7391
	Private Const PARACHUTE_RETURN_BOUNCE_HEIGHT As Single = 60F

	' Token: 0x04001CE0 RID: 7392
	Private Const PARACHUTE_RETURN_BOUNCE_SPEED As Single = 1.7F

	' Token: 0x04001CE1 RID: 7393
	Private Const PARACHUTE_RETURN_BOUNCE_FRAME_DELAY As Single = 3F

	' Token: 0x04001CE2 RID: 7394
	Private Const BUMP_RETURN_BOUNCE_HEIGHT As Single = 30F

	' Token: 0x04001CE3 RID: 7395
	Private Const BUMP_RETURN_BOUNCE_SPEED As Single = 3F

	' Token: 0x04001CE4 RID: 7396
	Private Const CAT_ATTACK_POS_X As Single = 600F

	' Token: 0x04001CE5 RID: 7397
	Private Const CAT_ATTACK_APPEAR_DELAY As Single = 0.35F

	' Token: 0x04001CE6 RID: 7398
	Private Const MOVE_DOWN_POS As Single = 256F

	' Token: 0x04001CE7 RID: 7399
	Private Const MOVE_TIME As Single = 0.3F

	' Token: 0x04001CE8 RID: 7400
	Private Const MOVE_LENGTH As Single = 4F

	' Token: 0x04001CE9 RID: 7401
	Public state As AirplaneLevelBulldogPlane.State

	' Token: 0x04001CEA RID: 7402
	<SerializeField()>
	Private leaderIntroBG As Animator

	' Token: 0x04001CEB RID: 7403
	<SerializeField()>
	Private hydrantAttackBG As Animator

	' Token: 0x04001CEC RID: 7404
	<Header("Roots")>
	<SerializeField()>
	Private turretSpawnPoints As AirplaneLevelTurretDog()

	' Token: 0x04001CED RID: 7405
	<SerializeField()>
	Private rocketSpawnLeft As Transform

	' Token: 0x04001CEE RID: 7406
	<SerializeField()>
	Private rocketSpawnRight As Transform

	' Token: 0x04001CEF RID: 7407
	<Header("Prefabs")>
	<SerializeField()>
	Private rocketPrefab As AirplaneLevelRocket

	' Token: 0x04001CF0 RID: 7408
	<Header("Bulldog")>
	<SerializeField()>
	Private bullDogPlane As Animator

	' Token: 0x04001CF1 RID: 7409
	<SerializeField()>
	Private bulldogParachute As AirplaneLevelBulldogParachute

	' Token: 0x04001CF2 RID: 7410
	<SerializeField()>
	Private bulldogCatAttack As AirplaneLevelBulldogCatAttack

	' Token: 0x04001CF3 RID: 7411
	<SerializeField()>
	Private canteenPlane As GameObject

	' Token: 0x04001CF4 RID: 7412
	Private damageDealer As DamageDealer

	' Token: 0x04001CF5 RID: 7413
	Private damageReceiver As DamageReceiver

	' Token: 0x04001CF6 RID: 7414
	Private moveTime As Single

	' Token: 0x04001CF7 RID: 7415
	Private startPosY As Single

	' Token: 0x04001CF8 RID: 7416
	Private bounceY As Single

	' Token: 0x04001CF9 RID: 7417
	Private bounceYTimer As Single

	' Token: 0x04001CFA RID: 7418
	Private bounceX As Single

	' Token: 0x04001CFB RID: 7419
	Private bounceXTimer As Single

	' Token: 0x04001CFC RID: 7420
	Private bounceXDir As Single = 1F

	' Token: 0x04001CFD RID: 7421
	Private exitBounce As Boolean

	' Token: 0x04001CFE RID: 7422
	Private baseX As Single

	' Token: 0x04001CFF RID: 7423
	Private wobbleTimer As Single

	' Token: 0x04001D00 RID: 7424
	<SerializeField()>
	Private wobbleX As Single = 10F

	' Token: 0x04001D01 RID: 7425
	<SerializeField()>
	Private wobbleY As Single = 10F

	' Token: 0x04001D02 RID: 7426
	<SerializeField()>
	Private wobbleSpeed As Single = 1F

	' Token: 0x04001D03 RID: 7427
	Private movingRight As Boolean

	' Token: 0x04001D04 RID: 7428
	Private dontDamage As Boolean

	' Token: 0x04001D05 RID: 7429
	Public endPhaseOne As Boolean

	' Token: 0x04001D06 RID: 7430
	Private firstAttack As Boolean = True

	' Token: 0x04001D07 RID: 7431
	Private sideString As PatternString

	' Token: 0x04001D08 RID: 7432
	<SerializeField()>
	Private smokePuff As Animator()

	' Token: 0x04001D09 RID: 7433
	Private smokePuffLCounter As Integer

	' Token: 0x04001D0A RID: 7434
	Private smokePuffRCounter As Integer = 2

	' Token: 0x04001D0B RID: 7435
	Private smokePuffLTimer As Single

	' Token: 0x04001D0C RID: 7436
	Private smokePuffRTimer As Single

	' Token: 0x04001D0D RID: 7437
	Public isDead As Boolean

	' Token: 0x04001D0E RID: 7438
	Public startPhaseTwo As Boolean

	' Token: 0x020004B6 RID: 1206
	Public Enum State
		' Token: 0x04001D10 RID: 7440
		Intro
		' Token: 0x04001D11 RID: 7441
		Main
		' Token: 0x04001D12 RID: 7442
		Parachute
		' Token: 0x04001D13 RID: 7443
		TripleAttack
		' Token: 0x04001D14 RID: 7444
		CatAttack
	End Enum
End Class
