Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200090A RID: 2314
Public Class PlatformingLevelReverseGravitySwitch
	Inherits ParrySwitch

	' Token: 0x0600364D RID: 13901 RVA: 0x001F7B8C File Offset: 0x001F5F8C
	Public Overrides Sub OnParryPostPause(player As AbstractPlayerController)
		MyBase.OnParryPostPause(player)
		Dim levelPlayerController As LevelPlayerController = TryCast(player, LevelPlayerController)
		levelPlayerController.motor.SetGravityReversed(Not levelPlayerController.motor.GravityReversed)
		MyBase.StartCoroutine(Me.start_spin_cr(levelPlayerController))
		MyBase.StartParryCooldown()
	End Sub

	' Token: 0x0600364E RID: 13902 RVA: 0x001F7BD4 File Offset: 0x001F5FD4
	Private Iterator Function start_spin_cr(player As LevelPlayerController) As IEnumerator
		MyBase.animator.SetBool("IsSpin", True)
		MyBase.animator.SetBool("IsUp", player.motor.GravityReversed)
		Dim t As Single = 0F
		While t < Me.spinTimer
			t += CupheadTime.Delta
			Yield Nothing
		End While
		MyBase.animator.SetBool("IsSpin", False)
		Yield Nothing
		Return
	End Function

	' Token: 0x04003E41 RID: 15937
	<SerializeField()>
	Private spinTimer As Single
End Class
