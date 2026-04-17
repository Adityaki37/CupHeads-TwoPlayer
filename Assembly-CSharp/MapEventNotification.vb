Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports TMPro
Imports UnityEngine
Imports UnityEngine.UI

' Token: 0x0200099E RID: 2462
Public Class MapEventNotification
	Inherits AbstractMonoBehaviour

	' Token: 0x170004AF RID: 1199
	' (get) Token: 0x060039B0 RID: 14768 RVA: 0x0020C145 File Offset: 0x0020A545
	' (set) Token: 0x060039B1 RID: 14769 RVA: 0x0020C14C File Offset: 0x0020A54C
	Public Shared Property Current As MapEventNotification

	' Token: 0x170004B0 RID: 1200
	' (get) Token: 0x060039B2 RID: 14770 RVA: 0x0020C154 File Offset: 0x0020A554
	' (set) Token: 0x060039B3 RID: 14771 RVA: 0x0020C15C File Offset: 0x0020A55C
	Public Property showing As Boolean

	' Token: 0x060039B4 RID: 14772 RVA: 0x0020C168 File Offset: 0x0020A568
	Protected Overrides Sub Awake()
		MyBase.Awake()
		MapEventNotification.Current = Me
		Me.input = New CupheadInput.AnyPlayerInput(False)
		Me.canvasGroup = MyBase.GetComponent(Of CanvasGroup)()
		Me.canvasGroup.alpha = 0F
		For i As Integer = 0 To Me.sparkleAnimatorsContract.Length - 1
			Me.sparkleAnimatorsContract(i) = Global.UnityEngine.[Object].Instantiate(Of GameObject)(Me.sparklePrefab, Me.sparkleTransformContract).GetComponent(Of Animator)()
		Next
		For j As Integer = 0 To Me.sparkleAnimatorsCoin1.Length - 1
			Me.sparkleAnimatorsCoin1(j) = Global.UnityEngine.[Object].Instantiate(Of GameObject)(Me.sparklePrefab, Me.sparkleTransformCoin1).GetComponent(Of Animator)()
		Next
		For k As Integer = 0 To Me.sparkleAnimatorsCoin2.Length - 1
			Me.sparkleAnimatorsCoin2(k) = Global.UnityEngine.[Object].Instantiate(Of GameObject)(Me.sparklePrefab, Me.sparkleTransformCoin2).GetComponent(Of Animator)()
		Next
		For l As Integer = 0 To Me.sparkleAnimatorsCoin3.Length - 1
			Me.sparkleAnimatorsCoin3(l) = Global.UnityEngine.[Object].Instantiate(Of GameObject)(Me.sparklePrefab, Me.sparkleTransformCoin3).GetComponent(Of Animator)()
		Next
		Me.dlcUI = Global.UnityEngine.[Object].Instantiate(Of GameObject)(Me.dlcUIPrefab, Me.dlcUIRoot).GetComponent(Of MapDLCUI)()
		Me.dlcUI.Init(False)
		MyBase.gameObject.SetActive(False)
	End Sub

	' Token: 0x060039B5 RID: 14773 RVA: 0x0020C2B9 File Offset: 0x0020A6B9
	Private Sub OnDestroy()
		If MapEventNotification.Current Is Me Then
			MapEventNotification.Current = Nothing
		End If
	End Sub

	' Token: 0x060039B6 RID: 14774 RVA: 0x0020C2D4 File Offset: 0x0020A6D4
	Private Sub Update()
		If Me.superShowing Then
			If Me.input.GetAnyButtonDown() Then
				MyBase.StartCoroutine(Me.tweenOut_cr(1.5F))
				MyBase.animator.SetTrigger("hide_super")
				Me.superShowing = False
			End If
			Me.timeBeforeNextSparkleCoin1 -= CupheadTime.Delta
			For i As Integer = 0 To Me.sparkleAnimatorsCoin1.Length - 1
				If Me.timeBeforeNextSparkleCoin1 <= 0F Then
					If Me.sparkleAnimatorsCoin1(i).GetCurrentAnimatorStateInfo(0).IsName("Empty") Then
						Me.timeBeforeNextSparkleCoin1 = Me.timeBetweenSparkle
						Me.sparkleAnimatorsCoin1(i).transform.position = New Vector3(Me.sparkleTransformCoin1.position.x + Global.UnityEngine.Random.Range(Me.sparkleTransformCoin1.sizeDelta.x * -0.5F, Me.sparkleTransformCoin1.sizeDelta.x * 0.5F), Me.sparkleTransformCoin1.position.y + Global.UnityEngine.Random.Range(Me.sparkleTransformCoin1.sizeDelta.y * -0.5F, Me.sparkleTransformCoin1.sizeDelta.y * 0.5F), 101F)
						Me.sparkleAnimatorsCoin1(i).SetTrigger(Global.UnityEngine.Random.Range(0, 4).ToStringInvariant())
					End If
				End If
			Next
		End If
		If Me.tooltipShowing AndAlso Me.input.GetAnyButtonDown() Then
			MyBase.StartCoroutine(Me.tweenOut_cr(1.5F))
			MyBase.animator.SetTrigger("hide_tooltip")
			Me.tooltipShowing = False
		End If
		If Me.tooltipEquipShowing AndAlso Me.input.GetButtonDown(CupheadButton.EquipMenu) Then
			MyBase.StartCoroutine(Me.tweenOut_cr(0.5F))
			MyBase.animator.SetTrigger("hide_tooltip")
			Me.tooltipShowing = False
		End If
		If Me.coinShowing Then
			If Me.input.GetAnyButtonDown() Then
				MyBase.StartCoroutine(Me.tweenOut_cr(1.5F))
				MyBase.animator.SetTrigger("hide_coin")
				Me.coinShowing = False
			End If
			Me.timeBeforeNextSparkleCoin1 -= CupheadTime.Delta
			Me.timeBeforeNextSparkleCoin2 -= CupheadTime.Delta
			Me.timeBeforeNextSparkleCoin3 -= CupheadTime.Delta
			For j As Integer = 0 To Me.sparkleAnimatorsCoin1.Length - 1
				If Me.timeBeforeNextSparkleCoin1 <= 0F Then
					If Me.sparkleAnimatorsCoin1(j).GetCurrentAnimatorStateInfo(0).IsName("Empty") Then
						Me.timeBeforeNextSparkleCoin1 = Me.timeBetweenSparkle
						Me.sparkleAnimatorsCoin1(j).transform.position = New Vector3(Me.sparkleTransformCoin1.position.x + Global.UnityEngine.Random.Range(Me.sparkleTransformCoin1.sizeDelta.x * -0.5F, Me.sparkleTransformCoin1.sizeDelta.x * 0.5F), Me.sparkleTransformCoin1.position.y + Global.UnityEngine.Random.Range(Me.sparkleTransformCoin1.sizeDelta.y * -0.5F, Me.sparkleTransformCoin1.sizeDelta.y * 0.5F), 101F)
						Me.sparkleAnimatorsCoin1(j).SetTrigger(Global.UnityEngine.Random.Range(0, 4).ToStringInvariant())
					End If
				End If
			Next
			For k As Integer = 0 To Me.sparkleAnimatorsCoin2.Length - 1
				If Me.timeBeforeNextSparkleCoin2 <= 0F Then
					If Me.sparkleAnimatorsCoin2(k).GetCurrentAnimatorStateInfo(0).IsName("Empty") Then
						Me.timeBeforeNextSparkleCoin2 = Me.timeBetweenSparkle
						Me.sparkleAnimatorsCoin2(k).transform.position = New Vector3(Me.sparkleTransformCoin2.position.x + Global.UnityEngine.Random.Range(Me.sparkleTransformCoin2.sizeDelta.x * -0.5F, Me.sparkleTransformCoin2.sizeDelta.x * 0.5F), Me.sparkleTransformCoin2.position.y + Global.UnityEngine.Random.Range(Me.sparkleTransformCoin2.sizeDelta.y * -0.5F, Me.sparkleTransformCoin2.sizeDelta.y * 0.5F), 101F)
						Me.sparkleAnimatorsCoin2(k).SetTrigger(Global.UnityEngine.Random.Range(0, 4).ToStringInvariant())
					End If
				End If
			Next
			For l As Integer = 0 To Me.sparkleAnimatorsCoin3.Length - 1
				If Me.timeBeforeNextSparkleCoin3 <= 0F Then
					If Me.sparkleAnimatorsCoin3(l).GetCurrentAnimatorStateInfo(0).IsName("Empty") Then
						Me.timeBeforeNextSparkleCoin3 = Me.timeBetweenSparkle
						Me.sparkleAnimatorsCoin3(l).transform.position = New Vector3(Me.sparkleTransformCoin3.position.x + Global.UnityEngine.Random.Range(Me.sparkleTransformCoin3.sizeDelta.x * -0.5F, Me.sparkleTransformCoin3.sizeDelta.x * 0.5F), Me.sparkleTransformCoin3.position.y + Global.UnityEngine.Random.Range(Me.sparkleTransformCoin3.sizeDelta.y * -0.5F, Me.sparkleTransformCoin3.sizeDelta.y * 0.5F), 101F)
						Me.sparkleAnimatorsCoin3(l).SetTrigger(Global.UnityEngine.Random.Range(0, 4).ToStringInvariant())
					End If
				End If
			Next
		End If
		If Me.sparkling Then
			If Me.input.GetAnyButtonDown() Then
				MyBase.StartCoroutine(Me.tweenOut_cr(1.5F))
				MyBase.animator.SetTrigger("hide")
				Me.sparkling = False
			End If
			Me.timeBeforeNextSparkleContract -= CupheadTime.Delta
			For m As Integer = 0 To Me.sparkleAnimatorsContract.Length - 1
				If Me.timeBeforeNextSparkleContract <= 0F Then
					If Me.sparkleAnimatorsContract(m).GetCurrentAnimatorStateInfo(0).IsName("Empty") Then
						Me.timeBeforeNextSparkleContract = Me.timeBetweenSparkle
						Me.sparkleAnimatorsContract(m).transform.position = New Vector3(Me.sparkleTransformContract.position.x + Global.UnityEngine.Random.Range(Me.sparkleTransformContract.sizeDelta.x * -0.5F, Me.sparkleTransformContract.sizeDelta.x * 0.5F), Me.sparkleTransformContract.position.y + Global.UnityEngine.Random.Range(Me.sparkleTransformContract.sizeDelta.y * -0.5F, Me.sparkleTransformContract.sizeDelta.y * 0.5F), 101F)
						Me.sparkleAnimatorsContract(m).SetTrigger(Global.UnityEngine.Random.Range(0, 4).ToStringInvariant())
					End If
				End If
			Next
		End If
		If Me.dlcAvailableShowing AndAlso Not Me.dlcUI.visible Then
			MyBase.StartCoroutine(Me.tweenOut_cr(0.25F))
			Me.dlcAvailableShowing = False
		End If
		If Me.ingredientShowing AndAlso Me.input.GetAnyButtonDown() Then
			MyBase.StartCoroutine(Me.tweenOut_cr(1.5F))
			MyBase.animator.SetTrigger("hide_ingred")
			Me.ingredientShowing = False
		End If
		If Me.djimmiShowing AndAlso Me.input.GetAnyButtonDown() Then
			MyBase.animator.SetTrigger("hide_djimmi")
			Me.djimmiShowing = False
		End If
	End Sub

	' Token: 0x060039B7 RID: 14775 RVA: 0x0020CB49 File Offset: 0x0020AF49
	Public Sub SparkleStart()
		Me.sparkling = True
		MyBase.StartCoroutine(Me.showGlyphs_cr())
	End Sub

	' Token: 0x060039B8 RID: 14776 RVA: 0x0020CB60 File Offset: 0x0020AF60
	Protected Iterator Function showGlyphs_cr() As IEnumerator
		Yield New WaitForSeconds(0.5F)
		Dim t As Single = 0F
		While t < 0.2F
			Dim val As Single = t / 0.2F
			Me.glyphCanvasGroup.alpha = Mathf.Lerp(0F, 1F, val)
			t += Time.deltaTime
			Yield Nothing
		End While
		Me.glyphCanvasGroup.alpha = 1F
		While Not Me.input.GetButtonDown(CupheadButton.Accept)
			Yield Nothing
		End While
		MyBase.animator.SetTrigger("hide")
		Yield Nothing
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "anim_map_ui_contract_end", 0, False, True)
		MyBase.gameObject.SetActive(False)
		Return
	End Function

	' Token: 0x060039B9 RID: 14777 RVA: 0x0020CB7C File Offset: 0x0020AF7C
	Public Sub DebugShowContract(level As Levels)
		MyBase.gameObject.SetActive(True)
		Me.super1.SetActive(False)
		Me.super2.SetActive(False)
		Me.super3.SetActive(False)
		Me.coin2.SetActive(False)
		Me.coin3.SetActive(False)
		Me.coinVariable.SetActive(False)
		Me.coinVariableText.enabled = False
		Me.curseCharm.SetActive(False)
		Me.airplaneIngred.SetActive(False)
		Me.rumIngred.SetActive(False)
		Me.oldManIngred.SetActive(False)
		Me.snowCultIngred.SetActive(False)
		Me.cowboyIngred.SetActive(False)
		InterruptingPrompt.SetCanInterrupt(True)
		Me.tooltipEquipGlyph.SetActive(False)
		AudioManager.Play("world_map_soul_contract_open")
		AudioManager.PlayLoop("world_map_soul_contract_stamp_shimmer_loop")
		MyBase.animator.SetTrigger("show")
		Dim translationElement As TranslationElement = Localization.Find(level.ToString())
		Me.localizationHelper.ApplyTranslation(translationElement, Nothing)
		Me.localizationHelper.textMeshProComponent.text = Me.localizationHelper.textMeshProComponent.text.ToUpper().Replace("\N", "\n")
		Dim text As String = If((Localization.language <> Localization.Languages.Japanese), " ", String.Empty)
		Me.notificationLocalizationHelper.ApplyTranslation(Localization.Find("UnlockContract"), New LocalizationHelper.LocalizationSubtext() { New LocalizationHelper.LocalizationSubtext("CONTRACT", translationElement.translation.text.Replace("\n", text), False) })
		Me.showing = True
		Me.canvasGroup.alpha = 1F
	End Sub

	' Token: 0x060039BA RID: 14778 RVA: 0x0020CD39 File Offset: 0x0020B139
	Public Sub DebugShowEvent(level As Levels)
		Me.DebugShowContract(level)
	End Sub

	' Token: 0x060039BB RID: 14779 RVA: 0x0020CD44 File Offset: 0x0020B144
	Public Iterator Function HideContract() As IEnumerator
		Me.showing = True
		MyBase.animator.SetTrigger("hide")
		Yield Nothing
		MyBase.gameObject.SetActive(False)
		Return
	End Function

	' Token: 0x060039BC RID: 14780 RVA: 0x0020CD60 File Offset: 0x0020B160
	Public Sub ShowEvent(eventType As MapEventNotification.Type)
		Me.EventQueue.Enqueue(Sub()
			Me.InternalShowEvent(eventType)
		End Sub)
	End Sub

	' Token: 0x060039BD RID: 14781 RVA: 0x0020CD98 File Offset: 0x0020B198
	Public Sub ShowVariableCoinEvent(coinCount As Integer)
		If coinCount > 1 Then
			Me.coinVariableCount = coinCount
			Me.EventQueue.Enqueue(Sub()
				Me.InternalShowEvent(MapEventNotification.Type.CoinVariable)
			End Sub)
		Else
			Me.EventQueue.Enqueue(Sub()
				Me.InternalShowEvent(MapEventNotification.Type.Coin)
			End Sub)
		End If
	End Sub

	' Token: 0x060039BE RID: 14782 RVA: 0x0020CDE8 File Offset: 0x0020B1E8
	Private Sub InternalShowEvent(eventType As MapEventNotification.Type)
		MyBase.gameObject.SetActive(True)
		Me.super1.SetActive(False)
		Me.super2.SetActive(False)
		Me.super3.SetActive(False)
		Me.coin2.SetActive(False)
		Me.coin3.SetActive(False)
		Me.coinVariable.SetActive(False)
		Me.coinVariableText.enabled = False
		Me.curseCharm.SetActive(False)
		Me.airplaneIngred.SetActive(False)
		Me.rumIngred.SetActive(False)
		Me.oldManIngred.SetActive(False)
		Me.snowCultIngred.SetActive(False)
		Me.cowboyIngred.SetActive(False)
		InterruptingPrompt.SetCanInterrupt(True)
		Select Case eventType
			Case MapEventNotification.Type.SoulContract
				Me.confirmGlyph.SetActive(True)
				Me.tooltipEquipGlyph.SetActive(False)
				AudioManager.Play("world_map_soul_contract_open")
				AudioManager.PlayLoop("world_map_soul_contract_stamp_shimmer_loop")
				MyBase.animator.SetTrigger("show")
				Dim translationElement As TranslationElement = Localization.Find(Level.PreviousLevel.ToString())
				Me.localizationHelper.ApplyTranslation(translationElement, Nothing)
				Me.localizationHelper.textMeshProComponent.text = Me.localizationHelper.textMeshProComponent.text.ToUpper().Replace("\N", "\n")
				Dim text As String = If((Localization.language <> Localization.Languages.Japanese), " ", String.Empty)
				Me.notificationLocalizationHelper.ApplyTranslation(Localization.Find("UnlockContract"), New LocalizationHelper.LocalizationSubtext() { New LocalizationHelper.LocalizationSubtext("CONTRACT", translationElement.translation.text.Replace("\n", text), False) })
			Case MapEventNotification.Type.Super
				Me.confirmGlyph.SetActive(True)
				MyBase.animator.SetTrigger("show_super")
				AudioManager.[Stop]("world_level_bridge_building_poof")
				AudioManager.Play("world_map_super_open")
				AudioManager.PlayLoop("world_map_super_loop")
				MyBase.StartCoroutine(Me.SuperInRoutine())
				Me.notificationLocalizationHelper.ApplyTranslation(Localization.Find("UnlockSuper"), Nothing)
				Dim currentMap As Scenes = PlayerData.Data.CurrentMap
				If currentMap <> Scenes.scene_map_world_1 Then
					If currentMap <> Scenes.scene_map_world_2 Then
						If currentMap = Scenes.scene_map_world_3 Then
							Me.super3.SetActive(True)
						End If
					Else
						Me.super2.SetActive(True)
					End If
				Else
					Me.super1.SetActive(True)
				End If
			Case MapEventNotification.Type.Coin
				Me.confirmGlyph.SetActive(True)
				AudioManager.Play("world_map_coin_open")
				MyBase.StartCoroutine(Me.CoinInRoutine())
				Me.notificationLocalizationHelper.ApplyTranslation(Localization.Find("GotACoin"), Nothing)
				MyBase.animator.SetTrigger("show_coin")
			Case MapEventNotification.Type.ThreeCoins
				Me.confirmGlyph.SetActive(True)
				Me.coin2.SetActive(True)
				Me.coin3.SetActive(True)
				AudioManager.Play("world_map_coin_open")
				MyBase.StartCoroutine(Me.CoinInRoutine())
				Me.notificationLocalizationHelper.ApplyTranslation(Localization.Find("GotThreeCoins"), Nothing)
				MyBase.animator.SetTrigger("show_coin")
			Case MapEventNotification.Type.Tooltip
				Me.confirmGlyph.SetActive(True)
				Me.tooltipEquipGlyph.SetActive(False)
				MyBase.StartCoroutine(Me.TooltipInRoutine())
				MyBase.animator.SetTrigger("show_tooltip")
				AudioManager.Play("menu_cardup")
			Case MapEventNotification.Type.TooltipEquip
				Me.confirmGlyph.SetActive(False)
				Me.tooltipEquipGlyph.SetActive(True)
				MyBase.StartCoroutine(Me.TooltipEquipInRoutine())
				MyBase.animator.SetTrigger("show_tooltip")
				AudioManager.Play("menu_cardup")
			Case MapEventNotification.Type.DLCAvailable
				MyBase.GetComponent(Of Animator)().enabled = False
				MyBase.transform.Find("Darker").gameObject.SetActive(False)
				MyBase.transform.Find("Background").gameObject.SetActive(False)
				MyBase.transform.Find("Text").gameObject.SetActive(False)
				MyBase.transform.Find("LetterboxTop").gameObject.SetActive(False)
				MyBase.transform.Find("LetterboxBottom").gameObject.SetActive(False)
				Me.confirmGlyph.SetActive(True)
				Me.notificationLocalizationHelper.textComponent.text = String.Empty
				Me.dlcUI.ShowMenu()
				MyBase.StartCoroutine(Me.DLCAvailableRoutine())
			Case MapEventNotification.Type.AirplaneIngredient
				Me.confirmGlyph.SetActive(True)
				Me.airplaneIngred.SetActive(True)
				MyBase.StartCoroutine(Me.IngredientRoutine())
				MyBase.animator.SetTrigger("show_ingred_airplane")
				AudioManager.Play("sfx_dlc_worldmap_ingredient")
				Dim translationElement As TranslationElement = Localization.Find("AirplaneIngredient")
				Me.localizationHelper.ApplyTranslation(translationElement, Nothing)
				Me.localizationHelper.textMeshProComponent.text = Me.localizationHelper.textMeshProComponent.text.ToUpper().Replace("\N", "\n")
				Dim text As String = If((Localization.language <> Localization.Languages.Japanese), " ", String.Empty)
				Me.notificationLocalizationHelper.ApplyTranslation(Localization.Find("UnlockIngredient"), New LocalizationHelper.LocalizationSubtext() { New LocalizationHelper.LocalizationSubtext("INGREDIENT", translationElement.translation.text.Replace("\n", text), False) })
			Case MapEventNotification.Type.RumIngredient
				Me.confirmGlyph.SetActive(True)
				Me.rumIngred.SetActive(True)
				MyBase.StartCoroutine(Me.IngredientRoutine())
				MyBase.animator.SetTrigger("show_ingred_rum")
				Dim translationElement As TranslationElement = Localization.Find("RumIngredient")
				AudioManager.Play("sfx_dlc_worldmap_ingredient")
				Me.localizationHelper.ApplyTranslation(translationElement, Nothing)
				Me.localizationHelper.textMeshProComponent.text = Me.localizationHelper.textMeshProComponent.text.ToUpper().Replace("\N", "\n")
				Dim text As String = If((Localization.language <> Localization.Languages.Japanese), " ", String.Empty)
				Me.notificationLocalizationHelper.ApplyTranslation(Localization.Find("UnlockIngredient"), New LocalizationHelper.LocalizationSubtext() { New LocalizationHelper.LocalizationSubtext("INGREDIENT", translationElement.translation.text.Replace("\n", text), False) })
			Case MapEventNotification.Type.OldManIngredient
				Me.confirmGlyph.SetActive(True)
				Me.oldManIngred.SetActive(True)
				MyBase.StartCoroutine(Me.IngredientRoutine())
				MyBase.animator.SetTrigger("show_ingred_oldman")
				AudioManager.Play("sfx_dlc_worldmap_ingredient")
				Dim translationElement As TranslationElement = Localization.Find("OldManIngredient")
				Me.localizationHelper.ApplyTranslation(translationElement, Nothing)
				Me.localizationHelper.textMeshProComponent.text = Me.localizationHelper.textMeshProComponent.text.ToUpper().Replace("\N", "\n")
				Dim text As String = If((Localization.language <> Localization.Languages.Japanese), " ", String.Empty)
				Me.notificationLocalizationHelper.ApplyTranslation(Localization.Find("UnlockIngredient"), New LocalizationHelper.LocalizationSubtext() { New LocalizationHelper.LocalizationSubtext("INGREDIENT", translationElement.translation.text.Replace("\n", text), False) })
			Case MapEventNotification.Type.SnowIngredient
				Me.confirmGlyph.SetActive(True)
				Me.snowCultIngred.SetActive(True)
				MyBase.StartCoroutine(Me.IngredientRoutine())
				MyBase.animator.SetTrigger("show_ingred_snowcult")
				AudioManager.Play("sfx_dlc_worldmap_ingredient")
				Dim translationElement As TranslationElement = Localization.Find("SnowCultIngredient")
				Me.localizationHelper.ApplyTranslation(translationElement, Nothing)
				Me.localizationHelper.textMeshProComponent.text = Me.localizationHelper.textMeshProComponent.text.ToUpper().Replace("\N", "\n")
				Dim text As String = If((Localization.language <> Localization.Languages.Japanese), " ", String.Empty)
				Me.notificationLocalizationHelper.ApplyTranslation(Localization.Find("UnlockIngredient"), New LocalizationHelper.LocalizationSubtext() { New LocalizationHelper.LocalizationSubtext("INGREDIENT", translationElement.translation.text.Replace("\n", text), False) })
			Case MapEventNotification.Type.CowboyIngredient
				Me.confirmGlyph.SetActive(True)
				Me.cowboyIngred.SetActive(True)
				MyBase.StartCoroutine(Me.IngredientRoutine())
				MyBase.animator.SetTrigger("show_ingred_cowboy")
				AudioManager.Play("sfx_dlc_worldmap_ingredient")
				Dim translationElement As TranslationElement = Localization.Find("CowboyIngredient")
				Me.localizationHelper.ApplyTranslation(translationElement, Nothing)
				Me.localizationHelper.textMeshProComponent.text = Me.localizationHelper.textMeshProComponent.text.ToUpper().Replace("\N", "\n")
				Dim text As String = If((Localization.language <> Localization.Languages.Japanese), " ", String.Empty)
				Me.notificationLocalizationHelper.ApplyTranslation(Localization.Find("UnlockIngredient"), New LocalizationHelper.LocalizationSubtext() { New LocalizationHelper.LocalizationSubtext("INGREDIENT", translationElement.translation.text.Replace("\n", text), False) })
			Case MapEventNotification.Type.CoinVariable
				Me.confirmGlyph.SetActive(True)
				Me.coinVariable.SetActive(True)
				Me.coinVariableText.text = "x" + Me.coinVariableCount.ToString()
				Me.coinVariableText.enabled = True
				AudioManager.Play("world_map_coin_open")
				MyBase.StartCoroutine(Me.CoinInRoutine())
				Me.notificationLocalizationHelper.ApplyTranslation(Localization.Find("GotACoin"), Nothing)
				MyBase.animator.SetTrigger("show_coinvariable")
			Case MapEventNotification.Type.Djimmi
				AudioManager.Play("sfx_worldmap_djimmi_open")
				Dim translationElement As TranslationElement = Localization.Find("GameDjimmi_Tooltip_Wish" + (3 - PlayerData.Data.djimmiWishes).ToString())
				Me.notificationLocalizationHelper.ApplyTranslation(translationElement, Nothing)
				MyBase.animator.SetTrigger("show_djimmi")
				MyBase.StartCoroutine(Me.DjimmiRoutine())
			Case MapEventNotification.Type.DjimmiFreed
				AudioManager.Play("sfx_worldmap_djimmi_open")
				Dim translationElement As TranslationElement = Localization.Find("GameDjimmi_Tooltip_Freed")
				Me.notificationLocalizationHelper.ApplyTranslation(translationElement, Nothing)
				MyBase.animator.SetTrigger("show_djimmi")
				MyBase.StartCoroutine(Me.DjimmiRoutine())
		End Select
		Me.showing = True
		MyBase.StartCoroutine(Me.tweenIn_cr())
	End Sub

	' Token: 0x060039BF RID: 14783 RVA: 0x0020D8DC File Offset: 0x0020BCDC
	Public Sub ShowTooltipEvent(tooltipEvent As TooltipEvent)
		InterruptingPrompt.SetCanInterrupt(True)
		Select Case tooltipEvent
			Case TooltipEvent.Turtle
				Me.tooltipPortrait.sprite = Me.TurtleSprite
				Me.tooltipLocalizationHelper.ApplyTranslation(Localization.Find("Pacifist_Tooltip_NewAudioVisMode"), Nothing)
				Me.ShowEvent(MapEventNotification.Type.Tooltip)
			Case TooltipEvent.Canteen
				Me.tooltipPortrait.sprite = Me.CanteenSprite
				Me.tooltipLocalizationHelper.ApplyTranslation(Localization.Find("Canteen_Tooltip_ShmupWeapons"), Nothing)
				Me.ShowEvent(MapEventNotification.Type.Tooltip)
			Case TooltipEvent.ShopKeep
				Me.tooltipPortrait.sprite = Me.ShopkeepSprite
				Me.tooltipLocalizationHelper.ApplyTranslation(Localization.Find("Shopkeeper_Tooltip_NewPurchase"), Nothing)
				Me.ShowEvent(MapEventNotification.Type.TooltipEquip)
			Case TooltipEvent.Professional
				Me.tooltipPortrait.sprite = Me.ForkSprite
				Me.tooltipLocalizationHelper.ApplyTranslation(Localization.Find("Professional_Tooltip_SuperEquip"), Nothing)
				Me.ShowEvent(MapEventNotification.Type.Tooltip)
			Case TooltipEvent.KingDice
				Me.tooltipPortrait.sprite = Me.KingDiceSprite
				Me.tooltipLocalizationHelper.ApplyTranslation(Localization.Find("KingDice_Tooltip_RegularSoulContracts"), Nothing)
				Me.ShowEvent(MapEventNotification.Type.Tooltip)
			Case TooltipEvent.Mausoleum
				Me.tooltipPortrait.sprite = Me.MausoleumSprite
				Me.tooltipLocalizationHelper.ApplyTranslation(Localization.Find("Chalice_Tooltip_NewSuperEquip"), Nothing)
				Me.ShowEvent(MapEventNotification.Type.TooltipEquip)
			Case TooltipEvent.Boatman
				Me.tooltipPortrait.sprite = Me.BoatmanSprite
				Me.tooltipLocalizationHelper.ApplyTranslation(Localization.Find("Boatman_Tooltip_UpgradedSave"), Nothing)
				Me.ShowEvent(MapEventNotification.Type.Tooltip)
			Case TooltipEvent.Chalice
				Me.tooltipPortrait.sprite = Me.SaltbakerSpriteB
				Me.tooltipLocalizationHelper.ApplyTranslation(Localization.Find("Chalice_Tooltip_CharmEquip"), Nothing)
				Me.ShowEvent(MapEventNotification.Type.TooltipEquip)
			Case TooltipEvent.ChaliceTutorialEquipCharm
				Me.tooltipPortrait.sprite = Me.SaltbakerSpriteA
				Me.tooltipLocalizationHelper.ApplyTranslation(Localization.Find(If((Not PlayerManager.Multiplayer), "Saltbaker_Tooltip_ChaliceTutorialSingle", "Saltbaker_Tooltip_ChaliceTutorialMulti")), Nothing)
				Me.ShowEvent(MapEventNotification.Type.TooltipEquip)
			Case TooltipEvent.SimpleIngredient
				Me.tooltipPortrait.sprite = Me.SaltbakerSpriteA
				Me.tooltipLocalizationHelper.ApplyTranslation(Localization.Find("Saltbaker_Tooltip_SimpleIngredient"), Nothing)
				Me.ShowEvent(MapEventNotification.Type.Tooltip)
			Case TooltipEvent.BackToKitchen
				Me.tooltipPortrait.sprite = Me.ChaliceSprite
				Me.tooltipLocalizationHelper.ApplyTranslation(Localization.Find("Chalice_Tooltip_GotAllIngredients"), Nothing)
				Me.ShowEvent(MapEventNotification.Type.Tooltip)
			Case TooltipEvent.ChaliceFan
				Me.tooltipPortrait.sprite = Me.ChaliceFanSprite
				Me.tooltipLocalizationHelper.ApplyTranslation(Localization.Find("ChaliceFan_Tooltip_NewFilter"), Nothing)
				Me.ShowEvent(MapEventNotification.Type.Tooltip)
			Case Else
				Me.tooltipPortrait.sprite = Nothing
				Me.tooltipLocalizationHelper.ApplyTranslation(Localization.Find("Shopkeeper_Tooltip_NewPurchase"), Nothing)
				Me.ShowEvent(MapEventNotification.Type.Tooltip)
		End Select
	End Sub

	' Token: 0x060039C0 RID: 14784 RVA: 0x0020DBD0 File Offset: 0x0020BFD0
	Protected Iterator Function CoinInRoutine() As IEnumerator
		Yield New WaitForSeconds(1F)
		Me.coinShowing = True
		Return
	End Function

	' Token: 0x060039C1 RID: 14785 RVA: 0x0020DBEC File Offset: 0x0020BFEC
	Protected Iterator Function TooltipInRoutine() As IEnumerator
		Yield New WaitForSeconds(1F)
		Me.tooltipShowing = True
		Return
	End Function

	' Token: 0x060039C2 RID: 14786 RVA: 0x0020DC08 File Offset: 0x0020C008
	Protected Iterator Function TooltipEquipInRoutine() As IEnumerator
		Yield New WaitForSeconds(1F)
		Me.tooltipEquipShowing = True
		Return
	End Function

	' Token: 0x060039C3 RID: 14787 RVA: 0x0020DC24 File Offset: 0x0020C024
	Protected Iterator Function SuperInRoutine() As IEnumerator
		Yield New WaitForSeconds(1F)
		Me.superShowing = True
		Return
	End Function

	' Token: 0x060039C4 RID: 14788 RVA: 0x0020DC40 File Offset: 0x0020C040
	Protected Iterator Function DLCAvailableRoutine() As IEnumerator
		Yield New WaitForSeconds(1F)
		Me.dlcAvailableShowing = True
		Return
	End Function

	' Token: 0x060039C5 RID: 14789 RVA: 0x0020DC5C File Offset: 0x0020C05C
	Protected Iterator Function IngredientRoutine() As IEnumerator
		Me.ingredientStarburst.SetActive(True)
		Yield New WaitForSeconds(1F)
		Me.ingredientShowing = True
		Return
	End Function

	' Token: 0x060039C6 RID: 14790 RVA: 0x0020DC77 File Offset: 0x0020C077
	Private Sub AniEvent_DjimmiAppear()
		AudioManager.Play("sfx_worldmap_djimmi_entrance")
	End Sub

	' Token: 0x060039C7 RID: 14791 RVA: 0x0020DC83 File Offset: 0x0020C083
	Private Sub AniEvent_DjimmiLaugh()
		AudioManager.Play("sfx_worldmap_djimmi_laugh")
	End Sub

	' Token: 0x060039C8 RID: 14792 RVA: 0x0020DC8F File Offset: 0x0020C08F
	Private Sub AniEvent_DjimmiMagicLoop()
		AudioManager.PlayLoop("sfx_worldmap_djimmi_magic")
		AudioManager.FadeSFXVolumeLinear("sfx_worldmap_djimmi_magic", 0.5F, 0.5F)
	End Sub

	' Token: 0x060039C9 RID: 14793 RVA: 0x0020DCB0 File Offset: 0x0020C0B0
	Protected Iterator Function DjimmiRoutine() As IEnumerator
		Yield New WaitForSeconds(1F)
		Me.djimmiShowing = True
		Yield MyBase.animator.WaitForAnimationToStart(Me, "anim_map_djimmi_out", False)
		AudioManager.Play("sfx_worldmap_djimmi_disappear")
		AudioManager.[Stop]("sfx_worldmap_djimmi_magic")
		MyBase.StartCoroutine(Me.tweenOut_cr(1.5F))
		Return
	End Function

	' Token: 0x060039CA RID: 14794 RVA: 0x0020DCCC File Offset: 0x0020C0CC
	Protected Iterator Function tweenIn_cr() As IEnumerator
		Dim t As Single = 0F
		While t < 0.2F
			Dim val As Single = t / 0.2F
			Me.canvasGroup.alpha = Mathf.Lerp(0F, 1F, val)
			t += Time.deltaTime
			Yield Nothing
		End While
		Me.canvasGroup.alpha = 1F
		Return
	End Function

	' Token: 0x060039CB RID: 14795 RVA: 0x0020DCE8 File Offset: 0x0020C0E8
	Protected Iterator Function tweenOut_cr(Optional time As Single = 1.5F) As IEnumerator
		AudioManager.FadeSFXVolume("world_map_soul_contract_stamp_shimmer_loop", 0F, 5F)
		AudioManager.FadeSFXVolume("world_map_super_loop", 0F, 5F)
		Yield New WaitForSeconds(time)
		Dim t As Single = 0F
		While t < 0.2F
			Dim val As Single = t / 0.2F
			Me.canvasGroup.alpha = Mathf.Lerp(1F, 0F, val)
			t += Time.deltaTime
			Yield Nothing
		End While
		Me.canvasGroup.alpha = 0F
		While InterruptingPrompt.IsInterrupting()
			Yield Nothing
		End While
		Me.showing = False
		MyBase.gameObject.SetActive(False)
		Return
	End Function

	' Token: 0x060039CC RID: 14796 RVA: 0x0020DD0C File Offset: 0x0020C10C
	Private Iterator Function text_cr() As IEnumerator
		Yield MyBase.StartCoroutine(Me.textScale_cr(0.9F, 1.1F, 0.5F))
		Yield MyBase.StartCoroutine(Me.textScale_cr(1.1F, 0.9F, 0.5F))
		While Not Me.input.GetButtonDown(CupheadButton.Accept)
			Yield Nothing
		End While
		Me.showing = False
		MyBase.gameObject.SetActive(False)
		Return
	End Function

	' Token: 0x060039CD RID: 14797 RVA: 0x0020DD28 File Offset: 0x0020C128
	Protected Iterator Function textScale_cr(start As Single, [end] As Single, time As Single) As IEnumerator
		Dim t As Single = 0F
		While t < time
			Dim val As Single = t / time
			Me.text.transform.localScale = Vector3.one * EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, start, [end], val)
			t += Time.deltaTime
			Yield Nothing
		End While
		Me.text.transform.localScale = Vector3.one * [end]
		Yield Nothing
		Return
	End Function

	' Token: 0x0400416F RID: 16751
	<SerializeField()>
	Private background As Image

	' Token: 0x04004170 RID: 16752
	<SerializeField()>
	Private text As TextMeshProUGUI

	' Token: 0x04004171 RID: 16753
	<SerializeField()>
	Private localizationHelper As LocalizationHelper

	' Token: 0x04004172 RID: 16754
	<SerializeField()>
	Private notificationLocalizationHelper As LocalizationHelper

	' Token: 0x04004173 RID: 16755
	<SerializeField()>
	Private sparkleTransformContract As RectTransform

	' Token: 0x04004174 RID: 16756
	<SerializeField()>
	Private sparkleTransformCoin1 As RectTransform

	' Token: 0x04004175 RID: 16757
	<SerializeField()>
	Private sparkleTransformCoin2 As RectTransform

	' Token: 0x04004176 RID: 16758
	<SerializeField()>
	Private sparkleTransformCoin3 As RectTransform

	' Token: 0x04004177 RID: 16759
	<SerializeField()>
	Private sparklePrefab As GameObject

	' Token: 0x04004178 RID: 16760
	<SerializeField()>
	Private glyphCanvasGroup As CanvasGroup

	' Token: 0x04004179 RID: 16761
	<SerializeField()>
	Private coin2 As GameObject

	' Token: 0x0400417A RID: 16762
	<SerializeField()>
	Private coin3 As GameObject

	' Token: 0x0400417B RID: 16763
	<SerializeField()>
	Private coinVariable As GameObject

	' Token: 0x0400417C RID: 16764
	<SerializeField()>
	Private coinVariableText As Text

	' Token: 0x0400417D RID: 16765
	<SerializeField()>
	Private super1 As GameObject

	' Token: 0x0400417E RID: 16766
	<SerializeField()>
	Private super2 As GameObject

	' Token: 0x0400417F RID: 16767
	<SerializeField()>
	Private super3 As GameObject

	' Token: 0x04004180 RID: 16768
	<SerializeField()>
	Private curseCharm As GameObject

	' Token: 0x04004181 RID: 16769
	<SerializeField()>
	Private ingredientStarburst As GameObject

	' Token: 0x04004182 RID: 16770
	<SerializeField()>
	Private airplaneIngred As GameObject

	' Token: 0x04004183 RID: 16771
	<SerializeField()>
	Private rumIngred As GameObject

	' Token: 0x04004184 RID: 16772
	<SerializeField()>
	Private oldManIngred As GameObject

	' Token: 0x04004185 RID: 16773
	<SerializeField()>
	Private snowCultIngred As GameObject

	' Token: 0x04004186 RID: 16774
	<SerializeField()>
	Private cowboyIngred As GameObject

	' Token: 0x04004187 RID: 16775
	<SerializeField()>
	Private confirmGlyph As GameObject

	' Token: 0x04004188 RID: 16776
	<SerializeField()>
	Private dlcUIPrefab As GameObject

	' Token: 0x04004189 RID: 16777
	<SerializeField()>
	Private dlcUIRoot As Transform

	' Token: 0x0400418A RID: 16778
	<Header("Tooltips")>
	<SerializeField()>
	Private tooltipCanvasGroup As CanvasGroup

	' Token: 0x0400418B RID: 16779
	<SerializeField()>
	Private tooltipPortrait As Image

	' Token: 0x0400418C RID: 16780
	<SerializeField()>
	Private tooltipLocalizationHelper As LocalizationHelper

	' Token: 0x0400418D RID: 16781
	<SerializeField()>
	Private tooltipEquipGlyph As GameObject

	' Token: 0x0400418E RID: 16782
	<SerializeField()>
	Private TurtleSprite As Sprite

	' Token: 0x0400418F RID: 16783
	<SerializeField()>
	Private CanteenSprite As Sprite

	' Token: 0x04004190 RID: 16784
	<SerializeField()>
	Private ShopkeepSprite As Sprite

	' Token: 0x04004191 RID: 16785
	<SerializeField()>
	Private ForkSprite As Sprite

	' Token: 0x04004192 RID: 16786
	<SerializeField()>
	Private KingDiceSprite As Sprite

	' Token: 0x04004193 RID: 16787
	<SerializeField()>
	Private MausoleumSprite As Sprite

	' Token: 0x04004194 RID: 16788
	<SerializeField()>
	Private SaltbakerSpriteA As Sprite

	' Token: 0x04004195 RID: 16789
	<SerializeField()>
	Private SaltbakerSpriteB As Sprite

	' Token: 0x04004196 RID: 16790
	<SerializeField()>
	Private ChaliceSprite As Sprite

	' Token: 0x04004197 RID: 16791
	<SerializeField()>
	Private ChaliceFanSprite As Sprite

	' Token: 0x04004198 RID: 16792
	<SerializeField()>
	Private BoatmanSprite As Sprite

	' Token: 0x04004199 RID: 16793
	Private canvasGroup As CanvasGroup

	' Token: 0x0400419B RID: 16795
	Private sparkling As Boolean

	' Token: 0x0400419C RID: 16796
	Private coinShowing As Boolean

	' Token: 0x0400419D RID: 16797
	Private tooltipShowing As Boolean

	' Token: 0x0400419E RID: 16798
	Private tooltipEquipShowing As Boolean

	' Token: 0x0400419F RID: 16799
	Private superShowing As Boolean

	' Token: 0x040041A0 RID: 16800
	Private dlcAvailableShowing As Boolean

	' Token: 0x040041A1 RID: 16801
	Private ingredientShowing As Boolean

	' Token: 0x040041A2 RID: 16802
	Private djimmiShowing As Boolean

	' Token: 0x040041A3 RID: 16803
	Private coinVariableCount As Integer

	' Token: 0x040041A4 RID: 16804
	Private sparkleAnimatorsContract As Animator() = New Animator(2) {}

	' Token: 0x040041A5 RID: 16805
	Private sparkleAnimatorsCoin1 As Animator() = New Animator(2) {}

	' Token: 0x040041A6 RID: 16806
	Private sparkleAnimatorsCoin2 As Animator() = New Animator(2) {}

	' Token: 0x040041A7 RID: 16807
	Private sparkleAnimatorsCoin3 As Animator() = New Animator(2) {}

	' Token: 0x040041A8 RID: 16808
	Private timeBeforeNextSparkleContract As Single = 0.2F

	' Token: 0x040041A9 RID: 16809
	Private timeBeforeNextSparkleCoin1 As Single = 0.2F

	' Token: 0x040041AA RID: 16810
	Private timeBeforeNextSparkleCoin2 As Single = 0.2F

	' Token: 0x040041AB RID: 16811
	Private timeBeforeNextSparkleCoin3 As Single = 0.2F

	' Token: 0x040041AC RID: 16812
	<SerializeField()>
	Private timeBetweenSparkle As Single = 0.3F

	' Token: 0x040041AD RID: 16813
	Private input As CupheadInput.AnyPlayerInput

	' Token: 0x040041AE RID: 16814
	Public EventQueue As Queue(Of Action) = New Queue(Of Action)()

	' Token: 0x040041AF RID: 16815
	Private dlcUI As MapDLCUI

	' Token: 0x0200099F RID: 2463
	Public Enum Type
		' Token: 0x040041B1 RID: 16817
		SoulContract
		' Token: 0x040041B2 RID: 16818
		Super
		' Token: 0x040041B3 RID: 16819
		Coin
		' Token: 0x040041B4 RID: 16820
		ThreeCoins
		' Token: 0x040041B5 RID: 16821
		Blueprint
		' Token: 0x040041B6 RID: 16822
		Tooltip
		' Token: 0x040041B7 RID: 16823
		TooltipEquip
		' Token: 0x040041B8 RID: 16824
		DLCAvailable
		' Token: 0x040041B9 RID: 16825
		AirplaneIngredient
		' Token: 0x040041BA RID: 16826
		RumIngredient
		' Token: 0x040041BB RID: 16827
		OldManIngredient
		' Token: 0x040041BC RID: 16828
		SnowIngredient
		' Token: 0x040041BD RID: 16829
		CowboyIngredient
		' Token: 0x040041BE RID: 16830
		CoinVariable
		' Token: 0x040041BF RID: 16831
		Djimmi
		' Token: 0x040041C0 RID: 16832
		DjimmiFreed
	End Enum
End Class
