Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020001FB RID: 507
Public Class HouseLevel
	Inherits Level

	' Token: 0x060005A1 RID: 1441 RVA: 0x000693CC File Offset: 0x000677CC
	Protected Overrides Sub PartialInit()
		Me.properties = LevelProperties.House.GetMode(MyBase.mode)
		AddHandler Me.properties.OnStateChange, AddressOf MyBase.zHack_OnStateChanged
		AddHandler Me.properties.OnBossDeath, AddressOf MyBase.zHack_OnWin
		MyBase.timeline = Me.properties.CreateTimeline(MyBase.mode)
		Me.goalTimes = Me.properties.goalTimes
		AddHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		MyBase.PartialInit()
	End Sub

	' Token: 0x170000FD RID: 253
	' (get) Token: 0x060005A2 RID: 1442 RVA: 0x00069462 File Offset: 0x00067862
	Public Overrides ReadOnly Property CurrentLevel As Levels
		Get
			Return Levels.House
		End Get
	End Property

	' Token: 0x170000FE RID: 254
	' (get) Token: 0x060005A3 RID: 1443 RVA: 0x00069469 File Offset: 0x00067869
	Public Overrides ReadOnly Property CurrentScene As Scenes
		Get
			Return Scenes.scene_level_house_elder_kettle
		End Get
	End Property

	' Token: 0x170000FF RID: 255
	' (get) Token: 0x060005A4 RID: 1444 RVA: 0x0006946D File Offset: 0x0006786D
	Public Overrides ReadOnly Property BossPortrait As Sprite
		Get
			Return Me._bossPortrait
		End Get
	End Property

	' Token: 0x17000100 RID: 256
	' (get) Token: 0x060005A5 RID: 1445 RVA: 0x00069475 File Offset: 0x00067875
	Public Overrides ReadOnly Property BossQuote As String
		Get
			Return Me._bossQuote
		End Get
	End Property

	' Token: 0x060005A6 RID: 1446 RVA: 0x00069480 File Offset: 0x00067880
	Protected Overrides Sub Start()
		MyBase.Start()
		If PlayerData.Data.CheckLevelsHaveMinDifficulty(New Levels() { Levels.Devil }, Level.Mode.Hard) Then
			Dialoguer.SetGlobalFloat(Me.dialoguerVariableID, 8F)
		ElseIf PlayerData.Data.CountLevelsHaveMinDifficulty(Level.world1BossLevels, Level.Mode.Hard) + PlayerData.Data.CountLevelsHaveMinDifficulty(Level.world2BossLevels, Level.Mode.Hard) + PlayerData.Data.CountLevelsHaveMinDifficulty(Level.world3BossLevels, Level.Mode.Hard) + PlayerData.Data.CountLevelsHaveMinDifficulty(Level.world4BossLevels, Level.Mode.Hard) > 0 Then
			Dialoguer.SetGlobalFloat(Me.dialoguerVariableID, 7F)
		ElseIf PlayerData.Data.IsHardModeAvailable Then
			Dialoguer.SetGlobalFloat(Me.dialoguerVariableID, 6F)
		ElseIf PlayerData.Data.CheckLevelsCompleted(Level.world2BossLevels) Then
			Dialoguer.SetGlobalFloat(Me.dialoguerVariableID, 5F)
		ElseIf PlayerData.Data.CheckLevelsCompleted(Level.world1BossLevels) Then
			Dialoguer.SetGlobalFloat(Me.dialoguerVariableID, 4F)
		ElseIf PlayerData.Data.CountLevelsCompleted(Level.world1BossLevels) > 1 Then
			Dialoguer.SetGlobalFloat(Me.dialoguerVariableID, 3F)
		ElseIf PlayerData.Data.IsTutorialCompleted Then
			Dialoguer.SetGlobalFloat(Me.dialoguerVariableID, 2F)
		ElseIf Dialoguer.GetGlobalFloat(Me.dialoguerVariableID) = 0F Then
			Me.tutorialGameObject.SetActive(False)
			MyBase.Ending = True
		End If
		AddHandler SceneLoader.OnLoaderCompleteEvent, AddressOf Me.SelectMusic
		Me.AddDialoguerEvents()
	End Sub

	' Token: 0x060005A7 RID: 1447 RVA: 0x00069627 File Offset: 0x00067A27
	Private Sub SelectMusic()
		If PlayerData.Data.pianoAudioEnabled Then
			AudioManager.PlayBGMPlaylistManually(False)
		Else
			AudioManager.PlayBGM()
		End If
	End Sub

	' Token: 0x060005A8 RID: 1448 RVA: 0x00069648 File Offset: 0x00067A48
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		RemoveHandler SceneLoader.OnLoaderCompleteEvent, AddressOf Me.SelectMusic
		Me.RemoveDialoguerEvents()
		Me.playerTutorialEffects = Nothing
	End Sub

	' Token: 0x060005A9 RID: 1449 RVA: 0x00069670 File Offset: 0x00067A70
	Public Sub AddDialoguerEvents()
		AddHandler Dialoguer.events.onMessageEvent, AddressOf Me.OnDialoguerMessageEvent
		AddHandler Dialoguer.events.onEnded, AddressOf Me.OnDialogueEndedHandler
		AddHandler Dialoguer.events.onInstantlyEnded, AddressOf Me.OnDialogueEndedHandler
	End Sub

	' Token: 0x060005AA RID: 1450 RVA: 0x000696BF File Offset: 0x00067ABF
	Private Sub OnDialogueEndedHandler()
		MyBase.Ending = False
	End Sub

	' Token: 0x060005AB RID: 1451 RVA: 0x000696C8 File Offset: 0x00067AC8
	Public Sub RemoveDialoguerEvents()
		RemoveHandler Dialoguer.events.onMessageEvent, AddressOf Me.OnDialoguerMessageEvent
	End Sub

	' Token: 0x060005AC RID: 1452 RVA: 0x000696E0 File Offset: 0x00067AE0
	Public Sub StartTutorial()
		Dim abstractPlayerController As AbstractPlayerController = PlayerManager.GetPlayer(PlayerId.PlayerOne)
		Me.playerTutorialEffects(0).gameObject.SetActive(True)
		Me.playerTutorialEffects(0).transform.position = abstractPlayerController.transform.position
		abstractPlayerController.gameObject.SetActive(False)
		Me.playerTutorialEffects(0).animator.SetTrigger("OnStartTutorial")
		abstractPlayerController = PlayerManager.GetPlayer(PlayerId.PlayerTwo)
		If abstractPlayerController IsNot Nothing Then
			Me.playerTutorialEffects(1).gameObject.SetActive(True)
			Me.playerTutorialEffects(1).transform.position = abstractPlayerController.transform.position
			abstractPlayerController.gameObject.SetActive(False)
			Me.playerTutorialEffects(1).animator.SetTrigger("OnStartTutorial")
		End If
	End Sub

	' Token: 0x060005AD RID: 1453 RVA: 0x000697B0 File Offset: 0x00067BB0
	Private Sub OnDialoguerMessageEvent(message As String, metadata As String)
		If message = "ElderKettleFirstWeapon" Then
			Me.tutorialGameObject.SetActive(True)
			MyBase.StartCoroutine(Me.power_up_cr())
		End If
		If message = "EndJoy" Then
		End If
		If message = "Sleep" Then
		End If
	End Sub

	' Token: 0x060005AE RID: 1454 RVA: 0x00069808 File Offset: 0x00067C08
	Private Iterator Function power_up_cr() As IEnumerator
		Yield New WaitForSeconds(0.15F)
		AudioManager.Play("sfx_potion_poof")
		For Each abstractPlayerController As AbstractPlayerController In Me.players
			If Not(abstractPlayerController Is Nothing) Then
				abstractPlayerController.animator.Play("Power_Up")
			End If
		Next
		Return
	End Function

	' Token: 0x060005AF RID: 1455 RVA: 0x00069823 File Offset: 0x00067C23
	Protected Overrides Sub OnLevelStart()
		MyBase.StartCoroutine(Me.housePattern_cr())
	End Sub

	' Token: 0x060005B0 RID: 1456 RVA: 0x00069834 File Offset: 0x00067C34
	Private Iterator Function housePattern_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 0.5F)
		If Dialoguer.GetGlobalFloat(Me.dialoguerVariableID) = 0F Then
			Me.elderDialoguePoint.BeginDialogue()
		End If
		Return
	End Function

	' Token: 0x04000A76 RID: 2678
	Private properties As LevelProperties.House

	' Token: 0x04000A77 RID: 2679
	<Header("Boss Info")>
	<SerializeField()>
	Private _bossPortrait As Sprite

	' Token: 0x04000A78 RID: 2680
	<SerializeField()>
	<Multiline()>
	Private _bossQuote As String

	' Token: 0x04000A79 RID: 2681
	<SerializeField()>
	Private playerTutorialEffects As PlayerDeathEffect()

	' Token: 0x04000A7A RID: 2682
	<SerializeField()>
	Private elderDialoguePoint As HouseElderKettle

	' Token: 0x04000A7B RID: 2683
	<SerializeField()>
	Private tutorialGameObject As GameObject

	' Token: 0x04000A7C RID: 2684
	<SerializeField()>
	Private dialoguerVariableID As Integer
End Class
