Imports System
Imports UnityEngine

' Token: 0x020002E2 RID: 738
Public Class TutorialLevel
	Inherits Level

	' Token: 0x06000835 RID: 2101 RVA: 0x000789EC File Offset: 0x00076DEC
	Protected Overrides Sub PartialInit()
		Me.properties = LevelProperties.Tutorial.GetMode(MyBase.mode)
		AddHandler Me.properties.OnStateChange, AddressOf MyBase.zHack_OnStateChanged
		AddHandler Me.properties.OnBossDeath, AddressOf MyBase.zHack_OnWin
		MyBase.timeline = Me.properties.CreateTimeline(MyBase.mode)
		Me.goalTimes = Me.properties.goalTimes
		AddHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		MyBase.PartialInit()
	End Sub

	' Token: 0x17000156 RID: 342
	' (get) Token: 0x06000836 RID: 2102 RVA: 0x00078A82 File Offset: 0x00076E82
	Public Overrides ReadOnly Property CurrentLevel As Levels
		Get
			Return Levels.Tutorial
		End Get
	End Property

	' Token: 0x17000157 RID: 343
	' (get) Token: 0x06000837 RID: 2103 RVA: 0x00078A85 File Offset: 0x00076E85
	Public Overrides ReadOnly Property CurrentScene As Scenes
		Get
			Return Scenes.scene_level_tutorial
		End Get
	End Property

	' Token: 0x17000158 RID: 344
	' (get) Token: 0x06000838 RID: 2104 RVA: 0x00078A88 File Offset: 0x00076E88
	Public Overrides ReadOnly Property BossPortrait As Sprite
		Get
			Return Me._bossPortrait
		End Get
	End Property

	' Token: 0x17000159 RID: 345
	' (get) Token: 0x06000839 RID: 2105 RVA: 0x00078A90 File Offset: 0x00076E90
	Public Overrides ReadOnly Property BossQuote As String
		Get
			Return Me._bossQuote
		End Get
	End Property

	' Token: 0x0600083A RID: 2106 RVA: 0x00078A98 File Offset: 0x00076E98
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.background.SetParent(Global.UnityEngine.Camera.main.transform)
		Me.background.ResetLocalTransforms()
	End Sub

	' Token: 0x0600083B RID: 2107 RVA: 0x00078AC0 File Offset: 0x00076EC0
	Protected Overrides Sub OnLevelStart()
	End Sub

	' Token: 0x0600083C RID: 2108 RVA: 0x00078AC4 File Offset: 0x00076EC4
	Public Sub GoBackToHouse()
		Dim abstractPlayerController As AbstractPlayerController = PlayerManager.GetPlayer(PlayerId.PlayerOne)
		Me.playerGoBackToHouseEffects(0).gameObject.SetActive(True)
		Me.playerGoBackToHouseEffects(0).transform.position = abstractPlayerController.transform.position
		abstractPlayerController.gameObject.SetActive(False)
		Me.playerGoBackToHouseEffects(0).animator.SetTrigger("OnStartTutorial")
		abstractPlayerController = PlayerManager.GetPlayer(PlayerId.PlayerTwo)
		If abstractPlayerController IsNot Nothing Then
			Me.playerGoBackToHouseEffects(1).gameObject.SetActive(True)
			Me.playerGoBackToHouseEffects(1).transform.position = abstractPlayerController.transform.position
			abstractPlayerController.gameObject.SetActive(False)
			Me.playerGoBackToHouseEffects(1).animator.SetTrigger("OnStartTutorial")
		End If
	End Sub

	' Token: 0x0600083D RID: 2109 RVA: 0x00078B94 File Offset: 0x00076F94
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		For i As Integer = 0 To Me.playerGoBackToHouseEffects.Length - 1
			Me.playerGoBackToHouseEffects(i).Clean()
		Next
		Me.playerGoBackToHouseEffects = Nothing
	End Sub

	' Token: 0x040010A2 RID: 4258
	Private properties As LevelProperties.Tutorial

	' Token: 0x040010A3 RID: 4259
	<SerializeField()>
	Private background As Transform

	' Token: 0x040010A4 RID: 4260
	<Header("Boss Info")>
	<SerializeField()>
	Private _bossPortrait As Sprite

	' Token: 0x040010A5 RID: 4261
	<SerializeField()>
	<Multiline()>
	Private _bossQuote As String

	' Token: 0x040010A6 RID: 4262
	<SerializeField()>
	Private playerGoBackToHouseEffects As PlayerDeathEffect()

	' Token: 0x02000831 RID: 2097
	<Serializable()>
	Public Class Prefabs
	End Class
End Class
