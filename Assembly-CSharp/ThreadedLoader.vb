Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.IO
Imports System.Threading
Imports UnityEngine

' Token: 0x020003B4 RID: 948
Public Class ThreadedLoader
	' Token: 0x06000BB8 RID: 3000 RVA: 0x00084945 File Offset: 0x00082D45
	Public Sub New(coroutineParent As MonoBehaviour)
		Me.coroutineParent = coroutineParent
	End Sub

	' Token: 0x06000BB9 RID: 3001 RVA: 0x00084960 File Offset: 0x00082D60
	Public Function LoadAssetBundle(path As String) As ThreadedLoader.LoadOperation
		Dim loadOperation As ThreadedLoader.LoadOperation = New ThreadedLoader.LoadOperation()
		loadOperation.path = path
		If Me.busy Then
			Me.operationQueue.Add(loadOperation)
			Return loadOperation
		End If
		Me.startLoad(loadOperation)
		Return loadOperation
	End Function

	' Token: 0x06000BBA RID: 3002 RVA: 0x0008499C File Offset: 0x00082D9C
	Private Sub startLoad(operation As ThreadedLoader.LoadOperation)
		Me.busy = True
		Me.threadBusy = True
		Me.coroutineParent.StartCoroutine(Me.threadWait_cr(operation))
		Dim thread As Thread = New Thread(Sub()
			Me.loadData(operation)
		End Sub)
		thread.Start()
	End Sub

	' Token: 0x06000BBB RID: 3003 RVA: 0x000849FC File Offset: 0x00082DFC
	Private Iterator Function threadWait_cr(operation As ThreadedLoader.LoadOperation) As IEnumerator
		While Me.threadBusy
			Yield Nothing
		End While
		Dim request As AssetBundleCreateRequest = AssetBundle.LoadFromMemoryAsync(operation.data)
		Yield request
		operation.SetComplete(request.assetBundle)
		Me.busy = False
		If Me.operationQueue.Count > 0 Then
			Dim loadOperation As ThreadedLoader.LoadOperation = Me.operationQueue(0)
			Me.operationQueue.RemoveAt(0)
			Me.startLoad(loadOperation)
		End If
		Return
	End Function

	' Token: 0x06000BBC RID: 3004 RVA: 0x00084A1E File Offset: 0x00082E1E
	Private Sub loadData(operation As ThreadedLoader.LoadOperation)
		operation.data = File.ReadAllBytes(operation.path)
		Me.threadBusy = False
	End Sub

	' Token: 0x0400156B RID: 5483
	Private coroutineParent As MonoBehaviour

	' Token: 0x0400156C RID: 5484
	Private operationQueue As List(Of ThreadedLoader.LoadOperation) = New List(Of ThreadedLoader.LoadOperation)()

	' Token: 0x0400156D RID: 5485
	Private busy As Boolean

	' Token: 0x0400156E RID: 5486
	Private threadBusy As Boolean

	' Token: 0x020003B5 RID: 949
	Public Class LoadOperation
		Inherits DLCManager.AssetBundleLoadWaitInstruction

		' Token: 0x06000BBE RID: 3006 RVA: 0x00084A69 File Offset: 0x00082E69
		Public Sub SetComplete(bundle As AssetBundle)
			Me.complete = True
			MyBase.assetBundle = bundle
		End Sub

		' Token: 0x0400156F RID: 5487
		Public path As String

		' Token: 0x04001570 RID: 5488
		Public data As Byte()
	End Class
End Class
