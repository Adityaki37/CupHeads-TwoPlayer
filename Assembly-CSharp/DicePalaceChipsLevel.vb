Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000107 RID: 263
Public Class DicePalaceChipsLevel
	Inherits AbstractDicePalaceLevel

	' Token: 0x060002FB RID: 763 RVA: 0x0005EEAC File Offset: 0x0005D2AC
	Protected Overrides Sub PartialInit()
		Me.properties = LevelProperties.DicePalaceChips.GetMode(MyBase.mode)
		AddHandler Me.properties.OnStateChange, AddressOf MyBase.zHack_OnStateChanged
		AddHandler Me.properties.OnBossDeath, AddressOf MyBase.zHack_OnWin
		MyBase.timeline = Me.properties.CreateTimeline(MyBase.mode)
		Me.goalTimes = Me.properties.goalTimes
		AddHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		MyBase.PartialInit()
	End Sub

	' Token: 0x1700007E RID: 126
	' (get) Token: 0x060002FC RID: 764 RVA: 0x0005EF42 File Offset: 0x0005D342
	Public Overrides ReadOnly Property CurrentDicePalaceLevel As DicePalaceLevels
		Get
			Return DicePalaceLevels.DicePalaceChips
		End Get
	End Property

	' Token: 0x1700007F RID: 127
	' (get) Token: 0x060002FD RID: 765 RVA: 0x0005EF49 File Offset: 0x0005D349
	Public Overrides ReadOnly Property CurrentLevel As Levels
		Get
			Return Levels.DicePalaceChips
		End Get
	End Property

	' Token: 0x17000080 RID: 128
	' (get) Token: 0x060002FE RID: 766 RVA: 0x0005EF50 File Offset: 0x0005D350
	Public Overrides ReadOnly Property CurrentScene As Scenes
		Get
			Return Scenes.scene_level_dice_palace_chips
		End Get
	End Property

	' Token: 0x17000081 RID: 129
	' (get) Token: 0x060002FF RID: 767 RVA: 0x0005EF54 File Offset: 0x0005D354
	Public Overrides ReadOnly Property BossPortrait As Sprite
		Get
			Return Me._bossPortrait
		End Get
	End Property

	' Token: 0x17000082 RID: 130
	' (get) Token: 0x06000300 RID: 768 RVA: 0x0005EF5C File Offset: 0x0005D35C
	Public Overrides ReadOnly Property BossQuote As String
		Get
			Return Me._bossQuote
		End Get
	End Property

	' Token: 0x06000301 RID: 769 RVA: 0x0005EF64 File Offset: 0x0005D364
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.chips.LevelInit(Me.properties)
		MyBase.StartCoroutine(CupheadLevelCamera.Current.rotate_camera())
		MyBase.StartCoroutine(Me.rotate_background_cr())
	End Sub

	' Token: 0x06000302 RID: 770 RVA: 0x0005EF9B File Offset: 0x0005D39B
	Protected Overrides Sub OnLevelStart()
		MyBase.StartCoroutine(Me.dicepalacechipsPattern_cr())
	End Sub

	' Token: 0x06000303 RID: 771 RVA: 0x0005EFAA File Offset: 0x0005D3AA
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me._bossPortrait = Nothing
	End Sub

	' Token: 0x06000304 RID: 772 RVA: 0x0005EFBC File Offset: 0x0005D3BC
	Private Iterator Function dicepalacechipsPattern_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		While True
			Yield MyBase.StartCoroutine(Me.nextPattern_cr())
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000305 RID: 773 RVA: 0x0005EFD8 File Offset: 0x0005D3D8
	Private Iterator Function nextPattern_cr() As IEnumerator
		Dim p As LevelProperties.DicePalaceChips.Pattern = Me.properties.CurrentState.NextPattern
		If p <> LevelProperties.DicePalaceChips.Pattern.[Default] Then
			Yield CupheadTime.WaitForSeconds(Me, 1F)
		Else
			Yield Nothing
		End If
		Return
	End Function

	' Token: 0x06000306 RID: 774 RVA: 0x0005EFF4 File Offset: 0x0005D3F4
	Private Iterator Function rotate_background_cr() As IEnumerator
		Dim time As Single = 1.5F
		Dim t As Single = 0F
		While True
			t += CupheadTime.Delta
			Dim phase As Single = Mathf.Sin(t / time)
			Me.background.transform.localRotation = Quaternion.Euler(New Vector3(0F, 0F, phase * 1F))
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x04000561 RID: 1377
	Private properties As LevelProperties.DicePalaceChips

	' Token: 0x04000562 RID: 1378
	<SerializeField()>
	Private background As GameObject

	' Token: 0x04000563 RID: 1379
	<SerializeField()>
	Private chips As DicePalaceChipsLevelChips

	' Token: 0x04000564 RID: 1380
	<Header("Boss Info")>
	<SerializeField()>
	Private _bossPortrait As Sprite

	' Token: 0x04000565 RID: 1381
	<SerializeField()>
	Private _bossQuote As String
End Class
