Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020005D8 RID: 1496
Public Class DicePalacePachinkoLevelPachinko
	Inherits LevelProperties.DicePalacePachinko.Entity

	' Token: 0x1700036E RID: 878
	' (get) Token: 0x06001D7E RID: 7550 RVA: 0x0010EC2D File Offset: 0x0010D02D
	' (set) Token: 0x06001D7F RID: 7551 RVA: 0x0010EC35 File Offset: 0x0010D035
	Public Property attacking As Boolean

	' Token: 0x06001D80 RID: 7552 RVA: 0x0010EC40 File Offset: 0x0010D040
	Protected Overrides Sub Awake()
		Me.reversing = False
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		AddHandler Level.Current.OnWinEvent, AddressOf Me.OnDeath
		MyBase.Awake()
	End Sub

	' Token: 0x06001D81 RID: 7553 RVA: 0x0010EC9E File Offset: 0x0010D09E
	Private Sub Update()
		Me.damageDealer.Update()
	End Sub

	' Token: 0x06001D82 RID: 7554 RVA: 0x0010ECAB File Offset: 0x0010D0AB
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.pct = 1F - MyBase.properties.CurrentHealth / Me.initialHP
		MyBase.properties.DealDamage(info.damage)
	End Sub

	' Token: 0x06001D83 RID: 7555 RVA: 0x0010ECDC File Offset: 0x0010D0DC
	Public Overrides Sub LevelInit(properties As LevelProperties.DicePalacePachinko)
		AddHandler Level.Current.OnIntroEvent, AddressOf Me.OnIntroEnd
		MyBase.LevelInit(properties)
		Me.attacking = False
		Me.direction = 1
		Me.pct = 0F
		Me.initialHP = properties.CurrentHealth
		Me.baseSpeed = properties.CurrentState.boss.movementSpeed.min
		MyBase.StartCoroutine(Me.intro_cr())
	End Sub

	' Token: 0x06001D84 RID: 7556 RVA: 0x0010ED54 File Offset: 0x0010D154
	Private Iterator Function intro_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1.2F)
		MyBase.animator.SetTrigger("Continue")
		AudioManager.Play("dice_palace_pachinko_intro")
		Me.emitAudioFromObject.Add("dice_palace_pachinko_intro")
		Yield Nothing
		Return
	End Function

	' Token: 0x06001D85 RID: 7557 RVA: 0x0010ED6F File Offset: 0x0010D16F
	Private Sub OnIntroEnd()
		MyBase.StartCoroutine(Me.move_cr())
		MyBase.StartCoroutine(Me.attack_cr())
		MyBase.StartCoroutine(Me.check_position_cr())
	End Sub

	' Token: 0x06001D86 RID: 7558 RVA: 0x0010ED98 File Offset: 0x0010D198
	Protected Overridable Function hitPauseCoefficient() As Single
		Return If((Not MyBase.GetComponent(Of DamageReceiver)().IsHitPaused), 1F, 0F)
	End Function

	' Token: 0x06001D87 RID: 7559 RVA: 0x0010EDBC File Offset: 0x0010D1BC
	Private Iterator Function move_cr() As IEnumerator
		AudioManager.PlayLoop("dice_palace_pachinko_movement_loop")
		Me.emitAudioFromObject.Add("dice_palace_pachinko_movement_loop")
		While True
			Dim speed As Single = Me.baseSpeed + (MyBase.properties.CurrentState.boss.movementSpeed.max - MyBase.properties.CurrentState.boss.movementSpeed.min) * Me.pct
			MyBase.transform.position += Vector3.right * speed * CSng(Me.direction) * CupheadTime.Delta * Me.hitPauseCoefficient()
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06001D88 RID: 7560 RVA: 0x0010EDD8 File Offset: 0x0010D1D8
	Private Iterator Function check_position_cr() As IEnumerator
		While True
			If(MyBase.transform.position.x < -640F + MyBase.properties.CurrentState.boss.leftBoundaryOffset AndAlso Me.direction = -1) OrElse (MyBase.transform.position.x > 640F - MyBase.properties.CurrentState.boss.rightBoundaryOffset AndAlso Me.direction = 1) Then
				MyBase.StartCoroutine(Me.reverse_cr())
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06001D89 RID: 7561 RVA: 0x0010EDF4 File Offset: 0x0010D1F4
	Private Iterator Function attack_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.boss.initialAttackDelay)
		While True
			MyBase.StartCoroutine(Me.lights_cr())
			MyBase.animator.SetTrigger("OnAttack")
			Yield MyBase.animator.WaitForAnimationToEnd(Me, "Attack_Warning_Start", False, True)
			Me.BeamWarning()
			Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.boss.warningDuration)
			MyBase.animator.SetTrigger("Continue")
			AudioManager.Play("dice_palace_pachinko_warning_trans")
			Me.emitAudioFromObject.Add("dice_palace_pachinko_warning_trans")
			Me.attacking = True
			Me.BeamOn()
			Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.boss.beamDuration)
			Me.BeamOff()
			Me.attacking = False
			MyBase.animator.SetTrigger("OnEnd")
			AudioManager.Play("dice_palace_pachinko_trans_out")
			Me.emitAudioFromObject.Add("dice_palace_pachinko_trans_out")
			Yield MyBase.animator.WaitForAnimationToEnd(Me, "Attack_Trans_Out", False, True)
			Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.boss.attackDelay.RandomFloat())
		End While
		Return
	End Function

	' Token: 0x06001D8A RID: 7562 RVA: 0x0010EE0F File Offset: 0x0010D20F
	Private Sub BeamWarning()
		Me.beam.SetActive(True)
		Me.beam.GetComponent(Of Collider2D)().enabled = False
		Me.beam.GetComponent(Of SpriteRenderer)().sprite = Me.beamSprites(0)
	End Sub

	' Token: 0x06001D8B RID: 7563 RVA: 0x0010EE46 File Offset: 0x0010D246
	Private Sub BeamOn()
		Me.beam.SetActive(True)
		Me.beam.GetComponent(Of Collider2D)().enabled = True
		Me.beam.GetComponent(Of SpriteRenderer)().sprite = Me.beamSprites(1)
	End Sub

	' Token: 0x06001D8C RID: 7564 RVA: 0x0010EE7D File Offset: 0x0010D27D
	Private Sub BeamOff()
		Me.beam.SetActive(False)
		Me.beam.GetComponent(Of Collider2D)().enabled = False
	End Sub

	' Token: 0x06001D8D RID: 7565 RVA: 0x0010EE9C File Offset: 0x0010D29C
	Private Iterator Function lights_cr() As IEnumerator
		Dim fastSpeed As Single = 6F
		Dim fadeTime As Single = 0.01F
		For Each light As Transform In Me.lights
			light.GetComponent(Of SpriteRenderer)().color = New Color(1F, 1F, 1F, 1F)
			Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.boss.warningDuration / CSng(Me.lights.Length))
		Next
		Dim fadingOut As Boolean = False
		While Not Me.attacking
			Yield Nothing
		End While
		Me.fire.speed = fastSpeed
		While Me.attacking
			For Each transform As Transform In Me.lights
				If fadingOut Then
					transform.GetComponent(Of SpriteRenderer)().color = New Color(1F, 1F, 1F, 0F)
				Else
					transform.GetComponent(Of SpriteRenderer)().color = New Color(1F, 1F, 1F, 1F)
				End If
			Next
			fadingOut = Not fadingOut
			Yield CupheadTime.WaitForSeconds(Me, fadeTime)
			Yield Nothing
		End While
		MyBase.StartCoroutine(Me.fire_speed_cr(fastSpeed))
		For Each transform2 As Transform In Me.lights
			transform2.GetComponent(Of SpriteRenderer)().color = New Color(1F, 1F, 1F, 1F)
		Next
		For Each light2 As Transform In Me.lights
			light2.GetComponent(Of SpriteRenderer)().color = New Color(1F, 1F, 1F, 0F)
			Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.boss.warningDuration / CSng(Me.lights.Length))
		Next
		Yield Nothing
		Return
	End Function

	' Token: 0x06001D8E RID: 7566 RVA: 0x0010EEB8 File Offset: 0x0010D2B8
	Private Iterator Function fire_speed_cr(fastSpeed As Single) As IEnumerator
		While fastSpeed > 0F
			fastSpeed -= 0.1F
			Me.fire.speed = fastSpeed
			Yield Nothing
		End While
		Yield Nothing
		Return
	End Function

	' Token: 0x06001D8F RID: 7567 RVA: 0x0010EEDC File Offset: 0x0010D2DC
	Private Iterator Function reverse_cr() As IEnumerator
		If Not Me.reversing Then
			Me.reversing = True
			Me.direction *= -1
			Yield CupheadTime.WaitForSeconds(Me, 0.1F)
			Me.reversing = False
		End If
		Yield Nothing
		Return
	End Function

	' Token: 0x06001D90 RID: 7568 RVA: 0x0010EEF7 File Offset: 0x0010D2F7
	Private Sub OnDeath()
		AudioManager.[Stop]("dice_palace_pachinko_movement_loop")
		AudioManager.Play("dice_palace_pachinko_death")
		Me.StopAllCoroutines()
		MyBase.GetComponent(Of Collider2D)().enabled = False
		MyBase.animator.SetTrigger("OnDeath")
	End Sub

	' Token: 0x06001D91 RID: 7569 RVA: 0x0010EF2F File Offset: 0x0010D32F
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		Me.damageDealer.DealDamage(hit)
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x06001D92 RID: 7570 RVA: 0x0010EF46 File Offset: 0x0010D346
	Protected Overrides Sub OnDestroy()
		Me.StopAllCoroutines()
		MyBase.OnDestroy()
	End Sub

	' Token: 0x04002669 RID: 9833
	<SerializeField()>
	Private fire As Animator

	' Token: 0x0400266A RID: 9834
	<SerializeField()>
	Private lights As Transform()

	' Token: 0x0400266B RID: 9835
	<SerializeField()>
	Private beamSprites As Sprite()

	' Token: 0x0400266C RID: 9836
	<SerializeField()>
	Private beam As GameObject

	' Token: 0x0400266D RID: 9837
	Private reversing As Boolean

	' Token: 0x0400266E RID: 9838
	Private direction As Integer

	' Token: 0x0400266F RID: 9839
	Private baseSpeed As Single

	' Token: 0x04002670 RID: 9840
	Private pct As Single

	' Token: 0x04002671 RID: 9841
	Private initialHP As Single

	' Token: 0x04002672 RID: 9842
	Private damageDealer As DamageDealer

	' Token: 0x04002673 RID: 9843
	Private damageReceiver As DamageReceiver
End Class
