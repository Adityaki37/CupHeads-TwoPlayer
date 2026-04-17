Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020007C3 RID: 1987
Public Class SaltbakerLevelBouncer
	Inherits LevelProperties.Saltbaker.Entity

	' Token: 0x06002CEA RID: 11498 RVA: 0x001A71CC File Offset: 0x001A55CC
	Private Sub Start()
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		Me.idleHash = Animator.StringToHash(MyBase.animator.GetLayerName(0) + ".Idle")
		For Each collisionChild As CollisionChild In Me.collisionKids
			AddHandler collisionChild.OnPlayerCollision, AddressOf Me.OnCollisionPlayer
			AddHandler collisionChild.OnPlayerProjectileCollision, AddressOf Me.OnCollisionPlayerProjectile
		Next
	End Sub

	' Token: 0x06002CEB RID: 11499 RVA: 0x001A726D File Offset: 0x001A566D
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06002CEC RID: 11500 RVA: 0x001A7285 File Offset: 0x001A5685
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		MyBase.properties.DealDamage(info.damage)
	End Sub

	' Token: 0x06002CED RID: 11501 RVA: 0x001A7298 File Offset: 0x001A5698
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06002CEE RID: 11502 RVA: 0x001A72B6 File Offset: 0x001A56B6
	Public Sub StartBouncer(startPos As Vector3)
		Me.bouncerStartPos = startPos
		MyBase.transform.position = startPos
		Me.saltHands.gameObject.SetActive(True)
		MyBase.StartCoroutine(Me.jump_cr())
	End Sub

	' Token: 0x06002CEF RID: 11503 RVA: 0x001A72EC File Offset: 0x001A56EC
	Private Function TimeToGround(curYVel As Single, groundY As Single, gravity As Single) As Single
		Dim num As Single = MyBase.transform.position.y - groundY
		Return(curYVel + Mathf.Sqrt(curYVel * curYVel + 2F * gravity * num)) / gravity
	End Function

	' Token: 0x06002CF0 RID: 11504 RVA: 0x001A7328 File Offset: 0x001A5728
	Private Iterator Function jump_cr() As IEnumerator
		Dim p As LevelProperties.Saltbaker.Bouncer = MyBase.properties.CurrentState.bouncer
		Dim animHelper As AnimationHelper = MyBase.GetComponent(Of AnimationHelper)()
		While Not Me.isDead
			Yield MyBase.animator.WaitForAnimationToEnd(Me, "Explode", False, False)
			MyBase.transform.position = Me.bouncerStartPos
			MyBase.animator.Play("Idle")
			Me.saltHands.Play()
			For Each collider2D As Collider2D In Me.colliders
				collider2D.enabled = False
			Next
			Yield CupheadTime.WaitForSeconds(Me, 3.5F)
			Me.SFX_SALTB_Bouncer_Twirl()
			Dim wait As YieldInstruction = New WaitForFixedUpdate()
			For Each collider2D2 As Collider2D In Me.colliders
				collider2D2.enabled = True
			Next
			Dim goingRight As Boolean = Rand.Bool()
			Dim velocityY As Single = 0F
			Dim velocityX As Single = 0F
			Dim gravity As Single = p.initDropYGravity
			Me.onGroundY = CSng(Level.Current.Ground) + MyBase.GetComponent(Of Collider2D)().bounds.size.y / 2F + 13F
			Dim maxX As Single = CSng(Level.Current.Right) - MyBase.GetComponent(Of Collider2D)().bounds.size.x / 2F
			Me.minShadowHeight = Me.onGroundY + 75F - p.jumpGravity * 0.027777778F + p.jumpYSpeed * 0.16666667F
			Me.maxShadowHeight = Me.onGroundY + p.jumpYSpeed * p.jumpYSpeed / (p.jumpGravity * 2F)
			Dim player As AbstractPlayerController = PlayerManager.GetNext()
			MyBase.transform.SetPosition(New Single?(player.transform.position.x), Nothing, Nothing)
			Dim timeToGround As Single = Me.TimeToGround(velocityY, Me.onGroundY + 75F, gravity)
			Dim animTimeOnLand As Single = -1F
			Dim useLandB As Boolean = False
			For i As Integer = 0 To p.numBounces + 1 - 1
				While MyBase.transform.position.y > Me.onGroundY + 75F
					timeToGround -= CupheadTime.FixedDelta
					If animTimeOnLand < 0F AndAlso MyBase.animator.GetCurrentAnimatorStateInfo(0).fullPathHash = Me.idleHash Then
						Dim num As Single = (timeToGround - 0.1F) / 0.6666667F
						animTimeOnLand = (num + MyBase.animator.GetCurrentAnimatorStateInfo(0).normalizedTime) Mod 1F
						Dim num2 As Single = Mathf.Min(Mathf.Abs(animTimeOnLand - 0.84375F), Mathf.Abs(0.84375F - animTimeOnLand))
						Dim num3 As Single = Mathf.Min(Mathf.Abs(animTimeOnLand - 0.40625F), Mathf.Abs(0.40625F - animTimeOnLand))
						useLandB = num2 > num3 AndAlso i < p.numBounces
						Dim num4 As Single = animTimeOnLand - If((Not useLandB), 0.84375F, 0.40625F)
						Dim num5 As Single = num - num4
						animHelper.Speed = num5 / num
					End If
					If timeToGround < 0.1F Then
						animHelper.Speed = 1F
						If i = p.numBounces OrElse Me.isDead Then
							MyBase.animator.Play("Explode")
						Else
							MyBase.animator.Play(If((Not useLandB), "Land_A", "Land_B"))
						End If
					End If
					velocityY -= gravity * CupheadTime.FixedDelta
					If i > 0 Then
						velocityX = If((Not goingRight), (-p.jumpXSpeed), p.jumpXSpeed)
					End If
					MyBase.transform.AddPosition(velocityX * CupheadTime.FixedDelta, velocityY * CupheadTime.FixedDelta, 0F)
					If(Not goingRight AndAlso MyBase.transform.position.x < -maxX) OrElse (goingRight AndAlso MyBase.transform.position.x > maxX) Then
						MyBase.transform.SetPosition(New Single?(If((Not goingRight), (-maxX), maxX)), Nothing, Nothing)
						goingRight = Not goingRight
						If velocityY < 0F Then
							velocityX = 0F
						End If
					End If
					Yield wait
				End While
				CupheadLevelCamera.Current.Shake(30F, 0.7F, False)
				MyBase.transform.SetPosition(New Single?(MyBase.transform.position.x + velocityX / Mathf.Abs(velocityY) * 75F), New Single?(Me.onGroundY), Nothing)
				Me.landFXAnimator.transform.position = MyBase.transform.position
				Me.landFXAnimator.Play("LandFX")
				While MyBase.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.55F AndAlso Not Me.isDead
					Yield Nothing
				End While
				If i < p.numBounces AndAlso Not Me.isDead Then
					velocityY = p.jumpYSpeed
					velocityX = If((Not goingRight), (-p.jumpXSpeed), p.jumpXSpeed)
					gravity = p.jumpGravity
					MyBase.transform.position += Vector3.up * 76F
					MyBase.transform.position += Vector3.right * (velocityX / Mathf.Abs(velocityY) * 75F)
					timeToGround = Me.TimeToGround(velocityY, Me.onGroundY + 75F, gravity)
					animTimeOnLand = -1F
				End If
				If Me.isDead Then
					Exit For
				End If
				Yield wait
			Next
			For Each collider2D3 As Collider2D In Me.colliders
				collider2D3.enabled = False
			Next
			If Me.isDead AndAlso Not MyBase.animator.GetCurrentAnimatorStateInfo(0).IsName("Explode") Then
				MyBase.animator.Play("Explode", 0, 0F)
				MyBase.animator.Update(0F)
			End If
		End While
		Return
	End Function

	' Token: 0x06002CF1 RID: 11505 RVA: 0x001A7343 File Offset: 0x001A5743
	Public Sub EndBouncer()
		MyBase.StartCoroutine(Me.end_bouncer_cr())
	End Sub

	' Token: 0x06002CF2 RID: 11506 RVA: 0x001A7354 File Offset: 0x001A5754
	Private Iterator Function end_bouncer_cr() As IEnumerator
		Me.isDead = True
		Yield MyBase.animator.WaitForAnimationToStart(Me, "Off", False)
		For Each collider2D As Collider2D In Me.colliders
			collider2D.enabled = False
		Next
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		For Each collisionChild As CollisionChild In Me.collisionKids
			RemoveHandler collisionChild.OnPlayerCollision, AddressOf Me.OnCollisionPlayer
			RemoveHandler collisionChild.OnPlayerProjectileCollision, AddressOf Me.OnCollisionPlayerProjectile
		Next
		RemoveHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Return
	End Function

	' Token: 0x06002CF3 RID: 11507 RVA: 0x001A7370 File Offset: 0x001A5770
	Private Sub LateUpdate()
		If MyBase.animator.GetCurrentAnimatorStateInfo(0).fullPathHash = Me.idleHash Then
			Me.shadow.sprite = Me.shadowSprites(CInt((Mathf.InverseLerp(Me.maxShadowHeight, Me.minShadowHeight, MyBase.transform.position.y) * CSng((Me.shadowSprites.Length - 1)))))
		End If
		Me.shadow.transform.position = New Vector3(MyBase.transform.position.x, Me.onGroundY)
	End Sub

	' Token: 0x06002CF4 RID: 11508 RVA: 0x001A7410 File Offset: 0x001A5810
	Public Overrides Sub OnPause()
		MyBase.OnPause()
		Me.pauseShadow.sprite = Me.shadow.sprite
		Me.pauseShadow.transform.position = New Vector3(Me.shadow.transform.position.x, Me.onGroundY)
		Me.shadow.enabled = False
	End Sub

	' Token: 0x06002CF5 RID: 11509 RVA: 0x001A7478 File Offset: 0x001A5878
	Public Overrides Sub OnUnpause()
		MyBase.OnUnpause()
		Me.pauseShadow.sprite = Nothing
		Me.shadow.enabled = True
	End Sub

	' Token: 0x06002CF6 RID: 11510 RVA: 0x001A7498 File Offset: 0x001A5898
	Private Sub AnimationEvent_SFX_SALTB_Bouncer_Bounce()
		AudioManager.Play("sfx_dlc_saltbaker_p3_bouncer_bounce")
		Me.emitAudioFromObject.Add("sfx_dlc_saltbaker_p3_bouncer_bounce")
	End Sub

	' Token: 0x06002CF7 RID: 11511 RVA: 0x001A74B4 File Offset: 0x001A58B4
	Private Sub AnimationEvent_SFX_SALTB_Bouncer_Death()
		AudioManager.[Stop]("sfx_dlc_saltbaker_p3_bouncer_twirl")
		AudioManager.Play("sfx_dlc_saltbaker_p3_bouncer_death")
		Me.emitAudioFromObject.Add("sfx_dlc_saltbaker_p3_bouncer_death")
	End Sub

	' Token: 0x06002CF8 RID: 11512 RVA: 0x001A74DA File Offset: 0x001A58DA
	Private Sub SFX_SALTB_Bouncer_Twirl()
		AudioManager.PlayLoop("sfx_dlc_saltbaker_p3_bouncer_twirl")
		Me.emitAudioFromObject.Add("sfx_dlc_saltbaker_p3_bouncer_twirl")
	End Sub

	' Token: 0x04003568 RID: 13672
	Private Const IDLE_ANIM_LENGTH As Single = 0.6666667F

	' Token: 0x04003569 RID: 13673
	Private Const ANIM_TIME_PRE_LAND As Single = 0.1F

	' Token: 0x0400356A RID: 13674
	Private Const NORMALIZED_ANIM_TIME_TO_RELAUNCH As Single = 0.55F

	' Token: 0x0400356B RID: 13675
	Private Const TARGET_TIME_LAND_A As Single = 0.84375F

	' Token: 0x0400356C RID: 13676
	Private Const TARGET_TIME_LAND_B As Single = 0.40625F

	' Token: 0x0400356D RID: 13677
	Private Const GROUND_TRIGGER_OFFSET As Single = 75F

	' Token: 0x0400356E RID: 13678
	Private Const GROUND_POS_OFFSET As Single = 13F

	' Token: 0x0400356F RID: 13679
	<SerializeField()>
	Private saltHands As SaltbakerLevelBGSaltHands

	' Token: 0x04003570 RID: 13680
	<SerializeField()>
	Private shadow As SpriteRenderer

	' Token: 0x04003571 RID: 13681
	<SerializeField()>
	Private pauseShadow As SpriteRenderer

	' Token: 0x04003572 RID: 13682
	<SerializeField()>
	Private shadowSprites As Sprite()

	' Token: 0x04003573 RID: 13683
	<SerializeField()>
	Private collisionKids As CollisionChild()

	' Token: 0x04003574 RID: 13684
	<SerializeField()>
	Private landFXAnimator As Animator

	' Token: 0x04003575 RID: 13685
	Private damageDealer As DamageDealer

	' Token: 0x04003576 RID: 13686
	Private damageReceiver As DamageReceiver

	' Token: 0x04003577 RID: 13687
	Private bouncerStartPos As Vector3

	' Token: 0x04003578 RID: 13688
	Private onGroundY As Single

	' Token: 0x04003579 RID: 13689
	<SerializeField()>
	Private colliders As Collider2D()

	' Token: 0x0400357A RID: 13690
	Private isDead As Boolean

	' Token: 0x0400357B RID: 13691
	Private shadowSprite As Integer

	' Token: 0x0400357C RID: 13692
	Private idleHash As Integer

	' Token: 0x0400357D RID: 13693
	Private minShadowHeight As Single

	' Token: 0x0400357E RID: 13694
	Private maxShadowHeight As Single
End Class
