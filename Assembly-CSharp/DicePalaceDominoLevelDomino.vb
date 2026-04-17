Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020005B5 RID: 1461
Public Class DicePalaceDominoLevelDomino
	Inherits LevelProperties.DicePalaceDomino.Entity

	' Token: 0x17000360 RID: 864
	' (get) Token: 0x06001C54 RID: 7252 RVA: 0x0010387E File Offset: 0x00101C7E
	' (set) Token: 0x06001C55 RID: 7253 RVA: 0x00103886 File Offset: 0x00101C86
	Public Property state As DicePalaceDominoLevelDomino.State

	' Token: 0x06001C56 RID: 7254 RVA: 0x0010388F File Offset: 0x00101C8F
	Protected Overrides Sub Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		MyBase.Awake()
	End Sub

	' Token: 0x06001C57 RID: 7255 RVA: 0x001038C5 File Offset: 0x00101CC5
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06001C58 RID: 7256 RVA: 0x001038DD File Offset: 0x00101CDD
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		MyBase.properties.DealDamage(info.damage)
	End Sub

	' Token: 0x06001C59 RID: 7257 RVA: 0x001038F0 File Offset: 0x00101CF0
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x06001C5A RID: 7258 RVA: 0x00103910 File Offset: 0x00101D10
	Public Overrides Sub LevelInit(properties As LevelProperties.DicePalaceDomino)
		AddHandler Level.Current.OnIntroEvent, AddressOf Me.OnIntroEnd
		AddHandler Level.Current.OnWinEvent, AddressOf Me.OnDeath
		MyBase.transform.parent.GetComponent(Of DicePalaceDominoLevelDominoSwing)().InitSwing(properties)
		Me.happyAttackAngleIndex = Global.UnityEngine.Random.Range(0, properties.CurrentState.bouncyBall.angleString.Split(New Char() { ","c }).Length)
		Me.happyAttackDirectionIndex = Global.UnityEngine.Random.Range(0, properties.CurrentState.bouncyBall.upDownString.Split(New Char() { ","c }).Length)
		Me.happyAttackBallTypePattern = properties.CurrentState.bouncyBall.projectileTypeString.Split(New Char() { ","c })
		Me.happyAttackBallTypeIndex = Global.UnityEngine.Random.Range(0, Me.happyAttackBallTypePattern.Length)
		Me.happyAttackDelay = properties.CurrentState.bouncyBall.attackDelayRange.RandomFloat()
		Me.sadAttackBoomerangTypeIndex = Global.UnityEngine.Random.Range(0, properties.CurrentState.boomerang.boomerangTypeString.Split(New Char() { ","c }).Length)
		Me.sadAttackDelay = properties.CurrentState.boomerang.attackDelayRange.RandomFloat()
		Me.floor.InitFloor(properties)
		MyBase.LevelInit(properties)
		MyBase.StartCoroutine(Me.intro_cr())
	End Sub

	' Token: 0x06001C5B RID: 7259 RVA: 0x00103A79 File Offset: 0x00101E79
	Private Sub OnIntroEnd()
		MyBase.animator.enabled = True
		Me.floor.StartSpawningTiles()
	End Sub

	' Token: 0x06001C5C RID: 7260 RVA: 0x00103A94 File Offset: 0x00101E94
	Private Iterator Function intro_cr() As IEnumerator
		AudioManager.PlayLoop("dice_palace_domino_intro_start_loop")
		Me.emitAudioFromObject.Add("dice_palace_domino_intro_start_loop")
		Yield CupheadTime.WaitForSeconds(Me, 2F)
		MyBase.animator.SetTrigger("Continue")
		Yield MyBase.animator.WaitForAnimationToStart(Me, "Intro", False)
		AudioManager.[Stop]("dice_palace_domino_intro_start_loop")
		AudioManager.Play("dice_palace_domino_intro")
		Me.emitAudioFromObject.Add("dice_palace_domino_intro")
		Me.state = DicePalaceDominoLevelDomino.State.Idle
		Return
	End Function

	' Token: 0x06001C5D RID: 7261 RVA: 0x00103AAF File Offset: 0x00101EAF
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.bouncyBallPrefab = Nothing
		Me.boomerangPrefab = Nothing
	End Sub

	' Token: 0x06001C5E RID: 7262 RVA: 0x00103AC5 File Offset: 0x00101EC5
	Public Sub OnBouncyBall()
		MyBase.StartCoroutine(Me.bouncyBall_cr())
	End Sub

	' Token: 0x06001C5F RID: 7263 RVA: 0x00103AD4 File Offset: 0x00101ED4
	Private Iterator Function bouncyBall_cr() As IEnumerator
		Me.state = DicePalaceDominoLevelDomino.State.BouncyBall
		Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.bouncyBall.initialAttackDelay)
		MyBase.animator.SetTrigger("OnProjectile")
		Yield MyBase.animator.WaitForAnimationToStart(Me, "Projectile_Attack", False)
		AudioManager.Play("dice_palace_domino_projectile_attack")
		Me.emitAudioFromObject.Add("dice_palace_domino_projectile_attack")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Projectile_Attack", False, True)
		Yield CupheadTime.WaitForSeconds(Me, Me.happyAttackDelay)
		Me.state = DicePalaceDominoLevelDomino.State.Idle
		Return
	End Function

	' Token: 0x06001C60 RID: 7264 RVA: 0x00103AEF File Offset: 0x00101EEF
	Public Sub SpawnBall()
		MyBase.StartCoroutine(Me.spawn_ball_cr())
	End Sub

	' Token: 0x06001C61 RID: 7265 RVA: 0x00103B00 File Offset: 0x00101F00
	Private Iterator Function spawn_ball_cr() As IEnumerator
		Dim angle As Single = CSng(Parser.IntParse(MyBase.properties.CurrentState.bouncyBall.angleString.Split(New Char() { ","c })(Me.happyAttackAngleIndex)))
		If MyBase.properties.CurrentState.bouncyBall.upDownString.Split(New Char() { ","c })(Me.happyAttackDirectionIndex)(0) = "U"c Then
			angle = -angle
		End If
		Dim direction As Vector3 = Vector3.left
		direction = Quaternion.AngleAxis(angle, Vector3.forward) * direction
		Dim proj As AbstractProjectile = Me.bouncyBallPrefab.Create(Me.bouncySpawnpoint.position)
		proj.SetParryable(Me.happyAttackBallTypePattern(Me.happyAttackBallTypeIndex)(0) = "P"c)
		proj.GetComponent(Of DicePalaceDominoLevelBouncyBall)().InitBouncyBall(MyBase.properties.CurrentState.bouncyBall.bulletSpeed, direction)
		Me.happyAttackAngleIndex += 1
		If Me.happyAttackAngleIndex >= MyBase.properties.CurrentState.bouncyBall.angleString.Split(New Char() { ","c }).Length Then
			Me.happyAttackAngleIndex = 0
		End If
		Me.happyAttackDirectionIndex += 1
		If Me.happyAttackDirectionIndex >= MyBase.properties.CurrentState.bouncyBall.upDownString.Split(New Char() { ","c }).Length Then
			Me.happyAttackDirectionIndex = 0
		End If
		Me.happyAttackBallTypeIndex += 1
		If Me.happyAttackBallTypeIndex >= Me.happyAttackBallTypePattern.Length Then
			Me.happyAttackBallTypeIndex = 0
		End If
		Yield Nothing
		Return
	End Function

	' Token: 0x06001C62 RID: 7266 RVA: 0x00103B1B File Offset: 0x00101F1B
	Public Sub OnBoomerang()
		MyBase.StartCoroutine(Me.boomerang_cr())
	End Sub

	' Token: 0x06001C63 RID: 7267 RVA: 0x00103B2C File Offset: 0x00101F2C
	Private Iterator Function boomerang_cr() As IEnumerator
		Me.state = DicePalaceDominoLevelDomino.State.Boomerang
		Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.boomerang.initialAttackDelay)
		MyBase.animator.SetTrigger("OnBird")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Bird_Attack", False, True)
		Yield CupheadTime.WaitForSeconds(Me, Me.sadAttackDelay)
		Me.state = DicePalaceDominoLevelDomino.State.Idle
		Return
	End Function

	' Token: 0x06001C64 RID: 7268 RVA: 0x00103B47 File Offset: 0x00101F47
	Public Sub SpawnBoomerang()
		MyBase.StartCoroutine(Me.spawn_boomerang_cr())
	End Sub

	' Token: 0x06001C65 RID: 7269 RVA: 0x00103B58 File Offset: 0x00101F58
	Private Iterator Function spawn_boomerang_cr() As IEnumerator
		Dim p As LevelProperties.DicePalaceDomino.Boomerang = MyBase.properties.CurrentState.boomerang
		If MyBase.properties.CurrentState.boomerang.boomerangTypeString.Split(New Char() { ","c })(Me.sadAttackBoomerangTypeIndex)(0) = "R"c Then
			Dim proj As DicePalaceDominoLevelBoomerang = Me.boomerangPrefab.Create(Me.birdSpawnpoint.position, p.boomerangSpeed, p.health)
		Else
			Dim proj As DicePalaceDominoLevelBoomerang = Me.boomerangPrefab.Create(Me.birdSpawnpoint.position, p.boomerangSpeed, p.health)
			proj.GetComponent(Of SpriteRenderer)().color = Color.magenta
			proj.SetParryable(True)
		End If
		Me.sadAttackBoomerangTypeIndex += 1
		If Me.sadAttackBoomerangTypeIndex >= MyBase.properties.CurrentState.boomerang.boomerangTypeString.Split(New Char() { ","c }).Length Then
			Me.sadAttackBoomerangTypeIndex = 0
		End If
		Yield Nothing
		Return
	End Function

	' Token: 0x06001C66 RID: 7270 RVA: 0x00103B73 File Offset: 0x00101F73
	Private Sub OnDeath()
		AudioManager.PlayLoop("dice_palace_domino_death_start_loop")
		Me.emitAudioFromObject.Add("dice_palace_domino_death_start_loop")
		MyBase.animator.SetTrigger("OnDeath")
	End Sub

	' Token: 0x06001C67 RID: 7271 RVA: 0x00103B9F File Offset: 0x00101F9F
	Private Sub EndDeathLoop()
		AudioManager.[Stop]("dice_palace_domino_death_start_loop")
	End Sub

	' Token: 0x06001C68 RID: 7272 RVA: 0x00103BAB File Offset: 0x00101FAB
	Private Sub DeathSFX()
		AudioManager.Play("dice_palace_domino_death")
		Me.emitAudioFromObject.Add("dice_palace_domino_death")
	End Sub

	' Token: 0x06001C69 RID: 7273 RVA: 0x00103BC7 File Offset: 0x00101FC7
	Private Sub BirdAttackSFX()
		AudioManager.Play("dice_palace_domino_bird_attack")
		Me.emitAudioFromObject.Add("dice_palace_domino_bird_attack")
	End Sub

	' Token: 0x06001C6A RID: 7274 RVA: 0x00103BE3 File Offset: 0x00101FE3
	Private Sub SwingForwardSFX()
		AudioManager.Play("swing_forward")
		Me.emitAudioFromObject.Add("swing_forward")
	End Sub

	' Token: 0x06001C6B RID: 7275 RVA: 0x00103BFF File Offset: 0x00101FFF
	Private Sub SwingBackSFX()
		AudioManager.Play("swing_back")
		Me.emitAudioFromObject.Add("swing_back")
	End Sub

	' Token: 0x04002555 RID: 9557
	<SerializeField()>
	Private bouncySpawnpoint As Transform

	' Token: 0x04002556 RID: 9558
	<SerializeField()>
	Private birdSpawnpoint As Transform

	' Token: 0x04002557 RID: 9559
	<SerializeField()>
	Private bouncyBallPrefab As DicePalaceDominoLevelBouncyBall

	' Token: 0x04002558 RID: 9560
	<SerializeField()>
	Private boomerangPrefab As DicePalaceDominoLevelBoomerang

	' Token: 0x04002559 RID: 9561
	<SerializeField()>
	Private floor As DicePalaceDominoLevelFloor

	' Token: 0x0400255A RID: 9562
	Private happyAttackAngleIndex As Integer

	' Token: 0x0400255B RID: 9563
	Private happyAttackDirectionIndex As Integer

	' Token: 0x0400255C RID: 9564
	Private happyAttackBallTypePattern As String()

	' Token: 0x0400255D RID: 9565
	Private happyAttackBallTypeIndex As Integer

	' Token: 0x0400255E RID: 9566
	Private happyAttackDelay As Single

	' Token: 0x0400255F RID: 9567
	Private sadAttackBoomerangTypeIndex As Integer

	' Token: 0x04002560 RID: 9568
	Private sadAttackDelay As Single

	' Token: 0x04002561 RID: 9569
	Private damageDealer As DamageDealer

	' Token: 0x04002562 RID: 9570
	Private damageReceiver As DamageReceiver

	' Token: 0x020005B6 RID: 1462
	Public Enum State
		' Token: 0x04002565 RID: 9573
		Idle
		' Token: 0x04002566 RID: 9574
		Boomerang
		' Token: 0x04002567 RID: 9575
		BouncyBall
	End Enum
End Class
