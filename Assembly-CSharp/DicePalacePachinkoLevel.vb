Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200014C RID: 332
Public Class DicePalacePachinkoLevel
	Inherits AbstractDicePalaceLevel

	' Token: 0x060003C4 RID: 964 RVA: 0x00060A84 File Offset: 0x0005EE84
	Protected Overrides Sub PartialInit()
		Me.properties = LevelProperties.DicePalacePachinko.GetMode(MyBase.mode)
		AddHandler Me.properties.OnStateChange, AddressOf MyBase.zHack_OnStateChanged
		AddHandler Me.properties.OnBossDeath, AddressOf MyBase.zHack_OnWin
		MyBase.timeline = Me.properties.CreateTimeline(MyBase.mode)
		Me.goalTimes = Me.properties.goalTimes
		AddHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		MyBase.PartialInit()
	End Sub

	' Token: 0x170000AF RID: 175
	' (get) Token: 0x060003C5 RID: 965 RVA: 0x00060B1A File Offset: 0x0005EF1A
	Public Overrides ReadOnly Property CurrentDicePalaceLevel As DicePalaceLevels
		Get
			Return DicePalaceLevels.DicePalacePachinko
		End Get
	End Property

	' Token: 0x170000B0 RID: 176
	' (get) Token: 0x060003C6 RID: 966 RVA: 0x00060B21 File Offset: 0x0005EF21
	Public Overrides ReadOnly Property CurrentLevel As Levels
		Get
			Return Levels.DicePalacePachinko
		End Get
	End Property

	' Token: 0x170000B1 RID: 177
	' (get) Token: 0x060003C7 RID: 967 RVA: 0x00060B28 File Offset: 0x0005EF28
	Public Overrides ReadOnly Property CurrentScene As Scenes
		Get
			Return Scenes.scene_level_dice_palace_pachinko
		End Get
	End Property

	' Token: 0x170000B2 RID: 178
	' (get) Token: 0x060003C8 RID: 968 RVA: 0x00060B2C File Offset: 0x0005EF2C
	Public Overrides ReadOnly Property BossPortrait As Sprite
		Get
			Return Me._bossPortrait
		End Get
	End Property

	' Token: 0x170000B3 RID: 179
	' (get) Token: 0x060003C9 RID: 969 RVA: 0x00060B34 File Offset: 0x0005EF34
	Public Overrides ReadOnly Property BossQuote As String
		Get
			Return Me._bossQuote
		End Get
	End Property

	' Token: 0x060003CA RID: 970 RVA: 0x00060B3C File Offset: 0x0005EF3C
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.pipes.LevelInit(Me.properties)
		Me.pachinko.LevelInit(Me.properties)
		For Each transform As Transform In Me.starDiscs
			MyBase.StartCoroutine(Me.star_disc_cr(transform))
		Next
	End Sub

	' Token: 0x060003CB RID: 971 RVA: 0x00060B9E File Offset: 0x0005EF9E
	Protected Overrides Sub OnLevelStart()
		MyBase.StartCoroutine(Me.dicepalacepachinkoPattern_cr())
	End Sub

	' Token: 0x060003CC RID: 972 RVA: 0x00060BB0 File Offset: 0x0005EFB0
	Private Iterator Function star_disc_cr(disc As Transform) As IEnumerator
		Dim fadingOut As Boolean = Rand.Bool()
		While True
			Dim fadeTime As Single = Global.UnityEngine.Random.Range(0.1F, 0.3F)
			Dim holdTime As Single = Global.UnityEngine.Random.Range(0.1F, 0.3F)
			Yield CupheadTime.WaitForSeconds(Me, holdTime)
			If fadingOut Then
				Dim t As Single = 0F
				While t < fadeTime
					disc.GetComponent(Of SpriteRenderer)().color = New Color(1F, 1F, 1F, 1F - t / fadeTime)
					t += CupheadTime.Delta
					Yield Nothing
				End While
			Else
				Dim t2 As Single = 0F
				While t2 < fadeTime
					disc.GetComponent(Of SpriteRenderer)().color = New Color(1F, 1F, 1F, t2 / fadeTime)
					t2 += CupheadTime.Delta
					Yield Nothing
				End While
			End If
			fadingOut = Not fadingOut
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060003CD RID: 973 RVA: 0x00060BD4 File Offset: 0x0005EFD4
	Private Iterator Function dicepalacepachinkoPattern_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		While True
			Yield MyBase.StartCoroutine(Me.nextPattern_cr())
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060003CE RID: 974 RVA: 0x00060BF0 File Offset: 0x0005EFF0
	Private Iterator Function nextPattern_cr() As IEnumerator
		Dim p As LevelProperties.DicePalacePachinko.Pattern = Me.properties.CurrentState.NextPattern
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		Return
	End Function

	' Token: 0x0400066A RID: 1642
	Private properties As LevelProperties.DicePalacePachinko

	' Token: 0x0400066B RID: 1643
	<SerializeField()>
	Private starDiscs As Transform()

	' Token: 0x0400066C RID: 1644
	<SerializeField()>
	Private pipes As DicePalacePachinkoLevelPipes

	' Token: 0x0400066D RID: 1645
	<SerializeField()>
	Private pachinko As DicePalacePachinkoLevelPachinko

	' Token: 0x0400066E RID: 1646
	<Header("Boss Info")>
	<SerializeField()>
	Private _bossPortrait As Sprite

	' Token: 0x0400066F RID: 1647
	<SerializeField()>
	Private _bossQuote As String
End Class
