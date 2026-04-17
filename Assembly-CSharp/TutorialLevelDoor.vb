Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000833 RID: 2099
Public Class TutorialLevelDoor
	Inherits AbstractLevelInteractiveEntity

	' Token: 0x060030AF RID: 12463 RVA: 0x001CA2A3 File Offset: 0x001C86A3
	Protected Overrides Sub Activate()
		If Me.activated Then
			Return
		End If
		MyBase.Activate()
		MyBase.StartCoroutine(Me.go_cr())
	End Sub

	' Token: 0x060030B0 RID: 12464 RVA: 0x001CA2C4 File Offset: 0x001C86C4
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		CupheadTime.SetLayerSpeed(CupheadTime.Layer.Player, 1F)
	End Sub

	' Token: 0x060030B1 RID: 12465 RVA: 0x001CA2D8 File Offset: 0x001C86D8
	Private Iterator Function go_cr() As IEnumerator
		Me.activated = True
		LevelCoin.OnLevelComplete()
		If Me.isChaliceTutorial Then
			PlayerData.Data.IsChaliceTutorialCompleted = True
		Else
			PlayerData.Data.IsTutorialCompleted = True
		End If
		PlayerData.SaveCurrentFile()
		For Each abstractPlayerController As AbstractPlayerController In PlayerManager.GetAllPlayers()
			Dim levelPlayerController As LevelPlayerController = CType(abstractPlayerController, LevelPlayerController)
			If Not(levelPlayerController Is Nothing) Then
				levelPlayerController.DisableInput()
				levelPlayerController.PauseAll()
			End If
		Next
		Dim level As TutorialLevel = TryCast(Level.Current, TutorialLevel)
		If level Then
			level.GoBackToHouse()
		Else
			Dim chaliceTutorialLevel As ChaliceTutorialLevel = TryCast(Level.Current, ChaliceTutorialLevel)
			If chaliceTutorialLevel Then
				chaliceTutorialLevel.[Exit]()
			End If
		End If
		Yield CupheadTime.WaitForSeconds(Me, 0.2F)
		If Me.isChaliceTutorial Then
			SceneLoader.LoadScene(Scenes.scene_map_world_DLC, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, Nothing)
		Else
			SceneLoader.LoadScene(Scenes.scene_level_house_elder_kettle, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, Nothing)
		End If
		Return
	End Function

	' Token: 0x04003952 RID: 14674
	Private activated As Boolean

	' Token: 0x04003953 RID: 14675
	<SerializeField()>
	Private isChaliceTutorial As Boolean
End Class
