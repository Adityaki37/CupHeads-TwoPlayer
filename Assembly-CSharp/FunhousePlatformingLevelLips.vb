Imports System
Imports UnityEngine

' Token: 0x020008BA RID: 2234
Public Class FunhousePlatformingLevelLips
	Inherits BasicProjectile

	' Token: 0x06003420 RID: 13344 RVA: 0x001E443A File Offset: 0x001E283A
	Protected Overrides Sub Awake()
		MyBase.Awake()
		AddHandler PlatformingLevelExit.OnWinStartEvent, AddressOf Me.OnWin
	End Sub

	' Token: 0x06003421 RID: 13345 RVA: 0x001E4453 File Offset: 0x001E2853
	Private Sub Kiss()
		AudioManager.Play("funhouse_honkbullet_kiss")
		Me.emitAudioFromObject.Add("funhouse_honkbullet_kiss")
	End Sub

	' Token: 0x06003422 RID: 13346 RVA: 0x001E446F File Offset: 0x001E286F
	Protected Overrides Sub OnDestroy()
		RemoveHandler PlatformingLevelExit.OnWinStartEvent, AddressOf Me.OnWin
		MyBase.OnDestroy()
	End Sub

	' Token: 0x06003423 RID: 13347 RVA: 0x001E4488 File Offset: 0x001E2888
	Private Sub OnWin()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub
End Class
