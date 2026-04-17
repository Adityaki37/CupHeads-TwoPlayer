Imports System

' Token: 0x02000533 RID: 1331
Public Class ChessCastleLevelKingInteractionPoint
	Inherits DialogueInteractionPoint

	' Token: 0x06001814 RID: 6164 RVA: 0x000D9D61 File Offset: 0x000D8161
	Public Sub BeginDialogue()
		Me.Activate()
		Me.speechBubble.waitForRealease = False
	End Sub

	' Token: 0x06001815 RID: 6165 RVA: 0x000D9D75 File Offset: 0x000D8175
	Protected Overrides Sub StartAnimation()
		CType(Level.Current, ChessCastleLevel).StartTalkAnimation()
	End Sub

	' Token: 0x06001816 RID: 6166 RVA: 0x000D9D86 File Offset: 0x000D8186
	Protected Overrides Sub EndAnimation()
		CType(Level.Current, ChessCastleLevel).EndTalkAnimation()
	End Sub
End Class
