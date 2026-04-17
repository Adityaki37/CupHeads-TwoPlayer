Imports System
Imports System.Collections
Imports System.Diagnostics
Imports UnityEngine

' Token: 0x020004DF RID: 1247
Public Class BaronessLevelCastle
	Inherits LevelProperties.Baroness.Entity

	' Token: 0x1700031A RID: 794
	' (get) Token: 0x06001569 RID: 5481 RVA: 0x000BF6E6 File Offset: 0x000BDAE6
	' (set) Token: 0x0600156A RID: 5482 RVA: 0x000BF6ED File Offset: 0x000BDAED
	Public Shared Property CURRENT_MINI_BOSS As BaronessLevelMiniBossBase

	' Token: 0x1700031B RID: 795
	' (get) Token: 0x0600156B RID: 5483 RVA: 0x000BF6F5 File Offset: 0x000BDAF5
	' (set) Token: 0x0600156C RID: 5484 RVA: 0x000BF6FD File Offset: 0x000BDAFD
	Public Property state As BaronessLevelCastle.State

	' Token: 0x1700031C RID: 796
	' (get) Token: 0x0600156D RID: 5485 RVA: 0x000BF706 File Offset: 0x000BDB06
	' (set) Token: 0x0600156E RID: 5486 RVA: 0x000BF70E File Offset: 0x000BDB0E
	Public Property teethState As BaronessLevelCastle.TeethState

	' Token: 0x1400003E RID: 62
	' (add) Token: 0x0600156F RID: 5487 RVA: 0x000BF718 File Offset: 0x000BDB18
	' (remove) Token: 0x06001570 RID: 5488 RVA: 0x000BF750 File Offset: 0x000BDB50
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnDeathEvent As Action

	' Token: 0x06001571 RID: 5489 RVA: 0x000BF788 File Offset: 0x000BDB88
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.maxMiniBosses = False
		Me.continueTransition = False
		Me.originalEmergePos = Me.emergePoint.position
		Me.originalBaronessPoint = Me.baronessPhase1.transform.position
		Me.teethState = BaronessLevelCastle.TeethState.Unspawned
		Me.baronessPhase2.gameObject.SetActive(False)
		Me.blink.enabled = False
		Me.blinkCounterMax = Global.UnityEngine.Random.Range(4, 7)
		Me.damageReceiver = Me.baronessPhase2.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		Me.damageDealer = New DamageDealer(1F, 1F)
	End Sub

	' Token: 0x06001572 RID: 5490 RVA: 0x000BF840 File Offset: 0x000BDC40
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		MyBase.properties.DealDamage(info.damage)
		If MyBase.properties.CurrentHealth <= 0F AndAlso Me.state <> BaronessLevelCastle.State.Dead Then
			Me.state = BaronessLevelCastle.State.Dead
			Me.StartDeath()
		End If
	End Sub

	' Token: 0x06001573 RID: 5491 RVA: 0x000BF88C File Offset: 0x000BDC8C
	Private Sub Update()
		Me.damageDealer.Update()
		Me.player = PlayerManager.GetNext()
		Me.distToGround = Me.player.transform.position.y - -360F
		If Me.state = BaronessLevelCastle.State.Idle Then
			If BaronessLevelCastle.CURRENT_MINI_BOSS Is Nothing Then
				If Not Me.maxMiniBosses Then
					Me.jellyChangeDelay = True
					Me.StartOpen()
				ElseIf Level.Current.mode <> Level.Mode.Easy Then
					Me.state = BaronessLevelCastle.State.ChaseIntro
					Me.StartChase()
				End If
			ElseIf Level.Current.mode = Level.Mode.Easy AndAlso Me.state <> BaronessLevelCastle.State.EasyFinal AndAlso BaronessLevelCastle.CURRENT_MINI_BOSS.isDying AndAlso Me.maxMiniBosses Then
				Me.state = BaronessLevelCastle.State.EasyFinal
				MyBase.StartCoroutine(Me.shoot_easy_cr())
			End If
		End If
	End Sub

	' Token: 0x06001574 RID: 5492 RVA: 0x000BF976 File Offset: 0x000BDD76
	Public Overrides Sub LevelInit(properties As LevelProperties.Baroness)
		MyBase.LevelInit(properties)
		Me.baronessPhase1.getProperties(properties, CSng(properties.CurrentState.baronessVonBonbon.HP), Me)
		Me.platform.getProperties(properties.CurrentState.platform)
	End Sub

	' Token: 0x06001575 RID: 5493 RVA: 0x000BF9B3 File Offset: 0x000BDDB3
	Public Sub StartIntro()
		MyBase.StartCoroutine(Me.intro_cr())
	End Sub

	' Token: 0x06001576 RID: 5494 RVA: 0x000BF9C4 File Offset: 0x000BDDC4
	Private Iterator Function intro_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 0.6F)
		Me.baronessPhase1.animator.SetTrigger("Continue")
		Yield CupheadTime.WaitForSeconds(Me, 2F)
		MyBase.animator.Play("Castle_Open")
		AudioManager.Play("level_baroness_castle_gate_open")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Castle_Open", False, True)
		Yield CupheadTime.WaitForSeconds(Me, 1.5F)
		Me.state = BaronessLevelCastle.State.Idle
		Return
	End Function

	' Token: 0x06001577 RID: 5495 RVA: 0x000BF9DF File Offset: 0x000BDDDF
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06001578 RID: 5496 RVA: 0x000BF9FD File Offset: 0x000BDDFD
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.peppermintPrefab = Nothing
		Me.cupcakePrefab = Nothing
		Me.wafflePrefab = Nothing
		Me.gumballPrefab = Nothing
		Me.jawBreakerPrefab = Nothing
		Me.candyCornPrefab = Nothing
		Me.greenJellyPrefab = Nothing
		Me.pinkJellyPrefab = Nothing
	End Sub

	' Token: 0x06001579 RID: 5497 RVA: 0x000BFA3D File Offset: 0x000BDE3D
	Public Sub StartOpen()
		Me.state = BaronessLevelCastle.State.Open
		MyBase.StartCoroutine(Me.open_cr())
	End Sub

	' Token: 0x0600157A RID: 5498 RVA: 0x000BFA53 File Offset: 0x000BDE53
	Private Sub SetEyes()
		MyBase.animator.SetBool("ToCastleLoop", True)
	End Sub

	' Token: 0x0600157B RID: 5499 RVA: 0x000BFA68 File Offset: 0x000BDE68
	Private Iterator Function open_cr() As IEnumerator
		Dim p As LevelProperties.Baroness.Open = MyBase.properties.CurrentState.open
		Me.castleOpen = True
		If Me.baronessPoppedUp Then
			Me.baronessPoppedUp = False
			Yield Me.baronessPhase1.animator.WaitForAnimationToEnd(Me, "Baroness_Leave", False, True)
		End If
		If Me.bossIndex <> 0 Then
			AudioManager.Play("level_baroness_castle_gate_open")
			MyBase.animator.Play("Castle_Open")
			Yield MyBase.animator.WaitForAnimationToEnd(Me, "Castle_Open", False, True)
			Me.baronessPhase1.animator.Play("Baroness_Mad_Start")
			AudioManager.Play("level_baroness_stick_head_pop")
			While Me.baronessPhase1.popUpCounter < 4
				Yield Nothing
			End While
			Me.baronessPhase1.animator.SetTrigger("PopIn")
			AudioManager.Play("level_baroness_stick_head_pop")
			Me.baronessPhase1.popUpCounter = 0
			Yield CupheadTime.WaitForSeconds(Me, 1.5F)
		End If
		Select Case CType([Enum].Parse(GetType(BaronessLevelCastle.BossPossibility), BaronessLevel.PICKED_BOSSES(Me.bossIndex)), BaronessLevelCastle.BossPossibility)
			Case BaronessLevelCastle.BossPossibility.Gumball
				Me.SpawnGumball()
			Case BaronessLevelCastle.BossPossibility.Waffle
				Me.SpawnWaffle()
			Case BaronessLevelCastle.BossPossibility.CandyCorn
				Me.SpawnCandyCorn()
			Case BaronessLevelCastle.BossPossibility.Cupcake
				Me.SpawnCupcake()
			Case BaronessLevelCastle.BossPossibility.Jawbreaker
				Me.SpawnJawbreaker()
		End Select
		Yield CupheadTime.WaitForSeconds(Me, Me.setWaitTime)
		MyBase.animator.SetBool("ToCastleLoop", False)
		If Me.bossIndex < p.miniBossAmount Then
			Me.bossIndex += 1
		End If
		If MyBase.properties.CurrentState.jellybeans.startingPoint = CSng(Me.bossIndex) Then
			Me.StartJellybeans()
		End If
		If Me.bossIndex = p.miniBossAmount Then
			Me.maxMiniBosses = True
		End If
		AudioManager.Play("level_baroness_castle_gate_close")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Castle_Close", False, True)
		Me.castleOpen = False
		If MyBase.properties.CurrentState.baronessVonBonbon.miniBossStart = CSng(Me.bossIndex) Then
			Me.StartBaronessShoot()
		End If
		Me.state = BaronessLevelCastle.State.Idle
		Yield Nothing
		Return
	End Function

	' Token: 0x0600157C RID: 5500 RVA: 0x000BFA84 File Offset: 0x000BDE84
	Private Sub Blink()
		If Me.blinkCounter < Me.blinkCounterMax Then
			Me.blink.enabled = False
			Me.blinkCounter += 1
		Else
			Me.blink.enabled = True
			Me.blinkCounter = 0
			Me.blinkCounterMax = Global.UnityEngine.Random.Range(4, 7)
		End If
	End Sub

	' Token: 0x0600157D RID: 5501 RVA: 0x000BFAE4 File Offset: 0x000BDEE4
	Private Sub SpawnGumball()
		Dim position As Vector3 = Me.emergePoint.position
		position.y = Me.emergePoint.position.y + 100F
		Me.emergePoint.position = position
		Dim baronessLevelGumball As BaronessLevelGumball = Global.UnityEngine.[Object].Instantiate(Of BaronessLevelGumball)(Me.gumballPrefab)
		Dim gumball As LevelProperties.Baroness.Gumball = MyBase.properties.CurrentState.gumball
		baronessLevelGumball.Init(gumball, Me.emergePoint.position, CSng(gumball.HP))
		BaronessLevelCastle.CURRENT_MINI_BOSS = baronessLevelGumball
		BaronessLevelCastle.CURRENT_MINI_BOSS.bossId = BaronessLevelCastle.BossPossibility.Gumball
		Me.setWaitTime = 1F
		Me.emergePoint.position = Me.originalEmergePos
	End Sub

	' Token: 0x0600157E RID: 5502 RVA: 0x000BFB94 File Offset: 0x000BDF94
	Private Sub SpawnWaffle()
		Dim baronessLevelWaffle As BaronessLevelWaffle = Global.UnityEngine.[Object].Instantiate(Of BaronessLevelWaffle)(Me.wafflePrefab)
		Dim waffle As LevelProperties.Baroness.Waffle = MyBase.properties.CurrentState.waffle
		baronessLevelWaffle.Init(waffle, Me.emergePoint.position, Me.pivotPoint, waffle.movementSpeed, CSng(waffle.HP))
		BaronessLevelCastle.CURRENT_MINI_BOSS = baronessLevelWaffle
		BaronessLevelCastle.CURRENT_MINI_BOSS.bossId = BaronessLevelCastle.BossPossibility.Waffle
		Me.setWaitTime = 1F
	End Sub

	' Token: 0x0600157F RID: 5503 RVA: 0x000BFC04 File Offset: 0x000BE004
	Private Sub SpawnCandyCorn()
		Dim baronessLevelCandyCorn As BaronessLevelCandyCorn = Global.UnityEngine.[Object].Instantiate(Of BaronessLevelCandyCorn)(Me.candyCornPrefab)
		Dim candyCorn As LevelProperties.Baroness.CandyCorn = MyBase.properties.CurrentState.candyCorn
		baronessLevelCandyCorn.Init(candyCorn, New Vector3(Me.emergePoint.position.x, Me.emergePoint.position.y + 40F), candyCorn.movementSpeed, CSng(candyCorn.HP))
		BaronessLevelCastle.CURRENT_MINI_BOSS = baronessLevelCandyCorn
		BaronessLevelCastle.CURRENT_MINI_BOSS.bossId = BaronessLevelCastle.BossPossibility.CandyCorn
		Me.setWaitTime = 1F
	End Sub

	' Token: 0x06001580 RID: 5504 RVA: 0x000BFC94 File Offset: 0x000BE094
	Private Sub SpawnCupcake()
		Dim position As Vector3 = Me.emergePoint.position
		position.y = Me.emergePoint.position.y
		position.x = Me.emergePoint.position.x + 200F
		Me.emergePoint.position = position
		Dim baronessLevelCupcake As BaronessLevelCupcake = Global.UnityEngine.[Object].Instantiate(Of BaronessLevelCupcake)(Me.cupcakePrefab)
		Dim cupcake As LevelProperties.Baroness.Cupcake = MyBase.properties.CurrentState.cupcake
		baronessLevelCupcake.Init(cupcake, Me.emergePoint.position, CSng(cupcake.HP))
		BaronessLevelCastle.CURRENT_MINI_BOSS = baronessLevelCupcake
		BaronessLevelCastle.CURRENT_MINI_BOSS.bossId = BaronessLevelCastle.BossPossibility.Cupcake
		Me.setWaitTime = 1F
		Me.emergePoint.position = Me.originalEmergePos
	End Sub

	' Token: 0x06001581 RID: 5505 RVA: 0x000BFD60 File Offset: 0x000BE160
	Private Sub SpawnJawbreaker()
		Dim [next] As AbstractPlayerController = PlayerManager.GetNext()
		Dim baronessLevelJawbreaker As BaronessLevelJawbreaker = Global.UnityEngine.[Object].Instantiate(Of BaronessLevelJawbreaker)(Me.jawBreakerPrefab)
		Dim jawbreaker As LevelProperties.Baroness.Jawbreaker = MyBase.properties.CurrentState.jawbreaker
		baronessLevelJawbreaker.Init(jawbreaker, [next], New Vector3(Me.emergePoint.position.x, Me.emergePoint.position.y + 10F), jawbreaker.jawbreakerHomingRotation, CSng(jawbreaker.jawbreakerHomingHP))
		BaronessLevelCastle.CURRENT_MINI_BOSS = baronessLevelJawbreaker
		BaronessLevelCastle.CURRENT_MINI_BOSS.bossId = BaronessLevelCastle.BossPossibility.Jawbreaker
		Me.setWaitTime = 3F
		For i As Integer = 0 To jawbreaker.jawbreakerMinis - 1
			Me.setWaitTime += 0.5F
		Next
	End Sub

	' Token: 0x06001582 RID: 5506 RVA: 0x000BFE25 File Offset: 0x000BE225
	Public Sub StartBaronessShoot()
		MyBase.StartCoroutine(Me.shoot_cr())
	End Sub

	' Token: 0x06001583 RID: 5507 RVA: 0x000BFE34 File Offset: 0x000BE234
	Private Iterator Function shoot_cr() As IEnumerator
		Me.state = BaronessLevelCastle.State.Idle
		Dim p As LevelProperties.Baroness.BaronessVonBonbon = MyBase.properties.CurrentState.baronessVonBonbon
		Dim pattern As String() = p.timeString.GetRandom().Split(New Char() { ","c })
		Me.timeIndex = Global.UnityEngine.Random.Range(0, pattern.Length)
		Dim collider As Collider2D = Me.baronessPhase1.shootPoint.GetComponent(Of Collider2D)()
		While True
			Dim timeShoot As Single
			Parser.FloatTryParse(pattern(Me.timeIndex), timeShoot)
			Yield CupheadTime.WaitForSeconds(Me, timeShoot)
			Me.baronessPhase1.shotEnough = False
			If Me.castleOpen Then
				Yield MyBase.animator.WaitForAnimationToEnd(Me, "Castle_Close", False, True)
			End If
			Me.baronessPoppedUp = True
			AudioManager.Play("level_baroness_stick_head_open")
			Me.baronessPhase1.animator.Play("Baroness_Pop_Up")
			While Me.baronessPoppedUp
				collider.enabled = True
				If CSng(Me.baronessPhase1.shootCounter) >= p.attackCount.RandomFloat() OrElse Me.baronessPhase1.shotEnough Then
					Exit While
				End If
				Me.baronessPhase1.animator.SetTrigger("ToShoot")
				Yield CupheadTime.WaitForSeconds(Me, p.attackDelay)
				Yield Nothing
			End While
			Me.baronessPoppedUp = False
			collider.enabled = False
			Me.baronessPhase1.shootCounter = 0
			AudioManager.Play("level_baroness_stick_head_closed")
			Me.baronessPhase1.animator.SetTrigger("Leave")
			If Me.timeIndex < pattern.Length - 1 Then
				Me.timeIndex += 1
			Else
				Me.timeIndex = 0
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06001584 RID: 5508 RVA: 0x000BFE50 File Offset: 0x000BE250
	Private Iterator Function shoot_easy_cr() As IEnumerator
		Dim t As Single = 0F
		Me.baronessPhase1.isEasyFinal = True
		Dim p As LevelProperties.Baroness.BaronessVonBonbon = MyBase.properties.CurrentState.baronessVonBonbon
		Dim collider As Collider2D = Me.baronessPhase1.shootPoint.GetComponent(Of Collider2D)()
		Me.baronessPoppedUp = True
		AudioManager.Play("level_baroness_stick_head_open")
		Me.baronessPhase1.animator.Play("Baroness_Pop_Up")
		collider.enabled = True
		If Me.castleOpen Then
			Yield MyBase.animator.WaitForAnimationToEnd(Me, "Castle_Close", False, True)
		End If
		While Me.baronessPhase1.isEasyFinal
			Me.baronessPhase1.animator.SetTrigger("ToShoot")
			While t < p.attackDelay AndAlso Me.baronessPhase1.isEasyFinal
				t += CupheadTime.Delta
				Yield Nothing
			End While
			t = 0F
		End While
		Me.StartDeathEasy()
		Return
	End Function

	' Token: 0x06001585 RID: 5509 RVA: 0x000BFE6B File Offset: 0x000BE26B
	Public Sub StartJellybeans()
		MyBase.StartCoroutine(Me.spawnJellybeans_cr())
	End Sub

	' Token: 0x06001586 RID: 5510 RVA: 0x000BFE7C File Offset: 0x000BE27C
	Private Sub SpawnJellyBeans(prefab As BaronessLevelJellybeans)
		Dim position As Vector3 = Me.emergePoint.position
		Me.emergePoint.position = position
		position.y = Me.emergePoint.position.y - 20F
		Dim jellybeans As LevelProperties.Baroness.Jellybeans = MyBase.properties.CurrentState.jellybeans
		prefab.Create(MyBase.properties.CurrentState.jellybeans, position, jellybeans.movementSpeed, CSng(jellybeans.HP))
		Me.emergePoint.position = Me.originalEmergePos
	End Sub

	' Token: 0x06001587 RID: 5511 RVA: 0x000BFF08 File Offset: 0x000BE308
	Private Iterator Function spawnJellybeans_cr() As IEnumerator
		Dim p As LevelProperties.Baroness.Jellybeans = MyBase.properties.CurrentState.jellybeans
		Dim typePattern As String() = p.typeArray.GetRandom().Split(New Char() { ","c })
		Dim change As Single = 0F
		While Me.state <> BaronessLevelCastle.State.ChaseIntro
			For i As Integer = 0 To typePattern.Length - 1
				Dim toSpawn As BaronessLevelJellybeans = Nothing
				Dim beanSpawnDelay As Single = p.spawnDelay.RandomFloat()
				If typePattern(i)(0) = "R"c Then
					toSpawn = Me.greenJellyPrefab
				ElseIf typePattern(i)(0) = "P"c Then
					toSpawn = Me.pinkJellyPrefab
				End If
				If(BaronessLevelCastle.CURRENT_MINI_BOSS IsNot Nothing AndAlso Me.state = BaronessLevelCastle.State.Idle) OrElse Me.state = BaronessLevelCastle.State.EasyFinal Then
					Me.SpawnJellyBeans(toSpawn)
					Yield CupheadTime.WaitForSeconds(Me, beanSpawnDelay - change)
				Else
					Yield Nothing
				End If
				If Me.jellyChangeDelay Then
					change += beanSpawnDelay - beanSpawnDelay * (1F - p.spawnDelayChangePercentage / 100F)
					Me.jellyChangeDelay = False
				End If
			Next
		End While
		Return
	End Function

	' Token: 0x06001588 RID: 5512 RVA: 0x000BFF23 File Offset: 0x000BE323
	Private Sub StartDeathEasy()
		Me.StopAllCoroutines()
		Me.state = BaronessLevelCastle.State.Dead
		MyBase.StartCoroutine(Me.death_easy_cr())
	End Sub

	' Token: 0x06001589 RID: 5513 RVA: 0x000BFF40 File Offset: 0x000BE340
	Private Iterator Function death_easy_cr() As IEnumerator
		Dim offset As Single = 100F
		Dim speed As Single = 400F
		If Not Me.baronessPoppedUp Then
			Dim position As Vector3 = Me.baronessPhase1.transform.position
			position.y -= offset
			Me.baronessPhase1.transform.position = position
		End If
		Me.baronessPhase1.animator.SetTrigger("Death")
		MyBase.animator.SetTrigger("DeathEasy")
		If Me.OnDeathEvent IsNot Nothing Then
			Me.OnDeathEvent()
		End If
		MyBase.properties.WinInstantly()
		If Not Me.baronessPoppedUp Then
			While Me.baronessPhase1.transform.position <> Me.originalBaronessPoint
				Me.baronessPhase1.transform.position = Vector3.MoveTowards(Me.baronessPhase1.transform.position, Me.originalBaronessPoint, speed * CupheadTime.Delta)
				Yield Nothing
			End While
		End If
		Yield Nothing
		Return
	End Function

	' Token: 0x0600158A RID: 5514 RVA: 0x000BFF5B File Offset: 0x000BE35B
	Private Sub StartDeath()
		Me.StopAllCoroutines()
		MyBase.StartCoroutine(Me.death_cr())
	End Sub

	' Token: 0x0600158B RID: 5515 RVA: 0x000BFF70 File Offset: 0x000BE370
	Private Iterator Function death_cr() As IEnumerator
		Me.pauseScrolling = True
		Me.teethState = BaronessLevelCastle.TeethState.Off
		MyBase.animator.SetTrigger("Death")
		Dim pos As Vector3 = Me.castleWallFix.transform.position
		pos.y = -45F
		Me.castleWallFix.transform.position = pos
		Me.castleWallFix.sortingLayerName = "Background"
		Me.castleWallFix.sortingOrder = 15
		If Me.OnDeathEvent IsNot Nothing Then
			Me.OnDeathEvent()
		End If
		For Each abstractPlayerController As AbstractPlayerController In PlayerManager.GetAllPlayers()
			Dim levelPlayerController As LevelPlayerController = CType(abstractPlayerController, LevelPlayerController)
			If Not(levelPlayerController Is Nothing) Then
				If Me.scrollForce IsNot Nothing Then
					levelPlayerController.motor.RemoveForce(Me.scrollForce)
				End If
			End If
		Next
		Me.blackCastleHole.gameObject.GetComponent(Of SpriteRenderer)().enabled = False
		MyBase.animator.Play("Castle_Death")
		Yield Nothing
		Return
	End Function

	' Token: 0x0600158C RID: 5516 RVA: 0x000BFF8B File Offset: 0x000BE38B
	Public Sub StartChase()
		Me.StopAllCoroutines()
		MyBase.StartCoroutine(Me.chase_intro_cr())
	End Sub

	' Token: 0x0600158D RID: 5517 RVA: 0x000BFFA0 File Offset: 0x000BE3A0
	Private Iterator Function chase_intro_cr() As IEnumerator
		Me.baronessPhase1.transformCounter = 0
		If Me.baronessPoppedUp Then
			Me.baronessPhase1.animator.SetTrigger("Leave")
			Me.baronessPhase1.animator.WaitForAnimationToEnd(Me, "Baroness_Leave", False, True)
			Me.baronessPoppedUp = False
			Yield CupheadTime.WaitForSeconds(Me, 1F)
		End If
		Me.baronessPhase2.gameObject.SetActive(True)
		Me.baronessPhase1.gameObject.GetComponent(Of SpriteRenderer)().sortingLayerName = "Background"
		Me.baronessPhase1.gameObject.GetComponent(Of SpriteRenderer)().sortingOrder = 6
		Me.baronessPhase1.animator.Play("Baroness_To_Idle_1")
		While Me.baronessPhase1.transformCounter <= 2
			Yield Nothing
		End While
		Me.baronessPhase1.animator.SetTrigger("Continue")
		Yield Me.baronessPhase1.animator.WaitForAnimationToEnd(Me.baronessPhase1, "Baroness_Transition_1", False, True)
		Me.baronessPhase1.transformCounter = 0
		While Me.baronessPhase1.transformCounter <= 2 AndAlso Me.continueTransition
			Me.inAnimationLoop = True
			Yield Nothing
		End While
		Me.baronessPhase1.animator.SetTrigger("Continue")
		Yield Me.baronessPhase1.animator.WaitForAnimationToEnd(Me.baronessPhase1, "Baroness_Transition_2_Loop", False, True)
		Me.baronessPhase1.animator.SetTrigger("OnCandyCaneExit")
		Yield Me.baronessPhase1.animator.WaitForAnimationToEnd(Me.baronessPhase1, "Baroness_Transition_3", False, True)
		MyBase.animator.SetTrigger("StartPhase2")
		Me.baronessPhase1.transformCounter = 0
		AudioManager.Play("level_baroness_grab_castle")
		While Me.transitionCounter <= 4
			Yield Nothing
		End While
		MyBase.animator.SetTrigger("Continue")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Castle_Transition_11", False, True)
		MyBase.animator.SetTrigger("LoopArms")
		Me.castleWallFix.sortingLayerName = "Default"
		Me.baronessPhase2.gameObject.SetActive(True)
		Me.castleCollidePhase2.SetActive(True)
		Me.state = BaronessLevelCastle.State.Chase
		MyBase.StartCoroutine(Me.handle_scroll_cr())
		MyBase.StartCoroutine(Me.peppermint_cr())
		MyBase.StartCoroutine(Me.final_shoot_cr())
		Yield Nothing
		Return
	End Function

	' Token: 0x0600158E RID: 5518 RVA: 0x000BFFBB File Offset: 0x000BE3BB
	Private Sub PauseScroll()
		Me.pauseScrolling = Not Me.pauseScrolling
		Me.activateForce = Not Me.activateForce
	End Sub

	' Token: 0x0600158F RID: 5519 RVA: 0x000BFFDB File Offset: 0x000BE3DB
	Private Sub ActivateTeeth()
		MyBase.animator.Play("Castle_Chase_Arms", 2)
		Me.teethState = BaronessLevelCastle.TeethState.Idle
	End Sub

	' Token: 0x06001590 RID: 5520 RVA: 0x000BFFF5 File Offset: 0x000BE3F5
	Private Sub HitCastleFrame()
		If Me.inAnimationLoop Then
			Me.continueTransition = True
		End If
	End Sub

	' Token: 0x06001591 RID: 5521 RVA: 0x000C0009 File Offset: 0x000BE409
	Private Sub TransitionCounter()
		Me.transitionCounter += 1
	End Sub

	' Token: 0x06001592 RID: 5522 RVA: 0x000C001C File Offset: 0x000BE41C
	Private Sub SwitchLayersToDefault()
		Me.baronessPhase2.gameObject.GetComponent(Of SpriteRenderer)().sortingLayerName = "Default"
		Me.baronessPhase2.gameObject.GetComponent(Of SpriteRenderer)().sortingOrder = 0
		Me.blackCastleHole.gameObject.GetComponent(Of SpriteRenderer)().sortingLayerName = "Default"
		Me.blackCastleHole.gameObject.GetComponent(Of SpriteRenderer)().sortingOrder = 1
		MyBase.gameObject.GetComponent(Of SpriteRenderer)().sortingLayerName = "Default"
		MyBase.gameObject.GetComponent(Of SpriteRenderer)().sortingOrder = 2
		Me.castlePhase2TopLayer.gameObject.GetComponent(Of SpriteRenderer)().sortingLayerName = "Default"
		Me.castlePhase2TopLayer.gameObject.GetComponent(Of SpriteRenderer)().sortingOrder = 5
	End Sub

	' Token: 0x06001593 RID: 5523 RVA: 0x000C00E0 File Offset: 0x000BE4E0
	Private Iterator Function handle_scroll_cr() As IEnumerator
		Me.scrollForce = New LevelPlayerMotor.VelocityManager.Force(LevelPlayerMotor.VelocityManager.Force.Type.Ground, 190F)
		While True
			For Each abstractPlayerController As AbstractPlayerController In PlayerManager.GetAllPlayers()
				Dim levelPlayerController As LevelPlayerController = CType(abstractPlayerController, LevelPlayerController)
				If Not(levelPlayerController Is Nothing) Then
					If Me.distToGround < 200F AndAlso Me.activateForce Then
						levelPlayerController.motor.AddForce(Me.scrollForce)
					Else
						levelPlayerController.motor.RemoveForce(Me.scrollForce)
					End If
				End If
			Next
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06001594 RID: 5524 RVA: 0x000C00FB File Offset: 0x000BE4FB
	Private Sub HandsSound()
		AudioManager.Play("level_baroness_castle_hands")
	End Sub

	' Token: 0x06001595 RID: 5525 RVA: 0x000C0107 File Offset: 0x000BE507
	Private Sub CastleRoar()
		AudioManager.Play("level_baroness_castle_roar")
	End Sub

	' Token: 0x06001596 RID: 5526 RVA: 0x000C0113 File Offset: 0x000BE513
	Private Sub PointSound()
		AudioManager.Play("level_baroness_go_castle")
	End Sub

	' Token: 0x06001597 RID: 5527 RVA: 0x000C0120 File Offset: 0x000BE520
	Private Iterator Function peppermint_cr() As IEnumerator
		While True
			Dim seconds As Single = MyBase.properties.CurrentState.peppermint.peppermintSpawnDurationRange.RandomFloat()
			Yield CupheadTime.WaitForSeconds(Me, seconds)
			Me.teethState = BaronessLevelCastle.TeethState.StartOpen
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06001598 RID: 5528 RVA: 0x000C013B File Offset: 0x000BE53B
	Private Sub OpenTeeth()
		MyBase.StartCoroutine(Me.open_teeth_cr())
	End Sub

	' Token: 0x06001599 RID: 5529 RVA: 0x000C014C File Offset: 0x000BE54C
	Private Iterator Function open_teeth_cr() As IEnumerator
		If Me.teethState = BaronessLevelCastle.TeethState.StartOpen Then
			Me.teethState = BaronessLevelCastle.TeethState.Open
			MyBase.animator.SetBool("TeethOpen", True)
			Yield CupheadTime.WaitForSeconds(Me, 1F)
			Dim peppermint As BaronessLevelPeppermint = Global.UnityEngine.[Object].Instantiate(Of BaronessLevelPeppermint)(Me.peppermintPrefab)
			Dim p As LevelProperties.Baroness.Peppermint = MyBase.properties.CurrentState.peppermint
			peppermint.Init(Me.emergePoint.position, p.peppermintSpeed)
			Yield CupheadTime.WaitForSeconds(Me, 0.5F)
			Me.teethState = BaronessLevelCastle.TeethState.Off
		End If
		Return
	End Function

	' Token: 0x0600159A RID: 5530 RVA: 0x000C0167 File Offset: 0x000BE567
	Private Sub CloseTeeth()
		If Me.teethState = BaronessLevelCastle.TeethState.Off Then
			MyBase.animator.SetBool("TeethOpen", False)
			Me.teethState = BaronessLevelCastle.TeethState.Idle
		End If
	End Sub

	' Token: 0x0600159B RID: 5531 RVA: 0x000C018D File Offset: 0x000BE58D
	Private Sub HideTeeth()
		Me.teeth.enabled = False
	End Sub

	' Token: 0x0600159C RID: 5532 RVA: 0x000C019B File Offset: 0x000BE59B
	Private Sub ShowTeeth()
		Me.teeth.enabled = True
	End Sub

	' Token: 0x0600159D RID: 5533 RVA: 0x000C01AC File Offset: 0x000BE5AC
	Private Iterator Function final_shoot_cr() As IEnumerator
		Dim p As LevelProperties.Baroness.BaronessVonBonbon = MyBase.properties.CurrentState.baronessVonBonbon
		Dim headString As String() = p.finalProjectileHeadToss.Split(New Char() { ","c })
		Dim headIndex As Integer = Global.UnityEngine.Random.Range(0, headString.Length)
		Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.baronessVonBonbon.finalProjectileInitialDelay)
		While True
			If headString(headIndex)(0) = "H"c Then
				MyBase.animator.SetBool("Toss", True)
				Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.baronessVonBonbon.finalProjectileAttackDelayRange.RandomFloat())
			Else
				Dim delayString As String() = headString(headIndex).Split(New Char() { ":"c })
				Yield CupheadTime.WaitForSeconds(Me, Parser.FloatParse(delayString(1)))
			End If
			headIndex = (headIndex + 1) Mod headString.Length
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600159E RID: 5534 RVA: 0x000C01C7 File Offset: 0x000BE5C7
	Private Sub FireHead()
		Me.baronessPhase1.FireFinalProjectile()
		MyBase.animator.SetBool("Toss", False)
	End Sub

	' Token: 0x04001EBD RID: 7869
	Public pauseScrolling As Boolean

	' Token: 0x04001EC1 RID: 7873
	Private homingRoots As Transform()

	' Token: 0x04001EC2 RID: 7874
	<SerializeField()>
	Private teeth As SpriteRenderer

	' Token: 0x04001EC3 RID: 7875
	<SerializeField()>
	Private blink As SpriteRenderer

	' Token: 0x04001EC4 RID: 7876
	<SerializeField()>
	Private platform As BaronessLevelPlatform

	' Token: 0x04001EC5 RID: 7877
	<SerializeField()>
	Private baronessPhase1 As BaronessLevelBaroness

	' Token: 0x04001EC6 RID: 7878
	<SerializeField()>
	Private baronessPhase2 As Transform

	' Token: 0x04001EC7 RID: 7879
	<SerializeField()>
	Private peppermintPrefab As BaronessLevelPeppermint

	' Token: 0x04001EC8 RID: 7880
	<SerializeField()>
	Private blackCastleHole As Transform

	' Token: 0x04001EC9 RID: 7881
	<SerializeField()>
	Private castlePhase2TopLayer As Transform

	' Token: 0x04001ECA RID: 7882
	<SerializeField()>
	Private cupcakePrefab As BaronessLevelCupcake

	' Token: 0x04001ECB RID: 7883
	<SerializeField()>
	Private wafflePrefab As BaronessLevelWaffle

	' Token: 0x04001ECC RID: 7884
	<SerializeField()>
	Private gumballPrefab As BaronessLevelGumball

	' Token: 0x04001ECD RID: 7885
	<SerializeField()>
	Private jawBreakerPrefab As BaronessLevelJawbreaker

	' Token: 0x04001ECE RID: 7886
	<SerializeField()>
	Private candyCornPrefab As BaronessLevelCandyCorn

	' Token: 0x04001ECF RID: 7887
	<SerializeField()>
	Private greenJellyPrefab As BaronessLevelJellybeans

	' Token: 0x04001ED0 RID: 7888
	<SerializeField()>
	Private pinkJellyPrefab As BaronessLevelJellybeans

	' Token: 0x04001ED1 RID: 7889
	<SerializeField()>
	Private emergePoint As Transform

	' Token: 0x04001ED2 RID: 7890
	<SerializeField()>
	Private pivotPoint As Transform

	' Token: 0x04001ED3 RID: 7891
	<SerializeField()>
	Private castleCollidePhase2 As GameObject

	' Token: 0x04001ED4 RID: 7892
	<SerializeField()>
	Private castleWallFix As SpriteRenderer

	' Token: 0x04001ED5 RID: 7893
	Private bossIndex As Integer

	' Token: 0x04001ED6 RID: 7894
	Private timeIndex As Integer

	' Token: 0x04001ED7 RID: 7895
	Private transitionCounter As Integer

	' Token: 0x04001ED8 RID: 7896
	Private blinkCounter As Integer

	' Token: 0x04001ED9 RID: 7897
	Private blinkCounterMax As Integer

	' Token: 0x04001EDA RID: 7898
	Private setWaitTime As Single

	' Token: 0x04001EDB RID: 7899
	Private distToGround As Single

	' Token: 0x04001EDC RID: 7900
	Private maxMiniBosses As Boolean

	' Token: 0x04001EDD RID: 7901
	Private castleOpen As Boolean

	' Token: 0x04001EDE RID: 7902
	Private baronessPoppedUp As Boolean

	' Token: 0x04001EDF RID: 7903
	Private continueTransition As Boolean

	' Token: 0x04001EE0 RID: 7904
	Private inAnimationLoop As Boolean

	' Token: 0x04001EE1 RID: 7905
	Private jellyChangeDelay As Boolean

	' Token: 0x04001EE2 RID: 7906
	Private openTeeth As Boolean

	' Token: 0x04001EE3 RID: 7907
	Private activateForce As Boolean = True

	' Token: 0x04001EE4 RID: 7908
	Private originalEmergePos As Vector3

	' Token: 0x04001EE5 RID: 7909
	Private originalBaronessPoint As Vector3

	' Token: 0x04001EE6 RID: 7910
	Private player As AbstractPlayerController

	' Token: 0x04001EE7 RID: 7911
	Private scrollForce As LevelPlayerMotor.VelocityManager.Force

	' Token: 0x04001EE8 RID: 7912
	Private damageReceiver As DamageReceiver

	' Token: 0x04001EE9 RID: 7913
	Private damageDealer As DamageDealer

	' Token: 0x020004E0 RID: 1248
	Public Enum State
		' Token: 0x04001EEB RID: 7915
		Intro
		' Token: 0x04001EEC RID: 7916
		Idle
		' Token: 0x04001EED RID: 7917
		ChaseIntro
		' Token: 0x04001EEE RID: 7918
		Chase
		' Token: 0x04001EEF RID: 7919
		Open
		' Token: 0x04001EF0 RID: 7920
		Dead
		' Token: 0x04001EF1 RID: 7921
		EasyFinal
	End Enum

	' Token: 0x020004E1 RID: 1249
	Public Enum TeethState
		' Token: 0x04001EF3 RID: 7923
		Unspawned
		' Token: 0x04001EF4 RID: 7924
		Idle
		' Token: 0x04001EF5 RID: 7925
		Off
		' Token: 0x04001EF6 RID: 7926
		StartOpen
		' Token: 0x04001EF7 RID: 7927
		Open
	End Enum

	' Token: 0x020004E2 RID: 1250
	Public Enum BossPossibility
		' Token: 0x04001EF9 RID: 7929
		Gumball = 1
		' Token: 0x04001EFA RID: 7930
		Waffle
		' Token: 0x04001EFB RID: 7931
		CandyCorn
		' Token: 0x04001EFC RID: 7932
		Cupcake
		' Token: 0x04001EFD RID: 7933
		Jawbreaker
	End Enum
End Class
