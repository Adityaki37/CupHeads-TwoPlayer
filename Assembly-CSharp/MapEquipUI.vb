Imports System

' Token: 0x02000988 RID: 2440
Public Class MapEquipUI
	Inherits AbstractEquipUI

	' Token: 0x170004A4 RID: 1188
	' (get) Token: 0x0600390E RID: 14606 RVA: 0x00205F80 File Offset: 0x00204380
	Protected Overrides ReadOnly Property CanPause As Boolean
		Get
			Return Map.Current.CurrentState = Map.State.Ready AndAlso MapDifficultySelectStartUI.Current.CurrentState = AbstractMapSceneStartUI.State.Inactive AndAlso MapConfirmStartUI.Current.CurrentState = AbstractMapSceneStartUI.State.Inactive AndAlso MapBasicStartUI.Current.CurrentState = AbstractMapSceneStartUI.State.Inactive AndAlso (Not(Map.Current IsNot Nothing) OrElse Map.Current.CurrentState <> Map.State.Graveyard)
		End Get
	End Property

	' Token: 0x0600390F RID: 14607 RVA: 0x00205FF5 File Offset: 0x002043F5
	Protected Overrides Sub OnPause()
		MyBase.OnPause()
		CupheadMapCamera.Current.StartBlur()
	End Sub

	' Token: 0x06003910 RID: 14608 RVA: 0x00206007 File Offset: 0x00204407
	Protected Overrides Sub OnUnpause()
		MyBase.OnUnpause()
		CupheadMapCamera.Current.EndBlur()
	End Sub

	' Token: 0x06003911 RID: 14609 RVA: 0x0020601C File Offset: 0x0020441C
	Protected Overrides Sub OnPauseAudio()
		AudioManager.HandleSnapshot(AudioManager.Snapshots.EquipMenu.ToString(), 0.15F)
		AudioManager.PauseAllSFX()
	End Sub

	' Token: 0x06003912 RID: 14610 RVA: 0x00206048 File Offset: 0x00204448
	Protected Overrides Sub OnUnpauseAudio()
		AudioManager.SnapshotReset(SceneLoader.SceneName, 0.1F)
	End Sub
End Class
