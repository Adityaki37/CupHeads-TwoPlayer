Imports System

' Token: 0x0200045D RID: 1117
Public Class InterruptingPrompt
	Inherits AbstractMonoBehaviour

	' Token: 0x060010F0 RID: 4336 RVA: 0x000A0C2C File Offset: 0x0009F02C
	Public Shared Sub SetCanInterrupt(canInterrupt As Boolean)
		If ControllerDisconnectedPrompt.Instance IsNot Nothing Then
			ControllerDisconnectedPrompt.Instance.allowedToShow = canInterrupt
		End If
	End Sub

	' Token: 0x060010F1 RID: 4337 RVA: 0x000A0C49 File Offset: 0x0009F049
	Public Shared Function IsInterrupting() As Boolean
		Return ControllerDisconnectedPrompt.Instance IsNot Nothing AndAlso ControllerDisconnectedPrompt.Instance.Visible
	End Function

	' Token: 0x060010F2 RID: 4338 RVA: 0x000A0C68 File Offset: 0x0009F068
	Public Shared Function CanInterrupt() As Boolean
		Return ControllerDisconnectedPrompt.Instance IsNot Nothing AndAlso ControllerDisconnectedPrompt.Instance.allowedToShow
	End Function

	' Token: 0x170002AA RID: 682
	' (get) Token: 0x060010F3 RID: 4339 RVA: 0x000A0C86 File Offset: 0x0009F086
	Public ReadOnly Property Visible As Boolean
		Get
			Return MyBase.gameObject.activeSelf
		End Get
	End Property

	' Token: 0x060010F4 RID: 4340 RVA: 0x000A0C93 File Offset: 0x0009F093
	Protected Overrides Sub Awake()
		MyBase.Awake()
		MyBase.gameObject.SetActive(False)
	End Sub

	' Token: 0x060010F5 RID: 4341 RVA: 0x000A0CA7 File Offset: 0x0009F0A7
	Public Sub Show()
		MyBase.gameObject.SetActive(True)
		Me.wasPausedBeforeInterrupt = PauseManager.state = PauseManager.State.Paused
		If Not Me.wasPausedBeforeInterrupt Then
			PauseManager.Pause()
		End If
	End Sub

	' Token: 0x060010F6 RID: 4342 RVA: 0x000A0CD3 File Offset: 0x0009F0D3
	Public Sub Dismiss()
		MyBase.gameObject.SetActive(False)
		If Not Me.wasPausedBeforeInterrupt Then
			PauseManager.Unpause()
		End If
	End Sub

	' Token: 0x04001A5C RID: 6748
	Private wasPausedBeforeInterrupt As Boolean
End Class
