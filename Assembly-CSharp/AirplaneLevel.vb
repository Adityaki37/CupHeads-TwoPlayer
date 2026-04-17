Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine
Imports UnityStandardAssets.ImageEffects

' Token: 0x02000016 RID: 22
Public Class AirplaneLevel
	Inherits Level

	' Token: 0x06000021 RID: 33 RVA: 0x00050BE0 File Offset: 0x0004EFE0
	Protected Overrides Sub PartialInit()
		Me.properties = LevelProperties.Airplane.GetMode(MyBase.mode)
		AddHandler Me.properties.OnStateChange, AddressOf MyBase.zHack_OnStateChanged
		AddHandler Me.properties.OnBossDeath, AddressOf MyBase.zHack_OnWin
		MyBase.timeline = Me.properties.CreateTimeline(MyBase.mode)
		Me.goalTimes = Me.properties.goalTimes
		AddHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		MyBase.PartialInit()
	End Sub

	' Token: 0x17000003 RID: 3
	' (get) Token: 0x06000022 RID: 34 RVA: 0x00050C76 File Offset: 0x0004F076
	Public Overrides ReadOnly Property CurrentLevel As Levels
		Get
			Return Levels.Airplane
		End Get
	End Property

	' Token: 0x17000004 RID: 4
	' (get) Token: 0x06000023 RID: 35 RVA: 0x00050C7D File Offset: 0x0004F07D
	Public Overrides ReadOnly Property CurrentScene As Scenes
		Get
			Return Scenes.scene_level_airplane
		End Get
	End Property

	' Token: 0x17000005 RID: 5
	' (get) Token: 0x06000024 RID: 36 RVA: 0x00050C84 File Offset: 0x0004F084
	Public Overrides ReadOnly Property BossPortrait As Sprite
		Get
			Select Case Me.properties.CurrentState.stateName
				Case LevelProperties.Airplane.States.Main, LevelProperties.Airplane.States.Rocket
					Return Me._bossPortraitMain
				Case LevelProperties.Airplane.States.Terriers
					Return Me._bossPortraitPhaseTwo
				Case LevelProperties.Airplane.States.Leader
					Return If((Not Me.secretPhaseActivated), Me._bossPortraitPhaseThree, Me._bossPortraitPhaseSecret)
			End Select
			Global.Debug.LogError("Couldn't find portrait for state " + Me.properties.CurrentState.stateName + ". Using Main.", Nothing)
			Return Me._bossPortraitMain
		End Get
	End Property

	' Token: 0x17000006 RID: 6
	' (get) Token: 0x06000025 RID: 37 RVA: 0x00050D1C File Offset: 0x0004F11C
	Public Overrides ReadOnly Property BossQuote As String
		Get
			Select Case Me.properties.CurrentState.stateName
				Case LevelProperties.Airplane.States.Main, LevelProperties.Airplane.States.Rocket
					Return Me._bossQuoteMain
				Case LevelProperties.Airplane.States.Terriers
					Return Me._bossQuotePhaseTwo
				Case LevelProperties.Airplane.States.Leader
					Return Me._bossQuotePhaseThree
			End Select
			Global.Debug.LogError("Couldn't find quote for state " + Me.properties.CurrentState.stateName + ". Using Main.", Nothing)
			Return Me._bossQuoteMain
		End Get
	End Property

	' Token: 0x17000007 RID: 7
	' (get) Token: 0x06000026 RID: 38 RVA: 0x00050D9E File Offset: 0x0004F19E
	' (set) Token: 0x06000027 RID: 39 RVA: 0x00050DA6 File Offset: 0x0004F1A6
	Public Property Rotating As Boolean

	' Token: 0x06000028 RID: 40 RVA: 0x00050DB0 File Offset: 0x0004F1B0
	Protected Overrides Sub Start()
		MyBase.CameraRotates = True
		Me.Rotating = False
		MyBase.Start()
		Me.airplane.LevelInit(Me.properties)
		Me.bulldogPlane.LevelInit(Me.properties)
		Me.bulldogParachute.LevelInit(Me.properties)
		Me.bulldogCatAttack.LevelInit(Me.properties)
		Me.canteenAnimator.LevelInit(Me.properties)
		Me.secretLeader.gameObject.SetActive(False)
		Me.airplane.SetXRange(-480F, 480F)
		Me.leader.LevelInit(Me.properties)
		Me.leader.gameObject.SetActive(False)
		MyBase.Invoke("StartGetShadowRenderers", 0.1F)
		AddHandler PlayerManager.OnPlayerJoinedEvent, AddressOf Me.GetShadowableRenderers
	End Sub

	' Token: 0x06000029 RID: 41 RVA: 0x00050E8F File Offset: 0x0004F28F
	Protected Overrides Sub OnDestroy()
		RemoveHandler PlayerManager.OnPlayerJoinedEvent, AddressOf Me.GetShadowableRenderers
		MyBase.OnDestroy()
		Me._bossPortraitMain = Nothing
		Me._bossPortraitPhaseTwo = Nothing
		Me._bossPortraitPhaseThree = Nothing
		Me._bossPortraitPhaseSecret = Nothing
		Me.WORKAROUND_NullifyFields()
	End Sub

	' Token: 0x0600002A RID: 42 RVA: 0x00050ECC File Offset: 0x0004F2CC
	Private Sub StartGetShadowRenderers()
		Me.GetShadowableRenderers(PlayerId.PlayerOne)
		Me.GetShadowableRenderers(PlayerId.PlayerTwo)
		For Each spriteRenderer As SpriteRenderer In Me.airplane.GetComponentsInChildren(Of SpriteRenderer)()
			If spriteRenderer.name <> "PaladinShadow0" AndAlso spriteRenderer.name <> "PaladinShadow1" Then
				Me.shadowableRenderers.Add(spriteRenderer)
			End If
		Next
	End Sub

	' Token: 0x0600002B RID: 43 RVA: 0x00050F44 File Offset: 0x0004F344
	Private Sub GetShadowableRenderers(playerId As PlayerId)
		Dim player As AbstractPlayerController = PlayerManager.GetPlayer(playerId)
		If player Then
			For Each spriteRenderer As SpriteRenderer In player.GetComponentsInChildren(Of SpriteRenderer)()
				If spriteRenderer.name <> "PaladinShadow0" AndAlso spriteRenderer.name <> "PaladinShadow1" Then
					Me.shadowableRenderers.Add(spriteRenderer)
				End If
			Next
		End If
		Me.UpdateShadow(Me.currentShadowLevel)
	End Sub

	' Token: 0x0600002C RID: 44 RVA: 0x00050FC8 File Offset: 0x0004F3C8
	Public Sub UpdateShadow(shadowValue As Single)
		Me.currentShadowLevel = shadowValue
		Me.shadowableRenderers.Remove(Nothing)
		For Each spriteRenderer As SpriteRenderer In Me.shadowableRenderers
			If spriteRenderer IsNot Nothing Then
				spriteRenderer.color = New Color(shadowValue, shadowValue, shadowValue)
			End If
		Next
	End Sub

	' Token: 0x0600002D RID: 45 RVA: 0x0005104C File Offset: 0x0004F44C
	Protected Overrides Sub OnStateChanged()
		MyBase.OnStateChanged()
		If Me.properties.CurrentState.stateName = LevelProperties.Airplane.States.Rocket Then
			Me.bulldogPlane.StartRocket()
		ElseIf Me.properties.CurrentState.stateName = LevelProperties.Airplane.States.Terriers Then
			Me.StopAllCoroutines()
			MyBase.StartCoroutine(Me.terrier_cr())
			Me.canteenAnimator.triggerCheer = True
		ElseIf Me.properties.CurrentState.stateName = LevelProperties.Airplane.States.Leader AndAlso Not Me.secretPhaseActivated Then
			AudioManager.FadeSFXVolume("sfx_dlc_dogfight_p2_terrierjetpack_loop", 0F, 0.5F)
			Me.canteenAnimator.triggerCheer = True
			Me.StartPhase3()
		End If
	End Sub

	' Token: 0x0600002E RID: 46 RVA: 0x00051108 File Offset: 0x0004F508
	Private Sub StartPhase3()
		For i As Integer = 0 To Me.smokeFXPool.Count - 1
			Me.smokeFXPool(i).dead = True
			If Not Me.smokeFXPool(i).inUse Then
				Global.UnityEngine.[Object].Destroy(Me.smokeFXPool(i).gameObject)
			End If
		Next
		Me.StopAllCoroutines()
		MyBase.StartCoroutine(Me.leader_cr())
	End Sub

	' Token: 0x0600002F RID: 47 RVA: 0x00051184 File Offset: 0x0004F584
	Public Function CurrentEnemyPos() As Vector3
		Select Case Me.properties.CurrentState.stateName
			Case LevelProperties.Airplane.States.Main, LevelProperties.Airplane.States.Generic, LevelProperties.Airplane.States.Rocket
				Return Me.bulldogPlane.transform.position
			Case LevelProperties.Airplane.States.Terriers
				Dim num As Integer = Global.UnityEngine.Random.Range(0, Me.terriers.Count)
				If Me.terriers(num) IsNot Nothing Then
					Return Me.terriers(num).transform.position
				End If
				Return Vector3.zero
			Case LevelProperties.Airplane.States.Leader
				Return Me.leader.transform.position
			Case Else
				Return Vector3.zero
		End Select
	End Function

	' Token: 0x06000030 RID: 48 RVA: 0x0005122E File Offset: 0x0004F62E
	Public Function ScreenHorizontal() As Boolean
		Return Me.leader.camRotatedHorizontally
	End Function

	' Token: 0x06000031 RID: 49 RVA: 0x0005123C File Offset: 0x0004F63C
	Private Iterator Function terrier_cr() As IEnumerator
		Me.bulldogPlane.OnStageChange()
		While Me.bulldogPlane.state <> AirplaneLevelBulldogPlane.State.Main
			Yield Nothing
		End While
		Me.bulldogPlane.BulldogDeath()
		While Not Me.bulldogPlane.startPhaseTwo
			Yield Nothing
		End While
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		MyBase.StartCoroutine(Me.handle_terriers_cr())
		Return
	End Function

	' Token: 0x06000032 RID: 50 RVA: 0x00051258 File Offset: 0x0004F658
	Private Iterator Function handle_terriers_cr() As IEnumerator
		Dim p As LevelProperties.Airplane.Terriers = Me.properties.CurrentState.terriers
		Dim shotOrder As PatternString = New PatternString(p.shotOrder, True, True)
		Dim delayString As PatternString = New PatternString(p.shotDelayString, True, True)
		Dim typeString As PatternString = New PatternString(p.shotTypeString, True, True)
		Me.decreaseAmount = 0F
		Dim start As Vector3 = Me.airplane.transform.position
		Dim [end] As Vector3 = New Vector3(0F, Me.properties.CurrentState.plane.moveDown)
		Me.airplane.AutoMoveToPos(New Vector3(0F, Me.properties.CurrentState.plane.moveDown), True, True)
		Me.canteenAnimator.ForceLook(New Vector3(0F, Me.properties.CurrentState.plane.moveDown), 5)
		Yield CupheadTime.WaitForSeconds(Me, 2F)
		Me.terriers = New List(Of AirplaneLevelTerrier)(4)
		Dim startHealthPercentage As Single = Me.properties.CurrentState.healthTrigger
		Dim endHealthPercentage As Single = Me.properties.GetNextStateHealthTrigger()
		Dim endHealth As Single = endHealthPercentage * Me.properties.TotalHealth
		Dim startHealth As Single = startHealthPercentage * Me.properties.TotalHealth
		Me.terrierHPTotal = startHealth - endHealth
		Dim terrierHP As Single = Me.terrierHPTotal / 4F
		Dim isClockwise As Boolean = Rand.Bool()
		For i As Integer = 0 To 4 - 1
			Me.terriers.Add(Global.UnityEngine.[Object].Instantiate(Of AirplaneLevelTerrier)(Me.terrierPrefab))
			Me.terriers(i).Init(Me.terrierPivotPoint, CSng((90 * i)), Me.properties.CurrentState.terriers, terrierHP, AirplaneLevel.TERRIER_PIVOT_OFFSET_X(i), AirplaneLevel.TERRIER_PIVOT_OFFSET_Y(i), isClockwise, i)
		Next
		AudioManager.PlayLoop("sfx_dlc_dogfight_p2_terrierjetpack_loop")
		Yield Nothing
		AudioManager.FadeSFXVolume("sfx_dlc_dogfight_p2_terrierjetpack_loop", 0.3F, 3F)
		Dim readyToMove As Boolean = True
		While readyToMove
			readyToMove = True
			For Each airplaneLevelTerrier As AirplaneLevelTerrier In Me.terriers
				If airplaneLevelTerrier.ReadyToMove Then
					readyToMove = False
					Exit For
				End If
			Next
			Yield Nothing
		End While
		For Each airplaneLevelTerrier2 As AirplaneLevelTerrier In Me.terriers
			airplaneLevelTerrier2.StartMoving()
		Next
		MyBase.StartCoroutine(Me.move_pivot_cr())
		Dim CheckTerriersCR As Coroutine = MyBase.StartCoroutine(Me.check_terriers_cr(terrierHP))
		Dim delay As Single = 0F
		Dim isWow As Boolean = False
		While Not Me.AllTerriersSmoking()
			Dim shotRound As Boolean = False
			Dim isPink As Boolean = typeString.PopLetter() = "P"c
			delay = delayString.PopFloat()
			Yield CupheadTime.WaitForSeconds(Me, delay - Me.decreaseAmount)
			While Not shotRound
				Dim shot As Integer = shotOrder.PopInt()
				If Me.terriers(shot) IsNot Nothing AndAlso Not Me.terriers(shot).IsDead Then
					Dim num As Single
					If PlayerManager.BothPlayersActive() Then
						num = Mathf.Min(Vector3.Distance(Me.terriers(shot).GetPredictedAttackPos(), PlayerManager.GetPlayer(PlayerId.PlayerOne).transform.position), Vector3.Distance(Me.terriers(shot).GetPredictedAttackPos(), PlayerManager.GetPlayer(PlayerId.PlayerTwo).transform.position))
					Else
						num = Vector3.Distance(Me.terriers(shot).GetPredictedAttackPos(), PlayerManager.GetNext().transform.position)
					End If
					If num > p.minAttackDistance Then
						Me.terriers(shot).StartAttack(isPink, isWow)
						isWow = Not isWow
						shotRound = True
					End If
				End If
				Yield Nothing
			End While
		End While
		If Me.AllTerriersSmoking() Then
			Me.secretPhaseActivated = True
			MyBase.StartCoroutine(Me.handle_secret_intro_cr(isClockwise, terrierHP))
		End If
		Return
	End Function

	' Token: 0x06000033 RID: 51 RVA: 0x00051274 File Offset: 0x0004F674
	Private Iterator Function check_terriers_cr(terrierHP As Single) As IEnumerator
		Dim allDead As Boolean = False
		Dim deadOnes As Boolean() = New Boolean(Me.terriers.Count - 1) {}
		For j As Integer = 0 To deadOnes.Length - 1
			deadOnes(j) = False
		Next
		While Not allDead
			allDead = True
			Dim count As Integer = 0
			For i As Integer = 0 To Me.terriers.Count - 1
				If Me.terriers(i) Is Nothing OrElse Me.terriers(i).IsDead Then
					count += 1
					If Not deadOnes(i) Then
						Me.properties.DealDamage(terrierHP)
						Me.decreaseAmount += Me.properties.CurrentState.terriers.shotMinus
						deadOnes(i) = True
					End If
					Yield Nothing
				Else
					allDead = False
				End If
				If Me.terriers(i).lastOne Then
					count = 0
				End If
			Next
			If count = 3 Then
				For k As Integer = 0 To Me.terriers.Count - 1
					If Me.terriers(k) IsNot Nothing AndAlso Not Me.terriers(k).IsDead Then
						Me.terriers(k).lastOne = True
					End If
				Next
			End If
			Yield Nothing
		End While
		If Me.properties.CurrentState.stateName = LevelProperties.Airplane.States.Terriers Then
			Me.properties.DealDamageToNextNamedState()
		End If
		Return
	End Function

	' Token: 0x06000034 RID: 52 RVA: 0x00051298 File Offset: 0x0004F698
	Public Function AllTerriersSmoking() As Boolean
		If Me.secretPhaseActivated Then
			Return True
		End If
		Dim flag As Boolean = True
		For i As Integer = 0 To Me.terriers.Count - 1
			If Me.terriers(i) Is Nothing OrElse Me.terriers(i).IsDead OrElse Not Me.terriers(i).IsSmoking() Then
				flag = False
			End If
		Next
		Return flag
	End Function

	' Token: 0x06000035 RID: 53 RVA: 0x00051318 File Offset: 0x0004F718
	Public Sub CreateSmokeFX(pos As Vector3, vel As Vector3, isSmoking As Boolean, sortingLayerID As Integer, sortingOrder As Integer)
		Dim airplaneLevelTerrierSmokeFX As AirplaneLevelTerrierSmokeFX = Nothing
		For i As Integer = 0 To Me.smokeFXPool.Count - 1
			If Not Me.smokeFXPool(i).inUse Then
				airplaneLevelTerrierSmokeFX = Me.smokeFXPool(i)
				Exit For
			End If
		Next
		If airplaneLevelTerrierSmokeFX Is Nothing Then
			airplaneLevelTerrierSmokeFX = CType(Me.smokePrefab.Create(pos), AirplaneLevelTerrierSmokeFX)
			Me.smokeFXPool.Add(airplaneLevelTerrierSmokeFX)
		End If
		airplaneLevelTerrierSmokeFX.animator.SetInteger("Effect", Global.UnityEngine.Random.Range(0, 3))
		airplaneLevelTerrierSmokeFX.animator.Play(If((Not isSmoking), "A", "AGray"), 0, 0F)
		airplaneLevelTerrierSmokeFX.rend.sortingLayerID = sortingLayerID
		airplaneLevelTerrierSmokeFX.rend.sortingOrder = sortingOrder
		airplaneLevelTerrierSmokeFX.rend.enabled = True
		airplaneLevelTerrierSmokeFX.inUse = True
		airplaneLevelTerrierSmokeFX.vel = vel
		airplaneLevelTerrierSmokeFX.myTransform = airplaneLevelTerrierSmokeFX.transform
		airplaneLevelTerrierSmokeFX.myTransform.position = pos
	End Sub

	' Token: 0x06000036 RID: 54 RVA: 0x0005141E File Offset: 0x0004F81E
	Private Sub HandleTimelineHP(terrierHP As Single)
		MyBase.timeline.DealDamage(terrierHP)
	End Sub

	' Token: 0x06000037 RID: 55 RVA: 0x0005142C File Offset: 0x0004F82C
	Private Iterator Function move_pivot_cr() As IEnumerator
		Dim t As Single = 1.5F
		Dim val As Single = 1F
		Dim reversed As Boolean = Rand.Bool()
		Dim start As Single = Me.terrierPivotPoint.position.x + 30F
		Dim [end] As Single = Me.terrierPivotPoint.position.x - 30F
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		While True
			While t < 3F
				t += CupheadTime.FixedDelta
				If reversed Then
					Me.terrierPivotPoint.transform.SetPosition(New Single?(Mathf.Lerp(start, [end], val - t / 3F)), Nothing, Nothing)
				Else
					Me.terrierPivotPoint.transform.SetPosition(New Single?(Mathf.Lerp(start, [end], t / 3F)), Nothing, Nothing)
				End If
				Yield wait
			End While
			t = 0F
			reversed = Not reversed
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000038 RID: 56 RVA: 0x00051448 File Offset: 0x0004F848
	Private Iterator Function leader_cr() As IEnumerator
		Dim p As LevelProperties.Airplane.Plane = Me.properties.CurrentState.plane
		If Me.terriers Is Nothing Then
			While Me.bulldogPlane.state <> AirplaneLevelBulldogPlane.State.Main
				Yield Nothing
			End While
			Me.bulldogPlane.BulldogDeath()
		End If
		If Not Me.secretPhaseActivated Then
			Yield CupheadTime.WaitForSeconds(Me, 0.5F)
		End If
		Me.leader.gameObject.SetActive(True)
		Me.leader.StartLeader()
		If Me.secretPhaseActivated Then
			Me.leader.animator.Play("Intro", 0, 0.54F)
			Yield Nothing
		Else
			Me.airplane.AutoMoveToPos(New Vector3(0F, p.moveDownPhThree), True, True)
			Me.canteenAnimator.ForceLook(New Vector3(0F, p.moveDownPhThree), 3)
			Me.airplane.SetXRange(-225F, 225F)
			Yield CupheadTime.WaitForSeconds(Me, 1F)
		End If
		Dim target As Integer = Animator.StringToHash(Me.leader.animator.GetLayerName(0) + ".Intro")
		While Me.leader.animator.GetCurrentAnimatorStateInfo(0).fullPathHash = target
			If Me.leader.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.75F Then
				CType(Level.Current, AirplaneLevel).UpdateShadow(1F - Mathf.InverseLerp(0.75F, 1F, Me.leader.animator.GetCurrentAnimatorStateInfo(0).normalizedTime) * 0.1F)
			End If
			Yield Nothing
		End While
		If Not Me.secretPhaseActivated Then
			MyBase.StartCoroutine(Me.rotate_camera())
		Else
			MyBase.StartCoroutine(Me.secret_phase_cr())
		End If
		Return
	End Function

	' Token: 0x06000039 RID: 57 RVA: 0x00051463 File Offset: 0x0004F863
	Public Sub MoveBoundsIn()
		MyBase.StartCoroutine(Me.move_bounds_for_phase_three_cr())
	End Sub

	' Token: 0x0600003A RID: 58 RVA: 0x00051474 File Offset: 0x0004F874
	Private Iterator Function move_bounds_for_phase_three_cr() As IEnumerator
		Dim t As Single = 0F
		Dim time As Single = 0.2F
		Dim boundsStart As Single = CSng(Me.bounds.left)
		While t < time
			t += CupheadTime.FixedDelta
			For Each abstractPlayerController As AbstractPlayerController In PlayerManager.GetAllPlayers()
				If abstractPlayerController Then
					abstractPlayerController.transform.position = New Vector3(Mathf.Clamp(abstractPlayerController.transform.position.x, Mathf.Lerp(-boundsStart, -465F, t / time), Mathf.Lerp(boundsStart, 465F, t / time)), abstractPlayerController.transform.position.y)
					If abstractPlayerController.stats.State = PlayerStatsManager.PlayerState.Super Then
						Dim component As LevelPlayerWeaponManager = abstractPlayerController.GetComponent(Of LevelPlayerWeaponManager)()
						If component.activeSuper IsNot Nothing Then
							component.activeSuper.transform.position = New Vector3(Mathf.Clamp(component.transform.position.x, Mathf.Lerp(-boundsStart, -465F, t / time), Mathf.Lerp(boundsStart, 465F, t / time)), component.transform.position.y)
						End If
					End If
				End If
			Next
			Yield New WaitForFixedUpdate()
		End While
		Me.bounds.left = 465
		Me.bounds.right = 465
		Return
	End Function

	' Token: 0x0600003B RID: 59 RVA: 0x0005148F File Offset: 0x0004F88F
	Public Sub BlurBGCamera()
		MyBase.StartCoroutine(Me.bg_camera_blur_cr())
	End Sub

	' Token: 0x0600003C RID: 60 RVA: 0x000514A0 File Offset: 0x0004F8A0
	Private Iterator Function bg_camera_blur_cr() As IEnumerator
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		While Me.bgBlur.blurSize < 3F
			Me.bgBlur.blurSize += CupheadTime.FixedDelta * 10F
			Yield wait
		End While
		Me.bgBlur.blurSize = 3F
		Return
	End Function

	' Token: 0x0600003D RID: 61 RVA: 0x000514BC File Offset: 0x0004F8BC
	Private Iterator Function rotate_camera() As IEnumerator
		Dim p As LevelProperties.Airplane.Leader = Me.properties.CurrentState.leader
		Dim airplanePhase3Pos As Vector3 = New Vector3(0F, Me.properties.CurrentState.plane.moveDownPhThree)
		Dim startAngle As Single = 0F
		Dim endAngle As Single = 360F
		While Me.properties.CurrentState.stateName = LevelProperties.Airplane.States.Leader
			Yield CupheadTime.WaitForSeconds(Me, p.attackDelay)
			If Me.leader.camRotatedHorizontally Then
				AudioManager.PlayLoop("sfx_dlc_dogfight_p3_dogcopter_close_loop")
				AudioManager.FadeSFXVolume("sfx_dlc_dogfight_p3_dogcopter_close_loop", 0.7F, 0.5F)
				Me.leader.StartDropshot()
			Else
				Me.leader.StartLaser()
			End If
			While Me.leader.IsAttacking
				Yield Nothing
			End While
			If Level.Current.mode <> Level.Mode.Easy Then
				If endAngle = 0F Then
					startAngle = 360F
				Else
					startAngle = endAngle
				End If
				endAngle = startAngle - 90F
				Me.leader.RotateCamera()
				If Me.leader.camRotatedHorizontally Then
					Me.pitTop.gameObject.SetActive(False)
				End If
				Me.leaderAnimator.SetTrigger("Continue")
				Me.Rotating = True
				Dim animation As String = "Rotate_Start_" + If((Not Me.leader.camRotatedHorizontally), "ToVer", "ToHor")
				Yield Me.leaderAnimator.WaitForAnimationToEnd(Me, animation, False, True)
				Me.SFX_DOGFIGHT_PlayerPlane_PositionChangeFlyby()
				Dim rotateTime As Single = Me.rotateClip.length
				Dim animName As String = If((Not Me.leader.camRotatedHorizontally), "Rotate_Mid_ToVer", "Rotate_Mid_ToHor")
				Dim start As Single = Me.airplane.transform.position.y
				Dim [end] As Single = If((Not Me.leader.camRotatedHorizontally), airplanePhase3Pos.y, Me.properties.CurrentState.plane.moveWhenRotate)
				Me.SFX_DOGFIGHT_P3_Dogcopter_ScreenRotateEndClunk()
				While Me.leaderAnimator.GetCurrentAnimatorStateInfo(0).IsName(animName)
					Dim easePos As Single = If((Not Me.leader.camRotatedHorizontally), Mathf.Clamp(EaseUtils.EaseInOutSine(-0.1F, 1F, Me.leaderAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime), 0F, 1F), EaseUtils.EaseOutSine(0F, 1F, Me.leaderAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime))
					Dim rotationAmount As Single = startAngle + Mathf.Lerp(0F, endAngle - startAngle, easePos)
					Me.airplane.transform.SetPosition(Nothing, New Single?(Mathf.Lerp(start, [end], Me.leaderAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime)), Nothing)
					CupheadLevelCamera.Current.SetRotation(rotationAmount)
					Me.bgCamera.transform.SetEulerAngles(Nothing, Nothing, New Single?(rotationAmount))
					If startAngle = 180F OrElse startAngle = 90F Then
						Me.leaderAnimator.transform.SetEulerAngles(Nothing, Nothing, New Single?(180F + rotationAmount))
					Else
						Me.leaderAnimator.transform.SetEulerAngles(Nothing, Nothing, New Single?(rotationAmount))
					End If
					Yield Nothing
				End While
				If Not Me.leader.camRotatedHorizontally Then
					CupheadLevelCamera.Current.Shake(20F, 0.5F, False)
					Me.pitTop.gameObject.SetActive(True)
				Else
					CupheadLevelCamera.Current.Shake(30F, 0.7F, False)
				End If
				CupheadLevelCamera.Current.SetRotation(endAngle)
				Me.bgCamera.transform.SetEulerAngles(Nothing, Nothing, New Single?(endAngle))
				If Me.leader.camRotatedHorizontally Then
					Me.leaderAnimator.transform.SetEulerAngles(Nothing, Nothing, New Single?(If((startAngle <> 180F), endAngle, (-endAngle))))
				Else
					Me.leaderAnimator.transform.SetEulerAngles(Nothing, Nothing, New Single?(180F))
					AudioManager.FadeSFXVolume("sfx_dlc_dogfight_p3_dogcopter_close_loop", 0F, 0.5F)
				End If
				Yield Me.leaderAnimator.WaitForAnimationToEnd(Me, If((Not Me.leader.camRotatedHorizontally), "Rotate_End_ToVer", "Rotate_End_ToHor"), False, True)
				Me.leaderAnimator.SetBool("CloseUp", Me.leader.camRotatedHorizontally)
				Me.leaderAnimator.Update(0F)
				Me.Rotating = False
			End If
		End While
		Return
	End Function

	' Token: 0x0600003E RID: 62 RVA: 0x000514D8 File Offset: 0x0004F8D8
	Private Sub LateUpdate()
		If Not Me.Rotating Then
			Return
		End If
		If Me.leaderAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle") Then
			Me.leaderAnimator.transform.SetEulerAngles(Nothing, Nothing, New Single?(0F))
		End If
		If Me.leaderAnimator.GetCurrentAnimatorStateInfo(0).IsName("Rotate_End_ToHor") Then
			Me.leaderAnimator.Play("Push_Wait", 3, 0F)
			Me.leaderAnimator.Update(0F)
		End If
		If Me.leaderAnimator.GetCurrentAnimatorStateInfo(0).IsName("Rotate_Mid_ToVer") Then
			Me.leaderAnimator.Play("None", 3, 0F)
			Me.leaderAnimator.Update(0F)
		End If
		If Me.properties.CurrentState.stateName = LevelProperties.Airplane.States.Terriers AndAlso Not Me.terriersIntroFinished Then
			Dim deltaTime As Single = Time.deltaTime
			For i As Integer = 0 To Me.smokeFXPool.Count - 1
				If Me.smokeFXPool(i) IsNot Nothing AndAlso Me.smokeFXPool(i).inUse Then
					Me.smokeFXPool(i).[Step](deltaTime)
				End If
			Next
		End If
	End Sub

	' Token: 0x0600003F RID: 63 RVA: 0x00051650 File Offset: 0x0004FA50
	Private Iterator Function handle_secret_intro_cr(clockwise As Boolean, terrierHP As Single) As IEnumerator
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		Me.canteenAnimator.triggerCheer = True
		For i As Integer = 0 To Me.terriers.Count - 1
			Me.terriers(i).StartSecret()
		Next
		Dim start As Vector3 = Me.airplane.transform.position
		Dim [end] As Vector3 = New Vector3(325F * CSng(If((Not clockwise), (-1), 1)), Me.airplane.transform.position.y)
		Dim time As Single = Mathf.Max(1F, Vector3.Distance(start, [end]) / 200F)
		If clockwise Then
			Me.airplane.SetXRange(250F, 480F)
		Else
			Me.airplane.SetXRange(-480F, -250F)
		End If
		Me.canteenAnimator.ForceLook(If((Not clockwise), (Vector3.right * 1000F), (Vector3.left * 1000F)), 9)
		Me.airplane.AutoMoveToPos([end], True, True)
		Yield CupheadTime.WaitForSeconds(Me, time / 2F)
		If clockwise Then
			Me.secretIntro.transform.localScale = New Vector3(-1F, 1F)
		End If
		Yield MyBase.StartCoroutine(Me.wait_for_terrier_to_reach_chomp_position_cr())
		Me.secretIntro.transform.position = New Vector3(-780F * CSng(If((Not clockwise), (-1), 1)), Me.secretIntro.transform.position.y)
		Me.secretIntro.gameObject.SetActive(True)
		Me.secretIntro.Play("Chomp", 0, 0F)
		AudioManager.Play("sfx_dlc_dogfight_ps_dogcopterintro_chompstart")
		MyBase.StartCoroutine(Me.handle_secret_intro_move_in_cr(clockwise))
		Dim hash As Integer = Animator.StringToHash(Me.secretIntro.GetLayerName(0) + ".Chomp")
		While Me.terriers.Count > 0
			While Me.secretIntro.GetCurrentAnimatorStateInfo(0).normalizedTime Mod 1F < 0.55F AndAlso Me.secretIntro.GetCurrentAnimatorStateInfo(0).fullPathHash = hash
				Yield wait
			End While
			Global.UnityEngine.[Object].Destroy(Me.terriers(Me.secretTargetTerrier).gameObject)
			Me.terriers.RemoveAt(Me.secretTargetTerrier)
			If Me.terriers.Count > 0 Then
				Yield MyBase.StartCoroutine(Me.wait_for_terrier_to_reach_chomp_position_cr())
				Me.secretIntro.Play("Chomp", 0, 0F)
				Me.secretIntro.Update(0F)
			End If
			If Me.terriers.Count = 1 Then
				Me.secretIntro.SetTrigger("Exit")
				AudioManager.FadeSFXVolume("sfx_dlc_dogfight_p2_terrierjetpack_loop", 0F, 0.5F)
			End If
		End While
		Yield Me.secretIntro.WaitForAnimationToStart(Me, "Exit", False)
		AudioManager.Play("sfx_dlc_dogfight_ps_dogcopterintro_chomplicklip")
		While Me.secretIntro.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.5F
			Yield wait
		End While
		AudioManager.Play("sfx_dlc_dogfight_ps_dogcopterintro_leaderintro")
		start = Me.airplane.transform.position
		[end] = New Vector3(0F, Me.properties.CurrentState.plane.moveDownPhThree)
		Me.airplane.AutoMoveToPos([end], True, True)
		Me.canteenAnimator.ForceLook([end], 3)
		Me.airplane.SetXRange(-225F, 225F)
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		Yield Me.secretIntro.WaitForAnimationToEnd(Me, "Exit", False, False)
		Me.secretIntro.gameObject.SetActive(False)
		Yield CupheadTime.WaitForSeconds(Me, 0.3F)
		Me.properties.DealDamage(terrierHP * CSng(Me.terriers.Count))
		Me.StartPhase3()
		Return
	End Function

	' Token: 0x06000040 RID: 64 RVA: 0x0005167C File Offset: 0x0004FA7C
	Private Iterator Function wait_for_terrier_to_reach_chomp_position_cr() As IEnumerator
		Dim foundNext As Boolean = False
		While Not foundNext
			For i As Integer = 0 To Me.terriers.Count - 1
				If Me.terriers(i).RelativeAngle() > 2.8415928F AndAlso Me.terriers(i).RelativeAngle() < 3.2415926F Then
					foundNext = True
					Me.secretTargetTerrier = i
					Me.terriers(Me.secretTargetTerrier).PrepareForChomp()
				End If
			Next
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000041 RID: 65 RVA: 0x00051698 File Offset: 0x0004FA98
	Private Iterator Function handle_secret_intro_move_in_cr(clockwise As Boolean) As IEnumerator
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		Yield wait
		Dim t As Single = 0F
		While t < 1F
			Me.secretIntro.transform.position = New Vector3(EaseUtils.EaseOutSine(-780F * CSng(If((Not clockwise), (-1), 1)), -35F * CSng(If((Not clockwise), (-1), 1)), t), Me.secretIntro.transform.position.y)
			If t <= Me.secretIntro.GetCurrentAnimatorStateInfo(0).normalizedTime Then
				t = Me.secretIntro.GetCurrentAnimatorStateInfo(0).normalizedTime
			Else
				t = 1F
			End If
			Yield wait
		End While
		Me.secretIntro.transform.position = New Vector3(-35F * CSng(If((Not clockwise), (-1), 1)), Me.secretIntro.transform.position.y)
		Return
	End Function

	' Token: 0x06000042 RID: 66 RVA: 0x000516BC File Offset: 0x0004FABC
	Public Function GetNextHole() As Integer
		Dim list As List(Of Integer) = New List(Of Integer)()
		For i As Integer = 0 To Me.secretPhaseHoleOccupied.Length - 1
			If Not Me.secretPhaseHoleOccupied(i) Then
				list.Add(i)
			End If
		Next
		If list.Count = 0 Then
			Return -1
		End If
		Dim num As Integer = list(Global.UnityEngine.Random.Range(0, list.Count))
		Me.secretPhaseHoleOccupied(num) = True
		Return num
	End Function

	' Token: 0x06000043 RID: 67 RVA: 0x00051726 File Offset: 0x0004FB26
	Public Function GetHolePosition(value As Integer, isLeader As Boolean) As Vector3
		Return If((Not isLeader), Me.secretPhaseTerrierPositions(value).position, Me.secretPhaseLeaderPositions(value).position)
	End Function

	' Token: 0x06000044 RID: 68 RVA: 0x0005174D File Offset: 0x0004FB4D
	Public Function GetLeaderDeathPosition(value As Integer) As Vector3
		Return Me.leaderDeathPositions(value).position
	End Function

	' Token: 0x06000045 RID: 69 RVA: 0x0005175C File Offset: 0x0004FB5C
	Public Sub LeaveHole(value As Integer)
		Me.secretPhaseHoleOccupied(value) = False
	End Sub

	' Token: 0x06000046 RID: 70 RVA: 0x00051767 File Offset: 0x0004FB67
	Public Sub OccupyHole(value As Integer)
		Me.secretPhaseHoleOccupied(value) = True
	End Sub

	' Token: 0x06000047 RID: 71 RVA: 0x00051774 File Offset: 0x0004FB74
	Private Iterator Function secret_phase_cr() As IEnumerator
		Me.leader.OpenPawHoles()
		Yield CupheadTime.WaitForSeconds(Me, 0.25F)
		Me.secretPhaseHoleOccupied = New Boolean(Me.secretPhaseTerrierPositions.Length - 1) {}
		Me.secretLeader.gameObject.SetActive(True)
		Me.secretLeader.LevelInit(Me.properties)
		For i As Integer = 0 To 4 - 1
			Me.secretTerriers(i).gameObject.SetActive(True)
			Me.secretTerriers(i).LevelInit(Me.properties)
		Next
		Dim dogAttackDelayString As PatternString = New PatternString(Me.properties.CurrentState.secretTerriers.dogAttackDelayString, True, True)
		Dim dogAttackOrderString As PatternString = New PatternString(Me.properties.CurrentState.secretTerriers.dogAttackOrderString, True, True)
		While True
			Yield CupheadTime.WaitForSeconds(Me, dogAttackDelayString.PopFloat())
			Dim nextAttacker As Integer = 0
			nextAttacker = dogAttackOrderString.PopInt()
			Me.secretTerriers(nextAttacker).TryStartAttack()
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000048 RID: 72 RVA: 0x0005178F File Offset: 0x0004FB8F
	Public Sub LeaderDeath()
		Me.secretLeader.gameObject.SetActive(True)
		Me.secretLeader.DieMain()
	End Sub

	' Token: 0x06000049 RID: 73 RVA: 0x000517AD File Offset: 0x0004FBAD
	Private Sub SFX_DOGFIGHT_PlayerPlane_PositionChangeFlyby()
		AudioManager.Play("sfx_DLC_Dogfight_PlayerPlane_PositionChangeFlyby")
	End Sub

	' Token: 0x0600004A RID: 74 RVA: 0x000517B9 File Offset: 0x0004FBB9
	Private Sub SFX_DOGFIGHT_P3_Dogcopter_ScreenRotateEndClunk()
		AudioManager.Play("sfx_DLC_Dogfight_P3_DogCopter_ScreenRotateEndClunk")
	End Sub

	' Token: 0x0600004B RID: 75 RVA: 0x000517C8 File Offset: 0x0004FBC8
	Private Sub WORKAROUND_NullifyFields()
		Me.leader = Nothing
		Me.airplane = Nothing
		Me.canteenAnimator = Nothing
		Me.bulldogPlane = Nothing
		Me.bulldogParachute = Nothing
		Me.bulldogCatAttack = Nothing
		Me.secretIntro = Nothing
		Me.leaderAnimator = Nothing
		Me.secretLeader = Nothing
		Me.secretTerriers = Nothing
		Me.rotateClip = Nothing
		Me.pitTop = Nothing
		Me.terrierPivotPoint = Nothing
		Me.secretPhaseTerrierPositions = Nothing
		Me.secretPhaseLeaderPositions = Nothing
		Me.leaderDeathPositions = Nothing
		Me.secretPhaseHoleOccupied = Nothing
		Me.bgBlur = Nothing
		Me.bgCamera = Nothing
		Me.terrierPrefab = Nothing
		Me._bossPortraitMain = Nothing
		Me._bossPortraitPhaseTwo = Nothing
		Me._bossPortraitPhaseThree = Nothing
		Me._bossPortraitPhaseSecret = Nothing
		Me._bossQuoteMain = Nothing
		Me._bossQuotePhaseTwo = Nothing
		Me._bossQuotePhaseThree = Nothing
		Me.terriers = Nothing
		Me.smokeFXPool = Nothing
		Me.smokePrefab = Nothing
		Me.shadowableRenderers = Nothing
	End Sub

	' Token: 0x040000AA RID: 170
	Private properties As LevelProperties.Airplane

	' Token: 0x040000AB RID: 171
	Private Const PHASE_TWO_DELAY As Single = 1F

	' Token: 0x040000AC RID: 172
	Private Const PHASE_THREE_DELAY As Single = 0.5F

	' Token: 0x040000AD RID: 173
	Private Const SECRET_INTRO_REAPPEAR_DELAY As Single = 0.3F

	' Token: 0x040000AE RID: 174
	Private Const PHASE_THREE_BOUNDS As Single = 465F

	' Token: 0x040000AF RID: 175
	Private Const PLANE_MAX_PHASE_ONE As Single = 480F

	' Token: 0x040000B0 RID: 176
	Private Const PLANE_MAX_PHASE_THREE As Single = 225F

	' Token: 0x040000B1 RID: 177
	Private Const TERRIERCOUNT As Integer = 4

	' Token: 0x040000B2 RID: 178
	Private Shared TERRIER_PIVOT_OFFSET_X As Single() = New Single() { 20F, -20F, 20F, -20F }

	' Token: 0x040000B3 RID: 179
	Private Shared TERRIER_PIVOT_OFFSET_Y As Single() = New Single() { 20F, -20F, -20F, 20F }

	' Token: 0x040000B4 RID: 180
	Private Const PIVOT_MOVE As Single = 30F

	' Token: 0x040000B5 RID: 181
	Private Const MOVE_TIME As Single = 3F

	' Token: 0x040000B6 RID: 182
	Private Const SECRET_INTRO_X_START As Single = -780F

	' Token: 0x040000B7 RID: 183
	Private Const SECRET_INTRO_X_END As Single = -35F

	' Token: 0x040000B8 RID: 184
	Private Const SECRET_INTRO_PLANE_POS As Single = 325F

	' Token: 0x040000B9 RID: 185
	<SerializeField()>
	Private airplane As AirplaneLevelPlayerPlane

	' Token: 0x040000BA RID: 186
	<SerializeField()>
	Private canteenAnimator As AirplaneLevelCanteenAnimator

	' Token: 0x040000BB RID: 187
	<SerializeField()>
	Private bulldogPlane As AirplaneLevelBulldogPlane

	' Token: 0x040000BC RID: 188
	<SerializeField()>
	Private bulldogParachute As AirplaneLevelBulldogParachute

	' Token: 0x040000BD RID: 189
	<SerializeField()>
	Private bulldogCatAttack As AirplaneLevelBulldogCatAttack

	' Token: 0x040000BE RID: 190
	<SerializeField()>
	Public leader As AirplaneLevelLeader

	' Token: 0x040000BF RID: 191
	<SerializeField()>
	Private secretIntro As Animator

	' Token: 0x040000C0 RID: 192
	<SerializeField()>
	Private leaderAnimator As Animator

	' Token: 0x040000C1 RID: 193
	<SerializeField()>
	Private secretLeader As AirplaneLevelSecretLeader

	' Token: 0x040000C2 RID: 194
	<SerializeField()>
	Private secretTerriers As AirplaneLevelSecretTerrier()

	' Token: 0x040000C3 RID: 195
	<SerializeField()>
	Private rotateClip As AnimationClip

	' Token: 0x040000C4 RID: 196
	<SerializeField()>
	Private pitTop As LevelPit

	' Token: 0x040000C5 RID: 197
	<SerializeField()>
	Private terrierPivotPoint As Transform

	' Token: 0x040000C6 RID: 198
	<SerializeField()>
	Private secretPhaseTerrierPositions As Transform()

	' Token: 0x040000C7 RID: 199
	<SerializeField()>
	Private secretPhaseLeaderPositions As Transform()

	' Token: 0x040000C8 RID: 200
	<SerializeField()>
	Private leaderDeathPositions As Transform()

	' Token: 0x040000C9 RID: 201
	Private secretPhaseHoleOccupied As Boolean()

	' Token: 0x040000CA RID: 202
	<SerializeField()>
	Private bgBlur As BlurOptimized

	' Token: 0x040000CB RID: 203
	<SerializeField()>
	Private bgCamera As Global.UnityEngine.Camera

	' Token: 0x040000CC RID: 204
	<Header("Prefabs")>
	<SerializeField()>
	Private terrierPrefab As AirplaneLevelTerrier

	' Token: 0x040000CD RID: 205
	Public terriersIntroFinished As Boolean

	' Token: 0x040000CE RID: 206
	<Header("Boss Info")>
	<SerializeField()>
	Private _bossPortraitMain As Sprite

	' Token: 0x040000CF RID: 207
	<SerializeField()>
	Private _bossPortraitPhaseTwo As Sprite

	' Token: 0x040000D0 RID: 208
	<SerializeField()>
	Private _bossPortraitPhaseThree As Sprite

	' Token: 0x040000D1 RID: 209
	<SerializeField()>
	Private _bossPortraitPhaseSecret As Sprite

	' Token: 0x040000D2 RID: 210
	<SerializeField()>
	Private _bossQuoteMain As String

	' Token: 0x040000D3 RID: 211
	<SerializeField()>
	Private _bossQuotePhaseTwo As String

	' Token: 0x040000D4 RID: 212
	<SerializeField()>
	Private _bossQuotePhaseThree As String

	' Token: 0x040000D5 RID: 213
	Private terrierHPTotal As Single

	' Token: 0x040000D6 RID: 214
	Private decreaseAmount As Single

	' Token: 0x040000D7 RID: 215
	Private allTerriersDead As Boolean

	' Token: 0x040000D8 RID: 216
	Private secretPhaseActivated As Boolean

	' Token: 0x040000D9 RID: 217
	Private secretTargetTerrier As Integer

	' Token: 0x040000DB RID: 219
	Private terriers As List(Of AirplaneLevelTerrier)

	' Token: 0x040000DC RID: 220
	Private smokeFXPool As List(Of AirplaneLevelTerrierSmokeFX) = New List(Of AirplaneLevelTerrierSmokeFX)()

	' Token: 0x040000DD RID: 221
	<SerializeField()>
	Private smokePrefab As AirplaneLevelTerrierSmokeFX

	' Token: 0x040000DE RID: 222
	Private shadowableRenderers As HashSet(Of SpriteRenderer) = New HashSet(Of SpriteRenderer)()

	' Token: 0x040000DF RID: 223
	Private currentShadowLevel As Single = 1F
End Class
