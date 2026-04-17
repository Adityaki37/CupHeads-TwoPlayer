Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000B03 RID: 2819
Public Class ShopScene
	Inherits AbstractMonoBehaviour

	' Token: 0x1700061D RID: 1565
	' (get) Token: 0x0600445D RID: 17501 RVA: 0x00242FF9 File Offset: 0x002413F9
	' (set) Token: 0x0600445E RID: 17502 RVA: 0x00243000 File Offset: 0x00241400
	Public Shared Property Current As ShopScene

	' Token: 0x0600445F RID: 17503 RVA: 0x00243008 File Offset: 0x00241408
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Cuphead.Init(False)
		ShopScene.Current = Me
		AddHandler Me.playerOne.OnPurchaseEvent, AddressOf Me.OnPurchase
		AddHandler Me.playerTwo.OnPurchaseEvent, AddressOf Me.OnPurchase
		AddHandler Me.playerOne.OnExitEvent, AddressOf Me.OnExit
		AddHandler Me.playerTwo.OnExitEvent, AddressOf Me.OnExit
		AddHandler SceneLoader.OnFadeOutEndEvent, AddressOf Me.OnLoaded
	End Sub

	' Token: 0x06004460 RID: 17504 RVA: 0x00243094 File Offset: 0x00241494
	Private Sub OnDestroy()
		If ShopScene.Current Is Me Then
			ShopScene.Current = Nothing
		End If
		RemoveHandler SceneLoader.OnFadeOutEndEvent, AddressOf Me.OnLoaded
	End Sub

	' Token: 0x06004461 RID: 17505 RVA: 0x002430BD File Offset: 0x002414BD
	Private Sub OnLoaded()
		Me.pig.OnStart()
		Me.playerOne.OnStart()
		Me.playerTwo.OnStart()
		InterruptingPrompt.SetCanInterrupt(True)
	End Sub

	' Token: 0x06004462 RID: 17506 RVA: 0x002430E6 File Offset: 0x002414E6
	Private Sub OnPurchase()
		Me.pig.OnPurchase()
	End Sub

	' Token: 0x06004463 RID: 17507 RVA: 0x002430F4 File Offset: 0x002414F4
	Private Sub OnExit()
		If(Not Me.playerOne.gameObject.activeInHierarchy OrElse Me.playerOne.state = ShopScenePlayer.State.Exiting OrElse Me.playerOne.state = ShopScenePlayer.State.Exited) AndAlso (Not Me.playerTwo.gameObject.activeInHierarchy OrElse Me.playerTwo.state = ShopScenePlayer.State.Exiting OrElse Me.playerTwo.state = ShopScenePlayer.State.Exited) Then
			MyBase.StartCoroutine(Me.exit_cr())
			Me.playerOne.OnExit()
			Me.playerTwo.OnExit()
		End If
	End Sub

	' Token: 0x06004464 RID: 17508 RVA: 0x00243194 File Offset: 0x00241594
	Private Iterator Function exit_cr() As IEnumerator
		If Me.HasBoughtEverythingForAchievement(PlayerId.PlayerOne) Then
			OnlineManager.Instance.[Interface].UnlockAchievement(PlayerId.PlayerOne, "BoughtAllItems")
		End If
		If Me.HasBoughtEverythingForAchievement(PlayerId.PlayerTwo) Then
			OnlineManager.Instance.[Interface].UnlockAchievement(PlayerId.PlayerTwo, "BoughtAllItems")
		End If
		Me.pig.OnExit()
		Yield Me.pig.animator.WaitForAnimationToEnd(Me, "Bye", False, True)
		SceneLoader.LoadLastMap()
		Return
	End Function

	' Token: 0x06004465 RID: 17509 RVA: 0x002431AF File Offset: 0x002415AF
	Public Function GetCharmItems(player As PlayerId) As ShopSceneItem()
		If player = PlayerId.PlayerTwo Then
			Return Me.playerTwo.GetCharmItemPrefabs()
		End If
		Return Me.playerOne.GetCharmItemPrefabs()
	End Function

	' Token: 0x06004466 RID: 17510 RVA: 0x002431CF File Offset: 0x002415CF
	Public Function GetWeaponItems(player As PlayerId) As ShopSceneItem()
		If player = PlayerId.PlayerTwo Then
			Return Me.playerTwo.GetWeaponItemPrefabs()
		End If
		Return Me.playerOne.GetWeaponItemPrefabs()
	End Function

	' Token: 0x06004467 RID: 17511 RVA: 0x002431F0 File Offset: 0x002415F0
	Public Function HasBoughtEverythingForAchievement(player As PlayerId) As Boolean
		Dim array As ShopSceneItem() = Me.GetCharmItems(player)
		For i As Integer = 0 To array.Length - 1
			If Not array(i).isPurchasedForBuyAllItemsAchievement(player) Then
				Return False
			End If
		Next
		array = Me.GetWeaponItems(player)
		For j As Integer = 0 To array.Length - 1
			If Not array(j).isPurchasedForBuyAllItemsAchievement(player) Then
				Return False
			End If
		Next
		Return True
	End Function

	' Token: 0x040049FF RID: 18943
	<SerializeField()>
	Private playerOne As ShopScenePlayer

	' Token: 0x04004A00 RID: 18944
	<SerializeField()>
	Private playerTwo As ShopScenePlayer

	' Token: 0x04004A01 RID: 18945
	<Space(10F)>
	<SerializeField()>
	Private pig As ShopScenePig

	' Token: 0x04004A02 RID: 18946
	Public isDLCShop As Boolean
End Class
