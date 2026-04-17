Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020001B4 RID: 436
Public Class FlyingCowboyLevel
	Inherits Level

	' Token: 0x060004D0 RID: 1232 RVA: 0x00065840 File Offset: 0x00063C40
	Protected Overrides Sub PartialInit()
		Me.properties = LevelProperties.FlyingCowboy.GetMode(MyBase.mode)
		AddHandler Me.properties.OnStateChange, AddressOf MyBase.zHack_OnStateChanged
		AddHandler Me.properties.OnBossDeath, AddressOf MyBase.zHack_OnWin
		MyBase.timeline = Me.properties.CreateTimeline(MyBase.mode)
		Me.goalTimes = Me.properties.goalTimes
		AddHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		MyBase.PartialInit()
	End Sub

	' Token: 0x170000DC RID: 220
	' (get) Token: 0x060004D1 RID: 1233 RVA: 0x000658D6 File Offset: 0x00063CD6
	Public Overrides ReadOnly Property CurrentLevel As Levels
		Get
			Return Levels.FlyingCowboy
		End Get
	End Property

	' Token: 0x170000DD RID: 221
	' (get) Token: 0x060004D2 RID: 1234 RVA: 0x000658DD File Offset: 0x00063CDD
	Public Overrides ReadOnly Property CurrentScene As Scenes
		Get
			Return Scenes.scene_level_flying_cowboy
		End Get
	End Property

	' Token: 0x170000DE RID: 222
	' (get) Token: 0x060004D3 RID: 1235 RVA: 0x000658E4 File Offset: 0x00063CE4
	Public Overrides ReadOnly Property BossPortrait As Sprite
		Get
			Select Case Me.properties.CurrentState.stateName
				Case LevelProperties.FlyingCowboy.States.Main
					Return Me._bossPortraitMain
				Case LevelProperties.FlyingCowboy.States.Vacuum
					Return Me._bossPortraitPhaseTwo
				Case LevelProperties.FlyingCowboy.States.Sausage
					Return Me._bossPortraitPhaseFour
				Case LevelProperties.FlyingCowboy.States.Meatball
					Return Me._bossPortraitPhaseThree
			End Select
			Global.Debug.LogError("Couldn't find portrait for state " + Me.properties.CurrentState.stateName + ". Using Main.", Nothing)
			Return Me._bossPortraitMain
		End Get
	End Property

	' Token: 0x170000DF RID: 223
	' (get) Token: 0x060004D4 RID: 1236 RVA: 0x00065970 File Offset: 0x00063D70
	Public Overrides ReadOnly Property BossQuote As String
		Get
			Select Case Me.properties.CurrentState.stateName
				Case LevelProperties.FlyingCowboy.States.Main
					Return Me._bossQuoteMain
				Case LevelProperties.FlyingCowboy.States.Vacuum
					Return Me._bossQuotePhaseTwo
				Case LevelProperties.FlyingCowboy.States.Sausage
					Return Me._bossQuotePhaseFour
				Case LevelProperties.FlyingCowboy.States.Meatball
					Return Me._bossQuotePhaseThree
			End Select
			Global.Debug.LogError("Couldn't find quote for state " + Me.properties.CurrentState.stateName + ". Using Main.", Nothing)
			Return Me._bossQuoteMain
		End Get
	End Property

	' Token: 0x060004D5 RID: 1237 RVA: 0x000659F9 File Offset: 0x00063DF9
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me._bossPortraitMain = Nothing
		Me._bossPortraitPhaseTwo = Nothing
		Me._bossPortraitPhaseThree = Nothing
		Me._bossPortraitPhaseFour = Nothing
	End Sub

	' Token: 0x060004D6 RID: 1238 RVA: 0x00065A20 File Offset: 0x00063E20
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.cowboy.LevelInit(Me.properties)
		Me.meat.LevelInit(Me.properties)
		Me.playerDusts(0) = Global.UnityEngine.[Object].Instantiate(Of PlanePlayerDust)(Me.playerDust)
		Me.playerDusts(1) = Global.UnityEngine.[Object].Instantiate(Of PlanePlayerDust)(Me.playerDust)
		Me.playerDusts(0).Initialize(Me.players(0), Me.playerDustSmallTrigger, Me.playerDustLargeTrigger)
		Me.playerDusts(1).Initialize(Me.players(1), Me.playerDustSmallTrigger, Me.playerDustLargeTrigger)
	End Sub

	' Token: 0x060004D7 RID: 1239 RVA: 0x00065ABD File Offset: 0x00063EBD
	Protected Overrides Sub CreatePlayerTwoOnJoin()
		MyBase.CreatePlayerTwoOnJoin()
		Me.playerDusts(1).Initialize(Me.players(1), Me.playerDustSmallTrigger, Me.playerDustLargeTrigger)
	End Sub

	' Token: 0x060004D8 RID: 1240 RVA: 0x00065AE8 File Offset: 0x00063EE8
	Protected Overrides Sub OnDrawGizmosSelected()
		Gizmos.color = Color.red
		Gizmos.DrawRay(New Vector3(-1000F, Me.playerDustSmallTrigger), Vector3.right * 2000F)
		Gizmos.color = Color.blue
		Gizmos.DrawRay(New Vector3(-1000F, Me.playerDustLargeTrigger), Vector3.right * 2000F)
	End Sub

	' Token: 0x060004D9 RID: 1241 RVA: 0x00065B54 File Offset: 0x00063F54
	Protected Overrides Sub OnStateChanged()
		MyBase.OnStateChanged()
		If Me.properties.CurrentState.stateName = LevelProperties.FlyingCowboy.States.Vacuum Then
			MyBase.StartCoroutine(Me.toPhase2_cr())
		ElseIf Me.properties.CurrentState.stateName = LevelProperties.FlyingCowboy.States.Meatball Then
			Me.StopAllCoroutines()
			MyBase.StartCoroutine(Me.toPhase3_cr())
		ElseIf Me.properties.CurrentState.stateName = LevelProperties.FlyingCowboy.States.Sausage Then
			Me.StopAllCoroutines()
			MyBase.StartCoroutine(Me.toPhase4_cr())
		End If
	End Sub

	' Token: 0x060004DA RID: 1242 RVA: 0x00065BE8 File Offset: 0x00063FE8
	Private Iterator Function toPhase2_cr() As IEnumerator
		Dim pattern As LevelProperties.FlyingCowboy.Pattern = Me.properties.CurrentState.PeekNextPattern
		Me.cowboy.OnPhase2(pattern)
		Yield Me.cowboy.animator.WaitForAnimationToStart(Me, "Ph1_To_Ph2", False)
		While Me.cowboy.state = FlyingCowboyLevelCowboy.State.PhaseTrans
			Yield Nothing
		End While
		MyBase.StartCoroutine(Me.phase2Loop_cr())
		Return
	End Function

	' Token: 0x060004DB RID: 1243 RVA: 0x00065C04 File Offset: 0x00064004
	Private Iterator Function phase2Loop_cr() As IEnumerator
		Dim initial As Boolean = True
		While True
			Dim p As LevelProperties.FlyingCowboy.Pattern = Me.properties.CurrentState.NextPattern
			If p = LevelProperties.FlyingCowboy.Pattern.Vacuum Then
				Yield MyBase.StartCoroutine(Me.vacuum_cr(initial))
			Else
				Yield MyBase.StartCoroutine(Me.ricochet_cr(initial))
			End If
			initial = False
		End While
		Return
	End Function

	' Token: 0x060004DC RID: 1244 RVA: 0x00065C20 File Offset: 0x00064020
	Private Iterator Function vacuum_cr(initial As Boolean) As IEnumerator
		While Me.cowboy.state <> FlyingCowboyLevelCowboy.State.Idle
			Yield Nothing
		End While
		Me.cowboy.Vacuum(initial, LevelProperties.FlyingCowboy.Pattern.[Default])
		While Me.cowboy.state <> FlyingCowboyLevelCowboy.State.Idle
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060004DD RID: 1245 RVA: 0x00065C44 File Offset: 0x00064044
	Private Iterator Function ricochet_cr(initial As Boolean) As IEnumerator
		If initial AndAlso Me.properties.CurrentState.ricochet.useRicochet Then
			Me.cowboy.animator.SetBool("OnRicochet", True)
		End If
		While Me.cowboy.state <> FlyingCowboyLevelCowboy.State.Idle
			Yield Nothing
		End While
		If Me.properties.CurrentState.ricochet.useRicochet Then
			Me.cowboy.Ricochet()
		End If
		While Me.cowboy.state <> FlyingCowboyLevelCowboy.State.Idle
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060004DE RID: 1246 RVA: 0x00065C68 File Offset: 0x00064068
	Private Iterator Function toPhase3_cr() As IEnumerator
		Me.background.BeginTransition()
		If Me.cowboy IsNot Nothing Then
			Me.cowboy.Death()
		End If
		While Not Me.cowboy.IsDead
			Yield Nothing
		End While
		Me.meat.SelectPhase(FlyingCowboyLevelMeat.MeatPhase.Sausage)
		Return
	End Function

	' Token: 0x060004DF RID: 1247 RVA: 0x00065C84 File Offset: 0x00064084
	Private Iterator Function toPhase4_cr() As IEnumerator
		While Me.cowboy.state <> FlyingCowboyLevelCowboy.State.Idle
			Yield Nothing
		End While
		Me.meat.SelectPhase(FlyingCowboyLevelMeat.MeatPhase.Can)
		Return
	End Function

	' Token: 0x040008CB RID: 2251
	Private properties As LevelProperties.FlyingCowboy

	' Token: 0x040008CC RID: 2252
	<SerializeField()>
	Private cowboy As FlyingCowboyLevelCowboy

	' Token: 0x040008CD RID: 2253
	<SerializeField()>
	Private meat As FlyingCowboyLevelMeat

	' Token: 0x040008CE RID: 2254
	<SerializeField()>
	Private background As FlyingCowboyLevelBackground

	' Token: 0x040008CF RID: 2255
	<SerializeField()>
	Private playerDust As PlanePlayerDust

	' Token: 0x040008D0 RID: 2256
	<SerializeField()>
	Private playerDustSmallTrigger As Single

	' Token: 0x040008D1 RID: 2257
	<SerializeField()>
	Private playerDustLargeTrigger As Single

	' Token: 0x040008D2 RID: 2258
	<Header("Boss Info")>
	<SerializeField()>
	Private _bossPortraitMain As Sprite

	' Token: 0x040008D3 RID: 2259
	<SerializeField()>
	Private _bossPortraitPhaseTwo As Sprite

	' Token: 0x040008D4 RID: 2260
	<SerializeField()>
	Private _bossPortraitPhaseThree As Sprite

	' Token: 0x040008D5 RID: 2261
	<SerializeField()>
	Private _bossPortraitPhaseFour As Sprite

	' Token: 0x040008D6 RID: 2262
	<SerializeField()>
	Private _bossQuoteMain As String

	' Token: 0x040008D7 RID: 2263
	<SerializeField()>
	Private _bossQuotePhaseTwo As String

	' Token: 0x040008D8 RID: 2264
	<SerializeField()>
	Private _bossQuotePhaseThree As String

	' Token: 0x040008D9 RID: 2265
	<SerializeField()>
	Private _bossQuotePhaseFour As String

	' Token: 0x040008DA RID: 2266
	Private playerDusts As PlanePlayerDust() = New PlanePlayerDust(1) {}
End Class
