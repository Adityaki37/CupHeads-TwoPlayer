Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.IO
Imports UnityEngine
Imports UnityEngine.U2D

' Token: 0x020003A1 RID: 929
Public Class AssetBundleLoader
	Inherits MonoBehaviour

	' Token: 0x06000B5A RID: 2906 RVA: 0x00082E32 File Offset: 0x00081232
	Private Sub Awake()
		If AssetBundleLoader.Instance IsNot Nothing Then
			Throw New Exception("Should only be one instance")
		End If
		AssetBundleLoader.Instance = Me
	End Sub

	' Token: 0x06000B5B RID: 2907 RVA: 0x00082E58 File Offset: 0x00081258
	Public Shared Sub UnloadAssetBundles()
		For Each keyValuePair As KeyValuePair(Of String, AssetBundleLoader.AssetBundleContainer) In AssetBundleLoader.Instance.loadedBundles
			keyValuePair.Value.assetBundle.Unload(False)
		Next
		AssetBundleLoader.Instance.loadedBundles.Clear()
	End Sub

	' Token: 0x06000B5C RID: 2908 RVA: 0x00082ED4 File Offset: 0x000812D4
	Public Shared Function LoadSpriteAtlas(atlasName As String, completionHandler As Action(Of SpriteAtlas)) As Coroutine
		Dim assetBundleLocation As AssetBundleLoader.AssetBundleLocation = AssetBundleLoader.AssetBundleLocation.StreamingAssets
		If AssetBundleLoader.Instance.atlasLocationDatabase.dlcAssets.Contains(atlasName) Then
			assetBundleLocation = AssetBundleLoader.AssetBundleLocation.DLC
		End If
		Dim spriteAtlasBundleName As String = AssetBundleLoader.GetSpriteAtlasBundleName(atlasName)
		Return AssetBundleLoader.Instance.StartCoroutine(AssetBundleLoader.Instance.loadAsset(Of SpriteAtlas)(spriteAtlasBundleName, assetBundleLocation, atlasName, completionHandler))
	End Function

	' Token: 0x06000B5D RID: 2909 RVA: 0x00082F20 File Offset: 0x00081320
	Public Shared Function LoadMusic(audioClipName As String, completionHandler As Action(Of AudioClip)) As Coroutine
		Dim assetBundleLocation As AssetBundleLoader.AssetBundleLocation = AssetBundleLoader.AssetBundleLocation.StreamingAssets
		If AssetBundleLoader.Instance.musicLocationDatabase.dlcAssets.Contains(audioClipName) Then
			assetBundleLocation = AssetBundleLoader.AssetBundleLocation.DLC
		End If
		Dim musicBundleName As String = AssetBundleLoader.GetMusicBundleName(audioClipName)
		Return AssetBundleLoader.Instance.StartCoroutine(AssetBundleLoader.Instance.loadAsset(Of AudioClip)(musicBundleName, assetBundleLocation, audioClipName, completionHandler))
	End Function

	' Token: 0x06000B5E RID: 2910 RVA: 0x00082F6C File Offset: 0x0008136C
	Public Shared Function LoadFont(bundleName As String, assetName As String, completionHandler As Action(Of Font)) As Coroutine
		Dim assetBundleLocation As AssetBundleLoader.AssetBundleLocation = AssetBundleLoader.AssetBundleLocation.StreamingAssets
		bundleName = AssetBundleLoader.AssetBundlePrefixFont + bundleName.ToLowerInvariant()
		Return AssetBundleLoader.Instance.StartCoroutine(AssetBundleLoader.Instance.loadAsset(Of Font)(bundleName, assetBundleLocation, assetName, completionHandler))
	End Function

	' Token: 0x06000B5F RID: 2911 RVA: 0x00082FA8 File Offset: 0x000813A8
	Public Shared Function LoadTMPFont(bundleName As String, completionHandler As Action(Of Global.UnityEngine.[Object]())) As Coroutine
		Dim assetBundleLocation As AssetBundleLoader.AssetBundleLocation = AssetBundleLoader.AssetBundleLocation.StreamingAssets
		bundleName = AssetBundleLoader.AssetBundlePrefixTMPFont + bundleName.ToLowerInvariant()
		Return AssetBundleLoader.Instance.StartCoroutine(AssetBundleLoader.Instance.loadAllAssets(Of Global.UnityEngine.[Object])(bundleName, assetBundleLocation, completionHandler))
	End Function

	' Token: 0x06000B60 RID: 2912 RVA: 0x00082FE0 File Offset: 0x000813E0
	Public Shared Function LoadTextures(bundleName As String, completionHandler As Action(Of Texture2D())) As Coroutine
		Dim assetBundleLocation As AssetBundleLoader.AssetBundleLocation = AssetBundleLoader.AssetBundleLocation.StreamingAssets
		bundleName = AssetBundleLoader.AssetBundlePrefixTexture + bundleName.ToLowerInvariant()
		Return AssetBundleLoader.Instance.StartCoroutine(AssetBundleLoader.Instance.loadAllAssets(Of Texture2D)(bundleName, assetBundleLocation, completionHandler))
	End Function

	' Token: 0x06000B61 RID: 2913 RVA: 0x00083018 File Offset: 0x00081418
	Public Shared Function LoadTexturesSynchronous(bundleName As String) As Texture2D()
		Dim assetBundleLocation As AssetBundleLoader.AssetBundleLocation = AssetBundleLoader.AssetBundleLocation.StreamingAssets
		bundleName = AssetBundleLoader.AssetBundlePrefixTexture + bundleName.ToLowerInvariant()
		Return AssetBundleLoader.Instance.loadAllAssetsSynchronous(Of Texture2D)(bundleName, assetBundleLocation)
	End Function

	' Token: 0x06000B62 RID: 2914 RVA: 0x00083048 File Offset: 0x00081448
	Private Iterator Function loadAssetBundle(assetBundleName As String, location As AssetBundleLoader.AssetBundleLocation) As IEnumerator
		AssetBundleLoader.loadCounter += 1
		Dim path As String = AssetBundleLoader.getBasePath(location)
		path = Path.Combine(path, "AssetBundles")
		path = Path.Combine(path, assetBundleName)
		Dim assetBundle As AssetBundle
		If location = AssetBundleLoader.AssetBundleLocation.DLC AndAlso DLCManager.UsesAlternateBundleLoadingMechanism() Then
			Dim waitInstruction As DLCManager.AssetBundleLoadWaitInstruction = DLCManager.LoadAssetBundle(path)
			Yield waitInstruction
			assetBundle = waitInstruction.assetBundle
		Else
			Dim request As AssetBundleCreateRequest = AssetBundle.LoadFromFileAsync(path)
			Yield request
			assetBundle = request.assetBundle
		End If
		Me.loadedBundles.Add(assetBundleName, New AssetBundleLoader.AssetBundleContainer(assetBundle, location))
		AssetBundleLoader.loadCounter -= 1
		Return
	End Function

	' Token: 0x06000B63 RID: 2915 RVA: 0x00083074 File Offset: 0x00081474
	Private Function loadAssetBundleSynchronous(assetBundleName As String, location As AssetBundleLoader.AssetBundleLocation) As AssetBundleLoader.AssetBundleContainer
		Dim text As String = AssetBundleLoader.getBasePath(location)
		text = Path.Combine(text, "AssetBundles")
		text = Path.Combine(text, assetBundleName)
		Dim assetBundle As AssetBundle = AssetBundle.LoadFromFile(text)
		Dim assetBundleContainer As AssetBundleLoader.AssetBundleContainer = New AssetBundleLoader.AssetBundleContainer(assetBundle, location)
		Me.loadedBundles.Add(assetBundleName, assetBundleContainer)
		Return assetBundleContainer
	End Function

	' Token: 0x06000B64 RID: 2916 RVA: 0x000830BC File Offset: 0x000814BC
	Private Iterator Function loadAsset(Of T As Global.UnityEngine.[Object])(assetBundleName As String, location As AssetBundleLoader.AssetBundleLocation, assetName As String, completionHandler As Action(Of T)) As IEnumerator
		AssetBundleLoader.loadCounter += 1
		Dim assetBundleContainer As AssetBundleLoader.AssetBundleContainer
		If Not Me.loadedBundles.TryGetValue(assetBundleName, assetBundleContainer) Then
			Yield MyBase.StartCoroutine(Me.loadAssetBundle(assetBundleName, location))
			assetBundleContainer = Me.loadedBundles(assetBundleName)
		End If
		Dim assetRequest As AssetBundleRequest = assetBundleContainer.assetBundle.LoadAssetAsync(Of T)(assetName)
		Yield assetRequest
		completionHandler(TryCast(assetRequest.asset, T))
		If assetBundleContainer.location = AssetBundleLoader.AssetBundleLocation.DLC AndAlso DLCManager.UnloadBundlesImmediately() AndAlso GetType(T) = GetType(SpriteAtlas) Then
			Me.loadedBundles.Remove(assetBundleContainer.assetBundle.name)
			assetBundleContainer.assetBundle.Unload(False)
		End If
		AssetBundleLoader.loadCounter -= 1
		Return
	End Function

	' Token: 0x06000B65 RID: 2917 RVA: 0x000830F4 File Offset: 0x000814F4
	Private Iterator Function loadAllAssets(Of T As Global.UnityEngine.[Object])(assetBundleName As String, location As AssetBundleLoader.AssetBundleLocation, completionHandler As Action(Of T())) As IEnumerator
		AssetBundleLoader.loadCounter += 1
		Dim assetBundleContainer As AssetBundleLoader.AssetBundleContainer
		If Not Me.loadedBundles.TryGetValue(assetBundleName, assetBundleContainer) Then
			Yield MyBase.StartCoroutine(Me.loadAssetBundle(assetBundleName, location))
			assetBundleContainer = Me.loadedBundles(assetBundleName)
		End If
		Dim assetRequest As AssetBundleRequest = assetBundleContainer.assetBundle.LoadAllAssetsAsync(Of T)()
		Yield assetRequest
		Dim allAssets As Global.UnityEngine.[Object]() = assetRequest.allAssets
		Dim castAssets As T() = New T(allAssets.Length - 1) {}
		For i As Integer = 0 To allAssets.Length - 1
			castAssets(i) = CType(CObj(allAssets(i)), T)
		Next
		completionHandler(castAssets)
		AssetBundleLoader.loadCounter -= 1
		Return
	End Function

	' Token: 0x06000B66 RID: 2918 RVA: 0x00083124 File Offset: 0x00081524
	Private Function loadAllAssetsSynchronous(Of T As Global.UnityEngine.[Object])(assetBundleName As String, location As AssetBundleLoader.AssetBundleLocation) As T()
		Dim assetBundleContainer As AssetBundleLoader.AssetBundleContainer
		If Not Me.loadedBundles.TryGetValue(assetBundleName, assetBundleContainer) Then
			assetBundleContainer = Me.loadAssetBundleSynchronous(assetBundleName, location)
		End If
		Return assetBundleContainer.assetBundle.LoadAllAssets(Of T)()
	End Function

	' Token: 0x06000B67 RID: 2919 RVA: 0x00083158 File Offset: 0x00081558
	Private Shared Function getBasePath(location As AssetBundleLoader.AssetBundleLocation) As String
		If location = AssetBundleLoader.AssetBundleLocation.DLC Then
			Return DLCManager.AssetBundlePath()
		End If
		Return Application.streamingAssetsPath
	End Function

	' Token: 0x06000B68 RID: 2920 RVA: 0x0008316C File Offset: 0x0008156C
	Public Shared Function GetSpriteAtlasBundleName(atlasName As String) As String
		Return AssetBundleLoader.AssetBundlePrefixSpriteAtlas + atlasName.ToLowerInvariant()
	End Function

	' Token: 0x06000B69 RID: 2921 RVA: 0x0008317E File Offset: 0x0008157E
	Public Shared Function GetMusicBundleName(audioClipName As String) As String
		Return AssetBundleLoader.AssetBundlePrefixMusic + audioClipName.ToLowerInvariant()
	End Function

	' Token: 0x06000B6A RID: 2922 RVA: 0x00083190 File Offset: 0x00081590
	Public Shared Function DEBUG_LoadedAssetBundles() As List(Of String)
		Return New List(Of String)(AssetBundleLoader.Instance.loadedBundles.Keys)
	End Function

	' Token: 0x17000207 RID: 519
	' (get) Token: 0x06000B6B RID: 2923 RVA: 0x000831A6 File Offset: 0x000815A6
	' (set) Token: 0x06000B6C RID: 2924 RVA: 0x000831AD File Offset: 0x000815AD
	Public Shared Property loadCounter As Integer

	' Token: 0x04001500 RID: 5376
	Public Shared AssetBundlePrefixSpriteAtlas As String = "atlas_"

	' Token: 0x04001501 RID: 5377
	Public Shared AssetBundlePrefixMusic As String = "music_"

	' Token: 0x04001502 RID: 5378
	Public Shared AssetBundlePrefixFont As String = "font_"

	' Token: 0x04001503 RID: 5379
	Public Shared AssetBundlePrefixTMPFont As String = "tmpfont_"

	' Token: 0x04001504 RID: 5380
	Public Shared AssetBundlePrefixTexture As String = "tex_"

	' Token: 0x04001505 RID: 5381
	Private Shared Instance As AssetBundleLoader

	' Token: 0x04001506 RID: 5382
	<SerializeField()>
	Private atlasLocationDatabase As AssetLocationDatabase

	' Token: 0x04001507 RID: 5383
	<SerializeField()>
	Private musicLocationDatabase As AssetLocationDatabase

	' Token: 0x04001508 RID: 5384
	Private loadedBundles As Dictionary(Of String, AssetBundleLoader.AssetBundleContainer) = New Dictionary(Of String, AssetBundleLoader.AssetBundleContainer)()

	' Token: 0x020003A2 RID: 930
	Private Enum AssetBundleLocation
		' Token: 0x0400150B RID: 5387
		StreamingAssets
		' Token: 0x0400150C RID: 5388
		DLC
	End Enum

	' Token: 0x020003A3 RID: 931
	Private Class AssetBundleContainer
		' Token: 0x06000B6E RID: 2926 RVA: 0x000831E9 File Offset: 0x000815E9
		Public Sub New(assetBundle As AssetBundle, location As AssetBundleLoader.AssetBundleLocation)
			Me.assetBundle = assetBundle
			Me.location = location
		End Sub

		' Token: 0x0400150D RID: 5389
		Public assetBundle As AssetBundle

		' Token: 0x0400150E RID: 5390
		Public location As AssetBundleLoader.AssetBundleLocation
	End Class
End Class
