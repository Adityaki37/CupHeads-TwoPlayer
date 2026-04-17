Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x020008FB RID: 2299
Public Class PlatformingLevel
	Inherits Level

	' Token: 0x1700045C RID: 1116
	' (get) Token: 0x060035E7 RID: 13799 RVA: 0x001D495E File Offset: 0x001D2D5E
	' (set) Token: 0x060035E8 RID: 13800 RVA: 0x001D4965 File Offset: 0x001D2D65
	Public Shared Property Current As PlatformingLevel

	' Token: 0x1700045D RID: 1117
	' (get) Token: 0x060035E9 RID: 13801 RVA: 0x001D496D File Offset: 0x001D2D6D
	Public Overrides ReadOnly Property CurrentLevel As Levels
		Get
			Return Me._currentLevel
		End Get
	End Property

	' Token: 0x1700045E RID: 1118
	' (get) Token: 0x060035EA RID: 13802 RVA: 0x001D4975 File Offset: 0x001D2D75
	Public Overrides ReadOnly Property CurrentScene As Scenes
		Get
			Return Me._currentScene
		End Get
	End Property

	' Token: 0x1700045F RID: 1119
	' (get) Token: 0x060035EB RID: 13803 RVA: 0x001D497D File Offset: 0x001D2D7D
	Public Overrides ReadOnly Property BossPortrait As Sprite
		Get
			Return If((Not Me.useAltQuote), Me._bossPortrait, Me._bossPortraitAlt)
		End Get
	End Property

	' Token: 0x17000460 RID: 1120
	' (get) Token: 0x060035EC RID: 13804 RVA: 0x001D499B File Offset: 0x001D2D9B
	Public Overrides ReadOnly Property BossQuote As String
		Get
			Return If((Not Me.useAltQuote), Me._bossQuote, Me._bossQuoteAlt)
		End Get
	End Property

	' Token: 0x060035ED RID: 13805 RVA: 0x001D49BC File Offset: 0x001D2DBC
	Protected Overrides Sub Awake()
		Me._currentLevel = SceneLoader.CurrentLevel
		Me._currentScene = EnumUtils.Parse(Of Scenes)(LevelProperties.GetLevelScene(Me._currentLevel))
		Me.goalTimes = New Level.GoalTimes(Me.goalTime, Me.goalTime, Me.goalTime)
		Level.OverrideDifficulty = True
		MyBase.mode = Level.Mode.Normal
		MyBase.Awake()
		PlatformingLevel.Current = Me
	End Sub

	' Token: 0x060035EE RID: 13806 RVA: 0x001D4A20 File Offset: 0x001D2E20
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.LevelCoinsIDs.Sort(Function(a As CoinPositionAndID, b As CoinPositionAndID) a.xPos.CompareTo(b.xPos))
	End Sub

	' Token: 0x060035EF RID: 13807 RVA: 0x001D4A50 File Offset: 0x001D2E50
	Protected Overrides Sub OnLevelStart()
		MyBase.OnLevelStart()
		MyBase.timeline = New Level.Timeline()
		MyBase.timeline.health = 100F
		MyBase.StartCoroutine(Me.checkPosition_cr())
		Level.ScoringData.pacifistRun = True
		AddHandler PlatformingLevelExit.OnWinStartEvent, AddressOf Me.OnWinStart
		AddHandler PlatformingLevelExit.OnWinCompleteEvent, AddressOf Me.OnWinComplete
	End Sub

	' Token: 0x060035F0 RID: 13808 RVA: 0x001D4AB8 File Offset: 0x001D2EB8
	Private Sub OnWinStart()
		MyBase.Ending = True
		CupheadLevelCamera.Current.MoveRightCollider()
	End Sub

	' Token: 0x060035F1 RID: 13809 RVA: 0x001D4ACB File Offset: 0x001D2ECB
	Private Sub OnWinComplete()
		LevelCoin.OnLevelComplete()
		Level.ScoringData.coinsCollected = PlayerData.Data.GetNumCoinsCollectedInLevel(Me.CurrentLevel)
		Level.ScoringData.useCoinsInsteadOfSuperMeter = True
		MyBase.zHack_OnWin()
	End Sub

	' Token: 0x060035F2 RID: 13810 RVA: 0x001D4AFD File Offset: 0x001D2EFD
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		If PlatformingLevel.Current Is Me Then
			PlatformingLevel.Current = Nothing
		End If
		Me._bossPortrait = Nothing
		Me._bossPortraitAlt = Nothing
	End Sub

	' Token: 0x060035F3 RID: 13811 RVA: 0x001D4B2C File Offset: 0x001D2F2C
	Protected Overrides Sub Reset()
		MyBase.Reset()
		Me.type = Level.Type.Platforming
		Me.bounds.bottom = 500
		Me.camera.moveX = True
		Me.camera.moveY = True
		Me.camera.mode = CupheadLevelCamera.Mode.Platforming
		Me.camera.colliders = True
		Me.camera.bounds.rightEnabled = False
		Me.camera.bounds.topEnabled = False
	End Sub

	' Token: 0x060035F4 RID: 13812 RVA: 0x001D4BA8 File Offset: 0x001D2FA8
	Private Iterator Function checkPosition_cr() As IEnumerator
		While True
			For Each abstractPlayerController As AbstractPlayerController In Me.players
				If abstractPlayerController IsNot Nothing AndAlso Not abstractPlayerController.IsDead AndAlso MyBase.LevelType = Level.Type.Platforming AndAlso Me.camera.mode = CupheadLevelCamera.Mode.Path Then
					Dim num As Single = Me.camera.path.GetClosestNormalizedPoint(abstractPlayerController.center, abstractPlayerController.center, True, True) * 100F
					MyBase.timeline.SetPlayerDamage(abstractPlayerController.id, num)
				End If
			Next
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060035F5 RID: 13813 RVA: 0x001D4BC3 File Offset: 0x001D2FC3
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		Me.DrawGizmos(0.2F)
	End Sub

	' Token: 0x060035F6 RID: 13814 RVA: 0x001D4BD6 File Offset: 0x001D2FD6
	Protected Overrides Sub OnDrawGizmosSelected()
		MyBase.OnDrawGizmosSelected()
		Me.DrawGizmos(1F)
	End Sub

	' Token: 0x060035F7 RID: 13815 RVA: 0x001D4BE9 File Offset: 0x001D2FE9
	Private Sub DrawGizmos(a As Single)
		If Me.camera.mode <> CupheadLevelCamera.Mode.Path Then
			Return
		End If
		Me.camera.path.DrawGizmos(a, MyBase.baseTransform.position)
	End Sub

	' Token: 0x04003DFA RID: 15866
	Public Const TIMELINE_LENGTH As Single = 100F

	' Token: 0x04003DFC RID: 15868
	Public LevelCoinsIDs As List(Of CoinPositionAndID) = New List(Of CoinPositionAndID)()

	' Token: 0x04003DFD RID: 15869
	<Header("Boss Info")>
	<SerializeField()>
	Private _bossPortrait As Sprite

	' Token: 0x04003DFE RID: 15870
	<SerializeField()>
	Private _bossPortraitAlt As Sprite

	' Token: 0x04003DFF RID: 15871
	<SerializeField()>
	Private _bossQuote As String

	' Token: 0x04003E00 RID: 15872
	<SerializeField()>
	Private _bossQuoteAlt As String

	' Token: 0x04003E01 RID: 15873
	<SerializeField()>
	Private goalTime As Single

	' Token: 0x04003E02 RID: 15874
	Private _currentLevel As Levels

	' Token: 0x04003E03 RID: 15875
	Private _currentScene As Scenes

	' Token: 0x04003E04 RID: 15876
	Public useAltQuote As Boolean

	' Token: 0x020008FC RID: 2300
	Public Enum Theme
		' Token: 0x04003E07 RID: 15879
		Forest
	End Enum
End Class
