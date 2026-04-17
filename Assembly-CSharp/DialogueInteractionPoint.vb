Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x02000426 RID: 1062
Public Class DialogueInteractionPoint
	Inherits SpeechInteractionPoint

	' Token: 0x06000F5C RID: 3932 RVA: 0x00097D2C File Offset: 0x0009612C
	Protected Overrides Sub OnDrawGizmosSelected()
		MyBase.OnDrawGizmosSelected()
		Gizmos.color = New Color(0.8627451F, 0.8627451F, 0.8627451F)
		Dim position As Vector3 = MyBase.transform.position
		position.x += Me.speechBubblePosition.x
		position.y += Me.speechBubblePosition.y
		Gizmos.DrawWireSphere(position, Me.interactionDistance * 0.5F)
		Gizmos.color = Color.red
		Gizmos.DrawWireSphere(Me.playerOneDialoguePosition, 10F)
		Gizmos.color = Color.blue
		Gizmos.DrawWireSphere(Me.playerTwoDialoguePosition, 10F)
	End Sub

	' Token: 0x06000F5D RID: 3933 RVA: 0x00097DE8 File Offset: 0x000961E8
	Protected Overridable Sub Start()
		AddHandler Dialoguer.events.onEnded, AddressOf Me.OnDialogueEndedHandler
		AddHandler Dialoguer.events.onInstantlyEnded, AddressOf Me.OnDialogueEndedHandler
		AddHandler PlayerManager.OnPlayerJoinedEvent, AddressOf Me.OnPlayerJoined
		AddHandler PlayerManager.OnPlayerLeaveEvent, AddressOf Me.OnPlayerLeave
	End Sub

	' Token: 0x06000F5E RID: 3934 RVA: 0x00097E44 File Offset: 0x00096244
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		RemoveHandler Dialoguer.events.onEnded, AddressOf Me.OnDialogueEndedHandler
		RemoveHandler Dialoguer.events.onInstantlyEnded, AddressOf Me.OnDialogueEndedHandler
		RemoveHandler PlayerManager.OnPlayerJoinedEvent, AddressOf Me.OnPlayerJoined
		RemoveHandler PlayerManager.OnPlayerLeaveEvent, AddressOf Me.OnPlayerLeave
		Me.onEndedActionQueue.Clear()
	End Sub

	' Token: 0x06000F5F RID: 3935 RVA: 0x00097EB0 File Offset: 0x000962B0
	Private Sub OnDialogueEndedHandler()
		If MyBase.AbleToActivate() Then
			Me.Show(PlayerId.PlayerOne)
		End If
		For Each action As Action In Me.onEndedActionQueue
			action()
		Next
		Me.onEndedActionQueue.Clear()
	End Sub

	' Token: 0x06000F60 RID: 3936 RVA: 0x00097F28 File Offset: 0x00096328
	Protected Overrides Sub Activate()
		If Me.speechBubble.displayState = SpeechBubble.DisplayState.Hidden Then
			Dim position As Vector3 = MyBase.transform.position
			position.x += Me.speechBubblePosition.x
			position.y += Me.speechBubblePosition.y
			Me.speechBubble.basePosition = position
			If Me.cutsceneCoroutine IsNot Nothing Then
				MyBase.StopCoroutine(Me.cutsceneCoroutine)
			End If
			Me.cutsceneCoroutine = MyBase.StartCoroutine(Me.CutScene_cr())
		End If
	End Sub

	' Token: 0x06000F61 RID: 3937 RVA: 0x00097FC0 File Offset: 0x000963C0
	Private Sub OnPlayerJoined(player As PlayerId)
		If player = PlayerId.PlayerTwo Then
			Dim player2 As AbstractPlayerController = PlayerManager.GetPlayer(PlayerId.PlayerTwo)
			AddHandler player2.OnReviveEvent, AddressOf Me.OnRevive
		End If
	End Sub

	' Token: 0x06000F62 RID: 3938 RVA: 0x00097FF0 File Offset: 0x000963F0
	Private Iterator Function move_cr(player As AbstractPlayerController, xPosition As Single) As IEnumerator
		If player Is Nothing Then
			Return
		End If
		Yield Nothing
		While Not player.gameObject.activeSelf
			Yield Nothing
		End While
		Dim playerMotor As LevelPlayerMotor = Nothing
		Dim playerWeaponManager As LevelPlayerWeaponManager = Nothing
		If player Then
			playerMotor = player.GetComponent(Of LevelPlayerMotor)()
			playerWeaponManager = player.GetComponent(Of LevelPlayerWeaponManager)()
			If playerWeaponManager Then
				playerWeaponManager.DisableInput()
			End If
			If playerMotor Then
				While playerMotor.Dashing
					Yield Nothing
				End While
				playerMotor.DisableInput()
				Yield playerMotor.StartCoroutine(playerMotor.MoveToX_cr(xPosition, 1))
			End If
			Me.onEndedActionQueue.Add(Sub()
				Me.StartCoroutine(Me.ReactivateInputsCoroutine(playerMotor, Nothing, playerWeaponManager, Nothing, Me.animatorOnEnd))
			End Sub)
		End If
		Return
	End Function

	' Token: 0x06000F63 RID: 3939 RVA: 0x00098019 File Offset: 0x00096419
	Private Sub OnPlayerLeave(player As PlayerId)
		If player = PlayerId.PlayerTwo Then
		End If
	End Sub

	' Token: 0x06000F64 RID: 3940 RVA: 0x00098024 File Offset: 0x00096424
	Public Sub OnRevive(pos As Vector3)
		If Me.conversationIsActive Then
			Dim player As AbstractPlayerController = PlayerManager.GetPlayer(PlayerId.PlayerTwo)
			MyBase.StartCoroutine(Me.move_cr(player, Me.playerTwoDialoguePosition.x))
		End If
	End Sub

	' Token: 0x06000F65 RID: 3941 RVA: 0x0009805C File Offset: 0x0009645C
	Private Iterator Function CutScene_cr() As IEnumerator
		If Me.speechBubble.displayState <> SpeechBubble.DisplayState.Hidden Then
			Return
		End If
		Dim playerOneMove As Coroutine = Nothing
		Dim playerTwoMove As Coroutine = Nothing
		Me.conversationIsActive = True
		Dim playerOne As AbstractPlayerController = PlayerManager.GetPlayer(PlayerId.PlayerOne)
		Dim playerTwo As AbstractPlayerController = PlayerManager.GetPlayer(PlayerId.PlayerTwo)
		playerOneMove = MyBase.StartCoroutine(Me.move_cr(playerOne, Me.playerOneDialoguePosition.x))
		playerTwoMove = MyBase.StartCoroutine(Me.move_cr(playerTwo, Me.playerTwoDialoguePosition.x))
		Yield playerOneMove
		Yield playerTwoMove
		If Me.animatorOnStart IsNot Nothing Then
			Me.StartAnimation()
			While Not Me.animatorOnStart.GetCurrentAnimatorStateInfo(0).IsName(Me.animationOnStartTextName)
				Yield Nothing
			End While
		End If
		Dialoguer.StartDialogue(Me.dialogueInteraction)
		Me.onEndedActionQueue.Add(Sub()
			If Me.animatorOnEnd IsNot Nothing Then
				Me.EndAnimation()
			End If
			playerOne = PlayerManager.GetPlayer(PlayerId.PlayerOne)
			playerTwo = PlayerManager.GetPlayer(PlayerId.PlayerTwo)
			Dim levelPlayerMotor As LevelPlayerMotor = Nothing
			Dim levelPlayerWeaponManager As LevelPlayerWeaponManager = Nothing
			If playerTwo IsNot Nothing Then
				levelPlayerMotor = playerTwo.GetComponent(Of LevelPlayerMotor)()
				levelPlayerWeaponManager = playerTwo.GetComponent(Of LevelPlayerWeaponManager)()
			End If
			Me.conversationIsActive = False
			Me.StartCoroutine(Me.ReactivateInputsCoroutine(playerOne.GetComponent(Of LevelPlayerMotor)(), levelPlayerMotor, playerOne.GetComponent(Of LevelPlayerWeaponManager)(), levelPlayerWeaponManager, Me.animatorOnEnd))
		End Sub)
		Return
	End Function

	' Token: 0x06000F66 RID: 3942 RVA: 0x00098078 File Offset: 0x00096478
	Protected Overridable Iterator Function ReactivateInputsCoroutine(playerOneMotor As LevelPlayerMotor, playerTwoMotor As LevelPlayerMotor, playerOneWeaponManager As LevelPlayerWeaponManager, playerTwoWeaponManager As LevelPlayerWeaponManager, animator As Animator) As IEnumerator
		If animator IsNot Nothing Then
			If Me.animationOnGiveBackInputAtEnd IsNot Nothing AndAlso Me.animationOnGiveBackInputAtEnd <> String.Empty Then
				While Not animator.GetCurrentAnimatorStateInfo(0).IsName(Me.animationOnGiveBackInput) AndAlso (Not animator.GetCurrentAnimatorStateInfo(0).IsName(Me.animationOnGiveBackInputAtEnd) OrElse CDbl(animator.GetCurrentAnimatorStateInfo(0).normalizedTime) <= 0.99)
					Yield Nothing
				End While
			Else
				While Not animator.GetCurrentAnimatorStateInfo(0).IsName(Me.animationOnGiveBackInput)
					Yield Nothing
				End While
			End If
		End If
		playerOneMotor.ClearBufferedInput()
		playerOneMotor.EnableInput()
		playerOneWeaponManager.EnableInput()
		If playerTwoMotor Then
			playerTwoMotor.ClearBufferedInput()
			playerTwoMotor.EnableInput()
		End If
		If playerTwoWeaponManager Then
			playerTwoWeaponManager.EnableInput()
		End If
		Return
	End Function

	' Token: 0x06000F67 RID: 3943 RVA: 0x000980B8 File Offset: 0x000964B8
	Protected Overridable Sub StartAnimation()
		Me.animatorOnStart.SetTrigger(Me.animationTriggerOnStart)
	End Sub

	' Token: 0x06000F68 RID: 3944 RVA: 0x000980CB File Offset: 0x000964CB
	Protected Overridable Sub EndAnimation()
		Me.animatorOnEnd.SetTrigger(Me.animationTriggerOnEnd)
	End Sub

	' Token: 0x04001870 RID: 6256
	<SerializeField()>
	Protected speechBubble As SpeechBubble

	' Token: 0x04001871 RID: 6257
	<SerializeField()>
	Public dialogueInteraction As DialoguerDialogues

	' Token: 0x04001872 RID: 6258
	<SerializeField()>
	Private speechBubblePosition As Vector2

	' Token: 0x04001873 RID: 6259
	Public playerOneDialoguePosition As Vector2

	' Token: 0x04001874 RID: 6260
	Public playerTwoDialoguePosition As Vector2

	' Token: 0x04001875 RID: 6261
	Public animatorOnStart As Animator

	' Token: 0x04001876 RID: 6262
	Public animationTriggerOnStart As String

	' Token: 0x04001877 RID: 6263
	Public animationOnStartTextName As String

	' Token: 0x04001878 RID: 6264
	Public animatorOnEnd As Animator

	' Token: 0x04001879 RID: 6265
	Public animationTriggerOnEnd As String

	' Token: 0x0400187A RID: 6266
	Public animationOnGiveBackInput As String

	' Token: 0x0400187B RID: 6267
	Public animationOnGiveBackInputAtEnd As String

	' Token: 0x0400187C RID: 6268
	Private cutsceneCoroutine As Coroutine

	' Token: 0x0400187D RID: 6269
	Private onEndedActionQueue As List(Of Action) = New List(Of Action)()

	' Token: 0x0400187E RID: 6270
	<HideInInspector()>
	Public conversationIsActive As Boolean
End Class
