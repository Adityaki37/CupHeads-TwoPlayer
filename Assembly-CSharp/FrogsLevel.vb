Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020001ED RID: 493
Public Class FrogsLevel
	Inherits Level

	' Token: 0x0600055E RID: 1374 RVA: 0x00067E04 File Offset: 0x00066204
	Protected Overrides Sub PartialInit()
		Me.properties = LevelProperties.Frogs.GetMode(MyBase.mode)
		AddHandler Me.properties.OnStateChange, AddressOf MyBase.zHack_OnStateChanged
		AddHandler Me.properties.OnBossDeath, AddressOf MyBase.zHack_OnWin
		MyBase.timeline = Me.properties.CreateTimeline(MyBase.mode)
		Me.goalTimes = Me.properties.goalTimes
		AddHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		MyBase.PartialInit()
	End Sub

	' Token: 0x170000F1 RID: 241
	' (get) Token: 0x0600055F RID: 1375 RVA: 0x00067E9A File Offset: 0x0006629A
	Public Overrides ReadOnly Property CurrentLevel As Levels
		Get
			Return Levels.Frogs
		End Get
	End Property

	' Token: 0x170000F2 RID: 242
	' (get) Token: 0x06000560 RID: 1376 RVA: 0x00067E9D File Offset: 0x0006629D
	Public Overrides ReadOnly Property CurrentScene As Scenes
		Get
			Return Scenes.scene_level_frogs
		End Get
	End Property

	' Token: 0x170000F3 RID: 243
	' (get) Token: 0x06000561 RID: 1377 RVA: 0x00067EA1 File Offset: 0x000662A1
	' (set) Token: 0x06000562 RID: 1378 RVA: 0x00067EA8 File Offset: 0x000662A8
	Public Shared Property FINAL_FORM As Boolean

	' Token: 0x170000F4 RID: 244
	' (get) Token: 0x06000563 RID: 1379 RVA: 0x00067EB0 File Offset: 0x000662B0
	' (set) Token: 0x06000564 RID: 1380 RVA: 0x00067EB7 File Offset: 0x000662B7
	Public Shared Property DEMON_TRIGGERED As Boolean

	' Token: 0x170000F5 RID: 245
	' (get) Token: 0x06000565 RID: 1381 RVA: 0x00067EC0 File Offset: 0x000662C0
	Public Overrides ReadOnly Property BossPortrait As Sprite
		Get
			Select Case Me.properties.CurrentState.stateName
				Case LevelProperties.Frogs.States.Main, LevelProperties.Frogs.States.Generic
					Return Me._bossPortraitMain
				Case LevelProperties.Frogs.States.Roll
					Return Me._bossPortraitRoll
				Case LevelProperties.Frogs.States.Morph
					Return Me._bossPortraitMorph
				Case Else
					Global.Debug.LogError("Couldn't find portrait for state " + Me.properties.CurrentState.stateName + ". Using Main.", Nothing)
					Return Me._bossPortraitMain
			End Select
		End Get
	End Property

	' Token: 0x170000F6 RID: 246
	' (get) Token: 0x06000566 RID: 1382 RVA: 0x00067F40 File Offset: 0x00066340
	Public Overrides ReadOnly Property BossQuote As String
		Get
			Select Case Me.properties.CurrentState.stateName
				Case LevelProperties.Frogs.States.Main, LevelProperties.Frogs.States.Generic
					Return Me._bossQuoteMain
				Case LevelProperties.Frogs.States.Roll
					Return Me._bossQuoteRoll
				Case LevelProperties.Frogs.States.Morph
					Return Me._bossQuoteMorph
				Case Else
					Global.Debug.LogError("Couldn't find quote for state " + Me.properties.CurrentState.stateName + ". Using Main.", Nothing)
					Return Me._bossQuoteMain
			End Select
		End Get
	End Property

	' Token: 0x06000567 RID: 1383 RVA: 0x00067FBE File Offset: 0x000663BE
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.tall.LevelInit(Me.properties)
		Me.small.LevelInit(Me.properties)
		Me.morphed.LevelInit(Me.properties)
	End Sub

	' Token: 0x06000568 RID: 1384 RVA: 0x00067FF9 File Offset: 0x000663F9
	Protected Overrides Sub OnLevelStart()
		FrogsLevel.FINAL_FORM = False
		Me.StartState(LevelProperties.Frogs.States.Main)
	End Sub

	' Token: 0x06000569 RID: 1385 RVA: 0x00068008 File Offset: 0x00066408
	Protected Overrides Sub OnStateChanged()
		MyBase.OnStateChanged()
		Dim stateName As LevelProperties.Frogs.States = Me.properties.CurrentState.stateName
		If stateName = LevelProperties.Frogs.States.Morph Then
			FrogsLevel.FINAL_FORM = True
		End If
		Me.StartState(stateName)
	End Sub

	' Token: 0x0600056A RID: 1386 RVA: 0x00068040 File Offset: 0x00066440
	Protected Overrides Sub CreatePlayers()
		MyBase.CreatePlayers()
		If PlayerManager.Multiplayer AndAlso Me.allowMultiplayer Then
			Me.tall.AddFanForce(Me.players(0))
			Me.tall.AddFanForce(Me.players(1))
		Else
			Me.tall.AddFanForce(Me.players(0))
		End If
	End Sub

	' Token: 0x0600056B RID: 1387 RVA: 0x000680A6 File Offset: 0x000664A6
	Protected Overrides Sub CreatePlayerTwoOnJoin()
		MyBase.CreatePlayerTwoOnJoin()
		If PlayerManager.Multiplayer AndAlso Me.allowMultiplayer Then
			Me.tall.AddFanForce(Me.players(1))
		End If
	End Sub

	' Token: 0x0600056C RID: 1388 RVA: 0x000680D6 File Offset: 0x000664D6
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me._bossPortraitMain = Nothing
		Me._bossPortraitMorph = Nothing
		Me._bossPortraitRoll = Nothing
	End Sub

	' Token: 0x0600056D RID: 1389 RVA: 0x000680F4 File Offset: 0x000664F4
	Private Sub StartState(state As LevelProperties.Frogs.States)
		If state <> LevelProperties.Frogs.States.Generic Then
			If Me.checkCoroutine IsNot Nothing Then
				MyBase.StopCoroutine(Me.checkCoroutine)
			End If
			Me.checkCoroutine = Nothing
			If Me.stateCoroutine IsNot Nothing Then
				MyBase.StopCoroutine(Me.stateCoroutine)
			End If
			Me.stateCoroutine = Nothing
			If Me.fanCoroutine IsNot Nothing Then
				MyBase.StopCoroutine(Me.fanCoroutine)
			End If
			Me.fanCoroutine = Nothing
		End If
		Me.checkCoroutine = MyBase.StartCoroutine(Me.startState_cr(state))
	End Sub

	' Token: 0x0600056E RID: 1390 RVA: 0x00068178 File Offset: 0x00066578
	Private Iterator Function startState_cr(state As LevelProperties.Frogs.States) As IEnumerator
		Select Case state
			Case Else
				Me.stateCoroutine = MyBase.StartCoroutine(Me.mainState_cr())
			Case LevelProperties.Frogs.States.Roll
				Me.wantsToRoll = True
				Yield MyBase.StartCoroutine(Me.waitForFrogs_cr())
				Me.small.StartRoll()
				Yield MyBase.StartCoroutine(Me.waitForShort_cr())
				Me.stateCoroutine = MyBase.StartCoroutine(Me.rollState_cr())
			Case LevelProperties.Frogs.States.Morph
				Yield MyBase.StartCoroutine(Me.waitForFrogs_cr())
				FrogsLevel.DEMON_TRIGGERED = Me.demonTrigger.getTrigger()
				Me.tall.StartMorph()
				Me.small.StartMorph()
		End Select
		Return
	End Function

	' Token: 0x0600056F RID: 1391 RVA: 0x0006819C File Offset: 0x0006659C
	Private Iterator Function mainState_cr() As IEnumerator
		While True
			Select Case Me.properties.CurrentState.NextPattern
				Case LevelProperties.Frogs.Pattern.TallFan
					Yield MyBase.StartCoroutine(Me.tallFan_cr())
				Case LevelProperties.Frogs.Pattern.ShortRage
					Yield MyBase.StartCoroutine(Me.shortRage_cr())
				Case LevelProperties.Frogs.Pattern.TallFireflies
					Yield MyBase.StartCoroutine(Me.tallFireflies_cr())
				Case LevelProperties.Frogs.Pattern.ShortClap
					Yield MyBase.StartCoroutine(Me.shortClap_cr())
				Case LevelProperties.Frogs.Pattern.Morph
					GoTo IL_0181
				Case LevelProperties.Frogs.Pattern.RagePlusFireflies
					Yield MyBase.StartCoroutine(Me.ragePlusFireflies_cr())
				Case Else
					GoTo IL_0181
			End Select
			Continue For
			IL_0181:
			Yield New WaitForSeconds(1F)
		End While
		Return
	End Function

	' Token: 0x06000570 RID: 1392 RVA: 0x000681B8 File Offset: 0x000665B8
	Private Iterator Function rollState_cr() As IEnumerator
		If Me.fanCoroutine IsNot Nothing Then
			MyBase.StopCoroutine(Me.fanCoroutine)
		End If
		Me.fanCoroutine = MyBase.StartCoroutine(Me.rollFan_cr())
		While True
			Dim p As LevelProperties.Frogs.Pattern = Me.properties.CurrentState.NextPattern
			If p <> LevelProperties.Frogs.Pattern.ShortClap Then
				If p <> LevelProperties.Frogs.Pattern.ShortRage Then
					Yield New WaitForSeconds(1F)
				Else
					Yield MyBase.StartCoroutine(Me.shortRage_cr())
				End If
			Else
				Yield MyBase.StartCoroutine(Me.shortClap_cr())
			End If
		End While
		Return
	End Function

	' Token: 0x06000571 RID: 1393 RVA: 0x000681D4 File Offset: 0x000665D4
	Private Iterator Function rollFan_cr() As IEnumerator
		Dim hesitate As Single = CSng(Me.properties.CurrentState.tallFan.hesitate)
		Yield MyBase.StartCoroutine(Me.waitForShort_cr())
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		While True
			Me.tall.StartFan()
			Yield MyBase.StartCoroutine(Me.waitForTall_cr())
			Yield CupheadTime.WaitForSeconds(Me, hesitate)
		End While
		Return
	End Function

	' Token: 0x06000572 RID: 1394 RVA: 0x000681F0 File Offset: 0x000665F0
	Private Iterator Function waitForFrogs_cr() As IEnumerator
		While (Me.tall.state <> FrogsLevelTall.State.Complete AndAlso Me.tall.state <> FrogsLevelTall.State.Morphed AndAlso Me.tall.state <> FrogsLevelTall.State.Idle) OrElse (Me.small.state <> FrogsLevelShort.State.Complete AndAlso Me.small.state <> FrogsLevelShort.State.Morphed AndAlso Me.small.state <> FrogsLevelShort.State.Idle)
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000573 RID: 1395 RVA: 0x0006820C File Offset: 0x0006660C
	Private Iterator Function waitForTall_cr() As IEnumerator
		While Me.tall.state <> FrogsLevelTall.State.Complete AndAlso Me.tall.state <> FrogsLevelTall.State.Morphed
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000574 RID: 1396 RVA: 0x00068228 File Offset: 0x00066628
	Private Iterator Function waitForShort_cr() As IEnumerator
		While Me.small.state <> FrogsLevelShort.State.Complete AndAlso Me.small.state <> FrogsLevelShort.State.Morphed
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000575 RID: 1397 RVA: 0x00068244 File Offset: 0x00066644
	Private Iterator Function tallFan_cr() As IEnumerator
		Me.tall.StartFan()
		Yield MyBase.StartCoroutine(Me.waitForTall_cr())
		Return
	End Function

	' Token: 0x06000576 RID: 1398 RVA: 0x00068260 File Offset: 0x00066660
	Private Iterator Function tallFireflies_cr() As IEnumerator
		Me.tall.StartFireflies()
		Yield MyBase.StartCoroutine(Me.waitForTall_cr())
		Return
	End Function

	' Token: 0x06000577 RID: 1399 RVA: 0x0006827C File Offset: 0x0006667C
	Private Iterator Function shortRage_cr() As IEnumerator
		Me.small.StartRage()
		Yield MyBase.StartCoroutine(Me.waitForShort_cr())
		Return
	End Function

	' Token: 0x06000578 RID: 1400 RVA: 0x00068298 File Offset: 0x00066698
	Private Iterator Function ragePlusFireflies_cr() As IEnumerator
		Me.tall.StartFireflies()
		Me.small.StartRage()
		While Not Me.wantsToRoll
			If Me.tall.state = FrogsLevelTall.State.Complete Then
				Me.tall.StartFireflies()
			End If
			If Me.small.state = FrogsLevelShort.State.Complete Then
				Me.small.StartRage()
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000579 RID: 1401 RVA: 0x000682B4 File Offset: 0x000666B4
	Private Iterator Function shortClap_cr() As IEnumerator
		Me.small.StartClap()
		Yield MyBase.StartCoroutine(Me.waitForShort_cr())
		Return
	End Function

	' Token: 0x04000A43 RID: 2627
	Private properties As LevelProperties.Frogs

	' Token: 0x04000A46 RID: 2630
	<SerializeField()>
	Private tall As FrogsLevelTall

	' Token: 0x04000A47 RID: 2631
	<SerializeField()>
	Private small As FrogsLevelShort

	' Token: 0x04000A48 RID: 2632
	<SerializeField()>
	Private morphed As FrogsLevelMorphed

	' Token: 0x04000A49 RID: 2633
	<SerializeField()>
	Private demonTrigger As FrogsLevelDemonTrigger

	' Token: 0x04000A4A RID: 2634
	Private wantsToRoll As Boolean

	' Token: 0x04000A4B RID: 2635
	<Header("Boss Info")>
	<SerializeField()>
	Private _bossPortraitMain As Sprite

	' Token: 0x04000A4C RID: 2636
	<SerializeField()>
	Private _bossPortraitRoll As Sprite

	' Token: 0x04000A4D RID: 2637
	<SerializeField()>
	Private _bossPortraitMorph As Sprite

	' Token: 0x04000A4E RID: 2638
	<SerializeField()>
	Private _bossQuoteMain As String

	' Token: 0x04000A4F RID: 2639
	<SerializeField()>
	Private _bossQuoteRoll As String

	' Token: 0x04000A50 RID: 2640
	<SerializeField()>
	Private _bossQuoteMorph As String

	' Token: 0x04000A51 RID: 2641
	Private checkCoroutine As Coroutine

	' Token: 0x04000A52 RID: 2642
	Private stateCoroutine As Coroutine

	' Token: 0x04000A53 RID: 2643
	Private fanCoroutine As Coroutine

	' Token: 0x020006C2 RID: 1730
	<Serializable()>
	Public Class Prefabs
	End Class
End Class
