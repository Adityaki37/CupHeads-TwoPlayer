Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200063B RID: 1595
Public Class FlyingBlimpLevelMoonLady
	Inherits LevelProperties.FlyingBlimp.Entity

	' Token: 0x17000386 RID: 902
	' (get) Token: 0x060020B9 RID: 8377 RVA: 0x0012E349 File Offset: 0x0012C749
	' (set) Token: 0x060020BA RID: 8378 RVA: 0x0012E351 File Offset: 0x0012C751
	Public Property state As FlyingBlimpLevelMoonLady.State

	' Token: 0x060020BB RID: 8379 RVA: 0x0012E35C File Offset: 0x0012C75C
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.childColliders = MyBase.gameObject.GetComponentsInChildren(Of CollisionChild)()
		Me.changeStarted = False
		Dim vector As Vector3 = New Vector3(1F, 1F, 1F)
		If Rand.Bool() Then
			vector.y = -vector.y
			Me.smoke.transform.position = Me.smokeFlippedPos.transform.position
		End If
		Me.smoke.transform.SetScale(New Single?(1F), New Single?(vector.y), New Single?(1F))
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		MyBase.GetComponent(Of Collider2D)().enabled = False
		For Each collisionChild As CollisionChild In Me.childColliders
			AddHandler collisionChild.OnPlayerCollision, AddressOf Me.OnCollisionPlayer
			collisionChild.GetComponent(Of Collider2D)().enabled = False
		Next
	End Sub

	' Token: 0x060020BC RID: 8380 RVA: 0x0012E480 File Offset: 0x0012C880
	Public Sub StartIntro()
		MyBase.GetComponent(Of Collider2D)().enabled = True
		MyBase.transform.position = Me.transformSpawnPoint.position
		MyBase.animator.SetTrigger("To A")
		MyBase.StartCoroutine(Me.intro_cr())
	End Sub

	' Token: 0x060020BD RID: 8381 RVA: 0x0012E4CC File Offset: 0x0012C8CC
	Public Overrides Sub LevelInit(properties As LevelProperties.FlyingBlimp)
		MyBase.LevelInit(properties)
		Me.state = FlyingBlimpLevelMoonLady.State.Unspawned
	End Sub

	' Token: 0x060020BE RID: 8382 RVA: 0x0012E4DC File Offset: 0x0012C8DC
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		If Me.state <> FlyingBlimpLevelMoonLady.State.Morph Then
			If MyBase.properties.CurrentState.uFO.invincibility Then
				If info.damage < MyBase.properties.CurrentHealth Then
					MyBase.properties.DealDamage(info.damage)
				ElseIf Me.state = FlyingBlimpLevelMoonLady.State.Idle Then
					MyBase.properties.DealDamage(info.damage)
				End If
			Else
				MyBase.properties.DealDamage(info.damage)
			End If
		End If
		If MyBase.properties.CurrentHealth <= 0F AndAlso Me.state <> FlyingBlimpLevelMoonLady.State.Death Then
			Me.StartDeath()
		End If
	End Sub

	' Token: 0x060020BF RID: 8383 RVA: 0x0012E598 File Offset: 0x0012C998
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
		If PauseManager.state = PauseManager.State.Paused Then
			Me.pedal.Pause()
			Me.gears.Pause()
		Else
			Me.pedal.UnPause()
			Me.gears.UnPause()
		End If
	End Sub

	' Token: 0x060020C0 RID: 8384 RVA: 0x0012E5F7 File Offset: 0x0012C9F7
	Public Overrides Sub OnLevelEnd()
		MyBase.OnLevelEnd()
		Me.pedal.[Stop]()
		Me.gears.[Stop]()
	End Sub

	' Token: 0x060020C1 RID: 8385 RVA: 0x0012E615 File Offset: 0x0012CA15
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x060020C2 RID: 8386 RVA: 0x0012E634 File Offset: 0x0012CA34
	Private Iterator Function intro_cr() As IEnumerator
		AudioManager.Play("level_flying_blimp_transform_moon")
		Me.state = FlyingBlimpLevelMoonLady.State.Morph
		Dim p As LevelProperties.FlyingBlimp.Morph = MyBase.properties.CurrentState.morph
		Dim playerOne As PlanePlayerController = PlayerManager.GetPlayer(Of PlanePlayerController)(PlayerId.PlayerOne)
		Dim playerTwo As PlanePlayerController = PlayerManager.GetPlayer(Of PlanePlayerController)(PlayerId.PlayerTwo)
		If playerOne IsNot Nothing AndAlso playerOne.isActiveAndEnabled Then
			playerOne.animationController.SetColorOverTime(Me.dimColor.GetComponent(Of SpriteRenderer)().color, 15F)
		End If
		If playerTwo IsNot Nothing AndAlso playerTwo.isActiveAndEnabled Then
			playerTwo.animationController.SetColorOverTime(Me.dimColor.GetComponent(Of SpriteRenderer)().color, 15F)
		End If
		Yield Nothing
		While MyBase.transform.position <> Me.transformMorphEndPoint.position
			MyBase.transform.position = Vector3.MoveTowards(MyBase.transform.position, Me.transformMorphEndPoint.position, 300F * CupheadTime.Delta)
			Yield CupheadTime.WaitForSeconds(Me, 0.1F)
		End While
		Yield CupheadTime.WaitForSeconds(Me, p.crazyAHold)
		Me.pedal.[Stop]()
		MyBase.animator.SetTrigger("To B")
		Yield CupheadTime.WaitForSeconds(Me, p.crazyBHold)
		MyBase.animator.SetTrigger("End")
		MyBase.StartCoroutine(Me.stars_cr())
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Morph_End", False, True)
		Me.state = FlyingBlimpLevelMoonLady.State.Idle
		For Each collisionChild As CollisionChild In Me.childColliders
			collisionChild.GetComponent(Of Collider2D)().enabled = True
		Next
		Level.Current.SetBounds(Nothing, New Integer?(Level.Current.Right - 250), Nothing, Nothing)
		MyBase.StartCoroutine(Me.ufo_attack_handler_cr())
		Yield Nothing
		Return
	End Function

	' Token: 0x060020C3 RID: 8387 RVA: 0x0012E650 File Offset: 0x0012CA50
	Private Sub SpawnStar(prefab As FlyingBlimpLevelStars, startPoint As Vector2)
		If prefab IsNot Nothing Then
			Dim vector As Vector2 = prefab.transform.position
			vector.y = 360F - startPoint.y
			vector.x = 640F
			prefab.Create(vector, MyBase.properties.CurrentState.stars)
		End If
	End Sub

	' Token: 0x060020C4 RID: 8388 RVA: 0x0012E6B4 File Offset: 0x0012CAB4
	Private Iterator Function stars_cr() As IEnumerator
		Dim p As LevelProperties.FlyingBlimp.Stars = MyBase.properties.CurrentState.stars
		Dim positionPattern As String() = p.positionString.GetRandom().Split(New Char() { ","c })
		Dim typePattern As String() = p.typeString.GetRandom().Split(New Char() { ","c })
		Dim i As Integer = Global.UnityEngine.Random.Range(0, typePattern.Length)
		Dim t As Single = 0F
		Dim place As Integer = Global.UnityEngine.Random.Range(0, positionPattern.Length)
		Dim waitTime As Single = 0F
		Dim spawnPos As Vector2 = Vector2.zero
		While True
			For j As Integer = place To positionPattern.Length - 1
				If waitTime > 0F Then
					Yield CupheadTime.WaitForSeconds(Me, waitTime)
				End If
				If positionPattern(j)(0) = "D"c Then
					Parser.FloatTryParse(positionPattern(j).Substring(1), waitTime)
				Else
					Dim array As String() = positionPattern(j).Split(New Char() { "-"c })
					For Each text As String In array
						Dim num As Single = 0F
						Parser.FloatTryParse(text, num)
						Dim flyingBlimpLevelStars As FlyingBlimpLevelStars = Nothing
						If typePattern(i)(0) = "A"c Then
							flyingBlimpLevelStars = Me.starPrefabA
						ElseIf typePattern(i)(0) = "B"c Then
							flyingBlimpLevelStars = Me.starPrefabB
						ElseIf typePattern(i)(0) = "C"c Then
							flyingBlimpLevelStars = Me.starPrefabC
						ElseIf typePattern(i)(0) = "P"c Then
							flyingBlimpLevelStars = Me.starPrefabPink
						End If
						Parser.FloatTryParse(positionPattern(j).Substring(1), waitTime)
						spawnPos.y = num
						If Me.state <> FlyingBlimpLevelMoonLady.State.Death Then
							Me.SpawnStar(flyingBlimpLevelStars, spawnPos)
						End If
						i = (i + 1) Mod typePattern.Length
					Next
					waitTime = p.delay
				End If
				t += waitTime
				j = j Mod positionPattern.Length
				place = 0
			Next
		End While
		Return
	End Function

	' Token: 0x060020C5 RID: 8389 RVA: 0x0012E6D0 File Offset: 0x0012CAD0
	Private Iterator Function ufo_attack_handler_cr() As IEnumerator
		Me.state = FlyingBlimpLevelMoonLady.State.Idle
		Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.uFO.moonWaitForNextATK)
		MyBase.StartCoroutine(Me.ufo_cr())
		Yield Nothing
		Return
	End Function

	' Token: 0x060020C6 RID: 8390 RVA: 0x0012E6EB File Offset: 0x0012CAEB
	Private Sub SmokeEffect()
		MyBase.animator.Play("Moon_Smoke")
	End Sub

	' Token: 0x060020C7 RID: 8391 RVA: 0x0012E700 File Offset: 0x0012CB00
	Private Sub SpawnUFO(prefab As FlyingBlimpLevelUFO)
		Dim uFO As LevelProperties.FlyingBlimp.UFO = MyBase.properties.CurrentState.uFO
		Dim flyingBlimpLevelUFO As FlyingBlimpLevelUFO = Global.UnityEngine.[Object].Instantiate(Of FlyingBlimpLevelUFO)(prefab)
		flyingBlimpLevelUFO.Init(Me.ufoStartPoint.position, Me.ufoMidPoint.position, Me.ufoStopPoint.position, uFO.UFOSpeed, uFO.UFOHP, uFO)
	End Sub

	' Token: 0x060020C8 RID: 8392 RVA: 0x0012E768 File Offset: 0x0012CB68
	Private Iterator Function ufo_cr() As IEnumerator
		Me.state = FlyingBlimpLevelMoonLady.State.Attack
		Dim volume As Single = 0.1F
		Dim p As LevelProperties.FlyingBlimp.UFO = MyBase.properties.CurrentState.uFO
		Dim typePattern As String() = p.UFOString.GetRandom().Split(New Char() { ","c })
		Dim index As Integer = Global.UnityEngine.Random.Range(0, typePattern.Length)
		AudioManager.Play("level_flying_blimp_moon_anticipation")
		MyBase.animator.SetTrigger("To ATK")
		Yield CupheadTime.WaitForSeconds(Me, p.moonATKAnticipation)
		Me.gears.Play()
		Me.gears.volume = volume
		AudioManager.Play("level_flying_blimp_moon_face_extend")
		MyBase.animator.SetTrigger("Continue")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Moon_Attack", False, True)
		Me.time = 0F
		Me.startTimer = True
		MyBase.StartCoroutine(Me.timer_cr())
		While Me.time < p.moonATKDuration
			Me.pedal.volume = volume
			If volume < 1F Then
				volume += 0.1F
			End If
			If Me.state <> FlyingBlimpLevelMoonLady.State.Death Then
				If typePattern(index)(0) = "A"c Then
					Me.SpawnUFO(Me.ufoPrefabA)
				ElseIf typePattern(index)(0) = "B"c Then
					Me.SpawnUFO(Me.ufoPrefabB)
				End If
			End If
			Yield CupheadTime.WaitForSeconds(Me, p.UFODelay)
			If index < typePattern.Length - 1 Then
				index += 1
			Else
				index = 0
			End If
		End While
		Me.pedal.volume = 1F
		MyBase.animator.SetTrigger("End")
		Me.startTimer = False
		Me.gears.[Stop]()
		AudioManager.Play("level_flying_blimp_moon_gears_idle")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Moon_Attack_To_Idle", False, True)
		MyBase.StartCoroutine(Me.ufo_attack_handler_cr())
		Return
	End Function

	' Token: 0x060020C9 RID: 8393 RVA: 0x0012E784 File Offset: 0x0012CB84
	Private Iterator Function timer_cr() As IEnumerator
		While Me.startTimer
			Me.time += CupheadTime.Delta
			Yield Nothing
		End While
		Yield Nothing
		Return
	End Function

	' Token: 0x060020CA RID: 8394 RVA: 0x0012E79F File Offset: 0x0012CB9F
	Public Sub StartDeath()
		Me.state = FlyingBlimpLevelMoonLady.State.Death
		MyBase.StartCoroutine(Me.die_cr())
	End Sub

	' Token: 0x060020CB RID: 8395 RVA: 0x0012E7B8 File Offset: 0x0012CBB8
	Private Iterator Function die_cr() As IEnumerator
		MyBase.animator.SetTrigger("Death")
		MyBase.GetComponent(Of Collider2D)().enabled = False
		Yield Nothing
		Return
	End Function

	' Token: 0x04002945 RID: 10565
	Public changeStarted As Boolean

	' Token: 0x04002947 RID: 10567
	<SerializeField()>
	Private smoke As GameObject

	' Token: 0x04002948 RID: 10568
	<SerializeField()>
	Private smokeFlippedPos As Transform

	' Token: 0x04002949 RID: 10569
	<SerializeField()>
	Private pedal As AudioSource

	' Token: 0x0400294A RID: 10570
	<SerializeField()>
	Private gears As AudioSource

	' Token: 0x0400294B RID: 10571
	<SerializeField()>
	Private ufoPrefabA As FlyingBlimpLevelUFO

	' Token: 0x0400294C RID: 10572
	<SerializeField()>
	Private ufoPrefabB As FlyingBlimpLevelUFO

	' Token: 0x0400294D RID: 10573
	<SerializeField()>
	Private ufoStartPoint As Transform

	' Token: 0x0400294E RID: 10574
	<SerializeField()>
	Private ufoMidPoint As Transform

	' Token: 0x0400294F RID: 10575
	<SerializeField()>
	Private ufoStopPoint As Transform

	' Token: 0x04002950 RID: 10576
	<SerializeField()>
	Private dimColor As Transform

	' Token: 0x04002951 RID: 10577
	<SerializeField()>
	Private transformSpawnPoint As Transform

	' Token: 0x04002952 RID: 10578
	<SerializeField()>
	Private transformMorphEndPoint As Transform

	' Token: 0x04002953 RID: 10579
	<SerializeField()>
	Private starPrefabA As FlyingBlimpLevelStars

	' Token: 0x04002954 RID: 10580
	<SerializeField()>
	Private starPrefabB As FlyingBlimpLevelStars

	' Token: 0x04002955 RID: 10581
	<SerializeField()>
	Private starPrefabC As FlyingBlimpLevelStars

	' Token: 0x04002956 RID: 10582
	<SerializeField()>
	Private starPrefabPink As FlyingBlimpLevelStars

	' Token: 0x04002957 RID: 10583
	Private damageDealer As DamageDealer

	' Token: 0x04002958 RID: 10584
	Private damageReceiver As DamageReceiver

	' Token: 0x04002959 RID: 10585
	Private childColliders As CollisionChild()

	' Token: 0x0400295A RID: 10586
	Private time As Single

	' Token: 0x0400295B RID: 10587
	Private startTimer As Boolean

	' Token: 0x0200063C RID: 1596
	Public Enum State
		' Token: 0x0400295D RID: 10589
		Unspawned
		' Token: 0x0400295E RID: 10590
		Morph
		' Token: 0x0400295F RID: 10591
		Idle
		' Token: 0x04002960 RID: 10592
		Attack
		' Token: 0x04002961 RID: 10593
		Death
	End Enum
End Class
