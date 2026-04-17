Imports System

' Token: 0x0200092F RID: 2351
Public Class MapBakeryLoader
	Inherits AbstractMapInteractiveEntity

	' Token: 0x06003701 RID: 14081 RVA: 0x001FB2F8 File Offset: 0x001F96F8
	Private Sub Start()
		If PlayerData.Data.shouldShowChaliceTooltip Then
			MapEventNotification.Current.ShowTooltipEvent(TooltipEvent.Chalice)
			PlayerData.Data.shouldShowChaliceTooltip = False
		End If
	End Sub

	' Token: 0x06003702 RID: 14082 RVA: 0x001FB320 File Offset: 0x001F9720
	Protected Overrides Sub Activate(player As MapPlayerController)
		If AbstractMapInteractiveEntity.HasPopupOpened Then
			Return
		End If
		AbstractMapInteractiveEntity.HasPopupOpened = True
		MyBase.Activate(player)
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, False, False)
		AudioManager.Play("world_map_level_difficulty_appear")
		PlayerData.Data.CurrentMapData.playerOnePosition = MyBase.transform.position + Me.returnPositions.playerOne
		PlayerData.Data.CurrentMapData.playerTwoPosition = MyBase.transform.position + Me.returnPositions.playerTwo
		If Not PlayerManager.Multiplayer Then
			PlayerData.Data.CurrentMapData.playerOnePosition = MyBase.transform.position + Me.returnPositions.singlePlayer
		End If
		If PlayerData.Data.GetLevelData(Levels.Saltbaker).played AndAlso Not Me.HoldingLAndR() Then
			MapDifficultySelectStartUI.Current.level = "Saltbaker"
			MapDifficultySelectStartUI.Current.[In](player)
			AddHandler MapDifficultySelectStartUI.Current.OnLoadLevelEvent, AddressOf Me.OnLoadLevel
			AddHandler MapDifficultySelectStartUI.Current.OnBackEvent, AddressOf Me.OnBack
			Me.loadKitchen = False
		Else
			MapBasicStartUI.Current.level = "BakeryWorldMap"
			MapBasicStartUI.Current.[In](player)
			AddHandler MapBasicStartUI.Current.OnLoadLevelEvent, AddressOf Me.OnLoadLevel
			AddHandler MapBasicStartUI.Current.OnBackEvent, AddressOf Me.OnBack
			Me.loadKitchen = True
		End If
	End Sub

	' Token: 0x06003703 RID: 14083 RVA: 0x001FB4C0 File Offset: 0x001F98C0
	Private Function HoldingLAndR() As Boolean
		Return(Map.Current.players(0) AndAlso Map.Current.players(0).input.actions.GetButton(11) AndAlso Map.Current.players(0).input.actions.GetButton(12)) OrElse (Map.Current.players(1) AndAlso Map.Current.players(1).input.actions.GetButton(11) AndAlso Map.Current.players(1).input.actions.GetButton(12))
	End Function

	' Token: 0x06003704 RID: 14084 RVA: 0x001FB586 File Offset: 0x001F9986
	Private Sub OnLoadLevel()
		AbstractMapInteractiveEntity.HasPopupOpened = False
		AudioNoiseHandler.Instance.BoingSound()
		If Me.loadKitchen Then
			SceneLoader.LoadScene(Scenes.scene_level_kitchen, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.None, Nothing)
		Else
			SceneLoader.LoadScene(Scenes.scene_level_saltbaker, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, Nothing)
		End If
	End Sub

	' Token: 0x06003705 RID: 14085 RVA: 0x001FB5C0 File Offset: 0x001F99C0
	Private Sub OnBack()
		AbstractMapInteractiveEntity.HasPopupOpened = False
		Me.ReCheck()
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, True, True)
		RemoveHandler MapDifficultySelectStartUI.Current.OnLoadLevelEvent, AddressOf Me.OnLoadLevel
		RemoveHandler MapDifficultySelectStartUI.Current.OnBackEvent, AddressOf Me.OnBack
		RemoveHandler MapConfirmStartUI.Current.OnLoadLevelEvent, AddressOf Me.OnLoadLevel
		RemoveHandler MapConfirmStartUI.Current.OnBackEvent, AddressOf Me.OnBack
		RemoveHandler MapBasicStartUI.Current.OnLoadLevelEvent, AddressOf Me.OnLoadLevel
		RemoveHandler MapBasicStartUI.Current.OnBackEvent, AddressOf Me.OnBack
	End Sub

	' Token: 0x04003F35 RID: 16181
	Private player1 As MapPlayerController

	' Token: 0x04003F36 RID: 16182
	Private player2 As MapPlayerController

	' Token: 0x04003F37 RID: 16183
	Private p1InTrigger As Boolean

	' Token: 0x04003F38 RID: 16184
	Private p2InTrigger As Boolean

	' Token: 0x04003F39 RID: 16185
	Private loadKitchen As Boolean
End Class
