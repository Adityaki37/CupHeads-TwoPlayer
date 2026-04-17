Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200080C RID: 2060
Public Class TrainLevelBlindSpecterEyeProjectile
	Inherits AbstractProjectile

	' Token: 0x06002FB1 RID: 12209 RVA: 0x001C4128 File Offset: 0x001C2528
	Public Function Create(pos As Vector2, time As Vector2, y As Single, flipped As Boolean, health As Single) As TrainLevelBlindSpecterEyeProjectile
		Dim trainLevelBlindSpecterEyeProjectile As TrainLevelBlindSpecterEyeProjectile = TryCast(MyBase.Create(), TrainLevelBlindSpecterEyeProjectile)
		trainLevelBlindSpecterEyeProjectile.transform.position = pos
		trainLevelBlindSpecterEyeProjectile.time = time
		trainLevelBlindSpecterEyeProjectile.[end] = y
		trainLevelBlindSpecterEyeProjectile.health = health
		If flipped Then
			trainLevelBlindSpecterEyeProjectile.sprite.transform.SetScale(New Single?(-1F), Nothing, Nothing)
		End If
		Return trainLevelBlindSpecterEyeProjectile
	End Function

	' Token: 0x06002FB2 RID: 12210 RVA: 0x001C419C File Offset: 0x001C259C
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.startPos = MyBase.transform.position.y
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		MyBase.StartCoroutine(Me.x_cr())
		MyBase.StartCoroutine(Me.y_cr())
		Dim trainLevel As TrainLevel = TryCast(Level.Current, TrainLevel)
		If trainLevel IsNot Nothing Then
			Me.handCarCollider = trainLevel.handCarCollider
		End If
	End Sub

	' Token: 0x06002FB3 RID: 12211 RVA: 0x001C4228 File Offset: 0x001C2628
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If Me.damageDealer IsNot Nothing AndAlso phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06002FB4 RID: 12212 RVA: 0x001C4251 File Offset: 0x001C2651
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.health -= info.damage
		If Me.health <= 0F Then
			Me.Die()
		End If
	End Sub

	' Token: 0x06002FB5 RID: 12213 RVA: 0x001C427C File Offset: 0x001C267C
	Private Sub [End]()
		Me.StopAllCoroutines()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06002FB6 RID: 12214 RVA: 0x001C428F File Offset: 0x001C268F
	Protected Overrides Sub Die()
		If Not MyBase.GetComponent(Of Collider2D)().enabled Then
			Return
		End If
		MyBase.Die()
		Me.StopAllCoroutines()
	End Sub

	' Token: 0x06002FB7 RID: 12215 RVA: 0x001C42B0 File Offset: 0x001C26B0
	Protected Overrides Sub OnCollisionGround(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionGround(hit, phase)
		If phase = CollisionPhase.Enter AndAlso hit.GetComponent(Of TrainLevelPlatform)() IsNot Nothing Then
			Me.start = hit.transform.position.y + 20F
			Me.t = 1000F
		End If
	End Sub

	' Token: 0x06002FB8 RID: 12216 RVA: 0x001C4308 File Offset: 0x001C2708
	Private Iterator Function x_cr() As IEnumerator
		Dim start As Single = MyBase.transform.position.x
		Dim t As Single = 0F
		While t < Me.time.x
			Dim val As Single = t / Me.time.x
			Dim x As Single = Mathf.Lerp(start, -740F, val)
			MyBase.transform.SetPosition(New Single?(x), Nothing, Nothing)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		Me.[End]()
		Return
	End Function

	' Token: 0x06002FB9 RID: 12217 RVA: 0x001C4324 File Offset: 0x001C2724
	Private Iterator Function y_cr() As IEnumerator
		Dim counter As Integer = 0
		Dim maxCounter As Integer = 2
		Dim frameTime As Single = 0F
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		Me.start = MyBase.transform.position.y
		While True
			AudioManager.Play("train_blindspector_eye_bounce")
			Me.emitAudioFromObject.Add("train_blindspector_eye_bounce")
			Me.t = 0F
			Dim newY As Single = Me.start
			If Me.handCarCollider IsNot Nothing Then
				Physics2D.IgnoreCollision(Me.handCarCollider, Me.eyeCollider, True)
			End If
			While Me.t < Me.time.y
				Dim val As Single = Me.t / Me.time.y
				newY = EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, Me.start, Me.[end], val)
				MyBase.transform.SetPosition(Nothing, New Single?(newY), Nothing)
				Me.t += CupheadTime.FixedDelta
				Yield wait
			End While
			MyBase.transform.SetPosition(Nothing, New Single?(Me.[end]), Nothing)
			Me.start = Me.startPos
			Yield Nothing
			If Me.handCarCollider IsNot Nothing Then
				Physics2D.IgnoreCollision(Me.handCarCollider, Me.eyeCollider, False)
			End If
			Me.t = 0F
			While Me.t < Me.time.y
				Dim val2 As Single = Me.t / Me.time.y
				newY = EaseUtils.Ease(EaseUtils.EaseType.easeInSine, Me.[end], Me.start, val2)
				MyBase.transform.SetPosition(Nothing, New Single?(newY), Nothing)
				Me.t += CupheadTime.FixedDelta
				Yield wait
			End While
			MyBase.transform.SetPosition(Nothing, New Single?(Me.start), Nothing)
			Me.effectPrefab.Create(MyBase.transform.position)
			While counter < maxCounter
				frameTime += CupheadTime.FixedDelta
				If frameTime > 0.041666668F Then
					counter += 1
					frameTime -= 0.041666668F
					If counter >= 2 Then
						MyBase.transform.SetScale(Nothing, New Single?(0.3F), Nothing)
						Exit While
					End If
					MyBase.transform.SetScale(Nothing, New Single?(0.5F), Nothing)
				End If
				Yield wait
			End While
			counter = 0
			MyBase.transform.SetScale(Nothing, New Single?(1F), Nothing)
			Yield wait
		End While
		Return
	End Function

	' Token: 0x06002FBA RID: 12218 RVA: 0x001C433F File Offset: 0x001C273F
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.effectPrefab = Nothing
	End Sub

	' Token: 0x0400388C RID: 14476
	Private Const FRAME_TIME As Single = 0.041666668F

	' Token: 0x0400388D RID: 14477
	<SerializeField()>
	Private effectPrefab As Effect

	' Token: 0x0400388E RID: 14478
	<SerializeField()>
	Private sprite As Transform

	' Token: 0x0400388F RID: 14479
	<SerializeField()>
	Private eyeCollider As Collider2D

	' Token: 0x04003890 RID: 14480
	Private damageReceiver As DamageReceiver

	' Token: 0x04003891 RID: 14481
	Private health As Single

	' Token: 0x04003892 RID: 14482
	Private startPos As Single

	' Token: 0x04003893 RID: 14483
	Private t As Single

	' Token: 0x04003894 RID: 14484
	Private start As Single

	' Token: 0x04003895 RID: 14485
	Private [end] As Single

	' Token: 0x04003896 RID: 14486
	Private time As Vector2

	' Token: 0x04003897 RID: 14487
	Private handCarCollider As Collider2D
End Class
