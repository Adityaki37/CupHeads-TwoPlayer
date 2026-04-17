Imports System
Imports System.Diagnostics

' Token: 0x0200040A RID: 1034
Public Class CutscenePauseGUI
	Inherits AbstractPauseGUI

	' Token: 0x14000026 RID: 38
	' (add) Token: 0x06000E75 RID: 3701 RVA: 0x00093F50 File Offset: 0x00092350
	' (remove) Token: 0x06000E76 RID: 3702 RVA: 0x00093F84 File Offset: 0x00092384
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Shared Event OnPauseEvent As Action

	' Token: 0x14000027 RID: 39
	' (add) Token: 0x06000E77 RID: 3703 RVA: 0x00093FB8 File Offset: 0x000923B8
	' (remove) Token: 0x06000E78 RID: 3704 RVA: 0x00093FEC File Offset: 0x000923EC
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Shared Event OnUnpauseEvent As Action

	' Token: 0x17000250 RID: 592
	' (get) Token: 0x06000E79 RID: 3705 RVA: 0x00094020 File Offset: 0x00092420
	Protected Overrides ReadOnly Property CanPause As Boolean
		Get
			Return PauseManager.state <> PauseManager.State.Paused AndAlso Me.pauseAllowed
		End Get
	End Property

	' Token: 0x06000E7A RID: 3706 RVA: 0x00094036 File Offset: 0x00092436
	Protected Overrides Sub OnPause()
		MyBase.OnPause()
		CupheadCutsceneCamera.Current.StartBlur()
		If CutscenePauseGUI.OnPauseEvent IsNot Nothing Then
			CutscenePauseGUI.OnPauseEvent()
		End If
	End Sub

	' Token: 0x06000E7B RID: 3707 RVA: 0x0009405C File Offset: 0x0009245C
	Protected Overrides Sub OnUnpause()
		MyBase.OnUnpause()
		CupheadCutsceneCamera.Current.EndBlur()
		If CutscenePauseGUI.OnUnpauseEvent IsNot Nothing Then
			CutscenePauseGUI.OnUnpauseEvent()
		End If
	End Sub

	' Token: 0x06000E7C RID: 3708 RVA: 0x00094082 File Offset: 0x00092482
	Private Sub OnDestroy()
		PauseManager.Unpause()
	End Sub

	' Token: 0x06000E7D RID: 3709 RVA: 0x0009408C File Offset: 0x0009248C
	Protected Overrides Sub Update()
		MyBase.Update()
		If MyBase.state <> AbstractPauseGUI.State.Paused Then
			Return
		End If
		If MyBase.GetButtonDown(CupheadButton.Pause) Then
			Me.Unpause()
			Return
		End If
		If MyBase.GetButtonDown(CupheadButton.Cancel) Then
			Me.Unpause()
			Return
		End If
		If MyBase.GetButtonDown(CupheadButton.Accept) Then
			Cutscene.Current.Skip()
			Return
		End If
	End Sub

	' Token: 0x06000E7E RID: 3710 RVA: 0x000940EB File Offset: 0x000924EB
	Private Sub Restart()
		MyBase.state = AbstractPauseGUI.State.Animating
		SceneLoader.ReloadLevel()
	End Sub

	' Token: 0x06000E7F RID: 3711 RVA: 0x000940F9 File Offset: 0x000924F9
	Private Sub StartNewGame()
		MyBase.state = AbstractPauseGUI.State.Animating
		PlayerManager.ResetPlayers()
		SceneLoader.LoadScene(Scenes.scene_title, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, Nothing)
	End Sub

	' Token: 0x06000E80 RID: 3712 RVA: 0x00094111 File Offset: 0x00092511
	Protected Overrides Sub InAnimation(i As Single)
	End Sub

	' Token: 0x06000E81 RID: 3713 RVA: 0x00094113 File Offset: 0x00092513
	Protected Overrides Sub OutAnimation(i As Single)
	End Sub

	' Token: 0x040017B4 RID: 6068
	Public pauseAllowed As Boolean = True
End Class
