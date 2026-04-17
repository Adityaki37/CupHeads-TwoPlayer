Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x0200057F RID: 1407
Public Class DevilLevelSittingDevil
	Inherits LevelProperties.Devil.Entity

	' Token: 0x06001ABE RID: 6846 RVA: 0x000F539C File Offset: 0x000F379C
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		Me.leftWallPositionX = Me.leftWall.transform.position.x
		Me.rightWallPositionX = Me.rightWall.transform.position.x
	End Sub

	' Token: 0x06001ABF RID: 6847 RVA: 0x000F540E File Offset: 0x000F380E
	Private Sub Start()
		Me.dragonPos = Me.dragonHead.transform.position
		MyBase.StartCoroutine(Me.intro_cr())
	End Sub

	' Token: 0x06001AC0 RID: 6848 RVA: 0x000F5434 File Offset: 0x000F3834
	Public Overrides Sub LevelInit(properties As LevelProperties.Devil)
		MyBase.LevelInit(properties)
		Me.isSpiderAttackNext = Rand.Bool()
		Me.spiderOffsets = properties.CurrentState.spider.positionOffset.Split(New Char() { ","c })
		Me.spiderOffsetIndex = Global.UnityEngine.Random.Range(0, Me.spiderOffsets.Length)
		Me.pitchforkPattern = properties.CurrentState.pitchfork.patternString.RandomChoice().Split(New Char() { ","c })
		Me.pitchforkPatternIndex = Global.UnityEngine.Random.Range(0, Me.pitchforkPattern.Length)
		Me.pitchforkTwoFlameWheelSpawner = New DevilLevelPitchforkProjectileSpawner(2, properties.CurrentState.pitchforkTwoFlameWheel.angleOffset)
		Me.pitchforkThreeFlameJumperSpawner = New DevilLevelPitchforkProjectileSpawner(3, properties.CurrentState.pitchforkThreeFlameJumper.angleOffset)
		Me.pitchforkFourFlameBouncerSpawner = New DevilLevelPitchforkProjectileSpawner(4, properties.CurrentState.pitchforkFourFlameBouncer.angleOffset)
		Me.pitchforkFiveFlameSpinnerSpawner = New DevilLevelPitchforkProjectileSpawner(4, properties.CurrentState.pitchforkFiveFlameSpinner.angleOffset)
		Me.pitchforkSixFlameRingSpawner = New DevilLevelPitchforkProjectileSpawner(6, properties.CurrentState.pitchforkSixFlameRing.angleOffset)
		If Level.CurrentMode = Level.Mode.Easy Then
			AddHandler properties.OnBossDeath, AddressOf Me.DeathEasy
		End If
	End Sub

	' Token: 0x06001AC1 RID: 6849 RVA: 0x000F5574 File Offset: 0x000F3974
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.demonPrefab = Nothing
		Me.wheelProjectilePrefab = Nothing
		Me.wheelOrbitingProjectilePrefab = Nothing
		Me.jumpingProjectilePrefab = Nothing
		Me.bouncingProjectilePrefab = Nothing
		Me.spinnerOrbitingProjectilePrefab = Nothing
		Me.spinnerProjectilePrefab = Nothing
		Me.ringProjectilePrefab = Nothing
	End Sub

	' Token: 0x06001AC2 RID: 6850 RVA: 0x000F55B4 File Offset: 0x000F39B4
	Private Iterator Function intro_cr() As IEnumerator
		Me.state = DevilLevelSittingDevil.State.Intro
		Yield CupheadTime.WaitForSeconds(Me, 0.25F)
		MyBase.animator.SetTrigger("Continue")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Intro", False, True)
		Me.state = DevilLevelSittingDevil.State.Idle
		Yield Nothing
		Return
	End Function

	' Token: 0x06001AC3 RID: 6851 RVA: 0x000F55CF File Offset: 0x000F39CF
	Public Sub StartDemons()
		MyBase.StartCoroutine(Me.demon_cr())
	End Sub

	' Token: 0x06001AC4 RID: 6852 RVA: 0x000F55E0 File Offset: 0x000F39E0
	Private Iterator Function demon_cr() As IEnumerator
		Dim fromLeft As Boolean = Rand.Bool()
		Dim playedFirstSound As Boolean = False
		Yield CupheadTime.WaitForSeconds(Me, 3F)
		While Not Me.endPH1
			Yield Nothing
			If playedFirstSound Then
				AudioManager.Play("devil_small_flame_imp_spawn")
				Me.emitAudioFromObject.Add("devil_small_flame_imp_spawn")
			Else
				AudioManager.Play("devil_small_flame_imp_first_spawn")
				Me.emitAudioFromObject.Add("devil_small_flame_imp_first_spawn")
				playedFirstSound = True
			End If
			Dim demon As DevilLevelDemon = Me.demonPrefab.Create(If((Not fromLeft), Me.rightDemonPeek.position, Me.leftDemonPeek.position), CSng(If((Not fromLeft), (-1), 1)), MyBase.properties.CurrentState.demons.speed, MyBase.properties.CurrentState.demons.hp, Me)
			If fromLeft Then
				demon.JumpRoot = Me.leftDemonJumpRoot.position
				demon.RunRoot = Me.leftDemonRunRoot.position
				demon.PillarDestination = Me.leftDemonPillar.position
				demon.FrontSpawn = Me.leftDemonFront.position
			Else
				demon.JumpRoot = Me.rightDemonJumpRoot.position
				demon.RunRoot = Me.rightDemonRunRoot.position
				demon.PillarDestination = Me.rightDemonPillar.position
				demon.FrontSpawn = Me.rightDemonFront.position
			End If
			fromLeft = Not fromLeft
			Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.demons.delay)
		End While
		Return
	End Function

	' Token: 0x06001AC5 RID: 6853 RVA: 0x000F55FB File Offset: 0x000F39FB
	Public Sub StartClap()
		Me.state = DevilLevelSittingDevil.State.Clap
		MyBase.StartCoroutine(Me.clap_cr())
	End Sub

	' Token: 0x06001AC6 RID: 6854 RVA: 0x000F5614 File Offset: 0x000F3A14
	Private Iterator Function clap_cr() As IEnumerator
		Dim p As LevelProperties.Devil.Clap = MyBase.properties.CurrentState.clap
		MyBase.animator.SetBool("StartRam", True)
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Ram_Start", False, True)
		Yield CupheadTime.WaitForSeconds(Me, p.delay.RandomFloat())
		For Each devilLevelDevilArm As DevilLevelDevilArm In Me.arms
			devilLevelDevilArm.Attack(p.speed)
		Next
		While Me.arms(0).state <> DevilLevelDevilArm.State.Idle
			Yield Nothing
		End While
		MyBase.animator.SetBool("StartRam", False)
		Yield CupheadTime.WaitForSeconds(Me, p.hesitate)
		Me.state = DevilLevelSittingDevil.State.Idle
		Return
	End Function

	' Token: 0x06001AC7 RID: 6855 RVA: 0x000F5630 File Offset: 0x000F3A30
	Public Sub StartHead()
		Me.state = DevilLevelSittingDevil.State.Head
		If Me.isSpiderAttackNext Then
			MyBase.StartCoroutine(Me.spider_cr())
		Else
			MyBase.StartCoroutine(Me.dragon_cr())
		End If
		Me.isSpiderAttackNext = Not Me.isSpiderAttackNext
	End Sub

	' Token: 0x06001AC8 RID: 6856 RVA: 0x000F5680 File Offset: 0x000F3A80
	Private Iterator Function spider_cr() As IEnumerator
		MyBase.animator.SetBool("StartSpider", True)
		Yield MyBase.animator.WaitForAnimationToStart(Me, "Spider_Start", False)
		AudioManager.Play("devil_spider_head_intro")
		Me.emitAudioFromObject.Add("devil_spider_head_intro")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Spider_Start", False, True)
		Dim p As LevelProperties.Devil.Spider = MyBase.properties.CurrentState.spider
		Dim numAttacks As Integer = p.numAttacks.RandomInt()
		For i As Integer = 0 To numAttacks - 1
			Yield CupheadTime.WaitForSeconds(Me, p.entranceDelay.RandomFloat())
			Me.spiderOffsetIndex = (Me.spiderOffsetIndex + 1) Mod Me.spiderOffsets.Length
			Dim offset As Single = 0F
			Parser.FloatTryParse(Me.spiderOffsets(Me.spiderOffsetIndex), offset)
			Me.spiderHead.Attack(Mathf.Clamp(PlayerManager.GetNext().center.x + offset, -620F, 620F), p.downSpeed, p.upSpeed)
			While Me.spiderHead.state <> DevilLevelSpiderHead.State.Idle
				Yield Nothing
			End While
		Next
		MyBase.animator.SetBool("StartSpider", False)
		Yield CupheadTime.WaitForSeconds(Me, p.hesitate)
		Me.state = DevilLevelSittingDevil.State.Idle
		Return
	End Function

	' Token: 0x06001AC9 RID: 6857 RVA: 0x000F569C File Offset: 0x000F3A9C
	Private Iterator Function dragon_cr() As IEnumerator
		MyBase.animator.SetBool("StartDragon", True)
		Dim isLeft As Boolean = Rand.Bool()
		MyBase.animator.SetBool("IsLeft", isLeft)
		Me.dragonHead.Attack(Me, isLeft)
		Dim p As LevelProperties.Devil.Dragon = MyBase.properties.CurrentState.dragon
		While Me.dragonHead.state <> DevilLevelDragonHead.State.Idle
			Yield Nothing
		End While
		MyBase.animator.SetBool("StartDragon", False)
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Morph_End", False, True)
		Yield CupheadTime.WaitForSeconds(Me, p.hesitate)
		Me.state = DevilLevelSittingDevil.State.Idle
		Yield Nothing
		Return
	End Function

	' Token: 0x06001ACA RID: 6858 RVA: 0x000F56B7 File Offset: 0x000F3AB7
	Private Sub DragonStop()
		MyBase.animator.SetTrigger("Continue")
		Me.dragonHead.state = DevilLevelDragonHead.State.Stopped
	End Sub

	' Token: 0x06001ACB RID: 6859 RVA: 0x000F56D5 File Offset: 0x000F3AD5
	Private Sub DragonReverse()
		MyBase.animator.SetTrigger("OnDragonEnd")
	End Sub

	' Token: 0x06001ACC RID: 6860 RVA: 0x000F56E7 File Offset: 0x000F3AE7
	Private Sub ResetPosition()
		Me.dragonHead.SetPosition(Me.dragonPos)
	End Sub

	' Token: 0x06001ACD RID: 6861 RVA: 0x000F56FA File Offset: 0x000F3AFA
	Public Sub StartPitchfork()
		Me.state = DevilLevelSittingDevil.State.Pitchfork
		MyBase.animator.SetBool("StartTrident", True)
	End Sub

	' Token: 0x06001ACE RID: 6862 RVA: 0x000F5714 File Offset: 0x000F3B14
	Private Sub SpawnProjectiles()
		Me.StartTridentHeadSFX()
		Me.pitchforkPatternIndex = (Me.pitchforkPatternIndex + 1) Mod Me.pitchforkPattern.Length
		Dim num As Integer = 0
		Parser.IntTryParse(Me.pitchforkPattern(Me.pitchforkPatternIndex), num)
		AudioManager.Play("devil_generic_projectile_start")
		Me.emitAudioFromObject.Add("devil_generic_projectile_start")
		Select Case num
			Case 2
				MyBase.StartCoroutine(Me.pitchforkTwoFlameWheel_cr())
			Case 3
				MyBase.StartCoroutine(Me.pitchforkThreeFlameJumper_cr())
			Case 4
				MyBase.StartCoroutine(Me.pitchforkFourFlameBouncer_cr())
			Case 5
				MyBase.StartCoroutine(Me.pitchforkFiveFlameSpinner_cr())
			Case 6
				MyBase.StartCoroutine(Me.pitchforkSixFlameRing_cr())
		End Select
	End Sub

	' Token: 0x06001ACF RID: 6863 RVA: 0x000F57EC File Offset: 0x000F3BEC
	Private Function getPitchforkFiringPos(angle As Single) As Vector2
		Return New Vector2(0F, MyBase.properties.CurrentState.pitchfork.spawnCenterY) + MathUtils.AngleToDirection(angle) * MyBase.properties.CurrentState.pitchfork.spawnRadius
	End Function

	' Token: 0x06001AD0 RID: 6864 RVA: 0x000F583D File Offset: 0x000F3C3D
	Private Sub StartParts()
		MyBase.animator.Play("Trident_Body", 2)
		MyBase.animator.Play("Trident_Attack", 3)
	End Sub

	' Token: 0x06001AD1 RID: 6865 RVA: 0x000F5861 File Offset: 0x000F3C61
	Private Sub StopParts()
		MyBase.animator.SetBool("StartTrident", False)
	End Sub

	' Token: 0x06001AD2 RID: 6866 RVA: 0x000F5874 File Offset: 0x000F3C74
	Private Iterator Function pitchforkTwoFlameWheel_cr() As IEnumerator
		Dim p As LevelProperties.Devil.PitchforkTwoFlameWheel = MyBase.properties.CurrentState.pitchforkTwoFlameWheel
		Dim projectiles As List(Of DevilLevelPitchforkWheelProjectile) = New List(Of DevilLevelPitchforkWheelProjectile)()
		Dim flipDelays As Boolean = Rand.Bool()
		For Each num As Single In Me.pitchforkTwoFlameWheelSpawner.getSpawnAngles()
			Dim flag As Boolean = projectiles.Count = 0
			If flipDelays Then
				flag = Not flag
			End If
			Dim num2 As Single = If((Not flag), p.secondAttackDelay, p.initialtAttackDelay)
			Dim devilLevelPitchforkWheelProjectile As DevilLevelPitchforkWheelProjectile = Me.wheelProjectilePrefab.Create(Me.getPitchforkFiringPos(num), num2, p.movementSpeed, Me)
			Me.wheelOrbitingProjectilePrefab.Create(devilLevelPitchforkWheelProjectile, 90F, CSng(Rand.PosOrNeg()) * p.rotationSpeed, 100F, Me)
			projectiles.Add(devilLevelPitchforkWheelProjectile)
		Next
		Dim allProjectilesFinished As Boolean = False
		While Not allProjectilesFinished
			allProjectilesFinished = True
			For Each devilLevelPitchforkWheelProjectile2 As DevilLevelPitchforkWheelProjectile In projectiles
				If devilLevelPitchforkWheelProjectile2 IsNot Nothing AndAlso devilLevelPitchforkWheelProjectile2.state <> DevilLevelPitchforkWheelProjectile.State.Returning Then
					allProjectilesFinished = False
				End If
			Next
			Yield Nothing
		End While
		AudioManager.Play("devil_generic_projectile_stop")
		Me.emitAudioFromObject.Add("devil_generic_projectile_stop")
		Yield CupheadTime.WaitForSeconds(Me, p.hesitate)
		Me.state = DevilLevelSittingDevil.State.Idle
		Return
	End Function

	' Token: 0x06001AD3 RID: 6867 RVA: 0x000F5890 File Offset: 0x000F3C90
	Private Iterator Function pitchforkThreeFlameJumper_cr() As IEnumerator
		Dim p As LevelProperties.Devil.PitchforkThreeFlameJumper = MyBase.properties.CurrentState.pitchforkThreeFlameJumper
		Dim projectiles As List(Of DevilLevelPitchforkJumpingProjectile) = New List(Of DevilLevelPitchforkJumpingProjectile)()
		For Each num As Single In Me.pitchforkThreeFlameJumperSpawner.getSpawnAngles()
			projectiles.Add(Me.jumpingProjectilePrefab.Create(Me.getPitchforkFiringPos(num), p.launchAngle, p.launchSpeed, p.gravity, p.numJumps, Me))
		Next
		projectiles.Shuffle()
		Dim delay As Single = p.initialAttackDelay.RandomFloat()
		For i As Integer = 0 To p.numJumps - 1
			For Each projectile As DevilLevelPitchforkJumpingProjectile In projectiles
				Yield CupheadTime.WaitForSeconds(Me, delay)
				projectile.Jump()
				delay = p.jumpDelay
			Next
		Next
		AudioManager.Play("devil_generic_projectile_stop")
		Me.emitAudioFromObject.Add("devil_generic_projectile_stop")
		Yield CupheadTime.WaitForSeconds(Me, p.hesitate)
		Me.state = DevilLevelSittingDevil.State.Idle
		Return
	End Function

	' Token: 0x06001AD4 RID: 6868 RVA: 0x000F58AC File Offset: 0x000F3CAC
	Private Iterator Function pitchforkFourFlameBouncer_cr() As IEnumerator
		Dim p As LevelProperties.Devil.PitchforkFourFlameBouncer = MyBase.properties.CurrentState.pitchforkFourFlameBouncer
		Dim projectiles As List(Of DevilLevelPitchforkBouncingProjectile) = New List(Of DevilLevelPitchforkBouncingProjectile)()
		Dim delay As Single = p.initialAttackDelay.RandomFloat()
		For Each num As Single In Me.pitchforkFourFlameBouncerSpawner.getSpawnAngles()
			projectiles.Add(Me.bouncingProjectilePrefab.Create(Me.getPitchforkFiringPos(num), delay, p.speed, num, p.numBounces, Me, MyBase.properties.CurrentState.pitchfork.dormantDuration))
		Next
		projectiles(Global.UnityEngine.Random.Range(0, projectiles.Count)).SetParryable(True)
		Dim allProjectilesFinished As Boolean = False
		While Not allProjectilesFinished
			allProjectilesFinished = True
			For Each devilLevelPitchforkBouncingProjectile As DevilLevelPitchforkBouncingProjectile In projectiles
				If devilLevelPitchforkBouncingProjectile.BouncesRemaining > 0 Then
					allProjectilesFinished = False
				End If
			Next
			Yield Nothing
		End While
		AudioManager.Play("devil_generic_projectile_stop")
		Me.emitAudioFromObject.Add("devil_generic_projectile_stop")
		Yield CupheadTime.WaitForSeconds(Me, p.hesitate)
		Me.state = DevilLevelSittingDevil.State.Idle
		Return
	End Function

	' Token: 0x06001AD5 RID: 6869 RVA: 0x000F58C8 File Offset: 0x000F3CC8
	Private Iterator Function pitchforkFiveFlameSpinner_cr() As IEnumerator
		Dim p As LevelProperties.Devil.PitchforkFiveFlameSpinner = MyBase.properties.CurrentState.pitchforkFiveFlameSpinner
		Dim centerProjectile As DevilLevelPitchforkSpinnerProjectile = Me.spinnerProjectilePrefab.Create(New Vector2(0F, MyBase.properties.CurrentState.pitchfork.spawnCenterY), p.maxSpeed, p.acceleration, p.attackDuration, Me, MyBase.properties.CurrentState.pitchfork.dormantDuration)
		Dim rotationSpeed As Single = CSng(Rand.PosOrNeg()) * p.rotationSpeed
		For Each num As Single In Me.pitchforkFiveFlameSpinnerSpawner.getSpawnAngles()
			Me.spinnerOrbitingProjectilePrefab.Create(centerProjectile, num, rotationSpeed, MyBase.properties.CurrentState.pitchfork.spawnRadius, Me, MyBase.properties.CurrentState.pitchfork.dormantDuration)
		Next
		AudioManager.Play("devil_generic_projectile_stop")
		Me.emitAudioFromObject.Add("devil_generic_projectile_stop")
		Yield CupheadTime.WaitForSeconds(Me, p.attackDuration + p.hesitate)
		Me.state = DevilLevelSittingDevil.State.Idle
		Return
	End Function

	' Token: 0x06001AD6 RID: 6870 RVA: 0x000F58E4 File Offset: 0x000F3CE4
	Private Iterator Function pitchforkSixFlameRing_cr() As IEnumerator
		Dim p As LevelProperties.Devil.PitchforkSixFlameRing = MyBase.properties.CurrentState.pitchforkSixFlameRing
		Dim projectiles As List(Of DevilLevelPitchforkRingProjectile) = New List(Of DevilLevelPitchforkRingProjectile)()
		For Each num As Single In Me.pitchforkSixFlameRingSpawner.getSpawnAngles()
			projectiles.Add(Me.ringProjectilePrefab.Create(Me.getPitchforkFiringPos(num), p.speed, p.groundDuration, Me, MyBase.properties.CurrentState.pitchfork.dormantDuration))
		Next
		projectiles(Global.UnityEngine.Random.Range(0, projectiles.Count)).SetParryable(True)
		Yield CupheadTime.WaitForSeconds(Me, p.initialAttackDelay.RandomFloat())
		projectiles(0).Attack()
		projectiles.RemoveAt(0)
		If Rand.Bool() Then
			projectiles.Reverse()
		End If
		For Each projectile As DevilLevelPitchforkRingProjectile In projectiles
			Yield CupheadTime.WaitForSeconds(Me, p.attackDelay)
			projectile.Attack()
		Next
		AudioManager.Play("devil_generic_projectile_stop")
		Me.emitAudioFromObject.Add("devil_generic_projectile_stop")
		Yield CupheadTime.WaitForSeconds(Me, p.hesitate)
		Me.state = DevilLevelSittingDevil.State.Idle
		Return
	End Function

	' Token: 0x06001AD7 RID: 6871 RVA: 0x000F5900 File Offset: 0x000F3D00
	Private Sub DeathEasy()
		If Level.Current.mode = Level.Mode.Easy Then
			RemoveHandler MyBase.properties.OnBossDeath, AddressOf Me.DeathEasy
			MyBase.GetComponent(Of LevelBossDeathExploder)().StartExplosion()
		End If
		MyBase.animator.Play("DeathEasy")
	End Sub

	' Token: 0x06001AD8 RID: 6872 RVA: 0x000F594E File Offset: 0x000F3D4E
	Public Sub StartTransform()
		Me.endPH1 = True
		Me.state = DevilLevelSittingDevil.State.EndPhase1
		MyBase.animator.SetTrigger("OnPhase2")
		MyBase.StartCoroutine(Me.on_phase_2_cr())
	End Sub

	' Token: 0x06001AD9 RID: 6873 RVA: 0x000F597C File Offset: 0x000F3D7C
	Private Iterator Function on_phase_2_cr() As IEnumerator
		Yield MyBase.animator.WaitForAnimationToStart(Me, "Death_Start", False)
		If Me.OnPhase1Death IsNot Nothing Then
			Me.OnPhase1Death()
		End If
		Yield MyBase.animator.WaitForAnimationToStart(Me, "Death_Hole", False)
		Me.middleGround.SetActive(False)
		MyBase.StartCoroutine(Me.move_fire_cr())
		Yield Nothing
		Return
	End Function

	' Token: 0x06001ADA RID: 6874 RVA: 0x000F5998 File Offset: 0x000F3D98
	Private Iterator Function move_fire_cr() As IEnumerator
		AudioManager.PlayLoop("devil_fire_wall")
		Me.emitAudioFromObject.Add("devil_fire_wall")
		While Not Me.endFire
			If Me.leftWall.transform.position.x >= -200F Then
				Exit While
			End If
			Me.leftWall.transform.position += Vector3.right * MyBase.properties.CurrentState.firewall.firewallSpeed * CupheadTime.FixedDelta
			If Me.rightWall.transform.position.x <= 200F Then
				Exit While
			End If
			Me.rightWall.transform.position += Vector3.left * MyBase.properties.CurrentState.firewall.firewallSpeed * CupheadTime.FixedDelta
			Yield New WaitForFixedUpdate()
		End While
		Return
	End Function

	' Token: 0x06001ADB RID: 6875 RVA: 0x000F59B3 File Offset: 0x000F3DB3
	Public Sub RemoveFire()
		MyBase.StartCoroutine(Me.remove_fire_cr())
	End Sub

	' Token: 0x06001ADC RID: 6876 RVA: 0x000F59C4 File Offset: 0x000F3DC4
	Private Iterator Function remove_fire_cr() As IEnumerator
		Me.endFire = True
		Dim t As Single = 0F
		Dim time As Single = 1F
		Dim startLeftPos As Single = Me.leftWall.transform.position.x
		Dim startRightPos As Single = Me.rightWall.transform.position.x
		While t < time
			t += CupheadTime.Delta
			Dim val As Single = EaseUtils.Ease(EaseUtils.EaseType.linear, 0F, 1F, t / time)
			Me.leftWall.transform.SetPosition(New Single?(Mathf.Lerp(startLeftPos, Me.leftWallPositionX, val)), Nothing, Nothing)
			Me.rightWall.transform.SetPosition(New Single?(Mathf.Lerp(startRightPos, Me.rightWallPositionX, val)), Nothing, Nothing)
			Yield Nothing
		End While
		Me.leftWall.transform.SetPosition(New Single?(Me.leftWallPositionX), Nothing, Nothing)
		Me.rightWall.transform.SetPosition(New Single?(Me.rightWallPositionX), Nothing, Nothing)
		AudioManager.FadeSFXVolume("devil_fire_wall", 0F, 1F)
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		AudioManager.[Stop]("devil_fire_wall")
		Return
	End Function

	' Token: 0x06001ADD RID: 6877 RVA: 0x000F59DF File Offset: 0x000F3DDF
	Private Sub TridentStartSFX()
		AudioManager.Play("devil_trident_start")
		Me.emitAudioFromObject.Add("devil_trident_start")
	End Sub

	' Token: 0x06001ADE RID: 6878 RVA: 0x000F59FB File Offset: 0x000F3DFB
	Private Sub TridentEndSFX()
		AudioManager.Play("devil_trident_end")
		Me.emitAudioFromObject.Add("devil_trident_end")
	End Sub

	' Token: 0x06001ADF RID: 6879 RVA: 0x000F5A17 File Offset: 0x000F3E17
	Private Sub TridentAttackSFX()
		AudioManager.Play("devil_trident_attack")
		Me.emitAudioFromObject.Add("devil_trident_attack")
	End Sub

	' Token: 0x06001AE0 RID: 6880 RVA: 0x000F5A33 File Offset: 0x000F3E33
	Private Sub SpiderMorphEndSFX()
		AudioManager.Play("devil_spider_morph_end")
		Me.emitAudioFromObject.Add("devil_spider_morph_end")
	End Sub

	' Token: 0x06001AE1 RID: 6881 RVA: 0x000F5A4F File Offset: 0x000F3E4F
	Private Sub DevilPhase1DeathSFX()
		AudioManager.Play("devil_phase_1_death_start")
		Me.emitAudioFromObject.Add("devil_phase_1_death_start")
	End Sub

	' Token: 0x06001AE2 RID: 6882 RVA: 0x000F5A6B File Offset: 0x000F3E6B
	Private Sub DragonMorphEndSFX()
		AudioManager.Play("devil_dragon_morph_end")
		Me.emitAudioFromObject.Add("devil_dragon_morph_end")
	End Sub

	' Token: 0x06001AE3 RID: 6883 RVA: 0x000F5A87 File Offset: 0x000F3E87
	Private Sub HandclapSnakeSFX()
		AudioManager.Play("devil_dragon_start")
		Me.emitAudioFromObject.Add("devil_dragon_start")
	End Sub

	' Token: 0x06001AE4 RID: 6884 RVA: 0x000F5AA3 File Offset: 0x000F3EA3
	Private Sub IntroPupilsSFX()
		AudioManager.Play("devil_intro_pupils")
		Me.emitAudioFromObject.Add("devil_intro_pupils")
	End Sub

	' Token: 0x06001AE5 RID: 6885 RVA: 0x000F5ABF File Offset: 0x000F3EBF
	Private Sub RamMorphStartSFX()
		AudioManager.Play("devil_ram_morph_start")
		Me.emitAudioFromObject.Add("devil_ram_morph_start")
	End Sub

	' Token: 0x06001AE6 RID: 6886 RVA: 0x000F5ADB File Offset: 0x000F3EDB
	Private Sub RamMorphEndSFX()
		AudioManager.Play("devil_ram_morph_end")
		Me.emitAudioFromObject.Add("devil_ram_morph_end")
	End Sub

	' Token: 0x06001AE7 RID: 6887 RVA: 0x000F5AF7 File Offset: 0x000F3EF7
	Private Sub StartTridentHeadSFX()
		AudioManager.Play("devil_trident_head")
		Me.emitAudioFromObject.Add("devil_trident_head")
	End Sub

	' Token: 0x06001AE8 RID: 6888 RVA: 0x000F5B13 File Offset: 0x000F3F13
	Private Sub EndTridentHeadSFX()
	End Sub

	' Token: 0x06001AE9 RID: 6889 RVA: 0x000F5B15 File Offset: 0x000F3F15
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		MyBase.properties.DealDamage(info.damage)
	End Sub

	' Token: 0x06001AEA RID: 6890 RVA: 0x000F5B28 File Offset: 0x000F3F28
	Public Sub ShowGoSign()
		Me.holeSign.SetActive(True)
	End Sub

	' Token: 0x040023F3 RID: 9203
	Public state As DevilLevelSittingDevil.State

	' Token: 0x040023F4 RID: 9204
	<SerializeField()>
	Private middleGround As GameObject

	' Token: 0x040023F5 RID: 9205
	<SerializeField()>
	Private giantHead As DevilLevelGiantHead

	' Token: 0x040023F6 RID: 9206
	<SerializeField()>
	Private demonPrefab As DevilLevelDemon

	' Token: 0x040023F7 RID: 9207
	<SerializeField()>
	Private leftDemonPeek As Transform

	' Token: 0x040023F8 RID: 9208
	<SerializeField()>
	Private leftDemonJumpRoot As Transform

	' Token: 0x040023F9 RID: 9209
	<SerializeField()>
	Private leftDemonRunRoot As Transform

	' Token: 0x040023FA RID: 9210
	<SerializeField()>
	Private leftDemonPillar As Transform

	' Token: 0x040023FB RID: 9211
	<SerializeField()>
	Private leftDemonFront As Transform

	' Token: 0x040023FC RID: 9212
	<SerializeField()>
	Private rightDemonPeek As Transform

	' Token: 0x040023FD RID: 9213
	<SerializeField()>
	Private rightDemonJumpRoot As Transform

	' Token: 0x040023FE RID: 9214
	<SerializeField()>
	Private rightDemonRunRoot As Transform

	' Token: 0x040023FF RID: 9215
	<SerializeField()>
	Private rightDemonPillar As Transform

	' Token: 0x04002400 RID: 9216
	<SerializeField()>
	Private rightDemonFront As Transform

	' Token: 0x04002401 RID: 9217
	<SerializeField()>
	Private arms As DevilLevelDevilArm()

	' Token: 0x04002402 RID: 9218
	<SerializeField()>
	Private spiderHead As DevilLevelSpiderHead

	' Token: 0x04002403 RID: 9219
	<SerializeField()>
	Private dragonHead As DevilLevelDragonHead

	' Token: 0x04002404 RID: 9220
	<SerializeField()>
	Private leftWall As Transform

	' Token: 0x04002405 RID: 9221
	Private leftWallPositionX As Single

	' Token: 0x04002406 RID: 9222
	<SerializeField()>
	Private rightWall As Transform

	' Token: 0x04002407 RID: 9223
	Private rightWallPositionX As Single

	' Token: 0x04002408 RID: 9224
	<SerializeField()>
	Private wheelProjectilePrefab As DevilLevelPitchforkWheelProjectile

	' Token: 0x04002409 RID: 9225
	<SerializeField()>
	Private wheelOrbitingProjectilePrefab As DevilLevelPitchforkOrbitingProjectile

	' Token: 0x0400240A RID: 9226
	<SerializeField()>
	Private jumpingProjectilePrefab As DevilLevelPitchforkJumpingProjectile

	' Token: 0x0400240B RID: 9227
	<SerializeField()>
	Private bouncingProjectilePrefab As DevilLevelPitchforkBouncingProjectile

	' Token: 0x0400240C RID: 9228
	<SerializeField()>
	Private spinnerProjectilePrefab As DevilLevelPitchforkSpinnerProjectile

	' Token: 0x0400240D RID: 9229
	<SerializeField()>
	Private spinnerOrbitingProjectilePrefab As DevilLevelPitchforkOrbitingProjectile

	' Token: 0x0400240E RID: 9230
	<SerializeField()>
	Private ringProjectilePrefab As DevilLevelPitchforkRingProjectile

	' Token: 0x0400240F RID: 9231
	<SerializeField()>
	Private holeSign As GameObject

	' Token: 0x04002410 RID: 9232
	Private dragonPos As Vector3

	' Token: 0x04002411 RID: 9233
	Private damageReceiver As DamageReceiver

	' Token: 0x04002412 RID: 9234
	Private isSpiderAttackNext As Boolean

	' Token: 0x04002413 RID: 9235
	Private endFire As Boolean

	' Token: 0x04002414 RID: 9236
	Private endPH1 As Boolean

	' Token: 0x04002415 RID: 9237
	Private spiderOffsetIndex As Integer

	' Token: 0x04002416 RID: 9238
	Private spiderOffsets As String()

	' Token: 0x04002417 RID: 9239
	Private pitchforkPatternIndex As Integer

	' Token: 0x04002418 RID: 9240
	Private pitchforkPattern As String()

	' Token: 0x04002419 RID: 9241
	Private pitchforkTwoFlameWheelSpawner As DevilLevelPitchforkProjectileSpawner

	' Token: 0x0400241A RID: 9242
	Private pitchforkThreeFlameJumperSpawner As DevilLevelPitchforkProjectileSpawner

	' Token: 0x0400241B RID: 9243
	Private pitchforkFourFlameBouncerSpawner As DevilLevelPitchforkProjectileSpawner

	' Token: 0x0400241C RID: 9244
	Private pitchforkFiveFlameSpinnerSpawner As DevilLevelPitchforkProjectileSpawner

	' Token: 0x0400241D RID: 9245
	Private pitchforkSixFlameRingSpawner As DevilLevelPitchforkProjectileSpawner

	' Token: 0x0400241E RID: 9246
	Public OnPhase1Death As Action

	' Token: 0x02000580 RID: 1408
	Public Enum State
		' Token: 0x04002420 RID: 9248
		Intro
		' Token: 0x04002421 RID: 9249
		Idle
		' Token: 0x04002422 RID: 9250
		Clap
		' Token: 0x04002423 RID: 9251
		Head
		' Token: 0x04002424 RID: 9252
		Pitchfork
		' Token: 0x04002425 RID: 9253
		EndPhase1
	End Enum
End Class
