Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000068 RID: 104
Public Class BeeLevel
	Inherits Level

	' Token: 0x0600010A RID: 266 RVA: 0x00056118 File Offset: 0x00054518
	Protected Overrides Sub PartialInit()
		Me.properties = LevelProperties.Bee.GetMode(MyBase.mode)
		AddHandler Me.properties.OnStateChange, AddressOf MyBase.zHack_OnStateChanged
		AddHandler Me.properties.OnBossDeath, AddressOf MyBase.zHack_OnWin
		MyBase.timeline = Me.properties.CreateTimeline(MyBase.mode)
		Me.goalTimes = Me.properties.goalTimes
		AddHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		MyBase.PartialInit()
	End Sub

	' Token: 0x17000028 RID: 40
	' (get) Token: 0x0600010B RID: 267 RVA: 0x000561AE File Offset: 0x000545AE
	Public Overrides ReadOnly Property CurrentLevel As Levels
		Get
			Return Levels.Bee
		End Get
	End Property

	' Token: 0x17000029 RID: 41
	' (get) Token: 0x0600010C RID: 268 RVA: 0x000561B5 File Offset: 0x000545B5
	Public Overrides ReadOnly Property CurrentScene As Scenes
		Get
			Return Scenes.scene_level_bee
		End Get
	End Property

	' Token: 0x1700002A RID: 42
	' (get) Token: 0x0600010D RID: 269 RVA: 0x000561B9 File Offset: 0x000545B9
	Public ReadOnly Property Speed As Single
		Get
			Return Me.speed * CupheadTime.GlobalSpeed
		End Get
	End Property

	' Token: 0x1700002B RID: 43
	' (get) Token: 0x0600010E RID: 270 RVA: 0x000561C7 File Offset: 0x000545C7
	Public ReadOnly Property MissingPlatformCount As Integer
		Get
			Return Me.missingPlatformCount
		End Get
	End Property

	' Token: 0x1700002C RID: 44
	' (get) Token: 0x0600010F RID: 271 RVA: 0x000561D0 File Offset: 0x000545D0
	Public Overrides ReadOnly Property BossPortrait As Sprite
		Get
			Select Case Me.properties.CurrentState.stateName
				Case LevelProperties.Bee.States.Main
					Return Me._bossPortraitGuard
				Case LevelProperties.Bee.States.Generic
					Return Me._bossPortraitMain
				Case LevelProperties.Bee.States.Airplane
					Return Me._bossPortraitAirplane
				Case Else
					Global.Debug.LogError("Couldn't find portrait for state " + Me.properties.CurrentState.stateName + ". Using Main.", Nothing)
					Return Me._bossPortraitMain
			End Select
		End Get
	End Property

	' Token: 0x1700002D RID: 45
	' (get) Token: 0x06000110 RID: 272 RVA: 0x0005624C File Offset: 0x0005464C
	Public Overrides ReadOnly Property BossQuote As String
		Get
			Select Case Me.properties.CurrentState.stateName
				Case LevelProperties.Bee.States.Main
					Return Me._bossQuoteGuard
				Case LevelProperties.Bee.States.Generic
					Return Me._bossQuoteMain
				Case LevelProperties.Bee.States.Airplane
					Return Me._bossQuoteAirplane
				Case Else
					Global.Debug.LogError("Couldn't find quote for state " + Me.properties.CurrentState.stateName + ". Using Main.", Nothing)
					Return Me._bossQuoteMain
			End Select
		End Get
	End Property

	' Token: 0x06000111 RID: 273 RVA: 0x000562C8 File Offset: 0x000546C8
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.StartCoroutine(Me.drip_cr())
		Me.queen.LevelInit(Me.properties)
		Me.guard.LevelInit(Me.properties)
		Me.background.LevelInit(Me.properties)
		Me.airplane.LevelInit(Me.properties)
	End Sub

	' Token: 0x06000112 RID: 274 RVA: 0x0005632C File Offset: 0x0005472C
	Protected Overrides Sub Update()
		MyBase.Update()
		Me.UpdateSpeed()
	End Sub

	' Token: 0x06000113 RID: 275 RVA: 0x0005633C File Offset: 0x0005473C
	Protected Overrides Sub CreatePlayers()
		MyBase.CreatePlayers()
		If PlayerManager.Multiplayer AndAlso Me.allowMultiplayer AndAlso Me.players(1).stats.isChalice Then
			Me.players(1).transform.position = Me.p2ChaliceSpawnPoint
		End If
	End Sub

	' Token: 0x06000114 RID: 276 RVA: 0x00056398 File Offset: 0x00054798
	Protected Overrides Sub OnLevelStart()
		Me.missingPlatformCount = Me.properties.CurrentState.movement.missingPlatforms
		Me.targetSpeed = -Me.properties.CurrentState.movement.speed
		MyBase.StartCoroutine(Me.beePattern_cr())
		Me.CheckGrunts()
	End Sub

	' Token: 0x06000115 RID: 277 RVA: 0x000563EF File Offset: 0x000547EF
	Protected Overrides Sub OnStateChanged()
		MyBase.OnStateChanged()
		If Me.properties.CurrentState.stateName = LevelProperties.Bee.States.Airplane Then
			MyBase.StartCoroutine(Me.airplane_cr())
		End If
	End Sub

	' Token: 0x06000116 RID: 278 RVA: 0x0005641A File Offset: 0x0005481A
	Private Sub UpdateSpeed()
		Me.speed = Mathf.Lerp(Me.speed, Me.targetSpeed, 0.5F * CupheadTime.Delta)
	End Sub

	' Token: 0x06000117 RID: 279 RVA: 0x00056443 File Offset: 0x00054843
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.prefabs = Nothing
		Me._bossPortraitAirplane = Nothing
		Me._bossPortraitGuard = Nothing
		Me._bossPortraitMain = Nothing
	End Sub

	' Token: 0x06000118 RID: 280 RVA: 0x00056468 File Offset: 0x00054868
	Private Sub CheckGrunts()
		If Me.gruntCoroutine IsNot Nothing Then
			MyBase.StopCoroutine(Me.gruntCoroutine)
		End If
		If Me.properties.CurrentState.grunts.active Then
			Me.gruntCoroutine = MyBase.StartCoroutine(Me.grunts_cr())
		End If
	End Sub

	' Token: 0x06000119 RID: 281 RVA: 0x000564B8 File Offset: 0x000548B8
	Private Iterator Function beePattern_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		While True
			Yield MyBase.StartCoroutine(Me.nextPattern_cr())
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600011A RID: 282 RVA: 0x000564D4 File Offset: 0x000548D4
	Private Iterator Function nextPattern_cr() As IEnumerator
		Select Case Me.properties.CurrentState.NextPattern
			Case LevelProperties.Bee.Pattern.BlackHole
				Yield MyBase.StartCoroutine(Me.blackHole_cr())
			Case LevelProperties.Bee.Pattern.Chain
				Yield MyBase.StartCoroutine(Me.chain_cr())
			Case LevelProperties.Bee.Pattern.Triangle
				Yield MyBase.StartCoroutine(Me.triangle_cr())
			Case LevelProperties.Bee.Pattern.Follower
				Yield MyBase.StartCoroutine(Me.follower_cr())
			Case LevelProperties.Bee.Pattern.SecurityGuard
				Yield MyBase.StartCoroutine(Me.security_cr())
			Case LevelProperties.Bee.Pattern.Wing
				Yield MyBase.StartCoroutine(Me.wing_cr())
			Case LevelProperties.Bee.Pattern.Turbine
				Yield MyBase.StartCoroutine(Me.turbine_cr())
			Case Else
				Yield CupheadTime.WaitForSeconds(Me, 1F)
		End Select
		Return
	End Function

	' Token: 0x0600011B RID: 283 RVA: 0x000564F0 File Offset: 0x000548F0
	Private Iterator Function airplane_cr() As IEnumerator
		While Me.queen.state <> BeeLevelQueen.State.Idle
			Yield Nothing
		End While
		Me.queen.StartMorph()
		Me.honeyDripping = False
		Me.targetSpeed = -Me.properties.CurrentState.general.screenScrollSpeed
		While Me.queen.state <> BeeLevelQueen.State.Idle
			Yield Nothing
		End While
		Yield Nothing
		Return
	End Function

	' Token: 0x0600011C RID: 284 RVA: 0x0005650C File Offset: 0x0005490C
	Private Iterator Function turbine_cr() As IEnumerator
		While Me.airplane.state <> BeeLevelAirplane.State.Idle
			Yield Nothing
		End While
		Me.airplane.StartTurbine()
		While Me.airplane.state <> BeeLevelAirplane.State.Idle
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600011D RID: 285 RVA: 0x00056528 File Offset: 0x00054928
	Private Iterator Function wing_cr() As IEnumerator
		While Me.airplane.state <> BeeLevelAirplane.State.Idle
			Yield Nothing
		End While
		Me.airplane.StartWing()
		While Me.airplane.state <> BeeLevelAirplane.State.Idle
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600011E RID: 286 RVA: 0x00056544 File Offset: 0x00054944
	Private Iterator Function security_cr() As IEnumerator
		Me.guard.StartSecurityGuard()
		While Me.guard.state <> BeeLevelSecurityGuard.State.Ready
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600011F RID: 287 RVA: 0x00056560 File Offset: 0x00054960
	Private Iterator Function blackHole_cr() As IEnumerator
		Me.queen.StartBlackHole()
		While Me.queen.state <> BeeLevelQueen.State.Idle
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000120 RID: 288 RVA: 0x0005657C File Offset: 0x0005497C
	Private Iterator Function triangle_cr() As IEnumerator
		Me.queen.StartTriangle()
		While Me.queen.state <> BeeLevelQueen.State.Idle
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000121 RID: 289 RVA: 0x00056598 File Offset: 0x00054998
	Private Iterator Function follower_cr() As IEnumerator
		Me.queen.StartFollower()
		While Me.queen.state <> BeeLevelQueen.State.Idle
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000122 RID: 290 RVA: 0x000565B4 File Offset: 0x000549B4
	Private Iterator Function chain_cr() As IEnumerator
		Me.queen.StartChain()
		While Me.queen.state <> BeeLevelQueen.State.Idle
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000123 RID: 291 RVA: 0x000565D0 File Offset: 0x000549D0
	Private Iterator Function drip_cr() As IEnumerator
		While Me.honeyDripping
			Yield CupheadTime.WaitForSeconds(Me, CSng(Global.UnityEngine.Random.Range(1, 3)))
			Me.prefabs.drip.Create()
		End While
		Return
	End Function

	' Token: 0x06000124 RID: 292 RVA: 0x000565EC File Offset: 0x000549EC
	Private Iterator Function grunts_cr() As IEnumerator
		Dim strings As String() = Me.properties.CurrentState.grunts.entrancePoints(Global.UnityEngine.Random.Range(0, Me.properties.CurrentState.grunts.entrancePoints.Length)).Split(New Char() { ","c })
		Dim positions As Integer() = New Integer(strings.Length - 1) {}
		For i As Integer = 0 To strings.Length - 1
			Parser.IntTryParse(strings(i), positions(i))
			positions(i) = Mathf.Clamp(positions(i), 0, Me.gruntRoots.Length)
		Next
		Dim index As Integer = Global.UnityEngine.Random.Range(0, positions.Length)
		While True
			Yield CupheadTime.WaitForSeconds(Me, Me.properties.CurrentState.grunts.delay)
			Dim scale As Integer = If((PlayerManager.Center.x <= 0F), 1, (-1))
			If PlayerManager.Center.x > 0F Then
				scale = -1
			End If
			Me.prefabs.grunt.Create(Me.gruntRoots(positions(index)).position + New Vector3(CSng((840 * scale)), 0F, 0F), scale, Me.properties.CurrentState.grunts.health, Me.properties.CurrentState.grunts.speed)
			index = CInt(Mathf.Repeat(CSng((index + 1)), CSng(positions.Length)))
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x04000276 RID: 630
	Private properties As LevelProperties.Bee

	' Token: 0x04000277 RID: 631
	Private Const SPEED_TIME As Single = 0.5F

	' Token: 0x04000278 RID: 632
	<SerializeField()>
	Private p2ChaliceSpawnPoint As Vector2

	' Token: 0x04000279 RID: 633
	<SerializeField()>
	Private airplane As BeeLevelAirplane

	' Token: 0x0400027A RID: 634
	<Space(10F)>
	<SerializeField()>
	Private queen As BeeLevelQueen

	' Token: 0x0400027B RID: 635
	<SerializeField()>
	Private guard As BeeLevelSecurityGuard

	' Token: 0x0400027C RID: 636
	<Space(10F)>
	<SerializeField()>
	Private gruntRoots As Transform()

	' Token: 0x0400027D RID: 637
	<Space(10F)>
	<SerializeField()>
	Private background As BeeLevelBackground

	' Token: 0x0400027E RID: 638
	<Space(10F)>
	<SerializeField()>
	Private prefabs As BeeLevel.Prefabs

	' Token: 0x0400027F RID: 639
	Private honeyDripping As Boolean = True

	' Token: 0x04000280 RID: 640
	Private speed As Single

	' Token: 0x04000281 RID: 641
	Private targetSpeed As Single

	' Token: 0x04000282 RID: 642
	Private missingPlatformCount As Integer

	' Token: 0x04000283 RID: 643
	<Header("Boss Info")>
	<SerializeField()>
	Private _bossPortraitGuard As Sprite

	' Token: 0x04000284 RID: 644
	<SerializeField()>
	Private _bossPortraitMain As Sprite

	' Token: 0x04000285 RID: 645
	<SerializeField()>
	Private _bossPortraitAirplane As Sprite

	' Token: 0x04000286 RID: 646
	<SerializeField()>
	Private _bossQuoteGuard As String

	' Token: 0x04000287 RID: 647
	<SerializeField()>
	Private _bossQuoteMain As String

	' Token: 0x04000288 RID: 648
	<SerializeField()>
	Private _bossQuoteAirplane As String

	' Token: 0x04000289 RID: 649
	Private gruntCoroutine As Coroutine

	' Token: 0x0200050D RID: 1293
	<Serializable()>
	Public Class Prefabs
		' Token: 0x04002038 RID: 8248
		Public grunt As BeeLevelGrunt

		' Token: 0x04002039 RID: 8249
		Public drip As BeeLevelHoneyDrip
	End Class
End Class
