Imports System
Imports UnityEngine

' Token: 0x020003AF RID: 943
Public Class MusicLoader
	Inherits AssetLoader(Of AudioClip)

	' Token: 0x06000BA3 RID: 2979 RVA: 0x0008463B File Offset: 0x00082A3B
	Protected Overrides Function loadAsset(assetName As String, completionHandler As Action(Of AudioClip)) As Coroutine
		Return AssetBundleLoader.LoadMusic(assetName, completionHandler)
	End Function

	' Token: 0x06000BA4 RID: 2980 RVA: 0x00084644 File Offset: 0x00082A44
	Protected Overrides Function loadAssetSynchronous(assetName As String) As AudioClip
		Throw New NotImplementedException()
	End Function

	' Token: 0x06000BA5 RID: 2981 RVA: 0x0008464B File Offset: 0x00082A4B
	Protected Overrides Sub destroyAsset(asset As AudioClip)
		Global.UnityEngine.[Object].Destroy(asset)
	End Sub
End Class
