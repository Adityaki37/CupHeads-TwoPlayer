Imports System
Imports System.Collections

' Token: 0x0200059B RID: 1435
Public Class DiceGateLevelToNextWorld
	Inherits AbstractLevelInteractiveEntity

	' Token: 0x06001B7F RID: 7039 RVA: 0x000FB2AD File Offset: 0x000F96AD
	Protected Overrides Sub Activate()
		If Me.activated Then
			Return
		End If
		MyBase.Activate()
		MyBase.StartCoroutine(Me.go_cr())
	End Sub

	' Token: 0x06001B80 RID: 7040 RVA: 0x000FB2CE File Offset: 0x000F96CE
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		CupheadTime.SetLayerSpeed(CupheadTime.Layer.Player, 1F)
	End Sub

	' Token: 0x06001B81 RID: 7041 RVA: 0x000FB2E4 File Offset: 0x000F96E4
	Private Iterator Function go_cr() As IEnumerator
		Me.activated = True
		CupheadTime.SetLayerSpeed(CupheadTime.Layer.Player, 0F)
		For Each abstractPlayerController As AbstractPlayerController In PlayerManager.GetAllPlayers()
			Dim levelPlayerController As LevelPlayerController = CType(abstractPlayerController, LevelPlayerController)
			If Not(levelPlayerController Is Nothing) Then
				levelPlayerController.DisableInput()
				levelPlayerController.PauseAll()
			End If
		Next
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		If PlayerData.Data.CurrentMap = Scenes.scene_map_world_1 Then
			If PlayerData.Data.GetMapData(Scenes.scene_map_world_2).sessionStarted Then
				SceneLoader.LoadScene(Scenes.scene_map_world_2, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, Nothing)
			Else
				Cutscene.Load(Scenes.scene_map_world_2, Scenes.scene_cutscene_world2, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass)
			End If
		ElseIf PlayerData.Data.CurrentMap = Scenes.scene_map_world_2 Then
			If PlayerData.Data.GetMapData(Scenes.scene_map_world_3).sessionStarted Then
				SceneLoader.LoadScene(Scenes.scene_map_world_3, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, Nothing)
			Else
				Cutscene.Load(Scenes.scene_map_world_3, Scenes.scene_cutscene_world3, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass)
			End If
		End If
		Return
	End Function

	' Token: 0x06001B82 RID: 7042 RVA: 0x000FB2FF File Offset: 0x000F96FF
	Protected Overrides Sub Show(playerId As PlayerId)
		MyBase.state = AbstractLevelInteractiveEntity.State.Ready
		Me.dialogue = LevelUIInteractionDialogue.Create(Me.dialogueProperties, PlayerManager.GetPlayer(playerId).input, Me.dialogueOffset, 0F, LevelUIInteractionDialogue.TailPosition.Right, False)
	End Sub

	' Token: 0x040024A8 RID: 9384
	Private activated As Boolean
End Class
