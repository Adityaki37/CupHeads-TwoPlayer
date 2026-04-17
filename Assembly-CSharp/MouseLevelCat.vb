Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020006E6 RID: 1766
Public Class MouseLevelCat
	Inherits LevelProperties.Mouse.Entity

	' Token: 0x170003C2 RID: 962
	' (get) Token: 0x060025C2 RID: 9666 RVA: 0x001617FD File Offset: 0x0015FBFD
	' (set) Token: 0x060025C3 RID: 9667 RVA: 0x00161805 File Offset: 0x0015FC05
	Public Property state As MouseLevelCat.State

	' Token: 0x060025C4 RID: 9668 RVA: 0x00161810 File Offset: 0x0015FC10
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		Me.headStartPos = Me.head.localPosition
		Me.head.GetComponent(Of Collider2D)().enabled = False
	End Sub

	' Token: 0x060025C5 RID: 9669 RVA: 0x0016186D File Offset: 0x0015FC6D
	Public Overrides Sub LevelInit(properties As LevelProperties.Mouse)
		MyBase.LevelInit(properties)
		Me.fallingObjectsIndex = Global.UnityEngine.Random.Range(0, properties.CurrentState.claw.fallingObjectStrings.Length)
	End Sub

	' Token: 0x060025C6 RID: 9670 RVA: 0x00161894 File Offset: 0x0015FC94
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		MyBase.properties.DealDamage(info.damage)
	End Sub

	' Token: 0x060025C7 RID: 9671 RVA: 0x001618A7 File Offset: 0x0015FCA7
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.fallingObjectPrefabs = Nothing
	End Sub

	' Token: 0x060025C8 RID: 9672 RVA: 0x001618B6 File Offset: 0x0015FCB6
	Public Sub StartIntro()
		AddHandler MyBase.properties.OnBossDeath, AddressOf Me.OnBossDeath
		MyBase.StartCoroutine(Me.intro_cr())
	End Sub

	' Token: 0x060025C9 RID: 9673 RVA: 0x001618DC File Offset: 0x0015FCDC
	Private Iterator Function intro_cr() As IEnumerator
		Me.head.GetComponent(Of Collider2D)().enabled = True
		Yield CupheadTime.WaitForSeconds(Me, 1.5F)
		MyBase.transform.position = Me.startPosition
		MyBase.animator.SetTrigger("StartIntro")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Intro", 2, False, True)
		Me.state = MouseLevelCat.State.Idle
		Return
	End Function

	' Token: 0x060025CA RID: 9674 RVA: 0x001618F7 File Offset: 0x0015FCF7
	Public Sub EatMouse()
		Me.mouse.BeEaten()
		MyBase.StartCoroutine(Me.tail_cr())
	End Sub

	' Token: 0x060025CB RID: 9675 RVA: 0x00161914 File Offset: 0x0015FD14
	Public Sub StartWallBreak()
		Me.wallAnimator.SetTrigger("OnContinue")
		For Each gameObject As GameObject In Me.toDestroyOnWallBreakStart
			Global.UnityEngine.[Object].Destroy(gameObject)
		Next
	End Sub

	' Token: 0x060025CC RID: 9676 RVA: 0x00161958 File Offset: 0x0015FD58
	Public Sub EndWallBreak()
		Me.wallAnimator.SetTrigger("OnContinue")
		For Each gameObject As GameObject In Me.toDestroyOnWallBreakEnd
			Global.UnityEngine.[Object].Destroy(gameObject)
		Next
		Global.UnityEngine.[Object].Destroy(Me.foreground)
		Me.alternateForeground.SetActive(True)
	End Sub

	' Token: 0x060025CD RID: 9677 RVA: 0x001619B4 File Offset: 0x0015FDB4
	Private Iterator Function tail_cr() As IEnumerator
		While True
			Yield CupheadTime.WaitForSeconds(Me, Global.UnityEngine.Random.Range(0F, 0.75F))
			MyBase.animator.SetTrigger("SwitchTailDirection")
			Yield MyBase.animator.WaitForAnimationToStart(Me, "Idle_Left", 1, False)
			Yield CupheadTime.WaitForSeconds(Me, Global.UnityEngine.Random.Range(0F, 0.75F))
			MyBase.animator.SetTrigger("SwitchTailDirection")
			Yield MyBase.animator.WaitForAnimationToStart(Me, "Idle_Right", 1, False)
		End While
		Return
	End Function

	' Token: 0x060025CE RID: 9678 RVA: 0x001619D0 File Offset: 0x0015FDD0
	Private Sub BlinkMaybe()
		Me.blinks += 1
		If Me.blinks >= Me.maxBlinks Then
			Me.blinks = 0
			Me.maxBlinks = Global.UnityEngine.Random.Range(8, 16)
			Me.blinkOverlaySprite.enabled = True
			MyBase.animator.SetBool("Blinking", True)
		Else
			Me.blinkOverlaySprite.enabled = False
			MyBase.animator.SetBool("Blinking", False)
		End If
	End Sub

	' Token: 0x060025CF RID: 9679 RVA: 0x00161A50 File Offset: 0x0015FE50
	Public Sub StartClaw(left As Boolean)
		Me.state = MouseLevelCat.State.Claw
		MyBase.StartCoroutine(Me.claw_cr(left))
	End Sub

	' Token: 0x060025D0 RID: 9680 RVA: 0x00161A68 File Offset: 0x0015FE68
	Private Iterator Function claw_cr(left As Boolean) As IEnumerator
		Dim p As LevelProperties.Mouse.Claw = MyBase.properties.CurrentState.claw
		Dim paw As MouseLevelCatPaw = If((Not left), Me.rightPaw, Me.leftPaw)
		Dim totalPawAttackTime As Single = 0.584F + 2F * p.holdGroundTime
		Dim totalPawLeaveTime As Single = 0.584F * p.moveSpeed / p.leaveSpeed
		Dim headMoveBackTime As Single = p.holdGroundTime + totalPawLeaveTime + 0.417F
		MyBase.animator.SetTrigger(If((Not left), "StartClawRight", "StartClawLeft"))
		Yield MyBase.animator.WaitForAnimationToStart(Me, If((Not left), "Claw_Right_Start", "Claw_Left_Start"), 2, False)
		Yield CupheadTime.WaitForSeconds(Me, p.attackDelay)
		paw.Attack(p)
		MyBase.StartCoroutine(Me.spawnFallingObjects_cr(left))
		Yield MyBase.StartCoroutine(Me.tween_cr(Me.head.transform, Me.headStartPos, Me.headMoveTransform.localPosition, Quaternion.identity, Me.headMoveTransform.rotation, EaseUtils.EaseType.easeOutQuad, totalPawAttackTime))
		MyBase.StartCoroutine(Me.tween_cr(Me.head.transform, Me.headMoveTransform.localPosition, Me.headStartPos, Me.headMoveTransform.rotation, Quaternion.identity, EaseUtils.EaseType.easeInOutSine, headMoveBackTime))
		Yield CupheadTime.WaitForSeconds(Me, headMoveBackTime - 0.417F)
		MyBase.animator.SetTrigger("Continue")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, If((Not left), "Claw_Right_End", "Claw_Left_End"), 2, False, True)
		Yield CupheadTime.WaitForSeconds(Me, p.hesitateAfterAttack)
		Me.state = MouseLevelCat.State.Idle
		Return
	End Function

	' Token: 0x060025D1 RID: 9681 RVA: 0x00161A8C File Offset: 0x0015FE8C
	Private Iterator Function spawnFallingObjects_cr(left As Boolean) As IEnumerator
		Dim p As LevelProperties.Mouse.Claw = MyBase.properties.CurrentState.claw
		Dim paw As MouseLevelCatPaw = If((Not left), Me.rightPaw, Me.leftPaw)
		Yield paw.animator.WaitForAnimationToStart(Me, "Attack_Hit", False)
		Me.fallingObjectsIndex = (Me.fallingObjectsIndex + 1) Mod p.fallingObjectStrings.Length
		Dim pattern As String() = p.fallingObjectStrings(Me.fallingObjectsIndex).Split(New Char() { ","c })
		Dim waitTime As Single = 0F
		For Each instruction As String In pattern
			If instruction(0) = "D"c Then
				Parser.FloatTryParse(instruction.Substring(1), waitTime)
			Else
				Yield CupheadTime.WaitForSeconds(Me, waitTime)
				Dim positions As String() = instruction.Split(New Char() { "-"c })
				For Each text As String In positions
					Dim num As Single = 0F
					Parser.FloatTryParse(text, num)
					Me.fallingObjectPrefabs.RandomChoice().Create(num, p)
				Next
				waitTime = p.objectSpawnDelay
			End If
		Next
		Return
	End Function

	' Token: 0x060025D2 RID: 9682 RVA: 0x00161AAE File Offset: 0x0015FEAE
	Public Sub StartGhostMouse()
		Me.state = MouseLevelCat.State.GhostMouse
		MyBase.StartCoroutine(Me.jailHead_cr())
	End Sub

	' Token: 0x060025D3 RID: 9683 RVA: 0x00161AC4 File Offset: 0x0015FEC4
	Private Iterator Function jailHead_cr() As IEnumerator
		Dim ghostMice As MouseLevelGhostMouse() = If((Not MyBase.properties.CurrentState.ghostMouse.fourMice), Me.twoGhostMice, Me.fourGhostMice)
		Dim unspawnedGhosts As Boolean = False
		For Each mouseLevelGhostMouse As MouseLevelGhostMouse In ghostMice
			If mouseLevelGhostMouse.state = MouseLevelGhostMouse.State.Unspawned Then
				unspawnedGhosts = True
				Exit For
			End If
		Next
		If unspawnedGhosts Then
			MyBase.animator.SetTrigger("StartGhostMouse")
			Yield MyBase.animator.WaitForAnimationToStart(Me, "Jail_Loop", 2, False)
			Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.ghostMouse.jailDuration)
			MyBase.animator.SetTrigger("Continue")
			Yield MyBase.animator.WaitForAnimationToEnd(Me, "Jail_End", 2, False, True)
			Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.ghostMouse.hesitateAfterAttack)
		End If
		Me.state = MouseLevelCat.State.Idle
		Return
	End Function

	' Token: 0x060025D4 RID: 9684 RVA: 0x00161ADF File Offset: 0x0015FEDF
	Public Sub SpawnGhostMice()
		MyBase.StartCoroutine(Me.spawnGhostMice_cr())
	End Sub

	' Token: 0x060025D5 RID: 9685 RVA: 0x00161AF0 File Offset: 0x0015FEF0
	Private Iterator Function spawnGhostMice_cr() As IEnumerator
		Dim ghostMice As MouseLevelGhostMouse() = If((Not MyBase.properties.CurrentState.ghostMouse.fourMice), Me.twoGhostMice, Me.fourGhostMice)
		ghostMice.Shuffle()
		For Each mouse As MouseLevelGhostMouse In ghostMice
			If mouse.state = MouseLevelGhostMouse.State.Unspawned Then
				mouse.Spawn(MyBase.properties)
				Yield CupheadTime.WaitForSeconds(Me, 0.1F)
			End If
		Next
		While ghostMice(ghostMice.Length - 1).state = MouseLevelGhostMouse.State.Intro
			Yield Nothing
		End While
		If Not Me.alreadyManagingGhostMice Then
			MyBase.StartCoroutine(Me.manageGhostMice_cr())
		End If
		Return
	End Function

	' Token: 0x060025D6 RID: 9686 RVA: 0x00161B0C File Offset: 0x0015FF0C
	Private Iterator Function manageGhostMice_cr() As IEnumerator
		Me.alreadyManagingGhostMice = True
		Dim ghostMice As MouseLevelGhostMouse() = If((Not MyBase.properties.CurrentState.ghostMouse.fourMice), Me.twoGhostMice, Me.fourGhostMice)
		Dim shotsTillPinkAttack As Integer = MyBase.properties.CurrentState.ghostMouse.pinkBallRange.RandomInt()
		Dim anyMiceSpawned As Boolean = True
		While anyMiceSpawned
			ghostMice.Shuffle()
			anyMiceSpawned = False
			For Each mouse As MouseLevelGhostMouse In ghostMice
				If mouse.state <> MouseLevelGhostMouse.State.Unspawned AndAlso mouse.state <> MouseLevelGhostMouse.State.Dying Then
					anyMiceSpawned = True
				End If
				If mouse.state = MouseLevelGhostMouse.State.Idle Then
					shotsTillPinkAttack -= 1
					Dim pinkAttack As Boolean = False
					If shotsTillPinkAttack = 0 Then
						pinkAttack = True
						shotsTillPinkAttack = MyBase.properties.CurrentState.ghostMouse.pinkBallRange.RandomInt()
					End If
					mouse.Attack(pinkAttack)
					While mouse.state = MouseLevelGhostMouse.State.Attack
						Yield Nothing
					End While
					Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.ghostMouse.attackDelayRange.RandomFloat())
				End If
			Next
			Yield Nothing
		End While
		Me.alreadyManagingGhostMice = False
		Return
	End Function

	' Token: 0x060025D7 RID: 9687 RVA: 0x00161B28 File Offset: 0x0015FF28
	Public Sub OnBossDeath()
		Me.state = MouseLevelCat.State.Dying
		Me.StopAllCoroutines()
		Global.UnityEngine.[Object].Destroy(Me.leftPaw.gameObject)
		Global.UnityEngine.[Object].Destroy(Me.rightPaw.gameObject)
		Me.head.transform.localPosition = Me.headStartPos
		Me.head.transform.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(0F))
		MyBase.animator.SetTrigger("Die")
		Me.headFrontRenderer.sortingLayerName = "Enemies"
		Dim array As MouseLevelGhostMouse() = If((Not MyBase.properties.CurrentState.ghostMouse.fourMice), Me.twoGhostMice, Me.fourGhostMice)
		For Each mouseLevelGhostMouse As MouseLevelGhostMouse In array
			mouseLevelGhostMouse.Die()
		Next
	End Sub

	' Token: 0x060025D8 RID: 9688 RVA: 0x00161C18 File Offset: 0x00160018
	Private Iterator Function tween_cr(trans As Transform, startPos As Vector2, endPos As Vector2, startRotation As Quaternion, endRotation As Quaternion, ease As EaseUtils.EaseType, time As Single) As IEnumerator
		Dim t As Single = 0F
		trans.localPosition = startPos
		trans.localRotation = startRotation
		Dim accumulator As Single = 0F
		While t < time
			accumulator += CupheadTime.Delta
			While accumulator > 0.041666668F
				accumulator -= 0.041666668F
				Dim num As Single = EaseUtils.Ease(ease, 0F, 1F, t / time)
				trans.localPosition = Vector2.Lerp(startPos, endPos, num)
				trans.localRotation = Quaternion.Slerp(startRotation, endRotation, num)
				t += 0.041666668F
			End While
			Yield Nothing
		End While
		trans.localPosition = endPos
		trans.localRotation = endRotation
		Yield Nothing
		Return
	End Function

	' Token: 0x060025D9 RID: 9689 RVA: 0x00161C61 File Offset: 0x00160061
	Private Sub SoundCatIntro()
		AudioManager.Play("level_mouse_cat_intro")
	End Sub

	' Token: 0x060025DA RID: 9690 RVA: 0x00161C6D File Offset: 0x0016006D
	Private Sub SoundCatJailEnd()
		AudioManager.Play("level_mouse_cat_jail_end")
		Me.emitAudioFromObject.Add("level_mouse_cat_jail_end")
	End Sub

	' Token: 0x04002E46 RID: 11846
	Private Const EnemiesLayerName As String = "Enemies"

	' Token: 0x04002E47 RID: 11847
	Private Const BODY_LAYER As Integer = 0

	' Token: 0x04002E48 RID: 11848
	Private Const TAIL_LAYER As Integer = 1

	' Token: 0x04002E49 RID: 11849
	Private Const HEAD_LAYER As Integer = 2

	' Token: 0x04002E4B RID: 11851
	<SerializeField()>
	Private startPosition As Vector2

	' Token: 0x04002E4C RID: 11852
	<SerializeField()>
	Private mouse As MouseLevelBrokenCanMouse

	' Token: 0x04002E4D RID: 11853
	<SerializeField()>
	Private wallAnimator As Animator

	' Token: 0x04002E4E RID: 11854
	<SerializeField()>
	Private wallPlatform As LevelPlatform

	' Token: 0x04002E4F RID: 11855
	<SerializeField()>
	Private foreground As GameObject

	' Token: 0x04002E50 RID: 11856
	<SerializeField()>
	Private alternateForeground As GameObject

	' Token: 0x04002E51 RID: 11857
	<SerializeField()>
	Private toDestroyOnWallBreakStart As GameObject()

	' Token: 0x04002E52 RID: 11858
	<SerializeField()>
	Private toDestroyOnWallBreakEnd As GameObject()

	' Token: 0x04002E53 RID: 11859
	<SerializeField()>
	Private blinkOverlaySprite As SpriteRenderer

	' Token: 0x04002E54 RID: 11860
	<SerializeField()>
	Private head As Transform

	' Token: 0x04002E55 RID: 11861
	<SerializeField()>
	Private leftPaw As MouseLevelCatPaw

	' Token: 0x04002E56 RID: 11862
	<SerializeField()>
	Private rightPaw As MouseLevelCatPaw

	' Token: 0x04002E57 RID: 11863
	<SerializeField()>
	Private headMoveTransform As Transform

	' Token: 0x04002E58 RID: 11864
	<SerializeField()>
	Private fallingObjectPrefabs As MouseLevelFallingObject()

	' Token: 0x04002E59 RID: 11865
	<SerializeField()>
	Private twoGhostMice As MouseLevelGhostMouse()

	' Token: 0x04002E5A RID: 11866
	<SerializeField()>
	Private fourGhostMice As MouseLevelGhostMouse()

	' Token: 0x04002E5B RID: 11867
	<SerializeField()>
	Private headFrontRenderer As SpriteRenderer

	' Token: 0x04002E5C RID: 11868
	Private damageReceiver As DamageReceiver

	' Token: 0x04002E5D RID: 11869
	Private headStartPos As Vector2

	' Token: 0x04002E5E RID: 11870
	Private fallingObjectsIndex As Integer

	' Token: 0x04002E5F RID: 11871
	Private blinks As Integer

	' Token: 0x04002E60 RID: 11872
	Private maxBlinks As Integer = 8

	' Token: 0x04002E61 RID: 11873
	Private alreadyManagingGhostMice As Boolean

	' Token: 0x020006E7 RID: 1767
	Public Enum State
		' Token: 0x04002E63 RID: 11875
		Init
		' Token: 0x04002E64 RID: 11876
		Idle
		' Token: 0x04002E65 RID: 11877
		Claw
		' Token: 0x04002E66 RID: 11878
		GhostMouse
		' Token: 0x04002E67 RID: 11879
		Dying
	End Enum
End Class
