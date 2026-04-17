Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020006B8 RID: 1720
Public Class FrogsLevelShort
	Inherits LevelProperties.Frogs.Entity

	' Token: 0x170003B2 RID: 946
	' (get) Token: 0x06002477 RID: 9335 RVA: 0x00155C70 File Offset: 0x00154070
	' (set) Token: 0x06002478 RID: 9336 RVA: 0x00155C78 File Offset: 0x00154078
	Public Property state As FrogsLevelShort.State

	' Token: 0x170003B3 RID: 947
	' (get) Token: 0x06002479 RID: 9337 RVA: 0x00155C81 File Offset: 0x00154081
	' (set) Token: 0x0600247A RID: 9338 RVA: 0x00155C89 File Offset: 0x00154089
	Public Property direction As FrogsLevelShort.Direction

	' Token: 0x0600247B RID: 9339 RVA: 0x00155C94 File Offset: 0x00154094
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.spriteRenderer = MyBase.GetComponent(Of SpriteRenderer)()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		Me.damageDealer = New DamageDealer(1F, 0.3F, DamageDealer.DamageSource.Enemy, True, False, False)
	End Sub

	' Token: 0x0600247C RID: 9340 RVA: 0x00155CEF File Offset: 0x001540EF
	Private Sub Start()
		AddHandler Level.Current.OnIntroEvent, AddressOf Me.OnLevelIntro
	End Sub

	' Token: 0x0600247D RID: 9341 RVA: 0x00155D07 File Offset: 0x00154107
	Private Sub Update()
		Me.damageDealer.Update()
	End Sub

	' Token: 0x0600247E RID: 9342 RVA: 0x00155D14 File Offset: 0x00154114
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x0600247F RID: 9343 RVA: 0x00155D32 File Offset: 0x00154132
	Public Overrides Sub LevelInit(properties As LevelProperties.Frogs)
		MyBase.LevelInit(properties)
		AddHandler properties.OnBossDeath, AddressOf Me.OnBossDeath
	End Sub

	' Token: 0x06002480 RID: 9344 RVA: 0x00155D4D File Offset: 0x0015414D
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		If FrogsLevel.FINAL_FORM Then
			Return
		End If
		MyBase.properties.DealDamage(info.damage)
	End Sub

	' Token: 0x06002481 RID: 9345 RVA: 0x00155D6C File Offset: 0x0015416C
	Private Sub OnBossDeath()
		AudioManager.Play("level_frogs_short_death")
		Me.emitAudioFromObject.Add("level_frogs_short_death")
		AudioManager.PlayLoop("level_frogs_short_death_loop")
		Me.emitAudioFromObject.Add("level_frogs_short_death_loop")
		Me.StopAllCoroutines()
		MyBase.animator.SetTrigger("OnDeath")
	End Sub

	' Token: 0x06002482 RID: 9346 RVA: 0x00155DC3 File Offset: 0x001541C3
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.introDust = Nothing
		Me.rageFireball = Nothing
		Me.rageFireballSpark = Nothing
		Me.clapBullet = Nothing
		Me.clapEffect = Nothing
	End Sub

	' Token: 0x06002483 RID: 9347 RVA: 0x00155DEE File Offset: 0x001541EE
	Private Sub SfxClap()
		AudioManager.Play("level_frogs_short_clap_shock")
		Me.emitAudioFromObject.Add("level_frogs_short_clap_shock")
	End Sub

	' Token: 0x06002484 RID: 9348 RVA: 0x00155E0A File Offset: 0x0015420A
	Private Sub SfxEndIntro()
		AudioManager.[Stop]("level_frogs_short_intro_loop")
		AudioManager.Play("level_frogs_short_intro_start")
		Me.emitAudioFromObject.Add("level_frogs_short_intro_start")
	End Sub

	' Token: 0x06002485 RID: 9349 RVA: 0x00155E30 File Offset: 0x00154230
	Private Sub OnLevelIntro()
		MyBase.StartCoroutine(Me.intro_cr())
	End Sub

	' Token: 0x06002486 RID: 9350 RVA: 0x00155E40 File Offset: 0x00154240
	Private Iterator Function intro_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		AudioManager.PlayLoop("level_frogs_short_intro_loop")
		Me.emitAudioFromObject.Add("level_frogs_short_intro_loop")
		MyBase.animator.Play("Intro")
		Return
	End Function

	' Token: 0x06002487 RID: 9351 RVA: 0x00155E5B File Offset: 0x0015425B
	Private Sub PlayIntroEffect()
		Me.introDust.Create(MyBase.transform.position)
	End Sub

	' Token: 0x06002488 RID: 9352 RVA: 0x00155E74 File Offset: 0x00154274
	Public Sub StartRage()
		If Me.state <> FrogsLevelShort.State.Idle AndAlso Me.state <> FrogsLevelShort.State.Complete Then
			Return
		End If
		Me.state = FrogsLevelShort.State.Rage
		MyBase.StartCoroutine(Me.rage_cr())
	End Sub

	' Token: 0x06002489 RID: 9353 RVA: 0x00155EA8 File Offset: 0x001542A8
	Private Sub Shoot(properties As LevelProperties.Frogs.ShortRage, pos As Vector3, parry As Boolean)
		Dim num As Integer = If((Me.direction <> FrogsLevelShort.Direction.Left), 1, (-1))
		AudioManager.Play("level_frogs_short_fireball")
		Me.emitAudioFromObject.Add("level_frogs_short_fireball")
		Me.rageFireballSpark.Create(pos, New Vector3(CSng(num), CSng(num), 1F))
		Dim basicProjectile As BasicProjectile = Me.rageFireball.Create(pos, 0F, New Vector2(CSng(num), CSng(num)), properties.shotSpeed * CSng(num))
		basicProjectile.SetParryable(parry)
		basicProjectile.CollisionDeath.OnlyPlayer()
	End Sub

	' Token: 0x0600248A RID: 9354 RVA: 0x00155F38 File Offset: 0x00154338
	Private Iterator Function rage_cr() As IEnumerator
		Dim p As LevelProperties.Frogs.ShortRage = MyBase.properties.CurrentState.shortRage
		MyBase.animator.SetTrigger("OnRage")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Rage", False, True)
		Yield CupheadTime.WaitForSeconds(Me, p.anticipationDelay)
		MyBase.animator.SetTrigger("OnRageAttack")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Rage_Anticipate_End", False, True)
		AudioManager.PlayLoop("level_frogs_short_ragefist_attack_loop")
		Me.emitAudioFromObject.Add("level_frogs_short_ragefist_attack_loop")
		Dim shotCount As Integer = p.shotCount
		Dim root As Integer = 0
		Dim parryString As String = p.parryPatterns(Global.UnityEngine.Random.Range(0, p.parryPatterns.Length)).ToLower()
		Dim parryIndex As Integer = 0
		While shotCount > 0
			Yield CupheadTime.WaitForSeconds(Me, p.shotDelay)
			shotCount -= 1
			Me.Shoot(p, Me.rageRoots(root).position, parryString(parryIndex) = "p"c)
			root = CInt(Mathf.Repeat(CSng((root + 1)), CSng(Me.rageRoots.Length)))
			parryIndex = CInt(Mathf.Repeat(CSng((parryIndex + 1)), CSng(parryString.Length)))
		End While
		Yield CupheadTime.WaitForSeconds(Me, p.shotDelay)
		MyBase.animator.SetTrigger("OnRageEnd")
		AudioManager.[Stop]("level_frogs_short_ragefist_attack_loop")
		Yield CupheadTime.WaitForSeconds(Me, p.hesitate)
		Me.state = FrogsLevelShort.State.Complete
		Return
	End Function

	' Token: 0x0600248B RID: 9355 RVA: 0x00155F53 File Offset: 0x00154353
	Public Sub StartClap()
		If Me.state <> FrogsLevelShort.State.Idle AndAlso Me.state <> FrogsLevelShort.State.Complete Then
			Return
		End If
		Me.state = FrogsLevelShort.State.Clap
		MyBase.StartCoroutine(Me.clap_cr())
	End Sub

	' Token: 0x0600248C RID: 9356 RVA: 0x00155F88 File Offset: 0x00154388
	Private Sub ShootClap()
		Me.SfxClap()
		Me.clapEffect.Create(Me.clapRoot.position)
		Me.clapBullet.Create(Me.direction, Me.clapDirection, Me.clapRoot.position, Me.clapRoot.right * Me.clapProperties.bulletSpeed)
	End Sub

	' Token: 0x0600248D RID: 9357 RVA: 0x00155FFC File Offset: 0x001543FC
	Private Iterator Function clap_cr() As IEnumerator
		Me.clapProperties = MyBase.properties.CurrentState.shortClap
		Me.clapDirection = If((Not Rand.Bool()), FrogsLevelShortClapBullet.Direction.Up, FrogsLevelShortClapBullet.Direction.Down)
		Me.clapRoot.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(Me.clapProperties.angles.GetRandom()))
		Dim patternString As String = Me.clapProperties.patterns(Global.UnityEngine.Random.Range(0, Me.clapProperties.patterns.Length))
		Dim pattern As KeyValue() = KeyValue.ListFromString(patternString, New Char() { "S"c, "D"c })
		MyBase.animator.SetTrigger("OnClap")
		MyBase.animator.SetBool("Clapping", True)
		Yield CupheadTime.WaitForSeconds(Me, 1F + Me.clapProperties.shotDelay)
		For i As Integer = 0 To pattern.Length - 1
			If pattern(i).key = "S" Then
				Dim ii As Integer = 0
				While CSng(ii) < pattern(i).value
					Me.clapDirection = If((Me.clapDirection <> FrogsLevelShortClapBullet.Direction.Down), FrogsLevelShortClapBullet.Direction.Down, FrogsLevelShortClapBullet.Direction.Up)
					If i >= pattern.Length - 1 AndAlso CSng(ii) >= pattern(i).value - 1F Then
						MyBase.animator.Play("Clap_End")
						Yield MyBase.animator.WaitForAnimationToEnd(Me, "Clap_End", False, True)
					Else
						MyBase.animator.Play("Clap_Shoot")
					End If
					Yield CupheadTime.WaitForSeconds(Me, 0.5F)
					ii += 1
				End While
			Else
				Yield CupheadTime.WaitForSeconds(Me, pattern(i).value)
			End If
		Next
		MyBase.animator.Play("Idle")
		MyBase.animator.ResetTrigger("OnClap")
		MyBase.animator.SetBool("Clapping", False)
		Yield CupheadTime.WaitForSeconds(Me, Me.clapProperties.hesitate)
		Me.state = FrogsLevelShort.State.Complete
		Return
	End Function

	' Token: 0x0600248E RID: 9358 RVA: 0x00156017 File Offset: 0x00154417
	Public Sub StartRoll()
		Me.StopAllCoroutines()
		MyBase.animator.Play("Idle")
		Me.state = FrogsLevelShort.State.Roll
		MyBase.StartCoroutine(Me.roll_cr())
	End Sub

	' Token: 0x0600248F RID: 9359 RVA: 0x00156043 File Offset: 0x00154443
	Private Function CheckRollable() As Boolean
		Return Me.state = FrogsLevelShort.State.Complete OrElse Me.state = FrogsLevelShort.State.Idle
	End Function

	' Token: 0x06002490 RID: 9360 RVA: 0x00156064 File Offset: 0x00154464
	Private Iterator Function roll_cr() As IEnumerator
		Yield Nothing
		Dim startX As Single = MyBase.transform.position.x
		Dim endX As Single = -(startX + 240F)
		Dim p As LevelProperties.Frogs.ShortRoll = MyBase.properties.CurrentState.shortRoll
		MyBase.animator.SetTrigger("OnRoll")
		Yield CupheadTime.WaitForSeconds(Me, 1.2F + p.delay)
		MyBase.animator.SetTrigger("OnRollContinue")
		CupheadLevelCamera.Current.StartShake(4F)
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		Yield MyBase.animator.WaitForAnimationToStart(Me, "Roll_Loop", False)
		Dim t As Single = 0F
		While t < p.time
			Dim val As Single = t / p.time
			Dim x As Single = EaseUtils.Ease(EaseUtils.EaseType.easeInSine, startX, endX, val)
			MyBase.transform.SetPosition(New Single?(x), Nothing, Nothing)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		MyBase.transform.SetPosition(New Single?(endX), Nothing, Nothing)
		Yield Nothing
		CupheadLevelCamera.Current.EndShake(0.5F)
		AudioManager.[Stop]("level_frogs_short_rolling_loop")
		AudioManager.Play("level_frogs_short_rolling_crash")
		Me.emitAudioFromObject.Add("level_frogs_short_rolling_crash")
		Me.spriteRenderer.enabled = False
		Me.direction = FrogsLevelShort.Direction.Right
		Yield CupheadTime.WaitForSeconds(Me, p.returnDelay)
		MyBase.transform.SetScale(New Single?(-1F), Nothing, Nothing)
		MyBase.transform.SetPosition(New Single?(-(startX + 140F)), Nothing, Nothing)
		MyBase.animator.SetTrigger("OnRollContinue")
		AudioManager.Play("level_frogs_short_rolling_end")
		Me.emitAudioFromObject.Add("level_frogs_short_rolling_end")
		Me.spriteRenderer.enabled = True
		Yield CupheadTime.WaitForSeconds(Me, 1F + p.hesitate)
		Me.state = FrogsLevelShort.State.Complete
		AudioManager.[Stop]("level_frogs_short_rolling_loop")
		Return
	End Function

	' Token: 0x06002491 RID: 9361 RVA: 0x0015607F File Offset: 0x0015447F
	Public Sub PlayRollSfx()
		AudioManager.PlayLoop("level_frogs_short_rolling_loop")
		Me.emitAudioFromObject.Add("level_frogs_short_rolling_loop")
		AudioManager.Play("level_frogs_short_rolling_start")
		Me.emitAudioFromObject.Add("level_frogs_short_rolling_start")
	End Sub

	' Token: 0x06002492 RID: 9362 RVA: 0x001560B5 File Offset: 0x001544B5
	Public Sub StartMorph()
		Me.StopAllCoroutines()
		MyBase.animator.Play("Idle")
		Me.state = FrogsLevelShort.State.Morphing
		MyBase.StartCoroutine(Me.morphRoll_cr())
	End Sub

	' Token: 0x06002493 RID: 9363 RVA: 0x001560E4 File Offset: 0x001544E4
	Private Iterator Function morphRoll_cr() As IEnumerator
		Dim start As Vector2 = MyBase.transform.position
		Dim [end] As Vector2 = FrogsLevelTall.Current.shortMorphRoot.position
		MyBase.animator.SetTrigger("OnRoll")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Roll", False, True)
		Yield CupheadTime.WaitForSeconds(Me, 1.5F)
		MyBase.animator.SetTrigger("OnRollContinue")
		CupheadLevelCamera.Current.StartShake(4F)
		AudioManager.PlayLoop("level_frogs_short_rolling_loop")
		Me.emitAudioFromObject.Add("level_frogs_short_rolling_loop")
		AudioManager.Play("level_frogs_short_rolling_start")
		Me.emitAudioFromObject.Add("level_frogs_short_rolling_start")
		Dim t As Single = 0F
		While t < 1F
			Dim val As Single = t / 1F
			Dim x As Single = EaseUtils.Ease(EaseUtils.EaseType.linear, start.x, [end].x, val)
			Dim y As Single = EaseUtils.Ease(EaseUtils.EaseType.linear, start.y, [end].y, val)
			MyBase.transform.SetPosition(New Single?(x), New Single?(y), Nothing)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		AudioManager.[Stop]("level_frogs_short_rolling_loop")
		MyBase.transform.SetPosition(New Single?([end].x), New Single?([end].y), Nothing)
		CupheadLevelCamera.Current.EndShake(0.5F)
		Yield Nothing
		FrogsLevelTall.Current.ContinueMorph()
		Me.spriteRenderer.enabled = False
		RemoveHandler MyBase.properties.OnBossDeath, AddressOf Me.OnBossDeath
		MyBase.gameObject.SetActive(False)
		Return
	End Function

	' Token: 0x04002D26 RID: 11558
	<SerializeField()>
	Private introDust As Effect

	' Token: 0x04002D27 RID: 11559
	<SerializeField()>
	Private rageRoots As Transform()

	' Token: 0x04002D28 RID: 11560
	<SerializeField()>
	Private rageFireball As FrogsLevelShortRageBullet

	' Token: 0x04002D29 RID: 11561
	<SerializeField()>
	Private rageFireballSpark As Effect

	' Token: 0x04002D2A RID: 11562
	<SerializeField()>
	Private clapBullet As FrogsLevelShortClapBullet

	' Token: 0x04002D2B RID: 11563
	<SerializeField()>
	Private clapEffect As Effect

	' Token: 0x04002D2C RID: 11564
	<SerializeField()>
	Private clapRoot As Transform

	' Token: 0x04002D2F RID: 11567
	Private spriteRenderer As SpriteRenderer

	' Token: 0x04002D30 RID: 11568
	Private damageReceiver As DamageReceiver

	' Token: 0x04002D31 RID: 11569
	Private damageDealer As DamageDealer

	' Token: 0x04002D32 RID: 11570
	Private clapProperties As LevelProperties.Frogs.ShortClap

	' Token: 0x04002D33 RID: 11571
	Private clapDirection As FrogsLevelShortClapBullet.Direction

	' Token: 0x04002D34 RID: 11572
	Private Const MORPH_ROLL_TIME As Single = 1F

	' Token: 0x020006B9 RID: 1721
	Public Enum State
		' Token: 0x04002D36 RID: 11574
		Idle
		' Token: 0x04002D37 RID: 11575
		Rage
		' Token: 0x04002D38 RID: 11576
		Roll
		' Token: 0x04002D39 RID: 11577
		Clap
		' Token: 0x04002D3A RID: 11578
		Morphing
		' Token: 0x04002D3B RID: 11579
		Complete = 1000
		' Token: 0x04002D3C RID: 11580
		Morphed
	End Enum

	' Token: 0x020006BA RID: 1722
	Public Enum Direction
		' Token: 0x04002D3E RID: 11582
		Left
		' Token: 0x04002D3F RID: 11583
		Right
	End Enum
End Class
