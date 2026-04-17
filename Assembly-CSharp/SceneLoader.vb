Imports System
Imports System.Collections
Imports System.Diagnostics
Imports UnityEngine
Imports UnityEngine.SceneManagement
Imports UnityEngine.U2D
Imports UnityEngine.UI

' Token: 0x02000AFC RID: 2812
Public Class SceneLoader
	Inherits AbstractMonoBehaviour

	' Token: 0x17000616 RID: 1558
	' (get) Token: 0x06004419 RID: 17433 RVA: 0x00240E71 File Offset: 0x0023F271
	Public Shared ReadOnly Property Exists As Boolean
		Get
			Return SceneLoader._instance IsNot Nothing
		End Get
	End Property

	' Token: 0x17000617 RID: 1559
	' (get) Token: 0x0600441A RID: 17434 RVA: 0x00240E7E File Offset: 0x0023F27E
	Public Shared ReadOnly Property instance As SceneLoader
		Get
			If SceneLoader._instance Is Nothing Then
				SceneLoader._instance = TryCast(Global.UnityEngine.[Object].Instantiate(Resources.Load("UI/Scene_Loader")), GameObject).GetComponent(Of SceneLoader)()
			End If
			Return SceneLoader._instance
		End Get
	End Property

	' Token: 0x17000618 RID: 1560
	' (get) Token: 0x0600441B RID: 17435 RVA: 0x00240EB3 File Offset: 0x0023F2B3
	' (set) Token: 0x0600441C RID: 17436 RVA: 0x00240EBA File Offset: 0x0023F2BA
	Public Shared Property CurrentLevel As Levels = Levels.Veggies

	' Token: 0x17000619 RID: 1561
	' (get) Token: 0x0600441D RID: 17437 RVA: 0x00240EC2 File Offset: 0x0023F2C2
	' (set) Token: 0x0600441E RID: 17438 RVA: 0x00240EC9 File Offset: 0x0023F2C9
	Public Shared Property SceneName As String = String.Empty

	' Token: 0x1700061A RID: 1562
	' (get) Token: 0x0600441F RID: 17439 RVA: 0x00240ED1 File Offset: 0x0023F2D1
	' (set) Token: 0x06004420 RID: 17440 RVA: 0x00240ED8 File Offset: 0x0023F2D8
	Public Shared Property properties As SceneLoader.Properties = New SceneLoader.Properties()

	' Token: 0x1700061B RID: 1563
	' (get) Token: 0x06004421 RID: 17441 RVA: 0x00240EE0 File Offset: 0x0023F2E0
	' (set) Token: 0x06004422 RID: 17442 RVA: 0x00240EE7 File Offset: 0x0023F2E7
	Public Shared Property CurrentContext As SceneLoader.Context

	' Token: 0x1700061C RID: 1564
	' (get) Token: 0x06004423 RID: 17443 RVA: 0x00240EEF File Offset: 0x0023F2EF
	Public Shared ReadOnly Property CurrentlyLoading As Boolean
		Get
			Return SceneLoader.currentlyLoading
		End Get
	End Property

	' Token: 0x06004424 RID: 17444 RVA: 0x00240EF8 File Offset: 0x0023F2F8
	Public Shared Sub LoadScene(sceneName As String, transitionStart As SceneLoader.Transition, transitionEnd As SceneLoader.Transition, Optional icon As SceneLoader.Icon = SceneLoader.Icon.Hourglass, Optional context As SceneLoader.Context = Nothing)
		Dim scenes As Scenes = Scenes.scene_start
		If Not EnumUtils.TryParse(Of Scenes)(sceneName, scenes) Then
			Return
		End If
		SceneLoader.LoadScene(scenes, transitionStart, transitionEnd, icon, context)
	End Sub

	' Token: 0x06004425 RID: 17445 RVA: 0x00240F20 File Offset: 0x0023F320
	Public Shared Sub LoadScene(scene As Scenes, transitionStart As SceneLoader.Transition, transitionEnd As SceneLoader.Transition, Optional icon As SceneLoader.Icon = SceneLoader.Icon.Hourglass, Optional context As SceneLoader.Context = Nothing)
		If SceneLoader.currentlyLoading Then
			Return
		End If
		InterruptingPrompt.SetCanInterrupt(False)
		SceneLoader.properties.transitionStart = transitionStart
		SceneLoader.properties.transitionEnd = transitionEnd
		SceneLoader.properties.icon = icon
		SceneLoader.EndTransitionDelay = 0.6F
		SceneLoader.previousSceneName = SceneLoader.SceneName
		SceneLoader.SceneName = scene.ToString()
		SceneLoader.CurrentContext = context
		SceneLoader.instance.load()
	End Sub

	' Token: 0x06004426 RID: 17446 RVA: 0x00240F96 File Offset: 0x0023F396
	Public Shared Sub LoadLevel(level As Levels, transitionStart As SceneLoader.Transition, Optional icon As SceneLoader.Icon = SceneLoader.Icon.Hourglass, Optional context As SceneLoader.Context = Nothing)
		SceneLoader.CurrentLevel = level
		SceneLoader.LoadScene(LevelProperties.GetLevelScene(level), transitionStart, SceneLoader.Transition.Iris, icon, context)
	End Sub

	' Token: 0x06004427 RID: 17447 RVA: 0x00240FB0 File Offset: 0x0023F3B0
	Public Shared Sub LoadDicePalaceLevel(dicePalaceLevel As DicePalaceLevels)
		Dim dicePalaceLevel2 As Levels = LevelProperties.GetDicePalaceLevel(dicePalaceLevel)
		SceneLoader.CurrentLevel = dicePalaceLevel2
		SceneLoader.LoadScene(LevelProperties.GetLevelScene(dicePalaceLevel2), SceneLoader.Transition.Fade, SceneLoader.Transition.Fade, SceneLoader.Icon.None, Nothing)
	End Sub

	' Token: 0x06004428 RID: 17448 RVA: 0x00240FD9 File Offset: 0x0023F3D9
	Public Shared Sub SetCurrentLevel(level As Levels)
		SceneLoader.CurrentLevel = level
	End Sub

	' Token: 0x06004429 RID: 17449 RVA: 0x00240FE4 File Offset: 0x0023F3E4
	Public Shared Sub ContinueTowerOfPower()
		Dim current_TURN As Integer = TowerOfPowerLevelGameInfo.CURRENT_TURN
		Dim turn_COUNTER As Integer = TowerOfPowerLevelGameInfo.TURN_COUNTER
		If current_TURN = turn_COUNTER Then
			TowerOfPowerLevelGameInfo.TURN_COUNTER += 1
		End If
		If TowerOfPowerLevelGameInfo.TURN_COUNTER = TowerOfPowerLevelGameInfo.allStageSpaces.Count Then
			TowerOfPowerLevelGameInfo.GameInfo.CleanUp()
			SceneLoader.LoadLastMap()
		Else
			SceneLoader.LoadScene(LevelProperties.GetLevelScene(Levels.TowerOfPower), SceneLoader.Transition.Fade, SceneLoader.Transition.Fade, SceneLoader.Icon.None, Nothing)
		End If
	End Sub

	' Token: 0x0600442A RID: 17450 RVA: 0x0024104B File Offset: 0x0023F44B
	Public Shared Sub ResetTheTowerOfPower()
		TowerOfPowerLevelGameInfo.ResetTowerOfPower()
		SceneLoader.LoadScene(LevelProperties.GetLevelScene(Levels.TowerOfPower), SceneLoader.Transition.Fade, SceneLoader.Transition.Fade, SceneLoader.Icon.None, Nothing)
	End Sub

	' Token: 0x0600442B RID: 17451 RVA: 0x00241068 File Offset: 0x0023F468
	Public Shared Sub ReloadLevel()
		If Level.IsTowerOfPower Then
			If TowerOfPowerLevelGameInfo.IsTokenLeft(0) Then
				TowerOfPowerLevelGameInfo.PLAYER_STATS(0).HP = 3
				TowerOfPowerLevelGameInfo.PLAYER_STATS(0).BonusHP = 3
				TowerOfPowerLevelGameInfo.PLAYER_STATS(0).SuperCharge = 0F
				TowerOfPowerLevelGameInfo.ReduceToken(0)
			Else
				TowerOfPowerLevelGameInfo.PLAYER_STATS(0).HP = 0
				TowerOfPowerLevelGameInfo.PLAYER_STATS(0).BonusHP = 0
				TowerOfPowerLevelGameInfo.PLAYER_STATS(0).SuperCharge = 0F
			End If
			If PlayerManager.Multiplayer Then
				If TowerOfPowerLevelGameInfo.IsTokenLeft(1) Then
					TowerOfPowerLevelGameInfo.PLAYER_STATS(1).HP = 3
					TowerOfPowerLevelGameInfo.PLAYER_STATS(1).BonusHP = 3
					TowerOfPowerLevelGameInfo.PLAYER_STATS(1).SuperCharge = 0F
					TowerOfPowerLevelGameInfo.ReduceToken(1)
				Else
					TowerOfPowerLevelGameInfo.PLAYER_STATS(1).HP = 0
					TowerOfPowerLevelGameInfo.PLAYER_STATS(1).BonusHP = 0
					TowerOfPowerLevelGameInfo.PLAYER_STATS(1).SuperCharge = 0F
				End If
			End If
		Else
			If Level.IsDicePalace Then
				SceneLoader.LoadDicePalaceLevel(DicePalaceLevels.DicePalaceMain)
				Return
			End If
			If Level.IsGraveyard Then
				SceneLoader.LoadScene(LevelProperties.GetLevelScene(SceneLoader.CurrentLevel), SceneLoader.Transition.Fade, SceneLoader.Transition.Blur, SceneLoader.Icon.None, Nothing)
				Return
			End If
			If Level.IsChessBoss Then
				If TypeOf SceneLoader.CurrentContext Is GauntletContext AndAlso Not CType(SceneLoader.CurrentContext, GauntletContext).complete Then
					Dim scenes As Scenes = Scenes.scene_level_chess_pawn
					Dim transition As SceneLoader.Transition = SceneLoader.Transition.Fade
					Dim transition2 As SceneLoader.Transition = SceneLoader.Transition.Iris
					Dim gauntletContext As GauntletContext = New GauntletContext(False)
					SceneLoader.LoadScene(scenes, transition, transition2, SceneLoader.Icon.Hourglass, gauntletContext)
					Return
				End If
				PlayerData.Data.IncrementKingOfGamesCounter()
				PlayerData.SaveCurrentFile()
			End If
		End If
		Dim transitionStartTime As Single = SceneLoader.properties.transitionStartTime
		SceneLoader.properties.transitionStartTime = 0.25F
		SceneLoader.LoadLevel(SceneLoader.CurrentLevel, SceneLoader.Transition.Fade, SceneLoader.Icon.None, Nothing)
		SceneLoader.properties.transitionStartTime = transitionStartTime
	End Sub

	' Token: 0x0600442C RID: 17452 RVA: 0x00241224 File Offset: 0x0023F624
	Public Shared Sub LoadLastMap()
		If Level.IsGraveyard Then
			SceneLoader.LoadScene(PlayerData.Data.CurrentMap, SceneLoader.Transition.Fade, SceneLoader.Transition.Blur, SceneLoader.Icon.Hourglass, Nothing)
			SceneLoader.IsInBlurTransition = True
		Else
			Dim scenes As Scenes = PlayerData.Data.CurrentMap
			If Level.IsChessBoss Then
				PlayerData.Data.IncrementKingOfGamesCounter()
				PlayerData.SaveCurrentFile()
				Dim flag As Boolean = PlayerData.Data.CountLevelsCompleted(Level.kingOfGamesLevels) = Level.kingOfGamesLevels.Length
				If flag Then
					scenes = Scenes.scene_level_chess_castle
				End If
			End If
			SceneLoader.LoadScene(scenes, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, Nothing)
		End If
	End Sub

	' Token: 0x0600442D RID: 17453 RVA: 0x002412A9 File Offset: 0x0023F6A9
	Public Shared Sub TransitionOut()
		SceneLoader.TransitionOut(SceneLoader.properties.transitionStart)
	End Sub

	' Token: 0x0600442E RID: 17454 RVA: 0x002412BA File Offset: 0x0023F6BA
	Public Shared Sub TransitionOut(transition As SceneLoader.Transition)
		SceneLoader.TransitionOut(transition, SceneLoader.properties.transitionStartTime)
	End Sub

	' Token: 0x0600442F RID: 17455 RVA: 0x002412CC File Offset: 0x0023F6CC
	Public Shared Sub TransitionOut(transition As SceneLoader.Transition, time As Single)
		SceneLoader.properties.transitionStart = transition
		SceneLoader.properties.transitionStartTime = time
		SceneLoader.instance.Out()
	End Sub

	' Token: 0x140000C0 RID: 192
	' (add) Token: 0x06004430 RID: 17456 RVA: 0x002412F0 File Offset: 0x0023F6F0
	' (remove) Token: 0x06004431 RID: 17457 RVA: 0x00241324 File Offset: 0x0023F724
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Shared Event OnFadeInStartEvent As SceneLoader.FadeHandler

	' Token: 0x140000C1 RID: 193
	' (add) Token: 0x06004432 RID: 17458 RVA: 0x00241358 File Offset: 0x0023F758
	' (remove) Token: 0x06004433 RID: 17459 RVA: 0x0024138C File Offset: 0x0023F78C
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Shared Event OnFadeInEndEvent As Action

	' Token: 0x140000C2 RID: 194
	' (add) Token: 0x06004434 RID: 17460 RVA: 0x002413C0 File Offset: 0x0023F7C0
	' (remove) Token: 0x06004435 RID: 17461 RVA: 0x002413F4 File Offset: 0x0023F7F4
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Shared Event OnFadeOutStartEvent As SceneLoader.FadeHandler

	' Token: 0x140000C3 RID: 195
	' (add) Token: 0x06004436 RID: 17462 RVA: 0x00241428 File Offset: 0x0023F828
	' (remove) Token: 0x06004437 RID: 17463 RVA: 0x0024145C File Offset: 0x0023F85C
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Shared Event OnFadeOutEndEvent As Action

	' Token: 0x140000C4 RID: 196
	' (add) Token: 0x06004438 RID: 17464 RVA: 0x00241490 File Offset: 0x0023F890
	' (remove) Token: 0x06004439 RID: 17465 RVA: 0x002414C4 File Offset: 0x0023F8C4
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Shared Event OnFaderValue As SceneLoader.FadeHandler

	' Token: 0x140000C5 RID: 197
	' (add) Token: 0x0600443A RID: 17466 RVA: 0x002414F8 File Offset: 0x0023F8F8
	' (remove) Token: 0x0600443B RID: 17467 RVA: 0x0024152C File Offset: 0x0023F92C
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Shared Event OnLoaderCompleteEvent As Action

	' Token: 0x0600443C RID: 17468 RVA: 0x00241560 File Offset: 0x0023F960
	Protected Overrides Sub Awake()
		MyBase.Awake()
		SceneLoader._instance = Me
		Me.SetIconAlpha(0F)
		Global.UnityEngine.[Object].DontDestroyOnLoad(MyBase.gameObject)
	End Sub

	' Token: 0x0600443D RID: 17469 RVA: 0x00241584 File Offset: 0x0023F984
	Private Sub load()
		If SceneLoader.SceneName <> Scenes.scene_slot_select.ToString() AndAlso SceneLoader.SceneName <> Scenes.scene_cutscene_dlc_saltbaker_prebattle.ToString() Then
			AudioManager.HandleSnapshot(AudioManager.Snapshots.Loadscreen.ToString(), 5F)
		End If
		MyBase.StartCoroutine(Me.loop_cr())
	End Sub

	' Token: 0x0600443E RID: 17470 RVA: 0x002415F4 File Offset: 0x0023F9F4
	Private Sub [In]()
		MyBase.StartCoroutine(Me.in_cr())
	End Sub

	' Token: 0x0600443F RID: 17471 RVA: 0x00241603 File Offset: 0x0023FA03
	Private Sub Out()
		If Not MyBase.gameObject.activeInHierarchy Then
			If SceneLoader.OnFadeOutEndEvent IsNot Nothing Then
				SceneLoader.OnFadeOutEndEvent()
			End If
			Return
		End If
		MyBase.StartCoroutine(Me.out_cr())
	End Sub

	' Token: 0x06004440 RID: 17472 RVA: 0x00241637 File Offset: 0x0023FA37
	Private Sub UpdateProgress(progress As Single)
	End Sub

	' Token: 0x06004441 RID: 17473 RVA: 0x00241639 File Offset: 0x0023FA39
	Private Sub SetIconAlpha(a As Single)
		Me.SetImageAlpha(Me.icon, a)
	End Sub

	' Token: 0x06004442 RID: 17474 RVA: 0x00241648 File Offset: 0x0023FA48
	Private Sub SetFaderAlpha(a As Single)
		Me.SetImageAlpha(Me.fader, a)
	End Sub

	' Token: 0x06004443 RID: 17475 RVA: 0x00241658 File Offset: 0x0023FA58
	Private Sub SetImageAlpha(i As Image, a As Single)
		Dim color As Color = i.color
		color.a = a
		i.color = color
	End Sub

	' Token: 0x06004444 RID: 17476 RVA: 0x0024167C File Offset: 0x0023FA7C
	Private Iterator Function loop_cr() As IEnumerator
		SceneLoader.currentlyLoading = True
		Yield MyBase.StartCoroutine(Me.in_cr())
		MyBase.StartCoroutine(Me.load_cr())
		Yield MyBase.StartCoroutine(Me.iconFadeIn_cr())
		While Not Me.doneLoadingSceneAsync
			Yield Nothing
		End While
		If SceneLoader.SceneName <> Scenes.scene_slot_select.ToString() Then
			AudioManager.SnapshotReset(SceneLoader.SceneName, 0.15F)
		End If
		Dim op As AsyncOperation = Resources.UnloadUnusedAssets()
		While Not op.isDone
			Yield Nothing
		End While
		Yield MyBase.StartCoroutine(Me.iconFadeOut_cr())
		Yield MyBase.StartCoroutine(Me.out_cr())
		SceneLoader.properties.Reset()
		SceneLoader.currentlyLoading = False
		Return
	End Function

	' Token: 0x06004445 RID: 17477 RVA: 0x00241698 File Offset: 0x0023FA98
	Private Iterator Function load_cr() As IEnumerator
		Me.doneLoadingSceneAsync = False
		GC.Collect()
		If SceneLoader.SceneName <> SceneLoader.previousSceneName AndAlso SceneLoader.SceneName <> Scenes.scene_slot_select.ToString() Then
			Dim text As String = Nothing
			If Not Array.Exists(Of Levels)(Level.kingOfGamesLevelsWithCastle, Function(level As Levels) LevelProperties.GetLevelScene(level) = SceneLoader.SceneName) Then
				text = Scenes.scene_level_chess_castle.ToString()
			End If
			AssetBundleLoader.UnloadAssetBundles()
			AssetLoader(Of SpriteAtlas).UnloadAssets(New String() { text })
			If SceneLoader.SceneName <> Scenes.scene_cutscene_dlc_saltbaker_prebattle.ToString() Then
				AssetLoader(Of AudioClip).UnloadAssets(New String(-1) {})
			End If
			AssetLoader(Of Texture2D()).UnloadAssets(New String(-1) {})
		End If
		If SceneLoader.SceneName = Scenes.scene_title.ToString() Then
			DLCManager.RefreshDLC()
		End If
		Dim atlasOption As AssetLoaderOption = AssetLoaderOption.None()
		If SceneLoader.SceneName = Scenes.scene_level_chess_castle.ToString() Then
			atlasOption = AssetLoaderOption.PersistInCacheTagged(SceneLoader.SceneName)
		End If
		Dim preloadAtlases As String() = AssetLoader(Of SpriteAtlas).GetPreloadAssetNames(SceneLoader.SceneName)
		Dim preloadMusic As String() = AssetLoader(Of AudioClip).GetPreloadAssetNames(SceneLoader.SceneName)
		If SceneLoader.SceneName <> SceneLoader.previousSceneName AndAlso (preloadAtlases.Length > 0 OrElse preloadMusic.Length > 0) Then
			Dim intermediateSceneAsyncOp As AsyncOperation = SceneManager.LoadSceneAsync(Me.LOAD_SCENE_NAME)
			While Not intermediateSceneAsyncOp.isDone
				Yield Nothing
			End While
			For i As Integer = 0 To preloadAtlases.Length - 1
				Yield AssetLoader(Of SpriteAtlas).LoadAsset(preloadAtlases(i), atlasOption)
			Next
			Dim musicOption As AssetLoaderOption = AssetLoaderOption.None()
			For j As Integer = 0 To preloadMusic.Length - 1
				Yield AssetLoader(Of AudioClip).LoadAsset(preloadMusic(j), musicOption)
			Next
			Dim persistentAssetsCoroutines As Coroutine() = DLCManager.LoadPersistentAssets()
			If persistentAssetsCoroutines IsNot Nothing Then
				For k As Integer = 0 To persistentAssetsCoroutines.Length - 1
					Yield persistentAssetsCoroutines(k)
				Next
			End If
			Yield Nothing
		End If
		Dim async As AsyncOperation = SceneManager.LoadSceneAsync(SceneLoader.SceneName)
		While Not async.isDone OrElse AssetBundleLoader.loadCounter > 0
			Me.UpdateProgress(async.progress)
			Yield Nothing
		End While
		Me.doneLoadingSceneAsync = True
		Return
	End Function

	' Token: 0x06004446 RID: 17478 RVA: 0x002416B4 File Offset: 0x0023FAB4
	Private Iterator Function in_cr() As IEnumerator
		Select Case SceneLoader.properties.transitionStart
			Case Else
				If SceneLoader.SceneName <> Scenes.scene_slot_select.ToString() AndAlso SceneLoader.SceneName <> Scenes.scene_cutscene_dlc_saltbaker_prebattle.ToString() Then
					Me.FadeOutBGM(0.6F)
				End If
				Yield MyBase.StartCoroutine(Me.irisIn_cr())
			Case SceneLoader.Transition.Fade
				If SceneLoader.SceneName <> Scenes.scene_slot_select.ToString() AndAlso SceneLoader.SceneName <> Scenes.scene_level_graveyard.ToString() AndAlso SceneLoader.SceneName <> Scenes.scene_cutscene_dlc_saltbaker_prebattle.ToString() AndAlso (SceneLoader.CurrentLevel <> Levels.Saltbaker OrElse SceneLoader.SceneName <> Scenes.scene_win.ToString()) Then
					Me.FadeOutBGM(SceneLoader.properties.transitionEndTime)
				End If
				Yield MyBase.StartCoroutine(Me.faderFadeIn_cr())
			Case SceneLoader.Transition.Blur
				Yield MyBase.StartCoroutine(Me.blurIn_cr())
			Case SceneLoader.Transition.None
				Me.SetFaderAlpha(1F)
		End Select
		Return
	End Function

	' Token: 0x06004447 RID: 17479 RVA: 0x002416D0 File Offset: 0x0023FAD0
	Private Iterator Function out_cr() As IEnumerator
		Yield Nothing
		Select Case SceneLoader.properties.transitionEnd
			Case Else
				Yield MyBase.StartCoroutine(Me.irisOut_cr())
			Case SceneLoader.Transition.Fade
				Yield MyBase.StartCoroutine(Me.faderFadeOut_cr())
			Case SceneLoader.Transition.Blur
				Yield MyBase.StartCoroutine(Me.blurOut_cr())
			Case SceneLoader.Transition.None
				Me.SetFaderAlpha(0F)
		End Select
		If SceneLoader.SceneName <> Scenes.scene_slot_select.ToString() AndAlso Not Level.IsGraveyard AndAlso SceneLoader.SceneName <> Scenes.scene_cutscene_dlc_saltbaker_prebattle.ToString() Then
			Me.ResetBgmVolume()
		End If
		If SceneLoader.OnLoaderCompleteEvent IsNot Nothing Then
			SceneLoader.OnLoaderCompleteEvent()
		End If
		SceneLoader.OnLoaderCompleteEvent = Nothing
		Return
	End Function

	' Token: 0x06004448 RID: 17480 RVA: 0x002416EC File Offset: 0x0023FAEC
	Private Iterator Function irisIn_cr() As IEnumerator
		SceneLoader.IsInIrisTransition = True
		Dim animator As Animator = Me.fader.GetComponent(Of Animator)()
		animator.SetTrigger("Iris_In")
		Me.SetFaderAlpha(1F)
		If SceneLoader.OnFadeInStartEvent IsNot Nothing Then
			SceneLoader.OnFadeInStartEvent(0.6F)
		End If
		Yield New WaitForSeconds(0.6F)
		If SceneLoader.OnFadeInEndEvent IsNot Nothing Then
			SceneLoader.OnFadeInEndEvent()
		End If
		Return
	End Function

	' Token: 0x06004449 RID: 17481 RVA: 0x00241708 File Offset: 0x0023FB08
	Private Iterator Function irisOut_cr() As IEnumerator
		Dim animator As Animator = Me.fader.GetComponent(Of Animator)()
		animator.SetTrigger("Iris_Out")
		Me.SetFaderAlpha(1F)
		If SceneLoader.OnFadeOutStartEvent IsNot Nothing Then
			SceneLoader.OnFadeOutStartEvent(0.6F)
		End If
		Yield New WaitForSeconds(0.6F)
		If SceneLoader.OnFadeOutEndEvent IsNot Nothing Then
			SceneLoader.OnFadeOutEndEvent()
		End If
		SceneLoader.IsInIrisTransition = False
		Return
	End Function

	' Token: 0x0600444A RID: 17482 RVA: 0x00241724 File Offset: 0x0023FB24
	Private Iterator Function faderFadeIn_cr() As IEnumerator
		SceneLoader.IsInIrisTransition = False
		Me.SetFaderAlpha(0F)
		Dim animator As Animator = Me.fader.GetComponent(Of Animator)()
		animator.SetTrigger("Black")
		If SceneLoader.OnFadeInStartEvent IsNot Nothing Then
			SceneLoader.OnFadeInStartEvent(SceneLoader.properties.transitionStartTime)
		End If
		Yield MyBase.StartCoroutine(Me.imageFade_cr(Me.fader, SceneLoader.properties.transitionStartTime, 0F, 1F, False))
		If SceneLoader.OnFadeInEndEvent IsNot Nothing Then
			SceneLoader.OnFadeInEndEvent()
		End If
		Return
	End Function

	' Token: 0x0600444B RID: 17483 RVA: 0x00241740 File Offset: 0x0023FB40
	Private Iterator Function faderFadeOut_cr() As IEnumerator
		If SceneLoader.OnFadeOutStartEvent IsNot Nothing Then
			SceneLoader.OnFadeOutStartEvent(SceneLoader.properties.transitionEndTime)
		End If
		Yield MyBase.StartCoroutine(Me.imageFade_cr(Me.fader, SceneLoader.properties.transitionEndTime, 1F, 0F, False))
		If SceneLoader.OnFadeOutEndEvent IsNot Nothing Then
			SceneLoader.OnFadeOutEndEvent()
		End If
		Return
	End Function

	' Token: 0x0600444C RID: 17484 RVA: 0x0024175C File Offset: 0x0023FB5C
	Private Iterator Function blurIn_cr() As IEnumerator
		SceneLoader.IsInBlurTransition = True
		Yield CupheadTime.WaitForSeconds(Me, 0.5F)
		Dim cam As AbstractCupheadGameCamera = If((Not(CupheadLevelCamera.Current IsNot Nothing)), CupheadMapCamera.Current, CupheadLevelCamera.Current)
		cam.StartBlur(0.5F, 2F)
		AudioManager.ChangeBGMPitch(0.9F, 0.5F)
		Yield CupheadTime.WaitForSeconds(Me, 0.5F)
		cam.EndBlur(0.5F)
		AudioManager.ChangeBGMPitch(1F, 0.5F)
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		cam.StartBlur(3F, 5F)
		AudioManager.ChangeBGMPitch(0.7F, 7F)
		Yield CupheadTime.WaitForSeconds(Me, 0.5F)
		SceneLoader.properties.transitionStartTime = 3F
		Me.FadeOutBGM(6F)
		Yield MyBase.StartCoroutine(Me.faderFadeIn_cr())
		Return
	End Function

	' Token: 0x0600444D RID: 17485 RVA: 0x00241778 File Offset: 0x0023FB78
	Private Iterator Function blurOut_cr() As IEnumerator
		SceneLoader.IsInBlurTransition = True
		Dim cam As AbstractCupheadGameCamera = If((Not(CupheadLevelCamera.Current IsNot Nothing)), CupheadMapCamera.Current, CupheadLevelCamera.Current)
		cam.StartBlur(0.01F, 5F)
		Yield New WaitForSeconds(0.015F)
		cam.EndBlur(2.5F, 5F)
		SceneLoader.properties.transitionEndTime = 2F
		Yield MyBase.StartCoroutine(Me.faderFadeOut_cr())
		cam.StartBlur(0.5F, 5F)
		Yield New WaitForSeconds(0.5F)
		cam.EndBlur(0.5F, 5F)
		Yield New WaitForSeconds(0.5F)
		SceneLoader.IsInBlurTransition = False
		Return
	End Function

	' Token: 0x0600444E RID: 17486 RVA: 0x00241794 File Offset: 0x0023FB94
	Private Iterator Function iconFadeIn_cr() As IEnumerator
		If SceneLoader.properties.icon = SceneLoader.Icon.None Then
			Me.SetIconAlpha(0F)
		Else
			Dim animator As Animator = Me.icon.GetComponent(Of Animator)()
			animator.SetTrigger(SceneLoader.properties.icon.ToString())
			Yield MyBase.StartCoroutine(Me.imageFade_cr(Me.icon, 0.4F, 0F, 1F, True))
		End If
		Return
	End Function

	' Token: 0x0600444F RID: 17487 RVA: 0x002417B0 File Offset: 0x0023FBB0
	Private Iterator Function iconFadeOut_cr() As IEnumerator
		If SceneLoader.properties.icon = SceneLoader.Icon.None Then
			Me.SetIconAlpha(0F)
			Yield New WaitForSeconds(0.6F)
		Else
			Dim startAlpha As Single = Me.icon.color.a
			Yield MyBase.StartCoroutine(Me.imageFade_cr(Me.icon, 0.6F * startAlpha, startAlpha, 0F, False))
			If startAlpha < 1F Then
				Yield New WaitForSeconds(0.6F * (1F - startAlpha))
			End If
		End If
		Return
	End Function

	' Token: 0x06004450 RID: 17488 RVA: 0x002417CC File Offset: 0x0023FBCC
	Private Iterator Function imageFade_cr(image As Image, time As Single, start As Single, [end] As Single, Optional interruptOnLoad As Boolean = False) As IEnumerator
		Dim t As Single = 0F
		Me.SetImageAlpha(image, start)
		While t < time AndAlso (Not interruptOnLoad OrElse Not Me.doneLoadingSceneAsync)
			Dim val As Single = Mathf.Lerp(start, [end], t / time)
			Me.SetImageAlpha(image, val)
			t += Time.deltaTime
			If SceneLoader.OnFaderValue IsNot Nothing Then
				SceneLoader.OnFaderValue(t / time)
			End If
			If interruptOnLoad Then
				SceneLoader.EndTransitionDelay = val * 0.6F
			End If
			Yield Nothing
		End While
		Me.SetImageAlpha(image, [end])
		If interruptOnLoad AndAlso Not Me.doneLoadingSceneAsync Then
			SceneLoader.EndTransitionDelay = 0.6F
		End If
		Return
	End Function

	' Token: 0x06004451 RID: 17489 RVA: 0x0024180C File Offset: 0x0023FC0C
	Private Iterator Function fadeBGM_cr(time As Single) As IEnumerator
		If AudioNoiseHandler.Instance IsNot Nothing Then
			AudioNoiseHandler.Instance.OpticalSound()
		End If
		Me.bgmVolumeStart = AudioManager.bgmOptionsVolume
		Me.bgmVolume = AudioManager.bgmOptionsVolume
		Me.sfxVolumeStart = AudioManager.sfxOptionsVolume
		Dim t As Single = 0F
		While t < time
			Dim val As Single = t / time
			AudioManager.bgmOptionsVolume = Mathf.Lerp(Me.bgmVolume, -80F, val)
			t += Time.deltaTime
			Yield Nothing
		End While
		AudioManager.bgmOptionsVolume = -80F
		AudioManager.StopBGM()
		Return
	End Function

	' Token: 0x06004452 RID: 17490 RVA: 0x0024182E File Offset: 0x0023FC2E
	Private Sub FadeOutBGM(time As Single)
		If Me.bgmCoroutine IsNot Nothing Then
			MyBase.StopCoroutine(Me.bgmCoroutine)
		End If
		Me.bgmCoroutine = MyBase.StartCoroutine(Me.fadeBGM_cr(time))
	End Sub

	' Token: 0x06004453 RID: 17491 RVA: 0x0024185A File Offset: 0x0023FC5A
	Public Sub ResetBgmVolume()
		If Me.bgmCoroutine IsNot Nothing Then
			MyBase.StopCoroutine(Me.bgmCoroutine)
		End If
		AudioManager.bgmOptionsVolume = Me.bgmVolumeStart
		AudioManager.sfxOptionsVolume = Me.sfxVolumeStart
	End Sub

	' Token: 0x040049C6 RID: 18886
	Private Const SCENE_LOADER_PATH As String = "UI/Scene_Loader"

	' Token: 0x040049C7 RID: 18887
	Private Const ICON_IN_TIME As Single = 0.4F

	' Token: 0x040049C8 RID: 18888
	Private Const ICON_OUT_TIME As Single = 0.6F

	' Token: 0x040049C9 RID: 18889
	Private Const ICON_WAIT_TIME As Single = 1F

	' Token: 0x040049CA RID: 18890
	Private Const ICON_NONE_TIME As Single = 1F

	' Token: 0x040049CB RID: 18891
	Private Const FADER_DELAY As Single = 0.5F

	' Token: 0x040049CC RID: 18892
	Private Const IRIS_TIME As Single = 0.6F

	' Token: 0x040049CD RID: 18893
	Private LOAD_SCENE_NAME As String = Scenes.scene_load_helper.ToString()

	' Token: 0x040049CE RID: 18894
	Public Shared EndTransitionDelay As Single

	' Token: 0x040049CF RID: 18895
	Public Shared IsInIrisTransition As Boolean

	' Token: 0x040049D0 RID: 18896
	Public Shared IsInBlurTransition As Boolean

	' Token: 0x040049D1 RID: 18897
	Private Shared _instance As SceneLoader

	' Token: 0x040049D3 RID: 18899
	Private Shared previousSceneName As String

	' Token: 0x040049D7 RID: 18903
	Private Shared currentlyLoading As Boolean

	' Token: 0x040049DE RID: 18910
	<SerializeField()>
	Private canvas As Canvas

	' Token: 0x040049DF RID: 18911
	<SerializeField()>
	Private fader As Image

	' Token: 0x040049E0 RID: 18912
	<SerializeField()>
	Private icon As Image

	' Token: 0x040049E1 RID: 18913
	<SerializeField()>
	Private camera As SceneLoaderCamera

	' Token: 0x040049E2 RID: 18914
	Private doneLoadingSceneAsync As Boolean

	' Token: 0x040049E3 RID: 18915
	Private bgmVolume As Single

	' Token: 0x040049E4 RID: 18916
	Private bgmLevelVolume As Single

	' Token: 0x040049E5 RID: 18917
	Private bgmVolumeStart As Single

	' Token: 0x040049E6 RID: 18918
	Private bgmLevelVolumeStart As Single

	' Token: 0x040049E7 RID: 18919
	Private sfxVolumeStart As Single

	' Token: 0x040049E8 RID: 18920
	Private bgmCoroutine As Coroutine

	' Token: 0x02000AFD RID: 2813
	Public MustInherit Class Context
	End Class

	' Token: 0x02000AFE RID: 2814
	' (Invoke) Token: 0x06004456 RID: 17494
	Public Delegate Sub FadeHandler(time As Single)

	' Token: 0x02000AFF RID: 2815
	Public Enum Transition
		' Token: 0x040049EA RID: 18922
		Iris
		' Token: 0x040049EB RID: 18923
		Fade
		' Token: 0x040049EC RID: 18924
		Blur
		' Token: 0x040049ED RID: 18925
		None
	End Enum

	' Token: 0x02000B00 RID: 2816
	Public Enum Icon
		' Token: 0x040049EF RID: 18927
		None
		' Token: 0x040049F0 RID: 18928
		Random
		' Token: 0x040049F1 RID: 18929
		Cuphead_Head
		' Token: 0x040049F2 RID: 18930
		Cuphead_Running
		' Token: 0x040049F3 RID: 18931
		Cuphead_Jumping
		' Token: 0x040049F4 RID: 18932
		Screen_OneMoment
		' Token: 0x040049F5 RID: 18933
		Hourglass
		' Token: 0x040049F6 RID: 18934
		HourglassBroken
	End Enum

	' Token: 0x02000B01 RID: 2817
	Public Class Properties
		' Token: 0x06004459 RID: 17497 RVA: 0x00241889 File Offset: 0x0023FC89
		Public Sub New()
			Me.Reset()
		End Sub

		' Token: 0x0600445A RID: 17498 RVA: 0x00241897 File Offset: 0x0023FC97
		Public Sub Reset()
			Me.icon = SceneLoader.Icon.Hourglass
			Me.transitionStart = SceneLoader.Transition.Fade
			Me.transitionEnd = SceneLoader.Transition.Fade
			Me.transitionStartTime = 0.4F
			Me.transitionEndTime = 0.4F
		End Sub

		' Token: 0x040049F7 RID: 18935
		Public Const FADE_START_DEFAULT As Single = 0.4F

		' Token: 0x040049F8 RID: 18936
		Public Const FADE_END_DEFAULT As Single = 0.4F

		' Token: 0x040049F9 RID: 18937
		Public icon As SceneLoader.Icon

		' Token: 0x040049FA RID: 18938
		Public transitionStart As SceneLoader.Transition

		' Token: 0x040049FB RID: 18939
		Public transitionEnd As SceneLoader.Transition

		' Token: 0x040049FC RID: 18940
		Public transitionStartTime As Single

		' Token: 0x040049FD RID: 18941
		Public transitionEndTime As Single
	End Class
End Class
