Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200068A RID: 1674
Public Class FlyingMermaidLevelFloater
	Inherits AbstractPausableComponent

	' Token: 0x0600234D RID: 9037 RVA: 0x0014B793 File Offset: 0x00149B93
	Private Sub Start()
		MyBase.StartCoroutine(Me.loop_cr())
	End Sub

	' Token: 0x0600234E RID: 9038 RVA: 0x0014B7A4 File Offset: 0x00149BA4
	Private Iterator Function loop_cr() As IEnumerator
		Dim waveWidth As Single = 0F
		If Me.trackingWater IsNot Nothing Then
			Dim component As SpriteRenderer = Me.trackingWater.GetComponent(Of SpriteRenderer)()
			waveWidth = component.bounds.size.x / 2F
		End If
		Dim lastY As Single = MyBase.transform.localPosition.y
		Dim relativeBobY As Single = 0F
		Dim originY As Single = MyBase.transform.localPosition.y
		Dim t As Single = 0F
		Dim frameTime As Single = 0F
		While True
			If Not MyBase.enabled Then
				Yield Nothing
			Else
				t += CupheadTime.Delta
				frameTime += CupheadTime.Delta
				While frameTime > 0.041666668F
					frameTime -= 0.041666668F
					Dim quaternion As Quaternion = MyBase.transform.rotation
					Dim num As Single
					Dim num2 As Single
					If Me.trackingWater IsNot Nothing Then
						Dim x As Single = Me.trackingWater.transform.position.x
						Dim x2 As Single = MyBase.transform.position.x
						num = 1F - Mathf.Cos((x2 - x) * 2F * (3.1415927F / waveWidth))
						num2 = Mathf.Sin((x2 - x) * 2F * (3.1415927F / waveWidth))
					Else
						num = 1F - Mathf.Cos(t * Me.bobSpeed * 2F * 3.1415927F)
						num2 = Mathf.Sin(t * Me.bobSpeed * 2F * 3.1415927F)
					End If
					relativeBobY = num * Me.bobAmount
					originY += MyBase.transform.localPosition.y - lastY
					quaternion = Quaternion.AngleAxis(Me.defaultRotation + num2 * Me.rotateAmount, Vector3.forward)
					MyBase.transform.SetPosition(Nothing, New Single?(originY + relativeBobY), Nothing)
					MyBase.transform.rotation = quaternion
					lastY = MyBase.transform.localPosition.y
				End While
				Yield Nothing
			End If
		End While
		Return
	End Function

	' Token: 0x04002BEC RID: 11244
	Private Const BOB_FRAME_TIME As Single = 0.041666668F

	' Token: 0x04002BED RID: 11245
	Public bobAmount As Single

	' Token: 0x04002BEE RID: 11246
	Public rotateAmount As Single

	' Token: 0x04002BEF RID: 11247
	Public defaultRotation As Single

	' Token: 0x04002BF0 RID: 11248
	Public trackingWater As GameObject

	' Token: 0x04002BF1 RID: 11249
	Public bobSpeed As Single
End Class
