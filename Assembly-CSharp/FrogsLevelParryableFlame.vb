Imports System

' Token: 0x020006B5 RID: 1717
Public Class FrogsLevelParryableFlame
	Inherits ParrySwitch

	' Token: 0x06002471 RID: 9329 RVA: 0x001559DF File Offset: 0x00153DDF
	Public Overrides Sub OnParryPrePause(player As AbstractPlayerController)
		MyBase.OnParryPrePause(player)
		player.stats.ParryOneQuarter()
	End Sub
End Class
