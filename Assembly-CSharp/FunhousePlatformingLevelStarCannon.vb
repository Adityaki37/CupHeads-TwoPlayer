Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020008BE RID: 2238
Public Class FunhousePlatformingLevelStarCannon
	Inherits PlatformingLevelPathMovementEnemy

	' Token: 0x06003438 RID: 13368 RVA: 0x001E4BDC File Offset: 0x001E2FDC
	Protected Overrides Sub Start()
		MyBase.Start()
		If Me.killable Then
			MyBase._damageReceiver.enabled = False
		End If
		MyBase.animator.SetBool("IsA", Rand.Bool())
		MyBase.StartCoroutine(Me.check_to_start_cr())
	End Sub

	' Token: 0x06003439 RID: 13369 RVA: 0x001E4C28 File Offset: 0x001E3028
	Protected Overrides Sub OnStart()
		MyBase.OnStart()
		MyBase.StartCoroutine(Me.shoot_cr())
	End Sub

	' Token: 0x0600343A RID: 13370 RVA: 0x001E4C40 File Offset: 0x001E3040
	Private Iterator Function check_to_start_cr() As IEnumerator
		While MyBase.transform.position.x > CupheadLevelCamera.Current.Bounds.xMax + Me.offset
			Yield Nothing
		End While
		Me.OnStart()
		Yield Nothing
		Return
	End Function

	' Token: 0x0600343B RID: 13371 RVA: 0x001E4C5C File Offset: 0x001E305C
	Private Iterator Function shoot_cr() As IEnumerator
		While True
			While MyBase.transform.position.x > CupheadLevelCamera.Current.Bounds.xMax + Me.offset OrElse MyBase.transform.position.x < CupheadLevelCamera.Current.Bounds.xMin - Me.offset
				Yield Nothing
			End While
			Yield CupheadTime.WaitForSeconds(Me, MyBase.Properties.cannonShotDelay)
			MyBase.animator.SetBool("isShooting", True)
			While Not Me.justShot
				Yield Nothing
			End While
			Me.justShot = False
			Yield CupheadTime.WaitForSeconds(Me, 0.7F)
			MyBase.animator.SetBool("isShooting", False)
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600343C RID: 13372 RVA: 0x001E4C78 File Offset: 0x001E3078
	Private Sub ShootStraight()
		Me.justShot = True
		AudioManager.Play("funhouse_starcannon_shoot")
		Me.emitAudioFromObject.Add("funhouse_starcannon_shoot")
		Me.StraightFX()
		For i As Integer = 0 To Me.straightRootPositions.Length - 1
			Dim funhousePlatformingLevelCannonProjectile As FunhousePlatformingLevelCannonProjectile = TryCast(Me.projectile.Create(Me.straightRootPositions(i).transform.position, 0F, MyBase.Properties.cannonSpeed), FunhousePlatformingLevelCannonProjectile)
			funhousePlatformingLevelCannonProjectile.direction = Me.straightRootPositions(i).transform.rotation * Vector3.right
			funhousePlatformingLevelCannonProjectile.Properties = MyBase.Properties
			funhousePlatformingLevelCannonProjectile.Init()
		Next
	End Sub

	' Token: 0x0600343D RID: 13373 RVA: 0x001E4D34 File Offset: 0x001E3134
	Private Sub ShootDiag()
		Me.justShot = True
		AudioManager.Play("funhouse_starcannon_shoot")
		Me.emitAudioFromObject.Add("funhouse_starcannon_shoot")
		Me.DiagFX()
		For i As Integer = 0 To Me.diagRootPositions.Length - 1
			Dim funhousePlatformingLevelCannonProjectile As FunhousePlatformingLevelCannonProjectile = TryCast(Me.projectile.Create(Me.diagRootPositions(i).transform.position, 0F, MyBase.Properties.cannonSpeed), FunhousePlatformingLevelCannonProjectile)
			funhousePlatformingLevelCannonProjectile.direction = Me.diagRootPositions(i).transform.rotation * Vector3.right
			funhousePlatformingLevelCannonProjectile.Properties = MyBase.Properties
			funhousePlatformingLevelCannonProjectile.Init()
		Next
	End Sub

	' Token: 0x0600343E RID: 13374 RVA: 0x001E4DED File Offset: 0x001E31ED
	Private Sub DiagFX()
		Me.diagFX.Create(MyBase.transform.position)
	End Sub

	' Token: 0x0600343F RID: 13375 RVA: 0x001E4E06 File Offset: 0x001E3206
	Private Sub StraightFX()
		Me.straightFX.Create(MyBase.transform.position)
	End Sub

	' Token: 0x06003440 RID: 13376 RVA: 0x001E4E1F File Offset: 0x001E321F
	Private Sub SoundCannonRotate()
		AudioManager.Play("funhouse_starcannon_rotation")
		Me.emitAudioFromObject.Add("funhouse_starcannon_rotation")
	End Sub

	' Token: 0x06003441 RID: 13377 RVA: 0x001E4E3B File Offset: 0x001E323B
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.projectile = Nothing
		Me.diagFX = Nothing
		Me.straightFX = Nothing
	End Sub

	' Token: 0x04003C75 RID: 15477
	<SerializeField()>
	Private diagFX As Effect

	' Token: 0x04003C76 RID: 15478
	<SerializeField()>
	Private straightFX As Effect

	' Token: 0x04003C77 RID: 15479
	<SerializeField()>
	Private killable As Boolean

	' Token: 0x04003C78 RID: 15480
	<SerializeField()>
	Private diagRootPositions As Transform()

	' Token: 0x04003C79 RID: 15481
	<SerializeField()>
	Private straightRootPositions As Transform()

	' Token: 0x04003C7A RID: 15482
	<SerializeField()>
	Private projectile As FunhousePlatformingLevelCannonProjectile

	' Token: 0x04003C7B RID: 15483
	Private offset As Single = 50F

	' Token: 0x04003C7C RID: 15484
	Private justShot As Boolean
End Class
