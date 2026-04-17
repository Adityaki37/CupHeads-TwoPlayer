Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x020003A4 RID: 932
Public MustInherit Class AssetLoader(Of T As Class)
	Inherits MonoBehaviour

	' Token: 0x06000B70 RID: 2928 RVA: 0x000836FF File Offset: 0x00081AFF
	Private Sub Awake()
		If AssetLoader(Of T).Instance IsNot Nothing Then
			Throw New Exception("More than one instance found")
		End If
		AssetLoader(Of T).Instance = Me
	End Sub

	' Token: 0x06000B71 RID: 2929 RVA: 0x00083722 File Offset: 0x00081B22
	Private Sub Start()
		MyBase.StartCoroutine(Me.loadPersistentAssets())
	End Sub

	' Token: 0x06000B72 RID: 2930 RVA: 0x00083734 File Offset: 0x00081B34
	Private Iterator Function loadPersistentAssets() As IEnumerator
		If Me.sceneAssetDatabase IsNot Nothing Then
			For Each assetName As String In Me.sceneAssetDatabase.persistentAssets
				Yield Me.loadAssetFromAssetBundle(assetName, AssetLoaderOption.PersistInCache(), Nothing)
			Next
		End If
		AssetLoader(Of T).persistentAssetsLoaded = True
		Return
	End Function

	' Token: 0x06000B73 RID: 2931 RVA: 0x00083750 File Offset: 0x00081B50
	Public Shared Function GetPreloadAssetNames(sceneName As String) As String()
		Dim array As String()
		If Not AssetLoader(Of T).Instance.sceneAssetDatabase.sceneAssetMappings.TryGetValue(sceneName, array) Then
			Return New String(-1) {}
		End If
		Return array
	End Function

	' Token: 0x06000B74 RID: 2932 RVA: 0x00083781 File Offset: 0x00081B81
	Public Shared Function LoadAsset(assetName As String, [option] As AssetLoaderOption) As Coroutine
		Return AssetLoader(Of T).Instance.loadAssetFromAssetBundle(assetName, [option], Nothing)
	End Function

	' Token: 0x06000B75 RID: 2933 RVA: 0x00083790 File Offset: 0x00081B90
	Public Shared Function LoadAssetSynchronous(assetName As String, [option] As AssetLoaderOption) As T
		Return AssetLoader(Of T).Instance.loadAssetFromAssetBundleSynchronous(assetName, [option])
	End Function

	' Token: 0x06000B76 RID: 2934
	Protected MustOverride Function loadAsset(assetName As String, completionHandler As Action(Of T)) As Coroutine

	' Token: 0x06000B77 RID: 2935
	Protected MustOverride Function loadAssetSynchronous(assetName As String) As T

	' Token: 0x06000B78 RID: 2936 RVA: 0x0008379E File Offset: 0x00081B9E
	Public Shared Function LoadPersistentAssetsDLC() As Coroutine
		Return AssetLoader(Of T).Instance.loadPersistentAssetsDLC()
	End Function

	' Token: 0x06000B79 RID: 2937 RVA: 0x000837AA File Offset: 0x00081BAA
	Private Function loadPersistentAssetsDLC() As Coroutine
		Return MyBase.StartCoroutine(Me.loadPersistentAssetsDLC_cr())
	End Function

	' Token: 0x06000B7A RID: 2938 RVA: 0x000837B8 File Offset: 0x00081BB8
	Private Iterator Function loadPersistentAssetsDLC_cr() As IEnumerator
		For Each assetName As String In Me.sceneAssetDatabase.persistentAssetsDLC
			Yield Me.loadAssetFromAssetBundle(assetName, AssetLoaderOption.PersistInCache(), Nothing)
		Next
		Return
	End Function

	' Token: 0x06000B7B RID: 2939 RVA: 0x000837D4 File Offset: 0x00081BD4
	Public Shared Function GetCachedAsset(assetName As String) As T
		Dim t As T
		If AssetLoader(Of T).Instance.tryGetAsset(assetName, t) Then
			Return t
		End If
		Throw New Exception("Asset not cached: " + assetName)
	End Function

	' Token: 0x06000B7C RID: 2940 RVA: 0x00083808 File Offset: 0x00081C08
	Public Shared Sub UnloadAssets(ParamArray persistentTagsToUnload As String())
		Dim list As List(Of String) = New List(Of String)(AssetLoader(Of T).Instance.loadedAssets.Keys)
		For i As Integer = list.Count - 1 To 0 Step -1
			Dim assetContainer As AssetLoader(Of T).AssetContainer(Of T) = AssetLoader(Of T).Instance.loadedAssets(list(i))
			If(assetContainer.assetOption.type And AssetLoaderOption.Type.PersistInCache) <> AssetLoaderOption.Type.None Then
				list.RemoveAt(i)
			ElseIf(assetContainer.assetOption.type And AssetLoaderOption.Type.PersistInCacheTagged) <> AssetLoaderOption.Type.None AndAlso Array.IndexOf(Of String)(persistentTagsToUnload, CStr(assetContainer.assetOption.context)) < 0 Then
				list.RemoveAt(i)
			ElseIf(assetContainer.assetOption.type And AssetLoaderOption.Type.DontDestroyOnUnload) = AssetLoaderOption.Type.None Then
				AssetLoader(Of T).Instance.destroyAsset(assetContainer.asset)
			End If
		Next
		For Each text As String In list
			AssetLoader(Of T).Instance.loadedAssets.Remove(text)
		Next
	End Sub

	' Token: 0x06000B7D RID: 2941
	Protected MustOverride Sub destroyAsset(asset As T)

	' Token: 0x06000B7E RID: 2942 RVA: 0x00083930 File Offset: 0x00081D30
	Private Sub cacheAsset(assetName As String, container As AssetLoader(Of T).AssetContainer(Of T))
		Me.loadedAssets.Add(assetName, container)
	End Sub

	' Token: 0x06000B7F RID: 2943 RVA: 0x00083940 File Offset: 0x00081D40
	Protected Function tryGetAsset(assetName As String, <System.Runtime.InteropServices.OutAttribute()> ByRef asset As T) As Boolean
		asset = CType(CObj(Nothing), T)
		Dim assetContainer As AssetLoader(Of T).AssetContainer(Of T)
		If Me.loadedAssets.TryGetValue(assetName, assetContainer) Then
			asset = assetContainer.asset
			Return True
		End If
		Return False
	End Function

	' Token: 0x06000B80 RID: 2944 RVA: 0x0008397C File Offset: 0x00081D7C
	Protected Function loadAssetFromAssetBundle(assetName As String, [option] As AssetLoaderOption, completionAction As Action(Of T)) As Coroutine
		Dim t As T
		If Me.tryGetAsset(assetName, t) Then
			If completionAction IsNot Nothing Then
				completionAction(t)
			End If
			Return Nothing
		End If
		If Not DLCManager.DLCEnabled() AndAlso Me.assetLocationDatabase.dlcAssets.Contains(assetName) Then
			If completionAction IsNot Nothing Then
				completionAction(CType(CObj(Nothing), T))
			End If
			Return Nothing
		End If
		Dim loadOperation As AssetLoader(Of T).LoadOperation
		If Not Me.loadOperations.TryGetValue(assetName, loadOperation) Then
			loadOperation = New AssetLoader(Of T).LoadOperation()
			Me.loadOperations.Add(assetName, loadOperation)
			loadOperation.coroutine = Me.loadAsset(assetName, Sub(asset As T)
				Me.cacheAsset(assetName, New AssetLoader(Of T).AssetContainer(Of T)(asset, [option]))
				Dim loadOperation2 As AssetLoader(Of T).LoadOperation = Me.loadOperations(assetName)
				For Each action As Action(Of T) In loadOperation2.completionHandlers
					If action IsNot Nothing Then
						action(asset)
					End If
				Next
				Me.loadOperations.Remove(assetName)
			End Sub)
		End If
		loadOperation.completionHandlers.Add(completionAction)
		Return loadOperation.coroutine
	End Function

	' Token: 0x06000B81 RID: 2945 RVA: 0x00083A60 File Offset: 0x00081E60
	Protected Function loadAssetFromAssetBundleSynchronous(assetName As String, [option] As AssetLoaderOption) As T
		Dim t As T
		If Me.tryGetAsset(assetName, t) Then
			Return t
		End If
		Dim t2 As T = Me.loadAssetSynchronous(assetName)
		Me.cacheAsset(assetName, New AssetLoader(Of T).AssetContainer(Of T)(t2, [option]))
		Return t2
	End Function

	' Token: 0x06000B82 RID: 2946 RVA: 0x00083A94 File Offset: 0x00081E94
	Public Shared Function IsDLCAsset(assetName As String) As Boolean
		Return AssetLoader(Of T).Instance.assetLocationDatabase.dlcAssets.Contains(assetName)
	End Function

	' Token: 0x06000B83 RID: 2947 RVA: 0x00083AAC File Offset: 0x00081EAC
	Public Shared Function DEBUG_GetLoadedAssets() As List(Of String)
		Dim list As List(Of String) = New List(Of String)(AssetLoader(Of T).Instance.loadedAssets.Count)
		For Each keyValuePair As KeyValuePair(Of String, AssetLoader(Of T).AssetContainer(Of T)) In AssetLoader(Of T).Instance.loadedAssets
			Dim value As AssetLoader(Of T).AssetContainer(Of T) = keyValuePair.Value
			Dim text As String = String.Format("{0} ({1})", keyValuePair.Key, value.assetOption.type.ToString())
			If(value.assetOption.type And AssetLoaderOption.Type.PersistInCacheTagged) <> AssetLoaderOption.Type.None Then
				text += String.Format(" [Tag={0}]", value.assetOption.context)
			End If
			list.Add(text)
		Next
		Return list
	End Function

	' Token: 0x17000208 RID: 520
	' (get) Token: 0x06000B84 RID: 2948 RVA: 0x00083B88 File Offset: 0x00081F88
	' (set) Token: 0x06000B85 RID: 2949 RVA: 0x00083B94 File Offset: 0x00081F94
	Public Shared Property persistentAssetsLoaded As Boolean
		Get
			Return AssetLoader(Of T).Instance._persistentAssetsLoaded
		End Get
		Private Set(value As Boolean)
			AssetLoader(Of T).Instance._persistentAssetsLoaded = value
		End Set
	End Property

	' Token: 0x0400150F RID: 5391
	Protected Shared Instance As AssetLoader(Of T)

	' Token: 0x04001510 RID: 5392
	<SerializeField()>
	Private sceneAssetDatabase As RuntimeSceneAssetDatabase

	' Token: 0x04001511 RID: 5393
	<SerializeField()>
	Private assetLocationDatabase As AssetLocationDatabase

	' Token: 0x04001512 RID: 5394
	Private loadOperations As Dictionary(Of String, AssetLoader(Of T).LoadOperation) = New Dictionary(Of String, AssetLoader(Of T).LoadOperation)()

	' Token: 0x04001513 RID: 5395
	Private loadedAssets As Dictionary(Of String, AssetLoader(Of T).AssetContainer(Of T)) = New Dictionary(Of String, AssetLoader(Of T).AssetContainer(Of T))()

	' Token: 0x04001514 RID: 5396
	Private _persistentAssetsLoaded As Boolean

	' Token: 0x020003A5 RID: 933
	Private Class AssetContainer(Of U)
		' Token: 0x06000B86 RID: 2950 RVA: 0x00083BA1 File Offset: 0x00081FA1
		Public Sub New(asset As U, assetOption As AssetLoaderOption)
			Me.asset = asset
			Me.assetOption = assetOption
		End Sub

		' Token: 0x04001515 RID: 5397
		Public asset As U

		' Token: 0x04001516 RID: 5398
		Public assetOption As AssetLoaderOption
	End Class

	' Token: 0x020003A6 RID: 934
	Private Class LoadOperation
		' Token: 0x04001517 RID: 5399
		Public coroutine As Coroutine

		' Token: 0x04001518 RID: 5400
		Public completionHandlers As List(Of Action(Of T)) = New List(Of Action(Of T))()
	End Class
End Class
