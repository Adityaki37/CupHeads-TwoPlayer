Imports System
Imports UnityEngine

' Token: 0x0200042D RID: 1069
Public Class SpeechInteractionPoint
	Inherits AbstractLevelInteractiveEntity

	' Token: 0x06000F9B RID: 3995 RVA: 0x00097BEE File Offset: 0x00095FEE
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.dialogueProperties.text = "Talk"
		Me.isDisabledP1 = False
	End Sub

	' Token: 0x06000F9C RID: 3996 RVA: 0x00097C10 File Offset: 0x00096010
	Protected Overrides Sub Check()
		MyBase.Check()
		If MyBase.PlayerWithinDistance(PlayerId.PlayerOne) Then
			PlayerManager.GetPlayer(PlayerId.PlayerOne).GetComponent(Of LevelPlayerMotor)().DisableJump()
			Me.isDisabledP1 = True
		ElseIf MyBase.PlayerWithinDistance(PlayerId.PlayerTwo) Then
			PlayerManager.GetPlayer(PlayerId.PlayerTwo).GetComponent(Of LevelPlayerMotor)().DisableJump()
			Me.isDisabledP2 = True
		ElseIf Me.isDisabledP1 Then
			PlayerManager.GetPlayer(PlayerId.PlayerOne).GetComponent(Of LevelPlayerMotor)().EnableJump()
			Me.isDisabledP1 = False
		ElseIf Me.isDisabledP2 Then
			PlayerManager.GetPlayer(PlayerId.PlayerTwo).GetComponent(Of LevelPlayerMotor)().EnableJump()
			Me.isDisabledP2 = False
		End If
	End Sub

	' Token: 0x06000F9D RID: 3997 RVA: 0x00097CBC File Offset: 0x000960BC
	Protected Overrides Sub Activate()
		MyBase.Activate()
		If Not Me.activated Then
			If MyBase.PlayerWithinDistance(PlayerId.PlayerOne) Then
				Me.Show(PlayerId.PlayerOne)
			End If
			If MyBase.PlayerWithinDistance(PlayerId.PlayerTwo) AndAlso PlayerManager.GetPlayer(PlayerId.PlayerTwo) IsNot Nothing Then
				Me.Show(PlayerId.PlayerTwo)
			End If
			Me.activated = True
		End If
	End Sub

	' Token: 0x040018CE RID: 6350
	<SerializeField()>
	Protected allDialogue As String()

	' Token: 0x040018CF RID: 6351
	Private activated As Boolean

	' Token: 0x040018D0 RID: 6352
	Private isDisabledP1 As Boolean

	' Token: 0x040018D1 RID: 6353
	Private isDisabledP2 As Boolean
End Class
