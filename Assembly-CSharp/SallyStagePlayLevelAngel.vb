Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x020007A4 RID: 1956
Public Class SallyStagePlayLevelAngel
	Inherits LevelProperties.SallyStagePlay.Entity

	' Token: 0x17000403 RID: 1027
	' (get) Token: 0x06002BCD RID: 11213 RVA: 0x001992C5 File Offset: 0x001976C5
	' (set) Token: 0x06002BCE RID: 11214 RVA: 0x001992CD File Offset: 0x001976CD
	Public Property state As SallyStagePlayLevelAngel.State

	' Token: 0x06002BCF RID: 11215 RVA: 0x001992D8 File Offset: 0x001976D8
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.signStart = Me.sign.transform.position
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		MyBase.GetComponent(Of Collider2D)().enabled = False
	End Sub

	' Token: 0x06002BD0 RID: 11216 RVA: 0x0019933C File Offset: 0x0019773C
	Public Overrides Sub LevelInit(properties As LevelProperties.SallyStagePlay)
		MyBase.LevelInit(properties)
		Dim lightning As LevelProperties.SallyStagePlay.Lightning = properties.CurrentState.lightning
		SallyStagePlayLevelAngel.extraHP = properties.CurrentState.husband.deityHP
		Me.lightningMax = Global.UnityEngine.Random.Range(CInt(lightning.lightningDelayRange.min), CInt(lightning.lightningDelayRange.max))
		Me.lightningShotIndex = Global.UnityEngine.Random.Range(0, lightning.lightningShotCount.Split(New Char() { ","c }).Length)
		Me.lightningAngleIndex = Global.UnityEngine.Random.Range(0, lightning.lightningAngleString.Split(New Char() { ","c }).Length)
		Me.lightningSpawnIndex = Global.UnityEngine.Random.Range(0, lightning.lightningSpawnString.Split(New Char() { ","c }).Length)
		Me.meteorSpawnIndex = Global.UnityEngine.Random.Range(0, properties.CurrentState.meteor.meteorSpawnString.Split(New Char() { ","c }).Length)
		Me.meteors = New List(Of SallyStagePlayLevelMeteor)()
		If Level.Current.mode = Level.Mode.Easy Then
			AddHandler Level.Current.OnWinEvent, AddressOf Me.OnEasyDeath
		Else
			AddHandler Level.Current.OnWinEvent, AddressOf Me.OnDeath
		End If
	End Sub

	' Token: 0x06002BD1 RID: 11217 RVA: 0x0019947C File Offset: 0x0019787C
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		If Me.killedHusband AndAlso SallyStagePlayLevelAngel.extraHP > 0F Then
			SallyStagePlayLevelAngel.extraHP -= info.damage
		Else
			MyBase.properties.DealDamage(info.damage)
		End If
	End Sub

	' Token: 0x06002BD2 RID: 11218 RVA: 0x001994CA File Offset: 0x001978CA
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06002BD3 RID: 11219 RVA: 0x001994E2 File Offset: 0x001978E2
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x06002BD4 RID: 11220 RVA: 0x00199500 File Offset: 0x00197900
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.meteorPrefab = Nothing
		Me.lightningPrefab = Nothing
		Me.umbrellaPrefab = Nothing
	End Sub

	' Token: 0x06002BD5 RID: 11221 RVA: 0x0019951D File Offset: 0x0019791D
	Public Sub StartPhase3(killedHusband As Boolean)
		Me.killedHusband = killedHusband
		MyBase.StartCoroutine(Me.intro_cr())
	End Sub

	' Token: 0x06002BD6 RID: 11222 RVA: 0x00199534 File Offset: 0x00197934
	Private Iterator Function intro_cr() As IEnumerator
		Dim t As Single = 0F
		Dim time As Single = 3F
		Dim endPos As Vector3 = New Vector3(MyBase.transform.position.x, Me.phase3Root.position.y)
		Dim start As Vector2 = MyBase.transform.position
		MyBase.GetComponent(Of Collider2D)().enabled = True
		MyBase.StartCoroutine(Me.sally_angel_intro_sound_cr())
		If Me.killedHusband Then
			MyBase.StartCoroutine(Me.spawn_husband_cr())
		End If
		While t < time
			Dim val As Single = EaseUtils.Ease(EaseUtils.EaseType.easeInOutBounce, 0F, 1F, t / time)
			MyBase.transform.position = Vector2.Lerp(start, endPos, val)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		MyBase.transform.position = endPos
		Me.nextAttack = 1
		MyBase.StartCoroutine(Me.sign_slide_cr())
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		MyBase.StartCoroutine(Me.main_cr())
		Yield Nothing
		Return
	End Function

	' Token: 0x06002BD7 RID: 11223 RVA: 0x00199550 File Offset: 0x00197950
	Private Iterator Function sally_angel_intro_sound_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 4F)
		AudioManager.Play("sally_vox_maniacal")
		Me.emitAudioFromObject.Add("sally_vox_maniacal")
		Return
	End Function

	' Token: 0x06002BD8 RID: 11224 RVA: 0x0019956C File Offset: 0x0019796C
	Private Iterator Function spawn_husband_cr() As IEnumerator
		Me.husband.gameObject.SetActive(True)
		Me.husband.GetComponent(Of Collider2D)().enabled = True
		Dim t As Single = 0F
		Dim time As Single = 3.5F
		Dim endPos As Vector3 = New Vector3(Me.phase3Root.transform.position.x, Me.husband.transform.position.y)
		Dim start As Vector2 = Me.husband.transform.position
		Dim soundTriggered As Boolean = False
		While t < time
			If t / time >= 0.3F AndAlso Not soundTriggered Then
				AudioManager.Play("sally_fiance_enter")
				Me.emitAudioFromObject.Add("sally_fiance_enter")
				soundTriggered = True
			End If
			Dim val As Single = EaseUtils.Ease(EaseUtils.EaseType.easeInOutBounce, 0F, 1F, t / time)
			Me.husband.transform.position = Vector2.Lerp(start, endPos, val)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		Me.husband.Attack()
		Yield Nothing
		Return
	End Function

	' Token: 0x06002BD9 RID: 11225 RVA: 0x00199588 File Offset: 0x00197988
	Private Iterator Function sign_slide_cr() As IEnumerator
		Dim attackName As String = String.Empty
		Dim num As Integer = Me.nextAttack
		If num <> 0 Then
			If num <> 1 Then
				If num = 2 Then
					attackName = "Wave"
				End If
			Else
				attackName = "Meteor"
			End If
		Else
			attackName = "Lightning"
		End If
		Me.sign.Play(attackName)
		Dim t As Single = 0F
		Dim time As Single = 0.1F
		Dim start As Vector3 = Me.sign.transform.position
		While t < time
			t += CupheadTime.Delta
			Me.sign.transform.position = Vector3.Lerp(start, New Vector3(Me.signStart.x, Me.signStart.y - 100F), t / time)
			Yield Nothing
		End While
		t = 0F
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		start = Me.sign.transform.position
		While t < time
			t += CupheadTime.Delta
			Me.sign.transform.position = Vector3.Lerp(start, Me.signStart, t / time)
			Yield Nothing
		End While
		Yield Nothing
		Return
	End Function

	' Token: 0x06002BDA RID: 11226 RVA: 0x001995A4 File Offset: 0x001979A4
	Private Iterator Function slide_out_cr() As IEnumerator
		Dim t As Single = 0F
		Dim time As Single = 0.1F
		Dim start As Vector3 = Me.sign.transform.position
		While t < time
			t += CupheadTime.Delta
			Me.sign.transform.position = Vector3.Lerp(start, Me.signStart, t / time)
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06002BDB RID: 11227 RVA: 0x001995C0 File Offset: 0x001979C0
	Private Iterator Function main_cr() As IEnumerator
		Dim p As LevelProperties.SallyStagePlay.General = MyBase.properties.CurrentState.general
		Dim main As String() = p.attackString.GetRandom().Split(New Char() { ","c })
		Dim mainIndex As Integer = Global.UnityEngine.Random.Range(0, main.Length)
		Me.nextAttack = mainIndex
		While main(mainIndex) <> "M"
			mainIndex = (mainIndex + 1) Mod main.Length
			Yield Nothing
		End While
		While True
			While Me.state <> SallyStagePlayLevelAngel.State.Idle
				Yield Nothing
			End While
			MyBase.animator.SetBool("OnPh3Attack", True)
			MyBase.StartCoroutine(Me.sign_slide_cr())
			Yield MyBase.animator.WaitForAnimationToStart(Me, "Phase3_Attack_Start", False)
			Dim text As String = main(mainIndex)
			If text IsNot Nothing Then
				If Not(text = "L") Then
					If Not(text = "M") Then
						If text = "T" Then
							MyBase.StartCoroutine(Me.tidal_wave_cr())
						End If
					Else
						MyBase.StartCoroutine(Me.meteor_cr())
					End If
				Else
					MyBase.StartCoroutine(Me.lightning_cr())
				End If
			End If
			mainIndex = (mainIndex + 1) Mod main.Length
			Me.GetNextAttack(main(mainIndex))
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06002BDC RID: 11228 RVA: 0x001995DC File Offset: 0x001979DC
	Private Sub GetNextAttack(main As String)
		If main IsNot Nothing Then
			If Not(main = "L") Then
				If Not(main = "M") Then
					If main = "T" Then
						Me.nextAttack = 2
					End If
				Else
					Me.nextAttack = 1
				End If
			Else
				Me.nextAttack = 0
			End If
		End If
	End Sub

	' Token: 0x06002BDD RID: 11229 RVA: 0x00199648 File Offset: 0x00197A48
	Private Iterator Function lightning_cr() As IEnumerator
		Me.state = SallyStagePlayLevelAngel.State.Lightning
		Dim p As LevelProperties.SallyStagePlay.Lightning = MyBase.properties.CurrentState.lightning
		Dim shotString As String() = p.lightningShotCount.Split(New Char() { ","c })
		Dim angleString As String() = p.lightningAngleString.Split(New Char() { ","c })
		Dim spawnString As String() = p.lightningSpawnString.Split(New Char() { ","c })
		Dim angle As Single = 0F
		Dim spawn As Single = 0F
		Dim rotation As Single = 0F
		Dim shotCount As Integer = 0
		Parser.IntTryParse(shotString(Me.lightningShotIndex), shotCount)
		For i As Integer = 0 To shotCount - 1
			Parser.FloatTryParse(spawnString(Me.lightningSpawnIndex), spawn)
			Dim aimAtPlayer As Boolean
			If Me.lightningMaxCounter >= Me.lightningMax Then
				aimAtPlayer = True
				Me.lightningMaxCounter = 0
			Else
				aimAtPlayer = False
				If Me.lightningMaxCounter = 0 Then
					Me.lightningMax = Global.UnityEngine.Random.Range(CInt(p.lightningDirectAimRange.min), CInt(p.lightningDirectAimRange.max))
				End If
				Parser.FloatTryParse(angleString(Me.lightningAngleIndex), angle)
				Me.lightningAngleIndex = (Me.lightningAngleIndex + 1) Mod angleString.Length
				Me.lightningMaxCounter += 1
			End If
			Dim pos As Vector3 = New Vector3(-640F + spawn, 460F)
			If aimAtPlayer Then
				Dim [next] As AbstractPlayerController = PlayerManager.GetNext()
				Dim vector As Vector3 = [next].transform.position - pos
				rotation = MathUtils.DirectionToAngle(vector)
			Else
				rotation = angle
			End If
			Me.lightningPrefab.Create(pos, rotation, p.lightningSpeed, i = shotCount - 1)
			Me.lightningSpawnIndex = (Me.lightningSpawnIndex + 1) Mod spawnString.Length
			Yield CupheadTime.WaitForSeconds(Me, p.lightningDelayRange.RandomFloat())
		Next
		MyBase.animator.SetBool("OnPh3Attack", False)
		Me.lightningShotIndex = (Me.lightningShotIndex + 1) Mod shotString.Length
		Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.general.attackDelayRange.RandomFloat())
		Me.state = SallyStagePlayLevelAngel.State.Idle
		Yield Nothing
		Return
	End Function

	' Token: 0x06002BDE RID: 11230 RVA: 0x00199664 File Offset: 0x00197A64
	Private Iterator Function meteor_cr() As IEnumerator
		Me.state = SallyStagePlayLevelAngel.State.Meteor
		Dim p As LevelProperties.SallyStagePlay.Meteor = MyBase.properties.CurrentState.meteor
		Dim meteorSpawnString As String() = p.meteorSpawnString.Split(New Char() { ","c })
		Dim index As Integer = 0
		Dim spawn As Single = 0F
		Parser.FloatTryParse(meteorSpawnString(Me.meteorSpawnIndex), spawn)
		Dim lockedPosition As Boolean = False
		For i As Integer = 0 To Me.meteors.Count - 1
			If Me.meteors(i).state = SallyStagePlayLevelMeteor.State.Leaving Then
				Me.meteors.Remove(Me.meteors(i))
				i += 1
			End If
		Next
		Yield Nothing
		For j As Integer = 0 To Me.meteors.Count - 1
			If spawn = Me.meteors(j).spawnPosition Then
				index = j
				lockedPosition = True
				Exit For
			End If
		Next
		Dim positionTaken As Boolean = False
		Dim meteorCounter As Integer = 0
		Dim spawnStringCounter As Integer = 0
		While lockedPosition
			While meteorCounter < Me.meteors.Count
				meteorCounter += 1
				If spawn = Me.meteors(index).spawnPosition Then
					positionTaken = True
				End If
				index = (index + 1) Mod Me.meteors.Count
			End While
			If Not positionTaken Then
				lockedPosition = False
				Exit While
			End If
			Me.meteorSpawnIndex = (Me.meteorSpawnIndex + 1) Mod meteorSpawnString.Length
			Parser.FloatTryParse(meteorSpawnString(Me.meteorSpawnIndex), spawn)
			spawnStringCounter += 1
			If spawnStringCounter >= meteorSpawnString.Length Then
				Exit While
			End If
			meteorCounter = 0
			positionTaken = False
			Yield Nothing
		End While
		If Me.meteors.Count <= 0 Then
			lockedPosition = False
		End If
		If Not lockedPosition Then
			Me.meteors.Add(Me.meteorPrefab.Create(spawn, CSng(p.meteorHP), p))
			Me.meteorSpawnIndex = (Me.meteorSpawnIndex + 1) Mod meteorSpawnString.Length
		End If
		MyBase.animator.SetBool("OnPh3Attack", False)
		Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.general.attackDelayRange.RandomFloat())
		Me.state = SallyStagePlayLevelAngel.State.Idle
		Yield Nothing
		Return
	End Function

	' Token: 0x06002BDF RID: 11231 RVA: 0x00199680 File Offset: 0x00197A80
	Private Iterator Function tidal_wave_cr() As IEnumerator
		Me.state = SallyStagePlayLevelAngel.State.Wave
		Dim p As LevelProperties.SallyStagePlay.Tidal = MyBase.properties.CurrentState.tidal
		Me.wave.StartWave(p)
		While Me.wave.isMoving
			Yield Nothing
		End While
		MyBase.animator.SetBool("OnPh3Attack", False)
		Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.tidal.tidalHesitate)
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Phase3_Attack", False, False)
		Me.state = SallyStagePlayLevelAngel.State.Idle
		Yield Nothing
		Return
	End Function

	' Token: 0x06002BE0 RID: 11232 RVA: 0x0019969C File Offset: 0x00197A9C
	Public Sub OnPhase4()
		AudioManager.[Stop]("sally_sally_lightning_move_loop")
		Me.StopAllCoroutines()
		MyBase.StartCoroutine(Me.slide_out_cr())
		MyBase.GetComponent(Of LevelBossDeathExploder)().StartExplosion()
		MyBase.animator.SetTrigger("OnPh3Death")
		MyBase.StartCoroutine(Me.start_phase_4_cr())
	End Sub

	' Token: 0x06002BE1 RID: 11233 RVA: 0x001996F0 File Offset: 0x00197AF0
	Private Iterator Function start_phase_4_cr() As IEnumerator
		MyBase.GetComponent(Of SpriteRenderer)().material = Me.phase4Material
		For i As Integer = 0 To Me.meteors.Count - 1
			If Me.meteors(i) IsNot Nothing Then
				Me.meteors(i).MeteorChangePhase()
			End If
		Next
		Dim t As Single = 0F
		Dim time As Single = 2.5F
		Dim endPos As Vector3 = New Vector3(MyBase.transform.position.x, 860F)
		Dim start As Vector2 = MyBase.transform.position
		Yield CupheadTime.WaitForSeconds(Me, 0.5F)
		If Me.killedHusband Then
			Me.husband.Dead()
			MyBase.StartCoroutine(Me.husband.move_cr())
		End If
		While t < time
			Dim val As Single = EaseUtils.Ease(EaseUtils.EaseType.linear, 0F, 1F, t / time)
			MyBase.transform.position = Vector2.Lerp(start, endPos, val)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		MyBase.GetComponent(Of LevelBossDeathExploder)().StopExplosions()
		For Each gameObject As GameObject In Me.[shadows]
			gameObject.SetActive(False)
		Next
		MyBase.animator.Play("Phase4_Idle")
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		t = 0F
		time = 1F
		Dim pos As Vector3 = MyBase.transform.position
		pos.x = -640F + MyBase.transform.GetComponent(Of Renderer)().bounds.size.x / 2F
		MyBase.transform.position = pos
		endPos = New Vector3(MyBase.transform.position.x, Me.phase4Root.position.y)
		start = MyBase.transform.position
		While t < time
			Dim val2 As Single = EaseUtils.Ease(EaseUtils.EaseType.linear, 0F, 1F, t / time)
			MyBase.transform.position = Vector2.Lerp(start, endPos, val2)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		MyBase.StartCoroutine(Me.move_cr())
		MyBase.StartCoroutine(Me.spawn_roses_cr())
		Me.SpawnUmbrella()
		Yield Nothing
		Return
	End Function

	' Token: 0x06002BE2 RID: 11234 RVA: 0x0019970C File Offset: 0x00197B0C
	Private Sub SpawnUmbrella()
		Dim sallyStagePlayLevelUmbrella As SallyStagePlayLevelUmbrella = Global.UnityEngine.[Object].Instantiate(Of SallyStagePlayLevelUmbrella)(Me.umbrellaPrefab)
		sallyStagePlayLevelUmbrella.GetProperties(MyBase.properties)
		sallyStagePlayLevelUmbrella.EnableHoming = False
		Dim num As Single = If((Not Rand.Bool()), 140F, (-140F))
		sallyStagePlayLevelUmbrella.transform.position = New Vector2(num, 460F)
		MyBase.StartCoroutine(Me.umbrella_cr(sallyStagePlayLevelUmbrella))
	End Sub

	' Token: 0x06002BE3 RID: 11235 RVA: 0x0019977C File Offset: 0x00197B7C
	Private Iterator Function umbrella_cr(umbrella As SallyStagePlayLevelUmbrella) As IEnumerator
		While True
			umbrella.TrackingPlayer = PlayerManager.GetNext()
			Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.umbrella.homingUntilSwitchPlayer)
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06002BE4 RID: 11236 RVA: 0x001997A0 File Offset: 0x00197BA0
	Private Iterator Function move_cr() As IEnumerator
		Dim t As Single = 0F
		Dim time As Single = MyBase.properties.CurrentState.general.finalMovementSpeed
		Dim sizeX As Single = MyBase.transform.GetComponent(Of Renderer)().bounds.size.x / 2F
		Dim ease As EaseUtils.EaseType = EaseUtils.EaseType.easeInOutSine
		Dim start As Single = -640F + sizeX
		Dim [end] As Single = 640F - sizeX
		While True
			t = 0F
			While t < time
				Dim val As Single = t / time
				MyBase.transform.SetPosition(New Single?(EaseUtils.Ease(ease, start, [end], val)), Nothing, Nothing)
				t += CupheadTime.Delta
				Yield Nothing
			End While
			MyBase.transform.SetPosition(New Single?([end]), Nothing, Nothing)
			t = 0F
			While t < time
				Dim val2 As Single = t / time
				MyBase.transform.SetPosition(New Single?(EaseUtils.Ease(ease, [end], start, val2)), Nothing, Nothing)
				t += CupheadTime.Delta
				Yield Nothing
			End While
			MyBase.transform.SetPosition(New Single?(start), Nothing, Nothing)
		End While
		Return
	End Function

	' Token: 0x06002BE5 RID: 11237 RVA: 0x001997BC File Offset: 0x00197BBC
	Private Iterator Function spawn_roses_cr() As IEnumerator
		Dim p As LevelProperties.SallyStagePlay.Roses = MyBase.properties.CurrentState.roses
		Dim roseString As String() = p.spawnString.GetRandom().Split(New Char() { ","c })
		Dim roseIndex As Integer = Global.UnityEngine.Random.Range(0, roseString.Length)
		Dim yCoord As Single = 460F
		Dim xCoord As Single = 0F
		Dim maxCount As Integer = p.playerAimRange.RandomInt()
		Dim counter As Integer = 0
		While True
			If counter < maxCount Then
				Parser.FloatTryParse(roseString(roseIndex), xCoord)
				counter += 1
			Else
				Dim [next] As AbstractPlayerController = PlayerManager.GetNext()
				xCoord = [next].transform.position.x
				counter = 0
				maxCount = p.playerAimRange.RandomInt()
			End If
			Dim position As Vector3 = New Vector3(-640F + xCoord, yCoord)
			Me.applauseHandler.ThrowRose(position, p)
			roseIndex = (roseIndex + 1) Mod roseString.Length
			Yield CupheadTime.WaitForSeconds(Me, p.spawnDelayRange.RandomFloat())
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06002BE6 RID: 11238 RVA: 0x001997D8 File Offset: 0x00197BD8
	Private Sub OnEasyDeath()
		Me.StopAllCoroutines()
		MyBase.GetComponent(Of Collider2D)().enabled = False
		If Me.killedHusband Then
			Me.husband.GetComponent(Of Animator)().SetTrigger("OnDeath")
		End If
		MyBase.animator.SetTrigger("OnPh3Death")
	End Sub

	' Token: 0x06002BE7 RID: 11239 RVA: 0x00199827 File Offset: 0x00197C27
	Private Sub OnDeath()
		Me.StopAllCoroutines()
		MyBase.StartCoroutine(Me.sally_angel_death_sound_cr())
		MyBase.GetComponent(Of Collider2D)().enabled = False
		MyBase.animator.SetTrigger("OnPh4Death")
		MyBase.StartCoroutine(Me.birds_death_cr())
	End Sub

	' Token: 0x06002BE8 RID: 11240 RVA: 0x00199868 File Offset: 0x00197C68
	Private Iterator Function sally_angel_death_sound_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1.5F)
		AudioManager.Play("sally_p4_angel_death_vox")
		Return
	End Function

	' Token: 0x06002BE9 RID: 11241 RVA: 0x00199884 File Offset: 0x00197C84
	Private Iterator Function birds_death_cr() As IEnumerator
		Dim t As Single = 0F
		Dim time As Single = 2F
		Dim pos As Vector3 = Me.birdsDeath.transform.position
		Me.birdsDeath.SetActive(True)
		While t < time
			Dim val As Single = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0F, 1F, t / time)
			pos.y = Mathf.Lerp(pos.y, Me.birdRoot.transform.position.y, val)
			Me.birdsDeath.transform.position = pos
			t += CupheadTime.Delta
			Yield Nothing
		End While
		Yield Nothing
		Return
	End Function

	' Token: 0x06002BEA RID: 11242 RVA: 0x0019989F File Offset: 0x00197C9F
	Private Sub SoundAngelIdle()
		AudioManager.Play("sally_angel_idle")
		Me.emitAudioFromObject.Add("sally_angel_idle")
	End Sub

	' Token: 0x06002BEB RID: 11243 RVA: 0x001998BB File Offset: 0x00197CBB
	Private Sub SoundAngelDeath()
		AudioManager.Play("sally_angel_death")
		Me.emitAudioFromObject.Add("sally_angel_death")
	End Sub

	' Token: 0x04003478 RID: 13432
	Public Shared extraHP As Single

	' Token: 0x04003479 RID: 13433
	<SerializeField()>
	Private phase4Material As Material

	' Token: 0x0400347A RID: 13434
	<SerializeField()>
	Private applauseHandler As SallyStagePlayApplauseHandler

	' Token: 0x0400347B RID: 13435
	<SerializeField()>
	Private sign As Animator

	' Token: 0x0400347C RID: 13436
	<SerializeField()>
	Private wave As SallyStagePlayLevelWave

	' Token: 0x0400347D RID: 13437
	<SerializeField()>
	Private meteorPrefab As SallyStagePlayLevelMeteor

	' Token: 0x0400347E RID: 13438
	<SerializeField()>
	Private lightningPrefab As SallyStagePlayLevelLightning

	' Token: 0x0400347F RID: 13439
	<SerializeField()>
	Private umbrellaPrefab As SallyStagePlayLevelUmbrella

	' Token: 0x04003480 RID: 13440
	<SerializeField()>
	Private birdsDeath As GameObject

	' Token: 0x04003481 RID: 13441
	<SerializeField()>
	Private husband As SallyStagePlayLevelFianceDeity

	' Token: 0x04003482 RID: 13442
	<SerializeField()>
	Private [shadows] As GameObject()

	' Token: 0x04003483 RID: 13443
	<Space(10F)>
	<SerializeField()>
	Private phase4Root As Transform

	' Token: 0x04003484 RID: 13444
	<SerializeField()>
	Private birdRoot As Transform

	' Token: 0x04003485 RID: 13445
	<SerializeField()>
	Private phase3Root As Transform

	' Token: 0x04003487 RID: 13447
	Private meteors As List(Of SallyStagePlayLevelMeteor)

	' Token: 0x04003488 RID: 13448
	Private damageDealer As DamageDealer

	' Token: 0x04003489 RID: 13449
	Private damageReceiver As DamageReceiver

	' Token: 0x0400348A RID: 13450
	Private signStart As Vector3

	' Token: 0x0400348B RID: 13451
	Private killedHusband As Boolean

	' Token: 0x0400348C RID: 13452
	Private nextAttack As Integer

	' Token: 0x0400348D RID: 13453
	Private lightningShotIndex As Integer

	' Token: 0x0400348E RID: 13454
	Private lightningAngleIndex As Integer

	' Token: 0x0400348F RID: 13455
	Private lightningSpawnIndex As Integer

	' Token: 0x04003490 RID: 13456
	Private lightningMax As Integer

	' Token: 0x04003491 RID: 13457
	Private lightningMaxCounter As Integer

	' Token: 0x04003492 RID: 13458
	Private meteorSpawnIndex As Integer

	' Token: 0x020007A5 RID: 1957
	Public Enum State
		' Token: 0x04003494 RID: 13460
		Idle
		' Token: 0x04003495 RID: 13461
		Lightning
		' Token: 0x04003496 RID: 13462
		Wave
		' Token: 0x04003497 RID: 13463
		Meteor
	End Enum
End Class
