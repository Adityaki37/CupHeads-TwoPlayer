Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020006CD RID: 1741
Public Class HouseLevelExit
	Inherits AbstractLevelInteractiveEntity

	' Token: 0x06002517 RID: 9495 RVA: 0x0015C12C File Offset: 0x0015A52C
	Protected Overrides Sub Activate()
		If Me.activated Then
			Return
		End If
		MyBase.playerActivating.transform.position += CType(MyBase.playerActivating, LevelPlayerController).motor.DistanceToGround() * Vector3.down
		Me.nonActivating = Nothing
		Me.SwitchToRun(MyBase.playerActivating.id)
		If PlayerManager.Multiplayer Then
			Me.nonActivating = CType(PlayerManager.GetPlayer(PlayerId.PlayerTwo - CInt(MyBase.playerActivating.id)), LevelPlayerController)
		End If
		MyBase.StartCoroutine(Me.go_cr())
	End Sub

	' Token: 0x06002518 RID: 9496 RVA: 0x0015C1CC File Offset: 0x0015A5CC
	Private Sub SwitchToRun(id As PlayerId)
		Dim gameObject As GameObject = If((id <> PlayerId.PlayerOne), If((Not PlayerManager.player1IsMugman), Me.mugmanRunning, Me.cupheadRunning), If((Not PlayerManager.player1IsMugman), Me.cupheadRunning, Me.mugmanRunning))
		Dim player As AbstractPlayerController = PlayerManager.GetPlayer(id)
		If player.stats.isChalice Then
			gameObject = Me.chaliceRunning
		End If
		player.gameObject.SetActive(False)
		gameObject.transform.position = player.transform.position
		gameObject.gameObject.SetActive(True)
		CType(player, LevelPlayerController).DisableInput()
		CType(player, LevelPlayerController).PauseAll()
	End Sub

	' Token: 0x06002519 RID: 9497 RVA: 0x0015C280 File Offset: 0x0015A680
	Private Iterator Function go_cr() As IEnumerator
		Me.activated = True
		Dim timeToGround As Single = 0F
		If Me.nonActivating IsNot Nothing Then
			While Me.nonActivating IsNot Nothing AndAlso Me.nonActivating.gameObject.activeInHierarchy AndAlso Not Me.nonActivating.motor.Grounded
				timeToGround += CupheadTime.Delta
				Yield Nothing
			End While
			If Me.nonActivating.gameObject.activeInHierarchy AndAlso Me.nonActivating IsNot Nothing Then
				Me.SwitchToRun(Me.nonActivating.id)
			End If
		End If
		If timeToGround < 1F Then
			Yield CupheadTime.WaitForSeconds(Me, 1F - timeToGround)
		End If
		SceneLoader.LoadScene(Me.sceneLoadOnExit, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, Nothing)
		Return
	End Function

	' Token: 0x0600251A RID: 9498 RVA: 0x0015C29B File Offset: 0x0015A69B
	Protected Overrides Sub Show(playerId As PlayerId)
		MyBase.state = AbstractLevelInteractiveEntity.State.Ready
		Me.dialogue = LevelUIInteractionDialogue.Create(Me.dialogueProperties, PlayerManager.GetPlayer(playerId).input, Me.dialogueOffset, 0F, LevelUIInteractionDialogue.TailPosition.Left, False)
	End Sub

	' Token: 0x04002DC5 RID: 11717
	Private activated As Boolean

	' Token: 0x04002DC6 RID: 11718
	<SerializeField()>
	Private cupheadRunning As GameObject

	' Token: 0x04002DC7 RID: 11719
	<SerializeField()>
	Private mugmanRunning As GameObject

	' Token: 0x04002DC8 RID: 11720
	<SerializeField()>
	Private chaliceRunning As GameObject

	' Token: 0x04002DC9 RID: 11721
	<SerializeField()>
	Private sceneLoadOnExit As Scenes

	' Token: 0x04002DCA RID: 11722
	Private nonActivating As LevelPlayerController
End Class
