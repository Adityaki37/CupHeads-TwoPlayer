Imports System
Imports UnityEngine

' Token: 0x02000409 RID: 1033
Public Class CutsceneGUI
	Inherits AbstractMonoBehaviour

	' Token: 0x1700024E RID: 590
	' (get) Token: 0x06000E6C RID: 3692 RVA: 0x000936EA File Offset: 0x00091AEA
	' (set) Token: 0x06000E6D RID: 3693 RVA: 0x000936F1 File Offset: 0x00091AF1
	Public Shared Property Current As CutsceneGUI

	' Token: 0x1700024F RID: 591
	' (get) Token: 0x06000E6E RID: 3694 RVA: 0x000936F9 File Offset: 0x00091AF9
	Public ReadOnly Property Canvas As Canvas
		Get
			Return Me.canvas
		End Get
	End Property

	' Token: 0x06000E6F RID: 3695 RVA: 0x00093701 File Offset: 0x00091B01
	Protected Overrides Sub Awake()
		MyBase.Awake()
		CutsceneGUI.Current = Me
	End Sub

	' Token: 0x06000E70 RID: 3696 RVA: 0x00093710 File Offset: 0x00091B10
	Private Sub Start()
		Me.uiCamera = Global.UnityEngine.[Object].Instantiate(Of CupheadUICamera)(Me.uiCameraPrefab)
		Me.uiCamera.transform.SetParent(MyBase.transform)
		Me.uiCamera.transform.ResetLocalTransforms()
		Me.canvas.worldCamera = Me.uiCamera.camera
	End Sub

	' Token: 0x06000E71 RID: 3697 RVA: 0x0009376A File Offset: 0x00091B6A
	Private Sub OnDestroy()
		If CutsceneGUI.Current Is Me Then
			CutsceneGUI.Current = Nothing
		End If
	End Sub

	' Token: 0x06000E72 RID: 3698 RVA: 0x00093782 File Offset: 0x00091B82
	Public Sub CutseneInit()
		Me.pause.Init(False)
	End Sub

	' Token: 0x06000E73 RID: 3699 RVA: 0x00093790 File Offset: 0x00091B90
	Protected Overridable Sub CutsceneSnapshot()
		AudioManager.HandleSnapshot(AudioManager.Snapshots.Cutscene.ToString(), 0.15F)
	End Sub

	' Token: 0x040017AC RID: 6060
	Public Const PATH As String = "UI/Cutscene_UI"

	' Token: 0x040017AE RID: 6062
	<SerializeField()>
	Private canvas As Canvas

	' Token: 0x040017AF RID: 6063
	<SerializeField()>
	Public pause As CutscenePauseGUI

	' Token: 0x040017B0 RID: 6064
	<Space(10F)>
	<SerializeField()>
	Private uiCameraPrefab As CupheadUICamera

	' Token: 0x040017B1 RID: 6065
	Private uiCamera As CupheadUICamera
End Class
