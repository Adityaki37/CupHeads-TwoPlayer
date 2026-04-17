Imports System
Imports UnityEngine

' Token: 0x020006FF RID: 1791
Public Class OldManLevelBubbleController
	Inherits MonoBehaviour

	' Token: 0x06002659 RID: 9817 RVA: 0x00166664 File Offset: 0x00164A64
	Private Sub Start()
		For i As Integer = 0 To Me.animators.Length - 1
			Me.animators(i).Play("Bubble", 0, 1F)
		Next
	End Sub

	' Token: 0x0600265A RID: 9818 RVA: 0x001666A4 File Offset: 0x00164AA4
	Private Sub FixedUpdate()
		Me.timer -= CupheadTime.FixedDelta
		If Me.timer <= 0F Then
			Dim num As Integer = Global.UnityEngine.Random.Range(0, Me.animators.Length)
			If Me.animators(num).GetCurrentAnimatorStateInfo(0).normalizedTime > Me.minTimeToRepeat Then
				Me.animators(num).Play("Bubble", 0, 0F)
			End If
			Me.timer = Global.UnityEngine.Random.Range(Me.minDelay, Me.maxDelay)
		End If
	End Sub

	' Token: 0x04002EE6 RID: 12006
	<SerializeField()>
	Private minDelay As Single = 0.05F

	' Token: 0x04002EE7 RID: 12007
	<SerializeField()>
	Private maxDelay As Single = 0.5F

	' Token: 0x04002EE8 RID: 12008
	<SerializeField()>
	Private minTimeToRepeat As Single = 2F

	' Token: 0x04002EE9 RID: 12009
	<SerializeField()>
	Private animators As Animator()

	' Token: 0x04002EEA RID: 12010
	Private timer As Single
End Class
