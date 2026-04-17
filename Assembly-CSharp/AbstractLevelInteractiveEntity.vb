Imports System
Imports System.Diagnostics
Imports UnityEngine

' Token: 0x02000476 RID: 1142
Public MustInherit Class AbstractLevelInteractiveEntity
	Inherits AbstractPausableComponent

	' Token: 0x14000029 RID: 41
	' (add) Token: 0x06001184 RID: 4484 RVA: 0x00097354 File Offset: 0x00095754
	' (remove) Token: 0x06001185 RID: 4485 RVA: 0x0009738C File Offset: 0x0009578C
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnActivateEvent As Action

	' Token: 0x170002BC RID: 700
	' (get) Token: 0x06001186 RID: 4486 RVA: 0x000973C2 File Offset: 0x000957C2
	' (set) Token: 0x06001187 RID: 4487 RVA: 0x000973CA File Offset: 0x000957CA
	Protected Property state As AbstractLevelInteractiveEntity.State

	' Token: 0x170002BD RID: 701
	' (get) Token: 0x06001188 RID: 4488 RVA: 0x000973D3 File Offset: 0x000957D3
	' (set) Token: 0x06001189 RID: 4489 RVA: 0x000973DB File Offset: 0x000957DB
	Protected Private Property playerActivating As AbstractPlayerController

	' Token: 0x0600118A RID: 4490 RVA: 0x000973E4 File Offset: 0x000957E4
	Protected Overrides Sub Awake()
		MyBase.Awake()
	End Sub

	' Token: 0x0600118B RID: 4491 RVA: 0x000973EC File Offset: 0x000957EC
	Private Sub Start()
		AddHandler Localization.OnLanguageChangedEvent, AddressOf Me.OnLanguageChanged
	End Sub

	' Token: 0x0600118C RID: 4492 RVA: 0x000973FF File Offset: 0x000957FF
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		RemoveHandler Localization.OnLanguageChangedEvent, AddressOf Me.OnLanguageChanged
	End Sub

	' Token: 0x0600118D RID: 4493 RVA: 0x00097418 File Offset: 0x00095818
	Private Sub OnLanguageChanged()
		Me.Hide(PlayerId.PlayerOne)
		Me.Hide(PlayerId.PlayerTwo)
		Me.lastInteractable = Not Me.lastInteractable
	End Sub

	' Token: 0x0600118E RID: 4494 RVA: 0x00097438 File Offset: 0x00095838
	Private Sub FixedUpdate()
		Me.Check()
		If Me.state = AbstractLevelInteractiveEntity.State.Activated Then
			Return
		End If
		Select Case Me.interactor
			Case Else
				If Me.PlayerWithinDistance(PlayerId.PlayerOne) AndAlso PlayerManager.GetPlayer(PlayerId.PlayerOne).input.actions.GetButtonDown(13) AndAlso Not Me.PlayerIsDashing(PlayerId.PlayerOne) Then
					Me.Activate(PlayerManager.GetPlayer(PlayerId.PlayerOne))
				End If
			Case AbstractLevelInteractiveEntity.Interactor.Mugman
				If Me.PlayerWithinDistance(PlayerId.PlayerTwo) AndAlso PlayerManager.GetPlayer(PlayerId.PlayerTwo).input.actions.GetButtonDown(13) AndAlso Not Me.PlayerIsDashing(PlayerId.PlayerTwo) Then
					Me.Activate(PlayerManager.GetPlayer(PlayerId.PlayerTwo))
				End If
			Case AbstractLevelInteractiveEntity.Interactor.Either
				If Me.PlayerWithinDistance(PlayerId.PlayerOne) OrElse Me.PlayerWithinDistance(PlayerId.PlayerTwo) Then
					If PlayerManager.GetPlayer(PlayerId.PlayerOne).input.actions.GetButtonDown(13) AndAlso Me.PlayerWithinDistance(PlayerId.PlayerOne) AndAlso Not Me.PlayerIsDashing(PlayerId.PlayerOne) Then
						Me.Activate(PlayerManager.GetPlayer(PlayerId.PlayerOne))
						Return
					End If
					If PlayerManager.GetPlayer(PlayerId.PlayerTwo) Is Nothing Then
						Return
					End If
					If PlayerManager.GetPlayer(PlayerId.PlayerTwo).input.actions.GetButtonDown(13) AndAlso Me.PlayerWithinDistance(PlayerId.PlayerTwo) AndAlso Not Me.PlayerIsDashing(PlayerId.PlayerTwo) Then
						Me.Activate(PlayerManager.GetPlayer(PlayerId.PlayerTwo))
						Return
					End If
				End If
			Case AbstractLevelInteractiveEntity.Interactor.Both
				If PlayerManager.GetPlayer(PlayerId.PlayerOne) Is Nothing OrElse PlayerManager.GetPlayer(PlayerId.PlayerTwo) Is Nothing Then
					Return
				End If
				If Me.PlayerWithinDistance(PlayerId.PlayerOne) AndAlso Me.PlayerWithinDistance(PlayerId.PlayerTwo) Then
					If PlayerManager.GetPlayer(PlayerId.PlayerOne).input.actions.GetButtonDown(13) AndAlso Me.PlayerWithinDistance(PlayerId.PlayerOne) AndAlso PlayerManager.GetPlayer(PlayerId.PlayerTwo).input.actions.GetButton(13) AndAlso Me.PlayerWithinDistance(PlayerId.PlayerTwo) AndAlso Not Me.PlayerIsDashing(PlayerId.PlayerOne) AndAlso Not Me.PlayerIsDashing(PlayerId.PlayerTwo) Then
						Me.Activate(PlayerManager.GetPlayer(PlayerId.PlayerOne))
						Return
					End If
					If PlayerManager.GetPlayer(PlayerId.PlayerTwo).input.actions.GetButtonDown(13) AndAlso Me.PlayerWithinDistance(PlayerId.PlayerTwo) AndAlso PlayerManager.GetPlayer(PlayerId.PlayerOne).input.actions.GetButton(13) AndAlso Me.PlayerWithinDistance(PlayerId.PlayerOne) AndAlso Not Me.PlayerIsDashing(PlayerId.PlayerOne) AndAlso Not Me.PlayerIsDashing(PlayerId.PlayerTwo) Then
						Me.Activate(PlayerManager.GetPlayer(PlayerId.PlayerTwo))
						Return
					End If
				End If
		End Select
	End Sub

	' Token: 0x0600118F RID: 4495 RVA: 0x000976E0 File Offset: 0x00095AE0
	Protected Function AbleToActivate() As Boolean
		Select Case Me.interactor
			Case Else
				Return Me.PlayerWithinDistance(PlayerId.PlayerOne)
			Case AbstractLevelInteractiveEntity.Interactor.Mugman
				Return Me.PlayerWithinDistance(PlayerId.PlayerTwo)
			Case AbstractLevelInteractiveEntity.Interactor.Either
				Return Me.PlayerWithinDistance(PlayerId.PlayerOne) OrElse Me.PlayerWithinDistance(PlayerId.PlayerTwo)
			Case AbstractLevelInteractiveEntity.Interactor.Both
				Return Me.PlayerWithinDistance(PlayerId.PlayerOne) AndAlso Me.PlayerWithinDistance(PlayerId.PlayerTwo)
		End Select
	End Function

	' Token: 0x06001190 RID: 4496 RVA: 0x00097768 File Offset: 0x00095B68
	Protected Function PlayerWithinDistance(id As PlayerId) As Boolean
		If PlayerManager.GetPlayer(id) Is Nothing Then
			Return False
		End If
		Dim vector As Vector2 = MyBase.transform.position + Me.interactionPoint
		Dim vector2 As Vector2 = PlayerManager.GetPlayer(id).transform.position
		Return Vector2.Distance(vector, vector2) <= Me.interactionDistance
	End Function

	' Token: 0x06001191 RID: 4497 RVA: 0x000977CC File Offset: 0x00095BCC
	Protected Function PlayerIsDashing(id As PlayerId) As Boolean
		If PlayerManager.GetPlayer(id) Is Nothing Then
			Return False
		End If
		If PlayerManager.GetPlayer(id).GetComponent(Of LevelPlayerMotor)() IsNot Nothing Then
			Dim levelPlayerController As LevelPlayerController = CType(PlayerManager.GetPlayer(id), LevelPlayerController)
			Return levelPlayerController.motor.Dashing
		End If
		Return False
	End Function

	' Token: 0x06001192 RID: 4498 RVA: 0x0009781C File Offset: 0x00095C1C
	Protected Overridable Sub Check()
		Dim flag As Boolean = Me.AbleToActivate()
		If flag <> Me.lastInteractable Then
			If flag Then
				If Me.PlayerWithinDistance(PlayerId.PlayerOne) Then
					Me.Show(PlayerId.PlayerOne)
				ElseIf Me.PlayerWithinDistance(PlayerId.PlayerTwo) AndAlso PlayerManager.GetPlayer(PlayerId.PlayerTwo) IsNot Nothing Then
					Me.Show(PlayerId.PlayerTwo)
				End If
			ElseIf Not Me.PlayerWithinDistance(PlayerId.PlayerOne) Then
				Me.Hide(PlayerId.PlayerOne)
			ElseIf Not Me.PlayerWithinDistance(PlayerId.PlayerTwo) AndAlso PlayerManager.GetPlayer(PlayerId.PlayerTwo) IsNot Nothing Then
				Me.Hide(PlayerId.PlayerTwo)
			End If
		End If
		Me.lastInteractable = flag
	End Sub

	' Token: 0x06001193 RID: 4499 RVA: 0x000978C8 File Offset: 0x00095CC8
	Private Sub Activate(player As AbstractPlayerController)
		If Me.dialogue Is Nothing Then
			Return
		End If
		Me.playerActivating = player
		Me.dialogue.Close()
		Me.dialogue = Nothing
		Me.state = AbstractLevelInteractiveEntity.State.Activated
		If Me.OnActivateEvent IsNot Nothing Then
			Me.OnActivateEvent()
		End If
		Me.Activate()
	End Sub

	' Token: 0x06001194 RID: 4500 RVA: 0x00097923 File Offset: 0x00095D23
	Protected Overridable Sub Activate()
	End Sub

	' Token: 0x06001195 RID: 4501 RVA: 0x00097928 File Offset: 0x00095D28
	Protected Overridable Sub Show(playerId As PlayerId)
		Me.state = AbstractLevelInteractiveEntity.State.Ready
		Me.dialogueProperties.text = String.Empty
		Me.dialogue = LevelUIInteractionDialogue.Create(Me.dialogueProperties, PlayerManager.GetPlayer(playerId).input, Me.dialogueOffset, 0F, LevelUIInteractionDialogue.TailPosition.Bottom, Me.hasTarget)
	End Sub

	' Token: 0x06001196 RID: 4502 RVA: 0x0009797A File Offset: 0x00095D7A
	Protected Overridable Sub Hide(playerId As PlayerId)
		If Me.dialogue Is Nothing Then
			Return
		End If
		Me.dialogue.Close()
		Me.dialogue = Nothing
		Me.state = AbstractLevelInteractiveEntity.State.Inactive
	End Sub

	' Token: 0x06001197 RID: 4503 RVA: 0x000979A8 File Offset: 0x00095DA8
	Protected Overrides Sub OnDrawGizmosSelected()
		MyBase.OnDrawGizmosSelected()
		Gizmos.color = Color.red
		Gizmos.DrawWireSphere(MyBase.baseTransform.position + Me.dialogueOffset, Mathf.Min(5F, Me.interactionDistance))
		Gizmos.color = Color.white
		Gizmos.DrawWireSphere(MyBase.baseTransform.position + Me.dialogueOffset, Mathf.Min(6F, Me.interactionDistance + 1F))
		Gizmos.color = Color.green
		Gizmos.DrawWireSphere(MyBase.baseTransform.position + Me.interactionPoint, Me.interactionDistance)
		Gizmos.color = Color.white
		Gizmos.DrawWireSphere(MyBase.baseTransform.position + Me.interactionPoint, Me.interactionDistance + 1F)
	End Sub

	' Token: 0x06001198 RID: 4504 RVA: 0x00097AB0 File Offset: 0x00095EB0
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		If Not Application.isPlaying Then
			Return
		End If
		Select Case Me.interactor
			Case AbstractLevelInteractiveEntity.Interactor.Cuphead
				Me.DrawGizmoLineToPlayer(PlayerId.PlayerOne, Me.PlayerWithinDistance(PlayerId.PlayerOne))
			Case AbstractLevelInteractiveEntity.Interactor.Mugman
				Me.DrawGizmoLineToPlayer(PlayerId.PlayerTwo, Me.PlayerWithinDistance(PlayerId.PlayerTwo))
			Case AbstractLevelInteractiveEntity.Interactor.Either
				Me.DrawGizmoLineToPlayer(PlayerId.PlayerOne, Me.PlayerWithinDistance(PlayerId.PlayerOne))
				Me.DrawGizmoLineToPlayer(PlayerId.PlayerTwo, Me.PlayerWithinDistance(PlayerId.PlayerTwo))
			Case AbstractLevelInteractiveEntity.Interactor.Both
				Me.DrawGizmoLineToPlayer(PlayerId.PlayerOne, Me.PlayerWithinDistance(PlayerId.PlayerOne) AndAlso Me.PlayerWithinDistance(PlayerId.PlayerTwo))
				Me.DrawGizmoLineToPlayer(PlayerId.PlayerTwo, Me.PlayerWithinDistance(PlayerId.PlayerOne) AndAlso Me.PlayerWithinDistance(PlayerId.PlayerTwo))
		End Select
	End Sub

	' Token: 0x06001199 RID: 4505 RVA: 0x00097B78 File Offset: 0x00095F78
	Private Sub DrawGizmoLineToPlayer(id As PlayerId, valid As Boolean)
		If PlayerManager.GetPlayer(id) Is Nothing Then
			Return
		End If
		Gizmos.color = If((Not valid), Color.red, Color.green)
		Gizmos.DrawLine(MyBase.transform.position + Me.interactionPoint, PlayerManager.GetPlayer(id).transform.position)
	End Sub

	' Token: 0x04001B14 RID: 6932
	Public interactor As AbstractLevelInteractiveEntity.Interactor = AbstractLevelInteractiveEntity.Interactor.Either

	' Token: 0x04001B15 RID: 6933
	Public interactionPoint As Vector2

	' Token: 0x04001B16 RID: 6934
	Public interactionDistance As Single = 100F

	' Token: 0x04001B17 RID: 6935
	Public dialogueProperties As AbstractUIInteractionDialogue.Properties

	' Token: 0x04001B18 RID: 6936
	Public dialogueOffset As Vector2

	' Token: 0x04001B19 RID: 6937
	Public once As Boolean = True

	' Token: 0x04001B1A RID: 6938
	Public hasTarget As Boolean = True

	' Token: 0x04001B1D RID: 6941
	Protected dialogue As LevelUIInteractionDialogue

	' Token: 0x04001B1E RID: 6942
	Private lastInteractable As Boolean

	' Token: 0x02000477 RID: 1143
	Public Enum Interactor
		' Token: 0x04001B20 RID: 6944
		Cuphead
		' Token: 0x04001B21 RID: 6945
		Mugman
		' Token: 0x04001B22 RID: 6946
		Either
		' Token: 0x04001B23 RID: 6947
		Both
	End Enum

	' Token: 0x02000478 RID: 1144
	Protected Enum State
		' Token: 0x04001B25 RID: 6949
		Inactive
		' Token: 0x04001B26 RID: 6950
		Ready
		' Token: 0x04001B27 RID: 6951
		Activated
	End Enum
End Class
