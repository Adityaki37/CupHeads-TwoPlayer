Imports System
Imports UnityEngine

' Token: 0x0200094D RID: 2381
Public Class MapNPCCircusgirl
	Inherits MonoBehaviour

	' Token: 0x060037A8 RID: 14248 RVA: 0x001FF7E4 File Offset: 0x001FDBE4
	Private Sub Start()
		If PlayerData.Data.coinManager.GetCoinCollected(Me.coinID) Then
			Dialoguer.SetGlobalFloat(Me.dialoguerVariableID, 2F)
		Else
			Me.AddDialoguerEvents()
			OnlineManager.Instance.[Interface].GetAchievement(PlayerId.PlayerOne, "FoundSecretPassage", Sub(achievement As OnlineAchievement)
				If achievement.IsUnlocked Then
					Dialoguer.SetGlobalFloat(Me.dialoguerVariableID, 1F)
				End If
			End Sub)
		End If
	End Sub

	' Token: 0x060037A9 RID: 14249 RVA: 0x001FF847 File Offset: 0x001FDC47
	Private Sub OnDestroy()
		Me.RemoveDialoguerEvents()
	End Sub

	' Token: 0x060037AA RID: 14250 RVA: 0x001FF84F File Offset: 0x001FDC4F
	Public Sub AddDialoguerEvents()
		AddHandler Dialoguer.events.onMessageEvent, AddressOf Me.OnDialoguerMessageEvent
	End Sub

	' Token: 0x060037AB RID: 14251 RVA: 0x001FF867 File Offset: 0x001FDC67
	Public Sub RemoveDialoguerEvents()
		RemoveHandler Dialoguer.events.onMessageEvent, AddressOf Me.OnDialoguerMessageEvent
	End Sub

	' Token: 0x060037AC RID: 14252 RVA: 0x001FF880 File Offset: 0x001FDC80
	Private Sub OnDialoguerMessageEvent(message As String, metadata As String)
		If Me.SkipDialogueEvent Then
			Return
		End If
		If message = "GingerbreadCoin" AndAlso Not PlayerData.Data.coinManager.GetCoinCollected(Me.coinID) Then
			Dialoguer.SetGlobalFloat(Me.dialoguerVariableID, 2F)
			PlayerData.Data.coinManager.SetCoinValue(Me.coinID, True, PlayerId.Any)
			PlayerData.Data.AddCurrency(PlayerId.PlayerOne, 1)
			PlayerData.Data.AddCurrency(PlayerId.PlayerTwo, 1)
			PlayerData.SaveCurrentFile()
			MapEventNotification.Current.ShowEvent(MapEventNotification.Type.Coin)
		End If
	End Sub

	' Token: 0x04003FAD RID: 16301
	<SerializeField()>
	Private dialoguerVariableID As Integer = 7

	' Token: 0x04003FAE RID: 16302
	<SerializeField()>
	Private coinID As String = Guid.NewGuid().ToString()

	' Token: 0x04003FAF RID: 16303
	<HideInInspector()>
	Public SkipDialogueEvent As Boolean
End Class
