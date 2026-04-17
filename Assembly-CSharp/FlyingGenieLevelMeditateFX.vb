Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200066F RID: 1647
Public Class FlyingGenieLevelMeditateFX
	Inherits Effect

	' Token: 0x060022A3 RID: 8867 RVA: 0x0014569C File Offset: 0x00143A9C
	Private Sub Start()
		MyBase.StartCoroutine(Me.effect_cr())
	End Sub

	' Token: 0x060022A4 RID: 8868 RVA: 0x001456AC File Offset: 0x00143AAC
	Private Iterator Function effect_cr() As IEnumerator
		Dim sprite As SpriteRenderer = MyBase.GetComponent(Of SpriteRenderer)()
		sprite.color = New Color(1F, 1F, 1F, 0F)
		Dim t As Single = 0F
		Dim time As Single = 1F
		While t < time
			t += CupheadTime.Delta
			sprite.color = New Color(1F, 1F, 1F, t / time)
			Yield Nothing
		End While
		sprite.color = New Color(1F, 1F, 1F, 1F)
		t = 0F
		While True
			Me.frameTime += CupheadTime.Delta
			t += CupheadTime.Delta
			If Me.frameTime > 0.083333336F Then
				MyBase.transform.SetEulerAngles(Nothing, Nothing, New Single?(360F * t))
				Me.frameTime -= 0.083333336F
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060022A5 RID: 8869 RVA: 0x001456C7 File Offset: 0x00143AC7
	Public Sub EndEffect()
		Me.StopAllCoroutines()
		MyBase.StartCoroutine(Me.end_effect_cr())
	End Sub

	' Token: 0x060022A6 RID: 8870 RVA: 0x001456DC File Offset: 0x00143ADC
	Private Iterator Function end_effect_cr() As IEnumerator
		Dim t As Single = 0F
		Dim time As Single = 1F
		MyBase.transform.SetEulerAngles(Nothing, Nothing, New Single?(0F))
		While t < time
			t += CupheadTime.Delta
			MyBase.transform.SetScale(New Single?(1F - t / time), New Single?(1F - t / time), Nothing)
			Yield Nothing
		End While
		Me.OnEffectComplete()
		Return
	End Function

	' Token: 0x04002B4A RID: 11082
	Private Const FRAME_TIME As Single = 0.083333336F

	' Token: 0x04002B4B RID: 11083
	Private frameTime As Single
End Class
