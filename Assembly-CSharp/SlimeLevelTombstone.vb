Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020007E1 RID: 2017
Public Class SlimeLevelTombstone
	Inherits LevelProperties.Slime.Entity

	' Token: 0x17000414 RID: 1044
	' (get) Token: 0x06002E25 RID: 11813 RVA: 0x001B3593 File Offset: 0x001B1993
	' (set) Token: 0x06002E26 RID: 11814 RVA: 0x001B359B File Offset: 0x001B199B
	Public Property state As SlimeLevelTombstone.State

	' Token: 0x06002E27 RID: 11815 RVA: 0x001B35A4 File Offset: 0x001B19A4
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		For Each collider2D As Collider2D In MyBase.GetComponents(Of Collider2D)()
			collider2D.enabled = False
		Next
		MyBase.GetComponent(Of LevelBossDeathExploder)().enabled = False
	End Sub

	' Token: 0x06002E28 RID: 11816 RVA: 0x001B3617 File Offset: 0x001B1A17
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06002E29 RID: 11817 RVA: 0x001B362F File Offset: 0x001B1A2F
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If Me.dealDamage AndAlso phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06002E2A RID: 11818 RVA: 0x001B3658 File Offset: 0x001B1A58
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		MyBase.properties.DealDamage(info.damage)
	End Sub

	' Token: 0x06002E2B RID: 11819 RVA: 0x001B366B File Offset: 0x001B1A6B
	Public Overrides Sub LevelInit(properties As LevelProperties.Slime)
		MyBase.LevelInit(properties)
	End Sub

	' Token: 0x06002E2C RID: 11820 RVA: 0x001B3674 File Offset: 0x001B1A74
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.dustPrefab = Nothing
		Me.smashDustBackPrefab = Nothing
		Me.smashDustFrontPrefab = Nothing
		Me.tinySlime = Nothing
	End Sub

	' Token: 0x06002E2D RID: 11821 RVA: 0x001B3698 File Offset: 0x001B1A98
	Public Sub StartIntro(x As Single)
		Me.state = SlimeLevelTombstone.State.Intro
		MyBase.transform.SetPosition(New Single?(x), Nothing, Nothing)
		Me.offsetIndex = Global.UnityEngine.Random.Range(0, MyBase.properties.CurrentState.tombstone.attackOffsetString.Length)
		For Each collider2D As Collider2D In MyBase.GetComponents(Of Collider2D)()
			collider2D.enabled = True
		Next
		AddHandler MyBase.properties.OnBossDeath, AddressOf Me.OnBossDeath
		MyBase.GetComponent(Of LevelBossDeathExploder)().enabled = True
		MyBase.StartCoroutine(Me.intro_cr())
	End Sub

	' Token: 0x06002E2E RID: 11822 RVA: 0x001B374C File Offset: 0x001B1B4C
	Private Iterator Function intro_cr() As IEnumerator
		MyBase.StartCoroutine(Me.crush_slime_cr())
		AudioManager.Play("slime_tombstone_drop_onto_slime")
		Me.emitAudioFromObject.Add("slime_tombstone_drop_onto_slime")
		Yield MyBase.TweenPositionY(550F, -80F, 0.2F, EaseUtils.EaseType.linear)
		MyBase.animator.SetTrigger("Continue")
		Me.dustPrefab.Create(MyBase.transform.position)
		Me.StartMove()
		Return
	End Function

	' Token: 0x06002E2F RID: 11823 RVA: 0x001B3768 File Offset: 0x001B1B68
	Private Iterator Function crush_slime_cr() As IEnumerator
		While MyBase.transform.position.y > 70F
			Yield Nothing
		End While
		Me.bigSlime.Explode()
		If SlimeLevelSlime.TINIES Then
			Dim slimeLevelTinySlime As SlimeLevelTinySlime = Global.UnityEngine.[Object].Instantiate(Of SlimeLevelTinySlime)(Me.tinySlime)
			Dim slimeLevelTinySlime2 As SlimeLevelTinySlime = Global.UnityEngine.[Object].Instantiate(Of SlimeLevelTinySlime)(Me.tinySlime)
			slimeLevelTinySlime.Init(Me.dust.transform.position, MyBase.properties.CurrentState.tombstone, True, Me)
			slimeLevelTinySlime2.Init(Me.dust.transform.position, MyBase.properties.CurrentState.tombstone, False, Me)
		End If
		CupheadLevelCamera.Current.Shake(20F, 0.7F, False)
		Return
	End Function

	' Token: 0x06002E30 RID: 11824 RVA: 0x001B3783 File Offset: 0x001B1B83
	Private Sub StartMove()
		Me.state = SlimeLevelTombstone.State.Move
		Me.wantsToSmash = False
		MyBase.StartCoroutine(Me.move_cr())
		MyBase.StartCoroutine(Me.waitForSmash_cr())
	End Sub

	' Token: 0x06002E31 RID: 11825 RVA: 0x001B37B0 File Offset: 0x001B1BB0
	Private Iterator Function move_cr() As IEnumerator
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		Me.direction = If((Not MathUtils.RandomBool()), SlimeLevelTombstone.Direction.Right, SlimeLevelTombstone.Direction.Left)
		Dim offsets As String() = MyBase.properties.CurrentState.tombstone.attackOffsetString.Split(New Char() { ","c })
		Me.offsetIndex = (Me.offsetIndex + 1) Mod offsets.Length
		Dim offset As Single = 0F
		Parser.FloatTryParse(offsets(Me.offsetIndex), offset)
		Dim justStarted As Boolean = True
		While Not Me.wantsToSmash
			MyBase.animator.SetTrigger(If((Me.direction <> SlimeLevelTombstone.Direction.Right), "MoveLeft", "MoveRight"))
			Yield MyBase.animator.WaitForAnimationToStart(Me, If((Me.direction <> SlimeLevelTombstone.Direction.Right), "Move_Left", "Move_Right"), False)
			MyBase.animator.Play("Dirt")
			If justStarted Then
				MyBase.animator.Play("Dust_Start")
			Else
				MyBase.animator.Play("Dust_Start_End")
			End If
			AudioManager.Play("slime_tombstone_slide")
			Me.emitAudioFromObject.Add("slime_tombstone_slide")
			Dim startX As Single = MyBase.transform.position.x
			Dim endX As Single = If((Me.direction <> SlimeLevelTombstone.Direction.Right), (-500F), 500F)
			Dim moveTime As Single = Mathf.Abs(startX - endX) / MyBase.properties.CurrentState.tombstone.moveSpeed
			Yield MyBase.TweenPositionX(startX, endX, moveTime, EaseUtils.EaseType.easeInOutSine)
			Me.direction = If((Me.direction <> SlimeLevelTombstone.Direction.Right), SlimeLevelTombstone.Direction.Right, SlimeLevelTombstone.Direction.Left)
			justStarted = False
		End While
		MyBase.animator.SetTrigger(If((Me.direction <> SlimeLevelTombstone.Direction.Right), "MoveLeft", "MoveRight"))
		Yield MyBase.animator.WaitForAnimationToStart(Me, If((Me.direction <> SlimeLevelTombstone.Direction.Right), "Move_Left", "Move_Right"), False)
		MyBase.animator.Play("Dust_Start_End")
		AudioManager.Play("slime_tombstone_slide")
		Me.emitAudioFromObject.Add("slime_tombstone_slide")
		Dim player As AbstractPlayerController = PlayerManager.GetNext()
		Dim startX2 As Single = MyBase.transform.position.x
		Dim endX2 As Single = If((Me.direction <> SlimeLevelTombstone.Direction.Right), (-500F), 500F)
		Dim moveTime2 As Single = Mathf.Abs(startX2 - endX2) / MyBase.properties.CurrentState.tombstone.moveSpeed
		Dim targetX As Single = 0F
		Dim t As Single = 0F
		Dim centeredOnPlayer As Boolean = False
		While Not centeredOnPlayer AndAlso t < moveTime2
			Yield wait
			t += CupheadTime.FixedDelta * Me.hitPauseCoefficient()
			MyBase.transform.SetPosition(New Single?(EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, startX2, endX2, t / moveTime2)), Nothing, Nothing)
			If player Is Nothing OrElse player.IsDead Then
				player = PlayerManager.GetNext()
			End If
			targetX = player.center.x + offset
			If(Me.direction = SlimeLevelTombstone.Direction.Right AndAlso MyBase.transform.position.x > targetX) OrElse (Me.direction = SlimeLevelTombstone.Direction.Left AndAlso MyBase.transform.position.x < targetX) Then
				centeredOnPlayer = True
			End If
		End While
		MyBase.transform.SetPosition(New Single?(Mathf.Clamp(targetX, -500F, 500F)), Nothing, Nothing)
		MyBase.animator.Play("Dust_End")
		MyBase.animator.Play("Dirt_Off")
		Me.StartSmash()
		Return
	End Function

	' Token: 0x06002E32 RID: 11826 RVA: 0x001B37CC File Offset: 0x001B1BCC
	Private Sub DustDirection()
		Me.dirt.SetScale(New Single?(CSng(If((Me.direction <> SlimeLevelTombstone.Direction.Right), (-1), 1))), Nothing, Nothing)
		Me.dust.SetScale(New Single?(CSng(If((Me.direction <> SlimeLevelTombstone.Direction.Right), 1, (-1)))), Nothing, Nothing)
		Me.dust2.SetScale(New Single?(CSng(If((Me.direction <> SlimeLevelTombstone.Direction.Right), (-1), 1))), Nothing, Nothing)
	End Sub

	' Token: 0x06002E33 RID: 11827 RVA: 0x001B387B File Offset: 0x001B1C7B
	Private Function hitPauseCoefficient() As Single
		Return If((Not MyBase.GetComponent(Of DamageReceiver)().IsHitPaused), 1F, 0F)
	End Function

	' Token: 0x06002E34 RID: 11828 RVA: 0x001B389C File Offset: 0x001B1C9C
	Private Iterator Function waitForSmash_cr() As IEnumerator
		Dim timeUntilAttack As Single = MyBase.properties.CurrentState.tombstone.attackDelay.RandomFloat()
		Yield CupheadTime.WaitForSeconds(Me, timeUntilAttack)
		Me.wantsToSmash = True
		Return
	End Function

	' Token: 0x06002E35 RID: 11829 RVA: 0x001B38B7 File Offset: 0x001B1CB7
	Private Sub StartSmash()
		Me.state = SlimeLevelTombstone.State.Smash
		MyBase.StartCoroutine(Me.smash_cr())
	End Sub

	' Token: 0x06002E36 RID: 11830 RVA: 0x001B38D0 File Offset: 0x001B1CD0
	Private Iterator Function smash_cr() As IEnumerator
		MyBase.animator.SetTrigger("StartSmash")
		Yield MyBase.animator.WaitForAnimationToStart(Me, "Smash_Pre_Hold", False)
		AudioManager.Play("slime_tombstone_splat")
		Me.emitAudioFromObject.Add("slime_tombstone_splat")
		AudioManager.Play("slime_tombstone_splat_start")
		Me.emitAudioFromObject.Add("slime_tombstone_splat_start")
		AudioManager.[Stop]("slime_tombstone_slide")
		Me.emitAudioFromObject.Add("slime_tombstone_slide")
		Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.tombstone.anticipationHold)
		MyBase.animator.SetTrigger("Continue")
		Me.StartMove()
		Return
	End Function

	' Token: 0x06002E37 RID: 11831 RVA: 0x001B38EB File Offset: 0x001B1CEB
	Private Sub DisableDamageReceiver()
		Me.damageReceiver.enabled = False
	End Sub

	' Token: 0x06002E38 RID: 11832 RVA: 0x001B38F9 File Offset: 0x001B1CF9
	Private Sub EnableDamageReceiver()
		Me.damageReceiver.enabled = True
	End Sub

	' Token: 0x06002E39 RID: 11833 RVA: 0x001B3907 File Offset: 0x001B1D07
	Private Sub EnableDamageDealer()
		Me.dealDamage = True
	End Sub

	' Token: 0x06002E3A RID: 11834 RVA: 0x001B3910 File Offset: 0x001B1D10
	Private Sub DisableDamageDealer()
		Me.dealDamage = False
	End Sub

	' Token: 0x06002E3B RID: 11835 RVA: 0x001B391C File Offset: 0x001B1D1C
	Private Sub OnSmash()
		CupheadLevelCamera.Current.Shake(30F, 0.7F, False)
		Me.smashDustFrontPrefab.Create(MyBase.transform.position)
		Me.smashDustBackPrefab.Create(MyBase.transform.position)
	End Sub

	' Token: 0x06002E3C RID: 11836 RVA: 0x001B396C File Offset: 0x001B1D6C
	Private Sub OnBossDeath()
		If Me.onDeath IsNot Nothing Then
			Me.onDeath()
		End If
		Me.StopAllCoroutines()
		MyBase.animator.SetTrigger("Death")
		AudioManager.Play("slime_tombstone_death")
	End Sub

	' Token: 0x06002E3D RID: 11837 RVA: 0x001B39A4 File Offset: 0x001B1DA4
	Private Sub TombstoneTauntsAudio()
		AudioManager.Play("slime_tombstone_taunts")
	End Sub

	' Token: 0x0400369D RID: 13981
	Private Const startY As Single = 550F

	' Token: 0x0400369E RID: 13982
	Private Const onGroundY As Single = -80F

	' Token: 0x0400369F RID: 13983
	Private Const maxX As Single = 500F

	' Token: 0x040036A0 RID: 13984
	Private Const fallTime As Single = 0.2F

	' Token: 0x040036A1 RID: 13985
	Private Const crushSlimeY As Single = 70F

	' Token: 0x040036A2 RID: 13986
	Private offsetIndex As Integer

	' Token: 0x040036A3 RID: 13987
	Private dealDamage As Boolean

	' Token: 0x040036A4 RID: 13988
	Private damageDealer As DamageDealer

	' Token: 0x040036A5 RID: 13989
	Private damageReceiver As DamageReceiver

	' Token: 0x040036A6 RID: 13990
	<SerializeField()>
	Private dirt As Transform

	' Token: 0x040036A7 RID: 13991
	<SerializeField()>
	Private dust As Transform

	' Token: 0x040036A8 RID: 13992
	<SerializeField()>
	Private dust2 As Transform

	' Token: 0x040036A9 RID: 13993
	<SerializeField()>
	Private dustPrefab As Effect

	' Token: 0x040036AA RID: 13994
	<SerializeField()>
	Private bigSlime As SlimeLevelSlime

	' Token: 0x040036AB RID: 13995
	<SerializeField()>
	Private tinySlime As SlimeLevelTinySlime

	' Token: 0x040036AC RID: 13996
	<SerializeField()>
	Private smashDustBackPrefab As Effect

	' Token: 0x040036AD RID: 13997
	<SerializeField()>
	Private smashDustFrontPrefab As Effect

	' Token: 0x040036AE RID: 13998
	Private direction As SlimeLevelTombstone.Direction

	' Token: 0x040036AF RID: 13999
	Public onDeath As Action

	' Token: 0x040036B0 RID: 14000
	Private wantsToSmash As Boolean

	' Token: 0x020007E2 RID: 2018
	Public Enum State
		' Token: 0x040036B2 RID: 14002
		Init
		' Token: 0x040036B3 RID: 14003
		Intro
		' Token: 0x040036B4 RID: 14004
		Move
		' Token: 0x040036B5 RID: 14005
		Smash
	End Enum

	' Token: 0x020007E3 RID: 2019
	Private Enum Direction
		' Token: 0x040036B7 RID: 14007
		Left
		' Token: 0x040036B8 RID: 14008
		Right
	End Enum
End Class
