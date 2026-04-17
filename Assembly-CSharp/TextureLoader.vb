Imports System
Imports UnityEngine

' Token: 0x020003B3 RID: 947
Public Class TextureLoader
	Inherits AssetLoader(Of Texture2D())

	' Token: 0x06000BB5 RID: 2997 RVA: 0x0008490A File Offset: 0x00082D0A
	Protected Overrides Function loadAsset(assetName As String, completionHandler As Action(Of Texture2D())) As Coroutine
		Return AssetBundleLoader.LoadTextures(assetName, completionHandler)
	End Function

	' Token: 0x06000BB6 RID: 2998 RVA: 0x00084913 File Offset: 0x00082D13
	Protected Overrides Function loadAssetSynchronous(assetName As String) As Texture2D()
		Return AssetBundleLoader.LoadTexturesSynchronous(assetName)
	End Function

	' Token: 0x06000BB7 RID: 2999 RVA: 0x0008491C File Offset: 0x00082D1C
	Protected Overrides Sub destroyAsset(asset As Texture2D())
		For i As Integer = 0 To asset.Length - 1
			Global.UnityEngine.[Object].Destroy(asset(i))
		Next
	End Sub
End Class
