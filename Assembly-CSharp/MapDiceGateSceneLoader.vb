Imports System
Imports UnityEngine

' Token: 0x02000935 RID: 2357
Public Class MapDiceGateSceneLoader
	Inherits AbstractMapInteractiveEntity

	' Token: 0x0600371C RID: 14108 RVA: 0x001FBD88 File Offset: 0x001FA188
	Protected Overrides Sub Activate(player As MapPlayerController)
		MyBase.Activate(player)
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, False, False)
		AudioManager.Play("world_map_level_difficulty_appear")
		Map.Current.OnLoadLevel()
		MyBase.SetPlayerReturnPos()
		If Me.nextWorld = Scenes.scene_map_world_1 Then
			MapBasicStartUI.Current.level = "MapWorld_1"
		ElseIf Me.nextWorld = Scenes.scene_map_world_2 Then
			MapBasicStartUI.Current.level = If((Not PlayerData.Data.GetMapData(Scenes.scene_map_world_2).sessionStarted), "DieHouse", "MapWorld_2")
		ElseIf Me.nextWorld = Scenes.scene_map_world_3 Then
			MapBasicStartUI.Current.level = If((Not PlayerData.Data.GetMapData(Scenes.scene_map_world_2).sessionStarted), "DieHouse", "MapWorld_3")
		ElseIf Me.nextWorld = Scenes.scene_map_world_4 Then
			MapBasicStartUI.Current.level = "Inkwell"
		End If
		MapBasicStartUI.Current.[In](player)
		AddHandler MapBasicStartUI.Current.OnLoadLevelEvent, AddressOf Me.OnLoadLevel
		AddHandler MapBasicStartUI.Current.OnBackEvent, AddressOf Me.OnBack
	End Sub

	' Token: 0x0600371D RID: 14109 RVA: 0x001FBEB0 File Offset: 0x001FA2B0
	Private Sub OnLoadLevel()
		AudioManager.HandleSnapshot(AudioManager.Snapshots.Paused.ToString(), 0.5F)
		AudioNoiseHandler.Instance.BoingSound()
		Me.CheckSceneToLoad()
	End Sub

	' Token: 0x0600371E RID: 14110 RVA: 0x001FBEE8 File Offset: 0x001FA2E8
	Private Sub CheckSceneToLoad()
		If PlayerData.Data.CurrentMap = Scenes.scene_map_world_1 Then
			If PlayerData.Data.GetMapData(Scenes.scene_map_world_2).sessionStarted Then
				PlayerData.Data.GetMapData(Scenes.scene_map_world_2).enteringFrom = PlayerData.MapData.EntryMethod.DiceHouseLeft
				SceneLoader.LoadScene(Me.nextWorld, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, Nothing)
			Else
				SceneLoader.LoadScene(Me.diceGate, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, Nothing)
			End If
		ElseIf PlayerData.Data.CurrentMap = Scenes.scene_map_world_2 Then
			If PlayerData.Data.GetMapData(Scenes.scene_map_world_3).sessionStarted Then
				PlayerData.Data.GetMapData(Scenes.scene_map_world_3).enteringFrom = PlayerData.MapData.EntryMethod.DiceHouseLeft
				SceneLoader.LoadScene(Me.nextWorld, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, Nothing)
			Else
				SceneLoader.LoadScene(Me.diceGate, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, Nothing)
			End If
		End If
	End Sub

	' Token: 0x0600371F RID: 14111 RVA: 0x001FBFAC File Offset: 0x001FA3AC
	Private Sub OnBack()
		Me.ReCheck()
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, True, True)
		RemoveHandler MapBasicStartUI.Current.OnLoadLevelEvent, AddressOf Me.OnLoadLevel
		RemoveHandler MapBasicStartUI.Current.OnBackEvent, AddressOf Me.OnBack
	End Sub

	' Token: 0x06003720 RID: 14112 RVA: 0x001FBFE8 File Offset: 0x001FA3E8
	Protected Overrides Sub Reset()
		MyBase.Reset()
		Me.dialogueProperties = New AbstractUIInteractionDialogue.Properties("ENTER <sprite=0>")
	End Sub

	' Token: 0x04003F4E RID: 16206
	<SerializeField()>
	Private nextWorld As Scenes

	' Token: 0x04003F4F RID: 16207
	Private diceGate As Scenes = Scenes.scene_level_dice_gate

	' Token: 0x04003F50 RID: 16208
	<SerializeField()>
	Private askDifficulty As Boolean
End Class
