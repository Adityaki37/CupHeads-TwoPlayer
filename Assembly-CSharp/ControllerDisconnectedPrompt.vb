Imports System
Imports UnityEngine
Imports UnityEngine.UI

' Token: 0x02000457 RID: 1111
Public Class ControllerDisconnectedPrompt
	Inherits InterruptingPrompt

	' Token: 0x060010CA RID: 4298 RVA: 0x000A0CF9 File Offset: 0x0009F0F9
	Protected Overrides Sub Awake()
		MyBase.Awake()
		ControllerDisconnectedPrompt.Instance = Me
	End Sub

	' Token: 0x060010CB RID: 4299 RVA: 0x000A0D07 File Offset: 0x0009F107
	Public Sub Show(player As PlayerId)
		Me.currentPlayer = player
		Me.localizationHelper.currentID = Localization.Find(If((player <> PlayerId.PlayerOne), "XboxPlayer2", "XboxPlayer1")).id
		PlayerManager.OnDisconnectPromptDisplayed(player)
		MyBase.Show()
	End Sub

	' Token: 0x060010CC RID: 4300 RVA: 0x000A0D46 File Offset: 0x0009F146
	Private Sub Update()
		If MyBase.Visible AndAlso Not PlayerManager.IsControllerDisconnected(Me.currentPlayer, True) Then
			MyBase.FrameDelayedCallback(AddressOf MyBase.Dismiss, 2)
		End If
	End Sub

	' Token: 0x060010CD RID: 4301 RVA: 0x000A0D78 File Offset: 0x0009F178
	Private Sub OnDestroy()
		ControllerDisconnectedPrompt.Instance = Nothing
	End Sub

	' Token: 0x04001A12 RID: 6674
	Public Shared Instance As ControllerDisconnectedPrompt

	' Token: 0x04001A13 RID: 6675
	Public currentPlayer As PlayerId

	' Token: 0x04001A14 RID: 6676
	Public allowedToShow As Boolean

	' Token: 0x04001A15 RID: 6677
	<SerializeField()>
	Private playerText As Text

	' Token: 0x04001A16 RID: 6678
	<SerializeField()>
	Private localizationHelper As LocalizationHelper
End Class
