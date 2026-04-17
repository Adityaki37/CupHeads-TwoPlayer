Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000201 RID: 513
Public Class KitchenLevel
	Inherits Level

	' Token: 0x060005BC RID: 1468 RVA: 0x00069A24 File Offset: 0x00067E24
	Protected Overrides Sub PartialInit()
		Me.properties = LevelProperties.Kitchen.GetMode(MyBase.mode)
		AddHandler Me.properties.OnStateChange, AddressOf MyBase.zHack_OnStateChanged
		AddHandler Me.properties.OnBossDeath, AddressOf MyBase.zHack_OnWin
		MyBase.timeline = Me.properties.CreateTimeline(MyBase.mode)
		Me.goalTimes = Me.properties.goalTimes
		AddHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		MyBase.PartialInit()
	End Sub

	' Token: 0x17000102 RID: 258
	' (get) Token: 0x060005BD RID: 1469 RVA: 0x00069ABA File Offset: 0x00067EBA
	Public Overrides ReadOnly Property CurrentLevel As Levels
		Get
			Return Levels.Kitchen
		End Get
	End Property

	' Token: 0x17000103 RID: 259
	' (get) Token: 0x060005BE RID: 1470 RVA: 0x00069AC1 File Offset: 0x00067EC1
	Public Overrides ReadOnly Property CurrentScene As Scenes
		Get
			Return Scenes.scene_level_kitchen
		End Get
	End Property

	' Token: 0x17000104 RID: 260
	' (get) Token: 0x060005BF RID: 1471 RVA: 0x00069AC5 File Offset: 0x00067EC5
	Public Overrides ReadOnly Property BossPortrait As Sprite
		Get
			Return Me._bossPortrait
		End Get
	End Property

	' Token: 0x17000105 RID: 261
	' (get) Token: 0x060005C0 RID: 1472 RVA: 0x00069ACD File Offset: 0x00067ECD
	Public Overrides ReadOnly Property BossQuote As String
		Get
			Return Me._bossQuote
		End Get
	End Property

	' Token: 0x060005C1 RID: 1473 RVA: 0x00069AD8 File Offset: 0x00067ED8
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.CheckIfBossesCompleted()
		MyBase.StartCoroutine(Me.check_camera_cr())
		MyBase.StartCoroutine(Me.cycle_sunbeams_cr())
		Me.beforeGettingIngredients.SetActive(Not Me.trapDoorOpen)
		Me.afterGettingIngredients.SetActive(Me.trapDoorOpen)
		Me.AddDialoguerEvents()
		AddHandler PlayerManager.OnPlayerJoinedEvent, AddressOf Me.SetPlayerBasementMaterial
	End Sub

	' Token: 0x060005C2 RID: 1474 RVA: 0x00069B47 File Offset: 0x00067F47
	Protected Overrides Sub OnDestroy()
		Me.RemoveDialoguerEvents()
		RemoveHandler PlayerManager.OnPlayerJoinedEvent, AddressOf Me.SetPlayerBasementMaterial
		MyBase.OnDestroy()
	End Sub

	' Token: 0x060005C3 RID: 1475 RVA: 0x00069B68 File Offset: 0x00067F68
	Private Sub SetPlayerBasementMaterial(p As PlayerId)
		If Not Me.basementBG.activeInHierarchy Then
			Return
		End If
		For Each spriteRenderer As SpriteRenderer In PlayerManager.GetPlayer(p).GetComponentsInChildren(Of SpriteRenderer)()
			If spriteRenderer.material.name = "Sprites-Default (Instance)" OrElse (spriteRenderer.sharedMaterial.name = "ChaliceRecolor (Instance)" AndAlso spriteRenderer.sharedMaterial.GetFloat("_RecolorFactor") = 0F) Then
				spriteRenderer.material = Me.playerBasementMaterial
				spriteRenderer.color = New Color(0.7137255F, 0.4862745F, 0.12941177F)
			End If
		Next
	End Sub

	' Token: 0x060005C4 RID: 1476 RVA: 0x00069C1E File Offset: 0x0006801E
	Public Sub AddDialoguerEvents()
		AddHandler Dialoguer.events.onMessageEvent, AddressOf Me.OnDialoguerMessageEvent
	End Sub

	' Token: 0x060005C5 RID: 1477 RVA: 0x00069C36 File Offset: 0x00068036
	Public Sub RemoveDialoguerEvents()
		RemoveHandler Dialoguer.events.onMessageEvent, AddressOf Me.OnDialoguerMessageEvent
	End Sub

	' Token: 0x060005C6 RID: 1478 RVA: 0x00069C4E File Offset: 0x0006804E
	Private Sub OnDialoguerMessageEvent(message As String, metadata As String)
		If message = "MetSaltbaker" Then
			PlayerData.SaveCurrentFile()
		End If
	End Sub

	' Token: 0x060005C7 RID: 1479 RVA: 0x00069C65 File Offset: 0x00068065
	Private Sub CheckIfBossesCompleted()
		If PlayerData.Data.CheckLevelsHaveMinDifficulty(Level.worldDLCBossLevels, Level.Mode.Normal) Then
			Me.trapDoorOpen = True
			MyBase.StartCoroutine(Me.check_trigger_cr())
		Else
			Me.trapDoorOpen = False
		End If
	End Sub

	' Token: 0x060005C8 RID: 1480 RVA: 0x00069C9C File Offset: 0x0006809C
	Protected Overrides Sub OnLevelStart()
		If Dialoguer.GetGlobalFloat(23) = 1F Then
			AudioManager.Play("sfx_dlc_bakery_doorenter")
		End If
		If Me.trapDoorOpen Then
			AudioManager.StartBGMAlternate(1)
		ElseIf PlayerData.Data.pianoAudioEnabled Then
			AudioManager.StartBGMAlternate(2)
		Else
			AudioManager.PlayBGM()
		End If
	End Sub

	' Token: 0x060005C9 RID: 1481 RVA: 0x00069CFC File Offset: 0x000680FC
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		If Me.triggerEndGame IsNot Nothing Then
			Dim vector As Vector2 = New Vector2(Me.triggerEndGame.position.x, Me.triggerEndGame.position.y + 1000F)
			Dim vector2 As Vector2 = New Vector2(Me.triggerEndGame.position.x, Me.triggerEndGame.position.y - 1000F)
			Gizmos.DrawLine(vector, vector2)
		End If
	End Sub

	' Token: 0x060005CA RID: 1482 RVA: 0x00069D9C File Offset: 0x0006819C
	Private Iterator Function check_trigger_cr() As IEnumerator
		Dim player As AbstractPlayerController = PlayerManager.GetPlayer(PlayerId.PlayerOne)
		Dim player2 As AbstractPlayerController = PlayerManager.GetPlayer(PlayerId.PlayerTwo)
		Dim hasntPassed As Boolean = True
		While hasntPassed
			If player.transform.position.x >= Me.triggerEndGame.position.x Then
				hasntPassed = False
			End If
			If player2 IsNot Nothing AndAlso player2.transform.position.x >= Me.triggerEndGame.position.x Then
				hasntPassed = False
			End If
			Yield Nothing
		End While
		PlayerManager.playerWasChalice(0) = player.stats.isChalice
		PlayerManager.playerWasChalice(1) = player2 IsNot Nothing AndAlso player2.stats.isChalice
		If Level.CurrentMode = Level.Mode.Easy Then
			Level.SetCurrentMode(Level.Mode.Normal)
		End If
		Cutscene.Load(Scenes.scene_level_saltbaker, Scenes.scene_cutscene_dlc_saltbaker_prebattle, SceneLoader.Transition.Fade, SceneLoader.Transition.Fade, SceneLoader.Icon.Hourglass)
		Return
	End Function

	' Token: 0x060005CB RID: 1483 RVA: 0x00069DB8 File Offset: 0x000681B8
	Private Iterator Function check_camera_cr() As IEnumerator
		Me.camera.mode = CupheadLevelCamera.Mode.Relative
		Dim inPit As Boolean = False
		Dim player As AbstractPlayerController = PlayerManager.GetPlayer(PlayerId.PlayerOne)
		Dim player2 As AbstractPlayerController = PlayerManager.GetPlayer(PlayerId.PlayerTwo)
		Dim lastP1YPos As Single = 0F
		Dim lastP2YPos As Single = 0F
		While Not inPit
			player = PlayerManager.GetPlayer(PlayerId.PlayerOne)
			player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo)
			If player2 IsNot Nothing AndAlso Not player2.IsDead Then
				inPit = player2.transform.position.y < -400F AndAlso player.transform.position.y < -400F
				If Not inPit Then
					If player.transform.position.y < -400F Then
						player.gameObject.SetActive(False)
					End If
					If player2.transform.position.y < -400F Then
						player2.gameObject.SetActive(False)
					End If
				End If
			Else
				inPit = player.transform.position.y < -400F
			End If
			If player IsNot Nothing AndAlso Mathf.Sign(player.transform.position.y + 208F) <> Mathf.Sign(lastP1YPos + 208F) Then
				For Each spriteRenderer As SpriteRenderer In player.GetComponentsInChildren(Of SpriteRenderer)()
					spriteRenderer.sortingLayerName = If((player.transform.position.y >= -208F), "Player", "Enemies")
				Next
				lastP1YPos = player.transform.position.y
			End If
			If player2 IsNot Nothing AndAlso Mathf.Sign(player2.transform.position.y + 208F) <> Mathf.Sign(lastP2YPos + 208F) Then
				For Each spriteRenderer2 As SpriteRenderer In player2.GetComponentsInChildren(Of SpriteRenderer)()
					spriteRenderer2.sortingLayerName = If((player2.transform.position.y >= -208F), "Player", "Enemies")
				Next
				lastP2YPos = player.transform.position.y
			End If
			Yield Nothing
		End While
		Me.kitchenBG.SetActive(False)
		Me.basementBG.SetActive(True)
		AudioManager.FadeSFXVolume("sfx_dlc_bakery_basementamb_loop", 0.0001F, 0.0001F)
		AudioManager.PlayLoop("sfx_dlc_bakery_basementamb_loop")
		AudioManager.PlayLoop("sfx_dlc_bakery_basementtorch_loop")
		Me.afterGettingIngredients.SetActive(False)
		CupheadLevelCamera.Current.ChangeHorizontalBounds(740, 3500)
		Level.Current.SetBounds(New Integer?(680), New Integer?(6860), Nothing, Nothing)
		CupheadLevelCamera.Current.ChangeCameraMode(CupheadLevelCamera.Mode.Lerp)
		CupheadLevelCamera.Current.LERP_SPEED = 5F
		CupheadLevelCamera.Current.SetPosition(New Vector3(-100F, 0F))
		player.transform.position = New Vector3(-500F, 800F)
		player.gameObject.SetActive(True)
		If player2 IsNot Nothing AndAlso Not player2.IsDead Then
			player2.transform.position = New Vector3(-400F, 800F)
			player2.gameObject.SetActive(True)
		End If
		For Each abstractPlayerController As AbstractPlayerController In PlayerManager.GetAllPlayers()
			If abstractPlayerController IsNot Nothing Then
				Me.SetPlayerBasementMaterial(abstractPlayerController.id)
				For Each spriteRenderer3 As SpriteRenderer In abstractPlayerController.GetComponentsInChildren(Of SpriteRenderer)()
					spriteRenderer3.sortingLayerName = "Player"
				Next
			End If
		Next
		AudioManager.StartBGMAlternate(0)
		AudioManager.FadeSFXVolume("sfx_dlc_bakery_basementamb_loop", 0.5F, 1F)
		While CupheadLevelCamera.Current.transform.position.x < 2320F
			Me.HandleTorchSFX()
			Yield Nothing
		End While
		Me.saltbakerShadow.SetTrigger("Continue")
		CupheadLevelCamera.Current.ChangeCameraMode(CupheadLevelCamera.Mode.Platforming)
		AudioManager.Play("sfx_dlc_saltbaker_evilbasementlaugh")
		While CupheadLevelCamera.Current.transform.position.x < 2800F
			Me.HandleTorchSFX()
			Yield Nothing
		End While
		Me.saltbakerShadow.SetTrigger("Continue")
		Return
	End Function

	' Token: 0x060005CC RID: 1484 RVA: 0x00069DD4 File Offset: 0x000681D4
	Private Sub HandleTorchSFX()
		Dim num As Single = Single.MaxValue
		Dim num2 As Single = 0F
		For Each transform As Transform In Me.torchPositions
			For Each abstractPlayerController As AbstractPlayerController In PlayerManager.GetAllPlayers()
				If abstractPlayerController IsNot Nothing Then
					Dim num3 As Single = Mathf.Abs(abstractPlayerController.center.x - transform.position.x)
					If num3 < num Then
						num2 = Mathf.Sign(transform.position.x - abstractPlayerController.center.x)
						num = num3
					End If
				End If
			Next
		Next
		Dim num4 As Single = Mathf.InverseLerp(320F, 0F, num)
		Dim num5 As Single = num2 * (1F - num4)
		AudioManager.FadeSFXVolume("sfx_dlc_bakery_basementtorch_loop", Mathf.Lerp(0.01F, 0.8F, num4), 0.0001F)
		AudioManager.Pan("sfx_dlc_bakery_basementtorch_loop", num5)
	End Sub

	' Token: 0x060005CD RID: 1485 RVA: 0x00069F0C File Offset: 0x0006830C
	Private Iterator Function cycle_sunbeams_cr() As IEnumerator
		Dim t As Single = 0F
		While True
			For i As Integer = 0 To 3 - 1
				Me.sunbeams(i).color = New Color(1F, 1F, 1F, (Mathf.Sin(CSng(i) * 2.0943952F + t) + 1F) / 2F)
			Next
			t += CupheadTime.Delta * Me.sunbeamCycleSpeed
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x04000A84 RID: 2692
	Private properties As LevelProperties.Kitchen

	' Token: 0x04000A85 RID: 2693
	Private Const DIALOGUER_VAR_ID As Integer = 23

	' Token: 0x04000A86 RID: 2694
	<Header("Boss Info")>
	<SerializeField()>
	Private _bossPortrait As Sprite

	' Token: 0x04000A87 RID: 2695
	<SerializeField()>
	<Multiline()>
	Private _bossQuote As String

	' Token: 0x04000A88 RID: 2696
	<SerializeField()>
	Private beforeGettingIngredients As GameObject

	' Token: 0x04000A89 RID: 2697
	<SerializeField()>
	Private afterGettingIngredients As GameObject

	' Token: 0x04000A8A RID: 2698
	<SerializeField()>
	Private sunbeams As SpriteRenderer()

	' Token: 0x04000A8B RID: 2699
	<SerializeField()>
	Private sunbeamCycleSpeed As Single = 2F

	' Token: 0x04000A8C RID: 2700
	<SerializeField()>
	Private saltbakerShadow As Animator

	' Token: 0x04000A8D RID: 2701
	<SerializeField()>
	Private triggerEndGame As Transform

	' Token: 0x04000A8E RID: 2702
	Private trapDoorOpen As Boolean

	' Token: 0x04000A8F RID: 2703
	<SerializeField()>
	Private trapDoorOverlay As SpriteRenderer

	' Token: 0x04000A90 RID: 2704
	Private forceUnlockSaltbakerBattle As Boolean

	' Token: 0x04000A91 RID: 2705
	<SerializeField()>
	Private kitchenBG As GameObject

	' Token: 0x04000A92 RID: 2706
	<SerializeField()>
	Private basementBG As GameObject

	' Token: 0x04000A93 RID: 2707
	<SerializeField()>
	Private playerBasementMaterial As Material

	' Token: 0x04000A94 RID: 2708
	<SerializeField()>
	Private torchPositions As Transform()
End Class
