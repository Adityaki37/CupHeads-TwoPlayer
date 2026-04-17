Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine
Imports UnityEngine.SceneManagement

' Token: 0x02000969 RID: 2409
Public Class Map
	Inherits AbstractMonoBehaviour

	' Token: 0x17000487 RID: 1159
	' (get) Token: 0x0600381F RID: 14367 RVA: 0x00201B53 File Offset: 0x001FFF53
	' (set) Token: 0x06003820 RID: 14368 RVA: 0x00201B5A File Offset: 0x001FFF5A
	Public Shared Property Current As Map

	' Token: 0x17000488 RID: 1160
	' (get) Token: 0x06003821 RID: 14369 RVA: 0x00201B62 File Offset: 0x001FFF62
	' (set) Token: 0x06003822 RID: 14370 RVA: 0x00201B6A File Offset: 0x001FFF6A
	Public Property CurrentState As Map.State

	' Token: 0x17000489 RID: 1161
	' (get) Token: 0x06003823 RID: 14371 RVA: 0x00201B73 File Offset: 0x001FFF73
	' (set) Token: 0x06003824 RID: 14372 RVA: 0x00201B7B File Offset: 0x001FFF7B
	Public Property players As MapPlayerController()

	' Token: 0x06003825 RID: 14373 RVA: 0x00201B84 File Offset: 0x001FFF84
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Map.Current = Me
		Cuphead.Init(False)
		Level.ResetBossesHub()
		Level.IsGraveyard = False
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, False, False)
		AddHandler PlayerManager.OnPlayerJoinedEvent, AddressOf Me.OnPlayerJoined
		AddHandler PlayerManager.OnPlayerLeaveEvent, AddressOf Me.OnPlayerLeave
		Me.scene = EnumUtils.Parse(Of Scenes)(SceneManager.GetActiveScene().name)
		PlayerData.Data.CurrentMap = Me.scene
		Me.CreateUI()
		Me.CreatePlayers()
		Me.ui.Init(Me.players)
		Me.cupheadMapCamera = Global.UnityEngine.[Object].FindObjectOfType(Of CupheadMapCamera)()
		Me.cupheadMapCamera.Init(Me.cameraProperties)
		CupheadTime.SetAll(1F)
		AddHandler SceneLoader.OnLoaderCompleteEvent, AddressOf Me.SelectMusic
	End Sub

	' Token: 0x06003826 RID: 14374 RVA: 0x00201C57 File Offset: 0x00200057
	Private Sub Start()
		If PlatformHelper.ManuallyRefreshDLCAvailability Then
			DLCManager.CheckInstallationStatusChanged()
		End If
		AudioManager.PlayLoop(String.Empty)
		MyBase.StartCoroutine(Me.start_cr())
	End Sub

	' Token: 0x06003827 RID: 14375 RVA: 0x00201C80 File Offset: 0x00200080
	Private Sub OnDestroy()
		RemoveHandler SceneLoader.OnLoaderCompleteEvent, AddressOf Me.SelectMusic
		RemoveHandler PlayerManager.OnPlayerJoinedEvent, AddressOf Me.OnPlayerJoined
		RemoveHandler PlayerManager.OnPlayerLeaveEvent, AddressOf Me.OnPlayerLeave
		Me.MapResources = Nothing
		Me.cameraProperties = Nothing
		Me.firstNode = Nothing
		Me.entryPoints = Nothing
		Me.ui = Nothing
		Me.cupheadMapCamera = Nothing
		Me.players = Nothing
		If Map.Current Is Me Then
			Map.Current = Nothing
		End If
	End Sub

	' Token: 0x06003828 RID: 14376 RVA: 0x00201D0C File Offset: 0x0020010C
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		Gizmos.DrawLine(New Vector3(Me.cameraProperties.bounds.left, Me.cameraProperties.bounds.top), New Vector3(Me.cameraProperties.bounds.left, Me.cameraProperties.bounds.bottom))
		Gizmos.DrawLine(New Vector3(Me.cameraProperties.bounds.right, Me.cameraProperties.bounds.top), New Vector3(Me.cameraProperties.bounds.right, Me.cameraProperties.bounds.bottom))
		Gizmos.DrawLine(New Vector3(Me.cameraProperties.bounds.right, Me.cameraProperties.bounds.top), New Vector3(Me.cameraProperties.bounds.left, Me.cameraProperties.bounds.top))
		Gizmos.DrawLine(New Vector3(Me.cameraProperties.bounds.right, Me.cameraProperties.bounds.bottom), New Vector3(Me.cameraProperties.bounds.left, Me.cameraProperties.bounds.bottom))
	End Sub

	' Token: 0x06003829 RID: 14377 RVA: 0x00201E5B File Offset: 0x0020025B
	Private Sub CreateUI()
		Me.ui = Global.UnityEngine.[Object].FindObjectOfType(Of MapUI)()
		If Me.ui Is Nothing Then
			Me.ui = MapUI.Create()
		End If
	End Sub

	' Token: 0x0600382A RID: 14378 RVA: 0x00201E84 File Offset: 0x00200284
	Private Sub CreatePlayers()
		If Not PlayerData.Data.CurrentMapData.sessionStarted Then
			PlayerData.Data.CurrentMapData.sessionStarted = True
			PlayerData.Data.CurrentMapData.playerOnePosition = Me.firstNode.transform.position + Me.firstNode.returnPositions.playerOne
			PlayerData.Data.CurrentMapData.playerTwoPosition = Me.firstNode.transform.position + Me.firstNode.returnPositions.playerTwo
			If Not PlayerManager.Multiplayer Then
				PlayerData.Data.CurrentMapData.playerOnePosition = Me.firstNode.transform.position + Me.firstNode.returnPositions.singlePlayer
			End If
		ElseIf PlayerData.Data.CurrentMapData.enteringFrom <> PlayerData.MapData.EntryMethod.None Then
			Me.entryPoints(CInt(PlayerData.Data.CurrentMapData.enteringFrom)).SetPlayerReturnPos()
			PlayerData.Data.CurrentMapData.enteringFrom = PlayerData.MapData.EntryMethod.None
		End If
		PlayerData.SaveCurrentFile()
		Dim mapPlayerPose As MapPlayerPose = MapPlayerPose.[Default]
		If Level.Won AndAlso Level.PreviousLevel <> Levels.Saltbaker Then
			mapPlayerPose = MapPlayerPose.Won
		End If
		Me.players = New MapPlayerController(1) {}
		Me.players(0) = MapPlayerController.Create(PlayerId.PlayerOne, New MapPlayerController.InitObject(PlayerData.Data.CurrentMapData.playerOnePosition, mapPlayerPose))
		If PlayerManager.Multiplayer Then
			Me.players(1) = MapPlayerController.Create(PlayerId.PlayerTwo, New MapPlayerController.InitObject(PlayerData.Data.CurrentMapData.playerTwoPosition, mapPlayerPose))
		End If
	End Sub

	' Token: 0x0600382B RID: 14379 RVA: 0x00202048 File Offset: 0x00200448
	Protected Overridable Sub OnPlayerJoined(playerId As PlayerId)
		If playerId = PlayerId.PlayerTwo Then
			Dim position As Vector3 = Me.players(0).transform.position
			Dim vector As Vector3 = position + New Vector3(0.05F, 0.05F, 0F)
			Dim layerMask As LayerMask = -257
			For i As Integer = 0 To 10 - 1
				Dim num As Single = CSng((36 * -CSng(i) + 150))
				Dim vector2 As Vector2 = New Vector2(Mathf.Cos(0.017453292F * num), Mathf.Sin(0.017453292F * num))
				If Not(Physics2D.CircleCast(position, 0.2F, vector2, 0.7F, layerMask.value).collider IsNot Nothing) Then
					vector = position + vector2 * 0.7F
					Exit For
				End If
			Next
			Me.players(1) = MapPlayerController.Create(PlayerId.PlayerTwo, New MapPlayerController.InitObject(vector, MapPlayerPose.Joined))
			Me.players(1).animationController.spriteRenderer.sortingOrder = Me.players(0).animationController.spriteRenderer.sortingOrder
			LevelNewPlayerGUI.Current.Init()
			Me.SetRichPresence()
		End If
		Me.CheckMusic(True)
	End Sub

	' Token: 0x0600382C RID: 14380 RVA: 0x0020218C File Offset: 0x0020058C
	Protected Overridable Sub OnPlayerLeave(playerId As PlayerId)
		If playerId = PlayerId.PlayerTwo Then
			Me.players(1).OnLeave()
		End If
		Me.CheckMusic(True)
	End Sub

	' Token: 0x0600382D RID: 14381 RVA: 0x002021A9 File Offset: 0x002005A9
	Protected Overridable Sub SelectMusic()
		Me.currentMusic = -1
		Me.CheckMusic(False)
	End Sub

	' Token: 0x0600382E RID: 14382 RVA: 0x002021B9 File Offset: 0x002005B9
	Public Sub OnCloseEquipMenu()
		Me.CheckMusic(True)
	End Sub

	' Token: 0x0600382F RID: 14383 RVA: 0x002021C2 File Offset: 0x002005C2
	Public Sub OnNPCChangeMusic()
		Me.CheckMusic(True)
	End Sub

	' Token: 0x06003830 RID: 14384 RVA: 0x002021CC File Offset: 0x002005CC
	Protected Overridable Sub CheckMusic(isRecheck As Boolean)
		Dim num As Integer = Me.currentMusic
		Dim playerLoadout As PlayerData.PlayerLoadouts.PlayerLoadout = PlayerData.Data.Loadouts.GetPlayerLoadout(PlayerId.PlayerOne)
		Dim playerLoadout2 As PlayerData.PlayerLoadouts.PlayerLoadout = PlayerData.Data.Loadouts.GetPlayerLoadout(PlayerId.PlayerTwo)
		If(playerLoadout.charm = Charm.charm_curse AndAlso CharmCurse.CalculateLevel(PlayerId.PlayerOne) > -1) OrElse (PlayerManager.Multiplayer AndAlso playerLoadout2.charm = Charm.charm_curse AndAlso CharmCurse.CalculateLevel(PlayerId.PlayerTwo) > -1) Then
			If(playerLoadout.charm = Charm.charm_curse AndAlso CharmCurse.IsMaxLevel(PlayerId.PlayerOne)) OrElse (PlayerManager.Multiplayer AndAlso playerLoadout2.charm = Charm.charm_curse AndAlso CharmCurse.IsMaxLevel(PlayerId.PlayerTwo)) Then
				num = If((Not PlayerData.Data.pianoAudioEnabled), 2, 4)
			Else
				num = If((Not PlayerData.Data.pianoAudioEnabled), 1, 3)
			End If
		Else
			num = If((Not PlayerData.Data.pianoAudioEnabled), (-1), 0)
		End If
		If Not isRecheck OrElse num <> Me.currentMusic Then
			Me.currentMusic = num
			If Me.currentMusic = -1 Then
				AudioManager.PlayBGM()
			Else
				AudioManager.StartBGMAlternate(Me.currentMusic)
			End If
		End If
	End Sub

	' Token: 0x06003831 RID: 14385 RVA: 0x0020230D File Offset: 0x0020070D
	Public Sub OnLoadLevel()
	End Sub

	' Token: 0x06003832 RID: 14386 RVA: 0x0020230F File Offset: 0x0020070F
	Public Sub OnLoadShop()
	End Sub

	' Token: 0x06003833 RID: 14387 RVA: 0x00202314 File Offset: 0x00200714
	Private Iterator Function start_cr() As IEnumerator
		Me.SetRichPresence()
		Level.ResetBossesHub()
		If Level.Won AndAlso Level.PreviousLevel <> Levels.Saltbaker Then
			Yield CupheadTime.WaitForSeconds(Me, 1.5F)
			Dim longPlayerAnimation As Boolean = True
			Dim cameraMoved As Boolean = False
			Dim cameraStartPos As Vector3 = Me.cupheadMapCamera.transform.position
			If AbstractMapLevelDependentEntity.RegisteredEntities IsNot Nothing Then
				While AbstractMapLevelDependentEntity.RegisteredEntities.Count > 0
					Yield Nothing
					Me.CurrentState = Map.State.[Event]
					Dim entity As AbstractMapLevelDependentEntity = AbstractMapLevelDependentEntity.RegisteredEntities(0)
					For Each abstractMapLevelDependentEntity As AbstractMapLevelDependentEntity In AbstractMapLevelDependentEntity.RegisteredEntities
						If Not abstractMapLevelDependentEntity.panCamera Then
							entity = abstractMapLevelDependentEntity
							Exit For
						End If
						If Not(abstractMapLevelDependentEntity Is entity) Then
							Dim num As Single = Vector2.Distance(Me.cupheadMapCamera.transform.position, abstractMapLevelDependentEntity.CameraPosition)
							If num < Vector2.Distance(Me.cupheadMapCamera.transform.position, entity.CameraPosition) Then
								entity = abstractMapLevelDependentEntity
							End If
						End If
					Next
					AbstractMapLevelDependentEntity.RegisteredEntities.Remove(entity)
					If entity.panCamera Then
						Yield Me.cupheadMapCamera.MoveToPosition(entity.CameraPosition, 0.5F, 0.9F)
						cameraMoved = True
					End If
					entity.MapMeetCondition()
					While entity.CurrentState <> AbstractMapLevelDependentEntity.State.Complete
						Yield Nothing
					End While
					Yield CupheadTime.WaitForSeconds(Me, 0.25F)
					longPlayerAnimation = False
				End While
				If cameraMoved Then
					Me.cupheadMapCamera.MoveToPosition(cameraStartPos, 0.75F, 1F)
				End If
			End If
			If Not PlayerManager.playerWasChalice(0) AndAlso (Not PlayerManager.Multiplayer OrElse Not PlayerManager.playerWasChalice(1)) Then
				Yield CupheadTime.WaitForSeconds(Me, If((Not longPlayerAnimation), 1F, 2.5F))
				Me.players(0).OnWinComplete()
				If PlayerManager.Multiplayer Then
					Me.players(1).OnWinComplete()
				End If
			Else
				If PlayerManager.playerWasChalice(0) Then
					Me.players(0).OnWinComplete()
				End If
				If PlayerManager.Multiplayer AndAlso PlayerManager.playerWasChalice(1) Then
					Me.players(1).OnWinComplete()
				End If
				Yield CupheadTime.WaitForSeconds(Me, 1F)
				If Not PlayerManager.playerWasChalice(0) Then
					Me.players(0).OnWinComplete()
				End If
				If PlayerManager.Multiplayer AndAlso Not PlayerManager.playerWasChalice(1) Then
					Me.players(1).OnWinComplete()
				End If
			End If
			If Not Level.PreviouslyWon OrElse Level.PreviousDifficulty < Level.Mode.Normal OrElse Level.PreviousLevel = Levels.Mausoleum Then
				If Not Level.IsDicePalace AndAlso Not Level.IsDicePalaceMain AndAlso Level.PreviousLevel <> Levels.Devil AndAlso Level.PreviousLevel <> Levels.DicePalaceMain AndAlso Level.PreviousLevel <> Levels.Mausoleum AndAlso Level.PreviousLevelType = Level.Type.Battle Then
					If Array.IndexOf(Of Levels)(Level.worldDLCBossLevels, Level.PreviousLevel) >= 0 Then
						If Level.Difficulty = Level.Mode.Easy AndAlso Not PlayerData.Data.hasBeatenAnyDLCBossOnEasy AndAlso Not PlayerData.Data.CheckLevelsHaveMinDifficulty(Level.worldDLCBossLevels, Level.Mode.Normal) Then
							MapEventNotification.Current.ShowTooltipEvent(TooltipEvent.SimpleIngredient)
							PlayerData.Data.hasBeatenAnyDLCBossOnEasy = True
							PlayerData.SaveCurrentFile()
						End If
					ElseIf Level.Difficulty = Level.Mode.Easy AndAlso Not PlayerData.Data.hasBeatenAnyBossOnEasy AndAlso (Not PlayerData.Data.CheckLevelsHaveMinDifficulty(Level.world1BossLevels, Level.Mode.Normal) OrElse Not PlayerData.Data.CheckLevelsHaveMinDifficulty(Level.world2BossLevels, Level.Mode.Normal) OrElse Not PlayerData.Data.CheckLevelsHaveMinDifficulty(Level.world3BossLevels, Level.Mode.Normal)) Then
						MapEventNotification.Current.ShowTooltipEvent(TooltipEvent.KingDice)
						PlayerData.Data.hasBeatenAnyBossOnEasy = True
						PlayerData.SaveCurrentFile()
					End If
					If Level.Difficulty >= Level.Mode.Normal Then
						If Level.PreviousLevel = Levels.Airplane Then
							MapEventNotification.Current.ShowEvent(MapEventNotification.Type.AirplaneIngredient)
							While MapEventNotification.Current.showing
								Yield Nothing
							End While
							longPlayerAnimation = False
							Yield CupheadTime.WaitForSeconds(Me, 0.25F)
						ElseIf Level.PreviousLevel = Levels.RumRunners Then
							MapEventNotification.Current.ShowEvent(MapEventNotification.Type.RumIngredient)
							While MapEventNotification.Current.showing
								Yield Nothing
							End While
							longPlayerAnimation = False
							Yield CupheadTime.WaitForSeconds(Me, 0.25F)
						ElseIf Level.PreviousLevel = Levels.OldMan Then
							MapEventNotification.Current.ShowEvent(MapEventNotification.Type.OldManIngredient)
							While MapEventNotification.Current.showing
								Yield Nothing
							End While
							longPlayerAnimation = False
							Yield CupheadTime.WaitForSeconds(Me, 0.25F)
						ElseIf Level.PreviousLevel = Levels.SnowCult Then
							MapEventNotification.Current.ShowEvent(MapEventNotification.Type.SnowIngredient)
							While MapEventNotification.Current.showing
								Yield Nothing
							End While
							longPlayerAnimation = False
							Yield CupheadTime.WaitForSeconds(Me, 0.25F)
						ElseIf Level.PreviousLevel = Levels.FlyingCowboy Then
							MapEventNotification.Current.ShowEvent(MapEventNotification.Type.CowboyIngredient)
							While MapEventNotification.Current.showing
								Yield Nothing
							End While
							longPlayerAnimation = False
							Yield CupheadTime.WaitForSeconds(Me, 0.25F)
						ElseIf Level.PreviousLevel = Levels.Graveyard Then
							GameObject.Find("GhostDetective").GetComponent(Of MapNPCGraveyardGhost)().TalkAfterPlayerGotCharm()
						ElseIf Level.PreviousLevel <> Levels.Saltbaker Then
							MapEventNotification.Current.ShowEvent(MapEventNotification.Type.SoulContract)
							While MapEventNotification.Current.showing
								Yield Nothing
							End While
							longPlayerAnimation = False
							Yield CupheadTime.WaitForSeconds(Me, 0.25F)
						End If
					End If
					If PlayerData.Data.CheckLevelsHaveMinDifficulty(Level.worldDLCBossLevels, Level.Mode.Normal) AndAlso Array.IndexOf(Of Levels)(Level.worldDLCBossLevels, Level.PreviousLevel) >= 0 Then
						MapEventNotification.Current.ShowTooltipEvent(TooltipEvent.BackToKitchen)
					End If
					Dim bossCounter As Integer = 0
					For i As Integer = 0 To Level.chaliceLevels.Length - 1
						If PlayerData.Data.GetLevelData(Level.chaliceLevels(i)).completedAsChaliceP1 Then
							bossCounter += 1
						End If
					Next
				ElseIf Level.SuperUnlocked Then
					MapEventNotification.Current.ShowEvent(MapEventNotification.Type.Super)
					If Not PlayerData.Data.hasUnlockedFirstSuper Then
						MapEventNotification.Current.ShowTooltipEvent(TooltipEvent.Mausoleum)
						PlayerData.Data.hasUnlockedFirstSuper = True
						PlayerData.SaveCurrentFile()
					End If
					longPlayerAnimation = False
				End If
			End If
			While MapEventNotification.Current AndAlso MapEventNotification.Current.showing
				Yield Nothing
			End While
		End If
		If DLCManager.showAvailabilityPrompt Then
			Yield CupheadTime.WaitForSeconds(Me, If((Not Level.Won), 1.5F, 0.5F))
			DLCManager.ResetAvailabilityPrompt()
			MapEventNotification.Current.ShowEvent(MapEventNotification.Type.DLCAvailable)
		End If
		Me.CurrentState = Map.State.Ready
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, True, True)
		InterruptingPrompt.SetCanInterrupt(True)
		Level.ResetPreviousLevelInfo()
		Return
	End Function

	' Token: 0x06003834 RID: 14388 RVA: 0x0020232F File Offset: 0x0020072F
	Private Sub SetRichPresence()
		OnlineManager.Instance.[Interface].SetStat(PlayerId.Any, "WorldMap", SceneLoader.SceneName)
		OnlineManager.Instance.[Interface].SetRichPresence(PlayerId.Any, "Exploring", True)
	End Sub

	' Token: 0x04003FFF RID: 16383
	Public MapResources As MapResources

	' Token: 0x04004000 RID: 16384
	<SerializeField()>
	Private cameraProperties As Map.Camera

	' Token: 0x04004001 RID: 16385
	<Space(10F)>
	<SerializeField()>
	Private firstNode As AbstractMapInteractiveEntity

	' Token: 0x04004002 RID: 16386
	<SerializeField()>
	Private entryPoints As AbstractMapInteractiveEntity()

	' Token: 0x04004003 RID: 16387
	Private ui As MapUI

	' Token: 0x04004004 RID: 16388
	Private scene As Scenes

	' Token: 0x04004005 RID: 16389
	Private cupheadMapCamera As CupheadMapCamera

	' Token: 0x04004008 RID: 16392
	Public level As Levels

	' Token: 0x04004009 RID: 16393
	Public LevelCoinsIDs As List(Of CoinPositionAndID) = New List(Of CoinPositionAndID)()

	' Token: 0x0400400A RID: 16394
	Protected currentMusic As Integer

	' Token: 0x0200096A RID: 2410
	Public Enum State
		' Token: 0x0400400C RID: 16396
		Starting
		' Token: 0x0400400D RID: 16397
		Ready
		' Token: 0x0400400E RID: 16398
		[Event]
		' Token: 0x0400400F RID: 16399
		Exiting
		' Token: 0x04004010 RID: 16400
		Graveyard
	End Enum

	' Token: 0x0200096B RID: 2411
	<Serializable()>
	Public Class Camera
		' Token: 0x04004011 RID: 16401
		Public moveX As Boolean = True

		' Token: 0x04004012 RID: 16402
		Public moveY As Boolean = True

		' Token: 0x04004013 RID: 16403
		Public bounds As CupheadBounds = New CupheadBounds(-6.4F, 6.4F, 3.6F, -3.6F)
	End Class
End Class
