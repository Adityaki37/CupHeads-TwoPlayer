Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000776 RID: 1910
Public Class RobotLevelRobotChest
	Inherits RobotLevelRobotBodyPart

	' Token: 0x060029B7 RID: 10679 RVA: 0x00184FE0 File Offset: 0x001833E0
	Public Overrides Sub InitBodyPart(parent As RobotLevelRobot, properties As LevelProperties.Robot, Optional primaryHP As Integer = 0, Optional secondaryHP As Integer = 1, Optional attackDelayMinus As Single = 0F)
		Me.primaryAttackDelay = properties.CurrentState.orb.orbInitialSpawnDelay.RandomFloat()
		Me.secondaryAttackDelay = properties.CurrentState.arms.attackDelayRange.RandomFloat()
		primaryHP = properties.CurrentState.orb.chestHP
		secondaryHP = properties.CurrentState.heart.heartHP
		attackDelayMinus = properties.CurrentState.orb.orbSpawnDelayMinus
		Me.armsTypeIndex = Global.UnityEngine.Random.Range(0, properties.CurrentState.arms.attackString.Split(New Char() { ","c }).Length)
		Me.twistyArmsPositionIndex = Global.UnityEngine.Random.Range(0, properties.CurrentState.twistyArms.twistyPositionString.Split(New Char() { ","c }).Length)
		MyBase.InitBodyPart(parent, properties, primaryHP, secondaryHP, attackDelayMinus)
		MyBase.animator.Play("Porthole_Idle", 0, 0.75F)
		MyBase.animator.Play("Off_Idle", 1, 0.75F)
		MyBase.StartCoroutine(Me.panicArmsLoop_cr())
		MyBase.StartCoroutine(Me.close_porthole_cr())
		Me.StartPrimary()
		Me.damageEffectRenderer = Me.damageEffect.GetComponent(Of SpriteRenderer)()
	End Sub

	' Token: 0x060029B8 RID: 10680 RVA: 0x0018511F File Offset: 0x0018351F
	Protected Overrides Sub OnPrimaryAttack()
		If Me.current = RobotLevelRobotBodyPart.state.primary Then
			MyBase.animator.SetTrigger("OnPortOpen")
			MyBase.OnPrimaryAttack()
		End If
	End Sub

	' Token: 0x060029B9 RID: 10681 RVA: 0x00185142 File Offset: 0x00183542
	Private Sub SpawnOrb()
		If Me.current <> RobotLevelRobotBodyPart.state.primary Then
			Return
		End If
		MyBase.StartCoroutine(Me.spawn_orb_cr())
	End Sub

	' Token: 0x060029BA RID: 10682 RVA: 0x00185160 File Offset: 0x00183560
	Private Iterator Function spawn_orb_cr() As IEnumerator
		Dim orb As RobotLevelOrb = Me.primary.GetComponent(Of RobotLevelOrb)().Create(Me.portholeRoot.transform.position, Me.portholeOffsetRoot.position)
		Me.primaryAttackDelay = Me.properties.CurrentState.orb.orbSpawnDelay
		orb.InitOrb(Me.properties)
		Me.isAttacking = False
		MyBase.animator.SetTrigger("OnFirstClose")
		Yield Nothing
		Return
	End Function

	' Token: 0x060029BB RID: 10683 RVA: 0x0018517C File Offset: 0x0018357C
	Private Iterator Function close_porthole_cr() As IEnumerator
		MyBase.animator.SetTrigger("OnFirstClose")
		Yield Nothing
		Return
	End Function

	' Token: 0x060029BC RID: 10684 RVA: 0x00185198 File Offset: 0x00183598
	Protected Overrides Sub OnPrimaryDeath()
		If Me.current <> RobotLevelRobotBodyPart.state.secondary AndAlso Me.currentHealth(0) <= 0F Then
			AudioManager.Play("robot_upper_chest_port_destroyed")
			Me.emitAudioFromObject.Add("robot_upper_chest_port_destroyed")
			Me.torsoTop.enabled = False
			Me.StartSecondary()
			MyBase.GetComponent(Of Collider2D)().enabled = False
			Me.DeathEffect()
			Me.panicArmsLoop = True
			Me.parent.animator.Play("Transition_Arms")
			For Each spriteRenderer As SpriteRenderer In MyBase.transform.GetComponentsInChildren(Of SpriteRenderer)()
				spriteRenderer.enabled = False
			Next
			For Each gameObject As GameObject In Me.damagedPortholes
				gameObject.SetActive(True)
				gameObject.GetComponent(Of SpriteRenderer)().enabled = True
			Next
		End If
		MyBase.OnPrimaryDeath()
	End Sub

	' Token: 0x060029BD RID: 10685 RVA: 0x00185288 File Offset: 0x00183688
	Private Sub EnablePorthole()
		If Me.current = RobotLevelRobotBodyPart.state.primary Then
			For Each spriteRenderer As SpriteRenderer In MyBase.transform.GetComponentsInChildren(Of SpriteRenderer)()
				spriteRenderer.enabled = True
			Next
		End If
	End Sub

	' Token: 0x060029BE RID: 10686 RVA: 0x001852CC File Offset: 0x001836CC
	Private Sub DisablePorthole()
		If Me.current = RobotLevelRobotBodyPart.state.primary Then
			For Each spriteRenderer As SpriteRenderer In MyBase.transform.GetComponentsInChildren(Of SpriteRenderer)()
				spriteRenderer.enabled = False
			Next
		End If
	End Sub

	' Token: 0x060029BF RID: 10687 RVA: 0x0018530F File Offset: 0x0018370F
	Protected Overrides Sub StartSecondary()
		Me.secondary = Global.UnityEngine.[Object].Instantiate(Of GameObject)(Me.secondary)
		Me.secondaryAnimator = Me.secondary.GetComponent(Of Animator)()
		MyBase.StartCoroutine(Me.secondaryStart_cr())
	End Sub

	' Token: 0x060029C0 RID: 10688 RVA: 0x00185340 File Offset: 0x00183740
	Private Iterator Function secondaryStart_cr() As IEnumerator
		Yield Me.parent.animator.WaitForAnimationToEnd(Me.parent, "Extend", 5, True, True)
		MyBase.StartSecondary()
		Return
	End Function

	' Token: 0x060029C1 RID: 10689 RVA: 0x0018535C File Offset: 0x0018375C
	Protected Overrides Sub OnSecondaryAttack()
		If Not Me.armsActive Then
			If CSng(Global.UnityEngine.Random.Range(0, 100)) <= 25F AndAlso Not AudioManager.CheckIfPlaying("robot_vocals_laugh") Then
				AudioManager.Play("robot_vocals_laugh")
				Me.emitAudioFromObject.Add("robot_vocals_laugh")
			End If
			Me.parent.animator.Play("Fast", 5, Me.parent.animator.GetCurrentAnimatorStateInfo(5).normalizedTime Mod 1F)
			Me.secondaryAttackDelay = 0F
			Me.armsActive = True
			Dim c As Char = Me.properties.CurrentState.arms.attackString.Split(New Char() { ","c })(Me.armsTypeIndex)(0)
			If c <> "M"c Then
				If c = "T"c Then
					MyBase.StartCoroutine(Me.twistyArmsEnter_cr())
				End If
			Else
				MyBase.StartCoroutine(Me.magnetArmsIntro_cr())
			End If
		End If
		MyBase.OnSecondaryAttack()
	End Sub

	' Token: 0x060029C2 RID: 10690 RVA: 0x00185470 File Offset: 0x00183870
	Private Iterator Function twistyArmsEnter_cr() As IEnumerator
		Me.secondary.GetComponent(Of RobotLevelSecondaryArms)().InitHelper(Me.properties)
		Me.secondaryAnimator.Play("Twisty_Arms_Enter", 0, 0F)
		AudioManager.Play("robot_arms_extend_appear")
		Yield Nothing
		Me.spriteBounds = Me.secondary.GetComponent(Of SpriteRenderer)().bounds.size
		Me.secondary.transform.position = New Vector3(-1248F, CSng((Level.Current.Ground + Parser.IntParse(Me.properties.CurrentState.twistyArms.twistyPositionString.Split(New Char() { ","c })(Me.twistyArmsPositionIndex)))), 0F)
		Me.secondary.transform.rotation = Quaternion.identity
		While Me.secondary.transform.position.x < -1248F + Me.properties.CurrentState.twistyArms.warningArmsMoveAmount
			Me.secondary.transform.position += Vector3.right * Me.properties.CurrentState.twistyArms.twistyMoveSpeed * CupheadTime.Delta
			Yield Nothing
		End While
		Yield CupheadTime.WaitForSeconds(Me, Me.properties.CurrentState.twistyArms.warningDuration)
		Me.secondary.GetComponent(Of BoxCollider2D)().enabled = True
		AudioManager.Play("robot_arms_extend_across")
		Me.emitAudioFromObject.Add("robot_arms_extend_across")
		While Me.secondary.transform.position.x < -1248F + Me.spriteBounds.x / 8F * 6F
			Me.secondary.transform.position += Vector3.right * Me.properties.CurrentState.twistyArms.twistyMoveSpeed * CupheadTime.Delta
			Yield Nothing
		End While
		Yield CupheadTime.WaitForSeconds(Me, Me.properties.CurrentState.twistyArms.twistyArmsStayDuration)
		AudioManager.Play("robot_arms_extend_back")
		Me.emitAudioFromObject.Add("robot_arms_extend_back")
		MyBase.StartCoroutine(Me.twistyArmsExit_cr(False))
		Return
	End Function

	' Token: 0x060029C3 RID: 10691 RVA: 0x0018548C File Offset: 0x0018388C
	Private Iterator Function twistyArmsExit_cr(Optional isPermaDeath As Boolean = False) As IEnumerator
		Dim speedMultiplier As Single = 1F
		If isPermaDeath Then
			speedMultiplier = 2F
		End If
		Dim nt As Single = MyBase.animator.GetCurrentAnimatorStateInfo(0).normalizedTime Mod 1F
		Me.secondaryAnimator.Play("Twisty_Arms_Exit", -1, nt)
		While Me.secondary.transform.position.x > -1248F
			Me.secondary.transform.position += Vector3.left * Me.properties.CurrentState.twistyArms.twistyMoveSpeed * speedMultiplier * CupheadTime.Delta
			Yield Nothing
		End While
		Me.secondaryAnimator.Play("Twisty_Arms_Enter")
		Me.twistyArmsPositionIndex += 1
		If Me.twistyArmsPositionIndex >= Me.properties.CurrentState.twistyArms.twistyPositionString.Split(New Char() { ","c }).Length Then
			Me.twistyArmsPositionIndex = 0
		End If
		Me.secondaryAttackDelay = Me.properties.CurrentState.arms.attackDelayRange.RandomFloat()
		Yield Nothing
		Me.secondary.GetComponent(Of BoxCollider2D)().enabled = False
		Me.armsActive = False
		Me.armsTypeIndex += 1
		If Me.armsTypeIndex >= Me.properties.CurrentState.arms.attackString.Split(New Char() { ","c }).Length Then
			Me.armsTypeIndex = 0
		End If
		Me.parent.animator.Play("Slow", 5, Me.parent.animator.GetCurrentAnimatorStateInfo(5).normalizedTime Mod 1F)
		Return
	End Function

	' Token: 0x060029C4 RID: 10692 RVA: 0x001854B0 File Offset: 0x001838B0
	Private Iterator Function magnetArmsIntro_cr() As IEnumerator
		Me.secondaryAnimator.Play("Magnet_Arms", -1, 0F)
		Yield Nothing
		Me.spriteBounds = Me.secondary.GetComponent(Of SpriteRenderer)().bounds.size
		Me.secondary.transform.position = Me.magnetStartRoot.transform.position
		Yield CupheadTime.WaitForSeconds(Me, Me.properties.CurrentState.magnetArms.magnetStartDelay)
		AudioManager.Play("robot_magnet_arms_start")
		While AudioManager.CheckIfPlaying("robot_magnet_arms_start")
			Yield Nothing
		End While
		AudioManager.PlayLoop("robot_magnet_arms_loop")
		Dim t As Single = 0F
		Dim time As Single = 1.8F
		Dim deltaRotation As Single = 0F
		While t < time
			t += CupheadTime.Delta
			deltaRotation = Mathf.Lerp(60F, 0F, t / time)
			Me.secondary.transform.position = Vector2.Lerp(Me.magnetStartRoot.transform.position, Me.magnetEndRoot.transform.position, t / time)
			Me.secondary.transform.SetEulerAngles(Nothing, Nothing, New Single?(deltaRotation))
			Yield Nothing
		End While
		time = 0F
		While time < Me.properties.CurrentState.magnetArms.magnetStayDelay
			time += CupheadTime.Delta
			For Each abstractPlayerController As AbstractPlayerController In PlayerManager.GetAllPlayers()
				Dim player As PlanePlayerController = CType(abstractPlayerController, PlanePlayerController)
				If Not(player Is Nothing) Then
					Dim playerForce As Vector2 = (PlayerManager.GetNext().center - Me.secondary.transform.GetChild(1).transform.position).normalized * Me.properties.CurrentState.magnetArms.magnetForce * 0.5F
					Me.force = New PlanePlayerMotor.Force(playerForce, True)
					player.motor.AddForce(Me.force)
					Yield Nothing
					time += CupheadTime.Delta
					If player.motor IsNot Nothing Then
						player.motor.RemoveForce(Me.force)
					End If
				End If
			Next
			If Me.current = RobotLevelRobotBodyPart.state.none Then
				time = Me.properties.CurrentState.magnetArms.magnetStayDelay
			End If
			Yield Nothing
		End While
		AudioManager.[Stop]("robot_magnet_arms_loop")
		AudioManager.Play("robot_magnet_arms_end")
		MyBase.StartCoroutine(Me.magnetArmsExit_cr(False))
		Return
	End Function

	' Token: 0x060029C5 RID: 10693 RVA: 0x001854CC File Offset: 0x001838CC
	Private Iterator Function magnetArmsExit_cr(Optional isPermaDeath As Boolean = False) As IEnumerator
		For Each abstractPlayerController As AbstractPlayerController In PlayerManager.GetAllPlayers()
			Dim planePlayerController As PlanePlayerController = CType(abstractPlayerController, PlanePlayerController)
			If Not(planePlayerController Is Nothing) Then
				planePlayerController.motor.RemoveForce(Me.force)
			End If
		Next
		Dim delay As Single = (1F - Me.secondaryAnimator.GetCurrentAnimatorStateInfo(1).normalizedTime Mod 1F) * Me.secondaryAnimator.GetCurrentAnimatorStateInfo(1).length
		Me.secondaryAnimator.Play("Magnet_Arms", -1, 0F)
		Yield CupheadTime.WaitForSeconds(Me, delay)
		Dim t As Single = 0F
		Dim time As Single = 1.8F
		Dim deltaRotation As Single = 0F
		Dim root As Vector3 = If((Not isPermaDeath), Me.magnetEndRoot.transform.position, Me.secondary.transform.position)
		While t < time
			t += CupheadTime.Delta
			deltaRotation = Mathf.Lerp(0F, 60F, t / time)
			Me.secondary.transform.position = Vector2.Lerp(root, Me.magnetStartRoot.transform.position, t / time)
			Me.secondary.transform.SetEulerAngles(Nothing, Nothing, New Single?(deltaRotation))
			Yield Nothing
		End While
		Me.secondaryAttackDelay = Me.properties.CurrentState.arms.attackDelayRange.RandomFloat()
		Yield Nothing
		Me.armsActive = False
		Me.armsTypeIndex += 1
		If Me.armsTypeIndex >= Me.properties.CurrentState.arms.attackString.Split(New Char() { ","c }).Length Then
			Me.armsTypeIndex = 0
		End If
		Me.parent.animator.Play("Slow", 5, Me.parent.animator.GetCurrentAnimatorStateInfo(5).normalizedTime Mod 1F)
		Return
	End Function

	' Token: 0x060029C6 RID: 10694 RVA: 0x001854F0 File Offset: 0x001838F0
	Private Iterator Function panicArmsLoop_cr() As IEnumerator
		While True
			Dim normalizedTime As Single = Me.parent.animator.GetCurrentAnimatorStateInfo(7).normalizedTime
			normalizedTime = normalizedTime Mod 1F
			Dim currentFrame As Integer = CInt((normalizedTime * 24F))
			If Me.panicArmsLoop Then
				Me.frontArm.transform.position = Me.panicArmsPath(currentFrame).position
				Dim num As Integer = currentFrame + 10
				If num >= 24 Then
					num -= 24
				End If
				Me.backArm.transform.position = Me.panicArmsPath(num).position
			End If
			currentFrame += 1
			If currentFrame >= 24 Then
				currentFrame = 0
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060029C7 RID: 10695 RVA: 0x0018550B File Offset: 0x0018390B
	Protected Overrides Sub OnSecondaryDeath()
		MyBase.StartCoroutine(Me.heart_cr())
		MyBase.OnSecondaryDeath()
	End Sub

	' Token: 0x060029C8 RID: 10696 RVA: 0x00185520 File Offset: 0x00183920
	Private Sub HeartIntroSFX()
		AudioManager.Play("robot_heart_spring_out")
		Me.emitAudioFromObject.Add("robot_heart_spring_out")
	End Sub

	' Token: 0x060029C9 RID: 10697 RVA: 0x0018553C File Offset: 0x0018393C
	Private Iterator Function heart_cr() As IEnumerator
		MyBase.GetComponent(Of SpriteRenderer)().enabled = True
		Dim waitDuration As Single = Me.parent.animator.GetCurrentAnimatorStateInfo(2).length
		Yield CupheadTime.WaitForSeconds(Me, waitDuration)
		MyBase.animator.SetTrigger("OnHeartActive")
		MyBase.GetComponent(Of Collider2D)().enabled = True
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Intro", 1, True, True)
		Return
	End Function

	' Token: 0x060029CA RID: 10698 RVA: 0x00185558 File Offset: 0x00183958
	Protected Overrides Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		If Me.current = RobotLevelRobotBodyPart.state.primary Then
			MyBase.OnDamageTaken(info)
			If Me.damageEffectRoutine IsNot Nothing Then
				MyBase.StopCoroutine(Me.damageEffectRoutine)
			End If
			Me.damageEffectRoutine = Me.damageEffect_cr()
			MyBase.StartCoroutine(Me.damageEffectRoutine)
		Else
			Dim num As Single = Me.currentHealth(1)
			Me.currentHealth(1) -= info.damage
			If Me.currentHealth(1) / CSng(Me.properties.CurrentState.heart.heartHP) * 100F <= CSng(Me.properties.CurrentState.heart.heartDamageChangePercentage) Then
				If Not Me.playedCrackedSound Then
					AudioManager.Play("robot_heart_spring_cracked")
					Me.emitAudioFromObject.Add("robot_heart_spring_cracked")
					Me.playedCrackedSound = True
				End If
				MyBase.animator.Play("Damaged Loop", 1, MyBase.animator.GetCurrentAnimatorStateInfo(0).normalizedTime Mod 1F)
			End If
			If Me.currentHealth(1) <= 0F AndAlso Not Me.destroyed Then
				AudioManager.[Stop]("robot_magnet_arms_loop")
				AudioManager.Play("robot_heart_spring_destroyed")
				Me.emitAudioFromObject.Add("robot_heart_spring_destroyed")
				Me.properties.DealDamageToNextNamedState()
				Me.destroyed = True
			End If
			If num > 0F Then
				Level.Current.timeline.DealDamage(Mathf.Clamp(num - Me.currentHealth(1), 0F, num))
			End If
		End If
	End Sub

	' Token: 0x060029CB RID: 10699 RVA: 0x001856E4 File Offset: 0x00183AE4
	Protected Overrides Sub ExitCurrentAttacks()
		Me.StopAllCoroutines()
		Dim flag As Boolean = False
		If Level.Current.mode = Level.Mode.Easy Then
			flag = True
			Me.secondary.GetComponent(Of RobotLevelSecondaryArms)().BossAlive = False
		End If
		If Me.armsActive Then
			Dim c As Char = Me.properties.CurrentState.arms.attackString.Split(New Char() { ","c })(Me.armsTypeIndex)(0)
			If c <> "T"c Then
				If c = "M"c Then
					MyBase.StartCoroutine(Me.magnetArmsExit_cr(flag))
				End If
			Else
				MyBase.StartCoroutine(Me.twistyArmsExit_cr(flag))
			End If
		End If
		MyBase.ExitCurrentAttacks()
	End Sub

	' Token: 0x060029CC RID: 10700 RVA: 0x0018579F File Offset: 0x00183B9F
	Public Sub InitAnims()
		MyBase.animator.SetTrigger("OnRobotIntro")
	End Sub

	' Token: 0x060029CD RID: 10701 RVA: 0x001857B4 File Offset: 0x00183BB4
	Protected Overrides Sub Die()
		If Me.damageEffectRoutine IsNot Nothing Then
			MyBase.StopCoroutine(Me.damageEffectRoutine)
		End If
		Me.damageEffect.SetActive(False)
		For Each spriteRenderer As SpriteRenderer In MyBase.transform.GetComponentsInChildren(Of SpriteRenderer)()
			spriteRenderer.enabled = False
		Next
		MyBase.Die()
	End Sub

	' Token: 0x060029CE RID: 10702 RVA: 0x00185818 File Offset: 0x00183C18
	Private Iterator Function damageEffect_cr() As IEnumerator
		For i As Integer = 0 To 3 - 1
			Me.damageEffectRenderer.enabled = True
			Me.damageEffect.SetActive(True)
			Yield CupheadTime.WaitForSeconds(Me, 0.041666668F)
			Me.damageEffect.SetActive(False)
			Yield CupheadTime.WaitForSeconds(Me, 0.041666668F)
		Next
		Return
	End Function

	' Token: 0x040032A3 RID: 12963
	Private Const PANIC_ARMS_ANIM_FRAME_COUNT As Integer = 24

	' Token: 0x040032A4 RID: 12964
	Private frameCount As Integer

	' Token: 0x040032A5 RID: 12965
	Private playedCrackedSound As Boolean

	' Token: 0x040032A6 RID: 12966
	Private armsActive As Boolean

	' Token: 0x040032A7 RID: 12967
	Private panicArmsLoop As Boolean

	' Token: 0x040032A8 RID: 12968
	Private armsTypeIndex As Integer

	' Token: 0x040032A9 RID: 12969
	Private twistyArmsPositionIndex As Integer

	' Token: 0x040032AA RID: 12970
	Private spriteBounds As Vector3

	' Token: 0x040032AB RID: 12971
	Private secondaryAnimator As Animator

	' Token: 0x040032AC RID: 12972
	Private force As PlanePlayerMotor.Force

	' Token: 0x040032AD RID: 12973
	Private destroyed As Boolean

	' Token: 0x040032AE RID: 12974
	<SerializeField()>
	Private torsoTop As SpriteRenderer

	' Token: 0x040032AF RID: 12975
	<SerializeField()>
	Private damagedPortholes As GameObject()

	' Token: 0x040032B0 RID: 12976
	<SerializeField()>
	Private frontArm As GameObject

	' Token: 0x040032B1 RID: 12977
	<SerializeField()>
	Private backArm As GameObject

	' Token: 0x040032B2 RID: 12978
	<SerializeField()>
	Private portholeRoot As Transform

	' Token: 0x040032B3 RID: 12979
	<SerializeField()>
	Private portholeOffsetRoot As Transform

	' Token: 0x040032B4 RID: 12980
	<SerializeField()>
	Private panicArmsPath As Transform()

	' Token: 0x040032B5 RID: 12981
	<SerializeField()>
	Private magnetStartRoot As Transform

	' Token: 0x040032B6 RID: 12982
	<SerializeField()>
	Private magnetEndRoot As Transform

	' Token: 0x040032B7 RID: 12983
	<SerializeField()>
	Private damageEffect As GameObject

	' Token: 0x040032B8 RID: 12984
	Private damageEffectRoutine As IEnumerator

	' Token: 0x040032B9 RID: 12985
	Private damageEffectRenderer As SpriteRenderer

	' Token: 0x02000777 RID: 1911
	Public Enum AnimationLayers
		' Token: 0x040032BB RID: 12987
		Main
		' Token: 0x040032BC RID: 12988
		Pilot
		' Token: 0x040032BD RID: 12989
		Head
		' Token: 0x040032BE RID: 12990
		Hose
		' Token: 0x040032BF RID: 12991
		Waist
		' Token: 0x040032C0 RID: 12992
		BackArm
		' Token: 0x040032C1 RID: 12993
		FrontArm
		' Token: 0x040032C2 RID: 12994
		Torso
	End Enum
End Class
