Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000861 RID: 2145
Public Class PlatformingLevelGroundMovementEnemy
	Inherits AbstractPlatformingLevelEnemy

	' Token: 0x060031D5 RID: 12757 RVA: 0x001D1740 File Offset: 0x001CFB40
	Public Function Spawn(position As Vector3, startDirection As PlatformingLevelGroundMovementEnemy.Direction, destroyEnemyAfterLeavingScreen As Boolean) As PlatformingLevelGroundMovementEnemy
		Dim platformingLevelGroundMovementEnemy As PlatformingLevelGroundMovementEnemy = Me.InstantiatePrefab(Of PlatformingLevelGroundMovementEnemy)()
		platformingLevelGroundMovementEnemy.transform.position = position
		platformingLevelGroundMovementEnemy._destroyEnemyAfterLeavingScreen = destroyEnemyAfterLeavingScreen
		platformingLevelGroundMovementEnemy._startCondition = AbstractPlatformingLevelEnemy.StartCondition.Instant
		platformingLevelGroundMovementEnemy._direction = startDirection
		Return platformingLevelGroundMovementEnemy
	End Function

	' Token: 0x17000435 RID: 1077
	' (get) Token: 0x060031D6 RID: 12758 RVA: 0x001D1776 File Offset: 0x001CFB76
	Public ReadOnly Property direction As PlatformingLevelGroundMovementEnemy.Direction
		Get
			Return Me._direction
		End Get
	End Property

	' Token: 0x17000436 RID: 1078
	' (get) Token: 0x060031D7 RID: 12759 RVA: 0x001D177E File Offset: 0x001CFB7E
	' (set) Token: 0x060031D8 RID: 12760 RVA: 0x001D1786 File Offset: 0x001CFB86
	Public Property Grounded As Boolean

	' Token: 0x17000437 RID: 1079
	' (get) Token: 0x060031D9 RID: 12761 RVA: 0x001D178F File Offset: 0x001CFB8F
	Protected Overridable ReadOnly Property collider As Collider2D
		Get
			Return Me._collider
		End Get
	End Property

	' Token: 0x060031DA RID: 12762 RVA: 0x001D1798 File Offset: 0x001CFB98
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me._collider = MyBase.GetComponent(Of Collider2D)()
		Me.directionManager = New PlatformingLevelGroundMovementEnemy.DirectionManager()
		Me.jumpManager = New PlatformingLevelGroundMovementEnemy.JumpManager()
		Me.timeSinceTurn = 10000F
		If Me.shadow IsNot Nothing Then
			Me.shadow.parent = Nothing
		End If
		Me.SetTurnTarget("Turn")
	End Sub

	' Token: 0x060031DB RID: 12763 RVA: 0x001D1800 File Offset: 0x001CFC00
	Protected Overrides Sub OnStart()
	End Sub

	' Token: 0x060031DC RID: 12764 RVA: 0x001D1804 File Offset: 0x001CFC04
	Public Sub GoToGround(Optional despawnOnPit As Boolean = True, Optional groundStateName As String = "Run")
		MyBase.animator.Play(groundStateName)
		Dim bounds As Bounds = Me.collider.bounds
		Dim vector As Vector2 = bounds.center - MyBase.transform.position
		If Not Me.gravityReversed Then
			Me.hits = Me.BoxCastAll(New Vector2(bounds.size.x, 1F), Vector2.down, Me.groundMask, New Vector2(0F, -bounds.size.y / 4F))
		Else
			Me.hits = Me.BoxCastAll(New Vector2(bounds.size.x, 1F), Vector2.up, Me.ceilingMask, New Vector2(0F, bounds.size.y))
		End If
		Dim vector2 As Vector2 = MyBase.transform.position
		Dim flag As Boolean = False
		For Each raycastHit2D As RaycastHit2D In Me.hits
			Dim component As LevelPlatform = raycastHit2D.collider.gameObject.GetComponent(Of LevelPlatform)()
			If raycastHit2D.collider IsNot Nothing AndAlso (Me.canSpawnOnPlatforms OrElse component Is Nothing OrElse Not component.canFallThrough) Then
				vector2 = raycastHit2D.point
				flag = True
				Exit For
			End If
		Next
		If Not flag Then
			Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		End If
		MyBase.transform.SetPosition(Nothing, New Single?(vector2.y), Nothing)
		Me.HandleRaycasts()
		Me.jumpManager.ableToLand = True
		Me.OnGrounded()
		If despawnOnPit Then
			Dim vector3 As Vector2 = MyBase.transform.position + vector + New Vector2(If((Me.direction <> PlatformingLevelGroundMovementEnemy.Direction.Left), (-bounds.size.x / 2F), (bounds.size.x / 2F)), Me.turnaroundDistance / 2F - bounds.size.y / 2F)
			Dim vector4 As Vector2 = MyBase.transform.position + vector + New Vector2(If((Me.direction <> PlatformingLevelGroundMovementEnemy.Direction.Left), Me.turnaroundDistance, (-Me.turnaroundDistance)), Me.turnaroundDistance / 2F - bounds.size.y / 2F)
			For j As Integer = 0 To 10
				Dim num As Single = CSng(j) / 10F
				If Not Me.gravityReversed Then
					If Physics2D.Raycast(Vector2.Lerp(vector3, vector4, num), Vector2.down, 30F + Me.turnaroundDistance, Me.groundMask).collider Is Nothing Then
						Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
						Return
					End If
				ElseIf Physics2D.Raycast(Vector2.Lerp(vector3, vector4, num), Vector2.up, 30F + Me.turnaroundDistance, Me.ceilingMask).collider Is Nothing Then
					Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
					Return
				End If
			Next
		End If
	End Sub

	' Token: 0x060031DD RID: 12765 RVA: 0x001D1B92 File Offset: 0x001CFF92
	Public Sub Float(Optional playAnim As Boolean = True)
		If playAnim Then
			MyBase.animator.Play("Float", 0, Global.UnityEngine.Random.Range(0F, 1F))
		End If
		Me.playFloatAnim = playAnim
		Me.floating = True
	End Sub

	' Token: 0x060031DE RID: 12766 RVA: 0x001D1BC8 File Offset: 0x001CFFC8
	Protected Overrides Sub Update()
		MyBase.Update()
		Me.CalculateDirection()
		Me.CalculateRender()
		If Me.shadow IsNot Nothing Then
			Me.UpdateShadow()
		End If
	End Sub

	' Token: 0x060031DF RID: 12767 RVA: 0x001D1BF3 File Offset: 0x001CFFF3
	Protected Overridable Sub FixedUpdate()
		If MyBase.Dead OrElse MyBase.GetComponent(Of DamageReceiver)().IsHitPaused Then
			Return
		End If
		Me.HandleRaycasts()
		Me.HandleFalling()
		Me.Move()
	End Sub

	' Token: 0x060031E0 RID: 12768 RVA: 0x001D1C24 File Offset: 0x001D0024
	Private Sub HandleFalling()
		If Me.Grounded Then
			Return
		End If
		If Me.floating Then
			Me.velocity = New Vector2(0F, -MyBase.Properties.floatSpeed)
		ElseIf Not Me.gravityReversed Then
			Me.velocity.y = Me.velocity.y - MyBase.Properties.gravity * CupheadTime.FixedDelta
			Me.jumpManager.ableToLand = Me.velocity.y < 0F
		Else
			Me.velocity.y = Me.velocity.y + MyBase.Properties.gravity * CupheadTime.FixedDelta
			Me.jumpManager.ableToLand = Me.velocity.y < 0F
		End If
	End Sub

	' Token: 0x060031E1 RID: 12769 RVA: 0x001D1CF9 File Offset: 0x001D00F9
	Protected Overridable Function GetMoveSpeed() As Single
		If Me.moveSpeed = 0F Then
			Me.moveSpeed = MyBase.Properties.MoveSpeed
		End If
		Return Me.moveSpeed
	End Function

	' Token: 0x060031E2 RID: 12770 RVA: 0x001D1D22 File Offset: 0x001D0122
	Protected Overridable Sub SetMoveSpeed(moveSpeed As Single)
		Me.moveSpeed = moveSpeed
	End Sub

	' Token: 0x060031E3 RID: 12771 RVA: 0x001D1D2C File Offset: 0x001D012C
	Private Sub Move()
		If Me.turning OrElse Me.landing OrElse (Me.jumping AndAlso Me.Grounded) Then
			Return
		End If
		Me.timeSinceTurn += CupheadTime.FixedDelta
		Dim num As Single = CSng(If((Me._direction <> PlatformingLevelGroundMovementEnemy.Direction.Right), (-1), 1))
		If Me.jumpManager.state = PlatformingLevelGroundMovementEnemy.JumpManager.State.Ready AndAlso Not Me.floating Then
			Me.velocity.x = Me.GetMoveSpeed() * num
		End If
		MyBase.transform.AddPosition(Me.velocity.x * CupheadTime.FixedDelta, Me.velocity.y * CupheadTime.FixedDelta, 0F)
		If Not Me.gravityReversed Then
			If Me.Grounded AndAlso MyBase.transform.position.y - Me.directionManager.down.pos.y < 30F Then
				Dim vector As Vector2 = MyBase.transform.position
				vector.y = Me.directionManager.down.pos.y
				MyBase.transform.position = vector
			End If
		ElseIf Me.Grounded AndAlso MyBase.transform.position.y + Me.directionManager.up.pos.y > 30F Then
			Dim vector2 As Vector2 = MyBase.transform.position
			vector2.y = Me.directionManager.up.pos.y
			MyBase.transform.position = vector2
		End If
	End Sub

	' Token: 0x060031E4 RID: 12772 RVA: 0x001D1EF8 File Offset: 0x001D02F8
	Private Sub CalculateRender()
		If CupheadLevelCamera.Current.ContainsPoint(MyBase.transform.position) AndAlso Not Me._enteredScreen Then
			Me._enteredScreen = True
		End If
		If Me._enteredScreen AndAlso Me._destroyEnemyAfterLeavingScreen AndAlso Not CupheadLevelCamera.Current.ContainsPoint(MyBase.transform.position, New Vector2(100F, 100F)) Then
			Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		End If
		If PlatformingLevel.Current IsNot Nothing AndAlso (MyBase.transform.position.x < CSng(PlatformingLevel.Current.Left) - 100F OrElse MyBase.transform.position.x > CSng(PlatformingLevel.Current.Right) + 100F OrElse MyBase.transform.position.y < CSng(PlatformingLevel.Current.Ground) - 100F) Then
			Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		End If
	End Sub

	' Token: 0x060031E5 RID: 12773 RVA: 0x001D201B File Offset: 0x001D041B
	Private Sub LateUpdate()
		Me.CalculateDirection()
	End Sub

	' Token: 0x060031E6 RID: 12774 RVA: 0x001D2024 File Offset: 0x001D0424
	Protected Overridable Sub CalculateDirection()
		If Me._direction = PlatformingLevelGroundMovementEnemy.Direction.Right Then
			MyBase.transform.SetScale(New Single?(-1F), Nothing, Nothing)
		Else
			MyBase.transform.SetScale(New Single?(1F), Nothing, Nothing)
		End If
	End Sub

	' Token: 0x060031E7 RID: 12775 RVA: 0x001D2090 File Offset: 0x001D0490
	Protected Overrides Sub Die()
		If Me.shadow IsNot Nothing Then
			Global.UnityEngine.[Object].Destroy(Me.shadow.gameObject)
		End If
		MyBase.Die()
	End Sub

	' Token: 0x060031E8 RID: 12776 RVA: 0x001D20B9 File Offset: 0x001D04B9
	Protected Overridable Function Turn() As Coroutine
		Me.turning = True
		Me.timeSinceTurn = 0F
		Return MyBase.StartCoroutine(Me.turn_cr())
	End Function

	' Token: 0x060031E9 RID: 12777 RVA: 0x001D20DC File Offset: 0x001D04DC
	Private Iterator Function turn_cr() As IEnumerator
		If Me.hasTurnAnimation AndAlso MyBase.animator IsNot Nothing Then
			MyBase.animator.Play("Turn")
			Dim target As Integer = Animator.StringToHash(MyBase.animator.GetLayerName(0) + "." + Me.turnTarget)
			While MyBase.animator.GetCurrentAnimatorStateInfo(0).fullPathHash <> target
				Yield Nothing
			End While
			Dim animLength As Single = MyBase.animator.GetCurrentAnimatorStateInfo(0).length
			While MyBase.animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= (animLength - CupheadTime.Delta) / animLength
				Yield Nothing
			End While
		End If
		If Me._direction = PlatformingLevelGroundMovementEnemy.Direction.Right Then
			Me._direction = PlatformingLevelGroundMovementEnemy.Direction.Left
		Else
			Me._direction = PlatformingLevelGroundMovementEnemy.Direction.Right
		End If
		Me.CalculateDirection()
		Me.turning = False
		Return
	End Function

	' Token: 0x060031EA RID: 12778 RVA: 0x001D20F7 File Offset: 0x001D04F7
	Protected Overridable Sub SetTurnTarget(turnTarget As String)
		Me.turnTarget = turnTarget
	End Sub

	' Token: 0x060031EB RID: 12779 RVA: 0x001D2100 File Offset: 0x001D0500
	Private Iterator Function floatLand_cr() As IEnumerator
		Me.floating = False
		Me.landing = True
		If Not Me.lockDirectionWhenLanding Then
			Me._direction = If((PlayerManager.GetNext().center.x <= MyBase.transform.position.x), PlatformingLevelGroundMovementEnemy.Direction.Left, PlatformingLevelGroundMovementEnemy.Direction.Right)
		End If
		MyBase.transform.SetPosition(Nothing, New Single?(Me.directionManager.down.pos.y), Nothing)
		If Me.playFloatAnim Then
			MyBase.animator.Play("Land")
		End If
		Me.playFloatAnim = False
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Land", False, True)
		Me.velocity.y = 0F
		Me.landing = False
		Return
	End Function

	' Token: 0x060031EC RID: 12780 RVA: 0x001D211B File Offset: 0x001D051B
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.jumpLandEffectPrefab = Nothing
	End Sub

	' Token: 0x060031ED RID: 12781 RVA: 0x001D212C File Offset: 0x001D052C
	Public Sub Jump()
		If MyBase.Properties.canJump AndAlso Me.jumpManager.state = PlatformingLevelGroundMovementEnemy.JumpManager.State.Ready AndAlso Not Me.turning Then
			Me.jumpManager.state = PlatformingLevelGroundMovementEnemy.JumpManager.State.Used
			MyBase.StartCoroutine(Me.jump_cr())
			Me.jumping = True
		End If
	End Sub

	' Token: 0x060031EE RID: 12782 RVA: 0x001D2184 File Offset: 0x001D0584
	Private Iterator Function jump_cr() As IEnumerator
		If Me.hasJumpAnimation AndAlso MyBase.animator IsNot Nothing Then
			MyBase.animator.Play("Jump")
			Dim target As Integer = Animator.StringToHash(MyBase.animator.GetLayerName(0) + ".Jump")
			While MyBase.animator.GetCurrentAnimatorStateInfo(0).fullPathHash <> target
				Yield Nothing
			End While
			Dim animLength As Single = MyBase.animator.GetCurrentAnimatorStateInfo(0).length
			While MyBase.animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= (animLength - CupheadTime.Delta) / animLength
				Yield Nothing
			End While
		End If
		Dim directionSign As Single = CSng(If((Me._direction <> PlatformingLevelGroundMovementEnemy.Direction.Right), (-1), 1))
		Dim timeToApex As Single = Mathf.Sqrt(2F * MyBase.Properties.jumpHeight / MyBase.Properties.gravity)
		Dim x As Single = If((Not Me.manuallySetJumpX), MyBase.Properties.jumpLength, Me.GetMoveSpeed())
		Me.LeaveGround()
		Me.velocity.y = MyBase.Properties.gravity * timeToApex
		Me.velocity.x = directionSign * x / (2F * timeToApex)
		If Me.hasJumpAnimation AndAlso MyBase.animator IsNot Nothing Then
			Yield CupheadTime.WaitForSeconds(Me, timeToApex)
			MyBase.animator.SetTrigger("Apex")
		End If
		While Me.jumping
			Yield Nothing
		End While
		Me.landing = True
		If Me.directionManager.down IsNot Nothing Then
			MyBase.transform.SetPosition(Nothing, New Single?(Me.directionManager.down.pos.y), Nothing)
		End If
		If Me.jumpLandEffectPrefab IsNot Nothing Then
			Me.jumpLandEffectPrefab.Create(MyBase.transform.position)
		End If
		If Me.hasJumpAnimation AndAlso MyBase.animator IsNot Nothing Then
			MyBase.animator.SetTrigger("Land")
			Yield MyBase.animator.WaitForAnimationToEnd(Me, "Jump_Land", False, True)
		End If
		Me.landing = False
		Return
	End Function

	' Token: 0x060031EF RID: 12783 RVA: 0x001D219F File Offset: 0x001D059F
	Private Function BoxCast(size As Vector2, direction As Vector2, layerMask As Integer) As RaycastHit2D
		Return Me.BoxCast(size, direction, layerMask, Vector2.zero)
	End Function

	' Token: 0x060031F0 RID: 12784 RVA: 0x001D21B0 File Offset: 0x001D05B0
	Private Function BoxCast(size As Vector2, direction As Vector2, layerMask As Integer, offset As Vector2) As RaycastHit2D
		Return Physics2D.BoxCast(Me.collider.bounds.center + offset, size, 0F, direction, 2000F, layerMask)
	End Function

	' Token: 0x060031F1 RID: 12785 RVA: 0x001D21F0 File Offset: 0x001D05F0
	Private Function BoxCastAll(size As Vector2, direction As Vector2, layerMask As Integer, offset As Vector2) As RaycastHit2D()
		Return Physics2D.BoxCastAll(Me.collider.bounds.center + offset, size, 0F, direction, 2000F, layerMask)
	End Function

	' Token: 0x060031F2 RID: 12786 RVA: 0x001D2230 File Offset: 0x001D0630
	Private Function CircleCast(radius As Single, direction As Vector2, layerMask As Integer) As RaycastHit2D
		Return Physics2D.CircleCast(Me.collider.bounds.center, radius, direction, 2000F, layerMask)
	End Function

	' Token: 0x060031F3 RID: 12787 RVA: 0x001D2262 File Offset: 0x001D0662
	Private Function DoesRaycastHitHaveCollider(hit As RaycastHit2D) As Boolean
		Return hit.collider IsNot Nothing
	End Function

	' Token: 0x060031F4 RID: 12788 RVA: 0x001D2274 File Offset: 0x001D0674
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		If Application.isPlaying Then
			Gizmos.color = Color.blue
			Gizmos.DrawSphere(MyBase.transform.position, 5F)
			Gizmos.color = Color.red
			Gizmos.DrawSphere(MyBase.transform.position, 5F)
		End If
	End Sub

	' Token: 0x060031F5 RID: 12789 RVA: 0x001D22D0 File Offset: 0x001D06D0
	Private Sub HandleRaycasts()
		If Me.fallInPit Then
			Return
		End If
		Dim flag As Boolean = True
		If Me.directionManager IsNot Nothing AndAlso Me.directionManager.up IsNot Nothing Then
			flag = Me.directionManager.up.able
		End If
		Dim bounds As Bounds = Me.collider.bounds
		Me.directionManager.Reset()
		Dim raycastHit2D As RaycastHit2D = Me.BoxCast(New Vector2(bounds.size.x, 1F), Vector2.up, Me.ceilingMask)
		Dim raycastHit2D2 As RaycastHit2D = Me.BoxCast(New Vector2(bounds.size.x, 1F), Vector2.down, Me.groundMask, New Vector2(MyBase.transform.position.x - bounds.center.x, MyBase.transform.position.y + 30F - bounds.center.y))
		Dim raycastHit2D3 As RaycastHit2D = Physics2D.Raycast(MyBase.transform.position + New Vector2(If((Me.direction <> PlatformingLevelGroundMovementEnemy.Direction.Left), Me.turnaroundDistance, (-Me.turnaroundDistance)), Me.turnaroundDistance / 2F), Vector2.down, 30F + Me.turnaroundDistance, Me.groundMask)
		Dim raycastHit2D4 As RaycastHit2D = Physics2D.Raycast(MyBase.transform.position + New Vector2(If((Me.direction <> PlatformingLevelGroundMovementEnemy.Direction.Left), Me.turnaroundDistance, (-Me.turnaroundDistance)), Me.turnaroundDistance / 2F), Vector2.up, 30F + Me.turnaroundDistance, Me.ceilingMask)
		Me.RaycastObstacle(Me.directionManager.up, raycastHit2D, bounds.size.y / 2F, PlatformingLevelGroundMovementEnemy.RaycastAxis.Y, bounds.center)
		Me.RaycastObstacle(Me.directionManager.down, raycastHit2D2, 30F, PlatformingLevelGroundMovementEnemy.RaycastAxis.Y, New Vector2(MyBase.transform.position.x, MyBase.transform.position.y + 30F))
		If Not Me.Grounded Then
			If Not Me.directionManager.down.able Then
				Me.OnGrounded()
				Me.directionManager.left.able = True
				Me.directionManager.right.able = True
				If Me.floating Then
					MyBase.StartCoroutine(Me.floatLand_cr())
				End If
			End If
			If Not Me.directionManager.up.able AndAlso Me.directionManager.up.able <> flag Then
				Me.OnHitCeiling()
			End If
		End If
		Dim raycastHit2D5 As RaycastHit2D = If(Me.gravityReversed, raycastHit2D4, raycastHit2D3)
		If Me.Grounded AndAlso raycastHit2D5.collider Is Nothing AndAlso Me.timeSinceTurn > 0.1F AndAlso Not Me.jumping Then
			If Not Me.noTurn Then
				Me.Turn()
			Else
				Me.LeaveGround()
			End If
		End If
	End Sub

	' Token: 0x060031F6 RID: 12790 RVA: 0x001D2620 File Offset: 0x001D0A20
	Private Function RaycastObstacle(directionProperties As PlatformingLevelGroundMovementEnemy.DirectionManager.Hit, raycastHit As RaycastHit2D, maxDistance As Single, axis As PlatformingLevelGroundMovementEnemy.RaycastAxis, origin As Vector2) As Single
		If Not Me.DoesRaycastHitHaveCollider(raycastHit) Then
			Return 1000F
		End If
		Dim num As Single = If((axis <> PlatformingLevelGroundMovementEnemy.RaycastAxis.X), Mathf.Abs(origin.y - raycastHit.point.y), Mathf.Abs(origin.x - raycastHit.point.x))
		directionProperties.pos = raycastHit.point
		directionProperties.gameObject = raycastHit.collider.gameObject
		directionProperties.distance = num
		If num < maxDistance Then
			directionProperties.able = False
		End If
		Return num
	End Function

	' Token: 0x060031F7 RID: 12791 RVA: 0x001D26B9 File Offset: 0x001D0AB9
	Private Sub ValidateRaycast()
	End Sub

	' Token: 0x060031F8 RID: 12792 RVA: 0x001D26BC File Offset: 0x001D0ABC
	Private Sub OnGrounded()
		If Me.Grounded OrElse Not Me.jumpManager.ableToLand Then
			Return
		End If
		Dim levelPlatform As LevelPlatform = If((Not(Me.directionManager.down.gameObject Is Nothing)), Me.directionManager.down.gameObject.GetComponent(Of LevelPlatform)(), Nothing)
		Dim levelPlatform2 As LevelPlatform = If((Not(Me.directionManager.up.gameObject Is Nothing)), Me.directionManager.up.gameObject.GetComponent(Of LevelPlatform)(), Nothing)
		Dim levelPlatform3 As LevelPlatform = If(Me.gravityReversed, levelPlatform2, levelPlatform)
		If levelPlatform3 IsNot Nothing Then
			levelPlatform3.AddChild(MyBase.transform)
		End If
		Me.jumpManager.state = PlatformingLevelGroundMovementEnemy.JumpManager.State.Ready
		Me.velocity.y = 0F
		Me.Grounded = True
		If Me.jumping Then
			Me.jumping = False
		End If
	End Sub

	' Token: 0x060031F9 RID: 12793 RVA: 0x001D27B4 File Offset: 0x001D0BB4
	Private Sub LeaveGround()
		Me.Grounded = False
		Me.jumpManager.ableToLand = False
		Me.velocity.y = 0F
		Me.ClearParent()
		If Me.jumpManager.state = PlatformingLevelGroundMovementEnemy.JumpManager.State.Ready Then
			Me.jumpManager.state = PlatformingLevelGroundMovementEnemy.JumpManager.State.Used
		End If
	End Sub

	' Token: 0x060031FA RID: 12794 RVA: 0x001D2808 File Offset: 0x001D0C08
	Private Sub OnHitCeiling()
		If Not Me.gravityReversed Then
			If Me.jumpManager.ableToLand Then
				Return
			End If
		Else
			If Me.Grounded Then
				Return
			End If
			Me.jumpManager.state = PlatformingLevelGroundMovementEnemy.JumpManager.State.Ready
			Dim levelPlatform As LevelPlatform = If((Not(Me.directionManager.up.gameObject Is Nothing)), Me.directionManager.up.gameObject.GetComponent(Of LevelPlatform)(), Nothing)
			If levelPlatform IsNot Nothing Then
				levelPlatform.AddChild(MyBase.transform)
			End If
			Me.Grounded = True
			If Me.jumping Then
				Me.jumping = False
			End If
		End If
		Me.velocity.y = 0F
		Me.directionManager.left.able = True
		Me.directionManager.right.able = True
	End Sub

	' Token: 0x060031FB RID: 12795 RVA: 0x001D28E8 File Offset: 0x001D0CE8
	Private Sub ClearParent()
		If MyBase.transform.parent IsNot Nothing Then
			MyBase.transform.parent.GetComponent(Of LevelPlatform)().OnPlayerExit(MyBase.transform)
		End If
		MyBase.transform.parent = Nothing
	End Sub

	' Token: 0x060031FC RID: 12796 RVA: 0x001D2928 File Offset: 0x001D0D28
	Private Sub UpdateShadow()
		If Me.Grounded Then
			Me.shadow.gameObject.SetActive(False)
			Return
		End If
		Dim raycastHit2D As RaycastHit2D = Physics2D.BoxCast(MyBase.transform.position, New Vector2(Me.collider.bounds.size.x, 1F), 0F, Vector2.down, Me.maxShadowDistance, Me.groundMask)
		If raycastHit2D.collider Is Nothing Then
			Me.shadow.gameObject.SetActive(False)
			Return
		End If
		Me.shadow.gameObject.SetActive(True)
		Me.shadow.SetPosition(New Single?(MyBase.transform.position.x), New Single?(raycastHit2D.point.y), Nothing)
		Dim num As Single = MyBase.transform.position.y - Me.shadow.position.y
		Me.shadow.GetComponent(Of Animator)().Play("Idle", 0, num / Me.maxShadowDistance)
		Me.shadow.GetComponent(Of Animator)().speed = 0F
	End Sub

	' Token: 0x04003A32 RID: 14898
	Private Const SCREEN_PADDING As Single = 100F

	' Token: 0x04003A33 RID: 14899
	Private Const DOWN_BOXCAST_Y As Single = 30F

	' Token: 0x04003A34 RID: 14900
	Public startPosition As Single = 0.5F

	' Token: 0x04003A35 RID: 14901
	<SerializeField()>
	Protected _direction As PlatformingLevelGroundMovementEnemy.Direction = PlatformingLevelGroundMovementEnemy.Direction.Right

	' Token: 0x04003A36 RID: 14902
	<SerializeField()>
	Private hasJumpAnimation As Boolean

	' Token: 0x04003A37 RID: 14903
	<SerializeField()>
	Private hasTurnAnimation As Boolean

	' Token: 0x04003A38 RID: 14904
	<SerializeField()>
	Private canSpawnOnPlatforms As Boolean

	' Token: 0x04003A39 RID: 14905
	<SerializeField()>
	Private turnaroundDistance As Single = 10F

	' Token: 0x04003A3A RID: 14906
	<SerializeField()>
	Private shadow As Transform

	' Token: 0x04003A3B RID: 14907
	<SerializeField()>
	Private maxShadowDistance As Single

	' Token: 0x04003A3C RID: 14908
	<SerializeField()>
	Private jumpLandEffectPrefab As Effect

	' Token: 0x04003A3D RID: 14909
	<SerializeField()>
	Protected noTurn As Boolean

	' Token: 0x04003A3E RID: 14910
	<SerializeField()>
	Protected lockDirectionWhenLanding As Boolean

	' Token: 0x04003A3F RID: 14911
	<SerializeField()>
	Protected gravityReversed As Boolean

	' Token: 0x04003A41 RID: 14913
	Private _collider As Collider2D

	' Token: 0x04003A42 RID: 14914
	Private _destroyEnemyAfterLeavingScreen As Boolean

	' Token: 0x04003A43 RID: 14915
	Private _enteredScreen As Boolean

	' Token: 0x04003A44 RID: 14916
	Private directionManager As PlatformingLevelGroundMovementEnemy.DirectionManager

	' Token: 0x04003A45 RID: 14917
	Private jumpManager As PlatformingLevelGroundMovementEnemy.JumpManager

	' Token: 0x04003A46 RID: 14918
	Protected turning As Boolean

	' Token: 0x04003A47 RID: 14919
	Protected floating As Boolean

	' Token: 0x04003A48 RID: 14920
	Protected manuallySetJumpX As Boolean

	' Token: 0x04003A49 RID: 14921
	Protected timeSinceTurn As Single

	' Token: 0x04003A4A RID: 14922
	Private turnTarget As String

	' Token: 0x04003A4B RID: 14923
	Private moveSpeed As Single

	' Token: 0x04003A4C RID: 14924
	Private jumping As Boolean

	' Token: 0x04003A4D RID: 14925
	Protected landing As Boolean

	' Token: 0x04003A4E RID: 14926
	Protected fallInPit As Boolean

	' Token: 0x04003A4F RID: 14927
	Private playFloatAnim As Boolean

	' Token: 0x04003A50 RID: 14928
	Private velocity As Vector2 = Vector2.zero

	' Token: 0x04003A51 RID: 14929
	Private hits As RaycastHit2D()

	' Token: 0x04003A52 RID: 14930
	Private Const RAY_DISTANCE As Single = 2000F

	' Token: 0x04003A53 RID: 14931
	Private Const MAX_GROUNDED_FALL_DISTANCE As Single = 30F

	' Token: 0x04003A54 RID: 14932
	Private ceilingMask As Integer = 524288

	' Token: 0x04003A55 RID: 14933
	Private groundMask As Integer = 1048576

	' Token: 0x02000862 RID: 2146
	Public Enum Direction
		' Token: 0x04003A57 RID: 14935
		Right = 1
		' Token: 0x04003A58 RID: 14936
		Left = -1
	End Enum

	' Token: 0x02000863 RID: 2147
	Private Enum RaycastAxis
		' Token: 0x04003A5A RID: 14938
		X
		' Token: 0x04003A5B RID: 14939
		Y
	End Enum

	' Token: 0x02000864 RID: 2148
	Public Class DirectionManager
		' Token: 0x060031FD RID: 12797 RVA: 0x001D2A79 File Offset: 0x001D0E79
		Public Sub New()
			Me.Reset()
		End Sub

		' Token: 0x060031FE RID: 12798 RVA: 0x001D2AB3 File Offset: 0x001D0EB3
		Public Sub Reset()
			Me.up.Reset()
			Me.down.Reset()
			Me.left.Reset()
			Me.right.Reset()
		End Sub

		' Token: 0x04003A5C RID: 14940
		Public up As PlatformingLevelGroundMovementEnemy.DirectionManager.Hit = New PlatformingLevelGroundMovementEnemy.DirectionManager.Hit()

		' Token: 0x04003A5D RID: 14941
		Public down As PlatformingLevelGroundMovementEnemy.DirectionManager.Hit = New PlatformingLevelGroundMovementEnemy.DirectionManager.Hit()

		' Token: 0x04003A5E RID: 14942
		Public left As PlatformingLevelGroundMovementEnemy.DirectionManager.Hit = New PlatformingLevelGroundMovementEnemy.DirectionManager.Hit()

		' Token: 0x04003A5F RID: 14943
		Public right As PlatformingLevelGroundMovementEnemy.DirectionManager.Hit = New PlatformingLevelGroundMovementEnemy.DirectionManager.Hit()

		' Token: 0x02000865 RID: 2149
		Public Class Hit
			' Token: 0x060031FF RID: 12799 RVA: 0x001D2AE1 File Offset: 0x001D0EE1
			Public Sub New()
				Me.Reset()
			End Sub

			' Token: 0x06003200 RID: 12800 RVA: 0x001D2AEF File Offset: 0x001D0EEF
			Public Sub New(able As Boolean, pos As Vector2, gameObject As GameObject, distance As Single)
				Me.able = able
				Me.pos = pos
				Me.gameObject = gameObject
				Me.distance = distance
			End Sub

			' Token: 0x06003201 RID: 12801 RVA: 0x001D2B14 File Offset: 0x001D0F14
			Public Sub Reset()
				Me.able = True
				Me.pos = Vector2.zero
				Me.gameObject = Nothing
				Me.distance = -1F
			End Sub

			' Token: 0x04003A60 RID: 14944
			Public able As Boolean

			' Token: 0x04003A61 RID: 14945
			Public pos As Vector2

			' Token: 0x04003A62 RID: 14946
			Public gameObject As GameObject

			' Token: 0x04003A63 RID: 14947
			Public distance As Single
		End Class
	End Class

	' Token: 0x02000866 RID: 2150
	Public Class JumpManager
		' Token: 0x04003A64 RID: 14948
		Public state As PlatformingLevelGroundMovementEnemy.JumpManager.State

		' Token: 0x04003A65 RID: 14949
		Public ableToLand As Boolean

		' Token: 0x02000867 RID: 2151
		Public Enum State
			' Token: 0x04003A67 RID: 14951
			Ready
			' Token: 0x04003A68 RID: 14952
			Used
		End Enum
	End Class
End Class
