Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x02000091 RID: 145
Public Class ChessCastleLevel
	Inherits Level

	' Token: 0x06000193 RID: 403 RVA: 0x00058538 File Offset: 0x00056938
	Protected Overrides Sub PartialInit()
		Me.properties = LevelProperties.ChessCastle.GetMode(MyBase.mode)
		AddHandler Me.properties.OnStateChange, AddressOf MyBase.zHack_OnStateChanged
		AddHandler Me.properties.OnBossDeath, AddressOf MyBase.zHack_OnWin
		MyBase.timeline = Me.properties.CreateTimeline(MyBase.mode)
		Me.goalTimes = Me.properties.goalTimes
		AddHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		MyBase.PartialInit()
	End Sub

	' Token: 0x17000043 RID: 67
	' (get) Token: 0x06000194 RID: 404 RVA: 0x000585CE File Offset: 0x000569CE
	Public Overrides ReadOnly Property CurrentLevel As Levels
		Get
			Return Levels.ChessCastle
		End Get
	End Property

	' Token: 0x17000044 RID: 68
	' (get) Token: 0x06000195 RID: 405 RVA: 0x000585D5 File Offset: 0x000569D5
	Public Overrides ReadOnly Property CurrentScene As Scenes
		Get
			Return Scenes.scene_level_chess_castle
		End Get
	End Property

	' Token: 0x17000045 RID: 69
	' (get) Token: 0x06000196 RID: 406 RVA: 0x000585D9 File Offset: 0x000569D9
	Public Overrides ReadOnly Property BossPortrait As Sprite
		Get
			Return Me._bossPortrait
		End Get
	End Property

	' Token: 0x17000046 RID: 70
	' (get) Token: 0x06000197 RID: 407 RVA: 0x000585E1 File Offset: 0x000569E1
	Public Overrides ReadOnly Property BossQuote As String
		Get
			Return Me._bossQuote
		End Get
	End Property

	' Token: 0x17000047 RID: 71
	' (get) Token: 0x06000198 RID: 408 RVA: 0x000585E9 File Offset: 0x000569E9
	' (set) Token: 0x06000199 RID: 409 RVA: 0x000585F1 File Offset: 0x000569F1
	Public Property rotating As Boolean

	' Token: 0x17000048 RID: 72
	' (get) Token: 0x0600019A RID: 410 RVA: 0x000585FA File Offset: 0x000569FA
	Public ReadOnly Property rotationMultiplier As Single
		Get
			Return Me._rotationMultiplier
		End Get
	End Property

	' Token: 0x0600019B RID: 411 RVA: 0x00058604 File Offset: 0x00056A04
	Protected Overrides Sub OnEnable()
		MyBase.OnEnable()
		AddHandler SceneLoader.OnFadeOutStartEvent, AddressOf Me.onFadeOutStartEventHandler
		AddHandler MyBase.OnIntroEvent, AddressOf Me.onIntroEventHandler
		AddHandler Dialoguer.events.onStarted, AddressOf Me.onDialogueStartedHandler
		AddHandler Dialoguer.events.onMessageEvent, AddressOf Me.onDialogueMessageHandler
		AddHandler Dialoguer.events.onEnded, AddressOf Me.onDialogueEndedHandler
		AddHandler Dialoguer.events.onInstantlyEnded, AddressOf Me.onDialogueEndedHandler
		AddHandler Dialoguer.events.onTextPhase, AddressOf Me.onDialogueAdvancedHandler
	End Sub

	' Token: 0x0600019C RID: 412 RVA: 0x000586A8 File Offset: 0x00056AA8
	Protected Overrides Sub OnDisable()
		MyBase.OnDisable()
		RemoveHandler SceneLoader.OnFadeOutStartEvent, AddressOf Me.onFadeOutStartEventHandler
		RemoveHandler MyBase.OnIntroEvent, AddressOf Me.onIntroEventHandler
		RemoveHandler Dialoguer.events.onStarted, AddressOf Me.onDialogueStartedHandler
		RemoveHandler Dialoguer.events.onMessageEvent, AddressOf Me.onDialogueMessageHandler
		RemoveHandler Dialoguer.events.onEnded, AddressOf Me.onDialogueEndedHandler
		RemoveHandler Dialoguer.events.onInstantlyEnded, AddressOf Me.onDialogueEndedHandler
		RemoveHandler Dialoguer.events.onTextPhase, AddressOf Me.onDialogueAdvancedHandler
	End Sub

	' Token: 0x0600019D RID: 413 RVA: 0x0005874C File Offset: 0x00056B4C
	Protected Overrides Sub Awake()
		Me.previousLevel = Level.PreviousLevel
		Me.previouslyWon = Level.Won
		If Me.previouslyWon Then
			Me.attemptsToBeat = PlayerData.Data.chessBossAttemptCounter
			PlayerData.Data.ResetKingOfGamesCounter()
			PlayerData.SaveCurrentFile()
		End If
		MyBase.Awake()
	End Sub

	' Token: 0x0600019E RID: 414 RVA: 0x000587A0 File Offset: 0x00056BA0
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Dim obj As Object = Nothing
		Dim abstractLevelInteractiveEntity As AbstractLevelInteractiveEntity = obj
		Me.exitEntity = obj
		Me.startEntity = abstractLevelInteractiveEntity
		Me.playerStartLevelEffects = Nothing
		Me.dialogueInteractionPoint = Nothing
		Me.speechBubble = Nothing
		Me.coinPrefab = Nothing
		Me.kingAnimator = Nothing
		Me.castleAnimator = Nothing
		Me.platformAnimator = Nothing
		Me.cloudPrefab = Nothing
	End Sub

	' Token: 0x0600019F RID: 415 RVA: 0x000587FC File Offset: 0x00056BFC
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.speechBubbleBasePosition = Me.speechBubble.basePosition
		Me.updateDialogueState()
		Dim flag As Boolean = PlayerData.Data.CountLevelsCompleted(Level.kingOfGamesLevels) = Level.kingOfGamesLevels.Length
		MyBase.StartCoroutine(Me.cloudSpawn_cr())
		AudioManager.PlayLoop("sfx_dlc_kog_castle_amb_wind_loop")
		AudioManager.FadeSFXVolumeLinear("sfx_dlc_kog_castle_amb_wind_loop", 0.4F, 1F)
		Dim levels As Levels
		If Me.previouslyWon AndAlso TypeOf SceneLoader.CurrentContext Is GauntletContext AndAlso CType(SceneLoader.CurrentContext, GauntletContext).complete Then
			levels = Levels.ChessQueen
			Me.movePlayersToDialoguePositions()
			Me.dialogueInteractionPoint.dialogueInteraction = DialoguerDialogues.KingOfGamesVictory_WDLC
			Dialoguer.SetGlobalFloat(ChessCastleLevel.KingOfGamesVictoryDialoguerStateIndex, -3F)
		ElseIf Me.previouslyWon AndAlso (Not flag OrElse Dialoguer.GetGlobalFloat(ChessCastleLevel.KingOfGamesDialoguerStateIndex) <> CSng(ChessCastleLevel.KingOfGamesFinalDialogueState)) Then
			Me.movePlayersToDialoguePositions()
			Me.dialogueInteractionPoint.dialogueInteraction = DialoguerDialogues.KingOfGamesVictory_WDLC
			levels = Me.calculatePreviousLevel()
			If flag Then
				levels = Levels.ChessQueen
				Dialoguer.SetGlobalFloat(ChessCastleLevel.KingOfGamesVictoryDialoguerStateIndex, -2F)
			ElseIf Me.attemptsToBeat < ChessCastleLevel.MaxAttemptsToContinue Then
				Dim num As Integer = CInt(Dialoguer.GetGlobalFloat(ChessCastleLevel.KingOfGamesVictoryDialoguerCountIndex))
				num += 1
				Dialoguer.SetGlobalFloat(ChessCastleLevel.KingOfGamesVictoryDialoguerCountIndex, CSng(num))
				Dialoguer.SetGlobalFloat(ChessCastleLevel.KingOfGamesVictoryDialoguerStateIndex, 0F)
			Else
				Dialoguer.SetGlobalFloat(ChessCastleLevel.KingOfGamesVictoryDialoguerStateIndex, -1F)
			End If
		Else
			Dialoguer.SetGlobalFloat(ChessCastleLevel.KingOfGamesVictoryDialoguerStateIndex, -2F)
			If flag Then
				If Array.Exists(Of Levels)(Level.kingOfGamesLevels, Function(level As Levels) level = Me.previousLevel) Then
					levels = Me.previousLevel
					Me.movePlayersToDialoguePositions()
				Else
					levels = Levels.ChessPawn
				End If
			Else
				levels = Me.calculateCurrentLevel()
			End If
		End If
		If Dialoguer.GetGlobalFloat(ChessCastleLevel.KingOfGamesDialoguerStateIndex) = 0F Then
			Me.firstEntry = True
		End If
		If Not Me.previouslyWon Then
			Me.introCameraMovementCoroutine = MyBase.StartCoroutine(Me.panCamera_cr())
		End If
		If Me.firstEntry OrElse Me.previouslyWon Then
			Dim text As String = ChessCastleLevel.LevelPrefixes(Array.IndexOf(Of Levels)(Level.kingOfGamesLevels, levels))
			Me.castleAnimator.Play(text + "Idle", ChessCastleLevel.CastleBaseLayer)
			Me.castleAnimator.Play(text + "Open", ChessCastleLevel.CastleDoorLayer, 1F)
			If levels = Levels.ChessBishop OrElse levels = Levels.ChessQueen Then
				Me.castleAnimator.Play(text, ChessCastleLevel.CastleFlairLayer, 1F)
			End If
			Me.platformAnimator.Play("Stop", ChessCastleLevel.PlatformBaseLayer, 1F)
			Me.setTargetLevel(levels, False)
		Else
			Me.rotate(Levels.ChessPawn, levels, False, True)
		End If
	End Sub

	' Token: 0x060001A0 RID: 416 RVA: 0x00058ACC File Offset: 0x00056ECC
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.introCameraMovementCoroutine Is Nothing Then
			Me.cameraSineAccumulator += CupheadTime.Delta
			Dim vector As Vector3 = New Vector3(0F, Me.sineAmplitude * Mathf.Sin(Me.cameraSineAccumulator / Me.sinePeriod * 2F * 3.1415927F + 1.5707964F))
			CupheadLevelCamera.Current.SetManualFloat(vector)
		End If
	End Sub

	' Token: 0x060001A1 RID: 417 RVA: 0x00058B43 File Offset: 0x00056F43
	Protected Overrides Sub OnLevelStart()
		If Me.firstEntry Then
			MyBase.StartCoroutine(Me.firstEntry_cr())
		ElseIf Me.dialogueInteractionPoint.dialogueInteraction = DialoguerDialogues.KingOfGamesVictory_WDLC Then
			MyBase.StartCoroutine(Me.postWinEntry_cr())
		End If
	End Sub

	' Token: 0x060001A2 RID: 418 RVA: 0x00058B84 File Offset: 0x00056F84
	Protected Overrides Sub OnTransitionInComplete()
		Dim flag As Boolean = PlayerData.Data.CountLevelsCompleted(Level.kingOfGamesLevels) = Level.kingOfGamesLevels.Length
		MyBase.OnTransitionInComplete()
		If flag Then
			AudioManager.StartBGMAlternate(1)
		ElseIf Dialoguer.GetGlobalFloat(ChessCastleLevel.KingOfGamesDialoguerStateIndex) = 0F Then
			AudioManager.PlayBGM()
		Else
			AudioManager.StartBGMAlternate(0)
		End If
	End Sub

	' Token: 0x060001A3 RID: 419 RVA: 0x00058BE8 File Offset: 0x00056FE8
	Private Iterator Function panCamera_cr() As IEnumerator
		CupheadLevelCamera.Current.SetManualFloat(New Vector3(0F, -Me.introPanAmount))
		While Not Me.beginIntroPan
			Yield Nothing
		End While
		Dim elapsedTime As Single = 0F
		While elapsedTime < Me.introPanDuration
			Yield Nothing
			elapsedTime += CupheadTime.Delta
			CupheadLevelCamera.Current.SetManualFloat(New Vector3(0F, EaseUtils.EaseOutCubic(-Me.introPanAmount, Me.sineAmplitude, elapsedTime / Me.introPanDuration)))
		End While
		Me.introCameraMovementCoroutine = Nothing
		Return
	End Function

	' Token: 0x060001A4 RID: 420 RVA: 0x00058C04 File Offset: 0x00057004
	Private Iterator Function firstEntry_cr() As IEnumerator
		Me.castleAnimator.Play(ChessCastleLevel.LevelPrefixes(0) + "Close", ChessCastleLevel.CastleDoorLayer, 1F)
		Me.startEntity.enabled = False
		Dim castleIntroMusic As AudioSource = GameObject.Find("MUS_CastleIntro").GetComponent(Of AudioSource)()
		While castleIntroMusic.isPlaying
			Yield Nothing
		End While
		AudioManager.StartBGMAlternate(0)
		Return
	End Function

	' Token: 0x060001A5 RID: 421 RVA: 0x00058C20 File Offset: 0x00057020
	Private Iterator Function postWinEntry_cr() As IEnumerator
		AudioManager.StopBGM()
		Dim behaviour As Behaviour = Me.startEntity
		Dim flag As Boolean = False
		Me.dialogueInteractionPoint.enabled = flag
		flag = flag
		Me.exitEntity.enabled = flag
		behaviour.enabled = flag
		Yield CupheadTime.WaitForSeconds(Me, 0.35F)
		If Dialoguer.GetGlobalFloat(ChessCastleLevel.KingOfGamesDialoguerStateIndex) <> 0F Then
			AudioManager.StartBGMAlternate(0)
		End If
		Me.dialogueInteractionPoint.BeginDialogue()
		Yield CupheadTime.WaitForSeconds(Me, 2F)
		Dim behaviour2 As Behaviour = Me.startEntity
		flag = True
		Me.dialogueInteractionPoint.enabled = flag
		flag = flag
		Me.exitEntity.enabled = flag
		behaviour2.enabled = flag
		Return
	End Function

	' Token: 0x060001A6 RID: 422 RVA: 0x00058C3B File Offset: 0x0005703B
	Private Sub rotate(startLevel As Levels, endLevel As Levels, gauntlet As Boolean, intro As Boolean)
		If Me.rotationCoroutine IsNot Nothing Then
			Return
		End If
		Me.rotationCoroutine = MyBase.StartCoroutine(Me.rotate_cr(startLevel, endLevel, gauntlet, intro))
	End Sub

	' Token: 0x060001A7 RID: 423 RVA: 0x00058C60 File Offset: 0x00057060
	Private Iterator Function rotate_cr(startLevel As Levels, endLevel As Levels, gauntlet As Boolean, intro As Boolean) As IEnumerator
		If Me.gauntletSparklesCoroutine IsNot Nothing Then
			MyBase.StopCoroutine(Me.gauntletSparklesCoroutine)
			Me.gauntletSparklesCoroutine = Nothing
		End If
		Dim startIndex As Integer = Array.IndexOf(Of Levels)(Level.kingOfGamesLevels, startLevel)
		Dim destinationIndex As Integer = Array.IndexOf(Of Levels)(Level.kingOfGamesLevels, endLevel)
		Dim startPrefix As String = ChessCastleLevel.LevelPrefixes(startIndex)
		Dim endPrefix As String = ChessCastleLevel.LevelPrefixes(destinationIndex)
		Dim behaviour As Behaviour = Me.startEntity
		Dim flag As Boolean = False
		Me.dialogueInteractionPoint.enabled = flag
		flag = flag
		Me.exitEntity.enabled = flag
		behaviour.enabled = flag
		If intro Then
			Dim num As Single = CSng(destinationIndex) / CSng(Level.kingOfGamesLevels.Length) - 0.25F
			Me.castleAnimator.Play("FullRotation", ChessCastleLevel.CastleBaseLayer, num)
			Me.castleAnimator.Play("Off", ChessCastleLevel.CastleDoorLayer)
			Me.kingAnimator.Play("LeverPull", 0, 0.2F)
		Else
			Me.castleAnimator.Play(startPrefix + "Close", ChessCastleLevel.CastleDoorLayer)
			AudioManager.Play(ChessCastleLevel.DoorSounds(startIndex) + "_close")
			Me.kingAnimator.SetTrigger("PullLever")
			Me.kingAnimator.SetBool("Talking", False)
			Yield Me.kingAnimator.WaitForNormalizedTime(Me, 0.6101695F, "LeverPull", 0, False, False, True)
			Yield Me.castleAnimator.WaitForNormalizedTimeLooping(Me, 0.9166667F, startPrefix + "Idle", ChessCastleLevel.CastleBaseLayer, True, False, True)
			Me.castleAnimator.Play(startPrefix + "Start")
		End If
		Me.castleAnimator.SetInteger("Destination", destinationIndex)
		Me.castleAnimator.SetBool("Gauntlet", gauntlet)
		Me.castleAnimator.Play("Off", ChessCastleLevel.CastleFlairLayer)
		Me.platformAnimator.Play("Start", ChessCastleLevel.PlatformBaseLayer)
		Me.rotating = True
		CupheadLevelCamera.Current.StartShake(2F)
		If destinationIndex - startIndex <> 1 Then
			AudioManager.PlayLoop("sfx_dlc_kog_castle_kog_rotate_loop")
			AudioManager.FadeSFXVolumeLinear("sfx_dlc_kog_castle_kog_rotate_loop", 0F, 0.2F, 0.2F)
		Else
			AudioManager.Play("sfx_dlc_kog_castle_kog_rotate")
		End If
		Yield Me.castleAnimator.WaitForAnimationToStart(Me, endPrefix + "Stop", ChessCastleLevel.CastleBaseLayer, False)
		AudioManager.FadeSFXVolumeLinear("sfx_dlc_kog_castle_kog_rotate_loop", 0F, 0.2F)
		AudioManager.Play("sfx_dlc_kog_castle_kog_roateend")
		Yield Me.castleAnimator.WaitForNormalizedTime(Me, 1F, endPrefix + "Stop", ChessCastleLevel.CastleBaseLayer, True, False, True)
		Me.rotating = False
		CupheadLevelCamera.Current.EndShake(0.2F)
		Me.castleAnimator.Play(endPrefix + "Idle", ChessCastleLevel.CastleBaseLayer)
		Me.castleAnimator.Play(endPrefix + "Open", ChessCastleLevel.CastleDoorLayer)
		AudioManager.Play(ChessCastleLevel.DoorSounds(destinationIndex) + "_open")
		If endLevel = Levels.ChessPawn OrElse endLevel = Levels.ChessBishop OrElse endLevel = Levels.ChessRook OrElse endLevel = Levels.ChessQueen Then
			Me.castleAnimator.Play(endPrefix, ChessCastleLevel.CastleFlairLayer)
		End If
		Me.platformAnimator.Play("Stop", ChessCastleLevel.PlatformBaseLayer)
		Me.setTargetLevel(endLevel, gauntlet)
		Dim behaviour2 As Behaviour = Me.startEntity
		flag = True
		Me.dialogueInteractionPoint.enabled = flag
		flag = flag
		Me.exitEntity.enabled = flag
		behaviour2.enabled = flag
		Me.rotationCoroutine = Nothing
		If gauntlet Then
			Me.gauntletSparklesCoroutine = MyBase.StartCoroutine(Me.gauntletSparkles_cr())
		End If
		Return
	End Function

	' Token: 0x060001A8 RID: 424 RVA: 0x00058C98 File Offset: 0x00057098
	Public Sub StartChessLevel()
		MyBase.StartCoroutine(Me.startChessLevel_cr())
	End Sub

	' Token: 0x060001A9 RID: 425 RVA: 0x00058CA8 File Offset: 0x000570A8
	Private Iterator Function startChessLevel_cr() As IEnumerator
		Dim player As AbstractPlayerController = PlayerManager.GetPlayer(PlayerId.PlayerOne)
		Me.playerStartLevelEffects(0).gameObject.SetActive(True)
		Me.playerStartLevelEffects(0).transform.position = player.transform.position
		player.gameObject.SetActive(False)
		Me.playerStartLevelEffects(0).animator.SetTrigger("OnStartTutorial")
		player = PlayerManager.GetPlayer(PlayerId.PlayerTwo)
		If player IsNot Nothing Then
			Me.playerStartLevelEffects(1).gameObject.SetActive(True)
			Me.playerStartLevelEffects(1).transform.position = player.transform.position
			player.gameObject.SetActive(False)
			Me.playerStartLevelEffects(1).animator.SetTrigger("OnStartTutorial")
		End If
		AudioManager.Play("sfx_dlc_kog_castle_kog_entercastle")
		Yield CupheadTime.WaitForSeconds(Me, 0.2F)
		Dim context As GauntletContext = If((Not Me.currentIsGauntlet), Nothing, New GauntletContext(False))
		Dim levels As Levels = Me.currentTargetLevel
		Dim transition As SceneLoader.Transition = SceneLoader.Transition.Iris
		Dim gauntletContext As GauntletContext = context
		SceneLoader.LoadLevel(levels, transition, SceneLoader.Icon.Hourglass, gauntletContext)
		Return
	End Function

	' Token: 0x060001AA RID: 426 RVA: 0x00058CC4 File Offset: 0x000570C4
	Public Sub [Exit]()
		For Each abstractPlayerController As AbstractPlayerController In PlayerManager.GetAllPlayers()
			Dim levelPlayerController As LevelPlayerController = CType(abstractPlayerController, LevelPlayerController)
			If Not(levelPlayerController Is Nothing) Then
				levelPlayerController.DisableInput()
			End If
		Next
		SceneLoader.LoadScene(Scenes.scene_map_world_DLC, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, Nothing)
	End Sub

	' Token: 0x060001AB RID: 427 RVA: 0x00058D40 File Offset: 0x00057140
	Private Iterator Function cloudSpawn_cr() As IEnumerator
		Dim cloudSpawnTime As MinMax = New MinMax(0.8F, 1.4F)
		While True
			Dim elapsedTime As Single = 0F
			Dim duration As Single = cloudSpawnTime.RandomFloat()
			While elapsedTime < If((Not Me.rotating), duration, (duration / Me.rotationMultiplier))
				Yield Nothing
				elapsedTime += CupheadTime.Delta
			End While
			Dim obj As GameObject = Global.UnityEngine.[Object].Instantiate(Of GameObject)(Me.cloudPrefab)
			obj.GetComponent(Of ChessCastleLevelCloud)().Initialize(Me)
		End While
		Return
	End Function

	' Token: 0x060001AC RID: 428 RVA: 0x00058D5B File Offset: 0x0005715B
	Private Sub AnimateCoins(count As Integer)
		MyBase.StartCoroutine(Me.coin_cr(count))
	End Sub

	' Token: 0x060001AD RID: 429 RVA: 0x00058D6C File Offset: 0x0005716C
	Private Iterator Function coin_cr(count As Integer) As IEnumerator
		For i As Integer = 0 To count - 1
			Dim animationName As String = If((i Mod 2 <> 0), "CoinB", "CoinA")
			Me.kingAnimator.Play(animationName, 1)
			Yield Me.kingAnimator.WaitForAnimationToEnd(Me, animationName, 1, False, True)
			AudioManager.Play("sfx_coin_pickup")
			Dim coinSpark As GameObject = Global.UnityEngine.[Object].Instantiate(Of GameObject)(Me.coinPrefab, Me.coinSparkSpawnPoint.position, Quaternion.Euler(0F, 0F, Global.UnityEngine.Random.Range(0F, 360F)))
			Dim coinSparkRenderer As Renderer = coinSpark.GetComponent(Of Renderer)()
			coinSparkRenderer.sortingLayerName = "Effects"
			coinSparkRenderer.sortingOrder = 50
			coinSpark.GetComponent(Of Animator)().Play("anim_level_coin_death")
		Next
		Return
	End Function

	' Token: 0x060001AE RID: 430 RVA: 0x00058D90 File Offset: 0x00057190
	Private Iterator Function gauntletSparkles_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 0.2F)
		While True
			Yield CupheadTime.WaitForSeconds(Me, 0.35F)
			Dim position As Vector3 = Me.sparklesCenter.position + Global.UnityEngine.Random.insideUnitCircle * 70F
			Dim effect As Effect = Me.sparkleEffect.Create(position)
			effect.GetComponent(Of AnimationHelper)().Speed = 0.33333334F
			effect.GetComponent(Of SpriteRenderer)().sortingLayerName = "Enemies"
		End While
		Return
	End Function

	' Token: 0x060001AF RID: 431 RVA: 0x00058DAB File Offset: 0x000571AB
	Private Sub onDialogueStartedHandler()
		MyBase.Ending = True
		AudioManager.Play("sfx_dlc_kog_castle_kingvoice")
	End Sub

	' Token: 0x060001B0 RID: 432 RVA: 0x00058DC0 File Offset: 0x000571C0
	Private Sub onDialogueMessageHandler(message As String, metadata As String)
		If message = "GiveCoins" Then
			Dim array As String()
			If ChessCastleLevel.Coins.TryGetValue(Me.currentTargetLevel, array) Then
				Me.AnimateCoins(array.Length)
			End If
		ElseIf message = "SetupChooseLevel" Then
			Me.setupChooseLevel()
		ElseIf message = "ChooseLevel" Then
			Me.revertChooseLevel()
			If metadata = "-1" Then
				Return
			End If
			Dim num As Integer
			If Parser.IntTryParse(metadata, num) Then
				If num = 5 Then
					Me.rotate(Me.currentTargetLevel, Levels.ChessPawn, True, False)
				Else
					Dim levels As Levels = Level.kingOfGamesLevels(num)
					Me.rotate(Me.currentTargetLevel, levels, False, False)
				End If
			End If
		End If
	End Sub

	' Token: 0x060001B1 RID: 433 RVA: 0x00058E88 File Offset: 0x00057288
	Private Sub onDialogueEndedHandler()
		MyBase.Ending = False
		Me.stopTalk()
		Dim flag As Boolean = TypeOf SceneLoader.CurrentContext Is GauntletContext AndAlso CType(SceneLoader.CurrentContext, GauntletContext).complete
		If flag Then
			OnlineManager.Instance.[Interface].UnlockAchievement(PlayerId.Any, OnlineAchievementData.DLC.DefeatKOGGauntlet)
			Me.dialogueInteractionPoint.dialogueInteraction = DialoguerDialogues.KingOfGames_WDLC
		ElseIf Me.dialogueInteractionPoint.dialogueInteraction = DialoguerDialogues.KingOfGamesVictory_WDLC Then
			PlayerData.SaveCurrentFile()
			Me.dialogueInteractionPoint.dialogueInteraction = DialoguerDialogues.KingOfGames_WDLC
			If Me.attemptsToBeat < ChessCastleLevel.MaxAttemptsToContinue AndAlso Dialoguer.GetGlobalFloat(ChessCastleLevel.KingOfGamesDialoguerStateIndex) <> CSng(ChessCastleLevel.KingOfGamesFinalDialogueState) Then
				Me.rotate(Me.currentTargetLevel, Me.calculateCurrentLevel(), False, False)
			Else
				Me.[Exit]()
			End If
		ElseIf Me.firstEntry Then
			PlayerData.SaveCurrentFile()
			If Not Me.startEntity.enabled Then
				AudioManager.Play("sfx_dlc_kog_castle_door_wooddoor_open")
			End If
			Me.castleAnimator.Play(ChessCastleLevel.LevelPrefixes(0) + "Open", ChessCastleLevel.CastleDoorLayer)
			Me.startEntity.enabled = True
		End If
	End Sub

	' Token: 0x060001B2 RID: 434 RVA: 0x00058FBC File Offset: 0x000573BC
	Private Sub onDialogueAdvancedHandler(data As DialoguerTextData)
		If Not Me.kingAnimator.GetCurrentAnimatorStateInfo(0).IsName("Talk") Then
			Me.kingAnimator.SetTrigger("Talk")
		End If
	End Sub

	' Token: 0x060001B3 RID: 435 RVA: 0x00058FF7 File Offset: 0x000573F7
	Public Sub StartTalkAnimation()
		Me.kingAnimator.SetBool("Talking", True)
	End Sub

	' Token: 0x060001B4 RID: 436 RVA: 0x0005900A File Offset: 0x0005740A
	Public Sub EndTalkAnimation()
		Me.kingAnimator.SetBool("Talking", False)
	End Sub

	' Token: 0x060001B5 RID: 437 RVA: 0x0005901D File Offset: 0x0005741D
	Private Sub onIntroEventHandler()
		Me.beginIntroPan = True
	End Sub

	' Token: 0x060001B6 RID: 438 RVA: 0x00059026 File Offset: 0x00057426
	Private Sub onFadeOutStartEventHandler(time As Single)
		Me.beginIntroPan = True
	End Sub

	' Token: 0x060001B7 RID: 439 RVA: 0x0005902F File Offset: 0x0005742F
	Private Sub stopTalk()
		Me.kingAnimator.SetBool("Talking", False)
	End Sub

	' Token: 0x060001B8 RID: 440 RVA: 0x00059044 File Offset: 0x00057444
	Private Sub updateDialogueState()
		Dim num As Integer = PlayerData.Data.CountLevelsCompleted(Level.kingOfGamesLevels)
		If num = 1 Then
			Dialoguer.SetGlobalFloat(ChessCastleLevel.KingOfGamesDialoguerStateIndex, 2F)
		ElseIf num = 2 Then
			Dialoguer.SetGlobalFloat(ChessCastleLevel.KingOfGamesDialoguerStateIndex, 3F)
		ElseIf num = 3 Then
			Dialoguer.SetGlobalFloat(ChessCastleLevel.KingOfGamesDialoguerStateIndex, 4F)
		ElseIf num = 4 Then
			Dialoguer.SetGlobalFloat(ChessCastleLevel.KingOfGamesDialoguerStateIndex, 5F)
		ElseIf Dialoguer.GetGlobalFloat(ChessCastleLevel.KingOfGamesDialoguerStateIndex) = 7F Then
			Dialoguer.SetGlobalFloat(ChessCastleLevel.KingOfGamesDialoguerStateIndex, CSng(ChessCastleLevel.KingOfGamesFinalDialogueState))
		End If
	End Sub

	' Token: 0x060001B9 RID: 441 RVA: 0x000590F4 File Offset: 0x000574F4
	Private Function calculateCurrentLevel() As Levels
		For Each levels As Levels In Level.kingOfGamesLevels
			If Not PlayerData.Data.CheckLevelCompleted(levels) Then
				Return levels
			End If
		Next
		Return Level.kingOfGamesLevels.GetLast()
	End Function

	' Token: 0x060001BA RID: 442 RVA: 0x0005913C File Offset: 0x0005753C
	Private Function calculatePreviousLevel() As Levels
		Dim kingOfGamesLevels As Levels() = Level.kingOfGamesLevels
		If Not PlayerData.Data.CheckLevelCompleted(kingOfGamesLevels(0)) Then
			Return Levels.Test
		End If
		For i As Integer = 1 To Level.kingOfGamesLevels.Length - 1
			If Not PlayerData.Data.CheckLevelCompleted(kingOfGamesLevels(i)) Then
				Return kingOfGamesLevels(i - 1)
			End If
		Next
		Return Levels.Test
	End Function

	' Token: 0x060001BB RID: 443 RVA: 0x00059194 File Offset: 0x00057594
	Private Sub setTargetLevel(level As Levels, gauntlet As Boolean)
		Me.currentTargetLevel = level
		Me.currentIsGauntlet = gauntlet
	End Sub

	' Token: 0x060001BC RID: 444 RVA: 0x000591A4 File Offset: 0x000575A4
	Private Sub setupChooseLevel()
		Dim num As Integer = Array.IndexOf(Of Levels)(Level.kingOfGamesLevels, Me.currentTargetLevel)
		Me.speechBubble.HideOptionByIndex(num)
		Dim vector As Vector2 = Me.speechBubbleBasePosition
		vector.y -= 130F
		Me.speechBubble.basePosition = vector
	End Sub

	' Token: 0x060001BD RID: 445 RVA: 0x000591F4 File Offset: 0x000575F4
	Private Sub revertChooseLevel()
		Me.speechBubble.basePosition = Me.speechBubbleBasePosition
	End Sub

	' Token: 0x060001BE RID: 446 RVA: 0x00059208 File Offset: 0x00057608
	Private Sub movePlayersToDialoguePositions()
		Dim abstractPlayerController As AbstractPlayerController = PlayerManager.GetPlayer(PlayerId.PlayerOne)
		If abstractPlayerController IsNot Nothing Then
			Dim position As Vector3 = abstractPlayerController.transform.position
			position.x = Me.dialogueInteractionPoint.playerOneDialoguePosition.x
			abstractPlayerController.transform.position = position
		End If
		abstractPlayerController = PlayerManager.GetPlayer(PlayerId.PlayerTwo)
		If abstractPlayerController IsNot Nothing Then
			Dim position2 As Vector3 = abstractPlayerController.transform.position
			position2.x = Me.dialogueInteractionPoint.playerTwoDialoguePosition.x
			abstractPlayerController.transform.position = position2
		End If
	End Sub

	' Token: 0x0400030A RID: 778
	Private properties As LevelProperties.ChessCastle

	' Token: 0x0400030B RID: 779
	Private Shared MaxAttemptsToContinue As Integer = 5

	' Token: 0x0400030C RID: 780
	Private Shared KingOfGamesDialoguerStateIndex As Integer = 36

	' Token: 0x0400030D RID: 781
	Private Shared KingOfGamesVictoryDialoguerCountIndex As Integer = 37

	' Token: 0x0400030E RID: 782
	Private Shared KingOfGamesVictoryDialoguerStateIndex As Integer = 42

	' Token: 0x0400030F RID: 783
	Private Shared KingOfGamesFinalDialogueState As Integer = 6

	' Token: 0x04000310 RID: 784
	Private Shared CastleBaseLayer As Integer = 0

	' Token: 0x04000311 RID: 785
	Private Shared CastleDoorLayer As Integer = 1

	' Token: 0x04000312 RID: 786
	Private Shared CastleFlairLayer As Integer = 2

	' Token: 0x04000313 RID: 787
	Private Shared LevelPrefixes As String() = New String() { "Pawn", "Knight", "Bishop", "Rook", "Queen" }

	' Token: 0x04000314 RID: 788
	Private Shared PlatformBaseLayer As Integer = 0

	' Token: 0x04000315 RID: 789
	Public Shared Coins As Dictionary(Of Levels, String()) = New Dictionary(Of Levels, String())() From { { Levels.ChessPawn, New String() { "a37b3d37-a32e-4b88-a583-34489496494d", "25f15554-d229-4330-96cc-ac8a13c18ea0" } }, { Levels.ChessKnight, New String() { "eacf4228-e200-4839-9d79-3439cfcc5824", "47f7edb1-b5c5-4afb-9acb-a46f5e6df557" } }, { Levels.ChessBishop, New String() { "3826615a-498b-4158-af7b-0d01acbc18c8", "d52b1cc6-414c-4a7c-9f8a-250316566d58" } }, { Levels.ChessRook, New String() { "fc2c48cd-5dec-472a-ae18-dccfc94232c6", "16732bc8-7230-467a-a9ac-ff9c62ab7657" } }, { Levels.ChessQueen, New String() { "e0c6e8bc-0c56-4e52-a9a1-c53887f5ca4c", "19090606-09e8-4e56-92ac-e08200926b94", "39bfe6d8-0dbc-4886-9998-52c67b57969e" } } }

	' Token: 0x04000316 RID: 790
	Private Shared DoorSounds As String() = New String() { "sfx_dlc_kog_castle_door_wooddoor", "sfx_dlc_kog_castle_door_drawbridge", "sfx_dlc_kog_castle_door_tall", "sfx_dlc_kog_castle_door_portcullis", "sfx_dlc_kog_castle_door_queen" }

	' Token: 0x04000317 RID: 791
	<Header("Boss Info")>
	<SerializeField()>
	Private _bossPortrait As Sprite

	' Token: 0x04000318 RID: 792
	<SerializeField()>
	<Multiline()>
	Private _bossQuote As String

	' Token: 0x04000319 RID: 793
	<SerializeField()>
	Private startEntity As AbstractLevelInteractiveEntity

	' Token: 0x0400031A RID: 794
	<SerializeField()>
	Private exitEntity As AbstractLevelInteractiveEntity

	' Token: 0x0400031B RID: 795
	<SerializeField()>
	Private playerStartLevelEffects As PlayerDeathEffect()

	' Token: 0x0400031C RID: 796
	<SerializeField()>
	Private dialogueInteractionPoint As ChessCastleLevelKingInteractionPoint

	' Token: 0x0400031D RID: 797
	<SerializeField()>
	Private speechBubble As SpeechBubble

	' Token: 0x0400031E RID: 798
	<SerializeField()>
	Private castleAnimator As Animator

	' Token: 0x0400031F RID: 799
	<SerializeField()>
	Private platformAnimator As Animator

	' Token: 0x04000320 RID: 800
	<SerializeField()>
	Private cloudPrefab As GameObject

	' Token: 0x04000321 RID: 801
	<SerializeField()>
	Private coinPrefab As GameObject

	' Token: 0x04000322 RID: 802
	<SerializeField()>
	Private coinSparkSpawnPoint As Transform

	' Token: 0x04000323 RID: 803
	<SerializeField()>
	Private kingAnimator As Animator

	' Token: 0x04000324 RID: 804
	<SerializeField()>
	Private sparkleEffect As Effect

	' Token: 0x04000325 RID: 805
	<SerializeField()>
	Private sparklesCenter As Transform

	' Token: 0x04000326 RID: 806
	<SerializeField()>
	Private sinePeriod As Single

	' Token: 0x04000327 RID: 807
	<SerializeField()>
	Private sineAmplitude As Single

	' Token: 0x04000328 RID: 808
	<SerializeField()>
	Private _rotationMultiplier As Single

	' Token: 0x04000329 RID: 809
	<SerializeField()>
	Private introPanAmount As Single

	' Token: 0x0400032A RID: 810
	<SerializeField()>
	Private introPanDuration As Single

	' Token: 0x0400032B RID: 811
	Private firstEntry As Boolean

	' Token: 0x0400032C RID: 812
	Private previousLevel As Levels

	' Token: 0x0400032D RID: 813
	Private previouslyWon As Boolean

	' Token: 0x0400032E RID: 814
	Private attemptsToBeat As Integer

	' Token: 0x0400032F RID: 815
	Private currentTargetLevel As Levels

	' Token: 0x04000330 RID: 816
	Private currentIsGauntlet As Boolean

	' Token: 0x04000331 RID: 817
	Private rotationCoroutine As Coroutine

	' Token: 0x04000332 RID: 818
	Private gauntletSparklesCoroutine As Coroutine

	' Token: 0x04000333 RID: 819
	Private speechBubbleBasePosition As Vector2

	' Token: 0x04000334 RID: 820
	Private cameraSineAccumulator As Single

	' Token: 0x04000335 RID: 821
	Private introCameraMovementCoroutine As Coroutine

	' Token: 0x04000336 RID: 822
	Private beginIntroPan As Boolean
End Class
