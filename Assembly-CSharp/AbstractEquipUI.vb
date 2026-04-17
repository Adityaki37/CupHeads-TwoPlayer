Imports System
Imports UnityEngine

' Token: 0x02000983 RID: 2435
Public MustInherit Class AbstractEquipUI
	Inherits AbstractPauseGUI

	' Token: 0x1700049E RID: 1182
	' (get) Token: 0x060038DB RID: 14555 RVA: 0x00205554 File Offset: 0x00203954
	' (set) Token: 0x060038DC RID: 14556 RVA: 0x0020555B File Offset: 0x0020395B
	Public Shared Property Current As AbstractEquipUI

	' Token: 0x1700049F RID: 1183
	' (get) Token: 0x060038DD RID: 14557 RVA: 0x00205563 File Offset: 0x00203963
	' (set) Token: 0x060038DE RID: 14558 RVA: 0x0020556B File Offset: 0x0020396B
	Public Property CurrentState As AbstractEquipUI.ActiveState

	' Token: 0x170004A0 RID: 1184
	' (get) Token: 0x060038DF RID: 14559 RVA: 0x00205574 File Offset: 0x00203974
	Protected Overrides ReadOnly Property CheckedActionSet As AbstractPauseGUI.InputActionSet
		Get
			Return AbstractPauseGUI.InputActionSet.UIInput
		End Get
	End Property

	' Token: 0x170004A1 RID: 1185
	' (get) Token: 0x060038E0 RID: 14560 RVA: 0x00205577 File Offset: 0x00203977
	Protected Overrides ReadOnly Property CanPause As Boolean
		Get
			Return False
		End Get
	End Property

	' Token: 0x170004A2 RID: 1186
	' (get) Token: 0x060038E1 RID: 14561 RVA: 0x0020557A File Offset: 0x0020397A
	Protected Overrides ReadOnly Property CanUnpause As Boolean
		Get
			Return False
		End Get
	End Property

	' Token: 0x060038E2 RID: 14562 RVA: 0x0020557D File Offset: 0x0020397D
	Protected Overrides Sub InAnimation(i As Single)
	End Sub

	' Token: 0x060038E3 RID: 14563 RVA: 0x0020557F File Offset: 0x0020397F
	Protected Overrides Sub OutAnimation(i As Single)
	End Sub

	' Token: 0x060038E4 RID: 14564 RVA: 0x00205584 File Offset: 0x00203984
	Protected Overrides Sub Awake()
		MyBase.Awake()
		AbstractEquipUI.Current = Me
		Me.playerTwo = Global.UnityEngine.[Object].Instantiate(Of MapEquipUICard)(Me.playerOne)
		Me.playerTwo.transform.SetParent(Me.playerOne.transform.parent, False)
		Me.playerTwo.Init(PlayerId.PlayerTwo, Me)
		Me.playerTwo.name = "PlayerTwo"
		Me.playerOne.transform.SetSiblingIndex(Me.playerTwo.transform.GetSiblingIndex())
		Me.playerOne.Init(PlayerId.PlayerOne, Me)
	End Sub

	' Token: 0x060038E5 RID: 14565 RVA: 0x0020561C File Offset: 0x00203A1C
	Private Sub Start()
		AddHandler PlayerManager.OnPlayerJoinedEvent, AddressOf Me.OnPlayerJoined
		AddHandler PlayerManager.OnPlayerLeaveEvent, AddressOf Me.OnPlayerLeft
		If PlayerManager.Multiplayer Then
			Dim anchoredPosition As Vector2 = Me.playerOne.container.anchoredPosition
			Me.playerOne.container.anchoredPosition = anchoredPosition
			Me.playerTwo.container.anchoredPosition = anchoredPosition
		End If
	End Sub

	' Token: 0x060038E6 RID: 14566 RVA: 0x00205688 File Offset: 0x00203A88
	Private Sub OnDestroy()
		RemoveHandler PlayerManager.OnPlayerJoinedEvent, AddressOf Me.OnPlayerJoined
		RemoveHandler PlayerManager.OnPlayerLeaveEvent, AddressOf Me.OnPlayerLeft
		If AbstractEquipUI.Current Is Me Then
			AbstractEquipUI.Current = Nothing
		End If
	End Sub

	' Token: 0x060038E7 RID: 14567 RVA: 0x002056C4 File Offset: 0x00203AC4
	Private Sub OnPlayerJoined(playerId As PlayerId)
		Dim anchoredPosition As Vector2 = Me.playerOne.container.anchoredPosition
		anchoredPosition.y += 10F
		Me.playerOne.container.anchoredPosition = anchoredPosition
		Me.playerTwo.container.anchoredPosition = anchoredPosition
	End Sub

	' Token: 0x060038E8 RID: 14568 RVA: 0x00205718 File Offset: 0x00203B18
	Private Sub OnPlayerLeft(playerId As PlayerId)
		Dim anchoredPosition As Vector2 = Me.playerOne.container.anchoredPosition
		anchoredPosition.y -= 10F
		Me.playerOne.container.anchoredPosition = anchoredPosition
		Me.playerTwo.container.anchoredPosition = anchoredPosition
	End Sub

	' Token: 0x060038E9 RID: 14569 RVA: 0x0020576C File Offset: 0x00203B6C
	Protected Overrides Sub OnPause()
		If PlatformHelper.GarbageCollectOnPause Then
			GC.Collect()
		End If
		Me.OnPauseAudio()
		MyBase.FrameDelayedCallback(AddressOf Me.SetStateActive, 1)
		Me.playerOne.CanRotate = False
		Me.playerTwo.CanRotate = False
		AudioManager.Play("menu_cardup")
		If PlayerManager.Multiplayer Then
			Me.playerOne.SetActive(True)
			Me.playerTwo.SetActive(True)
			Me.playerOne.SetMultiplayerOut(True)
			Me.playerTwo.SetMultiplayerOut(True)
			Me.playerOne.SetMultiplayerIn(False)
			Me.playerTwo.SetMultiplayerIn(False)
		Else
			Me.playerOne.SetActive(True)
			Me.playerTwo.SetActive(False)
			Me.playerOne.SetSinglePlayerOut(True)
			Me.playerOne.SetSinglePlayerIn(False)
		End If
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, False, False)
		PlayerData.Data.ResetHasNewPurchase(PlayerId.Any)
	End Sub

	' Token: 0x060038EA RID: 14570 RVA: 0x00205862 File Offset: 0x00203C62
	Private Sub SetStateActive()
		Me.CurrentState = AbstractEquipUI.ActiveState.Active
	End Sub

	' Token: 0x060038EB RID: 14571 RVA: 0x0020586B File Offset: 0x00203C6B
	Protected Overrides Sub OnPauseComplete()
		MyBase.OnPauseComplete()
		Me.playerOne.CanRotate = True
		Me.playerTwo.CanRotate = True
	End Sub

	' Token: 0x060038EC RID: 14572 RVA: 0x0020588C File Offset: 0x00203C8C
	Protected Overrides Sub OnUnpause()
		Me.OnUnpauseAudio()
		MyBase.OnUnpause()
		Me.playerOne.CanRotate = False
		Me.playerTwo.CanRotate = False
		If PlayerManager.Multiplayer Then
			Me.playerOne.SetMultiplayerOut(False)
			Me.playerTwo.SetMultiplayerOut(False)
		Else
			Me.playerOne.SetSinglePlayerOut(False)
		End If
	End Sub

	' Token: 0x060038ED RID: 14573 RVA: 0x002058F0 File Offset: 0x00203CF0
	Protected Overridable Sub OnPauseAudio()
	End Sub

	' Token: 0x060038EE RID: 14574 RVA: 0x002058F2 File Offset: 0x00203CF2
	Protected Overridable Sub OnUnpauseAudio()
	End Sub

	' Token: 0x060038EF RID: 14575 RVA: 0x002058F4 File Offset: 0x00203CF4
	Protected Overrides Sub OnUnpauseComplete()
		MyBase.OnUnpauseComplete()
		Me.CurrentState = AbstractEquipUI.ActiveState.Inactive
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, True, True)
	End Sub

	' Token: 0x060038F0 RID: 14576 RVA: 0x0020590C File Offset: 0x00203D0C
	Public Function Close() As Boolean
		If PlayerManager.Multiplayer AndAlso Not Me.playerOne.ReadyAndWaiting AndAlso Not Me.playerTwo.ReadyAndWaiting Then
			Return False
		End If
		If Map.Current IsNot Nothing Then
			Map.Current.OnCloseEquipMenu()
		End If
		AudioManager.Play("menu_carddown")
		Me.Unpause()
		Return True
	End Function

	' Token: 0x04004082 RID: 16514
	<SerializeField()>
	Private playerOne As MapEquipUICard

	' Token: 0x04004083 RID: 16515
	Private playerTwo As MapEquipUICard

	' Token: 0x02000984 RID: 2436
	Public Enum ActiveState
		' Token: 0x04004086 RID: 16518
		Inactive
		' Token: 0x04004087 RID: 16519
		Active
	End Enum
End Class
