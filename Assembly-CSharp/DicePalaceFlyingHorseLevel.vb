Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000129 RID: 297
Public Class DicePalaceFlyingHorseLevel
	Inherits AbstractDicePalaceLevel

	' Token: 0x06000360 RID: 864 RVA: 0x0005FE5C File Offset: 0x0005E25C
	Protected Overrides Sub PartialInit()
		Me.properties = LevelProperties.DicePalaceFlyingHorse.GetMode(MyBase.mode)
		AddHandler Me.properties.OnStateChange, AddressOf MyBase.zHack_OnStateChanged
		AddHandler Me.properties.OnBossDeath, AddressOf MyBase.zHack_OnWin
		MyBase.timeline = Me.properties.CreateTimeline(MyBase.mode)
		Me.goalTimes = Me.properties.goalTimes
		AddHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		MyBase.PartialInit()
	End Sub

	' Token: 0x17000096 RID: 150
	' (get) Token: 0x06000361 RID: 865 RVA: 0x0005FEF2 File Offset: 0x0005E2F2
	Public Overrides ReadOnly Property CurrentDicePalaceLevel As DicePalaceLevels
		Get
			Return DicePalaceLevels.DicePalaceFlyingHorse
		End Get
	End Property

	' Token: 0x17000097 RID: 151
	' (get) Token: 0x06000362 RID: 866 RVA: 0x0005FEF9 File Offset: 0x0005E2F9
	Public Overrides ReadOnly Property CurrentLevel As Levels
		Get
			Return Levels.DicePalaceFlyingHorse
		End Get
	End Property

	' Token: 0x17000098 RID: 152
	' (get) Token: 0x06000363 RID: 867 RVA: 0x0005FF00 File Offset: 0x0005E300
	Public Overrides ReadOnly Property CurrentScene As Scenes
		Get
			Return Scenes.scene_level_dice_palace_flying_horse
		End Get
	End Property

	' Token: 0x17000099 RID: 153
	' (get) Token: 0x06000364 RID: 868 RVA: 0x0005FF04 File Offset: 0x0005E304
	Public Overrides ReadOnly Property BossPortrait As Sprite
		Get
			Return Me._bossPortrait
		End Get
	End Property

	' Token: 0x1700009A RID: 154
	' (get) Token: 0x06000365 RID: 869 RVA: 0x0005FF0C File Offset: 0x0005E30C
	Public Overrides ReadOnly Property BossQuote As String
		Get
			Return Me._bossQuote
		End Get
	End Property

	' Token: 0x06000366 RID: 870 RVA: 0x0005FF14 File Offset: 0x0005E314
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.horse.LevelInit(Me.properties)
	End Sub

	' Token: 0x06000367 RID: 871 RVA: 0x0005FF2D File Offset: 0x0005E32D
	Protected Overrides Sub OnLevelStart()
		MyBase.StartCoroutine(Me.dicepalaceflyinghorsePattern_cr())
	End Sub

	' Token: 0x06000368 RID: 872 RVA: 0x0005FF3C File Offset: 0x0005E33C
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me._bossPortrait = Nothing
	End Sub

	' Token: 0x06000369 RID: 873 RVA: 0x0005FF4C File Offset: 0x0005E34C
	Private Iterator Function dicepalaceflyinghorsePattern_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		While True
			Yield MyBase.StartCoroutine(Me.nextPattern_cr())
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600036A RID: 874 RVA: 0x0005FF68 File Offset: 0x0005E368
	Private Iterator Function nextPattern_cr() As IEnumerator
		Dim p As LevelProperties.DicePalaceFlyingHorse.Pattern = Me.properties.CurrentState.NextPattern
		If p <> LevelProperties.DicePalaceFlyingHorse.Pattern.[Default] Then
			Yield CupheadTime.WaitForSeconds(Me, 1F)
		Else
			Yield Nothing
		End If
		Return
	End Function

	' Token: 0x040005E9 RID: 1513
	Private properties As LevelProperties.DicePalaceFlyingHorse

	' Token: 0x040005EA RID: 1514
	<SerializeField()>
	Private horse As DicePalaceFlyingHorseLevelHorse

	' Token: 0x040005EB RID: 1515
	<Header("Boss Info")>
	<SerializeField()>
	Private _bossPortrait As Sprite

	' Token: 0x040005EC RID: 1516
	<SerializeField()>
	Private _bossQuote As String
End Class
