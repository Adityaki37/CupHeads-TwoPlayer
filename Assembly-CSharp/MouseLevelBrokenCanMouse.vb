Imports System
Imports System.Collections
Imports System.Linq
Imports UnityEngine

' Token: 0x020006DE RID: 1758
Public Class MouseLevelBrokenCanMouse
	Inherits LevelProperties.Mouse.Entity

	' Token: 0x170003C0 RID: 960
	' (get) Token: 0x0600256E RID: 9582 RVA: 0x0015DE61 File Offset: 0x0015C261
	' (set) Token: 0x0600256F RID: 9583 RVA: 0x0015DE69 File Offset: 0x0015C269
	Public Property state As MouseLevelBrokenCanMouse.State

	' Token: 0x06002570 RID: 9584 RVA: 0x0015DE74 File Offset: 0x0015C274
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.platformLocalPos = Me.platform.localPosition
		Me.platform.transform.parent = Nothing
		Me.setFlameCollidersEnabled(False)
		Me.colliders = Me.mouse.GetComponents(Of Collider2D)()
		For i As Integer = 0 To Me.colliders.Length - 1
			Me.colliders(i).enabled = False
		Next
	End Sub

	' Token: 0x06002571 RID: 9585 RVA: 0x0015DF1C File Offset: 0x0015C31C
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
		Me.platform.position = New Vector2(MyBase.transform.position.x, MyBase.transform.position.y) + Me.platformLocalPos
		If Me.peeking AndAlso MyBase.properties.CurrentHealth < MyBase.properties.TotalHealth * Me.catPeeking.Peek2Threshold Then
			Me.catPeeking.StopPeeking()
			Me.peeking = False
		End If
	End Sub

	' Token: 0x06002572 RID: 9586 RVA: 0x0015DFC9 File Offset: 0x0015C3C9
	Private Sub LateUpdate()
		Me.leftFlame.UpdateParentTransform(MyBase.transform)
		Me.rightFlame.UpdateParentTransform(MyBase.transform)
	End Sub

	' Token: 0x06002573 RID: 9587 RVA: 0x0015DFED File Offset: 0x0015C3ED
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If Me.damageDealer IsNot Nothing AndAlso phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06002574 RID: 9588 RVA: 0x0015E016 File Offset: 0x0015C416
	Public Overrides Sub LevelInit(properties As LevelProperties.Mouse)
		MyBase.LevelInit(properties)
		AddHandler properties.OnBossDeath, AddressOf Me.OnBossDeath
	End Sub

	' Token: 0x06002575 RID: 9589 RVA: 0x0015E031 File Offset: 0x0015C431
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		MyBase.properties.DealDamage(info.damage)
	End Sub

	' Token: 0x06002576 RID: 9590 RVA: 0x0015E044 File Offset: 0x0015C444
	Private Function hitPauseCoefficient() As Single
		Return If((Not MyBase.GetComponent(Of DamageReceiver)().IsHitPaused), 1F, 0F)
	End Function

	' Token: 0x06002577 RID: 9591 RVA: 0x0015E065 File Offset: 0x0015C465
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.bulletPrefab = Nothing
	End Sub

	' Token: 0x06002578 RID: 9592 RVA: 0x0015E074 File Offset: 0x0015C474
	Public Sub StartPattern(transform As Transform)
		For i As Integer = 0 To Me.colliders.Length - 1
			Me.colliders(i).enabled = True
		Next
		MyBase.transform.position = transform.position
		MyBase.transform.localScale = transform.localScale
		MyBase.animator.SetTrigger("Continue")
		If Me.state <> MouseLevelBrokenCanMouse.State.Dying Then
			Me.state = MouseLevelBrokenCanMouse.State.Down
			MyBase.StartCoroutine("main_cr")
		End If
		MyBase.StartCoroutine(Me.move_cr())
		Me.direction = If((transform.localScale.x <= 0F), MouseLevelBrokenCanMouse.Direction.Right, MouseLevelBrokenCanMouse.Direction.Left)
	End Sub

	' Token: 0x06002579 RID: 9593 RVA: 0x0015E12C File Offset: 0x0015C52C
	Private Iterator Function main_cr() As IEnumerator
		Dim patternState As LevelProperties.Mouse.State = MyBase.properties.CurrentState
		Dim pattern As String() = patternState.brokenCanFlame.attackString.RandomChoice().Split(New Char() { ","c })
		While Not Me.dead
			If patternState IsNot MyBase.properties.CurrentState Then
				If Not patternState.brokenCanFlame.attackString.SequenceEqual(MyBase.properties.CurrentState.brokenCanFlame.attackString) Then
					pattern = MyBase.properties.CurrentState.brokenCanFlame.attackString.RandomChoice().Split(New Char() { ","c })
				End If
				patternState = MyBase.properties.CurrentState
			End If
			Dim p As LevelProperties.Mouse.BrokenCanFlame = MyBase.properties.CurrentState.brokenCanFlame
			For Each instruction As String In pattern
				Me.state = If((Me.state <> MouseLevelBrokenCanMouse.State.Down), MouseLevelBrokenCanMouse.State.Down, MouseLevelBrokenCanMouse.State.Up)
				MyBase.animator.SetTrigger(If((Me.state <> MouseLevelBrokenCanMouse.State.Down), "Up", "Down"))
				Yield MyBase.animator.WaitForAnimationToEnd(Me, If((Me.state <> MouseLevelBrokenCanMouse.State.Down), "Going_Up", "Going_Down"), 1, False, True)
				MyBase.animator.SetTrigger("Continue")
				Yield CupheadTime.WaitForSeconds(Me, p.delayBeforeShot)
				If instruction = "BF" Then
					MyBase.animator.SetTrigger("Shoot")
					Dim leftBullet As BasicProjectile = Me.bulletPrefab.Create(Me.leftBulletRoot.position, CSng(If((MyBase.transform.localScale.x <= 0F), 0, 180)), p.shotSpeed)
					leftBullet.SetParryable(True)
					Dim rightBullet As BasicProjectile = Me.bulletPrefab.Create(Me.rightBulletRoot.position, CSng(If((MyBase.transform.localScale.x <= 0F), 180, 0)), p.shotSpeed)
					rightBullet.SetParryable(True)
					Yield MyBase.animator.WaitForAnimationToEnd(Me, "Fire", 2, False, True)
					Yield CupheadTime.WaitForSeconds(Me, p.delayAfterShot)
				End If
				MyBase.StartCoroutine(Me.scale_flames_cr(True))
				MyBase.animator.SetTrigger("Flame")
				Yield CupheadTime.WaitForSeconds(Me, p.chargeTime)
				MyBase.animator.SetTrigger("FlameContinue")
				Me.setFlameCollidersEnabled(True)
				Me.flameOn = True
				Yield CupheadTime.WaitForSeconds(Me, p.loopTime)
				Me.setFlameCollidersEnabled(False)
				MyBase.StartCoroutine(Me.scale_flames_cr(False))
				Me.flameOn = False
				MyBase.animator.SetTrigger("FlameContinue")
				Yield MyBase.animator.WaitForAnimationToEnd(Me, "End", 4, False, True)
			Next
		End While
		Return
	End Function

	' Token: 0x0600257A RID: 9594 RVA: 0x0015E148 File Offset: 0x0015C548
	Private Iterator Function scale_flames_cr(turningOn As Boolean) As IEnumerator
		Dim t As Single = 0F
		Dim time As Single = 1F
		If turningOn Then
			Me.leftFlame.transform.SetScale(New Single?(1F), New Single?(1F), New Single?(0F))
			Me.rightFlame.transform.SetScale(New Single?(1F), New Single?(1F), New Single?(0F))
			Me.leftFlameSprite.transform.SetScale(New Single?(Me.finalFlameScale.x), New Single?(Me.finalFlameScale.y), New Single?(0F))
			Me.rightFlameSprite.transform.SetScale(New Single?(Me.finalFlameScale.x), New Single?(Me.finalFlameScale.y), New Single?(0F))
			Me.leftFlameSprite.enabled = True
			Me.rightFlameSprite.enabled = True
		End If
		While t < time
			t += CupheadTime.Delta
			If turningOn Then
				Me.leftFlame.transform.SetScale(New Single?(-(t / time)), New Single?(t / time), New Single?(0F))
				Me.rightFlame.transform.SetScale(New Single?(t / time), New Single?(t / time), New Single?(0F))
				Me.leftFlameSprite.transform.SetScale(New Single?(t / time * Me.finalFlameScale.x), New Single?(t / time * Me.finalFlameScale.y), New Single?(0F))
				Me.rightFlameSprite.transform.SetScale(New Single?(t / time * Me.finalFlameScale.x), New Single?(t / time * Me.finalFlameScale.y), New Single?(0F))
			Else
				Me.leftFlame.transform.SetScale(New Single?(-1F + t / time), New Single?(1F - t / time), New Single?(0F))
				Me.rightFlame.transform.SetScale(New Single?(1F - t / time), New Single?(1F - t / time), New Single?(0F))
				Me.leftFlameSprite.transform.SetScale(New Single?(Me.finalFlameScale.x - t / time * Me.finalFlameScale.x), New Single?(Me.finalFlameScale.y - t / time * Me.finalFlameScale.y), New Single?(0F))
				Me.rightFlameSprite.transform.SetScale(New Single?(Me.finalFlameScale.x - t / time * Me.finalFlameScale.x), New Single?(Me.finalFlameScale.y - t / time * Me.finalFlameScale.y), New Single?(0F))
			End If
			Yield Nothing
		End While
		If Not turningOn Then
			Me.leftFlame.transform.SetScale(New Single?(0F), New Single?(0F), New Single?(0F))
			Me.rightFlame.transform.SetScale(New Single?(0F), New Single?(0F), New Single?(0F))
			Me.leftFlameSprite.transform.SetScale(New Single?(0F), New Single?(0F), New Single?(0F))
			Me.rightFlameSprite.transform.SetScale(New Single?(0F), New Single?(0F), New Single?(0F))
			Me.leftFlameSprite.enabled = False
			Me.rightFlameSprite.enabled = False
		End If
		Yield Nothing
		Return
	End Function

	' Token: 0x0600257B RID: 9595 RVA: 0x0015E16A File Offset: 0x0015C56A
	Private Sub setFlameCollidersEnabled(enabled As Boolean)
		Me.leftFlame.SetColliderEnabled(enabled)
		Me.rightFlame.SetColliderEnabled(enabled)
		If enabled Then
			Me.SoundMouseFlameThrower()
		End If
	End Sub

	' Token: 0x0600257C RID: 9596 RVA: 0x0015E190 File Offset: 0x0015C590
	Private Iterator Function moveToX_cr(x As Single) As IEnumerator
		Me.overrideMove = True
		Me.overrideMoveX = x
		While Me.overrideMove
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600257D RID: 9597 RVA: 0x0015E1B4 File Offset: 0x0015C5B4
	Private Iterator Function move_cr() As IEnumerator
		Dim startPos As Vector2 = MyBase.transform.position
		Dim overridden As Boolean
		Do
			Dim p As LevelProperties.Mouse.BrokenCanMove = MyBase.properties.CurrentState.brokenCanMove
			Dim targetPos As Vector2 = startPos
			overridden = False
			If Me.overrideMove Then
				targetPos.x = Me.overrideMoveX
				overridden = True
			ElseIf Me.direction = MouseLevelBrokenCanMouse.Direction.Left Then
				targetPos.x -= p.maxXPositionRange.RandomFloat()
			Else
				targetPos.x += p.maxXPositionRange.RandomFloat()
			End If
			Dim time As Single = Mathf.Abs(targetPos.x - MyBase.transform.position.x) / p.speed
			Yield MyBase.StartCoroutine(Me.tween_cr(MyBase.transform, MyBase.transform.position, targetPos, EaseUtils.EaseType.easeInOutSine, time))
			Yield CupheadTime.WaitForSeconds(Me, 0.25F)
			Me.direction = If((Me.direction <> MouseLevelBrokenCanMouse.Direction.Left), MouseLevelBrokenCanMouse.Direction.Left, MouseLevelBrokenCanMouse.Direction.Right)
		Loop While Not Me.overrideMove OrElse Not overridden
		Me.overrideMove = False
		MyBase.animator.Play("Idle", 0)
		Return
	End Function

	' Token: 0x0600257E RID: 9598 RVA: 0x0015E1D0 File Offset: 0x0015C5D0
	Private Iterator Function tween_cr(trans As Transform, start As Vector2, [end] As Vector2, ease As EaseUtils.EaseType, time As Single) As IEnumerator
		Dim t As Single = 0F
		trans.position = start
		While t < time
			Dim val As Single = EaseUtils.Ease(ease, 0F, 1F, t / time)
			trans.position = Vector2.Lerp(start, [end], val)
			t += CupheadTime.Delta * Me.hitPauseCoefficient()
			Dim wheelAnimProgress As Single = -MyBase.transform.localScale.x * MyBase.transform.position.x / 100F
			wheelAnimProgress = wheelAnimProgress Mod 1F
			If wheelAnimProgress < 0F Then
				wheelAnimProgress += 1F
			End If
			MyBase.animator.Play("Move", 0, wheelAnimProgress)
			Yield Nothing
		End While
		trans.position = [end]
		Yield Nothing
		Return
	End Function

	' Token: 0x0600257F RID: 9599 RVA: 0x0015E210 File Offset: 0x0015C610
	Public Sub OnBossDeath()
		For Each collider2D As Collider2D In Me.mouse.GetComponentsInChildren(Of Collider2D)()
			collider2D.enabled = False
		Next
		Me.StopAllCoroutines()
		Me.state = MouseLevelBrokenCanMouse.State.Dying
		MyBase.StartCoroutine(Me.death_cr(False))
	End Sub

	' Token: 0x06002580 RID: 9600 RVA: 0x0015E263 File Offset: 0x0015C663
	Public Sub Transform()
		Me.state = MouseLevelBrokenCanMouse.State.Dying
		Me.SoundMouseScreamVoice()
		MyBase.StartCoroutine(Me.death_cr(True))
		RemoveHandler MyBase.properties.OnBossDeath, AddressOf Me.OnBossDeath
	End Sub

	' Token: 0x06002581 RID: 9601 RVA: 0x0015E297 File Offset: 0x0015C697
	Public Sub BeEaten()
		Global.UnityEngine.[Object].Destroy(Me.leftFlame.gameObject)
		Global.UnityEngine.[Object].Destroy(Me.rightFlame.gameObject)
		Global.UnityEngine.[Object].Destroy(Me.platform.gameObject)
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06002582 RID: 9602 RVA: 0x0015E2D4 File Offset: 0x0015C6D4
	Private Iterator Function death_cr(transform As Boolean) As IEnumerator
		MyBase.animator.SetTrigger("Die")
		Me.sawBlades.Leave()
		If transform Then
			MyBase.animator.SetTrigger("Down")
		Else
			MyBase.animator.SetBool("CrazyScissor", True)
		End If
		If Me.flameOn Then
			MyBase.animator.SetTrigger("FlameContinue")
		Else
			MyBase.animator.ResetTrigger("Flame")
			AudioManager.Play("level_mouse_scream_death_voice")
		End If
		Me.leftFlame.SetColliderEnabled(False)
		Me.rightFlame.SetColliderEnabled(False)
		MyBase.StopCoroutine("main_cr")
		If transform Then
			Yield MyBase.StartCoroutine(Me.moveToX_cr(0F))
			Yield MyBase.animator.WaitForAnimationToStart(Me, "Idle_Down", 1, False)
			Me.cat.StartIntro()
		End If
		Return
	End Function

	' Token: 0x06002583 RID: 9603 RVA: 0x0015E2F6 File Offset: 0x0015C6F6
	Private Sub SoundMouseBrkCanScissorUp()
		AudioManager.Play("level_mouse_broken_can_scissor_going_up")
		Me.emitAudioFromObject.Add("level_mouse_broken_can_scissor_going_up")
	End Sub

	' Token: 0x06002584 RID: 9604 RVA: 0x0015E312 File Offset: 0x0015C712
	Private Sub SoundMouseBrkCanScissorDown()
		AudioManager.Play("level_mouse_broken_can_scissor_going_down")
		Me.emitAudioFromObject.Add("level_mouse_broken_can_scissor_going_down")
	End Sub

	' Token: 0x06002585 RID: 9605 RVA: 0x0015E32E File Offset: 0x0015C72E
	Private Sub SoundMouseBrkCanStartUp()
		AudioManager.Play("level_mouse_broken_can_start_up")
		Me.emitAudioFromObject.Add("level_mouse_broken_can_start_up")
	End Sub

	' Token: 0x06002586 RID: 9606 RVA: 0x0015E34A File Offset: 0x0015C74A
	Private Sub SoundMouseFlameThrower()
		AudioManager.Play("level_mouse_flamethrower")
		Me.emitAudioFromObject.Add("level_mouse_flamethrower")
	End Sub

	' Token: 0x06002587 RID: 9607 RVA: 0x0015E366 File Offset: 0x0015C766
	Private Sub SoundMouseSnarkyVoice()
		AudioManager.Play("level_mouse_snarky_voice")
		Me.emitAudioFromObject.Add("level_mouse_snarky_voice")
	End Sub

	' Token: 0x06002588 RID: 9608 RVA: 0x0015E382 File Offset: 0x0015C782
	Private Sub SoundMouseScreamVoice()
		AudioManager.Play("level_mouse_scream_voice")
		Me.emitAudioFromObject.Add("level_mouse_scream_voice")
	End Sub

	' Token: 0x04002DF7 RID: 11767
	Private Const CART_LAYER As Integer = 0

	' Token: 0x04002DF8 RID: 11768
	Private Const SCISSOR_LAYER As Integer = 1

	' Token: 0x04002DF9 RID: 11769
	Private Const CANNON_LAYER As Integer = 2

	' Token: 0x04002DFA RID: 11770
	Private Const MOUSE_LAYER As Integer = 3

	' Token: 0x04002DFB RID: 11771
	Private Const FLAME_LAYER As Integer = 4

	' Token: 0x04002DFC RID: 11772
	Private Const CAN_LAYER As Integer = 5

	' Token: 0x04002DFD RID: 11773
	<SerializeField()>
	Private leftFlame As MouseLevelFlame

	' Token: 0x04002DFE RID: 11774
	<SerializeField()>
	Private rightFlame As MouseLevelFlame

	' Token: 0x04002DFF RID: 11775
	<SerializeField()>
	Private leftFlameSprite As SpriteRenderer

	' Token: 0x04002E00 RID: 11776
	<SerializeField()>
	Private rightFlameSprite As SpriteRenderer

	' Token: 0x04002E01 RID: 11777
	<SerializeField()>
	Private finalFlameScale As Vector2

	' Token: 0x04002E02 RID: 11778
	<SerializeField()>
	Private leftBulletRoot As Transform

	' Token: 0x04002E03 RID: 11779
	<SerializeField()>
	Private rightBulletRoot As Transform

	' Token: 0x04002E04 RID: 11780
	<SerializeField()>
	Private mouse As Transform

	' Token: 0x04002E05 RID: 11781
	<SerializeField()>
	Private platform As Transform

	' Token: 0x04002E06 RID: 11782
	<SerializeField()>
	Private bulletPrefab As BasicProjectile

	' Token: 0x04002E07 RID: 11783
	<SerializeField()>
	Private sawBlades As MouseLevelSawBladeManager

	' Token: 0x04002E08 RID: 11784
	<SerializeField()>
	Private cat As MouseLevelCat

	' Token: 0x04002E09 RID: 11785
	<SerializeField()>
	Private catPeeking As MouseLevelCatPeeking

	' Token: 0x04002E0B RID: 11787
	Private direction As MouseLevelBrokenCanMouse.Direction

	' Token: 0x04002E0C RID: 11788
	Private damageReceiver As DamageReceiver

	' Token: 0x04002E0D RID: 11789
	Private damageDealer As DamageDealer

	' Token: 0x04002E0E RID: 11790
	Private flameOn As Boolean

	' Token: 0x04002E0F RID: 11791
	Private platformLocalPos As Vector2

	' Token: 0x04002E10 RID: 11792
	Private dead As Boolean

	' Token: 0x04002E11 RID: 11793
	Private peeking As Boolean = True

	' Token: 0x04002E12 RID: 11794
	Private colliders As Collider2D()

	' Token: 0x04002E13 RID: 11795
	Private overrideMove As Boolean

	' Token: 0x04002E14 RID: 11796
	Private overrideMoveX As Single

	' Token: 0x04002E15 RID: 11797
	Private Const WHEEL_MOVE_FACTOR As Single = 100F

	' Token: 0x020006DF RID: 1759
	Public Enum State
		' Token: 0x04002E17 RID: 11799
		Init
		' Token: 0x04002E18 RID: 11800
		Down
		' Token: 0x04002E19 RID: 11801
		Up
		' Token: 0x04002E1A RID: 11802
		Dying
	End Enum

	' Token: 0x020006E0 RID: 1760
	Public Enum Direction
		' Token: 0x04002E1C RID: 11804
		Left
		' Token: 0x04002E1D RID: 11805
		Right
	End Enum
End Class
