Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020006D3 RID: 1747
Public Class MausoleumDialogueInteraction
	Inherits DialogueInteractionPoint

	' Token: 0x06002532 RID: 9522 RVA: 0x0015CB06 File Offset: 0x0015AF06
	Public Sub BeginDialogue()
		Me.Activate()
		Me.chaliceAnimator.SetBool("Talking", True)
		Me.speechBubble.waitForRealease = False
	End Sub

	' Token: 0x06002533 RID: 9523 RVA: 0x0015CB2B File Offset: 0x0015AF2B
	Protected Overrides Sub Start()
		MyBase.Start()
		AddHandler Dialoguer.events.onTextPhase, AddressOf Me.OnDialogueTextSound
	End Sub

	' Token: 0x06002534 RID: 9524 RVA: 0x0015CB49 File Offset: 0x0015AF49
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		RemoveHandler Dialoguer.events.onTextPhase, AddressOf Me.OnDialogueTextSound
	End Sub

	' Token: 0x06002535 RID: 9525 RVA: 0x0015CB67 File Offset: 0x0015AF67
	Private Sub OnDialogueTextSound(data As DialoguerTextData)
		If Not String.IsNullOrEmpty("mausoleum_queen_ghost_speech") Then
			AudioManager.[Stop]("mausoleum_queen_ghost_speech")
		End If
		AudioManager.Play("mausoleum_queen_ghost_speech")
	End Sub

	' Token: 0x06002536 RID: 9526 RVA: 0x0015CB8C File Offset: 0x0015AF8C
	Protected Overrides Iterator Function ReactivateInputsCoroutine(playerOneMotor As LevelPlayerMotor, playerTwoMotor As LevelPlayerMotor, playerOneWeaponManager As LevelPlayerWeaponManager, playerTwoWeaponManager As LevelPlayerWeaponManager, animator As Animator) As IEnumerator
		Me.speechBubble.preventQuit = True
		Dim playercontroller As AbstractPlayerController = PlayerManager.GetPlayer(PlayerId.PlayerOne)
		If playercontroller IsNot Nothing AndAlso playercontroller.animator IsNot Nothing AndAlso playercontroller.animator.GetCurrentAnimatorStateInfo(0).IsName("Power_Up") Then
			Yield playercontroller.animator.WaitForAnimationToEnd(Me, "Power_Up", False, True)
		End If
		Me.speechBubble.preventQuit = False
		Yield MyBase.StartCoroutine(MyBase.ReactivateInputsCoroutine(playerOneMotor, playerTwoMotor, playerOneWeaponManager, playerTwoWeaponManager, animator))
		Me.chaliceAnimator.SetBool("Talking", False)
		Yield CupheadTime.WaitForSeconds(Me, 0.5F)
		SceneLoader.LoadLastMap()
		Return
	End Function

	' Token: 0x04002DD8 RID: 11736
	Public chaliceAnimator As Animator
End Class
