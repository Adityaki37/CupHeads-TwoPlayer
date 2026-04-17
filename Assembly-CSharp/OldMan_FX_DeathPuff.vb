Imports System

' Token: 0x020006FB RID: 1787
Public Class OldMan_FX_DeathPuff
	Inherits AbstractPausableComponent

	' Token: 0x06002647 RID: 9799 RVA: 0x00165A9C File Offset: 0x00163E9C
	Private Sub AnimationEvent_SFX_OMM_GnomeDeathPuff()
		AudioManager.Play("sfx_dlc_omm_gnome_popper_deathpoof")
		Me.emitAudioFromObject.Add("sfx_dlc_omm_gnome_popper_deathpoof")
	End Sub
End Class
