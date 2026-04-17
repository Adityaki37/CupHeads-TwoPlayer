Imports System
Imports UnityEngine

' Token: 0x02000950 RID: 2384
Public Class MapNPCCompetitor
	Inherits AbstractMonoBehaviour

	' Token: 0x060037B6 RID: 14262 RVA: 0x001FFB24 File Offset: 0x001FDF24
	Private Sub Update()
		MyBase.animator.SetBool("PlayerClose", Me.interaction.PlayerWithinDistance(0) OrElse (PlayerManager.Multiplayer AndAlso Me.interaction.PlayerWithinDistance(1)) OrElse Me.interaction.currentlySpeaking)
		Me.blinkTimer -= CupheadTime.Delta
		If Me.blinkTimer < 0F Then
			Me.blinkTimer = Me.blinkRange.RandomFloat()
			MyBase.animator.SetTrigger("Blink")
		End If
	End Sub

	' Token: 0x04003FB9 RID: 16313
	<SerializeField()>
	Private interaction As MapDialogueInteraction

	' Token: 0x04003FBA RID: 16314
	<SerializeField()>
	Private blinkRange As MinMax = New MinMax(2.5F, 4.5F)

	' Token: 0x04003FBB RID: 16315
	Private blinkTimer As Single
End Class
