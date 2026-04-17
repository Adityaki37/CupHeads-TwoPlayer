Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x02000668 RID: 1640
Public Class FlyingGenieLevelGenie
	Inherits LevelProperties.FlyingGenie.Entity

	' Token: 0x17000393 RID: 915
	' (get) Token: 0x0600222B RID: 8747 RVA: 0x0013E30C File Offset: 0x0013C70C
	' (set) Token: 0x0600222C RID: 8748 RVA: 0x0013E314 File Offset: 0x0013C714
	Public Property state As FlyingGenieLevelGenie.State

	' Token: 0x0600222D RID: 8749 RVA: 0x0013E320 File Offset: 0x0013C720
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.defaultColor = MyBase.GetComponent(Of SpriteRenderer)().color
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
	End Sub

	' Token: 0x0600222E RID: 8750 RVA: 0x0013E374 File Offset: 0x0013C774
	Private Sub Start()
		MyBase.StartCoroutine(Me.intro_cr())
		Me.casketStartPos = Me.casket.transform.position
		MyBase.GetComponent(Of Collider2D)().enabled = False
		Me.hiero.speed = MyBase.properties.CurrentState.obelisk.obeliskMovementSpeed
		Me.brick.speed = MyBase.properties.CurrentState.obelisk.obeliskMovementSpeed
	End Sub

	' Token: 0x0600222F RID: 8751 RVA: 0x0013E3F0 File Offset: 0x0013C7F0
	Public Overrides Sub LevelInit(properties As LevelProperties.FlyingGenie)
		MyBase.LevelInit(properties)
		Me.treasureAttacks = New List(Of Integer)() From { 0, 1, 2 }
		Me.swordPinkPattern = properties.CurrentState.swords.swordPinkString.Split(New Char() { ","c })
		Me.swordPinkIndex = Global.UnityEngine.Random.Range(0, Me.swordPinkPattern.Length)
		Me.gemPinkPattern = properties.CurrentState.gems.gemPinkString.Split(New Char() { ","c })
		Me.gemPinkIndex = Global.UnityEngine.Random.Range(0, Me.gemPinkPattern.Length)
		Me.sphinxPinkPattern = properties.CurrentState.sphinx.scarabPinkString.Split(New Char() { ","c })
		Me.sphinxPinkIndex = Global.UnityEngine.Random.Range(0, Me.sphinxPinkPattern.Length)
	End Sub

	' Token: 0x06002230 RID: 8752 RVA: 0x0013E4D4 File Offset: 0x0013C8D4
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		MyBase.properties.DealDamage(info.damage)
	End Sub

	' Token: 0x06002231 RID: 8753 RVA: 0x0013E4E7 File Offset: 0x0013C8E7
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06002232 RID: 8754 RVA: 0x0013E505 File Offset: 0x0013C905
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06002233 RID: 8755 RVA: 0x0013E520 File Offset: 0x0013C920
	Private Iterator Function intro_cr() As IEnumerator
		Me.state = FlyingGenieLevelGenie.State.Intro
		Yield CupheadTime.WaitForSeconds(Me, 1.3F)
		MyBase.animator.SetTrigger("Continue")
		Me.GenieIntroSFX()
		Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.main.introHesitate)
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Intro_End", False, True)
		Me.state = FlyingGenieLevelGenie.State.Idle
		Me.StartTreasure()
		MyBase.StartCoroutine(Me.skull_attack_cr())
		Yield Nothing
		Return
	End Function

	' Token: 0x06002234 RID: 8756 RVA: 0x0013E53B File Offset: 0x0013C93B
	Private Sub SpawnPuff()
		MyBase.StartCoroutine(Me.handle_puff_cr(Me.puffEffect.Create(Me.puffRoot.position)))
	End Sub

	' Token: 0x06002235 RID: 8757 RVA: 0x0013E560 File Offset: 0x0013C960
	Private Sub StartCarpet()
		MyBase.animator.Play("Idle_Carpet")
		MyBase.StartCoroutine(Me.handle_carpet_fadein())
	End Sub

	' Token: 0x06002236 RID: 8758 RVA: 0x0013E57F File Offset: 0x0013C97F
	Private Sub EndCarpet()
		MyBase.StartCoroutine(Me.handle_carpet_fadeout())
	End Sub

	' Token: 0x06002237 RID: 8759 RVA: 0x0013E590 File Offset: 0x0013C990
	Private Iterator Function handle_puff_cr(puff As Effect) As IEnumerator
		Yield puff.animator.WaitForAnimationToEnd(Me, "Start", False, True)
		Dim puffRenderer As SpriteRenderer = puff.GetComponent(Of SpriteRenderer)()
		While puff.transform.position.x > -740F
			puff.transform.position -= Vector3.right * 200F * CupheadTime.Delta
			If puff.transform.position.x < -540F Then
				Dim color As Color = puffRenderer.color
				color.a -= 1F * CupheadTime.Delta
				puffRenderer.color = color
			End If
			Yield Nothing
		End While
		Global.UnityEngine.[Object].Destroy(puff.gameObject)
		Yield Nothing
		Return
	End Function

	' Token: 0x06002238 RID: 8760 RVA: 0x0013E5B4 File Offset: 0x0013C9B4
	Private Iterator Function handle_carpet_fadein() As IEnumerator
		Me.carpet.color = New Color(1F, 1F, 1F, 0F)
		Dim t As Single = 0F
		Dim time As Single = 2F
		While t < time
			Me.carpet.color = New Color(1F, 1F, 1F, t / time)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		Me.carpet.color = New Color(1F, 1F, 1F, 1F)
		Yield Nothing
		Return
	End Function

	' Token: 0x06002239 RID: 8761 RVA: 0x0013E5D0 File Offset: 0x0013C9D0
	Private Iterator Function handle_carpet_fadeout() As IEnumerator
		Me.carpet.color = New Color(1F, 1F, 1F, 1F)
		Dim t As Single = 0F
		Dim time As Single = 2F
		While t < time
			Me.carpet.color = New Color(1F, 1F, 1F, 1F - t / time)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		Me.carpet.color = New Color(1F, 1F, 1F, 0F)
		Yield Nothing
		Return
	End Function

	' Token: 0x0600223A RID: 8762 RVA: 0x0013E5EB File Offset: 0x0013C9EB
	Public Sub HitTrigger()
		Me.attackLooping = False
	End Sub

	' Token: 0x0600223B RID: 8763 RVA: 0x0013E5F4 File Offset: 0x0013C9F4
	Public Sub StartTreasure()
		Me.state = FlyingGenieLevelGenie.State.Treasure
		Me.skullCounter = 0
		MyBase.animator.SetBool("OnTreasure", True)
		Me.attackLooping = True
		Dim num As Integer = Global.UnityEngine.Random.Range(0, Me.treasureAttacks.Count)
		Me.treasureCounter = Me.treasureAttacks(num)
		Select Case Me.treasureCounter
			Case 0
				Me.StartSwords()
			Case 1
				Me.StartGems()
			Case 2
				Me.StartSphinx()
			Case Else
				Global.Debug.LogError("The counter is messed up: " + Me.treasureCounter, Nothing)
		End Select
		Me.treasureAttacks.Remove(num)
	End Sub

	' Token: 0x0600223C RID: 8764 RVA: 0x0013E6B8 File Offset: 0x0013CAB8
	Private Iterator Function skull_attack_cr() As IEnumerator
		While True
			If MyBase.animator.GetCurrentAnimatorStateInfo(0).IsName("Chest_Idle") AndAlso Me.skullCounter < MyBase.properties.CurrentState.skull.skullCount Then
				Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.skull.skullDelayRange)
				MyBase.animator.SetTrigger("OnSkull")
				AudioManager.Play("genie_skull_release")
				Me.emitAudioFromObject.Add("genie_skull_release")
				Yield MyBase.animator.WaitForAnimationToEnd(Me, "Chest_Skull_Attack", False, True)
				Me.skullCounter += 1
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600223D RID: 8765 RVA: 0x0013E6D4 File Offset: 0x0013CAD4
	Private Sub SpawnSkull()
		Me.skullPrefab.Create(Me.skullRoot.transform.position, 0F, -MyBase.properties.CurrentState.skull.skullSpeed)
		AudioManager.Play("genie_skull_release_projectile")
		Me.emitAudioFromObject.Add("genie_skull_release_projectile")
	End Sub

	' Token: 0x0600223E RID: 8766 RVA: 0x0013E737 File Offset: 0x0013CB37
	Private Sub DisableCarpet()
		MyBase.animator.Play("Off")
	End Sub

	' Token: 0x0600223F RID: 8767 RVA: 0x0013E749 File Offset: 0x0013CB49
	Private Sub EnableCarpet()
		MyBase.animator.Play("Idle_Carpet")
	End Sub

	' Token: 0x06002240 RID: 8768 RVA: 0x0013E75B File Offset: 0x0013CB5B
	Private Sub EnableChestIdle()
		MyBase.animator.Play("Idle_Carpet_Chest")
	End Sub

	' Token: 0x06002241 RID: 8769 RVA: 0x0013E76D File Offset: 0x0013CB6D
	Public Sub StartSwords()
		If Me.patternCoroutine IsNot Nothing Then
			MyBase.StopCoroutine(Me.patternCoroutine)
		End If
		Me.patternCoroutine = MyBase.StartCoroutine(Me.swords_cr())
	End Sub

	' Token: 0x06002242 RID: 8770 RVA: 0x0013E798 File Offset: 0x0013CB98
	Private Iterator Function swords_cr() As IEnumerator
		Me.attackLooping = True
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Chest_Intro", False, True)
		Dim p As LevelProperties.FlyingGenie.Swords = MyBase.properties.CurrentState.swords
		Dim positionIndex As Integer = Global.UnityEngine.Random.Range(0, p.patternPositionStrings.Length)
		Dim positionPattern As String() = p.patternPositionStrings(positionIndex).Split(New Char() { ","c })
		Dim endPosition As Vector3 = Vector3.zero
		Dim location As Single = 0F
		While Me.attackLooping
			positionPattern = p.patternPositionStrings(positionIndex).Split(New Char() { ","c })
			For i As Integer = 0 To positionPattern.Length - 1
				Dim coordinates As String() = positionPattern(i).Split(New Char() { "-"c })
				For j As Integer = 0 To coordinates.Length - 1
					Parser.FloatTryParse(coordinates(j), location)
					If j Mod 2 = 0 Then
						endPosition.x = -640F + location
					Else
						endPosition.y = 360F - location
					End If
				Next
				Me.SpawnSwords(endPosition)
				If Not Me.attackLooping Then
					Exit For
				End If
				Yield CupheadTime.WaitForSeconds(Me, p.spawnDelay)
			Next
			If Not Me.attackLooping Then
				Exit While
			End If
			Yield CupheadTime.WaitForSeconds(Me, p.repeatDelay)
			positionIndex = (positionIndex + 1) Mod p.patternPositionStrings.Length
			Yield Nothing
		End While
		MyBase.animator.SetBool("OnTreasure", False)
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Chest_Outro", False, True)
		Yield CupheadTime.WaitForSeconds(Me, p.hesitate)
		Me.state = FlyingGenieLevelGenie.State.Idle
		Yield Nothing
		Return
	End Function

	' Token: 0x06002243 RID: 8771 RVA: 0x0013E7B4 File Offset: 0x0013CBB4
	Private Sub SpawnSwords(pos As Vector3)
		Dim [next] As AbstractPlayerController = PlayerManager.GetNext()
		Dim flyingGenieLevelSword As FlyingGenieLevelSword = Global.UnityEngine.[Object].Instantiate(Of FlyingGenieLevelSword)(Me.swordPrefab)
		flyingGenieLevelSword.Init(Me.treasureRoot.position, pos, MyBase.properties.CurrentState.swords, [next])
		flyingGenieLevelSword.SetParryable(Me.swordPinkPattern(Me.swordPinkIndex)(0) = "P"c)
		Me.swordPinkIndex = (Me.swordPinkIndex + 1) Mod Me.swordPinkPattern.Length
	End Sub

	' Token: 0x06002244 RID: 8772 RVA: 0x0013E82A File Offset: 0x0013CC2A
	Public Sub StartGems()
		If Me.patternCoroutine IsNot Nothing Then
			MyBase.StopCoroutine(Me.patternCoroutine)
		End If
		Me.patternCoroutine = MyBase.StartCoroutine(Me.gems_cr())
	End Sub

	' Token: 0x06002245 RID: 8773 RVA: 0x0013E858 File Offset: 0x0013CC58
	Private Iterator Function gems_cr() As IEnumerator
		Me.attackLooping = True
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Chest_Intro", False, True)
		AudioManager.Play("genie_chest_jewel_escape")
		Me.emitAudioFromObject.Add("genie_chest_jewel_escape")
		AudioManager.PlayLoop("genie_chest_magic_loop")
		Me.emitAudioFromObject.Add("genie_chest_magic_loop")
		While Me.attackLooping
			Me.smallGemTimerUp = False
			Me.bigGemTimerUp = False
			If Me.bigGemsRoutine IsNot Nothing Then
				MyBase.StopCoroutine(Me.bigGemsRoutine)
			End If
			Me.bigGemsRoutine = MyBase.StartCoroutine(Me.big_gems_cr())
			If Me.smallGemsRoutine IsNot Nothing Then
				MyBase.StopCoroutine(Me.smallGemsRoutine)
			End If
			Me.smallGemsRoutine = MyBase.StartCoroutine(Me.small_gems_cr())
			While Not Me.smallGemTimerUp AndAlso Not Me.bigGemTimerUp
				If Not Me.attackLooping Then
					Exit While
				End If
				Yield Nothing
			End While
			If Me.attackLooping Then
				Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.gems.repeatDelay)
			End If
			Yield Nothing
		End While
		MyBase.animator.SetBool("OnTreasure", False)
		Yield MyBase.animator.WaitForAnimationToStart(Me, "Chest_Outro", False)
		AudioManager.[Stop]("genie_chest_magic_loop")
		AudioManager.Play("genie_chest_magic_loop_end")
		Me.emitAudioFromObject.Add("genie_chest_magic_loop_end")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Chest_Outro", False, True)
		Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.gems.hesitate)
		Me.state = FlyingGenieLevelGenie.State.Idle
		Yield Nothing
		Return
	End Function

	' Token: 0x06002246 RID: 8774 RVA: 0x0013E874 File Offset: 0x0013CC74
	Private Iterator Function small_gems_cr() As IEnumerator
		Dim p As LevelProperties.FlyingGenie.Gems = MyBase.properties.CurrentState.gems
		Me.smallGemTimerUp = False
		Dim mainOffsetIndex As Integer = Global.UnityEngine.Random.Range(0, p.gemSmallAimOffset.Length)
		Dim smallOffsetString As String() = p.gemSmallAimOffset(mainOffsetIndex).Split(New Char() { ","c })
		Dim offsetIndex As Integer = Global.UnityEngine.Random.Range(0, smallOffsetString.Length)
		Dim offset As Single = 0F
		MyBase.StartCoroutine(Me.small_gem_timer_cr())
		While Not Me.smallGemTimerUp AndAlso Me.attackLooping
			smallOffsetString = p.gemSmallAimOffset(mainOffsetIndex).Split(New Char() { ","c })
			Parser.FloatTryParse(smallOffsetString(offsetIndex), offset)
			Dim player As AbstractPlayerController = PlayerManager.GetNext()
			Me.gemPrefab.Create(Me.treasureRoot.position, player, offset, p.gemSmallSpeed, Me.gemPinkPattern(Me.gemPinkIndex)(0) = "P"c, False)
			Me.gemPinkIndex = (Me.gemPinkIndex + 1) Mod Me.gemPinkPattern.Length
			Yield CupheadTime.WaitForSeconds(Me, p.gemSmallDelayRange.RandomFloat())
			If offsetIndex < smallOffsetString.Length - 1 Then
				offsetIndex += 1
			Else
				mainOffsetIndex = (mainOffsetIndex + 1) Mod p.gemSmallAimOffset.Length
				offsetIndex = 0
			End If
		End While
		Yield Nothing
		Return
	End Function

	' Token: 0x06002247 RID: 8775 RVA: 0x0013E890 File Offset: 0x0013CC90
	Private Iterator Function small_gem_timer_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.gems.gemSmallAttackDuration)
		Me.smallGemTimerUp = True
		Return
	End Function

	' Token: 0x06002248 RID: 8776 RVA: 0x0013E8AC File Offset: 0x0013CCAC
	Private Iterator Function big_gems_cr() As IEnumerator
		Dim p As LevelProperties.FlyingGenie.Gems = MyBase.properties.CurrentState.gems
		Me.bigGemTimerUp = False
		Dim mainOffsetIndex As Integer = Global.UnityEngine.Random.Range(0, p.gemBigAimOffset.Length)
		Dim bigOffsetString As String() = p.gemBigAimOffset(mainOffsetIndex).Split(New Char() { ","c })
		Dim offsetIndex As Integer = Global.UnityEngine.Random.Range(0, bigOffsetString.Length)
		Dim offset As Single = 0F
		MyBase.StartCoroutine(Me.big_gems_timer_cr())
		While Not Me.bigGemTimerUp AndAlso Me.attackLooping
			Parser.FloatTryParse(bigOffsetString(offsetIndex), offset)
			Dim player As AbstractPlayerController = PlayerManager.GetNext()
			Me.gemPrefab.Create(Me.treasureRoot.position, player, offset, p.gemBigSpeed, False, True)
			Yield CupheadTime.WaitForSeconds(Me, p.gemBigDelayRange.RandomFloat())
			If offsetIndex < bigOffsetString.Length - 1 Then
				offsetIndex += 1
			Else
				mainOffsetIndex = (mainOffsetIndex + 1) Mod p.gemBigAimOffset.Length
				offsetIndex = 0
			End If
			Yield Nothing
		End While
		Yield Nothing
		Return
	End Function

	' Token: 0x06002249 RID: 8777 RVA: 0x0013E8C8 File Offset: 0x0013CCC8
	Private Iterator Function big_gems_timer_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.gems.gemBigAttackDuration)
		Me.bigGemTimerUp = True
		Return
	End Function

	' Token: 0x0600224A RID: 8778 RVA: 0x0013E8E3 File Offset: 0x0013CCE3
	Public Sub StartSphinx()
		If Me.patternCoroutine IsNot Nothing Then
			MyBase.StopCoroutine(Me.patternCoroutine)
		End If
		Me.patternCoroutine = MyBase.StartCoroutine(Me.sphinx_cr())
	End Sub

	' Token: 0x0600224B RID: 8779 RVA: 0x0013E910 File Offset: 0x0013CD10
	Private Iterator Function sphinx_cr() As IEnumerator
		Me.attackLooping = True
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Chest_Intro", False, True)
		AudioManager.Play("genie_chest_jewel_escape")
		Me.emitAudioFromObject.Add("genie_chest_jewel_escape")
		AudioManager.PlayLoop("genie_chest_magic_loop_nojingle")
		Me.emitAudioFromObject.Add("genie_chest_magic_loop_nojingle")
		Dim p As LevelProperties.FlyingGenie.Sphinx = MyBase.properties.CurrentState.sphinx
		Dim mainCountIndex As Integer = Global.UnityEngine.Random.Range(0, p.sphinxCount.Length)
		Dim sphinxCountPattern As String() = p.sphinxCount(mainCountIndex).Split(New Char() { ","c })
		Dim countIndex As Integer = Global.UnityEngine.Random.Range(0, sphinxCountPattern.Length)
		Dim mainXIndex As Integer = Global.UnityEngine.Random.Range(0, p.sphinxAimX.Length)
		Dim sphinxPosXPattern As String() = p.sphinxAimX(mainXIndex).Split(New Char() { ","c })
		Dim posXIndex As Integer = Global.UnityEngine.Random.Range(0, sphinxPosXPattern.Length)
		Dim mainYIndex As Integer = Global.UnityEngine.Random.Range(0, p.sphinxAimY.Length)
		Dim sphinxPosYPattern As String() = p.sphinxAimY(mainYIndex).Split(New Char() { ","c })
		Dim posYIndex As Integer = Global.UnityEngine.Random.Range(0, sphinxPosYPattern.Length)
		Dim sphinxCount As Single = 0F
		While Me.attackLooping
			sphinxCountPattern = p.sphinxCount(mainCountIndex).Split(New Char() { ","c })
			sphinxPosXPattern = p.sphinxAimX(mainXIndex).Split(New Char() { ","c })
			sphinxPosYPattern = p.sphinxAimY(mainYIndex).Split(New Char() { ","c })
			Parser.FloatTryParse(sphinxCountPattern(countIndex), sphinxCount)
			Dim i As Integer = 0
			While CSng(i) < sphinxCount
				Me.SpawnSphinx()
				Yield CupheadTime.WaitForSeconds(Me, p.sphinxMainDelay)
				If posXIndex < p.sphinxAimX.Length - 1 Then
					posXIndex += 1
				Else
					mainXIndex = (mainXIndex + 1) Mod p.sphinxAimX.Length
					posXIndex = 0
				End If
				If posYIndex < p.sphinxAimY.Length - 1 Then
					posYIndex += 1
				Else
					mainYIndex = (mainYIndex + 1) Mod p.sphinxAimY.Length
					posYIndex = 0
				End If
				If Not Me.attackLooping Then
					Exit While
				End If
				i += 1
			End While
			If Me.attackLooping Then
				Yield CupheadTime.WaitForSeconds(Me, p.repeatDelay)
			End If
			If countIndex < p.sphinxCount.Length - 1 Then
				countIndex += 1
			Else
				mainCountIndex = (mainCountIndex + 1) Mod p.sphinxCount.Length
				countIndex = 0
			End If
		End While
		MyBase.animator.SetBool("OnTreasure", False)
		Yield MyBase.animator.WaitForAnimationToStart(Me, "Chest_Outro", False)
		AudioManager.[Stop]("genie_chest_magic_loop_nojingle")
		AudioManager.Play("genie_chest_magic_loop_nojingle_end")
		Me.emitAudioFromObject.Add("genie_chest_magic_loop_nojingle_end")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Chest_Outro", False, True)
		Yield CupheadTime.WaitForSeconds(Me, p.hesitate)
		Me.state = FlyingGenieLevelGenie.State.Idle
		Yield Nothing
		Return
	End Function

	' Token: 0x0600224C RID: 8780 RVA: 0x0013E92C File Offset: 0x0013CD2C
	Private Sub SpawnSphinx()
		Dim [next] As AbstractPlayerController = PlayerManager.GetNext()
		Dim flyingGenieLevelSphinx As FlyingGenieLevelSphinx = Global.UnityEngine.[Object].Instantiate(Of FlyingGenieLevelSphinx)(Me.sphinxPrefab)
		flyingGenieLevelSphinx.Init(Me.treasureRoot.position, MyBase.properties.CurrentState.sphinx, [next], Me.sphinxPinkPattern, Me.sphinxPinkIndex)
		Me.sphinxPinkIndex = (Me.sphinxPinkIndex + CInt(MyBase.properties.CurrentState.sphinx.sphinxSpawnNum)) Mod Me.sphinxPinkPattern.Length
	End Sub

	' Token: 0x0600224D RID: 8781 RVA: 0x0013E9A5 File Offset: 0x0013CDA5
	Public Sub StartCoffin()
		Me.state = FlyingGenieLevelGenie.State.Coffin
		MyBase.animator.SetBool("OnDisappear", True)
		MyBase.StartCoroutine(Me.coffin_cr())
	End Sub

	' Token: 0x0600224E RID: 8782 RVA: 0x0013E9CC File Offset: 0x0013CDCC
	Private Iterator Function coffin_cr() As IEnumerator
		Me.attackLooping = True
		Dim p As LevelProperties.FlyingGenie.Coffin = MyBase.properties.CurrentState.coffin
		Dim mainPosIndex As Integer = Global.UnityEngine.Random.Range(0, p.mummyAppearString.Length)
		Dim coffinPosPattern As String() = p.mummyAppearString(mainPosIndex).Split(New Char() { ","c })
		Dim posIndex As Integer = Global.UnityEngine.Random.Range(0, coffinPosPattern.Length)
		Dim mainAngleIndex As Integer = Global.UnityEngine.Random.Range(0, p.mummyGenieDirection.Length)
		Dim coffinAnglePattern As String() = p.mummyGenieDirection(mainAngleIndex).Split(New Char() { ","c })
		Dim angleIndex As Integer = Global.UnityEngine.Random.Range(0, coffinAnglePattern.Length)
		Dim mainTypeIndex As Integer = Global.UnityEngine.Random.Range(0, p.mummyTypeString.Length)
		Dim coffinTypePattern As String() = p.mummyTypeString(mainTypeIndex).Split(New Char() { ","c })
		Dim typeIndex As Integer = Global.UnityEngine.Random.Range(0, coffinTypePattern.Length)
		Dim pos As Vector3 = Vector3.zero
		Dim position As Single = 0F
		Dim angle As Single = 0F
		Dim sortingOrder As Integer = 0
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		AudioManager.Play("genie_sarcophagus_enter")
		Me.emitAudioFromObject.Add("genie_sarcophagus_enter")
		While Me.casket.transform.position.y > -30F
			Me.casket.transform.AddPosition(0F, -800F * CupheadTime.Delta, 0F)
			Yield Nothing
		End While
		CupheadLevelCamera.Current.Shake(10F, 0.4F, False)
		Me.casket.GetComponent(Of Animator)().SetTrigger("StartCasket")
		Yield Me.casket.GetComponent(Of Animator)().WaitForAnimationToEnd(Me, "Open_Start", False, True)
		Me.goop.ActivateGoop()
		Yield Me.goop.GetComponent(Of Animator)().WaitForAnimationToEnd(Me, "Intro", False, True)
		While Me.attackLooping AndAlso MyBase.properties.CurrentState.stateName = LevelProperties.FlyingGenie.States.Disappear
			coffinPosPattern = p.mummyAppearString(mainPosIndex).Split(New Char() { ","c })
			coffinTypePattern = p.mummyTypeString(mainTypeIndex).Split(New Char() { ","c })
			coffinAnglePattern = p.mummyGenieDirection(mainAngleIndex).Split(New Char() { ","c })
			Parser.FloatTryParse(coffinPosPattern(posIndex), position)
			Parser.FloatTryParse(coffinAnglePattern(angleIndex), angle)
			pos = New Vector3(Me.casket.transform.position.x + 200F, position, 0F)
			If coffinTypePattern(typeIndex)(0) = "A"c Then
				Me.mummyClassic.Create(pos, -p.mummyASpeed, -angle, p, FlyingGenieLevelMummy.MummyType.Classic, p.mummyGenieHP, sortingOrder)
			ElseIf coffinTypePattern(typeIndex)(0) = "B"c Then
				Me.mummyChomper.Create(pos, -p.mummyBSpeed, -angle, p, FlyingGenieLevelMummy.MummyType.Chomper, p.mummyGenieHP, sortingOrder)
			ElseIf coffinTypePattern(typeIndex)(0) = "C"c Then
				Me.mummyChaser.Create(pos, -p.mummyCSpeed, -angle, p, FlyingGenieLevelMummy.MummyType.Grabby, p.mummyGenieHP, sortingOrder)
			End If
			Yield CupheadTime.WaitForSeconds(Me, p.mummyGenieDelay)
			If posIndex < coffinPosPattern.Length - 1 Then
				posIndex += 1
			Else
				mainPosIndex = (mainPosIndex + 1) Mod p.mummyAppearString.Length
				posIndex = 0
			End If
			If typeIndex < coffinTypePattern.Length - 1 Then
				typeIndex += 1
			Else
				mainTypeIndex = (mainTypeIndex + 1) Mod p.mummyTypeString.Length
				typeIndex = 0
			End If
			If angleIndex < coffinAnglePattern.Length - 1 Then
				angleIndex += 1
			Else
				mainAngleIndex = (mainAngleIndex + 1) Mod p.mummyGenieDirection.Length
				angleIndex = 0
			End If
			sortingOrder += 2
		End While
		Me.goop.StartDeath()
		Dim explosion As LevelBossDeathExploder = Me.casket.GetComponent(Of LevelBossDeathExploder)()
		explosion.StartExplosion()
		Me.casket.GetComponent(Of Animator)().SetTrigger("OnClose")
		AudioManager.Play("genie_sarcophagus_exit")
		Me.emitAudioFromObject.Add("genie_sarcophagus_exit")
		Yield Me.casket.GetComponent(Of Animator)().WaitForAnimationToEnd(Me, "Close", False, True)
		While Me.casket.transform.position.x < 1140F
			Me.casket.transform.AddPosition(200F * CupheadTime.Delta, 0F, 0F)
			Yield Nothing
		End While
		Me.casket.transform.position = Me.casketStartPos
		Me.casket.GetComponent(Of Animator)().SetTrigger("EndCasket")
		explosion.StopExplosions()
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		Me.FadeIntoIdle()
		Yield CupheadTime.WaitForSeconds(Me, p.hesitate)
		Me.state = FlyingGenieLevelGenie.State.Idle
		Yield Nothing
		Return
	End Function

	' Token: 0x0600224F RID: 8783 RVA: 0x0013E9E8 File Offset: 0x0013CDE8
	Public Sub StartObelisk()
		If Me.patternCoroutine IsNot Nothing Then
			MyBase.StopCoroutine(Me.patternCoroutine)
		End If
		If Me.bigGemsRoutine IsNot Nothing Then
			MyBase.StopCoroutine(Me.bigGemsRoutine)
		End If
		If Me.smallGemsRoutine IsNot Nothing Then
			MyBase.StopCoroutine(Me.smallGemsRoutine)
		End If
		Me.state = FlyingGenieLevelGenie.State.Disappear
		MyBase.animator.SetBool("OnDisappear", True)
		MyBase.StartCoroutine(Me.obelisk_cr())
		MyBase.StartCoroutine(Me.genie_laugh_sound_cr())
	End Sub

	' Token: 0x06002250 RID: 8784 RVA: 0x0013EA6C File Offset: 0x0013CE6C
	Private Iterator Function obelisk_cr() As IEnumerator
		Dim p As LevelProperties.FlyingGenie.Obelisk = MyBase.properties.CurrentState.obelisk
		Me.attackLooping = True
		Dim startPos As Vector3 = Vector3.zero
		startPos.x = 1340F
		startPos.y = 360F
		Dim t As Single = 0F
		Dim time As Single = 1F
		Dim angle As Single = 0F
		Dim firstPillar As Boolean = True
		Me.obelisks = New List(Of FlyingGenieLevelObelisk)()
		Dim obelisksListIndex As Integer = 0
		Dim obeliskPoolSize As Integer = 6
		Dim obeliskCounter As Integer = 0
		Dim mainObeliskIndex As Integer = Global.UnityEngine.Random.Range(0, p.obeliskGeniePos.Length)
		Dim blockOrderPattern As String() = p.obeliskGeniePos(mainObeliskIndex).Split(New Char() { ","c })
		Dim obeliskIndex As Integer = Global.UnityEngine.Random.Range(0, blockOrderPattern.Length)
		Dim mainBouncerIndex As Integer = Global.UnityEngine.Random.Range(0, p.bouncerAngleString.Length)
		Dim bouncerPattern As String() = p.bouncerAngleString(mainBouncerIndex).Split(New Char() { ","c })
		Dim bouncerIndex As Integer = Global.UnityEngine.Random.Range(0, bouncerPattern.Length)
		For i As Integer = 0 To obeliskPoolSize - 1
			Dim flyingGenieLevelObelisk As FlyingGenieLevelObelisk = Global.UnityEngine.[Object].Instantiate(Of FlyingGenieLevelObelisk)(Me.obeliskPrefab)
			flyingGenieLevelObelisk.Init(startPos, p, Me, i = 0)
			Me.obelisks.Add(flyingGenieLevelObelisk)
		Next
		Yield MyBase.animator.WaitForAnimationToStart(Me, "Genie_Meditate", False)
		Me.sawMask.gameObject.SetActive(True)
		While t < time
			Dim pos As Vector3 = Me.hieroBG.position
			Dim pos2 As Vector3 = Me.brickBG.position
			Dim val As Single = EaseUtils.Ease(EaseUtils.EaseType.easeInBounce, 0F, 1F, t / time)
			pos.y = Mathf.Lerp(Me.hieroBG.position.y, 340F, val)
			pos2.y = Mathf.Lerp(Me.brickBG.position.y, -320F, val)
			Me.hieroBG.position = pos
			Me.brickBG.position = pos2
			t += CupheadTime.Delta
			Yield Nothing
		End While
		While obeliskCounter < p.obeliskCount
			Parser.FloatTryParse(bouncerPattern(bouncerIndex), angle)
			Dim headLocations As String() = blockOrderPattern(obeliskIndex).Split(New Char() { "-"c })
			Me.obelisks(obelisksListIndex).ActivateObelisk(headLocations)
			If p.bounceShotOn Then
				If Not firstPillar Then
					Dim num As Integer
					If obelisksListIndex <= 0 Then
						num = Me.obelisks.Count - 1
					Else
						num = obelisksListIndex - 1
					End If
					Me.SpawnBouncer(Me.obelisks(obelisksListIndex), Me.obelisks(num), angle)
				Else
					Dim num2 As Single = Vector3.Distance(Me.obelisks(obelisksListIndex + 1).transform.position, MyBase.transform.position)
					Me.obelisks(obelisksListIndex).SetColliders((Me.obelisks(obelisksListIndex + 1).transform.position.x + Mathf.Abs(num2 / 2F)) / 2F, MyBase.transform.position.x - num2 / 2F)
					firstPillar = False
				End If
			End If
			obelisksListIndex = (obelisksListIndex + 1) Mod Me.obelisks.Count
			Yield Nothing
			Yield CupheadTime.WaitForSeconds(Me, p.obeliskAppearDelay)
			If obeliskIndex < blockOrderPattern.Length - 1 Then
				obeliskIndex += 1
			Else
				mainObeliskIndex = (mainObeliskIndex + 1) Mod p.obeliskGeniePos.Length
				obeliskIndex = 0
			End If
			If bouncerIndex < bouncerPattern.Length - 1 Then
				bouncerIndex += 1
			Else
				mainBouncerIndex = (mainBouncerIndex + 1) Mod p.bouncerAngleString.Length
				bouncerIndex = 0
			End If
			blockOrderPattern = p.obeliskGeniePos(mainObeliskIndex).Split(New Char() { ","c })
			bouncerPattern = p.bouncerAngleString(mainBouncerIndex).Split(New Char() { ","c })
			obeliskCounter += 1
			Yield Nothing
		End While
		For Each obelisk As FlyingGenieLevelObelisk In Me.obelisks
			If obelisk.isOn Then
				While obelisk.transform.position.x > -640F
					Yield Nothing
				End While
			End If
		Next
		AudioManager.[Stop]("genie_pillar_main_loop")
		AudioManager.[Stop]("genie_pillar_destructable_loop")
		Me.sawMask.gameObject.SetActive(False)
		MyBase.StartCoroutine(Me.delete_obelisks_cr(Me.obelisks))
		Me.state = FlyingGenieLevelGenie.State.Idle
		Me.StartCoffin()
		Yield Nothing
		Return
	End Function

	' Token: 0x06002251 RID: 8785 RVA: 0x0013EA88 File Offset: 0x0013CE88
	Private Iterator Function delete_obelisks_cr(obelisks As List(Of FlyingGenieLevelObelisk)) As IEnumerator
		Dim t As Single = 0F
		Dim time As Single = 2F
		For Each obelisk As FlyingGenieLevelObelisk In obelisks
			If obelisk.isOn Then
				While obelisk.transform.position.x > -740F
					Yield Nothing
				End While
			End If
			Global.UnityEngine.[Object].Destroy(obelisk.gameObject)
			Yield Nothing
		Next
		While t < time
			Dim pos As Vector3 = Me.hieroBG.position
			Dim pos2 As Vector3 = Me.brickBG.position
			Dim val As Single = EaseUtils.Ease(EaseUtils.EaseType.easeOutBounce, 0F, 1F, t / time)
			pos.y = Mathf.Lerp(Me.hieroBG.position.y, 460F, val)
			pos2.y = Mathf.Lerp(Me.brickBG.position.y, -460F, val)
			Me.hieroBG.position = pos
			Me.brickBG.position = pos2
			t += CupheadTime.Delta
			Yield Nothing
		End While
		Yield Nothing
		Return
	End Function

	' Token: 0x06002252 RID: 8786 RVA: 0x0013EAAC File Offset: 0x0013CEAC
	Private Sub SpawnBouncer(currentObelisk As FlyingGenieLevelObelisk, lastObelisk As FlyingGenieLevelObelisk, angle As Single)
		Dim num As Single = Vector3.Distance(lastObelisk.transform.position, currentObelisk.transform.position)
		Dim position As Vector3 = lastObelisk.transform.position
		position.x = currentObelisk.transform.position.x - num / 2F
		Dim num2 As Single = 150F
		Dim num3 As Single = 180F - (90F + angle / 2F)
		currentObelisk.SetColliders((lastObelisk.transform.position.x + Mathf.Abs(num / 2F)) / 2F, currentObelisk.transform.position.x - num / 2F)
		position.y = Global.UnityEngine.Random.Range(CSng(Level.Current.Ceiling) - num2, CSng(Level.Current.Ground) + num2)
		Dim flyingGenieLevelBouncer As FlyingGenieLevelBouncer = Global.UnityEngine.[Object].Instantiate(Of FlyingGenieLevelBouncer)(Me.bouncerPrefab).Init(position, MyBase.properties.CurrentState.obelisk, -num3)
		flyingGenieLevelBouncer.transform.parent = currentObelisk.transform
	End Sub

	' Token: 0x06002253 RID: 8787 RVA: 0x0013EBC5 File Offset: 0x0013CFC5
	Public Sub DoDamage(damage As Single)
		MyBase.properties.DealDamage(damage)
	End Sub

	' Token: 0x06002254 RID: 8788 RVA: 0x0013EBD3 File Offset: 0x0013CFD3
	Private Sub FadeIntoIdle()
		MyBase.StartCoroutine(Me.handle_fade_in_idle())
	End Sub

	' Token: 0x06002255 RID: 8789 RVA: 0x0013EBE4 File Offset: 0x0013CFE4
	Private Iterator Function handle_fade_in_idle() As IEnumerator
		MyBase.GetComponent(Of SpriteRenderer)().color = New Color(Me.defaultColor.r, Me.defaultColor.g, Me.defaultColor.a, 0F)
		Me.carpet.color = New Color(1F, 1F, 1F, 0F)
		Dim t As Single = 0F
		Dim time As Single = 0.3F
		MyBase.animator.Play("To_Phase_2")
		While t < time
			Me.carpet.color = New Color(1F, 1F, 1F, t / time)
			MyBase.GetComponent(Of SpriteRenderer)().color = New Color(Me.defaultColor.r, Me.defaultColor.g, Me.defaultColor.a, t / time)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		MyBase.GetComponent(Of SpriteRenderer)().color = New Color(Me.defaultColor.r, Me.defaultColor.g, Me.defaultColor.a, 1F)
		Me.carpet.color = New Color(1F, 1F, 1F, 1F)
		MyBase.GetComponent(Of Collider2D)().enabled = True
		Yield Nothing
		Return
	End Function

	' Token: 0x06002256 RID: 8790 RVA: 0x0013EBFF File Offset: 0x0013CFFF
	Public Sub StartPhase2()
		MyBase.StartCoroutine(Me.start_phase_2_cr())
	End Sub

	' Token: 0x06002257 RID: 8791 RVA: 0x0013EC10 File Offset: 0x0013D010
	Private Iterator Function start_phase_2_cr() As IEnumerator
		Me.genieTransformed.animator.Play("Genie_Head_Roll")
		Yield New WaitForEndOfFrame()
		Me.genieTransformed.StartMarionette(MyBase.transform.position, Me.meditateP1, Me.meditateP2)
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Yield Nothing
		Return
	End Function

	' Token: 0x06002258 RID: 8792 RVA: 0x0013EC2C File Offset: 0x0013D02C
	Private Sub CreateMeditateFX()
		Dim player As PlanePlayerController = PlayerManager.GetPlayer(Of PlanePlayerController)(PlayerId.PlayerOne)
		Dim player2 As PlanePlayerController = PlayerManager.GetPlayer(Of PlanePlayerController)(PlayerId.PlayerTwo)
		If player IsNot Nothing Then
			Me.meditateP1 = Global.UnityEngine.[Object].Instantiate(Of FlyingGenieLevelMeditateFX)(Me.meditateEffect)
			Me.meditateP1.transform.position = player.transform.position
			Me.meditateP1.transform.localScale = New Vector3(0.5F, 0.5F, 0F)
			Me.meditateP1.transform.parent = player.transform
		End If
		If player2 IsNot Nothing Then
			Me.meditateP2 = Global.UnityEngine.[Object].Instantiate(Of FlyingGenieLevelMeditateFX)(Me.meditateEffect)
			Me.meditateP2.transform.position = player2.transform.position
			Me.meditateP2.transform.localScale = New Vector3(0.5F, 0.5F, 0F)
			Me.meditateP2.transform.parent = player2.transform
		End If
	End Sub

	' Token: 0x06002259 RID: 8793 RVA: 0x0013ED2B File Offset: 0x0013D12B
	Private Sub GenieIntroSFX()
		AudioManager.Play("genie_entrance")
		Me.emitAudioFromObject.Add("genie_entrance")
	End Sub

	' Token: 0x0600225A RID: 8794 RVA: 0x0013ED47 File Offset: 0x0013D147
	Private Sub SoundGenieVoiceIntro()
		AudioManager.Play("genie_voice_intro_intake")
		Me.emitAudioFromObject.Add("genie_voice_intro_intake")
	End Sub

	' Token: 0x0600225B RID: 8795 RVA: 0x0013ED63 File Offset: 0x0013D163
	Private Sub SoundGenieVoiceEffort()
		AudioManager.Play("genie_voice_effort")
		Me.emitAudioFromObject.Add("genie_voice_effort")
	End Sub

	' Token: 0x0600225C RID: 8796 RVA: 0x0013ED7F File Offset: 0x0013D17F
	Private Sub SoundGenieVoiceLaugh()
		AudioManager.Play("genie_voice_laugh")
		Me.emitAudioFromObject.Add("genie_voice_laugh")
	End Sub

	' Token: 0x0600225D RID: 8797 RVA: 0x0013ED9B File Offset: 0x0013D19B
	Private Sub SoundGenieVoiceLure()
		AudioManager.Play("genie_voice_lure")
		Me.emitAudioFromObject.Add("genie_voice_lure")
	End Sub

	' Token: 0x0600225E RID: 8798 RVA: 0x0013EDB7 File Offset: 0x0013D1B7
	Private Sub SoundGenieVoiceMeditate()
		AudioManager.Play("genie_voice_meditate")
		Me.emitAudioFromObject.Add("genie_voice_meditate")
	End Sub

	' Token: 0x0600225F RID: 8799 RVA: 0x0013EDD3 File Offset: 0x0013D1D3
	Private Sub SoundGenieChestOpen()
		AudioManager.Play("genie_chest_attack_open")
		Me.emitAudioFromObject.Add("genie_chest_attack_open")
	End Sub

	' Token: 0x06002260 RID: 8800 RVA: 0x0013EDF0 File Offset: 0x0013D1F0
	Private Iterator Function genie_laugh_sound_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 6F)
		AudioManager.Play("genie_voice_laugh_reverb")
		Return
	End Function

	' Token: 0x06002261 RID: 8801 RVA: 0x0013EE0B File Offset: 0x0013D20B
	Private Sub SoundGenieTeleportDisappear()
		AudioManager.Play("genie_teleport_disappear")
		Me.emitAudioFromObject.Add("genie_teleport_disappear")
	End Sub

	' Token: 0x04002AD6 RID: 10966
	Private Const MUMMY_SPAWN_OFFSET As Single = 200F

	' Token: 0x04002AD8 RID: 10968
	<SerializeField()>
	Private hieroBG As Transform

	' Token: 0x04002AD9 RID: 10969
	<SerializeField()>
	Private brickBG As Transform

	' Token: 0x04002ADA RID: 10970
	<SerializeField()>
	Private hiero As ScrollingSprite

	' Token: 0x04002ADB RID: 10971
	<SerializeField()>
	Private brick As ScrollingSprite

	' Token: 0x04002ADC RID: 10972
	<SerializeField()>
	Private sawMask As Transform

	' Token: 0x04002ADD RID: 10973
	<SerializeField()>
	Private casket As GameObject

	' Token: 0x04002ADE RID: 10974
	<SerializeField()>
	Private meditateEffect As FlyingGenieLevelMeditateFX

	' Token: 0x04002ADF RID: 10975
	<SerializeField()>
	Private skullPrefab As BasicProjectile

	' Token: 0x04002AE0 RID: 10976
	<SerializeField()>
	Private bouncerPrefab As FlyingGenieLevelBouncer

	' Token: 0x04002AE1 RID: 10977
	<SerializeField()>
	Private obeliskPrefab As FlyingGenieLevelObelisk

	' Token: 0x04002AE2 RID: 10978
	<SerializeField()>
	Private sphinxPrefab As FlyingGenieLevelSphinx

	' Token: 0x04002AE3 RID: 10979
	<Space(10F)>
	<SerializeField()>
	Private gemPrefab As FlyingGenieLevelGem

	' Token: 0x04002AE4 RID: 10980
	<Space(10F)>
	<SerializeField()>
	Private goop As FlyingGenieLevelGoop

	' Token: 0x04002AE5 RID: 10981
	<SerializeField()>
	Private mummyClassic As FlyingGenieLevelMummy

	' Token: 0x04002AE6 RID: 10982
	<SerializeField()>
	Private mummyChomper As FlyingGenieLevelMummy

	' Token: 0x04002AE7 RID: 10983
	<SerializeField()>
	Private mummyChaser As FlyingGenieLevelMummy

	' Token: 0x04002AE8 RID: 10984
	<SerializeField()>
	Private swordPrefab As FlyingGenieLevelSword

	' Token: 0x04002AE9 RID: 10985
	<SerializeField()>
	Private genieTransformed As FlyingGenieLevelGenieTransform

	' Token: 0x04002AEA RID: 10986
	<Space(10F)>
	<SerializeField()>
	Private puffEffect As Effect

	' Token: 0x04002AEB RID: 10987
	<SerializeField()>
	Private puffRoot As Transform

	' Token: 0x04002AEC RID: 10988
	<SerializeField()>
	Private skullRoot As Transform

	' Token: 0x04002AED RID: 10989
	<SerializeField()>
	Private carpet As SpriteRenderer

	' Token: 0x04002AEE RID: 10990
	<SerializeField()>
	Private morphRoot As Transform

	' Token: 0x04002AEF RID: 10991
	<SerializeField()>
	Private treasureRoot As Transform

	' Token: 0x04002AF0 RID: 10992
	Private treasureAttacks As List(Of Integer)

	' Token: 0x04002AF1 RID: 10993
	Private obelisks As List(Of FlyingGenieLevelObelisk)

	' Token: 0x04002AF2 RID: 10994
	Private meditate As FlyingGenieLevelMeditateFX

	' Token: 0x04002AF3 RID: 10995
	Private damageReceiver As DamageReceiver

	' Token: 0x04002AF4 RID: 10996
	Private damageDealer As DamageDealer

	' Token: 0x04002AF5 RID: 10997
	Private attackLooping As Boolean

	' Token: 0x04002AF6 RID: 10998
	Private smallGemTimerUp As Boolean

	' Token: 0x04002AF7 RID: 10999
	Private bigGemTimerUp As Boolean

	' Token: 0x04002AF8 RID: 11000
	Private skullCounter As Integer

	' Token: 0x04002AF9 RID: 11001
	Private treasureCounter As Integer

	' Token: 0x04002AFA RID: 11002
	Private casketStartPos As Vector3

	' Token: 0x04002AFB RID: 11003
	Private patternCoroutine As Coroutine

	' Token: 0x04002AFC RID: 11004
	Private smallGemsRoutine As Coroutine

	' Token: 0x04002AFD RID: 11005
	Private bigGemsRoutine As Coroutine

	' Token: 0x04002AFE RID: 11006
	Private meditateP1 As FlyingGenieLevelMeditateFX

	' Token: 0x04002AFF RID: 11007
	Private meditateP2 As FlyingGenieLevelMeditateFX

	' Token: 0x04002B00 RID: 11008
	Private defaultColor As Color

	' Token: 0x04002B01 RID: 11009
	Private swordPinkPattern As String()

	' Token: 0x04002B02 RID: 11010
	Private swordPinkIndex As Integer

	' Token: 0x04002B03 RID: 11011
	Private gemPinkPattern As String()

	' Token: 0x04002B04 RID: 11012
	Private gemPinkIndex As Integer

	' Token: 0x04002B05 RID: 11013
	Private sphinxPinkPattern As String()

	' Token: 0x04002B06 RID: 11014
	Private sphinxPinkIndex As Integer

	' Token: 0x02000669 RID: 1641
	Public Enum State
		' Token: 0x04002B08 RID: 11016
		Intro
		' Token: 0x04002B09 RID: 11017
		Idle
		' Token: 0x04002B0A RID: 11018
		Transform
		' Token: 0x04002B0B RID: 11019
		Treasure
		' Token: 0x04002B0C RID: 11020
		Disappear
		' Token: 0x04002B0D RID: 11021
		Dead
		' Token: 0x04002B0E RID: 11022
		Coffin
	End Enum
End Class
