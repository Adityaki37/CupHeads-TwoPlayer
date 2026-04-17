Imports System
Imports System.Collections
Imports Steamworks
Imports UnityEngine
Imports UnityEngine.U2D

' Token: 0x020009B6 RID: 2486
Public Class DLCManager
	' Token: 0x06003A4A RID: 14922 RVA: 0x00211DBB File Offset: 0x002101BB
	Public Shared Sub RefreshDLC()
		DLCManager.refreshDLC()
	End Sub

	' Token: 0x06003A4B RID: 14923 RVA: 0x00211DC2 File Offset: 0x002101C2
	Public Shared Sub CheckInstallationStatusChanged()
		DLCManager.checkInstallationStatusChanged()
	End Sub

	' Token: 0x06003A4C RID: 14924 RVA: 0x00211DCC File Offset: 0x002101CC
	Public Shared Function DLCEnabled() As Boolean
		Dim flag As Boolean = False
		DLCManager.dlcEnabled(flag)
		Return flag
	End Function

	' Token: 0x06003A4D RID: 14925 RVA: 0x00211DE4 File Offset: 0x002101E4
	Public Shared Function AssetBundlePath() As String
		Dim text As String = Nothing
		DLCManager.assetBundlePath(text)
		Return text
	End Function

	' Token: 0x06003A4E RID: 14926 RVA: 0x00211DFC File Offset: 0x002101FC
	Public Shared Function UsesAlternateBundleLoadingMechanism() As Boolean
		Dim flag As Boolean = False
		DLCManager.usesAlternateBundleLoadingMechanism(flag)
		Return flag
	End Function

	' Token: 0x06003A4F RID: 14927 RVA: 0x00211E14 File Offset: 0x00210214
	Public Shared Function LoadAssetBundle(path As String) As DLCManager.AssetBundleLoadWaitInstruction
		Dim assetBundleLoadWaitInstruction As DLCManager.AssetBundleLoadWaitInstruction = Nothing
		DLCManager.loadAssetBundle(path, assetBundleLoadWaitInstruction)
		Return assetBundleLoadWaitInstruction
	End Function

	' Token: 0x06003A50 RID: 14928 RVA: 0x00211E2C File Offset: 0x0021022C
	Public Shared Function UnloadBundlesImmediately() As Boolean
		Dim flag As Boolean = False
		DLCManager.unloadBundlesImmediately(flag)
		Return flag
	End Function

	' Token: 0x06003A51 RID: 14929 RVA: 0x00211E44 File Offset: 0x00210244
	Public Shared Function CanRedirectToStore() As Boolean
		Dim flag As Boolean = False
		DLCManager.canRedirectToStore(flag)
		Return flag
	End Function

	' Token: 0x06003A52 RID: 14930 RVA: 0x00211E5B File Offset: 0x0021025B
	Public Shared Sub LaunchStore()
		DLCManager.launchStore()
	End Sub

	' Token: 0x06003A53 RID: 14931 RVA: 0x00211E62 File Offset: 0x00210262
	Public Shared Function LoadPersistentAssets() As Coroutine()
		If DLCManager.persistentAssetsLoaded OrElse Not DLCManager.DLCEnabled() Then
			Return Nothing
		End If
		DLCManager.persistentAssetsLoaded = True
		Return New Coroutine() { AssetLoader(Of SpriteAtlas).LoadPersistentAssetsDLC() }
	End Function

	' Token: 0x06003A54 RID: 14932 RVA: 0x00211E8E File Offset: 0x0021028E
	Public Shared Sub ResetAvailabilityPrompt()
		DLCManager.availabilityPromptTriggered = True
		DLCManager.showAvailabilityPrompt = False
	End Sub

	' Token: 0x170004BB RID: 1211
	' (get) Token: 0x06003A55 RID: 14933 RVA: 0x00211E9C File Offset: 0x0021029C
	' (set) Token: 0x06003A56 RID: 14934 RVA: 0x00211EA3 File Offset: 0x002102A3
	Public Shared Property persistentAssetsLoaded As Boolean

	' Token: 0x170004BC RID: 1212
	' (get) Token: 0x06003A57 RID: 14935 RVA: 0x00211EAB File Offset: 0x002102AB
	' (set) Token: 0x06003A58 RID: 14936 RVA: 0x00211EB2 File Offset: 0x002102B2
	Public Shared Property showAvailabilityPrompt As Boolean

	' Token: 0x06003A59 RID: 14937 RVA: 0x00211EBC File Offset: 0x002102BC
	Private Shared Function steamDLCStatus() As Boolean
		Dim num As ULong
		Dim num2 As ULong
		Return SteamApps.BIsDlcInstalled(DLCManager.DLCAppID) AndAlso Not SteamApps.GetDlcDownloadProgress(DLCManager.DLCAppID, num, num2)
	End Function

	' Token: 0x06003A5A RID: 14938 RVA: 0x00211EEB File Offset: 0x002102EB
	Private Shared Sub refreshDLC()
		If Not SteamManager.Initialized Then
			DLCManager.dlcAvailable = False
			Return
		End If
		If Not DLCManager.dlcAvailable Then
			DLCManager.dlcAvailable = DLCManager.steamDLCStatus()
		End If
	End Sub

	' Token: 0x06003A5B RID: 14939 RVA: 0x00211F12 File Offset: 0x00210312
	Private Shared Sub checkInstallationStatusChanged()
		If Not SteamManager.Initialized Then
			Return
		End If
		If DLCManager.DLCEnabled() OrElse DLCManager.availabilityPromptTriggered Then
			Return
		End If
		If DLCManager.steamDLCStatus() Then
			DLCManager.showAvailabilityPrompt = True
		End If
	End Sub

	' Token: 0x06003A5C RID: 14940 RVA: 0x00211F44 File Offset: 0x00210344
	Private Shared Sub dlcEnabled(ByRef enabled As Boolean)
		enabled = DLCManager.dlcAvailable
	End Sub

	' Token: 0x06003A5D RID: 14941 RVA: 0x00211F4D File Offset: 0x0021034D
	Private Shared Sub assetBundlePath(ByRef path As String)
		path = Application.streamingAssetsPath
	End Sub

	' Token: 0x06003A5E RID: 14942 RVA: 0x00211F56 File Offset: 0x00210356
	Private Shared Sub usesAlternateBundleLoadingMechanism(ByRef usesAlternate As Boolean)
		usesAlternate = False
	End Sub

	' Token: 0x06003A5F RID: 14943 RVA: 0x00211F5B File Offset: 0x0021035B
	Private Shared Sub unloadBundlesImmediately(ByRef unloadImmediately As Boolean)
		unloadImmediately = False
	End Sub

	' Token: 0x06003A60 RID: 14944 RVA: 0x00211F60 File Offset: 0x00210360
	Private Shared Sub loadAssetBundle(path As String, ByRef waitInstruction As DLCManager.AssetBundleLoadWaitInstruction)
		Throw New NotImplementedException()
	End Sub

	' Token: 0x06003A61 RID: 14945 RVA: 0x00211F67 File Offset: 0x00210367
	Private Shared Sub canRedirectToStore(ByRef canRedirect As Boolean)
		canRedirect = True
	End Sub

	' Token: 0x06003A62 RID: 14946 RVA: 0x00211F6C File Offset: 0x0021036C
	Private Shared Sub launchStore()
		If Not SteamManager.Initialized Then
			Return
		End If
		SteamFriends.ActivateGameOverlayToStore(DLCManager.DLCAppID, EOverlayToStoreFlag.k_EOverlayToStoreFlag_None)
	End Sub

	' Token: 0x04004281 RID: 17025
	Private Shared availabilityPromptTriggered As Boolean

	' Token: 0x04004284 RID: 17028
	Private Shared DLCAppID As AppId_t = New AppId_t(1117850UI)

	' Token: 0x04004285 RID: 17029
	Private Shared dlcAvailable As Boolean

	' Token: 0x020009B7 RID: 2487
	Public Class AssetBundleLoadWaitInstruction
		Implements IEnumerator

		' Token: 0x170004BD RID: 1213
		' (get) Token: 0x06003A65 RID: 14949 RVA: 0x00084A40 File Offset: 0x00082E40
		Public ReadOnly Property Current As Object Implements System.Collections.IEnumerator.Current
			Get
				Return Nothing
			End Get
		End Property

		' Token: 0x06003A66 RID: 14950 RVA: 0x00084A43 File Offset: 0x00082E43
		Public Function MoveNext() As Boolean Implements System.Collections.IEnumerator.MoveNext
			Return Not Me.complete
		End Function

		' Token: 0x06003A67 RID: 14951 RVA: 0x00084A4E File Offset: 0x00082E4E
		Public Sub Reset() Implements System.Collections.IEnumerator.Reset
		End Sub

		' Token: 0x170004BE RID: 1214
		' (get) Token: 0x06003A68 RID: 14952 RVA: 0x00084A50 File Offset: 0x00082E50
		' (set) Token: 0x06003A69 RID: 14953 RVA: 0x00084A58 File Offset: 0x00082E58
		Public Property assetBundle As AssetBundle

		' Token: 0x04004286 RID: 17030
		Protected complete As Boolean
	End Class
End Class
