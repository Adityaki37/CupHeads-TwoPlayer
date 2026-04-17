Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000692 RID: 1682
Public Class FlyingMermaidLevelMermaid
	Inherits LevelProperties.FlyingMermaid.Entity

	' Token: 0x06002390 RID: 9104 RVA: 0x0014DCDC File Offset: 0x0014C0DC
	Public Sub New()
		Dim array As FlyingMermaidLevelMermaid.FishPossibility() = New FlyingMermaidLevelMermaid.FishPossibility(1) {}
		array(0) = FlyingMermaidLevelMermaid.FishPossibility.Homer
		Me.fishPattern = array
		Me.maxBlinks = 3
		MyBase..ctor()
	End Sub

	' Token: 0x170003A3 RID: 931
	' (get) Token: 0x06002391 RID: 9105 RVA: 0x0014DD0F File Offset: 0x0014C10F
	' (set) Token: 0x06002392 RID: 9106 RVA: 0x0014DD17 File Offset: 0x0014C117
	Public Property state As FlyingMermaidLevelMermaid.State

	' Token: 0x06002393 RID: 9107 RVA: 0x0014DD20 File Offset: 0x0014C120
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.summonPattern.Shuffle()
		Me.fishPattern.Shuffle()
		MyBase.StartCoroutine(Me.intro_cr())
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		Dim collisionChild As CollisionChild = Me.blockingColliders.gameObject.AddComponent(Of CollisionChild)()
		AddHandler collisionChild.OnPlayerCollision, AddressOf Me.OnCollisionPlayer
	End Sub

	' Token: 0x06002394 RID: 9108 RVA: 0x0014DDA8 File Offset: 0x0014C1A8
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		MyBase.properties.DealDamage(info.damage)
	End Sub

	' Token: 0x06002395 RID: 9109 RVA: 0x0014DDBC File Offset: 0x0014C1BC
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
		If Not Me.stopMoving Then
			If Me.introEnded AndAlso Not Me.transformationStarting Then
				Dim num As Single = Mathf.Max(PlayerManager.GetNext().center.x, PlayerManager.GetNext().center.x)
				If num > MyBase.transform.position.x Then
					Me.Position(True)
				Else
					Me.Position(False)
				End If
			ElseIf Me.transformationStarting Then
				Me.Position(False)
			End If
		End If
	End Sub

	' Token: 0x06002396 RID: 9110 RVA: 0x0014DE6D File Offset: 0x0014C26D
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06002397 RID: 9111 RVA: 0x0014DE8C File Offset: 0x0014C28C
	Public Overrides Sub LevelInit(properties As LevelProperties.FlyingMermaid)
		MyBase.LevelInit(properties)
		Me.initFishPatternIndices()
		Me.spreadFishPinkPattern = properties.CurrentState.spreadshotFish.spreadshotPinkString.Split(New Char() { ","c })
		Me.spreadFishPinkIndex = Global.UnityEngine.Random.Range(0, Me.spreadFishPinkPattern.Length)
	End Sub

	' Token: 0x06002398 RID: 9112 RVA: 0x0014DEE0 File Offset: 0x0014C2E0
	Private Sub Position(closeGap As Boolean)
		Me.walkDuration = CSng(If((Not Me.transformationStarting), 4, 2))
		If closeGap Then
			Dim num As Single = Me.walkingPositions(0).position.x
			Dim num2 As Single = Me.walkingPositions(1).position.x
			Me.Move(num, num2, Me.walkDuration, 1)
		Else
			Dim num As Single = Me.walkingPositions(1).position.x
			Dim num2 As Single = Me.walkingPositions(0).position.x
			Me.Move(num, num2, Me.walkDuration, -1)
		End If
	End Sub

	' Token: 0x06002399 RID: 9113 RVA: 0x0014DF8C File Offset: 0x0014C38C
	Private Sub Move(startPosition As Single, endPosition As Single, duration As Single, direction As Integer)
		Me.walkTime += CupheadTime.Delta * CSng(direction)
		If direction < 0 Then
			If Me.walkTime <= 0F Then
				Me.walkTime = 0F
			End If
		ElseIf Me.walkTime >= duration Then
			Me.walkTime = duration
		End If
		Me.walkPCT = Me.walkTime / duration
		If Me.walkPCT >= 1F Then
			Me.walkPCT = 1F
		End If
		If direction < 0 Then
			Me.walkPCT = 1F - Me.walkPCT
		End If
		MyBase.transform.SetPosition(New Single?(startPosition + (endPosition - startPosition) * Me.walkPCT), Nothing, Nothing)
	End Sub

	' Token: 0x0600239A RID: 9114 RVA: 0x0014E060 File Offset: 0x0014C460
	Private Sub PlayIntroSound()
		AudioManager.Play("level_mermaid_intro")
		Me.emitAudioFromObject.Add("level_mermaid_intro")
	End Sub

	' Token: 0x0600239B RID: 9115 RVA: 0x0014E07C File Offset: 0x0014C47C
	Private Iterator Function intro_cr() As IEnumerator
		Dim t As Single = 0F
		MyBase.transform.SetPosition(Nothing, New Single?(Me.startUnderwaterY), Nothing)
		Yield CupheadTime.WaitForSeconds(Me, Me.introRiseTime * 0.5F)
		MyBase.StartCoroutine(Me.spawn_splash_cr())
		While t < Me.introRiseTime * 0.5F
			t += CupheadTime.Delta
			MyBase.transform.SetPosition(Nothing, New Single?(Mathf.Lerp(Me.startUnderwaterY, Me.regularY, t / (Me.introRiseTime * 0.5F))), Nothing)
			Yield Nothing
		End While
		MyBase.transform.SetPosition(Nothing, New Single?(Me.regularY), Nothing)
		While Not Me.introEnded
			Yield Nothing
		End While
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		Me.state = FlyingMermaidLevelMermaid.State.Idle
		Return
	End Function

	' Token: 0x0600239C RID: 9116 RVA: 0x0014E098 File Offset: 0x0014C498
	Private Iterator Function spawn_splash_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 0.35F)
		FlyingMermaidLevelSplashManager.Instance.SpawnMegaSplashMedium(MyBase.gameObject, -50F, True, -200F)
		Yield Nothing
		Return
	End Function

	' Token: 0x0600239D RID: 9117 RVA: 0x0014E0B4 File Offset: 0x0014C4B4
	Public Sub IntroContinue()
		Dim component As Animator = MyBase.GetComponent(Of Animator)()
		component.SetTrigger("Continue")
		Me.state = FlyingMermaidLevelMermaid.State.Intro
	End Sub

	' Token: 0x0600239E RID: 9118 RVA: 0x0014E0DA File Offset: 0x0014C4DA
	Private Sub OnIntroAnimComplete()
		Me.introEnded = True
	End Sub

	' Token: 0x0600239F RID: 9119 RVA: 0x0014E0E4 File Offset: 0x0014C4E4
	Private Sub BlinkMaybe()
		Me.blinks += 1
		If Me.blinks >= Me.maxBlinks Then
			Me.blinks = 0
			Me.maxBlinks = Global.UnityEngine.Random.Range(2, 5)
			Me.blinkOverlaySprite.enabled = True
		Else
			Me.blinkOverlaySprite.enabled = False
		End If
	End Sub

	' Token: 0x060023A0 RID: 9120 RVA: 0x0014E141 File Offset: 0x0014C541
	Public Sub StartYell()
		Me.state = FlyingMermaidLevelMermaid.State.Yell
		MyBase.StartCoroutine(Me.yell_cr())
	End Sub

	' Token: 0x060023A1 RID: 9121 RVA: 0x0014E158 File Offset: 0x0014C558
	Private Iterator Function yell_cr() As IEnumerator
		Dim p As LevelProperties.FlyingMermaid.Yell = MyBase.properties.CurrentState.yell
		Dim pattern As String() = p.patternString.GetRandom().Split(New Char() { ","c })
		MyBase.animator.SetTrigger("StartYell")
		MyBase.animator.SetBool("Repeat", True)
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Yell_Start", False, True)
		Dim waitTime As Single = p.anticipateInitialHold
		For i As Integer = 0 To pattern.Length - 1
			If pattern(i)(0) = "D"c Then
				Parser.FloatTryParse(pattern(i).Substring(1), waitTime)
			Else
				Dim repeatTimes As Integer = 0
				Parser.IntTryParse(pattern(i).Substring(1), repeatTimes)
				For j As Integer = 0 To repeatTimes - 1
					Yield CupheadTime.WaitForSeconds(Me, waitTime)
					MyBase.animator.SetTrigger("Continue")
					Yield MyBase.animator.WaitForAnimationToEnd(Me, "Yell_Anticipation_End", False, True)
					Me.FireProjectiles()
					Me.yellEffect.Create(Me.yellFxRoot.position)
					Yield CupheadTime.WaitForSeconds(Me, p.mouthHold)
					MyBase.animator.SetTrigger("Continue")
					waitTime = p.anticipateHold
					If i < pattern.Length - 1 OrElse j < repeatTimes - 1 Then
						Yield MyBase.animator.WaitForAnimationToEnd(Me, "Yell_Back", False, True)
					End If
				Next
			End If
		Next
		MyBase.animator.SetBool("Repeat", False)
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Yell_End", False, True)
		Yield CupheadTime.WaitForSeconds(Me, p.hesitateAfterAttack)
		Me.state = FlyingMermaidLevelMermaid.State.Idle
		Return
	End Function

	' Token: 0x060023A2 RID: 9122 RVA: 0x0014E174 File Offset: 0x0014C574
	Private Sub FireProjectiles()
		Dim yell As LevelProperties.FlyingMermaid.Yell = MyBase.properties.CurrentState.yell
		Dim [next] As AbstractPlayerController = PlayerManager.GetNext()
		For i As Integer = 0 To yell.numBullets - 1
			Dim floatAt As Single = yell.spreadAngle.GetFloatAt(CSng(i) / (CSng(yell.numBullets) - 1F))
			Dim flyingMermaidLevelYellProjectile As FlyingMermaidLevelYellProjectile = Me.yellProjectilePrefab.Create(Me.projectileRoot.position, yell.bulletSpeed, floatAt, [next])
			flyingMermaidLevelYellProjectile.animator.SetInteger("Variant", i)
		Next
	End Sub

	' Token: 0x060023A3 RID: 9123 RVA: 0x0014E202 File Offset: 0x0014C602
	Public Sub StartSummon()
		Me.state = FlyingMermaidLevelMermaid.State.Summon
		MyBase.StartCoroutine(Me.summon_cr())
	End Sub

	' Token: 0x060023A4 RID: 9124 RVA: 0x0014E218 File Offset: 0x0014C618
	Private Iterator Function summon_cr() As IEnumerator
		Dim p As LevelProperties.FlyingMermaid.Summon = MyBase.properties.CurrentState.summon
		MyBase.animator.SetBool("Summon", True)
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Summon_Start", False, True)
		AudioManager.Play("level_mermaid_summon_loop_start")
		Yield CupheadTime.WaitForSeconds(Me, p.holdBeforeCreature)
		Dim summon As FlyingMermaidLevelMermaid.SummonPossibility = Me.nextSummon()
		AudioManager.Play("level_mermaid_summon_loop")
		If summon <> FlyingMermaidLevelMermaid.SummonPossibility.Seahorse Then
			If summon <> FlyingMermaidLevelMermaid.SummonPossibility.Pufferfish Then
				If summon = FlyingMermaidLevelMermaid.SummonPossibility.Turtle Then
					Me.SummonTurtle()
				End If
			Else
				AudioManager.Play("level_mermaid_merdusa_puffer_fish_bubble_up")
				MyBase.StartCoroutine(Me.summonPufferFish_cr())
			End If
		Else
			Me.SummonSeahorse()
		End If
		Yield CupheadTime.WaitForSeconds(Me, p.holdAfterCreature)
		AudioManager.[Stop]("level_mermaid_summon_loop")
		AudioManager.Play("level_mermaid_summon_loop_end")
		MyBase.animator.SetBool("Summon", False)
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Summon_End", False, True)
		Yield CupheadTime.WaitForSeconds(Me, p.hesitateAfterAttack)
		Me.state = FlyingMermaidLevelMermaid.State.Idle
		Return
	End Function

	' Token: 0x060023A5 RID: 9125 RVA: 0x0014E233 File Offset: 0x0014C633
	Private Function nextSummon() As FlyingMermaidLevelMermaid.SummonPossibility
		Me.summonIndex = (Me.summonIndex + 1) Mod Me.summonPattern.Length
		Return Me.summonPattern(Me.summonIndex)
	End Function

	' Token: 0x060023A6 RID: 9126 RVA: 0x0014E25C File Offset: 0x0014C65C
	Private Iterator Function summonPufferFish_cr() As IEnumerator
		Dim p As LevelProperties.FlyingMermaid.Pufferfish = MyBase.properties.CurrentState.pufferfish
		Dim pattern As String() = p.spawnString.GetRandom().Split(New Char() { ","c })
		Dim i As Integer = Global.UnityEngine.Random.Range(0, pattern.Length)
		Dim t As Single = 0F
		Dim waitTime As Single = 0F
		Dim spawnsUntilPinkPufferfish As Integer = p.pinkPufferSpawnRange.RandomInt()
		While t < p.spawnDuration AndAlso Not Me.stopPufferfish
			If pattern(i)(0) = "D"c Then
				Parser.FloatTryParse(pattern(i).Substring(1), waitTime)
			Else
				If waitTime > 0F Then
					Yield CupheadTime.WaitForSeconds(Me, waitTime)
					t += waitTime
				End If
				Dim spawnLocations As String() = pattern(i).Split(New Char() { "-"c })
				For Each text As String In spawnLocations
					Dim num As Single = 0F
					Parser.FloatTryParse(text, num)
					spawnsUntilPinkPufferfish -= 1
					Dim flyingMermaidLevelPufferfish As FlyingMermaidLevelPufferfish
					If spawnsUntilPinkPufferfish = 0 Then
						spawnsUntilPinkPufferfish = p.pinkPufferSpawnRange.RandomInt()
						flyingMermaidLevelPufferfish = Me.pinkPufferfishPrefab
					Else
						flyingMermaidLevelPufferfish = Me.pufferfishPrefabs(Global.UnityEngine.Random.Range(0, Me.pufferfishPrefabs.Length))
					End If
					MyBase.StartCoroutine(Me.summon_pufferfish_cr(flyingMermaidLevelPufferfish, num))
				Next
				waitTime = p.delay
			End If
			i = (i + 1) Mod pattern.Length
		End While
		Return
	End Function

	' Token: 0x060023A7 RID: 9127 RVA: 0x0014E278 File Offset: 0x0014C678
	Private Sub SummonSeahorse()
		Dim [next] As AbstractPlayerController = PlayerManager.GetNext()
		Dim flyingMermaidLevelSeahorse As FlyingMermaidLevelSeahorse = Global.UnityEngine.[Object].Instantiate(Of FlyingMermaidLevelSeahorse)(Me.seahorsePrefab)
		Dim vector As Vector2 = flyingMermaidLevelSeahorse.transform.position
		vector.x = [next].transform.position.x
		flyingMermaidLevelSeahorse.transform.position = vector
		flyingMermaidLevelSeahorse.Init(MyBase.properties.CurrentState.seahorse)
		Dim component As GroundHomingMovement = flyingMermaidLevelSeahorse.GetComponent(Of GroundHomingMovement)()
		component.TrackingPlayer = [next]
	End Sub

	' Token: 0x060023A8 RID: 9128 RVA: 0x0014E2FC File Offset: 0x0014C6FC
	Private Sub SummonTurtle()
		Dim flyingMermaidLevelTurtle As FlyingMermaidLevelTurtle = Global.UnityEngine.[Object].Instantiate(Of FlyingMermaidLevelTurtle)(Me.turtlePrefab)
		Dim vector As Vector2 = flyingMermaidLevelTurtle.transform.position
		vector.x = CSng(Level.Current.Left) + MyBase.properties.CurrentState.turtle.appearPosition.RandomFloat()
		flyingMermaidLevelTurtle.transform.position = vector
		flyingMermaidLevelTurtle.Init(MyBase.properties.CurrentState.turtle)
	End Sub

	' Token: 0x060023A9 RID: 9129 RVA: 0x0014E37C File Offset: 0x0014C77C
	Private Iterator Function summon_pufferfish_cr(prefab As FlyingMermaidLevelPufferfish, x As Single) As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, Global.UnityEngine.Random.Range(0F, 0.15F))
		Dim pufferfish As FlyingMermaidLevelPufferfish = Global.UnityEngine.[Object].Instantiate(Of FlyingMermaidLevelPufferfish)(prefab)
		Dim position As Vector2 = pufferfish.transform.position
		position.x = x + CSng(Level.Current.Left)
		pufferfish.transform.position = position
		pufferfish.Init(MyBase.properties.CurrentState.pufferfish)
		Return
	End Function

	' Token: 0x060023AA RID: 9130 RVA: 0x0014E3A5 File Offset: 0x0014C7A5
	Public Sub StartFish()
		MyBase.StartCoroutine(Me.fish_cr())
	End Sub

	' Token: 0x060023AB RID: 9131 RVA: 0x0014E3B4 File Offset: 0x0014C7B4
	Private Sub PlayMermaidTuckdownSound()
		AudioManager.Play("level_mermaid_tuckdown_laugh")
		Me.emitAudioFromObject.Add("level_mermaid_tuckdown_laugh")
	End Sub

	' Token: 0x060023AC RID: 9132 RVA: 0x0014E3D0 File Offset: 0x0014C7D0
	Private Iterator Function fish_cr() As IEnumerator
		Me.state = FlyingMermaidLevelMermaid.State.Fish
		MyBase.animator.SetTrigger("StartFish")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Tuckdown_Start", False, True)
		Dim t As Single = 0F
		FlyingMermaidLevelSplashManager.Instance.SpawnMegaSplashLarge(MyBase.gameObject, 0F, False, 0F)
		While t < Me.tuckdownMoveTime
			t += CupheadTime.Delta
			MyBase.transform.SetPosition(Nothing, New Single?(Mathf.Lerp(Me.regularY, Me.fishUnderwaterY, t / Me.tuckdownMoveTime)), Nothing)
			Yield Nothing
		End While
		Yield CupheadTime.WaitForSeconds(Me, Me.tuckdownWaitTime)
		Me.fish = Me.nextFish()
		Me.spreadshotFishSprite.enabled = Me.fish = FlyingMermaidLevelMermaid.FishPossibility.Spreadshot
		Me.spreadshotFishOverlaySprite.enabled = Me.fish = FlyingMermaidLevelMermaid.FishPossibility.Spreadshot
		Me.spinnerFishSprite.enabled = Me.fish = FlyingMermaidLevelMermaid.FishPossibility.Spinner
		Me.spinnerFishOverlaySprite.enabled = Me.fish = FlyingMermaidLevelMermaid.FishPossibility.Spinner
		Me.homerFishSprite.enabled = Me.fish = FlyingMermaidLevelMermaid.FishPossibility.Homer
		Me.homerFishOverlaySprite.enabled = Me.fish = FlyingMermaidLevelMermaid.FishPossibility.Homer
		MyBase.animator.SetTrigger("Continue")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Tuckdown_Loop", False, True)
		t = 0F
		FlyingMermaidLevelSplashManager.Instance.SpawnMegaSplashLarge(MyBase.gameObject, 50F, True, 0F)
		While t < Me.tuckdownRiseTime
			t += CupheadTime.Delta
			MyBase.transform.SetPosition(Nothing, New Single?(Mathf.Lerp(Me.fishUnderwaterY, Me.regularY, t / Me.tuckdownRiseTime)), Nothing)
			Yield Nothing
		End While
		MyBase.animator.SetBool("Repeat", True)
		Dim pattern As String() = Me.nextFishPatternString().Split(New Char() { ","c })
		Dim waitTime As Single = MyBase.properties.CurrentState.fish.delayBeforeFirstAttack
		For i As Integer = 0 To pattern.Length - 1
			If pattern(i)(0) = "D"c Then
				Parser.FloatTryParse(pattern(i).Substring(1), waitTime)
			Else
				Yield CupheadTime.WaitForSeconds(Me, waitTime)
				MyBase.animator.SetTrigger("Continue")
				Yield MyBase.animator.WaitForAnimationToEnd(Me, "Fish_Attack_Start", False, True)
				Me.doFishAttack(pattern(i))
				If i < pattern.Length - 1 Then
					Yield MyBase.animator.WaitForAnimationToEnd(Me, "Fish_Attack_Repeat", False, True)
					waitTime = Me.waitTimeBetweenFishAttacks()
				End If
			End If
		Next
		MyBase.animator.SetBool("Repeat", False)
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Fish_Attack", False, True)
		Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.fish.delayBeforeFly)
		MyBase.animator.SetTrigger("Continue")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Fish_Launch", False, True)
		Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.fish.hesitateAfterAttack)
		Me.state = FlyingMermaidLevelMermaid.State.Idle
		Return
	End Function

	' Token: 0x060023AD RID: 9133 RVA: 0x0014E3EB File Offset: 0x0014C7EB
	Private Function nextFish() As FlyingMermaidLevelMermaid.FishPossibility
		Me.fishIndex = (Me.fishIndex + 1) Mod Me.fishPattern.Length
		Return Me.fishPattern(Me.fishIndex)
	End Function

	' Token: 0x060023AE RID: 9134 RVA: 0x0014E414 File Offset: 0x0014C814
	Private Sub initFishPatternIndices()
		Me.spreadshotPatternIndex = Global.UnityEngine.Random.Range(0, MyBase.properties.CurrentState.spreadshotFish.shootString.Length)
		Me.spinnerPatternIndex = Global.UnityEngine.Random.Range(0, MyBase.properties.CurrentState.spinnerFish.shootString.Length)
		Me.homerPatternIndex = Global.UnityEngine.Random.Range(0, MyBase.properties.CurrentState.homerFish.shootString.Length)
	End Sub

	' Token: 0x060023AF RID: 9135 RVA: 0x0014E48C File Offset: 0x0014C88C
	Private Function nextFishPatternString() As String
		Select Case Me.fish
			Case FlyingMermaidLevelMermaid.FishPossibility.Spreadshot
				Me.spreadshotPatternIndex = (Me.spreadshotPatternIndex + 1) Mod MyBase.properties.CurrentState.spreadshotFish.shootString.Length
				Return MyBase.properties.CurrentState.spreadshotFish.shootString(Me.spreadshotPatternIndex)
			Case FlyingMermaidLevelMermaid.FishPossibility.Spinner
				Me.spinnerPatternIndex = (Me.spinnerPatternIndex + 1) Mod MyBase.properties.CurrentState.spinnerFish.shootString.Length
				Return MyBase.properties.CurrentState.spinnerFish.shootString(Me.spinnerPatternIndex)
			Case FlyingMermaidLevelMermaid.FishPossibility.Homer
				Me.homerPatternIndex = (Me.homerPatternIndex + 1) Mod MyBase.properties.CurrentState.homerFish.shootString.Length
				Return MyBase.properties.CurrentState.homerFish.shootString(Me.homerPatternIndex)
			Case Else
				Return String.Empty
		End Select
	End Function

	' Token: 0x060023B0 RID: 9136 RVA: 0x0014E588 File Offset: 0x0014C988
	Private Function waitTimeBetweenFishAttacks() As Single
		Select Case Me.fish
			Case FlyingMermaidLevelMermaid.FishPossibility.Spreadshot
				Return MyBase.properties.CurrentState.spreadshotFish.attackDelay
			Case FlyingMermaidLevelMermaid.FishPossibility.Spinner
				Return MyBase.properties.CurrentState.spinnerFish.attackDelay
			Case FlyingMermaidLevelMermaid.FishPossibility.Homer
				Return MyBase.properties.CurrentState.homerFish.attackDelay
			Case Else
				Return 0F
		End Select
	End Function

	' Token: 0x060023B1 RID: 9137 RVA: 0x0014E5FC File Offset: 0x0014C9FC
	Private Sub doFishAttack(attackString As String)
		AudioManager.Play("level_mermaid_fish_attack")
		Me.emitAudioFromObject.Add("level_mermaid_fish_attack")
		Dim fishPossibility As FlyingMermaidLevelMermaid.FishPossibility = Me.fish
		If fishPossibility <> FlyingMermaidLevelMermaid.FishPossibility.Spreadshot Then
			If fishPossibility <> FlyingMermaidLevelMermaid.FishPossibility.Spinner Then
				If fishPossibility = FlyingMermaidLevelMermaid.FishPossibility.Homer Then
					Me.fishHomer()
				End If
			Else
				Me.fishSpinner()
			End If
		Else
			Me.fishSpreadshot(attackString)
		End If
	End Sub

	' Token: 0x060023B2 RID: 9138 RVA: 0x0014E668 File Offset: 0x0014CA68
	Private Sub fishSpreadshot(attackString As String)
		Dim num As Integer = 0
		Parser.IntTryParse(attackString.Substring(1), num)
		num -= 1
		Dim array As String() = MyBase.properties.CurrentState.spreadshotFish.spreadVariableGroups(num).Split(New Char() { ","c })
		Dim num2 As Single = 0F
		Dim num3 As Integer = 0
		Dim minMax As MinMax = New MinMax(0F, 0F)
		For Each text As String In array
			If text(0) = "S"c Then
				Parser.FloatTryParse(text.Substring(1), num2)
			ElseIf text(0) = "N"c Then
				Parser.IntTryParse(text.Substring(1), num3)
			Else
				Dim array3 As String() = text.Split(New Char() { "-"c })
				Parser.FloatTryParse(array3(0), minMax.min)
				Parser.FloatTryParse(array3(1), minMax.max)
			End If
		Next
		For j As Integer = 0 To num3 - 1
			Dim floatAt As Single = minMax.GetFloatAt(CSng(j) / (CSng(num3) - 1F))
			Dim basicProjectile As BasicProjectile = Me.fishSpreadshotBulletPrefab.Create(Me.fishProjectileRoot.position, floatAt, num2)
			basicProjectile.animator.SetInteger("Variant", j Mod 2)
			basicProjectile.SetParryable(Me.spreadFishPinkPattern(Me.spreadFishPinkIndex)(0) = "P"c)
			Me.spreadFishPinkIndex = (Me.spreadFishPinkIndex + 1) Mod Me.spreadFishPinkPattern.Length
		Next
	End Sub

	' Token: 0x060023B3 RID: 9139 RVA: 0x0014E804 File Offset: 0x0014CC04
	Private Sub fishSpinner()
		Dim [next] As AbstractPlayerController = PlayerManager.GetNext()
		Dim vector As Vector2 = [next].transform.position - Me.fishProjectileRoot.position
		vector.Normalize()
		If [next].transform.position.x > Me.fishProjectileRoot.transform.position.x Then
			vector = MathUtils.AngleToDirection(90F)
		End If
		Me.fishSpinnerBulletPrefab.Create(Me.fishProjectileRoot.position, vector, MyBase.properties.CurrentState.spinnerFish)
	End Sub

	' Token: 0x060023B4 RID: 9140 RVA: 0x0014E8A8 File Offset: 0x0014CCA8
	Private Sub fishHomer()
		Dim [next] As AbstractPlayerController = PlayerManager.GetNext()
		Dim vector As Vector2 = [next].transform.position - Me.fishProjectileRoot.position
		Dim num As Single = MathUtils.DirectionToAngle(vector) + Global.UnityEngine.Random.Range(-15F, 15F)
		Dim homerFish As LevelProperties.FlyingMermaid.HomerFish = MyBase.properties.CurrentState.homerFish
		If [next].transform.position.x > Me.fishProjectileRoot.transform.position.x Then
			num = 90F
		End If
		Me.fishHomerBulletPrefab.Create(Me.fishProjectileRoot.position, num, [next], homerFish)
	End Sub

	' Token: 0x060023B5 RID: 9141 RVA: 0x0014E958 File Offset: 0x0014CD58
	Public Sub LaunchFish()
		Dim flyingMermaidLevelFish As FlyingMermaidLevelFish = Nothing
		Dim fishPossibility As FlyingMermaidLevelMermaid.FishPossibility = Me.fish
		If fishPossibility <> FlyingMermaidLevelMermaid.FishPossibility.Spreadshot Then
			If fishPossibility <> FlyingMermaidLevelMermaid.FishPossibility.Spinner Then
				If fishPossibility = FlyingMermaidLevelMermaid.FishPossibility.Homer Then
					flyingMermaidLevelFish = Me.homerFishPrefab
				End If
			Else
				flyingMermaidLevelFish = Me.spinnerFishPrefab
			End If
		Else
			flyingMermaidLevelFish = Me.spreadshotFishPrefab
		End If
		flyingMermaidLevelFish.Create(Me.fishLaunchRoot.position, MyBase.properties.CurrentState.fish)
	End Sub

	' Token: 0x060023B6 RID: 9142 RVA: 0x0014E9D2 File Offset: 0x0014CDD2
	Private Sub OnFishSpitFx()
		Me.FishSpitEffectPrefab.Create(Me.fishProjectileRoot.position)
	End Sub

	' Token: 0x060023B7 RID: 9143 RVA: 0x0014E9EB File Offset: 0x0014CDEB
	Public Sub StartTransform()
		Me.transformationStarting = True
		MyBase.StartCoroutine(Me.transform_cr())
	End Sub

	' Token: 0x060023B8 RID: 9144 RVA: 0x0014EA04 File Offset: 0x0014CE04
	Private Iterator Function transform_cr() As IEnumerator
		While MyBase.transform.position.x <> Me.walkingPositions(0).position.x
			Yield Nothing
		End While
		Me.stopMoving = True
		Dim startX As Single = MyBase.transform.position.x
		Dim t As Single = 0F
		While t < Me.transformMoveTime
			t += CupheadTime.Delta
			MyBase.transform.SetPosition(New Single?(Mathf.Lerp(startX, startX - Me.transformMoveX, t / Me.transformMoveTime)), Nothing, Nothing)
			Yield Nothing
		End While
		MyBase.animator.SetTrigger("Transform")
		If Me.state = FlyingMermaidLevelMermaid.State.Summon Then
			Yield MyBase.animator.WaitForAnimationToStart(Me, "Idle", False)
			Me.stopPufferfish = True
		End If
		If Me.state = FlyingMermaidLevelMermaid.State.Idle Then
			Me.stopPufferfish = True
		End If
		Me.state = FlyingMermaidLevelMermaid.State.Transform
		Yield MyBase.animator.WaitForAnimationToStart(Me, "Transform", False)
		AudioManager.Play("level_mermaid_transform")
		CType(Level.Current, FlyingMermaidLevel).MerdusaTransformStarted = True
		Me.stopPufferfish = True
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Transform", False, True)
		t = 0F
		While t < Me.eelSinkTime
			t += CupheadTime.Delta
			MyBase.transform.SetPosition(Nothing, New Single?(Mathf.Lerp(Me.regularY, Me.eelUnderwaterY, t / Me.eelSinkTime)), Nothing)
			Yield Nothing
		End While
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Return
	End Function

	' Token: 0x060023B9 RID: 9145 RVA: 0x0014EA20 File Offset: 0x0014CE20
	Public Sub DisableColliders()
		Dim components As Collider2D() = MyBase.GetComponents(Of Collider2D)()
		For Each collider2D As Collider2D In components
			collider2D.enabled = False
		Next
		Me.blockingColliders.gameObject.SetActive(False)
	End Sub

	' Token: 0x060023BA RID: 9146 RVA: 0x0014EA66 File Offset: 0x0014CE66
	Public Sub SpawnMerdusa()
		Me.merdusa.StartIntro(MyBase.transform.position)
	End Sub

	' Token: 0x060023BB RID: 9147 RVA: 0x0014EA83 File Offset: 0x0014CE83
	Private Sub RightSplash()
		Me.splashRight.Create(Me.splashRoot.transform.position)
	End Sub

	' Token: 0x060023BC RID: 9148 RVA: 0x0014EAA1 File Offset: 0x0014CEA1
	Private Sub LeftSplash()
		Me.splashLeft.Create(Me.splashRoot.transform.position)
	End Sub

	' Token: 0x060023BD RID: 9149 RVA: 0x0014EABF File Offset: 0x0014CEBF
	Private Sub SoundMermaidFishLaunch()
		AudioManager.Play("level_mermaid_fish_launch")
		Me.emitAudioFromObject.Add("level_mermaid_fish_launch")
	End Sub

	' Token: 0x060023BE RID: 9150 RVA: 0x0014EADB File Offset: 0x0014CEDB
	Private Sub SoundMermaidAttackYellStart()
		AudioManager.Play("level_mermaid_yell_start")
		Me.emitAudioFromObject.Add("level_mermaid_yell_start")
	End Sub

	' Token: 0x060023BF RID: 9151 RVA: 0x0014EAF7 File Offset: 0x0014CEF7
	Private Sub SoundMermaidAttackYell()
		AudioManager.Play("level_mermaid_yell_attack")
		Me.emitAudioFromObject.Add("level_mermaid_yell_attack")
	End Sub

	' Token: 0x04002C38 RID: 11320
	<SerializeField()>
	Private walkingPositions As Transform()

	' Token: 0x04002C39 RID: 11321
	<SerializeField()>
	Private introRiseTime As Single

	' Token: 0x04002C3A RID: 11322
	<SerializeField()>
	Private tuckdownMoveTime As Single

	' Token: 0x04002C3B RID: 11323
	<SerializeField()>
	Private tuckdownWaitTime As Single

	' Token: 0x04002C3C RID: 11324
	<SerializeField()>
	Private tuckdownRiseTime As Single

	' Token: 0x04002C3D RID: 11325
	<SerializeField()>
	Private regularY As Single

	' Token: 0x04002C3E RID: 11326
	<SerializeField()>
	Private startUnderwaterY As Single

	' Token: 0x04002C3F RID: 11327
	<SerializeField()>
	Private fishUnderwaterY As Single

	' Token: 0x04002C40 RID: 11328
	<SerializeField()>
	Private transformMoveTime As Single

	' Token: 0x04002C41 RID: 11329
	<SerializeField()>
	Private transformMoveX As Single

	' Token: 0x04002C42 RID: 11330
	<SerializeField()>
	Private eelSinkTime As Single

	' Token: 0x04002C43 RID: 11331
	<SerializeField()>
	Private eelUnderwaterY As Single

	' Token: 0x04002C44 RID: 11332
	<SerializeField()>
	Private yellProjectilePrefab As FlyingMermaidLevelYellProjectile

	' Token: 0x04002C45 RID: 11333
	<SerializeField()>
	Private seahorsePrefab As FlyingMermaidLevelSeahorse

	' Token: 0x04002C46 RID: 11334
	<SerializeField()>
	Private FishSpitEffectPrefab As Effect

	' Token: 0x04002C47 RID: 11335
	Private introEnded As Boolean

	' Token: 0x04002C48 RID: 11336
	Private damageDealer As DamageDealer

	' Token: 0x04002C49 RID: 11337
	Private damageReceiver As DamageReceiver

	' Token: 0x04002C4A RID: 11338
	<SerializeField()>
	Private projectileRoot As Transform

	' Token: 0x04002C4B RID: 11339
	<SerializeField()>
	Private yellFxRoot As Transform

	' Token: 0x04002C4C RID: 11340
	<SerializeField()>
	Private pufferfishPrefabs As FlyingMermaidLevelPufferfish()

	' Token: 0x04002C4D RID: 11341
	<SerializeField()>
	Private pinkPufferfishPrefab As FlyingMermaidLevelPufferfish

	' Token: 0x04002C4E RID: 11342
	<SerializeField()>
	Private turtlePrefab As FlyingMermaidLevelTurtle

	' Token: 0x04002C4F RID: 11343
	<SerializeField()>
	Private blinkOverlaySprite As SpriteRenderer

	' Token: 0x04002C50 RID: 11344
	<SerializeField()>
	Private spreadshotFishSprite As SpriteRenderer

	' Token: 0x04002C51 RID: 11345
	<SerializeField()>
	Private spinnerFishSprite As SpriteRenderer

	' Token: 0x04002C52 RID: 11346
	<SerializeField()>
	Private homerFishSprite As SpriteRenderer

	' Token: 0x04002C53 RID: 11347
	<SerializeField()>
	Private spreadshotFishOverlaySprite As SpriteRenderer

	' Token: 0x04002C54 RID: 11348
	<SerializeField()>
	Private spinnerFishOverlaySprite As SpriteRenderer

	' Token: 0x04002C55 RID: 11349
	<SerializeField()>
	Private homerFishOverlaySprite As SpriteRenderer

	' Token: 0x04002C56 RID: 11350
	<SerializeField()>
	Private spreadshotFishPrefab As FlyingMermaidLevelFish

	' Token: 0x04002C57 RID: 11351
	<SerializeField()>
	Private spinnerFishPrefab As FlyingMermaidLevelFish

	' Token: 0x04002C58 RID: 11352
	<SerializeField()>
	Private homerFishPrefab As FlyingMermaidLevelFish

	' Token: 0x04002C59 RID: 11353
	<SerializeField()>
	Private fishSpreadshotBulletPrefab As BasicProjectile

	' Token: 0x04002C5A RID: 11354
	<SerializeField()>
	Private fishSpinnerBulletPrefab As FlyingMermaidLevelFishSpinner

	' Token: 0x04002C5B RID: 11355
	<SerializeField()>
	Private fishHomerBulletPrefab As FlyingMermaidLevelHomingProjectile

	' Token: 0x04002C5C RID: 11356
	<SerializeField()>
	Private fishLaunchRoot As Transform

	' Token: 0x04002C5D RID: 11357
	<SerializeField()>
	Private fishProjectileRoot As Transform

	' Token: 0x04002C5E RID: 11358
	<SerializeField()>
	Private merdusa As FlyingMermaidLevelMerdusa

	' Token: 0x04002C5F RID: 11359
	<SerializeField()>
	Private blockingColliders As Transform

	' Token: 0x04002C60 RID: 11360
	<SerializeField()>
	Private splashRight As Effect

	' Token: 0x04002C61 RID: 11361
	<SerializeField()>
	Private splashLeft As Effect

	' Token: 0x04002C62 RID: 11362
	<SerializeField()>
	Private splashRoot As Transform

	' Token: 0x04002C63 RID: 11363
	<SerializeField()>
	Private yellEffect As Effect

	' Token: 0x04002C64 RID: 11364
	Private summonPattern As FlyingMermaidLevelMermaid.SummonPossibility() = New FlyingMermaidLevelMermaid.SummonPossibility() { FlyingMermaidLevelMermaid.SummonPossibility.Seahorse, FlyingMermaidLevelMermaid.SummonPossibility.Pufferfish, FlyingMermaidLevelMermaid.SummonPossibility.Turtle }

	' Token: 0x04002C65 RID: 11365
	Private fishPattern As FlyingMermaidLevelMermaid.FishPossibility()

	' Token: 0x04002C66 RID: 11366
	Private summonIndex As Integer

	' Token: 0x04002C67 RID: 11367
	Private fishIndex As Integer

	' Token: 0x04002C68 RID: 11368
	Private spreadshotPatternIndex As Integer

	' Token: 0x04002C69 RID: 11369
	Private spinnerPatternIndex As Integer

	' Token: 0x04002C6A RID: 11370
	Private homerPatternIndex As Integer

	' Token: 0x04002C6B RID: 11371
	Private stopPufferfish As Boolean

	' Token: 0x04002C6C RID: 11372
	Private transformationStarting As Boolean

	' Token: 0x04002C6D RID: 11373
	Private stopMoving As Boolean

	' Token: 0x04002C6E RID: 11374
	Private walkPCT As Single

	' Token: 0x04002C6F RID: 11375
	Private walkTime As Single

	' Token: 0x04002C70 RID: 11376
	Private walkDuration As Single

	' Token: 0x04002C71 RID: 11377
	Private spreadFishPinkPattern As String()

	' Token: 0x04002C72 RID: 11378
	Private spreadFishPinkIndex As Integer

	' Token: 0x04002C73 RID: 11379
	Private blinks As Integer

	' Token: 0x04002C74 RID: 11380
	Private maxBlinks As Integer

	' Token: 0x04002C75 RID: 11381
	Private fish As FlyingMermaidLevelMermaid.FishPossibility

	' Token: 0x02000693 RID: 1683
	Public Enum State
		' Token: 0x04002C77 RID: 11383
		Intro
		' Token: 0x04002C78 RID: 11384
		Idle
		' Token: 0x04002C79 RID: 11385
		Yell
		' Token: 0x04002C7A RID: 11386
		Summon
		' Token: 0x04002C7B RID: 11387
		Fish
		' Token: 0x04002C7C RID: 11388
		Transform
	End Enum

	' Token: 0x02000694 RID: 1684
	Public Enum SummonPossibility
		' Token: 0x04002C7E RID: 11390
		Seahorse
		' Token: 0x04002C7F RID: 11391
		Pufferfish
		' Token: 0x04002C80 RID: 11392
		Turtle
	End Enum

	' Token: 0x02000695 RID: 1685
	Public Enum FishPossibility
		' Token: 0x04002C82 RID: 11394
		Spreadshot
		' Token: 0x04002C83 RID: 11395
		Spinner
		' Token: 0x04002C84 RID: 11396
		Homer
	End Enum
End Class
