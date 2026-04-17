Imports System
Imports System.Collections
Imports UnityEngine
Imports UnityEngine.UI

' Token: 0x0200099C RID: 2460
Public Class MapDLCUI
	Inherits AbstractMonoBehaviour

	' Token: 0x170004AD RID: 1197
	' (get) Token: 0x0600399A RID: 14746 RVA: 0x0020BAA7 File Offset: 0x00209EA7
	' (set) Token: 0x0600399B RID: 14747 RVA: 0x0020BAB0 File Offset: 0x00209EB0
	Private Property selection As Integer
		Get
			Return Me._selection
		End Get
		Set(value As Integer)
			Dim flag As Boolean = value > Me._selection
			Dim num As Integer = CInt(Mathf.Repeat(CSng(value), CSng(Me.menuItems.Length)))
			While Not Me.menuItems(num).gameObject.activeSelf
				num = If((Not flag), (num - 1), (num + 1))
				num = CInt(Mathf.Repeat(CSng(num), CSng(Me.menuItems.Length)))
			End While
			Me._selection = num
			Me.UpdateSelection()
		End Set
	End Property

	' Token: 0x0600399C RID: 14748 RVA: 0x0020BB26 File Offset: 0x00209F26
	Public Sub Init(checkIfDead As Boolean)
		Me.input = New CupheadInput.AnyPlayerInput(checkIfDead)
	End Sub

	' Token: 0x0600399D RID: 14749 RVA: 0x0020BB34 File Offset: 0x00209F34
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.canvasGroup = MyBase.GetComponent(Of CanvasGroup)()
		Me.canvasGroup.alpha = 0F
		Me.COLOR_SELECTED = Me.menuItems(0).color
		Me.COLOR_INACTIVE = Me.menuItems(Me.menuItems.Length - 1).color
	End Sub

	' Token: 0x0600399E RID: 14750 RVA: 0x0020BB92 File Offset: 0x00209F92
	Private Sub OnDestroy()
		PauseManager.Unpause()
	End Sub

	' Token: 0x0600399F RID: 14751 RVA: 0x0020BB9C File Offset: 0x00209F9C
	Private Sub Update()
		If Not Me.inputEnabled Then
			Return
		End If
		If Me.GetButtonDown(CupheadButton.Accept) Then
			Me.MenuSelectSound()
			Dim selection As Integer = Me.selection
			If selection <> 0 Then
				If selection = 1 Then
					Me.Close()
				End If
			Else
				Me.ExitToTitle()
			End If
			Return
		End If
		If Me._selectionTimer >= 0.15F Then
			If Me.GetButton(CupheadButton.MenuUp) Then
				Me.MenuMoveSound()
				Me.selection -= 1
			End If
			If Me.GetButton(CupheadButton.MenuDown) Then
				Me.MenuMoveSound()
				Me.selection += 1
			End If
		Else
			Me._selectionTimer += Time.deltaTime
		End If
	End Sub

	' Token: 0x060039A0 RID: 14752 RVA: 0x0020BC64 File Offset: 0x0020A064
	Private Sub UpdateVerticalSelection()
		Me._selectionTimer = 0F
		For i As Integer = 0 To Me.menuItems.Length - 1
			Dim text As Text = Me.menuItems(i)
			If i = Me.selection Then
				text.color = Me.COLOR_SELECTED
			Else
				text.color = Me.COLOR_INACTIVE
			End If
		Next
	End Sub

	' Token: 0x060039A1 RID: 14753 RVA: 0x0020BCC8 File Offset: 0x0020A0C8
	Private Sub UpdateSelection()
		Me._selectionTimer = 0F
		For i As Integer = 0 To Me.menuItems.Length - 1
			Dim text As Text = Me.menuItems(i)
			If i = Me.selection Then
				text.color = Me.COLOR_SELECTED
			Else
				text.color = Me.COLOR_INACTIVE
			End If
		Next
	End Sub

	' Token: 0x060039A2 RID: 14754 RVA: 0x0020BD2B File Offset: 0x0020A12B
	Private Sub ExitToTitle()
		PlayerManager.ResetPlayers()
		Dialoguer.EndDialogue()
		SceneLoader.LoadScene(Scenes.scene_title, SceneLoader.Transition.Fade, SceneLoader.Transition.Fade, SceneLoader.Icon.Hourglass, Nothing)
	End Sub

	' Token: 0x060039A3 RID: 14755 RVA: 0x0020BD41 File Offset: 0x0020A141
	Private Sub Close()
		Me.HideMenu()
	End Sub

	' Token: 0x060039A4 RID: 14756 RVA: 0x0020BD49 File Offset: 0x0020A149
	Public Sub ShowMenu()
		Me.visible = True
		Me.selection = 0
		Me.UpdateVerticalSelection()
		MyBase.StartCoroutine(Me.show_cr())
	End Sub

	' Token: 0x060039A5 RID: 14757 RVA: 0x0020BD6C File Offset: 0x0020A16C
	Private Iterator Function show_cr() As IEnumerator
		Dim t As Single = 0F
		While t < 0.2F
			Dim val As Single = t / 0.2F
			Me.canvasGroup.alpha = Mathf.Lerp(0F, 1F, val)
			t += Time.deltaTime
			Yield Nothing
		End While
		Me.canvasGroup.alpha = 1F
		Yield Nothing
		Me.Interactable()
		Return
	End Function

	' Token: 0x060039A6 RID: 14758 RVA: 0x0020BD87 File Offset: 0x0020A187
	Public Sub HideMenu()
		MyBase.StartCoroutine(Me.hide_cr())
		Me.canvasGroup.interactable = False
		Me.canvasGroup.blocksRaycasts = False
		Me.inputEnabled = False
	End Sub

	' Token: 0x060039A7 RID: 14759 RVA: 0x0020BDB8 File Offset: 0x0020A1B8
	Private Iterator Function hide_cr() As IEnumerator
		Dim t As Single = 0F
		While t < 0.2F
			Dim val As Single = t / 0.2F
			Me.canvasGroup.alpha = Mathf.Lerp(1F, 0F, val)
			t += Time.deltaTime
			Yield Nothing
		End While
		Me.canvasGroup.alpha = 0F
		Yield Nothing
		Me.selection = 0
		Me.visible = False
		Return
	End Function

	' Token: 0x060039A8 RID: 14760 RVA: 0x0020BDD3 File Offset: 0x0020A1D3
	Private Sub Interactable()
		Me.selection = 0
		Me.canvasGroup.interactable = True
		Me.canvasGroup.blocksRaycasts = True
		Me.inputEnabled = True
	End Sub

	' Token: 0x060039A9 RID: 14761 RVA: 0x0020BDFB File Offset: 0x0020A1FB
	Protected Function GetButtonDown(button As CupheadButton) As Boolean
		If Me.input.GetButtonDown(button) Then
			AudioManager.Play("level_menu_select")
			Return True
		End If
		Return False
	End Function

	' Token: 0x060039AA RID: 14762 RVA: 0x0020BE1B File Offset: 0x0020A21B
	Protected Function GetButton(button As CupheadButton) As Boolean
		Return Me.input.GetButton(button)
	End Function

	' Token: 0x060039AB RID: 14763 RVA: 0x0020BE31 File Offset: 0x0020A231
	Protected Sub MenuSelectSound()
		AudioManager.Play("level_menu_select")
	End Sub

	' Token: 0x060039AC RID: 14764 RVA: 0x0020BE3D File Offset: 0x0020A23D
	Protected Sub MenuMoveSound()
		AudioManager.Play("level_menu_move")
	End Sub

	' Token: 0x170004AE RID: 1198
	' (get) Token: 0x060039AD RID: 14765 RVA: 0x0020BE49 File Offset: 0x0020A249
	' (set) Token: 0x060039AE RID: 14766 RVA: 0x0020BE51 File Offset: 0x0020A251
	Public Property visible As Boolean

	' Token: 0x04004157 RID: 16727
	<SerializeField()>
	Private menuItems As Text()

	' Token: 0x04004158 RID: 16728
	Private _selectionTimer As Single

	' Token: 0x04004159 RID: 16729
	Private Const _SELECTION_TIME As Single = 0.15F

	' Token: 0x0400415A RID: 16730
	Private _selection As Integer

	' Token: 0x0400415B RID: 16731
	Private COLOR_SELECTED As Color

	' Token: 0x0400415C RID: 16732
	Private COLOR_INACTIVE As Color

	' Token: 0x0400415D RID: 16733
	Private inputEnabled As Boolean

	' Token: 0x0400415E RID: 16734
	Private input As CupheadInput.AnyPlayerInput

	' Token: 0x0400415F RID: 16735
	Private canvasGroup As CanvasGroup
End Class
