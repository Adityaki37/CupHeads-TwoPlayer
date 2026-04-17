Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020007C7 RID: 1991
Public Class SaltbakerLevelFeistTurret
	Inherits AbstractCollidableObject

	' Token: 0x1700040D RID: 1037
	' (get) Token: 0x06002D18 RID: 11544 RVA: 0x001A90DD File Offset: 0x001A74DD
	' (set) Token: 0x06002D19 RID: 11545 RVA: 0x001A90E5 File Offset: 0x001A74E5
	Public Property IsActivated As Boolean

	' Token: 0x06002D1A RID: 11546 RVA: 0x001A90F0 File Offset: 0x001A74F0
	Private Sub Start()
		Me.SFX_SALTBAKER_P2_Saltshaker_Appear()
		Me.basePos = MyBase.transform.position
		Me.fxRend.transform.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(CSng(Global.UnityEngine.Random.Range(0, 360))))
		Me.coll.enabled = True
	End Sub

	' Token: 0x06002D1B RID: 11547 RVA: 0x001A9158 File Offset: 0x001A7558
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		If Me.health <= 0F AndAlso Me.IsActivated Then
			Return
		End If
		Me.health -= info.damage
		If Me.health <= 0F Then
			If Me.IsActivated AndAlso Me.parent.phaseTwoStarted Then
				If Not Me.parent.preventAdditionalTurretLaunch Then
					If Me.parent.PreDamagePhaseTwoAndReturnWhetherDoomed(Me.startHealth) Then
						Me.parent.preventAdditionalTurretLaunch = True
					End If
					MyBase.StartCoroutine(Me.fire_and_wait_to_respawn_cr())
				Else
					Me.health = 1F
				End If
			Else
				Me.health = 1F
			End If
		End If
	End Sub

	' Token: 0x06002D1C RID: 11548 RVA: 0x001A921E File Offset: 0x001A761E
	Private Sub FixedUpdate()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.FixedUpdate()
		End If
	End Sub

	' Token: 0x06002D1D RID: 11549 RVA: 0x001A9236 File Offset: 0x001A7636
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06002D1E RID: 11550 RVA: 0x001A9254 File Offset: 0x001A7654
	Public Sub Setup(properties As LevelProperties.Saltbaker.Turrets, parent As SaltbakerLevelSaltbaker, index As Integer)
		Me.properties = properties
		Me.parent = parent
		Me.coll = MyBase.GetComponent(Of Collider2D)()
		Me.sprite = MyBase.GetComponent(Of SpriteRenderer)()
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		Me.index = index
	End Sub

	' Token: 0x06002D1F RID: 11551 RVA: 0x001A92BC File Offset: 0x001A76BC
	Private Sub AniEvent_Activate()
		Me.health = Me.properties.turretHealth
		Me.startHealth = Me.health
		Me.IsActivated = True
		Me.coll.enabled = True
	End Sub

	' Token: 0x06002D20 RID: 11552 RVA: 0x001A92F0 File Offset: 0x001A76F0
	Private Iterator Function fire_and_wait_to_respawn_cr() As IEnumerator
		MyBase.animator.Play("Explode", 1, 0F)
		If Me.shootCR IsNot Nothing Then
			MyBase.StopCoroutine(Me.shootCR)
		End If
		MyBase.transform.position = Me.basePos
		MyBase.animator.ResetTrigger("Shoot")
		Me.coll.enabled = False
		Me.IsActivated = False
		MyBase.transform.localScale = New Vector3(1F, 1F)
		MyBase.animator.Play("Attack" + Me.index)
		Me.SFX_SALTBAKER_P2_Saltshaker_DieLaunch()
		MyBase.animator.Update(0F)
		Yield New WaitForEndOfFrame()
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Attack" + Me.index, False, True)
		Me.sprite.enabled = False
		Yield CupheadTime.WaitForSeconds(Me, Me.properties.respawnTime - 0.75F)
		MyBase.transform.localScale = New Vector3(-Mathf.Sign(MyBase.transform.position.x), 1F)
		Me.fxRend.transform.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(CSng(Global.UnityEngine.Random.Range(0, 360))))
		Me.sprite.sortingLayerName = "Projectiles"
		MyBase.animator.Play("Intro")
		MyBase.animator.Update(0F)
		Me.SFX_SALTBAKER_P2_Saltshaker_Appear()
		Me.sprite.enabled = True
		Return
	End Function

	' Token: 0x06002D21 RID: 11553 RVA: 0x001A930B File Offset: 0x001A770B
	Private Sub AniEvent_DamageSaltbaker()
		Me.SFX_SALTBAKER_P2_Saltshaker_LaunchImpact()
		Me.parent.DamageSaltbaker(Me.startHealth, Me.index)
	End Sub

	' Token: 0x06002D22 RID: 11554 RVA: 0x001A932A File Offset: 0x001A772A
	Public Sub Shoot(isPink As Boolean, warning As Single)
		Me.shootCR = MyBase.StartCoroutine(Me.shoot_cr(isPink, warning))
	End Sub

	' Token: 0x06002D23 RID: 11555 RVA: 0x001A9340 File Offset: 0x001A7740
	Private Iterator Function shoot_cr(isPink As Boolean, warning As Single) As IEnumerator
		Dim upPos As Vector3 = MyBase.transform.position + Vector3.up * CSng(If((MyBase.transform.position.y >= 0F), (-24), 40))
		Me.shootPink = isPink
		MyBase.animator.Play("ShootStart")
		MyBase.animator.Update(0F)
		While MyBase.animator.GetCurrentAnimatorStateInfo(0).IsName("ShootStart")
			MyBase.transform.position = Vector3.Lerp(Me.basePos, upPos, EaseUtils.EaseOutSine(0F, 1F, MyBase.animator.GetCurrentAnimatorStateInfo(0).normalizedTime))
			Yield Nothing
		End While
		MyBase.transform.position = upPos
		If warning > 0.4F Then
			Yield CupheadTime.WaitForSeconds(Me, warning - 0.4F)
			Me.SFX_SALTBAKER_P2_Saltshaker_PreSneeze()
			Yield CupheadTime.WaitForSeconds(Me, 0.4F)
		Else
			Me.SFX_SALTBAKER_P2_Saltshaker_PreSneeze()
			Yield CupheadTime.WaitForSeconds(Me, warning)
		End If
		MyBase.animator.SetTrigger("Shoot")
		Yield MyBase.animator.WaitForAnimationToStart(Me, "ShootEnd", False)
		While MyBase.animator.GetCurrentAnimatorStateInfo(0).IsName("ShootEnd")
			MyBase.transform.position = Vector3.Lerp(upPos, Me.basePos, EaseUtils.EaseInSine(0F, 1F, MyBase.animator.GetCurrentAnimatorStateInfo(0).normalizedTime))
			Yield Nothing
		End While
		MyBase.transform.position = Me.basePos
		Return
	End Function

	' Token: 0x06002D24 RID: 11556 RVA: 0x001A936C File Offset: 0x001A776C
	Private Sub AniEvent_SpawnProjectile()
		Dim num As Single = MathUtils.DirectionToAngle(PlayerManager.GetNext().center - MyBase.transform.position)
		Dim saltbakerLevelTurretBullet As SaltbakerLevelTurretBullet = If(Me.shootPink, Me.pinkBulletPrefab, Me.bulletPrefab)
		saltbakerLevelTurretBullet = saltbakerLevelTurretBullet.Create(Me.sneezeFX.transform.position + MathUtils.AngleToDirection(num) * 150F, num, Me.properties.shotSpeed, Me.parent)
		saltbakerLevelTurretBullet.transform.localScale = MyBase.transform.localScale
		Me.sneezeFX.transform.localScale = MyBase.transform.localScale
		Me.sneezeFX.transform.eulerAngles = New Vector3(0F, 0F, num - 45F)
		Me.SFX_SALTBAKER_P2_Saltshaker_SneezeAttack()
	End Sub

	' Token: 0x06002D25 RID: 11557 RVA: 0x001A945B File Offset: 0x001A785B
	Private Sub AniEvent_BottomLeftTurretSnapForward()
		Me.sprite.sortingLayerName = "Foreground"
	End Sub

	' Token: 0x06002D26 RID: 11558 RVA: 0x001A9470 File Offset: 0x001A7870
	Private Sub LateUpdate()
		Me.pepperText.enabled = False
		Me.pepperTextFlip.enabled = False
		Dim spriteRenderer As SpriteRenderer = If((MyBase.transform.localScale.x <> 1F), Me.pepperTextFlip, Me.pepperText)
		spriteRenderer.enabled = Me.sprite.enabled AndAlso Me.sprite.sprite IsNot Nothing AndAlso Not MyBase.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack0") AndAlso Not MyBase.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1") AndAlso Not MyBase.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2") AndAlso Not MyBase.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack3")
	End Sub

	' Token: 0x06002D27 RID: 11559 RVA: 0x001A956C File Offset: 0x001A796C
	Public Sub Die()
		Me.coll.enabled = False
		Me.sprite.sortingLayerName = "Projectiles"
		Me.StopAllCoroutines()
		MyBase.animator.ResetTrigger("Shoot")
		MyBase.transform.position = Me.basePos
		MyBase.animator.SetBool("Dead", True)
		Me.SFX_SALTBAKER_P2_Saltshaker_Disappear()
	End Sub

	' Token: 0x06002D28 RID: 11560 RVA: 0x001A95D3 File Offset: 0x001A79D3
	Private Sub SFX_SALTBAKER_P2_Saltshaker_Appear()
		AudioManager.Play("sfx_dlc_saltbaker_p2_saltshaker_appear")
		Me.emitAudioFromObject.Add("sfx_dlc_saltbaker_p2_saltshaker_appear")
	End Sub

	' Token: 0x06002D29 RID: 11561 RVA: 0x001A95EF File Offset: 0x001A79EF
	Private Sub SFX_SALTBAKER_P2_Saltshaker_Disappear()
		AudioManager.Play("sfx_dlc_saltbaker_p2_saltshaker_disappear")
		Me.emitAudioFromObject.Add("sfx_dlc_saltbaker_p2_saltshaker_disappear")
	End Sub

	' Token: 0x06002D2A RID: 11562 RVA: 0x001A960B File Offset: 0x001A7A0B
	Private Sub SFX_SALTBAKER_P2_Saltshaker_SneezeAttack()
		AudioManager.Play("sfx_dlc_saltbaker_p2_saltshaker_sneezeattack")
		Me.emitAudioFromObject.Add("sfx_dlc_saltbaker_p2_saltshaker_sneezeattack")
	End Sub

	' Token: 0x06002D2B RID: 11563 RVA: 0x001A9627 File Offset: 0x001A7A27
	Private Sub SFX_SALTBAKER_P2_Saltshaker_PreSneeze()
		AudioManager.Play("sfx_dlc_saltbaker_p2_saltshaker_sneezepre")
		Me.emitAudioFromObject.Add("sfx_dlc_saltbaker_p2_saltshaker_sneezepre")
	End Sub

	' Token: 0x06002D2C RID: 11564 RVA: 0x001A9643 File Offset: 0x001A7A43
	Private Sub SFX_SALTBAKER_P2_Saltshaker_DieLaunch()
		AudioManager.Play("sfx_dlc_saltbaker_p2_saltshaker_dielaunch")
		Me.emitAudioFromObject.Add("sfx_dlc_saltbaker_p2_saltshaker_dielaunch")
	End Sub

	' Token: 0x06002D2D RID: 11565 RVA: 0x001A965F File Offset: 0x001A7A5F
	Private Sub SFX_SALTBAKER_P2_Saltshaker_LaunchImpact()
		AudioManager.Play("sfx_DLC_Saltbaker_P2_Saltshaker_LaunchImpact")
	End Sub

	' Token: 0x04003595 RID: 13717
	<SerializeField()>
	Private bulletPrefab As SaltbakerLevelTurretBullet

	' Token: 0x04003596 RID: 13718
	<SerializeField()>
	Private pinkBulletPrefab As SaltbakerLevelTurretBullet

	' Token: 0x04003597 RID: 13719
	Private properties As LevelProperties.Saltbaker.Turrets

	' Token: 0x04003598 RID: 13720
	Private parent As SaltbakerLevelSaltbaker

	' Token: 0x04003599 RID: 13721
	Private coll As Collider2D

	' Token: 0x0400359A RID: 13722
	Private sprite As SpriteRenderer

	' Token: 0x0400359B RID: 13723
	<SerializeField()>
	Private pepperText As SpriteRenderer

	' Token: 0x0400359C RID: 13724
	<SerializeField()>
	Private pepperTextFlip As SpriteRenderer

	' Token: 0x0400359D RID: 13725
	<SerializeField()>
	Private fxRend As GameObject

	' Token: 0x0400359E RID: 13726
	<SerializeField()>
	Private sneezeFX As GameObject

	' Token: 0x0400359F RID: 13727
	Private damageDealer As DamageDealer

	' Token: 0x040035A0 RID: 13728
	Private damageReceiver As DamageReceiver

	' Token: 0x040035A1 RID: 13729
	Private health As Single

	' Token: 0x040035A2 RID: 13730
	Private startHealth As Single

	' Token: 0x040035A3 RID: 13731
	Private index As Integer

	' Token: 0x040035A4 RID: 13732
	Private shootPink As Boolean

	' Token: 0x040035A5 RID: 13733
	Private shootCR As Coroutine

	' Token: 0x040035A6 RID: 13734
	Private basePos As Vector3
End Class
