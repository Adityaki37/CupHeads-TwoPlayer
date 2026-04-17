Imports System
Imports UnityEngine

' Token: 0x02000753 RID: 1875
Public Class RetroArcadeRobotBouncingProjectile
	Inherits AbstractProjectile

	' Token: 0x170003E0 RID: 992
	' (get) Token: 0x060028DD RID: 10461 RVA: 0x0017C96A File Offset: 0x0017AD6A
	Protected Overrides ReadOnly Property DestroyLifetime As Single
		Get
			Return -1F
		End Get
	End Property

	' Token: 0x170003E1 RID: 993
	' (get) Token: 0x060028DE RID: 10462 RVA: 0x0017C971 File Offset: 0x0017AD71
	Protected Overrides ReadOnly Property DestroyedAfterLeavingScreen As Boolean
		Get
			Return True
		End Get
	End Property

	' Token: 0x060028DF RID: 10463 RVA: 0x0017C974 File Offset: 0x0017AD74
	Public Function Create(pos As Vector2, speed As Single, angle As Single, bounce As Boolean) As RetroArcadeRobotBouncingProjectile
		Dim retroArcadeRobotBouncingProjectile As RetroArcadeRobotBouncingProjectile = Me.InstantiatePrefab(Of RetroArcadeRobotBouncingProjectile)()
		retroArcadeRobotBouncingProjectile.transform.position = pos
		retroArcadeRobotBouncingProjectile.velocity = speed * MathUtils.AngleToDirection(angle)
		retroArcadeRobotBouncingProjectile.bounce = bounce
		Return retroArcadeRobotBouncingProjectile
	End Function

	' Token: 0x060028E0 RID: 10464 RVA: 0x0017C9B4 File Offset: 0x0017ADB4
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		Me.damageDealer.DealDamage(hit)
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x060028E1 RID: 10465 RVA: 0x0017C9CC File Offset: 0x0017ADCC
	Protected Overrides Sub FixedUpdate()
		MyBase.FixedUpdate()
		If MyBase.dead Then
			Return
		End If
		MyBase.transform.AddPosition(Me.velocity.x * CupheadTime.FixedDelta, Me.velocity.y * CupheadTime.FixedDelta, 0F)
		Dim radius As Single = MyBase.GetComponent(Of CircleCollider2D)().radius
		If Me.bounce Then
			If(Me.velocity.x < 0F AndAlso MyBase.transform.position.x < CSng(Level.Current.Left) + radius) OrElse (Me.velocity.x > 0F AndAlso MyBase.transform.position.x > CSng(Level.Current.Right) - radius) Then
				Me.velocity.x = Me.velocity.x * -1F
			End If
			If Me.velocity.y < 0F AndAlso MyBase.transform.position.y < CSng(Level.Current.Ground) + radius Then
				Me.velocity.y = Me.velocity.y * -1F
			End If
		End If
	End Sub

	' Token: 0x040031BA RID: 12730
	Private bounce As Boolean

	' Token: 0x040031BB RID: 12731
	Private attackDelay As Single

	' Token: 0x040031BC RID: 12732
	Private velocity As Vector2

	' Token: 0x040031BD RID: 12733
	Private parent As DevilLevelSittingDevil
End Class
