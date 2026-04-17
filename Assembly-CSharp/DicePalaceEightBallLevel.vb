Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000121 RID: 289
Public Class DicePalaceEightBallLevel
	Inherits AbstractDicePalaceLevel

	' Token: 0x06000348 RID: 840 RVA: 0x0005FB58 File Offset: 0x0005DF58
	Protected Overrides Sub PartialInit()
		Me.properties = LevelProperties.DicePalaceEightBall.GetMode(MyBase.mode)
		AddHandler Me.properties.OnStateChange, AddressOf MyBase.zHack_OnStateChanged
		AddHandler Me.properties.OnBossDeath, AddressOf MyBase.zHack_OnWin
		MyBase.timeline = Me.properties.CreateTimeline(MyBase.mode)
		Me.goalTimes = Me.properties.goalTimes
		AddHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		MyBase.PartialInit()
	End Sub

	' Token: 0x17000090 RID: 144
	' (get) Token: 0x06000349 RID: 841 RVA: 0x0005FBEE File Offset: 0x0005DFEE
	Public Overrides ReadOnly Property CurrentDicePalaceLevel As DicePalaceLevels
		Get
			Return DicePalaceLevels.DicePalaceEightBall
		End Get
	End Property

	' Token: 0x17000091 RID: 145
	' (get) Token: 0x0600034A RID: 842 RVA: 0x0005FBF5 File Offset: 0x0005DFF5
	Public Overrides ReadOnly Property CurrentLevel As Levels
		Get
			Return Levels.DicePalaceEightBall
		End Get
	End Property

	' Token: 0x17000092 RID: 146
	' (get) Token: 0x0600034B RID: 843 RVA: 0x0005FBFC File Offset: 0x0005DFFC
	Public Overrides ReadOnly Property CurrentScene As Scenes
		Get
			Return Scenes.scene_level_dice_palace_eight_ball
		End Get
	End Property

	' Token: 0x17000093 RID: 147
	' (get) Token: 0x0600034C RID: 844 RVA: 0x0005FC00 File Offset: 0x0005E000
	Public Overrides ReadOnly Property BossPortrait As Sprite
		Get
			Return Me._bossPortrait
		End Get
	End Property

	' Token: 0x17000094 RID: 148
	' (get) Token: 0x0600034D RID: 845 RVA: 0x0005FC08 File Offset: 0x0005E008
	Public Overrides ReadOnly Property BossQuote As String
		Get
			Return Me._bossQuote
		End Get
	End Property

	' Token: 0x0600034E RID: 846 RVA: 0x0005FC10 File Offset: 0x0005E010
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.eightBall.LevelInit(Me.properties)
	End Sub

	' Token: 0x0600034F RID: 847 RVA: 0x0005FC29 File Offset: 0x0005E029
	Protected Overrides Sub OnLevelStart()
		MyBase.StartCoroutine(Me.dicepalaceeightballPattern_cr())
	End Sub

	' Token: 0x06000350 RID: 848 RVA: 0x0005FC38 File Offset: 0x0005E038
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me._bossPortrait = Nothing
	End Sub

	' Token: 0x06000351 RID: 849 RVA: 0x0005FC48 File Offset: 0x0005E048
	Private Iterator Function dicepalaceeightballPattern_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		While True
			Yield MyBase.StartCoroutine(Me.nextPattern_cr())
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000352 RID: 850 RVA: 0x0005FC64 File Offset: 0x0005E064
	Private Iterator Function nextPattern_cr() As IEnumerator
		Dim p As LevelProperties.DicePalaceEightBall.Pattern = Me.properties.CurrentState.NextPattern
		If p <> LevelProperties.DicePalaceEightBall.Pattern.[Default] Then
			Yield CupheadTime.WaitForSeconds(Me, 1F)
		Else
			Yield Nothing
		End If
		Return
	End Function

	' Token: 0x040005CB RID: 1483
	Private properties As LevelProperties.DicePalaceEightBall

	' Token: 0x040005CC RID: 1484
	<SerializeField()>
	Private eightBall As DicePalaceEightBallLevelEightBall

	' Token: 0x040005CD RID: 1485
	<Header("Boss Info")>
	<SerializeField()>
	Private _bossPortrait As Sprite

	' Token: 0x040005CE RID: 1486
	<SerializeField()>
	Private _bossQuote As String
End Class
