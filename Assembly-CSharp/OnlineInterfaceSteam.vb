Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.IO
Imports Steamworks
Imports UnityEngine

' Token: 0x020009C6 RID: 2502
Public Class OnlineInterfaceSteam
	Implements OnlineInterface

	' Token: 0x170004CA RID: 1226
	' (get) Token: 0x06003AB1 RID: 15025 RVA: 0x0021204C File Offset: 0x0021044C
	Private ReadOnly Property SavePath As String
		Get
			If Application.platform = RuntimePlatform.WindowsEditor OrElse Application.platform = RuntimePlatform.WindowsPlayer Then
				Return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Cuphead\")
			End If
			If Application.platform = RuntimePlatform.OSXEditor OrElse Application.platform = RuntimePlatform.OSXPlayer Then
				Return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Library/Application Support/unity.Studio MDHR.Cuphead/Cuphead/")
			End If
			Return String.Empty
		End Get
	End Property

	' Token: 0x14000072 RID: 114
	' (add) Token: 0x06003AB2 RID: 15026 RVA: 0x002120AC File Offset: 0x002104AC
	' (remove) Token: 0x06003AB3 RID: 15027 RVA: 0x002120E4 File Offset: 0x002104E4
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnUserSignedIn As SignInEventHandler Implements OnlineInterface.OnUserSignedIn

	' Token: 0x14000073 RID: 115
	' (add) Token: 0x06003AB4 RID: 15028 RVA: 0x0021211C File Offset: 0x0021051C
	' (remove) Token: 0x06003AB5 RID: 15029 RVA: 0x00212154 File Offset: 0x00210554
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnUserSignedOut As SignOutEventHandler Implements OnlineInterface.OnUserSignedOut

	' Token: 0x170004CB RID: 1227
	' (get) Token: 0x06003AB6 RID: 15030 RVA: 0x0021218A File Offset: 0x0021058A
	Public ReadOnly Property MainUser As OnlineUser Implements OnlineInterface.MainUser
		Get
			Return Nothing
		End Get
	End Property

	' Token: 0x170004CC RID: 1228
	' (get) Token: 0x06003AB7 RID: 15031 RVA: 0x0021218D File Offset: 0x0021058D
	Public ReadOnly Property SecondaryUser As OnlineUser Implements OnlineInterface.SecondaryUser
		Get
			Return Nothing
		End Get
	End Property

	' Token: 0x170004CD RID: 1229
	' (get) Token: 0x06003AB8 RID: 15032 RVA: 0x00212190 File Offset: 0x00210590
	Public ReadOnly Property CloudStorageInitialized As Boolean Implements OnlineInterface.CloudStorageInitialized
		Get
			Return True
		End Get
	End Property

	' Token: 0x170004CE RID: 1230
	' (get) Token: 0x06003AB9 RID: 15033 RVA: 0x00212193 File Offset: 0x00210593
	Public ReadOnly Property SupportsMultipleUsers As Boolean Implements OnlineInterface.SupportsMultipleUsers
		Get
			Return False
		End Get
	End Property

	' Token: 0x170004CF RID: 1231
	' (get) Token: 0x06003ABA RID: 15034 RVA: 0x00212196 File Offset: 0x00210596
	Public ReadOnly Property SupportsUserSignIn As Boolean Implements OnlineInterface.SupportsUserSignIn
		Get
			Return False
		End Get
	End Property

	' Token: 0x06003ABB RID: 15035 RVA: 0x0021219C File Offset: 0x0021059C
	Public Sub Init() Implements OnlineInterface.Init
		Me.steamManager = New GameObject("SteamManager").AddComponent(Of SteamManager)()
		Me.steamManager.transform.SetParent(Cuphead.Current.transform)
		If Not SteamManager.Initialized Then
			Return
		End If
		SteamUserStats.RequestCurrentStats()
	End Sub

	' Token: 0x06003ABC RID: 15036 RVA: 0x002121E9 File Offset: 0x002105E9
	Public Sub Reset() Implements OnlineInterface.Reset
	End Sub

	' Token: 0x06003ABD RID: 15037 RVA: 0x002121EB File Offset: 0x002105EB
	Public Sub SignInUser(silent As Boolean, player As PlayerId, controllerId As ULong) Implements OnlineInterface.SignInUser
		Me.OnUserSignedIn(Nothing)
	End Sub

	' Token: 0x06003ABE RID: 15038 RVA: 0x002121F9 File Offset: 0x002105F9
	Public Sub SwitchUser(player As PlayerId, controllerId As ULong) Implements OnlineInterface.SwitchUser
	End Sub

	' Token: 0x06003ABF RID: 15039 RVA: 0x002121FB File Offset: 0x002105FB
	Public Function GetUserForController(id As ULong) As OnlineUser Implements OnlineInterface.GetUserForController
		Return Nothing
	End Function

	' Token: 0x06003AC0 RID: 15040 RVA: 0x002121FE File Offset: 0x002105FE
	Public Function GetControllersForUser(player As PlayerId) As List(Of ULong) Implements OnlineInterface.GetControllersForUser
		Return Nothing
	End Function

	' Token: 0x06003AC1 RID: 15041 RVA: 0x00212201 File Offset: 0x00210601
	Public Function IsUserSignedIn(player As PlayerId) As Boolean Implements OnlineInterface.IsUserSignedIn
		Return False
	End Function

	' Token: 0x06003AC2 RID: 15042 RVA: 0x00212204 File Offset: 0x00210604
	Public Function GetUser(player As PlayerId) As OnlineUser Implements OnlineInterface.GetUser
		Return Nothing
	End Function

	' Token: 0x06003AC3 RID: 15043 RVA: 0x00212207 File Offset: 0x00210607
	Public Sub SetUser(player As PlayerId, user As OnlineUser) Implements OnlineInterface.SetUser
	End Sub

	' Token: 0x06003AC4 RID: 15044 RVA: 0x00212209 File Offset: 0x00210609
	Public Function GetProfilePic(player As PlayerId) As Texture2D Implements OnlineInterface.GetProfilePic
		Return Nothing
	End Function

	' Token: 0x06003AC5 RID: 15045 RVA: 0x0021220C File Offset: 0x0021060C
	Public Sub GetAchievement(player As PlayerId, id As String, achievementRetrievedHandler As AchievementEventHandler) Implements OnlineInterface.GetAchievement
	End Sub

	' Token: 0x06003AC6 RID: 15046 RVA: 0x00212210 File Offset: 0x00210610
	Public Sub UnlockAchievement(player As PlayerId, id As String) Implements OnlineInterface.UnlockAchievement
		If Not SteamManager.Initialized Then
			Return
		End If
		Dim flag As Boolean
		SteamUserStats.GetAchievement(id, flag)
		If Not flag Then
			SteamUserStats.SetAchievement(id)
			SteamUserStats.StoreStats()
		End If
	End Sub

	' Token: 0x06003AC7 RID: 15047 RVA: 0x00212244 File Offset: 0x00210644
	Public Sub SyncAchievementsAndStats() Implements OnlineInterface.SyncAchievementsAndStats
		If Not SteamManager.Initialized Then
			Return
		End If
		SteamUserStats.StoreStats()
	End Sub

	' Token: 0x06003AC8 RID: 15048 RVA: 0x00212257 File Offset: 0x00210657
	Public Sub SetStat(player As PlayerId, id As String, value As Integer) Implements OnlineInterface.SetStat
		If Not SteamManager.Initialized Then
			Return
		End If
		SteamUserStats.SetStat(id, value)
	End Sub

	' Token: 0x06003AC9 RID: 15049 RVA: 0x0021226C File Offset: 0x0021066C
	Public Sub SetStat(player As PlayerId, id As String, value As Single) Implements OnlineInterface.SetStat
		If Not SteamManager.Initialized Then
			Return
		End If
		SteamUserStats.SetStat(id, value)
	End Sub

	' Token: 0x06003ACA RID: 15050 RVA: 0x00212281 File Offset: 0x00210681
	Public Sub SetStat(player As PlayerId, id As String, value As String) Implements OnlineInterface.SetStat
	End Sub

	' Token: 0x06003ACB RID: 15051 RVA: 0x00212284 File Offset: 0x00210684
	Public Sub IncrementStat(player As PlayerId, id As String, value As Integer) Implements OnlineInterface.IncrementStat
		If Not SteamManager.Initialized Then
			Return
		End If
		Dim num As Integer
		SteamUserStats.GetStat(id, num)
		Dim num2 As Integer = num + value
		SteamUserStats.SetStat(id, num2)
		If id = "Parries" AndAlso (num2 = 20 OrElse num2 = 100) Then
			SteamUserStats.StoreStats()
		End If
	End Sub

	' Token: 0x06003ACC RID: 15052 RVA: 0x002122D7 File Offset: 0x002106D7
	Public Sub SetRichPresence(player As PlayerId, id As String, active As Boolean) Implements OnlineInterface.SetRichPresence
	End Sub

	' Token: 0x06003ACD RID: 15053 RVA: 0x002122D9 File Offset: 0x002106D9
	Public Sub SetRichPresenceActive(player As PlayerId, active As Boolean) Implements OnlineInterface.SetRichPresenceActive
	End Sub

	' Token: 0x06003ACE RID: 15054 RVA: 0x002122DB File Offset: 0x002106DB
	Public Sub InitializeCloudStorage(player As PlayerId, handler As InitializeCloudStoreHandler) Implements OnlineInterface.InitializeCloudStorage
		handler(True)
	End Sub

	' Token: 0x06003ACF RID: 15055 RVA: 0x002122E4 File Offset: 0x002106E4
	Public Sub UninitializeCloudStorage() Implements OnlineInterface.UninitializeCloudStorage
	End Sub

	' Token: 0x06003AD0 RID: 15056 RVA: 0x002122E8 File Offset: 0x002106E8
	Public Sub SaveCloudData(data As IDictionary(Of String, String), handler As SaveCloudDataHandler) Implements OnlineInterface.SaveCloudData
		Dim savePath As String = Me.SavePath
		If Not Directory.Exists(savePath) Then
			Directory.CreateDirectory(savePath)
		End If
		For Each text As String In data.Keys
			Try
				Dim textWriter As TextWriter = New StreamWriter(Path.Combine(savePath, text + ".sav"))
				textWriter.Write(data(text))
				textWriter.Close()
			Catch
				Cuphead.Current.StartCoroutine(Me.saveFailed_cr(handler))
				Return
			End Try
		Next
		handler(True)
	End Sub

	' Token: 0x06003AD1 RID: 15057 RVA: 0x002123B0 File Offset: 0x002107B0
	Private Iterator Function saveFailed_cr(handler As SaveCloudDataHandler) As IEnumerator
		Yield New WaitForSeconds(0.25F)
		handler(False)
		Return
	End Function

	' Token: 0x06003AD2 RID: 15058 RVA: 0x002123CC File Offset: 0x002107CC
	Public Sub LoadCloudData(keys As String(), handler As LoadCloudDataHandler) Implements OnlineInterface.LoadCloudData
		Dim array As String() = New String(keys.Length - 1) {}
		Dim savePath As String = Me.SavePath
		For i As Integer = 0 To array.Length - 1
			Dim text As String = Path.Combine(savePath, keys(i) + ".sav")
			If File.Exists(text) Then
				Try
					Dim textReader As TextReader = New StreamReader(Path.Combine(savePath, keys(i) + ".sav"))
					array(i) = textReader.ReadToEnd()
					textReader.Close()
				Catch
					handler(array, CloudLoadResult.Failed)
				End Try
			Else
				handler(array, CloudLoadResult.NoData)
			End If
		Next
		handler(array, CloudLoadResult.Success)
	End Sub

	' Token: 0x06003AD3 RID: 15059 RVA: 0x00212480 File Offset: 0x00210880
	Public Sub UpdateControllerMapping() Implements OnlineInterface.UpdateControllerMapping
	End Sub

	' Token: 0x06003AD4 RID: 15060 RVA: 0x00212482 File Offset: 0x00210882
	Public Function ControllerMappingChanged() As Boolean Implements OnlineInterface.ControllerMappingChanged
		Return False
	End Function

	' Token: 0x0400429C RID: 17052
	Private steamManager As SteamManager
End Class
