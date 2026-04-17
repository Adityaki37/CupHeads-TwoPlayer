Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020006C0 RID: 1728
Public Class FrogsLevelTallFirefly
	Inherits AbstractProjectile

	' Token: 0x170003B5 RID: 949
	' (get) Token: 0x060024B8 RID: 9400 RVA: 0x00158578 File Offset: 0x00156978
	Protected Overrides ReadOnly Property DestroyLifetime As Single
		Get
			Return 10000000F
		End Get
	End Property

	' Token: 0x060024B9 RID: 9401 RVA: 0x00158580 File Offset: 0x00156980
	Public Function Create(pos As Vector2, target As Vector2, speed As Single, hp As Integer, followDelay As Single, followTime As Single, followDistance As Single, invincibleDuration As Single, player As AbstractPlayerController, layer As Integer) As FrogsLevelTallFirefly
		Dim frogsLevelTallFirefly As FrogsLevelTallFirefly = TryCast(Me.Create(pos), FrogsLevelTallFirefly)
		frogsLevelTallFirefly.Health = hp
		frogsLevelTallFirefly.Speed = speed
		frogsLevelTallFirefly.DamagesType.OnlyPlayer()
		frogsLevelTallFirefly.CollisionDeath.OnlyPlayer()
		frogsLevelTallFirefly.CollisionDeath.PlayerProjectiles = True
		frogsLevelTallFirefly.Init(pos, target, followDelay, followTime, followDistance, player, layer, invincibleDuration)
		frogsLevelTallFirefly.DestroyDistance = 10000000F
		Return frogsLevelTallFirefly
	End Function

	' Token: 0x060024BA RID: 9402 RVA: 0x001585EC File Offset: 0x001569EC
	Protected Overrides Sub Awake()
		MyBase.Awake()
		If Level.Current Is Nothing OrElse Not Level.Current.Started Then
			Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		End If
	End Sub

	' Token: 0x060024BB RID: 9403 RVA: 0x00158620 File Offset: 0x00156A20
	Private Sub Init(pos As Vector2, target As Vector2, delay As Single, followTime As Single, followDistance As Single, player As AbstractPlayerController, layer As Integer, invincibleDuration As Single)
		MyBase.transform.position = pos
		Me.target = target
		Me.followDelay = delay
		Me.followTime = followTime
		Me.followDistance = followDistance
		Me.currentHp = CSng(Me.Health)
		Me.player = player
		Me.sprite.sortingOrder = layer
		Me.invincibleDuration = invincibleDuration
		MyBase.GetComponent(Of CircleCollider2D)().enabled = False
		MyBase.StartCoroutine(Me.firefly_cr())
	End Sub

	' Token: 0x060024BC RID: 9404 RVA: 0x001586A0 File Offset: 0x00156AA0
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.damageDealer.SetDamageFlags(True, False, False)
		Me.damageDealer.SetDamage(1F)
		Me.damageDealer.SetDamageSource(DamageDealer.DamageSource.Enemy)
		Me.damageDealer.SetRate(0.3F)
		Dim component As DamageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler component.OnDamageTaken, AddressOf Me.OnDamageTaken
	End Sub

	' Token: 0x060024BD RID: 9405 RVA: 0x00158706 File Offset: 0x00156B06
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.currentHp -= info.damage
		Me.Die()
	End Sub

	' Token: 0x060024BE RID: 9406 RVA: 0x00158721 File Offset: 0x00156B21
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		Gizmos.DrawWireSphere(Me.target, 20F)
	End Sub

	' Token: 0x060024BF RID: 9407 RVA: 0x00158740 File Offset: 0x00156B40
	Protected Overrides Sub Die()
		If Me.currentHp > 0F Then
			Return
		End If
		If Not MyBase.GetComponent(Of Collider2D)().enabled Then
			Return
		End If
		Me.StopAllCoroutines()
		MyBase.GetComponent(Of Collider2D)().enabled = False
		MyBase.animator.SetTrigger("OnDeath")
		AudioManager.Play("level_frogs_tall_firefly_death")
		Me.emitAudioFromObject.Add("level_frogs_tall_firefly_death")
		MyBase.Die()
	End Sub

	' Token: 0x060024C0 RID: 9408 RVA: 0x001587B1 File Offset: 0x00156BB1
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Me.currentHp = 0F
			Me.damageDealer.DealDamage(hit)
			Me.Die()
		End If
	End Sub

	' Token: 0x060024C1 RID: 9409 RVA: 0x001587D8 File Offset: 0x00156BD8
	Protected Overrides Sub OnCollisionGround(hit As GameObject, phase As CollisionPhase)
		Me.currentHp = 0F
		Me.Die()
	End Sub

	' Token: 0x060024C2 RID: 9410 RVA: 0x001587EB File Offset: 0x00156BEB
	Protected Overrides Sub OnCollisionCeiling(hit As GameObject, phase As CollisionPhase)
		Me.currentHp = 0F
		Me.Die()
	End Sub

	' Token: 0x060024C3 RID: 9411 RVA: 0x00158800 File Offset: 0x00156C00
	Private Sub SetMovementPose()
		Dim vector As Vector2 = Me.target - Me.aim.transform.position
		If vector.x > 0F Then
			MyBase.transform.SetScale(New Single?(-1F), Nothing, Nothing)
		Else
			MyBase.transform.SetScale(New Single?(1F), Nothing, Nothing)
		End If
		If Mathf.Abs(vector.x) >= Mathf.Abs(vector.y) Then
			If vector.y < 0F Then
				MyBase.animator.SetTrigger("OnMoveDown")
			Else
				MyBase.animator.SetTrigger("OnMoveForward")
			End If
		Else
			MyBase.animator.SetTrigger("OnMoveForward")
		End If
	End Sub

	' Token: 0x060024C4 RID: 9412 RVA: 0x001588FC File Offset: 0x00156CFC
	Private Iterator Function firefly_cr() As IEnumerator
		Yield MyBase.StartCoroutine(Me.initialMove_cr())
		While True
			Yield MyBase.StartCoroutine(Me.follow_cr())
		End While
		Return
	End Function

	' Token: 0x060024C5 RID: 9413 RVA: 0x00158918 File Offset: 0x00156D18
	Private Iterator Function initialMove_cr() As IEnumerator
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		MyBase.transform.SetScale(New Single?(1F), Nothing, Nothing)
		Dim falloffDistance As Single = 200F
		Dim t As Integer = 0
		Dim falloffFrames As Integer = CInt((falloffDistance * 2F / (Me.Speed / 60F)))
		Dim direction As Vector3 = Me.target - Me.aim.transform.position
		direction.Normalize()
		Dim speed As Single = Me.Speed
		Me.SetMovementPose()
		While Vector2.Distance(MyBase.transform.position, Me.target) > falloffDistance
			MyBase.transform.position += direction * speed * CupheadTime.FixedDelta
			Yield wait
		End While
		While t < falloffFrames
			If PauseManager.state <> PauseManager.State.Paused Then
				Dim num As Single = CSng(t) / CSng(falloffFrames)
				speed = EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, Me.Speed, 0F, num)
				MyBase.transform.position += direction * speed * CupheadTime.FixedDelta
				t += 1
			End If
			Yield wait
		End While
		MyBase.animator.SetTrigger("OnIdle")
		Return
	End Function

	' Token: 0x060024C6 RID: 9414 RVA: 0x00158934 File Offset: 0x00156D34
	Private Iterator Function follow_cr() As IEnumerator
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		MyBase.animator.SetTrigger("OnIdle")
		Yield CupheadTime.WaitForSeconds(Me, Me.followDelay)
		Dim start As Vector2 = MyBase.transform.position
		Me.aim.LookAt2D(Me.player.center)
		Me.target = MyBase.transform.position + Me.aim.right * Me.followDistance
		Me.SetMovementPose()
		Dim t As Single = 0F
		While t < Me.followTime
			Dim val As Single = t / Me.followTime
			Dim x As Single = EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, start.x, Me.target.x, val)
			Dim y As Single = EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, start.y, Me.target.y, val)
			MyBase.transform.SetPosition(New Single?(x), New Single?(y), Nothing)
			t += CupheadTime.FixedDelta
			Yield wait
		End While
		MyBase.transform.SetPosition(New Single?(Me.target.x), New Single?(Me.target.y), Nothing)
		MyBase.animator.SetTrigger("OnIdle")
		Return
	End Function

	' Token: 0x060024C7 RID: 9415 RVA: 0x00158950 File Offset: 0x00156D50
	Protected Overrides Sub FixedUpdate()
		MyBase.FixedUpdate()
		If Me.invincibleDuration > 0F Then
			Me.invincibleDuration -= CupheadTime.FixedDelta
			If Me.invincibleDuration <= 0F Then
				MyBase.GetComponent(Of CircleCollider2D)().enabled = True
			End If
		End If
	End Sub

	' Token: 0x04002D62 RID: 11618
	<SerializeField()>
	Private aim As Transform

	' Token: 0x04002D63 RID: 11619
	<SerializeField()>
	Private sprite As SpriteRenderer

	' Token: 0x04002D64 RID: 11620
	Private Health As Integer

	' Token: 0x04002D65 RID: 11621
	Private Speed As Single

	' Token: 0x04002D66 RID: 11622
	Private followDelay As Single

	' Token: 0x04002D67 RID: 11623
	Private followTime As Single

	' Token: 0x04002D68 RID: 11624
	Private followDistance As Single

	' Token: 0x04002D69 RID: 11625
	Private target As Vector2

	' Token: 0x04002D6A RID: 11626
	Private currentHp As Single

	' Token: 0x04002D6B RID: 11627
	Private player As AbstractPlayerController

	' Token: 0x04002D6C RID: 11628
	Private invincibleDuration As Single
End Class
