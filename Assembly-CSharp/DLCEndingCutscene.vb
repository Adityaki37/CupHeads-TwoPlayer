Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020003FC RID: 1020
Public Class DLCEndingCutscene
	Inherits DLCGenericCutscene

	' Token: 0x06000E09 RID: 3593 RVA: 0x000912F0 File Offset: 0x0008F6F0
	Protected Overrides Sub Start()
		MyBase.Start()
		AddHandler SceneLoader.OnLoaderCompleteEvent, AddressOf Me.StartMusic
		If Me.trappedChar = DLCGenericCutscene.TrappedChar.None Then
			Me.trappedChar = MyBase.DetectCharacter()
		End If
		Me.shot1Animator.SetBool("NeedToSwap", Me.trappedChar <> DLCGenericCutscene.TrappedChar.Chalice)
		Me.rightCuphead.SetActive(False)
		Me.ghostBodyChalice.SetActive(Me.trappedChar = DLCGenericCutscene.TrappedChar.Chalice)
		Me.ghostBodyCHMM.SetActive(Me.trappedChar <> DLCGenericCutscene.TrappedChar.Chalice)
		Dim trappedChar As DLCGenericCutscene.TrappedChar = Me.trappedChar
		If trappedChar <> DLCGenericCutscene.TrappedChar.Chalice Then
			If trappedChar <> DLCGenericCutscene.TrappedChar.Mugman Then
				If trappedChar = DLCGenericCutscene.TrappedChar.Cuphead Then
					Me.leftCuphead.SetActive(False)
					Me.rightMugman.SetActive(False)
					Me.rightCuphead.SetActive(False)
					Me.trappedChalice.SetActive(False)
					Me.trappedMugman.SetActive(False)
					Me.text(0) = Me.altText(1)
				End If
			Else
				Me.leftMugman.SetActive(False)
				Me.rightMugman.SetActive(False)
				Me.trappedChalice.SetActive(False)
				Me.trappedCuphead.SetActive(False)
				Me.text(0) = Me.altText(0)
			End If
		Else
			Me.leftMugman.SetActive(False)
			Me.rightChalice.SetActive(False)
			Me.trappedMugman.SetActive(False)
			Me.trappedCuphead.SetActive(False)
		End If
	End Sub

	' Token: 0x06000E0A RID: 3594 RVA: 0x0009146A File Offset: 0x0008F86A
	Private Sub StartMusic()
		MyBase.StartCoroutine(Me.handle_music_cr())
	End Sub

	' Token: 0x06000E0B RID: 3595 RVA: 0x0009147C File Offset: 0x0008F87C
	Private Iterator Function handle_music_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 0.5F)
		AudioManager.StartBGMAlternate(0)
		Yield MyBase.StartCoroutine(Me.hold_for_music_advance_and_loop(5.5F, String.Empty))
		AudioManager.StartBGMAlternate(1)
		Yield MyBase.StartCoroutine(Me.hold_for_music_advance_and_loop(3F, String.Empty))
		AudioManager.StartBGMAlternate(2)
		Return
	End Function

	' Token: 0x06000E0C RID: 3596 RVA: 0x00091498 File Offset: 0x0008F898
	Private Iterator Function hold_for_music_advance_and_loop(time As Single, loopName As String) As IEnumerator
		Dim t As Single = 0F
		Me.advanceMusic = False
		While t < time AndAlso Not Me.advanceMusic
			t += Time.deltaTime
			Yield Nothing
		End While
		If Not Me.advanceMusic Then
			AudioManager.PlayLoop(loopName)
		End If
		While Not Me.advanceMusic
			Yield Nothing
		End While
		AudioManager.[Stop](loopName)
		Return
	End Function

	' Token: 0x06000E0D RID: 3597 RVA: 0x000914C4 File Offset: 0x0008F8C4
	Private Iterator Function crossfade_final_music_cr() As IEnumerator
		AudioManager.FadeBGMVolume(0F, 1.5F, True)
		AudioManager.FadeSFXVolume("mus_dlc_ending_4", 0.0001F, 0.0001F)
		Yield Nothing
		AudioManager.Play("mus_dlc_ending_4")
		AudioManager.FadeSFXVolume("mus_dlc_ending_4", 0.4F, 1.5F)
		Return
	End Function

	' Token: 0x06000E0E RID: 3598 RVA: 0x000914D8 File Offset: 0x0008F8D8
	Public Sub AdvanceMusic()
		Me.advanceMusic = True
	End Sub

	' Token: 0x06000E0F RID: 3599 RVA: 0x000914E4 File Offset: 0x0008F8E4
	Public Sub SwapChars()
		Me.rightChalice.SetActive(False)
		Me.trappedChalice.SetActive(True)
		Me.ghostBodyChalice.SetActive(True)
		Me.ghostBodyCHMM.SetActive(False)
		Dim trappedChar As DLCGenericCutscene.TrappedChar = Me.trappedChar
		If trappedChar <> DLCGenericCutscene.TrappedChar.Mugman Then
			If trappedChar = DLCGenericCutscene.TrappedChar.Cuphead Then
				Me.rightCuphead.SetActive(True)
				Me.trappedCuphead.SetActive(False)
			End If
		Else
			Me.rightMugman.SetActive(True)
			Me.trappedMugman.SetActive(False)
		End If
	End Sub

	' Token: 0x06000E10 RID: 3600 RVA: 0x00091574 File Offset: 0x0008F974
	Public Overrides Sub IrisOut()
		SceneLoader.LoadScene(Scenes.scene_cutscene_dlc_credits_comic, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.None, Nothing)
	End Sub

	' Token: 0x06000E11 RID: 3601 RVA: 0x00091581 File Offset: 0x0008F981
	Public Sub StartShake()
		Me.screenShakeAmt = 4F
	End Sub

	' Token: 0x06000E12 RID: 3602 RVA: 0x0009158E File Offset: 0x0008F98E
	Public Sub StopShake()
		Me.screenShakeAmt = 0F
	End Sub

	' Token: 0x06000E13 RID: 3603 RVA: 0x0009159C File Offset: 0x0008F99C
	Private Sub LateUpdate()
		Dim num As Single = Global.UnityEngine.Random.Range(-Me.screenShakeAmt, Me.screenShakeAmt)
		Dim num2 As Single = Global.UnityEngine.Random.Range(-Me.screenShakeAmt, Me.screenShakeAmt)
		Me.screens(Me.curScreen).transform.localPosition = New Vector3(num, num2, 0F)
	End Sub

	' Token: 0x06000E14 RID: 3604 RVA: 0x000915F4 File Offset: 0x0008F9F4
	Protected Overrides Sub OnScreenAdvance(which As Integer)
		If which = 3 Then
			MyBase.StartCoroutine(Me.crossfade_final_music_cr())
			Dim gameObject As GameObject = GameObject.Find("Fader")
			If gameObject IsNot Nothing Then
				Dim component As Animator = gameObject.GetComponent(Of Animator)()
				If component IsNot Nothing Then
					component.Play("Transparent")
				End If
			End If
		End If
	End Sub

	' Token: 0x06000E15 RID: 3605 RVA: 0x0009164A File Offset: 0x0008FA4A
	Protected Overrides Sub OnDestroy()
		RemoveHandler SceneLoader.OnLoaderCompleteEvent, AddressOf Me.StartMusic
		MyBase.OnDestroy()
	End Sub

	' Token: 0x04001756 RID: 5974
	<SerializeField()>
	Private trappedChar As DLCGenericCutscene.TrappedChar

	' Token: 0x04001757 RID: 5975
	<SerializeField()>
	Private leftCuphead As GameObject

	' Token: 0x04001758 RID: 5976
	<SerializeField()>
	Private leftMugman As GameObject

	' Token: 0x04001759 RID: 5977
	<SerializeField()>
	Private rightMugman As GameObject

	' Token: 0x0400175A RID: 5978
	<SerializeField()>
	Private rightChalice As GameObject

	' Token: 0x0400175B RID: 5979
	<SerializeField()>
	Private rightCuphead As GameObject

	' Token: 0x0400175C RID: 5980
	<SerializeField()>
	Private trappedChalice As GameObject

	' Token: 0x0400175D RID: 5981
	<SerializeField()>
	Private trappedMugman As GameObject

	' Token: 0x0400175E RID: 5982
	<SerializeField()>
	Private trappedCuphead As GameObject

	' Token: 0x0400175F RID: 5983
	<SerializeField()>
	Private ghostBodyChalice As GameObject

	' Token: 0x04001760 RID: 5984
	<SerializeField()>
	Private ghostBodyCHMM As GameObject

	' Token: 0x04001761 RID: 5985
	<SerializeField()>
	Private shot1Animator As Animator

	' Token: 0x04001762 RID: 5986
	<SerializeField()>
	Private altText As GameObject()

	' Token: 0x04001763 RID: 5987
	Private screenShakeAmt As Single

	' Token: 0x04001764 RID: 5988
	Private advanceMusic As Boolean

	' Token: 0x04001765 RID: 5989
	<SerializeField()>
	<Range(-1F, 3F)>
	Private fastForward As Integer = -1
End Class
