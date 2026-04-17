Imports System

' Token: 0x02000936 RID: 2358
Public Class MapDicePalaceSceneLoader
	Inherits MapSceneLoader

	' Token: 0x06003722 RID: 14114 RVA: 0x001FC2EF File Offset: 0x001FA6EF
	Protected Overrides Sub LoadScene()
		If Not PlayerData.Data.GetLevelData(Levels.DicePalaceMain).played Then
			SceneLoader.LoadScene(Me.scene, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, Nothing)
		Else
			SceneLoader.LoadScene(Scenes.scene_level_dice_palace_main, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, Nothing)
		End If
	End Sub
End Class
