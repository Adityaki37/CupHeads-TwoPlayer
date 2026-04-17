Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200015E RID: 350
Public Class DicePalaceRouletteLevel
	Inherits AbstractDicePalaceLevel

	' Token: 0x060003F8 RID: 1016 RVA: 0x0006153C File Offset: 0x0005F93C
	Protected Overrides Sub PartialInit()
		Me.properties = LevelProperties.DicePalaceRoulette.GetMode(MyBase.mode)
		AddHandler Me.properties.OnStateChange, AddressOf MyBase.zHack_OnStateChanged
		AddHandler Me.properties.OnBossDeath, AddressOf MyBase.zHack_OnWin
		MyBase.timeline = Me.properties.CreateTimeline(MyBase.mode)
		Me.goalTimes = Me.properties.goalTimes
		AddHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		MyBase.PartialInit()
	End Sub

	' Token: 0x170000BB RID: 187
	' (get) Token: 0x060003F9 RID: 1017 RVA: 0x000615D2 File Offset: 0x0005F9D2
	Public Overrides ReadOnly Property CurrentDicePalaceLevel As DicePalaceLevels
		Get
			Return DicePalaceLevels.DicePalaceRoulette
		End Get
	End Property

	' Token: 0x170000BC RID: 188
	' (get) Token: 0x060003FA RID: 1018 RVA: 0x000615D9 File Offset: 0x0005F9D9
	Public Overrides ReadOnly Property CurrentLevel As Levels
		Get
			Return Levels.DicePalaceRoulette
		End Get
	End Property

	' Token: 0x170000BD RID: 189
	' (get) Token: 0x060003FB RID: 1019 RVA: 0x000615E0 File Offset: 0x0005F9E0
	Public Overrides ReadOnly Property CurrentScene As Scenes
		Get
			Return Scenes.scene_level_dice_palace_roulette
		End Get
	End Property

	' Token: 0x170000BE RID: 190
	' (get) Token: 0x060003FC RID: 1020 RVA: 0x000615E4 File Offset: 0x0005F9E4
	Public Overrides ReadOnly Property BossPortrait As Sprite
		Get
			Return Me._bossPortrait
		End Get
	End Property

	' Token: 0x170000BF RID: 191
	' (get) Token: 0x060003FD RID: 1021 RVA: 0x000615EC File Offset: 0x0005F9EC
	Public Overrides ReadOnly Property BossQuote As String
		Get
			Return Me._bossQuote
		End Get
	End Property

	' Token: 0x060003FE RID: 1022 RVA: 0x000615F4 File Offset: 0x0005F9F4
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.roulette.LevelInit(Me.properties)
		For Each dicePalaceRouletteLevelPlatform As DicePalaceRouletteLevelPlatform In Me.platforms
			dicePalaceRouletteLevelPlatform.Init(Me.properties.CurrentState.platform)
		Next
	End Sub

	' Token: 0x060003FF RID: 1023 RVA: 0x0006164D File Offset: 0x0005FA4D
	Protected Overrides Sub OnLevelStart()
		MyBase.StartCoroutine(Me.dicepalaceroulettePattern_cr())
	End Sub

	' Token: 0x06000400 RID: 1024 RVA: 0x0006165C File Offset: 0x0005FA5C
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me._bossPortrait = Nothing
	End Sub

	' Token: 0x06000401 RID: 1025 RVA: 0x0006166C File Offset: 0x0005FA6C
	Private Iterator Function dicepalaceroulettePattern_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		While True
			Yield MyBase.StartCoroutine(Me.nextPattern_cr())
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000402 RID: 1026 RVA: 0x00061688 File Offset: 0x0005FA88
	Private Iterator Function nextPattern_cr() As IEnumerator
		Dim p As LevelProperties.DicePalaceRoulette.Pattern = Me.properties.CurrentState.NextPattern
		If p <> LevelProperties.DicePalaceRoulette.Pattern.Twirl Then
			If p <> LevelProperties.DicePalaceRoulette.Pattern.Marble Then
				Yield CupheadTime.WaitForSeconds(Me, 1F)
			Else
				Yield MyBase.StartCoroutine(Me.marble_cr())
			End If
		Else
			Yield MyBase.StartCoroutine(Me.twirl_cr())
		End If
		Return
	End Function

	' Token: 0x06000403 RID: 1027 RVA: 0x000616A4 File Offset: 0x0005FAA4
	Private Iterator Function twirl_cr() As IEnumerator
		While Me.roulette.state <> DicePalaceRouletteLevelRoulette.State.Idle
			Yield Nothing
		End While
		Me.roulette.StartTwirl()
		While Me.roulette.state <> DicePalaceRouletteLevelRoulette.State.Idle
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000404 RID: 1028 RVA: 0x000616C0 File Offset: 0x0005FAC0
	Private Iterator Function marble_cr() As IEnumerator
		While Me.roulette.state <> DicePalaceRouletteLevelRoulette.State.Idle
			Yield Nothing
		End While
		Me.roulette.StartMarbleDrop()
		While Me.roulette.state <> DicePalaceRouletteLevelRoulette.State.Idle
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x040006AA RID: 1706
	Private properties As LevelProperties.DicePalaceRoulette

	' Token: 0x040006AB RID: 1707
	<SerializeField()>
	Private roulette As DicePalaceRouletteLevelRoulette

	' Token: 0x040006AC RID: 1708
	<SerializeField()>
	Private platforms As DicePalaceRouletteLevelPlatform()

	' Token: 0x040006AD RID: 1709
	<Header("Boss Info")>
	<SerializeField()>
	Private _bossPortrait As Sprite

	' Token: 0x040006AE RID: 1710
	<SerializeField()>
	Private _bossQuote As String
End Class
