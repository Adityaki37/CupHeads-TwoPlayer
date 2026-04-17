Imports System
Imports UnityEngine

' Token: 0x020008C4 RID: 2244
Public Class HarbourPlatformingLevelBuoy
	Inherits AbstractPausableComponent

	' Token: 0x0600346B RID: 13419 RVA: 0x001E7070 File Offset: 0x001E5470
	Private Sub Start()
		AddHandler Me.parrySwitch.OnActivate, AddressOf Me.ParrySoundSFX
		AddHandler Me.parrySwitch.OnActivate, AddressOf Me.parrySwitch.StartParryCooldown
	End Sub

	' Token: 0x0600346C RID: 13420 RVA: 0x001E70A8 File Offset: 0x001E54A8
	Private Sub PlayIdle()
		If CupheadLevelCamera.Current.ContainsPoint(MyBase.transform.position, New Vector2(100F, 1000F)) AndAlso Not AudioManager.CheckIfPlaying("harbour_buoy_idle") Then
			AudioManager.Play("harbour_buoy_idle")
			Me.emitAudioFromObject.Add("harbour_buoy_idle")
		End If
	End Sub

	' Token: 0x0600346D RID: 13421 RVA: 0x001E710C File Offset: 0x001E550C
	Private Sub ParrySoundSFX()
		AudioManager.Play("harbour_buoy_parry")
		Me.emitAudioFromObject.Add("harbour_buoy_parry")
	End Sub

	' Token: 0x04003C96 RID: 15510
	<SerializeField()>
	Private parrySwitch As ParrySwitch

	' Token: 0x04003C97 RID: 15511
	Private Const ON_SCREEN_SOUND_PADDING As Single = 100F
End Class
