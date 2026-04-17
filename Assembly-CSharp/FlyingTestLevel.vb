Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020001E0 RID: 480
Public Class FlyingTestLevel
	Inherits Level

	' Token: 0x06000543 RID: 1347 RVA: 0x00067B50 File Offset: 0x00065F50
	Protected Overrides Sub PartialInit()
		Me.properties = LevelProperties.FlyingTest.GetMode(MyBase.mode)
		AddHandler Me.properties.OnStateChange, AddressOf MyBase.zHack_OnStateChanged
		AddHandler Me.properties.OnBossDeath, AddressOf MyBase.zHack_OnWin
		MyBase.timeline = Me.properties.CreateTimeline(MyBase.mode)
		Me.goalTimes = Me.properties.goalTimes
		AddHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		MyBase.PartialInit()
	End Sub

	' Token: 0x170000EC RID: 236
	' (get) Token: 0x06000544 RID: 1348 RVA: 0x00067BE6 File Offset: 0x00065FE6
	Public Overrides ReadOnly Property CurrentLevel As Levels
		Get
			Return Levels.FlyingTest
		End Get
	End Property

	' Token: 0x170000ED RID: 237
	' (get) Token: 0x06000545 RID: 1349 RVA: 0x00067BED File Offset: 0x00065FED
	Public Overrides ReadOnly Property CurrentScene As Scenes
		Get
			Return Scenes.scene_level_flying_test
		End Get
	End Property

	' Token: 0x170000EE RID: 238
	' (get) Token: 0x06000546 RID: 1350 RVA: 0x00067BF1 File Offset: 0x00065FF1
	Public Overrides ReadOnly Property BossPortrait As Sprite
		Get
			Return Me._bossPortrait
		End Get
	End Property

	' Token: 0x170000EF RID: 239
	' (get) Token: 0x06000547 RID: 1351 RVA: 0x00067BF9 File Offset: 0x00065FF9
	Public Overrides ReadOnly Property BossQuote As String
		Get
			Return Me._bossQuote
		End Get
	End Property

	' Token: 0x06000548 RID: 1352 RVA: 0x00067C01 File Offset: 0x00066001
	Protected Overrides Sub Start()
		MyBase.Start()
	End Sub

	' Token: 0x06000549 RID: 1353 RVA: 0x00067C09 File Offset: 0x00066009
	Protected Overrides Sub OnLevelStart()
		MyBase.StartCoroutine(Me.flyingtestPattern_cr())
	End Sub

	' Token: 0x0600054A RID: 1354 RVA: 0x00067C18 File Offset: 0x00066018
	Private Iterator Function flyingtestPattern_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		While True
			Yield MyBase.StartCoroutine(Me.nextPattern_cr())
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600054B RID: 1355 RVA: 0x00067C34 File Offset: 0x00066034
	Private Iterator Function nextPattern_cr() As IEnumerator
		Dim p As LevelProperties.FlyingTest.Pattern = Me.properties.CurrentState.NextPattern
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		Return
	End Function

	' Token: 0x040009F7 RID: 2551
	Private properties As LevelProperties.FlyingTest

	' Token: 0x040009F8 RID: 2552
	<Header("Boss Info")>
	<SerializeField()>
	Private _bossPortrait As Sprite

	' Token: 0x040009F9 RID: 2553
	<SerializeField()>
	<Multiline()>
	Private _bossQuote As String

	' Token: 0x020004AA RID: 1194
	<Serializable()>
	Public Class Prefabs
	End Class
End Class
