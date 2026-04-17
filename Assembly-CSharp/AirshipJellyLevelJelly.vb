Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020004D4 RID: 1236
Public Class AirshipJellyLevelJelly
	Inherits LevelProperties.AirshipJelly.Entity

	' Token: 0x06001511 RID: 5393 RVA: 0x000BD2DC File Offset: 0x000BB6DC
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
	End Sub

	' Token: 0x06001512 RID: 5394 RVA: 0x000BD308 File Offset: 0x000BB708
	Private Sub Start()
		Me.spriteRenderer = MyBase.GetComponent(Of SpriteRenderer)()
		Me.startColor = MyBase.GetComponent(Of SpriteRenderer)().color
		AddHandler Level.Current.OnLevelStartEvent, AddressOf Me.OnLevelStart
		CupheadLevelCamera.Current.StartFloat(25F, 3F)
	End Sub

	' Token: 0x06001513 RID: 5395 RVA: 0x000BD35C File Offset: 0x000BB75C
	Public Overrides Sub LevelInit(properties As LevelProperties.AirshipJelly)
		MyBase.LevelInit(properties)
		Me.knobSwitch = AirshipJellyLevelKnob.Create(Me)
		AddHandler Me.knobSwitch.OnActivate, AddressOf Me.OnKnobParry
		AddHandler Me.knobSwitch.OnPrePauseActivate, AddressOf Me.OnKnobPreParry
		Me.maxHealth = properties.CurrentHealth
		Me.defaultSpeed = properties.CurrentState.main.speed.min
		Me.speed = Me.defaultSpeed
		Me.damageDealer = New DamageDealer(1F, 0.3F, DamageDealer.DamageSource.Enemy, True, False, False)
	End Sub

	' Token: 0x06001514 RID: 5396 RVA: 0x000BD3F8 File Offset: 0x000BB7F8
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		MyBase.properties.DealDamage(info.damage)
		MyBase.StartCoroutine(Me.flash_cr())
		Me.GetNewSpeed()
		If MyBase.properties.CurrentHealth <= 0F AndAlso Me.state <> AirshipJellyLevelJelly.State.Dead Then
			Me.state = AirshipJellyLevelJelly.State.Dead
			MyBase.animator.Play("Death")
		End If
	End Sub

	' Token: 0x06001515 RID: 5397 RVA: 0x000BD461 File Offset: 0x000BB861
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06001516 RID: 5398 RVA: 0x000BD479 File Offset: 0x000BB879
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06001517 RID: 5399 RVA: 0x000BD49B File Offset: 0x000BB89B
	Private Sub OnLevelStart()
		Me.state = AirshipJellyLevelJelly.State.Running
		MyBase.StartCoroutine(Me.start_cr())
	End Sub

	' Token: 0x06001518 RID: 5400 RVA: 0x000BD4B4 File Offset: 0x000BB8B4
	Private Sub GetNewSpeed()
		Dim minMax As MinMax = MyBase.properties.CurrentState.main.speed
		Dim num As Single = MyBase.properties.CurrentHealth / Me.maxHealth
		Dim num2 As Single = 1F - num
		Me.speed = Me.defaultSpeed + minMax.max * num2
	End Sub

	' Token: 0x06001519 RID: 5401 RVA: 0x000BD508 File Offset: 0x000BB908
	Private Sub OnTurnComplete()
		MyBase.transform.SetScale(New Single?(-MyBase.transform.localScale.x), Nothing, Nothing)
	End Sub

	' Token: 0x0600151A RID: 5402 RVA: 0x000BD54B File Offset: 0x000BB94B
	Private Sub OnKnobParry()
		MyBase.StartCoroutine(Me.hurt_cr())
	End Sub

	' Token: 0x0600151B RID: 5403 RVA: 0x000BD55A File Offset: 0x000BB95A
	Private Sub OnKnobPreParry()
		AudioManager.Play("levels_airship_jelly_hit")
		Me.smashEffect.Create(Me.knobRoot.position)
		CupheadLevelCamera.Current.Shake(10F, 0.6F, False)
	End Sub

	' Token: 0x0600151C RID: 5404 RVA: 0x000BD592 File Offset: 0x000BB992
	Private Sub SfxWalk()
		AudioManager.Play("levels_airship_jelly_walk")
	End Sub

	' Token: 0x0600151D RID: 5405 RVA: 0x000BD59E File Offset: 0x000BB99E
	Private Sub ResetMove()
		If Me.moveCoroutine IsNot Nothing Then
			MyBase.StopCoroutine(Me.moveCoroutine)
			Me.moveCoroutine = Nothing
		End If
		Me.moveCoroutine = MyBase.StartCoroutine(Me.jelly_cr())
	End Sub

	' Token: 0x0600151E RID: 5406 RVA: 0x000BD5D0 File Offset: 0x000BB9D0
	Private Iterator Function start_cr() As IEnumerator
		MyBase.animator.SetTrigger("OnIntroComplete")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Intro_Transition", False, True)
		Me.ResetMove()
		Return
	End Function

	' Token: 0x0600151F RID: 5407 RVA: 0x000BD5EC File Offset: 0x000BB9EC
	Private Iterator Function jelly_cr() As IEnumerator
		While MyBase.properties Is Nothing
		End While
		Dim offset As Single = 100F
		While True
			Dim pos As Vector3 = MyBase.transform.position
			If Me.direction = AirshipJellyLevelJelly.Direction.Left Then
				While MyBase.transform.position.x > -640F + offset
					If Not Me.Moving Then
						Yield MyBase.StartCoroutine(Me.waitForMove_cr())
					End If
					pos.x = Mathf.MoveTowards(MyBase.transform.position.x, -640F + offset, Me.speed * CupheadTime.Delta)
					MyBase.transform.position = pos
					Yield Nothing
				End While
				MyBase.animator.SetTrigger("OnTurn")
				Me.direction = AirshipJellyLevelJelly.Direction.Right
			ElseIf Me.direction = AirshipJellyLevelJelly.Direction.Right Then
				While MyBase.transform.position.x < 640F - offset
					If Not Me.Moving Then
						Yield MyBase.StartCoroutine(Me.waitForMove_cr())
					End If
					pos.x = Mathf.MoveTowards(MyBase.transform.position.x, 640F - offset, Me.speed * CupheadTime.Delta)
					MyBase.transform.position = pos
					Yield Nothing
				End While
				MyBase.animator.SetTrigger("OnTurn")
				Me.direction = AirshipJellyLevelJelly.Direction.Left
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x17000317 RID: 791
	' (get) Token: 0x06001520 RID: 5408 RVA: 0x000BD607 File Offset: 0x000BBA07
	Private ReadOnly Property Moving As Boolean
		Get
			Return Me.state = AirshipJellyLevelJelly.State.Running
		End Get
	End Property

	' Token: 0x06001521 RID: 5409 RVA: 0x000BD614 File Offset: 0x000BBA14
	Private Iterator Function waitForMove_cr() As IEnumerator
		While Not Me.Moving
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06001522 RID: 5410 RVA: 0x000BD630 File Offset: 0x000BBA30
	Private Iterator Function hurt_cr() As IEnumerator
		MyBase.properties.DealDamage(MyBase.properties.CurrentState.main.parryDamage)
		Me.GetNewSpeed()
		Me.knobSprite.enabled = False
		Me.knobSwitch.enabled = False
		AudioManager.Play("levels_airship_jelly_hurt")
		If MyBase.properties.CurrentHealth <= 0F Then
			Me.state = AirshipJellyLevelJelly.State.Dead
			MyBase.animator.Play("Death")
		Else
			MyBase.animator.SetTrigger("OnHurt")
			Dim lastState As AirshipJellyLevelJelly.State = Me.state
			Me.state = AirshipJellyLevelJelly.State.Hurt
			Me.ResetMove()
			Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.main.hurtDelay)
			Me.state = lastState
			MyBase.animator.SetTrigger("OnHurtComplete")
			Yield MyBase.animator.WaitForAnimationToEnd(Me, "Hurt_Loop", False, True)
			Me.state = AirshipJellyLevelJelly.State.Running
			MyBase.StartCoroutine(Me.enableKnob_cr())
		End If
		Return
	End Function

	' Token: 0x06001523 RID: 5411 RVA: 0x000BD64C File Offset: 0x000BBA4C
	Private Iterator Function enableKnob_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.main.orbDelay)
		Me.knobSprite.enabled = True
		Me.knobSwitch.enabled = True
		Return
	End Function

	' Token: 0x06001524 RID: 5412 RVA: 0x000BD668 File Offset: 0x000BBA68
	Private Sub SetColor(t As Single)
		Dim color As Color = Color.Lerp(Me.flashColor, Color.black, t)
		Me.spriteRenderer.color = color
	End Sub

	' Token: 0x06001525 RID: 5413 RVA: 0x000BD694 File Offset: 0x000BBA94
	Private Iterator Function flash_cr() As IEnumerator
		Dim t As Single = 0F
		Dim time As Single = 0.15F
		While t < time
			Dim val As Single = t / time
			Me.SetColor(val)
			t += Time.deltaTime
			Yield Nothing
		End While
		MyBase.GetComponent(Of SpriteRenderer)().color = Me.startColor
		Return
	End Function

	' Token: 0x04001E73 RID: 7795
	Private startColor As Color

	' Token: 0x04001E74 RID: 7796
	Private flashColor As Color = Color.red

	' Token: 0x04001E75 RID: 7797
	Public knobRoot As Transform

	' Token: 0x04001E76 RID: 7798
	<SerializeField()>
	Private knobSprite As SpriteRenderer

	' Token: 0x04001E77 RID: 7799
	<Space(10F)>
	<SerializeField()>
	Private smashEffect As Effect

	' Token: 0x04001E78 RID: 7800
	Private speed As Single

	' Token: 0x04001E79 RID: 7801
	Private defaultSpeed As Single

	' Token: 0x04001E7A RID: 7802
	Private maxHealth As Single

	' Token: 0x04001E7B RID: 7803
	Private spriteRenderer As SpriteRenderer

	' Token: 0x04001E7C RID: 7804
	Private damageDealer As DamageDealer

	' Token: 0x04001E7D RID: 7805
	Private damageReceiver As DamageReceiver

	' Token: 0x04001E7E RID: 7806
	Private knobSwitch As AirshipJellyLevelKnob

	' Token: 0x04001E7F RID: 7807
	Private state As AirshipJellyLevelJelly.State

	' Token: 0x04001E80 RID: 7808
	Private direction As AirshipJellyLevelJelly.Direction = AirshipJellyLevelJelly.Direction.Left

	' Token: 0x04001E81 RID: 7809
	Private Const MIN_X As Single = -550F

	' Token: 0x04001E82 RID: 7810
	Private Const MAX_X As Single = 550F

	' Token: 0x04001E83 RID: 7811
	Private moveCoroutine As Coroutine

	' Token: 0x020004D5 RID: 1237
	Public Enum State
		' Token: 0x04001E85 RID: 7813
		Init
		' Token: 0x04001E86 RID: 7814
		Running
		' Token: 0x04001E87 RID: 7815
		Hurt
		' Token: 0x04001E88 RID: 7816
		Dead
	End Enum

	' Token: 0x020004D6 RID: 1238
	Public Enum Direction
		' Token: 0x04001E8A RID: 7818
		Right
		' Token: 0x04001E8B RID: 7819
		Left
	End Enum
End Class
