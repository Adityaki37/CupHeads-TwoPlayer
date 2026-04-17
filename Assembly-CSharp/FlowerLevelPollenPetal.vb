Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000610 RID: 1552
Public Class FlowerLevelPollenPetal
	Inherits Effect

	' Token: 0x06001F4D RID: 8013 RVA: 0x0011F940 File Offset: 0x0011DD40
	Private Sub Start()
		Dim text As String = If((Not Rand.Bool()), "Petal_B", "Petal_A")
		MyBase.animator.Play(text)
		MyBase.StartCoroutine(Me.fall_cr())
		MyBase.StartCoroutine(Me.fade_cr())
	End Sub

	' Token: 0x06001F4E RID: 8014 RVA: 0x0011F990 File Offset: 0x0011DD90
	Private Iterator Function fall_cr() As IEnumerator
		Dim fallSpeed As Single = 100F
		While True
			MyBase.transform.position -= Vector3.up * fallSpeed * CupheadTime.Delta
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06001F4F RID: 8015 RVA: 0x0011F9AC File Offset: 0x0011DDAC
	Private Iterator Function fade_cr() As IEnumerator
		Dim t As Single = 0F
		Dim time As Single = 2F
		Dim currentColor As Color = MyBase.GetComponent(Of SpriteRenderer)().color
		Yield CupheadTime.WaitForSeconds(Me, 1.5F)
		While t < time
			MyBase.GetComponent(Of SpriteRenderer)().color = New Color(1F, 1F, 1F, 1F - t / time)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		MyBase.GetComponent(Of SpriteRenderer)().color = New Color(1F, 1F, 1F, 0F)
		Me.OnEffectComplete()
		Yield Nothing
		Return
	End Function
End Class
