Imports System

' Token: 0x02000401 RID: 1025
Public Class DLCIntroBoatman
	Inherits AbstractPausableComponent

	' Token: 0x06000E3A RID: 3642 RVA: 0x00091B9D File Offset: 0x0008FF9D
	Private Sub AniEvent_Paddle_SFX()
		AudioManager.Play("sfx_DLC_Intro_PaddleWater")
		Me.emitAudioFromObject.Add("sfx_DLC_Intro_PaddleWater")
	End Sub
End Class
