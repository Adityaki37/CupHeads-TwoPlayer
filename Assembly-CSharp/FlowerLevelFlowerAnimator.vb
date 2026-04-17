Imports System
Imports UnityEngine

' Token: 0x02000609 RID: 1545
Public Class FlowerLevelFlowerAnimator
	Inherits AbstractPausableComponent

	' Token: 0x06001F11 RID: 7953 RVA: 0x0011D9D4 File Offset: 0x0011BDD4
	Private Sub OnIdleEnd()
		If Me.loops >= Me.max Then
			Me.OnBlink()
			Return
		End If
		Me.loops += 1
	End Sub

	' Token: 0x06001F12 RID: 7954 RVA: 0x0011D9FC File Offset: 0x0011BDFC
	Private Sub OnBlink()
		MyBase.animator.SetTrigger("OnBlink")
		Me.max = Global.UnityEngine.Random.Range(2, 5)
		Me.loops = 0
	End Sub

	' Token: 0x040027B1 RID: 10161
	Private Const MIN_IDLE_LOOPS As Integer = 2

	' Token: 0x040027B2 RID: 10162
	Private Const MAX_IDLE_LOOPS As Integer = 4

	' Token: 0x040027B3 RID: 10163
	Private loops As Integer

	' Token: 0x040027B4 RID: 10164
	Private max As Integer = 2
End Class
