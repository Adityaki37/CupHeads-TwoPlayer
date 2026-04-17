Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x0200054E RID: 1358
Public Class ChessQueenLevelQueen
	Inherits LevelProperties.ChessQueen.Entity

	' Token: 0x17000340 RID: 832
	' (get) Token: 0x0600191C RID: 6428 RVA: 0x000E389F File Offset: 0x000E1C9F
	' (set) Token: 0x0600191D RID: 6429 RVA: 0x000E38A7 File Offset: 0x000E1CA7
	Public Property state As ChessQueenLevelQueen.States

	' Token: 0x17000341 RID: 833
	' (get) Token: 0x0600191E RID: 6430 RVA: 0x000E38B0 File Offset: 0x000E1CB0
	' (set) Token: 0x0600191F RID: 6431 RVA: 0x000E38B8 File Offset: 0x000E1CB8
	Public Property activeLightning As ChessQueenLevelLightning

	' Token: 0x06001920 RID: 6432 RVA: 0x000E38C4 File Offset: 0x000E1CC4
	Public Overrides Sub LevelInit(properties As LevelProperties.ChessQueen)
		MyBase.LevelInit(properties)
		AddHandler Level.Current.OnIntroEvent, AddressOf Me.onIntroEventHandler
		Dim turret As LevelProperties.ChessQueen.Turret = properties.CurrentState.turret
		Me.cannons = New List(Of ChessQueenLevelCannon)()
		Me.cannonLeft.SetProperties(turret.leftTurretRange.min, turret.leftTurretRange.max, turret.leftTurretRotationTime, ChessQueenLevelCannon.CannonPosition.Side, turret, Me)
		Me.cannons.Add(Me.cannonLeft)
		Me.cannonMiddle.SetProperties(turret.middleTurretRange.min, turret.middleTurretRange.max, turret.middleTurretRotationTime, ChessQueenLevelCannon.CannonPosition.Center, turret, Me)
		Me.cannons.Add(Me.cannonMiddle)
		Me.cannonRight.SetProperties(turret.rightTurretRange.min, turret.rightTurretRange.max, turret.rightTurretRotationTime, ChessQueenLevelCannon.CannonPosition.Side, turret, Me)
		Me.cannons.Add(Me.cannonRight)
		Me.cannonCycleDirection = MathUtils.PlusOrMinus()
		Me.activeCannonIndex = If((Me.cannonCycleDirection <> -1), 0, 2)
		Me.cannons(Me.activeCannonIndex).IsActive = True
		MyBase.StartCoroutine(Me.check_cannons_cr())
		Me.delayPattern = New PatternString(properties.CurrentState.queen.queenAttackDelayString, True, True)
		Me.lightningPositionPattern = New PatternString(properties.CurrentState.lightning.lightningPositionString, True, True)
		Me.flipPositionString = Rand.Bool()
		Me.positionPattern = New PatternString(properties.CurrentState.movement.queenPositionString, True)
		Me.SFX_KOG_QUEEN_IntroTypeWriter()
	End Sub

	' Token: 0x06001921 RID: 6433 RVA: 0x000E3A68 File Offset: 0x000E1E68
	Protected Overrides Sub OnCollisionEnemyProjectile(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionEnemyProjectile(hit, phase)
		If phase = CollisionPhase.Enter Then
			Dim component As ChessQueenLevelCannonball = hit.GetComponent(Of ChessQueenLevelCannonball)()
			If component Then
				Me.receiveDamage()
				Me.SFX_KOG_QUEEN_CannonHitQueenDing()
				component.HitQueen()
			End If
		End If
	End Sub

	' Token: 0x06001922 RID: 6434 RVA: 0x000E3AA8 File Offset: 0x000E1EA8
	Private Sub receiveDamage()
		MyBase.properties.DealDamage(If((Not PlayerManager.BothPlayersActive()), 10F, ChessKingLevelKing.multiplayerDamageNerf))
		Me.hitFlash.Flash(0.7F)
		If MyBase.properties.CurrentHealth <= 0F Then
			Me.die()
		Else
			Me.mouse.HitQueen()
		End If
		If Not MyBase.animator.GetBool("OnLightning") Then
			Me.headWobbleCurrentAmplitude = Me.headWobbleAmplitude
			Me.headWobbleTimer = 0F
		End If
	End Sub

	' Token: 0x06001923 RID: 6435 RVA: 0x000E3B40 File Offset: 0x000E1F40
	Public Sub StateChanged()
		Me.delayPattern = New PatternString(MyBase.properties.CurrentState.queen.queenAttackDelayString, True, True)
		Me.lightningPositionPattern = New PatternString(MyBase.properties.CurrentState.lightning.lightningPositionString, True, True)
		Me.positionPattern = New PatternString(MyBase.properties.CurrentState.movement.queenPositionString, True)
	End Sub

	' Token: 0x06001924 RID: 6436 RVA: 0x000E3BB4 File Offset: 0x000E1FB4
	Private Sub LateUpdate()
		For Each spriteRenderer As SpriteRenderer In Me.dressRenderers
			spriteRenderer.enabled = MyBase.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") OrElse MyBase.animator.GetCurrentAnimatorStateInfo(0).IsTag("Egg")
		Next
		Me.head.localPosition = New Vector3(Mathf.Sin(Me.headWobbleTimer) * Me.headWobbleCurrentAmplitude, 0F)
		Me.headWobbleTimer += CupheadTime.Delta * Me.headWobbleSpeed
		If Me.headWobbleCurrentAmplitude > 0F Then
			Me.headWobbleCurrentAmplitude -= CupheadTime.Delta * Me.headWobbleDecay
			If Me.headWobbleCurrentAmplitude < 0F Then
				Me.headWobbleCurrentAmplitude = 0F
			End If
		End If
	End Sub

	' Token: 0x06001925 RID: 6437 RVA: 0x000E3CAF File Offset: 0x000E20AF
	Private Sub onIntroEventHandler()
		MyBase.StartCoroutine(Me.intro_cr())
	End Sub

	' Token: 0x06001926 RID: 6438 RVA: 0x000E3CC0 File Offset: 0x000E20C0
	Private Iterator Function intro_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 0.4F)
		MyBase.animator.SetTrigger("Intro")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Intro.Start", False, True)
		MyBase.StartCoroutine(Me.moving_cr())
		Return
	End Function

	' Token: 0x06001927 RID: 6439 RVA: 0x000E3CDC File Offset: 0x000E20DC
	Private Iterator Function check_cannons_cr() As IEnumerator
		While True
			While Me.cannons(Me.activeCannonIndex).IsActive
				Yield Nothing
			End While
			Me.activeCannonIndex = If((Me.cannonCycleDirection <= 0), MathUtilities.PreviousIndex(Me.activeCannonIndex, Me.cannons.Count), MathUtilities.NextIndex(Me.activeCannonIndex, Me.cannons.Count))
			Me.cannons(Me.activeCannonIndex).SetActive(True)
		End While
		Return
	End Function

	' Token: 0x06001928 RID: 6440 RVA: 0x000E3CF8 File Offset: 0x000E20F8
	Private Iterator Function moving_cr() As IEnumerator
		Dim p As LevelProperties.ChessQueen.Queen = MyBase.properties.CurrentState.queen
		Dim moveSpeed As Single = 0F
		Me.lastXPos = MyBase.transform.position.x
		MyBase.animator.Play("MoveSlow", 1, 0F)
		MyBase.animator.Update(0F)
		Me.isMoving = True
		Dim firstMove As Boolean = True
		Dim inLightning As Boolean = False
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		While True
			While Not Me.isMoving
				Yield Nothing
			End While
			Dim elapsed As Single = 0F
			Dim startX As Single = MyBase.transform.position.x
			Dim endX As Single = Mathf.Lerp(CSng(Level.Current.Left) + 150F, CSng(Level.Current.Right) - 150F, (Me.positionPattern.PopFloat() * CSng(If((Not Me.flipPositionString), 1, (-1))) + 1F) / 2F)
			If firstMove AndAlso endX > 0F Then
				endX = -endX
			End If
			firstMove = False
			Me.movingLeft = endX < startX
			Dim distance As Single = Mathf.Abs(endX - startX)
			Dim time As Single = distance / p.queenMovementSpeed
			While elapsed <= time
				If Not Me.isMoving Then
					inLightning = True
				End If
				If inLightning AndAlso Me.isMoving Then
					inLightning = False
					startX = MyBase.transform.position.x
					elapsed = 0F
					distance = Mathf.Abs(endX - startX)
					Dim num As Integer = 0
					While distance < Me.minMoveDistanceAfterLightning AndAlso num < Me.positionPattern.SubStringLength()
						endX = Mathf.Lerp(CSng(Level.Current.Left) + 150F, CSng(Level.Current.Right) - 150F, (Me.positionPattern.PopFloat() * CSng(If((Not Me.flipPositionString), 1, (-1))) + 1F) / 2F)
						distance = Mathf.Abs(endX - startX)
						num += 1
					End While
					Me.movingLeft = endX < startX
					time = distance / p.queenMovementSpeed
					moveSpeed = 0F
				End If
				If(MyBase.animator.GetCurrentAnimatorStateInfo(1).IsName("MoveSlow") AndAlso MyBase.animator.GetCurrentAnimatorStateInfo(1).normalizedTime Mod 1F < 0.16666667F) OrElse MyBase.animator.GetCurrentAnimatorStateInfo(1).IsName("MoveEaseOut") Then
					For Each spriteRenderer As SpriteRenderer In Me.dressRenderers
						spriteRenderer.flipX = Not Me.movingLeft
					Next
				End If
				MyBase.animator.SetBool("Fast", Me.isMoving AndAlso Mathf.Abs(Me.lastXPos - MyBase.transform.position.x) > Me.speedThresholdForFastAnimation AndAlso Me.dressRenderers(0).flipX <> Me.lastXPos - MyBase.transform.position.x > 0F)
				moveSpeed = Mathf.Clamp(moveSpeed + CupheadTime.FixedDelta * If((Not Me.isMoving), (-Me.attackDecel), Me.accel), 0F, 1F)
				Dim t As Single = elapsed / time
				Dim val As Single = If((Not Me.useSineEasing), EaseUtils.EaseInOutArbitraryCoefficient(startX, endX, t, Me.easeCoefficient), EaseUtils.EaseInOutSine(startX, endX, t))
				Me.lastXPos = MyBase.transform.position.x
				MyBase.transform.SetPosition(New Single?(val), Nothing, Nothing)
				elapsed += CupheadTime.FixedDelta * moveSpeed
				Yield wait
			End While
		End While
		Return
	End Function

	' Token: 0x06001929 RID: 6441 RVA: 0x000E3D13 File Offset: 0x000E2113
	Public Sub StartLightning()
		Me.state = ChessQueenLevelQueen.States.Lightning
		MyBase.StartCoroutine(Me.SFX_KOG_QUEEN_VocalAttack_cr())
		MyBase.StartCoroutine(Me.lightning_cr())
	End Sub

	' Token: 0x0600192A RID: 6442 RVA: 0x000E3D38 File Offset: 0x000E2138
	Private Iterator Function lightning_cr() As IEnumerator
		Dim p As LevelProperties.ChessQueen.Lightning = MyBase.properties.CurrentState.lightning
		Me.isMoving = False
		MyBase.animator.SetBool("Fast", False)
		MyBase.animator.SetBool("OnLightning", True)
		Me.headWobbleDecay *= 2F
		Yield CupheadTime.WaitForSeconds(Me, p.lightningAnticipationTime)
		MyBase.animator.SetTrigger("OnAttack")
		While Me.activeLightning IsNot Nothing AndAlso Not Me.activeLightning.isGone
			Yield Nothing
		End While
		MyBase.animator.SetBool("OnLightning", False)
		MyBase.animator.Play("MoveEaseOutHold", 1, 0F)
		MyBase.animator.Update(0F)
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Lightning.Exit", False, True)
		MyBase.animator.Play("MoveSlow", 1, 0F)
		MyBase.animator.Update(0F)
		Me.isMoving = True
		Me.headWobbleDecay *= 0.5F
		Yield CupheadTime.WaitForSeconds(Me, Me.delayPattern.PopFloat())
		Me.state = ChessQueenLevelQueen.States.Idle
		Return
	End Function

	' Token: 0x0600192B RID: 6443 RVA: 0x000E3D54 File Offset: 0x000E2154
	Private Sub AniEvent_CreateLightning()
		Dim [next] As AbstractPlayerController = PlayerManager.GetNext()
		Me.lightningPositionPattern.IncrementString()
		Me.activeLightning = Me.lightningPrefab.Spawn()
		Me.activeLightning.Create(If((Me.lightningPositionPattern.GetString()(0) <> "P"c), Me.lightningPositionPattern.GetFloat(), [next].transform.position.x), MyBase.properties.CurrentState.lightning)
	End Sub

	' Token: 0x0600192C RID: 6444 RVA: 0x000E3DDA File Offset: 0x000E21DA
	Public Sub StartEgg()
		Me.state = ChessQueenLevelQueen.States.Egg
		MyBase.animator.SetBool("Egg", True)
		MyBase.StartCoroutine(Me.egg_cr())
	End Sub

	' Token: 0x0600192D RID: 6445 RVA: 0x000E3E04 File Offset: 0x000E2204
	Private Iterator Function egg_cr() As IEnumerator
		Dim p As LevelProperties.ChessQueen.Egg = MyBase.properties.CurrentState.egg
		Yield MyBase.animator.WaitForAnimationToStart(Me, "Egg.AttackLoop", False)
		Me.SFX_KOG_QUEEN_FabergeEggLoop()
		Me.SFX_KOG_QUEEN_FabergeEggTeethLoop()
		Dim rateTime As Single = 0F
		Dim attackTime As Single = 0F
		Dim attackDuration As Single = p.eggAttackDuration.RandomFloat()
		Me.eggRootLeft.SetPosition(Nothing, Nothing, New Single?(5E-07F))
		Me.eggRootRight.SetPosition(Nothing, Nothing, New Single?(0F))
		While attackTime < attackDuration
			attackTime += CupheadTime.Delta
			If rateTime > p.eggFireRate Then
				Me.fireProjectiles()
				rateTime = 0F
			Else
				rateTime += CupheadTime.Delta
			End If
			Yield Nothing
		End While
		Dim delay As Single = Me.delayPattern.PopFloat()
		If p.eggCooldownDuration + delay < Me.maxTimeToHoldForTwoEggAttacks AndAlso CType(Level.Current, ChessQueenLevel).NextPatternIsEgg() Then
			If p.eggCooldownDuration + delay > Me.maxTimeToStayOpenForTwoEggAttacks Then
				MyBase.animator.SetTrigger("ResetEgg")
				MyBase.animator.SetTrigger("EndAttack")
			End If
			Yield CupheadTime.WaitForSeconds(Me, p.eggCooldownDuration + delay)
			Me.StartEgg()
			Return
		End If
		MyBase.animator.SetTrigger("EndAttack")
		Me.SFX_KOG_QUEEN_FabergeEggLoopStopShort()
		Yield CupheadTime.WaitForSeconds(Me, p.eggCooldownDuration)
		MyBase.animator.SetBool("Egg", False)
		Me.SFX_KOG_QUEEN_FabergeEggClose()
		Me.SFX_KOG_QUEEN_FabergeEggLoopStopEnd()
		Me.SFX_KOG_QUEEN_FabergeEggTeethLoopStop()
		Yield MyBase.animator.WaitForAnimationToStart(Me, "Egg.End", False)
		Yield CupheadTime.WaitForSeconds(Me, delay)
		Me.state = ChessQueenLevelQueen.States.Idle
		Return
	End Function

	' Token: 0x0600192E RID: 6446 RVA: 0x000E3E20 File Offset: 0x000E2220
	Private Sub fireProjectiles()
		Dim egg As LevelProperties.ChessQueen.Egg = MyBase.properties.CurrentState.egg
		Dim zero As Vector2 = Vector2.zero
		Dim num As Single = CSng(If((Not Me.movingLeft), (-200), 200))
		zero.y = egg.eggVelocityY.RandomFloat()
		zero.x = egg.eggVelocityX.RandomFloat() + num
		Me.eggRootLeft.transform.position += Vector3.forward * 1E-06F
		Dim chessQueenLevelEgg As ChessQueenLevelEgg = Me.eggPrefab.Spawn()
		chessQueenLevelEgg.Create(Me.eggRootLeft.position, zero, egg.eggGravity, egg.eggSpawnCollisionTimer)
		zero.y = egg.eggVelocityY.RandomFloat()
		zero.x = egg.eggVelocityX.RandomFloat() + num
		Me.eggRootLeft.transform.position += Vector3.forward * 1E-06F
		Dim chessQueenLevelEgg2 As ChessQueenLevelEgg = Me.eggPrefab.Spawn()
		chessQueenLevelEgg2.Create(Me.eggRootRight.position, zero, egg.eggGravity, egg.eggSpawnCollisionTimer)
	End Sub

	' Token: 0x0600192F RID: 6447 RVA: 0x000E3F64 File Offset: 0x000E2364
	Private Sub die()
		If Me.dead Then
			Return
		End If
		Me.dead = True
		Me.mouse.Win()
		Me.headWobbleCurrentAmplitude = 0F
		Me.StopAllCoroutines()
		If MyBase.transform.position.x > 0F Then
			MyBase.transform.SetScale(New Single?(-1F), Nothing, Nothing)
		End If
		Dim component As LevelBossDeathExploder = MyBase.GetComponent(Of LevelBossDeathExploder)()
		component.offset.x = component.offset.x * MyBase.transform.localScale.x
		MyBase.animator.Play("Death")
		MyBase.StartCoroutine(Me.SFX_KOG_QUEEN_Death_cr())
	End Sub

	' Token: 0x06001930 RID: 6448 RVA: 0x000E402B File Offset: 0x000E242B
	Private Sub SFX_KOG_QUEEN_IntroTypeWriter()
		AudioManager.Play("sfx_dlc_kog_queen_introtypewriter")
	End Sub

	' Token: 0x06001931 RID: 6449 RVA: 0x000E4037 File Offset: 0x000E2437
	Private Sub AnimationEvent_SFX_KOG_QUEEN_IntroTableFlip()
		AudioManager.Play("sfx_dlc_kog_queen_introtableflip")
	End Sub

	' Token: 0x06001932 RID: 6450 RVA: 0x000E4043 File Offset: 0x000E2443
	Private Sub AnimationEvent_SFX_KOG_QUEEN_FabergeEggOpen()
		AudioManager.Play("sfx_dlc_kog_queen_fabergeegg_open")
		Me.emitAudioFromObject.Add("sfx_dlc_kog_queen_fabergeegg_open")
	End Sub

	' Token: 0x06001933 RID: 6451 RVA: 0x000E405F File Offset: 0x000E245F
	Private Sub SFX_KOG_QUEEN_FabergeEggClose()
		AudioManager.Play("sfx_dlc_kog_queen_fabergeegg_close")
		Me.emitAudioFromObject.Add("sfx_dlc_kog_queen_fabergeegg_close")
	End Sub

	' Token: 0x06001934 RID: 6452 RVA: 0x000E407B File Offset: 0x000E247B
	Private Sub SFX_KOG_QUEEN_FabergeEggTeethLoop()
		AudioManager.Play("sfx_dlc_kog_queen_fabergeeggteeth_loop")
		Me.emitAudioFromObject.Add("sfx_dlc_kog_queen_fabergeeggteeth_loop")
	End Sub

	' Token: 0x06001935 RID: 6453 RVA: 0x000E4097 File Offset: 0x000E2497
	Private Sub SFX_KOG_QUEEN_FabergeEggTeethLoopStop()
		AudioManager.[Stop]("sfx_dlc_kog_queen_fabergeeggteeth_loop")
	End Sub

	' Token: 0x06001936 RID: 6454 RVA: 0x000E40A3 File Offset: 0x000E24A3
	Private Sub SFX_KOG_QUEEN_FabergeEggLoop()
		AudioManager.PlayLoop("sfx_dlc_kog_queen_fabergeegg_loop")
		AudioManager.FadeSFXVolumeLinear("sfx_dlc_kog_queen_fabergeegg_loop", 0.7F, 2F)
		Me.emitAudioFromObject.Add("sfx_dlc_kog_queen_fabergeegg_loop")
	End Sub

	' Token: 0x06001937 RID: 6455 RVA: 0x000E40D3 File Offset: 0x000E24D3
	Private Sub SFX_KOG_QUEEN_FabergeEggLoopStopShort()
		AudioManager.[Stop]("sfx_dlc_kog_queen_fabergeegg_loop")
	End Sub

	' Token: 0x06001938 RID: 6456 RVA: 0x000E40DF File Offset: 0x000E24DF
	Private Sub SFX_KOG_QUEEN_FabergeEggLoopStopEnd()
		AudioManager.FadeSFXVolumeLinear("sfx_dlc_kog_queen_fabergeegg_loop", 0F, 1F)
	End Sub

	' Token: 0x06001939 RID: 6457 RVA: 0x000E40F5 File Offset: 0x000E24F5
	Private Sub AnimationEvent_SFX_KOG_QUEEN_SpawnChessPieces()
		AudioManager.Play("sfx_dlc_kog_queen_spawnchesspieces")
		Me.emitAudioFromObject.Add("sfx_dlc_kog_queen_spawnchesspieces")
	End Sub

	' Token: 0x0600193A RID: 6458 RVA: 0x000E4114 File Offset: 0x000E2514
	Private Iterator Function SFX_KOG_QUEEN_Death_cr() As IEnumerator
		Me.SFX_KOG_QUEEN_FabergeEggLoopStopShort()
		Yield CupheadTime.WaitForSeconds(Me, 0.1F)
		AudioManager.Play("sfx_dlc_kog_queen_death")
		Yield CupheadTime.WaitForSeconds(Me, 0.5F)
		AudioManager.Play("sfx_dlc_kog_queen_vocal_death")
		Yield CupheadTime.WaitForSeconds(Me, 0.7F)
		AudioManager.PlayLoop("sfx_dlc_kog_queen_deathcrownspin_loop")
		Return
	End Function

	' Token: 0x0600193B RID: 6459 RVA: 0x000E4130 File Offset: 0x000E2530
	Private Iterator Function SFX_KOG_QUEEN_VocalAttack_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 0F)
		AudioManager.Play("sfx_dlc_kog_queen_vocal_attack")
		Me.emitAudioFromObject.Add("sfx_dlc_kog_queen_vocal_attack")
		Return
	End Function

	' Token: 0x0600193C RID: 6460 RVA: 0x000E414B File Offset: 0x000E254B
	Private Sub AnimationEvent_SFX_KOG_QUEEN_VocalHurt()
		AudioManager.Play("sfx_dlc_kog_queen_vocal_hurt")
		Me.emitAudioFromObject.Add("sfx_dlc_kog_queen_vocal_hurt")
	End Sub

	' Token: 0x0600193D RID: 6461 RVA: 0x000E4167 File Offset: 0x000E2567
	Private Sub AnimationEvent_SFX_KOG_QUEEN_VocalLaughLrg()
		AudioManager.Play("sfx_dlc_kog_queen_vocal_laughlrg")
		Me.emitAudioFromObject.Add("sfx_dlc_kog_queen_vocal_laughlrg")
	End Sub

	' Token: 0x0600193E RID: 6462 RVA: 0x000E4183 File Offset: 0x000E2583
	Private Sub AnimationEvent_SFX_KOG_QUEEN_VocalLaughSml()
		AudioManager.Play("sfx_dlc_kog_queen_vocal_laughSml")
		Me.emitAudioFromObject.Add("sfx_dlc_kog_queen_vocal_laughSml")
	End Sub

	' Token: 0x0600193F RID: 6463 RVA: 0x000E419F File Offset: 0x000E259F
	Private Sub SFX_KOG_QUEEN_CannonHitQueenDing()
		AudioManager.Play("sfx_dlc_kog_queen_cannonhitqueending")
		Me.emitAudioFromObject.Add("sfx_dlc_kog_queen_cannonhitqueending")
	End Sub

	' Token: 0x04002235 RID: 8757
	<SerializeField()>
	Private dressRenderers As SpriteRenderer()

	' Token: 0x04002236 RID: 8758
	<SerializeField()>
	Private easeCoefficient As Single

	' Token: 0x04002237 RID: 8759
	<SerializeField()>
	Private accel As Single = 2F

	' Token: 0x04002238 RID: 8760
	<SerializeField()>
	Private attackDecel As Single = 2F

	' Token: 0x04002239 RID: 8761
	<SerializeField()>
	Private useSineEasing As Boolean

	' Token: 0x0400223A RID: 8762
	<SerializeField()>
	Private minMoveDistanceAfterLightning As Single = 100F

	' Token: 0x0400223B RID: 8763
	Private flipPositionString As Boolean

	' Token: 0x0400223C RID: 8764
	<SerializeField()>
	Private hitFlash As HitFlash

	' Token: 0x0400223D RID: 8765
	<SerializeField()>
	Private cannonLeft As ChessQueenLevelCannon

	' Token: 0x0400223E RID: 8766
	<SerializeField()>
	Private cannonMiddle As ChessQueenLevelCannon

	' Token: 0x0400223F RID: 8767
	<SerializeField()>
	Private cannonRight As ChessQueenLevelCannon

	' Token: 0x04002240 RID: 8768
	<SerializeField()>
	Private head As Transform

	' Token: 0x04002241 RID: 8769
	<SerializeField()>
	Private mouse As ChessQueenLevelLooseMouse

	' Token: 0x04002242 RID: 8770
	Private headWobbleCurrentAmplitude As Single

	' Token: 0x04002243 RID: 8771
	Private headWobbleTimer As Single

	' Token: 0x04002244 RID: 8772
	<SerializeField()>
	Private headWobbleSpeed As Single = 50F

	' Token: 0x04002245 RID: 8773
	<SerializeField()>
	Private headWobbleAmplitude As Single = 25F

	' Token: 0x04002246 RID: 8774
	<SerializeField()>
	Private headWobbleDecay As Single = 50F

	' Token: 0x04002247 RID: 8775
	<Header("Egg")>
	<SerializeField()>
	Private eggPrefab As ChessQueenLevelEgg

	' Token: 0x04002248 RID: 8776
	<SerializeField()>
	Private eggRootRight As Transform

	' Token: 0x04002249 RID: 8777
	<SerializeField()>
	Private eggRootLeft As Transform

	' Token: 0x0400224A RID: 8778
	<SerializeField()>
	Private maxTimeToHoldForTwoEggAttacks As Single

	' Token: 0x0400224B RID: 8779
	<SerializeField()>
	Private maxTimeToStayOpenForTwoEggAttacks As Single = 0.7F

	' Token: 0x0400224C RID: 8780
	<Header("Lightning")>
	<SerializeField()>
	Private lightningPrefab As ChessQueenLevelLightning

	' Token: 0x0400224D RID: 8781
	<SerializeField()>
	Public lightningDisableRange As Single = 150F

	' Token: 0x0400224E RID: 8782
	Private lastXPos As Single

	' Token: 0x0400224F RID: 8783
	Private cannonCycleDirection As Integer

	' Token: 0x04002252 RID: 8786
	Private cannons As List(Of ChessQueenLevelCannon)

	' Token: 0x04002253 RID: 8787
	Private activeCannonIndex As Integer

	' Token: 0x04002254 RID: 8788
	Private delayPattern As PatternString

	' Token: 0x04002255 RID: 8789
	Private lightningPositionPattern As PatternString

	' Token: 0x04002256 RID: 8790
	Private positionPattern As PatternString

	' Token: 0x04002257 RID: 8791
	Private movingLeft As Boolean

	' Token: 0x04002258 RID: 8792
	Private isMoving As Boolean

	' Token: 0x04002259 RID: 8793
	Private dead As Boolean

	' Token: 0x0400225A RID: 8794
	Public speedThresholdForFastAnimation As Single

	' Token: 0x0200054F RID: 1359
	Public Enum States
		' Token: 0x0400225C RID: 8796
		Idle
		' Token: 0x0400225D RID: 8797
		Lightning
		' Token: 0x0400225E RID: 8798
		Egg
	End Enum
End Class
