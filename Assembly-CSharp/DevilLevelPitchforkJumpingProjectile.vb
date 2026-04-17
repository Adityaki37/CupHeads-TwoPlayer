Imports System
Imports UnityEngine

' Token: 0x0200058E RID: 1422
Public Class DevilLevelPitchforkJumpingProjectile
	Inherits AbstractProjectile

	' Token: 0x17000353 RID: 851
	' (get) Token: 0x06001B31 RID: 6961 RVA: 0x000F9A4B File Offset: 0x000F7E4B
	Protected Overrides ReadOnly Property DestroyLifetime As Single
		Get
			Return -1F
		End Get
	End Property

	' Token: 0x06001B32 RID: 6962 RVA: 0x000F9A54 File Offset: 0x000F7E54
	Public Function Create(pos As Vector2, launchAngle As MinMax, launchSpeed As MinMax, gravity As Single, numJumps As Integer, parent As DevilLevelSittingDevil) As DevilLevelPitchforkJumpingProjectile
		Dim devilLevelPitchforkJumpingProjectile As DevilLevelPitchforkJumpingProjectile = Me.InstantiatePrefab(Of DevilLevelPitchforkJumpingProjectile)()
		devilLevelPitchforkJumpingProjectile.transform.position = pos
		devilLevelPitchforkJumpingProjectile.launchSpeed = launchSpeed
		devilLevelPitchforkJumpingProjectile.launchAngle = launchAngle
		devilLevelPitchforkJumpingProjectile.gravity = gravity
		devilLevelPitchforkJumpingProjectile.parent = parent
		devilLevelPitchforkJumpingProjectile.jumpsRemaining = numJumps
		Return devilLevelPitchforkJumpingProjectile
	End Function

	' Token: 0x06001B33 RID: 6963 RVA: 0x000F9AA0 File Offset: 0x000F7EA0
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.parent Is Nothing Then
			Me.Die()
		End If
	End Sub

	' Token: 0x06001B34 RID: 6964 RVA: 0x000F9ABF File Offset: 0x000F7EBF
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x06001B35 RID: 6965 RVA: 0x000F9AE0 File Offset: 0x000F7EE0
	Protected Overrides Sub FixedUpdate()
		MyBase.FixedUpdate()
		If Not MyBase.dead AndAlso Me.state = DevilLevelPitchforkJumpingProjectile.State.Jumping Then
			Me.velocity.y = Me.velocity.y - Me.gravity * CupheadTime.FixedDelta
			MyBase.transform.AddPosition(Me.velocity.x * CupheadTime.FixedDelta, Me.velocity.y * CupheadTime.FixedDelta, 0F)
			Dim radius As Single = MyBase.GetComponent(Of CircleCollider2D)().radius
			If MyBase.transform.position.y < CSng(Level.Current.Ground) + radius Then
				MyBase.transform.SetPosition(Nothing, New Single?(CSng(Level.Current.Ground) + radius), Nothing)
				If Me.jumpsRemaining > 0 Then
					Me.state = DevilLevelPitchforkJumpingProjectile.State.OnGround
				Else
					Me.Die()
				End If
			End If
		End If
	End Sub

	' Token: 0x06001B36 RID: 6966 RVA: 0x000F9BD8 File Offset: 0x000F7FD8
	Public Sub Jump()
		Dim num As Single = Single.MaxValue
		Dim vector As Vector2 = Vector2.zero
		Dim center As Vector3 = PlayerManager.GetNext().center
		Dim vector2 As Vector2 = center - MyBase.transform.position
		vector2.x = Mathf.Abs(vector2.x)
		Dim radius As Single = MyBase.GetComponent(Of CircleCollider2D)().radius
		AudioManager.Play("devil_projectile_move")
		Me.emitAudioFromObject.Add("devil_projectile_move")
		Dim num2 As Single
		If center.x < MyBase.transform.position.x Then
			num2 = MyBase.transform.position.x - (CSng(Level.Current.Left) + radius)
		Else
			num2 = CSng(Level.Current.Right) - radius - MyBase.transform.position.x
		End If
		Dim num3 As Single = 0F
		While num3 < 1F
			Dim floatAt As Single = Me.launchAngle.GetFloatAt(num3)
			Dim floatAt2 As Single = Me.launchSpeed.GetFloatAt(num3)
			Dim vector3 As Vector2 = MathUtils.AngleToDirection(floatAt) * floatAt2
			Dim num4 As Single = vector2.x / vector3.x
			Dim num5 As Single = vector3.y * num4 - 0.5F * Me.gravity * num4 * num4
			Dim num6 As Single = Mathf.Abs(vector2.y - num5)
			Dim num7 As Single = vector3.y - Me.gravity * num4
			If num7 <= 0F Then
				Dim num8 As Single = num2 / vector3.x
				Dim num9 As Single = vector3.y * num8 - 0.5F * Me.gravity * num8 * num8
				If num9 <= CSng(Level.Current.Ground) + radius Then
					If num6 < num Then
						num = num6
						vector = vector3
					End If
				End If
			End If
			num3 += 0.01F
		End While
		If center.x < MyBase.transform.position.x Then
			vector.x *= -1F
		End If
		Me.velocity = vector
		Me.state = DevilLevelPitchforkJumpingProjectile.State.Jumping
		Me.jumpsRemaining -= 1
	End Sub

	' Token: 0x06001B37 RID: 6967 RVA: 0x000F9E1A File Offset: 0x000F821A
	Protected Overrides Sub Die()
		MyBase.Die()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x0400246F RID: 9327
	Public state As DevilLevelPitchforkJumpingProjectile.State

	' Token: 0x04002470 RID: 9328
	Private velocity As Vector2

	' Token: 0x04002471 RID: 9329
	Private launchSpeed As MinMax

	' Token: 0x04002472 RID: 9330
	Private launchAngle As MinMax

	' Token: 0x04002473 RID: 9331
	Private gravity As Single

	' Token: 0x04002474 RID: 9332
	Private jumpsRemaining As Integer

	' Token: 0x04002475 RID: 9333
	Private parent As DevilLevelSittingDevil

	' Token: 0x0200058F RID: 1423
	Public Enum State
		' Token: 0x04002477 RID: 9335
		Idle
		' Token: 0x04002478 RID: 9336
		Jumping
		' Token: 0x04002479 RID: 9337
		OnGround
	End Enum
End Class
