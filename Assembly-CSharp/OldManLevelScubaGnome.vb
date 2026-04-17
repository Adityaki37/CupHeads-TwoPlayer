Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200070D RID: 1805
Public Class OldManLevelScubaGnome
	Inherits AbstractProjectile

	' Token: 0x060026F8 RID: 9976 RVA: 0x0016D2DC File Offset: 0x0016B6DC
	Public Overridable Function Init(pos As Vector3, player As AbstractPlayerController, isTypeA As Boolean, onLeft As Boolean, dartParryable As Boolean, properties As LevelProperties.OldMan.ScubaGnomes, leader As OldManLevelGnomeLeader) As OldManLevelScubaGnome
		MyBase.ResetLifetime()
		MyBase.ResetDistance()
		MyBase.transform.position = pos
		MyBase.transform.SetScale(New Single?(CSng(If((Not onLeft), 1, (-1)))), Nothing, Nothing)
		Me.properties = properties
		Me.player = player
		Me.hp = properties.hp
		Me.isTypeA = isTypeA
		Me.onLeft = onLeft
		Me.leader = leader
		Me.dartParryable = dartParryable
		MyBase.animator.SetBool("IsGreen", Rand.Bool())
		MyBase.animator.Play("Start")
		MyBase.StartCoroutine(Me.move_cr())
		Return Me
	End Function

	' Token: 0x060026F9 RID: 9977 RVA: 0x0016D3A0 File Offset: 0x0016B7A0
	Private Sub OnEnable()
		AddHandler Level.Current.OnLevelEndEvent, AddressOf Me.Dead
	End Sub

	' Token: 0x060026FA RID: 9978 RVA: 0x0016D3B8 File Offset: 0x0016B7B8
	Private Sub OnDisable()
		If Level.Current IsNot Nothing Then
			RemoveHandler Level.Current.OnLevelEndEvent, AddressOf Me.Dead
		End If
	End Sub

	' Token: 0x060026FB RID: 9979 RVA: 0x0016D3E0 File Offset: 0x0016B7E0
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
	End Sub

	' Token: 0x060026FC RID: 9980 RVA: 0x0016D40B File Offset: 0x0016B80B
	Protected Overrides Sub OnDestroy()
		RemoveHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		MyBase.OnDestroy()
		Me.WORKAROUND_NullifyFields()
	End Sub

	' Token: 0x060026FD RID: 9981 RVA: 0x0016D430 File Offset: 0x0016B830
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x060026FE RID: 9982 RVA: 0x0016D44E File Offset: 0x0016B84E
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.hp -= info.damage
		If Me.hp <= 0F Then
			Level.Current.RegisterMinionKilled()
			Me.Dead()
		End If
	End Sub

	' Token: 0x060026FF RID: 9983 RVA: 0x0016D484 File Offset: 0x0016B884
	Private Iterator Function move_cr() As IEnumerator
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		Dim t As Single = 0F
		Dim start As Single = MyBase.transform.position.y
		Dim shotBullet As Boolean = False
		Dim toTop As Boolean = False
		Dim splashed As Boolean = False
		While t < Me.properties.scubaMoveTime
			Dim val As Single = t / Me.properties.scubaMoveTime
			MyBase.transform.SetPosition(Nothing, New Single?(start + Mathf.Sin(val * 3.1415927F / 2F) * Me.properties.jumpHeight), Nothing)
			t += CupheadTime.FixedDelta
			If Not splashed AndAlso MyBase.transform.position.y > Me.leader.splashHandler.transform.position.y Then
				Me.leader.splashHandler.SplashIn(MyBase.transform.position.x + 35F * MyBase.transform.localScale.x)
				splashed = True
				Me.SFX_JumpOut()
				Me.SFX_Vocal()
			End If
			Me.underwaterSprite.color = New Color(1F, 1F, 1F, (1F - Mathf.InverseLerp(Me.leader.splashHandler.transform.position.y + -50F, Me.leader.splashHandler.transform.position.y + -50F - 140F, MyBase.transform.position.y)) * 0.5F)
			If Not toTop AndAlso t >= 0.8F Then
				MyBase.animator.SetTrigger("ToTop")
				toTop = True
			End If
			Yield wait
		End While
		splashed = False
		t = 0F
		While t < Me.properties.scubaMoveTime
			Dim val As Single = t / Me.properties.scubaMoveTime
			MyBase.transform.SetPosition(Nothing, New Single?(start + Mathf.Sin((val + 1F) * 3.1415927F / 2F) * Me.properties.jumpHeight), Nothing)
			t += CupheadTime.Delta
			If Not splashed AndAlso MyBase.transform.position.y < Me.leader.splashHandler.transform.position.y - 75F Then
				Me.leader.splashHandler.SplashIn(MyBase.transform.position.x + 35F * MyBase.transform.localScale.x)
				splashed = True
				Me.SFX_DiveDown()
			End If
			Me.underwaterSprite.color = New Color(1F, 1F, 1F, (1F - Mathf.InverseLerp(Me.leader.splashHandler.transform.position.y + -50F, Me.leader.splashHandler.transform.position.y + -50F - 140F, MyBase.transform.position.y)) * 0.5F)
			If Not toTop AndAlso t >= 0.8F Then
				MyBase.animator.SetTrigger("ToTop")
				toTop = True
			End If
			Dim dist As Single = Me.shootRoot.position.y - (Me.player.center.y + Me.properties.shootDistOffset)
			If Not shotBullet AndAlso (dist < 10F OrElse Me.shootRoot.position.y < 0F) Then
				MyBase.animator.SetTrigger("Shoot")
				shotBullet = True
			End If
			Yield wait
		End While
		Me.Recycle()
		Yield Nothing
		Return
	End Function

	' Token: 0x06002700 RID: 9984 RVA: 0x0016D4A0 File Offset: 0x0016B8A0
	Private Sub Shoot()
		Dim num As Single = If((MyBase.transform.position.x >= 0F), 180F, 0F)
		Dim num2 As Single = If((Not Me.isTypeA), Me.properties.shotSpeedB, Me.properties.shotSpeedA)
		Dim basicProjectile As BasicProjectile = Me.projectile.Create(Me.shootRoot.position, num, num2)
		basicProjectile.SetParryable(Me.dartParryable)
		If Me.dartParryable Then
			basicProjectile.GetComponent(Of Animator)().Play("Pink")
		End If
		basicProjectile.GetComponent(Of SpriteRenderer)().flipY = MyBase.transform.position.x > 0F
		Me.SFX_ShootDart()
	End Sub

	' Token: 0x06002701 RID: 9985 RVA: 0x0016D574 File Offset: 0x0016B974
	Private Sub Dead()
		Me.deathPuff.Create(MyBase.transform.position)
		For i As Integer = 0 To Me.deathParts.Length - 1
			If i <> 0 OrElse Global.UnityEngine.Random.Range(0, 10) = 0 Then
				Dim spriteDeathParts As SpriteDeathParts = Me.deathParts(i).CreatePart(MyBase.transform.position)
				If i <> 0 Then
					spriteDeathParts.animator.Play(If((Not MyBase.animator.GetBool("IsGreen")), "_Blue", "_Teal"))
				End If
			End If
		Next
		AudioManager.Play("sfx_dlc_omm_gnome_death")
		Me.emitAudioFromObject.Add("sfx_dlc_omm_gnome_death")
		AudioManager.[Stop]("sfx_dlc_omm_p3_gnomediver_vocal")
		Me.Recycle()
	End Sub

	' Token: 0x06002702 RID: 9986 RVA: 0x0016D63C File Offset: 0x0016BA3C
	Private Sub SFX_DiveDown()
		AudioManager.Play("sfx_dlc_omm_p3_gnomediver_divedown")
		Me.emitAudioFromObject.Add("sfx_dlc_omm_p3_gnomediver_divedown")
	End Sub

	' Token: 0x06002703 RID: 9987 RVA: 0x0016D658 File Offset: 0x0016BA58
	Private Sub SFX_JumpOut()
		AudioManager.Play("sfx_dlc_omm_p3_gnomediver_jumpout")
		Me.emitAudioFromObject.Add("sfx_dlc_omm_p3_gnomediver_jumpout")
	End Sub

	' Token: 0x06002704 RID: 9988 RVA: 0x0016D674 File Offset: 0x0016BA74
	Private Sub SFX_ShootDart()
		AudioManager.Play("sfx_dlc_omm_p3_gnomediver_shootdart")
		Me.emitAudioFromObject.Add("sfx_dlc_omm_p3_gnomediver_shootdart")
	End Sub

	' Token: 0x06002705 RID: 9989 RVA: 0x0016D690 File Offset: 0x0016BA90
	Private Sub SFX_Vocal()
		AudioManager.Play("sfx_dlc_omm_p3_gnomediver_vocal")
		Me.emitAudioFromObject.Add("sfx_dlc_omm_p3_gnomediver_vocal")
	End Sub

	' Token: 0x06002706 RID: 9990 RVA: 0x0016D6AC File Offset: 0x0016BAAC
	Private Sub WORKAROUND_NullifyFields()
		Me.deathPuff = Nothing
		Me.deathParts = Nothing
		Me.projectile = Nothing
		Me.shootRoot = Nothing
		Me.player = Nothing
		Me.leader = Nothing
		Me.underwaterSprite = Nothing
	End Sub

	' Token: 0x04002FAB RID: 12203
	Private Const SPLASH_IN_TRIGGER_OFFSET As Single = 75F

	' Token: 0x04002FAC RID: 12204
	Private Const SPLASH_X_POSITION_OFFSET As Single = 35F

	' Token: 0x04002FAD RID: 12205
	Private Const UNDERWATER_FADE_OFFSET As Single = -50F

	' Token: 0x04002FAE RID: 12206
	Private Const LOWEST_SHOOT_POS As Single = 0F

	' Token: 0x04002FAF RID: 12207
	<Header("Death FX")>
	<SerializeField()>
	Private deathPuff As Effect

	' Token: 0x04002FB0 RID: 12208
	<SerializeField()>
	Private deathParts As SpriteDeathParts()

	' Token: 0x04002FB1 RID: 12209
	<Header("Prefabs")>
	<SerializeField()>
	Private projectile As BasicProjectile

	' Token: 0x04002FB2 RID: 12210
	<SerializeField()>
	Private shootRoot As Transform

	' Token: 0x04002FB3 RID: 12211
	Private Const Y_DIST_TO_SHOOT As Single = 10F

	' Token: 0x04002FB4 RID: 12212
	Private hp As Single

	' Token: 0x04002FB5 RID: 12213
	Private isTypeA As Boolean

	' Token: 0x04002FB6 RID: 12214
	Private onLeft As Boolean

	' Token: 0x04002FB7 RID: 12215
	Private properties As LevelProperties.OldMan.ScubaGnomes

	' Token: 0x04002FB8 RID: 12216
	Private player As AbstractPlayerController

	' Token: 0x04002FB9 RID: 12217
	Private leader As OldManLevelGnomeLeader

	' Token: 0x04002FBA RID: 12218
	Private damageReceiver As DamageReceiver

	' Token: 0x04002FBB RID: 12219
	Private dartParryable As Boolean

	' Token: 0x04002FBC RID: 12220
	<SerializeField()>
	Private underwaterSprite As SpriteRenderer
End Class
