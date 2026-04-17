Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200076A RID: 1898
Public Class RetroArcadeWormPlatform
	Inherits LevelPlatform

	' Token: 0x06002951 RID: 10577 RVA: 0x0018185E File Offset: 0x0017FC5E
	Public Sub Rise()
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x06002952 RID: 10578 RVA: 0x00181870 File Offset: 0x0017FC70
	Private Iterator Function move_cr() As IEnumerator
		Dim moveTime As Single = 1F
		Dim t As Single = 0F
		While t < moveTime
			t += CupheadTime.FixedDelta
			MyBase.transform.AddPosition(0F, 50F * CupheadTime.FixedDelta, 0F)
			Yield New WaitForFixedUpdate()
		End While
		Return
	End Function

	' Token: 0x04003253 RID: 12883
	Private Const MOVE_Y As Single = 50F

	' Token: 0x04003254 RID: 12884
	Private Const MOVE_Y_SPEED As Single = 50F
End Class
