Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Diagnostics
Imports UnityEngine

' Token: 0x020007D4 RID: 2004
Public Class SaltbakerLevelSaltbaker
	Inherits LevelProperties.Saltbaker.Entity

	' Token: 0x1400004C RID: 76
	' (add) Token: 0x06002D86 RID: 11654 RVA: 0x001ADC10 File Offset: 0x001AC010
	' (remove) Token: 0x06002D87 RID: 11655 RVA: 0x001ADC48 File Offset: 0x001AC048
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnDeathEvent As Action

	' Token: 0x06002D88 RID: 11656 RVA: 0x001ADC80 File Offset: 0x001AC080
	Private Sub Start()
		Me.scale = MyBase.transform.localScale.x
		Me.startPos = MyBase.transform.position
		Me.damageReceiver = Me.phaseOneCollider.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		Me.doughFXAnimator.transform.parent = Nothing
		MyBase.animator.SetBool("IntroCubes", Rand.Bool())
	End Sub

	' Token: 0x06002D89 RID: 11657 RVA: 0x001ADD08 File Offset: 0x001AC108
	Public Overrides Sub LevelInit(properties As LevelProperties.Saltbaker)
		MyBase.LevelInit(properties)
		Me.strawberriesSpawnString = New PatternString(properties.CurrentState.strawberries.locationSpawnString, True, True)
		Me.strawberriesDelayString = New PatternString(properties.CurrentState.strawberries.bulletDelayString, True, True)
		Me.sugarcubesPhaseString = New PatternString(properties.CurrentState.sugarcubes.phaseString, True, True)
		Me.sugarcubesDelayString = New PatternString(properties.CurrentState.sugarcubes.bulletDelayString, True, True)
		Me.sugarcubesParryString = New PatternString(properties.CurrentState.sugarcubes.parryString, True)
		Me.doughSpawnSidePatternString = New PatternString(properties.CurrentState.dough.doughSpawnSideString, True, True)
		Me.doughSpawnDelayString = New PatternString(properties.CurrentState.dough.doughDelayString, True, True)
		Me.doughSpawnTypeString = New PatternString(properties.CurrentState.dough.doughSpawnTypeString, True, True)
		Me.limeHeightString = New PatternString(properties.CurrentState.limes.boomerangHeightString, True, True)
		Me.limesDelayString = New PatternString(properties.CurrentState.limes.boomerangDelayString, True, True)
		Me.timeToNextAttack(0) = properties.CurrentState.strawberries.startNextAtk
		Me.timeToNextAttack(1) = properties.CurrentState.sugarcubes.startNextAttack
		Me.timeToNextAttack(2) = properties.CurrentState.dough.startNextAttack
		Me.timeToNextAttack(3) = properties.CurrentState.limes.startNextAttack
		MyBase.StartCoroutine(Me.intro_cr())
	End Sub

	' Token: 0x06002D8A RID: 11658 RVA: 0x001ADEAC File Offset: 0x001AC2AC
	Private Iterator Function intro_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1.5F)
		CType(Level.Current, SaltbakerLevel).SpawnSwoopers()
		Yield CupheadTime.WaitForSeconds(Me, 1.5F)
		Me.currentAttack = If((Not MyBase.animator.GetBool("IntroCubes")), SaltbakerLevelSaltbaker.State.Limes, SaltbakerLevelSaltbaker.State.Sugarcubes)
		Me.prevAttack = Me.currentAttack
		Me.AniEvent_StartProjectiles()
		Me.attackCoroutines.Add(MyBase.StartCoroutine(Me.pattern_cr()))
		Return
	End Function

	' Token: 0x06002D8B RID: 11659 RVA: 0x001ADEC7 File Offset: 0x001AC2C7
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		MyBase.properties.DealDamage(info.damage)
	End Sub

	' Token: 0x06002D8C RID: 11660 RVA: 0x001ADEDC File Offset: 0x001AC2DC
	Private Iterator Function pattern_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		Do
			Me.currentAttack = CType((MyBase.properties.CurrentState.NextPattern + 1), SaltbakerLevelSaltbaker.State)
		Loop While Me.currentAttack = SaltbakerLevelSaltbaker.State.Strawberries OrElse Me.currentAttack = Me.prevAttack
		While Not Me.phaseOneEnded
			Dim state As SaltbakerLevelSaltbaker.State = Me.currentAttack
			If state <> SaltbakerLevelSaltbaker.State.Sugarcubes Then
				If state <> SaltbakerLevelSaltbaker.State.Dough Then
					If state = SaltbakerLevelSaltbaker.State.Limes Then
						MyBase.animator.Play("Limes")
					End If
				Else
					MyBase.animator.Play("Dough")
				End If
			Else
				MyBase.animator.Play("Sugarcubes")
				Me.sugarTextReversed.enabled = MyBase.transform.localScale.x = -1F
			End If
			Me.prevAttack = Me.currentAttack
			While Me.currentAttack <> SaltbakerLevelSaltbaker.State.Idle
				Yield Nothing
			End While
			If Not Me.phaseOneEnded Then
				Me.currentAttack = CType((MyBase.properties.CurrentState.NextPattern + 1), SaltbakerLevelSaltbaker.State)
				MyBase.animator.SetBool("NextStrawberries", Me.currentAttack = SaltbakerLevelSaltbaker.State.Strawberries)
				Dim timeToIdle As Single = Me.timeToNextAttack(Me.prevAttack - SaltbakerLevelSaltbaker.State.Strawberries) - Me.postAttackTime(Me.prevAttack - SaltbakerLevelSaltbaker.State.Strawberries) - Me.preAttackTime(Me.currentAttack - SaltbakerLevelSaltbaker.State.Strawberries)
				Yield CupheadTime.WaitForSeconds(Me, timeToIdle)
			End If
			Yield MyBase.animator.WaitForAnimationToStart(Me, "Idle", False)
		End While
		Return
	End Function

	' Token: 0x06002D8D RID: 11661 RVA: 0x001ADEF8 File Offset: 0x001AC2F8
	Private Sub AniEvent_StartProjectiles()
		If Me.phaseOneEnded Then
			Return
		End If
		Select Case Me.currentAttack
			Case SaltbakerLevelSaltbaker.State.Strawberries
				Me.attackCoroutines.Add(MyBase.StartCoroutine(Me.strawberries_cr()))
			Case SaltbakerLevelSaltbaker.State.Sugarcubes
				Me.attackCoroutines.Add(MyBase.StartCoroutine(Me.sugarcubes_cr()))
			Case SaltbakerLevelSaltbaker.State.Dough
				Me.attackCoroutines.Add(MyBase.StartCoroutine(Me.dough_cr()))
			Case SaltbakerLevelSaltbaker.State.Limes
				Me.attackCoroutines.Add(MyBase.StartCoroutine(Me.limes_cr()))
		End Select
		If Me.currentAttack = SaltbakerLevelSaltbaker.State.Strawberries Then
			Me.prevAttack = SaltbakerLevelSaltbaker.State.Strawberries
			If Not Me.phaseOneEnded Then
				Me.currentAttack = CType((MyBase.properties.CurrentState.NextPattern + 1), SaltbakerLevelSaltbaker.State)
			End If
		Else
			Me.currentAttack = SaltbakerLevelSaltbaker.State.Idle
		End If
	End Sub

	' Token: 0x06002D8E RID: 11662 RVA: 0x001ADFE8 File Offset: 0x001AC3E8
	Private Sub AniEvent_FinishMove()
		MyBase.transform.localScale = New Vector3(-MyBase.transform.localScale.x, MyBase.transform.localScale.y)
		Me.onLeft = Not Me.onLeft
	End Sub

	' Token: 0x06002D8F RID: 11663 RVA: 0x001AE03C File Offset: 0x001AC43C
	Private Iterator Function strawberries_cr() As IEnumerator
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		Dim p As LevelProperties.Saltbaker.Strawberries = MyBase.properties.CurrentState.strawberries
		Dim delay As Single = p.firstDelay
		Dim attackTime As Single = 0F
		Dim delayTime As Single = 0F
		Dim anim As Integer = Global.UnityEngine.Random.Range(0, 4)
		While attackTime <= p.diagAtkDuration
			attackTime += CupheadTime.FixedDelta
			delayTime += CupheadTime.FixedDelta
			If delayTime > delay Then
				delayTime -= delay
				delay = Me.strawberriesDelayString.PopFloat()
				Me.destroyOnPhaseEnd.Add(Me.strawberryPrefab.Create(New Vector3(Me.strawberriesSpawnString.PopFloat(), CSng(Level.Current.Ceiling) + 100F), p.diagAngle + 90F, p.bulletSpeed, anim).gameObject)
				anim = (anim + 1) Mod 4
			End If
			Yield wait
		End While
		Return
	End Function

	' Token: 0x06002D90 RID: 11664 RVA: 0x001AE057 File Offset: 0x001AC457
	Private Sub AniEvent_StrawberryBasketStart()
		Me.strawberryBasket.StartRunIn(Me.onLeft)
	End Sub

	' Token: 0x06002D91 RID: 11665 RVA: 0x001AE06A File Offset: 0x001AC46A
	Private Sub AniEvent_StrawberryBasketGrab()
		Me.strawberryBasket.GetGrabbed()
	End Sub

	' Token: 0x06002D92 RID: 11666 RVA: 0x001AE077 File Offset: 0x001AC477
	Private Sub AniEvent_StrawberryBasketExit()
		Me.strawberryBasket.StartRunOut()
	End Sub

	' Token: 0x06002D93 RID: 11667 RVA: 0x001AE084 File Offset: 0x001AC484
	Private Iterator Function sugarcubes_cr() As IEnumerator
		Dim p As LevelProperties.Saltbaker.Sugarcubes = MyBase.properties.CurrentState.sugarcubes
		Dim side As Boolean = Me.onLeft
		Dim delay As Single = p.firstDelay
		Dim phase As Single = Me.sugarcubesPhaseString.PopFloat()
		Dim delayTime As Single = 0F
		Dim attackTime As Single = 0F
		Dim anim As Integer = Global.UnityEngine.Random.Range(0, 3)
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		While attackTime <= p.sineAttackDuration
			attackTime += CupheadTime.FixedDelta
			delayTime += CupheadTime.FixedDelta
			If delayTime > delay Then
				delayTime -= delay
				delay = Me.sugarcubesDelayString.PopFloat()
				phase = Me.sugarcubesPhaseString.PopFloat()
				Dim saltbakerLevelSugarcube As SaltbakerLevelSugarcube = Me.sugarcubePrefab.Spawn()
				saltbakerLevelSugarcube.Init(New Vector3(CSng(If((Not side), (Level.Current.Right + 100), (Level.Current.Left - 100))), p.centerHeight), side, p, phase, Me, anim, Me.sugarcubesParryString.PopLetter() = "P"c)
				Me.destroyOnPhaseEnd.Add(saltbakerLevelSugarcube.gameObject)
				anim = (anim + 1) Mod 3
			End If
			Yield wait
		End While
		Return
	End Function

	' Token: 0x06002D94 RID: 11668 RVA: 0x001AE0A0 File Offset: 0x001AC4A0
	Private Iterator Function dough_cr() As IEnumerator
		Dim p As LevelProperties.Saltbaker.Dough = MyBase.properties.CurrentState.dough
		Dim side As Boolean = Me.onLeft
		Dim attackTime As Single = 0F
		Dim delayTime As Single = 0F
		Dim delay As Single = p.firstDelay
		Dim left As Vector3 = New Vector3(CSng(Level.Current.Left) - 100F, -300F)
		Dim right As Vector3 = New Vector3(CSng(Level.Current.Right) + 100F, -300F)
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		Dim count As Integer = 0
		Dim startAnimalType As Integer = Global.UnityEngine.Random.Range(0, 3)
		While attackTime <= p.doughAttackDuration
			attackTime += CupheadTime.FixedDelta
			delayTime += CupheadTime.FixedDelta
			If delayTime > delay Then
				delayTime -= delay
				delay = Me.doughSpawnDelayString.PopFloat()
				Dim num As Integer = Me.doughSpawnTypeString.PopInt()
				Dim c As Char = Me.doughSpawnSidePatternString.PopLetter()
				Dim flag As Boolean = If((c <> "P"c), (Not side), side)
				Dim saltbakerLevelDough As SaltbakerLevelDough = Me.doughPrefab.Spawn()
				saltbakerLevelDough.Init(If((Not flag), right, left), If((Not flag), (-p.doughXSpeed(num)), p.doughXSpeed(num)), p.doughYSpeed(num), p.doughGravity(num), p.doughHealth, count, (startAnimalType + count) Mod 3)
				Me.destroyOnPhaseEnd.Add(saltbakerLevelDough.gameObject)
				count += 1
			End If
			Yield wait
		End While
		Return
	End Function

	' Token: 0x06002D95 RID: 11669 RVA: 0x001AE0BB File Offset: 0x001AC4BB
	Private Sub AniEvent_FinishDough()
		Me.doughFXAnimator.transform.localScale = MyBase.transform.localScale
		Me.doughFXAnimator.Play("DoughFX")
	End Sub

	' Token: 0x06002D96 RID: 11670 RVA: 0x001AE0E8 File Offset: 0x001AC4E8
	Private Iterator Function limes_cr() As IEnumerator
		Dim p As LevelProperties.Saltbaker.Limes = MyBase.properties.CurrentState.limes
		Dim side As Boolean = Me.onLeft
		Dim attackTime As Single = 0F
		Dim delayTime As Single = 0F
		Dim delay As Single = p.firstDelay
		Dim sfxID As Integer = 0
		Dim anim As Integer = Global.UnityEngine.Random.Range(0, 4)
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		While attackTime <= p.boomerangAttackDuration
			attackTime += CupheadTime.FixedDelta
			delayTime += CupheadTime.FixedDelta
			If delayTime > delay Then
				delayTime -= delay
				delay = Me.limesDelayString.PopFloat()
				Dim saltbakerLevelLime As SaltbakerLevelLime = Me.limePrefab.Spawn()
				saltbakerLevelLime.Init(New Vector3(CSng(If((Not side), Level.Current.Right, Level.Current.Left)), 0F), side, Me.limeHeightString.PopLetter() = "H"c, MyBase.properties.CurrentState.limes, sfxID, anim)
				Me.destroyOnPhaseEnd.Add(saltbakerLevelLime.gameObject)
				sfxID = (sfxID + 1) Mod 3
				anim = (anim + 1) Mod 4
			End If
			Yield wait
		End While
		Yield wait
		Return
	End Function

	' Token: 0x06002D97 RID: 11671 RVA: 0x001AE104 File Offset: 0x001AC504
	Public Iterator Function phase_one_to_two_cr() As IEnumerator
		Me.phaseOneEnded = True
		MyBase.animator.SetTrigger("EndPhaseOne")
		Yield MyBase.animator.WaitForAnimationToStart(Me, "PhaseOneToTwo", False)
		Me.phaseTwoHPPrediction = CSng(CInt((MyBase.properties.CurrentHealth - MyBase.properties.GetNextStateHealthTrigger() * MyBase.properties.TotalHealth)))
		For Each abstractPlayerController As AbstractPlayerController In PlayerManager.GetAllPlayers()
			Dim levelPlayerController As LevelPlayerController = CType(abstractPlayerController, LevelPlayerController)
			If levelPlayerController IsNot Nothing Then
				levelPlayerController.weaponManager.EnableSuper(False)
			End If
		Next
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "PhaseTwoIntro", False, True)
		Me.phaseTwoStarted = True
		Me.Phase2SwitchOnPatterns()
		Return
	End Function

	' Token: 0x06002D98 RID: 11672 RVA: 0x001AE120 File Offset: 0x001AC520
	Private Sub AniEvent_HitTable()
		Me.phaseOneCollider.enabled = False
		CupheadLevelCamera.Current.Shake(55F, 0.5F, False)
		MyBase.transform.localScale = New Vector3(1F, 1F)
		AudioManager.StartBGMAlternate(0)
	End Sub

	' Token: 0x06002D99 RID: 11673 RVA: 0x001AE16E File Offset: 0x001AC56E
	Private Sub AniEvent_KillFires()
		CType(Level.Current, SaltbakerLevel).KillFires()
	End Sub

	' Token: 0x06002D9A RID: 11674 RVA: 0x001AE180 File Offset: 0x001AC580
	Private Sub AniEvent_HandsClosed()
		Me.ClearPhaseOneObjects()
		CType(Level.Current, SaltbakerLevel).ClearFires()
		For Each abstractPlayerController As AbstractPlayerController In PlayerManager.GetAllPlayers()
			Dim levelPlayerController As LevelPlayerController = CType(abstractPlayerController, LevelPlayerController)
			If levelPlayerController IsNot Nothing Then
				levelPlayerController.weaponManager.InterruptSuper()
			End If
		Next
		For Each abstractPlayerController2 As AbstractPlayerController In PlayerManager.GetAllPlayers()
			Dim levelPlayerController2 As LevelPlayerController = CType(abstractPlayerController2, LevelPlayerController)
			If levelPlayerController2 IsNot Nothing Then
				levelPlayerController2.DisableInput()
				levelPlayerController2.motor.ClearBufferedInput()
				Level.Current.SetBounds(New Integer?(10780), New Integer?(-9220), New Integer?(446), New Integer?(370))
				levelPlayerController2.transform.position = Me.playerDefrostPositions(CInt(levelPlayerController2.id)).position + Vector3.left * 10000F
			End If
		Next
		Me.transitionCamera.SetActive(True)
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, False, False)
		AddHandler LevelPauseGUI.OnUnpauseEvent, AddressOf Me.SuppressPlayerJoin
		If CType(Level.Current, SaltbakerLevel).playerLost Then
			MyBase.animator.speed = 0F
		Else
			MyBase.StartCoroutine(Me.scroll_bg_cr())
		End If
	End Sub

	' Token: 0x06002D9B RID: 11675 RVA: 0x001AE328 File Offset: 0x001AC728
	Private Sub AniEvent_ShakeScreen()
		CupheadLevelCamera.Current.Shake(55F, 0.5F, False)
	End Sub

	' Token: 0x06002D9C RID: 11676 RVA: 0x001AE33F File Offset: 0x001AC73F
	Private Sub AniEvent_FadeInReflection()
		MyBase.StartCoroutine(Me.fade_in_reflection_cr())
	End Sub

	' Token: 0x06002D9D RID: 11677 RVA: 0x001AE350 File Offset: 0x001AC750
	Private Sub ClearPhaseOneObjects()
		Me.attackCoroutines.RemoveAll(Function(c As Coroutine) c Is Nothing)
		For Each coroutine As Coroutine In Me.attackCoroutines
			MyBase.StopCoroutine(coroutine)
		Next
		Dim array As GameObject() = GameObject.FindGameObjectsWithTag("PlayerProjectile")
		For m As Integer = 0 To array.Length - 1
			Global.UnityEngine.[Object].Destroy(array(m))
		Next
		Dim array2 As Effect() = CType(Global.UnityEngine.[Object].FindObjectsOfType(GetType(Effect)), Effect())
		For j As Integer = 0 To array2.Length - 1
			Global.UnityEngine.[Object].Destroy(array2(j).gameObject)
		Next
		Me.destroyOnPhaseEnd.RemoveAll(Function(i As GameObject) i Is Nothing)
		For k As Integer = 0 To Me.destroyOnPhaseEnd.Count - 1
			Global.UnityEngine.[Object].Destroy(Me.destroyOnPhaseEnd(k))
		Next
		Global.UnityEngine.[Object].Destroy(Me.strawberryBasket.gameObject)
		For Each abstractPlayerController As AbstractPlayerController In PlayerManager.GetAllPlayers()
			Dim levelPlayerController As LevelPlayerController = CType(abstractPlayerController, LevelPlayerController)
			If levelPlayerController IsNot Nothing Then
				levelPlayerController.weaponManager.AbortEX()
			End If
		Next
		For Each playerSuperChaliceShieldHeart As PlayerSuperChaliceShieldHeart In Global.UnityEngine.[Object].FindObjectsOfType(Of PlayerSuperChaliceShieldHeart)()
			playerSuperChaliceShieldHeart.transform.parent = playerSuperChaliceShieldHeart.player.transform
		Next
	End Sub

	' Token: 0x06002D9E RID: 11678 RVA: 0x001AE548 File Offset: 0x001AC948
	Private Iterator Function scroll_bg_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, Me.transitionDelayAfterHandsClose)
		Dim level As SaltbakerLevel = TryCast(Level.Current, SaltbakerLevel)
		Dim t As Single = 0F
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		CupheadLevelCamera.Current.Shake(8F, Me.transitionDuration, False)
		Dim shadowOffset As Vector3 = Me.shadow.transform.position - Me.table.transform.position
		While t < Me.transitionDuration
			level.yScrollPos = EaseUtils.EaseInOut(EaseUtils.EaseType.easeInSine, EaseUtils.EaseType.easeOutBack, 0F, 1F, Mathf.InverseLerp(0F, Me.transitionDuration, t))
			Me.shadow.transform.position = shadowOffset + Me.table.transform.position + Vector3.up * level.yScrollPos * 1500F
			t += CupheadTime.FixedDelta
			Yield wait
		End While
		level.yScrollPos = 1F
		Return
	End Function

	' Token: 0x06002D9F RID: 11679 RVA: 0x001AE564 File Offset: 0x001AC964
	Private Iterator Function fade_in_reflection_cr() As IEnumerator
		Me.reflectionCamera.SetActive(True)
		Yield Nothing
		Me.reflectionTexture.SetActive(True)
		Dim c As Single = 0F
		While c < 0.5F
			c = Mathf.Clamp(c + CupheadTime.Delta * 5F, 0F, 0.5F)
			Me.reflectionMaterial.color = New Color(1F, 1F, 1F, c)
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06002DA0 RID: 11680 RVA: 0x001AE57F File Offset: 0x001AC97F
	Private Sub SuppressPlayerJoin()
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, False, False)
	End Sub

	' Token: 0x06002DA1 RID: 11681 RVA: 0x001AE58C File Offset: 0x001AC98C
	Private Sub AniEvent_HandsOpen()
		For Each abstractPlayerController As AbstractPlayerController In PlayerManager.GetAllPlayers()
			Dim levelPlayerController As LevelPlayerController = CType(abstractPlayerController, LevelPlayerController)
			If levelPlayerController IsNot Nothing Then
				levelPlayerController.EnableInput()
				levelPlayerController.weaponManager.DisableInput()
				levelPlayerController.transform.position = Me.playerDefrostPositions(CInt(levelPlayerController.id)).position + Vector3.left * 10000F
				levelPlayerController.motor.DoPostSuperHop()
			End If
		Next
	End Sub

	' Token: 0x06002DA2 RID: 11682 RVA: 0x001AE640 File Offset: 0x001ACA40
	Private Sub AniEvent_SFX_MagicDough()
		AudioManager.Play("sfx_dlc_saltbaker_p1_magiccookie")
		Me.emitAudioFromObject.Add("sfx_dlc_saltbaker_p1_magiccookie")
	End Sub

	' Token: 0x06002DA3 RID: 11683 RVA: 0x001AE65C File Offset: 0x001ACA5C
	Private Sub AniEvent_SpawnJumpers()
		CType(Level.Current, SaltbakerLevel).SpawnJumpers()
	End Sub

	' Token: 0x06002DA4 RID: 11684 RVA: 0x001AE66D File Offset: 0x001ACA6D
	Private Sub AniEvent_ShakeScreenSaltFall()
		CupheadLevelCamera.Current.Shake(20F, 2F, False)
	End Sub

	' Token: 0x06002DA5 RID: 11685 RVA: 0x001AE684 File Offset: 0x001ACA84
	Private Sub AniEvent_RestorePlayers()
		For Each abstractPlayerController As AbstractPlayerController In PlayerManager.GetAllPlayers()
			Dim levelPlayerController As LevelPlayerController = CType(abstractPlayerController, LevelPlayerController)
			If levelPlayerController IsNot Nothing Then
				levelPlayerController.weaponManager.EnableSuper(True)
				levelPlayerController.weaponManager.EnableInput()
				Level.Current.SetBounds(New Integer?(780), New Integer?(780), New Integer?(446), New Integer?(370))
				levelPlayerController.transform.position += Vector3.right * 10000F
			End If
		Next
		For Each playerSuperChaliceShieldHeart As PlayerSuperChaliceShieldHeart In Global.UnityEngine.[Object].FindObjectsOfType(Of PlayerSuperChaliceShieldHeart)()
			playerSuperChaliceShieldHeart.transform.parent = Nothing
		Next
		Me.transitionCamera.SetActive(False)
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, True, True)
		RemoveHandler LevelPauseGUI.OnUnpauseEvent, AddressOf Me.SuppressPlayerJoin
	End Sub

	' Token: 0x06002DA6 RID: 11686 RVA: 0x001AE7AC File Offset: 0x001ACBAC
	Private Sub AniEvent_StartHandIdle()
		Me.phaseOneRenderer.enabled = False
		Me.handAnimator.Play("Idle")
		Me.handAnimator.Update(0F)
		MyBase.GetComponent(Of SpriteRenderer)().enabled = False
	End Sub

	' Token: 0x06002DA7 RID: 11687 RVA: 0x001AE7E6 File Offset: 0x001ACBE6
	Private Sub Phase2SwitchOnPatterns()
		If MyBase.properties.CurrentState.leaf.leavesOn Then
			MyBase.StartCoroutine(Me.leaf_fall_cr())
		End If
	End Sub

	' Token: 0x06002DA8 RID: 11688 RVA: 0x001AE80F File Offset: 0x001ACC0F
	Public Function PreDamagePhaseTwoAndReturnWhetherDoomed(damage As Single) As Boolean
		Me.phaseTwoHPPrediction -= damage
		If Me.phaseTwoHPPrediction < 0F Then
			AudioManager.StopBGM()
			AudioManager.StartBGMAlternate(1)
		End If
		Return Me.phaseTwoHPPrediction < 0F
	End Function

	' Token: 0x06002DA9 RID: 11689 RVA: 0x001AE848 File Offset: 0x001ACC48
	Public Sub DamageSaltbaker(damage As Single, turretIndex As Integer)
		MyBase.properties.DealDamage(damage)
		If MyBase.properties.CurrentState.stateName <> LevelProperties.Saltbaker.States.PhaseThree Then
			MyBase.animator.Play(Me.turretHitAnimName(turretIndex))
			MyBase.animator.Update(0F)
			Me.handAnimator.Play("Hit", 0, 0F)
			Me.mintHandAnimator.Play(If((turretIndex <> 3), "HitA", "HitB"), 1, 0F)
			CupheadLevelCamera.Current.Shake(30F, 0.5F, False)
		End If
	End Sub

	' Token: 0x06002DAA RID: 11690 RVA: 0x001AE8EC File Offset: 0x001ACCEC
	Private Sub AniEvent_SpawnPepperShaker()
		Me.turrets(Me.turretIndex) = Global.UnityEngine.[Object].Instantiate(Of SaltbakerLevelFeistTurret)(Me.feistTurretPrefab)
		Me.turrets(Me.turretIndex).transform.position = Me.turretRoots(Me.turretIndex).position
		Me.turrets(Me.turretIndex).transform.localScale = New Vector3(Mathf.Sign(-Me.turrets(Me.turretIndex).transform.position.x), 1F)
		Me.turrets(Me.turretIndex).Setup(MyBase.properties.CurrentState.turrets, Me, Me.turretIndex)
		Me.turretIndex += 1
		If Me.turretIndex = 4 Then
			MyBase.StartCoroutine(Me.turret_cr())
		End If
	End Sub

	' Token: 0x06002DAB RID: 11691 RVA: 0x001AE9D0 File Offset: 0x001ACDD0
	Private Iterator Function turret_cr() As IEnumerator
		Dim p As LevelProperties.Saltbaker.Turrets = MyBase.properties.CurrentState.turrets
		Me.turretIndex = Global.UnityEngine.Random.Range(0, 4)
		Me.turretFiringDir = Rand.PosOrNeg()
		Dim bulletTypeString As PatternString = New PatternString(p.bulletTypeString, True, True)
		Yield CupheadTime.WaitForSeconds(Me, p.shotDelay)
		While True
			If Me.turrets IsNot Nothing AndAlso Me.turrets(Me.turretFiringOrder(Me.turretIndex)).IsActivated Then
				Dim isPink As Boolean = bulletTypeString.PopLetter() = "P"c
				Me.turrets(Me.turretFiringOrder(Me.turretIndex)).Shoot(isPink, p.warningTime)
				Yield CupheadTime.WaitForSeconds(Me, p.shotDelay)
			End If
			Me.turretIndex = (Me.turretIndex + Me.turretFiringDir + 4) Mod Me.turrets.Length
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06002DAC RID: 11692 RVA: 0x001AE9EC File Offset: 0x001ACDEC
	Private Iterator Function leaf_fall_cr() As IEnumerator
		Dim p As LevelProperties.Saltbaker.Leaf = MyBase.properties.CurrentState.leaf
		Dim leavesCountString As PatternString = New PatternString(p.leavesCountString, True, True)
		Dim animA As Boolean = Rand.Bool()
		Dim posX As Single = 0F
		Dim posY As Single = CSng(Level.Current.Ceiling) + 20F
		While True
			animA = Not animA
			MyBase.animator.SetTrigger(If((Not animA), "MintB", "MintA"))
			Yield MyBase.animator.WaitForAnimationToStart(Me, If((Not animA), "PhaseTwoMintB", "PhaseTwoMintA"), False)
			Me.mintHandAnimator.Play(If((Not animA), "MintB", "MintA"))
			Yield Me.mintHandAnimator.WaitForAnimationToEnd(Me, If((Not animA), "MintB", "MintA"), False, True)
			Yield CupheadTime.WaitForSeconds(Me, p.reenterDelay)
			Dim leavesCount As Integer = leavesCountString.PopInt()
			Dim offset As Single = CSng((Level.Current.Width / leavesCount))
			Dim extraOffset As Single = p.leavesOffset.RandomFloat()
			Dim animIDs As List(Of Integer) = New List(Of Integer)() From { 0, 1, 2, 3 }
			For i As Integer = 0 To animIDs.Count - 1
				Dim num As Integer = Global.UnityEngine.Random.Range(0, animIDs.Count)
				Dim num2 As Integer = animIDs(i)
				animIDs(i) = animIDs(num)
				animIDs(num) = num2
			Next
			For j As Integer = 0 To leavesCount - 1
				posX = offset * (CSng(j) - CSng((leavesCount - 1)) / 2F) - p.xDistance / 2F
				Dim vector As Vector3 = New Vector3(posX + extraOffset, posY)
				Dim saltBakerLevelLeaf As SaltBakerLevelLeaf = Me.leafFallPrefab.Spawn()
				saltBakerLevelLeaf.Init(vector, p.xTime, p.xDistance, p.yConstantSpeed, p.ySpeed, Me, animIDs(j Mod 4))
			Next
			For k As Integer = 0 To Global.UnityEngine.Random.Range(4, 8) - 1
				Dim saltbakerLevelBGMint As SaltbakerLevelBGMint = Global.UnityEngine.[Object].Instantiate(Of SaltbakerLevelBGMint)(Me.bgMintPrefab, New Vector3(CSng(Global.UnityEngine.Random.Range(Level.Current.Left, 0)), CSng((Level.Current.Ceiling + Global.UnityEngine.Random.Range(250, 500)))), Quaternion.identity, Nothing)
				saltbakerLevelBGMint.StartAnimation(k Mod 4)
			Next
			AudioManager.Play("sfx_dlc_saltbaker_p2_mintleafattack_leafdescend")
			Yield CupheadTime.WaitForSeconds(Me, p.leavesDelay - 1.75F)
		End While
		Return
	End Function

	' Token: 0x06002DAD RID: 11693 RVA: 0x001AEA07 File Offset: 0x001ACE07
	Public Sub OnPhaseThree()
		Me.StopAllCoroutines()
		MyBase.StartCoroutine(Me.phase_two_to_three_cr())
	End Sub

	' Token: 0x06002DAE RID: 11694 RVA: 0x001AEA1C File Offset: 0x001ACE1C
	Private Iterator Function phase_two_to_three_cr() As IEnumerator
		If Me.OnDeathEvent IsNot Nothing Then
			Me.OnDeathEvent()
		End If
		MyBase.animator.Play("PhaseTwoDeath")
		Me.handAnimator.Play("Death")
		Me.mintHandAnimator.Play("None")
		Yield MyBase.animator.WaitForAnimationToStart(Me, "PhaseTwoDeath", False)
		Me.transitionFader.gameObject.SetActive(True)
		Me.turretIndex = 0
		While Me.turretIndex < 4
			Me.turrets(Me.turretIndex).Die()
			Me.turretIndex += 1
			Yield CupheadTime.WaitForSeconds(Me, 0.1F)
		End While
		While MyBase.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.975F
			Me.transitionFader.color = New Color(1F, 1F, 1F, Mathf.InverseLerp(0.8F, 0.975F, MyBase.animator.GetCurrentAnimatorStateInfo(0).normalizedTime))
			CupheadLevelCamera.Current.Shake(MyBase.animator.GetCurrentAnimatorStateInfo(0).normalizedTime * Me.endPhaseTwoShakeAmount, Me.startPhaseThreeShakeHoldover, False)
			Yield Nothing
		End While
		CupheadLevelCamera.Current.Shake(Me.endPhaseTwoShakeAmount, Me.startPhaseThreeShakeHoldover, False)
		For Each saltbakerLevelFeistTurret As SaltbakerLevelFeistTurret In Me.turrets
			Global.UnityEngine.[Object].Destroy(saltbakerLevelFeistTurret.gameObject)
		Next
		Me.transitionFader.color = Color.white
		Yield CupheadTime.WaitForSeconds(Me, 0.5F)
		CType(Level.Current, SaltbakerLevel).StartPhase3()
		Me.BG.SetActive(False)
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Return
	End Function

	' Token: 0x06002DAF RID: 11695 RVA: 0x001AEA37 File Offset: 0x001ACE37
	Protected Overrides Sub OnDestroy()
		Global.UnityEngine.[Object].Destroy(Me.reflectionCamera)
		Global.UnityEngine.[Object].Destroy(Me.reflectionTexture)
		MyBase.OnDestroy()
	End Sub

	' Token: 0x06002DB0 RID: 11696 RVA: 0x001AEA55 File Offset: 0x001ACE55
	Private Sub AnimationEvent_SFX_SALTBAKER_P1_DoughAttack_RollAndKnead()
		AudioManager.Play("sfx_dlc_saltbaker_p1_doughattack_rollandknead")
	End Sub

	' Token: 0x06002DB1 RID: 11697 RVA: 0x001AEA61 File Offset: 0x001ACE61
	Private Sub AnimationEvent_SFX_SALTBAKER_P1_DoughAttack_RollingPinAppear()
		AudioManager.Play("sfx_dlc_saltbaker_p1_doughattack_rollingpinappear")
	End Sub

	' Token: 0x06002DB2 RID: 11698 RVA: 0x001AEA6D File Offset: 0x001ACE6D
	Private Sub AnimationEvent_SFX_SALTBAKER_P1_Intro_BowTiePull()
		AudioManager.Play("sfx_dlc_saltbaker_p1_intro_bowtiepull")
	End Sub

	' Token: 0x06002DB3 RID: 11699 RVA: 0x001AEA79 File Offset: 0x001ACE79
	Private Sub AnimationEvent_SFX_SALTBAKER_P1_Intro_HandSwipeLimesSugar()
		AudioManager.Play("sfx_dlc_saltbaker_p1_intro_handswipe_limessugar")
	End Sub

	' Token: 0x06002DB4 RID: 11700 RVA: 0x001AEA85 File Offset: 0x001ACE85
	Private Sub AnimationEvent_SFX_SALTBAKER_P1_Limes_Knife_ChopCut()
		AudioManager.Play("sfx_dlc_saltbaker_p1_limes_knife_chopcut")
	End Sub

	' Token: 0x06002DB5 RID: 11701 RVA: 0x001AEA91 File Offset: 0x001ACE91
	Private Sub AnimationEvent_SFX_SALTBAKER_P1_Limes_Knife_Scrape()
		AudioManager.Play("sfx_dlc_saltbaker_p1_limes_knife_scrape")
	End Sub

	' Token: 0x06002DB6 RID: 11702 RVA: 0x001AEA9D File Offset: 0x001ACE9D
	Private Sub AnimationEvent_SFX_SALTBAKER_P1_Limes_Knife_SliceSwing()
		AudioManager.Play("sfx_dlc_saltbaker_p1_limes_knife_sliceswing")
	End Sub

	' Token: 0x06002DB7 RID: 11703 RVA: 0x001AEAA9 File Offset: 0x001ACEA9
	Private Sub AnimationEvent_SFX_SALTBAKER_P1_StrawberrySqueeze_Attack()
		AudioManager.Play("sfx_dlc_saltbaker_p1_strawberrysqueeze_attack")
	End Sub

	' Token: 0x06002DB8 RID: 11704 RVA: 0x001AEAB5 File Offset: 0x001ACEB5
	Private Sub AnimationEvent_SFX_SALTBAKER_P1_SugarCube_Blow()
		AudioManager.Play("sfx_dlc_saltbaker_p1_sugarcube_blow")
	End Sub

	' Token: 0x06002DB9 RID: 11705 RVA: 0x001AEAC1 File Offset: 0x001ACEC1
	Private Sub AnimationEvent_SFX_SALTBAKER_P1_SugarCube_KnockAndBreak()
		AudioManager.Play("sfx_dlc_saltbaker_p1_sugarcube_knockandbreak")
	End Sub

	' Token: 0x06002DBA RID: 11706 RVA: 0x001AEACD File Offset: 0x001ACECD
	Private Sub AnimationEvent_SFX_SALTBAKER_P1_SugarCube_PlaceOnTable()
		AudioManager.Play("sfx_dlc_saltbaker_p1_sugarcube_placeontable")
	End Sub

	' Token: 0x06002DBB RID: 11707 RVA: 0x001AEAD9 File Offset: 0x001ACED9
	Private Sub AnimationEvent_SFX_SALTBAKER_P1_to_P2_Transition_A_TableSlam()
		AudioManager.Play("sfx_dlc_saltbaker_p1_to_p2_transition_a_tableslam")
	End Sub

	' Token: 0x06002DBC RID: 11708 RVA: 0x001AEAE5 File Offset: 0x001ACEE5
	Private Sub AnimationEvent_SFX_SALTBAKER_P1_to_P2_Transition_B_HatRemove()
		AudioManager.Play("sfx_dlc_saltbaker_p1_to_p2_transition_b_hatremove")
	End Sub

	' Token: 0x06002DBD RID: 11709 RVA: 0x001AEAF1 File Offset: 0x001ACEF1
	Private Sub AnimationEvent_SFX_SALTBAKER_P1_to_P2_Transition_C_ShroomInsert()
		AudioManager.Play("sfx_dlc_saltbaker_p1_to_p2_transition_c_shroominsert")
	End Sub

	' Token: 0x06002DBE RID: 11710 RVA: 0x001AEAFD File Offset: 0x001ACEFD
	Private Sub AnimationEvent_SFX_SALTBAKER_P1_to_P2_Transition_D_BakerPowerup()
		AudioManager.Play("sfx_dlc_saltbaker_p1_to_p2_transition_d_bakerpowerup")
	End Sub

	' Token: 0x06002DBF RID: 11711 RVA: 0x001AEB09 File Offset: 0x001ACF09
	Private Sub AnimationEvent_SFX_SALTBAKER_P1_to_P2_Transition_E_GrabandRise()
		AudioManager.Play("sfx_dlc_saltbaker_p1_to_p2_transition_e_grabandrise")
	End Sub

	' Token: 0x06002DC0 RID: 11712 RVA: 0x001AEB15 File Offset: 0x001ACF15
	Private Sub AnimationEvent_SFX_SALTBAKER_P2_MintLeafAttack_LaunchThrow()
		AudioManager.Play("sfx_dlc_saltbaker_p2_mintleafattack_launchthrow")
	End Sub

	' Token: 0x06002DC1 RID: 11713 RVA: 0x001AEB21 File Offset: 0x001ACF21
	Private Sub AnimationEvent_SFX_SALTBAKER_P2_MintLeafAttack_LeafRustle()
		AudioManager.Play("sfx_dlc_saltbaker_p2_mintleafattack_leafrustle")
	End Sub

	' Token: 0x06002DC2 RID: 11714 RVA: 0x001AEB2D File Offset: 0x001ACF2D
	Private Sub AnimationEvent_SFX_SALTBAKER_P2_Intro_Fingersnap()
		AudioManager.Play("sfx_dlc_saltbaker_p2_intro_fingersnap")
	End Sub

	' Token: 0x06002DC3 RID: 11715 RVA: 0x001AEB39 File Offset: 0x001ACF39
	Private Sub AnimationEvent_SFX_SALTBAKER_P2_Intro_Fingersnap_Laugh()
		AudioManager.Play("sfx_dlc_saltbaker_p2_intro_fingersnap_laugh")
	End Sub

	' Token: 0x06002DC4 RID: 11716 RVA: 0x001AEB45 File Offset: 0x001ACF45
	Private Sub AnimationEvent_SFX_SALTBAKER_P2_VocalPain()
		AudioManager.Play("sfx_dlc_saltbaker_p2_vocal_pain")
	End Sub

	' Token: 0x06002DC5 RID: 11717 RVA: 0x001AEB51 File Offset: 0x001ACF51
	Private Sub AnimationEvent_SFX_SALTBAKER_P2_Death()
		AudioManager.Play("sfx_dlc_saltbaker_p2_death")
	End Sub

	' Token: 0x06002DC6 RID: 11718 RVA: 0x001AEB5D File Offset: 0x001ACF5D
	Public Sub SFXLeafRustle()
		AudioManager.Play("sfx_dlc_saltbaker_p2_mintleafattack_leafrustle")
	End Sub

	' Token: 0x06002DC7 RID: 11719 RVA: 0x001AEB69 File Offset: 0x001ACF69
	Public Sub SFXLaunchThrow()
		AudioManager.Play("sfx_dlc_saltbaker_p2_mintleafattack_launchthrow")
	End Sub

	' Token: 0x0400360E RID: 13838
	Private Const MOVE_POS_X As Single = 370F

	' Token: 0x0400360F RID: 13839
	Private Const MINT_ANIMATION_LENGTH As Single = 1.75F

	' Token: 0x04003610 RID: 13840
	Private Const PHASE_TWO_REFLECTION_OPACITY As Single = 0.5F

	' Token: 0x04003611 RID: 13841
	Public currentAttack As SaltbakerLevelSaltbaker.State

	' Token: 0x04003612 RID: 13842
	Private prevAttack As SaltbakerLevelSaltbaker.State

	' Token: 0x04003613 RID: 13843
	Private preAttackTime As Single() = New Single() { 1.75F, 4.5416665F, 5.2083335F, 4.2083335F }

	' Token: 0x04003614 RID: 13844
	Private postAttackTime As Single() = New Single() { 1.125F, 1.5833334F, 0.33333334F, 1.9166666F }

	' Token: 0x04003616 RID: 13846
	<SerializeField()>
	Private sugarTextReversed As SpriteRenderer

	' Token: 0x04003617 RID: 13847
	<SerializeField()>
	Private playerDefrostPositions As Transform()

	' Token: 0x04003618 RID: 13848
	<SerializeField()>
	Private shadow As GameObject

	' Token: 0x04003619 RID: 13849
	<SerializeField()>
	Private table As GameObject

	' Token: 0x0400361A RID: 13850
	<Header("Prefabs")>
	<SerializeField()>
	Private strawberryPrefab As SaltbakerLevelStrawberry

	' Token: 0x0400361B RID: 13851
	<SerializeField()>
	Private sugarcubePrefab As SaltbakerLevelSugarcube

	' Token: 0x0400361C RID: 13852
	<SerializeField()>
	Private doughPrefab As SaltbakerLevelDough

	' Token: 0x0400361D RID: 13853
	<SerializeField()>
	Private limePrefab As SaltbakerLevelLime

	' Token: 0x0400361E RID: 13854
	<SerializeField()>
	Private strawberryBasket As SaltbakerLevelStrawberryBasket

	' Token: 0x0400361F RID: 13855
	<SerializeField()>
	Private feistTurretPrefab As SaltbakerLevelFeistTurret

	' Token: 0x04003620 RID: 13856
	Private turrets As SaltbakerLevelFeistTurret() = New SaltbakerLevelFeistTurret(3) {}

	' Token: 0x04003621 RID: 13857
	Private turretIndex As Integer

	' Token: 0x04003622 RID: 13858
	Private turretFiringOrder As Integer() = New Integer() { 2, 1, 3, 0 }

	' Token: 0x04003623 RID: 13859
	Private turretFiringDir As Integer

	' Token: 0x04003624 RID: 13860
	Private turretHitAnimName As String() = New String() { "PhaseTwoHitB", "PhaseTwoHitA", "PhaseTwoHitD", "PhaseTwoHitC" }

	' Token: 0x04003625 RID: 13861
	<SerializeField()>
	Private leafFallPrefab As SaltBakerLevelLeaf

	' Token: 0x04003626 RID: 13862
	<SerializeField()>
	Private bgMintPrefab As SaltbakerLevelBGMint

	' Token: 0x04003627 RID: 13863
	<SerializeField()>
	Private turretRoots As Transform()

	' Token: 0x04003628 RID: 13864
	<SerializeField()>
	Private handAnimator As Animator

	' Token: 0x04003629 RID: 13865
	<SerializeField()>
	Private transitionCamera As GameObject

	' Token: 0x0400362A RID: 13866
	<SerializeField()>
	Private transitionDelayAfterHandsClose As Single

	' Token: 0x0400362B RID: 13867
	<SerializeField()>
	Private transitionDuration As Single = 2.5F

	' Token: 0x0400362C RID: 13868
	<SerializeField()>
	Private transitionFader As SpriteRenderer

	' Token: 0x0400362D RID: 13869
	<SerializeField()>
	Private endPhaseTwoShakeAmount As Single = 75F

	' Token: 0x0400362E RID: 13870
	<SerializeField()>
	Private startPhaseThreeShakeHoldover As Single = 4F

	' Token: 0x0400362F RID: 13871
	Private damageReceiver As DamageReceiver

	' Token: 0x04003630 RID: 13872
	Private startPos As Vector3

	' Token: 0x04003631 RID: 13873
	Private onLeft As Boolean

	' Token: 0x04003632 RID: 13874
	Private scale As Single

	' Token: 0x04003633 RID: 13875
	Private phaseOneEnded As Boolean

	' Token: 0x04003634 RID: 13876
	Public phaseTwoStarted As Boolean

	' Token: 0x04003635 RID: 13877
	Public preventAdditionalTurretLaunch As Boolean

	' Token: 0x04003636 RID: 13878
	Private phaseTwoHPPrediction As Single

	' Token: 0x04003637 RID: 13879
	Private strawberriesSpawnString As PatternString

	' Token: 0x04003638 RID: 13880
	Private strawberriesDelayString As PatternString

	' Token: 0x04003639 RID: 13881
	Private sugarcubesPhaseString As PatternString

	' Token: 0x0400363A RID: 13882
	Private sugarcubesDelayString As PatternString

	' Token: 0x0400363B RID: 13883
	Private sugarcubesParryString As PatternString

	' Token: 0x0400363C RID: 13884
	Private doughSpawnSidePatternString As PatternString

	' Token: 0x0400363D RID: 13885
	Private doughSpawnTypeString As PatternString

	' Token: 0x0400363E RID: 13886
	Private doughSpawnDelayString As PatternString

	' Token: 0x0400363F RID: 13887
	<SerializeField()>
	Private doughFXAnimator As Animator

	' Token: 0x04003640 RID: 13888
	Private limeHeightString As PatternString

	' Token: 0x04003641 RID: 13889
	Private limesDelayString As PatternString

	' Token: 0x04003642 RID: 13890
	<SerializeField()>
	Private BG As GameObject

	' Token: 0x04003643 RID: 13891
	<SerializeField()>
	Private phaseOneCollider As Collider2D

	' Token: 0x04003644 RID: 13892
	<SerializeField()>
	Private phaseOneRenderer As SpriteRenderer

	' Token: 0x04003645 RID: 13893
	<SerializeField()>
	Private timeToNextAttack As Single() = New Single(3) {}

	' Token: 0x04003646 RID: 13894
	<SerializeField()>
	Private mintHandAnimator As Animator

	' Token: 0x04003647 RID: 13895
	Private destroyOnPhaseEnd As List(Of GameObject) = New List(Of GameObject)()

	' Token: 0x04003648 RID: 13896
	Private attackCoroutines As List(Of Coroutine) = New List(Of Coroutine)()

	' Token: 0x04003649 RID: 13897
	<SerializeField()>
	Private reflectionCamera As GameObject

	' Token: 0x0400364A RID: 13898
	<SerializeField()>
	Private reflectionMaterial As Material

	' Token: 0x0400364B RID: 13899
	<SerializeField()>
	Private reflectionTexture As GameObject

	' Token: 0x020007D5 RID: 2005
	Public Enum State
		' Token: 0x0400364F RID: 13903
		Idle
		' Token: 0x04003650 RID: 13904
		Strawberries
		' Token: 0x04003651 RID: 13905
		Sugarcubes
		' Token: 0x04003652 RID: 13906
		Dough
		' Token: 0x04003653 RID: 13907
		Limes
	End Enum
End Class
