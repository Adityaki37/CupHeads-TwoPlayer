Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020004A3 RID: 1187
Public Class LevelEnd
	Inherits AbstractMonoBehaviour

	' Token: 0x06001355 RID: 4949 RVA: 0x000AAC94 File Offset: 0x000A9094
	Protected Overrides Sub Awake()
		MyBase.Awake()
		MyBase.transform.SetAsFirstSibling()
		MyBase.gameObject.name = "LEVEL_END_CONTROLER"
	End Sub

	' Token: 0x06001356 RID: 4950 RVA: 0x000AACB8 File Offset: 0x000A90B8
	Private Shared Function Create() As LevelEnd
		Dim gameObject As GameObject = New GameObject()
		Return gameObject.AddComponent(Of LevelEnd)()
	End Function

	' Token: 0x06001357 RID: 4951 RVA: 0x000AACD4 File Offset: 0x000A90D4
	Public Shared Sub Win(knockoutSFXCoroutine As IEnumerator, onBossDeathCallback As Action, explosionsCallback As Action, explosionsFalloffCallback As Action, explosionsEndCallback As Action, players As AbstractPlayerController(), bossDeathTime As Single, goToWinScreen As Boolean, isMausoleum As Boolean, isDevil As Boolean, isTowerOfPower As Boolean)
		Dim levelEnd As LevelEnd = LevelEnd.Create()
		levelEnd.StartCoroutine(levelEnd.win_cr(knockoutSFXCoroutine, onBossDeathCallback, explosionsCallback, explosionsFalloffCallback, explosionsEndCallback, players, bossDeathTime, goToWinScreen, isMausoleum, isDevil, isTowerOfPower))
	End Sub

	' Token: 0x06001358 RID: 4952 RVA: 0x000AAD08 File Offset: 0x000A9108
	Private Iterator Function win_cr(knockoutSFXCoroutine As IEnumerator, onBossDeathCallback As Action, explosionsCallback As Action, explosionsFalloffCallback As Action, explosionsEndCallback As Action, players As AbstractPlayerController(), bossDeathTime As Single, goToWinScreen As Boolean, isMausoleum As Boolean, isDevil As Boolean, isTowerOfPower As Boolean) As IEnumerator
		PauseManager.Pause()
		Dim koAnim As LevelKOAnimation = LevelKOAnimation.Create(isMausoleum)
		If Level.IsChessBoss Then
			AudioManager.StartBGMAlternate(0)
		End If
		If Level.Current.CurrentLevel = Levels.Saltbaker Then
			AudioManager.StartBGMAlternate(2)
		End If
		MyBase.StartCoroutine(knockoutSFXCoroutine)
		Yield koAnim.StartCoroutine(koAnim.anim_cr())
		PauseManager.Unpause()
		explosionsCallback()
		CupheadTime.SetAll(1F)
		If Not isMausoleum Then
			For Each abstractPlayerController As AbstractPlayerController In PlayerManager.GetAllPlayers()
				If Not(abstractPlayerController Is Nothing) Then
					abstractPlayerController.OnLevelWin()
				End If
			Next
		End If
		If onBossDeathCallback IsNot Nothing Then
			onBossDeathCallback()
		End If
		Yield New WaitForSeconds(bossDeathTime + 0.3F)
		For Each abstractProjectile As AbstractProjectile In Global.UnityEngine.[Object].FindObjectsOfType(Of AbstractProjectile)()
			abstractProjectile.OnLevelEnd()
		Next
		If Level.IsTowerOfPower Then
			TowerOfPowerLevelGameInfo.SetPlayersStats(PlayerId.PlayerOne)
			If PlayerManager.Multiplayer Then
				TowerOfPowerLevelGameInfo.SetPlayersStats(PlayerId.PlayerTwo)
			End If
		ElseIf Level.IsDicePalace AndAlso Not Level.IsDicePalaceMain Then
			DicePalaceMainLevelGameInfo.SetPlayersStats()
		End If
		SceneLoader.properties.transitionStart = SceneLoader.Transition.Fade
		SceneLoader.properties.transitionStartTime = 3F
		If Level.IsChessBoss OrElse Level.Current.CurrentLevel = Levels.Saltbaker Then
			Yield New WaitForSeconds(2F)
		End If
		If goToWinScreen Then
			SceneLoader.LoadScene(Scenes.scene_win, SceneLoader.Transition.Fade, SceneLoader.Transition.Fade, SceneLoader.Icon.None, Nothing)
		ElseIf Level.IsTowerOfPower Then
			SceneLoader.ContinueTowerOfPower()
		ElseIf Level.IsGraveyard Then
			SceneLoader.LoadScene(Scenes.scene_map_world_DLC, SceneLoader.Transition.Fade, SceneLoader.Transition.Iris, SceneLoader.Icon.None, Nothing)
		ElseIf Level.IsChessBoss Then
			If TypeOf SceneLoader.CurrentContext Is GauntletContext Then
				Dim num As Integer = Array.IndexOf(Of Levels)(Level.kingOfGamesLevels, Level.Current.CurrentLevel)
				Dim num2 As Integer = MathUtilities.NextIndex(num, Level.kingOfGamesLevels.Length)
				If num2 = 0 Then
					SceneLoader.LoadScene(Scenes.scene_level_chess_castle, SceneLoader.Transition.Fade, SceneLoader.Transition.Fade, SceneLoader.Icon.None, New GauntletContext(True))
				Else
					Dim levels As Levels = Level.kingOfGamesLevels(num2)
					Dim transition As SceneLoader.Transition = SceneLoader.Transition.Fade
					Dim gauntletContext As GauntletContext = New GauntletContext(False)
					SceneLoader.LoadLevel(levels, transition, SceneLoader.Icon.Hourglass, gauntletContext)
				End If
			Else
				SceneLoader.LoadScene(Scenes.scene_level_chess_castle, SceneLoader.Transition.Fade, SceneLoader.Transition.Fade, SceneLoader.Icon.None, Nothing)
			End If
		ElseIf Not isMausoleum Then
			SceneLoader.ReloadLevel()
		End If
		Yield New WaitForSeconds(2.5F)
		explosionsEndCallback()
		Return
	End Function

	' Token: 0x06001359 RID: 4953 RVA: 0x000AAD58 File Offset: 0x000A9158
	Public Shared Sub Lose(isMausoleum As Boolean, secretTriggered As Boolean)
		Dim levelEnd As LevelEnd = LevelEnd.Create()
		levelEnd.StartCoroutine(levelEnd.lose_cr(isMausoleum, secretTriggered))
	End Sub

	' Token: 0x0600135A RID: 4954 RVA: 0x000AAD7C File Offset: 0x000A917C
	Private Iterator Function lose_cr(isMausoleum As Boolean, secretTriggered As Boolean) As IEnumerator
		If isMausoleum Then
			AudioManager.Play("level_announcer_fail")
		End If
		PauseManager.Unpause()
		For Each abstractPausableComponent As AbstractPausableComponent In Global.UnityEngine.[Object].FindObjectsOfType(Of AbstractPausableComponent)()
			abstractPausableComponent.OnLevelEnd()
		Next
		LevelGameOverGUI.Current.[In](secretTriggered)
		If Level.IsChessBoss Then
			Yield New WaitForSeconds(1F)
			AudioManager.StartBGMAlternate(1)
		End If
		Yield Nothing
		Return
	End Function

	' Token: 0x0600135B RID: 4955 RVA: 0x000AADA0 File Offset: 0x000A91A0
	Public Shared Sub PlayerJoined()
		Dim levelEnd As LevelEnd = LevelEnd.Create()
		levelEnd.StartCoroutine(levelEnd.playerJoined_cr())
	End Sub

	' Token: 0x0600135C RID: 4956 RVA: 0x000AADC0 File Offset: 0x000A91C0
	Private Iterator Function playerJoined_cr() As IEnumerator
		PauseManager.Unpause()
		For Each abstractPausableComponent As AbstractPausableComponent In Global.UnityEngine.[Object].FindObjectsOfType(Of AbstractPausableComponent)()
			abstractPausableComponent.OnLevelEnd()
		Next
		Yield New WaitForSeconds(1F)
		Yield New WaitForSeconds(1F)
		SceneLoader.LoadLastMap()
		Return
	End Function

	' Token: 0x04001C76 RID: 7286
	Private Const NAME As String = "LEVEL_END_CONTROLER"

	' Token: 0x04001C77 RID: 7287
	Private Const WIN_FADE_TIME As Single = 3F

	' Token: 0x04001C78 RID: 7288
	Private Const JOIN_WAIT As Single = 1F
End Class
