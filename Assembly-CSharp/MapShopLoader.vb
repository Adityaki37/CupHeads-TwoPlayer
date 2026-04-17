Imports System
Imports UnityEngine

' Token: 0x02000961 RID: 2401
Public Class MapShopLoader
	Inherits AbstractMapInteractiveEntity

	' Token: 0x06003807 RID: 14343 RVA: 0x002012E0 File Offset: 0x001FF6E0
	Protected Overrides Sub Activate(player As MapPlayerController)
		If AbstractMapInteractiveEntity.HasPopupOpened Then
			Return
		End If
		AbstractMapInteractiveEntity.HasPopupOpened = True
		MyBase.Activate(player)
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, False, False)
		AudioManager.Play("world_map_level_difficulty_appear")
		Map.Current.OnLoadShop()
		PlayerData.Data.CurrentMapData.playerOnePosition = MyBase.transform.position + Me.returnPositions.playerOne
		PlayerData.Data.CurrentMapData.playerTwoPosition = MyBase.transform.position + Me.returnPositions.playerTwo
		If Not PlayerManager.Multiplayer Then
			PlayerData.Data.CurrentMapData.playerOnePosition = MyBase.transform.position + Me.returnPositions.singlePlayer
		End If
		MapBasicStartUI.Current.level = "Shop"
		MapBasicStartUI.Current.[In](player)
		AddHandler MapBasicStartUI.Current.OnLoadLevelEvent, AddressOf Me.OnLoadLevel
		AddHandler MapBasicStartUI.Current.OnBackEvent, AddressOf Me.OnBack
	End Sub

	' Token: 0x06003808 RID: 14344 RVA: 0x0020140D File Offset: 0x001FF80D
	Private Sub OnLoadLevel()
		AbstractMapInteractiveEntity.HasPopupOpened = False
		AudioNoiseHandler.Instance.BoingSound()
		SceneLoader.LoadScene(If((Not Me.isDLCShop), Scenes.scene_shop, Scenes.scene_shop_DLC), SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.None, Nothing)
	End Sub

	' Token: 0x06003809 RID: 14345 RVA: 0x0020143C File Offset: 0x001FF83C
	Private Sub OnBack()
		AbstractMapInteractiveEntity.HasPopupOpened = False
		Me.ReCheck()
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, True, True)
		RemoveHandler MapBasicStartUI.Current.OnLoadLevelEvent, AddressOf Me.OnLoadLevel
		RemoveHandler MapBasicStartUI.Current.OnBackEvent, AddressOf Me.OnBack
	End Sub

	' Token: 0x04003FEC RID: 16364
	<SerializeField()>
	Private isDLCShop As Boolean
End Class
