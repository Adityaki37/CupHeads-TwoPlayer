Imports System
Imports UnityEngine

' Token: 0x02000934 RID: 2356
Public Class MapCoin
	Inherits MonoBehaviour

	' Token: 0x06003719 RID: 14105 RVA: 0x001FBCBC File Offset: 0x001FA0BC
	Private Sub Start()
		If PlayerData.Data.coinManager.GetCoinCollected(Me.coinID) Then
			Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		End If
	End Sub

	' Token: 0x0600371A RID: 14106 RVA: 0x001FBCE4 File Offset: 0x001FA0E4
	Private Sub OnTriggerEnter2D(collider As Collider2D)
		If Not PlayerData.Data.coinManager.GetCoinCollected(Me.coinID) Then
			PlayerData.Data.coinManager.SetCoinValue(Me.coinID, True, PlayerId.Any)
			PlayerData.Data.AddCurrency(PlayerId.PlayerOne, 1)
			PlayerData.Data.AddCurrency(PlayerId.PlayerTwo, 1)
			PlayerData.SaveCurrentFile()
			MapEventNotification.Current.ShowEvent(MapEventNotification.Type.Coin)
			If Me.mapNPCCoinMoneyman IsNot Nothing Then
				Me.mapNPCCoinMoneyman.UpdateCoins()
			End If
			Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		End If
	End Sub

	' Token: 0x04003F4C RID: 16204
	<SerializeField()>
	Private mapNPCCoinMoneyman As MapNPCCoinMoneyman

	' Token: 0x04003F4D RID: 16205
	Public coinID As String = Guid.NewGuid().ToString()
End Class
