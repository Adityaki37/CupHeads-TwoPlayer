Imports System
Imports UnityEngine

' Token: 0x0200095B RID: 2395
Public Class MapNPCRandomTrigger
	Inherits MonoBehaviour

	' Token: 0x060037EE RID: 14318 RVA: 0x00200CBA File Offset: 0x001FF0BA
	Private Sub Start()
		Me.loopToWait = Global.UnityEngine.Random.Range(Me.triggerMinFrequency, Me.triggerMaxFrequency + 1)
	End Sub

	' Token: 0x060037EF RID: 14319 RVA: 0x00200CD5 File Offset: 0x001FF0D5
	Private Sub Looped()
		Me.loopToWait -= 1
		If Me.loopToWait <= 0 Then
			Me.loopToWait = Global.UnityEngine.Random.Range(Me.triggerMinFrequency, Me.triggerMaxFrequency + 1)
			Me.Trigger()
		End If
	End Sub

	' Token: 0x060037F0 RID: 14320 RVA: 0x00200D10 File Offset: 0x001FF110
	Private Sub Trigger()
		Me.animator.SetTrigger(Me.trigger)
	End Sub

	' Token: 0x04003FDB RID: 16347
	<SerializeField()>
	Private animator As Animator

	' Token: 0x04003FDC RID: 16348
	<SerializeField()>
	Private triggerMinFrequency As Integer = 3

	' Token: 0x04003FDD RID: 16349
	<SerializeField()>
	Private triggerMaxFrequency As Integer = 5

	' Token: 0x04003FDE RID: 16350
	Public trigger As String = "blink"

	' Token: 0x04003FDF RID: 16351
	Private loopToWait As Integer
End Class
