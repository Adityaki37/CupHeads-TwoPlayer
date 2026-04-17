Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000612 RID: 1554
Public Class FlowerLevelSeedBullet
	Inherits AbstractProjectile

	' Token: 0x06001F5B RID: 8027 RVA: 0x001201A8 File Offset: 0x0011E5A8
	Public Sub OnBulletSeedStart(parent As FlowerLevelFlower, player As AbstractPlayerController, a As Single, min As Single, max As Single)
		MyBase.transform.LookAt2D(player.transform.position)
		MyBase.transform.Rotate(Vector3.forward, 180F)
		Me.minSpeed = min
		Me.maxSpeed = max
		Me.accelerationTime = a
		Me.player = player
		Me.parent = parent
		AddHandler parent.OnDeathEvent, AddressOf Me.Die
	End Sub

	' Token: 0x06001F5C RID: 8028 RVA: 0x00120218 File Offset: 0x0011E618
	Protected Overrides Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
		MyBase.Update()
	End Sub

	' Token: 0x06001F5D RID: 8029 RVA: 0x00120236 File Offset: 0x0011E636
	Public Sub LaunchBullet()
		MyBase.StartCoroutine(Me.launch_bullet_cr())
	End Sub

	' Token: 0x06001F5E RID: 8030 RVA: 0x00120248 File Offset: 0x0011E648
	Private Iterator Function launch_bullet_cr() As IEnumerator
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		MyBase.transform.LookAt2D(Me.player.transform.position)
		MyBase.transform.Rotate(Vector3.forward, 180F)
		While True
			If Me.timePassed < Me.accelerationTime Then
				Me.timePassed += CupheadTime.FixedDelta
			End If
			If Not Me.isDead Then
				Me.speed = Me.minSpeed + (Me.maxSpeed - Me.minSpeed) * Me.timePassed
			End If
			If Me.speed > 0F AndAlso Not Me.launched Then
				MyBase.animator.SetTrigger("Launch")
				Me.launched = True
				MyBase.StartCoroutine(Me.spawn_effect_cr())
			End If
			MyBase.transform.position -= MyBase.transform.right * (Me.speed * CupheadTime.FixedDelta)
			If MyBase.transform.position.x < CSng((Level.Current.Left - 100)) Then
				Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
			End If
			If MyBase.transform.position.y > CSng((Level.Current.Ceiling + 100)) Then
				Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
			End If
			Yield wait
		End While
		Return
	End Function

	' Token: 0x06001F5F RID: 8031 RVA: 0x00120264 File Offset: 0x0011E664
	Private Iterator Function spawn_effect_cr() As IEnumerator
		Yield MyBase.animator.WaitForAnimationToStart(Me, "Idle", False)
		Dim puff As Effect = Global.UnityEngine.[Object].Instantiate(Of Effect)(Me.puffPrefab)
		puff.transform.position = Me.root.transform.position
		puff.transform.LookAt2D(Me.player.transform.position)
		Return
	End Function

	' Token: 0x06001F60 RID: 8032 RVA: 0x0012027F File Offset: 0x0011E67F
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If Me.damageDealer IsNot Nothing AndAlso phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x06001F61 RID: 8033 RVA: 0x001202A8 File Offset: 0x0011E6A8
	Protected Overrides Sub OnCollisionEnemy(hit As GameObject, phase As CollisionPhase)
		Dim component As FlowerLevelMiniFlowerSpawn = hit.GetComponent(Of FlowerLevelMiniFlowerSpawn)()
		If component IsNot Nothing Then
			component.FriendlyFireDamage()
			Me.Die()
			MyBase.OnCollisionEnemy(hit, phase)
		End If
	End Sub

	' Token: 0x06001F62 RID: 8034 RVA: 0x001202DC File Offset: 0x0011E6DC
	Protected Overrides Sub OnCollisionGround(hit As GameObject, phase As CollisionPhase)
		Me.DeathAudio()
		MyBase.OnCollisionGround(hit, phase)
	End Sub

	' Token: 0x06001F63 RID: 8035 RVA: 0x001202EC File Offset: 0x0011E6EC
	Protected Overrides Sub Die()
		Me.isDead = True
		Me.speed = 0F
		MyBase.GetComponent(Of Collider2D)().enabled = False
		Me.StopAllCoroutines()
		MyBase.Die()
	End Sub

	' Token: 0x06001F64 RID: 8036 RVA: 0x00120318 File Offset: 0x0011E718
	Private Sub DeathAudio()
		AudioManager.Play("flower_bullet_seed_poof")
	End Sub

	' Token: 0x06001F65 RID: 8037 RVA: 0x00120324 File Offset: 0x0011E724
	Protected Overrides Sub OnDestroy()
		RemoveHandler Me.parent.OnDeathEvent, AddressOf Me.Die
		MyBase.OnDestroy()
		Me.puffPrefab = Nothing
	End Sub

	' Token: 0x040027F6 RID: 10230
	<SerializeField()>
	Private puffPrefab As Effect

	' Token: 0x040027F7 RID: 10231
	<SerializeField()>
	Private root As Transform

	' Token: 0x040027F8 RID: 10232
	Private isDead As Boolean

	' Token: 0x040027F9 RID: 10233
	Private launched As Boolean

	' Token: 0x040027FA RID: 10234
	Private speed As Single

	' Token: 0x040027FB RID: 10235
	Private minSpeed As Single

	' Token: 0x040027FC RID: 10236
	Private maxSpeed As Single

	' Token: 0x040027FD RID: 10237
	Private timePassed As Single

	' Token: 0x040027FE RID: 10238
	Private accelerationTime As Single

	' Token: 0x040027FF RID: 10239
	Private parent As FlowerLevelFlower

	' Token: 0x04002800 RID: 10240
	Private player As AbstractPlayerController
End Class
