Imports System

' Token: 0x02000965 RID: 2405
Public Class MapTutorialLoader
	Inherits AbstractMapInteractiveEntity

	' Token: 0x06003811 RID: 14353 RVA: 0x0020154C File Offset: 0x001FF94C
	Protected Overrides Sub Activate(player As MapPlayerController)
		MyBase.Activate(player)
		AudioManager.Play("world_map_level_difficulty_appear")
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, False, False)
		Map.Current.OnLoadShop()
		PlayerData.Data.CurrentMapData.playerOnePosition = MyBase.transform.position + Me.returnPositions.playerOne
		PlayerData.Data.CurrentMapData.playerTwoPosition = MyBase.transform.position + Me.returnPositions.playerTwo
		If Not PlayerManager.Multiplayer Then
			PlayerData.Data.CurrentMapData.playerOnePosition = MyBase.transform.position + Me.returnPositions.singlePlayer
		End If
		MapBasicStartUI.Current.level = "ElderKettleLevel"
		MapBasicStartUI.Current.[In](player)
		AddHandler MapBasicStartUI.Current.OnLoadLevelEvent, AddressOf Me.OnLoadLevel
		AddHandler MapBasicStartUI.Current.OnBackEvent, AddressOf Me.OnBack
	End Sub

	' Token: 0x06003812 RID: 14354 RVA: 0x00201668 File Offset: 0x001FFA68
	Private Sub OnLoadLevel()
		AudioNoiseHandler.Instance.BoingSound()
		SceneLoader.LoadScene(Scenes.scene_level_house_elder_kettle, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, Nothing)
	End Sub

	' Token: 0x06003813 RID: 14355 RVA: 0x0020167F File Offset: 0x001FFA7F
	Private Sub OnBack()
		Me.ReCheck()
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, True, True)
		RemoveHandler MapBasicStartUI.Current.OnLoadLevelEvent, AddressOf Me.OnLoadLevel
		RemoveHandler MapBasicStartUI.Current.OnBackEvent, AddressOf Me.OnBack
	End Sub
End Class
