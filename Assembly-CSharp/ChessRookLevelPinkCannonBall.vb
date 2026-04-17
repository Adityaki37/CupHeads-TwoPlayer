Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000551 RID: 1361
Public Class ChessRookLevelPinkCannonBall
	Inherits AbstractProjectile

	' Token: 0x17000342 RID: 834
	' (get) Token: 0x06001946 RID: 6470 RVA: 0x000E53E5 File Offset: 0x000E37E5
	Public Overrides ReadOnly Property ParryMeterMultiplier As Single
		Get
			Return 0F
		End Get
	End Property

	' Token: 0x17000343 RID: 835
	' (get) Token: 0x06001947 RID: 6471 RVA: 0x000E53EC File Offset: 0x000E37EC
	' (set) Token: 0x06001948 RID: 6472 RVA: 0x000E53F4 File Offset: 0x000E37F4
	Public Property finishedOriginalArc As Boolean

	' Token: 0x06001949 RID: 6473 RVA: 0x000E53FD File Offset: 0x000E37FD
	Public Overrides Sub OnLevelEnd()
	End Sub

	' Token: 0x0600194A RID: 6474 RVA: 0x000E53FF File Offset: 0x000E37FF
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.coll = MyBase.GetComponent(Of Collider2D)()
	End Sub

	' Token: 0x0600194B RID: 6475 RVA: 0x000E5414 File Offset: 0x000E3814
	Public Function Create(position As Vector3, apexHeight As Single, targetDistance As Single, properties As LevelProperties.ChessRook.PinkCannonBall) As ChessRookLevelPinkCannonBall
		MyBase.ResetLifetime()
		MyBase.ResetDistance()
		MyBase.transform.position = position
		Me.properties = properties
		Me.apexHeight = apexHeight
		Me.targetDistance = targetDistance
		Me.gravity = properties.pinkGravity
		Me.Move()
		Return Me
	End Function

	' Token: 0x0600194C RID: 6476 RVA: 0x000E5463 File Offset: 0x000E3863
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x0600194D RID: 6477 RVA: 0x000E5481 File Offset: 0x000E3881
	Private Sub Move()
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x0600194E RID: 6478 RVA: 0x000E5490 File Offset: 0x000E3890
	Private Iterator Function move_cr() As IEnumerator
		AudioManager.Play("sfx_dlc_kog_rook_ghosthead_launch")
		Me.emitAudioFromObject.Add("sfx_dlc_kog_rook_ghosthead_launch")
		Dim endPosY As Single = CSng(Level.Current.Ground)
		Me.newRoot = New Vector3(MyBase.transform.position.x - Me.targetDistance, endPosY)
		Dim x As Single = Me.newRoot.x - MyBase.transform.position.x
		Dim apexDist As Single = Me.apexHeight
		Dim toSqrRootForviY As Single = Mathf.Abs(2F * Me.gravity * apexDist)
		Dim viY As Single = Mathf.Sqrt(toSqrRootForviY)
		Dim timeToApex As Single = Mathf.Abs(viY / Me.gravity)
		Dim toSqrtForTimeToG As Single = Mathf.Abs(2F * (MyBase.transform.position.y + apexDist) / Me.gravity)
		Dim timeToGround As Single = Mathf.Sqrt(toSqrtForTimeToG)
		Dim viX As Single = x / (timeToApex + timeToGround)
		Dim stillMoving As Boolean = True
		If Me.finishedOriginalArc AndAlso Not Me.playerOnBottom Then
			viY = Me.properties.velocityAfterSlam
			Me.gravity = Me.properties.gravityAfterSlam
			Yield Nothing
		End If
		Dim speed As Vector3 = New Vector3(viX, viY)
		While stillMoving
			speed += New Vector3(0F, Me.gravity * CupheadTime.FixedDelta)
			MyBase.transform.Translate(speed * CupheadTime.FixedDelta)
			Yield New WaitForFixedUpdate()
			If Me.gotParried Then
				stillMoving = False
				Exit While
			End If
			If MyBase.transform.position.y < CSng((Level.Current.Ground + 120)) Then
				Me.Sink(speed.x)
			End If
			If MyBase.transform.position.y < CSng((Level.Current.Ground + 40)) Then
				Me.coll.enabled = False
			End If
			If MyBase.transform.position.y < CSng((Level.Current.Ground - 40)) Then
				Me.Die()
			End If
		End While
		If Me.gotParried Then
			Me.Bounce()
		End If
		Yield Nothing
		Return
	End Function

	' Token: 0x0600194F RID: 6479 RVA: 0x000E54AC File Offset: 0x000E38AC
	Private Sub Sink(speedX As Single)
		If Me.sinking Then
			Return
		End If
		Me.sinking = True
		Me.parryColl.enabled = False
		Me.sinkFX.Create(New Vector3(MyBase.transform.position.x + speedX / 9F, CSng((Level.Current.Ground - 40))))
		AudioManager.Play("sfx_dlc_kog_rook_ghosthead_hitground_explode")
		Me.emitAudioFromObject.Add("sfx_dlc_kog_rook_ghosthead_hitground_explode")
	End Sub

	' Token: 0x06001950 RID: 6480 RVA: 0x000E552C File Offset: 0x000E392C
	Public Sub Explosion()
		Me.StopAllCoroutines()
		Me.parryColl.enabled = False
		Me.coll.enabled = False
		Me.rotatingExplosion.transform.eulerAngles = New Vector3(0F, 0F, CSng(Global.UnityEngine.Random.Range(0, 360)))
		Me.topExplosion.flipX = False
		MyBase.animator.Play("Explode")
		AudioManager.Play("sfx_dlc_kog_rook_ghosthead_hitsrook")
		Me.emitAudioFromObject.Add("sfx_dlc_kog_rook_ghosthead_hitsrook")
	End Sub

	' Token: 0x06001951 RID: 6481 RVA: 0x000E55B8 File Offset: 0x000E39B8
	Protected Overrides Sub Die()
		Me.Recycle()
	End Sub

	' Token: 0x06001952 RID: 6482 RVA: 0x000E55C0 File Offset: 0x000E39C0
	Private Sub Bounce()
		Me.gravity = Me.properties.pinkReactionGravity
		Me.apexHeight = Me.properties.bounceUpApexHeight + Me.heightAddition
		Me.targetDistance = If((Not Me.playerOnLeft), (Me.properties.bounceUpTargetDist + Me.distAddition), (-Me.properties.bounceUpTargetDist - Me.distAddition))
		If Not Me.finishedOriginalArc Then
			Me.finishedOriginalArc = True
		End If
		Me.gotParried = False
		Me.Move()
	End Sub

	' Token: 0x06001953 RID: 6483 RVA: 0x000E5650 File Offset: 0x000E3A50
	Public Sub GotParried(player As AbstractPlayerController)
		Me.playerOnLeft = player.transform.position.x < MyBase.transform.position.x
		Me.playerOnBottom = True
		Dim vector As Vector3 = player.center - MyBase.transform.position
		Dim num As Single = MathUtils.DirectionToAngle(vector)
		If num < 0F Then
			num = 360F + num
		End If
		If num >= 180F - Me.properties.goodQuadrantClemencyLeft AndAlso num <= 270F + Me.properties.goodQuadrantClemencyBottom Then
			MyBase.animator.SetTrigger("Parried")
			MyBase.GetComponent(Of SpriteRenderer)().sortingOrder = 1
			Dim num2 As Single = Mathf.InverseLerp(270F + Me.properties.goodQuadrantClemencyBottom, 180F, num)
			Me.distAddition = Me.properties.distanceAddition.GetFloatAt(num2)
			Me.heightAddition = Me.properties.heightAddition.GetFloatAt(1F - num2)
		ElseIf num > 270F + Me.properties.goodQuadrantClemencyBottom Then
			Dim num3 As Single = Mathf.InverseLerp(270F + Me.properties.goodQuadrantClemencyBottom, 360F, num)
			Me.distAddition = Me.properties.distanceAdditionBack.GetFloatAt(num3)
			Me.heightAddition = Me.properties.heightAdditionBack.GetFloatAt(1F - num3)
		Else
			If Me.playerOnLeft Then
				MyBase.animator.SetTrigger("Parried")
			End If
			Dim num4 As Single = Mathf.InverseLerp(180F, 0F, num)
			Me.distAddition = If((Not Me.playerOnLeft), Me.properties.distanceAdditionBack.GetFloatAt(num4), Me.properties.distanceAddition.GetFloatAt(num4))
			Me.heightAddition = 0F
			MyBase.GetComponent(Of SpriteRenderer)().sortingOrder = 1
			Me.playerOnBottom = False
		End If
		Me.gotParried = True
		MyBase.StartCoroutine(Me.collider_off_cr())
	End Sub

	' Token: 0x06001954 RID: 6484 RVA: 0x000E5874 File Offset: 0x000E3C74
	Private Iterator Function collider_off_cr() As IEnumerator
		Me.parryColl.enabled = False
		Me.coll.enabled = False
		Yield CupheadTime.WaitForSeconds(Me, Me.properties.bounceCollisionOffTimer)
		Me.parryColl.enabled = True
		Me.coll.enabled = True
		Return
	End Function

	' Token: 0x06001955 RID: 6485 RVA: 0x000E5890 File Offset: 0x000E3C90
	Private Sub LateUpdate()
		Me.shadow.transform.position = New Vector3(MyBase.transform.position.x, CSng(Level.Current.Ground))
		Dim num As Integer = CInt((Mathf.Abs(MyBase.transform.position.y - CSng(Level.Current.Ground)) / Me.maxShadowDistance * CSng(Me.shadowSprites.Length)))
		Me.shadow.enabled = Me.coll.enabled AndAlso num >= 0 AndAlso num < Me.shadowSprites.Length
		If Me.shadow.enabled Then
			Me.shadow.sprite = Me.shadowSprites(num)
		End If
		If Level.Current.Ending Then
			Me.coll.enabled = False
			Me.parryColl.enabled = False
		End If
	End Sub

	' Token: 0x04002261 RID: 8801
	Private properties As LevelProperties.ChessRook.PinkCannonBall

	' Token: 0x04002262 RID: 8802
	Private coll As Collider2D

	' Token: 0x04002263 RID: 8803
	Private newRoot As Vector3

	' Token: 0x04002264 RID: 8804
	Private apexHeight As Single

	' Token: 0x04002265 RID: 8805
	Private targetDistance As Single

	' Token: 0x04002266 RID: 8806
	Private gravity As Single

	' Token: 0x04002267 RID: 8807
	Private distAddition As Single

	' Token: 0x04002268 RID: 8808
	Private heightAddition As Single

	' Token: 0x04002269 RID: 8809
	Private gotParried As Boolean

	' Token: 0x0400226A RID: 8810
	Private playerOnLeft As Boolean

	' Token: 0x0400226B RID: 8811
	Private playerOnBottom As Boolean

	' Token: 0x0400226C RID: 8812
	<SerializeField()>
	Private parryColl As Collider2D

	' Token: 0x0400226D RID: 8813
	<SerializeField()>
	Private shadow As SpriteRenderer

	' Token: 0x0400226E RID: 8814
	<SerializeField()>
	Private shadowSprites As Sprite()

	' Token: 0x0400226F RID: 8815
	<SerializeField()>
	Private topExplosion As SpriteRenderer

	' Token: 0x04002270 RID: 8816
	<SerializeField()>
	Private rotatingExplosion As SpriteRenderer

	' Token: 0x04002271 RID: 8817
	<SerializeField()>
	Private bigExplosion As SpriteRenderer

	' Token: 0x04002272 RID: 8818
	<SerializeField()>
	Private sinkFX As Effect

	' Token: 0x04002273 RID: 8819
	Private sinking As Boolean

	' Token: 0x04002274 RID: 8820
	<SerializeField()>
	Private maxShadowDistance As Single = 750F
End Class
