Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020006D2 RID: 1746
Public Class KitchenSaltbakerCounter
	Inherits DialogueInteractionPoint

	' Token: 0x0600252A RID: 9514 RVA: 0x0015C894 File Offset: 0x0015AC94
	Protected Overrides Sub Start()
		MyBase.Start()
		AddHandler Dialoguer.events.onTextPhase, AddressOf Me.onDialogueAdvancedHandler
		AddHandler Dialoguer.events.onEnded, AddressOf Me.onDialogueEndedHandler
		If Dialoguer.GetGlobalFloat(23) = 0F Then
			MyBase.StartCoroutine(Me.dialogue_on_first_visit_cr())
		End If
	End Sub

	' Token: 0x0600252B RID: 9515 RVA: 0x0015C8F1 File Offset: 0x0015ACF1
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		RemoveHandler Dialoguer.events.onTextPhase, AddressOf Me.onDialogueAdvancedHandler
		RemoveHandler Dialoguer.events.onEnded, AddressOf Me.onDialogueEndedHandler
	End Sub

	' Token: 0x0600252C RID: 9516 RVA: 0x0015C928 File Offset: 0x0015AD28
	Private Sub onDialogueAdvancedHandler(data As DialoguerTextData)
		If Not MyBase.animator.GetCurrentAnimatorStateInfo(0).IsName("Talk") Then
			MyBase.animator.SetTrigger("Talk")
		End If
	End Sub

	' Token: 0x0600252D RID: 9517 RVA: 0x0015C963 File Offset: 0x0015AD63
	Private Sub onDialogueEndedHandler()
		MyBase.animator.SetBool("PlayerClose", False)
	End Sub

	' Token: 0x0600252E RID: 9518 RVA: 0x0015C976 File Offset: 0x0015AD76
	Protected Overrides Sub Activate()
		MyBase.animator.SetBool("PlayerClose", True)
		MyBase.animator.SetTrigger("Talk")
		MyBase.Activate()
	End Sub

	' Token: 0x0600252F RID: 9519 RVA: 0x0015C9A0 File Offset: 0x0015ADA0
	Private Iterator Function dialogue_on_first_visit_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1.5F)
		Me.speechBubble.waitForRealease = False
		Me.Activate()
		Me.Hide(PlayerId.PlayerOne)
		If PlayerManager.GetPlayer(PlayerId.PlayerTwo) IsNot Nothing Then
			Me.Hide(PlayerId.PlayerTwo)
		End If
		Return
	End Function

	' Token: 0x06002530 RID: 9520 RVA: 0x0015C9BC File Offset: 0x0015ADBC
	Private Sub Update()
		MyBase.animator.SetBool("PlayerClose", Me.conversationIsActive)
		Me.blinkTimer -= CupheadTime.Delta
		If Me.blinkTimer < 0F Then
			Me.blinkTimer = Me.blinkRange.RandomFloat()
			MyBase.animator.SetTrigger("Blink")
		End If
	End Sub

	' Token: 0x04002DD5 RID: 11733
	Private Const DIALOGUER_VAR_ID As Integer = 23

	' Token: 0x04002DD6 RID: 11734
	<SerializeField()>
	Private blinkRange As MinMax = New MinMax(2.5F, 4.5F)

	' Token: 0x04002DD7 RID: 11735
	Private blinkTimer As Single
End Class
