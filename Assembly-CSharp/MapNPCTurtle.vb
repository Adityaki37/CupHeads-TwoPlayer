Imports System
Imports UnityEngine

' Token: 0x0200095C RID: 2396
Public Class MapNPCTurtle
	Inherits MapDialogueInteraction

	' Token: 0x060037F2 RID: 14322 RVA: 0x00200D34 File Offset: 0x001FF134
	Protected Overrides Sub Start()
		MyBase.Start()
		AddHandler Dialoguer.events.onEnded, AddressOf Me.OnDialogueEndedHandler
		AddHandler Dialoguer.events.onInstantlyEnded, AddressOf Me.OnDialogueEndedHandler
		AddHandler Dialoguer.events.onMessageEvent, AddressOf Me.OnDialoguerMessageEvent
		If Dialoguer.GetGlobalFloat(Me.dialoguerVariableID) < 2F Then
			If PlayerData.Data.CheckLevelsHaveMinGrade(Level.platformingLevels, LevelScoringData.Grade.P) Then
				Dialoguer.SetGlobalFloat(Me.dialoguerVariableID, 2F)
				PlayerData.SaveCurrentFile()
			ElseIf Dialoguer.GetGlobalFloat(Me.dialoguerVariableID) < 1F AndAlso PlayerData.Data.CountLevelsHaveMinGrade(Level.platformingLevels, LevelScoringData.Grade.P) > 1 Then
				Dialoguer.SetGlobalFloat(Me.dialoguerVariableID, 1F)
				PlayerData.SaveCurrentFile()
			End If
		End If
	End Sub

	' Token: 0x060037F3 RID: 14323 RVA: 0x00200E10 File Offset: 0x001FF210
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		RemoveHandler Dialoguer.events.onEnded, AddressOf Me.OnDialogueEndedHandler
		RemoveHandler Dialoguer.events.onInstantlyEnded, AddressOf Me.OnDialogueEndedHandler
		RemoveHandler Dialoguer.events.onMessageEvent, AddressOf Me.OnDialoguerMessageEvent
	End Sub

	' Token: 0x060037F4 RID: 14324 RVA: 0x00200E68 File Offset: 0x001FF268
	Private Sub OnDialoguerMessageEvent(message As String, metadata As String)
		If Me.SkipDialogueEvent Then
			Return
		End If
		If message = "Pacifist" Then
			MapEventNotification.Current.ShowTooltipEvent(TooltipEvent.Turtle)
			PlayerData.Data.unlockedBlackAndWhite = True
			PlayerData.SaveCurrentFile()
			MapUI.Current.Refresh()
		End If
	End Sub

	' Token: 0x060037F5 RID: 14325 RVA: 0x00200EB8 File Offset: 0x001FF2B8
	Protected Overrides Sub Activate(player As MapPlayerController)
		If Me.dialogues(CInt(player.id)).transform.localScale.x = 1F Then
			MyBase.Activate(player)
			If Me.colliderB.OverlapPoint(player.transform.position) Then
				MyBase.animator.SetTrigger("turn_b")
			Else
				MyBase.animator.SetTrigger("turn_a")
			End If
		End If
	End Sub

	' Token: 0x060037F6 RID: 14326 RVA: 0x00200F3A File Offset: 0x001FF33A
	Private Sub OnDialogueEndedHandler()
		MyBase.animator.SetTrigger("return")
	End Sub

	' Token: 0x04003FE0 RID: 16352
	<SerializeField()>
	Private colliderB As BoxCollider2D

	' Token: 0x04003FE1 RID: 16353
	<SerializeField()>
	Private dialoguerVariableID As Integer = 19

	' Token: 0x04003FE2 RID: 16354
	<HideInInspector()>
	Public SkipDialogueEvent As Boolean
End Class
