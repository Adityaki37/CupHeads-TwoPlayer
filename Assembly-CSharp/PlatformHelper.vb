Imports System

' Token: 0x02000379 RID: 889
Public Module PlatformHelper
	' Token: 0x170001F4 RID: 500
	' (get) Token: 0x06000A53 RID: 2643 RVA: 0x0007E702 File Offset: 0x0007CB02
	Public ReadOnly Property IsConsole As Boolean
		Get
			Return False
		End Get
	End Property

	' Token: 0x170001F5 RID: 501
	' (get) Token: 0x06000A54 RID: 2644 RVA: 0x0007E705 File Offset: 0x0007CB05
	Public ReadOnly Property PreloadSettingsData As Boolean
		Get
			Return False
		End Get
	End Property

	' Token: 0x170001F6 RID: 502
	' (get) Token: 0x06000A55 RID: 2645 RVA: 0x0007E708 File Offset: 0x0007CB08
	Public ReadOnly Property ShowAchievements As Boolean
		Get
			Return False
		End Get
	End Property

	' Token: 0x170001F7 RID: 503
	' (get) Token: 0x06000A56 RID: 2646 RVA: 0x0007E70B File Offset: 0x0007CB0B
	Public ReadOnly Property ShowDLCMenuItem As Boolean
		Get
			Return True
		End Get
	End Property

	' Token: 0x170001F8 RID: 504
	' (get) Token: 0x06000A57 RID: 2647 RVA: 0x0007E70E File Offset: 0x0007CB0E
	Public ReadOnly Property GarbageCollectOnPause As Boolean
		Get
			Return False
		End Get
	End Property

	' Token: 0x170001F9 RID: 505
	' (get) Token: 0x06000A58 RID: 2648 RVA: 0x0007E711 File Offset: 0x0007CB11
	Public ReadOnly Property ForceAdditionalHeapMemory As Boolean
		Get
			Return False
		End Get
	End Property

	' Token: 0x170001FA RID: 506
	' (get) Token: 0x06000A59 RID: 2649 RVA: 0x0007E714 File Offset: 0x0007CB14
	Public ReadOnly Property ManuallyRefreshDLCAvailability As Boolean
		Get
			Return True
		End Get
	End Property

	' Token: 0x170001FB RID: 507
	' (get) Token: 0x06000A5A RID: 2650 RVA: 0x0007E717 File Offset: 0x0007CB17
	Public ReadOnly Property CanSwitchUserFromPause As Boolean
		Get
			Return False
		End Get
	End Property
End Module
