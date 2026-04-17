Imports System
Imports UnityEngine

' Token: 0x020008AF RID: 2223
Public Class StartHotdog
	Inherits MonoBehaviour

	' Token: 0x060033D1 RID: 13265 RVA: 0x001E14A2 File Offset: 0x001DF8A2
	Private Sub OnTriggerEnter2D(c As Collider2D)
		If c.tag = "Player" Then
			Me.hotdog.ProjectilesCanHit = True
			MyBase.gameObject.SetActive(False)
		End If
	End Sub

	' Token: 0x04003C1E RID: 15390
	Private Const PlayerTag As String = "Player"

	' Token: 0x04003C1F RID: 15391
	<SerializeField()>
	Private hotdog As CircusPlatformingLevelHotdog
End Class
