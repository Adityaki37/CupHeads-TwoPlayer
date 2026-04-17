Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x0200064F RID: 1615
Public Class FlyingCowboyLevelCowboy
	Inherits LevelProperties.FlyingCowboy.Entity

	' Token: 0x1700038C RID: 908
	' (get) Token: 0x0600213D RID: 8509 RVA: 0x001334EC File Offset: 0x001318EC
	' (set) Token: 0x0600213E RID: 8510 RVA: 0x001334F4 File Offset: 0x001318F4
	Public Property state As FlyingCowboyLevelCowboy.State

	' Token: 0x1700038D RID: 909
	' (get) Token: 0x0600213F RID: 8511 RVA: 0x001334FD File Offset: 0x001318FD
	' (set) Token: 0x06002140 RID: 8512 RVA: 0x00133505 File Offset: 0x00131905
	Public Property IsDead As Boolean

	' Token: 0x1700038E RID: 910
	' (get) Token: 0x06002141 RID: 8513 RVA: 0x0013350E File Offset: 0x0013190E
	' (set) Token: 0x06002142 RID: 8514 RVA: 0x00133516 File Offset: 0x00131916
	Public Property onBottom As Boolean

	' Token: 0x06002143 RID: 8515 RVA: 0x0013351F File Offset: 0x0013191F
	Private Sub OnEnable()
		AddHandler SceneLoader.OnFadeOutStartEvent, AddressOf Me.onFadeOutStartEvent
		AddHandler PlayerManager.OnPlayerJoinedEvent, AddressOf Me.onPlayerJoinedEvent
	End Sub

	' Token: 0x06002144 RID: 8516 RVA: 0x00133543 File Offset: 0x00131943
	Private Sub OnDisable()
		RemoveHandler SceneLoader.OnFadeOutStartEvent, AddressOf Me.onFadeOutStartEvent
		RemoveHandler PlayerManager.OnPlayerJoinedEvent, AddressOf Me.onPlayerJoinedEvent
	End Sub

	' Token: 0x06002145 RID: 8517 RVA: 0x00133568 File Offset: 0x00131968
	Private Sub Start()
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		Me.allDebris = New List(Of FlyingCowboyLevelDebris)()
		Me.topPositions = New Vector3(5) {}
		Me.sidePositions = New Vector3(5) {}
		Me.bottomPositions = New Vector3(5) {}
		Me.topCurvePositions = New Vector3(3) {}
		Me.bottomCurvePositions = New Vector3(3) {}
		Dim currentState As LevelProperties.FlyingCowboy.State = MyBase.properties.CurrentState
		Me.debrisCurveString = New PatternString(currentState.debris.debrisCurveShotString, True, True)
		Me.debrisParryString = New PatternString(currentState.debris.debrisParryString, True)
		Me.ricochetParryString = New PatternString(currentState.ricochet.splitParryString, True)
		Me.backshotHighSpawnPosition = New PatternString(currentState.backshotEnemy.highSpawnPosition, True, True)
		Me.backshotLowSpawnPosition = New PatternString(currentState.backshotEnemy.lowSpawnPosition, True, True)
		Me.backshotSpawnDelay = New PatternString(currentState.backshotEnemy.spawnDelay, True, True)
		Me.backshotBulletParryable = New PatternString(currentState.backshotEnemy.bulletParryString, True)
		Me.backshotAnticipationStartDistancePattern = New PatternString(currentState.backshotEnemy.anticipationStartDistance, True, True)
		Me.SetupDebrisSpawnPoints()
		MyBase.StartCoroutine(Me.wobble_cr())
		Me.introBird = Me.birdPrefab.Spawn(Me.birdEndPosition.position)
		Me.introBird.InitializeIntro(Me.birdEndPosition.position)
		Me.SFX_COWGIRL_COWGIRL_WheelConstantLoop()
	End Sub

	' Token: 0x06002146 RID: 8518 RVA: 0x00133700 File Offset: 0x00131B00
	Private Sub Update()
		If Me.forcePlayer1 IsNot Nothing Then
			Me.forcePlayer1.UpdateStrength(CupheadTime.Delta)
		End If
		If Me.forcePlayer2 IsNot Nothing Then
			Me.forcePlayer2.UpdateStrength(CupheadTime.Delta)
		End If
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06002147 RID: 8519 RVA: 0x00133764 File Offset: 0x00131B64
	Private Sub SetupDebrisSpawnPoints()
		Me.sidePositions = New Vector3(5) {}
		Dim num As Single = (Me.vacuumSpawnTop.position.y - Me.vacuumSpawnBottom.position.y) / 5F
		For i As Integer = 0 To 6 - 1
			Dim x As Single = Me.vacuumSpawnBottom.position.x
			Dim num2 As Single = If((i <> 5), (Me.vacuumSpawnBottom.position.y + num * CSng(i)), Me.vacuumSpawnTop.position.y)
			Me.sidePositions(i) = New Vector3(x, num2)
		Next
		Dim num3 As Single = (CSng(Me.debrisSpawnHorizontalSpacing) - Me.vacuumSpawnTop.position.x) / 6F
		For j As Integer = 0 To 6 - 1
			Dim num4 As Single = Me.vacuumSpawnTop.position.x + num3 + num3 * CSng(j)
			Dim y As Single = Me.vacuumSpawnTop.position.y
			Me.topPositions(j) = New Vector3(num4, y)
		Next
		For k As Integer = 0 To 4 - 1
			Dim num5 As Single = Me.vacuumSpawnTop.position.x + num3 + num3 * CSng((6 + k))
			Dim y2 As Single = Me.vacuumSpawnTop.position.y
			Me.topCurvePositions(k) = New Vector3(num5, y2)
		Next
		For l As Integer = 0 To 6 - 1
			Dim num6 As Single = Me.vacuumSpawnBottom.position.x + num3 + num3 * CSng(l)
			Dim y3 As Single = Me.vacuumSpawnBottom.position.y
			Me.bottomPositions(l) = New Vector3(num6, y3)
		Next
		For m As Integer = 0 To 4 - 1
			Dim num7 As Single = Me.vacuumSpawnTop.position.x + num3 + num3 * CSng((6 + m))
			Dim y4 As Single = Me.vacuumSpawnTop.position.y
			Me.topCurvePositions(m) = New Vector3(num7, y4)
		Next
		For n As Integer = 0 To 4 - 1
			Dim num8 As Single = Me.vacuumSpawnBottom.position.x + num3 + num3 * CSng((6 + n))
			Dim y5 As Single = Me.vacuumSpawnBottom.position.y
			Me.bottomCurvePositions(n) = New Vector3(num8, y5)
		Next
	End Sub

	' Token: 0x06002148 RID: 8520 RVA: 0x00133A58 File Offset: 0x00131E58
	Public Overrides Sub LevelInit(properties As LevelProperties.FlyingCowboy)
		MyBase.LevelInit(properties)
		AddHandler Level.Current.OnIntroEvent, AddressOf Me.onIntroEventHandler
		Me.snakeOilShotsPerAttackString = New PatternString(properties.CurrentState.snakeAttack.shotsPerAttack, True, True)
		Me.snakeOffsetString = New PatternString(properties.CurrentState.snakeAttack.snakeOffsetString, True, True)
		Me.snakeWidthString = New PatternString(properties.CurrentState.snakeAttack.snakeWidthString, True, True)
		Me.debrisTopMainIndex = Global.UnityEngine.Random.Range(0, properties.CurrentState.debris.debrisTopSpawn.Length)
		Me.debrisBottomMainIndex = Global.UnityEngine.Random.Range(0, properties.CurrentState.debris.debrisBottomSpawn.Length)
		Me.debrisSideMainIndex = Global.UnityEngine.Random.Range(0, properties.CurrentState.debris.debrisSideSpawn.Length)
		Me.initialSaloonPosition = MyBase.transform.position
		Me.state = FlyingCowboyLevelCowboy.State.Idle
	End Sub

	' Token: 0x06002149 RID: 8521 RVA: 0x00133B4B File Offset: 0x00131F4B
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		MyBase.properties.DealDamage(info.damage)
	End Sub

	' Token: 0x0600214A RID: 8522 RVA: 0x00133B5E File Offset: 0x00131F5E
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x0600214B RID: 8523 RVA: 0x00133B7C File Offset: 0x00131F7C
	Private Sub onIntroEventHandler()
		MyBase.StartCoroutine(Me.intro_cr())
	End Sub

	' Token: 0x0600214C RID: 8524 RVA: 0x00133B8C File Offset: 0x00131F8C
	Private Iterator Function intro_cr() As IEnumerator
		MyBase.StartCoroutine(Me.introBird_cr(0F))
		Yield CupheadTime.WaitForSeconds(Me, 0.25F)
		MyBase.animator.Play("Intro", 0)
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Intro", 0, False, True)
		MyBase.StartCoroutine(Me.main_cr())
		Return
	End Function

	' Token: 0x0600214D RID: 8525 RVA: 0x00133BA8 File Offset: 0x00131FA8
	Private Iterator Function introBird_cr(time As Single) As IEnumerator
		If Me.introBirdTriggered Then
			Return
		End If
		Me.introBirdTriggered = True
		Yield CupheadTime.WaitForSeconds(Me, time)
		Me.introBird.MoveIntro(Me.birdStartPosition.position, MyBase.properties.CurrentState.bird)
		Me.introBird = Nothing
		Return
	End Function

	' Token: 0x0600214E RID: 8526 RVA: 0x00133BCC File Offset: 0x00131FCC
	Private Iterator Function main_cr() As IEnumerator
		Dim p As LevelProperties.FlyingCowboy.Cart = MyBase.properties.CurrentState.cart
		Dim pattern As PatternString = New PatternString(p.cartAttackString, True, True)
		MyBase.StartCoroutine(Me.spawnBirdEnemies_cr())
		While pattern.GetString() <> "S"
			pattern.PopString()
			Yield Nothing
		End While
		While True
			While Me.state <> FlyingCowboyLevelCowboy.State.Idle OrElse Me.phase2Trigger
				Yield Nothing
			End While
			Yield Nothing
			Dim [string] As String = pattern.GetString()
			If [string] IsNot Nothing Then
				If Not([string] = "M") Then
					If Not([string] = "S") Then
						If [string] = "B" Then
							MyBase.StartCoroutine(Me.beamAttack_cr())
							MyBase.StartCoroutine(Me.spawnBackshotEnemy_cr())
						End If
					Else
						MyBase.StartCoroutine(Me.snakeAttack_cr())
						MyBase.StartCoroutine(Me.spawnBackshotEnemy_cr())
					End If
				Else
					MyBase.StartCoroutine(Me.wait_cr())
				End If
			End If
			pattern.PopString()
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600214F RID: 8527 RVA: 0x00133BE8 File Offset: 0x00131FE8
	Private Iterator Function breakableRecoveryPhase1_cr(duration As Single) As IEnumerator
		Dim t As Single = 0F
		While t < duration AndAlso Not Me.phase2Trigger
			t += CupheadTime.Delta
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06002150 RID: 8528 RVA: 0x00133C0C File Offset: 0x0013200C
	Private Iterator Function wobble_cr() As IEnumerator
		While True
			Me.wobbleTimeElapsed.x = Me.wobbleTimeElapsed.x + CupheadTime.Delta
			Me.wobbleTimeElapsed.y = Me.wobbleTimeElapsed.y + CupheadTime.Delta
			If Me.wobbleTimeElapsed.x >= 2F * Me.wobbleDuration.x Then
				Me.wobbleTimeElapsed.x = Me.wobbleTimeElapsed.x - 2F * Me.wobbleDuration.x
			End If
			Dim tx As Single
			If Me.wobbleTimeElapsed.x > Me.wobbleDuration.x Then
				tx = 1F - (Me.wobbleTimeElapsed.x - Me.wobbleDuration.x) / Me.wobbleDuration.x
			Else
				tx = Me.wobbleTimeElapsed.x / Me.wobbleDuration.x
			End If
			If Me.wobbleTimeElapsed.y >= 2F * Me.wobbleDuration.y Then
				Me.wobbleTimeElapsed.y = Me.wobbleTimeElapsed.y - 2F * Me.wobbleDuration.y
			End If
			Dim ty As Single
			If Me.wobbleTimeElapsed.y > Me.wobbleDuration.y Then
				ty = 1F - (Me.wobbleTimeElapsed.y - Me.wobbleDuration.y) / Me.wobbleDuration.y
			Else
				ty = Me.wobbleTimeElapsed.y / Me.wobbleDuration.y
			End If
			Dim position As Vector3 = Me.initialSaloonPosition
			position.x += EaseUtils.EaseInOutSine(Me.wobbleRadius.x, -Me.wobbleRadius.x, tx)
			position.y += EaseUtils.EaseInOutSine(Me.wobbleRadius.y, -Me.wobbleRadius.y, ty)
			MyBase.transform.position = position
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06002151 RID: 8529 RVA: 0x00133C28 File Offset: 0x00132028
	Private Iterator Function wait_cr() As IEnumerator
		Me.state = FlyingCowboyLevelCowboy.State.Wait
		Dim p As LevelProperties.FlyingCowboy.Cart = MyBase.properties.CurrentState.cart
		MyBase.animator.SetBool("OnHide", True)
		Dim animationBaseName As String = If((Not Me.onBottom), "HideToLow", "HideToHigh")
		Yield MyBase.animator.WaitForNormalizedTime(Me, 1F, animationBaseName + "Start", 0, False, False, True)
		MyBase.animator.Play(If((Not Me.onBottom), "ToOpen", "ToClosed"), FlyingCowboyLevelCowboy.DoorsAnimatorLayer)
		Yield CupheadTime.WaitForSeconds(Me, p.cartPopinTime)
		Me.onBottom = Not Me.onBottom
		MyBase.animator.SetBool("IsLow", Me.onBottom)
		Yield MyBase.animator.WaitForAnimationToStart(Me, animationBaseName + "End", False)
		MyBase.animator.SetBool("OnHide", False)
		Yield MyBase.animator.WaitForAnimationToEnd(Me, animationBaseName + "End", False, True)
		Me.state = FlyingCowboyLevelCowboy.State.Idle
		Return
	End Function

	' Token: 0x06002152 RID: 8530 RVA: 0x00133C44 File Offset: 0x00132044
	Private Iterator Function snakeAttack_cr() As IEnumerator
		Dim p As LevelProperties.FlyingCowboy.SnakeAttack = MyBase.properties.CurrentState.snakeAttack
		Me.state = FlyingCowboyLevelCowboy.State.SnakeAttack
		Dim animationPrefix As String = "SnakeOil" + If((Not Me.onBottom), "_High", "_Low") + "."
		MyBase.animator.SetTrigger("OnSnakeOil")
		MyBase.animator.SetBool("SnakeInitialDelay", False)
		Dim shotsPerAttack As Integer = Me.snakeOilShotsPerAttackString.PopInt()
		For shotCount As Integer = 0 To shotsPerAttack - 1
			If Me.phase2Trigger AndAlso shotCount > 0 Then
				Exit For
			End If
			If shotCount > 0 Then
				Yield MyBase.animator.WaitForAnimationToEnd(Me, animationPrefix + "SnakeOilShoot", False, True)
				Yield CupheadTime.WaitForSeconds(Me, p.attackDelay)
				MyBase.animator.SetTrigger("OnSnakeShoot")
			End If
			Yield MyBase.animator.WaitForAnimationToStart(Me, animationPrefix + "SnakeOilShoot", False)
		Next
		MyBase.animator.SetTrigger("OnSnakeEnd")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, animationPrefix + "SnakeOilExit", False, True)
		Yield MyBase.StartCoroutine(Me.breakableRecoveryPhase1_cr(p.attackRecovery))
		Me.state = FlyingCowboyLevelCowboy.State.Idle
		Return
	End Function

	' Token: 0x06002153 RID: 8531 RVA: 0x00133C60 File Offset: 0x00132060
	Private Sub animationEvent_SnakeShoot()
		Dim snakeAttack As LevelProperties.FlyingCowboy.SnakeAttack = MyBase.properties.CurrentState.snakeAttack
		Dim num As Single = Me.snakeOffsetString.PopFloat()
		Dim num2 As Single = Me.snakeWidthString.PopFloat()
		Dim [next] As AbstractPlayerController = PlayerManager.GetNext()
		Dim num3 As Single = 640F - snakeAttack.breakLinePosition
		Dim num4 As Single = [next].transform.position.y + num
		For i As Integer = 0 To 2 - 1
			Dim num5 As Single = If((i <> 0), (-num2), num2)
			Dim num6 As Single = num4 + num5
			Dim num7 As Single = If((num6 <= 0F), If((num6 <= -360F), (-340F), num6), If((num6 >= 360F), 340F, num6))
			Dim vector As Vector3 = If((Not Me.onBottom), Me.snakeTopRoot(i).position, Me.snakeBottomRoot(i).position)
			Me.snakeOilMuzzleFXPrefab.Create(vector)
			Me.oilBlobPrefab.Create(vector, num7, num3, snakeAttack, i = 0)
		Next
	End Sub

	' Token: 0x06002154 RID: 8532 RVA: 0x00133D88 File Offset: 0x00132188
	Private Iterator Function beamAttack_cr() As IEnumerator
		Dim p As LevelProperties.FlyingCowboy.BeamAttack = MyBase.properties.CurrentState.beamAttack
		Me.state = FlyingCowboyLevelCowboy.State.BeamAttack
		Me.cactus.SetActive(True)
		Dim prefix As String = If((Not Me.onBottom), "Cactus_High.", "Cactus_Low.")
		MyBase.animator.SetTrigger("OnCactus")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, prefix + "Intro", False, True)
		Yield CupheadTime.WaitForSeconds(Me, p.beamWarningTime)
		MyBase.animator.SetTrigger("EndLasso")
		Me.SFX_COWGIRL_COWGIRL_LassoSpinLoopStop()
		Yield MyBase.animator.WaitForAnimationToStart(Me, prefix + "Hold", False)
		Yield CupheadTime.WaitForSeconds(Me, p.beamDuration)
		MyBase.animator.SetTrigger("EndCactusHold")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, prefix + "End", False, True)
		Me.cactus.SetActive(False)
		Yield MyBase.StartCoroutine(Me.breakableRecoveryPhase1_cr(p.attackRecovery))
		Me.state = FlyingCowboyLevelCowboy.State.Idle
		Return
	End Function

	' Token: 0x06002155 RID: 8533 RVA: 0x00133DA4 File Offset: 0x001321A4
	Private Iterator Function spawnBackshotEnemy_cr() As IEnumerator
		Dim p As LevelProperties.FlyingCowboy.BackshotEnemy = MyBase.properties.CurrentState.backshotEnemy
		Yield CupheadTime.WaitForSeconds(Me, Me.backshotSpawnDelay.PopFloat())
		Dim positionY As Single = If((Not Me.onBottom), Me.backshotLowSpawnPosition.PopFloat(), Me.backshotHighSpawnPosition.PopFloat())
		Dim position As Vector3 = New Vector3(740F, positionY)
		Me.backshotPrefab.Create(position, 180F, p.enemySpeed, p.bulletSpeed, p.enemyHealth, Me.backshotAnticipationStartDistancePattern.PopFloat(), Me.backshotBulletParryable.PopLetter() = "P"c)
		Return
	End Function

	' Token: 0x06002156 RID: 8534 RVA: 0x00133DC0 File Offset: 0x001321C0
	Private Iterator Function spawnBirdEnemies_cr() As IEnumerator
		Dim p As LevelProperties.FlyingCowboy.Bird = MyBase.properties.CurrentState.bird
		Dim bulletLandingPositionPattern As PatternString = New PatternString(p.bulletLandingPosition, True, True)
		While True
			Yield CupheadTime.WaitForSeconds(Me, p.spawnDelayRange.RandomFloat())
			Dim canSpawn As Boolean = False
			Dim safetyTimer As Single = 0F
			While Not canSpawn
				Dim found As Boolean = False
				For Each abstractPlayerController As AbstractPlayerController In PlayerManager.GetAllPlayers()
					If abstractPlayerController IsNot Nothing AndAlso Me.birdSafetyZone.Contains(abstractPlayerController.center) Then
						found = True
						safetyTimer += CupheadTime.Delta
						Exit For
					End If
				Next
				If found AndAlso safetyTimer < p.safetyZoneMaxDuration Then
					Yield Nothing
				Else
					canSpawn = True
				End If
			End While
			Dim bird As FlyingCowboyLevelBird = Me.birdPrefab.Spawn(Me.birdStartPosition.position)
			bird.Initialize(Me.birdStartPosition.position, Me.birdEndPosition.position, bulletLandingPositionPattern.PopFloat(), p, Me)
			While bird IsNot Nothing
				Yield Nothing
			End While
		End While
		Return
	End Function

	' Token: 0x06002157 RID: 8535 RVA: 0x00133DDC File Offset: 0x001321DC
	Private Sub SpawnUFOs()
		Dim uFOEnemy As LevelProperties.FlyingCowboy.UFOEnemy = MyBase.properties.CurrentState.uFOEnemy
		Dim vector As Vector3 = New Vector3(740F, uFOEnemy.topUFOVerticalPosition)
		Me.ufo = Me.ufoPrefab.Spawn()
		Me.ufo.Init(vector, MyBase.properties.CurrentState.uFOEnemy, uFOEnemy.UFOHealth)
	End Sub

	' Token: 0x06002158 RID: 8536 RVA: 0x00133E40 File Offset: 0x00132240
	Public Sub OnPhase2(postTransitionPattern As LevelProperties.FlyingCowboy.Pattern)
		Me.phase2Trigger = True
		MyBase.animator.SetBool("OnPhase2", True)
		MyBase.StartCoroutine(Me.phase2TransStart_cr(postTransitionPattern))
	End Sub

	' Token: 0x06002159 RID: 8537 RVA: 0x00133E68 File Offset: 0x00132268
	Private Iterator Function phase2TransStart_cr(postTransitionPattern As LevelProperties.FlyingCowboy.Pattern) As IEnumerator
		Dim hash As Integer = Animator.StringToHash("HideToLowStart")
		Dim hash2 As Integer = Animator.StringToHash("HideToHighStart")
		While True
			Dim hash3 As Integer = MyBase.animator.GetCurrentAnimatorStateInfo(0).shortNameHash
			If hash3 = hash OrElse hash3 = hash2 Then
				Exit For
			End If
			Yield Nothing
		End While
		Dim previousT As Single = Single.MaxValue
		Dim waitForEndOfFrame As WaitForEndOfFrame = New WaitForEndOfFrame()
		While True
			Yield waitForEndOfFrame
			Dim t As Single = MathUtilities.DecimalPart(MyBase.animator.GetCurrentAnimatorStateInfo(FlyingCowboyLevelCowboy.SaloonAnimatorLayer).normalizedTime)
			If previousT < 0.041666668F AndAlso t > 0.041666668F Then
				Exit For
			End If
			If previousT < 0.5416667F AndAlso t > 0.5416667F Then
				GoTo Block_5
			End If
			previousT = t
		End While
		Me.lanternARenderer.enabled = False
		Me.lanternBRenderer.enabled = True
		GoTo IL_01A8
		Block_5:
		Me.lanternARenderer.enabled = True
		Me.lanternBRenderer.enabled = False
		IL_01A8:
		MyBase.animator.Play("Ph1_To_Ph2", 0)
		Me.StopAllCoroutines()
		If Me.ufo IsNot Nothing Then
			Me.ufo.Dead()
		End If
		Me.state = FlyingCowboyLevelCowboy.State.PhaseTrans
		MyBase.StartCoroutine(Me.phase2_trans_cr(postTransitionPattern))
		Return
	End Function

	' Token: 0x0600215A RID: 8538 RVA: 0x00133E8C File Offset: 0x0013228C
	Private Iterator Function phase2_trans_cr(postTransitionPattern As LevelProperties.FlyingCowboy.Pattern) As IEnumerator
		Yield Nothing
		Yield MyBase.animator.WaitForNormalizedTime(Me, 0.8666667F, "Ph1_To_Ph2", 0, False, False, True)
		Me.SFX_COWGIRL_COWGIRL_WheelConstantLoopStop()
		Me.Vacuum(False, postTransitionPattern)
		Yield MyBase.animator.WaitForNormalizedTime(Me, 1F, "Ph1_To_Ph2", 0, False, False, True)
		MyBase.animator.Play("Vacuum", 0)
		MyBase.animator.Play("TransitionSmoke", FlyingCowboyLevelCowboy.TransitionSmokeLayer)
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "TransitionSmoke", FlyingCowboyLevelCowboy.TransitionSmokeLayer, False, True)
		Me.endTransitionTrigger = True
		While Me.transitionVacuumAttackCoroutine IsNot Nothing
			Yield Nothing
		End While
		If postTransitionPattern <> LevelProperties.FlyingCowboy.Pattern.Vacuum OrElse Me.phase3Trigger Then
			Me.endVacuumPullPlayer()
		End If
		Me.endTransitionTrigger = False
		Yield Nothing
		Yield Nothing
		Me.state = FlyingCowboyLevelCowboy.State.Idle
		Return
	End Function

	' Token: 0x0600215B RID: 8539 RVA: 0x00133EB0 File Offset: 0x001322B0
	Private Iterator Function moveDown_cr() As IEnumerator
		Me.phase2BasePosition = Me.initialSaloonPosition
		Me.phase2BasePosition.y = -183F
		Dim initialPosition As Vector3 = MyBase.transform.position
		Dim targetPosition As Vector3 = Me.phase2BasePosition
		Dim elapsedTime As Single = 0F
		While elapsedTime < 2F
			Yield Nothing
			elapsedTime += CupheadTime.Delta
			MyBase.transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / 2F)
		End While
		Return
	End Function

	' Token: 0x0600215C RID: 8540 RVA: 0x00133ECC File Offset: 0x001322CC
	Public Sub Vacuum(initial As Boolean, Optional postTransitionPattern As LevelProperties.FlyingCowboy.Pattern = LevelProperties.FlyingCowboy.Pattern.[Default])
		MyBase.animator.SetBool("OnRicochet", False)
		If postTransitionPattern <> LevelProperties.FlyingCowboy.Pattern.[Default] Then
			Me.transitionVacuumAttackCoroutine = MyBase.StartCoroutine(Me.vacuum_cr(initial, postTransitionPattern))
		Else
			MyBase.StartCoroutine(Me.vacuum_cr(initial, postTransitionPattern))
		End If
	End Sub

	' Token: 0x0600215D RID: 8541 RVA: 0x00133F18 File Offset: 0x00132318
	Private Iterator Function vacuumCurveShots_cr(transition As Boolean) As IEnumerator
		Dim p As LevelProperties.FlyingCowboy.Debris = MyBase.properties.CurrentState.debris
		Dim debrisCurveShotString As String()
		If transition Then
			Dim patternString As PatternString = New PatternString(p.transitionCurveShotString, True, True)
			debrisCurveShotString = patternString.PopString().Split(New Char() { ","c })
		Else
			debrisCurveShotString = Me.debrisCurveString.PopString().Split(New Char() { ","c })
		End If
		Dim positions As Vector3() = Me.topPositions
		Dim spawnIndex As Integer = 0
		Dim angle As Single = 0F
		Dim root As Vector3 = Me.vacuumDebrisAimTransform.position
		For i As Integer = 0 To debrisCurveShotString.Length - 1
			Dim spawn As String() = debrisCurveShotString(i).Split(New Char() { ":"c })
			For Each text As String In spawn
				If text = "B" Then
					positions = Me.bottomCurvePositions
				ElseIf text = "T" Then
					positions = Me.topCurvePositions
				Else
					Parser.IntTryParse(text, spawnIndex)
				End If
			Next
			Dim apexHeight As Single = Mathf.Abs(Me.vacuumDebrisAimTransform.position.x - positions(spawnIndex).x) + 300F
			Dim timeToApex As Single = p.debrisCurveApexTime
			Dim height As Single = -apexHeight
			Dim apexTime2 As Single = timeToApex * timeToApex
			Dim g As Single = -2F * height / apexTime2
			Dim viX As Single = 2F * height / timeToApex
			Dim viY2 As Single = viX * viX
			Dim x As Single = root.x - positions(spawnIndex).x
			Dim y As Single = root.y - positions(spawnIndex).y
			Dim sqrtRooted As Single = viY2 + 2F * g * x
			Dim tEnd As Single = (-viX + Mathf.Sqrt(sqrtRooted)) / g
			Dim tEnd2 As Single = (-viX - Mathf.Sqrt(sqrtRooted)) / g
			Dim tEnd3 As Single = Mathf.Max(tEnd, tEnd2)
			Dim velocityY As Single = y / tEnd3
			Dim debris As FlyingCowboyLevelDebris = TryCast(Me.largeVacuumDebrisPrefabs.GetRandom().Create(positions(spawnIndex), angle * 57.29578F, p.debrisOneSpeedStartEnd.min), FlyingCowboyLevelDebris)
			debris.GetComponent(Of SpriteRenderer)().sortingOrder = i
			Dim parryable As Boolean = Me.debrisParryString.PopLetter() = "P"c
			debris.SetParryable(parryable)
			Dim velocity As Vector3 = New Vector3(viX, velocityY)
			debris.ToCurve(velocity, g)
			debris.SetupVacuum(Me.vacuumDebrisAimTransform, Me.vacuumDebrisDisappearTransform)
			Me.allDebris.Add(debris)
			Yield CupheadTime.WaitForSeconds(Me, p.debrisDelay)
			If Me.phase3Trigger OrElse Me.endTransitionTrigger Then
				Exit For
			End If
		Next
		Return
	End Function

	' Token: 0x0600215E RID: 8542 RVA: 0x00133F3C File Offset: 0x0013233C
	Private Iterator Function vacuum_cr(initial As Boolean, postTransitionPattern As LevelProperties.FlyingCowboy.Pattern) As IEnumerator
		Dim transition As Boolean = postTransitionPattern <> LevelProperties.FlyingCowboy.Pattern.[Default]
		If Not initial Then
			Me.SFX_COWGIRL_COWGIRL_P2_VacuumSuckLoop()
		End If
		If Not transition Then
			Me.state = FlyingCowboyLevelCowboy.State.Vacuum
		End If
		Dim p As LevelProperties.FlyingCowboy.Debris = MyBase.properties.CurrentState.debris
		If Not initial AndAlso Not transition Then
			Me.startVacuumPullPlayer(False)
		End If
		If Not initial AndAlso Not transition Then
			Yield CupheadTime.WaitForSeconds(Me, p.warningDelayRange.RandomFloat())
		End If
		Dim debrisTypePattern As PatternString = New PatternString(p.debrisTypeString, True)
		MyBase.StartCoroutine(Me.vacuumCurveShots_cr(transition))
		Dim debrisTop As String()
		Dim debrisBottom As String()
		Dim debrisSide As String()
		If transition Then
			Dim num As Integer = Global.UnityEngine.Random.Range(0, MyBase.properties.CurrentState.debris.transitionTopSpawn.Length)
			Dim num2 As Integer = Global.UnityEngine.Random.Range(0, MyBase.properties.CurrentState.debris.transitionBottomSpawn.Length)
			Dim num3 As Integer = Global.UnityEngine.Random.Range(0, MyBase.properties.CurrentState.debris.transitionSideSpawn.Length)
			debrisTop = p.transitionTopSpawn(num).Split(New Char() { ","c })
			debrisBottom = p.transitionBottomSpawn(num2).Split(New Char() { ","c })
			debrisSide = p.transitionSideSpawn(num3).Split(New Char() { ","c })
		Else
			debrisTop = p.debrisTopSpawn(Me.debrisTopMainIndex).Split(New Char() { ","c })
			debrisBottom = p.debrisBottomSpawn(Me.debrisBottomMainIndex).Split(New Char() { ","c })
			debrisSide = p.debrisSideSpawn(Me.debrisSideMainIndex).Split(New Char() { ","c })
		End If
		Dim debrisTopCount As Integer = 0
		Dim debrisBottomCount As Integer = 0
		Dim debrisSideCount As Integer = 0
		Dim maxLength As Integer = Mathf.Max(New Integer() { debrisTop.Length, debrisBottom.Length, debrisSide.Length })
		If transition AndAlso postTransitionPattern = LevelProperties.FlyingCowboy.Pattern.Ricochet Then
			Me.vacuumSizeCoroutine = MyBase.StartCoroutine(Me.growVacuum_cr())
		End If
		For i As Integer = 0 To maxLength - 1
			If i < debrisTop.Length Then
				Dim posIndex As Integer
				Parser.IntTryParse(debrisTop(debrisTopCount), posIndex)
				Me.createLinearDebris(Me.topPositions(posIndex), debrisTypePattern.PopInt(), i)
				debrisTopCount += 1
			End If
			If i < debrisBottom.Length Then
				Dim posIndex As Integer
				Parser.IntTryParse(debrisBottom(debrisBottomCount), posIndex)
				Me.createLinearDebris(Me.bottomPositions(posIndex), debrisTypePattern.PopInt(), i)
				debrisBottomCount += 1
			End If
			If i < debrisSide.Length Then
				Dim posIndex As Integer
				Parser.IntTryParse(debrisSide(debrisSideCount), posIndex)
				Me.createLinearDebris(Me.sidePositions(posIndex), debrisTypePattern.PopInt(), i)
				debrisSideCount += 1
			End If
			Yield CupheadTime.WaitForSeconds(Me, p.debrisDelay)
			If Me.phase3Trigger OrElse Me.endTransitionTrigger Then
				Exit For
			End If
		Next
		If transition AndAlso postTransitionPattern = LevelProperties.FlyingCowboy.Pattern.Vacuum AndAlso Not Me.phase3Trigger Then
			Me.allDebris.Clear()
		Else
			If Not transition Then
				Me.vacuumSizeCoroutine = MyBase.StartCoroutine(Me.growVacuum_cr())
			End If
			Dim allDebrisGone As Boolean = False
			While Not allDebrisGone
				allDebrisGone = True
				For j As Integer = 0 To Me.allDebris.Count - 1
					If Me.allDebris(j) IsNot Nothing AndAlso Not Me.allDebris(j).dead Then
						allDebrisGone = False
					End If
				Next
				Yield Nothing
			End While
			Me.allDebris.Clear()
			While Me.vacuumSizeCoroutine IsNot Nothing
				Yield Nothing
			End While
		End If
		If Not transition OrElse (transition AndAlso (postTransitionPattern = LevelProperties.FlyingCowboy.Pattern.Ricochet OrElse Me.phase3Trigger)) Then
			Me.endVacuumPullPlayer()
			Me.SFX_COWGIRL_COWGIRL_P2_VacuumSuckLoopStop()
		End If
		If transition Then
			Me.transitionVacuumAttackCoroutine = Nothing
			If Me.phase3Trigger Then
				Me.SFX_COWGIRL_COWGIRL_P2_VacuumSuckLoopStop()
				MyBase.animator.SetBool("OnPhase3", True)
			End If
		Else
			Me.SFX_COWGIRL_COWGIRL_P2_VacuumSuckLoopStop()
			Me.debrisTopMainIndex = (Me.debrisTopMainIndex + 1) Mod p.debrisTopSpawn.Length
			Me.debrisBottomMainIndex = (Me.debrisBottomMainIndex + 1) Mod p.debrisBottomSpawn.Length
			Me.debrisSideMainIndex = (Me.debrisSideMainIndex + 1) Mod p.debrisSideSpawn.Length
			Dim manualDeathHandling As Boolean = True
			If Me.phase3Trigger Then
				manualDeathHandling = False
				MyBase.animator.SetBool("OnPhase3", True)
			Else
				MyBase.animator.SetBool("OnRicochet", True)
				Yield CupheadTime.WaitForSeconds(Me, p.hesitate)
			End If
			Me.state = FlyingCowboyLevelCowboy.State.Idle
			If Me.phase3Trigger AndAlso manualDeathHandling Then
				Me.Ricochet()
			End If
		End If
		Return
	End Function

	' Token: 0x0600215F RID: 8543 RVA: 0x00133F68 File Offset: 0x00132368
	Private Sub createLinearDebris(rootPosition As Vector3, type As Integer, sortingIndex As Integer)
		Dim debris As LevelProperties.FlyingCowboy.Debris = MyBase.properties.CurrentState.debris
		Dim minMax As MinMax
		Dim flyingCowboyLevelDebris As FlyingCowboyLevelDebris
		If type = 1 Then
			minMax = debris.debrisOneSpeedStartEnd
			flyingCowboyLevelDebris = Me.largeVacuumDebrisPrefabs.GetRandom()
		ElseIf type = 2 Then
			minMax = debris.debrisTwoSpeedStartEnd
			flyingCowboyLevelDebris = Me.mediumVacuumDebrisPrefabs.GetRandom()
		Else
			minMax = debris.debrisThreeSpeedStartEnd
			flyingCowboyLevelDebris = Me.smallVacuumDebrisPrefabs.GetRandom()
		End If
		Dim vector As Vector3 = Me.vacuumDebrisAimTransform.position - rootPosition
		Dim flyingCowboyLevelDebris2 As FlyingCowboyLevelDebris = TryCast(flyingCowboyLevelDebris.Create(rootPosition, MathUtils.DirectionToAngle(vector), minMax.min), FlyingCowboyLevelDebris)
		flyingCowboyLevelDebris2.GetComponent(Of SpriteRenderer)().sortingOrder = 50 * type + sortingIndex
		Dim flag As Boolean = Me.debrisParryString.PopLetter() = "P"c
		flyingCowboyLevelDebris2.SetParryable(flag)
		flyingCowboyLevelDebris2.SetupLinearSpeed(minMax, debris.debrisSpeedUpDistance, Me.vacuumDebrisAimTransform)
		flyingCowboyLevelDebris2.SetupVacuum(Me.vacuumDebrisAimTransform, Me.vacuumDebrisDisappearTransform)
		Me.allDebris.Add(flyingCowboyLevelDebris2)
	End Sub

	' Token: 0x06002160 RID: 8544 RVA: 0x00134070 File Offset: 0x00132470
	Private Sub startVacuumPullPlayer(immediateFullStrength As Boolean)
		Me.endVacuumPullPlayer()
		Dim debris As LevelProperties.FlyingCowboy.Debris = MyBase.properties.CurrentState.debris
		For Each abstractPlayerController As AbstractPlayerController In PlayerManager.GetAllPlayers()
			Dim planePlayerController As PlanePlayerController = CType(abstractPlayerController, PlanePlayerController)
			If Not(planePlayerController Is Nothing) Then
				Dim vacuumForce As FlyingCowboyLevelCowboy.VacuumForce = New FlyingCowboyLevelCowboy.VacuumForce(planePlayerController, Me.vacuumDebrisDisappearTransform, debris.vacuumWindStrength * 0.5F, If((Not immediateFullStrength), debris.vacuumTimeToFullStrength, 0F))
				planePlayerController.motor.AddForce(vacuumForce)
				If planePlayerController.id = PlayerId.PlayerOne Then
					Me.forcePlayer1 = vacuumForce
				ElseIf planePlayerController.id = PlayerId.PlayerTwo Then
					Me.forcePlayer2 = vacuumForce
				End If
			End If
		Next
	End Sub

	' Token: 0x06002161 RID: 8545 RVA: 0x00134158 File Offset: 0x00132558
	Private Sub endVacuumPullPlayer()
		For Each abstractPlayerController As AbstractPlayerController In PlayerManager.GetAllPlayers()
			Dim planePlayerController As PlanePlayerController = CType(abstractPlayerController, PlanePlayerController)
			If Not(planePlayerController Is Nothing) AndAlso Not(planePlayerController.motor Is Nothing) Then
				planePlayerController.motor.RemoveForce(Me.forcePlayer1)
				planePlayerController.motor.RemoveForce(Me.forcePlayer2)
			End If
		Next
		Me.forcePlayer1 = Nothing
		Me.forcePlayer2 = Nothing
	End Sub

	' Token: 0x06002162 RID: 8546 RVA: 0x00134204 File Offset: 0x00132604
	Private Iterator Function growVacuum_cr() As IEnumerator
		Dim hash As Integer = Animator.StringToHash("Vacuum")
		Dim previousTime As Single = Single.MaxValue
		While True
			Dim normalizedTime As Single = MathUtilities.DecimalPart(MyBase.animator.GetCurrentAnimatorStateInfo(0).normalizedTime)
			If previousTime < 0.41666666F AndAlso normalizedTime >= 0.41666666F Then
				Exit For
			End If
			previousTime = normalizedTime
			Yield Nothing
		End While
		Me.setVacuumTransition()
		previousTime = Single.MaxValue
		While True
			Dim normalizedTime2 As Single = MathUtilities.DecimalPart(MyBase.animator.GetCurrentAnimatorStateInfo(0).normalizedTime)
			If previousTime < 0.29166666F AndAlso normalizedTime2 >= 0.29166666F Then
				Exit For
			End If
			previousTime = normalizedTime2
			Yield Nothing
		End While
		Me.setVacuumBig()
		Me.vacuumSizeCoroutine = Nothing
		Return
	End Function

	' Token: 0x06002163 RID: 8547 RVA: 0x0013421F File Offset: 0x0013261F
	Public Sub Ricochet()
		MyBase.animator.SetBool("OnRicochet", True)
		MyBase.StartCoroutine(Me.ricochet_cr())
	End Sub

	' Token: 0x06002164 RID: 8548 RVA: 0x00134240 File Offset: 0x00132640
	Private Iterator Function ricochet_cr() As IEnumerator
		Me.nextSafeShoot = 1
		Me.SFX_COWGIRL_COWGIRL_P2_StirrupWheelsLoopStart()
		Me.state = FlyingCowboyLevelCowboy.State.Ricochet
		Dim p As LevelProperties.FlyingCowboy.Ricochet = MyBase.properties.CurrentState.ricochet
		Me.setVacuumBig()
		Yield MyBase.animator.WaitForAnimationToStart(Me, "Ricochet", False)
		Me.vacuumSizeCoroutine = MyBase.StartCoroutine(Me.shrinkVacuum_cr())
		MyBase.transform.position = Me.phase2BasePosition
		Dim elapsedTime As Single = 0F
		Dim delayPattern As PatternString = New PatternString(p.rainDelayString, True)
		Dim bulletTypePattern As PatternString = New PatternString(p.rainTypeString, True)
		Dim xPositionPattern As PatternString = New PatternString(p.rainSpawnString, True)
		Dim speedPattern As PatternString = New PatternString(p.rainSpeedString, True)
		While elapsedTime < p.rainDuration
			If Me.phase3Trigger AndAlso elapsedTime >= 2F Then
				Exit While
			End If
			Dim delayTime As Single = delayPattern.PopFloat()
			Yield CupheadTime.WaitForSeconds(Me, delayTime)
			Dim bulletType As FlyingCowboyLevelRicochetDebris.BulletType = FlyingCowboyLevelRicochetDebris.BulletType.[Nothing]
			If bulletTypePattern.PopLetter() = "R"c Then
				bulletType = FlyingCowboyLevelRicochetDebris.BulletType.Ricochet
			End If
			Dim xPosition As Single = xPositionPattern.PopFloat()
			Dim speed As Single = speedPattern.PopFloat()
			Me.ricochetPrefab.Create(New Vector3(-xPosition, 430F), speed, p.splitBulletSpeed, bulletType, bulletType <> FlyingCowboyLevelRicochetDebris.BulletType.[Nothing] AndAlso Me.ricochetParryString.PopLetter() = "P"c)
			elapsedTime += delayTime
		End While
		MyBase.animator.SetBool("OnRicochet", False)
		Me.SFX_COWGIRL_COWGIRL_P2_StirrupWheelsLoopStop()
		If Me.phase3Trigger Then
			If Me.vacuumSizeCoroutine IsNot Nothing Then
				MyBase.StopCoroutine(Me.vacuumSizeCoroutine)
				Me.vacuumSizeCoroutine = Nothing
				Me.setVacuumRegular()
			End If
			MyBase.animator.SetBool("OnPhase3", True)
		Else
			Yield CupheadTime.WaitForSeconds(Me, p.rainRecoveryTime)
		End If
		If Me.phase3Trigger Then
			MyBase.animator.SetBool("OnPhase3", True)
		End If
		Me.state = FlyingCowboyLevelCowboy.State.Idle
		Return
	End Function

	' Token: 0x06002165 RID: 8549 RVA: 0x0013425C File Offset: 0x0013265C
	Private Sub animationEvent_SafeShoot(eventType As Integer)
		If eventType = 0 Then
			If Me.nextSafeShoot = 0 Then
				Dim abstractProjectile As AbstractProjectile = Me.ricochetUpPrefab.Create(Me.ricochetUpSpawnPoint.position)
				abstractProjectile.animator.Play("A")
				abstractProjectile.animator.Update(0F)
				Me.nextSafeShoot = 1
			ElseIf Me.nextSafeShoot = 2 Then
				Dim abstractProjectile2 As AbstractProjectile = Me.ricochetUpPrefab.Create(Me.ricochetUpSpawnPoint.position)
				abstractProjectile2.animator.Play("C")
				abstractProjectile2.animator.Update(0F)
				Me.nextSafeShoot = 1
			End If
		ElseIf eventType = 1 AndAlso Me.nextSafeShoot = 1 Then
			Dim abstractProjectile3 As AbstractProjectile = Me.ricochetUpPrefab.Create(Me.ricochetUpSpawnPoint.position)
			abstractProjectile3.animator.Play("B")
			abstractProjectile3.animator.Update(0F)
			Me.nextSafeShoot = If((Not Rand.Bool()), 2, 0)
			Me.shootCoins()
		End If
	End Sub

	' Token: 0x06002166 RID: 8550 RVA: 0x00134384 File Offset: 0x00132784
	Private Sub shootCoins()
		Dim ricochet As LevelProperties.FlyingCowboy.Ricochet = MyBase.properties.CurrentState.ricochet
		Dim num As Integer = ricochet.coinCountRange.RandomInt()
		For i As Integer = 0 To num - 1
			Dim vector As Vector2 = New Vector2(ricochet.coinSpeedXRange.RandomFloat(), KinematicUtilities.CalculateInitialSpeedToReachApex(ricochet.coinHeightRange.RandomFloat(), ricochet.coinGravity))
			Dim num2 As Single = Mathf.Atan2(vector.y, vector.x) * 57.29578F
			Dim basicProjectile As BasicProjectile = Me.coinProjectile.Create(Me.ricochetUpSpawnPoint.transform.position, num2, vector.magnitude)
			basicProjectile.Gravity = ricochet.coinGravity
			basicProjectile.GetComponent(Of SpriteRenderer)().maskInteraction = SpriteMaskInteraction.None
		Next
	End Sub

	' Token: 0x06002167 RID: 8551 RVA: 0x00134448 File Offset: 0x00132848
	Private Iterator Function shrinkVacuum_cr() As IEnumerator
		Yield MyBase.animator.WaitForNormalizedTime(Me, 2F, "Ricochet", 0, False, False, True)
		Me.setVacuumTransition()
		Yield MyBase.animator.WaitForNormalizedTime(Me, 2.9375F, "Ricochet", 0, False, False, True)
		Me.setVacuumRegular()
		Me.vacuumSizeCoroutine = Nothing
		Return
	End Function

	' Token: 0x06002168 RID: 8552 RVA: 0x00134464 File Offset: 0x00132864
	Private Sub setVacuumRegular()
		Dim renderer As Renderer = Me.regularVacuumRenderer
		Dim flag As Boolean = True
		Me.regularHoseRenderer.enabled = flag
		renderer.enabled = flag
		Dim renderer2 As Renderer = Me.transitionVacuumRenderer
		flag = False
		Me.bigHoseRenderer.enabled = flag
		flag = flag
		Me.bigVacuumRenderer.enabled = flag
		flag = flag
		Me.transitionHoseRenderer.enabled = flag
		renderer2.enabled = flag
	End Sub

	' Token: 0x06002169 RID: 8553 RVA: 0x001344C4 File Offset: 0x001328C4
	Private Sub setVacuumTransition()
		Dim renderer As Renderer = Me.transitionVacuumRenderer
		Dim flag As Boolean = True
		Me.transitionHoseRenderer.enabled = flag
		renderer.enabled = flag
		Dim renderer2 As Renderer = Me.regularVacuumRenderer
		flag = False
		Me.bigHoseRenderer.enabled = flag
		flag = flag
		Me.bigVacuumRenderer.enabled = flag
		flag = flag
		Me.regularHoseRenderer.enabled = flag
		renderer2.enabled = flag
	End Sub

	' Token: 0x0600216A RID: 8554 RVA: 0x00134524 File Offset: 0x00132924
	Private Sub setVacuumBig()
		Dim renderer As Renderer = Me.bigVacuumRenderer
		Dim flag As Boolean = True
		Me.bigHoseRenderer.enabled = flag
		renderer.enabled = flag
		Dim renderer2 As Renderer = Me.regularVacuumRenderer
		flag = False
		Me.transitionHoseRenderer.enabled = flag
		flag = flag
		Me.transitionVacuumRenderer.enabled = flag
		flag = flag
		Me.regularHoseRenderer.enabled = flag
		renderer2.enabled = flag
	End Sub

	' Token: 0x0600216B RID: 8555 RVA: 0x00134581 File Offset: 0x00132981
	Public Sub Death()
		MyBase.StartCoroutine(Me.phase3_cr())
	End Sub

	' Token: 0x0600216C RID: 8556 RVA: 0x00134590 File Offset: 0x00132990
	Private Iterator Function phase3_cr() As IEnumerator
		Me.phase3Trigger = True
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Ph2_To_Ph3", False, True)
		Me.StopAllCoroutines()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Return
	End Function

	' Token: 0x0600216D RID: 8557 RVA: 0x001345AB File Offset: 0x001329AB
	Private Sub aniEvent_SpawnMeat()
		Me.IsDead = True
	End Sub

	' Token: 0x0600216E RID: 8558 RVA: 0x001345B4 File Offset: 0x001329B4
	Private Sub animationEvent_PosterFlyAway()
		If Not Me.posterFlyAwayTriggered Then
			MyBase.animator.Play("FlyAway", FlyingCowboyLevelCowboy.PosterAnimatorLayer)
			Me.posterRenderer.sortingLayerName = "Effects"
		End If
		Me.posterFlyAwayTriggered = True
	End Sub

	' Token: 0x0600216F RID: 8559 RVA: 0x001345F0 File Offset: 0x001329F0
	Private Sub animationEvent_DisablePhase1Saloon()
		For Each spriteRenderer As SpriteRenderer In Me.saloonTransitionDisableRenderers
			spriteRenderer.enabled = False
		Next
	End Sub

	' Token: 0x06002170 RID: 8560 RVA: 0x00134623 File Offset: 0x00132A23
	Private Sub animationEvent_DisableSaloonCollider()
		Me.saloonCollidersParent.SetActive(False)
	End Sub

	' Token: 0x06002171 RID: 8561 RVA: 0x00134631 File Offset: 0x00132A31
	Private Sub animationEvent_EnablePlayerVacuumForce()
		Me.startVacuumPullPlayer(False)
	End Sub

	' Token: 0x06002172 RID: 8562 RVA: 0x0013463A File Offset: 0x00132A3A
	Private Sub animationEvent_DisableFrontSaloonWheel()
		Me.frontWheelRenderer.enabled = False
	End Sub

	' Token: 0x06002173 RID: 8563 RVA: 0x00134648 File Offset: 0x00132A48
	Private Sub animationEvent_DisableBackSaloonWheel()
		Me.backWheelRenderer.enabled = False
	End Sub

	' Token: 0x06002174 RID: 8564 RVA: 0x00134658 File Offset: 0x00132A58
	Private Sub animationEvent_TurnOffPhase1Animators()
		MyBase.animator.Play("Off", FlyingCowboyLevelCowboy.SaloonAnimatorLayer)
		MyBase.animator.Play("Off", FlyingCowboyLevelCowboy.PosterAnimatorLayer)
		MyBase.animator.Play("Off", FlyingCowboyLevelCowboy.DoorsAnimatorLayer)
		MyBase.animator.Play("Off", FlyingCowboyLevelCowboy.WheelSmokeAnimatorLayer)
	End Sub

	' Token: 0x06002175 RID: 8565 RVA: 0x001346B9 File Offset: 0x00132AB9
	Private Sub animationEvent_MoveCowgirlDown()
		MyBase.StartCoroutine(Me.moveDown_cr())
	End Sub

	' Token: 0x06002176 RID: 8566 RVA: 0x001346C8 File Offset: 0x00132AC8
	Private Sub animationEvent_SwapPhase2Puffs()
		Me.phase2PuffARenderer.enabled = Me.phase2PuffBRenderer.enabled
		Me.phase2PuffBRenderer.enabled = Not Me.phase2PuffARenderer.enabled
	End Sub

	' Token: 0x06002177 RID: 8567 RVA: 0x001346F9 File Offset: 0x00132AF9
	Private Sub onFadeOutStartEvent(time As Single)
		MyBase.StartCoroutine(Me.introBird_cr(0.25F))
	End Sub

	' Token: 0x06002178 RID: 8568 RVA: 0x0013470D File Offset: 0x00132B0D
	Private Sub onPlayerJoinedEvent(playerId As PlayerId)
		If Me.forcePlayer1 IsNot Nothing OrElse Me.forcePlayer2 IsNot Nothing Then
			Me.startVacuumPullPlayer(True)
		End If
	End Sub

	' Token: 0x06002179 RID: 8569 RVA: 0x0013472C File Offset: 0x00132B2C
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		Gizmos.color = Color.green
		Dim num As Single = (Me.vacuumSpawnTop.position.y - Me.vacuumSpawnBottom.position.y) / 5F
		For i As Integer = 0 To 6 - 1
			Dim x As Single = Me.vacuumSpawnBottom.position.x
			Dim num2 As Single = If((i <> 5), (Me.vacuumSpawnBottom.position.y + num * CSng(i)), Me.vacuumSpawnTop.position.y)
			Gizmos.DrawWireSphere(New Vector3(x, num2), 10F)
		Next
		Gizmos.color = Color.yellow
		Dim num3 As Single = (CSng(Me.debrisSpawnHorizontalSpacing) - Me.vacuumSpawnTop.position.x) / 6F
		For j As Integer = 0 To 6 - 1
			Dim num4 As Single = Me.vacuumSpawnTop.position.x + num3 + num3 * CSng(j)
			Dim y As Single = Me.vacuumSpawnTop.position.y
			Gizmos.DrawWireSphere(New Vector3(num4, y), 10F)
		Next
		Gizmos.color = Color.yellow
		num3 = (CSng(Me.debrisSpawnHorizontalSpacing) - Me.vacuumSpawnBottom.position.x) / 6F
		For k As Integer = 0 To 6 - 1
			Dim num5 As Single = Me.vacuumSpawnBottom.position.x + num3 + num3 * CSng(k)
			Dim y2 As Single = Me.vacuumSpawnBottom.position.y
			Gizmos.DrawWireSphere(New Vector3(num5, y2), 10F)
		Next
		Gizmos.color = Color.red
		For l As Integer = 0 To 4 - 1
			Dim num6 As Single = Me.vacuumSpawnTop.position.x + num3 + num3 * CSng((6 + l))
			Dim y3 As Single = Me.vacuumSpawnTop.position.y
			Gizmos.DrawWireSphere(New Vector3(num6, y3), 10F)
		Next
		Gizmos.color = Color.red
		For m As Integer = 0 To 4 - 1
			Dim num7 As Single = Me.vacuumSpawnBottom.position.x + num3 + num3 * CSng((6 + m))
			Dim y4 As Single = Me.vacuumSpawnBottom.position.y
			Gizmos.DrawWireSphere(New Vector3(num7, y4), 10F)
		Next
	End Sub

	' Token: 0x0600217A RID: 8570 RVA: 0x001349DD File Offset: 0x00132DDD
	Private Sub AnimationEvent_SFX_COWGIRL_Vocal_Laugh()
		AudioManager.Play("sfx_dlc_cowgirl_vocal_laugh")
		Me.emitAudioFromObject.Add("sfx_dlc_cowgirl_vocal_laugh")
	End Sub

	' Token: 0x0600217B RID: 8571 RVA: 0x001349F9 File Offset: 0x00132DF9
	Private Sub AnimationEvent_SFX_COWGIRL_Vocal_MooHa()
		AudioManager.Play("sfx_dlc_cowgirl_vocal_mooha")
		Me.emitAudioFromObject.Add("sfx_dlc_cowgirl_vocal_mooha")
	End Sub

	' Token: 0x0600217C RID: 8572 RVA: 0x00134A15 File Offset: 0x00132E15
	Private Sub AnimationEvent_SFX_COWGIRL_Vocal_Surprised()
		AudioManager.Play("sfx_dlc_cowgirl_vocal_surprised")
		Me.emitAudioFromObject.Add("sfx_dlc_cowgirl_vocal_surprised")
	End Sub

	' Token: 0x0600217D RID: 8573 RVA: 0x00134A31 File Offset: 0x00132E31
	Private Sub AnimationEvent_SFX_COWGIRL_Vocal_YeeHaw()
		AudioManager.Play("sfx_dlc_cowgirl_vocal_yeehaw")
		Me.emitAudioFromObject.Add("sfx_dlc_cowgirl_vocal_yeehaw")
	End Sub

	' Token: 0x0600217E RID: 8574 RVA: 0x00134A4D File Offset: 0x00132E4D
	Private Sub AnimationEvent_SFX_COWGIRL_COWGIRL_JugGunRaise()
		AudioManager.Play("sfx_dlc_cowgirl_p1_snakeoilattack_juggun_raise")
		Me.emitAudioFromObject.Add("sfx_dlc_snowcult_p2_snowmonster_death_stompoffscreen")
	End Sub

	' Token: 0x0600217F RID: 8575 RVA: 0x00134A69 File Offset: 0x00132E69
	Private Sub AnimationEvent_SFX_COWGIRL_COWGIRL_JugGunHolster()
		AudioManager.Play("sfx_dlc_cowgirl_p1_snakeoilattack_juggun_holster")
		Me.emitAudioFromObject.Add("sfx_dlc_cowgirl_p1_snakeoilattack_juggun_holster")
	End Sub

	' Token: 0x06002180 RID: 8576 RVA: 0x00134A85 File Offset: 0x00132E85
	Private Sub AnimationEvent_SFX_COWGIRL_COWGIRL_JugGunBlow()
		AudioManager.Play("sfx_dlc_cowgirl_p1_snakeoilattack_juggun_blow")
		Me.emitAudioFromObject.Add("sfx_dlc_cowgirl_p1_snakeoilattack_juggun_blow")
	End Sub

	' Token: 0x06002181 RID: 8577 RVA: 0x00134AA1 File Offset: 0x00132EA1
	Private Sub AnimationEvent_SFX_COWGIRL_COWGIRL_JugGunBlowAndHolster()
		AudioManager.Play("sfx_dlc_cowgirl_p1_snakeoilattack_juggun_blowandholster")
		Me.emitAudioFromObject.Add("sfx_dlc_cowgirl_p1_snakeoilattack_juggun_blowandholster")
	End Sub

	' Token: 0x06002182 RID: 8578 RVA: 0x00134ABD File Offset: 0x00132EBD
	Private Sub AnimationEvent_SFX_COWGIRL_COWGIRL_JugGunBlast()
		AudioManager.[Stop]("sfx_dlc_cowgirl_p1_snakeoilattack_juggun_spin_loop")
		AudioManager.Play("sfx_dlc_cowgirl_p1_snakeoilattack_juggunblast")
		Me.emitAudioFromObject.Add("sfx_dlc_cowgirl_p1_snakeoilattack_juggunblast")
	End Sub

	' Token: 0x06002183 RID: 8579 RVA: 0x00134AE3 File Offset: 0x00132EE3
	Private Sub SFX_COWGIRL_COWGIRL_JugGunSpinLoop()
		AudioManager.PlayLoop("sfx_dlc_cowgirl_p1_snakeoilattack_juggun_spin_loop")
		Me.emitAudioFromObject.Add("sfx_dlc_cowgirl_p1_snakeoilattack_juggun_spin_loop")
	End Sub

	' Token: 0x06002184 RID: 8580 RVA: 0x00134AFF File Offset: 0x00132EFF
	Private Sub AnimationEvent_SFX_COWGIRL_COWGIRL_P1toP2VacuumSuckup()
		AudioManager.Play("sfx_dlc_cowgirl_p1_death_saloon_vacuumsuckup")
		Me.emitAudioFromObject.Add("sfx_dlc_cowgirl_p1_death_saloon_vacuumsuckup")
	End Sub

	' Token: 0x06002185 RID: 8581 RVA: 0x00134B1B File Offset: 0x00132F1B
	Private Sub SFX_COWGIRL_COWGIRL_LassoSpinLoop()
		AudioManager.PlayLoop("sfx_dlc_cowgirl_p1_lasso_spin_loop")
		AudioManager.FadeSFXVolume("sfx_dlc_cowgirl_p1_lasso_spin_loop", 0.7F, 0.01F)
		Me.emitAudioFromObject.Add("sfx_dlc_cowgirl_p1_lasso_spin_loop")
	End Sub

	' Token: 0x06002186 RID: 8582 RVA: 0x00134B4B File Offset: 0x00132F4B
	Private Sub SFX_COWGIRL_COWGIRL_LassoSpinLoopStop()
		AudioManager.FadeSFXVolume("sfx_dlc_cowgirl_p1_lasso_spin_loop", 0F, 0.2F)
	End Sub

	' Token: 0x06002187 RID: 8583 RVA: 0x00134B61 File Offset: 0x00132F61
	Private Sub AnimationEvent_SFX_COWGIRL_COWGIRL_LassoThrowCatchRelease()
		AudioManager.Play("sfx_dlc_cowgirl_p1_lasso_throw_catch_release")
		Me.emitAudioFromObject.Add("sfx_dlc_cowgirl_p1_lasso_throw_catch_release")
	End Sub

	' Token: 0x06002188 RID: 8584 RVA: 0x00134B7D File Offset: 0x00132F7D
	Private Sub SFX_COWGIRL_COWGIRL_WheelConstantLoop()
		AudioManager.PlayLoop("sfx_dlc_cowgirl_p1_saloon_wheelsconstant_loop")
		Me.emitAudioFromObject.Add("sfx_dlc_cowgirl_p1_saloon_wheelsconstant_loop")
	End Sub

	' Token: 0x06002189 RID: 8585 RVA: 0x00134B99 File Offset: 0x00132F99
	Private Sub SFX_COWGIRL_COWGIRL_WheelConstantLoopStop()
		AudioManager.[Stop]("sfx_dlc_cowgirl_p1_saloon_wheelsconstant_loop")
	End Sub

	' Token: 0x0600218A RID: 8586 RVA: 0x00134BA5 File Offset: 0x00132FA5
	Private Sub AnimationEvent_SFX_COWGIRL_COWGIRL_PositionLowtoHigh()
		AudioManager.Play("sfx_dlc_cowgirl_p1_saloon_positionchange_lowtohigh")
		Me.emitAudioFromObject.Add("sfx_dlc_cowgirl_p1_saloon_positionchange_lowtohigh")
	End Sub

	' Token: 0x0600218B RID: 8587 RVA: 0x00134BC1 File Offset: 0x00132FC1
	Private Sub AnimationEvent_SFX_COWGIRL_COWGIRL_PositionHightoLow()
		AudioManager.Play("sfx_dlc_cowgirl_p1_saloon_positionchange_hightolow")
		Me.emitAudioFromObject.Add("sfx_dlc_cowgirl_p1_saloon_positionchange_hightolow")
	End Sub

	' Token: 0x0600218C RID: 8588 RVA: 0x00134BDD File Offset: 0x00132FDD
	Private Sub AnimationEvent_SFX_COWGIRL_COWGIRL_P2_Stirrups()
		AudioManager.Play("sfx_DLC_Cowgirl_Stirrups")
		Me.emitAudioFromObject.Add("sfx_DLC_Cowgirl_Stirrups")
	End Sub

	' Token: 0x0600218D RID: 8589 RVA: 0x00134BF9 File Offset: 0x00132FF9
	Private Sub AnimationEvent_SFX_COWGIRL_COWGIRL_P2_VacuumBlowback()
		AudioManager.Play("sfx_dlc_cowgirl_p2_vacuum_blowback")
		Me.emitAudioFromObject.Add("sfx_dlc_cowgirl_p2_vacuum_blowback")
	End Sub

	' Token: 0x0600218E RID: 8590 RVA: 0x00134C15 File Offset: 0x00133015
	Private Sub AnimationEvent_SFX_COWGIRL_COWGIRL_P2_VacuumCrouchPosition()
		AudioManager.Play("sfx_dlc_cowgirl_p2_vacuum_blowback_crouchposition")
		Me.emitAudioFromObject.Add("sfx_dlc_cowgirl_p2_vacuum_blowback_crouchposition")
	End Sub

	' Token: 0x0600218F RID: 8591 RVA: 0x00134C31 File Offset: 0x00133031
	Private Sub SFX_COWGIRL_COWGIRL_P2_VacuumSuckLoop()
		AudioManager.PlayLoop("sfx_dlc_cowgirl_p2_vacuum_constantsuck_loop")
		Me.emitAudioFromObject.Add("sfx_dlc_cowgirl_p2_vacuum_constantsuck_loop")
		AudioManager.FadeSFXVolume("sfx_dlc_cowgirl_p2_vacuum_constantsuck_loop", 0F, 1F, 1F)
	End Sub

	' Token: 0x06002190 RID: 8592 RVA: 0x00134C66 File Offset: 0x00133066
	Private Sub SFX_COWGIRL_COWGIRL_P2_VacuumSuckLoopStop()
		AudioManager.FadeSFXVolume("sfx_dlc_cowgirl_p2_vacuum_constantsuck_loop", 0F, 1F)
	End Sub

	' Token: 0x06002191 RID: 8593 RVA: 0x00134C7C File Offset: 0x0013307C
	Private Sub AnimationEvent_SFX_COWGIRL_COWGIRL_P2_VacuumSuckIn()
		AudioManager.Play("sfx_dlc_cowgirl_p2_vacuum_suckin")
		Me.emitAudioFromObject.Add("sfx_dlc_cowgirl_p2_vacuum_suckin")
	End Sub

	' Token: 0x06002192 RID: 8594 RVA: 0x00134C98 File Offset: 0x00133098
	Private Sub AnimationEvent_SFX_COWGIRL_COWGIRL_P2_VacuumeExplosionDeath()
		AudioManager.Play("sfx_dlc_cowgirl_p2_death_vacuumexplosion_transition")
		Me.emitAudioFromObject.Add("sfx_dlc_cowgirl_p2_death_vacuumexplosion_transition")
	End Sub

	' Token: 0x06002193 RID: 8595 RVA: 0x00134CB4 File Offset: 0x001330B4
	Private Sub SFX_COWGIRL_COWGIRL_P2_StirrupWheelsLoopStart()
		AudioManager.PlayLoop("sfx_dlc_cowgirl_stirrupswheels_loop")
		Me.emitAudioFromObject.Add("sfx_dlc_cowgirl_stirrupswheels_loop")
	End Sub

	' Token: 0x06002194 RID: 8596 RVA: 0x00134CD0 File Offset: 0x001330D0
	Private Sub SFX_COWGIRL_COWGIRL_P2_StirrupWheelsLoopStop()
		AudioManager.[Stop]("sfx_dlc_cowgirl_stirrupswheels_loop")
	End Sub

	' Token: 0x040029D2 RID: 10706
	Private Const SNAKE_BULLET_COUNT As Integer = 2

	' Token: 0x040029D3 RID: 10707
	Private Const DEBRIS_SPAWN_COUNT As Integer = 6

	' Token: 0x040029D4 RID: 10708
	Private Const DEBRIS_CURVE_SPAWN_COUNT As Integer = 4

	' Token: 0x040029D5 RID: 10709
	Private Shared SaloonAnimatorLayer As Integer = 1

	' Token: 0x040029D6 RID: 10710
	Private Shared PosterAnimatorLayer As Integer = 2

	' Token: 0x040029D7 RID: 10711
	Private Shared DoorsAnimatorLayer As Integer = 3

	' Token: 0x040029D8 RID: 10712
	Private Shared WheelSmokeAnimatorLayer As Integer = 4

	' Token: 0x040029D9 RID: 10713
	Private Shared TransitionSmokeLayer As Integer = 5

	' Token: 0x040029DA RID: 10714
	<SerializeField()>
	Private posterRenderer As SpriteRenderer

	' Token: 0x040029DB RID: 10715
	<SerializeField()>
	Private ufoPrefab As FlyingCowboyLevelUFO

	' Token: 0x040029DC RID: 10716
	<SerializeField()>
	Private birdPrefab As FlyingCowboyLevelBird

	' Token: 0x040029DD RID: 10717
	<SerializeField()>
	Private birdStartPosition As Transform

	' Token: 0x040029DE RID: 10718
	<SerializeField()>
	Private birdEndPosition As Transform

	' Token: 0x040029DF RID: 10719
	<SerializeField()>
	Private birdSafetyZone As TriggerZone

	' Token: 0x040029E0 RID: 10720
	<SerializeField()>
	Private backshotPrefab As FlyingCowboyLevelBackshot

	' Token: 0x040029E1 RID: 10721
	<SerializeField()>
	Private snakeTopRoot As Transform()

	' Token: 0x040029E2 RID: 10722
	<SerializeField()>
	Private snakeBottomRoot As Transform()

	' Token: 0x040029E3 RID: 10723
	<SerializeField()>
	Private oilBlobPrefab As FlyingCowboyLevelOilBlob

	' Token: 0x040029E4 RID: 10724
	<SerializeField()>
	Private snakeOilMuzzleFXPrefab As Effect

	' Token: 0x040029E5 RID: 10725
	<SerializeField()>
	Private cactus As GameObject

	' Token: 0x040029E6 RID: 10726
	<SerializeField()>
	Private wobbleRadius As Vector2

	' Token: 0x040029E7 RID: 10727
	<SerializeField()>
	Private wobbleDuration As Vector2

	' Token: 0x040029E8 RID: 10728
	<SerializeField()>
	Private saloonCollidersParent As GameObject

	' Token: 0x040029E9 RID: 10729
	<SerializeField()>
	Private lanternARenderer As SpriteRenderer

	' Token: 0x040029EA RID: 10730
	<SerializeField()>
	Private lanternBRenderer As SpriteRenderer

	' Token: 0x040029EB RID: 10731
	<SerializeField()>
	Private saloonTransitionDisableRenderers As SpriteRenderer()

	' Token: 0x040029EC RID: 10732
	<SerializeField()>
	Private frontWheelRenderer As SpriteRenderer

	' Token: 0x040029ED RID: 10733
	<SerializeField()>
	Private backWheelRenderer As SpriteRenderer

	' Token: 0x040029EE RID: 10734
	<SerializeField()>
	Private smallVacuumDebrisPrefabs As FlyingCowboyLevelDebris()

	' Token: 0x040029EF RID: 10735
	<SerializeField()>
	Private mediumVacuumDebrisPrefabs As FlyingCowboyLevelDebris()

	' Token: 0x040029F0 RID: 10736
	<SerializeField()>
	Private largeVacuumDebrisPrefabs As FlyingCowboyLevelDebris()

	' Token: 0x040029F1 RID: 10737
	<SerializeField()>
	Private ricochetPrefab As FlyingCowboyLevelRicochetDebris

	' Token: 0x040029F2 RID: 10738
	<SerializeField()>
	Private ricochetUpPrefab As AbstractProjectile

	' Token: 0x040029F3 RID: 10739
	<SerializeField()>
	Private ricochetUpSpawnPoint As Transform

	' Token: 0x040029F4 RID: 10740
	<SerializeField()>
	Private coinProjectile As BasicProjectile

	' Token: 0x040029F5 RID: 10741
	<SerializeField()>
	Private pistolShootRoot As Transform

	' Token: 0x040029F6 RID: 10742
	<SerializeField()>
	Private debrisSpawnHorizontalSpacing As Integer = 140

	' Token: 0x040029F7 RID: 10743
	<SerializeField()>
	Private vacuumDebrisAimTransform As Transform

	' Token: 0x040029F8 RID: 10744
	<SerializeField()>
	Private vacuumDebrisDisappearTransform As Transform

	' Token: 0x040029F9 RID: 10745
	<SerializeField()>
	Private vacuumSpawnTop As Transform

	' Token: 0x040029FA RID: 10746
	<SerializeField()>
	Private vacuumSpawnBottom As Transform

	' Token: 0x040029FB RID: 10747
	<SerializeField()>
	Private bigVacuumRenderer As SpriteRenderer

	' Token: 0x040029FC RID: 10748
	<SerializeField()>
	Private transitionVacuumRenderer As SpriteRenderer

	' Token: 0x040029FD RID: 10749
	<SerializeField()>
	Private regularVacuumRenderer As SpriteRenderer

	' Token: 0x040029FE RID: 10750
	<SerializeField()>
	Private bigHoseRenderer As SpriteRenderer

	' Token: 0x040029FF RID: 10751
	<SerializeField()>
	Private transitionHoseRenderer As SpriteRenderer

	' Token: 0x04002A00 RID: 10752
	<SerializeField()>
	Private regularHoseRenderer As SpriteRenderer

	' Token: 0x04002A01 RID: 10753
	<SerializeField()>
	Private phase2PuffARenderer As SpriteRenderer

	' Token: 0x04002A02 RID: 10754
	<SerializeField()>
	Private phase2PuffBRenderer As SpriteRenderer

	' Token: 0x04002A05 RID: 10757
	Private introBirdTriggered As Boolean

	' Token: 0x04002A06 RID: 10758
	Private introBird As FlyingCowboyLevelBird

	' Token: 0x04002A07 RID: 10759
	Private phase2Trigger As Boolean

	' Token: 0x04002A08 RID: 10760
	Private endTransitionTrigger As Boolean

	' Token: 0x04002A09 RID: 10761
	Private phase3Trigger As Boolean

	' Token: 0x04002A0A RID: 10762
	Private initialSaloonPosition As Vector3

	' Token: 0x04002A0B RID: 10763
	Private wobbleTimeElapsed As Vector2

	' Token: 0x04002A0C RID: 10764
	Private topPositions As Vector3()

	' Token: 0x04002A0D RID: 10765
	Private bottomPositions As Vector3()

	' Token: 0x04002A0E RID: 10766
	Private sidePositions As Vector3()

	' Token: 0x04002A0F RID: 10767
	Private topCurvePositions As Vector3()

	' Token: 0x04002A10 RID: 10768
	Private bottomCurvePositions As Vector3()

	' Token: 0x04002A11 RID: 10769
	Private phase2BasePosition As Vector3

	' Token: 0x04002A12 RID: 10770
	Private damageDealer As DamageDealer

	' Token: 0x04002A13 RID: 10771
	Private damageReceiver As DamageReceiver

	' Token: 0x04002A14 RID: 10772
	Private allDebris As List(Of FlyingCowboyLevelDebris)

	' Token: 0x04002A15 RID: 10773
	Private ufo As FlyingCowboyLevelUFO

	' Token: 0x04002A16 RID: 10774
	Private snakeOilShotsPerAttackString As PatternString

	' Token: 0x04002A17 RID: 10775
	Private snakeOffsetString As PatternString

	' Token: 0x04002A18 RID: 10776
	Private snakeWidthString As PatternString

	' Token: 0x04002A19 RID: 10777
	Private backshotHighSpawnPosition As PatternString

	' Token: 0x04002A1A RID: 10778
	Private backshotLowSpawnPosition As PatternString

	' Token: 0x04002A1B RID: 10779
	Private backshotSpawnDelay As PatternString

	' Token: 0x04002A1C RID: 10780
	Private backshotBulletParryable As PatternString

	' Token: 0x04002A1D RID: 10781
	Private backshotAnticipationStartDistancePattern As PatternString

	' Token: 0x04002A1E RID: 10782
	Private debrisTopMainIndex As Integer

	' Token: 0x04002A1F RID: 10783
	Private debrisBottomMainIndex As Integer

	' Token: 0x04002A20 RID: 10784
	Private debrisSideMainIndex As Integer

	' Token: 0x04002A21 RID: 10785
	Private forcePlayer1 As FlyingCowboyLevelCowboy.VacuumForce

	' Token: 0x04002A22 RID: 10786
	Private forcePlayer2 As FlyingCowboyLevelCowboy.VacuumForce

	' Token: 0x04002A23 RID: 10787
	Private debrisCurveString As PatternString

	' Token: 0x04002A24 RID: 10788
	Private debrisParryString As PatternString

	' Token: 0x04002A25 RID: 10789
	Private ricochetParryString As PatternString

	' Token: 0x04002A26 RID: 10790
	Private nextSafeShoot As Integer

	' Token: 0x04002A27 RID: 10791
	Private posterFlyAwayTriggered As Boolean

	' Token: 0x04002A28 RID: 10792
	Private transitionVacuumAttackCoroutine As Coroutine

	' Token: 0x04002A29 RID: 10793
	Private vacuumSizeCoroutine As Coroutine

	' Token: 0x02000650 RID: 1616
	Public Enum State
		' Token: 0x04002A2B RID: 10795
		Idle
		' Token: 0x04002A2C RID: 10796
		Wait
		' Token: 0x04002A2D RID: 10797
		SnakeAttack
		' Token: 0x04002A2E RID: 10798
		BeamAttack
		' Token: 0x04002A2F RID: 10799
		Vacuum
		' Token: 0x04002A30 RID: 10800
		Ricochet
		' Token: 0x04002A31 RID: 10801
		PhaseTrans
	End Enum

	' Token: 0x02000651 RID: 1617
	Public Class VacuumForce
		Inherits PlanePlayerMotor.Force

		' Token: 0x06002196 RID: 8598 RVA: 0x00134D24 File Offset: 0x00133124
		Public Sub New(player As PlanePlayerController, aimPointTransform As Transform, strength As Single, timeToFullStrength As Single)
			MyBase.New(Vector2.zero, True)
			Me.player = player
			Me.aimPointTransform = aimPointTransform
			Me.strength = strength
			Me.currentStrength = 0F
			Me.timeToFullStrength = timeToFullStrength
			Me.elapsedTime = 0F
		End Sub

		' Token: 0x1700038F RID: 911
		' (get) Token: 0x06002197 RID: 8599 RVA: 0x00134D70 File Offset: 0x00133170
		Public Overrides ReadOnly Property force As Vector2
			Get
				If Me.player Is Nothing Then
					Return Vector2.zero
				End If
				Return(Me.player.center - Me.aimPointTransform.position).normalized * Me.currentStrength
			End Get
		End Property

		' Token: 0x06002198 RID: 8600 RVA: 0x00134DC7 File Offset: 0x001331C7
		Public Sub UpdateStrength(deltaTime As Single)
			Me.elapsedTime += deltaTime
			Me.currentStrength = Mathf.Lerp(0F, Me.strength, Me.elapsedTime / Me.timeToFullStrength)
		End Sub

		' Token: 0x04002A32 RID: 10802
		Private player As PlanePlayerController

		' Token: 0x04002A33 RID: 10803
		Private aimPointTransform As Transform

		' Token: 0x04002A34 RID: 10804
		Private strength As Single

		' Token: 0x04002A35 RID: 10805
		Private currentStrength As Single

		' Token: 0x04002A36 RID: 10806
		Private timeToFullStrength As Single

		' Token: 0x04002A37 RID: 10807
		Private elapsedTime As Single
	End Class
End Class
