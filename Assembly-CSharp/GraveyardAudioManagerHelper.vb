Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020006C4 RID: 1732
Public Class GraveyardAudioManagerHelper
	Inherits MonoBehaviour

	' Token: 0x170003B7 RID: 951
	' (get) Token: 0x060024D0 RID: 9424 RVA: 0x0015916C File Offset: 0x0015756C
	Public Shared ReadOnly Property Instance As GraveyardAudioManagerHelper
		Get
			Return GraveyardAudioManagerHelper._instance
		End Get
	End Property

	' Token: 0x060024D1 RID: 9425 RVA: 0x00159174 File Offset: 0x00157574
	Private Sub Awake()
		If GraveyardAudioManagerHelper._instance IsNot Nothing AndAlso GraveyardAudioManagerHelper._instance IsNot Me Then
			Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Else
			GraveyardAudioManagerHelper._instance = Me
			Me.sceneName = Scenes.scene_level_graveyard.ToString()
			MyBase.transform.parent = Nothing
			Global.UnityEngine.[Object].DontDestroyOnLoad(MyBase.gameObject)
			SceneLoader.instance.ResetBgmVolume()
			AudioManager.PlayBGM()
		End If
	End Sub

	' Token: 0x060024D2 RID: 9426 RVA: 0x001591F4 File Offset: 0x001575F4
	Private Iterator Function exit_level_cr() As IEnumerator
		AudioManager.ChangeBGMPitch(0.7F, 5F)
		While SceneLoader.CurrentlyLoading
			Yield Nothing
		End While
		AudioManager.ChangeBGMPitch(1F, 0F)
		Yield New WaitForEndOfFrame()
		SceneLoader.instance.ResetBgmVolume()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Return
	End Function

	' Token: 0x060024D3 RID: 9427 RVA: 0x00159210 File Offset: 0x00157610
	Private Sub Update()
		If Me.exitingLevel Then
			Return
		End If
		If SceneLoader.CurrentlyLoading AndAlso SceneLoader.SceneName <> Me.sceneName Then
			Me.exitingLevel = True
			MyBase.StartCoroutine(Me.exit_level_cr())
		End If
	End Sub

	' Token: 0x04002D6F RID: 11631
	Private exitingLevel As Boolean

	' Token: 0x04002D70 RID: 11632
	Private sceneName As String

	' Token: 0x04002D71 RID: 11633
	Private started As Boolean

	' Token: 0x04002D72 RID: 11634
	Private Shared _instance As GraveyardAudioManagerHelper
End Class
