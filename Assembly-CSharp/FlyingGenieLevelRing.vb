Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000678 RID: 1656
Public Class FlyingGenieLevelRing
	Inherits BasicProjectile

	' Token: 0x060022E3 RID: 8931 RVA: 0x00147830 File Offset: 0x00145C30
	Protected Overrides Sub Start()
		MyBase.Start()
		If Me.isMain Then
			MyBase.GetComponent(Of Collider2D)().enabled = False
			MyBase.animator.Play("Off")
		Else
			MyBase.StartCoroutine(Me.fade_cr())
		End If
	End Sub

	' Token: 0x060022E4 RID: 8932 RVA: 0x0014787C File Offset: 0x00145C7C
	Private Iterator Function fade_cr() As IEnumerator
		Dim frameTime As Single = 0F
		Dim color As Single = 1F
		While color > 0F
			MyBase.GetComponent(Of SpriteRenderer)().color = New Color(1F, 1F, 1F, color)
			frameTime += CupheadTime.Delta
			If frameTime > 0.041666668F Then
				color -= 0.05F
				frameTime -= 0.041666668F
			End If
			Yield Nothing
		End While
		Me.OnComplete()
		Yield Nothing
		Return
	End Function

	' Token: 0x060022E5 RID: 8933 RVA: 0x00147897 File Offset: 0x00145C97
	Private Sub OnComplete()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x060022E6 RID: 8934 RVA: 0x001478A4 File Offset: 0x00145CA4
	Public Sub DisableCollision()
		MyBase.GetComponent(Of Collider2D)().enabled = False
	End Sub

	' Token: 0x04002B82 RID: 11138
	Private Const FRAME_TIME As Single = 0.041666668F

	' Token: 0x04002B83 RID: 11139
	Public isMain As Boolean
End Class
