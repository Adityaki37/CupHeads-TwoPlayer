Imports System
Imports UnityEngine

' Token: 0x02000715 RID: 1813
Public Class OldManLevelStomachCeiling
	Inherits MonoBehaviour

	' Token: 0x0600276B RID: 10091 RVA: 0x00172140 File Offset: 0x00170540
	Private Sub Update()
		Me.currentPosition = CInt((Mathf.Clamp(Me.leader.GetPosition(), 0F, 0.99F) * CSng(Me.sprites.Length)))
		Me.timeSinceChangeFrame += CupheadTime.Delta
		If Me.lastPosition <> Me.currentPosition Then
			Me.lastPosition = Me.currentPosition
			Me.timeSinceChangeFrame = 0F
		End If
		Me.offset = CInt((Me.timeSinceChangeFrame / 0.16666667F)) Mod 2 * CInt((-CInt(Mathf.Sign(Me.leader.GetPosition() - 0.5F))))
		Me.rend.sprite = Me.sprites(Me.currentPosition + Me.offset)
	End Sub

	' Token: 0x04003025 RID: 12325
	Private Const MAX_FRAME_TIME As Single = 0.16666667F

	' Token: 0x04003026 RID: 12326
	<SerializeField()>
	Private rend As SpriteRenderer

	' Token: 0x04003027 RID: 12327
	<SerializeField()>
	Private sprites As Sprite()

	' Token: 0x04003028 RID: 12328
	<SerializeField()>
	Private leader As OldManLevelGnomeLeader

	' Token: 0x04003029 RID: 12329
	Private currentPosition As Integer

	' Token: 0x0400302A RID: 12330
	Private lastPosition As Integer

	' Token: 0x0400302B RID: 12331
	Private timeSinceChangeFrame As Single

	' Token: 0x0400302C RID: 12332
	Private offset As Integer
End Class
