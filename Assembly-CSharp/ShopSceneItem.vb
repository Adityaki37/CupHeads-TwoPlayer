Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000B06 RID: 2822
Public Class ShopSceneItem
	Inherits AbstractMonoBehaviour

	' Token: 0x1700061E RID: 1566
	' (get) Token: 0x06004471 RID: 17521 RVA: 0x00243780 File Offset: 0x00241B80
	' (set) Token: 0x06004472 RID: 17522 RVA: 0x00243788 File Offset: 0x00241B88
	Public Property state As ShopSceneItem.State

	' Token: 0x1700061F RID: 1567
	' (get) Token: 0x06004473 RID: 17523 RVA: 0x00243791 File Offset: 0x00241B91
	Public ReadOnly Property Purchased As Boolean
		Get
			Return Me.isPurchased(Me.player)
		End Get
	End Property

	' Token: 0x06004474 RID: 17524 RVA: 0x002437A0 File Offset: 0x00241BA0
	Private Function isPurchased(player As PlayerId) As Boolean
		Select Case Me.itemType
			Case ItemType.Weapon
				Return PlayerData.Data.IsUnlocked(player, Me.weapon)
			Case ItemType.Super
				Return PlayerData.Data.IsUnlocked(player, Me.super)
			Case ItemType.Charm
				Return PlayerData.Data.IsUnlocked(player, Me.charm)
			Case Else
				Return False
		End Select
	End Function

	' Token: 0x06004475 RID: 17525 RVA: 0x00243802 File Offset: 0x00241C02
	Public Function isPurchasedForBuyAllItemsAchievement(player As PlayerId) As Boolean
		Return Me.isDLCItem OrElse Me.isPurchased(player)
	End Function

	' Token: 0x17000620 RID: 1568
	' (get) Token: 0x06004476 RID: 17526 RVA: 0x00243818 File Offset: 0x00241C18
	Public ReadOnly Property DisplayName As String
		Get
			Select Case Me.itemType
				Case ItemType.Weapon
					Return WeaponProperties.GetDisplayName(Me.weapon)
				Case ItemType.Super
					Return WeaponProperties.GetDisplayName(Me.super)
				Case ItemType.Charm
					Return WeaponProperties.GetDisplayName(Me.charm)
				Case Else
					Return String.Empty
			End Select
		End Get
	End Property

	' Token: 0x17000621 RID: 1569
	' (get) Token: 0x06004477 RID: 17527 RVA: 0x0024386C File Offset: 0x00241C6C
	Public ReadOnly Property Subtext As String
		Get
			Select Case Me.itemType
				Case ItemType.Weapon
					Return WeaponProperties.GetSubtext(Me.weapon)
				Case ItemType.Super
					Return WeaponProperties.GetSubtext(Me.super)
				Case ItemType.Charm
					Return WeaponProperties.GetSubtext(Me.charm)
				Case Else
					Return String.Empty
			End Select
		End Get
	End Property

	' Token: 0x17000622 RID: 1570
	' (get) Token: 0x06004478 RID: 17528 RVA: 0x002438C0 File Offset: 0x00241CC0
	Public ReadOnly Property Description As String
		Get
			Select Case Me.itemType
				Case ItemType.Weapon
					Return WeaponProperties.GetDescription(Me.weapon)
				Case ItemType.Super
					Return WeaponProperties.GetDescription(Me.super)
				Case ItemType.Charm
					Return WeaponProperties.GetDescription(Me.charm)
				Case Else
					Return String.Empty
			End Select
		End Get
	End Property

	' Token: 0x17000623 RID: 1571
	' (get) Token: 0x06004479 RID: 17529 RVA: 0x00243914 File Offset: 0x00241D14
	Public ReadOnly Property Value As Integer
		Get
			Select Case Me.itemType
				Case ItemType.Weapon
					Return WeaponProperties.GetValue(Me.weapon)
				Case ItemType.Super
					Return WeaponProperties.GetValue(Me.super)
				Case ItemType.Charm
					Return WeaponProperties.GetValue(Me.charm)
				Case Else
					Return 0
			End Select
		End Get
	End Property

	' Token: 0x17000624 RID: 1572
	' (get) Token: 0x0600447A RID: 17530 RVA: 0x00243964 File Offset: 0x00241D64
	Public ReadOnly Property IsAvailable As Boolean
		Get
			Return Not Me.isDLCItem OrElse (Me.isDLCItem AndAlso DLCManager.DLCEnabled())
		End Get
	End Property

	' Token: 0x0600447B RID: 17531 RVA: 0x00243988 File Offset: 0x00241D88
	Public Sub Init(player As PlayerId)
		Me.startPosition = MyBase.transform.localPosition
		Me.endPosition = Me.startPosition
		Me.endPosition.y = Me.endPosition.y + 40F
		Me.player = player
		If Me.Purchased Then
			Me.SetSprite(ShopSceneItem.SpriteState.Purchased)
		Else
			Me.SetSprite(ShopSceneItem.SpriteState.Inactive)
		End If
	End Sub

	' Token: 0x0600447C RID: 17532 RVA: 0x002439F0 File Offset: 0x00241DF0
	Private Sub SetSprite(spriteState As ShopSceneItem.SpriteState)
		Me.spriteInactive.enabled = False
		Me.spriteSelected.enabled = False
		Me.spritePurchased.enabled = False
		Select Case spriteState
			Case ShopSceneItem.SpriteState.Inactive
				Me.spriteInactive.enabled = True
			Case ShopSceneItem.SpriteState.Selected
				Me.spriteSelected.enabled = True
			Case ShopSceneItem.SpriteState.Purchased
				Me.spritePurchased.enabled = True
		End Select
	End Sub

	' Token: 0x0600447D RID: 17533 RVA: 0x00243A70 File Offset: 0x00241E70
	Public Sub [Select]()
		If Me.state <> ShopSceneItem.State.Ready Then
			Return
		End If
		If Not Me.Purchased Then
			Me.SetSprite(ShopSceneItem.SpriteState.Selected)
		End If
		Me.StopAllCoroutines()
		MyBase.StartCoroutine(Me.float_cr(MyBase.transform.localPosition, Me.endPosition, Me.spriteShadowObject.transform.localScale, Me.originalShadowScale * 0.8F))
	End Sub

	' Token: 0x0600447E RID: 17534 RVA: 0x00243AE0 File Offset: 0x00241EE0
	Public Sub Deselect()
		If Me.state <> ShopSceneItem.State.Ready Then
			Return
		End If
		If Not Me.Purchased Then
			Me.SetSprite(ShopSceneItem.SpriteState.Inactive)
		End If
		Me.StopAllCoroutines()
		MyBase.StartCoroutine(Me.float_cr(MyBase.transform.localPosition, Me.startPosition, Me.spriteShadowObject.transform.localScale, Me.originalShadowScale))
	End Sub

	' Token: 0x0600447F RID: 17535 RVA: 0x00243B45 File Offset: 0x00241F45
	Private Sub UpdateFloat(value As Single)
		MyBase.transform.localPosition = Vector3.Lerp(Me.startPosition, Me.endPosition, value)
	End Sub

	' Token: 0x06004480 RID: 17536 RVA: 0x00243B64 File Offset: 0x00241F64
	Private Sub UpdatePurchasedColor(value As Single)
		Dim white As Color = Color.white
		Dim black As Color = Color.black
		Me.spritePurchased.color = Color.Lerp(white, black, value)
	End Sub

	' Token: 0x06004481 RID: 17537 RVA: 0x00243B90 File Offset: 0x00241F90
	Public Function Purchase() As Boolean
		If Me.state <> ShopSceneItem.State.Ready Then
			Return False
		End If
		If Me.Purchased Then
			Return False
		End If
		Dim flag As Boolean = False
		Select Case Me.itemType
			Case ItemType.Weapon
				flag = PlayerData.Data.Buy(Me.player, Me.weapon)
			Case ItemType.Super
				flag = PlayerData.Data.Buy(Me.player, Me.super)
			Case ItemType.Charm
				flag = PlayerData.Data.Buy(Me.player, Me.charm)
		End Select
		If flag Then
			MyBase.StartCoroutine(Me.purchase_cr())
			If ShopScene.Current.HasBoughtEverythingForAchievement(Me.player) Then
				OnlineManager.Instance.[Interface].UnlockAchievement(Me.player, "BoughtAllItems")
			End If
			If Not PlayerData.Data.hasMadeFirstPurchase Then
				PlayerData.Data.shouldShowShopkeepTooltip = True
				PlayerData.Data.hasMadeFirstPurchase = True
				PlayerData.SaveCurrentFile()
			End If
		End If
		Return flag
	End Function

	' Token: 0x06004482 RID: 17538 RVA: 0x00243CA0 File Offset: 0x002420A0
	Private Iterator Function float_cr(start As Vector3, [end] As Vector3, startShadowScale As Vector3, endShadowScale As Vector3) As IEnumerator
		Dim t As Single = 0F
		Dim time As Single = 0.3F * (Vector3.Distance(start, [end]) / Vector3.Distance(Me.startPosition, Me.endPosition))
		While t < time
			Dim val As Single = t / time
			MyBase.transform.localPosition = Vector3.Lerp(start, [end], EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, 0F, 1F, val))
			Me.spriteShadowObject.transform.localScale = Vector3.Lerp(startShadowScale, endShadowScale, EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, 0F, 1F, val))
			t += MyBase.LocalDeltaTime
			Yield Nothing
		End While
		MyBase.transform.localPosition = [end]
		Yield Nothing
		Return
	End Function

	' Token: 0x06004483 RID: 17539 RVA: 0x00243CD8 File Offset: 0x002420D8
	Private Iterator Function purchase_cr() As IEnumerator
		Me.state = ShopSceneItem.State.Busy
		Me.SetSprite(ShopSceneItem.SpriteState.Purchased)
		Dim buyAnim As SpriteRenderer = Global.UnityEngine.[Object].Instantiate(Of SpriteRenderer)(Me.buyAnimation, MyBase.GetComponentInChildren(Of SpriteRenderer)().bounds.center, Quaternion.identity)
		buyAnim.sortingOrder = MyBase.GetComponentInChildren(Of SpriteRenderer)().sortingOrder
		Me.spriteShadowObject.gameObject.SetActive(False)
		Yield MyBase.TweenValue(0F, 1F, 0.0001F, EaseUtils.EaseType.linear, AddressOf Me.UpdatePurchasedColor)
		Me.state = ShopSceneItem.State.Ready
		MyBase.gameObject.SetActive(False)
		Return
	End Function

	' Token: 0x06004484 RID: 17540 RVA: 0x00243CF3 File Offset: 0x002420F3
	Private Sub OnDestroy()
		Me.buyAnimation = Nothing
		Me.spriteShadow = Nothing
	End Sub

	' Token: 0x04004A10 RID: 18960
	Private Const FLOAT_TIME As Single = 0.3F

	' Token: 0x04004A11 RID: 18961
	Public itemType As ItemType

	' Token: 0x04004A12 RID: 18962
	<Space(5F)>
	Public weapon As Weapon = Weapon.None

	' Token: 0x04004A13 RID: 18963
	Public super As Super = Super.None

	' Token: 0x04004A14 RID: 18964
	Public charm As Charm = Charm.None

	' Token: 0x04004A15 RID: 18965
	<Header("Sprites")>
	Public spriteInactive As SpriteRenderer

	' Token: 0x04004A16 RID: 18966
	Public spriteSelected As SpriteRenderer

	' Token: 0x04004A17 RID: 18967
	Public spritePurchased As SpriteRenderer

	' Token: 0x04004A18 RID: 18968
	Public spriteShadowObject As SpriteRenderer

	' Token: 0x04004A19 RID: 18969
	Public spriteShadow As Sprite

	' Token: 0x04004A1A RID: 18970
	Public cantPurchaseYMovementPosition As Single

	' Token: 0x04004A1B RID: 18971
	Public cantPurchaseYMovementValue As Single

	' Token: 0x04004A1C RID: 18972
	Public poofOffset As Vector3

	' Token: 0x04004A1D RID: 18973
	<HideInInspector()>
	Public endPosition As Vector3

	' Token: 0x04004A1E RID: 18974
	Public player As PlayerId

	' Token: 0x04004A1F RID: 18975
	<HideInInspector()>
	Public startPosition As Vector3

	' Token: 0x04004A20 RID: 18976
	Public originalShadowScale As Vector3

	' Token: 0x04004A21 RID: 18977
	Public buyAnimation As SpriteRenderer

	' Token: 0x04004A22 RID: 18978
	Private selectionCoroutine As Coroutine

	' Token: 0x04004A24 RID: 18980
	<SerializeField()>
	Private isDLCItem As Boolean

	' Token: 0x02000B07 RID: 2823
	Public Enum State
		' Token: 0x04004A26 RID: 18982
		Ready
		' Token: 0x04004A27 RID: 18983
		Busy
	End Enum

	' Token: 0x02000B08 RID: 2824
	Public Enum SpriteState
		' Token: 0x04004A29 RID: 18985
		Inactive
		' Token: 0x04004A2A RID: 18986
		Selected
		' Token: 0x04004A2B RID: 18987
		Purchased
	End Enum
End Class
