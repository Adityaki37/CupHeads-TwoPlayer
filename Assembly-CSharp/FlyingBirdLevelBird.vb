Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000616 RID: 1558
Public Class FlyingBirdLevelBird
	Inherits LevelProperties.FlyingBird.Entity

	' Token: 0x1700037A RID: 890
	' (get) Token: 0x06001F7E RID: 8062 RVA: 0x001210BC File Offset: 0x0011F4BC
	' (set) Token: 0x06001F7F RID: 8063 RVA: 0x001210C4 File Offset: 0x0011F4C4
	Public Property state As FlyingBirdLevelBird.State

	' Token: 0x1700037B RID: 891
	' (get) Token: 0x06001F80 RID: 8064 RVA: 0x001210CD File Offset: 0x0011F4CD
	' (set) Token: 0x06001F81 RID: 8065 RVA: 0x001210D5 File Offset: 0x0011F4D5
	Public Property floating As Boolean

	' Token: 0x06001F82 RID: 8066 RVA: 0x001210E0 File Offset: 0x0011F4E0
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.state = FlyingBirdLevelBird.State.Init
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		AddHandler Me.heart.GetComponent(Of CollisionChild)().OnPlayerCollision, AddressOf Me.OnCollisionPlayer
		AddHandler Me.heart.GetComponent(Of DamageReceiver)().OnDamageTaken, AddressOf Me.OnDamageTaken
		Me.houseCollider = New GameObject("House Collider").transform
		Dim boxCollider2D As BoxCollider2D = Me.houseCollider.gameObject.AddComponent(Of BoxCollider2D)()
		boxCollider2D.isTrigger = True
		boxCollider2D.offset = New Vector2(60F, -50F)
		boxCollider2D.size = New Vector2(400F, 300F)
		Dim collisionChild As CollisionChild = Me.houseCollider.gameObject.AddComponent(Of CollisionChild)()
		AddHandler collisionChild.OnPlayerCollision, AddressOf Me.OnCollisionPlayer
	End Sub

	' Token: 0x06001F83 RID: 8067 RVA: 0x001211DC File Offset: 0x0011F5DC
	Private Sub Start()
		Me.featherPrefab.CreatePool(150)
		Me.heart.gameObject.SetActive(False)
		Me.feathersFirstTime = True
	End Sub

	' Token: 0x06001F84 RID: 8068 RVA: 0x00121206 File Offset: 0x0011F606
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		If Me.damageReceiver IsNot Nothing Then
			RemoveHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		End If
		Me.featherPrefab = Nothing
	End Sub

	' Token: 0x06001F85 RID: 8069 RVA: 0x00121240 File Offset: 0x0011F640
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		If Me.state = FlyingBirdLevelBird.State.Dead OrElse Me.state = FlyingBirdLevelBird.State.Dying Then
			Return
		End If
		MyBase.properties.DealDamage(info.damage)
		If MyBase.properties.CurrentHealth <= 0F Then
			Me.BirdDie()
		End If
	End Sub

	' Token: 0x06001F86 RID: 8070 RVA: 0x00121292 File Offset: 0x0011F692
	Private Sub Update()
		If Me.houseCollider IsNot Nothing Then
			Me.houseCollider.position = MyBase.transform.position
		End If
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06001F87 RID: 8071 RVA: 0x001212D1 File Offset: 0x0011F6D1
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If Me.damageDealer IsNot Nothing AndAlso phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06001F88 RID: 8072 RVA: 0x001212FC File Offset: 0x0011F6FC
	Public Overrides Sub LevelInit(properties As LevelProperties.FlyingBird)
		MyBase.LevelInit(properties)
		AddHandler properties.OnStateChange, AddressOf Me.OnStateChange
		Me.floating = False
		MyBase.StartCoroutine(Me.float_cr())
		Me.garbageIndex = Global.UnityEngine.Random.Range(0, properties.CurrentState.garbage.garbageTypeString.Length)
	End Sub

	' Token: 0x06001F89 RID: 8073 RVA: 0x00121354 File Offset: 0x0011F754
	Private Sub OnStateChange()
		If MyBase.properties.CurrentState.stateName = LevelProperties.FlyingBird.States.Whistle Then
			MyBase.StartCoroutine(Me.whistle_cr())
			If Me.patternCoroutine IsNot Nothing Then
				MyBase.StopCoroutine(Me.patternCoroutine)
			End If
			Me.patternCoroutine = MyBase.StartCoroutine(Me.whistle_cr())
		End If
	End Sub

	' Token: 0x06001F8A RID: 8074 RVA: 0x001213B0 File Offset: 0x0011F7B0
	Public Sub IntroContinue()
		Dim component As Animator = MyBase.GetComponent(Of Animator)()
		component.SetTrigger("Continue")
		MyBase.StartCoroutine(Me.intro_cr())
	End Sub

	' Token: 0x06001F8B RID: 8075 RVA: 0x001213DC File Offset: 0x0011F7DC
	Private Sub SfxIntroA()
		AudioManager.Play("level_flying_bird_intro_a")
	End Sub

	' Token: 0x06001F8C RID: 8076 RVA: 0x001213E8 File Offset: 0x0011F7E8
	Private Sub SfxIntroB()
		AudioManager.Play("level_flying_bird_intro_b")
	End Sub

	' Token: 0x06001F8D RID: 8077 RVA: 0x001213F4 File Offset: 0x0011F7F4
	Private Sub OnIntroAnimComplete()
		Me.introEnded = True
	End Sub

	' Token: 0x06001F8E RID: 8078 RVA: 0x00121400 File Offset: 0x0011F800
	Private Iterator Function intro_cr() As IEnumerator
		While Not Me.introEnded
			Yield Nothing
		End While
		Me.floating = True
		Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.floating.attackInitialDelayRange.RandomFloat())
		Me.state = FlyingBirdLevelBird.State.Idle
		Return
	End Function

	' Token: 0x06001F8F RID: 8079 RVA: 0x0012141B File Offset: 0x0011F81B
	Private Sub IdleNoBlink()
		Me.blinks += 1
		If Me.blinks >= Me.maxBlinks Then
			MyBase.animator.SetBool("Blink", True)
		End If
	End Sub

	' Token: 0x06001F90 RID: 8080 RVA: 0x0012144D File Offset: 0x0011F84D
	Private Sub Blink()
		Me.blinks = 0
		Me.maxBlinks = Global.UnityEngine.Random.Range(2, 5)
		MyBase.animator.SetBool("Blink", False)
	End Sub

	' Token: 0x06001F91 RID: 8081 RVA: 0x00121474 File Offset: 0x0011F874
	Private Iterator Function whistle_cr() As IEnumerator
		Me.state = FlyingBirdLevelBird.State.Whistle
		Me.floating = False
		Dim animator As Animator = MyBase.GetComponent(Of Animator)()
		animator.Play("Whistle")
		AudioManager.Play("level_flying_bird_whistle")
		Yield animator.WaitForAnimationToEnd(Me, "Whistle", False, True)
		Me.state = FlyingBirdLevelBird.State.Idle
		Me.floating = True
		Return
	End Function

	' Token: 0x06001F92 RID: 8082 RVA: 0x00121490 File Offset: 0x0011F890
	Private Iterator Function float_cr() As IEnumerator
		Dim goUp As Boolean = Rand.Bool()
		While True
			If goUp Then
				Yield MyBase.StartCoroutine(Me.floatTo_cr(MyBase.properties.CurrentState.floating.top, MyBase.properties.CurrentState.floating.time))
			Else
				Yield MyBase.StartCoroutine(Me.floatTo_cr(MyBase.properties.CurrentState.floating.bottom, MyBase.properties.CurrentState.floating.time))
			End If
			goUp = Not goUp
		End While
		Return
	End Function

	' Token: 0x06001F93 RID: 8083 RVA: 0x001214AC File Offset: 0x0011F8AC
	Private Iterator Function floatTo_cr([end] As Single, time As Single) As IEnumerator
		Dim t As Single = 0F
		Dim start As Single = MyBase.transform.position.y
		While t < time
			If Not Me.floating Then
				While Not Me.floating
					Yield Nothing
				End While
			End If
			Dim val As Single = t / time
			MyBase.transform.SetPosition(Nothing, New Single?(EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, start, [end], val)), Nothing)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		MyBase.transform.SetPosition(Nothing, New Single?([end]), Nothing)
		Return
	End Function

	' Token: 0x06001F94 RID: 8084 RVA: 0x001214D5 File Offset: 0x0011F8D5
	Public Sub StartFeathers()
		If Me.patternCoroutine IsNot Nothing Then
			MyBase.StopCoroutine(Me.patternCoroutine)
		End If
		Me.patternCoroutine = MyBase.StartCoroutine(Me.feathers_cr())
	End Sub

	' Token: 0x06001F95 RID: 8085 RVA: 0x00121500 File Offset: 0x0011F900
	Private Sub FireFeathers(count As Integer, offset As Single, parryable As Boolean)
		parryable = False
		For i As Integer = 0 To count - 1
			Dim num As Single = 360F * (CSng(i) / CSng(count))
			Me.featherPrefab.Spawn(MyBase.transform.position, Quaternion.Euler(New Vector3(0F, 0F, offset + num - 180F))).Init(MyBase.properties.CurrentState.feathers.speed).SetParryable(parryable)
		Next
	End Sub

	' Token: 0x06001F96 RID: 8086 RVA: 0x00121584 File Offset: 0x0011F984
	Private Iterator Function feathers_cr() As IEnumerator
		Me.state = FlyingBirdLevelBird.State.Feathers
		Me.floating = False
		Dim animator As Animator = MyBase.GetComponent(Of Animator)()
		animator.Play("Feathers_Start")
		AudioManager.Play("level_flyingbird_feathers_start")
		Me.emitAudioFromObject.Add("level_flyingbird_feathers_start")
		Yield animator.WaitForAnimationToEnd(Me, "Feathers_Start", False, True)
		Dim featherProperties As LevelProperties.FlyingBird.Feathers = MyBase.properties.CurrentState.feathers
		Dim pattern As KeyValue() = KeyValue.ListFromString(featherProperties.pattern(Global.UnityEngine.Random.Range(0, featherProperties.pattern.Length)), New Char() { "P"c, "D"c })
		AudioManager.PlayLoop("level_flyingbird_feathers_loop")
		Me.emitAudioFromObject.Add("level_flyingbird_feathers_loop")
		For i As Integer = 0 To pattern.Length - 1
			Dim offset As Single = 0F
			Dim parryable As Boolean = False
			If pattern(i).key = "P" Then
				Dim p As Integer = 0
				While CSng(p) < pattern(i).value
					Me.FireFeathers(featherProperties.count, offset, parryable)
					parryable = Not parryable
					offset += featherProperties.offset
					Yield CupheadTime.WaitForSeconds(Me, If(Me.feathersFirstTime, featherProperties.initalShotDelay, featherProperties.shotDelay))
					Me.feathersFirstTime = False
					p += 1
				End While
			Else
				Yield CupheadTime.WaitForSeconds(Me, pattern(i).value)
			End If
			Yield Nothing
		Next
		AudioManager.[Stop]("level_flyingbird_feathers_loop")
		Me.floating = True
		animator.Play("Feathers_End")
		AudioManager.Play("level_flyingbird_feathers_hesitate")
		Me.emitAudioFromObject.Add("level_flyingbird_feathers_hesitate")
		Yield CupheadTime.WaitForSeconds(Me, featherProperties.hesitate)
		animator.Play("Feathers_Hesitate_End")
		AudioManager.[Stop]("level_flyingbird_feathers_hesitate")
		Yield animator.WaitForAnimationToEnd(Me, "Feathers_Hesitate_End", False, True)
		Me.state = FlyingBirdLevelBird.State.Idle
		Return
	End Function

	' Token: 0x06001F97 RID: 8087 RVA: 0x0012159F File Offset: 0x0011F99F
	Public Sub StartEggs()
		If Me.patternCoroutine IsNot Nothing Then
			MyBase.StopCoroutine(Me.patternCoroutine)
		End If
		Me.patternCoroutine = MyBase.StartCoroutine(Me.eggs_cr())
	End Sub

	' Token: 0x06001F98 RID: 8088 RVA: 0x001215CA File Offset: 0x0011F9CA
	Private Sub FireEgg()
		Me.eggPrefab.Create(MyBase.properties.CurrentState.feathers.speed, Me.eggRoot.position)
	End Sub

	' Token: 0x06001F99 RID: 8089 RVA: 0x001215FD File Offset: 0x0011F9FD
	Private Sub SoundFireEggThroaty()
		AudioManager.Play("level_flying_bird_spit_throaty")
		Me.emitAudioFromObject.Add("level_flying_bird_spit_throaty")
	End Sub

	' Token: 0x06001F9A RID: 8090 RVA: 0x00121619 File Offset: 0x0011FA19
	Private Sub SoundFireEggProjectile()
		AudioManager.Play("level_flying_bird_spit")
		Me.emitAudioFromObject.Add("level_flying_bird_spit")
	End Sub

	' Token: 0x06001F9B RID: 8091 RVA: 0x00121638 File Offset: 0x0011FA38
	Private Iterator Function eggs_cr() As IEnumerator
		Me.floating = True
		Me.state = FlyingBirdLevelBird.State.Eggs
		Dim animator As Animator = MyBase.GetComponent(Of Animator)()
		Dim eggProperties As LevelProperties.FlyingBird.Eggs = MyBase.properties.CurrentState.eggs
		Dim pattern As KeyValue() = KeyValue.ListFromString(eggProperties.pattern(Global.UnityEngine.Random.Range(0, eggProperties.pattern.Length)), New Char() { "P"c, "D"c })
		For i As Integer = 0 To pattern.Length - 1
			If pattern(i).key = "P" Then
				Dim p As Integer = 0
				While CSng(p) < pattern(i).value
					Yield CupheadTime.WaitForSeconds(Me, eggProperties.shotDelay)
					animator.Play("Spit")
					p += 1
				End While
			Else
				Yield CupheadTime.WaitForSeconds(Me, pattern(i).value)
			End If
			Yield Nothing
		Next
		Yield animator.WaitForAnimationToEnd(Me, "Spit", False, True)
		Yield CupheadTime.WaitForSeconds(Me, eggProperties.hesitate)
		Me.state = FlyingBirdLevelBird.State.Idle
		Return
	End Function

	' Token: 0x06001F9C RID: 8092 RVA: 0x00121653 File Offset: 0x0011FA53
	Public Sub StartLasers()
		If Me.patternCoroutine IsNot Nothing Then
			MyBase.StopCoroutine(Me.patternCoroutine)
		End If
		Me.patternCoroutine = MyBase.StartCoroutine(Me.lasers_cr())
	End Sub

	' Token: 0x06001F9D RID: 8093 RVA: 0x00121680 File Offset: 0x0011FA80
	Private Sub FireLasers()
		AudioManager.Play("level_flyingbird_bird_laser_fire")
		Me.emitAudioFromObject.Add("level_flyingbird_bird_laser_fire")
		Me.laserEffect.Create(Me.laserRoots(0).position)
		For Each transform As Transform In Me.laserRoots
			Me.laserPrefab.Create(transform.position, -transform.eulerAngles.z, -MyBase.properties.CurrentState.lasers.speed)
		Next
	End Sub

	' Token: 0x06001F9E RID: 8094 RVA: 0x0012171B File Offset: 0x0011FB1B
	Private Sub LasersAnimEnded()
		Me.state = FlyingBirdLevelBird.State.LasersEnding
	End Sub

	' Token: 0x06001F9F RID: 8095 RVA: 0x00121724 File Offset: 0x0011FB24
	Private Iterator Function lasers_cr() As IEnumerator
		Dim animator As Animator = MyBase.GetComponent(Of Animator)()
		Me.state = FlyingBirdLevelBird.State.Lasers
		Me.floating = False
		Dim properties As LevelProperties.FlyingBird.Lasers = MyBase.properties.CurrentState.lasers
		animator.SetTrigger("StartLasers")
		While Me.state = FlyingBirdLevelBird.State.Lasers
			Yield Nothing
		End While
		Me.floating = True
		Yield CupheadTime.WaitForSeconds(Me, properties.hesitate)
		Me.state = FlyingBirdLevelBird.State.Idle
		Return
	End Function

	' Token: 0x06001FA0 RID: 8096 RVA: 0x0012173F File Offset: 0x0011FB3F
	Private Sub LasersSFX()
		AudioManager.Play("level_flyingbird_bird_lasers")
		Me.emitAudioFromObject.Add("level_flyingbird_bird_lasers")
	End Sub

	' Token: 0x06001FA1 RID: 8097 RVA: 0x0012175C File Offset: 0x0011FB5C
	Public Sub BirdFall()
		Me.state = FlyingBirdLevelBird.State.Dying
		Me.houseCollider.gameObject.SetActive(False)
		AudioManager.[Stop]("level_flyingbird_feathers_loop")
		MyBase.GetComponent(Of LevelBossDeathExploder)().StartExplosion()
		MyBase.GetComponent(Of CircleCollider2D)().enabled = False
		Me.StopAllCoroutines()
		MyBase.animator.Play("Death")
		MyBase.StartCoroutine(Me.die_cr())
		Me.nurses.Die()
	End Sub

	' Token: 0x06001FA2 RID: 8098 RVA: 0x001217D0 File Offset: 0x0011FBD0
	Public Sub BirdDie()
		Me.nurses.Die()
		Me.nurses.nurses(0).gameObject.SetActive(False)
		Me.nurses.nurses(1).gameObject.SetActive(False)
		MyBase.GetComponent(Of Collider2D)().enabled = False
		MyBase.StopCoroutine(Me.checkHeart_cr())
		MyBase.StopCoroutine(Me.garbage_cr())
		Me.nurses.animator.SetTrigger("Die")
		MyBase.animator.Play("Stretcher_Death")
		AudioManager.PlayLoop("level_flyingbird_stretcher_death")
		Me.emitAudioFromObject.Add("level_flyingbird_stretcher_death")
		For Each boxCollider2D As BoxCollider2D In MyBase.GetComponentsInChildren(Of BoxCollider2D)()
			boxCollider2D.enabled = False
		Next
		For Each circleCollider2D As CircleCollider2D In MyBase.GetComponentsInChildren(Of CircleCollider2D)()
			circleCollider2D.enabled = False
		Next
	End Sub

	' Token: 0x06001FA3 RID: 8099 RVA: 0x001218CF File Offset: 0x0011FCCF
	Private Sub OnDeathComplete()
		Me.StopAllCoroutines()
		MyBase.gameObject.SetActive(False)
	End Sub

	' Token: 0x06001FA4 RID: 8100 RVA: 0x001218E3 File Offset: 0x0011FCE3
	Private Sub DeathSfx()
	End Sub

	' Token: 0x06001FA5 RID: 8101 RVA: 0x001218E5 File Offset: 0x0011FCE5
	Private Sub OnDeathExploded()
		MyBase.GetComponent(Of LevelBossDeathExploder)().StopExplosions()
		Me.smallBird.StartPattern(MyBase.transform.position)
	End Sub

	' Token: 0x06001FA6 RID: 8102 RVA: 0x00121910 File Offset: 0x0011FD10
	Private Iterator Function die_cr() As IEnumerator
		Dim animator As Animator = MyBase.GetComponent(Of Animator)()
		While MyBase.transform.position.y > 100F OrElse MyBase.transform.position.y < 0F
			Yield Nothing
		End While
		Me.floating = False
		animator.Play("Death")
		Me.deathEffectFront.Create(Me.deathEffectsRoot.position)
		Me.deathEffectBack.Create(Me.deathEffectsRoot.position)
		Return
	End Function

	' Token: 0x06001FA7 RID: 8103 RVA: 0x0012192C File Offset: 0x0011FD2C
	Public Sub OnBossRevival()
		Me.state = FlyingBirdLevelBird.State.Reviving
		Me.houseCollider.gameObject.SetActive(True)
		Global.UnityEngine.[Object].Destroy(Me.deathParts)
		MyBase.gameObject.SetActive(True)
		MyBase.animator.Play("Revived")
		Me.nurses.animator.SetTrigger("StartNurses")
		MyBase.GetComponent(Of CircleCollider2D)().enabled = True
		MyBase.GetComponent(Of HitFlash)().StopAllCoroutines()
		MyBase.GetComponent(Of HitFlash)().SetColor(0F)
		MyBase.transform.SetPosition(New Single?(CSng(Level.Current.Right) + 250F), New Single?(CSng((Level.Current.Ground - 150))), Nothing)
		MyBase.StartCoroutine(Me.revival_cr())
		Me.heart.InitHeart(MyBase.properties)
	End Sub

	' Token: 0x06001FA8 RID: 8104 RVA: 0x00121A14 File Offset: 0x0011FE14
	Private Iterator Function revival_cr() As IEnumerator
		Dim [end] As Single = CSng(Level.Current.Ground) + 250F
		Yield MyBase.StartCoroutine(Me.move_to_position_cr(MyBase.transform.position.y, [end], MyBase.properties.CurrentState.floating.time, EaseUtils.EaseType.easeInOutSine))
		Me.state = FlyingBirdLevelBird.State.Revived
		Me.nurses.InitNurse(MyBase.properties.CurrentState.nurses)
		Return
	End Function

	' Token: 0x06001FA9 RID: 8105 RVA: 0x00121A30 File Offset: 0x0011FE30
	Private Iterator Function move_to_position_cr(start As Single, [end] As Single, time As Single, ease As EaseUtils.EaseType) As IEnumerator
		MyBase.transform.SetPosition(Nothing, New Single?(start), Nothing)
		Dim startX As Single = MyBase.transform.position.x
		Dim t As Single = 0F
		While t < time
			Dim val As Single = t / time
			MyBase.transform.SetPosition(New Single?(EaseUtils.Ease(ease, startX, 0F, val)), New Single?(EaseUtils.Ease(ease, start, [end], val)), Nothing)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		MyBase.transform.SetPosition(New Single?(0F), New Single?([end]), Nothing)
		Yield Nothing
		MyBase.StartCoroutine(Me.stretcherMove_cr())
		Return
	End Function

	' Token: 0x06001FAA RID: 8106 RVA: 0x00121A68 File Offset: 0x0011FE68
	Private Iterator Function stretcherMove_cr() As IEnumerator
		Dim movingRight As Boolean = Rand.Bool()
		Dim time As Single = MyBase.properties.CurrentState.bigBird.speedXTime
		Dim [end] As Single = 0F
		Do
			If Me.state <> FlyingBirdLevelBird.State.Heart Then
				Dim t As Single = 0F
				Dim start As Single = MyBase.transform.position.x
				If movingRight Then
					[end] = 290F
				Else
					[end] = -240F
				End If
				While t < time
					If Me.state <> FlyingBirdLevelBird.State.Heart Then
						Dim num As Single = t / time
						MyBase.transform.SetPosition(New Single?(EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, start, [end], num)), Nothing, Nothing)
						t += CupheadTime.Delta
					End If
					Yield Nothing
				End While
				MyBase.transform.SetPosition(New Single?([end]), Nothing, Nothing)
				movingRight = Not movingRight
			End If
			Yield Nothing
		Loop While MyBase.properties.CurrentHealth > 0F
		Return
	End Function

	' Token: 0x06001FAB RID: 8107 RVA: 0x00121A83 File Offset: 0x0011FE83
	Public Sub StartGarbageOne()
		MyBase.StartCoroutine(Me.garbage_cr())
	End Sub

	' Token: 0x06001FAC RID: 8108 RVA: 0x00121A94 File Offset: 0x0011FE94
	Private Iterator Function garbage_cr() As IEnumerator
		Me.state = FlyingBirdLevelBird.State.Garbage
		MyBase.animator.SetBool("OnGarbage", True)
		Yield MyBase.animator.WaitForAnimationToStart(Me, "Garbage_Start", False)
		AudioManager.Play("level_flyingbird_stretcher_garbage_start")
		Me.emitAudioFromObject.Add("level_flyingbird_stretcher_garbage_start")
		Dim garbageSpeed As Single = MyBase.properties.CurrentState.garbage.speedX
		Dim garbageCounter As Single = 0F
		Dim chosenPrefab As GameObject = Nothing
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Garbage_Start", False, True)
		Dim maxShotIndex As Integer = Global.UnityEngine.Random.Range(0, MyBase.properties.CurrentState.garbage.shotCount.Split(New Char() { ","c }).Length)
		Dim maxShot As Integer = Parser.IntParse(MyBase.properties.CurrentState.garbage.shotCount.Split(New Char() { ","c })(maxShotIndex))
		While garbageCounter < CSng(maxShot)
			Dim garbageTypes As String() = MyBase.properties.CurrentState.garbage.garbageTypeString(Me.garbageIndex).Split(New Char() { ","c })
			If garbageTypes(Me.typeIndex)(0) = "P"c Then
				chosenPrefab = Me.bootPinkPrefab
			ElseIf garbageTypes(Me.typeIndex)(0) = "B"c Then
				chosenPrefab = Me.bootPrefab
			ElseIf garbageTypes(Me.typeIndex)(0) = "F"c Then
				chosenPrefab = Me.fishPrefab
			ElseIf garbageTypes(Me.typeIndex)(0) = "A"c Then
				chosenPrefab = Me.applePrefab
			Else
				Global.Debug.LogError("Invalid garbage type string.", Nothing)
			End If
			Yield MyBase.animator.WaitForAnimationToEnd(Me, "Garbage", False, True)
			AudioManager.Play("level_flyingbird_stretcher_garbage")
			Me.emitAudioFromObject.Add("level_flyingbird_stretcher_garbage")
			Dim garbage As GameObject = Global.UnityEngine.[Object].Instantiate(Of GameObject)(chosenPrefab, Me.garbageRoot.transform.position, Quaternion.identity)
			garbage.GetComponent(Of BasicProjectile)().Speed = 1F
			garbage.transform.localScale = Vector3.one * MyBase.properties.CurrentState.garbage.shotSize
			MyBase.StartCoroutine(Me.multiShotGarbage_cr(garbageSpeed, garbage))
			garbageSpeed += MyBase.properties.CurrentState.garbage.speedXIncreaser
			garbageCounter += 1F
			If Me.typeIndex < garbageTypes.Length - 1 Then
				Me.typeIndex += 1
			Else
				Me.garbageIndex = (Me.garbageIndex + 1) Mod MyBase.properties.CurrentState.garbage.garbageTypeString.Length
				Me.typeIndex = 0
			End If
			If garbageCounter < CSng(maxShot) Then
				Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.garbage.shotDelay)
			End If
		End While
		garbageCounter = 0F
		MyBase.animator.SetTrigger("Continue")
		MyBase.animator.SetBool("OnGarbage", False)
		Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.garbage.hesitate.RandomFloat())
		Me.state = FlyingBirdLevelBird.State.Revived
		Return
	End Function

	' Token: 0x06001FAD RID: 8109 RVA: 0x00121AB0 File Offset: 0x0011FEB0
	Private Iterator Function multiShotGarbage_cr(speedX As Single, proj As GameObject) As IEnumerator
		Dim isFalling As Boolean = False
		Dim pct As Single = 1F
		Dim velocity As Vector3 = New Vector3(-speedX, MyBase.properties.CurrentState.garbage.speedY)
		While proj IsNot Nothing
			If proj.transform.position.y > CSng(Level.Current.Ground) - 200F Then
				If isFalling Then
					pct -= CupheadTime.Delta * 4F
					If pct < -1F Then
						pct = -1F
					End If
				End If
				velocity.y = MyBase.properties.CurrentState.garbage.speedY * pct
				proj.transform.position += velocity * CupheadTime.FixedDelta
				If proj.transform.position.y >= MyBase.properties.CurrentState.garbage.maxHeight Then
					isFalling = True
				End If
				Yield Nothing
			End If
			Yield Nothing
		End While
		Global.UnityEngine.[Object].Destroy(proj)
		Return
	End Function

	' Token: 0x06001FAE RID: 8110 RVA: 0x00121AD9 File Offset: 0x0011FED9
	Public Sub StartHeartAttack()
		Me.state = FlyingBirdLevelBird.State.HeartTrans
		MyBase.animator.SetBool("OnRegurgitate", True)
		AudioManager.Play("level_flyingbird_stretcher_regurgitate_start")
		Me.emitAudioFromObject.Add("level_flyingbird_stretcher_regurgitate_start")
	End Sub

	' Token: 0x06001FAF RID: 8111 RVA: 0x00121B0E File Offset: 0x0011FF0E
	Private Sub OpenHeart()
		Me.state = FlyingBirdLevelBird.State.Heart
		Me.heart.StartHeartAttack()
		MyBase.GetComponent(Of DamageReceiver)().enabled = False
		Me.heartSpitFX.SetActive(True)
		MyBase.StartCoroutine(Me.checkHeart_cr())
	End Sub

	' Token: 0x06001FB0 RID: 8112 RVA: 0x00121B48 File Offset: 0x0011FF48
	Private Iterator Function checkHeart_cr() As IEnumerator
		While Me.heart.gameObject.activeSelf
			Yield Nothing
		End While
		MyBase.animator.SetBool("OnRegurgitate", False)
		AudioManager.Play("level_flyingbird_stretcher_regurgitate_end")
		Me.emitAudioFromObject.Add("level_flyingbird_stretcher_regurgitate_end")
		MyBase.GetComponent(Of DamageReceiver)().enabled = True
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Regurgitate_End", False, True)
		Me.state = FlyingBirdLevelBird.State.HeartTrans
		Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.heart.hesitate.RandomFloat())
		Me.heartSpitFX.SetActive(False)
		Me.state = FlyingBirdLevelBird.State.Revived
		Return
	End Function

	' Token: 0x06001FB1 RID: 8113 RVA: 0x00121B64 File Offset: 0x0011FF64
	Private Sub NursesHeartHeight()
		For Each transform As Transform In Me.nurses.nurses
			transform.transform.localPosition = New Vector3(transform.transform.localPosition.x, transform.transform.localPosition.y + 8F)
		Next
	End Sub

	' Token: 0x06001FB2 RID: 8114 RVA: 0x00121BD4 File Offset: 0x0011FFD4
	Private Sub NursesGarbageHeight()
		For Each transform As Transform In Me.nurses.nurses
			transform.transform.localPosition = New Vector3(transform.transform.localPosition.x, transform.transform.localPosition.y + 6F)
		Next
	End Sub

	' Token: 0x06001FB3 RID: 8115 RVA: 0x00121C44 File Offset: 0x00120044
	Private Sub NursesReset()
		For Each transform As Transform In Me.nurses.nurses
			transform.transform.localPosition = Vector3.zero
		Next
	End Sub

	' Token: 0x0400280F RID: 10255
	<SerializeField()>
	Private bootPrefab As GameObject

	' Token: 0x04002810 RID: 10256
	<SerializeField()>
	Private bootPinkPrefab As GameObject

	' Token: 0x04002811 RID: 10257
	<SerializeField()>
	Private fishPrefab As GameObject

	' Token: 0x04002812 RID: 10258
	<SerializeField()>
	Private applePrefab As GameObject

	' Token: 0x04002813 RID: 10259
	<Space(10F)>
	<SerializeField()>
	Private smallBird As FlyingBirdLevelSmallBird

	' Token: 0x04002814 RID: 10260
	<Space(10F)>
	<SerializeField()>
	Private featherPrefab As FlyingBirdLevelBirdFeather

	' Token: 0x04002815 RID: 10261
	<Space(10F)>
	<SerializeField()>
	Private eggRoot As Transform

	' Token: 0x04002816 RID: 10262
	<SerializeField()>
	Private eggPrefab As FlyingBirdLevelBirdEgg

	' Token: 0x04002817 RID: 10263
	<SerializeField()>
	Private deathParts As GameObject

	' Token: 0x04002818 RID: 10264
	<Space(10F)>
	<SerializeField()>
	Private nurse1Root As Transform

	' Token: 0x04002819 RID: 10265
	<SerializeField()>
	Private nurse2Root As Transform

	' Token: 0x0400281A RID: 10266
	<SerializeField()>
	Private garbageRoot As Transform

	' Token: 0x0400281B RID: 10267
	<SerializeField()>
	Private heart As FlyingBirdLevelHeart

	' Token: 0x0400281C RID: 10268
	<SerializeField()>
	Private heartSpitFX As GameObject

	' Token: 0x0400281D RID: 10269
	<SerializeField()>
	Private nurses As FlyingBirdLevelNurses

	' Token: 0x0400281E RID: 10270
	<SerializeField()>
	Private head As GameObject

	' Token: 0x0400281F RID: 10271
	Private damageDealer As DamageDealer

	' Token: 0x04002820 RID: 10272
	Private damageReceiver As DamageReceiver

	' Token: 0x04002821 RID: 10273
	Private introEnded As Boolean

	' Token: 0x04002822 RID: 10274
	Private feathersFirstTime As Boolean

	' Token: 0x04002823 RID: 10275
	Private houseCollider As Transform

	' Token: 0x04002824 RID: 10276
	Private patternCoroutine As Coroutine

	' Token: 0x04002825 RID: 10277
	Private garbageIndex As Integer

	' Token: 0x04002826 RID: 10278
	Private typeIndex As Integer

	' Token: 0x04002827 RID: 10279
	Private blinks As Integer

	' Token: 0x04002828 RID: 10280
	Private maxBlinks As Integer = 6

	' Token: 0x04002829 RID: 10281
	<Space(10F)>
	<SerializeField()>
	Private laserRoots As Transform()

	' Token: 0x0400282A RID: 10282
	<SerializeField()>
	Private laserPrefab As BasicProjectile

	' Token: 0x0400282B RID: 10283
	<SerializeField()>
	Private laserEffect As Effect

	' Token: 0x0400282C RID: 10284
	<Space(10F)>
	<SerializeField()>
	Private deathEffectsRoot As Transform

	' Token: 0x0400282D RID: 10285
	<SerializeField()>
	Private deathEffectFront As Effect

	' Token: 0x0400282E RID: 10286
	<SerializeField()>
	Private deathEffectBack As Effect

	' Token: 0x02000617 RID: 1559
	Public Enum State
		' Token: 0x04002830 RID: 10288
		Init
		' Token: 0x04002831 RID: 10289
		Idle
		' Token: 0x04002832 RID: 10290
		Feathers
		' Token: 0x04002833 RID: 10291
		Eggs
		' Token: 0x04002834 RID: 10292
		Dying
		' Token: 0x04002835 RID: 10293
		Dead
		' Token: 0x04002836 RID: 10294
		Whistle
		' Token: 0x04002837 RID: 10295
		Lasers
		' Token: 0x04002838 RID: 10296
		LasersEnding
		' Token: 0x04002839 RID: 10297
		Reviving
		' Token: 0x0400283A RID: 10298
		Revived
		' Token: 0x0400283B RID: 10299
		Garbage
		' Token: 0x0400283C RID: 10300
		Heart
		' Token: 0x0400283D RID: 10301
		HeartTrans
	End Enum
End Class
