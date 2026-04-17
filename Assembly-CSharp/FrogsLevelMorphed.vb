Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020006AB RID: 1707
Public Class FrogsLevelMorphed
	Inherits LevelProperties.Frogs.Entity

	' Token: 0x0600242C RID: 9260 RVA: 0x00153780 File Offset: 0x00151B80
	Protected Overrides Sub Awake()
		MyBase.Awake()
		FrogsLevelMorphed.Current = Me
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		Me.damageDealer = New DamageDealer(1F, 0.3F, DamageDealer.DamageSource.Enemy, True, False, False)
		Me.slots.Init(Me)
		Me.handle = FrogsLevelMorphedSwitch.Create(Me)
		Me.handle.enabled = False
		AddHandler Me.handle.OnActivate, AddressOf Me.OnHandleActivated
		Me.slotsParent.SetActive(False)
		MyBase.gameObject.SetActive(False)
	End Sub

	' Token: 0x0600242D RID: 9261 RVA: 0x00153828 File Offset: 0x00151C28
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		If FrogsLevelMorphed.Current Is Me Then
			FrogsLevelMorphed.Current = Nothing
		End If
		Me.coin = Nothing
		Me.snakeBullet = Nothing
		Me.oniBullet = Nothing
		Me.bisonBullet = Nothing
		Me.tigerBullet = Nothing
		Me.dustEffect = Nothing
		Me.slots.OnDestroy()
	End Sub

	' Token: 0x0600242E RID: 9262 RVA: 0x00153886 File Offset: 0x00151C86
	Private Sub Start()
		Me.damageReceiver.enabled = False
	End Sub

	' Token: 0x0600242F RID: 9263 RVA: 0x00153894 File Offset: 0x00151C94
	Private Sub Update()
		Me.damageDealer.Update()
	End Sub

	' Token: 0x06002430 RID: 9264 RVA: 0x001538A1 File Offset: 0x00151CA1
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		AudioManager.Play("level_frogs_short_clap_shock")
		Me.emitAudioFromObject.Add("level_frogs_short_clap_shock")
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06002431 RID: 9265 RVA: 0x001538D9 File Offset: 0x00151CD9
	Public Overrides Sub LevelInit(properties As LevelProperties.Frogs)
		MyBase.LevelInit(properties)
	End Sub

	' Token: 0x06002432 RID: 9266 RVA: 0x001538E2 File Offset: 0x00151CE2
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		MyBase.properties.DealDamage(info.damage)
	End Sub

	' Token: 0x06002433 RID: 9267 RVA: 0x001538F8 File Offset: 0x00151CF8
	Public Sub Enable(demonTriggered As Boolean)
		Me.demonTriggered = demonTriggered
		MyBase.gameObject.SetActive(True)
		Me.dustEffect.gameObject.SetActive(True)
		AddHandler MyBase.properties.OnBossDeath, AddressOf Me.OnBossDeath
		MyBase.GetComponent(Of LevelBossDeathExploder)().enabled = True
		MyBase.StartCoroutine(Me.loop_cr())
	End Sub

	' Token: 0x06002434 RID: 9268 RVA: 0x00153959 File Offset: 0x00151D59
	Private Sub OnBossDeath()
		AudioManager.PlayLoop("level_frogs_morphed_death_loop")
		Me.emitAudioFromObject.Add("level_frogs_morphed_death_loop")
		Me.StopAllCoroutines()
		MyBase.animator.SetTrigger("OnDeath")
		Me.slotsParent.SetActive(False)
	End Sub

	' Token: 0x06002435 RID: 9269 RVA: 0x00153998 File Offset: 0x00151D98
	Private Sub ShootCoin()
		AudioManager.Play("level_frogs_morphed_mouth")
		Me.emitAudioFromObject.Add("level_frogs_morphed_mouth")
		Me.coinRoot.LookAt2D(PlayerManager.GetNext().center)
		Dim frogsLevelMorphedCoin As FrogsLevelMorphedCoin = Me.coin.CreateCoin(Me.coinRoot.position, Me.coinSpeed, Me.coinRoot.eulerAngles.z)
		frogsLevelMorphedCoin.transform.SetPosition(Nothing, Nothing, New Single?(-600F))
	End Sub

	' Token: 0x06002436 RID: 9270 RVA: 0x00153A30 File Offset: 0x00151E30
	Private Sub StartShooting()
		Me.EndShooting()
		Me.shootingCoroutine = Me.shootingLoop_cr()
		MyBase.StartCoroutine(Me.shootingCoroutine)
	End Sub

	' Token: 0x06002437 RID: 9271 RVA: 0x00153A51 File Offset: 0x00151E51
	Private Sub EndShooting()
		If Me.shootingCoroutine IsNot Nothing Then
			MyBase.StopCoroutine(Me.shootingCoroutine)
		End If
	End Sub

	' Token: 0x06002438 RID: 9272 RVA: 0x00153A6A File Offset: 0x00151E6A
	Private Sub OnHandleActivated()
		Me.handleActivated = True
	End Sub

	' Token: 0x06002439 RID: 9273 RVA: 0x00153A74 File Offset: 0x00151E74
	Private Iterator Function loop_cr() As IEnumerator
		If Me.demonTriggered Then
			Me.mainIndex = Global.UnityEngine.Random.Range(0, MyBase.properties.CurrentState.demon.demonString.Length)
			Me.index = Global.UnityEngine.Random.Range(0, MyBase.properties.CurrentState.demon.demonString(Me.mainIndex).Split(New Char() { ","c }).Length)
		End If
		AudioManager.Play("level_frogs_morphed_open")
		Me.emitAudioFromObject.Add("level_frogs_morphed_open")
		Me.slotsParent.SetActive(True)
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		MyBase.animator.Play("Open")
		AudioManager.Play("level_frogs_morphed_open")
		Me.emitAudioFromObject.Add("level_frogs_morphed_open")
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		While True
			Dim p As LevelProperties.Frogs.Morph = MyBase.properties.CurrentState.morph
			Me.StartShooting()
			Yield CupheadTime.WaitForSeconds(Me, p.armDownDelay)
			Yield MyBase.StartCoroutine(Me.waitForActivate_cr())
			Me.EndShooting()
			MyBase.animator.SetTrigger("OnActivated")
			Yield MyBase.StartCoroutine(Me.pattern_cr(p))
			Me.slotsParent.SetActive(True)
			MyBase.animator.SetTrigger("OnAttackEnd")
			Yield MyBase.animator.WaitForAnimationToEnd(Me, "Attack_End", False, True)
		End While
		Return
	End Function

	' Token: 0x0600243A RID: 9274 RVA: 0x00153A90 File Offset: 0x00151E90
	Private Iterator Function waitForActivate_cr() As IEnumerator
		Me.handleActivated = False
		Me.handle.enabled = True
		MyBase.animator.SetTrigger("OnArmDown")
		AudioManager.Play("level_frogs_morphed_arm_down")
		Me.emitAudioFromObject.Add("level_frogs_morphed_arm_down")
		While Not Me.handleActivated
			Yield Nothing
		End While
		Me.handle.enabled = False
		Return
	End Function

	' Token: 0x0600243B RID: 9275 RVA: 0x00153AAC File Offset: 0x00151EAC
	Private Iterator Function shootingLoop_cr() As IEnumerator
		Dim p As LevelProperties.Frogs.Morph = MyBase.properties.CurrentState.morph
		Dim time As Single = p.coinMinMaxTime
		Dim t As Single = 0F
		Dim val As Single = 0F
		Dim coinDelay As Single = 0F
		While True
			Dim delay As Single = p.coinDelay.GetFloatAt(val)
			Me.coinSpeed = p.coinSpeed.GetFloatAt(val)
			If coinDelay >= delay Then
				MyBase.animator.SetTrigger("OnShoot")
				coinDelay = 0F
			End If
			If val < 1F Then
				val = t / time
				t += CupheadTime.Delta
			Else
				val = 1F
			End If
			coinDelay += CupheadTime.Delta
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600243C RID: 9276 RVA: 0x00153AC8 File Offset: 0x00151EC8
	Private Iterator Function pattern_cr(p As LevelProperties.Frogs.Morph) As IEnumerator
		Dim mode As Slots.Mode = Slots.Mode.Snake
		If Not Me.demonTriggered Then
			Dim num As Integer = Global.UnityEngine.Random.Range(0, 3)
			mode = CType(num, Slots.Mode)
		Else
			mode = Slots.Mode.Oni
			Yield Nothing
		End If
		Me.slots.Spin()
		Yield CupheadTime.WaitForSeconds(Me, 3F * p.slotSelectionDurationPercentage)
		Me.slots.[Stop](mode)
		Yield CupheadTime.WaitForSeconds(Me, 1F * p.slotSelectionDurationPercentage)
		Me.slots.StartFlash()
		Yield CupheadTime.WaitForSeconds(Me, 0.8F * p.slotSelectionDurationPercentage)
		Me.slots.StartFlash()
		Yield CupheadTime.WaitForSeconds(Me, 0.8F * p.slotSelectionDurationPercentage)
		Me.slots.StartFlash()
		Yield CupheadTime.WaitForSeconds(Me, 0.8F * p.slotSelectionDurationPercentage)
		Me.damageReceiver.enabled = True
		MyBase.animator.SetTrigger("OnAttack")
		AudioManager.Play("level_frogs_morphed_attack")
		Me.emitAudioFromObject.Add("level_frogs_morphed_attack")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Attack", False, True)
		Me.slotsParent.SetActive(False)
		AudioManager.PlayLoop("level_frogs_platform_loop")
		Me.emitAudioFromObject.Add("level_frogs_platform_loop")
		Select Case mode
			Case Slots.Mode.Snake
				Yield MyBase.StartCoroutine(Me.snake_cr(p))
			Case Slots.Mode.Tiger
				Yield MyBase.StartCoroutine(Me.tiger_cr(p))
			Case Slots.Mode.Bison
				Yield MyBase.StartCoroutine(Me.bison_cr(p))
			Case Slots.Mode.Oni
				Yield MyBase.StartCoroutine(Me.oni_cr())
		End Select
		AudioManager.[Stop]("level_frogs_platform_loop")
		Me.damageReceiver.enabled = False
		Return
	End Function

	' Token: 0x0600243D RID: 9277 RVA: 0x00153AEA File Offset: 0x00151EEA
	Private Sub ShootSnake(speed As Single)
		Me.snakeBullet.Create(Me.slotBulletRoot.position, speed)
	End Sub

	' Token: 0x0600243E RID: 9278 RVA: 0x00153B09 File Offset: 0x00151F09
	Private Sub ShootBison(speed As Single, dir As FrogsLevelBisonBullet.Direction, bigX As Single, smallX As Single)
		Me.bisonBullet.Create(Me.slotBulletRoot.position, speed, dir, bigX, smallX)
	End Sub

	' Token: 0x0600243F RID: 9279 RVA: 0x00153B2C File Offset: 0x00151F2C
	Private Sub ShootTiger(speed As Single)
		Me.tigerBullet.Create(Me.slotBulletRoot.position, speed)
	End Sub

	' Token: 0x06002440 RID: 9280 RVA: 0x00153B4B File Offset: 0x00151F4B
	Private Sub ShootOni(speed As Single)
		Me.oniBullet.Create(Me.slotBulletRoot.position, speed, MyBase.properties.CurrentState.demon)
	End Sub

	' Token: 0x06002441 RID: 9281 RVA: 0x00153B7C File Offset: 0x00151F7C
	Private Iterator Function snake_cr(p As LevelProperties.Frogs.Morph) As IEnumerator
		Dim t As Single = 0F
		Dim time As Single = p.snakeDuration
		Dim val As Single = 0F
		Dim bulletDelay As Single = 1000F
		Dim bulletSpeed As Single = 0F
		Dim delay As Single = 0F
		While t < time
			If bulletDelay >= delay Then
				bulletSpeed = p.snakeSpeed.GetFloatAt(val)
				Me.ShootSnake(bulletSpeed)
				bulletDelay = 0F
			End If
			delay = p.snakeDelay.GetFloatAt(val)
			bulletDelay += CupheadTime.Delta
			If val < 1F Then
				val = t / time
				t += CupheadTime.Delta
			Else
				val = 1F
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06002442 RID: 9282 RVA: 0x00153BA0 File Offset: 0x00151FA0
	Private Iterator Function bison_cr(p As LevelProperties.Frogs.Morph) As IEnumerator
		Dim t As Single = 0F
		Dim time As Single = CSng(p.bisonDuration)
		Dim val As Single = 0F
		Dim bulletDelay As Single = 10000F
		Dim bulletSpeed As Single = 0F
		Dim delay As Single = 0F
		Dim sameDirCount As Integer = 0
		Dim lastDir As FrogsLevelBisonBullet.Direction = FrogsLevelBisonBullet.Direction.Down
		Dim dir As FrogsLevelBisonBullet.Direction = FrogsLevelBisonBullet.Direction.Up
		While t < time
			If bulletDelay >= delay Then
				bulletSpeed = p.bisonSpeed.GetFloatAt(val)
				Me.ShootBison(bulletSpeed, dir, p.bisonBigX, p.bisonSmallX)
				bulletDelay = 0F
				lastDir = dir
				dir = CType(Global.UnityEngine.Random.Range(0, 2), FrogsLevelBisonBullet.Direction)
				If lastDir = dir Then
					sameDirCount += 1
				Else
					sameDirCount = 0
				End If
				If sameDirCount >= 3 Then
					If dir = FrogsLevelBisonBullet.Direction.Up Then
						dir = FrogsLevelBisonBullet.Direction.Down
					Else
						dir = FrogsLevelBisonBullet.Direction.Up
					End If
					sameDirCount = 0
				End If
			End If
			delay = p.bisonDelay.GetFloatAt(val)
			bulletDelay += CupheadTime.Delta
			If val < 1F Then
				val = t / time
				t += CupheadTime.Delta
			Else
				val = 1F
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06002443 RID: 9283 RVA: 0x00153BC4 File Offset: 0x00151FC4
	Private Iterator Function tiger_cr(p As LevelProperties.Frogs.Morph) As IEnumerator
		Dim t As Single = 0F
		Dim time As Single = p.tigerDuration
		Dim val As Single = 0F
		Dim bulletDelay As Single = 1000F
		Dim bulletSpeed As Single = 0F
		Dim delay As Single = 0F
		While t < time
			If bulletDelay >= delay Then
				bulletSpeed = p.tigerSpeed
				Me.ShootTiger(bulletSpeed)
				bulletDelay = 0F
			End If
			delay = p.tigerDelay.GetFloatAt(val)
			bulletDelay += CupheadTime.Delta
			If val < 1F Then
				val = t / time
				t += CupheadTime.Delta
			Else
				val = 1F
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06002444 RID: 9284 RVA: 0x00153BE8 File Offset: 0x00151FE8
	Private Iterator Function oni_cr() As IEnumerator
		Dim p As LevelProperties.Frogs.Demon = MyBase.properties.CurrentState.demon
		Dim bulletSpeed As Single = 0F
		Dim bulletDelay As Single = 1000F
		Dim delay As Single = 0F
		Dim val As Single = 0F
		Dim time As Single = p.demonMaxTime
		Dim t As Single = 0F
		While True
			Dim dir As FrogsLevelBisonBullet.Direction = CType(Global.UnityEngine.Random.Range(0, 2), FrogsLevelBisonBullet.Direction)
			Dim demonPattern As String() = p.demonString(Me.mainIndex).Split(New Char() { ","c })
			If bulletDelay >= delay Then
				demonPattern = p.demonString(Me.mainIndex).Split(New Char() { ","c })
				bulletSpeed = p.demonSpeed.GetFloatAt(val)
				Dim c As Char = demonPattern(Me.index)(0)
				If c <> "S"c Then
					If c <> "T"c Then
						If c <> "B"c Then
							If c = "O"c Then
								Me.ShootOni(bulletSpeed)
							End If
						Else
							dir = CType(Global.UnityEngine.Random.Range(0, 2), FrogsLevelBisonBullet.Direction)
							Me.ShootBison(bulletSpeed, dir, MyBase.properties.CurrentState.morph.bisonBigX, MyBase.properties.CurrentState.morph.bisonSmallX)
							If dir = FrogsLevelBisonBullet.Direction.Up Then
								dir = FrogsLevelBisonBullet.Direction.Down
							Else
								dir = FrogsLevelBisonBullet.Direction.Up
							End If
						End If
					Else
						Me.ShootTiger(bulletSpeed)
					End If
				Else
					Me.ShootSnake(bulletSpeed)
				End If
				If Me.index < demonPattern.Length - 1 Then
					Me.index += 1
				Else
					Me.mainIndex = (Me.mainIndex + 1) Mod p.demonString.Length
					Me.index = 0
				End If
				bulletDelay = 0F
			End If
			delay = p.demonDelay.GetFloatAt(val)
			bulletDelay += CupheadTime.Delta
			If val < 1F Then
				val = t / time
				t += CupheadTime.Delta
			Else
				val = 1F
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x04002CED RID: 11501
	Public Shared Current As FrogsLevelMorphed

	' Token: 0x04002CEE RID: 11502
	<SerializeField()>
	Private coin As FrogsLevelMorphedCoin

	' Token: 0x04002CEF RID: 11503
	<SerializeField()>
	Private coinRoot As Transform

	' Token: 0x04002CF0 RID: 11504
	<Space(10F)>
	Public switchRoot As Transform

	' Token: 0x04002CF1 RID: 11505
	<Space(10F)>
	<SerializeField()>
	Private slotsParent As GameObject

	' Token: 0x04002CF2 RID: 11506
	<SerializeField()>
	Private slots As Slots

	' Token: 0x04002CF3 RID: 11507
	<Space(10F)>
	<SerializeField()>
	Private snakeBullet As FrogsLevelSnakeBullet

	' Token: 0x04002CF4 RID: 11508
	<SerializeField()>
	Private bisonBullet As FrogsLevelBisonBullet

	' Token: 0x04002CF5 RID: 11509
	<SerializeField()>
	Private tigerBullet As FrogsLevelTigerBullet

	' Token: 0x04002CF6 RID: 11510
	<SerializeField()>
	Private oniBullet As FrogsLevelOniBullet

	' Token: 0x04002CF7 RID: 11511
	<SerializeField()>
	Private slotBulletRoot As Transform

	' Token: 0x04002CF8 RID: 11512
	<Space(10F)>
	<SerializeField()>
	Private dustEffect As Effect

	' Token: 0x04002CF9 RID: 11513
	Private damageReceiver As DamageReceiver

	' Token: 0x04002CFA RID: 11514
	Private damageDealer As DamageDealer

	' Token: 0x04002CFB RID: 11515
	Private handle As FrogsLevelMorphedSwitch

	' Token: 0x04002CFC RID: 11516
	Private demonTriggered As Boolean

	' Token: 0x04002CFD RID: 11517
	Private mainIndex As Integer

	' Token: 0x04002CFE RID: 11518
	Private index As Integer

	' Token: 0x04002CFF RID: 11519
	Private handleActivated As Boolean

	' Token: 0x04002D00 RID: 11520
	Private shootingCoroutine As IEnumerator

	' Token: 0x04002D01 RID: 11521
	Private coinSpeed As Single
End Class
