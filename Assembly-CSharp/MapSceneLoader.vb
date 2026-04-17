Imports System
Imports UnityEngine

' Token: 0x0200095D RID: 2397
Public Class MapSceneLoader
	Inherits AbstractMapInteractiveEntity

	' Token: 0x060037F8 RID: 14328 RVA: 0x001FC008 File Offset: 0x001FA408
	Protected Overrides Sub Activate(player As MapPlayerController)
		MyBase.Activate(player)
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, False, False)
		AudioManager.Play("world_map_level_difficulty_appear")
		MyBase.SetPlayerReturnPos()
		If Me.askDifficulty Then
			If Me.scene = Scenes.scene_cutscene_kingdice Then
				MapDifficultySelectStartUI.Current.level = "DicePalaceMain"
			End If
			MapDifficultySelectStartUI.Current.[In](player)
			AddHandler MapDifficultySelectStartUI.Current.OnLoadLevelEvent, AddressOf Me.OnLoadLevel
			AddHandler MapDifficultySelectStartUI.Current.OnBackEvent, AddressOf Me.OnBack
		Else
			If Me.scene = Scenes.scene_map_world_1 Then
				PlayerData.Data.GetMapData(Scenes.scene_map_world_1).enteringFrom = PlayerData.MapData.EntryMethod.DiceHouseRight
				MapBasicStartUI.Current.level = "MapWorld_1"
			ElseIf Me.scene = Scenes.scene_map_world_2 Then
				PlayerData.Data.GetMapData(Scenes.scene_map_world_2).enteringFrom = PlayerData.MapData.EntryMethod.DiceHouseRight
				MapBasicStartUI.Current.level = "MapWorld_2"
			ElseIf Me.scene = Scenes.scene_map_world_3 Then
				If PlayerData.Data.CurrentMap = Scenes.scene_map_world_4 Then
					MapBasicStartUI.Current.level = "KingDiceToWorld3WorldMap"
				Else
					MapBasicStartUI.Current.level = "MapWorld_3"
				End If
			ElseIf Me.scene = Scenes.scene_map_world_4 Then
				MapBasicStartUI.Current.level = "Inkwell"
			ElseIf Me.scene = Scenes.scene_cutscene_kingdice Then
				MapBasicStartUI.Current.level = "KingDice"
			End If
			MapBasicStartUI.Current.[In](player)
			AddHandler MapBasicStartUI.Current.OnLoadLevelEvent, AddressOf Me.OnLoadLevel
			AddHandler MapBasicStartUI.Current.OnBackEvent, AddressOf Me.OnBack
		End If
	End Sub

	' Token: 0x060037F9 RID: 14329 RVA: 0x001FC1B0 File Offset: 0x001FA5B0
	Private Sub OnLoadLevel()
		AudioManager.HandleSnapshot(AudioManager.Snapshots.Paused.ToString(), 0.5F)
		AudioNoiseHandler.Instance.BoingSound()
		Me.LoadScene()
	End Sub

	' Token: 0x060037FA RID: 14330 RVA: 0x001FC1E8 File Offset: 0x001FA5E8
	Protected Overridable Sub LoadScene()
		RemoveHandler MapDifficultySelectStartUI.Current.OnLoadLevelEvent, AddressOf Me.OnLoadLevel
		RemoveHandler MapDifficultySelectStartUI.Current.OnBackEvent, AddressOf Me.OnBack
		RemoveHandler MapBasicStartUI.Current.OnLoadLevelEvent, AddressOf Me.OnLoadLevel
		RemoveHandler MapBasicStartUI.Current.OnBackEvent, AddressOf Me.OnBack
		SceneLoader.LoadScene(Me.scene, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, Nothing)
	End Sub

	' Token: 0x060037FB RID: 14331 RVA: 0x001FC25C File Offset: 0x001FA65C
	Private Sub OnBack()
		Me.ReCheck()
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, True, True)
		RemoveHandler MapDifficultySelectStartUI.Current.OnLoadLevelEvent, AddressOf Me.OnLoadLevel
		RemoveHandler MapDifficultySelectStartUI.Current.OnBackEvent, AddressOf Me.OnBack
		RemoveHandler MapBasicStartUI.Current.OnLoadLevelEvent, AddressOf Me.OnLoadLevel
		RemoveHandler MapBasicStartUI.Current.OnBackEvent, AddressOf Me.OnBack
	End Sub

	' Token: 0x060037FC RID: 14332 RVA: 0x001FC2CF File Offset: 0x001FA6CF
	Protected Overrides Sub Reset()
		MyBase.Reset()
		Me.dialogueProperties = New AbstractUIInteractionDialogue.Properties("ENTER <sprite=0>")
	End Sub

	' Token: 0x04003FE3 RID: 16355
	<SerializeField()>
	Protected scene As Scenes

	' Token: 0x04003FE4 RID: 16356
	<SerializeField()>
	Private askDifficulty As Boolean
End Class
