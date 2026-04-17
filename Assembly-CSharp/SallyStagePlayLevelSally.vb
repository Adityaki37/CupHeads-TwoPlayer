Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020007B5 RID: 1973
Public Class SallyStagePlayLevelSally
	Inherits LevelProperties.SallyStagePlay.Entity

	' Token: 0x1700040A RID: 1034
	' (get) Token: 0x06002C6F RID: 11375 RVA: 0x001A1CCE File Offset: 0x001A00CE
	' (set) Token: 0x06002C70 RID: 11376 RVA: 0x001A1CD6 File Offset: 0x001A00D6
	Public Property state As SallyStagePlayLevelSally.State

	' Token: 0x06002C71 RID: 11377 RVA: 0x001A1CE0 File Offset: 0x001A00E0
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		If Me.isInvincible Then
			Return
		End If
		MyBase.properties.DealDamage(info.damage)
		If Not SallyStagePlayLevelBackgroundHandler.HUSBAND_GONE AndAlso Me.husband.gameObject.activeInHierarchy Then
			If Not AudioManager.CheckIfPlaying("sally_bg_church_fiance_ohno") Then
				AudioManager.Play("sally_bg_church_fiance_ohno")
				Me.emitAudioFromObject.Add("sally_bg_church_fiance_ohno")
			End If
			Me.husband.GetComponent(Of Animator)().Play("OhNo")
		End If
	End Sub

	' Token: 0x06002C72 RID: 11378 RVA: 0x001A1D68 File Offset: 0x001A0168
	Public Overrides Sub LevelInit(properties As LevelProperties.SallyStagePlay)
		MyBase.LevelInit(properties)
		Me.bounds = MyBase.GetComponent(Of BoxCollider2D)().bounds.size
		Me.jumpTypeIndex = Global.UnityEngine.Random.Range(0, properties.CurrentState.jump.JumpAttackString.Split(New Char() { ","c }).Length)
		Me.jumpCountIndex = Global.UnityEngine.Random.Range(0, properties.CurrentState.jump.JumpAttackCountString.Split(New Char() { ","c }).Length)
		Me.jumpRollAttackTypeIndex = Global.UnityEngine.Random.Range(0, properties.CurrentState.jumpRoll.JumpAttackTypeString.Split(New Char() { ","c }).Length)
		Me.heartTypeIndex = Global.UnityEngine.Random.Range(0, properties.CurrentState.kiss.heartType.Split(New Char() { ","c }).Length)
		Me.teleportOffsetIndex = Global.UnityEngine.Random.Range(0, properties.CurrentState.teleport.appearOffsetString.Split(New Char() { ","c }).Length)
		MyBase.transform.position = Me.ground
		MyBase.StartCoroutine(Me.intro_cr())
	End Sub

	' Token: 0x06002C73 RID: 11379 RVA: 0x001A1E9E File Offset: 0x001A029E
	Public Sub GetParent(parent As SallyStagePlayLevel)
		AddHandler parent.OnPhase2, AddressOf Me.OnPhase2
	End Sub

	' Token: 0x06002C74 RID: 11380 RVA: 0x001A1EB4 File Offset: 0x001A02B4
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		AddHandler Me.collisionChild.OnPlayerCollision, AddressOf Me.OnCollisionPlayer
		AddHandler Me.collisionChild.GetComponent(Of DamageReceiver)().OnDamageTaken, AddressOf Me.OnDamageTaken
		Me.ground = New Vector3(MyBase.transform.position.x, CSng(Level.Current.Ground) + 300F, 0F)
	End Sub

	' Token: 0x06002C75 RID: 11381 RVA: 0x001A1F60 File Offset: 0x001A0360
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
		Me.ground.x = MyBase.transform.position.x
	End Sub

	' Token: 0x06002C76 RID: 11382 RVA: 0x001A1FA1 File Offset: 0x001A03A1
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x06002C77 RID: 11383 RVA: 0x001A1FC0 File Offset: 0x001A03C0
	Private Iterator Function intro_cr() As IEnumerator
		Me.state = SallyStagePlayLevelSally.State.Intro
		Yield CupheadTime.WaitForSeconds(Me, 2F)
		MyBase.animator.SetTrigger("Continue")
		AudioManager.Play("sally_sally_intro_phase1")
		Me.emitAudioFromObject.Add("sally_sally_intro_phase1")
		Me.state = SallyStagePlayLevelSally.State.Idle
		Yield Nothing
		Return
	End Function

	' Token: 0x06002C78 RID: 11384 RVA: 0x001A1FDC File Offset: 0x001A03DC
	Public Sub OnJumpAttack()
		Me.state = SallyStagePlayLevelSally.State.Attack
		Me.jumpType = CType(Parser.IntParse(MyBase.properties.CurrentState.jump.JumpAttackString.Split(New Char() { ","c })(Me.jumpTypeIndex)), SallyStagePlayLevelSally.JumpType)
		Me.target = PlayerManager.GetNext()
		Me.currentJumpAttackCount = 0
		MyBase.StartCoroutine(Me.jump_cr())
		Me.jumpCountIndex = (Me.jumpCountIndex + 1) Mod MyBase.properties.CurrentState.jump.JumpAttackCountString.Split(New Char() { ","c }).Length
	End Sub

	' Token: 0x06002C79 RID: 11385 RVA: 0x001A207C File Offset: 0x001A047C
	Private Iterator Function jump_cr() As IEnumerator
		If Me.currentJumpAttackCount >= Parser.IntParse(MyBase.properties.CurrentState.jump.JumpAttackCountString.Split(New Char() { ","c })(Me.jumpCountIndex)) Then
			Dim rand As Single = MyBase.properties.CurrentState.jump.JumpHesitate.RandomFloat()
			If rand > 0F Then
				Yield CupheadTime.WaitForSeconds(Me, rand)
			End If
			Me.state = SallyStagePlayLevelSally.State.Idle
			Yield Nothing
		Else
			Me.jumpTypeIndex = (Me.jumpTypeIndex + 1) Mod MyBase.properties.CurrentState.jump.JumpAttackString.Split(New Char() { ","c }).Length
			Me.jumpType = CType(Parser.IntParse(MyBase.properties.CurrentState.jump.JumpAttackString.Split(New Char() { ","c })(Me.jumpTypeIndex)), SallyStagePlayLevelSally.JumpType)
			If Me.currentJumpAttackCount > 0 AndAlso MyBase.properties.CurrentState.jump.JumpDelay > 0F Then
				Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.jump.JumpDelay)
			End If
			MyBase.animator.SetInteger("JumpType", CInt(Me.jumpType))
			MyBase.animator.SetTrigger("Jump")
			Yield MyBase.animator.WaitForAnimationToEnd(Me, "Jump_TakeOff", True, True)
			Dim start As Vector3 = MyBase.transform.position
			Dim [end] As Vector3 = start
			Dim jumpType As SallyStagePlayLevelSally.JumpType = Me.jumpType
			If jumpType <> SallyStagePlayLevelSally.JumpType.DiveKick Then
				If jumpType = SallyStagePlayLevelSally.JumpType.DoubleJump Then
					[end] += Vector3.up * MyBase.properties.CurrentState.jumpRoll.JumpHeight.RandomFloat()
				End If
			Else
				[end] += Vector3.up * MyBase.properties.CurrentState.diveKick.DiveAttackHeight.RandomFloat()
			End If
			MyBase.StartCoroutine(Me.shadow_cr(True))
			Dim timePassed As Single = 0F
			While timePassed / 0.1665F < 1F
				If CupheadTime.Delta IsNot 0F Then
					MyBase.transform.position = start + ([end] - start) * (timePassed / 0.1665F)
					timePassed += CupheadTime.Delta
				End If
				Yield Nothing
			End While
			MyBase.animator.SetTrigger("OnAttack")
			Me.currentJumpAttackCount += 1
			Me.StartJumpAttack(Me.jumpType)
		End If
		Return
	End Function

	' Token: 0x06002C7A RID: 11386 RVA: 0x001A2097 File Offset: 0x001A0497
	Private Sub StartJumpAttack(type As SallyStagePlayLevelSally.JumpType)
		If type <> SallyStagePlayLevelSally.JumpType.DiveKick Then
			If type = SallyStagePlayLevelSally.JumpType.DoubleJump Then
				MyBase.StartCoroutine(Me.jumpRoll_cr())
			End If
		Else
			MyBase.StartCoroutine(Me.diveKick_cr())
		End If
	End Sub

	' Token: 0x06002C7B RID: 11387 RVA: 0x001A20D8 File Offset: 0x001A04D8
	Private Iterator Function landing_cr(Optional useTrigger As Boolean = True) As IEnumerator
		MyBase.StartCoroutine(Me.shadow_cr(False))
		If Me.target Is Nothing OrElse Me.target.IsDead Then
			Me.target = PlayerManager.GetNext()
		End If
		If Me.target.center.x > MyBase.transform.position.x Then
			If MyBase.transform.right.x > 0F Then
				If useTrigger Then
					Yield New WaitForEndOfFrame()
					MyBase.animator.SetTrigger("OnTurnLanding")
					Yield MyBase.animator.WaitForAnimationToEnd(Me, True)
					MyBase.transform.right *= -1F
					Yield MyBase.animator.WaitForAnimationToEnd(Me, "Land_and_Turn", False, True)
				Else
					Yield New WaitForEndOfFrame()
					MyBase.animator.Play("Land_and_Turn")
					Yield MyBase.animator.WaitForAnimationToEnd(Me, True)
					MyBase.transform.right *= -1F
					Yield MyBase.animator.WaitForAnimationToEnd(Me, "Land_and_Turn", False, True)
				End If
			ElseIf useTrigger Then
				MyBase.animator.SetTrigger("OnLanding")
				Yield MyBase.animator.WaitForAnimationToEnd(Me, "Land", True, True)
			Else
				MyBase.animator.Play("Land")
				Yield MyBase.animator.WaitForAnimationToEnd(Me, "Land", True, True)
			End If
		ElseIf MyBase.transform.right.x < 0F Then
			If useTrigger Then
				Yield New WaitForEndOfFrame()
				MyBase.animator.SetTrigger("OnTurnLanding")
				Yield MyBase.animator.WaitForAnimationToEnd(Me, True)
				MyBase.transform.right *= -1F
				Yield MyBase.animator.WaitForAnimationToEnd(Me, "Land_and_Turn", False, True)
			Else
				Yield New WaitForEndOfFrame()
				MyBase.animator.Play("Land_and_Turn")
				Yield MyBase.animator.WaitForAnimationToEnd(Me, True)
				MyBase.transform.right *= -1F
				Yield MyBase.animator.WaitForAnimationToEnd(Me, "Land_and_Turn", False, True)
			End If
		ElseIf useTrigger Then
			MyBase.animator.SetTrigger("OnLanding")
			Yield MyBase.animator.WaitForAnimationToEnd(Me, "Land", True, True)
		Else
			MyBase.animator.Play("Land")
			Yield MyBase.animator.WaitForAnimationToEnd(Me, "Land", True, True)
		End If
		If Not Me.getOutOfJump Then
			MyBase.StartCoroutine(Me.jump_cr())
		Else
			Me.state = SallyStagePlayLevelSally.State.Idle
		End If
		Return
	End Function

	' Token: 0x06002C7C RID: 11388 RVA: 0x001A20FA File Offset: 0x001A04FA
	Private Sub LandSFX()
		AudioManager.Play("sally_sally_land")
		Me.emitAudioFromObject.Add("sally_sally_land")
	End Sub

	' Token: 0x06002C7D RID: 11389 RVA: 0x001A2118 File Offset: 0x001A0518
	Private Iterator Function shadow_cr(Optional fadeOut As Boolean = True) As IEnumerator
		Dim shadow As GameObject = Global.UnityEngine.[Object].Instantiate(Of GameObject)(Me.shadowPrefab, New Vector3(MyBase.transform.position.x, CSng(Level.Current.Ground), 0F), Quaternion.identity)
		If fadeOut Then
			shadow.GetComponent(Of Animator)().Play("FadeOut")
		Else
			shadow.GetComponent(Of Animator)().Play("FadeIn")
		End If
		Yield shadow.GetComponent(Of Animator)().WaitForAnimationToEnd(Me, True)
		Global.UnityEngine.[Object].Destroy(shadow)
		Return
	End Function

	' Token: 0x06002C7E RID: 11390 RVA: 0x001A213C File Offset: 0x001A053C
	Private Iterator Function diveKick_cr() As IEnumerator
		MyBase.animator.Play("DiveKick_Transition")
		Dim direction As Vector2 = -MyBase.transform.right
		Dim angle As Single = CSng(MyBase.properties.CurrentState.diveKick.DiveAngleRange.RandomInt()) / 100F
		If angle = 0F Then
			angle = 0.001F
		End If
		direction.x = direction.x * Mathf.Cos(angle) - direction.y * Mathf.Sin(angle)
		direction.y = direction.x * Mathf.Sin(angle) + direction.y * Mathf.Cos(angle)
		direction.y = -Mathf.Abs(direction.y)
		Dim attacking As Boolean = True
		AudioManager.Play("sally_divekick_loop")
		Me.emitAudioFromObject.Add("sally_divekick_loop")
		Dim deltaPosition As Vector3 = Vector3.zero
		While attacking
			If CupheadTime.Delta IsNot 0F Then
				deltaPosition = Vector3.zero
			End If
			If Mathf.Sign(direction.x) > 0F Then
				If MyBase.transform.position.x + Me.bounds.x / 2F < CSng(Level.Current.Right) Then
					deltaPosition.x = direction.x * MyBase.properties.CurrentState.diveKick.DiveSpeed * CupheadTime.Delta
				End If
			ElseIf MyBase.transform.position.x - Me.bounds.x / 2F > CSng(Level.Current.Left) Then
				deltaPosition.x = direction.x * MyBase.properties.CurrentState.diveKick.DiveSpeed * CupheadTime.Delta
			End If
			If MyBase.transform.position.y > Me.ground.y Then
				deltaPosition.y = Mathf.Sign(direction.y) * MyBase.properties.CurrentState.diveKick.DiveSpeed * CupheadTime.Delta
			Else
				deltaPosition.y = 0F
			End If
			If deltaPosition.y = 0F Then
				If CupheadTime.Delta IsNot 0F Then
					attacking = False
				End If
			Else
				MyBase.transform.position += deltaPosition
			End If
			Yield Nothing
		End While
		MyBase.StartCoroutine(Me.landing_cr(False))
		Return
	End Function

	' Token: 0x06002C7F RID: 11391 RVA: 0x001A2158 File Offset: 0x001A0558
	Private Iterator Function jumpRoll_cr() As IEnumerator
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "JumpRoll_Transition", True, True)
		If Not Me.getOutOfJump Then
			MyBase.StartCoroutine(Me.rollAttack_cr())
			AudioManager.PlayLoop("sally_double_jump_roll_loop")
			Me.emitAudioFromObject.Add("sally_double_jump_roll_loop")
		End If
		Dim start As Vector3 = MyBase.transform.position
		Dim [end] As Vector3 = start + Vector3.up * MyBase.properties.CurrentState.jumpRoll.RollJumpVerticalMovement
		[end] += -MyBase.transform.right * MyBase.properties.CurrentState.jumpRoll.RollJumpHorizontalMovement.RandomFloat()
		If [end].x - Me.bounds.x / 2F < CSng(Level.Current.Left) Then
			[end].x = CSng(Level.Current.Left) + Me.bounds.x / 2F
		ElseIf [end].x + Me.bounds.x / 2F > CSng(Level.Current.Right) Then
			[end].x = CSng(Level.Current.Right) - Me.bounds.x / 2F
		End If
		Dim pct As Single = 0F
		While pct < MyBase.properties.CurrentState.jumpRoll.JumpRollDuration
			MyBase.transform.position = start + ([end] - start) * pct
			pct += CupheadTime.Delta
			Yield Nothing
		End While
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "JumpRoll_Roll", True, True)
		AudioManager.[Stop]("sally_double_jump_roll_loop")
		MyBase.StartCoroutine(Me.fall_cr())
		Me.jumpRollAttackTypeIndex += 1
		If Me.jumpRollAttackTypeIndex >= MyBase.properties.CurrentState.jumpRoll.JumpAttackTypeString.Split(New Char() { ","c }).Length Then
			Me.jumpRollAttackTypeIndex = 0
		End If
		Return
	End Function

	' Token: 0x06002C80 RID: 11392 RVA: 0x001A2174 File Offset: 0x001A0574
	Private Iterator Function fall_cr() As IEnumerator
		Dim speed As Single = MyBase.properties.CurrentState.teleport.fallingSpeed.RandomFloat()
		Dim iteration As Integer = 1
		Dim offset As Single = 150F
		Dim useTrigger As Boolean = False
		If Me.isTeleporting Then
			offset = 180F
			useTrigger = True
		Else
			offset = Me.ground.y
			useTrigger = False
		End If
		While MyBase.transform.position.y > offset
			MyBase.transform.position += Vector3.down * speed * CupheadTime.Delta
			If CupheadTime.Delta IsNot 0F Then
				speed += MyBase.properties.CurrentState.teleport.acceleration * CSng(iteration)
				iteration += 1
			End If
			Yield Nothing
		End While
		MyBase.transform.position = New Vector3(MyBase.transform.position.x, offset, 0F)
		If Me.isTeleporting Then
			MyBase.animator.SetTrigger("OnSawEnd")
		End If
		MyBase.StartCoroutine(Me.landing_cr(useTrigger))
		Yield Nothing
		Return
	End Function

	' Token: 0x06002C81 RID: 11393 RVA: 0x001A2190 File Offset: 0x001A0590
	Private Iterator Function rollAttack_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.jumpRoll.RollShotDelayRange.RandomFloat())
		Dim c As Char = MyBase.properties.CurrentState.jumpRoll.JumpAttackTypeString.Split(New Char() { ","c })(Me.jumpRollAttackTypeIndex)(0)
		If c <> "S"c Then
			If c = "B"c Then
				Me.SpawnProjectile()
			End If
		Else
			Me.SpawnShuriken()
		End If
		Return
	End Function

	' Token: 0x06002C82 RID: 11394 RVA: 0x001A21AC File Offset: 0x001A05AC
	Private Sub SpawnShuriken()
		For i As Integer = -1 To 1 - 1
			Dim abstractProjectile As AbstractProjectile = Me.shurikenPrefab.Create(MyBase.transform.position + Vector3.up * 0.5F)
			abstractProjectile.GetComponent(Of SallyStagePlayLevelShurikenBomb)().InitShuriken(MyBase.properties, i, Me.target)
		Next
	End Sub

	' Token: 0x06002C83 RID: 11395 RVA: 0x001A2214 File Offset: 0x001A0614
	Private Sub SpawnProjectile()
		Dim vector As Vector3 = Me.target.transform.position - Me.centerPoint.transform.position
		Dim sallyStagePlayLevelProjectile As SallyStagePlayLevelProjectile = Global.UnityEngine.[Object].Instantiate(Of SallyStagePlayLevelProjectile)(Me.projectilePrefab)
		sallyStagePlayLevelProjectile.Init(Me.centerPoint.transform.position, MathUtils.DirectionToAngle(vector), MyBase.properties.CurrentState.projectile)
	End Sub

	' Token: 0x06002C84 RID: 11396 RVA: 0x001A2289 File Offset: 0x001A0689
	Public Sub OnUmbrellaAttack()
		Me.state = SallyStagePlayLevelSally.State.Attack
		MyBase.StartCoroutine(Me.startUmbrella_cr())
	End Sub

	' Token: 0x06002C85 RID: 11397 RVA: 0x001A22A0 File Offset: 0x001A06A0
	Private Iterator Function startUmbrella_cr() As IEnumerator
		MyBase.animator.SetBool("UmbrellaAttack", True)
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Umbrella_Spin_Start", False, True)
		AudioManager.Play("sally_umbrella_spin")
		Me.emitAudioFromObject.Add("sally_umbrella_spin")
		Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.umbrella.initialAttackDelay)
		For i As Integer = 0 To MyBase.properties.CurrentState.umbrella.objectCount - 1
			If i <> 0 Then
				Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.umbrella.objectDelay)
			End If
			AudioManager.Play("sally_umbrella_spin_shoot")
			Me.emitAudioFromObject.Add("sally_umbrella_spin_shoot")
			Dim proj As AbstractProjectile = Me.umbrellaProjectilePrefab.Create(Me.spawnPoints(0).position)
			proj.GetComponent(Of SallyStagePlayLevelUmbrellaProjectile)().InitProjectile(MyBase.properties, CInt((-CInt(MyBase.transform.right.x))))
			proj = Me.umbrellaProjectilePrefab.Create(Me.spawnPoints(1).position)
			proj.GetComponent(Of SallyStagePlayLevelUmbrellaProjectile)().InitProjectile(MyBase.properties, CInt(MyBase.transform.right.x))
			If Me.getOutOfJump Then
				Exit For
			End If
		Next
		MyBase.animator.SetBool("UmbrellaAttack", False)
		Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.umbrella.hesitate)
		AudioManager.Play("sally_umbrella_spin_end")
		Me.emitAudioFromObject.Add("sally_umbrella_spin_end")
		Me.state = SallyStagePlayLevelSally.State.Idle
		Return
	End Function

	' Token: 0x06002C86 RID: 11398 RVA: 0x001A22BB File Offset: 0x001A06BB
	Private Sub UmbrellaIntroSFX()
		AudioManager.Play("sally_sally_umbrella_intro")
		Me.emitAudioFromObject.Add("sally_sally_umbrella_intro")
	End Sub

	' Token: 0x06002C87 RID: 11399 RVA: 0x001A22D8 File Offset: 0x001A06D8
	Public Sub OnKissAttack()
		Me.state = SallyStagePlayLevelSally.State.Attack
		MyBase.animator.SetTrigger("OnKissAttack")
		Me.target = PlayerManager.GetNext()
		If Me.target.center.x > Me.centerPoint.position.x Then
			If MyBase.transform.eulerAngles.y = 0F Then
				MyBase.transform.right *= -1F
			End If
		ElseIf MyBase.transform.eulerAngles.y = 180F Then
			MyBase.transform.right *= -1F
		End If
	End Sub

	' Token: 0x06002C88 RID: 11400 RVA: 0x001A23A8 File Offset: 0x001A07A8
	Private Sub SpawnHeart()
		Dim abstractProjectile As AbstractProjectile = Me.heartPrefab.Create(Me.spawnPoints(0).position)
		Dim flag As Boolean = MyBase.properties.CurrentState.kiss.heartType.Split(New Char() { ","c })(Me.heartTypeIndex)(0) <> "R"c
		Dim num As Integer = If((CInt(MyBase.transform.eulerAngles.y) <> 180), 1, (-1))
		abstractProjectile.GetComponent(Of SallyStagePlayLevelHeart)().InitHeart(MyBase.properties, num, flag)
		abstractProjectile.GetComponent(Of Transform)().SetScale(New Single?(CSng(If((MyBase.transform.right.x <= 0F), (-1), 1))), Nothing, Nothing)
		Me.heartTypeIndex += 1
		If Me.heartTypeIndex >= MyBase.properties.CurrentState.kiss.heartType.Split(New Char() { ","c }).Length Then
			Me.heartTypeIndex = 0
		End If
		MyBase.StartCoroutine(Me.endKiss_cr())
	End Sub

	' Token: 0x06002C89 RID: 11401 RVA: 0x001A24EB File Offset: 0x001A08EB
	Private Sub KissSFX()
		AudioManager.Play("sally_sally_kiss")
		Me.emitAudioFromObject.Add("sally_sally_kiss")
	End Sub

	' Token: 0x06002C8A RID: 11402 RVA: 0x001A2508 File Offset: 0x001A0908
	Private Iterator Function endKiss_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.kiss.hesitate)
		Me.state = SallyStagePlayLevelSally.State.Idle
		Return
	End Function

	' Token: 0x06002C8B RID: 11403 RVA: 0x001A2523 File Offset: 0x001A0923
	Public Sub OnTeleportAttack()
		Me.state = SallyStagePlayLevelSally.State.Attack
		Me.isTeleporting = True
		MyBase.animator.SetTrigger("OnTeleport")
	End Sub

	' Token: 0x06002C8C RID: 11404 RVA: 0x001A2544 File Offset: 0x001A0944
	Private Sub Teleport()
		MyBase.transform.SetPosition(Nothing, New Single?(CSng(Level.Current.Ceiling) + Me.teleportOffset), Nothing)
		MyBase.StartCoroutine(Me.delay_cr())
	End Sub

	' Token: 0x06002C8D RID: 11405 RVA: 0x001A2594 File Offset: 0x001A0994
	Private Iterator Function delay_cr() As IEnumerator
		Dim pos As Vector3
		pos.y = CSng(Level.Current.Ceiling) + Me.teleportOffset
		pos.z = 0F
		MyBase.animator.SetTrigger("OnTeleport")
		Yield MyBase.animator.WaitForAnimationToStart(Me, "Teleport_Loop", False)
		Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.teleport.offScreenDelay)
		Me.target = PlayerManager.GetNext()
		pos.x = Me.target.center.x + CSng(Parser.IntParse(MyBase.properties.CurrentState.teleport.appearOffsetString.Split(New Char() { ","c })(Me.teleportOffsetIndex)))
		If Parser.IntParse(MyBase.properties.CurrentState.teleport.appearOffsetString.Split(New Char() { ","c })(Me.teleportOffsetIndex)) <= 0 Then
			If pos.x - 75F < CSng(Level.Current.Left) Then
				pos.x = CSng((Level.Current.Left + 75))
			End If
			MyBase.transform.right *= -1F
		Else
			If pos.x + 75F > CSng(Level.Current.Right) Then
				pos.x = CSng((Level.Current.Right - 75))
			End If
			MyBase.transform.right *= 1F
		End If
		MyBase.transform.position = pos
		MyBase.StartCoroutine(Me.fall_cr())
		Yield MyBase.animator.WaitForAnimationToStart(Me, "Idle", False)
		Me.teleportOffsetIndex += 1
		If Me.teleportOffsetIndex >= MyBase.properties.CurrentState.teleport.appearOffsetString.Split(New Char() { ","c }).Length Then
			Me.teleportOffsetIndex = 0
		End If
		Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.teleport.hesitate)
		MyBase.transform.position = Me.ground
		Me.isTeleporting = False
		Yield Nothing
		Return
	End Function

	' Token: 0x06002C8E RID: 11406 RVA: 0x001A25AF File Offset: 0x001A09AF
	Private Sub MovePosition()
		MyBase.StartCoroutine(Me.move_position_cr())
	End Sub

	' Token: 0x06002C8F RID: 11407 RVA: 0x001A25C0 File Offset: 0x001A09C0
	Private Iterator Function move_position_cr() As IEnumerator
		Dim pos As Vector3 = MyBase.transform.position
		Dim speed As Single = 700F
		While MyBase.transform.position.y > Me.ground.y
			pos.y -= speed * CupheadTime.Delta
			MyBase.transform.position = pos
			Yield Nothing
		End While
		MyBase.transform.position = Me.ground
		Yield Nothing
		Return
	End Function

	' Token: 0x06002C90 RID: 11408 RVA: 0x001A25DB File Offset: 0x001A09DB
	Private Sub TeleportOutSFX()
		AudioManager.Play("sally_sally_teleport_out")
		Me.emitAudioFromObject.Add("sally_sally_teleport_out")
	End Sub

	' Token: 0x06002C91 RID: 11409 RVA: 0x001A25F7 File Offset: 0x001A09F7
	Private Sub TeleportEndSFX()
		AudioManager.Play("sally_sally_teleport_end")
		Me.emitAudioFromObject.Add("sally_sally_teleport_end")
	End Sub

	' Token: 0x06002C92 RID: 11410 RVA: 0x001A2613 File Offset: 0x001A0A13
	Public Sub OnPhase3(killedHusband As Boolean)
		Me.StopAllCoroutines()
		MyBase.StartCoroutine(Me.phase2_death_cr(killedHusband))
	End Sub

	' Token: 0x06002C93 RID: 11411 RVA: 0x001A262C File Offset: 0x001A0A2C
	Private Iterator Function phase2_death_cr(killedHusband As Boolean) As IEnumerator
		Dim speed As Single = 300F
		MyBase.GetComponent(Of LevelBossDeathExploder)().StartExplosion()
		MyBase.animator.SetTrigger("OnDeath")
		AudioManager.Play("sally_vox_death_cry")
		Me.emitAudioFromObject.Add("sally_vox_death_cry")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Death_Ph2_Start", False, True)
		While MyBase.transform.position.y < 660F
			MyBase.transform.position += Vector3.up * speed * CupheadTime.Delta
			Yield Nothing
		End While
		MyBase.GetComponent(Of LevelBossDeathExploder)().StopExplosions()
		Me.angel.StartPhase3(killedHusband)
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Yield Nothing
		Return
	End Function

	' Token: 0x06002C94 RID: 11412 RVA: 0x001A264E File Offset: 0x001A0A4E
	Public Sub PrePhase2()
		Me.getOutOfJump = True
		Me.isInvincible = True
	End Sub

	' Token: 0x06002C95 RID: 11413 RVA: 0x001A265E File Offset: 0x001A0A5E
	Public Sub OnPhase2()
		Me.state = SallyStagePlayLevelSally.State.Transition
		Me.jumpTypeIndex = 0
		Me.jumpRollAttackTypeIndex = 0
	End Sub

	' Token: 0x06002C96 RID: 11414 RVA: 0x001A2675 File Offset: 0x001A0A75
	Public Sub StartPhase2()
		Me.getOutOfJump = True
		MyBase.animator.SetTrigger("OnIntro")
		MyBase.StartCoroutine(Me.phase_2_cr())
	End Sub

	' Token: 0x06002C97 RID: 11415 RVA: 0x001A269B File Offset: 0x001A0A9B
	Private Sub Intro2SFX()
		AudioManager.Play("sally_sally_intro_phase2")
		Me.emitAudioFromObject.Add("sally_sally_intro_phase2")
	End Sub

	' Token: 0x06002C98 RID: 11416 RVA: 0x001A26B8 File Offset: 0x001A0AB8
	Private Iterator Function phase_2_cr() As IEnumerator
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Teleport_GONE", False, True)
		MyBase.StartCoroutine(Me.slide_cr())
		Yield MyBase.animator.WaitForAnimationToStart(Me, "Idle", False)
		Me.isInvincible = False
		Me.getOutOfJump = False
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		Me.state = SallyStagePlayLevelSally.State.Idle
		Me.house.StartAttacks()
		Yield Nothing
		Return
	End Function

	' Token: 0x06002C99 RID: 11417 RVA: 0x001A26D4 File Offset: 0x001A0AD4
	Private Iterator Function slide_cr() As IEnumerator
		Dim startPos As Single = 0F
		Dim endPos As Single = 0F
		Dim appearPos As Single = 300F
		Dim player As AbstractPlayerController = PlayerManager.GetPlayer(PlayerId.PlayerOne)
		Dim player2 As AbstractPlayerController = PlayerManager.GetPlayer(PlayerId.PlayerTwo)
		If player2 Is Nothing OrElse player.IsDead OrElse player2.IsDead Then
			If Me.target Is Nothing OrElse Me.target.IsDead Then
				Me.target = PlayerManager.GetNext()
			End If
			If Me.target.transform.position.x > 0F Then
				If MyBase.transform.right.x > 0F Then
					MyBase.transform.right *= -1F
				End If
				startPos = -840F
				endPos = -640F + appearPos
			Else
				If MyBase.transform.right.x < 0F Then
					MyBase.transform.right *= -1F
				End If
				startPos = 840F
				endPos = 640F - appearPos
			End If
		Else
			Dim num As Single = -640F - player.transform.position.x
			Dim num2 As Single = 640F - player.transform.position.x
			Dim num3 As Single = -640F - player2.transform.position.x
			Dim num4 As Single = 640F - player2.transform.position.x
			If player.transform.position.x < 0F Then
				If player2.transform.position.x < 0F Then
					If MyBase.transform.right.x < 0F Then
						MyBase.transform.right *= -1F
					End If
					startPos = 840F
					endPos = 640F - appearPos
				ElseIf num < num4 Then
					If MyBase.transform.right.x < 0F Then
						MyBase.transform.right *= -1F
					End If
					startPos = 840F
					endPos = 640F - appearPos
				Else
					If MyBase.transform.right.x > 0F Then
						MyBase.transform.right *= -1F
					End If
					startPos = -840F
					endPos = -640F + appearPos
				End If
			ElseIf player2.transform.position.x > 0F Then
				If MyBase.transform.right.x > 0F Then
					MyBase.transform.right *= -1F
				End If
				startPos = -840F
				endPos = -640F + appearPos
			ElseIf num2 < num3 Then
				If MyBase.transform.right.x < 0F Then
					MyBase.transform.right *= -1F
				End If
				startPos = 840F
				endPos = 640F - appearPos
			Else
				If MyBase.transform.right.x > 0F Then
					MyBase.transform.right *= -1F
				End If
				startPos = -840F
				endPos = -640F + appearPos
			End If
		End If
		MyBase.transform.position = New Vector3(startPos, MyBase.transform.position.y, MyBase.transform.position.z)
		Dim t As Single = 0F
		Dim time As Single = 0.75F
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		Dim frameTime As Single = 0F
		While t < time
			t += CupheadTime.FixedDelta
			frameTime += CupheadTime.FixedDelta
			If frameTime > 0.041666668F Then
				frameTime -= 0.041666668F
				Dim num5 As Single = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0F, 1F, t / time)
				MyBase.transform.SetPosition(New Single?(Mathf.Lerp(startPos, endPos, num5)), Nothing, Nothing)
			End If
			Yield wait
		End While
		Yield Nothing
		Return
	End Function

	' Token: 0x06002C9A RID: 11418 RVA: 0x001A26EF File Offset: 0x001A0AEF
	Protected Overrides Sub OnDestroy()
		Me.StopAllCoroutines()
		MyBase.OnDestroy()
		Me.shurikenPrefab = Nothing
		Me.projectilePrefab = Nothing
		Me.umbrellaProjectilePrefab = Nothing
		Me.heartPrefab = Nothing
		Me.shadowPrefab = Nothing
	End Sub

	' Token: 0x06002C9B RID: 11419 RVA: 0x001A2720 File Offset: 0x001A0B20
	Private Sub SoundSallyVoxAttackMmmYoh()
		AudioManager.Play("sally_vox_attack_mmm_yoh")
		Me.emitAudioFromObject.Add("sally_vox_attack_mmm_yoh")
	End Sub

	' Token: 0x06002C9C RID: 11420 RVA: 0x001A273C File Offset: 0x001A0B3C
	Private Sub SoundSallyVoxAttackQuick()
		AudioManager.Play("sally_vox_attack_quick")
		Me.emitAudioFromObject.Add("sally_vox_attack_quick")
		AudioManager.[Stop]("sally_vox_maniacal")
	End Sub

	' Token: 0x06002C9D RID: 11421 RVA: 0x001A2762 File Offset: 0x001A0B62
	Private Sub SoundSallyVoxAttackDeathCry()
		AudioManager.Play("sally_vox_death_cry")
		Me.emitAudioFromObject.Add("sally_vox_death_cry")
	End Sub

	' Token: 0x06002C9E RID: 11422 RVA: 0x001A277E File Offset: 0x001A0B7E
	Private Sub SoundSallyVoxFrustrated()
		AudioManager.Play("sally_vox_frustrated")
		Me.emitAudioFromObject.Add("sally_vox_frustrated")
	End Sub

	' Token: 0x06002C9F RID: 11423 RVA: 0x001A279A File Offset: 0x001A0B9A
	Private Sub SoundSallyVoxLaughBig()
		AudioManager.Play("sally_vox_laugh_big")
		Me.emitAudioFromObject.Add("sally_vox_laugh_big")
	End Sub

	' Token: 0x06002CA0 RID: 11424 RVA: 0x001A27B6 File Offset: 0x001A0BB6
	Private Sub SoundSallyVoxLaughSmall()
		AudioManager.Play("sally_vox_laugh_small")
		Me.emitAudioFromObject.Add("sally_vox_laugh_small")
	End Sub

	' Token: 0x06002CA1 RID: 11425 RVA: 0x001A27D2 File Offset: 0x001A0BD2
	Private Sub SoundSallyVoxLaughManiacal()
		AudioManager.Play("sally_vox_maniacal")
		Me.emitAudioFromObject.Add("sally_vox_maniacal")
	End Sub

	' Token: 0x06002CA2 RID: 11426 RVA: 0x001A27EE File Offset: 0x001A0BEE
	Private Sub SoundSallyVoxDeathOperatic()
		AudioManager.Play("sally_vox_operatic_death")
		Me.emitAudioFromObject.Add("sally_vox_operatic_death")
	End Sub

	' Token: 0x06002CA3 RID: 11427 RVA: 0x001A280A File Offset: 0x001A0C0A
	Private Sub SoundSallyVoxPainGrowl()
		AudioManager.Play("sally_vox_pain_growl")
		Me.emitAudioFromObject.Add("sally_vox_pain_growl")
	End Sub

	' Token: 0x04003500 RID: 13568
	<Header("Projectiles")>
	Private Const FRAME_TIME As Single = 0.041666668F

	' Token: 0x04003501 RID: 13569
	<SerializeField()>
	Public collisionChild As CollisionChild

	' Token: 0x04003502 RID: 13570
	<SerializeField()>
	Private husband As Transform

	' Token: 0x04003503 RID: 13571
	<SerializeField()>
	Private angel As SallyStagePlayLevelAngel

	' Token: 0x04003504 RID: 13572
	<SerializeField()>
	Private shurikenPrefab As SallyStagePlayLevelShurikenBomb

	' Token: 0x04003505 RID: 13573
	<SerializeField()>
	Private projectilePrefab As SallyStagePlayLevelProjectile

	' Token: 0x04003506 RID: 13574
	<SerializeField()>
	Private umbrellaProjectilePrefab As SallyStagePlayLevelUmbrellaProjectile

	' Token: 0x04003507 RID: 13575
	<SerializeField()>
	Private heartPrefab As SallyStagePlayLevelHeart

	' Token: 0x04003508 RID: 13576
	<SerializeField()>
	Private house As SallyStagePlayLevelHouse

	' Token: 0x0400350A RID: 13578
	Private Const SALLY_INIT_JUMP_TIME As Single = 0.1665F

	' Token: 0x0400350B RID: 13579
	Private jumpType As SallyStagePlayLevelSally.JumpType

	' Token: 0x0400350C RID: 13580
	Private jumpTypeIndex As Integer

	' Token: 0x0400350D RID: 13581
	Private jumpCountIndex As Integer

	' Token: 0x0400350E RID: 13582
	Private currentJumpAttackCount As Integer

	' Token: 0x0400350F RID: 13583
	Private jumpRollAttackTypeIndex As Integer

	' Token: 0x04003510 RID: 13584
	Private heartTypeIndex As Integer

	' Token: 0x04003511 RID: 13585
	Private teleportOffsetIndex As Integer

	' Token: 0x04003512 RID: 13586
	Private teleportOffset As Single = 500F

	' Token: 0x04003513 RID: 13587
	Private getOutOfJump As Boolean

	' Token: 0x04003514 RID: 13588
	Private isTeleporting As Boolean

	' Token: 0x04003515 RID: 13589
	Private isInvincible As Boolean

	' Token: 0x04003516 RID: 13590
	Private bounds As Vector2

	' Token: 0x04003517 RID: 13591
	Private ground As Vector3

	' Token: 0x04003518 RID: 13592
	Private target As AbstractPlayerController

	' Token: 0x04003519 RID: 13593
	Private damageDealer As DamageDealer

	' Token: 0x0400351A RID: 13594
	Private damageReceiver As DamageReceiver

	' Token: 0x0400351B RID: 13595
	<Space(10F)>
	<SerializeField()>
	Private shadowPrefab As GameObject

	' Token: 0x0400351C RID: 13596
	<SerializeField()>
	Private centerPoint As Transform

	' Token: 0x0400351D RID: 13597
	<SerializeField()>
	Private spawnPoints As Transform()

	' Token: 0x020007B6 RID: 1974
	Public Enum State
		' Token: 0x0400351F RID: 13599
		Intro
		' Token: 0x04003520 RID: 13600
		Idle
		' Token: 0x04003521 RID: 13601
		Attack
		' Token: 0x04003522 RID: 13602
		Transition
	End Enum

	' Token: 0x020007B7 RID: 1975
	Private Enum JumpType
		' Token: 0x04003524 RID: 13604
		DiveKick = 1
		' Token: 0x04003525 RID: 13605
		DoubleJump
	End Enum
End Class
