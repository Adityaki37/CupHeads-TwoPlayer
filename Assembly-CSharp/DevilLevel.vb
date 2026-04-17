Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020000E9 RID: 233
Public Class DevilLevel
	Inherits Level

	' Token: 0x0600028F RID: 655 RVA: 0x0005C73C File Offset: 0x0005AB3C
	Protected Overrides Sub PartialInit()
		Me.properties = LevelProperties.Devil.GetMode(MyBase.mode)
		AddHandler Me.properties.OnStateChange, AddressOf MyBase.zHack_OnStateChanged
		AddHandler Me.properties.OnBossDeath, AddressOf MyBase.zHack_OnWin
		MyBase.timeline = Me.properties.CreateTimeline(MyBase.mode)
		Me.goalTimes = Me.properties.goalTimes
		AddHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		MyBase.PartialInit()
	End Sub

	' Token: 0x17000068 RID: 104
	' (get) Token: 0x06000290 RID: 656 RVA: 0x0005C7D2 File Offset: 0x0005ABD2
	Public Overrides ReadOnly Property CurrentLevel As Levels
		Get
			Return Levels.Devil
		End Get
	End Property

	' Token: 0x17000069 RID: 105
	' (get) Token: 0x06000291 RID: 657 RVA: 0x0005C7D9 File Offset: 0x0005ABD9
	Public Overrides ReadOnly Property CurrentScene As Scenes
		Get
			Return Scenes.scene_level_devil
		End Get
	End Property

	' Token: 0x1700006A RID: 106
	' (get) Token: 0x06000292 RID: 658 RVA: 0x0005C7E0 File Offset: 0x0005ABE0
	Public Overrides ReadOnly Property BossPortrait As Sprite
		Get
			Select Case Me.properties.CurrentState.stateName
				Case LevelProperties.Devil.States.Main, LevelProperties.Devil.States.Generic
					Return Me._bossPortraitMain
				Case LevelProperties.Devil.States.GiantHead
					Return Me._bossPortraitPhaseTwo
				Case LevelProperties.Devil.States.Hands, LevelProperties.Devil.States.Tears
					Return Me._bossPortraitPhaseThree
			End Select
			Global.Debug.LogError("Couldn't find portrait for state " + Me.properties.CurrentState.stateName + ". Using Main.", Nothing)
			Return Me._bossPortraitMain
		End Get
	End Property

	' Token: 0x1700006B RID: 107
	' (get) Token: 0x06000293 RID: 659 RVA: 0x0005C868 File Offset: 0x0005AC68
	Public Overrides ReadOnly Property BossQuote As String
		Get
			Select Case Me.properties.CurrentState.stateName
				Case LevelProperties.Devil.States.Main, LevelProperties.Devil.States.Generic
					Return Me._bossQuoteMain
				Case LevelProperties.Devil.States.GiantHead
					Return Me._bossQuotePhaseTwo
				Case LevelProperties.Devil.States.Hands, LevelProperties.Devil.States.Tears
					Return Me._bossQuotePhaseThree
			End Select
			Global.Debug.LogError("Couldn't find quote for state " + Me.properties.CurrentState.stateName + ". Using Main.", Nothing)
			Return Me._bossQuoteMain
		End Get
	End Property

	' Token: 0x06000294 RID: 660 RVA: 0x0005C8EE File Offset: 0x0005ACEE
	Protected Overrides Sub Awake()
		MyBase.Awake()
	End Sub

	' Token: 0x06000295 RID: 661 RVA: 0x0005C8F6 File Offset: 0x0005ACF6
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.isDevil = True
		Me.sittingDevil.LevelInit(Me.properties)
		Me.giantHead.LevelInit(Me.properties)
		MyBase.StartCoroutine(Me.DelayedStart())
	End Sub

	' Token: 0x06000296 RID: 662 RVA: 0x0005C934 File Offset: 0x0005AD34
	Private Iterator Function DelayedStart() As IEnumerator
		Yield Nothing
		Me.phase2Background.SetActive(False)
		Return
	End Function

	' Token: 0x06000297 RID: 663 RVA: 0x0005C94F File Offset: 0x0005AD4F
	Protected Overrides Sub OnLevelStart()
		MyBase.StartCoroutine(Me.devilPattern_cr())
		Me.sittingDevil.StartDemons()
	End Sub

	' Token: 0x06000298 RID: 664 RVA: 0x0005C96C File Offset: 0x0005AD6C
	Protected Overrides Sub OnStateChanged()
		MyBase.OnStateChanged()
		If Me.properties.CurrentState.stateName = LevelProperties.Devil.States.Split Then
			Me.StopAllCoroutines()
		ElseIf Me.properties.CurrentState.stateName = LevelProperties.Devil.States.GiantHead Then
			Me.StopAllCoroutines()
			Me.sittingDevil.StartTransform()
			MyBase.StartCoroutine(Me.phase_1_end_trans())
			MyBase.StartCoroutine(Me.devilPattern_cr())
		ElseIf Me.properties.CurrentState.stateName = LevelProperties.Devil.States.Hands Then
			Me.StopAllCoroutines()
			Me.giantHead.StartHands()
		ElseIf Me.properties.CurrentState.stateName = LevelProperties.Devil.States.Tears Then
			Me.StopAllCoroutines()
			Me.giantHead.StartTears()
		End If
	End Sub

	' Token: 0x06000299 RID: 665 RVA: 0x0005CA39 File Offset: 0x0005AE39
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me._bossPortraitMain = Nothing
		Me._bossPortraitPhaseThree = Nothing
		Me._bossPortraitPhaseTwo = Nothing
	End Sub

	' Token: 0x0600029A RID: 666 RVA: 0x0005CA58 File Offset: 0x0005AE58
	Private Iterator Function phase_1_end_trans() As IEnumerator
		For Each devilLevelEffectSpawner As DevilLevelEffectSpawner In Me.smokeSpawners
			devilLevelEffectSpawner.KillSmoke()
		Next
		While Not DevilLevelHole.PHASE_1_COMPLETE
			Yield Nothing
		End While
		Me.groundHandler.SetActive(False)
		Dim startZoomout As Boolean = False
		Dim t As Single = 0F
		Dim cameraSlideUpTime As Single = 1F
		Dim time As Single = 3.3F
		Dim endCameraTime As Single = 2F
		Dim phase1Start As Vector3 = Me.phase1Scroll.transform.position
		Dim phase1End As Vector3 = Vector3.zero
		Dim cameraStart As Vector3 = CupheadLevelCamera.Current.transform.position
		Dim cameraEffectEnd As Vector3 = New Vector3(CupheadLevelCamera.Current.transform.position.x, 50F)
		Dim cameraOffsetEnd As Vector3 = New Vector3(CupheadLevelCamera.Current.transform.position.x, 600F)
		For Each parallaxLayer As ParallaxLayer In Me.parallax
			parallaxLayer.enabled = False
		Next
		Me.sittingDevil.RemoveFire()
		Yield MyBase.StartCoroutine(CupheadLevelCamera.Current.slide_camera_cr(cameraEffectEnd, cameraSlideUpTime))
		MyBase.StartCoroutine(CupheadLevelCamera.Current.slide_camera_cr(cameraOffsetEnd, time))
		While t < time
			If t >= 2F AndAlso Not startZoomout Then
				Me.ZoomOut(cameraStart, endCameraTime)
				startZoomout = True
			End If
			t += CupheadTime.Delta
			Dim val As Single = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0F, 1F, t / time)
			Me.phase1Scroll.transform.position = Vector3.Lerp(phase1Start, phase1End, val)
			Dim c As Color = Me.phase1Foreground.color
			c.a = Mathf.Clamp(1F - t * 2F, 0F, 1F)
			Me.phase1Foreground.color = c
			Yield Nothing
		End While
		Me.phase1Scroll.transform.position = phase1End
		Me.giantHead.transform.parent = Nothing
		Me.giantHead.StartIntroTransform()
		MyBase.StartCoroutine(Me.phase2BackgroundFade_cr(2F))
		AudioManager.FadeBGMVolume(0F, 0.5F, True)
		AudioManager.PlayBGMPlaylistManually(False)
		AudioManager.Play("transition_sting")
		Yield CupheadTime.WaitForSeconds(Me, endCameraTime)
		Me.phase1Scroll.gameObject.SetActive(False)
		Global.UnityEngine.[Object].Destroy(Me.sittingDevil.gameObject)
		Yield Nothing
		Return
	End Function

	' Token: 0x0600029B RID: 667 RVA: 0x0005CA74 File Offset: 0x0005AE74
	Private Iterator Function phase2BackgroundFade_cr(time As Single) As IEnumerator
		Dim sprites As SpriteRenderer() = Me.phase2Background.GetComponentsInChildren(Of SpriteRenderer)()
		For i As Integer = 0 To sprites.Length - 1
			Dim color As Color = sprites(i).color
			color.a = 0F
			sprites(i).color = color
		Next
		Me.phase2Background.SetActive(True)
		Dim t As Single = 0F
		While t < time
			For j As Integer = 0 To sprites.Length - 1
				Dim color2 As Color = sprites(j).color
				color2.a = t / time
				sprites(j).color = color2
			Next
			t += CupheadTime.Delta
			Yield Nothing
		End While
		For k As Integer = 0 To sprites.Length - 1
			Dim color3 As Color = sprites(k).color
			color3.a = 1F
			sprites(k).color = color3
		Next
		Return
	End Function

	' Token: 0x0600029C RID: 668 RVA: 0x0005CA98 File Offset: 0x0005AE98
	Private Sub ZoomOut(cameraStart As Vector3, endCameraTime As Single)
		AudioManager.FadeBGMVolume(0F, 1F, True)
		Level.Current.SetBounds(New Integer?(932), New Integer?(932), New Integer?(460), New Integer?(306))
		MyBase.StartCoroutine(CupheadLevelCamera.Current.change_zoom_cr(0.811F, 10F))
		MyBase.StartCoroutine(CupheadLevelCamera.Current.slide_camera_cr(cameraStart, endCameraTime))
		Me.phase3Platforms.SetActive(True)
		Dim player As AbstractPlayerController = PlayerManager.GetPlayer(PlayerId.PlayerOne)
		player.transform.SetScale(New Single?(1F), Nothing, Nothing)
		player.transform.position = Me.Phase2P1spawn.position
		player.GetComponent(Of LevelPlayerMotor)().ForceLooking(New Trilean2(1, 0))
		If player.stats.isChalice Then
			player.GetComponent(Of LevelPlayerMotor)().DashComplete()
			player.GetComponent(Of LevelPlayerAnimationController)().ScaredChalice(False)
		Else
			player.GetComponent(Of LevelPlayerAnimationController)().PlayIntro()
		End If
		Dim player2 As AbstractPlayerController = PlayerManager.GetPlayer(PlayerId.PlayerTwo)
		If player2 IsNot Nothing Then
			player2.transform.SetScale(New Single?(1F), Nothing, Nothing)
			player2.transform.position = Me.Phase2P2spawn.position
			player2.GetComponent(Of LevelPlayerMotor)().ForceLooking(New Trilean2(1, 0))
			If player2.stats.isChalice Then
				player2.GetComponent(Of LevelPlayerMotor)().DashComplete()
				player2.GetComponent(Of LevelPlayerAnimationController)().ScaredChalice(False)
			Else
				player2.GetComponent(Of LevelPlayerAnimationController)().PlayIntro()
			End If
		End If
		MyBase.StartCoroutine(Me.disable_input_cr())
		Me.pit.SetActive(True)
	End Sub

	' Token: 0x0600029D RID: 669 RVA: 0x0005CC60 File Offset: 0x0005B060
	Private Iterator Function disable_input_cr() As IEnumerator
		Dim player As AbstractPlayerController = PlayerManager.GetPlayer(PlayerId.PlayerOne)
		Dim motorP As LevelPlayerMotor = player.GetComponent(Of LevelPlayerMotor)()
		Dim weaponManagerP As LevelPlayerWeaponManager = player.GetComponent(Of LevelPlayerWeaponManager)()
		motorP.DisableInput()
		If player.stats.isChalice Then
			motorP.ForceLooking(New Trilean2(0, 0))
		End If
		weaponManagerP.DisableInput()
		Dim player2 As AbstractPlayerController = PlayerManager.GetPlayer(PlayerId.PlayerTwo)
		If player2 IsNot Nothing Then
			player2.GetComponent(Of LevelPlayerMotor)().DisableInput()
			If player2.stats.isChalice Then
				player2.GetComponent(Of LevelPlayerMotor)().ForceLooking(New Trilean2(0, 0))
			End If
			player2.GetComponent(Of LevelPlayerWeaponManager)().DisableInput()
		End If
		If Not player.GetComponent(Of LevelPlayerController)().IsDead Then
			Yield motorP.animator.WaitForAnimationToEnd(Me, If((Not player.stats.isChalice), "Intro_Scared", "Intro_Chalice_Scared"), If((Not player.stats.isChalice), 0, 3), False, True)
		ElseIf player2 IsNot Nothing AndAlso Not player2.GetComponent(Of LevelPlayerController)().IsDead Then
			Yield player2.GetComponent(Of LevelPlayerMotor)().animator.WaitForAnimationToEnd(Me, If((Not player2.stats.isChalice), "Intro_Scared", "Intro_Chalice_Scared"), If((Not player2.stats.isChalice), 0, 3), False, True)
		End If
		motorP.ClearBufferedInput()
		motorP.EnableInput()
		weaponManagerP.EnableInput()
		If player.stats.isChalice Then
			motorP.ForceLooking(New Trilean2(1, 0))
		End If
		player.GetComponent(Of LevelPlayerAnimationController)().ResetMoveX()
		player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo)
		If player2 IsNot Nothing Then
			player2.GetComponent(Of LevelPlayerMotor)().ClearBufferedInput()
			player2.GetComponent(Of LevelPlayerMotor)().EnableInput()
			player2.GetComponent(Of LevelPlayerWeaponManager)().EnableInput()
			player2.GetComponent(Of LevelPlayerAnimationController)().ResetMoveX()
			If player2.stats.isChalice Then
				player2.GetComponent(Of LevelPlayerMotor)().ForceLooking(New Trilean2(1, 0))
			End If
		End If
		Yield Nothing
		Return
	End Function

	' Token: 0x0600029E RID: 670 RVA: 0x0005CC7C File Offset: 0x0005B07C
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		Gizmos.color = Color.red
		Gizmos.DrawSphere(Me.Phase2P1spawn.position, 30F)
		Gizmos.color = Color.blue
		Gizmos.DrawSphere(Me.Phase2P2spawn.position, 30F)
	End Sub

	' Token: 0x0600029F RID: 671 RVA: 0x0005CCD0 File Offset: 0x0005B0D0
	Private Iterator Function devilPattern_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		While True
			Yield MyBase.StartCoroutine(Me.nextPattern_cr())
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060002A0 RID: 672 RVA: 0x0005CCEC File Offset: 0x0005B0EC
	Private Iterator Function nextPattern_cr() As IEnumerator
		Select Case Me.properties.CurrentState.NextPattern
			Case LevelProperties.Devil.Pattern.Clap
				Yield MyBase.StartCoroutine(Me.clap_cr())
			Case LevelProperties.Devil.Pattern.Head
				Yield MyBase.StartCoroutine(Me.head_cr())
			Case LevelProperties.Devil.Pattern.Pitchfork
				Yield MyBase.StartCoroutine(Me.pitchfork_cr())
			Case LevelProperties.Devil.Pattern.BombEye
				Yield MyBase.StartCoroutine(Me.bombEye_cr())
			Case LevelProperties.Devil.Pattern.SkullEye
				Yield MyBase.StartCoroutine(Me.skullEye_cr())
			Case Else
				Yield CupheadTime.WaitForSeconds(Me, 1F)
		End Select
		Return
	End Function

	' Token: 0x060002A1 RID: 673 RVA: 0x0005CD08 File Offset: 0x0005B108
	Private Iterator Function clap_cr() As IEnumerator
		While Me.sittingDevil.state <> DevilLevelSittingDevil.State.Idle
			Yield Nothing
		End While
		Me.sittingDevil.StartClap()
		While Me.sittingDevil.state <> DevilLevelSittingDevil.State.Idle
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060002A2 RID: 674 RVA: 0x0005CD24 File Offset: 0x0005B124
	Private Iterator Function head_cr() As IEnumerator
		While Me.sittingDevil.state <> DevilLevelSittingDevil.State.Idle
			Yield Nothing
		End While
		Me.sittingDevil.StartHead()
		While Me.sittingDevil.state <> DevilLevelSittingDevil.State.Idle
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060002A3 RID: 675 RVA: 0x0005CD40 File Offset: 0x0005B140
	Private Iterator Function pitchfork_cr() As IEnumerator
		While Me.sittingDevil.state <> DevilLevelSittingDevil.State.Idle
			Yield Nothing
		End While
		Me.sittingDevil.StartPitchfork()
		While Me.sittingDevil.state <> DevilLevelSittingDevil.State.Idle
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060002A4 RID: 676 RVA: 0x0005CD5C File Offset: 0x0005B15C
	Private Iterator Function bombEye_cr() As IEnumerator
		While Me.giantHead.state <> DevilLevelGiantHead.State.Idle
			Yield Nothing
		End While
		Me.giantHead.StartBombEye()
		While Me.giantHead.state <> DevilLevelGiantHead.State.Idle
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060002A5 RID: 677 RVA: 0x0005CD78 File Offset: 0x0005B178
	Private Iterator Function skullEye_cr() As IEnumerator
		While Me.giantHead.state <> DevilLevelGiantHead.State.Idle
			Yield Nothing
		End While
		Me.giantHead.StartSkullEye()
		While Me.giantHead.state <> DevilLevelGiantHead.State.Idle
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x040004ED RID: 1261
	Private properties As LevelProperties.Devil

	' Token: 0x040004EE RID: 1262
	Private Const Phase2FadeInTime As Single = 2F

	' Token: 0x040004EF RID: 1263
	<Header("Boss Info")>
	<SerializeField()>
	Private _bossPortraitMain As Sprite

	' Token: 0x040004F0 RID: 1264
	<SerializeField()>
	Private _bossPortraitPhaseTwo As Sprite

	' Token: 0x040004F1 RID: 1265
	<SerializeField()>
	Private _bossPortraitPhaseThree As Sprite

	' Token: 0x040004F2 RID: 1266
	<SerializeField()>
	Private _bossQuoteMain As String

	' Token: 0x040004F3 RID: 1267
	<SerializeField()>
	Private _bossQuotePhaseTwo As String

	' Token: 0x040004F4 RID: 1268
	<SerializeField()>
	Private _bossQuotePhaseThree As String

	' Token: 0x040004F5 RID: 1269
	<SerializeField()>
	Private groundHandler As GameObject

	' Token: 0x040004F6 RID: 1270
	<SerializeField()>
	Private parallax As ParallaxLayer()

	' Token: 0x040004F7 RID: 1271
	<SerializeField()>
	Private pit As GameObject

	' Token: 0x040004F8 RID: 1272
	<SerializeField()>
	Private middlePiece As GameObject

	' Token: 0x040004F9 RID: 1273
	<SerializeField()>
	Private phase1Scroll As Transform

	' Token: 0x040004FA RID: 1274
	<SerializeField()>
	Private phase1Foreground As SpriteRenderer

	' Token: 0x040004FB RID: 1275
	<SerializeField()>
	Private phase2Background As GameObject

	' Token: 0x040004FC RID: 1276
	<SerializeField()>
	Private phase3Platforms As GameObject

	' Token: 0x040004FD RID: 1277
	<SerializeField()>
	Private phase1Fade As SpriteRenderer

	' Token: 0x040004FE RID: 1278
	<SerializeField()>
	Private sittingDevil As DevilLevelSittingDevil

	' Token: 0x040004FF RID: 1279
	<SerializeField()>
	Private giantHead As DevilLevelGiantHead

	' Token: 0x04000500 RID: 1280
	<SerializeField()>
	Private smokeSpawners As DevilLevelEffectSpawner()

	' Token: 0x04000501 RID: 1281
	<SerializeField()>
	Private Phase2P1spawn As Transform

	' Token: 0x04000502 RID: 1282
	<SerializeField()>
	Private Phase2P2spawn As Transform
End Class
