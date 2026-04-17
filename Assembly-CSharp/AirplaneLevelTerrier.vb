Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020004C6 RID: 1222
Public Class AirplaneLevelTerrier
	Inherits AbstractCollidableObject

	' Token: 0x17000313 RID: 787
	' (get) Token: 0x0600148C RID: 5260 RVA: 0x000B8426 File Offset: 0x000B6826
	' (set) Token: 0x0600148D RID: 5261 RVA: 0x000B842E File Offset: 0x000B682E
	Public Property IsDead As Boolean

	' Token: 0x17000314 RID: 788
	' (get) Token: 0x0600148E RID: 5262 RVA: 0x000B8437 File Offset: 0x000B6837
	' (set) Token: 0x0600148F RID: 5263 RVA: 0x000B843F File Offset: 0x000B683F
	Public Property ReadyToMove As Boolean

	' Token: 0x06001490 RID: 5264 RVA: 0x000B8448 File Offset: 0x000B6848
	Private Sub Start()
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
	End Sub

	' Token: 0x06001491 RID: 5265 RVA: 0x000B8478 File Offset: 0x000B6878
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.WORKAROUND_NullifyFields()
	End Sub

	' Token: 0x06001492 RID: 5266 RVA: 0x000B8488 File Offset: 0x000B6888
	Public Sub Init(pivotPoint As Transform, angle As Single, properties As LevelProperties.Airplane.Terriers, hp As Single, pivotOffsetX As Single, pivotOffsetY As Single, isClockwise As Boolean, index As Integer)
		Me.angle = angle
		Me.pivotPoint = pivotPoint
		Me.pivotOffset = New Vector2(pivotOffsetX, pivotOffsetY)
		Me.properties = properties
		Me.hp = hp
		Me.smokingThreshold = hp * properties.secretHPPercentage
		Me.isClockwise = isClockwise
		Me.index = index
		Me.wobbleTimer = CSng(index)
		MyBase.StartCoroutine(Me.setup_dogs_cr())
	End Sub

	' Token: 0x06001493 RID: 5267 RVA: 0x000B84F6 File Offset: 0x000B68F6
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.hp -= info.damage
		If Me.hp <= 0F Then
			Level.Current.RegisterMinionKilled()
			Me.Die()
		End If
	End Sub

	' Token: 0x06001494 RID: 5268 RVA: 0x000B852C File Offset: 0x000B692C
	Public Function IsSmoking() As Boolean
		If Not Me.isSmoking AndAlso Me.hp < Me.smokingThreshold Then
			Me.isSmoking = True
			MyBase.animator.SetTrigger(AirplaneLevelTerrier.OnShockParameterID)
			AudioManager.Play("sfx_dlc_dogfight_p2_terrierjetpack_dmgsmoke")
			Me.emitAudioFromObject.Add("sfx_dlc_dogfight_p2_terrierjetpack_dmgsmoke")
		End If
		Return Me.isSmoking
	End Function

	' Token: 0x06001495 RID: 5269 RVA: 0x000B858C File Offset: 0x000B698C
	Public Function Health() As Single
		Return Me.hp
	End Function

	' Token: 0x06001496 RID: 5270 RVA: 0x000B8594 File Offset: 0x000B6994
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x06001497 RID: 5271 RVA: 0x000B85B2 File Offset: 0x000B69B2
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
		Me.wobbleTimer += Me.wobbleSpeed * CupheadTime.Delta
	End Sub

	' Token: 0x06001498 RID: 5272 RVA: 0x000B85E8 File Offset: 0x000B69E8
	Private Function WobblePos() As Vector3
		Return New Vector3(Mathf.Sin(Me.wobbleTimer * 3F) * Me.wobbleX, Mathf.Sin(Me.wobbleTimer * 2F) * Me.wobbleY, 0F) * Me.wobbleModifier
	End Function

	' Token: 0x06001499 RID: 5273 RVA: 0x000B863C File Offset: 0x000B6A3C
	Private Iterator Function setup_dogs_cr() As IEnumerator
		Me.rotationOffset = Vector3.zero
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		Dim indexToPlay As Integer = Me.index
		MyBase.transform.SetScale(New Single?(If((Not Me.isClockwise), Mathf.Abs(MyBase.transform.localScale.x), (-Mathf.Abs(MyBase.transform.localScale.x)))), Nothing, Nothing)
		Dim num As Integer = Me.index
		If num <> 1 Then
			If num = 3 Then
				Dim num4 As Integer
				If Me.isClockwise Then
					Dim num2 As Integer = 1
					Dim num3 As Integer = num2
					indexToPlay = num2
					num4 = num3
				Else
					num4 = Me.index
				End If
				indexToPlay = num4
			End If
		Else
			Dim num6 As Integer
			If Me.isClockwise Then
				Dim num5 As Integer = 3
				Dim num3 As Integer = num5
				indexToPlay = num5
				num6 = num3
			Else
				num6 = Me.index
			End If
			indexToPlay = num6
		End If
		MyBase.animator.Play("Intro_" + indexToPlay)
		Dim flamePos As Integer = indexToPlay * 4
		If indexToPlay = 3 Then
			flamePos = 4
		End If
		Me.flame.transform.localPosition = Me.flameOffset(flamePos)
		If indexToPlay = 1 Then
			Me.flame.transform.localPosition = New Vector3(-Me.flame.transform.localPosition.x, Me.flame.transform.localPosition.y)
		End If
		Me.angle *= 0.017453292F
		Me.loopSizeX = 675F
		Me.loopSizeY = 328.5F
		Me.rotationOffset.x = Mathf.Sin(Me.angle) * Me.loopSizeX
		Me.rotationOffset.y = Mathf.Cos(Me.angle) * Me.loopSizeY
		Dim startPos As Vector3 = Me.pivotPoint.position + Me.pivotOffset + Me.rotationOffset * 2F
		Dim endPos As Vector3 = Me.pivotPoint.position + Me.pivotOffset + Me.rotationOffset
		Dim t As Single = 0F
		Dim time As Single = 0.5F
		MyBase.transform.position = startPos
		While t < time
			t += CupheadTime.FixedDelta
			MyBase.transform.position = Vector3.Lerp(startPos, endPos, t / time) + Me.WobblePos()
			Yield wait
		End While
		MyBase.animator.SetTrigger("ContinueIntro")
		t = 0F
		While t < 0.9F
			t += CupheadTime.FixedDelta
			MyBase.transform.position = endPos + Me.WobblePos()
			Me.wobbleModifier = Mathf.Lerp(1F, 0F, t / 0.9F)
			Yield wait
		End While
		Me.wobbleModifier = 0F
		MyBase.animator.SetTrigger("EndIntro")
		Me.rotationSpeed = 0F
		Me.ReadyToMove = True
		Yield MyBase.animator.WaitForAnimationToStart(Me, "Idle", False)
		Me.introFinished = True
		CType(Level.Current, AirplaneLevel).terriersIntroFinished = True
		Return
	End Function

	' Token: 0x0600149A RID: 5274 RVA: 0x000B8658 File Offset: 0x000B6A58
	Private Iterator Function ease_to_full_speed_and_radius_cr() As IEnumerator
		Dim t As Single = 0F
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		While t < 1F
			Me.loopSizeX = Mathf.Lerp(675F, 750F, EaseUtils.EaseOutSine(0F, 1F, t / 1F))
			Me.loopSizeY = Mathf.Lerp(328.5F, 365F, EaseUtils.EaseOutSine(0F, 1F, t / 1F))
			Me.rotationSpeed = Mathf.Lerp(0F, Me.properties.rotationTime, EaseUtils.EaseInSine(0F, 1F, t / 1F))
			t += CupheadTime.FixedDelta
			Yield wait
		End While
		Me.loopSizeX = 750F
		Me.loopSizeY = 365F
		Me.rotationSpeed = Me.properties.rotationTime
		Return
	End Function

	' Token: 0x0600149B RID: 5275 RVA: 0x000B8673 File Offset: 0x000B6A73
	Public Sub StartMoving()
		MyBase.StartCoroutine(Me.move_in_circle_cr())
		MyBase.StartCoroutine(Me.ease_to_full_speed_and_radius_cr())
	End Sub

	' Token: 0x0600149C RID: 5276 RVA: 0x000B8690 File Offset: 0x000B6A90
	Private Iterator Function move_in_circle_cr() As IEnumerator
		Me.rotationOffset = Vector3.zero
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		While True
			Me.angle += Me.rotationSpeed * CupheadTime.FixedDelta * CSng(If((Not Me.isClockwise), (-1), 1))
			If Not Me.gettingEaten Then
				Me.rotationOffset.x = Mathf.Sin(Me.angle) * Me.loopSizeX
			Else
				Dim flag As Boolean = If((Not Me.isClockwise), (Me.angle < 3.1415927F), (Me.angle > 3.1415927F))
				Me.rotationOffset.x = Mathf.Sin(Me.angle) * If((Not flag), Me.loopSizeX, 1000F)
				If flag Then
					Me.loopSizeY -= CupheadTime.FixedDelta * 50F
				End If
			End If
			Me.rotationOffset.y = Mathf.Cos(Me.angle) * Me.loopSizeY
			Dim lastPos As Vector3 = MyBase.transform.position
			MyBase.transform.position = Me.pivotPoint.position + Me.pivotOffset
			MyBase.transform.position += Me.rotationOffset
			Me.flame.flipX = Mathf.Sign(lastPos.x - MyBase.transform.position.x) = -1F
			If Me.angle > 6.2831855F Then
				Me.angle -= 6.2831855F
			End If
			If Me.angle < 0F Then
				Me.angle += 6.2831855F
			End If
			Yield wait
		End While
		Return
	End Function

	' Token: 0x0600149D RID: 5277 RVA: 0x000B86AC File Offset: 0x000B6AAC
	Public Function GetPredictedAttackPos() As Vector3
		Dim num As Single = 0.125F
		Dim num2 As Single = Me.angle + Me.properties.rotationTime * num * CSng(If((Not Me.isClockwise), (-1), 1))
		Return Me.pivotPoint.position + Me.pivotOffset + New Vector2(Mathf.Sin(num2) * 750F, Mathf.Cos(num2) * 365F)
	End Function

	' Token: 0x0600149E RID: 5278 RVA: 0x000B872C File Offset: 0x000B6B2C
	Public Sub StartAttack(isPink As Boolean, isWow As Boolean)
		Me.isPink = isPink
		Me.isWow = isWow
		If Me.isClockwise Then
			MyBase.transform.SetScale(New Single?(Mathf.Abs(MyBase.transform.localScale.x)), Nothing, Nothing)
		End If
		MyBase.animator.Play("Attack")
		Me.SFX_DOGFIGHT_P2_TerrierJetpack_BarkShoot()
	End Sub

	' Token: 0x0600149F RID: 5279 RVA: 0x000B87A4 File Offset: 0x000B6BA4
	Private Sub AniEvent_BarkFX()
		Me.barkFXRenderer.sortingLayerID = Me.rends(Me.currentAngle).sortingLayerID
		Me.barkFXRenderer.sortingOrder = Me.rends(Me.currentAngle).sortingOrder + If((Me.currentAngle > 4), (-1), 1)
		Me.barkFXRenderer.flipX = Me.rends(Me.currentAngle).flipX
		Me.barkFXRenderer.transform.localPosition = -Me.flame.transform.localPosition * 0.5F
		If Me.currentAngle = 1 Then
			Me.barkFXRenderer.transform.localPosition += New Vector3(CSng(If((Not Me.barkFXRenderer.flipX), 10, (-10))), 12F)
		End If
		If Me.currentAngle = 2 Then
			Me.barkFXRenderer.transform.localPosition += New Vector3(CSng(If((Not Me.barkFXRenderer.flipX), (-10), 10)), 5F)
		End If
		Me.barkFXRenderer.transform.eulerAngles = New Vector3(0F, 0F, CSng(Global.UnityEngine.Random.Range(0, 360)))
		Me.barkFXAnimator.Play(If((Not Rand.Bool()), "B", "A"))
	End Sub

	' Token: 0x060014A0 RID: 5280 RVA: 0x000B8930 File Offset: 0x000B6D30
	Private Sub AniEvent_ShootProjectile()
		Dim [next] As AbstractPlayerController = PlayerManager.GetNext()
		Dim vector As Vector3 = [next].center - Me.barkFXRenderer.transform.position
		Dim num As Single = Vector3.Magnitude(Me.rotationOffset) / 750F
		Dim airplaneLevelTerrierBullet As AirplaneLevelTerrierBullet
		If Me.isPink Then
			airplaneLevelTerrierBullet = Me.pinkProjectile.Create(Me.barkFXRenderer.transform.position, MathUtils.DirectionToAngle(vector), Me.properties.shotSpeed, num)
		Else
			airplaneLevelTerrierBullet = Me.regularProjectile.Create(Me.barkFXRenderer.transform.position, MathUtils.DirectionToAngle(vector), Me.properties.shotSpeed, num)
		End If
		If Me.isWow Then
			airplaneLevelTerrierBullet.PlayWow()
		End If
	End Sub

	' Token: 0x060014A1 RID: 5281 RVA: 0x000B8A04 File Offset: 0x000B6E04
	Private Sub AniEvent_SetScale()
		If Me.isClockwise Then
			MyBase.transform.SetScale(New Single?(Mathf.Abs(MyBase.transform.localScale.x)), Nothing, Nothing)
		End If
	End Sub

	' Token: 0x060014A2 RID: 5282 RVA: 0x000B8A56 File Offset: 0x000B6E56
	Public Sub StartSecret()
		MyBase.GetComponent(Of Collider2D)().enabled = False
		MyBase.StartCoroutine(Me.move_into_mouth_cr())
	End Sub

	' Token: 0x060014A3 RID: 5283 RVA: 0x000B8A74 File Offset: 0x000B6E74
	Public Sub PrepareForChomp()
		Me.gettingEaten = True
		For Each spriteRenderer As SpriteRenderer In Me.rends
			spriteRenderer.sortingOrder = 2
			spriteRenderer.sortingLayerName = "Foreground"
		Next
	End Sub

	' Token: 0x060014A4 RID: 5284 RVA: 0x000B8ABC File Offset: 0x000B6EBC
	Private Iterator Function move_into_mouth_cr() As IEnumerator
		Dim t As Single = 0F
		Dim startSpeed As Single = Me.rotationSpeed
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		While t < 1F
			t += CupheadTime.FixedDelta
			Me.rotationSpeed = Mathf.Lerp(startSpeed, 2.5F, EaseUtils.EaseInSine(0F, 1F, t))
			Me.loopSizeX = Mathf.Lerp(750F, 600F, EaseUtils.EaseInSine(0F, 1F, t))
			Me.loopSizeY = Mathf.Lerp(365F, 292F, EaseUtils.EaseInSine(0F, 1F, t))
			Yield wait
		End While
		Return
	End Function

	' Token: 0x060014A5 RID: 5285 RVA: 0x000B8AD8 File Offset: 0x000B6ED8
	Private Sub Die()
		Me.IsDead = True
		Me.flame.enabled = False
		Me.coll.enabled = False
		MyBase.animator.Play(If((Not Me.lastOne), "Death", "DeathShort"))
		Me.SFX_DOGFIGHT_P2_TerrierJetpack_Explosion()
		MyBase.StartCoroutine(Me.SFX_DOGFIGHT_P2_TerrierJetpack_DeathBark_cr(Me.index))
	End Sub

	' Token: 0x060014A6 RID: 5286 RVA: 0x000B8B42 File Offset: 0x000B6F42
	Private Sub AniEvent_DeathLayering()
		Me.deathRenderer.sortingLayerName = "Background"
		Me.deathRenderer.sortingOrder = 0
	End Sub

	' Token: 0x060014A7 RID: 5287 RVA: 0x000B8B60 File Offset: 0x000B6F60
	Private Sub AniEvent_OnDeath()
		Me.StopAllCoroutines()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x060014A8 RID: 5288 RVA: 0x000B8B74 File Offset: 0x000B6F74
	Private Sub CheckAllAnimations()
		If Me.gettingEaten Then
			Return
		End If
		Dim num As Single = Me.angle * 57.29578F
		If(num > 344F AndAlso num < 360F) OrElse (num < 8.2F AndAlso num > 0F) Then
			Me.ChangeAngle(False, 0)
		ElseIf num > 8.2F AndAlso num < 31.8F Then
			Me.ChangeAngle(True, 1)
		ElseIf num > 31.8F AndAlso num < 55.4F Then
			Me.ChangeAngle(True, 2)
		ElseIf num > 55.4F AndAlso num < 79F Then
			Me.ChangeAngle(True, 3)
		ElseIf num > 79F AndAlso num < 102.6F Then
			Me.ChangeAngle(True, 4)
		ElseIf num > 102.6F AndAlso num < 126.2F Then
			Me.ChangeAngle(True, 5)
		ElseIf num > 126.2F AndAlso num < 149.8F Then
			Me.ChangeAngle(True, 6)
		ElseIf num > 149.8F AndAlso num < 164.75F Then
			Me.ChangeAngle(True, 7)
		ElseIf num > 164.75F AndAlso num < 195.25F Then
			Me.ChangeAngle(False, 8)
		ElseIf num > 195.25F AndAlso num < 218.85F Then
			Me.ChangeAngle(False, 7)
		ElseIf num > 218.85F AndAlso num < 242.45F Then
			Me.ChangeAngle(False, 6)
		ElseIf num > 242.45F AndAlso num < 259F Then
			Me.ChangeAngle(False, 5)
		ElseIf num > 259F AndAlso num < 282.6F Then
			Me.ChangeAngle(False, 4)
		ElseIf num > 282.6F AndAlso num < 306.2F Then
			Me.ChangeAngle(False, 3)
		ElseIf num > 306.2F AndAlso num < 329.8F Then
			Me.ChangeAngle(False, 2)
		ElseIf num > 329.8F AndAlso num < 344F Then
			Me.ChangeAngle(False, 1)
		End If
	End Sub

	' Token: 0x060014A9 RID: 5289 RVA: 0x000B8DDC File Offset: 0x000B71DC
	Private Sub ChangeAngle(flipSprite As Boolean, layerIndex As Integer)
		Me.currentAngle = layerIndex
		If Not Me.isCurved <> (layerIndex = 3 OrElse layerIndex = 4 OrElse layerIndex = 5) Then
			Me.flameAnimator.Play(If((layerIndex <> 3 AndAlso layerIndex <> 4 AndAlso layerIndex <> 5), "Curve", "Straight"), 0, Me.flameAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime)
			Me.isCurved = Not Me.isCurved
		End If
		For Each gameObject As GameObject In Me.terrierLayers
			gameObject.SetActive(gameObject Is Me.terrierLayers(layerIndex))
		Next
		Me.rends(layerIndex).flipX = flipSprite
	End Sub

	' Token: 0x060014AA RID: 5290 RVA: 0x000B8EA8 File Offset: 0x000B72A8
	Private Sub FixedUpdate()
		If Not Me.IsDead Then
			Me.smokeTimer += CupheadTime.FixedDelta * If((Not Me.introFinished), 0.2F, If((Not Me.gettingEaten), 1F, 0.1F))
			If Me.smokeTimer > Me.smokeDelay Then
				Me.smokeTimer -= Me.smokeDelay
				CType(Level.Current, AirplaneLevel).CreateSmokeFX(Me.flame.transform.position, If((Not Me.introFinished), (MathUtils.AngleToDirection(Me.flame.transform.eulerAngles.z - 90F) * 300F), Vector2.zero), Me.hp < Me.smokingThreshold, Me.rends(Me.currentAngle).sortingLayerID, If((Me.currentAngle > 4), 30, (-1)))
			End If
		End If
	End Sub

	' Token: 0x060014AB RID: 5291 RVA: 0x000B8FC0 File Offset: 0x000B73C0
	Private Sub LateUpdate()
		If Me.rends(9).sprite Is Nothing Then
			Me.CheckAllAnimations()
		End If
		If Me.introFinished Then
			Me.flame.sortingLayerID = Me.rends(Me.currentAngle).sortingLayerID
			Me.flame.sortingOrder = Me.rends(Me.currentAngle).sortingOrder - 1
			Me.flame.transform.localPosition = New Vector3(Me.flameOffset(Me.currentAngle).x * CSng(If((Not Me.rends(Me.currentAngle).flipX), 1, (-1))), Me.flameOffset(Me.currentAngle).y)
		End If
	End Sub

	' Token: 0x060014AC RID: 5292 RVA: 0x000B9094 File Offset: 0x000B7494
	Public Function RelativeAngle() As Single
		If Not Me.isClockwise Then
			Return 6.2831855F - Me.angle
		End If
		Return Me.angle
	End Function

	' Token: 0x060014AD RID: 5293 RVA: 0x000B90B4 File Offset: 0x000B74B4
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		Dim array As Single() = New Single() { 344F, 8.2F, 31.8F, 55.4F, 329.8F, 306.2F, 282.6F, 164.75F, 242.45F, 218.85F, 195.25F, 102.6F, 126.2F, 149.8F, 79F, 259F }
		Dim vector As Vector3 = Vector3.zero
		Dim num As Single = 400F
		For i As Integer = 0 To array.Length - 1
			vector = MathUtils.AngleToDirection(array(i) + 90F)
			If array(i) = 344F OrElse array(i) = 164.75F OrElse array(i) = 259F OrElse array(i) = 79F Then
				Gizmos.color = Color.blue
			ElseIf array(i) = 195.25F OrElse array(i) = 282.6F OrElse array(i) = 8.2F OrElse array(i) = 102.6F Then
				Gizmos.color = Color.green
			Else
				Gizmos.color = Color.red
			End If
			Gizmos.DrawLine(Vector3.zero, vector * num)
		Next
	End Sub

	' Token: 0x060014AE RID: 5294 RVA: 0x000B91AF File Offset: 0x000B75AF
	Private Sub SFX_DOGFIGHT_P2_TerrierJetpack_BarkShoot()
		AudioManager.Play("sfx_dlc_dogfight_p2_terrierjetpack_barkshoot")
	End Sub

	' Token: 0x060014AF RID: 5295 RVA: 0x000B91BB File Offset: 0x000B75BB
	Private Sub SFX_DOGFIGHT_P2_TerrierJetpack_Explosion()
		AudioManager.Play("sfx_dlc_dogfight_p2_terrierjetpack_explosion")
	End Sub

	' Token: 0x060014B0 RID: 5296 RVA: 0x000B91C8 File Offset: 0x000B75C8
	Private Iterator Function SFX_DOGFIGHT_P2_TerrierJetpack_DeathBark_cr(id As Integer) As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 0.5F)
		AudioManager.Play("sfx_dlc_dogfight_p2_terrierjetpack_dmgdeath_0" + id)
		Return
	End Function

	' Token: 0x060014B1 RID: 5297 RVA: 0x000B91EC File Offset: 0x000B75EC
	Private Sub WORKAROUND_NullifyFields()
		Me.coll = Nothing
		Me.regularProjectile = Nothing
		Me.pinkProjectile = Nothing
		Me.terrierLayers = Nothing
		Me.deathRenderer = Nothing
		Me.rends = Nothing
		Me.flameOffset = Nothing
		Me.flame = Nothing
		Me.flameAnimator = Nothing
		Me.barkFXRenderer = Nothing
		Me.barkFXAnimator = Nothing
		Me.pivotPoint = Nothing
		Me.damageDealer = Nothing
	End Sub

	' Token: 0x04001DE0 RID: 7648
	Private Shared OnShockParameterID As Integer = Animator.StringToHash("OnShock")

	' Token: 0x04001DE3 RID: 7651
	Private Const NINETY_DEGREES As Single = 90F

	' Token: 0x04001DE4 RID: 7652
	Private Const THREE_SIXTY As Single = 360F

	' Token: 0x04001DE5 RID: 7653
	Private Const LOOP_SIZE_Y As Single = 365F

	' Token: 0x04001DE6 RID: 7654
	Private Const LOOP_SIZE_X As Single = 750F

	' Token: 0x04001DE7 RID: 7655
	Private Const LOOP_SIZE_X_SECRET_INTRO As Single = 1000F

	' Token: 0x04001DE8 RID: 7656
	Private Const LOOP_SIZE_INTRO_MOD As Single = 0.9F

	' Token: 0x04001DE9 RID: 7657
	Private Const TIME_TO_FULL_LOOP_SIZE As Single = 1F

	' Token: 0x04001DEA RID: 7658
	Private Const UP As Single = 344F

	' Token: 0x04001DEB RID: 7659
	Private Const UP_RIGHT_1 As Single = 8.2F

	' Token: 0x04001DEC RID: 7660
	Private Const UP_RIGHT_2 As Single = 31.8F

	' Token: 0x04001DED RID: 7661
	Private Const UP_RIGHT_3 As Single = 55.4F

	' Token: 0x04001DEE RID: 7662
	Private Const RIGHT As Single = 79F

	' Token: 0x04001DEF RID: 7663
	Private Const DOWN_RIGHT_1 As Single = 102.6F

	' Token: 0x04001DF0 RID: 7664
	Private Const DOWN_RIGHT_2 As Single = 126.2F

	' Token: 0x04001DF1 RID: 7665
	Private Const DOWN_RIGHT_3 As Single = 149.8F

	' Token: 0x04001DF2 RID: 7666
	Private Const DOWN As Single = 164.75F

	' Token: 0x04001DF3 RID: 7667
	Private Const DOWN_LEFT_1 As Single = 242.45F

	' Token: 0x04001DF4 RID: 7668
	Private Const DOWN_LEFT_2 As Single = 218.85F

	' Token: 0x04001DF5 RID: 7669
	Private Const DOWN_LEFT_3 As Single = 195.25F

	' Token: 0x04001DF6 RID: 7670
	Private Const LEFT As Single = 259F

	' Token: 0x04001DF7 RID: 7671
	Private Const UP_LEFT_1 As Single = 329.8F

	' Token: 0x04001DF8 RID: 7672
	Private Const UP_LEFT_2 As Single = 306.2F

	' Token: 0x04001DF9 RID: 7673
	Private Const UP_LEFT_3 As Single = 282.6F

	' Token: 0x04001DFA RID: 7674
	<SerializeField()>
	Private coll As BoxCollider2D

	' Token: 0x04001DFB RID: 7675
	<SerializeField()>
	Private regularProjectile As AirplaneLevelTerrierBullet

	' Token: 0x04001DFC RID: 7676
	<SerializeField()>
	Private pinkProjectile As AirplaneLevelTerrierBullet

	' Token: 0x04001DFD RID: 7677
	<SerializeField()>
	Private terrierLayers As GameObject()

	' Token: 0x04001DFE RID: 7678
	<SerializeField()>
	Private deathRenderer As SpriteRenderer

	' Token: 0x04001DFF RID: 7679
	<SerializeField()>
	Private rends As SpriteRenderer()

	' Token: 0x04001E00 RID: 7680
	<SerializeField()>
	Private flameOffset As Vector3()

	' Token: 0x04001E01 RID: 7681
	<SerializeField()>
	Private flame As SpriteRenderer

	' Token: 0x04001E02 RID: 7682
	<SerializeField()>
	Private flameAnimator As Animator

	' Token: 0x04001E03 RID: 7683
	<SerializeField()>
	Private barkFXRenderer As SpriteRenderer

	' Token: 0x04001E04 RID: 7684
	<SerializeField()>
	Private barkFXAnimator As Animator

	' Token: 0x04001E05 RID: 7685
	Private properties As LevelProperties.Airplane.Terriers

	' Token: 0x04001E06 RID: 7686
	Public angle As Single

	' Token: 0x04001E07 RID: 7687
	Private hp As Single

	' Token: 0x04001E08 RID: 7688
	Private smokingThreshold As Single

	' Token: 0x04001E09 RID: 7689
	Private index As Integer

	' Token: 0x04001E0A RID: 7690
	Private isClockwise As Boolean

	' Token: 0x04001E0B RID: 7691
	Private isPink As Boolean

	' Token: 0x04001E0C RID: 7692
	Private isWow As Boolean

	' Token: 0x04001E0D RID: 7693
	Private isSmoking As Boolean

	' Token: 0x04001E0E RID: 7694
	Private gettingEaten As Boolean

	' Token: 0x04001E0F RID: 7695
	Private pivotPoint As Transform

	' Token: 0x04001E10 RID: 7696
	Private damageDealer As DamageDealer

	' Token: 0x04001E11 RID: 7697
	Private damageReceiver As DamageReceiver

	' Token: 0x04001E12 RID: 7698
	Private pivotOffset As Vector2

	' Token: 0x04001E13 RID: 7699
	<SerializeField()>
	Private wobbleX As Single = 10F

	' Token: 0x04001E14 RID: 7700
	<SerializeField()>
	Private wobbleY As Single = 10F

	' Token: 0x04001E15 RID: 7701
	<SerializeField()>
	Private wobbleSpeed As Single = 1F

	' Token: 0x04001E16 RID: 7702
	Private wobbleTimer As Single

	' Token: 0x04001E17 RID: 7703
	Private wobbleModifier As Single = 1F

	' Token: 0x04001E18 RID: 7704
	Private rotationSpeed As Single

	' Token: 0x04001E19 RID: 7705
	Private loopSizeX As Single

	' Token: 0x04001E1A RID: 7706
	Private loopSizeY As Single

	' Token: 0x04001E1B RID: 7707
	Private isCurved As Boolean

	' Token: 0x04001E1C RID: 7708
	Private currentAngle As Integer

	' Token: 0x04001E1D RID: 7709
	Private smokeDelay As Single = 0.02F

	' Token: 0x04001E1E RID: 7710
	Private smokeTimer As Single

	' Token: 0x04001E1F RID: 7711
	Private introFinished As Boolean

	' Token: 0x04001E20 RID: 7712
	Public lastOne As Boolean

	' Token: 0x04001E21 RID: 7713
	Private rotationOffset As Vector3
End Class
