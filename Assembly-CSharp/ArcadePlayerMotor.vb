Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Diagnostics
Imports UnityEngine

' Token: 0x020009DF RID: 2527
Public Class ArcadePlayerMotor
	Inherits AbstractArcadePlayerComponent

	' Token: 0x170004F9 RID: 1273
	' (get) Token: 0x06003B97 RID: 15255 RVA: 0x002156B3 File Offset: 0x00213AB3
	' (set) Token: 0x06003B98 RID: 15256 RVA: 0x002156BB File Offset: 0x00213ABB
	Public Property LookDirection As Trilean2

	' Token: 0x170004FA RID: 1274
	' (get) Token: 0x06003B99 RID: 15257 RVA: 0x002156C4 File Offset: 0x00213AC4
	' (set) Token: 0x06003B9A RID: 15258 RVA: 0x002156CC File Offset: 0x00213ACC
	Public Property TrueLookDirection As Trilean2

	' Token: 0x170004FB RID: 1275
	' (get) Token: 0x06003B9B RID: 15259 RVA: 0x002156D5 File Offset: 0x00213AD5
	' (set) Token: 0x06003B9C RID: 15260 RVA: 0x002156DD File Offset: 0x00213ADD
	Public Property MoveDirection As Trilean2

	' Token: 0x170004FC RID: 1276
	' (get) Token: 0x06003B9D RID: 15261 RVA: 0x002156E6 File Offset: 0x00213AE6
	Public ReadOnly Property JumpState As ArcadePlayerMotor.JumpManager.State
		Get
			Return Me.jumpManager.state
		End Get
	End Property

	' Token: 0x170004FD RID: 1277
	' (get) Token: 0x06003B9E RID: 15262 RVA: 0x002156F3 File Offset: 0x00213AF3
	Public ReadOnly Property Dashing As Boolean
		Get
			Return Me.dashManager.IsDashing
		End Get
	End Property

	' Token: 0x170004FE RID: 1278
	' (get) Token: 0x06003B9F RID: 15263 RVA: 0x00215700 File Offset: 0x00213B00
	Public ReadOnly Property DashDirection As Integer
		Get
			Return Me.dashManager.direction
		End Get
	End Property

	' Token: 0x170004FF RID: 1279
	' (get) Token: 0x06003BA0 RID: 15264 RVA: 0x0021570D File Offset: 0x00213B0D
	' (set) Token: 0x06003BA1 RID: 15265 RVA: 0x00215715 File Offset: 0x00213B15
	Public Property Locked As Boolean

	' Token: 0x17000500 RID: 1280
	' (get) Token: 0x06003BA2 RID: 15266 RVA: 0x0021571E File Offset: 0x00213B1E
	' (set) Token: 0x06003BA3 RID: 15267 RVA: 0x00215726 File Offset: 0x00213B26
	Public Property Grounded As Boolean

	' Token: 0x17000501 RID: 1281
	' (get) Token: 0x06003BA4 RID: 15268 RVA: 0x0021572F File Offset: 0x00213B2F
	' (set) Token: 0x06003BA5 RID: 15269 RVA: 0x00215737 File Offset: 0x00213B37
	Public Property Parrying As Boolean

	' Token: 0x17000502 RID: 1282
	' (get) Token: 0x06003BA6 RID: 15270 RVA: 0x00215740 File Offset: 0x00213B40
	Public ReadOnly Property IsHit As Boolean
		Get
			Return Me.hitManager.state = ArcadePlayerMotor.HitManager.State.Hit
		End Get
	End Property

	' Token: 0x17000503 RID: 1283
	' (get) Token: 0x06003BA7 RID: 15271 RVA: 0x00215750 File Offset: 0x00213B50
	Public ReadOnly Property IsUsingSuperOrEx As Boolean
		Get
			Return Me.superManager.state = ArcadePlayerMotor.SuperManager.State.Super OrElse Me.superManager.state = ArcadePlayerMotor.SuperManager.State.Ex
		End Get
	End Property

	' Token: 0x14000077 RID: 119
	' (add) Token: 0x06003BA8 RID: 15272 RVA: 0x00215774 File Offset: 0x00213B74
	' (remove) Token: 0x06003BA9 RID: 15273 RVA: 0x002157AC File Offset: 0x00213BAC
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnGroundedEvent As Action

	' Token: 0x14000078 RID: 120
	' (add) Token: 0x06003BAA RID: 15274 RVA: 0x002157E4 File Offset: 0x00213BE4
	' (remove) Token: 0x06003BAB RID: 15275 RVA: 0x0021581C File Offset: 0x00213C1C
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnJumpEvent As Action

	' Token: 0x14000079 RID: 121
	' (add) Token: 0x06003BAC RID: 15276 RVA: 0x00215854 File Offset: 0x00213C54
	' (remove) Token: 0x06003BAD RID: 15277 RVA: 0x0021588C File Offset: 0x00213C8C
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnParryEvent As Action

	' Token: 0x1400007A RID: 122
	' (add) Token: 0x06003BAE RID: 15278 RVA: 0x002158C4 File Offset: 0x00213CC4
	' (remove) Token: 0x06003BAF RID: 15279 RVA: 0x002158FC File Offset: 0x00213CFC
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnParrySuccess As Action

	' Token: 0x1400007B RID: 123
	' (add) Token: 0x06003BB0 RID: 15280 RVA: 0x00215934 File Offset: 0x00213D34
	' (remove) Token: 0x06003BB1 RID: 15281 RVA: 0x0021596C File Offset: 0x00213D6C
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnHitEvent As Action

	' Token: 0x1400007C RID: 124
	' (add) Token: 0x06003BB2 RID: 15282 RVA: 0x002159A4 File Offset: 0x00213DA4
	' (remove) Token: 0x06003BB3 RID: 15283 RVA: 0x002159DC File Offset: 0x00213DDC
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnDashStartEvent As Action

	' Token: 0x1400007D RID: 125
	' (add) Token: 0x06003BB4 RID: 15284 RVA: 0x00215A14 File Offset: 0x00213E14
	' (remove) Token: 0x06003BB5 RID: 15285 RVA: 0x00215A4C File Offset: 0x00213E4C
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnDashEndEvent As Action

	' Token: 0x06003BB6 RID: 15286 RVA: 0x00215A84 File Offset: 0x00213E84
	Protected Overrides Sub OnAwake()
		MyBase.OnAwake()
		Me.properties = New ArcadePlayerMotor.Properties()
		Me.MoveDirection = New Trilean2(0, 0)
		Me.LookDirection = New Trilean2(1, 0)
		Me.TrueLookDirection = New Trilean2(1, 0)
		Me.velocityManager = New ArcadePlayerMotor.VelocityManager(Me, Me.properties.maxSpeedY, Me.properties.yEase)
		Me.jumpManager = New ArcadePlayerMotor.JumpManager()
		Me.dashManager = New ArcadePlayerMotor.DashManager()
		Me.parryManager = New ArcadePlayerMotor.ParryManager()
		Me.directionManager = New ArcadePlayerMotor.DirectionManager()
		Me.platformManager = New ArcadePlayerMotor.PlatformManager(Me)
		Me.hitManager = New ArcadePlayerMotor.HitManager()
		Me.superManager = New ArcadePlayerMotor.SuperManager()
		Me.boundsManager = New ArcadePlayerMotor.BoundsManager(MyBase.transform)
		AddHandler MyBase.player.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		Me.allowInput = True
		Me.allowFalling = True
	End Sub

	' Token: 0x06003BB7 RID: 15287 RVA: 0x00215B74 File Offset: 0x00213F74
	Private Sub Start()
		AddHandler MyBase.player.weaponManager.OnExStart, AddressOf Me.StartEx
		AddHandler MyBase.player.weaponManager.OnSuperStart, AddressOf Me.StartSuper
		AddHandler MyBase.player.weaponManager.OnExFire, AddressOf Me.OnExFired
		AddHandler MyBase.player.weaponManager.OnSuperEnd, AddressOf Me.OnSuperEnd
		AddHandler MyBase.player.weaponManager.OnExEnd, AddressOf Me.ResetSuperAndEx
		AddHandler MyBase.player.weaponManager.OnSuperEnd, AddressOf Me.ResetSuperAndEx
		AddHandler MyBase.player.OnReviveEvent, AddressOf Me.OnRevive
	End Sub

	' Token: 0x06003BB8 RID: 15288 RVA: 0x00215C40 File Offset: 0x00214040
	Private Sub FixedUpdate()
		If MyBase.player.IsDead Then
			Return
		End If
		If MyBase.player.controlScheme <> ArcadePlayerController.ControlScheme.Rocket Then
			Me.HandleLooking()
		End If
		If MyBase.player.weaponManager.FreezePosition Then
			Return
		End If
		If MyBase.player.controlScheme = ArcadePlayerController.ControlScheme.Rocket Then
			Me.RocketInput()
		Else
			Me.HandleInput()
			If MyBase.player.controlScheme = ArcadePlayerController.ControlScheme.Normal AndAlso Me.allowFalling Then
				Me.HandleFalling()
			End If
			Me.Move()
			Me.HandleRaycasts()
			Dim vector As Vector2 = MyBase.transform.localPosition
			Dim vector2 As Vector2 = vector - Me.lastPositionFixed
			vector2.x = CSng(CInt(vector2.x))
			vector2.y = CSng(CInt(vector2.y))
			Me.MoveDirection = vector2
			Me.lastPositionFixed = New Vector2(vector.x, vector.y)
			Me.lastPosition = MyBase.transform.position
		End If
		Me.ClampToBounds()
	End Sub

	' Token: 0x06003BB9 RID: 15289 RVA: 0x00215D59 File Offset: 0x00214159
	Private Sub LateUpdate()
		Me.ClampToBounds()
	End Sub

	' Token: 0x06003BBA RID: 15290 RVA: 0x00215D61 File Offset: 0x00214161
	Public Sub DisableInput()
		Me.allowInput = False
		Me.Locked = False
		Me.MoveDirection = New Trilean2(0, 0)
		Me.velocityManager.move = 0F
	End Sub

	' Token: 0x06003BBB RID: 15291 RVA: 0x00215D8E File Offset: 0x0021418E
	Public Sub EnableInput()
		Me.allowInput = True
	End Sub

	' Token: 0x06003BBC RID: 15292 RVA: 0x00215D98 File Offset: 0x00214198
	Public Sub DisableGravity()
		Me.allowFalling = False
		Me.MoveDirection = New Trilean2(Me.MoveDirection.x, 0)
		Me.velocityManager.y = 0F
	End Sub

	' Token: 0x06003BBD RID: 15293 RVA: 0x00215DDB File Offset: 0x002141DB
	Public Sub EnableGravity()
		Me.allowFalling = True
		Me.velocityManager.y = 0F
	End Sub

	' Token: 0x06003BBE RID: 15294 RVA: 0x00215DF4 File Offset: 0x002141F4
	Private Function BoxCast(size As Vector2, direction As Vector2, layerMask As Integer) As RaycastHit2D
		Return Me.BoxCast(size, direction, layerMask, Vector2.zero)
	End Function

	' Token: 0x06003BBF RID: 15295 RVA: 0x00215E04 File Offset: 0x00214204
	Private Function BoxCast(size As Vector2, direction As Vector2, layerMask As Integer, offset As Vector2) As RaycastHit2D
		Return Physics2D.BoxCast(MyBase.player.colliderManager.Center + offset, size, 0F, direction, 2000F, layerMask)
	End Function

	' Token: 0x06003BC0 RID: 15296 RVA: 0x00215E2F File Offset: 0x0021422F
	Private Function CircleCast(radius As Single, direction As Vector2, layerMask As Integer) As RaycastHit2D
		Return Physics2D.CircleCast(MyBase.player.colliderManager.Center, radius, direction, 2000F, layerMask)
	End Function

	' Token: 0x06003BC1 RID: 15297 RVA: 0x00215E4E File Offset: 0x0021424E
	Private Function DoesRaycastHitHaveCollider(hit As RaycastHit2D) As Boolean
		Return hit.collider IsNot Nothing
	End Function

	' Token: 0x06003BC2 RID: 15298 RVA: 0x00215E60 File Offset: 0x00214260
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		If Application.isPlaying Then
			Gizmos.color = Color.blue
			Gizmos.DrawSphere(MyBase.player.center, 5F)
			Gizmos.color = Color.red
			Gizmos.DrawSphere(MyBase.transform.position, 5F)
		End If
	End Sub

	' Token: 0x06003BC3 RID: 15299 RVA: 0x00215EBC File Offset: 0x002142BC
	Private Sub HandleRaycasts()
		Dim flag As Boolean = True
		If Me.directionManager IsNot Nothing AndAlso Me.directionManager.up IsNot Nothing Then
			flag = Me.directionManager.up.able
		End If
		Dim colliderManager As ArcadePlayerColliderManager = MyBase.player.colliderManager
		Me.directionManager.Reset()
		Dim raycastHit2D As RaycastHit2D = Me.BoxCast(New Vector2(1F, colliderManager.Height), Vector2.left, Me.wallMask)
		Dim raycastHit2D2 As RaycastHit2D = Me.BoxCast(New Vector2(1F, colliderManager.Height), Vector2.right, Me.wallMask)
		Dim raycastHit2D3 As RaycastHit2D = Me.BoxCast(New Vector2(colliderManager.Width, 1F), Vector2.up, Me.ceilingMask)
		Me.RaycastObstacle(Me.directionManager.left, raycastHit2D, MyBase.player.colliderManager.DefaultWidth / 2F, ArcadePlayerMotor.RaycastAxis.X)
		Me.RaycastObstacle(Me.directionManager.right, raycastHit2D2, MyBase.player.colliderManager.DefaultWidth / 2F, ArcadePlayerMotor.RaycastAxis.X)
		Me.RaycastObstacle(Me.directionManager.up, raycastHit2D3, MyBase.player.colliderManager.Height / 2F, ArcadePlayerMotor.RaycastAxis.Y)
		Dim vector As Vector2 = colliderManager.Center + New Vector2(0F, colliderManager.DefaultHeight)
		Dim array As RaycastHit2D() = Physics2D.BoxCastAll(vector, New Vector2(colliderManager.Width, 1F), 0F, Vector2.down, 1000F, Me.groundMask)
		Me.directionManager.down.pos = New Vector2(colliderManager.Center.x, -10000F)
		For Each raycastHit2D4 As RaycastHit2D In array
			If raycastHit2D4.point.y > Me.directionManager.down.pos.y Then
				If raycastHit2D4.point.y <= 20F + MyBase.transform.position.y Then
					Dim num As Single = Math.Abs(MyBase.transform.position.y - raycastHit2D4.point.y)
					Me.directionManager.down.pos = New Vector2(vector.x, raycastHit2D4.point.y)
					Me.directionManager.down.gameObject = raycastHit2D4.collider.gameObject
					Me.directionManager.down.distance = num
					If num < 20F Then
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
			If Not Me.directionManager.up.able AndAlso Me.directionManager.up.able <> flag Then
				Me.OnHitCeiling()
			End If
		End If
		Dim num2 As Single = MyBase.transform.position.y - Me.directionManager.down.pos.y
		If Me.Grounded AndAlso num2 > 30F Then
			Me.LeaveGround()
		End If
	End Sub

	' Token: 0x06003BC4 RID: 15300 RVA: 0x00216280 File Offset: 0x00214680
	Private Function RaycastObstacle(directionProperties As ArcadePlayerMotor.DirectionManager.Hit, raycastHit As RaycastHit2D, maxDistance As Single, axis As ArcadePlayerMotor.RaycastAxis) As Single
		If Not Me.DoesRaycastHitHaveCollider(raycastHit) Then
			Return 1000F
		End If
		Dim num As Single = If((axis <> ArcadePlayerMotor.RaycastAxis.X), Math.Abs(MyBase.player.colliderManager.Center.y - raycastHit.point.y), Math.Abs(MyBase.player.colliderManager.Center.x - raycastHit.point.x))
		directionProperties.pos = raycastHit.point
		directionProperties.gameObject = raycastHit.collider.gameObject
		directionProperties.distance = num
		If num < maxDistance Then
			directionProperties.able = False
		End If
		Return num
	End Function

	' Token: 0x06003BC5 RID: 15301 RVA: 0x0021633C File Offset: 0x0021473C
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
		Me.jumpManager.state = ArcadePlayerMotor.JumpManager.State.Ready
		Me.parryManager.state = ArcadePlayerMotor.ParryManager.State.Ready
		Me.velocityManager.y = 0F
		Me.platformManager.ResetAll()
		Me.Grounded = True
		Me.Parrying = False
		Me.dashManager.timeSinceGroundDash = 1000F
		If Me.OnGroundedEvent IsNot Nothing Then
			Me.OnGroundedEvent()
		End If
	End Sub

	' Token: 0x06003BC6 RID: 15302 RVA: 0x00216444 File Offset: 0x00214844
	Private Sub LeaveGround()
		Me.Grounded = False
		Me.jumpManager.ableToLand = False
		Me.velocityManager.y = 0F
		Me.ClearParent()
		If Me.jumpManager.state = ArcadePlayerMotor.JumpManager.State.Ready Then
			Me.jumpManager.state = ArcadePlayerMotor.JumpManager.State.Used
		End If
	End Sub

	' Token: 0x06003BC7 RID: 15303 RVA: 0x00216498 File Offset: 0x00214898
	Private Sub OnHitCeiling()
		If Me.jumpManager.ableToLand Then
			Return
		End If
		Me.velocityManager.y = 0F
		Me.directionManager.left.able = True
		Me.directionManager.right.able = True
	End Sub

	' Token: 0x06003BC8 RID: 15304 RVA: 0x002164E8 File Offset: 0x002148E8
	Private Sub Move()
		Me.velocityManager.Calculate()
		Dim vector As Vector3 = Me.velocityManager.Total
		If Me.hitManager.state <> ArcadePlayerMotor.HitManager.State.Hit AndAlso Me.superManager.state = ArcadePlayerMotor.SuperManager.State.Ready Then
			If Me.Grounded Then
				vector.x += Me.velocityManager.GroundForce
			Else
				vector.x += Me.velocityManager.AirForce
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
		MyBase.transform.localPosition += vector * CupheadTime.FixedDelta
		If Me.Grounded Then
			Dim vector2 As Vector2 = MyBase.transform.position
			vector2.y = Me.directionManager.down.pos.y
			MyBase.transform.position = vector2
			Dim component As LevelPlatform = Me.directionManager.down.gameObject.GetComponent(Of LevelPlatform)()
			If component Is Nothing AndAlso MyBase.transform.parent IsNot Nothing Then
				Me.ClearParent()
			ElseIf component IsNot Nothing AndAlso (MyBase.transform.parent Is Nothing OrElse component.gameObject IsNot MyBase.transform.parent.gameObject) Then
				Me.ClearParent()
				component.AddChild(MyBase.transform)
			End If
		End If
	End Sub

	' Token: 0x06003BC9 RID: 15305 RVA: 0x002167C0 File Offset: 0x00214BC0
	Private Sub ClampToBounds()
		Dim cupheadBounds As CupheadBounds = New CupheadBounds()
		cupheadBounds.left = Me.directionManager.left.pos.x + MyBase.player.colliderManager.Width / 2F
		cupheadBounds.right = Me.directionManager.right.pos.x - MyBase.player.colliderManager.Width / 2F
		cupheadBounds.top = Me.directionManager.up.pos.y - Me.boundsManager.TopY
		cupheadBounds.bottom = Me.directionManager.down.pos.y - Me.boundsManager.BottomY
		Dim position As Vector3 = MyBase.transform.position
		If Not Me.directionManager.left.able AndAlso MyBase.transform.position.x < cupheadBounds.left Then
			position.x = cupheadBounds.left
		End If
		If Not Me.directionManager.right.able AndAlso MyBase.transform.position.x > cupheadBounds.right Then
			position.x = cupheadBounds.right
		End If
		If Not Me.directionManager.up.able AndAlso MyBase.transform.position.y > cupheadBounds.top Then
			position.y = cupheadBounds.top
		End If
		position.x = Mathf.Clamp(position.x, CSng(Level.Current.Left) + MyBase.player.colliderManager.Width / 2F, CSng(Level.Current.Right) - MyBase.player.colliderManager.Width / 2F)
		If MyBase.player.controlScheme <> ArcadePlayerController.ControlScheme.Normal Then
			position.y = Mathf.Clamp(position.y, CSng(Level.Current.Ground) + MyBase.player.colliderManager.Height / 2F, CSng(Level.Current.Ceiling) - MyBase.player.colliderManager.Height / 2F)
		End If
		MyBase.transform.position = position
	End Sub

	' Token: 0x06003BCA RID: 15306 RVA: 0x00216A1C File Offset: 0x00214E1C
	Private Sub ResetSuperAndEx()
		If Me.superManager.state = ArcadePlayerMotor.SuperManager.State.Ready Then
			Return
		End If
		If Me.jumpManager.state <> ArcadePlayerMotor.JumpManager.State.Ready Then
			Me.jumpManager.state = ArcadePlayerMotor.JumpManager.State.Used
		End If
		MyBase.StopCoroutine(Me.exMove_cr())
		Me.superManager.state = ArcadePlayerMotor.SuperManager.State.Ready
		Me.EnableInput()
		Me.EnableGravity()
	End Sub

	' Token: 0x06003BCB RID: 15307 RVA: 0x00216A7A File Offset: 0x00214E7A
	Private Sub StartSuper()
		Me.LeaveGround()
		Me.jumpManager.state = ArcadePlayerMotor.JumpManager.State.Used
		Me.jumpManager.timer = 0F
		Me.velocityManager.y = 0F
	End Sub

	' Token: 0x06003BCC RID: 15308 RVA: 0x00216AAE File Offset: 0x00214EAE
	Public Sub OnSuperEnd()
		If Me.Grounded Then
			Me.jumpManager.state = ArcadePlayerMotor.JumpManager.State.Ready
		Else
			Me.LeaveGround()
			Me.velocityManager.y = Me.properties.superKnockUp
		End If
	End Sub

	' Token: 0x06003BCD RID: 15309 RVA: 0x00216AE8 File Offset: 0x00214EE8
	Private Sub StartEx()
		Me.exFirePose = MyBase.player.weaponManager.GetDirectionPose()
		Me.DisableInput()
		Me.DisableGravity()
		Me.superManager.state = ArcadePlayerMotor.SuperManager.State.Ex
	End Sub

	' Token: 0x06003BCE RID: 15310 RVA: 0x00216B18 File Offset: 0x00214F18
	Private Sub OnExFired()
		If Me.exFirePose = ArcadePlayerWeaponManager.Pose.Up OrElse Me.exFirePose = ArcadePlayerWeaponManager.Pose.Down Then
			MyBase.StartCoroutine(Me.exDelay_cr())
		Else
			MyBase.StartCoroutine(Me.exMove_cr())
		End If
	End Sub

	' Token: 0x06003BCF RID: 15311 RVA: 0x00216B54 File Offset: 0x00214F54
	Private Iterator Function exDelay_cr() As IEnumerator
		While Me.superManager.state <> ArcadePlayerMotor.SuperManager.State.Ready
			Yield Nothing
		End While
		Me.EnableInput()
		Me.EnableGravity()
		Me.superManager.state = ArcadePlayerMotor.SuperManager.State.Ready
		Return
	End Function

	' Token: 0x06003BD0 RID: 15312 RVA: 0x00216B70 File Offset: 0x00214F70
	Private Iterator Function exMove_cr() As IEnumerator
		While Me.superManager.state <> ArcadePlayerMotor.SuperManager.State.Ready
			Me.velocityManager.move = CSng((Me.TrueLookDirection.x * -1)) * Me.properties.exKnockback
			Yield Nothing
		End While
		Me.EnableInput()
		Me.EnableGravity()
		Me.superManager.state = ArcadePlayerMotor.SuperManager.State.Ready
		Return
	End Function

	' Token: 0x06003BD1 RID: 15313 RVA: 0x00216B8C File Offset: 0x00214F8C
	Private Sub HandleInput()
		If Not MyBase.player.levelStarted Then
			Return
		End If
		Me.timeSinceInputBuffered += CupheadTime.FixedDelta
		Me.dashManager.timeSinceGroundDash += CupheadTime.FixedDelta
		If(Not Me.allowInput OrElse Me.dashManager.state <> ArcadePlayerMotor.DashManager.State.Ready) AndAlso Me.hitManager.state = ArcadePlayerMotor.HitManager.State.Inactive Then
			Me.BufferInputs()
		End If
		If Not Me.allowInput Then
			Return
		End If
		If Not Me.HandleDash() Then
			If Me.hitManager.state = ArcadePlayerMotor.HitManager.State.Hit Then
				Me.HandleHit()
			Else
				If MyBase.player.controlScheme = ArcadePlayerController.ControlScheme.Normal Then
					Me.HandleParry()
					Me.HandleJumping()
				ElseIf MyBase.player.controlScheme = ArcadePlayerController.ControlScheme.Jetpack Then
					Me.HandleJetpackJump()
				End If
				Me.HandleWalking()
			End If
		End If
	End Sub

	' Token: 0x06003BD2 RID: 15314 RVA: 0x00216C77 File Offset: 0x00215077
	Private Sub BufferInput(input As ArcadePlayerMotor.BufferedInput)
		Me.bufferedInput = input
		Me.timeSinceInputBuffered = 0F
	End Sub

	' Token: 0x06003BD3 RID: 15315 RVA: 0x00216C8C File Offset: 0x0021508C
	Public Sub BufferInputs()
		If MyBase.player.input.actions.GetButtonDown(2) Then
			Me.BufferInput(ArcadePlayerMotor.BufferedInput.Jump)
		ElseIf MyBase.player.input.actions.GetButtonDown(7) AndAlso Not Me.dashManager.IsDashing Then
			Me.BufferInput(ArcadePlayerMotor.BufferedInput.Dash)
		ElseIf MyBase.player.input.actions.GetButtonDown(4) Then
			Me.BufferInput(ArcadePlayerMotor.BufferedInput.Super)
		End If
	End Sub

	' Token: 0x06003BD4 RID: 15316 RVA: 0x00216D19 File Offset: 0x00215119
	Public Sub ClearBufferedInput()
		Me.timeSinceInputBuffered = 0.134F
	End Sub

	' Token: 0x06003BD5 RID: 15317 RVA: 0x00216D26 File Offset: 0x00215126
	Public Function HasBufferedInput(input As ArcadePlayerMotor.BufferedInput) As Boolean
		Return Me.bufferedInput = input AndAlso Me.timeSinceInputBuffered < 0.134F
	End Function

	' Token: 0x06003BD6 RID: 15318 RVA: 0x00216D44 File Offset: 0x00215144
	Private Sub HandleJumping()
		If Me.jumpManager.state = ArcadePlayerMotor.JumpManager.State.Ready AndAlso (MyBase.player.input.actions.GetButtonDown(2) OrElse Me.HasBufferedInput(ArcadePlayerMotor.BufferedInput.Jump)) Then
			If Me.LookDirection.y < 0 AndAlso Me.Grounded AndAlso MyBase.transform.parent IsNot Nothing Then
				Dim component As LevelPlatform = MyBase.transform.parent.GetComponent(Of LevelPlatform)()
				If component.canFallThrough Then
					Me.platformManager.Ignore(MyBase.transform.parent)
					Me.jumpManager.state = ArcadePlayerMotor.JumpManager.State.Used
					Me.LeaveGround()
					Me.jumpManager.timeSinceDownJump = 0F
					Return
				End If
			End If
			AudioManager.Play("player_jump")
			Me.jumpManager.state = ArcadePlayerMotor.JumpManager.State.Hold
			Me.LeaveGround()
			Me.velocityManager.y = Me.properties.jumpPower
			Me.jumpManager.timer = CupheadTime.FixedDelta
			If Me.OnJumpEvent IsNot Nothing Then
				Me.OnJumpEvent()
			End If
		End If
		If Me.jumpManager.state = ArcadePlayerMotor.JumpManager.State.Hold Then
			If Not Me.directionManager.up.able OrElse (Me.jumpManager.timer >= Me.properties.jumpHoldMin AndAlso (MyBase.player.input.actions.GetButtonUp(2) OrElse Not MyBase.player.input.actions.GetButton(2))) OrElse Me.jumpManager.timer >= Me.properties.jumpHoldMax Then
				Me.jumpManager.state = ArcadePlayerMotor.JumpManager.State.Used
				Me.jumpManager.timer = 0F
			End If
			Me.velocityManager.y = Me.properties.jumpPower
			Me.jumpManager.timer += CupheadTime.FixedDelta
		End If
		Me.jumpManager.timeSinceDownJump += CupheadTime.FixedDelta
	End Sub

	' Token: 0x06003BD7 RID: 15319 RVA: 0x00216F64 File Offset: 0x00215364
	Private Sub HandleParry()
		If Me.IsHit Then
			Return
		End If
		If Me.parryManager.state = ArcadePlayerMotor.ParryManager.State.Ready AndAlso (MyBase.player.input.actions.GetButtonDown(2) OrElse Me.HasBufferedInput(ArcadePlayerMotor.BufferedInput.Jump)) AndAlso Me.jumpManager.state <> ArcadePlayerMotor.JumpManager.State.Ready AndAlso Not Me.IsHit Then
			Me.hitManager.state = ArcadePlayerMotor.HitManager.State.Inactive
			Me.parryManager.state = ArcadePlayerMotor.ParryManager.State.NotReady
			If Me.dashManager.IsDashing Then
				Me.dashManager.state = ArcadePlayerMotor.DashManager.State.[End]
			End If
			Me.Parrying = True
			If Me.OnParryEvent IsNot Nothing Then
				Me.OnParryEvent()
			End If
		End If
	End Sub

	' Token: 0x06003BD8 RID: 15320 RVA: 0x00217020 File Offset: 0x00215420
	Public Sub OnParryComplete()
		Me.LeaveGround()
		Me.parryManager.state = ArcadePlayerMotor.ParryManager.State.Ready
		Me.velocityManager.y = Me.properties.parryPower
		If Me.OnParrySuccess IsNot Nothing Then
			Me.OnParrySuccess()
		End If
	End Sub

	' Token: 0x06003BD9 RID: 15321 RVA: 0x00217060 File Offset: 0x00215460
	Public Sub OnParryHit()
		MyBase.StartCoroutine(Me.parryHit_cr())
	End Sub

	' Token: 0x06003BDA RID: 15322 RVA: 0x0021706F File Offset: 0x0021546F
	Public Sub OnParryCanceled()
		Me.Parrying = False
	End Sub

	' Token: 0x06003BDB RID: 15323 RVA: 0x00217078 File Offset: 0x00215478
	Public Sub OnParryAnimEnd()
		Me.Parrying = False
	End Sub

	' Token: 0x06003BDC RID: 15324 RVA: 0x00217084 File Offset: 0x00215484
	Private Function HandleDash() As Boolean
		If Me.dashManager.state = ArcadePlayerMotor.DashManager.State.Ready AndAlso (Not Me.Grounded OrElse Me.dashManager.timeSinceGroundDash > 0.1F) AndAlso (MyBase.player.input.actions.GetButtonDown(7) OrElse Me.HasBufferedInput(ArcadePlayerMotor.BufferedInput.Dash)) Then
			AudioManager.Play("player_dash")
			Me.dashManager.state = ArcadePlayerMotor.DashManager.State.Start
			Me.dashManager.direction = Me.TrueLookDirection.x
			If Me.jumpManager.state = ArcadePlayerMotor.JumpManager.State.Hold Then
				Me.jumpManager.state = ArcadePlayerMotor.JumpManager.State.Used
			End If
			If Me.OnDashStartEvent IsNot Nothing Then
				Me.OnDashStartEvent()
			End If
			Me.velocityManager.move = 0F
			Return True
		End If
		If Me.dashManager.state = ArcadePlayerMotor.DashManager.State.Start Then
			Me.dashManager.state = ArcadePlayerMotor.DashManager.State.Dashing
		End If
		If Me.dashManager.state = ArcadePlayerMotor.DashManager.State.Dashing Then
			Me.velocityManager.dash = Me.properties.dashSpeed * CSng(Me.dashManager.direction)
			Me.dashManager.timer += CupheadTime.FixedDelta
			Me.velocityManager.y = 0F
			Me.LookDirection = New Trilean2(Me.LookDirection.x, Me.dashManager.direction)
			If Me.dashManager.timer >= Me.properties.dashTime Then
				Me.DashComplete()
			End If
			If Not Me.Grounded Then
				Me.jumpManager.ableToLand = True
			End If
			Return True
		End If
		If Me.dashManager.state = ArcadePlayerMotor.DashManager.State.[End] Then
			If Me.Grounded Then
				Me.dashManager.state = ArcadePlayerMotor.DashManager.State.Ready
				If Me.dashManager.groundDash Then
					Me.dashManager.timeSinceGroundDash = 0F
				End If
			Else
				Me.dashManager.groundDash = False
			End If
		End If
		Return False
	End Function

	' Token: 0x06003BDD RID: 15325 RVA: 0x00217298 File Offset: 0x00215698
	Private Sub DashComplete()
		Me.dashManager.state = ArcadePlayerMotor.DashManager.State.[End]
		Me.dashManager.timer = 0F
		Me.velocityManager.dash = 0F
		If Me.OnDashEndEvent IsNot Nothing Then
			Me.OnDashEndEvent()
		End If
	End Sub

	' Token: 0x06003BDE RID: 15326 RVA: 0x002172E7 File Offset: 0x002156E7
	Private Sub HandleLocked()
		If MyBase.player.input.actions.GetButton(6) AndAlso Me.Grounded Then
			Me.Locked = True
		Else
			Me.Locked = False
		End If
	End Sub

	' Token: 0x06003BDF RID: 15327 RVA: 0x00217324 File Offset: 0x00215724
	Private Sub HandleWalking()
		Dim num As Single = If((MyBase.player.controlScheme <> ArcadePlayerController.ControlScheme.Normal), Me.properties.jetpackMoveSpeed, Me.properties.moveSpeed)
		Dim num2 As Single = CSng(MyBase.player.input.GetAxisInt(PlayerInput.Axis.X, False, False)) * num
		Me.velocityManager.move = num2
	End Sub

	' Token: 0x06003BE0 RID: 15328 RVA: 0x00217380 File Offset: 0x00215780
	Private Sub HandleLooking()
		If MyBase.player.levelStarted AndAlso Me.allowInput Then
			Dim axisInt As Integer = MyBase.player.input.GetAxisInt(PlayerInput.Axis.X, False, False)
			Dim axisInt2 As Integer = MyBase.player.input.GetAxisInt(PlayerInput.Axis.Y, False, False)
			Me.LookDirection = New Trilean2(axisInt, axisInt2)
		End If
		Dim num As Integer = Me.TrueLookDirection.x
		Dim num2 As Integer = Me.TrueLookDirection.y
		If Me.LookDirection.x <> 0 Then
			num = Me.LookDirection.x
		End If
		num2 = Me.LookDirection.y
		Me.TrueLookDirection = New Trilean2(num, num2)
	End Sub

	' Token: 0x06003BE1 RID: 15329 RVA: 0x0021745C File Offset: 0x0021585C
	Private Sub HandleFalling()
		If Me.Grounded OrElse Me.dashManager.IsDashing Then
			Return
		End If
		If Level.Current.LevelTime < 0.2F Then
			Return
		End If
		Dim num As Single = Me.properties.timeToMaxY * 60F
		Dim num2 As Single = Me.properties.maxSpeedY / num * CupheadTime.FixedDelta
		Me.velocityManager.y += num2
		Me.jumpManager.ableToLand = Me.velocityManager.y > 0F
	End Sub

	' Token: 0x06003BE2 RID: 15330 RVA: 0x002174F0 File Offset: 0x002158F0
	Public Function GetTimeUntilLand() As Single
		If Me.Grounded Then
			Return 0F
		End If
		Dim num As Single = Me.properties.timeToMaxY * 60F
		Dim num2 As Single = Me.properties.maxSpeedY / num
		Dim num3 As Single = (CSng(Level.Current.Ground) - MyBase.transform.position.y) / (Me.velocityManager.maxY * 2F)
		Return-(Me.velocityManager.y - Mathf.Sqrt(Me.velocityManager.y * Me.velocityManager.y - 2F * num2 * num3)) / num2
	End Function

	' Token: 0x06003BE3 RID: 15331 RVA: 0x00217595 File Offset: 0x00215995
	Public Function GetTimeUntilDashEnd() As Single
		If Not Me.Dashing Then
			Return 0F
		End If
		Return Me.properties.dashTime - Me.dashManager.timer
	End Function

	' Token: 0x06003BE4 RID: 15332 RVA: 0x002175C0 File Offset: 0x002159C0
	Private Sub HandleHit()
		If Me.hitManager.state <> ArcadePlayerMotor.HitManager.State.Hit Then
			Return
		End If
		If Me.hitManager.timer > Me.properties.hitStunTime Then
			Me.hitManager.state = ArcadePlayerMotor.HitManager.State.Inactive
			Me.velocityManager.hit = 0F
		Else
			Dim num As Single = Me.hitManager.timer / Me.properties.hitStunTime
			Me.velocityManager.hit = EaseUtils.Ease(Me.properties.hitKnockbackEase, Me.properties.hitKnockbackPower, 0F, num) * CSng(Me.hitManager.direction)
			Me.hitManager.timer += CupheadTime.FixedDelta
		End If
	End Sub

	' Token: 0x06003BE5 RID: 15333 RVA: 0x00217684 File Offset: 0x00215A84
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.hitManager.state = ArcadePlayerMotor.HitManager.State.Hit
		If Me.OnHitEvent IsNot Nothing Then
			Me.OnHitEvent()
		End If
		Me.DashComplete()
		Me.velocityManager.Clear()
		Me.ResetSuperAndEx()
		Dim num As Integer = Me.TrueLookDirection.x * -1
		Me.hitManager.direction = num
		Me.LeaveGround()
		Me.velocityManager.y = Me.properties.hitJumpPower
		Me.hitManager.timer = 0F
	End Sub

	' Token: 0x06003BE6 RID: 15334 RVA: 0x00217718 File Offset: 0x00215B18
	Public Sub OnRevive(pos As Vector3)
		MyBase.transform.position = pos
		Me.hitManager.state = ArcadePlayerMotor.HitManager.State.KnockedUp
		Me.DashComplete()
		Me.velocityManager.Clear()
		Me.ResetSuperAndEx()
		Me.hitManager.direction = 0
		Me.LeaveGround()
		Me.velocityManager.y = Me.properties.reviveKnockUpPower
		Me.hitManager.timer = 0F
	End Sub

	' Token: 0x06003BE7 RID: 15335 RVA: 0x0021778C File Offset: 0x00215B8C
	Private Sub RocketInput()
		Me.HandleRocketRotation()
		Me.HandleRocketBoost()
		If Not Me.HandleDash() AndAlso Me.hitManager.state = ArcadePlayerMotor.HitManager.State.Hit Then
			Me.HandleHit()
		End If
	End Sub

	' Token: 0x06003BE8 RID: 15336 RVA: 0x002177CC File Offset: 0x00215BCC
	Private Sub HandleJetpackJump()
		If MyBase.player.input.actions.GetButtonDown(2) Then
			Me.jumpManager.state = ArcadePlayerMotor.JumpManager.State.Hold
			Me.LeaveGround()
			Me.velocityManager.y = Me.properties.jetpackAcceleration
			Me.jumpManager.timer = CupheadTime.FixedDelta
		ElseIf Me.velocityManager.y < Me.properties.jetpackGravityMax Then
			Me.velocityManager.y += Me.properties.jetpackGravity
		End If
	End Sub

	' Token: 0x06003BE9 RID: 15337 RVA: 0x0021786C File Offset: 0x00215C6C
	Private Sub HandleRocketBoost()
		If MyBase.player.input.actions.GetButton(2) Then
			If Me.rocketSpeed < Me.properties.moveSpeed Then
				Me.rocketSpeed += Me.properties.rocketAcceleration
			Else
				Me.rocketSpeed = Me.properties.moveSpeed
			End If
		ElseIf Me.rocketSpeed > 0F Then
			Me.rocketSpeed -= Me.properties.rocketAcceleration
		Else
			Me.rocketSpeed = 0F
		End If
		MyBase.transform.position += MyBase.transform.up.normalized * Me.rocketSpeed * CupheadTime.FixedDelta
	End Sub

	' Token: 0x06003BEA RID: 15338 RVA: 0x00217953 File Offset: 0x00215D53
	Private Sub HandleRocketRotation()
		MyBase.transform.Rotate(0F, 0F, Me.properties.rocketRotation * CSng((-CSng(MyBase.player.input.GetAxisInt(PlayerInput.Axis.X, False, False)))) * CupheadTime.FixedDelta, Space.Self)
	End Sub

	' Token: 0x06003BEB RID: 15339 RVA: 0x00217992 File Offset: 0x00215D92
	Public Sub AddForce(force As ArcadePlayerMotor.VelocityManager.Force)
		Me.velocityManager.AddForce(force)
	End Sub

	' Token: 0x06003BEC RID: 15340 RVA: 0x002179A0 File Offset: 0x00215DA0
	Public Sub RemoveForce(force As ArcadePlayerMotor.VelocityManager.Force)
		Me.velocityManager.RemoveForce(force)
	End Sub

	' Token: 0x06003BED RID: 15341 RVA: 0x002179AE File Offset: 0x00215DAE
	Private Sub ClearParent()
		If MyBase.transform.parent IsNot Nothing Then
			MyBase.transform.parent.GetComponent(Of LevelPlatform)().OnPlayerExit(MyBase.transform)
		End If
		MyBase.transform.parent = Nothing
	End Sub

	' Token: 0x06003BEE RID: 15342 RVA: 0x002179F0 File Offset: 0x00215DF0
	Private Iterator Function parryHit_cr() As IEnumerator
		CupheadTime.GlobalSpeed = 1F
		Me.velocityManager.Clear()
		Yield Nothing
		PauseManager.Unpause()
		Me.velocityManager.Clear()
		CupheadTime.GlobalSpeed = 1F
		Return
	End Function

	' Token: 0x04004323 RID: 17187
	<SerializeField()>
	Private properties As ArcadePlayerMotor.Properties

	' Token: 0x04004324 RID: 17188
	Private lastPositionFixed As Vector2

	' Token: 0x04004325 RID: 17189
	Private lastPosition As Vector2

	' Token: 0x04004326 RID: 17190
	Private velocityManager As ArcadePlayerMotor.VelocityManager

	' Token: 0x04004327 RID: 17191
	Private jumpManager As ArcadePlayerMotor.JumpManager

	' Token: 0x04004328 RID: 17192
	Private dashManager As ArcadePlayerMotor.DashManager

	' Token: 0x04004329 RID: 17193
	Private parryManager As ArcadePlayerMotor.ParryManager

	' Token: 0x0400432A RID: 17194
	Private directionManager As ArcadePlayerMotor.DirectionManager

	' Token: 0x0400432B RID: 17195
	Private platformManager As ArcadePlayerMotor.PlatformManager

	' Token: 0x0400432C RID: 17196
	Private hitManager As ArcadePlayerMotor.HitManager

	' Token: 0x0400432D RID: 17197
	Private superManager As ArcadePlayerMotor.SuperManager

	' Token: 0x0400432E RID: 17198
	Private boundsManager As ArcadePlayerMotor.BoundsManager

	' Token: 0x0400432F RID: 17199
	Private allowInput As Boolean

	' Token: 0x04004330 RID: 17200
	Private allowFalling As Boolean

	' Token: 0x04004331 RID: 17201
	Private rocketSpeed As Single

	' Token: 0x04004339 RID: 17209
	Private Const RAY_DISTANCE As Single = 2000F

	' Token: 0x0400433A RID: 17210
	Private Const MAX_GROUNDED_FALL_DISTANCE As Single = 30F

	' Token: 0x0400433B RID: 17211
	Private wallMask As Integer = 262144

	' Token: 0x0400433C RID: 17212
	Private ceilingMask As Integer = 524288

	' Token: 0x0400433D RID: 17213
	Private groundMask As Integer = 1048576

	' Token: 0x0400433E RID: 17214
	Private exFirePose As ArcadePlayerWeaponManager.Pose

	' Token: 0x0400433F RID: 17215
	Private bufferedInput As ArcadePlayerMotor.BufferedInput

	' Token: 0x04004340 RID: 17216
	Private timeSinceInputBuffered As Single = 0.134F

	' Token: 0x020009E0 RID: 2528
	Private Enum RaycastAxis
		' Token: 0x04004342 RID: 17218
		X
		' Token: 0x04004343 RID: 17219
		Y
	End Enum

	' Token: 0x020009E1 RID: 2529
	Public Enum BufferedInput
		' Token: 0x04004345 RID: 17221
		Jump
		' Token: 0x04004346 RID: 17222
		Dash
		' Token: 0x04004347 RID: 17223
		Super
	End Enum

	' Token: 0x020009E2 RID: 2530
	Public Class Properties
		' Token: 0x04004348 RID: 17224
		Public rocketRotation As Single = 300F

		' Token: 0x04004349 RID: 17225
		Public rocketMaxSpeed As Single = 400F

		' Token: 0x0400434A RID: 17226
		Public rocketAcceleration As Single = 2.5F

		' Token: 0x0400434B RID: 17227
		Public jetpackAcceleration As Single = -0.1F

		' Token: 0x0400434C RID: 17228
		Public jetpackGravity As Single = 0.001F

		' Token: 0x0400434D RID: 17229
		Public jetpackGravityMax As Single = 0.1F

		' Token: 0x0400434E RID: 17230
		Private Const speedScale As Single = 0.75F

		' Token: 0x0400434F RID: 17231
		Public moveSpeed As Single = 367.5F

		' Token: 0x04004350 RID: 17232
		Public jetpackMoveSpeed As Single = 187.5F

		' Token: 0x04004351 RID: 17233
		Public maxSpeedY As Single = 1215F

		' Token: 0x04004352 RID: 17234
		Public timeToMaxY As Single = 7.3F

		' Token: 0x04004353 RID: 17235
		Public yEase As EaseUtils.EaseType = EaseUtils.EaseType.linear

		' Token: 0x04004354 RID: 17236
		Public jumpHoldMin As Single = 0.01F

		' Token: 0x04004355 RID: 17237
		Public jumpHoldMax As Single = 0.16F

		' Token: 0x04004356 RID: 17238
		<Range(0F, -1F)>
		Public jumpPower As Single = -0.56624997F

		' Token: 0x04004357 RID: 17239
		Public dashSpeed As Single = 825F

		' Token: 0x04004358 RID: 17240
		Public dashTime As Single = 0.3F

		' Token: 0x04004359 RID: 17241
		Public dashEndTime As Single = 0.21F

		' Token: 0x0400435A RID: 17242
		Public dashEase As EaseUtils.EaseType = EaseUtils.EaseType.easeOutSine

		' Token: 0x0400435B RID: 17243
		Public platformIgnoreTime As Single = 1F

		' Token: 0x0400435C RID: 17244
		Public hitStunTime As Single = 0.3F

		' Token: 0x0400435D RID: 17245
		Public hitFalloff As Single = 0.25F

		' Token: 0x0400435E RID: 17246
		<Range(0F, -1F)>
		Public hitJumpPower As Single = -0.6F

		' Token: 0x0400435F RID: 17247
		Public hitKnockbackPower As Single = 225F

		' Token: 0x04004360 RID: 17248
		Public hitKnockbackEase As EaseUtils.EaseType = EaseUtils.EaseType.linear

		' Token: 0x04004361 RID: 17249
		Public knockUpStunTime As Single = 0.2F

		' Token: 0x04004362 RID: 17250
		Public parryPower As Single = -0.75F

		' Token: 0x04004363 RID: 17251
		Public deathSpeed As Single = 3.75F

		' Token: 0x04004364 RID: 17252
		Public reviveKnockUpPower As Single = -0.75F

		' Token: 0x04004365 RID: 17253
		Public exKnockback As Single = 172.5F

		' Token: 0x04004366 RID: 17254
		Public superKnockUp As Single = -0.45000002F
	End Class

	' Token: 0x020009E3 RID: 2531
	Public Class VelocityManager
		' Token: 0x06003BF0 RID: 15344 RVA: 0x00217B60 File Offset: 0x00215F60
		Public Sub New(motor As ArcadePlayerMotor, maxY As Single, yEase As EaseUtils.EaseType)
			Me.maxY = maxY
			Me.yEase = yEase
			Me.forces = New List(Of ArcadePlayerMotor.VelocityManager.Force)()
		End Sub

		' Token: 0x17000504 RID: 1284
		' (get) Token: 0x06003BF1 RID: 15345 RVA: 0x00217B81 File Offset: 0x00215F81
		' (set) Token: 0x06003BF2 RID: 15346 RVA: 0x00217B89 File Offset: 0x00215F89
		Public Property GroundForce As Single

		' Token: 0x17000505 RID: 1285
		' (get) Token: 0x06003BF3 RID: 15347 RVA: 0x00217B92 File Offset: 0x00215F92
		' (set) Token: 0x06003BF4 RID: 15348 RVA: 0x00217B9A File Offset: 0x00215F9A
		Public Property AirForce As Single

		' Token: 0x17000506 RID: 1286
		' (get) Token: 0x06003BF5 RID: 15349 RVA: 0x00217BA3 File Offset: 0x00215FA3
		' (set) Token: 0x06003BF6 RID: 15350 RVA: 0x00217BC6 File Offset: 0x00215FC6
		Public Property y As Single
			Get
				Me._y = Mathf.Clamp(Me._y, -10F, 1F)
				Return Me._y
			End Get
			Set(value As Single)
				Me._y = Mathf.Clamp(value, -10F, 1F)
			End Set
		End Property

		' Token: 0x06003BF7 RID: 15351 RVA: 0x00217BE0 File Offset: 0x00215FE0
		Public Sub Calculate()
			Me.GroundForce = 0F
			Me.AirForce = 0F
			For Each force As ArcadePlayerMotor.VelocityManager.Force In Me.forces
				If force.enabled Then
					Dim type As ArcadePlayerMotor.VelocityManager.Force.Type = force.type
					If type <> ArcadePlayerMotor.VelocityManager.Force.Type.All Then
						If type <> ArcadePlayerMotor.VelocityManager.Force.Type.Air Then
							If type = ArcadePlayerMotor.VelocityManager.Force.Type.Ground Then
								Me.GroundForce += force.value
							End If
						Else
							Me.AirForce += force.value
						End If
					Else
						Me.AirForce += force.value
						Me.GroundForce += force.value
					End If
				End If
			Next
		End Sub

		' Token: 0x17000507 RID: 1287
		' (get) Token: 0x06003BF8 RID: 15352 RVA: 0x00217CD8 File Offset: 0x002160D8
		Public ReadOnly Property Total As Vector2
			Get
				Dim num As Single = Me.y / 2F + 0.5F
				Dim vector As Vector2 = Nothing
				vector.y = EaseUtils.Ease(Me.yEase, Me.maxY, -Me.maxY, num)
				vector.x += Me.move + Me.dash + Me.hit
				Return vector
			End Get
		End Property

		' Token: 0x06003BF9 RID: 15353 RVA: 0x00217D43 File Offset: 0x00216143
		Public Sub Clear()
			Me.move = 0F
			Me.dash = 0F
			Me.hit = 0F
			Me.y = 0F
		End Sub

		' Token: 0x06003BFA RID: 15354 RVA: 0x00217D71 File Offset: 0x00216171
		Public Sub AddForce(force As ArcadePlayerMotor.VelocityManager.Force)
			If Me.forces.Contains(force) Then
				Return
			End If
			Me.forces.Add(force)
		End Sub

		' Token: 0x06003BFB RID: 15355 RVA: 0x00217D91 File Offset: 0x00216191
		Public Sub RemoveForce(force As ArcadePlayerMotor.VelocityManager.Force)
			If Me.forces.Contains(force) Then
				Me.forces.Remove(force)
			End If
		End Sub

		' Token: 0x04004369 RID: 17257
		Public move As Single

		' Token: 0x0400436A RID: 17258
		Public dash As Single

		' Token: 0x0400436B RID: 17259
		Public hit As Single

		' Token: 0x0400436C RID: 17260
		Private forces As List(Of ArcadePlayerMotor.VelocityManager.Force)

		' Token: 0x0400436D RID: 17261
		Private yEase As EaseUtils.EaseType

		' Token: 0x0400436E RID: 17262
		Public maxY As Single

		' Token: 0x0400436F RID: 17263
		Private _y As Single

		' Token: 0x020009E4 RID: 2532
		Public Class Force
			' Token: 0x06003BFC RID: 15356 RVA: 0x00217DB1 File Offset: 0x002161B1
			Public Sub New()
				Me.type = ArcadePlayerMotor.VelocityManager.Force.Type.All
				Me.value = 0F
			End Sub

			' Token: 0x06003BFD RID: 15357 RVA: 0x00217DD2 File Offset: 0x002161D2
			Public Sub New(type As ArcadePlayerMotor.VelocityManager.Force.Type)
				Me.type = type
				Me.value = 0F
			End Sub

			' Token: 0x06003BFE RID: 15358 RVA: 0x00217DF3 File Offset: 0x002161F3
			Public Sub New(type As ArcadePlayerMotor.VelocityManager.Force.Type, force As Single)
				Me.type = type
				Me.value = force
			End Sub

			' Token: 0x04004370 RID: 17264
			Public enabled As Boolean = True

			' Token: 0x04004371 RID: 17265
			Public type As ArcadePlayerMotor.VelocityManager.Force.Type

			' Token: 0x04004372 RID: 17266
			Public value As Single

			' Token: 0x020009E5 RID: 2533
			Public Enum Type
				' Token: 0x04004374 RID: 17268
				All
				' Token: 0x04004375 RID: 17269
				Ground
				' Token: 0x04004376 RID: 17270
				Air
			End Enum
		End Class
	End Class

	' Token: 0x020009E6 RID: 2534
	Public Class JumpManager
		' Token: 0x04004377 RID: 17271
		Public state As ArcadePlayerMotor.JumpManager.State

		' Token: 0x04004378 RID: 17272
		Public timer As Single

		' Token: 0x04004379 RID: 17273
		Public timeSinceDownJump As Single = 1000F

		' Token: 0x0400437A RID: 17274
		Public ableToLand As Boolean

		' Token: 0x020009E7 RID: 2535
		Public Enum State
			' Token: 0x0400437C RID: 17276
			Ready
			' Token: 0x0400437D RID: 17277
			Hold
			' Token: 0x0400437E RID: 17278
			Used
		End Enum
	End Class

	' Token: 0x020009E8 RID: 2536
	Public Class DashManager
		' Token: 0x17000508 RID: 1288
		' (get) Token: 0x06003C01 RID: 15361 RVA: 0x00217E38 File Offset: 0x00216238
		Public ReadOnly Property IsDashing As Boolean
			Get
				Dim state As ArcadePlayerMotor.DashManager.State = Me.state
				Return state = ArcadePlayerMotor.DashManager.State.Start OrElse state = ArcadePlayerMotor.DashManager.State.Dashing OrElse state = ArcadePlayerMotor.DashManager.State.Ending
			End Get
		End Property

		' Token: 0x0400437F RID: 17279
		Public state As ArcadePlayerMotor.DashManager.State

		' Token: 0x04004380 RID: 17280
		Public direction As Integer

		' Token: 0x04004381 RID: 17281
		Public timer As Single

		' Token: 0x04004382 RID: 17282
		Public Const DASH_COOLDOWN_DURATION As Single = 0.1F

		' Token: 0x04004383 RID: 17283
		Public timeSinceGroundDash As Single = 0.1F

		' Token: 0x04004384 RID: 17284
		Public groundDash As Boolean

		' Token: 0x020009E9 RID: 2537
		Public Enum State
			' Token: 0x04004386 RID: 17286
			Ready
			' Token: 0x04004387 RID: 17287
			Start
			' Token: 0x04004388 RID: 17288
			Dashing
			' Token: 0x04004389 RID: 17289
			Ending
			' Token: 0x0400438A RID: 17290
			[End]
		End Enum
	End Class

	' Token: 0x020009EA RID: 2538
	Public Class ParryManager
		' Token: 0x0400438B RID: 17291
		Public state As ArcadePlayerMotor.ParryManager.State

		' Token: 0x020009EB RID: 2539
		Public Enum State
			' Token: 0x0400438D RID: 17293
			Ready
			' Token: 0x0400438E RID: 17294
			NotReady
		End Enum
	End Class

	' Token: 0x020009EC RID: 2540
	Public Class PlatformManager
		' Token: 0x06003C03 RID: 15363 RVA: 0x00217E71 File Offset: 0x00216271
		Public Sub New(motor As ArcadePlayerMotor)
			Me.ignoredPlatforms = New List(Of Transform)()
			Me.motor = motor
		End Sub

		' Token: 0x17000509 RID: 1289
		' (get) Token: 0x06003C04 RID: 15364 RVA: 0x00217E8B File Offset: 0x0021628B
		Public ReadOnly Property OnPlatform As Boolean
			Get
				Return Me.motor.transform.parent IsNot Nothing
			End Get
		End Property

		' Token: 0x06003C05 RID: 15365 RVA: 0x00217EA3 File Offset: 0x002162A3
		Public Sub Ignore(platform As Transform)
			Me.StopCoroutine()
			Me.ignoreCoroutine = Me.ignorePlatform_cr(platform)
			Me.motor.StartCoroutine(Me.ignoreCoroutine)
		End Sub

		' Token: 0x06003C06 RID: 15366 RVA: 0x00217ECA File Offset: 0x002162CA
		Public Sub StopCoroutine()
			If Me.ignoreCoroutine IsNot Nothing Then
				Me.motor.StopCoroutine(Me.ignoreCoroutine)
			End If
			Me.ignoreCoroutine = Nothing
		End Sub

		' Token: 0x06003C07 RID: 15367 RVA: 0x00217EEF File Offset: 0x002162EF
		Public Sub Add(platform As Transform)
			Me.ignoredPlatforms.Add(platform)
		End Sub

		' Token: 0x06003C08 RID: 15368 RVA: 0x00217EFD File Offset: 0x002162FD
		Public Sub Remove(platform As Transform)
			Me.ignoredPlatforms.Remove(platform)
		End Sub

		' Token: 0x06003C09 RID: 15369 RVA: 0x00217F0C File Offset: 0x0021630C
		Public Function IsPlatformIgnored(platform As Transform) As Boolean
			Return Me.ignoredPlatforms.Contains(platform)
		End Function

		' Token: 0x06003C0A RID: 15370 RVA: 0x00217F1A File Offset: 0x0021631A
		Public Sub ResetAll()
			Me.StopCoroutine()
			Me.ignoredPlatforms = New List(Of Transform)()
		End Sub

		' Token: 0x06003C0B RID: 15371 RVA: 0x00217F30 File Offset: 0x00216330
		Private Iterator Function ignorePlatform_cr(platform As Transform) As IEnumerator
			Me.Add(platform)
			Yield CupheadTime.WaitForSeconds(Me.motor, Me.motor.properties.platformIgnoreTime)
			Me.Remove(platform)
			Return
		End Function

		' Token: 0x0400438F RID: 17295
		Private ignoredPlatforms As List(Of Transform)

		' Token: 0x04004390 RID: 17296
		Private motor As ArcadePlayerMotor

		' Token: 0x04004391 RID: 17297
		Private ignoreCoroutine As IEnumerator
	End Class

	' Token: 0x020009ED RID: 2541
	Public Class DirectionManager
		' Token: 0x06003C0C RID: 15372 RVA: 0x0021801C File Offset: 0x0021641C
		Public Sub New()
			Me.Reset()
		End Sub

		' Token: 0x06003C0D RID: 15373 RVA: 0x00218056 File Offset: 0x00216456
		Public Sub Reset()
			Me.up.Reset()
			Me.down.Reset()
			Me.left.Reset()
			Me.right.Reset()
		End Sub

		' Token: 0x04004392 RID: 17298
		Public up As ArcadePlayerMotor.DirectionManager.Hit = New ArcadePlayerMotor.DirectionManager.Hit()

		' Token: 0x04004393 RID: 17299
		Public down As ArcadePlayerMotor.DirectionManager.Hit = New ArcadePlayerMotor.DirectionManager.Hit()

		' Token: 0x04004394 RID: 17300
		Public left As ArcadePlayerMotor.DirectionManager.Hit = New ArcadePlayerMotor.DirectionManager.Hit()

		' Token: 0x04004395 RID: 17301
		Public right As ArcadePlayerMotor.DirectionManager.Hit = New ArcadePlayerMotor.DirectionManager.Hit()

		' Token: 0x020009EE RID: 2542
		Public Class Hit
			' Token: 0x06003C0E RID: 15374 RVA: 0x00218084 File Offset: 0x00216484
			Public Sub New()
				Me.Reset()
			End Sub

			' Token: 0x06003C0F RID: 15375 RVA: 0x00218092 File Offset: 0x00216492
			Public Sub New(able As Boolean, pos As Vector2, gameObject As GameObject, distance As Single)
				Me.able = able
				Me.pos = pos
				Me.gameObject = gameObject
				Me.distance = distance
			End Sub

			' Token: 0x06003C10 RID: 15376 RVA: 0x002180B7 File Offset: 0x002164B7
			Public Sub Reset()
				Me.able = True
				Me.pos = Vector2.zero
				Me.gameObject = Nothing
				Me.distance = -1F
			End Sub

			' Token: 0x04004396 RID: 17302
			Public able As Boolean

			' Token: 0x04004397 RID: 17303
			Public pos As Vector2

			' Token: 0x04004398 RID: 17304
			Public gameObject As GameObject

			' Token: 0x04004399 RID: 17305
			Public distance As Single
		End Class
	End Class

	' Token: 0x020009EF RID: 2543
	Public Class HitManager
		' Token: 0x06003C12 RID: 15378 RVA: 0x002180E5 File Offset: 0x002164E5
		Public Sub Reset()
			Me.state = ArcadePlayerMotor.HitManager.State.Inactive
			Me.timer = 0F
			Me.direction = 0
		End Sub

		' Token: 0x0400439A RID: 17306
		Public state As ArcadePlayerMotor.HitManager.State

		' Token: 0x0400439B RID: 17307
		Public timer As Single

		' Token: 0x0400439C RID: 17308
		Public direction As Integer

		' Token: 0x020009F0 RID: 2544
		Public Enum State
			' Token: 0x0400439E RID: 17310
			Inactive
			' Token: 0x0400439F RID: 17311
			Hit
			' Token: 0x040043A0 RID: 17312
			KnockedUp
		End Enum
	End Class

	' Token: 0x020009F1 RID: 2545
	Public Class SuperManager
		' Token: 0x040043A1 RID: 17313
		Public state As ArcadePlayerMotor.SuperManager.State

		' Token: 0x020009F2 RID: 2546
		Public Enum State
			' Token: 0x040043A3 RID: 17315
			Ready
			' Token: 0x040043A4 RID: 17316
			Ex
			' Token: 0x040043A5 RID: 17317
			Super
		End Enum
	End Class

	' Token: 0x020009F3 RID: 2547
	Public Class BoundsManager
		' Token: 0x06003C14 RID: 15380 RVA: 0x00218108 File Offset: 0x00216508
		Public Sub New(playerTransform As Transform)
			Me.transform = playerTransform
			Me.boxCollider = TryCast(Me.transform.GetComponent(Of Collider2D)(), BoxCollider2D)
		End Sub

		' Token: 0x1700050A RID: 1290
		' (get) Token: 0x06003C15 RID: 15381 RVA: 0x00218130 File Offset: 0x00216530
		Public ReadOnly Property Top As Vector3
			Get
				Return New Vector3(Me.Center.x, Me.Center.y + Me.boxCollider.size.y / 2F, 0F)
			End Get
		End Property

		' Token: 0x1700050B RID: 1291
		' (get) Token: 0x06003C16 RID: 15382 RVA: 0x00218180 File Offset: 0x00216580
		Public ReadOnly Property TopLeft As Vector3
			Get
				Return New Vector3(Me.Center.x - Me.boxCollider.size.x / 2F, Me.Center.y + Me.boxCollider.size.y / 2F, 0F)
			End Get
		End Property

		' Token: 0x1700050C RID: 1292
		' (get) Token: 0x06003C17 RID: 15383 RVA: 0x002181E8 File Offset: 0x002165E8
		Public ReadOnly Property TopRight As Vector3
			Get
				Return New Vector3(Me.Center.x + Me.boxCollider.size.x / 2F, Me.Center.y + Me.boxCollider.size.y / 2F, 0F)
			End Get
		End Property

		' Token: 0x1700050D RID: 1293
		' (get) Token: 0x06003C18 RID: 15384 RVA: 0x00218250 File Offset: 0x00216650
		Public ReadOnly Property CenterLeft As Vector3
			Get
				Return New Vector3(Me.Center.x - Me.boxCollider.size.x / 2F, Me.Center.y, 0F)
			End Get
		End Property

		' Token: 0x1700050E RID: 1294
		' (get) Token: 0x06003C19 RID: 15385 RVA: 0x002182A0 File Offset: 0x002166A0
		Public ReadOnly Property CenterRight As Vector3
			Get
				Return New Vector3(Me.Center.x + Me.boxCollider.size.x / 2F, Me.Center.y, 0F)
			End Get
		End Property

		' Token: 0x1700050F RID: 1295
		' (get) Token: 0x06003C1A RID: 15386 RVA: 0x002182ED File Offset: 0x002166ED
		Public ReadOnly Property Center As Vector2
			Get
				Return Me.transform.position + Me.boxCollider.offset
			End Get
		End Property

		' Token: 0x17000510 RID: 1296
		' (get) Token: 0x06003C1B RID: 15387 RVA: 0x00218310 File Offset: 0x00216710
		Public ReadOnly Property Bottom As Vector3
			Get
				Return New Vector3(Me.Center.x, Me.Center.y - Me.boxCollider.size.y / 2F, 0F)
			End Get
		End Property

		' Token: 0x17000511 RID: 1297
		' (get) Token: 0x06003C1C RID: 15388 RVA: 0x00218360 File Offset: 0x00216760
		Public ReadOnly Property BottomLeft As Vector3
			Get
				Return New Vector3(Me.Center.x - Me.boxCollider.size.x / 2F, Me.Center.y - Me.boxCollider.size.y / 2F, 0F)
			End Get
		End Property

		' Token: 0x17000512 RID: 1298
		' (get) Token: 0x06003C1D RID: 15389 RVA: 0x002183C8 File Offset: 0x002167C8
		Public ReadOnly Property BottomRight As Vector3
			Get
				Return New Vector3(Me.Center.x + Me.boxCollider.size.x / 2F, Me.Center.y - Me.boxCollider.size.y / 2F, 0F)
			End Get
		End Property

		' Token: 0x17000513 RID: 1299
		' (get) Token: 0x06003C1E RID: 15390 RVA: 0x00218430 File Offset: 0x00216830
		Public ReadOnly Property TopY As Single
			Get
				Return Me.Top.y - Me.transform.position.y
			End Get
		End Property

		' Token: 0x17000514 RID: 1300
		' (get) Token: 0x06003C1F RID: 15391 RVA: 0x00218460 File Offset: 0x00216860
		Public ReadOnly Property BottomY As Single
			Get
				Return Me.Bottom.y - Me.transform.position.y
			End Get
		End Property

		' Token: 0x040043A6 RID: 17318
		Private transform As Transform

		' Token: 0x040043A7 RID: 17319
		Private boxCollider As BoxCollider2D
	End Class
End Class
