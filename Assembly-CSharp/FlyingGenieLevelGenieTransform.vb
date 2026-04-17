Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x0200066B RID: 1643
Public Class FlyingGenieLevelGenieTransform
	Inherits LevelProperties.FlyingGenie.Entity

	' Token: 0x17000394 RID: 916
	' (get) Token: 0x0600226A RID: 8810 RVA: 0x001422AB File Offset: 0x001406AB
	' (set) Token: 0x0600226B RID: 8811 RVA: 0x001422B3 File Offset: 0x001406B3
	Public Property state As FlyingGenieLevelGenieTransform.State

	' Token: 0x17000395 RID: 917
	' (get) Token: 0x0600226C RID: 8812 RVA: 0x001422BC File Offset: 0x001406BC
	' (set) Token: 0x0600226D RID: 8813 RVA: 0x001422C4 File Offset: 0x001406C4
	Public Property skipMarionette As Boolean

	' Token: 0x0600226E RID: 8814 RVA: 0x001422D0 File Offset: 0x001406D0
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.skipMarionette = False
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		MyBase.GetComponent(Of Collider2D)().enabled = False
	End Sub

	' Token: 0x0600226F RID: 8815 RVA: 0x00142324 File Offset: 0x00140724
	Public Overrides Sub LevelInit(properties As LevelProperties.FlyingGenie)
		MyBase.LevelInit(properties)
		Me.state = FlyingGenieLevelGenieTransform.State.Intro
		Me.pyramids = New List(Of FlyingGenieLevelPyramid)()
	End Sub

	' Token: 0x06002270 RID: 8816 RVA: 0x00142340 File Offset: 0x00140740
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		MyBase.properties.DealDamage(info.damage)
		If Me.skipMarionette AndAlso Me.state = FlyingGenieLevelGenieTransform.State.Giant AndAlso Me.transitionHP > 0F Then
			Dim num As Single = Me.transitionHP
			Me.transitionHP -= info.damage
			Level.Current.timeline.DealDamage(Mathf.Clamp(num - Me.transitionHP, 0F, num))
		ElseIf MyBase.properties.CurrentHealth <= 0F AndAlso Me.state <> FlyingGenieLevelGenieTransform.State.Dead Then
			Me.state = FlyingGenieLevelGenieTransform.State.Dead
			If Level.Current.mode = Level.Mode.Easy Then
				Me.MarionetteDead()
			Else
				Me.StartDeath()
			End If
		End If
	End Sub

	' Token: 0x06002271 RID: 8817 RVA: 0x0014240E File Offset: 0x0014080E
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06002272 RID: 8818 RVA: 0x0014242C File Offset: 0x0014082C
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06002273 RID: 8819 RVA: 0x00142444 File Offset: 0x00140844
	Public Sub StartMarionette(spawnPos As Vector3, meditateP1 As FlyingGenieLevelMeditateFX, meditateP2 As FlyingGenieLevelMeditateFX)
		MyBase.GetComponent(Of Collider2D)().enabled = True
		MyBase.transform.position = spawnPos
		Me.meditateP1 = meditateP1
		Me.meditateP2 = meditateP2
		MyBase.StartCoroutine(Me.phase2_intro_cr())
	End Sub

	' Token: 0x06002274 RID: 8820 RVA: 0x0014247C File Offset: 0x0014087C
	Private Iterator Function phase2_intro_cr() As IEnumerator
		AudioManager.Play("genie_return")
		Me.emitAudioFromObject.Add("genie_return")
		Dim p As LevelProperties.FlyingGenie.Scan = MyBase.properties.CurrentState.scan
		Dim timer As Single = 0F
		Dim P1ShrinkTimer As Single = 0F
		Dim P2ShrinkTimer As Single = 0F
		Dim player As PlanePlayerController = TryCast(PlayerManager.GetPlayer(PlayerId.PlayerOne), PlanePlayerController)
		Dim player2 As PlanePlayerController = TryCast(PlayerManager.GetPlayer(PlayerId.PlayerTwo), PlanePlayerController)
		Dim player2In As Boolean = player2 IsNot Nothing
		While timer < p.scanDuration
			timer += CupheadTime.Delta
			If Level.Current.mode <> Level.Mode.Easy Then
				If player.Shrunk Then
					P1ShrinkTimer += CupheadTime.Delta
				End If
				If player2In AndAlso player2.Shrunk Then
					P2ShrinkTimer += CupheadTime.Delta
				End If
			End If
			Yield Nothing
		End While
		If P1ShrinkTimer >= p.miniDuration Then
			If player2In Then
				If P2ShrinkTimer >= p.miniDuration Then
					Me.skipMarionette = True
				Else
					Me.skipMarionette = False
				End If
			Else
				Me.skipMarionette = True
			End If
		End If
		MyBase.animator.SetTrigger("Continue")
		Me.pyramidsGoingClockwise = Rand.Bool()
		If Me.skipMarionette Then
			Me.transitionHP = p.transitionDamage
			MyBase.animator.SetBool("IsPuppet", True)
			Me.state = FlyingGenieLevelGenieTransform.State.Giant
			MyBase.StartCoroutine(Me.move_up_puppet_cr())
			MyBase.properties.DealDamageToNextNamedState()
		Else
			MyBase.animator.SetBool("IsPuppet", False)
			Me.state = FlyingGenieLevelGenieTransform.State.Marionette
			Yield MyBase.animator.WaitForAnimationToEnd(Me, "Marionette_Intro", False, True)
			Me.startPos = MyBase.transform.position
			MyBase.StartCoroutine(Me.move_cr())
			MyBase.StartCoroutine(Me.shoot_cr())
		End If
		Yield Nothing
		Return
	End Function

	' Token: 0x06002275 RID: 8821 RVA: 0x00142497 File Offset: 0x00140897
	Private Sub EndFX()
		If Me.meditateP1 IsNot Nothing Then
			Me.meditateP1.EndEffect()
		End If
		If Me.meditateP2 IsNot Nothing Then
			Me.meditateP2.EndEffect()
		End If
	End Sub

	' Token: 0x06002276 RID: 8822 RVA: 0x001424D4 File Offset: 0x001408D4
	Private Sub SnapPosition()
		Me.HandSFX()
		MyBase.transform.position = Me.morphRoot.position
		Me.bottomLayer.transform.localPosition = New Vector3(-160F, Me.bottomLayer.transform.localPosition.y)
		MyBase.StartCoroutine(Me.handle_carpet_fadeout_cr())
	End Sub

	' Token: 0x06002277 RID: 8823 RVA: 0x0014253C File Offset: 0x0014093C
	Private Iterator Function move_up_puppet_cr() As IEnumerator
		Dim t As Single = 0F
		Dim timer As Single = 0.4F
		Dim slowTimer As Single = 0.8F
		Dim midTimer As Single = 0.5F
		Dim midEnd As Single = 300F
		Dim slowEnd As Single = 410F
		Dim [end] As Single = 1071F
		Dim start As Single = MyBase.transform.position.y
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Genie_Morph_Puppet", False, True)
		Me.tinyMarionette.gameObject.SetActive(True)
		While t < slowTimer
			Dim val As Single = EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, 0F, 1F, t / slowTimer)
			MyBase.transform.SetPosition(Nothing, New Single?(Mathf.Lerp(start, slowEnd, val)), Nothing)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		t = 0F
		start = MyBase.transform.position.y
		Dim startIntro As Boolean = False
		While t < midTimer
			Dim val2 As Single = EaseUtils.Ease(EaseUtils.EaseType.easeInSine, 0F, 1F, t / midTimer)
			MyBase.transform.SetPosition(Nothing, New Single?(Mathf.Lerp(start, midEnd, val2)), Nothing)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		t = 0F
		start = MyBase.transform.position.y
		While t < timer
			Dim val3 As Single = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0F, 1F, t / timer)
			MyBase.transform.SetPosition(Nothing, New Single?(Mathf.Lerp(start, [end], val3)), Nothing)
			If t / timer > 0.95F AndAlso Not startIntro Then
				Me.tinyMarionette.animator.SetTrigger("OnIntro")
				startIntro = True
			End If
			t += CupheadTime.Delta
			Yield Nothing
		End While
		Me.tinyMarionette.transform.parent = Nothing
		MyBase.properties.DealDamage(MyBase.properties.CurrentState.scan.transitionDamage)
		Dim endPos As Vector3 = New Vector3(Me.pyramidPivotPoint.position.x, Me.pyramidPivotPoint.position.y + 145F)
		Me.tinyMarionette.Activate(endPos, MyBase.properties.CurrentState.scan, Not Me.pyramidsGoingClockwise)
		Me.EndMarionette()
		Yield Nothing
		Return
	End Function

	' Token: 0x06002278 RID: 8824 RVA: 0x00142558 File Offset: 0x00140958
	Private Iterator Function handle_carpet_fadeout_cr() As IEnumerator
		Me.bottomLayer.color = New Color(1F, 1F, 1F, 1F)
		Dim t As Single = 0F
		Dim time As Single = 2F
		While t < time
			Me.bottomLayer.color = New Color(1F, 1F, 1F, 1F - t / time)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		MyBase.animator.Play("Hands_Off")
		Me.bottomLayer.color = New Color(1F, 1F, 1F, 1F)
		Me.bottomLayer.transform.localPosition = Vector3.zero
		Yield Nothing
		Return
	End Function

	' Token: 0x06002279 RID: 8825 RVA: 0x00142574 File Offset: 0x00140974
	Private Sub SpawnTurban()
		If Not Me.skipMarionette Then
			Me.spawner = Me.spawnerPrefab.Create(New Vector3(MyBase.transform.position.x, CSng(Level.Current.Height) + 100F), PlayerManager.GetNext(), MyBase.properties.CurrentState.bullets)
			Me.spawner.isDead = False
		End If
	End Sub

	' Token: 0x0600227A RID: 8826 RVA: 0x001425EC File Offset: 0x001409EC
	Private Iterator Function shoot_cr() As IEnumerator
		Dim p As LevelProperties.FlyingGenie.Bullets = MyBase.properties.CurrentState.bullets
		Dim mainShotIndex As Integer = Global.UnityEngine.Random.Range(0, p.shotCount.Length)
		Dim shotCount As String() = p.shotCount(mainShotIndex).Split(New Char() { ","c })
		Dim shotIndex As Integer = 0
		Dim pinkCount As String() = p.pinkString.Split(New Char() { ","c })
		Dim pinkIndex As Integer = 0
		While Me.state = FlyingGenieLevelGenieTransform.State.Marionette
			Me.isShooting = False
			shotCount = p.shotCount(mainShotIndex).Split(New Char() { ","c })
			Yield CupheadTime.WaitForSeconds(Me, p.hesitateRange.RandomFloat())
			Me.isShooting = True
			MyBase.animator.SetBool("IsAttacking", True)
			Yield MyBase.animator.WaitForAnimationToEnd(Me, "Marionette_Attack_Start", False, True)
			AudioManager.Play("genie_voice_laugh_reverb")
			Dim player As AbstractPlayerController = PlayerManager.GetNext()
			MyBase.animator.Play("Marionette_Spark")
			For i As Integer = 0 To shotCount.Length - 1
				For j As Integer = 0 To Parser.IntParse(shotCount(shotIndex)) - 1
					If player Is Nothing OrElse player.IsDead Then
						player = PlayerManager.GetNext()
					End If
					Dim dir As Vector3 = player.transform.position - Me.marionetteShootRoot.transform.position
					If dir.x > 0F Then
						dir.x = 0F
					End If
					If pinkCount(pinkIndex)(0) = "P"c Then
						Me.pinkBullet.Create(Me.marionetteShootRoot.transform.position, MathUtils.DirectionToAngle(dir), p.shotSpeed)
						AudioManager.Play("genie_puppet_shoot")
						Me.emitAudioFromObject.Add("genie_puppet_shoot")
					ElseIf pinkCount(pinkIndex)(0) = "R"c Then
						Me.shotBullet.Create(Me.marionetteShootRoot.transform.position, MathUtils.DirectionToAngle(dir), p.shotSpeed)
						AudioManager.Play("genie_puppet_shoot")
						Me.emitAudioFromObject.Add("genie_puppet_shoot")
					End If
					Yield Me.WaitWhileShooting(p.shotDelay, p.shotSpeed)
					pinkIndex = (pinkIndex + 1) Mod pinkCount.Length
				Next
				If player Is Nothing OrElse player.IsDead Then
					player = PlayerManager.GetNext()
				End If
				Yield Me.WaitWhileShooting(p.shotDelay, p.shotSpeed)
				If shotIndex < shotCount.Length - 1 Then
					shotIndex += 1
				Else
					mainShotIndex = (mainShotIndex + 1) Mod p.shotCount.Length
					shotIndex = 0
				End If
				Yield Nothing
			Next
			Yield Nothing
			MyBase.animator.SetBool("IsAttacking", False)
		End While
		Yield Nothing
		Return
	End Function

	' Token: 0x0600227B RID: 8827 RVA: 0x00142608 File Offset: 0x00140A08
	Private Iterator Function WaitWhileShooting(time As Single, shootSpeed As Single) As IEnumerator
		Dim pointingUp As Boolean = False
		Dim timeEsalpsed As Single = 0F
		Dim timeSinceSubShot As Single = 0F
		While timeEsalpsed <= time
			If timeSinceSubShot >= 0.12F Then
				Me.shootBullet.Create(Me.marionetteShootRoot.transform.position, CSng(If((Not pointingUp), (-100), 100)), shootSpeed)
				pointingUp = Not pointingUp
				timeSinceSubShot = 0F
			End If
			timeEsalpsed += CupheadTime.Delta
			timeSinceSubShot += CupheadTime.Delta
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600227C RID: 8828 RVA: 0x00142634 File Offset: 0x00140A34
	Private Iterator Function move_cr() As IEnumerator
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		While Me.state = FlyingGenieLevelGenieTransform.State.Marionette
			If Not Me.isShooting Then
				If MyBase.transform.position.x > -Me.startPos.x Then
					MyBase.transform.AddPosition(-MyBase.properties.CurrentState.bullets.marionetteMoveSpeed * CupheadTime.FixedDelta, 0F, 0F)
				End If
			ElseIf MyBase.transform.position.x < Me.startPos.x Then
				MyBase.transform.AddPosition(MyBase.properties.CurrentState.bullets.marionetteReturnSpeed * CupheadTime.FixedDelta, 0F, 0F)
			End If
			Yield wait
		End While
		Return
	End Function

	' Token: 0x0600227D RID: 8829 RVA: 0x00142650 File Offset: 0x00140A50
	Public Sub EndMarionette()
		If Not Me.skipMarionette Then
			AudioManager.Play("genie_puppet_exit")
			Me.emitAudioFromObject.Add("genie_puppet_exit")
		End If
		If Me.spawner IsNot Nothing Then
			Me.spawner.isDead = True
		End If
		Me.spark.SetActive(False)
		Me.StopAllCoroutines()
		Me.state = FlyingGenieLevelGenieTransform.State.Giant
		MyBase.StartCoroutine(Me.genie_intro_cr())
	End Sub

	' Token: 0x0600227E RID: 8830 RVA: 0x001426C5 File Offset: 0x00140AC5
	Private Sub MarionetteDead()
		MyBase.GetComponent(Of Collider2D)().enabled = False
		MyBase.animator.SetTrigger("MarionetteDeath")
	End Sub

	' Token: 0x0600227F RID: 8831 RVA: 0x001426E4 File Offset: 0x00140AE4
	Private Iterator Function genie_intro_cr() As IEnumerator
		Dim pullSpeed As Single = 700F
		Dim size As Single = MyBase.GetComponent(Of SpriteRenderer)().bounds.size.x
		Dim angle As Single = 120F
		Dim number As Integer = 1
		If Not Me.skipMarionette Then
			MyBase.animator.SetTrigger("MarionetteDeath")
			MyBase.GetComponent(Of LevelBossDeathExploder)().StartExplosion()
		End If
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		While MyBase.transform.position.y < 960F
			MyBase.transform.AddPosition(0F, pullSpeed * CupheadTime.Delta, 0F)
			Yield Nothing
		End While
		If Not Me.skipMarionette Then
			MyBase.GetComponent(Of LevelBossDeathExploder)().StopExplosions()
		End If
		Yield CupheadTime.WaitForSeconds(Me, 0.7F)
		MyBase.animator.Play("Giant_Intro")
		MyBase.transform.position = New Vector3(640F + size / 3F, 0F)
		Dim startPos As Vector3 = MyBase.transform.position
		Dim t As Single = 0F
		Dim time As Single = 1F
		While t < time
			Dim val As Single = EaseUtils.Ease(EaseUtils.EaseType.linear, 0F, 1F, t / time)
			MyBase.transform.position = Vector2.Lerp(startPos, Me.giantRoot.position, val)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		MyBase.transform.position = Me.giantRoot.position
		For i As Integer = 0 To 3 - 1
			Me.SpawnPyramids(angle * 0.017453292F * CSng(i), number)
			number += 1
		Next
		MyBase.StartCoroutine(Me.attack_cr())
		Yield Nothing
		Return
	End Function

	' Token: 0x06002280 RID: 8832 RVA: 0x001426FF File Offset: 0x00140AFF
	Private Sub IntroHands()
		MyBase.StartCoroutine(Me.intro_hands_cr())
	End Sub

	' Token: 0x06002281 RID: 8833 RVA: 0x00142710 File Offset: 0x00140B10
	Private Iterator Function intro_hands_cr() As IEnumerator
		Dim [end] As Vector3 = Me.handFront.transform.position
		Dim start As Vector3 = Me.handFront.transform.position
		start.y = Me.handFront.transform.position.y - 500F
		Me.handFront.transform.position = start
		Me.handBack.transform.position = start
		MyBase.animator.Play("Giant_Hands")
		Dim t As Single = 0F
		Dim time As Single = 1.25F
		While t < time
			Dim val As Single = EaseUtils.Ease(EaseUtils.EaseType.linear, 0F, 1F, t / time)
			Me.handFront.transform.position = Vector2.Lerp(start, [end], val)
			Me.handBack.transform.position = Vector2.Lerp(start, [end], val)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		Yield CupheadTime.WaitForSeconds(Me, 0.8F)
		t = 0F
		While t < time
			Dim val2 As Single = EaseUtils.Ease(EaseUtils.EaseType.linear, 0F, 1F, t / time)
			Me.handFront.transform.position = Vector2.Lerp([end], start, val2)
			Me.handBack.transform.position = Vector2.Lerp([end], start, val2)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		MyBase.animator.Play("Hands_Off")
		MyBase.StartCoroutine(Me.gem_stone_cr())
		Yield Nothing
		Return
	End Function

	' Token: 0x06002282 RID: 8834 RVA: 0x0014272C File Offset: 0x00140B2C
	Private Sub SpawnPyramids(startingAngle As Single, number As Integer)
		Dim pyramids As LevelProperties.FlyingGenie.Pyramids = MyBase.properties.CurrentState.pyramids
		Dim flyingGenieLevelPyramid As FlyingGenieLevelPyramid = Global.UnityEngine.[Object].Instantiate(Of FlyingGenieLevelPyramid)(Me.pyramidPrefab)
		flyingGenieLevelPyramid.Init(pyramids, MyBase.transform.position, startingAngle, pyramids.speedRotation, Me.pyramidPivotPoint, number, Me.pyramidsGoingClockwise)
		flyingGenieLevelPyramid.GetComponent(Of Collider2D)().enabled = False
		Me.pyramids.Add(flyingGenieLevelPyramid)
	End Sub

	' Token: 0x06002283 RID: 8835 RVA: 0x0014279C File Offset: 0x00140B9C
	Private Iterator Function attack_cr() As IEnumerator
		Dim p As LevelProperties.FlyingGenie.Pyramids = MyBase.properties.CurrentState.pyramids
		Dim delayString As String() = p.attackDelayString.GetRandom().Split(New Char() { ","c })
		Dim attackString As String() = p.pyramidAttackString.GetRandom().Split(New Char() { ","c })
		Dim delayIndex As Integer = Global.UnityEngine.Random.Range(0, delayString.Length)
		Dim attackIndex As Integer = Global.UnityEngine.Random.Range(0, attackString.Length)
		Dim delay As Single = 0F
		Dim numberReceived As Integer = 0
		Dim t As Single = 0F
		Dim time As Single = 2.5F
		For Each flyingGenieLevelPyramid As FlyingGenieLevelPyramid In Me.pyramids
			flyingGenieLevelPyramid.GetComponent(Of SpriteRenderer)().color = New Color(1F, 1F, 1F, 0F)
		Next
		While t < time
			t += CupheadTime.Delta
			For Each flyingGenieLevelPyramid2 As FlyingGenieLevelPyramid In Me.pyramids
				flyingGenieLevelPyramid2.GetComponent(Of SpriteRenderer)().color = New Color(1F, 1F, 1F, t / time)
			Next
			Yield Nothing
		End While
		For Each flyingGenieLevelPyramid3 As FlyingGenieLevelPyramid In Me.pyramids
			flyingGenieLevelPyramid3.GetComponent(Of SpriteRenderer)().color = New Color(1F, 1F, 1F, 1F)
			flyingGenieLevelPyramid3.GetComponent(Of Collider2D)().enabled = True
		Next
		While True
			For i As Integer = attackIndex To attackString.Length - 1
				Parser.FloatTryParse(delayString(delayIndex), delay)
				Yield CupheadTime.WaitForSeconds(Me, delay)
				Dim attackOrder As String() = attackString(i).Split(New Char() { "-"c })
				For Each text As String In attackOrder
					Parser.IntTryParse(text, numberReceived)
					For l As Integer = 0 To Me.pyramids.Count - 1
						If Me.pyramids(l).number = numberReceived Then
							MyBase.StartCoroutine(Me.pyramids(l).beam_cr())
						End If
					Next
				Next
				For j As Integer = 0 To Me.pyramids.Count - 1
					If Me.pyramids(j).number = numberReceived Then
						While Not Me.pyramids(j).finishedATK
							Yield Nothing
						End While
					End If
				Next
				attackIndex = 0
				i = i Mod attackString.Length
				delayIndex = (delayIndex + 1) Mod delayString.Length
			Next
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06002284 RID: 8836 RVA: 0x001427B8 File Offset: 0x00140BB8
	Private Iterator Function gem_stone_cr() As IEnumerator
		Dim p As LevelProperties.FlyingGenie.GemStone = MyBase.properties.CurrentState.gemStone
		Dim attackDelayPattern As String() = p.attackDelayString.GetRandom().Split(New Char() { ","c })
		Dim delayIndex As Integer = Global.UnityEngine.Random.Range(0, attackDelayPattern.Length)
		Me.pinkString = p.pinkString.Split(New Char() { ","c })
		Me.pinkIndex = Global.UnityEngine.Random.Range(0, Me.pinkString.Length)
		Dim delay As Single = 0F
		While True
			Yield CupheadTime.WaitForSeconds(Me, p.warningDuration)
			Parser.FloatTryParse(attackDelayPattern(delayIndex), delay)
			MyBase.animator.SetTrigger("OnGiantAttack")
			Yield MyBase.animator.WaitForAnimationToEnd(Me, "Giant_Attack", False, True)
			Yield CupheadTime.WaitForSeconds(Me, delay)
			delayIndex = (delayIndex + 1) Mod attackDelayPattern.Length
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06002285 RID: 8837 RVA: 0x001427D4 File Offset: 0x00140BD4
	Private Sub OnRing()
		Dim gemStone As LevelProperties.FlyingGenie.GemStone = MyBase.properties.CurrentState.gemStone
		Me.gemStone.LookAt2D(PlayerManager.GetNext().center)
		Dim flag As Boolean
		Dim flyingGenieLevelRing As FlyingGenieLevelRing
		If Me.pinkString(Me.pinkIndex)(0) = "P"c Then
			flag = True
			flyingGenieLevelRing = TryCast(Me.pinkRingPrefab.Create(Me.gemStone.position, Me.gemStone.eulerAngles.z, gemStone.bulletSpeed), FlyingGenieLevelRing)
		Else
			flag = False
			flyingGenieLevelRing = TryCast(Me.ringPrefab.Create(Me.gemStone.position, Me.gemStone.eulerAngles.z, gemStone.bulletSpeed), FlyingGenieLevelRing)
		End If
		MyBase.StartCoroutine(Me.ring_cr(flyingGenieLevelRing, flag))
		Me.pinkIndex = (Me.pinkIndex + 1) Mod Me.pinkString.Length
	End Sub

	' Token: 0x06002286 RID: 8838 RVA: 0x001428CC File Offset: 0x00140CCC
	Private Iterator Function ring_cr(ring As FlyingGenieLevelRing, isPink As Boolean) As IEnumerator
		ring.isMain = True
		Dim frameCount As Integer = 0
		Dim frameTime As Single = 0F
		Dim trailRing As FlyingGenieLevelRing = If((Not isPink), Me.ringPrefab, Me.pinkRingPrefab)
		Dim lastRing As FlyingGenieLevelRing = Nothing
		While ring IsNot Nothing
			frameTime += CupheadTime.Delta
			If frameTime > 0.041666668F Then
				If frameCount < 3 Then
					frameCount += 1
				Else
					frameCount = 0
					If lastRing IsNot Nothing Then
						lastRing.DisableCollision()
					End If
					lastRing = TryCast(trailRing.Create(ring.transform.position, Me.gemStone.eulerAngles.z, 0.1F), FlyingGenieLevelRing)
				End If
				frameTime -= 0.041666668F
				Yield Nothing
			End If
			Yield Nothing
		End While
		Yield Nothing
		Return
	End Function

	' Token: 0x06002287 RID: 8839 RVA: 0x001428F5 File Offset: 0x00140CF5
	Private Sub StartDeath()
		If Me.skipMarionette AndAlso Me.tinyMarionette IsNot Nothing Then
			Me.tinyMarionette.Die()
		End If
		MyBase.animator.SetTrigger("Death")
	End Sub

	' Token: 0x06002288 RID: 8840 RVA: 0x0014292E File Offset: 0x00140D2E
	Private Sub SpawnPuff()
		Me.deathPuffEffect.Create(Me.deathPuffRoot.transform.position)
	End Sub

	' Token: 0x06002289 RID: 8841 RVA: 0x0014294C File Offset: 0x00140D4C
	Private Sub HandSFX()
		AudioManager.Play("genie_puppet_hand_enter")
		Me.emitAudioFromObject.Add("genie_puppet_hand_enter")
	End Sub

	' Token: 0x0600228A RID: 8842 RVA: 0x00142968 File Offset: 0x00140D68
	Private Sub SoundGenieVoiceMorph()
		AudioManager.Play("genie_voice_excited")
		Me.emitAudioFromObject.Add("genie_voice_excited")
	End Sub

	' Token: 0x0600228B RID: 8843 RVA: 0x00142984 File Offset: 0x00140D84
	Private Sub SoundPuppetRun()
		AudioManager.Play("genie_puppet_run")
		Me.emitAudioFromObject.Add("genie_puppet_run")
	End Sub

	' Token: 0x0600228C RID: 8844 RVA: 0x001429A0 File Offset: 0x00140DA0
	Private Sub SoundGenieVoicePhase3Intro()
		AudioManager.Play("genie_voice_phase3_intro")
		Me.emitAudioFromObject.Add("genie_voice_phase3_intro")
	End Sub

	' Token: 0x0600228D RID: 8845 RVA: 0x001429BC File Offset: 0x00140DBC
	Private Sub SoundGenieMindShoot()
		AudioManager.Play("genie_phase3_mind_shoot")
		Me.emitAudioFromObject.Add("genie_phase3_mind_shoot")
	End Sub

	' Token: 0x0600228E RID: 8846 RVA: 0x001429D8 File Offset: 0x00140DD8
	Private Sub SoundPuppetSmallEnterMobile()
		AudioManager.Play("genie_puppetsmall_enter_mobile")
		Me.emitAudioFromObject.Add("genie_puppetsmall_enter_mobile")
	End Sub

	' Token: 0x04002B14 RID: 11028
	Private Const FRAME_TIME As Single = 0.041666668F

	' Token: 0x04002B16 RID: 11030
	<SerializeField()>
	Private deathPuffEffect As Effect

	' Token: 0x04002B17 RID: 11031
	<SerializeField()>
	Private bottomLayer As SpriteRenderer

	' Token: 0x04002B18 RID: 11032
	<Space(10F)>
	<SerializeField()>
	Private spawnerPrefab As FlyingGenieLevelSpawner

	' Token: 0x04002B19 RID: 11033
	<SerializeField()>
	Private marionetteShootRoot As Transform

	' Token: 0x04002B1A RID: 11034
	<SerializeField()>
	Private shotBullet As BasicProjectile

	' Token: 0x04002B1B RID: 11035
	<SerializeField()>
	Private pinkBullet As BasicProjectile

	' Token: 0x04002B1C RID: 11036
	<SerializeField()>
	Private shootBullet As BasicProjectile

	' Token: 0x04002B1D RID: 11037
	<SerializeField()>
	Private spreadProjectile As BasicProjectile

	' Token: 0x04002B1E RID: 11038
	<SerializeField()>
	Private ringPrefab As FlyingGenieLevelRing

	' Token: 0x04002B1F RID: 11039
	<SerializeField()>
	Private pinkRingPrefab As FlyingGenieLevelRing

	' Token: 0x04002B20 RID: 11040
	<SerializeField()>
	Private pyramidPrefab As FlyingGenieLevelPyramid

	' Token: 0x04002B21 RID: 11041
	<SerializeField()>
	Private tinyMarionette As FlyingGenieLevelTinyMarionette

	' Token: 0x04002B22 RID: 11042
	<Space(10F)>
	<SerializeField()>
	Private pyramidPivotPoint As Transform

	' Token: 0x04002B23 RID: 11043
	<SerializeField()>
	Private gemStone As Transform

	' Token: 0x04002B24 RID: 11044
	<SerializeField()>
	Private pipe As Transform

	' Token: 0x04002B25 RID: 11045
	<SerializeField()>
	Private giantRoot As Transform

	' Token: 0x04002B26 RID: 11046
	<SerializeField()>
	Private handFront As Transform

	' Token: 0x04002B27 RID: 11047
	<SerializeField()>
	Private handBack As Transform

	' Token: 0x04002B28 RID: 11048
	<SerializeField()>
	Private deathPuffRoot As Transform

	' Token: 0x04002B29 RID: 11049
	<SerializeField()>
	Private morphRoot As Transform

	' Token: 0x04002B2A RID: 11050
	<SerializeField()>
	Private marionetteRoot As Transform

	' Token: 0x04002B2B RID: 11051
	<SerializeField()>
	Private spark As GameObject

	' Token: 0x04002B2C RID: 11052
	Private meditateP1 As FlyingGenieLevelMeditateFX

	' Token: 0x04002B2D RID: 11053
	Private meditateP2 As FlyingGenieLevelMeditateFX

	' Token: 0x04002B2E RID: 11054
	Private spawner As FlyingGenieLevelSpawner

	' Token: 0x04002B2F RID: 11055
	Private bombs As List(Of FlyingGenieLevelBomb)

	' Token: 0x04002B30 RID: 11056
	Private pyramids As List(Of FlyingGenieLevelPyramid)

	' Token: 0x04002B31 RID: 11057
	Private damageDealer As DamageDealer

	' Token: 0x04002B32 RID: 11058
	Private damageReceiver As DamageReceiver

	' Token: 0x04002B33 RID: 11059
	Private startPos As Vector3

	' Token: 0x04002B34 RID: 11060
	Private pyramidsGoingClockwise As Boolean

	' Token: 0x04002B35 RID: 11061
	Private isShooting As Boolean

	' Token: 0x04002B37 RID: 11063
	Private transitionHP As Single

	' Token: 0x04002B38 RID: 11064
	Private pinkIndex As Integer

	' Token: 0x04002B39 RID: 11065
	Private pinkString As String()

	' Token: 0x0200066C RID: 1644
	Public Enum State
		' Token: 0x04002B3B RID: 11067
		Intro
		' Token: 0x04002B3C RID: 11068
		Idle
		' Token: 0x04002B3D RID: 11069
		Marionette
		' Token: 0x04002B3E RID: 11070
		Giant
		' Token: 0x04002B3F RID: 11071
		Dead
	End Enum
End Class
