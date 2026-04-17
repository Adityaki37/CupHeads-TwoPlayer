Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200053F RID: 1343
Public Class ChessKnightLevelKnight
	Inherits LevelProperties.ChessKnight.Entity

	' Token: 0x17000337 RID: 823
	' (get) Token: 0x06001885 RID: 6277 RVA: 0x000DDEB1 File Offset: 0x000DC2B1
	' (set) Token: 0x06001886 RID: 6278 RVA: 0x000DDEB9 File Offset: 0x000DC2B9
	Public Property state As ChessKnightLevelKnight.State

	' Token: 0x06001887 RID: 6279 RVA: 0x000DDEC4 File Offset: 0x000DC2C4
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		Gizmos.color = Color.red
		Dim num As Single = -640F + Me.positionBoundaryInset.minimum
		Gizmos.DrawLine(New Vector3(num, -500F), New Vector3(num, 500F))
		Gizmos.color = Color.green
		num = -640F + Me.positionBoundaryInset.maximum
		Gizmos.DrawLine(New Vector3(num, -500F), New Vector3(num, 500F))
		Gizmos.color = Color.red
		num = 640F - Me.positionBoundaryInset.minimum
		Gizmos.DrawLine(New Vector3(num, -500F), New Vector3(num, 500F))
		Gizmos.color = Color.green
		num = 640F - Me.positionBoundaryInset.maximum
		Gizmos.DrawLine(New Vector3(num, -500F), New Vector3(num, 500F))
	End Sub

	' Token: 0x06001888 RID: 6280 RVA: 0x000DDFB4 File Offset: 0x000DC3B4
	Public Overrides Sub LevelInit(properties As LevelProperties.ChessKnight)
		MyBase.LevelInit(properties)
		AddHandler Level.Current.OnIntroEvent, AddressOf Me.onIntroEventHandler
		Dim knight As LevelProperties.ChessKnight.Knight = properties.CurrentState.knight
		Me.attackIntervalPattern = New PatternString(knight.attackIntervalString, True, True)
		Dim player As AbstractPlayerController = PlayerManager.GetPlayer(PlayerId.PlayerOne)
		Dim player2 As AbstractPlayerController = PlayerManager.GetPlayer(PlayerId.PlayerTwo)
		If player2 IsNot Nothing AndAlso Not player2.IsDead Then
			Me.targetPlayer = If((Not Rand.Bool()), player2, player)
		End If
		Me.battleStartPosition = MyBase.transform.position.x
		Me.movementPattern = New PatternString(properties.CurrentState.movement.movementString, True, True)
		Me.numberTauntString = New PatternString(properties.CurrentState.tauntAttack.numberTauntString, True)
		Me.numberTauntString.SetSubStringIndex(-1)
		Me.tauntAttackCounter = Me.numberTauntString.PopInt()
	End Sub

	' Token: 0x06001889 RID: 6281 RVA: 0x000DE0A8 File Offset: 0x000DC4A8
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Dim position As Vector3 = MyBase.transform.position
		position.x = 640F - (Me.positionBoundaryInset.maximum + Me.positionBoundaryInset.minimum) * 0.5F
		MyBase.transform.position = position
	End Sub

	' Token: 0x0600188A RID: 6282 RVA: 0x000DE100 File Offset: 0x000DC500
	Private Sub Start()
		Me.damageDealer = DamageDealer.NewEnemy()
		AddHandler Me.pink.OnActivate, AddressOf Me.GotParried
		AddHandler Me.swordHitbox.OnPlayerCollision, AddressOf Me.OnCollisionPlayer
		AddHandler Me.upHitbox.OnPlayerCollision, AddressOf Me.OnCollisionPlayer
	End Sub

	' Token: 0x0600188B RID: 6283 RVA: 0x000DE15F File Offset: 0x000DC55F
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x0600188C RID: 6284 RVA: 0x000DE177 File Offset: 0x000DC577
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x0600188D RID: 6285 RVA: 0x000DE198 File Offset: 0x000DC598
	Private Sub GotParried()
		MyBase.properties.DealDamage(If((Not PlayerManager.BothPlayersActive()), 10F, ChessKingLevelKing.multiplayerDamageNerf))
		Me.hitFlash.Flash(0.7F)
		MyBase.StartCoroutine(Me.parry_timer_cr())
		If MyBase.properties.CurrentHealth <= 0F AndAlso Me.state <> ChessKnightLevelKnight.State.Death Then
			Me.state = ChessKnightLevelKnight.State.Death
			Me.death()
		End If
	End Sub

	' Token: 0x0600188E RID: 6286 RVA: 0x000DE214 File Offset: 0x000DC614
	Private Iterator Function parry_timer_cr() As IEnumerator
		Me.pink.gameObject.SetActive(False)
		Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.knight.parryCooldown)
		Me.pink.gameObject.SetActive(True)
		Return
	End Function

	' Token: 0x0600188F RID: 6287 RVA: 0x000DE22F File Offset: 0x000DC62F
	Private Sub onIntroEventHandler()
		MyBase.StartCoroutine(Me.intro_cr())
	End Sub

	' Token: 0x06001890 RID: 6288 RVA: 0x000DE240 File Offset: 0x000DC640
	Private Iterator Function intro_cr() As IEnumerator
		MyBase.animator.SetTrigger("Intro")
		Yield MyBase.animator.WaitForNormalizedTime(Me, 1F, "Intro", 0, False, False, True)
		Me.EndAttack()
		Return
	End Function

	' Token: 0x06001891 RID: 6289 RVA: 0x000DE25B File Offset: 0x000DC65B
	Private Sub EndAttack()
		Me.isTauntAttack = False
		Me.SFX_KOG_KNIGHT_RecoverFoley()
		Me.state = ChessKnightLevelKnight.State.Move
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x06001892 RID: 6290 RVA: 0x000DE280 File Offset: 0x000DC680
	Private Iterator Function move_cr() As IEnumerator
		If Me.tauntAttackCounter > 0 Then
			Dim p As LevelProperties.ChessKnight.Movement = MyBase.properties.CurrentState.movement
			Dim idleTime As Single = Me.attackIntervalPattern.PopFloat()
			Dim idleT As Single = 0F
			Dim moveT As Single = 0F
			Dim wait As YieldInstruction = New WaitForFixedUpdate()
			Dim startPosition As Single = MyBase.transform.position.x
			Dim movementAmount As Single = Me.movementPattern.PopFloat()
			Me.goingLeft = Me.chooseGoingLeft(movementAmount)
			Dim endPosition As Single = Me.getWalkingEndPosition(movementAmount)
			Dim moveTime As Single = Mathf.Abs(endPosition - startPosition) / p.movementSpeed
			MyBase.animator.SetBool("FacingLeft", Me.facingLeft)
			MyBase.animator.SetBool("Walking", True)
			While True
				idleT += CupheadTime.FixedDelta
				Dim previousPosition As Vector3 = MyBase.transform.position
				If moveT < moveTime Then
					moveT += CupheadTime.FixedDelta
					If p.hasEasing Then
						Dim num As Single = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0F, 1F, moveT / moveTime)
						MyBase.transform.SetPosition(New Single?(Mathf.Lerp(startPosition, endPosition, num)), Nothing, Nothing)
					ElseIf Me.goingLeft AndAlso MyBase.transform.position.x > endPosition Then
						MyBase.transform.position += Vector3.left * p.movementSpeed * CupheadTime.FixedDelta
					ElseIf Not Me.goingLeft AndAlso MyBase.transform.position.x < endPosition Then
						MyBase.transform.position += Vector3.right * p.movementSpeed * CupheadTime.FixedDelta
					End If
				Else
					If idleT > idleTime Then
						Exit For
					End If
					Me.goingLeft = Not Me.goingLeft
					MyBase.transform.SetPosition(New Single?(endPosition), Nothing, Nothing)
					startPosition = endPosition
					endPosition = Me.getWalkingEndPosition(Me.movementPattern.PopFloat())
					moveTime = Mathf.Abs(endPosition - startPosition) / p.movementSpeed
					moveT = 0F
				End If
				Me.updateAnimatorSpeed(MyBase.transform.position, previousPosition)
				Yield wait
			End While
			Me.goingLeft = Not Me.goingLeft
		End If
		Me.CheckTaunt()
		Return
	End Function

	' Token: 0x06001893 RID: 6291 RVA: 0x000DE29C File Offset: 0x000DC69C
	Private Function chooseGoingLeft(movementAmount As Single) As Boolean
		Dim flag As Boolean = Rand.Bool()
		Dim num As Single
		If Me.facingLeft Then
			num = If((Not flag), (640F - Me.positionBoundaryInset.minimum), (640F - Me.positionBoundaryInset.maximum))
		Else
			num = If((Not flag), (-640F + Me.positionBoundaryInset.maximum), (-640F + Me.positionBoundaryInset.minimum))
		End If
		Dim num2 As Single = num - MyBase.transform.position.x
		If Mathf.Abs(num2 / movementAmount) < 0.5F Then
			flag = Not flag
		End If
		Return flag
	End Function

	' Token: 0x06001894 RID: 6292 RVA: 0x000DE34C File Offset: 0x000DC74C
	Private Function getWalkingEndPosition(movementAmount As Single) As Single
		Dim num As Single = MyBase.transform.position.x + If((Not Me.goingLeft), movementAmount, (-movementAmount)) * 2F
		Return Me.clampEndPosition(num, Me.facingLeft)
	End Function

	' Token: 0x06001895 RID: 6293 RVA: 0x000DE398 File Offset: 0x000DC798
	Private Sub CheckTaunt()
		Dim player As AbstractPlayerController = PlayerManager.GetPlayer(PlayerId.PlayerOne)
		Dim player2 As AbstractPlayerController = PlayerManager.GetPlayer(PlayerId.PlayerTwo)
		Dim tauntDistance As Single = MyBase.properties.CurrentState.taunt.tauntDistance
		Dim num As Single = Mathf.Abs(player.transform.position.x - MyBase.transform.position.x)
		Dim num2 As Single = If((Not(player2 IsNot Nothing) OrElse player2.IsDead), num, Mathf.Abs(player2.transform.position.x - MyBase.transform.position.x))
		If num > tauntDistance AndAlso num2 > tauntDistance Then
			If Me.tauntAttackCounter <= 0 Then
				Me.isTauntAttack = True
				MyBase.StartCoroutine(Me.long_cr())
			Else
				MyBase.StartCoroutine(Me.taunt_cr())
			End If
		Else
			Me.state = ChessKnightLevelKnight.State.Idle
		End If
	End Sub

	' Token: 0x06001896 RID: 6294 RVA: 0x000DE494 File Offset: 0x000DC894
	Private Iterator Function taunt_cr() As IEnumerator
		Me.state = ChessKnightLevelKnight.State.Taunt
		MyBase.animator.SetBool("Taunting", True)
		MyBase.animator.SetBool("Walking", False)
		Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.taunt.tauntDuration)
		MyBase.animator.SetBool("Taunting", False)
		If Me.shouldBackDash() Then
			MyBase.animator.SetTrigger("BackDash")
			Yield MyBase.animator.WaitForNormalizedTime(Me, 1F, "Taunt.Exit", 0, False, False, True)
			Yield MyBase.StartCoroutine(Me.backDash_cr())
		ElseIf Me.shouldTurn() Then
			MyBase.animator.SetTrigger("Turn")
			Yield MyBase.animator.WaitForNormalizedTime(Me, 1F, "Taunt.Exit", 0, False, False, True)
			Yield MyBase.animator.WaitForNormalizedTime(Me, 1F, "Turn", 0, False, False, True)
			Me.turn()
		Else
			Yield MyBase.animator.WaitForNormalizedTime(Me, 1F, "Taunt.Exit", 0, False, False, True)
		End If
		Me.tauntAttackCounter -= 1
		Me.EndAttack()
		Return
	End Function

	' Token: 0x06001897 RID: 6295 RVA: 0x000DE4AF File Offset: 0x000DC8AF
	Public Sub [Short]()
		Me.state = ChessKnightLevelKnight.State.[Short]
		MyBase.StartCoroutine(Me.short_cr())
	End Sub

	' Token: 0x06001898 RID: 6296 RVA: 0x000DE4C8 File Offset: 0x000DC8C8
	Private Iterator Function short_cr() As IEnumerator
		Me.tauntAttackCounter = Me.numberTauntString.PopInt()
		Dim p As LevelProperties.ChessKnight.ShortAttack = MyBase.properties.CurrentState.shortAttack
		MyBase.animator.SetTrigger("RegularAttack")
		MyBase.animator.SetBool("Walking", False)
		Yield CupheadTime.WaitForSeconds(Me, p.shortAntiDuration)
		MyBase.animator.SetTrigger("OnAttack")
		Yield CupheadTime.WaitForSeconds(Me, p.shortAttackDuration)
		MyBase.animator.SetTrigger("OnAttackEnd")
		Yield MyBase.animator.WaitForAnimationToStart(Me, "RegularAttack.RecoveryHold", False)
		Yield CupheadTime.WaitForSeconds(Me, p.shortRecoveryDuration)
		If Me.shouldBackDash() Then
			MyBase.animator.SetTrigger("BackDash")
			Yield MyBase.StartCoroutine(Me.backDash_cr())
		Else
			MyBase.animator.SetTrigger("ExitRecovery")
			Yield MyBase.animator.WaitForNormalizedTime(Me, 1F, "RegularAttack.RecoveryExit", 0, False, False, True)
		End If
		Me.EndAttack()
		Return
	End Function

	' Token: 0x06001899 RID: 6297 RVA: 0x000DE4E3 File Offset: 0x000DC8E3
	Public Sub [Long]()
		Me.isTauntAttack = False
		Me.state = ChessKnightLevelKnight.State.[Long]
		MyBase.StartCoroutine(Me.long_cr())
	End Sub

	' Token: 0x0600189A RID: 6298 RVA: 0x000DE500 File Offset: 0x000DC900
	Private Iterator Function long_cr() As IEnumerator
		Me.tauntAttackCounter = Me.numberTauntString.PopInt()
		Dim antiDuration As Single = If((Not Me.isTauntAttack), MyBase.properties.CurrentState.longAttack.longAntiDuration, MyBase.properties.CurrentState.tauntAttack.tauntAttackAntiDuration)
		Dim attackTime As Single = If((Not Me.isTauntAttack), MyBase.properties.CurrentState.longAttack.longAttackTime, MyBase.properties.CurrentState.tauntAttack.tauntAttackTime)
		Dim attackDist As Single = If((Not Me.isTauntAttack), MyBase.properties.CurrentState.longAttack.longAttackDist, MyBase.properties.CurrentState.tauntAttack.tauntAttackDist)
		Dim attackRecovery As Single = If((Not Me.isTauntAttack), MyBase.properties.CurrentState.longAttack.longRecoveryDuration, MyBase.properties.CurrentState.tauntAttack.tauntAttackRecoveryDuration)
		Dim player As AbstractPlayerController = PlayerManager.GetPlayer(PlayerId.PlayerOne)
		Dim player2 As AbstractPlayerController = PlayerManager.GetPlayer(PlayerId.PlayerTwo)
		MyBase.animator.SetBool("Walking", False)
		If Me.isTauntAttack Then
			MyBase.animator.Play("DashAttack.Anticipation")
			MyBase.animator.SetBool("Taunting", False)
		Else
			MyBase.animator.SetTrigger("DashAttack")
		End If
		If antiDuration > 0.7F Then
			Yield CupheadTime.WaitForSeconds(Me, antiDuration)
			MyBase.animator.SetTrigger("OnAttack")
		Else
			If Not Me.isTauntAttack Then
				Yield MyBase.animator.WaitForAnimationToStart(Me, "DashAttack.Anticipation", False)
			End If
			Yield CupheadTime.WaitForSeconds(Me, Mathf.Max(antiDuration, 0.20833333F))
			MyBase.animator.Play("DashAttack.Attack")
			MyBase.animator.Update(0F)
		End If
		Dim t As Single = 0F
		Dim time As Single = attackTime
		Dim startPosition As Single = MyBase.transform.position.x
		Dim endPosition As Single = If((Not Me.facingLeft), (MyBase.transform.position.x + attackDist), (MyBase.transform.position.x - attackDist))
		endPosition = Me.clampEndPosition(endPosition, Not Me.facingLeft)
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		While t < time
			t += CupheadTime.FixedDelta
			MyBase.transform.SetPosition(New Single?(Mathf.Lerp(startPosition, endPosition, t / time)), Nothing, Nothing)
			Yield wait
		End While
		MyBase.animator.SetTrigger("OnAttackEnd")
		Me.recovery(attackRecovery)
		Me.SFX_KOG_KNIGHT_RecoverFoley()
		Return
	End Function

	' Token: 0x0600189B RID: 6299 RVA: 0x000DE51B File Offset: 0x000DC91B
	Public Sub Up()
		Me.state = ChessKnightLevelKnight.State.Up
		MyBase.StartCoroutine(Me.up_cr())
	End Sub

	' Token: 0x0600189C RID: 6300 RVA: 0x000DE534 File Offset: 0x000DC934
	Private Iterator Function up_cr() As IEnumerator
		Me.tauntAttackCounter = Me.numberTauntString.PopInt()
		Dim p As LevelProperties.ChessKnight.UpAttack = MyBase.properties.CurrentState.upAttack
		MyBase.animator.SetTrigger("MoonAttack")
		MyBase.animator.SetBool("Walking", False)
		Yield CupheadTime.WaitForSeconds(Me, p.upAntiDuration)
		MyBase.animator.SetTrigger("OnAttack")
		Yield MyBase.animator.WaitForAnimationToStart(Me, "Recovery", False)
		Me.recovery(p.upRecoveryDuration)
		Me.SFX_KOG_KNIGHT_RecoverFoley()
		Return
	End Function

	' Token: 0x0600189D RID: 6301 RVA: 0x000DE54F File Offset: 0x000DC94F
	Private Sub recovery(duration As Single)
		MyBase.StartCoroutine(Me.recovery_cr(duration))
	End Sub

	' Token: 0x0600189E RID: 6302 RVA: 0x000DE560 File Offset: 0x000DC960
	Private Iterator Function recovery_cr(duration As Single) As IEnumerator
		Me.SFX_KOG_KNIGHT_MoonSlash_Panting()
		Yield CupheadTime.WaitForSeconds(Me, duration)
		If Me.shouldBackDash() Then
			MyBase.animator.SetTrigger("BackDash")
			Yield MyBase.StartCoroutine(Me.backDash_cr())
		ElseIf Me.shouldTurn() Then
			MyBase.animator.SetTrigger("Turn")
			Yield MyBase.animator.WaitForNormalizedTime(Me, 1F, "Turn", 0, False, False, True)
			Me.turn()
		Else
			MyBase.animator.SetTrigger("ExitRecovery")
			Yield MyBase.animator.WaitForNormalizedTime(Me, 1F, "RecoveryExit", 0, False, False, True)
		End If
		Me.SFX_KOG_KNIGHT_MoonSlash_PantingStop()
		Me.EndAttack()
		Return
	End Function

	' Token: 0x0600189F RID: 6303 RVA: 0x000DE584 File Offset: 0x000DC984
	Private Function shouldFaceLeft() As Boolean
		If Me.targetPlayer Is Nothing OrElse Me.targetPlayer.IsDead Then
			Me.targetPlayer = PlayerManager.GetNext()
		End If
		Return Me.targetPlayer.transform.position.x < MyBase.transform.position.x
	End Function

	' Token: 0x060018A0 RID: 6304 RVA: 0x000DE5EA File Offset: 0x000DC9EA
	Private Function shouldTurn() As Boolean
		Return Me.shouldFaceLeft() <> Me.facingLeft
	End Function

	' Token: 0x060018A1 RID: 6305 RVA: 0x000DE600 File Offset: 0x000DCA00
	Private Sub turn()
		Me.facingLeft = Not Me.facingLeft
		MyBase.transform.SetScale(New Single?(CSng(If((Not Me.facingLeft), (-1), 1))), Nothing, Nothing)
	End Sub

	' Token: 0x060018A2 RID: 6306 RVA: 0x000DE654 File Offset: 0x000DCA54
	Private Function shouldBackDash() As Boolean
		Dim flag As Boolean = Me.shouldFaceLeft()
		Dim num As Single = CSng(If((Not flag), 640, (-640)))
		Dim num2 As Single = 640F
		Dim num3 As Single = Mathf.Abs(num - MyBase.transform.position.x)
		Dim flag2 As Boolean = False
		If PlayerManager.BothPlayersActive() Then
			Dim player As AbstractPlayerController = PlayerManager.GetPlayer(PlayerId.PlayerOne)
			Dim player2 As AbstractPlayerController = PlayerManager.GetPlayer(PlayerId.PlayerTwo)
			If Mathf.Sign(MyBase.transform.position.x - player.transform.position.x) <> Mathf.Sign(MyBase.transform.position.x - player2.transform.position.x) Then
				flag2 = True
			End If
		End If
		Return num3 < num2 AndAlso Not flag2
	End Function

	' Token: 0x060018A3 RID: 6307 RVA: 0x000DE734 File Offset: 0x000DCB34
	Private Iterator Function backDash_cr() As IEnumerator
		Dim returnSpeed As Single = If((Not Me.isTauntAttack), MyBase.properties.CurrentState.longAttack.longReturnSpeed, MyBase.properties.CurrentState.tauntAttack.tauntAttackReturnSpeed)
		Me.facingLeft = Me.shouldFaceLeft()
		MyBase.transform.SetScale(New Single?(CSng(If((Not Me.facingLeft), (-1), 1))), Nothing, Nothing)
		Dim startPosition As Single = MyBase.transform.position.x
		Dim endPosition As Single = If((Not Me.facingLeft), (-640F + Me.positionBoundaryInset.minimum), (640F - Me.positionBoundaryInset.minimum))
		Dim time As Single = Mathf.Abs(endPosition - MyBase.transform.position.x) / returnSpeed
		Dim t As Single = 0F
		MyBase.StartCoroutine(Me.backDashAnimation_cr())
		Dim smoke As Effect = Me.backDashSmoke.Spawn(Me.smokeSpawnPoint.position)
		smoke.transform.SetScale(New Single?(CSng(If((Not Me.facingLeft), (-1), 1))), Nothing, Nothing)
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		While t < time
			Dim previousPosition As Vector3 = MyBase.transform.position
			t += CupheadTime.FixedDelta
			If MyBase.properties.CurrentState.movement.hasEasing Then
				Dim num As Single = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0F, 1F, t / time)
				MyBase.transform.SetPosition(New Single?(Mathf.Lerp(startPosition, endPosition, num)), Nothing, Nothing)
			ElseIf Me.goingLeft AndAlso MyBase.transform.position.x > endPosition Then
				MyBase.transform.position += Vector3.left * returnSpeed * CupheadTime.FixedDelta
			ElseIf Not Me.goingLeft AndAlso MyBase.transform.position.x < endPosition Then
				MyBase.transform.position += Vector3.right * returnSpeed * CupheadTime.FixedDelta
			End If
			Me.updateAnimatorSpeed(MyBase.transform.position, previousPosition)
			Yield wait
		End While
		Return
	End Function

	' Token: 0x060018A4 RID: 6308 RVA: 0x000DE750 File Offset: 0x000DCB50
	Private Iterator Function backDashAnimation_cr() As IEnumerator
		Yield MyBase.animator.WaitForNormalizedTime(Me, 1F, "BackDash.Dash", 0, False, False, True)
		Me.SFX_KOG_KNIGHT_RecoverFoley()
		MyBase.animator.SetBool("FacingLeft", Me.facingLeft)
		MyBase.animator.SetBool("Walking", True)
		Return
	End Function

	' Token: 0x060018A5 RID: 6309 RVA: 0x000DE76C File Offset: 0x000DCB6C
	Private Sub death()
		Me.StopAllCoroutines()
		Me.SFX_KOG_KNIGHT_Die()
		MyBase.animator.SetBool("Walking", False)
		MyBase.animator.SetTrigger("Death")
		For i As Integer = 0 To Me.deathArmor.Length - 1
			Dim spriteDeathPartsDLC As SpriteDeathPartsDLC = Global.UnityEngine.[Object].Instantiate(Of SpriteDeathPartsDLC)(Me.deathArmor(i), Me.deathArmorSpawns(i).position, Quaternion.identity)
			spriteDeathPartsDLC.transform.localScale = New Vector3(-MyBase.transform.localScale.x, 1F)
			spriteDeathPartsDLC.transform.parent = MyBase.transform
			spriteDeathPartsDLC.SetVelocity(New Vector3(spriteDeathPartsDLC.transform.localPosition.x * 3F * MyBase.transform.localScale.x, spriteDeathPartsDLC.transform.localPosition.y * 6F + 800F))
		Next
	End Sub

	' Token: 0x060018A6 RID: 6310 RVA: 0x000DE874 File Offset: 0x000DCC74
	Private Sub updateAnimatorSpeed(currentPosition As Vector3, previousPosition As Vector3)
		If CupheadTime.IsPaused() Then
			Return
		End If
		Dim vector As Vector3 = (currentPosition - previousPosition) / CupheadTime.FixedDelta
		MyBase.animator.SetFloat("XSpeed", vector.x)
		Dim num As Single = MathUtilities.LerpMapping(Mathf.Abs(vector.x), 0F, Me.maximumLegVelocity, Me.legSpeedMultiplierRange.minimum, Me.legSpeedMultiplierRange.maximum, True)
		MyBase.animator.SetFloat("LegSpeed", num)
	End Sub

	' Token: 0x060018A7 RID: 6311 RVA: 0x000DE8FC File Offset: 0x000DCCFC
	Private Function clampEndPosition(endPosition As Single, isRightSide As Boolean) As Single
		If isRightSide Then
			Dim num As Single = 640F - Me.positionBoundaryInset.maximum
			Dim num2 As Single = 640F - Me.positionBoundaryInset.minimum
			endPosition = Mathf.Clamp(endPosition, num, num2)
		Else
			Dim num3 As Single = -640F + Me.positionBoundaryInset.minimum
			Dim num4 As Single = -640F + Me.positionBoundaryInset.maximum
			endPosition = Mathf.Clamp(endPosition, num3, num4)
		End If
		Return endPosition
	End Function

	' Token: 0x060018A8 RID: 6312 RVA: 0x000DE971 File Offset: 0x000DCD71
	Private Sub AnimationEvent_SFX_KOG_KNIGHT_AttackUpwards_Stab()
		AudioManager.Play("sfx_dlc_kog_knight_attackupwards_stab")
		Me.emitAudioFromObject.Add("sfx_dlc_kog_knight_attackupwards_stab")
	End Sub

	' Token: 0x060018A9 RID: 6313 RVA: 0x000DE98D File Offset: 0x000DCD8D
	Private Sub AnimationEvent_SFX_KOG_KNIGHT_AttackUpwards_Start()
		AudioManager.Play("sfx_dlc_kog_knight_attackupwards_start")
		Me.emitAudioFromObject.Add("sfx_dlc_kog_knight_attackupwards_start")
	End Sub

	' Token: 0x060018AA RID: 6314 RVA: 0x000DE9A9 File Offset: 0x000DCDA9
	Private Sub SFX_KOG_KNIGHT_Die()
		AudioManager.Play("sfx_dlc_kog_knight_die")
		Me.emitAudioFromObject.Add("sfx_dlc_kog_knight_die")
	End Sub

	' Token: 0x060018AB RID: 6315 RVA: 0x000DE9C5 File Offset: 0x000DCDC5
	Private Sub AnimationEvent_SFX_KOG_KNIGHT_Foley_Walk()
		AudioManager.Play("sfx_dlc_kog_knight_foley_walk")
		Me.emitAudioFromObject.Add("sfx_dlc_kog_knight_foley_walk")
	End Sub

	' Token: 0x060018AC RID: 6316 RVA: 0x000DE9E1 File Offset: 0x000DCDE1
	Private Sub AnimationEvent_SFX_KOG_KNIGHT_Intro_ShieldBash()
		AudioManager.Play("sfx_dlc_kog_knight_intro_shieldbash")
		Me.emitAudioFromObject.Add("sfx_dlc_kog_knight_intro_shieldbash")
	End Sub

	' Token: 0x060018AD RID: 6317 RVA: 0x000DE9FD File Offset: 0x000DCDFD
	Private Sub AnimationEvent_SFX_KOG_KNIGHT_Intro_Visor()
		AudioManager.Play("sfx_dlc_kog_knight_intro_visor")
		Me.emitAudioFromObject.Add("sfx_dlc_kog_knight_intro_visor")
	End Sub

	' Token: 0x060018AE RID: 6318 RVA: 0x000DEA19 File Offset: 0x000DCE19
	Private Sub AnimationEvent_SFX_KOG_KNIGHT_MoonSlash_End()
		AudioManager.[Stop]("sfx_dlc_kog_knight_moonslash_panting")
		AudioManager.Play("sfx_dlc_kog_knight_moonslash_end")
		Me.emitAudioFromObject.Add("sfx_dlc_kog_knight_moonslash_end")
	End Sub

	' Token: 0x060018AF RID: 6319 RVA: 0x000DEA3F File Offset: 0x000DCE3F
	Private Sub SFX_KOG_KNIGHT_MoonSlash_Panting()
		AudioManager.Play("sfx_dlc_kog_knight_moonslash_panting")
		Me.emitAudioFromObject.Add("sfx_dlc_kog_knight_moonslash_panting")
	End Sub

	' Token: 0x060018B0 RID: 6320 RVA: 0x000DEA5B File Offset: 0x000DCE5B
	Private Sub SFX_KOG_KNIGHT_MoonSlash_PantingStop()
		AudioManager.[Stop]("sfx_dlc_kog_knight_moonslash_panting")
	End Sub

	' Token: 0x060018B1 RID: 6321 RVA: 0x000DEA67 File Offset: 0x000DCE67
	Private Sub AnimationEvent_SFX_KOG_KNIGHT_MoonSlash_Start()
		AudioManager.Play("sfx_dlc_kog_knight_moonslash_start")
		Me.emitAudioFromObject.Add("sfx_dlc_kog_knight_moonslash_start")
	End Sub

	' Token: 0x060018B2 RID: 6322 RVA: 0x000DEA83 File Offset: 0x000DCE83
	Private Sub AnimationEvent_SFX_KOG_KNIGHT_MoonSlash_Swing()
		AudioManager.Play("sfx_dlc_kog_knight_moonslash_swing")
		Me.emitAudioFromObject.Add("sfx_dlc_kog_knight_moonslash_swing")
	End Sub

	' Token: 0x060018B3 RID: 6323 RVA: 0x000DEA9F File Offset: 0x000DCE9F
	Private Sub AnimationEvent_SFX_KOG_KNIGHT_TauntHand()
		AudioManager.Play("sfx_dlc_kog_knight_taunthand")
		Me.emitAudioFromObject.Add("sfx_dlc_kog_knight_taunthand")
	End Sub

	' Token: 0x060018B4 RID: 6324 RVA: 0x000DEABB File Offset: 0x000DCEBB
	Private Sub AnimationEvent_SFX_KOG_KNIGHT_Dash_End()
		AudioManager.Play("sfx_dlc_kog_knight_dash_end")
		Me.emitAudioFromObject.Add("sfx_dlc_kog_knight_dash_end")
	End Sub

	' Token: 0x060018B5 RID: 6325 RVA: 0x000DEAD7 File Offset: 0x000DCED7
	Private Sub SFX_KOG_KNIGHT_RecoverFoley()
		AudioManager.Play("sfx_dlc_kog_knight_recoverfoley")
		Me.emitAudioFromObject.Add("sfx_dlc_kog_knight_recoverfoley")
	End Sub

	' Token: 0x060018B6 RID: 6326 RVA: 0x000DEAF3 File Offset: 0x000DCEF3
	Private Sub AnimationEvent_SFX_KOG_KNIGHT_Dash_Start()
		AudioManager.Play("sfx_dlc_kog_knight_dash_start")
		Me.emitAudioFromObject.Add("sfx_dlc_kog_knight_dash_start")
	End Sub

	' Token: 0x060018B7 RID: 6327 RVA: 0x000DEB0F File Offset: 0x000DCF0F
	Private Sub AnimationEvent_SFX_KOG_KNIGHT_Dash_Attack()
		AudioManager.Play("sfx_dlc_kog_knight_dash_attack")
		Me.emitAudioFromObject.Add("sfx_dlc_kog_knight_dash_attack")
	End Sub

	' Token: 0x060018B8 RID: 6328 RVA: 0x000DEB2B File Offset: 0x000DCF2B
	Private Sub AnimationEvent_SFX_KOG_KNIGHT_Vocal_Attack()
		AudioManager.Play("sfx_dlc_kog_knight_vocal_attack")
		Me.emitAudioFromObject.Add("sfx_dlc_kog_knight_vocal_attack")
	End Sub

	' Token: 0x040021A6 RID: 8614
	<SerializeField()>
	Private pink As ParrySwitch

	' Token: 0x040021A7 RID: 8615
	<SerializeField()>
	Private swordHitbox As CollisionChild

	' Token: 0x040021A8 RID: 8616
	<SerializeField()>
	Private upHitbox As CollisionChild

	' Token: 0x040021A9 RID: 8617
	<SerializeField()>
	Private positionBoundaryInset As Rangef

	' Token: 0x040021AA RID: 8618
	<SerializeField()>
	Private smokeSpawnPoint As Transform

	' Token: 0x040021AB RID: 8619
	<SerializeField()>
	Private backDashSmoke As Effect

	' Token: 0x040021AC RID: 8620
	<SerializeField()>
	Private legSpeedMultiplierRange As Rangef

	' Token: 0x040021AD RID: 8621
	<SerializeField()>
	Private maximumLegVelocity As Single

	' Token: 0x040021AE RID: 8622
	<SerializeField()>
	Private deathArmor As SpriteDeathPartsDLC()

	' Token: 0x040021AF RID: 8623
	<SerializeField()>
	Private deathArmorSpawns As Transform()

	' Token: 0x040021B0 RID: 8624
	<SerializeField()>
	Private hitFlash As HitFlash

	' Token: 0x040021B1 RID: 8625
	Private damageDealer As DamageDealer

	' Token: 0x040021B2 RID: 8626
	Private battleStartPosition As Single

	' Token: 0x040021B3 RID: 8627
	Private goingLeft As Boolean = True

	' Token: 0x040021B4 RID: 8628
	Private facingLeft As Boolean = True

	' Token: 0x040021B5 RID: 8629
	Private targetPlayer As AbstractPlayerController

	' Token: 0x040021B6 RID: 8630
	Private attackIntervalPattern As PatternString

	' Token: 0x040021B7 RID: 8631
	Private movementPattern As PatternString

	' Token: 0x040021B8 RID: 8632
	Private numberTauntString As PatternString

	' Token: 0x040021B9 RID: 8633
	Public tauntAttackCounter As Integer

	' Token: 0x040021BA RID: 8634
	Private isTauntAttack As Boolean

	' Token: 0x02000540 RID: 1344
	Public Enum State
		' Token: 0x040021BD RID: 8637
		Intro
		' Token: 0x040021BE RID: 8638
		Move
		' Token: 0x040021BF RID: 8639
		Idle
		' Token: 0x040021C0 RID: 8640
		[Short]
		' Token: 0x040021C1 RID: 8641
		[Long]
		' Token: 0x040021C2 RID: 8642
		Up
		' Token: 0x040021C3 RID: 8643
		Taunt
		' Token: 0x040021C4 RID: 8644
		Death
	End Enum
End Class
