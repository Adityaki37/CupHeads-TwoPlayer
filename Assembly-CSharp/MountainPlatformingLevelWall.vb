Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020008F2 RID: 2290
Public Class MountainPlatformingLevelWall
	Inherits AbstractPlatformingLevelEnemy

	' Token: 0x060035AC RID: 13740 RVA: 0x001F49D5 File Offset: 0x001F2DD5
	Protected Overrides Sub OnStart()
	End Sub

	' Token: 0x060035AD RID: 13741 RVA: 0x001F49D8 File Offset: 0x001F2DD8
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.StartCoroutine(Me.move_cr())
		MyBase.GetComponent(Of Collider2D)().enabled = False
		Me.head.GetComponent(Of Collider2D)().enabled = False
		Me.platform.gameObject.SetActive(False)
		MyBase.GetComponent(Of DamageReceiver)().enabled = False
		AddHandler Me.head.GetComponent(Of DamageReceiver)().OnDamageTaken, AddressOf Me.OnDamageTaken
		Me.head.gameObject.tag = "Enemy"
		Dim component As ParrySwitch = Me.head.GetComponent(Of ParrySwitch)()
		AddHandler component.OnActivate, AddressOf component.StartParryCooldown
	End Sub

	' Token: 0x060035AE RID: 13742 RVA: 0x001F4A82 File Offset: 0x001F2E82
	Private Sub FaceOn()
		MyBase.animator.Play("Face_Idle")
		MyBase.animator.Play("Shield_Idle")
	End Sub

	' Token: 0x060035AF RID: 13743 RVA: 0x001F4AA4 File Offset: 0x001F2EA4
	Private Iterator Function move_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 0.2F)
		Dim player As AbstractPlayerController = PlayerManager.GetNext()
		While player.transform.position.x < Me.startTrigger.transform.position.x
			Yield Nothing
			If player Is Nothing OrElse player.IsDead Then
				player = PlayerManager.GetNext()
			End If
		End While
		MyBase.GetComponent(Of Collider2D)().enabled = True
		Me.head.GetComponent(Of Collider2D)().enabled = True
		Me.platform.gameObject.SetActive(True)
		MyBase.animator.SetTrigger("OnIntro")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Wall_Intro", False, True)
		MyBase.StartCoroutine(Me.shoot_cr())
		Dim t As Single = 0F
		Dim time As Single = MyBase.Properties.wallFaceTravelTime
		Dim movingUp As Boolean = False
		Dim top As Single = Me.head.transform.position.y + 100F
		Dim bottom As Single = Me.head.transform.position.y - 100F
		Dim start As Single = Me.head.transform.position.y
		Dim [end] As Single = 0F
		While True
			start = Me.head.transform.position.y
			If movingUp Then
				[end] = top
			Else
				[end] = bottom
			End If
			While t < time
				Dim val As Single = t / time
				Me.head.transform.SetPosition(Nothing, New Single?(EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, start, [end], val)), Nothing)
				t += CupheadTime.Delta
				Yield Nothing
			End While
			Me.head.transform.SetPosition(Nothing, New Single?([end]), Nothing)
			movingUp = Not movingUp
			t = 0F
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060035B0 RID: 13744 RVA: 0x001F4AC0 File Offset: 0x001F2EC0
	Private Iterator Function shoot_cr() As IEnumerator
		While True
			Yield CupheadTime.WaitForSeconds(Me, MyBase.Properties.wallAttackDelay.RandomFloat())
			MyBase.animator.SetTrigger("Attack")
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060035B1 RID: 13745 RVA: 0x001F4ADC File Offset: 0x001F2EDC
	Private Sub ShootProjectileEffect()
		If Me.projectileCount = 2 Then
			Me.projectilePinkEffect.Create(New Vector3(Me.projectileRoot.transform.position.x - 20F, Me.projectileRoot.transform.position.y))
		Else
			Me.projectileEffect.Create(New Vector3(Me.projectileRoot.transform.position.x - 20F, Me.projectileRoot.transform.position.y))
		End If
	End Sub

	' Token: 0x060035B2 RID: 13746 RVA: 0x001F4B88 File Offset: 0x001F2F88
	Private Sub ShootProjectile()
		If Me.projectileCount = 2 Then
			Me.projectileCount = 0
			Me.bouncyPinkProjectile.Create(Me.projectileRoot.position, 0F, New Vector2(-MyBase.Properties.wallProjectileXSpeed, MyBase.Properties.wallProjectileYSpeed), MyBase.Properties.wallProjectileGravity, Me.groundPosY.position.y)
		Else
			Me.projectileCount += 1
			Me.bouncyProjectile.Create(Me.projectileRoot.position, 0F, New Vector2(-MyBase.Properties.wallProjectileXSpeed, MyBase.Properties.wallProjectileYSpeed), MyBase.Properties.wallProjectileGravity, Me.groundPosY.position.y)
		End If
	End Sub

	' Token: 0x060035B3 RID: 13747 RVA: 0x001F4C74 File Offset: 0x001F3074
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		Gizmos.color = New Color(0F, 0F, 1F, 1F)
		Gizmos.DrawLine(Me.startTrigger.transform.position, New Vector3(Me.startTrigger.transform.position.x, 5000F, 0F))
	End Sub

	' Token: 0x060035B4 RID: 13748 RVA: 0x001F4CE4 File Offset: 0x001F30E4
	Protected Overrides Sub Die()
		Me.StopAllCoroutines()
		Me.head.GetComponent(Of Collider2D)().enabled = False
		MyBase.animator.SetTrigger("OnDeath")
		MyBase.StartCoroutine(Me.dying_cr())
		MyBase.StartCoroutine(Me.death_shake_cr())
		MyBase.StartCoroutine(Me.create_explosions_cr())
	End Sub

	' Token: 0x060035B5 RID: 13749 RVA: 0x001F4D3F File Offset: 0x001F313F
	Private Sub FaceDead()
		MyBase.animator.Play("Face_Death_Loop")
	End Sub

	' Token: 0x060035B6 RID: 13750 RVA: 0x001F4D54 File Offset: 0x001F3154
	Private Iterator Function death_shake_cr() As IEnumerator
		Dim movingUp As Boolean = False
		Dim top As Single = MyBase.transform.position.y + 4F
		Dim bottom As Single = MyBase.transform.position.y - 4F
		Dim start As Single = MyBase.transform.position.y
		Dim [end] As Single = 0F
		Dim t As Single = 0F
		Dim time As Single = 0.01F
		While Not Me.isDead
			start = MyBase.transform.position.y
			If movingUp Then
				[end] = top
			Else
				[end] = bottom
			End If
			While t < time
				Dim val As Single = t / time
				MyBase.transform.SetPosition(Nothing, New Single?(EaseUtils.Ease(EaseUtils.EaseType.easeInOutBounce, start, [end], val)), Nothing)
				t += CupheadTime.Delta
				Yield Nothing
			End While
			MyBase.transform.SetPosition(Nothing, New Single?([end]), Nothing)
			movingUp = Not movingUp
			t = 0F
			Yield Nothing
		End While
		Yield Nothing
		Return
	End Function

	' Token: 0x060035B7 RID: 13751 RVA: 0x001F4D70 File Offset: 0x001F3170
	Private Iterator Function create_explosions_cr() As IEnumerator
		While Not Me.isDead
			MyBase.GetComponent(Of EffectRadius)().CreateInRadius()
			Yield CupheadTime.WaitForSeconds(Me, Global.UnityEngine.Random.Range(0.2F, 0.4F))
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060035B8 RID: 13752 RVA: 0x001F4D8C File Offset: 0x001F318C
	Private Iterator Function dying_cr() As IEnumerator
		AudioManager.Play("castle_mountain_wall_death")
		Me.emitAudioFromObject.Add("castle_mountain_wall_death")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Wall_Death", False, True)
		Yield CupheadTime.WaitForSeconds(Me, 1.67F)
		Dim t As Single = 0F
		Dim time As Single = 0.65F
		While t < time
			MyBase.GetComponent(Of SpriteRenderer)().color = New Color(1F, 1F, 1F, 1F - t / time)
			Me.head.GetComponent(Of SpriteRenderer)().color = New Color(1F, 1F, 1F, 1F - t / time)
			Me.shield.GetComponent(Of SpriteRenderer)().color = New Color(1F, 1F, 1F, 1F - t / time)
			Me.foreground1.GetComponent(Of SpriteRenderer)().color = New Color(1F, 1F, 1F, 1F - t / time)
			Me.foreground2.GetComponent(Of SpriteRenderer)().color = New Color(1F, 1F, 1F, 1F - t / time)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		Me.isDead = True
		MyBase.Die()
		Yield Nothing
		Return
	End Function

	' Token: 0x060035B9 RID: 13753 RVA: 0x001F4DA7 File Offset: 0x001F31A7
	Private Sub SoundMountainWallShoot()
		AudioManager.Play("castle_mountain_wall_attack")
		Me.emitAudioFromObject.Add("castle_mountain_wall_attack")
	End Sub

	' Token: 0x060035BA RID: 13754 RVA: 0x001F4DC3 File Offset: 0x001F31C3
	Private Sub SoundMountainWallIntro()
		AudioManager.Play("castle_mountain_wall_spawn")
		Me.emitAudioFromObject.Add("castle_mountain_wall_spawn")
	End Sub

	' Token: 0x060035BB RID: 13755 RVA: 0x001F4DDF File Offset: 0x001F31DF
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.projectileEffect = Nothing
		Me.projectilePinkEffect = Nothing
		Me.bouncyPinkProjectile = Nothing
		Me.bouncyProjectile = Nothing
	End Sub

	' Token: 0x04003DC4 RID: 15812
	<SerializeField()>
	Private groundPosY As Transform

	' Token: 0x04003DC5 RID: 15813
	<SerializeField()>
	Private platform As Transform

	' Token: 0x04003DC6 RID: 15814
	<SerializeField()>
	Private foreground1 As SpriteRenderer

	' Token: 0x04003DC7 RID: 15815
	<SerializeField()>
	Private foreground2 As SpriteRenderer

	' Token: 0x04003DC8 RID: 15816
	<SerializeField()>
	Private shield As SpriteRenderer

	' Token: 0x04003DC9 RID: 15817
	<SerializeField()>
	Private head As Transform

	' Token: 0x04003DCA RID: 15818
	<SerializeField()>
	Private startTrigger As Transform

	' Token: 0x04003DCB RID: 15819
	<SerializeField()>
	Private projectileRoot As Transform

	' Token: 0x04003DCC RID: 15820
	<SerializeField()>
	Private projectileEffect As Effect

	' Token: 0x04003DCD RID: 15821
	<SerializeField()>
	Private projectilePinkEffect As Effect

	' Token: 0x04003DCE RID: 15822
	<SerializeField()>
	Private bouncyProjectile As MountainPlatformingLevelWallProjectile

	' Token: 0x04003DCF RID: 15823
	<SerializeField()>
	Private bouncyPinkProjectile As MountainPlatformingLevelWallProjectile

	' Token: 0x04003DD0 RID: 15824
	Private projectileCount As Integer

	' Token: 0x04003DD1 RID: 15825
	Private isDead As Boolean
End Class
