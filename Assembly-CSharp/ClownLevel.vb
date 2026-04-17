Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020000CF RID: 207
Public Class ClownLevel
	Inherits Level

	' Token: 0x06000261 RID: 609 RVA: 0x0005BDF0 File Offset: 0x0005A1F0
	Protected Overrides Sub PartialInit()
		Me.properties = LevelProperties.Clown.GetMode(MyBase.mode)
		AddHandler Me.properties.OnStateChange, AddressOf MyBase.zHack_OnStateChanged
		AddHandler Me.properties.OnBossDeath, AddressOf MyBase.zHack_OnWin
		MyBase.timeline = Me.properties.CreateTimeline(MyBase.mode)
		Me.goalTimes = Me.properties.goalTimes
		AddHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		MyBase.PartialInit()
	End Sub

	' Token: 0x17000063 RID: 99
	' (get) Token: 0x06000262 RID: 610 RVA: 0x0005BE86 File Offset: 0x0005A286
	Public Overrides ReadOnly Property CurrentLevel As Levels
		Get
			Return Levels.Clown
		End Get
	End Property

	' Token: 0x17000064 RID: 100
	' (get) Token: 0x06000263 RID: 611 RVA: 0x0005BE8D File Offset: 0x0005A28D
	Public Overrides ReadOnly Property CurrentScene As Scenes
		Get
			Return Scenes.scene_level_clown
		End Get
	End Property

	' Token: 0x17000065 RID: 101
	' (get) Token: 0x06000264 RID: 612 RVA: 0x0005BE94 File Offset: 0x0005A294
	Public Overrides ReadOnly Property BossPortrait As Sprite
		Get
			Select Case Me.properties.CurrentState.stateName
				Case LevelProperties.Clown.States.Main, LevelProperties.Clown.States.Generic
					Return Me._bossPortraitMain
				Case LevelProperties.Clown.States.HeliumTank
					Return Me._bossPortraitHeliumTank
				Case LevelProperties.Clown.States.CarouselHorse
					Return Me._bossPortraitCarouselHorse
				Case LevelProperties.Clown.States.Swing
					Return Me._bossPortraitSwing
				Case Else
					Global.Debug.LogError("Couldn't find portrait for state " + Me.properties.CurrentState.stateName + ". Using Main.", Nothing)
					Return Me._bossPortraitMain
			End Select
		End Get
	End Property

	' Token: 0x17000066 RID: 102
	' (get) Token: 0x06000265 RID: 613 RVA: 0x0005BF20 File Offset: 0x0005A320
	Public Overrides ReadOnly Property BossQuote As String
		Get
			Select Case Me.properties.CurrentState.stateName
				Case LevelProperties.Clown.States.Main, LevelProperties.Clown.States.Generic
					Return Me._bossQuoteMain
				Case LevelProperties.Clown.States.HeliumTank
					Return Me._bossQuoteHeliumTank
				Case LevelProperties.Clown.States.CarouselHorse
					Return Me._bossQuoteCarouselHorse
				Case LevelProperties.Clown.States.Swing
					Return Me._bossQuoteSwing
				Case Else
					Global.Debug.LogError("Couldn't find quote for state " + Me.properties.CurrentState.stateName + ". Using Main.", Nothing)
					Return Me._bossQuoteMain
			End Select
		End Get
	End Property

	' Token: 0x06000266 RID: 614 RVA: 0x0005BFAC File Offset: 0x0005A3AC
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.coasterHandler.LevelInit(Me.properties)
		Me.clown.LevelInit(Me.properties)
		Me.clownHelium.LevelInit(Me.properties)
		Me.clownHorse.LevelInit(Me.properties)
		Me.clownSwing.LevelInit(Me.properties)
	End Sub

	' Token: 0x06000267 RID: 615 RVA: 0x0005C014 File Offset: 0x0005A414
	Protected Overrides Sub OnLevelStart()
		MyBase.StartCoroutine(Me.clownPattern_cr())
	End Sub

	' Token: 0x06000268 RID: 616 RVA: 0x0005C024 File Offset: 0x0005A424
	Protected Overrides Sub OnStateChanged()
		MyBase.OnStateChanged()
		If Me.properties.CurrentState.stateName = LevelProperties.Clown.States.HeliumTank Then
			Me.StopAllCoroutines()
			MyBase.StartCoroutine(Me.helium_tank_cr())
		ElseIf Me.properties.CurrentState.stateName = LevelProperties.Clown.States.CarouselHorse Then
			Me.StopAllCoroutines()
			MyBase.StartCoroutine(Me.carousel_horse_cr())
		ElseIf Me.properties.CurrentState.stateName = LevelProperties.Clown.States.Swing Then
			Me.StopAllCoroutines()
			MyBase.StartCoroutine(Me.swing_cr())
		End If
	End Sub

	' Token: 0x06000269 RID: 617 RVA: 0x0005C0BC File Offset: 0x0005A4BC
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me._bossPortraitCarouselHorse = Nothing
		Me._bossPortraitHeliumTank = Nothing
		Me._bossPortraitMain = Nothing
		Me._bossPortraitSwing = Nothing
	End Sub

	' Token: 0x0600026A RID: 618 RVA: 0x0005C0E0 File Offset: 0x0005A4E0
	Private Iterator Function clownPattern_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		While True
			Yield MyBase.StartCoroutine(Me.nextPattern_cr())
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600026B RID: 619 RVA: 0x0005C0FC File Offset: 0x0005A4FC
	Private Iterator Function nextPattern_cr() As IEnumerator
		Dim p As LevelProperties.Clown.Pattern = Me.properties.CurrentState.NextPattern
		If p <> LevelProperties.Clown.Pattern.[Default] Then
			Yield CupheadTime.WaitForSeconds(Me, 1F)
		Else
			Yield Nothing
		End If
		Return
	End Function

	' Token: 0x0600026C RID: 620 RVA: 0x0005C118 File Offset: 0x0005A518
	Private Iterator Function bumper_car_cr() As IEnumerator
		Me.clown.StartBumperCar()
		Yield Nothing
		Return
	End Function

	' Token: 0x0600026D RID: 621 RVA: 0x0005C134 File Offset: 0x0005A534
	Private Iterator Function helium_tank_cr() As IEnumerator
		Me.clown.EndBumperCar()
		If Me.coasterHandler.isRunning Then
			Me.coasterHandler.finalRun = True
		End If
		If Me.properties.CurrentState.heliumClown.coasterOn Then
			While Me.coasterHandler.finalRun
				Yield Nothing
			End While
			Me.coasterHandler.StartCoaster()
		End If
		Yield Nothing
		Return
	End Function

	' Token: 0x0600026E RID: 622 RVA: 0x0005C150 File Offset: 0x0005A550
	Private Iterator Function carousel_horse_cr() As IEnumerator
		Me.clownHelium.StartDeath()
		If Me.coasterHandler.isRunning Then
			Me.coasterHandler.finalRun = True
		End If
		If Me.properties.CurrentState.horse.coasterOn Then
			While Me.coasterHandler.finalRun
				Yield Nothing
			End While
			Me.coasterHandler.StartCoaster()
		End If
		Yield Nothing
		Return
	End Function

	' Token: 0x0600026F RID: 623 RVA: 0x0005C16C File Offset: 0x0005A56C
	Private Iterator Function swing_cr() As IEnumerator
		Me.clownHorse.StartDeath()
		If Me.coasterHandler.isRunning Then
			Me.coasterHandler.finalRun = True
		End If
		While Me.coasterHandler.finalRun
			Yield Nothing
		End While
		Me.coasterHandler.StartCoaster()
		Yield Nothing
		Return
	End Function

	' Token: 0x0400044C RID: 1100
	Private properties As LevelProperties.Clown

	' Token: 0x0400044D RID: 1101
	<SerializeField()>
	Private clown As ClownLevelClown

	' Token: 0x0400044E RID: 1102
	<SerializeField()>
	Private clownHelium As ClownLevelClownHelium

	' Token: 0x0400044F RID: 1103
	<SerializeField()>
	Private clownHorse As ClownLevelClownHorse

	' Token: 0x04000450 RID: 1104
	<SerializeField()>
	Private clownSwing As ClownLevelClownSwing

	' Token: 0x04000451 RID: 1105
	<SerializeField()>
	Private coasterHandler As ClownLevelCoasterHandler

	' Token: 0x04000452 RID: 1106
	<Header("Boss Info")>
	<SerializeField()>
	Private _bossPortraitMain As Sprite

	' Token: 0x04000453 RID: 1107
	<SerializeField()>
	Private _bossPortraitHeliumTank As Sprite

	' Token: 0x04000454 RID: 1108
	<SerializeField()>
	Private _bossPortraitCarouselHorse As Sprite

	' Token: 0x04000455 RID: 1109
	<SerializeField()>
	Private _bossPortraitSwing As Sprite

	' Token: 0x04000456 RID: 1110
	<SerializeField()>
	Private _bossQuoteMain As String

	' Token: 0x04000457 RID: 1111
	<SerializeField()>
	Private _bossQuoteHeliumTank As String

	' Token: 0x04000458 RID: 1112
	<SerializeField()>
	Private _bossQuoteCarouselHorse As String

	' Token: 0x04000459 RID: 1113
	<SerializeField()>
	Private _bossQuoteSwing As String
End Class
