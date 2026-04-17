Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200050E RID: 1294
Public Class BeeLevelAirplane
	Inherits LevelProperties.Bee.Entity

	' Token: 0x17000326 RID: 806
	' (get) Token: 0x060016F1 RID: 5873 RVA: 0x000CE3A2 File Offset: 0x000CC7A2
	' (set) Token: 0x060016F2 RID: 5874 RVA: 0x000CE3AA File Offset: 0x000CC7AA
	Public Property state As BeeLevelAirplane.State

	' Token: 0x060016F3 RID: 5875 RVA: 0x000CE3B4 File Offset: 0x000CC7B4
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.state = BeeLevelAirplane.State.Unspawned
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		AddHandler Me.midLayer.GetComponentInChildren(Of CollisionChild)().OnPlayerCollision, AddressOf Me.OnCollisionPlayer
		AddHandler Me.topLayer.GetComponent(Of CollisionChild)().OnPlayerCollision, AddressOf Me.OnCollisionPlayer
	End Sub

	' Token: 0x060016F4 RID: 5876 RVA: 0x000CE438 File Offset: 0x000CC838
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		MyBase.properties.DealDamage(info.damage)
		If MyBase.properties.CurrentHealth <= 0F AndAlso Me.state <> BeeLevelAirplane.State.Dead Then
			Me.state = BeeLevelAirplane.State.Dead
			Me.Dead()
		End If
	End Sub

	' Token: 0x060016F5 RID: 5877 RVA: 0x000CE484 File Offset: 0x000CC884
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x060016F6 RID: 5878 RVA: 0x000CE4A2 File Offset: 0x000CC8A2
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x060016F7 RID: 5879 RVA: 0x000CE4BA File Offset: 0x000CC8BA
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.bullet = Nothing
	End Sub

	' Token: 0x060016F8 RID: 5880 RVA: 0x000CE4C9 File Offset: 0x000CC8C9
	Public Sub StartIntro()
		Me.state = BeeLevelAirplane.State.Intro
		MyBase.StartCoroutine(Me.intro_cr())
	End Sub

	' Token: 0x060016F9 RID: 5881 RVA: 0x000CE4E0 File Offset: 0x000CC8E0
	Private Iterator Function intro_cr() As IEnumerator
		Dim speed As Single = 400F
		Me.countPattern = MyBase.properties.CurrentState.wingSwipe.attackCount.GetRandom().Split(New Char() { ","c })
		Me.countIndex = Global.UnityEngine.Random.Range(0, Me.countPattern.Length)
		While MyBase.transform.position.y < -60F
			MyBase.transform.AddPosition(0F, speed * CupheadTime.Delta, 0F)
			Yield Nothing
		End While
		Me.state = BeeLevelAirplane.State.Idle
		MyBase.StartCoroutine(Me.move_cr())
		Yield Nothing
		Return
	End Function

	' Token: 0x060016FA RID: 5882 RVA: 0x000CE4FB File Offset: 0x000CC8FB
	Private Sub IdleCount()
		Me.blinkCount += 1
		If Me.blinkCount >= Me.blinkCountMax Then
			Me.blinkOne = Rand.Bool()
		End If
	End Sub

	' Token: 0x060016FB RID: 5883 RVA: 0x000CE528 File Offset: 0x000CC928
	Private Sub Blink_One()
		If Me.blinkCount >= Me.blinkCountMax AndAlso Me.blinkOne Then
			Me.topLayer.enabled = True
			Me.blinkCount = 0
			Me.blinkCountMax = Global.UnityEngine.Random.Range(3, 7)
		Else
			Me.topLayer.enabled = False
		End If
	End Sub

	' Token: 0x060016FC RID: 5884 RVA: 0x000CE584 File Offset: 0x000CC984
	Private Sub Blink_Two()
		If Me.blinkCount >= Me.blinkCountMax AndAlso Not Me.blinkOne Then
			Me.topLayer.enabled = True
			Me.blinkCount = 0
			Me.blinkCountMax = Global.UnityEngine.Random.Range(3, 7)
		Else
			Me.topLayer.enabled = False
		End If
	End Sub

	' Token: 0x060016FD RID: 5885 RVA: 0x000CE5E0 File Offset: 0x000CC9E0
	Private Iterator Function move_cr() As IEnumerator
		Dim isLooping As Boolean = False
		Dim p As LevelProperties.Bee.General = MyBase.properties.CurrentState.general
		Me.movingRight = False
		Me.isMoving = True
		Me.offset = p.movementOffset
		Me.speed = p.movementSpeed
		While True
			If Me.isMoving Then
				If Me.movingRight Then
					While MyBase.transform.position.x < 640F - Me.offset AndAlso Me.movingRight
						MyBase.transform.AddPosition(Me.speed * CupheadTime.Delta * Me.hitPauseCoefficient(), 0F, 0F)
						Yield Nothing
					End While
					If Me.state <> BeeLevelAirplane.State.Wing Then
						Me.movingRight = Not Me.movingRight
					End If
				Else
					While MyBase.transform.position.x > -640F + Me.offset AndAlso Not Me.movingRight
						MyBase.transform.AddPosition(-Me.speed * CupheadTime.Delta * Me.hitPauseCoefficient(), 0F, 0F)
						Yield Nothing
					End While
					If Me.state <> BeeLevelAirplane.State.Wing Then
						Me.movingRight = Not Me.movingRight
					End If
				End If
				If Not isLooping Then
					AudioManager.PlayLoop("bee_airplane_idle_loop")
					Me.emitAudioFromObject.Add("bee_airplane_idle_loop")
					isLooping = True
				End If
			ElseIf isLooping Then
				AudioManager.[Stop]("bee_airplane_idle_loop")
				isLooping = False
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060016FE RID: 5886 RVA: 0x000CE5FB File Offset: 0x000CC9FB
	Private Function hitPauseCoefficient() As Single
		Return If((Not MyBase.GetComponent(Of DamageReceiver)().IsHitPaused), 1F, 0F)
	End Function

	' Token: 0x060016FF RID: 5887 RVA: 0x000CE61C File Offset: 0x000CCA1C
	Public Sub StartTurbine()
		If Me.patternCoroutine IsNot Nothing Then
			MyBase.StopCoroutine(Me.patternCoroutine)
		End If
		Me.patternCoroutine = MyBase.StartCoroutine(Me.turbine_cr())
	End Sub

	' Token: 0x06001700 RID: 5888 RVA: 0x000CE648 File Offset: 0x000CCA48
	Private Iterator Function turbine_cr() As IEnumerator
		Me.state = BeeLevelAirplane.State.Turbine
		Dim p As LevelProperties.Bee.TurbineBlasters = MyBase.properties.CurrentState.turbineBlasters
		Dim bulletPattern As String() = p.attackDirectionString.GetRandom().Split(New Char() { ","c })
		For i As Integer = 0 To bulletPattern.Length - 1
			If bulletPattern(i)(0) = "R"c Then
				MyBase.animator.Play("Right_Pylon")
			ElseIf bulletPattern(i)(0) = "L"c Then
				MyBase.animator.Play("Left_Pylon")
			ElseIf bulletPattern(i)(0) = "B"c Then
				MyBase.animator.Play("Right_Pylon")
				MyBase.animator.Play("Left_Pylon")
			End If
			Yield CupheadTime.WaitForSeconds(Me, p.repeatDealy)
		Next
		Yield CupheadTime.WaitForSeconds(Me, p.hesitateRange.RandomFloat())
		Me.state = BeeLevelAirplane.State.Idle
		Return
	End Function

	' Token: 0x06001701 RID: 5889 RVA: 0x000CE664 File Offset: 0x000CCA64
	Private Sub ShootBulletRight()
		AudioManager.Play("bee_airplane_pylon")
		Me.emitAudioFromObject.Add("bee_airplane_pylon")
		Dim vector As Vector3 = New Vector3(0F, 360F, 0F) - New Vector3(0F, MyBase.transform.position.y, 0F)
		Dim num As Single = MathUtils.DirectionToAngle(vector)
		Me.bullet.Create(Me.rightShootRoot.transform.position, num, True, MyBase.properties.CurrentState.turbineBlasters)
	End Sub

	' Token: 0x06001702 RID: 5890 RVA: 0x000CE708 File Offset: 0x000CCB08
	Private Sub ShootBulletLeft()
		AudioManager.Play("bee_airplane_pylon")
		Me.emitAudioFromObject.Add("bee_airplane_pylon")
		Dim vector As Vector3 = New Vector3(0F, 360F, 0F) - New Vector3(0F, MyBase.transform.position.y, 0F)
		Dim num As Single = MathUtils.DirectionToAngle(vector)
		Me.bullet.Create(Me.leftShootRoot.transform.position, num, False, MyBase.properties.CurrentState.turbineBlasters)
	End Sub

	' Token: 0x06001703 RID: 5891 RVA: 0x000CE7AA File Offset: 0x000CCBAA
	Public Sub StartWing()
		If Me.patternCoroutine IsNot Nothing Then
			MyBase.StopCoroutine(Me.patternCoroutine)
		End If
		Me.patternCoroutine = MyBase.StartCoroutine(Me.wing_cr())
	End Sub

	' Token: 0x06001704 RID: 5892 RVA: 0x000CE7D8 File Offset: 0x000CCBD8
	Private Iterator Function wing_cr() As IEnumerator
		Me.state = BeeLevelAirplane.State.Wing
		Dim p As LevelProperties.Bee.WingSwipe = MyBase.properties.CurrentState.wingSwipe
		Dim player As AbstractPlayerController = PlayerManager.GetNext()
		Me.attackOnRight = player.transform.position.x >= 0F
		Dim startPos As Vector3 = Vector3.zero
		Dim count As Integer = 0
		Parser.IntTryParse(Me.countPattern(Me.countIndex), count)
		For i As Integer = 0 To count - 1
			MyBase.animator.SetTrigger("OnSaw")
			Yield MyBase.animator.WaitForAnimationToEnd(Me, "Saw_Start", False, True)
			Me.movingRight = Me.attackOnRight
			Me.offset = p.warningMaxDistance
			Me.speed = p.warningMovementSpeed
			If Me.attackOnRight Then
				While MyBase.transform.position.x < 640F - p.warningMaxDistance
					Yield Nothing
				End While
			Else
				While MyBase.transform.position.x > -640F + p.warningMaxDistance
					Yield Nothing
				End While
			End If
			Me.isMoving = False
			Yield CupheadTime.WaitForSeconds(Me, p.warningDuration)
			MyBase.animator.SetTrigger("Continue")
			Me.isMoving = True
			Me.offset = p.maxDistance
			Me.movingRight = Not Me.movingRight
			Me.speed = p.movementSpeed
			If Not Me.attackOnRight Then
				While MyBase.transform.position.x < 640F - p.maxDistance
					Yield Nothing
				End While
			Else
				While MyBase.transform.position.x > -640F + p.maxDistance
					Yield Nothing
				End While
			End If
			Me.isMoving = False
			Yield CupheadTime.WaitForSeconds(Me, p.attackDuration)
			MyBase.animator.SetTrigger("End")
			Me.isMoving = True
			Me.offset = MyBase.properties.CurrentState.general.movementOffset
			Me.attackOnRight = Not Me.attackOnRight
			Me.speed = MyBase.properties.CurrentState.general.movementSpeed
		Next
		Me.state = BeeLevelAirplane.State.EndWing
		Me.countIndex = (Me.countIndex + 1) Mod Me.countPattern.Length
		Yield CupheadTime.WaitForSeconds(Me, p.hesitateRange.RandomFloat())
		Me.state = BeeLevelAirplane.State.Idle
		Return
	End Function

	' Token: 0x06001705 RID: 5893 RVA: 0x000CE7F3 File Offset: 0x000CCBF3
	Private Sub SawLoopSFX()
		Me.SawStartSFX()
		AudioManager.PlayLoop("bee_airplane_saw_loop")
		Me.emitAudioFromObject.Add("bee_airplane_saw_loop")
	End Sub

	' Token: 0x06001706 RID: 5894 RVA: 0x000CE815 File Offset: 0x000CCC15
	Private Sub SawStartSFX()
		AudioManager.Play("bee_airplane_saw_start")
		Me.emitAudioFromObject.Add("bee_airplane_saw_start")
	End Sub

	' Token: 0x06001707 RID: 5895 RVA: 0x000CE831 File Offset: 0x000CCC31
	Private Sub SawEndSFX()
		AudioManager.[Stop]("bee_airplane_saw_loop")
		AudioManager.Play("bee_airplane_saw_end")
		Me.emitAudioFromObject.Add("bee_airplane_saw_end")
	End Sub

	' Token: 0x06001708 RID: 5896 RVA: 0x000CE857 File Offset: 0x000CCC57
	Private Sub DeathHeadSFX()
		AudioManager.Play("bee_airplane_death_head")
		Me.emitAudioFromObject.Add("bee_airplane_death_head")
	End Sub

	' Token: 0x06001709 RID: 5897 RVA: 0x000CE873 File Offset: 0x000CCC73
	Private Sub SawContinueSFX()
		If Not Me.isPreSFXPlaying Then
			AudioManager.Play("bee_airplane_saw_continue")
			Me.emitAudioFromObject.Add("bee_airplane_saw_continue")
			Me.isPreSFXPlaying = True
		End If
	End Sub

	' Token: 0x0600170A RID: 5898 RVA: 0x000CE8A1 File Offset: 0x000CCCA1
	Private Sub SawContinueSFXEnd()
		Me.isPreSFXPlaying = False
	End Sub

	' Token: 0x0600170B RID: 5899 RVA: 0x000CE8AC File Offset: 0x000CCCAC
	Private Sub Flip()
		If Me.attackOnRight Then
			MyBase.transform.SetScale(New Single?(1F), New Single?(1F), New Single?(1F))
		Else
			MyBase.transform.SetScale(New Single?(-1F), New Single?(1F), New Single?(1F))
		End If
	End Sub

	' Token: 0x0600170C RID: 5900 RVA: 0x000CE91B File Offset: 0x000CCD1B
	Private Sub Dead()
		Me.StopAllCoroutines()
		AudioManager.[Stop]("bee_airplane_saw_loop")
		AudioManager.Play("bee_airplane_death")
		Me.emitAudioFromObject.Add("bee_airplane_death")
		MyBase.animator.SetTrigger("Death")
	End Sub

	' Token: 0x0600170D RID: 5901 RVA: 0x000CE957 File Offset: 0x000CCD57
	Private Sub DeadHead()
		MyBase.animator.Play("Death_Head")
	End Sub

	' Token: 0x0400203A RID: 8250
	<SerializeField()>
	Private rightShootRoot As Transform

	' Token: 0x0400203B RID: 8251
	<SerializeField()>
	Private leftShootRoot As Transform

	' Token: 0x0400203C RID: 8252
	<SerializeField()>
	Private bullet As BeeLevelTurbineBullet

	' Token: 0x0400203D RID: 8253
	<SerializeField()>
	Private topLayer As SpriteRenderer

	' Token: 0x0400203E RID: 8254
	<SerializeField()>
	Private midLayer As SpriteRenderer

	' Token: 0x04002040 RID: 8256
	Private countPattern As String()

	' Token: 0x04002041 RID: 8257
	Private countIndex As Integer

	' Token: 0x04002042 RID: 8258
	Private blinkCount As Integer

	' Token: 0x04002043 RID: 8259
	Private blinkCountMax As Integer

	' Token: 0x04002044 RID: 8260
	Private blinkOne As Boolean

	' Token: 0x04002045 RID: 8261
	Private attackOnRight As Boolean

	' Token: 0x04002046 RID: 8262
	Private movingRight As Boolean

	' Token: 0x04002047 RID: 8263
	Private isMoving As Boolean

	' Token: 0x04002048 RID: 8264
	Private isPreSFXPlaying As Boolean

	' Token: 0x04002049 RID: 8265
	Private offset As Single

	' Token: 0x0400204A RID: 8266
	Private speed As Single

	' Token: 0x0400204B RID: 8267
	Private damageDealer As DamageDealer

	' Token: 0x0400204C RID: 8268
	Private damageReceiver As DamageReceiver

	' Token: 0x0400204D RID: 8269
	Private patternCoroutine As Coroutine

	' Token: 0x0200050F RID: 1295
	Public Enum State
		' Token: 0x0400204F RID: 8271
		Unspawned
		' Token: 0x04002050 RID: 8272
		Intro
		' Token: 0x04002051 RID: 8273
		Idle
		' Token: 0x04002052 RID: 8274
		Wing
		' Token: 0x04002053 RID: 8275
		EndWing
		' Token: 0x04002054 RID: 8276
		Turbine
		' Token: 0x04002055 RID: 8277
		Dead
	End Enum
End Class
