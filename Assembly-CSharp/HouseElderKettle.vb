Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020006CC RID: 1740
Public Class HouseElderKettle
	Inherits DialogueInteractionPoint

	' Token: 0x0600250B RID: 9483 RVA: 0x0015BA6B File Offset: 0x00159E6B
	Public Sub BeginDialogue()
		Me.Activate()
		Me.speechBubble.waitForRealease = False
	End Sub

	' Token: 0x0600250C RID: 9484 RVA: 0x0015BA80 File Offset: 0x00159E80
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.hasTarget = False
		AddHandler Dialoguer.events.onTextPhase, AddressOf Me.OnDialogueTextSound
		AddHandler Dialoguer.events.onStarted, AddressOf Me.StartTalkingCoroutine
		AddHandler Dialoguer.events.onMessageEvent, AddressOf Me.OnDialoguerMessageEvent
	End Sub

	' Token: 0x0600250D RID: 9485 RVA: 0x0015BADC File Offset: 0x00159EDC
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		RemoveHandler Dialoguer.events.onTextPhase, AddressOf Me.OnDialogueTextSound
		RemoveHandler Dialoguer.events.onStarted, AddressOf Me.StartTalkingCoroutine
		RemoveHandler Dialoguer.events.onMessageEvent, AddressOf Me.OnDialoguerMessageEvent
	End Sub

	' Token: 0x0600250E RID: 9486 RVA: 0x0015BB34 File Offset: 0x00159F34
	Private Sub OnDialoguerMessageEvent(message As String, metadata As String)
		If message = "ElderKettleBottle" Then
			MyBase.animator.SetTrigger("Bottle")
			MyBase.StartCoroutine(Me.bottle_sound_cr())
		End If
		If message = "ElderKettleFirstWeapon" Then
			MyBase.animator.SetTrigger("Continue")
		End If
	End Sub

	' Token: 0x0600250F RID: 9487 RVA: 0x0015BB8E File Offset: 0x00159F8E
	Private Sub StartTalkingCoroutine()
		MyBase.StartCoroutine(Me.talking_crs())
	End Sub

	' Token: 0x06002510 RID: 9488 RVA: 0x0015BBA0 File Offset: 0x00159FA0
	Private Sub OnDialogueTextSound(data As DialoguerTextData)
		If Not String.IsNullOrEmpty(Me.lastDialogueSFXName) Then
			AudioManager.[Stop](Me.lastDialogueSFXName)
		End If
		If data.metadata = "excitedburst" Then
			If Me.playFirstGroupExcited Then
				AudioManager.Play("ek_excitedburst")
				Me.lastDialogueSFXName = "ek_excitedburst"
				Me.playFirstGroupExcited = False
			Else
				AudioManager.Play("ek_excitedburst2")
				Me.lastDialogueSFXName = "ek_excitedburst2"
				Me.playFirstGroupExcited = True
			End If
		ElseIf data.metadata = "laugh" Then
			If Me.playFirstGroupLaugh Then
				AudioManager.Play("ek_laugh")
				Me.lastDialogueSFXName = "ek_laugh"
				Me.playFirstGroupLaugh = False
			Else
				AudioManager.Play("ek_laugh2")
				Me.lastDialogueSFXName = "ek_laugh2"
				Me.playFirstGroupLaugh = True
			End If
		ElseIf data.metadata = "mckellen" Then
			If Me.playFirstGroupMckellen Then
				AudioManager.Play("ek_mckellen")
				Me.lastDialogueSFXName = "ek_mckellen"
				Me.playFirstGroupMckellen = False
			Else
				AudioManager.Play("ek_mckellen2")
				Me.lastDialogueSFXName = "ek_mckellen2"
				Me.playFirstGroupMckellen = True
			End If
		ElseIf data.metadata = "warstory" Then
			If Me.playFirstGroupWarstory Then
				AudioManager.Play("ek_warstory")
				Me.lastDialogueSFXName = "ek_warstory"
				Me.playFirstGroupWarstory = False
			Else
				AudioManager.Play("ek_warstory2")
				Me.lastDialogueSFXName = "ek_warstory2"
				Me.playFirstGroupWarstory = True
			End If
		End If
	End Sub

	' Token: 0x06002511 RID: 9489 RVA: 0x0015BD4F File Offset: 0x0015A14F
	Public Sub LoopAnimation()
		Me.nbLoopsAnimator -= 1
	End Sub

	' Token: 0x06002512 RID: 9490 RVA: 0x0015BD60 File Offset: 0x0015A160
	Private Iterator Function bottle_sound_cr() As IEnumerator
		Yield New WaitForSeconds(0.1F)
		AudioManager.Play("sfx_potion_reveal")
		Return
	End Function

	' Token: 0x06002513 RID: 9491 RVA: 0x0015BD74 File Offset: 0x0015A174
	Private Iterator Function talking_crs() As IEnumerator
		MyBase.animator.SetBool("IsTalking", True)
		Me.nbLoopsAnimator = Global.UnityEngine.Random.Range(2, 7)
		While Me.conversationIsActive
			If Me.nbLoopsAnimator = 0 Then
				MyBase.animator.SetTrigger("Continue")
				Me.nbLoopsAnimator = Global.UnityEngine.Random.Range(2, 7)
			End If
			Yield Nothing
		End While
		If MyBase.animator.GetCurrentAnimatorStateInfo(0).IsName("Talking_Loop_B") Then
			MyBase.animator.SetTrigger("Continue")
		End If
		MyBase.animator.SetBool("IsTalking", False)
		Return
	End Function

	' Token: 0x06002514 RID: 9492 RVA: 0x0015BD90 File Offset: 0x0015A190
	Protected Overrides Iterator Function ReactivateInputsCoroutine(playerOneMotor As LevelPlayerMotor, playerTwoMotor As LevelPlayerMotor, playerOneWeaponManager As LevelPlayerWeaponManager, playerTwoWeaponManager As LevelPlayerWeaponManager, animator As Animator) As IEnumerator
		Me.speechBubble.preventQuit = True
		Dim playercontroller As AbstractPlayerController = PlayerManager.GetPlayer(PlayerId.PlayerOne)
		If playercontroller IsNot Nothing AndAlso playercontroller.animator IsNot Nothing AndAlso playercontroller.animator.GetCurrentAnimatorStateInfo(0).IsName("Power_Up") Then
			Yield playercontroller.animator.WaitForAnimationToEnd(Me, "Power_Up", False, True)
		End If
		Me.speechBubble.preventQuit = False
		Yield MyBase.StartCoroutine(MyBase.ReactivateInputsCoroutine(playerOneMotor, playerTwoMotor, playerOneWeaponManager, playerTwoWeaponManager, animator))
		Return
	End Function

	' Token: 0x04002DBF RID: 11711
	Private lastDialogueSFXName As String

	' Token: 0x04002DC0 RID: 11712
	Private nbLoopsAnimator As Integer

	' Token: 0x04002DC1 RID: 11713
	Private playFirstGroupMckellen As Boolean = True

	' Token: 0x04002DC2 RID: 11714
	Private playFirstGroupWarstory As Boolean = True

	' Token: 0x04002DC3 RID: 11715
	Private playFirstGroupExcited As Boolean = True

	' Token: 0x04002DC4 RID: 11716
	Private playFirstGroupLaugh As Boolean = True
End Class
