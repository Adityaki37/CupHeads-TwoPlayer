Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200008B RID: 139
Public Class ChessBOldBLevel
	Inherits Level

	' Token: 0x0600017E RID: 382 RVA: 0x00058258 File Offset: 0x00056658
	Protected Overrides Sub PartialInit()
		Me.properties = LevelProperties.ChessBOldB.GetMode(MyBase.mode)
		AddHandler Me.properties.OnStateChange, AddressOf MyBase.zHack_OnStateChanged
		AddHandler Me.properties.OnBossDeath, AddressOf MyBase.zHack_OnWin
		MyBase.timeline = Me.properties.CreateTimeline(MyBase.mode)
		Me.goalTimes = Me.properties.goalTimes
		AddHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		MyBase.PartialInit()
	End Sub

	' Token: 0x1700003E RID: 62
	' (get) Token: 0x0600017F RID: 383 RVA: 0x000582EE File Offset: 0x000566EE
	Public Overrides ReadOnly Property CurrentLevel As Levels
		Get
			Return Levels.ChessBOldB
		End Get
	End Property

	' Token: 0x1700003F RID: 63
	' (get) Token: 0x06000180 RID: 384 RVA: 0x000582F5 File Offset: 0x000566F5
	Public Overrides ReadOnly Property CurrentScene As Scenes
		Get
			Return Scenes.scene_level_chess_boldb
		End Get
	End Property

	' Token: 0x17000040 RID: 64
	' (get) Token: 0x06000181 RID: 385 RVA: 0x000582F9 File Offset: 0x000566F9
	Public Overrides ReadOnly Property BossPortrait As Sprite
		Get
			Return Me._bossPortrait
		End Get
	End Property

	' Token: 0x17000041 RID: 65
	' (get) Token: 0x06000182 RID: 386 RVA: 0x00058301 File Offset: 0x00056701
	Public Overrides ReadOnly Property BossQuote As String
		Get
			Return Me._bossQuote
		End Get
	End Property

	' Token: 0x06000183 RID: 387 RVA: 0x00058309 File Offset: 0x00056709
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.boss.LevelInit(Me.properties)
		Me.gameManager.SetupGameManager(Me.properties)
	End Sub

	' Token: 0x06000184 RID: 388 RVA: 0x00058333 File Offset: 0x00056733
	Protected Overrides Sub OnStateChanged()
		MyBase.OnStateChanged()
		Me.gameManager.OnStateChanged()
		Me.boss.OnStateChanged()
	End Sub

	' Token: 0x06000185 RID: 389 RVA: 0x00058351 File Offset: 0x00056751
	Protected Overrides Sub OnLevelStart()
	End Sub

	' Token: 0x06000186 RID: 390 RVA: 0x00058354 File Offset: 0x00056754
	Private Iterator Function ChessBOldBPattern_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		While True
			Yield MyBase.StartCoroutine(Me.nextPattern_cr())
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000187 RID: 391 RVA: 0x00058370 File Offset: 0x00056770
	Private Iterator Function nextPattern_cr() As IEnumerator
		Dim p As LevelProperties.ChessBOldB.Pattern = Me.properties.CurrentState.NextPattern
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		Return
	End Function

	' Token: 0x040002FE RID: 766
	Private properties As LevelProperties.ChessBOldB

	' Token: 0x040002FF RID: 767
	<SerializeField()>
	Private boss As ChessBOldBLevelBoss

	' Token: 0x04000300 RID: 768
	<SerializeField()>
	Private gameManager As ChessBOldBLevelGameManager

	' Token: 0x04000301 RID: 769
	<Header("Boss Info")>
	<SerializeField()>
	Private _bossPortrait As Sprite

	' Token: 0x04000302 RID: 770
	<SerializeField()>
	<Multiline()>
	Private _bossQuote As String
End Class
