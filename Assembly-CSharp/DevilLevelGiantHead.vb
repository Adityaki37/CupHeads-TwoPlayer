Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x02000574 RID: 1396
Public Class DevilLevelGiantHead
	Inherits LevelProperties.Devil.Entity

	' Token: 0x06001A75 RID: 6773 RVA: 0x000F2144 File Offset: 0x000F0544
	Protected Overrides Sub Awake()
		MyBase.Awake()
		MyBase.animator.Play("Idle")
		MyBase.animator.Play("Idle_Body", 1)
		Me.state = DevilLevelGiantHead.State.Intro
		AddHandler Me.child.OnDamageTaken, AddressOf Me.OnDamageTaken
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		AddHandler Level.Current.OnWinEvent, AddressOf Me.Death
	End Sub

	' Token: 0x06001A76 RID: 6774 RVA: 0x000F21CF File Offset: 0x000F05CF
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		MyBase.properties.DealDamage(info.damage)
	End Sub

	' Token: 0x06001A77 RID: 6775 RVA: 0x000F21E2 File Offset: 0x000F05E2
	Public Sub StartIntroTransform()
		MyBase.StartCoroutine(Me.intro_cr())
	End Sub

	' Token: 0x06001A78 RID: 6776 RVA: 0x000F21F4 File Offset: 0x000F05F4
	Private Iterator Function intro_cr() As IEnumerator
		MyBase.GetComponent(Of Collider2D)().enabled = False
		Me.child.GetComponent(Of Collider2D)().enabled = False
		Me.platformCr = MyBase.StartCoroutine(Me.platforms_cr())
		MyBase.StartCoroutine(Me.fireballs_cr())
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		Me.state = DevilLevelGiantHead.State.Idle
		Me.waitingForTransform = False
		Return
	End Function

	' Token: 0x06001A79 RID: 6777 RVA: 0x000F220F File Offset: 0x000F060F
	Private Sub OnNeck()
		MyBase.animator.Play("Idle_Body")
	End Sub

	' Token: 0x06001A7A RID: 6778 RVA: 0x000F2221 File Offset: 0x000F0621
	Private Sub NoNeck()
		MyBase.animator.Play("Off_Body")
	End Sub

	' Token: 0x06001A7B RID: 6779 RVA: 0x000F2233 File Offset: 0x000F0633
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.fireballPrefab = Nothing
		Me.bombPrefab = Nothing
		Me.skullPrefab = Nothing
		Me.swooperPrefab = Nothing
		Me.tearPrefab = Nothing
	End Sub

	' Token: 0x06001A7C RID: 6780 RVA: 0x000F2260 File Offset: 0x000F0660
	Private Iterator Function platforms_cr() As IEnumerator
		Dim p As LevelProperties.Devil.GiantHeadPlatforms = MyBase.properties.CurrentState.giantHeadPlatforms
		Dim pattern As String() = p.riseString.Split(New Char() { ","c })
		Dim patternIndex As Integer = Global.UnityEngine.Random.Range(0, pattern.Length)
		While True
			While Me.waitingForTransform
				Yield Nothing
			End While
			p = MyBase.properties.CurrentState.giantHeadPlatforms
			patternIndex = (patternIndex + 1) Mod pattern.Length
			Dim platformIndex As Integer
			Parser.IntTryParse(pattern(patternIndex), platformIndex)
			Dim platform As DevilLevelPlatform = Me.raisablePlatforms(platformIndex - 1)
			If platform.state <> DevilLevelPlatform.State.Idle Then
				Dim noIdlePlatforms As Boolean = True
				While noIdlePlatforms
					For Each devilLevelPlatform As DevilLevelPlatform In Me.raisablePlatforms
						If devilLevelPlatform.state = DevilLevelPlatform.State.Idle Then
							noIdlePlatforms = False
						End If
					Next
					If noIdlePlatforms Then
						Yield CupheadTime.WaitForSeconds(Me, p.riseDelayRange.RandomFloat())
					End If
				End While
			Else
				platform.Raise(p.riseSpeed, p.maxHeight, p.holdDelay)
				Yield CupheadTime.WaitForSeconds(Me, p.riseDelayRange.RandomFloat())
			End If
		End While
		Return
	End Function

	' Token: 0x06001A7D RID: 6781 RVA: 0x000F227C File Offset: 0x000F067C
	Private Iterator Function fireballs_cr() As IEnumerator
		Dim fromRight As Boolean = Rand.Bool()
		Dim index As Integer = If((Not fromRight), 0, (Me.raisablePlatforms.Length - 1))
		Dim p As LevelProperties.Devil.Fireballs = MyBase.properties.CurrentState.fireballs
		Yield CupheadTime.WaitForSeconds(Me, p.initialDelay)
		While True
			p = MyBase.properties.CurrentState.fireballs
			Dim platform As DevilLevelPlatform = Me.raisablePlatforms(index)
			index = (If((Not fromRight), (index + 1), (index - 1)) + Me.raisablePlatforms.Length) Mod Me.raisablePlatforms.Length
			If platform.state = DevilLevelPlatform.State.Dead Then
				Yield Nothing
			Else
				Me.fireballPrefab.Create(platform.transform.position.x, p.fallSpeed, p.fallAcceleration, p.size / 200F)
				Yield CupheadTime.WaitForSeconds(Me, p.spawnDelay)
			End If
		End While
		Return
	End Function

	' Token: 0x06001A7E RID: 6782 RVA: 0x000F2297 File Offset: 0x000F0697
	Public Sub StartBombEye()
		Me.state = DevilLevelGiantHead.State.BombEye
		MyBase.StartCoroutine(Me.eye_cr(MyBase.properties.CurrentState.bombEye.hesitate.RandomFloat()))
	End Sub

	' Token: 0x06001A7F RID: 6783 RVA: 0x000F22C7 File Offset: 0x000F06C7
	Public Sub StartSkullEye()
		Me.state = DevilLevelGiantHead.State.SkullEye
		MyBase.StartCoroutine(Me.eye_cr(MyBase.properties.CurrentState.skullEye.hesitate.RandomFloat()))
	End Sub

	' Token: 0x06001A80 RID: 6784 RVA: 0x000F22F8 File Offset: 0x000F06F8
	Private Iterator Function eye_cr(hesitateTime As Single) As IEnumerator
		If Me.state = DevilLevelGiantHead.State.BombEye Then
			Me.bombOnLeft = Rand.Bool()
			Me.spawnPos = If((Not Me.bombOnLeft), Me.rightEyeRoot.position, Me.leftEyeRoot.position)
			MyBase.animator.SetTrigger("OnBomb")
			MyBase.animator.SetBool("BombLeft", Me.bombOnLeft)
		Else
			Me.spawnPos = Me.middleRoot.transform.position
			MyBase.animator.SetTrigger("OnSpiral")
		End If
		Yield CupheadTime.WaitForSeconds(Me, hesitateTime)
		Me.state = DevilLevelGiantHead.State.Idle
		Return
	End Function

	' Token: 0x06001A81 RID: 6785 RVA: 0x000F231A File Offset: 0x000F071A
	Private Sub SpawnBomb()
		Me.bombPrefab.Create(Me.spawnPos, MyBase.properties.CurrentState.bombEye, Me.bombOnLeft)
	End Sub

	' Token: 0x06001A82 RID: 6786 RVA: 0x000F2344 File Offset: 0x000F0744
	Private Sub Offset()
		If MyBase.GetComponent(Of SpriteRenderer)().flipX Then
			MyBase.transform.AddPosition(-60F, 0F, 0F)
		Else
			MyBase.transform.AddPosition(60F, 0F, 0F)
		End If
	End Sub

	' Token: 0x06001A83 RID: 6787 RVA: 0x000F239A File Offset: 0x000F079A
	Private Sub SpawnSpiral()
		Me.skullPrefab.Create(Me.spawnPos, MyBase.properties.CurrentState.skullEye)
	End Sub

	' Token: 0x06001A84 RID: 6788 RVA: 0x000F23BE File Offset: 0x000F07BE
	Public Sub StartHands()
		MyBase.animator.SetTrigger("OnTransA")
		Me.handsCr = MyBase.StartCoroutine(Me.hands_cr())
	End Sub

	' Token: 0x06001A85 RID: 6789 RVA: 0x000F23E4 File Offset: 0x000F07E4
	Private Iterator Function hands_cr() As IEnumerator
		Me.waitingForTransform = True
		While Me.state <> DevilLevelGiantHead.State.Idle
			Yield Nothing
		End While
		Dim platformsDown As Boolean = False
		While Not platformsDown
			platformsDown = True
			For Each devilLevelPlatform As DevilLevelPlatform In Me.raisablePlatforms
				If devilLevelPlatform.state = DevilLevelPlatform.State.Raising Then
					platformsDown = False
				End If
			Next
			Yield Nothing
		End While
		Me.waitingForTransform = False
		For Each devilLevelPlatform2 As DevilLevelPlatform In Me.HandsPhaseExit
			devilLevelPlatform2.Lower(MyBase.properties.CurrentState.giantHeadPlatforms.exitSpeed)
		Next
		Me.StartSwoopers()
		Dim leftHandShoot As Boolean = Rand.Bool()
		Me.hands(0).StartPattern(MyBase.properties.CurrentState.hands)
		Me.hands(1).StartPattern(MyBase.properties.CurrentState.hands)
		Me.handsSpawnCr = MyBase.StartCoroutine(Me.spawn_hand_cr())
		While True
			Dim handIndex As Integer = If((Not leftHandShoot), 1, 0)
			If Me.hands(handIndex) IsNot Nothing Then
				Me.hands(handIndex).animator.SetTrigger("OnAttack")
			End If
			leftHandShoot = Not leftHandShoot
			Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.hands.shotDelay.RandomFloat())
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06001A86 RID: 6790 RVA: 0x000F2400 File Offset: 0x000F0800
	Private Iterator Function spawn_hand_cr() As IEnumerator
		Dim p As LevelProperties.Devil.Hands = MyBase.properties.CurrentState.hands
		Yield CupheadTime.WaitForSeconds(Me, p.initialSpawnDelay.RandomFloat())
		Me.hands(0).SpawnIn()
		Yield CupheadTime.WaitForSeconds(Me, p.initialSpawnDelay.RandomFloat())
		Me.hands(1).SpawnIn()
		While Not Me.hands(0).isDead
			While Not Me.hands(0).despawned AndAlso Not Me.hands(1).despawned
				Yield Nothing
			End While
			Yield CupheadTime.WaitForSeconds(Me, p.spawnDelayRange.RandomFloat())
			If Me.hands(0).despawned Then
				Me.hands(0).SpawnIn()
			ElseIf Me.hands(1).despawned Then
				Me.hands(1).SpawnIn()
			End If
			Yield Nothing
		End While
		Yield Nothing
		Return
	End Function

	' Token: 0x06001A87 RID: 6791 RVA: 0x000F241B File Offset: 0x000F081B
	Public Sub StartSwoopers()
		Me.swooperSpawnCr = MyBase.StartCoroutine(Me.swooper_spawn_cr())
		Me.swooperSwoopCr = MyBase.StartCoroutine(Me.swooper_swoop_cr())
	End Sub

	' Token: 0x06001A88 RID: 6792 RVA: 0x000F2444 File Offset: 0x000F0844
	Private Iterator Function swooper_spawn_cr() As IEnumerator
		Dim p As LevelProperties.Devil.Swoopers = MyBase.properties.CurrentState.swoopers
		Dim swooperSlotPositions As String() = p.positions.Split(New Char() { ","c })
		Me.swoopers = New List(Of DevilLevelSwooper)()
		Me.swooperSlots = New DevilLevelGiantHead.SwooperSlot(swooperSlotPositions.Length - 1) {}
		For i As Integer = 0 To Me.swooperSlots.Length - 1
			Dim num As Single = 0F
			Parser.FloatTryParse(swooperSlotPositions(i), num)
			Me.swooperSlots(i) = New DevilLevelGiantHead.SwooperSlot(num - 600F)
		Next
		Dim swooperSlotIndex As Integer = Global.UnityEngine.Random.Range(0, Me.swooperSlots.Length)
		Dim delay As Single = p.initialSpawnDelay.RandomFloat()
		Dim spawnPoint As Integer = Global.UnityEngine.Random.Range(0, Me.spawnPoints.Length)
		While True
			Yield CupheadTime.WaitForSeconds(Me, delay)
			delay = p.spawnDelay.RandomFloat()
			If Me.swoopers.Count < p.maxCount Then
				Dim numToSpawn As Integer = p.spawnCount.RandomInt()
				Dim numSpawned As Integer = 0
				MyBase.animator.SetBool("IsWhincing", True)
				While Not MyBase.animator.GetCurrentAnimatorStateInfo(0).IsName("Whince")
					Yield Nothing
				End While
				While numSpawned < numToSpawn AndAlso Me.swoopers.Count < p.maxCount
					swooperSlotIndex = (swooperSlotIndex + 1) Mod Me.swooperSlots.Length
					Dim slot As DevilLevelGiantHead.SwooperSlot = Me.swooperSlots(swooperSlotIndex)
					If slot.swooper Is Nothing Then
						Dim devilLevelSwooper As DevilLevelSwooper = Me.swooperPrefab.Create(Me, p, Me.spawnPoints(spawnPoint).position, slot.xPos)
						slot.swooper = devilLevelSwooper
						Me.swoopers.Add(devilLevelSwooper)
						numSpawned += 1
					End If
					spawnPoint = (spawnPoint + 1) Mod Me.spawnPoints.Length
					Yield CupheadTime.WaitForSeconds(Me, 0.4F)
				End While
			End If
			Yield CupheadTime.WaitForSeconds(Me, 1.5F)
			MyBase.animator.SetBool("IsWhincing", False)
		End While
		Return
	End Function

	' Token: 0x06001A89 RID: 6793 RVA: 0x000F2460 File Offset: 0x000F0860
	Private Iterator Function swooper_swoop_cr() As IEnumerator
		Dim p As LevelProperties.Devil.Swoopers = MyBase.properties.CurrentState.swoopers
		While True
			While Me.swoopers.Count = 0
				Yield Nothing
			End While
			Dim attackSwoopers As List(Of DevilLevelSwooper) = New List(Of DevilLevelSwooper)(Me.swoopers)
			attackSwoopers.Shuffle()
			Yield CupheadTime.WaitForSeconds(Me, p.attackDelay.RandomFloat())
			For Each swooper As DevilLevelSwooper In attackSwoopers
				If swooper IsNot Nothing AndAlso swooper.state = DevilLevelSwooper.State.Idle Then
					swooper.Swoop()
					If swooper Is attackSwoopers(attackSwoopers.Count - 1) Then
						swooper.finalSwooping = True
					End If
					Me.RemoveSwooperFromSlot(swooper)
					If swooper IsNot attackSwoopers(attackSwoopers.Count - 1) Then
						Yield CupheadTime.WaitForSeconds(Me, p.attackDelay.RandomFloat())
					End If
				End If
			Next
		End While
		Return
	End Function

	' Token: 0x06001A8A RID: 6794 RVA: 0x000F247B File Offset: 0x000F087B
	Public Sub OnSwooperDeath(swooper As DevilLevelSwooper)
		Me.swoopers.Remove(swooper)
		Me.RemoveSwooperFromSlot(swooper)
	End Sub

	' Token: 0x06001A8B RID: 6795 RVA: 0x000F2494 File Offset: 0x000F0894
	Private Sub RemoveSwooperFromSlot(swooper As DevilLevelSwooper)
		For i As Integer = 0 To Me.swooperSlots.Length - 1
			If Me.swooperSlots(i).swooper Is swooper Then
				Me.swooperSlots(i).swooper = Nothing
			End If
		Next
	End Sub

	' Token: 0x06001A8C RID: 6796 RVA: 0x000F24E8 File Offset: 0x000F08E8
	Public Function PutSwooperInSlot(swooper As DevilLevelSwooper) As Single
		Dim num As Single = Single.MaxValue
		Dim num2 As Integer = 0
		For i As Integer = 0 To Me.swooperSlots.Length - 1
			If Not(Me.swooperSlots(i).swooper IsNot Nothing) Then
				Dim num3 As Single = Mathf.Abs(Me.swooperSlots(i).xPos - swooper.transform.position.x)
				If num3 < num Then
					num = num3
					num2 = i
				End If
			End If
		Next
		Me.swooperSlots(num2).swooper = swooper
		Return Me.swooperSlots(num2).xPos
	End Function

	' Token: 0x06001A8D RID: 6797 RVA: 0x000F2591 File Offset: 0x000F0991
	Public Sub StartTears()
		MyBase.StartCoroutine(Me.tears_cr())
	End Sub

	' Token: 0x06001A8E RID: 6798 RVA: 0x000F25A0 File Offset: 0x000F09A0
	Private Iterator Function tears_cr() As IEnumerator
		MyBase.animator.SetTrigger("OnTransB")
		Me.waitingForTransform = True
		While Me.state <> DevilLevelGiantHead.State.Idle
			Yield Nothing
		End While
		Dim platformsDown As Boolean = False
		While Not platformsDown
			platformsDown = True
			For Each devilLevelPlatform As DevilLevelPlatform In Me.raisablePlatforms
				If devilLevelPlatform.state = DevilLevelPlatform.State.Raising Then
					platformsDown = False
				End If
			Next
			Yield Nothing
		End While
		Me.waitingForTransform = False
		For Each devilLevelPlatform2 As DevilLevelPlatform In Me.TearsPhaseExit
			devilLevelPlatform2.Lower(MyBase.properties.CurrentState.giantHeadPlatforms.exitSpeed)
		Next
		If Not MyBase.properties.CurrentState.giantHeadPlatforms.riseDuringTearPhase Then
			MyBase.StopCoroutine(Me.platformCr)
		End If
		MyBase.StopCoroutine(Me.handsCr)
		MyBase.StopCoroutine(Me.handsSpawnCr)
		MyBase.StopCoroutine(Me.swooperSpawnCr)
		MyBase.StopCoroutine(Me.swooperSwoopCr)
		While Me.swoopers.Count > 0
			Me.swoopers(0).Die()
		End While
		For Each devilLevelHand As DevilLevelHand In Me.hands
			devilLevelHand.isDead = True
			devilLevelHand.Die()
		Next
		Dim spawnLeft As Boolean = True
		Yield CupheadTime.WaitForSeconds(Me, 2F)
		While True
			Me.tearPrefab.CreateTear(If((Not spawnLeft), Me.rightTearRoot.transform.position, Me.leftTearRoot.transform.position), MyBase.properties.CurrentState.tears.speed)
			Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.tears.delay)
			spawnLeft = Not spawnLeft
		End While
		Return
	End Function

	' Token: 0x06001A8F RID: 6799 RVA: 0x000F25BB File Offset: 0x000F09BB
	Private Sub Death()
		MyBase.GetComponent(Of Collider2D)().enabled = False
		Me.StopAllCoroutines()
		MyBase.animator.SetTrigger("OnDead")
	End Sub

	' Token: 0x06001A90 RID: 6800 RVA: 0x000F25DF File Offset: 0x000F09DF
	Private Sub sfx_p3_bomb_appear()
		AudioManager.Play("p3_bomb_appear")
		Me.emitAudioFromObject.Add("p3_bomb_appear")
	End Sub

	' Token: 0x06001A91 RID: 6801 RVA: 0x000F25FB File Offset: 0x000F09FB
	Private Sub sfx_p3_bomb_attack()
		AudioManager.Play("p3_bomb_attack")
		Me.emitAudioFromObject.Add("p3_bomb_attack")
	End Sub

	' Token: 0x06001A92 RID: 6802 RVA: 0x000F2617 File Offset: 0x000F0A17
	Private Sub sfx_p3_cry_idle()
		AudioManager.Play("p3_cry_idle")
		Me.emitAudioFromObject.Add("p3_cry_idle")
	End Sub

	' Token: 0x06001A93 RID: 6803 RVA: 0x000F2633 File Offset: 0x000F0A33
	Private Sub sfx_p3_dead_loop()
		If Not Me.DeadLoopSFXActive Then
			AudioManager.PlayLoop("p3_dead_loop")
			Me.emitAudioFromObject.Add("p3_dead_loop")
			Me.DeadLoopSFXActive = True
		End If
	End Sub

	' Token: 0x06001A94 RID: 6804 RVA: 0x000F2661 File Offset: 0x000F0A61
	Private Sub sfx_p3_dead_loop_stop()
		AudioManager.[Stop]("p3_dead_loop")
		Me.DeadLoopSFXActive = False
	End Sub

	' Token: 0x06001A95 RID: 6805 RVA: 0x000F2674 File Offset: 0x000F0A74
	Private Sub sfx_p3_hand_release_start()
		AudioManager.Play("p3_hand_release_start")
		Me.emitAudioFromObject.Add("p3_hand_release_start")
	End Sub

	' Token: 0x06001A96 RID: 6806 RVA: 0x000F2690 File Offset: 0x000F0A90
	Private Sub sfx_p3_hurt_trans_a()
		AudioManager.Play("p3_hurt_trans_a")
		Me.emitAudioFromObject.Add("p3_hurt_trans_a")
	End Sub

	' Token: 0x06001A97 RID: 6807 RVA: 0x000F26AC File Offset: 0x000F0AAC
	Private Sub sfx_p3_spiral_attack()
		AudioManager.Play("p3_spiral_attack")
		Me.emitAudioFromObject.Add("p3_spiral_attack")
	End Sub

	' Token: 0x06001A98 RID: 6808 RVA: 0x000F26C8 File Offset: 0x000F0AC8
	Private Sub sfx_p3_intro_end()
		AudioManager.Play("p3_intro_end")
		Me.emitAudioFromObject.Add("p3_intro_end")
	End Sub

	' Token: 0x0400239F RID: 9119
	Public state As DevilLevelGiantHead.State

	' Token: 0x040023A0 RID: 9120
	<SerializeField()>
	Private groundPieces As GameObject()

	' Token: 0x040023A1 RID: 9121
	<SerializeField()>
	Private HandsPhaseExit As DevilLevelPlatform()

	' Token: 0x040023A2 RID: 9122
	<SerializeField()>
	Private TearsPhaseExit As DevilLevelPlatform()

	' Token: 0x040023A3 RID: 9123
	<SerializeField()>
	Private raisablePlatforms As DevilLevelPlatform()

	' Token: 0x040023A4 RID: 9124
	<SerializeField()>
	Private stage3Platforms As Transform

	' Token: 0x040023A5 RID: 9125
	<SerializeField()>
	Private fireballPrefab As DevilLevelFireball

	' Token: 0x040023A6 RID: 9126
	<SerializeField()>
	Private bombPrefab As DevilLevelBomb

	' Token: 0x040023A7 RID: 9127
	<SerializeField()>
	Private skullPrefab As DevilLevelSkull

	' Token: 0x040023A8 RID: 9128
	<SerializeField()>
	Private leftEyeRoot As Transform

	' Token: 0x040023A9 RID: 9129
	<SerializeField()>
	Private rightEyeRoot As Transform

	' Token: 0x040023AA RID: 9130
	<SerializeField()>
	Private middleRoot As Transform

	' Token: 0x040023AB RID: 9131
	<SerializeField()>
	Private leftTearRoot As Transform

	' Token: 0x040023AC RID: 9132
	<SerializeField()>
	Private rightTearRoot As Transform

	' Token: 0x040023AD RID: 9133
	<SerializeField()>
	Private hands As DevilLevelHand()

	' Token: 0x040023AE RID: 9134
	<SerializeField()>
	Private swooperPrefab As DevilLevelSwooper

	' Token: 0x040023AF RID: 9135
	<SerializeField()>
	Private tearPrefab As DevilLevelTear

	' Token: 0x040023B0 RID: 9136
	<SerializeField()>
	Private bottomSprite As SpriteRenderer

	' Token: 0x040023B1 RID: 9137
	<SerializeField()>
	Private child As DamageReceiver

	' Token: 0x040023B2 RID: 9138
	<SerializeField()>
	Private spawnPoints As Transform()

	' Token: 0x040023B3 RID: 9139
	Private waitingForTransform As Boolean

	' Token: 0x040023B4 RID: 9140
	Private bombOnLeft As Boolean

	' Token: 0x040023B5 RID: 9141
	Private DeadLoopSFXActive As Boolean

	' Token: 0x040023B6 RID: 9142
	Private damageReceiver As DamageReceiver

	' Token: 0x040023B7 RID: 9143
	Private platformCr As Coroutine

	' Token: 0x040023B8 RID: 9144
	Private handsCr As Coroutine

	' Token: 0x040023B9 RID: 9145
	Private handsSpawnCr As Coroutine

	' Token: 0x040023BA RID: 9146
	Private swooperSpawnCr As Coroutine

	' Token: 0x040023BB RID: 9147
	Private swooperSwoopCr As Coroutine

	' Token: 0x040023BC RID: 9148
	Private spawnPos As Vector2

	' Token: 0x040023BD RID: 9149
	Private color As Color

	' Token: 0x040023BE RID: 9150
	Private swooperSlots As DevilLevelGiantHead.SwooperSlot()

	' Token: 0x040023BF RID: 9151
	Private swoopers As List(Of DevilLevelSwooper)

	' Token: 0x02000575 RID: 1397
	Public Enum State
		' Token: 0x040023C1 RID: 9153
		Intro
		' Token: 0x040023C2 RID: 9154
		Idle
		' Token: 0x040023C3 RID: 9155
		BombEye
		' Token: 0x040023C4 RID: 9156
		SkullEye
	End Enum

	' Token: 0x02000576 RID: 1398
	Private Structure SwooperSlot
		' Token: 0x06001A99 RID: 6809 RVA: 0x000F26E4 File Offset: 0x000F0AE4
		Public Sub New(xPos As Single)
			Me.xPos = xPos
			Me.swooper = Nothing
		End Sub

		' Token: 0x040023C5 RID: 9157
		Public xPos As Single

		' Token: 0x040023C6 RID: 9158
		Public swooper As DevilLevelSwooper
	End Structure
End Class
