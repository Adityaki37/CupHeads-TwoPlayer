Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020005F1 RID: 1521
Public Class DragonLevelLeftSideDragon
	Inherits LevelProperties.Dragon.Entity

	' Token: 0x17000375 RID: 885
	' (get) Token: 0x06001E40 RID: 7744 RVA: 0x001168B8 File Offset: 0x00114CB8
	' (set) Token: 0x06001E41 RID: 7745 RVA: 0x001168C0 File Offset: 0x00114CC0
	Public Property state As DragonLevelLeftSideDragon.State

	' Token: 0x06001E42 RID: 7746 RVA: 0x001168CC File Offset: 0x00114CCC
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.state = DragonLevelLeftSideDragon.State.UnSpawned
		Me.headPicked = DragonLevelLeftSideDragon.HeadPicked.None
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = Me.damageBox.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		Me.middleHead.GetComponent(Of Collider2D)().enabled = False
		For Each collider2D As Collider2D In MyBase.GetComponents(Of Collider2D)()
			collider2D.enabled = False
		Next
		Me.damageReceiver.enabled = False
		Me.fire.SetColliderEnabled(False)
		Me.xPos = MyBase.transform.position.x
		Dim position As Vector3 = MyBase.transform.position
		position.x = -10000F
		MyBase.transform.position = position
	End Sub

	' Token: 0x06001E43 RID: 7747 RVA: 0x001169B0 File Offset: 0x00114DB0
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		If Me.dead Then
			Return
		End If
		MyBase.properties.DealDamage(info.damage)
		If MyBase.properties.CurrentHealth <= 0F AndAlso Me.state <> DragonLevelLeftSideDragon.State.Dead Then
			Me.state = DragonLevelLeftSideDragon.State.Dead
			Me.StartDeath()
		End If
	End Sub

	' Token: 0x06001E44 RID: 7748 RVA: 0x00116A08 File Offset: 0x00114E08
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06001E45 RID: 7749 RVA: 0x00116A20 File Offset: 0x00114E20
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If Me.damageDealer IsNot Nothing AndAlso phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06001E46 RID: 7750 RVA: 0x00116A4C File Offset: 0x00114E4C
	Public Overrides Sub LevelInit(properties As LevelProperties.Dragon)
		MyBase.LevelInit(properties)
		Me.potionTypeMainIndex = Global.UnityEngine.Random.Range(0, properties.CurrentState.potions.potionTypeString.Length)
		Me.potionTypeIndex = Global.UnityEngine.Random.Range(0, properties.CurrentState.potions.potionTypeString(Me.potionTypeMainIndex).Split(New Char() { ","c }).Length)
		Me.shotPositionMainIndex = Global.UnityEngine.Random.Range(0, properties.CurrentState.potions.shotPositionString.Length)
		Me.shotPositionIndex = Global.UnityEngine.Random.Range(0, properties.CurrentState.potions.shotPositionString(Me.shotPositionMainIndex).Split(New Char() { ","c }).Length)
		Me.attackCountMainIndex = Global.UnityEngine.Random.Range(0, properties.CurrentState.potions.attackCount.Length)
		Me.attackCountIndex = Global.UnityEngine.Random.Range(0, properties.CurrentState.potions.attackCount(Me.attackCountMainIndex).Split(New Char() { ","c }).Length)
		Me.AttackFrames = 36 - (properties.CurrentState.blowtorch.warningDurationOne + properties.CurrentState.blowtorch.warningDurationTwo)
	End Sub

	' Token: 0x06001E47 RID: 7751 RVA: 0x00116B83 File Offset: 0x00114F83
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.fireMarcherPrefabs = Nothing
		Me.fireMarcherLeaderPrefab = Nothing
		Me.bothPotionPrefab = Nothing
		Me.horizontalPotionPrefab = Nothing
		Me.verticalPotionPrefab = Nothing
	End Sub

	' Token: 0x06001E48 RID: 7752 RVA: 0x00116BAE File Offset: 0x00114FAE
	Public Sub StartIntro()
		MyBase.StartCoroutine(Me.intro_cr())
	End Sub

	' Token: 0x06001E49 RID: 7753 RVA: 0x00116BC0 File Offset: 0x00114FC0
	Private Iterator Function intro_cr() As IEnumerator
		AudioManager.Play("level_dragon_left_dragon_intro")
		Me.emitAudioFromObject.Add("level_dragon_left_dragon_intro")
		Yield MyBase.TweenPositionX(Me.xPos, -350F, 1.3F, EaseUtils.EaseType.easeInSine)
		MyBase.animator.SetTrigger("Continue")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Intro", 0, False, True)
		For Each collider2D As Collider2D In MyBase.GetComponents(Of Collider2D)()
			collider2D.enabled = True
		Next
		Me.damageReceiver.enabled = True
		MyBase.StartCoroutine(Me.fire_cr())
		Me.StartFireMarchers()
		Return
	End Function

	' Token: 0x06001E4A RID: 7754 RVA: 0x00116BDB File Offset: 0x00114FDB
	Private Sub TongueIntroSFX()
		AudioManager.Play("level_dragon_left_dragon_tongue_intro")
		Me.emitAudioFromObject.Add("level_dragon_left_dragon_tongue_intro")
	End Sub

	' Token: 0x06001E4B RID: 7755 RVA: 0x00116BF8 File Offset: 0x00114FF8
	Private Iterator Function fire_cr() As IEnumerator
		Me.state = DragonLevelLeftSideDragon.State.Fire
		Dim patternIndex As Integer = 0
		Me.fire.transform.parent = Nothing
		While True
			Dim pattern As String() = MyBase.properties.CurrentState.fireAndSmoke.PatternString.Split(New Char() { ","c })
			Dim text As String = pattern(patternIndex Mod pattern.Length)
			If text IsNot Nothing Then
				If Not(text = "F") Then
					If text = "S" Then
						AudioManager.Play("level_dragon_left_dragon_smoke_start")
						Me.emitAudioFromObject.Add("level_dragon_left_dragon_smoke_start")
						MyBase.animator.SetTrigger("StartSmoke")
						Yield MyBase.animator.WaitForAnimationToStart(Me, "Smoke_Loop", 2, False)
						AudioManager.Play("level_dragon_left_dragon_smoke_loop")
						Me.emitAudioFromObject.Add("level_dragon_left_dragon_smoke_loop")
						Yield CupheadTime.WaitForSeconds(Me, Global.UnityEngine.Random.Range(0.66F, 1.32F))
						AudioManager.Play("level_dragon_left_dragon_smoke_end")
						Me.emitAudioFromObject.Add("level_dragon_left_dragon_smoke_end")
					End If
				Else
					AudioManager.Play("level_dragon_left_dragon_fire_start")
					Me.emitAudioFromObject.Add("level_dragon_left_dragon_fire_start")
					MyBase.animator.SetTrigger("StartFire")
					Yield MyBase.animator.WaitForAnimationToStart(Me, "Fire_Loop", 2, False)
					AudioManager.Play("level_dragon_left_dragon_fire_loop")
					Me.emitAudioFromObject.Add("level_dragon_left_dragon_fire_loop")
					Me.fire.SetColliderEnabled(True)
					Yield CupheadTime.WaitForSeconds(Me, Global.UnityEngine.Random.Range(0.25F, 1.75F))
					AudioManager.Play("level_dragon_left_dragon_fire_end")
					Me.emitAudioFromObject.Add("level_dragon_left_dragon_fire_end")
					Me.fire.SetColliderEnabled(False)
				End If
			End If
			MyBase.animator.SetTrigger("Continue")
			Yield MyBase.animator.WaitForAnimationToStart(Me, "Idle", 2, False)
			Yield CupheadTime.WaitForSeconds(Me, 0.25F)
			patternIndex += 1
		End While
		Return
	End Function

	' Token: 0x06001E4C RID: 7756 RVA: 0x00116C13 File Offset: 0x00115013
	Private Sub StartFireMarchers()
		MyBase.StartCoroutine(Me.spawnFireMarchers_cr())
		MyBase.StartCoroutine(Me.fireMarchersJump_cr())
	End Sub

	' Token: 0x06001E4D RID: 7757 RVA: 0x00116C30 File Offset: 0x00115030
	Private Iterator Function spawnFireMarchers_cr() As IEnumerator
		Me.fireMarcherLeaderPrefab.Create(Me.fireMarcherRoot, MyBase.properties.CurrentState.fireMarchers)
		While True
			Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.fireMarchers.spawnDelay)
			Me.lastFireMarcher = Me.fireMarcherPrefabs.RandomChoice().Create(Me.fireMarcherRoot, MyBase.properties.CurrentState.fireMarchers)
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06001E4E RID: 7758 RVA: 0x00116C4C File Offset: 0x0011504C
	Private Iterator Function fireMarchersJump_cr() As IEnumerator
		While True
			Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.fireMarchers.jumpDelay.RandomFloat())
			Dim fireMarchers As DragonLevelFireMarcher() = Global.UnityEngine.[Object].FindObjectsOfType(Of DragonLevelFireMarcher)()
			fireMarchers.Shuffle()
			For Each dragonLevelFireMarcher As DragonLevelFireMarcher In fireMarchers
				If dragonLevelFireMarcher.CanJump() Then
					dragonLevelFireMarcher.StartJump(PlayerManager.GetNext())
					Exit For
				End If
			Next
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06001E4F RID: 7759 RVA: 0x00116C67 File Offset: 0x00115067
	Public Sub StartThreeHeads()
		Me.StopAllCoroutines()
		Me.state = DragonLevelLeftSideDragon.State.Transition
		Me.fire.gameObject.SetActive(False)
		MyBase.StartCoroutine(Me.three_heads_cr())
	End Sub

	' Token: 0x06001E50 RID: 7760 RVA: 0x00116C94 File Offset: 0x00115094
	Private Iterator Function three_heads_cr() As IEnumerator
		MyBase.animator.SetTrigger("StartThree")
		MyBase.GetComponent(Of LevelBossDeathExploder)().StartExplosion()
		While Me.lastFireMarcher IsNot Nothing
			Yield Nothing
		End While
		MyBase.animator.SetTrigger("FoldTongue")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Intro_Reverse", 1, False, True)
		MyBase.animator.SetTrigger("ToThree")
		Yield MyBase.animator.WaitForAnimationToStart(Me, "Three_Intro", False)
		MyBase.GetComponent(Of LevelBossDeathExploder)().StopExplosions()
		AudioManager.Play("level_dragon_three_dragon_intro")
		Me.emitAudioFromObject.Add("level_dragon_three_dragon_intro")
		Me.state = DragonLevelLeftSideDragon.State.ThreeHeads
		For Each dragonLevelBackgroundChange As DragonLevelBackgroundChange In Me.backgrounds
			dragonLevelBackgroundChange.StartChange()
		Next
		For j As Integer = 0 To Me.backgroundsToHide.Length - 1
			Me.backgroundsToHide(j).SetActive(False)
		Next
		Me.spire.StartChange()
		Me.rain.StartRain()
		MyBase.StartCoroutine(Me.potion_cr())
		MyBase.StartCoroutine(Me.blow_torch_cr())
		Yield Nothing
		Return
	End Function

	' Token: 0x06001E51 RID: 7761 RVA: 0x00116CAF File Offset: 0x001150AF
	Private Sub ActivateHeadLayers()
		MyBase.animator.SetTrigger("StartHeads")
	End Sub

	' Token: 0x06001E52 RID: 7762 RVA: 0x00116CC4 File Offset: 0x001150C4
	Private Sub SpawnPotion(type As Integer)
		Me.spittle.gameObject.SetActive(False)
		Dim vector As Vector3 = Vector3.zero
		Dim dragonLevelPotion As DragonLevelPotion = Me.horizontalPotionPrefab
		Dim potions As LevelProperties.Dragon.Potions = MyBase.properties.CurrentState.potions
		Dim array As String() = potions.potionTypeString(Me.potionTypeMainIndex).Split(New Char() { ","c })
		If array(Me.potionTypeIndex)(0) = "H"c Then
			dragonLevelPotion = Me.horizontalPotionPrefab
		ElseIf array(Me.potionTypeIndex)(0) = "V"c Then
			dragonLevelPotion = Me.verticalPotionPrefab
		ElseIf array(Me.potionTypeIndex)(0) = "X"c Then
			dragonLevelPotion = Me.bothPotionPrefab
		End If
		If type = 1 OrElse type = 3 Then
			vector = Me.topHead.position
		ElseIf type = 2 OrElse type = 4 Then
			vector = Me.bottomHead.position
		End If
		Dim [next] As AbstractPlayerController = PlayerManager.GetNext()
		Dim vector2 As Vector3 = Vector3.zero
		If [next].transform.position.x > MyBase.transform.position.x Then
			vector2 = [next].transform.position - vector
		Else
			vector2 = MathUtils.AngleToDirection(90F)
		End If
		Dim dragonLevelPotion2 As DragonLevelPotion = Global.UnityEngine.[Object].Instantiate(Of DragonLevelPotion)(dragonLevelPotion)
		dragonLevelPotion2.Init(vector, MyBase.properties.CurrentState.potions.potionHP, MathUtils.DirectionToAngle(vector2), MyBase.properties.CurrentState.potions)
		If Me.potionTypeIndex < array.Length - 1 Then
			Me.potionTypeIndex += 1
		Else
			Me.potionTypeMainIndex = (Me.potionTypeMainIndex + 1) Mod potions.potionTypeString.Length
			Me.potionTypeIndex = 0
		End If
		Me.spittle.gameObject.SetActive(True)
		Me.spittle.position = vector
	End Sub

	' Token: 0x06001E53 RID: 7763 RVA: 0x00116EC0 File Offset: 0x001152C0
	Private Iterator Function potion_cr() As IEnumerator
		Dim p As LevelProperties.Dragon.Potions = MyBase.properties.CurrentState.potions
		Dim attackCountString As String() = p.attackCount(Me.attackCountMainIndex).Split(New Char() { ","c })
		Dim shotPositionString As String() = p.shotPositionString(Me.shotPositionMainIndex).Split(New Char() { ","c })
		Dim attackCount As Integer = 0
		While True
			attackCountString = p.attackCount(Me.attackCountMainIndex).Split(New Char() { ","c })
			Parser.IntTryParse(attackCountString(Me.attackCountIndex), attackCount)
			For i As Integer = 0 To attackCount - 1
				shotPositionString = p.shotPositionString(Me.shotPositionMainIndex).Split(New Char() { ","c })
				Dim pickedDragon As String() = shotPositionString(Me.shotPositionIndex).Split(New Char() { ":"c })
				For Each picked As String In pickedDragon
					While Me.torch
						Yield Nothing
					End While
					If shotPositionString(Me.shotPositionIndex)(0) = "T"c Then
						Me.animationString = "High_Attack"
					ElseIf shotPositionString(Me.shotPositionIndex)(0) = "B"c Then
						Me.animationString = "Low_Attack"
					End If
					If picked = "A" Then
						Me.layer = 5
					ElseIf picked = "C" Then
						Me.layer = 6
					End If
				Next
				If Me.layer = 5 AndAlso Me.animationString = "High_Attack" Then
					Me.headPicked = DragonLevelLeftSideDragon.HeadPicked.CTop
				ElseIf Me.layer = 6 AndAlso Me.animationString = "High_Attack" Then
					Me.headPicked = DragonLevelLeftSideDragon.HeadPicked.ATop
				ElseIf Me.layer = 5 AndAlso Me.animationString = "Low_Attack" Then
					Me.headPicked = DragonLevelLeftSideDragon.HeadPicked.CBottom
				ElseIf Me.layer = 6 AndAlso Me.animationString = "Low_Attack" Then
					Me.headPicked = DragonLevelLeftSideDragon.HeadPicked.ABottom
				End If
				Yield MyBase.animator.WaitForAnimationToEnd(Me, Me.animationString, Me.layer, False, True)
				If Me.shotPositionIndex < shotPositionString.Length - 1 Then
					Me.shotPositionIndex += 1
				Else
					Me.shotPositionMainIndex = (Me.shotPositionMainIndex + 1) Mod p.shotPositionString.Length
					Me.shotPositionIndex = 0
				End If
				Yield CupheadTime.WaitForSeconds(Me, p.repeatDelay)
			Next
			If Me.attackCountIndex < attackCountString.Length - 1 Then
				Me.attackCountIndex += 1
			Else
				Me.attackCountMainIndex = (Me.attackCountMainIndex + 1) Mod p.attackCount.Length
				Me.attackCountIndex = 0
			End If
			Yield CupheadTime.WaitForSeconds(Me, p.attackMainDelay)
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06001E54 RID: 7764 RVA: 0x00116EDC File Offset: 0x001152DC
	Private Sub PotionAttack(picked As DragonLevelLeftSideDragon.HeadPicked)
		If picked = Me.headPicked Then
			AudioManager.Play("level_dragon_three_dragon_head_attack")
			Me.emitAudioFromObject.Add("level_dragon_three_dragon_head_attack")
			MyBase.animator.Play(Me.animationString, Me.layer)
			Me.headPicked = DragonLevelLeftSideDragon.HeadPicked.None
		End If
	End Sub

	' Token: 0x06001E55 RID: 7765 RVA: 0x00116F30 File Offset: 0x00115330
	Private Iterator Function blow_torch_cr() As IEnumerator
		Dim p As LevelProperties.Dragon.Blowtorch = MyBase.properties.CurrentState.blowtorch
		Dim delayPattern As String() = p.attackDelayString.GetRandom().Split(New Char() { ","c })
		Me.middleHead.SetScale(New Single?(Me.middleHead.transform.localScale.x), New Single?(p.fireSize), New Single?(1F))
		Dim delay As Single = 0F
		Dim delayCountIndex As Integer = Global.UnityEngine.Random.Range(0, delayPattern.Length)
		While True
			Parser.FloatTryParse(delayPattern(delayCountIndex), delay)
			Yield CupheadTime.WaitForSeconds(Me, delay)
			delayCountIndex = (delayCountIndex + 1) Mod delayPattern.Length
			Me.torch = True
			Yield MyBase.animator.WaitForAnimationToEnd(Me, "Dragon_Head_Idle_Loop", 3, False, True)
			AudioManager.Play("level_dragon_torch_warning_1_start")
			Me.emitAudioFromObject.Add("level_dragon_torch_warning_1_start")
			MyBase.animator.Play("Torch_Warning_One", 4)
			Yield MyBase.animator.WaitForAnimationToEnd(Me, "Torch_End", 4, False, True)
			Me.torch = False
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06001E56 RID: 7766 RVA: 0x00116F4C File Offset: 0x0011534C
	Private Sub ActivateTorch()
		Me.middleHead.GetComponent(Of Animator)().SetBool("TorchOn", True)
		AudioManager.Play("level_dragon_three_dragon_head_b_torch_attack_burst")
		Me.emitAudioFromObject.Add("level_dragon_three_dragon_head_b_torch_attack_burst")
		AudioManager.PlayLoop("level_dragon_three_dragon_head_b_torch_attack_loop")
		Me.emitAudioFromObject.Add("level_dragon_three_dragon_head_b_torch_attack_loop")
	End Sub

	' Token: 0x06001E57 RID: 7767 RVA: 0x00116FA3 File Offset: 0x001153A3
	Private Sub DeactivateTorch()
		Me.middleHead.GetComponent(Of Animator)().SetBool("TorchOn", False)
		AudioManager.[Stop]("level_dragon_three_dragon_head_b_torch_attack_loop")
	End Sub

	' Token: 0x06001E58 RID: 7768 RVA: 0x00116FC8 File Offset: 0x001153C8
	Private Sub Torch1Counter()
		If Me.Counter >= MyBase.properties.CurrentState.blowtorch.warningDurationOne Then
			Me.Counter = 0
			AudioManager.Play("level_dragon_three_dragon_head_b_torch_continue_one")
			Me.emitAudioFromObject.Add("level_dragon_three_dragon_head_b_torch_continue_one")
			MyBase.animator.Play("Torch_Continue", 4)
		Else
			Me.Counter += 1
		End If
	End Sub

	' Token: 0x06001E59 RID: 7769 RVA: 0x0011703C File Offset: 0x0011543C
	Private Sub Attack1Counter()
		If Me.Counter >= Me.AttackFrames / 2 + Me.AttackFrames Mod 2 Then
			Me.Counter = 0
			AudioManager.Play("level_dragon_three_dragon_head_b_torch_warning_2_start")
			Me.emitAudioFromObject.Add("level_dragon_three_dragon_head_b_torch_warning_2_start")
			MyBase.animator.Play("Torch_Warning_Two", 4)
		Else
			Me.Counter += 1
		End If
	End Sub

	' Token: 0x06001E5A RID: 7770 RVA: 0x001170AC File Offset: 0x001154AC
	Private Sub Torch2Counter()
		If Me.Counter >= MyBase.properties.CurrentState.blowtorch.warningDurationTwo Then
			Me.Counter = 0
			AudioManager.Play("level_dragon_three_dragon_head_b_torch_continue_one")
			Me.emitAudioFromObject.Add("level_dragon_three_dragon_head_b_torch_continue_one")
			MyBase.animator.Play("Torch_Continue_Two", 4)
		Else
			Me.Counter += 1
		End If
	End Sub

	' Token: 0x06001E5B RID: 7771 RVA: 0x00117120 File Offset: 0x00115520
	Private Sub Attack2Counter()
		If Me.Counter >= Me.AttackFrames / 2 Then
			Me.Counter = 0
			AudioManager.Play("level_dragon_three_dragon_head_b_torch_end")
			Me.emitAudioFromObject.Add("level_dragon_three_dragon_head_b_torch_end")
			MyBase.animator.Play("Torch_End", 4)
		Else
			Me.Counter += 1
		End If
	End Sub

	' Token: 0x06001E5C RID: 7772 RVA: 0x00117188 File Offset: 0x00115588
	Private Sub StartDeath()
		AudioManager.Play("level_dragon_three_dragon_death")
		Me.emitAudioFromObject.Add("level_dragon_three_dragon_death")
		Me.StopAllCoroutines()
		Me.middleHead.gameObject.SetActive(False)
		MyBase.animator.SetTrigger("Continue")
		If Level.Current.mode = Level.Mode.Easy Then
			MyBase.animator.SetTrigger("DeadEASY")
		Else
			MyBase.animator.SetTrigger("Dead")
		End If
	End Sub

	' Token: 0x04002708 RID: 9992
	Private Const FRAME_RATE As Single = 0.041666668F

	' Token: 0x0400270A RID: 9994
	Private headPicked As DragonLevelLeftSideDragon.HeadPicked

	' Token: 0x0400270B RID: 9995
	Private Const MAIN_LAYER As Integer = 0

	' Token: 0x0400270C RID: 9996
	Private Const TONGUE_LAYER As Integer = 1

	' Token: 0x0400270D RID: 9997
	Private Const FIRE_LAYER As Integer = 2

	' Token: 0x0400270E RID: 9998
	Private Const introTime As Single = 1.3F

	' Token: 0x0400270F RID: 9999
	Private Const mainX As Single = -350F

	' Token: 0x04002710 RID: 10000
	<SerializeField()>
	Private damageBox As Collider2D

	' Token: 0x04002711 RID: 10001
	<SerializeField()>
	Private spire As DragonLevelSpire

	' Token: 0x04002712 RID: 10002
	<SerializeField()>
	Private rain As DragonLevelRain

	' Token: 0x04002713 RID: 10003
	<SerializeField()>
	Private backgrounds As DragonLevelBackgroundChange()

	' Token: 0x04002714 RID: 10004
	<SerializeField()>
	Private backgroundsToHide As GameObject()

	' Token: 0x04002715 RID: 10005
	<SerializeField()>
	Private fire As DragonLevelFire

	' Token: 0x04002716 RID: 10006
	<SerializeField()>
	Private fireMarcherRoot As Transform

	' Token: 0x04002717 RID: 10007
	<SerializeField()>
	Private fireMarcherPrefabs As DragonLevelFireMarcher()

	' Token: 0x04002718 RID: 10008
	<SerializeField()>
	Private fireMarcherLeaderPrefab As DragonLevelFireMarcher

	' Token: 0x04002719 RID: 10009
	<SerializeField()>
	Private topHead As Transform

	' Token: 0x0400271A RID: 10010
	<SerializeField()>
	Private bottomHead As Transform

	' Token: 0x0400271B RID: 10011
	<SerializeField()>
	Private middleHead As Transform

	' Token: 0x0400271C RID: 10012
	<SerializeField()>
	Private horizontalPotionPrefab As DragonLevelPotion

	' Token: 0x0400271D RID: 10013
	<SerializeField()>
	Private verticalPotionPrefab As DragonLevelPotion

	' Token: 0x0400271E RID: 10014
	<SerializeField()>
	Private bothPotionPrefab As DragonLevelPotion

	' Token: 0x0400271F RID: 10015
	<SerializeField()>
	Private spittle As Transform

	' Token: 0x04002720 RID: 10016
	Private lastFireMarcher As DragonLevelFireMarcher

	' Token: 0x04002721 RID: 10017
	Private damageDealer As DamageDealer

	' Token: 0x04002722 RID: 10018
	Private damageReceiver As DamageReceiver

	' Token: 0x04002723 RID: 10019
	Private dead As Boolean

	' Token: 0x04002724 RID: 10020
	Private torch As Boolean

	' Token: 0x04002725 RID: 10021
	Private potionTypeIndex As Integer

	' Token: 0x04002726 RID: 10022
	Private potionTypeMainIndex As Integer

	' Token: 0x04002727 RID: 10023
	Private attackCountIndex As Integer

	' Token: 0x04002728 RID: 10024
	Private attackCountMainIndex As Integer

	' Token: 0x04002729 RID: 10025
	Private shotPositionIndex As Integer

	' Token: 0x0400272A RID: 10026
	Private shotPositionMainIndex As Integer

	' Token: 0x0400272B RID: 10027
	Private AttackFrames As Integer

	' Token: 0x0400272C RID: 10028
	Private Counter As Integer

	' Token: 0x0400272D RID: 10029
	Private animationString As String

	' Token: 0x0400272E RID: 10030
	Private layer As Integer

	' Token: 0x0400272F RID: 10031
	Private xPos As Single

	' Token: 0x020005F2 RID: 1522
	Public Enum State
		' Token: 0x04002731 RID: 10033
		UnSpawned
		' Token: 0x04002732 RID: 10034
		Fire
		' Token: 0x04002733 RID: 10035
		Transition
		' Token: 0x04002734 RID: 10036
		ThreeHeads
		' Token: 0x04002735 RID: 10037
		Dead
	End Enum

	' Token: 0x020005F3 RID: 1523
	Private Enum HeadPicked
		' Token: 0x04002737 RID: 10039
		ATop
		' Token: 0x04002738 RID: 10040
		ABottom
		' Token: 0x04002739 RID: 10041
		CTop
		' Token: 0x0400273A RID: 10042
		CBottom
		' Token: 0x0400273B RID: 10043
		None
	End Enum
End Class
