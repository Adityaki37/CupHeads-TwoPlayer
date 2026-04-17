Imports System

' Token: 0x02000798 RID: 1944
Public Class RumRunnersLevelPh2StartAnimation
	Inherits AbstractPausableComponent

	' Token: 0x170003F8 RID: 1016
	' (get) Token: 0x06002B2A RID: 11050 RVA: 0x00192BBF File Offset: 0x00190FBF
	' (set) Token: 0x06002B2B RID: 11051 RVA: 0x00192BC7 File Offset: 0x00190FC7
	Public Property showBug As Boolean

	' Token: 0x170003F9 RID: 1017
	' (get) Token: 0x06002B2C RID: 11052 RVA: 0x00192BD0 File Offset: 0x00190FD0
	' (set) Token: 0x06002B2D RID: 11053 RVA: 0x00192BD8 File Offset: 0x00190FD8
	Public Property dropped As Boolean

	' Token: 0x06002B2E RID: 11054 RVA: 0x00192BE1 File Offset: 0x00190FE1
	Private Sub animationEvent_StartWeb()
		MyBase.animator.Play("Loop", 1)
	End Sub

	' Token: 0x06002B2F RID: 11055 RVA: 0x00192BF4 File Offset: 0x00190FF4
	Private Sub animationEvent_EndWeb()
		MyBase.animator.Play("Off", 1)
	End Sub

	' Token: 0x06002B30 RID: 11056 RVA: 0x00192C07 File Offset: 0x00191007
	Private Sub animationEvent_ShowBug()
		Me.showBug = True
	End Sub

	' Token: 0x06002B31 RID: 11057 RVA: 0x00192C10 File Offset: 0x00191010
	Private Sub animationEvent_RopeDrop()
		Me.dropped = True
	End Sub

	' Token: 0x06002B32 RID: 11058 RVA: 0x00192C19 File Offset: 0x00191019
	Private Sub AnimationEvent_SFX_RUMRUN_ExitPhase1_SpiderReturns()
		AudioManager.Play("sfx_DLC_RUMRUN_ExitPhase1_SpiderReturns")
		Me.emitAudioFromObject.Add("sfx_DLC_RUMRUN_ExitPhase1_SpiderReturns")
	End Sub

	' Token: 0x06002B33 RID: 11059 RVA: 0x00192C35 File Offset: 0x00191035
	Private Sub AnimationEvent_SFX_RUMRUN_ExitPhase1_GrammoDrop()
		AudioManager.Play("sfx_dlc_rumrun_exitphase1_grammodrop")
		Me.emitAudioFromObject.Add("sfx_dlc_rumrun_exitphase1_grammodrop")
	End Sub
End Class
