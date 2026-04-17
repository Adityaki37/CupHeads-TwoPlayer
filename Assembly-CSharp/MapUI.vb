Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020009A1 RID: 2465
Public Class MapUI
	Inherits AbstractMonoBehaviour

	' Token: 0x170004B2 RID: 1202
	' (get) Token: 0x060039D3 RID: 14803 RVA: 0x0020EA91 File Offset: 0x0020CE91
	' (set) Token: 0x060039D4 RID: 14804 RVA: 0x0020EA98 File Offset: 0x0020CE98
	Public Shared Property Current As MapUI

	' Token: 0x060039D5 RID: 14805 RVA: 0x0020EAA0 File Offset: 0x0020CEA0
	Public Shared Function Create() As MapUI
		Return Global.UnityEngine.[Object].Instantiate(Of MapUI)(Map.Current.MapResources.mapUI)
	End Function

	' Token: 0x060039D6 RID: 14806 RVA: 0x0020EAC3 File Offset: 0x0020CEC3
	Protected Overrides Sub Awake()
		MyBase.Awake()
		MapUI.Current = Me
		CupheadEventSystem.Init()
		AddHandler LevelGUI.DebugOnDisableGuiEvent, AddressOf Me.OnDisableGUI
	End Sub

	' Token: 0x060039D7 RID: 14807 RVA: 0x0020EAE8 File Offset: 0x0020CEE8
	Private Sub Start()
		Me.uiCamera = Global.UnityEngine.[Object].Instantiate(Of CupheadUICamera)(Me.uiCameraPrefab)
		Me.uiCamera.transform.SetParent(MyBase.transform)
		Me.uiCamera.transform.ResetLocalTransforms()
		Me.screenCanvas.worldCamera = Me.uiCamera.camera
		Me.sceneCanvas.worldCamera = CupheadMapCamera.Current.camera
		Me.hudCanvas.worldCamera = CupheadMapCamera.Current.camera
		MyBase.StartCoroutine(Me.HandleReturnToMapTooltipEvents())
	End Sub

	' Token: 0x060039D8 RID: 14808 RVA: 0x0020EB79 File Offset: 0x0020CF79
	Private Sub OnDestroy()
		RemoveHandler LevelGUI.DebugOnDisableGuiEvent, AddressOf Me.OnDisableGUI
		If MapUI.Current Is Me Then
			MapUI.Current = Nothing
		End If
		Me.pauseUI = Nothing
		Me.equipUI = Nothing
		Me.optionsPrefab = Nothing
	End Sub

	' Token: 0x060039D9 RID: 14809 RVA: 0x0020EBB8 File Offset: 0x0020CFB8
	Public Sub Init(players As MapPlayerController())
		Me.optionsUI = Me.optionsPrefab.InstantiatePrefab(Of OptionsGUI)()
		Me.optionsUI.rectTransform.SetParent(Me.optionsRoot, False)
		If PlatformHelper.ShowAchievements Then
			Me.achievementsUI = Me.achievementsPrefab.InstantiatePrefab(Of AchievementsGUI)()
			Me.achievementsUI.rectTransform.SetParent(Me.achievementsRoot, False)
		End If
		Me.pauseUI.Init(False, Me.optionsUI, Me.achievementsUI)
		Me.equipUI.Init(False)
	End Sub

	' Token: 0x060039DA RID: 14810 RVA: 0x0020EC43 File Offset: 0x0020D043
	Private Sub OnDisableGUI()
		Me.hudCanvas.enabled = False
	End Sub

	' Token: 0x060039DB RID: 14811 RVA: 0x0020EC51 File Offset: 0x0020D051
	Public Sub Refresh()
		Me.optionsUI.SetupButtons()
	End Sub

	' Token: 0x060039DC RID: 14812 RVA: 0x0020EC5E File Offset: 0x0020D05E
	Private Sub Update()
		If Not MapEventNotification.Current.showing AndAlso MapEventNotification.Current.EventQueue.Count > 0 Then
			MapEventNotification.Current.EventQueue.Dequeue()()
		End If
	End Sub

	' Token: 0x060039DD RID: 14813 RVA: 0x0020EC98 File Offset: 0x0020D098
	Private Iterator Function HandleReturnToMapTooltipEvents() As IEnumerator
		Yield New WaitForSeconds(1F)
		If PlayerData.Data.shouldShowBoatmanTooltip Then
			MapEventNotification.Current.ShowTooltipEvent(TooltipEvent.Boatman)
			PlayerData.Data.shouldShowBoatmanTooltip = False
			PlayerData.SaveCurrentFile()
		End If
		If PlayerData.Data.shouldShowShopkeepTooltip Then
			MapEventNotification.Current.ShowTooltipEvent(TooltipEvent.ShopKeep)
			PlayerData.Data.shouldShowShopkeepTooltip = False
			PlayerData.SaveCurrentFile()
		End If
		If PlayerData.Data.shouldShowTurtleTooltip Then
			MapEventNotification.Current.ShowTooltipEvent(TooltipEvent.Turtle)
			PlayerData.Data.shouldShowTurtleTooltip = False
			PlayerData.SaveCurrentFile()
		End If
		If PlayerData.Data.shouldShowForkTooltip Then
			MapEventNotification.Current.ShowTooltipEvent(TooltipEvent.Professional)
			PlayerData.Data.shouldShowForkTooltip = False
			PlayerData.SaveCurrentFile()
		End If
		Return
	End Function

	' Token: 0x040041C2 RID: 16834
	<SerializeField()>
	Private pauseUI As MapPauseUI

	' Token: 0x040041C3 RID: 16835
	<SerializeField()>
	Private equipUI As MapEquipUI

	' Token: 0x040041C4 RID: 16836
	<SerializeField()>
	Private optionsPrefab As OptionsGUI

	' Token: 0x040041C5 RID: 16837
	<SerializeField()>
	Private optionsRoot As RectTransform

	' Token: 0x040041C6 RID: 16838
	<SerializeField()>
	Private achievementsPrefab As AchievementsGUI

	' Token: 0x040041C7 RID: 16839
	<SerializeField()>
	Private achievementsRoot As RectTransform

	' Token: 0x040041C8 RID: 16840
	<Space(10F)>
	<SerializeField()>
	Public sceneCanvas As Canvas

	' Token: 0x040041C9 RID: 16841
	<SerializeField()>
	Public screenCanvas As Canvas

	' Token: 0x040041CA RID: 16842
	<SerializeField()>
	Public hudCanvas As Canvas

	' Token: 0x040041CB RID: 16843
	<Space(10F)>
	<SerializeField()>
	Private uiCameraPrefab As CupheadUICamera

	' Token: 0x040041CC RID: 16844
	Private optionsUI As OptionsGUI

	' Token: 0x040041CD RID: 16845
	Private achievementsUI As AchievementsGUI

	' Token: 0x040041CE RID: 16846
	Private uiCamera As CupheadUICamera
End Class
