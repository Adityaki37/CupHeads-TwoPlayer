Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000556 RID: 1366
Public Class ClownLevelBackgroundLights
	Inherits AbstractPausableComponent

	' Token: 0x0600197F RID: 6527 RVA: 0x000E7754 File Offset: 0x000E5B54
	Private Sub Start()
		Me.lightVersion = MyBase.GetComponent(Of SpriteRenderer)()
		MyBase.StartCoroutine(Me.lights_cr())
		If Me.occasionalFlicker Then
			MyBase.StartCoroutine(Me.flicker_cr())
		End If
	End Sub

	' Token: 0x06001980 RID: 6528 RVA: 0x000E7788 File Offset: 0x000E5B88
	Private Iterator Function lights_cr() As IEnumerator
		While True
			Me.fadeTime = Global.UnityEngine.Random.Range(Me.fadeDurationMin, Me.fadeDurationMax)
			Me.getSecond = Global.UnityEngine.Random.Range(Me.waitMinSecond, Me.waitMaxSecond)
			If Me.fadeIn Then
				Dim t As Single = 0F
				While t < Me.fadeTime
					Me.lightVersion.color = New Color(1F, 1F, 1F, t / Me.fadeTime)
					t += CupheadTime.Delta
					Yield Nothing
				End While
				Me.lightVersion.color = New Color(1F, 1F, 1F, 1F)
			Else
				Dim t2 As Single = 0F
				While t2 < Me.fadeTime
					Me.lightVersion.color = New Color(1F, 1F, 1F, 1F - t2 / Me.fadeTime)
					t2 += CupheadTime.Delta
					Yield Nothing
				End While
				Me.lightVersion.color = New Color(1F, 1F, 1F, 0F)
				Yield CupheadTime.WaitForSeconds(Me, Me.getSecond)
			End If
			Me.fadeIn = Not Me.fadeIn
		End While
		Return
	End Function

	' Token: 0x06001981 RID: 6529 RVA: 0x000E77A4 File Offset: 0x000E5BA4
	Private Iterator Function flicker_cr() As IEnumerator
		While True
			Dim waitTime As Single = Global.UnityEngine.Random.Range(Me.fadeWaitMinSecond, Me.fadeWaitMaxSecond)
			Dim flickerTime As Single = Global.UnityEngine.Random.Range(Me.flickerMinTime, Me.flickerMaxTime)
			Dim t As Single = 0F
			While t < flickerTime
				Me.fadeTime = 0.08F
				t += CupheadTime.Delta
				Yield Nothing
			End While
			Me.fadeTime = 0F
			Yield CupheadTime.WaitForSeconds(Me, waitTime)
		End While
		Return
	End Function

	' Token: 0x04002298 RID: 8856
	Private waitMinSecond As Single

	' Token: 0x04002299 RID: 8857
	Private waitMaxSecond As Single = 1F

	' Token: 0x0400229A RID: 8858
	Private fadeDurationMin As Single = 0.5F

	' Token: 0x0400229B RID: 8859
	Private fadeDurationMax As Single = 2F

	' Token: 0x0400229C RID: 8860
	<SerializeField()>
	Private occasionalFlicker As Boolean

	' Token: 0x0400229D RID: 8861
	Private fadeWaitMinSecond As Single = 5F

	' Token: 0x0400229E RID: 8862
	Private fadeWaitMaxSecond As Single = 10F

	' Token: 0x0400229F RID: 8863
	Private flickerMinTime As Single = 1F

	' Token: 0x040022A0 RID: 8864
	Private flickerMaxTime As Single = 5F

	' Token: 0x040022A1 RID: 8865
	Private lightVersion As SpriteRenderer

	' Token: 0x040022A2 RID: 8866
	Private getSecond As Single

	' Token: 0x040022A3 RID: 8867
	Private fadeTime As Single

	' Token: 0x040022A4 RID: 8868
	Private fadeIn As Boolean
End Class
