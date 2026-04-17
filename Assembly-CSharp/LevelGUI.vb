Imports System
Imports System.Diagnostics
Imports UnityEngine

' Token: 0x02000481 RID: 1153
Public Class LevelGUI
	Inherits AbstractMonoBehaviour

	' Token: 0x170002C6 RID: 710
	' (get) Token: 0x060011D5 RID: 4565 RVA: 0x000A715D File Offset: 0x000A555D
	' (set) Token: 0x060011D6 RID: 4566 RVA: 0x000A7164 File Offset: 0x000A5564
	Public Shared Property Current As LevelGUI

	' Token: 0x1400002D RID: 45
	' (add) Token: 0x060011D7 RID: 4567 RVA: 0x000A716C File Offset: 0x000A556C
	' (remove) Token: 0x060011D8 RID: 4568 RVA: 0x000A71A0 File Offset: 0x000A55A0
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Shared Event DebugOnDisableGuiEvent As Action

	' Token: 0x060011D9 RID: 4569 RVA: 0x000A71D4 File Offset: 0x000A55D4
	Public Shared Sub DebugDisableGUI()
		If LevelGUI.DebugOnDisableGuiEvent IsNot Nothing Then
			LevelGUI.DebugOnDisableGuiEvent()
		End If
	End Sub

	' Token: 0x170002C7 RID: 711
	' (get) Token: 0x060011DA RID: 4570 RVA: 0x000A71EA File Offset: 0x000A55EA
	Public ReadOnly Property Canvas As Canvas
		Get
			Return Me.canvas
		End Get
	End Property

	' Token: 0x060011DB RID: 4571 RVA: 0x000A71F2 File Offset: 0x000A55F2
	Protected Overrides Sub Awake()
		MyBase.Awake()
		LevelGUI.Current = Me
	End Sub

	' Token: 0x060011DC RID: 4572 RVA: 0x000A7200 File Offset: 0x000A5600
	Private Sub Start()
		Me.uiCamera = Global.UnityEngine.[Object].Instantiate(Of CupheadUICamera)(Me.uiCameraPrefab)
		Me.uiCamera.transform.SetParent(MyBase.transform)
		Me.uiCamera.transform.ResetLocalTransforms()
		Me.canvas.worldCamera = Me.uiCamera.camera
	End Sub

	' Token: 0x060011DD RID: 4573 RVA: 0x000A725A File Offset: 0x000A565A
	Private Sub OnDestroy()
		Me.pause = Nothing
		Me.options = Nothing
		Me.restartTowerConfirm = Nothing
		If LevelGUI.Current Is Me Then
			LevelGUI.Current = Nothing
		End If
	End Sub

	' Token: 0x060011DE RID: 4574 RVA: 0x000A7288 File Offset: 0x000A5688
	Public Sub LevelInit()
		Me.options = Me.optionsPrefab.InstantiatePrefab(Of OptionsGUI)()
		Me.options.rectTransform.SetParent(Me.optionsRoot, False)
		If PlatformHelper.ShowAchievements Then
			Me.achievements = Me.achievementsPrefab.InstantiatePrefab(Of AchievementsGUI)()
			Me.achievements.rectTransform.SetParent(Me.achievementsRoot, False)
		End If
		If Level.IsTowerOfPower Then
			Me.restartTowerConfirm = Me.restartTowerConfirmPrefab.InstantiatePrefab(Of RestartTowerConfirmGUI)()
			Me.restartTowerConfirm.rectTransform.SetParent(Me.restartTowerConfirmRoot, False)
		End If
		Me.pause.Init(True, Me.options, Me.achievements, Me.restartTowerConfirm)
	End Sub

	' Token: 0x04001B58 RID: 7000
	<SerializeField()>
	Private canvas As Canvas

	' Token: 0x04001B59 RID: 7001
	<SerializeField()>
	Private pause As LevelPauseGUI

	' Token: 0x04001B5A RID: 7002
	<SerializeField()>
	Private gameOver As LevelGameOverGUI

	' Token: 0x04001B5B RID: 7003
	<SerializeField()>
	Private optionsPrefab As OptionsGUI

	' Token: 0x04001B5C RID: 7004
	<SerializeField()>
	Private optionsRoot As RectTransform

	' Token: 0x04001B5D RID: 7005
	<SerializeField()>
	Private restartTowerConfirmPrefab As RestartTowerConfirmGUI

	' Token: 0x04001B5E RID: 7006
	<SerializeField()>
	Private restartTowerConfirmRoot As RectTransform

	' Token: 0x04001B5F RID: 7007
	<SerializeField()>
	Private achievementsPrefab As AchievementsGUI

	' Token: 0x04001B60 RID: 7008
	<SerializeField()>
	Private achievementsRoot As RectTransform

	' Token: 0x04001B61 RID: 7009
	Private options As OptionsGUI

	' Token: 0x04001B62 RID: 7010
	Private achievements As AchievementsGUI

	' Token: 0x04001B63 RID: 7011
	Private restartTowerConfirm As RestartTowerConfirmGUI

	' Token: 0x04001B64 RID: 7012
	<Space(10F)>
	<SerializeField()>
	Private uiCameraPrefab As CupheadUICamera

	' Token: 0x04001B65 RID: 7013
	Private uiCamera As CupheadUICamera
End Class
