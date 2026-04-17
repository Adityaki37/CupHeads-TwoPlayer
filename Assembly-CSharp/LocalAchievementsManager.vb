Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports UnityEngine

' Token: 0x0200039A RID: 922
Public Module LocalAchievementsManager
	' Token: 0x1400000C RID: 12
	' (add) Token: 0x06000B39 RID: 2873 RVA: 0x000825C8 File Offset: 0x000809C8
	' (remove) Token: 0x06000B3A RID: 2874 RVA: 0x000825FC File Offset: 0x000809FC
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event AchievementUnlockedEvent As Action(Of LocalAchievementsManager.Achievement)

	' Token: 0x06000B3B RID: 2875 RVA: 0x00082630 File Offset: 0x00080A30
	Public Sub Initialize()
		If LocalAchievementsManager.initialized Then
			Return
		End If
		LocalAchievementsManager.initialized = True
		LocalAchievementsManager.loadFromCloud()
	End Sub

	' Token: 0x06000B3C RID: 2876 RVA: 0x00082648 File Offset: 0x00080A48
	Public Sub UnlockAchievement(playerId As PlayerId, achievementName As String)
		Dim achievement As LocalAchievementsManager.Achievement = CType([Enum].Parse(GetType(LocalAchievementsManager.Achievement), achievementName), LocalAchievementsManager.Achievement)
		If LocalAchievementsManager.IsAchievementUnlocked(achievement) Then
			Return
		End If
		LocalAchievementsManager.achievementData.unlockedAchievements.Add(achievement)
		LocalAchievementsManager.saveToCloud()
		If LocalAchievementsManager.AchievementUnlockedEvent IsNot Nothing Then
			LocalAchievementsManager.AchievementUnlockedEvent(achievement)
		End If
	End Sub

	' Token: 0x06000B3D RID: 2877 RVA: 0x000826A4 File Offset: 0x00080AA4
	Public Sub IncrementStat(player As PlayerId, id As String, value As Integer)
		If id = "Parries" Then
			If LocalAchievementsManager.achievementData.parryCount >= 100 Then
				Return
			End If
			LocalAchievementsManager.achievementData.parryCount += value
			Dim flag As Boolean = True
			If LocalAchievementsManager.achievementData.parryCount >= 20 Then
				LocalAchievementsManager.UnlockAchievement(PlayerId.Any, "ParryApprentice")
				flag = False
			End If
			If LocalAchievementsManager.achievementData.parryCount >= 100 Then
				LocalAchievementsManager.UnlockAchievement(PlayerId.Any, "ParryMaster")
				flag = False
			End If
			If flag Then
				LocalAchievementsManager.saveToCloud()
			End If
		End If
	End Sub

	' Token: 0x06000B3E RID: 2878 RVA: 0x00082736 File Offset: 0x00080B36
	Public Function GetUnlockedAchievements() As IList(Of LocalAchievementsManager.Achievement)
		Return LocalAchievementsManager.achievementData.unlockedAchievements
	End Function

	' Token: 0x06000B3F RID: 2879 RVA: 0x00082742 File Offset: 0x00080B42
	Public Function IsAchievementUnlocked(achievement As LocalAchievementsManager.Achievement) As Boolean
		Return LocalAchievementsManager.achievementData.unlockedAchievements.Contains(achievement)
	End Function

	' Token: 0x06000B40 RID: 2880 RVA: 0x00082754 File Offset: 0x00080B54
	Public Function IsHiddenAchievement(achievement As LocalAchievementsManager.Achievement) As Boolean
		Return achievement = LocalAchievementsManager.Achievement.FoundSecretPassage OrElse achievement = LocalAchievementsManager.Achievement.SmallPlaneOnlyWin OrElse achievement = LocalAchievementsManager.Achievement.FoundAllMoney OrElse achievement = LocalAchievementsManager.Achievement.PacifistRun OrElse achievement = LocalAchievementsManager.Achievement.NoHitsTakenDicePalace OrElse achievement = LocalAchievementsManager.Achievement.BadEnding OrElse achievement = LocalAchievementsManager.Achievement.CompleteDevil OrElse achievement = LocalAchievementsManager.Achievement.DefeatDevilPhase2 OrElse achievement = LocalAchievementsManager.Achievement.Paladin
	End Function

	' Token: 0x06000B41 RID: 2881 RVA: 0x000827AC File Offset: 0x00080BAC
	Private Sub saveToCloud()
		If OnlineManager.Instance.[Interface].CloudStorageInitialized Then
			Dim text As String = JsonUtility.ToJson(LocalAchievementsManager.achievementData)
			Dim dictionary As Dictionary(Of String, String) = New Dictionary(Of String, String)()
			dictionary(LocalAchievementsManager.CloudKey) = text
			OnlineManager.Instance.[Interface].SaveCloudData(dictionary, AddressOf LocalAchievementsManager.onSavedCloudData)
		End If
	End Sub

	' Token: 0x06000B42 RID: 2882 RVA: 0x00082817 File Offset: 0x00080C17
	Private Sub onSavedCloudData(success As Boolean)
	End Sub

	' Token: 0x06000B43 RID: 2883 RVA: 0x0008281C File Offset: 0x00080C1C
	Private Sub loadFromCloud()
		If OnlineManager.Instance.[Interface].CloudStorageInitialized Then
			OnlineManager.Instance.[Interface].LoadCloudData(New String() { LocalAchievementsManager.CloudKey }, AddressOf LocalAchievementsManager.onLoadedCloudData)
		End If
	End Sub

	' Token: 0x06000B44 RID: 2884 RVA: 0x00082878 File Offset: 0x00080C78
	Private Sub onLoadedCloudData(data As String(), result As CloudLoadResult)
		If result = CloudLoadResult.Failed Then
			LocalAchievementsManager.loadFromCloud()
			Return
		End If
		Try
			If result = CloudLoadResult.NoData Then
				LocalAchievementsManager.achievementData = New LocalAchievementsManager.AchievementData()
				LocalAchievementsManager.saveToCloud()
			Else
				LocalAchievementsManager.achievementData = JsonUtility.FromJson(Of LocalAchievementsManager.AchievementData)(data(0))
			End If
		Catch ex As ArgumentException
			LocalAchievementsManager.achievementData = New LocalAchievementsManager.AchievementData()
		End Try
	End Sub

	' Token: 0x040014BF RID: 5311
	Private CloudKey As String = "cuphead_ach"

	' Token: 0x040014C0 RID: 5312
	Public DLCAchievements As LocalAchievementsManager.Achievement() = New LocalAchievementsManager.Achievement() { LocalAchievementsManager.Achievement.CompleteWorldDLC, LocalAchievementsManager.Achievement.ARankWorldDLC, LocalAchievementsManager.Achievement.DefeatBossAsChalice, LocalAchievementsManager.Achievement.DefeatXBossesAsChalice, LocalAchievementsManager.Achievement.ChaliceSuperWin, LocalAchievementsManager.Achievement.DefeatBossDLCWeapon, LocalAchievementsManager.Achievement.DefeatAllKOG, LocalAchievementsManager.Achievement.DefeatKOGGauntlet, LocalAchievementsManager.Achievement.DefeatSaltbaker, LocalAchievementsManager.Achievement.SRankAnyDLC, LocalAchievementsManager.Achievement.DefeatBossNoMinions, LocalAchievementsManager.Achievement.HP9, LocalAchievementsManager.Achievement.DefeatDevilPhase2, LocalAchievementsManager.Achievement.Paladin }

	' Token: 0x040014C2 RID: 5314
	Private initialized As Boolean

	' Token: 0x040014C3 RID: 5315
	Private achievementData As LocalAchievementsManager.AchievementData

	' Token: 0x0200039B RID: 923
	Public Enum Achievement
		' Token: 0x040014C7 RID: 5319
		DefeatBoss
		' Token: 0x040014C8 RID: 5320
		ParryApprentice
		' Token: 0x040014C9 RID: 5321
		ParryMaster
		' Token: 0x040014CA RID: 5322
		ExWin
		' Token: 0x040014CB RID: 5323
		SuperWin
		' Token: 0x040014CC RID: 5324
		ParryChain
		' Token: 0x040014CD RID: 5325
		NoHitsTaken
		' Token: 0x040014CE RID: 5326
		ARankWorld1
		' Token: 0x040014CF RID: 5327
		ARankWorld2
		' Token: 0x040014D0 RID: 5328
		ARankWorld3
		' Token: 0x040014D1 RID: 5329
		CompleteWorld1
		' Token: 0x040014D2 RID: 5330
		CompleteWorld2
		' Token: 0x040014D3 RID: 5331
		CompleteWorld3
		' Token: 0x040014D4 RID: 5332
		UnlockedAllSupers
		' Token: 0x040014D5 RID: 5333
		FoundAllLevelMoney
		' Token: 0x040014D6 RID: 5334
		BoughtAllItems
		' Token: 0x040014D7 RID: 5335
		CompleteDicePalace
		' Token: 0x040014D8 RID: 5336
		ARankWorld4
		' Token: 0x040014D9 RID: 5337
		GoodEnding
		' Token: 0x040014DA RID: 5338
		SRank
		' Token: 0x040014DB RID: 5339
		NewGamePlus
		' Token: 0x040014DC RID: 5340
		FoundSecretPassage
		' Token: 0x040014DD RID: 5341
		SmallPlaneOnlyWin
		' Token: 0x040014DE RID: 5342
		FoundAllMoney
		' Token: 0x040014DF RID: 5343
		PacifistRun
		' Token: 0x040014E0 RID: 5344
		NoHitsTakenDicePalace
		' Token: 0x040014E1 RID: 5345
		BadEnding
		' Token: 0x040014E2 RID: 5346
		CompleteDevil
		' Token: 0x040014E3 RID: 5347
		CompleteWorldDLC
		' Token: 0x040014E4 RID: 5348
		ARankWorldDLC
		' Token: 0x040014E5 RID: 5349
		DefeatBossAsChalice
		' Token: 0x040014E6 RID: 5350
		DefeatXBossesAsChalice
		' Token: 0x040014E7 RID: 5351
		ChaliceSuperWin
		' Token: 0x040014E8 RID: 5352
		DefeatBossDLCWeapon
		' Token: 0x040014E9 RID: 5353
		DefeatAllKOG
		' Token: 0x040014EA RID: 5354
		DefeatKOGGauntlet
		' Token: 0x040014EB RID: 5355
		DefeatSaltbaker
		' Token: 0x040014EC RID: 5356
		SRankAnyDLC
		' Token: 0x040014ED RID: 5357
		DefeatBossNoMinions
		' Token: 0x040014EE RID: 5358
		HP9
		' Token: 0x040014EF RID: 5359
		DefeatDevilPhase2
		' Token: 0x040014F0 RID: 5360
		Paladin
	End Enum

	' Token: 0x0200039C RID: 924
	<Serializable()>
	Private Class AchievementData
		' Token: 0x040014F1 RID: 5361
		Public unlockedAchievements As List(Of LocalAchievementsManager.Achievement) = New List(Of LocalAchievementsManager.Achievement)()

		' Token: 0x040014F2 RID: 5362
		Public parryCount As Integer
	End Class
End Module
