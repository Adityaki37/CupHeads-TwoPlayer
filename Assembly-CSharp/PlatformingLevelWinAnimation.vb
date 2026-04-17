Imports System
Imports UnityEngine

' Token: 0x0200090C RID: 2316
Public Class PlatformingLevelWinAnimation
	Inherits AbstractLevelHUDComponent

	' Token: 0x17000466 RID: 1126
	' (get) Token: 0x06003654 RID: 13908 RVA: 0x001F7DB2 File Offset: 0x001F61B2
	' (set) Token: 0x06003655 RID: 13909 RVA: 0x001F7DBA File Offset: 0x001F61BA
	Public Property CurrentState As PlatformingLevelWinAnimation.State

	' Token: 0x06003656 RID: 13910 RVA: 0x001F7DC3 File Offset: 0x001F61C3
	Public Shared Function Create() As PlatformingLevelWinAnimation
		Return Global.UnityEngine.[Object].Instantiate(Of PlatformingLevelWinAnimation)(Level.Current.LevelResources.platformingWin)
	End Function

	' Token: 0x06003657 RID: 13911 RVA: 0x001F7DD9 File Offset: 0x001F61D9
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me._parentToHudCanvas = True
	End Sub

	' Token: 0x06003658 RID: 13912 RVA: 0x001F7DE8 File Offset: 0x001F61E8
	Private Sub OnAnimComplete()
		Me.CurrentState = PlatformingLevelWinAnimation.State.Complete
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x04003E44 RID: 15940
	Private Const FRAME_DELAY As Single = 5F

	' Token: 0x0200090D RID: 2317
	Public Enum State
		' Token: 0x04003E47 RID: 15943
		Paused
		' Token: 0x04003E48 RID: 15944
		Unpaused
		' Token: 0x04003E49 RID: 15945
		Complete
	End Enum
End Class
