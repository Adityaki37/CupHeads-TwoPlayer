Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020004ED RID: 1261
Public Class BaronessLevelGumball
	Inherits BaronessLevelMiniBossBase

	' Token: 0x17000321 RID: 801
	' (get) Token: 0x060015F9 RID: 5625 RVA: 0x000C5167 File Offset: 0x000C3567
	' (set) Token: 0x060015FA RID: 5626 RVA: 0x000C516F File Offset: 0x000C356F
	Public Property state As BaronessLevelGumball.State

	' Token: 0x060015FB RID: 5627 RVA: 0x000C5178 File Offset: 0x000C3578
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.fadeTime = 0.6F
		Me.isDying = False
		Me.movingLeft = True
		MyBase.RegisterCollisionChild(Me.headCollider)
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		Me.damageReceiverChild = Me.headCollider.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		AddHandler Me.damageReceiverChild.OnDamageTaken, AddressOf Me.OnDamageTaken
		AddHandler Me.headCollider.OnPlayerCollision, AddressOf Me.OnCollisionPlayer
		AddHandler Me.headCollider.OnPlayerProjectileCollision, AddressOf Me.OnCollisionPlayerProjectile
	End Sub

	' Token: 0x060015FC RID: 5628 RVA: 0x000C5238 File Offset: 0x000C3638
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.legs.GetComponent(Of SpriteRenderer)().sortingLayerName = SpriteLayer.Background.ToString()
		Me.legs.GetComponent(Of SpriteRenderer)().sortingOrder = 130
		Me.lid.GetComponent(Of SpriteRenderer)().sortingLayerName = SpriteLayer.Background.ToString()
		Me.lid.GetComponent(Of SpriteRenderer)().sortingOrder = 140
		AudioManager.PlayLoop("level_baroness_gumball_feet_loop")
		Me.emitAudioFromObject.Add("level_baroness_gumball_feet_loop")
	End Sub

	' Token: 0x060015FD RID: 5629 RVA: 0x000C52D0 File Offset: 0x000C36D0
	Public Sub Init(properties As LevelProperties.Baroness.Gumball, pos As Vector2, health As Single)
		Me.properties = properties
		Me.health = health
		MyBase.transform.position = pos
		Me.offTime = properties.gumballAttackDurationOffRange
		MyBase.StartCoroutine(Me.leaving_castle_cr())
		MyBase.StartCoroutine(Me.switch_child_cr())
		MyBase.StartCoroutine(Me.gumball_off_timer_cr())
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x060015FE RID: 5630 RVA: 0x000C5344 File Offset: 0x000C3744
	Protected Overridable Iterator Function leaving_castle_cr() As IEnumerator
		Dim t As Single = 0F
		Dim offTime As Single = 0.22F
		While t < offTime
			Me.lid.GetComponent(Of SpriteRenderer)().enabled = False
			MyBase.GetComponent(Of SpriteRenderer)().enabled = False
			t += CupheadTime.Delta
			Yield Nothing
		End While
		Me.lid.GetComponent(Of SpriteRenderer)().enabled = True
		MyBase.GetComponent(Of SpriteRenderer)().enabled = True
		Yield Nothing
		Return
	End Function

	' Token: 0x060015FF RID: 5631 RVA: 0x000C5360 File Offset: 0x000C3760
	Private Iterator Function switch_child_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 3F)
		Me.legs.GetComponent(Of SpriteRenderer)().sortingLayerName = SpriteLayer.Background.ToString()
		Me.lid.GetComponent(Of SpriteRenderer)().sortingLayerName = SpriteLayer.Background.ToString()
		Me.lid.GetComponent(Of SpriteRenderer)().sortingOrder = 252
		Me.legs.GetComponent(Of SpriteRenderer)().sortingOrder = 251
		Return
	End Function

	' Token: 0x06001600 RID: 5632 RVA: 0x000C537B File Offset: 0x000C377B
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06001601 RID: 5633 RVA: 0x000C5399 File Offset: 0x000C3799
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06001602 RID: 5634 RVA: 0x000C53B4 File Offset: 0x000C37B4
	Protected Overrides Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		If Me.health > 0F Then
			MyBase.OnDamageTaken(info)
		End If
		Me.health -= info.damage
		If Me.health < 0F AndAlso Me.state <> BaronessLevelGumball.State.Dying Then
			Dim damageInfo As DamageDealer.DamageInfo = New DamageDealer.DamageInfo(Me.health, info.direction, info.origin, info.damageSource)
			MyBase.OnDamageTaken(damageInfo)
			Me.state = BaronessLevelGumball.State.Dying
			MyBase.StartCoroutine(Me.death_cr())
		End If
	End Sub

	' Token: 0x06001603 RID: 5635 RVA: 0x000C5440 File Offset: 0x000C3840
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.projectilePrefabs = Nothing
	End Sub

	' Token: 0x06001604 RID: 5636 RVA: 0x000C5450 File Offset: 0x000C3850
	Private Iterator Function move_cr() As IEnumerator
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		Dim endedLoop As Boolean = False
		Dim movingRight As Boolean = False
		Dim time As Single = Me.properties.gumballMovementSpeed
		Dim [end] As Single = 0F
		Dim t As Single = 0F
		While True
			Dim start As Single = MyBase.transform.position.x
			If movingRight Then
				[end] = 640F - Me.properties.offsetX.max
			Else
				[end] = -640F + Me.properties.offsetX.min
			End If
			While t < time
				Dim val As Single = t / time
				MyBase.transform.SetPosition(New Single?(EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, start, [end], val)), Nothing, Nothing)
				If val > 0.8F AndAlso Not endedLoop Then
					If Me.isDying AndAlso Not movingRight Then
						Exit While
					End If
					If Me.state = BaronessLevelGumball.State.[On] Then
						Me.headSpark.SetActive(False)
					End If
					MyBase.animator.SetBool("Turn", True)
					MyBase.animator.Play("Run_Legs")
					endedLoop = True
				End If
				t += CupheadTime.FixedDelta
				Yield wait
			End While
			If Me.isDying Then
				Exit For
			End If
			endedLoop = False
			t = 0F
			MyBase.transform.SetPosition(New Single?([end]), Nothing, Nothing)
			movingRight = Not movingRight
			Yield wait
		End While
		While MyBase.transform.position.x > -940F
			MyBase.transform.AddPosition(-Me.properties.gumballDeathSpeed * CupheadTime.FixedDelta, 0F, 0F)
			Yield wait
		End While
		AudioManager.[Stop]("level_baroness_gumball_feet_loop")
		Me.Die()
		Return
	End Function

	' Token: 0x06001605 RID: 5637 RVA: 0x000C546C File Offset: 0x000C386C
	Private Sub Switch()
		MyBase.transform.SetScale(New Single?(-MyBase.transform.localScale.x), New Single?(1F), New Single?(1F))
		Me.feetDust.SetScale(New Single?(-Me.feetDust.localScale.x), New Single?(1F), New Single?(1F))
		MyBase.animator.SetBool("Turn", False)
		If Me.state = BaronessLevelGumball.State.[On] Then
			Me.headSpark.SetActive(True)
		End If
	End Sub

	' Token: 0x06001606 RID: 5638 RVA: 0x000C5514 File Offset: 0x000C3914
	Private Iterator Function on_cr() As IEnumerator
		Dim rateTime As Single = 0F
		Dim attackTime As Single = 0F
		Dim attackDuration As Single = Me.properties.gumballAttackDurationOnRange.RandomFloat()
		MyBase.animator.SetBool("Open", True)
		Yield MyBase.animator.WaitForAnimationToStart(Me, "Run_Open_Trans", False)
		AudioManager.PlayLoop("level_baroness_gumball_shoot_loop")
		Me.emitAudioFromObject.Add("level_baroness_gumball_shoot_loop")
		Me.headSpark.SetActive(True)
		Me.state = BaronessLevelGumball.State.[On]
		While attackTime < attackDuration
			If Me.isDying Then
				Exit While
			End If
			attackTime += CupheadTime.Delta
			If rateTime > Me.properties.rateOfFire Then
				Me.fireProjectiles()
				rateTime = 0F
			Else
				rateTime += CupheadTime.Delta
			End If
			Yield Nothing
		End While
		MyBase.animator.SetBool("Open", False)
		Yield New WaitForEndOfFrame()
		MyBase.StartCoroutine(Me.gumball_off_timer_cr())
		Yield Nothing
		Return
	End Function

	' Token: 0x06001607 RID: 5639 RVA: 0x000C5530 File Offset: 0x000C3930
	Private Sub fireProjectiles()
		Dim zero As Vector2 = Vector2.zero
		Dim num As Single = CSng(If((Not Me.movingLeft), 200, (-200)))
		zero.y = Me.properties.velocityY.RandomFloat()
		zero.x = Me.properties.velocityX.RandomFloat() + num
		Me.projectilePrefabs(Global.UnityEngine.Random.Range(0, Me.projectilePrefabs.Length - 1)).Create(Me.projectileRoot.position, zero, Me.properties.gravity)
	End Sub

	' Token: 0x06001608 RID: 5640 RVA: 0x000C55C8 File Offset: 0x000C39C8
	Private Iterator Function gumball_off_timer_cr() As IEnumerator
		Me.headSpark.SetActive(False)
		AudioManager.[Stop]("level_baroness_gumball_shoot_loop")
		Me.state = BaronessLevelGumball.State.Off
		Me.offTime = Me.properties.gumballAttackDurationOffRange.RandomFloat()
		Yield CupheadTime.WaitForSeconds(Me, Me.offTime)
		MyBase.StartCoroutine(Me.on_cr())
		Yield Nothing
		Return
	End Function

	' Token: 0x06001609 RID: 5641 RVA: 0x000C55E4 File Offset: 0x000C39E4
	Private Iterator Function death_cr() As IEnumerator
		Me.StartExplosions()
		Me.headCollider.GetComponent(Of Collider2D)().enabled = False
		MyBase.GetComponent(Of Collider2D)().enabled = False
		Me.isDying = True
		MyBase.animator.Play("Run_Death")
		MyBase.animator.SetTrigger("Death")
		Yield Nothing
		Return
	End Function

	' Token: 0x0600160A RID: 5642 RVA: 0x000C55FF File Offset: 0x000C39FF
	Private Sub SoundGumballLidOpen()
		AudioManager.Play("level_baroness_gumball_lid_open")
		Me.emitAudioFromObject.Add("level_baroness_gumball_lid_open")
	End Sub

	' Token: 0x0600160B RID: 5643 RVA: 0x000C561B File Offset: 0x000C3A1B
	Private Sub SoundGumballLidClose()
		AudioManager.Play("level_baroness_gumball_lid_close")
		Me.emitAudioFromObject.Add("level_baroness_gumball_lid_close")
	End Sub

	' Token: 0x04001F48 RID: 8008
	<SerializeField()>
	Private projectilePrefabs As BaronessLevelGumballProjectile()

	' Token: 0x04001F49 RID: 8009
	<SerializeField()>
	Private projectileRoot As Transform

	' Token: 0x04001F4A RID: 8010
	<SerializeField()>
	Private lid As SpriteRenderer

	' Token: 0x04001F4B RID: 8011
	<SerializeField()>
	Private legs As SpriteRenderer

	' Token: 0x04001F4C RID: 8012
	<SerializeField()>
	Private headCollider As CollisionChild

	' Token: 0x04001F4D RID: 8013
	<SerializeField()>
	Private headSpark As GameObject

	' Token: 0x04001F4E RID: 8014
	<SerializeField()>
	Private feetDust As Transform

	' Token: 0x04001F4F RID: 8015
	Private properties As LevelProperties.Baroness.Gumball

	' Token: 0x04001F50 RID: 8016
	Private damageDealer As DamageDealer

	' Token: 0x04001F51 RID: 8017
	Private damageReceiver As DamageReceiver

	' Token: 0x04001F52 RID: 8018
	Private damageReceiverChild As DamageReceiver

	' Token: 0x04001F53 RID: 8019
	Private health As Single

	' Token: 0x04001F54 RID: 8020
	Private offTime As Single

	' Token: 0x04001F55 RID: 8021
	Private onTime As Single

	' Token: 0x04001F56 RID: 8022
	Private movingLeft As Boolean

	' Token: 0x04001F57 RID: 8023
	Private slowDown As Boolean

	' Token: 0x020004EE RID: 1262
	Public Enum State
		' Token: 0x04001F59 RID: 8025
		[On]
		' Token: 0x04001F5A RID: 8026
		Off
		' Token: 0x04001F5B RID: 8027
		Dying
	End Enum
End Class
