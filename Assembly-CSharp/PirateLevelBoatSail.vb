Imports System
Imports UnityEngine

' Token: 0x02000721 RID: 1825
Public Class PirateLevelBoatSail
	Inherits AbstractMonoBehaviour

	' Token: 0x060027BB RID: 10171 RVA: 0x0017445C File Offset: 0x0017285C
	Private Sub RegularEnded()
		If Me.reg >= Me.regTarget Then
			Me.StartFast()
			Return
		End If
		Me.reg += 1
	End Sub

	' Token: 0x060027BC RID: 10172 RVA: 0x00174484 File Offset: 0x00172884
	Private Sub FastEnded()
		If Me.fast >= Me.fastTarget Then
			Me.StartReg()
			Return
		End If
		Me.fast += 1
	End Sub

	' Token: 0x060027BD RID: 10173 RVA: 0x001744AC File Offset: 0x001728AC
	Private Sub StartReg()
		Me.regTarget = Global.UnityEngine.Random.Range(Me.regularLoopsMin, Me.regularLoopsMax + 1)
		Me.reg = 0
		MyBase.animator.SetBool("Fast", False)
	End Sub

	' Token: 0x060027BE RID: 10174 RVA: 0x001744DF File Offset: 0x001728DF
	Private Sub StartFast()
		Me.fastTarget = Global.UnityEngine.Random.Range(Me.fastLoopsMin, Me.fastLoopsMax + 1)
		Me.fast = 0
		MyBase.animator.SetBool("Fast", True)
	End Sub

	' Token: 0x04003075 RID: 12405
	<Space(10F)>
	<Range(1F, 20F)>
	<SerializeField()>
	Private regularLoopsMin As Integer = 3

	' Token: 0x04003076 RID: 12406
	<Range(1F, 20F)>
	<SerializeField()>
	Private regularLoopsMax As Integer = 5

	' Token: 0x04003077 RID: 12407
	<Space(10F)>
	<Range(1F, 20F)>
	<SerializeField()>
	Private fastLoopsMin As Integer = 5

	' Token: 0x04003078 RID: 12408
	<Range(1F, 20F)>
	<SerializeField()>
	Private fastLoopsMax As Integer = 9

	' Token: 0x04003079 RID: 12409
	Private reg As Integer

	' Token: 0x0400307A RID: 12410
	Private fast As Integer

	' Token: 0x0400307B RID: 12411
	Private regTarget As Integer = 4

	' Token: 0x0400307C RID: 12412
	Private fastTarget As Integer = 7
End Class
