Imports System
Imports UnityEngine

' Token: 0x02000945 RID: 2373
Public Class MapNPCAppletraveller
	Inherits MonoBehaviour

	' Token: 0x06003772 RID: 14194 RVA: 0x001FDF87 File Offset: 0x001FC387
	Private Sub Start()
		Me.squareRadiusStartWaving = Me.radiusStartWaving * Me.radiusStartWaving
		Me.squareRadiusStopWaving = Me.radiusStopWaving * Me.radiusStopWaving
		Me.AddDialoguerEvents()
	End Sub

	' Token: 0x06003773 RID: 14195 RVA: 0x001FDFB5 File Offset: 0x001FC3B5
	Private Sub OnDestroy()
		Me.RemoveDialoguerEvents()
	End Sub

	' Token: 0x06003774 RID: 14196 RVA: 0x001FDFC0 File Offset: 0x001FC3C0
	Private Sub Update()
		If(Me.state = MapNPCAppletraveller.AppletravellerState.idle AndAlso (MyBase.transform.position - Map.Current.players(0).transform.position).sqrMagnitude < Me.squareRadiusStartWaving) OrElse (PlayerManager.Multiplayer AndAlso (MyBase.transform.position - Map.Current.players(1).transform.position).sqrMagnitude < Me.squareRadiusStartWaving) Then
			Me.state = MapNPCAppletraveller.AppletravellerState.wave
			Me.animator.SetTrigger("wave")
		ElseIf Me.state = MapNPCAppletraveller.AppletravellerState.wave Then
			If(MyBase.transform.position - Map.Current.players(0).transform.position).sqrMagnitude > Me.squareRadiusStartWaving AndAlso (Not PlayerManager.Multiplayer OrElse (MyBase.transform.position - Map.Current.players(1).transform.position).sqrMagnitude > Me.squareRadiusStartWaving) Then
				Me.state = MapNPCAppletraveller.AppletravellerState.idle
				Me.animator.SetTrigger("next")
			End If
			If(MyBase.transform.position - Map.Current.players(0).transform.position).sqrMagnitude < Me.squareRadiusStopWaving OrElse (PlayerManager.Multiplayer AndAlso (MyBase.transform.position - Map.Current.players(1).transform.position).sqrMagnitude < Me.squareRadiusStopWaving) Then
				Me.state = MapNPCAppletraveller.AppletravellerState.wait
				Me.animator.SetTrigger("next")
			End If
		ElseIf(MyBase.transform.position - Map.Current.players(0).transform.position).sqrMagnitude > Me.squareRadiusStartWaving AndAlso (Not PlayerManager.Multiplayer OrElse (MyBase.transform.position - Map.Current.players(1).transform.position).sqrMagnitude > Me.squareRadiusStartWaving) Then
			Me.state = MapNPCAppletraveller.AppletravellerState.idle
		End If
	End Sub

	' Token: 0x06003775 RID: 14197 RVA: 0x001FE230 File Offset: 0x001FC630
	Protected Sub OnDrawGizmosSelected()
		Gizmos.color = Color.green
		Gizmos.DrawWireSphere(MyBase.transform.position, Me.radiusStartWaving)
		Gizmos.color = Color.red
		Gizmos.DrawWireSphere(MyBase.transform.position, Me.radiusStopWaving)
	End Sub

	' Token: 0x06003776 RID: 14198 RVA: 0x001FE27D File Offset: 0x001FC67D
	Public Sub AddDialoguerEvents()
		AddHandler Dialoguer.events.onMessageEvent, AddressOf Me.OnDialoguerMessageEvent
	End Sub

	' Token: 0x06003777 RID: 14199 RVA: 0x001FE295 File Offset: 0x001FC695
	Public Sub RemoveDialoguerEvents()
		RemoveHandler Dialoguer.events.onMessageEvent, AddressOf Me.OnDialoguerMessageEvent
	End Sub

	' Token: 0x06003778 RID: 14200 RVA: 0x001FE2B0 File Offset: 0x001FC6B0
	Private Sub OnDialoguerMessageEvent(message As String, metadata As String)
		If Me.SkipDialogueEvent Then
			Return
		End If
		If message = "MacCoin" AndAlso Not PlayerData.Data.coinManager.GetCoinCollected(Me.coinID1) Then
			Dialoguer.SetGlobalFloat(Me.dialoguerVariableID, 1F)
			PlayerData.Data.coinManager.SetCoinValue(Me.coinID1, True, PlayerId.Any)
			PlayerData.Data.coinManager.SetCoinValue(Me.coinID2, True, PlayerId.Any)
			PlayerData.Data.coinManager.SetCoinValue(Me.coinID3, True, PlayerId.Any)
			PlayerData.Data.AddCurrency(PlayerId.PlayerOne, 3)
			PlayerData.Data.AddCurrency(PlayerId.PlayerTwo, 3)
			PlayerData.SaveCurrentFile()
			MapEventNotification.Current.ShowEvent(MapEventNotification.Type.ThreeCoins)
		End If
	End Sub

	' Token: 0x04003F80 RID: 16256
	<SerializeField()>
	Private animator As Animator

	' Token: 0x04003F81 RID: 16257
	<SerializeField()>
	Private dialoguerVariableID As Integer = 6

	' Token: 0x04003F82 RID: 16258
	<SerializeField()>
	Private coinID1 As String = Guid.NewGuid().ToString()

	' Token: 0x04003F83 RID: 16259
	<SerializeField()>
	Private coinID2 As String = Guid.NewGuid().ToString()

	' Token: 0x04003F84 RID: 16260
	<SerializeField()>
	Private coinID3 As String = Guid.NewGuid().ToString()

	' Token: 0x04003F85 RID: 16261
	<SerializeField()>
	Private radiusStartWaving As Single = 50F

	' Token: 0x04003F86 RID: 16262
	<SerializeField()>
	Private radiusStopWaving As Single = 10F

	' Token: 0x04003F87 RID: 16263
	Private squareRadiusStartWaving As Single

	' Token: 0x04003F88 RID: 16264
	Private squareRadiusStopWaving As Single

	' Token: 0x04003F89 RID: 16265
	Private state As MapNPCAppletraveller.AppletravellerState

	' Token: 0x04003F8A RID: 16266
	<HideInInspector()>
	Public SkipDialogueEvent As Boolean

	' Token: 0x02000946 RID: 2374
	Private Enum AppletravellerState
		' Token: 0x04003F8C RID: 16268
		idle
		' Token: 0x04003F8D RID: 16269
		wave
		' Token: 0x04003F8E RID: 16270
		wait
	End Enum
End Class
