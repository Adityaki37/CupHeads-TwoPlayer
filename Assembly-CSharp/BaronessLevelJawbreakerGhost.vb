Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020004F2 RID: 1266
Public Class BaronessLevelJawbreakerGhost
	Inherits AbstractCollidableObject

	' Token: 0x0600162E RID: 5678 RVA: 0x000C71A0 File Offset: 0x000C55A0
	Private Sub Start()
		MyBase.StartCoroutine(Me.deathrotation_cr())
	End Sub

	' Token: 0x0600162F RID: 5679 RVA: 0x000C71B0 File Offset: 0x000C55B0
	Private Iterator Function deathrotation_cr() As IEnumerator
		Dim frameTime As Single = 0F
		Dim t As Single = 0F
		While True
			frameTime += CupheadTime.Delta
			Me.deathPosition = New Vector3(MyBase.transform.position.x + Mathf.Sin(t) * Me.sinWaveStrength * CupheadTime.Delta * 60F, MyBase.transform.position.y + Me.deathSpeed, 0F)
			t += CupheadTime.Delta
			If frameTime > 0.083333336F Then
				frameTime -= 0.083333336F
				MyBase.transform.up = (Me.deathPosition - MyBase.transform.position).normalized * CupheadTime.Delta
			End If
			If CupheadTime.Delta IsNot 0F Then
				MyBase.transform.position = Me.deathPosition
			End If
			If MyBase.transform.position.y > 540F Then
				Exit For
			End If
			Yield CupheadTime.WaitForSeconds(Me, CupheadTime.Delta)
		End While
		Me.Die()
		Return
	End Function

	' Token: 0x06001630 RID: 5680 RVA: 0x000C71CB File Offset: 0x000C55CB
	Private Sub Die()
		Me.StopAllCoroutines()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x04001F7A RID: 8058
	Private Const ROTATE_FRAME_TIME As Single = 0.083333336F

	' Token: 0x04001F7B RID: 8059
	Private deathPosition As Vector3

	' Token: 0x04001F7C RID: 8060
	Private deathSpeed As Single = 2.3F

	' Token: 0x04001F7D RID: 8061
	Private sinWaveStrength As Single = 0.4F
End Class
