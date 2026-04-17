Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000594 RID: 1428
Public Class DevilLevelPitchforkWheelProjectile
	Inherits AbstractProjectile

	' Token: 0x17000358 RID: 856
	' (get) Token: 0x06001B5E RID: 7006 RVA: 0x000FA958 File Offset: 0x000F8D58
	Protected Overrides ReadOnly Property DestroyedAfterLeavingScreen As Boolean
		Get
			Return True
		End Get
	End Property

	' Token: 0x17000359 RID: 857
	' (get) Token: 0x06001B5F RID: 7007 RVA: 0x000FA95B File Offset: 0x000F8D5B
	Protected Overrides ReadOnly Property DestroyLifetime As Single
		Get
			Return -1F
		End Get
	End Property

	' Token: 0x06001B60 RID: 7008 RVA: 0x000FA964 File Offset: 0x000F8D64
	Public Function Create(pos As Vector2, attackDelay As Single, speed As Single, parent As DevilLevelSittingDevil) As DevilLevelPitchforkWheelProjectile
		Dim devilLevelPitchforkWheelProjectile As DevilLevelPitchforkWheelProjectile = Me.InstantiatePrefab(Of DevilLevelPitchforkWheelProjectile)()
		devilLevelPitchforkWheelProjectile.transform.position = pos
		devilLevelPitchforkWheelProjectile.attackDelay = attackDelay
		devilLevelPitchforkWheelProjectile.speed = speed
		devilLevelPitchforkWheelProjectile.state = DevilLevelPitchforkWheelProjectile.State.Idle
		devilLevelPitchforkWheelProjectile.StartCoroutine(devilLevelPitchforkWheelProjectile.main_cr())
		devilLevelPitchforkWheelProjectile.parent = parent
		Return devilLevelPitchforkWheelProjectile
	End Function

	' Token: 0x06001B61 RID: 7009 RVA: 0x000FA9B4 File Offset: 0x000F8DB4
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.parent Is Nothing Then
			Me.Die()
		End If
	End Sub

	' Token: 0x06001B62 RID: 7010 RVA: 0x000FA9D3 File Offset: 0x000F8DD3
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x06001B63 RID: 7011 RVA: 0x000FA9F4 File Offset: 0x000F8DF4
	Protected Overrides Sub FixedUpdate()
		MyBase.FixedUpdate()
		If Not MyBase.dead AndAlso Me.state <> DevilLevelPitchforkWheelProjectile.State.Idle Then
			MyBase.transform.AddPosition(Me.velocity.x * CupheadTime.FixedDelta, Me.velocity.y * CupheadTime.FixedDelta, 0F)
		End If
	End Sub

	' Token: 0x06001B64 RID: 7012 RVA: 0x000FAA50 File Offset: 0x000F8E50
	Private Iterator Function main_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, Me.attackDelay)
		Me.state = DevilLevelPitchforkWheelProjectile.State.Attacking
		Me.velocity = Me.speed * (PlayerManager.GetNext().center - MyBase.transform.position).normalized
		While Me.state = DevilLevelPitchforkWheelProjectile.State.Attacking
			Dim colliderRadius As Single = MyBase.GetComponent(Of CircleCollider2D)().radius
			If MyBase.transform.position.x < CSng(Level.Current.Left) + colliderRadius OrElse MyBase.transform.position.x > CSng(Level.Current.Right) - colliderRadius OrElse MyBase.transform.position.y < CSng(Level.Current.Ground) + colliderRadius OrElse MyBase.transform.position.y > CSng(Level.Current.Ceiling) - colliderRadius Then
				Me.velocity *= -1F
				Me.state = DevilLevelPitchforkWheelProjectile.State.Returning
			End If
			Yield New WaitForFixedUpdate()
		End While
		Return
	End Function

	' Token: 0x06001B65 RID: 7013 RVA: 0x000FAA6B File Offset: 0x000F8E6B
	Protected Overrides Sub Die()
		MyBase.Die()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x04002499 RID: 9369
	Public state As DevilLevelPitchforkWheelProjectile.State

	' Token: 0x0400249A RID: 9370
	Private attackDelay As Single

	' Token: 0x0400249B RID: 9371
	Private velocity As Vector2

	' Token: 0x0400249C RID: 9372
	Private speed As Single

	' Token: 0x0400249D RID: 9373
	Private parent As DevilLevelSittingDevil

	' Token: 0x02000595 RID: 1429
	Public Enum State
		' Token: 0x0400249F RID: 9375
		Idle
		' Token: 0x040024A0 RID: 9376
		Attacking
		' Token: 0x040024A1 RID: 9377
		Returning
	End Enum
End Class
