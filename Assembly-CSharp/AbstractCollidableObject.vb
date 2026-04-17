Imports System
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x020003EA RID: 1002
Public Class AbstractCollidableObject
	Inherits AbstractPausableComponent

	' Token: 0x06000D72 RID: 3442 RVA: 0x00008489 File Offset: 0x00006889
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.UnregisterAllCollisionChildren()
	End Sub

	' Token: 0x06000D73 RID: 3443 RVA: 0x00008497 File Offset: 0x00006897
	Protected Overridable Sub OnTriggerEnter2D(col As Collider2D)
		Me.checkCollision(col, CollisionPhase.Enter)
	End Sub

	' Token: 0x06000D74 RID: 3444 RVA: 0x000084A1 File Offset: 0x000068A1
	Protected Overridable Sub OnCollisionEnter2D(col As Collision2D)
		Me.checkCollision(col.collider, CollisionPhase.Enter)
	End Sub

	' Token: 0x06000D75 RID: 3445 RVA: 0x000084B0 File Offset: 0x000068B0
	Protected Overridable Sub OnTriggerStay2D(col As Collider2D)
		Me.checkCollision(col, CollisionPhase.Stay)
	End Sub

	' Token: 0x06000D76 RID: 3446 RVA: 0x000084BA File Offset: 0x000068BA
	Protected Overridable Sub OnCollisionStay2D(col As Collision2D)
		Me.checkCollision(col.collider, CollisionPhase.Stay)
	End Sub

	' Token: 0x06000D77 RID: 3447 RVA: 0x000084C9 File Offset: 0x000068C9
	Protected Overridable Sub OnTriggerExit2D(col As Collider2D)
		Me.checkCollision(col, CollisionPhase.[Exit])
	End Sub

	' Token: 0x06000D78 RID: 3448 RVA: 0x000084D3 File Offset: 0x000068D3
	Protected Overridable Sub OnCollisionExit2D(col As Collision2D)
		Me.checkCollision(col.collider, CollisionPhase.[Exit])
	End Sub

	' Token: 0x06000D79 RID: 3449 RVA: 0x000084E4 File Offset: 0x000068E4
	Protected Overridable Sub checkCollision(col As Collider2D, phase As CollisionPhase)
		Dim gameObject As GameObject = col.gameObject
		Me.OnCollision(gameObject, phase)
		If gameObject.CompareTag("Wall") Then
			Me.OnCollisionWalls(gameObject, phase)
		ElseIf gameObject.CompareTag("Ceiling") Then
			Me.OnCollisionCeiling(gameObject, phase)
		ElseIf gameObject.CompareTag("Ground") Then
			Me.OnCollisionGround(gameObject, phase)
		ElseIf gameObject.CompareTag("Enemy") Then
			If Me.allowCollisionEnemy Then
				Me.OnCollisionEnemy(gameObject, phase)
			End If
		ElseIf gameObject.CompareTag("EnemyProjectile") Then
			Me.OnCollisionEnemyProjectile(gameObject, phase)
		ElseIf gameObject.CompareTag("Player") Then
			If Me.allowCollisionPlayer Then
				Me.OnCollisionPlayer(gameObject, phase)
			End If
		ElseIf gameObject.CompareTag("PlayerProjectile") Then
			Me.OnCollisionPlayerProjectile(gameObject, phase)
		Else
			Me.OnCollisionOther(gameObject, phase)
		End If
	End Sub

	' Token: 0x17000242 RID: 578
	' (get) Token: 0x06000D7A RID: 3450 RVA: 0x000085E9 File Offset: 0x000069E9
	Protected Overridable ReadOnly Property allowCollisionPlayer As Boolean
		Get
			Return True
		End Get
	End Property

	' Token: 0x17000243 RID: 579
	' (get) Token: 0x06000D7B RID: 3451 RVA: 0x000085EC File Offset: 0x000069EC
	Protected Overridable ReadOnly Property allowCollisionEnemy As Boolean
		Get
			Return True
		End Get
	End Property

	' Token: 0x06000D7C RID: 3452 RVA: 0x000085EF File Offset: 0x000069EF
	Protected Overridable Sub OnCollision(hit As GameObject, phase As CollisionPhase)
	End Sub

	' Token: 0x06000D7D RID: 3453 RVA: 0x000085F1 File Offset: 0x000069F1
	Protected Overridable Sub OnCollisionWalls(hit As GameObject, phase As CollisionPhase)
	End Sub

	' Token: 0x06000D7E RID: 3454 RVA: 0x000085F3 File Offset: 0x000069F3
	Protected Overridable Sub OnCollisionCeiling(hit As GameObject, phase As CollisionPhase)
	End Sub

	' Token: 0x06000D7F RID: 3455 RVA: 0x000085F5 File Offset: 0x000069F5
	Protected Overridable Sub OnCollisionGround(hit As GameObject, phase As CollisionPhase)
	End Sub

	' Token: 0x06000D80 RID: 3456 RVA: 0x000085F7 File Offset: 0x000069F7
	Protected Overridable Sub OnCollisionEnemy(hit As GameObject, phase As CollisionPhase)
	End Sub

	' Token: 0x06000D81 RID: 3457 RVA: 0x000085F9 File Offset: 0x000069F9
	Protected Overridable Sub OnCollisionEnemyProjectile(hit As GameObject, phase As CollisionPhase)
	End Sub

	' Token: 0x06000D82 RID: 3458 RVA: 0x000085FB File Offset: 0x000069FB
	Protected Overridable Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
	End Sub

	' Token: 0x06000D83 RID: 3459 RVA: 0x000085FD File Offset: 0x000069FD
	Protected Overridable Sub OnCollisionPlayerProjectile(hit As GameObject, phase As CollisionPhase)
	End Sub

	' Token: 0x06000D84 RID: 3460 RVA: 0x000085FF File Offset: 0x000069FF
	Protected Overridable Sub OnCollisionOther(hit As GameObject, phase As CollisionPhase)
	End Sub

	' Token: 0x06000D85 RID: 3461 RVA: 0x00008604 File Offset: 0x00006A04
	Protected Sub RegisterCollisionChild(go As GameObject)
		Dim component As CollisionChild = go.GetComponent(Of CollisionChild)()
		If component Is Nothing Then
			Return
		End If
		Me.RegisterCollisionChild(component)
	End Sub

	' Token: 0x06000D86 RID: 3462 RVA: 0x0000862C File Offset: 0x00006A2C
	Public Sub RegisterCollisionChild(s As CollisionChild)
		Me.collisionChildren.Add(s)
		AddHandler s.OnAnyCollision, AddressOf Me.OnCollision
		AddHandler s.OnWallCollision, AddressOf Me.OnCollisionWalls
		AddHandler s.OnGroundCollision, AddressOf Me.OnCollisionGround
		AddHandler s.OnCeilingCollision, AddressOf Me.OnCollisionCeiling
		AddHandler s.OnPlayerCollision, AddressOf Me.OnCollisionPlayer
		AddHandler s.OnPlayerProjectileCollision, AddressOf Me.OnCollisionPlayerProjectile
		AddHandler s.OnEnemyCollision, AddressOf Me.OnCollisionEnemy
		AddHandler s.OnEnemyProjectileCollision, AddressOf Me.OnCollisionEnemyProjectile
		AddHandler s.OnOtherCollision, AddressOf Me.OnCollisionOther
	End Sub

	' Token: 0x06000D87 RID: 3463 RVA: 0x000086F0 File Offset: 0x00006AF0
	Protected Sub UnregisterCollisionChild(s As CollisionChild)
		If Me.collisionChildren.Contains(s) Then
			RemoveHandler s.OnAnyCollision, AddressOf Me.OnCollision
			RemoveHandler s.OnWallCollision, AddressOf Me.OnCollisionWalls
			RemoveHandler s.OnGroundCollision, AddressOf Me.OnCollisionGround
			RemoveHandler s.OnCeilingCollision, AddressOf Me.OnCollisionCeiling
			RemoveHandler s.OnPlayerCollision, AddressOf Me.OnCollisionPlayer
			RemoveHandler s.OnPlayerProjectileCollision, AddressOf Me.OnCollisionPlayerProjectile
			RemoveHandler s.OnEnemyCollision, AddressOf Me.OnCollisionEnemy
			RemoveHandler s.OnEnemyProjectileCollision, AddressOf Me.OnCollisionEnemyProjectile
			RemoveHandler s.OnOtherCollision, AddressOf Me.OnCollisionOther
			Me.collisionChildren.Remove(s)
		End If
	End Sub

	' Token: 0x06000D88 RID: 3464 RVA: 0x000087C8 File Offset: 0x00006BC8
	Protected Sub UnregisterAllCollisionChildren()
		For i As Integer = Me.collisionChildren.Count - 1 To 0 Step -1
			Me.UnregisterCollisionChild(Me.collisionChildren(i))
		Next
	End Sub

	' Token: 0x04001702 RID: 5890
	Private collisionChildren As List(Of CollisionChild) = New List(Of CollisionChild)()
End Class
