Imports System

' Token: 0x020007AD RID: 1965
Public Class SallyStagePlayLevelFirewheel
	Inherits AbstractPausableComponent

	' Token: 0x06002C2C RID: 11308 RVA: 0x0019FA00 File Offset: 0x0019DE00
	Public Sub PlaySound()
		AudioManager.Play("sally_cherub_fireprop_move")
		Me.emitAudioFromObject.Add("sally_cherub_fireprop_move")
	End Sub
End Class
