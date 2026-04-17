Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x0200020D RID: 525
Public Class MausoleumLevel
	Inherits Level

	' Token: 0x060005DF RID: 1503 RVA: 0x0006A884 File Offset: 0x00068C84
	Protected Overrides Sub PartialInit()
		Me.properties = LevelProperties.Mausoleum.GetMode(MyBase.mode)
		AddHandler Me.properties.OnStateChange, AddressOf MyBase.zHack_OnStateChanged
		AddHandler Me.properties.OnBossDeath, AddressOf MyBase.zHack_OnWin
		MyBase.timeline = Me.properties.CreateTimeline(MyBase.mode)
		Me.goalTimes = Me.properties.goalTimes
		AddHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		MyBase.PartialInit()
	End Sub

	' Token: 0x17000107 RID: 263
	' (get) Token: 0x060005E0 RID: 1504 RVA: 0x0006A91A File Offset: 0x00068D1A
	Public Overrides ReadOnly Property CurrentLevel As Levels
		Get
			Return Levels.Mausoleum
		End Get
	End Property

	' Token: 0x17000108 RID: 264
	' (get) Token: 0x060005E1 RID: 1505 RVA: 0x0006A921 File Offset: 0x00068D21
	Public Overrides ReadOnly Property CurrentScene As Scenes
		Get
			Return Scenes.scene_level_mausoleum
		End Get
	End Property

	' Token: 0x17000109 RID: 265
	' (get) Token: 0x060005E2 RID: 1506 RVA: 0x0006A928 File Offset: 0x00068D28
	Public Overrides ReadOnly Property BossPortrait As Sprite
		Get
			Select Case MyBase.mode
				Case Level.Mode.Easy
					Return Me._bossPortraitEasy
				Case Level.Mode.Normal
					Return Me._bossPortraitNormal
				Case Level.Mode.Hard
					Return Me._bossPortraitHard
				Case Else
					Global.Debug.LogError("Couldn't find portrait for state " + MyBase.mode + ". Using Main.", Nothing)
					Return Me._bossPortraitEasy
			End Select
		End Get
	End Property

	' Token: 0x1700010A RID: 266
	' (get) Token: 0x060005E3 RID: 1507 RVA: 0x0006A990 File Offset: 0x00068D90
	Public Overrides ReadOnly Property BossQuote As String
		Get
			Select Case MyBase.mode
				Case Level.Mode.Easy
					Return Me._bossQuoteEasy
				Case Level.Mode.Normal
					Return Me._bossQuoteNormal
				Case Level.Mode.Hard
					Return Me._bossQuoteHard
				Case Else
					Global.Debug.LogError("Couldn't find quote for state " + MyBase.mode + ". Using Main.", Nothing)
					Return Me._bossQuoteEasy
			End Select
		End Get
	End Property

	' Token: 0x060005E4 RID: 1508 RVA: 0x0006A9F8 File Offset: 0x00068DF8
	Protected Overrides Sub Awake()
		Level.OverrideDifficulty = True
		Me.LoseGame = CType([Delegate].Combine(Me.LoseGame, AddressOf Me.Failure), Action)
		Dim currentMap As Scenes = PlayerData.Data.CurrentMap
		If currentMap <> Scenes.scene_map_world_1 Then
			If currentMap <> Scenes.scene_map_world_2 Then
				If currentMap = Scenes.scene_map_world_3 Then
					MyBase.mode = Level.Mode.Hard
				End If
			Else
				MyBase.mode = Level.Mode.Normal
			End If
		Else
			MyBase.mode = Level.Mode.Easy
		End If
		Me.originalMode = Level.CurrentMode
		Level.SetCurrentMode(MyBase.mode)
		For Each gameObject As GameObject In Me.WorldBackgrounds
			gameObject.SetActive(False)
		Next
		Me.WorldBackgrounds(CInt(MyBase.mode)).SetActive(True)
		Me.currentUrnAnimator = Me.urnsAnimator(CInt(MyBase.mode))
		Me.currentChaliceAnimator = Me.chaliceCharacterAnimators(CInt(MyBase.mode))
		If(PlayerData.Data.IsUnlocked(PlayerId.Any, Super.level_super_beam) AndAlso MyBase.mode = Level.Mode.Easy) OrElse (PlayerData.Data.IsUnlocked(PlayerId.Any, Super.level_super_invincible) AndAlso MyBase.mode = Level.Mode.Normal) OrElse (PlayerData.Data.IsUnlocked(PlayerId.Any, Super.level_super_ghost) AndAlso MyBase.mode = Level.Mode.Hard) Then
			Me.noChalice = True
			Me.helpSignAnimator.gameObject.SetActive(False)
			Me.currentUrnAnimator.SetTrigger("NoGlow")
		End If
		MyBase.Awake()
	End Sub

	' Token: 0x060005E5 RID: 1509 RVA: 0x0006AB88 File Offset: 0x00068F88
	Protected Overrides Sub Start()
		Me.isMausoleum = True
		MyBase.Start()
		AddHandler Dialoguer.events.onMessageEvent, AddressOf Me.OnMessageEvent
		Me.dialogue.chaliceAnimator = Me.currentChaliceAnimator
		Dim currentMap As Scenes = PlayerData.Data.CurrentMap
		If currentMap <> Scenes.scene_map_world_1 Then
			If currentMap <> Scenes.scene_map_world_2 Then
				If currentMap = Scenes.scene_map_world_3 Then
					Me.super = Super.level_super_ghost
				End If
			Else
				Me.super = Super.level_super_invincible
			End If
		Else
			Me.super = Super.level_super_beam
		End If
	End Sub

	' Token: 0x060005E6 RID: 1510 RVA: 0x0006AC1E File Offset: 0x0006901E
	Protected Overrides Sub OnLevelStart()
		MyBase.StartCoroutine(Me.main_pattern_cr())
		If Me.noChalice Then
			Return
		End If
		MyBase.StartCoroutine(Me.helpsignDisappear_cr())
		MyBase.StartCoroutine(Me.urnrandomanimation_cr())
	End Sub

	' Token: 0x060005E7 RID: 1511 RVA: 0x0006AC53 File Offset: 0x00069053
	Protected Overrides Sub OnStateChanged()
		MyBase.OnStateChanged()
	End Sub

	' Token: 0x060005E8 RID: 1512 RVA: 0x0006AC5B File Offset: 0x0006905B
	Public Sub OnMessageEvent(message As String, metaData As String)
		If message = "PowerUpGiven" Then
			Me.currentChaliceAnimator.Play("Chalice_Magic_Burst")
			MyBase.StartCoroutine(Me.chalice_animation_cr())
			MyBase.StartCoroutine(Me.play_chalice_sound_cr())
		End If
	End Sub

	' Token: 0x060005E9 RID: 1513 RVA: 0x0006AC98 File Offset: 0x00069098
	Private Iterator Function chalice_animation_cr() As IEnumerator
		Yield Me.currentChaliceAnimator.WaitForAnimationToEnd(Me, "Chalice_Magic_Burst_mid", False, True)
		Me.PlaySuperPowerup()
		Yield Nothing
		Return
	End Function

	' Token: 0x060005EA RID: 1514 RVA: 0x0006ACB4 File Offset: 0x000690B4
	Private Sub PlaySuperPowerup()
		For Each abstractPlayerController As AbstractPlayerController In Me.players
			If Not(abstractPlayerController Is Nothing) Then
				If Not Me.PowerUpSFXActive Then
					Me.PowerUpSFXActive = True
				End If
				Dim num As Single = If((abstractPlayerController.transform.position.y >= -195F), 368F, 146F)
				Me.chaliceBeamEffect.Create(New Vector3(abstractPlayerController.transform.position.x - 10F, num))
				abstractPlayerController.animator.Play("Super_Power_Up")
			End If
		Next
	End Sub

	' Token: 0x060005EB RID: 1515 RVA: 0x0006AD70 File Offset: 0x00069170
	Private Iterator Function play_chalice_sound_cr() As IEnumerator
		Yield Me.currentChaliceAnimator.WaitForAnimationToEnd(Me, "Chalice_Magic_Burst", False, True)
		AudioManager.Play("player_power_up")
		Me.emitAudioFromObject.Add("player_power_up")
		Yield Nothing
		Return
	End Function

	' Token: 0x060005EC RID: 1516 RVA: 0x0006AD8C File Offset: 0x0006918C
	Private Iterator Function mausoleumPattern_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		While True
			Yield MyBase.StartCoroutine(Me.nextPattern_cr())
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060005ED RID: 1517 RVA: 0x0006ADA8 File Offset: 0x000691A8
	Private Iterator Function nextPattern_cr() As IEnumerator
		Dim p As LevelProperties.Mausoleum.Pattern = Me.properties.CurrentState.NextPattern
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		Return
	End Function

	' Token: 0x060005EE RID: 1518 RVA: 0x0006ADC4 File Offset: 0x000691C4
	Private Iterator Function helpsignDisappear_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1.5F)
		Me.helpSignAnimator.SetTrigger("Outro")
		Return
	End Function

	' Token: 0x060005EF RID: 1519 RVA: 0x0006ADE0 File Offset: 0x000691E0
	Private Iterator Function urnrandomanimation_cr() As IEnumerator
		While Not Me.isLevelOver
			Yield CupheadTime.WaitForSeconds(Me, 2F)
			AudioManager.Play("mausoleum_ghost_jar_shake")
			Me.emitAudioFromObject.Add("mausoleum_ghost_jar_shake")
			Dim rand As Integer = Global.UnityEngine.Random.Range(1, 4)
			If rand = 1 Then
				Me.currentUrnAnimator.SetTrigger("Shake")
			ElseIf rand = 2 Then
				Me.currentUrnAnimator.SetTrigger("SmallVibrate")
			ElseIf rand = 3 Then
				Me.currentUrnAnimator.SetTrigger("BigVibrate")
			End If
		End While
		Return
	End Function

	' Token: 0x060005F0 RID: 1520 RVA: 0x0006ADFC File Offset: 0x000691FC
	Private Iterator Function legendarychaliceappear_cr() As IEnumerator
		AudioManager.StopBGM()
		AudioManager.PlayBGMPlaylistManually(False)
		Me.currentUrnAnimator.SetTrigger("Sparkle")
		Yield CupheadTime.WaitForSeconds(Me, 2.5F)
		AudioManager.Play("mausoleum_lid_pop")
		Me.currentUrnAnimator.SetTrigger("Pop")
		Yield CupheadTime.WaitForSeconds(Me, 2F)
		Dim t As Single = 0F
		Dim TIME As Single = 1.5F
		Dim arcHeight As Single = 150F
		Dim arcHeightAdd As Single = 0F
		Dim startPosition As Vector3 = Me.currentChaliceAnimator.gameObject.transform.localPosition
		Dim endPosition As Vector3 = Me.currentChaliceAnimator.gameObject.transform.localPosition + New Vector3(400F, 0F, 0F)
		AudioManager.Play("mausoleum_ghost_jar_travel")
		Me.emitAudioFromObject.Add("mausoleum_ghost_jar_travel")
		While t < TIME
			Dim val As Single = t / TIME
			Dim newPosition As Vector3 = Vector3.Lerp(startPosition, endPosition, EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0F, 1F, val))
			If t < TIME / 2F Then
				arcHeightAdd = Mathf.Lerp(arcHeight, 0F, EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0F, 1F, 1F - val * 2F))
			Else
				arcHeightAdd = Mathf.Lerp(arcHeight, 0F, EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0F, 1F, val * 2F - 1F))
			End If
			newPosition.y += arcHeightAdd
			Me.currentChaliceAnimator.gameObject.transform.localPosition = newPosition
			t += Time.deltaTime
			Yield Nothing
		End While
		AudioManager.Play("mausoleum_ghost_jar_queen_ghost_appear")
		Me.emitAudioFromObject.Add("mausoleum_ghost_jar_queen_ghost_appear")
		Me.currentChaliceAnimator.SetTrigger("Transition")
		Me.dialogue.BeginDialogue()
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		Dim currentDialogueFloat As Single = Dialoguer.GetGlobalFloat(Me.dialoguerVariableID)
		Dialoguer.SetGlobalFloat(14, currentDialogueFloat + 1F)
		PlayerData.SaveCurrentFile()
		Return
	End Function

	' Token: 0x060005F1 RID: 1521 RVA: 0x0006AE18 File Offset: 0x00069218
	Private Sub SetupTimeline()
		MyBase.timeline = New Level.Timeline()
		MyBase.timeline.health = 0F
		Dim list As List(Of Single) = New List(Of Single)()
		Dim num As Integer = 3
		For i As Integer = 0 To num - 1
			MyBase.timeline.health += CSng(Me.properties.CurrentState.main.ghostCount)
			list.Add(CSng(Me.properties.CurrentState.main.ghostCount))
		Next
		For j As Integer = 0 To num - 1 - 1
			MyBase.timeline.AddEventAtHealth(j.ToStringInvariant(), MyBase.timeline.GetHealthOfLastEvent() + CInt(list(j)))
		Next
	End Sub

	' Token: 0x060005F2 RID: 1522 RVA: 0x0006AED8 File Offset: 0x000692D8
	Private Iterator Function main_pattern_cr() As IEnumerator
		Me.SetupTimeline()
		Yield Nothing
		While True
			Dim p As LevelProperties.Mausoleum.Main = Me.properties.CurrentState.main
			Dim delayMainIndex As Integer = Global.UnityEngine.Random.Range(0, p.delayString.Length)
			Dim delayString As String() = p.delayString(delayMainIndex).Split(New Char() { ","c })
			Dim delayIndex As Integer = Global.UnityEngine.Random.Range(0, delayString.Length)
			Dim spawnMainIndex As Integer = Global.UnityEngine.Random.Range(0, p.spawnString.Length)
			Dim spawnString As String() = p.spawnString(spawnMainIndex).Split(New Char() { ","c })
			Dim spawnIndex As Integer = Global.UnityEngine.Random.Range(0, spawnString.Length)
			Dim ghostTypeIndex As Integer = Global.UnityEngine.Random.Range(0, p.ghostTypeString.Length)
			Dim ghostString As String() = p.ghostTypeString(ghostTypeIndex).Split(New Char() { ","c })
			Dim ghostIndex As Integer = Global.UnityEngine.Random.Range(0, ghostString.Length)
			Dim delay As Single = 0F
			Dim spawnPos As Integer = 0
			Me.maxCounter = p.ghostCount
			MausoleumLevel.SPAWNCOUNTER = 0
			While MausoleumLevel.SPAWNCOUNTER < Me.maxCounter
				delayString = p.delayString(delayMainIndex).Split(New Char() { ","c })
				ghostString = p.ghostTypeString(ghostTypeIndex).Split(New Char() { ","c })
				Dim ghostSplit As String() = ghostString(ghostIndex).Split(New Char() { "-"c })
				Dim extraDelay As Single = 0F
				Dim splitcount As Integer = 0
				For Each split As String In ghostSplit
					spawnString = p.spawnString(spawnMainIndex).Split(New Char() { ","c })
					If Parser.IntParse(spawnString(spawnIndex)) >= 6 Then
						spawnPos = Parser.IntParse(spawnString(spawnIndex)) - 2
					Else
						spawnPos = Parser.IntParse(spawnString(spawnIndex)) - 1
					End If
					Dim direction As Vector3 = Me.urn.transform.position - Me.positions(spawnPos).transform.position
					Dim angle As Single = MathUtils.DirectionToAngle(direction)
					Dim repeatCount As Integer = 0
					Parser.IntTryParse(ghostString(ghostIndex).Substring(1), repeatCount)
					If Parser.IntParse(spawnString(spawnIndex)) = 2 OrElse Parser.IntParse(spawnString(spawnIndex)) = 8 Then
						Dim mausoleumLevelDelayGhost As MausoleumLevelDelayGhost = Me.delayGhost.Create(Me.positions(spawnPos).transform.position, angle, 0F, Me.properties.CurrentState.delayGhost)
						mausoleumLevelDelayGhost.GetParent(Me)
					Else
						Dim c2 As Char = ghostString(ghostIndex)(0)
						Select Case c2
							Case "B"c
								If repeatCount <> 0 Then
									For i As Integer = 0 To repeatCount - 1
										Dim b As MausoleumLevelBigGhost = Me.bigGhost.Create(Me.positions(spawnPos).transform.position, angle, Me.properties.CurrentState.bigGhost.speed, Me.properties.CurrentState.bigGhost, Me.urn.gameObject)
										b.Counts = splitcount = 0
										b.GetParent(Me)
										Yield CupheadTime.WaitForSeconds(Me, Me.properties.CurrentState.bigGhost.multiDelay)
									Next
									extraDelay += Me.properties.CurrentState.bigGhost.mainAddDelay
								Else
									Dim mausoleumLevelBigGhost As MausoleumLevelBigGhost = Me.bigGhost.Create(Me.positions(spawnPos).transform.position, angle, Me.properties.CurrentState.bigGhost.speed, Me.properties.CurrentState.bigGhost, Me.urn.gameObject)
									mausoleumLevelBigGhost.Counts = splitcount = 0
									mausoleumLevelBigGhost.GetParent(Me)
									extraDelay += Me.properties.CurrentState.bigGhost.mainAddDelay
								End If
							Case "C"c
								Dim c As MausoleumLevelCircleGhost = TryCast(Me.circleGhost.Create(Me.positions(spawnPos).transform.position, Me.urn.transform.position, angle, Me.properties.CurrentState.circleGhost.circleSpeed, Me.properties.CurrentState.circleGhost.circleRate), MausoleumLevelCircleGhost)
								c.Counts = splitcount = 0
								c.GetParent(Me)
							Case "D"c
								Dim d As MausoleumLevelDelayGhost = Me.delayGhost.Create(Me.positions(spawnPos).transform.position, angle, 0F, Me.properties.CurrentState.delayGhost)
								d.Counts = splitcount = 0
								d.GetParent(Me)
							Case Else
								If c2 <> "R"c Then
									If c2 = "S"c Then
										If repeatCount <> 0 Then
											For j As Integer = 0 To repeatCount - 1
												Dim s As MausoleumLevelSineGhost = Me.sineGhost.Create(Me.positions(spawnPos).transform.position, angle, Me.properties.CurrentState.sineGhost.ghostSpeed, Me.properties.CurrentState.sineGhost)
												s.Counts = splitcount = 0
												s.GetParent(Me)
												Yield CupheadTime.WaitForSeconds(Me, Me.properties.CurrentState.sineGhost.multiDelay)
											Next
											extraDelay = Me.properties.CurrentState.sineGhost.mainAddDelay
										Else
											Dim mausoleumLevelSineGhost As MausoleumLevelSineGhost = Me.sineGhost.Create(Me.positions(spawnPos).transform.position, angle, Me.properties.CurrentState.sineGhost.ghostSpeed, Me.properties.CurrentState.sineGhost)
											mausoleumLevelSineGhost.Counts = splitcount = 0
											mausoleumLevelSineGhost.GetParent(Me)
											extraDelay = Me.properties.CurrentState.sineGhost.mainAddDelay
										End If
									End If
								ElseIf repeatCount <> 0 Then
									For k As Integer = 0 To repeatCount - 1
										Dim g As MausoleumLevelRegularGhost = TryCast(Me.regularGhost.Create(Me.positions(spawnPos).transform.position, angle, Me.properties.CurrentState.regularGhost.speed), MausoleumLevelRegularGhost)
										g.Counts = splitcount = 0
										g.GetParent(Me)
										Yield CupheadTime.WaitForSeconds(Me, Me.properties.CurrentState.regularGhost.multiDelay)
									Next
									extraDelay += Me.properties.CurrentState.regularGhost.mainAddDelay
								Else
									Dim mausoleumLevelRegularGhost As MausoleumLevelRegularGhost = TryCast(Me.regularGhost.Create(Me.positions(spawnPos).transform.position, angle, Me.properties.CurrentState.regularGhost.speed), MausoleumLevelRegularGhost)
									mausoleumLevelRegularGhost.Counts = splitcount = 0
									mausoleumLevelRegularGhost.GetParent(Me)
									extraDelay += Me.properties.CurrentState.regularGhost.mainAddDelay
								End If
						End Select
					End If
					splitcount += 1
					If spawnIndex < spawnString.Length - 1 Then
						spawnIndex += 1
					Else
						spawnMainIndex = (spawnMainIndex + 1) Mod p.spawnString.Length
						spawnIndex = 0
					End If
				Next
				MyBase.timeline.DealDamage(1F)
				Yield Nothing
				delay = Parser.FloatParse(delayString(delayIndex)) + extraDelay
				Yield CupheadTime.WaitForSeconds(Me, delay)
				extraDelay = 0F
				If delayIndex < delayString.Length - 1 Then
					delayIndex += 1
				Else
					delayMainIndex = (delayMainIndex + 1) Mod p.delayString.Length
					delayIndex = 0
				End If
				If ghostIndex < ghostString.Length - 1 Then
					ghostIndex += 1
				Else
					ghostTypeIndex = (ghostTypeIndex + 1) Mod p.ghostTypeString.Length
					ghostIndex = 0
				End If
				Yield Nothing
			End While
			Dim ghosts As MausoleumLevelGhostBase() = TryCast(Global.UnityEngine.[Object].FindObjectsOfType(GetType(MausoleumLevelGhostBase)), MausoleumLevelGhostBase())
			Dim ghostsAlive As Boolean = True
			Dim ghostCounter As Integer = 0
			While ghostsAlive
				ghosts = TryCast(Global.UnityEngine.[Object].FindObjectsOfType(GetType(MausoleumLevelGhostBase)), MausoleumLevelGhostBase())
				For m As Integer = 0 To ghosts.Length - 1
					If ghosts(m).isDead Then
						ghostCounter += 1
						If ghostCounter >= ghosts.Length Then
							ghostsAlive = False
							Exit For
						End If
					End If
				Next
				ghostCounter = 0
				If Not ghostsAlive Then
					Exit While
				End If
				Yield CupheadTime.WaitForSeconds(Me, 0.25F)
				Yield Nothing
			End While
			Me.properties.DealDamageToNextNamedState()
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060005F3 RID: 1523 RVA: 0x0006AEF4 File Offset: 0x000692F4
	Private Sub Failure()
		MyBase._OnLose()
		AudioManager.Play("mausoleum_ghost_jar_burst")
		Me.emitAudioFromObject.Add("mausoleum_ghost_jar_burst")
		PlayerManager.GetPlayer(PlayerId.PlayerOne).GetComponent(Of LevelPlayerAnimationController)().ScaredSprite(Me.FacingLeft(PlayerManager.GetPlayer(PlayerId.PlayerOne)))
		If PlayerManager.GetPlayer(PlayerId.PlayerTwo) IsNot Nothing Then
			PlayerManager.GetPlayer(PlayerId.PlayerTwo).GetComponent(Of LevelPlayerAnimationController)().ScaredSprite(Me.FacingLeft(PlayerManager.GetPlayer(PlayerId.PlayerTwo)))
		End If
		MyBase.timeline.OnPlayerDeath(PlayerId.PlayerOne)
		If PlayerManager.GetPlayer(PlayerId.PlayerTwo) IsNot Nothing Then
			MyBase.timeline.OnPlayerDeath(PlayerId.PlayerTwo)
		End If
	End Sub

	' Token: 0x060005F4 RID: 1524 RVA: 0x0006AF94 File Offset: 0x00069394
	Private Function FacingLeft(player As AbstractPlayerController) As Boolean
		If player.transform.position.x > Me.urn.transform.position.x Then
			Return player.transform.localScale.x = 1F
		End If
		Return player.transform.localScale.x = -1F
	End Function

	' Token: 0x060005F5 RID: 1525 RVA: 0x0006B014 File Offset: 0x00069414
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Level.SetCurrentMode(Me.originalMode)
		MyBase.StopCoroutine(Me.urnrandomanimation_cr())
		RemoveHandler Dialoguer.events.onMessageEvent, AddressOf Me.OnMessageEvent
		Me.circleGhost = Nothing
		Me.regularGhost = Nothing
		Me.bigGhost = Nothing
		Me.delayGhost = Nothing
		Me.sineGhost = Nothing
		Me._bossPortraitEasy = Nothing
		Me._bossPortraitHard = Nothing
		Me._bossPortraitNormal = Nothing
	End Sub

	' Token: 0x060005F6 RID: 1526 RVA: 0x0006B08C File Offset: 0x0006948C
	Protected Overrides Sub OnPreWin()
		Me.isLevelOver = True
		MyBase.StopCoroutine(Me.urnrandomanimation_cr())
		If Me.noChalice Then
			MyBase.StartCoroutine(Me.win_no_chalice())
			Return
		End If
		MyBase.StartCoroutine(Me.legendarychaliceappear_cr())
	End Sub

	' Token: 0x060005F7 RID: 1527 RVA: 0x0006B0C8 File Offset: 0x000694C8
	Private Iterator Function win_no_chalice() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 0.5F)
		SceneLoader.LoadLastMap()
		Return
	End Function

	' Token: 0x060005F8 RID: 1528 RVA: 0x0006B0E4 File Offset: 0x000694E4
	Protected Overrides Sub OnWin()
		MyBase.OnWin()
		If Not PlayerData.Data.IsUnlocked(PlayerId.PlayerOne, Me.super) Then
			PlayerData.Data.Buy(PlayerId.PlayerOne, Me.super)
			PlayerData.Data.Buy(PlayerId.PlayerTwo, Me.super)
			Level.SuperUnlocked = True
		End If
		If PlayerData.Data.NumSupers(PlayerId.PlayerOne) >= 3 Then
			OnlineManager.Instance.[Interface].UnlockAchievement(PlayerId.Any, "UnlockedAllSupers")
		End If
	End Sub

	' Token: 0x04000AB9 RID: 2745
	Private properties As LevelProperties.Mausoleum

	' Token: 0x04000ABA RID: 2746
	Public Shared SPAWNCOUNTER As Integer

	' Token: 0x04000ABB RID: 2747
	<SerializeField()>
	Private WorldBackgrounds As GameObject()

	' Token: 0x04000ABC RID: 2748
	<SerializeField()>
	Private circleGhost As MausoleumLevelCircleGhost

	' Token: 0x04000ABD RID: 2749
	<SerializeField()>
	Private regularGhost As MausoleumLevelRegularGhost

	' Token: 0x04000ABE RID: 2750
	<SerializeField()>
	Private bigGhost As MausoleumLevelBigGhost

	' Token: 0x04000ABF RID: 2751
	<SerializeField()>
	Private delayGhost As MausoleumLevelDelayGhost

	' Token: 0x04000AC0 RID: 2752
	<SerializeField()>
	Private sineGhost As MausoleumLevelSineGhost

	' Token: 0x04000AC1 RID: 2753
	<SerializeField()>
	Private positions As Transform()

	' Token: 0x04000AC2 RID: 2754
	<SerializeField()>
	Private urn As MausoleumLevelUrn

	' Token: 0x04000AC3 RID: 2755
	<Header("Boss Info")>
	<SerializeField()>
	Private _bossPortraitEasy As Sprite

	' Token: 0x04000AC4 RID: 2756
	<SerializeField()>
	Private _bossPortraitNormal As Sprite

	' Token: 0x04000AC5 RID: 2757
	<SerializeField()>
	Private _bossPortraitHard As Sprite

	' Token: 0x04000AC6 RID: 2758
	<SerializeField()>
	Private _bossQuoteEasy As String

	' Token: 0x04000AC7 RID: 2759
	<SerializeField()>
	Private _bossQuoteNormal As String

	' Token: 0x04000AC8 RID: 2760
	<SerializeField()>
	Private _bossQuoteHard As String

	' Token: 0x04000AC9 RID: 2761
	<SerializeField()>
	Private helpSignAnimator As Animator

	' Token: 0x04000ACA RID: 2762
	<SerializeField()>
	Private urnsAnimator As Animator()

	' Token: 0x04000ACB RID: 2763
	<SerializeField()>
	Private chaliceCharacterAnimators As Animator()

	' Token: 0x04000ACC RID: 2764
	Private currentUrnAnimator As Animator

	' Token: 0x04000ACD RID: 2765
	Private currentChaliceAnimator As Animator

	' Token: 0x04000ACE RID: 2766
	<SerializeField()>
	Private chaliceBeamEffect As Effect

	' Token: 0x04000ACF RID: 2767
	<SerializeField()>
	Private dialogue As MausoleumDialogueInteraction

	' Token: 0x04000AD0 RID: 2768
	<SerializeField()>
	Private dialoguerVariableID As Integer = 14

	' Token: 0x04000AD1 RID: 2769
	Private isLevelOver As Boolean

	' Token: 0x04000AD2 RID: 2770
	Private PowerUpSFXActive As Boolean

	' Token: 0x04000AD3 RID: 2771
	Private super As Super = Super.level_super_beam

	' Token: 0x04000AD4 RID: 2772
	Private maxCounter As Integer

	' Token: 0x04000AD5 RID: 2773
	Private noChalice As Boolean

	' Token: 0x04000AD6 RID: 2774
	Public WinGame As Action

	' Token: 0x04000AD7 RID: 2775
	Public LoseGame As Action

	' Token: 0x04000AD8 RID: 2776
	Private originalMode As Level.Mode
End Class
