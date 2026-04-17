Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200058C RID: 1420
Public Class DevilLevelPitchforkBouncingProjectile
	Inherits AbstractProjectile

	' Token: 0x17000350 RID: 848
	' (get) Token: 0x06001B22 RID: 6946 RVA: 0x000F9334 File Offset: 0x000F7734
	Protected Overrides ReadOnly Property DestroyLifetime As Single
		Get
			Return -1F
		End Get
	End Property

	' Token: 0x17000351 RID: 849
	' (get) Token: 0x06001B23 RID: 6947 RVA: 0x000F933B File Offset: 0x000F773B
	' (set) Token: 0x06001B24 RID: 6948 RVA: 0x000F9343 File Offset: 0x000F7743
	Public Property BouncesRemaining As Integer

	' Token: 0x17000352 RID: 850
	' (get) Token: 0x06001B25 RID: 6949 RVA: 0x000F934C File Offset: 0x000F774C
	Protected Overrides ReadOnly Property DestroyedAfterLeavingScreen As Boolean
		Get
			Return True
		End Get
	End Property

	' Token: 0x06001B26 RID: 6950 RVA: 0x000F9350 File Offset: 0x000F7750
	Public Function Create(pos As Vector2, attackDelay As Single, speed As Single, angle As Single, numBounces As Integer, parent As DevilLevelSittingDevil, waitTime As Single) As DevilLevelPitchforkBouncingProjectile
		Dim devilLevelPitchforkBouncingProjectile As DevilLevelPitchforkBouncingProjectile = Me.InstantiatePrefab(Of DevilLevelPitchforkBouncingProjectile)()
		devilLevelPitchforkBouncingProjectile.transform.position = pos
		devilLevelPitchforkBouncingProjectile.attackDelay = attackDelay
		devilLevelPitchforkBouncingProjectile.velocity = speed * MathUtils.AngleToDirection(angle)
		devilLevelPitchforkBouncingProjectile.BouncesRemaining = numBounces
		devilLevelPitchforkBouncingProjectile.parent = parent
		devilLevelPitchforkBouncingProjectile.state = DevilLevelPitchforkBouncingProjectile.State.Idle
		devilLevelPitchforkBouncingProjectile.waitTime = waitTime
		devilLevelPitchforkBouncingProjectile.StartCoroutine(devilLevelPitchforkBouncingProjectile.main_cr())
		devilLevelPitchforkBouncingProjectile.animator.SetFloat("Variation", CSng(Global.UnityEngine.Random.Range(0, 3)) / 2F)
		Return devilLevelPitchforkBouncingProjectile
	End Function

	' Token: 0x06001B27 RID: 6951 RVA: 0x000F93DC File Offset: 0x000F77DC
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If MyBase.CanParry Then
			Dim component As LevelPlayerParryController = hit.GetComponent(Of LevelPlayerParryController)()
			If component IsNot Nothing AndAlso component.State = LevelPlayerParryController.ParryState.Parrying Then
				Return
			End If
		End If
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x06001B28 RID: 6952 RVA: 0x000F9430 File Offset: 0x000F7830
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.parent Is Nothing Then
			Me.Die()
		End If
	End Sub

	' Token: 0x06001B29 RID: 6953 RVA: 0x000F9450 File Offset: 0x000F7850
	Protected Overrides Sub FixedUpdate()
		MyBase.FixedUpdate()
		If Not Me.waitTimeUp Then
			Return
		End If
		If Not MyBase.dead AndAlso Me.state <> DevilLevelPitchforkBouncingProjectile.State.Idle Then
			MyBase.transform.AddPosition(Me.velocity.x * CupheadTime.FixedDelta, Me.velocity.y * CupheadTime.FixedDelta, 0F)
			If Me.velocity = Vector2.zero Then
				Me.bounceTime += CupheadTime.FixedDelta
				If Me.bounceTime > 0.083333336F Then
					Me.velocity = Me.velocityOld
				End If
			End If
			Dim radius As Single = MyBase.GetComponent(Of CircleCollider2D)().radius
			If Me.BouncesRemaining > 0 Then
				If(Me.velocity.x < 0F AndAlso MyBase.transform.position.x < CSng(Level.Current.Left) + radius) OrElse (Me.velocity.x > 0F AndAlso MyBase.transform.position.x > CSng(Level.Current.Right) - radius) Then
					If Me.bounceTime = 0F Then
						MyBase.animator.Play("BounceWall")
						Me.BounceSFX()
						Me.velocityOld = Me.velocity
						Me.velocity = Vector2.zero
					ElseIf Me.bounceTime > 0.083333336F Then
						Me.BouncesRemaining -= 1
						Me.velocity.x = Me.velocity.x * -1F
						Me.bounceTime = 0F
					End If
				End If
				If Me.velocity.y > 0F AndAlso MyBase.transform.position.y > CSng(Level.Current.Ceiling) + radius Then
					If Me.bounceTime = 0F Then
						MyBase.animator.Play("BounceGround")
						Me.BounceSFX()
						Me.velocityOld = Me.velocity
						Me.velocity = Vector2.zero
					ElseIf Me.bounceTime > 0.083333336F Then
						Me.BouncesRemaining -= 1
						Me.velocity.y = Me.velocity.y * -1F
						Me.bounceTime = 0F
					End If
				End If
			End If
			If Me.velocity.y < 0F AndAlso MyBase.transform.position.y < CSng(Level.Current.Ground) + radius Then
				If Me.bounceTime = 0F Then
					MyBase.animator.Play("BounceGround")
					Me.BounceSFX()
					Me.velocityOld = Me.velocity
					Me.velocity = Vector2.zero
					If MyBase.CanParry Then
						Me.bounceEffectPink.Create(MyBase.transform.position)
					Else
						Me.bounceEffect.Create(MyBase.transform.position)
					End If
				ElseIf Me.bounceTime > 0.083333336F Then
					Me.BouncesRemaining -= 1
					Me.velocity.y = Me.velocity.y * -1F
					Me.bounceTime = 0F
				End If
			End If
		End If
	End Sub

	' Token: 0x06001B2A RID: 6954 RVA: 0x000F97C0 File Offset: 0x000F7BC0
	Private Iterator Function main_cr() As IEnumerator
		MyBase.GetComponent(Of Collider2D)().enabled = False
		Yield CupheadTime.WaitForSeconds(Me, Me.waitTime)
		MyBase.animator.SetTrigger("Continue")
		Me.waitTimeUp = True
		MyBase.GetComponent(Of Collider2D)().enabled = True
		Yield CupheadTime.WaitForSeconds(Me, Me.attackDelay)
		Me.state = DevilLevelPitchforkBouncingProjectile.State.Attacking
		While True
			Yield CupheadTime.WaitForSeconds(Me, 0.1F)
			Dim selectedSparkle As Effect = If((Not MyBase.CanParry), Me.blueSparkle, Me.pinkSparkle)
			Dim inst As Effect = selectedSparkle.Create(MyBase.transform.position)
			Dim r As SpriteRenderer = inst.GetComponent(Of SpriteRenderer)()
			r.sortingLayerName = "Projectiles"
			r.sortingOrder = -1
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06001B2B RID: 6955 RVA: 0x000F97DB File Offset: 0x000F7BDB
	Protected Overrides Sub Die()
		MyBase.Die()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06001B2C RID: 6956 RVA: 0x000F97EE File Offset: 0x000F7BEE
	Public Overrides Sub SetParryable(parryable As Boolean)
		MyBase.SetParryable(parryable)
		MyBase.animator.SetBool("IsPink", parryable)
	End Sub

	' Token: 0x06001B2D RID: 6957 RVA: 0x000F9808 File Offset: 0x000F7C08
	Public Overrides Sub OnParry(player As AbstractPlayerController)
		MyBase.OnParry(player)
		Me.BouncesRemaining = 0
	End Sub

	' Token: 0x06001B2E RID: 6958 RVA: 0x000F9818 File Offset: 0x000F7C18
	Private Sub BounceSFX()
		AudioManager.Play("devil_projectile_bounce")
		Me.emitAudioFromObject.Add("devil_projectile_bounce")
	End Sub

	' Token: 0x06001B2F RID: 6959 RVA: 0x000F9834 File Offset: 0x000F7C34
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.blueSparkle = Nothing
		Me.pinkSparkle = Nothing
		Me.bounceEffect = Nothing
		Me.bounceEffectPink = Nothing
	End Sub

	' Token: 0x0400245C RID: 9308
	Private Const ProjectilesLayerName As String = "Projectiles"

	' Token: 0x0400245D RID: 9309
	Private Const VariationMax As Integer = 3

	' Token: 0x0400245E RID: 9310
	Private Const BounceTimeThreshold As Single = 0.083333336F

	' Token: 0x0400245F RID: 9311
	Public state As DevilLevelPitchforkBouncingProjectile.State

	' Token: 0x04002461 RID: 9313
	Private attackDelay As Single

	' Token: 0x04002462 RID: 9314
	Private velocity As Vector2

	' Token: 0x04002463 RID: 9315
	Private velocityOld As Vector2

	' Token: 0x04002464 RID: 9316
	Private parent As DevilLevelSittingDevil

	' Token: 0x04002465 RID: 9317
	Private waitTime As Single

	' Token: 0x04002466 RID: 9318
	Private waitTimeUp As Boolean

	' Token: 0x04002467 RID: 9319
	Private bounceTime As Single

	' Token: 0x04002468 RID: 9320
	<SerializeField()>
	Private blueSparkle As Effect

	' Token: 0x04002469 RID: 9321
	<SerializeField()>
	Private pinkSparkle As Effect

	' Token: 0x0400246A RID: 9322
	<SerializeField()>
	Private bounceEffect As Effect

	' Token: 0x0400246B RID: 9323
	<SerializeField()>
	Private bounceEffectPink As Effect

	' Token: 0x0200058D RID: 1421
	Public Enum State
		' Token: 0x0400246D RID: 9325
		Idle
		' Token: 0x0400246E RID: 9326
		Attacking
	End Enum
End Class
