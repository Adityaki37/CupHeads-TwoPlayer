Imports System
Imports UnityEngine

' Token: 0x02000957 RID: 2391
Public Class MapNPCNewsieCat
	Inherits MonoBehaviour

	' Token: 0x060037D5 RID: 14293 RVA: 0x00200528 File Offset: 0x001FE928
	Private Sub Start()
		Dialoguer.SetGlobalFloat(40, 0F)
		If Not PlayerData.Data.coinManager.GetCoinCollected(Me.coinID1) Then
			Dialoguer.SetGlobalFloat(39, 0F)
		End If
		Dim array As Levels() = New Levels() { Levels.Veggies, Levels.Slime, Levels.FlyingBlimp, Levels.Flower, Levels.Frogs }
		Dim array2 As Levels() = New Levels() { Levels.OldMan, Levels.RumRunners, Levels.Airplane, Levels.SnowCult, Levels.FlyingCowboy, Levels.Saltbaker }
		Dim num As Integer = 0
		For i As Integer = 0 To array2.Length - 1
			If PlayerData.Data.CheckLevelCompleted(array2(i)) Then
				num += 1
			End If
		Next
		If num > 0 Then
			Dialoguer.SetGlobalFloat(24, 3F)
		ElseIf PlayerData.Data.CheckLevelCompleted(Levels.Devil) Then
			Dialoguer.SetGlobalFloat(24, 2F)
		ElseIf PlayerData.Data.CheckLevelsCompleted(array) Then
			Dialoguer.SetGlobalFloat(24, 1F)
		Else
			Dialoguer.SetGlobalFloat(24, 0F)
		End If
		Me.AddDialoguerEvents()
	End Sub

	' Token: 0x060037D6 RID: 14294 RVA: 0x00200627 File Offset: 0x001FEA27
	Private Sub OnDestroy()
		Me.RemoveDialoguerEvents()
	End Sub

	' Token: 0x060037D7 RID: 14295 RVA: 0x0020062F File Offset: 0x001FEA2F
	Public Sub AddDialoguerEvents()
		AddHandler Dialoguer.events.onMessageEvent, AddressOf Me.OnDialoguerMessageEvent
	End Sub

	' Token: 0x060037D8 RID: 14296 RVA: 0x00200647 File Offset: 0x001FEA47
	Public Sub RemoveDialoguerEvents()
		RemoveHandler Dialoguer.events.onMessageEvent, AddressOf Me.OnDialoguerMessageEvent
	End Sub

	' Token: 0x060037D9 RID: 14297 RVA: 0x00200660 File Offset: 0x001FEA60
	Private Sub OnDialoguerMessageEvent(message As String, metadata As String)
		If message = "NewsieCoin" AndAlso Not PlayerData.Data.coinManager.GetCoinCollected(Me.coinID1) Then
			Dialoguer.SetGlobalFloat(39, 1F)
			PlayerData.Data.coinManager.SetCoinValue(Me.coinID1, True, PlayerId.Any)
			PlayerData.Data.coinManager.SetCoinValue(Me.coinID2, True, PlayerId.Any)
			PlayerData.Data.coinManager.SetCoinValue(Me.coinID3, True, PlayerId.Any)
			PlayerData.Data.AddCurrency(PlayerId.PlayerOne, 3)
			PlayerData.Data.AddCurrency(PlayerId.PlayerTwo, 3)
			PlayerData.SaveCurrentFile()
			MapEventNotification.Current.ShowEvent(MapEventNotification.Type.ThreeCoins)
		End If
	End Sub

	' Token: 0x04003FD0 RID: 16336
	Private Const DIALOGUER_VAR_INDEX As Integer = 24

	' Token: 0x04003FD1 RID: 16337
	Private Const DIALOGUER_VAR_GOT_COIN As Integer = 39

	' Token: 0x04003FD2 RID: 16338
	Private Const DIALOGUER_VAR_INTERACT_COUNTER As Integer = 40

	' Token: 0x04003FD3 RID: 16339
	<SerializeField()>
	Private coinID1 As String = Guid.NewGuid().ToString()

	' Token: 0x04003FD4 RID: 16340
	<SerializeField()>
	Private coinID2 As String = Guid.NewGuid().ToString()

	' Token: 0x04003FD5 RID: 16341
	<SerializeField()>
	Private coinID3 As String = Guid.NewGuid().ToString()
End Class
