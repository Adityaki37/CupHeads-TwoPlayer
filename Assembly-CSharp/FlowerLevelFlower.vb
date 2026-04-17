Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Diagnostics
Imports UnityEngine

' Token: 0x02000608 RID: 1544
Public Class FlowerLevelFlower
	Inherits LevelProperties.Flower.Entity

	' Token: 0x14000042 RID: 66
	' (add) Token: 0x06001ED0 RID: 7888 RVA: 0x0011B540 File Offset: 0x00119940
	' (remove) Token: 0x06001ED1 RID: 7889 RVA: 0x0011B578 File Offset: 0x00119978
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnDeathEvent As Action

	' Token: 0x14000043 RID: 67
	' (add) Token: 0x06001ED2 RID: 7890 RVA: 0x0011B5B0 File Offset: 0x001199B0
	' (remove) Token: 0x06001ED3 RID: 7891 RVA: 0x0011B5E8 File Offset: 0x001199E8
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnStateChanged As Action

	' Token: 0x06001ED4 RID: 7892 RVA: 0x0011B61E File Offset: 0x00119A1E
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		MyBase.properties.DealDamage(info.damage)
	End Sub

	' Token: 0x06001ED5 RID: 7893 RVA: 0x0011B631 File Offset: 0x00119A31
	Public Sub AdditionDamageTaken(info As DamageDealer.DamageInfo)
		Me.OnDamageTaken(info)
	End Sub

	' Token: 0x06001ED6 RID: 7894 RVA: 0x0011B63A File Offset: 0x00119A3A
	Public Sub PhaseTwoTrigger()
		MyBase.animator.SetTrigger("PhaseTwoTransition")
	End Sub

	' Token: 0x06001ED7 RID: 7895 RVA: 0x0011B64C File Offset: 0x00119A4C
	Private Sub Die()
		Me.isDead = True
		MyBase.StartCoroutine(Me.die_cr())
	End Sub

	' Token: 0x06001ED8 RID: 7896 RVA: 0x0011B662 File Offset: 0x00119A62
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.boomerangPrefab = Nothing
		Me.bulletSeedPrefab = Nothing
		Me.cloudBombPrefab = Nothing
		Me.enemySeedPrefab = Nothing
		Me.pollenProjectile = Nothing
		Me.gattlingFX = Nothing
		Me.vineHandPrefab = Nothing
	End Sub

	' Token: 0x06001ED9 RID: 7897 RVA: 0x0011B69B File Offset: 0x00119A9B
	Private Sub SpawnMainVine()
		If Me.OnStateChanged IsNot Nothing Then
			Me.OnStateChanged()
		End If
		Me.mainVine.SetActive(True)
		MyBase.animator.SetTrigger("SpawnMainVine")
	End Sub

	' Token: 0x06001EDA RID: 7898 RVA: 0x0011B6CF File Offset: 0x00119ACF
	Private Sub MainVineSpawned()
		MyBase.StartCoroutine(Me.vineHands_cr())
		Me.projectileSpawned = False
		MyBase.StartCoroutine(Me.pollenAttack_cr())
		Me.attackCount = 1
	End Sub

	' Token: 0x06001EDB RID: 7899 RVA: 0x0011B6FC File Offset: 0x00119AFC
	Private Iterator Function die_cr() As IEnumerator
		If Me.OnDeathEvent IsNot Nothing Then
			Me.OnDeathEvent()
		End If
		Me.StopAllCoroutines()
		MyBase.GetComponent(Of Collider2D)().enabled = False
		If Level.Current.mode = Level.Mode.Easy Then
			MyBase.animator.Play("Phase One Death")
		Else
			MyBase.animator.Play("Phase Two Death")
		End If
		Yield Nothing
		MyBase.animator.enabled = False
		MyBase.animator.enabled = True
		MyBase.properties.WinInstantly()
		Return
	End Function

	' Token: 0x06001EDC RID: 7900 RVA: 0x0011B718 File Offset: 0x00119B18
	Public Overrides Sub LevelInit(properties As LevelProperties.Flower)
		AddHandler properties.OnBossDeath, AddressOf Me.Phase2DeathAudio
		AddHandler properties.OnBossDeath, AddressOf Me.Die
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		MyBase.LevelInit(properties)
		Me.attackCount = 0
		Me.miniFlowerSpawned = False
		AddHandler Level.Current.OnIntroEvent, AddressOf Me.OnIntro
		Dim num As Integer = Global.UnityEngine.Random.Range(0, properties.CurrentState.laser.attackType.Split(New Char() { ","c }).Length)
		Me.currentLaserAttack = num
		Me.currentGattlingGunAttackPattern = New List(Of String)()
		Me.currentGattlingGunAttackString = Global.UnityEngine.Random.Range(0, properties.CurrentState.gattlingGun.seedSpawnString.Length)
		Dim array As String() = properties.CurrentState.vineHands.handAttackString.Split(New Char() { ","c })
		Me.currentVineHandsAttack = Global.UnityEngine.Random.Range(0, array.Length)
		Me.pollenAttackCount = Global.UnityEngine.Random.Range(0, properties.CurrentState.pollenSpit.pollenAttackCount.Split(New Char() { ","c }).Length)
		Me.currentPollenType = Global.UnityEngine.Random.Range(0, properties.CurrentState.pollenSpit.pollenType.Split(New Char() { ","c }).Length)
		MyBase.StartCoroutine(Me.find_s_cr())
	End Sub

	' Token: 0x06001EDD RID: 7901 RVA: 0x0011B894 File Offset: 0x00119C94
	Private Iterator Function find_s_cr() As IEnumerator
		While MyBase.properties.CurrentState.podHands.attacktype.Split(New Char() { ","c })(Me.currentPodHandsAttack)(0) <> "S"c
			Me.currentPodHandsAttack = Global.UnityEngine.Random.Range(0, MyBase.properties.CurrentState.podHands.attacktype.Split(New Char() { ","c }).Length)
			Yield Nothing
		End While
		Me.podHandsAttackCountTarget = Global.UnityEngine.Random.Range(0, MyBase.properties.CurrentState.podHands.attackAmount.Split(New Char() { ","c }).Length)
		Yield Nothing
		Return
	End Function

	' Token: 0x06001EDE RID: 7902 RVA: 0x0011B8AF File Offset: 0x00119CAF
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06001EDF RID: 7903 RVA: 0x0011B8C7 File Offset: 0x00119CC7
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06001EE0 RID: 7904 RVA: 0x0011B8E8 File Offset: 0x00119CE8
	Public Sub StartLaser(callback As Action)
		Me.attackCallback = callback
		Me.attackType = MyBase.properties.CurrentState.laser.attackType.Split(New Char() { ","c })(Me.currentLaserAttack)(0)
		Me.attackCharge = MyBase.properties.CurrentState.laser.anticHold
		Me.OnLaserStarted()
	End Sub

	' Token: 0x06001EE1 RID: 7905 RVA: 0x0011B958 File Offset: 0x00119D58
	Public Sub OnLaserStarted()
		If Me.attackType.Equals("T"c) Then
			Me.topLaserAttack = True
		Else
			Me.topLaserAttack = False
		End If
		If Me.topLaserAttack Then
			MyBase.animator.SetBool("TopLaser", True)
		Else
			MyBase.animator.SetBool("BottomLaser", True)
		End If
		MyBase.StartCoroutine(Me.laserCharge_cr())
	End Sub

	' Token: 0x06001EE2 RID: 7906 RVA: 0x0011B9CC File Offset: 0x00119DCC
	Private Iterator Function laserCharge_cr() As IEnumerator
		If Me.topLaserAttack Then
			Yield MyBase.animator.WaitForAnimationToEnd(Me, "TopLaserAttackStart", True, True)
		Else
			Yield MyBase.animator.WaitForAnimationToEnd(Me, "BottomLaserAttackStart", True, True)
		End If
		Yield CupheadTime.WaitForSeconds(Me, Me.attackCharge)
		MyBase.animator.SetTrigger("OnAttackChargeComplete")
		Return
	End Function

	' Token: 0x06001EE3 RID: 7907 RVA: 0x0011B9E7 File Offset: 0x00119DE7
	Private Sub OnHoldComplete()
		MyBase.StartCoroutine(Me.onLaser_cr())
	End Sub

	' Token: 0x06001EE4 RID: 7908 RVA: 0x0011B9F8 File Offset: 0x00119DF8
	Private Iterator Function onLaser_cr() As IEnumerator
		Me.attackCharge = MyBase.properties.CurrentState.laser.attackHold
		Yield CupheadTime.WaitForSeconds(Me, Me.attackCharge)
		If Me.topLaserAttack Then
			MyBase.animator.SetBool("TopLaser", False)
		Else
			MyBase.animator.SetBool("BottomLaser", False)
		End If
		If Me.topLaserAttack Then
			Yield MyBase.animator.WaitForAnimationToEnd(Me, "TopLaserAttackEnd", True, True)
		Else
			Yield MyBase.animator.WaitForAnimationToEnd(Me, "BottomLaserAttackEnd", True, True)
		End If
		Return
	End Function

	' Token: 0x06001EE5 RID: 7909 RVA: 0x0011BA14 File Offset: 0x00119E14
	Public Sub OnLaserComplete()
		Me.currentLaserAttack += 1
		If Me.currentLaserAttack >= MyBase.properties.CurrentState.laser.attackType.Split(New Char() { ","c }).Length Then
			Me.currentLaserAttack = 0
		End If
		Me.topLaserAttack = False
		If Me.attackCallback IsNot Nothing Then
			Me.attackCallback()
		End If
		Me.attackCallback = Nothing
	End Sub

	' Token: 0x06001EE6 RID: 7910 RVA: 0x0011BA8C File Offset: 0x00119E8C
	Public Sub StartPotHands(callback As Action)
		Me.attackCount = 0
		If Me.podHandsAttackCountTarget >= MyBase.properties.CurrentState.podHands.attackAmount.Split(New Char() { ","c }).Length Then
			Me.podHandsAttackCountTarget = 0
		End If
		Me.attackCountTarget = Parser.IntParse(MyBase.properties.CurrentState.podHands.attackAmount.Split(New Char() { ","c })(Me.podHandsAttackCountTarget).ToString())
		Me.attackType = MyBase.properties.CurrentState.podHands.attacktype.Split(New Char() { ","c })(Me.currentPodHandsAttack)(0)
		Me.attackCallback = callback
		Me.attackCharge = MyBase.properties.CurrentState.podHands.attackHold
		Me.OnPotHandsStarted()
	End Sub

	' Token: 0x06001EE7 RID: 7911 RVA: 0x0011BB75 File Offset: 0x00119F75
	Public Sub OnPotHandsStarted()
		MyBase.animator.SetBool("PotHandsAttack", True)
		MyBase.StartCoroutine(Me.potHandsHold_cr())
		Me.attackCount += 1
	End Sub

	' Token: 0x06001EE8 RID: 7912 RVA: 0x0011BBA4 File Offset: 0x00119FA4
	Private Iterator Function potHandsHold_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, Me.attackCharge)
		MyBase.animator.SetTrigger("OnAttackChargeComplete")
		Me.OpenPotHands()
		Return
	End Function

	' Token: 0x06001EE9 RID: 7913 RVA: 0x0011BBBF File Offset: 0x00119FBF
	Private Sub OpenPotHands()
		Me.attackCharge = MyBase.properties.CurrentState.podHands.attackDelay
	End Sub

	' Token: 0x06001EEA RID: 7914 RVA: 0x0011BBDC File Offset: 0x00119FDC
	Public Sub OnPotHandsComplete()
		Me.projectileSpawned = False
		If Me.attackCount >= Me.attackCountTarget Then
			Me.attackCount = 0
			Me.podHandsAttackCountTarget += 1
			If Me.podHandsAttackCountTarget >= MyBase.properties.CurrentState.podHands.attackAmount.Split(New Char() { ","c }).Length Then
				Me.podHandsAttackCountTarget = 0
			End If
			MyBase.animator.SetTrigger("OnAttackComplete")
			If Me.attackCallback IsNot Nothing Then
				Me.attackCallback()
				Me.attackCallback = Nothing
			End If
		End If
		MyBase.animator.SetBool("PotHandsAttack", False)
	End Sub

	' Token: 0x06001EEB RID: 7915 RVA: 0x0011BC8D File Offset: 0x0011A08D
	Public Sub StartGattlingGun(callback As Action)
		Me.attackCallback = callback
		Me.attackCharge = MyBase.properties.CurrentState.gattlingGun.loopDuration
		MyBase.animator.SetBool("GattlingGunAttack", True)
		Me.attackType = "G"c
	End Sub

	' Token: 0x06001EEC RID: 7916 RVA: 0x0011BCCC File Offset: 0x0011A0CC
	Private Iterator Function startGattlingFX_cr() As IEnumerator
		Dim animAttrib As String = "GattlingGunAttack"
		Dim target As Integer = Animator.StringToHash(MyBase.animator.GetLayerName(0) + ".GattlingGunStart")
		If target = MyBase.animator.GetCurrentAnimatorStateInfo(0).fullPathHash Then
			Yield MyBase.animator.WaitForAnimationToEnd(Me, "GattlingGunStart", True, True)
		End If
		While MyBase.animator.GetBool(animAttrib)
			Dim fxObject As GameObject = Global.UnityEngine.[Object].Instantiate(Of GameObject)(Me.gattlingFX, Me.topProjectileSpawnPoint.position, Quaternion.identity)
			Dim fx As Animator = fxObject.GetComponent(Of Animator)()
			Yield MyBase.StartCoroutine(Me.killGattlingFX_cr(fx))
		End While
		Return
	End Function

	' Token: 0x06001EED RID: 7917 RVA: 0x0011BCE8 File Offset: 0x0011A0E8
	Private Iterator Function killGattlingFX_cr(fx As Animator) As IEnumerator
		Yield fx.WaitForAnimationToEnd(Me, True)
		Global.UnityEngine.[Object].Destroy(fx.gameObject)
		Return
	End Function

	' Token: 0x06001EEE RID: 7918 RVA: 0x0011BD0A File Offset: 0x0011A10A
	Public Sub OnGattlingGunEnded()
		MyBase.animator.SetBool("GattlingGunAttack", False)
		Me.OnGattlingGunComplete()
	End Sub

	' Token: 0x06001EEF RID: 7919 RVA: 0x0011BD23 File Offset: 0x0011A123
	Public Sub OnGattlingGunComplete()
		If Me.attackCallback IsNot Nothing Then
			Me.attackCallback()
		End If
		Me.attackCallback = Nothing
	End Sub

	' Token: 0x06001EF0 RID: 7920 RVA: 0x0011BD44 File Offset: 0x0011A144
	Private Sub AddAttackTypes(s As String())
		For i As Integer = 0 To s.Length - 1
			Me.currentGattlingGunAttackPattern.Add(s(i))
		Next
	End Sub

	' Token: 0x06001EF1 RID: 7921 RVA: 0x0011BD73 File Offset: 0x0011A173
	Private Sub StartVineHandsAttack()
		MyBase.StartCoroutine(Me.vineHands_cr())
	End Sub

	' Token: 0x06001EF2 RID: 7922 RVA: 0x0011BD84 File Offset: 0x0011A184
	Private Iterator Function vineHands_cr() As IEnumerator
		While Not Me.isDead
			Dim attackPositions As String() = MyBase.properties.CurrentState.vineHands.handAttackString.Split(New Char() { ","c })
			Dim currentWave As String() = attackPositions(Me.currentVineHandsAttack).Split(New Char() { "-"c })
			If attackPositions(Me.currentVineHandsAttack)(0) <> "D"c Then
				If currentWave.Length > 1 Then
					Me.currentVineHandsAttack += 2
					Me.vineHandPrefab.GetComponent(Of FlowerLevelFlowerVineHand)().OnVineHandSpawn(MyBase.properties.CurrentState.vineHands.firstPositionHold, MyBase.properties.CurrentState.vineHands.secondPositionHold, Parser.IntParse(currentWave(0)), Parser.IntParse(currentWave(1)))
				Else
					Me.currentVineHandsAttack += 1
					Me.vineHandPrefab.GetComponent(Of FlowerLevelFlowerVineHand)().OnVineHandSpawn(MyBase.properties.CurrentState.vineHands.firstPositionHold, MyBase.properties.CurrentState.vineHands.secondPositionHold, Parser.IntParse(currentWave(0)), 0)
				End If
			Else
				Yield CupheadTime.WaitForSeconds(Me, Parser.FloatParse(attackPositions(Me.currentVineHandsAttack).Substring(1)))
			End If
			If Me.currentVineHandsAttack >= attackPositions.Length Then
				Me.currentVineHandsAttack = 0
			End If
			Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.vineHands.attackDelay.RandomFloat())
		End While
		Return
	End Function

	' Token: 0x06001EF3 RID: 7923 RVA: 0x0011BDA0 File Offset: 0x0011A1A0
	Private Iterator Function pollenAttack_cr() As IEnumerator
		While Not Me.isDead
			If Not Me.projectileSpawned Then
				Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.pollenSpit.pollenCommaDelay)
				Dim delay As String = MyBase.properties.CurrentState.pollenSpit.pollenAttackCount.Split(New Char() { ","c })(Me.pollenAttackCount).ToString()
				If delay(0).Equals("D"c) Then
					Yield CupheadTime.WaitForSeconds(Me, Parser.FloatParse(delay.Substring(1).ToString()))
				Else
					Me.attackCountTarget = Parser.IntParse(delay)
				End If
				Me.pollenAttackCount += 1
				If Me.pollenAttackCount >= MyBase.properties.CurrentState.pollenSpit.pollenAttackCount.Split(New Char() { ","c }).Length Then
					Me.pollenAttackCount = 0
				End If
				Me.projectileSpawned = True
				MyBase.animator.SetBool("OnPollenAttack", True)
			Else
				Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.pollenSpit.consecutiveAttackHold)
				MyBase.animator.SetTrigger("OnAttackChargeComplete")
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06001EF4 RID: 7924 RVA: 0x0011BDBC File Offset: 0x0011A1BC
	Private Sub launchPollen()
		Dim text As String = MyBase.properties.CurrentState.pollenSpit.pollenType.Split(New Char() { ","c })(Me.currentPollenType)
		Dim num As Integer
		If text(0).Equals("R"c) Then
			num = 0
		Else
			num = 1
		End If
		Me.currentPollenType += 1
		If Me.currentPollenType >= MyBase.properties.CurrentState.pollenSpit.pollenType.Split(New Char() { ","c }).Length Then
			Me.currentPollenType = 0
		End If
		Dim gameObject As GameObject = Global.UnityEngine.[Object].Instantiate(Of GameObject)(Me.pollenProjectile, Me.topProjectileSpawnPoint.position, Quaternion.identity)
		Me.currentPollenShot = gameObject.GetComponent(Of FlowerLevelPollenProjectile)()
		Me.currentPollenShot.InitPollen(CSng(MyBase.properties.CurrentState.pollenSpit.pollenSpeed), MyBase.properties.CurrentState.pollenSpit.pollenUpDownStrength, num, Me.topProjectileSpawnPoint)
		AudioManager.Play("flower_phase2_spit_projectile")
		Me.attackCount += 1
		If Me.attackCount > Me.attackCountTarget Then
			MyBase.animator.SetBool("OnPollenAttack", False)
			Me.attackCount = 1
			Me.projectileSpawned = False
		Else
			Me.projectileSpawned = True
		End If
	End Sub

	' Token: 0x06001EF5 RID: 7925 RVA: 0x0011BF17 File Offset: 0x0011A317
	Private Sub PollenShotEnd()
		Me.currentPollenShot.StartMoving()
	End Sub

	' Token: 0x06001EF6 RID: 7926 RVA: 0x0011BF24 File Offset: 0x0011A324
	Private Sub OnIntro()
		MyBase.animator.SetTrigger("OnIntroEnded")
	End Sub

	' Token: 0x06001EF7 RID: 7927 RVA: 0x0011BF38 File Offset: 0x0011A338
	Private Sub SpawnProjectile()
		Dim c As Char = Me.attackType
		If c <> "R"c Then
			If c <> "S"c Then
				If c <> "B"c Then
					If c = "G"c Then
						MyBase.StartCoroutine(Me.spawnGattlingGunSeeds_cr())
					End If
				Else
					Me.SpawnBoomerang()
				End If
			Else
				Me.SpawnBullets()
			End If
		Else
			Me.SpawnCloudShot()
		End If
		Me.currentPodHandsAttack += 1
		If Me.currentPodHandsAttack >= MyBase.properties.CurrentState.podHands.attacktype.Split(New Char() { ","c }).Length Then
			Me.currentPodHandsAttack = 0
		End If
		Me.attackType = MyBase.properties.CurrentState.podHands.attacktype.Split(New Char() { ","c })(Me.currentPodHandsAttack)(0)
	End Sub

	' Token: 0x06001EF8 RID: 7928 RVA: 0x0011C028 File Offset: 0x0011A428
	Private Iterator Function spawnGattlingGunSeeds_cr() As IEnumerator
		MyBase.StartCoroutine(Me.startGattlingFX_cr())
		Me.currentGattlingGunAttackPattern.Clear()
		Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.gattlingGun.initialSeedDelay)
		Dim projectileAttributes As String() = MyBase.properties.CurrentState.gattlingGun.seedSpawnString(Me.currentGattlingGunAttackString).Split(New Char() { ","c })
		Dim delayNextProjectileWave As Single = MyBase.properties.CurrentState.gattlingGun.fallingSeedDelay
		For j As Integer = 0 To projectileAttributes.Length - 1
			Dim array As String() = projectileAttributes(j).Split(New Char() { "-"c })
			If array.Length > 1 Then
				Me.AddAttackTypes(array)
			Else
				Me.currentGattlingGunAttackPattern.Add(projectileAttributes(j))
			End If
			Me.currentGattlingGunAttackPattern.Add("D" + MyBase.properties.CurrentState.gattlingGun.fallingSeedDelay.ToStringInvariant())
		Next
		For i As Integer = 0 To Me.currentGattlingGunAttackPattern.Count - 1
			Dim t As Char = Me.currentGattlingGunAttackPattern(i)(0)
			If t = "D"c Then
				Yield CupheadTime.WaitForSeconds(Me, Parser.FloatParse(Me.currentGattlingGunAttackPattern(i).Substring(1)))
			Else
				If Me.miniFlowerSpawned Then
					If t <> "C"c Then
						Me.SpawnEnemySeed(Parser.IntParse(Me.currentGattlingGunAttackPattern(i).Substring(1)), t, True)
					Else
						Me.SpawnEnemySeed(Parser.IntParse(Me.currentGattlingGunAttackPattern(i).Substring(1)), t, False)
					End If
				Else
					Me.SpawnEnemySeed(Parser.IntParse(Me.currentGattlingGunAttackPattern(i).Substring(1)), t, True)
				End If
				If t = "C"c Then
					Me.miniFlowerSpawned = True
				End If
			End If
		Next
		Yield CupheadTime.WaitForSeconds(Me, delayNextProjectileWave)
		Me.currentGattlingGunAttackString += 1
		If Me.currentGattlingGunAttackString >= MyBase.properties.CurrentState.gattlingGun.seedSpawnString.Length Then
			Me.currentGattlingGunAttackString = 0
		End If
		AudioManager.[Stop]("flower_gattling_gun_loop")
		Me.OnGattlingGunEnded()
		Return
	End Function

	' Token: 0x06001EF9 RID: 7929 RVA: 0x0011C043 File Offset: 0x0011A443
	Public Sub OnMiniFlowerDeath()
		Me.miniFlowerSpawned = False
	End Sub

	' Token: 0x06001EFA RID: 7930 RVA: 0x0011C04C File Offset: 0x0011A44C
	Private Sub SpawnEnemySeed(xPos As Integer, t As Char, Optional a As Boolean = True)
		Dim gameObject As GameObject = Global.UnityEngine.[Object].Instantiate(Of GameObject)(Me.enemySeedPrefab)
		gameObject.transform.position = New Vector3(CSng((-600 + xPos)), CSng(Level.Current.Height), 0F)
		gameObject.GetComponent(Of FlowerLevelEnemySeed)().OnSeedSpawn(MyBase.properties, Me, t, a)
	End Sub

	' Token: 0x06001EFB RID: 7931 RVA: 0x0011C0A4 File Offset: 0x0011A4A4
	Private Sub SpawnBoomerang()
		Dim basicProjectile As BasicProjectile = Me.boomerangPrefab.GetComponent(Of FlowerLevelBoomerang)().Create(Me.bottomProjectileSpawnPoint.position + (Me.topProjectileSpawnPoint.position - Me.bottomProjectileSpawnPoint.position) / 2F, 0F, 0F)
		MyBase.StartCoroutine(Me.spawnBoomerang_cr(basicProjectile))
	End Sub

	' Token: 0x06001EFC RID: 7932 RVA: 0x0011C114 File Offset: 0x0011A514
	Private Iterator Function spawnBoomerang_cr(proj As BasicProjectile) As IEnumerator
		proj.GetComponent(Of FlowerLevelBoomerang)().OnBoomerangStart(MyBase.properties.CurrentState.boomerang.offScreenDelay)
		Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.boomerang.initialMovementDelay)
		proj.GetComponent(Of BasicProjectile)().Speed = CSng((-CSng(MyBase.properties.CurrentState.boomerang.speed)))
		Me.OnPotHandsComplete()
		Return
	End Function

	' Token: 0x06001EFD RID: 7933 RVA: 0x0011C138 File Offset: 0x0011A538
	Private Sub SpawnBullets()
		Me.bulletSpawns.Clear()
		For i As Integer = 0 To MyBase.properties.CurrentState.bullets.numberOfProjectiles - 1
			Me.bulletSpawns.Add(Me.bulletSeedPrefab.GetComponent(Of FlowerLevelSeedBullet)().Create(Vector2.zero))
			Dim vector As Vector3 = Me.bottomProjectileSpawnPoint.position + (Me.topProjectileSpawnPoint.position - Me.bottomProjectileSpawnPoint.position) / CSng((MyBase.properties.CurrentState.bullets.numberOfProjectiles - 1)) * CSng(i)
			Me.bulletSpawns(Me.bulletSpawns.Count - 1).transform.position = vector
		Next
		MyBase.StartCoroutine(Me.spawnBullets_cr())
	End Sub

	' Token: 0x06001EFE RID: 7934 RVA: 0x0011C218 File Offset: 0x0011A618
	Private Iterator Function spawnBullets_cr() As IEnumerator
		Dim bullets As List(Of AbstractProjectile) = New List(Of AbstractProjectile)()
		Dim activeBullets As List(Of AbstractProjectile) = Me.bulletSpawns
		For i As Integer = 0 To MyBase.properties.CurrentState.bullets.numberOfProjectiles - 1
			Dim delay As Single = CSng(MyBase.properties.CurrentState.bullets.holdDelay) / CSng(MyBase.properties.CurrentState.bullets.numberOfProjectiles)
			Dim rand As Integer = Global.UnityEngine.Random.Range(0, activeBullets.Count)
			bullets.Add(activeBullets(rand))
			bullets(i).GetComponent(Of FlowerLevelSeedBullet)().OnBulletSeedStart(Me, PlayerManager.GetNext(), MyBase.properties.CurrentState.bullets.acceleration, MyBase.properties.CurrentState.bullets.speedMinMax.min, MyBase.properties.CurrentState.bullets.speedMinMax.max)
			activeBullets.RemoveAt(rand)
			Yield CupheadTime.WaitForSeconds(Me, delay)
		Next
		Yield Nothing
		For j As Integer = 0 To bullets.Count - 1
			Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.bullets.delayNextShot)
			Yield Nothing
			If bullets(j) IsNot Nothing Then
				bullets(j).GetComponent(Of FlowerLevelSeedBullet)().LaunchBullet()
			End If
		Next
		Me.OnPotHandsComplete()
		Yield Nothing
		Return
	End Function

	' Token: 0x06001EFF RID: 7935 RVA: 0x0011C234 File Offset: 0x0011A634
	Private Sub SpawnCloudShot()
		Dim gameObject As GameObject = Global.UnityEngine.[Object].Instantiate(Of GameObject)(Me.cloudBombPrefab)
		Dim vector As Vector3 = Me.bottomProjectileSpawnPoint.position + (Me.topProjectileSpawnPoint.position - Me.bottomProjectileSpawnPoint.position) / 2F
		gameObject.transform.position = vector
		gameObject.GetComponent(Of FlowerLevelCloudBomb)().OnCloudBombStart(PlayerManager.GetNext().center, CSng(MyBase.properties.CurrentState.puffUp.speed), MyBase.properties.CurrentState.puffUp.delayExplosion)
		Me.OnPotHandsComplete()
	End Sub

	' Token: 0x06001F00 RID: 7936 RVA: 0x0011C2D5 File Offset: 0x0011A6D5
	Private Sub PodHandsFX()
		MyBase.animator.Play("Twinkle", 2)
	End Sub

	' Token: 0x06001F01 RID: 7937 RVA: 0x0011C2E8 File Offset: 0x0011A6E8
	Private Sub GattlingEndAudio()
		AudioManager.Play("flower_gattling_gun_end")
		Me.emitAudioFromObject.Add("flower_gattling_gun_end")
	End Sub

	' Token: 0x06001F02 RID: 7938 RVA: 0x0011C304 File Offset: 0x0011A704
	Private Sub GattlingLoopAudio()
		MyBase.StartCoroutine(Me.gattlingLoopEnd_cr())
	End Sub

	' Token: 0x06001F03 RID: 7939 RVA: 0x0011C314 File Offset: 0x0011A714
	Private Iterator Function gattlingLoopEnd_cr() As IEnumerator
		Yield New WaitForEndOfFrame()
		AudioManager.PlayLoop("flower_gattling_gun_loop")
		Me.emitAudioFromObject.Add("flower_gattling_gun_loop")
		Return
	End Function

	' Token: 0x06001F04 RID: 7940 RVA: 0x0011C32F File Offset: 0x0011A72F
	Private Sub StopGattlingLoopAudio()
		AudioManager.[Stop]("flower_gattling_gun_loop")
	End Sub

	' Token: 0x06001F05 RID: 7941 RVA: 0x0011C33B File Offset: 0x0011A73B
	Private Sub GattlingStartAudio()
		AudioManager.Play("flower_gattling_gun_start")
		Me.emitAudioFromObject.Add("flower_gattling_gun_start")
	End Sub

	' Token: 0x06001F06 RID: 7942 RVA: 0x0011C357 File Offset: 0x0011A757
	Private Sub Phase1IntroAudio()
		AudioManager.Play("flower_intro_yell")
		Me.emitAudioFromObject.Add("flower_intro_yell")
	End Sub

	' Token: 0x06001F07 RID: 7943 RVA: 0x0011C373 File Offset: 0x0011A773
	Private Sub Phase1_2TransitionAudio()
		AudioManager.Play("flower_phase1_2_transition")
	End Sub

	' Token: 0x06001F08 RID: 7944 RVA: 0x0011C37F File Offset: 0x0011A77F
	Private Sub Phase2DeathAudio()
	End Sub

	' Token: 0x06001F09 RID: 7945 RVA: 0x0011C381 File Offset: 0x0011A781
	Private Sub PodHandsStartAudio()
		AudioManager.Play("flower_pod_hands_start")
	End Sub

	' Token: 0x06001F0A RID: 7946 RVA: 0x0011C38D File Offset: 0x0011A78D
	Private Sub PodHandsOpenAudio()
		AudioManager.Play("flower_pod_hands_open")
	End Sub

	' Token: 0x06001F0B RID: 7947 RVA: 0x0011C399 File Offset: 0x0011A799
	Private Sub PodHandsCloseAudio()
		AudioManager.Play("flower_pod_hands_end")
	End Sub

	' Token: 0x06001F0C RID: 7948 RVA: 0x0011C3A5 File Offset: 0x0011A7A5
	Private Sub SpitStartAudio()
		AudioManager.Play("flower_spit_start")
	End Sub

	' Token: 0x06001F0D RID: 7949 RVA: 0x0011C3B1 File Offset: 0x0011A7B1
	Private Sub TopLaserAttackStartAudio()
		AudioManager.Play("flower_top_laser_attack_start")
	End Sub

	' Token: 0x06001F0E RID: 7950 RVA: 0x0011C3BD File Offset: 0x0011A7BD
	Private Sub TopLaserAttackHoldAudio()
		AudioManager.PlayLoop("flower_top_laser_attack_hold")
	End Sub

	' Token: 0x06001F0F RID: 7951 RVA: 0x0011C3C9 File Offset: 0x0011A7C9
	Private Sub TopLaserAttackEndAudio()
		AudioManager.Play("flower_top_laser_attack_end")
		AudioManager.[Stop]("flower_top_laser_attack_hold")
	End Sub

	' Token: 0x0400278F RID: 10127
	Private attackCallback As Action

	' Token: 0x04002792 RID: 10130
	Public attackPoint As GameObject

	' Token: 0x04002793 RID: 10131
	Private topLaserAttack As Boolean

	' Token: 0x04002794 RID: 10132
	Private projectileSpawned As Boolean

	' Token: 0x04002795 RID: 10133
	Private isDead As Boolean

	' Token: 0x04002796 RID: 10134
	Private attackCharge As Single

	' Token: 0x04002797 RID: 10135
	Private attackCount As Integer

	' Token: 0x04002798 RID: 10136
	Private attackCountTarget As Integer

	' Token: 0x04002799 RID: 10137
	Private attackType As Char

	' Token: 0x0400279A RID: 10138
	Private currentLaserAttack As Integer

	' Token: 0x0400279B RID: 10139
	Private currentPodHandsAttack As Integer

	' Token: 0x0400279C RID: 10140
	Private podHandsAttackCountTarget As Integer

	' Token: 0x0400279D RID: 10141
	Private currentGattlingGunAttackString As Integer

	' Token: 0x0400279E RID: 10142
	Private currentGattlingGunAttackPattern As List(Of String)

	' Token: 0x0400279F RID: 10143
	Private currentVineHandsAttack As Integer

	' Token: 0x040027A0 RID: 10144
	Private pollenAttackCount As Integer

	' Token: 0x040027A1 RID: 10145
	Private currentPollenType As Integer

	' Token: 0x040027A2 RID: 10146
	Private currentPollenShot As FlowerLevelPollenProjectile

	' Token: 0x040027A3 RID: 10147
	<Header("Vines")>
	<SerializeField()>
	Private vineHandPrefab As GameObject

	' Token: 0x040027A4 RID: 10148
	<Space(10F)>
	<Header("Prefabs")>
	<SerializeField()>
	Private boomerangPrefab As GameObject

	' Token: 0x040027A5 RID: 10149
	<SerializeField()>
	Private bulletSeedPrefab As GameObject

	' Token: 0x040027A6 RID: 10150
	<SerializeField()>
	Private cloudBombPrefab As GameObject

	' Token: 0x040027A7 RID: 10151
	<SerializeField()>
	Private enemySeedPrefab As GameObject

	' Token: 0x040027A8 RID: 10152
	Private miniFlowerSpawned As Boolean

	' Token: 0x040027A9 RID: 10153
	<SerializeField()>
	Private pollenProjectile As GameObject

	' Token: 0x040027AA RID: 10154
	<Space(10F)>
	<SerializeField()>
	Private topProjectileSpawnPoint As Transform

	' Token: 0x040027AB RID: 10155
	<SerializeField()>
	Private bottomProjectileSpawnPoint As Transform

	' Token: 0x040027AC RID: 10156
	<SerializeField()>
	Private mainVine As GameObject

	' Token: 0x040027AD RID: 10157
	<SerializeField()>
	Private gattlingFX As GameObject

	' Token: 0x040027AE RID: 10158
	Private damageDealer As DamageDealer

	' Token: 0x040027AF RID: 10159
	Private damageReceiver As DamageReceiver

	' Token: 0x040027B0 RID: 10160
	Private bulletSpawns As List(Of AbstractProjectile) = New List(Of AbstractProjectile)()
End Class
