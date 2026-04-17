Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Diagnostics
Imports UnityEngine

' Token: 0x020007FF RID: 2047
Public Class SnowCultLevelYeti
	Inherits LevelProperties.SnowCult.Entity

	' Token: 0x1400004E RID: 78
	' (add) Token: 0x06002F23 RID: 12067 RVA: 0x001BDC3C File Offset: 0x001BC03C
	' (remove) Token: 0x06002F24 RID: 12068 RVA: 0x001BDC74 File Offset: 0x001BC074
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnDeathEvent As Action

	' Token: 0x17000417 RID: 1047
	' (get) Token: 0x06002F25 RID: 12069 RVA: 0x001BDCAA File Offset: 0x001BC0AA
	' (set) Token: 0x06002F26 RID: 12070 RVA: 0x001BDCB2 File Offset: 0x001BC0B2
	Public Property inBallForm As Boolean

	' Token: 0x06002F27 RID: 12071 RVA: 0x001BDCBC File Offset: 0x001BC0BC
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.xScale = 1F
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		Me.ballDamageReceiver = Me.ball.GetComponent(Of DamageReceiver)()
		AddHandler Me.ballDamageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		Me.idleAnimFullPathHash = Animator.StringToHash(MyBase.animator.GetLayerName(0) + ".Idle")
		If Level.Current.mode <> Level.Mode.Easy Then
			Me.InitBats()
		Else
			AddHandler MyBase.properties.OnBossDeath, AddressOf Me.OnBossDeath
		End If
		AddHandler Me.ball.GetComponent(Of CollisionChild)().OnPlayerCollision, AddressOf Me.OnCollisionPlayer
	End Sub

	' Token: 0x06002F28 RID: 12072 RVA: 0x001BDD9F File Offset: 0x001BC19F
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06002F29 RID: 12073 RVA: 0x001BDDB8 File Offset: 0x001BC1B8
	Private Function InIdleAnim() As Boolean
		Return MyBase.animator.GetCurrentAnimatorStateInfo(0).fullPathHash = Me.idleAnimFullPathHash
	End Function

	' Token: 0x06002F2A RID: 12074 RVA: 0x001BDDE4 File Offset: 0x001BC1E4
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		If Level.Current.mode = Level.Mode.Easy AndAlso MyBase.properties.CurrentState.stateName = LevelProperties.SnowCult.States.EasyYeti AndAlso Not Me.InIdleAnim() AndAlso info.damage >= MyBase.properties.CurrentHealth Then
			Return
		End If
		MyBase.properties.DealDamage(info.damage)
	End Sub

	' Token: 0x06002F2B RID: 12075 RVA: 0x001BDE49 File Offset: 0x001BC249
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06002F2C RID: 12076 RVA: 0x001BDE68 File Offset: 0x001BC268
	Public Overrides Sub LevelInit(properties As LevelProperties.SnowCult)
		MyBase.LevelInit(properties)
		Me.offsetCoordIndex = Global.UnityEngine.Random.Range(0, properties.CurrentState.icePillar.offsetCoordString.Split(New Char() { ","c }).Length)
		Me.snowballMainIndex = Global.UnityEngine.Random.Range(0, properties.CurrentState.snowball.snowballTypeString.Length)
	End Sub

	' Token: 0x06002F2D RID: 12077 RVA: 0x001BDEC8 File Offset: 0x001BC2C8
	Public Sub StartOnLeft(reflectionPoint As Vector3)
		Me.yetiSpawnPoint.position = New Vector3(reflectionPoint.x + (reflectionPoint.x - Me.yetiSpawnPoint.position.x), Me.yetiSpawnPoint.position.y)
		MyBase.transform.localScale = New Vector3(-1F, 1F)
		Me.xScale = MyBase.transform.localScale.x
		Me.onLeft = True
	End Sub

	' Token: 0x06002F2E RID: 12078 RVA: 0x001BDF55 File Offset: 0x001BC355
	Public Sub StartYeti()
		MyBase.StartCoroutine(Me.intro_cr())
		If Level.Current.mode <> Level.Mode.Easy Then
			MyBase.StartCoroutine(Me.bats_attack_cr())
		End If
	End Sub

	' Token: 0x06002F2F RID: 12079 RVA: 0x001BDF80 File Offset: 0x001BC380
	Private Sub SetState(s As SnowCultLevelYeti.States)
		Me.previousState = Me.state
		Me.state = s
	End Sub

	' Token: 0x06002F30 RID: 12080 RVA: 0x001BDF98 File Offset: 0x001BC398
	Private Iterator Function intro_cr() As IEnumerator
		Me.state = SnowCultLevelYeti.States.Intro
		Me.introRibcageClosed = False
		MyBase.transform.position = Vector3.zero
		MyBase.transform.position = Me.yetiSpawnPoint.position
		MyBase.transform.position += Vector3.up * 300F
		Dim t As Single = 0F
		Me.sprite = MyBase.GetComponent(Of SpriteRenderer)()
		MyBase.animator.Play("Intro", 0, 0F)
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		While MyBase.animator.GetCurrentAnimatorStateInfo(0).fullPathHash = Animator.StringToHash(MyBase.animator.GetLayerName(0) + ".Intro")
			If t < 0.34F Then
				MyBase.transform.position = New Vector3(MyBase.transform.position.x, EaseUtils.EaseOutBack(Me.yetiSpawnPoint.position.y + 300F, Me.yetiSpawnPoint.position.y, t * 3F))
				t += CupheadTime.FixedDelta
			End If
			Me.introShadow.transform.position = New Vector3(MyBase.transform.position.x, -25F)
			Yield wait
		End While
		Me.SetState(SnowCultLevelYeti.States.Idle)
		MyBase.StartCoroutine(Me.do_patterns_cr())
		Yield Nothing
		Return
	End Function

	' Token: 0x06002F31 RID: 12081 RVA: 0x001BDFB3 File Offset: 0x001BC3B3
	Private Sub ShakeScreenInIntro()
		CupheadLevelCamera.Current.Shake(30F, 0.7F, False)
		CType(Level.Current, SnowCultLevel).YetiHitGround()
	End Sub

	' Token: 0x06002F32 RID: 12082 RVA: 0x001BDFD9 File Offset: 0x001BC3D9
	Public Sub RibcageClosedAroundWizard()
		Me.introRibcageClosed = True
	End Sub

	' Token: 0x06002F33 RID: 12083 RVA: 0x001BDFE4 File Offset: 0x001BC3E4
	Private Sub FlipSprite()
		MyBase.transform.SetScale(New Single?(If((Not Me.onLeft), Me.xScale, (-Me.xScale))), Nothing, Nothing)
	End Sub

	' Token: 0x06002F34 RID: 12084 RVA: 0x001BE030 File Offset: 0x001BC430
	Private Iterator Function do_patterns_cr() As IEnumerator
		Dim p As LevelProperties.SnowCult.Yeti = MyBase.properties.CurrentState.yeti
		Me.patternString = p.yetiPatternString.Split(New Char() { ","c })
		Me.patternStringIndex = Global.UnityEngine.Random.Range(0, Me.patternString.Length)
		While Not Me.forceOutroToStart
			Dim text As String = Me.patternString(Me.patternStringIndex)
			If text Is Nothing Then
				GoTo IL_0144
			End If
			If Not(text = "S") Then
				If Not(text = "J") Then
					If Not(text = "L") Then
						If Not(text = "P") Then
							GoTo IL_0144
						End If
						Me.StartIcePillar()
					Else
						Me.Snowball()
					End If
				Else
					MyBase.StartCoroutine(Me.start_jump_cr())
				End If
			Else
				MyBase.StartCoroutine(Me.start_dash_cr())
			End If
			IL_0154:
			While Me.state <> SnowCultLevelYeti.States.Idle
				Yield Nothing
			End While
			Me.patternStringIndex = (Me.patternStringIndex + 1) Mod Me.patternString.Length
			Yield Nothing
			Continue While
			IL_0144:
			Me.Snowball()
			GoTo IL_0154
		End While
		Return
	End Function

	' Token: 0x06002F35 RID: 12085 RVA: 0x001BE04C File Offset: 0x001BC44C
	Private Function PeekNextPattern() As String
		If Me.forceOutroToStart OrElse Level.Current.mode = Level.Mode.Easy Then
			Return "I"
		End If
		Dim text As String = Me.patternString((Me.patternStringIndex + 1) Mod Me.patternString.Length)
		If text IsNot Nothing Then
			If text = "S" OrElse text = "J" Then
				Return Me.patternString((Me.patternStringIndex + 1) Mod Me.patternString.Length)
			End If
			If text = "L" OrElse text = "P" Then
				Return "I"
			End If
		End If
		Return Nothing
	End Function

	' Token: 0x06002F36 RID: 12086 RVA: 0x001BE100 File Offset: 0x001BC500
	Private Iterator Function cue_reform_effect_cr(delayTime As Single, position As Single, clipName As String) As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, delayTime - 0.9583333F)
		Me.meltFXAnimator(1).gameObject.transform.SetPosition(New Single?(position), Nothing, Nothing)
		Me.meltFXAnimator(1).gameObject.transform.localScale = New Vector3(MyBase.transform.localScale.x * -1F, 1F)
		Me.meltFXAnimator(1).gameObject.SetActive(True)
		Me.meltFXAnimator(1).Play(clipName)
		Return
	End Function

	' Token: 0x06002F37 RID: 12087 RVA: 0x001BE130 File Offset: 0x001BC530
	Private Iterator Function start_dash_cr() As IEnumerator
		Me.inBallForm = True
		Dim PRE_DASH_TIME As Single = 0.25F
		Dim DASH_TIME As Single = 0.375F
		Dim p As LevelProperties.SnowCult.Yeti = MyBase.properties.CurrentState.yeti
		Dim start As Single = If((Not Me.onLeft), 493F, (-493F))
		Dim [end] As Single = If((Not Me.onLeft), (-493F), 493F)
		start += Me.ball.transform.localPosition.x * -MyBase.transform.localScale.x * 2F
		Dim t As Single = 0F
		Dim time As Single = p.slideTime
		If Me.previousState <> SnowCultLevelYeti.States.Move OrElse Level.Current.mode = Level.Mode.Easy Then
			MyBase.animator.Play("IdleToDash")
		End If
		Me.SetState(SnowCultLevelYeti.States.Move)
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		Yield MyBase.animator.WaitForAnimationToStart(Me, "PreDash", False)
		MyBase.StartCoroutine(Me.cue_reform_effect_cr(PRE_DASH_TIME + p.slideWarning + DASH_TIME + time, [end], "DashReformEffect"))
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "PreDash", False, True)
		Yield CupheadTime.WaitForSeconds(Me, p.slideWarning)
		MyBase.animator.Play("Dash")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Dash", False, True)
		Me.meltFXAnimator(0).transform.position = MyBase.transform.position
		Me.meltFXAnimator(0).transform.localScale = MyBase.transform.localScale
		Me.meltFXAnimator(0).gameObject.SetActive(True)
		Me.meltFXAnimator(0).Play("DashMeltEffect")
		Me.meltFXAnimator(0).transform.parent = Nothing
		Yield Nothing
		MyBase.transform.SetPosition(New Single?(start), Nothing, Nothing)
		Me.ball.transform.localPosition = Me.BALL_DASH_OFFSET
		Me.ball.SetActive(True)
		Me.dashGroundFX.SetActive(True)
		Me.groundMask.SetActive(True)
		Me.sprite.enabled = False
		Me.coll.enabled = False
		MyBase.animator.Play("DashBall", 1, 0F)
		While t < time
			If t < time - 0.9583333F AndAlso t + CupheadTime.FixedDelta >= time - 0.9583333F Then
				Me.meltFXAnimator(1).gameObject.transform.SetPosition(New Single?([end]), Nothing, Nothing)
				Me.meltFXAnimator(1).gameObject.transform.localScale = New Vector3(MyBase.transform.localScale.x * -1F, 1F)
				Me.meltFXAnimator(1).gameObject.SetActive(True)
				Me.meltFXAnimator(1).Play("DashReformEffect")
			End If
			t += CupheadTime.FixedDelta
			MyBase.transform.SetPosition(New Single?(Mathf.Lerp(start, [end], t / time)), Nothing, Nothing)
			Yield wait
		End While
		Me.onLeft = Not Me.onLeft
		Me.FlipSprite()
		Me.sprite.enabled = True
		Me.coll.enabled = True
		Me.ball.SetActive(False)
		Me.dashGroundFX.SetActive(False)
		Me.groundMask.SetActive(False)
		Me.meltFXAnimator(1).gameObject.SetActive(False)
		Dim text As String = Me.PeekNextPattern()
		If text IsNot Nothing Then
			If Not(text = "I") Then
				If Not(text = "S") Then
					If text = "J" Then
						MyBase.animator.Play("DashToJump")
						Yield MyBase.animator.WaitForAnimationToEnd(Me, "DashToJump", False, True)
					End If
				Else
					MyBase.animator.Play("DashToDash")
					Yield MyBase.animator.WaitForAnimationToEnd(Me, "DashToDash", False, True)
				End If
			Else
				MyBase.animator.Play("DashToIdle")
				Yield MyBase.animator.WaitForAnimationToEnd(Me, "DashToIdle", False, True)
			End If
		End If
		If Me.PeekNextPattern() = "I" Then
			Yield CupheadTime.WaitForSeconds(Me, If((Not Me.forceOutroToStart), p.hesitate, 0F))
		End If
		Me.SetState(SnowCultLevelYeti.States.Idle)
		Me.inBallForm = False
		Return
	End Function

	' Token: 0x06002F38 RID: 12088 RVA: 0x001BE14C File Offset: 0x001BC54C
	Private Iterator Function start_jump_cr() As IEnumerator
		Me.inBallForm = True
		Dim p As LevelProperties.SnowCult.Yeti = MyBase.properties.CurrentState.yeti
		Dim PRE_JUMP_TIME As Single = 0.20833333F
		Dim JUMP_TIME As Single = 0.25F
		Dim endArcPosX As Single = If((Not Me.onLeft), (-393F), 393F)
		Dim reformPosX As Single = If((Not Me.onLeft), (-493F), 493F)
		Dim xDistance As Single = endArcPosX - MyBase.transform.position.x
		Dim ground As Single = MyBase.transform.position.y
		Dim timeToApex As Single = p.jumpApexTime
		Dim height As Single = p.jumpApexHeight
		Dim apexTime2 As Single = timeToApex * timeToApex
		Dim g As Single = -2F * height / apexTime2
		Dim viY As Single = 2F * height / timeToApex
		Dim viX2 As Single = viY * viY
		Dim sqrtRooted As Single = viX2 + 2F * g * ground
		Dim tEnd As Single = (-viY + Mathf.Sqrt(sqrtRooted)) / g
		Dim tEnd2 As Single = (-viY - Mathf.Sqrt(sqrtRooted)) / g
		Dim tEnd3 As Single = Mathf.Max(tEnd, tEnd2)
		Dim velocityX As Single = xDistance / tEnd3
		Dim speed As Vector3 = New Vector3(velocityX, viY)
		Dim t As Single = 0F
		If Me.previousState <> SnowCultLevelYeti.States.Move OrElse Level.Current.mode = Level.Mode.Easy Then
			MyBase.animator.Play("IdleToJump")
		End If
		Me.SetState(SnowCultLevelYeti.States.Move)
		Yield MyBase.animator.WaitForAnimationToStart(Me, "PreJump", False)
		MyBase.StartCoroutine(Me.cue_reform_effect_cr(PRE_JUMP_TIME + p.jumpWarning + JUMP_TIME + tEnd3, reformPosX, "JumpReformEffect"))
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "PreJump", False, True)
		Yield CupheadTime.WaitForSeconds(Me, p.jumpWarning)
		MyBase.animator.Play("Jump")
		Me.ball.transform.localPosition = Me.BALL_JUMP_OFFSET
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Jump", False, True)
		Me.meltFXAnimator(0).transform.position = MyBase.transform.position
		Me.meltFXAnimator(0).transform.localScale = MyBase.transform.localScale
		Me.meltFXAnimator(0).gameObject.SetActive(True)
		Me.meltFXAnimator(0).Play("JumpMeltEffect")
		Me.meltFXAnimator(0).transform.parent = Nothing
		MyBase.transform.position += Vector3.right * (Me.ball.transform.localPosition.x * -MyBase.transform.localScale.x * 2F)
		Me.ball.SetActive(True)
		Me.ballShadow.sprite = Me.shadowSprites(0)
		Me.ballShadow.enabled = True
		Me.sprite.enabled = False
		Me.coll.enabled = False
		MyBase.animator.Play("JumpBall", 1, 0F)
		Dim stillMoving As Boolean = True
		While stillMoving
			speed += New Vector3(0F, g * CupheadTime.FixedDelta)
			MyBase.transform.Translate(speed * CupheadTime.FixedDelta)
			Yield New WaitForFixedUpdate()
			Me.ballShadow.transform.SetPosition(New Single?(Me.ball.transform.position.x), New Single?(ground - 145F), Nothing)
			Me.ballShadow.sprite = Me.shadowSprites(Mathf.Clamp(CInt(((MyBase.transform.position.y - ground) / height * CSng(Me.shadowSprites.Length))), 0, Me.shadowSprites.Length - 1))
			t += CupheadTime.FixedDelta
			If t > timeToApex AndAlso MyBase.transform.position.y <= ground Then
				stillMoving = False
			End If
		End While
		MyBase.transform.SetPosition(New Single?(reformPosX), New Single?(ground), Nothing)
		MyBase.transform.SetEulerAngles(Nothing, Nothing, New Single?(0F))
		Me.onLeft = Not Me.onLeft
		Me.FlipSprite()
		Me.sprite.enabled = True
		Me.coll.enabled = True
		Me.ballShadow.enabled = False
		Me.ball.SetActive(False)
		Me.meltFXAnimator(1).gameObject.SetActive(False)
		Dim text As String = Me.PeekNextPattern()
		If text IsNot Nothing Then
			If Not(text = "I") Then
				If Not(text = "S") Then
					If text = "J" Then
						MyBase.animator.Play("JumpToJump")
						Yield MyBase.animator.WaitForAnimationToEnd(Me, "JumpToJump", False, True)
					End If
				Else
					MyBase.animator.Play("JumpToDash")
					Yield MyBase.animator.WaitForAnimationToEnd(Me, "JumpToDash", False, True)
				End If
			Else
				MyBase.animator.Play("JumpToIdle")
				Yield MyBase.animator.WaitForAnimationToEnd(Me, "JumpToIdle", False, True)
			End If
		End If
		If Me.PeekNextPattern() = "I" Then
			Yield CupheadTime.WaitForSeconds(Me, If((Not Me.forceOutroToStart), p.hesitate, 0F))
		End If
		Me.SetState(SnowCultLevelYeti.States.Idle)
		Me.inBallForm = False
		Return
	End Function

	' Token: 0x06002F39 RID: 12089 RVA: 0x001BE167 File Offset: 0x001BC567
	Public Sub StartIcePillar()
		Me.SetState(SnowCultLevelYeti.States.IcePillar)
		MyBase.animator.SetTrigger("OnSmash")
	End Sub

	' Token: 0x06002F3A RID: 12090 RVA: 0x001BE180 File Offset: 0x001BC580
	Private Sub SpawnIcePillars()
		CupheadLevelCamera.Current.Shake(30F, 0.7F, False)
		Me.snowCultBGHandler.CandleGust()
		Dim vector As Vector3 = New Vector3(MyBase.transform.position.x + 290F * CSng(If((Not Me.onLeft), (-1), 1)), 95F)
		Me.snowBurstA.Create(vector, CSng(If((Not Me.onLeft), (-1), 1)))
		MyBase.StartCoroutine(Me.spawn_snowfall_cr())
		MyBase.StartCoroutine(Me.ice_pillar_cr())
	End Sub

	' Token: 0x06002F3B RID: 12091 RVA: 0x001BE220 File Offset: 0x001BC620
	Private Iterator Function ice_pillar_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 0.25F)
		Dim p As LevelProperties.SnowCult.IcePillar = MyBase.properties.CurrentState.icePillar
		Dim offset As Single = 0F
		Dim dir As Integer = If((Not Me.onLeft), (-1), 1)
		Dim type As Boolean = Rand.Bool()
		Parser.FloatTryParse(p.offsetCoordString.Split(New Char() { ","c })(Me.offsetCoordIndex), offset)
		For i As Integer = 0 To p.icePillarCount - 1
			Dim pos As Vector3 = New Vector3(Me.yetiMidPoint.position.x + offset * CSng(dir) + p.icePillarSpacing * CSng(i) * CSng(dir), -142F)
			Dim icePillar As SnowCultLevelIcePillar = Me.icePillarPrefab.Spawn()
			icePillar.Init(pos, p, type, p.appearDelay * CSng((i + 1)))
			type = Not type
			Yield CupheadTime.WaitForSeconds(Me, p.appearDelay)
		Next
		Me.offsetCoordIndex = (Me.offsetCoordIndex + 1) Mod p.offsetCoordString.Split(New Char() { ","c }).Length
		Yield CupheadTime.WaitForSeconds(Me, If((Not Me.forceOutroToStart), p.hesitate, 0F))
		Me.SetState(SnowCultLevelYeti.States.Idle)
		Yield Nothing
		Return
	End Function

	' Token: 0x06002F3C RID: 12092 RVA: 0x001BE23C File Offset: 0x001BC63C
	Private Iterator Function spawn_snowfall_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		Dim pos As Vector3 = New Vector3(MyBase.transform.position.x + 290F * CSng(If((Not Me.onLeft), (-1), 1)), 510F)
		Me.snowFallA.Create(pos, CSng(If((Not Me.onLeft), (-1), 1)))
		Yield Nothing
		Return
	End Function

	' Token: 0x06002F3D RID: 12093 RVA: 0x001BE258 File Offset: 0x001BC658
	Private Sub InitBats()
		Me.batAttackPositionString = New PatternString(MyBase.properties.CurrentState.snowball.batAttackPosition, True)
		Me.batAttackHeightString = New PatternString(MyBase.properties.CurrentState.snowball.batAttackHeight, True)
		Me.batAttackWidthString = New PatternString(MyBase.properties.CurrentState.snowball.batAttackWidth, True)
		Me.batAttackSideString = New PatternString(MyBase.properties.CurrentState.snowball.batAttackSide, True)
		Me.batAttackInterDelayString = New PatternString(MyBase.properties.CurrentState.snowball.batAttackInterDelay, True)
		Me.batArcModifierString = New PatternString(MyBase.properties.CurrentState.snowball.batArcModifier, True)
		Me.batParryableString = New PatternString(MyBase.properties.CurrentState.snowball.batParryableString, True)
	End Sub

	' Token: 0x06002F3E RID: 12094 RVA: 0x001BE34C File Offset: 0x001BC74C
	Private Iterator Function bats_attack_cr() As IEnumerator
		Dim p As LevelProperties.SnowCult.Snowball = MyBase.properties.CurrentState.snowball
		Me.batLaunchTimer = Me.batAttackInterDelayString.PopFloat()
		Dim player As AbstractPlayerController = PlayerManager.GetNext()
		While True
			While Me.batCirclingList.Count > 0
				Dim which As Integer = Global.UnityEngine.Random.Range(0, Me.batCirclingList.Count)
				If Me.batCirclingList(which) IsNot Nothing AndAlso Me.batCirclingList(which).reachedCircle Then
					While Me.batLaunchTimer > 0F
						Me.batLaunchTimer -= CupheadTime.Delta
						Yield Nothing
					End While
					If Me.batCirclingList.Count > which AndAlso Me.batCirclingList(which) IsNot Nothing Then
						Me.batLaunchTimer = Me.batAttackInterDelayString.PopFloat()
						Dim num As Single = Me.batAttackHeightString.PopFloat()
						Dim num2 As Single = Me.batAttackWidthString.PopFloat()
						Dim position As Vector3 = Me.batAttackPositions(Me.batAttackPositionString.PopInt()).position
						position.x *= CSng(If((Not Me.onLeft), (-1), 1))
						num2 *= CSng(If((Not Me.onLeft), 1, (-1)))
						Dim flag As Boolean = Me.batAttackSideString.PopLetter() = "S"c
						position.x *= CSng(If((Not flag), 1, (-1)))
						num2 *= CSng(If((Not flag), 1, (-1)))
						Me.batCirclingList(which).AttackPlayer(position, num, num2, Me.batArcModifierString.PopFloat())
						Me.batCirclingList.RemoveAt(which)
					End If
				ElseIf Me.batCirclingList(which) Is Nothing Then
					Me.batCirclingList.RemoveAt(which)
				End If
				Yield Nothing
				player = PlayerManager.GetNext()
			End While
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06002F3F RID: 12095 RVA: 0x001BE367 File Offset: 0x001BC767
	Public Sub ReturnBatToList(bat As SnowCultLevelBat)
		Me.batCirclingList.Add(bat)
	End Sub

	' Token: 0x06002F40 RID: 12096 RVA: 0x001BE378 File Offset: 0x001BC778
	Private Iterator Function spawn_bats_cr() As IEnumerator
		Me.batSpawnEffectPrefab.Create(MyBase.transform.position + Vector3.up * 180F + Vector3.right * CSng(If((Not Me.onLeft), (-20), 20)))
		If Me.bats Is Nothing Then
			Me.bats = New SnowCultLevelBat(MyBase.properties.CurrentState.snowball.batCount - 1) {}
		End If
		For j As Integer = 0 To Me.batCirclingList.Count - 1
			Global.UnityEngine.[Object].Destroy(Me.batCirclingList(j).gameObject)
		Next
		Yield Nothing
		Me.batCirclingList.RemoveAll(Function(b As SnowCultLevelBat) b Is Nothing)
		Me.SFX_SNOWCULT_YetiFreezerScream()
		For i As Integer = 0 To Me.bats.Length - 1
			If Me.bats(i) Is Nothing OrElse Me.bats(i).gameObject Is Nothing OrElse Not Me.bats(i).gameObject.activeInHierarchy Then
				Dim launchVelocity As Vector3 = New Vector3(CSng(If((Not Me.onLeft), (-1), 1)), 0.25F)
				launchVelocity *= CSng(Global.UnityEngine.Random.Range(500, 800))
				launchVelocity = Quaternion.Euler(0F, 0F, CSng(Global.UnityEngine.Random.Range(-30, 30))) * launchVelocity
				Me.bats(i) = Me.batPrefab.Spawn()
				Dim parryable As Boolean = Me.batParryableString.PopLetter() = "P"c
				Me.bats(i).Init(MyBase.transform.position + Vector3.up * 180F, launchVelocity, MyBase.properties.CurrentState.snowball, Me, parryable, If((Not parryable), If((Not Me.batColor), "Yellow", String.Empty), "Pink"))
				If Not parryable Then
					Me.batColor = Not Me.batColor
				End If
				Me.batCirclingList.Add(Me.bats(i))
				Yield CupheadTime.WaitForSeconds(Me, 0.125F)
			End If
		Next
		Me.batLaunchTimer = MyBase.properties.CurrentState.snowball.batInitialDelay
		Return
	End Function

	' Token: 0x06002F41 RID: 12097 RVA: 0x001BE394 File Offset: 0x001BC794
	Private Sub RemoveBats()
		If Me.bats IsNot Nothing Then
			For i As Integer = 0 To Me.bats.Length - 1
				If Me.bats(i) IsNot Nothing Then
					Me.bats(i).Dead()
				End If
			Next
		End If
	End Sub

	' Token: 0x06002F42 RID: 12098 RVA: 0x001BE3E5 File Offset: 0x001BC7E5
	Public Sub Snowball()
		MyBase.StartCoroutine(Me.snowball_cr())
	End Sub

	' Token: 0x06002F43 RID: 12099 RVA: 0x001BE3F4 File Offset: 0x001BC7F4
	Private Function BreakOutOfFridge() As Boolean
		Return Me.forceOutroToStart OrElse MyBase.properties.CurrentState.stateName = LevelProperties.SnowCult.States.EasyYeti
	End Function

	' Token: 0x06002F44 RID: 12100 RVA: 0x001BE417 File Offset: 0x001BC817
	Public Sub FridgeCanShoot()
		Me.fridgeCanShoot = True
	End Sub

	' Token: 0x06002F45 RID: 12101 RVA: 0x001BE420 File Offset: 0x001BC820
	Public Function GetIceCubeStartFrame() As Single
		Me.iceCubeStartFrame = (Me.iceCubeStartFrame + 1) Mod 3
		Select Case Me.iceCubeStartFrame
			Case Else
				Return 0F
			Case 1
				Return 2F
			Case 2
				Return 5F
		End Select
	End Function

	' Token: 0x06002F46 RID: 12102 RVA: 0x001BE46C File Offset: 0x001BC86C
	Public Function GetMediumExplosion() As Integer
		Me.iceCubeExplosionCounterMedium = (Me.iceCubeExplosionCounterMedium + 1) Mod 2
		Return Me.iceCubeExplosionCounterMedium
	End Function

	' Token: 0x06002F47 RID: 12103 RVA: 0x001BE484 File Offset: 0x001BC884
	Public Function GetSmallExplosion() As Integer
		Me.iceCubeExplosionCounterSmall = (Me.iceCubeExplosionCounterSmall + 1) Mod 3
		Return Me.iceCubeExplosionCounterSmall
	End Function

	' Token: 0x06002F48 RID: 12104 RVA: 0x001BE49C File Offset: 0x001BC89C
	Private Iterator Function snowball_cr() As IEnumerator
		Me.SetState(SnowCultLevelYeti.States.Snowball)
		Me.fridgeCanShoot = False
		MyBase.animator.SetTrigger("OnFridgeMorph")
		Dim p As LevelProperties.SnowCult.Snowball = MyBase.properties.CurrentState.snowball
		Dim snowballType As String() = p.snowballTypeString(Me.snowballMainIndex).Split(New Char() { ","c })
		Dim target As Integer = Animator.StringToHash(MyBase.animator.GetLayerName(0) + ".Idle")
		While MyBase.animator.GetCurrentAnimatorStateInfo(0).fullPathHash = target
			If Me.BreakOutOfFridge() Then
				MyBase.animator.ResetTrigger("OnFridgeMorph")
				Me.SetState(SnowCultLevelYeti.States.Idle)
				Return
			End If
			Yield Nothing
		End While
		Dim count As Integer = snowballType.Length
		Dim t As Single
		For i As Integer = 0 To count - 1
			If Me.BreakOutOfFridge() Then
				Exit For
			End If
			While Not Me.fridgeCanShoot AndAlso Not Me.forceOutroToStart
				Yield Nothing
			End While
			Me.fridgeCanShoot = False
			If Not Me.BreakOutOfFridge() Then
				MyBase.animator.Play("FridgeShoot")
				Me.SFX_SNOWCULT_YetiFreezerIceCubeLaunch()
				Dim [next] As AbstractPlayerController = PlayerManager.GetNext()
				Dim num As Single = p.shotMaxAngle
				Dim num2 As Single = p.shotMaxSpeed
				Dim num3 As Single = Single.MaxValue
				Dim vector As Vector2 = New Vector2([next].transform.position.x, CSng(Level.Current.Ground)) - Me.cubeLaunchPosition.transform.position
				vector.x = Mathf.Abs(vector.x)
				Dim minMax As MinMax = New MinMax(p.shotMinAngle, p.shotMaxAngle)
				Dim minMax2 As MinMax = New MinMax(p.shotMinSpeed, p.shotMaxSpeed)
				If vector.y > 0F Then
					Dim num4 As Single = minMax2.max / p.shotGravity
					Dim num5 As Single = minMax2.max * num4 - 0.5F * p.shotGravity * num4 * num4
					Dim num6 As Single = num5 + vector.y * 0F
					Dim num7 As Single = Mathf.Sqrt(2F * num6 / p.shotGravity)
					minMax2.max = num7 * p.shotGravity
					minMax2.min *= minMax2.max / p.shotMaxSpeed
				End If
				Dim num8 As Single = 0F
				While num8 < 1F
					Dim floatAt As Single = minMax.GetFloatAt(num8)
					Dim floatAt2 As Single = minMax2.GetFloatAt(num8)
					Dim vector2 As Vector2 = MathUtils.AngleToDirection(floatAt) * floatAt2
					t = vector.x / vector2.x
					Dim num9 As Single = vector2.y * t - 0.5F * p.shotGravity * t * t
					Dim num10 As Single = Mathf.Abs(vector.y - num9)
					If p.shotGravity <= 0.01F Then
						GoTo IL_043C
					End If
					Dim num11 As Single = vector2.y - p.shotGravity * t
					If num11 <= 0F Then
						GoTo IL_043C
					End If
					IL_0450:
					num8 += 0.01F
					Continue While
					IL_043C:
					If num10 < num3 Then
						num3 = num10
						num = floatAt
						num2 = floatAt2
						GoTo IL_0450
					End If
					GoTo IL_0450
				End While
				If [next].transform.position.x < MyBase.transform.position.x Then
					num = 180F - num
				End If
				Dim snowCultLevelSnowball As SnowCultLevelSnowball = Nothing
				If snowballType(i)(0) = "S"c Then
					snowCultLevelSnowball = Me.smallSnowballPrefab.Spawn()
				ElseIf snowballType(i)(0) = "M"c Then
					snowCultLevelSnowball = Me.mediumSnowballPrefab.Spawn()
				ElseIf snowballType(i)(0) = "L"c Then
					snowCultLevelSnowball = Me.largeSnowballPrefab.Spawn()
				End If
				snowCultLevelSnowball.InitOriginal(Me.cubeLaunchPosition.transform.position, p.shotGravity, num2, num, p, Me)
				If i = snowballType.Length - 1 AndAlso Level.Current.mode = Level.Mode.Easy AndAlso Not Me.BreakOutOfFridge() Then
					i = -1
					Me.snowballMainIndex = (Me.snowballMainIndex + 1) Mod p.snowballTypeString.Length
					snowballType = p.snowballTypeString(Me.snowballMainIndex).Split(New Char() { ","c })
					count = snowballType.Length
				End If
			End If
			If Not Me.BreakOutOfFridge() AndAlso i < count - 1 Then
				Yield CupheadTime.WaitForSeconds(Me, p.snowballThrowDelay)
			End If
		Next
		t = 0F
		While t < p.batLaunchDelay AndAlso Not Me.BreakOutOfFridge()
			t += CupheadTime.Delta
			Yield Nothing
		End While
		If Not Me.BreakOutOfFridge() Then
			MyBase.animator.SetTrigger("OnFridgeOutro")
			Yield MyBase.animator.WaitForAnimationToStart(Me, "FridgeOutroLoop", False)
		Else
			Yield MyBase.animator.WaitForAnimationToEnd(Me, "FridgeShoot", False, False)
			If MyBase.animator.GetCurrentAnimatorStateInfo(0).fullPathHash = Animator.StringToHash(MyBase.animator.GetLayerName(0) + ".FridgeIdle") Then
				While MyBase.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.2777778F AndAlso MyBase.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.8333333F
					Yield Nothing
				End While
				MyBase.animator.Play("FridgeOutroOpen")
			End If
		End If
		If Not Me.forceOutroToStart Then
			Yield MyBase.StartCoroutine(Me.spawn_bats_cr())
			Yield CupheadTime.WaitForSeconds(Me, 0.1F)
			Me.snowballMainIndex = (Me.snowballMainIndex + 1) Mod p.snowballTypeString.Length
			MyBase.animator.Play("FridgeOutroMorph")
		End If
		Yield CupheadTime.WaitForSeconds(Me, If((Not Me.BreakOutOfFridge()), p.hesitate, 0F))
		Yield MyBase.animator.WaitForAnimationToStart(Me, "Idle", False)
		Me.SetState(SnowCultLevelYeti.States.Idle)
		Yield Nothing
		Return
	End Function

	' Token: 0x06002F49 RID: 12105 RVA: 0x001BE4B8 File Offset: 0x001BC8B8
	Public Sub ToEasyPhaseThree()
		Dim yeti As LevelProperties.SnowCult.Yeti = MyBase.properties.CurrentState.yeti
		Me.patternString = yeti.yetiPatternString.Split(New Char() { ","c })
		Me.patternStringIndex = Global.UnityEngine.Random.Range(0, Me.patternString.Length)
		Me.InitBats()
		MyBase.StartCoroutine(Me.bats_attack_cr())
	End Sub

	' Token: 0x06002F4A RID: 12106 RVA: 0x001BE519 File Offset: 0x001BC919
	Public Sub ForceOutroToStart()
		MyBase.animator.SetBool("ForceOutro", True)
		Me.forceOutroToStart = True
	End Sub

	' Token: 0x06002F4B RID: 12107 RVA: 0x001BE533 File Offset: 0x001BC933
	Private Sub OnBossDeath()
		Me.StopAllCoroutines()
		MyBase.animator.Play("DeathEasy")
	End Sub

	' Token: 0x06002F4C RID: 12108 RVA: 0x001BE54B File Offset: 0x001BC94B
	Public Sub OnDeath()
		MyBase.animator.SetBool("Dead", True)
		Me.StopAllCoroutines()
		Me.RemoveBats()
		If Me.OnDeathEvent IsNot Nothing Then
			Me.OnDeathEvent()
		End If
	End Sub

	' Token: 0x06002F4D RID: 12109 RVA: 0x001BE580 File Offset: 0x001BC980
	Public Sub ActivateLegs()
		Me.bucket.transform.parent = Nothing
		Me.legs.transform.parent = Nothing
		Me.legs.SetActive(True)
	End Sub

	' Token: 0x06002F4E RID: 12110 RVA: 0x001BE5B0 File Offset: 0x001BC9B0
	Public Sub DeathAnimationEnded()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06002F4F RID: 12111 RVA: 0x001BE5BD File Offset: 0x001BC9BD
	Private Sub AnimationEvent_SFX_SNOWCULT_YetiIntro02DroptoGround()
		AudioManager.Play("sfx_dlc_snowcult_p2_snowmonster_intro_02_droptoground")
		Me.emitAudioFromObject.Add("sfx_dlc_snowcult_p2_snowmonster_intro_02_droptoground")
	End Sub

	' Token: 0x06002F50 RID: 12112 RVA: 0x001BE5D9 File Offset: 0x001BC9D9
	Private Sub AnimationEvent_SFX_SNOWCULT_YetiFridgeToSnowmonster()
		AudioManager.Play("sfx_dlc_snowcult_p2_transform_from_fridge_to_snowmonster")
		Me.emitAudioFromObject.Add("sfx_dlc_snowcult_p2_transform_from_fridge_to_snowmonster")
	End Sub

	' Token: 0x06002F51 RID: 12113 RVA: 0x001BE5F5 File Offset: 0x001BC9F5
	Private Sub AnimationEvent_SFX_SNOWCULT_YetiSnowmonsterToFridge()
		AudioManager.Play("sfx_dlc_snowcult_p2_transform_from_snowmonster_to_fridge")
		Me.emitAudioFromObject.Add("sfx_dlc_snowcult_p2_transform_from_snowmonster_to_fridge")
	End Sub

	' Token: 0x06002F52 RID: 12114 RVA: 0x001BE611 File Offset: 0x001BCA11
	Private Sub AnimationEvent_SFX_SNOWCULT_GroundSmash()
		AudioManager.Play("sfx_DLC_SnowCult_P2_SnowMonster_GroundSmash_withHands")
		Me.emitAudioFromObject.Add("sfx_DLC_SnowCult_P2_SnowMonster_GroundSmash_withHands")
	End Sub

	' Token: 0x06002F53 RID: 12115 RVA: 0x001BE62D File Offset: 0x001BCA2D
	Private Sub AnimationEvent_SFX_SNOWCULT_BodyRollPre()
		AudioManager.Play("sfx_dlc_snowcult_p2_snowmonster_bodyrollpre")
		Me.emitAudioFromObject.Add("sfx_dlc_snowcult_p2_snowmonster_bodyrollpre")
	End Sub

	' Token: 0x06002F54 RID: 12116 RVA: 0x001BE649 File Offset: 0x001BCA49
	Private Sub AnimationEvent_SFX_SNOWCULT_BodyRoll()
		AudioManager.Play("sfx_dlc_snowcult_p2_snowmonster_bodyroll")
		Me.emitAudioFromObject.Add("sfx_dlc_snowcult_p2_snowmonster_bodyroll")
	End Sub

	' Token: 0x06002F55 RID: 12117 RVA: 0x001BE665 File Offset: 0x001BCA65
	Private Sub AnimationEvent_SFX_SNOWCULT_BodyTossPre()
		AudioManager.Play("sfx_dlc_snowcult_p2_snowmonster_bodytosspre")
		Me.emitAudioFromObject.Add("sfx_dlc_snowcult_p2_snowmonster_bodytosspre")
	End Sub

	' Token: 0x06002F56 RID: 12118 RVA: 0x001BE681 File Offset: 0x001BCA81
	Private Sub AnimationEvent_SFX_SNOWCULT_BodyToss()
		AudioManager.Play("sfx_dlc_snowcult_p2_snowmonster_bodytoss")
		Me.emitAudioFromObject.Add("sfx_dlc_snowcult_p2_snowmonster_bodytoss")
	End Sub

	' Token: 0x06002F57 RID: 12119 RVA: 0x001BE69D File Offset: 0x001BCA9D
	Private Sub AnimationEvent_SFX_SNOWCULT_YetiDie()
		AudioManager.Play("sfx_dlc_snowcult_p2_snowmonster_death_explode")
		Me.emitAudioFromObject.Add("sfx_dlc_snowcult_p2_snowmonster_death_explode")
	End Sub

	' Token: 0x06002F58 RID: 12120 RVA: 0x001BE6BC File Offset: 0x001BCABC
	Private Sub SFX_SNOWCULT_YetiFreezerScream()
		Me.batSoundLong = Not Me.batSoundLong
		AudioManager.Play(If((Not Me.batSoundLong), "sfx_dlc_snowcult_p2_snowmonster_fridge_freezerscream_short", "sfx_dlc_snowcult_p2_snowmonster_fridge_freezerscream_long"))
		Me.emitAudioFromObject.Add(If((Not Me.batSoundLong), "sfx_dlc_snowcult_p2_snowmonster_fridge_freezerscream_short", "sfx_dlc_snowcult_p2_snowmonster_fridge_freezerscream_long"))
	End Sub

	' Token: 0x06002F59 RID: 12121 RVA: 0x001BE71C File Offset: 0x001BCB1C
	Private Sub SFX_SNOWCULT_YetiFreezerIceCubeLaunch()
		AudioManager.Play("sfx_dlc_snowcult_p2_snowmonster_fridge_icecube_launch")
		Me.emitAudioFromObject.Add("sfx_dlc_snowcult_p2_snowmonster_fridge_icecube_launch")
	End Sub

	' Token: 0x040037E3 RID: 14307
	Public state As SnowCultLevelYeti.States

	' Token: 0x040037E4 RID: 14308
	Private previousState As SnowCultLevelYeti.States

	' Token: 0x040037E6 RID: 14310
	Private Const BURST_SPAWN_X As Single = 290F

	' Token: 0x040037E7 RID: 14311
	Private Const Y_TO_SPAWN As Single = 95F

	' Token: 0x040037E8 RID: 14312
	Private Const Y_ICE_PILLAR_SPAWN As Single = -142F

	' Token: 0x040037E9 RID: 14313
	Private Const POS_OFFSET_X As Single = 147F

	' Token: 0x040037EA RID: 14314
	Private Const JUMP_LANDING_OFFSET_X As Single = 247F

	' Token: 0x040037EB RID: 14315
	Private Const REFORM_TIME As Single = 0.9583333F

	' Token: 0x040037EC RID: 14316
	Private Const BALL_RADIUS As Single = 180F

	' Token: 0x040037ED RID: 14317
	Private BALL_JUMP_OFFSET As Vector3 = New Vector3(50F, -100F)

	' Token: 0x040037EE RID: 14318
	Private BALL_DASH_OFFSET As Vector3 = New Vector3(50F, -180F)

	' Token: 0x040037EF RID: 14319
	<SerializeField()>
	Private snowCultBGHandler As SnowCultHandleBackground

	' Token: 0x040037F0 RID: 14320
	<SerializeField()>
	Private yetiMidPoint As Transform

	' Token: 0x040037F1 RID: 14321
	<SerializeField()>
	Private yetiSpawnPoint As Transform

	' Token: 0x040037F2 RID: 14322
	<SerializeField()>
	Private icePillarPrefab As SnowCultLevelIcePillar

	' Token: 0x040037F3 RID: 14323
	<SerializeField()>
	Private batPrefab As SnowCultLevelBat

	' Token: 0x040037F4 RID: 14324
	<SerializeField()>
	Private batSpawnEffectPrefab As Effect

	' Token: 0x040037F5 RID: 14325
	Private batSoundLong As Boolean

	' Token: 0x040037F6 RID: 14326
	<SerializeField()>
	Private snowBurstA As SnowCultLevelBurstEffect

	' Token: 0x040037F7 RID: 14327
	<SerializeField()>
	Private snowFallA As SnowCultLevelBurstEffect

	' Token: 0x040037F8 RID: 14328
	<Header("Snowballs")>
	<SerializeField()>
	Private smallSnowballPrefab As SnowCultLevelSnowball

	' Token: 0x040037F9 RID: 14329
	<SerializeField()>
	Private mediumSnowballPrefab As SnowCultLevelSnowball

	' Token: 0x040037FA RID: 14330
	<SerializeField()>
	Private largeSnowballPrefab As SnowCultLevelSnowball

	' Token: 0x040037FB RID: 14331
	<SerializeField()>
	Private cubeLaunchPosition As GameObject

	' Token: 0x040037FC RID: 14332
	<SerializeField()>
	Private ball As GameObject

	' Token: 0x040037FD RID: 14333
	<SerializeField()>
	Private meltFXAnimator As Animator()

	' Token: 0x040037FE RID: 14334
	<SerializeField()>
	Private dashGroundFX As GameObject

	' Token: 0x040037FF RID: 14335
	<SerializeField()>
	Private groundMask As GameObject

	' Token: 0x04003800 RID: 14336
	<SerializeField()>
	Private ballShadow As SpriteRenderer

	' Token: 0x04003801 RID: 14337
	<SerializeField()>
	Private introShadow As GameObject

	' Token: 0x04003802 RID: 14338
	<SerializeField()>
	Private shadowSprites As Sprite()

	' Token: 0x04003803 RID: 14339
	Private offsetCoordIndex As Integer

	' Token: 0x04003804 RID: 14340
	Private snowballMainIndex As Integer

	' Token: 0x04003805 RID: 14341
	Private patternString As String()

	' Token: 0x04003806 RID: 14342
	Private patternStringIndex As Integer

	' Token: 0x04003807 RID: 14343
	Private batAttackPositionString As PatternString

	' Token: 0x04003808 RID: 14344
	Private batAttackHeightString As PatternString

	' Token: 0x04003809 RID: 14345
	Private batAttackWidthString As PatternString

	' Token: 0x0400380A RID: 14346
	Private batAttackSideString As PatternString

	' Token: 0x0400380B RID: 14347
	Private batAttackInterDelayString As PatternString

	' Token: 0x0400380C RID: 14348
	Private batArcModifierString As PatternString

	' Token: 0x0400380D RID: 14349
	Private batParryableString As PatternString

	' Token: 0x0400380E RID: 14350
	<SerializeField()>
	Private batAttackPositions As Transform()

	' Token: 0x0400380F RID: 14351
	Private xScale As Single

	' Token: 0x04003810 RID: 14352
	Private onLeft As Boolean

	' Token: 0x04003811 RID: 14353
	<SerializeField()>
	Private sprite As SpriteRenderer

	' Token: 0x04003812 RID: 14354
	<SerializeField()>
	Private coll As Collider2D

	' Token: 0x04003813 RID: 14355
	<SerializeField()>
	Private legs As GameObject

	' Token: 0x04003814 RID: 14356
	<SerializeField()>
	Private bucket As GameObject

	' Token: 0x04003815 RID: 14357
	Private bats As SnowCultLevelBat()

	' Token: 0x04003816 RID: 14358
	Private batCirclingList As List(Of SnowCultLevelBat) = New List(Of SnowCultLevelBat)()

	' Token: 0x04003817 RID: 14359
	Private batLaunchTimer As Single

	' Token: 0x04003818 RID: 14360
	Private batColor As Boolean

	' Token: 0x04003819 RID: 14361
	Private damageDealer As DamageDealer

	' Token: 0x0400381A RID: 14362
	Private damageReceiver As DamageReceiver

	' Token: 0x0400381B RID: 14363
	Private ballDamageReceiver As DamageReceiver

	' Token: 0x0400381C RID: 14364
	Private fridgeCanShoot As Boolean

	' Token: 0x0400381D RID: 14365
	Public introRibcageClosed As Boolean

	' Token: 0x0400381E RID: 14366
	Private forceOutroToStart As Boolean

	' Token: 0x0400381F RID: 14367
	Private iceCubeStartFrame As Integer

	' Token: 0x04003820 RID: 14368
	Private iceCubeExplosionCounterMedium As Integer

	' Token: 0x04003821 RID: 14369
	Private iceCubeExplosionCounterSmall As Integer

	' Token: 0x04003822 RID: 14370
	Private idleAnimFullPathHash As Integer

	' Token: 0x02000800 RID: 2048
	Public Enum States
		' Token: 0x04003825 RID: 14373
		Intro
		' Token: 0x04003826 RID: 14374
		Idle
		' Token: 0x04003827 RID: 14375
		Move
		' Token: 0x04003828 RID: 14376
		IcePillar
		' Token: 0x04003829 RID: 14377
		Sled
		' Token: 0x0400382A RID: 14378
		Snowball
	End Enum
End Class
