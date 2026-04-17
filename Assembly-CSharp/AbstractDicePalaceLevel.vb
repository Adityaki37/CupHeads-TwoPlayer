Imports System

' Token: 0x0200059D RID: 1437
Public MustInherit Class AbstractDicePalaceLevel
	Inherits Level

	' Token: 0x1700035A RID: 858
	' (get) Token: 0x06001B89 RID: 7049
	Public MustOverride ReadOnly Property CurrentDicePalaceLevel As DicePalaceLevels

	' Token: 0x06001B8A RID: 7050 RVA: 0x0005E564 File Offset: 0x0005C964
	Protected Overrides Sub Awake()
		MyBase.Awake()
		If DicePalaceMainLevelGameInfo.GameInfo IsNot Nothing Then
			AddHandler Level.Current.OnLoseEvent, AddressOf DicePalaceMainLevelGameInfo.GameInfo.CleanUp
		End If
		AddHandler MyBase.OnLoseEvent, AddressOf Me.ResetScore
	End Sub

	' Token: 0x06001B8B RID: 7051 RVA: 0x0005E5B3 File Offset: 0x0005C9B3
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		RemoveHandler MyBase.OnLoseEvent, AddressOf Me.ResetScore
	End Sub

	' Token: 0x06001B8C RID: 7052 RVA: 0x0005E5CD File Offset: 0x0005C9CD
	Private Sub ResetScore()
		RemoveHandler MyBase.OnLoseEvent, AddressOf Me.ResetScore
		MyBase.CleanUpScore()
	End Sub

	' Token: 0x06001B8D RID: 7053 RVA: 0x0005E5E7 File Offset: 0x0005C9E7
	Protected Overrides Sub CheckIfInABossesHub()
		MyBase.CheckIfInABossesHub()
		If Not Level.IsTowerOfPower Then
			Level.IsDicePalace = True
		End If
	End Sub
End Class
