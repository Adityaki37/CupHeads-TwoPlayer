Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200076F RID: 1903
Public Class RobotLevelHelihead
	Inherits AbstractCollidableObject

	' Token: 0x06002967 RID: 10599 RVA: 0x00182177 File Offset: 0x00180577
	Protected Overrides Sub Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		Me.current = RobotLevelHelihead.state.first
		MyBase.Awake()
	End Sub

	' Token: 0x06002968 RID: 10600 RVA: 0x001821B4 File Offset: 0x001805B4
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
		If Me.properties IsNot Nothing AndAlso Me.properties.CurrentHealth <= 0F Then
			Me.StopAllCoroutines()
		End If
	End Sub

	' Token: 0x06002969 RID: 10601 RVA: 0x001821F4 File Offset: 0x001805F4
	Protected Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Level.Current.timeline.DealDamage(info.damage)
		Me.properties.DealDamage(info.damage)
		If Me.properties.CurrentHealth <= 0F AndAlso Me.current <> RobotLevelHelihead.state.dead Then
			Me.current = RobotLevelHelihead.state.dead
			Me.StartDeath()
		End If
	End Sub

	' Token: 0x0600296A RID: 10602 RVA: 0x00182258 File Offset: 0x00180658
	Public Sub InitHeliHead(properties As LevelProperties.Robot)
		Me.introActive = True
		Me.screenHeights = properties.CurrentState.heliHead.onScreenHeight.Split(New Char() { ","c })
		Me.coordinateIndex = Global.UnityEngine.Random.Range(0, Me.screenHeights.Length)
		Me.attackTypeIndex = Global.UnityEngine.Random.Range(0, properties.CurrentState.inventor.gemColourString.Split(New Char() { ","c }).Length)
		Me.pivotPoint = New GameObject("pivotPoint")
		Me.pivotPoint.transform.position = New Vector3(CSng((Level.Current.Right - Level.Current.Width / 4)), CSng((Level.Current.Ground + Level.Current.Height / 2)), 0F)
		Me.speed = CSng(properties.CurrentState.heliHead.heliheadMovementSpeed)
		Me.attackDelay = properties.CurrentState.heliHead.attackDelay
		Me.width = 300F
		Me.properties = properties
		Me.speed = CSng(properties.CurrentState.heliHead.heliheadMovementSpeed)
		MyBase.transform.Rotate(Vector3.forward, 90F)
		MyBase.transform.position = Me.spawnPoint.position
		MyBase.StartCoroutine(Me.horizontalMovement_cr())
		MyBase.StartCoroutine(Me.attack_cr())
		MyBase.StartCoroutine(Me.check_sound_cr())
	End Sub

	' Token: 0x0600296B RID: 10603 RVA: 0x001823D6 File Offset: 0x001807D6
	Private Sub SpinSFX()
		AudioManager.Play("robot_headspin")
		Me.emitAudioFromObject.Add("robot_headspin")
	End Sub

	' Token: 0x0600296C RID: 10604 RVA: 0x001823F4 File Offset: 0x001807F4
	Private Iterator Function check_sound_cr() As IEnumerator
		Dim onscreen As Boolean = False
		While Me.current = RobotLevelHelihead.state.first
			If MyBase.transform.position.x < CSng(Level.Current.Right) AndAlso MyBase.transform.position.x > CSng(Level.Current.Left) Then
				If Not onscreen Then
					Me.SpinSFX()
					onscreen = True
				End If
			ElseIf onscreen Then
				AudioManager.[Stop]("robot_headspin")
				onscreen = False
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600296D RID: 10605 RVA: 0x00182410 File Offset: 0x00180810
	Private Iterator Function horizontalMovement_cr() As IEnumerator
		Me.offScreen = False
		Yield New WaitForEndOfFrame()
		MyBase.transform.position += Vector3.left * MyBase.GetComponent(Of SpriteRenderer)().bounds.size.x / 10F
		While True
			MyBase.transform.position += Vector3.left * Me.speed * CupheadTime.Delta
			If MyBase.transform.position.x <= CSng(Level.Current.Left) - Me.width Then
				MyBase.GetComponent(Of BoxCollider2D)().enabled = False
				Me.offScreen = True
				If Me.introActive Then
					Me.introActive = False
					MyBase.animator.Play("Loop")
				End If
				Me.speed = 0F
				MyBase.transform.position = New Vector3(MyBase.transform.position.x, CSng((Level.Current.Ground + Parser.IntParse(Me.screenHeights(Me.coordinateIndex)))), MyBase.transform.position.z)
				MyBase.transform.Rotate(Vector3.forward, 180F)
				Me.coordinateIndex += 1
				If Me.coordinateIndex >= Me.screenHeights.Length Then
					Me.coordinateIndex = 0
				End If
				Yield CupheadTime.WaitForSeconds(Me, Me.properties.CurrentState.heliHead.offScreenDelay)
				Me.speed = CSng((-CSng(Me.properties.CurrentState.heliHead.heliheadMovementSpeed)))
				MyBase.transform.position += Vector3.right * 50F
				Me.offScreen = False
				MyBase.GetComponent(Of BoxCollider2D)().enabled = True
			End If
			If MyBase.transform.position.x >= CSng(Level.Current.Right) + Me.width Then
				MyBase.GetComponent(Of BoxCollider2D)().enabled = False
				Me.offScreen = True
				Me.speed = 0F
				MyBase.transform.position = New Vector3(MyBase.transform.position.x, CSng((Level.Current.Ground + Parser.IntParse(Me.screenHeights(Me.coordinateIndex)))), MyBase.transform.position.z)
				MyBase.transform.Rotate(Vector3.forward, 180F)
				MyBase.transform.position += Vector3.left * 50F
				Me.coordinateIndex += 1
				If Me.coordinateIndex >= Me.screenHeights.Length Then
					Me.coordinateIndex = 0
				End If
				Yield CupheadTime.WaitForSeconds(Me, Me.properties.CurrentState.heliHead.offScreenDelay)
				Me.speed = CSng(Me.properties.CurrentState.heliHead.heliheadMovementSpeed)
				Me.offScreen = False
				MyBase.GetComponent(Of BoxCollider2D)().enabled = True
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600296E RID: 10606 RVA: 0x0018242C File Offset: 0x0018082C
	Private Iterator Function inventorIntro_cr() As IEnumerator
		Dim [end] As Vector3 = New Vector3(Me.pivotPoint.transform.position.x, -760F)
		Dim start As Vector3 = MyBase.transform.position
		Dim pct As Single = 0F
		While pct < 1F
			MyBase.transform.position = Vector3.Lerp(start, [end], pct)
			pct += CupheadTime.Delta
			Yield Nothing
		End While
		MyBase.transform.position = [end]
		MyBase.StartCoroutine(Me.stateEasing_cr())
		Return
	End Function

	' Token: 0x0600296F RID: 10607 RVA: 0x00182448 File Offset: 0x00180848
	Private Iterator Function stateEasing_cr() As IEnumerator
		MyBase.transform.rotation = Quaternion.identity
		Dim start As Vector3 = MyBase.transform.position
		Dim [end] As Vector3 = New Vector3(Me.pivotPoint.transform.position.x - 200F, Me.pivotPoint.transform.position.y)
		Dim pct As Single = 0F
		While pct < 1F
			MyBase.transform.position = Vector3.Lerp(start, [end], pct)
			pct += CupheadTime.Delta
			Yield Nothing
		End While
		AudioManager.[Stop]("robot_headspin")
		MyBase.animator.Play("Inventor Intro")
		MyBase.StartCoroutine(Me.verticalMovement_cr())
		Me.speed *= 2F
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "End", True, True)
		Yield CupheadTime.WaitForSeconds(Me, Me.properties.CurrentState.inventor.initialAttackDelay)
		Dim normalizedTime As Single = MyBase.animator.GetCurrentAnimatorStateInfo(0).normalizedTime Mod 1F
		Dim delay As Single = 0F
		If MyBase.animator.GetCurrentAnimatorStateInfo(0).length / normalizedTime < 1F Then
			delay -= normalizedTime
		Else
			delay += 1F - normalizedTime
		End If
		Yield CupheadTime.WaitForSeconds(Me, delay)
		MyBase.StartCoroutine(Me.blockade_cr())
		If Me.properties.CurrentState.inventor.gemColourString.Split(New Char() { ","c })(Me.attackTypeIndex) = "R" Then
			MyBase.animator.Play("Red Gem Attack")
			Yield MyBase.animator.WaitForAnimationToEnd(Me, "Red Gem Attack", False, True)
			MyBase.animator.Play("RedGemFXIntro", 2)
			Me.gem.InitFinalStage(Me, Me.properties, False)
		Else
			MyBase.animator.Play("Blue Gem Attack")
			Yield MyBase.animator.WaitForAnimationToEnd(Me, "Blue Gem Attack", False, True)
			MyBase.animator.Play("BlueGemFXIntro", 2)
			Me.gem.InitFinalStage(Me, Me.properties, True)
		End If
		Me.speed /= 2F
		MyBase.StartCoroutine(Me.easeValues_cr(True))
		Return
	End Function

	' Token: 0x06002970 RID: 10608 RVA: 0x00182464 File Offset: 0x00180864
	Private Iterator Function verticalMovement_cr() As IEnumerator
		Me.speed = 1F
		Dim time As Single = 0F
		While True
			time += CupheadTime.Delta * 2F
			MyBase.transform.position = Me.pivotPoint.transform.position + Vector3.left * 200F + Vector3.up * Mathf.Sin(time * Me.speed) * Me.verticalMovementStrength + Vector3.right * Mathf.Sin(time * (2F * Me.speed)) * Me.horizontalMovementStrength
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06002971 RID: 10609 RVA: 0x00182480 File Offset: 0x00180880
	Private Iterator Function easeValues_cr(Optional easeIn As Boolean = True) As IEnumerator
		If easeIn Then
			Me.speed = Me.properties.CurrentState.inventor.inventorIdleSpeedMultiplier
		End If
		MyBase.StartCoroutine(Me.easeStrength_cr(easeIn))
		If easeIn Then
			Yield CupheadTime.WaitForSeconds(Me, Me.properties.CurrentState.inventor.attackDuration.RandomFloat())
			If Me.properties.CurrentState.inventor.gemColourString.Split(New Char() { ","c })(Me.attackTypeIndex) = "R" Then
				MyBase.animator.SetTrigger("RedGemAttack")
			Else
				MyBase.animator.SetTrigger("BlueGemAttack")
			End If
			Me.gem.OnAttackEnd()
			Me.attackTypeIndex += 1
			If Me.attackTypeIndex >= Me.properties.CurrentState.inventor.gemColourString.Split(New Char() { ","c }).Length Then
				Me.attackTypeIndex = 0
			End If
			Yield CupheadTime.WaitForSeconds(Me, Me.properties.CurrentState.inventor.attackDelay.RandomFloat())
			MyBase.StartCoroutine(Me.easeValues_cr(False))
		Else
			Dim normalizedTime As Single = MyBase.animator.GetCurrentAnimatorStateInfo(0).normalizedTime Mod 1F
			Dim delay As Single = 0F
			If MyBase.animator.GetCurrentAnimatorStateInfo(0).length / normalizedTime < 1F Then
				delay -= normalizedTime
			Else
				delay += 1F - normalizedTime
			End If
			Yield CupheadTime.WaitForSeconds(Me, delay)
			If Me.properties.CurrentState.inventor.gemColourString.Split(New Char() { ","c })(Me.attackTypeIndex) = "R" Then
				MyBase.animator.Play("Red Gem Attack")
				Yield MyBase.animator.WaitForAnimationToEnd(Me, "Red Gem Attack", False, True)
				MyBase.animator.Play("RedGemFXIntro", 2)
				Me.gem.InitFinalStage(Me, Me.properties, False)
			Else
				MyBase.animator.Play("Blue Gem Attack")
				Yield MyBase.animator.WaitForAnimationToEnd(Me, "Blue Gem Attack", False, True)
				MyBase.animator.Play("BlueGemFXIntro", 2)
				Me.gem.InitFinalStage(Me, Me.properties, True)
			End If
			MyBase.StartCoroutine(Me.easeValues_cr(True))
		End If
		Return
	End Function

	' Token: 0x06002972 RID: 10610 RVA: 0x001824A2 File Offset: 0x001808A2
	Public Sub OnGemEnd()
		Me.GemEndSFX()
		MyBase.animator.SetTrigger("StopGemFX")
	End Sub

	' Token: 0x06002973 RID: 10611 RVA: 0x001824BA File Offset: 0x001808BA
	Private Sub GemStartSFX()
		AudioManager.Play("robot_diamond_attack_start")
		Me.emitAudioFromObject.Add("robot_diamond_attack_start")
		AudioManager.PlayLoop("robot_diamond_attack_loop")
		Me.emitAudioFromObject.Add("robot_diamond_attack_loop")
	End Sub

	' Token: 0x06002974 RID: 10612 RVA: 0x001824F0 File Offset: 0x001808F0
	Private Sub GemEndSFX()
		AudioManager.[Stop]("robot_diamond_attack_loop")
		AudioManager.Play("robot_diamond_attack_end")
		Me.emitAudioFromObject.Add("robot_diamond_attack_end")
	End Sub

	' Token: 0x06002975 RID: 10613 RVA: 0x00182516 File Offset: 0x00180916
	Private Sub IntroSFX()
		AudioManager.Play("robot_head_transform")
		Me.emitAudioFromObject.Add("robot_head_transform")
	End Sub

	' Token: 0x06002976 RID: 10614 RVA: 0x00182534 File Offset: 0x00180934
	Private Iterator Function easeSpeed_cr() As IEnumerator
		Dim pct As Single = 0F
		While pct < 1F
			Me.speed = 1F + (Me.properties.CurrentState.inventor.inventorIdleSpeedMultiplier - 1F) * pct
			pct += 10F * CupheadTime.Delta
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06002977 RID: 10615 RVA: 0x00182550 File Offset: 0x00180950
	Private Iterator Function easeStrength_cr(easeIn As Boolean) As IEnumerator
		Dim pct As Single = 0F
		Dim hStrength As Single = Me.horizontalMovementStrength
		Dim vStrength As Single = Me.verticalMovementStrength
		While pct < 1F
			If easeIn Then
				Me.horizontalMovementStrength = hStrength + (25F - hStrength) * pct
				Me.verticalMovementStrength = vStrength + (160F - vStrength) * pct
			Else
				Me.horizontalMovementStrength = hStrength + (0F - hStrength) * pct
				Me.verticalMovementStrength = vStrength + (20F - vStrength) * pct
			End If
			pct += 0.25F * CupheadTime.Delta
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06002978 RID: 10616 RVA: 0x00182574 File Offset: 0x00180974
	Private Iterator Function attack_cr() As IEnumerator
		While True
			If Me.offScreen Then
				Me.attackDelay -= CupheadTime.Delta
				If Me.attackDelay <= 0F Then
					Me.SpawnBombBot()
					Me.attackDelay = 100F
				End If
			Else
				Me.attackDelay = Me.properties.CurrentState.heliHead.attackDelay
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06002979 RID: 10617 RVA: 0x00182590 File Offset: 0x00180990
	Private Iterator Function blockade_cr() As IEnumerator
		Dim groupSize As Single = CSng(Me.properties.CurrentState.inventor.blockadeGroupSize)
		Dim dir As Integer = 1
		While True
			Dim i As Integer = 0
			While CSng(i) < groupSize
				If dir > 0 Then
					Dim robotLevelBlockade As RobotLevelBlockade = Me.blockadeSegement.Create(New Vector3(CSng(Level.Current.Right), CSng(Level.Current.Ceiling), 0F), dir)
					robotLevelBlockade.InitBlockade(dir, Me.properties.CurrentState.inventor.blockadeHorizontalSpeed, Me.properties.CurrentState.inventor.blockadeVerticalSpeed)
				Else
					Dim robotLevelBlockade2 As RobotLevelBlockade = Me.blockadeSegement.Create(New Vector3(CSng(Level.Current.Right), CSng(Level.Current.Ground), 0F), dir)
					robotLevelBlockade2.InitBlockade(dir, Me.properties.CurrentState.inventor.blockadeHorizontalSpeed, Me.properties.CurrentState.inventor.blockadeVerticalSpeed)
				End If
				dir *= -1
				Yield CupheadTime.WaitForSeconds(Me, Me.properties.CurrentState.inventor.blockadeIndividualDelay)
				Yield Nothing
				i += 1
			End While
			Yield CupheadTime.WaitForSeconds(Me, Me.properties.CurrentState.inventor.blockadeGroupDelay)
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600297A RID: 10618 RVA: 0x001825AC File Offset: 0x001809AC
	Private Sub SpawnBombBot()
		Dim homingProjectile As HomingProjectile = Me.bombBotPrefab.GetComponent(Of RobotLevelHatchBombBot)().Create(MyBase.transform.GetChild(0).transform.position, CSng((CInt(MyBase.transform.eulerAngles.z) + 90)), CSng(Me.properties.CurrentState.bombBot.initialBombMovementSpeed), CSng(Me.properties.CurrentState.bombBot.bombHomingSpeed), Me.properties.CurrentState.bombBot.bombRotationSpeed, CSng(Me.properties.CurrentState.bombBot.bombLifeTime), 4F, PlayerManager.GetNext())
		homingProjectile.GetComponent(Of RobotLevelHatchBombBot)().InitBombBot(Me.properties.CurrentState.bombBot)
	End Sub

	' Token: 0x0600297B RID: 10619 RVA: 0x00182679 File Offset: 0x00180A79
	Public Sub ChangeState()
		Me.current = RobotLevelHelihead.state.second
		Me.StopAllCoroutines()
		MyBase.StartCoroutine(Me.inventorIntro_cr())
	End Sub

	' Token: 0x0600297C RID: 10620 RVA: 0x00182695 File Offset: 0x00180A95
	Protected Overrides Sub OnDestroy()
		Me.StopAllCoroutines()
		MyBase.OnDestroy()
		AudioManager.[Stop]("robot_diamond_attack_loop")
	End Sub

	' Token: 0x0600297D RID: 10621 RVA: 0x001826AD File Offset: 0x00180AAD
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x0600297E RID: 10622 RVA: 0x001826CC File Offset: 0x00180ACC
	Private Sub StartDeath()
		Me.StopAllCoroutines()
		If Me.OnDeath IsNot Nothing Then
			Me.OnDeath()
		End If
		MyBase.GetComponent(Of Collider2D)().enabled = False
		MyBase.animator.SetTrigger("OnDeath")
		AudioManager.[Stop]("robot_diamond_attack_loop")
	End Sub

	' Token: 0x04003268 RID: 12904
	<SerializeField()>
	Private verticalMovementStrength As Single

	' Token: 0x04003269 RID: 12905
	<SerializeField()>
	Private horizontalMovementStrength As Single

	' Token: 0x0400326A RID: 12906
	<SerializeField()>
	Private spawnPoint As Transform

	' Token: 0x0400326B RID: 12907
	Private pivotPoint As GameObject

	' Token: 0x0400326C RID: 12908
	Private introActive As Boolean

	' Token: 0x0400326D RID: 12909
	Private coordinateIndex As Integer

	' Token: 0x0400326E RID: 12910
	Private screenHeights As String()

	' Token: 0x0400326F RID: 12911
	Private speed As Single

	' Token: 0x04003270 RID: 12912
	Private width As Single

	' Token: 0x04003271 RID: 12913
	Private attackDelay As Single

	' Token: 0x04003272 RID: 12914
	Private offScreen As Boolean

	' Token: 0x04003273 RID: 12915
	Private attackTypeIndex As Integer

	' Token: 0x04003274 RID: 12916
	Private current As RobotLevelHelihead.state

	' Token: 0x04003275 RID: 12917
	Private properties As LevelProperties.Robot

	' Token: 0x04003276 RID: 12918
	Private damageDealer As DamageDealer

	' Token: 0x04003277 RID: 12919
	Private damageReceiver As DamageReceiver

	' Token: 0x04003278 RID: 12920
	<SerializeField()>
	Private bombBotPrefab As GameObject

	' Token: 0x04003279 RID: 12921
	<SerializeField()>
	Private blockadeSegement As RobotLevelBlockade

	' Token: 0x0400327A RID: 12922
	<SerializeField()>
	Private gem As RobotLevelGem

	' Token: 0x0400327B RID: 12923
	Public OnDeath As Action

	' Token: 0x02000770 RID: 1904
	Private Enum state
		' Token: 0x0400327D RID: 12925
		first
		' Token: 0x0400327E RID: 12926
		second
		' Token: 0x0400327F RID: 12927
		dead
	End Enum
End Class
