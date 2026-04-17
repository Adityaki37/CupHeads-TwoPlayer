Imports System
Imports System.Collections
Imports UnityEngine
Imports UnityEngine.UI

' Token: 0x02000408 RID: 1032
Public Class OutroCutscene
	Inherits Cutscene

	' Token: 0x06000E62 RID: 3682 RVA: 0x00093278 File Offset: 0x00091678
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.book.SetActive(Localization.language = Localization.Languages.English)
		Me.bookLocalized.SetActive(Localization.language <> Localization.Languages.English)
		CreditsScreen.goodEnding = True
		Me.input = New CupheadInput.AnyPlayerInput(False)
		CutsceneGUI.Current.pause.pauseAllowed = False
		MyBase.StartCoroutine(Me.main_cr())
		MyBase.StartCoroutine(Me.skip_cr())
	End Sub

	' Token: 0x06000E63 RID: 3683 RVA: 0x000932F0 File Offset: 0x000916F0
	Private Iterator Function main_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 0.33F)
		Dim numScreens As Integer = 6
		For i As Integer = 0 To numScreens - 1
			Yield CupheadTime.WaitForSeconds(Me, 1.75F)
			Me.arrowVisible = True
			While Not Me.input.GetAnyButtonDown()
				Yield Nothing
			End While
			Me.arrowVisible = False
			MyBase.animator.SetTrigger("Continue")
			If i <> 5 Then
				Me.NextPageSFX()
			End If
			If i = 0 Then
				Me.FireWhooshSFX()
			End If
			If i = 4 Then
				Me.Cheering()
			End If
		Next
		CreditsScreen.goodEnding = True
		Yield CupheadTime.WaitForSeconds(Me, 6.25F)
		AudioManager.FadeBGMVolume(0F, 3F, True)
		Yield CupheadTime.WaitForSeconds(Me, 3F)
		Cutscene.Load(Scenes.scene_title, Scenes.scene_cutscene_credits, SceneLoader.Transition.Fade, SceneLoader.Transition.Fade, SceneLoader.Icon.None)
		Return
	End Function

	' Token: 0x06000E64 RID: 3684 RVA: 0x0009330C File Offset: 0x0009170C
	Private Iterator Function skip_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		While True
			If Me.input.GetButtonDown(CupheadButton.Pause) Then
				Cutscene.Load(Scenes.scene_title, Scenes.scene_cutscene_credits, SceneLoader.Transition.Fade, SceneLoader.Transition.Fade, SceneLoader.Icon.None)
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000E65 RID: 3685 RVA: 0x00093328 File Offset: 0x00091728
	Private Sub Update()
		If Me.arrowVisible Then
			Me.arrowTransparency = Mathf.Clamp01(Me.arrowTransparency + Time.deltaTime / 0.25F)
		Else
			Me.arrowTransparency = 0F
		End If
		Me.arrow.color = New Color(1F, 1F, 1F, Me.arrowTransparency * 0.35F)
	End Sub

	' Token: 0x06000E66 RID: 3686 RVA: 0x00093398 File Offset: 0x00091798
	Private Sub NextPageSFX()
		AudioManager.Play("ui_confirm")
		AudioManager.Play("ui_pageturn")
	End Sub

	' Token: 0x06000E67 RID: 3687 RVA: 0x000933AE File Offset: 0x000917AE
	Private Sub FireWhooshSFX()
		AudioManager.Play("firewhoosh")
	End Sub

	' Token: 0x06000E68 RID: 3688 RVA: 0x000933BA File Offset: 0x000917BA
	Private Sub Cheering()
		AudioManager.Play("cheering")
	End Sub

	' Token: 0x06000E69 RID: 3689 RVA: 0x000933C6 File Offset: 0x000917C6
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.arrow = Nothing
	End Sub

	' Token: 0x06000E6A RID: 3690 RVA: 0x000933D5 File Offset: 0x000917D5
	Protected Overrides Sub SetRichPresence()
		OnlineManager.Instance.[Interface].SetRichPresence(PlayerId.Any, "Ending", True)
	End Sub

	' Token: 0x040017A6 RID: 6054
	Private input As CupheadInput.AnyPlayerInput

	' Token: 0x040017A7 RID: 6055
	<SerializeField()>
	Private arrow As Image

	' Token: 0x040017A8 RID: 6056
	<SerializeField()>
	Private book As GameObject

	' Token: 0x040017A9 RID: 6057
	<SerializeField()>
	Private bookLocalized As GameObject

	' Token: 0x040017AA RID: 6058
	Private arrowTransparency As Single

	' Token: 0x040017AB RID: 6059
	Private arrowVisible As Boolean
End Class
