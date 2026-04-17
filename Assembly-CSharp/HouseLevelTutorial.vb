Imports System
Imports System.Collections

' Token: 0x020006CF RID: 1743
Public Class HouseLevelTutorial
	Inherits AbstractLevelInteractiveEntity

	' Token: 0x0600251E RID: 9502 RVA: 0x0015C4B2 File Offset: 0x0015A8B2
	Protected Overrides Sub Activate()
		If Me.activated Then
			Return
		End If
		MyBase.Activate()
		MyBase.StartCoroutine(Me.go_cr())
	End Sub

	' Token: 0x0600251F RID: 9503 RVA: 0x0015C4D3 File Offset: 0x0015A8D3
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		CupheadTime.SetLayerSpeed(CupheadTime.Layer.Player, 1F)
	End Sub

	' Token: 0x06002520 RID: 9504 RVA: 0x0015C4E8 File Offset: 0x0015A8E8
	Private Iterator Function go_cr() As IEnumerator
		Me.activated = True
		Dim level As HouseLevel = TryCast(Level.Current, HouseLevel)
		If level Then
			level.StartTutorial()
		End If
		Yield CupheadTime.WaitForSeconds(Me, 0.2F)
		SceneLoader.LoadScene(Scenes.scene_level_tutorial, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, Nothing)
		Return
	End Function

	' Token: 0x06002521 RID: 9505 RVA: 0x0015C503 File Offset: 0x0015A903
	Protected Overrides Sub Show(playerId As PlayerId)
		MyBase.state = AbstractLevelInteractiveEntity.State.Ready
		Me.dialogue = LevelUIInteractionDialogue.Create(Me.dialogueProperties, PlayerManager.GetPlayer(playerId).input, Me.dialogueOffset, 0F, LevelUIInteractionDialogue.TailPosition.Bottom, False)
	End Sub

	' Token: 0x04002DCB RID: 11723
	Private activated As Boolean
End Class
