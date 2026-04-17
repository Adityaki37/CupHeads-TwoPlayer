Imports System
Imports UnityEngine

' Token: 0x020004C9 RID: 1225
Public Class AirplaneLevelTurretDog
	Inherits AbstractPausableComponent

	' Token: 0x060014BC RID: 5308 RVA: 0x000BA1F4 File Offset: 0x000B85F4
	Private Sub ShootProjectile()
		Me.FX.Create(MyBase.transform.position, MyBase.transform.localScale)
		Dim vector As Vector3 = New Vector3(Me.rootPos.position.x + 30F, Me.rootPos.position.y)
		Dim vector2 As Vector3 = New Vector3(Me.rootPos.position.x - 30F, Me.rootPos.position.y)
		Dim airplaneLevelTurretBullet As AirplaneLevelTurretBullet = Me.bulletPrefab.Create(vector, New Vector3(Me.velocityX, Me.velocityY), Me.gravity)
		airplaneLevelTurretBullet.GetComponent(Of SpriteRenderer)().sortingOrder = 1
		airplaneLevelTurretBullet.GetComponent(Of Animator)().Play("TennisBallA")
		Dim airplaneLevelTurretBullet2 As AirplaneLevelTurretBullet = Me.bulletPrefab.Create(Me.rootPos.position, New Vector3(0F, Me.velocityY), Me.gravity)
		airplaneLevelTurretBullet2.GetComponent(Of SpriteRenderer)().sortingOrder = 2
		airplaneLevelTurretBullet2.GetComponent(Of Animator)().Play("TennisBallB")
		Dim airplaneLevelTurretBullet3 As AirplaneLevelTurretBullet = Me.bulletPrefab.Create(vector2, New Vector3(-Me.velocityX, Me.velocityY), Me.gravity)
		airplaneLevelTurretBullet3.GetComponent(Of SpriteRenderer)().sortingOrder = 1
		airplaneLevelTurretBullet3.GetComponent(Of Animator)().Play("TennisBallC")
	End Sub

	' Token: 0x060014BD RID: 5309 RVA: 0x000BA37C File Offset: 0x000B877C
	Public Sub StartAttack(velocityX As Single, velocityY As Single, gravity As Single)
		MyBase.animator.Play("Flap")
		Me.velocityX = velocityX
		Me.velocityY = velocityY
		Me.gravity = gravity
	End Sub

	' Token: 0x060014BE RID: 5310 RVA: 0x000BA3A3 File Offset: 0x000B87A3
	Private Sub AnimationEvent_SFX_DOGFIGHT_BulldogPlane_TurretDogHatchOpen()
		AudioManager.Play("sfx_dlc_dogfight_p1_terrierplane_hatchopen")
	End Sub

	' Token: 0x060014BF RID: 5311 RVA: 0x000BA3AF File Offset: 0x000B87AF
	Private Sub AnimationEvent_SFX_DOGFIGHT_BulldogPlane_TurretDogBark()
		AudioManager.Play("sfx_dlc_dogfight_p1_terrierplane_bark")
	End Sub

	' Token: 0x060014C0 RID: 5312 RVA: 0x000BA3BB File Offset: 0x000B87BB
	Private Sub AnimationEvent_SFX_DOGFIGHT_BulldogPlane_TurretDogWhistle()
		AudioManager.Play("sfx_dlc_dogfight_p1_terrierplane_baseball_whistle")
	End Sub

	' Token: 0x060014C1 RID: 5313 RVA: 0x000BA3C7 File Offset: 0x000B87C7
	Private Sub AnimationEvent_SFX_DOGFIGHT_BulldogPlane_TurretDogToss()
		AudioManager.Play("sfx_dlc_dogfight_p1_terrierplane_baseball_toss")
	End Sub

	' Token: 0x04001E29 RID: 7721
	Private Const BALL_OFFSET As Single = 30F

	' Token: 0x04001E2A RID: 7722
	<SerializeField()>
	Private bulletPrefab As AirplaneLevelTurretBullet

	' Token: 0x04001E2B RID: 7723
	<SerializeField()>
	Private rootPos As Transform

	' Token: 0x04001E2C RID: 7724
	<SerializeField()>
	Private FX As Effect

	' Token: 0x04001E2D RID: 7725
	Private velocityX As Single

	' Token: 0x04001E2E RID: 7726
	Private velocityY As Single

	' Token: 0x04001E2F RID: 7727
	Private gravity As Single
End Class
