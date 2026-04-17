Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000132 RID: 306
Public Class DicePalaceFlyingMemoryLevel
	Inherits AbstractDicePalaceLevel

	' Token: 0x06000379 RID: 889 RVA: 0x00060160 File Offset: 0x0005E560
	Protected Overrides Sub PartialInit()
		Me.properties = LevelProperties.DicePalaceFlyingMemory.GetMode(MyBase.mode)
		AddHandler Me.properties.OnStateChange, AddressOf MyBase.zHack_OnStateChanged
		AddHandler Me.properties.OnBossDeath, AddressOf MyBase.zHack_OnWin
		MyBase.timeline = Me.properties.CreateTimeline(MyBase.mode)
		Me.goalTimes = Me.properties.goalTimes
		AddHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		MyBase.PartialInit()
	End Sub

	' Token: 0x1700009C RID: 156
	' (get) Token: 0x0600037A RID: 890 RVA: 0x000601F6 File Offset: 0x0005E5F6
	Public Overrides ReadOnly Property CurrentDicePalaceLevel As DicePalaceLevels
		Get
			Return DicePalaceLevels.DicePalaceFlyingMemory
		End Get
	End Property

	' Token: 0x1700009D RID: 157
	' (get) Token: 0x0600037B RID: 891 RVA: 0x000601FD File Offset: 0x0005E5FD
	Public Overrides ReadOnly Property CurrentLevel As Levels
		Get
			Return Levels.DicePalaceFlyingMemory
		End Get
	End Property

	' Token: 0x1700009E RID: 158
	' (get) Token: 0x0600037C RID: 892 RVA: 0x00060204 File Offset: 0x0005E604
	Public Overrides ReadOnly Property CurrentScene As Scenes
		Get
			Return Scenes.scene_level_dice_palace_flying_memory
		End Get
	End Property

	' Token: 0x1700009F RID: 159
	' (get) Token: 0x0600037D RID: 893 RVA: 0x00060208 File Offset: 0x0005E608
	Public Overrides ReadOnly Property BossPortrait As Sprite
		Get
			Return Me._bossPortrait
		End Get
	End Property

	' Token: 0x170000A0 RID: 160
	' (get) Token: 0x0600037E RID: 894 RVA: 0x00060210 File Offset: 0x0005E610
	Public Overrides ReadOnly Property BossQuote As String
		Get
			Return Me._bossQuote
		End Get
	End Property

	' Token: 0x0600037F RID: 895 RVA: 0x00060218 File Offset: 0x0005E618
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.gameManager.LevelInit(Me.properties)
		Me.stuffedToy.LevelInit(Me.properties)
	End Sub

	' Token: 0x06000380 RID: 896 RVA: 0x00060242 File Offset: 0x0005E642
	Protected Overrides Sub OnLevelStart()
		MyBase.StartCoroutine(Me.dicepalaceflyingmemoryPattern_cr())
	End Sub

	' Token: 0x06000381 RID: 897 RVA: 0x00060251 File Offset: 0x0005E651
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me._bossPortrait = Nothing
	End Sub

	' Token: 0x06000382 RID: 898 RVA: 0x00060260 File Offset: 0x0005E660
	Private Iterator Function dicepalaceflyingmemoryPattern_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		While True
			Yield MyBase.StartCoroutine(Me.nextPattern_cr())
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000383 RID: 899 RVA: 0x0006027C File Offset: 0x0005E67C
	Private Iterator Function nextPattern_cr() As IEnumerator
		Dim p As LevelProperties.DicePalaceFlyingMemory.Pattern = Me.properties.CurrentState.NextPattern
		If p <> LevelProperties.DicePalaceFlyingMemory.Pattern.[Default] Then
			Yield CupheadTime.WaitForSeconds(Me, 1F)
		Else
			Yield Nothing
		End If
		Return
	End Function

	' Token: 0x04000616 RID: 1558
	Private properties As LevelProperties.DicePalaceFlyingMemory

	' Token: 0x04000617 RID: 1559
	<SerializeField()>
	Private stuffedToy As DicePalaceFlyingMemoryLevelStuffedToy

	' Token: 0x04000618 RID: 1560
	<SerializeField()>
	Private gameManager As DicePalaceFlyingMemoryLevelGameManager

	' Token: 0x04000619 RID: 1561
	<Header("Boss Info")>
	<SerializeField()>
	Private _bossPortrait As Sprite

	' Token: 0x0400061A RID: 1562
	<SerializeField()>
	Private _bossQuote As String
End Class
