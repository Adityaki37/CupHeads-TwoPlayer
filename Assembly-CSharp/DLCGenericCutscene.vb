Imports System
Imports System.Collections
Imports UnityEngine
Imports UnityEngine.UI

' Token: 0x020003FE RID: 1022
Public Class DLCGenericCutscene
	Inherits Cutscene

	' Token: 0x06000E2B RID: 3627 RVA: 0x00090C2A File Offset: 0x0008F02A
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.input = New CupheadInput.AnyPlayerInput(False)
		CutsceneGUI.Current.pause.pauseAllowed = False
		MyBase.StartCoroutine(Me.main_cr())
		MyBase.StartCoroutine(Me.skip_cr())
	End Sub

	' Token: 0x06000E2C RID: 3628 RVA: 0x00090C68 File Offset: 0x0008F068
	Protected Overridable Sub Update()
		If Me.arrowVisible Then
			Me.arrowTransparency = Mathf.Clamp01(Me.arrowTransparency + Time.deltaTime / 0.25F)
		Else
			Me.arrowTransparency = 0F
		End If
		Me.arrow.color = New Color(1F, 1F, 1F, Me.arrowTransparency)
	End Sub

	' Token: 0x06000E2D RID: 3629 RVA: 0x00090CD2 File Offset: 0x0008F0D2
	Protected Overridable Sub OnScreenAdvance(which As Integer)
	End Sub

	' Token: 0x06000E2E RID: 3630 RVA: 0x00090CD4 File Offset: 0x0008F0D4
	Protected Overridable Sub OnContinue()
	End Sub

	' Token: 0x06000E2F RID: 3631 RVA: 0x00090CD6 File Offset: 0x0008F0D6
	Protected Overridable Sub OnScreenSkip()
	End Sub

	' Token: 0x06000E30 RID: 3632 RVA: 0x00090CD8 File Offset: 0x0008F0D8
	Private Iterator Function main_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, Me.mainDelay)
		Me.curScreen = 0
		While Me.curScreen < Me.screens.Length
			Me.screens(Me.curScreen).gameObject.SetActive(True)
			Dim target As Integer = Animator.StringToHash(Me.screens(Me.curScreen).GetLayerName(0) + ".End")
			While Me.screens(Me.curScreen).GetCurrentAnimatorStateInfo(0).fullPathHash <> target
				Yield Nothing
				If Me.arrowVisible Then
					While (Me.input.GetButtonDown(CupheadButton.Pause) OrElse Not Me.input.GetAnyButtonDown()) AndAlso Not Me.fastForwardActive
						Yield Nothing
					End While
					Me.curPathHash = Me.screens(Me.curScreen).GetCurrentAnimatorStateInfo(0).fullPathHash
					Me.screens(Me.curScreen).SetTrigger("Continue")
					Me.OnContinue()
					Me.text(Me.textCounter).SetActive(False)
					Me.arrowVisible = False
				ElseIf Me.allowScreenSkip AndAlso Me.input.GetAnyButtonDown() Then
					Me.OnScreenSkip()
				End If
			End While
			Me.OnScreenAdvance(Me.curScreen)
			If Me.curScreen < Me.screens.Length - 1 Then
				Me.screens(Me.curScreen).gameObject.SetActive(False)
			End If
			Me.arrowVisible = False
			Me.curScreen += 1
		End While
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		MyBase.Skip()
		Return
	End Function

	' Token: 0x06000E31 RID: 3633 RVA: 0x00090CF4 File Offset: 0x0008F0F4
	Public Sub IrisIn()
		Me.allowScreenSkip = False
		Dim component As Animator = Me.fader.GetComponent(Of Animator)()
		Dim color As Color = Me.fader.color
		color.a = 1F
		Me.fader.color = color
		component.SetTrigger("Iris_In")
	End Sub

	' Token: 0x06000E32 RID: 3634 RVA: 0x00090D44 File Offset: 0x0008F144
	Public Overridable Sub IrisOut()
		Dim component As Animator = Me.fader.GetComponent(Of Animator)()
		Dim color As Color = Me.fader.color
		color.a = 1F
		Me.fader.color = color
		component.SetTrigger("Iris_Out")
	End Sub

	' Token: 0x06000E33 RID: 3635 RVA: 0x00090D8C File Offset: 0x0008F18C
	Public Sub ShowText()
		Me.textCounter += 1
		Me.text(Me.textCounter).SetActive(True)
	End Sub

	' Token: 0x06000E34 RID: 3636 RVA: 0x00090DB0 File Offset: 0x0008F1B0
	Public Sub ShowArrow()
		If Me.curPathHash <> Me.screens(Me.curScreen).GetCurrentAnimatorStateInfo(0).fullPathHash Then
			Me.arrowVisible = True
		End If
	End Sub

	' Token: 0x06000E35 RID: 3637 RVA: 0x00090DEC File Offset: 0x0008F1EC
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

	' Token: 0x06000E36 RID: 3638 RVA: 0x00090E08 File Offset: 0x0008F208
	Protected Function DetectCharacter() As DLCGenericCutscene.TrappedChar
		If PlayerManager.Multiplayer Then
			If Not PlayerManager.playerWasChalice(0) AndAlso Not PlayerManager.playerWasChalice(1) Then
				Return DLCGenericCutscene.TrappedChar.Chalice
			End If
			If PlayerManager.playerWasChalice(0) Then
				Return If((Not PlayerManager.player1IsMugman), DLCGenericCutscene.TrappedChar.Cuphead, DLCGenericCutscene.TrappedChar.Mugman)
			End If
			Return If((Not PlayerManager.player1IsMugman), DLCGenericCutscene.TrappedChar.Mugman, DLCGenericCutscene.TrappedChar.Cuphead)
		Else
			If PlayerManager.playerWasChalice(0) Then
				Return If((Not PlayerManager.player1IsMugman), DLCGenericCutscene.TrappedChar.Cuphead, DLCGenericCutscene.TrappedChar.Mugman)
			End If
			Return If((PlayerData.Data.Loadouts.GetPlayerLoadout(PlayerId.PlayerTwo).charm <> Charm.charm_chalice), DLCGenericCutscene.TrappedChar.Chalice, If((Not PlayerManager.player1IsMugman), DLCGenericCutscene.TrappedChar.Mugman, DLCGenericCutscene.TrappedChar.Cuphead))
		End If
	End Function

	' Token: 0x04001766 RID: 5990
	Private input As CupheadInput.AnyPlayerInput

	' Token: 0x04001767 RID: 5991
	<SerializeField()>
	Private cursorToVisableTime As Single = 1.25F

	' Token: 0x04001768 RID: 5992
	<SerializeField()>
	Private mainDelay As Single = 0.25F

	' Token: 0x04001769 RID: 5993
	<SerializeField()>
	Private arrow As Image

	' Token: 0x0400176A RID: 5994
	<SerializeField()>
	Protected text As GameObject()

	' Token: 0x0400176B RID: 5995
	<SerializeField()>
	Protected screens As Animator()

	' Token: 0x0400176C RID: 5996
	Private activeScreen As Integer

	' Token: 0x0400176D RID: 5997
	Protected allowScreenSkip As Boolean

	' Token: 0x0400176E RID: 5998
	Private arrowTransparency As Single

	' Token: 0x0400176F RID: 5999
	Private arrowVisible As Boolean

	' Token: 0x04001770 RID: 6000
	Private textCounter As Integer = -1

	' Token: 0x04001771 RID: 6001
	Private curPathHash As Integer

	' Token: 0x04001772 RID: 6002
	Protected curScreen As Integer

	' Token: 0x04001773 RID: 6003
	Protected fastForwardActive As Boolean

	' Token: 0x04001774 RID: 6004
	Public fader As Image

	' Token: 0x020003FF RID: 1023
	Protected Enum TrappedChar
		' Token: 0x04001776 RID: 6006
		None = -1
		' Token: 0x04001777 RID: 6007
		Cuphead
		' Token: 0x04001778 RID: 6008
		Mugman
		' Token: 0x04001779 RID: 6009
		Chalice
	End Enum
End Class
