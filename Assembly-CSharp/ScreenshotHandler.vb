Imports System
Imports UnityEngine

' Token: 0x02000420 RID: 1056
Public Class ScreenshotHandler
	Inherits MonoBehaviour

	' Token: 0x06000F52 RID: 3922 RVA: 0x0009725A File Offset: 0x0009565A
	Private Sub Awake()
		ScreenshotHandler.instance = Me
		Global.UnityEngine.[Object].DontDestroyOnLoad(MyBase.gameObject)
		Global.UnityEngine.[Object].Destroy(Me)
	End Sub

	' Token: 0x06000F53 RID: 3923 RVA: 0x00097273 File Offset: 0x00095673
	Public Shared Sub TakeScreenshot_Static(_camera As ScreenshotHandler.cameraType, _folderName As String, _fileName As String)
	End Sub

	' Token: 0x04001855 RID: 6229
	Private Shared instance As ScreenshotHandler

	' Token: 0x04001856 RID: 6230
	Private takeScreenshotNextFrame As Boolean

	' Token: 0x04001857 RID: 6231
	Private myCamera As Camera

	' Token: 0x04001858 RID: 6232
	Private fileName As String

	' Token: 0x04001859 RID: 6233
	Private folderName As String

	' Token: 0x0400185A RID: 6234
	Private currentCameraType As ScreenshotHandler.cameraType

	' Token: 0x02000421 RID: 1057
	Public Enum cameraType
		' Token: 0x0400185C RID: 6236
		Map
		' Token: 0x0400185D RID: 6237
		UI
		' Token: 0x0400185E RID: 6238
		Level
	End Enum
End Class
