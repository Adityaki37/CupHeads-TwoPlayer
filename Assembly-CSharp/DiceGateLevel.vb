Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x020000EF RID: 239
Public Class DiceGateLevel
	Inherits Level

	' Token: 0x060002B1 RID: 689 RVA: 0x0005DF54 File Offset: 0x0005C354
	Protected Overrides Sub PartialInit()
		Me.properties = LevelProperties.DiceGate.GetMode(MyBase.mode)
		AddHandler Me.properties.OnStateChange, AddressOf MyBase.zHack_OnStateChanged
		AddHandler Me.properties.OnBossDeath, AddressOf MyBase.zHack_OnWin
		MyBase.timeline = Me.properties.CreateTimeline(MyBase.mode)
		Me.goalTimes = Me.properties.goalTimes
		AddHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		MyBase.PartialInit()
	End Sub

	' Token: 0x1700006D RID: 109
	' (get) Token: 0x060002B2 RID: 690 RVA: 0x0005DFEA File Offset: 0x0005C3EA
	Public Overrides ReadOnly Property CurrentLevel As Levels
		Get
			Return Levels.DiceGate
		End Get
	End Property

	' Token: 0x1700006E RID: 110
	' (get) Token: 0x060002B3 RID: 691 RVA: 0x0005DFF1 File Offset: 0x0005C3F1
	Public Overrides ReadOnly Property CurrentScene As Scenes
		Get
			Return Scenes.scene_level_dice_gate
		End Get
	End Property

	' Token: 0x1700006F RID: 111
	' (get) Token: 0x060002B4 RID: 692 RVA: 0x0005DFF5 File Offset: 0x0005C3F5
	Public Overrides ReadOnly Property BossPortrait As Sprite
		Get
			Return Me._bossPortrait
		End Get
	End Property

	' Token: 0x17000070 RID: 112
	' (get) Token: 0x060002B5 RID: 693 RVA: 0x0005DFFD File Offset: 0x0005C3FD
	Public Overrides ReadOnly Property BossQuote As String
		Get
			Return Me._bossQuote
		End Get
	End Property

	' Token: 0x060002B6 RID: 694 RVA: 0x0005E008 File Offset: 0x0005C408
	Protected Overrides Sub Start()
		MyBase.Start()
		AddHandler SceneLoader.OnLoaderCompleteEvent, AddressOf Me.SetMusic
		If PlayerData.Data.CurrentMap = Scenes.scene_map_world_1 Then
			Me.world1Background.SetActive(True)
			If PlayerData.Data.CheckLevelCompleted(Levels.Veggies) Then
				Me.chalkboardCrosses(0).SetActive(True)
			End If
			If PlayerData.Data.CheckLevelCompleted(Levels.Slime) Then
				Me.chalkboardCrosses(1).SetActive(True)
			End If
			If PlayerData.Data.CheckLevelCompleted(Levels.Frogs) Then
				Me.chalkboardCrosses(2).SetActive(True)
			End If
			If PlayerData.Data.CheckLevelCompleted(Levels.FlyingBlimp) Then
				Me.chalkboardCrosses(3).SetActive(True)
			End If
			If PlayerData.Data.CheckLevelCompleted(Levels.Flower) Then
				Me.chalkboardCrosses(4).SetActive(True)
			End If
			If PlayerData.Data.CheckLevelsCompleted(Level.world1BossLevels) Then
				Me.dialogueInteractionPoint.animationTriggerOnEnd = Me.completeLevelAnimationTrigger
				Me.OpenWay()
			Else
				Me.CloseWay()
			End If
		ElseIf PlayerData.Data.CurrentMap = Scenes.scene_map_world_2 Then
			Me.world2Background.SetActive(True)
			Me.toPrevWorld.dialogueProperties = Me.world2PrevProperties
			Me.toNextWorld.dialogueProperties = Me.world2NextProperties
			If PlayerData.Data.CheckLevelCompleted(Levels.Baroness) Then
				Me.chalkboardCrosses(0).SetActive(True)
			End If
			If PlayerData.Data.CheckLevelCompleted(Levels.FlyingGenie) Then
				Me.chalkboardCrosses(1).SetActive(True)
			End If
			If PlayerData.Data.CheckLevelCompleted(Levels.Clown) Then
				Me.chalkboardCrosses(2).SetActive(True)
			End If
			If PlayerData.Data.CheckLevelCompleted(Levels.FlyingBird) Then
				Me.chalkboardCrosses(3).SetActive(True)
			End If
			If PlayerData.Data.CheckLevelCompleted(Levels.Dragon) Then
				Me.chalkboardCrosses(4).SetActive(True)
			End If
			Me.dialogueInteractionPoint.dialogueInteraction = Me.dialogueWorld2
			If PlayerData.Data.CheckLevelsCompleted(Level.world2BossLevels) Then
				Me.dialogueInteractionPoint.animationTriggerOnEnd = Me.completeLevelAnimationTrigger
				Me.OpenWay()
			Else
				Me.CloseWay()
			End If
		Else
			Global.Debug.LogError("SOMETHING BAD HAPPENED", Nothing)
		End If
	End Sub

	' Token: 0x060002B7 RID: 695 RVA: 0x0005E28C File Offset: 0x0005C68C
	Private Sub SetMusic()
		AudioManager.PlayBGMPlaylistManually(True)
	End Sub

	' Token: 0x060002B8 RID: 696 RVA: 0x0005E294 File Offset: 0x0005C694
	Protected Overrides Sub OnLevelStart()
	End Sub

	' Token: 0x060002B9 RID: 697 RVA: 0x0005E296 File Offset: 0x0005C696
	Protected Overrides Sub OnDestroy()
		RemoveHandler SceneLoader.OnLoaderCompleteEvent, AddressOf Me.SetMusic
		MyBase.OnDestroy()
		Me._bossPortrait = Nothing
	End Sub

	' Token: 0x060002BA RID: 698 RVA: 0x0005E2B8 File Offset: 0x0005C6B8
	Private Sub CloseWay()
		Me.toNextWorld.enabled = False
		Me.kingDice.SetActive(True)
		If PlayerData.Data.CurrentMapData.hasVisitedDieHouse Then
			If PlayerData.Data.CurrentMap = Scenes.scene_map_world_1 Then
				Dialoguer.SetGlobalFloat(16, 1F)
			End If
			PlayerData.SaveCurrentFile()
		End If
	End Sub

	' Token: 0x060002BB RID: 699 RVA: 0x0005E314 File Offset: 0x0005C714
	Private Sub OpenWay()
		Me.toNextWorld.enabled = True
		If PlayerData.Data.CurrentMapData.hasKingDiceDisappeared Then
			Me.kingDice.SetActive(False)
		End If
		If PlayerData.Data.CurrentMap = Scenes.scene_map_world_1 Then
			Dialoguer.SetGlobalFloat(16, 2F)
		Else
			Dialoguer.SetGlobalFloat(17, 1F)
		End If
		PlayerData.SaveCurrentFile()
	End Sub

	' Token: 0x060002BC RID: 700 RVA: 0x0005E380 File Offset: 0x0005C780
	Private Iterator Function dicegatePattern_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		While True
			Yield MyBase.StartCoroutine(Me.nextPattern_cr())
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060002BD RID: 701 RVA: 0x0005E39C File Offset: 0x0005C79C
	Private Iterator Function nextPattern_cr() As IEnumerator
		Dim p As LevelProperties.DiceGate.Pattern = Me.properties.CurrentState.NextPattern
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		Return
	End Function

	' Token: 0x0400050A RID: 1290
	Private properties As LevelProperties.DiceGate

	' Token: 0x0400050B RID: 1291
	<SerializeField()>
	Private toNextWorld As AbstractLevelInteractiveEntity

	' Token: 0x0400050C RID: 1292
	<SerializeField()>
	Private toPrevWorld As AbstractLevelInteractiveEntity

	' Token: 0x0400050D RID: 1293
	Public world2PrevProperties As AbstractUIInteractionDialogue.Properties

	' Token: 0x0400050E RID: 1294
	Public world2NextProperties As AbstractUIInteractionDialogue.Properties

	' Token: 0x0400050F RID: 1295
	<SerializeField()>
	Private kingDice As GameObject

	' Token: 0x04000510 RID: 1296
	<SerializeField()>
	Private chalkboardCrosses As List(Of GameObject)

	' Token: 0x04000511 RID: 1297
	<SerializeField()>
	Private dialogueInteractionPoint As DialogueInteractionPoint

	' Token: 0x04000512 RID: 1298
	<SerializeField()>
	Private dialogueWorld2 As DialoguerDialogues

	' Token: 0x04000513 RID: 1299
	<SerializeField()>
	Private completeLevelAnimationTrigger As String

	' Token: 0x04000514 RID: 1300
	<SerializeField()>
	Private world1Background As GameObject

	' Token: 0x04000515 RID: 1301
	<SerializeField()>
	Private world2Background As GameObject

	' Token: 0x04000516 RID: 1302
	<Header("Boss Info")>
	<SerializeField()>
	Private _bossPortrait As Sprite

	' Token: 0x04000517 RID: 1303
	<SerializeField()>
	<Multiline()>
	Private _bossQuote As String
End Class
