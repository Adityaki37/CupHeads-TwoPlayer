Imports System
Imports System.Collections
Imports UnityEngine
Imports UnityEngine.UI

' Token: 0x02000404 RID: 1028
Public Class GenericCutscene
	Inherits Cutscene

	' Token: 0x06000E48 RID: 3656 RVA: 0x00092565 File Offset: 0x00090965
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.input = New CupheadInput.AnyPlayerInput(False)
		CutsceneGUI.Current.pause.pauseAllowed = False
		MyBase.StartCoroutine(Me.main_cr())
		MyBase.StartCoroutine(Me.skip_cr())
	End Sub

	' Token: 0x06000E49 RID: 3657 RVA: 0x000925A4 File Offset: 0x000909A4
	Private Iterator Function main_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 0.25F)
		For i As Integer = 0 To Me.numScreens - 1
			If Me.specialCase AndAlso i = 3 Then
				Yield CupheadTime.WaitForSeconds(Me, 2.25F)
			Else
				Yield CupheadTime.WaitForSeconds(Me, 1.25F)
			End If
			Me.arrowVisible = True
			While Me.input.GetButtonDown(CupheadButton.Pause) OrElse Not Me.input.GetAnyButtonDown()
				Yield Nothing
			End While
			Me.arrowVisible = False
			If i < Me.numScreens - 1 Then
				MyBase.animator.SetTrigger("Continue")
			End If
			AudioManager.Play("ui_confirm_generic")
		Next
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		MyBase.Skip()
		Return
	End Function

	' Token: 0x06000E4A RID: 3658 RVA: 0x000925C0 File Offset: 0x000909C0
	Private Iterator Function skip_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		While True
			If Me.input.GetButtonDown(CupheadButton.Pause) Then
				MyBase.Skip()
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000E4B RID: 3659 RVA: 0x000925DC File Offset: 0x000909DC
	Private Sub Update()
		If Me.arrowVisible Then
			Me.arrowTransparency = Mathf.Clamp01(Me.arrowTransparency + Time.deltaTime / 0.25F)
		Else
			Me.arrowTransparency = 0F
		End If
		Me.arrow.color = New Color(1F, 1F, 1F, Me.arrowTransparency)
	End Sub

	' Token: 0x06000E4C RID: 3660 RVA: 0x00092646 File Offset: 0x00090A46
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.arrow = Nothing
	End Sub

	' Token: 0x04001794 RID: 6036
	Private input As CupheadInput.AnyPlayerInput

	' Token: 0x04001795 RID: 6037
	<SerializeField()>
	Private specialCase As Boolean

	' Token: 0x04001796 RID: 6038
	<SerializeField()>
	Private arrow As Image

	' Token: 0x04001797 RID: 6039
	<SerializeField()>
	Private numScreens As Integer

	' Token: 0x04001798 RID: 6040
	Private arrowTransparency As Single

	' Token: 0x04001799 RID: 6041
	Private arrowVisible As Boolean
End Class
