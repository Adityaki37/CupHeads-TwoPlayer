Imports System
Imports Rewired
Imports Rewired.UI.ControlMapper
Imports UnityEngine

' Token: 0x020003ED RID: 1005
Public Class Cuphead
	Inherits AbstractMonoBehaviour

	' Token: 0x17000244 RID: 580
	' (get) Token: 0x06000D90 RID: 3472 RVA: 0x0008E44B File Offset: 0x0008C84B
	' (set) Token: 0x06000D91 RID: 3473 RVA: 0x0008E452 File Offset: 0x0008C852
	Public Shared Property Current As Cuphead

	' Token: 0x06000D92 RID: 3474 RVA: 0x0008E45C File Offset: 0x0008C85C
	Public Shared Sub Init(Optional lightInit As Boolean = False)
		If Cuphead.Current Is Nothing Then
			Global.UnityEngine.[Object].Instantiate(Of Cuphead)(Resources.Load(Of Cuphead)("Core/CupheadCore"))
		Else
			If Not Cuphead.didLightInit Then
				Return
			End If
			Cuphead.didLightInit = False
		End If
		If lightInit Then
			Cuphead.didLightInit = True
		Else
			Cuphead.Current.rewired.gameObject.SetActive(True)
			Cuphead.Current.eventSystem.gameObject.SetActive(True)
			Cuphead.Current.controlMapper.gameObject.SetActive(True)
			PlayerManager.Awake()
			If Not PlatformHelper.PreloadSettingsData Then
				OnlineManager.Instance.Init()
			End If
			PlmManager.Instance.Init()
			PlayerManager.Init()
			Cuphead.didFullInit = True
		End If
	End Sub

	' Token: 0x17000245 RID: 581
	' (get) Token: 0x06000D93 RID: 3475 RVA: 0x0008E522 File Offset: 0x0008C922
	Public ReadOnly Property ScoringProperties As ScoringEditorData
		Get
			Return Me.scoringProperties
		End Get
	End Property

	' Token: 0x17000246 RID: 582
	' (get) Token: 0x06000D94 RID: 3476 RVA: 0x0008E52A File Offset: 0x0008C92A
	' (set) Token: 0x06000D95 RID: 3477 RVA: 0x0008E532 File Offset: 0x0008C932
	Public Property achievementToastManager As AchievementToastManager

	' Token: 0x06000D96 RID: 3478 RVA: 0x0008E53C File Offset: 0x0008C93C
	Protected Overrides Sub Awake()
		MyBase.Awake()
		If Cuphead.Current Is Nothing Then
			Cuphead.Current = Me
			MyBase.gameObject.name = MyBase.gameObject.name.Replace("(Clone)", String.Empty)
			Global.UnityEngine.[Object].DontDestroyOnLoad(MyBase.gameObject)
			Me.noiseHandler = Global.UnityEngine.[Object].Instantiate(Of AudioNoiseHandler)(Me.noiseHandler)
			Me.noiseHandler.transform.SetParent(MyBase.transform)
			Dim hasBootedUpGame As Boolean = SettingsData.Data.hasBootedUpGame
			If PlatformHelper.ShowAchievements Then
				Me.achievementToastManager = Global.UnityEngine.[Object].Instantiate(Of AchievementToastManager)(Me.achievementToastManagerPrefab)
				Me.achievementToastManager.transform.SetParent(MyBase.transform)
			End If
			Return
		End If
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06000D97 RID: 3479 RVA: 0x0008E609 File Offset: 0x0008CA09
	Private Sub OnDestroy()
		If Cuphead.Current Is Me Then
			Cuphead.Current = Nothing
		End If
	End Sub

	' Token: 0x06000D98 RID: 3480 RVA: 0x0008E621 File Offset: 0x0008CA21
	Private Sub Update()
		If Cuphead.didFullInit Then
			PlayerManager.Update()
		End If
		Cursor.visible = Not Screen.fullScreen
	End Sub

	' Token: 0x04001707 RID: 5895
	Private Const PATH As String = "Core/CupheadCore"

	' Token: 0x04001708 RID: 5896
	Private Shared didLightInit As Boolean

	' Token: 0x04001709 RID: 5897
	Private Shared didFullInit As Boolean

	' Token: 0x0400170B RID: 5899
	<SerializeField()>
	Private noiseHandler As AudioNoiseHandler

	' Token: 0x0400170C RID: 5900
	<SerializeField()>
	Private rewired As InputManager

	' Token: 0x0400170D RID: 5901
	Public controlMapper As ControlMapper

	' Token: 0x0400170E RID: 5902
	<SerializeField()>
	Private eventSystem As CupheadEventSystem

	' Token: 0x0400170F RID: 5903
	<SerializeField()>
	Private renderer As CupheadRenderer

	' Token: 0x04001710 RID: 5904
	<SerializeField()>
	Private scoringProperties As ScoringEditorData

	' Token: 0x04001711 RID: 5905
	<SerializeField()>
	Private achievementToastManagerPrefab As AchievementToastManager
End Class
