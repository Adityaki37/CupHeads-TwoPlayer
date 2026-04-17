Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200085A RID: 2138
<RequireComponent(GetType(DamageReceiver))>
<RequireComponent(GetType(HitFlash))>
<RequireComponent(GetType(Collider2D))>
<RequireComponent(GetType(Rigidbody2D))>
Public MustInherit Class AbstractPlatformingLevelEnemy
	Inherits AbstractLevelEntity

	' Token: 0x1700042D RID: 1069
	' (get) Token: 0x06003193 RID: 12691 RVA: 0x001CED90 File Offset: 0x001CD190
	Public ReadOnly Property ID As EnemyID
		Get
			Return Me._id
		End Get
	End Property

	' Token: 0x1700042E RID: 1070
	' (get) Token: 0x06003194 RID: 12692 RVA: 0x001CED98 File Offset: 0x001CD198
	Public ReadOnly Property StartDelay As Single
		Get
			Return Me._startDelay
		End Get
	End Property

	' Token: 0x1700042F RID: 1071
	' (get) Token: 0x06003195 RID: 12693 RVA: 0x001CEDA0 File Offset: 0x001CD1A0
	Public ReadOnly Property Properties As EnemyProperties
		Get
			If Me._properties Is Nothing Then
				Me._properties = EnemyDatabase.GetProperties(Me._id)
			End If
			Return Me._properties
		End Get
	End Property

	' Token: 0x17000430 RID: 1072
	' (get) Token: 0x06003196 RID: 12694 RVA: 0x001CEDC4 File Offset: 0x001CD1C4
	' (set) Token: 0x06003197 RID: 12695 RVA: 0x001CEDCC File Offset: 0x001CD1CC
	Public Property Health As Single

	' Token: 0x17000431 RID: 1073
	' (get) Token: 0x06003198 RID: 12696 RVA: 0x001CEDD5 File Offset: 0x001CD1D5
	' (set) Token: 0x06003199 RID: 12697 RVA: 0x001CEDDD File Offset: 0x001CD1DD
	Public Property Dead As Boolean

	' Token: 0x17000432 RID: 1074
	' (get) Token: 0x0600319A RID: 12698 RVA: 0x001CEDE6 File Offset: 0x001CD1E6
	' (set) Token: 0x0600319B RID: 12699 RVA: 0x001CEDEE File Offset: 0x001CD1EE
	Protected Private Property _damageReceiver As DamageReceiver

	' Token: 0x17000433 RID: 1075
	' (get) Token: 0x0600319C RID: 12700 RVA: 0x001CEDF7 File Offset: 0x001CD1F7
	' (set) Token: 0x0600319D RID: 12701 RVA: 0x001CEDFF File Offset: 0x001CD1FF
	Protected Private Property _damageDealer As DamageDealer

	' Token: 0x17000434 RID: 1076
	' (get) Token: 0x0600319E RID: 12702 RVA: 0x001CEE08 File Offset: 0x001CD208
	' (set) Token: 0x0600319F RID: 12703 RVA: 0x001CEE10 File Offset: 0x001CD210
	Protected Private Property _started As Boolean

	' Token: 0x060031A0 RID: 12704 RVA: 0x001CEE1C File Offset: 0x001CD21C
	Protected Overrides Sub Awake()
		MyBase.Awake()
		If Me.Properties Is Nothing Then
			Me.Health = 10F
			Me._canParry = False
		Else
			Me.Health = Me.Properties.Health
			Me._canParry = Me.Properties.CanParry
		End If
		Me._damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		Me._damageDealer = DamageDealer.NewEnemy()
		AddHandler Me._damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
	End Sub

	' Token: 0x060031A1 RID: 12705 RVA: 0x001CEEA2 File Offset: 0x001CD2A2
	Protected Overridable Sub Start()
		Me.StartWithCondition(AbstractPlatformingLevelEnemy.StartCondition.Instant)
		AddHandler Level.Current.OnLevelStartEvent, AddressOf Me.OnLevelStart
	End Sub

	' Token: 0x060031A2 RID: 12706 RVA: 0x001CEEC1 File Offset: 0x001CD2C1
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		If Level.Current IsNot Nothing Then
			RemoveHandler Level.Current.OnLevelStartEvent, AddressOf Me.OnLevelStart
		End If
		Me.explosionPrefabs = Nothing
		Me.parryEffectPrefab = Nothing
	End Sub

	' Token: 0x060031A3 RID: 12707 RVA: 0x001CEF00 File Offset: 0x001CD300
	Protected Overridable Sub Update()
		If Me._startCondition = AbstractPlatformingLevelEnemy.StartCondition.TriggerVolume AndAlso Not Me._started Then
			Dim rect As Rect = RectUtils.NewFromCenter(Me._triggerPosition.x, Me._triggerPosition.y, Me._triggerSize.x, Me._triggerSize.y)
			If rect.Contains(PlayerManager.GetPlayer(PlayerId.PlayerOne).center) OrElse (PlayerManager.GetPlayer(PlayerId.PlayerTwo) IsNot Nothing AndAlso rect.Contains(PlayerManager.GetPlayer(PlayerId.PlayerTwo).center)) Then
				Me.StartWithCondition(AbstractPlatformingLevelEnemy.StartCondition.TriggerVolume)
			End If
		End If
		If Me._damageDealer IsNot Nothing Then
			Me._damageDealer.Update()
		End If
	End Sub

	' Token: 0x060031A4 RID: 12708 RVA: 0x001CEFB2 File Offset: 0x001CD3B2
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If Me._damageDealer IsNot Nothing AndAlso phase <> CollisionPhase.[Exit] Then
			Me._damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x060031A5 RID: 12709 RVA: 0x001CEFDB File Offset: 0x001CD3DB
	Protected Overridable Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.Health -= info.damage
		If Me.Health <= 0F Then
			Level.ScoringData.pacifistRun = False
			Me.Die()
		End If
	End Sub

	' Token: 0x060031A6 RID: 12710 RVA: 0x001CF011 File Offset: 0x001CD411
	Public Overrides Sub OnLevelEnd()
		MyBase.OnLevelEnd()
		Me.Die()
	End Sub

	' Token: 0x060031A7 RID: 12711 RVA: 0x001CF01F File Offset: 0x001CD41F
	Protected Overridable Sub Die()
		Me.IdleSounds = False
		If Me.Dead Then
			Return
		End If
		Me.Dead = True
		Me.Explode()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x060031A8 RID: 12712 RVA: 0x001CF04C File Offset: 0x001CD44C
	Protected Sub Explode()
		If Me.explosionPrefabs.Length > 0 AndAlso CupheadLevelCamera.Current.ContainsPoint(MyBase.transform.position, AbstractPlatformingLevelEnemy.CAMERA_DEATH_PADDING) Then
			Me.explosionPrefabs.RandomChoice().Create(MyBase.GetComponent(Of Collider2D)().bounds.center)
		End If
	End Sub

	' Token: 0x060031A9 RID: 12713 RVA: 0x001CF0B0 File Offset: 0x001CD4B0
	Public Overrides Sub OnParry(player As AbstractPlayerController)
		MyBase.OnParry(player)
		If Me.parryEffectPrefab IsNot Nothing Then
			Me.parryEffectPrefab.Create(MyBase.GetComponent(Of Collider2D)().bounds.center)
		End If
		player.stats.OnParry(1F, True)
		Me.Die()
	End Sub

	' Token: 0x060031AA RID: 12714 RVA: 0x001CF10C File Offset: 0x001CD50C
	Protected Overridable Iterator Function idle_audio_delayer_cr(key As String, delayMin As Single, delayMax As Single) As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 0.2F)
		While True
			If CupheadLevelCamera.Current.ContainsPoint(MyBase.transform.position, AbstractPlatformingLevelEnemy.CAMERA_DEATH_PADDING) Then
				Dim delay As Single = Global.UnityEngine.Random.Range(delayMin, delayMax)
				Yield CupheadTime.WaitForSeconds(Me, delay)
				Yield Nothing
				If Me.IdleSounds Then
					AudioManager.Play(key)
					While AudioManager.CheckIfPlaying(key)
						Yield Nothing
					End While
				End If
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060031AB RID: 12715 RVA: 0x001CF13C File Offset: 0x001CD53C
	Public Sub StartFromCustom()
		If Not Me._started Then
			Me.StartWithCondition(AbstractPlatformingLevelEnemy.StartCondition.Custom)
		End If
	End Sub

	' Token: 0x060031AC RID: 12716 RVA: 0x001CF150 File Offset: 0x001CD550
	Public Sub ResetStartingCondition()
		Me._started = False
	End Sub

	' Token: 0x060031AD RID: 12717
	Protected MustOverride Sub OnStart()

	' Token: 0x060031AE RID: 12718 RVA: 0x001CF159 File Offset: 0x001CD559
	Private Sub OnLevelStart()
		Me.StartWithCondition(AbstractPlatformingLevelEnemy.StartCondition.LevelStart)
	End Sub

	' Token: 0x060031AF RID: 12719 RVA: 0x001CF162 File Offset: 0x001CD562
	Private Sub StartWithCondition(condition As AbstractPlatformingLevelEnemy.StartCondition)
		If Me.Dead OrElse condition <> Me._startCondition OrElse Me._started Then
			Return
		End If
		Me._started = True
		MyBase.StartCoroutine(Me.startWithCondition_cr())
	End Sub

	' Token: 0x060031B0 RID: 12720 RVA: 0x001CF19C File Offset: 0x001CD59C
	Private Iterator Function startWithCondition_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, Me._startDelay)
		Me.OnStart()
		Return
	End Function

	' Token: 0x060031B1 RID: 12721 RVA: 0x001CF1B7 File Offset: 0x001CD5B7
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		Me.DrawGizmos(0.2F)
	End Sub

	' Token: 0x060031B2 RID: 12722 RVA: 0x001CF1CA File Offset: 0x001CD5CA
	Protected Overrides Sub OnDrawGizmosSelected()
		MyBase.OnDrawGizmosSelected()
		Me.DrawGizmos(1F)
	End Sub

	' Token: 0x060031B3 RID: 12723 RVA: 0x001CF1E0 File Offset: 0x001CD5E0
	Private Sub DrawGizmos(a As Single)
		If Me._startCondition = AbstractPlatformingLevelEnemy.StartCondition.TriggerVolume Then
			Gizmos.color = New Color(0F, 1F, 0F, a)
			Gizmos.DrawWireCube(Me._triggerPosition, Me._triggerSize)
		End If
	End Sub

	' Token: 0x04003A05 RID: 14853
	Public Shared CAMERA_DEATH_PADDING As Vector2 = New Vector2(100F, 100F)

	' Token: 0x04003A06 RID: 14854
	<SerializeField()>
	Private _id As EnemyID

	' Token: 0x04003A07 RID: 14855
	<SerializeField()>
	Protected _startCondition As AbstractPlatformingLevelEnemy.StartCondition

	' Token: 0x04003A08 RID: 14856
	<SerializeField()>
	Private _startDelay As Single

	' Token: 0x04003A09 RID: 14857
	<SerializeField()>
	Protected _triggerPosition As Vector2 = Vector2.zero

	' Token: 0x04003A0A RID: 14858
	<SerializeField()>
	Protected _triggerSize As Vector2 = Vector2.one * 100F

	' Token: 0x04003A0B RID: 14859
	<SerializeField()>
	Private explosionPrefabs As PlatformingLevelGenericExplosion()

	' Token: 0x04003A0C RID: 14860
	<SerializeField()>
	Private parryEffectPrefab As Effect

	' Token: 0x04003A0D RID: 14861
	Private _properties As EnemyProperties

	' Token: 0x04003A13 RID: 14867
	Protected IdleSounds As Boolean = True

	' Token: 0x0200085B RID: 2139
	Public Enum StartCondition
		' Token: 0x04003A15 RID: 14869
		LevelStart
		' Token: 0x04003A16 RID: 14870
		TriggerVolume
		' Token: 0x04003A17 RID: 14871
		Instant
		' Token: 0x04003A18 RID: 14872
		Custom
	End Enum
End Class
