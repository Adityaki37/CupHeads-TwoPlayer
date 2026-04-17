Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x020004BE RID: 1214
Public Class AirplaneLevelLeader
	Inherits LevelProperties.Airplane.Entity

	' Token: 0x17000310 RID: 784
	' (get) Token: 0x06001418 RID: 5144 RVA: 0x000B2EF1 File Offset: 0x000B12F1
	' (set) Token: 0x06001419 RID: 5145 RVA: 0x000B2EF9 File Offset: 0x000B12F9
	Public Property IsAttacking As Boolean

	' Token: 0x17000311 RID: 785
	' (get) Token: 0x0600141A RID: 5146 RVA: 0x000B2F02 File Offset: 0x000B1302
	' (set) Token: 0x0600141B RID: 5147 RVA: 0x000B2F0A File Offset: 0x000B130A
	Public Property camRotatedHorizontally As Boolean

	' Token: 0x0600141C RID: 5148 RVA: 0x000B2F13 File Offset: 0x000B1313
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
	End Sub

	' Token: 0x0600141D RID: 5149 RVA: 0x000B2F3E File Offset: 0x000B133E
	Protected Overrides Sub OnDestroy()
		RemoveHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		MyBase.OnDestroy()
		Me.WORKAROUND_NullifyFields()
	End Sub

	' Token: 0x0600141E RID: 5150 RVA: 0x000B2F64 File Offset: 0x000B1364
	Public Overrides Sub LevelInit(properties As LevelProperties.Airplane)
		MyBase.LevelInit(properties)
		Me.bulletDelayString = New PatternString(properties.CurrentState.dropshot.bulletDelayStrings, True, True)
		Me.bulletColorString = New PatternString(properties.CurrentState.dropshot.bulletColorString, True, True)
		Me.laserPositionStringsMainIndex = Global.UnityEngine.Random.Range(0, properties.CurrentState.laser.laserPositionStrings.Length)
	End Sub

	' Token: 0x0600141F RID: 5151 RVA: 0x000B2FD0 File Offset: 0x000B13D0
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		If CType(Level.Current, AirplaneLevel).Rotating Then
			Return
		End If
		MyBase.properties.DealDamage(info.damage)
		If MyBase.properties.CurrentHealth <= 0F AndAlso Not Me.isDead Then
			Me.StopAllCoroutines()
			MyBase.StartCoroutine(Me.death_cr())
		End If
	End Sub

	' Token: 0x06001420 RID: 5152 RVA: 0x000B3036 File Offset: 0x000B1436
	Public Sub StartLeader()
		MyBase.animator.Play("Intro")
	End Sub

	' Token: 0x06001421 RID: 5153 RVA: 0x000B3048 File Offset: 0x000B1448
	Public Sub RotateCamera()
		Me.camRotatedHorizontally = Not Me.camRotatedHorizontally
	End Sub

	' Token: 0x06001422 RID: 5154 RVA: 0x000B3059 File Offset: 0x000B1459
	Private Sub AniEvent_PawGrab()
		CupheadLevelCamera.Current.Shake(30F, 0.8F, False)
		CType(Level.Current, AirplaneLevel).MoveBoundsIn()
		CType(Level.Current, AirplaneLevel).BlurBGCamera()
	End Sub

	' Token: 0x06001423 RID: 5155 RVA: 0x000B3090 File Offset: 0x000B1490
	Private Sub AniEvent_StartButtonPush()
		If MyBase.animator.GetCurrentAnimatorStateInfo(3).IsName("Push_Wait") Then
			MyBase.animator.Play("Push_Start", 3, 0F)
			MyBase.animator.Update(0F)
			AudioManager.Play("sfx_dlc_dogfight_leadervocal_buttonbashbegin")
			Me.emitAudioFromObject.Add("sfx_dlc_dogfight_leadervocal_buttonbashbegin")
		End If
	End Sub

	' Token: 0x06001424 RID: 5156 RVA: 0x000B30FC File Offset: 0x000B14FC
	Private Sub LateUpdate()
		If MyBase.animator.GetCurrentAnimatorStateInfo(3).IsName("Push") AndAlso MyBase.animator.GetCurrentAnimatorStateInfo(0).IsName("Sideways_Idle") AndAlso MyBase.animator.GetCurrentAnimatorStateInfo(3).normalizedTime <> MyBase.animator.GetCurrentAnimatorStateInfo(0).normalizedTime Then
			MyBase.animator.Play("Push", 3, MyBase.animator.GetCurrentAnimatorStateInfo(0).normalizedTime)
			MyBase.animator.Update(0F)
		End If
	End Sub

	' Token: 0x06001425 RID: 5157 RVA: 0x000B31A8 File Offset: 0x000B15A8
	Public Sub StartDropshot()
		MyBase.StartCoroutine(Me.drop_shot_cr())
	End Sub

	' Token: 0x06001426 RID: 5158 RVA: 0x000B31B8 File Offset: 0x000B15B8
	Private Iterator Function drop_shot_cr() As IEnumerator
		Me.IsAttacking = True
		Dim p As LevelProperties.Airplane.Dropshot = MyBase.properties.CurrentState.dropshot
		Me.bulletDelayString.SetSubStringIndex(0)
		Dim bullets As AirplaneLevelDropBullet() = New AirplaneLevelDropBullet(Me.bulletDelayString.SubStringLength() - 1) {}
		Dim onLeft As Boolean = True
		AudioManager.PlayLoop("sfx_dlc_dogfight_p3_leader_buttonpresses_loop")
		For i As Integer = 0 To bullets.Length - 1
			Yield CupheadTime.WaitForSeconds(Me, Me.bulletDelayString.PopFloat())
			Dim isRed As Boolean = Me.bulletColorString.PopLetter() = "R"c
			Dim bullet As AirplaneLevelDropBullet = If((Not isRed), Me.yellowBullet, Me.redBullet)
			Dim pos As Vector3 = Vector3.zero
			pos.x = If((Not onLeft), 380F, (-380F))
			pos.y = If((Not isRed), Me.yellowPosSideways.position.y, Me.redPosSideways.position.y)
			Dim startPos As Transform = If((Not onLeft), Me.rightDogBowlSpawn, Me.leftDogBowlSpawn)
			Dim b As AirplaneLevelDropBullet = bullet.Spawn()
			b.Init(pos, startPos.position, p.bulletDropSpeed, p.bulletShootSpeed, onLeft, Me.camRotatedHorizontally)
			bullets(i) = b
			AudioManager.Play("sfx_dlc_dogfight_p3_dogcopter_dogbowl_fire")
			Dim flashPos As Transform = If((Not onLeft), Me.flashRootRight, Me.flashRootLeft)
			Dim flash As Effect = Me.flashEffect.Create(flashPos.position, flashPos.localScale)
			flash.transform.rotation = flashPos.rotation
			onLeft = Not onLeft
		Next
		MyBase.animator.SetTrigger("EndButtonPush")
		AudioManager.FadeSFXVolume("sfx_dlc_dogfight_p3_leader_buttonpresses_loop", 0F, 0.25F)
		Dim stillAttacking As Boolean = True
		While stillAttacking
			Dim bulletsAlive As Boolean = False
			For Each airplaneLevelDropBullet As AirplaneLevelDropBullet In bullets
				If airplaneLevelDropBullet.isMoving Then
					bulletsAlive = True
				End If
			Next
			If Not bulletsAlive Then
				stillAttacking = False
				Exit While
			End If
			Yield Nothing
		End While
		Me.IsAttacking = False
		Yield Nothing
		Return
	End Function

	' Token: 0x06001427 RID: 5159 RVA: 0x000B31D4 File Offset: 0x000B15D4
	Public Sub OpenPawHoles()
		For i As Integer = 0 To Me.laserAnimator.Length - 1
			Me.laserAnimator(i).Play("SecretOpen")
		Next
	End Sub

	' Token: 0x06001428 RID: 5160 RVA: 0x000B320C File Offset: 0x000B160C
	Public Sub StartLaser()
		MyBase.StartCoroutine(Me.laser_main_cr())
	End Sub

	' Token: 0x06001429 RID: 5161 RVA: 0x000B321C File Offset: 0x000B161C
	Private Function GetLasersToShoot(lasers As String()) As List(Of Integer)
		Dim list As List(Of Integer) = New List(Of Integer)()
		For i As Integer = 0 To lasers.Length - 1
			list.Add(CInt((lasers(i)(0) - "A"c)))
		Next
		Return list
	End Function

	' Token: 0x0600142A RID: 5162 RVA: 0x000B3258 File Offset: 0x000B1658
	Private Iterator Function laser_main_cr() As IEnumerator
		Me.IsAttacking = True
		Dim p As LevelProperties.Airplane.Laser = MyBase.properties.CurrentState.laser
		Dim laserPositionStrings As String() = p.laserPositionStrings(Me.laserPositionStringsMainIndex).Split(New Char() { ","c })
		For i As Integer = 0 To laserPositionStrings.Length - 1
			Dim lasers As String() = laserPositionStrings(i).Split(New Char() { ":"c })
			Me.lasersToShoot = Me.GetLasersToShoot(lasers)
			If i + 1 < laserPositionStrings.Length Then
				Dim array As String() = laserPositionStrings(i + 1).Split(New Char() { ":"c })
				Me.lasersNextToShoot = Me.GetLasersToShoot(array)
			Else
				Me.lasersNextToShoot = New List(Of Integer)()
			End If
			MyBase.StartCoroutine(Me.fire_lasers_cr(Me.lasersToShoot, Me.lasersNextToShoot, i))
			Yield CupheadTime.WaitForSeconds(Me, Me.buildLaserAni.length + p.laserHesitation + p.warningTime + p.laserDuration + p.laserDelay)
		Next
		Yield CupheadTime.WaitForSeconds(Me, Me.buildLaserAni.length)
		Me.laserPositionStringsMainIndex = (Me.laserPositionStringsMainIndex + 1) Mod p.laserPositionStrings.Length
		Me.IsAttacking = False
		Yield Nothing
		Return
	End Function

	' Token: 0x0600142B RID: 5163 RVA: 0x000B3274 File Offset: 0x000B1674
	Private Iterator Function fire_lasers_cr(lasers As List(Of Integer), lasersNext As List(Of Integer), round As Integer) As IEnumerator
		Dim p As LevelProperties.Airplane.Laser = MyBase.properties.CurrentState.laser
		For i As Integer = 0 To lasers.Count - 1
			If Not Me.laserOut(lasers(i)) Then
				Me.laserAnimator(lasers(i)).Play("In")
			End If
			Me.laserOut(lasers(i)) = True
		Next
		AudioManager.Play("sfx_dlc_dogfight_p3_dogcopter_laser_buildout")
		Yield CupheadTime.WaitForSeconds(Me, Me.buildLaserAni.length)
		Yield CupheadTime.WaitForSeconds(Me, p.laserHesitation)
		For j As Integer = 0 To lasers.Count - 1
			Me.laserAnimator(lasers(j)).Play("WarningStart")
		Next
		AudioManager.Play("sfx_dlc_dogfight_p3_dogcopter_laser_prefire_warning")
		Yield CupheadTime.WaitForSeconds(Me, p.warningTime)
		For k As Integer = 0 To lasers.Count - 1
			Me.laserAnimator(lasers(k)).Play("FireStart")
		Next
		AudioManager.Play("sfx_dlc_dogfight_p3_dogcopter_laser_fire")
		Yield CupheadTime.WaitForSeconds(Me, p.laserDuration)
		Dim puttingAtLeastOneLaserAway As Boolean = False
		For l As Integer = 0 To lasers.Count - 1
			Me.laserOut(lasers(l)) = lasersNext.Contains(lasers(l)) AndAlso Not p.forceHide
			Me.laserAnimator(lasers(l)).SetBool("StayOut", Me.laserOut(lasers(l)))
			Me.laserAnimator(lasers(l)).Play("End")
			If Not Me.laserOut(lasers(l)) Then
				puttingAtLeastOneLaserAway = True
			End If
		Next
		If puttingAtLeastOneLaserAway Then
			AudioManager.Play("sfx_dlc_dogfight_p3_dogcopter_laser_unbuild")
		End If
		Return
	End Function

	' Token: 0x0600142C RID: 5164 RVA: 0x000B32A0 File Offset: 0x000B16A0
	Private Iterator Function rocket_cr() As IEnumerator
		Dim p As LevelProperties.Airplane.Rocket = MyBase.properties.CurrentState.rocket
		Dim delayMainIndex As Integer = Global.UnityEngine.Random.Range(0, p.attackDelayString.Length)
		Dim delayString As String() = p.attackDelayString(delayMainIndex).Split(New Char() { ","c })
		Dim delayIndex As Integer = Global.UnityEngine.Random.Range(0, delayString.Length)
		Dim dirMainIndex As Integer = Global.UnityEngine.Random.Range(0, p.attackOrderString.Length)
		Dim dirString As String() = p.attackOrderString(dirMainIndex).Split(New Char() { ","c })
		Dim dirIndex As Integer = Global.UnityEngine.Random.Range(0, dirString.Length)
		While True
			delayString = p.attackDelayString(delayMainIndex).Split(New Char() { ","c })
			dirString = p.attackOrderString(dirMainIndex).Split(New Char() { ","c })
			Dim delay As Integer = 0
			Parser.IntTryParse(delayString(delayIndex), delay)
			Yield CupheadTime.WaitForSeconds(Me, CSng(delay))
			Dim position As Vector3
			If dirString(dirIndex)(0) = "R"c Then
				position = Me.rocketSpawnRight.position
			Else
				position = Me.rocketSpawnLeft.position
			End If
			Me.rocketPrefab.Create(PlayerManager.GetNext(), position, p.homingSpeed, p.homingRotation, p.homingHP, p.homingTime)
			If dirIndex < dirString.Length - 1 Then
				dirIndex += 1
			Else
				dirMainIndex = (dirMainIndex + 1) Mod p.attackOrderString.Length
			End If
			If delayIndex < delayString.Length - 1 Then
				delayIndex += 1
			Else
				delayMainIndex = (delayMainIndex + 1) Mod p.attackDelayString.Length
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600142D RID: 5165 RVA: 0x000B32BC File Offset: 0x000B16BC
	Private Iterator Function death_cr() As IEnumerator
		Me.isDead = True
		MyBase.GetComponent(Of BoxCollider2D)().enabled = False
		Me.rotatedExploder.enabled = Me.camRotatedHorizontally
		Me.pawRightExploder.enabled = Not Me.camRotatedHorizontally
		Me.pawLeftExploder.enabled = Not Me.camRotatedHorizontally
		MyBase.animator.Play(If(Me.camRotatedHorizontally, "Copter_Death_Closeup", "Copter_Death"), MyBase.animator.GetLayerIndex("Death"))
		If Not Me.camRotatedHorizontally Then
			MyBase.animator.Play("Off")
			MyBase.animator.Play("Blades", MyBase.animator.GetLayerIndex("DeathBlades"))
		Else
			MyBase.animator.Play("Death_Closeup", 3)
			MyBase.animator.Play("SidewaysTears", 4)
		End If
		AudioManager.Play("sfx_dlc_dogfight_leadervocal_death")
		MyBase.animator.Update(0F)
		MyBase.StartCoroutine(Me.activate_death_puffs_cr())
		If Not Me.camRotatedHorizontally Then
			For i As Integer = 0 To Me.laserAnimator.Length - 1
				If Not Me.laserAnimator(i).GetCurrentAnimatorStateInfo(0).IsName("Off") Then
					Me.laserAnimator(i).Play(If((i <> 2), "Out", "Dead"), 0, Me.laserDeathTime(i))
				ElseIf i = 2 Then
					Me.laserAnimator(i).Play("SecretOpen")
				End If
			Next
			While PauseManager.state = PauseManager.State.Paused
				Yield Nothing
			End While
			For j As Integer = 0 To Me.laserAnimator.Length - 1
				Me.laserAnimator(j).GetComponent(Of AnimationHelper)().Speed = 1.25F
			Next
			While Not Me.laserAnimator(2).GetCurrentAnimatorStateInfo(0).IsName("HoldOpen")
				Yield Nothing
			End While
			CType(Level.Current, AirplaneLevel).LeaderDeath()
		End If
		Return
	End Function

	' Token: 0x0600142E RID: 5166 RVA: 0x000B32D8 File Offset: 0x000B16D8
	Private Iterator Function activate_death_puffs_cr() As IEnumerator
		While Me.deathPuffs.Count > 0
			Dim i As Integer = Global.UnityEngine.Random.Range(0, Me.deathPuffs.Count)
			Me.deathPuffs(i).gameObject.SetActive(True)
			If Me.camRotatedHorizontally Then
				Me.deathPuffs(i).Play("Sideways")
				Me.deathPuffs(i).Update(0F)
			End If
			Me.deathPuffs.RemoveAt(i)
			Yield CupheadTime.WaitForSeconds(Me, 0.16666667F)
		End While
		Return
	End Function

	' Token: 0x0600142F RID: 5167 RVA: 0x000B32F3 File Offset: 0x000B16F3
	Private Sub AnimationEvent_SFX_DOGFIGHT_P3_Dogcopter_ScreenRotateChomp()
		AudioManager.Play("sfx_dlc_dogfight_p3_dogcopter_screenrotate_chomp")
	End Sub

	' Token: 0x06001430 RID: 5168 RVA: 0x000B32FF File Offset: 0x000B16FF
	Private Sub AnimationEvent_SFX_DOGFIGHT_P3_Dogcopter_ScreenRotate()
		AudioManager.Play("sfx_DLC_Dogfight_P3_DogCopter_ScreenRotate")
	End Sub

	' Token: 0x06001431 RID: 5169 RVA: 0x000B330B File Offset: 0x000B170B
	Private Sub AnimationEvent_SFX_DOGFIGHT_P3_Dogcopter_GrabScreen()
		AudioManager.Play("sfx_dlc_dogfight_p3_dogcopter_settle_grabscreen")
	End Sub

	' Token: 0x06001432 RID: 5170 RVA: 0x000B3317 File Offset: 0x000B1717
	Private Sub AnimationEvent_SFX_DOGFIGHT_P3_Dogcopter_Intro()
		AudioManager.Play("sfx_dlc_dogfight_p3_dogcopter_intro")
	End Sub

	' Token: 0x06001433 RID: 5171 RVA: 0x000B3323 File Offset: 0x000B1723
	Private Sub AnimationEvent_SFX_DOGFIGHT_P3_Dogcopter_Intro2()
		AudioManager.Play("sfx_dlc_dogfight_p3_dogcopter_intro2")
	End Sub

	' Token: 0x06001434 RID: 5172 RVA: 0x000B332F File Offset: 0x000B172F
	Private Sub AnimationEvent_SFX_DOGFIGHT_P3_LeaderVocalEnd()
		AudioManager.Play("sfx_dlc_dogfight_leadervocal_command")
	End Sub

	' Token: 0x06001435 RID: 5173 RVA: 0x000B333C File Offset: 0x000B173C
	Private Sub WORKAROUND_NullifyFields()
		Me.laserPositions = Nothing
		Me.rocketSpawnLeft = Nothing
		Me.rocketSpawnRight = Nothing
		Me.yellowPosSideways = Nothing
		Me.redPosSideways = Nothing
		Me.flashRootLeft = Nothing
		Me.flashRootRight = Nothing
		Me.leftDogBowlSpawn = Nothing
		Me.rightDogBowlSpawn = Nothing
		Me.buildLaserAni = Nothing
		Me.rocketPrefab = Nothing
		Me.yellowBullet = Nothing
		Me.redBullet = Nothing
		Me.laserAnimator = Nothing
		Me.flashEffect = Nothing
		Me.lasersToShoot = Nothing
		Me.lasersNextToShoot = Nothing
		Me.laserOut = Nothing
		Me.laserDeathTime = Nothing
		Me.bulletDelayString = Nothing
		Me.bulletColorString = Nothing
		Me.rotatedExploder = Nothing
		Me.pawRightExploder = Nothing
		Me.pawLeftExploder = Nothing
		Me.deathPuffs = Nothing
	End Sub

	' Token: 0x04001D41 RID: 7489
	Private Const PAW_MOVE_X As Single = 750F

	' Token: 0x04001D42 RID: 7490
	Private Const BULLET_SPAWN_X As Single = 380F

	' Token: 0x04001D43 RID: 7491
	<Header("Spawn Positions")>
	<SerializeField()>
	Private laserPositions As Transform()

	' Token: 0x04001D44 RID: 7492
	<SerializeField()>
	Private rocketSpawnLeft As Transform

	' Token: 0x04001D45 RID: 7493
	<SerializeField()>
	Private rocketSpawnRight As Transform

	' Token: 0x04001D46 RID: 7494
	<SerializeField()>
	Private yellowPosSideways As Transform

	' Token: 0x04001D47 RID: 7495
	<SerializeField()>
	Private redPosSideways As Transform

	' Token: 0x04001D48 RID: 7496
	<SerializeField()>
	Private flashRootLeft As Transform

	' Token: 0x04001D49 RID: 7497
	<SerializeField()>
	Private flashRootRight As Transform

	' Token: 0x04001D4A RID: 7498
	<SerializeField()>
	Private leftDogBowlSpawn As Transform

	' Token: 0x04001D4B RID: 7499
	<SerializeField()>
	Private rightDogBowlSpawn As Transform

	' Token: 0x04001D4C RID: 7500
	<SerializeField()>
	Private buildLaserAni As AnimationClip

	' Token: 0x04001D4D RID: 7501
	<Header("Prefabs")>
	<SerializeField()>
	Private rocketPrefab As AirplaneLevelRocket

	' Token: 0x04001D4E RID: 7502
	<SerializeField()>
	Private yellowBullet As AirplaneLevelDropBullet

	' Token: 0x04001D4F RID: 7503
	<SerializeField()>
	Private redBullet As AirplaneLevelDropBullet

	' Token: 0x04001D50 RID: 7504
	<SerializeField()>
	Private laserAnimator As Animator()

	' Token: 0x04001D51 RID: 7505
	<SerializeField()>
	Private flashEffect As Effect

	' Token: 0x04001D54 RID: 7508
	Private lasersToShoot As List(Of Integer)

	' Token: 0x04001D55 RID: 7509
	Private lasersNextToShoot As List(Of Integer)

	' Token: 0x04001D56 RID: 7510
	Private laserOut As Boolean() = New Boolean(4) {}

	' Token: 0x04001D57 RID: 7511
	Private laserDeathTime As Single() = New Single() { 0.2F, 0.6F, 0.8F, 0.26666668F, 0.4F }

	' Token: 0x04001D58 RID: 7512
	Private bulletDelayString As PatternString

	' Token: 0x04001D59 RID: 7513
	Private bulletColorString As PatternString

	' Token: 0x04001D5A RID: 7514
	Private laserPositionStringsMainIndex As Integer

	' Token: 0x04001D5B RID: 7515
	Private isDead As Boolean

	' Token: 0x04001D5C RID: 7516
	Private damageReceiver As DamageReceiver

	' Token: 0x04001D5D RID: 7517
	<SerializeField()>
	Private rotatedExploder As LevelBossDeathExploder

	' Token: 0x04001D5E RID: 7518
	<SerializeField()>
	Private pawRightExploder As LevelBossDeathExploder

	' Token: 0x04001D5F RID: 7519
	<SerializeField()>
	Private pawLeftExploder As LevelBossDeathExploder

	' Token: 0x04001D60 RID: 7520
	<SerializeField()>
	Private deathPuffs As List(Of Animator)
End Class
