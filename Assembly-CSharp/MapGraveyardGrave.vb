Imports System
Imports UnityEngine

' Token: 0x02000938 RID: 2360
Public Class MapGraveyardGrave
	Inherits MonoBehaviour

	' Token: 0x0600372E RID: 14126 RVA: 0x001FC84A File Offset: 0x001FAC4A
	Private Sub Start()
		Me.hasCharm = Me.HasCharm()
	End Sub

	' Token: 0x0600372F RID: 14127 RVA: 0x001FC858 File Offset: 0x001FAC58
	Private Sub OnTriggerEnter2D(collision As Collider2D)
		If collision.GetComponent(Of MapPlayerController)() Then
			Dim component As MapPlayerController = collision.GetComponent(Of MapPlayerController)()
			If component.id = PlayerId.PlayerOne Then
				If Me.player1 Is Nothing Then
					Me.player1 = component
				End If
				Me.p1InTrigger = True
			Else
				If Me.player2 Is Nothing Then
					Me.player2 = component
				End If
				Me.p2InTrigger = True
			End If
		End If
	End Sub

	' Token: 0x06003730 RID: 14128 RVA: 0x001FC8CC File Offset: 0x001FACCC
	Private Sub OnTriggerExit2D(collision As Collider2D)
		If collision.GetComponent(Of MapPlayerController)() Then
			Dim component As MapPlayerController = collision.GetComponent(Of MapPlayerController)()
			If component.id = PlayerId.PlayerOne Then
				Me.p1InTrigger = False
			Else
				Me.p2InTrigger = False
			End If
		End If
	End Sub

	' Token: 0x06003731 RID: 14129 RVA: 0x001FC910 File Offset: 0x001FAD10
	Private Function HasCharm() As Boolean
		Return(PlayerData.Data.IsUnlocked(PlayerId.PlayerOne, Charm.charm_curse) AndAlso CharmCurse.CalculateLevel(PlayerId.PlayerOne) >= 0) OrElse (PlayerData.Data.IsUnlocked(PlayerId.PlayerTwo, Charm.charm_curse) AndAlso CharmCurse.CalculateLevel(PlayerId.PlayerTwo) >= 0)
	End Function

	' Token: 0x06003732 RID: 14130 RVA: 0x001FC968 File Offset: 0x001FAD68
	Private Sub Update()
		If SceneLoader.IsInBlurTransition Then
			Return
		End If
		If Me.hasCharm AndAlso Not Me.main.canReenter Then
			Return
		End If
		If Me.canInteract OrElse (Me.hasCharm AndAlso Me.main.canReenter) Then
			If Me.p1InTrigger AndAlso Me.player1.input.actions.GetButtonDown(13) Then
				If Me.player1.animationController.facingUpwards Then
					Me.InteractWith(0)
				End If
			ElseIf Me.p2InTrigger AndAlso Me.player2.input.actions.GetButtonDown(13) AndAlso Me.player2.animationController.facingUpwards Then
				Me.InteractWith(1)
			End If
		End If
	End Sub

	' Token: 0x06003733 RID: 14131 RVA: 0x001FCA50 File Offset: 0x001FAE50
	Private Sub InteractWith(playerNum As Integer)
		If Not Me.isResetGrave Then
			Me.canInteract = False
		End If
		Me.main.ActivatedGrave(Me.index, playerNum, If((Not Me.isResetGrave), Me.ghostPos.transform.position, Vector3.zero))
	End Sub

	' Token: 0x06003734 RID: 14132 RVA: 0x001FCAA6 File Offset: 0x001FAEA6
	Public Sub SetInteractable(value As Boolean)
		Me.canInteract = value
	End Sub

	' Token: 0x04003F52 RID: 16210
	<SerializeField()>
	Private isResetGrave As Boolean

	' Token: 0x04003F53 RID: 16211
	<SerializeField()>
	Private index As Integer

	' Token: 0x04003F54 RID: 16212
	Private player1 As MapPlayerController

	' Token: 0x04003F55 RID: 16213
	Private player2 As MapPlayerController

	' Token: 0x04003F56 RID: 16214
	Private p1InTrigger As Boolean

	' Token: 0x04003F57 RID: 16215
	Private p2InTrigger As Boolean

	' Token: 0x04003F58 RID: 16216
	Private canInteract As Boolean

	' Token: 0x04003F59 RID: 16217
	<SerializeField()>
	Private main As MapGraveyardHandler

	' Token: 0x04003F5A RID: 16218
	<SerializeField()>
	Private ghostPos As Transform

	' Token: 0x04003F5B RID: 16219
	Private hasCharm As Boolean
End Class
