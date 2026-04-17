Imports System
Imports System.Collections
Imports System.Diagnostics
Imports UnityEngine

' Token: 0x02000925 RID: 2341
Public MustInherit Class AbstractMapInteractiveEntity
	Inherits MapSprite

	' Token: 0x14000066 RID: 102
	' (add) Token: 0x060036BE RID: 14014 RVA: 0x00098964 File Offset: 0x00096D64
	' (remove) Token: 0x060036BF RID: 14015 RVA: 0x0009899C File Offset: 0x00096D9C
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnActivateEvent As Action

	' Token: 0x17000478 RID: 1144
	' (get) Token: 0x060036C0 RID: 14016 RVA: 0x000989D2 File Offset: 0x00096DD2
	' (set) Token: 0x060036C1 RID: 14017 RVA: 0x000989DA File Offset: 0x00096DDA
	Protected Private Property state As AbstractMapInteractiveEntity.State

	' Token: 0x17000479 RID: 1145
	' (get) Token: 0x060036C2 RID: 14018 RVA: 0x000989E3 File Offset: 0x00096DE3
	' (set) Token: 0x060036C3 RID: 14019 RVA: 0x000989EB File Offset: 0x00096DEB
	Protected Private Property playerActivating As MapPlayerController

	' Token: 0x1700047A RID: 1146
	' (get) Token: 0x060036C4 RID: 14020 RVA: 0x000989F4 File Offset: 0x00096DF4
	' (set) Token: 0x060036C5 RID: 14021 RVA: 0x000989FC File Offset: 0x00096DFC
	Protected Private Property playerChecking As MapPlayerController

	' Token: 0x1700047B RID: 1147
	' (get) Token: 0x060036C6 RID: 14022 RVA: 0x00098A05 File Offset: 0x00096E05
	Protected Overrides ReadOnly Property ChangesDepth As Boolean
		Get
			Return Me.playerCanWalkBehind
		End Get
	End Property

	' Token: 0x060036C7 RID: 14023 RVA: 0x00098A0D File Offset: 0x00096E0D
	Protected Overrides Sub Awake()
		MyBase.Awake()
		AbstractMapInteractiveEntity.HasPopupOpened = False
		Me.lockInput = True
		MyBase.StartCoroutine(Me.lock_input_cr())
	End Sub

	' Token: 0x060036C8 RID: 14024 RVA: 0x00098A30 File Offset: 0x00096E30
	Private Iterator Function lock_input_cr() As IEnumerator
		Yield New WaitForSeconds(1F)
		Me.lockInput = False
		Yield Nothing
		Return
	End Function

	' Token: 0x060036C9 RID: 14025 RVA: 0x00098A4C File Offset: 0x00096E4C
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.lockInput Then
			Return
		End If
		If InterruptingPrompt.IsInterrupting() Then
			Return
		End If
		Me.Check()
		If Me.state = AbstractMapInteractiveEntity.State.Activated Then
			Return
		End If
		If MapConfirmStartUI.Current.CurrentState <> AbstractMapSceneStartUI.State.Inactive OrElse MapDifficultySelectStartUI.Current.CurrentState <> AbstractMapSceneStartUI.State.Inactive OrElse MapEventNotification.Current.showing OrElse (Map.Current IsNot Nothing AndAlso Map.Current.CurrentState = Map.State.Graveyard) OrElse SceneLoader.IsInBlurTransition Then
			Return
		End If
		Select Case Me.interactor
			Case Else
				If Me.PlayerWithinDistance(0) AndAlso Map.Current.players(0).input.actions.GetButtonDown(13) Then
					Me.Activate(Map.Current.players(0))
				End If
			Case AbstractMapInteractiveEntity.Interactor.Mugman
				If Me.PlayerWithinDistance(1) AndAlso Map.Current.players(1).input.actions.GetButtonDown(13) Then
					Me.Activate(Map.Current.players(1))
				End If
			Case AbstractMapInteractiveEntity.Interactor.Either
				If Me.PlayerWithinDistance(0) AndAlso Map.Current.players(0).input.actions.GetButtonDown(13) Then
					Me.Activate(Map.Current.players(0))
					Return
				End If
				If Me.PlayerWithinDistance(1) AndAlso Map.Current.players(1).input.actions.GetButtonDown(13) Then
					Me.Activate(Map.Current.players(1))
					Return
				End If
			Case AbstractMapInteractiveEntity.Interactor.Both
				If Map.Current.players(0) Is Nothing OrElse Map.Current.players(1) Is Nothing Then
					Return
				End If
				If Me.PlayerWithinDistance(0) AndAlso Me.PlayerWithinDistance(1) Then
					If Map.Current.players(0).input.actions.GetButtonDown(13) AndAlso Map.Current.players(1).input.actions.GetButton(13) Then
						Me.Activate(Map.Current.players(0))
						Return
					End If
					If Map.Current.players(1).input.actions.GetButtonDown(13) AndAlso Map.Current.players(0).input.actions.GetButton(13) Then
						Me.Activate(Map.Current.players(1))
						Return
					End If
				End If
		End Select
	End Sub

	' Token: 0x060036CA RID: 14026 RVA: 0x00098D10 File Offset: 0x00097110
	Protected Function PlayersAbleToActivate() As AbstractMapInteractiveEntity.MapActivateData
		Dim mapActivateData As AbstractMapInteractiveEntity.MapActivateData
		mapActivateData.length = 0
		mapActivateData.controller1 = Nothing
		mapActivateData.controller2 = Nothing
		If Map.Current.CurrentState <> Map.State.Ready Then
			Return mapActivateData
		End If
		Select Case Me.interactor
			Case Else
				Me.playerChecking = Map.Current.players(0)
				If Me.PlayerWithinDistance(0) Then
					Return AbstractMapInteractiveEntity.MapActivateData.Fill(mapActivateData, 1, Me.playerChecking, Nothing)
				End If
			Case AbstractMapInteractiveEntity.Interactor.Mugman
				Me.playerChecking = Map.Current.players(1)
				If Me.PlayerWithinDistance(1) Then
					Return AbstractMapInteractiveEntity.MapActivateData.Fill(mapActivateData, 1, Me.playerChecking, Nothing)
				End If
			Case AbstractMapInteractiveEntity.Interactor.Either
				Me.playerChecking = Map.Current.players(0)
				If Me.PlayerWithinDistance(0) AndAlso Me.PlayerWithinDistance(1) Then
					Return AbstractMapInteractiveEntity.MapActivateData.Fill(mapActivateData, 2, Map.Current.players(0), Map.Current.players(1))
				End If
				If Me.PlayerWithinDistance(0) Then
					Return AbstractMapInteractiveEntity.MapActivateData.Fill(mapActivateData, 1, Map.Current.players(0), Nothing)
				End If
				Me.playerChecking = Map.Current.players(1)
				If Me.PlayerWithinDistance(1) Then
					Return AbstractMapInteractiveEntity.MapActivateData.Fill(mapActivateData, 1, Map.Current.players(1), Nothing)
				End If
			Case AbstractMapInteractiveEntity.Interactor.Both
				Me.playerChecking = Map.Current.players(0)
				If Me.PlayerWithinDistance(0) AndAlso Me.PlayerWithinDistance(1) Then
					Return AbstractMapInteractiveEntity.MapActivateData.Fill(mapActivateData, 2, Map.Current.players(0), Map.Current.players(1))
				End If
		End Select
		Return mapActivateData
	End Function

	' Token: 0x060036CB RID: 14027 RVA: 0x00098EC8 File Offset: 0x000972C8
	Protected Function AbleToActivate() As Boolean
		Return Me.PlayersAbleToActivate().Length > 0
	End Function

	' Token: 0x060036CC RID: 14028 RVA: 0x00098EE8 File Offset: 0x000972E8
	Public Function PlayerWithinDistance(i As Integer) As Boolean
		If Map.Current.players(i) Is Nothing OrElse Map.Current.players(i).state <> MapPlayerController.State.Walking OrElse Map.Current.players(i).hideInteractionPrompts Then
			Return False
		End If
		Dim vector As Vector2 = MyBase.transform.position + Me.interactionPoint
		Dim vector2 As Vector2 = Map.Current.players(i).transform.position
		Return Vector2.Distance(vector, vector2) <= Me.interactionDistance
	End Function

	' Token: 0x060036CD RID: 14029 RVA: 0x00098F84 File Offset: 0x00097384
	Protected Overridable Sub Check()
		Dim mapActivateData As AbstractMapInteractiveEntity.MapActivateData = Me.PlayersAbleToActivate()
		Me.showed.CopyTo(Me.checkPrevious, 0)
		For i As Integer = 0 To Me.showed.Length - 1
			Me.showed(i) = False
		Next
		For j As Integer = 0 To mapActivateData.Length - 1
			If mapActivateData(j).id < CType(Me.showed.Length, PlayerId) Then
				Me.showed(CInt(mapActivateData(j).id)) = True
			End If
		Next
		For k As Integer = 0 To Map.Current.players.Length - 1
			If Not(Map.Current.players(k) Is Nothing) Then
				Dim id As Integer = CInt(Map.Current.players(k).id)
				If Me.checkPrevious(id) <> Me.showed(id) Then
					If Me.showed(id) Then
						Me.dialogues(id) = Me.Show(Map.Current.players(k).input)
					Else
						Me.Hide(Me.dialogues(id))
						Me.dialogues(id) = Nothing
					End If
				End If
			End If
		Next
	End Sub

	' Token: 0x060036CE RID: 14030 RVA: 0x000990C4 File Offset: 0x000974C4
	Protected Overridable Sub ReCheck()
		Dim mapActivateData As AbstractMapInteractiveEntity.MapActivateData = Me.PlayersAbleToActivate()
		Me.CleanUpHiddenPrompts()
		Me.showed.CopyTo(Me.recheckPrevious, 0)
		For i As Integer = 0 To Me.showed.Length - 1
			Me.showed(i) = False
		Next
		For j As Integer = 0 To mapActivateData.Length - 1
			If mapActivateData(j).id < CType(Me.showed.Length, PlayerId) Then
				Me.showed(CInt(mapActivateData(j).id)) = True
			End If
		Next
		For k As Integer = 0 To Map.Current.players.Length - 1
			If Not(Map.Current.players(k) Is Nothing) Then
				Dim id As Integer = CInt(Map.Current.players(k).id)
				If Me.recheckPrevious(id) <> Me.showed(id) Then
					If Me.showed(id) Then
						Me.dialogues(id) = Me.Show(Map.Current.players(k).input)
					Else
						Me.Hide(Me.dialogues(id))
						Me.dialogues(id) = Nothing
					End If
				End If
			End If
		Next
	End Sub

	' Token: 0x060036CF RID: 14031 RVA: 0x00099208 File Offset: 0x00097608
	Public Overridable Sub CleanUpHiddenPrompts()
		For i As Integer = 0 To Me.showed.Length - 1
			If Me.showed(i) AndAlso Me.dialogues(i) Is Nothing Then
				Me.showed(i) = False
			End If
		Next
	End Sub

	' Token: 0x060036D0 RID: 14032 RVA: 0x00099258 File Offset: 0x00097658
	Protected Overridable Sub Activate(player As MapPlayerController)
		Dim mapUIInteractionDialogue As MapUIInteractionDialogue = Me.dialogues(CInt(player.id))
		If mapUIInteractionDialogue Is Nothing Then
			Return
		End If
		Me.playerActivating = player
		mapUIInteractionDialogue.Close()
		Me.state = AbstractMapInteractiveEntity.State.Activated
		If Me.OnActivateEvent IsNot Nothing Then
			Me.OnActivateEvent()
		End If
		Me.Activate()
	End Sub

	' Token: 0x060036D1 RID: 14033 RVA: 0x000992B2 File Offset: 0x000976B2
	Protected Overridable Sub Activate()
	End Sub

	' Token: 0x060036D2 RID: 14034 RVA: 0x000992B4 File Offset: 0x000976B4
	Protected Overridable Function Show(player As PlayerInput) As MapUIInteractionDialogue
		AudioManager.Play("world_map_level_bubble_appear")
		Me.state = AbstractMapInteractiveEntity.State.Ready
		Return MapUIInteractionDialogue.Create(Me.dialogueProperties, player, Me.dialogueOffset)
	End Function

	' Token: 0x060036D3 RID: 14035 RVA: 0x000992D9 File Offset: 0x000976D9
	Public Overridable Sub Hide(dialogue As MapUIInteractionDialogue)
		AudioManager.Play("world_map_level_bubble_disappear")
		If dialogue Is Nothing Then
			Return
		End If
		dialogue.Close()
		dialogue = Nothing
		Me.state = AbstractMapInteractiveEntity.State.Inactive
	End Sub

	' Token: 0x060036D4 RID: 14036 RVA: 0x00099304 File Offset: 0x00097704
	Public Sub SetPlayerReturnPos()
		PlayerData.Data.CurrentMapData.playerOnePosition = MyBase.transform.position + Me.returnPositions.playerOne
		PlayerData.Data.CurrentMapData.playerTwoPosition = MyBase.transform.position + Me.returnPositions.playerTwo
		If Not PlayerManager.Multiplayer Then
			PlayerData.Data.CurrentMapData.playerOnePosition = MyBase.transform.position + Me.returnPositions.singlePlayer
		End If
	End Sub

	' Token: 0x060036D5 RID: 14037 RVA: 0x000993B7 File Offset: 0x000977B7
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		AbstractMapInteractiveEntity.HasPopupOpened = False
	End Sub

	' Token: 0x060036D6 RID: 14038 RVA: 0x000993C8 File Offset: 0x000977C8
	Protected Overrides Sub OnDrawGizmosSelected()
		MyBase.OnDrawGizmosSelected()
		Gizmos.color = Color.red
		Gizmos.DrawWireSphere(MyBase.baseTransform.position + Me.dialogueOffset, 0.05F)
		Gizmos.color = Color.white
		Gizmos.DrawWireSphere(MyBase.baseTransform.position + Me.dialogueOffset, 0.06F)
		Gizmos.color = Color.green
		Gizmos.DrawWireSphere(MyBase.baseTransform.position + Me.interactionPoint, 0.05F)
		Gizmos.color = Color.white
		Gizmos.DrawWireSphere(MyBase.baseTransform.position + Me.interactionPoint, 0.06F)
		Gizmos.color = Color.green
		Gizmos.DrawWireSphere(MyBase.baseTransform.position + Me.interactionPoint, Me.interactionDistance)
		Gizmos.color = Color.white
		Gizmos.DrawWireSphere(MyBase.baseTransform.position + Me.interactionPoint, Me.interactionDistance + 0.01F)
		Dim vector As Vector3 = New Vector3(0.3F, 0.3F, 0.3F)
		Dim vector2 As Vector3 = vector * 0.9F
		Dim vector3 As Vector3 = New Vector3(0.25F, 0.25F, 0.25F)
		Dim vector4 As Vector3 = vector3 * 0.9F
		Gizmos.color = Color.white
		Gizmos.DrawWireCube(Me.returnPositions.singlePlayer + MyBase.transform.position, vector)
		Gizmos.color = Color.black
		Gizmos.DrawWireCube(Me.returnPositions.playerOne + MyBase.transform.position, vector3)
		Gizmos.DrawWireCube(Me.returnPositions.playerTwo + MyBase.transform.position, vector3)
		Gizmos.color = Color.red
		Gizmos.DrawWireCube(Me.returnPositions.singlePlayer + MyBase.transform.position, vector2)
		Gizmos.DrawWireCube(Me.returnPositions.playerOne + MyBase.transform.position, vector4)
		Gizmos.color = Color.blue
		Gizmos.DrawWireCube(Me.returnPositions.playerTwo + MyBase.transform.position, vector4)
		Gizmos.color = Color.white
	End Sub

	' Token: 0x060036D7 RID: 14039 RVA: 0x00099678 File Offset: 0x00097A78
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		If Not Application.isPlaying Then
			Return
		End If
		Select Case Me.interactor
			Case AbstractMapInteractiveEntity.Interactor.Cuphead
				Me.DrawGizmoLineToPlayer(0, Me.PlayerWithinDistance(0))
			Case AbstractMapInteractiveEntity.Interactor.Mugman
				Me.DrawGizmoLineToPlayer(1, Me.PlayerWithinDistance(1))
			Case AbstractMapInteractiveEntity.Interactor.Either
				Me.DrawGizmoLineToPlayer(0, Me.PlayerWithinDistance(0))
				Me.DrawGizmoLineToPlayer(1, Me.PlayerWithinDistance(1))
			Case AbstractMapInteractiveEntity.Interactor.Both
				Me.DrawGizmoLineToPlayer(0, Me.PlayerWithinDistance(0) AndAlso Me.PlayerWithinDistance(1))
				Me.DrawGizmoLineToPlayer(1, Me.PlayerWithinDistance(0) AndAlso Me.PlayerWithinDistance(1))
		End Select
	End Sub

	' Token: 0x060036D8 RID: 14040 RVA: 0x00099740 File Offset: 0x00097B40
	Private Sub DrawGizmoLineToPlayer(i As Integer, valid As Boolean)
		If Map.Current.players(i) Is Nothing Then
			Return
		End If
		Gizmos.color = If((Not valid), Color.red, Color.green)
		Gizmos.DrawLine(MyBase.transform.position + Me.interactionPoint, Map.Current.players(i).transform.position)
	End Sub

	' Token: 0x04003EE8 RID: 16104
	Protected Const MapWorld1 As String = "MapWorld_1"

	' Token: 0x04003EE9 RID: 16105
	Protected Const MapWorld2 As String = "MapWorld_2"

	' Token: 0x04003EEA RID: 16106
	Protected Const MapWorld3 As String = "MapWorld_3"

	' Token: 0x04003EEB RID: 16107
	Protected Const MapWorld4Exit As String = "KingDiceToWorld3WorldMap"

	' Token: 0x04003EEC RID: 16108
	Protected Const Inkwell As String = "Inkwell"

	' Token: 0x04003EED RID: 16109
	Protected Const Mausoleum As String = "Mausoleum"

	' Token: 0x04003EEE RID: 16110
	Protected Const Mausoleum1 As String = "Mausoleum_1"

	' Token: 0x04003EEF RID: 16111
	Protected Const Mausoleum2 As String = "Mausoleum_2"

	' Token: 0x04003EF0 RID: 16112
	Protected Const Mausoleum3 As String = "Mausoleum_3"

	' Token: 0x04003EF1 RID: 16113
	Protected Const Devil As String = "Devil"

	' Token: 0x04003EF2 RID: 16114
	Protected Const DicePalaceMain As String = "DicePalaceMain"

	' Token: 0x04003EF3 RID: 16115
	Protected Const KingDice As String = "KingDice"

	' Token: 0x04003EF4 RID: 16116
	Protected Const Shop As String = "Shop"

	' Token: 0x04003EF5 RID: 16117
	Protected Const ElderKettleLevel As String = "ElderKettleLevel"

	' Token: 0x04003EF6 RID: 16118
	Protected Const Kitchen As String = "BakeryWorldMap"

	' Token: 0x04003EF7 RID: 16119
	Protected Const KitchenFight As String = "Saltbaker"

	' Token: 0x04003EF8 RID: 16120
	Protected Const KingOfGamesCastle As String = "KingOfGamesWorldMap"

	' Token: 0x04003EF9 RID: 16121
	Protected Shared HasPopupOpened As Boolean

	' Token: 0x04003EFB RID: 16123
	Public interactor As AbstractMapInteractiveEntity.Interactor = AbstractMapInteractiveEntity.Interactor.Either

	' Token: 0x04003EFC RID: 16124
	Public interactionPoint As Vector2

	' Token: 0x04003EFD RID: 16125
	Public interactionDistance As Single = 1F

	' Token: 0x04003EFE RID: 16126
	Public dialogueProperties As AbstractUIInteractionDialogue.Properties

	' Token: 0x04003EFF RID: 16127
	Public dialogueOffset As Vector2

	' Token: 0x04003F00 RID: 16128
	Public returnPositions As AbstractMapInteractiveEntity.PositionProperties

	' Token: 0x04003F01 RID: 16129
	Public playerCanWalkBehind As Boolean = True

	' Token: 0x04003F05 RID: 16133
	<HideInInspector()>
	Public dialogues As MapUIInteractionDialogue() = New MapUIInteractionDialogue(1) {}

	' Token: 0x04003F06 RID: 16134
	Private lastInteractable As Boolean

	' Token: 0x04003F07 RID: 16135
	Private lockInput As Boolean

	' Token: 0x04003F08 RID: 16136
	Private showed As Boolean() = New Boolean(1) {}

	' Token: 0x04003F09 RID: 16137
	Private checkPrevious As Boolean() = New Boolean(1) {}

	' Token: 0x04003F0A RID: 16138
	Private recheckPrevious As Boolean() = New Boolean(1) {}

	' Token: 0x02000926 RID: 2342
	Public Enum Interactor
		' Token: 0x04003F0C RID: 16140
		Cuphead
		' Token: 0x04003F0D RID: 16141
		Mugman
		' Token: 0x04003F0E RID: 16142
		Either
		' Token: 0x04003F0F RID: 16143
		Both
	End Enum

	' Token: 0x02000927 RID: 2343
	Protected Enum State
		' Token: 0x04003F11 RID: 16145
		Inactive
		' Token: 0x04003F12 RID: 16146
		Ready
		' Token: 0x04003F13 RID: 16147
		Activated
	End Enum

	' Token: 0x02000928 RID: 2344
	Protected Structure MapActivateData
		' Token: 0x1700047C RID: 1148
		' (get) Token: 0x060036D9 RID: 14041 RVA: 0x000997BA File Offset: 0x00097BBA
		Public ReadOnly Property Length As Integer
			Get
				Return Me.length
			End Get
		End Property

		' Token: 0x1700047D RID: 1149
		Public ReadOnly Default Property Item(index As Integer) As MapPlayerController
			Get
				If index = 0 Then
					Return Me.controller1
				End If
				If index = 1 Then
					Return Me.controller2
				End If
				Return Nothing
			End Get
		End Property

		' Token: 0x060036DB RID: 14043 RVA: 0x000997E0 File Offset: 0x00097BE0
		Public Shared Function Fill(ByRef mapActivateData As AbstractMapInteractiveEntity.MapActivateData, length As Integer, Optional controller1 As MapPlayerController = Nothing, Optional controller2 As MapPlayerController = Nothing) As AbstractMapInteractiveEntity.MapActivateData
			mapActivateData.length = length
			If length >= 1 Then
				mapActivateData.controller1 = controller1
			End If
			If length >= 2 Then
				mapActivateData.controller2 = controller2
			End If
			Return mapActivateData
		End Function

		' Token: 0x04003F14 RID: 16148
		Public length As Integer

		' Token: 0x04003F15 RID: 16149
		Public controller1 As MapPlayerController

		' Token: 0x04003F16 RID: 16150
		Public controller2 As MapPlayerController
	End Structure

	' Token: 0x02000929 RID: 2345
	<Serializable()>
	Public Class PositionProperties
		' Token: 0x04003F17 RID: 16151
		<Header("One Player")>
		Public singlePlayer As Vector2 = New Vector2(0F, -1F)

		' Token: 0x04003F18 RID: 16152
		<Header("Two Players")>
		Public playerOne As Vector2 = New Vector2(-1F, -1F)

		' Token: 0x04003F19 RID: 16153
		Public playerTwo As Vector2 = New Vector2(1F, -1F)
	End Class
End Class
