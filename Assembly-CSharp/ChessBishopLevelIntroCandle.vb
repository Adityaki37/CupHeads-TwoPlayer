Imports System
Imports UnityEngine

' Token: 0x0200053A RID: 1338
Public Class ChessBishopLevelIntroCandle
	Inherits MonoBehaviour

	' Token: 0x06001853 RID: 6227 RVA: 0x000DC720 File Offset: 0x000DAB20
	Private Sub AniEvent_StartMove()
		Me.moving = True
		Me.glow.SetActive(False)
	End Sub

	' Token: 0x06001854 RID: 6228 RVA: 0x000DC738 File Offset: 0x000DAB38
	Private Sub Update()
		Me.shadow.transform.position = New Vector3(Me.shadow.transform.position.x, -40F)
	End Sub

	' Token: 0x04002189 RID: 8585
	Public moving As Boolean

	' Token: 0x0400218A RID: 8586
	<SerializeField()>
	Private glow As GameObject

	' Token: 0x0400218B RID: 8587
	<SerializeField()>
	Private shadow As GameObject
End Class
