Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000155 RID: 341
Public Class DicePalaceRabbitLevel
	Inherits AbstractDicePalaceLevel

	' Token: 0x060003DD RID: 989 RVA: 0x00060FD8 File Offset: 0x0005F3D8
	Protected Overrides Sub PartialInit()
		Me.properties = LevelProperties.DicePalaceRabbit.GetMode(MyBase.mode)
		AddHandler Me.properties.OnStateChange, AddressOf MyBase.zHack_OnStateChanged
		AddHandler Me.properties.OnBossDeath, AddressOf MyBase.zHack_OnWin
		MyBase.timeline = Me.properties.CreateTimeline(MyBase.mode)
		Me.goalTimes = Me.properties.goalTimes
		AddHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		MyBase.PartialInit()
	End Sub

	' Token: 0x170000B5 RID: 181
	' (get) Token: 0x060003DE RID: 990 RVA: 0x0006106E File Offset: 0x0005F46E
	Public Overrides ReadOnly Property CurrentDicePalaceLevel As DicePalaceLevels
		Get
			Return DicePalaceLevels.DicePalaceRabbit
		End Get
	End Property

	' Token: 0x170000B6 RID: 182
	' (get) Token: 0x060003DF RID: 991 RVA: 0x00061075 File Offset: 0x0005F475
	Public Overrides ReadOnly Property CurrentLevel As Levels
		Get
			Return Levels.DicePalaceRabbit
		End Get
	End Property

	' Token: 0x170000B7 RID: 183
	' (get) Token: 0x060003E0 RID: 992 RVA: 0x0006107C File Offset: 0x0005F47C
	Public Overrides ReadOnly Property CurrentScene As Scenes
		Get
			Return Scenes.scene_level_dice_palace_rabbit
		End Get
	End Property

	' Token: 0x170000B8 RID: 184
	' (get) Token: 0x060003E1 RID: 993 RVA: 0x00061080 File Offset: 0x0005F480
	Public Overrides ReadOnly Property BossPortrait As Sprite
		Get
			Return Me._bossPortrait
		End Get
	End Property

	' Token: 0x170000B9 RID: 185
	' (get) Token: 0x060003E2 RID: 994 RVA: 0x00061088 File Offset: 0x0005F488
	Public Overrides ReadOnly Property BossQuote As String
		Get
			Return Me._bossQuote
		End Get
	End Property

	' Token: 0x060003E3 RID: 995 RVA: 0x00061090 File Offset: 0x0005F490
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.rabbit.LevelInit(Me.properties)
	End Sub

	' Token: 0x060003E4 RID: 996 RVA: 0x000610A9 File Offset: 0x0005F4A9
	Protected Overrides Sub OnLevelStart()
		MyBase.StartCoroutine(Me.dicepalacerabbitPattern_cr())
	End Sub

	' Token: 0x060003E5 RID: 997 RVA: 0x000610B8 File Offset: 0x0005F4B8
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me._bossPortrait = Nothing
	End Sub

	' Token: 0x060003E6 RID: 998 RVA: 0x000610C8 File Offset: 0x0005F4C8
	Private Iterator Function dicepalacerabbitPattern_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		While True
			Yield MyBase.StartCoroutine(Me.nextPattern_cr())
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060003E7 RID: 999 RVA: 0x000610E4 File Offset: 0x0005F4E4
	Private Iterator Function nextPattern_cr() As IEnumerator
		Dim p As LevelProperties.DicePalaceRabbit.Pattern = Me.properties.CurrentState.NextPattern
		If p <> LevelProperties.DicePalaceRabbit.Pattern.MagicWand Then
			If p <> LevelProperties.DicePalaceRabbit.Pattern.MagicParry Then
				Yield CupheadTime.WaitForSeconds(Me, 1F)
			Else
				Yield MyBase.StartCoroutine(Me.magicparry_cr())
			End If
		Else
			Yield MyBase.StartCoroutine(Me.magicwand_cr())
		End If
		Return
	End Function

	' Token: 0x060003E8 RID: 1000 RVA: 0x00061100 File Offset: 0x0005F500
	Private Iterator Function magicwand_cr() As IEnumerator
		While Me.rabbit.state <> DicePalaceRabbitLevelRabbit.State.Idle
			Yield Nothing
		End While
		Me.rabbit.OnMagicWand()
		While Me.rabbit.state <> DicePalaceRabbitLevelRabbit.State.Idle
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060003E9 RID: 1001 RVA: 0x0006111C File Offset: 0x0005F51C
	Private Iterator Function magicparry_cr() As IEnumerator
		While Me.rabbit.state <> DicePalaceRabbitLevelRabbit.State.Idle
			Yield Nothing
		End While
		Me.rabbit.OnMagicParry()
		While Me.rabbit.state <> DicePalaceRabbitLevelRabbit.State.Idle
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0400068C RID: 1676
	Private properties As LevelProperties.DicePalaceRabbit

	' Token: 0x0400068D RID: 1677
	<SerializeField()>
	Private rabbit As DicePalaceRabbitLevelRabbit

	' Token: 0x0400068E RID: 1678
	<Header("Boss Info")>
	<SerializeField()>
	Private _bossPortrait As Sprite

	' Token: 0x0400068F RID: 1679
	<SerializeField()>
	Private _bossQuote As String
End Class
