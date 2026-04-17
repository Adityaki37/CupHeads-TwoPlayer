Imports System
Imports System.Collections
Imports UnityEngine
Imports UnityEngine.UI

' Token: 0x02000406 RID: 1030
Public Class IntroCutscene
	Inherits Cutscene

	' Token: 0x06000E51 RID: 3665 RVA: 0x0009299C File Offset: 0x00090D9C
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.book.SetActive(Localization.language = Localization.Languages.English)
		Me.bookLocalized.SetActive(Localization.language <> Localization.Languages.English)
		Me.input = New CupheadInput.AnyPlayerInput(False)
		CutsceneGUI.Current.pause.pauseAllowed = False
		MyBase.StartCoroutine(Me.main_cr())
		MyBase.StartCoroutine(Me.skip_cr())
	End Sub

	' Token: 0x06000E52 RID: 3666 RVA: 0x00092A10 File Offset: 0x00090E10
	Private Iterator Function main_cr() As IEnumerator
		Dim numScreens As Integer = 11
		Yield CupheadTime.WaitForSeconds(Me, 6F)
		For i As Integer = 0 To numScreens - 1
			Yield CupheadTime.WaitForSeconds(Me, 1.75F)
			Me.arrowVisible = True
			While Me.input.GetButtonDown(CupheadButton.Pause) OrElse Not Me.input.GetAnyButtonDown()
				Yield Nothing
			End While
			Me.arrowVisible = False
			MyBase.animator.SetTrigger("Continue")
			If i <> numScreens - 1 Then
				Me.NextPageSFX()
			End If
			If i = 2 Then
				Me.DevilLaugh()
			End If
			If i = 4 Then
				Me.DiceRoll()
			End If
			If i = 5 Then
				Me.DevilSlam()
			End If
			If i = 8 Then
				Me.DevilKick()
			End If
		Next
		AudioManager.FadeBGMVolume(0F, 0.75F, True)
		Yield CupheadTime.WaitForSeconds(Me, 0.75F)
		SceneLoader.LoadLevel(Levels.House, SceneLoader.Transition.Fade, SceneLoader.Icon.Hourglass, Nothing)
		Return
	End Function

	' Token: 0x06000E53 RID: 3667 RVA: 0x00092A2C File Offset: 0x00090E2C
	Private Iterator Function skip_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		While True
			If Me.input.GetButtonDown(CupheadButton.Pause) Then
				SceneLoader.LoadLevel(Levels.House, SceneLoader.Transition.Fade, SceneLoader.Icon.Hourglass, Nothing)
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000E54 RID: 3668 RVA: 0x00092A48 File Offset: 0x00090E48
	Private Sub Update()
		If Me.arrowVisible Then
			Me.arrowTransparency = Mathf.Clamp01(Me.arrowTransparency + Time.deltaTime / 0.25F)
		Else
			Me.arrowTransparency = 0F
		End If
		Me.arrow.color = New Color(1F, 1F, 1F, Me.arrowTransparency * 0.35F)
	End Sub

	' Token: 0x06000E55 RID: 3669 RVA: 0x00092AB8 File Offset: 0x00090EB8
	Private Sub NextPageSFX()
		AudioManager.Play("ui_confirm")
		AudioManager.Play("ui_pageturn")
	End Sub

	' Token: 0x06000E56 RID: 3670 RVA: 0x00092ACE File Offset: 0x00090ECE
	Private Sub DevilLaugh()
		AudioManager.Play("devil_laugh")
	End Sub

	' Token: 0x06000E57 RID: 3671 RVA: 0x00092ADA File Offset: 0x00090EDA
	Private Sub DiceRoll()
		AudioManager.Play("dice_roll")
	End Sub

	' Token: 0x06000E58 RID: 3672 RVA: 0x00092AE6 File Offset: 0x00090EE6
	Private Sub DevilSlam()
		AudioManager.Play("devil_slam")
	End Sub

	' Token: 0x06000E59 RID: 3673 RVA: 0x00092AF2 File Offset: 0x00090EF2
	Private Sub DevilKick()
		AudioManager.Play("devil_kick")
	End Sub

	' Token: 0x06000E5A RID: 3674 RVA: 0x00092AFE File Offset: 0x00090EFE
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.arrow = Nothing
	End Sub

	' Token: 0x0400179C RID: 6044
	Private input As CupheadInput.AnyPlayerInput

	' Token: 0x0400179D RID: 6045
	<SerializeField()>
	Private arrow As Image

	' Token: 0x0400179E RID: 6046
	<SerializeField()>
	Private book As GameObject

	' Token: 0x0400179F RID: 6047
	<SerializeField()>
	Private bookLocalized As GameObject

	' Token: 0x040017A0 RID: 6048
	Private arrowTransparency As Single

	' Token: 0x040017A1 RID: 6049
	Private arrowVisible As Boolean
End Class
