Imports System
Imports System.Collections

' Token: 0x020004AD RID: 1197
Public Class TestLevelPlatform
	Inherits LevelPlatform

	' Token: 0x06001388 RID: 5000 RVA: 0x000ABFB0 File Offset: 0x000AA3B0
	Private Sub Start()
		MyBase.StartCoroutine(Me.loop_cr())
	End Sub

	' Token: 0x06001389 RID: 5001 RVA: 0x000ABFC0 File Offset: 0x000AA3C0
	Private Iterator Function loop_cr() As IEnumerator
		While True
			Yield MyBase.TweenLocalPositionX(-700F, 700F, 4F, EaseUtils.EaseType.easeInOutSine)
			Yield MyBase.TweenLocalPositionX(700F, -700F, 4F, EaseUtils.EaseType.easeInOutSine)
		End While
		Return
	End Function

	' Token: 0x04001C97 RID: 7319
	Private Const X As Single = 700F

	' Token: 0x04001C98 RID: 7320
	Private Const TIME As Single = 4F

	' Token: 0x04001C99 RID: 7321
	Private Const EASE As EaseUtils.EaseType = EaseUtils.EaseType.easeInOutSine
End Class
