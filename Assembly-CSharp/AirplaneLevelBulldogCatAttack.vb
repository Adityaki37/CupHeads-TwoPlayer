Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020004B3 RID: 1203
Public Class AirplaneLevelBulldogCatAttack
	Inherits LevelProperties.Airplane.Entity

	' Token: 0x1700030D RID: 781
	' (get) Token: 0x060013A5 RID: 5029 RVA: 0x000AE20C File Offset: 0x000AC60C
	' (set) Token: 0x060013A6 RID: 5030 RVA: 0x000AE214 File Offset: 0x000AC614
	Public Property isAttacking As Boolean

	' Token: 0x060013A7 RID: 5031 RVA: 0x000AE21D File Offset: 0x000AC61D
	Private Sub Start()
		Me.damageDealer = DamageDealer.NewEnemy()
	End Sub

	' Token: 0x060013A8 RID: 5032 RVA: 0x000AE22A File Offset: 0x000AC62A
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
	End Sub

	' Token: 0x060013A9 RID: 5033 RVA: 0x000AE232 File Offset: 0x000AC632
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x060013AA RID: 5034 RVA: 0x000AE24A File Offset: 0x000AC64A
	Public Overrides Sub LevelInit(properties As LevelProperties.Airplane)
		MyBase.LevelInit(properties)
	End Sub

	' Token: 0x060013AB RID: 5035 RVA: 0x000AE253 File Offset: 0x000AC653
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x060013AC RID: 5036 RVA: 0x000AE274 File Offset: 0x000AC674
	Public Sub StartCat(pos As Vector2)
		Me.count = Global.UnityEngine.Random.Range(0, 3)
		Me.isAttacking = True
		MyBase.transform.localScale = New Vector3(Mathf.Sign(pos.x), 1F)
		MyBase.transform.position = pos
		MyBase.animator.SetBool("Exit", False)
		MyBase.StartCoroutine(Me.cat_cr())
	End Sub

	' Token: 0x060013AD RID: 5037 RVA: 0x000AE2E8 File Offset: 0x000AC6E8
	Private Iterator Function cat_cr() As IEnumerator
		Dim p As LevelProperties.Airplane.Triple = MyBase.properties.CurrentState.triple
		MyBase.animator.Play("Intro")
		AudioManager.FadeSFXVolume("sfx_dlc_dogfight_p1_catattack_hover", 1E-05F, 1E-05F)
		AudioManager.Play("sfx_dlc_dogfight_p1_bulldog_whistle_in")
		Me.emitAudioFromObject.Add("sfx_dlc_dogfight_p1_bulldog_whistle_in")
		Yield MyBase.animator.WaitForAnimationToStart(Me, "IntroLoop", False)
		Yield CupheadTime.WaitForSeconds(Me, p.initialDelay)
		MyBase.animator.SetTrigger("Continue")
		AudioManager.PlayLoop("sfx_dlc_dogfight_p1_catattack_hover")
		Me.emitAudioFromObject.Add("sfx_dlc_dogfight_p1_catattack_hover")
		AudioManager.FadeSFXVolume("sfx_dlc_dogfight_p1_catattack_hover", 0.15F, 0.5F)
		AudioManager.Play("sfx_dlc_dogfight_p1_catattack_enter")
		Me.emitAudioFromObject.Add("sfx_dlc_dogfight_p1_catattack_enter")
		Yield MyBase.animator.WaitForAnimationToStart(Me, "Idle", False)
		Yield CupheadTime.WaitForSeconds(Me, p.shootWarning)
		Me.SFX_DOGFIGHT_Cat_Shoot()
		MyBase.animator.Play("ShootA")
		MyBase.animator.Update(0F)
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "ShootA", False, True)
		Yield CupheadTime.WaitForSeconds(Me, p.delayAfterFirst.RandomFloat())
		Me.SFX_DOGFIGHT_Cat_Shoot()
		MyBase.animator.Play("ShootB")
		MyBase.animator.Update(0F)
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "ShootB", False, True)
		Yield CupheadTime.WaitForSeconds(Me, p.delayAfterSecond.RandomFloat())
		Me.SFX_DOGFIGHT_Cat_Shoot()
		MyBase.animator.Play("ShootA")
		MyBase.animator.Update(0F)
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "ShootA", False, True)
		Yield CupheadTime.WaitForSeconds(Me, p.shootRecovery)
		MyBase.animator.SetBool("Exit", True)
		AudioManager.FadeSFXVolume("sfx_dlc_dogfight_p1_catattack_hover", 0F, 0.5F)
		AudioManager.Play("sfx_dlc_dogfight_p1_catattack_leave")
		Me.emitAudioFromObject.Add("sfx_dlc_dogfight_p1_catattack_leave")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Exit", False, True)
		Yield CupheadTime.WaitForSeconds(Me, p.returnDelay)
		Me.isAttacking = False
		Return
	End Function

	' Token: 0x060013AE RID: 5038 RVA: 0x000AE303 File Offset: 0x000AC703
	Public Sub EarlyExit()
		Me.StopAllCoroutines()
		MyBase.StartCoroutine(Me.early_exit_cr())
	End Sub

	' Token: 0x060013AF RID: 5039 RVA: 0x000AE318 File Offset: 0x000AC718
	Private Iterator Function early_exit_cr() As IEnumerator
		MyBase.animator.SetBool("Exit", True)
		Yield MyBase.animator.WaitForAnimationToStart(Me, "None", False)
		Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.triple.returnDelay)
		Me.isAttacking = False
		Return
	End Function

	' Token: 0x060013B0 RID: 5040 RVA: 0x000AE334 File Offset: 0x000AC734
	Private Sub AniEvent_Shoot()
		Dim triple As LevelProperties.Airplane.Triple = MyBase.properties.CurrentState.triple
		Dim num As Single = triple.attackAngleRange.RandomFloat()
		Dim num2 As Single = If((MyBase.transform.localScale.x >= 0F), 180F, 0F)
		Dim basicProjectile As BasicProjectile = Me.projectile.Create(Me.root.position, num2 + num, triple.bulletSpeed)
		basicProjectile.transform.localScale = New Vector3(1F, CSng(If((num2 <= 0F), 1, (-1))))
		Dim component As Animator = basicProjectile.GetComponent(Of Animator)()
		component.Play((Me.count Mod 3).ToString(), 0, Global.UnityEngine.Random.Range(0.375F, 0.75F))
		component.Update(0F)
		Me.count += 1
		MyBase.animator.Play(Global.UnityEngine.Random.Range(0, 4).ToString(), 1, 0F)
		MyBase.animator.Update(0F)
	End Sub

	' Token: 0x060013B1 RID: 5041 RVA: 0x000AE462 File Offset: 0x000AC862
	Private Sub OnDisable()
		MyBase.GetComponent(Of HitFlash)().StopAllCoroutinesWithoutSettingScale()
		MyBase.GetComponent(Of SpriteRenderer)().color = Color.black
	End Sub

	' Token: 0x060013B2 RID: 5042 RVA: 0x000AE47F File Offset: 0x000AC87F
	Private Sub AniEvent_IntroFX()
		MyBase.animator.Play("IntroFX", 1)
		MyBase.animator.Update(0F)
	End Sub

	' Token: 0x060013B3 RID: 5043 RVA: 0x000AE4A2 File Offset: 0x000AC8A2
	Private Sub AniEvent_FlashA()
		MyBase.animator.Play("FlashA", 2)
		MyBase.animator.Update(0F)
	End Sub

	' Token: 0x060013B4 RID: 5044 RVA: 0x000AE4C5 File Offset: 0x000AC8C5
	Private Sub AniEvent_FlashB()
		MyBase.animator.Play("FlashB", 2)
		MyBase.animator.Update(0F)
	End Sub

	' Token: 0x060013B5 RID: 5045 RVA: 0x000AE4E8 File Offset: 0x000AC8E8
	Private Sub AniEvent_EmberA()
		MyBase.animator.Play("EmberA", 3)
	End Sub

	' Token: 0x060013B6 RID: 5046 RVA: 0x000AE4FB File Offset: 0x000AC8FB
	Private Sub AniEvent_EmberB()
		MyBase.animator.Play("EmberB", 3)
	End Sub

	' Token: 0x060013B7 RID: 5047 RVA: 0x000AE50E File Offset: 0x000AC90E
	Private Sub SFX_DOGFIGHT_Cat_Shoot()
		AudioManager.Play("sfx_dlc_dogfight_catgun_shoot")
		Me.emitAudioFromObject.Add("sfx_dlc_dogfight_catgun_shoot")
	End Sub

	' Token: 0x060013B8 RID: 5048 RVA: 0x000AE52A File Offset: 0x000AC92A
	Private Sub SFX_DOGFIGHT_Cat_StartMeow()
		AudioManager.Play("sfx_dlc_dogfight_p1_catgunmeow")
		Me.emitAudioFromObject.Add("sfx_dlc_dogfight_p1_catgunmeow")
	End Sub

	' Token: 0x04001CC8 RID: 7368
	<SerializeField()>
	Private main As AirplaneLevelBulldogPlane

	' Token: 0x04001CC9 RID: 7369
	<SerializeField()>
	Private projectile As BasicProjectile

	' Token: 0x04001CCA RID: 7370
	<SerializeField()>
	Private root As Transform

	' Token: 0x04001CCB RID: 7371
	Private damageDealer As DamageDealer

	' Token: 0x04001CCC RID: 7372
	Private count As Integer
End Class
