Imports System
Imports UnityEngine

' Token: 0x0200096C RID: 2412
Public Class MapDLC
	Inherits Map

	' Token: 0x06003837 RID: 14391 RVA: 0x00202E63 File Offset: 0x00201263
	Protected Overrides Sub SelectMusic()
		Me.currentMusic = -2
		Me.CheckMusic(False)
		Me.CheckIfBossesCompleted()
	End Sub

	' Token: 0x06003838 RID: 14392 RVA: 0x00202E7C File Offset: 0x0020127C
	Protected Overrides Sub CheckMusic(isRecheck As Boolean)
		Dim num As Integer = Me.currentMusic
		Dim playerLoadout As PlayerData.PlayerLoadouts.PlayerLoadout = PlayerData.Data.Loadouts.GetPlayerLoadout(PlayerId.PlayerOne)
		Dim playerLoadout2 As PlayerData.PlayerLoadouts.PlayerLoadout = PlayerData.Data.Loadouts.GetPlayerLoadout(PlayerId.PlayerTwo)
		If(playerLoadout.charm = Charm.charm_curse AndAlso CharmCurse.CalculateLevel(PlayerId.PlayerOne) > -1) OrElse (PlayerManager.Multiplayer AndAlso playerLoadout2.charm = Charm.charm_curse AndAlso CharmCurse.CalculateLevel(PlayerId.PlayerTwo) > -1) Then
			If(playerLoadout.charm = Charm.charm_curse AndAlso CharmCurse.IsMaxLevel(PlayerId.PlayerOne)) OrElse (PlayerManager.Multiplayer AndAlso playerLoadout2.charm = Charm.charm_curse AndAlso CharmCurse.IsMaxLevel(PlayerId.PlayerTwo)) Then
				num = If((Not PlayerData.Data.pianoAudioEnabled), 3, 5)
			Else
				num = If((Not PlayerData.Data.pianoAudioEnabled), 2, 4)
			End If
		ElseIf PlayerData.Data.pianoAudioEnabled Then
			num = 1
		Else
			num = If((Not MapDLC.haveVisited), (-1), 0)
			MapDLC.haveVisited = True
		End If
		If(Me.currentMusic = -1 AndAlso num = 0) OrElse (Me.currentMusic = 0 AndAlso num = -1) Then
			Return
		End If
		If num <> Me.currentMusic Then
			Me.currentMusic = num
			If Me.currentMusic = -1 Then
				AudioManager.PlayBGM()
			Else
				AudioManager.StartBGMAlternate(Me.currentMusic)
			End If
		End If
	End Sub

	' Token: 0x06003839 RID: 14393 RVA: 0x00202FF3 File Offset: 0x002013F3
	Protected Overrides Sub OnPlayerJoined(playerId As PlayerId)
		MyBase.OnPlayerJoined(playerId)
		Me.CheckMusic(True)
	End Sub

	' Token: 0x0600383A RID: 14394 RVA: 0x00203003 File Offset: 0x00201403
	Protected Overrides Sub OnPlayerLeave(playerId As PlayerId)
		MyBase.OnPlayerLeave(playerId)
		Me.CheckMusic(True)
	End Sub

	' Token: 0x0600383B RID: 14395 RVA: 0x00203013 File Offset: 0x00201413
	Private Sub CheckIfBossesCompleted()
		If PlayerData.Data.CheckLevelsHaveMinDifficulty(Level.worldDLCBossLevels, Level.Mode.Normal) Then
			Me.bakerySoundLoop.gameObject.SetActive(False)
		Else
			Me.bakerySoundLoop.Play()
		End If
	End Sub

	' Token: 0x04004014 RID: 16404
	Private Shared haveVisited As Boolean

	' Token: 0x04004015 RID: 16405
	<SerializeField()>
	Private bakerySoundLoop As AudioSource
End Class
