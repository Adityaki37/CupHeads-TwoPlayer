Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000110 RID: 272
Public Class DicePalaceCigarLevel
	Inherits AbstractDicePalaceLevel

	' Token: 0x06000315 RID: 789 RVA: 0x0005F2F0 File Offset: 0x0005D6F0
	Protected Overrides Sub PartialInit()
		Me.properties = LevelProperties.DicePalaceCigar.GetMode(MyBase.mode)
		AddHandler Me.properties.OnStateChange, AddressOf MyBase.zHack_OnStateChanged
		AddHandler Me.properties.OnBossDeath, AddressOf MyBase.zHack_OnWin
		MyBase.timeline = Me.properties.CreateTimeline(MyBase.mode)
		Me.goalTimes = Me.properties.goalTimes
		AddHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		MyBase.PartialInit()
	End Sub

	' Token: 0x17000084 RID: 132
	' (get) Token: 0x06000316 RID: 790 RVA: 0x0005F386 File Offset: 0x0005D786
	Public Overrides ReadOnly Property CurrentDicePalaceLevel As DicePalaceLevels
		Get
			Return DicePalaceLevels.DicePalaceCigar
		End Get
	End Property

	' Token: 0x17000085 RID: 133
	' (get) Token: 0x06000317 RID: 791 RVA: 0x0005F38D File Offset: 0x0005D78D
	Public Overrides ReadOnly Property CurrentLevel As Levels
		Get
			Return Levels.DicePalaceCigar
		End Get
	End Property

	' Token: 0x17000086 RID: 134
	' (get) Token: 0x06000318 RID: 792 RVA: 0x0005F394 File Offset: 0x0005D794
	Public Overrides ReadOnly Property CurrentScene As Scenes
		Get
			Return Scenes.scene_level_dice_palace_cigar
		End Get
	End Property

	' Token: 0x17000087 RID: 135
	' (get) Token: 0x06000319 RID: 793 RVA: 0x0005F398 File Offset: 0x0005D798
	Public Overrides ReadOnly Property BossPortrait As Sprite
		Get
			Return Me._bossPortrait
		End Get
	End Property

	' Token: 0x17000088 RID: 136
	' (get) Token: 0x0600031A RID: 794 RVA: 0x0005F3A0 File Offset: 0x0005D7A0
	Public Overrides ReadOnly Property BossQuote As String
		Get
			Return Me._bossQuote
		End Get
	End Property

	' Token: 0x0600031B RID: 795 RVA: 0x0005F3A8 File Offset: 0x0005D7A8
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.cigar.LevelInit(Me.properties)
	End Sub

	' Token: 0x0600031C RID: 796 RVA: 0x0005F3C1 File Offset: 0x0005D7C1
	Protected Overrides Sub OnLevelStart()
		MyBase.StartCoroutine(Me.dicepalacecigarPattern_cr())
	End Sub

	' Token: 0x0600031D RID: 797 RVA: 0x0005F3D0 File Offset: 0x0005D7D0
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me._bossPortrait = Nothing
	End Sub

	' Token: 0x0600031E RID: 798 RVA: 0x0005F3E0 File Offset: 0x0005D7E0
	Private Iterator Function dicepalacecigarPattern_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		While True
			Yield MyBase.StartCoroutine(Me.nextPattern_cr())
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600031F RID: 799 RVA: 0x0005F3FC File Offset: 0x0005D7FC
	Private Iterator Function nextPattern_cr() As IEnumerator
		Dim p As LevelProperties.DicePalaceCigar.Pattern = Me.properties.CurrentState.NextPattern
		If p <> LevelProperties.DicePalaceCigar.Pattern.[Default] Then
			Yield CupheadTime.WaitForSeconds(Me, 1F)
		Else
			Yield Nothing
		End If
		Return
	End Function

	' Token: 0x0400057F RID: 1407
	Private properties As LevelProperties.DicePalaceCigar

	' Token: 0x04000580 RID: 1408
	<SerializeField()>
	Private cigar As DicePalaceCigarLevelCigar

	' Token: 0x04000581 RID: 1409
	<SerializeField()>
	Private fire As Animator

	' Token: 0x04000582 RID: 1410
	<Header("Boss Info")>
	<SerializeField()>
	Private _bossPortrait As Sprite

	' Token: 0x04000583 RID: 1411
	<SerializeField()>
	Private _bossQuote As String
End Class
