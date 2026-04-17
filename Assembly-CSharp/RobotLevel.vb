Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000264 RID: 612
Public Class RobotLevel
	Inherits Level

	' Token: 0x060006CF RID: 1743 RVA: 0x00071BD0 File Offset: 0x0006FFD0
	Protected Overrides Sub PartialInit()
		Me.properties = LevelProperties.Robot.GetMode(MyBase.mode)
		AddHandler Me.properties.OnStateChange, AddressOf MyBase.zHack_OnStateChanged
		AddHandler Me.properties.OnBossDeath, AddressOf MyBase.zHack_OnWin
		MyBase.timeline = Me.properties.CreateTimeline(MyBase.mode)
		Me.goalTimes = Me.properties.goalTimes
		AddHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		MyBase.PartialInit()
	End Sub

	' Token: 0x17000121 RID: 289
	' (get) Token: 0x060006D0 RID: 1744 RVA: 0x00071C66 File Offset: 0x00070066
	Public Overrides ReadOnly Property CurrentLevel As Levels
		Get
			Return Levels.Robot
		End Get
	End Property

	' Token: 0x17000122 RID: 290
	' (get) Token: 0x060006D1 RID: 1745 RVA: 0x00071C6D File Offset: 0x0007006D
	Public Overrides ReadOnly Property CurrentScene As Scenes
		Get
			Return Scenes.scene_level_robot
		End Get
	End Property

	' Token: 0x17000123 RID: 291
	' (get) Token: 0x060006D2 RID: 1746 RVA: 0x00071C74 File Offset: 0x00070074
	Public Overrides ReadOnly Property BossPortrait As Sprite
		Get
			Select Case Me.properties.CurrentState.stateName
				Case LevelProperties.Robot.States.Main, LevelProperties.Robot.States.Generic
					Return Me._bossPortraitMain
				Case LevelProperties.Robot.States.HeliHead
					Return Me._bossPortraitHeliHead
				Case LevelProperties.Robot.States.Inventor
					Return Me._bossPortraitInventor
				Case Else
					Global.Debug.LogError("Couldn't find portrait for state " + Me.properties.CurrentState.stateName + ". Using Main.", Nothing)
					Return Me._bossPortraitMain
			End Select
		End Get
	End Property

	' Token: 0x17000124 RID: 292
	' (get) Token: 0x060006D3 RID: 1747 RVA: 0x00071CF4 File Offset: 0x000700F4
	Public Overrides ReadOnly Property BossQuote As String
		Get
			Select Case Me.properties.CurrentState.stateName
				Case LevelProperties.Robot.States.Main, LevelProperties.Robot.States.Generic
					Return Me._bossQuoteMain
				Case LevelProperties.Robot.States.HeliHead
					Return Me._bossQuoteHeliHead
				Case LevelProperties.Robot.States.Inventor
					Return Me._bossQuoteInventor
				Case Else
					Global.Debug.LogError("Couldn't find quote for state " + Me.properties.CurrentState.stateName + ". Using Main.", Nothing)
					Return Me._bossQuoteMain
			End Select
		End Get
	End Property

	' Token: 0x060006D4 RID: 1748 RVA: 0x00071D74 File Offset: 0x00070174
	Protected Overrides Sub Start()
		MyBase.Start()
		RemoveHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		Dim array As Single() = New Single(MyBase.timeline.events.Count - 1) {}
		For i As Integer = 0 To MyBase.timeline.events.Count - 1
			array(i) = MyBase.timeline.events(i).percentage
		Next
		MyBase.timeline = New Level.Timeline()
		MyBase.timeline.health = 0F
		MyBase.timeline.health += CSng(Me.properties.CurrentState.hose.health)
		MyBase.timeline.health += CSng(Me.properties.CurrentState.orb.chestHP)
		MyBase.timeline.health += CSng(Me.properties.CurrentState.shotBot.hatchGateHealth)
		MyBase.timeline.health += CSng(Me.properties.CurrentState.heart.heartHP)
		Dim num As Single = MyBase.timeline.health
		If Level.Current.mode <> Level.Mode.Easy Then
			For j As Integer = 0 To array.Length - 1
				Dim num2 As Single = Me.properties.TotalHealth * If((j >= array.Length - 1), array(j), (array(j) - array(j + 1)))
				Level.Current.timeline.health += num2
			Next
			MyBase.timeline.AddEvent(New Level.Timeline.[Event](String.Empty, 1F - num / Level.Current.timeline.health))
			For k As Integer = 0 To array.Length - 1
				num += Me.properties.TotalHealth * If((k >= array.Length - 1), array(k), (array(k) - array(k + 1)))
				If k < array.Length - 1 Then
					MyBase.timeline.AddEvent(New Level.Timeline.[Event](String.Empty, 1F - num / Level.Current.timeline.health))
				End If
			Next
		End If
		Me.robot.LevelInit(Me.properties)
	End Sub

	' Token: 0x060006D5 RID: 1749 RVA: 0x00071FDB File Offset: 0x000703DB
	Protected Overrides Sub OnLevelStart()
		MyBase.StartCoroutine(Me.robotPattern_cr())
	End Sub

	' Token: 0x060006D6 RID: 1750 RVA: 0x00071FEC File Offset: 0x000703EC
	Protected Overrides Sub OnStateChanged()
		MyBase.OnStateChanged()
		Dim stateName As LevelProperties.Robot.States = Me.properties.CurrentState.stateName
		If stateName <> LevelProperties.Robot.States.HeliHead Then
			If stateName = LevelProperties.Robot.States.Inventor Then
				Me.heliHead.ChangeState()
			End If
		Else
			Me.StopAllCoroutines()
			Me.robot.TriggerPhaseTwo(AddressOf Me.OnHeliheadSpawn)
		End If
	End Sub

	' Token: 0x060006D7 RID: 1751 RVA: 0x0007205A File Offset: 0x0007045A
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me._bossPortraitHeliHead = Nothing
		Me._bossPortraitInventor = Nothing
		Me._bossPortraitMain = Nothing
	End Sub

	' Token: 0x060006D8 RID: 1752 RVA: 0x00072078 File Offset: 0x00070478
	Private Iterator Function robotPattern_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		While True
			Yield MyBase.StartCoroutine(Me.nextPattern_cr())
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060006D9 RID: 1753 RVA: 0x00072094 File Offset: 0x00070494
	Private Iterator Function nextPattern_cr() As IEnumerator
		Dim p As LevelProperties.Robot.Pattern = Me.properties.CurrentState.NextPattern
		If p <> LevelProperties.Robot.Pattern.[Default] Then
			Yield CupheadTime.WaitForSeconds(Me, 1F)
		Else
			Yield Nothing
		End If
		Return
	End Function

	' Token: 0x060006DA RID: 1754 RVA: 0x000720AF File Offset: 0x000704AF
	Private Sub OnHeliheadSpawn()
		MyBase.StartCoroutine(Me.spawnHeliHead_cr())
	End Sub

	' Token: 0x060006DB RID: 1755 RVA: 0x000720C0 File Offset: 0x000704C0
	Private Iterator Function spawnHeliHead_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 2.5F)
		Me.robot.animator.SetTrigger("Phase2Transition")
		Yield Me.robot.animator.WaitForAnimationToEnd(Me, "Death Dance", True, True)
		Me.heliHead.GetComponent(Of RobotLevelHelihead)().InitHeliHead(Me.properties)
		Return
	End Function

	' Token: 0x04000D76 RID: 3446
	Private properties As LevelProperties.Robot

	' Token: 0x04000D77 RID: 3447
	<SerializeField()>
	Private robot As RobotLevelRobot

	' Token: 0x04000D78 RID: 3448
	<SerializeField()>
	Private heliHead As RobotLevelHelihead

	' Token: 0x04000D79 RID: 3449
	<Header("Boss Info")>
	<SerializeField()>
	Private _bossPortraitMain As Sprite

	' Token: 0x04000D7A RID: 3450
	<SerializeField()>
	Private _bossPortraitHeliHead As Sprite

	' Token: 0x04000D7B RID: 3451
	<SerializeField()>
	Private _bossPortraitInventor As Sprite

	' Token: 0x04000D7C RID: 3452
	<SerializeField()>
	Private _bossQuoteMain As String

	' Token: 0x04000D7D RID: 3453
	<SerializeField()>
	Private _bossQuoteHeliHead As String

	' Token: 0x04000D7E RID: 3454
	<SerializeField()>
	Private _bossQuoteInventor As String
End Class
