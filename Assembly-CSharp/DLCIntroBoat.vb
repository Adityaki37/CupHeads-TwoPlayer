Imports System
Imports UnityEngine

' Token: 0x02000400 RID: 1024
Public Class DLCIntroBoat
	Inherits AbstractPausableComponent

	' Token: 0x06000E38 RID: 3640 RVA: 0x00091AEC File Offset: 0x0008FEEC
	Private Sub FixedUpdate()
		Me.curSpeed = Mathf.Lerp(Me.speed.GetFloatAt(1F - Me.boatmanAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime Mod 1F), Me.speed.GetFloatAt((1.1F - Me.boatmanAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime Mod 1F) Mod 1F), 0.5F)
		MyBase.transform.position += Vector3.right * Me.curSpeed * CupheadTime.FixedDelta
	End Sub

	' Token: 0x0400177A RID: 6010
	<SerializeField()>
	Private boatmanAnimator As Animator

	' Token: 0x0400177B RID: 6011
	<SerializeField()>
	Private speed As MinMax

	' Token: 0x0400177C RID: 6012
	Private curSpeed As Single
End Class
