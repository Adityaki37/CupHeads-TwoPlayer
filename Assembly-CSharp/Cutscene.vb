Imports System
Imports UnityEngine

' Token: 0x020003F4 RID: 1012
Public MustInherit Class Cutscene
	Inherits AbstractPausableComponent

	' Token: 0x1700024C RID: 588
	' (get) Token: 0x06000DBE RID: 3518 RVA: 0x0008F4F2 File Offset: 0x0008D8F2
	' (set) Token: 0x06000DBF RID: 3519 RVA: 0x0008F4F9 File Offset: 0x0008D8F9
	Public Shared Property Current As Cutscene

	' Token: 0x1700024D RID: 589
	' (get) Token: 0x06000DC0 RID: 3520 RVA: 0x0008F501 File Offset: 0x0008D901
	' (set) Token: 0x06000DC1 RID: 3521 RVA: 0x0008F508 File Offset: 0x0008D908
	Public Shared Property transitionProperties As SceneLoader.Properties = New SceneLoader.Properties()

	' Token: 0x06000DC2 RID: 3522 RVA: 0x0008F510 File Offset: 0x0008D910
	Public Shared Sub Load(level As Levels, cutscene As Scenes, transitionStart As SceneLoader.Transition, transitionEnd As SceneLoader.Transition, Optional icon As SceneLoader.Icon = SceneLoader.Icon.Hourglass)
		Cutscene.transitionProperties.transitionStart = transitionStart
		Cutscene.transitionProperties.transitionEnd = transitionEnd
		Cutscene.transitionProperties.icon = icon
		Cutscene.mode = Cutscene.Mode.Level
		Cutscene.levelAfterCutscene = level
		SceneLoader.LoadScene(cutscene, transitionStart, transitionEnd, SceneLoader.Icon.None, Nothing)
	End Sub

	' Token: 0x06000DC3 RID: 3523 RVA: 0x0008F54A File Offset: 0x0008D94A
	Public Shared Sub Load(scene As Scenes, cutscene As Scenes, transitionStart As SceneLoader.Transition, transitionEnd As SceneLoader.Transition, Optional icon As SceneLoader.Icon = SceneLoader.Icon.Hourglass)
		Cutscene.transitionProperties.transitionStart = transitionStart
		Cutscene.transitionProperties.transitionEnd = transitionEnd
		Cutscene.transitionProperties.icon = icon
		Cutscene.mode = Cutscene.Mode.Scene
		Cutscene.sceneAfterCutscene = scene
		SceneLoader.LoadScene(cutscene, transitionStart, transitionEnd, SceneLoader.Icon.None, Nothing)
	End Sub

	' Token: 0x06000DC4 RID: 3524 RVA: 0x0008F584 File Offset: 0x0008D984
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Cutscene.Current = Me
		Cuphead.Init(False)
		Me.CreateUI()
	End Sub

	' Token: 0x06000DC5 RID: 3525 RVA: 0x0008F5A0 File Offset: 0x0008D9A0
	Protected Overridable Sub Start()
		CupheadTime.SetAll(1F)
		InterruptingPrompt.SetCanInterrupt(True)
		Me.CreateCamera()
		Me.gui.CutseneInit()
		Me.SetRichPresence()
		If Me.translationText IsNot Nothing Then
			Me.translationText.SetActive(Localization.language <> Localization.Languages.English)
		End If
	End Sub

	' Token: 0x06000DC6 RID: 3526 RVA: 0x0008F5FB File Offset: 0x0008D9FB
	Private Sub CreateUI()
		Me.gui = Global.UnityEngine.[Object].FindObjectOfType(Of CutsceneGUI)()
		If Me.gui Is Nothing Then
			Me.gui = Resources.Load(Of CutsceneGUI)("UI/Cutscene_UI").InstantiatePrefab(Of CutsceneGUI)()
		End If
	End Sub

	' Token: 0x06000DC7 RID: 3527 RVA: 0x0008F634 File Offset: 0x0008DA34
	Private Sub CreateCamera()
		Dim cupheadCutsceneCamera As CupheadCutsceneCamera = Global.UnityEngine.[Object].FindObjectOfType(Of CupheadCutsceneCamera)()
		cupheadCutsceneCamera.Init()
	End Sub

	' Token: 0x06000DC8 RID: 3528 RVA: 0x0008F650 File Offset: 0x0008DA50
	Protected Overridable Sub OnCutsceneOver()
		Dim mode As Cutscene.Mode = Cutscene.mode
		If mode <> Cutscene.Mode.Scene Then
			If mode = Cutscene.Mode.Level Then
				SceneLoader.LoadLevel(Cutscene.levelAfterCutscene, SceneLoader.Transition.Fade, SceneLoader.Icon.Hourglass, Nothing)
			End If
		Else
			SceneLoader.LoadScene(Cutscene.sceneAfterCutscene, SceneLoader.Transition.Fade, Cutscene.transitionProperties.transitionEnd, Cutscene.transitionProperties.icon, Nothing)
		End If
	End Sub

	' Token: 0x06000DC9 RID: 3529 RVA: 0x0008F6B1 File Offset: 0x0008DAB1
	Public Sub Skip()
		Me.OnCutsceneOver()
	End Sub

	' Token: 0x06000DCA RID: 3530 RVA: 0x0008F6B9 File Offset: 0x0008DAB9
	Protected Overridable Sub SetRichPresence()
		OnlineManager.Instance.[Interface].SetRichPresence(PlayerId.Any, "Cutscene", True)
	End Sub

	' Token: 0x06000DCB RID: 3531 RVA: 0x0008F6D5 File Offset: 0x0008DAD5
	Public Function IsTranslationTextActive() As Boolean
		Return Me.translationText.activeSelf
	End Function

	' Token: 0x0400172E RID: 5934
	<SerializeField()>
	Private translationText As GameObject

	' Token: 0x04001730 RID: 5936
	Private Shared sceneAfterCutscene As Scenes

	' Token: 0x04001731 RID: 5937
	Private Shared levelAfterCutscene As Levels

	' Token: 0x04001732 RID: 5938
	Private Shared mode As Cutscene.Mode

	' Token: 0x04001733 RID: 5939
	Protected gui As CutsceneGUI

	' Token: 0x020003F5 RID: 1013
	Public Enum Mode
		' Token: 0x04001735 RID: 5941
		Scene
		' Token: 0x04001736 RID: 5942
		Level
	End Enum
End Class
