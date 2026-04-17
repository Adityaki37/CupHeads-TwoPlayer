Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020005C1 RID: 1473
Public Class DicePalaceFlyingHorseLevelHorse
	Inherits LevelProperties.DicePalaceFlyingHorse.Entity

	' Token: 0x17000362 RID: 866
	' (get) Token: 0x06001CB0 RID: 7344 RVA: 0x001069ED File Offset: 0x00104DED
	' (set) Token: 0x06001CB1 RID: 7345 RVA: 0x001069F5 File Offset: 0x00104DF5
	Public Property miniHorseType As DicePalaceFlyingHorseLevelHorse.MiniHorseType

	' Token: 0x06001CB2 RID: 7346 RVA: 0x001069FE File Offset: 0x00104DFE
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
	End Sub

	' Token: 0x06001CB3 RID: 7347 RVA: 0x00106A34 File Offset: 0x00104E34
	Public Overrides Sub LevelInit(properties As LevelProperties.DicePalaceFlyingHorse)
		MyBase.LevelInit(properties)
		AddHandler Level.Current.OnLevelStartEvent, AddressOf Me.StartAttacks
		AddHandler Level.Current.OnWinEvent, AddressOf Me.Death
		Me.giftPosXMainIndex = Global.UnityEngine.Random.Range(0, properties.CurrentState.giftBombs.giftPositionStringX.Length)
		Me.giftPosYMainIndex = Global.UnityEngine.Random.Range(0, properties.CurrentState.giftBombs.giftPositionStringY.Length)
		Me.giftPosXIndex = Global.UnityEngine.Random.Range(0, properties.CurrentState.giftBombs.giftPositionStringX(Me.giftPosXMainIndex).Split(New Char() { ","c }).Length)
		Me.giftPosYIndex = Global.UnityEngine.Random.Range(0, properties.CurrentState.giftBombs.giftPositionStringY(Me.giftPosYMainIndex).Split(New Char() { ","c }).Length)
		Me.playerAimMaxCounter = properties.CurrentState.giftBombs.playerAimRange.RandomInt()
		MyBase.StartCoroutine(Me.intro_cr())
	End Sub

	' Token: 0x06001CB4 RID: 7348 RVA: 0x00106B42 File Offset: 0x00104F42
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		MyBase.properties.DealDamage(info.damage)
	End Sub

	' Token: 0x06001CB5 RID: 7349 RVA: 0x00106B55 File Offset: 0x00104F55
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06001CB6 RID: 7350 RVA: 0x00106B73 File Offset: 0x00104F73
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06001CB7 RID: 7351 RVA: 0x00106B8C File Offset: 0x00104F8C
	Private Iterator Function intro_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 2F)
		MyBase.animator.SetTrigger("Continue")
		AudioManager.Play("dice_palace_flying_horse_intro")
		Me.emitAudioFromObject.Add("dice_palace_flying_horse_intro")
		Yield Nothing
		Return
	End Function

	' Token: 0x06001CB8 RID: 7352 RVA: 0x00106BA7 File Offset: 0x00104FA7
	Private Sub StartAttacks()
		MyBase.StartCoroutine(Me.presents_cr())
		MyBase.StartCoroutine(Me.mini_horses_cr())
	End Sub

	' Token: 0x06001CB9 RID: 7353 RVA: 0x00106BC3 File Offset: 0x00104FC3
	Private Sub SpawnPresent()
		MyBase.StartCoroutine(Me.spawn_present_cr())
	End Sub

	' Token: 0x06001CBA RID: 7354 RVA: 0x00106BD4 File Offset: 0x00104FD4
	Private Iterator Function spawn_present_cr() As IEnumerator
		Dim p As LevelProperties.DicePalaceFlyingHorse.GiftBombs = MyBase.properties.CurrentState.giftBombs
		Dim positionX As Single = 0F
		Dim positionY As Single = 0F
		Dim endPos As Vector3 = Vector3.zero
		Dim player As AbstractPlayerController = PlayerManager.GetNext()
		Dim giftPositionXPattern As String() = p.giftPositionStringX(Me.giftPosXMainIndex).Split(New Char() { ","c })
		Dim giftPositionYPattern As String() = p.giftPositionStringY(Me.giftPosYMainIndex).Split(New Char() { ","c })
		If Me.playerAimCounter >= Me.playerAimMaxCounter Then
			endPos = player.transform.position
			player = PlayerManager.GetNext()
			Me.playerAimMaxCounter = p.playerAimRange.RandomInt()
			Me.playerAimCounter = 0
		Else
			Parser.FloatTryParse(giftPositionXPattern(Me.giftPosXIndex), positionX)
			Parser.FloatTryParse(giftPositionYPattern(Me.giftPosYIndex), positionY)
			endPos.x = -640F + positionX
			endPos.y = 360F - positionY
			Me.playerAimCounter += 1
		End If
		Dim present As DicePalaceFlyingHorseLevelPresent = Global.UnityEngine.[Object].Instantiate(Of DicePalaceFlyingHorseLevelPresent)(Me.presentPrefab)
		present.Init(Me.projectileRoot.position, endPos, MyBase.properties.CurrentState.giftBombs)
		If Me.giftPosXIndex < giftPositionXPattern(Me.giftPosXIndex).Length Then
			Me.giftPosXIndex += 1
		Else
			Me.giftPosXMainIndex = (Me.giftPosXMainIndex + 1) Mod p.giftPositionStringX.Length
			Me.giftPosXIndex = 0
		End If
		If Me.giftPosYIndex < giftPositionYPattern(Me.giftPosYIndex).Length Then
			Me.giftPosYIndex += 1
		Else
			Me.giftPosYMainIndex = (Me.giftPosYMainIndex + 1) Mod p.giftPositionStringY.Length
			Me.giftPosYIndex = 0
		End If
		Yield Nothing
		Return
	End Function

	' Token: 0x06001CBB RID: 7355 RVA: 0x00106BF0 File Offset: 0x00104FF0
	Private Iterator Function presents_cr() As IEnumerator
		While True
			Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.giftBombs.giftDelay)
			MyBase.animator.SetTrigger("OnAttack")
			Yield MyBase.animator.WaitForAnimationToStart(Me, "Attack", False)
			AudioManager.Play("dice_palace_flying_horse_attack")
			Me.emitAudioFromObject.Add("dice_palace_flying_horse_attack")
			Yield MyBase.animator.WaitForAnimationToEnd(Me, "Attack", False, True)
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06001CBC RID: 7356 RVA: 0x00106C0B File Offset: 0x0010500B
	Private Sub AttackVox()
		AudioManager.Play("dice_palace_horse_vox")
		Me.emitAudioFromObject.Add("dice_palace_horse_vox")
	End Sub

	' Token: 0x06001CBD RID: 7357 RVA: 0x00106C27 File Offset: 0x00105027
	Private Sub TrotSFX()
		AudioManager.Play("dice_horse_trot")
		Me.emitAudioFromObject.Add("dice_horse_trot")
	End Sub

	' Token: 0x06001CBE RID: 7358 RVA: 0x00106C43 File Offset: 0x00105043
	Private Sub DieSFX()
		AudioManager.Play("dice_horse_death")
		Me.emitAudioFromObject.Add("dice_horse_death")
	End Sub

	' Token: 0x06001CBF RID: 7359 RVA: 0x00106C60 File Offset: 0x00105060
	Private Sub SpawnMiniHorses(startPos As Vector3, prefab As DicePalaceFlyingHorseLevelMiniHorse, type As DicePalaceFlyingHorseLevelHorse.MiniHorseType, isPink As Boolean, threeProx As Single, lane As Integer)
		Dim miniHorses As LevelProperties.DicePalaceFlyingHorse.MiniHorses = MyBase.properties.CurrentState.miniHorses
		Dim [next] As AbstractPlayerController = PlayerManager.GetNext()
		Dim dicePalaceFlyingHorseLevelMiniHorse As DicePalaceFlyingHorseLevelMiniHorse = Global.UnityEngine.[Object].Instantiate(Of DicePalaceFlyingHorseLevelMiniHorse)(prefab)
		Dim vector As Vector3
		If lane = 0 Then
			vector = Me.topLineBackground.position
		ElseIf lane = 1 Then
			vector = Me.middleLineBackground.position
		Else
			vector = Me.bottomLineBackground.position
		End If
		dicePalaceFlyingHorseLevelMiniHorse.Init(startPos, miniHorses.HP, MyBase.properties.CurrentState.miniHorses, [next], type, isPink, threeProx, lane, vector)
	End Sub

	' Token: 0x06001CC0 RID: 7360 RVA: 0x00106CF0 File Offset: 0x001050F0
	Private Iterator Function mini_horses_cr() As IEnumerator
		Dim p As LevelProperties.DicePalaceFlyingHorse.MiniHorses = MyBase.properties.CurrentState.miniHorses
		Dim typePattern As String() = p.miniTypeString.GetRandom().Split(New Char() { ","c })
		Dim delayPattern As String() = p.delayString.GetRandom().Split(New Char() { ","c })
		Dim pinkPattern As String() = p.miniTwoPinkString.GetRandom().Split(New Char() { ","c })
		Dim proxPattern As String() = p.miniThreeProxString.GetRandom().Split(New Char() { ","c })
		Dim typeIndex As Integer = Global.UnityEngine.Random.Range(0, typePattern.Length)
		Dim delayIndex As Integer = Global.UnityEngine.Random.Range(0, delayPattern.Length)
		Dim pinkIndex As Integer = Global.UnityEngine.Random.Range(0, pinkPattern.Length)
		Dim proxIndex As Integer = Global.UnityEngine.Random.Range(0, proxPattern.Length)
		Dim type As Integer = 0
		Dim trackCounter As Integer = 0
		Dim pinkCounter As Integer = 0
		Dim maxPink As Integer = 0
		Dim threeProximity As Integer = 0
		Dim delay As Single = 0F
		Dim isPink As Boolean = False
		Dim lane As Integer = 0
		Dim position As Vector3 = MyBase.transform.position
		Dim prefab As DicePalaceFlyingHorseLevelMiniHorse = Nothing
		Dim [getType] As DicePalaceFlyingHorseLevelHorse.MiniHorseType = DicePalaceFlyingHorseLevelHorse.MiniHorseType.One
		position.x = CSng(Level.Current.Right) + 100F
		While True
			For i As Integer = typeIndex To typePattern.Length - 1
				Parser.IntTryParse(typePattern(i), type)
				Parser.FloatTryParse(delayPattern(delayIndex), delay)
				trackCounter += 1
				If trackCounter <= 1 Then
					position.y = Me.topLine.position.y
					lane = 0
				ElseIf trackCounter = 2 Then
					position.y = Me.middleLine.position.y
					lane = 1
				ElseIf trackCounter >= 3 Then
					position.y = Me.bottomLine.position.y
					trackCounter = 0
					lane = 2
				End If
				If type <> 1 Then
					If type <> 2 Then
						If type = 3 Then
							prefab = Me.miniHorse3Prefab
							[getType] = DicePalaceFlyingHorseLevelHorse.MiniHorseType.Three
							Parser.IntTryParse(proxPattern(proxIndex), threeProximity)
							proxIndex = (proxIndex + 1) Mod proxPattern.Length
						End If
					Else
						prefab = Me.miniHorse2Prefab
						[getType] = DicePalaceFlyingHorseLevelHorse.MiniHorseType.Two
						If pinkCounter = 0 Then
							isPink = False
							Parser.IntTryParse(pinkPattern(pinkIndex), maxPink)
							pinkIndex = pinkIndex Mod pinkPattern.Length
							pinkCounter += 1
						ElseIf pinkCounter >= maxPink Then
							isPink = True
							pinkCounter = 0
						Else
							isPink = False
							pinkCounter += 1
						End If
					End If
				Else
					prefab = Me.miniHorse1Prefab
					[getType] = DicePalaceFlyingHorseLevelHorse.MiniHorseType.One
				End If
				Me.SpawnMiniHorses(position, prefab, [getType], isPink, CSng(threeProximity), lane)
				Yield CupheadTime.WaitForSeconds(Me, delay)
				delayIndex = (delayIndex + 1) Mod delayPattern.Length
				i = i Mod typePattern.Length
				typeIndex = 0
			Next
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06001CC1 RID: 7361 RVA: 0x00106D0C File Offset: 0x0010510C
	Private Sub Death()
		Me.StopAllCoroutines()
		MyBase.GetComponent(Of Collider2D)().enabled = False
		MyBase.animator.SetTrigger("OnDeath")
		AudioManager.PlayLoop("dice_palace_flying_horse_death_loop")
		Me.emitAudioFromObject.Add("dice_palace_flying_horse_death_loop")
		Me.DieSFX()
	End Sub

	' Token: 0x0400259A RID: 9626
	<SerializeField()>
	Private bottomLine As Transform

	' Token: 0x0400259B RID: 9627
	<SerializeField()>
	Private middleLine As Transform

	' Token: 0x0400259C RID: 9628
	<SerializeField()>
	Private topLine As Transform

	' Token: 0x0400259D RID: 9629
	<SerializeField()>
	Private bottomLineBackground As Transform

	' Token: 0x0400259E RID: 9630
	<SerializeField()>
	Private middleLineBackground As Transform

	' Token: 0x0400259F RID: 9631
	<SerializeField()>
	Private topLineBackground As Transform

	' Token: 0x040025A0 RID: 9632
	<SerializeField()>
	Private projectileRoot As Transform

	' Token: 0x040025A1 RID: 9633
	<SerializeField()>
	Private miniHorse1Prefab As DicePalaceFlyingHorseLevelMiniHorse

	' Token: 0x040025A2 RID: 9634
	<SerializeField()>
	Private miniHorse2Prefab As DicePalaceFlyingHorseLevelMiniHorse

	' Token: 0x040025A3 RID: 9635
	<SerializeField()>
	Private miniHorse3Prefab As DicePalaceFlyingHorseLevelMiniHorse

	' Token: 0x040025A4 RID: 9636
	<SerializeField()>
	Private presentPrefab As DicePalaceFlyingHorseLevelPresent

	' Token: 0x040025A5 RID: 9637
	Private damageDealer As DamageDealer

	' Token: 0x040025A6 RID: 9638
	Private damageReceiver As DamageReceiver

	' Token: 0x040025A7 RID: 9639
	Private giftPosYMainIndex As Integer

	' Token: 0x040025A8 RID: 9640
	Private giftPosYIndex As Integer

	' Token: 0x040025A9 RID: 9641
	Private giftPosXMainIndex As Integer

	' Token: 0x040025AA RID: 9642
	Private giftPosXIndex As Integer

	' Token: 0x040025AB RID: 9643
	Private playerAimMaxCounter As Integer

	' Token: 0x040025AC RID: 9644
	Private playerAimCounter As Integer

	' Token: 0x020005C2 RID: 1474
	Public Enum MiniHorseType
		' Token: 0x040025AE RID: 9646
		One
		' Token: 0x040025AF RID: 9647
		Two
		' Token: 0x040025B0 RID: 9648
		Three
	End Enum
End Class
