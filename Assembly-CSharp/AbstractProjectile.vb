Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports UnityEngine

' Token: 0x02000AE3 RID: 2787
Public MustInherit Class AbstractProjectile
	Inherits AbstractCollidableObject

	' Token: 0x170005F4 RID: 1524
	' (get) Token: 0x0600433F RID: 17215 RVA: 0x000ACF20 File Offset: 0x000AB320
	Public ReadOnly Property CanParry As Boolean
		Get
			Return Me._canParry
		End Get
	End Property

	' Token: 0x170005F5 RID: 1525
	' (get) Token: 0x06004340 RID: 17216 RVA: 0x000ACF28 File Offset: 0x000AB328
	Public ReadOnly Property CountParryTowardsScore As Boolean
		Get
			Return Me._countParryTowardsScore
		End Get
	End Property

	' Token: 0x170005F6 RID: 1526
	' (get) Token: 0x06004341 RID: 17217 RVA: 0x000ACF30 File Offset: 0x000AB330
	' (set) Token: 0x06004342 RID: 17218 RVA: 0x000ACF38 File Offset: 0x000AB338
	Protected Private Property distance As Single

	' Token: 0x170005F7 RID: 1527
	' (get) Token: 0x06004343 RID: 17219 RVA: 0x000ACF41 File Offset: 0x000AB341
	' (set) Token: 0x06004344 RID: 17220 RVA: 0x000ACF49 File Offset: 0x000AB349
	Protected Private Property lifetime As Single

	' Token: 0x170005F8 RID: 1528
	' (get) Token: 0x06004345 RID: 17221 RVA: 0x000ACF52 File Offset: 0x000AB352
	' (set) Token: 0x06004346 RID: 17222 RVA: 0x000ACF5A File Offset: 0x000AB35A
	Public Property dead As Boolean

	' Token: 0x170005F9 RID: 1529
	' (get) Token: 0x06004347 RID: 17223 RVA: 0x000ACF63 File Offset: 0x000AB363
	' (set) Token: 0x06004348 RID: 17224 RVA: 0x000ACF6B File Offset: 0x000AB36B
	Public Property StoneTime As Single

	' Token: 0x170005FA RID: 1530
	' (get) Token: 0x06004349 RID: 17225 RVA: 0x000ACF74 File Offset: 0x000AB374
	Public Overridable ReadOnly Property ParryMeterMultiplier As Single
		Get
			Return 1F
		End Get
	End Property

	' Token: 0x170005FB RID: 1531
	' (get) Token: 0x0600434A RID: 17226 RVA: 0x000ACF7B File Offset: 0x000AB37B
	' (set) Token: 0x0600434B RID: 17227 RVA: 0x000ACF83 File Offset: 0x000AB383
	Public Property DamageSource As DamageDealer.DamageSource
		Get
			Return Me.damageSource
		End Get
		Set(value As DamageDealer.DamageSource)
			Me.damageSource = value
		End Set
	End Property

	' Token: 0x170005FC RID: 1532
	' (get) Token: 0x0600434C RID: 17228 RVA: 0x000ACF8C File Offset: 0x000AB38C
	Public ReadOnly Property DamageMultiplier As Single
		Get
			Dim num As Single = PlayerManager.DamageMultiplier
			If MyBase.tag = "PlayerProjectile" Then
				If PlayerManager.GetPlayer(Me.PlayerId).stats.Loadout.charm = Charm.charm_health_up_1 Then
					num *= 1F - WeaponProperties.CharmHealthUpOne.weaponDebuff
				ElseIf PlayerManager.GetPlayer(Me.PlayerId).stats.Loadout.charm = Charm.charm_health_up_2 Then
					num *= 1F - WeaponProperties.CharmHealthUpTwo.weaponDebuff
				ElseIf PlayerManager.GetPlayer(Me.PlayerId).stats.Loadout.charm = Charm.charm_EX AndAlso Level.Current.playerMode = PlayerMode.Plane AndAlso TypeOf Me Is PlaneWeaponPeashotExProjectile Then
					num *= 1F - WeaponProperties.CharmEXCharm.planePeashotEXDebuff
				End If
			End If
			Return num
		End Get
	End Property

	' Token: 0x170005FD RID: 1533
	' (get) Token: 0x0600434D RID: 17229 RVA: 0x000AD070 File Offset: 0x000AB470
	Protected Overridable ReadOnly Property DestroyLifetime As Single
		Get
			Return 20F
		End Get
	End Property

	' Token: 0x170005FE RID: 1534
	' (get) Token: 0x0600434E RID: 17230 RVA: 0x000AD077 File Offset: 0x000AB477
	Protected Overridable ReadOnly Property DestroyedAfterLeavingScreen As Boolean
		Get
			Return False
		End Get
	End Property

	' Token: 0x170005FF RID: 1535
	' (get) Token: 0x0600434F RID: 17231 RVA: 0x000AD07A File Offset: 0x000AB47A
	Protected Overridable ReadOnly Property SafeTime As Single
		Get
			Return 0.005F
		End Get
	End Property

	' Token: 0x17000600 RID: 1536
	' (get) Token: 0x06004350 RID: 17232 RVA: 0x000AD081 File Offset: 0x000AB481
	Protected Overridable ReadOnly Property PlayerSafeTime As Single
		Get
			Return 0F
		End Get
	End Property

	' Token: 0x17000601 RID: 1537
	' (get) Token: 0x06004351 RID: 17233 RVA: 0x000AD088 File Offset: 0x000AB488
	Protected Overridable ReadOnly Property EnemySafeTime As Single
		Get
			Return 0F
		End Get
	End Property

	' Token: 0x140000BC RID: 188
	' (add) Token: 0x06004352 RID: 17234 RVA: 0x000AD090 File Offset: 0x000AB490
	' (remove) Token: 0x06004353 RID: 17235 RVA: 0x000AD0C8 File Offset: 0x000AB4C8
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnDealDamageEvent As DamageDealer.OnDealDamageHandler

	' Token: 0x140000BD RID: 189
	' (add) Token: 0x06004354 RID: 17236 RVA: 0x000AD100 File Offset: 0x000AB500
	' (remove) Token: 0x06004355 RID: 17237 RVA: 0x000AD138 File Offset: 0x000AB538
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnDie As Action(Of AbstractProjectile)

	' Token: 0x06004356 RID: 17238 RVA: 0x000AD170 File Offset: 0x000AB570
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.distance = 0F
		Me.lifetime = 0F
		Me.StoneTime = -1F
		If MyBase.CompareTag("PlayerProjectile") OrElse Not MyBase.CompareTag("EnemyProjectile") Then
		End If
		If MyBase.gameObject.layer <> 12 Then
			MyBase.gameObject.layer = 12
		End If
		Me.RandomizeVariant()
		If Level.Current IsNot Nothing AndAlso Level.Current.CurrentScene = Scenes.scene_level_airplane Then
			Me._setYPadding = -600F
		End If
	End Sub

	' Token: 0x06004357 RID: 17239 RVA: 0x000AD218 File Offset: 0x000AB618
	Protected Overridable Sub Start()
		Me.damageDealer = New DamageDealer(Me)
		AddHandler Me.damageDealer.OnDealDamage, AddressOf Me.OnDealDamage
		Me.damageDealer.SetStoneTime(Me.StoneTime)
		Me.damageDealer.PlayerId = Me.PlayerId
		If Me.tracker IsNot Nothing Then
			Me.tracker.Add(Me.damageDealer)
		End If
	End Sub

	' Token: 0x06004358 RID: 17240 RVA: 0x000AD288 File Offset: 0x000AB688
	Protected Overridable Sub Update()
		Dim position As Vector3 = MyBase.transform.position
		If Me.lifetime = 0F Then
			Dim vector As Vector3 = position
			Dim vector2 As Vector3 = vector
			Me.startPosition = vector
			Me.lastPosition = vector2
		End If
		If Me.DestroyDistance > 0F AndAlso Vector3.Distance(Me.startPosition, position) >= Me.DestroyDistance Then
			Me.OnDieDistance()
		End If
		Me.distance += Vector3.Distance(Me.lastPosition, position)
		Me.lastPosition = position
		If Me.DestroyLifetime > 0F AndAlso Me.lifetime >= Me.DestroyLifetime Then
			Me.OnDieLifetime()
		End If
		Me.lifetime += Time.deltaTime
		If Me.DestroyedAfterLeavingScreen Then
			Dim flag As Boolean = CupheadLevelCamera.Current.ContainsPoint(position, New Vector2(150F, Me._setYPadding))
			If Me.hasBeenRendered AndAlso Not flag Then
				Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
			End If
			If Not Me.hasBeenRendered Then
				Me.hasBeenRendered = flag
			End If
		End If
	End Sub

	' Token: 0x06004359 RID: 17241 RVA: 0x000AD3A1 File Offset: 0x000AB7A1
	Protected Sub ResetLifetime()
		Me.lifetime = 0F
	End Sub

	' Token: 0x0600435A RID: 17242 RVA: 0x000AD3AE File Offset: 0x000AB7AE
	Protected Sub ResetDistance()
		Me.distance = 0F
	End Sub

	' Token: 0x0600435B RID: 17243 RVA: 0x000AD3BB File Offset: 0x000AB7BB
	Protected Overrides Sub checkCollision(col As Collider2D, phase As CollisionPhase)
		If Me.lifetime < Me.SafeTime Then
			Return
		End If
		MyBase.checkCollision(col, phase)
	End Sub

	' Token: 0x17000602 RID: 1538
	' (get) Token: 0x0600435C RID: 17244 RVA: 0x000AD3D7 File Offset: 0x000AB7D7
	Protected Overrides ReadOnly Property allowCollisionPlayer As Boolean
		Get
			Return Me.lifetime > Me.PlayerSafeTime
		End Get
	End Property

	' Token: 0x17000603 RID: 1539
	' (get) Token: 0x0600435D RID: 17245 RVA: 0x000AD3E7 File Offset: 0x000AB7E7
	Protected Overrides ReadOnly Property allowCollisionEnemy As Boolean
		Get
			Return Me.lifetime > Me.EnemySafeTime
		End Get
	End Property

	' Token: 0x0600435E RID: 17246 RVA: 0x000AD3F8 File Offset: 0x000AB7F8
	Public Overridable Function Create() As AbstractProjectile
		Dim gameObject As GameObject = Global.UnityEngine.[Object].Instantiate(Of GameObject)(MyBase.gameObject)
		Return gameObject.GetComponent(Of AbstractProjectile)()
	End Function

	' Token: 0x0600435F RID: 17247 RVA: 0x000AD41C File Offset: 0x000AB81C
	Public Overridable Function Create(position As Vector2) As AbstractProjectile
		Dim abstractProjectile As AbstractProjectile = Me.Create()
		abstractProjectile.transform.position = position
		Return abstractProjectile
	End Function

	' Token: 0x06004360 RID: 17248 RVA: 0x000AD444 File Offset: 0x000AB844
	Public Overridable Function Create(position As Vector2, rotation As Single) As AbstractProjectile
		Dim abstractProjectile As AbstractProjectile = Me.Create(position)
		abstractProjectile.transform.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(rotation))
		Return abstractProjectile
	End Function

	' Token: 0x06004361 RID: 17249 RVA: 0x000AD480 File Offset: 0x000AB880
	Public Overridable Function Create(position As Vector2, rotation As Single, scale As Vector2) As AbstractProjectile
		Dim abstractProjectile As AbstractProjectile = Me.Create(position, rotation)
		abstractProjectile.transform.SetScale(New Single?(scale.x), New Single?(scale.y), New Single?(1F))
		Return abstractProjectile
	End Function

	' Token: 0x06004362 RID: 17250 RVA: 0x000AD4C4 File Offset: 0x000AB8C4
	Protected Overridable Sub OnDealDamage(damage As Single, receiver As DamageReceiver, damageDealer As DamageDealer)
		If Me.OnDealDamageEvent IsNot Nothing Then
			Me.OnDealDamageEvent(damage, receiver, damageDealer)
		End If
	End Sub

	' Token: 0x06004363 RID: 17251 RVA: 0x000AD4DF File Offset: 0x000AB8DF
	Public Function GetDamagesType(type As DamageReceiver.Type) As Boolean
		Return Me.DamagesType.[GetType](type)
	End Function

	' Token: 0x06004364 RID: 17252 RVA: 0x000AD4ED File Offset: 0x000AB8ED
	Public Overridable Sub SetParryable(parryable As Boolean)
		Me._canParry = parryable
		Me.SetBool(AbstractProjectile.Parry, parryable)
	End Sub

	' Token: 0x06004365 RID: 17253 RVA: 0x000AD502 File Offset: 0x000AB902
	Public Sub SetStoneTime(stoneTime As Single)
		Me.StoneTime = stoneTime
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.SetStoneTime(stoneTime)
		End If
	End Sub

	' Token: 0x06004366 RID: 17254 RVA: 0x000AD522 File Offset: 0x000AB922
	Protected Overrides Sub OnCollisionWalls(hit As GameObject, phase As CollisionPhase)
		If Me.CollisionDeath.Walls Then
			Me.OnCollisionDie(hit, phase)
		End If
	End Sub

	' Token: 0x06004367 RID: 17255 RVA: 0x000AD53C File Offset: 0x000AB93C
	Protected Overrides Sub OnCollisionCeiling(hit As GameObject, phase As CollisionPhase)
		If Me.CollisionDeath.Ceiling Then
			Me.OnCollisionDie(hit, phase)
		End If
	End Sub

	' Token: 0x06004368 RID: 17256 RVA: 0x000AD556 File Offset: 0x000AB956
	Protected Overrides Sub OnCollisionGround(hit As GameObject, phase As CollisionPhase)
		If Me.CollisionDeath.Ground Then
			Me.OnCollisionDie(hit, phase)
		End If
	End Sub

	' Token: 0x06004369 RID: 17257 RVA: 0x000AD570 File Offset: 0x000AB970
	Protected Overrides Sub OnCollisionEnemy(hit As GameObject, phase As CollisionPhase)
		Me.missed = False
		If Me.CollisionDeath.Enemies Then
			Me.OnCollisionDie(hit, phase)
		End If
	End Sub

	' Token: 0x0600436A RID: 17258 RVA: 0x000AD591 File Offset: 0x000AB991
	Protected Overrides Sub OnCollisionEnemyProjectile(hit As GameObject, phase As CollisionPhase)
		If Me.CollisionDeath.EnemyProjectiles Then
			Me.OnCollisionDie(hit, phase)
		End If
	End Sub

	' Token: 0x0600436B RID: 17259 RVA: 0x000AD5AB File Offset: 0x000AB9AB
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If Me.CollisionDeath.Player Then
			Me.OnCollisionDie(hit, phase)
		End If
	End Sub

	' Token: 0x0600436C RID: 17260 RVA: 0x000AD5C5 File Offset: 0x000AB9C5
	Protected Overrides Sub OnCollisionPlayerProjectile(hit As GameObject, phase As CollisionPhase)
		If Me.CollisionDeath.PlayerProjectiles Then
			Me.OnCollisionDie(hit, phase)
		End If
	End Sub

	' Token: 0x0600436D RID: 17261 RVA: 0x000AD5DF File Offset: 0x000AB9DF
	Protected Overrides Sub OnCollisionOther(hit As GameObject, phase As CollisionPhase)
		If Me.CollisionDeath.Other Then
			Me.OnCollisionDie(hit, phase)
		End If
	End Sub

	' Token: 0x0600436E RID: 17262 RVA: 0x000AD5F9 File Offset: 0x000AB9F9
	Public Overridable Sub OnCollisionWideShotEX(hit As GameObject, phase As CollisionPhase)
		Me.OnCollisionDie(hit, phase)
	End Sub

	' Token: 0x0600436F RID: 17263 RVA: 0x000AD603 File Offset: 0x000ABA03
	Protected Overridable Sub OnCollisionDie(hit As GameObject, phase As CollisionPhase)
		If Not Me.dead Then
			Me.Die()
		End If
	End Sub

	' Token: 0x06004370 RID: 17264 RVA: 0x000AD616 File Offset: 0x000ABA16
	Protected Overridable Sub OnDieAnimationComplete()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06004371 RID: 17265 RVA: 0x000AD623 File Offset: 0x000ABA23
	Public Overridable Sub OnParry(player As AbstractPlayerController)
		If Me.CanParry Then
			Me.OnParryDie()
		End If
	End Sub

	' Token: 0x06004372 RID: 17266 RVA: 0x000AD636 File Offset: 0x000ABA36
	Public Overridable Sub OnParryDie()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Me.Die()
	End Sub

	' Token: 0x06004373 RID: 17267 RVA: 0x000AD649 File Offset: 0x000ABA49
	Public Overrides Sub OnLevelEnd()
		MyBase.OnLevelEnd()
		Me.Die()
	End Sub

	' Token: 0x06004374 RID: 17268 RVA: 0x000AD658 File Offset: 0x000ABA58
	Protected Overridable Sub Die()
		Me.dead = True
		If MyBase.GetComponent(Of Collider2D)() IsNot Nothing Then
			MyBase.GetComponent(Of Collider2D)().enabled = False
		End If
		Me.RandomizeVariant()
		Me.SetTrigger(AbstractProjectile.OnDeathTrigger)
		If Me.OnDie IsNot Nothing Then
			Me.OnDie(Me)
		End If
	End Sub

	' Token: 0x06004375 RID: 17269 RVA: 0x000AD6B1 File Offset: 0x000ABAB1
	Protected Overridable Sub OnDieDistance()
		If Me.DestroyDistanceAnimated Then
			Me.Die()
		Else
			Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		End If
	End Sub

	' Token: 0x06004376 RID: 17270 RVA: 0x000AD6D4 File Offset: 0x000ABAD4
	Protected Overridable Sub OnDieLifetime()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06004377 RID: 17271 RVA: 0x000AD6E1 File Offset: 0x000ABAE1
	Protected Overridable Sub SetTrigger(trigger As String)
		MyBase.animator.SetTrigger(trigger)
	End Sub

	' Token: 0x06004378 RID: 17272 RVA: 0x000AD6EF File Offset: 0x000ABAEF
	Protected Overridable Sub SetInt([integer] As String, i As Integer)
		MyBase.animator.SetInteger([integer], i)
	End Sub

	' Token: 0x06004379 RID: 17273 RVA: 0x000AD6FE File Offset: 0x000ABAFE
	Protected Overridable Sub SetBool([boolean] As String, b As Boolean)
		MyBase.animator.SetBool([boolean], b)
	End Sub

	' Token: 0x0600437A RID: 17274 RVA: 0x000AD70D File Offset: 0x000ABB0D
	Protected Overridable Function GetVariants() As Integer
		Return MyBase.animator.GetInteger(AbstractProjectile.MaxVariants)
	End Function

	' Token: 0x0600437B RID: 17275 RVA: 0x000AD720 File Offset: 0x000ABB20
	Protected Overridable Sub RandomizeVariant()
		Dim num As Integer = Global.UnityEngine.Random.Range(0, Me.GetVariants())
		Me.SetInt(AbstractProjectile.[Variant], num)
	End Sub

	' Token: 0x0600437C RID: 17276 RVA: 0x000AD746 File Offset: 0x000ABB46
	Public Sub AddFiringHitbox(hitbox As LevelPlayerWeaponFiringHitbox)
		Me.firingHitbox = hitbox
		MyBase.RegisterCollisionChild(hitbox)
		MyBase.GetComponent(Of Collider2D)().enabled = False
	End Sub

	' Token: 0x0600437D RID: 17277 RVA: 0x000AD764 File Offset: 0x000ABB64
	Protected Overridable Sub FixedUpdate()
		If Me.firingHitbox IsNot Nothing Then
			If Me.firstUpdate Then
				Me.firstUpdate = False
			Else
				If Not Me.dead Then
					MyBase.GetComponent(Of Collider2D)().enabled = True
				End If
				Global.UnityEngine.[Object].Destroy(Me.firingHitbox.gameObject)
				Me.firingHitbox = Nothing
			End If
		End If
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.FixedUpdate()
		End If
	End Sub

	' Token: 0x0600437E RID: 17278 RVA: 0x000AD7DD File Offset: 0x000ABBDD
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		If Me.firingHitbox IsNot Nothing Then
			Global.UnityEngine.[Object].Destroy(Me.firingHitbox.gameObject)
		End If
	End Sub

	' Token: 0x0600437F RID: 17279 RVA: 0x000AD806 File Offset: 0x000ABC06
	Public Overridable Sub AddToMeterScoreTracker(tracker As MeterScoreTracker)
		Me.tracker = tracker
		If Me.damageDealer IsNot Nothing Then
			tracker.Add(Me.damageDealer)
		End If
	End Sub

	' Token: 0x06004380 RID: 17280 RVA: 0x000AD828 File Offset: 0x000ABC28
	Public Shared Function FindOverlapScreenDamageReceivers() As IEnumerable(Of DamageReceiver)
		AbstractProjectile.DamageReceiverComponentBuffer.Clear()
		AbstractProjectile.DamageReceiverSearchSet.Clear()
		Dim vector As Vector2 = New Vector2(100F, 100F)
		Dim rect As Rect = CupheadLevelCamera.Current.CalculateContainsBounds(vector)
		Dim num As Integer = Physics2D.OverlapBoxNonAlloc(rect.center, rect.size, 0F, AbstractProjectile.ColliderBuffer)
		For i As Integer = 0 To num - 1
			AbstractProjectile.DamageReceiverComponentBuffer.Clear()
			Dim collider2D As Collider2D = AbstractProjectile.ColliderBuffer(i)
			collider2D.GetComponentsInParent(Of DamageReceiver)(True, AbstractProjectile.DamageReceiverComponentBuffer)
			AbstractProjectile.DamageReceiverSearchSet.UnionWith(AbstractProjectile.DamageReceiverComponentBuffer)
		Next
		Return AbstractProjectile.DamageReceiverSearchSet
	End Function

	' Token: 0x0400492B RID: 18731
	Protected Shared [Variant] As String = "Variant"

	' Token: 0x0400492C RID: 18732
	Protected Shared MaxVariants As String = "MaxVariants"

	' Token: 0x0400492D RID: 18733
	Protected Shared OnDeathTrigger As String = "OnDeath"

	' Token: 0x0400492E RID: 18734
	Protected Shared Parry As String = "Parry"

	' Token: 0x0400492F RID: 18735
	Private startPosition As Vector3

	' Token: 0x04004930 RID: 18736
	Private lastPosition As Vector3

	' Token: 0x04004931 RID: 18737
	Protected tracker As MeterScoreTracker

	' Token: 0x04004932 RID: 18738
	Private hasBeenRendered As Boolean

	' Token: 0x04004933 RID: 18739
	<SerializeField()>
	Private _canParry As Boolean

	' Token: 0x04004934 RID: 18740
	Protected _countParryTowardsScore As Boolean = True

	' Token: 0x04004938 RID: 18744
	Protected missed As Boolean = True

	' Token: 0x04004939 RID: 18745
	Protected damageDealer As DamageDealer

	' Token: 0x0400493B RID: 18747
	Private _setYPadding As Single = 150F

	' Token: 0x0400493C RID: 18748
	Private damageSource As DamageDealer.DamageSource

	' Token: 0x0400493D RID: 18749
	Public Damage As Single = 1F

	' Token: 0x0400493E RID: 18750
	Public DamageRate As Single

	' Token: 0x0400493F RID: 18751
	Public PlayerId As PlayerId = PlayerId.None

	' Token: 0x04004940 RID: 18752
	Public DamagesType As DamageDealer.DamageTypesManager

	' Token: 0x04004941 RID: 18753
	Public CollisionDeath As AbstractProjectile.CollisionProperties

	' Token: 0x04004942 RID: 18754
	<NonSerialized()>
	Public DestroyDistance As Single = 3000F

	' Token: 0x04004943 RID: 18755
	<NonSerialized()>
	Public DestroyDistanceAnimated As Boolean

	' Token: 0x04004946 RID: 18758
	Private firingHitbox As LevelPlayerWeaponFiringHitbox

	' Token: 0x04004947 RID: 18759
	Private firstUpdate As Boolean = True

	' Token: 0x04004948 RID: 18760
	Private Shared ColliderBuffer As Collider2D() = New Collider2D(499) {}

	' Token: 0x04004949 RID: 18761
	Private Shared DamageReceiverSearchSet As HashSet(Of DamageReceiver) = New HashSet(Of DamageReceiver)()

	' Token: 0x0400494A RID: 18762
	Private Shared DamageReceiverComponentBuffer As List(Of DamageReceiver) = New List(Of DamageReceiver)()

	' Token: 0x02000AE4 RID: 2788
	<Serializable()>
	Public Class CollisionProperties
		' Token: 0x06004383 RID: 17283 RVA: 0x000AD941 File Offset: 0x000ABD41
		Public Function Copy() As AbstractProjectile.CollisionProperties
			Return TryCast(MyBase.MemberwiseClone(), AbstractProjectile.CollisionProperties)
		End Function

		' Token: 0x06004384 RID: 17284 RVA: 0x000AD94E File Offset: 0x000ABD4E
		Public Sub SetAll(b As Boolean)
			Me.Walls = b
			Me.Ceiling = b
			Me.Ground = b
			Me.Enemies = b
			Me.EnemyProjectiles = b
			Me.Player = b
			Me.PlayerProjectiles = b
			Me.Other = b
		End Sub

		' Token: 0x06004385 RID: 17285 RVA: 0x000AD988 File Offset: 0x000ABD88
		Public Sub All()
			Me.SetAll(True)
		End Sub

		' Token: 0x06004386 RID: 17286 RVA: 0x000AD991 File Offset: 0x000ABD91
		Public Sub None()
			Me.SetAll(False)
		End Sub

		' Token: 0x06004387 RID: 17287 RVA: 0x000AD99A File Offset: 0x000ABD9A
		Public Sub OnlyPlayer()
			Me.SetAll(False)
			Me.Player = True
		End Sub

		' Token: 0x06004388 RID: 17288 RVA: 0x000AD9AA File Offset: 0x000ABDAA
		Public Sub OnlyEnemies()
			Me.SetAll(False)
			Me.Player = True
		End Sub

		' Token: 0x06004389 RID: 17289 RVA: 0x000AD9BA File Offset: 0x000ABDBA
		Public Sub OnlyBounds()
			Me.SetAll(False)
			Me.SetBounds(True)
		End Sub

		' Token: 0x0600438A RID: 17290 RVA: 0x000AD9CA File Offset: 0x000ABDCA
		Public Sub SetBounds(b As Boolean)
			Me.Walls = b
			Me.Ceiling = b
			Me.Ground = b
		End Sub

		' Token: 0x0600438B RID: 17291 RVA: 0x000AD9E1 File Offset: 0x000ABDE1
		Public Sub PlayerProjectileDefault()
			Me.SetAll(False)
			Me.SetBounds(True)
			Me.Enemies = True
			Me.Other = True
		End Sub

		' Token: 0x0400494B RID: 18763
		Public Walls As Boolean = True

		' Token: 0x0400494C RID: 18764
		Public Ceiling As Boolean = True

		' Token: 0x0400494D RID: 18765
		Public Ground As Boolean = True

		' Token: 0x0400494E RID: 18766
		Public Enemies As Boolean

		' Token: 0x0400494F RID: 18767
		Public EnemyProjectiles As Boolean

		' Token: 0x04004950 RID: 18768
		Public Player As Boolean

		' Token: 0x04004951 RID: 18769
		Public PlayerProjectiles As Boolean

		' Token: 0x04004952 RID: 18770
		Public Other As Boolean
	End Class
End Class
