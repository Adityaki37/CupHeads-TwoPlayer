Imports System

' Token: 0x02000534 RID: 1332
Public Class ChessCastleLevelStart
	Inherits AbstractLevelInteractiveEntity

	' Token: 0x06001818 RID: 6168 RVA: 0x000D9D9F File Offset: 0x000D819F
	Protected Overrides Sub Activate()
		If Me.activated Then
			Return
		End If
		Me.activated = True
		MyBase.Activate()
		CType(Level.Current, ChessCastleLevel).StartChessLevel()
	End Sub

	' Token: 0x06001819 RID: 6169 RVA: 0x000D9DC9 File Offset: 0x000D81C9
	Protected Overrides Sub Show(playerId As PlayerId)
		MyBase.state = AbstractLevelInteractiveEntity.State.Ready
		Me.dialogue = LevelUIInteractionDialogue.Create(Me.dialogueProperties, PlayerManager.GetPlayer(playerId).input, Me.dialogueOffset, 0F, LevelUIInteractionDialogue.TailPosition.Bottom, False)
	End Sub

	' Token: 0x04002147 RID: 8519
	Private activated As Boolean
End Class
