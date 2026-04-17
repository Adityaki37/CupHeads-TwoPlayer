Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200021D RID: 541
Public Class MouseLevel
	Inherits Level

	' Token: 0x0600060E RID: 1550 RVA: 0x0006C96C File Offset: 0x0006AD6C
	Protected Overrides Sub PartialInit()
		Me.properties = LevelProperties.Mouse.GetMode(MyBase.mode)
		AddHandler Me.properties.OnStateChange, AddressOf MyBase.zHack_OnStateChanged
		AddHandler Me.properties.OnBossDeath, AddressOf MyBase.zHack_OnWin
		MyBase.timeline = Me.properties.CreateTimeline(MyBase.mode)
		Me.goalTimes = Me.properties.goalTimes
		AddHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		MyBase.PartialInit()
	End Sub

	' Token: 0x1700010C RID: 268
	' (get) Token: 0x0600060F RID: 1551 RVA: 0x0006CA02 File Offset: 0x0006AE02
	Public Overrides ReadOnly Property CurrentLevel As Levels
		Get
			Return Levels.Mouse
		End Get
	End Property

	' Token: 0x1700010D RID: 269
	' (get) Token: 0x06000610 RID: 1552 RVA: 0x0006CA09 File Offset: 0x0006AE09
	Public Overrides ReadOnly Property CurrentScene As Scenes
		Get
			Return Scenes.scene_level_mouse
		End Get
	End Property

	' Token: 0x1700010E RID: 270
	' (get) Token: 0x06000611 RID: 1553 RVA: 0x0006CA10 File Offset: 0x0006AE10
	Public Overrides ReadOnly Property BossPortrait As Sprite
		Get
			Select Case Me.properties.CurrentState.stateName
				Case LevelProperties.Mouse.States.Main, LevelProperties.Mouse.States.Generic
					Return Me._bossPortraitMain
				Case LevelProperties.Mouse.States.BrokenCan
					Return Me._bossPortraitBrokenCan
				Case LevelProperties.Mouse.States.Cat
					Return Me._bossPortraitCat
				Case Else
					Global.Debug.LogError("Couldn't find portrait for state " + Me.properties.CurrentState.stateName + ". Using Main.", Nothing)
					Return Me._bossPortraitMain
			End Select
		End Get
	End Property

	' Token: 0x1700010F RID: 271
	' (get) Token: 0x06000612 RID: 1554 RVA: 0x0006CA90 File Offset: 0x0006AE90
	Public Overrides ReadOnly Property BossQuote As String
		Get
			Select Case Me.properties.CurrentState.stateName
				Case LevelProperties.Mouse.States.Main, LevelProperties.Mouse.States.Generic
					Return Me._bossQuoteMain
				Case LevelProperties.Mouse.States.BrokenCan
					Return Me._bossQuoteBrokenCan
				Case LevelProperties.Mouse.States.Cat
					Return Me._bossQuoteCat
				Case Else
					Global.Debug.LogError("Couldn't find quote for state " + Me.properties.CurrentState.stateName + ". Using Main.", Nothing)
					Return Me._bossQuoteMain
			End Select
		End Get
	End Property

	' Token: 0x06000613 RID: 1555 RVA: 0x0006CB0E File Offset: 0x0006AF0E
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.mouseCan.LevelInit(Me.properties)
		Me.mouseBrokenCan.LevelInit(Me.properties)
		Me.cat.LevelInit(Me.properties)
	End Sub

	' Token: 0x06000614 RID: 1556 RVA: 0x0006CB49 File Offset: 0x0006AF49
	Protected Overrides Sub OnLevelStart()
		MyBase.StartCoroutine(Me.mousePattern_cr())
	End Sub

	' Token: 0x06000615 RID: 1557 RVA: 0x0006CB58 File Offset: 0x0006AF58
	Protected Overrides Sub OnStateChanged()
		MyBase.OnStateChanged()
		If Me.properties.CurrentState.stateName = LevelProperties.Mouse.States.BrokenCan Then
			Me.StopAllCoroutines()
			Me.mouseCan.Explode(AddressOf Me.StartMouseCanPlatform, AddressOf Me.OnMouseCanTransitionComplete)
		ElseIf Me.properties.CurrentState.stateName = LevelProperties.Mouse.States.Cat Then
			Me.StopAllCoroutines()
			MyBase.StartCoroutine(Me.catIntro_cr())
		End If
	End Sub

	' Token: 0x06000616 RID: 1558 RVA: 0x0006CBD8 File Offset: 0x0006AFD8
	Private Sub StartMouseCanPlatform()
		Me.wallAnimator.SetTrigger("OnContinue")
		AudioManager.Play("level_mouse_phase2_background_shelf_drop")
	End Sub

	' Token: 0x06000617 RID: 1559 RVA: 0x0006CBF4 File Offset: 0x0006AFF4
	Private Sub OnMouseCanTransitionComplete()
		MyBase.StartCoroutine(Me.mousePattern_cr())
	End Sub

	' Token: 0x06000618 RID: 1560 RVA: 0x0006CC03 File Offset: 0x0006B003
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me._bossPortraitBrokenCan = Nothing
		Me._bossPortraitCat = Nothing
		Me._bossPortraitMain = Nothing
	End Sub

	' Token: 0x06000619 RID: 1561 RVA: 0x0006CC20 File Offset: 0x0006B020
	Private Iterator Function mousePattern_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, Me.properties.CurrentState.canMove.initialHesitate)
		While True
			Yield MyBase.StartCoroutine(Me.nextPattern_cr())
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600061A RID: 1562 RVA: 0x0006CC3C File Offset: 0x0006B03C
	Private Iterator Function nextPattern_cr() As IEnumerator
		Dim p As LevelProperties.Mouse.Pattern = Me.properties.CurrentState.NextPattern
		If p <> LevelProperties.Mouse.Pattern.Dash OrElse Me.hasMoved Then
			Me.hasMoved = True
			Select Case p
				Case LevelProperties.Mouse.Pattern.Dash
					Yield MyBase.StartCoroutine(Me.canDash_cr())
				Case LevelProperties.Mouse.Pattern.CherryBomb
					Yield MyBase.StartCoroutine(Me.cherryBomb_cr())
				Case LevelProperties.Mouse.Pattern.Catapult
					Yield MyBase.StartCoroutine(Me.catapult_cr())
				Case LevelProperties.Mouse.Pattern.RomanCandle
					Yield MyBase.StartCoroutine(Me.romanCandle_cr())
				Case Else
					Yield CupheadTime.WaitForSeconds(Me, 1F)
				Case LevelProperties.Mouse.Pattern.LeftClaw
					Yield MyBase.StartCoroutine(Me.claw_cr(True))
				Case LevelProperties.Mouse.Pattern.RightClaw
					Yield MyBase.StartCoroutine(Me.claw_cr(False))
				Case LevelProperties.Mouse.Pattern.GhostMouse
					Yield MyBase.StartCoroutine(Me.ghostMouse_cr())
			End Select
		End If
		Return
	End Function

	' Token: 0x0600061B RID: 1563 RVA: 0x0006CC58 File Offset: 0x0006B058
	Private Iterator Function canDash_cr() As IEnumerator
		While Me.mouseCan.state <> MouseLevelCanMouse.State.Idle
			Yield Nothing
		End While
		Me.mouseCan.StartDash()
		While Me.mouseCan.state <> MouseLevelCanMouse.State.Idle
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600061C RID: 1564 RVA: 0x0006CC74 File Offset: 0x0006B074
	Private Iterator Function cherryBomb_cr() As IEnumerator
		While Me.mouseCan.state <> MouseLevelCanMouse.State.Idle
			Yield Nothing
		End While
		Me.mouseCan.StartCherryBomb()
		While Me.mouseCan.state <> MouseLevelCanMouse.State.Idle
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600061D RID: 1565 RVA: 0x0006CC90 File Offset: 0x0006B090
	Private Iterator Function catapult_cr() As IEnumerator
		While Me.mouseCan.state <> MouseLevelCanMouse.State.Idle
			Yield Nothing
		End While
		Me.mouseCan.StartCatapult()
		While Me.mouseCan.state <> MouseLevelCanMouse.State.Idle
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600061E RID: 1566 RVA: 0x0006CCAC File Offset: 0x0006B0AC
	Private Iterator Function romanCandle_cr() As IEnumerator
		While Me.mouseCan.state <> MouseLevelCanMouse.State.Idle
			Yield Nothing
		End While
		Me.mouseCan.StartRomanCandle()
		While Me.mouseCan.state <> MouseLevelCanMouse.State.Idle
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600061F RID: 1567 RVA: 0x0006CCC8 File Offset: 0x0006B0C8
	Private Iterator Function claw_cr(left As Boolean) As IEnumerator
		While Me.cat.state <> MouseLevelCat.State.Idle
			Yield Nothing
		End While
		Me.cat.StartClaw(left)
		While Me.cat.state <> MouseLevelCat.State.Idle
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000620 RID: 1568 RVA: 0x0006CCEC File Offset: 0x0006B0EC
	Private Iterator Function ghostMouse_cr() As IEnumerator
		While Me.cat.state <> MouseLevelCat.State.Idle
			Yield Nothing
		End While
		Me.cat.StartGhostMouse()
		While Me.cat.state <> MouseLevelCat.State.Idle
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000621 RID: 1569 RVA: 0x0006CD08 File Offset: 0x0006B108
	Private Iterator Function catIntro_cr() As IEnumerator
		Me.mouseBrokenCan.Transform()
		While Me.cat.state <> MouseLevelCat.State.Idle
			Yield Nothing
		End While
		MyBase.StartCoroutine(Me.mousePattern_cr())
		Return
	End Function

	' Token: 0x04000B37 RID: 2871
	Private properties As LevelProperties.Mouse

	' Token: 0x04000B38 RID: 2872
	<SerializeField()>
	Private mouseCan As MouseLevelCanMouse

	' Token: 0x04000B39 RID: 2873
	<SerializeField()>
	Private mouseBrokenCan As MouseLevelBrokenCanMouse

	' Token: 0x04000B3A RID: 2874
	<SerializeField()>
	Private cat As MouseLevelCat

	' Token: 0x04000B3B RID: 2875
	<SerializeField()>
	Private wallAnimator As Animator

	' Token: 0x04000B3C RID: 2876
	<Header("Boss Info")>
	<SerializeField()>
	Private _bossPortraitMain As Sprite

	' Token: 0x04000B3D RID: 2877
	<SerializeField()>
	Private _bossPortraitBrokenCan As Sprite

	' Token: 0x04000B3E RID: 2878
	<SerializeField()>
	Private _bossPortraitCat As Sprite

	' Token: 0x04000B3F RID: 2879
	<SerializeField()>
	Private _bossQuoteMain As String

	' Token: 0x04000B40 RID: 2880
	<SerializeField()>
	Private _bossQuoteBrokenCan As String

	' Token: 0x04000B41 RID: 2881
	<SerializeField()>
	Private _bossQuoteCat As String

	' Token: 0x04000B42 RID: 2882
	Private hasMoved As Boolean

	' Token: 0x020006DB RID: 1755
	<Serializable()>
	Public Class Prefabs
	End Class
End Class
