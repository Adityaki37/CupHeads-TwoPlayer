Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200078D RID: 1933
Public Class RumRunnersLevelGrub
	Inherits AbstractProjectile

	' Token: 0x170003EE RID: 1006
	' (get) Token: 0x06002AB6 RID: 10934 RVA: 0x0018F101 File Offset: 0x0018D501
	' (set) Token: 0x06002AB7 RID: 10935 RVA: 0x0018F109 File Offset: 0x0018D509
	Public Property x As Integer

	' Token: 0x170003EF RID: 1007
	' (get) Token: 0x06002AB8 RID: 10936 RVA: 0x0018F112 File Offset: 0x0018D512
	' (set) Token: 0x06002AB9 RID: 10937 RVA: 0x0018F11A File Offset: 0x0018D51A
	Public Property y As Integer

	' Token: 0x170003F0 RID: 1008
	' (get) Token: 0x06002ABA RID: 10938 RVA: 0x0018F123 File Offset: 0x0018D523
	' (set) Token: 0x06002ABB RID: 10939 RVA: 0x0018F12B File Offset: 0x0018D52B
	Public Property speed As Single

	' Token: 0x170003F1 RID: 1009
	' (get) Token: 0x06002ABC RID: 10940 RVA: 0x0018F134 File Offset: 0x0018D534
	' (set) Token: 0x06002ABD RID: 10941 RVA: 0x0018F13C File Offset: 0x0018D53C
	Public Property moving As Boolean

	' Token: 0x170003F2 RID: 1010
	' (get) Token: 0x06002ABE RID: 10942 RVA: 0x0018F145 File Offset: 0x0018D545
	' (set) Token: 0x06002ABF RID: 10943 RVA: 0x0018F14D File Offset: 0x0018D54D
	Public Property startedEntering As Boolean

	' Token: 0x06002AC0 RID: 10944 RVA: 0x0018F158 File Offset: 0x0018D558
	Public Function Create(path As RumRunnersLevelGrubPath, rotation As Single, speed As Single, time As Single, hp As Single, parent As RumRunnersLevelSpider, enterVariant As Integer, [variant] As Integer, spawnOrder As Integer, x As Integer, y As Integer) As RumRunnersLevelGrub
		Dim rumRunnersLevelGrub As RumRunnersLevelGrub = TryCast(MyBase.Create(path.start, rotation), RumRunnersLevelGrub)
		rumRunnersLevelGrub.transform.localScale = New Vector3(0.3F * Mathf.Sign(path.transform.position.x - path.start.x), 0.3F)
		rumRunnersLevelGrub.path = path
		rumRunnersLevelGrub.speed = speed
		rumRunnersLevelGrub.time = time
		rumRunnersLevelGrub.hp = hp
		rumRunnersLevelGrub.parent = parent
		rumRunnersLevelGrub.GetComponent(Of Collider2D)().enabled = True
		rumRunnersLevelGrub.enterVariant = enterVariant
		rumRunnersLevelGrub.[variant] = [variant]
		rumRunnersLevelGrub.animator.SetInteger("Variant", enterVariant)
		rumRunnersLevelGrub.animator.SetInteger("BlinkLoops", Global.UnityEngine.Random.Range(CInt(RumRunnersLevelGrub.BlinkLoopsRange.minimum), CInt(RumRunnersLevelGrub.BlinkLoopsRange.maximum) + 1))
		rumRunnersLevelGrub.animator.Play("Start", 0, 0F)
		rumRunnersLevelGrub.spawnOrder = spawnOrder
		rumRunnersLevelGrub.shadowDist = Me.shadowTransform.localPosition.y
		rumRunnersLevelGrub.x = x
		rumRunnersLevelGrub.y = y
		Return rumRunnersLevelGrub
	End Function

	' Token: 0x06002AC1 RID: 10945 RVA: 0x0018F288 File Offset: 0x0018D688
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.collider = MyBase.GetComponent(Of Collider2D)()
		If MyBase.GetComponent(Of DamageReceiver)() Then
			Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
			AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.onDamageTaken
		End If
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x06002AC2 RID: 10946 RVA: 0x0018F2E7 File Offset: 0x0018D6E7
	Protected Overrides Sub OnDieDistance()
	End Sub

	' Token: 0x06002AC3 RID: 10947 RVA: 0x0018F2E9 File Offset: 0x0018D6E9
	Protected Overrides Sub OnDieLifetime()
	End Sub

	' Token: 0x06002AC4 RID: 10948 RVA: 0x0018F2EC File Offset: 0x0018D6EC
	Protected Overrides Sub FixedUpdate()
		MyBase.FixedUpdate()
		If Me.moving Then
			Me.horizontalSpeedEasingTime += CupheadTime.FixedDelta
			Me.basePos.x = Me.basePos.x + Mathf.Lerp(0F, Me.speed, Me.horizontalSpeedEasingTime / 0.5F) * CupheadTime.FixedDelta
			If Me.finishedEntering Then
				Me.basePos.y = RumRunnersLevel.GroundWalkingPosY(Me.basePos, Nothing, Me.yOffset, 200F)
			End If
			If Me.basePos.x > 960F OrElse Me.basePos.x < -960F Then
				Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
			End If
			MyBase.transform.position = Me.basePos + Me.wobblePos()
			If Me.finishedEntering Then
				Me.shadowTransform.position = New Vector3(MyBase.transform.position.x, Me.basePos.y + Me.shadowDist)
				Me.wobbleTimer += Me.wobbleSpeed * CupheadTime.FixedDelta
			End If
		End If
	End Sub

	' Token: 0x06002AC5 RID: 10949 RVA: 0x0018F42C File Offset: 0x0018D82C
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If Me.damageDealer IsNot Nothing AndAlso phase = CollisionPhase.Enter Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06002AC6 RID: 10950 RVA: 0x0018F454 File Offset: 0x0018D854
	Private Sub onDamageTaken(info As DamageDealer.DamageInfo)
		Me.hp -= info.damage
		If Me.hp <= 0F Then
			Level.Current.RegisterMinionKilled()
			Me.die(True)
		End If
	End Sub

	' Token: 0x06002AC7 RID: 10951 RVA: 0x0018F48A File Offset: 0x0018D88A
	Private Function wobblePos() As Vector3
		Return New Vector3(Mathf.Sin(Me.wobbleTimer * 3F) * Me.wobbleX, Mathf.Sin(Me.wobbleTimer * 2F) * Me.wobbleY, 0F)
	End Function

	' Token: 0x06002AC8 RID: 10952 RVA: 0x0018F4C8 File Offset: 0x0018D8C8
	Public Function GetTimeToMove() As Single
		If Me.moving Then
			Return 0F
		End If
		If Not Me.startedEntering Then
			Return(15F + RumRunnersLevelGrub.flipEndClipLength(Me.[variant])) * 0.041666668F
		End If
		Dim num As Integer = Animator.StringToHash(MyBase.animator.GetLayerName(0) + ".Flip")
		If MyBase.animator.GetCurrentAnimatorStateInfo(0).fullPathHash = num Then
			Return(15F * (1F - MyBase.animator.GetCurrentAnimatorStateInfo(0).normalizedTime) + RumRunnersLevelGrub.flipEndClipLength(Me.[variant])) * 1F / 24F
		End If
		Return RumRunnersLevelGrub.flipEndClipLength(Me.[variant]) * (1F - MyBase.animator.GetCurrentAnimatorStateInfo(0).normalizedTime) * 1F / 24F
	End Function

	' Token: 0x06002AC9 RID: 10953 RVA: 0x0018F5B0 File Offset: 0x0018D9B0
	Private Iterator Function move_cr() As IEnumerator
		Me.collider.enabled = False
		Dim t As Single = 0F
		Dim destinationPoint As Vector3 = New Vector3(Me.path.GetPoint(1F).x, RumRunnersLevel.GroundWalkingPosY(Me.path.GetPoint(1F), Nothing, Me.yOffset, 200F) + 4F)
		Dim pathOffset As Single = destinationPoint.y - Me.path.GetPoint(1F).y
		Dim orientation As Single = Mathf.Sign(MyBase.transform.position.x - destinationPoint.x)
		While t <= 1F
			t += 0.033333335F
			Me.setSortingOrder(75 + CInt((t * 10F)))
			If t > 0.9F Then
				MyBase.animator.SetTrigger("EndFlyUp")
			End If
			If t + 0.033333335F >= Me.path.forceFGSet AndAlso t < Me.path.forceFGSet Then
				Me.setSortingLayer("Default")
			End If
			MyBase.transform.position = Me.path.GetPoint(EaseUtils.EaseOutSine(0F, 1F, t)) + Vector2.up * pathOffset
			MyBase.transform.localScale = New Vector3(EaseUtils.EaseInCubic(0.3F, 0.8F, t) * orientation, EaseUtils.EaseInCubic(0.3F, 0.8F, t))
			Yield CupheadTime.WaitForSeconds(Me, 0.033333335F)
		End While
		MyBase.animator.SetTrigger("EndFlyUp")
		MyBase.transform.localScale = New Vector3(Mathf.Sign(MyBase.transform.localScale.x) * 0.8F, 0.8F)
		MyBase.transform.position = destinationPoint
		Dim vel As Vector3 = destinationPoint - (Me.path.GetPoint(EaseUtils.EaseOutSine(0F, 1F, t - 0.033333335F)) + Vector2.up * pathOffset)
		t = 0F
		While t < Me.time OrElse (Me.parent AndAlso Not Me.parent.GrubCanEnter(MyBase.transform.position, Me.GetTimeToMove()))
			MyBase.transform.position = destinationPoint + Mathf.Sin(t * 10F) * vel * (Mathf.InverseLerp(1F, 0F, t) * 10F)
			vel = vel.magnitude * MathUtils.AngleToDirection(MathUtils.DirectionToAngle(vel) + 3F)
			Yield CupheadTime.WaitForSeconds(Me, 0.033333335F)
			t += 0.033333335F
		End While
		MyBase.transform.position = destinationPoint
		Dim onGroundPoint As Vector3 = New Vector3(Me.path.GetPoint(1F).x, RumRunnersLevel.GroundWalkingPosY(Me.path.GetPoint(1F), Nothing, Me.yOffset, 200F))
		Dim timeToMove As Single = Me.GetTimeToMove()
		Dim flipLength As Single = 0.625F
		t = 0F
		MyBase.animator.SetTrigger("Enter")
		Me.startedEntering = True
		Me.SFX_RUMRUN_Grub_VocalIntro()
		Me.SFX_RUMRUN_Grub_FlyingLoop()
		While t < timeToMove
			Dim moveTime As Single = Mathf.Clamp(t / flipLength, 0F, 1F)
			Dim flipTime As Single = Mathf.Clamp(t / flipLength, 0F, 1F)
			MyBase.transform.position = New Vector3(MyBase.transform.position.x, Mathf.Lerp(destinationPoint.y, onGroundPoint.y, moveTime) + Mathf.Sin(flipTime * 3.1415927F) * 30F)
			MyBase.transform.localScale = New Vector3(Mathf.Lerp(0.8F, 1F, moveTime) * Mathf.Sign(MyBase.transform.localScale.x), Mathf.Lerp(0.8F, 1F, moveTime))
			Me.basePos = MyBase.transform.position
			Me.shadowTransform.position = New Vector3(MyBase.transform.position.x, onGroundPoint.y + Me.shadowDist)
			Yield CupheadTime.WaitForSeconds(Me, 0.033333335F)
			t += 0.033333335F
		End While
		MyBase.transform.position = New Vector3(MyBase.transform.position.x, onGroundPoint.y)
		Me.finishedEntering = True
		Return
	End Function

	' Token: 0x06002ACA RID: 10954 RVA: 0x0018F5CB File Offset: 0x0018D9CB
	Private Sub die(playSound As Boolean)
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Me.deathEffect.Create(MyBase.transform.position)
		Me.SFX_RUMRUN_Grub_FlyingLoopStop()
		If playSound Then
			Me.SFX_RUMRUN_Grub_Lackey_DiePoof()
		End If
	End Sub

	' Token: 0x06002ACB RID: 10955 RVA: 0x0018F601 File Offset: 0x0018DA01
	Private Sub AniEvent_EnableCollision()
		Me.collider.enabled = True
	End Sub

	' Token: 0x06002ACC RID: 10956 RVA: 0x0018F60F File Offset: 0x0018DA0F
	Private Sub AniEvent_StartMoving()
		Me.moving = True
	End Sub

	' Token: 0x06002ACD RID: 10957 RVA: 0x0018F618 File Offset: 0x0018DA18
	Private Sub AniEvent_OnFlip()
		Me.setSortingLayer("Enemies")
		Me.setSortingOrder(Me.spawnOrder + 1)
		MyBase.transform.localScale = New Vector3(Mathf.Abs(MyBase.transform.localScale.x) * Mathf.Sign(MyBase.transform.position.x - PlayerManager.GetNext().transform.position.x), MyBase.transform.localScale.y)
		Me.speed *= -MyBase.transform.localScale.x
		MyBase.animator.SetInteger("Variant", Me.[variant])
	End Sub

	' Token: 0x06002ACE RID: 10958 RVA: 0x0018F6E4 File Offset: 0x0018DAE4
	Private Sub animationEvent_BlinkCompleted()
		MyBase.animator.SetInteger("BlinkLoops", Global.UnityEngine.Random.Range(CInt(RumRunnersLevelGrub.BlinkLoopsRange.minimum), CInt(RumRunnersLevelGrub.BlinkLoopsRange.maximum) + 1))
	End Sub

	' Token: 0x06002ACF RID: 10959 RVA: 0x0018F724 File Offset: 0x0018DB24
	Private Sub setSortingLayer(layerName As String)
		Me.mainRenderer.sortingLayerName = layerName
		Me.blinkRenderer.sortingLayerName = layerName
	End Sub

	' Token: 0x06002AD0 RID: 10960 RVA: 0x0018F73E File Offset: 0x0018DB3E
	Private Sub setSortingOrder(order As Integer)
		Me.mainRenderer.sortingOrder = order
		Me.blinkRenderer.sortingOrder = order + 1
	End Sub

	' Token: 0x06002AD1 RID: 10961 RVA: 0x0018F75A File Offset: 0x0018DB5A
	Private Sub SFX_RUMRUN_Grub_Lackey_DiePoof()
		AudioManager.Play("sfx_dlc_rumrun_lackey_poof")
		Me.emitAudioFromObject.Add("sfx_dlc_rumrun_lackey_poof")
	End Sub

	' Token: 0x06002AD2 RID: 10962 RVA: 0x0018F776 File Offset: 0x0018DB76
	Private Sub SFX_RUMRUN_Grub_VocalIntro()
	End Sub

	' Token: 0x06002AD3 RID: 10963 RVA: 0x0018F778 File Offset: 0x0018DB78
	Private Sub SFX_RUMRUN_Grub_FlyingLoop()
		AudioManager.Play("sfx_dlc_rumrun_grub_flying_loop")
		Me.emitAudioFromObject.Add("sfx_dlc_rumrun_grub_flying_loop")
	End Sub

	' Token: 0x06002AD4 RID: 10964 RVA: 0x0018F794 File Offset: 0x0018DB94
	Private Sub SFX_RUMRUN_Grub_FlyingLoopStop()
		AudioManager.[Stop]("sfx_dlc_rumrun_grub_flying_loop")
	End Sub

	' Token: 0x04003373 RID: 13171
	Private Shared enterEndClipLength As Single() = New Single() { 10F, 11F, 16F }

	' Token: 0x04003374 RID: 13172
	Private Const flipClipLength As Single = 15F

	' Token: 0x04003375 RID: 13173
	Private Shared flipEndClipLength As Single() = New Single() { 17F, 12F, 9F, 19F }

	' Token: 0x04003376 RID: 13174
	Private Const START_SIZE As Single = 0.3F

	' Token: 0x04003377 RID: 13175
	Private Const WAIT_SIZE As Single = 0.8F

	' Token: 0x04003378 RID: 13176
	Private Const END_FLY_UP_TRIGGER As Single = 0.9F

	' Token: 0x04003379 RID: 13177
	Private Const OVERSHOOT As Single = 10F

	' Token: 0x0400337A RID: 13178
	Private Const FLIP_HEIGHT As Single = 30F

	' Token: 0x0400337B RID: 13179
	Private Const TIME_TO_FULL_X_SPEED As Single = 0.5F

	' Token: 0x0400337C RID: 13180
	Private Shared BlinkLoopsRange As Rangef = New Rangef(2F, 3F)

	' Token: 0x0400337D RID: 13181
	Private Const EnterYOffset As Single = 4F

	' Token: 0x0400337E RID: 13182
	<SerializeField()>
	Private yOffset As Single

	' Token: 0x0400337F RID: 13183
	<SerializeField()>
	Private mainRenderer As SpriteRenderer

	' Token: 0x04003380 RID: 13184
	<SerializeField()>
	Private blinkRenderer As SpriteRenderer

	' Token: 0x04003381 RID: 13185
	<SerializeField()>
	Private shadowTransform As Transform

	' Token: 0x04003382 RID: 13186
	<SerializeField()>
	Private wobbleX As Single = 10F

	' Token: 0x04003383 RID: 13187
	<SerializeField()>
	Private wobbleY As Single = 10F

	' Token: 0x04003384 RID: 13188
	<SerializeField()>
	Private wobbleSpeed As Single = 1F

	' Token: 0x04003385 RID: 13189
	<SerializeField()>
	Private deathEffect As Effect

	' Token: 0x0400338B RID: 13195
	Private time As Single

	' Token: 0x0400338C RID: 13196
	Private hp As Single

	' Token: 0x0400338D RID: 13197
	Private damageReceiver As DamageReceiver

	' Token: 0x0400338E RID: 13198
	Private parent As RumRunnersLevelSpider

	' Token: 0x0400338F RID: 13199
	Private collider As Collider2D

	' Token: 0x04003390 RID: 13200
	Private finishedEntering As Boolean

	' Token: 0x04003391 RID: 13201
	Private path As RumRunnersLevelGrubPath

	' Token: 0x04003392 RID: 13202
	Private enterVariant As Integer

	' Token: 0x04003393 RID: 13203
	Private [variant] As Integer

	' Token: 0x04003394 RID: 13204
	Private spawnOrder As Integer

	' Token: 0x04003395 RID: 13205
	Private wobbleTimer As Single

	' Token: 0x04003396 RID: 13206
	Private basePos As Vector3

	' Token: 0x04003397 RID: 13207
	Private shadowDist As Single

	' Token: 0x04003398 RID: 13208
	Private horizontalSpeedEasingTime As Single
End Class
