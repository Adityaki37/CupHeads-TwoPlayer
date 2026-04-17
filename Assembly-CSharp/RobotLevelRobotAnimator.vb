Imports System

' Token: 0x02000773 RID: 1907
Public Class RobotLevelRobotAnimator
	Inherits AbstractPausableComponent

	' Token: 0x0600299F RID: 10655 RVA: 0x00184807 File Offset: 0x00182C07
	Private Sub ContinueMainAnimation()
		MyBase.animator.SetTrigger("StartMainAnim")
	End Sub

	' Token: 0x060029A0 RID: 10656 RVA: 0x00184819 File Offset: 0x00182C19
	Private Sub SyncAnimationLayers()
		MyBase.animator.SetTrigger("SyncLayers")
	End Sub

	' Token: 0x060029A1 RID: 10657 RVA: 0x0018482B File Offset: 0x00182C2B
	Private Sub MainAnimationStateOff()
		MyBase.animator.SetBool("MainAnimationActive", False)
	End Sub

	' Token: 0x060029A2 RID: 10658 RVA: 0x0018483E File Offset: 0x00182C3E
	Private Sub MainAnimationStateOn()
		MyBase.animator.SetBool("MainAnimationActive", True)
	End Sub
End Class
