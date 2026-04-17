Imports System
Imports System.Collections
Imports UnityEngine
Imports UnityEngine.UI

' Token: 0x020003FA RID: 1018
Public Class DLCCreditsCutscene
	Inherits Cutscene

	' Token: 0x06000DEB RID: 3563 RVA: 0x0009075A File Offset: 0x0008EB5A
	Protected Overrides Sub Start()
		MyBase.Start()
		CutsceneGUI.Current.pause.pauseAllowed = False
		Me.input = New CupheadInput.AnyPlayerInput(False)
		MyBase.StartCoroutine(Me.credits_cr())
	End Sub

	' Token: 0x06000DEC RID: 3564 RVA: 0x0009078C File Offset: 0x0008EB8C
	Private Sub Update()
		If Me.canSkip Then
			If Me.input.GetButtonDown(CupheadButton.Pause) Then
				Me.canSkip = False
				Me.StopAllCoroutines()
				Me.goToNext()
				Return
			End If
			If Me.input.GetAnyButtonHeld() AndAlso Not Me.input.GetButtonDown(CupheadButton.Pause) Then
				If Me.multiplier = 1F Then
					Me.multiplier = 8F
					AudioManager.ChangeBGMPitch(8F, 0.125F)
				End If
			ElseIf Me.multiplier > 1F Then
				Me.multiplier = 1F
				AudioManager.ChangeBGMPitch(1F, 0.125F)
			End If
		ElseIf Me.multiplier > 1F Then
			Me.multiplier = 1F
			AudioManager.ChangeBGMPitch(1F, 0.125F)
		End If
	End Sub

	' Token: 0x06000DED RID: 3565 RVA: 0x00090874 File Offset: 0x0008EC74
	Private Iterator Function credits_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 3F)
		AudioManager.PlayBGM()
		Me.canSkip = True
		Dim preferredHeight As Single = Me.contentTransform.GetComponent(Of VerticalLayoutGroup)().preferredHeight
		Dim speed As Single = preferredHeight / Me.scrollDuration
		Dim elapsedTime As Single = 0F
		Dim accumulator As Single = 0F
		While elapsedTime < Me.scrollDuration
			Yield Nothing
			accumulator += CupheadTime.Delta * Me.multiplier
			While accumulator > 0.041666668F
				elapsedTime += 0.041666668F
				accumulator -= 0.041666668F
			End While
			Dim position As Vector2 = Me.contentTransform.anchoredPosition
			position.y = Mathf.Lerp(0F, preferredHeight - 720F, elapsedTime / Me.scrollDuration)
			Me.contentTransform.anchoredPosition = position
		End While
		Me.canSkip = False
		Yield CupheadTime.WaitForSeconds(Me, 3F)
		Me.goToNext()
		Return
	End Function

	' Token: 0x06000DEE RID: 3566 RVA: 0x0009088F File Offset: 0x0008EC8F
	Private Sub goToNext()
		PlayerManager.ResetPlayers()
		SceneLoader.LoadScene(Scenes.scene_title, SceneLoader.Transition.Fade, SceneLoader.Transition.Fade, SceneLoader.Icon.None, Nothing)
	End Sub

	' Token: 0x0400174D RID: 5965
	<SerializeField()>
	Private scrollDuration As Single

	' Token: 0x0400174E RID: 5966
	<SerializeField()>
	Private contentTransform As RectTransform

	' Token: 0x0400174F RID: 5967
	<SerializeField()>
	Private memphisFontSize As Single

	' Token: 0x04001750 RID: 5968
	<SerializeField()>
	Private vogueBoldFontSize As Single

	' Token: 0x04001751 RID: 5969
	<SerializeField()>
	Private vogueExtraBoldFontSize As Single

	' Token: 0x04001752 RID: 5970
	Private input As CupheadInput.AnyPlayerInput

	' Token: 0x04001753 RID: 5971
	Private canSkip As Boolean

	' Token: 0x04001754 RID: 5972
	Private multiplier As Single = 1F
End Class
