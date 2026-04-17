Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020002A3 RID: 675
Public Class ShmupTutorialLevel
	Inherits Level

	' Token: 0x0600077C RID: 1916 RVA: 0x00075980 File Offset: 0x00073D80
	Protected Overrides Sub PartialInit()
		Me.properties = LevelProperties.ShmupTutorial.GetMode(MyBase.mode)
		AddHandler Me.properties.OnStateChange, AddressOf MyBase.zHack_OnStateChanged
		AddHandler Me.properties.OnBossDeath, AddressOf MyBase.zHack_OnWin
		MyBase.timeline = Me.properties.CreateTimeline(MyBase.mode)
		Me.goalTimes = Me.properties.goalTimes
		AddHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		MyBase.PartialInit()
	End Sub

	' Token: 0x17000136 RID: 310
	' (get) Token: 0x0600077D RID: 1917 RVA: 0x00075A16 File Offset: 0x00073E16
	Public Overrides ReadOnly Property CurrentLevel As Levels
		Get
			Return Levels.ShmupTutorial
		End Get
	End Property

	' Token: 0x17000137 RID: 311
	' (get) Token: 0x0600077E RID: 1918 RVA: 0x00075A1D File Offset: 0x00073E1D
	Public Overrides ReadOnly Property CurrentScene As Scenes
		Get
			Return Scenes.scene_level_shmup_tutorial
		End Get
	End Property

	' Token: 0x17000138 RID: 312
	' (get) Token: 0x0600077F RID: 1919 RVA: 0x00075A21 File Offset: 0x00073E21
	Public Overrides ReadOnly Property BossPortrait As Sprite
		Get
			Return Me._bossPortrait
		End Get
	End Property

	' Token: 0x17000139 RID: 313
	' (get) Token: 0x06000780 RID: 1920 RVA: 0x00075A29 File Offset: 0x00073E29
	Public Overrides ReadOnly Property BossQuote As String
		Get
			Return Me._bossQuote
		End Get
	End Property

	' Token: 0x06000781 RID: 1921 RVA: 0x00075A31 File Offset: 0x00073E31
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.canvasAnimator.SetTrigger("StartAnimation")
	End Sub

	' Token: 0x06000782 RID: 1922 RVA: 0x00075A49 File Offset: 0x00073E49
	Protected Overrides Sub OnLevelStart()
		MyBase.StartCoroutine(Me.shmuptutorialPattern_cr())
	End Sub

	' Token: 0x06000783 RID: 1923 RVA: 0x00075A58 File Offset: 0x00073E58
	Private Iterator Function shmuptutorialPattern_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		While True
			Yield MyBase.StartCoroutine(Me.nextPattern_cr())
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000784 RID: 1924 RVA: 0x00075A74 File Offset: 0x00073E74
	Private Iterator Function shmupTutorialStartAnimation_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, Me.waitForAnimationTime)
		Me.canvasAnimator.SetTrigger("StartAnimation")
		Return
	End Function

	' Token: 0x06000785 RID: 1925 RVA: 0x00075A90 File Offset: 0x00073E90
	Private Iterator Function nextPattern_cr() As IEnumerator
		Dim p As LevelProperties.ShmupTutorial.Pattern = Me.properties.CurrentState.NextPattern
		If p <> LevelProperties.ShmupTutorial.Pattern.[Default] Then
			Yield CupheadTime.WaitForSeconds(Me, 1F)
		Else
			Yield Nothing
		End If
		Return
	End Function

	' Token: 0x04000F2E RID: 3886
	Private properties As LevelProperties.ShmupTutorial

	' Token: 0x04000F2F RID: 3887
	<Header("Boss Info")>
	<SerializeField()>
	Private _bossPortrait As Sprite

	' Token: 0x04000F30 RID: 3888
	<SerializeField()>
	<Multiline()>
	Private _bossQuote As String

	' Token: 0x04000F31 RID: 3889
	Public canvasAnimator As Animator

	' Token: 0x04000F32 RID: 3890
	Public waitForAnimationTime As Single
End Class
