Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000119 RID: 281
Public Class DicePalaceDominoLevel
	Inherits AbstractDicePalaceLevel

	' Token: 0x0600032E RID: 814 RVA: 0x0005F5F4 File Offset: 0x0005D9F4
	Protected Overrides Sub PartialInit()
		Me.properties = LevelProperties.DicePalaceDomino.GetMode(MyBase.mode)
		AddHandler Me.properties.OnStateChange, AddressOf MyBase.zHack_OnStateChanged
		AddHandler Me.properties.OnBossDeath, AddressOf MyBase.zHack_OnWin
		MyBase.timeline = Me.properties.CreateTimeline(MyBase.mode)
		Me.goalTimes = Me.properties.goalTimes
		AddHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		MyBase.PartialInit()
	End Sub

	' Token: 0x1700008A RID: 138
	' (get) Token: 0x0600032F RID: 815 RVA: 0x0005F68A File Offset: 0x0005DA8A
	Public Overrides ReadOnly Property CurrentDicePalaceLevel As DicePalaceLevels
		Get
			Return DicePalaceLevels.DicePalaceDomino
		End Get
	End Property

	' Token: 0x1700008B RID: 139
	' (get) Token: 0x06000330 RID: 816 RVA: 0x0005F691 File Offset: 0x0005DA91
	Public Overrides ReadOnly Property CurrentLevel As Levels
		Get
			Return Levels.DicePalaceDomino
		End Get
	End Property

	' Token: 0x1700008C RID: 140
	' (get) Token: 0x06000331 RID: 817 RVA: 0x0005F698 File Offset: 0x0005DA98
	Public Overrides ReadOnly Property CurrentScene As Scenes
		Get
			Return Scenes.scene_level_dice_palace_domino
		End Get
	End Property

	' Token: 0x1700008D RID: 141
	' (get) Token: 0x06000332 RID: 818 RVA: 0x0005F69C File Offset: 0x0005DA9C
	Public Overrides ReadOnly Property BossPortrait As Sprite
		Get
			Return Me._bossPortrait
		End Get
	End Property

	' Token: 0x1700008E RID: 142
	' (get) Token: 0x06000333 RID: 819 RVA: 0x0005F6A4 File Offset: 0x0005DAA4
	Public Overrides ReadOnly Property BossQuote As String
		Get
			Return Me._bossQuote
		End Get
	End Property

	' Token: 0x06000334 RID: 820 RVA: 0x0005F6AC File Offset: 0x0005DAAC
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.domino.LevelInit(Me.properties)
	End Sub

	' Token: 0x06000335 RID: 821 RVA: 0x0005F6C5 File Offset: 0x0005DAC5
	Protected Overrides Sub OnLevelStart()
		MyBase.StartCoroutine(Me.dicepalacedominoPattern_cr())
	End Sub

	' Token: 0x06000336 RID: 822 RVA: 0x0005F6D4 File Offset: 0x0005DAD4
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me._bossPortrait = Nothing
	End Sub

	' Token: 0x06000337 RID: 823 RVA: 0x0005F6E4 File Offset: 0x0005DAE4
	Private Iterator Function dicepalacedominoPattern_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		While True
			Yield MyBase.StartCoroutine(Me.nextPattern_cr())
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000338 RID: 824 RVA: 0x0005F700 File Offset: 0x0005DB00
	Private Iterator Function nextPattern_cr() As IEnumerator
		Dim p As LevelProperties.DicePalaceDomino.Pattern = Me.properties.CurrentState.NextPattern
		If p <> LevelProperties.DicePalaceDomino.Pattern.Boomerang Then
			If p <> LevelProperties.DicePalaceDomino.Pattern.BouncyBall Then
				Yield CupheadTime.WaitForSeconds(Me, 1F)
			Else
				Yield MyBase.StartCoroutine(Me.bouncyball_cr())
			End If
		Else
			Yield MyBase.StartCoroutine(Me.boomerang_cr())
		End If
		Return
	End Function

	' Token: 0x06000339 RID: 825 RVA: 0x0005F71C File Offset: 0x0005DB1C
	Private Iterator Function boomerang_cr() As IEnumerator
		While Me.domino.state <> DicePalaceDominoLevelDomino.State.Idle
			Yield Nothing
		End While
		Me.domino.OnBoomerang()
		While Me.domino.state <> DicePalaceDominoLevelDomino.State.Idle
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600033A RID: 826 RVA: 0x0005F738 File Offset: 0x0005DB38
	Private Iterator Function bouncyball_cr() As IEnumerator
		While Me.domino.state <> DicePalaceDominoLevelDomino.State.Idle
			Yield Nothing
		End While
		Me.domino.OnBouncyBall()
		While Me.domino.state <> DicePalaceDominoLevelDomino.State.Idle
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x040005A3 RID: 1443
	Private properties As LevelProperties.DicePalaceDomino

	' Token: 0x040005A4 RID: 1444
	<SerializeField()>
	Private domino As DicePalaceDominoLevelDomino

	' Token: 0x040005A5 RID: 1445
	<Header("Boss Info")>
	<SerializeField()>
	Private _bossPortrait As Sprite

	' Token: 0x040005A6 RID: 1446
	<SerializeField()>
	Private _bossQuote As String
End Class
