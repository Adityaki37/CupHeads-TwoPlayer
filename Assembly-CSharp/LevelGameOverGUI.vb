Imports System
Imports System.Collections
Imports RektTransform
Imports TMPro
Imports UnityEngine
Imports UnityEngine.UI

' Token: 0x0200047E RID: 1150
<RequireComponent(GetType(CanvasGroup))>
Public Class LevelGameOverGUI
	Inherits AbstractMonoBehaviour

	' Token: 0x170002C3 RID: 707
	' (get) Token: 0x060011B5 RID: 4533 RVA: 0x000A60BE File Offset: 0x000A44BE
	' (set) Token: 0x060011B6 RID: 4534 RVA: 0x000A60C5 File Offset: 0x000A44C5
	Public Shared Property COLOR_SELECTED As Color

	' Token: 0x170002C4 RID: 708
	' (get) Token: 0x060011B7 RID: 4535 RVA: 0x000A60CD File Offset: 0x000A44CD
	' (set) Token: 0x060011B8 RID: 4536 RVA: 0x000A60D4 File Offset: 0x000A44D4
	Public Shared Property COLOR_INACTIVE As Color

	' Token: 0x170002C5 RID: 709
	' (get) Token: 0x060011B9 RID: 4537 RVA: 0x000A60DC File Offset: 0x000A44DC
	' (set) Token: 0x060011BA RID: 4538 RVA: 0x000A60E3 File Offset: 0x000A44E3
	Public Shared Property COLOR_DESABLE As Color

	' Token: 0x060011BB RID: 4539 RVA: 0x000A60EC File Offset: 0x000A44EC
	Protected Overrides Sub Awake()
		MyBase.Awake()
		LevelGameOverGUI.Current = Me
		Me.canvasGroup = MyBase.GetComponent(Of CanvasGroup)()
		MyBase.gameObject.SetActive(False)
		Me.input = New CupheadInput.AnyPlayerInput(False)
		Me.cardCanvasGroup.alpha = 0F
		Me.helpCanvasGroup.alpha = 0F
		Me.ignoreGlobalTime = True
		Me.timeLayer = CupheadTime.Layer.UI
		LevelGameOverGUI.COLOR_SELECTED = Me.menuItems(0).color
		LevelGameOverGUI.COLOR_INACTIVE = Me.menuItems(Me.menuItems.Length - 1).color
		If Level.IsTowerOfPower Then
			Me.equipToolTip.SetActive(False)
			If Not TowerOfPowerLevelGameInfo.IsTokenLeft() Then
				Me.menuItems(0).gameObject.SetActive(False)
				Me.selection = 1
				Me.UpdateSelection()
			Else
				Me.retryLocHelper.currentID = Localization.Find("OptionMenuRetryTowerBattle").id
				Me.retryLocHelper.ApplyTranslation()
			End If
		End If
		Me.state = LevelGameOverGUI.State.Init
	End Sub

	' Token: 0x060011BC RID: 4540 RVA: 0x000A61F5 File Offset: 0x000A45F5
	Private Sub Start()
		If Level.Current IsNot Nothing AndAlso Level.Current.CurrentLevel = Levels.Airplane Then
			Me.updateRotateControlsToggleVisualValue()
		End If
	End Sub

	' Token: 0x060011BD RID: 4541 RVA: 0x000A6221 File Offset: 0x000A4621
	Private Sub OnDestroy()
		LevelGameOverGUI.Current = Nothing
		Me.youDiedText = Nothing
		Me.bossPortraitImage = Nothing
		Me.timeline.cuphead = Nothing
		Me.timeline.mugman = Nothing
		Me.timeline = Nothing
	End Sub

	' Token: 0x060011BE RID: 4542 RVA: 0x000A6258 File Offset: 0x000A4658
	Private Sub Update()
		If Me.state <> LevelGameOverGUI.State.Ready Then
			Return
		End If
		If Me.selection = 2 AndAlso Level.Current IsNot Nothing AndAlso Level.Current.CurrentLevel = Levels.Airplane AndAlso (Me.getButtonDown(CupheadButton.Accept) OrElse Me.getButtonDown(CupheadButton.MenuLeft) OrElse Me.getButtonDown(CupheadButton.MenuRight)) Then
			AudioManager.Play("level_menu_card_down")
			Me.toggleRotateControls()
			Return
		End If
		Dim num As Integer = 0
		If Me.getButtonDown(CupheadButton.Accept) Then
			Me.[Select]()
			AudioManager.Play("level_menu_select")
			Me.state = LevelGameOverGUI.State.Exiting
		End If
		If Not Level.IsTowerOfPower AndAlso Me.getButtonDown(CupheadButton.EquipMenu) Then
			Me.ChangeEquipment()
		End If
		If Me.getButtonDown(CupheadButton.MenuDown) Then
			AudioManager.Play("level_menu_move")
			num += 1
		End If
		If Me.getButtonDown(CupheadButton.MenuUp) Then
			AudioManager.Play("level_menu_move")
			num -= 1
		End If
		Me.selection += num
		Me.selection = Mathf.Clamp(Me.selection, 0, Me.menuItems.Length - 1)
		If Not Me.menuItems(Me.selection).gameObject.activeSelf Then
			Me.selection -= num
			Me.selection = Mathf.Clamp(Me.selection, 0, Me.menuItems.Length - 1)
		End If
		Me.UpdateSelection()
	End Sub

	' Token: 0x060011BF RID: 4543 RVA: 0x000A63C9 File Offset: 0x000A47C9
	Private Function getButtonDown(button As CupheadButton) As Boolean
		Return Me.input.GetButtonDown(button)
	End Function

	' Token: 0x060011C0 RID: 4544 RVA: 0x000A63D8 File Offset: 0x000A47D8
	Private Sub UpdateSelection()
		For i As Integer = 0 To Me.menuItems.Length - 1
			Dim text As Text = Me.menuItems(i)
			If i = Me.selection Then
				text.color = LevelGameOverGUI.COLOR_SELECTED
			Else
				text.color = LevelGameOverGUI.COLOR_INACTIVE
			End If
		Next
	End Sub

	' Token: 0x060011C1 RID: 4545 RVA: 0x000A6430 File Offset: 0x000A4830
	Private Sub [Select]()
		If Not Level.IsGraveyard Then
			AudioManager.SnapshotReset(SceneLoader.SceneName, 2F)
			AudioManager.ChangeBGMPitch(1F, 2F)
		End If
		If Level.Current IsNot Nothing AndAlso Level.Current.CurrentLevel = Levels.Airplane Then
			SettingsData.Save()
			If PlatformHelper.IsConsole Then
				SettingsData.SaveToCloud()
			End If
		End If
		Select Case Me.selection
			Case Else
				Me.Retry()
				AudioManager.Play("level_menu_card_down")
			Case 1
				Me.ExitToMap()
				AudioManager.Play("level_menu_card_down")
			Case 2
				Me.QuitGame()
				AudioManager.Play("level_menu_card_down")
		End Select
	End Sub

	' Token: 0x060011C2 RID: 4546 RVA: 0x000A64FA File Offset: 0x000A48FA
	Private Sub Retry()
		If Level.IsDicePalaceMain OrElse Level.IsDicePalace Then
			DicePalaceMainLevelGameInfo.CleanUpRetry()
		End If
		SceneLoader.ReloadLevel()
	End Sub

	' Token: 0x060011C3 RID: 4547 RVA: 0x000A651A File Offset: 0x000A491A
	Private Sub ExitToMap()
		SceneLoader.LoadLastMap()
	End Sub

	' Token: 0x060011C4 RID: 4548 RVA: 0x000A6521 File Offset: 0x000A4921
	Private Sub QuitGame()
		Level.IsGraveyard = False
		PlayerManager.ResetPlayers()
		SceneLoader.LoadScene(Scenes.scene_title, SceneLoader.Transition.Fade, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, Nothing)
	End Sub

	' Token: 0x060011C5 RID: 4549 RVA: 0x000A6538 File Offset: 0x000A4938
	Private Sub ChangeEquipment()
		MyBase.StartCoroutine(Me.outforequip_cr())
	End Sub

	' Token: 0x060011C6 RID: 4550 RVA: 0x000A6547 File Offset: 0x000A4947
	Public Sub ReactivateOnChangeEquipmentClosed()
		MyBase.StartCoroutine(Me.inforequip_cr())
	End Sub

	' Token: 0x060011C7 RID: 4551 RVA: 0x000A6556 File Offset: 0x000A4956
	Private Sub SetAlpha(value As Single)
		Me.canvasGroup.alpha = value
	End Sub

	' Token: 0x060011C8 RID: 4552 RVA: 0x000A6564 File Offset: 0x000A4964
	Private Sub SetTextAlpha(value As Single)
		Dim color As Color = Me.youDiedText.color
		color.a = value
		Me.youDiedText.color = color
	End Sub

	' Token: 0x060011C9 RID: 4553 RVA: 0x000A6594 File Offset: 0x000A4994
	Private Sub SetCardValue(value As Single)
		Me.cardCanvasGroup.alpha = value
		Me.helpCanvasGroup.alpha = value
		Me.cardCanvasGroup.transform.SetLocalEulerAngles(Nothing, Nothing, New Single?(Mathf.Lerp(30F, 4F, value)))
	End Sub

	' Token: 0x060011CA RID: 4554 RVA: 0x000A65F0 File Offset: 0x000A49F0
	Private Sub SetCardValueEquipSwap(value As Single)
		Me.cardCanvasGroup.alpha = value
		Me.helpCanvasGroup.alpha = value
		Me.cardCanvasGroup.transform.SetLocalEulerAngles(Nothing, Nothing, New Single?(Mathf.Lerp(30F, 4F, value)))
		Me.cardCanvasGroup.transform.SetLocalPosition(Nothing, New Single?(Mathf.Lerp(-720F, 0F, value)), Nothing)
	End Sub

	' Token: 0x060011CB RID: 4555 RVA: 0x000A6684 File Offset: 0x000A4A84
	Public Sub [In](secretTriggered As Boolean)
		MyBase.gameObject.SetActive(True)
		Me.bossPortraitImage.sprite = Level.Current.BossPortrait
		If secretTriggered Then
			Me.cardCanvasGroup.GetComponent(Of Image)().sprite = Me.timelineSecret
			Me.timelineObj.SetActive(False)
		End If
		If Me.bossQuoteLocalization Is Nothing Then
			Me.bossQuoteText.text = """" + Level.Current.BossQuote + """"
		Else
			Me.bossQuoteLocalization.ApplyTranslation(Localization.Find(Level.Current.BossQuote), Nothing)
			If Localization.language = Localization.Languages.Korean Then
				Me.bossQuoteLocalization.textMeshProComponent.fontStyle = FontStyles.Bold
			End If
		End If
		If Me.bossPortraitImage.sprite IsNot Nothing Then
			Me.bossPortraitImage.rectTransform.SetSize(Me.bossPortraitImage.sprite.rect.width, Me.bossPortraitImage.sprite.rect.height)
		End If
		MyBase.StartCoroutine(Me.in_cr())
	End Sub

	' Token: 0x060011CC RID: 4556 RVA: 0x000A67B0 File Offset: 0x000A4BB0
	Private Iterator Function in_cr() As IEnumerator
		AudioManager.Play("level_menu_card_up")
		Yield MyBase.TweenValue(0F, 1F, 0.05F, EaseUtils.EaseType.linear, AddressOf Me.SetAlpha)
		Yield New WaitForSeconds(1F)
		For Each playerDeathEffect As PlayerDeathEffect In Global.UnityEngine.[Object].FindObjectsOfType(Of PlayerDeathEffect)()
			playerDeathEffect.GameOverUnpause()
		Next
		For Each planePlayerDeathPart As PlanePlayerDeathPart In Global.UnityEngine.[Object].FindObjectsOfType(Of PlanePlayerDeathPart)()
			planePlayerDeathPart.GameOverUnpause()
		Next
		Yield MyBase.TweenValue(1F, 0F, 0.25F, EaseUtils.EaseType.linear, AddressOf Me.SetTextAlpha)
		Yield New WaitForSeconds(0.3F)
		If Not Level.IsGraveyard AndAlso Not Level.IsChessBoss Then
			AudioManager.Play("player_die_vinylscratch")
			AudioManager.HandleSnapshot(AudioManager.Snapshots.Death.ToString(), 4F)
			AudioManager.ChangeBGMPitch(0.7F, 6F)
		End If
		CupheadLevelCamera.Current.StartBlur()
		Me.timeline.Setup(Me, Level.Current.timeline)
		MyBase.TweenValue(0F, 1F, 0.3F, EaseUtils.EaseType.easeOutCubic, AddressOf Me.SetCardValue)
		Me.state = LevelGameOverGUI.State.Ready
		Yield Nothing
		Return
	End Function

	' Token: 0x060011CD RID: 4557 RVA: 0x000A67CC File Offset: 0x000A4BCC
	Private Iterator Function outforequip_cr() As IEnumerator
		Me.state = LevelGameOverGUI.State.Init
		Me.equipUI.gameObject.SetActive(True)
		Me.equipUI.Activate()
		Yield MyBase.TweenValue(1F, 0F, 0.3F, EaseUtils.EaseType.easeOutCubic, AddressOf Me.SetCardValueEquipSwap)
		Return
	End Function

	' Token: 0x060011CE RID: 4558 RVA: 0x000A67E8 File Offset: 0x000A4BE8
	Private Iterator Function inforequip_cr() As IEnumerator
		Yield MyBase.TweenValue(0F, 1F, 0.3F, EaseUtils.EaseType.easeOutCubic, AddressOf Me.SetCardValueEquipSwap)
		Me.state = LevelGameOverGUI.State.Ready
		Return
	End Function

	' Token: 0x060011CF RID: 4559 RVA: 0x000A6803 File Offset: 0x000A4C03
	Private Sub toggleRotateControls()
		SettingsData.Data.rotateControlsWithCamera = Not SettingsData.Data.rotateControlsWithCamera
		Me.updateRotateControlsToggleVisualValue()
	End Sub

	' Token: 0x060011D0 RID: 4560 RVA: 0x000A6824 File Offset: 0x000A4C24
	Private Sub updateRotateControlsToggleVisualValue()
		Dim text As Text = Me.menuItems(2)
		text.GetComponent(Of LocalizationHelper)().ApplyTranslation(Localization.Find("CameraRotationControl"), Nothing)
		text.text = String.Format(text.text, If((Not SettingsData.Data.rotateControlsWithCamera), "A", "B"))
	End Sub

	' Token: 0x04001B38 RID: 6968
	Public Shared Current As LevelGameOverGUI

	' Token: 0x04001B39 RID: 6969
	<SerializeField()>
	Private youDiedText As Image

	' Token: 0x04001B3A RID: 6970
	<Space(10F)>
	<SerializeField()>
	Private cardCanvasGroup As CanvasGroup

	' Token: 0x04001B3B RID: 6971
	<Space(10F)>
	<SerializeField()>
	Private helpCanvasGroup As CanvasGroup

	' Token: 0x04001B3C RID: 6972
	<Space(10F)>
	<SerializeField()>
	Private bossPortraitImage As Image

	' Token: 0x04001B3D RID: 6973
	<SerializeField()>
	Private bossQuoteText As Text

	' Token: 0x04001B3E RID: 6974
	<SerializeField()>
	Private bossQuoteLocalization As LocalizationHelper

	' Token: 0x04001B3F RID: 6975
	<Space(10F)>
	<SerializeField()>
	Private menuItems As Text()

	' Token: 0x04001B40 RID: 6976
	<SerializeField()>
	Private timeline As LevelGameOverGUI.TimelineObjects

	' Token: 0x04001B41 RID: 6977
	<SerializeField()>
	Private timelineObj As GameObject

	' Token: 0x04001B42 RID: 6978
	<SerializeField()>
	Private timelineSecret As Sprite

	' Token: 0x04001B43 RID: 6979
	<SerializeField()>
	Private equipUI As LevelEquipUI

	' Token: 0x04001B44 RID: 6980
	<SerializeField()>
	Private equipToolTip As GameObject

	' Token: 0x04001B45 RID: 6981
	Private state As LevelGameOverGUI.State

	' Token: 0x04001B46 RID: 6982
	Private input As CupheadInput.AnyPlayerInput

	' Token: 0x04001B47 RID: 6983
	Private canvasGroup As CanvasGroup

	' Token: 0x04001B48 RID: 6984
	Private selection As Integer

	' Token: 0x04001B49 RID: 6985
	<SerializeField()>
	Private retryLocHelper As LocalizationHelper

	' Token: 0x0200047F RID: 1151
	Private Enum State
		' Token: 0x04001B4B RID: 6987
		Init
		' Token: 0x04001B4C RID: 6988
		Ready
		' Token: 0x04001B4D RID: 6989
		Exiting
	End Enum

	' Token: 0x02000480 RID: 1152
	<Serializable()>
	Public Class TimelineObjects
		' Token: 0x060011D2 RID: 4562 RVA: 0x000A6888 File Offset: 0x000A4C88
		Public Sub Setup(gui As LevelGameOverGUI, properties As Level.Timeline)
			Dim num As Integer = 0
			For Each [event] As Level.Timeline.[Event] In properties.events
				Dim rectTransform As RectTransform = Global.UnityEngine.[Object].Instantiate(Of RectTransform)(Me.line)
				rectTransform.SetParent(Me.line.parent, False)
				rectTransform.SetAsFirstSibling()
				Dim [object] As Global.UnityEngine.[Object] = rectTransform
				Dim obj As Object = "Line "
				Dim num2 As Integer = num
				num = num2 + 1
				[object].name = obj + num2
				Dim vector As Vector3 = Vector3.Lerp(Me.[end].localPosition, Me.start.localPosition, [event].percentage)
				vector.y -= 7F
				rectTransform.localPosition = vector
			Next
			Me.line.gameObject.SetActive(False)
			Dim image As Image = If((Not PlayerManager.GetPlayer(PlayerId.PlayerOne).stats.isChalice), If((Not PlayerManager.player1IsMugman), Me.cuphead, Me.mugman), Me.chalice)
			Dim num3 As Single = If((Not PlayerManager.player1IsMugman), properties.cuphead, properties.mugman)
			gui.StartCoroutine(Me.timelineIcon_cr(image, num3 / properties.health))
			Dim image2 As Image = Nothing
			If PlayerManager.Multiplayer Then
				image2 = If((Not PlayerManager.GetPlayer(PlayerId.PlayerTwo).stats.isChalice), If((Not PlayerManager.player1IsMugman), Me.mugman, Me.cuphead), Me.chalice)
				Dim num4 As Single = If((Not PlayerManager.player1IsMugman), properties.mugman, properties.cuphead)
				gui.StartCoroutine(Me.timelineIcon_cr(image2, num4 / properties.health))
			End If
			Me.cuphead.gameObject.SetActive(image Is Me.cuphead OrElse image2 Is Me.cuphead)
			Me.mugman.gameObject.SetActive(image Is Me.mugman OrElse image2 Is Me.mugman)
			Me.chalice.gameObject.SetActive(image Is Me.chalice OrElse image2 Is Me.chalice)
		End Sub

		' Token: 0x060011D3 RID: 4563 RVA: 0x000A6AF8 File Offset: 0x000A4EF8
		Private Iterator Function timelineIcon_cr(icon As Image, percent As Single) As IEnumerator
			Dim startColor As Color = New Color(1F, 1F, 1F, 0F)
			Dim endColor As Color = New Color(1F, 1F, 1F, 1F)
			Dim t As Single = 0F
			Dim endPosition As Vector3 = Vector3.Lerp(Me.start.localPosition, Me.[end].localPosition, percent)
			icon.rectTransform.localPosition = Me.start.localPosition
			While t < 2F
				Dim val As Single = t / 2F
				Dim newPosition As Vector3 = Vector3.Lerp(Me.start.localPosition, endPosition, EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, 0F, 1F, val))
				icon.rectTransform.localPosition = newPosition
				icon.color = Color.Lerp(startColor, endColor, val * 8F)
				t += Time.deltaTime
				Yield Nothing
			End While
			icon.rectTransform.localPosition = endPosition
			Return
		End Function

		' Token: 0x04001B4E RID: 6990
		Public timeline As RectTransform

		' Token: 0x04001B4F RID: 6991
		Public line As RectTransform

		' Token: 0x04001B50 RID: 6992
		<Header("Players")>
		Public cuphead As Image

		' Token: 0x04001B51 RID: 6993
		Public mugman As Image

		' Token: 0x04001B52 RID: 6994
		Public chalice As Image

		' Token: 0x04001B53 RID: 6995
		<Header("Positions")>
		Public start As Transform

		' Token: 0x04001B54 RID: 6996
		Public [end] As Transform

		' Token: 0x04001B55 RID: 6997
		Private gui As LevelGameOverGUI
	End Class
End Class
