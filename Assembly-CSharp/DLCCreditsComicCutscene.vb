Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020003F9 RID: 1017
Public Class DLCCreditsComicCutscene
	Inherits Cutscene

	' Token: 0x06000DE5 RID: 3557 RVA: 0x000903F9 File Offset: 0x0008E7F9
	Protected Overrides Sub Start()
		MyBase.Start()
		CutsceneGUI.Current.pause.pauseAllowed = False
		Me.input = New CupheadInput.AnyPlayerInput(False)
		MyBase.StartCoroutine(Me.credits_cr())
	End Sub

	' Token: 0x06000DE6 RID: 3558 RVA: 0x0009042C File Offset: 0x0008E82C
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

	' Token: 0x06000DE7 RID: 3559 RVA: 0x00090514 File Offset: 0x0008E914
	Private Iterator Function credits_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 3F)
		Me.canSkip = True
		AudioManager.PlayBGM()
		Dim distance As Single = Me.panels.GetLast().transform.position.x - Me.panels(0).transform.position.x - DLCCreditsComicCutscene.EndingAdjustment
		Dim elapsedTime As Single = 0F
		While elapsedTime < DLCCreditsComicCutscene.ScrollDuration
			Yield Nothing
			elapsedTime += CupheadTime.Delta * Me.multiplier
			Dim position As Vector3 = Me.parentTransform.position
			position.x = Mathf.Lerp(0F, -distance, elapsedTime / DLCCreditsComicCutscene.ScrollDuration)
			Me.parentTransform.position = position
		End While
		Yield CupheadTime.WaitForSeconds(Me, 5F)
		Me.canSkip = False
		Me.goToNext()
		Return
	End Function

	' Token: 0x06000DE8 RID: 3560 RVA: 0x0009052F File Offset: 0x0008E92F
	Private Sub goToNext()
		SceneLoader.LoadScene(Scenes.scene_cutscene_dlc_credits, SceneLoader.Transition.Fade, SceneLoader.Transition.Fade, SceneLoader.Icon.None, Nothing)
	End Sub

	' Token: 0x04001745 RID: 5957
	Private Shared AdjustmentAmount As Single = -1F

	' Token: 0x04001746 RID: 5958
	Private Shared EndingAdjustment As Single = 15F

	' Token: 0x04001747 RID: 5959
	Private Shared ScrollDuration As Single = 90.8F

	' Token: 0x04001748 RID: 5960
	<SerializeField()>
	Private parentTransform As Transform

	' Token: 0x04001749 RID: 5961
	<SerializeField()>
	Private panels As SpriteRenderer()

	' Token: 0x0400174A RID: 5962
	Private input As CupheadInput.AnyPlayerInput

	' Token: 0x0400174B RID: 5963
	Private canSkip As Boolean

	' Token: 0x0400174C RID: 5964
	Private multiplier As Single = 1F
End Class
