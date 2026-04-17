Imports System
Imports System.Collections
Imports UnityEngine
Imports UnityEngine.SceneManagement
Imports UnityEngine.U2D

' Token: 0x020003EE RID: 1006
Public Class CupheadStartScene
	Inherits AbstractMonoBehaviour

	' Token: 0x06000D9B RID: 3483 RVA: 0x0008E649 File Offset: 0x0008CA49
	Protected Overrides Sub Awake()
		Application.targetFrameRate = 60
		Cuphead.Init(True)
	End Sub

	' Token: 0x06000D9C RID: 3484 RVA: 0x0008E658 File Offset: 0x0008CA58
	Private Sub Start()
		MyBase.StartCoroutine(Me.start_cr())
	End Sub

	' Token: 0x06000D9D RID: 3485 RVA: 0x0008E668 File Offset: 0x0008CA68
	Private Iterator Function start_cr() As IEnumerator
		Yield Nothing
		Yield Nothing
		AssetLoader(Of Texture2D()).LoadAssetSynchronous("screen_fx", AssetLoaderOption.DontDestroyOnUnload())
		Global.UnityEngine.[Object].FindObjectOfType(Of ChromaticAberrationFilmGrain)().Initialize(AssetLoader(Of Texture2D()).GetCachedAsset("screen_fx"))
		If PlatformHelper.ForceAdditionalHeapMemory Then
			HeapAllocator.Allocate(100)
			Yield Nothing
			Yield Nothing
		End If
		If PlatformHelper.PreloadSettingsData Then
			OnlineManager.Instance.Init()
			SettingsData.LoadFromCloud(AddressOf Me.OnSettingsDataLoaded)
			While Not Me.settingsDataLoaded
				Yield Nothing
			End While
		End If
		Dim startScreenLoadData As StartScreen.InitialLoadData = New StartScreen.InitialLoadData()
		Dim titleScreenOverride As PlatformHandlingTitleScreenOverride = New PlatformHandlingTitleScreenOverride(startScreenLoadData)
		Yield MyBase.StartCoroutine(titleScreenOverride.GetTitleScreenOverrideStatus_cr(Me))
		StartScreen.initialLoadData = startScreenLoadData
		titleScreenOverride = Nothing
		Dim fontCoroutines As Coroutine() = FontLoader.Initialize()
		For Each coroutine As Coroutine In fontCoroutines
			Yield coroutine
		Next
		While AssetBundleLoader.loadCounter > 0 OrElse Not AssetLoader(Of SpriteAtlas).persistentAssetsLoaded OrElse Not AssetLoader(Of AudioClip).persistentAssetsLoaded OrElse Not AssetLoader(Of Texture2D()).persistentAssetsLoaded
			Yield Nothing
		End While
		Yield Nothing
		Cuphead.Init(False)
		Yield New WaitForSeconds(0.1F)
		DLCManager.RefreshDLC()
		Yield Nothing
		Yield Nothing
		Dim coroutines As Coroutine() = DLCManager.LoadPersistentAssets()
		If coroutines IsNot Nothing Then
			For Each coroutine2 As Coroutine In coroutines
				Yield coroutine2
			Next
			Yield Nothing
			Yield Nothing
		End If
		Dim titleSceneName As String = "scene_title"
		Dim preloadAtlases As String() = AssetLoader(Of SpriteAtlas).GetPreloadAssetNames(titleSceneName)
		For Each atlas As String In preloadAtlases
			Yield AssetLoader(Of SpriteAtlas).LoadAsset(atlas, AssetLoaderOption.None())
		Next
		Dim preloadMusic As String() = AssetLoader(Of AudioClip).GetPreloadAssetNames(titleSceneName)
		For Each clip As String In preloadMusic
			Yield AssetLoader(Of AudioClip).LoadAsset(clip, AssetLoaderOption.None())
		Next
		Yield Nothing
		Yield Nothing
		SceneManager.LoadSceneAsync(1)
		Return
	End Function

	' Token: 0x06000D9E RID: 3486 RVA: 0x0008E683 File Offset: 0x0008CA83
	Private Sub OnSettingsDataLoaded(success As Boolean)
		If Not success Then
			SettingsData.LoadFromCloud(AddressOf Me.OnSettingsDataLoaded)
			Return
		End If
		Me.settingsDataLoaded = True
	End Sub

	' Token: 0x04001713 RID: 5907
	Private settingsDataLoaded As Boolean
End Class
