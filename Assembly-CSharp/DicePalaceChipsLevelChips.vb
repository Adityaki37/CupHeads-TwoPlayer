Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020005AB RID: 1451
Public Class DicePalaceChipsLevelChips
	Inherits LevelProperties.DicePalaceChips.Entity

	' Token: 0x06001BF1 RID: 7153 RVA: 0x000FFEED File Offset: 0x000FE2ED
	Protected Overrides Sub Awake()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		MyBase.Awake()
	End Sub

	' Token: 0x06001BF2 RID: 7154 RVA: 0x000FFF18 File Offset: 0x000FE318
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		MyBase.properties.DealDamage(info.damage)
	End Sub

	' Token: 0x06001BF3 RID: 7155 RVA: 0x000FFF2B File Offset: 0x000FE32B
	Protected Overridable Function hitPauseCoefficient() As Single
		Return If((Not MyBase.GetComponent(Of DamageReceiver)().IsHitPaused), 1F, 0F)
	End Function

	' Token: 0x06001BF4 RID: 7156 RVA: 0x000FFF4C File Offset: 0x000FE34C
	Public Overrides Sub LevelInit(properties As LevelProperties.DicePalaceChips)
		AddHandler Level.Current.OnLevelStartEvent, AddressOf Me.StartAttacking
		AddHandler Level.Current.OnWinEvent, AddressOf Me.Death
		Me.leftScreenXPos = CSng(Level.Current.Left) + 100F
		Me.rightScreenXPos = CSng(Level.Current.Right) - 100F
		Me.rightScreenXPosStart = Me.chips(0).chipTransform.position.x
		For i As Integer = 0 To Me.chips.Length - 1
			Me.chips(i).startPosition = Me.chips(i).chipTransform.position
		Next
		Me.currentAttackCount = 0
		MyBase.LevelInit(properties)
	End Sub

	' Token: 0x06001BF5 RID: 7157 RVA: 0x00100019 File Offset: 0x000FE419
	Private Sub StartAttacking()
		MyBase.StartCoroutine(Me.chipAttack_cr())
	End Sub

	' Token: 0x06001BF6 RID: 7158 RVA: 0x00100028 File Offset: 0x000FE428
	Private Iterator Function chipAttack_cr() As IEnumerator
		Dim p As LevelProperties.DicePalaceChips.Chips = MyBase.properties.CurrentState.chips
		Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.chips.initialAttackDelay)
		Dim mainStringIndex As Integer = Global.UnityEngine.Random.Range(0, p.chipAttackString.Length)
		Dim dir As Integer = -1
		Dim attackIndex As Integer = Global.UnityEngine.Random.Range(0, Me.maxAttacksPerCycle)
		While True
			Dim currentAttackChips As String() = p.chipAttackString(mainStringIndex).Split(New Char() { ","c })
			Me.maxAttacksPerCycle = currentAttackChips.Length
			For j As Integer = 0 To Me.chips.Length - 1
				Dim num As Single = If((Not Rand.Bool()), (-5F), 5F)
				Me.chips(j).rotationSpeed = num
			Next
			MyBase.animator.SetBool("IsSpread", True)
			Yield MyBase.animator.WaitForAnimationToStart(Me, "Spread_Open", False)
			Dim startPos As Single = Me.chips(Me.chips.Length - 1).chipTransform.position.y
			Dim frameTime As Single = 0F
			Dim time As Single = 1.5F
			Dim t As Single = 0F
			Dim counter As Integer = 0
			While t < time
				Dim val As Single = EaseUtils.Ease(EaseUtils.EaseType.linear, 0F, 1F, t / time)
				frameTime += CupheadTime.Delta * MyBase.animator.speed
				t += CupheadTime.Delta * MyBase.animator.speed
				If frameTime > 0.041666668F Then
					frameTime -= 0.041666668F
					For k As Integer = Me.chips.Length - 1 To 0 Step -1
						Dim num2 As Single = If((k <> 0), (Me.chips(k).chipTransform.GetComponent(Of Renderer)().bounds.size.y / 1.7F), (Me.chips(k).chipTransform.GetComponent(Of Renderer)().bounds.size.y / 5.5F))
						Dim num3 As Single = startPos + CSng(counter) * num2
						Dim position As Vector3 = Me.chips(k).chipTransform.position
						position.y = Mathf.Lerp(position.y, num3, val)
						Me.chips(k).chipTransform.position = position
						counter = (counter + 1) Mod Me.chips.Length
						Dim num4 As Single = Mathf.Sin(t / 0.7F)
						Me.chips(k).chipTransform.localRotation = Quaternion.Euler(New Vector3(0F, 0F, num4 * Me.chips(k).rotationSpeed))
					Next
				End If
				Yield Nothing
			End While
			Me.currentlyFloating = True
			For Each chipPieces As DicePalaceChipsLevelChips.ChipPieces In Me.chips
				MyBase.StartCoroutine(Me.rotate_chips_cr(chipPieces.chipTransform, chipPieces.rotationSpeed, 0.7F, t))
			Next
			Yield Nothing
			For i As Integer = attackIndex To currentAttackChips.Length - 1
				Dim currentAttackChipsMultiple As String() = currentAttackChips(i).Split(New Char() { "-"c })
				Me.SFX_DicePalaceChipsShoot()
				For Each chip As String In currentAttackChipsMultiple
					If chip(0) = "D"c Then
						Yield CupheadTime.WaitForSeconds(Me, Parser.FloatParse(chip.Substring(1)))
					ElseIf Me.currentAttackCount < Me.maxAttacksPerCycle - 1 Then
						MyBase.StartCoroutine(Me.moveChip_cr(MyBase.transform.GetChild(Parser.IntParse(chip) - 1).transform, dir, False))
					Else
						MyBase.StartCoroutine(Me.moveChip_cr(MyBase.transform.GetChild(Parser.IntParse(chip) - 1).transform, dir, True))
					End If
				Next
				Me.currentAttackCount += 1
				If Me.currentAttackCount >= Me.maxAttacksPerCycle Then
					Me.currentAttackCount = 0
					attackIndex = Global.UnityEngine.Random.Range(0, Me.maxAttacksPerCycle)
				Else
					Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.chips.chipAttackDelay)
				End If
				attackIndex = 0
			Next
			While Me.chipInFlight
				Yield Nothing
			End While
			Yield CupheadTime.WaitForSeconds(Me, 1F)
			time = 0.3F
			t = 0F
			counter = 0
			MyBase.animator.SetBool("IsSpread", False)
			Yield MyBase.animator.WaitForAnimationToStart(Me, "Spread_Close", False)
			Yield CupheadTime.WaitForSeconds(Me, 0.4F)
			Me.currentlyFloating = False
			While t < time
				Dim val2 As Single = EaseUtils.Ease(EaseUtils.EaseType.linear, 0F, 1F, t / time)
				For n As Integer = Me.chips.Length - 1 To 0 Step -1
					Dim num5 As Single = 0F
					If n <> Me.chips.Length - 1 Then
						num5 = 10F
					End If
					Dim num6 As Single = Me.chips(n).startPosition.y - CSng(counter) * num5
					Dim position2 As Vector3 = Me.chips(n).chipTransform.position
					position2.y = Mathf.Lerp(position2.y, num6, val2)
					Me.chips(n).chipTransform.position = position2
					counter = (counter + 1) Mod Me.chips.Length
				Next
				t += CupheadTime.Delta
				Yield Nothing
			End While
			dir *= -1
			Yield Nothing
			mainStringIndex = (mainStringIndex + 1) Mod p.chipAttackString.Length
			Dim tt As Single = 0F
			While tt < MyBase.properties.CurrentState.chips.attackCycleDelay
				tt += CupheadTime.Delta * MyBase.animator.speed
				Yield Nothing
			End While
		End While
		Return
	End Function

	' Token: 0x06001BF7 RID: 7159 RVA: 0x00100044 File Offset: 0x000FE444
	Private Sub FlipSprite()
		Me.mainLayer.transform.SetScale(New Single?(-Me.mainLayer.transform.localScale.x), New Single?(1F), New Single?(1F))
		For Each chipPieces As DicePalaceChipsLevelChips.ChipPieces In Me.chips
			Dim position As Vector3 = chipPieces.chipTransform.position
			position.y = chipPieces.startPosition.y
			chipPieces.chipTransform.position = position
		Next
	End Sub

	' Token: 0x06001BF8 RID: 7160 RVA: 0x001000E0 File Offset: 0x000FE4E0
	Private Iterator Function moveChip_cr(chip As Transform, dir As Integer, lastChipOfCycle As Boolean) As IEnumerator
		Me.chipInFlight = lastChipOfCycle
		Dim start As Single = If((dir <> 1), Me.rightScreenXPos, Me.leftScreenXPos)
		Dim [end] As Single = If((dir <> 1), Me.leftScreenXPos, Me.rightScreenXPos)
		Dim pos As Vector3 = chip.position
		If Me.firstTimeMoving Then
			start = Me.rightScreenXPosStart
		End If
		Dim pct As Single = 0F
		While pct < 1F
			pos.x = start + ([end] - start) * pct
			chip.position = pos
			pct += CupheadTime.Delta * MyBase.properties.CurrentState.chips.chipSpeedMultiplier * Me.hitPauseCoefficient() * MyBase.animator.speed
			Yield Nothing
		End While
		pos.x = [end]
		chip.position = pos
		Me.chipInFlight = False
		If lastChipOfCycle Then
			Me.firstTimeMoving = False
		End If
		Return
	End Function

	' Token: 0x06001BF9 RID: 7161 RVA: 0x00100110 File Offset: 0x000FE510
	Private Iterator Function rotate_chips_cr(chip As Transform, speed As Single, time As Single, t As Single) As IEnumerator
		While Me.currentlyFloating
			t += CupheadTime.Delta
			Dim phase As Single = Mathf.Sin(t / time)
			chip.localRotation = Quaternion.Euler(New Vector3(0F, 0F, phase * speed))
			Yield Nothing
		End While
		chip.localRotation = Quaternion.Euler(New Vector3(0F, 0F, 0F))
		Yield Nothing
		Return
	End Function

	' Token: 0x06001BFA RID: 7162 RVA: 0x00100148 File Offset: 0x000FE548
	Private Sub Death()
		Me.StopAllCoroutines()
		MyBase.animator.SetBool("IsSpread", True)
		MyBase.animator.SetTrigger("OnDeath")
		Me.chips(0).chipTransform.SetScale(New Single?(Me.mainLayer.transform.localScale.x), New Single?(1F), New Single?(1F))
		MyBase.StartCoroutine(Me.head_fall_cr())
		For i As Integer = 1 To Me.chips.Length - 1
			MyBase.StartCoroutine(Me.chips_die(Me.chips(i).chipTransform))
		Next
	End Sub

	' Token: 0x06001BFB RID: 7163 RVA: 0x00100200 File Offset: 0x000FE600
	Private Iterator Function chips_die(chip As Transform) As IEnumerator
		Dim speed As Single = 2500F
		Dim angle As Single = CSng(Global.UnityEngine.Random.Range(0, 360))
		Dim dir As Vector3 = MathUtils.AngleToDirection(-angle)
		chip.GetComponent(Of Collider2D)().enabled = False
		While True
			chip.position += dir * speed * CupheadTime.FixedDelta
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06001BFC RID: 7164 RVA: 0x0010021B File Offset: 0x000FE61B
	Private Sub SpawnHat()
		Me.hat.SetActive(True)
		MyBase.StartCoroutine(Me.hat_fall_cr())
	End Sub

	' Token: 0x06001BFD RID: 7165 RVA: 0x00100238 File Offset: 0x000FE638
	Private Iterator Function head_fall_cr() As IEnumerator
		Dim velocity As Single = 800F
		Dim posY As Single = CSng(Level.Current.Ground) + Me.chips(0).chipTransform.GetComponent(Of Collider2D)().bounds.size.y / 1.2F
		While Me.chips(0).chipTransform.position.y > posY
			Me.chips(0).chipTransform.position += Vector3.down * velocity * CupheadTime.Delta
			Yield Nothing
		End While
		MyBase.animator.SetTrigger("Continue")
		Yield Nothing
		Return
	End Function

	' Token: 0x06001BFE RID: 7166 RVA: 0x00100254 File Offset: 0x000FE654
	Private Iterator Function hat_fall_cr() As IEnumerator
		Dim velocity As Single = 30F
		While Me.hat.transform.position.y > -250F
			Me.hat.transform.position += Vector3.down * velocity * CupheadTime.Delta
			Yield Nothing
		End While
		Me.hat.GetComponent(Of Animator)().SetTrigger("Continue")
		Yield Nothing
		Return
	End Function

	' Token: 0x06001BFF RID: 7167 RVA: 0x0010026F File Offset: 0x000FE66F
	Private Sub SFX_DicePalaceChipsIntro()
		AudioManager.Play("chips_intro")
		Me.emitAudioFromObject.Add("chips_intro")
		AudioManager.Play("vox_intro")
		Me.emitAudioFromObject.Add("vox_intro")
	End Sub

	' Token: 0x06001C00 RID: 7168 RVA: 0x001002A8 File Offset: 0x000FE6A8
	Private Sub SFX_DicePalaceChipsDeath()
		If Not Me.DeathSoundPlaying Then
			AudioManager.PlayLoop("chips_death")
			Me.emitAudioFromObject.Add("chips_death")
			AudioManager.Play("vox_die")
			Me.emitAudioFromObject.Add("vox_die")
			Me.DeathSoundPlaying = True
		End If
	End Sub

	' Token: 0x06001C01 RID: 7169 RVA: 0x001002FC File Offset: 0x000FE6FC
	Private Sub SFX_DicePalaceChipsExpand()
		If Not Me.ExpandSoundPlaying Then
			AudioManager.Play("chips_expand")
			Me.emitAudioFromObject.Add("chips_expand")
			AudioManager.Play("vox_idle")
			Me.emitAudioFromObject.Add("vox_idle")
			Me.ExpandSoundPlaying = True
		End If
	End Sub

	' Token: 0x06001C02 RID: 7170 RVA: 0x0010034F File Offset: 0x000FE74F
	Private Sub SFX_DicePalaceChipsRetract()
		AudioManager.Play("chips_retract")
		AudioManager.Play("vox_idle")
		Me.ExpandSoundPlaying = False
	End Sub

	' Token: 0x06001C03 RID: 7171 RVA: 0x0010036C File Offset: 0x000FE76C
	Private Sub SFX_DicePalaceChipsShoot()
		AudioManager.Play("chips_shoot")
		Me.emitAudioFromObject.Add("chips_shoot")
	End Sub

	' Token: 0x06001C04 RID: 7172 RVA: 0x00100388 File Offset: 0x000FE788
	Private Sub SFX_DicePalaceChipsSpinLoop()
		If Not Me.SpinSoundPlaying Then
			AudioManager.PlayLoop("chips_spin_loop")
			Me.emitAudioFromObject.Add("chips_spin_loop")
			Me.SpinSoundPlaying = True
		End If
	End Sub

	' Token: 0x06001C05 RID: 7173 RVA: 0x001003B6 File Offset: 0x000FE7B6
	Private Sub SFX_DicePalaceChipsSpinLoopStop()
		AudioManager.[Stop]("chips_spin_loop")
		Me.SpinSoundPlaying = False
	End Sub

	' Token: 0x06001C06 RID: 7174 RVA: 0x001003C9 File Offset: 0x000FE7C9
	Private Sub SFX_DicePalaceChipsBounce()
		AudioManager.Play("chips_bounce")
	End Sub

	' Token: 0x04002505 RID: 9477
	Private Const FRAME_TIME As Single = 0.041666668F

	' Token: 0x04002506 RID: 9478
	<SerializeField()>
	Private chips As DicePalaceChipsLevelChips.ChipPieces()

	' Token: 0x04002507 RID: 9479
	<SerializeField()>
	Private mainLayer As Transform

	' Token: 0x04002508 RID: 9480
	<SerializeField()>
	Private hat As GameObject

	' Token: 0x04002509 RID: 9481
	Private leftScreenXPos As Single

	' Token: 0x0400250A RID: 9482
	Private rightScreenXPos As Single

	' Token: 0x0400250B RID: 9483
	Private rightScreenXPosStart As Single

	' Token: 0x0400250C RID: 9484
	Private currentAttackCount As Integer

	' Token: 0x0400250D RID: 9485
	Private maxAttacksPerCycle As Integer

	' Token: 0x0400250E RID: 9486
	Private chipInFlight As Boolean

	' Token: 0x0400250F RID: 9487
	Private currentlyFloating As Boolean

	' Token: 0x04002510 RID: 9488
	Private firstTimeMoving As Boolean = True

	' Token: 0x04002511 RID: 9489
	Private damageReceiver As DamageReceiver

	' Token: 0x04002512 RID: 9490
	Private DeathSoundPlaying As Boolean

	' Token: 0x04002513 RID: 9491
	Private SpinSoundPlaying As Boolean

	' Token: 0x04002514 RID: 9492
	Private ExpandSoundPlaying As Boolean

	' Token: 0x020005AC RID: 1452
	<Serializable()>
	Public Class ChipPieces
		' Token: 0x04002515 RID: 9493
		Public chipTransform As Transform

		' Token: 0x04002516 RID: 9494
		Public startPosition As Vector3

		' Token: 0x04002517 RID: 9495
		Public rotationSpeed As Single
	End Class
End Class
