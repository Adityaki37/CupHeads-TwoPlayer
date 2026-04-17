Imports System

' Token: 0x020006D8 RID: 1752
Public Class MausoleumLevelRegularGhost
	Inherits MausoleumLevelGhostBase

	' Token: 0x06002554 RID: 9556 RVA: 0x0015D596 File Offset: 0x0015B996
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.animator.SetBool("IsA", Rand.Bool())
	End Sub

	' Token: 0x06002555 RID: 9557 RVA: 0x0015D5B3 File Offset: 0x0015B9B3
	Public Overrides Sub OnParry(player As AbstractPlayerController)
		AudioManager.Play("mausoleum_regular_ghost_1_death")
		Me.emitAudioFromObject.Add("mausoleum_regular_ghost_1_death")
		MyBase.OnParry(player)
	End Sub
End Class
