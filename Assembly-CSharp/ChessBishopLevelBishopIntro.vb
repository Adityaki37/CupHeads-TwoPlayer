Imports System

' Token: 0x02000538 RID: 1336
Public Class ChessBishopLevelBishopIntro
	Inherits AbstractCollidableObject

	' Token: 0x06001841 RID: 6209 RVA: 0x000DBCA8 File Offset: 0x000DA0A8
	Private Sub AniEvent_BishopIntroSFX()
	End Sub

	' Token: 0x06001842 RID: 6210 RVA: 0x000DBCAA File Offset: 0x000DA0AA
	Private Sub AnimationEvent_SFX_KOG_Bishop_Intro_Vocal()
		AudioManager.Play("sfx_dlc_kog_bishop_intro_vocal")
		Me.emitAudioFromObject.Add("sfx_dlc_kog_bishop_intro_vocal")
	End Sub

	' Token: 0x06001843 RID: 6211 RVA: 0x000DBCC6 File Offset: 0x000DA0C6
	Private Sub AnimationEvent_SFX_KOG_Bishop_Intro_Sfx()
		AudioManager.Play("sfx_dlc_kog_bishop_intro_sfx")
		Me.emitAudioFromObject.Add("sfx_dlc_kog_bishop_intro_sfx")
	End Sub
End Class
