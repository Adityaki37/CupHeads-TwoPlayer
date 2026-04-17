Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200055A RID: 1370
Public Class ClownLevelClownHelium
	Inherits LevelProperties.Clown.Entity

	' Token: 0x17000346 RID: 838
	' (get) Token: 0x0600199F RID: 6559 RVA: 0x000E8DBD File Offset: 0x000E71BD
	' (set) Token: 0x060019A0 RID: 6560 RVA: 0x000E8DC5 File Offset: 0x000E71C5
	Public Property state As ClownLevelClownHelium.State

	' Token: 0x060019A1 RID: 6561 RVA: 0x000E8DD0 File Offset: 0x000E71D0
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageReceiver = Me.head.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		Me.head.GetComponent(Of Collider2D)().enabled = False
	End Sub

	' Token: 0x060019A2 RID: 6562 RVA: 0x000E8E1C File Offset: 0x000E721C
	Public Overrides Sub LevelInit(properties As LevelProperties.Clown)
		MyBase.LevelInit(properties)
		Me.pivotPoint.transform.position = Me.heliumStopPos.transform.position
		Me.headMoving = True
	End Sub

	' Token: 0x060019A3 RID: 6563 RVA: 0x000E8E4C File Offset: 0x000E724C
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		MyBase.properties.DealDamage(info.damage)
	End Sub

	' Token: 0x060019A4 RID: 6564 RVA: 0x000E8E5F File Offset: 0x000E725F
	Protected Overridable Function hitPauseCoefficient() As Single
		Return If((Not MyBase.GetComponent(Of DamageReceiver)().IsHitPaused), 1F, 0F)
	End Function

	' Token: 0x060019A5 RID: 6565 RVA: 0x000E8E80 File Offset: 0x000E7280
	Public Sub StartHeliumTank()
		Me.StopAllCoroutines()
		Me.state = ClownLevelClownHelium.State.Helium
		MyBase.StartCoroutine(Me.helium_tank_intro_cr())
	End Sub

	' Token: 0x060019A6 RID: 6566 RVA: 0x000E8E9C File Offset: 0x000E729C
	Private Iterator Function helium_tank_intro_cr() As IEnumerator
		MyBase.gameObject.GetComponent(Of SpriteRenderer)().sortingOrder = 0
		Dim t As Single = 0F
		Dim time As Single = 5F
		Dim start As Vector2 = MyBase.transform.position
		While t < time
			Dim val As Single = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0F, 1F, t / time)
			MyBase.transform.position = Vector2.Lerp(start, Me.heliumStopPos.position, val)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		MyBase.transform.position = Me.heliumStopPos.position
		MyBase.StartCoroutine(Me.helium_tank_cr())
		MyBase.StartCoroutine(Me.tank_effects_cr())
		MyBase.StartCoroutine(Me.pipe_puffs_cr())
		Yield Nothing
		Return
	End Function

	' Token: 0x060019A7 RID: 6567 RVA: 0x000E8EB8 File Offset: 0x000E72B8
	Private Sub SpawnBalloonDogs(dogPrefab As ClownLevelDogBalloon, startPos As Vector3, isFlipped As Boolean)
		Dim heliumClown As LevelProperties.Clown.HeliumClown = MyBase.properties.CurrentState.heliumClown
		Dim [next] As AbstractPlayerController = PlayerManager.GetNext()
		If dogPrefab IsNot Nothing Then
			Dim clownLevelDogBalloon As ClownLevelDogBalloon = Global.UnityEngine.[Object].Instantiate(Of ClownLevelDogBalloon)(dogPrefab)
			clownLevelDogBalloon.Init(heliumClown.dogHP, startPos, heliumClown.dogSpeed, [next], heliumClown, isFlipped)
		End If
	End Sub

	' Token: 0x060019A8 RID: 6568 RVA: 0x000E8F0A File Offset: 0x000E730A
	Private Sub HeliumTankSFX()
		AudioManager.Play("clown_helium_tanks")
		Me.emitAudioFromObject.Add("clown_helium_tanks")
	End Sub

	' Token: 0x060019A9 RID: 6569 RVA: 0x000E8F28 File Offset: 0x000E7328
	Private Iterator Function helium_tank_cr() As IEnumerator
		Me.emitAudioFromObject.Add("clown_helium_tanks")
		AudioManager.Play("clown_helium_intro_continue")
		Me.emitAudioFromObject.Add("clown_helium_intro_continue")
		AudioManager.Play("clown_helium_extend_pipes")
		Me.emitAudioFromObject.Add("clown_helium_extend_pipes")
		MyBase.animator.SetTrigger("Continue")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Helium_Intro_End", 3, False, True)
		Dim p As LevelProperties.Clown.HeliumClown = MyBase.properties.CurrentState.heliumClown
		Dim spawnPattern As String() = p.dogSpawnOrder.GetRandom().Split(New Char() { ","c })
		Dim delayPattern As String() = p.dogDelayString.GetRandom().Split(New Char() { ","c })
		Dim typePattern As String() = p.dogTypeString.GetRandom().Split(New Char() { ","c })
		Dim pickedPipePos As Vector3 = Vector3.zero
		Dim waitTime As Single = 0F
		Dim isFlipped As Boolean = False
		Dim spawnIndex As Integer = Global.UnityEngine.Random.Range(0, spawnPattern.Length)
		Dim delayIndex As Integer = Global.UnityEngine.Random.Range(0, delayPattern.Length)
		Dim typeIndex As Integer = Global.UnityEngine.Random.Range(0, typePattern.Length)
		While True
			Dim toSpawn As ClownLevelDogBalloon = Nothing
			Dim nextPos As String() = spawnPattern(spawnIndex).Split(New Char() { "-"c })
			For Each text As String In nextPos
				Dim pipeSelection As Integer
				Parser.IntTryParse(text, pipeSelection)
				For Each pipePositions As ClownLevelClownHelium.PipePositions In Me.pipePositions
					If pipePositions.orderNum = pipeSelection Then
						pickedPipePos = pipePositions.pipeEntrance.position
						isFlipped = pipeSelection > 3
					End If
				Next
				If typePattern(typeIndex)(0) = "R"c Then
					toSpawn = Me.regularDog
				ElseIf typePattern(typeIndex)(0) = "P"c Then
					toSpawn = Me.pinkDog
				End If
				Me.SpawnBalloonDogs(toSpawn, pickedPipePos, isFlipped)
				typeIndex = (typeIndex + 1) Mod typePattern.Length
			Next
			Parser.FloatTryParse(delayPattern(delayIndex), waitTime)
			Yield CupheadTime.WaitForSeconds(Me, waitTime)
			spawnIndex = (spawnIndex + 1) Mod spawnPattern.Length
			delayIndex = (delayIndex + 1) Mod delayPattern.Length
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060019AA RID: 6570 RVA: 0x000E8F44 File Offset: 0x000E7344
	Private Iterator Function pipe_puffs_cr() As IEnumerator
		Dim order As String = "0,5,1,4,2,3,5,1,2,3"
		Dim orderIndex As Integer = Global.UnityEngine.Random.Range(0, order.Split(New Char() { ","c }).Length)
		While True
			Yield CupheadTime.WaitForSeconds(Me, Global.UnityEngine.Random.Range(0.16F, 0.65F))
			Me.pipePositions(Parser.IntParse(order.Split(New Char() { ","c })(orderIndex))).pipeEntrance.GetComponent(Of Animator)().SetInteger("Type", Global.UnityEngine.Random.Range(0, 3))
			Me.pipePositions(Parser.IntParse(order.Split(New Char() { ","c })(orderIndex))).pipeEntrance.GetComponent(Of Animator)().SetTrigger("OnPuff")
			orderIndex = (orderIndex + 1) Mod order.Split(New Char() { ","c }).Length
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060019AB RID: 6571 RVA: 0x000E8F60 File Offset: 0x000E7360
	Private Iterator Function tank_effects_cr() As IEnumerator
		Dim isRight As Boolean = Rand.Bool()
		While True
			Yield CupheadTime.WaitForSeconds(Me, Global.UnityEngine.Random.Range(0.16F, 0.85F))
			Me.tankEffects.SetBool("isLeft", isRight)
			Me.tankEffects.SetTrigger("OnPuff")
			isRight = Not isRight
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060019AC RID: 6572 RVA: 0x000E8F7B File Offset: 0x000E737B
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.regularDog = Nothing
		Me.pinkDog = Nothing
	End Sub

	' Token: 0x060019AD RID: 6573 RVA: 0x000E8F94 File Offset: 0x000E7394
	Private Sub SetHead()
		Me.pivotPoint.transform.position = Me.head.transform.position
		Me.head.GetComponent(Of Collider2D)().enabled = True
		MyBase.animator.SetTrigger("Head")
		MyBase.StartCoroutine(Me.head_moving_cr())
	End Sub

	' Token: 0x060019AE RID: 6574 RVA: 0x000E8FEF File Offset: 0x000E73EF
	Private Sub SetBody()
		MyBase.animator.Play("Helium_Idle")
	End Sub

	' Token: 0x060019AF RID: 6575 RVA: 0x000E9004 File Offset: 0x000E7404
	Private Iterator Function head_moving_cr() As IEnumerator
		While True
			If Me.headMoving Then
				Me.PathMovement()
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060019B0 RID: 6576 RVA: 0x000E9020 File Offset: 0x000E7420
	Private Sub PathMovement()
		Me.angle += 1.8F * CupheadTime.Delta * Me.hitPauseCoefficient()
		Dim vector As Vector3 = New Vector3(-Mathf.Sin(Me.angle) * Me.loopSize, 0F, 0F)
		Dim vector2 As Vector3 = New Vector3(0F, Mathf.Cos(Me.angle) * Me.loopSize, 0F)
		Me.head.transform.position = Me.pivotPoint.position
		Me.head.transform.position += vector + vector2
	End Sub

	' Token: 0x060019B1 RID: 6577 RVA: 0x000E90D5 File Offset: 0x000E74D5
	Public Sub StartDeath()
		Me.StopAllCoroutines()
		MyBase.StartCoroutine(Me.death_cr())
	End Sub

	' Token: 0x060019B2 RID: 6578 RVA: 0x000E90EC File Offset: 0x000E74EC
	Private Iterator Function death_cr() As IEnumerator
		Me.head.GetComponent(Of Collider2D)().enabled = False
		MyBase.StartCoroutine(Me.head_moving_cr())
		MyBase.gameObject.GetComponent(Of SpriteRenderer)().sortingOrder = 0
		Me.head.transform.parent = Nothing
		MyBase.StartCoroutine(Me.head_death_cr())
		Dim moveSpeed As Single = MyBase.properties.CurrentState.heliumClown.heliumMoveSpeed
		Dim acceleration As Single = MyBase.properties.CurrentState.heliumClown.heliumAcceleration
		Dim endPos As Single = -860F
		While MyBase.transform.position.y > endPos
			moveSpeed += acceleration
			MyBase.transform.AddPosition(0F, -moveSpeed * CupheadTime.Delta, 0F)
			Yield Nothing
		End While
		Yield Nothing
		Return
	End Function

	' Token: 0x060019B3 RID: 6579 RVA: 0x000E9108 File Offset: 0x000E7508
	Private Iterator Function head_death_cr() As IEnumerator
		Me.StartExplosions()
		Dim moveSpeed As Single = MyBase.properties.CurrentState.heliumClown.heliumMoveSpeed
		Dim acceleration As Single = MyBase.properties.CurrentState.heliumClown.heliumAcceleration
		Dim endPos As Single = 1060F
		Dim t As Single = 0F
		Dim time As Single = 1F
		Dim start As Vector2 = Me.head.transform.position
		Dim [end] As Vector2 = New Vector3(Me.head.transform.position.x, Me.heliumStopPos.transform.position.y - 50F, 0F)
		Me.headMoving = False
		MyBase.animator.SetTrigger("Dead")
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		While t < time
			Dim val As Single = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0F, 1F, t / time)
			Me.head.transform.position = Vector2.Lerp(start, [end], val)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		Yield CupheadTime.WaitForSeconds(Me, 0.3F)
		While Me.head.transform.position.y < endPos
			If CupheadTime.Delta IsNot 0F Then
				moveSpeed += acceleration
				Me.head.transform.AddPosition(0F, moveSpeed * CupheadTime.Delta, 0F)
			End If
			Yield Nothing
		End While
		Me.EndExplosions()
		Me.clownHorse.StartCarouselHorse()
		Global.UnityEngine.[Object].Destroy(Me.head.gameObject)
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Yield Nothing
		Return
	End Function

	' Token: 0x060019B4 RID: 6580 RVA: 0x000E9123 File Offset: 0x000E7523
	Private Sub StartExplosions()
		Me.head.GetComponent(Of LevelBossDeathExploder)().StartExplosion()
	End Sub

	' Token: 0x060019B5 RID: 6581 RVA: 0x000E9135 File Offset: 0x000E7535
	Private Sub EndExplosions()
		Me.head.GetComponent(Of LevelBossDeathExploder)().StopExplosions()
	End Sub

	' Token: 0x040022BC RID: 8892
	<SerializeField()>
	Private tankEffects As Animator

	' Token: 0x040022BD RID: 8893
	<SerializeField()>
	Private clownHorse As ClownLevelClownHorse

	' Token: 0x040022BE RID: 8894
	<SerializeField()>
	Private head As GameObject

	' Token: 0x040022BF RID: 8895
	<SerializeField()>
	Private pivotPoint As Transform

	' Token: 0x040022C0 RID: 8896
	<SerializeField()>
	Private heliumStopPos As Transform

	' Token: 0x040022C1 RID: 8897
	<SerializeField()>
	Private pipePositions As ClownLevelClownHelium.PipePositions()

	' Token: 0x040022C2 RID: 8898
	<SerializeField()>
	Private regularDog As ClownLevelDogBalloon

	' Token: 0x040022C3 RID: 8899
	<SerializeField()>
	Private pinkDog As ClownLevelDogBalloon

	' Token: 0x040022C4 RID: 8900
	Private damageReceiver As DamageReceiver

	' Token: 0x040022C5 RID: 8901
	Private headMoving As Boolean

	' Token: 0x040022C6 RID: 8902
	Private angle As Single

	' Token: 0x040022C7 RID: 8903
	Private loopSize As Single = 10F

	' Token: 0x0200055B RID: 1371
	Public Enum State
		' Token: 0x040022C9 RID: 8905
		BumperCar
		' Token: 0x040022CA RID: 8906
		Helium
		' Token: 0x040022CB RID: 8907
		Death
	End Enum

	' Token: 0x0200055C RID: 1372
	<Serializable()>
	Public Class PipePositions
		' Token: 0x040022CC RID: 8908
		Public pipeEntrance As Transform

		' Token: 0x040022CD RID: 8909
		Public orderNum As Integer
	End Class
End Class
