Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Diagnostics
Imports Rewired
Imports TMPro
Imports UnityEngine
Imports UnityEngine.UI

' Token: 0x02000B0A RID: 2826
Public Class ShopScenePlayer
	Inherits AbstractMonoBehaviour

	' Token: 0x17000625 RID: 1573
	' (get) Token: 0x0600448B RID: 17547 RVA: 0x002440C5 File Offset: 0x002424C5
	Private ReadOnly Property CurrentItem As ShopSceneItem
		Get
			Me.index = Mathf.Clamp(Me.index, 0, Me.items.Count - 1)
			Return Me.items(Me.index)
		End Get
	End Property

	' Token: 0x140000C6 RID: 198
	' (add) Token: 0x0600448C RID: 17548 RVA: 0x002440F8 File Offset: 0x002424F8
	' (remove) Token: 0x0600448D RID: 17549 RVA: 0x00244130 File Offset: 0x00242530
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnPurchaseEvent As Action

	' Token: 0x140000C7 RID: 199
	' (add) Token: 0x0600448E RID: 17550 RVA: 0x00244168 File Offset: 0x00242568
	' (remove) Token: 0x0600448F RID: 17551 RVA: 0x002441A0 File Offset: 0x002425A0
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnExitEvent As Action

	' Token: 0x06004490 RID: 17552 RVA: 0x002441D8 File Offset: 0x002425D8
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.doorPositionOpen = Me.door.transform.localPosition.x
		Me.door.transform.SetLocalPosition(New Single?(0F), Nothing, Nothing)
		Me.doorPositionClosed = Me.door.transform.localPosition.x
		Me.currencyCanvasOriginalScale = Me.currencyCanvas.localScale.x
		If Not PlayerManager.Multiplayer AndAlso Me.player = PlayerId.PlayerTwo Then
			Me.items(0).gameObject.SetActive(False)
			Return
		End If
		Me.weaponIndex = 0
		Me.charmIndex = 0
		For i As Integer = 0 To Me.items.Count - 1
			Dim shopSceneItem As ShopSceneItem = Nothing
			Dim itemType As ItemType = Me.items(i).itemType
			If itemType <> ItemType.Weapon Then
				If itemType = ItemType.Charm Then
					While Me.charmIndex < Me.charmItemPrefabs.Length AndAlso (PlayerData.Data.IsUnlocked(Me.player, Me.charmItemPrefabs(Me.charmIndex).charm) OrElse Not Me.charmItemPrefabs(Me.charmIndex).IsAvailable)
						Me.charmIndex += 1
					End While
					If Me.charmIndex < Me.charmItemPrefabs.Length Then
						shopSceneItem = Me.charmItemPrefabs(Me.charmIndex)
						Me.charmIndex += 1
					End If
				End If
			Else
				While Me.weaponIndex < Me.weaponItemPrefabs.Length AndAlso (PlayerData.Data.IsUnlocked(Me.player, Me.weaponItemPrefabs(Me.weaponIndex).weapon) OrElse Not Me.weaponItemPrefabs(Me.weaponIndex).IsAvailable)
					Me.weaponIndex += 1
				End While
				If Me.weaponIndex < Me.weaponItemPrefabs.Length Then
					shopSceneItem = Me.weaponItemPrefabs(Me.weaponIndex)
					Me.weaponIndex += 1
				End If
			End If
			If shopSceneItem Is Nothing Then
				Me.items(i).gameObject.SetActive(False)
				Me.items.RemoveAt(i)
				i -= 1
			Else
				Dim shopSceneItem2 As ShopSceneItem = Me.items(i)
				shopSceneItem2.gameObject.SetActive(False)
				Me.items(i) = Global.UnityEngine.[Object].Instantiate(Of ShopSceneItem)(shopSceneItem)
				Me.items(i).transform.position = shopSceneItem2.transform.position
				Me.items(i).spriteShadowObject.transform.SetParent(Nothing)
			End If
		Next
		For Each shopSceneItem3 As ShopSceneItem In Me.items
			shopSceneItem3.Init(Me.player)
		Next
	End Sub

	' Token: 0x06004491 RID: 17553 RVA: 0x0024452C File Offset: 0x0024292C
	Private Sub Start()
		If Me.player <> PlayerId.PlayerOne AndAlso Not PlayerManager.Multiplayer Then
			MyBase.enabled = False
			MyBase.gameObject.SetActive(False)
			Return
		End If
		Me.singleDigitCoinPosition = Me.coinImage.transform.position
		If PlayerData.Data.GetCurrency(Me.player) >= 10 Then
			Me.isMoneyDoubleDigit = True
			Me.coinImage.transform.position = Me.doubleDigitCoinPosition.position
		End If
		AddHandler PlayerManager.OnPlayerLeaveEvent, AddressOf Me.OnPlayerLeft
		Me.displayNameText.font = Localization.Instance.fonts(CInt(Localization.language))(41).fontAsset
		Me.subText.font = Localization.Instance.fonts(CInt(Localization.language))(41).fontAsset
		Me.descriptionText.font = Localization.Instance.fonts(CInt(Localization.language))(11).fontAsset
	End Sub

	' Token: 0x06004492 RID: 17554 RVA: 0x00244644 File Offset: 0x00242A44
	Private Sub Update()
		If InterruptingPrompt.IsInterrupting() Then
			Return
		End If
		If PlayerData.Data.GetCurrency(Me.player) >= 10 AndAlso Not Me.isMoneyDoubleDigit Then
			Me.isMoneyDoubleDigit = True
			Me.coinImage.transform.position = Me.doubleDigitCoinPosition.position
		ElseIf PlayerData.Data.GetCurrency(Me.player) < 10 AndAlso Me.isMoneyDoubleDigit Then
			Me.isMoneyDoubleDigit = False
			Me.coinImage.transform.position = Me.singleDigitCoinPosition
		End If
		Dim currency As Integer = PlayerData.Data.GetCurrency(Me.player)
		Dim sprite As Sprite
		If currency < 0 Then
			sprite = Me.coinSprites(0)
		ElseIf currency > Me.coinSprites.Count - 1 Then
			sprite = Me.coinSprites(Me.coinSprites.Count - 1)
		Else
			sprite = Me.coinSprites(currency)
		End If
		Me.currencyNbImage.sprite = sprite
		Select Case Me.state
			Case ShopScenePlayer.State.Selecting
				If Me.items.Count > 0 AndAlso Me.CurrentItem.state <> ShopSceneItem.State.Ready Then
					Return
				End If
				If Me.items.Count > 1 AndAlso Me.input.GetButtonDown(18) Then
					AudioManager.Play("shop_selection_change")
					Me.index -= 1
					Me.UpdateSelection()
					Return
				End If
				If Me.items.Count > 1 AndAlso Me.input.GetButtonDown(20) Then
					AudioManager.Play("shop_selection_change")
					Me.index += 1
					Me.UpdateSelection()
					Return
				End If
				If Me.items.Count > 0 AndAlso Me.input.GetButtonDown(13) Then
					If Me.CurrentItem.Purchase() Then
						Me.Purchase()
					Else
						Me.CantPurchase()
					End If
				End If
				If Me.input.GetButtonDown(14) OrElse Me.playerLeft Then
					Me.[Exit]()
				End If
			Case ShopScenePlayer.State.Purchasing
				Return
			Case ShopScenePlayer.State.Exited
				If Me.input.GetButtonDown(13) AndAlso Not Me.exitingShop Then
					Me.StopAllCoroutines()
					Me.state = ShopScenePlayer.State.Init
					Me.OnStart()
				End If
		End Select
	End Sub

	' Token: 0x06004493 RID: 17555 RVA: 0x002448DE File Offset: 0x00242CDE
	Public Function GetWeaponItemPrefabs() As ShopSceneItem()
		Return Me.weaponItemPrefabs
	End Function

	' Token: 0x06004494 RID: 17556 RVA: 0x002448E6 File Offset: 0x00242CE6
	Public Function GetCharmItemPrefabs() As ShopSceneItem()
		Return Me.charmItemPrefabs
	End Function

	' Token: 0x06004495 RID: 17557 RVA: 0x002448EE File Offset: 0x00242CEE
	Public Sub OnStart()
		If Not MyBase.gameObject.activeInHierarchy Then
			Return
		End If
		MyBase.StartCoroutine(Me.in_cr())
	End Sub

	' Token: 0x06004496 RID: 17558 RVA: 0x00244910 File Offset: 0x00242D10
	Private Sub Purchase()
		AudioManager.Play("shop_purchase")
		Me.UpdateSelection()
		If Me.scaleCoinCoroutine IsNot Nothing Then
			MyBase.StopCoroutine(Me.scaleCoinCoroutine)
		End If
		Me.scaleCoinCoroutine = MyBase.StartCoroutine(Me.scaleCoin_cr())
		Me.state = ShopScenePlayer.State.Purchasing
		If Me.OnPurchaseEvent IsNot Nothing Then
			Me.OnPurchaseEvent()
		End If
	End Sub

	' Token: 0x06004497 RID: 17559 RVA: 0x00244973 File Offset: 0x00242D73
	Private Sub CantPurchase()
		AudioManager.Play("shop_cantpurchase")
		If Me.moveItemCantPurchaseCoroutine IsNot Nothing Then
			MyBase.StopCoroutine(Me.moveItemCantPurchaseCoroutine)
		End If
		Me.moveItemCantPurchaseCoroutine = MyBase.StartCoroutine(Me.cantBuy_cr())
	End Sub

	' Token: 0x06004498 RID: 17560 RVA: 0x002449A8 File Offset: 0x00242DA8
	Private Sub [Exit]()
		If Not MyBase.gameObject.activeInHierarchy Then
			Return
		End If
		Me.state = ShopScenePlayer.State.Exiting
		If Me.moveItemCantPurchaseCoroutine IsNot Nothing Then
			MyBase.StopCoroutine(Me.moveItemCantPurchaseCoroutine)
		End If
		MyBase.StartCoroutine(Me.out_cr())
		If Me.OnExitEvent IsNot Nothing Then
			Me.OnExitEvent()
		End If
	End Sub

	' Token: 0x06004499 RID: 17561 RVA: 0x00244A07 File Offset: 0x00242E07
	Public Sub OnExit()
		Me.exitingShop = True
	End Sub

	' Token: 0x0600449A RID: 17562 RVA: 0x00244A10 File Offset: 0x00242E10
	Private Sub UpdateSelection()
		If Me.items.Count = 0 Then
			Dim translation As Localization.Translation = Localization.Translate("out_of_stock_name")
			Me.displayNameText.text = translation.text
			Me.displayNameText.font = translation.fonts.fontAsset
			Dim translation2 As Localization.Translation = Localization.Translate("out_of_stock_subtext")
			Me.subText.text = translation2.text
			Me.subText.font = translation2.fonts.fontAsset
			Dim translation3 As Localization.Translation = Localization.Translate("out_of_stock_description")
			Me.descriptionText.text = translation3.text
			Me.descriptionText.font = translation3.fonts.fontAsset
			Me.priceSpriteRenderer.enabled = False
			Me.chalkCoinSpriteRenderer.enabled = False
			Return
		End If
		For Each shopSceneItem As ShopSceneItem In Me.items
			shopSceneItem.Deselect()
		Next
		Me.CurrentItem.[Select]()
		If Me.CurrentItem.Purchased Then
			Me.displayNameText.text = Localization.Translate("item_purchased_name").text
			Me.subText.text = Localization.Translate("item_purchased_subtext").text
			Me.descriptionText.text = Localization.Translate("item_purchased_description").text
			Return
		End If
		Me.displayNameText.text = Me.CurrentItem.DisplayName.ToUpper()
		Me.priceSpriteRenderer.sprite = Me.priceSprites(Me.CurrentItem.Value - 1)
		Me.subText.text = Me.CurrentItem.Subtext
		Me.descriptionText.text = Me.CurrentItem.Description
		If Me.CurrentItem.charm = Charm.charm_curse Then
			Me.displayNameText.text = Localization.Translate("charm_broken_name").text.ToUpper()
			Me.subText.text = Localization.Translate("charm_broken_subtext").text
			Me.descriptionText.text = Localization.Translate("charm_broken_description").text
		End If
	End Sub

	' Token: 0x0600449B RID: 17563 RVA: 0x00244C84 File Offset: 0x00243084
	Private Sub OnDoorTweened(value As Single)
		Me.door.SetLocalPosition(New Single?(Mathf.Lerp(Me.doorPositionClosed, Me.doorPositionOpen, value)), Nothing, Nothing)
	End Sub

	' Token: 0x0600449C RID: 17564 RVA: 0x00244CC8 File Offset: 0x002430C8
	Private Iterator Function in_cr() As IEnumerator
		If Me.firstStart Then
			Yield New WaitForSeconds(If((Me.player <> PlayerId.PlayerOne), 1.4F, 1.1F))
		End If
		Me.firstStart = False
		Me.input = PlayerManager.GetPlayerInput(Me.player)
		Me.UpdateSelection()
		If Me.player = PlayerId.PlayerOne Then
			AudioManager.Play("shop_slide_open_cuphead")
		Else
			AudioManager.Play("shop_slide_open_mugman")
		End If
		Yield MyBase.TweenValue(0F, 1F, 1F, EaseUtils.EaseType.easeOutBounce, AddressOf Me.OnDoorTweened)
		Me.state = ShopScenePlayer.State.Selecting
		Return
	End Function

	' Token: 0x0600449D RID: 17565 RVA: 0x00244CE4 File Offset: 0x002430E4
	Private Iterator Function out_cr() As IEnumerator
		If Me.player = PlayerId.PlayerOne Then
			AudioManager.Play("shop_slide_close_cuphead")
		Else
			AudioManager.Play("shop_slide_close_mugman")
		End If
		For Each shopSceneItem As ShopSceneItem In Me.items
			shopSceneItem.Deselect()
		Next
		Yield MyBase.TweenValue(1F, 0F, 1F, EaseUtils.EaseType.easeOutBounce, AddressOf Me.OnDoorTweened)
		Me.state = ShopScenePlayer.State.Exited
		Return
	End Function

	' Token: 0x0600449E RID: 17566 RVA: 0x00244D00 File Offset: 0x00243100
	Private Iterator Function scaleCoin_cr() As IEnumerator
		While Me.currencyCanvas.localScale.x < Me.currencyCanvasOriginalScale * Me.currencyCanvasMultiplier
			Me.currencyCanvas.localScale = New Vector2(Me.currencyCanvas.localScale.x + Me.currencyCanvasScaleValue, Me.currencyCanvas.localScale.y + Me.currencyCanvasScaleValue)
			Yield Nothing
		End While
		While Me.currencyCanvas.localScale.x > Me.currencyCanvasOriginalScale
			Me.currencyCanvas.localScale = New Vector2(Me.currencyCanvas.localScale.x - Me.currencyCanvasScaleValue, Me.currencyCanvas.localScale.y - Me.currencyCanvasScaleValue)
			Yield Nothing
		End While
		Me.scaleCoinCoroutine = Nothing
		MyBase.StartCoroutine(Me.addNewItem_cr())
		Return
	End Function

	' Token: 0x0600449F RID: 17567 RVA: 0x00244D1C File Offset: 0x0024311C
	Private Iterator Function cantBuy_cr() As IEnumerator
		Dim startPositionY As Single = Me.CurrentItem.endPosition.y
		While Me.CurrentItem.transform.localPosition.y > startPositionY - Me.CurrentItem.cantPurchaseYMovementPosition
			Me.CurrentItem.transform.localPosition = New Vector2(Me.CurrentItem.transform.localPosition.x, Me.CurrentItem.transform.localPosition.y - Me.CurrentItem.cantPurchaseYMovementValue)
			Yield Nothing
		End While
		Yield CupheadTime.WaitForSeconds(Me, 0.1F)
		While Me.CurrentItem.transform.position.y < startPositionY
			Me.CurrentItem.transform.localPosition = New Vector2(Me.CurrentItem.transform.localPosition.x, Me.CurrentItem.transform.localPosition.y + Me.CurrentItem.cantPurchaseYMovementValue * 1.5F)
			Yield Nothing
		End While
		Me.moveItemCantPurchaseCoroutine = Nothing
		Return
	End Function

	' Token: 0x060044A0 RID: 17568 RVA: 0x00244D38 File Offset: 0x00243138
	Private Iterator Function addNewItem_cr() As IEnumerator
		Dim type As ItemType = Me.CurrentItem.itemType
		Dim originalItem As ShopSceneItem = Me.CurrentItem
		If type = ItemType.Charm Then
			Dim foundItem As Boolean = False
			For i As Integer = Me.charmIndex To Me.charmItemPrefabs.Length - 1
				If Not PlayerData.Data.IsUnlocked(Me.player, Me.charmItemPrefabs(i).charm) AndAlso Me.charmItemPrefabs(i).IsAvailable Then
					foundItem = True
					Dim itemIndex As Integer = Me.items.IndexOf(Me.CurrentItem)
					Me.items(itemIndex) = Global.UnityEngine.[Object].Instantiate(Of ShopSceneItem)(Me.charmItemPrefabs(i))
					Me.items(itemIndex).player = Me.player
					Me.items(itemIndex).startPosition = originalItem.startPosition
					Me.items(itemIndex).endPosition = originalItem.endPosition
					Dim startPosition As Vector3 = Me.items(itemIndex).startPosition
					startPosition.y += 800F
					Me.items(itemIndex).transform.position = Me.items(itemIndex).startPosition
					Me.items(itemIndex).spriteShadowObject.transform.SetParent(Nothing)
					Me.items(itemIndex).transform.position = startPosition
					Dim originalShadowScale As Vector3 = Me.items(itemIndex).spriteShadowObject.transform.localScale
					Me.items(itemIndex).spriteShadowObject.transform.localScale = Vector3.zero
					Me.items(itemIndex).TweenLocalPositionY(Me.items(itemIndex).transform.position.y, Me.items(itemIndex).startPosition.y, 0.5F, EaseUtils.EaseType.linear)
					Dim t As Single = 0F
					Dim TIME As Single = 0.5F
					While t < TIME
						Dim val As Single = t / TIME
						Dim newScale As Vector3 = Vector3.Lerp(Vector3.zero, originalShadowScale, EaseUtils.Ease(EaseUtils.EaseType.linear, 0F, 1F, val))
						Me.items(itemIndex).spriteShadowObject.transform.localScale = newScale
						t += Time.deltaTime
						Yield Nothing
					End While
					Dim dustPoof As SpriteRenderer = Global.UnityEngine.[Object].Instantiate(Of SpriteRenderer)(Me.poofPrefab, Me.items(itemIndex).transform.position + Me.items(itemIndex).poofOffset, Quaternion.identity)
					AudioManager.Play("item_drop")
					dustPoof.sortingOrder = 501
					Global.UnityEngine.[Object].Destroy(dustPoof.gameObject, 3F)
					Yield Me.items(itemIndex).TweenLocalPositionY(Me.items(itemIndex).transform.position.y, Me.items(itemIndex).transform.position.y + 30F, 0.1F, EaseUtils.EaseType.linear)
					Yield Me.items(itemIndex).TweenLocalPositionY(Me.items(itemIndex).transform.position.y, Me.items(itemIndex).transform.position.y - 30F, 0.1F, EaseUtils.EaseType.linear)
					Yield CupheadTime.WaitForSeconds(Me, 0.2F)
					Me.charmIndex = i + 1
					Me.UpdateSelection()
					Exit For
				End If
			Next
			If Not foundItem Then
				Me.CurrentItem.spriteShadowObject.gameObject.SetActive(False)
				Me.items.Remove(Me.CurrentItem)
				Me.UpdateSelection()
			End If
		ElseIf type = ItemType.Weapon Then
			Dim foundItem2 As Boolean = False
			For j As Integer = Me.weaponIndex To Me.weaponItemPrefabs.Length - 1
				If Not PlayerData.Data.IsUnlocked(Me.player, Me.weaponItemPrefabs(j).weapon) AndAlso Me.weaponItemPrefabs(j).IsAvailable Then
					foundItem2 = True
					Dim itemIndex2 As Integer = Me.items.IndexOf(Me.CurrentItem)
					Me.items(itemIndex2) = Global.UnityEngine.[Object].Instantiate(Of ShopSceneItem)(Me.weaponItemPrefabs(j))
					Me.items(itemIndex2).player = Me.player
					Me.items(itemIndex2).startPosition = originalItem.startPosition
					Me.items(itemIndex2).endPosition = originalItem.endPosition
					Dim startPosition2 As Vector3 = Me.items(itemIndex2).startPosition
					startPosition2.y += 800F
					Me.items(itemIndex2).transform.position = Me.items(itemIndex2).startPosition
					Me.items(itemIndex2).spriteShadowObject.transform.SetParent(Nothing)
					Me.items(itemIndex2).transform.position = startPosition2
					Dim originalShadowScale2 As Vector3 = Me.items(itemIndex2).spriteShadowObject.transform.localScale
					Me.items(itemIndex2).spriteShadowObject.transform.localScale = Vector3.zero
					Me.items(itemIndex2).TweenLocalPositionY(Me.items(itemIndex2).transform.position.y, Me.items(itemIndex2).startPosition.y, 0.5F, EaseUtils.EaseType.linear)
					Dim t2 As Single = 0F
					Dim TIME2 As Single = 0.5F
					While t2 < TIME2
						Dim val2 As Single = t2 / TIME2
						Dim newScale2 As Vector3 = Vector3.Lerp(Vector3.zero, originalShadowScale2, EaseUtils.Ease(EaseUtils.EaseType.linear, 0F, 1F, val2))
						Me.items(itemIndex2).spriteShadowObject.transform.localScale = newScale2
						t2 += Time.deltaTime
						Yield Nothing
					End While
					Dim dustPoof2 As SpriteRenderer = Global.UnityEngine.[Object].Instantiate(Of SpriteRenderer)(Me.poofPrefab, Me.items(itemIndex2).transform.position + Me.items(itemIndex2).poofOffset, Quaternion.identity)
					AudioManager.Play("item_drop")
					dustPoof2.sortingOrder = 401
					Global.UnityEngine.[Object].Destroy(dustPoof2.gameObject, 3F)
					Yield Me.items(itemIndex2).TweenLocalPositionY(Me.items(itemIndex2).transform.position.y, Me.items(itemIndex2).transform.position.y + 30F, 0.1F, EaseUtils.EaseType.linear)
					Yield Me.items(itemIndex2).TweenLocalPositionY(Me.items(itemIndex2).transform.position.y, Me.items(itemIndex2).transform.position.y - 30F, 0.1F, EaseUtils.EaseType.linear)
					Yield CupheadTime.WaitForSeconds(Me, 0.2F)
					Me.weaponIndex = j + 1
					Me.UpdateSelection()
					Exit For
				End If
			Next
			If Not foundItem2 Then
				Me.CurrentItem.spriteShadowObject.gameObject.SetActive(False)
				Me.items.Remove(Me.CurrentItem)
				Me.UpdateSelection()
			End If
		End If
		Me.state = ShopScenePlayer.State.Selecting
		Return
	End Function

	' Token: 0x060044A1 RID: 17569 RVA: 0x00244D53 File Offset: 0x00243153
	Private Sub OnPlayerLeft(playerId As PlayerId)
		If playerId = Me.player Then
			Me.playerLeft = True
		End If
	End Sub

	' Token: 0x060044A2 RID: 17570 RVA: 0x00244D68 File Offset: 0x00243168
	Private Sub OnDestroy()
		Me.weaponItemPrefabs = Nothing
		Me.charmItemPrefabs = Nothing
		Me.currencyNbImage = Nothing
		Me.coinImage = Nothing
		Me.priceSprites = Nothing
		Me.poofPrefab = Nothing
		Me.items = Nothing
	End Sub

	' Token: 0x04004A30 RID: 18992
	Private Const DOOR_TIME As Single = 1F

	' Token: 0x04004A31 RID: 18993
	Private Const START_DELAY As Single = 1F

	' Token: 0x04004A32 RID: 18994
	<SerializeField()>
	Private player As PlayerId

	' Token: 0x04004A33 RID: 18995
	<Header("Visuals")>
	<SerializeField()>
	Private door As Transform

	' Token: 0x04004A34 RID: 18996
	<Header("Items")>
	<SerializeField()>
	Private items As List(Of ShopSceneItem)

	' Token: 0x04004A35 RID: 18997
	<SerializeField()>
	Private weaponItemPrefabs As ShopSceneItem()

	' Token: 0x04004A36 RID: 18998
	<SerializeField()>
	Private charmItemPrefabs As ShopSceneItem()

	' Token: 0x04004A37 RID: 18999
	<Header("UI Elements")>
	<SerializeField()>
	Private currencyText As TMP_Text

	' Token: 0x04004A38 RID: 19000
	<Space(10F)>
	<SerializeField()>
	Private displayNameText As TextMeshProUGUI

	' Token: 0x04004A39 RID: 19001
	<SerializeField()>
	Private subText As TextMeshProUGUI

	' Token: 0x04004A3A RID: 19002
	<SerializeField()>
	Private descriptionText As TextMeshProUGUI

	' Token: 0x04004A3B RID: 19003
	<SerializeField()>
	Private coinSprites As List(Of Sprite)

	' Token: 0x04004A3C RID: 19004
	<SerializeField()>
	Private currencyNbImage As Image

	' Token: 0x04004A3D RID: 19005
	<SerializeField()>
	Private coinImage As Image

	' Token: 0x04004A3E RID: 19006
	<SerializeField()>
	Private doubleDigitCoinPosition As Transform

	' Token: 0x04004A3F RID: 19007
	Private singleDigitCoinPosition As Vector3

	' Token: 0x04004A40 RID: 19008
	<SerializeField()>
	Private currencyCanvas As Transform

	' Token: 0x04004A41 RID: 19009
	<SerializeField()>
	Private currencyCanvasScaleValue As Single

	' Token: 0x04004A42 RID: 19010
	<SerializeField()>
	Private currencyCanvasMultiplier As Single

	' Token: 0x04004A43 RID: 19011
	<SerializeField()>
	Private poofPrefab As SpriteRenderer

	' Token: 0x04004A44 RID: 19012
	<SerializeField()>
	Private priceSprites As Sprite()

	' Token: 0x04004A45 RID: 19013
	<SerializeField()>
	Private priceSpriteRenderer As SpriteRenderer

	' Token: 0x04004A46 RID: 19014
	<SerializeField()>
	Private chalkCoinSpriteRenderer As SpriteRenderer

	' Token: 0x04004A47 RID: 19015
	Private input As Player

	' Token: 0x04004A48 RID: 19016
	Private doorPositionClosed As Single

	' Token: 0x04004A49 RID: 19017
	Private doorPositionOpen As Single

	' Token: 0x04004A4A RID: 19018
	Public state As ShopScenePlayer.State

	' Token: 0x04004A4B RID: 19019
	Private index As Integer

	' Token: 0x04004A4C RID: 19020
	Private currencyCanvasOriginalScale As Single

	' Token: 0x04004A4D RID: 19021
	Private scaleCoinCoroutine As Coroutine

	' Token: 0x04004A4E RID: 19022
	Private moveItemCantPurchaseCoroutine As Coroutine

	' Token: 0x04004A51 RID: 19025
	Private exitingShop As Boolean

	' Token: 0x04004A52 RID: 19026
	Private firstStart As Boolean = True

	' Token: 0x04004A53 RID: 19027
	Private playerLeft As Boolean

	' Token: 0x04004A54 RID: 19028
	Private isMoneyDoubleDigit As Boolean

	' Token: 0x04004A55 RID: 19029
	Private weaponIndex As Integer

	' Token: 0x04004A56 RID: 19030
	Private charmIndex As Integer

	' Token: 0x02000B0B RID: 2827
	Public Enum State
		' Token: 0x04004A58 RID: 19032
		Init
		' Token: 0x04004A59 RID: 19033
		Selecting
		' Token: 0x04004A5A RID: 19034
		Viewing
		' Token: 0x04004A5B RID: 19035
		Purchasing
		' Token: 0x04004A5C RID: 19036
		Exiting
		' Token: 0x04004A5D RID: 19037
		Exited
	End Enum
End Class
