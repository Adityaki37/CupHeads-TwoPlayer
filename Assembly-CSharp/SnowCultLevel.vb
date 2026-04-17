Imports System
Imports System.Collections
Imports System.Diagnostics
Imports UnityEngine

' Token: 0x020002C2 RID: 706
Public Class SnowCultLevel
	Inherits Level

	' Token: 0x060007BA RID: 1978 RVA: 0x00076208 File Offset: 0x00074608
	Protected Overrides Sub PartialInit()
		Me.properties = LevelProperties.SnowCult.GetMode(MyBase.mode)
		AddHandler Me.properties.OnStateChange, AddressOf MyBase.zHack_OnStateChanged
		AddHandler Me.properties.OnBossDeath, AddressOf MyBase.zHack_OnWin
		MyBase.timeline = Me.properties.CreateTimeline(MyBase.mode)
		Me.goalTimes = Me.properties.goalTimes
		AddHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		MyBase.PartialInit()
	End Sub

	' Token: 0x17000140 RID: 320
	' (get) Token: 0x060007BB RID: 1979 RVA: 0x0007629E File Offset: 0x0007469E
	Public Overrides ReadOnly Property CurrentLevel As Levels
		Get
			Return Levels.SnowCult
		End Get
	End Property

	' Token: 0x17000141 RID: 321
	' (get) Token: 0x060007BC RID: 1980 RVA: 0x000762A5 File Offset: 0x000746A5
	Public Overrides ReadOnly Property CurrentScene As Scenes
		Get
			Return Scenes.scene_level_snow_cult
		End Get
	End Property

	' Token: 0x14000006 RID: 6
	' (add) Token: 0x060007BD RID: 1981 RVA: 0x000762AC File Offset: 0x000746AC
	' (remove) Token: 0x060007BE RID: 1982 RVA: 0x000762E4 File Offset: 0x000746E4
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnYetiHitGround As Action

	' Token: 0x17000142 RID: 322
	' (get) Token: 0x060007BF RID: 1983 RVA: 0x0007631C File Offset: 0x0007471C
	Public Overrides ReadOnly Property BossPortrait As Sprite
		Get
			Select Case Me.properties.CurrentState.stateName
				Case LevelProperties.SnowCult.States.Main
					Return Me._bossPortraitMain
				Case LevelProperties.SnowCult.States.JackFrost
					Return Me._bossPortraitPhaseThree
				Case LevelProperties.SnowCult.States.Yeti, LevelProperties.SnowCult.States.EasyYeti
					Return Me._bossPortraitPhaseTwo
			End Select
			Global.Debug.LogError("Couldn't find portrait for state " + Me.properties.CurrentState.stateName + ". Using Main.", Nothing)
			Return Me._bossPortraitMain
		End Get
	End Property

	' Token: 0x17000143 RID: 323
	' (get) Token: 0x060007C0 RID: 1984 RVA: 0x000763A0 File Offset: 0x000747A0
	Public Overrides ReadOnly Property BossQuote As String
		Get
			Select Case Me.properties.CurrentState.stateName
				Case LevelProperties.SnowCult.States.Main
					Return Me._bossQuoteMain
				Case LevelProperties.SnowCult.States.JackFrost
					Return Me._bossQuotePhaseThree
				Case LevelProperties.SnowCult.States.Yeti, LevelProperties.SnowCult.States.EasyYeti
					Return Me._bossQuotePhaseTwo
			End Select
			Global.Debug.LogError("Couldn't find quote for state " + Me.properties.CurrentState.stateName + ". Using Main.", Nothing)
			Return Me._bossQuoteMain
		End Get
	End Property

	' Token: 0x060007C1 RID: 1985 RVA: 0x00076422 File Offset: 0x00074822
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me._bossPortraitMain = Nothing
		Me._bossPortraitPhaseTwo = Nothing
		Me._bossPortraitPhaseThree = Nothing
	End Sub

	' Token: 0x060007C2 RID: 1986 RVA: 0x0007643F File Offset: 0x0007483F
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.yeti.LevelInit(Me.properties)
		Me.jackFrost.LevelInit(Me.properties)
		Me.wizard.LevelInit(Me.properties)
	End Sub

	' Token: 0x060007C3 RID: 1987 RVA: 0x0007647A File Offset: 0x0007487A
	Protected Overrides Sub OnLevelStart()
		MyBase.StartCoroutine(Me.snowcultPattern_cr())
	End Sub

	' Token: 0x060007C4 RID: 1988 RVA: 0x0007648C File Offset: 0x0007488C
	Private Iterator Function snowcultPattern_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		While True
			Yield MyBase.StartCoroutine(Me.nextPattern_cr())
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060007C5 RID: 1989 RVA: 0x000764A8 File Offset: 0x000748A8
	Protected Overrides Sub OnStateChanged()
		MyBase.OnStateChanged()
		If Me.properties.CurrentState.stateName = LevelProperties.SnowCult.States.Yeti Then
			MyBase.StartCoroutine(Me.to_phase_2_cr())
		ElseIf Me.properties.CurrentState.stateName = LevelProperties.SnowCult.States.JackFrost Then
			MyBase.StartCoroutine(Me.to_phase_3_cr())
		ElseIf Me.properties.CurrentState.stateName = LevelProperties.SnowCult.States.EasyYeti Then
			MyBase.StartCoroutine(Me.to_phase_3_easy_cr())
		End If
	End Sub

	' Token: 0x060007C6 RID: 1990 RVA: 0x00076530 File Offset: 0x00074930
	Private Iterator Function nextPattern_cr() As IEnumerator
		While Me.wizard IsNot Nothing AndAlso (Me.wizard.Turning() OrElse Me.wizard.dead)
			Yield Nothing
		End While
		Dim p As LevelProperties.SnowCult.Pattern = Me.properties.CurrentState.NextPattern
		If Me.firstAttack Then
			While p <> LevelProperties.SnowCult.Pattern.Quad
				p = Me.properties.CurrentState.NextPattern
			End While
			Me.firstAttack = False
		End If
		Select Case p
			Case LevelProperties.SnowCult.Pattern.Switch
				Yield MyBase.StartCoroutine(Me.switch_cr())
				GoTo IL_02E6
			Case LevelProperties.SnowCult.Pattern.Eye
				Yield MyBase.StartCoroutine(Me.eye_attack_cr())
				GoTo IL_02E6
			Case LevelProperties.SnowCult.Pattern.Shard
				Yield MyBase.StartCoroutine(Me.shard_attack_cr())
				GoTo IL_02E6
			Case LevelProperties.SnowCult.Pattern.Mouth
				Yield MyBase.StartCoroutine(Me.mouth_shot_cr())
				GoTo IL_02E6
			Case LevelProperties.SnowCult.Pattern.Quad
				Yield MyBase.StartCoroutine(Me.quad_cr())
				GoTo IL_02E6
			Case LevelProperties.SnowCult.Pattern.Block
				Yield MyBase.StartCoroutine(Me.ice_block_cr())
				GoTo IL_02E6
			Case LevelProperties.SnowCult.Pattern.SeriesShot
				Yield MyBase.StartCoroutine(Me.series_shot_cr())
				GoTo IL_02E6
			Case LevelProperties.SnowCult.Pattern.Yeti
				GoTo IL_02E6
		End Select
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		IL_02E6:
		Return
	End Function

	' Token: 0x060007C7 RID: 1991 RVA: 0x0007654C File Offset: 0x0007494C
	Private Iterator Function to_phase_2_cr() As IEnumerator
		Me.firstAttack = False
		Me.wizard.ToOutro(Me.yeti)
		Yield Nothing
		Return
	End Function

	' Token: 0x060007C8 RID: 1992 RVA: 0x00076567 File Offset: 0x00074967
	Public Sub CultistsSummon()
		Me.cultists.SetTrigger("Summon")
	End Sub

	' Token: 0x060007C9 RID: 1993 RVA: 0x00076579 File Offset: 0x00074979
	Public Sub YetiHitGround()
		If Me.OnYetiHitGround IsNot Nothing Then
			Me.OnYetiHitGround()
		End If
		Me.cultists.SetTrigger("Summon")
	End Sub

	' Token: 0x060007CA RID: 1994 RVA: 0x000765A4 File Offset: 0x000749A4
	Private Iterator Function to_phase_3_easy_cr() As IEnumerator
		Me.yeti.ToEasyPhaseThree()
		Yield Nothing
		Return
	End Function

	' Token: 0x060007CB RID: 1995 RVA: 0x000765C0 File Offset: 0x000749C0
	Private Iterator Function to_phase_3_cr() As IEnumerator
		Me.yeti.ForceOutroToStart()
		While Me.yeti.state <> SnowCultLevelYeti.States.Idle OrElse Me.yeti.inBallForm
			Yield Nothing
		End While
		Me.cultists.SetTrigger("Summon")
		Me.yeti.OnDeath()
		Me.jackFrost.Intro()
		Yield CupheadTime.WaitForSeconds(Me, Me.properties.CurrentState.yeti.timeToPlatforms)
		Me.jackFrost.CreatePlatforms()
		MyBase.StartCoroutine(Me.SFX_SNOWCULT_IcePlatformAppear_cr())
		MyBase.StartCoroutine(Me.SFX_SNOWCULT_P2_to_P3_Transition_cr())
		For i As Integer = 0 To 5 - 1
			Me.jackFrost.CreateAscendingPlatform(i)
			If i < 4 Then
				Yield CupheadTime.WaitForSeconds(Me, 0.2F)
			End If
		Next
		Dim player As AbstractPlayerController = PlayerManager.GetPlayer(PlayerId.PlayerOne)
		Dim player2 As AbstractPlayerController = PlayerManager.GetPlayer(PlayerId.PlayerTwo)
		Dim p1Motor As LevelPlayerMotor = player.GetComponent(Of LevelPlayerMotor)()
		Dim p2Motor As LevelPlayerMotor = Nothing
		Dim hasStarted As Boolean = False
		While Not hasStarted
			If player2 IsNot Nothing AndAlso Not player2.IsDead Then
				If p2Motor Is Nothing Then
					p2Motor = player2.GetComponent(Of LevelPlayerMotor)()
				End If
				If(player.transform.position.y > -80F AndAlso p1Motor.Grounded) OrElse (player2.transform.position.y > -80F AndAlso p2Motor.Grounded) Then
					hasStarted = True
				End If
			ElseIf player.transform.position.y > -80F AndAlso p1Motor.Grounded Then
				hasStarted = True
			End If
			Yield Nothing
		End While
		Dim cameraEndPos As Vector3 = New Vector3(0F, 950F, 0F)
		Dim time As Single = Me.properties.CurrentState.yeti.timeForCameraMove
		CupheadLevelCamera.Current.ChangeVerticalBounds(1290, 675)
		Me.pit.SetActive(True)
		Dim cameraStartPos As Single = CupheadLevelCamera.Current.transform.position.y
		MyBase.StartCoroutine(CupheadLevelCamera.Current.slide_camera_cr(cameraEndPos, time))
		time = 0F
		While time < 0.5F
			time = Mathf.InverseLerp(cameraStartPos, cameraEndPos.y, CupheadLevelCamera.Current.transform.position.y)
			Yield Nothing
		End While
		Level.Current.SetBounds(New Integer?(640), New Integer?(640), New Integer?(1290), New Integer?(675))
		While time < 0.75F
			time = Mathf.InverseLerp(cameraStartPos, cameraEndPos.y, CupheadLevelCamera.Current.transform.position.y)
			Yield Nothing
		End While
		Me.jackFrost.StartPhase3()
		Me.pit.transform.parent = Nothing
		While time < 0.95F
			time = Mathf.InverseLerp(cameraStartPos, cameraEndPos.y, CupheadLevelCamera.Current.transform.position.y)
			Me.pit.transform.localPosition = CupheadLevelCamera.Current.transform.position + Vector3.down * 500F
			Yield Nothing
		End While
		Me.pit.transform.localPosition = cameraEndPos + Vector3.down * 500F
		Return
	End Function

	' Token: 0x060007CC RID: 1996 RVA: 0x000765DC File Offset: 0x000749DC
	Private Iterator Function quad_cr() As IEnumerator
		While Me.wizard.state <> SnowCultLevelWizard.States.Idle
			Yield Nothing
		End While
		Me.wizard.StartQuadAttack()
		While Me.wizard.state <> SnowCultLevelWizard.States.Idle
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060007CD RID: 1997 RVA: 0x000765F8 File Offset: 0x000749F8
	Private Iterator Function ice_block_cr() As IEnumerator
		While Me.wizard.state <> SnowCultLevelWizard.States.Idle
			Yield Nothing
		End While
		Me.wizard.Whale()
		While Me.wizard.state <> SnowCultLevelWizard.States.Idle
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060007CE RID: 1998 RVA: 0x00076614 File Offset: 0x00074A14
	Private Iterator Function series_shot_cr() As IEnumerator
		While Me.wizard.state <> SnowCultLevelWizard.States.Idle
			Yield Nothing
		End While
		Me.wizard.SeriesShot()
		While Me.wizard.state <> SnowCultLevelWizard.States.Idle
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060007CF RID: 1999 RVA: 0x00076630 File Offset: 0x00074A30
	Private Iterator Function switch_cr() As IEnumerator
		While Me.jackFrost.state <> SnowCultLevelJackFrost.States.Idle
			Yield Nothing
		End While
		Me.jackFrost.StartSwitch()
		While Me.jackFrost.state <> SnowCultLevelJackFrost.States.Idle
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060007D0 RID: 2000 RVA: 0x0007664C File Offset: 0x00074A4C
	Private Iterator Function eye_attack_cr() As IEnumerator
		While Me.jackFrost.state <> SnowCultLevelJackFrost.States.Idle
			Yield Nothing
		End While
		Me.jackFrost.StartEyeAttack()
		While Me.jackFrost.state <> SnowCultLevelJackFrost.States.Idle
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060007D1 RID: 2001 RVA: 0x00076668 File Offset: 0x00074A68
	Private Iterator Function shard_attack_cr() As IEnumerator
		While Me.jackFrost.state <> SnowCultLevelJackFrost.States.Idle
			Yield Nothing
		End While
		Me.jackFrost.StartShardAttack()
		While Me.jackFrost.state <> SnowCultLevelJackFrost.States.Idle
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060007D2 RID: 2002 RVA: 0x00076684 File Offset: 0x00074A84
	Private Iterator Function mouth_shot_cr() As IEnumerator
		While Me.jackFrost.state <> SnowCultLevelJackFrost.States.Idle
			Yield Nothing
		End While
		Me.jackFrost.StartMouthShot()
		While Me.jackFrost.state <> SnowCultLevelJackFrost.States.Idle
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060007D3 RID: 2003 RVA: 0x000766A0 File Offset: 0x00074AA0
	Private Iterator Function SFX_SNOWCULT_IcePlatformAppear_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 0.1F)
		AudioManager.Play("sfx_dlc_snowcult_p2_iceplatform_appear")
		Me.emitAudioFromObject.Add("sfx_dlc_snowcult_p2_iceplatform_appear")
		Return
	End Function

	' Token: 0x060007D4 RID: 2004 RVA: 0x000766BC File Offset: 0x00074ABC
	Private Iterator Function SFX_SNOWCULT_P2_to_P3_Transition_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 3F)
		AudioManager.Play("sfx_dlc_snowcult_p2_snow_cultists_wave_hands_transition")
		Return
	End Function

	' Token: 0x0400100A RID: 4106
	Private properties As LevelProperties.SnowCult

	' Token: 0x0400100B RID: 4107
	Private Const CLIMBING_PLATFORMS_INTERAPPEAR_DELAY As Single = 0.2F

	' Token: 0x0400100C RID: 4108
	Private Const HEIGHT_TO_START_PHASE_THREE As Single = -80F

	' Token: 0x0400100D RID: 4109
	Private Const PHASE_THREE_CAMERA_POS As Single = 950F

	' Token: 0x0400100F RID: 4111
	<SerializeField()>
	Private wizard As SnowCultLevelWizard

	' Token: 0x04001010 RID: 4112
	<SerializeField()>
	Private yeti As SnowCultLevelYeti

	' Token: 0x04001011 RID: 4113
	<SerializeField()>
	Private jackFrost As SnowCultLevelJackFrost

	' Token: 0x04001012 RID: 4114
	<SerializeField()>
	Private cultists As Animator

	' Token: 0x04001013 RID: 4115
	<SerializeField()>
	Private pit As GameObject

	' Token: 0x04001014 RID: 4116
	<Header("Boss Info")>
	<SerializeField()>
	Private _bossPortraitMain As Sprite

	' Token: 0x04001015 RID: 4117
	<SerializeField()>
	Private _bossPortraitPhaseTwo As Sprite

	' Token: 0x04001016 RID: 4118
	<SerializeField()>
	Private _bossPortraitPhaseThree As Sprite

	' Token: 0x04001017 RID: 4119
	<SerializeField()>
	Private _bossQuoteMain As String

	' Token: 0x04001018 RID: 4120
	<SerializeField()>
	Private _bossQuotePhaseTwo As String

	' Token: 0x04001019 RID: 4121
	<SerializeField()>
	Private _bossQuotePhaseThree As String

	' Token: 0x0400101A RID: 4122
	Private firstAttack As Boolean = True
End Class
