Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine
Imports UnityEngine.UI

' Token: 0x0200022E RID: 558
Public Class OldManLevel
	Inherits Level

	' Token: 0x06000638 RID: 1592 RVA: 0x0006D76C File Offset: 0x0006BB6C
	Protected Overrides Sub PartialInit()
		Me.properties = LevelProperties.OldMan.GetMode(MyBase.mode)
		AddHandler Me.properties.OnStateChange, AddressOf MyBase.zHack_OnStateChanged
		AddHandler Me.properties.OnBossDeath, AddressOf MyBase.zHack_OnWin
		MyBase.timeline = Me.properties.CreateTimeline(MyBase.mode)
		Me.goalTimes = Me.properties.goalTimes
		AddHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		MyBase.PartialInit()
	End Sub

	' Token: 0x17000111 RID: 273
	' (get) Token: 0x06000639 RID: 1593 RVA: 0x0006D802 File Offset: 0x0006BC02
	Public Overrides ReadOnly Property CurrentLevel As Levels
		Get
			Return Levels.OldMan
		End Get
	End Property

	' Token: 0x17000112 RID: 274
	' (get) Token: 0x0600063A RID: 1594 RVA: 0x0006D809 File Offset: 0x0006BC09
	Public Overrides ReadOnly Property CurrentScene As Scenes
		Get
			Return Scenes.scene_level_old_man
		End Get
	End Property

	' Token: 0x17000113 RID: 275
	' (get) Token: 0x0600063B RID: 1595 RVA: 0x0006D810 File Offset: 0x0006BC10
	Public Overrides ReadOnly Property BossPortrait As Sprite
		Get
			Select Case Me.properties.CurrentState.stateName
				Case LevelProperties.OldMan.States.Main
					Return Me._bossPortraitMain
				Case LevelProperties.OldMan.States.SockPuppet
					Return Me._bossPortraitPhaseTwo
				Case LevelProperties.OldMan.States.GnomeLeader
					Return Me._bossPortraitPhaseThree
			End Select
			Global.Debug.LogError("Couldn't find portrait for state " + Me.properties.CurrentState.stateName + ". Using Main.", Nothing)
			Return Me._bossPortraitMain
		End Get
	End Property

	' Token: 0x17000114 RID: 276
	' (get) Token: 0x0600063C RID: 1596 RVA: 0x0006D890 File Offset: 0x0006BC90
	Public Overrides ReadOnly Property BossQuote As String
		Get
			Select Case Me.properties.CurrentState.stateName
				Case LevelProperties.OldMan.States.Main
					Return Me._bossQuoteMain
				Case LevelProperties.OldMan.States.SockPuppet
					Return Me._bossQuotePhaseTwo
				Case LevelProperties.OldMan.States.GnomeLeader
					Return Me._bossQuotePhaseThree
			End Select
			Global.Debug.LogError("Couldn't find quote for state " + Me.properties.CurrentState.stateName + ". Using Main.", Nothing)
			Return Me._bossQuoteMain
		End Get
	End Property

	' Token: 0x0600063D RID: 1597 RVA: 0x0006D90E File Offset: 0x0006BD0E
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me._bossPortraitMain = Nothing
		Me._bossPortraitPhaseTwo = Nothing
		Me._bossPortraitPhaseThree = Nothing
		Me.WORKAROUND_NullifyFields()
	End Sub

	' Token: 0x0600063E RID: 1598 RVA: 0x0006D934 File Offset: 0x0006BD34
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.firstAttack = True
		Me.platformManager.LevelInit(Me.properties)
		Me.oldMan.LevelInit(Me.properties)
		Me.sockPuppet.LevelInit(Me.properties)
		Me.gnomeLeader.LevelInit(Me.properties)
		Me.climberPosString = New PatternString(Me.properties.CurrentState.climberGnomes.gnomePositionStrings, True, True)
		For i As Integer = 0 To Me.spikes.Length - 1
			Me.spikes(i).SetProperties(Me.properties)
			Me.spikes(i).SetID(i)
		Next
		Me.gnomeLeader.gameObject.SetActive(False)
		AudioManager.FadeSFXVolume("sfx_dlc_omm_p3_stomachacid_amb_loop", 0F, 0F)
	End Sub

	' Token: 0x0600063F RID: 1599 RVA: 0x0006DA14 File Offset: 0x0006BE14
	Protected Overrides Sub OnStateChanged()
		MyBase.OnStateChanged()
		If Me.properties.CurrentState.stateName = LevelProperties.OldMan.States.SockPuppet Then
			Me.StopAllCoroutines()
			MyBase.StartCoroutine(Me.phase_2_trans_cr())
		ElseIf Me.properties.CurrentState.stateName = LevelProperties.OldMan.States.GnomeLeader Then
			Me.StopAllCoroutines()
			MyBase.StartCoroutine(Me.phase_3_trans_cr())
		End If
	End Sub

	' Token: 0x06000640 RID: 1600 RVA: 0x0006DA7E File Offset: 0x0006BE7E
	Protected Overrides Sub OnLevelStart()
		MyBase.StartCoroutine(Me.oldmanPattern_cr())
		MyBase.StartCoroutine(Me.gnome_turrets_cr())
		MyBase.StartCoroutine(Me.climbers_cr())
	End Sub

	' Token: 0x06000641 RID: 1601 RVA: 0x0006DAA7 File Offset: 0x0006BEA7
	Protected Overrides Sub OnPreWin()
		If Level.Current.mode = Level.Mode.Easy Then
			Me.sockPuppet.OnPhase3()
		End If
	End Sub

	' Token: 0x06000642 RID: 1602 RVA: 0x0006DAC4 File Offset: 0x0006BEC4
	Public Sub CreateFX(pos As Vector3, isSparkle As Boolean, isPink As Boolean)
		Dim effect As Effect = Nothing
		Dim list As List(Of Effect) = If((Not isSparkle), Me.smokeFXPool, Me.sparkleFXPool)
		For i As Integer = 0 To list.Count - 1
			If Not list(i).inUse Then
				effect = list(i)
				Exit For
			End If
		Next
		If effect Is Nothing Then
			effect = If((Not isSparkle), Me.smokePrefab.Create(pos), Me.sparklePrefab.Create(pos))
			list.Add(effect)
		End If
		effect.Initialize(pos)
		effect.animator.Play(If((Not isPink), Me.EffectReset, Me.EffectResetPink))
		effect.inUse = True
	End Sub

	' Token: 0x06000643 RID: 1603 RVA: 0x0006DB88 File Offset: 0x0006BF88
	Private Sub ClearFX(pool As List(Of Effect))
		For i As Integer = 0 To pool.Count - 1
			If pool(i).inUse Then
				pool(i).removeOnEnd = True
			Else
				Global.UnityEngine.[Object].Destroy(pool(i).gameObject)
			End If
		Next
	End Sub

	' Token: 0x06000644 RID: 1604 RVA: 0x0006DBE0 File Offset: 0x0006BFE0
	Private Iterator Function oldmanPattern_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		While True
			Yield MyBase.StartCoroutine(Me.nextPattern_cr())
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000645 RID: 1605 RVA: 0x0006DBFC File Offset: 0x0006BFFC
	Private Iterator Function nextPattern_cr() As IEnumerator
		Dim p As LevelProperties.OldMan.Pattern = Me.properties.CurrentState.NextPattern
		While p = LevelProperties.OldMan.Pattern.Camel AndAlso Me.firstAttack
			p = Me.properties.CurrentState.NextPattern
		End While
		Me.firstAttack = False
		Select Case p
			Case LevelProperties.OldMan.Pattern.Spit
				Yield MyBase.StartCoroutine(Me.spit_cr())
			Case LevelProperties.OldMan.Pattern.Duck
				Yield MyBase.StartCoroutine(Me.duck_cr())
			Case LevelProperties.OldMan.Pattern.Camel
				Yield MyBase.StartCoroutine(Me.camel_cr())
			Case Else
				Yield CupheadTime.WaitForSeconds(Me, 1F)
		End Select
		Return
	End Function

	' Token: 0x06000646 RID: 1606 RVA: 0x0006DC18 File Offset: 0x0006C018
	Private Iterator Function spit_cr() As IEnumerator
		While Me.oldMan.state <> OldManLevelOldMan.State.Idle
			Yield Nothing
		End While
		Me.oldMan.Spit()
		While Me.oldMan.state <> OldManLevelOldMan.State.Idle
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000647 RID: 1607 RVA: 0x0006DC34 File Offset: 0x0006C034
	Private Iterator Function duck_cr() As IEnumerator
		While Me.oldMan.state <> OldManLevelOldMan.State.Idle
			Yield Nothing
		End While
		Me.oldMan.Goose()
		While Me.oldMan.state <> OldManLevelOldMan.State.Idle
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000648 RID: 1608 RVA: 0x0006DC50 File Offset: 0x0006C050
	Private Iterator Function camel_cr() As IEnumerator
		While Me.oldMan.state <> OldManLevelOldMan.State.Idle
			Yield Nothing
		End While
		Me.oldMan.Bear()
		While Me.oldMan.state <> OldManLevelOldMan.State.Idle
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000649 RID: 1609 RVA: 0x0006DC6C File Offset: 0x0006C06C
	Private Iterator Function gnome_turrets_cr() As IEnumerator
		Dim p As LevelProperties.OldMan.Turret = Me.properties.CurrentState.turret
		Me.gnomesSpawned = New List(Of OldManLevelSpikeFloor)()
		Dim appearString As PatternString = New PatternString(p.appearOrder, True, True)
		While True
			While Not p.gnomesOn
				Yield Nothing
			End While
			Yield CupheadTime.WaitForSeconds(Me, p.appearDelayRange.RandomFloat())
			Me.gnomesSpawned.RemoveAll(Function(g As OldManLevelSpikeFloor) g.spikeState <> OldManLevelSpikeFloor.SpikeState.Gnomed)
			If Me.gnomesSpawned.Count < p.maxCount Then
				Dim appearOrder As Integer = 0
				Do
					appearOrder = appearString.PopInt()
					Yield Nothing
				Loop While Me.spikes(appearOrder).spikeState <> OldManLevelSpikeFloor.SpikeState.Idle
				Me.spikes(appearOrder).SpawnGnome()
				Me.gnomesSpawned.Add(Me.spikes(appearOrder))
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600064A RID: 1610 RVA: 0x0006DC88 File Offset: 0x0006C088
	Private Iterator Function climbers_cr() As IEnumerator
		Dim p As LevelProperties.OldMan.ClimberGnomes = Me.properties.CurrentState.climberGnomes
		While True
			Yield Nothing
			Dim pos As Integer = Me.climberPosString.PopInt()
			Dim platform As Integer = 4 - pos / 2
			If Not Me.platformManager.PlatformRemoved(platform) Then
				Dim oldManLevelGnomeClimber As OldManLevelGnomeClimber = Me.gnomeClimberPrefab.Spawn()
				Dim num As Single = CSng(If((pos Mod 2 <> 0), 1, (-1)))
				Dim platform2 As Transform = Me.platformManager.GetPlatform(platform)
				oldManLevelGnomeClimber.Init(Me.climberXPosition(pos), num, platform2, p)
				Me.platformManager.AttachGnome(platform, oldManLevelGnomeClimber)
			End If
			Yield CupheadTime.WaitForSeconds(Me, p.spawnDelayRange.RandomFloat())
		End While
		Return
	End Function

	' Token: 0x0600064B RID: 1611 RVA: 0x0006DCA4 File Offset: 0x0006C0A4
	Public Sub ActivatePhase2Beard()
		For Each gameObject As GameObject In Me.hairObjects
			gameObject.SetActive(True)
		Next
	End Sub

	' Token: 0x0600064C RID: 1612 RVA: 0x0006DCD8 File Offset: 0x0006C0D8
	Private Iterator Function phase_2_trans_cr() As IEnumerator
		Me.oldMan.EndPhase1()
		Me.ClearFX(Me.sparkleFXPool)
		Me.ClearFX(Me.smokeFXPool)
		Yield Me.oldMan.animator.WaitForAnimationToStart(Me, "Phase_Trans", False)
		Me.oldMan.StopAllCoroutines()
		Yield Nothing
		For Each oldManLevelSpikeFloor As OldManLevelSpikeFloor In Me.spikes
			oldManLevelSpikeFloor.[Exit]()
		Next
		Me.platformManager.EndPhase()
		While Me.oldMan.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.68421054F
			Yield Nothing
		End While
		Level.Current.SetBounds(New Integer?(1249), New Integer?(331), Nothing, Nothing)
		CupheadLevelCamera.Current.ChangeHorizontalBounds(1002, 85)
		Dim cameraEndPos As Vector3 = New Vector3(-460F, 0F, 0F)
		MyBase.StartCoroutine(CupheadLevelCamera.Current.slide_camera_cr(cameraEndPos, 3F))
		MyBase.StartCoroutine(Me.move_clouds_cr(3F))
		Yield CupheadTime.WaitForSeconds(Me, 2F)
		Me.oldMan.OnPhase2()
		Yield CupheadTime.WaitForSeconds(Me, 2F)
		Me.bleachers.SetActive(True)
		Yield Nothing
		Return
	End Function

	' Token: 0x0600064D RID: 1613 RVA: 0x0006DCF4 File Offset: 0x0006C0F4
	Private Iterator Function move_clouds_cr(time As Single) As IEnumerator
		Dim leftStartPos As Single = Me.cloudLeft.transform.localPosition.x
		Dim rightStartPos As Single = Me.cloudRight.transform.localPosition.x
		Dim t As Single = 0F
		While t < time
			t += CupheadTime.Delta
			Me.cloudLeft.transform.localPosition = New Vector3(EaseUtils.EaseInOutSine(leftStartPos, -720F, t / time), Me.cloudLeft.transform.localPosition.y)
			Me.cloudRight.transform.localPosition = New Vector3(EaseUtils.EaseInOutSine(rightStartPos, 420F, t / time), Me.cloudRight.transform.localPosition.y)
			Yield Nothing
		End While
		Me.cloudLeft.transform.localPosition = New Vector3(-720F, Me.cloudLeft.transform.localPosition.y)
		Me.cloudRight.transform.localPosition = New Vector3(420F, Me.cloudRight.transform.localPosition.y)
		Return
	End Function

	' Token: 0x0600064E RID: 1614 RVA: 0x0006DD16 File Offset: 0x0006C116
	Public Function InPhase2() As Boolean
		Return Me.properties.CurrentState.stateName = LevelProperties.OldMan.States.SockPuppet
	End Function

	' Token: 0x0600064F RID: 1615 RVA: 0x0006DD2C File Offset: 0x0006C12C
	Private Iterator Function phase_3_trans_cr() As IEnumerator
		Global.UnityEngine.[Object].Destroy(Me.platformManager.gameObject)
		Me.sockPuppet.OnPhase3()
		Dim p As AbstractPlayerController = PlayerManager.GetPlayer(PlayerId.PlayerOne)
		Dim weaponManagerP As LevelPlayerWeaponManager = p.GetComponent(Of LevelPlayerWeaponManager)()
		Dim motorP As LevelPlayerMotor = p.GetComponent(Of LevelPlayerMotor)()
		weaponManagerP.InterruptSuper()
		Dim weaponManagerP2 As LevelPlayerWeaponManager = Nothing
		Dim motorP2 As LevelPlayerMotor = Nothing
		Dim p2 As AbstractPlayerController = PlayerManager.GetPlayer(PlayerId.PlayerTwo)
		If p2 IsNot Nothing Then
			motorP2 = p2.GetComponent(Of LevelPlayerMotor)()
			weaponManagerP2 = p2.GetComponent(Of LevelPlayerWeaponManager)()
			weaponManagerP2.InterruptSuper()
		End If
		Yield New WaitForEndOfFrame()
		While Me.sockPuppet.transState <> OldManLevelSockPuppetHandler.TransitionState.PlatformDestroyed
			Yield Nothing
		End While
		Me.mainPlatform.SetActive(False)
		Me.phaseTransitionTrigger.gameObject.SetActive(True)
		Me.mainPit.SetActive(False)
		If motorP Then
			motorP.OnTrampolineKnockUp(-2.3F)
		End If
		If motorP2 Then
			motorP2.OnTrampolineKnockUp(-2.3F)
		End If
		Dim readyToGo As Boolean = False
		While Not readyToGo
			If(p.IsDead OrElse Me.phaseTransitionTrigger.bounds.Contains(p.transform.position + Vector3.down * 10F)) AndAlso (PlayerManager.GetPlayer(PlayerId.PlayerTwo) Is Nothing OrElse PlayerManager.GetPlayer(PlayerId.PlayerTwo).IsDead OrElse Me.phaseTransitionTrigger.bounds.Contains(PlayerManager.GetPlayer(PlayerId.PlayerTwo).transform.position + Vector3.down * 10F)) Then
				readyToGo = True
				Me.sockPuppet.SwallowedPlayers()
			End If
			If p.IsDead OrElse Me.phaseTransitionTrigger.bounds.Contains(p.transform.position + Vector3.down * 10F) Then
				p.gameObject.SetActive(False)
			End If
			If(PlayerManager.GetPlayer(PlayerId.PlayerTwo) Is Nothing OrElse PlayerManager.GetPlayer(PlayerId.PlayerTwo).IsDead OrElse Me.phaseTransitionTrigger.bounds.Contains(PlayerManager.GetPlayer(PlayerId.PlayerTwo).transform.position + Vector3.down * 10F)) AndAlso PlayerManager.GetPlayer(PlayerId.PlayerTwo) IsNot Nothing Then
				PlayerManager.GetPlayer(PlayerId.PlayerTwo).gameObject.SetActive(False)
			End If
			Yield Nothing
		End While
		Dim ghosts As PlayerDeathEffect() = TryCast(Global.UnityEngine.[Object].FindObjectsOfType(GetType(PlayerDeathEffect)), PlayerDeathEffect())
		For Each playerDeathEffect As PlayerDeathEffect In ghosts
			playerDeathEffect.transform.position += Vector3.up * 5000F
		Next
		Dim superGhosts As PlayerSuperGhost() = TryCast(Global.UnityEngine.[Object].FindObjectsOfType(GetType(PlayerSuperGhost)), PlayerSuperGhost())
		For Each playerSuperGhost As PlayerSuperGhost In superGhosts
			Global.UnityEngine.[Object].Destroy(playerSuperGhost.gameObject)
		Next
		Dim superGhostHearts As PlayerSuperGhostHeart() = TryCast(Global.UnityEngine.[Object].FindObjectsOfType(GetType(PlayerSuperGhostHeart)), PlayerSuperGhostHeart())
		For Each playerSuperGhostHeart As PlayerSuperGhostHeart In superGhostHearts
			Global.UnityEngine.[Object].Destroy(playerSuperGhostHeart)
		Next
		While Me.sockPuppet.transState <> OldManLevelSockPuppetHandler.TransitionState.InStomach
			Yield Nothing
		End While
		Yield MyBase.StartCoroutine(Me.iris_cr())
		If Not p.IsDead Then
			motorP.EnableInput()
		End If
		p2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo)
		If p2 IsNot Nothing AndAlso Not p2.IsDead Then
			motorP2.EnableInput()
		End If
		MyBase.StartCoroutine(Me.scuba_gnomes_cr())
		Yield Nothing
		Return
	End Function

	' Token: 0x06000650 RID: 1616 RVA: 0x0006DD48 File Offset: 0x0006C148
	Private Iterator Function iris_cr() As IEnumerator
		Dim pauseGUI As LevelPauseGUI = GameObject.Find("Level_UI").GetComponentInChildren(Of LevelPauseGUI)()
		pauseGUI.ForceDisablePause(True)
		Dim faderAni As Animator = Me.fader.GetComponent(Of Animator)()
		Dim c As Color = Me.fader.color
		c.a = 1F
		Me.fader.color = c
		faderAni.SetTrigger("Iris_In")
		Yield faderAni.WaitForAnimationToEnd(Me, "Iris_In", False, True)
		Yield New WaitForSeconds(0.9F)
		Me.SetupStomach()
		faderAni.SetTrigger("Iris_Out")
		Me.gnomeLeader.StartGnomeLeader()
		Yield faderAni.WaitForAnimationToEnd(Me, "Iris_Out", False, True)
		pauseGUI.ForceDisablePause(False)
		c = Me.fader.color
		c.a = 0F
		Me.fader.color = c
		Return
	End Function

	' Token: 0x06000651 RID: 1617 RVA: 0x0006DD64 File Offset: 0x0006C164
	Private Sub SetupStomach()
		Level.Current.SetBounds(New Integer?(1249), New Integer?(331), Nothing, Nothing)
		Me.gnomeLeader.gameObject.SetActive(True)
		Me.phaseTransitionTrigger.gameObject.SetActive(False)
		Me.mountainBG.SetActive(False)
		Me.stomachBG.SetActive(True)
		Me.sockPuppet.FinishPuppet()
		Dim player As AbstractPlayerController = PlayerManager.GetPlayer(PlayerId.PlayerOne)
		If Not player.IsDead Then
			player.gameObject.SetActive(True)
			Dim component As LevelPlayerMotor = player.GetComponent(Of LevelPlayerMotor)()
			component.ClearBufferedInput()
			component.ForceLooking(New Trilean2(1, 1))
			player.GetComponent(Of LevelPlayerAnimationController)().ResetMoveX()
			component.OnRevive(Me.gnomeLeader.platformPositions(1).position + Vector3.up * 1000F)
			component.CancelReviveBounce()
			component.EnableInput()
		End If
		Dim player2 As AbstractPlayerController = PlayerManager.GetPlayer(PlayerId.PlayerTwo)
		If player2 IsNot Nothing AndAlso Not player2.IsDead Then
			player2.gameObject.SetActive(True)
			Dim levelPlayerMotor As LevelPlayerMotor = player2.GetComponent(Of LevelPlayerMotor)()
			levelPlayerMotor = player2.GetComponent(Of LevelPlayerMotor)()
			levelPlayerMotor.ClearBufferedInput()
			levelPlayerMotor.ForceLooking(New Trilean2(1, 1))
			player2.GetComponent(Of LevelPlayerAnimationController)().ResetMoveX()
			levelPlayerMotor.OnRevive(Me.gnomeLeader.platformPositions(3).position + Vector3.up * 1000F)
			levelPlayerMotor.CancelReviveBounce()
			levelPlayerMotor.EnableInput()
		End If
		Me.SFX_StomachLoop()
	End Sub

	' Token: 0x06000652 RID: 1618 RVA: 0x0006DEFC File Offset: 0x0006C2FC
	Private Iterator Function scuba_gnomes_cr() As IEnumerator
		Dim onLeft As Boolean = Rand.Bool()
		Dim p As LevelProperties.OldMan.ScubaGnomes = Me.properties.CurrentState.scubaGnomes
		Dim scubaTypeString As PatternString = New PatternString(p.scubaTypeString, True, True)
		Dim spawnDelayString As PatternString = New PatternString(p.spawnDelayString, True, True)
		Dim dartParryableString As PatternString = New PatternString(p.dartParryableString, True)
		Dim offset As Single = 50F
		While True
			Dim xPos As Single = If((Not onLeft), (CupheadLevelCamera.Current.Bounds.xMax - offset), (CupheadLevelCamera.Current.Bounds.xMin + offset))
			Dim scubaGnome As OldManLevelScubaGnome = Me.scubaGnomePrefab.Spawn()
			scubaGnome.Init(New Vector3(xPos, CupheadLevelCamera.Current.Bounds.yMin), PlayerManager.GetNext(), scubaTypeString.PopLetter() = "A"c, onLeft, dartParryableString.PopLetter() = "P"c, p, Me.gnomeLeader)
			Yield CupheadTime.WaitForSeconds(Me, spawnDelayString.PopFloat())
			onLeft = Not onLeft
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000653 RID: 1619 RVA: 0x0006DF18 File Offset: 0x0006C318
	Private Sub SFX_StomachLoop()
		MyBase.transform.position = Me.stomachBG.transform.position
		AudioManager.PlayLoop("sfx_dlc_omm_p3_stomachacid_amb_loop")
		Me.emitAudioFromObject.Add("sfx_dlc_omm_p3_stomachacid_amb_loop")
		AudioManager.FadeSFXVolume("sfx_dlc_omm_p3_stomachacid_amb_loop", 1F, 1F)
	End Sub

	' Token: 0x06000654 RID: 1620 RVA: 0x0006DF70 File Offset: 0x0006C370
	Private Sub WORKAROUND_NullifyFields()
		Me.platformManager = Nothing
		Me.fader = Nothing
		Me.hairObjects = Nothing
		Me.scubaGnomePrefab = Nothing
		Me.mainPlatform = Nothing
		Me.oldMan = Nothing
		Me.sockPuppet = Nothing
		Me.gnomeLeader = Nothing
		Me.gnomeClimberPrefab = Nothing
		Me.spikes = Nothing
		Me.mountainBG = Nothing
		Me.cloudLeft = Nothing
		Me.cloudRight = Nothing
		Me.stomachBG = Nothing
		Me.phaseTransitionTrigger = Nothing
		Me.mainPit = Nothing
		Me.bleachers = Nothing
		Me.gnomesSpawned = Nothing
		Me.climberPosString = Nothing
		Me.climberXPosition = Nothing
		Me._bossPortraitMain = Nothing
		Me._bossPortraitPhaseTwo = Nothing
		Me._bossPortraitPhaseThree = Nothing
		Me._bossQuoteMain = Nothing
		Me._bossQuotePhaseTwo = Nothing
		Me._bossQuotePhaseThree = Nothing
	End Sub

	' Token: 0x04000BB2 RID: 2994
	Private properties As LevelProperties.OldMan

	' Token: 0x04000BB3 RID: 2995
	Private EffectReset As Integer = Animator.StringToHash("Reset")

	' Token: 0x04000BB4 RID: 2996
	Private EffectResetPink As Integer = Animator.StringToHash("ResetPink")

	' Token: 0x04000BB5 RID: 2997
	Private Const CAM_END_POS_X As Single = -460F

	' Token: 0x04000BB6 RID: 2998
	Private Const CAM_MOVE_TIME As Single = 3F

	' Token: 0x04000BB7 RID: 2999
	Private Const CAM_PHASE2_BOUNDS_LEFT As Integer = 1002

	' Token: 0x04000BB8 RID: 3000
	Private Const CAM_PHASE2_BOUNDS_RIGHT As Integer = 85

	' Token: 0x04000BB9 RID: 3001
	Private Const PHASE2_BOUNDS_LEFT As Integer = 1249

	' Token: 0x04000BBA RID: 3002
	Private Const PHASE2_BOUNDS_RIGHT As Integer = 331

	' Token: 0x04000BBB RID: 3003
	Private Const IRIS_TIME As Single = 0.9F

	' Token: 0x04000BBC RID: 3004
	<SerializeField()>
	Private fader As Image

	' Token: 0x04000BBD RID: 3005
	<SerializeField()>
	Private hairObjects As GameObject()

	' Token: 0x04000BBE RID: 3006
	<SerializeField()>
	Private scubaGnomePrefab As OldManLevelScubaGnome

	' Token: 0x04000BBF RID: 3007
	<SerializeField()>
	Private mainPlatform As GameObject

	' Token: 0x04000BC0 RID: 3008
	Public platformManager As OldManLevelPlatformManager

	' Token: 0x04000BC1 RID: 3009
	<SerializeField()>
	Private oldMan As OldManLevelOldMan

	' Token: 0x04000BC2 RID: 3010
	<SerializeField()>
	Private sockPuppet As OldManLevelSockPuppetHandler

	' Token: 0x04000BC3 RID: 3011
	<SerializeField()>
	Private gnomeLeader As OldManLevelGnomeLeader

	' Token: 0x04000BC4 RID: 3012
	<SerializeField()>
	Private gnomeClimberPrefab As OldManLevelGnomeClimber

	' Token: 0x04000BC5 RID: 3013
	<SerializeField()>
	Private spikes As OldManLevelSpikeFloor()

	' Token: 0x04000BC6 RID: 3014
	<SerializeField()>
	Private mountainBG As GameObject

	' Token: 0x04000BC7 RID: 3015
	<SerializeField()>
	Private cloudLeft As GameObject

	' Token: 0x04000BC8 RID: 3016
	<SerializeField()>
	Private cloudRight As GameObject

	' Token: 0x04000BC9 RID: 3017
	<SerializeField()>
	Private stomachBG As GameObject

	' Token: 0x04000BCA RID: 3018
	<SerializeField()>
	Private phaseTransitionTrigger As Collider2D

	' Token: 0x04000BCB RID: 3019
	<SerializeField()>
	Private mainPit As GameObject

	' Token: 0x04000BCC RID: 3020
	<SerializeField()>
	Private bleachers As GameObject

	' Token: 0x04000BCD RID: 3021
	Private gnomesSpawned As List(Of OldManLevelSpikeFloor)

	' Token: 0x04000BCE RID: 3022
	Private climberPosString As PatternString

	' Token: 0x04000BCF RID: 3023
	Public playedFirstSpikeSound As Boolean

	' Token: 0x04000BD0 RID: 3024
	Private firstAttack As Boolean

	' Token: 0x04000BD1 RID: 3025
	Private climberXPosition As Single() = New Single() { -1006F, -766F, -792F, -562.2F, -590.8F, -357F, -377F, -147.7F, -163.2F, 63.2F }

	' Token: 0x04000BD2 RID: 3026
	Private smokeFXPool As List(Of Effect) = New List(Of Effect)()

	' Token: 0x04000BD3 RID: 3027
	<SerializeField()>
	Private smokePrefab As Effect

	' Token: 0x04000BD4 RID: 3028
	Private sparkleFXPool As List(Of Effect) = New List(Of Effect)()

	' Token: 0x04000BD5 RID: 3029
	<SerializeField()>
	Private sparklePrefab As Effect

	' Token: 0x04000BD6 RID: 3030
	<Header("Boss Info")>
	<SerializeField()>
	Private _bossPortraitMain As Sprite

	' Token: 0x04000BD7 RID: 3031
	<SerializeField()>
	Private _bossPortraitPhaseTwo As Sprite

	' Token: 0x04000BD8 RID: 3032
	<SerializeField()>
	Private _bossPortraitPhaseThree As Sprite

	' Token: 0x04000BD9 RID: 3033
	<SerializeField()>
	Private _bossQuoteMain As String

	' Token: 0x04000BDA RID: 3034
	<SerializeField()>
	Private _bossQuotePhaseTwo As String

	' Token: 0x04000BDB RID: 3035
	<SerializeField()>
	Private _bossQuotePhaseThree As String
End Class
