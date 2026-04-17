Imports System
Imports System.Collections

' Token: 0x02000952 RID: 2386
Public Class MapNPCGraveyardGhost
	Inherits MapDialogueInteraction

	' Token: 0x060037BA RID: 14266 RVA: 0x001FFC08 File Offset: 0x001FE008
	Protected Overrides Sub Start()
		MyBase.Start()
		If CharmCurse.IsMaxLevel(PlayerId.PlayerOne) OrElse CharmCurse.IsMaxLevel(PlayerId.PlayerTwo) Then
			Dialoguer.SetGlobalFloat(41, 2F)
		ElseIf CharmCurse.CalculateLevel(PlayerId.PlayerOne) > -1 OrElse CharmCurse.CalculateLevel(PlayerId.PlayerTwo) > -1 Then
			Dialoguer.SetGlobalFloat(41, 1F)
		Else
			Dialoguer.SetGlobalFloat(41, 0F)
		End If
	End Sub

	' Token: 0x17000486 RID: 1158
	' (get) Token: 0x060037BB RID: 14267 RVA: 0x001FFC77 File Offset: 0x001FE077
	Protected Overrides ReadOnly Property ChangesDepth As Boolean
		Get
			Return False
		End Get
	End Property

	' Token: 0x060037BC RID: 14268 RVA: 0x001FFC7A File Offset: 0x001FE07A
	Public Sub TalkAfterPlayerGotCharm()
		MyBase.StartCoroutine(Me.got_charm_notification_cr())
	End Sub

	' Token: 0x060037BD RID: 14269 RVA: 0x001FFC8C File Offset: 0x001FE08C
	Private Iterator Function got_charm_notification_cr() As IEnumerator
		Dialoguer.SetGlobalFloat(41, 1F)
		While Map.Current.players(0).state = MapPlayerController.State.Stationary OrElse (Map.Current.players(1) IsNot Nothing AndAlso Map.Current.players(1).state = MapPlayerController.State.Stationary)
			Yield Nothing
		End While
		Me.StartSpeechBubble()
		While Me.currentlySpeaking
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060037BE RID: 14270 RVA: 0x001FFCA8 File Offset: 0x001FE0A8
	Protected Overrides Sub Update()
		MyBase.Update()
		If MyBase.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") Then
			If MyBase.animator.GetCurrentAnimatorStateInfo(0).normalizedTime Mod 1F < Me.idleNormalizedTime Then
				Me.idleCycleCount += 1
				If Me.idleCycleCount Mod 3 = 0 Then
					MyBase.animator.SetTrigger("Puff")
				End If
				If Me.idleCycleCount Mod 7 = 3 Then
					MyBase.animator.SetTrigger("BlinkOnce")
				End If
				If Me.idleCycleCount Mod 7 = 6 Then
					MyBase.animator.SetTrigger("BlinkTwice")
				End If
			End If
			Me.idleNormalizedTime = MyBase.animator.GetCurrentAnimatorStateInfo(0).normalizedTime Mod 1F
		End If
	End Sub

	' Token: 0x04003FBC RID: 16316
	Private Const GRAVEYARD_GHOST_STATE_INDEX As Integer = 41

	' Token: 0x04003FBD RID: 16317
	Private idleNormalizedTime As Single

	' Token: 0x04003FBE RID: 16318
	Private idleCycleCount As Integer

	' Token: 0x04003FBF RID: 16319
	Private nextPuffMultiplier As Integer = 4
End Class
