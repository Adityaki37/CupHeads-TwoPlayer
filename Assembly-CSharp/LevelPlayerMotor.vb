Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Diagnostics
Imports UnityEngine

' Token: 0x02000A1F RID: 2591
Public Class LevelPlayerMotor
	Inherits AbstractLevelPlayerComponent

	' Token: 0x1700053A RID: 1338
	' (get) Token: 0x06003D73 RID: 15731 RVA: 0x0021F0D0 File Offset: 0x0021D4D0
	' (set) Token: 0x06003D74 RID: 15732 RVA: 0x0021F0D8 File Offset: 0x0021D4D8
	Public Property LookDirection As Trilean2

	' Token: 0x1700053B RID: 1339
	' (get) Token: 0x06003D75 RID: 15733 RVA: 0x0021F0E1 File Offset: 0x0021D4E1
	' (set) Token: 0x06003D76 RID: 15734 RVA: 0x0021F0E9 File Offset: 0x0021D4E9
	Public Property TrueLookDirection As Trilean2

	' Token: 0x1700053C RID: 1340
	' (get) Token: 0x06003D77 RID: 15735 RVA: 0x0021F0F2 File Offset: 0x0021D4F2
	' (set) Token: 0x06003D78 RID: 15736 RVA: 0x0021F0FA File Offset: 0x0021D4FA
	Public Property MoveDirection As Trilean2

	' Token: 0x1700053D RID: 1341
	' (get) Token: 0x06003D79 RID: 15737 RVA: 0x0021F103 File Offset: 0x0021D503
	Public ReadOnly Property JumpState As LevelPlayerMotor.JumpManager.State
		Get
			Return Me.jumpManager.state
		End Get
	End Property

	' Token: 0x1700053E RID: 1342
	' (get) Token: 0x06003D7A RID: 15738 RVA: 0x0021F110 File Offset: 0x0021D510
	Public ReadOnly Property Dashing As Boolean
		Get
			Return Me.dashManager.IsDashing
		End Get
	End Property

	' Token: 0x1700053F RID: 1343
	' (get) Token: 0x06003D7B RID: 15739 RVA: 0x0021F11D File Offset: 0x0021D51D
	Public ReadOnly Property DashDirection As Integer
		Get
			Return Me.dashManager.direction
		End Get
	End Property

	' Token: 0x17000540 RID: 1344
	' (get) Token: 0x06003D7C RID: 15740 RVA: 0x0021F12A File Offset: 0x0021D52A
	Public ReadOnly Property DashState As LevelPlayerMotor.DashManager.State
		Get
			Return Me.dashManager.state
		End Get
	End Property

	' Token: 0x17000541 RID: 1345
	' (get) Token: 0x06003D7D RID: 15741 RVA: 0x0021F137 File Offset: 0x0021D537
	' (set) Token: 0x06003D7E RID: 15742 RVA: 0x0021F13F File Offset: 0x0021D53F
	Public Property Locked As Boolean

	' Token: 0x17000542 RID: 1346
	' (get) Token: 0x06003D7F RID: 15743 RVA: 0x0021F148 File Offset: 0x0021D548
	' (set) Token: 0x06003D80 RID: 15744 RVA: 0x0021F150 File Offset: 0x0021D550
	Public Property Grounded As Boolean

	' Token: 0x17000543 RID: 1347
	' (get) Token: 0x06003D81 RID: 15745 RVA: 0x0021F159 File Offset: 0x0021D559
	' (set) Token: 0x06003D82 RID: 15746 RVA: 0x0021F161 File Offset: 0x0021D561
	Public Property Parrying As Boolean

	' Token: 0x17000544 RID: 1348
	' (get) Token: 0x06003D83 RID: 15747 RVA: 0x0021F16C File Offset: 0x0021D56C
	Public ReadOnly Property Ducking As Boolean
		Get
			Return Me.LookDirection.y < 0 AndAlso Not Me.Locked AndAlso Me.Grounded
		End Get
	End Property

	' Token: 0x17000545 RID: 1349
	' (get) Token: 0x06003D84 RID: 15748 RVA: 0x0021F1A6 File Offset: 0x0021D5A6
	Public ReadOnly Property IsHit As Boolean
		Get
			Return Me.hitManager.state = LevelPlayerMotor.HitManager.State.Hit
		End Get
	End Property

	' Token: 0x17000546 RID: 1350
	' (get) Token: 0x06003D85 RID: 15749 RVA: 0x0021F1B6 File Offset: 0x0021D5B6
	Public ReadOnly Property IsUsingSuperOrEx As Boolean
		Get
			Return Me.superManager.state = LevelPlayerMotor.SuperManager.State.Super OrElse Me.superManager.state = LevelPlayerMotor.SuperManager.State.Ex
		End Get
	End Property

	' Token: 0x17000547 RID: 1351
	' (get) Token: 0x06003D86 RID: 15750 RVA: 0x0021F1DA File Offset: 0x0021D5DA
	' (set) Token: 0x06003D87 RID: 15751 RVA: 0x0021F1E2 File Offset: 0x0021D5E2
	Public Property GravityReversed As Boolean

	' Token: 0x17000548 RID: 1352
	' (get) Token: 0x06003D88 RID: 15752 RVA: 0x0021F1EB File Offset: 0x0021D5EB
	' (set) Token: 0x06003D89 RID: 15753 RVA: 0x0021F1F3 File Offset: 0x0021D5F3
	Public Property ChaliceDoubleJumped As Boolean

	' Token: 0x17000549 RID: 1353
	' (get) Token: 0x06003D8A RID: 15754 RVA: 0x0021F1FC File Offset: 0x0021D5FC
	' (set) Token: 0x06003D8B RID: 15755 RVA: 0x0021F204 File Offset: 0x0021D604
	Public Property ChaliceDuckDashed As Boolean

	' Token: 0x1700054A RID: 1354
	' (get) Token: 0x06003D8C RID: 15756 RVA: 0x0021F20D File Offset: 0x0021D60D
	Public ReadOnly Property GravityReversalMultiplier As Single
		Get
			Return CSng(If((Not Me.GravityReversed), 1, (-1)))
		End Get
	End Property

	' Token: 0x1400008B RID: 139
	' (add) Token: 0x06003D8D RID: 15757 RVA: 0x0021F224 File Offset: 0x0021D624
	' (remove) Token: 0x06003D8E RID: 15758 RVA: 0x0021F25C File Offset: 0x0021D65C
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnGroundedEvent As Action

	' Token: 0x1400008C RID: 140
	' (add) Token: 0x06003D8F RID: 15759 RVA: 0x0021F294 File Offset: 0x0021D694
	' (remove) Token: 0x06003D90 RID: 15760 RVA: 0x0021F2CC File Offset: 0x0021D6CC
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnJumpEvent As Action

	' Token: 0x1400008D RID: 141
	' (add) Token: 0x06003D91 RID: 15761 RVA: 0x0021F304 File Offset: 0x0021D704
	' (remove) Token: 0x06003D92 RID: 15762 RVA: 0x0021F33C File Offset: 0x0021D73C
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnDoubleJumpEvent As Action

	' Token: 0x1400008E RID: 142
	' (add) Token: 0x06003D93 RID: 15763 RVA: 0x0021F374 File Offset: 0x0021D774
	' (remove) Token: 0x06003D94 RID: 15764 RVA: 0x0021F3AC File Offset: 0x0021D7AC
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnParryEvent As Action

	' Token: 0x1400008F RID: 143
	' (add) Token: 0x06003D95 RID: 15765 RVA: 0x0021F3E4 File Offset: 0x0021D7E4
	' (remove) Token: 0x06003D96 RID: 15766 RVA: 0x0021F41C File Offset: 0x0021D81C
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnParrySuccess As Action

	' Token: 0x14000090 RID: 144
	' (add) Token: 0x06003D97 RID: 15767 RVA: 0x0021F454 File Offset: 0x0021D854
	' (remove) Token: 0x06003D98 RID: 15768 RVA: 0x0021F48C File Offset: 0x0021D88C
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnHitEvent As Action

	' Token: 0x14000091 RID: 145
	' (add) Token: 0x06003D99 RID: 15769 RVA: 0x0021F4C4 File Offset: 0x0021D8C4
	' (remove) Token: 0x06003D9A RID: 15770 RVA: 0x0021F4FC File Offset: 0x0021D8FC
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnDashStartEvent As Action

	' Token: 0x14000092 RID: 146
	' (add) Token: 0x06003D9B RID: 15771 RVA: 0x0021F534 File Offset: 0x0021D934
	' (remove) Token: 0x06003D9C RID: 15772 RVA: 0x0021F56C File Offset: 0x0021D96C
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnDashEndEvent As Action

	' Token: 0x1700054B RID: 1355
	' (get) Token: 0x06003D9D RID: 15773 RVA: 0x0021F5A2 File Offset: 0x0021D9A2
	' (set) Token: 0x06003D9E RID: 15774 RVA: 0x0021F5AA File Offset: 0x0021D9AA
	Public Property isFloating As Boolean

	' Token: 0x06003D9F RID: 15775 RVA: 0x0021F5B4 File Offset: 0x0021D9B4
	Protected Overrides Sub OnAwake()
		MyBase.OnAwake()
		Me.properties = New LevelPlayerMotor.Properties()
		Me.MoveDirection = New Trilean2(0, 0)
		Me.LookDirection = New Trilean2(1, 0)
		Me.TrueLookDirection = New Trilean2(1, 0)
		Me.velocityManager = New LevelPlayerMotor.VelocityManager(Me, Me.properties.maxSpeedY, Me.properties.yEase)
		Me.jumpManager = New LevelPlayerMotor.JumpManager()
		Me.dashManager = New LevelPlayerMotor.DashManager()
		Me.parryManager = New LevelPlayerMotor.ParryManager()
		Me.directionManager = New LevelPlayerMotor.DirectionManager()
		Me.platformManager = New LevelPlayerMotor.PlatformManager(Me)
		Me.hitManager = New LevelPlayerMotor.HitManager()
		Me.superManager = New LevelPlayerMotor.SuperManager()
		Me.boundsManager = New LevelPlayerMotor.BoundsManager(Me)
		AddHandler MyBase.player.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		Me.allowInput = True
		Me.allowFalling = True
		Me.allowJumping = True
		Me.forceLaunchUp = False
	End Sub

	' Token: 0x06003DA0 RID: 15776 RVA: 0x0021F6B0 File Offset: 0x0021DAB0
	Private Sub Start()
		AddHandler MyBase.player.weaponManager.OnExStart, AddressOf Me.StartEx
		AddHandler MyBase.player.weaponManager.OnSuperStart, AddressOf Me.StartSuper
		AddHandler MyBase.player.weaponManager.OnExFire, AddressOf Me.OnExFired
		AddHandler MyBase.player.weaponManager.OnSuperEnd, AddressOf Me.OnSuperEnd
		AddHandler MyBase.player.weaponManager.OnExEnd, AddressOf Me.ResetSuperAndEx
		AddHandler MyBase.player.weaponManager.OnSuperEnd, AddressOf Me.ResetSuperAndEx
		AddHandler MyBase.player.OnReviveEvent, AddressOf Me.OnRevive
		Me.parryController = MyBase.player.GetComponent(Of LevelPlayerParryController)()
		Me.jumpPower = If(MyBase.player.stats.isChalice, Me.properties.chaliceFirstJumpPower, Me.properties.jumpPower)
	End Sub

	' Token: 0x06003DA1 RID: 15777 RVA: 0x0021F7C4 File Offset: 0x0021DBC4
	Private Sub FixedUpdate()
		If MyBase.player.IsDead Then
			Return
		End If
		Me.HandleLooking()
		If MyBase.player.weaponManager.FreezePosition Then
			Return
		End If
		Me.HandleInput()
		If Me.allowFalling Then
			Me.HandleFalling()
		End If
		If Not Me.Grounded Then
			Me.jumpManager.timeInAir += CupheadTime.FixedDelta
			If Me.jumpManager.state = LevelPlayerMotor.JumpManager.State.Ready AndAlso Me.jumpManager.timeInAir > 0.0834F Then
				Me.jumpManager.state = LevelPlayerMotor.JumpManager.State.Used
			End If
		End If
		Me.Move()
		Me.HandleRaycasts()
		Dim vector As Vector2 = MyBase.transform.localPosition
		Dim vector2 As Vector2 = vector - If((Not Me.platformManager.OnPlatform AndAlso MyBase.player.stats.isChalice), Me.lastPosition, Me.lastPositionFixed)
		vector2.x = CSng(CInt(vector2.x))
		vector2.y = CSng(CInt(vector2.y))
		Me.MoveDirection = vector2
		Me.lastPositionFixed = New Vector2(vector.x, vector.y)
		Me.lastPosition = MyBase.transform.position
		Me.ClampToBounds()
	End Sub

	' Token: 0x06003DA2 RID: 15778 RVA: 0x0021F924 File Offset: 0x0021DD24
	Public Sub DisableInput()
		Me.allowInput = False
		Me.Locked = False
		Me.MoveDirection = New Trilean2(0, 0)
		Me.velocityManager.move = 0F
		Me.velocityManager.dash = 0F
		Me.velocityManager.verticalDash = 0F
	End Sub

	' Token: 0x06003DA3 RID: 15779 RVA: 0x0021F97C File Offset: 0x0021DD7C
	Public Sub EnableInput()
		Me.allowInput = True
	End Sub

	' Token: 0x06003DA4 RID: 15780 RVA: 0x0021F985 File Offset: 0x0021DD85
	Public Sub DisableJump()
		Me.allowJumping = False
	End Sub

	' Token: 0x06003DA5 RID: 15781 RVA: 0x0021F98E File Offset: 0x0021DD8E
	Public Sub EnableJump()
		Me.allowJumping = True
	End Sub

	' Token: 0x06003DA6 RID: 15782 RVA: 0x0021F998 File Offset: 0x0021DD98
	Public Sub DisableGravity()
		Me.allowFalling = False
		Me.MoveDirection = New Trilean2(Me.MoveDirection.x, 0)
		Me.velocityManager.y = 0F
	End Sub

	' Token: 0x06003DA7 RID: 15783 RVA: 0x0021F9DB File Offset: 0x0021DDDB
	Public Sub EnableGravity()
		Me.allowFalling = True
		Me.velocityManager.y = 0F
	End Sub

	' Token: 0x06003DA8 RID: 15784 RVA: 0x0021F9F4 File Offset: 0x0021DDF4
	Public Sub SetGravityReversed(reversed As Boolean)
		If reversed <> Me.GravityReversed Then
			Me.GravityReversed = reversed
			MyBase.player.animationController.OnGravityReversed()
			MyBase.transform.AddPosition(0F, -(MyBase.player.center.y - MyBase.transform.position.y) * CSng(If((Not MyBase.player.stats.isChalice), 2, 4)), 0F)
			Me.reversingGravity = True
		End If
	End Sub

	' Token: 0x06003DA9 RID: 15785 RVA: 0x0021FA86 File Offset: 0x0021DE86
	Private Function BoxCast(size As Vector2, direction As Vector2, layerMask As Integer) As RaycastHit2D
		Return Me.BoxCast(size, direction, layerMask, Vector2.zero)
	End Function

	' Token: 0x06003DAA RID: 15786 RVA: 0x0021FA96 File Offset: 0x0021DE96
	Private Function BoxCast(size As Vector2, direction As Vector2, layerMask As Integer, offset As Vector2) As RaycastHit2D
		Return Physics2D.BoxCast(MyBase.player.colliderManager.DefaultCenter + offset, size, 0F, direction, 2000F, layerMask)
	End Function

	' Token: 0x06003DAB RID: 15787 RVA: 0x0021FAC1 File Offset: 0x0021DEC1
	Private Function CircleCast(radius As Single, direction As Vector2, layerMask As Integer) As RaycastHit2D
		Return Physics2D.CircleCast(MyBase.player.colliderManager.DefaultCenter, radius, direction, 2000F, layerMask)
	End Function

	' Token: 0x06003DAC RID: 15788 RVA: 0x0021FAE0 File Offset: 0x0021DEE0
	Private Function DoesRaycastHitHaveCollider(hit As RaycastHit2D) As Boolean
		Return hit.collider IsNot Nothing
	End Function

	' Token: 0x06003DAD RID: 15789 RVA: 0x0021FAF0 File Offset: 0x0021DEF0
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		If Application.isPlaying Then
			Gizmos.color = Color.blue
			Gizmos.DrawSphere(MyBase.player.center, 5F)
			Gizmos.color = Color.red
			Gizmos.DrawSphere(MyBase.transform.position, 5F)
		End If
	End Sub

	' Token: 0x06003DAE RID: 15790 RVA: 0x0021FB4C File Offset: 0x0021DF4C
	Private Sub HandleRaycasts()
		Dim flag As Boolean = True
		If Me.directionManager IsNot Nothing AndAlso Me.directionManager.up IsNot Nothing Then
			flag = Me.directionManager.up.able
		End If
		Dim colliderManager As LevelPlayerColliderManager = MyBase.player.colliderManager
		Me.directionManager.Reset()
		Dim raycastHit2D As RaycastHit2D = Me.BoxCast(New Vector2(1F, If((Not flag AndAlso MyBase.player.stats.isChalice), 1F, colliderManager.DefaultHeight)), Vector2.left, Me.wallMask)
		Dim raycastHit2D2 As RaycastHit2D = Me.BoxCast(New Vector2(1F, If((Not flag AndAlso MyBase.player.stats.isChalice), 1F, colliderManager.DefaultHeight)), Vector2.right, Me.wallMask)
		Dim raycastHit2D3 As RaycastHit2D = Me.BoxCast(New Vector2(colliderManager.DefaultWidth, 1F), If((Not Me.GravityReversed), Vector2.up, Vector2.down), If((Not Me.GravityReversed), Me.ceilingMask, Me.groundMask))
		Me.RaycastObstacle(Me.directionManager.left, raycastHit2D, colliderManager.DefaultWidth / 2F, LevelPlayerMotor.RaycastAxis.X)
		Me.RaycastObstacle(Me.directionManager.right, raycastHit2D2, colliderManager.DefaultWidth / 2F, LevelPlayerMotor.RaycastAxis.X)
		Me.RaycastObstacle(Me.directionManager.up, raycastHit2D3, colliderManager.DefaultHeight / 2F, LevelPlayerMotor.RaycastAxis.Y)
		Dim vector As Vector2 = colliderManager.DefaultCenter + New Vector2(0F, colliderManager.DefaultHeight * Me.GravityReversalMultiplier)
		Dim num As Integer = Physics2D.BoxCastNonAlloc(vector, New Vector2(colliderManager.DefaultWidth, 1F), 0F, If((Not Me.GravityReversed), Vector2.down, Vector2.up), Me.hitBuffer, 1000F, If((Not Me.GravityReversed), Me.groundMask, Me.ceilingMask))
		Me.directionManager.down.pos = New Vector2(colliderManager.DefaultCenter.x, -10000F * Me.GravityReversalMultiplier)
		For i As Integer = 0 To num - 1
			Dim raycastHit2D4 As RaycastHit2D = Me.hitBuffer(i)
			If If((Not Me.GravityReversed), (raycastHit2D4.point.y > Me.directionManager.down.pos.y), (raycastHit2D4.point.y < Me.directionManager.down.pos.y)) Then
				If Not If((Not Me.GravityReversed), (raycastHit2D4.point.y > 20F + MyBase.transform.position.y), (raycastHit2D4.point.y < -20F + MyBase.transform.position.y)) Then
					Dim num2 As Single = Math.Abs(MyBase.transform.position.y - raycastHit2D4.point.y)
					Me.directionManager.down.pos = New Vector2(vector.x, raycastHit2D4.point.y)
					Me.directionManager.down.gameObject = raycastHit2D4.collider.gameObject
					Me.directionManager.down.distance = num2
					If num2 < 20F Then
						Me.directionManager.down.able = False
					End If
					Global.Debug.DrawLine(vector, Me.directionManager.down.pos, Color.red)
				End If
			End If
		Next
		If Not Me.Grounded Then
			If Not Me.directionManager.down.able Then
				Me.OnGrounded()
				Me.directionManager.left.able = True
				Me.directionManager.right.able = True
			End If
			If Not Me.directionManager.up.able AndAlso (Me.reversingGravity OrElse Me.directionManager.up.able <> flag) Then
				Dim gameObject As GameObject = Me.directionManager.up.gameObject
				Dim levelPlatform As LevelPlatform = If((Not(gameObject Is Nothing)), gameObject.GetComponent(Of LevelPlatform)(), Nothing)
				If Not Me.GravityReversed OrElse levelPlatform Is Nothing OrElse Not levelPlatform.canFallThrough Then
					Me.OnHitCeiling()
				End If
			End If
		End If
		Dim num3 As Single = Mathf.Abs(MyBase.transform.position.y - Me.directionManager.down.pos.y)
		If Me.Grounded AndAlso num3 > 30F Then
			Me.LeaveGround(True)
		End If
	End Sub

	' Token: 0x06003DAF RID: 15791 RVA: 0x0022007C File Offset: 0x0021E47C
	Private Function RaycastObstacle(directionProperties As LevelPlayerMotor.DirectionManager.Hit, raycastHit As RaycastHit2D, maxDistance As Single, axis As LevelPlayerMotor.RaycastAxis) As Single
		If Not Me.DoesRaycastHitHaveCollider(raycastHit) Then
			Return 1000F
		End If
		Dim num As Single = If((axis <> LevelPlayerMotor.RaycastAxis.X), Math.Abs(MyBase.player.colliderManager.DefaultCenter.y - raycastHit.point.y), Math.Abs(MyBase.player.colliderManager.DefaultCenter.x - raycastHit.point.x))
		directionProperties.pos = raycastHit.point
		directionProperties.gameObject = raycastHit.collider.gameObject
		directionProperties.distance = num
		If num < maxDistance Then
			directionProperties.able = False
		End If
		Return num
	End Function

	' Token: 0x06003DB0 RID: 15792 RVA: 0x00220138 File Offset: 0x0021E538
	Private Sub OnGrounded()
		If Me.Grounded OrElse Not Me.jumpManager.ableToLand Then
			Return
		End If
		If Me.platformManager.IsPlatformIgnored(Me.directionManager.down.gameObject.transform) Then
			Return
		End If
		Dim component As LevelPlatform = Me.directionManager.down.gameObject.GetComponent(Of LevelPlatform)()
		If component IsNot Nothing Then
			If component.canFallThrough AndAlso Me.jumpManager.timeSinceDownJump < 0.1F Then
				Return
			End If
			component.AddChild(MyBase.transform)
		End If
		If Me.jumpManager.doubleJumped Then
			Me.jumpManager.doubleJumped = False
		End If
		Me.jumpManager.state = LevelPlayerMotor.JumpManager.State.Ready
		Me.parryManager.state = LevelPlayerMotor.ParryManager.State.Ready
		Me.velocityManager.y = 0F
		Me.platformManager.ResetAll()
		Me.Grounded = True
		Me.Parrying = False
		Me.reversingGravity = False
		Me.dashManager.timeSinceGroundDash = 1000F
		If MyBase.player.stats.isChalice Then
			Me.ChaliceDoubleJumped = False
		End If
		If Me.jumpManager.timeInAir > Me.jumpManager.longestTimeInAir Then
			Me.jumpManager.longestTimeInAir = Me.jumpManager.timeInAir
			OnlineManager.Instance.[Interface].SetStat(MyBase.player.id, "HangTime", Me.jumpManager.timeInAir)
		End If
		If Me.OnGroundedEvent IsNot Nothing Then
			Me.OnGroundedEvent()
		End If
	End Sub

	' Token: 0x06003DB1 RID: 15793 RVA: 0x002202D8 File Offset: 0x0021E6D8
	Private Sub LeaveGround(Optional allowLateJump As Boolean = False)
		If Not Me.Dashing AndAlso MyBase.player.stats.Loadout.charm = Charm.charm_parry_plus AndAlso Not Level.IsChessBoss Then
			Me.ForceParry()
		End If
		If Me.Grounded Then
			Me.Grounded = False
			Me.jumpManager.ableToLand = False
			Me.jumpManager.timeInAir = 0F
			MyBase.player.stats.ResetJumpParries()
			Me.ResetSuperAndEx()
			MyBase.player.weaponManager.ResetEx()
		End If
		Me.velocityManager.y = 0F
		Me.ClearParent()
		If Me.jumpManager.state = LevelPlayerMotor.JumpManager.State.Ready AndAlso Not allowLateJump Then
			Me.jumpManager.state = LevelPlayerMotor.JumpManager.State.Used
		End If
	End Sub

	' Token: 0x06003DB2 RID: 15794 RVA: 0x002203AC File Offset: 0x0021E7AC
	Private Sub OnHitCeiling()
		If Me.jumpManager.ableToLand Then
			Return
		End If
		Me.velocityManager.y = 0F
		Me.directionManager.left.able = True
		Me.directionManager.right.able = True
	End Sub

	' Token: 0x06003DB3 RID: 15795 RVA: 0x002203FC File Offset: 0x0021E7FC
	Public Iterator Function MoveToX_cr(x As Single, Optional endingLookDirection As Integer = 1) As IEnumerator
		If MyBase.transform.position.x = x Then
			Return
		End If
		Dim walk As Single = 0F
		Me.MoveDirection = New Trilean2(0, 0)
		Me.LookDirection = New Trilean2(1, 0)
		Dim left As Boolean = MyBase.transform.position.x < x
		If Not left Then
			Me.LookDirection = New Trilean2(-1, 0)
		End If
		While If((Not left), (MyBase.transform.position.x > x), (MyBase.transform.position.x < x))
			If(Me.LookDirection.y >= 0 OrElse Not Me.Grounded) AndAlso Not Me.Locked Then
				walk = CSng(If((Not left), (-1), 1)) * Me.properties.moveSpeed
			End If
			Me.velocityManager.move = walk
			Yield Nothing
		End While
		walk = 0F
		Me.velocityManager.move = walk
		Me.MoveDirection = New Trilean2(0, 0)
		Me.LookDirection = New Trilean2(endingLookDirection, 0)
		Yield Nothing
		Me.LookDirection = New Trilean2(0, 0)
		Return
	End Function

	' Token: 0x06003DB4 RID: 15796 RVA: 0x00220428 File Offset: 0x0021E828
	Private Sub Move()
		Me.velocityManager.Calculate()
		Dim vector As Vector3 = Me.velocityManager.Total
		If Me.hitManager.state <> LevelPlayerMotor.HitManager.State.Hit AndAlso Me.superManager.state = LevelPlayerMotor.SuperManager.State.Ready Then
			If Not Me.velocityManager.yAxisForce Then
				Me.forceLaunchUp = False
				If Me.Grounded Then
					vector.x += Me.velocityManager.GroundForce
				Else
					vector.x += Me.velocityManager.AirForce
				End If
			ElseIf Me.Grounded Then
				If Not Me.forceLaunchUp Then
					Me.LeaveGround(False)
					Me.velocityManager.y = Me.properties.jumpPower * 2F
					Me.DisableGravity()
					Me.forceLaunchUp = True
				End If
			Else
				vector.y += Me.velocityManager.AirForce
				MyBase.FrameDelayedCallback(AddressOf Me.EnableGravity, 1)
			End If
		End If
		If vector.x > 0F AndAlso Not Me.directionManager.right.able Then
			vector.x = 0F
		End If
		If vector.x < 0F AndAlso Not Me.directionManager.left.able Then
			vector.x = 0F
		End If
		If Me.platformManager.OnPlatform Then
			If Not Me.directionManager.right.able AndAlso Me.MoveDirection.x > 0 Then
				vector.x = 0F
				MyBase.transform.SetPosition(New Single?(Me.lastPosition.x), Nothing, Nothing)
			End If
			If Not Me.directionManager.left.able AndAlso Me.MoveDirection.x < 0 Then
				vector.x = 0F
				MyBase.transform.SetPosition(New Single?(Me.lastPosition.x), Nothing, Nothing)
			End If
		End If
		If Me.GravityReversed Then
			vector.y *= -1F
		End If
		MyBase.transform.localPosition += vector * CupheadTime.FixedDelta
		If Me.Grounded Then
			Dim vector2 As Vector2 = MyBase.transform.position
			vector2.y = Me.directionManager.down.pos.y
			MyBase.transform.position = vector2
			Dim levelPlatform As LevelPlatform = Nothing
			If Me.directionManager.down.gameObject IsNot Nothing Then
				levelPlatform = Me.directionManager.down.gameObject.GetComponent(Of LevelPlatform)()
			End If
			If levelPlatform Is Nothing AndAlso MyBase.transform.parent IsNot Nothing Then
				Me.ClearParent()
			ElseIf levelPlatform IsNot Nothing AndAlso (MyBase.transform.parent Is Nothing OrElse levelPlatform.gameObject IsNot MyBase.transform.parent.gameObject) Then
				Me.ClearParent()
				levelPlatform.AddChild(MyBase.transform)
			End If
		End If
	End Sub

	' Token: 0x06003DB5 RID: 15797 RVA: 0x002207D0 File Offset: 0x0021EBD0
	Private Sub ClampToBounds()
		Dim num As Single = MyBase.player.colliderManager.Width / 2F
		Dim num2 As Single = Me.directionManager.left.pos.x + If((Not Me.reversingGravity), num, (-num))
		Dim num3 As Single = Me.directionManager.right.pos.x - If((Not Me.reversingGravity), num, (-num))
		Dim num4 As Single = Me.directionManager.up.pos.y - If((Not Me.GravityReversed), Me.boundsManager.TopY, Me.boundsManager.BottomY)
		Dim num5 As Single = Me.directionManager.down.pos.y - If((Not Me.GravityReversed), Me.boundsManager.BottomY, Me.boundsManager.TopY)
		Dim gameObject As GameObject = Me.directionManager.up.gameObject
		Dim levelPlatform As LevelPlatform = If((Not(gameObject Is Nothing)), gameObject.GetComponent(Of LevelPlatform)(), Nothing)
		Dim flag As Boolean = Not Me.GravityReversed OrElse levelPlatform Is Nothing OrElse Not levelPlatform.canFallThrough
		Dim position As Vector3 = MyBase.transform.position
		If Not Me.directionManager.left.able AndAlso MyBase.transform.position.x < num2 Then
			position.x = num2
		End If
		If Not Me.directionManager.right.able AndAlso MyBase.transform.position.x > num3 Then
			position.x = num3
		End If
		If Not Me.directionManager.up.able AndAlso flag AndAlso If((Not Me.GravityReversed), (MyBase.transform.position.y > num4), (MyBase.transform.position.y < num4)) Then
			position.y = num4
		End If
		position.x = Mathf.Clamp(position.x, CSng(Level.Current.Left) + num, CSng(Level.Current.Right) - num)
		MyBase.transform.position = position
	End Sub

	' Token: 0x06003DB6 RID: 15798 RVA: 0x00220A34 File Offset: 0x0021EE34
	Private Sub ResetSuperAndEx()
		If Me.superManager.state = LevelPlayerMotor.SuperManager.State.Ready Then
			Return
		End If
		If Me.jumpManager.state <> LevelPlayerMotor.JumpManager.State.Ready Then
			Me.jumpManager.state = LevelPlayerMotor.JumpManager.State.Used
		End If
		MyBase.StopCoroutine(Me.exMove_cr())
		Me.superManager.state = LevelPlayerMotor.SuperManager.State.Ready
		Me.EnableInput()
		Me.EnableGravity()
	End Sub

	' Token: 0x06003DB7 RID: 15799 RVA: 0x00220A92 File Offset: 0x0021EE92
	Public Sub StartSuper()
		Me.LeaveGround(False)
		Me.jumpManager.state = LevelPlayerMotor.JumpManager.State.Used
		Me.jumpManager.timer = 0F
		Me.velocityManager.y = 0F
	End Sub

	' Token: 0x06003DB8 RID: 15800 RVA: 0x00220AC7 File Offset: 0x0021EEC7
	Public Sub OnSuperEnd()
		If Me.Grounded Then
			Me.jumpManager.state = LevelPlayerMotor.JumpManager.State.Ready
		Else
			Me.DoPostSuperHop()
		End If
	End Sub

	' Token: 0x06003DB9 RID: 15801 RVA: 0x00220AEC File Offset: 0x0021EEEC
	Public Sub DoPostSuperHop()
		Me.LeaveGround(False)
		Me.velocityManager.y = If((MyBase.player.stats.Loadout.super <> Super.level_super_invincible), Me.properties.superKnockUp, Me.properties.superInvincibleKnockUp)
	End Sub

	' Token: 0x06003DBA RID: 15802 RVA: 0x00220B45 File Offset: 0x0021EF45
	Public Sub CheckForPostSuperHop()
		Me.HandleRaycasts()
		If Not Me.Grounded Then
			Me.DoPostSuperHop()
			MyBase.player.animator.Play("Jump_Launch")
		End If
	End Sub

	' Token: 0x06003DBB RID: 15803 RVA: 0x00220B73 File Offset: 0x0021EF73
	Private Sub StartEx()
		Me.exFirePose = MyBase.player.weaponManager.GetDirectionPose()
		Me.DisableInput()
		Me.DisableGravity()
		Me.superManager.state = LevelPlayerMotor.SuperManager.State.Ex
	End Sub

	' Token: 0x06003DBC RID: 15804 RVA: 0x00220BA3 File Offset: 0x0021EFA3
	Private Sub OnExFired()
		If Me.exFirePose = LevelPlayerWeaponManager.Pose.Up OrElse Me.exFirePose = LevelPlayerWeaponManager.Pose.Down Then
			MyBase.StartCoroutine(Me.exDelay_cr())
		Else
			MyBase.StartCoroutine(Me.exMove_cr())
		End If
	End Sub

	' Token: 0x06003DBD RID: 15805 RVA: 0x00220BDC File Offset: 0x0021EFDC
	Private Iterator Function exDelay_cr() As IEnumerator
		While Me.superManager.state <> LevelPlayerMotor.SuperManager.State.Ready
			Yield Nothing
		End While
		Me.EnableInput()
		Me.EnableGravity()
		Me.superManager.state = LevelPlayerMotor.SuperManager.State.Ready
		Return
	End Function

	' Token: 0x06003DBE RID: 15806 RVA: 0x00220BF8 File Offset: 0x0021EFF8
	Private Iterator Function exMove_cr() As IEnumerator
		While Me.superManager.state <> LevelPlayerMotor.SuperManager.State.Ready
			Me.velocityManager.move = CSng((Me.TrueLookDirection.x * -1)) * Me.properties.exKnockback
			Yield Nothing
		End While
		Me.EnableInput()
		Me.EnableGravity()
		Me.superManager.state = LevelPlayerMotor.SuperManager.State.Ready
		Return
	End Function

	' Token: 0x06003DBF RID: 15807 RVA: 0x00220C14 File Offset: 0x0021F014
	Private Sub HandleInput()
		If Not MyBase.player.levelStarted Then
			Return
		End If
		Me.timeSinceInputBuffered += CupheadTime.FixedDelta
		Me.dashManager.timeSinceGroundDash += CupheadTime.FixedDelta
		If(Not Me.allowInput OrElse Me.dashManager.IsDashing) AndAlso Me.hitManager.state = LevelPlayerMotor.HitManager.State.Inactive Then
			Me.BufferInputs()
		End If
		If Not Me.allowInput Then
			Return
		End If
		If Not Me.HandleDash() Then
			If Me.hitManager.state = LevelPlayerMotor.HitManager.State.Hit Then
				Me.HandleHit()
			Else
				If Me.hitManager.state <> LevelPlayerMotor.HitManager.State.KnockedUp Then
					Me.HandleParry()
					Me.HandleJumping()
					Me.HandleLocked()
				Else
					Me.HandlePitKnockUp()
				End If
				Me.HandleWalking()
			End If
		End If
	End Sub

	' Token: 0x06003DC0 RID: 15808 RVA: 0x00220CF5 File Offset: 0x0021F0F5
	Private Sub BufferInput(input As LevelPlayerMotor.BufferedInput)
		Me.bufferedInput = input
		Me.timeSinceInputBuffered = 0F
	End Sub

	' Token: 0x06003DC1 RID: 15809 RVA: 0x00220D0C File Offset: 0x0021F10C
	Public Sub BufferInputs()
		If MyBase.player.input.actions.GetButtonDown(2) Then
			Me.BufferInput(LevelPlayerMotor.BufferedInput.Jump)
		ElseIf MyBase.player.input.actions.GetButtonDown(7) AndAlso Not Me.dashManager.IsDashing Then
			Me.BufferInput(LevelPlayerMotor.BufferedInput.Dash)
		ElseIf MyBase.player.input.actions.GetButtonDown(4) Then
			Me.BufferInput(LevelPlayerMotor.BufferedInput.Super)
		End If
	End Sub

	' Token: 0x06003DC2 RID: 15810 RVA: 0x00220D99 File Offset: 0x0021F199
	Public Sub ClearBufferedInput()
		Me.timeSinceInputBuffered = 0.134F
	End Sub

	' Token: 0x06003DC3 RID: 15811 RVA: 0x00220DA6 File Offset: 0x0021F1A6
	Public Function HasBufferedInput(input As LevelPlayerMotor.BufferedInput) As Boolean
		Return Me.bufferedInput = input AndAlso Me.timeSinceInputBuffered < 0.134F
	End Function

	' Token: 0x06003DC4 RID: 15812 RVA: 0x00220DC4 File Offset: 0x0021F1C4
	Private Sub HandleJumping()
		If Me.allowJumping Then
			If Me.jumpManager.state = LevelPlayerMotor.JumpManager.State.Ready AndAlso (MyBase.player.input.actions.GetButtonDown(2) OrElse Me.HasBufferedInput(LevelPlayerMotor.BufferedInput.Jump)) Then
				Me.hardExitParry = False
				Me.ClearBufferedInput()
				If If((MyBase.player.stats.ReverseTime > 0F), (Me.LookDirection.y > 0), (Me.LookDirection.y < 0)) AndAlso Me.Grounded AndAlso MyBase.transform.parent IsNot Nothing Then
					Dim component As LevelPlatform = MyBase.transform.parent.GetComponent(Of LevelPlatform)()
					If component.canFallThrough Then
						Me.platformManager.Ignore(MyBase.transform.parent)
						Me.jumpManager.state = LevelPlayerMotor.JumpManager.State.Used
						Me.LeaveGround(False)
						Me.jumpManager.timeSinceDownJump = 0F
						Return
					End If
				End If
				AudioManager.Play("player_jump")
				Me.jumpManager.state = LevelPlayerMotor.JumpManager.State.Hold
				Me.LeaveGround(False)
				Me.velocityManager.y = Me.jumpPower
				Me.jumpManager.timer = CupheadTime.FixedDelta
				If Me.OnJumpEvent IsNot Nothing Then
					Me.OnJumpEvent()
				End If
			End If
			If Me.jumpManager.state = LevelPlayerMotor.JumpManager.State.Hold Then
				If Not Me.directionManager.up.able OrElse (Me.jumpManager.timer >= Me.properties.jumpHoldMin AndAlso (MyBase.player.input.actions.GetButtonUp(2) OrElse Not MyBase.player.input.actions.GetButton(2))) OrElse Me.jumpManager.timer >= Me.properties.jumpHoldMax Then
					Me.jumpManager.state = LevelPlayerMotor.JumpManager.State.Used
					Me.jumpManager.timer = 0F
				End If
				If MyBase.player.stats.isChalice Then
					Me.velocityManager.y = If((Not Me.jumpManager.doubleJumped), Me.properties.chaliceFirstJumpPower, Me.properties.chaliceSecondJumpPower)
				Else
					Me.velocityManager.y = Me.jumpPower
				End If
				Me.jumpManager.timer += CupheadTime.FixedDelta
			End If
			Me.jumpManager.timeSinceDownJump += CupheadTime.FixedDelta
			If MyBase.player.stats.isChalice AndAlso Not Me.jumpManager.doubleJumped Then
				Me.ChaliceDoubleJump()
			End If
		End If
	End Sub

	' Token: 0x06003DC5 RID: 15813 RVA: 0x002210A3 File Offset: 0x0021F4A3
	Public Sub OnChaliceRevive()
		Me.ChaliceDoubleJumped = True
	End Sub

	' Token: 0x06003DC6 RID: 15814 RVA: 0x002210AC File Offset: 0x0021F4AC
	Private Sub ChaliceDoubleJump()
		If(MyBase.player.input.actions.GetButtonDown(2) OrElse Me.HasBufferedInput(LevelPlayerMotor.BufferedInput.Jump)) AndAlso Me.jumpManager.state = LevelPlayerMotor.JumpManager.State.Used AndAlso Not Me.IsHit Then
			Me.hardExitParry = False
			Me.ClearBufferedInput()
			If Me.dashManager.state = LevelPlayerMotor.DashManager.State.[End] AndAlso Me.parryManager.state = LevelPlayerMotor.ParryManager.State.Ready Then
				Me.dashManager.state = LevelPlayerMotor.DashManager.State.Ready
			End If
			AudioManager.Play("chalice_doublejump")
			Me.jumpManager.state = LevelPlayerMotor.JumpManager.State.Hold
			Me.LeaveGround(False)
			Me.jumpManager.doubleJumped = True
			Me.velocityManager.y = Me.properties.chaliceSecondJumpPower
			Me.jumpManager.timer = CupheadTime.FixedDelta
			Me.ChaliceDoubleJumped = True
			Me.platformManager.ResetAll()
			If Me.OnJumpEvent IsNot Nothing Then
				Me.OnJumpEvent()
			End If
			If Me.OnDoubleJumpEvent IsNot Nothing Then
				Me.OnDoubleJumpEvent()
			End If
		End If
	End Sub

	' Token: 0x06003DC7 RID: 15815 RVA: 0x002211C4 File Offset: 0x0021F5C4
	Private Sub HandleParry()
		If MyBase.player.stats.isChalice Then
			Return
		End If
		If Me.IsHit Then
			Return
		End If
		If Me.parryManager.state = LevelPlayerMotor.ParryManager.State.Ready AndAlso (MyBase.player.input.actions.GetButtonDown(2) OrElse Me.HasBufferedInput(LevelPlayerMotor.BufferedInput.Jump)) AndAlso Me.jumpManager.state <> LevelPlayerMotor.JumpManager.State.Ready AndAlso Not Me.IsHit Then
			Me.ClearBufferedInput()
			Me.hitManager.state = LevelPlayerMotor.HitManager.State.Inactive
			Me.parryManager.state = LevelPlayerMotor.ParryManager.State.NotReady
			If Me.dashManager.IsDashing Then
				Me.dashManager.state = LevelPlayerMotor.DashManager.State.[End]
			End If
			Me.Parrying = True
			If Me.OnParryEvent IsNot Nothing Then
				Me.OnParryEvent()
			End If
		End If
	End Sub

	' Token: 0x06003DC8 RID: 15816 RVA: 0x0022129C File Offset: 0x0021F69C
	Public Sub OnParryComplete()
		If MyBase.player.stats.Loadout.charm = Charm.charm_parry_plus AndAlso Not Level.IsChessBoss Then
			Me.hardExitParry = True
		End If
		Me.LeaveGround(False)
		Me.parryManager.state = LevelPlayerMotor.ParryManager.State.Ready
		Me.velocityManager.y = If((Not Me.parryController.HasHitEnemy), Me.properties.parryPower, Me.properties.parryAttackBounce)
		If Me.OnParrySuccess IsNot Nothing Then
			Me.OnParrySuccess()
		End If
		If MyBase.player.stats.isChalice Then
			Me.dashManager.chaliceParryCoolDown = True
			Me.DashComplete()
		End If
		Me.platformManager.ResetAll()
	End Sub

	' Token: 0x06003DC9 RID: 15817 RVA: 0x0022136A File Offset: 0x0021F76A
	Public Sub OnParryHit()
		MyBase.StartCoroutine(Me.parryHit_cr())
	End Sub

	' Token: 0x06003DCA RID: 15818 RVA: 0x00221379 File Offset: 0x0021F779
	Public Sub OnParryCanceled()
		Me.Parrying = False
	End Sub

	' Token: 0x06003DCB RID: 15819 RVA: 0x00221382 File Offset: 0x0021F782
	Public Sub OnParryAnimEnd()
		Me.Parrying = False
	End Sub

	' Token: 0x06003DCC RID: 15820 RVA: 0x0022138C File Offset: 0x0021F78C
	Private Function HandleDash() As Boolean
		If Me.dashManager.state = LevelPlayerMotor.DashManager.State.Ready AndAlso (Not Me.Grounded OrElse Me.dashManager.timeSinceGroundDash > 0.1F) AndAlso (MyBase.player.input.actions.GetButtonDown(7) OrElse Me.HasBufferedInput(LevelPlayerMotor.BufferedInput.Dash)) Then
			Me.ClearBufferedInput()
			AudioManager.Play("player_dash")
			Me.dashManager.state = LevelPlayerMotor.DashManager.State.Start
			Me.dashManager.direction = Me.TrueLookDirection.x
			Me.dashManager.groundDash = Me.Grounded
			Me.ChaliceDuckDashed = MyBase.player.stats.isChalice AndAlso Me.Ducking
			If Me.jumpManager.state = LevelPlayerMotor.JumpManager.State.Hold Then
				Me.jumpManager.state = LevelPlayerMotor.JumpManager.State.Used
			End If
			If Me.OnDashStartEvent IsNot Nothing Then
				Me.OnDashStartEvent()
			End If
			Me.velocityManager.move = 0F
			Return True
		End If
		If Me.dashManager.state = LevelPlayerMotor.DashManager.State.Start Then
			Me.dashManager.state = LevelPlayerMotor.DashManager.State.Dashing
		End If
		If MyBase.player.stats.isChalice AndAlso Not Me.ChaliceDuckDashed Then
			Me.ChaliceDashParry()
		End If
		If Me.dashManager.state = LevelPlayerMotor.DashManager.State.Dashing Then
			Me.velocityManager.dash = Me.properties.dashSpeed * CSng(Me.dashManager.direction)
			Me.dashManager.timer += CupheadTime.FixedDelta
			Me.velocityManager.y = 0F
			If Me.dashManager.timer >= Me.properties.dashTime Then
				Me.DashComplete()
			End If
			If Not Me.Grounded Then
				Me.jumpManager.ableToLand = True
			End If
			Return True
		End If
		If Me.dashManager.state = LevelPlayerMotor.DashManager.State.[End] Then
			If Me.Grounded Then
				Me.dashManager.state = LevelPlayerMotor.DashManager.State.Ready
				If Me.dashManager.groundDash Then
					Me.dashManager.timeSinceGroundDash = 0F
				End If
				If MyBase.player.stats.isChalice Then
					Me.dashManager.chaliceParryCoolDown = False
					Me.dashManager.chaliceParryCoolDownTimer = 0F
				End If
			Else
				Me.dashManager.groundDash = False
			End If
			Me.ChaliceDuckDashed = False
			If MyBase.player.stats.isChalice AndAlso Not Me.dashManager.chaliceParryCoolDown Then
				Me.dashManager.state = LevelPlayerMotor.DashManager.State.Ready
			End If
			If MyBase.player.stats.isChalice Then
				Me.ChaliceDashCooldownCheck()
			End If
		End If
		Return False
	End Function

	' Token: 0x06003DCD RID: 15821 RVA: 0x0022165C File Offset: 0x0021FA5C
	Public Sub DashComplete()
		If MyBase.player.stats.Loadout.charm = Charm.charm_parry_plus AndAlso Not Level.IsChessBoss Then
			Me.ForceParry()
		End If
		Me.dashManager.state = LevelPlayerMotor.DashManager.State.[End]
		Me.dashManager.timer = 0F
		Me.velocityManager.dash = 0F
		Me.velocityManager.verticalDash = 0F
		If Me.OnDashEndEvent IsNot Nothing Then
			Me.OnDashEndEvent()
		End If
	End Sub

	' Token: 0x06003DCE RID: 15822 RVA: 0x002216EC File Offset: 0x0021FAEC
	Private Sub ForceParry()
		If Me.hitManager.state <> LevelPlayerMotor.HitManager.State.Hit AndAlso Not Me.hardExitParry Then
			Me.hitManager.state = LevelPlayerMotor.HitManager.State.Inactive
			Me.parryManager.state = LevelPlayerMotor.ParryManager.State.NotReady
			Me.Parrying = True
			If Me.OnParryEvent IsNot Nothing Then
				Me.OnParryEvent()
			End If
		End If
	End Sub

	' Token: 0x06003DCF RID: 15823 RVA: 0x0022174C File Offset: 0x0021FB4C
	Private Sub ChaliceDashParry()
		If Me.dashManager.IsDashing AndAlso Not Me.dashManager.chaliceParryCoolDown AndAlso Me.hitManager.state <> LevelPlayerMotor.HitManager.State.Hit AndAlso Not Me.hardExitParry Then
			Me.hitManager.state = LevelPlayerMotor.HitManager.State.Inactive
			Me.parryManager.state = LevelPlayerMotor.ParryManager.State.NotReady
			Me.Parrying = True
			If Me.OnParryEvent IsNot Nothing Then
				Me.OnParryEvent()
			End If
			Me.dashManager.chaliceParryCoolDown = True
		End If
	End Sub

	' Token: 0x06003DD0 RID: 15824 RVA: 0x002217D6 File Offset: 0x0021FBD6
	Public Sub ResetChaliceDoubleJump()
		Me.jumpManager.doubleJumped = False
		If MyBase.player.stats.isChalice Then
			Me.dashManager.chaliceParryCoolDown = False
			Me.dashManager.chaliceParryCoolDownTimer = 0F
		End If
	End Sub

	' Token: 0x06003DD1 RID: 15825 RVA: 0x00221818 File Offset: 0x0021FC18
	Private Sub ChaliceDashCooldownCheck()
		If Me.dashManager.chaliceParryCoolDown Then
			Me.dashManager.chaliceParryCoolDownTimer += CupheadTime.FixedDelta
			If Me.dashManager.chaliceParryCoolDownTimer >= Me.properties.dashParryCooldownTime Then
				Me.dashManager.chaliceParryCoolDown = False
				Me.dashManager.chaliceParryCoolDownTimer = 0F
			End If
		End If
	End Sub

	' Token: 0x06003DD2 RID: 15826 RVA: 0x00221883 File Offset: 0x0021FC83
	Public Function DistanceToGround() As Single
		Me.HandleRaycasts()
		Return Me.directionManager.down.distance
	End Function

	' Token: 0x06003DD3 RID: 15827 RVA: 0x0022189B File Offset: 0x0021FC9B
	Private Sub HandleLocked()
		If MyBase.player.input.actions.GetButton(6) AndAlso Me.Grounded Then
			Me.Locked = True
		Else
			Me.Locked = False
		End If
	End Sub

	' Token: 0x06003DD4 RID: 15828 RVA: 0x002218D8 File Offset: 0x0021FCD8
	Private Sub HandleWalking()
		Dim num As Single = 0F
		If(Me.LookDirection.y >= 0 OrElse Not Me.Grounded) AndAlso Not Me.Locked Then
			Dim num2 As Integer = If((MyBase.player.stats.ReverseTime > 0F), (-MyBase.player.input.GetAxisInt(PlayerInput.Axis.X, False, False)), MyBase.player.input.GetAxisInt(PlayerInput.Axis.X, False, False))
			num = CSng(num2) * Me.properties.moveSpeed
		End If
		Me.velocityManager.move = num
	End Sub

	' Token: 0x06003DD5 RID: 15829 RVA: 0x0022197C File Offset: 0x0021FD7C
	Private Sub HandleLooking()
		If MyBase.player.levelStarted AndAlso Me.allowInput Then
			Dim num As Integer = MyBase.player.input.GetAxisInt(PlayerInput.Axis.X, False, False)
			num = If((MyBase.player.stats.ReverseTime > 0F), (-num), num)
			Dim num2 As Integer = MyBase.player.input.GetAxisInt(PlayerInput.Axis.Y, True, Me.Grounded AndAlso Not Me.Locked AndAlso Not Me.IsUsingSuperOrEx)
			num2 = If((MyBase.player.stats.ReverseTime > 0F), (-num2), num2)
			If Me.GravityReversed Then
				num2 *= -1
			End If
			Me.LookDirection = New Trilean2(num, num2)
		End If
		Dim num3 As Integer = Me.TrueLookDirection.x
		Dim num4 As Integer = Me.TrueLookDirection.y
		If Me.LookDirection.x <> 0 Then
			num3 = Me.LookDirection.x
		End If
		num4 = Me.LookDirection.y
		Me.TrueLookDirection = New Trilean2(num3, num4)
	End Sub

	' Token: 0x06003DD6 RID: 15830 RVA: 0x00221ACB File Offset: 0x0021FECB
	Public Sub ForceLooking(direction As Trilean2)
		Me.LookDirection = direction
		Me.TrueLookDirection = direction
		MyBase.GetComponent(Of LevelPlayerAnimationController)().ForceDirection()
	End Sub

	' Token: 0x06003DD7 RID: 15831 RVA: 0x00221AE8 File Offset: 0x0021FEE8
	Private Sub HandleFalling()
		If Me.Grounded OrElse Me.dashManager.IsDashing Then
			Me.isFloating = False
			Me.jumpManager.floatTimer = 0F
			Return
		End If
		If Level.Current.LevelTime < 0.2F Then
			Return
		End If
		Dim num As Single = Me.properties.timeToMaxY * 60F
		Dim num2 As Single = Me.properties.maxSpeedY / num * CupheadTime.FixedDelta
		Me.velocityManager.y += num2
		Me.jumpManager.ableToLand = Me.velocityManager.y > 0F
		If MyBase.player.stats.Loadout.charm = Charm.charm_float AndAlso Me.jumpManager.ableToLand AndAlso MyBase.player.input.actions.GetButton(2) AndAlso Me.jumpManager.floatTimer < WeaponProperties.CharmFloat.maxTime Then
			Me.isFloating = True
			Dim num3 As Single = Mathf.Clamp(Me.jumpManager.floatTimer - WeaponProperties.CharmFloat.falloffStartTime, 0F, WeaponProperties.CharmFloat.maxTime - WeaponProperties.CharmFloat.falloffStartTime)
			num3 = Mathf.InverseLerp(0F, WeaponProperties.CharmFloat.maxTime - WeaponProperties.CharmFloat.falloffStartTime, num3)
			Me.velocityManager.y = Mathf.Clamp(Me.velocityManager.y, 0F, EaseUtils.EaseInSine(WeaponProperties.CharmFloat.minFallSpeed, WeaponProperties.CharmFloat.maxFallSpeed, num3))
			Me.jumpManager.floatTimer += CupheadTime.FixedDelta
		Else
			Me.isFloating = False
		End If
	End Sub

	' Token: 0x06003DD8 RID: 15832 RVA: 0x00221C8C File Offset: 0x0022008C
	Public Sub HandlePitKnockUp()
		If Me.hitManager.state <> LevelPlayerMotor.HitManager.State.KnockedUp Then
			Return
		End If
		If Me.hitManager.timer > Me.properties.knockUpStunTime Then
			Me.hitManager.state = LevelPlayerMotor.HitManager.State.Inactive
			Me.velocityManager.hit = 0F
		Else
			Me.hitManager.timer += CupheadTime.FixedDelta
		End If
	End Sub

	' Token: 0x06003DD9 RID: 15833 RVA: 0x00221D00 File Offset: 0x00220100
	Private Sub HandleHit()
		If Me.hitManager.state <> LevelPlayerMotor.HitManager.State.Hit Then
			Return
		End If
		If Me.hitManager.timer > Me.properties.hitStunTime Then
			Me.hitManager.state = LevelPlayerMotor.HitManager.State.Inactive
			Me.velocityManager.hit = 0F
		Else
			Dim num As Single = Me.hitManager.timer / Me.properties.hitStunTime
			Me.velocityManager.hit = EaseUtils.Ease(Me.properties.hitKnockbackEase, Me.properties.hitKnockbackPower, 0F, num) * CSng(Me.hitManager.direction)
			Me.hitManager.timer += CupheadTime.FixedDelta
		End If
	End Sub

	' Token: 0x06003DDA RID: 15834 RVA: 0x00221DC4 File Offset: 0x002201C4
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		If MyBase.player.stats.SuperInvincible Then
			Return
		End If
		Me.hitManager.state = LevelPlayerMotor.HitManager.State.Hit
		If Me.OnHitEvent IsNot Nothing Then
			Me.OnHitEvent()
		End If
		Me.DashComplete()
		Me.velocityManager.Clear()
		Me.ResetSuperAndEx()
		Dim num As Integer = Me.TrueLookDirection.x * -1
		Me.hitManager.direction = num
		Me.LeaveGround(False)
		Me.velocityManager.y = Me.properties.hitJumpPower
		Me.hitManager.timer = 0F
	End Sub

	' Token: 0x06003DDB RID: 15835 RVA: 0x00221E70 File Offset: 0x00220270
	Public Sub OnPitKnockUp(y As Single, Optional velocityScale As Single = 1F)
		If MyBase.player.IsDead Then
			MyBase.transform.SetPosition(Nothing, New Single?(y + 200F * Me.GravityReversalMultiplier), Nothing)
			Return
		End If
		If Not MyBase.player.stats.isChalice Then
			Me.hardExitParry = True
		End If
		MyBase.transform.SetPosition(Nothing, New Single?(y), Nothing)
		Me.hitManager.state = LevelPlayerMotor.HitManager.State.KnockedUp
		Me.DashComplete()
		Me.velocityManager.Clear()
		Me.ResetSuperAndEx()
		Me.hitManager.direction = 0
		Me.LeaveGround(False)
		If Level.Current.LevelType = Level.Type.Platforming Then
			Me.velocityManager.y = Me.properties.platformingPitKnockUpPower * velocityScale
		Else
			Me.velocityManager.y = Me.properties.pitKnockUpPower * velocityScale
		End If
		Me.hitManager.timer = 0F
		Me.dashManager.state = LevelPlayerMotor.DashManager.State.Ready
		Me.parryManager.state = LevelPlayerMotor.ParryManager.State.Ready
	End Sub

	' Token: 0x06003DDC RID: 15836 RVA: 0x00221FA0 File Offset: 0x002203A0
	Public Sub OnTrampolineKnockUp(y As Single)
		If MyBase.player.IsDead Then
			MyBase.transform.SetPosition(Nothing, New Single?(y * Me.GravityReversalMultiplier), Nothing)
			Return
		End If
		Me.LeaveGround(False)
		Me.hitManager.state = LevelPlayerMotor.HitManager.State.KnockedUp
		Me.DashComplete()
		Me.velocityManager.Clear()
		Me.ResetSuperAndEx()
		Me.hitManager.direction = 0
		Me.velocityManager.y = y
		Me.hitManager.timer = 0F
		Me.dashManager.state = LevelPlayerMotor.DashManager.State.Ready
		Me.parryManager.state = LevelPlayerMotor.ParryManager.State.Ready
		Me.jumpManager.state = LevelPlayerMotor.JumpManager.State.Ready
	End Sub

	' Token: 0x06003DDD RID: 15837 RVA: 0x00222060 File Offset: 0x00220460
	Private Iterator Function launch_player_cr([end] As Single) As IEnumerator
		Dim time As Single = 0.1F
		Dim t As Single = 0F
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		While t < time
			t += CupheadTime.FixedDelta
			Dim posY As Single = Mathf.Lerp(MyBase.transform.position.y, [end], t / time)
			MyBase.transform.SetPosition(Nothing, New Single?(posY), Nothing)
			Yield wait
		End While
		Return
	End Function

	' Token: 0x06003DDE RID: 15838 RVA: 0x00222084 File Offset: 0x00220484
	Public Sub OnRevive(pos As Vector3)
		If Me.GravityReversed Then
			pos.y -= (MyBase.player.center.y - MyBase.transform.position.y) * 2F
		End If
		MyBase.transform.position = pos
		Me.hitManager.state = LevelPlayerMotor.HitManager.State.KnockedUp
		Me.DashComplete()
		Me.velocityManager.Clear()
		Me.ResetSuperAndEx()
		Me.hitManager.direction = 0
		Me.LeaveGround(False)
		Me.velocityManager.y = Me.properties.reviveKnockUpPower
		Me.hitManager.timer = 0F
		MyBase.player.animationController.UpdateAnimator()
	End Sub

	' Token: 0x06003DDF RID: 15839 RVA: 0x0022214F File Offset: 0x0022054F
	Public Sub CancelReviveBounce()
		Me.velocityManager.y = 0F
	End Sub

	' Token: 0x06003DE0 RID: 15840 RVA: 0x00222161 File Offset: 0x00220561
	Public Sub AddForce(force As LevelPlayerMotor.VelocityManager.Force)
		Me.velocityManager.AddForce(force)
	End Sub

	' Token: 0x06003DE1 RID: 15841 RVA: 0x0022216F File Offset: 0x0022056F
	Public Sub RemoveForce(force As LevelPlayerMotor.VelocityManager.Force)
		Me.velocityManager.RemoveForce(force)
		force.yAxisForce = False
	End Sub

	' Token: 0x06003DE2 RID: 15842 RVA: 0x00222184 File Offset: 0x00220584
	Private Sub ClearParent()
		If MyBase.transform.parent IsNot Nothing Then
			MyBase.transform.parent.GetComponent(Of LevelPlatform)().OnPlayerExit(MyBase.transform)
		End If
		MyBase.transform.parent = Nothing
		Dim localScale As Vector3 = MyBase.transform.localScale
		localScale.y = 1F * Me.GravityReversalMultiplier
		MyBase.transform.localScale = localScale
	End Sub

	' Token: 0x06003DE3 RID: 15843 RVA: 0x002221F9 File Offset: 0x002205F9
	Public Sub OnPlatformingLevelExit()
		MyBase.StartCoroutine(Me.platformingExit_cr())
	End Sub

	' Token: 0x06003DE4 RID: 15844 RVA: 0x00222208 File Offset: 0x00220608
	Private Iterator Function platformingExit_cr() As IEnumerator
		While True
			If Me.Dashing Then
				Me.DashComplete()
			End If
			Me.allowInput = False
			Me.Locked = False
			Me.LookDirection = New Trilean2(1, 0)
			Me.velocityManager.move = Me.properties.moveSpeed
			Yield New WaitForFixedUpdate()
		End While
		Return
	End Function

	' Token: 0x06003DE5 RID: 15845 RVA: 0x00222224 File Offset: 0x00220624
	Private Iterator Function parryHit_cr() As IEnumerator
		Me.velocityManager.Clear()
		Yield Nothing
		Me.velocityManager.Clear()
		Return
	End Function

	' Token: 0x040044BE RID: 17598
	<SerializeField()>
	Private properties As LevelPlayerMotor.Properties

	' Token: 0x040044BF RID: 17599
	Private lastPositionFixed As Vector2

	' Token: 0x040044C0 RID: 17600
	Private lastPosition As Vector2

	' Token: 0x040044C1 RID: 17601
	Private velocityManager As LevelPlayerMotor.VelocityManager

	' Token: 0x040044C2 RID: 17602
	Private jumpManager As LevelPlayerMotor.JumpManager

	' Token: 0x040044C3 RID: 17603
	Private dashManager As LevelPlayerMotor.DashManager

	' Token: 0x040044C4 RID: 17604
	Private parryManager As LevelPlayerMotor.ParryManager

	' Token: 0x040044C5 RID: 17605
	Private directionManager As LevelPlayerMotor.DirectionManager

	' Token: 0x040044C6 RID: 17606
	Private platformManager As LevelPlayerMotor.PlatformManager

	' Token: 0x040044C7 RID: 17607
	Private hitManager As LevelPlayerMotor.HitManager

	' Token: 0x040044C8 RID: 17608
	Private superManager As LevelPlayerMotor.SuperManager

	' Token: 0x040044C9 RID: 17609
	Private boundsManager As LevelPlayerMotor.BoundsManager

	' Token: 0x040044CA RID: 17610
	Private allowInput As Boolean

	' Token: 0x040044CB RID: 17611
	Private allowJumping As Boolean

	' Token: 0x040044CC RID: 17612
	Private allowFalling As Boolean

	' Token: 0x040044CD RID: 17613
	Private forceLaunchUp As Boolean

	' Token: 0x040044CE RID: 17614
	Private hardExitParry As Boolean

	' Token: 0x040044CF RID: 17615
	Private reversingGravity As Boolean

	' Token: 0x040044D0 RID: 17616
	Private jumpPower As Single

	' Token: 0x040044D1 RID: 17617
	Private hitBuffer As RaycastHit2D() = New RaycastHit2D(24) {}

	' Token: 0x040044DA RID: 17626
	Private parryController As LevelPlayerParryController

	' Token: 0x040044DC RID: 17628
	Private Const RAY_DISTANCE As Single = 2000F

	' Token: 0x040044DD RID: 17629
	Private Const MAX_GROUNDED_FALL_DISTANCE As Single = 30F

	' Token: 0x040044DE RID: 17630
	Private wallMask As Integer = 262144

	' Token: 0x040044DF RID: 17631
	Private ceilingMask As Integer = 524288

	' Token: 0x040044E0 RID: 17632
	Private groundMask As Integer = 1048576

	' Token: 0x040044E1 RID: 17633
	Private exFirePose As LevelPlayerWeaponManager.Pose

	' Token: 0x040044E2 RID: 17634
	Private Const JUMP_BUFFER_TIME As Single = 0.0834F

	' Token: 0x040044E3 RID: 17635
	Public Const INPUT_BUFFER_TIME As Single = 0.134F

	' Token: 0x040044E4 RID: 17636
	Private bufferedInput As LevelPlayerMotor.BufferedInput

	' Token: 0x040044E5 RID: 17637
	Private timeSinceInputBuffered As Single = 0.134F

	' Token: 0x02000A20 RID: 2592
	Private Enum RaycastAxis
		' Token: 0x040044E7 RID: 17639
		X
		' Token: 0x040044E8 RID: 17640
		Y
	End Enum

	' Token: 0x02000A21 RID: 2593
	Public Enum BufferedInput
		' Token: 0x040044EA RID: 17642
		Jump
		' Token: 0x040044EB RID: 17643
		Dash
		' Token: 0x040044EC RID: 17644
		Super
	End Enum

	' Token: 0x02000A22 RID: 2594
	Public Class Properties
		' Token: 0x040044ED RID: 17645
		Public moveSpeed As Single = 490F

		' Token: 0x040044EE RID: 17646
		Public maxSpeedY As Single = 1620F

		' Token: 0x040044EF RID: 17647
		Public timeToMaxY As Single = 7.3F

		' Token: 0x040044F0 RID: 17648
		Public yEase As EaseUtils.EaseType = EaseUtils.EaseType.linear

		' Token: 0x040044F1 RID: 17649
		Public jumpHoldMin As Single = 0.01F

		' Token: 0x040044F2 RID: 17650
		Public jumpHoldMax As Single = 0.16F

		' Token: 0x040044F3 RID: 17651
		<Range(0F, -1F)>
		Public jumpPower As Single = -0.755F

		' Token: 0x040044F4 RID: 17652
		Public chaliceFirstJumpPower As Single = -0.63F

		' Token: 0x040044F5 RID: 17653
		Public chaliceSecondJumpPower As Single = -0.55F

		' Token: 0x040044F6 RID: 17654
		Public dashSpeed As Single = 1100F

		' Token: 0x040044F7 RID: 17655
		Public verticalDashSpeed As Single = 935F

		' Token: 0x040044F8 RID: 17656
		Public dashTime As Single = 0.3F

		' Token: 0x040044F9 RID: 17657
		Public dashEndTime As Single = 0.21F

		' Token: 0x040044FA RID: 17658
		Public dashEase As EaseUtils.EaseType = EaseUtils.EaseType.easeOutSine

		' Token: 0x040044FB RID: 17659
		Public dashParryCooldownTime As Single = 0.3F

		' Token: 0x040044FC RID: 17660
		Public platformIgnoreTime As Single = 1F

		' Token: 0x040044FD RID: 17661
		Public hitStunTime As Single = 0.3F

		' Token: 0x040044FE RID: 17662
		Public hitFalloff As Single = 0.25F

		' Token: 0x040044FF RID: 17663
		<Range(0F, -1F)>
		Public hitJumpPower As Single = -0.6F

		' Token: 0x04004500 RID: 17664
		Public hitKnockbackPower As Single = 300F

		' Token: 0x04004501 RID: 17665
		Public hitKnockbackEase As EaseUtils.EaseType = EaseUtils.EaseType.linear

		' Token: 0x04004502 RID: 17666
		Public knockUpStunTime As Single = 0.2F

		' Token: 0x04004503 RID: 17667
		<Range(0F, -3F)>
		Public pitKnockUpPower As Single = -1.5F

		' Token: 0x04004504 RID: 17668
		<Range(0F, -3F)>
		Public platformingPitKnockUpPower As Single = -1.5F

		' Token: 0x04004505 RID: 17669
		Public parryPower As Single = -1F

		' Token: 0x04004506 RID: 17670
		Public parryAttackBounce As Single = -1F

		' Token: 0x04004507 RID: 17671
		Public deathSpeed As Single = 5F

		' Token: 0x04004508 RID: 17672
		Public reviveKnockUpPower As Single = -1F

		' Token: 0x04004509 RID: 17673
		Public exKnockback As Single = 230F

		' Token: 0x0400450A RID: 17674
		Public superKnockUp As Single = -0.6F

		' Token: 0x0400450B RID: 17675
		Public superInvincibleKnockUp As Single = -1.2F
	End Class

	' Token: 0x02000A23 RID: 2595
	Public Class VelocityManager
		' Token: 0x06003DE7 RID: 15847 RVA: 0x0022239F File Offset: 0x0022079F
		Public Sub New(motor As LevelPlayerMotor, maxY As Single, yEase As EaseUtils.EaseType)
			Me.maxY = maxY
			Me.yEase = yEase
			Me.forces = New List(Of LevelPlayerMotor.VelocityManager.Force)()
		End Sub

		' Token: 0x1700054C RID: 1356
		' (get) Token: 0x06003DE8 RID: 15848 RVA: 0x002223C0 File Offset: 0x002207C0
		' (set) Token: 0x06003DE9 RID: 15849 RVA: 0x002223C8 File Offset: 0x002207C8
		Public Property yAxisForce As Boolean

		' Token: 0x1700054D RID: 1357
		' (get) Token: 0x06003DEA RID: 15850 RVA: 0x002223D1 File Offset: 0x002207D1
		' (set) Token: 0x06003DEB RID: 15851 RVA: 0x002223D9 File Offset: 0x002207D9
		Public Property GroundForce As Single

		' Token: 0x1700054E RID: 1358
		' (get) Token: 0x06003DEC RID: 15852 RVA: 0x002223E2 File Offset: 0x002207E2
		' (set) Token: 0x06003DED RID: 15853 RVA: 0x002223EA File Offset: 0x002207EA
		Public Property AirForce As Single

		' Token: 0x1700054F RID: 1359
		' (get) Token: 0x06003DEE RID: 15854 RVA: 0x002223F3 File Offset: 0x002207F3
		' (set) Token: 0x06003DEF RID: 15855 RVA: 0x00222416 File Offset: 0x00220816
		Public Property y As Single
			Get
				Me._y = Mathf.Clamp(Me._y, -10F, 1F)
				Return Me._y
			End Get
			Set(value As Single)
				Me._y = Mathf.Clamp(value, -10F, 1F)
			End Set
		End Property

		' Token: 0x06003DF0 RID: 15856 RVA: 0x00222430 File Offset: 0x00220830
		Public Sub Calculate()
			Me.GroundForce = 0F
			Me.AirForce = 0F
			For Each force As LevelPlayerMotor.VelocityManager.Force In Me.forces
				If force.enabled Then
					Dim type As LevelPlayerMotor.VelocityManager.Force.Type = force.type
					If type <> LevelPlayerMotor.VelocityManager.Force.Type.All Then
						If type <> LevelPlayerMotor.VelocityManager.Force.Type.Air Then
							If type = LevelPlayerMotor.VelocityManager.Force.Type.Ground Then
								Me.GroundForce += force.value
							End If
						Else
							Me.AirForce += force.value
						End If
					Else
						Me.AirForce += force.value
						Me.GroundForce += force.value
					End If
					If force.yAxisForce Then
						Me.yAxisForce = True
					End If
				End If
			Next
		End Sub

		' Token: 0x17000550 RID: 1360
		' (get) Token: 0x06003DF1 RID: 15857 RVA: 0x00222538 File Offset: 0x00220938
		Public ReadOnly Property Total As Vector2
			Get
				Dim num As Single = Me.y / 2F + 0.5F
				Dim vector As Vector2 = Nothing
				vector.y = EaseUtils.Ease(Me.yEase, Me.maxY, -Me.maxY, num) + Me.verticalDash
				vector.x += Me.move + Me.dash + Me.hit
				Return vector
			End Get
		End Property

		' Token: 0x06003DF2 RID: 15858 RVA: 0x002225AA File Offset: 0x002209AA
		Public Sub Clear()
			Me.move = 0F
			Me.dash = 0F
			Me.hit = 0F
			Me.y = 0F
		End Sub

		' Token: 0x06003DF3 RID: 15859 RVA: 0x002225D8 File Offset: 0x002209D8
		Public Sub AddForce(force As LevelPlayerMotor.VelocityManager.Force)
			If Me.forces.Contains(force) Then
				Return
			End If
			Me.forces.Add(force)
		End Sub

		' Token: 0x06003DF4 RID: 15860 RVA: 0x002225F8 File Offset: 0x002209F8
		Public Sub RemoveForce(force As LevelPlayerMotor.VelocityManager.Force)
			Me.yAxisForce = False
			If Me.forces.Contains(force) Then
				Me.forces.Remove(force)
			End If
		End Sub

		' Token: 0x0400450F RID: 17679
		Public move As Single

		' Token: 0x04004510 RID: 17680
		Public dash As Single

		' Token: 0x04004511 RID: 17681
		Public verticalDash As Single

		' Token: 0x04004512 RID: 17682
		Public hit As Single

		' Token: 0x04004513 RID: 17683
		Private forces As List(Of LevelPlayerMotor.VelocityManager.Force)

		' Token: 0x04004514 RID: 17684
		Private yEase As EaseUtils.EaseType

		' Token: 0x04004515 RID: 17685
		Private maxY As Single

		' Token: 0x04004516 RID: 17686
		Private _y As Single

		' Token: 0x02000A24 RID: 2596
		Public Class Force
			' Token: 0x06003DF5 RID: 15861 RVA: 0x0022261F File Offset: 0x00220A1F
			Public Sub New()
				Me.type = LevelPlayerMotor.VelocityManager.Force.Type.All
				Me.value = 0F
			End Sub

			' Token: 0x06003DF6 RID: 15862 RVA: 0x00222640 File Offset: 0x00220A40
			Public Sub New(type As LevelPlayerMotor.VelocityManager.Force.Type)
				Me.type = type
				Me.value = 0F
			End Sub

			' Token: 0x06003DF7 RID: 15863 RVA: 0x00222661 File Offset: 0x00220A61
			Public Sub New(type As LevelPlayerMotor.VelocityManager.Force.Type, force As Single)
				Me.type = type
				Me.value = force
			End Sub

			' Token: 0x06003DF8 RID: 15864 RVA: 0x0022267E File Offset: 0x00220A7E
			Public Sub New(type As LevelPlayerMotor.VelocityManager.Force.Type, force As Single, yAxis As Boolean)
				Me.type = type
				Me.value = force
				Me.yAxisForce = yAxis
			End Sub

			' Token: 0x04004517 RID: 17687
			Public yAxisForce As Boolean

			' Token: 0x04004518 RID: 17688
			Public enabled As Boolean = True

			' Token: 0x04004519 RID: 17689
			Public type As LevelPlayerMotor.VelocityManager.Force.Type

			' Token: 0x0400451A RID: 17690
			Public value As Single

			' Token: 0x02000A25 RID: 2597
			Public Enum Type
				' Token: 0x0400451C RID: 17692
				All
				' Token: 0x0400451D RID: 17693
				Ground
				' Token: 0x0400451E RID: 17694
				Air
			End Enum
		End Class
	End Class

	' Token: 0x02000A26 RID: 2598
	Public Class JumpManager
		' Token: 0x0400451F RID: 17695
		Public state As LevelPlayerMotor.JumpManager.State

		' Token: 0x04004520 RID: 17696
		Public timer As Single

		' Token: 0x04004521 RID: 17697
		Public timeSinceDownJump As Single = 1000F

		' Token: 0x04004522 RID: 17698
		Public timeInAir As Single

		' Token: 0x04004523 RID: 17699
		Public longestTimeInAir As Single

		' Token: 0x04004524 RID: 17700
		Public ableToLand As Boolean

		' Token: 0x04004525 RID: 17701
		Public floatTimer As Single

		' Token: 0x04004526 RID: 17702
		Public doubleJumped As Boolean

		' Token: 0x02000A27 RID: 2599
		Public Enum State
			' Token: 0x04004528 RID: 17704
			Ready
			' Token: 0x04004529 RID: 17705
			Hold
			' Token: 0x0400452A RID: 17706
			Used
		End Enum
	End Class

	' Token: 0x02000A28 RID: 2600
	Public Class DashManager
		' Token: 0x17000551 RID: 1361
		' (get) Token: 0x06003DFB RID: 15867 RVA: 0x002226C8 File Offset: 0x00220AC8
		Public ReadOnly Property IsDashing As Boolean
			Get
				Dim state As LevelPlayerMotor.DashManager.State = Me.state
				Return state = LevelPlayerMotor.DashManager.State.Start OrElse state = LevelPlayerMotor.DashManager.State.Dashing OrElse state = LevelPlayerMotor.DashManager.State.Ending
			End Get
		End Property

		' Token: 0x0400452B RID: 17707
		Public state As LevelPlayerMotor.DashManager.State

		' Token: 0x0400452C RID: 17708
		Public direction As Integer

		' Token: 0x0400452D RID: 17709
		Public timer As Single

		' Token: 0x0400452E RID: 17710
		Public Const DASH_COOLDOWN_DURATION As Single = 0.1F

		' Token: 0x0400452F RID: 17711
		Public timeSinceGroundDash As Single = 0.1F

		' Token: 0x04004530 RID: 17712
		Public groundDash As Boolean

		' Token: 0x04004531 RID: 17713
		Public chaliceParryCoolDownTimer As Single

		' Token: 0x04004532 RID: 17714
		Public chaliceParryCoolDown As Boolean

		' Token: 0x02000A29 RID: 2601
		Public Enum State
			' Token: 0x04004534 RID: 17716
			Ready
			' Token: 0x04004535 RID: 17717
			Start
			' Token: 0x04004536 RID: 17718
			Dashing
			' Token: 0x04004537 RID: 17719
			Ending
			' Token: 0x04004538 RID: 17720
			[End]
		End Enum
	End Class

	' Token: 0x02000A2A RID: 2602
	Public Class ParryManager
		' Token: 0x04004539 RID: 17721
		Public state As LevelPlayerMotor.ParryManager.State

		' Token: 0x02000A2B RID: 2603
		Public Enum State
			' Token: 0x0400453B RID: 17723
			Ready
			' Token: 0x0400453C RID: 17724
			NotReady
		End Enum
	End Class

	' Token: 0x02000A2C RID: 2604
	Public Class PlatformManager
		' Token: 0x06003DFD RID: 15869 RVA: 0x00222701 File Offset: 0x00220B01
		Public Sub New(motor As LevelPlayerMotor)
			Me.ignoredPlatforms = New List(Of Transform)()
			Me.motor = motor
		End Sub

		' Token: 0x17000552 RID: 1362
		' (get) Token: 0x06003DFE RID: 15870 RVA: 0x0022271B File Offset: 0x00220B1B
		Public ReadOnly Property OnPlatform As Boolean
			Get
				Return Me.motor.transform.parent IsNot Nothing
			End Get
		End Property

		' Token: 0x06003DFF RID: 15871 RVA: 0x00222733 File Offset: 0x00220B33
		Public Sub Ignore(platform As Transform)
			Me.StopCoroutine()
			Me.ignoreCoroutine = Me.ignorePlatform_cr(platform)
			Me.motor.StartCoroutine(Me.ignoreCoroutine)
		End Sub

		' Token: 0x06003E00 RID: 15872 RVA: 0x0022275A File Offset: 0x00220B5A
		Public Sub StopCoroutine()
			If Me.ignoreCoroutine IsNot Nothing Then
				Me.motor.StopCoroutine(Me.ignoreCoroutine)
			End If
			Me.ignoreCoroutine = Nothing
		End Sub

		' Token: 0x06003E01 RID: 15873 RVA: 0x0022277F File Offset: 0x00220B7F
		Public Sub Add(platform As Transform)
			Me.ignoredPlatforms.Add(platform)
		End Sub

		' Token: 0x06003E02 RID: 15874 RVA: 0x0022278D File Offset: 0x00220B8D
		Public Sub Remove(platform As Transform)
			Me.ignoredPlatforms.Remove(platform)
		End Sub

		' Token: 0x06003E03 RID: 15875 RVA: 0x0022279C File Offset: 0x00220B9C
		Public Function IsPlatformIgnored(platform As Transform) As Boolean
			Return Me.ignoredPlatforms.Contains(platform)
		End Function

		' Token: 0x06003E04 RID: 15876 RVA: 0x002227AA File Offset: 0x00220BAA
		Public Sub ResetAll()
			Me.StopCoroutine()
			Me.ignoredPlatforms = New List(Of Transform)()
		End Sub

		' Token: 0x06003E05 RID: 15877 RVA: 0x002227C0 File Offset: 0x00220BC0
		Private Iterator Function ignorePlatform_cr(platform As Transform) As IEnumerator
			Me.Add(platform)
			Yield CupheadTime.WaitForSeconds(Me.motor, Me.motor.properties.platformIgnoreTime)
			Me.Remove(platform)
			Return
		End Function

		' Token: 0x0400453D RID: 17725
		Private ignoredPlatforms As List(Of Transform)

		' Token: 0x0400453E RID: 17726
		Private motor As LevelPlayerMotor

		' Token: 0x0400453F RID: 17727
		Private ignoreCoroutine As IEnumerator
	End Class

	' Token: 0x02000A2D RID: 2605
	Public Class DirectionManager
		' Token: 0x06003E06 RID: 15878 RVA: 0x002228AC File Offset: 0x00220CAC
		Public Sub New()
			Me.Reset()
		End Sub

		' Token: 0x06003E07 RID: 15879 RVA: 0x002228E6 File Offset: 0x00220CE6
		Public Sub Reset()
			Me.up.Reset()
			Me.down.Reset()
			Me.left.Reset()
			Me.right.Reset()
		End Sub

		' Token: 0x04004540 RID: 17728
		Public up As LevelPlayerMotor.DirectionManager.Hit = New LevelPlayerMotor.DirectionManager.Hit()

		' Token: 0x04004541 RID: 17729
		Public down As LevelPlayerMotor.DirectionManager.Hit = New LevelPlayerMotor.DirectionManager.Hit()

		' Token: 0x04004542 RID: 17730
		Public left As LevelPlayerMotor.DirectionManager.Hit = New LevelPlayerMotor.DirectionManager.Hit()

		' Token: 0x04004543 RID: 17731
		Public right As LevelPlayerMotor.DirectionManager.Hit = New LevelPlayerMotor.DirectionManager.Hit()

		' Token: 0x02000A2E RID: 2606
		Public Class Hit
			' Token: 0x06003E08 RID: 15880 RVA: 0x00222914 File Offset: 0x00220D14
			Public Sub New()
				Me.Reset()
			End Sub

			' Token: 0x06003E09 RID: 15881 RVA: 0x00222922 File Offset: 0x00220D22
			Public Sub New(able As Boolean, pos As Vector2, gameObject As GameObject, distance As Single)
				Me.able = able
				Me.pos = pos
				Me.gameObject = gameObject
				Me.distance = distance
			End Sub

			' Token: 0x06003E0A RID: 15882 RVA: 0x00222947 File Offset: 0x00220D47
			Public Sub Reset()
				Me.able = True
				Me.pos = Vector2.zero
				Me.gameObject = Nothing
				Me.distance = -1F
			End Sub

			' Token: 0x04004544 RID: 17732
			Public able As Boolean

			' Token: 0x04004545 RID: 17733
			Public pos As Vector2

			' Token: 0x04004546 RID: 17734
			Public gameObject As GameObject

			' Token: 0x04004547 RID: 17735
			Public distance As Single
		End Class
	End Class

	' Token: 0x02000A2F RID: 2607
	Public Class HitManager
		' Token: 0x06003E0C RID: 15884 RVA: 0x00222975 File Offset: 0x00220D75
		Public Sub Reset()
			Me.state = LevelPlayerMotor.HitManager.State.Inactive
			Me.timer = 0F
			Me.direction = 0
		End Sub

		' Token: 0x04004548 RID: 17736
		Public state As LevelPlayerMotor.HitManager.State

		' Token: 0x04004549 RID: 17737
		Public timer As Single

		' Token: 0x0400454A RID: 17738
		Public direction As Integer

		' Token: 0x02000A30 RID: 2608
		Public Enum State
			' Token: 0x0400454C RID: 17740
			Inactive
			' Token: 0x0400454D RID: 17741
			Hit
			' Token: 0x0400454E RID: 17742
			KnockedUp
		End Enum
	End Class

	' Token: 0x02000A31 RID: 2609
	Public Class SuperManager
		' Token: 0x0400454F RID: 17743
		Public state As LevelPlayerMotor.SuperManager.State

		' Token: 0x02000A32 RID: 2610
		Public Enum State
			' Token: 0x04004551 RID: 17745
			Ready
			' Token: 0x04004552 RID: 17746
			Ex
			' Token: 0x04004553 RID: 17747
			Super
		End Enum
	End Class

	' Token: 0x02000A33 RID: 2611
	Public Class BoundsManager
		' Token: 0x06003E0E RID: 15886 RVA: 0x00222998 File Offset: 0x00220D98
		Public Sub New(motor As LevelPlayerMotor)
			Me.Motor = motor
			Me.transform = motor.transform
			Me.boxCollider = TryCast(Me.transform.GetComponent(Of Collider2D)(), BoxCollider2D)
		End Sub

		' Token: 0x17000553 RID: 1363
		' (get) Token: 0x06003E0F RID: 15887 RVA: 0x002229C9 File Offset: 0x00220DC9
		' (set) Token: 0x06003E10 RID: 15888 RVA: 0x002229D1 File Offset: 0x00220DD1
		Public Property Motor As LevelPlayerMotor

		' Token: 0x17000554 RID: 1364
		' (get) Token: 0x06003E11 RID: 15889 RVA: 0x002229DC File Offset: 0x00220DDC
		Public ReadOnly Property Top As Vector3
			Get
				Return New Vector3(Me.Center.x, Me.Center.y + Me.boxCollider.size.y / 2F, 0F)
			End Get
		End Property

		' Token: 0x17000555 RID: 1365
		' (get) Token: 0x06003E12 RID: 15890 RVA: 0x00222A2C File Offset: 0x00220E2C
		Public ReadOnly Property TopLeft As Vector3
			Get
				Return New Vector3(Me.Center.x - Me.boxCollider.size.x / 2F, Me.Center.y + Me.boxCollider.size.y / 2F, 0F)
			End Get
		End Property

		' Token: 0x17000556 RID: 1366
		' (get) Token: 0x06003E13 RID: 15891 RVA: 0x00222A94 File Offset: 0x00220E94
		Public ReadOnly Property TopRight As Vector3
			Get
				Return New Vector3(Me.Center.x + Me.boxCollider.size.x / 2F, Me.Center.y + Me.boxCollider.size.y / 2F, 0F)
			End Get
		End Property

		' Token: 0x17000557 RID: 1367
		' (get) Token: 0x06003E14 RID: 15892 RVA: 0x00222AFC File Offset: 0x00220EFC
		Public ReadOnly Property CenterLeft As Vector3
			Get
				Return New Vector3(Me.Center.x - Me.boxCollider.size.x / 2F, Me.Center.y, 0F)
			End Get
		End Property

		' Token: 0x17000558 RID: 1368
		' (get) Token: 0x06003E15 RID: 15893 RVA: 0x00222B4C File Offset: 0x00220F4C
		Public ReadOnly Property CenterRight As Vector3
			Get
				Return New Vector3(Me.Center.x + Me.boxCollider.size.x / 2F, Me.Center.y, 0F)
			End Get
		End Property

		' Token: 0x17000559 RID: 1369
		' (get) Token: 0x06003E16 RID: 15894 RVA: 0x00222B9C File Offset: 0x00220F9C
		Public ReadOnly Property Center As Vector2
			Get
				Return Me.transform.position + New Vector2(Me.boxCollider.offset.x, Me.Motor.GravityReversalMultiplier * Me.boxCollider.offset.y)
			End Get
		End Property

		' Token: 0x1700055A RID: 1370
		' (get) Token: 0x06003E17 RID: 15895 RVA: 0x00222BF8 File Offset: 0x00220FF8
		Public ReadOnly Property Bottom As Vector3
			Get
				Return New Vector3(Me.Center.x, Me.Center.y - Me.boxCollider.size.y / 2F, 0F)
			End Get
		End Property

		' Token: 0x1700055B RID: 1371
		' (get) Token: 0x06003E18 RID: 15896 RVA: 0x00222C48 File Offset: 0x00221048
		Public ReadOnly Property BottomLeft As Vector3
			Get
				Return New Vector3(Me.Center.x - Me.boxCollider.size.x / 2F, Me.Center.y - Me.boxCollider.size.y / 2F, 0F)
			End Get
		End Property

		' Token: 0x1700055C RID: 1372
		' (get) Token: 0x06003E19 RID: 15897 RVA: 0x00222CB0 File Offset: 0x002210B0
		Public ReadOnly Property BottomRight As Vector3
			Get
				Return New Vector3(Me.Center.x + Me.boxCollider.size.x / 2F, Me.Center.y - Me.boxCollider.size.y / 2F, 0F)
			End Get
		End Property

		' Token: 0x1700055D RID: 1373
		' (get) Token: 0x06003E1A RID: 15898 RVA: 0x00222D18 File Offset: 0x00221118
		Public ReadOnly Property TopY As Single
			Get
				Return Me.Top.y - Me.transform.position.y
			End Get
		End Property

		' Token: 0x1700055E RID: 1374
		' (get) Token: 0x06003E1B RID: 15899 RVA: 0x00222D48 File Offset: 0x00221148
		Public ReadOnly Property BottomY As Single
			Get
				Return Me.Bottom.y - Me.transform.position.y
			End Get
		End Property

		' Token: 0x04004554 RID: 17748
		Private transform As Transform

		' Token: 0x04004555 RID: 17749
		Private boxCollider As BoxCollider2D
	End Class
End Class
