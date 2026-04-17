Imports System
Imports UnityEngine

' Token: 0x0200094E RID: 2382
Public Class MapNPCCoinMoneyman
	Inherits MonoBehaviour

	' Token: 0x060037AF RID: 14255 RVA: 0x001FF949 File Offset: 0x001FDD49
	Private Sub Start()
		Me.UpdateCoins()
		Me.LookAroundFinished()
	End Sub

	' Token: 0x060037B0 RID: 14256 RVA: 0x001FF958 File Offset: 0x001FDD58
	Public Sub UpdateCoins()
		For i As Integer = 0 To Me.hiddenCoinIds.Length - 1
			If Not PlayerData.Data.coinManager.GetCoinCollected(Me.hiddenCoinIds(i)) Then
				Return
			End If
		Next
		Dialoguer.SetGlobalFloat(Me.dialoguerVariableID, 1F)
		PlayerData.SaveCurrentFile()
	End Sub

	' Token: 0x060037B1 RID: 14257 RVA: 0x001FF9B0 File Offset: 0x001FDDB0
	Private Sub Update()
		If Me.waiting Then
			Return
		End If
		Me.durationBeforeNext -= CupheadTime.Delta
		Me.durationBeforeBlink -= CupheadTime.Delta
		If Me.durationBeforeBlink <= 0F Then
			Me.durationBeforeBlink = Single.PositiveInfinity
			Me.animator.SetTrigger("blink")
		End If
		If Me.durationBeforeNext <= 0F Then
			Me.waiting = True
			Me.animator.SetTrigger("next")
		End If
	End Sub

	' Token: 0x060037B2 RID: 14258 RVA: 0x001FFA49 File Offset: 0x001FDE49
	Private Sub LookAroundFinished()
		Me.durationBeforeNext = Global.UnityEngine.Random.Range(Me.idleDurationMin, Me.idleDurationMax)
		Me.durationBeforeBlink = Global.UnityEngine.Random.Range(0F, Me.durationBeforeNext)
		Me.waiting = False
	End Sub

	' Token: 0x04003FB0 RID: 16304
	<SerializeField()>
	Private animator As Animator

	' Token: 0x04003FB1 RID: 16305
	<SerializeField()>
	Private idleDurationMin As Single

	' Token: 0x04003FB2 RID: 16306
	<SerializeField()>
	Private idleDurationMax As Single

	' Token: 0x04003FB3 RID: 16307
	Private durationBeforeNext As Single

	' Token: 0x04003FB4 RID: 16308
	Private durationBeforeBlink As Single

	' Token: 0x04003FB5 RID: 16309
	Private waiting As Boolean = True

	' Token: 0x04003FB6 RID: 16310
	<SerializeField()>
	Private hiddenCoinIds As String()

	' Token: 0x04003FB7 RID: 16311
	<SerializeField()>
	Private dialoguerVariableID As Integer = 4
End Class
