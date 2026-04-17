Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000439 RID: 1081
Public Class LevelLightFlicker
	Inherits AbstractPausableComponent

	' Token: 0x06000FE4 RID: 4068 RVA: 0x0009D5E1 File Offset: 0x0009B9E1
	Private Sub Start()
		Me.sprite = MyBase.GetComponent(Of SpriteRenderer)()
	End Sub

	' Token: 0x06000FE5 RID: 4069 RVA: 0x0009D5F0 File Offset: 0x0009B9F0
	Private Iterator Function flicker_cr() As IEnumerator
		Dim flickerTime As Single = 0.3F
		While True
			Dim counter As Integer = 0
			Dim waitTime As Single = Global.UnityEngine.Random.Range(Me.fadeWaitMinSecond, Me.fadeWaitMaxSecond)
			Dim t As Single = 0F
			Yield CupheadTime.WaitForSeconds(Me, waitTime)
			While counter < Me.countUntilPause
				While t < flickerTime
					Me.sprite.color = New Color(1F, 1F, 1F, 1F - t / flickerTime)
					t += CupheadTime.Delta
					Yield Nothing
				End While
				t = 0F
				Me.sprite.color = New Color(1F, 1F, 1F, 0F)
				While t < flickerTime
					Me.sprite.color = New Color(1F, 1F, 1F, t / flickerTime)
					t += CupheadTime.Delta
					Yield Nothing
				End While
				Me.sprite.color = New Color(1F, 1F, 1F, 1F)
				counter += 1
				Yield Nothing
			End While
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0400197C RID: 6524
	<SerializeField()>
	Private fadeWaitMinSecond As Single = 8F

	' Token: 0x0400197D RID: 6525
	<SerializeField()>
	Private fadeWaitMaxSecond As Single = 25F

	' Token: 0x0400197E RID: 6526
	<SerializeField()>
	Private countUntilPause As Integer = 3

	' Token: 0x0400197F RID: 6527
	Private sprite As SpriteRenderer
End Class
