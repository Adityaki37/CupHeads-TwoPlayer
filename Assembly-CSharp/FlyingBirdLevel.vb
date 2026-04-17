Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000192 RID: 402
Public Class FlyingBirdLevel
	Inherits Level

	' Token: 0x0600047D RID: 1149 RVA: 0x000638EC File Offset: 0x00061CEC
	Protected Overrides Sub PartialInit()
		Me.properties = LevelProperties.FlyingBird.GetMode(MyBase.mode)
		AddHandler Me.properties.OnStateChange, AddressOf MyBase.zHack_OnStateChanged
		AddHandler Me.properties.OnBossDeath, AddressOf MyBase.zHack_OnWin
		MyBase.timeline = Me.properties.CreateTimeline(MyBase.mode)
		Me.goalTimes = Me.properties.goalTimes
		AddHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		MyBase.PartialInit()
	End Sub

	' Token: 0x170000D2 RID: 210
	' (get) Token: 0x0600047E RID: 1150 RVA: 0x00063982 File Offset: 0x00061D82
	Public Overrides ReadOnly Property CurrentLevel As Levels
		Get
			Return Levels.FlyingBird
		End Get
	End Property

	' Token: 0x170000D3 RID: 211
	' (get) Token: 0x0600047F RID: 1151 RVA: 0x00063989 File Offset: 0x00061D89
	Public Overrides ReadOnly Property CurrentScene As Scenes
		Get
			Return Scenes.scene_level_flying_bird
		End Get
	End Property

	' Token: 0x170000D4 RID: 212
	' (get) Token: 0x06000480 RID: 1152 RVA: 0x00063990 File Offset: 0x00061D90
	Public Overrides ReadOnly Property BossPortrait As Sprite
		Get
			Select Case Me.properties.CurrentState.stateName
				Case LevelProperties.FlyingBird.States.Main, LevelProperties.FlyingBird.States.Generic, LevelProperties.FlyingBird.States.Whistle
					Return Me._bossPortraitMain
				Case LevelProperties.FlyingBird.States.HouseDeath
					Return Me._bossPortraitHouseDeath
				Case LevelProperties.FlyingBird.States.BirdRevival
					Return Me._bossPortraitBirdRevival
				Case Else
					Global.Debug.LogError("Couldn't find portrait for state " + Me.properties.CurrentState.stateName + ". Using Main.", Nothing)
					Return Me._bossPortraitMain
			End Select
		End Get
	End Property

	' Token: 0x170000D5 RID: 213
	' (get) Token: 0x06000481 RID: 1153 RVA: 0x00063A14 File Offset: 0x00061E14
	Public Overrides ReadOnly Property BossQuote As String
		Get
			Select Case Me.properties.CurrentState.stateName
				Case LevelProperties.FlyingBird.States.Main, LevelProperties.FlyingBird.States.Generic, LevelProperties.FlyingBird.States.Whistle
					Return Me._bossQuoteMain
				Case LevelProperties.FlyingBird.States.HouseDeath
					Return Me._bossQuoteHouseDeath
				Case LevelProperties.FlyingBird.States.BirdRevival
					Return Me._bossQuoteBirdRevival
				Case Else
					Global.Debug.LogError("Couldn't find quote for state " + Me.properties.CurrentState.stateName + ". Using Main.", Nothing)
					Return Me._bossQuoteMain
			End Select
		End Get
	End Property

	' Token: 0x06000482 RID: 1154 RVA: 0x00063A96 File Offset: 0x00061E96
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.bird.LevelInit(Me.properties)
		Me.smallBird.LevelInit(Me.properties)
		Me.skybirdPattern = Me.skybirdPattern_cr()
	End Sub

	' Token: 0x06000483 RID: 1155 RVA: 0x00063ACC File Offset: 0x00061ECC
	Protected Overrides Sub OnLevelStart()
		Me.bird.IntroContinue()
		MyBase.StartCoroutine(Me.skybirdPattern_cr())
		MyBase.StartCoroutine(Me.enemies_cr())
		MyBase.StartCoroutine(Me.turrets_cr())
	End Sub

	' Token: 0x06000484 RID: 1156 RVA: 0x00063B00 File Offset: 0x00061F00
	Protected Overrides Sub OnStateChanged()
		MyBase.OnStateChanged()
		If Me.properties.CurrentState.stateName = LevelProperties.FlyingBird.States.HouseDeath Then
			MyBase.StopCoroutine(Me.skybirdPattern)
			MyBase.StartCoroutine(Me.houseDie_cr())
		ElseIf Me.properties.CurrentState.stateName = LevelProperties.FlyingBird.States.BirdRevival Then
			MyBase.StopCoroutine(Me.skybirdPattern)
			MyBase.StartCoroutine(Me.birdHouseRevival_cr())
		End If
	End Sub

	' Token: 0x06000485 RID: 1157 RVA: 0x00063B76 File Offset: 0x00061F76
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me._bossPortraitBirdRevival = Nothing
		Me._bossPortraitHouseDeath = Nothing
		Me._bossPortraitMain = Nothing
	End Sub

	' Token: 0x06000486 RID: 1158 RVA: 0x00063B94 File Offset: 0x00061F94
	Private Iterator Function skybirdPattern_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		While True
			Yield MyBase.StartCoroutine(Me.nextPattern_cr())
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000487 RID: 1159 RVA: 0x00063BB0 File Offset: 0x00061FB0
	Private Iterator Function nextPattern_cr() As IEnumerator
		Select Case Me.properties.CurrentState.NextPattern
			Case LevelProperties.FlyingBird.Pattern.Feathers
				Yield MyBase.StartCoroutine(Me.feathers_cr())
			Case LevelProperties.FlyingBird.Pattern.Eggs
				Yield MyBase.StartCoroutine(Me.eggs_cr())
			Case LevelProperties.FlyingBird.Pattern.Lasers
				Yield MyBase.StartCoroutine(Me.lasers_cr())
			Case Else
				Yield CupheadTime.WaitForSeconds(Me, 1F)
			Case LevelProperties.FlyingBird.Pattern.Garbage
				Yield MyBase.StartCoroutine(Me.garbage_cr())
			Case LevelProperties.FlyingBird.Pattern.Heart
				Yield MyBase.StartCoroutine(Me.heartAttack_cr())
		End Select
		Return
	End Function

	' Token: 0x06000488 RID: 1160 RVA: 0x00063BCC File Offset: 0x00061FCC
	Private Iterator Function feathers_cr() As IEnumerator
		While Me.bird.state <> FlyingBirdLevelBird.State.Idle
			Yield Nothing
		End While
		Me.bird.StartFeathers()
		While Me.bird.state <> FlyingBirdLevelBird.State.Idle
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000489 RID: 1161 RVA: 0x00063BE8 File Offset: 0x00061FE8
	Private Iterator Function eggs_cr() As IEnumerator
		While Me.bird.state <> FlyingBirdLevelBird.State.Idle
			Yield Nothing
		End While
		Me.bird.StartEggs()
		While Me.bird.state <> FlyingBirdLevelBird.State.Idle
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600048A RID: 1162 RVA: 0x00063C04 File Offset: 0x00062004
	Private Iterator Function lasers_cr() As IEnumerator
		While Me.bird.state <> FlyingBirdLevelBird.State.Idle
			Yield Nothing
		End While
		Me.bird.StartLasers()
		While Me.bird.state <> FlyingBirdLevelBird.State.Idle
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600048B RID: 1163 RVA: 0x00063C20 File Offset: 0x00062020
	Private Iterator Function houseDie_cr() As IEnumerator
		Me.bird.BirdFall()
		Yield Nothing
		Return
	End Function

	' Token: 0x0600048C RID: 1164 RVA: 0x00063C3C File Offset: 0x0006203C
	Private Iterator Function enemies_cr() As IEnumerator
		Dim firstTime As Boolean = True
		Dim target As AbstractPlayerController = PlayerManager.GetNext()
		Dim r As Integer = 1
		While True
			If Not Me.properties.CurrentState.enemies.active Then
				firstTime = True
				While Not Me.properties.CurrentState.enemies.active
					Yield Nothing
				End While
			End If
			Dim properties As LevelProperties.FlyingBird.Enemies = Me.properties.CurrentState.enemies
			Dim i As Integer = 0
			Dim p As FlyingBirdLevelEnemy.Properties = New FlyingBirdLevelEnemy.Properties(properties.health, properties.speed, properties.floatRange, properties.floatTime, properties.projectileHeight, properties.projectileFallTime, properties.projectileDelay)
			target = PlayerManager.GetNext()
			Dim pos As Vector2 = Me.enemyRoot.position
			If Not Me.properties.CurrentState.enemies.aim Then
				pos.y *= CSng(r)
				r *= -1
			Else
				pos.y = target.center.y
			End If
			While i < properties.count
				Yield CupheadTime.WaitForSeconds(Me, properties.delay)
				Dim parryable As Boolean = i = properties.count - 1
				Me.prefabs.formationBird.Create(pos, p).SetParryable(parryable)
				i += 1
			End While
			Yield CupheadTime.WaitForSeconds(Me, If(firstTime, properties.initalGroupDelay, properties.groupDelay))
			firstTime = False
		End While
		Return
	End Function

	' Token: 0x0600048D RID: 1165 RVA: 0x00063C58 File Offset: 0x00062058
	Private Iterator Function turrets_cr() As IEnumerator
		Dim top As FlyingBirdLevelTurret = Nothing
		Dim bottom As FlyingBirdLevelTurret = Nothing
		While True
			If Not Me.properties.CurrentState.turrets.active Then
				While Not Me.properties.CurrentState.turrets.active
					Yield Nothing
				End While
			End If
			If top Is Nothing OrElse top.transform Is Nothing OrElse top.state = FlyingBirdLevelTurret.State.Respawn Then
				top = Me.CreateTurret(Me.turretRootTop.position)
			End If
			If bottom Is Nothing OrElse bottom.transform Is Nothing OrElse bottom.state = FlyingBirdLevelTurret.State.Respawn Then
				bottom = Me.CreateTurret(Me.turretRootBottom.position)
			End If
			Yield CupheadTime.WaitForSeconds(Me, Me.properties.CurrentState.turrets.respawnDelay)
		End While
		Return
	End Function

	' Token: 0x0600048E RID: 1166 RVA: 0x00063C74 File Offset: 0x00062074
	Private Iterator Function birdHouseRevival_cr() As IEnumerator
		While Me.smallBird.isActiveAndEnabled
			Yield Nothing
		End While
		Me.bird.OnBossRevival()
		While Me.bird.state = FlyingBirdLevelBird.State.Reviving
			Yield Nothing
		End While
		MyBase.StartCoroutine(Me.skybirdPattern_cr())
		Return
	End Function

	' Token: 0x0600048F RID: 1167 RVA: 0x00063C90 File Offset: 0x00062090
	Private Iterator Function garbage_cr() As IEnumerator
		While Me.bird.state <> FlyingBirdLevelBird.State.Revived
			Yield Nothing
		End While
		Me.bird.StartGarbageOne()
		While Me.bird.state <> FlyingBirdLevelBird.State.Revived
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000490 RID: 1168 RVA: 0x00063CAC File Offset: 0x000620AC
	Private Iterator Function heartAttack_cr() As IEnumerator
		While Me.bird.state <> FlyingBirdLevelBird.State.Revived
			Yield Nothing
		End While
		Me.bird.StartHeartAttack()
		While Me.bird.state <> FlyingBirdLevelBird.State.Revived
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000491 RID: 1169 RVA: 0x00063CC8 File Offset: 0x000620C8
	Private Function CreateTurret(pos As Vector2) As FlyingBirdLevelTurret
		Dim properties As FlyingBirdLevelTurret.Properties = New FlyingBirdLevelTurret.Properties(CSng(Me.properties.CurrentState.turrets.health), Me.properties.CurrentState.turrets.inTime, pos.x, Me.properties.CurrentState.turrets.bulletSpeed, Me.properties.CurrentState.turrets.bulletDelay, Me.properties.CurrentState.turrets.floatRange, Me.properties.CurrentState.turrets.floatTime)
		Return Me.prefabs.turretBird.Create(New Vector2(690F, pos.y), properties)
	End Function

	' Token: 0x040007C4 RID: 1988
	Private properties As LevelProperties.FlyingBird

	' Token: 0x040007C5 RID: 1989
	<SerializeField()>
	Private bird As FlyingBirdLevelBird

	' Token: 0x040007C6 RID: 1990
	<SerializeField()>
	Private smallBird As FlyingBirdLevelSmallBird

	' Token: 0x040007C7 RID: 1991
	<Space(10F)>
	<SerializeField()>
	Private enemyRoot As Transform

	' Token: 0x040007C8 RID: 1992
	<Space(10F)>
	<SerializeField()>
	Private turretRootTop As Transform

	' Token: 0x040007C9 RID: 1993
	<SerializeField()>
	Private turretRootBottom As Transform

	' Token: 0x040007CA RID: 1994
	<Space(10F)>
	<SerializeField()>
	Private prefabs As FlyingBirdLevel.Prefabs

	' Token: 0x040007CB RID: 1995
	Private skybirdPattern As IEnumerator

	' Token: 0x040007CC RID: 1996
	<Header("Boss Info")>
	<SerializeField()>
	Private _bossPortraitMain As Sprite

	' Token: 0x040007CD RID: 1997
	<SerializeField()>
	Private _bossPortraitHouseDeath As Sprite

	' Token: 0x040007CE RID: 1998
	<SerializeField()>
	Private _bossPortraitBirdRevival As Sprite

	' Token: 0x040007CF RID: 1999
	<SerializeField()>
	Private _bossQuoteMain As String

	' Token: 0x040007D0 RID: 2000
	<SerializeField()>
	Private _bossQuoteHouseDeath As String

	' Token: 0x040007D1 RID: 2001
	<SerializeField()>
	Private _bossQuoteBirdRevival As String

	' Token: 0x02000615 RID: 1557
	<Serializable()>
	Public Class Prefabs
		' Token: 0x0400280B RID: 10251
		Public formationBird As FlyingBirdLevelEnemy

		' Token: 0x0400280C RID: 10252
		Public turretBird As FlyingBirdLevelTurret
	End Class
End Class
