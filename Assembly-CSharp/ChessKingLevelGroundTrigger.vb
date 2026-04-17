Imports System
Imports UnityEngine

' Token: 0x0200053B RID: 1339
Public Class ChessKingLevelGroundTrigger
	Inherits AbstractCollidableObject

	' Token: 0x17000333 RID: 819
	' (get) Token: 0x06001856 RID: 6230 RVA: 0x000DC77F File Offset: 0x000DAB7F
	' (set) Token: 0x06001857 RID: 6231 RVA: 0x000DC787 File Offset: 0x000DAB87
	Public Property PLAYER_FALLEN As Boolean

	' Token: 0x06001858 RID: 6232 RVA: 0x000DC790 File Offset: 0x000DAB90
	Public Sub CheckPlayer(checkPlayer As Boolean)
		Me.checkingPlayer = checkPlayer
		Me.PLAYER_FALLEN = False
	End Sub

	' Token: 0x06001859 RID: 6233 RVA: 0x000DC7A0 File Offset: 0x000DABA0
	Private Sub Update()
		If Me.checkingPlayer Then
			If PlayerManager.GetPlayer(PlayerId.PlayerOne).transform.position.y < MyBase.transform.position.y Then
				Me.PLAYER_FALLEN = True
			Else
				Me.PLAYER_FALLEN = False
			End If
		End If
	End Sub

	' Token: 0x0600185A RID: 6234 RVA: 0x000DC7FC File Offset: 0x000DABFC
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		Gizmos.DrawLine(New Vector3(-800F, MyBase.transform.position.y), New Vector3(800F, MyBase.transform.position.y))
	End Sub

	' Token: 0x0400218D RID: 8589
	Private checkingPlayer As Boolean
End Class
