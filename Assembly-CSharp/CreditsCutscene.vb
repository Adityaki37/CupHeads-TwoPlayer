Imports System
Imports System.Collections

' Token: 0x020003F6 RID: 1014
Public Class CreditsCutscene
	Inherits Cutscene

	' Token: 0x06000DCD RID: 3533 RVA: 0x0008F6EA File Offset: 0x0008DAEA
	Protected Overrides Sub Start()
		MyBase.Start()
		CutsceneGUI.Current.pause.pauseAllowed = False
		MyBase.StartCoroutine(Me.music_cr())
	End Sub

	' Token: 0x06000DCE RID: 3534 RVA: 0x0008F710 File Offset: 0x0008DB10
	Private Iterator Function music_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		If CreditsScreen.goodEnding Then
			AudioManager.PlayBGM()
			OnlineManager.Instance.[Interface].UnlockAchievement(PlayerId.Any, "GoodEnding")
		Else
			AudioManager.PlayBGMPlaylistManually(True)
			OnlineManager.Instance.[Interface].UnlockAchievement(PlayerId.Any, "BadEnding")
		End If
		Return
	End Function

	' Token: 0x06000DCF RID: 3535 RVA: 0x0008F72B File Offset: 0x0008DB2B
	Protected Overrides Sub SetRichPresence()
		OnlineManager.Instance.[Interface].SetRichPresence(PlayerId.Any, "Ending", True)
	End Sub
End Class
