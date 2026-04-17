Imports System
Imports System.Collections
Imports Rewired
Imports UnityEngine

' Token: 0x02000939 RID: 2361
Public Class MapGraveyardHandler
	Inherits MapDialogueInteraction

	' Token: 0x17000485 RID: 1157
	' (get) Token: 0x06003736 RID: 14134 RVA: 0x001FCAC2 File Offset: 0x001FAEC2
	' (set) Token: 0x06003737 RID: 14135 RVA: 0x001FCACA File Offset: 0x001FAECA
	Public Property canReenter As Boolean

	' Token: 0x06003738 RID: 14136 RVA: 0x001FCAD4 File Offset: 0x001FAED4
	Protected Overrides Sub Start()
		Me.extantGhosts = New Animator(1) {}
		If Not PlayerData.Data.IsUnlocked(PlayerId.PlayerOne, Charm.charm_curse) AndAlso Not PlayerData.Data.IsUnlocked(PlayerId.PlayerTwo, Charm.charm_curse) Then
			For Each mapGraveyardGrave As MapGraveyardGrave In Me.grave
				mapGraveyardGrave.SetInteractable(False)
			Next
			MyBase.gameObject.SetActive(False)
			Return
		End If
		MyBase.Start()
		Me.puzzleOrder = PlayerData.Data.curseCharmPuzzleOrder
		Me.AddDialoguerEvents()
		If Not PlayerData.Data.curseCharmPuzzleComplete Then
			Me.ResetGraves()
		Else
			If Not PlayerData.Data.GetLevelData(Levels.Graveyard).completed Then
				Me.showBeam()
			End If
			Me.grave(5).SetInteractable(True)
		End If
	End Sub

	' Token: 0x06003739 RID: 14137 RVA: 0x001FCBAD File Offset: 0x001FAFAD
	Protected Overrides Function Show(player As PlayerInput) As MapUIInteractionDialogue
		If Not PlayerData.Data.GetLevelData(Levels.Graveyard).completed Then
			Return MyBase.Show(player)
		End If
		Return Nothing
	End Function

	' Token: 0x0600373A RID: 14138 RVA: 0x001FCBD1 File Offset: 0x001FAFD1
	Public Sub AddDialoguerEvents()
		AddHandler Dialoguer.events.onMessageEvent, AddressOf Me.OnDialoguerMessageEvent
	End Sub

	' Token: 0x0600373B RID: 14139 RVA: 0x001FCBE9 File Offset: 0x001FAFE9
	Public Sub RemoveDialoguerEvents()
		RemoveHandler Dialoguer.events.onMessageEvent, AddressOf Me.OnDialoguerMessageEvent
	End Sub

	' Token: 0x0600373C RID: 14140 RVA: 0x001FCC01 File Offset: 0x001FB001
	Private Sub OnDialoguerMessageEvent(message As String, metadata As String)
		If metadata = "LOADGRAVEYARD" Then
			MyBase.StartCoroutine(Me.load_fight_cr())
		End If
	End Sub

	' Token: 0x0600373D RID: 14141 RVA: 0x001FCC20 File Offset: 0x001FB020
	Private Iterator Function load_fight_cr() As IEnumerator
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, False, False)
		MyBase.SetPlayerReturnPos()
		Map.Current.CurrentState = Map.State.Graveyard
		If Map.Current.players(0) IsNot Nothing Then
			Map.Current.players(0).animator.SetTrigger("Sleep")
		End If
		If Map.Current.players(1) IsNot Nothing Then
			Map.Current.players(1).animator.SetTrigger("Sleep")
		End If
		Yield New WaitForSeconds(1F)
		SceneLoader.LoadScene(Scenes.scene_level_graveyard, SceneLoader.Transition.Blur, SceneLoader.Transition.Blur, SceneLoader.Icon.HourglassBroken, Nothing)
		Return
	End Function

	' Token: 0x0600373E RID: 14142 RVA: 0x001FCC3B File Offset: 0x001FB03B
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.RemoveDialoguerEvents()
	End Sub

	' Token: 0x0600373F RID: 14143 RVA: 0x001FCC49 File Offset: 0x001FB049
	Private Sub showBeam()
		Me.beamAnimator.Play("Aura", 1, 0F)
		Me.beamAnimator.Play("Start", 0, 0F)
		Me.beamAnimator.Update(0F)
	End Sub

	' Token: 0x06003740 RID: 14144 RVA: 0x001FCC88 File Offset: 0x001FB088
	Public Sub ActivatedGrave(index As Integer, playerNum As Integer, ghostPos As Vector3)
		If Not PlayerData.Data.curseCharmPuzzleComplete Then
			If index >= 0 AndAlso Me.entryCount < 3 Then
				Dim component As Animator = Global.UnityEngine.[Object].Instantiate(Of GameObject)(Me.ghostPrefab, ghostPos, Quaternion.identity).GetComponent(Of Animator)()
				Me.SFX_GRAVEYARD_Interact(Me.entryCount)
				If index = Me.puzzleOrder(Me.entryCount) Then
					Me.correctCount += 1
				End If
				Me.entryCount += 1
				If Me.entryCount = Me.puzzleOrder.Length Then
					If Me.correctCount = Me.entryCount Then
						component.Play("Yes")
						Me.SFX_GRAVEYARD_Positive()
						Me.showBeam()
						PlayerData.Data.curseCharmPuzzleComplete = True
						PlayerData.SaveCurrentFile()
					Else
						component.Play("No")
						Me.SFX_GRAVEYARD_Negative()
						MyBase.StartCoroutine(Me.reset_cr())
					End If
					Me.extantGhosts(0).SetTrigger("EngageEnd")
					Me.extantGhosts(1).SetTrigger("EngageEnd")
				Else
					component.Play("EngageStart")
					Me.extantGhosts(Me.entryCount - 1) = component
				End If
			End If
		ElseIf(index = -1 AndAlso Not PlayerData.Data.GetLevelData(Levels.Graveyard).completed) OrElse Me.canReenter Then
			Me.StartSpeechBubble()
		End If
	End Sub

	' Token: 0x06003741 RID: 14145 RVA: 0x001FCDF4 File Offset: 0x001FB1F4
	Private Sub UpdateReenterCodeActive()
		Select Case Me.interactor
			Case Else
				If MyBase.PlayerWithinDistance(0) Then
					Dim actions As Player = Map.Current.players(0).input.actions
					If actions.GetButton(11) AndAlso actions.GetButton(12) Then
						Me.currentDuration += CupheadTime.Delta
					Else
						Me.currentDuration = 0F
					End If
				End If
			Case AbstractMapInteractiveEntity.Interactor.Mugman
				If MyBase.PlayerWithinDistance(1) Then
					Dim actions2 As Player = Map.Current.players(1).input.actions
					If actions2.GetButton(11) AndAlso actions2.GetButton(12) Then
						Me.currentDuration += CupheadTime.Delta
					Else
						Me.currentDuration = 0F
					End If
				End If
			Case AbstractMapInteractiveEntity.Interactor.Either
				Dim flag As Boolean = False
				If MyBase.PlayerWithinDistance(0) Then
					Dim actions3 As Player = Map.Current.players(0).input.actions
					If actions3.GetButton(11) AndAlso actions3.GetButton(12) Then
						Me.currentDuration += CupheadTime.Delta
						flag = True
					End If
				End If
				If MyBase.PlayerWithinDistance(1) Then
					Dim actions4 As Player = Map.Current.players(1).input.actions
					If actions4.GetButton(11) AndAlso actions4.GetButton(12) Then
						Me.currentDuration += CupheadTime.Delta
						flag = True
					End If
				End If
				If Not flag Then
					Me.currentDuration = 0F
				End If
			Case AbstractMapInteractiveEntity.Interactor.Both
				If Map.Current.players(0) Is Nothing OrElse Map.Current.players(1) Is Nothing Then
					Me.canReenter = False
				End If
				If MyBase.PlayerWithinDistance(0) AndAlso MyBase.PlayerWithinDistance(1) Then
					If Map.Current.players(0).input.actions.GetButton(13) Then
						If Map.Current.players(1).input.actions.GetButton(13) Then
							Me.currentDuration += CupheadTime.Delta
						Else
							Me.currentDuration = 0F
						End If
					Else
						Me.currentDuration = 0F
					End If
				End If
		End Select
		If Me.currentDuration >= Me.pressDurationToReEnable AndAlso Not Me.canReenter AndAlso PlayerData.Data.GetLevelData(Levels.Graveyard).completed Then
			Me.SFX_GRAVEYARD_Positive()
			Me.showBeam()
			Me.canReenter = True
		End If
	End Sub

	' Token: 0x06003742 RID: 14146 RVA: 0x001FD0D4 File Offset: 0x001FB4D4
	Private Sub ResetGraves()
		For Each mapGraveyardGrave As MapGraveyardGrave In Me.grave
			mapGraveyardGrave.SetInteractable(True)
		Next
		Me.entryCount = 0
		Me.correctCount = 0
	End Sub

	' Token: 0x06003743 RID: 14147 RVA: 0x001FD118 File Offset: 0x001FB518
	Private Iterator Function reset_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 2F)
		Me.ResetGraves()
		Return
	End Function

	' Token: 0x06003744 RID: 14148 RVA: 0x001FD133 File Offset: 0x001FB533
	Protected Overrides Sub Update()
		Me.UpdateReenterCodeActive()
	End Sub

	' Token: 0x06003745 RID: 14149 RVA: 0x001FD13B File Offset: 0x001FB53B
	Private Sub SFX_GRAVEYARD_Activate()
		AudioManager.Play("sfx_dlc_worldmap_graveyard_activate")
	End Sub

	' Token: 0x06003746 RID: 14150 RVA: 0x001FD147 File Offset: 0x001FB547
	Private Sub SFX_GRAVEYARD_Interact(i As Integer)
		AudioManager.Play("sfx_dlc_worldmap_graveyard_interact_" + (i + 1))
	End Sub

	' Token: 0x06003747 RID: 14151 RVA: 0x001FD160 File Offset: 0x001FB560
	Private Sub SFX_GRAVEYARD_Negative()
		AudioManager.Play("sfx_dlc_worldmap_graveyard_negative")
	End Sub

	' Token: 0x06003748 RID: 14152 RVA: 0x001FD16C File Offset: 0x001FB56C
	Private Sub SFX_GRAVEYARD_Positive()
		AudioManager.Play("sfx_dlc_worldmap_graveyard_positive")
	End Sub

	' Token: 0x04003F5C RID: 16220
	<SerializeField()>
	Private graveFire As GameObject

	' Token: 0x04003F5D RID: 16221
	<SerializeField()>
	Private grave As MapGraveyardGrave()

	' Token: 0x04003F5E RID: 16222
	<SerializeField()>
	Private pressDurationToReEnable As Single = 1F

	' Token: 0x04003F5F RID: 16223
	<SerializeField()>
	Private ghostPrefab As GameObject

	' Token: 0x04003F60 RID: 16224
	<SerializeField()>
	Private beamAnimator As Animator

	' Token: 0x04003F61 RID: 16225
	Private puzzleOrder As Integer()

	' Token: 0x04003F62 RID: 16226
	Private entryCount As Integer

	' Token: 0x04003F63 RID: 16227
	Private correctCount As Integer

	' Token: 0x04003F64 RID: 16228
	Private currentDuration As Single

	' Token: 0x04003F65 RID: 16229
	Private extantGhosts As Animator()
End Class
