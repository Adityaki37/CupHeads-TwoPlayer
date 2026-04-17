Imports System
Imports UnityEngine

' Token: 0x020003CE RID: 974
Public Class AudioNoiseHandler
	Inherits AbstractMonoBehaviour

	' Token: 0x17000224 RID: 548
	' (get) Token: 0x06000CAC RID: 3244 RVA: 0x00089094 File Offset: 0x00087494
	Public Shared ReadOnly Property Instance As AudioNoiseHandler
		Get
			If AudioNoiseHandler.noiseHandler Is Nothing Then
				Dim audioNoiseHandler As AudioNoiseHandler = TryCast(Global.UnityEngine.[Object].Instantiate(Resources.Load("Audio/AudioNoiseHandler")), AudioNoiseHandler)
				audioNoiseHandler.name = "NoiseHandler"
			End If
			Return AudioNoiseHandler.noiseHandler
		End Get
	End Property

	' Token: 0x06000CAD RID: 3245 RVA: 0x000890D6 File Offset: 0x000874D6
	Protected Overrides Sub Awake()
		MyBase.Awake()
		AudioNoiseHandler.noiseHandler = Me
		MyBase.GetComponent(Of AudioSource)().ignoreListenerPause = True
		Global.UnityEngine.[Object].DontDestroyOnLoad(MyBase.gameObject)
	End Sub

	' Token: 0x06000CAE RID: 3246 RVA: 0x000890FB File Offset: 0x000874FB
	Public Sub OpticalSound()
		AudioManager.Play("optical_start")
	End Sub

	' Token: 0x06000CAF RID: 3247 RVA: 0x00089107 File Offset: 0x00087507
	Public Sub BoingSound()
		AudioManager.Play("worldmap_level_select")
	End Sub

	' Token: 0x0400163B RID: 5691
	Private Shared noiseHandler As AudioNoiseHandler

	' Token: 0x0400163C RID: 5692
	Private Const PATH As String = "Audio/AudioNoiseHandler"
End Class
