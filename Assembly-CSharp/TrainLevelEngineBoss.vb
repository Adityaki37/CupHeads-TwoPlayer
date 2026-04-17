Imports System
Imports System.Collections
Imports System.Diagnostics
Imports UnityEngine

' Token: 0x0200080D RID: 2061
Public Class TrainLevelEngineBoss
	Inherits LevelProperties.Train.Entity

	' Token: 0x14000051 RID: 81
	' (add) Token: 0x06002FBC RID: 12220 RVA: 0x001C49D0 File Offset: 0x001C2DD0
	' (remove) Token: 0x06002FBD RID: 12221 RVA: 0x001C4A08 File Offset: 0x001C2E08
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnDamageTakenEvent As TrainLevelEngineBoss.OnDamageTakenHandler

	' Token: 0x14000052 RID: 82
	' (add) Token: 0x06002FBE RID: 12222 RVA: 0x001C4A40 File Offset: 0x001C2E40
	' (remove) Token: 0x06002FBF RID: 12223 RVA: 0x001C4A78 File Offset: 0x001C2E78
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnDeathEvent As Action

	' Token: 0x06002FC0 RID: 12224 RVA: 0x001C4AB0 File Offset: 0x001C2EB0
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.tailSwitch = TrainLevelEngineBossTail.Create(Me.tailRoot)
		AddHandler Me.tailSwitch.OnActivate, AddressOf Me.OnTailParried
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		Me.smokeRenderer = Me.dropperRoot.GetComponent(Of SpriteRenderer)()
	End Sub

	' Token: 0x06002FC1 RID: 12225 RVA: 0x001C4B1F File Offset: 0x001C2F1F
	Private Sub Start()
		Me.UpdateHeartDamageReceiver()
	End Sub

	' Token: 0x06002FC2 RID: 12226 RVA: 0x001C4B27 File Offset: 0x001C2F27
	Public Overrides Sub LevelInit(properties As LevelProperties.Train)
		MyBase.LevelInit(properties)
		Me.health = properties.CurrentState.engine.health
	End Sub

	' Token: 0x06002FC3 RID: 12227 RVA: 0x001C4B48 File Offset: 0x001C2F48
	Public Sub StartBoss()
		AudioManager.Play("train_engine_boss_run_start")
		Me.TrainRunStep = True
		MyBase.StartCoroutine(Me.move_cr())
		MyBase.StartCoroutine(Me.tailTimer_cr())
		MyBase.StartCoroutine(Me.fireProjectiles_cr())
		Me.StartAttack()
		Me.UpdateHeartDamageReceiver()
	End Sub

	' Token: 0x06002FC4 RID: 12228 RVA: 0x001C4B99 File Offset: 0x001C2F99
	Private Sub UpdateHeartDamageReceiver()
		Me.heartDamageReceiver.enabled = Me.doorState = TrainLevelEngineBoss.DoorState.Open OrElse Me.doorState = TrainLevelEngineBoss.DoorState.Closing
	End Sub

	' Token: 0x06002FC5 RID: 12229 RVA: 0x001C4BC0 File Offset: 0x001C2FC0
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		If Me.dead Then
			Return
		End If
		If Me.OnDamageTakenEvent IsNot Nothing Then
			Me.OnDamageTakenEvent(info.damage)
		End If
		MyBase.animator.SetBool("Hit", True)
		MyBase.CancelInvoke("StopHitAnim")
		MyBase.Invoke("StopHitAnim", 0.25F)
		Me.health -= info.damage
		If Me.health <= 0F Then
			Me.Die()
		End If
	End Sub

	' Token: 0x06002FC6 RID: 12230 RVA: 0x001C4C4A File Offset: 0x001C304A
	Private Sub Die()
		If Me.dead Then
			Return
		End If
		Me.dead = True
		Me.damageReceiver.enabled = False
		Me.StopAllCoroutines()
		MyBase.StartCoroutine(Me.die_cr())
	End Sub

	' Token: 0x06002FC7 RID: 12231 RVA: 0x001C4C80 File Offset: 0x001C3080
	Private Iterator Function die_cr() As IEnumerator
		AudioManager.Play("train_engine_boss_die")
		Me.emitAudioFromObject.Add("train_engine_boss_die")
		MyBase.animator.SetTrigger("OnDeath")
		Me.door.SetActive(False)
		If Me.OnDeathEvent IsNot Nothing Then
			Me.OnDeathEvent()
		End If
		Yield MyBase.TweenPositionX(MyBase.transform.position.x, -300F, 2.5F * Mathf.Abs(-300F - MyBase.transform.position.x) / 400F, EaseUtils.EaseType.easeInOutSine)
		While True
			Yield MyBase.TweenPositionX(MyBase.transform.position.x, 100F, 2.5F, EaseUtils.EaseType.easeInOutSine)
			Yield MyBase.TweenPositionX(MyBase.transform.position.x, -300F, 2.5F, EaseUtils.EaseType.easeInOutSine)
		End While
		Return
	End Function

	' Token: 0x06002FC8 RID: 12232 RVA: 0x001C4C9B File Offset: 0x001C309B
	Private Sub StopHitAnim()
		MyBase.animator.SetBool("Hit", False)
	End Sub

	' Token: 0x06002FC9 RID: 12233 RVA: 0x001C4CAE File Offset: 0x001C30AE
	Public Sub SpawnDustOnFeet()
		Me.footDustPrefab.Create(Me.footDustRoot.position, Me.footDustRoot.localScale).Play()
	End Sub

	' Token: 0x06002FCA RID: 12234 RVA: 0x001C4CD6 File Offset: 0x001C30D6
	Private Sub StartAttack()
		Me.StopAttack()
		Me.attackCoroutine = Me.attack_cr()
		MyBase.StartCoroutine(Me.attackCoroutine)
	End Sub

	' Token: 0x06002FCB RID: 12235 RVA: 0x001C4CF7 File Offset: 0x001C30F7
	Private Sub StopAttack()
		If Me.attackCoroutine IsNot Nothing Then
			MyBase.StopCoroutine(Me.attackCoroutine)
		End If
	End Sub

	' Token: 0x06002FCC RID: 12236 RVA: 0x001C4D10 File Offset: 0x001C3110
	Private Sub OnAttackAnimComplete()
		Me.dropperPrefab.Create(Me.dropperRoot.position, MyBase.properties.CurrentState.engine.projectileUpSpeed, MyBase.properties.CurrentState.engine.projectileXSpeed, MyBase.properties.CurrentState.engine.projectileGravity)
	End Sub

	' Token: 0x06002FCD RID: 12237 RVA: 0x001C4D78 File Offset: 0x001C3178
	Public Sub SmokeFX()
		Me.smokeRenderer.flipX = Rand.Bool()
		MyBase.animator.SetTrigger("Smoke")
	End Sub

	' Token: 0x06002FCE RID: 12238 RVA: 0x001C4D9C File Offset: 0x001C319C
	Private Iterator Function attack_cr() As IEnumerator
		While True
			Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.engine.projectileDelay)
			MyBase.animator.SetTrigger("OnAttack")
			AudioManager.Play("train_engine_boss_attack")
			Me.emitAudioFromObject.Add("train_engine_boss_attack")
			Yield MyBase.animator.WaitForAnimationToStart(Me, "Attack", False)
			Yield MyBase.animator.WaitForAnimationToEnd(Me, "Attack", False, True)
		End While
		Return
	End Function

	' Token: 0x06002FCF RID: 12239 RVA: 0x001C4DB8 File Offset: 0x001C31B8
	Private Iterator Function fireProjectiles_cr() As IEnumerator
		While True
			If Me.doorState = TrainLevelEngineBoss.DoorState.Open Then
				MyBase.animator.SetTrigger("FireAttack")
				Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.engine.fireDelay)
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06002FD0 RID: 12240 RVA: 0x001C4DD4 File Offset: 0x001C31D4
	Public Sub SpawnProjectile()
		Dim zero As Vector2 = Vector2.zero
		zero.y = MyBase.properties.CurrentState.engine.fireVelocityY
		zero.x = MyBase.properties.CurrentState.engine.fireVelocityX
		Me.firePrefab.Create(Me.fireRoot.position, zero, CSng(MyBase.properties.CurrentState.engine.fireGravity))
	End Sub

	' Token: 0x17000419 RID: 1049
	' (get) Token: 0x06002FD1 RID: 12241 RVA: 0x001C4E5B File Offset: 0x001C325B
	' (set) Token: 0x06002FD2 RID: 12242 RVA: 0x001C4E63 File Offset: 0x001C3263
	Private Property doorState As TrainLevelEngineBoss.DoorState
		Get
			Return Me._ds
		End Get
		Set(value As TrainLevelEngineBoss.DoorState)
			If value = Me._ds Then
				Return
			End If
			Me._ds = value
			Me.UpdateHeartDamageReceiver()
		End Set
	End Property

	' Token: 0x06002FD3 RID: 12243 RVA: 0x001C4E7F File Offset: 0x001C327F
	Private Sub DoorAnimOpenStarted()
		If Me.desiredDoorState = TrainLevelEngineBoss.DoorState.Open AndAlso Me.doorState = TrainLevelEngineBoss.DoorState.Closed Then
			Me.doorState = TrainLevelEngineBoss.DoorState.Opening
			MyBase.animator.SetTrigger("Open")
		End If
		Me.UpdateDoorSprite()
	End Sub

	' Token: 0x06002FD4 RID: 12244 RVA: 0x001C4EB5 File Offset: 0x001C32B5
	Private Sub DoorAnimCloseStarted()
		If Me.desiredDoorState = TrainLevelEngineBoss.DoorState.Closed AndAlso Me.doorState = TrainLevelEngineBoss.DoorState.Open Then
			Me.doorState = TrainLevelEngineBoss.DoorState.Closing
			MyBase.animator.SetTrigger("Close")
		End If
		Me.UpdateDoorSprite()
	End Sub

	' Token: 0x06002FD5 RID: 12245 RVA: 0x001C4EEB File Offset: 0x001C32EB
	Private Sub DoorOpenAnimComplete()
		If Me.doorState = TrainLevelEngineBoss.DoorState.Opening Then
			AudioManager.Play("train_engine_boss_door")
			Me.emitAudioFromObject.Add("train_engine_boss_door")
			Me.doorState = TrainLevelEngineBoss.DoorState.Open
		End If
		Me.UpdateDoorSprite()
	End Sub

	' Token: 0x06002FD6 RID: 12246 RVA: 0x001C4F20 File Offset: 0x001C3320
	Private Sub DoorCloseAnimComplete()
		If Me.doorState = TrainLevelEngineBoss.DoorState.Closing Then
			AudioManager.Play("train_engine_boss_door_shut")
			Me.emitAudioFromObject.Add("train_engine_boss_door_shut")
			Me.doorState = TrainLevelEngineBoss.DoorState.Closed
		End If
		Me.UpdateDoorSprite()
	End Sub

	' Token: 0x06002FD7 RID: 12247 RVA: 0x001C4F55 File Offset: 0x001C3355
	Private Sub IronStepSFX()
		If Me.TrainRunStep Then
			AudioManager.Play("train_engine_step")
			Me.emitAudioFromObject.Add("train_engine_step")
		End If
	End Sub

	' Token: 0x06002FD8 RID: 12248 RVA: 0x001C4F7C File Offset: 0x001C337C
	Private Sub UpdateDoorSprite()
		Me.doorSprites.DisableAll()
		Me.doorSprites(Me.doorState).enabled = True
	End Sub

	' Token: 0x06002FD9 RID: 12249 RVA: 0x001C4FA0 File Offset: 0x001C33A0
	Private Iterator Function doorTimer_cr() As IEnumerator
		Me.desiredDoorState = TrainLevelEngineBoss.DoorState.Open
		Dim time As Single = MyBase.properties.CurrentState.engine.doorTime.GetFloatAt(Me.health / MyBase.properties.CurrentState.engine.health)
		While Me.doorState <> TrainLevelEngineBoss.DoorState.Open
			Yield Nothing
		End While
		Yield CupheadTime.WaitForSeconds(Me, time)
		Me.desiredDoorState = TrainLevelEngineBoss.DoorState.Closed
		While Me.doorState <> TrainLevelEngineBoss.DoorState.Closed
			Yield Nothing
		End While
		MyBase.StartCoroutine(Me.tailTimer_cr())
		Return
	End Function

	' Token: 0x1700041A RID: 1050
	' (get) Token: 0x06002FDA RID: 12250 RVA: 0x001C4FBB File Offset: 0x001C33BB
	' (set) Token: 0x06002FDB RID: 12251 RVA: 0x001C4FC3 File Offset: 0x001C33C3
	Private Property tailState As TrainLevelEngineBoss.TailState
		Get
			Return Me._tailState
		End Get
		Set(value As TrainLevelEngineBoss.TailState)
			Me.ChangeTail(value)
		End Set
	End Property

	' Token: 0x06002FDC RID: 12252 RVA: 0x001C4FCC File Offset: 0x001C33CC
	Private Sub ChangeTail(state As TrainLevelEngineBoss.TailState)
		If state = Me.tailState Then
			Return
		End If
		Me.tailSwitch.tailEnabled = state = TrainLevelEngineBoss.TailState.[On]
		Me._tailState = state
		Me.tailSprites.DisableAll()
		Me.tailSprites(state).enabled = True
	End Sub

	' Token: 0x06002FDD RID: 12253 RVA: 0x001C5019 File Offset: 0x001C3419
	Private Sub OnTailParried()
		Me.tailState = TrainLevelEngineBoss.TailState.Off
		MyBase.StartCoroutine(Me.doorTimer_cr())
	End Sub

	' Token: 0x06002FDE RID: 12254 RVA: 0x001C5030 File Offset: 0x001C3430
	Private Iterator Function tailTimer_cr() As IEnumerator
		Me.tailState = TrainLevelEngineBoss.TailState.Off
		Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.engine.tailDelay)
		Me.tailState = TrainLevelEngineBoss.TailState.[On]
		Return
	End Function

	' Token: 0x06002FDF RID: 12255 RVA: 0x001C504C File Offset: 0x001C344C
	Private Iterator Function move_cr() As IEnumerator
		Dim max_x As Single = MyBase.properties.CurrentState.engine.maxDist
		Dim min_x As Single = MyBase.properties.CurrentState.engine.minDist
		Dim forwardTime As Single = MyBase.properties.CurrentState.engine.forwardTime
		Dim backTime As Single = MyBase.properties.CurrentState.engine.backTime
		Yield MyBase.TweenLocalPositionX(MyBase.transform.position.x, min_x, 3F, EaseUtils.EaseType.easeOutSine)
		AudioManager.FadeSFXVolume("train_engine_boss_run_start", 0F, 3F)
		AudioManager.PlayLoop("train_engine_boss_run_loop")
		Me.emitAudioFromObject.Add("train_engine_boss_run_loop")
		AudioManager.PlayLoop("train_engine_boss_fire_idle")
		Me.emitAudioFromObject.Add("train_engine_boss_fire_idle")
		While True
			Yield MyBase.TweenLocalPositionX(MyBase.transform.position.x, max_x, forwardTime, EaseUtils.EaseType.easeInOutSine)
			Yield MyBase.TweenLocalPositionX(MyBase.transform.position.x, min_x, backTime, EaseUtils.EaseType.easeInOutSine)
		End While
		Return
	End Function

	' Token: 0x06002FE0 RID: 12256 RVA: 0x001C5067 File Offset: 0x001C3467
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.footDustPrefab = Nothing
		Me.dropperPrefab = Nothing
		Me.firePrefab = Nothing
	End Sub

	' Token: 0x04003898 RID: 14488
	Private Const HitParameterName As String = "Hit"

	' Token: 0x04003899 RID: 14489
	Private Const StopHitAnimName As String = "StopHitAnim"

	' Token: 0x0400389A RID: 14490
	Private Const StopHitAnimTime As Single = 0.25F

	' Token: 0x0400389B RID: 14491
	<SerializeField()>
	Private heartDamageReceiver As DamageReceiverChild

	' Token: 0x0400389C RID: 14492
	<SerializeField()>
	Private footDustRoot As Transform

	' Token: 0x0400389D RID: 14493
	<SerializeField()>
	Private footDustPrefab As Effect

	' Token: 0x0400389E RID: 14494
	Private damageReceiver As DamageReceiver

	' Token: 0x0400389F RID: 14495
	Private health As Single

	' Token: 0x040038A0 RID: 14496
	Private dead As Boolean

	' Token: 0x040038A1 RID: 14497
	Private TrainRunStep As Boolean

	' Token: 0x040038A4 RID: 14500
	<Header("Dropper")>
	<SerializeField()>
	Private dropperRoot As Transform

	' Token: 0x040038A5 RID: 14501
	<SerializeField()>
	Private dropperPrefab As TrainLevelEngineBossDropperProjectile

	' Token: 0x040038A6 RID: 14502
	Private attackCoroutine As IEnumerator

	' Token: 0x040038A7 RID: 14503
	Private smokeRenderer As SpriteRenderer

	' Token: 0x040038A8 RID: 14504
	Private Const FireAttackParameterName As String = "FireAttack"

	' Token: 0x040038A9 RID: 14505
	<Header("Fire")>
	<SerializeField()>
	Private fireRoot As Transform

	' Token: 0x040038AA RID: 14506
	<SerializeField()>
	Private firePrefab As TrainLevelEngineBossFireProjectile

	' Token: 0x040038AB RID: 14507
	Private Const OpenDoorParameterName As String = "Open"

	' Token: 0x040038AC RID: 14508
	Private Const CloseDoorParameterName As String = "Close"

	' Token: 0x040038AD RID: 14509
	<Header("Door")>
	<SerializeField()>
	Private doorSprites As TrainLevelEngineBoss.DoorSprites

	' Token: 0x040038AE RID: 14510
	<SerializeField()>
	Private door As GameObject

	' Token: 0x040038AF RID: 14511
	Private _ds As TrainLevelEngineBoss.DoorState = TrainLevelEngineBoss.DoorState.Closed

	' Token: 0x040038B0 RID: 14512
	Private desiredDoorState As TrainLevelEngineBoss.DoorState = TrainLevelEngineBoss.DoorState.Closed

	' Token: 0x040038B1 RID: 14513
	<Header("Tail")>
	<SerializeField()>
	Private tailSprites As TrainLevelEngineBoss.TailSprites

	' Token: 0x040038B2 RID: 14514
	<SerializeField()>
	Private tailRoot As Transform

	' Token: 0x040038B3 RID: 14515
	Private _tailState As TrainLevelEngineBoss.TailState = TrainLevelEngineBoss.TailState.Off

	' Token: 0x040038B4 RID: 14516
	Private tailSwitch As TrainLevelEngineBossTail

	' Token: 0x0200080E RID: 2062
	' (Invoke) Token: 0x06002FE2 RID: 12258
	Public Delegate Sub OnDamageTakenHandler(damage As Single)

	' Token: 0x0200080F RID: 2063
	Public Enum DoorState
		' Token: 0x040038B6 RID: 14518
		Open
		' Token: 0x040038B7 RID: 14519
		Closed
		' Token: 0x040038B8 RID: 14520
		Opening
		' Token: 0x040038B9 RID: 14521
		Closing
	End Enum

	' Token: 0x02000810 RID: 2064
	<Serializable()>
	Public Class DoorSprites
		' Token: 0x1700041B RID: 1051
		Public ReadOnly Default Property Item(state As TrainLevelEngineBoss.DoorState) As SpriteRenderer
			Get
				Select Case state
					Case Else
						Return Me.open
					Case TrainLevelEngineBoss.DoorState.Closed
						Return Me.closed
					Case TrainLevelEngineBoss.DoorState.Opening
						Return Me.opening
					Case TrainLevelEngineBoss.DoorState.Closing
						Return Me.closing
				End Select
			End Get
		End Property

		' Token: 0x06002FE7 RID: 12263 RVA: 0x001C50C4 File Offset: 0x001C34C4
		Public Sub DisableAll()
			Me.open.enabled = False
			Me.closed.enabled = False
			Me.opening.enabled = False
			Me.closing.enabled = False
		End Sub

		' Token: 0x040038BA RID: 14522
		Public open As SpriteRenderer

		' Token: 0x040038BB RID: 14523
		Public closed As SpriteRenderer

		' Token: 0x040038BC RID: 14524
		Public opening As SpriteRenderer

		' Token: 0x040038BD RID: 14525
		Public closing As SpriteRenderer
	End Class

	' Token: 0x02000811 RID: 2065
	Public Enum TailState
		' Token: 0x040038BF RID: 14527
		[On]
		' Token: 0x040038C0 RID: 14528
		Off
	End Enum

	' Token: 0x02000812 RID: 2066
	<Serializable()>
	Public Class TailSprites
		' Token: 0x1700041C RID: 1052
		Public ReadOnly Default Property Item(state As TrainLevelEngineBoss.TailState) As SpriteRenderer
			Get
				If state = TrainLevelEngineBoss.TailState.[On] OrElse state <> TrainLevelEngineBoss.TailState.Off Then
					Return Me.[on]
				End If
				Return Me.off
			End Get
		End Property

		' Token: 0x06002FEA RID: 12266 RVA: 0x001C511F File Offset: 0x001C351F
		Public Sub DisableAll()
			Me.[on].enabled = False
			Me.off.enabled = False
		End Sub

		' Token: 0x040038C1 RID: 14529
		Public [on] As SpriteRenderer

		' Token: 0x040038C2 RID: 14530
		Public off As SpriteRenderer
	End Class
End Class
