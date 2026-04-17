Imports System
Imports System.Collections
Imports System.Diagnostics
Imports UnityEngine

' Token: 0x02000976 RID: 2422
Public Class MapPlayerController
	Inherits MapSprite

	' Token: 0x14000068 RID: 104
	' (add) Token: 0x06003871 RID: 14449 RVA: 0x00203F3C File Offset: 0x0020233C
	' (remove) Token: 0x06003872 RID: 14450 RVA: 0x00203F70 File Offset: 0x00202370
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Shared Event OnEquipMenuOpenedEvent As Action

	' Token: 0x14000069 RID: 105
	' (add) Token: 0x06003873 RID: 14451 RVA: 0x00203FA4 File Offset: 0x002023A4
	' (remove) Token: 0x06003874 RID: 14452 RVA: 0x00203FD8 File Offset: 0x002023D8
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Shared Event OnEquipMenuClosedEvent As Action

	' Token: 0x06003875 RID: 14453 RVA: 0x0020400C File Offset: 0x0020240C
	Public Shared Function CanMove() As Boolean
		Return MapDifficultySelectStartUI.Current.CurrentState = AbstractMapSceneStartUI.State.Inactive AndAlso MapConfirmStartUI.Current.CurrentState = AbstractMapSceneStartUI.State.Inactive AndAlso MapBasicStartUI.Current.CurrentState = AbstractMapSceneStartUI.State.Inactive AndAlso (Not SceneLoader.Exists OrElse (Not SceneLoader.IsInIrisTransition AndAlso Not SceneLoader.IsInBlurTransition)) AndAlso (Not(Map.Current IsNot Nothing) OrElse Map.Current.CurrentState <> Map.State.Graveyard) AndAlso (Not MapEventNotification.Current OrElse Not MapEventNotification.Current.showing)
	End Function

	' Token: 0x17000491 RID: 1169
	' (get) Token: 0x06003876 RID: 14454 RVA: 0x002040A6 File Offset: 0x002024A6
	' (set) Token: 0x06003877 RID: 14455 RVA: 0x002040AE File Offset: 0x002024AE
	Public Property state As MapPlayerController.State

	' Token: 0x17000492 RID: 1170
	' (get) Token: 0x06003878 RID: 14456 RVA: 0x002040B7 File Offset: 0x002024B7
	' (set) Token: 0x06003879 RID: 14457 RVA: 0x002040BF File Offset: 0x002024BF
	Public Property id As PlayerId

	' Token: 0x17000493 RID: 1171
	' (get) Token: 0x0600387A RID: 14458 RVA: 0x002040C8 File Offset: 0x002024C8
	' (set) Token: 0x0600387B RID: 14459 RVA: 0x002040D0 File Offset: 0x002024D0
	Public Property EquipMenuOpen As Boolean

	' Token: 0x17000494 RID: 1172
	' (get) Token: 0x0600387C RID: 14460 RVA: 0x002040D9 File Offset: 0x002024D9
	' (set) Token: 0x0600387D RID: 14461 RVA: 0x002040E1 File Offset: 0x002024E1
	Public Property input As PlayerInput

	' Token: 0x17000495 RID: 1173
	' (get) Token: 0x0600387E RID: 14462 RVA: 0x002040EA File Offset: 0x002024EA
	' (set) Token: 0x0600387F RID: 14463 RVA: 0x002040F2 File Offset: 0x002024F2
	Public Property motor As MapPlayerMotor

	' Token: 0x17000496 RID: 1174
	' (get) Token: 0x06003880 RID: 14464 RVA: 0x002040FB File Offset: 0x002024FB
	' (set) Token: 0x06003881 RID: 14465 RVA: 0x00204103 File Offset: 0x00202503
	Public Property animationController As MapPlayerAnimationController

	' Token: 0x17000497 RID: 1175
	' (get) Token: 0x06003882 RID: 14466 RVA: 0x0020410C File Offset: 0x0020250C
	' (set) Token: 0x06003883 RID: 14467 RVA: 0x00204114 File Offset: 0x00202514
	Public Property ladderManager As MapPlayerLadderManager

	' Token: 0x17000498 RID: 1176
	' (get) Token: 0x06003884 RID: 14468 RVA: 0x0020411D File Offset: 0x0020251D
	Public ReadOnly Property Velocity As Vector2
		Get
			Return Me.motor.velocity
		End Get
	End Property

	' Token: 0x17000499 RID: 1177
	' (get) Token: 0x06003885 RID: 14469 RVA: 0x0020412A File Offset: 0x0020252A
	Public ReadOnly Property Direction As MapPlayerAnimationController.Direction
		Get
			Return Me.animationController.direction
		End Get
	End Property

	' Token: 0x06003886 RID: 14470 RVA: 0x00204138 File Offset: 0x00202538
	Protected Overrides Sub Awake()
		Dim array As MapPlayerController() = Global.UnityEngine.[Object].FindObjectsOfType(Of MapPlayerController)()
		For i As Integer = 0 To array.Length - 1
			If array(i).name.Contains("PlayerTwo") Then
				Global.UnityEngine.[Object].Destroy(array(i).gameObject)
			End If
		Next
		MyBase.Awake()
		MyBase.tag = "Player_Map"
		Me.input = MyBase.GetComponent(Of PlayerInput)()
		Me.motor = MyBase.GetComponent(Of MapPlayerMotor)()
		Me.animationController = MyBase.GetComponent(Of MapPlayerAnimationController)()
		Me.ladderManager = MyBase.GetComponent(Of MapPlayerLadderManager)()
	End Sub

	' Token: 0x06003887 RID: 14471 RVA: 0x002041C4 File Offset: 0x002025C4
	Private Sub Start()
		AddHandler MapPlayerController.OnEquipMenuOpenedEvent, AddressOf Me.OnEquipMenuOpened
		AddHandler MapPlayerController.OnEquipMenuClosedEvent, AddressOf Me.OnEquipMenuClosed
	End Sub

	' Token: 0x06003888 RID: 14472 RVA: 0x002041E8 File Offset: 0x002025E8
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		RemoveHandler MapPlayerController.OnEquipMenuOpenedEvent, AddressOf Me.OnEquipMenuOpened
		RemoveHandler MapPlayerController.OnEquipMenuClosedEvent, AddressOf Me.OnEquipMenuClosed
	End Sub

	' Token: 0x06003889 RID: 14473 RVA: 0x00204214 File Offset: 0x00202614
	Public Shared Function Create(playerId As PlayerId, init As MapPlayerController.InitObject) As MapPlayerController
		Dim mapPlayerController As MapPlayerController = Global.UnityEngine.[Object].Instantiate(Of MapPlayerController)(Map.Current.MapResources.mapPlayer)
		mapPlayerController.Init(playerId, init)
		Return mapPlayerController
	End Function

	' Token: 0x0600388A RID: 14474 RVA: 0x00204240 File Offset: 0x00202640
	Private Sub Init(playerId As PlayerId, init As MapPlayerController.InitObject)
		MyBase.gameObject.name = playerId.ToString()
		Me.id = playerId
		Me.input.Init(Me.id)
		Me.animationController.Init(init.pose)
		MyBase.transform.position = init.position
		Select Case init.pose
			Case MapPlayerPose.[Default]
				Me.state = MapPlayerController.State.Walking
			Case MapPlayerPose.Joined
				Me.state = MapPlayerController.State.Stationary
				MyBase.StartCoroutine(Me.joined_cr())
			Case MapPlayerPose.Won
				Me.state = MapPlayerController.State.Stationary
		End Select
	End Sub

	' Token: 0x0600388B RID: 14475 RVA: 0x002042F8 File Offset: 0x002026F8
	Public Sub Disable()
		Me.state = MapPlayerController.State.Stationary
	End Sub

	' Token: 0x0600388C RID: 14476 RVA: 0x00204301 File Offset: 0x00202701
	Public Sub Enable()
		Me.state = MapPlayerController.State.Walking
	End Sub

	' Token: 0x0600388D RID: 14477 RVA: 0x0020430A File Offset: 0x0020270A
	Public Sub OnLeave()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x0600388E RID: 14478 RVA: 0x00204317 File Offset: 0x00202717
	Private Sub OnChaliceJumpAnimationComplete()
		MyBase.transform.localScale = New Vector3(1F, 1F)
		Me.OnJumpAnimationComplete()
	End Sub

	' Token: 0x0600388F RID: 14479 RVA: 0x0020433C File Offset: 0x0020273C
	Private Sub OnJumpAnimationComplete()
		If Me.joinedMidGame Then
			Me.joinedMidGame = False
			If Me.id = PlayerId.PlayerTwo Then
				Dim mapPlayerController As MapPlayerController = Map.Current.players(0)
				If mapPlayerController.state <> MapPlayerController.State.Stationary Then
					Me.Enable()
				End If
			Else
				Me.Enable()
			End If
		Else
			Me.Enable()
		End If
	End Sub

	' Token: 0x06003890 RID: 14480 RVA: 0x0020439C File Offset: 0x0020279C
	Private Sub OnEquipMenuOpened()
		Me.EquipMenuOpen = True
	End Sub

	' Token: 0x06003891 RID: 14481 RVA: 0x002043A5 File Offset: 0x002027A5
	Private Sub OnEquipMenuClosed()
		Me.EquipMenuOpen = False
	End Sub

	' Token: 0x1400006A RID: 106
	' (add) Token: 0x06003892 RID: 14482 RVA: 0x002043B0 File Offset: 0x002027B0
	' (remove) Token: 0x06003893 RID: 14483 RVA: 0x002043E8 File Offset: 0x002027E8
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event LadderEnterEvent As MapPlayerController.LadderEnterEventHandler

	' Token: 0x1400006B RID: 107
	' (add) Token: 0x06003894 RID: 14484 RVA: 0x00204420 File Offset: 0x00202820
	' (remove) Token: 0x06003895 RID: 14485 RVA: 0x00204458 File Offset: 0x00202858
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event LadderExitEvent As MapPlayerController.LadderExitEventHandler

	' Token: 0x1400006C RID: 108
	' (add) Token: 0x06003896 RID: 14486 RVA: 0x00204490 File Offset: 0x00202890
	' (remove) Token: 0x06003897 RID: 14487 RVA: 0x002044C8 File Offset: 0x002028C8
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event LadderEnterCompleteEvent As Action

	' Token: 0x1400006D RID: 109
	' (add) Token: 0x06003898 RID: 14488 RVA: 0x00204500 File Offset: 0x00202900
	' (remove) Token: 0x06003899 RID: 14489 RVA: 0x00204538 File Offset: 0x00202938
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event LadderExitCompleteEvent As Action

	' Token: 0x0600389A RID: 14490 RVA: 0x0020456E File Offset: 0x0020296E
	Public Sub LadderEnter(point As Vector2, ladder As MapPlayerLadderObject, location As MapLadder.Location)
		Me.state = MapPlayerController.State.LadderEnter
		If Me.LadderEnterEvent IsNot Nothing Then
			Me.LadderEnterEvent(point, ladder, location)
		End If
	End Sub

	' Token: 0x0600389B RID: 14491 RVA: 0x00204590 File Offset: 0x00202990
	Public Sub LadderExit(point As Vector2, [exit] As Vector2, location As MapLadder.Location)
		Me.state = MapPlayerController.State.LadderExit
		If Me.LadderExitEvent IsNot Nothing Then
			Me.LadderExitEvent(point, [exit], location)
		End If
	End Sub

	' Token: 0x0600389C RID: 14492 RVA: 0x002045B2 File Offset: 0x002029B2
	Public Sub LadderEnterComplete()
		Me.state = MapPlayerController.State.Ladder
		If Me.LadderEnterCompleteEvent IsNot Nothing Then
			Me.LadderEnterCompleteEvent()
		End If
	End Sub

	' Token: 0x0600389D RID: 14493 RVA: 0x002045D1 File Offset: 0x002029D1
	Public Sub LadderExitComplete()
		Me.state = MapPlayerController.State.Walking
		If Me.LadderExitCompleteEvent IsNot Nothing Then
			Me.LadderExitCompleteEvent()
		End If
	End Sub

	' Token: 0x0600389E RID: 14494 RVA: 0x002045F0 File Offset: 0x002029F0
	Public Sub OnWinComplete()
		Me.animationController.CompleteJump()
	End Sub

	' Token: 0x0600389F RID: 14495 RVA: 0x00204600 File Offset: 0x00202A00
	Private Iterator Function joined_cr() As IEnumerator
		Yield Nothing
		Me.joinedMidGame = True
		Me.animationController.CompleteJump()
		Return
	End Function

	' Token: 0x060038A0 RID: 14496 RVA: 0x0020461C File Offset: 0x00202A1C
	Public Sub SecretPathEnter(enter As Boolean)
		Dim componentsInChildren As SpriteRenderer() = MyBase.GetComponentsInChildren(Of SpriteRenderer)()
		For Each spriteRenderer As SpriteRenderer In componentsInChildren
			spriteRenderer.sortingOrder = If((Not enter), 0, (-1000))
		Next
		MyBase.gameObject.layer = If((Not enter), LayerMask.NameToLayer("Default"), LayerMask.NameToLayer("Map_Secret"))
		Me.hideInteractionPrompts = enter
	End Sub

	' Token: 0x060038A1 RID: 14497 RVA: 0x00204692 File Offset: 0x00202A92
	Public Sub TryActivateDjimmi()
		If Not PlayerData.Data.TryActivateDjimmi() Then
			AudioManager.Play("menu_locked")
		End If
	End Sub

	' Token: 0x060038A2 RID: 14498 RVA: 0x002046AD File Offset: 0x00202AAD
	Public Sub JumpSFX()
		AudioManager.Play("complete_bounce")
	End Sub

	' Token: 0x0400404B RID: 16459
	Public Const TAG As String = "Player_Map"

	' Token: 0x0400404F RID: 16463
	Private joinedMidGame As Boolean

	' Token: 0x04004054 RID: 16468
	Public hideInteractionPrompts As Boolean

	' Token: 0x02000977 RID: 2423
	Public Enum State
		' Token: 0x0400405A RID: 16474
		Walking
		' Token: 0x0400405B RID: 16475
		LadderEnter
		' Token: 0x0400405C RID: 16476
		LadderExit
		' Token: 0x0400405D RID: 16477
		Ladder
		' Token: 0x0400405E RID: 16478
		Stationary
	End Enum

	' Token: 0x02000978 RID: 2424
	<Serializable()>
	Public Class InitObject
		' Token: 0x060038A3 RID: 14499 RVA: 0x002046B9 File Offset: 0x00202AB9
		Public Sub New(position As Vector2, pose As MapPlayerPose)
			Me.position = position
			Me.pose = pose
		End Sub

		' Token: 0x0400405F RID: 16479
		Public position As Vector2

		' Token: 0x04004060 RID: 16480
		Public pose As MapPlayerPose
	End Class

	' Token: 0x02000979 RID: 2425
	' (Invoke) Token: 0x060038A5 RID: 14501
	Public Delegate Sub LadderEnterEventHandler(point As Vector2, ladder As MapPlayerLadderObject, location As MapLadder.Location)

	' Token: 0x0200097A RID: 2426
	' (Invoke) Token: 0x060038A9 RID: 14505
	Public Delegate Sub LadderExitEventHandler(point As Vector2, [exit] As Vector2, location As MapLadder.Location)
End Class
