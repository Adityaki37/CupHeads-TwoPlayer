Imports System
Imports System.Collections

' Token: 0x0200059C RID: 1436
Public Class DiceGateLevelToPrevWorld
	Inherits AbstractLevelInteractiveEntity

	' Token: 0x06001B84 RID: 7044 RVA: 0x000FB4E3 File Offset: 0x000F98E3
	Protected Overrides Sub Activate()
		If Me.activated Then
			Return
		End If
		MyBase.Activate()
		MyBase.StartCoroutine(Me.go_cr())
	End Sub

	' Token: 0x06001B85 RID: 7045 RVA: 0x000FB504 File Offset: 0x000F9904
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		CupheadTime.SetLayerSpeed(CupheadTime.Layer.Player, 1F)
	End Sub

	' Token: 0x06001B86 RID: 7046 RVA: 0x000FB518 File Offset: 0x000F9918
	Private Iterator Function go_cr() As IEnumerator
		Me.activated = True
		CupheadTime.SetLayerSpeed(CupheadTime.Layer.Player, 0F)
		For Each abstractPlayerController As AbstractPlayerController In PlayerManager.GetAllPlayers()
			Dim levelPlayerController As LevelPlayerController = CType(abstractPlayerController, LevelPlayerController)
			If Not(levelPlayerController Is Nothing) Then
				levelPlayerController.DisableInput()
				levelPlayerController.PauseAll()
			End If
		Next
		PlayerData.Data.CurrentMapData.hasVisitedDieHouse = True
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		SceneLoader.LoadLastMap()
		Return
	End Function

	' Token: 0x06001B87 RID: 7047 RVA: 0x000FB533 File Offset: 0x000F9933
	Protected Overrides Sub Show(playerId As PlayerId)
		MyBase.state = AbstractLevelInteractiveEntity.State.Ready
		Me.dialogue = LevelUIInteractionDialogue.Create(Me.dialogueProperties, PlayerManager.GetPlayer(playerId).input, Me.dialogueOffset, 0F, LevelUIInteractionDialogue.TailPosition.Left, False)
	End Sub

	' Token: 0x040024A9 RID: 9385
	Private activated As Boolean
End Class
