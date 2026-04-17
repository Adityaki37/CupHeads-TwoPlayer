Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000172 RID: 370
Public Class DragonLevel
	Inherits Level

	' Token: 0x0600042D RID: 1069 RVA: 0x00061DA8 File Offset: 0x000601A8
	Protected Overrides Sub PartialInit()
		Me.properties = LevelProperties.Dragon.GetMode(MyBase.mode)
		AddHandler Me.properties.OnStateChange, AddressOf MyBase.zHack_OnStateChanged
		AddHandler Me.properties.OnBossDeath, AddressOf MyBase.zHack_OnWin
		MyBase.timeline = Me.properties.CreateTimeline(MyBase.mode)
		Me.goalTimes = Me.properties.goalTimes
		AddHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		MyBase.PartialInit()
	End Sub

	' Token: 0x170000C7 RID: 199
	' (get) Token: 0x0600042E RID: 1070 RVA: 0x00061E3E File Offset: 0x0006023E
	Public Overrides ReadOnly Property CurrentLevel As Levels
		Get
			Return Levels.Dragon
		End Get
	End Property

	' Token: 0x170000C8 RID: 200
	' (get) Token: 0x0600042F RID: 1071 RVA: 0x00061E45 File Offset: 0x00060245
	Public Overrides ReadOnly Property CurrentScene As Scenes
		Get
			Return Scenes.scene_level_dragon
		End Get
	End Property

	' Token: 0x170000C9 RID: 201
	' (get) Token: 0x06000430 RID: 1072 RVA: 0x00061E49 File Offset: 0x00060249
	' (set) Token: 0x06000431 RID: 1073 RVA: 0x00061E50 File Offset: 0x00060250
	Public Shared Property SPEED As Single

	' Token: 0x170000CA RID: 202
	' (get) Token: 0x06000432 RID: 1074 RVA: 0x00061E58 File Offset: 0x00060258
	Public Overrides ReadOnly Property BossPortrait As Sprite
		Get
			Select Case Me.properties.CurrentState.stateName
				Case LevelProperties.Dragon.States.Main, LevelProperties.Dragon.States.Generic
					Return Me._bossPortraitMain
				Case LevelProperties.Dragon.States.ThreeHeads
					Return Me._bossPortraitThreeHeads
				Case LevelProperties.Dragon.States.FireMarchers
					Return Me._bossPortraitFireMarchers
				Case Else
					Global.Debug.LogError("Couldn't find portrait for state " + Me.properties.CurrentState.stateName + ". Using Main.", Nothing)
					Return Me._bossPortraitMain
			End Select
		End Get
	End Property

	' Token: 0x170000CB RID: 203
	' (get) Token: 0x06000433 RID: 1075 RVA: 0x00061ED8 File Offset: 0x000602D8
	Public Overrides ReadOnly Property BossQuote As String
		Get
			Select Case Me.properties.CurrentState.stateName
				Case LevelProperties.Dragon.States.Main, LevelProperties.Dragon.States.Generic
					Return Me._bossQuoteMain
				Case LevelProperties.Dragon.States.ThreeHeads
					Return Me._bossQuoteThreeHeads
				Case LevelProperties.Dragon.States.FireMarchers
					Return Me._bossQuoteFireMarchers
				Case Else
					Global.Debug.LogError("Couldn't find quote for state " + Me.properties.CurrentState.stateName + ". Using Main.", Nothing)
					Return Me._bossQuoteMain
			End Select
		End Get
	End Property

	' Token: 0x06000434 RID: 1076 RVA: 0x00061F56 File Offset: 0x00060356
	Protected Overrides Sub Awake()
		MyBase.Awake()
		DragonLevel.SPEED = 0F
	End Sub

	' Token: 0x06000435 RID: 1077 RVA: 0x00061F68 File Offset: 0x00060368
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.dragon.LevelInit(Me.properties)
		Me.tail.LevelInit(Me.properties)
		Me.leftSideDragon.LevelInit(Me.properties)
	End Sub

	' Token: 0x06000436 RID: 1078 RVA: 0x00061FA4 File Offset: 0x000603A4
	Protected Overrides Sub OnLevelStart()
		MyBase.StartCoroutine(Me.speed_cr())
		MyBase.StartCoroutine(Me.tail_cr())
		MyBase.StartCoroutine(Me.dragonPattern_cr())
		Me.manager.Init(Me.properties.CurrentState.clouds)
		Me.SetPlatformVariables(True)
		Me.cloudsSameDir = Me.properties.CurrentState.clouds.movingRight
	End Sub

	' Token: 0x06000437 RID: 1079 RVA: 0x00062018 File Offset: 0x00060418
	Protected Overrides Sub OnStateChanged()
		MyBase.OnStateChanged()
		Me.manager.UpdateProperties(Me.properties.CurrentState.clouds)
		Me.SetPlatformVariables(False)
		If Me.properties.CurrentState.clouds.movingRight <> Me.cloudsSameDir Then
			MyBase.StartCoroutine(Me.speed_cr())
			Me.cloudsSameDir = Me.properties.CurrentState.clouds.movingRight
		End If
		If Me.properties.CurrentState.stateName = LevelProperties.Dragon.States.FireMarchers Then
			Me.StopAllCoroutines()
			Me.dragon.Leave()
		ElseIf Me.properties.CurrentState.stateName = LevelProperties.Dragon.States.ThreeHeads Then
			Me.StopAllCoroutines()
			Me.leftSideDragon.StartThreeHeads()
			MyBase.StartCoroutine(Me.phase3ColorTransition())
		End If
	End Sub

	' Token: 0x06000438 RID: 1080 RVA: 0x000620F8 File Offset: 0x000604F8
	Private Sub SetPlatformVariables(firstTime As Boolean)
		For Each dragonLevelCloudPlatform As DragonLevelCloudPlatform In Me.platforms
			dragonLevelCloudPlatform.GetProperties(Me.properties.CurrentState.clouds, firstTime)
		Next
	End Sub

	' Token: 0x06000439 RID: 1081 RVA: 0x0006213B File Offset: 0x0006053B
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me._bossPortraitMain = Nothing
		Me._bossPortraitThreeHeads = Nothing
		Me._bossPortraitFireMarchers = Nothing
	End Sub

	' Token: 0x0600043A RID: 1082 RVA: 0x00062158 File Offset: 0x00060558
	Private Iterator Function dragonPattern_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		While True
			Yield MyBase.StartCoroutine(Me.nextPattern_cr())
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600043B RID: 1083 RVA: 0x00062174 File Offset: 0x00060574
	Private Iterator Function nextPattern_cr() As IEnumerator
		Dim p As LevelProperties.Dragon.Pattern = Me.properties.CurrentState.NextPattern
		If p <> LevelProperties.Dragon.Pattern.Meteor Then
			If p <> LevelProperties.Dragon.Pattern.Peashot Then
				Yield CupheadTime.WaitForSeconds(Me, 1F)
			Else
				Yield MyBase.StartCoroutine(Me.peashot_cr())
			End If
		Else
			Yield MyBase.StartCoroutine(Me.meteor_cr())
		End If
		Return
	End Function

	' Token: 0x0600043C RID: 1084 RVA: 0x00062190 File Offset: 0x00060590
	Private Iterator Function meteor_cr() As IEnumerator
		While Me.dragon.state <> DragonLevelDragon.State.Idle
			Yield Nothing
		End While
		Me.dragon.StartMeteor()
		While Me.dragon.state <> DragonLevelDragon.State.Idle
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600043D RID: 1085 RVA: 0x000621AC File Offset: 0x000605AC
	Private Iterator Function peashot_cr() As IEnumerator
		While Me.dragon.state <> DragonLevelDragon.State.Idle
			Yield Nothing
		End While
		Me.dragon.StartPeashot()
		While Me.dragon.state <> DragonLevelDragon.State.Idle
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600043E RID: 1086 RVA: 0x000621C8 File Offset: 0x000605C8
	Private Iterator Function speed_cr() As IEnumerator
		Dim t As Single = 0F
		While t < 3F
			DragonLevel.SPEED = t / 3F
			t += CupheadTime.Delta
			Yield Nothing
		End While
		DragonLevel.SPEED = 1F
		Return
	End Function

	' Token: 0x0600043F RID: 1087 RVA: 0x000621DC File Offset: 0x000605DC
	Private Iterator Function tail_cr() As IEnumerator
		While Me.dragon.state <> DragonLevelDragon.State.Idle
			Yield Nothing
		End While
		While True
			While Not Me.properties.CurrentState.tail.active
				Yield Nothing
			End While
			Dim tailProperties As LevelProperties.Dragon.Tail = Me.properties.CurrentState.tail
			Me.tail.TailStart(tailProperties.warningTime, tailProperties.inTime, tailProperties.holdTime, tailProperties.outTime)
			While Me.tail.state <> DragonLevelTail.State.Idle
				Yield Nothing
			End While
			Yield CupheadTime.WaitForSeconds(Me, tailProperties.attackDelay.RandomFloat())
		End While
		Return
	End Function

	' Token: 0x06000440 RID: 1088 RVA: 0x000621F8 File Offset: 0x000605F8
	Private Iterator Function phase3ColorTransition() As IEnumerator
		While Me.leftSideDragon.state <> DragonLevelLeftSideDragon.State.ThreeHeads
			Yield Nothing
		End While
		MyBase.StartCoroutine(Me.lightning_cr())
		Dim t As Single = 0F
		Dim fadeTime As Single = 6F
		Dim lastLightningState As DragonLevel.LightningState = Me.lightningState
		Dim dragonHitFlash As HitFlash = Me.leftSideDragon.GetComponentInChildren(Of HitFlash)()
		Me.dragonMaterial = Me.leftSideDragon.GetComponent(Of SpriteRenderer)().material
		While True
			Dim playerOne As LevelPlayerController = PlayerManager.GetPlayer(Of LevelPlayerController)(PlayerId.PlayerOne)
			Dim playerTwo As LevelPlayerController = PlayerManager.GetPlayer(Of LevelPlayerController)(PlayerId.PlayerTwo)
			Dim ratio As Single = Mathf.Min(1F, t / fadeTime)
			Dim playerColor As Color
			Dim projectileColor As Color
			Dim dragonColor As Color
			Dim darkSpireColor As Color
			Dim platformColor As Color
			If Me.lightningState = DragonLevel.LightningState.FirstFlash Then
				playerColor = ColorUtils.HexToColor("333333")
				projectileColor = ColorUtils.HexToColor("333333")
				dragonColor = ColorUtils.HexToColor("333333")
				darkSpireColor = New Color(0.2F, 0.2F, 0.2F, Me.darkSpire.color.a)
				platformColor = ColorUtils.HexToColor("191919")
			ElseIf Me.lightningState = DragonLevel.LightningState.SecondFlash Then
				playerColor = ColorUtils.HexToColor("191919")
				projectileColor = ColorUtils.HexToColor("191919")
				dragonColor = ColorUtils.HexToColor("191919")
				darkSpireColor = New Color(0.1F, 0.1F, 0.1F, Me.darkSpire.color.a)
				platformColor = ColorUtils.HexToColor("0c0c0c")
			Else
				playerColor = Color.Lerp(Color.white, ColorUtils.HexToColor("d8d8d8"), ratio)
				projectileColor = Color.Lerp(Color.white, ColorUtils.HexToColor("d8d8d8"), ratio)
				dragonColor = Color.black
				darkSpireColor = New Color(1F, 1F, 1F, Me.darkSpire.color.a)
				platformColor = Color.Lerp(Color.white, ColorUtils.HexToColor("9c9da63"), ratio)
			End If
			If playerOne IsNot Nothing Then
				playerOne.animationController.SetColor(playerColor)
			End If
			If playerTwo IsNot Nothing Then
				playerTwo.animationController.SetColor(playerColor)
			End If
			Me.darkSpire.color = darkSpireColor
			If Me.lightningState <> lastLightningState Then
				Dim array As GameObject() = GameObject.FindGameObjectsWithTag("PlayerProjectile")
				For Each gameObject As GameObject In array
					Dim component As SpriteRenderer = gameObject.GetComponent(Of SpriteRenderer)()
					If component IsNot Nothing Then
						component.color = projectileColor
					End If
				Next
			End If
			If Not dragonHitFlash.flashing Then
				For Each spriteRenderer As SpriteRenderer In Me.leftSideDragon.GetComponentsInChildren(Of SpriteRenderer)()
					spriteRenderer.material = If((Me.lightningState <> DragonLevel.LightningState.Off), Me.dragonFlashMaterial, Me.dragonMaterial)
					spriteRenderer.color = dragonColor
				Next
			End If
			For Each dragonLevelCloudPlatform As DragonLevelCloudPlatform In Me.manager.platforms
				dragonLevelCloudPlatform.GetComponent(Of SpriteRenderer)().color = platformColor
				dragonLevelCloudPlatform.top.color = platformColor
			Next
			For k As Integer = 0 To Me.lightningFlashes.Length - 1
				If Me.lightningState = DragonLevel.LightningState.FirstFlash Then
					Me.lightningFlashes(k).SetFlash1()
				ElseIf Me.lightningState = DragonLevel.LightningState.SecondFlash Then
					Me.lightningFlashes(k).SetFlash2()
				ElseIf Me.lightningState = DragonLevel.LightningState.Off Then
					Me.lightningFlashes(k).SetNormal()
				End If
			Next
			t += CupheadTime.Delta
			lastLightningState = Me.lightningState
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000441 RID: 1089 RVA: 0x00062214 File Offset: 0x00060614
	Private Iterator Function lightning_cr() As IEnumerator
		While True
			Me.lightningState = DragonLevel.LightningState.Off
			Yield CupheadTime.WaitForSeconds(Me, MathUtils.ExpRandom(2F) + 1F)
			Me.lightningStrikes.PlayLightning()
			Dim rand As Single = Global.UnityEngine.Random.value
			If rand < 0.25F Then
				Me.lightningState = DragonLevel.LightningState.FirstFlash
				Yield CupheadTime.WaitForSeconds(Me, 0.041F)
			ElseIf rand < 0.5F Then
				Me.lightningState = DragonLevel.LightningState.SecondFlash
				Yield CupheadTime.WaitForSeconds(Me, 0.041F)
			Else
				Me.lightningState = DragonLevel.LightningState.FirstFlash
				Yield CupheadTime.WaitForSeconds(Me, 0.041F)
				Me.lightningState = DragonLevel.LightningState.Off
				Yield CupheadTime.WaitForSeconds(Me, 0.041F)
				Me.lightningState = DragonLevel.LightningState.SecondFlash
				Yield CupheadTime.WaitForSeconds(Me, 0.041F)
			End If
		End While
		Return
	End Function

	' Token: 0x040006FA RID: 1786
	Private properties As LevelProperties.Dragon

	' Token: 0x040006FB RID: 1787
	Private Const Flash1Probability As Single = 0.25F

	' Token: 0x040006FC RID: 1788
	Private Const Flash2Probability As Single = 0.25F

	' Token: 0x040006FE RID: 1790
	<SerializeField()>
	Private lightningFlashes As DragonLevelBackgroundFlash()

	' Token: 0x040006FF RID: 1791
	<SerializeField()>
	Private platforms As DragonLevelCloudPlatform()

	' Token: 0x04000700 RID: 1792
	<SerializeField()>
	Private spire As SpriteRenderer

	' Token: 0x04000701 RID: 1793
	<SerializeField()>
	Private darkSpire As SpriteRenderer

	' Token: 0x04000702 RID: 1794
	<SerializeField()>
	Private dragon As DragonLevelDragon

	' Token: 0x04000703 RID: 1795
	<SerializeField()>
	Private leftSideDragon As DragonLevelLeftSideDragon

	' Token: 0x04000704 RID: 1796
	<SerializeField()>
	Private tail As DragonLevelTail

	' Token: 0x04000705 RID: 1797
	<SerializeField()>
	Private manager As DragonLevelPlatformManager

	' Token: 0x04000706 RID: 1798
	<SerializeField()>
	Private lightningStrikes As DragonLevelLightning

	' Token: 0x04000707 RID: 1799
	<SerializeField()>
	Private dragonFlashMaterial As Material

	' Token: 0x04000708 RID: 1800
	Private lightningState As DragonLevel.LightningState

	' Token: 0x04000709 RID: 1801
	<SerializeField()>
	Private backgroundClouds As SpriteRenderer()

	' Token: 0x0400070A RID: 1802
	<Header("Boss Info")>
	<SerializeField()>
	Private _bossPortraitMain As Sprite

	' Token: 0x0400070B RID: 1803
	<SerializeField()>
	Private _bossPortraitFireMarchers As Sprite

	' Token: 0x0400070C RID: 1804
	<SerializeField()>
	Private _bossPortraitThreeHeads As Sprite

	' Token: 0x0400070D RID: 1805
	<SerializeField()>
	Private _bossQuoteMain As String

	' Token: 0x0400070E RID: 1806
	<SerializeField()>
	Private _bossQuoteFireMarchers As String

	' Token: 0x0400070F RID: 1807
	<SerializeField()>
	Private _bossQuoteThreeHeads As String

	' Token: 0x04000710 RID: 1808
	Private cloudsSameDir As Boolean

	' Token: 0x04000711 RID: 1809
	Private dragonMaterial As Material

	' Token: 0x020005E8 RID: 1512
	Private Enum LightningState
		' Token: 0x040026CF RID: 9935
		Off
		' Token: 0x040026D0 RID: 9936
		FirstFlash
		' Token: 0x040026D1 RID: 9937
		SecondFlash
	End Enum

	' Token: 0x020005E9 RID: 1513
	<Serializable()>
	Public Class Prefabs
	End Class
End Class
