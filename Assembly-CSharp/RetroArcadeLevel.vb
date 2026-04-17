Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000251 RID: 593
Public Class RetroArcadeLevel
	Inherits Level

	' Token: 0x06000698 RID: 1688 RVA: 0x000708F4 File Offset: 0x0006ECF4
	Protected Overrides Sub PartialInit()
		Me.properties = LevelProperties.RetroArcade.GetMode(MyBase.mode)
		AddHandler Me.properties.OnStateChange, AddressOf MyBase.zHack_OnStateChanged
		AddHandler Me.properties.OnBossDeath, AddressOf MyBase.zHack_OnWin
		MyBase.timeline = Me.properties.CreateTimeline(MyBase.mode)
		Me.goalTimes = Me.properties.goalTimes
		AddHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		MyBase.PartialInit()
	End Sub

	' Token: 0x1700011B RID: 283
	' (get) Token: 0x06000699 RID: 1689 RVA: 0x0007098A File Offset: 0x0006ED8A
	Public Overrides ReadOnly Property CurrentLevel As Levels
		Get
			Return Levels.RetroArcade
		End Get
	End Property

	' Token: 0x1700011C RID: 284
	' (get) Token: 0x0600069A RID: 1690 RVA: 0x00070991 File Offset: 0x0006ED91
	Public Overrides ReadOnly Property CurrentScene As Scenes
		Get
			Return Scenes.scene_level_retro_arcade
		End Get
	End Property

	' Token: 0x1700011D RID: 285
	' (get) Token: 0x0600069B RID: 1691 RVA: 0x00070995 File Offset: 0x0006ED95
	' (set) Token: 0x0600069C RID: 1692 RVA: 0x0007099C File Offset: 0x0006ED9C
	Public Shared Property ACCURACY_BONUS As Single

	' Token: 0x1700011E RID: 286
	' (get) Token: 0x0600069D RID: 1693 RVA: 0x000709A4 File Offset: 0x0006EDA4
	Public Overrides ReadOnly Property BossPortrait As Sprite
		Get
			Return Me._bossPortrait
		End Get
	End Property

	' Token: 0x1700011F RID: 287
	' (get) Token: 0x0600069E RID: 1694 RVA: 0x000709AC File Offset: 0x0006EDAC
	Public Overrides ReadOnly Property BossQuote As String
		Get
			Return Me._bossQuote
		End Get
	End Property

	' Token: 0x0600069F RID: 1695 RVA: 0x000709B4 File Offset: 0x0006EDB4
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.alienManager.LevelInit(Me.properties)
		Me.caterpillarManager.LevelInit(Me.properties)
		Me.robotManager.LevelInit(Me.properties)
		Me.paddleShip.LevelInit(Me.properties)
		Me.qShip.LevelInit(Me.properties)
		Me.ufo.LevelInit(Me.properties)
		Me.toadManager.LevelInit(Me.properties)
		Me.worm.LevelInit(Me.properties)
		Me.bouncyManager.LevelInit(Me.properties)
		Me.missileMan.LevelInit(Me.properties)
		Me.chaserManager.LevelInit(Me.properties)
		Me.sheriffManager.LevelInit(Me.properties)
		Me.snakeManager.LevelInit(Me.properties)
		Me.tentacleManager.LevelInit(Me.properties)
		Me.trafficManager.LevelInit(Me.properties)
		RetroArcadeLevel.ACCURACY_BONUS = Me.properties.CurrentState.general.accuracyBonus
		Me.bigCuphead.Init(TryCast(PlayerManager.GetPlayer(PlayerId.PlayerOne), ArcadePlayerController))
		Me.bigMugman.Init(TryCast(PlayerManager.GetPlayer(PlayerId.PlayerTwo), ArcadePlayerController))
	End Sub

	' Token: 0x060006A0 RID: 1696 RVA: 0x00070B0C File Offset: 0x0006EF0C
	Protected Overrides Sub CreatePlayers()
		MyBase.CreatePlayers()
	End Sub

	' Token: 0x060006A1 RID: 1697 RVA: 0x00070B14 File Offset: 0x0006EF14
	Protected Overrides Sub Update()
		MyBase.Update()
	End Sub

	' Token: 0x060006A2 RID: 1698 RVA: 0x00070B1C File Offset: 0x0006EF1C
	Protected Overrides Sub OnLevelStart()
		Me.bigCuphead.LevelStart()
		Me.bigMugman.LevelStart()
		Me.StartStateCoroutine()
	End Sub

	' Token: 0x060006A3 RID: 1699 RVA: 0x00070B3A File Offset: 0x0006EF3A
	Protected Overrides Sub OnStateChanged()
		MyBase.OnStateChanged()
		Me.bigCuphead.OnVictory()
		Me.bigMugman.OnVictory()
		Me.StartStateCoroutine()
	End Sub

	' Token: 0x060006A4 RID: 1700 RVA: 0x00070B60 File Offset: 0x0006EF60
	Private Sub StartStateCoroutine()
		Select Case Me.properties.CurrentState.stateName
			Case LevelProperties.RetroArcade.States.Main, LevelProperties.RetroArcade.States.MissileMan
				MyBase.StartCoroutine(Me.startMissile_cr())
			Case LevelProperties.RetroArcade.States.Caterpillar
				MyBase.StartCoroutine(Me.startCaterpillars_cr())
			Case LevelProperties.RetroArcade.States.Robots
				MyBase.StartCoroutine(Me.startRobots_cr())
			Case LevelProperties.RetroArcade.States.PaddleShip
				MyBase.StartCoroutine(Me.startPaddleShip_cr())
			Case LevelProperties.RetroArcade.States.QShip
				MyBase.StartCoroutine(Me.startQShip_cr())
			Case LevelProperties.RetroArcade.States.UFO
				MyBase.StartCoroutine(Me.startUFO_cr())
			Case LevelProperties.RetroArcade.States.Toad
				MyBase.StartCoroutine(Me.startToad_cr())
			Case LevelProperties.RetroArcade.States.Worm
				MyBase.StartCoroutine(Me.startWorm_cr())
			Case LevelProperties.RetroArcade.States.Aliens
				MyBase.StartCoroutine(Me.startAliens_cr())
			Case LevelProperties.RetroArcade.States.Bouncy
				MyBase.StartCoroutine(Me.startBouncy_cr())
			Case LevelProperties.RetroArcade.States.Chaser
				MyBase.StartCoroutine(Me.startChaser_cr())
			Case LevelProperties.RetroArcade.States.Sheriff
				MyBase.StartCoroutine(Me.startSheriff_cr())
			Case LevelProperties.RetroArcade.States.Snake
				MyBase.StartCoroutine(Me.startSnake_cr())
			Case LevelProperties.RetroArcade.States.Tentacle
				MyBase.StartCoroutine(Me.startTentacle_cr())
			Case LevelProperties.RetroArcade.States.Traffic
				MyBase.StartCoroutine(Me.startTrafficUFO_cr())
			Case LevelProperties.RetroArcade.States.JetpackTest
				MyBase.StartCoroutine(Me.switchToJetpack_cr())
		End Select
	End Sub

	' Token: 0x060006A5 RID: 1701 RVA: 0x00070CF1 File Offset: 0x0006F0F1
	Public Overrides Sub OnLevelEnd()
		MyBase.OnLevelEnd()
		Me.bigCuphead.OnVictory()
		Me.bigMugman.OnVictory()
	End Sub

	' Token: 0x060006A6 RID: 1702 RVA: 0x00070D10 File Offset: 0x0006F110
	Private Iterator Function startAliens_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		Me.alienManager.StartAliens()
		Return
	End Function

	' Token: 0x060006A7 RID: 1703 RVA: 0x00070D2C File Offset: 0x0006F12C
	Private Iterator Function startCaterpillars_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		Me.caterpillarManager.StartCaterpillar()
		Return
	End Function

	' Token: 0x060006A8 RID: 1704 RVA: 0x00070D48 File Offset: 0x0006F148
	Private Iterator Function startRobots_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		Me.robotManager.StartRobots()
		Return
	End Function

	' Token: 0x060006A9 RID: 1705 RVA: 0x00070D64 File Offset: 0x0006F164
	Private Iterator Function startPaddleShip_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		Me.paddleShip.StartPaddleShip()
		Return
	End Function

	' Token: 0x060006AA RID: 1706 RVA: 0x00070D80 File Offset: 0x0006F180
	Private Iterator Function startQShip_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		Me.qShip.StartQShip()
		Return
	End Function

	' Token: 0x060006AB RID: 1707 RVA: 0x00070D9C File Offset: 0x0006F19C
	Private Iterator Function startUFO_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		Me.ufo.StartUFO()
		Return
	End Function

	' Token: 0x060006AC RID: 1708 RVA: 0x00070DB8 File Offset: 0x0006F1B8
	Private Iterator Function startToad_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		Me.toadManager.StartToad()
		Return
	End Function

	' Token: 0x060006AD RID: 1709 RVA: 0x00070DD4 File Offset: 0x0006F1D4
	Private Iterator Function startWorm_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		Me.worm.StartWorm()
		Return
	End Function

	' Token: 0x060006AE RID: 1710 RVA: 0x00070DF0 File Offset: 0x0006F1F0
	Private Iterator Function startBouncy_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		Me.bouncyManager.StartBouncy()
		Return
	End Function

	' Token: 0x060006AF RID: 1711 RVA: 0x00070E0C File Offset: 0x0006F20C
	Private Iterator Function startMissile_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		Me.missileMan.StartMissile()
		Return
	End Function

	' Token: 0x060006B0 RID: 1712 RVA: 0x00070E28 File Offset: 0x0006F228
	Private Iterator Function switchToRocket_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		Dim player As ArcadePlayerController = PlayerManager.GetPlayer(Of ArcadePlayerController)(PlayerId.PlayerOne)
		player.ChangeToRocket()
		If PlayerManager.GetPlayer(PlayerId.PlayerTwo) IsNot Nothing Then
			Dim player2 As ArcadePlayerController = PlayerManager.GetPlayer(Of ArcadePlayerController)(PlayerId.PlayerTwo)
			player2.ChangeToRocket()
		End If
		Return
	End Function

	' Token: 0x060006B1 RID: 1713 RVA: 0x00070E44 File Offset: 0x0006F244
	Private Iterator Function startChaser_cr() As IEnumerator
		Dim player As ArcadePlayerController = PlayerManager.GetPlayer(Of ArcadePlayerController)(PlayerId.PlayerOne)
		If player.controlScheme <> ArcadePlayerController.ControlScheme.Rocket Then
			Yield MyBase.StartCoroutine(Me.switchToRocket_cr())
		End If
		Yield CupheadTime.WaitForSeconds(Me, 3F)
		Me.chaserManager.StartChasers()
		Return
	End Function

	' Token: 0x060006B2 RID: 1714 RVA: 0x00070E60 File Offset: 0x0006F260
	Private Iterator Function startSheriff_cr() As IEnumerator
		Dim player As ArcadePlayerController = PlayerManager.GetPlayer(Of ArcadePlayerController)(PlayerId.PlayerOne)
		If player.controlScheme <> ArcadePlayerController.ControlScheme.Rocket Then
			Yield MyBase.StartCoroutine(Me.switchToRocket_cr())
		End If
		Yield CupheadTime.WaitForSeconds(Me, 3F)
		Me.sheriffManager.StartSheriff()
		Return
	End Function

	' Token: 0x060006B3 RID: 1715 RVA: 0x00070E7C File Offset: 0x0006F27C
	Private Iterator Function startSnake_cr() As IEnumerator
		Dim player As ArcadePlayerController = PlayerManager.GetPlayer(Of ArcadePlayerController)(PlayerId.PlayerOne)
		If player.controlScheme <> ArcadePlayerController.ControlScheme.Rocket Then
			Yield MyBase.StartCoroutine(Me.switchToRocket_cr())
		End If
		Yield CupheadTime.WaitForSeconds(Me, 3F)
		Me.snakeManager.StartSnake()
		Return
	End Function

	' Token: 0x060006B4 RID: 1716 RVA: 0x00070E98 File Offset: 0x0006F298
	Private Iterator Function startTentacle_cr() As IEnumerator
		Dim player As ArcadePlayerController = PlayerManager.GetPlayer(Of ArcadePlayerController)(PlayerId.PlayerOne)
		If player.controlScheme <> ArcadePlayerController.ControlScheme.Rocket Then
			Yield MyBase.StartCoroutine(Me.switchToRocket_cr())
		End If
		Yield CupheadTime.WaitForSeconds(Me, 3F)
		Me.tentacleManager.StartTentacle()
		Return
	End Function

	' Token: 0x060006B5 RID: 1717 RVA: 0x00070EB4 File Offset: 0x0006F2B4
	Private Iterator Function startTrafficUFO_cr() As IEnumerator
		Dim player As ArcadePlayerController = PlayerManager.GetPlayer(Of ArcadePlayerController)(PlayerId.PlayerOne)
		If player.controlScheme <> ArcadePlayerController.ControlScheme.Rocket Then
			Yield MyBase.StartCoroutine(Me.switchToRocket_cr())
		End If
		Yield CupheadTime.WaitForSeconds(Me, 3F)
		Me.trafficManager.StartTraffic()
		Return
	End Function

	' Token: 0x060006B6 RID: 1718 RVA: 0x00070ED0 File Offset: 0x0006F2D0
	Private Iterator Function switchToJetpack_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		Dim player As ArcadePlayerController = PlayerManager.GetPlayer(Of ArcadePlayerController)(PlayerId.PlayerOne)
		player.ChangeToJetpack()
		If PlayerManager.GetPlayer(PlayerId.PlayerTwo) IsNot Nothing Then
			Dim player2 As ArcadePlayerController = PlayerManager.GetPlayer(Of ArcadePlayerController)(PlayerId.PlayerTwo)
			player2.ChangeToJetpack()
		End If
		Yield Nothing
		Return
	End Function

	' Token: 0x04000CED RID: 3309
	Private properties As LevelProperties.RetroArcade

	' Token: 0x04000CEE RID: 3310
	Public Shared TOTAL_POINTS As Single

	' Token: 0x04000CF0 RID: 3312
	<Header("Boss Info")>
	<SerializeField()>
	Private _bossPortrait As Sprite

	' Token: 0x04000CF1 RID: 3313
	<SerializeField()>
	<Multiline()>
	Private _bossQuote As String

	' Token: 0x04000CF2 RID: 3314
	<SerializeField()>
	Private trafficManager As RetroArcadeTrafficManager

	' Token: 0x04000CF3 RID: 3315
	<SerializeField()>
	Private tentacleManager As RetroArcadeTentacleManager

	' Token: 0x04000CF4 RID: 3316
	<SerializeField()>
	Private snakeManager As RetroArcadeSnakeManager

	' Token: 0x04000CF5 RID: 3317
	<SerializeField()>
	Private sheriffManager As RetroArcadeSheriffManager

	' Token: 0x04000CF6 RID: 3318
	<SerializeField()>
	Private chaserManager As RetroArcadeChaserManager

	' Token: 0x04000CF7 RID: 3319
	<SerializeField()>
	Private bouncyManager As RetroArcadeBouncyManager

	' Token: 0x04000CF8 RID: 3320
	<SerializeField()>
	Private alienManager As RetroArcadeAlienManager

	' Token: 0x04000CF9 RID: 3321
	<SerializeField()>
	Private caterpillarManager As RetroArcadeCaterpillarManager

	' Token: 0x04000CFA RID: 3322
	<SerializeField()>
	Private robotManager As RetroArcadeRobotManager

	' Token: 0x04000CFB RID: 3323
	<SerializeField()>
	Private paddleShip As RetroArcadePaddleShip

	' Token: 0x04000CFC RID: 3324
	<SerializeField()>
	Private qShip As RetroArcadeQShip

	' Token: 0x04000CFD RID: 3325
	<SerializeField()>
	Private ufo As RetroArcadeUFO

	' Token: 0x04000CFE RID: 3326
	<SerializeField()>
	Private toadManager As RetroArcadeToadManager

	' Token: 0x04000CFF RID: 3327
	<SerializeField()>
	Private missileMan As RetroArcadeMissileMan

	' Token: 0x04000D00 RID: 3328
	<SerializeField()>
	Private worm As RetroArcadeWorm

	' Token: 0x04000D01 RID: 3329
	<SerializeField()>
	Private bigCuphead As RetroArcadeBigPlayer

	' Token: 0x04000D02 RID: 3330
	<SerializeField()>
	Private bigMugman As RetroArcadeBigPlayer

	' Token: 0x04000D03 RID: 3331
	Public playerPrefab As ArcadePlayerController
End Class
