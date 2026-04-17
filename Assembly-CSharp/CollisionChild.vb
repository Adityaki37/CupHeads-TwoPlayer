Imports System
Imports System.Diagnostics
Imports UnityEngine

' Token: 0x02000B23 RID: 2851
Public Class CollisionChild
	Inherits AbstractCollidableObject

	' Token: 0x140000C9 RID: 201
	' (add) Token: 0x060044FA RID: 17658 RVA: 0x0011DA2C File Offset: 0x0011BE2C
	' (remove) Token: 0x060044FB RID: 17659 RVA: 0x0011DA64 File Offset: 0x0011BE64
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnAnyCollision As CollisionChild.OnCollisionHandler

	' Token: 0x140000CA RID: 202
	' (add) Token: 0x060044FC RID: 17660 RVA: 0x0011DA9C File Offset: 0x0011BE9C
	' (remove) Token: 0x060044FD RID: 17661 RVA: 0x0011DAD4 File Offset: 0x0011BED4
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnWallCollision As CollisionChild.OnCollisionHandler

	' Token: 0x140000CB RID: 203
	' (add) Token: 0x060044FE RID: 17662 RVA: 0x0011DB0C File Offset: 0x0011BF0C
	' (remove) Token: 0x060044FF RID: 17663 RVA: 0x0011DB44 File Offset: 0x0011BF44
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnGroundCollision As CollisionChild.OnCollisionHandler

	' Token: 0x140000CC RID: 204
	' (add) Token: 0x06004500 RID: 17664 RVA: 0x0011DB7C File Offset: 0x0011BF7C
	' (remove) Token: 0x06004501 RID: 17665 RVA: 0x0011DBB4 File Offset: 0x0011BFB4
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnCeilingCollision As CollisionChild.OnCollisionHandler

	' Token: 0x140000CD RID: 205
	' (add) Token: 0x06004502 RID: 17666 RVA: 0x0011DBEC File Offset: 0x0011BFEC
	' (remove) Token: 0x06004503 RID: 17667 RVA: 0x0011DC24 File Offset: 0x0011C024
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnPlayerCollision As CollisionChild.OnCollisionHandler

	' Token: 0x140000CE RID: 206
	' (add) Token: 0x06004504 RID: 17668 RVA: 0x0011DC5C File Offset: 0x0011C05C
	' (remove) Token: 0x06004505 RID: 17669 RVA: 0x0011DC94 File Offset: 0x0011C094
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnPlayerProjectileCollision As CollisionChild.OnCollisionHandler

	' Token: 0x140000CF RID: 207
	' (add) Token: 0x06004506 RID: 17670 RVA: 0x0011DCCC File Offset: 0x0011C0CC
	' (remove) Token: 0x06004507 RID: 17671 RVA: 0x0011DD04 File Offset: 0x0011C104
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnEnemyCollision As CollisionChild.OnCollisionHandler

	' Token: 0x140000D0 RID: 208
	' (add) Token: 0x06004508 RID: 17672 RVA: 0x0011DD3C File Offset: 0x0011C13C
	' (remove) Token: 0x06004509 RID: 17673 RVA: 0x0011DD74 File Offset: 0x0011C174
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnEnemyProjectileCollision As CollisionChild.OnCollisionHandler

	' Token: 0x140000D1 RID: 209
	' (add) Token: 0x0600450A RID: 17674 RVA: 0x0011DDAC File Offset: 0x0011C1AC
	' (remove) Token: 0x0600450B RID: 17675 RVA: 0x0011DDE4 File Offset: 0x0011C1E4
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnOtherCollision As CollisionChild.OnCollisionHandler

	' Token: 0x0600450C RID: 17676 RVA: 0x0011DE1A File Offset: 0x0011C21A
	Public Function ForwardParry(<System.Runtime.InteropServices.OutAttribute()> ByRef collisionParent As AbstractCollidableObject) As Boolean
		collisionParent = Me.collisionParent
		Return Me.forwardParry
	End Function

	' Token: 0x0600450D RID: 17677 RVA: 0x0011DE2A File Offset: 0x0011C22A
	Private Sub Start()
		If Me.collisionParent IsNot Nothing Then
			Me.collisionParent.RegisterCollisionChild(Me)
		End If
	End Sub

	' Token: 0x0600450E RID: 17678 RVA: 0x0011DE49 File Offset: 0x0011C249
	Protected Overrides Sub OnCollision(hit As GameObject, phase As CollisionPhase)
		If Me.OnAnyCollision IsNot Nothing Then
			Me.OnAnyCollision(hit, phase)
		End If
	End Sub

	' Token: 0x0600450F RID: 17679 RVA: 0x0011DE63 File Offset: 0x0011C263
	Protected Overrides Sub OnCollisionWalls(hit As GameObject, phase As CollisionPhase)
		If Me.OnWallCollision IsNot Nothing Then
			Me.OnWallCollision(hit, phase)
		End If
	End Sub

	' Token: 0x06004510 RID: 17680 RVA: 0x0011DE7D File Offset: 0x0011C27D
	Protected Overrides Sub OnCollisionGround(hit As GameObject, phase As CollisionPhase)
		If Me.OnGroundCollision IsNot Nothing Then
			Me.OnGroundCollision(hit, phase)
		End If
	End Sub

	' Token: 0x06004511 RID: 17681 RVA: 0x0011DE97 File Offset: 0x0011C297
	Protected Overrides Sub OnCollisionCeiling(hit As GameObject, phase As CollisionPhase)
		If Me.OnCeilingCollision IsNot Nothing Then
			Me.OnCeilingCollision(hit, phase)
		End If
	End Sub

	' Token: 0x06004512 RID: 17682 RVA: 0x0011DEB1 File Offset: 0x0011C2B1
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If Me.OnPlayerCollision IsNot Nothing Then
			Me.OnPlayerCollision(hit, phase)
		End If
	End Sub

	' Token: 0x06004513 RID: 17683 RVA: 0x0011DECB File Offset: 0x0011C2CB
	Protected Overrides Sub OnCollisionPlayerProjectile(hit As GameObject, phase As CollisionPhase)
		If Me.OnPlayerProjectileCollision IsNot Nothing Then
			Me.OnPlayerProjectileCollision(hit, phase)
		End If
	End Sub

	' Token: 0x06004514 RID: 17684 RVA: 0x0011DEE5 File Offset: 0x0011C2E5
	Protected Overrides Sub OnCollisionEnemy(hit As GameObject, phase As CollisionPhase)
		If Me.OnEnemyCollision IsNot Nothing Then
			Me.OnEnemyCollision(hit, phase)
		End If
	End Sub

	' Token: 0x06004515 RID: 17685 RVA: 0x0011DEFF File Offset: 0x0011C2FF
	Protected Overrides Sub OnCollisionEnemyProjectile(hit As GameObject, phase As CollisionPhase)
		If Me.OnEnemyProjectileCollision IsNot Nothing Then
			Me.OnEnemyProjectileCollision(hit, phase)
		End If
	End Sub

	' Token: 0x06004516 RID: 17686 RVA: 0x0011DF19 File Offset: 0x0011C319
	Protected Overrides Sub OnCollisionOther(hit As GameObject, phase As CollisionPhase)
		If Me.OnOtherCollision IsNot Nothing Then
			Me.OnOtherCollision(hit, phase)
		End If
	End Sub

	' Token: 0x04004ACF RID: 19151
	<SerializeField()>
	<Tooltip("OPTIONAL: Drag collision parent to this slot to register all collision events to this child. If null, no collisions are registered.")>
	Private collisionParent As AbstractCollidableObject

	' Token: 0x04004AD0 RID: 19152
	<SerializeField()>
	Private forwardParry As Boolean

	' Token: 0x02000B24 RID: 2852
	' (Invoke) Token: 0x06004518 RID: 17688
	Public Delegate Sub OnCollisionHandler(hit As GameObject, phase As CollisionPhase)
End Class
