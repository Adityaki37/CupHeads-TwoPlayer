Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Diagnostics
Imports UnityEngine

' Token: 0x020007FD RID: 2045
Public Class SnowCultLevelWizard
	Inherits LevelProperties.SnowCult.Entity

	' Token: 0x1400004D RID: 77
	' (add) Token: 0x06002EF9 RID: 12025 RVA: 0x001BB41C File Offset: 0x001B981C
	' (remove) Token: 0x06002EFA RID: 12026 RVA: 0x001BB454 File Offset: 0x001B9854
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnDeathEvent As Action

	' Token: 0x06002EFB RID: 12027 RVA: 0x001BB48A File Offset: 0x001B988A
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
	End Sub

	' Token: 0x06002EFC RID: 12028 RVA: 0x001BB4C0 File Offset: 0x001B98C0
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06002EFD RID: 12029 RVA: 0x001BB4D8 File Offset: 0x001B98D8
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		MyBase.properties.DealDamage(info.damage)
	End Sub

	' Token: 0x06002EFE RID: 12030 RVA: 0x001BB4EC File Offset: 0x001B98EC
	Public Overrides Sub LevelInit(properties As LevelProperties.SnowCult)
		MyBase.LevelInit(properties)
		Me.state = SnowCultLevelWizard.States.Idle
		Me.wizardHesitationString = New PatternString(properties.CurrentState.wizard.wizardHesitationString, True, True)
		Me.attackLocationString = New PatternString(properties.CurrentState.quadShot.attackLocationString, True, True)
		Me.quadShotBallDelayString = New PatternString(properties.CurrentState.quadShot.ballDelayString, True, True)
		Me.hazardDirectionString = New PatternString(properties.CurrentState.quadShot.hazardDirectionString, True, True)
		Me.seriesShotCountString = New PatternString(properties.CurrentState.seriesShot.seriesShotCountString, True, True)
		Me.seriesShotParryString = New PatternString(properties.CurrentState.seriesShot.parryString, True)
		Me.quadShotBallDelayString.SetSubStringIndex(-1)
		Me.hazardDirectionString.SetSubStringIndex(-1)
		MyBase.StartCoroutine(Me.intro_cr())
	End Sub

	' Token: 0x06002EFF RID: 12031 RVA: 0x001BB5D9 File Offset: 0x001B99D9
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06002F00 RID: 12032 RVA: 0x001BB5F7 File Offset: 0x001B99F7
	Public Sub PlayerHitByWhale(hit As GameObject, phase As CollisionPhase)
		Me.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x06002F01 RID: 12033 RVA: 0x001BB604 File Offset: 0x001B9A04
	Private Iterator Function intro_cr() As IEnumerator
		Yield MyBase.animator.WaitForAnimationToStart(Me, "Idle", False)
		Dim t As Single = 0F
		Dim startPos As Vector3 = MyBase.transform.position
		Dim endPos As Vector3 = New Vector3(Me.pivotPoint.position.x + 540F, Me.pivotPoint.transform.position.y)
		MyBase.animator.SetBool("Turn", True)
		While t < 1F
			Dim easedT As Single = EaseUtils.EaseInOutSine(0F, 1F, t)
			MyBase.transform.position = New Vector3(Mathf.Lerp(startPos.x, endPos.x, easedT), EaseUtils.EaseInSine(startPos.y, endPos.y, easedT))
			t += CupheadTime.Delta
			Yield Nothing
		End While
		Yield CupheadTime.WaitForSeconds(Me, 0.25F)
		MyBase.StartCoroutine(Me.move_cr())
		Return
	End Function

	' Token: 0x06002F02 RID: 12034 RVA: 0x001BB61F File Offset: 0x001B9A1F
	Private Sub AniEvent_StartTurn()
		Me.turnAnimationPlaying = True
	End Sub

	' Token: 0x06002F03 RID: 12035 RVA: 0x001BB628 File Offset: 0x001B9A28
	Private Sub AniEvent_CompleteTurn()
		If Not Me.dead AndAlso Me.turnAnimationPlaying Then
			Dim flag As Boolean = Me.goingLeft
			If Me.currentPosition > 0.9F Then
				flag = Not flag
			End If
			MyBase.transform.localScale = New Vector3(CSng(If((Not flag), 1, (-1))), MyBase.transform.localScale.y)
			Me.turnAnimationPlaying = False
		End If
	End Sub

	' Token: 0x06002F04 RID: 12036 RVA: 0x001BB6A0 File Offset: 0x001B9AA0
	Private Sub AniEvent_AlignForOutro()
		MyBase.transform.localScale = New Vector3(Mathf.Sign(MyBase.transform.position.x - Camera.main.transform.position.x), MyBase.transform.localScale.y)
		Me.outroWobbling = True
	End Sub

	' Token: 0x06002F05 RID: 12037 RVA: 0x001BB707 File Offset: 0x001B9B07
	Public Function Turning() As Boolean
		Return MyBase.animator.GetBool("Turn")
	End Function

	' Token: 0x06002F06 RID: 12038 RVA: 0x001BB71C File Offset: 0x001B9B1C
	Private Iterator Function move_cr() As IEnumerator
		Me.goingLeft = True
		Dim p As LevelProperties.SnowCult.Movement = MyBase.properties.CurrentState.movement
		Dim startAngle As Single = 1.5707964F
		Dim endAngle As Single = -1.5707964F
		Dim angle As Single = endAngle
		Dim loopSizeX As Single = 540F
		Dim loopSizeY As Single = p.dipAmount
		Dim loopSpeed As Single = p.speed
		Dim startSpeed As Single = p.speed
		Dim endSpeed As Single = p.easing
		Dim easeIn As Boolean = True
		Dim handleRotationX As Vector3 = Vector3.zero
		Dim handleRotationY As Vector3 = Vector3.zero
		MyBase.transform.SetPosition(New Single?(Me.pivotPoint.position.x + loopSizeX), Nothing, Nothing)
		Dim t As Single = 1F
		Dim time As Single = 1F
		Me.isMoving = True
		While True
			While Not Me.isMoving
				Yield Nothing
			End While
			angle += loopSpeed * CupheadTime.FixedDelta * Me.postWhalePositionLerpTimer * If((Not Me.dead), 1F, 1.5F)
			If(angle < endAngle AndAlso Not Me.goingLeft) OrElse (angle > startAngle AndAlso Me.goingLeft) Then
				Me.reachedApex = True
				Me.notReachedMid = True
				loopSpeed = -loopSpeed
				Me.goingLeft = Not Me.goingLeft
				t = 0F
				startSpeed = If((Not easeIn), If((Not Me.goingLeft), (-p.easing), p.easing), If((Not Me.goingLeft), (-p.speed), p.speed))
				endSpeed = If((Not easeIn), If((Not Me.goingLeft), (-p.speed), p.speed), If((Not Me.goingLeft), (-p.easing), p.easing))
				easeIn = True
			Else
				Me.reachedApex = False
			End If
			If(angle > startAngle - 1.5F AndAlso Me.goingLeft AndAlso easeIn) OrElse (angle < endAngle + 1.5F AndAlso Not Me.goingLeft AndAlso easeIn) Then
				t = 0F
				startSpeed = If((Not easeIn), If((Not Me.goingLeft), (-p.easing), p.easing), If((Not Me.goingLeft), (-p.speed), p.speed))
				endSpeed = If((Not easeIn), If((Not Me.goingLeft), (-p.speed), p.speed), If((Not Me.goingLeft), (-p.easing), p.easing))
				easeIn = False
			End If
			If((Me.goingLeft AndAlso MyBase.transform.position.x < 0F) OrElse (Not Me.goingLeft AndAlso MyBase.transform.position.x > 0F)) AndAlso Me.notReachedMid Then
				Me.notReachedMid = False
			End If
			If t < time Then
				t += CupheadTime.FixedDelta
				loopSpeed = Mathf.Lerp(startSpeed, endSpeed, t / time)
			End If
			Dim handleRotation As Vector3 = New Vector3(-Mathf.Sin(angle) * loopSizeX, -Mathf.Cos(angle) * loopSizeY, 0F)
			Dim destinationPos As Vector3 = Me.pivotPoint.position + handleRotation
			Me.lastPos = MyBase.transform.position
			MyBase.transform.position = New Vector3(destinationPos.x, Mathf.Lerp(MyBase.transform.position.y, destinationPos.y, Me.postWhalePositionLerpTimer))
			Me.postWhalePositionLerpTimer = Mathf.Clamp(Me.postWhalePositionLerpTimer + CupheadTime.FixedDelta * 2.5F, 0F, 1F)
			Me.currentPosition = Mathf.InverseLerp(startAngle, endAngle, angle)
			If Me.goingLeft Then
				Me.currentPosition = 1F - Me.currentPosition
			End If
			Dim goingLeftOrientation As Boolean = Me.goingLeft
			If Me.currentPosition > 0.9F Then
				goingLeftOrientation = Not goingLeftOrientation
			End If
			If Not Me.dead Then
				MyBase.animator.SetBool("Turn", CInt(MyBase.transform.localScale.x) <> If((Not goingLeftOrientation), 1, (-1)) AndAlso Not Me.seriesShotActive)
			End If
			Yield New WaitForFixedUpdate()
		End While
		Return
	End Function

	' Token: 0x06002F07 RID: 12039 RVA: 0x001BB737 File Offset: 0x001B9B37
	Public Sub StartQuadAttack()
		MyBase.StartCoroutine(Me.quad_cr())
	End Sub

	' Token: 0x06002F08 RID: 12040 RVA: 0x001BB748 File Offset: 0x001B9B48
	Private Iterator Function quad_cr() As IEnumerator
		Me.state = SnowCultLevelWizard.States.Quad
		Dim p As LevelProperties.SnowCult.QuadShot = MyBase.properties.CurrentState.quadShot
		Dim targetPosX As Single = Me.attackLocationString.PopFloat()
		Dim inAttackPos As Boolean = False
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		While Not inAttackPos
			If Me.dead Then
				Return
			End If
			If Mathf.Abs(targetPosX - MyBase.transform.position.x) < p.distToAttack AndAlso Not Me.turnAnimationPlaying Then
				inAttackPos = True
			End If
			Yield wait
		End While
		Dim curIdleFrame As Integer = Mathf.RoundToInt(MyBase.animator.GetCurrentAnimatorStateInfo(0).normalizedTime * 23F)
		If curIdleFrame >= 14 AndAlso curIdleFrame <= 22 Then
			MyBase.animator.Play("QuadshotIntro", 0, 0.2857143F)
		ElseIf curIdleFrame >= 2 AndAlso curIdleFrame <= 10 Then
			MyBase.animator.Play("QuadshotIntro", 0, 0.14285715F)
		Else
			MyBase.animator.Play("QuadshotIntro")
		End If
		Me.SFX_SNOWCULT_WizardQuadshotAttack()
		Me.isMoving = False
		Dim quadShots As List(Of SnowCultLevelQuadShot) = New List(Of SnowCultLevelQuadShot)()
		Dim downAmount As Single = 0F
		Yield CupheadTime.WaitForSeconds(Me, p.preattackDelay)
		MyBase.animator.Play("QuadshotContinue")
		Yield Nothing
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "QuadshotContinue", False, True)
		Me.quadshotMask.enabled = True
		For i As Integer = 0 To 4 - 1
			downAmount = If((i <= 0 OrElse i >= 3), 0F, p.distanceDown)
			Dim vector As Vector3 = New Vector3(MyBase.transform.position.x - p.distanceBetween * 0.8F * 2F + p.distanceBetween * 0.8F * 0.5F + p.distanceBetween * 0.8F * CSng(i), MyBase.transform.position.y - downAmount)
			Dim vector2 As Vector3 = New Vector3(MyBase.transform.position.x - p.distanceBetween * 2F + p.distanceBetween / 2F + p.distanceBetween * CSng(i), MyBase.transform.position.y - downAmount)
			Dim snowCultLevelQuadShot As SnowCultLevelQuadShot = Me.quadShotProjectile.Spawn()
			Dim num As Single = Me.quadShotBallDelayString.PopFloat() / 4F * p.ballDelay
			snowCultLevelQuadShot.Init(vector, vector2, p.shotVelocity, Me.hazardDirectionString.PopString(), p, i, num, p.distanceBetween, PlayerManager.GetNext())
			quadShots.Add(snowCultLevelQuadShot)
		Next
		Yield CupheadTime.WaitForSeconds(Me, 0.25F)
		Me.quadshotMask.enabled = False
		Yield CupheadTime.WaitForSeconds(Me, p.attackDelay - 0.25F)
		MyBase.animator.Play("QuadshotEnd")
		Yield Nothing
		While MyBase.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.27272728F
			Yield Nothing
		End While
		Dim player As AbstractPlayerController = PlayerManager.GetNext()
		Dim first As Single = 1000F
		Dim second As Single = 1000F
		Dim shotQuadChosen As SnowCultLevelQuadShot = Nothing
		Dim shotQuadChosen2 As SnowCultLevelQuadShot = Nothing
		For j As Integer = 0 To 4 - 1
			Dim num2 As Single = Mathf.Abs(quadShots(j).transform.position.x - player.transform.position.x)
			If num2 < first Then
				second = first
				first = num2
				shotQuadChosen2 = shotQuadChosen
				shotQuadChosen = quadShots(j)
			ElseIf num2 < second AndAlso num2 <> first Then
				second = num2
				shotQuadChosen2 = quadShots(j)
			End If
		Next
		Dim offset As Single = Global.UnityEngine.Random.Range(0F, p.maxOffset)
		offset = If((Not Rand.Bool()), (-offset), offset)
		Dim shotQuadChosen3 As SnowCultLevelQuadShot = If((Not Rand.Bool()), shotQuadChosen2, shotQuadChosen)
		Dim endPos As Vector3 = New Vector3(player.transform.position.x, CSng(Level.Current.Ground))
		Dim direction As Vector3 = endPos - shotQuadChosen3.transform.position
		Dim finalDirection As Vector3 = New Vector3(direction.x + offset, direction.y)
		Me.lineStartPos = shotQuadChosen3.transform.position
		Me.lineEndPos = New Vector3(player.transform.position.x + offset, endPos.y)
		Dim startWithRight As Boolean = Rand.Bool()
		Dim rightIndex As Integer = 3
		For k As Integer = 0 To 4 - 1
			Dim num3 As Integer = If((Not startWithRight), k, (rightIndex - k))
			quadShots(num3).Shoot(MathUtils.DirectionToAngle(finalDirection))
		Next
		Yield MyBase.animator.WaitForAnimationToStart(Me, "Idle", False)
		Me.isMoving = True
		Yield CupheadTime.WaitForSeconds(Me, Me.wizardHesitationString.PopFloat())
		Me.state = SnowCultLevelWizard.States.Idle
		Return
	End Function

	' Token: 0x06002F09 RID: 12041 RVA: 0x001BB763 File Offset: 0x001B9B63
	Public Sub Whale()
		MyBase.StartCoroutine(Me.whale_cr())
	End Sub

	' Token: 0x06002F0A RID: 12042 RVA: 0x001BB774 File Offset: 0x001B9B74
	Private Iterator Function whale_cr() As IEnumerator
		Me.state = SnowCultLevelWizard.States.Whale
		Dim p As LevelProperties.SnowCult.Whale = MyBase.properties.CurrentState.whale
		Me.dropAttackComplete = False
		Dim drop As Boolean = False
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		Dim player As AbstractPlayerController = PlayerManager.GetNext()
		Dim lastPlayerOffset As Single = MyBase.transform.position.x - Mathf.Clamp(player.transform.position.x, -445F, 445F)
		While Not drop
			Dim playerClampedX As Single = Mathf.Clamp(player.transform.position.x, -445F, 445F)
			If Mathf.Abs(playerClampedX - MyBase.transform.position.x) < p.distToDrop OrElse Mathf.Sign(lastPlayerOffset) <> Mathf.Sign(MyBase.transform.position.x - playerClampedX) Then
				drop = True
			End If
			lastPlayerOffset = MyBase.transform.position.x - playerClampedX
			Yield wait
		End While
		Me.isMoving = False
		Yield MyBase.animator.WaitForAnimationToStart(Me, "Idle", False)
		Dim currentAnimatorTime As Single = MyBase.animator.GetCurrentAnimatorStateInfo(0).normalizedTime
		MyBase.animator.Play(If((currentAnimatorTime <= 0.083333336F OrElse currentAnimatorTime >= 0.5833333F), "WhaleDrop_IntroAlt", "WhaleDrop_Intro"))
		Me.SFX_SNOWCULT_WizardWhalesmashAttack()
		Dim t As Single = 0F
		Dim val As Single = 0F
		Dim startPos As Vector3 = MyBase.transform.position
		Dim endPos As Vector3 = New Vector3(startPos.x, 200F)
		While t < 0.22F
			t += CupheadTime.Delta
			val = Mathf.InverseLerp(0F, 0.22F, t)
			MyBase.transform.position = Vector3.Lerp(startPos, endPos, EaseUtils.EaseInSine(0F, 1F, val))
			Yield Nothing
		End While
		MyBase.transform.position = endPos
		t = 0F
		While t < p.attackDelay
			t += CupheadTime.Delta
			Yield Nothing
		End While
		MyBase.animator.SetTrigger("DropWhale")
		While Not Me.dropAttackComplete
			Yield Nothing
		End While
		Yield CupheadTime.WaitForSeconds(Me, 0.083333336F)
		Me.postWhalePositionLerpTimer = 0F
		Me.isMoving = True
		Yield CupheadTime.WaitForSeconds(Me, p.recoveryDelay)
		Yield CupheadTime.WaitForSeconds(Me, Me.wizardHesitationString.PopFloat())
		Me.state = SnowCultLevelWizard.States.Idle
		Return
	End Function

	' Token: 0x06002F0B RID: 12043 RVA: 0x001BB78F File Offset: 0x001B9B8F
	Private Sub WhaleAttackImpact()
		CupheadLevelCamera.Current.Shake(55F, 0.5F, False)
	End Sub

	' Token: 0x06002F0C RID: 12044 RVA: 0x001BB7A8 File Offset: 0x001B9BA8
	Private Sub WhaleAttackComplete()
		Me.whaleDropFX.transform.position = New Vector3(MyBase.transform.position.x, Me.whaleDropFX.transform.position.y)
		Me.whaleDropFX.gameObject.SetActive(True)
		Me.whaleDropFX.Play("Main")
		Me.dropAttackComplete = True
	End Sub

	' Token: 0x06002F0D RID: 12045 RVA: 0x001BB81D File Offset: 0x001B9C1D
	Public Sub SeriesShot()
		MyBase.StartCoroutine(Me.series_shot_cr())
	End Sub

	' Token: 0x06002F0E RID: 12046 RVA: 0x001BB82C File Offset: 0x001B9C2C
	Private Iterator Function series_shot_cr() As IEnumerator
		Me.seriesShotCanExit = False
		Me.seriesShotActive = True
		Me.state = SnowCultLevelWizard.States.SeriesShot
		Dim p As LevelProperties.SnowCult.SeriesShot = MyBase.properties.CurrentState.seriesShot
		Dim shotCount As Integer = Me.seriesShotCountString.PopInt()
		Dim t As Single = 0F
		MyBase.animator.SetTrigger("StartPeashot")
		Yield MyBase.animator.WaitForAnimationToStart(Me, "Peashot_Intro", False)
		Me.table.Intro(MyBase.transform.position - Me.lastPos)
		For i As Integer = 0 To shotCount - 1
			While t < p.seriesShotWarningTime AndAlso Not Me.dead
				t += CupheadTime.Delta
				Yield Nothing
			End While
			If Not Me.dead Then
				MyBase.animator.SetTrigger("OnShoot")
				While Not Me.seriesShotFired
					Yield Nothing
				End While
				Me.SFX_SNOWCULT_WizardTarotCardAttackLaunch()
				Me.seriesShotFired = False
			End If
			t = 0F
			While t < p.betweenShotDelay AndAlso Not Me.dead
				t += CupheadTime.Delta
				Yield Nothing
			End While
			If Me.dead Then
				Exit For
			End If
		Next
		Me.seriesShotCanExit = True
		While Me.seriesShotActive
			Yield Nothing
		End While
		If Not Me.dead Then
			Yield CupheadTime.WaitForSeconds(Me, Me.wizardHesitationString.PopFloat())
		End If
		Me.state = SnowCultLevelWizard.States.Idle
		Yield Nothing
		Return
	End Function

	' Token: 0x06002F0F RID: 12047 RVA: 0x001BB847 File Offset: 0x001B9C47
	Private Sub CreatePeashot()
		MyBase.StartCoroutine(Me.create_peashot())
	End Sub

	' Token: 0x06002F10 RID: 12048 RVA: 0x001BB858 File Offset: 0x001B9C58
	Private Iterator Function create_peashot() As IEnumerator
		Me.shootFX.Play("ShootFX")
		Me.shootFX.Update(0F)
		Yield CupheadTime.WaitForSeconds(Me, 0.041666668F)
		Dim player As AbstractPlayerController = PlayerManager.GetNext()
		Dim dir As Vector3 = player.transform.position - Me.shootFX.transform.position
		Dim proj As BasicProjectile = Me.seriesShot.Create(Me.shootFX.transform.position, MathUtils.DirectionToAngle(dir) + 90F, MyBase.properties.CurrentState.seriesShot.bulletSpeed)
		proj.transform.position += dir.normalized * 25F
		proj.SetParryable(Me.seriesShotParryString.PopLetter() = "P"c)
		Me.seriesShotFired = True
		While Me.shootFX.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.8F
			Dim sparkle As Effect = Me.cardSparkle.Create(Me.shootFX.transform.position + MathUtils.AngleToDirection(CSng(Global.UnityEngine.Random.Range(0, 360))) * Me.shootFX.GetCurrentAnimatorStateInfo(0).normalizedTime * 200F)
			Yield CupheadTime.WaitForSeconds(Me, Global.UnityEngine.Random.Range(0.005F, 0.01F))
		End While
		Return
	End Function

	' Token: 0x06002F11 RID: 12049 RVA: 0x001BB873 File Offset: 0x001B9C73
	Private Sub CanExitPeashotLoop()
		If Me.seriesShotCanExit Then
			MyBase.animator.Play("Peashot_Outro_A")
			Me.table.Outro()
		End If
	End Sub

	' Token: 0x06002F12 RID: 12050 RVA: 0x001BB89B File Offset: 0x001B9C9B
	Private Sub EndPeashotLoop()
		Me.seriesShotActive = False
	End Sub

	' Token: 0x06002F13 RID: 12051 RVA: 0x001BB8A4 File Offset: 0x001B9CA4
	Public Sub ToOutro(yeti As SnowCultLevelYeti)
		Me.dead = True
		MyBase.StartCoroutine(Me.outro_cr(yeti))
	End Sub

	' Token: 0x06002F14 RID: 12052 RVA: 0x001BB8BB File Offset: 0x001B9CBB
	Private Sub AniEvent_CultistsSummon()
		CType(Level.Current, SnowCultLevel).CultistsSummon()
	End Sub

	' Token: 0x06002F15 RID: 12053 RVA: 0x001BB8CC File Offset: 0x001B9CCC
	Private Iterator Function outro_cr(yeti As SnowCultLevelYeti) As IEnumerator
		While Not Me.reachedApex
			Yield Nothing
		End While
		If MyBase.transform.localScale.x <> Mathf.Sign(MyBase.transform.position.x - Camera.main.transform.position.x) Then
			MyBase.animator.SetBool("Turn", True)
		End If
		Me.state = SnowCultLevelWizard.States.Idle
		Me.isMoving = False
		MyBase.animator.SetTrigger("OnOutro")
		Dim t As Single = 0F
		Dim startPos As Vector3 = MyBase.transform.position
		If MyBase.transform.position.x < Me.pivotPoint.position.x Then
			Me.outroPos.position = New Vector3(Me.pivotPoint.position.x + (Me.pivotPoint.position.x - Me.outroPos.position.x), Me.outroPos.position.y)
			yeti.StartOnLeft(Me.pivotPoint.position)
		End If
		While t < 0.5F
			MyBase.transform.position = New Vector3(EaseUtils.EaseOutSine(startPos.x, Me.outroPos.position.x, t * 2F), EaseUtils.EaseOutBack(startPos.y, Me.outroPos.position.y, t * 2F))
			t += CupheadTime.FixedDelta
			Yield New WaitForFixedUpdate()
		End While
		While MyBase.animator.GetCurrentAnimatorStateInfo(0).fullPathHash <> Animator.StringToHash(MyBase.animator.GetLayerName(0) + ".OutroLoop")
			MyBase.transform.position = Me.outroPos.position
			Yield Nothing
		End While
		yeti.gameObject.SetActive(True)
		yeti.StartYeti()
		While Not yeti.introRibcageClosed
			MyBase.transform.position = Me.outroPos.position
			Yield Nothing
		End While
		Me.OnDeath()
		Return
	End Function

	' Token: 0x06002F16 RID: 12054 RVA: 0x001BB8F0 File Offset: 0x001B9CF0
	Private Sub LateUpdate()
		If Not Me.outroWobbling Then
			Return
		End If
		Me.outroWobbleTime += CupheadTime.FixedDelta * 1.5F
		MyBase.transform.position += New Vector3(Mathf.Sin(Me.outroWobbleTime * 3F) * 1F, Mathf.Cos(Me.outroWobbleTime * 2F) * 5F)
	End Sub

	' Token: 0x06002F17 RID: 12055 RVA: 0x001BB96A File Offset: 0x001B9D6A
	Public Sub OnDeath()
		If Me.OnDeathEvent IsNot Nothing Then
			Me.OnDeathEvent()
		End If
		Me.StopAllCoroutines()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06002F18 RID: 12056 RVA: 0x001BB993 File Offset: 0x001B9D93
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		Gizmos.DrawLine(Me.lineStartPos, Me.lineEndPos)
	End Sub

	' Token: 0x06002F19 RID: 12057 RVA: 0x001BB9AC File Offset: 0x001B9DAC
	Private Sub AnimationEvent_SFX_SNOWCULT_WizardIntro()
		AudioManager.Play("sfx_dlc_snowcult_p1_wizard_intro")
		Me.emitAudioFromObject.Add("sfx_dlc_snowcult_p1_wizard_intro")
	End Sub

	' Token: 0x06002F1A RID: 12058 RVA: 0x001BB9C8 File Offset: 0x001B9DC8
	Private Sub AnimationEvent_SFX_SNOWCULT_WizardQuadshot_Attack()
		AudioManager.Play("sfx_dlc_snowcult_p1_wizard_quadshot_attack")
		Me.emitAudioFromObject.Add("sfx_dlc_snowcult_p1_wizard_quadshot_attack")
	End Sub

	' Token: 0x06002F1B RID: 12059 RVA: 0x001BB9E4 File Offset: 0x001B9DE4
	Private Sub SFX_SNOWCULT_WizardWhalesmashAttack()
		AudioManager.Play("sfx_dlc_snowcult_p1_wizard_whalesmash_attack")
		Me.emitAudioFromObject.Add("sfx_dlc_snowcult_p1_wizard_whalesmash_attack")
	End Sub

	' Token: 0x06002F1C RID: 12060 RVA: 0x001BBA00 File Offset: 0x001B9E00
	Private Sub SFX_SNOWCULT_WizardTarotCardAttackLaunch()
		AudioManager.Play("sfx_dlc_snowcult_p1_wizard_tarotcardattack_launch")
		Me.emitAudioFromObject.Add("sfx_dlc_snowcult_p1_wizard_tarotcardattack_launch")
	End Sub

	' Token: 0x06002F1D RID: 12061 RVA: 0x001BBA1C File Offset: 0x001B9E1C
	Private Sub SFX_SNOWCULT_WizardQuadshotAttack()
		AudioManager.Play("sfx_dlc_snowcult_p1_wizard_quadshot_attack")
		Me.emitAudioFromObject.Add("sfx_dlc_snowcult_p1_wizard_quadshot_attack")
	End Sub

	' Token: 0x06002F1E RID: 12062 RVA: 0x001BBA38 File Offset: 0x001B9E38
	Private Sub AnimationEvent_SFX_SNOWCULT_WizardYetiIntroBellComesToLife()
		AudioManager.Play("sfx_dlc_snowcult_p2_snowmonster_intro_bell_comestolife")
		Me.emitAudioFromObject.Add("sfx_dlc_snowcult_p2_snowmonster_intro_bell_comestolife")
	End Sub

	' Token: 0x06002F1F RID: 12063 RVA: 0x001BBA54 File Offset: 0x001B9E54
	Private Sub AnimationEvent_SFX_SNOWCULT_WizardVoiceEffortLarge()
		AudioManager.[Stop]("sfx_dlc_snowcult_wizard_voice_laugh")
		AudioManager.Play("sfx_dlc_snowcult_wizard_voice_effort_large")
		Me.emitAudioFromObject.Add("sfx_dlc_snowcult_wizard_voice_effort_large")
	End Sub

	' Token: 0x06002F20 RID: 12064 RVA: 0x001BBA7A File Offset: 0x001B9E7A
	Private Sub AnimationEvent_SFX_SNOWCULT_WizardVoiceLaugh()
		AudioManager.[Stop]("sfx_dlc_snowcult_wizard_voice_effort_large")
		AudioManager.[Stop]("sfx_dlc_snowcult_wizard_voice_laugh")
		AudioManager.Play("sfx_dlc_snowcult_wizard_voice_laugh")
		Me.emitAudioFromObject.Add("sfx_dlc_snowcult_wizard_voice_laugh")
	End Sub

	' Token: 0x06002F21 RID: 12065 RVA: 0x001BBAAA File Offset: 0x001B9EAA
	Private Sub AnimationEvent_SFX_SNOWCULT_WizardVoiceWhee()
		AudioManager.Play("sfx_dlc_snowcult_wizard_voice_whee")
		Me.emitAudioFromObject.Add("sfx_dlc_snowcult_wizard_voice_whee")
	End Sub

	' Token: 0x040037AE RID: 14254
	Public state As SnowCultLevelWizard.States

	' Token: 0x040037B0 RID: 14256
	Private Const NUM_OF_SLAM_SLOTS As Integer = 4

	' Token: 0x040037B1 RID: 14257
	Private Const NUM_OF_QUAD_SHOTS As Integer = 4

	' Token: 0x040037B2 RID: 14258
	Private Const QUAD_SHOT_START_SPACING_MULTIPLIER As Single = 0.8F

	' Token: 0x040037B3 RID: 14259
	Private Const WIZARD_SLAM_OFFSET As Single = 230F

	' Token: 0x040037B4 RID: 14260
	Private Const WHALE_ATTACK_HEIGHT As Single = 200F

	' Token: 0x040037B5 RID: 14261
	Private Const WHALE_ATTACK_MOVE_DELAY As Single = 0.22F

	' Token: 0x040037B6 RID: 14262
	Private Const WHALE_POSTATTACK_MOVE_DELAY As Single = 0.4F

	' Token: 0x040037B7 RID: 14263
	Private Const WHALE_RANGE As Single = 195F

	' Token: 0x040037B8 RID: 14264
	Private damageDealer As DamageDealer

	' Token: 0x040037B9 RID: 14265
	Private damageReceiver As DamageReceiver

	' Token: 0x040037BA RID: 14266
	<SerializeField()>
	Private seriesShot As BasicProjectile

	' Token: 0x040037BB RID: 14267
	<SerializeField()>
	Private whaleDropFX As Animator

	' Token: 0x040037BC RID: 14268
	<SerializeField()>
	Private table As SnowCultLevelTable

	' Token: 0x040037BD RID: 14269
	<SerializeField()>
	Private shootFX As Animator

	' Token: 0x040037BE RID: 14270
	<SerializeField()>
	Private quadShotProjectile As SnowCultLevelQuadShot

	' Token: 0x040037BF RID: 14271
	<SerializeField()>
	Private pivotPoint As Transform

	' Token: 0x040037C0 RID: 14272
	<SerializeField()>
	Private outroPos As Transform

	' Token: 0x040037C1 RID: 14273
	<SerializeField()>
	Private quadshotMask As SpriteMask

	' Token: 0x040037C2 RID: 14274
	<SerializeField()>
	Private cardSparkle As Effect

	' Token: 0x040037C3 RID: 14275
	<SerializeField()>
	Private introWizRend As SpriteRenderer

	' Token: 0x040037C4 RID: 14276
	Private lineStartPos As Vector3

	' Token: 0x040037C5 RID: 14277
	Private lineEndPos As Vector3

	' Token: 0x040037C6 RID: 14278
	Private goingLeft As Boolean

	' Token: 0x040037C7 RID: 14279
	Private isMoving As Boolean

	' Token: 0x040037C8 RID: 14280
	Private reachedApex As Boolean

	' Token: 0x040037C9 RID: 14281
	Private notReachedMid As Boolean

	' Token: 0x040037CA RID: 14282
	Private lastPos As Vector3 = Vector3.zero

	' Token: 0x040037CB RID: 14283
	Private wizardHesitationString As PatternString

	' Token: 0x040037CC RID: 14284
	Private attackLocationString As PatternString

	' Token: 0x040037CD RID: 14285
	Private hazardDirectionString As PatternString

	' Token: 0x040037CE RID: 14286
	Private iceSummonString As PatternString

	' Token: 0x040037CF RID: 14287
	Private seriesShotCountString As PatternString

	' Token: 0x040037D0 RID: 14288
	Private quadShotBallDelayString As PatternString

	' Token: 0x040037D1 RID: 14289
	Private seriesShotFired As Boolean

	' Token: 0x040037D2 RID: 14290
	Private seriesShotCanExit As Boolean = True

	' Token: 0x040037D3 RID: 14291
	Private seriesShotActive As Boolean

	' Token: 0x040037D4 RID: 14292
	Private seriesShotParryString As PatternString

	' Token: 0x040037D5 RID: 14293
	Private dropAttackComplete As Boolean

	' Token: 0x040037D6 RID: 14294
	Private postWhalePositionLerpTimer As Single = 1F

	' Token: 0x040037D7 RID: 14295
	Public dead As Boolean

	' Token: 0x040037D8 RID: 14296
	Private turnAnimationPlaying As Boolean

	' Token: 0x040037D9 RID: 14297
	Private currentPosition As Single = 1F

	' Token: 0x040037DA RID: 14298
	Private outroWobbling As Boolean

	' Token: 0x040037DB RID: 14299
	Private outroWobbleTime As Single

	' Token: 0x020007FE RID: 2046
	Public Enum States
		' Token: 0x040037DD RID: 14301
		Idle
		' Token: 0x040037DE RID: 14302
		Quad
		' Token: 0x040037DF RID: 14303
		Whale
		' Token: 0x040037E0 RID: 14304
		Slam
		' Token: 0x040037E1 RID: 14305
		Wind
		' Token: 0x040037E2 RID: 14306
		SeriesShot
	End Enum
End Class
