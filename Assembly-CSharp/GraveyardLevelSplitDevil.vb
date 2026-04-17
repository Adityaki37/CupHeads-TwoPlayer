Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020006C7 RID: 1735
Public Class GraveyardLevelSplitDevil
	Inherits LevelProperties.Graveyard.Entity

	' Token: 0x060024DB RID: 9435 RVA: 0x00159592 File Offset: 0x00157992
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x060024DC RID: 9436 RVA: 0x001595AA File Offset: 0x001579AA
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If Me.damageDealer IsNot Nothing AndAlso phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x060024DD RID: 9437 RVA: 0x001595D4 File Offset: 0x001579D4
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf MyBase.GetComponentInParent(Of GraveyardLevelSplitDevil)().OnDamageTaken
		If MyBase.transform.localScale.x > 0F Then
			Me.SetIsAngel(True)
			Me.id = 0
		End If
		Me.sideString = If((MyBase.transform.localScale.x >= 0F), "left", "right")
	End Sub

	' Token: 0x060024DE RID: 9438 RVA: 0x00159678 File Offset: 0x00157A78
	Public Overrides Sub LevelInit(properties As LevelProperties.Graveyard)
		MyBase.LevelInit(properties)
		Me.numProjectiles = New PatternString(properties.CurrentState.splitDevilProjectiles.numProjectiles(Me.id), True)
		Me.projectileAngleOffset = New PatternString(properties.CurrentState.splitDevilProjectiles.angleOffsetString, True)
		Me.projectilePinkString = New PatternString(properties.CurrentState.splitDevilProjectiles.pinkString, True)
		Me.level = TryCast(Level.Current, GraveyardLevel)
		MyBase.StartCoroutine(Me.fade_in_cr())
	End Sub

	' Token: 0x060024DF RID: 9439 RVA: 0x00159704 File Offset: 0x00157B04
	Private Iterator Function fade_in_cr() As IEnumerator
		Me.mainRend.color = New Color(0F, 0F, 0F, 0F)
		Me.haloRend.color = New Color(0F, 0F, 0F, 0F)
		Dim t As Single = 0F
		While t < 4F
			Me.mainRend.color = New Color(0F, 0F, 0F, t / 4F)
			Me.haloRend.color = New Color(0F, 0F, 0F, t / 4F)
			t += CupheadTime.Delta
			Yield New WaitForFixedUpdate()
		End While
		Me.mainRend.color = New Color(0F, 0F, 0F, 1F)
		Me.haloRend.color = New Color(0F, 0F, 0F, 1F)
		Return
	End Function

	' Token: 0x060024E0 RID: 9440 RVA: 0x00159720 File Offset: 0x00157B20
	Private Sub SetIsAngel(value As Boolean)
		If Me.isAngel <> value AndAlso Not value Then
			AudioManager.Play("sfx_dlc_graveyard_changedirectionbad")
			Me.emitAudioFromObject.Add("sfx_dlc_graveyard_changedirectionbad")
		End If
		MyBase.animator.SetBool("isAngel", value)
		Me.coll.enabled = Not value
		Me.isAngel = value
		AudioManager.FadeSFXVolume("sfx_dlc_graveyard_angelsing_" + Me.sideString, If((Not Me.isAngel), 1E-05F, 0.4F), 0.4F)
		AudioManager.FadeSFXVolume("sfx_dlc_graveyard_devilangryrage_" + Me.sideString, If(Me.isAngel, 1E-05F, 0.4F), 0.4F)
	End Sub

	' Token: 0x060024E1 RID: 9441 RVA: 0x001597E8 File Offset: 0x00157BE8
	Private Sub LateUpdate()
		If Me.dead Then
			Return
		End If
		Dim num As Integer = 0
		Dim num2 As Integer = 0
		For Each abstractPlayerController As AbstractPlayerController In PlayerManager.GetAllPlayers()
			Dim levelPlayerController As LevelPlayerController = CType(abstractPlayerController, LevelPlayerController)
			If levelPlayerController IsNot Nothing AndAlso Not levelPlayerController.IsDead Then
				num2 += 1
				num += CInt(Mathf.Sign(levelPlayerController.transform.localScale.x))
			End If
		Next
		If Mathf.Abs(num) = num2 Then
			If Me.isAngel <> (Mathf.Sign(CSng(num)) = Mathf.Sign(MyBase.transform.localScale.x)) AndAlso Me.headLooping Then
				Me.ResyncHead()
			End If
			Me.SetIsAngel(Mathf.Sign(CSng(num)) = Mathf.Sign(MyBase.transform.localScale.x))
		End If
		Me.headlessRend.enabled = Me.headRend.sprite IsNot Nothing
		Me.mainRend.enabled = Not Me.headlessRend.enabled
	End Sub

	' Token: 0x060024E2 RID: 9442 RVA: 0x00159930 File Offset: 0x00157D30
	Public Sub NextPattern()
		MyBase.StartCoroutine(If((Not Me.level.CheckForBeamAttack()), Me.projectile_cr(), Me.roar_cr()))
	End Sub

	' Token: 0x060024E3 RID: 9443 RVA: 0x0015995C File Offset: 0x00157D5C
	Private Iterator Function roar_cr() As IEnumerator
		Dim p As LevelProperties.Graveyard.SplitDevilBeam = MyBase.properties.CurrentState.splitDevilBeam
		MyBase.animator.SetBool("isSinging", True)
		Dim targetA As Integer = Animator.StringToHash(MyBase.animator.GetLayerName(0) + ".SingStartAngel")
		Dim targetB As Integer = Animator.StringToHash(MyBase.animator.GetLayerName(0) + ".SingStartDevil")
		While MyBase.animator.GetCurrentAnimatorStateInfo(0).fullPathHash <> targetA AndAlso MyBase.animator.GetCurrentAnimatorStateInfo(0).fullPathHash <> targetB
			Yield Nothing
		End While
		Me.beamPrefab.Create(New Vector3(Mathf.Sign(MyBase.transform.position.x) * CSng((Level.Current.Right - 50)), 80F), p.speed.RandomFloat() * -Mathf.Sign(MyBase.transform.position.x), p.warning, Me)
		Yield New WaitForSeconds(1F)
		MyBase.animator.SetBool("isSinging", False)
		Yield CupheadTime.WaitForSeconds(Me, p.hesitateAfterAttack.RandomFloat())
		Me.NextPattern()
		Return
	End Function

	' Token: 0x060024E4 RID: 9444 RVA: 0x00159978 File Offset: 0x00157D78
	Private Sub ResyncHead()
		Dim num As Single = MyBase.animator.GetCurrentAnimatorStateInfo(0).normalizedTime Mod 0.25F
		Dim num2 As Single = MyBase.animator.GetCurrentAnimatorStateInfo(1).normalizedTime - MyBase.animator.GetCurrentAnimatorStateInfo(1).normalizedTime Mod 0.25F
		If MyBase.animator.GetCurrentAnimatorStateInfo(0).fullPathHash = Animator.StringToHash(MyBase.animator.GetLayerName(0) + ".IdleAngel") Then
			MyBase.animator.Play("ShootLoopAngel", 1, num2 + num)
			MyBase.animator.Update(0F)
		ElseIf MyBase.animator.GetCurrentAnimatorStateInfo(0).fullPathHash = Animator.StringToHash(MyBase.animator.GetLayerName(0) + ".IdleDevil") Then
			MyBase.animator.Play("ShootLoopDevil", 1, num2 + num)
			MyBase.animator.Update(0F)
		End If
	End Sub

	' Token: 0x060024E5 RID: 9445 RVA: 0x00159A88 File Offset: 0x00157E88
	Private Iterator Function projectile_cr() As IEnumerator
		Me.triggerShoot = True
		While Me.triggerShoot
			Yield Nothing
		End While
		MyBase.animator.Play("Charge", 2, 0F)
		Yield CupheadTime.WaitForSeconds(Me, 0.16666667F)
		Me.headLooping = True
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Charge", 2, False, False)
		Dim p As LevelProperties.Graveyard.SplitDevilProjectiles = MyBase.properties.CurrentState.splitDevilProjectiles
		Dim player As AbstractPlayerController = PlayerManager.GetNext()
		Dim delayBetweenProjectiles As Single = p.delayBetweenProjectiles.RandomFloat()
		Dim p2 As LevelPlayerController = TryCast(PlayerManager.GetPlayer(PlayerId.PlayerOne), LevelPlayerController)
		Dim projectileCount As Integer = Me.numProjectiles.PopInt()
		For i As Integer = 0 To projectileCount - 1
			If player Is Nothing OrElse player.IsDead Then
				player = PlayerManager.GetNext()
			End If
			Dim rotation As Single = MathUtils.DirectionToAngle(player.center - Me.projectileRoot.transform.position)
			rotation += Me.projectileAngleOffset.PopFloat()
			Dim isPink As Boolean = Me.projectilePinkString.PopLetter() = "P"c
			If isPink Then
				Me.projectilePinkPrefab.Create(Me.projectileRoot.transform.position, rotation, p.projectileSpeed, Me)
			Else
				Me.projectilePrefab.Create(Me.projectileRoot.transform.position, rotation, p.projectileSpeed, Me)
			End If
			MyBase.animator.Play(If((Not Me.isAngel), If((Not Rand.Bool()), "FireB", "FireA"), "Light"), If((Not isPink), 3, 4), 0F)
			Me.shootFXRend(0).flipX = Me.isAngel AndAlso Rand.Bool()
			Me.shootFXRend(1).flipX = Me.isAngel AndAlso Rand.Bool()
			Me.shootFXRend(0).flipY = Me.isAngel AndAlso Rand.Bool()
			Me.shootFXRend(1).flipY = Me.isAngel AndAlso Rand.Bool()
			Me.SFX_SplitDevil_Shoot()
			If i < projectileCount - 1 Then
				Yield CupheadTime.WaitForSeconds(Me, Mathf.Clamp(delayBetweenProjectiles - 0.45833334F, 0F, Single.MaxValue))
				MyBase.animator.Play("Charge", 2, 0F)
				Yield MyBase.animator.WaitForAnimationToEnd(Me, "Charge", 2, False, True)
				Yield CupheadTime.WaitForSeconds(Me, 0.125F)
			End If
		Next
		Me.headLooping = False
		MyBase.animator.SetBool("isShooting", False)
		Yield CupheadTime.WaitForSeconds(Me, p.hesitateAfterAttack.RandomFloat())
		Me.NextPattern()
		Return
	End Function

	' Token: 0x060024E6 RID: 9446 RVA: 0x00159AA4 File Offset: 0x00157EA4
	Public Sub AniEvent_CanStartShoot()
		If Me.triggerShoot Then
			Dim flag As Boolean = MyBase.animator.GetCurrentAnimatorStateInfo(0).fullPathHash = Animator.StringToHash(MyBase.animator.GetLayerName(0) + ".IdleAngel")
			If flag <> Me.isAngel Then
				MyBase.animator.Play(If((Not flag), "ShootStartDevilToAngel", "ShootStartAngelToDevil"), 1, 0F)
			Else
				MyBase.animator.Play(If((Not Me.isAngel), "ShootStartDevil", "ShootStartAngel"), 1, 0F)
			End If
			MyBase.animator.SetBool("isShooting", True)
			Me.triggerShoot = False
		End If
	End Sub

	' Token: 0x060024E7 RID: 9447 RVA: 0x00159B68 File Offset: 0x00157F68
	Private Sub OnDisable()
		If Not MyBase.animator.GetBool("isShooting") Then
			Me.headlessRend.enabled = False
			Me.mainRend.enabled = True
		End If
	End Sub

	' Token: 0x060024E8 RID: 9448 RVA: 0x00159B97 File Offset: 0x00157F97
	Public Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		MyBase.properties.DealDamage(info.damage)
	End Sub

	' Token: 0x060024E9 RID: 9449 RVA: 0x00159BAC File Offset: 0x00157FAC
	Public Sub Die()
		Me.dead = True
		Me.headRend.enabled = False
		Me.headlessRend.enabled = False
		Me.mainRend.enabled = True
		Me.triggerShoot = False
		Me.StopAllCoroutines()
		MyBase.animator.Play(If((Not Me.isAngel), "DeathDevilLoop", "DeathAngelLoop"))
		MyBase.StartCoroutine(Me.death_cr())
	End Sub

	' Token: 0x060024EA RID: 9450 RVA: 0x00159C24 File Offset: 0x00158024
	Private Iterator Function death_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 2.5F)
		MyBase.animator.SetTrigger("DeathContinue")
		Yield CupheadTime.WaitForSeconds(Me, 0.25F)
		MyBase.GetComponent(Of LevelBossDeathExploder)().StopExplosions()
		Return
	End Function

	' Token: 0x060024EB RID: 9451 RVA: 0x00159C3F File Offset: 0x0015803F
	Private Sub ActivateBGSkellyMask()
		Me.bgSkellyMask.SetActive(True)
	End Sub

	' Token: 0x060024EC RID: 9452 RVA: 0x00159C50 File Offset: 0x00158050
	Private Sub SFXSingRoar()
		AudioManager.FadeSFXVolume("sfx_dlc_graveyard_angelsing_" + Me.sideString, If((Not Me.isAngel), 1E-05F, 0.4F), 0.01F)
		AudioManager.FadeSFXVolume("sfx_dlc_graveyard_devilangryrage_" + Me.sideString, If(Me.isAngel, 1E-05F, 0.4F), 0.01F)
		AudioManager.Play("sfx_dlc_graveyard_angelsing_" + Me.sideString)
		Me.emitAudioFromObject.Add("sfx_dlc_graveyard_angelsing_" + Me.sideString)
		AudioManager.Play("sfx_dlc_graveyard_devilangryrage_" + Me.sideString)
		Me.emitAudioFromObject.Add("sfx_dlc_graveyard_devilangryrage_" + Me.sideString)
	End Sub

	' Token: 0x060024ED RID: 9453 RVA: 0x00159D25 File Offset: 0x00158125
	Private Sub AnimationEvent_SFX_SplitDevil_AngelSing()
		Me.SFXSingRoar()
	End Sub

	' Token: 0x060024EE RID: 9454 RVA: 0x00159D2D File Offset: 0x0015812D
	Private Sub AnimationEvent_SFX_SplitDevil_DevilRage()
		Me.SFXSingRoar()
	End Sub

	' Token: 0x060024EF RID: 9455 RVA: 0x00159D38 File Offset: 0x00158138
	Private Sub SFX_SplitDevil_Shoot()
		AudioManager.Play(If((Not Me.isAngel), "sfx_dlc_graveyard_devil_shoot", "sfx_DLC_Graveyard_Angel_Shoot"))
		Me.emitAudioFromObject.Add(If((Not Me.isAngel), "sfx_dlc_graveyard_devil_shoot", "sfx_DLC_Graveyard_Angel_Shoot"))
	End Sub

	' Token: 0x04002D7B RID: 11643
	Private Const SING_ROAR_MAX_VOLUME As Single = 0.4F

	' Token: 0x04002D7C RID: 11644
	<SerializeField()>
	Private projectilePrefab As GraveyardLevelSplitDevilProjectile

	' Token: 0x04002D7D RID: 11645
	<SerializeField()>
	Private projectilePinkPrefab As GraveyardLevelSplitDevilProjectile

	' Token: 0x04002D7E RID: 11646
	<SerializeField()>
	Private beamPrefab As GraveyardLevelSplitDevilBeam

	' Token: 0x04002D7F RID: 11647
	Private damageDealer As DamageDealer

	' Token: 0x04002D80 RID: 11648
	Private damageReceiver As DamageReceiver

	' Token: 0x04002D81 RID: 11649
	<SerializeField()>
	Private bgSkellyMask As GameObject

	' Token: 0x04002D82 RID: 11650
	<SerializeField()>
	Private coll As Collider2D

	' Token: 0x04002D83 RID: 11651
	<SerializeField()>
	Private headRend As SpriteRenderer

	' Token: 0x04002D84 RID: 11652
	<SerializeField()>
	Private mainRend As SpriteRenderer

	' Token: 0x04002D85 RID: 11653
	<SerializeField()>
	Private headlessRend As SpriteRenderer

	' Token: 0x04002D86 RID: 11654
	<SerializeField()>
	Private haloRend As SpriteRenderer

	' Token: 0x04002D87 RID: 11655
	<SerializeField()>
	Private projectileRoot As Transform

	' Token: 0x04002D88 RID: 11656
	Private numProjectiles As PatternString

	' Token: 0x04002D89 RID: 11657
	Private projectileAngleOffset As PatternString

	' Token: 0x04002D8A RID: 11658
	Private triggerShoot As Boolean

	' Token: 0x04002D8B RID: 11659
	Public isAngel As Boolean

	' Token: 0x04002D8C RID: 11660
	Public dead As Boolean

	' Token: 0x04002D8D RID: 11661
	Private headLooping As Boolean

	' Token: 0x04002D8E RID: 11662
	<SerializeField()>
	Private shootFXRend As SpriteRenderer()

	' Token: 0x04002D8F RID: 11663
	Private id As Integer = 1

	' Token: 0x04002D90 RID: 11664
	Private level As GraveyardLevel

	' Token: 0x04002D91 RID: 11665
	Private projectilePinkString As PatternString

	' Token: 0x04002D92 RID: 11666
	Private sideString As String
End Class
