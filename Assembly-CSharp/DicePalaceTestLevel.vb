Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000164 RID: 356
Public Class DicePalaceTestLevel
	Inherits AbstractDicePalaceLevel

	' Token: 0x06000410 RID: 1040 RVA: 0x00061AE0 File Offset: 0x0005FEE0
	Protected Overrides Sub PartialInit()
		Me.properties = LevelProperties.DicePalaceTest.GetMode(MyBase.mode)
		AddHandler Me.properties.OnStateChange, AddressOf MyBase.zHack_OnStateChanged
		AddHandler Me.properties.OnBossDeath, AddressOf MyBase.zHack_OnWin
		MyBase.timeline = Me.properties.CreateTimeline(MyBase.mode)
		Me.goalTimes = Me.properties.goalTimes
		AddHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		MyBase.PartialInit()
	End Sub

	' Token: 0x170000C1 RID: 193
	' (get) Token: 0x06000411 RID: 1041 RVA: 0x00061B76 File Offset: 0x0005FF76
	Public Overrides ReadOnly Property CurrentDicePalaceLevel As DicePalaceLevels
		Get
			Return DicePalaceLevels.DicePalaceTest
		End Get
	End Property

	' Token: 0x170000C2 RID: 194
	' (get) Token: 0x06000412 RID: 1042 RVA: 0x00061B7D File Offset: 0x0005FF7D
	Public Overrides ReadOnly Property CurrentLevel As Levels
		Get
			Return Levels.DicePalaceTest
		End Get
	End Property

	' Token: 0x170000C3 RID: 195
	' (get) Token: 0x06000413 RID: 1043 RVA: 0x00061B84 File Offset: 0x0005FF84
	Public Overrides ReadOnly Property CurrentScene As Scenes
		Get
			Return Scenes.scene_level_dice_palace_test
		End Get
	End Property

	' Token: 0x170000C4 RID: 196
	' (get) Token: 0x06000414 RID: 1044 RVA: 0x00061B88 File Offset: 0x0005FF88
	Public Overrides ReadOnly Property BossPortrait As Sprite
		Get
			Return Me._bossPortrait
		End Get
	End Property

	' Token: 0x170000C5 RID: 197
	' (get) Token: 0x06000415 RID: 1045 RVA: 0x00061B90 File Offset: 0x0005FF90
	Public Overrides ReadOnly Property BossQuote As String
		Get
			Return Me._bossQuote
		End Get
	End Property

	' Token: 0x06000416 RID: 1046 RVA: 0x00061B98 File Offset: 0x0005FF98
	Protected Overrides Sub Start()
		MyBase.Start()
	End Sub

	' Token: 0x06000417 RID: 1047 RVA: 0x00061BA0 File Offset: 0x0005FFA0
	Protected Overrides Sub OnLevelStart()
		MyBase.StartCoroutine(Me.dicepalacetestPattern_cr())
		MyBase.StartCoroutine(Me.test.start_it_cr())
	End Sub

	' Token: 0x06000418 RID: 1048 RVA: 0x00061BC4 File Offset: 0x0005FFC4
	Private Iterator Function dicepalacetestPattern_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		While True
			Yield MyBase.StartCoroutine(Me.nextPattern_cr())
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000419 RID: 1049 RVA: 0x00061BE0 File Offset: 0x0005FFE0
	Private Iterator Function nextPattern_cr() As IEnumerator
		Dim p As LevelProperties.DicePalaceTest.Pattern = Me.properties.CurrentState.NextPattern
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		Return
	End Function

	' Token: 0x040006B6 RID: 1718
	Private properties As LevelProperties.DicePalaceTest

	' Token: 0x040006B7 RID: 1719
	<SerializeField()>
	Private test As DicePalaceTestLevelTest

	' Token: 0x040006B8 RID: 1720
	<Header("Boss Info")>
	<SerializeField()>
	Private _bossPortrait As Sprite

	' Token: 0x040006B9 RID: 1721
	<SerializeField()>
	Private _bossQuote As String
End Class
