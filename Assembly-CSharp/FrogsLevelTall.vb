Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x020006BE RID: 1726
Public Class FrogsLevelTall
	Inherits LevelProperties.Frogs.Entity

	' Token: 0x170003B4 RID: 948
	' (get) Token: 0x0600249D RID: 9373 RVA: 0x001574EA File Offset: 0x001558EA
	' (set) Token: 0x0600249E RID: 9374 RVA: 0x001574F2 File Offset: 0x001558F2
	Public Property state As FrogsLevelTall.State

	' Token: 0x0600249F RID: 9375 RVA: 0x001574FC File Offset: 0x001558FC
	Protected Overrides Sub Awake()
		MyBase.Awake()
		FrogsLevelTall.Current = Me
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		Me.damageDealer = New DamageDealer(1F, 0.3F, DamageDealer.DamageSource.Enemy, True, False, False)
	End Sub

	' Token: 0x060024A0 RID: 9376 RVA: 0x00157551 File Offset: 0x00155951
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		If FrogsLevelTall.Current Is Me Then
			FrogsLevelTall.Current = Nothing
		End If
		Me.fireflyPrefab = Nothing
	End Sub

	' Token: 0x060024A1 RID: 9377 RVA: 0x00157576 File Offset: 0x00155976
	Private Sub Start()
		AddHandler Level.Current.OnIntroEvent, AddressOf Me.OnLevelIntro
	End Sub

	' Token: 0x060024A2 RID: 9378 RVA: 0x0015758E File Offset: 0x0015598E
	Private Sub Update()
		Me.damageDealer.Update()
	End Sub

	' Token: 0x060024A3 RID: 9379 RVA: 0x0015759B File Offset: 0x0015599B
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x060024A4 RID: 9380 RVA: 0x001575B9 File Offset: 0x001559B9
	Public Overrides Sub LevelInit(properties As LevelProperties.Frogs)
		MyBase.LevelInit(properties)
		AddHandler properties.OnBossDeath, AddressOf Me.OnBossDeath
	End Sub

	' Token: 0x060024A5 RID: 9381 RVA: 0x001575D4 File Offset: 0x001559D4
	Public Sub AddFanForce(player As AbstractPlayerController)
		If Me.fanForce Is Nothing Then
			Me.fanForce = New LevelPlayerMotor.VelocityManager.Force(LevelPlayerMotor.VelocityManager.Force.Type.All, 0F)
			Me.fanForce.enabled = False
		End If
		player.GetComponent(Of LevelPlayerMotor)().AddForce(Me.fanForce)
	End Sub

	' Token: 0x060024A6 RID: 9382 RVA: 0x0015760F File Offset: 0x00155A0F
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		If FrogsLevel.FINAL_FORM Then
			Return
		End If
		If MyBase.properties.CurrentState.stateName <> LevelProperties.Frogs.States.Main Then
			MyBase.properties.DealDamage(info.damage)
		End If
	End Sub

	' Token: 0x060024A7 RID: 9383 RVA: 0x00157642 File Offset: 0x00155A42
	Private Sub OnBossDeath()
		Me.StopAllCoroutines()
		Me.fanForce.enabled = False
		MyBase.animator.SetTrigger("OnDeath")
		AudioManager.Play("level_frogs_tall_death")
	End Sub

	' Token: 0x060024A8 RID: 9384 RVA: 0x00157670 File Offset: 0x00155A70
	Private Sub OnLevelIntro()
		AudioManager.Play("level_frogs_tall_intro_full")
		MyBase.animator.Play("Intro")
	End Sub

	' Token: 0x060024A9 RID: 9385 RVA: 0x0015768C File Offset: 0x00155A8C
	Public Sub StartFan()
		If Me.state <> FrogsLevelTall.State.Idle AndAlso Me.state <> FrogsLevelTall.State.Complete Then
			Return
		End If
		Me.state = FrogsLevelTall.State.Fan
		MyBase.StartCoroutine(Me.fan_cr())
	End Sub

	' Token: 0x060024AA RID: 9386 RVA: 0x001576C0 File Offset: 0x00155AC0
	Private Iterator Function fan_cr() As IEnumerator
		Dim p As LevelProperties.Frogs.TallFan = MyBase.properties.CurrentState.tallFan
		Dim time As Single = p.duration
		MyBase.animator.Play("Fan")
		Yield CupheadTime.WaitForSeconds(Me, 0.5F)
		AudioManager.Play("level_frogs_tall_fan_start")
		Me.emitAudioFromObject.Add("level_frogs_tall_fan_start")
		MyBase.StartCoroutine(Me.fanAccelerate_cr(p))
		Yield CupheadTime.WaitForSeconds(Me, 2F)
		AudioManager.PlayLoop("level_frogs_tall_fan_attack_loop")
		Me.emitAudioFromObject.Add("level_frogs_tall_fan_attack_loop")
		If Me.firstFan Then
			Me.firstFan = False
			Dim startX As Single = MyBase.transform.position.x
			Yield CupheadTime.WaitForSeconds(Me, 0.25F)
			Dim t As Single = 0F
			While t < 0.5F
				Dim val As Single = t / 0.5F
				Dim x As Single = EaseUtils.Ease(EaseUtils.EaseType.easeInSine, startX, startX + 60F, val)
				MyBase.transform.SetPosition(New Single?(x), Nothing, Nothing)
				t += CupheadTime.Delta
				Yield Nothing
			End While
			MyBase.transform.SetPosition(New Single?(startX + 60F), Nothing, Nothing)
			Yield CupheadTime.WaitForSeconds(Me, p.duration.RandomFloat() - 0.75F)
		Else
			Yield CupheadTime.WaitForSeconds(Me, p.duration.RandomFloat())
		End If
		Yield CupheadTime.WaitForSeconds(Me, time)
		AudioManager.Play("level_frogs_tall_fan_end")
		Me.emitAudioFromObject.Add("level_frogs_tall_fan_end")
		Yield CupheadTime.WaitForSeconds(Me, 0.2F)
		AudioManager.[Stop]("level_frogs_tall_fan_attack_loop")
		MyBase.animator.SetTrigger("OnFanEnd")
		MyBase.StartCoroutine(Me.fanDecelerate_cr(p))
		Yield CupheadTime.WaitForSeconds(Me, 0.5F)
		Me.state = FrogsLevelTall.State.Complete
		Return
	End Function

	' Token: 0x060024AB RID: 9387 RVA: 0x001576DC File Offset: 0x00155ADC
	Private Iterator Function fanAccelerate_cr(p As LevelProperties.Frogs.TallFan) As IEnumerator
		Me.fanForce.enabled = True
		Yield MyBase.StartCoroutine(Me.fanPowerTween_cr(0F, p.power, CSng(p.accelerationTime)))
		Return
	End Function

	' Token: 0x060024AC RID: 9388 RVA: 0x00157700 File Offset: 0x00155B00
	Private Iterator Function fanDecelerate_cr(p As LevelProperties.Frogs.TallFan) As IEnumerator
		Yield MyBase.StartCoroutine(Me.fanPowerTween_cr(p.power, 0F, 0.75F))
		Me.fanForce.enabled = False
		Return
	End Function

	' Token: 0x060024AD RID: 9389 RVA: 0x00157724 File Offset: 0x00155B24
	Private Iterator Function fanPowerTween_cr(start As Single, [end] As Single, time As Single) As IEnumerator
		Me.fanForce.value = start
		Dim t As Single = 0F
		While t < time
			Dim val As Single = t / time
			Me.fanForce.value = Mathf.Lerp(start, [end], val)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		Me.fanForce.value = [end]
		Return
	End Function

	' Token: 0x060024AE RID: 9390 RVA: 0x00157754 File Offset: 0x00155B54
	Public Sub StartFireflies()
		Me.layer = 0
		If Me.state <> FrogsLevelTall.State.Idle AndAlso Me.state <> FrogsLevelTall.State.Complete Then
			Return
		End If
		Me.state = FrogsLevelTall.State.Fireflies
		Me.fireflyCount = 0
		MyBase.animator.SetBool("EndFirefly", False)
		MyBase.StartCoroutine(Me.fireflies_cr())
	End Sub

	' Token: 0x060024AF RID: 9391 RVA: 0x001577B0 File Offset: 0x00155BB0
	Private Sub ResetFireflyRoots()
		Me.tempRoots = New List(Of FrogsLevelTallFireflyRoot)(Me.fireflyRoots)
	End Sub

	' Token: 0x060024B0 RID: 9392 RVA: 0x001577C4 File Offset: 0x00155BC4
	Private Sub ShootFirefly()
		AudioManager.Play("level_frogs_tall_spit_shoot")
		Me.emitAudioFromObject.Add("level_frogs_tall_spit_shoot")
		Dim frogsLevelTallFireflyRoot As FrogsLevelTallFireflyRoot = Me.tempRoots(Global.UnityEngine.Random.Range(0, Me.tempRoots.Count))
		Me.tempRoots.Remove(frogsLevelTallFireflyRoot)
		Dim vector As Vector2 = frogsLevelTallFireflyRoot.transform.position
		Dim vector2 As Vector2 = vector
		Dim vector3 As Vector2 = New Vector2(Global.UnityEngine.Random.value * CSng(If((Not Rand.Bool()), (-1), 1)), Global.UnityEngine.Random.value * CSng(If((Not Rand.Bool()), (-1), 1)))
		vector = vector2 + vector3.normalized * frogsLevelTallFireflyRoot.radius * Global.UnityEngine.Random.value
		Dim frogsLevelTallFirefly As FrogsLevelTallFirefly = Me.fireflyPrefab
		Dim vector4 As Vector2 = Me.spitRoot.position
		Dim vector5 As Vector2 = vector
		Dim speed As Single = Me.fireflyProperties.speed
		Dim hp As Integer = Me.fireflyProperties.hp
		Dim followDelay As Single = Me.fireflyProperties.followDelay
		Dim followTime As Single = Me.fireflyProperties.followTime
		Dim followDistance As Single = Me.fireflyProperties.followDistance
		Dim invincibleDuration As Single = Me.fireflyProperties.invincibleDuration
		Dim [next] As AbstractPlayerController = PlayerManager.GetNext()
		Dim num As Integer = Me.layer
		Dim num2 As Integer = num
		Me.layer = num + 1
		frogsLevelTallFirefly.Create(vector4, vector5, speed, hp, followDelay, followTime, followDistance, invincibleDuration, [next], num2)
		Me.fireflyCount -= 1
	End Sub

	' Token: 0x060024B1 RID: 9393 RVA: 0x00157904 File Offset: 0x00155D04
	Private Iterator Function fireflies_cr() As IEnumerator
		Me.fireflyProperties = MyBase.properties.CurrentState.tallFireflies
		Dim patternString As String = Me.fireflyProperties.patterns(Global.UnityEngine.Random.Range(0, Me.fireflyProperties.patterns.Length))
		Dim pattern As KeyValue() = KeyValue.ListFromString(patternString, New Char() { "S"c, "D"c })
		MyBase.animator.SetTrigger("OnFirefly")
		Yield CupheadTime.WaitForSeconds(Me, 2F)
		For i As Integer = 0 To pattern.Length - 1
			If pattern(i).key = "S" Then
				Me.ResetFireflyRoots()
				Me.fireflyCount = CInt(pattern(i).value)
				MyBase.animator.SetInteger("FireflyCount", Me.fireflyCount)
				MyBase.animator.SetTrigger("OnFireflyStart")
				MyBase.animator.SetBool("EndFirefly", i >= pattern.Length - 1)
				While Me.fireflyCount > 0
					MyBase.animator.SetInteger("FireflyCount", Me.fireflyCount)
					Yield Nothing
				End While
				MyBase.animator.SetInteger("FireflyCount", Me.fireflyCount)
			Else
				Yield CupheadTime.WaitForSeconds(Me, pattern(i).value)
			End If
		Next
		Yield CupheadTime.WaitForSeconds(Me, Me.fireflyProperties.hesitate)
		Me.state = FrogsLevelTall.State.Complete
		Return
	End Function

	' Token: 0x060024B2 RID: 9394 RVA: 0x0015791F File Offset: 0x00155D1F
	Private Sub MorphSFX()
		AudioManager.Play("level_frogs_tall_morph_end")
		Me.emitAudioFromObject.Add("level_frogs_tall_morph_end")
	End Sub

	' Token: 0x060024B3 RID: 9395 RVA: 0x0015793B File Offset: 0x00155D3B
	Public Sub StartMorph()
		Me.StopAllCoroutines()
		Me.fanForce.value = 0F
		Me.fanForce.enabled = False
		Me.state = FrogsLevelTall.State.Morphing
		MyBase.animator.Play("Morph")
	End Sub

	' Token: 0x060024B4 RID: 9396 RVA: 0x00157976 File Offset: 0x00155D76
	Public Sub ContinueMorph()
		MyBase.StartCoroutine(Me.morph_cr())
	End Sub

	' Token: 0x060024B5 RID: 9397 RVA: 0x00157988 File Offset: 0x00155D88
	Private Iterator Function morph_cr() As IEnumerator
		MyBase.animator.SetTrigger("OnMorphContinue")
		Dim start As Vector2 = MyBase.transform.position
		Dim [end] As Vector2 = New Vector2(631F, -314F)
		Dim t As Single = 0F
		While t < 1F
			Dim val As Single = t / 1F
			Dim x As Single = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, start.x, [end].x, val)
			Dim y As Single = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, start.y, [end].y, val)
			MyBase.transform.SetPosition(New Single?(x), New Single?(y), Nothing)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		MyBase.transform.position = [end]
		Me.state = FrogsLevelTall.State.Morphed
		Return
	End Function

	' Token: 0x060024B6 RID: 9398 RVA: 0x001579A4 File Offset: 0x00155DA4
	Private Sub OnMorphAnimationComplete()
		FrogsLevelMorphed.Current.Enable(FrogsLevel.DEMON_TRIGGERED)
		CupheadLevelCamera.Current.Shake(20F, 0.6F, False)
		RemoveHandler MyBase.properties.OnBossDeath, AddressOf Me.OnBossDeath
		MyBase.gameObject.SetActive(False)
	End Sub

	' Token: 0x04002D48 RID: 11592
	Public Shared Current As FrogsLevelTall

	' Token: 0x04002D49 RID: 11593
	<SerializeField()>
	Private fireflyPrefab As FrogsLevelTallFirefly

	' Token: 0x04002D4A RID: 11594
	<SerializeField()>
	Private fireflyRoots As FrogsLevelTallFireflyRoot()

	' Token: 0x04002D4B RID: 11595
	<SerializeField()>
	Private spitRoot As Transform

	' Token: 0x04002D4C RID: 11596
	<Space(10F)>
	Public shortMorphRoot As Transform

	' Token: 0x04002D4E RID: 11598
	Private fanForce As LevelPlayerMotor.VelocityManager.Force

	' Token: 0x04002D4F RID: 11599
	Private damageReceiver As DamageReceiver

	' Token: 0x04002D50 RID: 11600
	Private damageDealer As DamageDealer

	' Token: 0x04002D51 RID: 11601
	Private layer As Integer

	' Token: 0x04002D52 RID: 11602
	Private Const FAN_START_TIME As Single = 2F

	' Token: 0x04002D53 RID: 11603
	Private Const FAN_END_TIME As Single = 0.5F

	' Token: 0x04002D54 RID: 11604
	Private Const FIRST_FAN_MOVE_OFFSET As Single = 60F

	' Token: 0x04002D55 RID: 11605
	Private Const FAN_DECELERATE_TIME As Single = 0.75F

	' Token: 0x04002D56 RID: 11606
	Private firstFan As Boolean = True

	' Token: 0x04002D57 RID: 11607
	Private fireflyCount As Integer

	' Token: 0x04002D58 RID: 11608
	Private tempRoots As List(Of FrogsLevelTallFireflyRoot)

	' Token: 0x04002D59 RID: 11609
	Private fireflyProperties As LevelProperties.Frogs.TallFireflies

	' Token: 0x04002D5A RID: 11610
	Private Const MORPH_MOVE_TIME As Single = 1F

	' Token: 0x020006BF RID: 1727
	Public Enum State
		' Token: 0x04002D5C RID: 11612
		Idle
		' Token: 0x04002D5D RID: 11613
		Fan
		' Token: 0x04002D5E RID: 11614
		Fireflies
		' Token: 0x04002D5F RID: 11615
		Morphing
		' Token: 0x04002D60 RID: 11616
		Complete = 1000000
		' Token: 0x04002D61 RID: 11617
		Morphed
	End Enum
End Class
