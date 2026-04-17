Imports System
Imports UnityEngine

' Token: 0x02000A51 RID: 2641
Public Class PlayerSuperChaliceBounceBall
	Inherits AbstractProjectile

	' Token: 0x06003EEB RID: 16107 RVA: 0x0022729B File Offset: 0x0022569B
	Protected Overrides Sub OnDieLifetime()
	End Sub

	' Token: 0x06003EEC RID: 16108 RVA: 0x0022729D File Offset: 0x0022569D
	Protected Overrides Sub OnDieDistance()
	End Sub

	' Token: 0x06003EED RID: 16109 RVA: 0x002272A0 File Offset: 0x002256A0
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.baseScale = MyBase.transform.localScale
		Me.colliderSize = MyBase.GetComponent(Of CircleCollider2D)().radius
		Me.rend = MyBase.GetComponent(Of SpriteRenderer)()
		Me.velocity.y = 0F
		Me.damageDealer.SetDamageSource(DamageDealer.DamageSource.Super)
	End Sub

	' Token: 0x06003EEE RID: 16110 RVA: 0x00227300 File Offset: 0x00225700
	Protected Overrides Sub FixedUpdate()
		MyBase.FixedUpdate()
		Me.velocity.y = Me.velocity.y - Me.GRAVITY * CupheadTime.FixedDelta
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
		Me.HandleInput()
		Me.HandleJiggle()
		Me.CheckEdges()
		Me.CheckCollisionsThenMove()
		If Not Me.super.LAUNCHED_VERSION Then
			Me.player.transform.position = MyBase.transform.position
		End If
		Me.lastHitTimer -= CupheadTime.FixedDelta
		If Me.super.timer < 1F Then
			Me.rend.enabled = Time.frameCount Mod 2 = 0
		End If
	End Sub

	' Token: 0x06003EEF RID: 16111 RVA: 0x002273C6 File Offset: 0x002257C6
	Public Overrides Sub OnLevelEnd()
		Me.super.CleanUp()
	End Sub

	' Token: 0x06003EF0 RID: 16112 RVA: 0x002273D4 File Offset: 0x002257D4
	Private Sub HandleJiggle()
		If Me.jiggleTime > 0F Then
			MyBase.transform.localScale = New Vector3(Me.baseScale.x + Mathf.Sin(Me.jiggleTime * 3.1415927F * 15F) * Me.jiggleTime * 10F, Me.baseScale.y + Mathf.Cos(Me.jiggleTime * 3.1415927F * 15F) * Me.jiggleTime * 10F, 1F)
			Me.jiggleTime -= CupheadTime.FixedDelta
		Else
			MyBase.transform.localScale = Me.baseScale
		End If
	End Sub

	' Token: 0x06003EF1 RID: 16113 RVA: 0x0022748E File Offset: 0x0022588E
	Private Sub SetJiggle()
		AudioManager.Play("player_jump")
		AudioManager.Play("circus_trampoline_bounce")
		Me.jiggleTime = 0.2F
	End Sub

	' Token: 0x06003EF2 RID: 16114 RVA: 0x002274B0 File Offset: 0x002258B0
	Private Sub CheckCollisionsThenMove()
		Dim vector As Vector3 = Vector3.zero
		Dim num As Integer = 0
		Dim num2 As Single = Vector3.Magnitude(Me.velocity * CupheadTime.FixedDelta)
		Dim num3 As Integer = 9
		Dim num4 As Single = 3F
		Dim num5 As Single = Me.baseScale.x * Me.colliderSize * 0.9F
		Dim num6 As Integer = 262144
		Dim num7 As Integer = 1048576
		Dim num8 As Integer = 524288
		Dim num9 As Integer = num6 + num8 + num7
		Dim num10 As Integer = 1
		Dim array As Vector3() = New Vector3(num3 - 1) {}
		While num2 > 0F AndAlso CSng(num) < num4
			Dim gameObject As GameObject = Nothing
			Dim flag As Boolean = False
			Dim vector2 As Vector3 = Me.velocity.normalized
			Dim num11 As Single = CSng((180 / (array.Length - 1)))
			For i As Integer = 0 To array.Length - 1
				array(i) = MyBase.transform.position + Quaternion.Euler(0F, 0F, -90F + num11 * CSng(i)) * vector2 * num5
			Next
			Dim num12 As Single = num2
			For j As Integer = 0 To num3 - 1
				If Physics2D.OverlapPoint(array(j), num9) IsNot Nothing AndAlso Physics2D.OverlapPoint(MyBase.transform.position, num9) Is Nothing Then
					Dim raycastHit2D As RaycastHit2D = Physics2D.Raycast(MyBase.transform.position, array(j) - MyBase.transform.position, num5 * 2F, num9)
					If raycastHit2D.collider IsNot Nothing Then
						array(j) = Vector3.Lerp(raycastHit2D.point, MyBase.transform.position, 0.001F)
					End If
				End If
			Next
			For k As Integer = 0 To num3 - 1
				Dim raycastHit2D2 As RaycastHit2D = Physics2D.Raycast(array(k), Me.velocity, num12, num6)
				Global.Debug.DrawLine(array(k), array(k) + vector2 * num12, Color.red, 1F)
				If raycastHit2D2.collider IsNot Nothing Then
					Me.smokePuffEffect.Create(raycastHit2D2.point)
					If Vector3.Distance(array(k), raycastHit2D2.point) <= num12 Then
						flag = True
						vector = raycastHit2D2.normal
						num12 = Vector3.Distance(array(k), raycastHit2D2.point)
					End If
				End If
			Next
			For l As Integer = 0 To num3 - 1
				Dim raycastHit2D2 As RaycastHit2D = Physics2D.Raycast(array(l), Me.velocity, num12, num10)
				If raycastHit2D2.collider IsNot Nothing AndAlso raycastHit2D2.collider.gameObject.CompareTag("Enemy") AndAlso (Me.lastEnemyHit Is Nothing OrElse Me.lastEnemyHit IsNot raycastHit2D2 OrElse Me.lastHitTimer <= 0F) Then
					Me.smokePuffEffect.Create(raycastHit2D2.point)
					If Vector3.Distance(array(l), raycastHit2D2.point) <= num12 Then
						flag = True
						vector = raycastHit2D2.normal
						num12 = Vector3.Distance(array(l), raycastHit2D2.point)
						gameObject = raycastHit2D2.collider.gameObject
					End If
				End If
			Next
			If Me.velocity.y >= 0F Then
				For m As Integer = 0 To num3 - 1
					Dim raycastHit2D2 As RaycastHit2D = Physics2D.Raycast(array(m), Me.velocity, num12, num8)
					If raycastHit2D2.collider IsNot Nothing Then
						Me.smokePuffEffect.Create(raycastHit2D2.point)
						If Vector3.Distance(array(m), raycastHit2D2.point) <= num12 Then
							flag = True
							vector = raycastHit2D2.normal
							num12 = Vector3.Distance(array(m), raycastHit2D2.point)
						End If
					End If
				Next
			End If
			For n As Integer = 0 To num3 - 1
				Dim raycastHit2D2 As RaycastHit2D = Physics2D.Raycast(array(n), Me.velocity, num12, num7)
				If raycastHit2D2.collider IsNot Nothing Then
					Dim component As LevelPlatform = raycastHit2D2.collider.gameObject.GetComponent(Of LevelPlatform)()
					Dim flag2 As Boolean = False
					If component IsNot Nothing AndAlso (MyBase.transform.position.y < raycastHit2D2.point.y OrElse Me.velocity.y > 0F) Then
						flag2 = True
					End If
					If Not flag2 AndAlso (component Is Nothing OrElse Not component.canFallThrough OrElse Me.player.input.actions.GetAxis(1) > -0.35F) Then
						Me.smokePuffEffect.Create(raycastHit2D2.point)
						If Vector3.Distance(array(n), raycastHit2D2.point) <= num12 Then
							flag = True
							vector = raycastHit2D2.normal
							num12 = Vector3.Distance(array(n), raycastHit2D2.point)
						End If
					End If
				End If
			Next
			num2 -= num12
			MyBase.transform.position += vector2 * num12
			If flag Then
				Me.SetJiggle()
				num += 1
				Me.velocity = Vector3.Reflect(Me.velocity, vector)
				If vector.y > 0F Then
					Me.velocity.y = vector.y * Me.BOUNCE_VEL * If((Not Me.player.input.actions.GetButton(2)), Me.BOUNCE_MODIFIER_NO_JUMP, 1F)
				End If
				If gameObject IsNot Nothing Then
					Me.DoCollisionEnemy(gameObject)
					Me.velocity.x = Me.velocity.x * Me.ENEMY_REBOUND_MULTIPLIER
				End If
			End If
		End While
	End Sub

	' Token: 0x06003EF3 RID: 16115 RVA: 0x00227BC0 File Offset: 0x00225FC0
	Protected Sub DoCollisionEnemy(hit As GameObject)
		Me.lastEnemyHit = hit
		Me.lastHitTimer = Me.ENEMY_MULTIHIT_DELAY
		Dim num As Single = Me.damageDealer.DealDamage(hit)
		If num > 0F Then
			MyBase.animator.Play("Player_Super_Chalice_BounceBall_Flash")
			AudioManager.Play("player_parry_axe")
		End If
		Me.damageCount += num
		If Me.damageCount >= Me.MAX_DAMAGE Then
			Me.super.Interrupt()
		End If
	End Sub

	' Token: 0x06003EF4 RID: 16116 RVA: 0x00227C3C File Offset: 0x0022603C
	Private Sub HandleInput()
		Dim trilean As Trilean = 0
		Dim trilean2 As Trilean = 0
		Dim axis As Single = Me.player.input.actions.GetAxis(0)
		If axis > 0.35F OrElse axis < -0.35F Then
			trilean = axis
		End If
		Dim move_ACCEL As Single = Me.MOVE_ACCEL
		Me.velocity.x = Me.velocity.x + CSng(trilean.Value) * move_ACCEL * CupheadTime.FixedDelta
		Me.velocity.x = Mathf.Clamp(Me.velocity.x, -Me.MOVE_MAX_SPEED, Me.MOVE_MAX_SPEED)
	End Sub

	' Token: 0x06003EF5 RID: 16117 RVA: 0x00227CDC File Offset: 0x002260DC
	Private Sub CheckEdges()
		If LevelPit.Instance IsNot Nothing AndAlso MyBase.transform.position.y < LevelPit.Instance.transform.position.y AndAlso Me.velocity.y < 0F Then
			MyBase.transform.position += Vector3.down * 300F
			Me.super.Interrupt()
		End If
		Dim vector As Vector2 = MyBase.transform.position
		vector.x = Mathf.Clamp(vector.x, CSng(Level.Current.Left) + 30F, CSng(Level.Current.Right) - 30F)
		If vector.x <> MyBase.transform.position.x Then
			Me.velocity.x = -Me.velocity.x
		End If
		MyBase.transform.position = vector
	End Sub

	' Token: 0x040045E5 RID: 17893
	Private Const PADDING_LEFT As Single = 30F

	' Token: 0x040045E6 RID: 17894
	Private Const PADDING_RIGHT As Single = 30F

	' Token: 0x040045E7 RID: 17895
	Private Const ANALOG_THRESHOLD As Single = 0.35F

	' Token: 0x040045E8 RID: 17896
	Private MAX_DAMAGE As Single = WeaponProperties.LevelSuperChaliceBounce.maxDamage

	' Token: 0x040045E9 RID: 17897
	Private MOVE_ACCEL As Single = WeaponProperties.LevelSuperChaliceBounce.horizontalAcceleration

	' Token: 0x040045EA RID: 17898
	Private MOVE_MAX_SPEED As Single = WeaponProperties.LevelSuperChaliceBounce.maxHorizontalSpeed

	' Token: 0x040045EB RID: 17899
	Private BOUNCE_VEL As Single = WeaponProperties.LevelSuperChaliceBounce.bounceVelocity

	' Token: 0x040045EC RID: 17900
	Private BOUNCE_MODIFIER_NO_JUMP As Single = WeaponProperties.LevelSuperChaliceBounce.bounceModifierNoJump

	' Token: 0x040045ED RID: 17901
	Private GRAVITY As Single = WeaponProperties.LevelSuperChaliceBounce.gravity

	' Token: 0x040045EE RID: 17902
	Private ENEMY_REBOUND_MULTIPLIER As Single = WeaponProperties.LevelSuperChaliceBounce.enemyReboundMultiplier

	' Token: 0x040045EF RID: 17903
	Private ENEMY_MULTIHIT_DELAY As Single = WeaponProperties.LevelSuperChaliceBounce.enemyMultihitDelay

	' Token: 0x040045F0 RID: 17904
	<SerializeField()>
	Private smokePuffEffect As Effect

	' Token: 0x040045F1 RID: 17905
	Public velocity As Vector2

	' Token: 0x040045F2 RID: 17906
	Public player As LevelPlayerController

	' Token: 0x040045F3 RID: 17907
	Private lastEnemyHit As GameObject

	' Token: 0x040045F4 RID: 17908
	Private lastHitTimer As Single

	' Token: 0x040045F5 RID: 17909
	Public super As PlayerSuperChaliceBounce

	' Token: 0x040045F6 RID: 17910
	Private jiggleTime As Single

	' Token: 0x040045F7 RID: 17911
	Private baseScale As Vector3

	' Token: 0x040045F8 RID: 17912
	Private colliderSize As Single

	' Token: 0x040045F9 RID: 17913
	Private rend As SpriteRenderer

	' Token: 0x040045FA RID: 17914
	Private damageCount As Single
End Class
