Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200006E RID: 110
Public Class ChaliceTutorialLevel
	Inherits Level

	' Token: 0x06000130 RID: 304 RVA: 0x00057334 File Offset: 0x00055734
	Protected Overrides Sub PartialInit()
		Me.properties = LevelProperties.ChaliceTutorial.GetMode(MyBase.mode)
		AddHandler Me.properties.OnStateChange, AddressOf MyBase.zHack_OnStateChanged
		AddHandler Me.properties.OnBossDeath, AddressOf MyBase.zHack_OnWin
		MyBase.timeline = Me.properties.CreateTimeline(MyBase.mode)
		Me.goalTimes = Me.properties.goalTimes
		AddHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		MyBase.PartialInit()
	End Sub

	' Token: 0x1700002F RID: 47
	' (get) Token: 0x06000131 RID: 305 RVA: 0x000573CA File Offset: 0x000557CA
	Public Overrides ReadOnly Property CurrentLevel As Levels
		Get
			Return Levels.ChaliceTutorial
		End Get
	End Property

	' Token: 0x17000030 RID: 48
	' (get) Token: 0x06000132 RID: 306 RVA: 0x000573D1 File Offset: 0x000557D1
	Public Overrides ReadOnly Property CurrentScene As Scenes
		Get
			Return Scenes.scene_level_chalice_tutorial
		End Get
	End Property

	' Token: 0x17000031 RID: 49
	' (get) Token: 0x06000133 RID: 307 RVA: 0x000573D5 File Offset: 0x000557D5
	Public Overrides ReadOnly Property BossPortrait As Sprite
		Get
			Return Me._bossPortrait
		End Get
	End Property

	' Token: 0x17000032 RID: 50
	' (get) Token: 0x06000134 RID: 308 RVA: 0x000573DD File Offset: 0x000557DD
	Public Overrides ReadOnly Property BossQuote As String
		Get
			Return Me._bossQuote
		End Get
	End Property

	' Token: 0x06000135 RID: 309 RVA: 0x000573E8 File Offset: 0x000557E8
	Protected Overrides Sub Start()
		MyBase.Start()
		For Each abstractPlayerController As AbstractPlayerController In PlayerManager.GetAllPlayers()
			If abstractPlayerController IsNot Nothing Then
				For Each transform As Transform In abstractPlayerController.GetComponentsInChildren(Of Transform)()
					transform.gameObject.layer = 31
				Next
			End If
		Next
		MyBase.StartCoroutine(Me.parryables_cr())
	End Sub

	' Token: 0x06000136 RID: 310 RVA: 0x0005748C File Offset: 0x0005588C
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.WORKAROUND_NullifyFields()
	End Sub

	' Token: 0x06000137 RID: 311 RVA: 0x0005749A File Offset: 0x0005589A
	Protected Overrides Sub OnLevelStart()
		MyBase.StartCoroutine(Me.intro_cr())
		MyBase.StartCoroutine(Me.chalicetutorialPattern_cr())
	End Sub

	' Token: 0x06000138 RID: 312 RVA: 0x000574B8 File Offset: 0x000558B8
	Private Iterator Function intro_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		Me.backgroundAnimator.Play("Zoom")
		Return
	End Function

	' Token: 0x06000139 RID: 313 RVA: 0x000574D4 File Offset: 0x000558D4
	Private Iterator Function parryables_cr() As IEnumerator
		While True
			For j As Integer = 0 To Me.parrybles.Length - 1
				Me.parrybles(j).Deactivated()
			Next
			While Not Me.backgroundAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle")
				Dim es As Effect() = Global.UnityEngine.[Object].FindObjectsOfType(Of Effect)()
				For Each effect As Effect In es
					effect.gameObject.layer = 31
					If effect.transform.childCount > 0 Then
						For Each transform As Transform In effect.transform.GetChildTransforms()
							transform.gameObject.layer = 31
						Next
					End If
				Next
				Dim ps As AbstractProjectile() = Global.UnityEngine.[Object].FindObjectsOfType(Of AbstractProjectile)()
				For Each abstractProjectile As AbstractProjectile In ps
					abstractProjectile.gameObject.layer = 31
					If abstractProjectile.transform.childCount > 0 Then
						For Each transform2 As Transform In abstractProjectile.transform.GetChildTransforms()
							transform2.gameObject.layer = 31
						Next
					End If
				Next
				PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, False, False)
				Yield Nothing
			End While
			PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, True, True)
			For i As Integer = 0 To Me.parrybles.Length - 1
				Me.parrybles(i).Activated()
				While Not Me.parrybles(i).isDeactivated
					If Me.resetParryables Then
						Exit While
					End If
					Yield Nothing
				End While
				If Me.resetParryables Then
					Exit For
				End If
			Next
			Me.resetParryables = False
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600013A RID: 314 RVA: 0x000574F0 File Offset: 0x000558F0
	Public Sub [Exit]()
		Dim abstractPlayerController As AbstractPlayerController = PlayerManager.GetPlayer(PlayerId.PlayerOne)
		Me.playerExitEffects(0).gameObject.SetActive(True)
		Me.playerExitEffects(0).transform.position = abstractPlayerController.transform.position
		abstractPlayerController.gameObject.SetActive(False)
		Me.playerExitEffects(0).animator.SetTrigger("OnStartTutorial")
		abstractPlayerController = PlayerManager.GetPlayer(PlayerId.PlayerTwo)
		If abstractPlayerController IsNot Nothing Then
			Me.playerExitEffects(1).gameObject.SetActive(True)
			Me.playerExitEffects(1).transform.position = abstractPlayerController.transform.position
			abstractPlayerController.gameObject.SetActive(False)
			Me.playerExitEffects(1).animator.SetTrigger("OnStartTutorial")
		End If
	End Sub

	' Token: 0x0600013B RID: 315 RVA: 0x000575C0 File Offset: 0x000559C0
	Private Iterator Function chalicetutorialPattern_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		While True
			Yield MyBase.StartCoroutine(Me.nextPattern_cr())
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600013C RID: 316 RVA: 0x000575DC File Offset: 0x000559DC
	Private Iterator Function nextPattern_cr() As IEnumerator
		Dim p As LevelProperties.ChaliceTutorial.Pattern = Me.properties.CurrentState.NextPattern
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		Return
	End Function

	' Token: 0x0600013D RID: 317 RVA: 0x000575F7 File Offset: 0x000559F7
	Private Sub WORKAROUND_NullifyFields()
		Me._bossPortrait = Nothing
		Me._bossQuote = Nothing
		Me.backgroundAnimator = Nothing
		Me.parrybles = Nothing
		Me.playerExitEffects = Nothing
	End Sub

	' Token: 0x04000291 RID: 657
	Private properties As LevelProperties.ChaliceTutorial

	' Token: 0x04000292 RID: 658
	<Header("Boss Info")>
	<SerializeField()>
	Private _bossPortrait As Sprite

	' Token: 0x04000293 RID: 659
	<SerializeField()>
	<Multiline()>
	Private _bossQuote As String

	' Token: 0x04000294 RID: 660
	<SerializeField()>
	Private backgroundAnimator As Animator

	' Token: 0x04000295 RID: 661
	<SerializeField()>
	Private parrybles As ChaliceTutorialLevelParryable()

	' Token: 0x04000296 RID: 662
	Private finishedPuzzle As Boolean

	' Token: 0x04000297 RID: 663
	Public resetParryables As Boolean

	' Token: 0x04000298 RID: 664
	<SerializeField()>
	Private playerExitEffects As PlayerDeathEffect()
End Class
