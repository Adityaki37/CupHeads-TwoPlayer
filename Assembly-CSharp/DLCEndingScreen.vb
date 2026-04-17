Imports System

' Token: 0x020003FD RID: 1021
Public Class DLCEndingScreen
	Inherits DLCCutsceneScreen

	' Token: 0x06000E17 RID: 3607 RVA: 0x0009197C File Offset: 0x0008FD7C
	Private Sub AniEvent_SwapChars()
		CType(Me.cutscene, DLCEndingCutscene).SwapChars()
	End Sub

	' Token: 0x06000E18 RID: 3608 RVA: 0x0009198E File Offset: 0x0008FD8E
	Private Sub AniEvent_IrisOut()
		CType(Me.cutscene, DLCEndingCutscene).IrisOut()
	End Sub

	' Token: 0x06000E19 RID: 3609 RVA: 0x000919A0 File Offset: 0x0008FDA0
	Private Sub AniEvent_StartShake()
		CType(Me.cutscene, DLCEndingCutscene).StartShake()
	End Sub

	' Token: 0x06000E1A RID: 3610 RVA: 0x000919B2 File Offset: 0x0008FDB2
	Private Sub AniEvent_StopShake()
		CType(Me.cutscene, DLCEndingCutscene).StopShake()
	End Sub

	' Token: 0x06000E1B RID: 3611 RVA: 0x000919C4 File Offset: 0x0008FDC4
	Private Sub AniEvent_AdvanceMusic()
		CType(Me.cutscene, DLCEndingCutscene).AdvanceMusic()
	End Sub

	' Token: 0x06000E1C RID: 3612 RVA: 0x000919D6 File Offset: 0x0008FDD6
	Private Sub AniEvent_ActivateChaliceRArm()
		MyBase.animator.Play("ChaliceArmRLoop", 1, 0F)
		MyBase.animator.Update(0F)
	End Sub

	' Token: 0x06000E1D RID: 3613 RVA: 0x000919FE File Offset: 0x0008FDFE
	Private Sub AniEvent_LowerChaliceRArm()
		MyBase.animator.SetTrigger("Arm")
	End Sub

	' Token: 0x06000E1E RID: 3614 RVA: 0x00091A10 File Offset: 0x0008FE10
	Private Sub AniEvent_HideChaliceRArm()
		MyBase.animator.Play("None", 1, 0F)
		MyBase.animator.Update(0F)
	End Sub

	' Token: 0x06000E1F RID: 3615 RVA: 0x00091A38 File Offset: 0x0008FE38
	Private Sub AnimEvent_SFX_Ending_BakeryGoesDown()
		AudioManager.Play("sfx_DLC_Cutscene_Ending_BakeryGoesDown")
	End Sub

	' Token: 0x06000E20 RID: 3616 RVA: 0x00091A44 File Offset: 0x0008FE44
	Private Sub AnimEvent_SFX_Ending_ChaliceGlassBreak()
		AudioManager.Play("sfx_DLC_Cutscene_Ending_ChaliceGlassBreak")
	End Sub

	' Token: 0x06000E21 RID: 3617 RVA: 0x00091A50 File Offset: 0x0008FE50
	Private Sub AnimEvent_SFX_Ending_ChaliceGlassShake()
		AudioManager.Play("sfx_DLC_Cutscene_Ending_ChaliceGlassShake")
	End Sub

	' Token: 0x06000E22 RID: 3618 RVA: 0x00091A5C File Offset: 0x0008FE5C
	Private Sub AnimEvent_SFX_Ending_ChaliceWink()
		AudioManager.Play("sfx_DLC_Cutscene_Ending_ChaliceWink")
	End Sub

	' Token: 0x06000E23 RID: 3619 RVA: 0x00091A68 File Offset: 0x0008FE68
	Private Sub AnimEvent_SFX_Ending_ChaliceHug()
		AudioManager.Play("sfx_dlc_cutscene_ending_chalicehug")
	End Sub

	' Token: 0x06000E24 RID: 3620 RVA: 0x00091A74 File Offset: 0x0008FE74
	Private Sub AnimEvent_SFX_Ending_CollapsingBegins()
		AudioManager.Play("sfx_DLC_Cutscene_Ending_CollapsingBegins")
	End Sub

	' Token: 0x06000E25 RID: 3621 RVA: 0x00091A80 File Offset: 0x0008FE80
	Private Sub AnimEvent_SFX_Ending_EscapingBaker()
		AudioManager.Play("sfx_DLC_Cutscene_Ending_EscapingBaker")
	End Sub

	' Token: 0x06000E26 RID: 3622 RVA: 0x00091A8C File Offset: 0x0008FE8C
	Private Sub AnimEvent_SFX_Ending_EscapingGroup()
		AudioManager.Play("sfx_DLC_Cutscene_Ending_EscapingGroup")
	End Sub

	' Token: 0x06000E27 RID: 3623 RVA: 0x00091A98 File Offset: 0x0008FE98
	Private Sub AnimEvent_SFX_Ending_BakerSit()
		AudioManager.Play("sfx_DLC_Cutscene_Ending_BakerSit")
	End Sub

	' Token: 0x06000E28 RID: 3624 RVA: 0x00091AA4 File Offset: 0x0008FEA4
	Private Sub AnimEvent_SFX_Ending_RumbleLoopStart()
		AudioManager.PlayLoop("sfx_DLC_Cutscene_Ending_Rumble_Loop")
		AudioManager.FadeSFXVolume("sfx_DLC_Cutscene_Ending_Rumble_Loop", 0.2F, 3F)
	End Sub

	' Token: 0x06000E29 RID: 3625 RVA: 0x00091AC4 File Offset: 0x0008FEC4
	Private Sub AnimEvent_SFX_Ending_RumbleLoopStop()
		AudioManager.FadeSFXVolume("sfx_DLC_Cutscene_Ending_Rumble_Loop", 0F, 3F)
		AudioManager.[Stop]("sfx_DLC_Cutscene_Ending_Rumble_Loop")
	End Sub
End Class
