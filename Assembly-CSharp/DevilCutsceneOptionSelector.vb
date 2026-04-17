Imports System
Imports System.Collections
Imports UnityEngine
Imports UnityEngine.UI

' Token: 0x020003F8 RID: 1016
Public Class DevilCutsceneOptionSelector
	Inherits AbstractMonoBehaviour

	' Token: 0x06000DDF RID: 3551 RVA: 0x0008FF5F File Offset: 0x0008E35F
	Private Sub Start()
		Me.input = New CupheadInput.AnyPlayerInput(False)
		Me.cursor.gameObject.SetActive(False)
	End Sub

	' Token: 0x06000DE0 RID: 3552 RVA: 0x0008FF7E File Offset: 0x0008E37E
	Public Sub Show()
		MyBase.StartCoroutine(Me.main_cr())
		MyBase.StartCoroutine(Me.fadeIn_cr())
	End Sub

	' Token: 0x06000DE1 RID: 3553 RVA: 0x0008FF9C File Offset: 0x0008E39C
	Private Iterator Function main_cr() As IEnumerator
		Me.cursor.gameObject.SetActive(True)
		If Me.cutscene.IsTranslationTextActive() Then
			Me.cursor.transform.position = Me.options(Me.currentOption).position
		Else
			Me.cursor.transform.position = Me.bakedOptions(Me.currentOption).position
		End If
		While True
			Dim prevOption As Integer = Me.currentOption
			If Me.input.GetButtonDown(CupheadButton.MenuLeft) Then
				Me.currentOption = Mathf.Max(0, Me.currentOption - 1)
			End If
			If Me.input.GetButtonDown(CupheadButton.MenuRight) Then
				Me.currentOption = Mathf.Min(Me.options.Length - 1, Me.currentOption + 1)
			End If
			If Me.cutscene.IsTranslationTextActive() Then
				Me.cursor.transform.position = Me.options(Me.currentOption).position
			Else
				Me.cursor.transform.position = Me.bakedOptions(Me.currentOption).position
			End If
			If Me.currentOption > prevOption Then
				Me.ToggleSFX()
				MyBase.animator.SetTrigger("MoveRight")
			End If
			If Me.currentOption < prevOption Then
				Me.ToggleSFX()
				MyBase.animator.SetTrigger("MoveLeft")
			End If
			If Me.input.GetButtonDown(CupheadButton.Accept) Then
				Exit For
			End If
			Yield Nothing
		End While
		If Me.currentOption = 0 Then
			Me.cutscene.RefuseDevil()
		Else
			Me.cutscene.JoinDevil()
		End If
		Me.cursor.gameObject.SetActive(False)
		Return
		Return
	End Function

	' Token: 0x06000DE2 RID: 3554 RVA: 0x0008FFB8 File Offset: 0x0008E3B8
	Private Iterator Function fadeIn_cr() As IEnumerator
		Dim t As Single = 0F
		While t < 0.75F
			Me.cursorImage.color = New Color(1F, 1F, 1F, t / 0.75F)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		Me.cursorImage.color = New Color(1F, 1F, 1F, 1F)
		Return
	End Function

	' Token: 0x06000DE3 RID: 3555 RVA: 0x0008FFD3 File Offset: 0x0008E3D3
	Private Sub ToggleSFX()
		AudioManager.Play("ui_toggle")
	End Sub

	' Token: 0x0400173E RID: 5950
	Public bakedOptions As Transform()

	' Token: 0x0400173F RID: 5951
	Public options As Transform()

	' Token: 0x04001740 RID: 5952
	Public cursor As Transform

	' Token: 0x04001741 RID: 5953
	Public cutscene As DevilCutscene

	' Token: 0x04001742 RID: 5954
	Private currentOption As Integer

	' Token: 0x04001743 RID: 5955
	Private input As CupheadInput.AnyPlayerInput

	' Token: 0x04001744 RID: 5956
	Public cursorImage As Image
End Class
