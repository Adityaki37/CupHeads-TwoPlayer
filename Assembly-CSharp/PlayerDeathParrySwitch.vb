Imports System

' Token: 0x02000A0A RID: 2570
Public Class PlayerDeathParrySwitch
	Inherits ParrySwitch

	' Token: 0x06003CC2 RID: 15554 RVA: 0x0021A0E6 File Offset: 0x002184E6
	Public Overrides Sub OnParryPrePause(player As AbstractPlayerController)
		MyBase.OnParryPrePause(player)
		player.stats.OnParry(1F, True)
	End Sub
End Class
