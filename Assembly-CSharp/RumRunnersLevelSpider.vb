Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Diagnostics
Imports UnityEngine

' Token: 0x0200079F RID: 1951
Public Class RumRunnersLevelSpider
	Inherits LevelProperties.RumRunners.Entity

	' Token: 0x1400004B RID: 75
	' (add) Token: 0x06002B6F RID: 11119 RVA: 0x00194310 File Offset: 0x00192710
	' (remove) Token: 0x06002B70 RID: 11120 RVA: 0x00194348 File Offset: 0x00192748
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnDeathEvent As Action

	' Token: 0x170003FE RID: 1022
	' (get) Token: 0x06002B71 RID: 11121 RVA: 0x0019437E File Offset: 0x0019277E
	' (set) Token: 0x06002B72 RID: 11122 RVA: 0x00194386 File Offset: 0x00192786
	Public Property goingLeft As Boolean

	' Token: 0x170003FF RID: 1023
	' (get) Token: 0x06002B73 RID: 11123 RVA: 0x0019438F File Offset: 0x0019278F
	' (set) Token: 0x06002B74 RID: 11124 RVA: 0x00194397 File Offset: 0x00192797
	Public Property moving As Boolean

	' Token: 0x17000400 RID: 1024
	' (get) Token: 0x06002B75 RID: 11125 RVA: 0x001943A0 File Offset: 0x001927A0
	Private ReadOnly Property dir As Single
		Get
			Return CSng(If((Not Me.goingLeft), 1, (-1)))
		End Get
	End Property

	' Token: 0x06002B76 RID: 11126 RVA: 0x001943B8 File Offset: 0x001927B8
	Private Sub Start()
		Me.goingLeft = True
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		Me.collider = MyBase.GetComponent(Of Collider2D)()
		Me.scaleX = MyBase.transform.localScale.x
		MyBase.transform.SetScale(New Single?(Me.scaleX * Me.dir), Nothing, Nothing)
		Me.SetUpMinePositions()
		Me.grubEnterVariant = Global.UnityEngine.Random.Range(0, 3)
		Me.grubVariant = Global.UnityEngine.Random.Range(0, 4)
	End Sub

	' Token: 0x06002B77 RID: 11127 RVA: 0x00194470 File Offset: 0x00192870
	Public Overrides Sub LevelInit(properties As LevelProperties.RumRunners)
		MyBase.LevelInit(properties)
		Me.mineMainIndex = Global.UnityEngine.Random.Range(0, properties.CurrentState.mine.minePlacementString.Length)
		Me.mineIndex = Global.UnityEngine.Random.Range(0, properties.CurrentState.mine.minePlacementString(Me.mineMainIndex).Split(New Char() { ","c }).Length)
		Me.bouncingPattern = New PatternString(properties.CurrentState.bouncing.shootBeetleAngleString, True, True)
		Me.grubDelayString = New PatternString(properties.CurrentState.grubs.delayString, True, True)
		Me.grubPositionString = New PatternString(properties.CurrentState.grubs.appearPositionString, True, False)
		AddHandler Level.Current.OnLevelStartEvent, AddressOf Me.OnIntroEnd
	End Sub

	' Token: 0x06002B78 RID: 11128 RVA: 0x00194544 File Offset: 0x00192944
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Dim x As Single = MyBase.transform.position.x
		Dim num As Single = CSng((-CSng(Level.Current.Width))) * 0.5F + Me.deathInvincibilityBuffer
		Dim num2 As Single = CSng(Level.Current.Width) * 0.5F - Me.deathInvincibilityBuffer
		Dim num3 As Single = MyBase.properties.CurrentHealth - MyBase.properties.GetNextStateHealthTrigger() * MyBase.properties.TotalHealth
		If info.damage > num3 AndAlso (x > num2 OrElse x < num) Then
			Return
		End If
		MyBase.properties.DealDamage(info.damage)
	End Sub

	' Token: 0x06002B79 RID: 11129 RVA: 0x001945EA File Offset: 0x001929EA
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If Me.damageDealer IsNot Nothing AndAlso phase = CollisionPhase.Enter Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06002B7A RID: 11130 RVA: 0x00194612 File Offset: 0x00192A12
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06002B7B RID: 11131 RVA: 0x0019462C File Offset: 0x00192A2C
	Private Sub OnIntroEnd()
		Me.policeman.SetProperties(MyBase.properties.CurrentState.spider, Me)
		RemoveHandler Level.Current.OnLevelStartEvent, AddressOf Me.OnIntroEnd
		MyBase.StartCoroutine(Me.introExit())
	End Sub

	' Token: 0x06002B7C RID: 11132 RVA: 0x00194678 File Offset: 0x00192A78
	Private Iterator Function introExit() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 0.2F)
		MyBase.animator.SetTrigger("IntroExit")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "IntroExit", False, True)
		MyBase.StartCoroutine(Me.run_cr())
		Return
	End Function

	' Token: 0x06002B7D RID: 11133 RVA: 0x00194693 File Offset: 0x00192A93
	Private Sub SummonSelection()
		MyBase.StartCoroutine(Me.check_to_start_summon_cr())
	End Sub

	' Token: 0x06002B7E RID: 11134 RVA: 0x001946A4 File Offset: 0x00192AA4
	Private Iterator Function check_to_start_summon_cr() As IEnumerator
		Dim summonType As RumRunnersLevelSpider.SummonType = Me.summonType
		If summonType <> RumRunnersLevelSpider.SummonType.Bouncing Then
			If summonType <> RumRunnersLevelSpider.SummonType.Mine Then
				If summonType = RumRunnersLevelSpider.SummonType.Grubs Then
					MyBase.animator.Play("GrubSummonWait")
					Yield Nothing
					Me.isSummoning = True
					Me.StartGrubs()
					Yield MyBase.animator.WaitForAnimationToEnd(Me, "GrubSummonWait", False, True)
					Me.isSummoning = False
				End If
			Else
				MyBase.animator.Play("MineSummon")
				Yield Nothing
				Me.isSummoning = True
				While MyBase.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.5F
					Yield Nothing
				End While
				Me.StartMine()
				Yield MyBase.animator.WaitForAnimationToEnd(Me, "MineSummon", False, True)
				Me.isSummoning = False
			End If
		Else
			MyBase.animator.Play("Kick")
			Yield Nothing
			Me.isSummoning = True
			Yield MyBase.animator.WaitForAnimationToEnd(Me, "Kick", False, True)
			Me.isSummoning = False
		End If
		Return
	End Function

	' Token: 0x06002B7F RID: 11135 RVA: 0x001946BF File Offset: 0x00192ABF
	Private Sub animationEvent_SpawnFrontPuffEffect()
	End Sub

	' Token: 0x06002B80 RID: 11136 RVA: 0x001946C1 File Offset: 0x00192AC1
	Private Sub animationEvent_SpawnBackPuffEffect()
	End Sub

	' Token: 0x06002B81 RID: 11137 RVA: 0x001946C4 File Offset: 0x00192AC4
	Private Iterator Function run_cr() As IEnumerator
		Dim p As LevelProperties.RumRunners.Spider = MyBase.properties.CurrentState.spider
		Dim hasSummoned As Boolean = False
		Dim spawnedCop As Boolean = False
		Me.moving = False
		Dim copPositionString As PatternString = New PatternString(p.copPositionString, True, True)
		Dim copBulletTypeString As PatternString = New PatternString(p.copBulletTypeString, True, True)
		Dim spiderPositionString As PatternString = New PatternString(p.spiderPositionString, True, True)
		Dim spiderActionString As PatternString = New PatternString(p.spiderActionString, True, True)
		Dim spiderActionPositionString As PatternString = New PatternString(p.spiderActionPositionString, True)
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		Me.copSpawnPos = p.copSpawnSpiderDist
		Dim isInitial As Boolean = True
		While True
			Dim summonChar As Char
			If isInitial Then
				summonChar = If((Not Rand.Bool()), "M"c, "N"c)
				spawnedCop = True
			Else
				Yield CupheadTime.WaitForSeconds(Me, p.spiderEnterDelay)
				summonChar = spiderActionString.PopLetter()
			End If
			Dim spawnPointIndex As Integer = spiderPositionString.PopInt()
			Dim popOutX As Single = CSng(If((Not Me.goingLeft), (-640), 640))
			Dim summonPos As Single = Mathf.Lerp(popOutX, -popOutX, spiderActionPositionString.PopFloat())
			If summonChar <> "B"c Then
				If summonChar <> "G"c Then
					If summonChar <> "M"c Then
						Me.summonType = RumRunnersLevelSpider.SummonType.None
						hasSummoned = True
					Else
						Me.summonType = RumRunnersLevelSpider.SummonType.Mine
					End If
				Else
					Me.summonType = RumRunnersLevelSpider.SummonType.Grubs
				End If
			Else
				summonPos = Mathf.Lerp(popOutX, -popOutX, 0.05F)
				Me.summonType = RumRunnersLevelSpider.SummonType.Bouncing
				If spawnPointIndex = 2 Then
					spawnPointIndex = If((Not Rand.Bool()), 1, 0)
				End If
			End If
			If Not isInitial Then
				MyBase.transform.position = New Vector3((CSng(Level.Current.Right) + 350F) * -Me.dir, Me.spawnPoints(spawnPointIndex).position.y)
			End If
			If isInitial AndAlso Me.summonType = RumRunnersLevelSpider.SummonType.Mine Then
				hasSummoned = False
				summonPos = Mathf.Lerp(popOutX, -popOutX, 0.75F)
			End If
			Dim timeToSummon As Single = Mathf.Abs(MyBase.transform.position.x - summonPos) / p.spiderSpeed
			Dim animatorStartTime As Single = 1F - timeToSummon / Me.runClip.length
			Dim copPos As Integer = copPositionString.PopInt()
			Me.nextCopPosition = New Vector3(Me.spawnPoints(copPos).position.x * Me.dir, Me.spawnPoints(copPos).position.y)
			MyBase.transform.SetScale(New Single?(Me.scaleX * Me.dir), Nothing, Nothing)
			If Not isInitial Then
				Dim text As String = "Run"
				If Me.summonType = RumRunnersLevelSpider.SummonType.Grubs Then
					text = "GrubSummonEnter"
				ElseIf Me.summonType = RumRunnersLevelSpider.SummonType.Bouncing Then
					text = "RunCaterpillar"
				End If
				MyBase.animator.Play(text, 0, animatorStartTime)
			End If
			Dim isPink As Boolean = copBulletTypeString.PopLetter() = "P"c
			Me.moving = True
			If Me.summonType = RumRunnersLevelSpider.SummonType.Grubs Then
				Me.SFX_RUMRUN_Spider_GrubSummon_PhoneTinyVoice()
			End If
			While (Me.goingLeft AndAlso MyBase.transform.position.x > popOutX) OrElse (Not Me.goingLeft AndAlso MyBase.transform.position.x < popOutX)
				MyBase.transform.position += Vector3.right * p.spiderSpeed * CupheadTime.FixedDelta * Me.dir
				MyBase.transform.SetPosition(Nothing, New Single?(RumRunnersLevel.GroundWalkingPosY(MyBase.transform.position, Me.collider, 0F, 200F)), Nothing)
				Yield wait
			End While
			While Me.moving
				If Not hasSummoned AndAlso ((Me.goingLeft AndAlso MyBase.transform.position.x <= summonPos) OrElse (Not Me.goingLeft AndAlso MyBase.transform.position.x >= summonPos)) Then
					Me.SummonSelection()
					hasSummoned = True
				End If
				While Me.isSummoning
					Yield Nothing
				End While
				If(Not Me.goingLeft AndAlso CSng(Level.Current.Right) + 350F > MyBase.transform.position.x) OrElse (Me.goingLeft AndAlso CSng(Level.Current.Left) - 350F < MyBase.transform.position.x) Then
					Dim copSpawnDistanceRemaining As Single = Me.copSpawnPos - Me.dir * MyBase.transform.position.x
					If Not spawnedCop AndAlso copSpawnDistanceRemaining < 0F Then
						Me.policeman.CopAppear(Me.nextCopPosition, isPink, Me.goingLeft)
						spawnedCop = True
						Me.nextCopPosition = Vector3.up * 5000F
					End If
					MyBase.transform.position += Vector3.right * p.spiderSpeed * CupheadTime.FixedDelta * Me.dir
					MyBase.transform.SetPosition(Nothing, New Single?(RumRunnersLevel.GroundWalkingPosY(MyBase.transform.position, Me.collider, 0F, 200F)), Nothing)
					Yield wait
				Else
					Me.moving = False
				End If
			End While
			hasSummoned = False
			spawnedCop = False
			Me.goingLeft = Not Me.goingLeft
			isInitial = False
			Yield wait
		End While
		Return
	End Function

	' Token: 0x06002B82 RID: 11138 RVA: 0x001946E0 File Offset: 0x00192AE0
	Public Function GrubCanEnter(pos As Vector3, enterTime As Single) As Boolean
		If Not Me.moving Then
			Return False
		End If
		If Mathf.Abs(MyBase.transform.position.y - pos.y) < 100F AndAlso (Mathf.Abs(MyBase.transform.position.x + MyBase.properties.CurrentState.spider.spiderSpeed * Me.dir * enterTime - pos.x) < 500F OrElse Mathf.Abs(MyBase.transform.position.x - pos.x) < 500F) Then
			Return False
		End If
		If Mathf.Abs(MyBase.transform.position.x) > 400F Then
			If Me.policeman.isActive AndAlso Mathf.Abs(Me.policeman.transform.position.y - pos.y) < 100F AndAlso Mathf.Sign(Me.policeman.transform.position.x) = Mathf.Sign(pos.x) Then
				Return False
			End If
			If Mathf.Abs(Me.nextCopPosition.y - pos.y) < 100F AndAlso Mathf.Sign(Me.nextCopPosition.x) = Mathf.Sign(pos.x) Then
				Return False
			End If
		End If
		Me.grubList.RemoveAll(Function(g As RumRunnersLevelGrub) g Is Nothing)
		For i As Integer = 0 To Me.grubList.Count - 1
			If Me.grubList(i).startedEntering AndAlso Mathf.Abs(Me.grubList(i).transform.position.y - pos.y) < 100F Then
				Dim flag As Boolean = Not Me.grubList(i).moving
				If Mathf.Abs((Me.grubList(i).transform.position + Vector3.right * Me.grubList(i).speed * (enterTime - Me.grubList(i).GetTimeToMove())).x - pos.x) < 200F Then
					Return False
				End If
				If flag AndAlso Mathf.Abs((Me.grubList(i).transform.position + Vector3.left * Me.grubList(i).speed * (enterTime - Me.grubList(i).GetTimeToMove())).x - pos.x) < 200F Then
					Return False
				End If
			End If
		Next
		Return True
	End Function

	' Token: 0x06002B83 RID: 11139 RVA: 0x00194A0D File Offset: 0x00192E0D
	Private Sub StartGrubs()
		MyBase.StartCoroutine(Me.grubs_cr())
	End Sub

	' Token: 0x06002B84 RID: 11140 RVA: 0x00194A1C File Offset: 0x00192E1C
	Private Iterator Function grubs_cr() As IEnumerator
		Dim p As LevelProperties.RumRunners.Grubs = MyBase.properties.CurrentState.grubs
		Dim delay As Single = 0F
		Dim y As Integer = 0
		Dim x As Integer = 0
		Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.grubs.grubSummonWarning)
		Dim count As Integer = Me.grubPositionString.SubStringLength()
		Me.grubPositionString.SetSubStringIndex(count)
		For i As Integer = 0 To count - 1
			Dim player As AbstractPlayerController = PlayerManager.GetNext()
			Dim appearPosition As Integer = Me.grubPositionString.PopInt()
			x = appearPosition Mod 6
			y = appearPosition / 6
			If x <> 5 OrElse y <> 2 Then
				Me.grubList.RemoveAll(Function(g As RumRunnersLevelGrub) g Is Nothing)
				Dim canSpawn As Boolean = True
				For j As Integer = 0 To Me.grubList.Count - 1
					If Not Me.grubList(j).startedEntering AndAlso Me.grubList(j).x = x AndAlso Me.grubList(j).y = y Then
						canSpawn = False
					End If
				Next
				If canSpawn Then
					Me.grubList.Add(Me.grubPrefab.Create(Me.grubPaths(y * 6 + x), 0F, p.movementSpeed, p.warningDuration, p.hp, Me, Me.grubEnterVariant, Me.grubVariant, count - i, x, y))
					Me.grubEnterVariant = (Me.grubEnterVariant + 1) Mod 3
					Me.grubVariant = (Me.grubVariant + 1) Mod 4
					If i < count - 1 Then
						delay = Me.grubDelayString.PopFloat()
						Yield CupheadTime.WaitForSeconds(Me, delay)
					End If
				End If
			End If
		Next
		Yield Nothing
		Return
	End Function

	' Token: 0x06002B85 RID: 11141 RVA: 0x00194A38 File Offset: 0x00192E38
	Private Sub SetUpMinePositions()
		Me.minePositions = New Vector3(4, 2) {}
		Me.minePositions(0, 0) = New Vector3(-553F, 354F)
		Me.minePositions(1, 0) = New Vector3(-282F, 321F)
		Me.minePositions(2, 0) = New Vector3(16F, 354F)
		Me.minePositions(3, 0) = New Vector3(311F, 313F)
		Me.minePositions(4, 0) = New Vector3(545F, 343F)
		Me.minePositions(0, 1) = New Vector3(-492F, 33F)
		Me.minePositions(1, 1) = New Vector3(-247F, 19F)
		Me.minePositions(2, 1) = New Vector3(42F, 35F)
		Me.minePositions(3, 1) = New Vector3(287F, 7F)
		Me.minePositions(4, 1) = New Vector3(509F, 36F)
		Me.minePositions(0, 2) = New Vector3(-524F, -284F)
		Me.minePositions(1, 2) = New Vector3(-224F, -265F)
		Me.minePositions(2, 2) = New Vector3(-17F, -294F)
		Me.minePositions(3, 2) = New Vector3(253F, -252F)
		Me.minePositions(4, 2) = New Vector3(575F, -291F)
	End Sub

	' Token: 0x06002B86 RID: 11142 RVA: 0x00194C41 File Offset: 0x00193041
	Public Sub StartMine()
		MyBase.StartCoroutine(Me.mine_cr())
	End Sub

	' Token: 0x06002B87 RID: 11143 RVA: 0x00194C50 File Offset: 0x00193050
	Private Iterator Function mine_cr() As IEnumerator
		Dim p As LevelProperties.RumRunners.Mine = MyBase.properties.CurrentState.mine
		Dim player As AbstractPlayerController = PlayerManager.GetPlayer(PlayerId.PlayerOne)
		Dim player2 As AbstractPlayerController = PlayerManager.GetPlayer(PlayerId.PlayerTwo)
		Dim pos As Vector2 = Vector2.zero
		Dim minePlacementString As String() = p.minePlacementString(Me.mineMainIndex).Split(New Char() { ","c })
		Me.mineList.RemoveAll(Function(m As RumRunnersLevelMine) m Is Nothing)
		Me.mineIndex = 0
		Dim i As Integer = 0
		While CSng(i) < Mathf.Min(p.mineNumber, CSng(minePlacementString.Length))
			pos = Me.GetMinePos(Parser.IntParse(minePlacementString(Me.mineIndex)))
			Dim foundFreeSpot As Boolean = False
			Dim checkedPositionsCount As Integer = 0
			While Not foundFreeSpot AndAlso checkedPositionsCount < 15
				Dim distP As Single = 1000F
				Dim distP2 As Single = 1000F
				Dim spotOccupied As Boolean = False
				For j As Integer = 0 To Me.mineList.Count - 1
					If Me.mineList(j).xPos = CInt(pos.x) AndAlso Me.mineList(j).yPos = CInt(pos.y) Then
						spotOccupied = True
					End If
				Next
				If Not spotOccupied Then
					If Not player.IsDead Then
						distP = Vector3.Distance(player.transform.position, Me.minePositions(CInt(pos.x), CInt(pos.y)))
					End If
					If player2 IsNot Nothing AndAlso Not player2.IsDead Then
						distP2 = Vector3.Distance(player2.transform.position, Me.minePositions(CInt(pos.x), CInt(pos.y)))
					End If
				End If
				If distP > p.mineCheckToLand AndAlso distP2 > p.mineCheckToLand AndAlso Not spotOccupied Then
					foundFreeSpot = True
					Exit While
				End If
				If Me.mineIndex < minePlacementString.Length - 1 Then
					Me.mineIndex += 1
				Else
					Me.mineMainIndex = (Me.mineMainIndex + 1) Mod p.minePlacementString.Length
					Me.mineIndex = 0
				End If
				pos = Me.GetMinePos(Parser.IntParse(minePlacementString(Me.mineIndex)))
				checkedPositionsCount += 1
				Yield Nothing
			End While
			If checkedPositionsCount < 15 Then
				Dim rumRunnersLevelMine As RumRunnersLevelMine = Me.minePrefab.Spawn()
				Me.mineList.Add(rumRunnersLevelMine.Init(Me.minePositions(CInt(pos.x), CInt(pos.y)), p, Me, Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y)))
			End If
			Yield CupheadTime.WaitForSeconds(Me, 0.2F)
			If Me.mineIndex < minePlacementString.Length - 1 Then
				Me.mineIndex += 1
			Else
				Me.mineMainIndex = (Me.mineMainIndex + 1) Mod p.minePlacementString.Length
				Me.mineIndex = 0
			End If
			i += 1
		End While
		Yield Nothing
		Return
	End Function

	' Token: 0x06002B88 RID: 11144 RVA: 0x00194C6C File Offset: 0x0019306C
	Private Function GetMinePos(mineNum As Integer) As Vector2
		Dim zero As Vector2 = Vector2.zero
		zero.x = CSng((mineNum Mod 5))
		zero.y = CSng((mineNum / 5))
		If zero.x > 4F OrElse zero.x < 0F OrElse zero.y > 2F OrElse zero.y < 0F Then
			Global.Debug.Break()
		End If
		Return zero
	End Function

	' Token: 0x06002B89 RID: 11145 RVA: 0x00194CE0 File Offset: 0x001930E0
	Private Sub animationEvent_StartBouncing()
		Dim bouncing As LevelProperties.RumRunners.Bouncing = MyBase.properties.CurrentState.bouncing
		Do
			Me.beetleList.RemoveAll(Function(b As RumRunnersLevelBouncingBeetle) b Is Nothing OrElse b.leaveScreen)
			If Me.beetleList.Count >= bouncing.maxBeetleCount Then
				Me.beetleList(0).leaveScreen = True
			End If
		Loop While Me.beetleList.Count >= bouncing.maxBeetleCount
		Dim num As Single = Me.bouncingPattern.PopFloat()
		num = Mathf.Clamp(num, 10F, 80F)
		If Me.dir < 0F Then
			num = 180F - num
		End If
		Dim vector As Vector3 = MathUtils.AngleToDirection(num)
		Dim rumRunnersLevelBouncingBeetle As RumRunnersLevelBouncingBeetle = Me.caterpillarPrefab.Spawn()
		rumRunnersLevelBouncingBeetle.Init(Me.caterpillarSpawnPoint.position + vector * 110F, vector, bouncing.shootBeetleInitialSpeed, CSng(bouncing.shootBeetleTimeToSlowdown), bouncing.shootBeetleSpeed, bouncing.shootBeetleHealth)
		Me.beetleList.Add(rumRunnersLevelBouncingBeetle)
		Me.kickFXEffect.Create(Me.kickFXSpawnPoint.position + vector * 110F * 0.8F)
	End Sub

	' Token: 0x06002B8A RID: 11146 RVA: 0x00194E30 File Offset: 0x00193230
	Public Sub Die()
		MyBase.animator.SetTrigger("Dead")
		MyBase.GetComponent(Of SpriteRenderer)().sortingLayerName = SpriteLayer.Effects.ToString()
		MyBase.GetComponent(Of SpriteRenderer)().sortingOrder = 0
		Me.StopAllCoroutines()
		Me.deathExplodeEffect.Create(MyBase.transform.position)
		For Each rumRunnersLevelBouncingBeetle As RumRunnersLevelBouncingBeetle In Me.beetleList
			rumRunnersLevelBouncingBeetle.leaveScreen = True
		Next
		Me.mineList.RemoveAll(Function(m As RumRunnersLevelMine) m Is Nothing)
		Me.mineList.Sort(Function(m1 As RumRunnersLevelMine, m2 As RumRunnersLevelMine) m1.endPhaseExplodePriority.CompareTo(m2.endPhaseExplodePriority))
		For i As Integer = 0 To Me.mineList.Count - 1
			Me.mineList(i).SetTimer(CSng(i) * 0.6F)
		Next
		If Me.OnDeathEvent IsNot Nothing Then
			Me.OnDeathEvent()
		End If
		Me.SFX_RUMRUN_ExitPhase1_SpiderFalling()
	End Sub

	' Token: 0x06002B8B RID: 11147 RVA: 0x00194F84 File Offset: 0x00193384
	Private Sub AniEvent_ChangeToForeground()
		MyBase.GetComponent(Of SpriteRenderer)().sortingLayerName = SpriteLayer.Foreground.ToString()
		MyBase.GetComponent(Of SpriteRenderer)().sortingOrder = 100
	End Sub

	' Token: 0x06002B8C RID: 11148 RVA: 0x00194FB8 File Offset: 0x001933B8
	Private Sub animationEvent_ShakeScreen()
		CupheadLevelCamera.Current.Shake(30F, 0.6F, False)
	End Sub

	' Token: 0x06002B8D RID: 11149 RVA: 0x00194FCF File Offset: 0x001933CF
	Private Sub AniEvent_DeathComplete()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06002B8E RID: 11150 RVA: 0x00194FDC File Offset: 0x001933DC
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		If Level.Current Then
			Dim num As Single = Me.copSpawnPos * Me.dir
			Dim vector As Vector3 = New Vector3(num, -360F)
			Dim vector2 As Vector3 = New Vector3(num, 360F)
			Gizmos.color = Color.blue
			Gizmos.DrawLine(vector, vector2)
		End If
		Gizmos.DrawWireSphere(Me.caterpillarSpawnPoint.position, 110F)
	End Sub

	' Token: 0x06002B8F RID: 11151 RVA: 0x0019504C File Offset: 0x0019344C
	Private Sub AnimationEvent_SFX_RUMRUN_Mine_SpiderButtonPress()
		AudioManager.Play("sfx_dlc_rumrun_mine_spiderbuttonpress")
	End Sub

	' Token: 0x06002B90 RID: 11152 RVA: 0x00195058 File Offset: 0x00193458
	Private Sub AnimationEvent_SFX_RUMRUN_Spider_GrubSummon_Phone()
		AudioManager.Play("sfx_dlc_rumrun_spider_grubsummon_phone")
		AudioManager.[Stop]("sfx_dlc_rumrun_spider_grubsummon_phonetinyvoice")
	End Sub

	' Token: 0x06002B91 RID: 11153 RVA: 0x0019506E File Offset: 0x0019346E
	Private Sub SFX_RUMRUN_Spider_GrubSummon_PhoneTinyVoice()
		AudioManager.Play("sfx_dlc_rumrun_spider_grubsummon_phonetinyvoice")
		Me.emitAudioFromObject.Add("sfx_dlc_rumrun_spider_grubsummon_phonetinyvoice")
	End Sub

	' Token: 0x06002B92 RID: 11154 RVA: 0x0019508A File Offset: 0x0019348A
	Private Sub AnimationEvent_SFX_RUMRUN_CaterpillarBall_SpiderKick()
		AudioManager.Play("sfx_dlc_rumrun_caterpillarball_spiderkick")
	End Sub

	' Token: 0x06002B93 RID: 11155 RVA: 0x00195096 File Offset: 0x00193496
	Private Sub SFX_RUMRUN_ExitPhase1_SpiderFalling()
		AudioManager.Play("sfx_DLC_RUMRUN_ExitPhase1_SpiderFalling")
		AudioManager.FadeSFXVolume("sfx_DLC_RUMRUN_ExitPhase1_SpiderFalling", 1F, 10F)
	End Sub

	' Token: 0x0400341C RID: 13340
	Private Const INTRO_EXIT_DELAY As Single = 0.2F

	' Token: 0x0400341D RID: 13341
	Private Const EDGE_OFFSET As Single = 350F

	' Token: 0x0400341E RID: 13342
	Private Const MINE_DELAY As Single = 0.2F

	' Token: 0x0400341F RID: 13343
	Private Const MINE_EXPLODE_INTERVAL_ON_PHASE_END As Single = 0.6F

	' Token: 0x04003420 RID: 13344
	Private Const GRUB_MIN_SPIDER_DISTANCE_TO_ENTER As Single = 500F

	' Token: 0x04003421 RID: 13345
	Private Const GRUB_MIN_OTHER_GRUB_DISTANCE_TO_ENTER As Single = 200F

	' Token: 0x04003422 RID: 13346
	Private Const BOUNCER_MIN_ANGLE As Single = 10F

	' Token: 0x04003423 RID: 13347
	Private Const BOUNCER_MAX_ANGLE As Single = 80F

	' Token: 0x04003424 RID: 13348
	Private Const KICK_SPAWN_RADIUS As Single = 110F

	' Token: 0x04003425 RID: 13349
	<SerializeField()>
	Private spawnPoints As Transform()

	' Token: 0x04003426 RID: 13350
	<SerializeField()>
	Private runClip As AnimationClip

	' Token: 0x04003427 RID: 13351
	<SerializeField()>
	Private policeman As RumRunnersLevelPoliceman

	' Token: 0x04003428 RID: 13352
	<SerializeField()>
	Private deathInvincibilityBuffer As Single

	' Token: 0x04003429 RID: 13353
	<SerializeField()>
	Private deathExplodeEffect As Effect

	' Token: 0x0400342A RID: 13354
	<Header("Summons")>
	<SerializeField()>
	Private grubPrefab As RumRunnersLevelGrub

	' Token: 0x0400342B RID: 13355
	<SerializeField()>
	Private grubPaths As RumRunnersLevelGrubPath()

	' Token: 0x0400342C RID: 13356
	<SerializeField()>
	Private minePrefab As RumRunnersLevelMine

	' Token: 0x0400342D RID: 13357
	<SerializeField()>
	Private caterpillarPrefab As RumRunnersLevelBouncingBeetle

	' Token: 0x0400342E RID: 13358
	<SerializeField()>
	Private caterpillarSpawnPoint As Transform

	' Token: 0x0400342F RID: 13359
	<SerializeField()>
	Private kickFXEffect As Effect

	' Token: 0x04003430 RID: 13360
	<SerializeField()>
	Private kickFXSpawnPoint As Transform

	' Token: 0x04003433 RID: 13363
	Private summonType As RumRunnersLevelSpider.SummonType

	' Token: 0x04003434 RID: 13364
	Public isSummoning As Boolean

	' Token: 0x04003435 RID: 13365
	Private nextCopPosition As Vector3

	' Token: 0x04003436 RID: 13366
	Private grubDelayString As PatternString

	' Token: 0x04003437 RID: 13367
	Private grubPositionString As PatternString

	' Token: 0x04003438 RID: 13368
	Private grubList As List(Of RumRunnersLevelGrub) = New List(Of RumRunnersLevelGrub)()

	' Token: 0x04003439 RID: 13369
	Private mineMainIndex As Integer

	' Token: 0x0400343A RID: 13370
	Private mineIndex As Integer

	' Token: 0x0400343B RID: 13371
	Private mineList As List(Of RumRunnersLevelMine) = New List(Of RumRunnersLevelMine)()

	' Token: 0x0400343C RID: 13372
	Private minePositions As Vector3(,)

	' Token: 0x0400343D RID: 13373
	Private bouncingPattern As PatternString

	' Token: 0x0400343E RID: 13374
	Private grubEnterVariant As Integer

	' Token: 0x0400343F RID: 13375
	Private grubVariant As Integer

	' Token: 0x04003440 RID: 13376
	Private damageDealer As DamageDealer

	' Token: 0x04003441 RID: 13377
	Private damageReceiver As DamageReceiver

	' Token: 0x04003442 RID: 13378
	Private collider As Collider2D

	' Token: 0x04003443 RID: 13379
	Private scaleX As Single

	' Token: 0x04003444 RID: 13380
	Private copSpawnPos As Single

	' Token: 0x04003445 RID: 13381
	Private beetleList As List(Of RumRunnersLevelBouncingBeetle) = New List(Of RumRunnersLevelBouncingBeetle)()

	' Token: 0x020007A0 RID: 1952
	Private Enum SummonType
		' Token: 0x0400344B RID: 13387
		Grubs
		' Token: 0x0400344C RID: 13388
		Mine
		' Token: 0x0400344D RID: 13389
		Bouncing
		' Token: 0x0400344E RID: 13390
		None
	End Enum
End Class
