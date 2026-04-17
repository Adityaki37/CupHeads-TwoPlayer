Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020002DC RID: 732
Public Class TrainLevel
	Inherits Level

	' Token: 0x0600081A RID: 2074 RVA: 0x00078094 File Offset: 0x00076494
	Protected Overrides Sub PartialInit()
		Me.properties = LevelProperties.Train.GetMode(MyBase.mode)
		AddHandler Me.properties.OnStateChange, AddressOf MyBase.zHack_OnStateChanged
		AddHandler Me.properties.OnBossDeath, AddressOf MyBase.zHack_OnWin
		MyBase.timeline = Me.properties.CreateTimeline(MyBase.mode)
		Me.goalTimes = Me.properties.goalTimes
		AddHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		MyBase.PartialInit()
	End Sub

	' Token: 0x17000151 RID: 337
	' (get) Token: 0x0600081B RID: 2075 RVA: 0x0007812A File Offset: 0x0007652A
	Public Overrides ReadOnly Property CurrentLevel As Levels
		Get
			Return Levels.Train
		End Get
	End Property

	' Token: 0x17000152 RID: 338
	' (get) Token: 0x0600081C RID: 2076 RVA: 0x0007812D File Offset: 0x0007652D
	Public Overrides ReadOnly Property CurrentScene As Scenes
		Get
			Return Scenes.scene_level_train
		End Get
	End Property

	' Token: 0x17000153 RID: 339
	' (get) Token: 0x0600081D RID: 2077 RVA: 0x00078134 File Offset: 0x00076534
	Public Overrides ReadOnly Property BossPortrait As Sprite
		Get
			Select Case Me.currentPhase
				Case 1
					Return Me._bossPortraitSpecter
				Case 2
					Return Me._bossPortraitSkeleton
				Case 3
					Return Me._bossPortraitLollipop
				Case 4
					Return Me._bossPortraitEngine
				Case Else
					Global.Debug.LogError("Couldn't find portrait for phase " + Me.currentPhase + ". Using Main.", Nothing)
					Return Me._bossPortraitEngine
			End Select
		End Get
	End Property

	' Token: 0x17000154 RID: 340
	' (get) Token: 0x0600081E RID: 2078 RVA: 0x000781A8 File Offset: 0x000765A8
	Public Overrides ReadOnly Property BossQuote As String
		Get
			Select Case Me.currentPhase
				Case 1
					Return Me._bossQuoteSpecter
				Case 2
					Return Me._bossQuoteSkeleton
				Case 3
					Return Me._bossQuoteLollipop
				Case 4
					Return Me._bossQuoteEngine
				Case Else
					Global.Debug.LogError("Couldn't find portrait for phase " + Me.currentPhase + ". Using Main.", Nothing)
					Return Me._bossQuoteEngine
			End Select
		End Get
	End Property

	' Token: 0x0600081F RID: 2079 RVA: 0x0007821C File Offset: 0x0007661C
	Protected Overrides Sub Start()
		MyBase.Start()
		RemoveHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		MyBase.timeline = New Level.Timeline()
		MyBase.timeline.health = 0F
		MyBase.timeline.health += CSng(Me.properties.CurrentState.blindSpecter.health)
		MyBase.timeline.health += Me.properties.CurrentState.skeleton.health
		MyBase.timeline.health += Me.properties.CurrentState.lollipopGhouls.health * 2F
		MyBase.timeline.health += Me.properties.CurrentState.engine.health
		MyBase.timeline.AddEvent(New Level.Timeline.[Event]("Skeleton", 1F - CSng(Me.properties.CurrentState.blindSpecter.health) / MyBase.timeline.health))
		MyBase.timeline.AddEvent(New Level.Timeline.[Event]("Lollipop Ghouls", 1F - (CSng(Me.properties.CurrentState.blindSpecter.health) + Me.properties.CurrentState.skeleton.health) / MyBase.timeline.health))
		MyBase.timeline.AddEvent(New Level.Timeline.[Event]("Engine", 1F - (CSng(Me.properties.CurrentState.blindSpecter.health) + Me.properties.CurrentState.skeleton.health + Me.properties.CurrentState.lollipopGhouls.health * 2F) / MyBase.timeline.health))
		Me.train.LevelInit(Me.properties)
		Me.blindSpecter.LevelInit(Me.properties)
		Me.skeleton.LevelInit(Me.properties)
		Me.ghouls.LevelInit(Me.properties)
		Me.engine.LevelInit(Me.properties)
		AddHandler Me.blindSpecter.OnDeathEvent, AddressOf Me.OnBlindSpecterDeath
		AddHandler Me.skeleton.OnDeathEvent, AddressOf Me.OnSkeletonDeath
		AddHandler Me.ghouls.OnDeathEvent, AddressOf Me.OnLollipopsDeath
		AddHandler Me.engine.OnDeathEvent, AddressOf Me.OnEngineDeath
		AddHandler Me.blindSpecter.OnDamageTakenEvent, AddressOf MyBase.timeline.DealDamage
		AddHandler Me.skeleton.OnDamageTakenEvent, AddressOf MyBase.timeline.DealDamage
		AddHandler Me.ghouls.OnDamageTakenEvent, AddressOf MyBase.timeline.DealDamage
		AddHandler Me.engine.OnDamageTakenEvent, AddressOf MyBase.timeline.DealDamage
	End Sub

	' Token: 0x06000820 RID: 2080 RVA: 0x00078529 File Offset: 0x00076929
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.pumpkinPrefab = Nothing
		Me._bossPortraitEngine = Nothing
		Me._bossPortraitLollipop = Nothing
		Me._bossPortraitSkeleton = Nothing
		Me._bossPortraitSpecter = Nothing
	End Sub

	' Token: 0x06000821 RID: 2081 RVA: 0x00078554 File Offset: 0x00076954
	Protected Overrides Sub OnLevelStart()
		MyBase.StartCoroutine(Me.pumpkins_cr())
		MyBase.StartCoroutine(Me.trainPattern_cr())
		Me.setPhase(1)
	End Sub

	' Token: 0x06000822 RID: 2082 RVA: 0x00078577 File Offset: 0x00076977
	Private Sub OnBlindSpecterDeath()
		Me.train.OnBlindSpectreDeath()
		Me.setPhase(2)
	End Sub

	' Token: 0x06000823 RID: 2083 RVA: 0x0007858B File Offset: 0x0007698B
	Private Sub OnSkeletonDeath()
		Me.train.OnSkeletonDeath()
		Me.setPhase(3)
	End Sub

	' Token: 0x06000824 RID: 2084 RVA: 0x0007859F File Offset: 0x0007699F
	Private Sub OnLollipopsDeath()
		If Level.Current.mode = Level.Mode.Easy Then
			Me.properties.WinInstantly()
		Else
			Me.train.OnLollipopsDeath()
			Me.setPhase(4)
		End If
	End Sub

	' Token: 0x06000825 RID: 2085 RVA: 0x000785D2 File Offset: 0x000769D2
	Private Sub OnEngineDeath()
		Me.properties.WinInstantly()
	End Sub

	' Token: 0x06000826 RID: 2086 RVA: 0x000785E0 File Offset: 0x000769E0
	Private Sub setPhase(phase As Integer)
		Me.currentPhase = phase
		For Each text As String In Me.properties.CurrentState.pumpkins.bossPhaseOn.Split(New Char() { ","c })
			Dim num As Integer = 0
			Parser.IntTryParse(text, num)
			If num = phase Then
				Me.pumpkinsEnabled = True
				Return
			End If
		Next
		Me.pumpkinsEnabled = False
	End Sub

	' Token: 0x06000827 RID: 2087 RVA: 0x00078654 File Offset: 0x00076A54
	Private Iterator Function trainPattern_cr() As IEnumerator
		Yield New WaitForSeconds(1F)
		While True
			Yield MyBase.StartCoroutine(Me.nextPattern_cr())
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000828 RID: 2088 RVA: 0x00078670 File Offset: 0x00076A70
	Private Iterator Function nextPattern_cr() As IEnumerator
		Dim p As LevelProperties.Train.Pattern = Me.properties.CurrentState.NextPattern
		Yield New WaitForSeconds(1F)
		Return
	End Function

	' Token: 0x06000829 RID: 2089 RVA: 0x0007868C File Offset: 0x00076A8C
	Private Iterator Function pumpkins_cr() As IEnumerator
		Dim dir As Integer = If((Not Rand.Bool()), (-1), 1)
		Dim target As Transform = Me.rightValve
		Dim p As LevelProperties.Train.Pumpkins = Me.properties.CurrentState.pumpkins
		While True
			Yield CupheadTime.WaitForSeconds(Me, p.delay)
			If Me.pumpkinsEnabled Then
				Me.pumpkinPrefab.Create(New Vector2(CSng((840 * -CSng(dir))), 280F), dir, p.speed, p.health, p.fallTime, target)
				dir *= -1
				If Me.train.state <> TrainLevelTrain.State.BlindSpecter Then
					If dir < 0 Then
						target = Me.rightValve
					Else
						target = Me.leftValve
					End If
				End If
			End If
		End While
		Return
	End Function

	' Token: 0x04001087 RID: 4231
	Private properties As LevelProperties.Train

	' Token: 0x04001088 RID: 4232
	<SerializeField()>
	Private train As TrainLevelTrain

	' Token: 0x04001089 RID: 4233
	<Space(10F)>
	<SerializeField()>
	Private pumpkinPrefab As TrainLevelPumpkin

	' Token: 0x0400108A RID: 4234
	<SerializeField()>
	Private leftValve As Transform

	' Token: 0x0400108B RID: 4235
	<SerializeField()>
	Private rightValve As Transform

	' Token: 0x0400108C RID: 4236
	<Space(10F)>
	<SerializeField()>
	Private blindSpecter As TrainLevelBlindSpecter

	' Token: 0x0400108D RID: 4237
	<SerializeField()>
	Private skeleton As TrainLevelSkeleton

	' Token: 0x0400108E RID: 4238
	<SerializeField()>
	Private ghouls As TrainLevelLollipopGhoulsManager

	' Token: 0x0400108F RID: 4239
	<SerializeField()>
	Private engine As TrainLevelEngineBoss

	' Token: 0x04001090 RID: 4240
	Public handCarCollider As Collider2D

	' Token: 0x04001091 RID: 4241
	<Header("Boss Info")>
	<SerializeField()>
	Private _bossPortraitSpecter As Sprite

	' Token: 0x04001092 RID: 4242
	<SerializeField()>
	Private _bossPortraitSkeleton As Sprite

	' Token: 0x04001093 RID: 4243
	<SerializeField()>
	Private _bossPortraitLollipop As Sprite

	' Token: 0x04001094 RID: 4244
	<SerializeField()>
	Private _bossPortraitEngine As Sprite

	' Token: 0x04001095 RID: 4245
	<SerializeField()>
	Private _bossQuoteSpecter As String

	' Token: 0x04001096 RID: 4246
	<SerializeField()>
	Private _bossQuoteSkeleton As String

	' Token: 0x04001097 RID: 4247
	<SerializeField()>
	Private _bossQuoteLollipop As String

	' Token: 0x04001098 RID: 4248
	<SerializeField()>
	Private _bossQuoteEngine As String

	' Token: 0x04001099 RID: 4249
	Private pumpkinsEnabled As Boolean

	' Token: 0x0400109A RID: 4250
	Private currentPhase As Integer

	' Token: 0x02000809 RID: 2057
	<Serializable()>
	Public Class Prefabs
	End Class
End Class
