Imports System
Imports System.Collections
Imports System.Diagnostics
Imports UnityEngine

' Token: 0x0200028A RID: 650
Public Class SallyStagePlayLevel
	Inherits Level

	' Token: 0x06000728 RID: 1832 RVA: 0x00073734 File Offset: 0x00071B34
	Protected Overrides Sub PartialInit()
		Me.properties = LevelProperties.SallyStagePlay.GetMode(MyBase.mode)
		AddHandler Me.properties.OnStateChange, AddressOf MyBase.zHack_OnStateChanged
		AddHandler Me.properties.OnBossDeath, AddressOf MyBase.zHack_OnWin
		MyBase.timeline = Me.properties.CreateTimeline(MyBase.mode)
		Me.goalTimes = Me.properties.goalTimes
		AddHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		MyBase.PartialInit()
	End Sub

	' Token: 0x1700012B RID: 299
	' (get) Token: 0x06000729 RID: 1833 RVA: 0x000737CA File Offset: 0x00071BCA
	Public Overrides ReadOnly Property CurrentLevel As Levels
		Get
			Return Levels.SallyStagePlay
		End Get
	End Property

	' Token: 0x1700012C RID: 300
	' (get) Token: 0x0600072A RID: 1834 RVA: 0x000737D1 File Offset: 0x00071BD1
	Public Overrides ReadOnly Property CurrentScene As Scenes
		Get
			Return Scenes.scene_level_sally_stage_play
		End Get
	End Property

	' Token: 0x14000003 RID: 3
	' (add) Token: 0x0600072B RID: 1835 RVA: 0x000737D8 File Offset: 0x00071BD8
	' (remove) Token: 0x0600072C RID: 1836 RVA: 0x00073810 File Offset: 0x00071C10
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnPhase3 As Action

	' Token: 0x14000004 RID: 4
	' (add) Token: 0x0600072D RID: 1837 RVA: 0x00073848 File Offset: 0x00071C48
	' (remove) Token: 0x0600072E RID: 1838 RVA: 0x00073880 File Offset: 0x00071C80
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnPhase2 As Action

	' Token: 0x14000005 RID: 5
	' (add) Token: 0x0600072F RID: 1839 RVA: 0x000738B8 File Offset: 0x00071CB8
	' (remove) Token: 0x06000730 RID: 1840 RVA: 0x000738F0 File Offset: 0x00071CF0
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnPhase4 As Action

	' Token: 0x1700012D RID: 301
	' (get) Token: 0x06000731 RID: 1841 RVA: 0x00073928 File Offset: 0x00071D28
	Public Overrides ReadOnly Property BossPortrait As Sprite
		Get
			Select Case Me.properties.CurrentState.stateName
				Case LevelProperties.SallyStagePlay.States.Main, LevelProperties.SallyStagePlay.States.Generic
					Return Me._bossPortraitMain
				Case LevelProperties.SallyStagePlay.States.House
					Return Me._bossPortraitHouse
				Case LevelProperties.SallyStagePlay.States.Angel
					Return Me._bossPortraitAngel
				Case LevelProperties.SallyStagePlay.States.Final
					Return Me._bossPortraitFinal
				Case Else
					Global.Debug.LogError("Couldn't find portrait for state " + Me.properties.CurrentState.stateName + ". Using Main.", Nothing)
					Return Me._bossPortraitMain
			End Select
		End Get
	End Property

	' Token: 0x1700012E RID: 302
	' (get) Token: 0x06000732 RID: 1842 RVA: 0x000739B4 File Offset: 0x00071DB4
	Public Overrides ReadOnly Property BossQuote As String
		Get
			Select Case Me.properties.CurrentState.stateName
				Case LevelProperties.SallyStagePlay.States.Main, LevelProperties.SallyStagePlay.States.Generic
					Return Me._bossQuoteMain
				Case LevelProperties.SallyStagePlay.States.House
					Return Me._bossQuoteHouse
				Case LevelProperties.SallyStagePlay.States.Angel
					Return Me._bossQuoteAngel
				Case LevelProperties.SallyStagePlay.States.Final
					Return Me._bossQuoteFinal
				Case Else
					Global.Debug.LogError("Couldn't find quote for state " + Me.properties.CurrentState.stateName + ". Using Main.", Nothing)
					Return Me._bossQuoteMain
			End Select
		End Get
	End Property

	' Token: 0x06000733 RID: 1843 RVA: 0x00073A40 File Offset: 0x00071E40
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.sally.LevelInit(Me.properties)
		Me.sally.GetParent(Me)
		Me.angel.LevelInit(Me.properties)
		Me.backgroundHandler.GetProperties(Me.properties, Me)
		Me.husband.LevelInit(Me.properties)
		MyBase.StartCoroutine(Me.intro_cr())
	End Sub

	' Token: 0x06000734 RID: 1844 RVA: 0x00073AB4 File Offset: 0x00071EB4
	Protected Overrides Sub OnStateChanged()
		MyBase.OnStateChanged()
		If Me.properties.CurrentState.stateName = LevelProperties.SallyStagePlay.States.House Then
			MyBase.StartCoroutine(Me.residence_cr())
		ElseIf Me.properties.CurrentState.stateName = LevelProperties.SallyStagePlay.States.Angel Then
			Me.StopAllCoroutines()
			MyBase.StartCoroutine(Me.angel_cr())
		ElseIf Me.properties.CurrentState.stateName = LevelProperties.SallyStagePlay.States.Final Then
			Me.StopAllCoroutines()
			MyBase.StartCoroutine(Me.final_cr())
		End If
	End Sub

	' Token: 0x06000735 RID: 1845 RVA: 0x00073B46 File Offset: 0x00071F46
	Protected Overrides Sub OnLevelStart()
		MyBase.StartCoroutine(Me.sallystageplayPattern_cr())
	End Sub

	' Token: 0x06000736 RID: 1846 RVA: 0x00073B55 File Offset: 0x00071F55
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me._bossPortraitAngel = Nothing
		Me._bossPortraitFinal = Nothing
		Me._bossPortraitHouse = Nothing
		Me._bossPortraitMain = Nothing
	End Sub

	' Token: 0x06000737 RID: 1847 RVA: 0x00073B7C File Offset: 0x00071F7C
	Private Iterator Function sallystageplayPattern_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		While True
			If Me.sally.state <> SallyStagePlayLevelSally.State.Transition Then
				Yield MyBase.StartCoroutine(Me.nextPattern_cr())
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000738 RID: 1848 RVA: 0x00073B98 File Offset: 0x00071F98
	Private Iterator Function nextPattern_cr() As IEnumerator
		Select Case Me.properties.CurrentState.NextPattern
			Case LevelProperties.SallyStagePlay.Pattern.Jump
				MyBase.StartCoroutine(Me.jump_cr())
			Case LevelProperties.SallyStagePlay.Pattern.Umbrella
				MyBase.StartCoroutine(Me.umbrella_cr())
			Case LevelProperties.SallyStagePlay.Pattern.Kiss
				MyBase.StartCoroutine(Me.kiss_cr())
			Case LevelProperties.SallyStagePlay.Pattern.Teleport
				MyBase.StartCoroutine(Me.teleport_cr())
			Case Else
				Yield CupheadTime.WaitForSeconds(Me, 1F)
		End Select
		Return
	End Function

	' Token: 0x06000739 RID: 1849 RVA: 0x00073BB4 File Offset: 0x00071FB4
	Private Iterator Function intro_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 2.2F)
		Me.backgroundHandler.OpenCurtain(SallyStagePlayLevelBackgroundHandler.Backgrounds.Church)
		Yield Nothing
		Return
	End Function

	' Token: 0x0600073A RID: 1850 RVA: 0x00073BD0 File Offset: 0x00071FD0
	Private Iterator Function jump_cr() As IEnumerator
		While Me.sally.state <> SallyStagePlayLevelSally.State.Idle
			Yield Nothing
		End While
		Me.sally.OnJumpAttack()
		While Me.sally.state <> SallyStagePlayLevelSally.State.Idle
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600073B RID: 1851 RVA: 0x00073BEC File Offset: 0x00071FEC
	Private Iterator Function umbrella_cr() As IEnumerator
		While Me.sally.state <> SallyStagePlayLevelSally.State.Idle
			Yield Nothing
		End While
		Me.sally.OnUmbrellaAttack()
		While Me.sally.state <> SallyStagePlayLevelSally.State.Idle
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600073C RID: 1852 RVA: 0x00073C08 File Offset: 0x00072008
	Private Iterator Function kiss_cr() As IEnumerator
		While Me.sally.state <> SallyStagePlayLevelSally.State.Idle
			Yield Nothing
		End While
		Me.sally.OnKissAttack()
		While Me.sally.state <> SallyStagePlayLevelSally.State.Idle
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600073D RID: 1853 RVA: 0x00073C24 File Offset: 0x00072024
	Private Iterator Function teleport_cr() As IEnumerator
		While Me.sally.state <> SallyStagePlayLevelSally.State.Idle
			Yield Nothing
		End While
		Me.sally.OnTeleportAttack()
		While Me.sally.state <> SallyStagePlayLevelSally.State.Idle
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600073E RID: 1854 RVA: 0x00073C40 File Offset: 0x00072040
	Private Iterator Function residence_cr() As IEnumerator
		Me.backgroundHandler.RollUpCupids()
		Me.sally.PrePhase2()
		Me.secretTriggered = SallyStagePlayLevelBackgroundHandler.HUSBAND_GONE
		While Me.sally.state <> SallyStagePlayLevelSally.State.Idle
			Yield Nothing
		End While
		If Me.OnPhase2 IsNot Nothing Then
			Me.OnPhase2()
			Me.StopAllCoroutines()
			MyBase.StartCoroutine(Me.sallystageplayPattern_cr())
		End If
		Yield Nothing
		Return
	End Function

	' Token: 0x0600073F RID: 1855 RVA: 0x00073C5C File Offset: 0x0007205C
	Private Iterator Function angel_cr() As IEnumerator
		If Me.OnPhase3 IsNot Nothing Then
			Me.OnPhase3()
		End If
		Me.sally.OnPhase3(SallyStagePlayLevelBackgroundHandler.HUSBAND_GONE)
		Yield Nothing
		Return
	End Function

	' Token: 0x06000740 RID: 1856 RVA: 0x00073C78 File Offset: 0x00072078
	Private Iterator Function final_cr() As IEnumerator
		Me.angel.OnPhase4()
		If Me.OnPhase4 IsNot Nothing Then
			Me.OnPhase4()
		End If
		Yield Nothing
		AudioManager.PlayLoop("sally_audience_applause_ph4_loop")
		Return
	End Function

	' Token: 0x04000E76 RID: 3702
	Private properties As LevelProperties.SallyStagePlay

	' Token: 0x04000E7A RID: 3706
	<SerializeField()>
	Private backgroundHandler As SallyStagePlayLevelBackgroundHandler

	' Token: 0x04000E7B RID: 3707
	<SerializeField()>
	Private angel As SallyStagePlayLevelAngel

	' Token: 0x04000E7C RID: 3708
	<SerializeField()>
	Private sally As SallyStagePlayLevelSally

	' Token: 0x04000E7D RID: 3709
	<SerializeField()>
	Private husband As SallyStagePlayLevelFianceDeity

	' Token: 0x04000E7E RID: 3710
	<Header("Boss Info")>
	<SerializeField()>
	Private _bossPortraitMain As Sprite

	' Token: 0x04000E7F RID: 3711
	<SerializeField()>
	Private _bossPortraitHouse As Sprite

	' Token: 0x04000E80 RID: 3712
	<SerializeField()>
	Private _bossPortraitAngel As Sprite

	' Token: 0x04000E81 RID: 3713
	<SerializeField()>
	Private _bossPortraitFinal As Sprite

	' Token: 0x04000E82 RID: 3714
	<SerializeField()>
	Private _bossQuoteMain As String

	' Token: 0x04000E83 RID: 3715
	<SerializeField()>
	Private _bossQuoteHouse As String

	' Token: 0x04000E84 RID: 3716
	<SerializeField()>
	Private _bossQuoteAngel As String

	' Token: 0x04000E85 RID: 3717
	<SerializeField()>
	Private _bossQuoteFinal As String
End Class
