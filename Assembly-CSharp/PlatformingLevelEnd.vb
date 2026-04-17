Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000902 RID: 2306
Public Class PlatformingLevelEnd
	Inherits AbstractMonoBehaviour

	' Token: 0x06003615 RID: 13845 RVA: 0x001F6B06 File Offset: 0x001F4F06
	Protected Overrides Sub Awake()
		MyBase.Awake()
		MyBase.transform.SetAsFirstSibling()
		MyBase.gameObject.name = "PLATFORMING_LEVEL_END_CONTROLER"
	End Sub

	' Token: 0x06003616 RID: 13846 RVA: 0x001F6B2C File Offset: 0x001F4F2C
	Private Shared Function Create() As PlatformingLevelEnd
		Dim gameObject As GameObject = New GameObject()
		Return gameObject.AddComponent(Of PlatformingLevelEnd)()
	End Function

	' Token: 0x06003617 RID: 13847 RVA: 0x001F6B48 File Offset: 0x001F4F48
	Public Shared Sub Win()
		Dim platformingLevelEnd As PlatformingLevelEnd = PlatformingLevelEnd.Create()
		platformingLevelEnd.StartCoroutine(platformingLevelEnd.win_cr())
	End Sub

	' Token: 0x06003618 RID: 13848 RVA: 0x001F6B68 File Offset: 0x001F4F68
	Private Sub OnWinComplete()
		Me.winReadyToExit = True
	End Sub

	' Token: 0x06003619 RID: 13849 RVA: 0x001F6B74 File Offset: 0x001F4F74
	Private Iterator Function win_cr() As IEnumerator
		AddHandler PlatformingLevelExit.OnWinCompleteEvent, AddressOf Me.OnWinComplete
		PauseManager.Pause()
		Dim bravoAnimation As PlatformingLevelWinAnimation = PlatformingLevelWinAnimation.Create()
		AudioManager.Play("platforming_announcer_bravo")
		While bravoAnimation.CurrentState = PlatformingLevelWinAnimation.State.Paused
			Yield Nothing
		End While
		PauseManager.Unpause()
		CupheadTime.SetAll(1F)
		For Each abstractPlayerController As AbstractPlayerController In PlayerManager.GetAllPlayers()
			Dim levelPlayerController As LevelPlayerController = CType(abstractPlayerController, LevelPlayerController)
			If Not(levelPlayerController Is Nothing) Then
				levelPlayerController.OnLevelWin()
			End If
		Next
		For Each abstractProjectile As AbstractProjectile In Global.UnityEngine.[Object].FindObjectsOfType(Of AbstractProjectile)()
			abstractProjectile.OnLevelEnd()
		Next
		For Each abstractPlatformingLevelEnemy As AbstractPlatformingLevelEnemy In Global.UnityEngine.[Object].FindObjectsOfType(Of AbstractPlatformingLevelEnemy)()
			abstractPlatformingLevelEnemy.OnLevelEnd()
		Next
		Yield Nothing
		While Not Me.winReadyToExit
			Yield Nothing
		End While
		SceneLoader.properties.transitionStart = SceneLoader.Transition.Fade
		SceneLoader.properties.transitionStartTime = 3F
		SceneLoader.LoadScene(Scenes.scene_win, SceneLoader.Transition.Fade, SceneLoader.Transition.Fade, SceneLoader.Icon.None, Nothing)
		Return
	End Function

	' Token: 0x0600361A RID: 13850 RVA: 0x001F6B90 File Offset: 0x001F4F90
	Public Shared Sub Lose()
		Dim platformingLevelEnd As PlatformingLevelEnd = PlatformingLevelEnd.Create()
		platformingLevelEnd.StartCoroutine(platformingLevelEnd.lose_cr())
	End Sub

	' Token: 0x0600361B RID: 13851 RVA: 0x001F6BB0 File Offset: 0x001F4FB0
	Private Iterator Function lose_cr() As IEnumerator
		PauseManager.Unpause()
		For Each abstractPausableComponent As AbstractPausableComponent In Global.UnityEngine.[Object].FindObjectsOfType(Of AbstractPausableComponent)()
			abstractPausableComponent.OnLevelEnd()
		Next
		LevelGameOverGUI.Current.[In](False)
		Yield Nothing
		Return
	End Function

	' Token: 0x04003E1D RID: 15901
	Private Const NAME As String = "PLATFORMING_LEVEL_END_CONTROLER"

	' Token: 0x04003E1E RID: 15902
	Private Const WIN_FADE_TIME As Single = 3F

	' Token: 0x04003E1F RID: 15903
	Private winReadyToExit As Boolean
End Class
