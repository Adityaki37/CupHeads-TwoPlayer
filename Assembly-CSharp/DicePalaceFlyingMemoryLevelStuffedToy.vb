Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020005CC RID: 1484
Public Class DicePalaceFlyingMemoryLevelStuffedToy
	Inherits LevelProperties.DicePalaceFlyingMemory.Entity

	' Token: 0x06001D0C RID: 7436 RVA: 0x0010A9D0 File Offset: 0x00108DD0
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.state = DicePalaceFlyingMemoryLevelStuffedToy.State.Closed
		MyBase.GetComponent(Of DamageReceiver)().enabled = False
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
	End Sub

	' Token: 0x06001D0D RID: 7437 RVA: 0x0010AA24 File Offset: 0x00108E24
	Public Overrides Sub LevelInit(properties As LevelProperties.DicePalaceFlyingMemory)
		MyBase.LevelInit(properties)
		Me.shotPattern = properties.CurrentState.stuffedToy.shotType.GetRandom().Split(New Char() { ","c })
		Me.shotIndex = 0
		AddHandler Level.Current.OnWinEvent, AddressOf Me.OnDeath
		MyBase.StartCoroutine(Me.intro_cr())
	End Sub

	' Token: 0x06001D0E RID: 7438 RVA: 0x0010AA8D File Offset: 0x00108E8D
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06001D0F RID: 7439 RVA: 0x0010AAAB File Offset: 0x00108EAB
	Protected Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06001D10 RID: 7440 RVA: 0x0010AAC3 File Offset: 0x00108EC3
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		If MyBase.properties.CurrentHealth > 0F Then
			MyBase.properties.DealDamage(info.damage)
		End If
	End Sub

	' Token: 0x06001D11 RID: 7441 RVA: 0x0010AAEB File Offset: 0x00108EEB
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.projectile = Nothing
		Me.spiralProjectile = Nothing
	End Sub

	' Token: 0x06001D12 RID: 7442 RVA: 0x0010AB04 File Offset: 0x00108F04
	Private Iterator Function intro_cr() As IEnumerator
		Dim t As Single = 0F
		Dim time As Single = 1.5F
		Dim [end] As Vector3 = New Vector3(MyBase.transform.position.x, 0F)
		Dim start As Vector3 = MyBase.transform.position
		While t < time
			Dim val As Single = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0F, 1F, t / time)
			MyBase.transform.position = Vector2.Lerp(start, [end], val)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		MyBase.transform.position = [end]
		MyBase.animator.SetTrigger("Continue")
		AudioManager.Play("dice_palace_memory_monkey_intro")
		Me.emitAudioFromObject.Add("dice_palace_memory_monkey_intro")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Intro", False, True)
		MyBase.StartCoroutine(Me.check_boundaries_cr())
		MyBase.StartCoroutine(Me.pick_angle_cr())
		Yield Nothing
		Return
	End Function

	' Token: 0x06001D13 RID: 7443 RVA: 0x0010AB20 File Offset: 0x00108F20
	Private Sub FireSingle(speed As Single)
		Dim [next] As AbstractPlayerController = PlayerManager.GetNext()
		Dim vector As Vector3 = [next].transform.position - MyBase.transform.position
		Dim num As Single = MathUtils.DirectionToAngle(vector)
		Me.projectile.Create(Me.projectileRoot.transform.position, num, speed)
	End Sub

	' Token: 0x06001D14 RID: 7444 RVA: 0x0010AB80 File Offset: 0x00108F80
	Private Sub FireSpreadshot()
		Dim stuffedToy As LevelProperties.DicePalaceFlyingMemory.StuffedToy = MyBase.properties.CurrentState.stuffedToy
		For i As Integer = 0 To stuffedToy.spreadBullets - 1
			Dim floatAt As Single = stuffedToy.spreadAngle.GetFloatAt(CSng(i) / (CSng(stuffedToy.spreadBullets) - 1F))
			Me.projectile.Create(Me.projectileRoot.transform.position, floatAt, stuffedToy.spreadSpeed, stuffedToy.musicDeathTimer)
		Next
	End Sub

	' Token: 0x06001D15 RID: 7445 RVA: 0x0010ABFC File Offset: 0x00108FFC
	Private Sub FireSpiral()
		Dim stuffedToy As LevelProperties.DicePalaceFlyingMemory.StuffedToy = MyBase.properties.CurrentState.stuffedToy
		Me.spiralProjectile.Create(Me.projectileRoot.transform.position, 0F, stuffedToy.spiralSpeed, stuffedToy.spiralMovementRate, 1)
	End Sub

	' Token: 0x06001D16 RID: 7446 RVA: 0x0010AC50 File Offset: 0x00109050
	Private Iterator Function punishment_cr() As IEnumerator
		Me.timer = 0F
		Dim p As LevelProperties.DicePalaceFlyingMemory.StuffedToy = MyBase.properties.CurrentState.stuffedToy
		Dim speedUp As Boolean = True
		Me.startedPunishment = True
		MyBase.animator.SetTrigger("OnNoMatch")
		AudioManager.PlayLoop("dice_palace_memory_monkey_shake")
		Me.emitAudioFromObject.Add("dice_palace_memory_monkey_shake")
		While speedUp
			If Me.speed >= p.punishSpeed Then
				speedUp = False
				Exit While
			End If
			Me.speed += p.incrementSpeedBy
			Yield Nothing
		End While
		Me.speed = p.punishSpeed
		While Me.timer < p.punishTime AndAlso Me.state = DicePalaceFlyingMemoryLevelStuffedToy.State.Closed
			Me.timer += CupheadTime.Delta
			Yield Nothing
		End While
		MyBase.animator.SetTrigger("Continue")
		While Me.speed > p.bounceSpeed
			Me.speed -= p.incrementSpeedBy
			Yield Nothing
		End While
		AudioManager.[Stop]("dice_palace_memory_monkey_shake")
		Me.speed = p.bounceSpeed
		Me.startedPunishment = False
		Me.SFXAllowAnticipation()
		Yield Nothing
		Return
	End Function

	' Token: 0x06001D17 RID: 7447 RVA: 0x0010AC6C File Offset: 0x0010906C
	Private Iterator Function pick_angle_cr() As IEnumerator
		Dim p As LevelProperties.DicePalaceFlyingMemory.StuffedToy = MyBase.properties.CurrentState.stuffedToy
		Dim angleString As String() = p.angleString.GetRandom().Split(New Char() { ","c })
		Dim countString As String() = p.bounceCount.GetRandom().Split(New Char() { ","c })
		Dim angleAddString As String() = p.angleAdditionString.GetRandom().Split(New Char() { ","c })
		Dim angleIndex As Integer = Global.UnityEngine.Random.Range(0, angleString.Length)
		Dim maxCountIndex As Integer = Global.UnityEngine.Random.Range(0, countString.Length)
		Dim angleAddIndex As Integer = Global.UnityEngine.Random.Range(0, angleAddString.Length)
		Dim chosenAngle As Single = 0F
		Dim angle As Single = 0F
		Dim angleAdd As Single = 0F
		Dim t As Single = 0F
		Dim dirChangeTime As Single = p.directionChangeDelay
		Parser.FloatTryParse(angleString(angleIndex), angle)
		Parser.FloatTryParse(countString(maxCountIndex), Me.maxCount)
		Parser.FloatTryParse(angleAddString(angleAddIndex), angleAdd)
		Me.maxCount = 0F
		Me.currentAngle = angle
		MyBase.transform.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(angle))
		Me.sprite.transform.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(0F))
		MyBase.StartCoroutine(Me.move_cr())
		Yield Nothing
		While True
			If(CSng(Me.bounceCounter) >= Me.maxCount AndAlso Me.state = DicePalaceFlyingMemoryLevelStuffedToy.State.Closed) OrElse Me.guessedWrong Then
				If Me.guessedWrong Then
					If Not Me.startedPunishment Then
						MyBase.StartCoroutine(Me.punishment_cr())
					Else
						Me.timer = 0F
					End If
					While Me.currentlyColliding
						Yield New WaitForEndOfFrame()
					End While
					Me.isMoving = False
					While t < dirChangeTime
						t += CupheadTime.FixedDelta
						Yield New WaitForFixedUpdate()
					End While
					Me.isMoving = True
					While Me.currentlyColliding
						Yield New WaitForEndOfFrame()
					End While
					angleIndex = (angleIndex + 1) Mod angleString.Length
					Parser.FloatTryParse(angleString(angleIndex), angle)
					t = 0F
				End If
				angleAddIndex = (angleAddIndex + 1) Mod angleAddString.Length
				maxCountIndex = (maxCountIndex + 1) Mod countString.Length
				Parser.FloatTryParse(countString(maxCountIndex), Me.maxCount)
				Parser.FloatTryParse(angleAddString(angleAddIndex), angleAdd)
				If Not Me.guessedWrong Then
					chosenAngle = Me.currentAngle + angleAdd
				Else
					chosenAngle = angle
				End If
				Me.bounceCounter = 0
				Me.guessedWrong = False
				MyBase.transform.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(chosenAngle))
				Me.sprite.transform.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(0F))
				Me.velocity = MyBase.transform.right
			End If
			Yield New WaitForFixedUpdate()
		End While
		Return
	End Function

	' Token: 0x06001D18 RID: 7448 RVA: 0x0010AC87 File Offset: 0x00109087
	Public Sub Open()
		Me.state = DicePalaceFlyingMemoryLevelStuffedToy.State.Open
		MyBase.animator.SetTrigger("OnMatch")
		MyBase.animator.SetBool("OnClosing", False)
		MyBase.StartCoroutine(Me.open_cr())
	End Sub

	' Token: 0x06001D19 RID: 7449 RVA: 0x0010ACC0 File Offset: 0x001090C0
	Private Iterator Function open_cr() As IEnumerator
		Dim p As LevelProperties.DicePalaceFlyingMemory.StuffedToy = MyBase.properties.CurrentState.stuffedToy
		Dim shot As Integer = 0
		AudioManager.[Stop]("dice_palace_memory_monkey_shake")
		Yield MyBase.animator.WaitForAnimationToStart(Me, "Open_Attack_A", False)
		Me.isMoving = False
		MyBase.GetComponent(Of DamageReceiver)().enabled = True
		While Me.currentlyColliding
			Yield Nothing
		End While
		Yield CupheadTime.WaitForSeconds(Me, p.directionChangeDelay)
		Yield CupheadTime.WaitForSeconds(Me, p.attackAnti)
		Me.isMoving = True
		MyBase.animator.SetTrigger("Continue")
		While Me.state = DicePalaceFlyingMemoryLevelStuffedToy.State.Open
			Parser.IntTryParse(Me.shotPattern(Me.shotIndex), shot)
			Select Case shot
				Case 1
					Me.FireSingle(p.regularSpeed)
				Case 2
					Me.FireSpreadshot()
				Case 3
					Me.FireSpiral()
			End Select
			Yield Nothing
			Me.shotIndex = (Me.shotIndex + 1) Mod Me.shotPattern.Length
			Yield CupheadTime.WaitForSeconds(Me, p.shotDelayRange)
			If Me.state <> DicePalaceFlyingMemoryLevelStuffedToy.State.Open Then
				Exit While
			End If
			MyBase.animator.SetTrigger("OnAttack")
			Yield MyBase.animator.WaitForAnimationToStart(Me, "Open_Attack_B", False)
			Me.isMoving = False
			Yield Nothing
			Yield CupheadTime.WaitForSeconds(Me, p.attackAnti)
			MyBase.animator.SetTrigger("Continue")
			Me.isMoving = True
		End While
		Yield Nothing
		Return
	End Function

	' Token: 0x06001D1A RID: 7450 RVA: 0x0010ACDB File Offset: 0x001090DB
	Public Sub Closed()
		MyBase.StartCoroutine(Me.closed_cr())
	End Sub

	' Token: 0x06001D1B RID: 7451 RVA: 0x0010ACEC File Offset: 0x001090EC
	Private Iterator Function closed_cr() As IEnumerator
		Me.state = DicePalaceFlyingMemoryLevelStuffedToy.State.Closed
		MyBase.animator.SetBool("OnClosing", True)
		Yield MyBase.animator.WaitForAnimationToStart(Me, "Idle_Closed", False)
		MyBase.GetComponent(Of DamageReceiver)().enabled = False
		Yield Nothing
		Return
	End Function

	' Token: 0x06001D1C RID: 7452 RVA: 0x0010AD07 File Offset: 0x00109107
	Private Sub DisableDamageReceiver()
		MyBase.GetComponent(Of DamageReceiver)().enabled = False
	End Sub

	' Token: 0x06001D1D RID: 7453 RVA: 0x0010AD15 File Offset: 0x00109115
	Private Sub ChangeLayer(layer As Integer)
		Me.hand.GetComponent(Of SpriteRenderer)().sortingOrder = layer
	End Sub

	' Token: 0x06001D1E RID: 7454 RVA: 0x0010AD28 File Offset: 0x00109128
	Protected Iterator Function move_cr() As IEnumerator
		Dim soundLooping As Boolean = True
		Me.isMoving = True
		Me.velocity = MyBase.transform.right
		Me.speed = MyBase.properties.CurrentState.stuffedToy.bounceSpeed
		AudioManager.[Stop]("dice_palace_memory_monkey_shake")
		AudioManager.PlayLoop("dice_palace_memory_monkey_crane_movement")
		Me.emitAudioFromObject.Add("dice_palace_memory_monkey_crane_movement")
		While True
			If Me.isMoving Then
				If Not soundLooping Then
					AudioManager.PlayLoop("dice_palace_memory_monkey_crane_movement")
					Me.emitAudioFromObject.Add("dice_palace_memory_monkey_crane_movement")
					soundLooping = True
				End If
				MyBase.transform.position += MyBase.transform.right * Me.speed * CupheadTime.FixedDelta
			ElseIf soundLooping Then
				AudioManager.[Stop]("dice_palace_memory_monkey_crane_movement")
				soundLooping = False
				Me.SFXAnticipationActive = False
			End If
			Yield New WaitForFixedUpdate()
		End While
		Return
	End Function

	' Token: 0x06001D1F RID: 7455 RVA: 0x0010AD44 File Offset: 0x00109144
	Protected Overrides Sub OnCollisionCeiling(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionCeiling(hit, phase)
		If phase = CollisionPhase.Enter OrElse phase = CollisionPhase.Stay Then
			Me.currentlyColliding = True
		End If
		If phase = CollisionPhase.[Exit] Then
			Me.currentlyColliding = False
		End If
		If Me.currentlyColliding Then
			Dim vector As Vector3 = Me.velocity
			vector.y = Mathf.Min(vector.y, -vector.y)
			Me.ChangeDir(vector)
		End If
	End Sub

	' Token: 0x06001D20 RID: 7456 RVA: 0x0010ADB0 File Offset: 0x001091B0
	Protected Overrides Sub OnCollisionGround(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionGround(hit, phase)
		If phase = CollisionPhase.Enter OrElse phase = CollisionPhase.Stay Then
			Me.currentlyColliding = True
		End If
		If phase = CollisionPhase.[Exit] Then
			Me.currentlyColliding = False
		End If
		If Me.currentlyColliding Then
			Dim vector As Vector3 = Me.velocity
			vector.y = Mathf.Max(vector.y, -vector.y)
			Me.ChangeDir(vector)
		End If
	End Sub

	' Token: 0x06001D21 RID: 7457 RVA: 0x0010AE1C File Offset: 0x0010921C
	Protected Overrides Sub OnCollisionWalls(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionWalls(hit, phase)
		If phase = CollisionPhase.Enter OrElse phase = CollisionPhase.Stay Then
			Me.currentlyColliding = True
		End If
		If phase = CollisionPhase.[Exit] Then
			Me.currentlyColliding = False
		End If
		If Me.currentlyColliding Then
			Dim vector As Vector3 = Me.velocity
			If MyBase.transform.position.x > 0F Then
				vector.x = Mathf.Min(vector.x, -vector.x)
				Me.ChangeDir(vector)
			Else
				vector.x = Mathf.Max(vector.x, -vector.x)
				Me.ChangeDir(vector)
			End If
		End If
	End Sub

	' Token: 0x06001D22 RID: 7458 RVA: 0x0010AECC File Offset: 0x001092CC
	Protected Sub ChangeDir(newVelocity As Vector3)
		If Me.state = DicePalaceFlyingMemoryLevelStuffedToy.State.Closed Then
			Me.bounceCounter += 1
		End If
		Me.velocity = newVelocity
		Me.currentAngle = Mathf.Atan2(Me.velocity.y, Me.velocity.x) * 57.29578F
		MyBase.transform.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(Me.currentAngle))
		Me.sprite.transform.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(0F))
	End Sub

	' Token: 0x06001D23 RID: 7459 RVA: 0x0010AF79 File Offset: 0x00109379
	Private Sub OnDeath()
		Me.StopAllCoroutines()
		AudioManager.PlayLoop("dice_palace_memory_monkey_death")
		Me.emitAudioFromObject.Add("dice_palace_memory_monkey_death")
		MyBase.animator.SetTrigger("OnDeath")
		MyBase.GetComponent(Of Collider2D)().enabled = False
	End Sub

	' Token: 0x06001D24 RID: 7460 RVA: 0x0010AFB8 File Offset: 0x001093B8
	Private Iterator Function check_boundaries_cr() As IEnumerator
		While MyBase.transform.position.y <= 720F AndAlso MyBase.transform.position.y >= -720F AndAlso MyBase.transform.position.x >= -1280F AndAlso MyBase.transform.position.x <= 1280F
			Yield Nothing
		End While
		MyBase.properties.DealDamage(MyBase.properties.CurrentHealth)
		Return
	End Function

	' Token: 0x06001D25 RID: 7461 RVA: 0x0010AFD3 File Offset: 0x001093D3
	Private Sub AttackSFX()
		AudioManager.Play("dice_palace_memory_monkey_open_attack")
		Me.emitAudioFromObject.Add("dice_palace_memory_monkey_open_attack")
		Me.VOXAngryActive = False
	End Sub

	' Token: 0x06001D26 RID: 7462 RVA: 0x0010AFF6 File Offset: 0x001093F6
	Private Sub AttackEndSFX()
		AudioManager.Play("dice_palace_memory_monkey_attack_end")
		Me.emitAudioFromObject.Add("dice_palace_memory_monkey_attack_end")
	End Sub

	' Token: 0x06001D27 RID: 7463 RVA: 0x0010B012 File Offset: 0x00109412
	Private Sub SFXOpentoClose()
		AudioManager.Play("dice_palace_memory_monkey_open_to_close")
		Me.emitAudioFromObject.Add("dice_palace_memory_monkey_open_to_close")
	End Sub

	' Token: 0x06001D28 RID: 7464 RVA: 0x0010B02E File Offset: 0x0010942E
	Private Sub SFXShake()
		AudioManager.PlayLoop("shake_sound")
		Me.emitAudioFromObject.Add("shake_sound")
	End Sub

	' Token: 0x06001D29 RID: 7465 RVA: 0x0010B04A File Offset: 0x0010944A
	Private Sub SFXShakeStop()
		AudioManager.[Stop]("shake_sound")
	End Sub

	' Token: 0x06001D2A RID: 7466 RVA: 0x0010B056 File Offset: 0x00109456
	Private Sub SFXVOXAngry()
		If Not Me.VOXAngryActive Then
			AudioManager.Play("vox_angry")
			Me.emitAudioFromObject.Add("vox_angry")
			Me.VOXAngryActive = True
		End If
	End Sub

	' Token: 0x06001D2B RID: 7467 RVA: 0x0010B084 File Offset: 0x00109484
	Private Sub SFXVOXAngryAnim()
		AudioManager.Play("vox_angry")
		Me.emitAudioFromObject.Add("vox_angry")
	End Sub

	' Token: 0x06001D2C RID: 7468 RVA: 0x0010B0A0 File Offset: 0x001094A0
	Private Sub SFXVOXAnticipation()
		If Not Me.SFXAnticipationActive Then
			AudioManager.Play("vox_anticipation")
			Me.emitAudioFromObject.Add("vox_anticipation")
			Me.SFXAnticipationActive = True
		End If
	End Sub

	' Token: 0x06001D2D RID: 7469 RVA: 0x0010B0CE File Offset: 0x001094CE
	Private Sub SFXAllowAnticipation()
		Me.SFXAnticipationActive = False
	End Sub

	' Token: 0x04002603 RID: 9731
	<SerializeField()>
	Private projectileRoot As Transform

	' Token: 0x04002604 RID: 9732
	<SerializeField()>
	Private projectile As DicePalaceFlyingMemoryMusicNote

	' Token: 0x04002605 RID: 9733
	<SerializeField()>
	Private spiralProjectile As DicePalaceFlyingMemoryLevelSpiralProjectile

	' Token: 0x04002606 RID: 9734
	<SerializeField()>
	Private sprite As GameObject

	' Token: 0x04002607 RID: 9735
	<SerializeField()>
	Private hand As SpriteRenderer

	' Token: 0x04002608 RID: 9736
	Public guessedWrong As Boolean

	' Token: 0x04002609 RID: 9737
	Public state As DicePalaceFlyingMemoryLevelStuffedToy.State

	' Token: 0x0400260A RID: 9738
	Private damageDealer As DamageDealer

	' Token: 0x0400260B RID: 9739
	Private damageReceiver As DamageReceiver

	' Token: 0x0400260C RID: 9740
	Private velocity As Vector3

	' Token: 0x0400260D RID: 9741
	Private bounceCounter As Integer

	' Token: 0x0400260E RID: 9742
	Private shotIndex As Integer

	' Token: 0x0400260F RID: 9743
	Private speed As Single

	' Token: 0x04002610 RID: 9744
	Private newAngle As Single

	' Token: 0x04002611 RID: 9745
	Private currentAngle As Single

	' Token: 0x04002612 RID: 9746
	Private maxCount As Single

	' Token: 0x04002613 RID: 9747
	Private timer As Single

	' Token: 0x04002614 RID: 9748
	Private isMoving As Boolean

	' Token: 0x04002615 RID: 9749
	Private startedPunishment As Boolean

	' Token: 0x04002616 RID: 9750
	Public currentlyColliding As Boolean

	' Token: 0x04002617 RID: 9751
	Private shotPattern As String()

	' Token: 0x04002618 RID: 9752
	Private VOXAngryActive As Boolean

	' Token: 0x04002619 RID: 9753
	Private SFXAnticipationActive As Boolean

	' Token: 0x020005CD RID: 1485
	Public Enum State
		' Token: 0x0400261B RID: 9755
		Open
		' Token: 0x0400261C RID: 9756
		Closed
	End Enum
End Class
