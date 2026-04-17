Imports System
Imports System.Collections
Imports UnityEngine
Imports UnityEngine.UI

' Token: 0x020003F3 RID: 1011
Public Class CreditsScreen
	Inherits AbstractMonoBehaviour

	' Token: 0x06000DB4 RID: 3508 RVA: 0x0008EF51 File Offset: 0x0008D351
	Private Sub Start()
		Me.Init(False)
	End Sub

	' Token: 0x06000DB5 RID: 3509 RVA: 0x0008EF5C File Offset: 0x0008D35C
	Public Sub Init(checkIfDead As Boolean)
		Me.input = New CupheadInput.AnyPlayerInput(False)
		Me.verticalLayoutGroup = Me.content.GetComponent(Of VerticalLayoutGroup)()
		' Fix overlapping credits text by increasing spacing
		Me.verticalLayoutGroup.spacing = 15F
		Me.verticalLayoutGroup.childForceExpandHeight = False
		MyBase.StartCoroutine(Me.credits_cr())
		MyBase.StartCoroutine(Me.skip_cr())
		MyBase.StartCoroutine(Me.fastForward_cr())
	End Sub

	' Token: 0x06000DB6 RID: 3510 RVA: 0x0008EFB0 File Offset: 0x0008D3B0
	Private Iterator Function credits_cr() As IEnumerator
		Dim wait As Single = Me.introDuration
		While wait > 0F
			wait -= CupheadTime.Delta * Me.timeMultiplier
			Yield Nothing
		End While
		Dim accumulator As Single = 0F
		While Me.content.anchoredPosition.y < Me.verticalLayoutGroup.preferredHeight - MyBase.rectTransform.sizeDelta.y
			accumulator += CupheadTime.Delta * Me.timeMultiplier
			While accumulator > 0.041666668F
				accumulator -= 0.041666668F
				Me.content.anchoredPosition = New Vector2(0F, Me.content.anchoredPosition.y + Me.scrollSpeed * 0.041666668F)
			End While
			Yield Nothing
		End While
		Me.doneScrolling = True
		wait = Me.outroDuration
		While wait > 0F
			wait -= CupheadTime.Delta * Me.timeMultiplier
			Yield Nothing
		End While
		PlayerManager.ResetPlayers()
		SceneLoader.LoadScene(Scenes.scene_title, SceneLoader.Transition.Iris, SceneLoader.Transition.Fade, SceneLoader.Icon.None, Nothing)
		Return
	End Function

	' Token: 0x06000DB7 RID: 3511 RVA: 0x0008EFCC File Offset: 0x0008D3CC
	Private Iterator Function fastForward_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		While True
			If Me.input.GetAnyButtonHeld() AndAlso Not Me.input.GetButtonDown(CupheadButton.Pause) AndAlso Not Me.doneScrolling Then
				If Me.timeMultiplier = 1F Then
					Me.timeMultiplier = 8F
					AudioManager.ChangeBGMPitch(8F, 0.125F)
				End If
			ElseIf Me.timeMultiplier > 1F Then
				Me.timeMultiplier = 1F
				AudioManager.ChangeBGMPitch(1F, 0.125F)
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000DB8 RID: 3512 RVA: 0x0008EFE8 File Offset: 0x0008D3E8
	Private Iterator Function skip_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		While True
			If Me.input.GetButtonDown(CupheadButton.Pause) Then
				PlayerManager.ResetPlayers()
				SceneLoader.LoadScene(Scenes.scene_title, SceneLoader.Transition.Iris, SceneLoader.Transition.Fade, SceneLoader.Icon.None, Nothing)
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000DB9 RID: 3513 RVA: 0x0008F003 File Offset: 0x0008D403
	Private Sub LateUpdate()
		If CupheadMapCamera.Current Is Nothing Then
			Return
		End If
		MyBase.transform.position = CupheadMapCamera.Current.transform.position
	End Sub

	' Token: 0x06000DBA RID: 3514 RVA: 0x0008F030 File Offset: 0x0008D430
	Protected Function GetButtonDown(button As CupheadButton) As Boolean
		Return Me.input.GetButtonDown(button)
	End Function

	' Token: 0x04001724 RID: 5924
	Public Shared goodEnding As Boolean = True

	' Token: 0x04001725 RID: 5925
	<SerializeField()>
	Private content As RectTransform

	' Token: 0x04001726 RID: 5926
	Private verticalLayoutGroup As VerticalLayoutGroup

	' Token: 0x04001727 RID: 5927
	<SerializeField()>
	Private introDuration As Single

	' Token: 0x04001728 RID: 5928
	<SerializeField()>
	Private scrollSpeed As Single

	' Token: 0x04001729 RID: 5929
	<SerializeField()>
	Private outroDuration As Single

	' Token: 0x0400172A RID: 5930
	Private input As CupheadInput.AnyPlayerInput

	' Token: 0x0400172B RID: 5931
	Private doneScrolling As Boolean

	' Token: 0x0400172C RID: 5932
	Private timeMultiplier As Single = 1F
End Class
