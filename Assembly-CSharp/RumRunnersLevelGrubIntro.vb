Imports System
Imports UnityEngine

' Token: 0x0200078E RID: 1934
Public Class RumRunnersLevelGrubIntro
	Inherits AbstractPausableComponent

	' Token: 0x06002AD7 RID: 10967 RVA: 0x0018FF68 File Offset: 0x0018E368
	Private Sub animationEvent_StartExit()
		Dim component As Animator = MyBase.GetComponent(Of Animator)()
		component.SetLayerWeight(1, 0F)
	End Sub

	' Token: 0x06002AD8 RID: 10968 RVA: 0x0018FF88 File Offset: 0x0018E388
	Private Sub AnimationEvent_MoveToForeground()
		Me.rend.sortingLayerName = "Foreground"
		Me.rend.sortingOrder = 200
	End Sub

	' Token: 0x06002AD9 RID: 10969 RVA: 0x0018FFAA File Offset: 0x0018E3AA
	Private Sub AnimationEvent_SFX_RUMRUN_FakeAnnouncer_BeginAhhh()
		AudioManager.FadeSFXVolume("sfx_dlc_rumrun_vx_fakeannouncer_begin", 0F, 0.1F)
		AudioManager.Play("sfx_dlc_rumrun_vx_fakeannouncer_begin_ahhh")
	End Sub

	' Token: 0x06002ADA RID: 10970 RVA: 0x0018FFCA File Offset: 0x0018E3CA
	Private Sub AnimationEvent_SFX_RUMRUN_Intro_GrubFliesAway()
		AudioManager.Play("sfx_dlc_rumrun_intro_grubfliesaway")
	End Sub

	' Token: 0x04003399 RID: 13209
	<SerializeField()>
	Private rend As SpriteRenderer
End Class
