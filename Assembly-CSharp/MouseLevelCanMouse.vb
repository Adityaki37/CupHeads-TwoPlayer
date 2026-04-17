Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020006E2 RID: 1762
Public Class MouseLevelCanMouse
	Inherits LevelProperties.Mouse.Entity

	' Token: 0x170003C1 RID: 961
	' (get) Token: 0x0600258E RID: 9614 RVA: 0x0015F6B4 File Offset: 0x0015DAB4
	' (set) Token: 0x0600258F RID: 9615 RVA: 0x0015F6BC File Offset: 0x0015DABC
	Public Property state As MouseLevelCanMouse.State

	' Token: 0x06002590 RID: 9616 RVA: 0x0015F6C5 File Offset: 0x0015DAC5
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.SetWheels(False)
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		Me.damageDealer = DamageDealer.NewEnemy()
	End Sub

	' Token: 0x06002591 RID: 9617 RVA: 0x0015F704 File Offset: 0x0015DB04
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
		If Not Me.peeking AndAlso MyBase.properties.CurrentHealth < MyBase.properties.TotalHealth * Me.catPeeking.Peek1Threshold Then
			Me.catPeeking.StartPeeking()
			Me.peeking = True
		End If
	End Sub

	' Token: 0x06002592 RID: 9618 RVA: 0x0015F76B File Offset: 0x0015DB6B
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If Me.damageDealer IsNot Nothing AndAlso phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06002593 RID: 9619 RVA: 0x0015F794 File Offset: 0x0015DB94
	Public Overrides Sub LevelInit(properties As LevelProperties.Mouse)
		MyBase.LevelInit(properties)
		AddHandler Level.Current.OnIntroEvent, AddressOf Me.OnLevelStart
	End Sub

	' Token: 0x06002594 RID: 9620 RVA: 0x0015F7B3 File Offset: 0x0015DBB3
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		MyBase.properties.DealDamage(info.damage)
	End Sub

	' Token: 0x06002595 RID: 9621 RVA: 0x0015F7C6 File Offset: 0x0015DBC6
	Private Function hitPauseCoefficient() As Single
		Return If((Not MyBase.GetComponent(Of DamageReceiver)().IsHitPaused), 1F, 0F)
	End Function

	' Token: 0x06002596 RID: 9622 RVA: 0x0015F7E7 File Offset: 0x0015DBE7
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.cherryBombPrefab = Nothing
		Me.catapultProjectilePrefab = Nothing
		Me.romanCandlePrefab = Nothing
		Me.wheelSprites = Nothing
	End Sub

	' Token: 0x06002597 RID: 9623 RVA: 0x0015F80B File Offset: 0x0015DC0B
	Public Sub OnLevelStart()
		MyBase.StartCoroutine(Me.wheels_cr())
		MyBase.StartCoroutine(Me.levelStart_cr())
	End Sub

	' Token: 0x06002598 RID: 9624 RVA: 0x0015F828 File Offset: 0x0015DC28
	Private Iterator Function levelStart_cr() As IEnumerator
		MyBase.animator.Play("Intro", 0)
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Intro", 0, False, True)
		MyBase.animator.Play("Intro_Down", 1)
		Me.state = MouseLevelCanMouse.State.Idle
		Return
	End Function

	' Token: 0x06002599 RID: 9625 RVA: 0x0015F844 File Offset: 0x0015DC44
	Private Iterator Function moveBack_cr() As IEnumerator
		Me.exitAfterMoveBack = True
		While Me.exitAfterMoveBack
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600259A RID: 9626 RVA: 0x0015F860 File Offset: 0x0015DC60
	Private Iterator Function moveToX_cr(x As Single) As IEnumerator
		Me.overrideMove = True
		Me.overrideMoveX = x
		If Not Me.moving Then
			MyBase.StartCoroutine(Me.move_cr())
		End If
		While Me.overrideMove
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600259B RID: 9627 RVA: 0x0015F882 File Offset: 0x0015DC82
	Private Sub SetWheels(b As Boolean)
		Me.wheelRenderer.enabled = b
	End Sub

	' Token: 0x0600259C RID: 9628 RVA: 0x0015F890 File Offset: 0x0015DC90
	Private Iterator Function move_cr() As IEnumerator
		MyBase.animator.Play("Move", 1)
		Me.SetWheels(True)
		Me.moving = True
		Dim movingBack As Boolean = False
		While True
			Dim p As LevelProperties.Mouse.CanMove = MyBase.properties.CurrentState.canMove
			Dim [end] As Vector2 = New Vector2(500F * MyBase.transform.localScale.x, MyBase.transform.position.y)
			Dim overriden As Boolean = False
			If Me.overrideMove Then
				[end].x = Me.overrideMoveX
				overriden = True
			ElseIf Not movingBack Then
				[end].x -= p.maxXPositionRange.RandomFloat() * MyBase.transform.localScale.x
			End If
			Dim time As Single = Mathf.Abs(MyBase.transform.position.x - [end].x) / p.speed
			Yield MyBase.StartCoroutine(Me.tween_cr(MyBase.transform, MyBase.transform.position, [end], EaseUtils.EaseType.easeInOutSine, time))
			Yield CupheadTime.WaitForSeconds(Me, p.stopTime)
			If Me.overrideMove AndAlso overriden Then
				Exit For
			End If
			If movingBack AndAlso Me.exitAfterMoveBack Then
				GoTo Block_6
			End If
			movingBack = Not movingBack
		End While
		Me.overrideMove = False
		GoTo IL_0270
		Block_6:
		Me.exitAfterMoveBack = False
		IL_0270:
		Me.SetWheels(False)
		MyBase.animator.Play("IdleDown", 1)
		Me.moving = False
		Return
	End Function

	' Token: 0x0600259D RID: 9629 RVA: 0x0015F8AC File Offset: 0x0015DCAC
	Private Iterator Function wheels_cr() As IEnumerator
		Dim currentFrame As Integer = 0
		Dim lastPos As Vector2 = MyBase.transform.position
		Dim direction As Integer = 1
		While True
			Dim distance As Single = 0F
			While distance < 6F
				Dim speed As Single = lastPos.x - MyBase.transform.position.x
				distance += Mathf.Abs(speed)
				If MyBase.transform.localScale.x > 0F Then
					If speed < 0F Then
						direction = -1
					Else
						direction = 1
					End If
				ElseIf speed < 0F Then
					direction = 1
				Else
					direction = -1
				End If
				lastPos = MyBase.transform.position
				Yield Nothing
			End While
			currentFrame += direction
			currentFrame = CInt(Mathf.Repeat(CSng(currentFrame), CSng(Me.wheelSprites.Length)))
			Me.wheelRenderer.sprite = Me.wheelSprites(currentFrame)
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600259E RID: 9630 RVA: 0x0015F8C7 File Offset: 0x0015DCC7
	Public Sub StartDash()
		Me.state = MouseLevelCanMouse.State.Dash
		MyBase.StartCoroutine(Me.dash_cr())
	End Sub

	' Token: 0x0600259F RID: 9631 RVA: 0x0015F8DD File Offset: 0x0015DCDD
	Private Sub StartDashMove()
		Me.dash = True
		MyBase.animator.SetTrigger("CanContinue")
		AudioManager.Play("level_mouse_can_dash_start")
		Me.emitAudioFromObject.Add("level_mouse_can_dash_start")
	End Sub

	' Token: 0x060025A0 RID: 9632 RVA: 0x0015F910 File Offset: 0x0015DD10
	Private Sub DashFlipX()
		MyBase.transform.SetScale(New Single?(-MyBase.transform.localScale.x), New Single?(1F), New Single?(1F))
		If MyBase.transform.localScale.x < 0F Then
			Me.direction = MouseLevelCanMouse.Direction.Right
			MyBase.transform.AddPosition(40F, 0F, 0F)
		Else
			Me.direction = MouseLevelCanMouse.Direction.Left
			MyBase.transform.AddPosition(-40F, 0F, 0F)
		End If
		MyBase.animator.SetTrigger("CanContinue")
	End Sub

	' Token: 0x060025A1 RID: 9633 RVA: 0x0015F9CC File Offset: 0x0015DDCC
	Private Iterator Function dash_cr() As IEnumerator
		Dim dashProperties As LevelProperties.Mouse.CanDash = MyBase.properties.CurrentState.canDash
		For i As Integer = 0 To Me.springs.Length - 1
			Dim vector As Vector2 = New Vector2(dashProperties.springVelocityX(i).RandomFloat(), dashProperties.springVelocityY(i).RandomFloat())
			If Me.direction = MouseLevelCanMouse.Direction.Right Then
				vector.x *= -1F
			End If
			Me.springs(i).LaunchSpring(New Vector2(MyBase.transform.position.x, MyBase.transform.position.y + 200F), vector, dashProperties.springGravity)
			AudioManager.Play("level_mouse_can_springboard_shoot")
			Me.emitAudioFromObject.Add("level_mouse_can_springboard_shoot")
			MyBase.StartCoroutine(Me.timedAudioMouseSnarky_cr())
		Next
		If Me.moving Then
			Yield MyBase.StartCoroutine(Me.moveBack_cr())
		End If
		Dim start As Vector2 = MyBase.transform.position
		Dim [end] As Vector2 = New Vector2(-450F * MyBase.transform.localScale.x, MyBase.transform.position.y)
		MyBase.animator.Play("Dash", 1)
		AudioManager.PlayLoop("level_mouse_can_dash_loop")
		Me.dash = False
		While Not Me.dash
			Yield Nothing
		End While
		Yield MyBase.StartCoroutine(Me.tween_cr(MyBase.transform, start, [end], EaseUtils.EaseType.easeInSine, dashProperties.time))
		MyBase.animator.SetTrigger("CanContinue")
		AudioManager.[Stop]("level_mouse_can_dash_loop")
		AudioManager.Play("level_mouse_can_dash_stop")
		Me.emitAudioFromObject.Add("level_mouse_can_dash_stop")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Dash_End", 1, False, True)
		Yield CupheadTime.WaitForSeconds(Me, dashProperties.hesitate)
		Me.state = MouseLevelCanMouse.State.Idle
		Return
	End Function

	' Token: 0x060025A2 RID: 9634 RVA: 0x0015F9E7 File Offset: 0x0015DDE7
	Public Sub StartCherryBomb()
		Me.state = MouseLevelCanMouse.State.CherryBomb
		MyBase.StartCoroutine(Me.cherryBomb_cr())
		MyBase.StartCoroutine(Me.timedAudioMouseSnarky_cr())
		If Not Me.moving Then
			MyBase.StartCoroutine(Me.move_cr())
		End If
	End Sub

	' Token: 0x060025A3 RID: 9635 RVA: 0x0015FA24 File Offset: 0x0015DE24
	Private Sub FireCherryBomb()
		MyBase.animator.SetTrigger("Shoot")
		Dim vector As Vector2 = New Vector2(MyBase.properties.CurrentState.canCherryBomb.xVelocity * MyBase.transform.localScale.x, MyBase.properties.CurrentState.canCherryBomb.yVelocity)
		Me.cherryBombPrefab.Create(Me.cherryBombRoot.position, vector, MyBase.properties.CurrentState.canCherryBomb.gravity, CSng(MyBase.properties.CurrentState.canCherryBomb.childSpeed))
	End Sub

	' Token: 0x060025A4 RID: 9636 RVA: 0x0015FAD8 File Offset: 0x0015DED8
	Private Iterator Function cherryBomb_cr() As IEnumerator
		MyBase.animator.ResetTrigger("Continue")
		MyBase.animator.ResetTrigger("Shoot")
		Dim properties As LevelProperties.Mouse.CanCherryBomb = MyBase.properties.CurrentState.canCherryBomb
		Dim pattern As KeyValue() = KeyValue.ListFromString(properties.patterns(Global.UnityEngine.Random.Range(0, properties.patterns.Length)), New Char() { "P"c, "D"c })
		MyBase.animator.Play("Cannon_Start", 0)
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Cannon_Start", 0, False, True)
		For i As Integer = 0 To pattern.Length - 1
			If pattern(i).key = "P" Then
				Dim p As Integer = 0
				While CSng(p) < pattern(i).value
					Yield CupheadTime.WaitForSeconds(Me, properties.delay)
					Me.FireCherryBomb()
					p += 1
				End While
			Else
				Yield CupheadTime.WaitForSeconds(Me, pattern(i).value)
			End If
			Yield Nothing
		Next
		MyBase.animator.SetTrigger("Continue")
		Yield MyBase.animator.WaitForAnimationToStart(Me, "Idle_Down", 0, False)
		Yield CupheadTime.WaitForSeconds(Me, properties.hesitate)
		Me.state = MouseLevelCanMouse.State.Idle
		Return
	End Function

	' Token: 0x060025A5 RID: 9637 RVA: 0x0015FAF3 File Offset: 0x0015DEF3
	Public Sub StartCatapult()
		Me.state = MouseLevelCanMouse.State.Catapult
		MyBase.StartCoroutine(Me.catapult_cr())
		MyBase.StartCoroutine(Me.timedAudioMouseSnarky_cr())
		If Not Me.moving Then
			MyBase.StartCoroutine(Me.move_cr())
		End If
	End Sub

	' Token: 0x060025A6 RID: 9638 RVA: 0x0015FB30 File Offset: 0x0015DF30
	Private Sub FireCatapult()
		Dim canCatapult As LevelProperties.Mouse.CanCatapult = MyBase.properties.CurrentState.canCatapult
		Dim array As Char() = canCatapult.patterns.GetRandom().ToLower().ToCharArray()
		Dim num As Single = CSng(If((Me.direction <> MouseLevelCanMouse.Direction.Right), 165, (-45)))
		If array.Length <= 1 Then
			Me.catapultProjectilePrefab.CreateFromPrefab(Me.catapultRoot.position, num + canCatapult.angleOffset, CSng(canCatapult.projectileSpeed), array(0))
			Return
		End If
		For i As Integer = 0 To array.Length - 1
			Dim num2 As Single = num + canCatapult.spreadAngle / CSng((array.Length - 1)) * CSng(i)
			Me.catapultProjectilePrefab.CreateFromPrefab(Me.catapultRoot.position, num2, CSng(canCatapult.projectileSpeed), array(i))
		Next
	End Sub

	' Token: 0x060025A7 RID: 9639 RVA: 0x0015FC08 File Offset: 0x0015E008
	Private Iterator Function catapult_cr() As IEnumerator
		Dim properties As LevelProperties.Mouse.CanCatapult = MyBase.properties.CurrentState.canCatapult
		MyBase.animator.ResetTrigger("Continue")
		MyBase.animator.ResetTrigger("Shoot")
		MyBase.animator.Play("Catapult_Idle", 0)
		Yield MyBase.StartCoroutine(Me.tweenCatapultY_cr(-280F, 0F, properties.timeIn, EaseUtils.EaseType.easeOutSine))
		Yield CupheadTime.WaitForSeconds(Me, properties.pumpDelay)
		For i As Integer = 0 To properties.count - 1
			MyBase.animator.SetTrigger("Continue")
			Me.SoundMouseCatapultGlug()
			Yield MyBase.animator.WaitForAnimationToEnd(Me, "Catapult_Pump", 0, False, True)
			Yield CupheadTime.WaitForSeconds(Me, properties.pumpDelay)
			MyBase.animator.SetTrigger("Shoot")
			Yield MyBase.animator.WaitForAnimationToEnd(Me, "Catapult_Shoot", 0, False, True)
			Yield CupheadTime.WaitForSeconds(Me, properties.repeatDelay)
		Next
		Yield MyBase.StartCoroutine(Me.tweenCatapultY_cr(0F, -280F, properties.timeOut, EaseUtils.EaseType.easeOutSine))
		MyBase.animator.Play("Idle_Down", 0)
		Yield CupheadTime.WaitForSeconds(Me, CSng(properties.hesitate))
		Me.state = MouseLevelCanMouse.State.Idle
		Return
	End Function

	' Token: 0x060025A8 RID: 9640 RVA: 0x0015FC24 File Offset: 0x0015E024
	Private Iterator Function tweenCatapultY_cr(start As Single, [end] As Single, time As Single, ease As EaseUtils.EaseType) As IEnumerator
		Me.catapult.transform.SetLocalPosition(Nothing, New Single?(start), Nothing)
		Dim t As Single = 0F
		While t < time
			Dim val As Single = t / time
			Me.catapult.transform.SetLocalPosition(Nothing, New Single?(EaseUtils.Ease(ease, start, [end], val)), Nothing)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		Me.catapult.transform.SetLocalPosition(Nothing, New Single?([end]), Nothing)
		Yield Nothing
		Return
	End Function

	' Token: 0x060025A9 RID: 9641 RVA: 0x0015FC5C File Offset: 0x0015E05C
	Public Sub StartRomanCandle()
		Me.state = MouseLevelCanMouse.State.RomanCandle
		MyBase.StartCoroutine(Me.romanCandle_cr())
		MyBase.StartCoroutine(Me.timedAudioMouseSnarky_cr())
		If Not Me.moving Then
			MyBase.StartCoroutine(Me.move_cr())
		End If
	End Sub

	' Token: 0x060025AA RID: 9642 RVA: 0x0015FC98 File Offset: 0x0015E098
	Private Sub FireRomanCandle()
		Me.romanCandlePrefab.Create(Me.romanCandleRoot.position, CSng(If((MyBase.transform.localScale.x <= 0F), 0, 180)), MyBase.properties.CurrentState.canRomanCandle.speed, MyBase.properties.CurrentState.canRomanCandle.speed, MyBase.properties.CurrentState.canRomanCandle.rotationSpeed, 100F, MyBase.properties.CurrentState.canRomanCandle.timeBeforeHoming, PlayerManager.GetNext())
	End Sub

	' Token: 0x060025AB RID: 9643 RVA: 0x0015FD48 File Offset: 0x0015E148
	Private Iterator Function romanCandle_cr() As IEnumerator
		Dim properties As LevelProperties.Mouse.CanRomanCandle = MyBase.properties.CurrentState.canRomanCandle
		MyBase.animator.ResetTrigger("Continue")
		MyBase.animator.ResetTrigger("Shoot")
		For i As Integer = 0 To properties.count.RandomInt() - 1
			Yield CupheadTime.WaitForSeconds(Me, properties.repeatDelay)
			MyBase.animator.Play("Roman_Candle", 0)
			Yield MyBase.animator.WaitForAnimationToEnd(Me, "Roman_Candle", 0, False, True)
		Next
		Yield CupheadTime.WaitForSeconds(Me, properties.hesitate)
		Me.state = MouseLevelCanMouse.State.Idle
		Return
	End Function

	' Token: 0x060025AC RID: 9644 RVA: 0x0015FD63 File Offset: 0x0015E163
	Public Sub Explode(onStartPlatform As Action, onTransitionComplete As Action)
		Me.onStartPlatform = onStartPlatform
		Me.onTransitionComplete = onTransitionComplete
		MyBase.StartCoroutine(Me.explode_cr())
	End Sub

	' Token: 0x060025AD RID: 9645 RVA: 0x0015FD80 File Offset: 0x0015E180
	Private Iterator Function explode_cr() As IEnumerator
		While Me.state <> MouseLevelCanMouse.State.Idle
			Yield Nothing
		End While
		Me.sawBlades.Begin(MyBase.properties)
		Yield MyBase.StartCoroutine(Me.moveToX_cr(0F))
		MyBase.animator.Play("Explode", 1)
		Return
	End Function

	' Token: 0x060025AE RID: 9646 RVA: 0x0015FD9B File Offset: 0x0015E19B
	Private Sub OnExplodedAnim()
		Me.onStartPlatform()
		Me.SetWheels(False)
		If Me.brokenCan.state <> MouseLevelBrokenCanMouse.State.Dying Then
			Me.catPeeking.IsPhase2 = True
		End If
	End Sub

	' Token: 0x060025AF RID: 9647 RVA: 0x0015FDCC File Offset: 0x0015E1CC
	Private Sub SpawnBrokenCan()
		Me.brokenCan.StartPattern(MyBase.transform)
		Me.onTransitionComplete()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x060025B0 RID: 9648 RVA: 0x0015FDF8 File Offset: 0x0015E1F8
	Private Iterator Function tween_cr(trans As Transform, start As Vector2, [end] As Vector2, ease As EaseUtils.EaseType, time As Single) As IEnumerator
		Dim t As Single = 0F
		trans.position = start
		While t < time
			Dim val As Single = EaseUtils.Ease(ease, 0F, 1F, t / time)
			trans.position = Vector2.Lerp(start, [end], val)
			t += CupheadTime.Delta * Me.hitPauseCoefficient()
			Yield Nothing
		End While
		trans.position = [end]
		Yield Nothing
		Return
	End Function

	' Token: 0x060025B1 RID: 9649 RVA: 0x0015FE38 File Offset: 0x0015E238
	Private Sub SoundMouseCanIntro()
		AudioManager.Play("level_mouse_can_intro")
		Me.emitAudioFromObject.Add("level_mouse_can_intro")
	End Sub

	' Token: 0x060025B2 RID: 9650 RVA: 0x0015FE54 File Offset: 0x0015E254
	Private Sub SoundMouseCannonShoot()
		AudioManager.Play("level_mouse_can_cannon_shoot")
		Me.emitAudioFromObject.Add("level_mouse_can_cannon_shoot")
	End Sub

	' Token: 0x060025B3 RID: 9651 RVA: 0x0015FE70 File Offset: 0x0015E270
	Private Sub SoundMouseCannonEnd()
		AudioManager.Play("level_mouse_can_cannon_end")
		Me.emitAudioFromObject.Add("level_mouse_can_cannon_end")
	End Sub

	' Token: 0x060025B4 RID: 9652 RVA: 0x0015FE8C File Offset: 0x0015E28C
	Private Sub SoundMouseCatapultShoot()
		AudioManager.Play("level_mouse_can_catapult_shoot")
		Me.emitAudioFromObject.Add("level_mouse_can_catapult_shoot")
	End Sub

	' Token: 0x060025B5 RID: 9653 RVA: 0x0015FEA8 File Offset: 0x0015E2A8
	Private Sub SoundMouseCatapultGlug()
		AudioManager.Play("level_mouse_can_catapult_glug")
		Me.emitAudioFromObject.Add("level_mouse_can_catapult_glug")
	End Sub

	' Token: 0x060025B6 RID: 9654 RVA: 0x0015FEC4 File Offset: 0x0015E2C4
	Private Sub SoundMouseCanDashStart()
		AudioManager.Play("level_mouse_can_dash_start")
		Me.emitAudioFromObject.Add("level_mouse_can_dash_start")
	End Sub

	' Token: 0x060025B7 RID: 9655 RVA: 0x0015FEE0 File Offset: 0x0015E2E0
	Private Sub SoundMouseCanDashLoop()
		AudioManager.PlayLoop("level_mouse_can_dash_loop")
		Me.emitAudioFromObject.Add("level_mouse_can_dash_loop")
	End Sub

	' Token: 0x060025B8 RID: 9656 RVA: 0x0015FEFC File Offset: 0x0015E2FC
	Private Sub SoundMouseCanDashStop()
		AudioManager.PlayLoop("level_mouse_can_dash_stop")
		Me.emitAudioFromObject.Add("level_mouse_can_dash_stop")
	End Sub

	' Token: 0x060025B9 RID: 9657 RVA: 0x0015FF18 File Offset: 0x0015E318
	Private Sub SoundMouseCanDashEndAnim()
		AudioManager.Play("level_mouse_can_dash_end")
		Me.emitAudioFromObject.Add("level_mouse_can_dash_end")
	End Sub

	' Token: 0x060025BA RID: 9658 RVA: 0x0015FF34 File Offset: 0x0015E334
	Private Sub SoundMouseCanExplode()
		AudioManager.Play("level_mouse_can_explode")
	End Sub

	' Token: 0x060025BB RID: 9659 RVA: 0x0015FF40 File Offset: 0x0015E340
	Private Sub SoundMouseCanExplodePre()
		AudioManager.Play("level_mouse_can_explode_pre")
	End Sub

	' Token: 0x060025BC RID: 9660 RVA: 0x0015FF4C File Offset: 0x0015E34C
	Private Sub SoundMouseCanRomanCandle()
		AudioManager.Play("level_mouse_can_roman_candle")
		Me.emitAudioFromObject.Add("level_mouse_can_roman_candle")
	End Sub

	' Token: 0x060025BD RID: 9661 RVA: 0x0015FF68 File Offset: 0x0015E368
	Private Sub SoundMouseChargeVoice()
		AudioManager.Play("level_mouse_charge_voice")
		Me.emitAudioFromObject.Add("level_mouse_charge_voice")
	End Sub

	' Token: 0x060025BE RID: 9662 RVA: 0x0015FF84 File Offset: 0x0015E384
	Private Iterator Function timedAudioMouseSnarky_cr() As IEnumerator
		Yield New WaitForSeconds(1F)
		AudioManager.Play("level_mouse_snarky_voice")
		Return
	End Function

	' Token: 0x04002E1E RID: 11806
	Private Const MOUSE_LAYER As Integer = 0

	' Token: 0x04002E1F RID: 11807
	Private Const CAN_LAYER As Integer = 1

	' Token: 0x04002E20 RID: 11808
	Private Const MOVE_START_X As Single = 500F

	' Token: 0x04002E21 RID: 11809
	Private Const DASH_END_X As Single = 450F

	' Token: 0x04002E22 RID: 11810
	<Header("Cannon")>
	<SerializeField()>
	Private cherryBombRoot As Transform

	' Token: 0x04002E23 RID: 11811
	<SerializeField()>
	Private cherryBombPrefab As MouseLevelCherryBombProjectile

	' Token: 0x04002E24 RID: 11812
	<Header("Catapult")>
	<SerializeField()>
	Private catapult As Transform

	' Token: 0x04002E25 RID: 11813
	<SerializeField()>
	Private catapultProjectilePrefab As MouseLevelCanCatapultProjectile

	' Token: 0x04002E26 RID: 11814
	<SerializeField()>
	Private catapultRoot As Transform

	' Token: 0x04002E27 RID: 11815
	<Header("Roman Candle")>
	<SerializeField()>
	Private romanCandleRoot As Transform

	' Token: 0x04002E28 RID: 11816
	<SerializeField()>
	Private romanCandlePrefab As MouseLevelRomanCandleProjectile

	' Token: 0x04002E29 RID: 11817
	<Header("Wheels")>
	<SerializeField()>
	Private wheelRenderer As SpriteRenderer

	' Token: 0x04002E2A RID: 11818
	<SerializeField()>
	Private wheelSprites As Sprite()

	' Token: 0x04002E2B RID: 11819
	<SerializeField()>
	Private brokenCan As MouseLevelBrokenCanMouse

	' Token: 0x04002E2C RID: 11820
	<SerializeField()>
	Private sawBlades As MouseLevelSawBladeManager

	' Token: 0x04002E2D RID: 11821
	<SerializeField()>
	Private catPeeking As MouseLevelCatPeeking

	' Token: 0x04002E2E RID: 11822
	<Header("Springs")>
	<SerializeField()>
	Private springs As MouseLevelSpring()

	' Token: 0x04002E30 RID: 11824
	Private direction As MouseLevelCanMouse.Direction

	' Token: 0x04002E31 RID: 11825
	Private damageReceiver As DamageReceiver

	' Token: 0x04002E32 RID: 11826
	Private damageDealer As DamageDealer

	' Token: 0x04002E33 RID: 11827
	Private moving As Boolean

	' Token: 0x04002E34 RID: 11828
	Private peeking As Boolean

	' Token: 0x04002E35 RID: 11829
	Private overrideMove As Boolean

	' Token: 0x04002E36 RID: 11830
	Private exitAfterMoveBack As Boolean

	' Token: 0x04002E37 RID: 11831
	Private overrideMoveX As Single

	' Token: 0x04002E38 RID: 11832
	Private dash As Boolean

	' Token: 0x04002E39 RID: 11833
	Private Const FLIP_OFFSET As Single = 40F

	' Token: 0x04002E3A RID: 11834
	Private onStartPlatform As Action

	' Token: 0x04002E3B RID: 11835
	Private onTransitionComplete As Action

	' Token: 0x020006E3 RID: 1763
	Public Enum State
		' Token: 0x04002E3D RID: 11837
		Intro
		' Token: 0x04002E3E RID: 11838
		Idle
		' Token: 0x04002E3F RID: 11839
		Dash
		' Token: 0x04002E40 RID: 11840
		CherryBomb
		' Token: 0x04002E41 RID: 11841
		Catapult
		' Token: 0x04002E42 RID: 11842
		RomanCandle
	End Enum

	' Token: 0x020006E4 RID: 1764
	Public Enum Direction
		' Token: 0x04002E44 RID: 11844
		Left
		' Token: 0x04002E45 RID: 11845
		Right
	End Enum
End Class
