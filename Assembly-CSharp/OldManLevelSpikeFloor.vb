Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000711 RID: 1809
Public Class OldManLevelSpikeFloor
	Inherits AbstractCollidableObject

	' Token: 0x06002740 RID: 10048 RVA: 0x0017073A File Offset: 0x0016EB3A
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageReceiver = MyBase.GetComponentInChildren(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
	End Sub

	' Token: 0x06002741 RID: 10049 RVA: 0x00170765 File Offset: 0x0016EB65
	Protected Overrides Sub OnDestroy()
		RemoveHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		MyBase.OnDestroy()
		Me.WORKAROUND_NullifyFields()
	End Sub

	' Token: 0x06002742 RID: 10050 RVA: 0x0017078C File Offset: 0x0016EB8C
	Public Sub SetID(i As Integer)
		Me.id = i
		MyBase.animator.SetInteger("Variant", i Mod 4)
		Me.gnomeRenderer.flipX = i Mod 8 > 3
		Me.tuftRenderer.flipX = Me.gnomeRenderer.flipX
	End Sub

	' Token: 0x06002743 RID: 10051 RVA: 0x001707DC File Offset: 0x0016EBDC
	Private Function AnimSuffix() As String
		Select Case Me.id Mod 4
			Case 0
				Return "A"
			Case 1
				Return "B"
			Case 2
				Return "C"
			Case Else
				Return "D"
		End Select
	End Function

	' Token: 0x06002744 RID: 10052 RVA: 0x00170820 File Offset: 0x0016EC20
	Private Function PopStartSuffix() As String
		Select Case Me.id Mod 4
			Case 0
				Return "A_C"
			Case 1
				Return "B"
			Case 2
				Return "A_C"
			Case Else
				Return "D"
		End Select
	End Function

	' Token: 0x06002745 RID: 10053 RVA: 0x00170864 File Offset: 0x0016EC64
	Private Function PopWarningSuffix() As String
		Select Case Me.id Mod 4
			Case 0
				Return "A_C"
			Case 1
				Return "B_D"
			Case 2
				Return "A_C"
			Case Else
				Return "B_D"
		End Select
	End Function

	' Token: 0x06002746 RID: 10054 RVA: 0x001708A8 File Offset: 0x0016ECA8
	Private Sub Update()
		If Me.spikeState = OldManLevelSpikeFloor.SpikeState.Gnomed Then
			Return
		End If
		If Me.spikeState <> OldManLevelSpikeFloor.SpikeState.Spiked AndAlso Not Me.deathTimeOut AndAlso Me.MinDistanceToPlayer(MyBase.transform.position) < 50F Then
			Me.ChangeState(OldManLevelSpikeFloor.SpikeState.Spiked)
		End If
	End Sub

	' Token: 0x06002747 RID: 10055 RVA: 0x001708FC File Offset: 0x0016ECFC
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		If Me.spikeState <> OldManLevelSpikeFloor.SpikeState.Gnomed Then
			Return
		End If
		Me.hp -= info.damage
		If Me.hp <= 0F Then
			Me.ChangeState(OldManLevelSpikeFloor.SpikeState.Idle)
			Level.Current.RegisterMinionKilled()
			Me.Dead()
		End If
	End Sub

	' Token: 0x06002748 RID: 10056 RVA: 0x00170950 File Offset: 0x0016ED50
	Public Sub SetProperties(properties As LevelProperties.OldMan)
		Me.spikeProperties = properties.CurrentState.spikes
		Me.gnomeProperties = properties.CurrentState.turret
		Me.gnomeShootPatternString = New PatternString(Me.gnomeProperties.attackString, True, True)
		Me.gnomePinkPatternString = New PatternString(Me.gnomeProperties.pinkShotString, True, True)
		Me.ChangeState(OldManLevelSpikeFloor.SpikeState.Idle)
	End Sub

	' Token: 0x06002749 RID: 10057 RVA: 0x001709B6 File Offset: 0x0016EDB6
	Public Sub SpawnGnome()
		Me.ChangeState(OldManLevelSpikeFloor.SpikeState.Gnomed)
	End Sub

	' Token: 0x0600274A RID: 10058 RVA: 0x001709C0 File Offset: 0x0016EDC0
	Private Sub ChangeState(state As OldManLevelSpikeFloor.SpikeState)
		If Me.[exit] Then
			Return
		End If
		If Me.spikeState = OldManLevelSpikeFloor.SpikeState.Idle OrElse state = OldManLevelSpikeFloor.SpikeState.Idle Then
			If Me.gnomeCR IsNot Nothing Then
				MyBase.StopCoroutine(Me.gnomeCR)
			End If
			If Me.spikeCR IsNot Nothing Then
				MyBase.StopCoroutine(Me.spikeCR)
			End If
			MyBase.animator.ResetTrigger("OnPimple")
			MyBase.animator.ResetTrigger("OnPop")
			MyBase.animator.ResetTrigger("OnWarning")
			MyBase.animator.SetBool("IsAttacking", False)
			Me.spikeState = state
			If state <> OldManLevelSpikeFloor.SpikeState.Gnomed Then
				If state <> OldManLevelSpikeFloor.SpikeState.Idle Then
					If state = OldManLevelSpikeFloor.SpikeState.Spiked Then
						Me.spikeCR = MyBase.StartCoroutine(Me.spike_up_cr())
					End If
				ElseIf Not MyBase.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle_" + Me.AnimSuffix()) Then
					MyBase.StartCoroutine(Me.restart_idle_cr())
				End If
			Else
				Me.gnomeCR = MyBase.StartCoroutine(Me.gnome_up_cr())
			End If
		End If
	End Sub

	' Token: 0x0600274B RID: 10059 RVA: 0x00170AE8 File Offset: 0x0016EEE8
	Private Iterator Function restart_idle_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, Global.UnityEngine.Random.Range(0.5F, 1F))
		MyBase.animator.Play("Restart_Idle_" + Me.PopStartSuffix())
		Me.deathTimeOut = False
		Return
	End Function

	' Token: 0x0600274C RID: 10060 RVA: 0x00170B04 File Offset: 0x0016EF04
	Private Function MinDistanceToPlayer(pos As Vector3) As Single
		Dim num As Single = Single.MaxValue
		Dim player As AbstractPlayerController = PlayerManager.GetPlayer(PlayerId.PlayerOne)
		Dim player2 As AbstractPlayerController = PlayerManager.GetPlayer(PlayerId.PlayerTwo)
		If player IsNot Nothing Then
			Dim num2 As Single = Vector3.SqrMagnitude(pos - player.transform.position)
			If num2 < num Then
				num = num2
			End If
		End If
		If player2 IsNot Nothing Then
			Dim num3 As Single = Vector3.SqrMagnitude(pos - player2.transform.position)
			If num3 < num Then
				num = num3
			End If
		End If
		Return Mathf.Sqrt(num)
	End Function

	' Token: 0x0600274D RID: 10061 RVA: 0x00170B88 File Offset: 0x0016EF88
	Private Iterator Function gnome_up_cr() As IEnumerator
		Me.hp = Me.gnomeProperties.hp
		MyBase.animator.SetTrigger("OnPimple")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Pop_Start_" + Me.PopStartSuffix(), False, True)
		Dim t As Single = 0F
		While t < Me.gnomeProperties.appearWarning AndAlso Not Me.[exit]
			t += CupheadTime.Delta
			Yield Nothing
		End While
		Yield Nothing
		t = 0F
		While t < Me.gnomeProperties.spawnSecondaryBuffer AndAlso Me.MinDistanceToPlayer(MyBase.transform.position) < Me.gnomeProperties.spawnDistanceCheck AndAlso Not Me.[exit]
			t += CupheadTime.Delta
			Yield Nothing
		End While
		MyBase.animator.SetTrigger("OnPop")
		Yield Nothing
		While Me.spikeState = OldManLevelSpikeFloor.SpikeState.Gnomed
			t = 0F
			While t < Me.gnomeProperties.shotDelay AndAlso Not Me.[exit]
				t += CupheadTime.Delta
				Yield Nothing
			End While
			MyBase.animator.SetBool("IsAttacking", True)
			Me.shootAngle = Me.gnomeShootPatternString.PopFloat()
			If Me.shootAngle <> 0F AndAlso ((Me.shootAngle <= 180F AndAlso Me.dontShootLeft) OrElse (Me.shootAngle > 180F AndAlso Me.dontShootRight)) Then
				Me.shootAngle = 360F - Me.shootAngle
			End If
			MyBase.animator.SetBool("Diagonal", Me.shootAngle <> 0F)
			t = 0F
			While t < Me.gnomeProperties.warningDuration AndAlso Not Me.[exit]
				t += CupheadTime.Delta
				Yield Nothing
			End While
			Yield Nothing
			If Not Me.[exit] Then
				MyBase.transform.localScale = New Vector3(CSng(If((Not((Me.shootAngle > 180F) Xor Me.gnomeRenderer.flipX)), 1, (-1))), 1F)
			End If
			MyBase.animator.SetBool("IsAttacking", False)
			Yield New WaitForEndOfFrame()
		End While
		Yield Nothing
		Return
	End Function

	' Token: 0x0600274E RID: 10062 RVA: 0x00170BA4 File Offset: 0x0016EFA4
	Private Function MinPlayerDistance() As Single
		Dim num As Single = Single.MaxValue
		For Each abstractPlayerController As AbstractPlayerController In PlayerManager.GetAllPlayers()
			Dim levelPlayerController As LevelPlayerController = CType(abstractPlayerController, LevelPlayerController)
			If Not(levelPlayerController Is Nothing) AndAlso levelPlayerController.transform.position.y <= MyBase.transform.position.y + 200F Then
				Dim num2 As Single = Mathf.Abs(MyBase.transform.position.x - levelPlayerController.transform.position.x)
				If num2 < num Then
					num = num2
				End If
			End If
		Next
		Return num
	End Function

	' Token: 0x0600274F RID: 10063 RVA: 0x00170C80 File Offset: 0x0016F080
	Private Iterator Function spike_up_cr() As IEnumerator
		If Not CType(Level.Current, OldManLevel).playedFirstSpikeSound Then
			Me.SFX_OMM_Gnome_SpikeRaiseFirst()
			CType(Level.Current, OldManLevel).playedFirstSpikeSound = True
		End If
		MyBase.transform.GetChild(0).gameObject.tag = "EnemyProjectile"
		MyBase.animator.SetTrigger("OnWarning")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Warning_Start_" + Me.AnimSuffix(), False, True)
		Dim t As Single = 0F
		While t < Me.spikeProperties.warningDuration AndAlso Not Me.[exit]
			t += CupheadTime.Delta
			Yield Nothing
		End While
		MyBase.animator.SetBool("IsAttacking", True)
		t = 0F
		While t < Me.spikeProperties.attackDuration AndAlso Not Me.[exit]
			t += CupheadTime.Delta
			Yield Nothing
		End While
		While Me.MinPlayerDistance() < 75F AndAlso Not Me.[exit]
			Yield Nothing
		End While
		Yield Nothing
		MyBase.animator.SetBool("IsAttacking", False)
		Yield MyBase.animator.WaitForAnimationToStart(Me, "Idle_" + Me.AnimSuffix(), False)
		Me.ChangeState(OldManLevelSpikeFloor.SpikeState.Idle)
		MyBase.transform.GetChild(0).gameObject.tag = "Enemy"
		Yield Nothing
		Return
	End Function

	' Token: 0x06002750 RID: 10064 RVA: 0x00170C9C File Offset: 0x0016F09C
	Public Sub Dead()
		Dim vector As Vector3 = New Vector3(Me.shootRoot.position.x, Me.shootRoot.position.y - 100F)
		Me.deathPuff.Create(vector)
		MyBase.animator.Play("None")
		For i As Integer = 0 To Me.deathParts.Length - 1
			If i <> 0 OrElse Global.UnityEngine.Random.Range(0, 10) = 0 Then
				Me.deathParts(i).CreatePart(vector)
			End If
		Next
		Me.deathTimeOut = True
		AudioManager.Play("sfx_dlc_omm_gnome_death")
		Me.emitAudioFromObject.Add("sfx_dlc_omm_gnome_death")
	End Sub

	' Token: 0x06002751 RID: 10065 RVA: 0x00170D55 File Offset: 0x0016F155
	Public Sub [Exit]()
		MyBase.StartCoroutine(Me.exit_cr())
	End Sub

	' Token: 0x06002752 RID: 10066 RVA: 0x00170D64 File Offset: 0x0016F164
	Private Iterator Function exit_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, Global.UnityEngine.Random.Range(0F, 1F))
		Me.[exit] = True
		MyBase.animator.SetBool("Dead", True)
		Yield MyBase.animator.WaitForAnimationToStart(Me, "None", False)
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Return
	End Function

	' Token: 0x06002753 RID: 10067 RVA: 0x00170D80 File Offset: 0x0016F180
	Private Sub AniEvent_ShootProjectile()
		Dim basicProjectile As BasicProjectile = If((Me.gnomePinkPatternString.PopLetter() <> "P"c), Me.gnomeProjectile, Me.gnomePinkProjectile)
		If Me.shootAngle = 0F Then
			basicProjectile.Create(Me.shootRoot.position, Me.shootAngle, Me.gnomeProperties.shotSpeed)
			Me.shootFXRenderer.transform.eulerAngles = Vector3.zero
			Me.shootFXRenderer.transform.localPosition = Vector3.up * 18F
		Else
			basicProjectile.Create(Me.shootRoot.position + Vector3.right * 40F * Mathf.Sign(Me.shootAngle - 180F), Me.shootAngle, Me.gnomeProperties.shotSpeed)
			Me.shootFXRenderer.transform.eulerAngles = New Vector3(0F, 0F, 40F * Mathf.Sign(Me.shootAngle) * CSng(If((Me.shootAngle <= 180F), 1, (-1))))
			Me.shootFXRenderer.transform.localPosition = New Vector3(30.5F * CSng(If((Not Me.gnomeRenderer.flipX), 1, (-1))), 33F)
		End If
		AudioManager.Play("sfx_dlc_omm_gnome_shoot_projectile")
		Me.emitAudioFromObject.Add("sfx_dlc_omm_gnome_shoot_projectile")
	End Sub

	' Token: 0x06002754 RID: 10068 RVA: 0x00170F0F File Offset: 0x0016F30F
	Private Sub SFX_OMM_Gnome_SpikeRaiseFirst()
		AudioManager.Play("sfx_dlc_omm_gnome_spike_raisefirst")
		Me.emitAudioFromObject.Add("sfx_dlc_omm_gnome_spike_raisefirst")
	End Sub

	' Token: 0x06002755 RID: 10069 RVA: 0x00170F2B File Offset: 0x0016F32B
	Private Sub AnimationEvent_SFX_OMM_Gnome_SpikeRaise()
		AudioManager.Play("sfx_dlc_omm_gnome_spike_raise")
		Me.emitAudioFromObject.Add("sfx_dlc_omm_gnome_spike_raise")
	End Sub

	' Token: 0x06002756 RID: 10070 RVA: 0x00170F47 File Offset: 0x0016F347
	Private Sub AnimationEvent_SFX_OMM_Gnome_SpikeRetract()
		AudioManager.Play("sfx_dlc_omm_gnome_spike_retract")
		Me.emitAudioFromObject.Add("sfx_dlc_omm_gnome_spike_retract")
	End Sub

	' Token: 0x06002757 RID: 10071 RVA: 0x00170F63 File Offset: 0x0016F363
	Private Sub AnimationEvent_SFX_OMM_Gnome_BeardAnticipation()
		AudioManager.Play("sfx_dlc_omm_gnome_beard_anticipation")
		Me.emitAudioFromObject.Add("sfx_dlc_omm_gnome_beard_anticipation")
	End Sub

	' Token: 0x06002758 RID: 10072 RVA: 0x00170F7F File Offset: 0x0016F37F
	Private Sub AnimationEvent_SFX_OMM_Gnome_BeardPopup()
		AudioManager.Play("sfx_dlc_omm_gnome_beard_popup")
		Me.emitAudioFromObject.Add("sfx_dlc_omm_gnome_beard_popup")
	End Sub

	' Token: 0x06002759 RID: 10073 RVA: 0x00170F9C File Offset: 0x0016F39C
	Private Sub WORKAROUND_NullifyFields()
		Me.deathPuff = Nothing
		Me.deathParts = Nothing
		Me.shootRoot = Nothing
		Me.gnomeProjectile = Nothing
		Me.gnomePinkProjectile = Nothing
		Me.gnomeShootPatternString = Nothing
		Me.gnomePinkPatternString = Nothing
		Me.spikeCR = Nothing
		Me.gnomeCR = Nothing
		Me.gnomeRenderer = Nothing
		Me.tuftRenderer = Nothing
		Me.shootFXRenderer = Nothing
	End Sub

	' Token: 0x04002FFC RID: 12284
	Private Const SPIKE_TRIGGER_RANGE As Single = 50F

	' Token: 0x04002FFD RID: 12285
	Private Const MIN_DISTANCE_TO_STAY_SPIKED As Single = 75F

	' Token: 0x04002FFE RID: 12286
	<Header("Death FX")>
	<SerializeField()>
	Private deathPuff As Effect

	' Token: 0x04002FFF RID: 12287
	<SerializeField()>
	Private deathParts As SpriteDeathParts()

	' Token: 0x04003000 RID: 12288
	<Header("Prefabs")>
	<SerializeField()>
	Private shootRoot As Transform

	' Token: 0x04003001 RID: 12289
	<SerializeField()>
	Private gnomeProjectile As BasicProjectile

	' Token: 0x04003002 RID: 12290
	<SerializeField()>
	Private gnomePinkProjectile As BasicProjectile

	' Token: 0x04003003 RID: 12291
	Public spikeState As OldManLevelSpikeFloor.SpikeState

	' Token: 0x04003004 RID: 12292
	Private spikeProperties As LevelProperties.OldMan.Spikes

	' Token: 0x04003005 RID: 12293
	Private gnomeProperties As LevelProperties.OldMan.Turret

	' Token: 0x04003006 RID: 12294
	Private gnomeShootPatternString As PatternString

	' Token: 0x04003007 RID: 12295
	Private gnomePinkPatternString As PatternString

	' Token: 0x04003008 RID: 12296
	Private hp As Single

	' Token: 0x04003009 RID: 12297
	Private damageReceiver As DamageReceiver

	' Token: 0x0400300A RID: 12298
	Private shootAngle As Single

	' Token: 0x0400300B RID: 12299
	Private spikeCR As Coroutine

	' Token: 0x0400300C RID: 12300
	Private gnomeCR As Coroutine

	' Token: 0x0400300D RID: 12301
	<SerializeField()>
	Private dontShootLeft As Boolean

	' Token: 0x0400300E RID: 12302
	<SerializeField()>
	Private dontShootRight As Boolean

	' Token: 0x0400300F RID: 12303
	<SerializeField()>
	Private gnomeRenderer As SpriteRenderer

	' Token: 0x04003010 RID: 12304
	<SerializeField()>
	Private tuftRenderer As SpriteRenderer

	' Token: 0x04003011 RID: 12305
	<SerializeField()>
	Private shootFXRenderer As SpriteRenderer

	' Token: 0x04003012 RID: 12306
	Private id As Integer

	' Token: 0x04003013 RID: 12307
	Private [exit] As Boolean

	' Token: 0x04003014 RID: 12308
	Private deathTimeOut As Boolean

	' Token: 0x02000712 RID: 1810
	Public Enum SpikeState
		' Token: 0x04003016 RID: 12310
		Idle
		' Token: 0x04003017 RID: 12311
		Spiked
		' Token: 0x04003018 RID: 12312
		Gnomed
	End Enum
End Class
