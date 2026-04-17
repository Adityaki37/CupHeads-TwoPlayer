Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020007A1 RID: 1953
Public Class RumRunnersLevelWorm
	Inherits LevelProperties.RumRunners.Entity

	' Token: 0x17000401 RID: 1025
	' (get) Token: 0x06002B99 RID: 11161 RVA: 0x001966BA File Offset: 0x00194ABA
	' (set) Token: 0x06002B9A RID: 11162 RVA: 0x001966C2 File Offset: 0x00194AC2
	Public Property introDrop As Boolean

	' Token: 0x17000402 RID: 1026
	' (get) Token: 0x06002B9B RID: 11163 RVA: 0x001966CB File Offset: 0x00194ACB
	' (set) Token: 0x06002B9C RID: 11164 RVA: 0x001966D3 File Offset: 0x00194AD3
	Public Property isDead As Boolean

	' Token: 0x06002B9D RID: 11165 RVA: 0x001966DC File Offset: 0x00194ADC
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.boxCollider = MyBase.GetComponent(Of BoxCollider2D)()
		Me.boxCollider.enabled = False
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
	End Sub

	' Token: 0x06002B9E RID: 11166 RVA: 0x00196735 File Offset: 0x00194B35
	Public Overrides Sub LevelInit(properties As LevelProperties.RumRunners)
		MyBase.LevelInit(properties)
	End Sub

	' Token: 0x06002B9F RID: 11167 RVA: 0x00196740 File Offset: 0x00194B40
	Public Sub Setup()
		MyBase.gameObject.SetActive(True)
		Me.phonographPos = MyBase.transform.position
		Dim position As Vector3 = Me.laserGroup1.transform.parent.position
		Dim vector As Vector3 = New Vector3(MyBase.transform.position.x, 720F)
		MyBase.transform.position = vector
		Me.diamond.transform.position = Me.phonographPos
		Me.laserGroup1.transform.parent.position = position
	End Sub

	' Token: 0x06002BA0 RID: 11168 RVA: 0x001967D8 File Offset: 0x00194BD8
	Public Sub StartWorm(introDamage As Single)
		Me.bossMaxHealth = MyBase.properties.CurrentHealth
		If introDamage > 0F Then
			MyBase.properties.DealDamage(introDamage * MyBase.properties.CurrentState.worm.introDamageMultiplier)
			Me.GetNewSpeed()
		End If
		Me.diamond.transform.parent = Nothing
		Me.laserGroup1.transform.parent.parent = Nothing
		Me.speed = MyBase.properties.CurrentState.worm.rotationSpeedRange.min
		MyBase.StartCoroutine(Me.bugIntro_cr())
	End Sub

	' Token: 0x06002BA1 RID: 11169 RVA: 0x00196880 File Offset: 0x00194C80
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		If Not Me.canTakeDamage Then
			Return
		End If
		MyBase.properties.DealDamage(info.damage)
		Me.GetNewSpeed()
		If Level.Current.mode = Level.Mode.Easy AndAlso Not Me.isDead AndAlso MyBase.properties.CurrentHealth <= 0F Then
			Me.StartDeath()
		End If
	End Sub

	' Token: 0x06002BA2 RID: 11170 RVA: 0x001968E5 File Offset: 0x00194CE5
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		Me.damageDealer.DealDamage(hit)
	End Sub

	' Token: 0x06002BA3 RID: 11171 RVA: 0x001968FC File Offset: 0x00194CFC
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06002BA4 RID: 11172 RVA: 0x00196914 File Offset: 0x00194D14
	Public Sub StartBarrels()
		Me.runnersCoroutine = MyBase.StartCoroutine(Me.spawnRunners_cr())
	End Sub

	' Token: 0x06002BA5 RID: 11173 RVA: 0x00196928 File Offset: 0x00194D28
	Private Sub AniEvent_StartBug()
		MyBase.StartCoroutine(Me.revealLaser_cr())
		Me.lasersChangeCoroutine = MyBase.StartCoroutine(Me.lasersChangeDir_cr())
		Me.lasersTurnOnCoroutine = MyBase.StartCoroutine(Me.lasersTurnOn_cr())
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x06002BA6 RID: 11174 RVA: 0x00196968 File Offset: 0x00194D68
	Private Iterator Function bugIntro_cr() As IEnumerator
		Me.boxCollider.enabled = True
		Me.canTakeDamage = True
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		Dim startPos As Vector3 = New Vector3(MyBase.transform.position.x, 720F)
		Dim endPos As Vector3 = Me.phonographPos
		Me.offscreenPos = startPos
		MyBase.transform.position = startPos
		MyBase.animator.Play(0, 0, 0.2F)
		Dim elapsedTime As Single = 0F
		Dim canDrop As Boolean = False
		While Not canDrop
			If Me.introDrop Then
				Dim normalizedTime As Single = MyBase.animator.GetCurrentAnimatorStateInfo(0).normalizedTime
				canDrop = normalizedTime >= 0.9F OrElse normalizedTime <= 0.1F
			End If
			elapsedTime += CupheadTime.FixedDelta
			MyBase.transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / 1.65F)
			Yield wait
		End While
		MyBase.animator.SetTrigger("Continue")
		elapsedTime = 0F
		Dim dustSpawned As Boolean = False
		Dim dropPosition As Single = MyBase.transform.position.y
		While elapsedTime < 0.6F
			elapsedTime += CupheadTime.FixedDelta
			Dim t As Single = elapsedTime / 0.6F
			Dim position As Vector3 = MyBase.transform.position
			Dim startPosition As Single = dropPosition
			If t >= 0.36363637F Then
				startPosition = (dropPosition - endPos.y) * 0.6F + endPos.y
			End If
			position.y = EaseUtils.EaseOutBounce(startPosition, endPos.y, t)
			MyBase.transform.position = position
			Dim shadowIndex As Integer = Mathf.Clamp(Mathf.RoundToInt(t * 10F), 0, Me.dropShadowSprites.Length - 1)
			Me.fakePhonographShadowRenderer.sprite = Me.dropShadowSprites(shadowIndex)
			Me.fakePhonographShadowRenderer.transform.position = endPos
			If Not dustSpawned AndAlso t >= 0.36363637F Then
				Me.dropDustEffect.Create(endPos)
				dustSpawned = True
				CupheadLevelCamera.Current.Shake(10F, 0.3F, False)
				Me.diamond.GetComponent(Of Collider2D)().enabled = True
			End If
			Yield wait
		End While
		MyBase.animator.SetTrigger("Continue")
		Yield MyBase.animator.WaitForNormalizedTime(Me, 1F, "IntroEnd1", 0, True, False, True)
		Me.fakePhonographShadowRenderer.sprite = Nothing
		MyBase.animator.Play("IntroEnd2")
		Me.diamond.animator.Play("Slack", 0)
		Yield Me.diamond.animator.WaitForNormalizedTime(Me, 1F, "Slack", 0, True, False, True)
		Me.diamond.animator.Play("Loop", 0)
		Me.diamond.animator.Play("Idle", 1)
		Return
	End Function

	' Token: 0x06002BA7 RID: 11175 RVA: 0x00196984 File Offset: 0x00194D84
	Private Iterator Function revealLaser_cr() As IEnumerator
		Me.diamond.StartSparkle()
		Me.laserGroup1.Begin()
		Yield CupheadTime.WaitForSeconds(Me, 0.5F)
		Me.laserGroup2.Begin()
		Return
	End Function

	' Token: 0x06002BA8 RID: 11176 RVA: 0x001969A0 File Offset: 0x00194DA0
	Private Iterator Function lasersChangeDir_cr() As IEnumerator
		Dim p As LevelProperties.RumRunners.Worm = MyBase.properties.CurrentState.worm
		Dim directionPattern As String() = p.directionAttackString.GetRandom().Split(New Char() { ","c })
		Dim directionIndex As Integer = Global.UnityEngine.Random.Range(0, directionPattern.Length)
		Me.groupSpeed1 = Me.speed
		Me.groupSpeed2 = -Me.speed
		MyBase.StartCoroutine(Me.lasersRotate_cr())
		While Not Me.isDead
			Parser.IntTryParse(directionPattern(directionIndex), Me.direction)
			If Me.direction = 1 Then
				Me.groupSpeed1 = Me.speed
				Me.groupSpeed2 = -Me.speed
			Else
				Me.groupSpeed1 = -Me.speed
				Me.groupSpeed2 = Me.speed
			End If
			Yield CupheadTime.WaitForSeconds(Me, p.directionTime)
			directionIndex = (directionIndex + 1) Mod directionPattern.Length
		End While
		Return
	End Function

	' Token: 0x06002BA9 RID: 11177 RVA: 0x001969BC File Offset: 0x00194DBC
	Private Iterator Function lasersTurnOn_cr() As IEnumerator
		Dim p As LevelProperties.RumRunners.Worm = MyBase.properties.CurrentState.worm
		Me.MusicSnapshot_StartGreenBeam()
		While Not Me.isDead
			Dim currentLaser As RumRunnersLevelLaser = If((Not Rand.Bool()), Me.laserGroup2, Me.laserGroup1)
			Yield CupheadTime.WaitForSeconds(Me, p.attackOffDurationRange.RandomFloat())
			Yield Nothing
			If currentLaser IsNot Nothing Then
				currentLaser.Warning()
				Me.MusicSnapshot_StartYellowBeam()
			End If
			Yield CupheadTime.WaitForSeconds(Me, p.warningDuration)
			Yield Nothing
			If currentLaser IsNot Nothing Then
				Me.lasersAttack(currentLaser)
				Me.audioWarble.HandleWarble()
				Me.MusicSnapshot_StartRedBeam()
			End If
			Yield CupheadTime.WaitForSeconds(Me, p.attackOnDurationRange.RandomFloat())
			Yield Nothing
			If currentLaser IsNot Nothing Then
				Me.lasersEndAttack(currentLaser)
				Me.MusicSnapshot_StartGreenBeam()
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06002BAA RID: 11178 RVA: 0x001969D8 File Offset: 0x00194DD8
	Private Iterator Function lasersRotate_cr() As IEnumerator
		While True
			If Me.laserGroup1 IsNot Nothing Then
				Me.laserGroup1.transform.Rotate(Vector3.forward * Me.groupSpeed1 * CupheadTime.Delta)
			End If
			If Me.laserGroup2 IsNot Nothing Then
				Me.laserGroup2.transform.Rotate(Vector3.forward * Me.groupSpeed2 * CupheadTime.Delta)
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06002BAB RID: 11179 RVA: 0x001969F4 File Offset: 0x00194DF4
	Public Sub GetNewSpeed()
		Dim rotationSpeedRange As MinMax = MyBase.properties.CurrentState.worm.rotationSpeedRange
		Dim num As Single = MyBase.properties.CurrentHealth / Me.bossMaxHealth
		Dim num2 As Single = 1F - num
		Me.speed = rotationSpeedRange.min + rotationSpeedRange.max * num2
		If Me.direction = 1 Then
			Me.groupSpeed1 = Me.speed
			Me.groupSpeed2 = -Me.speed
		Else
			Me.groupSpeed1 = -Me.speed
			Me.groupSpeed2 = Me.speed
		End If
	End Sub

	' Token: 0x06002BAC RID: 11180 RVA: 0x00196A8C File Offset: 0x00194E8C
	Private Sub endLasers()
		MyBase.StopCoroutine(Me.lasersTurnOnCoroutine)
		MyBase.StopCoroutine(Me.lasersChangeCoroutine)
		Me.laserGroup1.CancelWarning()
		Me.laserGroup2.CancelWarning()
		Me.lasersEndAttack(Me.laserGroup1)
		Me.lasersEndAttack(Me.laserGroup2)
		Me.laserGroup1.[End]()
		Me.laserGroup2.[End]()
		Me.diamond.EndSparkle()
	End Sub

	' Token: 0x06002BAD RID: 11181 RVA: 0x00196B00 File Offset: 0x00194F00
	Private Sub lasersAttack(laserGroup As RumRunnersLevelLaser)
		laserGroup.Attack()
		Me.diamond.SetAttack(True)
	End Sub

	' Token: 0x06002BAE RID: 11182 RVA: 0x00196B14 File Offset: 0x00194F14
	Private Sub lasersEndAttack(laserGroup As RumRunnersLevelLaser)
		laserGroup.EndAttack()
		Me.diamond.SetAttack(False)
	End Sub

	' Token: 0x06002BAF RID: 11183 RVA: 0x00196B28 File Offset: 0x00194F28
	Private Iterator Function spawnRunners_cr() As IEnumerator
		Dim p As LevelProperties.RumRunners.Barrels = MyBase.properties.CurrentState.barrels
		Dim topDirection As Single = CSng(Rand.PosOrNeg())
		Dim bottom As Boolean = False
		Dim player As AbstractPlayerController = PlayerManager.GetPlayer(PlayerId.PlayerOne)
		If player Is Nothing Then
			player = PlayerManager.GetPlayer(PlayerId.PlayerTwo)
		End If
		If player IsNot Nothing Then
			Dim position As Vector3 = player.transform.position
			If(position.x > 0F AndAlso topDirection < 0F) OrElse (position.x < 0F AndAlso topDirection > 0F) Then
				bottom = True
			End If
		End If
		Dim barrelDelayPattern As PatternString = New PatternString(p.barrelDelayString, True)
		Dim parryString As PatternString = New PatternString(p.barrelParryString, True)
		While MyBase.properties.CurrentState.stateName <> LevelProperties.RumRunners.States.Anteater
			Dim isCop As Boolean = If((Not bottom), Me.topBarrelCop, Me.bottomBarrelCop)
			Me.bottomBarrelCop = If((Not bottom), Me.bottomBarrelCop, (Not Me.bottomBarrelCop))
			Me.topBarrelCop = If((Not bottom), (Not Me.topBarrelCop), Me.topBarrelCop)
			Dim r As RumRunnersLevelBarrel = Me.barrelPrefab.InstantiatePrefab(Of RumRunnersLevelBarrel)()
			Dim parryable As Boolean = Not isCop AndAlso parryString.PopLetter() = "P"c
			Dim spawnPos As Vector3 = If((Not bottom), Me.runnerSpawnPointTop.position, Me.runnerSpawnPointBottom.position)
			Dim direction As Single = topDirection * CSng(If((Not bottom), 1, (-1)))
			spawnPos.x *= direction
			r.LevelInit(MyBase.properties)
			r.Initialize(direction, spawnPos, Me, parryable, isCop)
			bottom = Not bottom
			Dim delayTime As Single = barrelDelayPattern.PopFloat()
			Yield CupheadTime.WaitForSeconds(Me, delayTime)
		End While
		Return
	End Function

	' Token: 0x06002BB0 RID: 11184 RVA: 0x00196B44 File Offset: 0x00194F44
	Private Iterator Function move_cr() As IEnumerator
		Dim movingOut As Boolean = True
		Dim time As Single = MyBase.properties.CurrentState.worm.moveTime
		Dim startPos As Vector3 = New Vector3(-MyBase.properties.CurrentState.worm.moveDistance / 2F, MyBase.transform.position.y)
		Dim endPos As Vector3 = New Vector3(MyBase.properties.CurrentState.worm.moveDistance / 2F, MyBase.transform.position.y)
		Dim t As Single = time / 2F
		Dim kick As Boolean = False
		Dim endMove As Boolean = False
		Me.AnimationEvent_SFX_RUMRUN_BugGirl_Tapdance()
		Dim spriteRenderer As SpriteRenderer = MyBase.GetComponent(Of SpriteRenderer)()
		Dim initialSortingOrder As Integer = spriteRenderer.sortingOrder
		spriteRenderer.sortingOrder = 10
		Dim waitInstruction As YieldInstruction = New WaitForFixedUpdate()
		Dim initialLoop As Boolean = True
		While Not endMove
			Dim start As Single = If((Not movingOut), endPos.x, startPos.x)
			Dim [end] As Single = If((Not movingOut), startPos.x, endPos.x)
			If initialLoop Then
				start = MyBase.transform.position.x
				t = 0F
				time /= 2F
			End If
			While t < time AndAlso Not Me.isDead
				t += CupheadTime.FixedDelta
				Dim val As Single = t / time
				Dim position As Vector3 = MyBase.transform.position
				position.x = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, start, [end], val)
				position.y = RumRunnersLevel.GroundWalkingPosY(position, Me.boxCollider, RumRunnersLevelWorm.PositionYOffset, RumRunnersLevelWorm.PositionYRayLength)
				MyBase.transform.position = position
				Yield waitInstruction
				If Not kick AndAlso val > 0.7F Then
					kick = True
					Dim text As String = If(([end] <= 0F), "KickLeft", "KickRight")
					MyBase.animator.SetTrigger(text)
				End If
			End While
			If Me.isDead Then
				endMove = True
				MyBase.StartCoroutine(Me.defeat_cr())
			Else
				movingOut = Not movingOut
				kick = False
				t = 0F
				MyBase.transform.SetPosition(New Single?([end]), Nothing, Nothing)
				If initialLoop Then
					time *= 2F
					initialLoop = False
				End If
			End If
			Yield Nothing
		End While
		MyBase.StartCoroutine(Me.deathMove_cr(initialSortingOrder))
		Return
	End Function

	' Token: 0x06002BB1 RID: 11185 RVA: 0x00196B60 File Offset: 0x00194F60
	Private Iterator Function defeat_cr() As IEnumerator
		MyBase.animator.SetBool("EasyMode", Level.Current.mode = Level.Mode.Easy)
		MyBase.animator.Play("Defeat")
		MyBase.transform.localScale = New Vector3(Mathf.Sign(MyBase.transform.position.x), 1F)
		Yield CupheadTime.WaitForSeconds(Me, 0.5F)
		Me.diamond.animator.Play("Defeat")
		Return
	End Function

	' Token: 0x06002BB2 RID: 11186 RVA: 0x00196B7C File Offset: 0x00194F7C
	Private Iterator Function deathMove_cr(initialSortingOrder As Integer) As IEnumerator
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		Dim FALL_SPEED As Single = If((Level.Current.mode <> Level.Mode.Easy), 400F, 200F)
		Dim flipped As Boolean = MyBase.transform.position.x < 0F
		Dim POS_X As Single = 612F * Mathf.Sign(MyBase.transform.position.x)
		Dim fallTime As Single = (Mathf.Abs(POS_X) - Mathf.Abs(MyBase.transform.position.x)) / FALL_SPEED
		Dim startPos As Single = MyBase.transform.position.x
		Dim endpos As Single = POS_X
		Dim elapsedTime As Single = 0F
		While elapsedTime < fallTime
			elapsedTime += CupheadTime.FixedDelta
			Dim position As Vector3 = MyBase.transform.position
			position.x = Mathf.Lerp(startPos, endpos, elapsedTime / fallTime)
			position.y = RumRunnersLevel.GroundWalkingPosY(position, Me.boxCollider, RumRunnersLevelWorm.PositionYOffset, RumRunnersLevelWorm.PositionYRayLength)
			MyBase.transform.position = position
			Yield wait
		End While
		MyBase.animator.SetTrigger("Continue")
		If Level.Current.mode = Level.Mode.Easy Then
			Me.StopAllCoroutines()
			Return
		End If
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Fall", False, True)
		Me.canTakeDamage = False
		MyBase.GetComponent(Of HitFlash)().disabled = True
		MyBase.GetComponent(Of SpriteRenderer)().sortingOrder = initialSortingOrder
		elapsedTime = 0F
		startPos = MyBase.transform.position.x
		Dim targetPositionX As Single = If((Not flipped), Me.phonographPos.x, (-105F))
		MyBase.animator.enabled = False
		While elapsedTime < 2F
			Dim position2 As Vector2 = MyBase.transform.position
			position2.x = Mathf.Lerp(startPos, targetPositionX, elapsedTime / 2F)
			position2.y = RumRunnersLevel.GroundWalkingPosY(position2, Me.boxCollider, RumRunnersLevelWorm.PositionYOffset, RumRunnersLevelWorm.PositionYRayLength)
			MyBase.transform.position = position2
			Yield Nothing
			MyBase.animator.Update(CupheadTime.Delta)
			elapsedTime = MyBase.animator.GetCurrentAnimatorStateInfo(0).normalizedTime
		End While
		MyBase.transform.SetPosition(New Single?(targetPositionX), Nothing, Nothing)
		MyBase.animator.enabled = True
		MyBase.animator.SetBool("Flipped", flipped)
		MyBase.animator.SetTrigger("End")
		Dim animationName As String = If((Not flipped), "Jump", "JumpFlipped")
		Yield MyBase.animator.WaitForNormalizedTime(Me, 1F, animationName, 0, True, False, True)
		animationName = If((Not flipped), "JumpSquish", "JumpSquishFlipped")
		MyBase.animator.Play(animationName)
		Me.diamond.animator.Play(If((Not flipped), "DefeatSquish", "DefeatSquishFlipped"))
		Yield MyBase.animator.WaitForNormalizedTime(Me, 1F, animationName, 0, True, False, True)
		MyBase.transform.SetPosition(New Single?(Me.phonographPos.x * Mathf.Sign(MyBase.transform.position.x) + If((Not flipped), 0F, (-6F))), Nothing, Nothing)
		MyBase.animator.Play("Wave")
		Me.diamond.GetComponent(Of Collider2D)().enabled = False
		elapsedTime = 0F
		Dim start As Vector3 = MyBase.transform.position
		Dim targetPosition As Vector3 = New Vector3(MyBase.transform.position.x, Me.offscreenPos.y)
		Me.diamond.transform.parent = MyBase.transform
		MyBase.StartCoroutine(Me.exitShadow_cr())
		While elapsedTime < 0.866F
			elapsedTime += CupheadTime.FixedDelta
			MyBase.transform.position = Vector3.Lerp(start, targetPosition, elapsedTime / 0.866F)
			Yield wait
		End While
		Me.StopAllCoroutines()
		Me.diamond.Die()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Yield Nothing
		Return
	End Function

	' Token: 0x06002BB3 RID: 11187 RVA: 0x00196BA0 File Offset: 0x00194FA0
	Private Iterator Function exitShadow_cr() As IEnumerator
		Me.realPhonographShadowRenderer.enabled = False
		Dim position As Vector3 = Me.realPhonographShadowRenderer.transform.position
		Dim accumulator As Single = 0F
		Dim index As Integer = 4
		While index >= 0
			Me.fakePhonographShadowRenderer.sprite = Me.dropShadowSprites(index)
			Me.fakePhonographShadowRenderer.transform.position = position
			Yield Nothing
			accumulator += CupheadTime.Delta
			If accumulator > 0.041666668F Then
				accumulator -= 0.041666668F
				index -= 1
			End If
		End While
		Me.fakePhonographShadowRenderer.sprite = Nothing
		Return
	End Function

	' Token: 0x06002BB4 RID: 11188 RVA: 0x00196BBC File Offset: 0x00194FBC
	Public Sub StartDeath()
		If Me.isDead Then
			Return
		End If
		Me.AnimationEvent_SFX_RUMRUN_BugGirl_Tapdance_Stop()
		Me.SFX_RUMRUN_BugGirl_DieFalltoGround()
		Me.MusicSnapshot_RevertToDefault()
		Me.isDead = True
		Me.endLasers()
		MyBase.StopCoroutine(Me.runnersCoroutine)
		If Level.Current.mode = Level.Mode.Easy Then
			MyBase.GetComponent(Of LevelBossDeathExploder)().StartExplosion()
		End If
	End Sub

	' Token: 0x06002BB5 RID: 11189 RVA: 0x00196C1C File Offset: 0x0019501C
	Protected Overridable Sub MusicSnapshot_StartGreenBeam()
		AudioManager.SnapshotTransition(New String() { "RumRunners_GreenBeam", "Unpaused", "Unpaused_1920s" }, New Single() { 1F, 0F, 0F }, 0.5F)
	End Sub

	' Token: 0x06002BB6 RID: 11190 RVA: 0x00196C74 File Offset: 0x00195074
	Protected Overridable Sub MusicSnapshot_StartYellowBeam()
		AudioManager.SnapshotTransition(New String() { "RumRunners_YellowBeam", "Unpaused", "Unpaused_1920s" }, New Single() { 1F, 0F, 0F }, 0.5F)
	End Sub

	' Token: 0x06002BB7 RID: 11191 RVA: 0x00196CCC File Offset: 0x001950CC
	Protected Overridable Sub MusicSnapshot_StartRedBeam()
		AudioManager.SnapshotTransition(New String() { "RumRunners_RedBeam", "RumRunners_GreenBeam", "Unpaused_1920s" }, New Single() { 1F, 0F, 0F }, 0.16F)
	End Sub

	' Token: 0x06002BB8 RID: 11192 RVA: 0x00196D24 File Offset: 0x00195124
	Protected Overridable Sub MusicSnapshot_RevertToDefault()
		Dim array As String() = New String(1) {}
		array(0) = "RumRunners_RedBeam"
		If SettingsData.Data.vintageAudioEnabled Then
			array(1) = "Unpaused_1920s"
		Else
			array(1) = "Unpaused"
		End If
		AudioManager.SnapshotTransition(array, New Single() { 0F, 1F }, 3F)
	End Sub

	' Token: 0x06002BB9 RID: 11193 RVA: 0x00196D88 File Offset: 0x00195188
	Private Iterator Function StartRedBeamMusicSnapshotWait_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 3F)
		Return
	End Function

	' Token: 0x06002BBA RID: 11194 RVA: 0x00196DA3 File Offset: 0x001951A3
	Private Sub SFX_RUMRUN_BugGirl_DieFalltoGround()
		AudioManager.Play("sfx_DLC_RUMRUN_P2_BugGirl_DieFalltoGround")
		Me.emitAudioFromObject.Add("sfx_DLC_RUMRUN_P2_BugGirl_DieFalltoGround")
	End Sub

	' Token: 0x06002BBB RID: 11195 RVA: 0x00196DBF File Offset: 0x001951BF
	Private Sub AnimationEvent_SFX_RUMRUN_BugGirl_DismountJumpLand()
		AudioManager.Play("sfx_DLC_RUMRUN_P2_BugGirl_DismountJumpLand")
		Me.emitAudioFromObject.Add("sfx_DLC_RUMRUN_P2_BugGirl_DismountJumpLand")
	End Sub

	' Token: 0x06002BBC RID: 11196 RVA: 0x00196DDB File Offset: 0x001951DB
	Private Sub AnimationEvent_SFX_RUMRUN_BugGirl_ExitWinch()
		AudioManager.Play("sfx_DLC_RUMRUN_P2_BugGirl_ExitWinch")
		Me.emitAudioFromObject.Add("sfx_DLC_RUMRUN_P2_BugGirl_ExitWinch")
	End Sub

	' Token: 0x06002BBD RID: 11197 RVA: 0x00196DF7 File Offset: 0x001951F7
	Private Sub AnimationEvent_SFX_RUMRUN_BugGirl_Tapdance()
		AudioManager.PlayLoop("sfx_dlc_rumrun_p2_buggirl_tapdance")
		Me.emitAudioFromObject.Add("sfx_dlc_rumrun_p2_buggirl_tapdance")
	End Sub

	' Token: 0x06002BBE RID: 11198 RVA: 0x00196E13 File Offset: 0x00195213
	Private Sub AnimationEvent_SFX_RUMRUN_BugGirl_Tapdance_Stop()
		AudioManager.[Stop]("sfx_dlc_rumrun_p2_buggirl_tapdance")
	End Sub

	' Token: 0x06002BBF RID: 11199 RVA: 0x00196E1F File Offset: 0x0019521F
	Private Sub AnimationEvent_SFX_RUMRUN_BugGirl_VocalDismountLaugh()
		AudioManager.Play("sfx_DLC_RUMRUN_P2_BugGirl_VocalDismountLaugh")
		Me.emitAudioFromObject.Add("sfx_DLC_RUMRUN_P2_BugGirl_VocalDismountLaugh")
	End Sub

	' Token: 0x06002BC0 RID: 11200 RVA: 0x00196E3B File Offset: 0x0019523B
	Private Sub AnimationEvent_SFX_RUMRUN_BugGirl_VocalExcited()
		AudioManager.Play("sfx_DLC_RUMRUN_P2_BugGirl_VocalExcited")
		Me.emitAudioFromObject.Add("sfx_DLC_RUMRUN_P2_BugGirl_VocalExcited")
	End Sub

	' Token: 0x0400344F RID: 13391
	Private Shared PositionYOffset As Single = 20F

	' Token: 0x04003450 RID: 13392
	Private Shared PositionYRayLength As Single = 250F

	' Token: 0x04003451 RID: 13393
	<SerializeField()>
	Private dropShadowSprites As Sprite()

	' Token: 0x04003452 RID: 13394
	<SerializeField()>
	Private fakePhonographShadowRenderer As SpriteRenderer

	' Token: 0x04003453 RID: 13395
	<SerializeField()>
	Private realPhonographShadowRenderer As SpriteRenderer

	' Token: 0x04003454 RID: 13396
	<SerializeField()>
	Private dropDustEffect As Effect

	' Token: 0x04003455 RID: 13397
	<SerializeField()>
	Private laserGroup1 As RumRunnersLevelLaser

	' Token: 0x04003456 RID: 13398
	<SerializeField()>
	Private laserGroup2 As RumRunnersLevelLaser

	' Token: 0x04003457 RID: 13399
	<SerializeField()>
	Private diamond As RumRunnersLevelDiamond

	' Token: 0x04003458 RID: 13400
	<SerializeField()>
	Private barrelPrefab As RumRunnersLevelBarrel

	' Token: 0x04003459 RID: 13401
	<SerializeField()>
	Private runnerSpawnPointTop As Transform

	' Token: 0x0400345A RID: 13402
	<SerializeField()>
	Private runnerSpawnPointBottom As Transform

	' Token: 0x0400345B RID: 13403
	<SerializeField()>
	Private audioWarble As AudioWarble

	' Token: 0x0400345C RID: 13404
	Private damageDealer As DamageDealer

	' Token: 0x0400345D RID: 13405
	Private damageReceiver As DamageReceiver

	' Token: 0x0400345E RID: 13406
	Private canTakeDamage As Boolean

	' Token: 0x0400345F RID: 13407
	Private phonographPos As Vector3

	' Token: 0x04003460 RID: 13408
	Private offscreenPos As Vector3

	' Token: 0x04003461 RID: 13409
	Private speed As Single

	' Token: 0x04003462 RID: 13410
	Private groupSpeed1 As Single

	' Token: 0x04003463 RID: 13411
	Private groupSpeed2 As Single

	' Token: 0x04003464 RID: 13412
	Private bossMaxHealth As Single

	' Token: 0x04003465 RID: 13413
	Private topBarrelCop As Boolean

	' Token: 0x04003466 RID: 13414
	Private bottomBarrelCop As Boolean

	' Token: 0x04003467 RID: 13415
	Private direction As Integer

	' Token: 0x04003468 RID: 13416
	Private runnersCoroutine As Coroutine

	' Token: 0x04003469 RID: 13417
	Private lasersChangeCoroutine As Coroutine

	' Token: 0x0400346A RID: 13418
	Private lasersTurnOnCoroutine As Coroutine

	' Token: 0x0400346B RID: 13419
	Private boxCollider As BoxCollider2D
End Class
