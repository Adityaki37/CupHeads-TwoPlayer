Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000706 RID: 1798
Public Class OldManLevelOldMan
	Inherits LevelProperties.OldMan.Entity

	' Token: 0x170003CC RID: 972
	' (get) Token: 0x060026A6 RID: 9894 RVA: 0x00169BE6 File Offset: 0x00167FE6
	' (set) Token: 0x060026A7 RID: 9895 RVA: 0x00169BEE File Offset: 0x00167FEE
	Public Property state As OldManLevelOldMan.State

	' Token: 0x060026A8 RID: 9896 RVA: 0x00169BF7 File Offset: 0x00167FF7
	Private Sub Start()
		Me.sprite = MyBase.GetComponent(Of SpriteRenderer)()
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
	End Sub

	' Token: 0x060026A9 RID: 9897 RVA: 0x00169C33 File Offset: 0x00168033
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x060026AA RID: 9898 RVA: 0x00169C4B File Offset: 0x0016804B
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		MyBase.properties.DealDamage(info.damage)
	End Sub

	' Token: 0x060026AB RID: 9899 RVA: 0x00169C60 File Offset: 0x00168060
	Public Overrides Sub LevelInit(properties As LevelProperties.OldMan)
		MyBase.LevelInit(properties)
		Dim spitAttack As LevelProperties.OldMan.SpitAttack = properties.CurrentState.spitAttack
		Me.spitStringMainIndex = Global.UnityEngine.Random.Range(0, spitAttack.spitString.Length)
		Me.gooseSpawnString = New PatternString(properties.CurrentState.gooseAttack.gooseSpawnString, ":"c, True, True)
		MyBase.StartCoroutine(Me.intro_cr())
	End Sub

	' Token: 0x060026AC RID: 9900 RVA: 0x00169CC0 File Offset: 0x001680C0
	Private Iterator Function intro_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 3F)
		Me.state = OldManLevelOldMan.State.Idle
		Return
	End Function

	' Token: 0x060026AD RID: 9901 RVA: 0x00169CDB File Offset: 0x001680DB
	Private Sub AniEvent_CameraRumble()
		CupheadLevelCamera.Current.Shake(5F, 0.66F, False)
	End Sub

	' Token: 0x060026AE RID: 9902 RVA: 0x00169CF2 File Offset: 0x001680F2
	Private Sub AniEvent_CameraShake()
		CupheadLevelCamera.Current.Shake(30F, 1.2F, False)
	End Sub

	' Token: 0x060026AF RID: 9903 RVA: 0x00169D09 File Offset: 0x00168109
	Private Sub ChangeColor(color As Color)
		Me.sprite.color = color
	End Sub

	' Token: 0x060026B0 RID: 9904 RVA: 0x00169D17 File Offset: 0x00168117
	Public Sub Spit()
		MyBase.StartCoroutine(Me.spit_cr())
	End Sub

	' Token: 0x060026B1 RID: 9905 RVA: 0x00169D28 File Offset: 0x00168128
	Private Iterator Function spit_cr() As IEnumerator
		AudioManager.FadeSFXVolume("sfx_dlc_omm_mouthcauldron_stirring_loop_start", 0F, 0.0001F)
		Me.state = OldManLevelOldMan.State.Spit
		MyBase.animator.SetBool(OldManLevelOldMan.IsSpitAttackParameterID, True)
		Dim p As LevelProperties.OldMan.SpitAttack = MyBase.properties.CurrentState.spitAttack
		Dim spitString As String() = p.spitString(Me.spitStringMainIndex).Split(New Char() { ","c })
		Dim spitParryString As PatternString = New PatternString(p.spitParryString, True)
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Spit_Intro_Continued", False, True)
		Dim height As Single = p.spitApexHeight
		Dim apexTime2 As Single = p.spitApexTime * p.spitApexTime
		Dim g As Single = -2F * height / apexTime2
		Dim viX As Single = 2F * height / p.spitApexTime
		Dim viY2 As Single = viX * viX
		Dim endPosX As Single = Me.spitEndArc.position.x
		Dim endPosY As Single = 0F
		Dim startPosition As Vector3 = Vector3.zero
		Dim endPosition As Vector3 = Vector3.zero
		For i As Integer = 0 To spitString.Length - 1
			If Me.endPhaseOne Then
				Exit For
			End If
			Parser.FloatTryParse(spitString(i), endPosY)
			startPosition = Me.spitRoot.transform.position + Vector3.right * CSng(Global.UnityEngine.Random.Range(-15, 15))
			endPosition = New Vector3(endPosX, endPosY)
			Dim x As Single = endPosition.x - startPosition.x
			Dim y As Single = endPosition.y - startPosition.y
			Dim sqrtRooted As Single = viY2 + 2F * g * x
			Dim tEnd As Single = (-viX + Mathf.Sqrt(sqrtRooted)) / g
			Dim tEnd2 As Single = (-viX - Mathf.Sqrt(sqrtRooted)) / g
			Dim tEnd3 As Single = Mathf.Max(tEnd, tEnd2)
			Dim velocityY As Single = y / tEnd3
			Dim projectile As OldManLevelSpitProjectile = If((spitParryString.PopLetter() <> "P"c), TryCast(Me.spitProjectile.Create(), OldManLevelSpitProjectile), TryCast(Me.spitProjectilePink.Create(), OldManLevelSpitProjectile))
			projectile.Move(startPosition, viX, velocityY, Me.spitEndArc.position.x, g, p.spitApexTime, i Mod 4)
			If Not Me.endPhaseOne Then
				Yield CupheadTime.WaitForSeconds(Me, p.spitDelay)
			End If
		Next
		MyBase.animator.SetBool(OldManLevelOldMan.IsSpitAttackParameterID, False)
		Me.spitStringMainIndex = (Me.spitStringMainIndex + 1) Mod p.spitString.Length
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Spit_Outro", False, True)
		Dim t As Single = 0F
		While t < p.attackCooldown AndAlso Not Me.endPhaseOne
			t += CupheadTime.Delta
			Yield Nothing
		End While
		Me.state = OldManLevelOldMan.State.Idle
		Yield Nothing
		Return
	End Function

	' Token: 0x060026B2 RID: 9906 RVA: 0x00169D43 File Offset: 0x00168143
	Public Sub Goose()
		MyBase.StartCoroutine(Me.Goose_cr())
	End Sub

	' Token: 0x060026B3 RID: 9907 RVA: 0x00169D54 File Offset: 0x00168154
	Private Iterator Function Goose_cr() As IEnumerator
		Dim p As LevelProperties.OldMan.GooseAttack = MyBase.properties.CurrentState.gooseAttack
		Me.state = OldManLevelOldMan.State.Goose
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		MyBase.animator.SetBool(OldManLevelOldMan.IsGooseAttackParameterID, True)
		Dim targetAnimHash As Integer = Animator.StringToHash(MyBase.animator.GetLayerName(0) + ".Goose_Atk_Anti")
		Dim idleOneAnimHash As Integer = Animator.StringToHash(MyBase.animator.GetLayerName(0) + ".Idle_Part_1")
		Dim idleTwoAnimHash As Integer = Animator.StringToHash(MyBase.animator.GetLayerName(0) + ".Idle_Part_2")
		While MyBase.animator.GetCurrentAnimatorStateInfo(0).fullPathHash <> targetAnimHash AndAlso Not Me.endPhaseOne
			Yield Nothing
		End While
		If Me.endPhaseOne AndAlso (MyBase.animator.GetCurrentAnimatorStateInfo(0).fullPathHash = idleOneAnimHash OrElse MyBase.animator.GetCurrentAnimatorStateInfo(0).fullPathHash = idleTwoAnimHash) Then
			MyBase.animator.SetBool(OldManLevelOldMan.IsGooseAttackParameterID, False)
			Return
		End If
		Dim t As Single = 0F
		While t < p.goosePreAntic AndAlso Not Me.endPhaseOne
			t += CupheadTime.Delta
			Yield Nothing
		End While
		MyBase.animator.SetTrigger(OldManLevelOldMan.ContinueParameterID)
		t = 0F
		While t < p.gooseWarning AndAlso Not Me.endPhaseOne
			t += CupheadTime.Delta
			Yield Nothing
		End While
		Dim spawningGeese As Boolean = True
		Dim geeseDurationTimer As Single = 0F
		Dim geeseDelayTimer As Single = 0F
		Dim geeseDelayMaxTime As Single = Me.gooseSpawnString.GetSubsubstringFloat(0)
		Dim xPos As Single = 840F
		Dim speed As Single = 0F
		While spawningGeese AndAlso Not Me.endPhaseOne
			geeseDurationTimer += CupheadTime.FixedDelta
			geeseDelayTimer += CupheadTime.FixedDelta
			If geeseDelayTimer >= geeseDelayMaxTime Then
				Dim oldManLevelGoose As OldManLevelGoose = TryCast(Me.goosePrefab.Create(), OldManLevelGoose)
				Dim flag As Boolean = False
				Dim text As String = "Default"
				Dim num As Integer = 100
				Dim num2 As Single = 0F
				Dim subsubstringLetter As Char = Me.gooseSpawnString.GetSubsubstringLetter(1)
				Select Case subsubstringLetter
					Case "B"c
						oldManLevelGoose.transform.localScale = New Vector3(0.655F, 0.655F)
						speed = p.gooseBSpeed
						text = "Background"
						num = 1000
						num2 = 0.15F
					Case "C"c
						speed = p.gooseCSpeed
						flag = True
					Case Else
						If subsubstringLetter = "M"c Then
							oldManLevelGoose.transform.localScale = New Vector3(0.848F, 0.848F)
							speed = p.gooseMSpeed
							flag = True
							num = -100
							num2 = 0.05F
						End If
					Case "F"c
						oldManLevelGoose.transform.localScale = New Vector3(1.4414F, 1.4414F)
						speed = p.gooseFSpeed
						text = "Foreground"
						num2 = 0.85F
				End Select
				Dim vector As Vector3 = New Vector3(xPos, Me.gooseSpawnString.GetSubsubstringFloat(2))
				oldManLevelGoose.Init(vector, speed, p, flag, text, num, num2)
				geeseDelayTimer = 0F
				geeseDelayMaxTime = Me.gooseSpawnString.GetSubsubstringFloat(0)
				Me.gooseSpawnString.IncrementString()
			End If
			If geeseDurationTimer >= p.gooseDuration Then
				spawningGeese = False
			End If
			Yield wait
		End While
		MyBase.animator.SetBool(OldManLevelOldMan.IsGooseAttackParameterID, False)
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Goose_Atk_End", False, True)
		t = 0F
		While t < p.gooseCooldown AndAlso Not Me.endPhaseOne
			t += CupheadTime.Delta
			Yield Nothing
		End While
		Me.state = OldManLevelOldMan.State.Idle
		Yield Nothing
		Return
	End Function

	' Token: 0x060026B4 RID: 9908 RVA: 0x00169D6F File Offset: 0x0016816F
	Public Sub Bear()
		MyBase.StartCoroutine(Me.bear_cr())
	End Sub

	' Token: 0x060026B5 RID: 9909 RVA: 0x00169D80 File Offset: 0x00168180
	Private Iterator Function bear_cr() As IEnumerator
		Dim p As LevelProperties.OldMan.CamelAttack = MyBase.properties.CurrentState.camelAttack
		Me.state = OldManLevelOldMan.State.Bear
		MyBase.animator.SetBool(OldManLevelOldMan.IsBearAttackParameterID, True)
		Dim targetAnimHash As Integer = Animator.StringToHash(MyBase.animator.GetLayerName(0) + ".Bear_Atk_Start")
		Dim targetAnimHashAlt As Integer = Animator.StringToHash(MyBase.animator.GetLayerName(0) + ".Bear_Atk_Start_F10")
		Dim idleOneAnimHash As Integer = Animator.StringToHash(MyBase.animator.GetLayerName(0) + ".Idle_Part_1")
		Dim idleTwoAnimHash As Integer = Animator.StringToHash(MyBase.animator.GetLayerName(0) + ".Idle_Part_2")
		While MyBase.animator.GetCurrentAnimatorStateInfo(0).fullPathHash <> targetAnimHash AndAlso MyBase.animator.GetCurrentAnimatorStateInfo(0).fullPathHash <> targetAnimHashAlt AndAlso Not Me.endPhaseOne
			Yield Nothing
		End While
		If Me.endPhaseOne AndAlso (MyBase.animator.GetCurrentAnimatorStateInfo(0).fullPathHash = idleOneAnimHash OrElse MyBase.animator.GetCurrentAnimatorStateInfo(0).fullPathHash = idleTwoAnimHash) Then
			MyBase.animator.SetBool(OldManLevelOldMan.IsBearAttackParameterID, False)
			Return
		End If
		Yield MyBase.animator.WaitForAnimationToStart(Me, "Bear_Atk_Anti", False)
		Dim bearAni As Animator = Me.bearBeam.GetComponent(Of Animator)()
		Me.bearBeam.gameObject.SetActive(True)
		Me.bearBeam.transform.position = New Vector3(-1300F, 100F)
		Me.bearBeam.thrown = False
		Dim t As Single = 0F
		While t < p.camelAttackWarning AndAlso Not Me.endPhaseOne
			t += CupheadTime.Delta
			Yield Nothing
		End While
		MyBase.animator.SetTrigger(OldManLevelOldMan.ContinueParameterID)
		t = 0F
		Yield MyBase.animator.WaitForNormalizedTime(Me, 1.2916666F, "Bear_Atk_Cont", 0, False, False, True)
		Yield CupheadTime.WaitForSeconds(Me, If((Not Me.endPhaseOne), p.camelOffScreenTime, 0.5F))
		Dim endPoint As Single = If((Not Me.endPhaseOne), p.endingPoint, (-990F))
		Dim exiting As Boolean = False
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		bearAni.Play("Idle")
		While Me.bearBeam.transform.position.x < endPoint AndAlso Not Me.bearBeam.thrown
			If(Me.endPhaseOne OrElse Me.bearBeam.transform.position.x > p.boredomPoint) AndAlso Not exiting Then
				exiting = True
				MyBase.StartCoroutine(Me.exit_bear_cr())
			End If
			Me.bearBeam.transform.position += Vector3.right * p.camelAttackSpeed * CupheadTime.FixedDelta
			Yield wait
		End While
		If Not exiting Then
			Yield MyBase.StartCoroutine(Me.exit_bear_cr())
		End If
		t = 0F
		While t < p.camelAttackCooldown AndAlso Not Me.endPhaseOne
			t += CupheadTime.Delta
			Yield Nothing
		End While
		Me.state = OldManLevelOldMan.State.Idle
		Yield Nothing
		Return
	End Function

	' Token: 0x060026B6 RID: 9910 RVA: 0x00169D9C File Offset: 0x0016819C
	Private Iterator Function exit_bear_cr() As IEnumerator
		Dim bearAni As Animator = Me.bearBeam.GetComponent(Of Animator)()
		MyBase.animator.SetBool(OldManLevelOldMan.IsBearAttackParameterID, False)
		Yield MyBase.animator.WaitForAnimationToStart(Me, "Bear_Atk_End_1", False)
		bearAni.SetTrigger("OnExit")
		Yield bearAni.WaitForAnimationToEnd(Me, "End", False, True)
		Me.bearBeam.StartCoroutine(Me.bearBeam.fall_cr())
		MyBase.animator.SetTrigger(OldManLevelOldMan.ContinueParameterID)
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Bear_Atk_End", False, False)
		Return
	End Function

	' Token: 0x060026B7 RID: 9911 RVA: 0x00169DB7 File Offset: 0x001681B7
	Public Sub EndPhase1()
		MyBase.animator.SetBool("Phase2", True)
		Me.endPhaseOne = True
	End Sub

	' Token: 0x060026B8 RID: 9912 RVA: 0x00169DD1 File Offset: 0x001681D1
	Private Sub AniEvent_EndPhase1BeardBoil()
		Me.rightWall.SetActive(False)
		MyBase.animator.Play("None", 1)
	End Sub

	' Token: 0x060026B9 RID: 9913 RVA: 0x00169DF0 File Offset: 0x001681F0
	Private Sub AniEvent_ActivatePhase2Beard()
		CType(Level.Current, OldManLevel).ActivatePhase2Beard()
		MyBase.GetComponent(Of SpriteRenderer)().sortingOrder = -1
	End Sub

	' Token: 0x060026BA RID: 9914 RVA: 0x00169E0D File Offset: 0x0016820D
	Public Sub OnPhase2()
		Me.sockPuppets.StartPhase2()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x060026BB RID: 9915 RVA: 0x00169E25 File Offset: 0x00168225
	Private Sub animationEvent_PlayGooseFX()
		Me.gooseFXAnimator.Play("FX")
	End Sub

	' Token: 0x060026BC RID: 9916 RVA: 0x00169E37 File Offset: 0x00168237
	Private Sub animationEvent_IdleBlinkStart()
		If Me.shouldIdleBlink Then
			Me.eyeRenderer.enabled = True
		End If
		Me.shouldIdleBlink = Not Me.shouldIdleBlink
	End Sub

	' Token: 0x060026BD RID: 9917 RVA: 0x00169E5F File Offset: 0x0016825F
	Private Sub animationEvent_IdleBlinkEnd()
		Me.eyeRenderer.enabled = False
	End Sub

	' Token: 0x060026BE RID: 9918 RVA: 0x00169E6D File Offset: 0x0016826D
	Private Sub animationEvent_BeginCauldron()
		Me.cauldron.SetActive(True)
	End Sub

	' Token: 0x060026BF RID: 9919 RVA: 0x00169E7B File Offset: 0x0016827B
	Private Sub animationEvent_EndCauldron()
		Me.cauldron.SetActive(False)
	End Sub

	' Token: 0x060026C0 RID: 9920 RVA: 0x00169E89 File Offset: 0x00168289
	Private Sub animationEvent_BeginSpitEyes()
		Me.cauldronEyes.SetActive(True)
	End Sub

	' Token: 0x060026C1 RID: 9921 RVA: 0x00169E97 File Offset: 0x00168297
	Private Sub animationEvent_EndSpitEyes()
		Me.cauldronEyes.SetActive(False)
	End Sub

	' Token: 0x060026C2 RID: 9922 RVA: 0x00169EA5 File Offset: 0x001682A5
	Private Sub AnimationEvent_SFX_OMM_Intro()
		AudioManager.Play("sfx_dlc_omm_intro")
		Me.emitAudioFromObject.Add("sfx_dlc_omm_intro")
	End Sub

	' Token: 0x060026C3 RID: 9923 RVA: 0x00169EC1 File Offset: 0x001682C1
	Private Sub AnimationEvent_SFX_OMM_IntroPickaxe()
		AudioManager.Play("sfx_dlc_omm_intropickaxe")
		Me.emitAudioFromObject.Add("sfx_dlc_omm_intropickaxe")
	End Sub

	' Token: 0x060026C4 RID: 9924 RVA: 0x00169EDD File Offset: 0x001682DD
	Private Sub AnimationEvent_SFX_OMM_GooseStormIntro()
		AudioManager.Play("sfx_dlc_omm_goosestorm")
		Me.emitAudioFromObject.Add("sfx_dlc_omm_goosestorm")
	End Sub

	' Token: 0x060026C5 RID: 9925 RVA: 0x00169EF9 File Offset: 0x001682F9
	Private Sub AnimationEvent_SFX_OMM_GooseStormLoop()
		MyBase.StartCoroutine(Me.SFX_OMM_GooseStormLoop_cr())
	End Sub

	' Token: 0x060026C6 RID: 9926 RVA: 0x00169F08 File Offset: 0x00168308
	Private Iterator Function SFX_OMM_GooseStormLoop_cr() As IEnumerator
		Yield New WaitForEndOfFrame()
		AudioManager.PlayLoop("sfx_dlc_omm_goosestorm_loop")
		Me.emitAudioFromObject.Add("sfx_dlc_omm_goosestorm_loop")
		Return
	End Function

	' Token: 0x060026C7 RID: 9927 RVA: 0x00169F23 File Offset: 0x00168323
	Private Sub AnimationEvent_SFX_OMM_GooseStormLoopEnd()
		AudioManager.[Stop]("sfx_dlc_omm_goosestorm_loop")
		AudioManager.Play("sfx_dlc_omm_goosestorm_loop_end")
		Me.emitAudioFromObject.Add("sfx_dlc_omm_goosestorm_loop_end")
	End Sub

	' Token: 0x060026C8 RID: 9928 RVA: 0x00169F49 File Offset: 0x00168349
	Private Sub AnimationEvent_SFX_OMM_MouthCauldron_MouthClose()
		AudioManager.Play("sfx_dlc_omm_mouthcauldron_mouthclose")
		Me.emitAudioFromObject.Add("sfx_dlc_omm_mouthcauldron_mouthclose")
		AudioManager.[Stop]("sfx_dlc_omm_mouthcauldron_stirring_loop")
	End Sub

	' Token: 0x060026C9 RID: 9929 RVA: 0x00169F70 File Offset: 0x00168370
	Private Sub AnimationEvent_SFX_OMM_MouthCauldron_MouthOpen()
		AudioManager.Play("sfx_dlc_omm_mouthcauldron_mouthopen")
		Me.emitAudioFromObject.Add("sfx_dlc_omm_mouthcauldron_mouthopen")
		AudioManager.FadeSFXVolume("sfx_dlc_omm_mouthcauldron_stirring_loop_start", 1F, 0.1F)
		AudioManager.Play("sfx_dlc_omm_mouthcauldron_stirring_loop_start")
		Me.emitAudioFromObject.Add("sfx_dlc_omm_mouthcauldron_stirring_loop_start")
		AudioManager.PlayLoop("sfx_dlc_omm_mouthcauldron_stirring_loop")
		Me.emitAudioFromObject.Add("sfx_dlc_omm_mouthcauldron_stirring_loop")
	End Sub

	' Token: 0x060026CA RID: 9930 RVA: 0x00169FDF File Offset: 0x001683DF
	Private Sub AnimationEvent_SFX_OMM_BearAttackOMMStartVocal()
		AudioManager.Play("sfx_dlc_omm_bearattack_ommstartvocal")
		Me.emitAudioFromObject.Add("sfx_dlc_omm_bearattack_ommstartvocal")
	End Sub

	' Token: 0x060026CB RID: 9931 RVA: 0x00169FFB File Offset: 0x001683FB
	Private Sub AnimationEvent_SFX_OMM_BearAttackStart()
		AudioManager.Play("sfx_dlc_omm_bearattack_start")
		Me.emitAudioFromObject.Add("sfx_dlc_omm_bearattack_start")
	End Sub

	' Token: 0x060026CC RID: 9932 RVA: 0x0016A017 File Offset: 0x00168417
	Private Sub AnimationEvent_SFX_OMM_P2_OMMVocalFrustrated()
		AudioManager.Play("sfx_dlc_omm_p2_end_ommvocalfrustrated")
		Me.emitAudioFromObject.Add("sfx_dlc_omm_p2_end_ommvocalfrustrated")
	End Sub

	' Token: 0x060026CD RID: 9933 RVA: 0x0016A033 File Offset: 0x00168433
	Private Sub AnimationEvent_SFX_OMM_P2_TransitionBeardPull()
		AudioManager.Play("sfx_dlc_omm_p2_transition_pullbeardoff")
		Me.emitAudioFromObject.Add("sfx_dlc_omm_p2_transition_pullbeardoff")
	End Sub

	' Token: 0x04002F5D RID: 12125
	Private Shared IsSpitAttackParameterID As Integer = Animator.StringToHash("IsSpitAttack")

	' Token: 0x04002F5E RID: 12126
	Private Shared IsSpitAttackEyeLoopParameterID As Integer = Animator.StringToHash("IsSpitAttackEyeLoop")

	' Token: 0x04002F5F RID: 12127
	Private Shared IsGooseAttackParameterID As Integer = Animator.StringToHash("IsGooseAttack")

	' Token: 0x04002F60 RID: 12128
	Private Shared ContinueParameterID As Integer = Animator.StringToHash("Continue")

	' Token: 0x04002F61 RID: 12129
	Private Shared IsBearAttackParameterID As Integer = Animator.StringToHash("IsBearAttack")

	' Token: 0x04002F62 RID: 12130
	Private Const DUCK_MOVE_END As Single = -165F

	' Token: 0x04002F63 RID: 12131
	Private Const BEAR_START_X As Single = -1300F

	' Token: 0x04002F64 RID: 12132
	Private Const BEAR_Y As Single = 100F

	' Token: 0x04002F65 RID: 12133
	<SerializeField()>
	Private goosePrefab As OldManLevelGoose

	' Token: 0x04002F66 RID: 12134
	<SerializeField()>
	Private bearBeam As OldManLevelBear

	' Token: 0x04002F67 RID: 12135
	<SerializeField()>
	Private spitRoot As Transform

	' Token: 0x04002F68 RID: 12136
	<SerializeField()>
	Private spitEndArc As Transform

	' Token: 0x04002F69 RID: 12137
	<SerializeField()>
	Private spitProjectile As OldManLevelSpitProjectile

	' Token: 0x04002F6A RID: 12138
	<SerializeField()>
	Private spitProjectilePink As OldManLevelSpitProjectile

	' Token: 0x04002F6B RID: 12139
	<SerializeField()>
	Private platformManager As OldManLevelPlatformManager

	' Token: 0x04002F6C RID: 12140
	<SerializeField()>
	Private sockPuppets As OldManLevelSockPuppetHandler

	' Token: 0x04002F6D RID: 12141
	<SerializeField()>
	Private eyeRenderer As SpriteRenderer

	' Token: 0x04002F6E RID: 12142
	<SerializeField()>
	Private cauldron As GameObject

	' Token: 0x04002F6F RID: 12143
	<SerializeField()>
	Private cauldronEyes As GameObject

	' Token: 0x04002F70 RID: 12144
	<SerializeField()>
	Private gooseFXAnimator As Animator

	' Token: 0x04002F71 RID: 12145
	<SerializeField()>
	Private rightWall As GameObject

	' Token: 0x04002F73 RID: 12147
	Private sprite As SpriteRenderer

	' Token: 0x04002F74 RID: 12148
	Private damageReceiver As DamageReceiver

	' Token: 0x04002F75 RID: 12149
	Private damageDealer As DamageDealer

	' Token: 0x04002F76 RID: 12150
	Private spitStringMainIndex As Integer

	' Token: 0x04002F77 RID: 12151
	Private shouldIdleBlink As Boolean

	' Token: 0x04002F78 RID: 12152
	Private endPhaseOne As Boolean

	' Token: 0x04002F79 RID: 12153
	Private gooseSpawnString As PatternString

	' Token: 0x02000707 RID: 1799
	Public Enum State
		' Token: 0x04002F7B RID: 12155
		Intro
		' Token: 0x04002F7C RID: 12156
		Idle
		' Token: 0x04002F7D RID: 12157
		Spit
		' Token: 0x04002F7E RID: 12158
		Goose
		' Token: 0x04002F7F RID: 12159
		Bear
	End Enum
End Class
