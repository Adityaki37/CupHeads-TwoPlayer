Imports System
Imports System.Collections
Imports UnityEngine
Imports UnityEngine.UI

' Token: 0x0200045B RID: 1115
Public Class DLCGUI
	Inherits AbstractMonoBehaviour

	' Token: 0x170002A6 RID: 678
	' (get) Token: 0x060010DC RID: 4316 RVA: 0x000A1D82 File Offset: 0x000A0182
	' (set) Token: 0x060010DD RID: 4317 RVA: 0x000A1D8A File Offset: 0x000A018A
	Public Property dlcMenuOpen As Boolean

	' Token: 0x170002A7 RID: 679
	' (get) Token: 0x060010DE RID: 4318 RVA: 0x000A1D93 File Offset: 0x000A0193
	' (set) Token: 0x060010DF RID: 4319 RVA: 0x000A1D9B File Offset: 0x000A019B
	Public Property inputEnabled As Boolean

	' Token: 0x170002A8 RID: 680
	' (get) Token: 0x060010E0 RID: 4320 RVA: 0x000A1DA4 File Offset: 0x000A01A4
	' (set) Token: 0x060010E1 RID: 4321 RVA: 0x000A1DAC File Offset: 0x000A01AC
	Public Property justClosed As Boolean

	' Token: 0x060010E2 RID: 4322 RVA: 0x000A1DB5 File Offset: 0x000A01B5
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.dlcMenuOpen = False
		Me.canvasGroup = MyBase.GetComponent(Of CanvasGroup)()
		Me.canvasGroup.alpha = 0F
	End Sub

	' Token: 0x060010E3 RID: 4323 RVA: 0x000A1DE0 File Offset: 0x000A01E0
	Public Sub Init(checkIfDead As Boolean)
		Me.input = New CupheadInput.AnyPlayerInput(checkIfDead)
	End Sub

	' Token: 0x060010E4 RID: 4324 RVA: 0x000A1DF0 File Offset: 0x000A01F0
	Private Sub Update()
		Me.justClosed = False
		Me.timeSinceStart += Time.deltaTime
		Me.timeSinceConfirmPressed += Time.deltaTime
		If Me.timeSinceStart < 0.25F Then
			Return
		End If
		If Not Me.inputEnabled Then
			Return
		End If
		If Me.GetButtonDown(CupheadButton.Cancel) Then
			MyBase.StartCoroutine(Me.hide_cr())
			Return
		End If
		If Not Me.dlcEnabled AndAlso Me.timeSinceConfirmPressed >= 0.5F AndAlso DLCManager.CanRedirectToStore() AndAlso Me.GetButtonDown(CupheadButton.Accept) Then
			Me.timeSinceConfirmPressed = 0F
			DLCManager.LaunchStore()
			Return
		End If
	End Sub

	' Token: 0x060010E5 RID: 4325 RVA: 0x000A1EA4 File Offset: 0x000A02A4
	Public Sub ShowDLCMenu()
		Me.dlcEnabled = DLCManager.DLCEnabled()
		Me.timeSinceStart = 0F
		Me.timeSinceConfirmPressed = 0F
		Me.dlcMenuOpen = True
		Me.canvasGroup.alpha = 1F
		MyBase.StartCoroutine(Me.show_cr())
	End Sub

	' Token: 0x060010E6 RID: 4326 RVA: 0x000A1EF6 File Offset: 0x000A02F6
	Private Sub hideDLCMenu()
		Me.canvasGroup.alpha = 0F
		Me.canvasGroup.interactable = False
		Me.canvasGroup.blocksRaycasts = False
		Me.inputEnabled = False
		Me.dlcMenuOpen = False
		Me.justClosed = True
	End Sub

	' Token: 0x060010E7 RID: 4327 RVA: 0x000A1F35 File Offset: 0x000A0335
	Private Sub interactable()
		Me.canvasGroup.interactable = True
		Me.canvasGroup.blocksRaycasts = True
		Me.inputEnabled = True
	End Sub

	' Token: 0x060010E8 RID: 4328 RVA: 0x000A1F58 File Offset: 0x000A0358
	Private Iterator Function show_cr() As IEnumerator
		Dim scaler As Transform
		Dim text As Text
		If Me.dlcEnabled Then
			scaler = Me.installedScaler
			Me.notInstalled.SetActive(False)
			Me.installed.SetActive(True)
			text = Me.installedText
		Else
			scaler = Me.notInstalledScaler
			Me.notInstalled.SetActive(True)
			Me.installed.SetActive(False)
			text = Me.notInstalledText
		End If
		Me.fader.color = New Color(0F, 0F, 0F, DLCGUI.FaderAlpha)
		Dim fadeImages As Image() = scaler.GetComponentsInChildren(Of Image)()
		For Each image As Image In fadeImages
			Dim color2 As Color = image.color
			color2.a = 1F
			image.color = color2
		Next
		Dim elapsedTime As Single = 0F
		While elapsedTime < 0.4F
			elapsedTime += CupheadTime.Delta
			Dim scale As Vector3 = scaler.localScale
			Dim num As Single = EaseUtils.EaseOutCubic(2F, 1F, elapsedTime / 0.4F)
			Dim num2 As Single = num
			scale.y = num
			scale.x = num2
			scaler.localScale = scale
			Dim color As Color = text.color
			color.a = Mathf.Lerp(0F, 1F, elapsedTime / 0.4F)
			text.color = color
			Yield Nothing
		End While
		Me.interactable()
		Return
	End Function

	' Token: 0x060010E9 RID: 4329 RVA: 0x000A1F74 File Offset: 0x000A0374
	Private Iterator Function hide_cr() As IEnumerator
		Me.canvasGroup.interactable = False
		Me.canvasGroup.blocksRaycasts = False
		Me.inputEnabled = False
		Dim scaler As Transform
		Dim text As Text
		If Me.dlcEnabled Then
			scaler = Me.installedScaler
			Me.notInstalled.SetActive(False)
			Me.installed.SetActive(True)
			text = Me.installedText
		Else
			scaler = Me.notInstalledScaler
			Me.notInstalled.SetActive(True)
			Me.installed.SetActive(False)
			text = Me.notInstalledText
		End If
		Dim fadeImages As Image() = scaler.GetComponentsInChildren(Of Image)()
		Dim elapsedTime As Single = 0F
		While elapsedTime < 0.2F
			elapsedTime += CupheadTime.Delta
			Dim scale As Vector3 = scaler.localScale
			Dim num As Single = EaseUtils.EaseInCubic(1F, 2F, elapsedTime / 0.2F)
			Dim num2 As Single = num
			scale.y = num
			scale.x = num2
			scaler.localScale = scale
			Dim color As Color = text.color
			color.a = Mathf.Lerp(1F, 0F, elapsedTime / 0.2F)
			text.color = color
			For Each image As Image In fadeImages
				color = image.color
				color.a = Mathf.Lerp(1F, 0F, elapsedTime / 0.2F)
				image.color = color
			Next
			color = Me.fader.color
			color.a = Mathf.Lerp(DLCGUI.FaderAlpha, 0F, elapsedTime / 0.2F)
			Me.fader.color = color
			Yield Nothing
		End While
		Me.hideDLCMenu()
		Return
	End Function

	' Token: 0x060010EA RID: 4330 RVA: 0x000A1F8F File Offset: 0x000A038F
	Protected Function GetButtonDown(button As CupheadButton) As Boolean
		If Me.input.GetButtonDown(button) Then
			AudioManager.Play("level_menu_select")
			Return True
		End If
		Return False
	End Function

	' Token: 0x04001A48 RID: 6728
	Private Shared FaderAlpha As Single = 0.5F

	' Token: 0x04001A49 RID: 6729
	<SerializeField()>
	Private notInstalled As GameObject

	' Token: 0x04001A4A RID: 6730
	<SerializeField()>
	Private installed As GameObject

	' Token: 0x04001A4B RID: 6731
	<SerializeField()>
	Private notInstalledScaler As Transform

	' Token: 0x04001A4C RID: 6732
	<SerializeField()>
	Private installedScaler As Transform

	' Token: 0x04001A4D RID: 6733
	<SerializeField()>
	Private fader As Image

	' Token: 0x04001A4E RID: 6734
	<SerializeField()>
	Private notInstalledText As Text

	' Token: 0x04001A4F RID: 6735
	<SerializeField()>
	Private installedText As Text

	' Token: 0x04001A50 RID: 6736
	Private canvasGroup As CanvasGroup

	' Token: 0x04001A51 RID: 6737
	Private input As CupheadInput.AnyPlayerInput

	' Token: 0x04001A52 RID: 6738
	Private timeSinceStart As Single

	' Token: 0x04001A53 RID: 6739
	Private timeSinceConfirmPressed As Single

	' Token: 0x04001A54 RID: 6740
	Private dlcEnabled As Boolean
End Class
