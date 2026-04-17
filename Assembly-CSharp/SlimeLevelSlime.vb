Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020007DD RID: 2013
Public Class SlimeLevelSlime
	Inherits LevelProperties.Slime.Entity

	' Token: 0x17000411 RID: 1041
	' (get) Token: 0x06002DEC RID: 11756 RVA: 0x001B12A3 File Offset: 0x001AF6A3
	' (set) Token: 0x06002DED RID: 11757 RVA: 0x001B12AA File Offset: 0x001AF6AA
	Public Shared Property TINIES As Boolean

	' Token: 0x17000412 RID: 1042
	' (get) Token: 0x06002DEE RID: 11758 RVA: 0x001B12B2 File Offset: 0x001AF6B2
	' (set) Token: 0x06002DEF RID: 11759 RVA: 0x001B12BA File Offset: 0x001AF6BA
	Public Property state As SlimeLevelSlime.State

	' Token: 0x17000413 RID: 1043
	' (get) Token: 0x06002DF0 RID: 11760 RVA: 0x001B12C3 File Offset: 0x001AF6C3
	' (set) Token: 0x06002DF1 RID: 11761 RVA: 0x001B12CB File Offset: 0x001AF6CB
	Public Property CurrentPropertyState As LevelProperties.Slime.State

	' Token: 0x06002DF2 RID: 11762 RVA: 0x001B12D4 File Offset: 0x001AF6D4
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		Me.onGroundY = MyBase.transform.position.y
		Me.shadowY = Me.shadow.transform.position.y
		SlimeLevelSlime.TINIES = False
		Me.shadow.enabled = False
		If Me.isBig Then
			For Each collider2D As Collider2D In MyBase.GetComponents(Of Collider2D)()
				collider2D.enabled = False
			Next
		End If
		For Each animator As Animator In Me.questionMarks
			animator.GetComponent(Of Collider2D)().enabled = False
		Next
	End Sub

	' Token: 0x06002DF3 RID: 11763 RVA: 0x001B13C9 File Offset: 0x001AF7C9
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06002DF4 RID: 11764 RVA: 0x001B13E4 File Offset: 0x001AF7E4
	Private Sub LateUpdate()
		Me.updateShadow(MyBase.transform.position.y - Me.onGroundY)
	End Sub

	' Token: 0x06002DF5 RID: 11765 RVA: 0x001B1411 File Offset: 0x001AF811
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06002DF6 RID: 11766 RVA: 0x001B1430 File Offset: 0x001AF830
	Public Overrides Sub LevelInit(properties As LevelProperties.Slime)
		MyBase.LevelInit(properties)
		Me.CurrentPropertyState = properties.CurrentState
		Me.jumpsBeforeFirstPunch = Me.CurrentPropertyState.jump.bigSlimeInitialJumpPunchCount
		If Me.isBig Then
			AddHandler properties.OnBossDeath, AddressOf Me.OnBossDeath
		End If
	End Sub

	' Token: 0x06002DF7 RID: 11767 RVA: 0x001B1484 File Offset: 0x001AF884
	Private Sub QuestionMarksOn()
		For Each animator As Animator In Me.questionMarks
			animator.transform.SetScale(New Single?(MyBase.transform.localScale.x), Nothing, Nothing)
			animator.SetBool("IsOn", True)
			animator.GetComponent(Of Collider2D)().enabled = True
		Next
	End Sub

	' Token: 0x06002DF8 RID: 11768 RVA: 0x001B1500 File Offset: 0x001AF900
	Private Sub QuestionMarksOff()
		For Each animator As Animator In Me.questionMarks
			If animator IsNot Nothing Then
				animator.SetBool("IsOn", False)
				animator.GetComponent(Of Collider2D)().enabled = False
			End If
		Next
	End Sub

	' Token: 0x06002DF9 RID: 11769 RVA: 0x001B1550 File Offset: 0x001AF950
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.dustPrefab = Nothing
		Me.explosionPrefab = Nothing
	End Sub

	' Token: 0x06002DFA RID: 11770 RVA: 0x001B1566 File Offset: 0x001AF966
	Public Sub IntroContinue()
		Me.StartJump()
	End Sub

	' Token: 0x06002DFB RID: 11771 RVA: 0x001B156E File Offset: 0x001AF96E
	Public Sub StartJump()
		Me.state = SlimeLevelSlime.State.Jump
		MyBase.StartCoroutine(Me.jump_cr())
	End Sub

	' Token: 0x06002DFC RID: 11772 RVA: 0x001B1584 File Offset: 0x001AF984
	Private Iterator Function jump_cr() As IEnumerator
		Dim p As LevelProperties.Slime.Jump = Me.CurrentPropertyState.jump
		Dim pattern As String() = p.patternString.Split(New Char() { ","c })
		Dim i As Integer = Global.UnityEngine.Random.Range(0, pattern.Length)
		Dim numJumpsLeft As Integer = 0
		If Me.firstTime AndAlso Me.isBig Then
			numJumpsLeft = p.bigSlimeInitialJumpPunchCount
			Me.firstTime = False
		Else
			numJumpsLeft = p.numJumps.RandomInt()
		End If
		MyBase.animator.SetTrigger("Jump")
		Dim delay As Single = p.groundDelay
		Me.playerToPunch = PlayerManager.GetNext()
		While True
			If pattern(i)(0) = "D"c Then
				Parser.FloatTryParse(pattern(i).Substring(1), delay)
			Else
				Yield MyBase.animator.WaitForAnimationToStart(Me, "Jump_Squish_Loop", False)
				If Me.isBig Then
					Me.BigJumpAudio()
				Else
					Me.SmallJumpAudio()
				End If
				Yield CupheadTime.WaitForSeconds(Me, delay)
				MyBase.animator.SetTrigger("Continue")
				Yield MyBase.animator.WaitForAnimationToStart(Me, "Up", False)
				Dim goingUp As Boolean = True
				Dim highJump As Boolean = pattern(i)(0) = "H"c
				If pattern(i)(0) = "R"c Then
					highJump = Rand.Bool()
				End If
				Me.velocityY = If((Not highJump), p.lowJumpVerticalSpeed, p.highJumpVerticalSpeed)
				Dim speedX As Single = If((Not highJump), p.lowJumpHorizontalSpeed, p.highJumpHorizontalSpeed)
				Me.gravity = If((Not highJump), p.lowJumpGravity, p.highJumpGravity)
				Me.inAir = True
				Dim moveDir As SlimeLevelSlime.Direction = Me.facingDirection
				Me.shadow.enabled = True
				While goingUp OrElse MyBase.transform.position.y > Me.onGroundY
					Me.velocityY -= Me.gravity * CupheadTime.FixedDelta * Me.hitPauseCoefficient()
					Dim velocityX As Single = If((moveDir <> SlimeLevelSlime.Direction.Left), speedX, (-speedX))
					MyBase.transform.AddPosition(velocityX * CupheadTime.FixedDelta * Me.hitPauseCoefficient(), Me.velocityY * CupheadTime.FixedDelta * Me.hitPauseCoefficient(), 0F)
					If Me.velocityY < 0F AndAlso goingUp Then
						goingUp = False
						MyBase.animator.SetTrigger("Apex")
					End If
					If(moveDir = SlimeLevelSlime.Direction.Left AndAlso MyBase.transform.position.x < -Me.maxX) OrElse (moveDir = SlimeLevelSlime.Direction.Right AndAlso MyBase.transform.position.x > Me.maxX) Then
						If moveDir = SlimeLevelSlime.Direction.Left Then
							MyBase.transform.SetPosition(New Single?(-Me.maxX), Nothing, Nothing)
							moveDir = SlimeLevelSlime.Direction.Right
						Else
							MyBase.transform.SetPosition(New Single?(Me.maxX), Nothing, Nothing)
							moveDir = SlimeLevelSlime.Direction.Left
						End If
						If Not goingUp Then
							speedX = 0F
						End If
						Me.Turn()
					End If
					Yield New WaitForFixedUpdate()
				End While
				MyBase.transform.SetPosition(Nothing, New Single?(Me.onGroundY), Nothing)
				Me.shadow.enabled = False
				Me.inAir = False
				delay = p.groundDelay
				Dim screenShakeCoefficient As Single = If((Not highJump), 1F, 1.5F)
				screenShakeCoefficient *= If((Not Me.isBig), 1F, 2F)
				CupheadLevelCamera.Current.Shake(5F * screenShakeCoefficient, 0.2F * screenShakeCoefficient, False)
				Me.dustPrefab.Create(MyBase.transform.position)
				If Me.wantsToTransform AndAlso MyBase.transform.position.x > -350F AndAlso MyBase.transform.position.x < 350F Then
					Exit For
				End If
				If Me.dieOnLand Then
					GoTo Block_23
				End If
				MyBase.animator.SetTrigger("Land")
				If Me.isBig AndAlso Not Me.firstPunch Then
					Me.jumpsBeforeFirstPunch -= 1
					If Me.jumpsBeforeFirstPunch = 0 Then
						GoTo Block_26
					End If
				Else
					numJumpsLeft -= 1
					If numJumpsLeft <= 0 AndAlso Me.inPunchPosition() Then
						GoTo Block_28
					End If
				End If
			End If
			i = (i + 1) Mod pattern.Length
		End While
		MyBase.animator.SetTrigger("Transform")
		Return
		Block_23:
		MyBase.animator.SetTrigger("LandingDeath")
		Me.state = SlimeLevelSlime.State.Dying
		Return
		Block_26:
		Me.firstPunch = True
		Yield Me.Punch()
		Return
		Block_28:
		Yield Me.Punch()
		Return
		Return
	End Function

	' Token: 0x06002DFD RID: 11773 RVA: 0x001B15A0 File Offset: 0x001AF9A0
	Private Iterator Function Punch() As IEnumerator
		If Me.playerToPunch Is Nothing OrElse Me.playerToPunch.IsDead Then
			Me.playerToPunch = PlayerManager.GetNext()
		End If
		Me.punchDirection = If((Me.playerToPunch.transform.position.x <= MyBase.transform.position.x), SlimeLevelSlime.Direction.Left, SlimeLevelSlime.Direction.Right)
		If Not Me.isBig Then
			MyBase.animator.SetTrigger("Continue")
			Yield MyBase.animator.WaitForAnimationToEnd(Me, "Jump_Squish_Loop", False, True)
		End If
		Me.StartPunch()
		Return
	End Function

	' Token: 0x06002DFE RID: 11774 RVA: 0x001B15BC File Offset: 0x001AF9BC
	Private Sub updateShadow(jumpY As Single)
		Me.shadow.transform.SetPosition(Nothing, New Single?(Me.shadowY), Nothing)
		Dim num As Single = Mathf.Clamp01(jumpY / Me.shadowMaxY)
		Dim component As Animator = Me.shadow.GetComponent(Of Animator)()
		component.Play("Idle", 0, num)
		component.speed = 0F
	End Sub

	' Token: 0x06002DFF RID: 11775 RVA: 0x001B1628 File Offset: 0x001AFA28
	Private Sub Turn()
		MyBase.animator.SetTrigger("Turn")
		MyBase.StartCoroutine(Me.turn_cr())
	End Sub

	' Token: 0x06002E00 RID: 11776 RVA: 0x001B1648 File Offset: 0x001AFA48
	Private Iterator Function turn_cr() As IEnumerator
		Dim upTurn As Integer = Animator.StringToHash(MyBase.animator.GetLayerName(0) + ".Up_Turn")
		Dim downTurn As Integer = Animator.StringToHash(MyBase.animator.GetLayerName(0) + ".Down_Turn")
		Dim startSquish As Integer = Animator.StringToHash(MyBase.animator.GetLayerName(0) + ".Jump_Squish_Start")
		While MyBase.animator.GetCurrentAnimatorStateInfo(0).fullPathHash <> upTurn AndAlso MyBase.animator.GetCurrentAnimatorStateInfo(0).fullPathHash <> downTurn AndAlso MyBase.animator.GetCurrentAnimatorStateInfo(0).fullPathHash <> startSquish
			Yield New WaitForEndOfFrame()
		End While
		While MyBase.animator.GetCurrentAnimatorStateInfo(0).fullPathHash = upTurn OrElse MyBase.animator.GetCurrentAnimatorStateInfo(0).fullPathHash = downTurn
			Yield New WaitForEndOfFrame()
		End While
		Me.facingDirection = If((Me.facingDirection <> SlimeLevelSlime.Direction.Left), SlimeLevelSlime.Direction.Left, SlimeLevelSlime.Direction.Right)
		MyBase.transform.SetScale(New Single?(CSng(If((Me.facingDirection <> SlimeLevelSlime.Direction.Right), 1, (-1)))), Nothing, Nothing)
		Return
	End Function

	' Token: 0x06002E01 RID: 11777 RVA: 0x001B1664 File Offset: 0x001AFA64
	Private Function inPunchPosition() As Boolean
		If Me.playerToPunch Is Nothing OrElse Me.playerToPunch.IsDead Then
			Me.playerToPunch = PlayerManager.GetNext()
		End If
		Return(Me.playerToPunch.transform.position.x > MyBase.transform.position.x AndAlso MyBase.transform.position.x < Me.punchMaxX AndAlso MyBase.transform.position.x > Me.punchMinX) OrElse (Me.playerToPunch.transform.position.x < MyBase.transform.position.x AndAlso MyBase.transform.position.x > -Me.punchMaxX AndAlso MyBase.transform.position.x < -Me.punchMinX)
	End Function

	' Token: 0x06002E02 RID: 11778 RVA: 0x001B177E File Offset: 0x001AFB7E
	Public Sub StartPunch()
		Me.state = SlimeLevelSlime.State.Punch
		MyBase.StartCoroutine(Me.punch_cr())
	End Sub

	' Token: 0x06002E03 RID: 11779 RVA: 0x001B1794 File Offset: 0x001AFB94
	Private Iterator Function punch_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 0F)
		Dim turn As Boolean = Me.punchDirection <> Me.facingDirection
		MyBase.animator.SetTrigger(If((Not turn), "StartPunch", "StartPunchTurn"))
		Yield MyBase.animator.WaitForAnimationToStart(Me, "Punch_Pre_Hold", False)
		Yield CupheadTime.WaitForSeconds(Me, Me.CurrentPropertyState.punch.preHold)
		MyBase.animator.SetTrigger("Continue")
		Yield MyBase.animator.WaitForAnimationToStart(Me, "Punch_Hold", False)
		Yield CupheadTime.WaitForSeconds(Me, Me.CurrentPropertyState.punch.mainHold)
		MyBase.animator.SetTrigger("Continue")
		Yield MyBase.animator.WaitForAnimationToStart(Me, "Punch_End", False)
		Me.BigPunchPlaying = False
		Me.StartJump()
		If Me.isBig Then
			MyBase.animator.SetBool("FirstPunch", False)
		End If
		Return
	End Function

	' Token: 0x06002E04 RID: 11780 RVA: 0x001B17B0 File Offset: 0x001AFBB0
	Private Sub PunchTurn()
		MyBase.animator.SetTrigger("Continue")
		Me.facingDirection = If((Me.facingDirection <> SlimeLevelSlime.Direction.Left), SlimeLevelSlime.Direction.Left, SlimeLevelSlime.Direction.Right)
		MyBase.transform.SetScale(New Single?(CSng(If((Me.facingDirection <> SlimeLevelSlime.Direction.Right), 1, (-1)))), Nothing, Nothing)
	End Sub

	' Token: 0x06002E05 RID: 11781 RVA: 0x001B181B File Offset: 0x001AFC1B
	Public Sub Transform()
		Me.wantsToTransform = True
	End Sub

	' Token: 0x06002E06 RID: 11782 RVA: 0x001B1824 File Offset: 0x001AFC24
	Public Sub TurnBig()
		Me.bigSlime.transform.position = MyBase.transform.position
		Me.bigSlime.StartJump()
		Me.bigSlime.facingDirection = Me.facingDirection
		Me.bigSlime.transform.localScale = MyBase.transform.localScale
		For Each collider2D As Collider2D In Me.bigSlime.GetComponents(Of Collider2D)()
			collider2D.enabled = True
		Next
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06002E07 RID: 11783 RVA: 0x001B18B9 File Offset: 0x001AFCB9
	Private Sub OnBossDeath()
		Me.Die(False)
	End Sub

	' Token: 0x06002E08 RID: 11784 RVA: 0x001B18C4 File Offset: 0x001AFCC4
	Public Sub DeathTransform()
		If Me.state <> SlimeLevelSlime.State.Dying Then
			RemoveHandler MyBase.properties.OnBossDeath, AddressOf Me.OnBossDeath
			If Me.inAir Then
				Me.dieOnLand = True
			Else
				Me.Die(False)
			End If
		End If
		MyBase.StartCoroutine(Me.transformToTombstone_cr())
	End Sub

	' Token: 0x06002E09 RID: 11785 RVA: 0x001B1920 File Offset: 0x001AFD20
	Private Iterator Function transformToTombstone_cr() As IEnumerator
		Yield MyBase.animator.WaitForAnimationToStart(Me, "Death_Loop", False)
		Yield CupheadTime.WaitForSeconds(Me, 3.5F)
		Me.tombStone.StartIntro(MyBase.transform.position.x)
		Return
	End Function

	' Token: 0x06002E0A RID: 11786 RVA: 0x001B193C File Offset: 0x001AFD3C
	Private Sub Die(earlyKnockout As Boolean)
		Me.StopAllCoroutines()
		Me.state = SlimeLevelSlime.State.Dying
		MyBase.animator.ResetTrigger("Continue")
		If earlyKnockout Then
			MyBase.animator.SetTrigger("EarlyKnockout")
			If Level.Current.mode = Level.Mode.Easy Then
				MyBase.properties.WinInstantly()
			Else
				MyBase.properties.DealDamageToNextNamedState()
			End If
		ElseIf Me.inAir Then
			MyBase.animator.SetTrigger("AirDeath")
			MyBase.StartCoroutine(Me.airDeath_cr())
		Else
			MyBase.animator.SetTrigger("GroundDeath")
		End If
	End Sub

	' Token: 0x06002E0B RID: 11787 RVA: 0x001B19E8 File Offset: 0x001AFDE8
	Private Iterator Function airDeath_cr() As IEnumerator
		Me.velocityY = Mathf.Min(0F, Me.velocityY)
		While MyBase.transform.position.y > Me.onGroundY
			Me.velocityY -= Me.gravity * CupheadTime.FixedDelta * Me.hitPauseCoefficient()
			MyBase.transform.AddPosition(0F, Me.velocityY * CupheadTime.FixedDelta * Me.hitPauseCoefficient(), 0F)
			Yield New WaitForFixedUpdate()
		End While
		MyBase.transform.SetPosition(Nothing, New Single?(Me.onGroundY), Nothing)
		Dim screenShakeCoefficient As Single = 2.5F
		CupheadLevelCamera.Current.Shake(5F * screenShakeCoefficient, 0.2F * screenShakeCoefficient, False)
		Me.dustPrefab.Create(MyBase.transform.position)
		Me.shadow.enabled = False
		MyBase.animator.SetTrigger("Continue")
		Return
	End Function

	' Token: 0x06002E0C RID: 11788 RVA: 0x001B1A03 File Offset: 0x001AFE03
	Private Function hitPauseCoefficient() As Single
		Return If((Not MyBase.GetComponent(Of DamageReceiver)().IsHitPaused), 1F, 0F)
	End Function

	' Token: 0x06002E0D RID: 11789 RVA: 0x001B1A24 File Offset: 0x001AFE24
	Public Sub Explode()
		Me.explosionPrefab.Create(MyBase.transform.position)
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06002E0E RID: 11790 RVA: 0x001B1A48 File Offset: 0x001AFE48
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		MyBase.properties.DealDamage(info.damage)
	End Sub

	' Token: 0x06002E0F RID: 11791 RVA: 0x001B1A5B File Offset: 0x001AFE5B
	Private Sub IntroAudio()
		AudioManager.Play("slime_small_intro_anim")
		Me.emitAudioFromObject.Add("slime_small_intro_anim")
		AudioManager.Play("slime_tiphat")
		Me.emitAudioFromObject.Add("slime_tiphat")
	End Sub

	' Token: 0x06002E10 RID: 11792 RVA: 0x001B1A91 File Offset: 0x001AFE91
	Private Sub BlinkAudio()
		AudioManager.Play("slime_blink")
	End Sub

	' Token: 0x06002E11 RID: 11793 RVA: 0x001B1A9D File Offset: 0x001AFE9D
	Private Sub SmallJumpAudio()
		AudioManager.Play("slime_small_jump")
	End Sub

	' Token: 0x06002E12 RID: 11794 RVA: 0x001B1AA9 File Offset: 0x001AFEA9
	Private Sub SmallLandAudio()
		AudioManager.Play("slime_small_land")
	End Sub

	' Token: 0x06002E13 RID: 11795 RVA: 0x001B1AB5 File Offset: 0x001AFEB5
	Private Sub SmallStretchPunchAudio()
		AudioManager.Play("slime_small_stretch_punch")
		Me.emitAudioFromObject.Add("slime_small_stretch_punch")
	End Sub

	' Token: 0x06002E14 RID: 11796 RVA: 0x001B1AD1 File Offset: 0x001AFED1
	Private Sub SmallTransformAudio()
		AudioManager.Play("slime_small_transform")
	End Sub

	' Token: 0x06002E15 RID: 11797 RVA: 0x001B1ADD File Offset: 0x001AFEDD
	Private Sub BigJumpAudio()
		AudioManager.Play("slime_big_jump")
	End Sub

	' Token: 0x06002E16 RID: 11798 RVA: 0x001B1AE9 File Offset: 0x001AFEE9
	Private Sub BigLandAudio()
		AudioManager.Play("slime_big_land")
	End Sub

	' Token: 0x06002E17 RID: 11799 RVA: 0x001B1AF8 File Offset: 0x001AFEF8
	Private Sub BigPunchAudio()
		If Not Me.BigPunchPlaying Then
			AudioManager.Play("slime_big_punch")
			Me.emitAudioFromObject.Add("slime_big_punch")
			AudioManager.Play("slime_big_punch_voice")
			Me.emitAudioFromObject.Add("slime_big_punch_voice")
			Me.BigPunchPlaying = True
		End If
	End Sub

	' Token: 0x06002E18 RID: 11800 RVA: 0x001B1B4B File Offset: 0x001AFF4B
	Private Sub BigDeathAudio()
		If Not Me.deathAudioPlayed Then
			AudioManager.Play("slime_big_death")
			AudioManager.Play("slime_big_death_voice")
			Me.deathAudioPlayed = True
		End If
	End Sub

	' Token: 0x0400366B RID: 13931
	<SerializeField()>
	Private questionMarks As Animator()

	' Token: 0x0400366D RID: 13933
	Private Const TRANSFORM_MAX_X As Single = 350F

	' Token: 0x0400366F RID: 13935
	Private facingDirection As SlimeLevelSlime.Direction

	' Token: 0x04003670 RID: 13936
	Private damageDealer As DamageDealer

	' Token: 0x04003671 RID: 13937
	Private damageReceiver As DamageReceiver

	' Token: 0x04003672 RID: 13938
	Private onGroundY As Single

	' Token: 0x04003673 RID: 13939
	Private shadowY As Single

	' Token: 0x04003674 RID: 13940
	Private wantsToTransform As Boolean

	' Token: 0x04003675 RID: 13941
	Private punchDirection As SlimeLevelSlime.Direction

	' Token: 0x04003676 RID: 13942
	Private inAir As Boolean

	' Token: 0x04003677 RID: 13943
	Private velocityY As Single

	' Token: 0x04003678 RID: 13944
	Private gravity As Single

	' Token: 0x04003679 RID: 13945
	Private dieOnLand As Boolean

	' Token: 0x0400367A RID: 13946
	Private deathAudioPlayed As Boolean

	' Token: 0x0400367B RID: 13947
	Private firstTime As Boolean = True

	' Token: 0x0400367C RID: 13948
	Private firstPunch As Boolean

	' Token: 0x0400367D RID: 13949
	Private BigPunchPlaying As Boolean

	' Token: 0x0400367E RID: 13950
	Private jumpsBeforeFirstPunch As Integer

	' Token: 0x04003680 RID: 13952
	Private playerToPunch As AbstractPlayerController

	' Token: 0x04003681 RID: 13953
	<SerializeField()>
	Private shadow As SpriteRenderer

	' Token: 0x04003682 RID: 13954
	<SerializeField()>
	Private shadowMaxY As Single

	' Token: 0x04003683 RID: 13955
	<SerializeField()>
	Private bigSlime As SlimeLevelSlime

	' Token: 0x04003684 RID: 13956
	<SerializeField()>
	Private tombStone As SlimeLevelTombstone

	' Token: 0x04003685 RID: 13957
	<SerializeField()>
	Private explosionPrefab As Effect

	' Token: 0x04003686 RID: 13958
	<SerializeField()>
	Private dustPrefab As Effect

	' Token: 0x04003687 RID: 13959
	<SerializeField()>
	Private isBig As Boolean

	' Token: 0x04003688 RID: 13960
	<SerializeField()>
	Private punchMaxX As Single

	' Token: 0x04003689 RID: 13961
	<SerializeField()>
	Private punchMinX As Single

	' Token: 0x0400368A RID: 13962
	<SerializeField()>
	Private maxX As Single

	' Token: 0x0400368B RID: 13963
	<SerializeField()>
	Private eyeMaxPosition As Transform

	' Token: 0x020007DE RID: 2014
	Public Enum State
		' Token: 0x0400368D RID: 13965
		Intro
		' Token: 0x0400368E RID: 13966
		Jump
		' Token: 0x0400368F RID: 13967
		Punch
		' Token: 0x04003690 RID: 13968
		Dying
	End Enum

	' Token: 0x020007DF RID: 2015
	Private Enum Direction
		' Token: 0x04003692 RID: 13970
		Left
		' Token: 0x04003693 RID: 13971
		Right
	End Enum
End Class
