Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x02000049 RID: 73
Public Class BaronessLevel
	Inherits Level

	' Token: 0x060000C0 RID: 192 RVA: 0x00054D38 File Offset: 0x00053138
	Protected Overrides Sub PartialInit()
		Me.properties = LevelProperties.Baroness.GetMode(MyBase.mode)
		AddHandler Me.properties.OnStateChange, AddressOf MyBase.zHack_OnStateChanged
		AddHandler Me.properties.OnBossDeath, AddressOf MyBase.zHack_OnWin
		MyBase.timeline = Me.properties.CreateTimeline(MyBase.mode)
		Me.goalTimes = Me.properties.goalTimes
		AddHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		MyBase.PartialInit()
	End Sub

	' Token: 0x1700001D RID: 29
	' (get) Token: 0x060000C1 RID: 193 RVA: 0x00054DCE File Offset: 0x000531CE
	Public Overrides ReadOnly Property CurrentLevel As Levels
		Get
			Return Levels.Baroness
		End Get
	End Property

	' Token: 0x1700001E RID: 30
	' (get) Token: 0x060000C2 RID: 194 RVA: 0x00054DD5 File Offset: 0x000531D5
	Public Overrides ReadOnly Property CurrentScene As Scenes
		Get
			Return Scenes.scene_level_baroness
		End Get
	End Property

	' Token: 0x1700001F RID: 31
	' (get) Token: 0x060000C3 RID: 195 RVA: 0x00054DD9 File Offset: 0x000531D9
	' (set) Token: 0x060000C4 RID: 196 RVA: 0x00054DE0 File Offset: 0x000531E0
	Public Shared Property PICKED_BOSSES As List(Of String)

	' Token: 0x17000020 RID: 32
	' (get) Token: 0x060000C5 RID: 197 RVA: 0x00054DE8 File Offset: 0x000531E8
	Public Overrides ReadOnly Property BossPortrait As Sprite
		Get
			Select Case Me.properties.CurrentState.stateName
				Case LevelProperties.Baroness.States.Main, LevelProperties.Baroness.States.Generic
					If Me.currentMiniBoss Is Nothing Then
						Return Me._bossPortraitChase
					End If
					If Me.currentMiniBoss.bossId = BaronessLevelCastle.BossPossibility.Gumball Then
						Return Me._bossPortraitGumball
					End If
					If Me.currentMiniBoss.bossId = BaronessLevelCastle.BossPossibility.Waffle Then
						Return Me._bossPortraitWaffle
					End If
					If Me.currentMiniBoss.bossId = BaronessLevelCastle.BossPossibility.CandyCorn Then
						Return Me._bossPortraitCandyCorn
					End If
					If Me.currentMiniBoss.bossId = BaronessLevelCastle.BossPossibility.Cupcake Then
						Return Me._bossPortraitCupcake
					End If
					If Me.currentMiniBoss.bossId = BaronessLevelCastle.BossPossibility.Jawbreaker Then
						Return Me._bossPortraitJawbreaker
					End If
					Return Me._bossPortraitChase
				Case LevelProperties.Baroness.States.Chase
					Return Me._bossPortraitChase
				Case Else
					Global.Debug.LogError("Couldn't find portrait for state " + Me.properties.CurrentState.stateName + ". Using Main.", Nothing)
					Return Me._bossPortraitChase
			End Select
		End Get
	End Property

	' Token: 0x17000021 RID: 33
	' (get) Token: 0x060000C6 RID: 198 RVA: 0x00054EEC File Offset: 0x000532EC
	Public Overrides ReadOnly Property BossQuote As String
		Get
			Select Case Me.properties.CurrentState.stateName
				Case LevelProperties.Baroness.States.Main, LevelProperties.Baroness.States.Generic
					If Me.currentMiniBoss Is Nothing Then
						Return Me._bossQuoteChase
					End If
					If Me.currentMiniBoss.bossId = BaronessLevelCastle.BossPossibility.Gumball Then
						Return Me._bossQuoteGumball
					End If
					If Me.currentMiniBoss.bossId = BaronessLevelCastle.BossPossibility.Waffle Then
						Return Me._bossQuoteWaffle
					End If
					If Me.currentMiniBoss.bossId = BaronessLevelCastle.BossPossibility.CandyCorn Then
						Return Me._bossQuoteCandyCorn
					End If
					If Me.currentMiniBoss.bossId = BaronessLevelCastle.BossPossibility.Cupcake Then
						Return Me._bossQuoteCupcake
					End If
					If Me.currentMiniBoss.bossId = BaronessLevelCastle.BossPossibility.Jawbreaker Then
						Return Me._bossQuoteJawbreaker
					End If
					Return Me._bossQuoteChase
				Case LevelProperties.Baroness.States.Chase
					Return Me._bossQuoteChase
				Case Else
					Global.Debug.LogError("Couldn't find quote for state " + Me.properties.CurrentState.stateName + ". Using Main.", Nothing)
					Return Me._bossQuoteChase
			End Select
		End Get
	End Property

	' Token: 0x060000C7 RID: 199 RVA: 0x00054FEF File Offset: 0x000533EF
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.castle.LevelInit(Me.properties)
		Me.PickMiniBosses()
	End Sub

	' Token: 0x060000C8 RID: 200 RVA: 0x0005500E File Offset: 0x0005340E
	Public Sub PickMiniBosses()
		MyBase.StartCoroutine(Me.pickminibosses_cr())
	End Sub

	' Token: 0x060000C9 RID: 201 RVA: 0x00055020 File Offset: 0x00053420
	Private Iterator Function update_current_boss_cr() As IEnumerator
		While Me.properties.CurrentState.stateName <> LevelProperties.Baroness.States.Chase
			While BaronessLevelCastle.CURRENT_MINI_BOSS Is Me.currentMiniBoss AndAlso BaronessLevelCastle.CURRENT_MINI_BOSS IsNot Nothing
				Yield Nothing
			End While
			Me.currentMiniBoss = BaronessLevelCastle.CURRENT_MINI_BOSS
			If Me.currentMiniBoss IsNot Nothing Then
				AddHandler Me.currentMiniBoss.OnDamageTakenEvent, AddressOf MyBase.timeline.DealDamage
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060000CA RID: 202 RVA: 0x0005503C File Offset: 0x0005343C
	Private Iterator Function pickminibosses_cr() As IEnumerator
		Dim p As LevelProperties.Baroness.Open = Me.properties.CurrentState.open
		Dim pattern As String() = p.miniBossString.GetRandom().Split(New Char() { ","c })
		Dim randIndex As Integer = 0
		Dim tempList As List(Of String) = New List(Of String)(pattern)
		BaronessLevel.PICKED_BOSSES = New List(Of String)()
		For i As Integer = 0 To p.miniBossAmount - 1
			randIndex = Global.UnityEngine.Random.Range(0, tempList.ToArray().Length)
			BaronessLevel.PICKED_BOSSES.Add(tempList(randIndex))
			tempList.Remove(tempList(randIndex))
		Next
		Me.SetUpTimeline()
		Yield Nothing
		Return
	End Function

	' Token: 0x060000CB RID: 203 RVA: 0x00055058 File Offset: 0x00053458
	Private Sub SetUpTimeline()
		RemoveHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		MyBase.timeline = New Level.Timeline()
		MyBase.timeline.health = 0F
		Dim list As List(Of Single) = New List(Of Single)()
		For i As Integer = 0 To BaronessLevel.PICKED_BOSSES.Count - 1
			Dim text As String = BaronessLevel.PICKED_BOSSES(i)
			If text IsNot Nothing Then
				If Not(text = "1") Then
					If Not(text = "2") Then
						If Not(text = "3") Then
							If Not(text = "4") Then
								If text = "5" Then
									MyBase.timeline.health += CSng(Me.properties.CurrentState.jawbreaker.jawbreakerHomingHP)
									list.Add(CSng(Me.properties.CurrentState.jawbreaker.jawbreakerHomingHP))
								End If
							Else
								MyBase.timeline.health += CSng(Me.properties.CurrentState.cupcake.HP)
								list.Add(CSng(Me.properties.CurrentState.cupcake.HP))
							End If
						Else
							MyBase.timeline.health += CSng(Me.properties.CurrentState.candyCorn.HP)
							list.Add(CSng(Me.properties.CurrentState.candyCorn.HP))
						End If
					Else
						MyBase.timeline.health += CSng(Me.properties.CurrentState.waffle.HP)
						list.Add(CSng(Me.properties.CurrentState.waffle.HP))
					End If
				Else
					MyBase.timeline.health += CSng(Me.properties.CurrentState.gumball.HP)
					list.Add(CSng(Me.properties.CurrentState.gumball.HP))
				End If
			End If
		Next
		MyBase.timeline.health += Me.properties.CurrentHealth
		For j As Integer = 0 To BaronessLevel.PICKED_BOSSES.Count - 1
			MyBase.timeline.AddEventAtHealth(BaronessLevel.PICKED_BOSSES(j), MyBase.timeline.GetHealthOfLastEvent() + CInt(list(j)))
		Next
		AddHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		Me.castle.StartIntro()
		MyBase.StartCoroutine(Me.update_current_boss_cr())
	End Sub

	' Token: 0x060000CC RID: 204 RVA: 0x00055327 File Offset: 0x00053727
	Protected Overrides Sub OnStateChanged()
		MyBase.OnStateChanged()
		If Me.properties.CurrentState.stateName = LevelProperties.Baroness.States.Chase Then
			Me.StopAllCoroutines()
			MyBase.StartCoroutine(Me.chase_cr())
		End If
	End Sub

	' Token: 0x060000CD RID: 205 RVA: 0x00055358 File Offset: 0x00053758
	Protected Overrides Sub OnLevelStart()
		MyBase.StartCoroutine(Me.baronessPattern_cr())
	End Sub

	' Token: 0x060000CE RID: 206 RVA: 0x00055367 File Offset: 0x00053767
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me._bossPortraitCandyCorn = Nothing
		Me._bossPortraitChase = Nothing
		Me._bossPortraitCupcake = Nothing
		Me._bossPortraitGumball = Nothing
		Me._bossPortraitJawbreaker = Nothing
		Me._bossPortraitWaffle = Nothing
	End Sub

	' Token: 0x060000CF RID: 207 RVA: 0x0005539C File Offset: 0x0005379C
	Private Iterator Function baronessPattern_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		While True
			Yield MyBase.StartCoroutine(Me.nextPattern_cr())
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060000D0 RID: 208 RVA: 0x000553B8 File Offset: 0x000537B8
	Private Iterator Function nextPattern_cr() As IEnumerator
		Dim p As LevelProperties.Baroness.Pattern = Me.properties.CurrentState.NextPattern
		If p <> LevelProperties.Baroness.Pattern.[Default] Then
			Yield CupheadTime.WaitForSeconds(Me, 1F)
		Else
			Yield Nothing
		End If
		Return
	End Function

	' Token: 0x060000D1 RID: 209 RVA: 0x000553D4 File Offset: 0x000537D4
	Private Iterator Function chase_cr() As IEnumerator
		Me.castle.StartChase()
		Yield Nothing
		Return
	End Function

	' Token: 0x040001B8 RID: 440
	Private properties As LevelProperties.Baroness

	' Token: 0x040001BA RID: 442
	<SerializeField()>
	Private castle As BaronessLevelCastle

	' Token: 0x040001BB RID: 443
	Private currentMiniBoss As BaronessLevelMiniBossBase

	' Token: 0x040001BC RID: 444
	<Header("Boss Info")>
	<SerializeField()>
	Private _bossPortraitGumball As Sprite

	' Token: 0x040001BD RID: 445
	<SerializeField()>
	Private _bossPortraitWaffle As Sprite

	' Token: 0x040001BE RID: 446
	<SerializeField()>
	Private _bossPortraitCandyCorn As Sprite

	' Token: 0x040001BF RID: 447
	<SerializeField()>
	Private _bossPortraitCupcake As Sprite

	' Token: 0x040001C0 RID: 448
	<SerializeField()>
	Private _bossPortraitJawbreaker As Sprite

	' Token: 0x040001C1 RID: 449
	<SerializeField()>
	Private _bossPortraitChase As Sprite

	' Token: 0x040001C2 RID: 450
	<SerializeField()>
	Private _bossQuoteGumball As String

	' Token: 0x040001C3 RID: 451
	<SerializeField()>
	Private _bossQuoteWaffle As String

	' Token: 0x040001C4 RID: 452
	<SerializeField()>
	Private _bossQuoteCandyCorn As String

	' Token: 0x040001C5 RID: 453
	<SerializeField()>
	Private _bossQuoteCupcake As String

	' Token: 0x040001C6 RID: 454
	<SerializeField()>
	Private _bossQuoteJawbreaker As String

	' Token: 0x040001C7 RID: 455
	<SerializeField()>
	Private _bossQuoteChase As String
End Class
