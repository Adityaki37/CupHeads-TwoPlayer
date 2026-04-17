Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x020005BF RID: 1471
Public Class DicePalaceEightBallLevelEightBall
	Inherits LevelProperties.DicePalaceEightBall.Entity

	' Token: 0x06001C97 RID: 7319 RVA: 0x001051FC File Offset: 0x001035FC
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		Dim list As List(Of Integer) = New List(Of Integer)(Me.balls.Count)
		Me.newList = New List(Of Integer)()
		For i As Integer = 0 To Me.balls.Count - 1
			list.Add(i)
		Next
		For j As Integer = 0 To Me.balls.Count - 1
			Dim num As Integer = Global.UnityEngine.Random.Range(0, list.Count)
			Me.newList.Add(list(num))
			list.RemoveAt(num)
		Next
		Me.ballIndex = 0
	End Sub

	' Token: 0x06001C98 RID: 7320 RVA: 0x001052BA File Offset: 0x001036BA
	Public Overrides Sub LevelInit(properties As LevelProperties.DicePalaceEightBall)
		MyBase.LevelInit(properties)
		AddHandler Level.Current.OnWinEvent, AddressOf Me.OnDeath
		MyBase.StartCoroutine(Me.intro_cr())
	End Sub

	' Token: 0x06001C99 RID: 7321 RVA: 0x001052E6 File Offset: 0x001036E6
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		MyBase.properties.DealDamage(info.damage)
	End Sub

	' Token: 0x06001C9A RID: 7322 RVA: 0x001052FC File Offset: 0x001036FC
	Private Iterator Function intro_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1.5F)
		MyBase.animator.SetTrigger("Continue")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Intro", False, True)
		AudioManager.Play("dice_palace_eight_ball_intro")
		Me.emitAudioFromObject.Add("dice_palace_eight_ball_intro")
		Yield MyBase.animator.WaitForAnimationToStart(Me, "Right_Idle", False)
		MyBase.StartCoroutine(Me.shoot_bullet_cr())
		MyBase.StartCoroutine(Me.spawn_balls_cr())
		Yield Nothing
		Return
	End Function

	' Token: 0x06001C9B RID: 7323 RVA: 0x00105318 File Offset: 0x00103718
	Private Sub LoopCounter()
		If Me.currentLoops < MyBase.properties.CurrentState.general.idleLoopAmount Then
			Me.currentLoops += 1
		Else
			MyBase.animator.SetTrigger("Continue")
			Me.currentLoops = 0
		End If
	End Sub

	' Token: 0x06001C9C RID: 7324 RVA: 0x0010536F File Offset: 0x0010376F
	Private Sub HitLeftIdle()
		MyBase.animator.SetBool("MovingLeft", False)
	End Sub

	' Token: 0x06001C9D RID: 7325 RVA: 0x00105382 File Offset: 0x00103782
	Private Sub HitRightIdle()
		MyBase.animator.SetBool("MovingLeft", True)
	End Sub

	' Token: 0x06001C9E RID: 7326 RVA: 0x00105395 File Offset: 0x00103795
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.attackEffect = Nothing
		Me.projectileEffect = Nothing
		Me.projectile = Nothing
		Me.pinkProjectile = Nothing
		Me.balls = Nothing
	End Sub

	' Token: 0x06001C9F RID: 7327 RVA: 0x001053C0 File Offset: 0x001037C0
	Private Iterator Function shoot_bullet_cr() As IEnumerator
		Dim p As LevelProperties.DicePalaceEightBall.General = MyBase.properties.CurrentState.general
		Dim projectileType As String() = p.shootString.GetRandom().Split(New Char() { ","c })
		Dim projectileIndex As Integer = Global.UnityEngine.Random.Range(0, projectileType.Length)
		While True
			Yield CupheadTime.WaitForSeconds(Me, p.shootDelay)
			MyBase.animator.SetTrigger("OnAttack")
			Yield MyBase.animator.WaitForAnimationToStart(Me, "Attack_Start", False)
			AudioManager.Play("dice_palace_eight_ball_attack_start")
			Me.emitAudioFromObject.Add("dice_palace_eight_ball_attack_start")
			Yield MyBase.animator.WaitForAnimationToEnd(Me, "Attack_Start", False, True)
			Dim effect As Effect = Global.UnityEngine.[Object].Instantiate(Of Effect)(Me.projectileEffect)
			effect.transform.position = Me.root.transform.position
			Yield effect.GetComponent(Of Animator)().WaitForAnimationToEnd(Me, "Projectile", False, True)
			Dim player As AbstractPlayerController = PlayerManager.GetNext()
			Dim dir As Vector3 = player.transform.position - MyBase.transform.position
			AudioManager.Play("dice_palace_eight_ball_eight_attack_fire")
			Me.emitAudioFromObject.Add("dice_palace_eight_ball_eight_attack_fire")
			If projectileType(projectileIndex)(0) = "R"c Then
				Me.attackEffect.Create(Me.root.transform.position)
				Me.projectile.Create(Me.root.transform.position, MathUtils.DirectionToAngle(dir), MyBase.properties.CurrentState.general.shootSpeed)
			ElseIf projectileType(projectileIndex)(0) = "P"c Then
				Me.attackEffect.Create(Me.root.transform.position)
				Me.pinkProjectile.Create(Me.root.transform.position, MathUtils.DirectionToAngle(dir), MyBase.properties.CurrentState.general.shootSpeed)
			End If
			projectileIndex = (projectileIndex + 1) Mod projectileType.Length
			Yield CupheadTime.WaitForSeconds(Me, p.attackDuration)
			MyBase.animator.SetTrigger("OnEnd")
			AudioManager.Play("dice_palace_eight_ball_attack_end")
			Me.emitAudioFromObject.Add("dice_palace_eight_ball_attack_end")
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06001CA0 RID: 7328 RVA: 0x001053DB File Offset: 0x001037DB
	Private Sub IntroSFX()
		AudioManager.Play("dice_palace_eight_ball_eight_intro")
		Me.emitAudioFromObject.Add("dice_palace_eight_ball_eight_intro")
	End Sub

	' Token: 0x06001CA1 RID: 7329 RVA: 0x001053F8 File Offset: 0x001037F8
	Private Iterator Function spawn_balls_cr() As IEnumerator
		Dim p As LevelProperties.DicePalaceEightBall.PoolBalls = MyBase.properties.CurrentState.poolBalls
		Dim side As String() = p.sideString.GetRandom().Split(New Char() { ","c })
		Dim offset As Single = MyBase.GetComponent(Of Renderer)().bounds.size.x / 2F
		Dim sideIndex As Integer = Global.UnityEngine.Random.Range(0, side.Length)
		Dim onLeft As Boolean = False
		Dim leftPos As Vector3 = New Vector3(-640F, 360F + offset, 0F)
		Dim rightPos As Vector3 = New Vector3(640F, 360F + offset, 0F)
		Dim pos As Vector3 = Vector3.zero
		While True
			Yield CupheadTime.WaitForSeconds(Me, p.spawnDelay)
			Dim ballInstance As DicePalaceEightBallLevelPoolBall = Nothing
			If side(sideIndex)(0) = "L"c Then
				onLeft = True
				pos = leftPos
			ElseIf side(sideIndex)(0) = "R"c Then
				onLeft = False
				pos = rightPos
			Else
				Global.Debug.LogError("sideString pattern is wrong", Nothing)
			End If
			Dim index As Integer = Me.newList(Me.ballIndex)
			While index < 0 OrElse index > Me.balls.Count
				Me.ballIndex = (Me.ballIndex + 1) Mod Me.balls.Count
				index = Me.newList(Me.ballIndex)
				Yield Nothing
			End While
			If index = 0 Then
				ballInstance = Me.balls(index).Create(pos, p.oneJumpHorizontalSpeed, p.oneJumpVerticalSpeed, p.oneJumpGravity, p.oneGroundDelay, onLeft, Me)
			ElseIf index = 1 Then
				ballInstance = Me.balls(index).Create(pos, p.twoJumpHorizontalSpeed, p.twoJumpVerticalSpeed, p.twoJumpGravity, p.twoGroundDelay, onLeft, Me)
			ElseIf index = 2 Then
				ballInstance = Me.balls(index).Create(pos, p.threeJumpHorizontalSpeed, p.threeJumpVerticalSpeed, p.threeJumpGravity, p.threeGroundDelay, onLeft, Me)
			ElseIf index = 3 Then
				ballInstance = Me.balls(index).Create(pos, p.fourJumpHorizontalSpeed, p.fourJumpVerticalSpeed, p.fourJumpGravity, p.fourGroundDelay, onLeft, Me)
			ElseIf index = 4 Then
				ballInstance = Me.balls(index).Create(pos, p.fiveJumpHorizontalSpeed, p.fiveJumpVerticalSpeed, p.fiveJumpGravity, p.fiveGroundDelay, onLeft, Me)
			Else
				Global.Debug.LogError("Invalid index", Nothing)
			End If
			If ballInstance IsNot Nothing Then
				ballInstance.SetVariation(Me.newList(Me.ballIndex))
			End If
			Me.ballIndex = (Me.ballIndex + 1) Mod Me.balls.Count
			sideIndex = (sideIndex + 1) Mod side.Length
		End While
		Return
	End Function

	' Token: 0x06001CA2 RID: 7330 RVA: 0x00105414 File Offset: 0x00103814
	Private Sub OnDeath()
		If Me.OnEightBallDeath IsNot Nothing Then
			Me.OnEightBallDeath()
		End If
		Me.StopAllCoroutines()
		AudioManager.PlayLoop("dice_palace_eight_ball_attack_death_loop")
		Me.emitAudioFromObject.Add("dice_palace_eight_ball_attack_death_loop")
		MyBase.animator.SetTrigger("OnDeath")
		MyBase.GetComponent(Of Collider2D)().enabled = False
	End Sub

	' Token: 0x04002582 RID: 9602
	<SerializeField()>
	Private attackEffect As Effect

	' Token: 0x04002583 RID: 9603
	<SerializeField()>
	Private projectileEffect As Effect

	' Token: 0x04002584 RID: 9604
	<SerializeField()>
	Private root As Transform

	' Token: 0x04002585 RID: 9605
	<SerializeField()>
	Private projectile As BasicProjectile

	' Token: 0x04002586 RID: 9606
	<SerializeField()>
	Private pinkProjectile As BasicProjectile

	' Token: 0x04002587 RID: 9607
	<SerializeField()>
	Private balls As List(Of DicePalaceEightBallLevelPoolBall)

	' Token: 0x04002588 RID: 9608
	Private newList As List(Of Integer)

	' Token: 0x04002589 RID: 9609
	Private damageReceiver As DamageReceiver

	' Token: 0x0400258A RID: 9610
	Private currentLoops As Integer

	' Token: 0x0400258B RID: 9611
	Private ballIndex As Integer

	' Token: 0x0400258C RID: 9612
	Public OnEightBallDeath As Action
End Class
