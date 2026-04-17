Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200064B RID: 1611
Public Class FlyingCowboyLevelBird
	Inherits AbstractProjectile

	' Token: 0x0600211C RID: 8476 RVA: 0x0013219F File Offset: 0x0013059F
	Public Sub Initialize(startPosition As Vector3, endPosition As Vector3, bulletLandingPosition As Single, properties As LevelProperties.FlyingCowboy.Bird, cowgirl As FlyingCowboyLevelCowboy)
		Me.bulletLandingPosition = bulletLandingPosition
		Me.properties = properties
		Me.cowgirl = cowgirl
		MyBase.StartCoroutine(Me.move_cr(startPosition, endPosition, properties))
		MyBase.StartCoroutine(Me.attack_cr())
	End Sub

	' Token: 0x0600211D RID: 8477 RVA: 0x001321D8 File Offset: 0x001305D8
	Public Sub InitializeIntro(startPosition As Vector3)
		MyBase.transform.position = startPosition
		Dim component As SpriteRenderer = MyBase.GetComponent(Of SpriteRenderer)()
		component.sortingLayerName = "Default"
		component.sortingOrder = -120
		MyBase.animator.Play("Return")
	End Sub

	' Token: 0x0600211E RID: 8478 RVA: 0x0013221B File Offset: 0x0013061B
	Public Sub MoveIntro(endPosition As Vector3, properties As LevelProperties.FlyingCowboy.Bird)
		MyBase.StartCoroutine(Me.moveIntro_cr(endPosition, properties))
	End Sub

	' Token: 0x0600211F RID: 8479 RVA: 0x0013222C File Offset: 0x0013062C
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06002120 RID: 8480 RVA: 0x0013224C File Offset: 0x0013064C
	Private Sub move()
		Dim position As Vector3 = MyBase.transform.position
		position.x += Me.properties.speed * CupheadTime.FixedDelta
		MyBase.transform.position = position
	End Sub

	' Token: 0x06002121 RID: 8481 RVA: 0x00132290 File Offset: 0x00130690
	Private Iterator Function move_cr(startPosition As Vector3, endPosition As Vector3, properties As LevelProperties.FlyingCowboy.Bird) As IEnumerator
		Dim wait As WaitForFixedUpdate = New WaitForFixedUpdate()
		While MyBase.transform.position.x < endPosition.x - 60F
			Yield wait
			Me.move()
		End While
		While MyBase.animator.GetCurrentAnimatorStateInfo(1).IsName("Throw")
			Yield wait
			Me.move()
		End While
		Dim normalizedTime As Single = MyBase.animator.GetCurrentAnimatorStateInfo(0).normalizedTime
		While normalizedTime >= 0.18181819F AndAlso normalizedTime <= 0.8181818F
			Yield wait
			Me.move()
			normalizedTime = MyBase.animator.GetCurrentAnimatorStateInfo(0).normalizedTime
		End While
		MyBase.animator.Play("Turn")
		Dim slowdownTime As Single = KinematicUtilities.CalculateTimeToChangeVelocity(properties.speed, 0F, 60F)
		Dim elapsedTime As Single = 0F
		While elapsedTime < slowdownTime
			Yield wait
			elapsedTime += CupheadTime.FixedDelta
			Dim position As Vector3 = MyBase.transform.position
			position.x += Mathf.Lerp(properties.speed, 0F, elapsedTime / slowdownTime) * CupheadTime.FixedDelta
			MyBase.transform.position = position
		End While
		Yield MyBase.animator.WaitForNormalizedTime(Me, 0.75F, "Turn", 0, False, False, True)
		elapsedTime = 0F
		While MyBase.transform.position.x > startPosition.x
			Yield wait
			elapsedTime += CupheadTime.FixedDelta
			Dim speed As Single = Mathf.Lerp(0F, properties.speed, elapsedTime / 0.25F)
			Dim position2 As Vector3 = MyBase.transform.position
			position2.x -= properties.speed * CupheadTime.FixedDelta
			MyBase.transform.position = position2
		End While
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Return
	End Function

	' Token: 0x06002122 RID: 8482 RVA: 0x001322C0 File Offset: 0x001306C0
	Private Iterator Function moveIntro_cr(endPosition As Vector3, properties As LevelProperties.FlyingCowboy.Bird) As IEnumerator
		Dim wait As WaitForFixedUpdate = New WaitForFixedUpdate()
		While MyBase.transform.position.x > endPosition.x
			Yield wait
			Dim position As Vector3 = MyBase.transform.position
			position.x -= properties.speed * CupheadTime.FixedDelta
			MyBase.transform.position = position
		End While
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Return
	End Function

	' Token: 0x06002123 RID: 8483 RVA: 0x001322EC File Offset: 0x001306EC
	Private Iterator Function attack_cr() As IEnumerator
		If Me.bulletLandingPosition > -400F Then
			Return
		End If
		While MyBase.transform.position.x < -385F
			Yield Nothing
		End While
		If Me.projectileSpawned OrElse MyBase.animator.GetCurrentAnimatorStateInfo(0).IsName("Turn") Then
			Return
		End If
		MyBase.animator.RoundFrame(0)
		MyBase.animator.Play("Throw", 1)
		Yield MyBase.animator.WaitForNormalizedTime(Me, 1F, "Throw", 1, True, False, True)
		MyBase.animator.Play("Off", 1)
		Me.holdingFeetRenderer.enabled = False
		Me.emptyFeetRenderer.enabled = True
		Return
	End Function

	' Token: 0x06002124 RID: 8484 RVA: 0x00132308 File Offset: 0x00130708
	Private Sub spawnProjectile()
		If Me.projectileSpawned Then
			Return
		End If
		Me.projectileSpawned = True
		Me.SFX_COWGIRL_COWGIRL_P1_BirdCall()
		Dim position As Vector3 = Me.projectileSpawnPoint.position
		Dim num As Single = KinematicUtilities.CalculateInitialSpeedToReachApex(Me.properties.bulletArcHeight, Me.properties.bulletGravity)
		Dim num2 As Single = Me.bulletLandingPosition - position.x
		Dim num3 As Single = KinematicUtilities.CalculateHorizontalSpeedToTravelDistance(num2, num, position.y - FlyingCowboyLevelBirdProjectile.HighLandingPosition, Me.properties.bulletGravity)
		Dim vector As Vector2 = New Vector2(num3, num)
		Dim num4 As Single = Mathf.Atan2(vector.y, vector.x) * 57.29578F
		Dim flyingCowboyLevelBirdProjectile As FlyingCowboyLevelBirdProjectile = TryCast(Me.projectilePrefab.Create(Me.projectileSpawnPoint.position), FlyingCowboyLevelBirdProjectile)
		flyingCowboyLevelBirdProjectile.Initialize(vector, Me.properties.bulletGravity, Me.properties.shrapnelSecondStageDelay, Me.properties.shrapnelSpeed, Me.properties.shrapnelSpreadAngle, Me.cowgirl)
		flyingCowboyLevelBirdProjectile.shrapnelCount = Me.properties.shrapnelCount
	End Sub

	' Token: 0x06002125 RID: 8485 RVA: 0x00132418 File Offset: 0x00130818
	Private Sub animationEvent_SpawnProjectile()
		Me.spawnProjectile()
	End Sub

	' Token: 0x06002126 RID: 8486 RVA: 0x00132420 File Offset: 0x00130820
	Private Sub animationEvent_ShiftLayers()
		For Each spriteRenderer As SpriteRenderer In MyBase.GetComponentsInChildren(Of SpriteRenderer)()
			spriteRenderer.sortingLayerName = "Background"
		Next
	End Sub

	' Token: 0x06002127 RID: 8487 RVA: 0x00132457 File Offset: 0x00130857
	Private Sub SFX_COWGIRL_COWGIRL_P1_BirdCall()
		AudioManager.Play("sfx_dlc_cowgirl_p1_birdcall")
		Me.emitAudioFromObject.Add("sfx_dlc_cowgirl_p1_birdcall")
	End Sub

	' Token: 0x040029B5 RID: 10677
	<SerializeField()>
	Private projectilePrefab As FlyingCowboyLevelBirdProjectile

	' Token: 0x040029B6 RID: 10678
	<SerializeField()>
	Private holdingFeetRenderer As SpriteRenderer

	' Token: 0x040029B7 RID: 10679
	<SerializeField()>
	Private emptyFeetRenderer As SpriteRenderer

	' Token: 0x040029B8 RID: 10680
	<SerializeField()>
	Private projectileSpawnPoint As Transform

	' Token: 0x040029B9 RID: 10681
	Private properties As LevelProperties.FlyingCowboy.Bird

	' Token: 0x040029BA RID: 10682
	Private cowgirl As FlyingCowboyLevelCowboy

	' Token: 0x040029BB RID: 10683
	Private bulletLandingPosition As Single

	' Token: 0x040029BC RID: 10684
	Private projectileSpawned As Boolean
End Class
