Imports System
Imports System.Collections.Generic
Imports UnityEngine
Imports UnityEngine.U2D

' Token: 0x020003B2 RID: 946
Public Class SpriteAtlasLoader
	Inherits AssetLoader(Of SpriteAtlas)

	' Token: 0x06000BAC RID: 2988 RVA: 0x0008471E File Offset: 0x00082B1E
	Private Sub OnEnable()
		AddHandler SpriteAtlasManager.atlasRequested, AddressOf Me.atlasRequestedHandler
	End Sub

	' Token: 0x06000BAD RID: 2989 RVA: 0x00084731 File Offset: 0x00082B31
	Private Sub OnDisable()
		RemoveHandler SpriteAtlasManager.atlasRequested, AddressOf Me.atlasRequestedHandler
	End Sub

	' Token: 0x06000BAE RID: 2990 RVA: 0x00084744 File Offset: 0x00082B44
	Protected Overrides Function loadAsset(assetName As String, completionHandler As Action(Of SpriteAtlas)) As Coroutine
		Dim action As Action(Of SpriteAtlas) = Sub(atlas As SpriteAtlas)
			Me.resolveDeferredRequests(assetName, atlas)
			completionHandler(atlas)
		End Sub
		Return AssetBundleLoader.LoadSpriteAtlas(assetName, action)
	End Function

	' Token: 0x06000BAF RID: 2991 RVA: 0x00084785 File Offset: 0x00082B85
	Protected Overrides Function loadAssetSynchronous(assetName As String) As SpriteAtlas
		Throw New NotImplementedException()
	End Function

	' Token: 0x06000BB0 RID: 2992 RVA: 0x0008478C File Offset: 0x00082B8C
	Protected Overrides Sub destroyAsset(asset As SpriteAtlas)
		Global.UnityEngine.[Object].Destroy(asset)
	End Sub

	' Token: 0x06000BB1 RID: 2993 RVA: 0x00084794 File Offset: 0x00082B94
	Private Sub addDeferredRequest(assetName As String, action As Action(Of SpriteAtlas))
		Dim list As List(Of Action(Of SpriteAtlas))
		If Not Me.deferredAtlastRequests.TryGetValue(assetName, list) Then
			list = New List(Of Action(Of SpriteAtlas))()
			Me.deferredAtlastRequests.Add(assetName, list)
		End If
		list.Add(action)
	End Sub

	' Token: 0x06000BB2 RID: 2994 RVA: 0x000847D0 File Offset: 0x00082BD0
	Private Sub resolveDeferredRequests(assetName As String, atlas As SpriteAtlas)
		If atlas Is Nothing Then
			Return
		End If
		Dim list As List(Of Action(Of SpriteAtlas))
		If Me.deferredAtlastRequests.TryGetValue(assetName, list) Then
			For Each action As Action(Of SpriteAtlas) In list
				action(atlas)
			Next
			Me.deferredAtlastRequests.Remove(assetName)
		End If
	End Sub

	' Token: 0x06000BB3 RID: 2995 RVA: 0x00084854 File Offset: 0x00082C54
	Private Sub atlasRequestedHandler(atlasTag As String, action As Action(Of SpriteAtlas))
		Dim action2 As Action(Of SpriteAtlas) = Sub(atlas As SpriteAtlas)
			If atlas Is Nothing Then
				Me.addDeferredRequest(atlasTag, action)
			Else
				action(atlas)
			End If
		End Sub
		MyBase.loadAssetFromAssetBundle(atlasTag, AssetLoaderOption.None(), action2)
	End Sub

	' Token: 0x0400156A RID: 5482
	Private deferredAtlastRequests As Dictionary(Of String, List(Of Action(Of SpriteAtlas))) = New Dictionary(Of String, List(Of Action(Of SpriteAtlas)))()
End Class
