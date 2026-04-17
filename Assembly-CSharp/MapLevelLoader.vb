Imports System
Imports UnityEngine

' Token: 0x0200093D RID: 2365
Public Class MapLevelLoader
	Inherits AbstractMapInteractiveEntity

	' Token: 0x06003759 RID: 14169 RVA: 0x001FD4CE File Offset: 0x001FB8CE
	Protected Overrides Sub Awake()
		MyBase.Awake()
	End Sub

	' Token: 0x0600375A RID: 14170 RVA: 0x001FD4D8 File Offset: 0x001FB8D8
	Protected Overrides Sub Activate(player As MapPlayerController)
		If AbstractMapInteractiveEntity.HasPopupOpened Then
			Return
		End If
		If PlatformHelper.ManuallyRefreshDLCAvailability Then
			DLCManager.CheckInstallationStatusChanged()
			If DLCManager.showAvailabilityPrompt Then
				DLCManager.ResetAvailabilityPrompt()
				MapEventNotification.Current.ShowEvent(MapEventNotification.Type.DLCAvailable)
				Return
			End If
		End If
		AbstractMapInteractiveEntity.HasPopupOpened = True
		MyBase.Activate(player)
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, False, False)
		AudioManager.Play("world_map_level_difficulty_appear")
		Map.Current.OnLoadLevel()
		PlayerData.Data.CurrentMapData.playerOnePosition = MyBase.transform.position + Me.returnPositions.playerOne
		PlayerData.Data.CurrentMapData.playerTwoPosition = MyBase.transform.position + Me.returnPositions.playerTwo
		If Not PlayerManager.Multiplayer Then
			PlayerData.Data.CurrentMapData.playerOnePosition = MyBase.transform.position + Me.returnPositions.singlePlayer
		End If
		If Me.askDifficulty Then
			MapDifficultySelectStartUI.Current.level = Me.level.ToString()
			MapDifficultySelectStartUI.Current.[In](player)
			AddHandler MapDifficultySelectStartUI.Current.OnLoadLevelEvent, AddressOf Me.OnLoadLevel
			AddHandler MapDifficultySelectStartUI.Current.OnBackEvent, AddressOf Me.OnBack
		Else
			Dim text As String = Me.level.ToString()
			If text <> "Mausoleum" AndAlso text <> "Devil" AndAlso text <> "ShmupTutorial" AndAlso text <> "ChaliceTutorial" AndAlso text <> "ChessCastle" Then
				MapConfirmStartUI.Current.level = text
				MapConfirmStartUI.Current.[In](player)
				AddHandler MapConfirmStartUI.Current.OnLoadLevelEvent, AddressOf Me.OnLoadLevel
				AddHandler MapConfirmStartUI.Current.OnBackEvent, AddressOf Me.OnBack
				Return
			End If
			If text = "Mausoleum" Then
				If PlayerData.Data.CurrentMap = Scenes.scene_map_world_1 Then
					text = "Mausoleum_1"
				ElseIf PlayerData.Data.CurrentMap = Scenes.scene_map_world_2 Then
					text = "Mausoleum_2"
				Else
					text = "Mausoleum_3"
				End If
			ElseIf text = "ChessCastle" Then
				text = "KingOfGamesWorldMap"
			End If
			MapBasicStartUI.Current.level = text
			MapBasicStartUI.Current.[In](player)
			AddHandler MapBasicStartUI.Current.OnLoadLevelEvent, AddressOf Me.OnLoadLevel
			AddHandler MapBasicStartUI.Current.OnBackEvent, AddressOf Me.OnBack
		End If
	End Sub

	' Token: 0x0600375B RID: 14171 RVA: 0x001FD79C File Offset: 0x001FBB9C
	Private Sub OnLoadLevel()
		AbstractMapInteractiveEntity.HasPopupOpened = False
		AudioManager.HandleSnapshot(AudioManager.Snapshots.Paused.ToString(), 0.5F)
		AudioNoiseHandler.Instance.BoingSound()
		If Me.level = Levels.Devil Then
			SceneLoader.LoadScene(Scenes.scene_cutscene_devil, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, Nothing)
		Else
			SceneLoader.LoadLevel(Me.level, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, Nothing)
		End If
	End Sub

	' Token: 0x0600375C RID: 14172 RVA: 0x001FD800 File Offset: 0x001FBC00
	Private Sub OnBack()
		AbstractMapInteractiveEntity.HasPopupOpened = False
		Me.ReCheck()
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, True, True)
		If Me.askDifficulty Then
			RemoveHandler MapDifficultySelectStartUI.Current.OnLoadLevelEvent, AddressOf Me.OnLoadLevel
			RemoveHandler MapDifficultySelectStartUI.Current.OnBackEvent, AddressOf Me.OnBack
		Else
			RemoveHandler MapConfirmStartUI.Current.OnLoadLevelEvent, AddressOf Me.OnLoadLevel
			RemoveHandler MapConfirmStartUI.Current.OnBackEvent, AddressOf Me.OnBack
			RemoveHandler MapBasicStartUI.Current.OnLoadLevelEvent, AddressOf Me.OnLoadLevel
			RemoveHandler MapBasicStartUI.Current.OnBackEvent, AddressOf Me.OnBack
		End If
		If Me.OnBackCallback IsNot Nothing Then
			Me.OnBackCallback()
			Me.OnBackCallback = CType([Delegate].Remove(Me.OnBackCallback, Me.OnBackCallback), Action)
		End If
	End Sub

	' Token: 0x0600375D RID: 14173 RVA: 0x001FD8E7 File Offset: 0x001FBCE7
	Protected Overrides Sub Reset()
		MyBase.Reset()
		Me.dialogueProperties = New AbstractUIInteractionDialogue.Properties("ENTER <sprite=0>")
	End Sub

	' Token: 0x04003F73 RID: 16243
	<SerializeField()>
	Private level As Levels

	' Token: 0x04003F74 RID: 16244
	<SerializeField()>
	Private askDifficulty As Boolean

	' Token: 0x04003F75 RID: 16245
	Public OnBackCallback As Action
End Class
