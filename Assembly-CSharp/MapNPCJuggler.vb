Imports System
Imports UnityEngine

' Token: 0x02000953 RID: 2387
Public Class MapNPCJuggler
	Inherits MonoBehaviour

	' Token: 0x060037C0 RID: 14272 RVA: 0x001FFECC File Offset: 0x001FE2CC
	Private Sub Start()
		Me.AddDialoguerEvents()
		If Dialoguer.GetGlobalFloat(Me.dialoguerVariableID) = 1F Then
			Me.animator.SetTrigger("three")
			Return
		End If
		Dim numParriesInRow As Integer = PlayerData.Data.GetNumParriesInRow(PlayerId.Any)
		If numParriesInRow <= 1 Then
			Me.animator.SetTrigger("one")
		ElseIf numParriesInRow = 2 Then
			Me.animator.SetTrigger("two")
		ElseIf numParriesInRow = 3 Then
			Me.animator.SetTrigger("three")
		ElseIf numParriesInRow > 3 Then
			Me.animator.SetTrigger("three")
			Dialoguer.SetGlobalFloat(Me.dialoguerVariableID, 1F)
			PlayerData.SaveCurrentFile()
		End If
	End Sub

	' Token: 0x060037C1 RID: 14273 RVA: 0x001FFF95 File Offset: 0x001FE395
	Private Sub OnDestroy()
		Me.RemoveDialoguerEvents()
	End Sub

	' Token: 0x060037C2 RID: 14274 RVA: 0x001FFF9D File Offset: 0x001FE39D
	Public Sub AddDialoguerEvents()
		AddHandler Dialoguer.events.onMessageEvent, AddressOf Me.OnDialoguerMessageEvent
	End Sub

	' Token: 0x060037C3 RID: 14275 RVA: 0x001FFFB5 File Offset: 0x001FE3B5
	Public Sub RemoveDialoguerEvents()
		RemoveHandler Dialoguer.events.onMessageEvent, AddressOf Me.OnDialoguerMessageEvent
	End Sub

	' Token: 0x060037C4 RID: 14276 RVA: 0x001FFFD0 File Offset: 0x001FE3D0
	Private Sub OnDialoguerMessageEvent(message As String, metadata As String)
		If Me.SkipDialogueEvent Then
			Return
		End If
		If message = "JugglerCoin" AndAlso Not PlayerData.Data.coinManager.GetCoinCollected(Me.coinID) Then
			PlayerData.Data.coinManager.SetCoinValue(Me.coinID, True, PlayerId.Any)
			PlayerData.Data.AddCurrency(PlayerId.PlayerOne, 1)
			PlayerData.Data.AddCurrency(PlayerId.PlayerTwo, 1)
			PlayerData.SaveCurrentFile()
			MapEventNotification.Current.ShowEvent(MapEventNotification.Type.Coin)
		End If
	End Sub

	' Token: 0x04003FC0 RID: 16320
	<SerializeField()>
	Private animator As Animator

	' Token: 0x04003FC1 RID: 16321
	<SerializeField()>
	Private dialoguerVariableID As Integer

	' Token: 0x04003FC2 RID: 16322
	<SerializeField()>
	Private coinID As String = Guid.NewGuid().ToString()

	' Token: 0x04003FC3 RID: 16323
	<HideInInspector()>
	Public SkipDialogueEvent As Boolean
End Class
