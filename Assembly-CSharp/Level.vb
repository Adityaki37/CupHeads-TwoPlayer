Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Diagnostics
Imports UnityEngine

' Token: 0x02000491 RID: 1169
Public MustInherit Class Level
	Inherits AbstractPausableComponent

	' Token: 0x170002D3 RID: 723
	' (get) Token: 0x06001269 RID: 4713 RVA: 0x0004C9E7 File Offset: 0x0004ADE7
	' (set) Token: 0x0600126A RID: 4714 RVA: 0x0004C9EE File Offset: 0x0004ADEE
	Public Shared Property Current As Level

	' Token: 0x170002D4 RID: 724
	' (get) Token: 0x0600126B RID: 4715 RVA: 0x0004C9F6 File Offset: 0x0004ADF6
	' (set) Token: 0x0600126C RID: 4716 RVA: 0x0004C9FD File Offset: 0x0004ADFD
	Public Shared Property CurrentMode As Level.Mode = Level.Mode.Normal

	' Token: 0x0600126D RID: 4717 RVA: 0x0004CA05 File Offset: 0x0004AE05
	Public Shared Sub SetCurrentMode(mode As Level.Mode)
		Level.CurrentMode = mode
	End Sub

	' Token: 0x170002D5 RID: 725
	' (get) Token: 0x0600126E RID: 4718 RVA: 0x0004CA0D File Offset: 0x0004AE0D
	' (set) Token: 0x0600126F RID: 4719 RVA: 0x0004CA14 File Offset: 0x0004AE14
	Public Shared Property PreviouslyWon As Boolean

	' Token: 0x170002D6 RID: 726
	' (get) Token: 0x06001270 RID: 4720 RVA: 0x0004CA1C File Offset: 0x0004AE1C
	' (set) Token: 0x06001271 RID: 4721 RVA: 0x0004CA23 File Offset: 0x0004AE23
	Public Shared Property Won As Boolean

	' Token: 0x170002D7 RID: 727
	' (get) Token: 0x06001272 RID: 4722 RVA: 0x0004CA2B File Offset: 0x0004AE2B
	' (set) Token: 0x06001273 RID: 4723 RVA: 0x0004CA32 File Offset: 0x0004AE32
	Public Shared Property Grade As LevelScoringData.Grade

	' Token: 0x170002D8 RID: 728
	' (get) Token: 0x06001274 RID: 4724 RVA: 0x0004CA3A File Offset: 0x0004AE3A
	' (set) Token: 0x06001275 RID: 4725 RVA: 0x0004CA41 File Offset: 0x0004AE41
	Public Shared Property PreviousGrade As LevelScoringData.Grade

	' Token: 0x170002D9 RID: 729
	' (get) Token: 0x06001276 RID: 4726 RVA: 0x0004CA49 File Offset: 0x0004AE49
	' (set) Token: 0x06001277 RID: 4727 RVA: 0x0004CA50 File Offset: 0x0004AE50
	Public Shared Property Difficulty As Level.Mode

	' Token: 0x170002DA RID: 730
	' (get) Token: 0x06001278 RID: 4728 RVA: 0x0004CA58 File Offset: 0x0004AE58
	' (set) Token: 0x06001279 RID: 4729 RVA: 0x0004CA5F File Offset: 0x0004AE5F
	Public Shared Property PreviousDifficulty As Level.Mode

	' Token: 0x170002DB RID: 731
	' (get) Token: 0x0600127A RID: 4730 RVA: 0x0004CA67 File Offset: 0x0004AE67
	' (set) Token: 0x0600127B RID: 4731 RVA: 0x0004CA6E File Offset: 0x0004AE6E
	Public Shared Property ScoringData As LevelScoringData

	' Token: 0x170002DC RID: 732
	' (get) Token: 0x0600127C RID: 4732 RVA: 0x0004CA76 File Offset: 0x0004AE76
	' (set) Token: 0x0600127D RID: 4733 RVA: 0x0004CA7D File Offset: 0x0004AE7D
	Public Shared Property PreviousLevel As Levels

	' Token: 0x170002DD RID: 733
	' (get) Token: 0x0600127E RID: 4734 RVA: 0x0004CA85 File Offset: 0x0004AE85
	' (set) Token: 0x0600127F RID: 4735 RVA: 0x0004CA8C File Offset: 0x0004AE8C
	Public Shared Property PreviousLevelType As Level.Type

	' Token: 0x170002DE RID: 734
	' (get) Token: 0x06001280 RID: 4736 RVA: 0x0004CA94 File Offset: 0x0004AE94
	' (set) Token: 0x06001281 RID: 4737 RVA: 0x0004CA9B File Offset: 0x0004AE9B
	Public Shared Property IsDicePalace As Boolean

	' Token: 0x170002DF RID: 735
	' (get) Token: 0x06001282 RID: 4738 RVA: 0x0004CAA3 File Offset: 0x0004AEA3
	' (set) Token: 0x06001283 RID: 4739 RVA: 0x0004CAAA File Offset: 0x0004AEAA
	Public Shared Property IsDicePalaceMain As Boolean

	' Token: 0x170002E0 RID: 736
	' (get) Token: 0x06001284 RID: 4740 RVA: 0x0004CAB2 File Offset: 0x0004AEB2
	' (set) Token: 0x06001285 RID: 4741 RVA: 0x0004CAB9 File Offset: 0x0004AEB9
	Public Shared Property SuperUnlocked As Boolean

	' Token: 0x170002E1 RID: 737
	' (get) Token: 0x06001286 RID: 4742 RVA: 0x0004CAC1 File Offset: 0x0004AEC1
	' (set) Token: 0x06001287 RID: 4743 RVA: 0x0004CAC8 File Offset: 0x0004AEC8
	Public Shared Property OverrideDifficulty As Boolean

	' Token: 0x170002E2 RID: 738
	' (get) Token: 0x06001288 RID: 4744 RVA: 0x0004CAD0 File Offset: 0x0004AED0
	' (set) Token: 0x06001289 RID: 4745 RVA: 0x0004CAD7 File Offset: 0x0004AED7
	Public Shared Property IsChessBoss As Boolean

	' Token: 0x170002E3 RID: 739
	' (get) Token: 0x0600128A RID: 4746 RVA: 0x0004CADF File Offset: 0x0004AEDF
	' (set) Token: 0x0600128B RID: 4747 RVA: 0x0004CAE6 File Offset: 0x0004AEE6
	Public Shared Property IsTowerOfPower As Boolean

	' Token: 0x170002E4 RID: 740
	' (get) Token: 0x0600128C RID: 4748 RVA: 0x0004CAEE File Offset: 0x0004AEEE
	' (set) Token: 0x0600128D RID: 4749 RVA: 0x0004CAF5 File Offset: 0x0004AEF5
	Public Shared Property IsTowerOfPowerMain As Boolean

	' Token: 0x170002E5 RID: 741
	' (get) Token: 0x0600128E RID: 4750 RVA: 0x0004CAFD File Offset: 0x0004AEFD
	' (set) Token: 0x0600128F RID: 4751 RVA: 0x0004CB04 File Offset: 0x0004AF04
	Public Shared Property IsGraveyard As Boolean

	' Token: 0x06001290 RID: 4752 RVA: 0x0004CB0C File Offset: 0x0004AF0C
	Public Shared Sub ResetPreviousLevelInfo()
		Level.Won = False
		Level.SuperUnlocked = False
		PlayerManager.playerWasChalice(0) = False
		PlayerManager.playerWasChalice(1) = False
	End Sub

	' Token: 0x06001291 RID: 4753 RVA: 0x0004CB2C File Offset: 0x0004AF2C
	Public Shared Function GetEnumByName(levelName As String) As Levels
		Return CType([Enum].Parse(GetType(Levels), levelName), Levels)
	End Function

	' Token: 0x06001292 RID: 4754 RVA: 0x0004CB50 File Offset: 0x0004AF50
	Public Shared Function GetLevelName(level As Levels) As String
		Return Localization.Translate(level.ToString()).text
	End Function

	' Token: 0x170002E6 RID: 742
	' (get) Token: 0x06001293 RID: 4755 RVA: 0x0004CB77 File Offset: 0x0004AF77
	' (set) Token: 0x06001294 RID: 4756 RVA: 0x0004CB7F File Offset: 0x0004AF7F
	Public Property mode As Level.Mode

	' Token: 0x170002E7 RID: 743
	' (get) Token: 0x06001295 RID: 4757 RVA: 0x0004CB88 File Offset: 0x0004AF88
	' (set) Token: 0x06001296 RID: 4758 RVA: 0x0004CB90 File Offset: 0x0004AF90
	Public Property defeatedMinion As Boolean

	' Token: 0x170002E8 RID: 744
	' (get) Token: 0x06001297 RID: 4759 RVA: 0x0004CB99 File Offset: 0x0004AF99
	' (set) Token: 0x06001298 RID: 4760 RVA: 0x0004CBA1 File Offset: 0x0004AFA1
	Public Property PlayersCreated As Boolean

	' Token: 0x170002E9 RID: 745
	' (get) Token: 0x06001299 RID: 4761 RVA: 0x0004CBAA File Offset: 0x0004AFAA
	' (set) Token: 0x0600129A RID: 4762 RVA: 0x0004CBB2 File Offset: 0x0004AFB2
	Public Property Initialized As Boolean

	' Token: 0x170002EA RID: 746
	' (get) Token: 0x0600129B RID: 4763 RVA: 0x0004CBBB File Offset: 0x0004AFBB
	' (set) Token: 0x0600129C RID: 4764 RVA: 0x0004CBC3 File Offset: 0x0004AFC3
	Public Property Started As Boolean

	' Token: 0x170002EB RID: 747
	' (get) Token: 0x0600129D RID: 4765 RVA: 0x0004CBCC File Offset: 0x0004AFCC
	' (set) Token: 0x0600129E RID: 4766 RVA: 0x0004CBD4 File Offset: 0x0004AFD4
	Public Property BlockChaliceCharm As Boolean()

	' Token: 0x170002EC RID: 748
	' (get) Token: 0x0600129F RID: 4767 RVA: 0x0004CBDD File Offset: 0x0004AFDD
	' (set) Token: 0x060012A0 RID: 4768 RVA: 0x0004CBE5 File Offset: 0x0004AFE5
	Public Property LevelTime As Single

	' Token: 0x170002ED RID: 749
	' (get) Token: 0x060012A1 RID: 4769 RVA: 0x0004CBEE File Offset: 0x0004AFEE
	Public ReadOnly Property Ground As Integer
		Get
			Return-Me.bounds.bottom
		End Get
	End Property

	' Token: 0x170002EE RID: 750
	' (get) Token: 0x060012A2 RID: 4770 RVA: 0x0004CBFC File Offset: 0x0004AFFC
	Public ReadOnly Property Ceiling As Integer
		Get
			Return Me.bounds.top
		End Get
	End Property

	' Token: 0x170002EF RID: 751
	' (get) Token: 0x060012A3 RID: 4771 RVA: 0x0004CC09 File Offset: 0x0004B009
	Public ReadOnly Property Left As Integer
		Get
			Return-Me.bounds.left
		End Get
	End Property

	' Token: 0x170002F0 RID: 752
	' (get) Token: 0x060012A4 RID: 4772 RVA: 0x0004CC17 File Offset: 0x0004B017
	Public ReadOnly Property Right As Integer
		Get
			Return Me.bounds.right
		End Get
	End Property

	' Token: 0x170002F1 RID: 753
	' (get) Token: 0x060012A5 RID: 4773 RVA: 0x0004CC24 File Offset: 0x0004B024
	Public ReadOnly Property Width As Integer
		Get
			Return Me.bounds.left + Me.bounds.right
		End Get
	End Property

	' Token: 0x170002F2 RID: 754
	' (get) Token: 0x060012A6 RID: 4774 RVA: 0x0004CC3D File Offset: 0x0004B03D
	Public ReadOnly Property Height As Integer
		Get
			Return Me.bounds.top + Me.bounds.bottom
		End Get
	End Property

	' Token: 0x170002F3 RID: 755
	' (get) Token: 0x060012A7 RID: 4775 RVA: 0x0004CC56 File Offset: 0x0004B056
	Public ReadOnly Property LevelType As Level.Type
		Get
			Return Me.type
		End Get
	End Property

	' Token: 0x170002F4 RID: 756
	' (get) Token: 0x060012A8 RID: 4776 RVA: 0x0004CC5E File Offset: 0x0004B05E
	' (set) Token: 0x060012A9 RID: 4777 RVA: 0x0004CC66 File Offset: 0x0004B066
	Public Property CameraRotates As Boolean

	' Token: 0x170002F5 RID: 757
	' (get) Token: 0x060012AA RID: 4778 RVA: 0x0004CC6F File Offset: 0x0004B06F
	Public ReadOnly Property IntroComplete As Boolean
		Get
			Return Me.intro.introComplete
		End Get
	End Property

	' Token: 0x170002F6 RID: 758
	' (get) Token: 0x060012AB RID: 4779 RVA: 0x0004CC7C File Offset: 0x0004B07C
	' (set) Token: 0x060012AC RID: 4780 RVA: 0x0004CC84 File Offset: 0x0004B084
	Public Property timeline As Level.Timeline

	' Token: 0x170002F7 RID: 759
	' (get) Token: 0x060012AD RID: 4781
	Public MustOverride ReadOnly Property CurrentLevel As Levels

	' Token: 0x170002F8 RID: 760
	' (get) Token: 0x060012AE RID: 4782
	Public MustOverride ReadOnly Property CurrentScene As Scenes

	' Token: 0x170002F9 RID: 761
	' (get) Token: 0x060012AF RID: 4783 RVA: 0x0004CC8D File Offset: 0x0004B08D
	Public ReadOnly Property CameraSettings As Level.Camera
		Get
			Return Me.camera
		End Get
	End Property

	' Token: 0x170002FA RID: 762
	' (get) Token: 0x060012B0 RID: 4784
	Public MustOverride ReadOnly Property BossPortrait As Sprite

	' Token: 0x170002FB RID: 763
	' (get) Token: 0x060012B1 RID: 4785
	Public MustOverride ReadOnly Property BossQuote As String

	' Token: 0x170002FC RID: 764
	' (get) Token: 0x060012B2 RID: 4786 RVA: 0x0004CC95 File Offset: 0x0004B095
	' (set) Token: 0x060012B3 RID: 4787 RVA: 0x0004CC9D File Offset: 0x0004B09D
	Public Property Ending As Boolean

	' Token: 0x170002FD RID: 765
	' (get) Token: 0x060012B4 RID: 4788 RVA: 0x0004CCA6 File Offset: 0x0004B0A6
	Public Shared ReadOnly Property IsInBossesHub As Boolean
		Get
			Return Level.IsDicePalace OrElse Level.IsDicePalaceMain OrElse Level.IsTowerOfPower
		End Get
	End Property

	' Token: 0x060012B5 RID: 4789 RVA: 0x0004CCC4 File Offset: 0x0004B0C4
	Public Shared Function GetPlayerStats(playerId As PlayerId) As PlayersStatsBossesHub
		If Level.IsTowerOfPower Then
			Return TowerOfPowerLevelGameInfo.PLAYER_STATS(CInt(playerId))
		End If
		If Level.IsDicePalace OrElse Level.IsDicePalaceMain Then
			Return If((playerId <> PlayerId.PlayerOne), DicePalaceMainLevelGameInfo.PLAYER_TWO_STATS, DicePalaceMainLevelGameInfo.PLAYER_ONE_STATS)
		End If
		Return Nothing
	End Function

	' Token: 0x060012B6 RID: 4790 RVA: 0x0004CD03 File Offset: 0x0004B103
	Protected Overridable Sub OnEnable()
		EventManager.Instance.AddListener(Of PlayerStatsManager.DeathEvent)(AddressOf Me.OnPlayerDeath)
		EventManager.Instance.AddListener(Of PlayerStatsManager.ReviveEvent)(AddressOf Me.OnPlayerRevive)
	End Sub

	' Token: 0x060012B7 RID: 4791 RVA: 0x0004CD31 File Offset: 0x0004B131
	Protected Overridable Sub OnDisable()
		EventManager.Instance.RemoveListener(Of PlayerStatsManager.DeathEvent)(AddressOf Me.OnPlayerDeath)
		EventManager.Instance.RemoveListener(Of PlayerStatsManager.ReviveEvent)(AddressOf Me.OnPlayerRevive)
	End Sub

	' Token: 0x060012B8 RID: 4792 RVA: 0x0004CD60 File Offset: 0x0004B160
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.CheckIfInABossesHub()
		Cuphead.Init(False)
		AddHandler PlayerManager.OnPlayerJoinedEvent, AddressOf Me.OnPlayerJoined
		AddHandler PlayerManager.OnPlayerLeaveEvent, AddressOf Me.OnPlayerLeave
		DamageDealer.didDamageWithNonSmallPlaneWeapon = False
		Dim currentLevel As Levels = Me.CurrentLevel
		Select Case currentLevel
			Case Levels.Platforming_Level_1_1, Levels.Platforming_Level_1_2, Levels.Platforming_Level_3_1, Levels.Platforming_Level_3_2
			Case Else
				If currentLevel <> Levels.Platforming_Level_2_2 AndAlso currentLevel <> Levels.Platforming_Level_2_1 Then
					Me.mode = Level.CurrentMode
					GoTo IL_0095
				End If
		End Select
		Me.mode = Level.Mode.Normal
		IL_0095:
		Level.Current = Me
		Dim levelData As PlayerData.PlayerLevelDataObject = PlayerData.Data.GetLevelData(Me.CurrentLevel)
		Level.Won = False
		Me.BGMPlaylistCurrent = levelData.bgmPlayListCurrent
		Level.PreviousLevel = Me.CurrentLevel
		Level.PreviousLevelType = Me.type
		Level.PreviouslyWon = levelData.completed
		Level.PreviousGrade = levelData.grade
		Level.PreviousDifficulty = levelData.difficultyBeaten
		Level.SuperUnlocked = False
		Level.IsChessBoss = False
		Level.IsGraveyard = False
		Me.Ending = False
		Me.PartialInit()
		Application.targetFrameRate = 60
		Me.CreateUI()
		Me.CreateHUD()
		LevelCoin.OnLevelStart()
		SceneLoader.SetCurrentLevel(Me.CurrentLevel)
	End Sub

	' Token: 0x060012B9 RID: 4793 RVA: 0x0004CEA4 File Offset: 0x0004B2A4
	Public Overridable Function AllowDjimmi() As Boolean
		Return Not Me.isMausoleum AndAlso Me.type <> Level.Type.Tutorial AndAlso Not(TypeOf SceneLoader.CurrentContext Is GauntletContext)
	End Function

	' Token: 0x060012BA RID: 4794 RVA: 0x0004CED0 File Offset: 0x0004B2D0
	Protected Overridable Sub CheckIfInABossesHub()
		Level.IsDicePalace = False
		Level.IsDicePalaceMain = False
	End Sub

	' Token: 0x060012BB RID: 4795 RVA: 0x0004CEDE File Offset: 0x0004B2DE
	Public Shared Sub ResetBossesHub()
		Level.IsDicePalace = False
		Level.IsDicePalaceMain = False
		If Level.IsTowerOfPower Then
			TowerOfPowerLevelGameInfo.GameInfo.CleanUp()
			Level.IsTowerOfPower = False
			Level.IsTowerOfPowerMain = False
		End If
	End Sub

	' Token: 0x060012BC RID: 4796 RVA: 0x0004CF0C File Offset: 0x0004B30C
	Protected Overridable Sub PartialInit()
		If Level.ScoringData Is Nothing OrElse ((Me.type = Level.Type.Battle OrElse Me.type = Level.Type.Platforming) AndAlso (Not Level.IsDicePalace OrElse (Level.IsDicePalaceMain AndAlso DicePalaceMainLevelGameInfo.TURN_COUNTER = 0)) AndAlso (Not Level.IsTowerOfPowerMain OrElse TowerOfPowerLevelGameInfo.TURN_COUNTER = 0)) Then
			Level.ScoringData = New LevelScoringData()
			Level.ScoringData.goalTime = If((Me.mode <> Level.Mode.Easy), If((Me.mode <> Level.Mode.Normal), Me.goalTimes.hard, Me.goalTimes.normal), Me.goalTimes.easy)
		End If
		If(Level.IsDicePalace AndAlso Not Level.IsDicePalaceMain) OrElse (Level.IsTowerOfPower AndAlso Not Level.IsTowerOfPowerMain) Then
			Level.ScoringData.goalTime += If((Me.mode <> Level.Mode.Easy), If((Me.mode <> Level.Mode.Normal), Me.goalTimes.hard, Me.goalTimes.normal), Me.goalTimes.easy)
		End If
		Level.ScoringData.difficulty = Me.mode
	End Sub

	' Token: 0x060012BD RID: 4797 RVA: 0x0004D050 File Offset: 0x0004B450
	Protected Overridable Sub Start()
		CupheadTime.SetAll(1F)
		Select Case Me.type
			Case Else
				MyBase.StartCoroutine(Me.startBattle_cr())
			Case Level.Type.Tutorial
				MyBase.StartCoroutine(Me.startNonBattle_cr())
			Case Level.Type.Platforming
				MyBase.StartCoroutine(Me.startPlatforming_cr())
		End Select
		Me.CreateAudio()
		Me.CreateColliders()
		Me.CreatePlayers()
		Me.CreateCamera()
		Me.gui.LevelInit()
		Me.hud.LevelInit()
		Me.SetRichPresence()
		Me.Initialized = True
		If Me.playerMode <> PlayerMode.Plane AndAlso Me.CurrentLevel <> Levels.Devil AndAlso Me.CurrentLevel <> Levels.Saltbaker AndAlso Me.type <> Level.Type.Platforming AndAlso Me.type <> Level.Type.Tutorial Then
			MyBase.StartCoroutine(Me.check_intros_cr())
		End If
		If Me.CurrentLevel = Levels.Devil OrElse Me.CurrentLevel = Levels.Saltbaker Then
			Me.CheckIntros()
		End If
	End Sub

	' Token: 0x060012BE RID: 4798 RVA: 0x0004D170 File Offset: 0x0004B570
	Protected Overridable Sub Update()
		If Not Me.Started Then
			Me.CheckPlayerHoldingButtons()
		End If
		Me.LevelTime += CupheadTime.Delta
		If Me.playerIsDead Then
			Me.playerDeathDelayFrames += 1
			If Me.playerDeathDelayFrames < 5 Then
				If PlayerManager.Multiplayer Then
					If Me.players(0).IsDead AndAlso Me.players(1).IsDead Then
						Me._OnLose()
					End If
				Else
					Me._OnLose()
				End If
				Me.playerIsDead = False
				Me.playerDeathDelayFrames = 0
			End If
		End If
	End Sub

	' Token: 0x060012BF RID: 4799 RVA: 0x0004D218 File Offset: 0x0004B618
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		PlayerManager.ClearPlayers()
		Level.Current = Nothing
		RemoveHandler PlayerManager.OnPlayerJoinedEvent, AddressOf Me.OnPlayerJoined
		RemoveHandler PlayerManager.OnPlayerLeaveEvent, AddressOf Me.OnPlayerLeave
		Me.LevelResources = Nothing
		Me.players = Nothing
	End Sub

	' Token: 0x060012C0 RID: 4800 RVA: 0x0004D268 File Offset: 0x0004B668
	Public Sub SetBounds(left As Integer?, right As Integer?, top As Integer?, bottom As Integer?)
		If left IsNot Nothing Then
			Me.bounds.left = left.Value
		End If
		If right IsNot Nothing Then
			Me.bounds.right = right.Value
		End If
		If top IsNot Nothing Then
			Me.bounds.top = top.Value
		End If
		If bottom IsNot Nothing Then
			Me.bounds.bottom = bottom.Value
		End If
		Me.bounds.SetColliderPositions()
	End Sub

	' Token: 0x060012C1 RID: 4801 RVA: 0x0004D2F8 File Offset: 0x0004B6F8
	Protected Sub CleanUpScore()
		Level.ScoringData = Nothing
	End Sub

	' Token: 0x060012C2 RID: 4802 RVA: 0x0004D300 File Offset: 0x0004B700
	Public Sub RegisterMinionKilled()
		If Not Me.defeatedMinion Then
		End If
		Me.defeatedMinion = True
	End Sub

	' Token: 0x060012C3 RID: 4803 RVA: 0x0004D314 File Offset: 0x0004B714
	Private Sub CreateAudio()
		LevelAudio.Create()
	End Sub

	' Token: 0x060012C4 RID: 4804 RVA: 0x0004D31C File Offset: 0x0004B71C
	Protected Overridable Sub CreatePlayers()
		Me.PlayersCreated = True
		For Each abstractPlayerController As AbstractPlayerController In Global.UnityEngine.[Object].FindObjectsOfType(Of AbstractPlayerController)()
			Global.UnityEngine.[Object].Destroy(abstractPlayerController.gameObject)
		Next
		Me.players = New AbstractPlayerController(1) {}
		Me.BlockChaliceCharm = New Boolean(1) {}
		Me.BlockChaliceCharm(0) = Me.blockChalice
		Me.BlockChaliceCharm(1) = Me.blockChalice
		If Me.playerMode = PlayerMode.Custom Then
			Return
		End If
		If PlayerManager.Multiplayer AndAlso Me.allowMultiplayer Then
			If PlayerData.Data.Loadouts.GetPlayerLoadout(PlayerId.PlayerOne).charm = Charm.charm_chalice AndAlso PlayerData.Data.Loadouts.GetPlayerLoadout(PlayerId.PlayerTwo).charm = Charm.charm_chalice Then
				Me.BlockChaliceCharm(If((Not Rand.Bool()), 1, 0)) = True
			End If
			If Me.isMausoleum Then
				Me.BlockChaliceCharm(0) = True
				Me.BlockChaliceCharm(1) = True
			End If
			Dim vector As Vector3 = If((Me.playerMode <> PlayerMode.Plane), Me.spawns.playerOne, Me.player1PlaneSpawnPos)
			Me.players(0) = AbstractPlayerController.Create(PlayerId.PlayerOne, vector, Me.playerMode)
			Dim vector2 As Vector3 = If((Me.playerMode <> PlayerMode.Plane), Me.spawns.playerTwo, Me.player2PlaneSpawnPos)
			Me.players(1) = AbstractPlayerController.Create(PlayerId.PlayerTwo, vector2, Me.playerMode)
		Else
			Dim vector3 As Vector3 = If((Me.playerMode <> PlayerMode.Plane), Me.spawns.playerOneSingle, Me.player1PlaneSpawnPos)
			Me.players(0) = AbstractPlayerController.Create(PlayerId.PlayerOne, vector3, Me.playerMode)
		End If
	End Sub

	' Token: 0x060012C5 RID: 4805 RVA: 0x0004D4F8 File Offset: 0x0004B8F8
	Private Sub CheckPlayerCharacters()
		Level.ScoringData.player1IsChalice = Me.players(0).stats.isChalice
		If PlayerManager.Multiplayer AndAlso Me.allowMultiplayer Then
			Level.ScoringData.player2IsChalice = Me.players(1).stats.isChalice
		End If
	End Sub

	' Token: 0x060012C6 RID: 4806 RVA: 0x0004D554 File Offset: 0x0004B954
	Private Sub CheckIntros()
		Dim component As LevelPlayerAnimationController = Me.players(0).GetComponent(Of LevelPlayerAnimationController)()
		If component IsNot Nothing Then
			If Me.players(0).stats.Loadout.charm = Charm.charm_chalice Then
				If Me.players(1) IsNot Nothing AndAlso Me.players(1).stats.isChalice AndAlso Me.CurrentLevel <> Levels.Devil AndAlso Me.CurrentLevel <> Levels.Saltbaker AndAlso (Not Level.IsDicePalace OrElse DicePalaceMainLevelGameInfo.IS_FIRST_ENTRY) Then
					component.CookieFail()
				End If
				If Me.players(0).stats.isChalice AndAlso (Me.CurrentLevel = Levels.Devil OrElse Me.CurrentLevel = Levels.Saltbaker) Then
					component.ScaredChalice(Me.CurrentLevel = Levels.Devil)
				End If
			ElseIf Me.CurrentLevel <> Levels.Devil AndAlso Me.CurrentLevel <> Levels.Saltbaker Then
				If Me.player1HeldJump AndAlso Not Me.player1HeldSuper Then
					component.IsIntroB()
				ElseIf Not Me.player1HeldJump AndAlso Not Me.player1HeldSuper AndAlso Rand.Bool() Then
					component.IsIntroB()
				End If
			End If
		End If
		If Me.players.Length >= 2 AndAlso Me.players(1) IsNot Nothing Then
			Dim component2 As LevelPlayerAnimationController = Me.players(1).GetComponent(Of LevelPlayerAnimationController)()
			If component2 IsNot Nothing Then
				If Me.players(1).stats.Loadout.charm = Charm.charm_chalice Then
					If Me.players(0).stats.isChalice AndAlso Me.CurrentLevel <> Levels.Devil AndAlso Me.CurrentLevel <> Levels.Saltbaker AndAlso (Not Level.IsDicePalace OrElse DicePalaceMainLevelGameInfo.IS_FIRST_ENTRY) Then
						component2.CookieFail()
					End If
					If Me.players(1).stats.isChalice AndAlso (Me.CurrentLevel = Levels.Devil OrElse Me.CurrentLevel = Levels.Saltbaker) Then
						component2.ScaredChalice(Me.CurrentLevel = Levels.Devil)
					End If
				ElseIf PlayerManager.Multiplayer AndAlso Me.CurrentLevel <> Levels.Devil AndAlso Me.CurrentLevel <> Levels.Saltbaker Then
					If Me.player2HeldJump AndAlso Not Me.player2HeldSuper Then
						component2.IsIntroB()
					ElseIf Not Me.player2HeldJump AndAlso Not Me.player2HeldSuper AndAlso Rand.Bool() Then
						component2.IsIntroB()
					End If
				End If
			End If
		End If
	End Sub

	' Token: 0x060012C7 RID: 4807 RVA: 0x0004D824 File Offset: 0x0004BC24
	Protected Overridable Sub CreatePlayerTwoOnJoin()
		If PlayerManager.Multiplayer AndAlso Me.allowMultiplayer Then
			If Me.players(0).stats.isChalice OrElse Me.blockChalice Then
				Me.BlockChaliceCharm(1) = True
			End If
			Me.players(1) = AbstractPlayerController.Create(PlayerId.PlayerTwo, Me.players(0).center, Me.playerMode)
			Me.players(1).LevelJoin(Me.players(0).center)
		End If
	End Sub

	' Token: 0x060012C8 RID: 4808 RVA: 0x0004D8B4 File Offset: 0x0004BCB4
	Private Sub CreateCamera()
		If Me.players Is Nothing Then
			Global.Debug.LogError("Level.CreateCamera() must be called AFTER Level.CreatePlayers()", Nothing)
		End If
		Dim cupheadLevelCamera As CupheadLevelCamera = Global.UnityEngine.[Object].FindObjectOfType(Of CupheadLevelCamera)()
		cupheadLevelCamera.Init(Me.camera)
	End Sub

	' Token: 0x060012C9 RID: 4809 RVA: 0x0004D8E9 File Offset: 0x0004BCE9
	Private Sub CreateUI()
		Me.gui = Global.UnityEngine.[Object].FindObjectOfType(Of LevelGUI)()
		If Me.gui Is Nothing Then
			Me.gui = Me.LevelResources.levelGUI.InstantiatePrefab(Of LevelGUI)()
		End If
	End Sub

	' Token: 0x060012CA RID: 4810 RVA: 0x0004D922 File Offset: 0x0004BD22
	Private Sub CreateHUD()
		Me.hud = Global.UnityEngine.[Object].FindObjectOfType(Of LevelHUD)()
		If Me.hud Is Nothing Then
			Me.hud = Me.LevelResources.levelHUD.InstantiatePrefab(Of LevelHUD)()
		End If
	End Sub

	' Token: 0x060012CB RID: 4811 RVA: 0x0004D95C File Offset: 0x0004BD5C
	Private Sub CreateColliders()
		If Me.playerMode = PlayerMode.Plane Then
			Return
		End If
		Me.collidersRoot = New GameObject("Colliders").transform
		Me.collidersRoot.parent = MyBase.transform
		Me.collidersRoot.ResetLocalTransforms()
		Me.SetupCollider(Level.Bounds.Side.Left)
		Me.SetupCollider(Level.Bounds.Side.Right)
		Me.SetupCollider(Level.Bounds.Side.Top)
		Me.SetupCollider(Level.Bounds.Side.Bottom)
	End Sub

	' Token: 0x060012CC RID: 4812 RVA: 0x0004D9C8 File Offset: 0x0004BDC8
	Private Function SetupCollider(side As Level.Bounds.Side) As Transform
		Dim text As String = String.Empty
		Dim text2 As String = String.Empty
		Dim num As Integer = 0
		Dim num2 As Integer = 0
		Dim zero As Vector2 = Vector2.zero
		Select Case side
			Case Level.Bounds.Side.Left
				text = "Level_Wall_Left"
				text2 = "Wall"
				num = LayerMask.NameToLayer(Layers.Bounds_Walls.ToString())
				num2 = 90
			Case Level.Bounds.Side.Right
				text = "Level_Wall_Right"
				text2 = "Wall"
				num = LayerMask.NameToLayer(Layers.Bounds_Walls.ToString())
				num2 = -90
			Case Level.Bounds.Side.Top
				text = "Level_Ceiling"
				text2 = "Ceiling"
				num = LayerMask.NameToLayer(Layers.Bounds_Ceiling.ToString())
			Case Level.Bounds.Side.Bottom
				text = "Level_Ground"
				text2 = "Ground"
				num = LayerMask.NameToLayer(Layers.Bounds_Ground.ToString())
				num2 = 180
		End Select
		Dim gameObject As GameObject = New GameObject(text)
		gameObject.tag = text2
		gameObject.layer = num
		gameObject.transform.ResetLocalTransforms()
		gameObject.transform.SetPosition(New Single?(zero.x), New Single?(zero.y), Nothing)
		gameObject.transform.SetEulerAngles(Nothing, Nothing, New Single?(CSng(num2)))
		gameObject.transform.parent = Me.collidersRoot
		Dim boxCollider2D As BoxCollider2D = gameObject.AddComponent(Of BoxCollider2D)()
		boxCollider2D.isTrigger = True
		boxCollider2D.size = New Vector2(10000F, 400F)
		Dim rigidbody2D As Rigidbody2D = gameObject.AddComponent(Of Rigidbody2D)()
		rigidbody2D.gravityScale = 0F
		rigidbody2D.drag = 0F
		rigidbody2D.angularDrag = 0F
		rigidbody2D.isKinematic = True
		Me.bounds.colliders.Add(side, boxCollider2D)
		Me.bounds.SetColliderPositions()
		gameObject.SetActive(Me.bounds.GetEnabled(side))
		Return gameObject.transform
	End Function

	' Token: 0x060012CD RID: 4813 RVA: 0x0004DBD4 File Offset: 0x0004BFD4
	Protected Overrides Sub OnDrawGizmosSelected()
		MyBase.OnDrawGizmosSelected()
		Gizmos.color = Color.white
		Gizmos.DrawSphere(Me.spawns.playerOne, 20F)
		Gizmos.DrawSphere(Me.spawns.playerTwo, 20F)
		Gizmos.DrawSphere(Me.spawns.playerOneSingle, 30F)
		Gizmos.color = Color.red
		Gizmos.DrawCube(Me.spawns.playerOneSingle, New Vector3(20F, 20F, 20F))
		Gizmos.DrawWireSphere(Me.spawns.playerOne, 20F)
		Gizmos.color = Color.blue
		Gizmos.DrawWireSphere(Me.spawns.playerTwo, 20F)
		Gizmos.color = Color.white
		If Me.camera.bounds.topEnabled Then
			Gizmos.DrawLine(New Vector3(CSng(Me.camera.bounds.right), CSng(Me.camera.bounds.top), 0F), New Vector3(CSng((-CSng(Me.camera.bounds.left))), CSng(Me.camera.bounds.top), 0F))
		End If
		If Me.camera.bounds.bottomEnabled Then
			Gizmos.DrawLine(New Vector3(CSng(Me.camera.bounds.right), CSng((-CSng(Me.camera.bounds.bottom))), 0F), New Vector3(CSng((-CSng(Me.camera.bounds.left))), CSng((-CSng(Me.camera.bounds.bottom))), 0F))
		End If
		If Me.camera.bounds.leftEnabled Then
			Gizmos.DrawLine(New Vector3(CSng((-CSng(Me.camera.bounds.left))), CSng(Me.camera.bounds.top), 0F), New Vector3(CSng((-CSng(Me.camera.bounds.left))), CSng((-CSng(Me.camera.bounds.bottom))), 0F))
		End If
		If Me.camera.bounds.rightEnabled Then
			Gizmos.DrawLine(New Vector3(CSng(Me.camera.bounds.right), CSng(Me.camera.bounds.top), 0F), New Vector3(CSng(Me.camera.bounds.right), CSng((-CSng(Me.camera.bounds.bottom))), 0F))
		End If
		If Me.bounds.topEnabled Then
			Gizmos.color = Color.blue
		Else
			Gizmos.color = Color.black
		End If
		Gizmos.DrawLine(New Vector3(CSng(Me.bounds.right), CSng(Me.bounds.top), 0F), New Vector3(CSng((-CSng(Me.bounds.left))), CSng(Me.bounds.top), 0F))
		If Me.bounds.bottomEnabled Then
			Gizmos.color = Color.green
		Else
			Gizmos.color = Color.black
		End If
		Gizmos.DrawLine(New Vector3(CSng(Me.bounds.right), CSng((-CSng(Me.bounds.bottom))), 0F), New Vector3(CSng((-CSng(Me.bounds.left))), CSng((-CSng(Me.bounds.bottom))), 0F))
		If Me.bounds.leftEnabled Then
			Gizmos.color = Color.red
		Else
			Gizmos.color = Color.black
		End If
		Gizmos.DrawLine(New Vector3(CSng((-CSng(Me.bounds.left))), CSng(Me.bounds.top), 0F), New Vector3(CSng((-CSng(Me.bounds.left))), CSng((-CSng(Me.bounds.bottom))), 0F))
		If Me.bounds.rightEnabled Then
			Gizmos.color = Color.red
		Else
			Gizmos.color = Color.black
		End If
		Gizmos.DrawLine(New Vector3(CSng(Me.bounds.right), CSng(Me.bounds.top), 0F), New Vector3(CSng(Me.bounds.right), CSng((-CSng(Me.bounds.bottom))), 0F))
	End Sub

	' Token: 0x060012CE RID: 4814 RVA: 0x0004E05A File Offset: 0x0004C45A
	Protected Overridable Sub OnPlayerJoined(playerId As PlayerId)
		LevelNewPlayerGUI.Current.Init()
		Me.CreatePlayerTwoOnJoin()
		Me.SetRichPresence()
	End Sub

	' Token: 0x060012CF RID: 4815 RVA: 0x0004E074 File Offset: 0x0004C474
	Private Sub OnPlayerLeave(playerId As PlayerId)
		If playerId = PlayerId.PlayerTwo Then
			Dim player As AbstractPlayerController = PlayerManager.GetPlayer(playerId)
			If player IsNot Nothing Then
				player.OnLeave(playerId)
			End If
			If PlayerManager.GetPlayer(PlayerId.PlayerOne).IsDead Then
				Me._OnLose()
			End If
		End If
	End Sub

	' Token: 0x060012D0 RID: 4816 RVA: 0x0004E0B8 File Offset: 0x0004C4B8
	Private Sub SetRichPresence()
		If Me.CurrentLevel = Levels.Mausoleum Then
			OnlineManager.Instance.[Interface].SetRichPresence(PlayerId.Any, "Mausoleum", True)
		ElseIf Me.CurrentLevel = Levels.Tutorial OrElse Me.CurrentLevel = Levels.House Then
			OnlineManager.Instance.[Interface].SetRichPresence(PlayerId.Any, "Tutorial", True)
		ElseIf Me.CurrentLevel = Levels.ShmupTutorial Then
			OnlineManager.Instance.[Interface].SetRichPresence(PlayerId.Any, "Blueprint", True)
		Else
			Dim type As Level.Type = Me.type
			If type <> Level.Type.Battle Then
				If type = Level.Type.Platforming Then
					OnlineManager.Instance.[Interface].SetStat(PlayerId.Any, "PlatformingLevel", SceneLoader.SceneName)
					OnlineManager.Instance.[Interface].SetRichPresence(PlayerId.Any, "Playing", True)
				End If
			Else
				OnlineManager.Instance.[Interface].SetStat(PlayerId.Any, "Boss", SceneLoader.SceneName)
				OnlineManager.Instance.[Interface].SetRichPresence(PlayerId.Any, "Fighting", True)
			End If
		End If
	End Sub

	' Token: 0x060012D1 RID: 4817 RVA: 0x0004E1F0 File Offset: 0x0004C5F0
	Private Sub OnPlayerDeath(e As PlayerStatsManager.DeathEvent)
		If Me.timeline IsNot Nothing AndAlso Me.LevelType <> Level.Type.Platforming Then
			Me.timeline.OnPlayerDeath(e.playerId)
		End If
		Me.playerIsDead = True
	End Sub

	' Token: 0x060012D2 RID: 4818 RVA: 0x0004E221 File Offset: 0x0004C621
	Private Sub OnPlayerRevive(e As PlayerStatsManager.ReviveEvent)
		Me.timeline.OnPlayerRevive(e.playerId)
	End Sub

	' Token: 0x060012D3 RID: 4819 RVA: 0x0004E234 File Offset: 0x0004C634
	Private Sub CheckPlayerHoldingButtons()
		If PlayerManager.GetPlayerInput(PlayerId.PlayerOne).GetButton(2) AndAlso Not Me.player1HeldJump Then
			Me.player1HeldJump = True
		End If
		If PlayerManager.GetPlayerInput(PlayerId.PlayerOne).GetButton(4) AndAlso Not Me.player1HeldSuper Then
			Me.player1HeldSuper = True
		End If
		If PlayerManager.Multiplayer Then
			If PlayerManager.GetPlayerInput(PlayerId.PlayerTwo).GetButton(2) AndAlso Not Me.player2HeldJump Then
				Me.player2HeldJump = True
			End If
			If PlayerManager.GetPlayerInput(PlayerId.PlayerTwo).GetButton(4) AndAlso Not Me.player2HeldSuper Then
				Me.player2HeldSuper = True
			End If
		End If
	End Sub

	' Token: 0x14000031 RID: 49
	' (add) Token: 0x060012D4 RID: 4820 RVA: 0x0004E2D8 File Offset: 0x0004C6D8
	' (remove) Token: 0x060012D5 RID: 4821 RVA: 0x0004E310 File Offset: 0x0004C710
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnLevelStartEvent As Action

	' Token: 0x14000032 RID: 50
	' (add) Token: 0x060012D6 RID: 4822 RVA: 0x0004E348 File Offset: 0x0004C748
	' (remove) Token: 0x060012D7 RID: 4823 RVA: 0x0004E380 File Offset: 0x0004C780
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnLevelEndEvent As Action

	' Token: 0x14000033 RID: 51
	' (add) Token: 0x060012D8 RID: 4824 RVA: 0x0004E3B8 File Offset: 0x0004C7B8
	' (remove) Token: 0x060012D9 RID: 4825 RVA: 0x0004E3F0 File Offset: 0x0004C7F0
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnPlatformingLevelAwakeEvent As Action

	' Token: 0x14000034 RID: 52
	' (add) Token: 0x060012DA RID: 4826 RVA: 0x0004E428 File Offset: 0x0004C828
	' (remove) Token: 0x060012DB RID: 4827 RVA: 0x0004E460 File Offset: 0x0004C860
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnStateChangedEvent As Action

	' Token: 0x14000035 RID: 53
	' (add) Token: 0x060012DC RID: 4828 RVA: 0x0004E498 File Offset: 0x0004C898
	' (remove) Token: 0x060012DD RID: 4829 RVA: 0x0004E4D0 File Offset: 0x0004C8D0
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnWinEvent As Action

	' Token: 0x14000036 RID: 54
	' (add) Token: 0x060012DE RID: 4830 RVA: 0x0004E508 File Offset: 0x0004C908
	' (remove) Token: 0x060012DF RID: 4831 RVA: 0x0004E540 File Offset: 0x0004C940
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnPreWinEvent As Action

	' Token: 0x14000037 RID: 55
	' (add) Token: 0x060012E0 RID: 4832 RVA: 0x0004E578 File Offset: 0x0004C978
	' (remove) Token: 0x060012E1 RID: 4833 RVA: 0x0004E5B0 File Offset: 0x0004C9B0
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnLoseEvent As Action

	' Token: 0x14000038 RID: 56
	' (add) Token: 0x060012E2 RID: 4834 RVA: 0x0004E5E8 File Offset: 0x0004C9E8
	' (remove) Token: 0x060012E3 RID: 4835 RVA: 0x0004E620 File Offset: 0x0004CA20
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnPreLoseEvent As Action

	' Token: 0x14000039 RID: 57
	' (add) Token: 0x060012E4 RID: 4836 RVA: 0x0004E658 File Offset: 0x0004CA58
	' (remove) Token: 0x060012E5 RID: 4837 RVA: 0x0004E690 File Offset: 0x0004CA90
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnTransitionInCompleteEvent As Action

	' Token: 0x1400003A RID: 58
	' (add) Token: 0x060012E6 RID: 4838 RVA: 0x0004E6C8 File Offset: 0x0004CAC8
	' (remove) Token: 0x060012E7 RID: 4839 RVA: 0x0004E700 File Offset: 0x0004CB00
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnIntroEvent As Action

	' Token: 0x1400003B RID: 59
	' (add) Token: 0x060012E8 RID: 4840 RVA: 0x0004E738 File Offset: 0x0004CB38
	' (remove) Token: 0x060012E9 RID: 4841 RVA: 0x0004E770 File Offset: 0x0004CB70
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnBossDeathExplosionsEvent As Action

	' Token: 0x1400003C RID: 60
	' (add) Token: 0x060012EA RID: 4842 RVA: 0x0004E7A8 File Offset: 0x0004CBA8
	' (remove) Token: 0x060012EB RID: 4843 RVA: 0x0004E7E0 File Offset: 0x0004CBE0
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnBossDeathExplosionsEndEvent As Action

	' Token: 0x1400003D RID: 61
	' (add) Token: 0x060012EC RID: 4844 RVA: 0x0004E818 File Offset: 0x0004CC18
	' (remove) Token: 0x060012ED RID: 4845 RVA: 0x0004E850 File Offset: 0x0004CC50
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnBossDeathExplosionsFalloffEvent As Action

	' Token: 0x060012EE RID: 4846 RVA: 0x0004E888 File Offset: 0x0004CC88
	Private Sub _OnLevelStart()
		Me.OnLevelStart()
		If Me.OnLevelStartEvent IsNot Nothing Then
			Me.OnLevelStartEvent()
		End If
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, True, True)
		InterruptingPrompt.SetCanInterrupt(True)
		Dim levelData As PlayerData.PlayerLevelDataObject = PlayerData.Data.GetLevelData(Me.CurrentLevel)
		If levelData IsNot Nothing AndAlso Not Level.IsTowerOfPower Then
			levelData.played = True
		End If
	End Sub

	' Token: 0x060012EF RID: 4847 RVA: 0x0004E8E7 File Offset: 0x0004CCE7
	Private Sub _OnLevelEnd()
		Me.Ending = True
		Me.OnLevelEnd()
		If Me.OnLevelEndEvent IsNot Nothing Then
			Me.OnLevelEndEvent()
		End If
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, False, False)
		PlayerManager.ClearJoinPrompt()
	End Sub

	' Token: 0x060012F0 RID: 4848 RVA: 0x0004E919 File Offset: 0x0004CD19
	Protected Sub zHack_OnStateChanged()
		Me.OnStateChanged()
		If Me.OnStateChangedEvent IsNot Nothing Then
			Me.OnStateChangedEvent()
		End If
	End Sub

	' Token: 0x060012F1 RID: 4849 RVA: 0x0004E938 File Offset: 0x0004CD38
	Protected Sub zHack_OnWin()
		PlayerManager.playerWasChalice(0) = PlayerManager.GetPlayer(PlayerId.PlayerOne).stats.isChalice
		PlayerManager.playerWasChalice(1) = PlayerManager.GetPlayer(PlayerId.PlayerTwo) IsNot Nothing AndAlso PlayerManager.GetPlayer(PlayerId.PlayerTwo).stats.isChalice
		Me.CheckPlayerCharacters()
		Level.Won = True
		Level.Difficulty = Me.mode
		Dim levelData As PlayerData.PlayerLevelDataObject = PlayerData.Data.GetLevelData(Me.CurrentLevel)
		Level.ScoringData.finalHP = PlayerManager.GetPlayer(PlayerId.PlayerOne).stats.Health
		If PlayerManager.GetPlayer(PlayerId.PlayerTwo) IsNot Nothing Then
			Level.ScoringData.finalHP = Mathf.Max(Level.ScoringData.finalHP, PlayerManager.GetPlayer(PlayerId.PlayerTwo).stats.Health)
		End If
		Level.ScoringData.finalHP = Mathf.Min(Level.ScoringData.finalHP, CInt(Cuphead.Current.ScoringProperties.hitsForNoScore))
		Level.ScoringData.usedDjimmi = PlayerData.Data.DjimmiActivatedCurrentRegion() AndAlso Me.AllowDjimmi() AndAlso Me.mode <> Level.Mode.Hard
		If Level.ScoringData.usedDjimmi AndAlso (Not Level.IsDicePalace OrElse Level.IsDicePalaceMain) Then
			PlayerData.Data.DeactivateDjimmi()
		End If
		If Not Level.IsTowerOfPower Then
			levelData.completed = True
			If PlayerData.Data.Loadouts.GetPlayerLoadout(PlayerId.PlayerOne).charm = Charm.charm_chalice Then
				levelData.completedAsChaliceP1 = True
			End If
			If PlayerData.Data.Loadouts.GetPlayerLoadout(PlayerId.PlayerTwo).charm = Charm.charm_chalice Then
				levelData.completedAsChaliceP2 = True
			End If
		End If
		Level.ScoringData.time += Me.LevelTime
		If(Me.type = Level.Type.Battle OrElse Me.type = Level.Type.Platforming) AndAlso (Not Level.IsDicePalace OrElse Level.IsDicePalaceMain) Then
			Level.Grade = Level.ScoringData.CalculateGrade()
			Dim time As Single = Level.ScoringData.time
			If Not Level.IsTowerOfPower Then
				If Level.Difficulty > Level.PreviousDifficulty OrElse Not Level.PreviouslyWon Then
					levelData.difficultyBeaten = Level.Difficulty
				End If
				If Level.Grade > Level.PreviousGrade OrElse Not Level.PreviouslyWon Then
					levelData.grade = Level.Grade
					levelData.bestTime = time
				ElseIf Level.Grade = Level.PreviousGrade AndAlso time < levelData.bestTime Then
					levelData.bestTime = time
				End If
				If Me.CurrentLevel = Levels.Devil Then
					PlayerData.Data.IsHardModeAvailable = True
				End If
				If Me.CurrentLevel = Levels.Saltbaker Then
					PlayerData.Data.IsHardModeAvailableDLC = True
				End If
			End If
		End If
		If Level.IsChessBoss Then
			If PlayerData.Data.currentChessBossZone <> MapCastleZones.Zone.None Then
				Dim currentChessBossZone As MapCastleZones.Zone = PlayerData.Data.currentChessBossZone
				PlayerData.Data.currentChessBossZone = MapCastleZones.Zone.None
				Dim usedChessBossZones As List(Of MapCastleZones.Zone) = PlayerData.Data.usedChessBossZones
				If Not usedChessBossZones.Contains(currentChessBossZone) Then
					usedChessBossZones.Add(currentChessBossZone)
				End If
			End If
			Dim array As String()
			If ChessCastleLevel.Coins.TryGetValue(Me.CurrentLevel, array) Then
				For Each text As String In array
					If Not PlayerData.Data.coinManager.GetCoinCollected(text) Then
						PlayerData.Data.coinManager.SetCoinValue(text, True, PlayerId.Any)
						PlayerData.Data.AddCurrency(PlayerId.PlayerOne, 1)
						PlayerData.Data.AddCurrency(PlayerId.PlayerTwo, 1)
					End If
				Next
			End If
		End If
		Dim player As AbstractPlayerController = PlayerManager.GetPlayer(PlayerId.PlayerOne)
		If Level.Difficulty <> Level.Mode.Easy AndAlso player IsNot Nothing AndAlso player.stats.Loadout.charm = Charm.charm_curse AndAlso CharmCurse.CalculateLevel(PlayerId.PlayerOne) >= 0 Then
			levelData.curseCharmP1 = True
		End If
		Dim player2 As AbstractPlayerController = PlayerManager.GetPlayer(PlayerId.PlayerTwo)
		If Level.Difficulty <> Level.Mode.Easy AndAlso player2 IsNot Nothing AndAlso player2.stats.Loadout.charm = Charm.charm_curse AndAlso CharmCurse.CalculateLevel(PlayerId.PlayerTwo) >= 0 Then
			levelData.curseCharmP2 = True
		End If
		Me._OnLevelEnd()
		Me._OnPreWin()
		If Me.LevelType = Level.Type.Battle Then
			MyBase.StartCoroutine(Me.bossDeath_cr())
		End If
		Me.OnWin()
		If Me.OnWinEvent IsNot Nothing Then
			Me.OnWinEvent()
		End If
		If Not Level.IsTowerOfPower Then
			PlayerData.SaveCurrentFile()
		End If
		If Not Level.IsTowerOfPower Then
			Dim array3 As Levels() = Nothing
			Dim array4 As Levels() = Nothing
			Dim text2 As String = Nothing
			Dim currentMap As Scenes = PlayerData.Data.CurrentMap
			Dim flag As Boolean = Array.Exists(Of Levels)(Level.kingOfGamesLevels, Function(level As Levels) Me.CurrentLevel = level)
			Select Case currentMap
				Case Scenes.scene_map_world_1
					Dim array5 As Levels() = Level.world1BossLevels
					array4 = array5
					array3 = array5
					text2 = "World1"
				Case Scenes.scene_map_world_2
					Dim array6 As Levels() = Level.world2BossLevels
					array4 = array6
					array3 = array6
					text2 = "World2"
				Case Scenes.scene_map_world_3
					Dim array7 As Levels() = Level.world3BossLevels
					array4 = array7
					array3 = array7
					text2 = "World3"
				Case Scenes.scene_map_world_4
					Dim array8 As Levels() = Level.world4BossLevels
					array4 = array8
					array3 = array8
					text2 = "World4"
				Case Else
					If currentMap = Scenes.scene_map_world_DLC Then
						array3 = Level.worldDLCBossLevels
						array4 = Level.worldDLCBossLevelsWithSaltbaker
						text2 = "WorldDLC"
					End If
			End Select
			If currentMap = Scenes.scene_map_world_4 Then
				If Me.CurrentLevel = Levels.DicePalaceMain Then
					OnlineManager.Instance.[Interface].UnlockAchievement(PlayerId.Any, "CompleteDicePalace")
				ElseIf Me.CurrentLevel = Levels.Devil Then
					OnlineManager.Instance.[Interface].UnlockAchievement(PlayerId.Any, "CompleteDevil")
				End If
			ElseIf Me.type = Level.Type.Battle AndAlso PlayerData.Data.CheckLevelsCompleted(array3) Then
				OnlineManager.Instance.[Interface].UnlockAchievement(PlayerId.Any, "Complete" + text2)
			End If
			If Me.CurrentLevel = Levels.Saltbaker Then
				OnlineManager.Instance.[Interface].UnlockAchievement(PlayerId.Any, OnlineAchievementData.DLC.DefeatSaltbaker)
			End If
			If Me.type = Level.Type.Battle AndAlso Level.Difficulty = Level.Mode.Hard AndAlso PlayerData.Data.CheckLevelsHaveMinDifficulty(Level.world1BossLevels, Level.Mode.Hard) AndAlso PlayerData.Data.CheckLevelsHaveMinDifficulty(Level.world2BossLevels, Level.Mode.Hard) AndAlso PlayerData.Data.CheckLevelsHaveMinDifficulty(Level.world3BossLevels, Level.Mode.Hard) AndAlso PlayerData.Data.CheckLevelsHaveMinDifficulty(Level.world4BossLevels, Level.Mode.Hard) Then
				OnlineManager.Instance.[Interface].UnlockAchievement(PlayerId.Any, "NewGamePlus")
			End If
			If Me.type = Level.Type.Battle AndAlso Level.Grade >= LevelScoringData.Grade.AMinus AndAlso PlayerData.Data.CheckLevelsHaveMinGrade(array4, LevelScoringData.Grade.AMinus) Then
				OnlineManager.Instance.[Interface].UnlockAchievement(PlayerId.Any, "ARank" + text2)
			End If
			If Me.type = Level.Type.Platforming AndAlso Not Me.isMausoleum AndAlso Level.ScoringData.pacifistRun AndAlso PlayerData.Data.CheckLevelsHaveMinGrade(Level.platformingLevels, LevelScoringData.Grade.P) Then
				OnlineManager.Instance.[Interface].UnlockAchievement(PlayerId.Any, "PacifistRun")
			End If
			If(Me.type = Level.Type.Battle OrElse Me.type = Level.Type.Platforming) AndAlso (Not Level.IsDicePalace OrElse Level.IsDicePalaceMain) AndAlso Not Me.isMausoleum AndAlso Not flag AndAlso Level.ScoringData.numTimesHit = 0 Then
				If Level.IsDicePalaceMain Then
					OnlineManager.Instance.[Interface].UnlockAchievement(PlayerId.Any, "NoHitsTakenDicePalace")
				End If
				OnlineManager.Instance.[Interface].UnlockAchievement(PlayerId.Any, "NoHitsTaken")
			End If
			If Me.type = Level.Type.Battle Then
				If DamageDealer.lastPlayerDamageSource = DamageDealer.DamageSource.Super Then
					OnlineManager.Instance.[Interface].UnlockAchievement(DamageDealer.lastPlayer, "SuperWin")
					Dim abstractPlayerController As AbstractPlayerController = If((DamageDealer.lastPlayer <> PlayerId.PlayerOne), player2, player)
					If abstractPlayerController IsNot Nothing AndAlso abstractPlayerController.stats.isChalice Then
						OnlineManager.Instance.[Interface].UnlockAchievement(DamageDealer.lastPlayer, OnlineAchievementData.DLC.ChaliceSuperWin)
					End If
				End If
				If DamageDealer.lastPlayerDamageSource = DamageDealer.DamageSource.Ex Then
					OnlineManager.Instance.[Interface].UnlockAchievement(DamageDealer.lastPlayer, "ExWin")
				End If
				If Me.playerMode = PlayerMode.Plane AndAlso Not DamageDealer.didDamageWithNonSmallPlaneWeapon Then
					OnlineManager.Instance.[Interface].UnlockAchievement(PlayerId.Any, "SmallPlaneOnlyWin")
				End If
				If DamageDealer.lastDamageWasDLCWeapon Then
					OnlineManager.Instance.[Interface].UnlockAchievement(DamageDealer.lastPlayer, OnlineAchievementData.DLC.DefeatBossDLCWeapon)
				End If
				If player IsNot Nothing AndAlso player.stats.Loadout.charm = Charm.charm_curse AndAlso CharmCurse.IsMaxLevel(PlayerId.PlayerOne) Then
					OnlineManager.Instance.[Interface].UnlockAchievement(PlayerId.PlayerOne, OnlineAchievementData.DLC.Paladin)
				End If
				If player2 IsNot Nothing AndAlso player2.stats.Loadout.charm = Charm.charm_curse AndAlso CharmCurse.IsMaxLevel(PlayerId.PlayerTwo) Then
					OnlineManager.Instance.[Interface].UnlockAchievement(PlayerId.PlayerTwo, OnlineAchievementData.DLC.Paladin)
				End If
			End If
			Dim num As Integer = 0
			Dim num2 As Integer = 0
			Dim list As List(Of Levels) = New List(Of Levels)(Level.world1BossLevels)
			list.AddRange(Level.world2BossLevels)
			list.AddRange(Level.world3BossLevels)
			For Each levels As Levels In list
				Dim levelData2 As PlayerData.PlayerLevelDataObject = PlayerData.Data.GetLevelData(levels)
				If levelData2.completed AndAlso levelData2.difficultyBeaten >= Level.Mode.Normal Then
					num2 += 1
				End If
			Next
			Dim list2 As List(Of Levels) = New List(Of Levels)(Level.world1BossLevels)
			list2.AddRange(Level.world2BossLevels)
			list2.AddRange(Level.world3BossLevels)
			list2.AddRange(Level.world4BossLevels)
			list2.AddRange(Level.platformingLevels)
			For Each levels2 As Levels In list2
				Dim levelData3 As PlayerData.PlayerLevelDataObject = PlayerData.Data.GetLevelData(levels2)
				If levelData3.completed AndAlso levelData3.grade >= LevelScoringData.Grade.AMinus Then
					num += 1
				End If
			Next
			If Me.type = Level.Type.Battle AndAlso Me.CurrentLevel <> Levels.Mausoleum Then
				OnlineManager.Instance.[Interface].UnlockAchievement(PlayerId.Any, "DefeatBoss")
			End If
			If Me.type = Level.Type.Battle AndAlso Array.Exists(Of Levels)(Level.chaliceLevels, Function(level As Levels) Me.CurrentLevel = level) Then
				Dim flag2 As Boolean = False
				If player IsNot Nothing AndAlso player.stats.isChalice Then
					flag2 = True
					OnlineManager.Instance.[Interface].UnlockAchievement(PlayerId.PlayerOne, OnlineAchievementData.DLC.DefeatBossAsChalice)
				End If
				If player2 IsNot Nothing AndAlso player2.stats.isChalice Then
					flag2 = True
					OnlineManager.Instance.[Interface].UnlockAchievement(PlayerId.PlayerTwo, OnlineAchievementData.DLC.DefeatBossAsChalice)
				End If
				If flag2 Then
					If PlayerData.Data.CountLevelsChaliceCompleted(Level.chaliceLevels, PlayerId.PlayerOne) >= OnlineAchievementData.DLC.Triggers.DefeatXBossesAsChaliceTrigger Then
						OnlineManager.Instance.[Interface].UnlockAchievement(PlayerId.PlayerOne, OnlineAchievementData.DLC.DefeatXBossesAsChalice)
					End If
					If PlayerData.Data.CountLevelsChaliceCompleted(Level.chaliceLevels, PlayerId.PlayerTwo) >= OnlineAchievementData.DLC.Triggers.DefeatXBossesAsChaliceTrigger Then
						OnlineManager.Instance.[Interface].UnlockAchievement(PlayerId.PlayerTwo, OnlineAchievementData.DLC.DefeatXBossesAsChalice)
					End If
				End If
			End If
			If Me.type = Level.Type.Battle AndAlso Me.CurrentLevel = Levels.Graveyard Then
				OnlineManager.Instance.[Interface].UnlockAchievement(PlayerId.Any, OnlineAchievementData.DLC.DefeatDevilPhase2)
			End If
			If Level.Grade = LevelScoringData.Grade.S AndAlso Me.CurrentLevel <> Levels.Mausoleum AndAlso Not flag Then
				OnlineManager.Instance.[Interface].UnlockAchievement(PlayerId.Any, "SRank")
				If Array.Exists(Of Levels)(Level.worldDLCBossLevelsWithSaltbaker, Function(level As Levels) Me.CurrentLevel = level) Then
					OnlineManager.Instance.[Interface].UnlockAchievement(PlayerId.Any, OnlineAchievementData.DLC.SRankAnyDLC)
				End If
			End If
			If Array.Exists(Of Levels)(Level.worldDLCBossLevels, Function(level As Levels) Me.CurrentLevel = level) AndAlso Not Me.defeatedMinion Then
				OnlineManager.Instance.[Interface].UnlockAchievement(PlayerId.Any, OnlineAchievementData.DLC.DefeatBossNoMinions)
			End If
			If flag AndAlso PlayerData.Data.CheckLevelsCompleted(Level.kingOfGamesLevels) Then
				OnlineManager.Instance.[Interface].UnlockAchievement(PlayerId.Any, OnlineAchievementData.DLC.DefeatAllKOG)
			End If
			OnlineManager.Instance.[Interface].SetStat(PlayerId.Any, "ARanks", num)
			OnlineManager.Instance.[Interface].SetStat(PlayerId.Any, "BossesDefeatedNormal", num2)
			OnlineManager.Instance.[Interface].SyncAchievementsAndStats()
		End If
		If Not Me.isMausoleum Then
			InterruptingPrompt.SetCanInterrupt(False)
		End If
	End Sub

	' Token: 0x060012F2 RID: 4850 RVA: 0x0004F660 File Offset: 0x0004DA60
	Private Sub _OnPreWin()
		Me.OnPreWin()
		If Me.OnPreWinEvent IsNot Nothing Then
			Me.OnPreWinEvent()
		End If
	End Sub

	' Token: 0x060012F3 RID: 4851 RVA: 0x0004F680 File Offset: 0x0004DA80
	Protected Sub _OnLose()
		Me._OnLevelEnd()
		Me._OnPreLose()
		Me.OnLose()
		If Me.OnLoseEvent IsNot Nothing Then
			Me.OnLoseEvent()
		End If
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, False, False)
		LevelEnd.Lose(Me.isMausoleum, Me.secretTriggered)
		If Not Level.IsTowerOfPower Then
			PlayerData.SaveCurrentFile()
		End If
	End Sub

	' Token: 0x060012F4 RID: 4852 RVA: 0x0004F6DD File Offset: 0x0004DADD
	Private Sub _OnPreLose()
		Me.OnPreLose()
		If Me.OnPreLoseEvent IsNot Nothing Then
			Me.OnPreLoseEvent()
		End If
	End Sub

	' Token: 0x060012F5 RID: 4853 RVA: 0x0004F6FB File Offset: 0x0004DAFB
	Private Sub _OnTransitionInComplete()
		Me.OnTransitionInComplete()
		If Me.OnTransitionInCompleteEvent IsNot Nothing Then
			Me.OnTransitionInCompleteEvent()
		End If
	End Sub

	' Token: 0x060012F6 RID: 4854 RVA: 0x0004F719 File Offset: 0x0004DB19
	Private Sub OnStartExplosions()
		If Me.OnBossDeathExplosionsEvent IsNot Nothing Then
			Me.OnBossDeathExplosionsEvent()
		End If
	End Sub

	' Token: 0x060012F7 RID: 4855 RVA: 0x0004F731 File Offset: 0x0004DB31
	Private Sub OnEndExplosions()
		If Me.OnBossDeathExplosionsEndEvent IsNot Nothing Then
			Me.OnBossDeathExplosionsEndEvent()
		End If
	End Sub

	' Token: 0x060012F8 RID: 4856 RVA: 0x0004F749 File Offset: 0x0004DB49
	Private Sub OnFalloffExplosions()
		If Me.OnBossDeathExplosionsFalloffEvent IsNot Nothing Then
			Me.OnBossDeathExplosionsFalloffEvent()
		End If
	End Sub

	' Token: 0x170002FE RID: 766
	' (get) Token: 0x060012F9 RID: 4857 RVA: 0x0004F761 File Offset: 0x0004DB61
	Protected Overridable ReadOnly Property LevelIntroTime As Single
		Get
			Return 1F
		End Get
	End Property

	' Token: 0x170002FF RID: 767
	' (get) Token: 0x060012FA RID: 4858 RVA: 0x0004F768 File Offset: 0x0004DB68
	Protected Overridable ReadOnly Property BossDeathTime As Single
		Get
			Return 2F
		End Get
	End Property

	' Token: 0x060012FB RID: 4859 RVA: 0x0004F76F File Offset: 0x0004DB6F
	Protected Overridable Sub PlayAnnouncerReady()
		If Not Me.isMausoleum Then
			AudioManager.Play("level_announcer_ready")
		Else
			AudioManager.Play("level_announcer_opening_line")
		End If
	End Sub

	' Token: 0x060012FC RID: 4860 RVA: 0x0004F795 File Offset: 0x0004DB95
	Protected Overridable Sub PlayAnnouncerBegin()
		AudioManager.Play("level_announcer_begin")
	End Sub

	' Token: 0x060012FD RID: 4861 RVA: 0x0004F7A1 File Offset: 0x0004DBA1
	Protected Overridable Function CreateLevelIntro(callback As Action) As LevelIntroAnimation
		Return LevelIntroAnimation.Create(callback)
	End Function

	' Token: 0x060012FE RID: 4862 RVA: 0x0004F7A9 File Offset: 0x0004DBA9
	Protected Overridable Sub OnLevelStart()
	End Sub

	' Token: 0x060012FF RID: 4863 RVA: 0x0004F7AB File Offset: 0x0004DBAB
	Protected Overridable Sub OnStateChanged()
	End Sub

	' Token: 0x06001300 RID: 4864 RVA: 0x0004F7AD File Offset: 0x0004DBAD
	Protected Overridable Sub OnWin()
	End Sub

	' Token: 0x06001301 RID: 4865 RVA: 0x0004F7AF File Offset: 0x0004DBAF
	Protected Overridable Sub OnPreWin()
	End Sub

	' Token: 0x06001302 RID: 4866 RVA: 0x0004F7B1 File Offset: 0x0004DBB1
	Protected Overridable Sub OnLose()
	End Sub

	' Token: 0x06001303 RID: 4867 RVA: 0x0004F7B3 File Offset: 0x0004DBB3
	Protected Overridable Sub OnPreLose()
	End Sub

	' Token: 0x06001304 RID: 4868 RVA: 0x0004F7B5 File Offset: 0x0004DBB5
	Protected Overridable Sub OnTransitionInComplete()
	End Sub

	' Token: 0x06001305 RID: 4869 RVA: 0x0004F7B8 File Offset: 0x0004DBB8
	Protected Overridable Iterator Function knockoutSFX_cr() As IEnumerator
		If Not Me.isMausoleum Then
			AudioManager.Play("level_announcer_knockout_bell")
			AudioManager.Play("level_announcer_knockout")
			Yield CupheadTime.WaitForSeconds(Me, 1.4F)
			If Not Level.IsChessBoss AndAlso Me.CurrentLevel <> Levels.Saltbaker AndAlso Me.CurrentLevel <> Levels.Graveyard Then
				AudioManager.Play("level_boss_defeat_sting")
			End If
		Else
			AudioManager.Play("level_announcer_victory")
		End If
		Return
	End Function

	' Token: 0x06001306 RID: 4870 RVA: 0x0004F7D3 File Offset: 0x0004DBD3
	Protected Overridable Sub OnBossDeath()
	End Sub

	' Token: 0x06001307 RID: 4871 RVA: 0x0004F7D8 File Offset: 0x0004DBD8
	Private Iterator Function check_intros_cr() As IEnumerator
		Yield New WaitForSeconds(0.25F)
		Me.CheckIntros()
		Yield Nothing
		Return
	End Function

	' Token: 0x06001308 RID: 4872 RVA: 0x0004F7F4 File Offset: 0x0004DBF4
	Protected Overridable Iterator Function startBattle_cr() As IEnumerator
		Dim introAnim As LevelIntroAnimation = Me.CreateLevelIntro(AddressOf Me.intro.OnReadyAnimComplete)
		Yield New WaitForSeconds(0.4F + SceneLoader.EndTransitionDelay)
		If Not Level.IsDicePalaceMain AndAlso Not Level.IsTowerOfPowerMain Then
			Me.PlayAnnouncerReady()
			AudioManager.Play("level_bell_intro")
		End If
		Yield New WaitForSeconds(0.25F)
		If Me.players(0) IsNot Nothing Then
			Me.players(0).PlayIntro()
		End If
		If Me.players(1) IsNot Nothing Then
			If Not Me.players(1).stats.isChalice Then
				Yield CupheadTime.WaitForSeconds(Me, 0.7F)
			End If
			Me.players(1).PlayIntro()
		End If
		Yield New WaitForSeconds(0.25F)
		Me._OnTransitionInComplete()
		If Me.OnIntroEvent IsNot Nothing Then
			Me.OnIntroEvent()
		End If
		Me.OnIntroEvent = Nothing
		Yield New WaitForSeconds(Me.LevelIntroTime)
		If Not Level.IsDicePalaceMain AndAlso Not Level.IsTowerOfPowerMain Then
			introAnim.Play()
			While Not Me.intro.readyComplete
				Yield Nothing
			End While
			Me.PlayAnnouncerBegin()
		ElseIf Not Level.IsTowerOfPowerMain Then
			Yield CupheadTime.WaitForSeconds(Me, 1.5F)
		End If
		For Each abstractPlayerController As AbstractPlayerController In Me.players
			If Not(abstractPlayerController Is Nothing) Then
				abstractPlayerController.LevelStart()
			End If
		Next
		Me.Started = True
		Me._OnLevelStart()
		Return
	End Function

	' Token: 0x06001309 RID: 4873 RVA: 0x0004F810 File Offset: 0x0004DC10
	Protected Overridable Iterator Function startPlatforming_cr() As IEnumerator
		Dim introAnim As PlatformingLevelIntroAnimation = PlatformingLevelIntroAnimation.Create(AddressOf Me.intro.OnReadyAnimComplete)
		Yield New WaitForEndOfFrame()
		If Me.players(0) IsNot Nothing Then
			Me.players(0).OnPlatformingLevelAwake()
		End If
		If Me.players(1) IsNot Nothing Then
			Me.players(1).OnPlatformingLevelAwake()
		End If
		Yield New WaitForSeconds(0.4F + SceneLoader.EndTransitionDelay)
		Me._OnTransitionInComplete()
		If Me.OnIntroEvent IsNot Nothing Then
			Me.OnIntroEvent()
		End If
		Me.OnIntroEvent = Nothing
		introAnim.Play()
		AudioManager.Play("level_announcer_begin")
		While Not Me.intro.readyComplete
			Yield Nothing
		End While
		For Each abstractPlayerController As AbstractPlayerController In Me.players
			If Not(abstractPlayerController Is Nothing) Then
				abstractPlayerController.LevelStart()
			End If
		Next
		Me.Started = True
		Me._OnLevelStart()
		Return
	End Function

	' Token: 0x0600130A RID: 4874 RVA: 0x0004F82C File Offset: 0x0004DC2C
	Protected Overridable Iterator Function startNonBattle_cr() As IEnumerator
		Yield New WaitForSeconds(0.4F + SceneLoader.EndTransitionDelay - 0.25F)
		If Me.playerMode = PlayerMode.Plane Then
			Yield New WaitForSeconds(0.5F)
			If Me.players(0) IsNot Nothing Then
				Me.players(0).PlayIntro()
			End If
			If Me.players(1) IsNot Nothing Then
				Yield CupheadTime.WaitForSeconds(Me, 0.7F)
				Me.players(1).PlayIntro()
			End If
			Yield New WaitForSeconds(0.25F)
		End If
		Me._OnTransitionInComplete()
		If Me.OnIntroEvent IsNot Nothing Then
			Me.OnIntroEvent()
		End If
		Me.OnIntroEvent = Nothing
		If Me.playerMode = PlayerMode.Plane Then
			Yield New WaitForSeconds(1.25F)
		End If
		For Each abstractPlayerController As AbstractPlayerController In Me.players
			If Not(abstractPlayerController Is Nothing) Then
				abstractPlayerController.LevelStart()
			End If
		Next
		Me.Started = True
		Me._OnLevelStart()
		Return
	End Function

	' Token: 0x0600130B RID: 4875 RVA: 0x0004F848 File Offset: 0x0004DC48
	Protected Overridable Iterator Function bossDeath_cr() As IEnumerator
		LevelEnd.Win(Me.knockoutSFX_cr(), AddressOf Me.OnBossDeath, AddressOf Me.OnStartExplosions, AddressOf Me.OnFalloffExplosions, AddressOf Me.OnEndExplosions, Me.players, Me.BossDeathTime, (Me.type = Level.Type.Battle OrElse Me.type = Level.Type.Platforming) AndAlso (Not Level.IsDicePalace OrElse Level.IsDicePalaceMain) AndAlso Not Level.IsTowerOfPower AndAlso Not Me.isMausoleum AndAlso Not Level.IsGraveyard AndAlso Not Level.IsChessBoss, Me.isMausoleum, Me.isDevil, Me.isTowerOfPower)
		Yield Nothing
		Return
	End Function

	' Token: 0x04001BD5 RID: 7125
	Private Const BOUND_COLLIDER_SIZE As Integer = 400

	' Token: 0x04001BD6 RID: 7126
	Private Const IRIS_NO_INTRO_DELAY As Single = 0.4F

	' Token: 0x04001BD7 RID: 7127
	Private Const IRIS_OPEN_DELAY As Single = 1F

	' Token: 0x04001BD8 RID: 7128
	Private Const PLAYER_DEATH_DELAY As Integer = 5

	' Token: 0x04001BD9 RID: 7129
	Public Const GENERIC_STATE_NAME As String = "Generic"

	' Token: 0x04001BED RID: 7149
	Public Shared world1BossLevels As Levels() = New Levels() { Levels.Veggies, Levels.Slime, Levels.FlyingBlimp, Levels.Flower, Levels.Frogs }

	' Token: 0x04001BEE RID: 7150
	Public Shared world2BossLevels As Levels() = New Levels() { Levels.Baroness, Levels.Clown, Levels.FlyingGenie, Levels.Dragon, Levels.FlyingBird }

	' Token: 0x04001BEF RID: 7151
	Public Shared world3BossLevels As Levels() = New Levels() { Levels.Bee, Levels.Pirate, Levels.SallyStagePlay, Levels.Mouse, Levels.Robot, Levels.FlyingMermaid, Levels.Train }

	' Token: 0x04001BF0 RID: 7152
	Public Shared world4BossLevels As Levels() = New Levels() { Levels.DicePalaceMain, Levels.Devil }

	' Token: 0x04001BF1 RID: 7153
	Public Shared world4MiniBossLevels As Levels() = New Levels() { Levels.DicePalaceBooze, Levels.DicePalaceChips, Levels.DicePalaceCigar, Levels.DicePalaceDomino, Levels.DicePalaceEightBall, Levels.DicePalaceFlyingHorse, Levels.DicePalaceFlyingMemory, Levels.DicePalaceRabbit, Levels.DicePalaceRoulette }

	' Token: 0x04001BF2 RID: 7154
	Public Shared worldDLCBossLevels As Levels() = New Levels() { Levels.Airplane, Levels.FlyingCowboy, Levels.OldMan, Levels.RumRunners, Levels.SnowCult }

	' Token: 0x04001BF3 RID: 7155
	Public Shared worldDLCBossLevelsWithSaltbaker As Levels() = New Levels() { Levels.Airplane, Levels.FlyingCowboy, Levels.OldMan, Levels.RumRunners, Levels.SnowCult, Levels.Saltbaker }

	' Token: 0x04001BF4 RID: 7156
	Public Shared platformingLevels As Levels() = New Levels() { Levels.Platforming_Level_1_1, Levels.Platforming_Level_1_2, Levels.Platforming_Level_2_1, Levels.Platforming_Level_2_2, Levels.Platforming_Level_3_1, Levels.Platforming_Level_3_2 }

	' Token: 0x04001BF5 RID: 7157
	Public Shared kingOfGamesLevels As Levels() = New Levels() { Levels.ChessPawn, Levels.ChessKnight, Levels.ChessBishop, Levels.ChessRook, Levels.ChessQueen }

	' Token: 0x04001BF6 RID: 7158
	Public Shared kingOfGamesLevelsWithCastle As Levels() = New Levels() { Levels.ChessPawn, Levels.ChessKnight, Levels.ChessBishop, Levels.ChessRook, Levels.ChessQueen, Levels.ChessCastle }

	' Token: 0x04001BF7 RID: 7159
	Public Shared chaliceLevels As Levels() = New Levels() { Levels.Veggies, Levels.Slime, Levels.FlyingBlimp, Levels.Flower, Levels.Frogs, Levels.Baroness, Levels.Clown, Levels.FlyingGenie, Levels.Dragon, Levels.FlyingBird, Levels.Bee, Levels.Pirate, Levels.SallyStagePlay, Levels.Mouse, Levels.Robot, Levels.FlyingMermaid, Levels.Train, Levels.DicePalaceMain, Levels.Devil, Levels.Airplane, Levels.FlyingCowboy, Levels.OldMan, Levels.RumRunners, Levels.SnowCult, Levels.Saltbaker }

	' Token: 0x04001BF8 RID: 7160
	Public LevelResources As LevelResources

	' Token: 0x04001BF9 RID: 7161
	<SerializeField()>
	Protected type As Level.Type

	' Token: 0x04001BFA RID: 7162
	<SerializeField()>
	Public playerMode As PlayerMode

	' Token: 0x04001BFB RID: 7163
	<SerializeField()>
	Protected allowMultiplayer As Boolean = True

	' Token: 0x04001BFC RID: 7164
	<SerializeField()>
	Public blockChalice As Boolean

	' Token: 0x04001BFE RID: 7166
	<SerializeField()>
	Protected intro As Level.IntroProperties

	' Token: 0x04001BFF RID: 7167
	<SerializeField()>
	Protected spawns As Level.Spawns

	' Token: 0x04001C00 RID: 7168
	<SerializeField()>
	Protected bounds As Level.Bounds = New Level.Bounds(640, 640, 360, 200)

	' Token: 0x04001C01 RID: 7169
	Public playerShadowSortingOrder As Integer

	' Token: 0x04001C02 RID: 7170
	<SerializeField()>
	Protected camera As Level.Camera = New Level.Camera(CupheadLevelCamera.Mode.Lerp, 640, 640, 360, 360)

	' Token: 0x04001C03 RID: 7171
	Protected gui As LevelGUI

	' Token: 0x04001C04 RID: 7172
	Protected hud As LevelHUD

	' Token: 0x04001C05 RID: 7173
	Protected players As AbstractPlayerController()

	' Token: 0x04001C06 RID: 7174
	Protected collidersRoot As Transform

	' Token: 0x04001C07 RID: 7175
	Protected goalTimes As Level.GoalTimes

	' Token: 0x04001C08 RID: 7176
	Protected waitingForPlayerJoin As Boolean

	' Token: 0x04001C09 RID: 7177
	Protected isMausoleum As Boolean

	' Token: 0x04001C0A RID: 7178
	Protected isDevil As Boolean

	' Token: 0x04001C0B RID: 7179
	Protected isTowerOfPower As Boolean

	' Token: 0x04001C0C RID: 7180
	Protected secretTriggered As Boolean

	' Token: 0x04001C16 RID: 7190
	Public BGMPlaylistCurrent As Integer

	' Token: 0x04001C17 RID: 7191
	Private player1PlaneSpawnPos As Vector3 = New Vector3(-550F, 74.3F)

	' Token: 0x04001C18 RID: 7192
	Private player2PlaneSpawnPos As Vector3 = New Vector3(-450F, -79.8F)

	' Token: 0x04001C19 RID: 7193
	Private playerDeathDelayFrames As Integer

	' Token: 0x04001C1A RID: 7194
	Private playerIsDead As Boolean

	' Token: 0x04001C1B RID: 7195
	Private player1HeldJump As Boolean

	' Token: 0x04001C1C RID: 7196
	Private player2HeldJump As Boolean

	' Token: 0x04001C1D RID: 7197
	Private player1HeldSuper As Boolean

	' Token: 0x04001C1E RID: 7198
	Private player2HeldSuper As Boolean

	' Token: 0x02000492 RID: 1170
	Public Enum Type
		' Token: 0x04001C2D RID: 7213
		Battle
		' Token: 0x04001C2E RID: 7214
		Tutorial
		' Token: 0x04001C2F RID: 7215
		Platforming
	End Enum

	' Token: 0x02000493 RID: 1171
	Public Enum Mode
		' Token: 0x04001C31 RID: 7217
		Easy
		' Token: 0x04001C32 RID: 7218
		Normal
		' Token: 0x04001C33 RID: 7219
		Hard
	End Enum

	' Token: 0x02000494 RID: 1172
	<Serializable()>
	Public Class Bounds
		' Token: 0x06001310 RID: 4880 RVA: 0x0004F890 File Offset: 0x0004DC90
		Public Sub New()
			Me.left = 0
			Me.right = 0
			Me.top = 0
			Me.bottom = 0
		End Sub

		' Token: 0x06001311 RID: 4881 RVA: 0x0004F8E8 File Offset: 0x0004DCE8
		Public Sub New(left As Integer, right As Integer, top As Integer, bottom As Integer)
			Me.left = left
			Me.right = right
			Me.top = top
			Me.bottom = bottom
		End Sub

		' Token: 0x06001312 RID: 4882 RVA: 0x0004F940 File Offset: 0x0004DD40
		Public Sub SetColliderPositions()
			Dim rect As Rect = Nothing
			rect.xMin = CSng((-CSng(Me.left)))
			rect.xMax = CSng(Me.right)
			rect.yMin = CSng((-CSng(Me.bottom)))
			rect.yMax = CSng(Me.top)
			If Me.colliders.ContainsKey(Level.Bounds.Side.Left) AndAlso Me.colliders(Level.Bounds.Side.Left) IsNot Nothing Then
				Me.colliders(Level.Bounds.Side.Left).transform.position = New Vector2(CSng((-CSng(Me.left) - 200)), rect.center.y)
			End If
			If Me.colliders.ContainsKey(Level.Bounds.Side.Right) AndAlso Me.colliders(Level.Bounds.Side.Right) IsNot Nothing Then
				Me.colliders(Level.Bounds.Side.Right).transform.position = New Vector2(CSng((Me.right + 200)), rect.center.y)
			End If
			If Me.colliders.ContainsKey(Level.Bounds.Side.Top) AndAlso Me.colliders(Level.Bounds.Side.Top) IsNot Nothing Then
				Me.colliders(Level.Bounds.Side.Top).transform.position = New Vector2(rect.center.x, CSng((Me.top + 200)))
			End If
			If Me.colliders.ContainsKey(Level.Bounds.Side.Bottom) AndAlso Me.colliders(Level.Bounds.Side.Bottom) IsNot Nothing Then
				Me.colliders(Level.Bounds.Side.Bottom).transform.position = New Vector2(rect.center.x, CSng((-CSng(Me.bottom) - 200)))
			End If
		End Sub

		' Token: 0x06001313 RID: 4883 RVA: 0x0004FB22 File Offset: 0x0004DF22
		Public Function GetValue(side As Level.Bounds.Side) As Integer
			Select Case side
				Case Level.Bounds.Side.Left
					Return Me.left
				Case Level.Bounds.Side.Right
					Return Me.right
				Case Level.Bounds.Side.Top
					Return Me.top
				Case Else
					Return Me.bottom
			End Select
		End Function

		' Token: 0x06001314 RID: 4884 RVA: 0x0004FB5C File Offset: 0x0004DF5C
		Public Sub SetValue(side As Level.Bounds.Side, value As Integer)
			Select Case side
				Case Level.Bounds.Side.Left
					Me.left = value
				Case Level.Bounds.Side.Right
					Me.right = value
				Case Level.Bounds.Side.Top
					Me.top = value
				Case Else
					Me.bottom = value
			End Select
		End Sub

		' Token: 0x06001315 RID: 4885 RVA: 0x0004FBB4 File Offset: 0x0004DFB4
		Public Function GetEnabled(side As Level.Bounds.Side) As Boolean
			Select Case side
				Case Level.Bounds.Side.Left
					Return Me.leftEnabled
				Case Level.Bounds.Side.Right
					Return Me.rightEnabled
				Case Level.Bounds.Side.Top
					Return Me.topEnabled
				Case Else
					Return Me.bottomEnabled
			End Select
		End Function

		' Token: 0x06001316 RID: 4886 RVA: 0x0004FBEC File Offset: 0x0004DFEC
		Public Sub SetEnabled(side As Level.Bounds.Side, value As Boolean)
			Select Case side
				Case Level.Bounds.Side.Left
					Me.leftEnabled = value
				Case Level.Bounds.Side.Right
					Me.rightEnabled = value
				Case Level.Bounds.Side.Top
					Me.topEnabled = value
				Case Else
					Me.bottomEnabled = value
			End Select
		End Sub

		' Token: 0x06001317 RID: 4887 RVA: 0x0004FC44 File Offset: 0x0004E044
		Public Function Copy() As Level.Bounds
			Return TryCast(MyBase.MemberwiseClone(), Level.Bounds)
		End Function

		' Token: 0x17000300 RID: 768
		' (get) Token: 0x06001318 RID: 4888 RVA: 0x0004FC51 File Offset: 0x0004E051
		Public ReadOnly Property Width As Integer
			Get
				Return Me.left + Me.right
			End Get
		End Property

		' Token: 0x17000301 RID: 769
		' (get) Token: 0x06001319 RID: 4889 RVA: 0x0004FC60 File Offset: 0x0004E060
		Public ReadOnly Property Height As Integer
			Get
				Return Me.top + Me.bottom
			End Get
		End Property

		' Token: 0x17000302 RID: 770
		' (get) Token: 0x0600131A RID: 4890 RVA: 0x0004FC70 File Offset: 0x0004E070
		Public ReadOnly Property Center As Vector2
			Get
				Return New Vector2(CSng((Me.right - Me.left)), CSng((Me.top - Me.bottom))) / 2F
			End Get
		End Property

		' Token: 0x04001C34 RID: 7220
		Public left As Integer

		' Token: 0x04001C35 RID: 7221
		Public right As Integer

		' Token: 0x04001C36 RID: 7222
		Public top As Integer

		' Token: 0x04001C37 RID: 7223
		Public bottom As Integer

		' Token: 0x04001C38 RID: 7224
		Public topEnabled As Boolean = True

		' Token: 0x04001C39 RID: 7225
		Public bottomEnabled As Boolean = True

		' Token: 0x04001C3A RID: 7226
		Public leftEnabled As Boolean = True

		' Token: 0x04001C3B RID: 7227
		Public rightEnabled As Boolean = True

		' Token: 0x04001C3C RID: 7228
		Public colliders As Dictionary(Of Level.Bounds.Side, BoxCollider2D) = New Dictionary(Of Level.Bounds.Side, BoxCollider2D)()

		' Token: 0x02000495 RID: 1173
		Public Enum Side
			' Token: 0x04001C3E RID: 7230
			Left
			' Token: 0x04001C3F RID: 7231
			Right
			' Token: 0x04001C40 RID: 7232
			Top
			' Token: 0x04001C41 RID: 7233
			Bottom
		End Enum
	End Class

	' Token: 0x02000496 RID: 1174
	<Serializable()>
	Public Class Spawns
		' Token: 0x17000303 RID: 771
		Public ReadOnly Default Property Item(i As Integer) As Vector2
			Get
				If i = 0 Then
					Return Me.playerOne
				End If
				If i = 1 Then
					Return Me.playerTwo
				End If
				If i = 2 Then
					Return Me.playerOneSingle
				End If
				Global.Debug.LogError("Spawn index '" + i + "' not in range", Nothing)
				Return Vector2.zero
			End Get
		End Property

		' Token: 0x04001C42 RID: 7234
		Public playerOne As Vector2 = New Vector2(-460F, 0F)

		' Token: 0x04001C43 RID: 7235
		Public playerTwo As Vector2 = New Vector2(-580F, 0F)

		' Token: 0x04001C44 RID: 7236
		Public playerOneSingle As Vector2 = New Vector2(-520F, 0F)
	End Class

	' Token: 0x02000497 RID: 1175
	<Serializable()>
	Public Class Camera
		' Token: 0x0600131D RID: 4893 RVA: 0x0004FD58 File Offset: 0x0004E158
		Public Sub New(mode As CupheadLevelCamera.Mode, left As Integer, right As Integer, top As Integer, bottom As Integer)
			Me.mode = mode
			Me.bounds = New Level.Bounds(left, right, top, bottom)
		End Sub

		' Token: 0x04001C45 RID: 7237
		Public mode As CupheadLevelCamera.Mode = CupheadLevelCamera.Mode.Relative

		' Token: 0x04001C46 RID: 7238
		<Space(10F)>
		<Range(0.5F, 2F)>
		Public zoom As Single = 1F

		' Token: 0x04001C47 RID: 7239
		<Space(10F)>
		Public moveX As Boolean

		' Token: 0x04001C48 RID: 7240
		Public moveY As Boolean

		' Token: 0x04001C49 RID: 7241
		Public stabilizeY As Boolean

		' Token: 0x04001C4A RID: 7242
		Public stabilizePaddingTop As Single = 50F

		' Token: 0x04001C4B RID: 7243
		Public stabilizePaddingBottom As Single = 100F

		' Token: 0x04001C4C RID: 7244
		<Space(10F)>
		Public colliders As Boolean

		' Token: 0x04001C4D RID: 7245
		<Space(10F)>
		Public bounds As Level.Bounds

		' Token: 0x04001C4E RID: 7246
		<HideInInspector()>
		Public path As VectorPath

		' Token: 0x04001C4F RID: 7247
		Public pathMovesOnlyForward As Boolean
	End Class

	' Token: 0x02000498 RID: 1176
	Public Class GoalTimes
		' Token: 0x0600131E RID: 4894 RVA: 0x0004FDAB File Offset: 0x0004E1AB
		Public Sub New(easy As Single, normal As Single, hard As Single)
			Me.easy = easy
			Me.normal = normal
			Me.hard = hard
		End Sub

		' Token: 0x04001C50 RID: 7248
		Public easy As Single

		' Token: 0x04001C51 RID: 7249
		Public normal As Single

		' Token: 0x04001C52 RID: 7250
		Public hard As Single
	End Class

	' Token: 0x02000499 RID: 1177
	<Serializable()>
	Public Class IntroProperties
		' Token: 0x06001320 RID: 4896 RVA: 0x0004FDD0 File Offset: 0x0004E1D0
		Public Sub OnIntroAnimComplete()
			Me.introComplete = True
		End Sub

		' Token: 0x06001321 RID: 4897 RVA: 0x0004FDD9 File Offset: 0x0004E1D9
		Public Sub OnReadyAnimComplete()
			Me.readyComplete = True
		End Sub

		' Token: 0x04001C53 RID: 7251
		<NonSerialized()>
		Public introComplete As Boolean

		' Token: 0x04001C54 RID: 7252
		<NonSerialized()>
		Public readyComplete As Boolean
	End Class

	' Token: 0x0200049A RID: 1178
	Public Class Timeline
		' Token: 0x06001322 RID: 4898 RVA: 0x0004FDE2 File Offset: 0x0004E1E2
		Public Sub New()
			Me.health = 0F
			Me.damage = 0F
			Me.cuphead = -1F
			Me.mugman = -1F
			Me.events = New List(Of Level.Timeline.[Event])()
		End Sub

		' Token: 0x17000304 RID: 772
		' (get) Token: 0x06001323 RID: 4899 RVA: 0x0004FE21 File Offset: 0x0004E221
		' (set) Token: 0x06001324 RID: 4900 RVA: 0x0004FE29 File Offset: 0x0004E229
		Public Property damage As Single

		' Token: 0x17000305 RID: 773
		' (get) Token: 0x06001325 RID: 4901 RVA: 0x0004FE32 File Offset: 0x0004E232
		' (set) Token: 0x06001326 RID: 4902 RVA: 0x0004FE3A File Offset: 0x0004E23A
		Public Property events As List(Of Level.Timeline.[Event])

		' Token: 0x17000306 RID: 774
		' (get) Token: 0x06001327 RID: 4903 RVA: 0x0004FE43 File Offset: 0x0004E243
		' (set) Token: 0x06001328 RID: 4904 RVA: 0x0004FE4B File Offset: 0x0004E24B
		Public Property cuphead As Single

		' Token: 0x17000307 RID: 775
		' (get) Token: 0x06001329 RID: 4905 RVA: 0x0004FE54 File Offset: 0x0004E254
		' (set) Token: 0x0600132A RID: 4906 RVA: 0x0004FE5C File Offset: 0x0004E25C
		Public Property mugman As Single

		' Token: 0x0600132B RID: 4907 RVA: 0x0004FE68 File Offset: 0x0004E268
		Public Function GetHealthOfLastEvent() As Integer
			Dim num As Single = 1F
			For i As Integer = 0 To Me.events.Count - 1
				If Me.events(i).percentage < num Then
					num = Me.events(i).percentage
				End If
			Next
			Return CInt((Me.health * (1F - num)))
		End Function

		' Token: 0x0600132C RID: 4908 RVA: 0x0004FED5 File Offset: 0x0004E2D5
		Public Sub DealDamage(damage As Single)
			Me.damage += damage
		End Sub

		' Token: 0x0600132D RID: 4909 RVA: 0x0004FEE8 File Offset: 0x0004E2E8
		Public Sub OnPlayerDeath(playerId As PlayerId)
			If playerId = PlayerId.PlayerOne OrElse playerId <> PlayerId.PlayerTwo Then
				If PlayerManager.player1IsMugman Then
					Me.mugman = Me.damage
				Else
					Me.cuphead = Me.damage
				End If
			ElseIf PlayerManager.player1IsMugman Then
				Me.cuphead = Me.damage
			Else
				Me.mugman = Me.damage
			End If
		End Sub

		' Token: 0x0600132E RID: 4910 RVA: 0x0004FF60 File Offset: 0x0004E360
		Public Sub OnPlayerRevive(playerId As PlayerId)
			If playerId = PlayerId.PlayerOne OrElse playerId <> PlayerId.PlayerTwo Then
				If PlayerManager.player1IsMugman Then
					Me.mugman = -1F
				Else
					Me.cuphead = -1F
				End If
			ElseIf PlayerManager.player1IsMugman Then
				Me.cuphead = -1F
			Else
				Me.mugman = -1F
			End If
		End Sub

		' Token: 0x0600132F RID: 4911 RVA: 0x0004FFD4 File Offset: 0x0004E3D4
		Public Sub SetPlayerDamage(playerId As PlayerId, value As Single)
			If playerId = PlayerId.PlayerOne OrElse playerId <> PlayerId.PlayerTwo Then
				If PlayerManager.player1IsMugman Then
					Me.mugman = value
				Else
					Me.cuphead = value
				End If
			ElseIf PlayerManager.player1IsMugman Then
				Me.cuphead = value
			Else
				Me.mugman = value
			End If
		End Sub

		' Token: 0x06001330 RID: 4912 RVA: 0x00050037 File Offset: 0x0004E437
		Public Sub AddEvent(e As Level.Timeline.[Event])
			Me.events.Add(e)
		End Sub

		' Token: 0x06001331 RID: 4913 RVA: 0x00050048 File Offset: 0x0004E448
		Public Sub AddEventAtHealth(eventName As String, targetHealth As Integer)
			Dim num As Single = 1F - CSng(targetHealth) / Me.health
			Me.AddEvent(New Level.Timeline.[Event](eventName, num))
		End Sub

		' Token: 0x04001C55 RID: 7253
		Public health As Single

		' Token: 0x0200049B RID: 1179
		Public Class [Event]
			' Token: 0x06001332 RID: 4914 RVA: 0x00050072 File Offset: 0x0004E472
			Public Sub New(name As String, percentage As Single)
				Me.name = name
				Me.percentage = percentage
			End Sub

			' Token: 0x17000308 RID: 776
			' (get) Token: 0x06001333 RID: 4915 RVA: 0x00050088 File Offset: 0x0004E488
			' (set) Token: 0x06001334 RID: 4916 RVA: 0x00050090 File Offset: 0x0004E490
			Public Property name As String

			' Token: 0x17000309 RID: 777
			' (get) Token: 0x06001335 RID: 4917 RVA: 0x00050099 File Offset: 0x0004E499
			' (set) Token: 0x06001336 RID: 4918 RVA: 0x000500A1 File Offset: 0x0004E4A1
			Public Property percentage As Single
		End Class
	End Class
End Class
