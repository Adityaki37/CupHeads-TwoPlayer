Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000593 RID: 1427
Public Class DevilLevelPitchforkSpinnerProjectile
	Inherits AbstractProjectile

	' Token: 0x17000357 RID: 855
	' (get) Token: 0x06001B53 RID: 6995 RVA: 0x000FA57B File Offset: 0x000F897B
	Protected Overrides ReadOnly Property DestroyLifetime As Single
		Get
			Return -1F
		End Get
	End Property

	' Token: 0x06001B54 RID: 6996 RVA: 0x000FA584 File Offset: 0x000F8984
	Public Function Create(pos As Vector2, maxSpeed As Single, acceleration As Single, homingDuration As Single, parent As DevilLevelSittingDevil, waitTime As Single) As DevilLevelPitchforkSpinnerProjectile
		Dim devilLevelPitchforkSpinnerProjectile As DevilLevelPitchforkSpinnerProjectile = Me.InstantiatePrefab(Of DevilLevelPitchforkSpinnerProjectile)()
		devilLevelPitchforkSpinnerProjectile.transform.position = pos
		devilLevelPitchforkSpinnerProjectile.homingDuration = homingDuration
		devilLevelPitchforkSpinnerProjectile.startingY = pos.y
		devilLevelPitchforkSpinnerProjectile.parent = parent
		devilLevelPitchforkSpinnerProjectile.waitTime = waitTime
		devilLevelPitchforkSpinnerProjectile.homingMaxSpeed = maxSpeed
		devilLevelPitchforkSpinnerProjectile.homingAcceleration = acceleration
		devilLevelPitchforkSpinnerProjectile.StartCoroutine(devilLevelPitchforkSpinnerProjectile.main_cr())
		devilLevelPitchforkSpinnerProjectile.SetParryable(True)
		devilLevelPitchforkSpinnerProjectile.animator.SetBool("IsPink", True)
		devilLevelPitchforkSpinnerProjectile.OrbitStartSFX()
		Return devilLevelPitchforkSpinnerProjectile
	End Function

	' Token: 0x06001B55 RID: 6997 RVA: 0x000FA608 File Offset: 0x000F8A08
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.parent Is Nothing Then
			Me.Die()
		End If
	End Sub

	' Token: 0x06001B56 RID: 6998 RVA: 0x000FA627 File Offset: 0x000F8A27
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x06001B57 RID: 6999 RVA: 0x000FA648 File Offset: 0x000F8A48
	Protected Overrides Sub FixedUpdate()
		If Not Me.waitTimeUp Then
			Return
		End If
		Me.t += CupheadTime.FixedDelta
		MyBase.transform.SetPosition(Nothing, New Single?(Me.startingY + Mathf.Sin(Me.t * 3.1415927F * 2F / 1.5F) * 10F), Nothing)
		If Mathf.Abs(MyBase.transform.position.x) > 1500F Then
			Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		End If
		MyBase.Update()
	End Sub

	' Token: 0x06001B58 RID: 7000 RVA: 0x000FA6F4 File Offset: 0x000F8AF4
	Private Iterator Function main_cr() As IEnumerator
		MyBase.GetComponent(Of Collider2D)().enabled = False
		MyBase.GetComponent(Of GroundHomingMovement)().EnableHoming = False
		Yield CupheadTime.WaitForSeconds(Me, Me.waitTime)
		Me.waitTimeUp = True
		MyBase.animator.SetTrigger("Continue")
		MyBase.animator.SetBool("StartAtHalf", Rand.Bool())
		Dim homingMovement As GroundHomingMovement = MyBase.GetComponent(Of GroundHomingMovement)()
		homingMovement.maxSpeed = Me.homingMaxSpeed
		homingMovement.acceleration = Me.homingAcceleration
		homingMovement.bounceEnabled = False
		homingMovement.destroyOffScreen = False
		homingMovement.TrackingPlayer = PlayerManager.GetNext()
		homingMovement.EnableHoming = False
		MyBase.GetComponent(Of Collider2D)().enabled = True
		MyBase.GetComponent(Of GroundHomingMovement)().EnableHoming = True
		Yield CupheadTime.WaitForSeconds(Me, Me.homingDuration)
		MyBase.GetComponent(Of GroundHomingMovement)().EnableHoming = False
		Return
	End Function

	' Token: 0x06001B59 RID: 7001 RVA: 0x000FA70F File Offset: 0x000F8B0F
	Protected Overrides Sub Die()
		MyBase.Die()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Me.OrbitStopSFX()
	End Sub

	' Token: 0x06001B5A RID: 7002 RVA: 0x000FA728 File Offset: 0x000F8B28
	Public Overrides Sub OnParry(player As AbstractPlayerController)
		MyBase.GetComponent(Of Collider2D)().enabled = False
		MyBase.GetComponent(Of SpriteRenderer)().enabled = False
	End Sub

	' Token: 0x06001B5B RID: 7003 RVA: 0x000FA742 File Offset: 0x000F8B42
	Private Sub OrbitStartSFX()
		If Not Me.SpinSFXPlaying Then
			AudioManager.PlayLoop("devil_orbit_projectile")
			Me.emitAudioFromObject.Add("devil_orbit_projectile")
			Me.SpinSFXPlaying = True
		End If
	End Sub

	' Token: 0x06001B5C RID: 7004 RVA: 0x000FA770 File Offset: 0x000F8B70
	Private Sub OrbitStopSFX()
		AudioManager.[Stop]("devil_orbit_projectile")
		Me.SpinSFXPlaying = False
	End Sub

	' Token: 0x0400248D RID: 9357
	Private Const SIN_HEIGHT As Single = 10F

	' Token: 0x0400248E RID: 9358
	Private Const SIN_PERIOD As Single = 1.5F

	' Token: 0x0400248F RID: 9359
	Private Const DESTROY_X As Single = 1500F

	' Token: 0x04002490 RID: 9360
	Private waitTime As Single

	' Token: 0x04002491 RID: 9361
	Private homingDuration As Single

	' Token: 0x04002492 RID: 9362
	Private homingMaxSpeed As Single

	' Token: 0x04002493 RID: 9363
	Private homingAcceleration As Single

	' Token: 0x04002494 RID: 9364
	Private startingY As Single

	' Token: 0x04002495 RID: 9365
	Private t As Single

	' Token: 0x04002496 RID: 9366
	Private waitTimeUp As Boolean

	' Token: 0x04002497 RID: 9367
	Private SpinSFXPlaying As Boolean

	' Token: 0x04002498 RID: 9368
	Private parent As DevilLevelSittingDevil
End Class
