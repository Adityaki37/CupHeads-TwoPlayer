Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000690 RID: 1680
Public Class FlyingMermaidLevelMerdusaHead
	Inherits LevelProperties.FlyingMermaid.Entity

	' Token: 0x170003A2 RID: 930
	' (get) Token: 0x06002378 RID: 9080 RVA: 0x0014CC71 File Offset: 0x0014B071
	' (set) Token: 0x06002379 RID: 9081 RVA: 0x0014CC79 File Offset: 0x0014B079
	Public Property state As FlyingMermaidLevelMerdusaHead.State

	' Token: 0x0600237A RID: 9082 RVA: 0x0014CC82 File Offset: 0x0014B082
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
	End Sub

	' Token: 0x0600237B RID: 9083 RVA: 0x0014CCB8 File Offset: 0x0014B0B8
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		MyBase.properties.DealDamage(info.damage)
	End Sub

	' Token: 0x0600237C RID: 9084 RVA: 0x0014CCCB File Offset: 0x0014B0CB
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x0600237D RID: 9085 RVA: 0x0014CCE3 File Offset: 0x0014B0E3
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x0600237E RID: 9086 RVA: 0x0014CD01 File Offset: 0x0014B101
	Public Sub StartIntro(pos As Vector2)
		MyBase.transform.position = pos
		AddHandler MyBase.properties.OnBossDeath, AddressOf Me.OnBossDeath
		MyBase.StartCoroutine(Me.intro_cr())
	End Sub

	' Token: 0x0600237F RID: 9087 RVA: 0x0014CD38 File Offset: 0x0014B138
	Public Sub CheckParts(parts As FlyingMermaidLevelMerdusaBodyPart())
		MyBase.StartCoroutine(Me.check_parts_cr(parts))
	End Sub

	' Token: 0x06002380 RID: 9088 RVA: 0x0014CD48 File Offset: 0x0014B148
	Private Iterator Function check_parts_cr(parts As FlyingMermaidLevelMerdusaBodyPart()) As IEnumerator
		For Each part As FlyingMermaidLevelMerdusaBodyPart In parts
			While Not part.IsSinking
				Yield Nothing
			End While
		Next
		Me.coral.speed = MyBase.properties.CurrentState.coral.coralMoveSpeed
		For Each spriteRenderer As SpriteRenderer In Me.wave1.GetComponentsInChildren(Of SpriteRenderer)()
			spriteRenderer.sortingLayerName = SpriteLayer.Background.ToString()
			spriteRenderer.sortingOrder = 100
		Next
		For Each spriteRenderer2 As SpriteRenderer In Me.wave2.GetComponentsInChildren(Of SpriteRenderer)()
			spriteRenderer2.sortingLayerName = SpriteLayer.Background.ToString()
			spriteRenderer2.sortingOrder = 101
		Next
		For Each scrollingSpriteSpawner As ScrollingSpriteSpawner In Me.scrollingSpritesToEnd
			scrollingSpriteSpawner.HandlePausing(True)
		Next
		For Each scrollingSpriteSpawner2 As ScrollingSpriteSpawner In Me.scrollingSprites
			scrollingSpriteSpawner2.StartLoop(False)
		Next
		MyBase.StartCoroutine(Me.move_head_cr())
		Me.state = FlyingMermaidLevelMerdusaHead.State.Idle
		MyBase.StartCoroutine(Me.spawn_yellow_dots_cr())
		Return
	End Function

	' Token: 0x06002381 RID: 9089 RVA: 0x0014CD6A File Offset: 0x0014B16A
	Public Overrides Sub LevelInit(properties As LevelProperties.FlyingMermaid)
		MyBase.LevelInit(properties)
	End Sub

	' Token: 0x06002382 RID: 9090 RVA: 0x0014CD73 File Offset: 0x0014B173
	Private Sub OnBossDeath()
		Me.StopAllCoroutines()
		MyBase.animator.Play("Death")
		Me.state = FlyingMermaidLevelMerdusaHead.State.Dead
	End Sub

	' Token: 0x06002383 RID: 9091 RVA: 0x0014CD94 File Offset: 0x0014B194
	Private Iterator Function intro_cr() As IEnumerator
		Me.state = FlyingMermaidLevelMerdusaHead.State.Intro
		Level.Current.SetBounds(Nothing, Nothing, Nothing, New Integer?(300))
		Yield Nothing
		Return
	End Function

	' Token: 0x06002384 RID: 9092 RVA: 0x0014CDB0 File Offset: 0x0014B1B0
	Private Iterator Function move_head_cr() As IEnumerator
		Dim pos As Vector2 = MyBase.transform.position
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		Dim offset As Single = Me.xPosition - pos.x
		While True
			Dim targetXDistance As Single = Single.MaxValue
			Dim targetY As Single = 0F
			For Each transform As Transform In Me.coral.points
				Dim num As Single = transform.position.x - pos.x
				If num > 0F AndAlso num < targetXDistance Then
					targetXDistance = num
					targetY = transform.position.y
				End If
			Next
			Dim t As Single = 0F
			Dim time As Single = targetXDistance / Me.coral.speed
			Dim startY As Single = pos.y
			While t < time
				If MyBase.transform.position.x < Me.xPosition Then
					pos.x += offset * (CupheadTime.FixedDelta / Me.headBackMoveTime)
				Else
					pos.x = Me.xPosition
				End If
				t += CupheadTime.FixedDelta
				pos.y = EaseUtils.EaseInOutSine(startY, targetY, t / time)
				MyBase.transform.position = pos
				Yield wait
			End While
			Yield wait
		End While
		Return
	End Function

	' Token: 0x06002385 RID: 9093 RVA: 0x0014CDCB File Offset: 0x0014B1CB
	Public Sub StartBubble()
		If Me.patternCoroutine IsNot Nothing Then
			MyBase.StopCoroutine(Me.patternCoroutine)
		End If
		Me.patternCoroutine = MyBase.StartCoroutine(Me.bubble_cr())
	End Sub

	' Token: 0x06002386 RID: 9094 RVA: 0x0014CDF6 File Offset: 0x0014B1F6
	Public Sub StartHeadBlast()
		If Me.patternCoroutine IsNot Nothing Then
			MyBase.StopCoroutine(Me.patternCoroutine)
		End If
		Me.patternCoroutine = MyBase.StartCoroutine(Me.head_blast_cr())
	End Sub

	' Token: 0x06002387 RID: 9095 RVA: 0x0014CE21 File Offset: 0x0014B221
	Public Sub StartHeadBubble()
		If Me.patternCoroutine IsNot Nothing Then
			MyBase.StopCoroutine(Me.patternCoroutine)
		End If
		Me.patternCoroutine = MyBase.StartCoroutine(Me.head_blast_bubble_cr())
	End Sub

	' Token: 0x06002388 RID: 9096 RVA: 0x0014CE4C File Offset: 0x0014B24C
	Private Iterator Function bubble_cr() As IEnumerator
		Me.state = FlyingMermaidLevelMerdusaHead.State.Bubble
		MyBase.animator.SetTrigger("OnSnakeATK")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Snake_Attack", False, True)
		Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.bubbles.attackDelayRange.RandomFloat())
		Me.state = FlyingMermaidLevelMerdusaHead.State.Idle
		Yield Nothing
		Return
	End Function

	' Token: 0x06002389 RID: 9097 RVA: 0x0014CE68 File Offset: 0x0014B268
	Private Iterator Function head_blast_cr() As IEnumerator
		Me.state = FlyingMermaidLevelMerdusaHead.State.HeadBlast
		MyBase.animator.SetTrigger("OnEyewave")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Eyewave_Attack", False, True)
		Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.headBlast.attackDelayRange.RandomFloat())
		Me.state = FlyingMermaidLevelMerdusaHead.State.Idle
		Yield Nothing
		Return
	End Function

	' Token: 0x0600238A RID: 9098 RVA: 0x0014CE84 File Offset: 0x0014B284
	Private Iterator Function head_blast_bubble_cr() As IEnumerator
		Me.state = FlyingMermaidLevelMerdusaHead.State.Both
		MyBase.animator.SetTrigger("OnBoth")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Snake_Eyewave", False, True)
		Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.coral.bubbleEyewaveSpawnDelayRange.RandomFloat())
		Me.state = FlyingMermaidLevelMerdusaHead.State.Idle
		Yield Nothing
		Return
	End Function

	' Token: 0x0600238B RID: 9099 RVA: 0x0014CEA0 File Offset: 0x0014B2A0
	Private Sub SpawnBubble()
		Dim [next] As AbstractPlayerController = PlayerManager.GetNext()
		Dim vector As Vector3 = [next].transform.position - MyBase.transform.position
		Dim bubbles As LevelProperties.FlyingMermaid.Bubbles = MyBase.properties.CurrentState.bubbles
		Me.bubblePrefab.CreateBubble(Me.snakeRoot.transform.position, bubbles.movementSpeed, bubbles.waveSpeed, bubbles.waveAmount, MathUtils.DirectionToAngle(vector))
	End Sub

	' Token: 0x0600238C RID: 9100 RVA: 0x0014CF20 File Offset: 0x0014B320
	Private Sub SpawnHeadBlast()
		Dim basicProjectile As BasicProjectile = Me.heatBlastPrefab.Create(Me.eyebeamRoot.transform.position, 0F, -MyBase.properties.CurrentState.headBlast.movementSpeed)
		basicProjectile.GetComponent(Of FlyingMermaidLevelLaser)().SetStoneTime(MyBase.properties.CurrentState.zap.stoneTime)
	End Sub

	' Token: 0x0600238D RID: 9101 RVA: 0x0014CF8C File Offset: 0x0014B38C
	Private Iterator Function spawn_yellow_dots_cr() As IEnumerator
		Dim xPos As Single = 690F
		Dim p As LevelProperties.FlyingMermaid.Coral = MyBase.properties.CurrentState.coral
		Dim mainIndex As Integer = Global.UnityEngine.Random.Range(0, p.yellowDotPosString.Length)
		Dim yPosString As String() = p.yellowDotPosString(mainIndex).Split(New Char() { ","c })
		While True
			yPosString = p.yellowDotPosString(mainIndex).Split(New Char() { ","c })
			Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.coral.yellowSpawnDelayRange.RandomFloat())
			Dim yPos As Single() = New Single(yPosString.Length - 1) {}
			For i As Integer = 0 To yPosString.Length - 1
				yPos(i) = Parser.FloatParse(yPosString(i))
			Next
			Array.Sort(Of Single)(yPos)
			For j As Integer = 0 To yPosString.Length - 1
				Dim vector As Vector3 = New Vector3(xPos, MyBase.transform.position.y - 20F + yPos(j))
				Dim basicProjectile As BasicProjectile = Me.yellowDot.Create(vector, 0F, -p.coralMoveSpeed)
				If yPosString.Length = 1 Then
					basicProjectile.animator.SetFloat("PillarType", 1F)
				ElseIf j = 0 Then
					basicProjectile.animator.SetFloat("PillarType", 0.5F)
				ElseIf j = yPosString.Length - 1 Then
					basicProjectile.animator.SetFloat("PillarType", If((Not Rand.Bool()), 0.25F, 0F))
				Else
					basicProjectile.animator.SetFloat("PillarType", 0.75F)
				End If
				basicProjectile.animator.Play("Pillar", 0, Global.UnityEngine.Random.value)
				basicProjectile.GetComponent(Of SpriteRenderer)().sortingOrder = j
			Next
			mainIndex = (mainIndex + 1) Mod p.yellowDotPosString.Length
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600238E RID: 9102 RVA: 0x0014CFA7 File Offset: 0x0014B3A7
	Private Sub SoundMermaidPhase3GhostShoot()
		AudioManager.Play("level_mermaid_phase3_ghostshoot")
		Me.emitAudioFromObject.Add("level_mermaid_phase3_ghostshoot")
	End Sub

	' Token: 0x0600238F RID: 9103 RVA: 0x0014CFC3 File Offset: 0x0014B3C3
	Private Sub SoundMermaidPhase3SnakeShoot()
		AudioManager.Play("level_mermaid_phase3_snakeshoot")
		Me.emitAudioFromObject.Add("level_mermaid_phase3_snakeshoot")
	End Sub

	' Token: 0x04002C19 RID: 11289
	Private Const PillarTopA As Single = 0F

	' Token: 0x04002C1A RID: 11290
	Private Const PillarTopB As Single = 0.25F

	' Token: 0x04002C1B RID: 11291
	Private Const PillarBottom As Single = 0.5F

	' Token: 0x04002C1C RID: 11292
	Private Const PillarPlain As Single = 0.75F

	' Token: 0x04002C1D RID: 11293
	Private Const PillarSingle As Single = 1F

	' Token: 0x04002C1E RID: 11294
	Private Const PillarParameterName As String = "PillarType"

	' Token: 0x04002C1F RID: 11295
	Private Const PillarStateName As String = "Pillar"

	' Token: 0x04002C20 RID: 11296
	<SerializeField()>
	Private yellowDot As BasicProjectile

	' Token: 0x04002C21 RID: 11297
	<SerializeField()>
	Private wave1 As SpriteRenderer

	' Token: 0x04002C22 RID: 11298
	<SerializeField()>
	Private wave2 As SpriteRenderer

	' Token: 0x04002C23 RID: 11299
	<SerializeField()>
	Private scrollingSpritesToEnd As ScrollingSpriteSpawner()

	' Token: 0x04002C24 RID: 11300
	<SerializeField()>
	Private scrollingSprites As ScrollingSpriteSpawner()

	' Token: 0x04002C25 RID: 11301
	<SerializeField()>
	Private coral As FlyingMermaidLevelBackgroundChange

	' Token: 0x04002C26 RID: 11302
	<SerializeField()>
	Private snakeRoot As Transform

	' Token: 0x04002C27 RID: 11303
	<SerializeField()>
	Private eyebeamRoot As Transform

	' Token: 0x04002C28 RID: 11304
	<SerializeField()>
	Private bubblePrefab As FlyingMermaidLevelSkullBubble

	' Token: 0x04002C29 RID: 11305
	<SerializeField()>
	Private heatBlastPrefab As BasicProjectile

	' Token: 0x04002C2A RID: 11306
	<SerializeField()>
	Private xPosition As Single

	' Token: 0x04002C2B RID: 11307
	<SerializeField()>
	Private headBackMoveTime As Single

	' Token: 0x04002C2D RID: 11309
	Private damageDealer As DamageDealer

	' Token: 0x04002C2E RID: 11310
	Private damageReceiver As DamageReceiver

	' Token: 0x04002C2F RID: 11311
	Private patternCoroutine As Coroutine

	' Token: 0x02000691 RID: 1681
	Public Enum State
		' Token: 0x04002C31 RID: 11313
		Intro
		' Token: 0x04002C32 RID: 11314
		Idle
		' Token: 0x04002C33 RID: 11315
		HeadBlast
		' Token: 0x04002C34 RID: 11316
		Bubble
		' Token: 0x04002C35 RID: 11317
		Both
		' Token: 0x04002C36 RID: 11318
		Dead
	End Enum
End Class
