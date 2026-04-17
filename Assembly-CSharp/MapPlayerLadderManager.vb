Imports System
Imports UnityEngine

' Token: 0x0200097D RID: 2429
Public Class MapPlayerLadderManager
	Inherits AbstractMapPlayerComponent

	' Token: 0x1700049A RID: 1178
	' (get) Token: 0x060038B1 RID: 14513 RVA: 0x00204960 File Offset: 0x00202D60
	' (set) Token: 0x060038B2 RID: 14514 RVA: 0x00204968 File Offset: 0x00202D68
	Public Property Current As MapPlayerLadderObject

	' Token: 0x060038B3 RID: 14515 RVA: 0x00204971 File Offset: 0x00202D71
	Protected Overrides Sub Awake()
		MyBase.Awake()
	End Sub

	' Token: 0x060038B4 RID: 14516 RVA: 0x00204979 File Offset: 0x00202D79
	Protected Overrides Sub OnLadderEnter(point As Vector2, ladder As MapPlayerLadderObject, location As MapLadder.Location)
		MyBase.OnLadderEnter(point, ladder, location)
		MyBase.GetComponent(Of Collider2D)().enabled = False
		Me.Current = ladder
	End Sub

	' Token: 0x060038B5 RID: 14517 RVA: 0x00204997 File Offset: 0x00202D97
	Protected Overrides Sub OnLadderExitComplete()
		MyBase.OnLadderExitComplete()
		MyBase.GetComponent(Of Collider2D)().enabled = True
		Me.Current = Nothing
	End Sub
End Class
