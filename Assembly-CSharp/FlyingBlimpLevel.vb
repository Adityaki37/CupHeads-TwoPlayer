Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020001A4 RID: 420
Public Class FlyingBlimpLevel
	Inherits Level

	' Token: 0x060004A9 RID: 1193 RVA: 0x00064B64 File Offset: 0x00062F64
	Protected Overrides Sub PartialInit()
		Me.properties = LevelProperties.FlyingBlimp.GetMode(MyBase.mode)
		AddHandler Me.properties.OnStateChange, AddressOf MyBase.zHack_OnStateChanged
		AddHandler Me.properties.OnBossDeath, AddressOf MyBase.zHack_OnWin
		MyBase.timeline = Me.properties.CreateTimeline(MyBase.mode)
		Me.goalTimes = Me.properties.goalTimes
		AddHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		MyBase.PartialInit()
	End Sub

	' Token: 0x170000D7 RID: 215
	' (get) Token: 0x060004AA RID: 1194 RVA: 0x00064BFA File Offset: 0x00062FFA
	Public Overrides ReadOnly Property CurrentLevel As Levels
		Get
			Return Levels.FlyingBlimp
		End Get
	End Property

	' Token: 0x170000D8 RID: 216
	' (get) Token: 0x060004AB RID: 1195 RVA: 0x00064C01 File Offset: 0x00063001
	Public Overrides ReadOnly Property CurrentScene As Scenes
		Get
			Return Scenes.scene_level_flying_blimp
		End Get
	End Property

	' Token: 0x170000D9 RID: 217
	' (get) Token: 0x060004AC RID: 1196 RVA: 0x00064C08 File Offset: 0x00063008
	Public Overrides ReadOnly Property BossPortrait As Sprite
		Get
			Select Case Me.properties.CurrentState.stateName
				Case LevelProperties.FlyingBlimp.States.Main, LevelProperties.FlyingBlimp.States.Generic
					Return Me._bossPortraitMain
				Case LevelProperties.FlyingBlimp.States.Moon
					Return Me._bossPortraitMoon
				Case LevelProperties.FlyingBlimp.States.Sagittarius, LevelProperties.FlyingBlimp.States.Taurus, LevelProperties.FlyingBlimp.States.Gemini, LevelProperties.FlyingBlimp.States.SagOrGem
					Return Me._bossPortraitMagical
				Case Else
					Global.Debug.LogError("Couldn't find portrait for state " + Me.properties.CurrentState.stateName + ". Using Main.", Nothing)
					Return Me._bossPortraitMain
			End Select
		End Get
	End Property

	' Token: 0x170000DA RID: 218
	' (get) Token: 0x060004AD RID: 1197 RVA: 0x00064C94 File Offset: 0x00063094
	Public Overrides ReadOnly Property BossQuote As String
		Get
			Select Case Me.properties.CurrentState.stateName
				Case LevelProperties.FlyingBlimp.States.Main, LevelProperties.FlyingBlimp.States.Generic
					Return Me._bossQuoteMain
				Case LevelProperties.FlyingBlimp.States.Moon
					Return Me._bossQuoteMoon
				Case LevelProperties.FlyingBlimp.States.Sagittarius, LevelProperties.FlyingBlimp.States.Taurus, LevelProperties.FlyingBlimp.States.Gemini, LevelProperties.FlyingBlimp.States.SagOrGem
					Return Me._bossQuoteMagical
				Case Else
					Global.Debug.LogError("Couldn't find quote for state " + Me.properties.CurrentState.stateName + ". Using Main.", Nothing)
					Return Me._bossQuoteMain
			End Select
		End Get
	End Property

	' Token: 0x060004AE RID: 1198 RVA: 0x00064D1E File Offset: 0x0006311E
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.blimpLady.LevelInit(Me.properties)
		Me.moonLady.LevelInit(Me.properties)
	End Sub

	' Token: 0x060004AF RID: 1199 RVA: 0x00064D48 File Offset: 0x00063148
	Protected Overrides Sub OnLevelStart()
		MyBase.StartCoroutine(Me.flyingblimpPattern_cr())
		MyBase.StartCoroutine(Me.enemies_cr())
	End Sub

	' Token: 0x060004B0 RID: 1200 RVA: 0x00064D64 File Offset: 0x00063164
	Protected Overrides Sub OnStateChanged()
		MyBase.OnStateChanged()
		Me.changingStates = True
		Me.StopAllCoroutines()
		MyBase.StopCoroutine(Me.blimpLady.spawnEnemy_cr())
		MyBase.StartCoroutine(Me.enemies_cr())
		If Me.properties.CurrentState.stateName = LevelProperties.FlyingBlimp.States.Moon Then
			MyBase.StartCoroutine(Me.morph_to_moon_cr())
		ElseIf Me.properties.CurrentState.stateName = LevelProperties.FlyingBlimp.States.Sagittarius Then
			MyBase.StartCoroutine(Me.sagittarius_cr())
		ElseIf Me.properties.CurrentState.stateName = LevelProperties.FlyingBlimp.States.Taurus Then
			MyBase.StartCoroutine(Me.taurus_cr())
		ElseIf Me.properties.CurrentState.stateName = LevelProperties.FlyingBlimp.States.Gemini Then
			MyBase.StartCoroutine(Me.gemini_cr())
		ElseIf Me.properties.CurrentState.stateName = LevelProperties.FlyingBlimp.States.SagOrGem Then
			If Rand.Bool() Then
				MyBase.StartCoroutine(Me.sagittarius_cr())
			Else
				MyBase.StartCoroutine(Me.gemini_cr())
			End If
		Else
			MyBase.StartCoroutine(Me.flyingblimpPattern_cr())
		End If
	End Sub

	' Token: 0x060004B1 RID: 1201 RVA: 0x00064E93 File Offset: 0x00063293
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me._bossPortraitMagical = Nothing
		Me._bossPortraitMain = Nothing
		Me._bossPortraitMoon = Nothing
	End Sub

	' Token: 0x060004B2 RID: 1202 RVA: 0x00064EB0 File Offset: 0x000632B0
	Private Iterator Function flyingblimpPattern_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		While True
			Yield MyBase.StartCoroutine(Me.nextPattern_cr())
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060004B3 RID: 1203 RVA: 0x00064ECC File Offset: 0x000632CC
	Private Iterator Function nextPattern_cr() As IEnumerator
		Dim p As LevelProperties.FlyingBlimp.Pattern = Me.properties.CurrentState.NextPattern
		If p <> LevelProperties.FlyingBlimp.Pattern.Tornado Then
			If p <> LevelProperties.FlyingBlimp.Pattern.Shoot Then
				Yield CupheadTime.WaitForSeconds(Me, 1F)
			Else
				Yield MyBase.StartCoroutine(Me.shoot_cr())
			End If
		Else
			Yield MyBase.StartCoroutine(Me.tornado_cr())
		End If
		Return
	End Function

	' Token: 0x060004B4 RID: 1204 RVA: 0x00064EE8 File Offset: 0x000632E8
	Private Iterator Function tornado_cr() As IEnumerator
		While Me.blimpLady.state <> FlyingBlimpLevelBlimpLady.State.Idle
			Yield Nothing
		End While
		Me.blimpLady.StartTornado()
		While Me.blimpLady.state <> FlyingBlimpLevelBlimpLady.State.Idle
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060004B5 RID: 1205 RVA: 0x00064F04 File Offset: 0x00063304
	Private Iterator Function sagittarius_cr() As IEnumerator
		While Me.blimpLady.state <> FlyingBlimpLevelBlimpLady.State.Idle
			Yield Nothing
		End While
		Me.blimpLady.StartSagittarius()
		While Me.blimpLady.state <> FlyingBlimpLevelBlimpLady.State.Idle
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060004B6 RID: 1206 RVA: 0x00064F20 File Offset: 0x00063320
	Private Iterator Function taurus_cr() As IEnumerator
		While Me.blimpLady.state <> FlyingBlimpLevelBlimpLady.State.Idle
			Yield Nothing
		End While
		Me.blimpLady.StartTaurus()
		While Me.blimpLady.state <> FlyingBlimpLevelBlimpLady.State.Idle
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060004B7 RID: 1207 RVA: 0x00064F3C File Offset: 0x0006333C
	Private Iterator Function gemini_cr() As IEnumerator
		While Me.blimpLady.state <> FlyingBlimpLevelBlimpLady.State.Idle
			Yield Nothing
		End While
		Me.blimpLady.StartGemini()
		While Me.blimpLady.state <> FlyingBlimpLevelBlimpLady.State.Idle
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060004B8 RID: 1208 RVA: 0x00064F58 File Offset: 0x00063358
	Private Iterator Function shoot_cr() As IEnumerator
		While Me.blimpLady.state <> FlyingBlimpLevelBlimpLady.State.Idle
			Yield Nothing
		End While
		Me.blimpLady.StartShoot()
		While Me.blimpLady.state <> FlyingBlimpLevelBlimpLady.State.Idle
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060004B9 RID: 1209 RVA: 0x00064F74 File Offset: 0x00063374
	Private Iterator Function enemies_cr() As IEnumerator
		If Not Me.properties.CurrentState.enemy.active Then
			Yield Nothing
		Else
			MyBase.StartCoroutine(Me.blimpLady.spawnEnemy_cr())
		End If
		Yield Nothing
		Return
	End Function

	' Token: 0x060004BA RID: 1210 RVA: 0x00064F90 File Offset: 0x00063390
	Private Iterator Function morph_to_moon_cr() As IEnumerator
		While Me.blimpLady.state <> FlyingBlimpLevelBlimpLady.State.Idle
			Yield Nothing
		End While
		Me.blimpLady.SpawnMoonLady()
		While Me.blimpLady.state <> FlyingBlimpLevelBlimpLady.State.Idle
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x04000832 RID: 2098
	Private properties As LevelProperties.FlyingBlimp

	' Token: 0x04000833 RID: 2099
	<Space(10F)>
	<SerializeField()>
	Private blimpLady As FlyingBlimpLevelBlimpLady

	' Token: 0x04000834 RID: 2100
	<SerializeField()>
	Private moonLady As FlyingBlimpLevelMoonLady

	' Token: 0x04000835 RID: 2101
	Public changingStates As Boolean

	' Token: 0x04000836 RID: 2102
	<Header("Boss Info")>
	<SerializeField()>
	Private _bossPortraitMain As Sprite

	' Token: 0x04000837 RID: 2103
	<SerializeField()>
	Private _bossPortraitMagical As Sprite

	' Token: 0x04000838 RID: 2104
	<SerializeField()>
	Private _bossPortraitMoon As Sprite

	' Token: 0x04000839 RID: 2105
	<SerializeField()>
	Private _bossQuoteMain As String

	' Token: 0x0400083A RID: 2106
	<SerializeField()>
	Private _bossQuoteMagical As String

	' Token: 0x0400083B RID: 2107
	<SerializeField()>
	Private _bossQuoteMoon As String
End Class
