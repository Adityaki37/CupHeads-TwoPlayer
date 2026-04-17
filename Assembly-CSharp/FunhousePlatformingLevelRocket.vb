Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020008BC RID: 2236
Public Class FunhousePlatformingLevelRocket
	Inherits PlatformingLevelGroundMovementEnemy

	' Token: 0x0600342A RID: 13354 RVA: 0x001E457A File Offset: 0x001E297A
	Protected Overrides Sub Update()
		MyBase.Update()
	End Sub

	' Token: 0x0600342B RID: 13355 RVA: 0x001E4584 File Offset: 0x001E2984
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.collisionChild = MyBase.GetComponentInChildren(Of CollisionChild)()
		AddHandler Me.collisionChild.OnPlayerCollision, AddressOf Me.OnCollisionPlayer
		AddHandler Me.collisionDamageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
	End Sub

	' Token: 0x0600342C RID: 13356 RVA: 0x001E45D4 File Offset: 0x001E29D4
	Public Sub Init(pos As Vector2, gravityReversed As Boolean, onRight As Boolean)
		MyBase.transform.position = pos
		AudioManager.PlayLoop("funhouse_rocket_idle_loop")
		FunhousePlatformingLevelRocket.ROCKETS_ALIVE += 1
		Me.emitAudioFromObject.Add("funhouse_rocket_idle_loop")
		Me.gravityReversed = gravityReversed
		If gravityReversed Then
			MyBase.transform.SetScale(Nothing, New Single?(-1F), Nothing)
		End If
		Me._direction = If((Not onRight), PlatformingLevelGroundMovementEnemy.Direction.Right, PlatformingLevelGroundMovementEnemy.Direction.Left)
		MyBase.StartCoroutine(Me.launch_cr())
	End Sub

	' Token: 0x0600342D RID: 13357 RVA: 0x001E466C File Offset: 0x001E2A6C
	Private Iterator Function launch_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 0.2F)
		Dim player As AbstractPlayerController = PlayerManager.GetNext()
		Dim dist As Single = player.transform.position.x - MyBase.transform.position.x
		While Mathf.Abs(dist) > Me.distToLaunch
			player = PlayerManager.GetNext()
			dist = player.transform.position.x - MyBase.transform.position.x
			Yield Nothing
		End While
		FunhousePlatformingLevelRocket.ROCKETS_ALIVE -= 1
		If FunhousePlatformingLevelRocket.ROCKETS_ALIVE = 0 Then
			AudioManager.[Stop]("funhouse_rocket_idle_loop")
		End If
		Me.landing = True
		Me.launched = True
		MyBase.animator.SetTrigger("OnShoot")
		While Me.launched
			If Me.gravityReversed Then
				MyBase.transform.position += Vector3.down * Me.launchSpeed * CupheadTime.FixedDelta
			Else
				MyBase.transform.position += Vector3.up * Me.launchSpeed * CupheadTime.FixedDelta
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600342E RID: 13358 RVA: 0x001E4687 File Offset: 0x001E2A87
	Protected Overrides Sub OnCollisionGround(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionGround(hit, phase)
		If Me.launched AndAlso phase = CollisionPhase.Enter Then
			Me.Die()
		End If
	End Sub

	' Token: 0x0600342F RID: 13359 RVA: 0x001E46A8 File Offset: 0x001E2AA8
	Protected Overrides Sub OnCollisionCeiling(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionCeiling(hit, phase)
		If Me.launched AndAlso phase = CollisionPhase.Enter Then
			Me.Die()
		End If
	End Sub

	' Token: 0x06003430 RID: 13360 RVA: 0x001E46CC File Offset: 0x001E2ACC
	Protected Overrides Sub Die()
		AudioManager.[Stop]("funhouse_rocket_trans_to_spin")
		AudioManager.Play("funhouse_rocket_explode")
		Me.emitAudioFromObject.Add("funhouse_rocket_explode")
		Me.explosion.Create(Me.sprite.transform.position)
		MyBase.Die()
	End Sub

	' Token: 0x06003431 RID: 13361 RVA: 0x001E471F File Offset: 0x001E2B1F
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		If Not Me.launched Then
			FunhousePlatformingLevelRocket.ROCKETS_ALIVE -= 1
		End If
		If FunhousePlatformingLevelRocket.ROCKETS_ALIVE = 0 Then
			AudioManager.[Stop]("funhouse_rocket_idle_loop")
		End If
	End Sub

	' Token: 0x06003432 RID: 13362 RVA: 0x001E4752 File Offset: 0x001E2B52
	Private Sub SoundRocketTransToSpin()
		AudioManager.Play("funhouse_rocket_trans_to_spin")
		Me.emitAudioFromObject.Add("funhouse_rocket_trans_to_spin")
		AudioManager.Play("funhouse_rocket_explode")
		Me.emitAudioFromObject.Add("funhouse_rocket_explode")
	End Sub

	' Token: 0x04003C67 RID: 15463
	Private Shared ROCKETS_ALIVE As Integer

	' Token: 0x04003C68 RID: 15464
	<SerializeField()>
	Private sprite As Transform

	' Token: 0x04003C69 RID: 15465
	<SerializeField()>
	Private explosion As FunhousePlatformingLevelExplosionFX

	' Token: 0x04003C6A RID: 15466
	<SerializeField()>
	Private distToLaunch As Single

	' Token: 0x04003C6B RID: 15467
	<SerializeField()>
	Private launchSpeed As Single

	' Token: 0x04003C6C RID: 15468
	Private launched As Boolean

	' Token: 0x04003C6D RID: 15469
	Private collisionChild As CollisionChild

	' Token: 0x04003C6E RID: 15470
	<SerializeField()>
	Private collisionDamageReceiver As DamageReceiver
End Class
