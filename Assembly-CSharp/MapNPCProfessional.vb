Imports System
Imports UnityEngine

' Token: 0x02000959 RID: 2393
Public Class MapNPCProfessional
	Inherits MonoBehaviour

	' Token: 0x060037DD RID: 14301 RVA: 0x00200738 File Offset: 0x001FEB38
	Private Sub Start()
		Me.AddDialoguerEvents()
		If Dialoguer.GetGlobalFloat(Me.dialoguerVariableID) < 3F Then
			Dim num As Integer = PlayerData.Data.CountLevelsHaveMinGrade(Level.world1BossLevels, LevelScoringData.Grade.AMinus)
			num += PlayerData.Data.CountLevelsHaveMinGrade(Level.world2BossLevels, LevelScoringData.Grade.AMinus)
			num += PlayerData.Data.CountLevelsHaveMinGrade(Level.world3BossLevels, LevelScoringData.Grade.AMinus)
			num += PlayerData.Data.CountLevelsHaveMinGrade(Level.world4BossLevels, LevelScoringData.Grade.AMinus)
			num += PlayerData.Data.CountLevelsHaveMinGrade(Level.platformingLevels, LevelScoringData.Grade.AMinus)
			If num >= 15 Then
				Dialoguer.SetGlobalFloat(Me.dialoguerVariableID, 3F)
				PlayerData.SaveCurrentFile()
			ElseIf num >= 10 Then
				If Dialoguer.GetGlobalFloat(Me.dialoguerVariableID) < 2F Then
					Dialoguer.SetGlobalFloat(Me.dialoguerVariableID, 2F)
					PlayerData.SaveCurrentFile()
				End If
			ElseIf num >= 5 AndAlso Dialoguer.GetGlobalFloat(Me.dialoguerVariableID) < 1F Then
				Dialoguer.SetGlobalFloat(Me.dialoguerVariableID, 1F)
				PlayerData.SaveCurrentFile()
			End If
		End If
	End Sub

	' Token: 0x060037DE RID: 14302 RVA: 0x0020084C File Offset: 0x001FEC4C
	Private Sub OnDestroy()
		Me.RemoveDialoguerEvents()
	End Sub

	' Token: 0x060037DF RID: 14303 RVA: 0x00200854 File Offset: 0x001FEC54
	Public Sub AddDialoguerEvents()
		AddHandler Dialoguer.events.onMessageEvent, AddressOf Me.OnDialoguerMessageEvent
	End Sub

	' Token: 0x060037E0 RID: 14304 RVA: 0x0020086C File Offset: 0x001FEC6C
	Public Sub RemoveDialoguerEvents()
		RemoveHandler Dialoguer.events.onMessageEvent, AddressOf Me.OnDialoguerMessageEvent
	End Sub

	' Token: 0x060037E1 RID: 14305 RVA: 0x00200884 File Offset: 0x001FEC84
	Private Sub OnDialoguerMessageEvent(message As String, metadata As String)
		If Me.SkipDialogueEvent Then
			Return
		End If
		If message = "RetroColorUnlock" Then
			MapEventNotification.Current.ShowTooltipEvent(TooltipEvent.Professional)
			PlayerData.Data.unlocked2Strip = True
			PlayerData.SaveCurrentFile()
			MapUI.Current.Refresh()
		End If
	End Sub

	' Token: 0x04003FD6 RID: 16342
	<SerializeField()>
	Private dialoguerVariableID As Integer = 20

	' Token: 0x04003FD7 RID: 16343
	<HideInInspector()>
	Public SkipDialogueEvent As Boolean
End Class
