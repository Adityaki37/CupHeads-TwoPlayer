Imports System

' Token: 0x020004A9 RID: 1193
Public Class LevelResources
	Inherits AbstractMonoBehaviour

	' Token: 0x0600137A RID: 4986 RVA: 0x000AB99C File Offset: 0x000A9D9C
	Private Sub OnDestroy()
		Me.levelHUD = Nothing
		Me.levelGUI = Nothing
		Me.levelAudio = Nothing
		Me.levelBossDeathExplosion = Nothing
		Me.levelPlayer = Nothing
		Me.planePlayer = Nothing
		Me.joinEffect = Nothing
		Me.platformingIntro = Nothing
		Me.platformingWin = Nothing
		Me.levelIntro = Nothing
		Me.levelUIInteractionDialogue = Nothing
	End Sub

	' Token: 0x04001C87 RID: 7303
	Public Const EDITOR_PATH As String = "Assets/_CUPHEAD/Prefabs/LevelResources/Level_Resources.prefab"

	' Token: 0x04001C88 RID: 7304
	Public levelHUD As LevelHUD

	' Token: 0x04001C89 RID: 7305
	Public levelGUI As LevelGUI

	' Token: 0x04001C8A RID: 7306
	Public levelAudio As LevelAudio

	' Token: 0x04001C8B RID: 7307
	Public levelBossDeathExplosion As Effect

	' Token: 0x04001C8C RID: 7308
	Public levelPlayer As LevelPlayerController

	' Token: 0x04001C8D RID: 7309
	Public planePlayer As PlanePlayerController

	' Token: 0x04001C8E RID: 7310
	Public joinEffect As PlayerJoinEffect

	' Token: 0x04001C8F RID: 7311
	Public platformingIntro As PlatformingLevelIntroAnimation

	' Token: 0x04001C90 RID: 7312
	Public platformingWin As PlatformingLevelWinAnimation

	' Token: 0x04001C91 RID: 7313
	Public levelIntro As LevelIntroAnimation

	' Token: 0x04001C92 RID: 7314
	Public levelKO As LevelKOAnimation

	' Token: 0x04001C93 RID: 7315
	Public levelUIInteractionDialogue As LevelUIInteractionDialogue
End Class
