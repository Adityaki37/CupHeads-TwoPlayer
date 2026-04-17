Imports System
Imports UnityEngine
Imports UnityEngine.UI

' Token: 0x020009A2 RID: 2466
Public Class MapUICoins
	Inherits MonoBehaviour

	' Token: 0x060039DF RID: 14815 RVA: 0x0020EDEE File Offset: 0x0020D1EE
	Private Sub Start()
		Me.singleDigitCoinPosition = Me.coinImage.transform.localPosition
	End Sub

	' Token: 0x060039E0 RID: 14816 RVA: 0x0020EE08 File Offset: 0x0020D208
	Private Sub Update()
		If Me.playerId = PlayerId.PlayerTwo Then
			If Not PlayerManager.Multiplayer Then
				Me.coinImage.enabled = False
				Me.currencyNbImage.enabled = False
				Return
			End If
			Me.coinImage.enabled = True
			Me.currencyNbImage.enabled = True
		End If
		Dim currency As Integer = PlayerData.Data.GetCurrency(Me.playerId)
		If currency <> Me.previousCurrency Then
			Me.previousCurrency = currency
			Me.currencyNbImage.sprite = Me.coinSprites(currency)
			If currency > 9 Then
				Me.coinImage.transform.localPosition = Me.doubleDigitCoinTransform.localPosition
			Else
				Me.coinImage.transform.localPosition = Me.singleDigitCoinPosition
			End If
		End If
	End Sub

	' Token: 0x040041CF RID: 16847
	<SerializeField()>
	Private playerId As PlayerId

	' Token: 0x040041D0 RID: 16848
	<SerializeField()>
	Private coinImage As Image

	' Token: 0x040041D1 RID: 16849
	<SerializeField()>
	Private currencyNbImage As Image

	' Token: 0x040041D2 RID: 16850
	<SerializeField()>
	Private coinSprites As Sprite()

	' Token: 0x040041D3 RID: 16851
	<SerializeField()>
	Private doubleDigitCoinTransform As Transform

	' Token: 0x040041D4 RID: 16852
	Private singleDigitCoinPosition As Vector3

	' Token: 0x040041D5 RID: 16853
	Private previousCurrency As Integer = -1
End Class
