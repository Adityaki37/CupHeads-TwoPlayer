Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020009B2 RID: 2482
Public Class StartScreen
	Inherits AbstractMonoBehaviour

	' Token: 0x170004B9 RID: 1209
	' (get) Token: 0x06003A36 RID: 14902 RVA: 0x002116CE File Offset: 0x0020FACE
	' (set) Token: 0x06003A37 RID: 14903 RVA: 0x002116D6 File Offset: 0x0020FAD6
	Public Property state As StartScreen.State

	' Token: 0x06003A38 RID: 14904 RVA: 0x002116DF File Offset: 0x0020FADF
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Global.UnityEngine.Debug.Log("Build version " + Application.version)
		Cuphead.Init(False)
		CupheadTime.Reset()
		PauseManager.Reset()
		Me.shouldLoadSlotSelect = False
		PlayerData.inGame = False
		PlayerManager.ResetPlayers()
	End Sub

	' Token: 0x06003A39 RID: 14905 RVA: 0x00211720 File Offset: 0x0020FB20
	Private Sub Start()
		If PlatformHelper.PreloadSettingsData Then
			SettingsData.ApplySettingsOnStartup()
		End If
		If AudioNoiseHandler.Instance IsNot Nothing Then
			AudioNoiseHandler.Instance.OpticalSound()
		End If
		If StartScreenAudio.Instance Is Nothing Then
			Dim startScreenAudio As StartScreenAudio = TryCast(Global.UnityEngine.[Object].Instantiate(Resources.Load("Audio/TitleScreenAudio")), StartScreenAudio)
			startScreenAudio.name = "StartScreenAudio"
		End If
		SettingsData.ApplySettingsOnStartup()
		MyBase.FrameDelayedCallback(AddressOf Me.StartFrontendSnapshot, 1)
		MyBase.StartCoroutine(Me.loop_cr())
	End Sub

	' Token: 0x06003A3A RID: 14906 RVA: 0x002117B0 File Offset: 0x0020FBB0
	Private Sub Update()
		Dim state As StartScreen.State = Me.state
		If state <> StartScreen.State.MDHR_Splash Then
			If state = StartScreen.State.Title Then
				Me.UpdateTitleScreen()
			End If
		Else
			Me.UpdateSplashMDHR()
		End If
	End Sub

	' Token: 0x06003A3B RID: 14907 RVA: 0x002117ED File Offset: 0x0020FBED
	Private Sub UpdateSplashMDHR()
	End Sub

	' Token: 0x06003A3C RID: 14908 RVA: 0x002117EF File Offset: 0x0020FBEF
	Private Sub UpdateTitleScreen()
		If Me.shouldLoadSlotSelect Then
			AudioManager.Play("ui_playerconfirm")
			AudioManager.Play("level_select")
			SceneLoader.LoadScene(Scenes.scene_slot_select, SceneLoader.Transition.Iris, SceneLoader.Transition.Fade, SceneLoader.Icon.None, Nothing)
			MyBase.enabled = False
			Return
		End If
	End Sub

	' Token: 0x06003A3D RID: 14909 RVA: 0x00211822 File Offset: 0x0020FC22
	Private Sub onPlayerJoined(playerId As PlayerId)
		Me.shouldLoadSlotSelect = True
	End Sub

	' Token: 0x06003A3E RID: 14910 RVA: 0x0021182C File Offset: 0x0020FC2C
	Private Iterator Function loop_cr() As IEnumerator
		Yield New WaitForSeconds(1F)
		AudioManager.Play("mdhr_logo_sting")
		Yield MyBase.StartCoroutine(Me.tweenRenderer_cr(Me.fader, 1F))
		Me.mdhrSplash.Play("Logo")
		Yield Me.mdhrSplash.WaitForAnimationToEnd(Me, "Logo", False, True)
		AudioManager.SnapshotReset(Scenes.scene_title.ToString(), 0.3F)
		If Not CreditsScreen.goodEnding Then
			AudioManager.PlayBGM()
		ElseIf DLCManager.DLCEnabled() AndAlso Not Me.forceOriginalTitleScreen() Then
			AudioManager.StartBGMAlternate(0)
			Me.titleAnimation.SetActive(False)
			Me.titleAnimationDLC.SetActive(True)
		Else
			AudioManager.PlayBGMPlaylistManually(True)
		End If
		StartScreen.initialLoadData = Nothing
		CreditsScreen.goodEnding = True
		SettingsData.Data.hasBootedUpGame = True
		Yield MyBase.StartCoroutine(Me.tweenRenderer_cr(Me.mdhrSplash.GetComponent(Of SpriteRenderer)(), 0.4F))
		Me.state = StartScreen.State.Title
		AddHandler PlayerManager.OnPlayerJoinedEvent, AddressOf Me.onPlayerJoined
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerOne, True, False)
		Return
	End Function

	' Token: 0x06003A3F RID: 14911 RVA: 0x00211847 File Offset: 0x0020FC47
	Private Sub OnDestroy()
		RemoveHandler PlayerManager.OnPlayerJoinedEvent, AddressOf Me.onPlayerJoined
	End Sub

	' Token: 0x06003A40 RID: 14912 RVA: 0x0021185C File Offset: 0x0020FC5C
	Private Iterator Function tweenRenderer_cr(renderer As SpriteRenderer, time As Single) As IEnumerator
		Dim t As Single = 0F
		Dim c As Color = renderer.color
		c.a = 1F
		Yield Nothing
		While t < time
			c.a = 1F - t / time
			renderer.color = c
			t += Time.deltaTime
			Yield Nothing
		End While
		c.a = 0F
		renderer.color = c
		Yield Nothing
		Return
	End Function

	' Token: 0x06003A41 RID: 14913 RVA: 0x0021187E File Offset: 0x0020FC7E
	Private Function forceOriginalTitleScreen() As Boolean
		If StartScreen.initialLoadData IsNot Nothing Then
			Return StartScreen.initialLoadData.forceOriginalTitleScreen
		End If
		Return SettingsData.Data.forceOriginalTitleScreen
	End Function

	' Token: 0x06003A42 RID: 14914 RVA: 0x002118A0 File Offset: 0x0020FCA0
	Protected Overridable Sub StartFrontendSnapshot()
		AudioManager.HandleSnapshot(AudioManager.Snapshots.FrontEnd.ToString(), 0.15F)
	End Sub

	' Token: 0x0400426C RID: 17004
	Public Shared initialLoadData As StartScreen.InitialLoadData

	' Token: 0x0400426E RID: 17006
	Public SelectSound As AudioClip()

	' Token: 0x0400426F RID: 17007
	<SerializeField()>
	Private mdhrSplash As Animator

	' Token: 0x04004270 RID: 17008
	<SerializeField()>
	Private fader As SpriteRenderer

	' Token: 0x04004271 RID: 17009
	<SerializeField()>
	Private titleAnimation As GameObject

	' Token: 0x04004272 RID: 17010
	<SerializeField()>
	Private titleAnimationDLC As GameObject

	' Token: 0x04004273 RID: 17011
	Private input As CupheadInput.AnyPlayerInput

	' Token: 0x04004274 RID: 17012
	Private shouldLoadSlotSelect As Boolean

	' Token: 0x04004275 RID: 17013
	Private Const PATH As String = "Audio/TitleScreenAudio"

	' Token: 0x020009B3 RID: 2483
	Public Class InitialLoadData
		' Token: 0x04004276 RID: 17014
		Public forceOriginalTitleScreen As Boolean
	End Class

	' Token: 0x020009B4 RID: 2484
	Public Enum State
		' Token: 0x04004278 RID: 17016
		Animating
		' Token: 0x04004279 RID: 17017
		MDHR_Splash
		' Token: 0x0400427A RID: 17018
		Title
	End Enum
End Class
