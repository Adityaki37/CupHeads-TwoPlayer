Imports System
Imports System.Collections.Generic
Imports UnityEngine
Imports UnityEngine.EventSystems
Imports UnityEngine.Serialization

Namespace Rewired.Integration.UnityUI
	' Token: 0x02000C51 RID: 3153
	<AddComponentMenu("Event/Rewired Standalone Input Module")>
	Public Class RewiredStandaloneInputModule
		Inherits PointerInputModule

		' Token: 0x06004D9E RID: 19870 RVA: 0x002763B8 File Offset: 0x002747B8
		Protected Sub New()
		End Sub

		' Token: 0x170007D6 RID: 2006
		' (get) Token: 0x06004D9F RID: 19871 RVA: 0x00276423 File Offset: 0x00274823
		' (set) Token: 0x06004DA0 RID: 19872 RVA: 0x0027642C File Offset: 0x0027482C
		Public Property UseAllRewiredGamePlayers As Boolean
			Get
				Return Me.useAllRewiredGamePlayers
			End Get
			Set(value As Boolean)
				Dim flag As Boolean = value <> Me.useAllRewiredGamePlayers
				Me.useAllRewiredGamePlayers = value
				If flag Then
					Me.SetupRewiredVars()
				End If
			End Set
		End Property

		' Token: 0x170007D7 RID: 2007
		' (get) Token: 0x06004DA1 RID: 19873 RVA: 0x00276459 File Offset: 0x00274859
		' (set) Token: 0x06004DA2 RID: 19874 RVA: 0x00276464 File Offset: 0x00274864
		Public Property UseRewiredSystemPlayer As Boolean
			Get
				Return Me.useRewiredSystemPlayer
			End Get
			Set(value As Boolean)
				Dim flag As Boolean = value <> Me.useRewiredSystemPlayer
				Me.useRewiredSystemPlayer = value
				If flag Then
					Me.SetupRewiredVars()
				End If
			End Set
		End Property

		' Token: 0x170007D8 RID: 2008
		' (get) Token: 0x06004DA3 RID: 19875 RVA: 0x00276491 File Offset: 0x00274891
		' (set) Token: 0x06004DA4 RID: 19876 RVA: 0x002764A3 File Offset: 0x002748A3
		Public Property RewiredPlayerIds As Integer()
			Get
				Return CType(Me.rewiredPlayerIds.Clone(), Integer())
			End Get
			Set(value As Integer())
				Me.rewiredPlayerIds = If((value Is Nothing), New Integer(-1) {}, CType(value.Clone(), Integer()))
				Me.SetupRewiredVars()
			End Set
		End Property

		' Token: 0x170007D9 RID: 2009
		' (get) Token: 0x06004DA5 RID: 19877 RVA: 0x002764CD File Offset: 0x002748CD
		' (set) Token: 0x06004DA6 RID: 19878 RVA: 0x002764D5 File Offset: 0x002748D5
		Public Property UsePlayingPlayersOnly As Boolean
			Get
				Return Me.usePlayingPlayersOnly
			End Get
			Set(value As Boolean)
				Me.usePlayingPlayersOnly = value
			End Set
		End Property

		' Token: 0x170007DA RID: 2010
		' (get) Token: 0x06004DA7 RID: 19879 RVA: 0x002764DE File Offset: 0x002748DE
		' (set) Token: 0x06004DA8 RID: 19880 RVA: 0x002764E6 File Offset: 0x002748E6
		Public Property MoveOneElementPerAxisPress As Boolean
			Get
				Return Me.moveOneElementPerAxisPress
			End Get
			Set(value As Boolean)
				Me.moveOneElementPerAxisPress = value
			End Set
		End Property

		' Token: 0x170007DB RID: 2011
		' (get) Token: 0x06004DA9 RID: 19881 RVA: 0x002764EF File Offset: 0x002748EF
		' (set) Token: 0x06004DAA RID: 19882 RVA: 0x002764F7 File Offset: 0x002748F7
		Public Property allowMouseInput As Boolean
			Get
				Return Me.m_allowMouseInput
			End Get
			Set(value As Boolean)
				Me.m_allowMouseInput = value
			End Set
		End Property

		' Token: 0x170007DC RID: 2012
		' (get) Token: 0x06004DAB RID: 19883 RVA: 0x00276500 File Offset: 0x00274900
		' (set) Token: 0x06004DAC RID: 19884 RVA: 0x00276508 File Offset: 0x00274908
		Public Property allowMouseInputIfTouchSupported As Boolean
			Get
				Return Me.m_allowMouseInputIfTouchSupported
			End Get
			Set(value As Boolean)
				Me.m_allowMouseInputIfTouchSupported = value
			End Set
		End Property

		' Token: 0x170007DD RID: 2013
		' (get) Token: 0x06004DAD RID: 19885 RVA: 0x00276511 File Offset: 0x00274911
		Private ReadOnly Property isMouseSupported As Boolean
			Get
				Return Input.mousePresent AndAlso Me.m_allowMouseInput AndAlso (Not Me.isTouchSupported OrElse Me.m_allowMouseInputIfTouchSupported)
			End Get
		End Property

		' Token: 0x170007DE RID: 2014
		' (get) Token: 0x06004DAE RID: 19886 RVA: 0x00276543 File Offset: 0x00274943
		' (set) Token: 0x06004DAF RID: 19887 RVA: 0x0027654B File Offset: 0x0027494B
		<Obsolete("allowActivationOnMobileDevice has been deprecated. Use forceModuleActive instead")>
		Public Property allowActivationOnMobileDevice As Boolean
			Get
				Return Me.m_ForceModuleActive
			End Get
			Set(value As Boolean)
				Me.m_ForceModuleActive = value
			End Set
		End Property

		' Token: 0x170007DF RID: 2015
		' (get) Token: 0x06004DB0 RID: 19888 RVA: 0x00276554 File Offset: 0x00274954
		' (set) Token: 0x06004DB1 RID: 19889 RVA: 0x0027655C File Offset: 0x0027495C
		Public Property forceModuleActive As Boolean
			Get
				Return Me.m_ForceModuleActive
			End Get
			Set(value As Boolean)
				Me.m_ForceModuleActive = value
			End Set
		End Property

		' Token: 0x170007E0 RID: 2016
		' (get) Token: 0x06004DB2 RID: 19890 RVA: 0x00276565 File Offset: 0x00274965
		' (set) Token: 0x06004DB3 RID: 19891 RVA: 0x0027656D File Offset: 0x0027496D
		Public Property inputActionsPerSecond As Single
			Get
				Return Me.m_InputActionsPerSecond
			End Get
			Set(value As Single)
				Me.m_InputActionsPerSecond = value
			End Set
		End Property

		' Token: 0x170007E1 RID: 2017
		' (get) Token: 0x06004DB4 RID: 19892 RVA: 0x00276576 File Offset: 0x00274976
		' (set) Token: 0x06004DB5 RID: 19893 RVA: 0x0027657E File Offset: 0x0027497E
		Public Property repeatDelay As Single
			Get
				Return Me.m_RepeatDelay
			End Get
			Set(value As Single)
				Me.m_RepeatDelay = value
			End Set
		End Property

		' Token: 0x170007E2 RID: 2018
		' (get) Token: 0x06004DB6 RID: 19894 RVA: 0x00276587 File Offset: 0x00274987
		' (set) Token: 0x06004DB7 RID: 19895 RVA: 0x0027658F File Offset: 0x0027498F
		Public Property horizontalAxis As String
			Get
				Return Me.m_HorizontalAxis
			End Get
			Set(value As String)
				Me.m_HorizontalAxis = value
			End Set
		End Property

		' Token: 0x170007E3 RID: 2019
		' (get) Token: 0x06004DB8 RID: 19896 RVA: 0x00276598 File Offset: 0x00274998
		' (set) Token: 0x06004DB9 RID: 19897 RVA: 0x002765A0 File Offset: 0x002749A0
		Public Property verticalAxis As String
			Get
				Return Me.m_VerticalAxis
			End Get
			Set(value As String)
				Me.m_VerticalAxis = value
			End Set
		End Property

		' Token: 0x170007E4 RID: 2020
		' (get) Token: 0x06004DBA RID: 19898 RVA: 0x002765A9 File Offset: 0x002749A9
		' (set) Token: 0x06004DBB RID: 19899 RVA: 0x002765B1 File Offset: 0x002749B1
		Public Property submitButton As String
			Get
				Return Me.m_SubmitButton
			End Get
			Set(value As String)
				Me.m_SubmitButton = value
			End Set
		End Property

		' Token: 0x170007E5 RID: 2021
		' (get) Token: 0x06004DBC RID: 19900 RVA: 0x002765BA File Offset: 0x002749BA
		' (set) Token: 0x06004DBD RID: 19901 RVA: 0x002765C2 File Offset: 0x002749C2
		Public Property cancelButton As String
			Get
				Return Me.m_CancelButton
			End Get
			Set(value As String)
				Me.m_CancelButton = value
			End Set
		End Property

		' Token: 0x06004DBE RID: 19902 RVA: 0x002765CC File Offset: 0x002749CC
		Protected Overrides Sub Awake()
			MyBase.Awake()
			Me.isTouchSupported = Input.touchSupported
			Dim component As TouchInputModule = MyBase.GetComponent(Of TouchInputModule)()
			If component IsNot Nothing Then
				component.enabled = False
			End If
			Me.InitializeRewired()
		End Sub

		' Token: 0x06004DBF RID: 19903 RVA: 0x0027660C File Offset: 0x00274A0C
		Public Overrides Sub UpdateModule()
			Me.CheckEditorRecompile()
			If Me.recompiling Then
				Return
			End If
			If Not ReInput.isReady Then
				Return
			End If
			If Not Me.m_HasFocus AndAlso Me.ShouldIgnoreEventsOnNoFocus() Then
				Return
			End If
			If Me.isMouseSupported Then
				Me.m_LastMousePosition = Me.m_MousePosition
				Me.m_MousePosition = Input.mousePosition
			End If
		End Sub

		' Token: 0x06004DC0 RID: 19904 RVA: 0x00276674 File Offset: 0x00274A74
		Public Overrides Function IsModuleSupported() As Boolean
			Return True
		End Function

		' Token: 0x06004DC1 RID: 19905 RVA: 0x00276678 File Offset: 0x00274A78
		Public Overrides Function ShouldActivateModule() As Boolean
			If Not MyBase.ShouldActivateModule() Then
				Return False
			End If
			If Me.recompiling Then
				Return False
			End If
			If Not ReInput.isReady Then
				Return False
			End If
			Dim flag As Boolean = Me.m_ForceModuleActive
			For i As Integer = 0 To Me.playerIds.Length - 1
				Dim player As Player = ReInput.players.GetPlayer(Me.playerIds(i))
				If player IsNot Nothing Then
					If Not Me.usePlayingPlayersOnly OrElse player.isPlaying Then
						flag = flag Or player.GetButtonDown(Me.m_SubmitButton)
						flag = flag Or player.GetButtonDown(Me.m_CancelButton)
						If Me.moveOneElementPerAxisPress Then
							flag = flag Or player.GetButtonDown(Me.m_HorizontalAxis) OrElse player.GetNegativeButtonDown(Me.m_HorizontalAxis)
							flag = flag Or player.GetButtonDown(Me.m_VerticalAxis) OrElse player.GetNegativeButtonDown(Me.m_VerticalAxis)
						Else
							flag = flag Or Not Mathf.Approximately(player.GetAxisRaw(Me.m_HorizontalAxis), 0F)
							flag = flag Or Not Mathf.Approximately(player.GetAxisRaw(Me.m_VerticalAxis), 0F)
						End If
					End If
				End If
			Next
			If Me.isMouseSupported Then
				flag = flag Or (Me.m_MousePosition - Me.m_LastMousePosition).sqrMagnitude > 0F
				flag = flag Or Input.GetMouseButtonDown(0)
			End If
			If Me.isTouchSupported Then
				For j As Integer = 0 To Input.touchCount - 1
					Dim touch As Touch = Input.GetTouch(j)
					flag = flag Or touch.phase = TouchPhase.Began OrElse touch.phase = TouchPhase.Moved OrElse touch.phase = TouchPhase.Stationary
				Next
			End If
			Return flag
		End Function

		' Token: 0x06004DC2 RID: 19906 RVA: 0x00276840 File Offset: 0x00274C40
		Public Overrides Sub ActivateModule()
			If Not Me.m_HasFocus AndAlso Me.ShouldIgnoreEventsOnNoFocus() Then
				Return
			End If
			MyBase.ActivateModule()
			If Me.isMouseSupported Then
				Dim vector As Vector2 = Input.mousePosition
				Me.m_MousePosition = vector
				Me.m_LastMousePosition = vector
			End If
			Dim gameObject As GameObject = MyBase.eventSystem.currentSelectedGameObject
			If gameObject Is Nothing Then
				gameObject = MyBase.eventSystem.firstSelectedGameObject
			End If
			MyBase.eventSystem.SetSelectedGameObject(gameObject, Me.GetBaseEventData())
		End Sub

		' Token: 0x06004DC3 RID: 19907 RVA: 0x002768C4 File Offset: 0x00274CC4
		Public Overrides Sub DeactivateModule()
			MyBase.DeactivateModule()
			MyBase.ClearSelection()
		End Sub

		' Token: 0x06004DC4 RID: 19908 RVA: 0x002768D4 File Offset: 0x00274CD4
		Public Overrides Sub Process()
			If Not ReInput.isReady Then
				Return
			End If
			If Not Me.m_HasFocus AndAlso Me.ShouldIgnoreEventsOnNoFocus() Then
				Return
			End If
			Dim flag As Boolean = Me.SendUpdateEventToSelectedObject()
			If MyBase.eventSystem.sendNavigationEvents Then
				If Not flag Then
					flag = flag Or Me.SendMoveEventToSelectedObject()
				End If
				If Not flag Then
					Me.SendSubmitEventToSelectedObject()
				End If
			End If
			If Not Me.ProcessTouchEvents() AndAlso Me.isMouseSupported Then
				Me.ProcessMouseEvent()
			End If
		End Sub

		' Token: 0x06004DC5 RID: 19909 RVA: 0x00276954 File Offset: 0x00274D54
		Private Function ProcessTouchEvents() As Boolean
			If Not Me.isTouchSupported Then
				Return False
			End If
			For i As Integer = 0 To Input.touchCount - 1
				Dim touch As Touch = Input.GetTouch(i)
				If touch.type <> TouchType.Indirect Then
					Dim flag As Boolean
					Dim flag2 As Boolean
					Dim touchPointerEventData As PointerEventData = MyBase.GetTouchPointerEventData(touch, flag, flag2)
					Me.ProcessTouchPress(touchPointerEventData, flag, flag2)
					If Not flag2 Then
						Me.ProcessMove(touchPointerEventData)
						Me.ProcessDrag(touchPointerEventData)
					Else
						MyBase.RemovePointerData(touchPointerEventData)
					End If
				End If
			Next
			Return Input.touchCount > 0
		End Function

		' Token: 0x06004DC6 RID: 19910 RVA: 0x002769E0 File Offset: 0x00274DE0
		Private Sub ProcessTouchPress(pointerEvent As PointerEventData, pressed As Boolean, released As Boolean)
			Dim gameObject As GameObject = pointerEvent.pointerCurrentRaycast.gameObject
			If pressed Then
				pointerEvent.eligibleForClick = True
				pointerEvent.delta = Vector2.zero
				pointerEvent.dragging = False
				pointerEvent.useDragThreshold = True
				pointerEvent.pressPosition = pointerEvent.position
				pointerEvent.pointerPressRaycast = pointerEvent.pointerCurrentRaycast
				MyBase.DeselectIfSelectionChanged(gameObject, pointerEvent)
				If pointerEvent.pointerEnter IsNot gameObject Then
					MyBase.HandlePointerExitAndEnter(pointerEvent, gameObject)
					pointerEvent.pointerEnter = gameObject
				End If
				Dim gameObject2 As GameObject = ExecuteEvents.ExecuteHierarchy(Of IPointerDownHandler)(gameObject, pointerEvent, ExecuteEvents.pointerDownHandler)
				If gameObject2 Is Nothing Then
					gameObject2 = ExecuteEvents.GetEventHandler(Of IPointerClickHandler)(gameObject)
				End If
				Dim unscaledTime As Single = Time.unscaledTime
				If gameObject2 Is pointerEvent.lastPress Then
					Dim num As Single = unscaledTime - pointerEvent.clickTime
					If num < 0.3F Then
						pointerEvent.clickCount += 1
					Else
						pointerEvent.clickCount = 1
					End If
					pointerEvent.clickTime = unscaledTime
				Else
					pointerEvent.clickCount = 1
				End If
				pointerEvent.pointerPress = gameObject2
				pointerEvent.rawPointerPress = gameObject
				pointerEvent.clickTime = unscaledTime
				pointerEvent.pointerDrag = ExecuteEvents.GetEventHandler(Of IDragHandler)(gameObject)
				If pointerEvent.pointerDrag IsNot Nothing Then
					ExecuteEvents.Execute(Of IInitializePotentialDragHandler)(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.initializePotentialDrag)
				End If
			End If
			If released Then
				ExecuteEvents.Execute(Of IPointerUpHandler)(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerUpHandler)
				Dim eventHandler As GameObject = ExecuteEvents.GetEventHandler(Of IPointerClickHandler)(gameObject)
				If pointerEvent.pointerPress Is eventHandler AndAlso pointerEvent.eligibleForClick Then
					ExecuteEvents.Execute(Of IPointerClickHandler)(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerClickHandler)
				ElseIf pointerEvent.pointerDrag IsNot Nothing AndAlso pointerEvent.dragging Then
					ExecuteEvents.ExecuteHierarchy(Of IDropHandler)(gameObject, pointerEvent, ExecuteEvents.dropHandler)
				End If
				pointerEvent.eligibleForClick = False
				pointerEvent.pointerPress = Nothing
				pointerEvent.rawPointerPress = Nothing
				If pointerEvent.pointerDrag IsNot Nothing AndAlso pointerEvent.dragging Then
					ExecuteEvents.Execute(Of IEndDragHandler)(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.endDragHandler)
				End If
				pointerEvent.dragging = False
				pointerEvent.pointerDrag = Nothing
				If pointerEvent.pointerDrag IsNot Nothing Then
					ExecuteEvents.Execute(Of IEndDragHandler)(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.endDragHandler)
				End If
				pointerEvent.pointerDrag = Nothing
				ExecuteEvents.ExecuteHierarchy(Of IPointerExitHandler)(pointerEvent.pointerEnter, pointerEvent, ExecuteEvents.pointerExitHandler)
				pointerEvent.pointerEnter = Nothing
			End If
		End Sub

		' Token: 0x06004DC7 RID: 19911 RVA: 0x00276C34 File Offset: 0x00275034
		Protected Function SendSubmitEventToSelectedObject() As Boolean
			If MyBase.eventSystem.currentSelectedGameObject Is Nothing Then
				Return False
			End If
			If Me.recompiling Then
				Return False
			End If
			Dim baseEventData As BaseEventData = Me.GetBaseEventData()
			For i As Integer = 0 To Me.playerIds.Length - 1
				Dim player As Player = ReInput.players.GetPlayer(Me.playerIds(i))
				If player IsNot Nothing Then
					If Not Me.usePlayingPlayersOnly OrElse player.isPlaying Then
						If player.GetButtonDown(Me.m_SubmitButton) Then
							ExecuteEvents.Execute(Of ISubmitHandler)(MyBase.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.submitHandler)
							Exit For
						End If
						If player.GetButtonDown(Me.m_CancelButton) Then
							ExecuteEvents.Execute(Of ICancelHandler)(MyBase.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.cancelHandler)
							Exit For
						End If
					End If
				End If
			Next
			Return baseEventData.used
		End Function

		' Token: 0x06004DC8 RID: 19912 RVA: 0x00276D20 File Offset: 0x00275120
		Private Function GetRawMoveVector() As Vector2
			If Me.recompiling Then
				Return Vector2.zero
			End If
			Dim zero As Vector2 = Vector2.zero
			Dim flag As Boolean = False
			Dim flag2 As Boolean = False
			For i As Integer = 0 To Me.playerIds.Length - 1
				Dim player As Player = ReInput.players.GetPlayer(Me.playerIds(i))
				If player IsNot Nothing Then
					If Not Me.usePlayingPlayersOnly OrElse player.isPlaying Then
						If Me.moveOneElementPerAxisPress Then
							Dim num As Single = 0F
							If player.GetButtonDown(Me.m_HorizontalAxis) Then
								num = 1F
							ElseIf player.GetNegativeButtonDown(Me.m_HorizontalAxis) Then
								num = -1F
							End If
							Dim num2 As Single = 0F
							If player.GetButtonDown(Me.m_VerticalAxis) Then
								num2 = 1F
							ElseIf player.GetNegativeButtonDown(Me.m_VerticalAxis) Then
								num2 = -1F
							End If
							zero.x += num
							zero.y += num2
						Else
							zero.x += player.GetAxisRaw(Me.m_HorizontalAxis)
							zero.y += player.GetAxisRaw(Me.m_VerticalAxis)
						End If
						flag = flag Or player.GetButtonDown(Me.m_HorizontalAxis) OrElse player.GetNegativeButtonDown(Me.m_HorizontalAxis)
						flag2 = flag2 Or player.GetButtonDown(Me.m_VerticalAxis) OrElse player.GetNegativeButtonDown(Me.m_VerticalAxis)
					End If
				End If
			Next
			If flag Then
				If zero.x < 0F Then
					zero.x = -1F
				End If
				If zero.x > 0F Then
					zero.x = 1F
				End If
			End If
			If flag2 Then
				If zero.y < 0F Then
					zero.y = -1F
				End If
				If zero.y > 0F Then
					zero.y = 1F
				End If
			End If
			Return zero
		End Function

		' Token: 0x06004DC9 RID: 19913 RVA: 0x00276F4C File Offset: 0x0027534C
		Protected Function SendMoveEventToSelectedObject() As Boolean
			If Me.recompiling Then
				Return False
			End If
			Dim unscaledTime As Single = Time.unscaledTime
			Dim rawMoveVector As Vector2 = Me.GetRawMoveVector()
			If Mathf.Approximately(rawMoveVector.x, 0F) AndAlso Mathf.Approximately(rawMoveVector.y, 0F) Then
				Me.m_ConsecutiveMoveCount = 0
				Return False
			End If
			Dim flag As Boolean = Vector2.Dot(rawMoveVector, Me.m_LastMoveVector) > 0F
			Dim flag2 As Boolean = Me.CheckButtonOrKeyMovement(unscaledTime)
			Dim flag3 As Boolean = flag2
			If Not flag3 Then
				If Me.m_RepeatDelay > 0F Then
					If flag AndAlso Me.m_ConsecutiveMoveCount = 1 Then
						flag3 = unscaledTime > Me.m_PrevActionTime + Me.m_RepeatDelay
					Else
						flag3 = unscaledTime > Me.m_PrevActionTime + 1F / Me.m_InputActionsPerSecond
					End If
				Else
					flag3 = unscaledTime > Me.m_PrevActionTime + 1F / Me.m_InputActionsPerSecond
				End If
			End If
			If Not flag3 Then
				Return False
			End If
			Dim axisEventData As AxisEventData = Me.GetAxisEventData(rawMoveVector.x, rawMoveVector.y, 0.6F)
			If axisEventData.moveDir = MoveDirection.None Then
				Return False
			End If
			ExecuteEvents.Execute(Of IMoveHandler)(MyBase.eventSystem.currentSelectedGameObject, axisEventData, ExecuteEvents.moveHandler)
			If Not flag Then
				Me.m_ConsecutiveMoveCount = 0
			End If
			Me.m_ConsecutiveMoveCount += 1
			Me.m_PrevActionTime = unscaledTime
			Me.m_LastMoveVector = rawMoveVector
			Return axisEventData.used
		End Function

		' Token: 0x06004DCA RID: 19914 RVA: 0x002770B8 File Offset: 0x002754B8
		Private Function CheckButtonOrKeyMovement(time As Single) As Boolean
			Dim flag As Boolean = False
			For i As Integer = 0 To Me.playerIds.Length - 1
				Dim player As Player = ReInput.players.GetPlayer(Me.playerIds(i))
				If player IsNot Nothing Then
					If Not Me.usePlayingPlayersOnly OrElse player.isPlaying Then
						flag = flag Or player.GetButtonDown(Me.m_HorizontalAxis) OrElse player.GetNegativeButtonDown(Me.m_HorizontalAxis)
						flag = flag Or player.GetButtonDown(Me.m_VerticalAxis) OrElse player.GetNegativeButtonDown(Me.m_VerticalAxis)
					End If
				End If
			Next
			Return flag
		End Function

		' Token: 0x06004DCB RID: 19915 RVA: 0x00277160 File Offset: 0x00275560
		Protected Sub ProcessMouseEvent()
			Me.ProcessMouseEvent(0)
		End Sub

		' Token: 0x06004DCC RID: 19916 RVA: 0x0027716C File Offset: 0x0027556C
		Protected Sub ProcessMouseEvent(id As Integer)
			Dim mousePointerEventData As PointerInputModule.MouseState = Me.GetMousePointerEventData()
			Dim eventData As PointerInputModule.MouseButtonEventData = mousePointerEventData.GetButtonState(PointerEventData.InputButton.Left).eventData
			Me.ProcessMousePress(eventData)
			Me.ProcessMove(eventData.buttonData)
			Me.ProcessDrag(eventData.buttonData)
			Me.ProcessMousePress(mousePointerEventData.GetButtonState(PointerEventData.InputButton.Right).eventData)
			Me.ProcessDrag(mousePointerEventData.GetButtonState(PointerEventData.InputButton.Right).eventData.buttonData)
			Me.ProcessMousePress(mousePointerEventData.GetButtonState(PointerEventData.InputButton.Middle).eventData)
			Me.ProcessDrag(mousePointerEventData.GetButtonState(PointerEventData.InputButton.Middle).eventData.buttonData)
			If Not Mathf.Approximately(eventData.buttonData.scrollDelta.sqrMagnitude, 0F) Then
				Dim eventHandler As GameObject = ExecuteEvents.GetEventHandler(Of IScrollHandler)(eventData.buttonData.pointerCurrentRaycast.gameObject)
				ExecuteEvents.ExecuteHierarchy(Of IScrollHandler)(eventHandler, eventData.buttonData, ExecuteEvents.scrollHandler)
			End If
		End Sub

		' Token: 0x06004DCD RID: 19917 RVA: 0x0027724C File Offset: 0x0027564C
		Protected Function SendUpdateEventToSelectedObject() As Boolean
			If MyBase.eventSystem.currentSelectedGameObject Is Nothing Then
				Return False
			End If
			Dim baseEventData As BaseEventData = Me.GetBaseEventData()
			ExecuteEvents.Execute(Of IUpdateSelectedHandler)(MyBase.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.updateSelectedHandler)
			Return baseEventData.used
		End Function

		' Token: 0x06004DCE RID: 19918 RVA: 0x00277298 File Offset: 0x00275698
		Protected Sub ProcessMousePress(data As PointerInputModule.MouseButtonEventData)
			Dim buttonData As PointerEventData = data.buttonData
			Dim gameObject As GameObject = buttonData.pointerCurrentRaycast.gameObject
			If data.PressedThisFrame() Then
				buttonData.eligibleForClick = True
				buttonData.delta = Vector2.zero
				buttonData.dragging = False
				buttonData.useDragThreshold = True
				buttonData.pressPosition = buttonData.position
				buttonData.pointerPressRaycast = buttonData.pointerCurrentRaycast
				MyBase.DeselectIfSelectionChanged(gameObject, buttonData)
				Dim gameObject2 As GameObject = ExecuteEvents.ExecuteHierarchy(Of IPointerDownHandler)(gameObject, buttonData, ExecuteEvents.pointerDownHandler)
				If gameObject2 Is Nothing Then
					gameObject2 = ExecuteEvents.GetEventHandler(Of IPointerClickHandler)(gameObject)
				End If
				Dim unscaledTime As Single = Time.unscaledTime
				If gameObject2 Is buttonData.lastPress Then
					Dim num As Single = unscaledTime - buttonData.clickTime
					If num < 0.3F Then
						buttonData.clickCount += 1
					Else
						buttonData.clickCount = 1
					End If
					buttonData.clickTime = unscaledTime
				Else
					buttonData.clickCount = 1
				End If
				buttonData.pointerPress = gameObject2
				buttonData.rawPointerPress = gameObject
				buttonData.clickTime = unscaledTime
				buttonData.pointerDrag = ExecuteEvents.GetEventHandler(Of IDragHandler)(gameObject)
				If buttonData.pointerDrag IsNot Nothing Then
					ExecuteEvents.Execute(Of IInitializePotentialDragHandler)(buttonData.pointerDrag, buttonData, ExecuteEvents.initializePotentialDrag)
				End If
			End If
			If data.ReleasedThisFrame() Then
				ExecuteEvents.Execute(Of IPointerUpHandler)(buttonData.pointerPress, buttonData, ExecuteEvents.pointerUpHandler)
				Dim eventHandler As GameObject = ExecuteEvents.GetEventHandler(Of IPointerClickHandler)(gameObject)
				If buttonData.pointerPress Is eventHandler AndAlso buttonData.eligibleForClick Then
					ExecuteEvents.Execute(Of IPointerClickHandler)(buttonData.pointerPress, buttonData, ExecuteEvents.pointerClickHandler)
				ElseIf buttonData.pointerDrag IsNot Nothing AndAlso buttonData.dragging Then
					ExecuteEvents.ExecuteHierarchy(Of IDropHandler)(gameObject, buttonData, ExecuteEvents.dropHandler)
				End If
				buttonData.eligibleForClick = False
				buttonData.pointerPress = Nothing
				buttonData.rawPointerPress = Nothing
				If buttonData.pointerDrag IsNot Nothing AndAlso buttonData.dragging Then
					ExecuteEvents.Execute(Of IEndDragHandler)(buttonData.pointerDrag, buttonData, ExecuteEvents.endDragHandler)
				End If
				buttonData.dragging = False
				buttonData.pointerDrag = Nothing
				If gameObject IsNot buttonData.pointerEnter Then
					MyBase.HandlePointerExitAndEnter(buttonData, Nothing)
					MyBase.HandlePointerExitAndEnter(buttonData, gameObject)
				End If
			End If
		End Sub

		' Token: 0x06004DCF RID: 19919 RVA: 0x002774BC File Offset: 0x002758BC
		Protected Overridable Sub OnApplicationFocus(hasFocus As Boolean)
			Me.m_HasFocus = hasFocus
		End Sub

		' Token: 0x06004DD0 RID: 19920 RVA: 0x002774C5 File Offset: 0x002758C5
		Private Function ShouldIgnoreEventsOnNoFocus() As Boolean
			Return Not ReInput.isReady OrElse ReInput.configuration.ignoreInputWhenAppNotInFocus
		End Function

		' Token: 0x06004DD1 RID: 19921 RVA: 0x002774DD File Offset: 0x002758DD
		Private Sub InitializeRewired()
			If Not ReInput.isReady Then
				Global.Debug.LogError("Rewired is not initialized! Are you missing a Rewired Input Manager in your scene?", Nothing)
				Return
			End If
			AddHandler ReInput.EditorRecompileEvent, AddressOf Me.OnEditorRecompile
			Me.SetupRewiredVars()
		End Sub

		' Token: 0x06004DD2 RID: 19922 RVA: 0x0027750C File Offset: 0x0027590C
		Private Sub SetupRewiredVars()
			If Me.useAllRewiredGamePlayers Then
				Dim list As IList(Of Player) = If((Not Me.useRewiredSystemPlayer), ReInput.players.Players, ReInput.players.AllPlayers)
				Me.playerIds = New Integer(list.Count - 1) {}
				For i As Integer = 0 To list.Count - 1
					Me.playerIds(i) = list(i).id
				Next
			Else
				Dim num As Integer = Me.rewiredPlayerIds.Length + If((Not Me.useRewiredSystemPlayer), 0, 1)
				Me.playerIds = New Integer(num - 1) {}
				For j As Integer = 0 To Me.rewiredPlayerIds.Length - 1
					Me.playerIds(j) = ReInput.players.GetPlayer(Me.rewiredPlayerIds(j)).id
				Next
				If Me.useRewiredSystemPlayer Then
					Me.playerIds(num - 1) = ReInput.players.GetSystemPlayer().id
				End If
			End If
		End Sub

		' Token: 0x06004DD3 RID: 19923 RVA: 0x0027760E File Offset: 0x00275A0E
		Private Sub CheckEditorRecompile()
			If Not Me.recompiling Then
				Return
			End If
			If Not ReInput.isReady Then
				Return
			End If
			Me.recompiling = False
			Me.InitializeRewired()
		End Sub

		' Token: 0x06004DD4 RID: 19924 RVA: 0x00277634 File Offset: 0x00275A34
		Private Sub OnEditorRecompile()
			Me.recompiling = True
			Me.ClearRewiredVars()
		End Sub

		' Token: 0x06004DD5 RID: 19925 RVA: 0x00277643 File Offset: 0x00275A43
		Private Sub ClearRewiredVars()
			Array.Clear(Me.playerIds, 0, Me.playerIds.Length)
		End Sub

		' Token: 0x040051A9 RID: 20905
		Private Const DEFAULT_ACTION_MOVE_HORIZONTAL As String = "UIHorizontal"

		' Token: 0x040051AA RID: 20906
		Private Const DEFAULT_ACTION_MOVE_VERTICAL As String = "UIVertical"

		' Token: 0x040051AB RID: 20907
		Private Const DEFAULT_ACTION_SUBMIT As String = "UISubmit"

		' Token: 0x040051AC RID: 20908
		Private Const DEFAULT_ACTION_CANCEL As String = "UICancel"

		' Token: 0x040051AD RID: 20909
		Private playerIds As Integer()

		' Token: 0x040051AE RID: 20910
		Private recompiling As Boolean

		' Token: 0x040051AF RID: 20911
		Private isTouchSupported As Boolean

		' Token: 0x040051B0 RID: 20912
		<SerializeField()>
		<Tooltip("Use all Rewired game Players to control the UI. This does not include the System Player. If enabled, this setting overrides individual Player Ids set in Rewired Player Ids.")>
		Private useAllRewiredGamePlayers As Boolean

		' Token: 0x040051B1 RID: 20913
		<SerializeField()>
		<Tooltip("Allow the Rewired System Player to control the UI.")>
		Private useRewiredSystemPlayer As Boolean

		' Token: 0x040051B2 RID: 20914
		<SerializeField()>
		<Tooltip("A list of Player Ids that are allowed to control the UI. If Use All Rewired Game Players = True, this list will be ignored.")>
		Private rewiredPlayerIds As Integer() = New Integer(0) {}

		' Token: 0x040051B3 RID: 20915
		<SerializeField()>
		<Tooltip("Allow only Players with Player.isPlaying = true to control the UI.")>
		Private usePlayingPlayersOnly As Boolean

		' Token: 0x040051B4 RID: 20916
		<SerializeField()>
		<Tooltip("Makes an axis press always move only one UI selection. Enable if you do not want to allow scrolling through UI elements by holding an axis direction.")>
		Private moveOneElementPerAxisPress As Boolean

		' Token: 0x040051B5 RID: 20917
		Private m_PrevActionTime As Single

		' Token: 0x040051B6 RID: 20918
		Private m_LastMoveVector As Vector2

		' Token: 0x040051B7 RID: 20919
		Private m_ConsecutiveMoveCount As Integer

		' Token: 0x040051B8 RID: 20920
		Private m_LastMousePosition As Vector2

		' Token: 0x040051B9 RID: 20921
		Private m_MousePosition As Vector2

		' Token: 0x040051BA RID: 20922
		Private m_HasFocus As Boolean = True

		' Token: 0x040051BB RID: 20923
		<SerializeField()>
		Private m_HorizontalAxis As String = "UIHorizontal"

		' Token: 0x040051BC RID: 20924
		<SerializeField()>
		<Tooltip("Name of the vertical axis for movement (if axis events are used).")>
		Private m_VerticalAxis As String = "UIVertical"

		' Token: 0x040051BD RID: 20925
		<SerializeField()>
		<Tooltip("Name of the action used to submit.")>
		Private m_SubmitButton As String = "UISubmit"

		' Token: 0x040051BE RID: 20926
		<SerializeField()>
		<Tooltip("Name of the action used to cancel.")>
		Private m_CancelButton As String = "UICancel"

		' Token: 0x040051BF RID: 20927
		<SerializeField()>
		<Tooltip("Number of selection changes allowed per second when a movement button/axis is held in a direction.")>
		Private m_InputActionsPerSecond As Single = 10F

		' Token: 0x040051C0 RID: 20928
		<SerializeField()>
		<Tooltip("Delay in seconds before vertical/horizontal movement starts repeating continouously when a movement direction is held.")>
		Private m_RepeatDelay As Single

		' Token: 0x040051C1 RID: 20929
		<SerializeField()>
		<Tooltip("Allows the mouse to be used to select elements.")>
		Private m_allowMouseInput As Boolean = True

		' Token: 0x040051C2 RID: 20930
		<SerializeField()>
		<Tooltip("Allows the mouse to be used to select elements if the device also supports touch control.")>
		Private m_allowMouseInputIfTouchSupported As Boolean = True

		' Token: 0x040051C3 RID: 20931
		<SerializeField()>
		<FormerlySerializedAs("m_AllowActivationOnMobileDevice")>
		<Tooltip("Forces the module to always be active.")>
		Private m_ForceModuleActive As Boolean
	End Class
End Namespace
