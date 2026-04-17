Imports System
Imports System.Collections
Imports Rewired
Imports UnityEngine
Imports UnityEngine.UI

' Token: 0x02000989 RID: 2441
Public Class MapEquipUICard
	Inherits AbstractMonoBehaviour

	' Token: 0x170004A5 RID: 1189
	' (get) Token: 0x06003914 RID: 14612 RVA: 0x00206070 File Offset: 0x00204470
	' (set) Token: 0x06003915 RID: 14613 RVA: 0x00206078 File Offset: 0x00204478
	Public Property ReadyAndWaiting As Boolean

	' Token: 0x06003916 RID: 14614 RVA: 0x00206081 File Offset: 0x00204481
	Private Sub Start()
		AddHandler PlayerManager.OnPlayerJoinedEvent, AddressOf Me.OnPlayerJoined
		AddHandler PlayerManager.OnPlayerLeaveEvent, AddressOf Me.OnPlayerLeft
	End Sub

	' Token: 0x06003917 RID: 14615 RVA: 0x002060A5 File Offset: 0x002044A5
	Private Sub OnDestroy()
		RemoveHandler PlayerManager.OnPlayerJoinedEvent, AddressOf Me.OnPlayerJoined
		RemoveHandler PlayerManager.OnPlayerLeaveEvent, AddressOf Me.OnPlayerLeft
	End Sub

	' Token: 0x06003918 RID: 14616 RVA: 0x002060C9 File Offset: 0x002044C9
	Private Sub Update()
		Me.HandleInput()
	End Sub

	' Token: 0x06003919 RID: 14617 RVA: 0x002060D4 File Offset: 0x002044D4
	Private Sub HandleInput()
		If Me.playerInput Is Nothing OrElse Not Me.inputEnabled OrElse Me.equipUI.CurrentState <> AbstractEquipUI.ActiveState.Active OrElse InterruptingPrompt.IsInterrupting() Then
			Return
		End If
		Dim side As MapEquipUICard.Side = Me.side
		If side <> MapEquipUICard.Side.Front Then
			If side = MapEquipUICard.Side.Back Then
				Select Case Me.back
					Case MapEquipUICard.Back.[Select]
						Me.HandleInputBackSelect()
					Case MapEquipUICard.Back.Ready
						Me.HandleInputBackReady()
					Case MapEquipUICard.Back.Checklist
						Me.HandleInputChecklistReady()
				End Select
			End If
		Else
			Me.HandleInputFront()
		End If
	End Sub

	' Token: 0x0600391A RID: 14618 RVA: 0x00206188 File Offset: 0x00204588
	Private Sub HandleInputFront()
		If Me.playerInput.GetButtonDown(14) Then
			Me.Close()
			Return
		End If
		If Me.playerInput.GetButtonDown(18) Then
			Me.front.ChangeSelection(-1)
			Return
		End If
		If Me.playerInput.GetButtonDown(20) Then
			Me.front.ChangeSelection(1)
			Return
		End If
		If Me.front.checkListSelected Then
			If Me.playerInput.GetButtonDown(13) Then
				Dim num As Integer = 0
				If PlayerData.Data.CurrentMap = Scenes.scene_map_world_2 Then
					num = 1
				ElseIf PlayerData.Data.CurrentMap = Scenes.scene_map_world_3 Then
					num = 2
				ElseIf PlayerData.Data.CurrentMap = Scenes.scene_map_world_4 Then
					num = 3
				ElseIf PlayerData.Data.CurrentMap = Scenes.scene_map_world_DLC Then
					num = 4
				End If
				Me.checkList.SetCursorPosition(num, True)
				Me.RotateToCheckList()
				Return
			End If
		Else
			If Me.playerInput.GetButtonDown(13) Then
				Me.RotateToBackSelect(Me.front.Slot)
				Return
			End If
			If Me.playerInput.GetButtonDown(15) Then
				Me.front.Unequip()
				Return
			End If
		End If
	End Sub

	' Token: 0x0600391B RID: 14619 RVA: 0x002062C4 File Offset: 0x002046C4
	Private Sub HandleInputBackSelect()
		If Me.playerInput.GetButtonDown(14) Then
			Me.front.ChangeSelection(0)
			Me.RotateToFront()
			Return
		End If
		If Not Me.backSelect.lockInput Then
			If Me.playerInput.GetButtonDown(15) Then
				Me.backSelect.Unequip()
				Return
			End If
			If Me.playerInput.GetButtonDown(18) Then
				Me.backSelect.ChangeSelection(New Trilean2(-1, 0))
				Return
			End If
			If Me.playerInput.GetButtonDown(20) Then
				Me.backSelect.ChangeSelection(New Trilean2(1, 0))
				Return
			End If
			If Me.playerInput.GetButtonDown(16) Then
				Me.backSelect.ChangeSelection(New Trilean2(0, 1))
				Return
			End If
			If Me.playerInput.GetButtonDown(19) Then
				Me.backSelect.ChangeSelection(New Trilean2(0, -1))
				Return
			End If
			If Me.playerInput.GetButtonDown(11) Then
				AudioManager.Play("menu_equipment_page")
				Me.backSelect.ChangeSlot(1)
				Return
			End If
			If Me.playerInput.GetButtonDown(12) Then
				AudioManager.Play("menu_equipment_page")
				Me.backSelect.ChangeSlot(-1)
				Return
			End If
			If Me.playerInput.GetButtonDown(13) Then
				Me.backSelect.Accept()
				Return
			End If
		End If
	End Sub

	' Token: 0x0600391C RID: 14620 RVA: 0x00206428 File Offset: 0x00204828
	Private Sub HandleInputBackReady()
		If Me.playerInput.GetButtonDown(13) Then
			Me.RotateToFront()
			Return
		End If
		If Me.playerInput.GetButtonDown(15) Then
			Me.RotateToFront()
			Return
		End If
	End Sub

	' Token: 0x0600391D RID: 14621 RVA: 0x0020645C File Offset: 0x0020485C
	Private Sub HandleInputChecklistReady()
		If Me.playerInput.GetButtonDown(14) Then
			Me.RotateToFront()
			Return
		End If
		If Me.playerInput.GetButtonDown(18) Then
			Me.checkList.ChangeSelection(-1)
			Return
		End If
		If Me.playerInput.GetButtonDown(20) Then
			Me.checkList.ChangeSelection(1)
			Return
		End If
	End Sub

	' Token: 0x0600391E RID: 14622 RVA: 0x002064C0 File Offset: 0x002048C0
	Private Sub LateUpdate()
		MyBase.transform.localPosition = Vector2.Lerp(MyBase.transform.localPosition, Me.position, Time.deltaTime * 10F)
		MyBase.transform.localRotation = Quaternion.Lerp(MyBase.transform.localRotation, Quaternion.Euler(0F, 0F, Me.roll), Time.deltaTime * 8F)
		If Me.rotation > 90F Then
			Me.side = MapEquipUICard.Side.Back
			Me.SetBackActive(True)
			Me.front.SetActive(False)
		Else
			Me.side = MapEquipUICard.Side.Front
			Me.SetBackActive(False)
			Me.front.SetActive(True)
		End If
	End Sub

	' Token: 0x0600391F RID: 14623 RVA: 0x00206587 File Offset: 0x00204987
	Private Sub Close()
		If Not Me.equipUI.Close() Then
			Me.RotateToBackReady()
		End If
	End Sub

	' Token: 0x06003920 RID: 14624 RVA: 0x002065A0 File Offset: 0x002049A0
	Public Sub Init(id As PlayerId, equipUI As AbstractEquipUI)
		Me.playerID = id
		Me.equipUI = equipUI
		Me.playerInput = PlayerManager.GetPlayerInput(id)
		Me.backSelect.transform.SetScale(New Single?(-1F), Nothing, Nothing)
		Me.backReady.transform.SetScale(New Single?(-1F), Nothing, Nothing)
		Me.checkList.transform.SetScale(New Single?(-1F), Nothing, Nothing)
		For Each image As Image In Me.cupheadImages
			image.gameObject.SetActive(False)
		Next
		For Each image2 As Image In Me.mugmanImages
			image2.gameObject.SetActive(False)
		Next
		Me.cupheadChaos.SetActive(False)
		Me.mugmanChaos.SetActive(False)
		Dim playerId As PlayerId = Me.playerID
		If playerId <> PlayerId.PlayerOne Then
			If playerId <> PlayerId.PlayerTwo Then
				If playerId <> PlayerId.Any AndAlso playerId <> PlayerId.None Then
				End If
			Else
				Dim array3 As Image() = If((Not PlayerManager.player1IsMugman), Me.mugmanImages, Me.cupheadImages)
				For Each image3 As Image In array3
					image3.gameObject.SetActive(True)
				Next
				Dim gameObject As GameObject = If((Not PlayerManager.player1IsMugman), Me.cupheadChaos, Me.mugmanChaos)
				Me.cuphead2POverlay.SetActive(PlayerManager.player1IsMugman)
				Me.mugman1POverlay.SetActive(False)
				gameObject.SetActive(Localization.language <> Localization.Languages.English)
			End If
		Else
			Dim array5 As Image() = If((Not PlayerManager.player1IsMugman), Me.cupheadImages, Me.mugmanImages)
			For Each image4 As Image In array5
				image4.gameObject.SetActive(True)
			Next
			Dim gameObject2 As GameObject = If((Not PlayerManager.player1IsMugman), Me.cupheadChaos, Me.mugmanChaos)
			Me.mugman1POverlay.SetActive(PlayerManager.player1IsMugman)
			Me.cuphead2POverlay.SetActive(False)
			gameObject2.SetActive(Localization.language <> Localization.Languages.English)
		End If
		Me.front.Init(Me.playerID)
		Me.backSelect.Init(Me.playerID)
		Me.checkList.Init(Me.playerID)
	End Sub

	' Token: 0x06003921 RID: 14625 RVA: 0x00206870 File Offset: 0x00204C70
	Private Sub OnPlayerJoined(playerId As PlayerId)
		Me.SetActive(True)
		If Me.playerID = PlayerId.PlayerTwo Then
			Me.SetMultiplayerOut(True)
		End If
		Me.SetMultiplayerIn(False)
	End Sub

	' Token: 0x06003922 RID: 14626 RVA: 0x00206893 File Offset: 0x00204C93
	Private Sub OnPlayerLeft(playerId As PlayerId)
		If Me.playerID = PlayerId.PlayerTwo Then
			Me.SetMultiplayerOut(False)
			Return
		End If
		Me.SetSinglePlayerIn(False)
	End Sub

	' Token: 0x06003923 RID: 14627 RVA: 0x002068B0 File Offset: 0x00204CB0
	Public Sub SetActive(active As Boolean)
		If MyBase.gameObject Is Nothing Then
			Return
		End If
		MyBase.gameObject.SetActive(active)
	End Sub

	' Token: 0x06003924 RID: 14628 RVA: 0x002068D0 File Offset: 0x00204CD0
	Private Sub SetBackActive(active As Boolean)
		Me.backSelect.SetActive(False)
		Me.backReady.SetActive(False)
		Me.checkList.SetActive(False)
		If Not active Then
			Return
		End If
		Select Case Me.back
			Case MapEquipUICard.Back.[Select]
				Me.backSelect.SetActive(active)
			Case MapEquipUICard.Back.Ready
				Me.backReady.SetActive(active)
			Case MapEquipUICard.Back.Checklist
				Me.checkList.SetActive(active)
		End Select
	End Sub

	' Token: 0x06003925 RID: 14629 RVA: 0x0020695E File Offset: 0x00204D5E
	Public Sub SetSinglePlayerIn(Optional instant As Boolean = False)
		Me.ResetToFront()
		Me.SetPosition(Vector2.zero, instant)
		Me.SetRoll(Global.UnityEngine.Random.Range(-4F, 4F), instant)
	End Sub

	' Token: 0x06003926 RID: 14630 RVA: 0x00206988 File Offset: 0x00204D88
	Public Sub SetSinglePlayerOut(Optional instant As Boolean = False)
		Me.SetPosition(New Vector2(0F, -720F), instant)
		Me.SetRoll(0F, instant)
	End Sub

	' Token: 0x06003927 RID: 14631 RVA: 0x002069AC File Offset: 0x00204DAC
	Public Sub SetMultiplayerIn(Optional instant As Boolean = False)
		Me.ResetToFront()
		Dim vector As Vector2 = New Vector2(-320F, 0F)
		If Me.playerID = PlayerId.PlayerTwo Then
			vector.x *= -1F
		End If
		Me.SetPosition(vector, instant)
		Me.SetRoll(Global.UnityEngine.Random.Range(-4F, 4F), instant)
	End Sub

	' Token: 0x06003928 RID: 14632 RVA: 0x00206A10 File Offset: 0x00204E10
	Public Sub SetMultiplayerOut(Optional instant As Boolean = False)
		Dim vector As Vector2 = New Vector2(-1280F, 0F)
		If Me.playerID = PlayerId.PlayerTwo Then
			vector.x *= -1F
		End If
		Me.SetPosition(vector, instant)
		Me.SetRoll(0F, instant)
	End Sub

	' Token: 0x06003929 RID: 14633 RVA: 0x00206A61 File Offset: 0x00204E61
	Private Sub SetPosition(pos As Vector2, instant As Boolean)
		Me.position = pos
		If instant Then
			MyBase.transform.localPosition = Me.position
		End If
	End Sub

	' Token: 0x0600392A RID: 14634 RVA: 0x00206A86 File Offset: 0x00204E86
	Private Sub RotateToFront()
		AudioManager.Play("menu_cardflip")
		Me.front.Refresh()
		If Not Me.CanRotate Then
			Return
		End If
		Me.ReadyAndWaiting = False
		Me.StartRotation(Me.rotation, 0F)
	End Sub

	' Token: 0x0600392B RID: 14635 RVA: 0x00206AC1 File Offset: 0x00204EC1
	Private Sub ResetToFront()
		Me.StopRotation()
		Me.SetRotation(0F)
		Me.ReadyAndWaiting = False
	End Sub

	' Token: 0x0600392C RID: 14636 RVA: 0x00206ADB File Offset: 0x00204EDB
	Private Sub RotateToBackSelect(slot As MapEquipUICard.Slot)
		AudioManager.Play("menu_cardflip")
		Me.backSelect.Setup(slot)
		If Not Me.CanRotate Then
			Return
		End If
		Me.back = MapEquipUICard.Back.[Select]
		Me.StartRotation(Me.rotation, 180F)
	End Sub

	' Token: 0x0600392D RID: 14637 RVA: 0x00206B17 File Offset: 0x00204F17
	Private Sub RotateToBackReady()
		If Not Me.CanRotate Then
			Return
		End If
		Me.ReadyAndWaiting = True
		Me.back = MapEquipUICard.Back.Ready
		AudioManager.Play("menu_ready")
		Me.StartRotation(Me.rotation, 180F)
	End Sub

	' Token: 0x0600392E RID: 14638 RVA: 0x00206B4E File Offset: 0x00204F4E
	Private Sub RotateToCheckList()
		AudioManager.Play("menu_cardflip")
		If Not Me.CanRotate Then
			Return
		End If
		Me.back = MapEquipUICard.Back.Checklist
		Me.StartRotation(Me.rotation, 180F)
	End Sub

	' Token: 0x0600392F RID: 14639 RVA: 0x00206B7E File Offset: 0x00204F7E
	Private Sub StartRotation(start As Single, [end] As Single)
		Me.StopRotation()
		If Not Me.CanRotate Then
			Return
		End If
		Me.rotationCoroutine = Me.rotate_cr(start, [end], 0.15F)
		MyBase.StartCoroutine(Me.rotationCoroutine)
	End Sub

	' Token: 0x06003930 RID: 14640 RVA: 0x00206BB2 File Offset: 0x00204FB2
	Private Sub StopRotation()
		If Me.rotationCoroutine IsNot Nothing Then
			MyBase.StopCoroutine(Me.rotationCoroutine)
		End If
		Me.rotationCoroutine = Nothing
	End Sub

	' Token: 0x06003931 RID: 14641 RVA: 0x00206BD4 File Offset: 0x00204FD4
	Private Sub SetRotation(r As Single)
		Me.rotation = r
		Me.container.SetLocalEulerAngles(Nothing, New Single?(Me.rotation), Nothing)
	End Sub

	' Token: 0x06003932 RID: 14642 RVA: 0x00206C10 File Offset: 0x00205010
	Private Iterator Function rotate_cr(start As Single, [end] As Single, time As Single) As IEnumerator
		Me.inputEnabled = False
		Dim t As Single = 0F
		While t < time
			Dim val As Single = t / time
			Me.SetRotation(EaseUtils.Ease(Me.ROTATION_EASE, start, [end], val))
			t += Time.deltaTime
			Yield Nothing
		End While
		Me.SetRotation([end])
		Me.inputEnabled = True
		Return
	End Function

	' Token: 0x06003933 RID: 14643 RVA: 0x00206C40 File Offset: 0x00205040
	Private Sub SetRoll(r As Single, instant As Boolean)
		Me.roll = r
		If instant Then
			MyBase.transform.SetLocalEulerAngles(Nothing, Nothing, New Single?(r))
		End If
	End Sub

	' Token: 0x04004093 RID: 16531
	Private Const POSITION_SPEED As Single = 10F

	' Token: 0x04004094 RID: 16532
	Private Const ROLL_SPEED As Single = 8F

	' Token: 0x04004095 RID: 16533
	Public container As RectTransform

	' Token: 0x04004096 RID: 16534
	<Header("Cards")>
	<SerializeField()>
	Private front As MapEquipUICardFront

	' Token: 0x04004097 RID: 16535
	<SerializeField()>
	Private backSelect As MapEquipUICardBackSelect

	' Token: 0x04004098 RID: 16536
	<SerializeField()>
	Private backReady As MapEquipUICardBackReady

	' Token: 0x04004099 RID: 16537
	<SerializeField()>
	Private checkList As MapEquipUIChecklist

	' Token: 0x0400409A RID: 16538
	<Header("Sprite Assets")>
	Public cupheadImages As Image()

	' Token: 0x0400409B RID: 16539
	Public mugmanImages As Image()

	' Token: 0x0400409C RID: 16540
	<SerializeField()>
	Private cupheadChaos As GameObject

	' Token: 0x0400409D RID: 16541
	<SerializeField()>
	Private mugmanChaos As GameObject

	' Token: 0x0400409E RID: 16542
	<SerializeField()>
	Private cuphead2POverlay As GameObject

	' Token: 0x0400409F RID: 16543
	<SerializeField()>
	Private mugman1POverlay As GameObject

	' Token: 0x040040A0 RID: 16544
	<NonSerialized()>
	Public CanRotate As Boolean

	' Token: 0x040040A1 RID: 16545
	Private inputEnabled As Boolean = True

	' Token: 0x040040A2 RID: 16546
	Private equipUI As AbstractEquipUI

	' Token: 0x040040A3 RID: 16547
	Private side As MapEquipUICard.Side

	' Token: 0x040040A4 RID: 16548
	Private back As MapEquipUICard.Back

	' Token: 0x040040A5 RID: 16549
	Private position As Vector2

	' Token: 0x040040A6 RID: 16550
	Private rotation As Single

	' Token: 0x040040A7 RID: 16551
	Private roll As Single

	' Token: 0x040040A8 RID: 16552
	Private playerID As PlayerId

	' Token: 0x040040A9 RID: 16553
	Private playerInput As Player

	' Token: 0x040040AB RID: 16555
	Private Const ROTATION_FRONT As Single = 0F

	' Token: 0x040040AC RID: 16556
	Private Const ROTATION_BACK As Single = 180F

	' Token: 0x040040AD RID: 16557
	Private Const ROTATION_TIME As Single = 0.15F

	' Token: 0x040040AE RID: 16558
	Private ROTATION_EASE As EaseUtils.EaseType = EaseUtils.EaseType.easeOutBack

	' Token: 0x040040AF RID: 16559
	Private rotationCoroutine As IEnumerator

	' Token: 0x040040B0 RID: 16560
	Private Const ROLL_MIN As Single = 1F

	' Token: 0x040040B1 RID: 16561
	Private Const ROLL_MAX As Single = 4F

	' Token: 0x0200098A RID: 2442
	Public Enum Side
		' Token: 0x040040B3 RID: 16563
		Front
		' Token: 0x040040B4 RID: 16564
		Back
	End Enum

	' Token: 0x0200098B RID: 2443
	Public Enum Slot
		' Token: 0x040040B6 RID: 16566
		SHOT_A
		' Token: 0x040040B7 RID: 16567
		SHOT_B
		' Token: 0x040040B8 RID: 16568
		SUPER
		' Token: 0x040040B9 RID: 16569
		CHARM
	End Enum

	' Token: 0x0200098C RID: 2444
	Public Enum Back
		' Token: 0x040040BB RID: 16571
		[Select]
		' Token: 0x040040BC RID: 16572
		Ready
		' Token: 0x040040BD RID: 16573
		Checklist
	End Enum
End Class
