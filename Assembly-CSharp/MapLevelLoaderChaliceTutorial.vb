Imports System

' Token: 0x0200093E RID: 2366
Public Class MapLevelLoaderChaliceTutorial
	Inherits MapLevelLoader

	' Token: 0x0600375F RID: 14175 RVA: 0x001FD907 File Offset: 0x001FBD07
	Protected Overrides Sub Activate(player As MapPlayerController)
		If PlayerData.Data.Loadouts.GetPlayerLoadout(player.id).charm = Charm.charm_chalice Then
			MyBase.Activate(player)
		Else
			MapEventNotification.Current.ShowTooltipEvent(TooltipEvent.ChaliceTutorialEquipCharm)
		End If
	End Sub
End Class
