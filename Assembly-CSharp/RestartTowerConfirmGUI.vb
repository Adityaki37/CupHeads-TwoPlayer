Imports System
Imports System.Collections.Generic
Imports UnityEngine
Imports UnityEngine.UI

' Token: 0x02000468 RID: 1128
Public Class RestartTowerConfirmGUI
	Inherits AbstractMonoBehaviour

	' Token: 0x170002B3 RID: 691
	' (get) Token: 0x06001138 RID: 4408 RVA: 0x000A4247 File Offset: 0x000A2647
	' (set) Token: 0x06001139 RID: 4409 RVA: 0x000A424E File Offset: 0x000A264E
	Public Shared Property COLOR_SELECTED As Color

	' Token: 0x170002B4 RID: 692
	' (get) Token: 0x0600113A RID: 4410 RVA: 0x000A4256 File Offset: 0x000A2656
	' (set) Token: 0x0600113B RID: 4411 RVA: 0x000A425D File Offset: 0x000A265D
	Public Shared Property COLOR_INACTIVE As Color

	' Token: 0x170002B5 RID: 693
	' (get) Token: 0x0600113C RID: 4412 RVA: 0x000A4265 File Offset: 0x000A2665
	' (set) Token: 0x0600113D RID: 4413 RVA: 0x000A426D File Offset: 0x000A266D
	Public Property restartTowerConfirmMenuOpen As Boolean

	' Token: 0x170002B6 RID: 694
	' (get) Token: 0x0600113E RID: 4414 RVA: 0x000A4276 File Offset: 0x000A2676
	' (set) Token: 0x0600113F RID: 4415 RVA: 0x000A427E File Offset: 0x000A267E
	Public Property inputEnabled As Boolean

	' Token: 0x170002B7 RID: 695
	' (get) Token: 0x06001140 RID: 4416 RVA: 0x000A4287 File Offset: 0x000A2687
	' (set) Token: 0x06001141 RID: 4417 RVA: 0x000A4290 File Offset: 0x000A2690
	Private Property verticalSelection As Integer
		Get
			Return Me._verticalSelection
		End Get
		Set(value As Integer)
			Dim flag As Boolean = value > Me._verticalSelection
			Dim num As Integer = CInt(Mathf.Repeat(CSng(value), CSng(Me.currentItems.Count)))
			While Not Me.currentItems(num).text.gameObject.activeSelf
				num = If((Not flag), (num - 1), (num + 1))
				num = CInt(Mathf.Repeat(CSng(num), CSng(Me.currentItems.Count)))
			End While
			Me._verticalSelection = num
			Me.UpdateVerticalSelection()
		End Set
	End Property

	' Token: 0x170002B8 RID: 696
	' (get) Token: 0x06001142 RID: 4418 RVA: 0x000A4315 File Offset: 0x000A2715
	' (set) Token: 0x06001143 RID: 4419 RVA: 0x000A431D File Offset: 0x000A271D
	Public Property justClosed As Boolean

	' Token: 0x06001144 RID: 4420 RVA: 0x000A4328 File Offset: 0x000A2728
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.restartTowerConfirmMenuOpen = False
		Me.canvasGroup = MyBase.GetComponent(Of CanvasGroup)()
		Me.canvasGroup.alpha = 0F
		Me.currentItems = New List(Of RestartTowerConfirmGUI.Button)(Me.mainObjectButtons)
		RestartTowerConfirmGUI.COLOR_SELECTED = Me.currentItems(0).text.color
		RestartTowerConfirmGUI.COLOR_INACTIVE = Me.currentItems(Me.currentItems.Count - 1).text.color
	End Sub

	' Token: 0x06001145 RID: 4421 RVA: 0x000A43B1 File Offset: 0x000A27B1
	Public Sub Init(checkIfDead As Boolean)
		Me.input = New CupheadInput.AnyPlayerInput(checkIfDead)
	End Sub

	' Token: 0x06001146 RID: 4422 RVA: 0x000A43C0 File Offset: 0x000A27C0
	Private Sub Update()
		Me.justClosed = False
		If Not Me.inputEnabled Then
			Return
		End If
		If Me.GetButtonDown(CupheadButton.Pause) OrElse Me.GetButtonDown(CupheadButton.Cancel) Then
			Me.MenuSelectSound()
			Me.HideMenu()
			Return
		End If
		If Me.GetButtonDown(CupheadButton.Accept) Then
			Me.MenuSelectSound()
			Dim verticalSelection As Integer = Me.verticalSelection
			If verticalSelection <> 0 Then
				If verticalSelection = 1 Then
					Me.ToPauseMenu()
				End If
			Else
				Me.RestartTower()
			End If
			Return
		End If
		If Me._selectionTimer >= 0.15F Then
			If Me.GetButton(CupheadButton.MenuUp) Then
				Me.MenuSelectSound()
				Me.verticalSelection -= 1
			End If
			If Me.GetButton(CupheadButton.MenuDown) Then
				Me.MenuSelectSound()
				Me.verticalSelection += 1
			End If
		Else
			Me._selectionTimer += Time.deltaTime
		End If
	End Sub

	' Token: 0x06001147 RID: 4423 RVA: 0x000A44B4 File Offset: 0x000A28B4
	Private Sub UpdateVerticalSelection()
		Me._selectionTimer = 0F
		For i As Integer = 0 To Me.currentItems.Count - 1
			Dim button As RestartTowerConfirmGUI.Button = Me.currentItems(i)
			If i = Me.verticalSelection Then
				button.text.color = RestartTowerConfirmGUI.COLOR_SELECTED
			Else
				button.text.color = RestartTowerConfirmGUI.COLOR_INACTIVE
			End If
		Next
	End Sub

	' Token: 0x06001148 RID: 4424 RVA: 0x000A4526 File Offset: 0x000A2926
	Public Sub ShowMenu()
		Me.restartTowerConfirmMenuOpen = True
		Me.verticalSelection = 0
		Me.canvasGroup.alpha = 1F
		MyBase.FrameDelayedCallback(AddressOf Me.Interactable, 1)
		Me.UpdateVerticalSelection()
	End Sub

	' Token: 0x06001149 RID: 4425 RVA: 0x000A4560 File Offset: 0x000A2960
	Public Sub HideMenu()
		Me.verticalSelection = 0
		Me.canvasGroup.alpha = 0F
		Me.canvasGroup.interactable = False
		Me.canvasGroup.blocksRaycasts = False
		Me.inputEnabled = False
		Me.restartTowerConfirmMenuOpen = False
		Me.justClosed = True
	End Sub

	' Token: 0x0600114A RID: 4426 RVA: 0x000A45B1 File Offset: 0x000A29B1
	Private Sub Interactable()
		Me.verticalSelection = 0
		Me.canvasGroup.interactable = True
		Me.canvasGroup.blocksRaycasts = True
		Me.inputEnabled = True
	End Sub

	' Token: 0x0600114B RID: 4427 RVA: 0x000A45D9 File Offset: 0x000A29D9
	Protected Sub MenuSelectSound()
		AudioManager.Play("level_menu_select")
	End Sub

	' Token: 0x0600114C RID: 4428 RVA: 0x000A45E5 File Offset: 0x000A29E5
	Private Sub ToPauseMenu()
		Me.restartTowerConfirmMenuOpen = False
		Me.HideMenu()
	End Sub

	' Token: 0x0600114D RID: 4429 RVA: 0x000A45F4 File Offset: 0x000A29F4
	Private Sub RestartTower()
		PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerOne, False)
		PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerTwo, False)
		SceneLoader.ResetTheTowerOfPower()
		Dialoguer.EndDialogue()
	End Sub

	' Token: 0x0600114E RID: 4430 RVA: 0x000A460E File Offset: 0x000A2A0E
	Protected Function GetButtonDown(button As CupheadButton) As Boolean
		If Me.input.GetButtonDown(button) Then
			AudioManager.Play("level_menu_select")
			Return True
		End If
		Return False
	End Function

	' Token: 0x0600114F RID: 4431 RVA: 0x000A462E File Offset: 0x000A2A2E
	Protected Function GetButton(button As CupheadButton) As Boolean
		Return Me.input.GetButton(button)
	End Function

	' Token: 0x04001AB8 RID: 6840
	<SerializeField()>
	Private mainObject As GameObject

	' Token: 0x04001AB9 RID: 6841
	<SerializeField()>
	Private mainObjectButtons As RestartTowerConfirmGUI.Button()

	' Token: 0x04001ABA RID: 6842
	Private currentItems As List(Of RestartTowerConfirmGUI.Button)

	' Token: 0x04001ABB RID: 6843
	Private canvasGroup As CanvasGroup

	' Token: 0x04001ABC RID: 6844
	Private pauseMenu As AbstractPauseGUI

	' Token: 0x04001ABD RID: 6845
	Private _selectionTimer As Single

	' Token: 0x04001ABE RID: 6846
	Private Const _SELECTION_TIME As Single = 0.15F

	' Token: 0x04001ABF RID: 6847
	Private _verticalSelection As Integer

	' Token: 0x04001AC0 RID: 6848
	Private input As CupheadInput.AnyPlayerInput

	' Token: 0x04001AC1 RID: 6849
	Private lastIndex As Integer

	' Token: 0x02000469 RID: 1129
	<Serializable()>
	Public Class Button
		' Token: 0x04001AC3 RID: 6851
		Public text As Text
	End Class
End Class
