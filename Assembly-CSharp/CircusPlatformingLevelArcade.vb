Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000899 RID: 2201
Public Class CircusPlatformingLevelArcade
	Inherits AbstractPlatformingLevelEnemy

	' Token: 0x0600332F RID: 13103 RVA: 0x001DCB07 File Offset: 0x001DAF07
	Protected Overrides Sub OnStart()
		MyBase.StartCoroutine(Me.shoot_cr())
		Me.goingRight = Rand.Bool()
	End Sub

	' Token: 0x06003330 RID: 13104 RVA: 0x001DCB21 File Offset: 0x001DAF21
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.StartCoroutine(Me.check_to_start_cr())
	End Sub

	' Token: 0x06003331 RID: 13105 RVA: 0x001DCB38 File Offset: 0x001DAF38
	Private Iterator Function check_to_start_cr() As IEnumerator
		While MyBase.transform.position.x > CupheadLevelCamera.Current.Bounds.xMax + Me.offset
			Yield Nothing
		End While
		Me.OnStart()
		Yield Nothing
		Return
	End Function

	' Token: 0x06003332 RID: 13106 RVA: 0x001DCB54 File Offset: 0x001DAF54
	Private Iterator Function shoot_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, MyBase.Properties.arcadeAttackDelayInit.RandomFloat())
		While True
			While MyBase.transform.position.x > CupheadLevelCamera.Current.Bounds.xMax + Me.offset OrElse MyBase.transform.position.x < CupheadLevelCamera.Current.Bounds.xMin - Me.offset
				Yield Nothing
			End While
			MyBase.animator.SetBool("IsAttacking", True)
			Me.isAttacking = True
			While Me.isAttacking
				Yield Nothing
			End While
			Yield CupheadTime.WaitForSeconds(Me, MyBase.Properties.arcadeAttackDelay.RandomFloat())
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06003333 RID: 13107 RVA: 0x001DCB70 File Offset: 0x001DAF70
	Private Sub Shoot()
		Me.goingRight = Not Me.goingRight
		Me.introBulletInstance = Global.UnityEngine.[Object].Instantiate(Of Transform)(Me.introBullet)
		MyBase.StartCoroutine(Me.shoot_intro_cr())
		MyBase.animator.SetBool("IsAttacking", False)
		MyBase.StartCoroutine(Me.drop_cr())
	End Sub

	' Token: 0x06003334 RID: 13108 RVA: 0x001DCBC8 File Offset: 0x001DAFC8
	Private Iterator Function shoot_intro_cr() As IEnumerator
		While Me.introBulletInstance.position.y < CSng(Level.Current.Ceiling) + 100F
			Me.introBulletInstance.position += Vector3.up * MyBase.Properties.arcadeBulletSpeed * CupheadTime.Delta
			Yield Nothing
		End While
		Global.UnityEngine.[Object].Destroy(Me.introBulletInstance.gameObject)
		Return
	End Function

	' Token: 0x06003335 RID: 13109 RVA: 0x001DCBE4 File Offset: 0x001DAFE4
	Private Iterator Function drop_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, MyBase.Properties.arcadeBulletReturnDelay)
		Dim player As AbstractPlayerController = PlayerManager.GetNext()
		Dim sizeX As Single = 100F
		Dim posX As Single = If((Not Me.goingRight), Me.bulletSpawnB.transform.position.x, Me.bulletSpawnA.transform.position.x)
		For i As Integer = 0 To MyBase.Properties.arcadeBulletCount - 1
			If player Is Nothing Then
				player = PlayerManager.GetNext()
			End If
			Yield Nothing
			Me.bullet.Create(New Vector2(If((Not Me.goingRight), (posX - sizeX * CSng(i)), (posX + sizeX * CSng(i))), CupheadLevelCamera.Current.Bounds.yMax + 50F), -90F, MyBase.Properties.arcadeBulletSpeed)
			Yield CupheadTime.WaitForSeconds(Me, MyBase.Properties.arcadeBulletIndividualDelay)
		Next
		Me.isAttacking = False
		Return
	End Function

	' Token: 0x06003336 RID: 13110 RVA: 0x001DCC00 File Offset: 0x001DB000
	Protected Overrides Sub Die()
		AudioManager.Play("circus_arcade_death")
		Me.emitAudioFromObject.Add("circus_arcade_death")
		MyBase.animator.Play("Death")
		Me.effect.Create(MyBase.transform.position)
		Me.StopAllCoroutines()
		If Me.introBulletInstance IsNot Nothing Then
			Global.UnityEngine.[Object].Destroy(Me.introBulletInstance.gameObject)
		End If
		MyBase.StartCoroutine(Me.Explosion_cr())
		MyBase.GetComponent(Of Collider2D)().enabled = False
	End Sub

	' Token: 0x06003337 RID: 13111 RVA: 0x001DCC90 File Offset: 0x001DB090
	Private Iterator Function Explosion_cr() As IEnumerator
		Me.exploder.StartExplosion()
		Yield New WaitForSeconds(2.5F)
		Me.exploder.StopExplosions()
		Return
	End Function

	' Token: 0x06003338 RID: 13112 RVA: 0x001DCCAB File Offset: 0x001DB0AB
	Private Sub AttackSFX()
		AudioManager.Play("circus_arcade_attack")
		Me.emitAudioFromObject.Add("circus_arcade_attack")
	End Sub

	' Token: 0x06003339 RID: 13113 RVA: 0x001DCCC7 File Offset: 0x001DB0C7
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		Gizmos.DrawWireSphere(Me.bulletSpawnA.position, 50F)
		Gizmos.color = Color.blue
		Gizmos.DrawWireSphere(Me.bulletSpawnB.position, 50F)
	End Sub

	' Token: 0x0600333A RID: 13114 RVA: 0x001DCD03 File Offset: 0x001DB103
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.effect = Nothing
		Me.bullet = Nothing
		Me.introBullet = Nothing
		Me.introBulletInstance = Nothing
	End Sub

	' Token: 0x04003B71 RID: 15217
	<SerializeField()>
	Private bulletSpawnA As Transform

	' Token: 0x04003B72 RID: 15218
	<SerializeField()>
	Private bulletSpawnB As Transform

	' Token: 0x04003B73 RID: 15219
	<SerializeField()>
	Private effect As Effect

	' Token: 0x04003B74 RID: 15220
	<SerializeField()>
	Private arcadeRoot As Transform

	' Token: 0x04003B75 RID: 15221
	<SerializeField()>
	Private introBullet As Transform

	' Token: 0x04003B76 RID: 15222
	<SerializeField()>
	Private bullet As BasicProjectile

	' Token: 0x04003B77 RID: 15223
	<SerializeField()>
	Private exploder As LevelBossDeathExploder

	' Token: 0x04003B78 RID: 15224
	Private offset As Single = 50F

	' Token: 0x04003B79 RID: 15225
	Private isAttacking As Boolean

	' Token: 0x04003B7A RID: 15226
	Private goingRight As Boolean

	' Token: 0x04003B7B RID: 15227
	Private introBulletInstance As Transform
End Class
