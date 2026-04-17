Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020001DA RID: 474
Public Class FlyingMermaidLevel
	Inherits Level

	' Token: 0x06000522 RID: 1314 RVA: 0x00066C68 File Offset: 0x00065068
	Protected Overrides Sub PartialInit()
		Me.properties = LevelProperties.FlyingMermaid.GetMode(MyBase.mode)
		AddHandler Me.properties.OnStateChange, AddressOf MyBase.zHack_OnStateChanged
		AddHandler Me.properties.OnBossDeath, AddressOf MyBase.zHack_OnWin
		MyBase.timeline = Me.properties.CreateTimeline(MyBase.mode)
		Me.goalTimes = Me.properties.goalTimes
		AddHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		MyBase.PartialInit()
	End Sub

	' Token: 0x170000E6 RID: 230
	' (get) Token: 0x06000523 RID: 1315 RVA: 0x00066CFE File Offset: 0x000650FE
	Public Overrides ReadOnly Property CurrentLevel As Levels
		Get
			Return Levels.FlyingMermaid
		End Get
	End Property

	' Token: 0x170000E7 RID: 231
	' (get) Token: 0x06000524 RID: 1316 RVA: 0x00066D05 File Offset: 0x00065105
	Public Overrides ReadOnly Property CurrentScene As Scenes
		Get
			Return Scenes.scene_level_flying_mermaid
		End Get
	End Property

	' Token: 0x170000E8 RID: 232
	' (get) Token: 0x06000525 RID: 1317 RVA: 0x00066D09 File Offset: 0x00065109
	' (set) Token: 0x06000526 RID: 1318 RVA: 0x00066D11 File Offset: 0x00065111
	Public Property MerdusaTransformStarted As Boolean

	' Token: 0x170000E9 RID: 233
	' (get) Token: 0x06000527 RID: 1319 RVA: 0x00066D1C File Offset: 0x0006511C
	Public Overrides ReadOnly Property BossPortrait As Sprite
		Get
			Select Case Me.properties.CurrentState.stateName
				Case LevelProperties.FlyingMermaid.States.Main, LevelProperties.FlyingMermaid.States.Generic
					Return Me._bossPortraitMain
				Case LevelProperties.FlyingMermaid.States.Merdusa
					Return Me._bossPortraitMerdusa
				Case LevelProperties.FlyingMermaid.States.Head
					Return Me._bossPortraitHead
				Case Else
					Global.Debug.LogError("Couldn't find portrait for state " + Me.properties.CurrentState.stateName + ". Using Main.", Nothing)
					Return Me._bossPortraitMain
			End Select
		End Get
	End Property

	' Token: 0x170000EA RID: 234
	' (get) Token: 0x06000528 RID: 1320 RVA: 0x00066D9C File Offset: 0x0006519C
	Public Overrides ReadOnly Property BossQuote As String
		Get
			Select Case Me.properties.CurrentState.stateName
				Case LevelProperties.FlyingMermaid.States.Main, LevelProperties.FlyingMermaid.States.Generic
					Return Me._bossQuoteMain
				Case LevelProperties.FlyingMermaid.States.Merdusa
					Return Me._bossQuoteMerdusa
				Case LevelProperties.FlyingMermaid.States.Head
					Return Me._bossQuoteHead
				Case Else
					Global.Debug.LogError("Couldn't find quote for state " + Me.properties.CurrentState.stateName + ". Using Main.", Nothing)
					Return Me._bossQuoteMain
			End Select
		End Get
	End Property

	' Token: 0x06000529 RID: 1321 RVA: 0x00066E1C File Offset: 0x0006521C
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.mermaid.LevelInit(Me.properties)
		Me.merdusa.LevelInit(Me.properties)
		Me.merdusaHead.LevelInit(Me.properties)
		Me.MerdusaTransformStarted = False
	End Sub

	' Token: 0x0600052A RID: 1322 RVA: 0x00066E69 File Offset: 0x00065269
	Protected Overrides Sub OnLevelStart()
		Me.mermaid.IntroContinue()
		MyBase.StartCoroutine(Me.mermaidPattern_cr())
	End Sub

	' Token: 0x0600052B RID: 1323 RVA: 0x00066E84 File Offset: 0x00065284
	Protected Overrides Sub OnStateChanged()
		MyBase.OnStateChanged()
		If Me.properties.CurrentState.stateName = LevelProperties.FlyingMermaid.States.Merdusa Then
			Me.StopAllCoroutines()
			MyBase.StartCoroutine(Me.transform_to_merdusa_cr())
		End If
		If Me.properties.CurrentState.stateName = LevelProperties.FlyingMermaid.States.Head Then
			Me.StopAllCoroutines()
			MyBase.StartCoroutine(Me.mermaidPattern_cr())
			MyBase.StartCoroutine(Me.transform_to_head_cr())
		End If
	End Sub

	' Token: 0x0600052C RID: 1324 RVA: 0x00066EF6 File Offset: 0x000652F6
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me._bossPortraitHead = Nothing
		Me._bossPortraitMain = Nothing
		Me._bossPortraitMerdusa = Nothing
	End Sub

	' Token: 0x0600052D RID: 1325 RVA: 0x00066F14 File Offset: 0x00065314
	Private Iterator Function mermaidPattern_cr() As IEnumerator
		While True
			Yield MyBase.StartCoroutine(Me.nextPattern_cr())
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600052E RID: 1326 RVA: 0x00066F30 File Offset: 0x00065330
	Private Iterator Function nextPattern_cr() As IEnumerator
		Select Case Me.properties.CurrentState.NextPattern
			Case LevelProperties.FlyingMermaid.Pattern.Yell
				Yield MyBase.StartCoroutine(Me.yell_cr())
			Case LevelProperties.FlyingMermaid.Pattern.Summon
				Yield MyBase.StartCoroutine(Me.summon_cr())
			Case LevelProperties.FlyingMermaid.Pattern.Fish
				Yield MyBase.StartCoroutine(Me.fish_cr())
			Case LevelProperties.FlyingMermaid.Pattern.Zap
				Yield MyBase.StartCoroutine(Me.zap_cr())
			Case Else
				Yield CupheadTime.WaitForSeconds(Me, 1F)
			Case LevelProperties.FlyingMermaid.Pattern.Bubble
				Yield MyBase.StartCoroutine(Me.bubble_cr())
			Case LevelProperties.FlyingMermaid.Pattern.HeadBlast
				Yield MyBase.StartCoroutine(Me.head_blast_cr())
			Case LevelProperties.FlyingMermaid.Pattern.BubbleHeadBlast
				Yield MyBase.StartCoroutine(Me.bubble_head_blast_cr())
		End Select
		Return
	End Function

	' Token: 0x0600052F RID: 1327 RVA: 0x00066F4C File Offset: 0x0006534C
	Private Iterator Function yell_cr() As IEnumerator
		While Me.mermaid.state <> FlyingMermaidLevelMermaid.State.Idle
			Yield Nothing
		End While
		Me.mermaid.StartYell()
		While Me.mermaid.state <> FlyingMermaidLevelMermaid.State.Idle
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000530 RID: 1328 RVA: 0x00066F68 File Offset: 0x00065368
	Private Iterator Function transform_to_merdusa_cr() As IEnumerator
		Me.mermaid.StartTransform()
		While Me.merdusa.state <> FlyingMermaidLevelMerdusa.State.Idle
			Yield Nothing
		End While
		MyBase.StartCoroutine(Me.mermaidPattern_cr())
		Return
	End Function

	' Token: 0x06000531 RID: 1329 RVA: 0x00066F84 File Offset: 0x00065384
	Private Iterator Function transform_to_head_cr() As IEnumerator
		Me.merdusa.StartTransform()
		While Me.merdusaHead.state <> FlyingMermaidLevelMerdusaHead.State.Idle
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000532 RID: 1330 RVA: 0x00066FA0 File Offset: 0x000653A0
	Private Iterator Function summon_cr() As IEnumerator
		While Me.mermaid.state <> FlyingMermaidLevelMermaid.State.Idle
			Yield Nothing
		End While
		Me.mermaid.StartSummon()
		While Me.mermaid.state <> FlyingMermaidLevelMermaid.State.Idle
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000533 RID: 1331 RVA: 0x00066FBC File Offset: 0x000653BC
	Private Iterator Function fish_cr() As IEnumerator
		While Me.mermaid.state <> FlyingMermaidLevelMermaid.State.Idle
			Yield Nothing
		End While
		Me.mermaid.StartFish()
		While Me.mermaid.state <> FlyingMermaidLevelMermaid.State.Idle
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000534 RID: 1332 RVA: 0x00066FD8 File Offset: 0x000653D8
	Private Iterator Function zap_cr() As IEnumerator
		While Me.merdusa.state <> FlyingMermaidLevelMerdusa.State.Idle
			Yield Nothing
		End While
		Me.merdusa.StartZap()
		While Me.merdusa.state <> FlyingMermaidLevelMerdusa.State.Idle
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000535 RID: 1333 RVA: 0x00066FF4 File Offset: 0x000653F4
	Private Iterator Function bubble_cr() As IEnumerator
		While Me.merdusaHead.state <> FlyingMermaidLevelMerdusaHead.State.Idle
			Yield Nothing
		End While
		Me.merdusaHead.StartBubble()
		While Me.merdusaHead.state <> FlyingMermaidLevelMerdusaHead.State.Idle
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000536 RID: 1334 RVA: 0x00067010 File Offset: 0x00065410
	Private Iterator Function head_blast_cr() As IEnumerator
		While Me.merdusaHead.state <> FlyingMermaidLevelMerdusaHead.State.Idle
			Yield Nothing
		End While
		Me.merdusaHead.StartHeadBlast()
		While Me.merdusaHead.state <> FlyingMermaidLevelMerdusaHead.State.Idle
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000537 RID: 1335 RVA: 0x0006702C File Offset: 0x0006542C
	Private Iterator Function bubble_head_blast_cr() As IEnumerator
		While Me.merdusaHead.state <> FlyingMermaidLevelMerdusaHead.State.Idle
			Yield Nothing
		End While
		Me.merdusaHead.StartHeadBubble()
		While Me.merdusaHead.state <> FlyingMermaidLevelMerdusaHead.State.Idle
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x040009E4 RID: 2532
	Private properties As LevelProperties.FlyingMermaid

	' Token: 0x040009E5 RID: 2533
	<SerializeField()>
	Private mermaid As FlyingMermaidLevelMermaid

	' Token: 0x040009E6 RID: 2534
	<Header("FlyingMermaidLevel")>
	<SerializeField()>
	Private prefabs As FlyingMermaidLevel.Prefabs

	' Token: 0x040009E7 RID: 2535
	<SerializeField()>
	Private merdusa As FlyingMermaidLevelMerdusa

	' Token: 0x040009E8 RID: 2536
	<SerializeField()>
	Private merdusaHead As FlyingMermaidLevelMerdusaHead

	' Token: 0x040009EA RID: 2538
	<Header("Boss Info")>
	<SerializeField()>
	Private _bossPortraitMain As Sprite

	' Token: 0x040009EB RID: 2539
	<SerializeField()>
	Private _bossPortraitMerdusa As Sprite

	' Token: 0x040009EC RID: 2540
	<SerializeField()>
	Private _bossPortraitHead As Sprite

	' Token: 0x040009ED RID: 2541
	<SerializeField()>
	Private _bossQuoteMain As String

	' Token: 0x040009EE RID: 2542
	<SerializeField()>
	Private _bossQuoteMerdusa As String

	' Token: 0x040009EF RID: 2543
	<SerializeField()>
	Private _bossQuoteHead As String

	' Token: 0x0200067F RID: 1663
	<Serializable()>
	Public Class Prefabs
	End Class
End Class
