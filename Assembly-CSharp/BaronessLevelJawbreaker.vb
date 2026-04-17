Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x020004F0 RID: 1264
Public Class BaronessLevelJawbreaker
	Inherits BaronessLevelMiniBossBase

	' Token: 0x17000322 RID: 802
	' (get) Token: 0x06001617 RID: 5655 RVA: 0x000C6295 File Offset: 0x000C4695
	' (set) Token: 0x06001618 RID: 5656 RVA: 0x000C629D File Offset: 0x000C469D
	Public Property state As BaronessLevelJawbreaker.State

	' Token: 0x06001619 RID: 5657 RVA: 0x000C62A8 File Offset: 0x000C46A8
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.isDying = False
		Me.state = BaronessLevelJawbreaker.State.Spawned
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
	End Sub

	' Token: 0x0600161A RID: 5658 RVA: 0x000C62F8 File Offset: 0x000C46F8
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.sprite.GetComponent(Of SpriteRenderer)().sortingLayerName = SpriteLayer.Background.ToString()
		Me.sprite.GetComponent(Of SpriteRenderer)().sortingOrder = 150
		MyBase.StartCoroutine(Me.check_rotation_cr())
		MyBase.StartCoroutine(Me.switch_cr())
		MyBase.StartCoroutine(Me.reset_sprite_cr())
	End Sub

	' Token: 0x0600161B RID: 5659 RVA: 0x000C6368 File Offset: 0x000C4768
	Private Iterator Function switch_cr() As IEnumerator
		MyBase.StartCoroutine(Me.fade_color_cr())
		Yield CupheadTime.WaitForSeconds(Me, 3F)
		Me.sprite.GetComponent(Of SpriteRenderer)().sortingLayerName = SpriteLayer.Enemies.ToString()
		Me.sprite.GetComponent(Of SpriteRenderer)().sortingOrder = 251
		Return
	End Function

	' Token: 0x0600161C RID: 5660 RVA: 0x000C6383 File Offset: 0x000C4783
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x0600161D RID: 5661 RVA: 0x000C63A4 File Offset: 0x000C47A4
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
		If Me.player Is Nothing OrElse Me.player.IsDead Then
			Me.player = PlayerManager.GetNext()
		End If
		If Me.aim Is Nothing OrElse Me.player Is Nothing OrElse Me.state = BaronessLevelJawbreaker.State.Unspawned Then
			Return
		End If
	End Sub

	' Token: 0x0600161E RID: 5662 RVA: 0x000C6424 File Offset: 0x000C4824
	Private Sub FixedUpdate()
		If Me.state = BaronessLevelJawbreaker.State.Spawned Then
			MyBase.transform.position -= MyBase.transform.right * Me.properties.jawbreakerHomingSpeed * CupheadTime.FixedDelta * Me.hitPauseCoefficient()
			Me.aim.LookAt2D(2F * MyBase.transform.position - Me.player.center)
			MyBase.transform.rotation = Quaternion.Slerp(MyBase.transform.rotation, Me.aim.rotation, Me.rotationSpeed * CupheadTime.FixedDelta * Me.hitPauseCoefficient())
		End If
	End Sub

	' Token: 0x0600161F RID: 5663 RVA: 0x000C64EC File Offset: 0x000C48EC
	Private Iterator Function check_rotation_cr() As IEnumerator
		While True
			If((Me.player.transform.position.x < MyBase.transform.position.x AndAlso Not Me.lookingLeft) OrElse (Me.player.transform.position.x > MyBase.transform.position.x AndAlso Me.lookingLeft)) AndAlso Not Me.isTurning Then
				Me.isTurning = True
				MyBase.animator.SetTrigger("Turn")
				Yield MyBase.animator.WaitForAnimationToEnd(Me, "Turn", False, True)
				Me.lookingLeft = Not Me.lookingLeft
				Me.isTurning = False
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06001620 RID: 5664 RVA: 0x000C6508 File Offset: 0x000C4908
	Private Sub Turn()
		Me.sprite.transform.SetScale(New Single?(-Me.sprite.transform.localScale.x), New Single?(1F), New Single?(1F))
	End Sub

	' Token: 0x06001621 RID: 5665 RVA: 0x000C6558 File Offset: 0x000C4958
	Protected Overrides Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		If Me.health > 0F Then
			MyBase.OnDamageTaken(info)
		End If
		Me.health -= info.damage
		If Me.health < 0F AndAlso Me.state = BaronessLevelJawbreaker.State.Spawned Then
			Dim damageInfo As DamageDealer.DamageInfo = New DamageDealer.DamageInfo(Me.health, info.direction, info.origin, info.damageSource)
			MyBase.OnDamageTaken(damageInfo)
			MyBase.StartCoroutine(Me.stopminis_cr())
			Me.StartDeath()
		End If
	End Sub

	' Token: 0x06001622 RID: 5666 RVA: 0x000C65E4 File Offset: 0x000C49E4
	Public Sub Init(properties As LevelProperties.Baroness.Jawbreaker, player As AbstractPlayerController, pos As Vector2, rotationSpeed As Single, health As Single)
		Me.aim = New GameObject("Aim").transform
		Me.aim.SetParent(MyBase.transform)
		Me.aim.ResetLocalTransforms()
		Me.properties = properties
		Me.player = player
		Me.rotationSpeed = rotationSpeed
		Me.health = health
		MyBase.transform.position = pos
		Me.spawnPos = MyBase.transform.position
		MyBase.StartCoroutine(Me.pickplayer_cr())
		Me.minisRoutine = MyBase.StartCoroutine(Me.minis_cr())
	End Sub

	' Token: 0x06001623 RID: 5667 RVA: 0x000C6684 File Offset: 0x000C4A84
	Private Iterator Function pickplayer_cr() As IEnumerator
		While True
			Yield CupheadTime.WaitForSeconds(Me, Me.properties.jawbreakerHomeDuration)
			Me.player = PlayerManager.GetNext()
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06001624 RID: 5668 RVA: 0x000C669F File Offset: 0x000C4A9F
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.miniBluePrefab = Nothing
		Me.miniRedPrefab = Nothing
		Me.ghostPrefab = Nothing
	End Sub

	' Token: 0x06001625 RID: 5669 RVA: 0x000C66BC File Offset: 0x000C4ABC
	Private Iterator Function minis_cr() As IEnumerator
		Me.targetPos = Me.followPoint
		Dim targetPos2 As Transform = Me.targetPos
		Me.prefabsList = New List(Of BaronessLevelJawbreakerMini)()
		Dim spawnTime As Single = Me.properties.jawbreakerMiniSpace / Me.properties.jawbreakerHomingSpeed
		For i As Integer = 0 To Me.properties.jawbreakerMinis - 1
			If i Mod 2 = 0 Then
				Yield CupheadTime.WaitForSeconds(Me, spawnTime)
				Dim blueminijawbreakers As BaronessLevelJawbreakerMini = Global.UnityEngine.[Object].Instantiate(Of BaronessLevelJawbreakerMini)(Me.miniBluePrefab)
				blueminijawbreakers.Init(Me.properties, Me.spawnPos, Me.targetPos, Me.rotationSpeed)
				targetPos2 = blueminijawbreakers.transform
				Me.prefabsList.Add(blueminijawbreakers)
			ElseIf i Mod 2 = 1 Then
				Yield CupheadTime.WaitForSeconds(Me, spawnTime)
				Dim redminijawbreakers As BaronessLevelJawbreakerMini = Global.UnityEngine.[Object].Instantiate(Of BaronessLevelJawbreakerMini)(Me.miniRedPrefab)
				redminijawbreakers.Init(Me.properties, Me.spawnPos, targetPos2, Me.rotationSpeed)
				Me.targetPos = redminijawbreakers.transform
				Me.prefabsList.Add(redminijawbreakers)
			End If
			Yield Nothing
		Next
		Yield Nothing
		Return
	End Function

	' Token: 0x06001626 RID: 5670 RVA: 0x000C66D8 File Offset: 0x000C4AD8
	Private Iterator Function stopminis_cr() As IEnumerator
		MyBase.StopCoroutine(Me.minisRoutine)
		For i As Integer = 0 To Me.prefabsList.Count - 1
			Me.prefabsList(i).[Stop]()
			Yield Nothing
		Next
		MyBase.StartCoroutine(Me.killminis_cr())
		Return
	End Function

	' Token: 0x06001627 RID: 5671 RVA: 0x000C66F4 File Offset: 0x000C4AF4
	Private Iterator Function killminis_cr() As IEnumerator
		Me.prefabsList.Reverse()
		For i As Integer = 0 To Me.prefabsList.Count - 1
			Me.prefabsList(i).StartDying()
			Yield CupheadTime.WaitForSeconds(Me, 0.8F)
		Next
		Return
	End Function

	' Token: 0x06001628 RID: 5672 RVA: 0x000C670F File Offset: 0x000C4B0F
	Public Sub StartDeath()
		Me.state = BaronessLevelJawbreaker.State.Explode
		MyBase.StartCoroutine(Me.dying_cr())
	End Sub

	' Token: 0x06001629 RID: 5673 RVA: 0x000C6728 File Offset: 0x000C4B28
	Public Iterator Function dying_cr() As IEnumerator
		Me.StartExplosions()
		Me.isDying = True
		MyBase.transform.rotation = Quaternion.identity
		MyBase.animator.SetTrigger("Dead")
		MyBase.GetComponent(Of Collider2D)().enabled = False
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Death", False, True)
		Dim ghost As BaronessLevelJawbreakerGhost = Global.UnityEngine.[Object].Instantiate(Of BaronessLevelJawbreakerGhost)(Me.ghostPrefab)
		ghost.transform.position = MyBase.transform.position
		Me.Die()
		Return
	End Function

	' Token: 0x0600162A RID: 5674 RVA: 0x000C6744 File Offset: 0x000C4B44
	Private Iterator Function reset_sprite_cr() As IEnumerator
		While True
			Me.sprite.transform.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(0F))
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600162B RID: 5675 RVA: 0x000C675F File Offset: 0x000C4B5F
	Private Sub SoundJawbreakerMouth()
		AudioManager.Play("level_baroness_large_jawbreaker_mouth")
		Me.emitAudioFromObject.Add("level_baroness_large_jawbreaker_mouth")
	End Sub

	' Token: 0x0600162C RID: 5676 RVA: 0x000C677B File Offset: 0x000C4B7B
	Private Sub SoundJawbreakerDeath()
		AudioManager.[Stop]("level_baroness_large_jawbreaker_mouth")
		AudioManager.Play("level_baroness_large_jawbreaker_death")
	End Sub

	' Token: 0x04001F60 RID: 8032
	Private Const ROTATE_FRAME_TIME As Single = 0.083333336F

	' Token: 0x04001F62 RID: 8034
	<SerializeField()>
	Private sprite As Transform

	' Token: 0x04001F63 RID: 8035
	<SerializeField()>
	Private miniBluePrefab As BaronessLevelJawbreakerMini

	' Token: 0x04001F64 RID: 8036
	<SerializeField()>
	Private miniRedPrefab As BaronessLevelJawbreakerMini

	' Token: 0x04001F65 RID: 8037
	<SerializeField()>
	Private followPoint As Transform

	' Token: 0x04001F66 RID: 8038
	<SerializeField()>
	Private ghostPrefab As BaronessLevelJawbreakerGhost

	' Token: 0x04001F67 RID: 8039
	Private prefabsList As List(Of BaronessLevelJawbreakerMini)

	' Token: 0x04001F68 RID: 8040
	Private properties As LevelProperties.Baroness.Jawbreaker

	' Token: 0x04001F69 RID: 8041
	Private player As AbstractPlayerController

	' Token: 0x04001F6A RID: 8042
	Private damageDealer As DamageDealer

	' Token: 0x04001F6B RID: 8043
	Private damageReceiver As DamageReceiver

	' Token: 0x04001F6C RID: 8044
	Private health As Single

	' Token: 0x04001F6D RID: 8045
	Private rotationSpeed As Single

	' Token: 0x04001F6E RID: 8046
	Private lookingLeft As Boolean = True

	' Token: 0x04001F6F RID: 8047
	Private isTurning As Boolean

	' Token: 0x04001F70 RID: 8048
	Private aim As Transform

	' Token: 0x04001F71 RID: 8049
	Private targetPos As Transform

	' Token: 0x04001F72 RID: 8050
	Private spawnPos As Vector3

	' Token: 0x04001F73 RID: 8051
	Private deathPosition As Vector3

	' Token: 0x04001F74 RID: 8052
	Private minisRoutine As Coroutine

	' Token: 0x020004F1 RID: 1265
	Public Enum State
		' Token: 0x04001F76 RID: 8054
		Unspawned
		' Token: 0x04001F77 RID: 8055
		Spawned
		' Token: 0x04001F78 RID: 8056
		Explode
		' Token: 0x04001F79 RID: 8057
		Ghost
	End Enum
End Class
