Imports System
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x020009C5 RID: 2501
Public Interface OnlineInterface
	' Token: 0x14000070 RID: 112
	' (add) Token: 0x06003A8E RID: 14990
	' (remove) Token: 0x06003A8F RID: 14991
	Event OnUserSignedIn As SignInEventHandler

	' Token: 0x14000071 RID: 113
	' (add) Token: 0x06003A90 RID: 14992
	' (remove) Token: 0x06003A91 RID: 14993
	Event OnUserSignedOut As SignOutEventHandler

	' Token: 0x170004C5 RID: 1221
	' (get) Token: 0x06003A92 RID: 14994
	ReadOnly Property MainUser As OnlineUser

	' Token: 0x170004C6 RID: 1222
	' (get) Token: 0x06003A93 RID: 14995
	ReadOnly Property SecondaryUser As OnlineUser

	' Token: 0x170004C7 RID: 1223
	' (get) Token: 0x06003A94 RID: 14996
	ReadOnly Property CloudStorageInitialized As Boolean

	' Token: 0x170004C8 RID: 1224
	' (get) Token: 0x06003A95 RID: 14997
	ReadOnly Property SupportsMultipleUsers As Boolean

	' Token: 0x170004C9 RID: 1225
	' (get) Token: 0x06003A96 RID: 14998
	ReadOnly Property SupportsUserSignIn As Boolean

	' Token: 0x06003A97 RID: 14999
	Sub Init()

	' Token: 0x06003A98 RID: 15000
	Sub Reset()

	' Token: 0x06003A99 RID: 15001
	Sub SignInUser(silent As Boolean, player As PlayerId, controllerId As ULong)

	' Token: 0x06003A9A RID: 15002
	Sub SwitchUser(player As PlayerId, controllerId As ULong)

	' Token: 0x06003A9B RID: 15003
	Function GetUserForController(id As ULong) As OnlineUser

	' Token: 0x06003A9C RID: 15004
	Function GetControllersForUser(player As PlayerId) As List(Of ULong)

	' Token: 0x06003A9D RID: 15005
	Function IsUserSignedIn(player As PlayerId) As Boolean

	' Token: 0x06003A9E RID: 15006
	Function GetUser(player As PlayerId) As OnlineUser

	' Token: 0x06003A9F RID: 15007
	Sub SetUser(player As PlayerId, user As OnlineUser)

	' Token: 0x06003AA0 RID: 15008
	Function GetProfilePic(player As PlayerId) As Texture2D

	' Token: 0x06003AA1 RID: 15009
	Sub GetAchievement(player As PlayerId, id As String, achievementRetrievedHandler As AchievementEventHandler)

	' Token: 0x06003AA2 RID: 15010
	Sub UnlockAchievement(player As PlayerId, id As String)

	' Token: 0x06003AA3 RID: 15011
	Sub SyncAchievementsAndStats()

	' Token: 0x06003AA4 RID: 15012
	Sub SetStat(player As PlayerId, id As String, value As Integer)

	' Token: 0x06003AA5 RID: 15013
	Sub SetStat(player As PlayerId, id As String, value As Single)

	' Token: 0x06003AA6 RID: 15014
	Sub SetStat(player As PlayerId, id As String, value As String)

	' Token: 0x06003AA7 RID: 15015
	Sub IncrementStat(player As PlayerId, id As String, value As Integer)

	' Token: 0x06003AA8 RID: 15016
	Sub SetRichPresence(player As PlayerId, id As String, active As Boolean)

	' Token: 0x06003AA9 RID: 15017
	Sub SetRichPresenceActive(player As PlayerId, active As Boolean)

	' Token: 0x06003AAA RID: 15018
	Sub InitializeCloudStorage(player As PlayerId, handler As InitializeCloudStoreHandler)

	' Token: 0x06003AAB RID: 15019
	Sub UninitializeCloudStorage()

	' Token: 0x06003AAC RID: 15020
	Sub SaveCloudData(data As IDictionary(Of String, String), handler As SaveCloudDataHandler)

	' Token: 0x06003AAD RID: 15021
	Sub LoadCloudData(keys As String(), handler As LoadCloudDataHandler)

	' Token: 0x06003AAE RID: 15022
	Sub UpdateControllerMapping()

	' Token: 0x06003AAF RID: 15023
	Function ControllerMappingChanged() As Boolean
End Interface
