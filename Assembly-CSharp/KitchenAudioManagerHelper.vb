Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020006D0 RID: 1744
Public Class KitchenAudioManagerHelper
	Inherits MonoBehaviour

	' Token: 0x170003BA RID: 954
	' (get) Token: 0x06002523 RID: 9507 RVA: 0x0015C612 File Offset: 0x0015AA12
	Public Shared ReadOnly Property Instance As KitchenAudioManagerHelper
		Get
			Return KitchenAudioManagerHelper._instance
		End Get
	End Property

	' Token: 0x06002524 RID: 9508 RVA: 0x0015C61C File Offset: 0x0015AA1C
	Private Sub Awake()
		If KitchenAudioManagerHelper._instance IsNot Nothing AndAlso KitchenAudioManagerHelper._instance IsNot Me Then
			Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Else
			KitchenAudioManagerHelper._instance = Me
			Me.sceneName = Scenes.scene_level_kitchen.ToString()
			MyBase.transform.parent = Nothing
			Global.UnityEngine.[Object].DontDestroyOnLoad(MyBase.gameObject)
			SceneLoader.instance.ResetBgmVolume()
		End If
	End Sub

	' Token: 0x06002525 RID: 9509 RVA: 0x0015C698 File Offset: 0x0015AA98
	Private Iterator Function exit_level_cr() As IEnumerator
		While SceneLoader.CurrentlyLoading
			Yield Nothing
		End While
		Yield New WaitForEndOfFrame()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Return
	End Function

	' Token: 0x06002526 RID: 9510 RVA: 0x0015C6B4 File Offset: 0x0015AAB4
	Private Sub Update()
		If Me.exitingLevel Then
			Return
		End If
		If SceneLoader.CurrentlyLoading AndAlso SceneLoader.SceneName <> Me.sceneName AndAlso SceneLoader.SceneName <> Scenes.scene_cutscene_dlc_saltbaker_prebattle.ToString() Then
			Me.exitingLevel = True
			MyBase.StartCoroutine(Me.exit_level_cr())
		End If
	End Sub

	' Token: 0x04002DCC RID: 11724
	Private exitingLevel As Boolean

	' Token: 0x04002DCD RID: 11725
	Private sceneName As String

	' Token: 0x04002DCE RID: 11726
	Private started As Boolean

	' Token: 0x04002DCF RID: 11727
	Private Shared _instance As KitchenAudioManagerHelper
End Class
