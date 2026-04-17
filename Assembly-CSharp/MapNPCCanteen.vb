Imports System
Imports UnityEngine

' Token: 0x0200094B RID: 2379
Public Class MapNPCCanteen
	Inherits MonoBehaviour

	' Token: 0x06003797 RID: 14231 RVA: 0x001FEFBC File Offset: 0x001FD3BC
	Private Sub Start()
		If PlayerData.Data.IsUnlocked(PlayerId.PlayerOne, Weapon.plane_weapon_bomb) AndAlso (PlayerManager.Multiplayer OrElse PlayerData.Data.IsUnlocked(PlayerId.PlayerTwo, Weapon.plane_weapon_bomb)) Then
			Dialoguer.SetGlobalFloat(Me.dialoguerVariableID, 1F)
		End If
		Me.AddDialoguerEvents()
	End Sub

	' Token: 0x06003798 RID: 14232 RVA: 0x001FF013 File Offset: 0x001FD413
	Private Sub OnDestroy()
		Me.RemoveDialoguerEvents()
	End Sub

	' Token: 0x06003799 RID: 14233 RVA: 0x001FF01B File Offset: 0x001FD41B
	Public Sub AddDialoguerEvents()
		AddHandler Dialoguer.events.onMessageEvent, AddressOf Me.OnDialoguerMessageEvent
	End Sub

	' Token: 0x0600379A RID: 14234 RVA: 0x001FF033 File Offset: 0x001FD433
	Public Sub RemoveDialoguerEvents()
		RemoveHandler Dialoguer.events.onMessageEvent, AddressOf Me.OnDialoguerMessageEvent
	End Sub

	' Token: 0x0600379B RID: 14235 RVA: 0x001FF04C File Offset: 0x001FD44C
	Private Sub OnDialoguerMessageEvent(message As String, metadata As String)
		If Me.SkipDialogueEvent Then
			Return
		End If
		If message = "CanteenWeaponTwo" Then
			MapEventNotification.Current.ShowTooltipEvent(TooltipEvent.Canteen)
			If Not PlayerData.Data.IsUnlocked(PlayerId.PlayerOne, Weapon.plane_weapon_bomb) Then
				PlayerData.Data.Gift(PlayerId.PlayerOne, Weapon.plane_weapon_bomb)
				If Not PlayerData.Data.Loadouts.GetPlayerLoadout(PlayerId.PlayerOne).HasEquippedSecondarySHMUPWeapon Then
					PlayerData.Data.Loadouts.GetPlayerLoadout(PlayerId.PlayerOne).MustNotifySwitchSHMUPWeapon = True
				End If
				PlayerData.Data.Loadouts.GetPlayerLoadout(PlayerId.PlayerOne).HasEquippedSecondarySHMUPWeapon = True
			End If
			If Not PlayerData.Data.IsUnlocked(PlayerId.PlayerTwo, Weapon.plane_weapon_bomb) Then
				PlayerData.Data.Gift(PlayerId.PlayerTwo, Weapon.plane_weapon_bomb)
				If Not PlayerData.Data.Loadouts.GetPlayerLoadout(PlayerId.PlayerTwo).HasEquippedSecondarySHMUPWeapon Then
					PlayerData.Data.Loadouts.GetPlayerLoadout(PlayerId.PlayerTwo).MustNotifySwitchSHMUPWeapon = True
				End If
				PlayerData.Data.Loadouts.GetPlayerLoadout(PlayerId.PlayerTwo).HasEquippedSecondarySHMUPWeapon = True
			End If
			Dialoguer.SetGlobalFloat(Me.dialoguerVariableID, 1F)
			PlayerData.SaveCurrentFile()
		End If
	End Sub

	' Token: 0x04003FA4 RID: 16292
	<SerializeField()>
	Private dialoguerVariableID As Integer = 13

	' Token: 0x04003FA5 RID: 16293
	<HideInInspector()>
	Public SkipDialogueEvent As Boolean
End Class
