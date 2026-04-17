Imports System
Imports UnityEngine

' Token: 0x0200093A RID: 2362
Public Class MapLadderEntity
	Inherits AbstractMapInteractiveEntity

	' Token: 0x0600374A RID: 14154 RVA: 0x001FD334 File Offset: 0x001FB734
	Public Sub Init(playerLadder As MapPlayerLadderObject, [exit] As Vector2, location As MapLadder.Location)
		Me.playerLadder = playerLadder
		Me.[exit] = MyBase.transform.position + [exit]
		Me.location = location
	End Sub

	' Token: 0x0600374B RID: 14155 RVA: 0x001FD360 File Offset: 0x001FB760
	Protected Overrides Function Show(player As PlayerInput) As MapUIInteractionDialogue
		Select Case MyBase.playerChecking.state
			Case MapPlayerController.State.Walking, MapPlayerController.State.LadderExit
				Me.dialogueProperties = MapLadder.DIALOGUE_ENTER
			Case MapPlayerController.State.LadderEnter, MapPlayerController.State.Ladder
				Me.dialogueProperties = MapLadder.DIALOGUE_EXIT
		End Select
		Return MyBase.Show(player)
	End Function

	' Token: 0x0600374C RID: 14156 RVA: 0x001FD3BC File Offset: 0x001FB7BC
	Protected Overrides Sub Activate()
		MyBase.Activate()
		Dim state As MapPlayerController.State = MyBase.playerActivating.state
		If state <> MapPlayerController.State.Walking Then
			If state = MapPlayerController.State.Ladder Then
				MyBase.playerActivating.LadderExit(MyBase.transform.position, Me.[exit], Me.location)
			End If
		Else
			MyBase.playerActivating.LadderEnter(MyBase.transform.position, Me.playerLadder, Me.location)
		End If
	End Sub

	' Token: 0x04003F67 RID: 16231
	Private playerLadder As MapPlayerLadderObject

	' Token: 0x04003F68 RID: 16232
	Private [exit] As Vector2

	' Token: 0x04003F69 RID: 16233
	Private location As MapLadder.Location
End Class
