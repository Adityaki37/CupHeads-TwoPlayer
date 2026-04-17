Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200092C RID: 2348
Public Class BoatmanEnabler
	Inherits MapLevelDependentObstacle

	' Token: 0x060036F6 RID: 14070 RVA: 0x001FAD18 File Offset: 0x001F9118
	Protected Overrides Sub Start()
		If DLCManager.DLCEnabled() Then
			MyBase.StartCoroutine(Me.check_cr())
		End If
	End Sub

	' Token: 0x060036F7 RID: 14071 RVA: 0x001FAD34 File Offset: 0x001F9134
	Private Iterator Function check_cr() As IEnumerator
		If Me.forceBoatmanUnlocking OrElse PlayerData.Data.CurrentMap = Scenes.scene_map_world_DLC Then
			Me.OnConditionAlreadyMet()
		ElseIf Not PlayerData.Data.hasUnlockedBoatman Then
			If PlayerData.Data.hasUnlockedFirstSuper Then
				PlayerData.Data.shouldShowBoatmanTooltip = True
				While Not MapEventNotification.Current.showing
					Yield Nothing
				End While
				While MapEventNotification.Current.showing
					Yield Nothing
				End While
				MyBase.StartCoroutine(Me.showAppear_cr())
			ElseIf PlayerData.Data.GetLevelData(Levels.Mausoleum).completed Then
				While AbstractEquipUI.Current.CurrentState = AbstractEquipUI.ActiveState.Inactive
					Yield Nothing
				End While
				While AbstractEquipUI.Current.CurrentState = AbstractEquipUI.ActiveState.Active
					Yield Nothing
				End While
				MyBase.StartCoroutine(Me.showAppear_cr())
			End If
		Else
			Me.OnConditionAlreadyMet()
		End If
		Return
	End Function

	' Token: 0x060036F8 RID: 14072 RVA: 0x001FAD50 File Offset: 0x001F9150
	Private Iterator Function showAppear_cr() As IEnumerator
		Map.Current.CurrentState = Map.State.[Event]
		MapEventNotification.Current.showing = True
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, False, False)
		Dim cupheadMapCamera As CupheadMapCamera = Global.UnityEngine.[Object].FindObjectOfType(Of CupheadMapCamera)()
		Dim cameraStartPos As Vector3 = cupheadMapCamera.transform.position
		If Me.panCamera Then
			Yield cupheadMapCamera.MoveToPosition(MyBase.CameraPosition, 0.5F, 0.9F)
		End If
		MyBase.MapMeetCondition()
		While MyBase.CurrentState <> AbstractMapLevelDependentEntity.State.Complete
			Yield Nothing
		End While
		Yield CupheadTime.WaitForSeconds(Me, 0.25F)
		If Me.panCamera Then
			cupheadMapCamera.MoveToPosition(cameraStartPos, 0.75F, 1F)
		End If
		Map.Current.CurrentState = Map.State.Ready
		MapEventNotification.Current.showing = False
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, True, True)
		Return
	End Function

	' Token: 0x04003F29 RID: 16169
	Private forceBoatmanUnlocking As Boolean
End Class
