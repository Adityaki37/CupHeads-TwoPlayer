Imports System

' Token: 0x02000987 RID: 2439
Public Class LevelEquipUI
	Inherits AbstractEquipUI

	' Token: 0x06003906 RID: 14598 RVA: 0x00205F39 File Offset: 0x00204339
	Private Sub Start()
		MyBase.gameObject.SetActive(False)
	End Sub

	' Token: 0x06003907 RID: 14599 RVA: 0x00205F47 File Offset: 0x00204347
	Public Sub Activate()
		MyBase.StartCoroutine(MyBase.pause_cr())
	End Sub

	' Token: 0x06003908 RID: 14600 RVA: 0x00205F56 File Offset: 0x00204356
	Protected Overrides Sub Unpause()
		LevelGameOverGUI.Current.ReactivateOnChangeEquipmentClosed()
		MyBase.StartCoroutine(MyBase.unpause_cr())
	End Sub

	' Token: 0x06003909 RID: 14601 RVA: 0x00205F6F File Offset: 0x0020436F
	Protected Overrides Sub OnPauseSound()
	End Sub

	' Token: 0x0600390A RID: 14602 RVA: 0x00205F71 File Offset: 0x00204371
	Protected Overrides Sub OnUnpauseSound()
	End Sub

	' Token: 0x0600390B RID: 14603 RVA: 0x00205F73 File Offset: 0x00204373
	Protected Overrides Sub PauseGameplay()
	End Sub

	' Token: 0x0600390C RID: 14604 RVA: 0x00205F75 File Offset: 0x00204375
	Protected Overrides Sub UnpauseGameplay()
	End Sub
End Class
