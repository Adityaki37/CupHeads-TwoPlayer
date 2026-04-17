Imports System
Imports System.Diagnostics
Imports UnityEngine

' Token: 0x02000930 RID: 2352
Public Class MapCastleZoneCollider
	Inherits AbstractCollidableObject

	' Token: 0x14000067 RID: 103
	' (add) Token: 0x06003707 RID: 14087 RVA: 0x001FB674 File Offset: 0x001F9A74
	' (remove) Token: 0x06003708 RID: 14088 RVA: 0x001FB6AC File Offset: 0x001F9AAC
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnMapCastleZoneCollision As MapCastleZoneCollider.MapCastleZoneCollision

	' Token: 0x06003709 RID: 14089 RVA: 0x001FB6E4 File Offset: 0x001F9AE4
	Protected Overrides Sub OnDrawGizmosSelected()
		MyBase.OnDrawGizmosSelected()
		Dim vector As Vector2 = Me.interactionPoint.position
		Gizmos.DrawWireSphere(vector, 0.2F)
		Gizmos.color = Color.red
		Gizmos.DrawWireSphere(vector + Me.returnPositions.singlePlayer, 0.2F)
		Gizmos.color = Color.red
		Gizmos.DrawWireCube(vector + Me.returnPositions.playerOne, Vector3.one * 0.2F)
		Gizmos.color = Color.blue
		Gizmos.DrawWireCube(vector + Me.returnPositions.playerTwo, Vector3.one * 0.2F)
	End Sub

	' Token: 0x0600370A RID: 14090 RVA: 0x001FB7AC File Offset: 0x001F9BAC
	Protected Overrides Sub OnCollision(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollision(hit, phase)
		If Not hit.CompareTag("Player_Map") Then
			Return
		End If
		If(phase = CollisionPhase.Enter OrElse phase = CollisionPhase.[Exit]) AndAlso Me.OnMapCastleZoneCollision IsNot Nothing Then
			Me.OnMapCastleZoneCollision(Me, hit, phase)
		End If
	End Sub

	' Token: 0x04003F3B RID: 16187
	<SerializeField()>
	Public zone As MapCastleZones.Zone

	' Token: 0x04003F3C RID: 16188
	<SerializeField()>
	Public interactionPoint As Transform

	' Token: 0x04003F3D RID: 16189
	<SerializeField()>
	Public enableLadderShadow As Boolean = True

	' Token: 0x04003F3E RID: 16190
	<SerializeField()>
	Public returnPositions As AbstractMapInteractiveEntity.PositionProperties

	' Token: 0x02000931 RID: 2353
	' (Invoke) Token: 0x0600370C RID: 14092
	Public Delegate Sub MapCastleZoneCollision(collider As MapCastleZoneCollider, other As GameObject, phase As CollisionPhase)
End Class
