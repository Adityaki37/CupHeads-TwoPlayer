Imports System
Imports UnityEngine

' Token: 0x020004BD RID: 1213
Public Class AirplaneLevelLaser
	Inherits ParrySwitch

	' Token: 0x06001415 RID: 5141 RVA: 0x000B2E93 File Offset: 0x000B1293
	Public Overrides Sub OnParryPrePause(player As AbstractPlayerController)
		MyBase.OnParryPrePause(player)
		player.stats.ParryOneQuarter()
	End Sub

	' Token: 0x06001416 RID: 5142 RVA: 0x000B2EA7 File Offset: 0x000B12A7
	Public Overrides Sub OnParryPostPause(player As AbstractPlayerController)
		MyBase.OnParryPostPause(player)
		MyBase.StartParryCooldown()
		Me.anim.Play("End")
	End Sub

	' Token: 0x04001D40 RID: 7488
	<SerializeField()>
	Private anim As Animator
End Class
