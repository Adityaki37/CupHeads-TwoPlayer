Imports System

' Token: 0x02000531 RID: 1329
Public Class ChessCastleLevelExit
	Inherits AbstractLevelInteractiveEntity

	' Token: 0x0600180F RID: 6159 RVA: 0x000D9CE9 File Offset: 0x000D80E9
	Protected Overrides Sub Activate()
		If Me.activated Then
			Return
		End If
		MyBase.Activate()
		Me.activated = True
		CType(Level.Current, ChessCastleLevel).[Exit]()
	End Sub

	' Token: 0x06001810 RID: 6160 RVA: 0x000D9D13 File Offset: 0x000D8113
	Protected Overrides Sub Show(playerId As PlayerId)
		MyBase.state = AbstractLevelInteractiveEntity.State.Ready
		Me.dialogue = LevelUIInteractionDialogue.Create(Me.dialogueProperties, PlayerManager.GetPlayer(playerId).input, Me.dialogueOffset, 0F, LevelUIInteractionDialogue.TailPosition.Bottom, False)
	End Sub

	' Token: 0x04002146 RID: 8518
	Private activated As Boolean
End Class
