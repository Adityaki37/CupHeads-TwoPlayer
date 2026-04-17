Imports System
Imports UnityEngine

' Token: 0x0200045E RID: 1118
Public Class JitterGameObject
	Inherits MonoBehaviour

	' Token: 0x060010F8 RID: 4344 RVA: 0x000A2665 File Offset: 0x000A0A65
	Private Sub Start()
		Me.jitterDelay = 0.083333336F
		Me.tr = MyBase.transform
		Me.startingPosition = Me.tr.position
	End Sub

	' Token: 0x060010F9 RID: 4345 RVA: 0x000A2690 File Offset: 0x000A0A90
	Private Sub Update()
		Me.currentJitterDelay -= CupheadTime.Delta
		If Me.currentJitterDelay <= 0F Then
			Me.currentJitterDelay = Me.jitterDelay
			Dim num As Single = Global.UnityEngine.Random.Range(0F, 6.2831855F)
			Me.tr.position = Me.startingPosition + New Vector3(Mathf.Cos(num), Mathf.Sin(num), 0F) * Me.jitterAmplitude
		End If
	End Sub

	' Token: 0x04001A5D RID: 6749
	Public jitterAmplitude As Single = 0.1F

	' Token: 0x04001A5E RID: 6750
	Private jitterDelay As Single = 0.1F

	' Token: 0x04001A5F RID: 6751
	Private currentJitterDelay As Single

	' Token: 0x04001A60 RID: 6752
	Private tr As Transform

	' Token: 0x04001A61 RID: 6753
	Private startingPosition As Vector3
End Class
