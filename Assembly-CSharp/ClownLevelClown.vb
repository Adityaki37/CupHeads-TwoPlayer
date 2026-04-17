Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000558 RID: 1368
Public Class ClownLevelClown
	Inherits LevelProperties.Clown.Entity

	' Token: 0x17000345 RID: 837
	' (get) Token: 0x06001987 RID: 6535 RVA: 0x000E7BFD File Offset: 0x000E5FFD
	' (set) Token: 0x06001988 RID: 6536 RVA: 0x000E7C05 File Offset: 0x000E6005
	Public Property state As ClownLevelClown.State

	' Token: 0x06001989 RID: 6537 RVA: 0x000E7C0E File Offset: 0x000E600E
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
	End Sub

	' Token: 0x0600198A RID: 6538 RVA: 0x000E7C44 File Offset: 0x000E6044
	Private Sub Start()
		Me.state = ClownLevelClown.State.BumperCar
		Me.notDashing = True
		MyBase.StartCoroutine(Me.intro_cr())
	End Sub

	' Token: 0x0600198B RID: 6539 RVA: 0x000E7C61 File Offset: 0x000E6061
	Public Overrides Sub LevelInit(properties As LevelProperties.Clown)
		MyBase.LevelInit(properties)
	End Sub

	' Token: 0x0600198C RID: 6540 RVA: 0x000E7C6A File Offset: 0x000E606A
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		MyBase.properties.DealDamage(info.damage)
	End Sub

	' Token: 0x0600198D RID: 6541 RVA: 0x000E7C7D File Offset: 0x000E607D
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If Me.state <> ClownLevelClown.State.Helium AndAlso phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x0600198E RID: 6542 RVA: 0x000E7CA7 File Offset: 0x000E60A7
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x0600198F RID: 6543 RVA: 0x000E7CBF File Offset: 0x000E60BF
	Protected Overridable Function hitPauseCoefficient() As Single
		Return If((Not MyBase.GetComponent(Of DamageReceiver)().IsHitPaused), 1F, 0F)
	End Function

	' Token: 0x06001990 RID: 6544 RVA: 0x000E7CE0 File Offset: 0x000E60E0
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.regularDuck = Nothing
		Me.pinkDuck = Nothing
		Me.bombDuck = Nothing
	End Sub

	' Token: 0x06001991 RID: 6545 RVA: 0x000E7D00 File Offset: 0x000E6100
	Private Iterator Function intro_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 2F)
		MyBase.animator.SetTrigger("Continue")
		AudioManager.Play("clown_intro_continue")
		Me.emitAudioFromObject.Add("clown_intro_continue")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Intro_End", False, True)
		Me.StartBumperCar()
		Return
	End Function

	' Token: 0x06001992 RID: 6546 RVA: 0x000E7D1B File Offset: 0x000E611B
	Public Sub StartBumperCar()
		Me.state = ClownLevelClown.State.BumperCar
		MyBase.animator.SetBool("BumperDeath", False)
		MyBase.StartCoroutine(Me.bumper_car_cr())
		MyBase.StartCoroutine(Me.ducks_cr())
	End Sub

	' Token: 0x06001993 RID: 6547 RVA: 0x000E7D4F File Offset: 0x000E614F
	Public Sub EndBumperCar()
		MyBase.animator.SetBool("BumperDeath", True)
	End Sub

	' Token: 0x06001994 RID: 6548 RVA: 0x000E7D62 File Offset: 0x000E6162
	Private Sub SwitchLayer()
		MyBase.GetComponent(Of SpriteRenderer)().sortingLayerName = "Background"
		MyBase.GetComponent(Of SpriteRenderer)().sortingOrder = 101
	End Sub

	' Token: 0x06001995 RID: 6549 RVA: 0x000E7D84 File Offset: 0x000E6184
	Private Iterator Function end_bumper_car_cr() As IEnumerator
		AudioManager.Play("clown_bumper_death")
		Me.emitAudioFromObject.Add("clown_bumper_death")
		While MyBase.transform.position.y > -660F
			If CupheadTime.Delta IsNot 0F Then
				MyBase.transform.position += New Vector2(-300F, Me.fallAccumulatedGravity) * CupheadTime.Delta
				Me.fallAccumulatedGravity += -100F
			End If
			Yield Nothing
		End While
		Me.clownHelium.StartHeliumTank()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Yield Nothing
		Return
	End Function

	' Token: 0x06001996 RID: 6550 RVA: 0x000E7DA0 File Offset: 0x000E61A0
	Private Iterator Function dash_timer_cr(delayPattern As String()) As IEnumerator
		Dim waitTime As Single
		Parser.FloatTryParse(delayPattern(Me.timerIndex), waitTime)
		Yield CupheadTime.WaitForSeconds(Me, waitTime)
		Me.notDashing = False
		Me.timerIndex = (Me.timerIndex + 1) Mod delayPattern.Length
		Yield Nothing
		Return
	End Function

	' Token: 0x06001997 RID: 6551 RVA: 0x000E7DC4 File Offset: 0x000E61C4
	Private Iterator Function bumper_car_cr() As IEnumerator
		Me.notDashing = True
		Dim isFlipped As Boolean = False
		Dim bumperPos As Vector3 = MyBase.transform.position
		Dim offsetDash As Single = 150F
		Dim offsetMove As Single = 250F
		Dim p As LevelProperties.Clown.BumperCar = MyBase.properties.CurrentState.bumperCar
		Dim movementPattern As String() = p.movementStrings.GetRandom().Split(New Char() { ","c })
		Dim dashDelayPattern As String() = p.attackDelayString.GetRandom().Split(New Char() { ","c })
		Dim t As Single = 0F
		Dim speed As Single = p.movementSpeed
		Dim movementIndex As Integer = Global.UnityEngine.Random.Range(0, movementPattern.Length)
		Me.timerIndex = Global.UnityEngine.Random.Range(0, dashDelayPattern.Length)
		MyBase.StartCoroutine(Me.dash_timer_cr(dashDelayPattern))
		Me.emitAudioFromObject.Add("clown_bumper_move")
		Me.emitAudioFromObject.Add("clown_dash_start")
		Me.emitAudioFromObject.Add("clown_dash_end")
		While True
			If Me.notDashing Then
				If movementPattern(movementIndex)(0) = "F"c Then
					MyBase.animator.SetTrigger("Move")
					While t < p.movementDuration AndAlso Me.notDashing AndAlso Not Me.[stop]
						speed = If((Not isFlipped), (-p.movementSpeed), p.movementSpeed)
						MyBase.transform.AddPosition(speed * CupheadTime.Delta, 0F, 0F)
						t += CupheadTime.Delta
						Yield Nothing
					End While
					AudioManager.Play("clown_bumper_move")
					If Me.notDashing Then
						Yield CupheadTime.WaitForSeconds(Me, p.movementDelay)
					End If
				ElseIf movementPattern(movementIndex)(0) = "B"c Then
					MyBase.animator.SetTrigger("Move")
					While t < p.movementDuration AndAlso Me.notDashing AndAlso Not Me.[stop]
						speed = If((Not isFlipped), p.movementSpeed, (-p.movementSpeed))
						If MyBase.transform.position.x >= CSng(Level.Current.Left) + offsetDash AndAlso MyBase.transform.position.x <= CSng(Level.Current.Right) - offsetDash Then
							MyBase.transform.AddPosition(speed * CupheadTime.Delta, 0F, 0F)
							t += CupheadTime.Delta
							Yield Nothing
						End If
						Yield Nothing
					End While
					AudioManager.Play("clown_bumper_move")
					If Me.notDashing Then
						Yield CupheadTime.WaitForSeconds(Me, p.movementDelay)
					End If
				End If
				Me.[stop] = False
				t = 0F
				movementIndex = (movementIndex + 1) Mod movementPattern.Length
				Yield Nothing
			Else
				Dim dist As Single = 640F - MyBase.transform.position.x
				If dist < 50F Then
					MyBase.animator.Play("Move_Forward")
					While t < p.movementDuration
						speed = If((Not isFlipped), (-p.movementSpeed), p.movementSpeed)
						MyBase.transform.AddPosition(speed * CupheadTime.Delta, 0F, 0F)
						t += CupheadTime.Delta
						Yield Nothing
					End While
				End If
				AudioManager.Play("clown_dash_start")
				MyBase.animator.Play("Dash_Start")
				Yield CupheadTime.WaitForSeconds(Me, p.bumperDashWarning)
				MyBase.animator.SetTrigger("Continue")
				Dim endPos As Single = If(isFlipped, (CSng(Level.Current.Right) - offsetMove), (CSng(Level.Current.Left) + offsetMove))
				While MyBase.transform.position.x <> endPos
					bumperPos.x = Mathf.MoveTowards(MyBase.transform.position.x, endPos, p.dashSpeed * CupheadTime.Delta * Me.hitPauseCoefficient())
					MyBase.transform.position = bumperPos
					Yield Nothing
				End While
				AudioManager.Play("clown_dash_end")
				MyBase.animator.SetTrigger("End")
				isFlipped = Not isFlipped
				Yield MyBase.animator.WaitForAnimationToEnd(Me, "Dash_End", False, True)
				Me.notDashing = True
				t = 0F
				MyBase.StartCoroutine(Me.dash_timer_cr(dashDelayPattern))
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06001998 RID: 6552 RVA: 0x000E7DE0 File Offset: 0x000E61E0
	Private Sub FlipSprite()
		MyBase.transform.SetScale(New Single?(-MyBase.transform.localScale.x), New Single?(1F), New Single?(1F))
	End Sub

	' Token: 0x06001999 RID: 6553 RVA: 0x000E7E28 File Offset: 0x000E6228
	Private Sub MoveAStop()
		Dim vector As Vector2 = MyBase.transform.position
		Me.[stop] = True
		vector.y = Me.forwardYPos.position.y
		MyBase.transform.position = vector
	End Sub

	' Token: 0x0600199A RID: 6554 RVA: 0x000E7E78 File Offset: 0x000E6278
	Private Sub MoveBStop()
		Me.[stop] = True
	End Sub

	' Token: 0x0600199B RID: 6555 RVA: 0x000E7E84 File Offset: 0x000E6284
	Private Sub AnimationOffsetUp()
		Dim vector As Vector2 = MyBase.transform.position
		vector.y = Me.forwardYPos.position.y
		MyBase.transform.position = vector
	End Sub

	' Token: 0x0600199C RID: 6556 RVA: 0x000E7ED0 File Offset: 0x000E62D0
	Private Sub SpawnDuck(prefab As ClownLevelDucks, startPercent As Single)
		If prefab IsNot Nothing Then
			Dim num As Single = 100F
			Dim duck As LevelProperties.Clown.Duck = MyBase.properties.CurrentState.duck
			Dim num2 As Single = duck.duckYHeightRange.RandomFloat()
			Dim num3 As Single = startPercent / 100F * duck.duckYHeightRange.max
			Dim vector As Vector2 = Vector3.zero
			vector.y = 360F - num3
			vector.x = 640F + num
			Dim clownLevelDucks As ClownLevelDucks = Global.UnityEngine.[Object].Instantiate(Of ClownLevelDucks)(prefab).Init(vector, MyBase.properties.CurrentState.duck, num2, duck.duckYMovementSpeed)
			clownLevelDucks.Init(vector, MyBase.properties.CurrentState.duck, num2, duck.duckYMovementSpeed)
		End If
	End Sub

	' Token: 0x0600199D RID: 6557 RVA: 0x000E7F90 File Offset: 0x000E6390
	Private Iterator Function ducks_cr() As IEnumerator
		Dim p As LevelProperties.Clown.Duck = MyBase.properties.CurrentState.duck
		Dim positionPattern As String() = p.duckYStartPercentString.GetRandom().Split(New Char() { ","c })
		Dim typePattern As String() = p.duckTypeString.GetRandom().Split(New Char() { ","c })
		Dim typeIndex As Integer = Global.UnityEngine.Random.Range(0, typePattern.Length)
		Dim posPercentIndex As Integer = Global.UnityEngine.Random.Range(0, positionPattern.Length)
		While True
			Dim spawnY As Single = 0F
			Parser.FloatTryParse(positionPattern(posPercentIndex), spawnY)
			Dim toSpawn As ClownLevelDucks = Nothing
			If typePattern(typeIndex)(0) = "R"c Then
				toSpawn = Me.regularDuck
			ElseIf typePattern(typeIndex)(0) = "P"c Then
				toSpawn = Me.pinkDuck
			ElseIf typePattern(typeIndex)(0) = "B"c Then
				toSpawn = Me.bombDuck
			End If
			If Me.state <> ClownLevelClown.State.Death Then
				Me.SpawnDuck(toSpawn, spawnY)
			End If
			Yield CupheadTime.WaitForSeconds(Me, p.duckDelay)
			typeIndex = (typeIndex + 1) Mod typePattern.Length
			posPercentIndex = (posPercentIndex + 1) Mod positionPattern.Length
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x040022A6 RID: 8870
	Private Const FALL_GRAVITY As Single = -100F

	' Token: 0x040022A8 RID: 8872
	<SerializeField()>
	Private forwardYPos As Transform

	' Token: 0x040022A9 RID: 8873
	<SerializeField()>
	Private regularDuck As ClownLevelDucks

	' Token: 0x040022AA RID: 8874
	<SerializeField()>
	Private pinkDuck As ClownLevelDucks

	' Token: 0x040022AB RID: 8875
	<SerializeField()>
	Private bombDuck As ClownLevelDucks

	' Token: 0x040022AC RID: 8876
	<SerializeField()>
	Private clownHelium As ClownLevelClownHelium

	' Token: 0x040022AD RID: 8877
	Private notDashing As Boolean

	' Token: 0x040022AE RID: 8878
	Private firstSelection As Boolean

	' Token: 0x040022AF RID: 8879
	Private [stop] As Boolean

	' Token: 0x040022B0 RID: 8880
	Private speed As Single

	' Token: 0x040022B1 RID: 8881
	Private fallAccumulatedGravity As Single

	' Token: 0x040022B2 RID: 8882
	Private timerIndex As Integer

	' Token: 0x040022B3 RID: 8883
	Private damageDealer As DamageDealer

	' Token: 0x040022B4 RID: 8884
	Private damageReceiver As DamageReceiver

	' Token: 0x040022B5 RID: 8885
	Private damageReceiverChild As DamageReceiver

	' Token: 0x040022B6 RID: 8886
	Private fallVelocity As Vector2

	' Token: 0x02000559 RID: 1369
	Public Enum State
		' Token: 0x040022B8 RID: 8888
		BumperCar
		' Token: 0x040022B9 RID: 8889
		Helium
		' Token: 0x040022BA RID: 8890
		Death
	End Enum
End Class
