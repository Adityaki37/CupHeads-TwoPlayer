Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020008D2 RID: 2258
Public Class HarbourPlatformingLevelStarfishBubble
	Inherits Effect

	' Token: 0x060034DD RID: 13533 RVA: 0x001EBDB7 File Offset: 0x001EA1B7
	Private Sub Start()
		Me.sinWaveStrength = Global.UnityEngine.Random.Range(0.4F, 0.9F)
		Me.speed = Global.UnityEngine.Random.Range(50F, 100F)
		MyBase.StartCoroutine(Me.deathrotation_cr())
	End Sub

	' Token: 0x060034DE RID: 13534 RVA: 0x001EBDF0 File Offset: 0x001EA1F0
	Private Iterator Function deathrotation_cr() As IEnumerator
		Dim totalTime As Single = 0F
		Dim maxTime As Single = Global.UnityEngine.Random.Range(4F, 7F)
		Dim t As Single = Global.UnityEngine.Random.Range(0F, 6.2831855F)
		While totalTime < maxTime
			totalTime += CupheadTime.Delta
			t += CupheadTime.Delta
			MyBase.transform.SetPosition(New Single?(MyBase.transform.position.x + Mathf.Sin(t) * Me.sinWaveStrength * CupheadTime.Delta * 60F), Nothing, Nothing)
			MyBase.transform.AddPosition(0F, Me.speed * CupheadTime.Delta, 0F)
			Yield Nothing
		End While
		MyBase.animator.Play("Pop")
		Yield Nothing
		Return
	End Function

	' Token: 0x04003D06 RID: 15622
	Private Const ROTATE_FRAME_TIME As Single = 0.083333336F

	' Token: 0x04003D07 RID: 15623
	Private pos As Vector3

	' Token: 0x04003D08 RID: 15624
	Private speed As Single

	' Token: 0x04003D09 RID: 15625
	Private sinWaveStrength As Single
End Class
