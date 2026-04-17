Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020005E5 RID: 1509
Public Class DicePalaceRouletteLevelRoulette
	Inherits LevelProperties.DicePalaceRoulette.Entity

	' Token: 0x17000373 RID: 883
	' (get) Token: 0x06001DEA RID: 7658 RVA: 0x001130A0 File Offset: 0x001114A0
	' (set) Token: 0x06001DEB RID: 7659 RVA: 0x001130A8 File Offset: 0x001114A8
	Public Property state As DicePalaceRouletteLevelRoulette.State

	' Token: 0x06001DEC RID: 7660 RVA: 0x001130B1 File Offset: 0x001114B1
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
	End Sub

	' Token: 0x06001DED RID: 7661 RVA: 0x001130E7 File Offset: 0x001114E7
	Private Sub Start()
		Me.state = DicePalaceRouletteLevelRoulette.State.Intro
		MyBase.StartCoroutine(Me.intro_cr())
	End Sub

	' Token: 0x06001DEE RID: 7662 RVA: 0x001130FD File Offset: 0x001114FD
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06001DEF RID: 7663 RVA: 0x00113115 File Offset: 0x00111515
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06001DF0 RID: 7664 RVA: 0x00113134 File Offset: 0x00111534
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		MyBase.properties.DealDamage(info.damage)
		If MyBase.properties.CurrentHealth <= 0F AndAlso Me.state <> DicePalaceRouletteLevelRoulette.State.Death Then
			Me.state = DicePalaceRouletteLevelRoulette.State.Death
			Me.StartDeath()
		End If
	End Sub

	' Token: 0x06001DF1 RID: 7665 RVA: 0x00113180 File Offset: 0x00111580
	Private Iterator Function intro_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 2F)
		AudioManager.Play("dice_palace_roulette_intro")
		Me.emitAudioFromObject.Add("dice_palace_roulette_intro")
		MyBase.animator.Play("Roulette_Intro")
		Me.state = DicePalaceRouletteLevelRoulette.State.Idle
		Return
	End Function

	' Token: 0x06001DF2 RID: 7666 RVA: 0x0011319B File Offset: 0x0011159B
	Protected Overridable Function hitPauseCoefficient() As Single
		Return If((Not MyBase.GetComponent(Of DamageReceiver)().IsHitPaused), 1F, 0F)
	End Function

	' Token: 0x06001DF3 RID: 7667 RVA: 0x001131BC File Offset: 0x001115BC
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.marble = Nothing
		Me.marbleLaunch = Nothing
	End Sub

	' Token: 0x06001DF4 RID: 7668 RVA: 0x001131D2 File Offset: 0x001115D2
	Public Sub StartTwirl()
		If Me.patternCoroutine IsNot Nothing Then
			MyBase.StopCoroutine(Me.patternCoroutine)
		End If
		Me.patternCoroutine = MyBase.StartCoroutine(Me.twirl_cr())
	End Sub

	' Token: 0x06001DF5 RID: 7669 RVA: 0x00113200 File Offset: 0x00111600
	Private Iterator Function twirl_cr() As IEnumerator
		Me.state = DicePalaceRouletteLevelRoulette.State.Twirl
		MyBase.animator.Play("Roulette_Travel")
		Dim p As LevelProperties.DicePalaceRoulette.Twirl = MyBase.properties.CurrentState.twirl
		Dim amountPattern As String() = p.twirlAmount.GetRandom().Split(New Char() { ","c })
		MyBase.StartCoroutine(Me.twirl_vary_speed_cr())
		Dim twirlAmount As Single = 0F
		Dim stopDist As Single = 200F
		Parser.FloatTryParse(amountPattern(Me.index), twirlAmount)
		Dim pos As Vector3 = MyBase.transform.position
		Dim i As Integer = 0
		While CSng(i) < twirlAmount
			If Me.onRight Then
				Me.slowDown = False
				Dim maxPoint As Single = -630F
				While MyBase.transform.position.x > maxPoint
					If Not Me.stopTwirl Then
						Dim num As Single = maxPoint - MyBase.transform.position.x
						num = Mathf.Abs(num)
						pos.x = Mathf.MoveTowards(MyBase.transform.position.x, maxPoint, Me.speed * CupheadTime.Delta * Me.hitPauseCoefficient())
						If num < stopDist Then
							Me.slowDown = True
						End If
						MyBase.transform.position = pos
					End If
					Yield Nothing
				End While
				Me.onRight = Not Me.onRight
			Else
				Me.slowDown = False
				Dim maxPoint2 As Single = 490F
				While MyBase.transform.position.x < maxPoint2
					If Not Me.stopTwirl Then
						Dim num2 As Single = maxPoint2 - MyBase.transform.position.x
						num2 = Mathf.Abs(num2)
						pos.x = Mathf.MoveTowards(MyBase.transform.position.x, maxPoint2, Me.speed * CupheadTime.Delta * Me.hitPauseCoefficient())
						If num2 < stopDist Then
							Me.slowDown = True
						End If
						MyBase.transform.position = pos
					End If
					Yield Nothing
				End While
				Me.onRight = Not Me.onRight
			End If
			i += 1
		End While
		twirlAmount = (twirlAmount + 1F) Mod CSng(amountPattern.Length)
		MyBase.StopCoroutine(Me.twirl_vary_speed_cr())
		Me.state = DicePalaceRouletteLevelRoulette.State.Idle
		Return
	End Function

	' Token: 0x06001DF6 RID: 7670 RVA: 0x0011321B File Offset: 0x0011161B
	Private Sub TwirlStop()
		Me.stopTwirl = True
	End Sub

	' Token: 0x06001DF7 RID: 7671 RVA: 0x00113224 File Offset: 0x00111624
	Private Sub TwirlStart()
		Me.stopTwirl = False
	End Sub

	' Token: 0x06001DF8 RID: 7672 RVA: 0x00113230 File Offset: 0x00111630
	Private Iterator Function twirl_vary_speed_cr() As IEnumerator
		Dim p As LevelProperties.DicePalaceRoulette.Twirl = MyBase.properties.CurrentState.twirl
		Dim incrementspeed As Single = p.movementSpeed / 50F
		While True
			If Me.slowDown Then
				If Me.speed <= 50F Then
					Me.slowDown = False
				Else
					Me.speed -= incrementspeed
				End If
			ElseIf Me.speed < p.movementSpeed Then
				Me.speed += incrementspeed
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06001DF9 RID: 7673 RVA: 0x0011324B File Offset: 0x0011164B
	Public Sub StartMarbleDrop()
		If Me.patternCoroutine IsNot Nothing Then
			MyBase.StopCoroutine(Me.patternCoroutine)
		End If
		Me.patternCoroutine = MyBase.StartCoroutine(Me.marble_drop_cr())
	End Sub

	' Token: 0x06001DFA RID: 7674 RVA: 0x00113278 File Offset: 0x00111678
	Private Sub SpawnMarble(xOffset As Single)
		Dim marbleDrop As LevelProperties.DicePalaceRoulette.MarbleDrop = MyBase.properties.CurrentState.marbleDrop
		Dim num As Single = Mathf.Atan2(CSng(Level.Current.Ground), 0F) * 57.29578F
		Dim vector As Vector2 = MyBase.transform.position
		vector.y = 360F
		vector.x = If((Not Me.onRight), (640F - xOffset), (-640F + xOffset))
		Me.marble.Create(vector, num, marbleDrop.marbleSpeed)
	End Sub

	' Token: 0x06001DFB RID: 7675 RVA: 0x00113308 File Offset: 0x00111708
	Private Iterator Function marble_drop_cr() As IEnumerator
		Dim p As LevelProperties.DicePalaceRoulette.MarbleDrop = MyBase.properties.CurrentState.marbleDrop
		Dim spawnPattern As String() = p.marblePositionStrings.GetRandom().Split(New Char() { ","c })
		Dim waitTime As Single = 0F
		Me.state = DicePalaceRouletteLevelRoulette.State.Marble
		Me.firstLaunch = True
		Me.stopMarbles = False
		MyBase.animator.Play("Roulette_Attack_Start")
		AudioManager.Play("dice_palace_roulette_attack_start")
		Me.emitAudioFromObject.Add("dice_palace_roulette_attack_start")
		Yield MyBase.animator.WaitForAnimationToStart(Me, "Roulette_Attack_Loop", False)
		AudioManager.PlayLoop("dice_palace_roulette_attack_loop")
		Me.emitAudioFromObject.Add("dice_palace_roulette_attack_loop")
		MyBase.StartCoroutine(Me.marble_sound_cr())
		Yield CupheadTime.WaitForSeconds(Me, p.marbleInitalDelay)
		For i As Integer = 0 To spawnPattern.Length - 1
			If spawnPattern(i)(0) = "D"c Then
				Parser.FloatTryParse(spawnPattern(i).Substring(1), waitTime)
				Yield CupheadTime.WaitForSeconds(Me, waitTime)
			Else
				Dim array As String() = spawnPattern(i).Split(New Char() { "-"c })
				For Each text As String In array
					Dim num As Single = 0F
					Parser.FloatTryParse(text, num)
					Me.SpawnMarble(num)
				Next
			End If
			i = i Mod spawnPattern.Length
			Yield CupheadTime.WaitForSeconds(Me, p.marbleDelay)
		Next
		Me.stopMarbles = True
		Yield CupheadTime.WaitForSeconds(Me, p.hesitate)
		MyBase.animator.SetTrigger("Continue")
		AudioManager.[Stop]("dice_palace_roulette_attack_loop")
		AudioManager.Play("dice_palace_roulette_attack_end")
		Me.emitAudioFromObject.Add("dice_palace_roulette_attack_end")
		Yield CupheadTime.WaitForSeconds(Me, 0.5F)
		Me.state = DicePalaceRouletteLevelRoulette.State.Idle
		Return
	End Function

	' Token: 0x06001DFC RID: 7676 RVA: 0x00113324 File Offset: 0x00111724
	Public Sub SpawnMarbleAnimation()
		If Me.stopMarbles Then
			Return
		End If
		Dim dicePalaceRouletteLevelMarblesLaunch As DicePalaceRouletteLevelMarblesLaunch = Global.UnityEngine.[Object].Instantiate(Of DicePalaceRouletteLevelMarblesLaunch)(Me.marbleLaunch, Me.marbleRoot, False)
		dicePalaceRouletteLevelMarblesLaunch.IsFirstTime = Me.firstLaunch
		Me.firstLaunch = False
	End Sub

	' Token: 0x06001DFD RID: 7677 RVA: 0x00113364 File Offset: 0x00111764
	Private Iterator Function marble_sound_cr() As IEnumerator
		AudioManager.Play("dice_palace_roulette_balls_start")
		Me.emitAudioFromObject.Add("dice_palace_roulette_balls_start")
		AudioManager.PlayLoop("dice_palace_roulette_balls_shoot_loop")
		Me.emitAudioFromObject.Add("dice_palace_roulette_balls_shoot_loop")
		While Not Me.stopMarbles
			Yield Nothing
		End While
		AudioManager.[Stop]("dice_palace_roulette_balls_shoot_loop")
		AudioManager.Play("dice_palace_roulette_balls_end")
		Me.emitAudioFromObject.Add("dice_palace_roulette_balls_end")
		Return
	End Function

	' Token: 0x06001DFE RID: 7678 RVA: 0x0011337F File Offset: 0x0011177F
	Private Sub StartDeath()
		Me.StopAllCoroutines()
		MyBase.GetComponent(Of Collider2D)().enabled = False
		MyBase.animator.Play("Roulette_Death")
		AudioManager.Play("dice_palace_roulette_death")
		Me.emitAudioFromObject.Add("dice_palace_roulette_death")
	End Sub

	' Token: 0x06001DFF RID: 7679 RVA: 0x001133BD File Offset: 0x001117BD
	Private Sub TravelSFX()
		AudioManager.Play("dice_palace_roulette_travel")
		Me.emitAudioFromObject.Add("dice_palace_roulette_travel")
	End Sub

	' Token: 0x040026B6 RID: 9910
	<SerializeField()>
	Private marble As BasicProjectile

	' Token: 0x040026B7 RID: 9911
	<SerializeField()>
	Private marbleLaunch As DicePalaceRouletteLevelMarblesLaunch

	' Token: 0x040026B8 RID: 9912
	<SerializeField()>
	Private marbleRoot As Transform

	' Token: 0x040026B9 RID: 9913
	Private onRight As Boolean = True

	' Token: 0x040026BA RID: 9914
	Private slowDown As Boolean

	' Token: 0x040026BB RID: 9915
	Private stopTwirl As Boolean

	' Token: 0x040026BC RID: 9916
	Private firstLaunch As Boolean = True

	' Token: 0x040026BD RID: 9917
	Private stopMarbles As Boolean

	' Token: 0x040026BE RID: 9918
	Private index As Integer

	' Token: 0x040026BF RID: 9919
	Private speed As Single

	' Token: 0x040026C0 RID: 9920
	Private damageDealer As DamageDealer

	' Token: 0x040026C1 RID: 9921
	Private damageReceiver As DamageReceiver

	' Token: 0x040026C2 RID: 9922
	Private patternCoroutine As Coroutine

	' Token: 0x020005E6 RID: 1510
	Public Enum State
		' Token: 0x040026C4 RID: 9924
		Intro
		' Token: 0x040026C5 RID: 9925
		Idle
		' Token: 0x040026C6 RID: 9926
		Twirl
		' Token: 0x040026C7 RID: 9927
		Marble
		' Token: 0x040026C8 RID: 9928
		Death
	End Enum
End Class
